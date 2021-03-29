using Minesweeper.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Minesweeper.ViewModels
{
    public class PauseViewModel
    {
        public SetDialogResultTrueCommand SetDialogResultTrueCommand { get; set; }
        public PauseViewModel()
        {
            SetDialogResultTrueCommand = new SetDialogResultTrueCommand();
        }
    }
}
