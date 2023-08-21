using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WindowsFormsAppWordTools
{
    public class MorphoRow
    {
        public string Letters { get; set; }
    }
    public class MorphoPuzzle
    {
        public int NumRows { get; set; }
        public int NumCols { get; set; }
        public List<MorphoRow> Rows { get; set; }
        public string Fail { get; set; }
        public int NumIterations { get; set; }
        public MorphoPuzzle(int numRows, int numCols)
        {
            NumRows = numRows;
            NumCols = numCols;
            Rows = new List<MorphoRow>();
            for (int rowOffset = 0; rowOffset < numRows; rowOffset++)
            {
                MorphoRow row = new MorphoRow { Letters = "" };
                Rows.Add(row);
            }
        }
        public void GeneratePuzzle()
        {
            Fail = null;
            NumIterations = 0;
            Random random = new Random();
            var wordssamelength = WordDatabase.WordsOfLength(NumCols);
            bool timeout = false;
            DateTime startTime = DateTime.Now;
            int rowIndex = 0;
            List<Dictionary<string, bool>> candidatesByRow = new List<Dictionary<string, bool>>();
            for (int i = 0; i < NumRows; i++)
            {
                candidatesByRow.Add(new Dictionary<string, bool>());
            }
            candidatesByRow[0] = wordssamelength;
            while (rowIndex > -1 && rowIndex < NumRows && !timeout)
            {
                DateTime currentTime = DateTime.Now;
                TimeSpan timeSpan = currentTime - startTime;
                timeout = timeSpan.Ticks > 300000000; // 30 seconds
                if (timeout)
                {
                    break;
                }
                NumIterations++;
                if (candidatesByRow[rowIndex].Count == 0)
                {
                    rowIndex--;
                }
                else
                {
                    int rand = random.Next(candidatesByRow[rowIndex].Count - 1);
                    string newRow = (candidatesByRow[rowIndex]).ElementAt(rand).Key;
                    candidatesByRow[rowIndex].Remove(newRow);
                    // Avoid words ending in S for shorter puzzles to make the puzzles more interesting / challenging.
                    // Allow it for longer puzzles so it finds a puzzle sooner.
                    if (!(NumCols < 6 && rowIndex == 0 && newRow[NumCols - 1] == 's'))
                    {
                        Rows[rowIndex].Letters = newRow;
                        rowIndex++; // Ends the loop if all rows were filled
                        if (rowIndex < NumRows)
                        {
                            // Prep candidates for next row
                            var swaps = WordFunctions.GetSwapWords(newRow, wordssamelength);
                            candidatesByRow[rowIndex] = new Dictionary<string, bool>();
                            foreach (var swap in swaps)
                            {
                                if (ValidNextMorph(Rows[0].Letters, rowIndex, newRow, swap))
                                {
                                    candidatesByRow[rowIndex].Add(swap, true);
                                }
                            }
                        }
                    }
                }
            }
            if (timeout)
            {
                Fail = "Program quit to protect against infinite looping.";
            }
            else if (rowIndex < 0)
            {
                Fail = String.Format("Cannot seem to make a puzzle {0} rows by {1} columns.", NumRows, NumCols);
            }
        }

        private bool ValidNextMorph(string startWord, int requiredDiffLetterCount, string previousWord, string currentWord)
        {
            // A valid morph changes one letter from the previous word, but not at the same position as a previous morph.

            // Start word is row index 0
            // Word at row index 1 must have 1 letter swap
            // Word at row index 2 must have 2 letter swaps relative to the start word, and 1 relative to previous word
            // Word at row index 3 must have 3 letter swaps relative to the start word, and 1 relative to previous word
            // Etc. So pass row index to requiredDiffLetterCount.

            // Avoid words ending in S for shorter puzzles to make the puzzles more interesting / challenging.
            // Allow it for longer puzzles so it finds a puzzle sooner.
            if (currentWord.Length < 6 && currentWord[currentWord.Length - 1] == 's')
            {
                return false;
            }
            int diffFromStartCount = WordFunctions.CountSwaps(startWord, currentWord);
            int diffFromPreviousCount = WordFunctions.CountSwaps(previousWord, currentWord);
            return (diffFromStartCount == requiredDiffLetterCount && diffFromPreviousCount == 1);
        }

    }
    public static class WordFunctions
    {
        private static readonly string alphabet = "abcdefghijklmnopqrstuvwxyz";

        public static bool HasLetters(List<char> lettersRequired, string wordToCheck)
        {
            if (lettersRequired.Count > wordToCheck.Length)
            {
                return false;
            }
            foreach (char letter in lettersRequired)
            {
                int required = 0;
                int actual = 0;
                foreach (char letter2 in lettersRequired)
                {
                    if (char.ToLowerInvariant(letter) == char.ToLowerInvariant(letter2))
                    {
                        ++required;
                    }
                }
                foreach (char letter2 in wordToCheck)
                {
                    if (char.ToLowerInvariant(letter) == char.ToLowerInvariant(letter2))
                    {
                        ++actual;
                        if (actual == required)
                        {
                            break;
                        }
                    }
                }
                if (actual < required)
                {
                    return false;
                }
            }
            return true;
        }
        public static bool StringMatchesStringBuilder(string s, StringBuilder sb)
        {
            bool match = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != sb[i])
                {
                    match = false;
                    break;
                }
            }
            return match;
        }
        public static List<string> GetSwapWords(string word, Dictionary<string,bool> wordssamelength)
        {
            List<string> swaps = new List<string>();
            StringBuilder testword = new StringBuilder(word.Length);
            for (var i = 0; i < word.Length; i++)
            {
                for (var j = 0; j < 26; j++)
                {
                    if (word[i] != alphabet[j])
                    {
                        testword.Clear();
                        testword.Append(word);
                        testword[i] = alphabet[j];
                        string tw = testword.ToString();
                        if (wordssamelength.ContainsKey(tw))
                        {
                            if (!swaps.Exists(s => StringMatchesStringBuilder(s, testword)))
                            {
                                swaps.Add(tw);
                            }
                        }
                    }
                }
            }
            return swaps;
        }
        public static int CountSwaps(string word1, string word2)
        {
            int result = 0;
            if (word1.Length == word2.Length)
            {
                for (int i = 0; i < word1.Length; i++)
                {
                    if (word1[i] != word2[i])
                    {
                        result++;
                    }
                }
            }
            else
            {
                result = -1;
            }
            return result;
        }
    }
    public static class WordDatabase
    {
        private const string filepath = @"C:\Users\User\source\repos\ScrabbleClub\WebScrabbleClub\WebAppScrabbleClub\wwwroot\ENABLE2K_word_list.txt";
        private static readonly char[] tiles = "aaaaaaaaabbccddddeeeeeeeeeeeeffggghhiiiiiiiiijkllllmmnnnnnnooooooooppqrrrrrrssssttttttuuuuvvwwxyyz".ToCharArray();
        //private const int numwords = 173226;
        public static bool WordExists(string word)
        {
            bool exists = false;
            using (var sr = new System.IO.StreamReader(filepath))
            {
                while (sr.Peek() > 0 && !exists)
                {
                    string line = sr.ReadLine();
                    exists = string.Compare(line, word, true) == 0;
                }
            }
            return exists;
        }
        public static Dictionary<string,bool> WordsOfLength(int length)
        {
            Dictionary<string, bool> words = new Dictionary<string, bool>();
            using (var sr = new StreamReader(filepath))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    if (line.Length == length)
                    {
                        words.Add(line, true);
                    }
                }
            }
            return words;
        }
        /// <summary>
        /// Pick a random word from the lexicon, at least minLength long, and return that word scrambled
        /// </summary>
        /// <param name="minLength">Pick a random word at least this long</param>
        /// <returns>Return the chosen random word with its letters randomly rearranged</returns>
        public static string PickFybTiles(int minLength)
        {
            string chosen = "urp"; // In case the logic I found online and retrofitted is wrong
            int numberSeen = 0;
            var rng = new Random();
            using (var sr = new StreamReader(filepath))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    // This approach avoids loading the entire word list into memory
                    // The first line is always chosen, second line 1/2 times, third line 1/3 times...
                    // For example, if you had only 3 words in the list, each word has a 1/3 chance of being chosen:
                    /* Pick first word (always)
                     *  Pick second word (half the time)
                     *   Pick third word (a third of the time)  ... 1/3 of 1/2 is 1/6 = 1/6 times replacing second with third
                     *   Do not pick third word                 ... 2/3 of 1/2 is 2/6 = 1/3 times keeping second word
                     *  Do not pick second word (half the time)
                     *   Pick third word (a third of the time)  ... 1/3 of 1/2 is 1/6 = 1/6 times replacing first with third
                     *   Do not pick third word                 ... 2/3 of 1/2 is 2/6 = 1/3 times keeping first word
                     * */
                    if (line.Length >= minLength && rng.Next(++numberSeen) == 0) // Ignore words shorter than guaranteed minimum length
                    {
                        chosen = line;
                    }
                }
            }
            // Now read all words that are longer than chosen and have all the letters in chosen, so we have a short list to pick
            // more tiles against
            List<char> chosenLettersList = chosen.ToList();
            List<string> shortlist = new List<string>();
            using (var sr = new StreamReader(filepath))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    if (WordFunctions.HasLetters(chosenLettersList, line))
                    {
                        shortlist.Add(line);
                    }
                }
            }
            // Now keep picking an extra letter until the letter picked cannot be used
            bool picking = true;
            while (picking)
            {
                char nextletter = tiles[rng.Next(tiles.Length - 1)];
                chosenLettersList.Add(nextletter);
                bool found = false;
                foreach (var item in shortlist)
                {
                    if (WordFunctions.HasLetters(chosenLettersList, item))
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    picking = false;
                    chosenLettersList.RemoveAt(chosenLettersList.Count - 1); // Unpick the failing letter
                }
            }
            string randomizedLetters = new string(chosenLettersList.OrderBy(s => (rng.Next(2) % 2) == 0).ToArray());
            return randomizedLetters;
        }
        private class TopAnswerComparer : Comparer<string>
        {
            public override int Compare(string a, string b)
            {
                if (a.Length != b.Length)
                {
                    return a.Length.CompareTo(b.Length);
                }
                return a.CompareTo(b);
            }
        }
        public static string[] GetTopAnswers(string letters, int numWanted)
        {
            List<char> chosenLettersList = letters.ToList();
            List<string> sortedList = new List<string>();
            using (var sr = new StreamReader(filepath))
            {
                while (sr.Peek() > 0)
                {
                    string line = sr.ReadLine();
                    if (WordFunctions.HasLetters(chosenLettersList, line))
                    {
                        sortedList.Add(line);
                    }
                }
            }
            sortedList.Sort(new TopAnswerComparer());
            return sortedList.Take(numWanted).ToArray();
        }
    }
}
