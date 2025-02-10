using SlotMachine.ViewModels;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;

namespace SlotMachine
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = (MainViewModel)DataContext;
            viewModel.SpinStarted += OnSpinStarted;
            viewModel.SpinEnded += OnSpinEnded;
        }

        private void OnSpinStarted(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Asterisk.Play();
        }

        private void OnSpinEnded(object sender, decimal winning)
        {
            if (winning > 0)
            {
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
    }
}