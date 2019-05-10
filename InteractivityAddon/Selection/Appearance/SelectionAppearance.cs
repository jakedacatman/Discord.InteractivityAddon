using System;
using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents the appearance options of a <see cref="Selection{T}"/>. This class is immutable!
    /// </summary>
    public sealed class SelectionAppearance
    {
        private readonly Embed cancelledEmbed;
        private readonly Embed timeoutedEmbed;

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed => cancelledEmbed.DeepClone();

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed => timeoutedEmbed.DeepClone();

        /// <summary>
        /// Gets the string which can be selected to cancel the <see cref="Selection{T}"/>.
        /// </summary>
        public string CancelString { get; }

        /// <summary>
        /// Gets whether the <see cref="Selection{T}"/> will delete invalid messages.
        /// </summary>
        public bool DeleteInvalidMessages { get; }

        /// <summary>
        /// Gets whether the <see cref="Selection{T}"/> will delete the valid message.
        /// </summary>
        public bool DeleteValidMessage { get; }

        /// <summary>
        /// Gets whether the <see cref="Selection{T}"/> will get deleted after a <see cref="InteractivityResult{T}"/> has been captured.
        /// </summary>
        public bool DeleteSelectionAfterCapturedResult { get; }

        internal SelectionAppearance(Embed cancelled, Embed timeouted, string cancelString, bool deleteInvalidMessages, bool deleteValidMessage, bool deleteSelectionAfterCapturedResult)
        {
            cancelledEmbed = cancelled;
            timeoutedEmbed = timeouted;
            CancelString = cancelString;
            DeleteInvalidMessages = deleteInvalidMessages;
            DeleteValidMessage = deleteValidMessage;
            DeleteSelectionAfterCapturedResult = deleteSelectionAfterCapturedResult;
        }
    }
}
