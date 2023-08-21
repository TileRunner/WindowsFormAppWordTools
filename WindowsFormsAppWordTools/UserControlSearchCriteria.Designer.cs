namespace WindowsFormsAppWordTools
{
    partial class UserControlSearchCriteria
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.checkBoxSelect = new System.Windows.Forms.CheckBox();
            this.comboBoxSearchOption = new System.Windows.Forms.ComboBox();
            this.labelTextBox = new System.Windows.Forms.Label();
            this.textBoxLettersOrRegex = new System.Windows.Forms.TextBox();
            this.labelNumericUpDown1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelNumericUpDown2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.checkBoxNot = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxSelect
            // 
            this.checkBoxSelect.AutoSize = true;
            this.checkBoxSelect.Location = new System.Drawing.Point(4, 7);
            this.checkBoxSelect.Name = "checkBoxSelect";
            this.checkBoxSelect.Size = new System.Drawing.Size(15, 14);
            this.checkBoxSelect.TabIndex = 0;
            this.checkBoxSelect.UseVisualStyleBackColor = true;
            // 
            // comboBoxSearchOption
            // 
            this.comboBoxSearchOption.FormattingEnabled = true;
            this.comboBoxSearchOption.Location = new System.Drawing.Point(54, 4);
            this.comboBoxSearchOption.Name = "comboBoxSearchOption";
            this.comboBoxSearchOption.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSearchOption.TabIndex = 1;
            this.comboBoxSearchOption.SelectedIndexChanged += new System.EventHandler(this.comboBoxSearchOption_SelectedIndexChanged);
            // 
            // labelTextBox
            // 
            this.labelTextBox.AutoSize = true;
            this.labelTextBox.Location = new System.Drawing.Point(240, 8);
            this.labelTextBox.Name = "labelTextBox";
            this.labelTextBox.Size = new System.Drawing.Size(68, 13);
            this.labelTextBox.TabIndex = 2;
            this.labelTextBox.Text = "labelTextBox";
            this.labelTextBox.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxLettersOrRegex
            // 
            this.textBoxLettersOrRegex.Location = new System.Drawing.Point(347, 4);
            this.textBoxLettersOrRegex.Name = "textBoxLettersOrRegex";
            this.textBoxLettersOrRegex.Size = new System.Drawing.Size(100, 20);
            this.textBoxLettersOrRegex.TabIndex = 3;
            // 
            // labelNumericUpDown1
            // 
            this.labelNumericUpDown1.AutoSize = true;
            this.labelNumericUpDown1.Location = new System.Drawing.Point(481, 8);
            this.labelNumericUpDown1.Name = "labelNumericUpDown1";
            this.labelNumericUpDown1.Size = new System.Drawing.Size(116, 13);
            this.labelNumericUpDown1.TabIndex = 4;
            this.labelNumericUpDown1.Text = "labelNumericUpDown1";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(625, 4);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown1.TabIndex = 5;
            // 
            // labelNumericUpDown2
            // 
            this.labelNumericUpDown2.AutoSize = true;
            this.labelNumericUpDown2.Location = new System.Drawing.Point(761, 8);
            this.labelNumericUpDown2.Name = "labelNumericUpDown2";
            this.labelNumericUpDown2.Size = new System.Drawing.Size(116, 13);
            this.labelNumericUpDown2.TabIndex = 6;
            this.labelNumericUpDown2.Text = "labelNumericUpDown2";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(902, 4);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 20);
            this.numericUpDown2.TabIndex = 7;
            // 
            // checkBoxNot
            // 
            this.checkBoxNot.AutoSize = true;
            this.checkBoxNot.Location = new System.Drawing.Point(1058, 6);
            this.checkBoxNot.Name = "checkBoxNot";
            this.checkBoxNot.Size = new System.Drawing.Size(43, 17);
            this.checkBoxNot.TabIndex = 8;
            this.checkBoxNot.Text = "Not";
            this.checkBoxNot.UseVisualStyleBackColor = true;
            // 
            // UserControlSearchCriteria
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxNot);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.labelNumericUpDown2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.labelNumericUpDown1);
            this.Controls.Add(this.textBoxLettersOrRegex);
            this.Controls.Add(this.labelTextBox);
            this.Controls.Add(this.comboBoxSearchOption);
            this.Controls.Add(this.checkBoxSelect);
            this.Name = "UserControlSearchCriteria";
            this.Size = new System.Drawing.Size(1103, 32);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSelect;
        private System.Windows.Forms.ComboBox comboBoxSearchOption;
        private System.Windows.Forms.Label labelTextBox;
        private System.Windows.Forms.TextBox textBoxLettersOrRegex;
        private System.Windows.Forms.Label labelNumericUpDown1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label labelNumericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.CheckBox checkBoxNot;
    }
}
