using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Scrabble
{
    public class ModalWord : Form
    {
        private Label label1;
        private ComboBox comboBox1;
        private Button button1;
        private string selectedWord;

        public string SelectedWord
        {
            get { return selectedWord; }
        }

        //if no choices dont return
        public ModalWord(List<string> choices)
        {
            InitializeComponent();
            choices.Sort();
            DialogResult = DialogResult.Cancel;
            
            foreach (var result in choices)
            {
                comboBox1.Items.Add(result);
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select one of the following words:";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(184, 6);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ModalWord
            // 
            this.ClientSize = new System.Drawing.Size(309, 70);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ModalWord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            //need to catch null reference exception
            selectedWord = comboBox1.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }
    }
}
