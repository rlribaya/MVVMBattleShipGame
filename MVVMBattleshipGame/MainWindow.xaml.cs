using System.Windows;

namespace MVVMBattleshipGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new PlacementPhase.PlacementPhaseWindow().Show();
            Close();
        }
    }
}