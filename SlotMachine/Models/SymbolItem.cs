using System;
using System.ComponentModel;

namespace SlotMachine.Models
{
    public class SymbolItem : INotifyPropertyChanged
    {
        private bool _isRemoving;
        private string _symbolName;

        public string SymbolName
        {
            get => _symbolName;
            set
            {
                if (_symbolName != value)
                {
                    _symbolName = value;
                    OnPropertyChanged(nameof(SymbolName));
                }
            }
        }

        public bool IsRemoving
        {
            get => _isRemoving;
            set
            {
                if (_isRemoving != value)
                {
                    _isRemoving = value;
                    OnPropertyChanged(nameof(IsRemoving));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public SymbolItem(string name)
        {
            _symbolName = name; 
        }
    }
}
