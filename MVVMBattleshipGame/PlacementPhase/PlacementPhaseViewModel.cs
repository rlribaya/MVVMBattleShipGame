using MVVMBattleshipGame.Common;
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
        private bool _isPlacingShip;
		public bool IsPlacingShip
		{
			get { return _isPlacingShip; }
			set
			{
				if (_isPlacingShip != value)
				{
					_isPlacingShip = value;
					OnPropertyChanged(nameof(IsPlacingShip));
				}
			}
		}
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
					HandleShipPlacement();
				}
			}
		}

		#endregion

		#region COMMANDS
		public RelayCommand<Direction> PlaceShipCommand { get; }
        #endregion

        #region CONSTRUCTOR
        public PlacementPhaseViewModel()
        {
			// COMMANDS
			PlaceShipCommand = new RelayCommand<Direction>(PlaceShip, CanPlaceShip);

			// READY THE FIRST SHIP TO BE PLACED ON THE BOARD
			CurrentShip = new ShipModel(ShipType.Carrier);
        }
		#endregion

		#region FUNCTIONALITY
		// SHIP PLACEMENT FUNCTIONALITY
		private void HandleShipPlacement()
		{
			
		}

		// PLACE THE SHIP
		private void PlaceShip(Direction direction) {

		}

		// CHECK FOR VIABLE DIRECTION
		private bool CanPlaceShip(Direction direction)
		{

			return false;
		}
        #endregion
    }
}
