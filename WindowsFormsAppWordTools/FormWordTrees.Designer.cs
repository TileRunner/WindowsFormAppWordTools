namespace WindowsFormsAppWordTools
{
    partial class FormWordTrees
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWordTrees));
            this.listBoxWords = new System.Windows.Forms.ListBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonGetInfo = new System.Windows.Forms.Button();
            this.userControlSearchCriteria1 = new WindowsFormsAppWordTools.UserControlSearchCriteria();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.buttonTabDotsToggle = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxWords
            // 
            this.listBoxWords.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxWords.ContextMenuStrip = this.contextMenuStrip1;
            this.listBoxWords.FormattingEnabled = true;
            this.listBoxWords.HorizontalScrollbar = true;
            this.listBoxWords.Location = new System.Drawing.Point(66, 115);
            this.listBoxWords.Name = "listBoxWords";
            this.listBoxWords.Size = new System.Drawing.Size(633, 290);
            this.listBoxWords.TabIndex = 2;
            this.listBoxWords.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listBoxWords_KeyPress);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem1,
            this.toolStripMenuItem3});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 70);
            this.contextMenuStrip1.Text = "Context menu for output list box";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem2.Text = "Collapse";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem1.Text = "Expand";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(152, 22);
            this.toolStripMenuItem3.Text = "Set as tree root";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // buttonGetInfo
            // 
            this.buttonGetInfo.Location = new System.Drawing.Point(66, 65);
            this.buttonGetInfo.Name = "buttonGetInfo";
            this.buttonGetInfo.Size = new System.Drawing.Size(75, 23);
            this.buttonGetInfo.TabIndex = 3;
            this.buttonGetInfo.Text = "Get info";
            this.buttonGetInfo.UseVisualStyleBackColor = true;
            this.buttonGetInfo.Click += new System.EventHandler(this.buttonGetInfo_Click);
            // 
            // userControlSearchCriteria1
            // 
            this.userControlSearchCriteria1.Location = new System.Drawing.Point(12, 12);
            this.userControlSearchCriteria1.Name = "userControlSearchCriteria1";
            this.userControlSearchCriteria1.Size = new System.Drawing.Size(1103, 32);
            this.userControlSearchCriteria1.TabIndex = 4;
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClipboard.Location = new System.Drawing.Point(13, 115);
            this.buttonClipboard.Name = "buttonClipboard";
            this.buttonClipboard.Size = new System.Drawing.Size(36, 187);
            this.buttonClipboard.TabIndex = 5;
            this.buttonClipboard.Text = "C\r\nL\r\nI\r\nP\r\nB\r\nO\r\nA\r\nR\r\nD";
            this.buttonClipboard.UseVisualStyleBackColor = true;
            this.buttonClipboard.Click += new System.EventHandler(this.buttonClipboard_Click);
            // 
            // buttonTabDotsToggle
            // 
            this.buttonTabDotsToggle.Location = new System.Drawing.Point(13, 309);
            this.buttonTabDotsToggle.Name = "buttonTabDotsToggle";
            this.buttonTabDotsToggle.Size = new System.Drawing.Size(36, 96);
            this.buttonTabDotsToggle.TabIndex = 6;
            this.buttonTabDotsToggle.Text = "Use ...";
            this.buttonTabDotsToggle.UseVisualStyleBackColor = true;
            this.buttonTabDotsToggle.Click += new System.EventHandler(this.buttonTabDotsToggle_Click);
            // 
            // FormWordTrees
            // 
            this.AcceptButton = this.buttonGetInfo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1131, 450);
            this.Controls.Add(this.buttonTabDotsToggle);
            this.Controls.Add(this.buttonClipboard);
            this.Controls.Add(this.userControlSearchCriteria1);
            this.Controls.Add(this.buttonGetInfo);
            this.Controls.Add(this.listBoxWords);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormWordTrees";
            this.Text = "FormWordTrees";
            this.Load += new System.EventHandler(this.FormWordTrees_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox listBoxWords;
        private System.Windows.Forms.Button buttonGetInfo;
        private UserControlSearchCriteria userControlSearchCriteria1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.Button buttonClipboard;
        private System.Windows.Forms.Button buttonTabDotsToggle;
    }
}