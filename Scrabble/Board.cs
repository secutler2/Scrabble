using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Scrabble
{
    [ToolboxItem(true)]
    [Docking(DockingBehavior.Ask)]
    public partial class Board : UserControl
    {
        private const int BoardPieceCount = 15;
        private const int BoardMovingPieceCount = 7;
        private const int PieceWidth = 30, PieceHeight = 30, PiecePadding = 1;
        private const int movingPieceCount = 7;
        //public static string blankWord;

        //private Board board;
        private Bitmap buffer;
        private Square[][] boardPieces;
        private Square[] boardPiecesSelected;
        private Square[] movingBoardPieces;
        private List<Square> allMovingPiecesPlayed;
        //private Letter[] player1LetterArray;
        //private Letter[] player2LetterArray;
        private Player player1;
        private Player player2;
        private bool isMovingTileSelected;
        private Square movingTileSelected;
        private Square boardTileSelected;
        private Point mouseMovingPoint;
        private Dictionary<int, Rectangle> movingTileDictionary;
        private Trie trie;
        private string[] rows;
        private string[] columns;
        private Square[][] recentRows;
        private Square[][] recentColumns;
        //private string recentHorizontalWord;
        //private string recentVerticalWord;
        private List<Square> recentHorizontalRow = new List<Square>();
        private List<Square> recentVerticalRow = new List<Square>();
        private List<Square> recentHorizontalWord = new List<Square>();
        private List<Square> recentVerticalWord = new List<Square>();
        private List<Square> recentWord = new List<Square>();
        private string[][] lettersOnBoard;
        private string blankWord;
        private ModalWord modalWord;
        private Square[] horizontalWord;
        private Square[] verticalWord;
        private Square temp;
        private string selectedWord;
        private int j = 0;
        private List<Player> players;

        public Player Player1
        {
            get { return player1; }
        }

        public Player Player2
        {
            get { return player2; }
        }

        public List<Square> RecentHorizontalRow
        {
            get { return recentHorizontalRow; }
        }

        public List<Square> RecentVerticalRow
        {
            get { return recentVerticalRow; }
        }

        public List<Square> RecentHorizontalWord
        {
            get { return recentHorizontalWord; }
            set { recentHorizontalWord = value; }
        }

        public List<Square> RecentVerticalWord
        {
            get { return recentVerticalWord; }
            set { recentVerticalWord = value; }
        }

        public List<Square> RecentWord
        {
            get { return recentWord; }
            set { recentWord = value; }
        }

        public string SelectedWord
        {
            get { return selectedWord; }
            set { selectedWord = value; }
        }

        public ModalWord ModalWord
        {
            get { return modalWord; }
            set { modalWord = value; }
        }

        public string BlankWord
        {
            get { return blankWord; }
            set { blankWord = value; }
        }
        //come up with way to pass selected combobox item from other form when user clicks button.
        //public static string selectedWord;

        public string[] Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        public string[] Columns
        {
            get { return columns; }
            set { columns = value; }
        }

        //public Board(string blankWord) {
        //    this.blankWord = blankWord;
        //}

        public Board()
        {
            InitializeComponent();
            allMovingPiecesPlayed = new List<Square>();
            horizontalWord = new Square[BoardPieceCount];
            verticalWord = new Square[BoardPieceCount];
            Rows = new string[15];
            Columns = new string[15];
            recentRows = new Square[15][];
            recentColumns = new Square[15][];
            trie = new Trie();
            WordList wordList = new WordList();
            wordList.readFile("SOWPODS (Europe Scrabble Word List).txt");
            wordList.constructTrie(trie);
            boardPiecesSelected = new Square[100];
            movingTileDictionary = new Dictionary<int, Rectangle>();
            ForeColor = SystemColors.Control;
            Player.initializeLetters();
            char[] letters1 = {};
            player1 = new Player(0, letters1, 0);
            player1.IsTurn = true;
            player1.Letters = player1.GenerateLetters(7);
            char[] letters2 = { };
            player2 = new Player(0, letters2, 0);
            player2.IsTurn = false;
            player2.Letters = player2.GenerateLetters(7);
            players = new List<Player>();
            players.Add(player1);
            players.Add(player2);

            //isMovingTileSelected = false;
            movingTileSelected = new Square(1, 1, 1, 1, new Pen(Color.Black), new SolidBrush(Color.White));
            boardTileSelected = new Square(2, 2, 1, 1, new Pen(Color.Black), new SolidBrush(Color.White), 1, 1, false);
            mouseMovingPoint = new Point(0, 0);
            //Letter letter = new Letter(

            //movingBoardPieces = new Square(

            //initialize matrices
            boardPieces = new Square[BoardPieceCount][];
            for (int i = 0; i < BoardPieceCount; i++)
            {
                boardPieces[i] = new Square[BoardPieceCount];
                recentRows[i] = new Square[BoardPieceCount];
                recentColumns[i] = new Square[BoardPieceCount];
                for (int j = 0; j < BoardPieceCount; j++)
                {
                    //boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                    //    Pens.Black, new SolidBrush(Color.GreenYellow), i + j, i - j, (char)('A' + i+j));

                    //previousBoardPieces used to compare to current boardpieces

                    if (i == 0 || i == 14)
                    {
                        if (j == 0 || j == 7 || j == 14)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                            Pens.White, new SolidBrush(Color.Red), 1, 3, false);
                            temp = boardPieces[i][j];
                        }
                        else if (j == 3 || j == 11)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                            Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                            Pens.White, new SolidBrush(Color.Olive), 1, 1, false);

                        }

                    }

                    else if (i == 1 || i == 13)
                    {

                        if (j == 1 || j == 13)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                       Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else if (j == 5 || j == 9)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                       Pens.White, new SolidBrush(Color.DarkBlue), 3, 1, false);

                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                       Pens.White, new SolidBrush(Color.Olive), 1, 1, false);

                        }
                    }

                    else if (i == 2 || i == 12)
                    {
                        if (j == 2 || j == 12)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else if (j == 6 || j == 8)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else
                        {

                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);

                        }

                    }

                    else if (i == 3 || i == 11)
                    {
                        if (j == 0 || j == 7 || j == 14)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else if (j == 3 || j == 11)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }
                    }

                    else if (i == 4 || i == 10)
                    {
                        if (j == 4 || j == 10)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                         Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                     Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }

                    }

                    else if (i == 5 || i == 9)
                    {
                        if (j == 1 || j == 5 || j == 9 || j == 13)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.DarkBlue), 3, 1, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }

                    }

                    else if (i == 6 || i == 8)
                    {
                        if (j == 2 || j == 6 || j == 8 || j == 12)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }
                    }

                    else
                    {
                        if (j == 0 || j == 14)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Red), 1, 3, false);
                        }
                        else if (j == 3 || j == 11)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);

                        }
                        else if (j == 7)
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new Square(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                          Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }
                    }
                }

            }
            movingBoardPieces = new Square[14];

            for (int i = 0; i < 14; i++) {
                movingBoardPieces[i] = new Square(15 * (PieceWidth + PiecePadding), i * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                Pens.Beige, new SolidBrush(Color.Tan));
                if (i < 7)
                {
                    movingBoardPieces[i].LetterChar = player1.Letters[i];
                    player1.setGamePieces(movingBoardPieces[i]);
                }
                else { 
                    movingBoardPieces[i].LetterChar = player2.Letters[j];
                    player2.setGamePieces(movingBoardPieces[i]);
                    j++;
                }
            }

            //for (int i = 0; i < 7; i++)
            //{

                //movingBoardPieces[i] = new Square(letterArray[i], 15 * (PieceWidth + PiecePadding), i * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                //Pens.Beige, new SolidBrush(Color.Tan));
                ////////////if player 1 turn have tiles show player 1's tiles if player 2 turn show player 2's tiles.

                //movingBoardPieces[i].LetterChar = player1.Letters[i];//player1LetterArray[i].Character; //player1.Letters[i];//player1LetterArray[i].Character;
                
                
                //player1.setGamePieces(movingBoardPieces[i], i);
                ////////////

            //}
            //for(int j = 7; j < 14; j++){
                //int i = 0;
                //movingBoardPieces[j].LetterChar = player2.Letters[i];//player2LetterArray[i].Character; //player2.Letters[j-7];//player2LetterArray[j - 7].Character;
                //i++;
            //}
        }

        public void hideMovingBoardPieces(List<Player> players, Graphics e)
        {
            foreach (Player player in players)
            {
                if (player.IsTurn)
                {
                    Square[] pieces = player.getGamePieces();
                    foreach (Square piece in pieces)
                    {
                        piece.showBoardPiece(e);
                        if (!piece.IsSelectable) {
                            allMovingPiecesPlayed.Add(piece);
                        }
                    }
                }
                else {
                    Square[] pieces = player.getGamePieces();
                    //foreach (Square square in recentWord)
                    //{
                        //for (int i = 0; i < movingBoardPieces.Length; i++)
                    foreach (Square piece in pieces)
                    {
                        piece.hideBoardPiece(e);
                        if (!piece.IsSelectable) {
                            allMovingPiecesPlayed.Add(piece);
                        }
                    }
                }
            }
            foreach (Square piece in allMovingPiecesPlayed)
            {
                piece.showBoardPiece(e);
            }
        }

        //return most recent tile moved on board
        public Square FindMostRecentLetter()
        {
            for (int i = 0; i < BoardPieceCount; i++)
            {
                for (int j = 0; j < BoardPieceCount; j++)
                {
                        int numberOne = boardPieces[i][i].MoveNumber;
                        int numberTwo = boardPieces[i][j].MoveNumber;
                        int numberThree = boardPieces[j][i].MoveNumber;
                        
                    if (numberOne > temp.MoveNumber)
                    {
                        temp = boardPieces[i][i];         
                    }

                    if (numberTwo > temp.MoveNumber) 
                    {
                        temp = boardPieces[i][j];
                    }

                    if (numberThree > temp.MoveNumber)
                    {
                        temp = boardPieces[j][i];
                    }
                }
            }
            //Console.WriteLine("Most Recent Char: " + temp.LetterChar);
            return temp;
        }

        //look for recent rows and columns
        public void FindMostRecentRows(Square mostRecentLetter) {
            for (int i = 0; i < BoardPieceCount; i++) {
                for (int j = 0; j < BoardPieceCount; j++) {
                    if (boardPieces[j][i].Y == mostRecentLetter.Y)
                    {
                        recentHorizontalRow.Add(boardPieces[j][i]);
                    }

                    if (boardPieces[j][i].X == mostRecentLetter.X)
                    {
                        recentVerticalRow.Add(boardPieces[j][i]);
                    }   
                }
            }
        }

        //make more generic so i can pass in more parameters to elimate code here and just call function 2 times.
        public void ProcessMostRecentRows(Square mostRecentLetter)
        {
            List<Square> hPart1 = new List<Square>();
            List<Square> hPart2 = new List<Square>();
            List<Square> hPart3 = new List<Square>();
            List<Square> hPart4 = new List<Square>();
            List<Square> vPart1 = new List<Square>();
            List<Square> vPart2 = new List<Square>();
            List<Square> vPart3 = new List<Square>();
            List<Square> vPart4 = new List<Square>();

            foreach (Square horizontalSquare in recentHorizontalRow) {
                if (horizontalSquare.X <= mostRecentLetter.X)
                {
                    hPart1.Add(horizontalSquare);
                }
                else
                {
                    hPart2.Add(horizontalSquare);
                }
                
            }
            hPart1.Reverse();
            foreach (Square horizontalSquare in hPart1)
            {
                if (horizontalSquare.LetterChar != '*')
                {
                    hPart3.Add(horizontalSquare);
                }
                else {
                    break;
                }
            }
            foreach (Square horizontalSquare in hPart2) {
                if (horizontalSquare.LetterChar != '*')
                {
                    hPart4.Add(horizontalSquare);
                }
                else {
                    break;
                }
            }
            hPart3.Reverse();
            hPart3.AddRange(hPart4);
            ///
            foreach (Square verticalSquare in recentVerticalRow)
            {
                if (verticalSquare.Y <= mostRecentLetter.Y)
                {
                    vPart1.Add(verticalSquare);
                }
                else
                {
                    vPart2.Add(verticalSquare);
                }

            }
            vPart1.Reverse();
            foreach (Square verticalSquare in vPart1)
            {
                if (verticalSquare.LetterChar != '*')
                {
                    vPart3.Add(verticalSquare);
                }
                else
                {
                    break;
                }
            }
            foreach (Square verticalSquare in vPart2)
            {
                if (verticalSquare.LetterChar != '*')
                {
                    vPart4.Add(verticalSquare);
                }
                else
                {
                    break;
                }
            }
            vPart3.Reverse();
            vPart3.AddRange(vPart4);
            ///
            foreach (Square horizontalSquare in hPart3) {
                recentHorizontalWord.Add(horizontalSquare);
            }

            foreach (Square verticalSquare in vPart3) {
                recentVerticalWord.Add(verticalSquare);
            }

        }


        /// <summary>
        /// Processes most recent horizontal/vertical row. returns "" if player puts a space between letters.
        /// </summary>
        /// <param name="mostRecentLetter"></param>
        /// <param name="recentWord"></param>
        /// <param name="isHorizontal"></param>
        public void ProcessMostRecentRow(Square mostRecentLetter, List<Square> recentWord, bool isHorizontal)
        {
            List<Square> part1 = new List<Square>();
            List<Square> part2 = new List<Square>();
            List<Square> part3 = new List<Square>();
            List<Square> part4 = new List<Square>();
            if (isHorizontal)
            {
                recentHorizontalRow.ForEach(e =>
                {
                    if (e.X <= mostRecentLetter.X)
                        part1.Add(e);
                    else
                        part2.Add(e);
                });
            }
            else
            {
                recentVerticalRow.ForEach(e =>
                {
                    if (e.Y <= mostRecentLetter.Y)
                        part1.Add(e);
                    else
                        part2.Add(e);
                });

            }
            part1.Reverse();
            foreach (Square square in part1)
            {
                if (square.LetterChar != '*')
                {
                    part3.Add(square);
                }
                else
                {
                    break;
                }
            }
            foreach (Square square in part2)
            {
                if (square.LetterChar != '*')
                {
                    part4.Add(square);
                }
                else
                {
                    break;
                }
            }
            part3.Reverse();
            part3.AddRange(part4);
            foreach (Square square in part3)
            {
                recentWord.Add(square);
            }

        }

        //public void EvaluateRecentWordWithBlanks(List<Square> recentWord)
        //{
        //    string word = "";
        //    foreach (Square square in recentWord)
        //    {
        //        word += square.LetterChar;
        //    }

        //    if (detectBlankLetters(word))
        //    {
        //        string[] theWords = word.Split('_');
        //        string prefix = "";
        //        string suffix = "";
        //        // 1 blank space
        //        if (theWords.Length == 2)
        //        {
        //            prefix = theWords[0];
        //            suffix = theWords[1];
        //        }

        //        // 2 blank spaces
        //        if (theWords.Length == 3)
        //        {
        //            prefix = theWords[0];
        //            suffix = theWords[2];
        //        }

        //        List<string> trieSearch = trie.Match(prefix);
        //        List<string> results = new List<string>();
        //        foreach (string result in trieSearch)
        //        {
        //            //trim whitespace from each trie result.
        //            string theResult = result.TrimStart();
        //            if (theResult.Length == word.Length)
        //            {
        //                if ((Equals(theResult, word)) && (theResult.EndsWith(suffix.ToLower())))
        //                {
        //                    results.Add(theResult);
        //                }
        //            }
        //        }
        //        ModalWord = new ModalWord(results);
        //        var answer = ModalWord.ShowDialog();
        //        if (answer == DialogResult.OK)
        //        {
        //            int i = 0;
        //            foreach (Square square in recentWord)
        //            {
        //                if (square.LetterChar == '_')//recentWord
        //                {
        //                    square.LetterChar = char.ToUpper(ModalWord.SelectedWord[i]);
        //                }
        //                i++;
        //            }
        //        }
        //    }
        //}

        //public void EvaluateRecentWordWithBlanks(List<Square> recentWord)
        //{
        //    string word = "";
        //    foreach (Square square in recentWord)
        //    {
        //        word += square.LetterChar;
        //    }

        //    if (detectBlankLetters(word))
        //    {
        //        string[] theWords = word.Split('_');
        //        string prefix = "";
        //        string suffix = "";
        //        // 1 blank space
        //        if (theWords.Length == 2)
        //        {
        //            prefix = theWords[0];
        //            suffix = theWords[1];
        //        }

        //        // 2 blank spaces
        //        if (theWords.Length == 3)
        //        {
        //            prefix = theWords[0];
        //            suffix = theWords[2];
        //        }

        //        List<string> trieSearch = trie.Match(prefix);
        //        List<string> results = new List<string>();
        //        foreach (string result in trieSearch)
        //        {
        //            //trim whitespace from each trie result.
        //            string theResult = result.TrimStart();
        //            if (theResult.Length == word.Length)
        //            {
        //                if ((Equals(theResult, word)) && (theResult.EndsWith(suffix.ToLower())))
        //                {
        //                    results.Add(theResult);
        //                }
        //            }
        //        }
        //        ModalWord = new ModalWord(results);
        //        var answer = ModalWord.ShowDialog();
        //        if (answer == DialogResult.OK)
        //        {
        //            int i = 0;
        //            foreach (Square square in recentWord)
        //            {
        //                if (square.LetterChar == '_')//recentWord
        //                {
        //                    square.LetterChar = char.ToUpper(ModalWord.SelectedWord[i]);
        //                }
        //                i++;
        //            }
        //        }
        //    }
        //}

        private string [] SplitBlankWord(string word){
            string [] words = word.Split('_');
            string [] results = new string[2];
            string prefix = "";
            string suffix = "";
            //1 blank space
            if(words.Length == 2){
                prefix = words[0];
                suffix = words[1];
            }
            // 2 blank space
            if(words.Length == 3){
                prefix = words[0];
                suffix = words[2];
            }
            results[0] = prefix;
            results[1] = suffix;
            return results;
        }

        public void EvaluateBlankWord(List<Square> recentWord1, List<Square> recentWord2)
        {
            bool isResults1 = true;
            bool isResults2 = true;
            string word1 = ToWord(recentWord1);
            string word2 = ToWord(recentWord2);
            if(detectBlankLetters(word1)){
                string[] blankWord = SplitBlankWord(word1);
                List<string> trieSearch = trie.Match(blankWord[0]);
                List<string> results = new List<string>();
                foreach (string result in trieSearch) {
                    string theResult = result.TrimStart();
                    if (theResult.Length == word1.Length) { 
                        if(Equals(theResult, word1) && (theResult.EndsWith(blankWord[1].ToLower()))){
                            results.Add(theResult);
                        }
                    }
                }
                if (results.Count > 0)
                {
                    ModalWord = new ModalWord(results);
                    var answer = ModalWord.ShowDialog();
                    if (answer == DialogResult.OK)
                    {
                        int i = 0;
                        foreach (Square square in recentWord1)
                        {
                            if (square.LetterChar == '_')
                            {
                                square.LetterChar = char.ToUpper(ModalWord.SelectedWord[i]);
                            }
                            i++;
                        }
                    }
                }
                else {
                    isResults1 = false;
                }
            }
            //if (!isResults)
            //{
                if (detectBlankLetters(word2))
                {
                    string[] blankWord = SplitBlankWord(word2);
                    List<string> trieSearch = trie.Match(blankWord[0]);
                    List<string> results = new List<string>();
                    foreach (string result in trieSearch)
                    {
                        string theResult = result.TrimStart();
                        if (theResult.Length == word2.Length)
                        {
                            if (Equals(theResult, word2) && (theResult.EndsWith(blankWord[1].ToLower())))
                            {
                                results.Add(theResult);
                            }
                        }
                    }
                    if (results.Count > 0)
                    {
                        ModalWord = new ModalWord(results);
                        var answer = ModalWord.ShowDialog();
                        if (answer == DialogResult.OK)
                        {
                            int i = 0;
                            foreach (Square square in recentWord2)
                            {
                                if (square.LetterChar == '_')
                                {
                                    square.LetterChar = char.ToUpper(ModalWord.SelectedWord[i]);
                                }
                                i++;
                            }
                        }
                    }
                    else
                    {
                        isResults2 = false;
                    }
                }
                if (!isResults1 && !isResults2) {
                    MessageBox.Show("Word does not exist. Try a different word then click Play Word");
                }

        }

        // method to determine score of word after blanks declared
        public Word calculateRecentWord(List<Square> recentWord)
        {
            string wordName = "";
            Word word = null;
            int wordBonus = 0;
            int score = 0;
            foreach (Square square in recentWord)
            {
                wordName += square.LetterChar;
                if (square.WordBonus != 1)
                {
                    wordBonus = wordBonus + square.WordBonus;
                }
                foreach (Letter letter in Player.AllLetters)
                {
                    if (square.LetterChar == letter.Character)
                    {
                        score += (letter.Value * square.LetterBonus);
                        break;
                    }
                }
            }
            //string wordNameReversed = wordName.Reverse().ToString();
            if (wordBonus > 0)
            {
                score = (score * wordBonus);
            }
            if (lookUp(wordName))
            {
                word = new Word(wordName, score);
            }
            //else
            //{
            //    if (lookUp(wordNameReversed))
            //    {
            //        word = new Word(wordNameReversed, score);
            //    }
            //}
            return word;
        }

        public string ToWord(List<Square> squares) {
            string word = "";
            foreach (Square square in squares) {
                word += square.LetterChar;
            }
            return word;
        }

        //public void FinalizeTiles(List<Square> squares)
        //{
        //    foreach (Square square in squares)
        //    {
        //        square.IsSelectable = false;
        //    }
        //}

        public List<Square> getGamePieceTiles(List<Square> squares) { 
            List<Square> gamePieceTiles = new List<Square>();
            foreach(Square square in squares){
                for(int i = 0; i < movingBoardPieces.Length; i++){
                    if (square.Point == movingBoardPieces[i].Point) {
                        gamePieceTiles.Add(movingBoardPieces[i]);
                    }
                }
            }
            return gamePieceTiles;
        }

        public void FinalizeTiles(List<Square> squares)
        {
            foreach (Square square in squares)
            {
                square.IsSelectable = false;
            }
        }

        private void ClearRecentWords() {
            recentHorizontalWord = new List<Square>();
            recentVerticalWord = new List<Square>();
            recentWord = new List<Square>();
        }

        private void ClearRecentRows() {
            recentHorizontalRow = new List<Square>();
            recentVerticalRow = new List<Square>();
        }

        public void Clear() {
            ClearRecentRows();
            ClearRecentWords();
        }


        public Word scanRecentHorizontalWord(Square mostRecentLetter)
        {
            Word word = null;
            string wordName = "";
            int wordBonus = 0;
            int score = 0;
            List<Square> prefix = new List<Square>();
            List<Square> suffix = new List<Square>();
            List<Square> newPrefixReversed = new List<Square>();
            List<Square> newSuffix = new List<Square>();
 
            for (int i = 0; i < BoardPieceCount; i++)
            {
                for (int j = 0; j < BoardPieceCount; j++)
                {
                    if (boardPieces[j][i].Y == mostRecentLetter.Y && boardPieces[j][i].X <= mostRecentLetter.X)
                    {
                        prefix.Add(boardPieces[j][i]);
                    }

                    if (boardPieces[j][i].Y == mostRecentLetter.Y && boardPieces[j][i].X > mostRecentLetter.X)
                    {
                        suffix.Add(boardPieces[j][i]);
                    }
                }
            }
            prefix.Reverse();
            foreach (Square square in prefix)
            {
                if (square.LetterChar != '*')
                {
                    newPrefixReversed.Add(square);
                }
                else
                {
                    break;
                }
            }
            foreach (Square square in suffix)
            {
                if (square.LetterChar != '*')
                {
                    newSuffix.Add(square);
                }
                else
                {
                    break;
                }
            }
            newPrefixReversed.Reverse();
            //construct word
            newPrefixReversed.AddRange(newSuffix);
            foreach (Square square1 in newPrefixReversed)
            {
                wordName += square1.LetterChar;
            }
            //if word bonus add bonus to wordBonus.
            foreach (Square square in newPrefixReversed) {
                if (square.WordBonus != 1)
                {
                    wordBonus = wordBonus + square.WordBonus;
                }
                foreach (Letter letter in Player.AllLetters) {
                    if (square.LetterChar == letter.Character) {
                        score += (letter.Value*square.LetterBonus);
                        break;
                    }
                }
            }
            if (wordBonus > 0) {
                score = (score * wordBonus);
            }

            char[] wordArray = wordName.ToCharArray();
            Array.Reverse(wordArray);
            string wordReversed = new string(wordArray);

            //check if word exists
            if (lookUp(wordName))
            {
                word = new Word(wordName, score);
            }

            //check if word exists backwards
            else if(lookUp(wordReversed)){
                word = new Word(wordReversed, score);
            }
            //word does not exist!
            else {
                word = new Word("Invalid Word!", 0);
            }

            return word;        
        }

        // b must contain blank( _ ) character(s)
        private bool Equals(string a, string b) {
            a = a.ToLower();
            b = b.ToLower();
            if (a.Length == b.Length)
            {
                for (int i = 0; i < a.Length - 1; i++) {
                    if (b[i] != '_')
                    {
                        if (a[i] != b[i])
                        {
                            return false;
                        }
                    }
                }
                return true;

            }
            return false;  
        }

        //lookup word for matches if found return true.
        public bool lookUp(string word)
        {
            ///newly added
            word = word.ToLower();
            ///
            bool found = false;
            if (word.Length >= 2)
            {
                List<string> results = trie.Match(word);
                foreach (string result in results)
                {
                    if (result.TrimStart() == word)
                    {
                        found = true;
                        break;
                    }
                    else
                    {
                        found = false;
                    }
                }
            }
            return found;
        }

        //detects blank('_') character useful for detecting if player should declare the blank.
        public bool detectBlankLetters(string word) {
            if (!String.IsNullOrWhiteSpace(word))
            {
                string[] words = word.Split('_');
                if (words.Length == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else {
                return false;
            }
        }

        public void occupyBoard(Square[][] boardPieces, int i, int j)
        {
            boardPieces[i][j].Occupied = true;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            if (Width < 0 || Height < 0) return;
            buffer = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            base.OnSizeChanged(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                for (int i = 0; i < 14; i++)
                {

                        //if (movingBoardPieces[i].Intersect(e.Location)) 
                        if (movingBoardPieces[i].Intersect(e.Location) && movingBoardPieces[i].IsSelectable) 
                        {
                                //Console.WriteLine(movingBoardPieces[i].Letter.Character);
                                isMovingTileSelected = true;
                                movingTileSelected = movingBoardPieces[i];
                                break;
                        }
                }
            }
                //if (e.Button == MouseButtons.Right)
            if (e.Button == MouseButtons.Right && movingTileSelected.IsSelectable)
                {
                    if (isMovingTileSelected)
                    {
                        Point point = new Point(e.X, e.Y);
                        for (int i = 0; i < BoardPieceCount; i++)
                        {
                            for (int j = 0; j < BoardPieceCount; j++)
                            {
                                //set board piece id and set selected board tile
                                if (boardPieces[i][j].Rectangle.Contains(point))
                                {
                                    boardPieces[i][j].Id = movingTileSelected.Id;
                                    boardTileSelected = boardPieces[i][j];
                                    break;
                                }
                            }
                        }

                        //if both movingpiece and boardpiece are overlapping then write character to board
                        if ((movingTileSelected.Rectangle.Contains(point)) && (boardTileSelected.Rectangle.Contains(point)))
                        {
                            movingTileSelected.Rectangle = new Rectangle(boardTileSelected.X, boardTileSelected.Y, movingTileSelected.Rectangle.Width, movingTileSelected.Rectangle.Height);
                            movingTileSelected.Point = new Point(boardTileSelected.X, boardTileSelected.Y);
                            boardTileSelected.LetterChar = movingTileSelected.LetterChar;
                            //boardTileSelected.MoveNumber = ++Square.numberOfMoves;


                            if (!movingTileDictionary.ContainsKey(movingTileSelected.Id))
                            {
                                movingTileDictionary.Add(movingTileSelected.Id, boardTileSelected.Rectangle);
                                //boardTileSelected.MoveNumber = Square.NumberOfMoves++;
                                boardTileSelected.MoveNumber = ++Square.numberOfMoves;
                            }
                            //remove all letters from moving same tile to different locations on board then add most recent letter to board
                            else
                            {
                                for (int k = 0; k < BoardPieceCount; k++)
                                {
                                    for (int l = 0; l < BoardPieceCount; l++)
                                    {
                                        if (boardPieces[k][l].Id == movingTileSelected.Id)
                                        {
                                            boardPieces[k][l].LetterChar = '*';
                                            //boardPieces[k][l].MoveNumber = 0;
                                            boardTileSelected.MoveNumber = 0;
                                        }
                                    }
                                }
                                //boardTileSelected.LetterChar = movingTileSelected.Letter.Character;
                                boardTileSelected.LetterChar = movingTileSelected.LetterChar;
                                //boardTileSelected.MoveNumber = Square.NumberOfMoves++;
                                boardTileSelected.MoveNumber = ++Square.numberOfMoves;
                            }
                        }
                        //newer when player moves piece off board make sure to reset the boardpieces letter char.
                        if ((movingTileSelected.Rectangle.Contains(point)) && (!boardTileSelected.Rectangle.Contains(point)))
                        {
                            for (int k = 0; k < BoardPieceCount; k++)
                            {
                                for (int l = 0; l < BoardPieceCount; l++)
                                {
                                    if (boardPieces[k][l].Id == movingTileSelected.Id)
                                    {
                                        movingTileSelected.Point = new Point();
                                        boardPieces[k][l].Id = 0;
                                        boardPieces[k][l].LetterChar = '*';
                                        boardPieces[k][l].MoveNumber = 0;
                                    }
                                }
                            }
                        }
                    }

                    isMovingTileSelected = false;
                }
            
            base.OnMouseClick(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            //also check if movingtile is selectable
            ///if (isMovingTileSelected)
                //if (isMovingTileSelected)
                if(isMovingTileSelected && movingTileSelected.IsSelectable)
                {
                    movingTileSelected.Rectangle = new Rectangle(e.X - 15, e.Y - 15, movingTileSelected.Rectangle.Width, movingTileSelected.Rectangle.Height);
                }
                this.Invalidate();

                base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (var g = Graphics.FromImage(buffer))
            {
                g.Clear(ForeColor);
                for (int i = 0; i < BoardPieceCount; i++)
                {
                    for (int j = 0; j < BoardPieceCount; j++)
                    {
                        var piece = boardPieces[i][j];
                        piece.drawBoard(g);
                    }
                }

                for (int i = 0; i < BoardPieceCount; i++)
                {
                    g.DrawLine(Pens.White, i * (PieceHeight + PiecePadding), 0, i * (PieceHeight + PiecePadding), Height);
                    g.DrawLine(Pens.White, 0, i * (PieceHeight + PiecePadding), Width, i * (PieceHeight + PiecePadding));
                }

                //List<Player> players = new List<Player>();
                //players.Add(player1);
                //players.Add(player2);
                //drawMovingBoardPieces(players);

                //for (int i = 0; i < 14; i++)
                //{
                    //var piece = movingBoardPieces[i];
                    //piece.drawBoardLetters(g);
                //}
                hideMovingBoardPieces(players, g);




            }
            e.Graphics.DrawImage(buffer, 0, 0);
            //base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            base.OnPaintBackground(e);
        }

    }
}
