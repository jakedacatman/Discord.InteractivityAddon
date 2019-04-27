using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Appearance options of a <see cref="SelectionRequest{T}"/>.
    /// </summary>
    public sealed class SelectionAppearance
    {
        /// <summary>
        /// Gets or sets the <see cref="Embed"/> which the <see cref="Selection"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build();

        /// <summary>
        /// Gets or sets the <see cref="Embed"/> which the <see cref="Selection"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build();

        /// <summary>
        /// Gets or sets the string which can be selected to cancel the <see cref="Selection{T}"/>.
        /// </summary>
        public string CancelString { get; set; } = "Cancel";

        /// <summary>
        /// Gets or sets whether the <see cref="Selection"/> will delete invalid messages.
        /// </summary>
        public bool DeleteInvalidMessages { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the <see cref="Selection"/> will delete the valid message.
        /// </summary>
        public bool DeleteValidMessage { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the <see cref="Selection"/> will get deleted after a <see cref="InteractivityResult{T}"/> has been captured.
        /// </summary>
        public bool DeleteSelectionAfterCapturedResult { get; set; } = false;

        /// <summary>
        /// Creates a new instance of <see cref="SelectionAppearance"/>.
        /// </summary>
        /// <param name="cancelledEmbed">The <see cref="Embed"/> which the <paramref name="selectionEmbed"/> gets modified to when the selection is cancelled.</param>
        /// <param name="timeoutedEmbed">The <see cref="Embed"/> which the <paramref name="selectionEmbed"/> gets modified to when the selection is timeouted.</param>
        /// <param name="deleteInvalidMessages">Whether to delete all messages which do not interact with the selection.</param>
        /// <param name="deleteValidMessage">Whether to delete the message that completes the selection.</param>
        /// <param name="deleteSelectionAfterCapturedResult">Whether the <see cref="Selection"/> will get deleted after a <see cref="InteractivityResult{T}"/> has been captured.</param>
        public SelectionAppearance(Embed cancelledEmbed = null, Embed timeoutedEmbed = null,
            bool? deleteInvalidMessages = null, bool? deleteValidMessage = null, bool? deleteSelectionAfterCapturedResult = null)
        {
            CancelledEmbed = cancelledEmbed ?? new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build();
            TimeoutedEmbed = timeoutedEmbed ?? new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build();
            DeleteInvalidMessages = deleteInvalidMessages ?? false;
            DeleteValidMessage = deleteValidMessage ?? false;
            DeleteSelectionAfterCapturedResult = deleteSelectionAfterCapturedResult ?? false;
        }

        /// <summary>
        /// Creates a new instane of <see cref="SelectionAppearance"/> with the default values.
        /// </summary>
        public SelectionAppearance() { }

        /// <summary>
        /// Creates a new instane of <see cref="SelectionAppearance"/> with the default values.
        /// </summary>
        public static SelectionAppearance Default => new SelectionAppearance();

        /// <summary>
        /// Sets the cancelledembed of the <see cref="Selection"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public SelectionAppearance WithCancelledEmbed(Embed embed)
        {
            CancelledEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the timeoutedembed of the <see cref="Selection"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public SelectionAppearance WithTimeoutedEmbed(Embed embed)
        {
            TimeoutedEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the deletion settings of the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="deleteInvalidMessages"></param>
        /// <param name="deleteValidMessage"></param>
        /// <param name="deleteSelectionAfterCapturedResult"></param>
        /// <returns></returns>
        public SelectionAppearance WithSettings(bool deleteInvalidMessages = false, bool deleteValidMessage = false, bool deleteSelectionAfterCapturedResult = false)
        {
            DeleteInvalidMessages = deleteInvalidMessages;
            DeleteValidMessage = deleteValidMessage;
            DeleteSelectionAfterCapturedResult = deleteSelectionAfterCapturedResult;
            return this;
        }
    }
}
