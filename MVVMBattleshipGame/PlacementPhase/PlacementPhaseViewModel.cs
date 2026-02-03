using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.Grid;
using MVVMBattleshipGame.Ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMBattleshipGame.PlacementPhase
{
	public enum Direction
	{
		North,
		South,
		East,
		West
	}
    public class PlacementPhaseViewModel : MyINotifyPropertyChanged
	{
        #region PROPERTIES
		// FLAG FOR WHEN THE END OF THE SHIP IS PLACED
        private bool _isPartlyPlaced;
		public bool IsPartlyPlaced
		{
			get { return _isPartlyPlaced; }
			set
			{
				if (_isPartlyPlaced != value)
				{
					_isPartlyPlaced = value;
					OnPropertyChanged(nameof(IsPartlyPlaced));
				}
			}
		}
		// SHIP THAT IS CURRENTLY BEING PLACED
		private ShipModel _currentShip;
		public ShipModel CurrentShip
		{
			get { return _currentShip; }
			set
			{
				if (_currentShip != value)
				{
					_currentShip = value;
					OnPropertyChanged(nameof(CurrentShip));
				}
			}
		}
		public BattleFieldGridViewModel PlayerField { get; }
		public BattleFieldGridViewModel ComputerField { get; }
		#endregion

		#region COMMANDS
		public RelayCommand<Direction> PlaceShipCommand { get; }
        #endregion

        #region CONSTRUCTOR
        public PlacementPhaseViewModel()
        {
			// COMMANDS
			PlaceShipCommand = new RelayCommand<Direction>(PlaceShip, CanPlaceShip);

			// READY FIELDS
			PlayerField = new BattleFieldGridViewModel();
			ComputerField = new BattleFieldGridViewModel();

            // HANDLE CELL CLICK EVENT
            PlayerField.CellClicked += OnCellClick;

			// READY THE FIRST SHIP TO BE PLACED ON THE BOARD
			_currentShip = new ShipModel(ShipType.Carrier);
        }
        #endregion

        #region FUNCTIONALITY
        // COORDINATES OF THE LAST CLICKED CELL
        private int _lastClickedX = -1;
        private int _lastClickedY = -1;

		// COORDINATES OF VALID STARTING CELL
		private int _startX = -1;
		private int _startY = -1;

        // CELL CLICK
        private void OnCellClick(object? sender, CellClickEventArgs e)
        {
			IsPartlyPlaced = false;
			// GET CELL COORDS
            _lastClickedX = e.X;
			_lastClickedY = e.Y;

			// CHECK IF POSSIBLE TO PLACE IN AT LEAST ONE DIRECTION FIRST
			if (CanPlaceShip(Direction.North)
				|| CanPlaceShip(Direction.South)
				|| CanPlaceShip(Direction.East)
				||  CanPlaceShip(Direction.West))
			{
				// PROCEED TO PARTLY PLACE THE SHIP (AWAIT FOR DIRECTION INPUT)
				IsPartlyPlaced = true;
				_startX = _lastClickedX;
				_startY = _lastClickedY;
				PlaceShipCommand.RaiseCanExecuteChanged(); // UPDATE BUTTONS
            }
        }
        // CHECK FOR VIABLE DIRECTION
        private bool CanPlaceShip(Direction direction)
        {
			if (CurrentShip == null || _lastClickedX == -1 || _lastClickedY == -1) return false;
            
			// HOLD VALUES
			int startX = _lastClickedX;
            int startY = _lastClickedY;

            // QUICK BOUNDS CHECK
            switch (direction)
			{
				case Direction.North: if (startY - (CurrentShip.Length - 1) < 0) return false; break;
				case Direction.South: if (startY + (CurrentShip.Length - 1) > 9) return false; break;
				case Direction.East: if (startX + (CurrentShip.Length - 1) > 9) return false; break;
				case Direction.West: if (startX - (CurrentShip.Length - 1) < 0) return false; break;
			}

			// ITERATIVELY CHECK EACH CELL THE OTHER PARTS OF THE SHIP WOULD OCCUPY
			int currentX = startX, currentY = startY;
            for (int i = 0; i < CurrentShip.Length; i++)
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

                    // 1. GetCell handles the out-of-bounds check (returns null)
                    var cell = PlayerField.GetCell(_x, _y);

                    // 2. If the cell exists and has a ship, the area is NOT clear
                    if (cell != null && cell.HasShip)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        // PLACE THE SHIP
        private void PlaceShip(Direction direction) {
			if (!IsPartlyPlaced) return; // SHIP MUST BE PARTLY PLACED FIRST

			// ASSUMING SHIP IS VALID IN DIRECTION (BUTTON AUTOMATICALLY DISABLED)
			// PLACE NOW THE SHIP IN THE CELL
			int currentX = _startX, currentY = _startY;
			for (int i = 0; i < CurrentShip.Length; i++)
			{
                switch (direction)
                {
                    case Direction.North: currentY = _startY - i; break;
                    case Direction.South: currentY = _startY + i; break;
                    case Direction.East: currentX = _startX + i; break;
                    case Direction.West: currentX = _startX - i; break;
                }
				// CELL SHOULDN'T BE NULL HERE AS WE CHECKED IT ALREADY
				PlayerField.GetCell(currentX, currentY).Ship = CurrentShip;
            }
			NextShipPlacement();
		}
        // NEXT SHIP PLACEMENT FUNCTIONALITY
        private void NextShipPlacement()
        {
			// RESET DEPENDENT VALUES
			IsPartlyPlaced = false;
			_lastClickedX = -1;
			_lastClickedY = -1;
			_startX = -1;
			_startY = -1;

            CurrentShip = CurrentShip.ShipType switch
            {
                ShipType.Carrier => new ShipModel(ShipType.Battleship),
                ShipType.Battleship => new ShipModel(ShipType.Cruiser),
                ShipType.Cruiser => new ShipModel(ShipType.Submarine),
                ShipType.Submarine => new ShipModel(ShipType.Destroyer),
                _ => null // No more ships to place
            };

            if (CurrentShip == null)
            {
                // Transition to Battle Phase logic here
            }
        }
        #endregion
    }
}

/*
 public enum ShipType
    {
        Carrier, // 5
        Battleship, // 4
        Cruiser, // 3
        Submarine, // 3
        Destroyer // 2
    }
 */
