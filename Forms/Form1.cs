using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Nodes;
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

        private void DataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var dataGridView = sender as DataGridView;

            if (dataGridView != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if (dataGridView.Columns[e.ColumnIndex].Name == "Target" ||
                    dataGridView.Columns[e.ColumnIndex].Name == "Accumulated")
                {
                    string cellValue = dataGridView[e.ColumnIndex, e.RowIndex].Value?.ToString();

                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        try
                        {
                            double result = EvaluateExpression(cellValue);

                            dataGridView[e.ColumnIndex, e.RowIndex].Value = result;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Invalid expression: {cellValue}\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
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
                "Accumulated is bigger or equals to target. Delete row to finish target.",
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

                ITarget target = tableObj.ToObject<Target>();

                if (target.accumulated != null)
                {
                    foreach (char c in target.accumulated)
                    {
                        if (!char.IsDigit(c))
                        {
                            deniedReason = 0;
                            deniedString = $"{index}: accumulated";
                            break;
                        }
                    }
                }

                if (target.target != null)
                {
                    foreach (char c in target.target)
                    {
                        if (!char.IsDigit(c))
                        {
                            deniedReason = 0;
                            deniedString = $"{index}: target";
                            break;
                        }
                    }
                }

                if (deniedReason != -1)
                    break;

                if (Int32.Parse(target.target) <= Int32.Parse(target.accumulated))
                {
                    deniedReason = 1;
                    deniedString = $"{index}";
                }
                if (target.name.Length < 4)
                {
                    deniedReason = 2;
                    deniedString = $"{index}: name";
                }
                if (target.name.Length > 64)
                {
                    deniedReason = 3;
                    deniedString = $"{index}: name";
                }
                if (target.currency.Length < 3)
                {
                    deniedReason = 4;
                    deniedString = $"{index}: currency";
                }
                if (target.currency.Length > 24)
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
                manager.SetNestedValue("piggy.targets", table);
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTargetsToTable();
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
            JArray last = (JArray) manager.GetNestedValue("piggy.targets");

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
    }
}
