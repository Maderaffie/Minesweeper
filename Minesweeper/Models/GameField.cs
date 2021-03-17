using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Minesweeper.Models
{
    public class GameField
    {
        public Button Button{ get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Name { get; set; }
        public bool IsMine { get; set; }
        public int NumberOfMinesAround { get; set; }
        
        public GameField(Button button, int row, int column)
        {
            Button = button;
            Row = row;
            Column = column;
            Name = "r" + Row.ToString() + "c" + Column.ToString();
            IsMine = false;
        }
    }
}
