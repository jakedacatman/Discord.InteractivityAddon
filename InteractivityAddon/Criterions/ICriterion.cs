using System.Threading.Tasks;

namespace InteractivityAddon.Criterions
{
    public interface ICriterion<T>
    {
        Task<bool> JudgeAsync(T value);
    }
}