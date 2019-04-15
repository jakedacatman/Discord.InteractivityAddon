using System;
using System.Threading.Tasks;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that the <see cref="T"/> passes a <see cref="PredicateCriterion{T}"/>"/>
    /// </summary>
    public sealed class PredicateCriterion<T> : ICriterion<T>
    {
        private Predicate<T> Filter { get; }

        public PredicateCriterion(Predicate<T> filter)
        {
            Filter = filter;
        }

        public Task<bool> JudgeAsync(T value)
        => Task.FromResult(Filter.Invoke(value));
    }
}