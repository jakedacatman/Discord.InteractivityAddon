using Discord;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Appearance options of a <see cref="Paginator"/>.
    /// </summary>
    public sealed class PaginatorAppearance
    {
        public IEmote BackwardEmote { get; set; }
        public IEmote ForwardEmote { get; set; }

        public IEmote SkipToStartEmote { get; set; }
        public IEmote SkipToEndEmote { get; set; }

        public IEmote ExitEmote { get; set; }

        internal IEmote[] Emotes => new IEmote[] { BackwardEmote, ForwardEmote, SkipToStartEmote, SkipToEndEmote, ExitEmote };

        public Embed CancelledEmbed { get; set; }
        public Embed TimeoutedEmbed { get; set; }

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
        public PaginatorAppearance(IEmote backwardEmote, IEmote forwardEmote, IEmote skipToStartEmote, IEmote skipToEndEmote, IEmote exitEmote,
            Embed timeoutedEmbed, Embed cancelledEmbed)
        {
            BackwardEmote = backwardEmote;
            ForwardEmote = forwardEmote;
            SkipToStartEmote = skipToStartEmote;
            SkipToEndEmote = skipToEndEmote;
            ExitEmote = exitEmote;
            TimeoutedEmbed = timeoutedEmbed;
            CancelledEmbed = cancelledEmbed;
        }

        /// <summary>
        /// The default appearance of a <see cref="Paginator"/>
        /// </summary>
        public static PaginatorAppearance Default => new PaginatorAppearance(
            backwardEmote: new Emoji("◀"),
            forwardEmote: new Emoji("▶"),
            skipToStartEmote: new Emoji("⏮"),
            skipToEndEmote: new Emoji("⏭"),
            exitEmote: new Emoji("🛑"),
            timeoutedEmbed: new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build(),
            cancelledEmbed: new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build()
        );

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
        /// Sets the cancelledembed of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="embed">The cancel embed.</param>
        /// <returns></returns>
        public PaginatorAppearance WithCancelledEmbed(Embed embed)
        {
            CancelledEmbed = embed;
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
