using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Connect4
{
    /// <summary>
    /// Represents the core of the game providing rules such as the win condition and 
    /// determining how to setup the game.
    /// </summary>
    /// /// <author>
    /// Connor O'Leary and Jeff Daffern
    /// </author>
    class Game
    {
        public Board gameBoard;
        public bool turn;

        /// <summary>
        /// Starts the game and keeps it running until the win condition is met.
        /// </summary>
        public void Start()
        {
            Setup();

            while (!Console.KeyAvailable)
            {
                Thread.Sleep(500);
            }

            while (!GameOver())
            {
                turn = !turn;
                gameBoard.TakeTurn(turn);
            }

            gameBoard.GameOver(turn);
        }
        /// <summary>
        /// Win condition determined by 4 matching pieces.
        /// </summary>
        /// <returns></returns>
        private bool GameOver()
        {
            bool gameOver = false;
            for (int i = 0; i < gameBoard.Counter; i++)
            {
                 gameOver = gameBoard.StartingPoint(gameBoard.Coordinates[i, 0], gameBoard.Coordinates[i, 1], gameBoard.Coordinates[i, 2]);
                if (gameOver)
                    break;
            }
            if (!gameOver)
            {
                gameOver = gameBoard.CheckForTie();
            }
            return gameOver;
        }
        /// <summary>
        /// Board setup where the method calls to draw the board on the console 
        /// are performed and a new board is created.
        /// </summary>
        private void Setup()
        {
            // Test random Gameboard
            //Random rand = new Random();
            //gameBoard = new Board(rand.Next(0,10), rand.Next(0, 10), rand.Next(2, 10), rand.Next(2, 10));

            gameBoard = new Board(5, 2, 7, 6);
            gameBoard.Draw();

            turn = false;
        }
    }
}


