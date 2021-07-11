using System;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        static GameField CreateGameField() 
        {
            int x, y;
            Console.WriteLine("Введите игровое поле:");
            Console.Write("x = ");
            bool xIsInt = Int32.TryParse(Console.ReadLine(), out x);
            Console.Write("y = ");
            bool yIsInt = Int32.TryParse(Console.ReadLine(), out y);
            if (xIsInt == false || yIsInt == false)
            {
                Error("Нужно вводить только числа!\nПопробуйте сначала", 2000);
                Program.Main();
            }
            if (x < 3 || x > 5 || y < 3 || y > 5)
            {
                Error("Разрешено поле только от 3x3 до 5x5\nПопробуйте сначала", 2000);
                Program.Main();
            }
            Console.Clear();
            return (new GameField(y, x));
        }

        static void CheckWins(GameField gameField, int y, int x, char symbol) 
        {
            if (gameField.CheckWins(y, x, symbol) == true)
            {
                Console.Clear();
                gameField.OutputField();
                Console.WriteLine($"Победил {symbol}!!!");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        static void CheckDraw(GameField gameField) 
        {
            if (gameField.CheckDraw() == true)
            {
                Console.Clear();
                gameField.OutputField();
                Console.WriteLine("Победила дружба!!!");
                Thread.Sleep(2000);
                Environment.Exit(0);
            }
        }

        static void Error(string error, int messageInSecond)
        {
            Console.WriteLine(error);
            Thread.Sleep(messageInSecond);
            Console.Clear();
        }

        static void PlayerVsPlayer() 
        {
            GameField gameField = CreateGameField();

            gameField.OutputField();

            char cross = 'X';
            while (true)
            {
                int y, x;
                if (cross == 'X')
                {
                    Console.WriteLine("Ходит крестик.");
                    Console.WriteLine("Введите позицию крестика:");

                    Console.Write("y = ");
                    y = (Convert.ToInt32(Console.ReadLine())) - 1;
                    Console.Write("x = ");
                    x = (Convert.ToInt32(Console.ReadLine())) - 1;
                    if (y > gameField.Y || y < 0 || x > gameField.X || x < 0)
                    {
                        Error("Координаты вне поля попробуйте сначала!", 1000);
                        gameField.OutputField();
                        continue;
                    }
                    if (gameField.CellIsBusy(y, x) == true)
                    {
                        Error("Клетка занята, выберете сначала!", 1000);
                        gameField.OutputField();
                        continue;
                    }
                    else
                        gameField.SetCharInCell(y, x, 'X');
                    CheckWins(gameField, y, x, cross);
                    CheckDraw(gameField);
                    cross = 'O';
                }
                else
                {
                    Console.WriteLine("Ходит нолик.");
                    Console.WriteLine("Введите позицию нолика:");

                    Console.Write("y = ");
                    y = (Convert.ToInt32(Console.ReadLine())) - 1;
                    Console.Write("x = ");
                    x = (Convert.ToInt32(Console.ReadLine())) - 1;
                    if (y > gameField.Y || y < 0 || x > gameField.X || x < 0)
                    {
                        Error("Координаты вне поля попробуйте сначала!", 1000);
                        gameField.OutputField();
                        continue;
                    }
                    if (gameField.CellIsBusy(y, x) == true)
                    {
                        Error("Клетка занята, выберете сначала!", 1000);
                        gameField.OutputField();
                        continue;
                    }
                    else
                        gameField.SetCharInCell(y, x, 'O');
                    CheckWins(gameField, y, x, cross);
                    CheckDraw(gameField);
                    cross = 'X';
                }
                Console.Clear();
                gameField.OutputField();
            }
        }

        static void PlayerVsComputer() 
        {
            GameField gameField = CreateGameField();
            Console.Write("Выберите сторону игры (X или O): ");
            char playerSide = '\0';
            bool isSymbol = Char.TryParse(Console.ReadLine(), out playerSide);
            if (isSymbol == false)
            {
                Error("Извините вы ошиблись принимаются только символы!", 2000);
                PlayerVsComputer();
            }
            if (playerSide == 'O' || playerSide == 'X')
            {
                Console.Clear();
                gameField.OutputField();
                char computerSide = playerSide == 'X' ? 'O' : 'X';
                GameBot gameBot = new GameBot(computerSide, playerSide, gameField.Y, gameField.X, gameField.X);
                string cross;

                bool botFirst;
                if (playerSide == 'X')
                {
                    cross = "Human";
                    botFirst = false;
                }
                else
                {
                    cross = "Bot";
                    botFirst = true;
                }
                int stageAI = 0;
                while (true)
                {
                    if (string.Equals(cross, "Human"))
                    {
                        Console.WriteLine("Введите координаты:");
                        Console.Write("y = ");
                        int y;
                        bool isIntY = int.TryParse(Console.ReadLine(), out y);//Convert.ToInt32(Console.ReadLine());
                        Console.Write("x = ");
                        int x;
                        /*int x*/
                        bool isIntX = int.TryParse(Console.ReadLine(), out x);//Convert.ToInt32(Console.ReadLine());
                        if (y > gameField.Y || y < 0 || x > gameField.X || x < 0 || isIntY == false || isIntX == false)
                        {
                            Error("Координаты вне поля попробуйте сначала!", 1000);
                            gameField.OutputField();
                            continue;
                        }
                        if (gameField.CellIsBusy(y - 1, x - 1) == true)
                        {
                            Error("Клетка занята, выберете сначала!", 1000);
                            gameField.OutputField();
                            continue;
                        }
                        else
                            gameField.SetCharInCell(y - 1, x - 1, playerSide);
                        CheckWins(gameField, y - 1, x - 1, playerSide);
                        CheckDraw(gameField);
                        cross = "Bot";
                    }
                    else
                    {
                        cross = "Human";
                        if (botFirst == false)
                            gameBot.BestMoveAI(ref gameField, stageAI);
                        //position = gameBot.Minimax(gameField, 5, true, 0, 0, computerSide);
                        else
                            gameBot.BestMoveAI(ref gameField, stageAI);
                        CheckWins(gameField, gameBot.lastMoveY, gameBot.lastMoveX, computerSide);
                        CheckDraw(gameField);
                        stageAI++;
                    }
                    Console.Clear();
                    gameField.OutputField();
                }
            }
            else
            {
                Error("Вы ошиблись принимаются только либо (X или O)", 2000);
                PlayerVsComputer();
            }
        }

        static public void ComputerVsComputer() 
        {
            GameField gameField = CreateGameField();
            GameBot bot1 = new GameBot('X', 'O', gameField.Y, gameField.X, gameField.X * gameField.Y);
            GameBot bot2 = new GameBot('O', 'X', gameField.Y, gameField.X, gameField.X * gameField.Y);
            char cross = 'X';
            while (true) 
            {
                if (cross == 'X')
                {
                    bot1.RandMove(ref gameField);
                    CheckWins(gameField, bot1.lastMoveY, bot1.lastMoveX, 'X');
                    CheckDraw(gameField);
                    cross = 'O';
                }
                else 
                {
                    bot2.RandMove(ref gameField);
                    CheckWins(gameField, bot2.lastMoveY, bot2.lastMoveX, 'X');
                    CheckDraw(gameField);
                    cross = 'X';
                }
                Console.Clear();
                gameField.OutputField();
                Thread.Sleep(500);
            }
        }

        static void Main()
        {
            Console.WriteLine("Режимы игры: ");
            Console.WriteLine("1) Игрок против компьютера.");
            Console.WriteLine("2) Игрок против игрока.");
            Console.WriteLine("3) Компьютер против компьютера.");
            Console.Write("Введите цифру выбрав режим игры: ");

            int gameMode = Convert.ToInt32(Console.ReadLine());
            if (gameMode < 1 || gameMode > 3)
            {
                Error("Извините такого режима игры нет!!!", 2000);
                Main();
            }
            else
            {
                Console.Clear();
                if (gameMode == 1)
                    PlayerVsComputer();
                if (gameMode == 2)
                    PlayerVsPlayer();
                if (gameMode == 3)
                    ComputerVsComputer();
            }
            Console.ReadLine();
        }
    }
}
