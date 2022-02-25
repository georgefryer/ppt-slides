using System;

namespace NoughtsAndCrosses
{
    enum Piece
    {
        Blank,
        Nought,
        Cross
    }

    class TileCoords
    {
        public uint X { get; private set; }
        public uint Y { get; private set; }

        public TileCoords(uint x, uint y)
        {
            X = x;
            Y = y;
        }
    }

    class Board
    {
        public Board(IRuleset rules)
        {
            this.rules = rules;

            tileContents = new Piece[Dimension, Dimension];
            ClearBoard();
        }

        public void PlayPiece(TileCoords tile, Piece piece)
        {
            VerifyTileCoords(tile);

            if (tileContents[tile.X, tile.Y] != Piece.Blank)
            {
                throw new ArgumentException("Tile has already been played.", "tile");
            }

            tileContents[tile.X, tile.Y] = piece;
        }

        public Piece GetPieceAtTile(TileCoords tile)
        {
            VerifyTileCoords(tile);

            return tileContents[tile.X, tile.Y];
        }

        public void ClearBoard()
        {
            for (uint x = 0; x < Dimension; x++)
            {
                for (uint y = 0; y < Dimension; y++)
                {
                    tileContents[x, y] = Piece.Blank;
                }
            }
        }

        public bool IsFilled()
        {
            for (uint x = 0; x < Dimension; x++)
            {
                for (uint y = 0; y < Dimension; x++)
                {
                    if (tileContents[x,y] == Piece.Blank)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool HasPlayerWon(Piece playerPiece)
        {
            return rules.HasPlayerWon(playerPiece, this);
        }

        public uint Dimension => rules.Dimension;

        private void VerifyTileCoords(TileCoords tile)
        {
            if (tile.X >= Dimension || tile.Y >= Dimension)
            {
                throw new ArgumentOutOfRangeException("tile", "Tile is out of board bounds.");
            }
        }

        private IRuleset rules;
        private Piece[,] tileContents;
    }
}
