using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using servecoin.interfaces;

namespace servecoin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        public static double EvaluateExpression(string expression)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(expression))
                    throw new ArgumentException("Expression cannot be null or empty.");

                var dataTable = new DataTable();

                dataTable.Columns.Add("expression", typeof(string), expression);

                var result = dataTable.Compute(expression, null);

                return Convert.ToDouble(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error evaluating expression: {ex.Message}");
                throw;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            onload();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int deniedReason = -1;
            int[] numAlpha = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            string[] denyReasons = {
                "Incorrect value",
                "Accumulated is bigger or equals to goal. Delete row to finish goal.",
                "Name must be longer. (4-64 limit)",
                "Name must be shorter. (4-64 limit)",
                "Currency must be longer. (3-24 limit)",
                "Currency must be shorter. (3-24 limit)"
            };
            string deniedString = "";

            string finalErrorString = "";

            JArray table = ConvertTableToArray();

            foreach (JObject tableObj in table)
            {
                int index = table.IndexOf(tableObj);

                IGoal goal = tableObj.ToObject<Goal>();

                if (goal.accumulated != null)
                {
                    foreach (char c in goal.accumulated)
                    {
                        if (!char.IsDigit(c))
                        {
                            deniedReason = 0;
                            deniedString = $"{index}: accumulated";
                            break;
                        }
                    }
                }

                if (goal.goal != null)
                {
                    foreach (char c in goal.goal)
                    {
                        if (!char.IsDigit(c))
                        {
                            deniedReason = 0;
                            deniedString = $"{index}: goal";
                            break;
                        }
                    }
                }

                if (deniedReason != -1)
                    break;

                if (Int32.Parse(goal.goal) <= Int32.Parse(goal.accumulated))
                {
                    deniedReason = 1;
                    deniedString = $"{index}";
                }
                if (goal.name.Length < 4)
                {
                    deniedReason = 2;
                    deniedString = $"{index}: name";
                }
                if (goal.name.Length > 64)
                {
                    deniedReason = 3;
                    deniedString = $"{index}: name";
                }
                if (goal.currency.Length < 3)
                {
                    deniedReason = 4;
                    deniedString = $"{index}: currency";
                }
                if (goal.currency.Length > 24)
                {
                    deniedReason = 5;
                    deniedString = $"{index}: currency";
                }
            }

            if (deniedReason != -1)
            {
                finalErrorString = $"Error occurred: {denyReasons[deniedReason]} at {deniedString}";
                MessageBox.Show(finalErrorString, "Error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                manager.SetNestedValue("piggy.goals", table);
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddGoalsToTable();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openCalculatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("calc.exe");
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            JArray cur = ConvertTableToArray();
            JArray last = (JArray)manager.GetNestedValue("piggy.goals");

            if (!JToken.DeepEquals(cur, last))
            {
                DialogResult result = MessageBox.Show(
                    "Changes detected. Do you want to save them?",
                    "Save Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        private Dictionary<(int RowIndex, int ColumnIndex), string> _pendingChanges = new();

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = sender as DataGridView;
            if (dataGridView != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView.Columns[e.ColumnIndex].Name == "Goal" || dataGridView.Columns[e.ColumnIndex].Name == "Accumulated")
                {
                    string cellValue = dataGridView[e.ColumnIndex, e.RowIndex].Value?.ToString();
                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        _pendingChanges[(e.RowIndex, e.ColumnIndex)] = cellValue;
                    }
                }
            }
        }
        private void applyFormattingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var change in _pendingChanges)
            {
                try
                {
                    int rowIndex = change.Key.RowIndex;
                    int columnIndex = change.Key.ColumnIndex;

                    double result = EvaluateExpression(change.Value);

                    var control = this.Controls.Find("dataGridView1", true);
                    if (control.Length > 0 && control[0] is DataGridView dataGridView)
                    {
                        dataGridView[columnIndex, rowIndex].Value = result;
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Invalid expression in cell ({change.Key.RowIndex}, {change.Key.ColumnIndex}): {change.Value}\n{ex.Message}",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }

            _pendingChanges.Clear();
        }

        private void viewSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int totalGoals = 0;
            int totalAccumulated = 0;

            JArray table = ConvertTableToArray();

            foreach (var t in table)
            {
                Goal item = t.ToObject<Goal>();
                if (item != null)
                {
                    totalGoals += Int32.Parse(item.goal);
                    totalAccumulated += Int32.Parse(item.accumulated);
                }
            }
            MessageBox.Show($"TOTAL TABLE SUMMARY\n\nTotal sum needed: {totalGoals}\nTotal accumulated: {totalAccumulated}", "Goals summary", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
