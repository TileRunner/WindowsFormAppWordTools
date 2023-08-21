using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppWordTools
{
    public class ClassListBoxItem
    {
        public enum eItemType
        {
            alphagram,
            word,
            info,
            usernote,
            specialSearchResult
        }
        private string _display;
        private string _value;
        private eItemType _itemType;
        private int _leadingTabCount;

        public ClassListBoxItem(string display, string value, eItemType itemType)
        {
            _display = display;
            _value = value;
            _itemType = itemType;
            _leadingTabCount = 0; // _display.ToArray().Count(c => char.Equals(c, '\t'));
            for (int i = 0; i < _display.Length && char.Equals(_display[i], '\t'); i++)
            {
                _leadingTabCount++;
            }
        }

        public string Display
        {
            get { return _display; }
        }

        public string Value
        {
            get { return _value; }
        }

        public eItemType ItemType
        {
            get { return _itemType; }
        }

        public int LeadingTabCount
        {
            get { return _leadingTabCount; }
        }
    }
}
