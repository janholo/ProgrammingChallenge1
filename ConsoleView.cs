using System;
using System.Numerics;

namespace ConsolePong
{
    internal class ConsoleView
    {
        public char WallSymbol { get; } = '#';
        public char BallSymbol { get; } = '@';
        public char PaddleSymbol { get; } = '%';

        private char[][] _field;

        public int _maxCharactersPerLine;

        private int _fieldSizeX;
        private int _fieldSizeY;
        private GameSettings _gameSettings;
        private bool firstDraw = true;

        public ConsoleView(int maxCharactersPerLine, GameSettings gameSettings)
        {
            _gameSettings = gameSettings;

            if (maxCharactersPerLine <= 0)
            {
                throw new ArgumentException("Parameter " + nameof(maxCharactersPerLine) + "has to be greater than 0");
            }
            _maxCharactersPerLine = maxCharactersPerLine;

            _fieldSizeX = _maxCharactersPerLine - 2;
            _fieldSizeY = (int)Math.Round(_fieldSizeX * gameSettings.FieldSize.Y / gameSettings.FieldSize.X);

            _field = new char[_fieldSizeY][];

            for (int y = 0; y < _fieldSizeY; y++)
            {
                _field[y] = new char[_fieldSizeX];
            }

            Console.WriteLine("Field Size: " + _fieldSizeX + " x " + _fieldSizeY);

        }

        public void Show(GameState gamestate)
        {
            ClearField();

            SetPaddle(gamestate.LeftPaddle);
            SetPaddle(gamestate.RightPaddle);
            SetBall(gamestate.Ball);

            DrawField();
        }

        private void SetPaddle(Paddle paddle)
        {
            (int paddleXStart, int paddleYStart) = ToConsoleCoordinates(paddle.Position - _gameSettings.PaddleSize * 0.5f);
            (int paddleXEnd, int paddleYEnd) = ToConsoleCoordinates(paddle.Position + _gameSettings.PaddleSize * 0.5f);

            for (int y = paddleYStart; y <= paddleYEnd; y++)
            {
                for (int x = paddleXStart; x <= paddleXEnd; x++)
                {
                    _field[y][x] = PaddleSymbol;
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

            return ((int)Math.Round(consoleCoords.X), (int)Math.Round(consoleCoords.Y));
        }

        private Vector2 ToGameCoordinates(int x, int y)
        {
            return new Vector2(x, y) / new Vector2(_fieldSizeX, _fieldSizeY) * _gameSettings.FieldSize;
        }

        private void DrawField()
        {
            if(!firstDraw)
            {
                Console.SetCursorPosition(0, Console.CursorTop - (_fieldSizeY + 3));
            }
            
            Console.WriteLine(new string(WallSymbol, _maxCharactersPerLine));

            for (int line = _fieldSizeY - 1; line >= 0; line--)
            {
                Console.Write(WallSymbol);
                Console.Write(new string(_field[line]));
                Console.WriteLine(WallSymbol);
            }

            Console.WriteLine(new string(WallSymbol, _maxCharactersPerLine));

            firstDraw = false;
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