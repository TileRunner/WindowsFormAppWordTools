using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsAppWordTools
{
    public partial class FormMultipleSearchCriteria : Form
    {
        private ClassLexicon CurrentLexicon;

        private List<UserControlSearchCriteria> _conditions;
        private Color _normalCondBackgroundColor;

        public FormMultipleSearchCriteria()
        {
            InitializeComponent();
            _conditions = new List<UserControlSearchCriteria>();
            var cond = new UserControlSearchCriteria();
            _normalCondBackgroundColor = cond.BackColor;
            _conditions.Add(cond);
            panelConditions.Controls.Add(cond);
        }

        private void buttonAddCondition_Click(object sender, EventArgs e)
        {
            var cond = new UserControlSearchCriteria();
            var lastCond = _conditions.Last();
            _conditions.Add(cond);
            panelConditions.SuspendLayout();
            panelConditions.Controls.Add(cond);
            cond.Visible = true;
            cond.Left = 0;
            cond.Top = lastCond.Top + 30;
            panelConditions.ResumeLayout();
            panelConditions.SelectNextControl(lastCond, true, true, true, true);
        }

        private void buttonRemoveSelectedConditions_Click(object sender, EventArgs e)
        {
            if (!_conditions.Any(c => (!c.IsSelected)))
                MessageBox.Show("Cannot delete all conditions");
            else
            {
                panelConditions.SuspendLayout();
                for (int i = 0; i < _conditions.Count; i++)
                {
                    if (_conditions.ElementAt(i).IsSelected)
                    {
                        panelConditions.Controls.RemoveAt(i);
                        for (int j = i; j < _conditions.Count; j++)
                        {
                            _conditions.ElementAt(j).Top -= 30;
                        }
                        _conditions.RemoveAt(i);
                    }
                }
                panelConditions.ResumeLayout();
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            DateTime startTime = DateTime.Now;
            MDIParentWordTools parent = (MDIParentWordTools)this.MdiParent;
            CurrentLexicon = parent.globalLexicon;
            string result = string.Empty;
            bool anyCondSet = false;
            bool anyBadCond = false;
            foreach (UserControlSearchCriteria cond in _conditions)
            {
                if (!cond.GetIsValid() || (!CurrentLexicon.LoadedBaseLexicon && (cond.SearchCriteria.SearchOption == UserControlSearchCriteria.eSearchOption.WordIsAdded || cond.SearchCriteria.SearchOption == UserControlSearchCriteria.eSearchOption.WordIsDeleted)))
                {
                    cond.BackColor = Color.Red;
                    anyBadCond = true;
                }
                else
                {
                    cond.BackColor = _normalCondBackgroundColor;
                    anyCondSet = anyCondSet || cond.IsConditionSet;
                }
            }
            if (anyBadCond)
                result = "Bad condition(s) detected";
            else if (!anyCondSet)
                result = "No conditions are set";
            else
            {
                List<string> hits = new List<string>();
                bool supported = true;
                bool first = true;
                for (int i = 0; supported && i < _conditions.Count; i++)
                {
                    var cond = _conditions.ElementAt(i);
                    if (cond.IsConditionSet)
                    {
                        if (first)
                        {
                            first = false;
                            hits = CurrentLexicon.SelectBySearchCriteria(cond.SearchCriteria, out supported);
                        }
                        else
                        {
                            hits = CurrentLexicon.SelectBySearchCriteria(cond.SearchCriteria, hits, out supported);
                        }
                    }
                }
                if (!supported)
                    result = "Unsupported condition. Not coded yet.";
                else
                {
                    List<ClassWord> hitsWithInfo = new List<ClassWord>();
                    int width = 1;
                    foreach (string hitword in hits)
                    {
                        hitsWithInfo.Add(CurrentLexicon.GetClassWordForKnownWord(hitword));
                        width = Math.Max(width, hitword.Length);
                    }
                    dataGridViewResults.ColumnCount = 8;
                    dataGridViewResults.Columns[0].Name = "Front hooks";
                    dataGridViewResults.Columns[1].Name = "Word";
                    dataGridViewResults.Columns[2].Name = "State";
                    dataGridViewResults.Columns[3].Name = "Back hooks";
                    dataGridViewResults.Columns[4].Name = "Point value";
                    dataGridViewResults.Columns[5].Name = "Probability";
                    dataGridViewResults.Columns[6].Name = "Length";
                    dataGridViewResults.Columns[7].Name = "Alphagram";
                    dataGridViewResults.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                    dataGridViewResults.Rows.Clear();
                    dataGridViewResults.Columns[2].Visible = CurrentLexicon.LoadedBaseLexicon;
                    var hitsWithInfoQ = from ClassWord cw in hitsWithInfo
                                        orderby cw.Word.Length descending, cw.PointValue descending, cw.Word
                                        select cw;
                    foreach (ClassWord hitword in hitsWithInfoQ)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataGridViewResults);
                        row.DefaultCellStyle.BackColor = CurrentLexicon.LoadedBaseLexicon ? hitword.BaseLexiconState == ClassWord.eBaseLexiconState.Deleted ? Color.Red : hitword.BaseLexiconState == ClassWord.eBaseLexiconState.Added ? Color.Green : row.DefaultCellStyle.BackColor : row.DefaultCellStyle.BackColor;
                        row.SetValues(new string[] { string.Format("{0,26}", hitword.FrontHooks.ToLower()), hitword.Word, hitword.BaseLexiconState.ToString(), string.Format("{0,-26}", hitword.BackHooks.ToLower()), hitword.PointValue.ToString(), hitword.Probability.ToString(), hitword.Length.ToString(), hitword.Alphagram });
                        dataGridViewResults.Rows.Add(row);
                        //dataGridViewResults.Rows.Add(new string[] { string.Format("{0,26}", hitword.FrontHooks.ToLower()), hitword.Word, hitword.BaseLexiconState.ToString(), string.Format("{0,-26}", hitword.BackHooks.ToLower()), hitword.PointValue.ToString(), hitword.Probability.ToString(), hitword.Length.ToString(), hitword.Alphagram });
                    }
                }
            }
            DateTime endTime = DateTime.Now;
            TimeSpan elapsed = endTime.Subtract(startTime);
            this.labelSearchResultMessage.Text = $"{dataGridViewResults.RowCount} rows returned, elapsed time = {elapsed}";
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            var contents = dataGridViewResults.GetClipboardContent();
            Clipboard.SetText(contents.GetText());
            MessageBox.Show("Selected cells and corresponding headers were copied to the clipboard.");
        }
    }
}
