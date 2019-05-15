using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a <see langword="abstract"/> class for creating custom selection builders.
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    /// <typeparam name="T1">The way of selecting in discord. Either <see cref="SocketMessage"/> or <see cref="SocketReaction"/>.</typeparam>
    /// <typeparam name="TAppearanceBuilder">The custom appearance builder of the selection.</typeparam>
    /// <typeparam name="TAppearance">The custom appearance of the selection.</typeparam>
    public abstract class SelectionBuilder<T, T1, TAppearanceBuilder, TAppearance>
        where T1 : class where TAppearanceBuilder : SelectionAppearanceBuilder<TAppearance>, new() where TAppearance : SelectionAppearance
    {
        /// <summary>
        /// Gets or sets the values to select from.
        /// </summary>
        public List<T> Values { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the appearance of the <see cref="Selection{T, T1, TSelectionAppearance}"/>.
        /// </summary>
        public TAppearanceBuilder Appearance { get; set; } = new TAppearanceBuilder();

        /// <summary>
        /// Gets or sets the users who can interact with the <see cref="Selection{T, T1, TSelectionAppearance}"/>.
        /// </summary>
        public List<SocketUser> Users { get; set; } = new List<SocketUser>();

        /// <summary>
        /// Gets whether the selection is user restricted.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;

        /// <summary>
        /// Gets or sets the selection embed of the <see cref="Selection{T, T1, TSelectionAppearance}"/>.
        /// </summary>
        public EmbedBuilder SelectionEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Blue);

        protected SelectionBuilder()
        {
            if (typeof(T1) != typeof(SocketReaction) && typeof(T1) != typeof(SocketMessage)) {
                throw new InvalidOperationException($"{nameof(T1)} can ONLY be SocketMessage or SocketReaction!");
            }
        }

        /// <summary>
        /// Build the <see cref="SelectionBuilder{T, T1, TAppearanceBuilder, TAppearance}"/> to a immutable <see cref="Selection{T, T1, TSelectionAppearance}"/>.
        /// </summary>
        /// <returns></returns>
        public abstract Task<Selection<T, T1, TAppearance>> Build();
    }
}
