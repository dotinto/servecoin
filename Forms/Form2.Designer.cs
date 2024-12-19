using Newtonsoft.Json.Linq;

namespace servecoin
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        Form1 form1 = new Form1();

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        /// 


        private void SaveButtonPressed(object sender, EventArgs args)
        {
            String currency = "Не указано";
            bool permitedToSave = true;

            if (textBox4.Text == "" && comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Cannot create goal: Currency not selected", "Cannot create goal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                permitedToSave = false;
            } else if (textBox4.Text != "")
            {
                currency = textBox4.Text;
            } else
            {
                currency = comboBox1.SelectedItem.ToString();
            }
            
            if (int.Parse(textBox2.Text) <= int.Parse(textBox3.Text))
            {
                MessageBox.Show("Cannot create goal: Accumulated more than the goal", "Cannot create goal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                permitedToSave = false;
            }
            if (textBox1.TextLength < 4)
            {
                MessageBox.Show("Cannot create goal: Name contains less than 4 characters", "Cannot create goal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                permitedToSave = false;
            }

            JObject data = new JObject
            {
                ["name"] = textBox1.Text,
                ["goal"] = textBox2.Text,
                ["accumulated"] = textBox3.Text,
                ["currency"] = currency,
            };
            
            if (permitedToSave)
            {
                manager.AddToArrayByPath("piggy.goals", data);
                form1.AddGoalsToTable();
                this.Close();
            }
        }
        private void InitializeComponent()
        {
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            comboBox1 = new ComboBox();
            button1 = new Button();
            button2 = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(12, 12);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Name (4-64 symbols)";
            textBox1.Size = new Size(437, 23);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(12, 41);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Goal";
            textBox2.Size = new Size(113, 23);
            textBox2.TabIndex = 1;
            textBox2.KeyPress += textBox2_KeyPress;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(12, 70);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Accumulated";
            textBox3.Size = new Size(113, 23);
            textBox3.TabIndex = 2;
            textBox3.KeyPress += textBox3_KeyPress;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(217, 41);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Currency (3-24 symbols)";
            textBox4.Size = new Size(232, 23);
            textBox4.TabIndex = 3;
            // 
            // comboBox1
            // 
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "US Dollar (USD)", "", "Euro (EUR)", "", "Japanese Yen (JPY)", "", "British Pound Sterling (GBP)", "", "Australian Dollar (AUD)", "", "Canadian Dollar (CAD)", "", "Swiss Franc (CHF)", "", "Chinese Yuan Renminbi (CNY)", "", "Hong Kong Dollar (HKD)", "", "New Zealand Dollar (NZD)" });
            comboBox1.Location = new Point(217, 70);
            comboBox1.Name = "comboBox1";
            comboBox1.RightToLeft = RightToLeft.No;
            comboBox1.Size = new Size(232, 23);
            comboBox1.TabIndex = 4;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(336, 133);
            button1.Name = "button1";
            button1.Size = new Size(113, 23);
            button1.TabIndex = 5;
            button1.Text = "Save and close";
            button1.UseVisualStyleBackColor = true;
            button1.Click += SaveButtonPressed;
            // 
            // button2
            // 
            button2.Location = new Point(12, 133);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 6;
            button2.Text = "Cancel";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(461, 168);
            ControlBox = false;
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(comboBox1);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            ShowInTaskbar = false;
            Text = "Create goal";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private ComboBox comboBox1;
        private Button button1;
        private Button button2;
    }
}