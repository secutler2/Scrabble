using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class Letter
    {
        private char letter;
        private int value;
        private int frequency;

        public char Character
        {
            get { return letter; }
            set { letter = value; }
        }

        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public int Frequency
        {
            get { return frequency; }
            set { frequency = value; }
        }

        public Letter(char letter, int value, int frequency)
        {
            if (frequency >= 0)
            {
                Character = letter;
                Value = value;
                Frequency = frequency;
            }
            else {
                Console.WriteLine("Out of " + letter);
            }
        }

        private void decrementFrequency(int frequency) {
            Frequency = frequency - 1;
        }
    }
}
