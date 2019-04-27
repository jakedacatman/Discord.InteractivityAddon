using Discord;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Appearance options of a <see cref="Paginator"/>.
    /// </summary>
    public sealed class PaginatorAppearance
    {
        /// <summary>
        /// The <see cref="IEmote"/> to navigate a page backward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote BackwardEmote { get; set; } = new Emoji("◀");

        /// <summary>
        /// The <see cref="IEmote"/> to navigate a page forward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ForwardEmote { get; set; } = new Emoji("▶");

        /// <summary>
        /// The <see cref="IEmote"/> to navigate to the first page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToStartEmote { get; set; } = new Emoji("⏮");

        /// <summary>
        /// The <see cref="IEmote"/> to navigate to the last page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToEndEmote { get; set; } = new Emoji("⏭");

        /// <summary>
        /// The <see cref="IEmote"/> to exit the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ExitEmote { get; set; } = new Emoji("🛑");

        internal IEmote[] Emotes => new IEmote[] { BackwardEmote, ForwardEmote, SkipToStartEmote, SkipToEndEmote, ExitEmote };

        /// <summary>
        /// Gets or sets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build();

        /// <summary>
        /// Gets or sets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build();

        /// <summary>
        /// Gets or sets whether the <see cref="Paginator"/> will delete reactions which are not associated with the <see cref="Paginator"/>.
        /// </summary>
        public bool DeleteOtherReactions { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the <see cref="Paginator"/> will get deleted after it exited.
        /// </summary>
        public bool DeletePaginatorAfterExit { get; set; } = false;

        /// <summary>
        /// Creates a new instance of <see cref="PaginatorAppearance"/>.
        /// </summary>
        /// <param name="backwardEmote">The <see cref="IEmote"/> to move one page backward.</param>
        /// <param name="forwardEmote">The <see cref="IEmote"/> to move one page forward.</param>
        /// <param name="skipToStartEmote">The <see cref="IEmote"/> to skip the the beginning.</param>
        /// <param name="skipToEndEmote">The <see cref="IEmote"/> to skip to the end.</param>
        /// <param name="exitEmote">The <see cref="IEmote"/> to exit the Paginator.</param>
        /// <param name="timeoutedEmbed">The <see cref="Embed"/> to be shown after the timeout occured.</param>
        /// <param name="cancelledEmbed">The <see cref="Embed"/> to be shown when the paginator got cancelled.</param>
        public PaginatorAppearance(IEmote backwardEmote = null, IEmote forwardEmote = null, IEmote skipToStartEmote = null, IEmote skipToEndEmote = null, IEmote exitEmote = null,
            Embed cancelledEmbed = null, Embed timeoutedEmbed = null,
            bool? deleteOtherReactions = null, bool? deletePaginatorAfterExit = null)
        {
            BackwardEmote = backwardEmote ?? new Emoji("◀");
            ForwardEmote = forwardEmote ?? new Emoji("▶");
            SkipToStartEmote = skipToStartEmote ?? new Emoji("⏮");
            SkipToEndEmote = skipToEndEmote ?? new Emoji("⏭");
            ExitEmote = exitEmote ?? new Emoji("🛑");
            CancelledEmbed = cancelledEmbed ?? new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build();
            TimeoutedEmbed = timeoutedEmbed ?? new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build();
            DeleteOtherReactions = deleteOtherReactions ?? true;
            DeletePaginatorAfterExit = deletePaginatorAfterExit ?? false;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PaginatorAppearance"/> with the default values.
        /// </summary>
        public PaginatorAppearance() { }

        /// <summary>
        /// Creates a new instance of <see cref="PaginatorAppearance"/> with the default values.
        /// </summary>
        public static PaginatorAppearance Default => new PaginatorAppearance();

        /// <summary>
        /// Sets the backward emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The backward emote.</param>
        /// <returns></returns>
        public PaginatorAppearance WithBackwardEmote(IEmote emote)
        {
            BackwardEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the forward emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The forward emote.</param>
        /// <returns></returns>
        public PaginatorAppearance WithForwardEmote(IEmote emote)
        {
            ForwardEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the skiptostart emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The skiptostart emote.</param>
        /// <returns></returns>
        public PaginatorAppearance WithSkipToStartEmote(IEmote emote)
        {
            SkipToStartEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the skiptoend emote of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="emote">The skiptoend emote.</param>
        /// <returns></returns>
        public PaginatorAppearance WithSkipToEndEmote(IEmote emote)
        {
            SkipToEndEmote = emote;
            return this;
        }

        /// <summary>
        /// Sets the cancelledembed of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="embed">The cancel embed.</param>
        /// <returns></returns>
        public PaginatorAppearance WithCancelledEmbed(Embed embed)
        {
            CancelledEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the timeoutedembed of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="embed">The timeout embed.</param>
        /// <returns></returns>
        public PaginatorAppearance WithTimeoutedEmbed(Embed embed)
        {
            TimeoutedEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the deletion settings of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="deleteOtherReactions"></param>
        /// <param name="deletePaginatorAfterExit"></param>
        /// <returns></returns>
        public PaginatorAppearance WithSettings(bool deleteOtherReactions = true, bool deletePaginatorAfterExit = false)
        {
            DeleteOtherReactions = deleteOtherReactions;
            DeletePaginatorAfterExit = deletePaginatorAfterExit;
            return this;
        }

        internal PaginatorAction ParseAction(IEmote emote) => emote.Equals(ForwardEmote)
                                                              ? PaginatorAction.Forward
                                                              : emote.Equals(BackwardEmote)
                                                                  ? PaginatorAction.Backward
                                                                  : emote.Equals(SkipToStartEmote)
                                                                      ? PaginatorAction.SkipToStart
                                                                      : emote.Equals(SkipToEndEmote)
                                                                          ? PaginatorAction.SkipToEnd
                                                                          : emote.Equals(ExitEmote)
                                                                              ? PaginatorAction.Exit
                                                                              : PaginatorAction.None;
    }
}