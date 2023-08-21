using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsAppWordTools
{
    public partial class FormAdHoc : Form
    {
        private ClassLexicon CurrentLexicon;

        public FormAdHoc()
        {
            InitializeComponent();
        }

        private void FormAdHoc_Load(object sender, EventArgs e)
        {
            MDIParentWordTools parent = (MDIParentWordTools)this.MdiParent;
            CurrentLexicon = parent.globalLexicon;
        }
        private void buttonGo_Click(object sender, EventArgs e)
        {
            GenerateDoubleSixesFrom12LetterWords();
        }
        private void GenerateDoubleSixesFrom12LetterWords()
        {
            this.textBoxInfo.Text = $"{Environment.NewLine}Started at {DateTime.Now.ToLongTimeString()}{Environment.NewLine}";
            foreach (string wordLen12 in CurrentLexicon.WordList[10])
            {
                char[] chars12 = wordLen12.ToCharArray();
                Array.Sort(chars12);
                string alphagram12 = new string(chars12);
                var anagrams12 = CurrentLexicon.GetAnagrams(alphagram12);
                if (String.Equals(wordLen12, anagrams12[0]) )
                {
                    var answers = GetSixPacks(wordLen12);
                    if (answers.Count == 1)
                    {
                        this.textBoxInfo.Text += $"{wordLen12}: {answers[0]}{Environment.NewLine}";
                    }
                }
            }
            this.textBoxInfo.Text += $"{Environment.NewLine}Finished at {DateTime.Now.ToLongTimeString()}";
        }
        private void TestMaddeners()
        {
            this.textBoxInfo.Text = $"{Environment.NewLine}Started at {DateTime.Now.ToLongTimeString()}{Environment.NewLine}";
            /*
            foreach (string wordLen12 in CurrentLexicon.WordList[10])
            {
                char[] chars12 = wordLen12.ToCharArray();
                Array.Sort(chars12);
                string alphagram12 = new string(chars12);
                var anagrams12 = CurrentLexicon.GetAnagrams(alphagram12);
                if (String.Equals(wordLen12, anagrams12[0]) )
                {
                    var answers = GetSixPacks(wordLen12);
                    if (answers.Count == 1)
                    {
                        this.textBoxInfo.Text += $"{wordLen12}: {answers[0]}{Environment.NewLine}";
                    }
                }
            }
            */
            var letters = "WYNSSALTTREK";
            if (InputBoxClass.InputBox("Enter 12 letters", "Letters:", ref letters) != DialogResult.OK)
            {
                return;
            }
            var answers = GetSixPacks(letters.ToUpper().Trim());
            this.textBoxInfo.Text += $"Letters: {letters}{Environment.NewLine}Answers:{Environment.NewLine}";
            foreach (var answer in answers)
            {
                this.textBoxInfo.Text += $"   {answer}{Environment.NewLine}";
            }
            this.textBoxInfo.Text += $"{Environment.NewLine}Finished at {DateTime.Now.ToLongTimeString()}";
        }
        private List<string> GetSixPacks(string letters) {
            List<string> list = new List<string>();
            letters = letters.ToUpper();
            int wordLen = 6;
            foreach (string word in CurrentLexicon.WordList[wordLen - 2])
            {
                if (ClassWord.IsSubAnagramMatch(word, letters))
                {
                    string leftovers = letters;
                    foreach (char letter in word)
                    {
                        leftovers = leftovers.Remove(leftovers.IndexOf(letter), 1);
                    }


                    char[] alphchars = leftovers.ToCharArray();
                    Array.Sort(alphchars);
                    string alphagram = new string(alphchars);
                    var anagrams = CurrentLexicon.GetAnagrams(alphagram);
                    foreach (string anagram in anagrams)
                    {
                        if (string.Compare(anagram, word) > 0)
                        {
                            list.Add($"{word} + {anagram}");
                        }
                    }
                }
            }
            return list;
        }
        private void TestMorphoGeneration()
        {
            this.textBoxInfo.Text = "Morpho puzzle generation";
            for (int i = 3; i < 9; i++)
            {
                DateTime startTime = DateTime.Now;
                MorphoPuzzle puzzle = new MorphoPuzzle(i + 1, i);
                puzzle.GeneratePuzzle();
                DateTime endTime = DateTime.Now;
                this.textBoxInfo.Text += Environment.NewLine + "i=" + i.ToString() + ":" + (endTime - startTime).ToString();
                this.textBoxInfo.Text += ", iterations=" + puzzle.NumIterations.ToString();
                if (string.IsNullOrEmpty(puzzle.Fail))
                {
                    this.textBoxInfo.Text += ", success:";
                    foreach (var row in puzzle.Rows)
                    {
                        this.textBoxInfo.Text += " " + row.Letters + "|";
                    }
                } else
                {
                    this.textBoxInfo.Text += ", fail: " + puzzle.Fail;
                }
            }

        }
        private void NoPlusBlankNoMinusBlank()
        {
            this.textBoxInfo.Text = $"{Environment.NewLine}Started at {DateTime.Now.ToLongTimeString()}";
            int wordLen = 7;
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = CurrentLexicon.WordList[wordLen - 2].Count;
            foreach (var word in CurrentLexicon.WordList[wordLen-2])
            {
                this.progressBar1.Increment(1);
                if (!CurrentLexicon.GetPlusBlankWords(word).Any() && !CurrentLexicon.GetMinusBlankWords(word).Any())
                {
                    this.textBoxInfo.Text += $"{Environment.NewLine}{word}";
                }
            }
            this.textBoxInfo.Text += $"{Environment.NewLine}Ended at {DateTime.Now.ToLongTimeString()}";
        }
        private void CarveIntoSlices(int wordLen, int numSlices)
        {
            this.textBoxInfo.Text = $"{Environment.NewLine}Started at {DateTime.Now.ToLongTimeString()}";
            string lenDesc;
            switch (wordLen)
            {
                case 2:
                    lenDesc = "Twos";
                    break;
                case 3:
                    lenDesc = "Threes";
                    break;
                case 4:
                    lenDesc = "Fours";
                    break;
                case 5:
                    lenDesc = "Fives";
                    break;
                case 6:
                    lenDesc = "Sixes";
                    break;
                case 7:
                    lenDesc = "Sevens";
                    break;
                case 8:
                    lenDesc = "Eights";
                    break;
                case 9:
                    lenDesc = "Nines";
                    break;
                case 10:
                    lenDesc = "Tens";
                    break;
                case 11:
                    lenDesc = "Elevens";
                    break;
                case 12:
                    lenDesc = "Twelves";
                    break;
                case 13:
                    lenDesc = "Thirteens";
                    break;
                case 14:
                    lenDesc = "Fourteens";
                    break;
                case 15:
                    lenDesc = "Fifteens";
                    break;
                default:
                    this.textBoxInfo.Text = "Something stupid happened with wordLen";
                    return;
            }
            this.textBoxInfo.Text += $"{Environment.NewLine}Slicing the {lenDesc} into {numSlices} slices.";
            SortedList alphagrams = new SortedList();
            foreach (string word in CurrentLexicon.WordList[wordLen-2])
            {
                var cword = CurrentLexicon.GetClassWordForKnownWord(word);
                string alphagram = cword.Alphagram;
                if(!alphagrams.Contains(alphagram))
                {
                    alphagrams.Add(alphagram, word);
                }
                else
                {
                    alphagrams[alphagram] += Environment.NewLine + word;
                }
            }
            for (int slice = 1; slice <= numSlices; slice++)
            {
                this.textBoxInfo.Text += $"{Environment.NewLine}Creating slice {slice} ...";
                Application.DoEvents();
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter($"Carved{lenDesc}{slice}.txt"))
                {
                    int subIterationCount = 0;
                    foreach (DictionaryEntry alphagramEntry in alphagrams)
                    {
                        subIterationCount++;
                        if (subIterationCount > numSlices)
                        {
                            subIterationCount = 1;
                        }
                        if (subIterationCount == slice)
                        {
                            sw.WriteLine(alphagramEntry.Value);
                        }
                    }
                }
                this.textBoxInfo.Text += " done.";
                Application.DoEvents();
            }
            this.textBoxInfo.Text += $"{Environment.NewLine}Finished at {DateTime.Now.ToLongTimeString()}";
        }
        private void GetAnaverbs()
        {
            bool endInE;
            string addS;
            string addES;
            string addD;
            string addED;
            string addING;
            string minusEaddING;
            this.textBoxInfo.Text += $"{Environment.NewLine}Started at {DateTime.Now.ToLongTimeString()}";
            for (int wordLen = 2; wordLen < 13; wordLen++)
            {
                foreach (var word in CurrentLexicon.WordList[wordLen - 2])
                {
                    endInE = word.EndsWith("E");
                    bool endInIE = word.EndsWith("IE");
                    addS = word + "S";
                    addES = word + "ES";
                    addD = word + "D";
                    addED = word + "ED";
                    addING = word + "ING";
                    minusEaddING = word.Substring(0, wordLen - 1) + "ING";
                    string minusIEaddYING = word.Substring(0, wordLen - 2) + "YING";
                    if (endInE && (CurrentLexicon.WordExists(addING) || CurrentLexicon.WordExists(minusEaddING)))
                    {
                        //bypass
                    }
                    else if (endInIE && CurrentLexicon.WordExists(minusIEaddYING))
                    {
                        //bypass
                    }
                    else
                    {
                        if (endInE)
                        {
                            Check(word, addS, addD, addING);
                            Check(word, addS, addD, minusEaddING);
                        }
                        Check(word, addS, addED, addING);
                        Check(word, addES, addED, addING);
                    }
                }
            }
        }
        private void Check(string word, string tense1, string tense2, string tense3)
        {
            if (CurrentLexicon.WordExists(tense1) && CurrentLexicon.WordExists(tense2) && !CurrentLexicon.WordExists(tense3))
            {
                char[] alphchars = tense3.ToCharArray();
                Array.Sort(alphchars);
                string alphagram = new string(alphchars);
                var anagrams = CurrentLexicon.GetAnagrams(alphagram);
                if (anagrams.Any())
                {
                    this.textBoxInfo.Text += $"{Environment.NewLine}{word}\t{tense1}\t{tense2}\t(not {tense3}*)\t";
                    bool first = true;
                    foreach (var anag in anagrams)
                    {
                        if (!first)
                        {
                            this.textBoxInfo.Text += ", ";
                        }
                        else
                        {
                            first = false;
                        }
                        this.textBoxInfo.Text += $"{anag}";
                    }
                }
            }
        }
        private void GetLetterPositionStats()
        {
            this.textBoxInfo.Text += $"{Environment.NewLine}Started at {DateTime.Now.ToLongTimeString()}";
            Application.DoEvents();
            int[,,] statsArray = new int[15, 15, 26]; //[wordLen-2,letterPosition-1,letter-'A']
            System.IO.StreamWriter sw;

            using (sw = new System.IO.StreamWriter("AdHocOutput.txt"))
            {
                sw.WriteLine("Length\tPosition\tLetter\tCount");
                for (int wordLen = 2; wordLen < 16; wordLen++)
                {
                    int offset1 = wordLen - 2;
                    foreach (var cword in CurrentLexicon.WordDictionary[offset1].Values)
                    {
                        for (int offset2 = 0; offset2 < cword.Length; offset2++)
                        {
                            int offset3 = cword.WordChars[offset2] - (int)'A';
                            statsArray[offset1, offset2, offset3]++;
                        }
                    }
                }
                for (int wordLen = 2; wordLen < 16; wordLen++)
                {
                    int offset1 = wordLen - 2;
                    for (int letterPosition = 1; letterPosition <= wordLen; letterPosition++)
                    {
                        int offset2 = letterPosition - 1;
                        foreach (char letter in ClassWord.Alphabet)
                        {
                            int offset3 = (int)letter - (int)'A';
                            sw.WriteLine($"{wordLen}\t{letterPosition}\t{letter}\t{statsArray[offset1, offset2, offset3]}");
                        }
                    }
                }
            }
            this.textBoxInfo.Text += $"{Environment.NewLine}Finished at {DateTime.Now.ToLongTimeString()}";
        }
    }
}
