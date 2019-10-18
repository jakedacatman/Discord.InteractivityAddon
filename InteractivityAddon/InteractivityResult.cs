using System;

namespace Interactivity
{
    /// <summary>
    /// The result of interactivityrequests.
    /// </summary>
    /// <typeparam name="T">The type of the value in this <see cref="InteractivityResult{T}"/>.</typeparam>
    public sealed class InteractivityResult<T>
    {
        public T Value { get; }
        public TimeSpan Elapsed { get; }
        public bool IsTimeouted { get; }
        public bool IsCancelled { get; }
        public bool IsSuccess => !IsCancelled && !IsTimeouted;

        public InteractivityResult(T value, TimeSpan elapsed, bool isTimeouted, bool isCancelled)
        {
            Value = value;
            Elapsed = elapsed;
            IsTimeouted = isTimeouted;
            IsCancelled = isCancelled;
        }
    }
}