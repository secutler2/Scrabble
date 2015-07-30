using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class Letter
    {

        public char AlphabeticalCharacter { get; set; }
        public int FrequencyOfLetter { get; set; }
        public int ValueOfLetter { get; set; }

        public Letter(char letter, int value, int frequency)
        {
            bool validFrequency = frequency >= 0;
            if (validFrequency)
            {
                AlphabeticalCharacter = letter;
                ValueOfLetter = value;
                FrequencyOfLetter = frequency;
            }
        }

        private void decrementFrequency(int frequency) {
            FrequencyOfLetter = frequency - 1;
        }
    }
}
