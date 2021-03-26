using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class GameEndViewModel
    {
        public string Title { get; set; }
        public string Time { get; set; }
        public SetDialogResultTrueCommand SetDialogResultTrueCommand { get; set; }
        public GameEndViewModel()
        {
            SetDialogResultTrueCommand = new SetDialogResultTrueCommand();
        }
    }
}
