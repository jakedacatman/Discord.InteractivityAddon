using System;
using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a builder class for making a <see cref="ReactionSelectionAppearance"/>.
    /// </summary>
    public sealed class ReactionSelectionAppearanceBuilder : SelectionAppearanceBuilder<ReactionSelectionAppearance>
    {
        /// <summary>
        /// Gets or sets the cancel emote if cancel is enabled in the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        public IEmote CancelEmote { get; set; } = new Emoji("❌");

        /// <summary>
        /// Creates a new <see cref="ReactionSelectionAppearanceBuilder"/> with default values.
        /// </summary>
        public ReactionSelectionAppearanceBuilder() { }

        /// <summary>
        /// Creates a new <see cref="ReactionSelectionAppearanceBuilder"/> with default values.
        /// </summary>
        public static ReactionSelectionAppearanceBuilder Default => new ReactionSelectionAppearanceBuilder();

        /// <summary>
        /// Builds the <see cref="ReactionSelectionAppearanceBuilder"/> to a <see cref="ReactionSelectionAppearance"/>.
        /// </summary>
        /// <returns></returns>
        public override ReactionSelectionAppearance Build() => new ReactionSelectionAppearance(
                CancelledEmbed.Build(),
                TimeoutedEmbed.Build(),
                Deletion,
                CancelEmote.DeepClone());

        /// <summary>
        /// Sets the cancel embed of the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public ReactionSelectionAppearanceBuilder WithCancelledEmbed(EmbedBuilder embed)
        {
            CancelledEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the timeouted embed of the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public ReactionSelectionAppearanceBuilder WithTimeoutedEmbed(EmbedBuilder embed)
        {
            TimeoutedEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the deletion option of the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        /// <param name="deletion"></param>
        /// <returns></returns>
        public ReactionSelectionAppearanceBuilder WithDeletion(DeletionOption deletion)
        {
            Deletion = deletion;
            return this;
        }

        /// <summary>
        /// Sets the cancel emote of the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        /// <param name="cancelEmote"></param>
        /// <returns></returns>
        public ReactionSelectionAppearanceBuilder WithCancelEmote(IEmote cancelEmote)
        {
            CancelEmote = cancelEmote;
            return this;
        }
    }
}
