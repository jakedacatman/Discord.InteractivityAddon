namespace InteractivityAddon
{
    /// <summary>
    /// The result of interactivityrequests.
    /// </summary>
    /// <typeparam name="T">The type of the value in this <see cref="InteractivityResult{T}"/>.</typeparam>
    public sealed class InteractivityResult<T>
    {
        public T Value { get; }
        public bool IsTimeouted { get; }
        public bool IsCancelled { get; }
        public bool IsSuccess => !IsCancelled && !IsTimeouted;

        public InteractivityResult(T value, bool isTimeouted, bool isCancelled)
        {
            Value = value;
            IsTimeouted = isTimeouted;
            IsCancelled = isCancelled;
        }
    }
}