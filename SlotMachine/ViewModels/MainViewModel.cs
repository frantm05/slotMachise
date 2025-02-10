using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SlotMachine.Models;
using SlotMachine.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using SlotMachine.Services;
using Microsoft.Win32;
using System.Text.Json;
using System.IO;
using SlotMachine.View;

namespace SlotMachine.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ICommand OpenSaveWindowCommand { get; }
        public ICommand OpenLoadWindowCommand { get; }

        private GameModel _gameModel { get; set; }
        public RelayCommand SaveGameCommand { get; set; }
        public RelayCommand LoadGameCommand { get; set; }
        private Random random = new Random();
        private string _saveDirectory;
        private readonly IDispatcherService _dispatcherService;
        private bool _isSpinning;
        public event EventHandler SpinStarted;
        public event EventHandler<decimal> SpinEnded;

        public bool IsSpinning
        {
            get => _isSpinning;
            set
            {
                _isSpinning = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }



        // Každý válec = 1 SymbolItem (1 obrázek v UI)
        public SymbolItem Reel1Symbols { get; set; }
        public SymbolItem Reel2Symbols { get; set; }
        public SymbolItem Reel3Symbols { get; set; }

        public MainViewModel() : this(new DispatcherService())
        {
            // Optional: Any initialization needed if called from XAML
        }

        public MainViewModel(IDispatcherService dispatcherService = null)
        {
            OpenSaveWindowCommand = new RelayCommand(OpenSaveWindow);
            OpenLoadWindowCommand = new RelayCommand(OpenLoadWindow);
            _dispatcherService = dispatcherService ?? new DispatcherService(); 
            _gameModel = new GameModel();
            SaveGameCommand = new RelayCommand(SaveGame);
            LoadGameCommand = new RelayCommand(LoadGame);

            Reel1Symbols = new SymbolItem("A");
            Reel2Symbols = new SymbolItem("A");
            Reel3Symbols = new SymbolItem("A");

            SpinResult = new ObservableCollection<string>();

            OnPropertyChanged(nameof(Balance));
            OnPropertyChanged(nameof(CurrentBet));

            BetCommand = new RelayCommand(BetExecute, CanBetExecute);
            NewGameCommand = new RelayCommand(NewGameExecute, CanNewGameExecute);

            _saveDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "SlotMachineSaves");
            if(!Directory.Exists(_saveDirectory))
            {
                Directory.CreateDirectory(_saveDirectory);
            }
        }

        private void SaveGame(object parameter)
        {
            // Use a custom save dialog (see below) or keep the SaveFileDialog
            string fileName = $"save_{DateTime.Now.ToString("yyyyMMddHHmmss")}.json";
            string filePath = Path.Combine(_saveDirectory, fileName);

            try
            {
                string jsonString = JsonSerializer.Serialize(_gameModel, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);
                MessageBox.Show("Hra uložena.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Chyba při ukládání hry: {ex.Message}");
            }
        }

        private void LoadGame(object parameter)
        {
            // Use a custom load dialog (see below) or keep the OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = _saveDirectory; // Set initial directory
            openFileDialog.Filter = "JSON soubory (*.json)|*.json|Všechny soubory (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string jsonString = File.ReadAllText(openFileDialog.FileName);
                    _gameModel = JsonSerializer.Deserialize<GameModel>(jsonString);
                    OnPropertyChanged(nameof(_gameModel)); // Notify UI about changes in GameModel
                    MessageBox.Show("Hra načtena.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Chyba při načítání hry: {ex.Message}");
                }
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        // == PROPERTIES ==
        public decimal Balance
        {
            get => _gameModel.Balance;
            set
            {
                if (_gameModel.Balance != value)
                {
                    _gameModel.Balance = value;
                    OnPropertyChanged();
                }
            }
        }

        public decimal CurrentBet
        {
            get => _gameModel.CurrentBet;
            set
            {
                if (_gameModel.CurrentBet != value)
                {
                    _gameModel.CurrentBet = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<string> SpinResult { get; set; }
        public Dictionary<string, decimal> PayoutTable => _gameModel.PayoutTable;

        // == Commands ==
        public ICommand BetCommand { get; set; }
        public ICommand NewGameCommand { get; set; }

        public List<decimal> BetOptions { get; } = new List<decimal> { 10, 20, 50, 100, 1000 };

        public event EventHandler SpinResultUpdated;

        private async void BetExecute(object parameter)
        {
            IsSpinning = true;
            SpinStarted?.Invoke(this, EventArgs.Empty);
            decimal betAmount = CurrentBet;
            if (Balance >= betAmount)
            {
                // Odečteme
                Balance -= betAmount;

                // Možná chceme vyčistit spin result
                SpinResult.Clear();

                // Upozorníme (pokud to UI využívá)
                SpinResultUpdated?.Invoke(this, EventArgs.Empty);

                // Teď spustíme "spin" na pozadí
                await StartSpinAsync();
            }
            else
            {
                System.Console.WriteLine("Nedostatek financí");
            }
            IsSpinning = false;
        }

        private bool CanBetExecute(object parameter) => !IsSpinning && Balance >= CurrentBet;

        private void NewGameExecute(object parameter)
        {
            _gameModel = new GameModel();
            OnPropertyChanged(nameof(Balance));
            OnPropertyChanged(nameof(CurrentBet));
            SpinResult.Clear();
        }
        private bool CanNewGameExecute(object parameter) => true;

        // == "Multi-core" točení ==

        /// <summary>
        /// Spustí 3 úkoly (Task) - každý točí (bliká) svým vlastním tempem. 
        /// Pak po uplynutí definované doby je válec zastaven.
        /// </summary>
        private async Task StartSpinAsync()
        {
            // Nastavíme zpočátku nějaké symboly
            Reel1Symbols.SymbolName = "A";
            Reel2Symbols.SymbolName = "A";
            Reel3Symbols.SymbolName = "A";

            // Tady definujeme, kdy se válec zastaví (v sekundách)
            // Válec1 => 2s, Válec2 => 2,5s, Válec3 => 3s
            double stop1 = 1.0, stop2 = 2.0, stop3 = 3.0;

            // Spustíme 3 asynchronní úkoly na pozadí (paralelně)
            var task1 = SpinReel(Reel1Symbols, stop1);
            var task2 = SpinReel(Reel2Symbols, stop2);
            var task3 = SpinReel(Reel3Symbols, stop3);

            // Počkám, až všechny 3 skončí
            await Task.WhenAll(task1, task2, task3);

            // A nakonec vyhodnotím výhru
            CheckWin();
        }

        /// <summary>
        /// Jednoduchá async metoda, která 
        ///  - co 20 ms vygeneruje náhodný symbol 
        ///  - uplyne-li definovaný čas stopTime, vygeneruje finální symbol a skončí
        /// </summary>
        private async Task SpinReel(SymbolItem reel, double stopTimeSeconds)
        {
            var start = DateTime.Now;

            while (true)
            {
                double elapsed = (DateTime.Now - start).TotalSeconds;
                if (elapsed >= stopTimeSeconds)
                {
                    // Zastavíme => finální symbol
                    await _dispatcherService.InvokeAsync(()=>
                        reel.SymbolName = GetWeightedSymbol()
                        );
                    break;
                }
                else
                {
                    // Jinak generujeme 
                    // (musíme nastavit SymbolName na UI threadu)
                    await _dispatcherService.InvokeAsync(() =>
                        reel.SymbolName = GetWeightedSymbol()
                        );

                    // Počkáme 20 ms (blikání)
                    await Task.Delay(10);
                }
            }
        }

        /// <summary>
        /// Po skončení spinu vyhodnotíme výhru
        /// </summary>
        private void CheckWin()
        {
            decimal winnings = 0;
            string finalCombo = Reel1Symbols.SymbolName
                              + Reel2Symbols.SymbolName
                              + Reel3Symbols.SymbolName;

            if (_gameModel.PayoutTable.ContainsKey(finalCombo))
            {
                winnings = _gameModel.PayoutTable[finalCombo] * CurrentBet;
                Balance += winnings;
            }
            SpinEnded?.Invoke(this, winnings);
        }

        private string GetWeightedSymbol()
        {
            double roll = random.NextDouble() * 100;
            double cumulative = 0;
            foreach (var kvp in _gameModel.SymbolWeights)
            {
                cumulative += kvp.Value;
                if(roll < cumulative) return kvp.Key;
            }
            return "A";
        }

        private void OpenSaveWindow(object parametr)
        {
            Save saveWindow = new Save(_gameModel);
            saveWindow.ShowDialog();
        }
        private void OpenLoadWindow(object parameter)
        {
            // Vytvořím instanci okna, předám mu
            //  1) currentGame (pokud je potřeba),
            //  2) callback, který se má zavolat po načtení
            var loadWindow = new Load(_gameModel, (loadedGame) =>
            {
                // Až LoadViewModel dokončí načtení, 
                // vrátí loadedGame sem:
                UpdateFromGameModel(loadedGame);
            });

            loadWindow.ShowDialog();
        }

        public void UpdateFromGameModel(GameModel loadedGame)
        {
            // Přes property, aby se volal OnPropertyChanged
            Balance = loadedGame.Balance;
            CurrentBet = loadedGame.CurrentBet;
            // A podle potřeby další data
        }

    }
}
