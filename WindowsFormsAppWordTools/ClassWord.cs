using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppWordTools
{
    public class ClassWord
    {
        public static char[] Alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        private static int[] _tileDistribution = new int[27] { 9, 2, 2, 4, 12, 2, 3, 2, 9, 1, 1, 4, 2, 6, 8, 2, 1, 6, 4, 6, 4, 2, 2, 1, 2, 1, 2 };
        private static int[] _tileValue = new int[27] { 1, 3, 3, 2, 1, 4, 2, 4, 1, 8, 5, 1, 3, 1, 1, 3, 10, 1, 1, 1, 1, 4, 4, 8, 4, 10, 0 };

        public static bool IsAnagramMatch(string word, string anagramExpression)
        {
            bool retval = false;
            int minLen = anagramExpression.Length;
            int maxLen = minLen;
            if (anagramExpression.Contains('*'))
            {
                maxLen = 15;
                minLen -= anagramExpression.Count(c => c == '*');
            }
            if (word.Length >= minLen && word.Length <= maxLen)
            {
                string knownLetters = string.Empty;
                foreach (char c in anagramExpression)
                {
                    if (char.IsLetter(c))
                    {
                        knownLetters += c;
                    }
                }
                retval = IsSubAnagramMatch(knownLetters, word);
            }
            return retval;
        }

        public static bool IsSubAnagramMatch(string word, string tiles)
        {
            bool retval = false;
            Dictionary<char, int> tilebag = new Dictionary<char, int>();
            foreach (char c in tiles)
            {
                retval = true;
                if (tilebag.ContainsKey(c))
                {
                    tilebag[c] += 1;
                }
                else
                {
                    tilebag.Add(c, 1);
                }
            }
            foreach (char letter in word)
            {
                if (!tilebag.ContainsKey(letter))
                    retval = false;
                else
                {
                    int remain = tilebag[letter];
                    tilebag[letter] = remain - 1;
                    if (remain < 1)
                    {
                        retval = false;
                    }
                }
            }
            return retval;
        }

        private string _word;
        private char[] _wordChars;
        private string _alphagram;
        private int _pointValue;
        private string _frontHooks;
        private string _backHooks;
        private long _probability;
        public enum eBaseLexiconState
        {
            Undetermined,
            Added,
            Deleted,
            Unchanged
        }

        private eBaseLexiconState _baseLexiconState;

        public ClassWord(string word)
        {
            _word = word.ToUpper();
            _wordChars = _word.ToCharArray();
            var alphagramChars = _word.ToCharArray();
            Array.Sort(alphagramChars);
            _alphagram = new string(alphagramChars);
            _pointValue = CalcPointValue();
            _frontHooks = string.Empty;
            _backHooks = string.Empty;
            _probability = CalcProbTwoBlanks(_alphagram);
            _baseLexiconState = eBaseLexiconState.Undetermined;
        }

        public ClassWord(string word, string alphagram, int pointValue, string frontHooks, string backHooks, long probabilty, eBaseLexiconState baseLexiconState)
        {
            _word = word;
            _wordChars = _word.ToCharArray();
            _alphagram = alphagram;
            _pointValue = pointValue;
            _probability = probabilty;
            _frontHooks = frontHooks;
            _backHooks = backHooks;
            _baseLexiconState = baseLexiconState;
        }

        public string Word
        { get { return _word; } }

        public char[] WordChars
        { get { return _wordChars; } }

        public string Alphagram
        { get { return _alphagram; } }

        public int PointValue
        { get { return _pointValue; } }

        public string FrontHooks
        {
            get { return _frontHooks; }
            set { _frontHooks = value; }
        }

        public string BackHooks
        {
            get { return _backHooks; }
            set { _backHooks = value; }
        }

        public long Probability
        { get { return _probability; } }

        public eBaseLexiconState BaseLexiconState
        {
            get { return _baseLexiconState; }
            set { _baseLexiconState = value; }
        }

        public int Length
        { get { return _word.Length; } }

        public string CentredWord(int width)
        {
            int leftPadding = (width - _word.Length) / 2;
            int rightPadding = width - _word.Length - leftPadding;
            return new string(' ', leftPadding) + _word + new string(' ', rightPadding);
        }

        public static string GetAlphagram(string word)
        {
            char[] chars = word.ToCharArray();
            Array.Sort(chars);
            return new string(chars);
        }

        private int CalcPointValue()
        {
            int retval = 0;
            bool ok = true;
            int[] used = new int[26];
            int blanks = 0;
            var wordBytes = ASCIIEncoding.ASCII.GetBytes(_word);
            foreach (byte b in wordBytes)
            {
                if (b < 65 || b > 91)
                    ok = false;
                else
                {
                    used[b - 65]++;
                    if (used[b - 65] > _tileDistribution[b - 65])
                        blanks++;
                    else
                        retval += _tileValue[b - 65];
                }
            }
            if (!ok || blanks > 2)
                retval = 0;
            return retval;
        }

        #region Probablity - Adapted from code supplied courtesy of Allen Pengelly

        private long CalcProb(string rack)
        {

            int[] rackval = new int[27];

            byte[] array = Encoding.ASCII.GetBytes(rack);

            int oldelement = 0;

            // Loop through contents of the array.
            foreach (byte element in array)
            {

                if (element < 65 | element > 91)
                {
                    return -1;
                }

                else
                {
                    if (element < oldelement)
                        return -2;
                    else
                        rackval[element - 65]++;
                }

                oldelement = element;
            }

            long mult = 1;

            for (int i = 0; i <= 26; i++)
            {
                if (rackval[i] > _tileDistribution[i])
                    return 0;
                else
                    mult = mult * Factorial(_tileDistribution[i]) / Factorial(rackval[i]) / Factorial(_tileDistribution[i] - rackval[i]);
            }

            return mult;

        }

        private int Factorial(int n)
        {
            int f = 1;
            for (int i = 2; i <= n; i++)
            {
                f = f * i;
            }
            return f;
        }

        private long CalcProbOneBlank(string rack)
        {
            long prob = CalcProb(rack);

            if (prob >= 0)
            {

                string CompStr = "";

                for (int i = 0; i < rack.Length; i++)
                {
                    if (i == 0 || rack.Substring(i, 1) != rack.Substring(i - 1, 1))
                    {
                        if (i == 0)
                        {
                            CompStr = rack.Substring(1, rack.Length - 1) + "[";
                        }
                        else if (i == rack.Length - 1)
                        {
                            CompStr = rack.Substring(0, rack.Length - 1) + "[";
                        }
                        else
                        {
                            CompStr = rack.Substring(0, i);
                            CompStr = CompStr + rack.Substring(i + 1, rack.Length - i - 1);
                            CompStr = CompStr + "[";
                        }

                        prob = prob + CalcProb(CompStr);

                    }
                }
            }

            return prob;

        }

        private long CalcProbTwoBlanks(string rack)
        {
            long prob = CalcProb(rack);

            if (prob >= 0)
            {
                prob = CalcProbOneBlank(rack);

                for (int i = 0; i < rack.Length - 1; i++)
                {
                    if (i == 0 || rack.Substring(i, 1) != rack.Substring(i - 1, 1))
                    {
                        for (int j = i + 1; j < rack.Length; j++)
                        {

                            if (j == i + 1 || rack.Substring(j, 1) != rack.Substring(j - 1, 1))
                            {
                                string CompStr = rack.Substring(0, i);
                                CompStr = CompStr + rack.Substring(i + 1, j - i - 1);
                                CompStr = CompStr + rack.Substring(j + 1, rack.Length - j - 1);
                                CompStr = CompStr + "[[";

                                prob = prob + CalcProb(CompStr);
                            }
                        }
                    }
                }

            }

            return prob;

        }


        #endregion
    }
}
