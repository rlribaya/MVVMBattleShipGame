using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.Grid;
using System.Windows;

namespace MVVMBattleshipGame.BattlePhase
{
    public class BattlePhaseViewModel : MyINotifyPropertyChanged
    {
        private int _playerShipCount = 5;
        private int _computerShipCount = 5;
        public BattleFieldGridViewModel PlayerField { get; private set; }
        public BattleFieldGridViewModel ComputerField { get; private set; }
        public Action? GameOverAction { get; set; }
        public BattlePhaseViewModel(BattleFieldGridViewModel playerField, BattleFieldGridViewModel computerField)
        {
            PlayerField = playerField;
            ComputerField = computerField;

            ComputerField.CellClicked += OnComputerFieldCellClicked;

            PlayerField.ShipSunk += PlayerField_ShipSunk;
            ComputerField.ShipSunk += ComputerField_ShipSunk;
        }
        private void PlayerField_ShipSunk(object? sender, ShipSunkEventArgs e)
        {
            _playerShipCount--;
            MessageBox.Show($"Computer has sunk your {e.Ship.ShipType}");
            // CHECK IF LOSE
            if (_playerShipCount == 0)
            {
                MessageBox.Show("YOU LOSE");
                GameOverAction?.Invoke();
            }
        }
        private void ComputerField_ShipSunk(object? sender, ShipSunkEventArgs e)
        {
            _computerShipCount--;
            MessageBox.Show($"You have sunk the opponent's {e.Ship.ShipType}");
            // CHECK IF WIN
            if (_computerShipCount == 0)
            {
                MessageBox.Show("YOU WIN!!");
                GameOverAction?.Invoke();
            }
        }
        private bool _isComputerMove = false;
        private async void OnComputerFieldCellClicked(object? sender, CellClickEventArgs e)
        {
            if (_isComputerMove) return;
            if (!(sender is BattleFieldGridViewModel fieldViewModel)) return;

            // HIT THE CELL
            fieldViewModel.HitCell(e.X, e.Y);

            // COMPUTER'S TURN
            _isComputerMove = true;
            await ComputerMove();
            _isComputerMove = false;
        }
        private async Task ComputerMove() // SIMPLE AI - RANDOM HITS
        {
            await Task.Delay(500); // DELAY FOR A BIT (PSEUDO THINKING)
            Random random = new Random();
            CellModel? target;
            do
            {
                int x = random.Next(10);
                int y = random.Next(10);
                target = PlayerField.GetCell(x, y);
            } while (target == null || !target.CanBeAttacked);

            PlayerField.HitCell(target);
        }
    }
}
