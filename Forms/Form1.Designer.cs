using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Windows.Forms;

namespace servecoin
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Console.WriteLine("Disposing components...");
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        JsonFileManager manager = new JsonFileManager();
        public DataGridView _dataGridView = new DataGridView();
        void TargetsTableForm()
        {
            _dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dataGridView.Location = new Point(0, 65);
            _dataGridView.Name = "dataGridView1";
            _dataGridView.Size = new Size(800, 385);
            _dataGridView.TabIndex = 5;
            _dataGridView.BackgroundColor = SystemColors.Control;
            _dataGridView.GridColor = SystemColors.Control;
            _dataGridView.AllowUserToAddRows = false;
            _dataGridView.AllowUserToDeleteRows = true;
            _dataGridView.AllowUserToOrderColumns = false;
            _dataGridView.AllowUserToResizeColumns = true;
            _dataGridView.AllowUserToResizeRows = false;
            _dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            _dataGridView.CellEndEdit += DataGridView_CellEndEdit;

            _dataGridView.Columns.Add("Id", "ID");
            _dataGridView.Columns.Add("Name", "Name");
            _dataGridView.Columns.Add("Target", "Target");
            _dataGridView.Columns.Add("Accumulated", "Accumulated");
            _dataGridView.Columns.Add("Currency", "Currency");

            Controls.Add(_dataGridView);
        }

        private JArray ConvertTableToArray()
        {
            JArray jsonArray = new JArray();
            var control = this.Controls.Find("dataGridView1", true);
            if (control.Length > 0 && control[0] is DataGridView dataGridView)
            {
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        JObject obj = new JObject
                        {
                            ["name"] = row.Cells["Name"].Value?.ToString() ?? string.Empty,
                            ["target"] = row.Cells["Target"].Value?.ToString() ?? string.Empty,
                            ["accumulated"] = row.Cells["Accumulated"].Value?.ToString() ?? string.Empty,
                            ["currency"] = row.Cells["Currency"].Value?.ToString() ?? string.Empty
                        };

                        jsonArray.Add(obj);
                    }
                }
            }
            return jsonArray;
        }

        public void AddTargetsToTable()
        {
            JArray targets = (JArray) manager.GetNestedValue("piggy.targets");
            var control = this.Controls.Find("dataGridView1", true);
            if (control.Length > 0 && control[0] is DataGridView dataGridView)
            {
                dataGridView.Rows.Clear();
                int i = 0;
                foreach (var target in targets)
                {
                    dataGridView.Rows.Add(
                        i,
                        target["name"]?.ToString() ?? "Unknown",
                        target["target"]?.ToString() ?? "0",
                        target["accumulated"]?.ToString() ?? "0",
                        target["currency"]?.ToString() ?? ""
                    );
                    i++;
                }
            }
        }


        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            label1 = new Label();
            toolStrip1 = new ToolStrip();
            toolStripDropDownButton1 = new ToolStripDropDownButton();
            saveToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton2 = new ToolStripDropDownButton();
            createToolStripMenuItem = new ToolStripMenuItem();
            refreshToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            applyFormattingToolStripMenuItem = new ToolStripMenuItem();
            toolStripDropDownButton3 = new ToolStripDropDownButton();
            openCalculatorToolStripMenuItem = new ToolStripMenuItem();
            dataGridView2 = new DataGridView();
            viewSummaryToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            ((ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(12, 25);
            label1.Name = "label1";
            label1.Size = new Size(74, 28);
            label1.TabIndex = 4;
            label1.Text = "Targets";
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImeMode = ImeMode.NoControl;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripDropDownButton1, toolStripDropDownButton2, toolStripDropDownButton3 });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(800, 25);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            toolStripDropDownButton1.AccessibleName = "";
            toolStripDropDownButton1.AutoSize = false;
            toolStripDropDownButton1.AutoToolTip = false;
            toolStripDropDownButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton1.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, toolStripSeparator1, aboutToolStripMenuItem, exitToolStripMenuItem });
            toolStripDropDownButton1.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            toolStripDropDownButton1.Size = new Size(66, 22);
            toolStripDropDownButton1.Text = "Program";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(107, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(104, 6);
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(107, 22);
            aboutToolStripMenuItem.Text = "About";
            aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(107, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton2
            // 
            toolStripDropDownButton2.AutoSize = false;
            toolStripDropDownButton2.AutoToolTip = false;
            toolStripDropDownButton2.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton2.DropDownItems.AddRange(new ToolStripItem[] { createToolStripMenuItem, refreshToolStripMenuItem, viewSummaryToolStripMenuItem, toolStripSeparator2, applyFormattingToolStripMenuItem });
            toolStripDropDownButton2.Image = (Image)resources.GetObject("toolStripDropDownButton2.Image");
            toolStripDropDownButton2.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            toolStripDropDownButton2.Size = new Size(47, 22);
            toolStripDropDownButton2.Text = "Table";
            // 
            // createToolStripMenuItem
            // 
            createToolStripMenuItem.Name = "createToolStripMenuItem";
            createToolStripMenuItem.Size = new Size(180, 22);
            createToolStripMenuItem.Text = "Create target";
            createToolStripMenuItem.Click += createToolStripMenuItem_Click;
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new Size(180, 22);
            refreshToolStripMenuItem.Text = "Refresh table";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(177, 6);
            // 
            // applyFormattingToolStripMenuItem
            // 
            applyFormattingToolStripMenuItem.Name = "applyFormattingToolStripMenuItem";
            applyFormattingToolStripMenuItem.Size = new Size(180, 22);
            applyFormattingToolStripMenuItem.Text = "Apply formatting";
            applyFormattingToolStripMenuItem.Click += applyFormattingToolStripMenuItem_Click;
            // 
            // toolStripDropDownButton3
            // 
            toolStripDropDownButton3.AutoSize = false;
            toolStripDropDownButton3.AutoToolTip = false;
            toolStripDropDownButton3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripDropDownButton3.DropDownItems.AddRange(new ToolStripItem[] { openCalculatorToolStripMenuItem });
            toolStripDropDownButton3.Image = (Image)resources.GetObject("toolStripDropDownButton3.Image");
            toolStripDropDownButton3.ImageTransparentColor = Color.Magenta;
            toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            toolStripDropDownButton3.Size = new Size(47, 22);
            toolStripDropDownButton3.Text = "Tools";
            // 
            // openCalculatorToolStripMenuItem
            // 
            openCalculatorToolStripMenuItem.Name = "openCalculatorToolStripMenuItem";
            openCalculatorToolStripMenuItem.Size = new Size(160, 22);
            openCalculatorToolStripMenuItem.Text = "Open Calculator";
            openCalculatorToolStripMenuItem.Click += openCalculatorToolStripMenuItem_Click;
            // 
            // dataGridView2
            // 
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Location = new Point(0, 65);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.Size = new Size(800, 385);
            dataGridView2.TabIndex = 6;
            // 
            // viewSummaryToolStripMenuItem
            // 
            viewSummaryToolStripMenuItem.Name = "viewSummaryToolStripMenuItem";
            viewSummaryToolStripMenuItem.Size = new Size(180, 22);
            viewSummaryToolStripMenuItem.Text = "View summary";
            viewSummaryToolStripMenuItem.Click += viewSummaryToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(dataGridView2);
            Controls.Add(toolStrip1);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "ServeCoin";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ((ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        void onload()
        {
            dataGridView2.Visible = false;
            TargetsTableForm();
            AddTargetsToTable();
        }
        private Label label1;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton2;
        private ToolStripMenuItem createToolStripMenuItem;
        private ToolStripMenuItem refreshToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButton3;
        private ToolStripMenuItem openCalculatorToolStripMenuItem;
        private ToolStripMenuItem createToolStripMenuItem1;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem applyFormattingToolStripMenuItem;
        private DataGridView dataGridView2;
        private ToolStripMenuItem viewSummaryToolStripMenuItem;
    }
}
