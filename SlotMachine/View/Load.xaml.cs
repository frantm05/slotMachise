using SlotMachine.Models;
using SlotMachine.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SlotMachine.View
{
    /// <summary>
    /// Interakční logika pro Save.xaml
    /// </summary>
    public partial class Load : Window
    {
        public Load(GameModel currentGame, Action<GameModel> onGameLoadedCallback)
        {
            InitializeComponent();
            DataContext = new LoadViewModel(
                currentGame,
                () => this.Close(),         // akce pro zavření okna
                onGameLoadedCallback        // akce volaná po načtení
            );
        }
    }
}
