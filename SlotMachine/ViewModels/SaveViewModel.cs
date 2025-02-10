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
    public class SaveViewModel : INotifyPropertyChanged
    {
        private const string SaveDirectory = "Models/Saves";
        private readonly Action _closeWindowAction;

        private string _saveFileName;

        public GameModel CurrentGame { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<string> SaveFiles { get; set; }

        // Tato vlastnost bude svázaná s TextBoxem:
        public string SaveFileName
        {
            get => _saveFileName;
            set
            {
                if (_saveFileName != value)
                {
                    _saveFileName = value;
                    OnPropertyChanged();
                }
            }
        }

        public SaveViewModel(GameModel gameModel, Action closeWindowAction)
        {
            CurrentGame = gameModel;
            _closeWindowAction = closeWindowAction;

            // Předvyplníme TextBox např. "Save_20250211_145300" (datum_čas):
            SaveFileName = "Save_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            CancelCommand = new RelayCommand(ExecuteCancelCommand);

            SaveFiles = new ObservableCollection<string>();
            LoadSaveFiles();
        }

        private void LoadSaveFiles()
        {
            SaveFiles.Clear();
            if (Directory.Exists(SaveDirectory))
            {
                foreach (var file in Directory.GetFiles(SaveDirectory, "*.json"))
                {
                    SaveFiles.Add(Path.GetFileName(file));
                }
            }
        }

        private void ExecuteSaveCommand(object parameter)
        {
            try
            {
                string saveFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "Saves");

                if (!Directory.Exists(saveFolderPath))
                    Directory.CreateDirectory(saveFolderPath);

                // Použijeme text z TextBoxu (SaveFileName):
                // Ošetřete si případné neplatné znaky nebo prázdný vstup.
                string fileName = SaveFileName + ".json";

                string filePath = Path.Combine(saveFolderPath, fileName);

                string jsonData = JsonSerializer.Serialize(CurrentGame, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(filePath, jsonData);
                _closeWindowAction(); // Zavře okno
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při ukládání: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExecuteCancelCommand(object parameter)
        {
            _closeWindowAction(); // Zavře okno
        }

        // Implementace INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
