using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Minesweeper.Models
{
    public class GameField
    {
        public Button TopButton{ get; set; }
        public Button BottomButton { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string Name { get; set; }
        public bool IsMine { get; set; }
        public int NumberOfMinesAround { get; set; }
        public bool IsFlagged { get; set; }
        
        public GameField(Button topButton, Button bottomButton, int row, int column)
        {
            TopButton = topButton;
            BottomButton = bottomButton;
            Row = row;
            Column = column;
            Name = "r" + Row.ToString() + "c" + Column.ToString();
            IsMine = false;
            IsFlagged = false;
        }
    }
}
