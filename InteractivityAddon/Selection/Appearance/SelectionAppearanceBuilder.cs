using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a abstract builder class for making a <see cref="MessageSelectionAppearance"/>.
    /// </summary>
    /// <typeparam name="TAppearance">The appearance the builder will build into.</typeparam>
    public abstract class SelectionAppearanceBuilder<TAppearance> where TAppearance : SelectionAppearance
    {
        /// <summary>
        /// Gets or sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1, TSelectionAppearance}"/> has been cancelled.
        /// </summary>
        public EmbedBuilder CancelledEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:");

        /// <summary>
        /// Gets or sets the embed which the selection embed gets modified to after the <see cref="Selection{T, T1, TSelectionAppearance}"/> has timed out.
        /// </summary>
        public EmbedBuilder TimeoutedEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:");

        /// <summary>
        /// Gets or sets what the <see cref="Selection{T, T1, TSelectionAppearance}"/> should delete.
        /// </summary>
        public DeletionOption Deletion { get; set; }

        /// <summary>
        /// Builds the <see cref="SelectionAppearanceBuilder{TAppearance}"/> to a immutable <see cref="SelectionAppearance"/>.
        /// </summary>
        /// <returns></returns>
        public abstract TAppearance Build();
    }
}
