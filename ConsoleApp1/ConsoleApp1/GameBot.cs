using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
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
                int scoreEnemyPieces = maxNegativeInt;
                int bestMoveY = 0;
                int bestMoveX = 0;

                for (int horizontalLineY = 0; horizontalLineY <= tempY; horizontalLineY++)
                {
                    int inSideCountPieces = 0;
                    for (int horizontalLineX = 0; horizontalLineX <= tempX; horizontalLineX++)
                    {
                        if (gameField.Field[horizontalLineY, horizontalLineX] == this._playerSide)
                            inSideCountPieces++;
                    }

                    if (inSideCountPieces > scoreEnemyPieces)
                    {
                        scoreEnemyPieces = inSideCountPieces;
                        for (int horizontalLineX = 0; horizontalLineX < tempX; horizontalLineX++)
                        {
                            if (gameField.Field[horizontalLineY, horizontalLineX] == '_')
                            {
                                bestMoveY = horizontalLineY;
                                bestMoveX = horizontalLineX;
                                break;
                            }
                        }
                    }
                }
                for (int verticalLineX = 0; verticalLineX <= tempX; verticalLineX++)
                {
                    int inSideCountPieces = 0;
                    for (int verticalLineY = 0; verticalLineY <= tempY; verticalLineY++)
                    {
                        if (gameField.Field[verticalLineY, verticalLineX] == this._playerSide)
                            inSideCountPieces++;
                    }
                    if (inSideCountPieces > scoreEnemyPieces)
                    {
                        scoreEnemyPieces = inSideCountPieces;
                        for (int symbolInLine = 0; symbolInLine < tempY; symbolInLine++)
                        {
                            if (gameField.Field[symbolInLine, verticalLineX] == '_')
                            {
                                bestMoveY = symbolInLine;
                                bestMoveX = verticalLineX;
                                break;
                            }
                        }
                    }
                }
                int sideCountPieces = 0;
                int sideDiagonalX = tempX;
                for (int sideDiagonalY = 0; sideDiagonalY <= tempY; sideDiagonalY++)
                {
                    if (gameField.Field[sideDiagonalY, sideDiagonalX] == this._playerSide)
                        sideCountPieces++;
                    sideDiagonalX--;
                }
                sideDiagonalX = tempX;
                if (sideCountPieces >= scoreEnemyPieces)
                {
                    scoreEnemyPieces = sideCountPieces;
                    for (int searchEmptySymbolY = 0; searchEmptySymbolY <= tempY; searchEmptySymbolY++)
                    {
                        if (gameField.Field[searchEmptySymbolY, sideDiagonalX] == '_')
                        {
                            bestMoveY = searchEmptySymbolY;
                            bestMoveX = sideDiagonalX;
                        }
                        sideDiagonalX--;
                    }
                }
                int mainCountPieces = 0;
                int mainDiagonalX = 0;
                for (int mainDiagonalY = 0; mainDiagonalY <= tempY; mainDiagonalY++)
                {
                    if (gameField.Field[mainDiagonalY, mainDiagonalX] == this._playerSide)
                        mainCountPieces++;
                    mainDiagonalX++;
                }
                mainDiagonalX = 0;
                if (mainCountPieces > scoreEnemyPieces)
                {
                    scoreEnemyPieces = mainCountPieces;
                    for (int mainHorizontalY = tempY; mainHorizontalY >= 0; mainHorizontalY--)
                    {
                        if (gameField.Field[mainHorizontalY, mainDiagonalX] == '_') 
                        {
                            bestMoveY = mainHorizontalY;
                            bestMoveX = mainDiagonalX;
                            break;
                        }
                        mainDiagonalX++;
                    }
                }
                if (bestMoveY == 0 && bestMoveX == 0)
                    RandMove(ref gameField);
                else
                    gameField.Field[bestMoveY, bestMoveX] = _computerSide;
            }
        }

        private void CloseMainDiagonal(ref GameField gameField, int tempY, int tempX) 
        {
            int mainHorizontalX = 0;
            for (int mainHorizontalY = 0; mainHorizontalY <= tempY; mainHorizontalY++)
            {
                if (gameField.Field[mainHorizontalY, mainHorizontalX] == '_')
                {
                    gameField.Field[mainHorizontalY, mainHorizontalX] = _computerSide;
                    break;
                }
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
    }
}
