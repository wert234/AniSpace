using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AniSpace.Infructuctre.Commands.Base
{
    internal class RelayCommand : CommandBase
    {
        private readonly Func<Task> _AsyncExecute;
        private readonly Func<object, bool> _CanExcute;
        private Action<object> onChangeStudioCommand;
        private Func<object, bool> canChangeStudioCommand;

        public RelayCommand(Action<object> onChangeStudioCommand, Func<object, bool> canChangeStudioCommand)
        {
            this.onChangeStudioCommand = onChangeStudioCommand;
            this.canChangeStudioCommand = canChangeStudioCommand;
        }

        internal RelayCommand(Func<Task> AsyncExecute, Func<object, bool> CanExcute)
        {
            _AsyncExecute = AsyncExecute;
            _CanExcute = CanExcute;
        }
        public override bool CanExecute(object? parameter) => _CanExcute?.Invoke(parameter) ?? true;
        public override async void Execute(object? parameter) {
            if (_AsyncExecute != null)
            {
                await _AsyncExecute();
                return;
            }
            onChangeStudioCommand(parameter);
        } 
        public override Task ExecuteAsync() => _AsyncExecute();
    }
}
