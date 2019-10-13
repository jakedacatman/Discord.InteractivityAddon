using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using InteractivityAddon.Extensions;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a <see langword="abstract"/> class which allows for user selection in discord.
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    /// <typeparam name="T1">The way of selecting in discord. Either <see cref="SocketMessage"/> or <see cref="SocketReaction"/>.</typeparam>
    /// <typeparam name="TAppearance">The custom appearance of the selection.</typeparam>
    public abstract class Selection<T, T1> where T1 : class
    {
        #region Fields
        /// <summary>
        /// Gets the values to select from in the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public ImmutableList<T> Values { get; }

        /// <summary>
        /// Gets which users can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public ImmutableList<SocketUser> Users { get; }

        /// <summary>
        /// Gets the <see cref="Embed"/> which is sent into the channel.
        /// </summary>
        public Embed SelectionEmbed { get; }

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after cancellation.
        /// </summary>
        public Embed CancelledEmbed { get; }

        /// <summary>
        /// Gets the <see cref="EmbedBuilder"/> which the <see cref="Selection{T}"/> gets modified to after a timeout.
        /// </summary>
        public Embed TimeoutedEmbed { get; }

        /// <summary>
        /// Gets an ORing determiting what the <see cref="Selection{T1, T2}"/> will delete.
        /// </summary>
        public DeletionOption Deletion { get; }

        /// <summary>
        /// Determites whether everyone can interact with the <see cref="Selection{T, T1}"/>.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;

        /// <summary>
        /// Determites whether the selection is of <see cref="IEmote"/>.
        /// </summary>
        public bool IsReactionSelection => typeof(T1) == typeof(SocketReaction);
        #endregion

        #region Constructor
        protected Selection(ImmutableList<T> values, ImmutableList<SocketUser> users,
            Embed selectionEmbed, Embed cancelledEmbed, Embed timeoutedEmbed,
            DeletionOption deletion)
        {
            if (typeof(T1) != typeof(SocketReaction) && typeof(T1) != typeof(SocketMessage))
            {
                throw new InvalidOperationException("T2 can ONLY be SocketMessage or SocketReaction!");
            }

            Values = values;
            Users = users;
            SelectionEmbed = selectionEmbed;
            CancelledEmbed = cancelledEmbed;
            TimeoutedEmbed = timeoutedEmbed;
            Deletion = deletion;
        }
        #endregion

        #region Methods
        internal async Task<bool> HandleResponseAsync(BaseSocketClient client, T1 response)
        {
            bool valid = false;

            if (response is SocketMessage s)
            {
                valid = await RunChecksAsync(client, response).ConfigureAwait(false) && (IsUserRestricted || Users.Contains(s.Author));
                if (Deletion.HasFlag(DeletionOption.Invalids) == true && valid == false)
                {
                    await s.DeleteAsync().ConfigureAwait(false);
                }
                if (Deletion.HasFlag(DeletionOption.Valid) == true && valid == true)
                {
                    await s.DeleteAsync().ConfigureAwait(false);
                }
            }
            if (response is SocketReaction r)
            {
                var user = r.User.Value as SocketUser ?? client.GetUser(r.UserId);
                valid = await RunChecksAsync(client, response).ConfigureAwait(false) && (IsUserRestricted || Users.Contains(user));
                if (Deletion.HasFlag(DeletionOption.Invalids) == true && valid == false)
                {
                    await r.DeleteAsync(client).ConfigureAwait(false);
                }
                if (Deletion.HasFlag(DeletionOption.Valid) == true && valid == true)
                {
                    await r.DeleteAsync(client).ConfigureAwait(false);
                }
            }
            return valid;
        }

        /// <summary>
        /// Do something to the message before the actual selection starts. E.g. adding reactions...
        /// </summary>
        /// <param name="message">The selection message.</param>
        /// <returns></returns>
        public virtual Task InitializeMessageAsync(IUserMessage message)
            => Task.CompletedTask;

        /// <summary>
        /// Run additional checks on the <see cref="T1"/> to decide whether it is parseable or not.
        /// </summary>
        /// <param name="client">The standard discord client.</param>
        /// <param name="message">The selection message.</param>
        /// <param name="value">The value to run checks on.</param>
        /// <returns></returns>
        public virtual Task<bool> RunChecksAsync(BaseSocketClient client, T1 value)
            => Task.FromResult(true);

        /// <summary>
        /// Run additional actions to the selection message dependent of the user input beeing valid or invalid.
        /// </summary>
        /// <param name="client">The standard discord client.</param>
        /// <param name="message">The selection message.</param>
        /// <param name="value">The user input.</param>
        /// <param name="isValid">Whether the user input passed the checks.</param>
        /// <returns></returns>
        public virtual Task RunActionsAsync(BaseSocketClient client, IUserMessage message, T1 value, bool isValid)
            => Task.CompletedTask;

        /// <summary>
        /// Try to parse the user input to a <see cref="InteractivityResult{T}"/>. 
        /// </summary>
        /// <param name="value">The value to parse</param>
        /// <returns></returns>
        public abstract Task<Optional<InteractivityResult<T>>> ParseAsync(T1 value, DateTime startTime);
        #endregion
    }
}
