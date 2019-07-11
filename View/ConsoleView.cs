using System;
using System.IO;
using System.Numerics;
using ConsolePong.GameLogic;

namespace ConsolePong.View
{
    internal class ConsoleView : IView
    {
        public char WallSymbol { get; } = '#';
        public char BallSymbol { get; } = '@';
        public char PaddleSymbol { get; } = '%';

        private readonly char[][] _field;
        private int _fieldSizeX;
        private int _fieldSizeY;
        private readonly GameSettings _gameSettings;

        public ConsoleView(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;

            CalcScreenSize();

            _field = new char[_fieldSizeY][];

            for (int y = 0; y < _fieldSizeY; y++)
            {
                _field[y] = new char[_fieldSizeX];
            }
        }

        private void CalcScreenSize()
        {
            var maxCharactersPerLine = 80;
            try
            {
                maxCharactersPerLine = Console.WindowWidth - 1;
            }
            catch (IOException)
            {
                Console.WriteLine($"Cannot get Console.WindowWidth -> use default width of {maxCharactersPerLine}");
            }

            _fieldSizeX = maxCharactersPerLine - 2;
            _fieldSizeY = (int)Math.Round(_fieldSizeX * _gameSettings.FieldSize.Y / _gameSettings.FieldSize.X);

            if (_fieldSizeY + 3 > Console.WindowHeight)
            {
                _fieldSizeY = Console.WindowHeight - 4;
                _fieldSizeX = (int)Math.Round(_fieldSizeY * _gameSettings.FieldSize.X / _gameSettings.FieldSize.Y);
            }
        }

        public void Show(GameState gameState, string leftControllerName, string rightControllerName)
        {
            ClearField();

            if (gameState.GameResult != GameResult.LeftPlayerWon)
            {
                SetWinnerMessage($"{leftControllerName} (Left) won");
            }
            else if (gameState.GameResult != GameResult.RightPlayerWon)
            {
                SetWinnerMessage($"{rightControllerName} (Right) won");
            }

            SetPaddle(gameState.LeftPaddle);
            SetPaddle(gameState.RightPaddle);
            SetBall(gameState.Ball);

            DrawField(gameState, leftControllerName, rightControllerName);
        }

        private void SetWinnerMessage(string winnerMessage)
        {

        }

        private void SetPaddle(Paddle paddle)
        {
            (int paddleXStart, int paddleYStart) = ToConsoleCoordinates(paddle.Position - _gameSettings.PaddleSize * 0.5f);
            (int paddleXEnd, int paddleYEnd) = ToConsoleCoordinates(paddle.Position + _gameSettings.PaddleSize * 0.5f);

            for (int y = paddleYStart; y <= paddleYEnd; y++)
            {
                for (int x = paddleXStart; x <= paddleXEnd; x++)
                {
                    try
                    {
                        _field[y][x] = PaddleSymbol;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        // do nothing
                    }

                }
            }
        }

        private void SetBall(Ball ball)
        {
            (int ballXStart, int ballYStart) = ToConsoleCoordinates(ball.Position - Vector2.One * (_gameSettings.BallSize * 0.5f));
            (int ballXEnd, int ballYEnd) = ToConsoleCoordinates(ball.Position + Vector2.One * (_gameSettings.BallSize * 0.5f));

            for (int y = ballYStart; y <= ballYEnd; y++)
            {
                for (int x = ballXStart; x <= ballXEnd; x++)
                {
                    var gameCoords = ToGameCoordinates(x, y);
                    if ((gameCoords - ball.Position).LengthSquared() < _gameSettings.BallSize * _gameSettings.BallSize * 0.25f)
                    {
                        _field[y][x] = BallSymbol;
                    }
                }
            }
        }

        private (int x, int y) ToConsoleCoordinates(Vector2 coords)
        {
            var consoleCoords = coords / _gameSettings.FieldSize * new Vector2(_fieldSizeX, _fieldSizeY);

            consoleCoords -= new Vector2(0.5f, 0.5f);

            return ((int)Math.Round(consoleCoords.X), (int)Math.Round(consoleCoords.Y));
        }

        private Vector2 ToGameCoordinates(int x, int y)
        {
            return new Vector2(x + 0.5f, y + 0.5f) / new Vector2(_fieldSizeX, _fieldSizeY) * _gameSettings.FieldSize;
        }

        private void DrawField(GameState gameState, string leftControllerName, string rightControllerName)
        {
            Console.SetCursorPosition(0, 0);

            //Console.WriteLine($"Fps: {gameState.Fps:F0} Res: {_fieldSizeX}x{_fieldSizeY} Ball: {gameState.Ball.Position.X} {gameState.Ball.Position.Y}");
            Console.WriteLine(leftControllerName + 
                              new string(' ', _fieldSizeX + 2 - leftControllerName.Length - rightControllerName.Length) + 
                              rightControllerName);

            Console.WriteLine(new string(WallSymbol, _fieldSizeX + 2));

            for (int line = _fieldSizeY - 1; line >= 0; line--)
            {
                Console.Write(WallSymbol);
                Console.Write(new string(_field[line]));
                Console.WriteLine(WallSymbol);
            }

            Console.WriteLine(new string(WallSymbol, _fieldSizeX + 2));
        }

        private void ClearField()
        {
            for (int y = 0; y < _fieldSizeY; y++)
            {
                for (int x = 0; x < _fieldSizeX; x++)
                {
                    _field[y][x] = ' ';
                }
            }
        }
    }
}