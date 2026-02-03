using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.PlacementPhase;
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
        // CELL PLACEMENT FUNCTIONALITY
        // CHECK IF CAN PLACE SHIP
        public bool CanPlaceShip(int startX, int startY, Direction direction, ShipModel ship)
        {
            if (ship == null) return false;

            // QUICK BOUNDS CHECK
            switch (direction)
            {
                case Direction.North: if (startY - (ship.Length - 1) < 0) return false; break;
                case Direction.South: if (startY + (ship.Length - 1) > 9) return false; break;
                case Direction.East: if (startX + (ship.Length - 1) > 9) return false; break;
                case Direction.West: if (startX - (ship.Length - 1) < 0) return false; break;
            }

            // ITERATIVELY CHECK EACH CELL THE OTHER PARTS OF THE SHIP WOULD OCCUPY
            int currentX = startX, currentY = startY;
            for (int i = 0; i < ship.Length; i++)
            {
                // CELL CHECKING MOVEMENT
                switch (direction)
                {
                    case Direction.North: currentY = startY - i; break;
                    case Direction.South: currentY = startY + i; break;
                    case Direction.East: currentX = startX + i; break;
                    case Direction.West: currentX = startX - i; break;
                }
                // CHECK TILE
                if (!IsValidTile(currentX, currentY)) return false;
            }
            return true;
        }
        // CHECK IF TILE IS VALID
        private bool IsValidTile(int x, int y)
        {
            for (int offsetX = -1; offsetX <= 1; offsetX++)
            {
                for (int offsetY = -1; offsetY <= 1; offsetY++)
                {
                    int _x = x + offsetX;
                    int _y = y + offsetY;

                    var cell = GetCell(_x, _y);

                    if (cell != null && cell.HasShip)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // ACTUAL SHIP PLACEMENT
        public void PlaceShip(int startX, int startY, Direction direction, ShipModel ship)
        {
            if (ship == null) return;

            // ASSUMING SHIP PLACEMENT DIRECTION IS VALID,
            // PLACE NOW THE SHIP IN THE CORRESPONDING CELLS
            int currentX = startX, currentY = startY;
            for (int i = 0; i < ship.Length; i++)
            {
                switch (direction)
                {
                    case Direction.North: currentY = startY - i; break;
                    case Direction.South: currentY = startY + i; break;
                    case Direction.East: currentX = startX + i; break;
                    case Direction.West: currentX = startX - i; break;
                }
                var cell = GetCell(currentX, currentY);
                if (cell != null) cell.Ship = ship;
            }
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
