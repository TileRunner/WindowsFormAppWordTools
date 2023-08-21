namespace WindowsFormsAppWordTools
{
    partial class FormMultipleSearchCriteria
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMultipleSearchCriteria));
            this.panelConditions = new System.Windows.Forms.Panel();
            this.buttonAddCondition = new System.Windows.Forms.Button();
            this.buttonRemoveSelectedConditions = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.dataGridViewResults = new System.Windows.Forms.DataGridView();
            this.classLexiconBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.labelSearchResultMessage = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.classLexiconBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panelConditions
            // 
            this.panelConditions.AutoScroll = true;
            this.panelConditions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelConditions.Location = new System.Drawing.Point(0, 0);
            this.panelConditions.Name = "panelConditions";
            this.panelConditions.Size = new System.Drawing.Size(1159, 164);
            this.panelConditions.TabIndex = 0;
            // 
            // buttonAddCondition
            // 
            this.buttonAddCondition.AutoSize = true;
            this.buttonAddCondition.Location = new System.Drawing.Point(13, 171);
            this.buttonAddCondition.Name = "buttonAddCondition";
            this.buttonAddCondition.Size = new System.Drawing.Size(82, 23);
            this.buttonAddCondition.TabIndex = 1;
            this.buttonAddCondition.Text = "Add condition";
            this.buttonAddCondition.UseVisualStyleBackColor = true;
            this.buttonAddCondition.Click += new System.EventHandler(this.buttonAddCondition_Click);
            // 
            // buttonRemoveSelectedConditions
            // 
            this.buttonRemoveSelectedConditions.AutoSize = true;
            this.buttonRemoveSelectedConditions.Location = new System.Drawing.Point(134, 171);
            this.buttonRemoveSelectedConditions.Name = "buttonRemoveSelectedConditions";
            this.buttonRemoveSelectedConditions.Size = new System.Drawing.Size(151, 23);
            this.buttonRemoveSelectedConditions.TabIndex = 2;
            this.buttonRemoveSelectedConditions.Text = "Remove selected conditions";
            this.buttonRemoveSelectedConditions.UseVisualStyleBackColor = true;
            this.buttonRemoveSelectedConditions.Click += new System.EventHandler(this.buttonRemoveSelectedConditions_Click);
            // 
            // buttonSearch
            // 
            this.buttonSearch.AutoSize = true;
            this.buttonSearch.Location = new System.Drawing.Point(335, 170);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 3;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = true;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Location = new System.Drawing.Point(13, 216);
            this.buttonClipboard.Name = "buttonClipboard";
            this.buttonClipboard.Size = new System.Drawing.Size(29, 222);
            this.buttonClipboard.TabIndex = 5;
            this.buttonClipboard.Text = "C\r\nL\r\nI\r\nP\r\nB\r\nO\r\nA\r\nR\r\nD";
            this.buttonClipboard.UseVisualStyleBackColor = true;
            this.buttonClipboard.Click += new System.EventHandler(this.buttonClipboard_Click);
            // 
            // dataGridViewResults
            // 
            this.dataGridViewResults.AllowUserToAddRows = false;
            this.dataGridViewResults.AllowUserToDeleteRows = false;
            this.dataGridViewResults.AllowUserToOrderColumns = true;
            this.dataGridViewResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewResults.Location = new System.Drawing.Point(48, 216);
            this.dataGridViewResults.Name = "dataGridViewResults";
            this.dataGridViewResults.ReadOnly = true;
            this.dataGridViewResults.Size = new System.Drawing.Size(1099, 222);
            this.dataGridViewResults.TabIndex = 6;
            // 
            // classLexiconBindingSource
            // 
            this.classLexiconBindingSource.DataSource = typeof(WindowsFormsAppWordTools.ClassLexicon);
            // 
            // labelSearchResultMessage
            // 
            this.labelSearchResultMessage.AutoSize = true;
            this.labelSearchResultMessage.Location = new System.Drawing.Point(444, 175);
            this.labelSearchResultMessage.Name = "labelSearchResultMessage";
            this.labelSearchResultMessage.Size = new System.Drawing.Size(0, 13);
            this.labelSearchResultMessage.TabIndex = 7;
            // 
            // FormMultipleSearchCriteria
            // 
            this.AcceptButton = this.buttonSearch;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 450);
            this.Controls.Add(this.labelSearchResultMessage);
            this.Controls.Add(this.dataGridViewResults);
            this.Controls.Add(this.buttonClipboard);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonRemoveSelectedConditions);
            this.Controls.Add(this.buttonAddCondition);
            this.Controls.Add(this.panelConditions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMultipleSearchCriteria";
            this.Text = "Multiple Search Criteria";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewResults)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.classLexiconBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelConditions;
        private System.Windows.Forms.Button buttonAddCondition;
        private System.Windows.Forms.Button buttonRemoveSelectedConditions;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonClipboard;
        private System.Windows.Forms.BindingSource classLexiconBindingSource;
        private System.Windows.Forms.DataGridView dataGridViewResults;
        private System.Windows.Forms.Label labelSearchResultMessage;
    }
}