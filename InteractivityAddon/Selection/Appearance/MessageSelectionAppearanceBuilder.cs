using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a builder class for making a <see cref="MessageSelectionAppearance"/>.
    /// </summary>
    public sealed class MessageSelectionAppearanceBuilder : SelectionAppearanceBuilder<MessageSelectionAppearance>
    {
        /// <summary>
        /// Gets or sets the cancel display name if cancel is enabled in the <see cref="MessageSelection{T}"/>.
        /// </summary>
        public string CancelDisplayName { get; set; } = "Cancel";

        /// <summary>
        /// Creates a new <see cref="MessageSelectionAppearanceBuilder"/> with default values.
        /// </summary>
        public MessageSelectionAppearanceBuilder() { }

        /// <summary>
        /// Creates a new <see cref="MessageSelectionAppearanceBuilder"/> with default values.
        /// </summary>
        public static MessageSelectionAppearanceBuilder Default => new MessageSelectionAppearanceBuilder();

        /// <summary>
        /// Builds the <see cref="MessageSelectionAppearanceBuilder"/> to a immutable <see cref="MessageSelectionAppearance"/>.
        /// </summary>
        /// <returns></returns>
        public override MessageSelectionAppearance Build() => new MessageSelectionAppearance(
            CancelledEmbed.Build(),
            TimeoutedEmbed.Build(),
            Deletion,
            CancelDisplayName);

        /// <summary>
        /// Sets the cancel embed of the <see cref="MessageSelection{T}"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public MessageSelectionAppearanceBuilder WithCancelledEmbed(EmbedBuilder embed)
        {
            CancelledEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the timeouted embed of the <see cref="MessageSelection{T}"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public MessageSelectionAppearanceBuilder WithTimeoutedEmbed(EmbedBuilder embed)
        {
            TimeoutedEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the deletion options of the <see cref="MessageSelection{T}"/>.
        /// </summary>
        /// <param name="deletion"></param>
        /// <returns></returns>
        public MessageSelectionAppearanceBuilder WithDeletion(DeletionOption deletion)
        {
            Deletion = deletion;
            return this;
        }

        /// <summary>
        /// Sets the cancel display name of the <see cref="MessageSelection{T}"/>.
        /// </summary>
        /// <param name="cancelDisplayName"></param>
        /// <returns></returns>
        public MessageSelectionAppearanceBuilder WithCancelDisplayName(string cancelDisplayName)
        {
            CancelDisplayName = cancelDisplayName;
            return this;
        }
    }
}
