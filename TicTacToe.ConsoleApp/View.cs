using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicTacToe
{
    public interface IRenderer
    {
        void RenderBoard(int size, IReadOnlyList<char> BoardState);
        void CharWin(char input);
        void Draw();
        void ValidMovePrompt(char input);
        void ErrorOutputForSize(int size);
    }

    public interface IInputProvider
    {
        Task<int> ReadInput();
        void MovePrompt(char xoro, int size);
        void AIPrompt(String aiType);
        void InputSizePrompt();
        void InvalidSizePrompt();
        Task<int> ReadSizeInput();
        bool ResetGameReader();
        void GenericInvalidInput();
    }

    class ConsoleInputProvider() : IInputProvider
    {
        public Task<int> ReadInput()
        {
            bool validInput = false;
            int userInputInt = default;
            while (!validInput)
            {
                String? userInputString = Console.ReadLine();
                if (userInputString == null || !int.TryParse(userInputString, out userInputInt))
                {
                    GenericInvalidInput();
                    continue;
                }
                validInput = true;
            }
            return Task.FromResult(userInputInt);
        }

        public async Task<int> ReadSizeInput()
        {
            bool validInput = false;
            int sizeInput = 0;
            while (!validInput)
            {
                sizeInput = await ReadInput();
                if (sizeInput < 3 || sizeInput > 100)
                {
                    InvalidSizePrompt();
                    continue;
                }
                validInput = true;
            }
            return sizeInput;
        }

        public void MovePrompt(char xoro, int size)
        {
            Console.WriteLine($"Player '{xoro}', enter your move (0-{size - 1}): ");
        }

        public void AIPrompt(String aiType)
        {
            Console.WriteLine($"{aiType} AI Turn!!!");
        }

        public void InputSizePrompt()
        {
            Console.WriteLine("Input size: ");
        }
        public void InvalidSizePrompt()
        {
            Console.WriteLine("INVALID SIZE!!!\nPlease Enter a valid size value between 3-100!!!");
        }
        public void GenericInvalidInput()
        {
            Console.WriteLine("Please enter a valid value for input!!!");
        }

        public bool ResetGameReader()
        {
            Console.WriteLine("Do you want to continue? (Y/N)");
            String? newGame = Console.ReadLine();
            if (newGame == null) return false;
            else if (newGame.Equals("y", StringComparison.OrdinalIgnoreCase)) return true;
            else return false;
        }
    }

    class ConsoleRenderer() : IRenderer
    {
        public void RenderBoard(int size, IReadOnlyList<char> BoardState)
        {
            StringBuilder sb = new();
            string separator = PrintHorizontalSeparator(size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    char c = BoardState[i * size + j];
                    sb.Append(' ').Append(c).Append(' ');
                    if (j < size - 1) sb.Append('|');
                    else sb.Append('\n');
                }
                if (i < size - 1)
                {
                    sb.Append(separator);
                }
            }
            Console.Write(sb.ToString());
        }

        static string PrintHorizontalSeparator(int size)
        {
            StringBuilder sb = new();
            for (int j = 0; j < size; j++)
            {
                sb.Append("---");
                if (j < size - 1) sb.Append('|');
                else sb.Append('\n');
            }
            return sb.ToString();
        }

        public void CharWin(char input)
        {
            Console.WriteLine($"{input} WINS!!!");
        }

        public void Draw()
        {
            Console.WriteLine("It's a DRAW!!!");
        }
        public void ValidMovePrompt(char input)
        {
            Console.WriteLine($"Please enter valid move for {input}!!!");
        }

        public void ErrorOutputForSize(int size)
        {
            Console.WriteLine($"Invalid input. Please enter a number between 0 and {size - 1}, in valid spaces.");
        }
    }
}
