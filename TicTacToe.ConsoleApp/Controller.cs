using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    interface IMove
    {
        Task<bool> MakeMove();
    }

    public class Move(Board board, char xoro, IInputProvider inputProvider) : IMove
    {

        public async Task<bool> MakeMove()
        {
            inputProvider.MovePrompt(xoro, board.GetBoardSize);
            Task<int> move = inputProvider.ReadInput();
            int userInputInt = await move;

            if (board.ValidateUserInput(userInputInt))
            {
                board.UpdateBoard(userInputInt, xoro);
                return true;
            }
            return false;
        }
    }

    class MoveEasyAI(Board board, char xoro, IInputProvider inputProvider) : IMove
    {
        private static readonly Random random = new();
        public async Task<bool> MakeMove()
        {
            inputProvider.AIPrompt("Easy");
            IReadOnlyList<int> indexList = board.GetIndexState;
            await Task.Delay(500);
            if (indexList.Count == 0) return false;
            int randomInput = random.Next(0, indexList.Count);
            board.UpdateBoard(indexList[randomInput], xoro);
            return true;
        }
    }

    class GameLoopClass(IRenderer renderer, Board board, IMove xMove, IMove oMove)
    {
        public async Task GameLoop()
        {
            char xoro = 'X';
            int turn = 0;
            WinStatus winStatus = WinStatus.Game;
            while (winStatus == WinStatus.Game)
            {

                Task<bool> validMoveTask = xoro == 'X' ? xMove.MakeMove() : oMove.MakeMove();
                bool validMove = await validMoveTask;
                if (!validMove)
                {
                    renderer.ValidMovePrompt(xoro);
                    renderer.ErrorOutputForSize(board.GetBoardSize);
                    continue;
                }

                winStatus = board.EvaluateWinCon(turn);

                renderer.RenderBoard(board.GetBoardDimension, board.GetBoardState);

                if (winStatus == WinStatus.Draw)
                {
                    renderer.Draw();
                    continue;
                }
                else if (winStatus == WinStatus.Win)
                {
                    renderer.CharWin(xoro);
                    continue;
                }

                xoro = xoro == 'X' ? 'O' : 'X';
                turn++;
            }
        }
    }

    class MoveMediumAI(Board board, char xoro, IInputProvider inputProvider, MoveEasyAI easyAI) : IMove
    {
        public async Task<bool> MakeMove()
        {
            inputProvider.AIPrompt("Medium");
            int multiplier = xoro == 'X' ? 1 : -1;
            IReadOnlyList<int> rowCheckList = board.GetRowSumTracker;
            IReadOnlyList<int> colCheckList = board.GetColumnSumTracker;
            int mainDiagonalCheck = board.GetMainDiagonalSumTracker;
            int offDiagonalCheck = board.GetOffDiagonalSumTracker;
            int target = multiplier * (board.GetBoardDimension - 1);
            int dimension = board.GetBoardDimension;
            bool status = false;
            int[] priorities = [ target, -1 * target ];

            foreach (int priority in priorities) {
                int m = rowCheckList.IndexOf(priority);
                int n;
                if (m != -1)
                {
                    status = ProcessBoard(dimension, xoro, i => m * dimension + i);
                    if (status) break;
                }

                else if ((n = colCheckList.IndexOf(priority)) != -1)
                {
                    status = ProcessBoard(dimension, xoro, i => i * dimension + n);
                    if (status) break;
                }

                else if (mainDiagonalCheck == priority)
                {
                    status = ProcessBoard(dimension, xoro, i => i * dimension + i);
                    if (status) break;
                }

                else if (offDiagonalCheck == priority)
                {
                    status = ProcessBoard(dimension, xoro, i => i * dimension + dimension - 1 - i);
                    if (status) break;
                }
            }

            if (!status)
            {
                return await easyAI.MakeMove();
            }

            await Task.Delay(500);
            return status;
        }

        private bool ProcessBoard(int dimension, char xoro, Func<int, int> indexFormula)
        {
            for (int i = 0; i < dimension; i++)
            {
                int entryIndex = indexFormula(i);
                if (board.GetBoardStateChar(entryIndex) == ' ')
                {
                    board.UpdateBoard(entryIndex, xoro);
                    return true;
                }
            }
            return false;
        }
    }

    static class IndexOfExtension
    {
        public static int IndexOf<T>(this IReadOnlyList<T> self, T elementToFind)
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

    static class AIFactory
    {
        public static MoveMediumAI CreateMediumAI(Board board, char xoro, IInputProvider inputProvider)
        {
            MoveEasyAI easyAI = new(board, xoro, inputProvider);
            return new MoveMediumAI(board, xoro, inputProvider, easyAI);
        }
    }
}
