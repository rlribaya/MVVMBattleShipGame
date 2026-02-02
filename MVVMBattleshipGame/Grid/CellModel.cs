using MVVMBattleshipGame.Common;
using MVVMBattleshipGame.Ship;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMBattleshipGame.Grid
{
    public class CellModel : MyINotifyPropertyChanged
    {
        public CellModel()
        {
            // Initialize Default Values
            _hasShip = false;
            _isMiss = false;
            _ship = null;
            _isFogged = true;
        }
        // HasShip
        private bool _hasShip;
        public bool HasShip
        {
            get { return _hasShip; }
            set
            {
                if (_hasShip != value)
                {
                    _hasShip = value;
                    OnPropertyChanged(nameof(HasShip));
                }
            }
        }
        // IsHit
        private bool _isMiss;
        public bool IsHit
        {
            get { return _isMiss; }
            set
            {
                if (_isMiss != value)
                {
                    _isMiss = value;
                    OnPropertyChanged(nameof(IsHit));
                }
            }
        }
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
                }
            }
        }
        // IsFogged
        private bool _isFogged;
        public bool IsFogged
        {
            get { return _isFogged; }
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
