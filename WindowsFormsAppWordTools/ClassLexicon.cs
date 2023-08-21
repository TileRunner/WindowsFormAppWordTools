using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsAppWordTools
{
    public class ClassLexicon
    {
        private int _wordCount;
        public Dictionary<string, ClassWord>[] WordDictionary;
        public List<string>[] WordList;
        public Dictionary<string, string> UserNotes;
        public bool LoadedBaseLexicon;

        public ClassLexicon()
        {
            WordDictionary = new Dictionary<string, ClassWord>[14];
            WordList = new List<string>[14];
            for (int wordLength = 2; wordLength <= 15; wordLength++)
            {
                WordDictionary[wordLength - 2] = new Dictionary<string, ClassWord>();
                WordList[wordLength - 2] = new List<string>();
            }
            UserNotes = new Dictionary<string, string>();
            LoadedBaseLexicon = false;
        }

        public void CreateLexicon(string filename, string lexiconName, out int wordCount)
        {
            wordCount = 0;
            using (var sr = new System.IO.StreamReader(filename))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    int wordLength = line.Length;
                    if (!string.IsNullOrWhiteSpace(line) && wordLength > 1 && wordLength < 16)
                    {
                        line = line.ToUpper();
                        ClassWord cword = new ClassWord(line);
                        WordDictionary[wordLength - 2].Add(line, cword);
                        WordList[wordLength - 2].Add(line);
                        wordCount++;
                    }
                }
            }
            _wordCount = wordCount;
            string lexiconFilename = System.IO.Directory.GetCurrentDirectory() + "\\Lexicons\\" + lexiconName + ".lex";
            using (var sw = new System.IO.StreamWriter(lexiconFilename))
            {
                for (int wordLength = 2; wordLength <= 15; wordLength++)
                {
                    foreach (ClassWord cword in WordDictionary[wordLength - 2].Values)
                    {
                        cword.FrontHooks = GetFrontHooks(cword.Word);
                        cword.BackHooks = GetBackHooks(cword.Word);
                        sw.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4}|{5}", cword.Word, cword.Alphagram, cword.PointValue, cword.FrontHooks, cword.BackHooks, cword.Probability));
                    }
                }
            }
        }

        public void LoadLexicon(string lexiconName, out int wordCount)
        {
            wordCount = 0;
            string lexiconFilename = System.IO.Directory.GetCurrentDirectory() + "\\Lexicons\\" + lexiconName + ".lex";
            using (var sr = new System.IO.StreamReader(lexiconFilename))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    string[] lineitems = line.Split('|');
                    string word = lineitems[0];
                    string alphagram = lineitems[1];
                    int pointValue = Convert.ToInt32(lineitems[2]);
                    string frontHooks = lineitems[3];
                    string backHooks = lineitems[4];
                    long probabilty = Convert.ToInt64(lineitems[5]);
                    ClassWord cword = new ClassWord(word, alphagram, pointValue, frontHooks, backHooks, probabilty, ClassWord.eBaseLexiconState.Added);
                    WordDictionary[cword.Length - 2].Add(cword.Word, cword);
                    WordList[cword.Length - 2].Add(word);
                    wordCount++;
                }
            }
            _wordCount = wordCount;
            string lexiconUserNotesFilename = System.IO.Directory.GetCurrentDirectory() + "\\Lexicons\\" + lexiconName + ".uno";
            if ((new System.IO.FileInfo(lexiconUserNotesFilename)).Exists)
            {
                using (var sr = new System.IO.StreamReader(lexiconUserNotesFilename))
                {
                    while (sr.Peek() > 0)
                    {
                        string line = sr.ReadLine();
                        string[] lineitems = line.Split('|');
                        string word = lineitems[0];
                        string userNote = lineitems[1];
                        UserNotes.Add(word, userNote);
                    }
                }
            }
        }

        public void LoadBaseLexicon(string lexiconName, out int unchangedWordCount, out int deletedWordCount, out int addedWordCount)
        {
            unchangedWordCount = 0;
            deletedWordCount = 0;
            addedWordCount = _wordCount;
            string lexiconFilename = System.IO.Directory.GetCurrentDirectory() + "\\Lexicons\\" + lexiconName + ".lex";
            using (var sr = new System.IO.StreamReader(lexiconFilename))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    string[] lineitems = line.Split('|');
                    string word = lineitems[0];
                    if (WordExists(word))
                    {
                        ClassWord cwordexists = GetClassWordForKnownWord(word);
                        cwordexists.BaseLexiconState = ClassWord.eBaseLexiconState.Unchanged;
                        unchangedWordCount++;
                        addedWordCount--;
                    }
                    else
                    {
                        string alphagram = lineitems[1];
                        int pointValue = Convert.ToInt32(lineitems[2]);
                        string frontHooks = lineitems[3];
                        string backHooks = lineitems[4];
                        long probabilty = Convert.ToInt64(lineitems[5]);
                        ClassWord cword = new ClassWord(word, alphagram, pointValue, frontHooks, backHooks, probabilty, ClassWord.eBaseLexiconState.Deleted);
                        WordDictionary[cword.Length - 2].Add(cword.Word, cword);
                        WordList[cword.Length - 2].Add(word);
                        deletedWordCount++;
                    }
                }
            }
            this.LoadedBaseLexicon = true;
        }

        public bool WordExists(string word)
        {
            bool retval = false;
            int wordlen = word.Length;
            if (word.Length > 1 && word.Length < 16)
            {
                retval = WordDictionary[wordlen - 2].ContainsKey(word);
            }
            return retval;
        }

        public ClassWord GetClassWordForKnownWord(string word)
        {
            ClassWord retval;
            int wordLength = word.Length;
            WordDictionary[wordLength - 2].TryGetValue(word, out retval);
            return retval;
        }

        public List<string> GetAnagrams(string alphagram)
        {
            List<string> anagrams = new List<string>();
            foreach (ClassWord candidate in WordDictionary[alphagram.Length - 2].Values)
                if (string.Equals(alphagram, candidate.Alphagram))
                    anagrams.Add(candidate.Word);
            return anagrams;
        }

        public string GetFrontHooks(string word)
        {
            string frontHooks = "";
            int baseWordLength = word.Length;
            if (baseWordLength < 15)
            {
                foreach (char letter in ClassWord.Alphabet)
                {
                    if (WordDictionary[baseWordLength - 1].ContainsKey(letter + word))
                        frontHooks += letter;
                }
            }
            return frontHooks;
        }

        public string GetBackHooks(string word)
        {
            string backHooks = "";
            int baseWordLength = word.Length;
            if (baseWordLength < 15)
            {
                foreach (char letter in ClassWord.Alphabet)
                {
                    if (WordDictionary[baseWordLength - 1].ContainsKey(word + letter))
                        backHooks += letter;
                }
            }
            return backHooks;
        }

        public List<string> GetInserts(string word)
        {
            List<string> retval = new List<string>();
            int baseWordLength = word.Length;
            if (baseWordLength < 15)
            {
                StringBuilder candidateWord = new StringBuilder();
                for (int i = 0; i <= baseWordLength; i++)
                {
                    candidateWord.Clear();
                    if (i == 0)
                    {
                        candidateWord.Append('.');
                        candidateWord.Append(word);
                    }
                    else if (i < baseWordLength)
                    {
                        candidateWord.Append(word.Substring(0, i));
                        candidateWord.Append('.');
                        candidateWord.Append(word.Substring(i));
                    }
                    else
                    {
                        candidateWord.Append(word);
                        candidateWord.Append('.');
                    }
                    foreach (char letter in ClassWord.Alphabet)
                    {
                        candidateWord[i] = letter;
                        if (WordDictionary[baseWordLength - 1].ContainsKey(candidateWord.ToString())
                            && !retval.Contains(candidateWord.ToString()))
                        {
                            retval.Add(candidateWord.ToString());
                        }
                    }
                }
            }
            return retval;
        }

        public List<string> GetDrops(string word)
        {
            List<string> retval = new List<string>();
            int baseWordLength = word.Length;
            StringBuilder candidateWord = new StringBuilder();
            for (int i = 0; baseWordLength > 2 && i < baseWordLength; i++)
            {
                candidateWord.Clear();
                candidateWord.Append(word);
                candidateWord.Remove(i, 1);
                if (WordDictionary[baseWordLength - 3].ContainsKey(candidateWord.ToString())
                    && !retval.Contains(candidateWord.ToString()))
                {
                    retval.Add(candidateWord.ToString());
                }
            }
            return retval;
        }

        public List<string> GetSwaps(string word)
        {
            List<string> retval = new List<string>();
            StringBuilder candidateWord = new StringBuilder();
            int baseWordLength = word.Length;
            for (int i = 0; i < baseWordLength; i++)
            {
                foreach (char letter in ClassWord.Alphabet)
                {
                    candidateWord.Clear();
                    candidateWord.Append(word);
                    if (letter != candidateWord[i])
                    {
                        candidateWord[i] = letter;
                        if (WordDictionary[baseWordLength - 2].ContainsKey(candidateWord.ToString()))
                        {
                            retval.Add(candidateWord.ToString());
                        }
                    }
                }
            }
            return retval;
        }

        public List<string> GetPlusBlankWords(string word)
        {
            List<string> retval = new List<string>();
            int baseWordLength = word.Length;
            if (baseWordLength < 15)
            {
                foreach (char letter in ClassWord.Alphabet)
                {

                    var alphagramChars = (word+letter).ToCharArray();
                    Array.Sort(alphagramChars);
                    var alphagram = new string(alphagramChars);
                    retval.AddRange(GetAnagrams(alphagram));
                }
            }
            return retval.Distinct().ToList();
        }
        public List<string> GetMinusBlankWords(string word)
        {
            List<string> retval = new List<string>();
            int baseWordLength = word.Length;
            if (baseWordLength > 2)
            {
                StringBuilder interim = new StringBuilder();
                for (int i = 0; baseWordLength > 2 && i < baseWordLength; i++)
                {
                    interim.Clear();
                    interim.Append(word);
                    interim.Remove(i, 1);
                    var alphagramChars = interim.ToString().ToCharArray();
                    Array.Sort(alphagramChars);
                    var alphagram = new string(alphagramChars);
                    retval.AddRange(GetAnagrams(alphagram));
                }
            }
            return retval.Distinct().ToList();
        }
        #region Search me
        /// <summary>
        /// Use this so search the entire lexicon for words matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">Search criteria from the user control</param>
        /// <param name="supported">Whether the search option is coded for yet</param>
        /// <returns>A list of words that match the search criteria</returns>
        public List<string> SelectBySearchCriteria(UserControlSearchCriteria.StructSearchCriteria searchCriteria, out bool supported)
        {
            List<string> retval = new List<string>();
            supported = true;
            for (int wordLength = 2; supported && wordLength <= 15; wordLength++)
            {
                retval.AddRange(SelectBySearchCriteria(searchCriteria, WordList[wordLength - 2], out supported));
            }
            return retval;
        }

        /// <summary>
        /// Use this to search a list of candidate words for words matching the search criteria
        /// Option "sub word at" will check the entire lexicon to see if there is a valid word at the specified position in the candidate words
        /// Option "point value" will get the point value from the lexicon for candidate words
        /// </summary>
        /// <param name="searchCriteria">Search criteria from the user control</param>
        /// <param name="candidateWordList">Only look in this list for words matching the search criteria</param>
        /// <param name="supported">Whether the search option is coded for yet</param>
        /// <returns>A list of words from the candidate list that match the search criteria</returns>
        public List<string> SelectBySearchCriteria(UserControlSearchCriteria.StructSearchCriteria searchCriteria, List<string> candidateWordList, out bool supported)
        {
            List<string> hits = new List<string>();
            ClassWord knownCandidateCWord;
            supported = true;
            int pos = searchCriteria.SpecifiedSubMatchPosition;
            int len = searchCriteria.SpecifiedSubMatchLength;
            int offset = pos - 1;
            switch (searchCriteria.SearchOption)
            {
                case UserControlSearchCriteria.eSearchOption.Anagram:
                    var qAnagram = from string word in candidateWordList
                                   where searchCriteria.SpecifiedNot ^ ClassWord.IsAnagramMatch(word, searchCriteria.SpecifiedLetters.ToUpper())
                                   select word;
                    hits = qAnagram.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.BackHooksRegex:
                    System.Text.RegularExpressions.Regex bhRegex = new System.Text.RegularExpressions.Regex(searchCriteria.SpecifiedRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    foreach (string candidateWord in candidateWordList)
                    {
                        knownCandidateCWord = this.GetClassWordForKnownWord(candidateWord);
                        if (searchCriteria.SpecifiedNot ^ (bhRegex.IsMatch(knownCandidateCWord.BackHooks) && string.Equals(bhRegex.Match(knownCandidateCWord.BackHooks).Value, knownCandidateCWord.BackHooks)))
                            hits.Add(candidateWord);
                    }
                    break;
                case UserControlSearchCriteria.eSearchOption.FrontHooksRegex:
                    System.Text.RegularExpressions.Regex fhRegex = new System.Text.RegularExpressions.Regex(searchCriteria.SpecifiedRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    foreach (string candidateWord in candidateWordList)
                    {
                        knownCandidateCWord = this.GetClassWordForKnownWord(candidateWord);
                        if (searchCriteria.SpecifiedNot ^ (fhRegex.IsMatch(knownCandidateCWord.FrontHooks) && string.Equals(fhRegex.Match(knownCandidateCWord.FrontHooks).Value, knownCandidateCWord.FrontHooks)))
                            hits.Add(candidateWord);
                    }
                    break;
                case UserControlSearchCriteria.eSearchOption.Length:
                    var qLength = from string word in candidateWordList
                                  where searchCriteria.SpecifiedNot ^ (word.Length >= searchCriteria.SpecifiedMinimumWordLength && word.Length <= searchCriteria.SpecifiedMaximumWordLength)
                                  select word;
                    hits = qLength.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.Points:
                    var qPoints = from string word in candidateWordList
                                  where searchCriteria.SpecifiedNot ^ (GetClassWordForKnownWord(word).PointValue >= searchCriteria.SpecifiedMinimumPointValue && GetClassWordForKnownWord(word).PointValue <= searchCriteria.SpecifiedMaximumPointValue)
                                  select word;
                    hits = qPoints.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.Regex:
                    System.Text.RegularExpressions.Regex xRegex = new System.Text.RegularExpressions.Regex(searchCriteria.SpecifiedRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var qRegex = from string word in candidateWordList
                                 where searchCriteria.SpecifiedNot ^ (xRegex.IsMatch(word) && string.Equals(xRegex.Match(word).Value, word))
                                 select word;
                    hits = qRegex.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.RegexAt:
                    System.Text.RegularExpressions.Regex xRegexAt = new System.Text.RegularExpressions.Regex(searchCriteria.SpecifiedRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var qRegexAt = from string word in candidateWordList
                                   where searchCriteria.SpecifiedNot ^ (word.Length >= offset + len
                                   && xRegexAt.IsMatch(word.Substring(offset, len))
                                   && string.Equals(xRegexAt.Match(word.Substring(offset, len)).Value, word.Substring(offset, len)))
                                   select word;
                    hits = qRegexAt.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.RegexOfAlphagram:
                    System.Text.RegularExpressions.Regex xRegexOfAlphagram = new System.Text.RegularExpressions.Regex(searchCriteria.SpecifiedRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    foreach (string word in candidateWordList)
                    {
                        string alphagram = GetClassWordForKnownWord(word).Alphagram;
                        if (searchCriteria.SpecifiedNot ^ (xRegexOfAlphagram.IsMatch(alphagram) && string.Equals(xRegexOfAlphagram.Match(alphagram).Value, alphagram)))
                        {
                            hits.Add(word);
                        }
                    }
                    break;
                case UserControlSearchCriteria.eSearchOption.RegexPartial:
                    System.Text.RegularExpressions.Regex xRegexPartial = new System.Text.RegularExpressions.Regex(searchCriteria.SpecifiedRegex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    var qRegexPartial = from string word in candidateWordList
                                        where searchCriteria.SpecifiedNot ^ xRegexPartial.IsMatch(word)
                                        select word;
                    hits = qRegexPartial.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.StickyFirstLEtter:
                    var qStickyFirstLetter = from string word in candidateWordList
                                             where searchCriteria.SpecifiedNot ^ (word.Length > 2
                                             && !WordDictionary[word.Length-3].ContainsKey(word.Substring(1,word.Length-1)))
                                             select word;
                    hits = qStickyFirstLetter.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.StickyLastLetter:
                    var qStickyLastLetter = from string word in candidateWordList
                                            where searchCriteria.SpecifiedNot ^ (word.Length > 2
                                            && !WordDictionary[word.Length - 3].ContainsKey(word.Substring(0, word.Length - 1)))
                                            select word;
                    hits = qStickyLastLetter.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.SubAnagram:
                    var qSubAnagram = from string word in candidateWordList
                                      where searchCriteria.SpecifiedNot ^ ClassWord.IsSubAnagramMatch(word, searchCriteria.SpecifiedLetters.ToUpper())
                                      select word;
                    hits = qSubAnagram.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.SubWordAt:
                    var qSubWordAt = from string word in candidateWordList
                                     where searchCriteria.SpecifiedNot ^ (word.Length >= offset + len
                                     && WordDictionary[len - 2].ContainsKey(word.Substring(offset, len)))
                                     select word;
                    hits = qSubWordAt.ToList();
                    break;
                case UserControlSearchCriteria.eSearchOption.WordIsAdded:
                    if (this.LoadedBaseLexicon)
                    {
                        foreach (string word in candidateWordList)
                        {
                            if (searchCriteria.SpecifiedNot ^ GetClassWordForKnownWord(word).BaseLexiconState == ClassWord.eBaseLexiconState.Added)
                            {
                                hits.Add(word);
                            }
                        }
                    }
                    break;
                case UserControlSearchCriteria.eSearchOption.WordIsDeleted:
                    if (this.LoadedBaseLexicon)
                    {
                        foreach (string word in candidateWordList)
                        {
                            if (searchCriteria.SpecifiedNot ^ GetClassWordForKnownWord(word).BaseLexiconState == ClassWord.eBaseLexiconState.Deleted)
                            {
                                hits.Add(word);
                            }
                        }
                    }
                    break;
                default:
                    supported = false;
                    break;
            }
            return hits;
        }
        #endregion
    }
}
