using System;
using System.Windows;
using System.Windows.Input;

namespace MazeApp.ViewModel
{
    public class CloseWindowCommand : ICommand
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
