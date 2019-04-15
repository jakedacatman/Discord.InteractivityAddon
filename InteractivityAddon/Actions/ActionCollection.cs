using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace InteractivityAddon.Actions
{
    /// <summary>
    /// Represents a collection of <see cref="IAction{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ActionCollection<T>
    {
        private ImmutableList<IAction<T>> Actions { get; set; }
        public int ActionAmount => Actions.Count;

        public ActionCollection(List<IAction<T>> actions)
        {
            Actions = actions.ToImmutableList();
        }

        public ActionCollection(params IAction<T>[] action)
        {
            Actions = action.ToImmutableList();
        }

        public ActionCollection()
        {
            Actions = new List<IAction<T>>().ToImmutableList();
        }

        internal void AddAction(IAction<T> action) => Actions = Actions.Add(action);

        public async Task ApplyAsync(T value, bool isInvalid)
        {
            foreach (var action in Actions) {
                if (action.ApplyToInvalid == true && isInvalid == true) {
                    await action.ApplyAsync(value);
                }
                if (action.ApplyToValid == true && isInvalid == false) {
                    await action.ApplyAsync(value);
                }
            }
        }
    }
}
