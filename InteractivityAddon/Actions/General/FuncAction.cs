using System;
using System.Threading.Tasks;

namespace InteractivityAddon.Actions
{
    /// <summary>
    /// Applies an <see cref="Func{T, Task}"/> to a <see cref="T"/>
    /// </summary>
    public sealed class FuncAction<T> : IAction<T>
    {
        private Func<T, Task> Function { get; }

        public bool ApplyToInvalid { get; }
        public bool ApplyToValid { get; }

        public FuncAction(Func<T, Task> function, bool applyToInvalid, bool applyToValid)
        {
            Function = function;
            ApplyToInvalid = applyToInvalid;
            ApplyToValid = applyToValid;
        }

        public async Task ApplyAsync(T value) => await Function.Invoke(value);
    }
}
