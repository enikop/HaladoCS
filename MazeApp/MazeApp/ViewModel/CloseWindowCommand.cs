using MazeApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MazeApp.ViewModel
{
    public class CloseWindowCommand: ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true; // Can always execute
        }

        public void Execute(object? parameter)
        {
            if (parameter != null && parameter is Window) ((Window)parameter).Close();
        }
    }
}
