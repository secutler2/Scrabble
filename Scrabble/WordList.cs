using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class WordList
    {
        public Dictionary<int, string> DictionaryOfScrabbleWords { get; private set; }

        public WordList()
        {
            DictionaryOfScrabbleWords = new Dictionary<int, string>();
        }

        public void readFile(string fileToRead)
        {
            int iterations = 0;
            foreach (var word in File.ReadAllLines(fileToRead))
            {
                DictionaryOfScrabbleWords.Add(iterations, word);
                iterations++;
            }
        }
    }
}
