namespace TicTacToe
{
    class Program
    {
        static async Task Main()
        {
            ConsoleRenderer renderer = new();
            ConsoleInputProvider inputProvider = new();
            GameManager gameManager = new(renderer, inputProvider);
            await gameManager.NewTicTacToeGame();
        }
    }

    class GameManager(IRenderer renderer, IInputProvider inputProvider)
    {
        async Task<bool> TicTacToeApp()
        {
            inputProvider.InputSizePrompt();
            Task<int> sizeInTask = inputProvider.ReadSizeInput();
            int sizeInput = await sizeInTask;
            Board myBoard = new(sizeInput);

            //Move xMove = new(myBoard, 'X', inputProvider);
            MoveMediumAI aiMove = AIFactory.CreateMediumAI(myBoard, 'O', inputProvider);
            MoveMediumAI xMove = AIFactory.CreateMediumAI(myBoard, 'X', inputProvider);

            GameLoopClass gameLoop = new(renderer, myBoard, xMove, aiMove);
            renderer.RenderBoard(sizeInput, myBoard.GetBoardState);
            await gameLoop.GameLoop();
            return inputProvider.ResetGameReader();
        }

        public async Task NewTicTacToeGame()
        {
            while (await TicTacToeApp());
        }
    }
}