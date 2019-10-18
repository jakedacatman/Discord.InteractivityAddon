using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace Interactivity.Pagination
{
    /// <summary>
    /// Represents a class which is used to send a multi paged message to a <see cref="IMessageChannel"/> via a <see cref="InteractivityService"/>. This class is immutable!
    /// </summary>
    public sealed class Paginator
    {
        /// <summary>
        /// The pages of the <see cref="Paginator"/>.
        /// </summary>
        public IReadOnlyList<Page> Pages { get; }

        private int _currentPageIndex;

        /// <summary>
        /// The index of the current page of the <see cref="Paginator"/>.
        /// </summary>
        public int CurrentPageIndex
        {
            get => _currentPageIndex; internal set {
                _currentPageIndex = value;
                if (value < 0)
                {
                    _currentPageIndex = 0;
                }
                if (value > Pages.Count - 1)
                {
                    _currentPageIndex = Pages.Count - 1;
                }
            }
        }

        /// <summary>
        /// The current page of the <see cref="Paginator"/>.
        /// </summary>
        public Page CurrentPage => Pages[CurrentPageIndex];

        /// <summary>
        /// Determited whether everyone can interact with the <see cref="Paginator"/>.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;

        /// <summary>
        /// Determites which users can interact with the <see cref="Paginator"/>.
        /// </summary>
        public IReadOnlyList<SocketUser> Users { get; }

        /// <summary>
        /// The appearance of the <see cref="Paginator"/>.
        /// </summary>
        public PaginatorAppearance Appearance { get; }

        internal Paginator(ImmutableArray<Page> pages, int currentPageIndex, ImmutableArray<SocketUser> users, PaginatorAppearance appearance)
        {
            Pages = pages;
            Users = users;
            CurrentPageIndex = currentPageIndex;
            Appearance = appearance;
        }

        internal bool ApplyAction(PaginatorAction action)
        {
            int pageBeforeChangeApply = CurrentPageIndex;

            switch (action)
            {
                case PaginatorAction.Backward:
                    CurrentPageIndex--;
                    break;
                case PaginatorAction.Forward:
                    CurrentPageIndex++;
                    break;
                case PaginatorAction.SkipToStart:
                    CurrentPageIndex = 0;
                    break;
                case PaginatorAction.SkipToEnd:
                    CurrentPageIndex = Pages.Count - 1;
                    break;
            }

            return pageBeforeChangeApply != CurrentPageIndex
                          ? true
                          : false;
        }

        internal Predicate<SocketReaction> GetReactionFilter() => reaction =>
            Appearance.Emotes.Contains(reaction.Emote)
            &&
            (!IsUserRestricted || Users.Where(x => x.Id == reaction.UserId).Any());

        internal Func<SocketReaction, bool, Task> GetActions() => async (reaction, invalid) =>
            {
                if ((Appearance.Deletion.HasFlag(DeletionOption.Invalids) && invalid)
                    ||
                    (Appearance.Deletion.HasFlag(DeletionOption.Valid) && !invalid))
                {
                    await reaction.Message.Value.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
                }
            };
    }
}
