using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TicTacToe.WPF
{
    public class CellViewModel : INotifyPropertyChanged
    {
        private char _symbol = ' ';
        private string _textColor = "Black";
        private bool _isWinningCell = false;

        public int Index { get; }

        public char Symbol
        {
            get => _symbol;
            set
            {
                _symbol = value;
                TextColor = value == 'X' ? "Red" : (value == 'O' ? "Green" : "Black");
                OnPropertyChanged(nameof(Symbol));
            }
        }

        public string TextColor
        {
            get => _textColor;
            set { _textColor = value; OnPropertyChanged(nameof(TextColor)); }
        }

        public bool IsWinningCell
        {
            get => _isWinningCell;
            set { _isWinningCell = value; OnPropertyChanged(nameof(IsWinningCell)); }
        }

        public ICommand ClickCommand { get; }

        public CellViewModel(int index, Action<int> onCellClicked)
        {
            Index = index;
            ClickCommand = new RelayCommand(_ => onCellClicked(Index));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
