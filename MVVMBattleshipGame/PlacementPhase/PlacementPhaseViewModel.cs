using MVVMBattleshipGame.BattlePhase;
using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.Grid;
using MVVMBattleshipGame.Ship;

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

        #region COMMANDS/ACTIONS
        public RelayCommand<Direction> PlaceShipCommand { get; }
        public Action? CloseAction { get; set; }
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
        // PLAYER PLACING FLAG
        private bool _isPlayerPlacing = true;

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
                || CanPlaceShip(Direction.West))
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

            if (_isPlayerPlacing)
            {
                return PlayerField.CanPlaceShip(_lastClickedX, _lastClickedY, direction, CurrentShip);
            } 
            else
            {
                return ComputerField.CanPlaceShip(_lastClickedX, _lastClickedY, direction, CurrentShip);
            }
        }
        // PLACE THE SHIP
        private void PlaceShip(Direction direction)
        {
            if (_isPlayerPlacing && !IsPartlyPlaced) return; // SHIP MUST BE PARTLY PLACED FIRST FOR PLAYER

            if (_isPlayerPlacing && IsPartlyPlaced)
            {
                PlayerField.PlaceShip(_startX, _startY, direction, CurrentShip);
                NextShipPlacement();
            }
            else
            {
                ComputerField.PlaceShip(_startX, _startY, direction, CurrentShip);
            }
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
                // PLAYER FINISHES SHIP PLACEMENT SO UNSUBSCRIBE
                PlayerField.CellClicked -= OnCellClick;

                // COMPUTER PLACES SHIPS
                _isPlayerPlacing = false;
                ComputerPlaceShips();

                // START BATTLE PHASE
                var battlePhaseViewModel = new BattlePhaseViewModel(PlayerField, ComputerField);
                var battleWindow = new BattlePhaseWindow(battlePhaseViewModel);
                battleWindow.Show();
                CloseAction?.Invoke();
            }
        }
        // COMPUTER PICK RANDOMIZATION FUNCTIONALITY
        private void ComputerPlaceShips()
        {
            List<Direction> possibleDirections = new List<Direction>();
            Random random = new Random();

            // CREATE NEW SHIP
            CurrentShip = new ShipModel(ShipType.Carrier);

            // SHIP PLACEMENT LOOP
            do
            {
                // LOCATION CHOOSE LOOP
                do
                {
                    possibleDirections.Clear();
                    // COMPUTER CHOOSES A RANDOM LOCATION
                    int x = random.Next(10);
                    int y = random.Next(10);
                    _lastClickedX = x;
                    _lastClickedY = y;
                    // CHECK VALID DIRECTIONS
                    if (CanPlaceShip(Direction.North)) possibleDirections.Add(Direction.North);
                    if (CanPlaceShip(Direction.South)) possibleDirections.Add(Direction.South);
                    if (CanPlaceShip(Direction.East)) possibleDirections.Add(Direction.East);
                    if (CanPlaceShip(Direction.West)) possibleDirections.Add(Direction.West);
                }
                while (possibleDirections.Count == 0); // CHOOSE AGAIN IF NO POSSIBLE DIRECTIONS

                // CONFIRM START COORDS
                _startX = _lastClickedX;
                _startY = _lastClickedY;

                // RANDOMLY CHOOSE ONE OF THE POSSIBLE DIRECTIONS
                int dirIndex = random.Next(possibleDirections.Count);

                // PLACE SHIP
                PlaceShip(possibleDirections[dirIndex]);

                // GET NEXT SHIP
                CurrentShip = CurrentShip.ShipType switch
                {
                    ShipType.Carrier => new ShipModel(ShipType.Battleship),
                    ShipType.Battleship => new ShipModel(ShipType.Cruiser),
                    ShipType.Cruiser => new ShipModel(ShipType.Submarine),
                    ShipType.Submarine => new ShipModel(ShipType.Destroyer),
                    _ => null // NO MORE SHIPS TO PLACE
                };
            }
            while (CurrentShip != null); // LOOP WHILE THERE ARE SHIPS TO PLACE
        }
        #endregion
    }
}