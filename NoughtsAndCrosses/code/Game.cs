using System;
using System.Diagnostics;

namespace NoughtsAndCrosses
{
    partial class Game
    {
        static void Main(/*string[] args*/)
        {
            new Game(new StandardRuleset(), new ConsoleIOHandlerFactory()).Run();
        }

        private Game(IRuleset rules, IIOHandlerFactory ioHandlerFactory)
        {
            Debug.Assert(rules.Dimension < 10); // Input takes coord as single digit

            gameBoard = new Board(rules);

            inputHandler = ioHandlerFactory.CreateInputHandler();
            outputHandler = ioHandlerFactory.CreateOutputHandler();
            
            state = new NoughtsGameState(this, null); // Noughts goes first
        }

        private void Run()
        {
            try
            {
                while (true)
                {
                    state = state.Advance();
                }
            }
            catch (Exception e)
            {
                outputHandler.ReportError(e.Message, true);
            }
        }

        private Board gameBoard;
        private IInputHandler inputHandler;
        private IOutputHandler outputHandler;
        private GameState state;
    }
}
