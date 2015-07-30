using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scrabble
{
    public class GamePiece
    {
        private static int numberOfSquares = 0;
        public static int numberOfMoves = 0;
        public bool IsSelectable { get; set; }
        public int MoveNumber { get; set; }
        public char LetterChar { get; set; }
        public int Id { get; set; }
        public bool Occupied { get; set; }
        public SolidBrush SolidBrush { get; set; }
        public Pen Pen { get; set; }
        public Rectangle Rectangle { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point Point { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int LetterBonus { get; set; }
        public int WordBonus { get; set; }

        //board game piece
        public GamePiece(int x, int y, int width, int height, Pen pen, SolidBrush solidBrush, int letterBonus, int wordBonus, bool occupied)
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
        }

        //letter game piece
        public GamePiece(int x, int y, int width, int height, Pen pen, SolidBrush solidBrush)
        {
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
            MoveNumber = 0;
            IsSelectable = true;
        }

        public void drawGamePiece(Graphics e)
        {
            e.FillRectangle(SolidBrush, Rectangle);
            e.DrawRectangle(Pen, Rectangle);
        }


        public void showBoardPiece(Graphics e)
        {
            e.FillRectangle(SolidBrush, Rectangle);
            e.DrawRectangle(Pen, Rectangle);
            e.DrawString(LetterChar.ToString(), new Font("Arial", 16), new SolidBrush(Color.Black), Rectangle);
        }

        public void hideGamePiece(Graphics e)
        {
            if (IsSelectable == true)
            {
                Rectangle hiddenRectangle = new Rectangle(X, Y, Width + 4, Height + 4);
                e.ExcludeClip(hiddenRectangle);               
            }
        }

        public bool Intersect(Point point)
        {
            return Rectangle.Contains(point) ? true : false;
        }
    }
}
