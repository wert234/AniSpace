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

        internal RelayCommand(Func<Task> AsyncExecute, Func<object, bool> CanExcute)
        {
            _AsyncExecute = AsyncExecute;
            _CanExcute = CanExcute;

        }
        public override bool CanExecute(object? parameter) => _CanExcute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => _AsyncExecute();

        public override Task ExecuteAsync() => _AsyncExecute();
    }
}
