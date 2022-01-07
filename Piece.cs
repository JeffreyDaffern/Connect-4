using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connect4
{
    /// <summary>
    /// Represents a game piece that is placed that moves and is drawn in a position determined by the user.
    /// </summary>
    /// <author>
    /// Connor O'Leary and Jeff Daffern
    /// </author>
    class Piece
    {
        public bool IsDropped { get; set; }
        public bool Turn { get; }
        public int ColXPos { get; set; }
        public int Cols { get; }
        
        public int X { get; set; }
        public int Y { get; set; }
        private Board TheBoard { get; }
        /// <summary>
        /// Constructor to initialize the fields.
        /// </summary>
        /// <param name="theBoard"></param> The board that the piece will be drawn on.
        /// <param name="turn"></param> Indicates the player that this piece belongs to.
        /// <param name="x"></param> X coordinate of the current piece.
        /// <param name="y"></param> Y coordinate of the current piece.
        public Piece(Board theBoard, bool turn)
        {
            IsDropped = false;

            Turn = turn;
            X = 0;
            Y = 0;
            TheBoard = theBoard;

            Cols = TheBoard.GameBoard.GetLength(0);
            ColXPos = (Cols / 2);


        }
        /// <summary>
        /// Moves the piece left or right based on user input. 
        /// </summary>
        /// <param name="left"></param> True to move left and false to move right.
        public void Move(bool left)
        {
            if (left)
            {
                ColXPos -= 1;
                if (ColXPos < 0)
                {
                    ColXPos = Cols - 1;
                }
            }
            else
            {
                ColXPos += 1;
                if (ColXPos >= Cols)
                {
                    ColXPos = 0;
                }
            }

        }

        /// <summary>
        /// Drops the current piece where the user determines and it falls 
        /// to the last open spot where it is drawn. Cannot exceed the board height.
        /// </summary>
        public void Drop()
        {
            bool pieceCanDrop = false;
            for (int i = 0; i < TheBoard.GameBoard.GetLength(1); i++)
            {
                if (TheBoard.GameBoard[ColXPos, i] == 0)
                {
                    TheBoard.GameBoard[ColXPos, i] = Turn ? 1 : -1;
                    pieceCanDrop = true;

                    X = ColXPos + 1;
                    Y = (TheBoard.YPos.GetLength(0) - i);
                    //Console.SetCursorPosition(0, 0); Testing statements to see where the piece was dropped
                    //Console.Write($"({X},{Y})");

                    DrawDrop(TheBoard.YPos[i]);
                    break;
                }
            }
            if (pieceCanDrop)
            {
                IsDropped = true;
            }
        }

        /// <summary>
        /// Clears the current piece so it can be drawn after it is dropped.
        /// </summary>
        public void Draw()
        {
            //Clear Original
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(TheBoard.Left, TheBoard.Top - 3);
            int length = ((TheBoard.GameBoard.GetLength(0) * 2) + 1) * 2;
            Console.Write($"{{0, {length}}}", ' ');


            //Draw Piece
            DrawPos(TheBoard.Top - 3, false);

        }

        /// <summary>
        /// Animates the dropping of the piece into the board
        /// </summary>
        /// <param name="yPos"></param> final location of the piece
        public void DrawDrop(int yPos)
        {
            int start = TheBoard.Top - 3;
            int end = yPos;

            for (int i = 0; i + start <= end; i++)
            {
                bool cursorOnRow = (((end - (start + i)) % 2) == 1);
                if (i < 3 || cursorOnRow)
                    DrawPos((start + i) - 1, true);
                if (i < 3 || !cursorOnRow)
                    DrawPos((start + i), false);
                Thread.Sleep(70);
            }
        }

        /// <summary>
        /// Draws the piece at the selected y coordinate based on its current X
        /// </summary>
        /// <param name="yPos"></param> Y coordinate to be dropped to
        /// <param name="clear"></param> determines if it will draw in black to clear a piece from being there
        public void DrawPos(int yPos, bool clear)
        {
            Console.BackgroundColor = clear ? ConsoleColor.Black : (Board.getPlayerColor(Turn));
            Console.SetCursorPosition(TheBoard.XPos[ColXPos], yPos);
            Console.Write("  ");

            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);

        }
    }
}


