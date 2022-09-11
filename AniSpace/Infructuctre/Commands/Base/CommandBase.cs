using MvvmHelpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AniSpace.Infructuctre.Commands.Base
{
    internal abstract class CommandBase : IAsyncCommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }

        public abstract bool CanExecute(object? parameter);

        public abstract void Execute(object? parameter);

        public abstract Task ExecuteAsync();
    }
}
