using Discord;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Represents the appearance options of a <see cref="Paginator"/>. This class is immutable!
    /// </summary>
    public sealed class PaginatorAppearance
    {
        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate a page backward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote BackwardEmote { get; }

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate a page forward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ForwardEmote { get; }

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate to the first page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToStartEmote { get; }

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate to the last page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToEndEmote { get; }

        /// <summary>
        /// Gets the <see cref="IEmote"/> to exit the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ExitEmote { get; }

        internal IEmote[] Emotes => new IEmote[] { BackwardEmote, ForwardEmote, SkipToStartEmote, SkipToEndEmote, ExitEmote };

        /// <summary>
        /// Gets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed { get; }

        /// <summary>
        /// Gets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed { get; }

        /// <summary>
        /// Gets what the <see cref="Paginator"/> should delete.
        /// </summary>
        public DeletionOption Deletion { get; }

        internal PaginatorAppearance(IEmote backward, IEmote forward, IEmote skipToStart, IEmote skipToEnd, IEmote exit,
            Embed cancelled, Embed timeouted,
            DeletionOption deletion)
        {
            BackwardEmote = backward;
            ForwardEmote = forward;
            SkipToStartEmote = skipToStart;
            SkipToEndEmote = skipToEnd;
            ExitEmote = exit;
            CancelledEmbed = cancelled;
            TimeoutedEmbed = timeouted;
            Deletion = deletion;
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