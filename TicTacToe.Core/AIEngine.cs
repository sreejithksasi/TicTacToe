using System;
using System.Collections.Generic;

namespace TicTacToe.Core
{
    public static class AIEngine
    {
        public static int CalculateMediumMove(Board board, char xoro)
        {
            int multiplier = xoro == 'X' ? 1 : -1;
            IReadOnlyList<int> rowCheckList = board.GetRowSumTracker;
            IReadOnlyList<int> colCheckList = board.GetColumnSumTracker;
            int mainDiagonalCheck = board.GetMainDiagonalSumTracker;
            int offDiagonalCheck = board.GetOffDiagonalSumTracker;
            int target = multiplier * (board.GetBoardDimension - 1);
            int dimension = board.GetBoardDimension;

            int[] priorities = [target, -1 * target];

            foreach (int priority in priorities)
            {
                int m = rowCheckList.IndexOfList(priority);
                int n;
                if (m != -1)
                {
                    int move = ProcessBoard(board, dimension, i => m * dimension + i);
                    if (move != -1) return move;
                }
                else if ((n = colCheckList.IndexOfList(priority)) != -1)
                {
                    int move = ProcessBoard(board, dimension, i => i * dimension + n);
                    if (move != -1) return move;
                }
                else if (mainDiagonalCheck == priority)
                {
                    int move = ProcessBoard(board, dimension, i => i * dimension + i);
                    if (move != -1) return move;
                }
                else if (offDiagonalCheck == priority)
                {
                    int move = ProcessBoard(board, dimension, i => i * dimension + dimension - 1 - i);
                    if (move != -1) return move;
                }
            }

            // Fallback to Easy AI (Random Move) if no block/win is found
            Random random = new();
            var indexList = board.GetIndexState;
            if (indexList.Count == 0) return -1;
            return indexList[random.Next(0, indexList.Count)];
        }

        private static int ProcessBoard(Board board, int dimension, Func<int, int> indexFormula)
        {
            for (int i = 0; i < dimension; i++)
            {
                int entryIndex = indexFormula(i);
                if (board.GetBoardStateChar(entryIndex) == ' ')
                {
                    return entryIndex;
                }
            }
            return -1; // No valid move found
        }
    }

    public static class IndexOfExtension
    {
        public static int IndexOfList<T>(this IReadOnlyList<T> self, T elementToFind)
        {
            int i = 0;
            foreach (T element in self)
            {
                if (Equals(element, elementToFind))
                    return i;
                i++;
            }
            return -1;
        }
    }
}