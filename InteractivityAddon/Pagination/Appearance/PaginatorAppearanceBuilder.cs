using System;
using Discord;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Represents a builder class for creating a <see cref="PaginatorAppearance"/>.
    /// </summary>
    public sealed class PaginatorAppearanceBuilder
    {
        private IEmote backwardEmote = new Emoji("◀");
        private IEmote forwardEmote = new Emoji("▶");
        private IEmote skipToStartEmote = new Emoji("⏮");
        private IEmote skipToEndEmote = new Emoji("⏭");
        private IEmote exitEmote = new Emoji("🛑");
        private EmbedBuilder cancelledEmbed = new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:");
        private EmbedBuilder timeoutedEmbed = new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:");

        /// <summary>
        /// The <see cref="IEmote"/> to navigate a page backward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote BackwardEmote
        {
            get => backwardEmote;
            set => backwardEmote = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// The <see cref="IEmote"/> to navigate a page forward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ForwardEmote
        {
            get => forwardEmote;
            set => forwardEmote = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// The <see cref="IEmote"/> to navigate to the first page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToStartEmote
        {
            get => skipToStartEmote;
            set => skipToStartEmote = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// The <see cref="IEmote"/> to navigate to the last page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToEndEmote
        {
            get => skipToEndEmote;
            set => skipToEndEmote = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// The <see cref="IEmote"/> to exit the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ExitEmote
        {
            get => exitEmote;
            set => exitEmote = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets or sets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after cancellation.
        /// </summary>
        public EmbedBuilder CancelledEmbed
        {
            get => cancelledEmbed;
            set => cancelledEmbed = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets or sets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after a timeout.
        /// </summary>
        public EmbedBuilder TimeoutedEmbed
        {
            get => timeoutedEmbed;
            set => timeoutedEmbed = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets or sets what the <see cref="Paginator"/> should delete.
        /// </summary>
        public DeletionOption Deletion { get; set; } = DeletionOption.Valid | DeletionOption.Invalids;

        /// <summary>
        /// Builds the the <see cref="PaginatorAppearanceBuilder"/> to a immutable <see cref="PaginatorAppearance"/>.
        /// </summary>
        /// <returns></returns>
        public PaginatorAppearance Build() => new PaginatorAppearance(
            BackwardEmote.DeepClone(), //Required because Emote is ref type
            ForwardEmote.DeepClone(),  //User could change ref in Builder and it would affect the built instance
            SkipToStartEmote.DeepClone(),
            SkipToEndEmote.DeepClone(),
            ExitEmote.DeepClone(),
            CancelledEmbed.Build(),
            TimeoutedEmbed.Build(),
            Deletion);

        /// <summary>
        /// Creates a new instance of <see cref="PaginatorAppearance"/> with the default values.
        /// </summary>
        public PaginatorAppearanceBuilder() { }

        /// <summary>
        /// Creates a new instance of <see cref="PaginatorAppearance"/> with the default values.
        /// </summary>
        public static PaginatorAppearanceBuilder Default => new PaginatorAppearanceBuilder();

        /// <summary>
        /// Sets the backward emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The backward emote.</param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithBackwardEmote(IEmote emote)
        {
            BackwardEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the forward emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The forward emote.</param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithForwardEmote(IEmote emote)
        {
            ForwardEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the skiptostart emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The skiptostart emote.</param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithSkipToStartEmote(IEmote emote)
        {
            SkipToStartEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the skiptoend emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The skiptoend emote.</param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithSkipToEndEmote(IEmote emote)
        {
            SkipToEndEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the exit emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote"></param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithExitEmote(IEmote emote)
        {
            ExitEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the cancelledembed of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="embed">The cancel embed.</param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithCancelledEmbed(EmbedBuilder embed)
        {
            CancelledEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the timeoutedembed of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="embed">The timeout embed.</param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithTimeoutedEmbed(EmbedBuilder embed)
        {
            TimeoutedEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the deletion options of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="deletion"></param>
        /// <returns></returns>
        public PaginatorAppearanceBuilder WithDeletion(DeletionOption deletion)
        {
            Deletion = deletion;
            return this;
        }
    }
}
