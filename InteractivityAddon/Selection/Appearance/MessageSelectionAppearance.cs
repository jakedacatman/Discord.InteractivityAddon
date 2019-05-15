using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents the appearance of a <see cref="MessageSelection{T}"/>. This class is immutable!
    /// </summary>
    public sealed class MessageSelectionAppearance : SelectionAppearance
    {
        /// <summary>
        /// Gets the cancel display name if cancel is enabled in the selection.
        /// </summary>
        public string CancelDisplayName { get; }

        /// <summary>
        /// Creates a new <see cref="MessageSelectionAppearance"/> with default values.
        /// </summary>
        public static MessageSelectionAppearance Default => MessageSelectionAppearanceBuilder.Default.Build() as MessageSelectionAppearance;

        internal MessageSelectionAppearance(Embed cancelledEmbed, Embed timeoutedEmbed, DeletionOption deletion,
            string cancelDisplayName) 
            : base(cancelledEmbed, timeoutedEmbed, deletion)
        {
            CancelDisplayName = cancelDisplayName;
        }
    }
}
