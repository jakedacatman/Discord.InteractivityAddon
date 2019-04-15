using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Appearance options of a <see cref="SelectionRequest{T}"/>.
    /// </summary>
    public class SelectionAppearance
    {
        public Embed SelectionEmbed { get; }
        public Embed TimeoutedEmbed { get; }
        public Embed CancelledEmbed { get; }

        public bool DeleteInvalidMessages { get; }
        public bool DeleteValidMessage { get; }

        /// <summary>
        /// Creates a new instance of <see cref="SelectionAppearance"/>.
        /// </summary>
        /// <param name="selectionEmbed">The <see cref="Embed"/> which is sent in the selection.</param>
        /// <param name="timeoutedEmbed">The <see cref="Embed"/> which the <paramref name="selectionEmbed"/> gets modified to when the selection is timeouted.</param>
        /// <param name="cancelledEmbed">The <see cref="Embed"/> which the <paramref name="selectionEmbed"/> gets modified to when the selection is cancelled.</param>
        /// <param name="deleteInvalidMessages">Whether to delete all messages which do not interact with the selection.</param>
        /// <param name="deleteValidMessage">Whether to delete the message that completes the selection.</param>
        public SelectionAppearance(Embed selectionEmbed = null, Embed timeoutedEmbed = null, Embed cancelledEmbed = null,
            bool deleteInvalidMessages = false, bool deleteValidMessage = false)
        {
            SelectionEmbed = selectionEmbed ?? Default.SelectionEmbed;
            TimeoutedEmbed = timeoutedEmbed ?? Default.TimeoutedEmbed;
            CancelledEmbed = cancelledEmbed ?? Default.CancelledEmbed;

            DeleteInvalidMessages = deleteInvalidMessages;
            DeleteValidMessage = deleteValidMessage;
        }

        /// <summary>
        /// The standart appearance of a <see cref="SelectionRequest{T}"/>.
        /// </summary>
        public static SelectionAppearance Default => new SelectionAppearance(
            selectionEmbed: new EmbedBuilder().WithColor(Color.Blue).WithTitle("Select one of these :arrow_double_down:").Build(),
            timeoutedEmbed: new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build(),
            cancelledEmbed: new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build(),
            deleteInvalidMessages: false,
            deleteValidMessage: false
            );

        /// <summary>
        /// Creates a default <see cref="SelectionAppearance"/> with a custom <paramref name="selectionEmbed"/>.
        /// </summary>
        /// <param name="selectionEmbed">Your custom <paramref name="selectionEmbed"/>.</param>
        /// <returns></returns>
        public static SelectionAppearance FromSelectionEmbed(Embed selectionEmbed) => new SelectionAppearance(selectionEmbed: selectionEmbed);

        /// <summary>
        /// Creates a default <see cref="SelectionAppearance"/> with a custom <paramref name="timeoutedEmbed"/>.
        /// </summary>
        /// <param name="timeoutedEmbed">Your custom <paramref name="timeoutedEmbed"/>.</param>
        /// <returns></returns>
        public static SelectionAppearance FromTimeoutedEmbed(Embed timeoutedEmbed) => new SelectionAppearance(timeoutedEmbed: timeoutedEmbed);

        /// <summary>
        /// Creates a default <see cref="SelectionAppearance"/> with a custom <paramref name="cancelledEmbed"/>.
        /// </summary>
        /// <param name="cancelledEmbed">Your custom <paramref name="cancelledEmbed"/>.</param>
        /// <returns></returns>
        public static SelectionAppearance FromCancelledEmbed(Embed cancelledEmbed) => new SelectionAppearance(cancelledEmbed: cancelledEmbed);
    }
}
