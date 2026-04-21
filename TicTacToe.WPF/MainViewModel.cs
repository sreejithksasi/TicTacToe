using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using TicTacToe.Core;

namespace TicTacToe.WPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Board _board;
        private bool _isSetupVisible = true;
        private bool _isGameVisible = false;
        private int _boardSize = 3;
        private string _playerXType = "Human";
        private string _playerOType = "Human";
        private string _gameStatusText = "Tic-Tac-Toe";
        private char _currentTurn = 'X';
        private int _turnCount = 0;
        private bool _isGameOver = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        public bool IsSetupVisible
        {
            get => _isSetupVisible;
            set { _isSetupVisible = value; OnPropertyChanged(nameof(IsSetupVisible)); }
        }

        public bool IsGameVisible
        {
            get => _isGameVisible;
            set { _isGameVisible = value; OnPropertyChanged(nameof(IsGameVisible)); }
        }

        public bool IsGameOver
        {
            get => _isGameOver;
            set { _isGameOver = value; OnPropertyChanged(nameof(IsGameOver)); }
        }

        public int BoardSize
        {
            get => _boardSize;
            set { _boardSize = Math.Max(3, Math.Min(10, value)); OnPropertyChanged(nameof(BoardSize)); }
        }

        public string PlayerXType
        {
            get => _playerXType;
            set { _playerXType = value; OnPropertyChanged(nameof(PlayerXType)); }
        }

        public string PlayerOType
        {
            get => _playerOType;
            set { _playerOType = value; OnPropertyChanged(nameof(PlayerOType)); }
        }

        public string GameStatusText
        {
            get => _gameStatusText;
            set { _gameStatusText = value; OnPropertyChanged(nameof(GameStatusText)); }
        }

        public ObservableCollection<CellViewModel> BoardCells { get; set; } = new();
        public ICommand StartGameCommand { get; }
        public ICommand ReturnToMenuCommand { get; }
        public ICommand ContinueCommand { get; }

        public MainViewModel()
        {
            StartGameCommand = new RelayCommand(_ => StartGame());
            ReturnToMenuCommand = new RelayCommand(_ => GoToMenu());
            ContinueCommand = new RelayCommand(_ => StartGame()); // Restarts with current settings
        }

        private async void StartGame()
        {
            _board = new Board(BoardSize);
            _currentTurn = 'X';
            _turnCount = 0;
            IsGameOver = false;
            GameStatusText = "Player X's Turn";

            BoardCells.Clear();
            for (int i = 0; i < BoardSize * BoardSize; i++)
            {
                BoardCells.Add(new CellViewModel(i, OnHumanCellClicked));
            }

            IsSetupVisible = false;
            IsGameVisible = true;

            if (PlayerXType != "Human")
            {
                await ProcessTurnAsync();
            }
        }

        private void GoToMenu()
        {
            IsGameVisible = false;
            IsSetupVisible = true;
        }

        private async void OnHumanCellClicked(int index)
        {
            if (IsGameOver) return;

            string currentPlayerType = _currentTurn == 'X' ? PlayerXType : PlayerOType;
            if (currentPlayerType != "Human" || _board.GetBoardStateChar(index) != ' ')
                return;

            await ProcessTurnAsync(index);
        }

        private async Task ProcessTurnAsync(int? humanIndex = null)
        {
            string currentPlayerType = _currentTurn == 'X' ? PlayerXType : PlayerOType;
            int moveIndex = -1;

            if (currentPlayerType == "Human")
            {
                if (humanIndex == null) return;
                moveIndex = humanIndex.Value;
            }
            else
            {
                GameStatusText = $"Player {_currentTurn} (AI) is thinking...";
                await Task.Delay(600);

                var availableMoves = _board.GetIndexState;
                if (availableMoves.Count == 0) return;

                if (currentPlayerType == "EasyAI")
                {
                    Random rng = new();
                    moveIndex = availableMoves[rng.Next(availableMoves.Count)];
                }
                else if (currentPlayerType == "MediumAI")
                {
                    moveIndex = AIEngine.CalculateMediumMove(_board, _currentTurn);
                }
            }

            // Apply move
            _board.UpdateBoard(moveIndex, _currentTurn);
            _turnCount++;
            BoardCells[moveIndex].Symbol = _currentTurn;

            // Check Game State
            WinStatus status = _board.EvaluateWinCon(_turnCount);
            if (status == WinStatus.Win)
            {
                GameStatusText = $"Player {_currentTurn} WINS!";
                HighlightWinningCells(_currentTurn); // Trigger the UI animation
                IsGameOver = true;
                return;
            }
            else if (status == WinStatus.Draw)
            {
                GameStatusText = "It's a DRAW!";
                IsGameOver = true;
                return;
            }

            // Switch Turns
            _currentTurn = _currentTurn == 'X' ? 'O' : 'X';
            string nextPlayerType = _currentTurn == 'X' ? PlayerXType : PlayerOType;
            GameStatusText = $"Player {_currentTurn}'s Turn";

            if (nextPlayerType != "Human")
            {
                await ProcessTurnAsync();
            }
        }

        private void HighlightWinningCells(char winningSymbol)
        {
            int n = BoardSize;

            // Check Rows
            for (int r = 0; r < n; r++)
            {
                if (Enumerable.Range(0, n).All(c => BoardCells[r * n + c].Symbol == winningSymbol))
                {
                    foreach (var c in Enumerable.Range(0, n)) BoardCells[r * n + c].IsWinningCell = true;
                    return;
                }
            }
            // Check Columns
            for (int c = 0; c < n; c++)
            {
                if (Enumerable.Range(0, n).All(r => BoardCells[r * n + c].Symbol == winningSymbol))
                {
                    foreach (var r in Enumerable.Range(0, n)) BoardCells[r * n + c].IsWinningCell = true;
                    return;
                }
            }
            // Check Main Diagonal
            if (Enumerable.Range(0, n).All(i => BoardCells[i * n + i].Symbol == winningSymbol))
            {
                foreach (var i in Enumerable.Range(0, n)) BoardCells[i * n + i].IsWinningCell = true;
                return;
            }
            // Check Off Diagonal
            if (Enumerable.Range(0, n).All(i => BoardCells[i * n + (n - 1 - i)].Symbol == winningSymbol))
            {
                foreach (var i in Enumerable.Range(0, n)) BoardCells[i * n + (n - 1 - i)].IsWinningCell = true;
                return;
            }
        }

        private int CalculateMediumAIMove()
        {
            Random rng = new();
            var available = _board.GetIndexState;
            return available[rng.Next(available.Count)];
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
