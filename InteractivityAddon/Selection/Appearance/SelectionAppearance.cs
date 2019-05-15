using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a abstract class to create your own SelectionAppearances. The child classes should be immutable!
    /// </summary>
    public abstract class SelectionAppearance
    {
        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed { get; }

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed { get; }

        /// <summary>
        /// Gets an ORing determiting what the <see cref="Selection{T1, T2}"/> will delete.
        /// </summary>
        public DeletionOption Deletion { get; }

        protected SelectionAppearance(Embed cancelledEmbed, Embed timeoutedEmbed, DeletionOption deletion)
        {
            CancelledEmbed = cancelledEmbed;
            TimeoutedEmbed = timeoutedEmbed;
            Deletion = deletion;
        }
    }
}
