using System;
using System.Collections.Generic;

namespace NoughtsAndCrosses
{
    interface IRuleset
    {
        bool HasPlayerWon(Piece playerPiece, Board gameBoard);

        uint Dimension { get; }
    }

    class StandardRuleset : IRuleset
    {
        public bool HasPlayerWon(Piece playerPiece, Board gameBoard)
        {
            Predicate<Piece> isPlayerPiece = (Piece piece) => piece == playerPiece;

            for (uint x = 0; x < Dimension; x++)
            {
                List<Piece> currentColumn = new List<Piece>();

                for (uint y = 0; y < Dimension; y++)
                {
                    TileCoords tile = new TileCoords(x, y);
                    currentColumn.Add(gameBoard.GetPieceAtTile(tile));
                }
                
                if (currentColumn.TrueForAll(isPlayerPiece))
                {
                    return true;
                }
            }

            for (uint y = 0; y < Dimension; y++)
            {
                List<Piece> currentRow = new List<Piece>();

                for (uint x = 0; x < Dimension; x++)
                {
                    TileCoords tile = new TileCoords(x, y);
                    currentRow.Add(gameBoard.GetPieceAtTile(tile));
                }

                if (currentRow.TrueForAll(isPlayerPiece))
                {
                    return true;
                }
            }

            List<Piece> diagFromTopLeft = new List<Piece>();
            List<Piece> diagFromBottomLeft = new List<Piece>();

            for (uint i = 0; i < Dimension; i++)
            {
                TileCoords fromTopLeft = new TileCoords(i, i); 
                TileCoords fromBottomLeft = new TileCoords(i, Dimension - 1 - i);
                diagFromTopLeft.Add(gameBoard.GetPieceAtTile(fromTopLeft));
                diagFromBottomLeft.Add(gameBoard.GetPieceAtTile(fromBottomLeft));
            }

            if (diagFromTopLeft.TrueForAll(isPlayerPiece) || diagFromBottomLeft.TrueForAll(isPlayerPiece))
            {
                return true;
            }

            return false;
        }

        public uint Dimension => 3;
    }
}
