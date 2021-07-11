using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
    struct DataBot 
    {
        public int BestMoveY;
        public int BestMoveX;
        public int ScoreEnemyPieces;

        public DataBot(int bestMoveY, int bestMoveX, int scoreEnemyPieces)
        {
            this.BestMoveY = bestMoveY;
            this.BestMoveX = bestMoveX;
            this.ScoreEnemyPieces = scoreEnemyPieces;
        }
    }

    class GameBot
    {
        public int lastMoveY;
        public int lastMoveX;
        private char _computerSide;
        private char _playerSide;

        private int countRandMoves;
        private int maxRandMoves;

        private readonly int maxNegativeInt = -2147483648;
        public GameBot(char computerSide, char playerSide, int y, int x, int maxRandMoves)
        {
            this._computerSide = computerSide;
            this._playerSide = playerSide;
            this.countRandMoves = 0;
            if (maxRandMoves < 4)
                this.maxRandMoves = maxRandMoves;
            else
                this.maxRandMoves = (y * x) - y;
        }

        public void BestMoveAI(ref GameField gameField, int stageAI)
        {
            int tempY = gameField.Y - 1;
            int tempX = gameField.X - 1;
            if (stageAI == 0)
            {
                CloseMainDiagonal(ref gameField, tempY, tempX);
            }
            else
            {
                DataBot dataBot = new DataBot(0, 0, maxNegativeInt);

                CheckMainDiagonal(ref dataBot, gameField, tempY, tempX);
                CheckHorizontalLines(ref dataBot, gameField, tempY, tempX);
                CheckVerticalLines(ref dataBot, gameField, tempY, tempX);
                CheckSideDiagonal(ref dataBot, gameField, tempY, tempX);
                if (dataBot.BestMoveY == 0 && dataBot.BestMoveX == 0)
                    RandMove(ref gameField);
                else
                    gameField.Field[dataBot.BestMoveY, dataBot.BestMoveX] = _computerSide;
            }
        }

        public void RandMove(ref GameField field)
        {
            Random randomY = new Random();
            Random randomX = new Random();

            bool isEmptyCells = true;
            while (isEmptyCells == true)
            {
                int y = randomY.Next(0, field.Y);
                int x = randomX.Next(0, field.X);

                if (field.Field[y, x] == '_')
                {
                    field.Field[y, x] = _computerSide;
                    this.countRandMoves++;
                    this.lastMoveY = y;
                    this.lastMoveX = x;
                    isEmptyCells = false;
                }
            }
        }

        private void CloseMainDiagonal(ref GameField gameField, int tempY, int tempX) 
        {
            bool firstCellBusy = false;
            if (gameField.Field[0, 0] == this._playerSide)
                firstCellBusy = true;
            if (firstCellBusy == true)
                gameField.Field[gameField.Y - 1, gameField.X - 1] = this._computerSide;
            else
                gameField.Field[0, 0] = this._computerSide;
        }

        private void CheckMainDiagonal(ref DataBot dataBot, GameField gameField, int tempY, int tempX)
        {
            int mainCountPieces = 0;
            int mainDiagonalX = 0;
            for (int mainDiagonalY = 0; mainDiagonalY <= tempY; mainDiagonalY++)
            {
                if (gameField.Field[mainDiagonalY, mainDiagonalX] == this._playerSide)
                    mainCountPieces++;
                mainDiagonalX++;
            }
            mainDiagonalX = 0;
            if (mainCountPieces > dataBot.ScoreEnemyPieces)
            {
                dataBot.ScoreEnemyPieces = mainCountPieces;
                for (int mainHorizontalY = tempY; mainHorizontalY >= 0; mainHorizontalY--)
                {
                    if (gameField.Field[mainHorizontalY, mainDiagonalX] == '_')
                    {
                        dataBot.BestMoveY = mainHorizontalY;
                        dataBot.BestMoveX = mainDiagonalX;
                        break;
                    }
                    mainDiagonalX++;
                }
            }
        }

        private void CheckSideDiagonal(ref DataBot dataBot, GameField gameField, int tempY, int tempX) 
        {
            int sideCountPieces = 0;
            int sideDiagonalX = tempX;
            for (int sideDiagonalY = 0; sideDiagonalY <= tempY; sideDiagonalY++)
            {
                if (gameField.Field[sideDiagonalY, sideDiagonalX] == this._playerSide)
                    sideCountPieces++;
                sideDiagonalX--;
            }
            sideDiagonalX = tempX;
            if (sideCountPieces > dataBot.ScoreEnemyPieces)
            {
                dataBot.ScoreEnemyPieces = sideCountPieces;
                for (int searchEmptySymbolY = 0; searchEmptySymbolY <= tempY; searchEmptySymbolY++)
                {
                    if (gameField.Field[searchEmptySymbolY, sideDiagonalX] == '_')
                    {
                        dataBot.BestMoveY = searchEmptySymbolY;
                        dataBot.BestMoveX = sideDiagonalX;
                    }
                    sideDiagonalX--;
                }
            }
        }

        private void CheckHorizontalLines(ref DataBot dataBot, GameField gameField, int tempY, int tempX) 
        {
            for (int horizontalLineY = 0; horizontalLineY <= tempY; horizontalLineY++)
            {
                int inSideCountPieces = 0;
                for (int horizontalLineX = 0; horizontalLineX <= tempX; horizontalLineX++)
                {
                    if (gameField.Field[horizontalLineY, horizontalLineX] == this._playerSide)
                        inSideCountPieces++;
                }

                if (inSideCountPieces > dataBot.ScoreEnemyPieces)
                {
                    dataBot.ScoreEnemyPieces = inSideCountPieces;
                    for (int horizontalLineX = 0; horizontalLineX <= tempX; horizontalLineX++)
                    {
                        if (gameField.Field[horizontalLineY, horizontalLineX] == '_')
                        {
                            dataBot.BestMoveY = horizontalLineY;
                            dataBot.BestMoveX = horizontalLineX;
                            break;
                        }
                    }
                }
            }
        }

        private void CheckVerticalLines(ref DataBot dataBot, GameField gameField, int tempY, int tempX) 
        {
            for (int verticalLineX = 0; verticalLineX <= tempX; verticalLineX++)
            {
                int inSideCountPieces = 0;
                for (int verticalLineY = 0; verticalLineY <= tempY; verticalLineY++)
                {
                    if (gameField.Field[verticalLineY, verticalLineX] == this._playerSide)
                        inSideCountPieces++;
                }
                if (inSideCountPieces >= dataBot.ScoreEnemyPieces)
                {
                    Console.WriteLine($"{inSideCountPieces}");
                    dataBot.ScoreEnemyPieces = inSideCountPieces;
                    for (int symbolInLine = 0; symbolInLine <= tempY; symbolInLine++)
                    {
                        Console.WriteLine($"{gameField.Field[symbolInLine, verticalLineX]}");
                        if (gameField.Field[symbolInLine, verticalLineX] == '_')
                        {
                            dataBot.BestMoveY = symbolInLine;
                            dataBot.BestMoveX = verticalLineX;
                            break;
                        }
                    }
                }
            }
        }

    }
}
