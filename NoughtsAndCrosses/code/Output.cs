using System;
using static NoughtsAndCrosses.Game;

namespace NoughtsAndCrosses
{
    interface IOutputHandler
    {
        void UpdateOutput(GameState state, Board gameBoard);
        void CustomOutput(string message, bool clearScr = false);
        void ReportError(string errorMessage, bool clearScr = false);
    }

    class ConsoleOutputHandler : IOutputHandler
    {
        public void UpdateOutput(GameState state, Board gameBoard)
        {
            Console.Clear();

            for (uint y = 0; y < gameBoard.Dimension; y++)
            {
                for (uint x = 0; x < gameBoard.Dimension; x++)
                {
                    TileCoords tile = new TileCoords(x, y);
                    Console.Write($"{PieceToChar(gameBoard.GetPieceAtTile(tile))}");
                    if (x != gameBoard.Dimension - 1)
                    {
                        Console.Write("|");
                    }
                }

                Console.WriteLine();

                if (y != gameBoard.Dimension - 1)
                {
                    Console.Write(new string('-', (int)(2*gameBoard.Dimension - 1)));
                }

                Console.WriteLine();
            }
        }

        public void CustomOutput(string message, bool clearScr)
        {
            if (clearScr)
            {
                Console.Clear();
            }
            Console.WriteLine(message);
        }

        public void ReportError(string errorMessage, bool clearScr)
        {
            CustomOutput($"Error: {errorMessage}", clearScr);
            Console.ReadKey(true);
        }

        private char PieceToChar(Piece piece)
        {
            switch (piece)
            {
                case Piece.Blank:
                    return ' ';
                case Piece.Nought:
                    return 'O';
                case Piece.Cross:
                    return 'X';
            }

            throw new InvalidOperationException("Piece to character conversion doesn't exist.");
        }
    }
}
