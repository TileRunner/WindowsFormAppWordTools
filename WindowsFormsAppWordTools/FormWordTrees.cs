using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace WindowsFormsAppWordTools
{
    public partial class FormWordTrees : Form
    {
        private ClassLexicon CurrentLexicon;
        private ArrayList _listBoxItems;

        public FormWordTrees()
        {
            InitializeComponent();
        }

        private void FormWordTrees_Load(object sender, EventArgs e)
        {
            MDIParentWordTools parent = (MDIParentWordTools)this.MdiParent;
            CurrentLexicon = parent.globalLexicon;
            _listBoxItems = new ArrayList();
        }

        private void buttonGetInfo_Click(object sender, EventArgs e)
        {
            getInfo();
            bindListItems();
        }

        private void getInfo()
        {
            _listBoxItems.Clear();
            if (!userControlSearchCriteria1.IsConditionSet)
            {
                MessageBox.Show("Please select a search type", "Get info problem", MessageBoxButtons.OK);
                return;
            }
            if (!userControlSearchCriteria1.GetIsValid())
            {
                MessageBox.Show("Invalid selection criteria", "Get info problem", MessageBoxButtons.OK);
                return;
            }
            var matchingWords = CurrentLexicon.SelectBySearchCriteria(userControlSearchCriteria1.SearchCriteria, out bool supported);
            if (!supported)
            {
                MessageBox.Show("That search type is not supported yet", "Get info problem", MessageBoxButtons.OK);
                return;
            }
            List<string> matchingAlphagrams = new List<string>();
            foreach (string matchingWord in matchingWords)
            {
                matchingAlphagrams.Add(CurrentLexicon.GetClassWordForKnownWord(matchingWord).Alphagram);
            }
            var sortedAlphagramsQuery = from string alphagram in matchingAlphagrams orderby alphagram.Length descending, alphagram select alphagram;
            string userNote;
            string baseState = "";
            _listBoxItems.Add(new ClassListBoxItem("(* signifies criteria match)", "", ClassListBoxItem.eItemType.info));
            foreach (string alphagram in sortedAlphagramsQuery.Distinct())
            {
                userNote = getUserNote(alphagram);
                _listBoxItems.Add(new ClassListBoxItem(string.Format("Alphagram\t{0}\t{1}", alphagram, userNote), alphagram, ClassListBoxItem.eItemType.alphagram));
                var anagrams = CurrentLexicon.GetAnagrams(alphagram);
                foreach (string word in anagrams)
                {
                    userNote = getUserNote(word);
                    baseState = getBaseState(word);
                    string asterisk = "";
                    if (matchingWords.Contains(word))
                        asterisk = "*";
                    _listBoxItems.Add(new ClassListBoxItem(string.Format("\tAnagram{0}\t{1}\t{2}\t{3}", asterisk, word, baseState, userNote), word, ClassListBoxItem.eItemType.word));
                    List<string> drops = CurrentLexicon.GetDrops(word);
                    List<string> swaps = CurrentLexicon.GetSwaps(word);
                    List<string> inserts = CurrentLexicon.GetInserts(word);
                    foreach (string drop in drops)
                    {
                        userNote = getUserNote(drop);
                        baseState = getBaseState(drop);
                        _listBoxItems.Add(new ClassListBoxItem(string.Format("\t\tDrop\t{0}\t{1}\t{2}", drop, baseState, userNote), drop, ClassListBoxItem.eItemType.word));
                    }
                    foreach (string swap in swaps)
                    {
                        userNote = getUserNote(swap);
                        baseState = getBaseState(swap);
                        _listBoxItems.Add(new ClassListBoxItem(string.Format("\t\tSwap\t{0}\t{1}\t{2}", swap, baseState, userNote), swap, ClassListBoxItem.eItemType.word));
                    }
                    foreach (string insert in inserts)
                    {
                        userNote = getUserNote(insert);
                        baseState = getBaseState(insert);
                        _listBoxItems.Add(new ClassListBoxItem(string.Format("\t\tInsert\t{0}\t{1}\t{2}", insert, baseState, userNote), insert, ClassListBoxItem.eItemType.word));
                    }
                }
            }
        }

        private string getUserNote(string word)
        {
            if (CurrentLexicon.UserNotes.ContainsKey(word))
                return CurrentLexicon.UserNotes[word];
            return "";
        }

        private string getBaseState(string word)
        {
            string baseState = "";
            if (CurrentLexicon.LoadedBaseLexicon)
            {
                ClassWord cword = CurrentLexicon.GetClassWordForKnownWord(word);
                baseState = cword.BaseLexiconState.ToString();
            }
            return baseState;
        }
        private void bindListItems()
        {
            listBoxWords.DataSource = null;
            listBoxWords.DataSource = _listBoxItems;
            listBoxWords.DisplayMember = "Display";
            listBoxWords.ValueMember = "Value";
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //Collapse
            CollapseInfoForSelectedItem();

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //Expand
            ExpandInfoForSelectedItem();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            //Set as tree root
            SetSelectedItemAsRoot();
        }

        private void listBoxWords_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '>':
                case '.':
                    ExpandInfoForSelectedItem();
                    break;
                case '<':
                case ',':
                    CollapseInfoForSelectedItem();
                    break;
            }
        }

        private void CollapseInfoForSelectedItem()
        {
            var item = (ClassListBoxItem)listBoxWords.SelectedItem;
            if (item.ItemType == ClassListBoxItem.eItemType.word)
            {
                string word = item.Value;
                if (word.Length >= 2 && word.Length <= 15)
                {
                    int deletionPoint = listBoxWords.SelectedIndex + 1;
                    int numTabs = item.LeadingTabCount;
                    bool b = true;
                    while (b)
                    {
                        b = false;
                        if (deletionPoint < _listBoxItems.Count)
                        {
                            listBoxWords.SelectedIndex = deletionPoint;
                            int numTabs2 = ((ClassListBoxItem)_listBoxItems[deletionPoint]).LeadingTabCount;
                            if (numTabs2 > numTabs)
                            {
                                _listBoxItems.RemoveAt(deletionPoint);
                                if (deletionPoint == _listBoxItems.Count)
                                {
                                    listBoxWords.SelectedIndex = deletionPoint - 1; // Removed last item, must not stay selected there
                                }
                                else
                                {
                                    b = true;
                                }
                            }
                        }
                    }
                }
                bindListItems();
            }
        }

        private void ExpandInfoForSelectedItem()
        {
            var item = (ClassListBoxItem)listBoxWords.SelectedItem;
            if (item.ItemType == ClassListBoxItem.eItemType.word)
            {
                string word = item.Value;
                if (word.Length >= 2 && word.Length <= 15)
                {
                    //string tags = string.Empty;
                    int insertionPoint = listBoxWords.SelectedIndex + 1;
                    string tabs = "\t";
                    string baseState = "";
                    for (int i = 0; i < item.Display.Length && char.Equals(item.Display[i],'\t'); i++)
                    {
                        tabs += "\t";
                    }
                    List<string> anagrams = CurrentLexicon.GetAnagrams(ClassWord.GetAlphagram(word));
                    List<string> drops = CurrentLexicon.GetDrops(word);
                    List<string> swaps = CurrentLexicon.GetSwaps(word);
                    List<string> inserts = CurrentLexicon.GetInserts(word);
                    foreach (string anagram in anagrams.Where(a=>!string.Equals(a,word)))
                    {
                        //tags = GetTags(anagram);
                        baseState = getBaseState(anagram);
                        _listBoxItems.Insert(insertionPoint, new ClassListBoxItem(string.Format("{0}Anagram\t{1}\t{2}", tabs, anagram, baseState), anagram, ClassListBoxItem.eItemType.word));
                        ++insertionPoint;
                    }
                    foreach (string drop in drops)
                    {
                        //tags = GetTags(drop);
                        baseState = getBaseState(drop);
                        _listBoxItems.Insert(insertionPoint, new ClassListBoxItem(string.Format("{0}Drop\t{1}\t{2}", tabs, drop, baseState), drop, ClassListBoxItem.eItemType.word));
                        ++insertionPoint;
                    }
                    foreach (string swap in swaps)
                    {
                        //tags = GetTags(swap);
                        baseState = getBaseState(swap);
                        _listBoxItems.Insert(insertionPoint, new ClassListBoxItem(string.Format("{0}Swap\t{1}\t{2}", tabs, swap, baseState), swap, ClassListBoxItem.eItemType.word));
                        ++insertionPoint;
                    }
                    foreach (string insert in inserts)
                    {
                        //tags = GetTags(insert);
                        baseState = getBaseState(insert);
                        _listBoxItems.Insert(insertionPoint, new ClassListBoxItem(string.Format("{0}Insert\t{1}\t{2}", tabs, insert, baseState), insert, ClassListBoxItem.eItemType.word));
                        ++insertionPoint;
                    }
                }
                bindListItems();
            }
        }

        private void SetSelectedItemAsRoot()
        {
            var item = (ClassListBoxItem)listBoxWords.SelectedItem;
            string alphagram = null;
            if (item.ItemType == ClassListBoxItem.eItemType.word)
            {
                alphagram = ClassWord.GetAlphagram(item.Value);
            }
            else if (item.ItemType == ClassListBoxItem.eItemType.alphagram)
            {
                alphagram = item.Value;
            }
            if (!string.IsNullOrEmpty(alphagram))
            {
                _listBoxItems.Clear();
                string tags;
                tags = string.Empty; // GetTags(alphagram) equivalent to be done later
                _listBoxItems.Add(new ClassListBoxItem(string.Format("Alphagram{0}\t{1}", tags, alphagram), alphagram, ClassListBoxItem.eItemType.alphagram));
                var anagrams = CurrentLexicon.GetAnagrams(alphagram);
                foreach (string word in anagrams)
                {
                    tags = string.Empty; // GetTags(word) equivalent to be done later
                    _listBoxItems.Add(new ClassListBoxItem(string.Format("\tAnagram{0}\t{1}", tags, word), word, ClassListBoxItem.eItemType.word));
                    tags = "";
                    List<string> drops = CurrentLexicon.GetDrops(word);
                    List<string> swaps = CurrentLexicon.GetSwaps(word);
                    List<string> inserts = CurrentLexicon.GetInserts(word);
                    foreach (string drop in drops)
                    {
                        //tags = GetTags(drop);
                        _listBoxItems.Add(new ClassListBoxItem(string.Format("\t\tDrop{0}\t{1}", tags, drop), drop, ClassListBoxItem.eItemType.word));
                    }
                    foreach (string swap in swaps)
                    {
                        //tags = GetTags(swap);
                        _listBoxItems.Add(new ClassListBoxItem(string.Format("\t\tSwap{0}\t{1}", tags, swap), swap, ClassListBoxItem.eItemType.word));
                    }
                    foreach (string insert in inserts)
                    {
                        //tags = GetTags(insert);
                        _listBoxItems.Add(new ClassListBoxItem(string.Format("\t\tInsert{0}\t{1}", tags, insert), insert, ClassListBoxItem.eItemType.word));
                    }
                }
                bindListItems();
            }
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            foreach (ClassListBoxItem item in listBoxWords.Items)
            {
                text += item.Display + Environment.NewLine;
            }
            Clipboard.SetText(text);
            MessageBox.Show("The word tree was copied to the clipboard.");
        }

        private void buttonTabDotsToggle_Click(object sender, EventArgs e)
        {
            string text = string.Empty;
            foreach (ClassListBoxItem item in listBoxWords.Items)
            {
                string s = item.Display;
                string leadingDots = "";
                for (int i = 0; i < item.LeadingTabCount; i++)
                {
                    leadingDots += "...";
                }
                s = leadingDots + s.Substring(item.LeadingTabCount);
                text += s + Environment.NewLine;
            }
            Clipboard.SetText(text);
            MessageBox.Show("The word tree was copied to the clipboard using ... instead of tab.");
        }
    }
}
