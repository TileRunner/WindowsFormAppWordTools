using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppWordTools
{
    public partial class UserControlSearchCriteria : UserControl
    {
        public enum eSearchOption
        {
            Unselected,
            Anagram,
            BackHooksRegex,
            FrontHooksRegex,
            Length,
            Points,
            Regex,
            RegexAt,
            RegexOfAlphagram,
            RegexPartial,
            StickyFirstLEtter,
            StickyLastLetter,
            SubAnagram,
            SubWordAt,
            WordIsAdded,
            WordIsDeleted
        }

        private eSearchOption _searchOption;

        public struct StructSearchCriteria
        {
            public eSearchOption SearchOption;
            public string SpecifiedLetters;
            public string SpecifiedRegex;
            public int SpecifiedSubMatchPosition;
            public int SpecifiedSubMatchLength;
            public int SpecifiedMinimumPointValue;
            public int SpecifiedMaximumPointValue;
            public int SpecifiedMinimumWordLength;
            public int SpecifiedMaximumWordLength;
            public bool SpecifiedNot;
        }

        private StructSearchCriteria _searchCriteria;

        public StructSearchCriteria SearchCriteria
        {
            get
            {
                _searchCriteria.SearchOption = _searchOption;
                _searchCriteria.SpecifiedLetters = textBoxLettersOrRegex.Text;
                _searchCriteria.SpecifiedRegex = textBoxLettersOrRegex.Text;
                _searchCriteria.SpecifiedSubMatchPosition = (int)numericUpDown1.Value;
                _searchCriteria.SpecifiedSubMatchLength = (int)numericUpDown2.Value;
                _searchCriteria.SpecifiedMinimumPointValue = (int)numericUpDown1.Value;
                _searchCriteria.SpecifiedMaximumPointValue = (int)numericUpDown2.Value;
                _searchCriteria.SpecifiedMinimumWordLength = _minLen;
                _searchCriteria.SpecifiedMaximumWordLength = _maxLen;
                _searchCriteria.SpecifiedNot = checkBoxNot.Checked;
                return _searchCriteria;
            }
        }

        public UserControlSearchCriteria()
        {
            InitializeComponent();
            comboBoxSearchOption.Items.Add("<Please Select>");
            comboBoxSearchOption.Items.Add("Anagram");
            comboBoxSearchOption.Items.Add("Back hooks regex");
            comboBoxSearchOption.Items.Add("Front hooks regex");
            comboBoxSearchOption.Items.Add("Length");
            comboBoxSearchOption.Items.Add("Points");
            comboBoxSearchOption.Items.Add("Regex");
            comboBoxSearchOption.Items.Add("Regex at");
            comboBoxSearchOption.Items.Add("Regex of alphagram");
            comboBoxSearchOption.Items.Add("Regex partial");
            comboBoxSearchOption.Items.Add("Sticky first letter");
            comboBoxSearchOption.Items.Add("Sticky last letter");
            comboBoxSearchOption.Items.Add("Sub anagram");
            comboBoxSearchOption.Items.Add("Sub word at");
            comboBoxSearchOption.Items.Add("Word is added");
            comboBoxSearchOption.Items.Add("Word is deleted");
            comboBoxSearchOption.SelectedIndex = 0;
            _searchCriteria = new StructSearchCriteria();
        }

        public bool IsSelected
        {
            get { return checkBoxSelect.Checked; }
        }

        public bool GetIsValid()
        {
            bool retval = true;
            string chars;
            switch (_searchOption)
            {
                case eSearchOption.Anagram:
                    retval = (!String.Equals(String.Empty, textBoxLettersOrRegex.Text));
                    if (retval)
                    {
                        chars = textBoxLettersOrRegex.Text;
                        foreach (char c in chars)
                        {
                            if (!char.IsLetter(c) && c != '.' && c != '?' && c != '*')
                                retval = false;
                        }
                    }
                    if (retval)
                    {
                        if (textBoxLettersOrRegex.Text.Contains('*'))
                        {
                            _minLen = textBoxLettersOrRegex.Text.Length - textBoxLettersOrRegex.Text.Count(c => c == '*');
                            _maxLen = 15;
                        }
                        else
                        {
                            _minLen = textBoxLettersOrRegex.Text.Length;
                            _maxLen = _minLen;
                        }
                    }
                    break;
                case eSearchOption.BackHooksRegex:
                case eSearchOption.FrontHooksRegex:
                case eSearchOption.Regex:
                case eSearchOption.RegexOfAlphagram:
                case eSearchOption.RegexPartial:
                    retval = (!String.Equals(String.Empty, textBoxLettersOrRegex.Text));
                    if (retval)
                    {
                        try
                        {
                            _rx = new System.Text.RegularExpressions.Regex(textBoxLettersOrRegex.Text, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            _rx.Match("test");
                        }
                        catch
                        {
                            retval = false;
                        }
                    }
                    if (retval)
                    {
                        // I do not know how to get min and max lengths of regex expressions
                        _minLen = 2;
                        _maxLen = 15;
                    }
                    break;
                case eSearchOption.StickyFirstLEtter:
                case eSearchOption.StickyLastLetter:
                    _minLen = 2;
                    _maxLen = 15;
                    break;
                case eSearchOption.Length:
                    retval = (numericUpDown1.Value <= numericUpDown2.Value);
                    if (retval)
                    {
                        _minLen = Convert.ToInt32(numericUpDown1.Value);
                        _maxLen = Convert.ToInt32(numericUpDown2.Value);
                    }
                    break;
                case eSearchOption.Points:
                    retval = (numericUpDown1.Value <= numericUpDown2.Value);
                    if (retval)
                    {
                        _minLen = 2;
                        _maxLen = 15;
                    }
                    break;
                case eSearchOption.RegexAt:
                    retval = (!String.Equals(String.Empty, textBoxLettersOrRegex.Text));
                    if (retval)
                    {
                        try
                        {
                            _rx = new System.Text.RegularExpressions.Regex(textBoxLettersOrRegex.Text, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                            _rx.Match("test");
                        }
                        catch
                        {
                            retval = false;
                        }
                    }
                    retval = retval && (numericUpDown1.Value + numericUpDown2.Value < 17);
                    if (retval)
                    {
                        _minLen = Convert.ToInt32(numericUpDown1.Value + numericUpDown2.Value - 1);
                        _maxLen = 15;
                    }
                    break;
                case eSearchOption.SubAnagram:
                    retval = (!String.Equals(String.Empty, textBoxLettersOrRegex.Text));
                    if (retval)
                    {
                        chars = textBoxLettersOrRegex.Text;
                        foreach (char c in chars)
                        {
                            if (!char.IsLetter(c))
                                retval = false;
                        }
                    }
                    if (retval)
                    {
                        _minLen = 2;
                        _maxLen = textBoxLettersOrRegex.Text.Length;
                    }
                    break;
                case eSearchOption.SubWordAt:
                    retval = retval && (numericUpDown1.Value + numericUpDown2.Value < 17);
                    if (retval)
                    {
                        _minLen = Convert.ToInt32(numericUpDown1.Value + numericUpDown2.Value - 1);
                        _maxLen = 15;
                    }
                    break;
                case eSearchOption.WordIsAdded:
                case eSearchOption.WordIsDeleted:
                    _minLen = 2;
                    _maxLen = 15;
                    break;
                case eSearchOption.Unselected:
                    retval = true;
                    break;
                default:
                    throw new NotImplementedException("Well, this looks bad on the programmer.");
            }
            return retval;
        }

        public bool IsConditionSet
        {
            get
            {
                bool retval = true;
                if (_searchOption == eSearchOption.Unselected)
                    retval = false;
                return retval;
            }
        }

        private System.Text.RegularExpressions.Regex _rx;
        private int _minLen;
        private int _maxLen;

        private void comboBoxSearchOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBoxSearchOption.SelectedIndex)
            {
                case 0:
                    _searchOption = eSearchOption.Unselected;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 1:
                    _searchOption = eSearchOption.Anagram;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Anagram letters:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 2:
                    _searchOption = eSearchOption.BackHooksRegex;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Regex expression:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 3:
                    _searchOption = eSearchOption.FrontHooksRegex;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Regex expression:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 4:
                    _searchOption = eSearchOption.Length;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = true;
                    labelNumericUpDown1.Text = "Minimum:";
                    numericUpDown1.Visible = true;
                    numericUpDown1.Minimum = 2;
                    numericUpDown1.Maximum = 15;
                    numericUpDown1.Value = Math.Max(2, Math.Min(15, numericUpDown1.Value));
                    labelNumericUpDown2.Visible = true;
                    labelNumericUpDown2.Text = "Maximum:";
                    numericUpDown2.Visible = true;
                    numericUpDown2.Minimum = 2;
                    numericUpDown2.Maximum = 15;
                    numericUpDown2.Value = Math.Max(2, Math.Min(15, numericUpDown2.Value));
                    break;
                case 5:
                    _searchOption = eSearchOption.Points;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = true;
                    labelNumericUpDown1.Text = "Minimum:";
                    numericUpDown1.Visible = true;
                    numericUpDown1.Minimum = 2;
                    numericUpDown1.Maximum = 150;
                    numericUpDown1.Value = Math.Max(2, Math.Min(150, numericUpDown1.Value));
                    labelNumericUpDown2.Visible = true;
                    labelNumericUpDown2.Text = "Maximum:";
                    numericUpDown2.Visible = true;
                    numericUpDown2.Minimum = 2;
                    numericUpDown2.Maximum = 150;
                    numericUpDown2.Value = Math.Max(2, Math.Min(150, numericUpDown2.Value));
                    break;
                case 6:
                    _searchOption = eSearchOption.Regex;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Regex expression:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 7:
                    _searchOption = eSearchOption.RegexAt;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Regex expression:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = true;
                    labelNumericUpDown1.Text = "Position:";
                    numericUpDown1.Visible = true;
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = 14;
                    numericUpDown1.Value = Math.Max(1, Math.Min(14, numericUpDown1.Value));
                    labelNumericUpDown2.Visible = true;
                    labelNumericUpDown2.Text = "Length:";
                    numericUpDown2.Visible = true;
                    numericUpDown2.Minimum = 2;
                    numericUpDown2.Maximum = 14;
                    numericUpDown2.Value = Math.Max(2, Math.Min(14, numericUpDown2.Value));
                    break;
                case 8:
                    _searchOption = eSearchOption.RegexOfAlphagram;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Regex expression:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 9:
                    _searchOption = eSearchOption.RegexPartial;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Regex expression:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 10:
                    _searchOption = eSearchOption.StickyFirstLEtter;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 11:
                    _searchOption = eSearchOption.StickyLastLetter;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 12:
                    _searchOption = eSearchOption.SubAnagram;
                    labelTextBox.Visible = true;
                    labelTextBox.Text = "Tile pool:";
                    textBoxLettersOrRegex.Visible = true;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 13:
                    _searchOption = eSearchOption.SubWordAt;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = true;
                    labelNumericUpDown1.Text = "Position:";
                    numericUpDown1.Visible = true;
                    numericUpDown1.Minimum = 1;
                    numericUpDown1.Maximum = 14;
                    numericUpDown1.Value = Math.Max(1, Math.Min(14, numericUpDown1.Value));
                    labelNumericUpDown2.Visible = true;
                    labelNumericUpDown2.Text = "Length:";
                    numericUpDown2.Visible = true;
                    numericUpDown2.Minimum = 2;
                    numericUpDown2.Maximum = 14;
                    numericUpDown2.Value = Math.Max(2, Math.Min(14, numericUpDown2.Value));
                    break;
                case 14:
                    _searchOption = eSearchOption.WordIsAdded;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
                case 15:
                    _searchOption = eSearchOption.WordIsDeleted;
                    labelTextBox.Visible = false;
                    textBoxLettersOrRegex.Visible = false;
                    labelNumericUpDown1.Visible = false;
                    numericUpDown1.Visible = false;
                    labelNumericUpDown2.Visible = false;
                    numericUpDown2.Visible = false;
                    break;
            }
        }

    }
}
