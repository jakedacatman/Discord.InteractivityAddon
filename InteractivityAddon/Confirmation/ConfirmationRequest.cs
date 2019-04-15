using System.Collections.Generic;
using System.Collections.Immutable;
using Discord;
using Discord.WebSocket;
using InteractivityAddon.Actions;
using InteractivityAddon.Criterions;

namespace InteractivityAddon.Confirmation
{
    /// <summary>
    /// Used to confirm an action via reaction on a <see cref="IUserMessage"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConfirmationRequest
    {
        public ConfirmationAppearance Appearance { get; }
        public IUserMessage Message { get; }
        public ImmutableList<ulong> Users { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ConfirmationRequest"/>.
        /// </summary>
        /// <param name="message">The message to add the reactions on.</param>
        /// <param name="users">The users who can select a reaction.</param>
        /// <param name="appearance">The visual options of this <see cref="ConfirmationRequest"/>.</param>
        public ConfirmationRequest(IUserMessage message, List<ulong> users = null,
            ConfirmationAppearance appearance = null)
        {
            Appearance = appearance ?? ConfirmationAppearance.Default;
            Message = message;
            Users = users?.ToImmutableList() ?? new List<ulong>().ToImmutableList();
        }

        /// <summary>
        /// Creates a new instance of <see cref="ConfirmationRequest"/>.
        /// </summary>
        /// <param name="message">The message to add the reactions on.</param>
        /// <param name="user">The user who can select a reaction.</param>
        /// <param name="appearance">The visual options of this <see cref="ConfirmationRequest"/>.</param>
        public ConfirmationRequest(IUserMessage message, ulong user,
            ConfirmationAppearance appearance = null) : this(message, new List<ulong>() { user }, appearance)
        {
        }

        internal Criteria<SocketReaction> GetCriterions()
        {
            var criteria = new Criteria<SocketReaction>(
                new EnsureReactionEmote(Appearance.Emotes));

            if (Users.IsEmpty == false) {
                criteria.AddCriterion(new EnsureReactionUser(Users));
            }

            return criteria;
        }

        internal ActionCollection<SocketReaction> GetActions() => new ActionCollection<SocketReaction>(
            new DeleteReactions(true, true)
            );
    }
}
