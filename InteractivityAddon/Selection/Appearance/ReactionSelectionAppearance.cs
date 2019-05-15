using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents the appearance of a <see cref="ReactionSelection{T}"/>. This class is immutable!
    /// </summary>
    public sealed class ReactionSelectionAppearance : SelectionAppearance
    {
        /// <summary>
        /// Gets the cancel emote if cancel is enabled in the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        public IEmote CancelEmote { get; }

        /// <summary>
        /// Creates a new <see cref="ReactionSelectionAppearance"/> with default values.
        /// </summary>
        public static ReactionSelectionAppearance Default => new ReactionSelectionAppearanceBuilder().Build() as ReactionSelectionAppearance;

        internal ReactionSelectionAppearance(Embed cancelledEmbed, Embed timeoutedEmbed, DeletionOption deletion,
            IEmote cancelEmote) 
            : base(cancelledEmbed, timeoutedEmbed, deletion)
        {
            CancelEmote = cancelEmote;
        }
    }
}
