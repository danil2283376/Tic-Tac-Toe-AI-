using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class GameField
    {
        public char winer = '\0';

        private int _x;
        private int _y;
        private char[,] _field;
        private int _countCellsForWin;
        public int X
        {
            get
            {
                return (this._x);
            }
            private set
            {
                this._x = value;
            }
        }
        public int Y
        {
            get
            {
                return (this._y);
            }
            private set
            {
                this._y = value;
            }
        }

        public char[,] Field
        {
            get
            {
                return (this._field);
            }
            private set
            {
                this._field = value;
            }
        }

        public GameField(int x, int y)
        {
            this.X = x;
            this.Y = y;
            this._countCellsForWin = x;
            _field = new char[y, x];
            StandartValueField();
        }

        public GameField(GameField copy) 
        {
            this.Y = copy.Y;
            this.X = copy.X;
            this._field = copy.Field;
            this._countCellsForWin = _x;
        }

        public void OutputField() 
        {
            for (int y = 0; y < this._y; y++)
            {
                for (int x = 0; x < this._x; x++)
                {
                    Console.Write(_field[y, x]);
                    Console.Write('|');
                }
                Console.WriteLine();
            }
        }

        public void SetCharInCell(int y, int x, char symbol) 
        {
            if (y < 0 || y > this.Y || x < 0 || x > this.X)
                throw new InvalidOperationException("Set char is out bounds array!!!");
            _field[y, x] = symbol;
        }

        public bool CellIsBusy(int y, int x) 
        {
            if (_field[y, x] != '_')
                return (true);
            return (false);
        }

        public bool CheckWins(int y, int x, char symbol)
        {
            if (CheckVerticalLine(y, x, symbol) == true)
            {
                this.winer = symbol;
                return (true);
            }
            if (CheckHorizontalLine(y, x, symbol) == true)
            {
                this.winer = symbol;
                return (true);
            }
            if (CheckMainDiagonal(y, x, symbol) == true)
            {
                this.winer = symbol;
                return (true);
            }
            if (CheckSideDiagonal(y, x, symbol) == true)
            {
                this.winer = symbol;
                return (true);
            }
            else
            {
                this.winer = '\0';
                return (false);
            }
        }

        public bool CheckDraw() 
        {
            int countCells = this._x * this._y;
            int cellsBusy = 0;
            for (int y = 0; y < this._y; y++) 
            {
                for (int x = 0; x < this._x; x++) 
                {
                    if (this._field[y, x] != '_')
                        cellsBusy++;
                }
            }
            if (cellsBusy == countCells)
                return (true);
            return (false);
        }

        private bool CheckVerticalLine(int y, int x, char symbol) 
        {
            int identicalSymbols = 0;
            int copyY = y;
            int copyX = x;

            while (true)
            {
                if (y >= this._y)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
                y++;
            }
            y = copyY;
            x = copyX;
            while (true)
            {
                y--;
                if (y < 0)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
            }
            if (identicalSymbols == this._countCellsForWin)
                return (true);
            return (false);
        }

        private bool CheckHorizontalLine(int y, int x, char symbol) 
        {
            int identicalSymbols = 0;
            int copyY = y;
            int copyX = x;

            while (true)
            {
                if (x >= this._x)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
                x++;
            }
            y = copyY;
            x = copyX;
            while (true) 
            {
                x--;
                if (x < 0)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
            }
            if (identicalSymbols == this._countCellsForWin)
                return (true);
            return (false);
        }

        private bool CheckMainDiagonal(int y, int x, char symbol) 
        {
            int identicalSymbols = 0;
            int copyY = y;
            int copyX = x;

            while (true) 
            {
                if (x < 0 || y < 0)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
                y--;
                x--;
            }
            y = copyY;
            x = copyX;
            while (true) 
            {
                y++;
                x++;
                if (x >= this._x || y >= this._y)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
            }
            if (identicalSymbols == this._countCellsForWin)
                return (true);
            return (false);
        }

        private bool CheckSideDiagonal(int y, int x, char symbol)
        {
            int identicalSymbols = 0;
            int copyY = y;
            int copyX = x;

            while (true)
            {
                if (x >= this._x || y < 0)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
                y--;
                x++;
            }
            y = copyY;
            x = copyX;
            while (true)
            {
                y++;
                x--;
                if (x < 0 || y >= this._y)
                    break;
                if (this._field[y, x] == symbol)
                    identicalSymbols++;
            }
            if (identicalSymbols == this._countCellsForWin)
                return (true);
            return (false);
        }

        private void StandartValueField()
        {
            for (int y = 0; y < this._y; y++)
            {
                for (int x = 0; x < this._x; x++)
                {
                    _field[y, x] = '_';
                }
            }
        }
    }
}
