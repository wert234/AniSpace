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
        private readonly Action<object> _Execute;
        private readonly Func<object, bool> _CanExcute;

        internal RelayCommand(Func<Task> AsyncExecute, Action<object> Execute, Func<object, bool> CanExcute)
        {
            _AsyncExecute = AsyncExecute;
            _Execute = Execute;
            _CanExcute = CanExcute;

        }
        public override bool CanExecute(object? parameter) => _CanExcute?.Invoke(parameter) ?? true;

        public override void Execute(object? parameter) => _Execute(parameter);

        public override Task ExecuteAsync() => _AsyncExecute();
    }
}
