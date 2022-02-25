using System;

namespace NoughtsAndCrosses
{
    partial class Game
    {
        public abstract class GameState
        {
            public GameState(Game game, GameState previousState)
            {
                this.game = game;
                this.previousState = previousState;
            }

            public abstract GameState Advance();

            protected Game game;
            protected GameState previousState;
        }

        public abstract class PlayerGameState : GameState
        {
            public PlayerGameState(Game game, GameState previousState) : base(game, previousState) { }

            public virtual Piece PlayerPiece { get; }
            public string PlayerName => PlayerPiece.ToString();

            public override GameState Advance()
            {
                game.outputHandler.UpdateOutput(this, game.gameBoard);
                ProcessTurn();

                if (game.gameBoard.HasPlayerWon(PlayerPiece))
                {
                    return new WinGameState(game, null);
                }
                else
                {
                    if (game.gameBoard.IsFilled())
                    {
                        return new DrawnGameState(game, this);
                    }

                    return (PlayerGameState)Activator.CreateInstance(NextPlayerGameState, game, this);
                }
            }

            protected void ProcessTurn()
            {
                TileCoords tilePlayed = game.inputHandler.GetInput(this);
                try
                {
                    game.gameBoard.PlayPiece(tilePlayed, PlayerPiece);
                }
                catch (ArgumentException)
                {
                    game.outputHandler.CustomOutput("Invalid tile, choose again.");
                    ProcessTurn();
                }
            }

            protected virtual Type NextPlayerGameState { get; }
        }

        public class NoughtsGameState : PlayerGameState
        {
            public NoughtsGameState(Game game, GameState previousState) : base(game, previousState) { }

            public override Piece PlayerPiece => Piece.Nought;
            protected override Type NextPlayerGameState => typeof(CrossesGameState);
        }

        public class CrossesGameState : PlayerGameState
        {
            public CrossesGameState(Game game, GameState previousState) : base(game, previousState) { }

            public override Piece PlayerPiece => Piece.Cross;
            protected override Type NextPlayerGameState => typeof(NoughtsGameState);
        }

        public abstract class EndGameState : GameState
        {
            public EndGameState(Game game, GameState previousState) : base(game, previousState) { }

            public override GameState Advance()
            {
                game.outputHandler.CustomOutput(OutputString, true);
                game.inputHandler.GetInput(this);

                game.gameBoard.ClearBoard();
                return new NoughtsGameState(game, this);
            }

            protected virtual string OutputString { get; }
        }

        public class DrawnGameState : EndGameState
        {
            public DrawnGameState(Game game, GameState previousState) : base(game, previousState) { }

            protected override string OutputString => "Game ended in a draw.";
        }

        public class WinGameState : EndGameState
        {
            public WinGameState(Game game, GameState previousState) : base(game, previousState) { }

            protected override string OutputString
            {
                get
                {
                    PlayerGameState previousPlayerState = previousState as PlayerGameState;
                    string winningPlayerName = previousPlayerState.PlayerName;
                    return $"{winningPlayerName}{(winningPlayerName.EndsWith("s") ? "es" : "s")} has won!";
                }
            }
        }
    }
}
