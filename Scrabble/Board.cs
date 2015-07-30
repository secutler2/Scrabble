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
        private Bitmap buffer;
        private GamePiece[][] boardPieces;
        private GamePiece[] boardPiecesSelected;
        private GamePiece[] movingBoardPieces;
        private List<GamePiece> allMovingPiecesPlayed;
        private bool isMovingTileSelected;
        private GamePiece movingTileSelected;
        private GamePiece boardTileSelected;
        private Point mouseMovingPoint;
        private Dictionary<int, Rectangle> movingTileDictionary;
        private Trie trie;
        private string[] rows;
        private string[] columns;
        private GamePiece[][] recentRows;
        private GamePiece[][] recentColumns;
        private string[][] lettersOnBoard;
        private string blankWord;
        private ModalWord modalWord;
        private GamePiece[] horizontalWord;
        private GamePiece[] verticalWord;
        private GamePiece temp;
        private string selectedWord;
        private int j = 0;
        private List<Player> players;
        private TrieImplementation trieImplementation = new TrieImplementation();

        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public List<GamePiece> RecentHorizontalRow { get; private set; }
        public List<GamePiece> RecentVerticalRow { get; private set; }
        public List<GamePiece> RecentHorizontalWord { get; private set; }
        public List<GamePiece> RecentVerticalWord { get; private set; }
        public List<GamePiece> RecentWord { get; set; }

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

        public Board()
        {
            RecentHorizontalWord = new List<GamePiece>();
            RecentHorizontalRow = new List<GamePiece>();
            RecentVerticalRow = new List<GamePiece>();
            RecentVerticalWord = new List<GamePiece>();
            RecentWord = new List<GamePiece>();






            InitializeComponent();
            allMovingPiecesPlayed = new List<GamePiece>();
            horizontalWord = new GamePiece[BoardPieceCount];
            verticalWord = new GamePiece[BoardPieceCount];
            Rows = new string[15];
            Columns = new string[15];
            recentRows = new GamePiece[15][];
            recentColumns = new GamePiece[15][];
            trie = new Trie();
            WordList wordList = new WordList();
            wordList.readFile("SOWPODS (Europe Scrabble Word List).txt");
            trieImplementation.constructTrie(trie, wordList);
            boardPiecesSelected = new GamePiece[100];
            movingTileDictionary = new Dictionary<int, Rectangle>();
            ForeColor = SystemColors.Control;
            Player.initializeLetters();
            char[] letters1 = {};
            Player1 = new Player(0, letters1, 0);
            Player1.IsTurn = true;
            Player1.Letters = Player1.ChooseLetters(7);
            char[] letters2 = { };
            Player2 = new Player(0, letters2, 0);
            Player2.IsTurn = false;
            Player2.Letters = Player2.ChooseLetters(7);
            players = new List<Player>();
            players.Add(Player1);
            players.Add(Player2);

            movingTileSelected = new GamePiece(1, 1, 1, 1, new Pen(Color.Black), new SolidBrush(Color.White));
            boardTileSelected = new GamePiece(2, 2, 1, 1, new Pen(Color.Black), new SolidBrush(Color.White), 1, 1, false);
            mouseMovingPoint = new Point(0, 0);

            //initialize matrices
            boardPieces = new GamePiece[BoardPieceCount][];
            for (int i = 0; i < BoardPieceCount; i++)
            {
                boardPieces[i] = new GamePiece[BoardPieceCount];
                recentRows[i] = new GamePiece[BoardPieceCount];
                recentColumns[i] = new GamePiece[BoardPieceCount];
                for (int j = 0; j < BoardPieceCount; j++)
                {

                    //previousBoardPieces used to compare to current boardpieces

                    if (i == 0 || i == 14)
                    {
                        if (j == 0 || j == 7 || j == 14)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                            Pens.White, new SolidBrush(Color.Red), 1, 3, false);
                            temp = boardPieces[i][j];
                        }
                        else if (j == 3 || j == 11)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                            Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                            Pens.White, new SolidBrush(Color.Olive), 1, 1, false);

                        }

                    }

                    else if (i == 1 || i == 13)
                    {

                        if (j == 1 || j == 13)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                       Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else if (j == 5 || j == 9)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                       Pens.White, new SolidBrush(Color.DarkBlue), 3, 1, false);

                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                       Pens.White, new SolidBrush(Color.Olive), 1, 1, false);

                        }
                    }

                    else if (i == 2 || i == 12)
                    {
                        if (j == 2 || j == 12)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else if (j == 6 || j == 8)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else
                        {

                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);

                        }

                    }

                    else if (i == 3 || i == 11)
                    {
                        if (j == 0 || j == 7 || j == 14)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else if (j == 3 || j == 11)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }
                    }

                    else if (i == 4 || i == 10)
                    {
                        if (j == 4 || j == 10)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                         Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                     Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }

                    }

                    else if (i == 5 || i == 9)
                    {
                        if (j == 1 || j == 5 || j == 9 || j == 13)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.DarkBlue), 3, 1, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }

                    }

                    else if (i == 6 || i == 8)
                    {
                        if (j == 2 || j == 6 || j == 8 || j == 12)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }
                    }

                    else
                    {
                        if (j == 0 || j == 14)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.Red), 1, 3, false);
                        }
                        else if (j == 3 || j == 11)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.LightBlue), 2, 1, false);

                        }
                        else if (j == 7)
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                      Pens.White, new SolidBrush(Color.HotPink), 1, 2, false);
                        }
                        else
                        {
                            boardPieces[i][j] = new GamePiece(i * (PieceWidth + PiecePadding), j * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                          Pens.White, new SolidBrush(Color.Olive), 1, 1, false);
                        }
                    }
                }

            }
            movingBoardPieces = new GamePiece[14];

            for (int i = 0; i < 14; i++) {
                movingBoardPieces[i] = new GamePiece(15 * (PieceWidth + PiecePadding), i * (PieceHeight + PiecePadding), PieceWidth, PieceHeight,
                Pens.Beige, new SolidBrush(Color.Tan));
                if (i < 7)
                {
                    movingBoardPieces[i].LetterChar = Player1.Letters[i];
                    Player1.setGamePieces(movingBoardPieces[i]);
                }
                else {
                    movingBoardPieces[i].LetterChar = Player2.Letters[j];
                    Player2.setGamePieces(movingBoardPieces[i]);
                    j++;
                }
            }
        }

        public void hideMovingBoardPieces(List<Player> players, Graphics e)
        {
            foreach (Player player in players)
            {
                if (player.IsTurn)
                {
                    GamePiece[] pieces = player.getGamePieces();
                    foreach (GamePiece piece in pieces)
                    {
                        piece.showBoardPiece(e);
                        if (!piece.IsSelectable) {
                            allMovingPiecesPlayed.Add(piece);
                        }
                    }
                }
                else {
                    GamePiece[] pieces = player.getGamePieces();
                    foreach (GamePiece piece in pieces)
                    {
                        piece.hideGamePiece(e);
                        if (!piece.IsSelectable) {
                            allMovingPiecesPlayed.Add(piece);
                        }
                    }
                }
            }
            foreach (GamePiece piece in allMovingPiecesPlayed)
            {
                piece.showBoardPiece(e);
            }
        }


        public GamePiece FindMostRecentLetter()
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
        public void FindMostRecentRows(GamePiece mostRecentLetter) {
            for (int i = 0; i < BoardPieceCount; i++) {
                for (int j = 0; j < BoardPieceCount; j++) {
                    if (boardPieces[j][i].Y == mostRecentLetter.Y)
                    {
                        RecentHorizontalRow.Add(boardPieces[j][i]);
                    }

                    if (boardPieces[j][i].X == mostRecentLetter.X)
                    {
                        RecentVerticalRow.Add(boardPieces[j][i]);
                    }   
                }
            }
        }

        /// <summary>
        /// Processes most recent horizontal/vertical row. returns "" if player puts a space between letters.
        /// </summary>
        /// <param name="mostRecentLetter"></param>
        /// <param name="recentWord"></param>
        /// <param name="isHorizontal"></param>
        public void ProcessMostRecentRow(GamePiece mostRecentLetter, List<GamePiece> recentWord, bool isHorizontal)
        {
            List<GamePiece> part1 = new List<GamePiece>();
            List<GamePiece> part2 = new List<GamePiece>();
            List<GamePiece> part3 = new List<GamePiece>();
            List<GamePiece> part4 = new List<GamePiece>();
            if (isHorizontal)
            {
                RecentHorizontalRow.ForEach(e =>
                {
                    if (e.X <= mostRecentLetter.X)
                        part1.Add(e);
                    else
                        part2.Add(e);
                });
            }
            else
            {
                RecentVerticalRow.ForEach(e =>
                {
                    if (e.Y <= mostRecentLetter.Y)
                        part1.Add(e);
                    else
                        part2.Add(e);
                });

            }
            part1.Reverse();
            foreach (GamePiece square in part1)
            {
                if (square.LetterChar != '*')
                {
                    part3.Add(square);
                }
            }
            foreach (GamePiece square in part2)
            {
                if (square.LetterChar != '*')
                {
                    part4.Add(square);
                }
            }
            part3.Reverse();
            part3.AddRange(part4);
            foreach (GamePiece square in part3)
            {
                recentWord.Add(square);
            }
        }

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

        public void EvaluateBlankWord(List<GamePiece> recentWord1, List<GamePiece> recentWord2)
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
                        foreach (GamePiece square in recentWord1)
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
                            foreach (GamePiece square in recentWord2)
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
        public Word calculateRecentWord(List<GamePiece> recentWord)
        {
            string wordName = "";
            Word word = null;
            int wordBonus = 0;
            int score = 0;
            foreach (GamePiece square in recentWord)
            {
                wordName += square.LetterChar;
                if (square.WordBonus != 1)
                {
                    wordBonus = wordBonus + square.WordBonus;
                }
                foreach (Letter letter in Player.AllLetters)
                {
                    if (square.LetterChar == letter.AlphabeticalCharacter)
                    {
                        score += (letter.ValueOfLetter * square.LetterBonus);
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
           
            return word;
        }

        public string ToWord(List<GamePiece> squares) {
            string word = "";
            foreach (GamePiece square in squares) {
                word += square.LetterChar;
            }
            return word;
        }


        public List<GamePiece> getGamePieceTiles(List<GamePiece> squares) { 
            List<GamePiece> gamePieceTiles = new List<GamePiece>();
            foreach(GamePiece square in squares){
                for(int i = 0; i < movingBoardPieces.Length; i++){
                    if (square.Point == movingBoardPieces[i].Point) {
                        gamePieceTiles.Add(movingBoardPieces[i]);
                    }
                }
            }
            return gamePieceTiles;
        }

        public void FinalizeTiles(List<GamePiece> squares)
        {
            foreach (GamePiece square in squares)
            {
                square.IsSelectable = false;
            }
        }

        private void ClearRecentWords() {
            RecentHorizontalWord = new List<GamePiece>();
            RecentVerticalWord = new List<GamePiece>();
            RecentWord = new List<GamePiece>();
        }

        private void ClearRecentRows() {
            RecentHorizontalRow = new List<GamePiece>();
            RecentVerticalRow = new List<GamePiece>();
        }

        public void Clear() {
            ClearRecentRows();
            ClearRecentWords();
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
                                boardTileSelected.MoveNumber = ++GamePiece.numberOfMoves;
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
                                boardTileSelected.MoveNumber = ++GamePiece.numberOfMoves;
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
                        piece.drawGamePiece(g);
                    }
                }

                for (int i = 0; i < BoardPieceCount; i++)
                {
                    g.DrawLine(Pens.White, i * (PieceHeight + PiecePadding), 0, i * (PieceHeight + PiecePadding), Height);
                    g.DrawLine(Pens.White, 0, i * (PieceHeight + PiecePadding), Width, i * (PieceHeight + PiecePadding));
                }

                hideMovingBoardPieces(players, g);




            }
            e.Graphics.DrawImage(buffer, 0, 0);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            e.Graphics.Clear(BackColor);
            base.OnPaintBackground(e);
        }

    }
}
