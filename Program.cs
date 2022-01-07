namespace Connect4
{
    /// <summary>
    /// Represents the game Connect4 and calls the game to get things started.
    /// </summary>
    /// /// <author>
    /// Connor O'Leary and Jeff Daffern
    /// </author>
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Start();
        }
    }
}