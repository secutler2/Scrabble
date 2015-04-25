using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrabble
{
    public class Player
    {
        private bool isTurn;
        private int score;
        private char[] letters;
        private int numberOfLetters;
        private int id;
        private static int numberOfPlayers = 0;
        private static Letter[] allLetters = new Letter[27];
        private Square[] boardPieces;
        private int gamePieceNumber;
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        
        private static int RandomNumber(int min, int max)
        {
            lock (syncLock)
            { 
                return random.Next(min, max);
            }
        }

        public int GamePieceNumber
        {
            get { return gamePieceNumber; }
            set { gamePieceNumber = value; }
        }

        public bool IsTurn
        {
            get { return isTurn; }
            set { isTurn = value; }
        }

        public static Letter[] AllLetters
        {
            get { return Player.allLetters; }
        }

        public int Score
        {
            get { return score; }
            set { score = value; }
        }

        public char[] Letters
        {
            get { return letters; }
            set { letters = value; }
        }

        public int NumberOfLetters
        {
            get { return numberOfLetters; }
            set { numberOfLetters = value; }
        }

        public int Id
        {
            get { return id; }
        }


        public void setGamePieces(Square gamePiece) {
            boardPieces[gamePieceNumber] = gamePiece;
            gamePieceNumber++;
        }

        public void updateGamePieces() {
            List<Square> newBoardPieces = new List<Square>();
            foreach (Square square in boardPieces) {
                if (square.IsSelectable) {
                    newBoardPieces.Add(square);
                }
            }
            boardPieces = new Square[7];
            GamePieceNumber = 0;
            foreach (Square square in newBoardPieces) {
                setGamePieces(square);
            }
        }

        public Square[] getGamePieces()
        {
            return boardPieces;
        }

        public Player(int score, char[] letters, int numberOfLetters)
        {
            Score = score;
            Letters = letters;
            NumberOfLetters = numberOfLetters;
            id = numberOfPlayers++;
            boardPieces = new Square[7];
            IsTurn = false;
        }

        public static void initializeLetters()
        {
            allLetters[0] = new Letter(('A'), 1, 9);
            allLetters[1] = new Letter(('B'), 3, 2);
            allLetters[2] = new Letter(('C'), 3, 2);
            allLetters[3] = new Letter(('D'), 2, 4);
            allLetters[4] = new Letter(('E'), 1, 12);
            allLetters[5] = new Letter(('F'), 4, 2);
            allLetters[6] = new Letter(('G'), 2, 3);
            allLetters[7] = new Letter(('H'), 4, 2);
            allLetters[8] = new Letter(('I'), 1, 9);
            allLetters[9] = new Letter(('J'), 8, 1);
            allLetters[10] = new Letter(('K'), 5, 1);
            allLetters[11] = new Letter(('L'), 1, 4);
            allLetters[12] = new Letter(('M'), 3, 2);
            allLetters[13] = new Letter(('N'), 1, 6);
            allLetters[14] = new Letter(('O'), 1, 8);
            allLetters[15] = new Letter(('P'), 3, 2);
            allLetters[16] = new Letter(('Q'), 10, 1);
            allLetters[17] = new Letter(('R'), 1, 6);
            allLetters[18] = new Letter(('S'), 1, 4);
            allLetters[19] = new Letter(('T'), 1, 6);
            allLetters[20] = new Letter(('U'), 1, 4);
            allLetters[21] = new Letter(('V'), 4, 2);
            allLetters[22] = new Letter(('W'), 4, 2);
            allLetters[23] = new Letter(('X'), 8, 1);
            allLetters[24] = new Letter(('Y'), 4, 2);
            allLetters[25] = new Letter(('Z'), 10, 1);
            allLetters[26] = new Letter(('_'), 1, 2);//blank letter value should be worth 0 but it gives me error so I set to 1.
        }

        /// <summary>
        /// Generates letters for player
        /// </summary>
        /// <param name="tilesNeeded"></param>
        /// <returns></returns>
        //public Letter[] GenerateLetters(int tilesNeeded)
        public char[] GenerateLetters(int tilesNeeded)
        {
            //Random random = new Random();
            //Letter[] letters = new Letter[tilesNeeded];
            int tilesChosen = 0;
            char[] tiles = new char[tilesNeeded];
            int numberOfTiles = allLetters.Sum(freq => freq.Value);
            if ((tilesNeeded > 0) && (tilesNeeded <= 7))
            {
                if (numberOfTiles > 0)
                {
                    while (tilesNeeded > 0)
                    {
                        //int number = random.Next(0, 27);
                        int number = RandomNumber(0, 27);
                        char letter = allLetters[number].Character;
                        int frequency = allLetters[number].Frequency;
                        //check if letter is available
                        if (frequency >= 1)
                        {
                            tiles[tilesChosen] = letter;
                            allLetters[number].Frequency = allLetters[number].Frequency - 1;
                            tilesNeeded--;
                            tilesChosen++;
                            numberOfTiles = allLetters.Sum(x => x.Frequency);
                        }


                        //else if (frequency == 0)
                        //{
                        //    Console.WriteLine(letter + " Last Character!");
                        //}
                        //else
                        //{
                        //    Console.WriteLine(letter + " Not a valid character!");
                        //}
                    }

                }
            }
            return tiles;
        }
    }
}
