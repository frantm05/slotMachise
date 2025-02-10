using SlotMachine.Models;
using SlotMachine.Commands;
using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SlotMachine.ViewModels
{
    public class LoadViewModel : INotifyPropertyChanged
    {
        private const string SaveDirectory = "Models/Saves";
        private readonly Action _closeWindowAction;
        private readonly Action<GameModel> _onGameLoadedCallback;
       

        // Tato property slouží pro "TwoWay" binding v UI (ListBox / ListView / ComboBox),
        // kde se uživatel rozhodne, který soubor načíst.
        private string _selectedSaveFile;
        public string SelectedSaveFile
        {
            get => _selectedSaveFile;
            set
            {
                _selectedSaveFile = value;
                OnPropertyChanged();
            }
        }

        public GameModel CurrentGame { get; set; }
        public ICommand LoadCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<string> SaveFiles { get; set; }

        public LoadViewModel(GameModel currentGame,
                         Action closeWindowAction,
                         Action<GameModel> onGameLoadedCallback)
        {
            CurrentGame = currentGame;
            _closeWindowAction = closeWindowAction;
            _onGameLoadedCallback = onGameLoadedCallback;

            LoadCommand = new RelayCommand(ExecuteLoadCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);

            SaveFiles = new ObservableCollection<string>();
            LoadSaveFiles();
        }

        // Načte všechny savy (soubory *.json) z adresáře
        private void LoadSaveFiles()
        {
            SaveFiles.Clear();
            if (Directory.Exists(SaveDirectory))
            {
                foreach (var file in Directory.GetFiles(SaveDirectory, "*.json"))
                {
                    // Uložíme pouze název souboru bez cesty (např. "save_20230210121530.json").
                    SaveFiles.Add(Path.GetFileName(file));
                }
            }
        }

        private void ExecuteLoadCommand(object parameter)
        {
            if (!string.IsNullOrEmpty(SelectedSaveFile))
            {
                try
                {
                    var filePath = Path.Combine(SaveDirectory, SelectedSaveFile);
                    var jsonString = File.ReadAllText(filePath);
                    var loadedGame = JsonSerializer.Deserialize<GameModel>(jsonString);

                    // Tady zavoláme callback, který provede UpdateFromGameModel v MainViewModel
                    _onGameLoadedCallback?.Invoke(loadedGame);

   
                    _closeWindowAction?.Invoke(); // Zavření okna
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při načítání hry: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Prosím, vyberte soubor ze seznamu.");
            }
        }


        private void ExecuteCancelCommand(object parameter)
        {
            // Zavře okno
            _closeWindowAction();
        }

        // ==== INotifyPropertyChanged ====
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


    }
}
