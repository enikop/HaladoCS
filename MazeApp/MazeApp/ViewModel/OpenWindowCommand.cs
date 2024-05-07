using MazeApp.Model;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace MazeApp.ViewModel
{
    public class OpenWindowCommand : ICommand
    {
        private readonly Type windowType;
        private readonly Settings? settings;

        public OpenWindowCommand(Type windowType)
        {
            this.windowType = windowType;
        }

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
            // if settings is null, empty parameter constructor should be called, otherwise the one with the Settings type parameter
            ConstructorInfo? ctor = windowType.GetConstructor(settings == null ? Type.EmptyTypes : new[] { typeof(Settings) });
            if (ctor != null)
            {
                Window window = (Window)ctor.Invoke(settings == null ? Type.EmptyTypes : new object[] { settings });
                window.ShowDialog();
            }
            else
            {
                throw new ArgumentException("Constructor not found.");
            }
        }
    }
}
