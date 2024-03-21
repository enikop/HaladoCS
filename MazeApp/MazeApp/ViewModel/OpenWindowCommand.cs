using System;
using System.Windows.Input;
using System.Windows;
using MazeApp.Model;
using System.Reflection;

namespace MazeApp.ViewModel
{
    public class OpenWindowCommand : ICommand
    {
        private readonly Type windowType;
        private readonly Settings settings;


        public OpenWindowCommand(Type windowType, Settings settings)
        {
            this.windowType = windowType;
            this.settings = settings;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true; // Can always execute
        }

        public void Execute(object? parameter)
        {
            ConstructorInfo? ctor = windowType.GetConstructor(new[] { typeof(Settings) });
            if (ctor != null)
            {
                Window window = (Window)ctor.Invoke(new object[] { settings });
                window.ShowDialog();
            }
            else
            {
                throw new ArgumentException("Constructor with Settings parameter not found.");
            }
        }
    }
}
