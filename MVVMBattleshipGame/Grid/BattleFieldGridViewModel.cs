using MVVMBattleshipGame.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region CONSTRUCTOR
        public BattleFieldGridViewModel()
        {
            // SET UP THE FIELD
            _cells = new ObservableCollection<CellModel>();
            for (int i = 0; i < 10 * 10; i++) _cells.Add(new CellModel());
        }
        #endregion

        #region FUNCTIONALITY
        // CELL OBTAINING FUNCTIONALITY
        private CellModel? GetCell(int x, int y)
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
        #endregion

        #region EVENTS

        #endregion
    }
    public class CellHitEventArgs : EventArgs
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}
