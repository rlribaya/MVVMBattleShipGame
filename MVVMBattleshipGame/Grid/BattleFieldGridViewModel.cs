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

        #region COMMANDS
        public RelayCommand<CellModel> CellClickedCommand { get; }
        #endregion

        #region CONSTRUCTOR
        public BattleFieldGridViewModel()
        {
            // COMMANDS
            CellClickedCommand = new RelayCommand<CellModel>(OnCellClick);

            // SET UP THE FIELD
            _cells = new ObservableCollection<CellModel>();
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 10; x++)
                {
                    Cells.Add(new CellModel(x, y));
                }
            }
        }
        #endregion

        #region FUNCTIONALITY
        // CELL CLICK
        

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
        #endregion

        #region EVENTS
        public event EventHandler<CellClickEventArgs>? CellClicked;
        private void OnCellClick(CellModel cell)
        {
            if (cell == null) return;
            CellClicked?.Invoke(this, new CellClickEventArgs(cell.X, cell.Y));
        }
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
}
