using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Connect4
{
    /// <summary>
    /// Represents the physical Board of a game called Connect4 that has a turn order and 
    /// messages for instruction.
    /// </summary>
    /// <author>
    /// Connor O'Leary and Jeff Daffern
    /// </author>
   
    class Board
    {
        public int Top { get; }
        public int Left { get; }
        public int[,] GameBoard { get; }
        public int[] XPos { get; }
        public int[] YPos { get; }
        public int[,] Coordinates { get; set; }
        public int Counter { get; set; }
        public int Matches { get; set; }
        private bool TieGame { get; set; }

        private int defaultDistance = 8;


        /// <summary>
        /// Constructor to initialize the fields and draw the current game board.
        /// </summary>
        /// <param name="top"></param> top margin
        /// <param name="left"></param> left margin
        /// <param name="colCount"></param> Determines how many columns across there are.
        /// <param name="colHeight"></param> Determines how many rows down there are.
        public Board(int top, int left, int colCount, int colHeight)
        {
            TieGame = false;
            Top = top + defaultDistance;
            Left = left;
            GameBoard = new int[colCount, colHeight];
            XPos = new int[colCount];
            YPos = new int[colHeight];
            for (int i = 0; i < colCount; i++)
            {
                XPos[i] = Left + 2 + (i * 4);
            }

            for (int i = 0; i < colHeight; i++)
            {
                YPos[i] = Top + 1 + (((colHeight - 1) - i) * 2);
            }

            DrawGameMessage(" Connect 4 ", ConsoleColor.Black, ConsoleColor.Yellow, 8);
            DrawGameMessage("Push Any Key to Start", ConsoleColor.White);
           
            Coordinates = new int[colCount * colHeight, 3];
            Counter = 0;
            Matches = 0;
        
        }

        /// <summary>
        /// Ends the game and displayed who(if any) won
        /// </summary>
        /// <param name="turn"></param>
        public void GameOver(bool turn)
        {
            if (TieGame)
            {
                DrawGameMessage("Tie Game!", ConsoleColor.Yellow);
            }
            else
            {
                string playerNum = turn ? "1" : "2";
                string turnMessage = $"Player {playerNum} Wins!";
                DrawGameMessage(turnMessage, turn);
            }

            Thread.Sleep(3000);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.SetCursorPosition(Left, Top - 5);
            Environment.Exit(0);
        }

        /// <summary>
        /// Performs all of the actions of a normal turn for a single player.
        /// </summary>
        /// <param name="turn"></param> true for player 1 and false for player 2
        public void TakeTurn(bool turn)
        {

            string playerNum = turn ? "1" : "2";
            string turnMessage = $"Player {playerNum}'s Turn";
            DrawGameMessage(turnMessage, turn);

            Piece currentPiece = new Piece(this, turn);
            currentPiece.Draw();

            while (!currentPiece.IsDropped)
            {
                ConsoleKey userInput = Console.ReadKey().Key;
                if (userInput == ConsoleKey.LeftArrow)
                {
                    currentPiece.Move(true);
                }
                else if (userInput == ConsoleKey.RightArrow)
                {
                    currentPiece.Move(false);
                }
                else if (userInput == ConsoleKey.Spacebar)
                {
                    currentPiece.Drop();
                    
                    Coordinates[Counter, 0] = currentPiece.X;
                    Coordinates[Counter, 1] = currentPiece.Y;
                   
                    if (currentPiece.Turn == true)
                        Coordinates[Counter, 2] = 1;
                    else
                        Coordinates[Counter, 2] = 2;
                    Counter++;
                }

                if (!currentPiece.IsDropped)
                {
                    currentPiece.Draw();
                }
            }
        }

        /// <summary>
        /// Checks to see if all the spaces have been filled in
        /// </summary>
        /// <returns>Returns true if all spaces have been filled</returns>
        public bool CheckForTie()
        {
            if (Counter == (GameBoard.GetLength(0) * GameBoard.GetLength(1)))
            {
                TieGame = true;
            }

            return TieGame;
        }

        /// <summary>
        /// Calls the hasMatch method and provides a strating point for each possible direction 
        /// in order to check all possible pieces from every starting point.
        /// </summary>
        /// <param name="x"></param> X coordinate starting position.
        /// <param name="y"></param> Y coordinate starting position.
        /// <param name="player"></param> Represents the current player that is being checked.
        /// <returns></returns> returns true if any direction fulfills the win condition, otherwise returns false.
        public bool StartingPoint(int x, int y, int player)
        {
           return HasMatch(x, y, 1, 0, player) || // Hoizontal Check
            HasMatch(x, y, 0, -1, player) || // Vertical Check
            HasMatch(x, y, 1, 1, player) || // Diagonal Down Check
            HasMatch(x, y, 1, -1, player); // Diagonal Up Check

        }
        /// <summary>
        /// Checks subsequent pieces for the win condition in a direction given by the x and y 
        /// difference parameters to determine direction.
        /// </summary>
        /// <param name="x"></param> X coordinate starting position.
        /// <param name="y"></param> Y coordinate starting position.
        /// <param name="xDiff"></param> Determines the X coordinate direction to check
        /// <param name="yDiff"></param> Determines the Y coordinate direction to check
        /// <param name="player"></param> Represents the current player that is being checked.
        /// <returns></returns> returns true if 3 matches are found, otherwise returns false.
        private bool HasMatch(int x, int y, int xDiff, int yDiff, int player)
        {
            
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < Counter; j++)
                {

                    if (Coordinates[j, 0] == (x + xDiff) 
                        && Coordinates[j, 1] == (y + yDiff) 
                        && Coordinates[j, 2] == player)
                    {
                        if (yDiff > 0)
                            yDiff++;
                        else if (yDiff < 0)
                            yDiff--;
                        if (xDiff > 0)
                            xDiff++;
                        else if (xDiff < 0)
                            xDiff--;

                        Matches++;
                    }
                    else { }

                }

            }

            if (Matches == 3)
                return true;
            else
                Matches = 0;
                return false;

        }
        /// <summary>
        /// Draws a message above the board to indicate which player's turn it is or a startup message.
        /// </summary>
        /// /// <param name="message"></param> Message to be overritten above the board.
        /// <param name="turn"></param> Implying it should be drawn in turn colors
        public void DrawGameMessage(string message, bool turn)
        {
            DrawGameMessage(message, getPlayerColor(turn));
        }

        /// <param name="message"></param> Message to be overritten above the board.
        /// <param name="fore"></param> Color of the text to be written.
        public void DrawGameMessage(string message, ConsoleColor fore)
        {
            DrawGameMessage(message, fore, ConsoleColor.Black, 5);
        }

        /// <param name="message"></param> Message to be overritten above the board.
        /// <param name="fore"></param> Color of the text to be written.
        /// <param name="back"></param> Color of the text background to be written
        /// <param name="heightFromTop"></param> How high above the board the text should be(shouldn't be greater than defaultDistance)
        public void DrawGameMessage(string message, ConsoleColor fore, ConsoleColor back, int heightFromTop)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(Left, Top - heightFromTop);
            Console.Write($"{{0, {50}}}", ' ');

            Console.BackgroundColor = back;
            Console.ForegroundColor = fore;

            Console.SetCursorPosition(Left, Top - heightFromTop);
            Console.Write(message);

            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Draws the board during the startup phase.
        /// </summary>
        public void Draw()
        {
            int height = (GameBoard.GetLength(1) * 2) + 1;
            int length = ((GameBoard.GetLength(0) * 2) + 1) * 2;

            Console.BackgroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(Left, Top + i);
                Console.WriteLine($"{{0, {length}}}", ' ');

            }

            Console.BackgroundColor = ConsoleColor.Black;

            for (int i = 1; i < height; i += 2)
            {
                for (int j = 2; j < length; j += 4)
                {
                    Console.SetCursorPosition(Left + j, Top + i);
                    Console.Write("  ");
                }
            }

            Console.SetCursorPosition(0, 0);
        }

        /// <summary>
        /// Returns the players color based on whos turn it is
        /// </summary>
        /// <param name="turn"></param>
        /// <returns>Color of the turn player</returns>
        public static ConsoleColor getPlayerColor(bool turn)
        {
            return turn ? ConsoleColor.Red : ConsoleColor.Cyan;
        }

    }
}


