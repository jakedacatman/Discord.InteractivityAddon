using System.Threading.Tasks;

namespace InteractivityAddon.Actions
{
    public interface IAction<T>
    {
        Task ApplyAsync(T value);
        bool ApplyToInvalid { get; }
        bool ApplyToValid { get; }
    }
}
