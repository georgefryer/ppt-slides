using System;
using static NoughtsAndCrosses.Game;

namespace NoughtsAndCrosses
{
    interface IInputHandler
    {
        TileCoords GetInput(GameState state);
    }

    class ConsoleInputHandler : IInputHandler
    {
        public TileCoords GetInput(GameState state)
        {
            if (state is EndGameState)
            {
                Console.Write("Press any key to play again.");
                Console.ReadKey(true);

                return new TileCoords(0,0);
            }
            else if (state is PlayerGameState playerState)
            {
                Console.WriteLine($"{playerState.PlayerName}'{(playerState.PlayerName.EndsWith("s") ? "" : "s")} turn.");

                Func<string, uint> readDigit = null;
                readDigit = (string prompt) =>
                {
                    try
                    {
                        Console.Write(prompt);
                        return Convert.ToUInt32(Console.ReadKey().KeyChar.ToString(), 10) - 1;
                    }
                    catch (Exception)
                    {
                        // Clear current line
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new string(' ', prompt.Length + 1));
                        Console.SetCursorPosition(0, Console.CursorTop);

                        return readDigit(prompt);
                    }
                };

                uint x = readDigit("x: ");
                Console.WriteLine();

                uint y = readDigit("y: ");
                Console.WriteLine();

                return new TileCoords(x, y);
            }
            else
            {
                throw new ArgumentException("Cannot process input for this game state.");
            }
        }
    }
}
