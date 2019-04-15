using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Discord.WebSocket;
using InteractivityAddon.Actions;
using InteractivityAddon.Criterions;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Used to send a selection to a <see cref="Discord.IMessageChannel"/> via a <see cref="InteractivityService"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SelectionRequest<T>
    {
        public ImmutableList<T> Values { get; }
        public Func<T, string> NameSelector { get; }
        public ImmutableList<ulong> SelectingUsers { get; }
        internal IReadOnlyCollection<string> Names
        {
            get {
                var names = new List<string>();
                for (int i = 0; i < Values.Count; i++) {
                    names.Add(NameSelector.Invoke(Values[i]));
                }

                if (AllowCancel == true) {
                    names.Add("Cancel");
                }

                return names.ToArray();
            }
        }

        public SelectionAppearance Appearance { get; }
        public bool AllowCancel { get; }
        public bool IsCaseSensitive { get; }

        /// <summary>
        /// Creates a new <see cref="SelectionRequest{T}"/>
        /// </summary>
        /// <param name="values">The values to select from.</param>
        /// <param name="nameSelector">A function that converts <see cref="T"/> to the selectionstring in the <see cref="Discord.Embed"/>.</param>
        /// <param name="selectingUsers">The users who can select</param>
        /// <param name="appearance">The visual options of this selection.</param>
        /// <param name="allowCancel">Allow the user to select cancel instead of value.</param>
        /// <param name="isCaseSensitive">Check for case-sensitivity.</param>
        public SelectionRequest(List<T> values, List<ulong> selectingUsers = null,
            Func<T, string> nameSelector = null, SelectionAppearance appearance = null,
            bool allowCancel = false, bool isCaseSensitive = false)
        {
            SelectingUsers = selectingUsers?.ToImmutableList() ?? new List<ulong>().ToImmutableList();
            Values = values.ToImmutableList();
            Appearance = appearance ?? SelectionAppearance.Default;
            AllowCancel = allowCancel;
            NameSelector = nameSelector ?? (x => x.ToString());
            IsCaseSensitive = isCaseSensitive;
        }

        /// <summary>
        /// Creates a new <see cref="SelectionRequest{T}"/>
        /// </summary>
        /// <param name="values">The values to select from.</param>
        /// <param name="nameSelector">A function that converts <see cref="T"/> to the selectionstring in the <see cref="Discord.Embed"/>.</param>
        /// <param name="selectingUser">The user who can select</param>
        /// <param name="appearance">The visual options of this selection.</param>
        /// <param name="allowCancel">Allow the user to select cancel instead of value.</param>
        /// <param name="isCaseSensitive">Check for case-sensitivity.</param>
        public SelectionRequest(List<T> values, ulong selectingUser,
            Func<T, string> nameSelector = null, SelectionAppearance appearance = null,
            bool allowCancel = false, bool isCaseSensitive = false)
            :this(values, new List<ulong>() { selectingUser },
                nameSelector, appearance,
                allowCancel, isCaseSensitive)
        {
        }

        internal Criteria<SocketMessage> GetCriteria()
        {
            var criteria = new Criteria<SocketMessage>();

            if (SelectingUsers.Count != 0) {
                criteria.AddCriterion(new EnsureMessageUser(SelectingUsers));
            }

            return criteria;
        }

        internal ActionCollection<SocketMessage> GetActions() => new ActionCollection<SocketMessage>(
                new DeleteMessages(Appearance.DeleteInvalidMessages, Appearance.DeleteValidMessage)
            );
    }
}
