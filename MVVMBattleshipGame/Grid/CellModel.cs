using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.Ship;

namespace MVVMBattleshipGame.Grid
{
    public class CellModel : MyINotifyPropertyChanged
    {
        public CellModel(int x, int y, bool isFogged = false)
        {
            // Initialize Default Values
            _isHit = false;
            _ship = null;
            _isFogged = isFogged;
            X = x;
            Y = y;
        }
        public int X { get; }
        public int Y { get; }
        // IsHit
        private bool _isHit;
        public bool IsHit
        {
            get { return _isHit; }
            set
            {
                if (_isHit != value && CanBeAttacked)
                {
                    _isHit = value;
                    OnPropertyChanged(nameof(IsHit));
                    OnPropertyChanged(nameof(IsFogged));
                    OnPropertyChanged(nameof(CanBeAttacked));
                }
            }
        }
        public bool CanBeAttacked => !IsHit;
        // SHIP
        private ShipModel? _ship;
        public ShipModel? Ship
        {
            get { return _ship; }
            set
            {
                if (_ship != value)
                {
                    _ship = value;
                    OnPropertyChanged(nameof(Ship));
                    OnPropertyChanged(nameof(HasShip));
                }
            }
        }
        // HasShip
        public bool HasShip => Ship != null;
        // IsFogged
        private bool _isFogged;
        public bool IsFogged
        {
            get { return IsHit ? false : _isFogged; }
            set
            {
                if (_isFogged != value)
                {
                    _isFogged = value;
                    OnPropertyChanged(nameof(IsFogged));
                }
            }
        }
    }
}
