using System.Windows;

namespace MVVMBattleshipGame.PlacementPhase
{
    /// <summary>
    /// Interaction logic for PlacementPhaseWindow.xaml
    /// </summary>
    public partial class PlacementPhaseWindow : Window
    {
        public PlacementPhaseWindow()
        {
            InitializeComponent();

            if (DataContext is PlacementPhaseViewModel vm)
            {
                vm.CloseAction = new Action(this.Close);
            }
        }
    }
}
