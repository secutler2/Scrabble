using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble
{
    public partial class Form1 : Form
    {
        private Board board;

        public Board Board
        {
            get { return board; }
            set { board = value; }
        }

        public Form1()
        {
            InitializeComponent();
            Board = boardEvents1;
        }

        private void SwitchPlayers() {
            if (Board.Player1.IsTurn == true)
            {
                Board.Player1.IsTurn = false;
                Board.Player2.IsTurn = true;
            }
            else
            {
                Board.Player1.IsTurn = true;
                Board.Player2.IsTurn = false;
            }
        }

        private void updateGamePieces() {
            if (Board.Player1.IsTurn)
            {
                board.Player1.updateGamePieces();
            }
            else
            {
                board.Player2.updateGamePieces();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GamePiece square = Board.FindMostRecentLetter();
            Board.FindMostRecentRows(square);
            Board.ProcessMostRecentRow(square, Board.RecentHorizontalWord, true);
            Board.ProcessMostRecentRow(square, Board.RecentVerticalWord, false);
         
            //TODO need to prevent user from adding 2 words on one turn.
            Board.EvaluateBlankWord(Board.RecentHorizontalWord, Board.RecentVerticalWord);

            bool wordExists = false;
            if (Board.lookUp(Board.ToWord(Board.RecentHorizontalWord)))
            {
                Board.RecentWord = Board.RecentHorizontalWord;
                Board.FinalizeTiles(Board.getGamePieceTiles(Board.RecentHorizontalWord));
                Console.WriteLine(Board.calculateRecentWord(Board.RecentHorizontalWord));
                wordExists = true;
                SwitchPlayers();
            }
            else {
                if (Board.lookUp(Board.ToWord(Board.RecentVerticalWord)))
                {
                    Board.RecentWord = Board.RecentHorizontalWord;
                    Board.FinalizeTiles(Board.getGamePieceTiles(Board.RecentVerticalWord));
                    Console.WriteLine(Board.calculateRecentWord(Board.RecentVerticalWord));
                    wordExists = true;
                    SwitchPlayers();
                }
            }

            if (!wordExists)
            {
                Console.WriteLine("No Such Word Exists");
      
            }
            Board.Clear();
            square = null;
        }
    }
}
