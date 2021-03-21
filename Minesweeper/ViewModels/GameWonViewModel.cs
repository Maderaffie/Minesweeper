using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class GameWonViewModel
    {
        public SetDialogResultTrueCommand SetDialogResultTrueCommand { get; set; }
        public GameWonViewModel()
        {
            SetDialogResultTrueCommand = new SetDialogResultTrueCommand();
        }
    }
}
