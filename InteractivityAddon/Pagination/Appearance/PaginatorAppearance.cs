using System;
using Discord;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Represents the appearance options of a <see cref="Paginator"/>. This class is immutable!
    /// </summary>
    public sealed class PaginatorAppearance
    {
        private readonly IEmote backwardEmote;
        private readonly IEmote forwardEmote;
        private readonly IEmote skipToStartEmote;
        private readonly IEmote skipToEndEmote;
        private readonly IEmote exitEmote;
        private readonly Embed cancelledEmbed;
        private readonly Embed timeoutedEmbed;

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate a page backward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote BackwardEmote => backwardEmote.DeepClone();

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate a page forward in the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ForwardEmote => forwardEmote.DeepClone();

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate to the first page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToStartEmote => skipToStartEmote.DeepClone();

        /// <summary>
        /// Gets the <see cref="IEmote"/> to navigate to the last page of the <see cref="Paginator"/>.
        /// </summary>
        public IEmote SkipToEndEmote => skipToEndEmote.DeepClone();

        /// <summary>
        /// Gets the <see cref="IEmote"/> to exit the <see cref="Paginator"/>.
        /// </summary>
        public IEmote ExitEmote => exitEmote.DeepClone();

        internal IEmote[] Emotes => new IEmote[] { BackwardEmote, ForwardEmote, SkipToStartEmote, SkipToEndEmote, ExitEmote };

        /// <summary>
        /// Gets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed => cancelledEmbed.DeepClone();

        /// <summary>
        /// Gets the <see cref="Embed"/> which the <see cref="Paginator"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed => timeoutedEmbed.DeepClone();

        /// <summary>
        /// Gets what the <see cref="Paginator"/> should delete.
        /// </summary>
        public DeletionOption Deletion { get; }

        internal PaginatorAppearance(IEmote backward, IEmote forward, IEmote skipToStart, IEmote skipToEnd, IEmote exit,
            Embed cancelled, Embed timeouted,
            DeletionOption deletion)
        {
            backwardEmote = backward;
            forwardEmote = forward;
            skipToStartEmote = skipToStart;
            skipToEndEmote = skipToEnd;
            exitEmote = exit;
            cancelledEmbed = cancelled;
            timeoutedEmbed = timeouted;
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