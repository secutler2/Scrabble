using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble
{
    public class Square
    {
        private int x;
        private int y;
        private Point point;
        private int width;
        private int height;
        private int letterBonus; //(1 - 3) 1 = none, 2 = 2x, 3 = 3x
        private int wordBonus; //(1 - 3) 1 = none, 2 = 2x, 3 = 3x
        private Rectangle rectangle;
        private Pen pen;
        private SolidBrush solidBrush;
        //private Letter letter;
        private char letterChar;
        private bool occupied;
        private int id;
        private static int numberOfSquares = 0;
        public static int numberOfMoves = 0;
        private int moveNumber;
        ///
        private bool isSelectable;

        public bool IsSelectable
        {
            get { return isSelectable; }
            set { isSelectable = value; }
        }
        ///

        //public static int NumberOfMoves
        //{
        //    get { return Square.numberOfMoves; }
        //    set { Square.numberOfMoves = value; }
        //}

        public int MoveNumber
        {
            get { return moveNumber; }
            set { moveNumber = value; }
        }

        public char LetterChar
        {
            get { return letterChar; }
            set { letterChar = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public bool Occupied
        {
            get { return occupied; }
            set { occupied = value; }
        }

        public SolidBrush SolidBrush
        {
            get { return solidBrush; }
            set { solidBrush = value; }
        }

        public Pen Pen
        {
            get { return pen; }
            set { pen = value; }
        }

        public Rectangle Rectangle
        {
            get { return rectangle; }
            set { rectangle = value; }
        }

        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public Point Point
        {
            get { return point; }
            set { point = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int LetterBonus
        {
            get { return letterBonus; }
            set { letterBonus = value; }
        }

        public int WordBonus
        {
            get { return wordBonus; }
            set { wordBonus = value; }
        }

        //public Letter Letter
        //{
        //    get { return letter; }
        //    set { letter = value; }
        //}

        //public Square(int x, int y, int width, int height, Pen pen, SolidBrush solidBrush, int letterBonus, int wordBonus, char c)
        //{
        //    X = x;
        //    Y = y;
        //    Width = width;
        //    Height = height;
        //    Pen = pen;
        //    SolidBrush = solidBrush;
        //    LetterBonus = letterBonus;
        //    WordBonus = wordBonus;
        //    initializeBoardRectangle();
        //    initializePoint();
        //    Letter = new Letter(c, 1, 1);
        //}

        //scrabble game board ( Point is a struct I dont want it to move, so I set x and y values)
        public Square(int x, int y, int width, int height, Pen pen, SolidBrush solidBrush, int letterBonus, int wordBonus, bool occupied)
        {

            X = x;
            Y = y;
            Width = width;
            Height = height;
            Pen = pen;
            SolidBrush = solidBrush;
            LetterBonus = letterBonus;
            WordBonus = wordBonus;
            Occupied = occupied;
            LetterChar = '*';
            Rectangle = new Rectangle(x, y, width, height);
            Point = new Point(x, y);
            //IsSelectable = true;
            //moveNumber = 0;
        }

        //scrabble moveable game piece ( Point is a struct I want point to move so I dont set x and y values until I move game piece)
        public Square(int x, int y, int width, int height, Pen pen, SolidBrush solidBrush)
        {
            //Letter = letter;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Pen = pen;
            SolidBrush = solidBrush;
            Id = numberOfSquares++;
            LetterChar = '*';
            Rectangle = new Rectangle(x, y, width, height);
            Point = new Point();
            moveNumber = 0;
            IsSelectable = true;
            ///
        }

        public void setPoint(int x, int y) 
        {
            point = new Point(x, y);

        }

        public Point getPoint() {
            return point;
        }

        //public void drawBoard(Graphics e)
        //{
        //    e.FillRectangle(solidBrush, rectangle);
        //    e.DrawRectangle(pen, rectangle);
        //    e.DrawString(Letter.Character.ToString(), new Font(FontFamily.GenericSerif, 20), Brushes.White, Point);
        //}

        public void drawBoard(Graphics e)
        {
            e.FillRectangle(solidBrush, rectangle);
            e.DrawRectangle(pen, rectangle);
        }

        public void drawBoardLetters(Graphics e) {
            e.FillRectangle(solidBrush, rectangle);
            e.DrawRectangle(pen, rectangle);
            e.DrawString(LetterChar.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), Rectangle);
        }

        public void showBoardPiece(Graphics e)
        {
            e.FillRectangle(solidBrush, rectangle);
            e.DrawRectangle(pen, rectangle);
            e.DrawString(LetterChar.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), Rectangle);
        }

        public void hideBoardPiece(Graphics e)
        {
            if (IsSelectable == true)
            {
                //e.Dispose();
                Rectangle hiddenRectangle = new Rectangle(x, y, width + 4, height + 4);
                e.ExcludeClip(hiddenRectangle);
                
                
            }
        }

        //List<Square> gamePieceTiles = new List<Square>();
        //    foreach(Square square in squares){
        //        for(int i = 0; i < movingBoardPieces.Length; i++){
        //            if (square.Point == movingBoardPieces[i].Point) {
        //                gamePieceTiles.Add(movingBoardPieces[i]);
        //            }
        //        }
        //    }

        //public void hideBoardPiece(Graphics e)
        //{
        //    if (IsSelectable == true)
        //    {
        //        e.Dispose();
        //    }
        //}

        //public void hideBoardPiece(Graphics e, Square[] movingBoardPieces, List<Square> squares)
        //{
        //    foreach (Square square in squares)
        //    {
        //        for (int i = 0; i < movingBoardPieces.Length; i++)
        //        {
        //            if (square.Point == movingBoardPieces[i].Point)
        //            {
        //                //e.Dispose();
        //            }
        //        }
        //    }
        //}

        public bool Intersect(Point point)
        {
            return rectangle.Contains(point) ? true : false;
        }

        //used to compare old board pieces to current board pieces
        //public Square[][] shallowCopy()
        //{   
        //    //throw new Exception();
        //   return (Square[][])this.MemberwiseClone();
        //}
    }
}
