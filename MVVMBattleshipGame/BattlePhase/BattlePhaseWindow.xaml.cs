using System.Windows;

namespace MVVMBattleshipGame.BattlePhase
{
    /// <summary>
    /// Interaction logic for BattlePhaseWindow.xaml
    /// </summary>
    public partial class BattlePhaseWindow : Window
    {
        public BattlePhaseWindow(BattlePhaseViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            vm.GameOverAction = new Action(() =>
            {
                new MainWindow().Show();
                Close();
            });
        }
    }
}
