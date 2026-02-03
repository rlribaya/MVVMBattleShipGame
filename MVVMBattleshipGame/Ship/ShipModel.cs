using MVVMBattleshipGame.Common;

namespace MVVMBattleshipGame.Ship
{
    public enum ShipType
    {
        Carrier, // 5
        Battleship, // 4
        Cruiser, // 3
        Submarine, // 3
        Destroyer // 2
    }
    public class ShipModel : MyINotifyPropertyChanged
    {
        public ShipModel(ShipType shipType)
        {
            ShipType = shipType;
        }
        private ShipType _shipType;
        public ShipType ShipType
        {
            get { return _shipType; }
            set
            {
                if (_shipType != value)
                {
                    _shipType = value;
                    OnPropertyChanged(nameof(ShipType));
                    OnPropertyChanged(nameof(Length));
                }
            }
        }
        public int Length => ShipType switch
        {
            ShipType.Carrier => 5,
            ShipType.Battleship => 4,
            ShipType.Cruiser => 3,
            ShipType.Submarine => 3,
            ShipType.Destroyer => 2,
            _ => 0
        };
        private int _hits;
        public int Hits
        {
            get => _hits;
            set
            {
                if (_hits != value)
                {
                    _hits = value;
                    OnPropertyChanged(nameof(Hits));
                    OnPropertyChanged(nameof(IsSunk));
                }
            }
        }
        // A ship is sunk if hits equal its length
        public bool IsSunk => Hits >= Length;
    }
}
