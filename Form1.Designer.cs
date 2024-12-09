using Newtonsoft.Json.Linq;
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
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        JsonFileManager manager = new JsonFileManager();
        LanguageManager lang = new LanguageManager();

        void TargetsTableForm()
        {
            DataGridView _dataGridView = new DataGridView();

            _dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _dataGridView.Location = new Point(12, 49);
            _dataGridView.Name = "dataGridView1";
            _dataGridView.Size = new Size(776, 324);
            _dataGridView.TabIndex = 5;
            _dataGridView.BackgroundColor = SystemColors.Control;
            _dataGridView.GridColor = SystemColors.Control;
            _dataGridView.AllowUserToAddRows = false;
            _dataGridView.AllowUserToDeleteRows = true;
            _dataGridView.AllowUserToOrderColumns = false;
            _dataGridView.AllowUserToResizeColumns = true;
            _dataGridView.AllowUserToResizeRows = false;
            _dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            _dataGridView.Columns.Add("Id", "ID");
            _dataGridView.Columns.Add("Name", lang.GetText((string)manager.GetNestedValue("language"), "nameColumn"));
            _dataGridView.Columns.Add("Target", lang.GetText((string)manager.GetNestedValue("language"), "targetColumn"));
            _dataGridView.Columns.Add("Accumulated", lang.GetText((string)manager.GetNestedValue("language"), "accumulatedColumn"));
            _dataGridView.Columns.Add("Currency", lang.GetText((string)manager.GetNestedValue("language"), "currencyColumn"));
            _dataGridView.Columns.Add("Controls", lang.GetText((string)manager.GetNestedValue("language"), "controlsColumn"));

            Controls.Add(_dataGridView);
        }

        private void AddTargetsToTable()
        {
            JArray targets = (JArray) manager.GetNestedValue("piggy.targets");
            var control = this.Controls.Find("dataGridView1", true);
            if (control.Length > 0 && control[0] is DataGridView dataGridView)
            {
                (control[0] as DataGridView).Rows.Clear();
                int i = 0;
                foreach (var target in targets)
                {
                    (control[0] as DataGridView).Rows.Add(
                        i,
                        target["name"]?.ToString() ?? lang.GetText((string) manager.GetNestedValue("language"), "unknownTarget"),
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            button1 = new Button();
            panel1 = new Panel();
            button2 = new Button();
            label1 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(0, 3);
            button1.Name = "button1";
            button1.Size = new Size(200, 23);
            button1.TabIndex = 1;
            button1.Text = lang.GetText((string)manager.GetNestedValue("language"), "createButton");
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(12, 379);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 59);
            panel1.TabIndex = 2;
            // 
            // button2
            // 
            button2.Location = new Point(0, 32);
            button2.Name = "button2";
            button2.Size = new Size(200, 23);
            button2.TabIndex = 2;
            button2.Text = lang.GetText((string)manager.GetNestedValue("language"), "updateButton");
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15F);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(60, 28);
            label1.TabIndex = 4;
            label1.Text = lang.GetText((string)manager.GetNestedValue("language"), "targetsTitle");
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(label1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "ServeCoin";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Panel panel1;

        void onload()
        {
            TargetsTableForm();
            AddTargetsToTable();
        }
        private Label label1;
        private Button button2;
    }
}
