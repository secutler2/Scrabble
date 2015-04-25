using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class Word
    {
        private string wordName;
        private int wordScore;

        public string WordName
        {
            get { return wordName; }
            set { wordName = value; }
        }

        public int WordScore
        {
            get { return wordScore; }
            set { wordScore = value; }
        }

        public Word(string wordName, int wordScore) {
            WordName = wordName;
            WordScore = wordScore;
        }

        public override string ToString()
        {
            return wordName + " " + wordScore;
        }
    }
}
