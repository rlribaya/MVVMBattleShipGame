using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.Ship;
using System.Collections.ObjectModel;

namespace MVVMBattleshipGame.Grid
{
    public class BattleFieldGridViewModel : MyINotifyPropertyChanged
    {
        #region PROPERTIES
        private ObservableCollection<CellModel> _cells;
        public ObservableCollection<CellModel> Cells
        {
            get { return _cells; }
            set
            {
                if (_cells != value)
                {
                    _cells = value;
                    OnPropertyChanged(nameof(Cells));
                }
            }
        }
        #endregion

        #region COMMANDS
        public RelayCommand<CellModel> CellClickedCommand { get; }
        #endregion

        #region CONSTRUCTOR
        public BattleFieldGridViewModel() : this(true) { }
        public BattleFieldGridViewModel(bool isPlayer)
        {
            // COMMANDS
            CellClickedCommand = new RelayCommand<CellModel>(OnCellClick);

            // SET UP THE FIELD
            _cells = new ObservableCollection<CellModel>();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Cells.Add(new CellModel(x, y, !isPlayer));
                }
            }
        }
        #endregion

        #region FUNCTIONALITY
        // CELL OBTAINING FUNCTIONALITY
        public CellModel? GetCell(int x, int y)
        {
            if (!IsInBounds(x, y))
                return null;

            return Cells[ToIndex(x, y)];
        }
        private bool IsInBounds(int x, int y)
        {
            return x >= 0 && x < 10 && y >= 0 && y < 10;
        }
        private int ToIndex(int x, int y)
        {
            return y * 10 + x;
        }
        // SHIP HIT FUNCTIONALITY
        public void HitCell(int x, int y) => HitCell(GetCell(x, y));
        public void HitCell(CellModel cell)
        {
            if (cell == null) return;

            // HIT THE CELL
            cell.IsHit = true;

            // CHECK IF SHIP
            if (cell.HasShip)
            {
                cell.Ship.Hits++;

                // IF SHIP IS SUNK
                if (cell.Ship.IsSunk)
                {
                    RaiseShipHit(new ShipSunkEventArgs(cell.Ship));
                }
            }
        }
        #endregion

        #region EVENTS
        public event EventHandler<CellClickEventArgs>? CellClicked;
        private void OnCellClick(CellModel cell)
        {
            if (cell == null) return;
            CellClicked?.Invoke(this, new CellClickEventArgs(cell.X, cell.Y));
        }
        public event EventHandler<ShipSunkEventArgs>? ShipSunk;
        private void RaiseShipHit(ShipSunkEventArgs e) => ShipSunk?.Invoke(this, e);
        #endregion
    }
    public class CellClickEventArgs : EventArgs
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CellClickEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
    public class ShipSunkEventArgs : EventArgs
    {
        public ShipModel Ship { get; set; }
        public ShipSunkEventArgs(ShipModel ship)
        {
            Ship = ship;
        }
    }
}
