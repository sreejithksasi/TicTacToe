using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public enum WinStatus
    {
        Win,
        Draw,
        Game
    }
    public class Board
    {
        public Board(int dimension)
        {
            _N = dimension;
            _size = dimension * dimension;
            BoardState = [.. Enumerable.Repeat(' ', _size)];
            unfilledIndexList = [.. Enumerable.Range(0, _size)];

            rowSumTracker = [.. Enumerable.Repeat(0, _N)];
            columnSumTracker = [.. Enumerable.Repeat(0, _N)];
            diagonalSumTracker = 0;
            offDiagonalSumTracker = 0;
            winStatus = WinStatus.Game;
        }

        private readonly int _N;
        private readonly int _size;

        private readonly char[] BoardState;
        private readonly List<int> unfilledIndexList;
        public IReadOnlyList<int> GetIndexState => unfilledIndexList;
        public IReadOnlyList<char> GetBoardState => BoardState;

        //Evaluator state variables
        private readonly List<int> rowSumTracker;
        public IReadOnlyList<int> GetRowSumTracker => rowSumTracker;

        private readonly List<int> columnSumTracker;
        public IReadOnlyList<int> GetColumnSumTracker => columnSumTracker;

        private int diagonalSumTracker;
        public int GetMainDiagonalSumTracker => diagonalSumTracker;

        private int offDiagonalSumTracker;
        public int GetOffDiagonalSumTracker => offDiagonalSumTracker;
        public int GetBoardDimension => _N;
        public int GetBoardSize => _size;
        private WinStatus winStatus;

        public char GetBoardStateChar(int index)
        {
            return BoardState[index];
        }

        public bool ValidateUserInput(int userInput)
        {
            if (userInput < 0 || userInput > _size - 1 || BoardState[userInput] == 'X' || BoardState[userInput] == 'O')
            {
                return false;
            }
            return true;
        }

        public void UpdateBoard(int userInput, char playerSymbol)
        {
            BoardState[userInput] = playerSymbol;
            unfilledIndexList.Remove(userInput);
            winStatus = CheckSum(userInput, playerSymbol);
        }

        public WinStatus EvaluateWinCon(int turn)
        {
            if (turn >= 2 * _N - 2 && winStatus == WinStatus.Win)
            {
                return WinStatus.Win;
            }
            else if (GetIndexState.Count == 0)
            {
                return WinStatus.Draw;
            }
            return WinStatus.Game;
        }

        WinStatus CheckSum(int userInput, char playerSymbol)
        {
            int m = userInput / _N;
            int n = userInput % _N;
            int value = playerSymbol == 'X' ? 1 : -1;

            rowSumTracker[m] += value;
            columnSumTracker[n] += value;
            if (m == n) diagonalSumTracker += value;
            if (m + n == _N - 1) offDiagonalSumTracker += value;

            if (Math.Abs(rowSumTracker[m]) == _N || 
                Math.Abs(columnSumTracker[n]) == _N || 
                Math.Abs(diagonalSumTracker) == _N || 
                Math.Abs(offDiagonalSumTracker) == _N)
                return WinStatus.Win;
            return WinStatus.Game;
        }     
        
    }

}
