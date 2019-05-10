using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Discord;
using Discord.WebSocket;
using InteractivityAddon.Actions;
using InteractivityAddon.Criterions;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Represents a class which is used to send a multi paged message to a <see cref="IMessageChannel"/> via a <see cref="InteractivityService"/>. This class is immutable!
    /// </summary>
    public sealed class Paginator
    {
        /// <summary>
        /// The pages of the <see cref="Paginator"/>.
        /// </summary>
        public ImmutableList<Embed> Pages { get; }

        /// <summary>
        /// The index of the current page of the <see cref="Paginator"/>.
        /// </summary>
        public int CurrentPageIndex { get; private set; }

        /// <summary>
        /// The current page of the <see cref="Paginator"/>.
        /// </summary>
        public Embed CurrentPage => Pages[CurrentPageIndex].DeepClone();

        /// <summary>
        /// Determited whether everyone can interact with the <see cref="Paginator"/>.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;

        /// <summary>
        /// Determites which users can interact with the <see cref="Paginator"/>.
        /// </summary>
        public ImmutableList<SocketUser> Users { get; }

        /// <summary>
        /// The appearance of the <see cref="Paginator"/>.
        /// </summary>
        public PaginatorAppearance Appearance { get; }

        internal Paginator(List<Embed> pages, int currentPageIndex, List<SocketUser> users, PaginatorAppearance appearance)
        {
            Pages = pages.ToImmutableList();
            Users = users.ToImmutableList();
            CurrentPageIndex = currentPageIndex;        
            Appearance = appearance;
        }

        internal void ApplyAction(PaginatorAction action, out bool pageChanged)
        {
            int pageBeforeChangeApply = CurrentPageIndex;

            switch (action) {
                case PaginatorAction.Backward:
                    if (CurrentPageIndex > 0) {
                        CurrentPageIndex--;
                    }
                    break;
                case PaginatorAction.Forward:
                    if (CurrentPageIndex < Pages.Count - 1) {
                        CurrentPageIndex++;
                    }
                    break;
                case PaginatorAction.SkipToStart:
                    CurrentPageIndex = 0;
                    break;
                case PaginatorAction.SkipToEnd:
                    CurrentPageIndex = Pages.Count - 1;
                    break;
            }

            pageChanged = pageBeforeChangeApply != CurrentPageIndex
                          ? true
                          : false;
        }

        internal Criteria<SocketReaction> GetCriteria()
        {
            var criteria = new Criteria<SocketReaction>(
                new EnsureReactionEmote(Appearance.Emotes)
                );

            if (IsUserRestricted == true) {
                criteria.AddCriterion(new EnsureReactionUser(Users.ToArray()));
            }

            return criteria;
        }

        internal ActionCollection<SocketReaction> GetActions() => new ActionCollection<SocketReaction>(
            new DeleteReactions(Appearance.DeleteOtherReactions, true)
            );

    }
}
