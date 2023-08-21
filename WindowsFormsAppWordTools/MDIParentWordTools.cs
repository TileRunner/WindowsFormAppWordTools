using System;
using System.Windows.Forms;

namespace WindowsFormsAppWordTools
{
    public partial class MDIParentWordTools : Form
    {
        public ClassLexicon globalLexicon;

        private int childFormNumber = 0;

        public MDIParentWordTools()
        {
            InitializeComponent();
        }

        private void MDIParentWordTools_Load(object sender, EventArgs e)
        {
            selectLexiconAndLoad();
        }


        #region icon clicks
        private void NewLexiconIcon_Click(object sender, EventArgs e)
        {
            selectFileAndCreateLexicon();
        }

        private void OpenLexiconIcon_Click(object sender, EventArgs e)
        {
            selectLexiconAndLoad();
        }

        private void MultipleSearchCriteriaIcon_Click(object sender, EventArgs e)
        {
            launchMultipleSearchCriteria();
        }

        private void WordTreesIcon_Click(object sender, EventArgs e)
        {
            launchWordTrees();
        }

        private void HelpIcon_Click(object sender, EventArgs e)
        {
            showHelp();
        }
        #endregion

        #region menu item clicks
        private void NewLexiconMenuItem_Click(object sender, EventArgs e)
        {
            selectFileAndCreateLexicon();
        }

        private void OpenLexiconMenuItem_Click(object sender, EventArgs e)
        {
            selectLexiconAndLoad();
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        #endregion

        #region implementation
        private void selectFileAndCreateLexicon()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select source file (one word per line)";
            openFileDialog.CheckFileExists = true;
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All file (*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FullFileName = openFileDialog.FileName;
                string FileName = openFileDialog.SafeFileName;
                string LexiconName = FileName.Substring(0, FileName.Length - 4);
                this.Text = "Word Tools - " + LexiconName;
                globalLexicon = new ClassLexicon();
                int wordCount;
                globalLexicon.CreateLexicon(FullFileName, LexiconName, out wordCount);
                MessageBox.Show(string.Format("Lexicon {0} created, {1} words", LexiconName, wordCount));
            }
        }

        private void selectLexiconAndLoad()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select existing lexicon";
            openFileDialog.CheckFileExists = true;
            //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.InitialDirectory = System.IO.Directory.GetCurrentDirectory() + "\\Lexicons";
            openFileDialog.DefaultExt = "lex";
            openFileDialog.Filter = "Lexicons (*.lex)|*.lex";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.SafeFileName;
                string LexiconName = FileName.Substring(0, FileName.Length - 4);
                this.Text = "Word Tools - " + LexiconName;
                globalLexicon = new ClassLexicon();
                int wordCount;
                globalLexicon.LoadLexicon(LexiconName, out wordCount);
                var loadBaseLex = MessageBox.Show(string.Format("Lexicon {0} loaded, {1} words. Load a base Lexicon?", LexiconName, wordCount), "Lexicon Load", MessageBoxButtons.YesNo);
                if (loadBaseLex == DialogResult.Yes)
                {
                    if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        FileName = openFileDialog.SafeFileName;
                        LexiconName = FileName.Substring(0, FileName.Length - 4);
                        this.Text += string.Format(" (base {0})", LexiconName);
                        globalLexicon.LoadBaseLexicon(LexiconName, out int unchangedWordCount, out int deletedWordCount, out int addedWordCount);
                        MessageBox.Show(string.Format("Base Lexicon {0} loaded, {1} words unchanged, {2} words added, {3} words deleted.", LexiconName, unchangedWordCount, addedWordCount, deletedWordCount));
                    }
                }
            }
        }

        private void launchMultipleSearchCriteria()
        {
            FormMultipleSearchCriteria childForm = new FormMultipleSearchCriteria();
            childForm.MdiParent = this;
            childForm.Text = "Multiple Search Criteria (" + childFormNumber++ + ")";
            childForm.Show();
        }

        private void launchWordTrees()
        {
            FormWordTrees childForm = new FormWordTrees();
            childForm.MdiParent = this;
            childForm.Text = "Word Trees (" + childFormNumber++ + ")";
            childForm.Show();
        }

        private void launchAdHoc()
        {
            FormAdHoc childForm = new FormAdHoc();
            childForm.MdiParent = this;
            childForm.Text = "Window " + childFormNumber++;
            childForm.Show();
        }

        private void launchAdHocIcon_Click(object sender, EventArgs e)
        {
            launchAdHoc();
        }

        private void showHelp()
        {
            MessageBox.Show("Help is not done yet.");
        }
        #endregion
    }
}
