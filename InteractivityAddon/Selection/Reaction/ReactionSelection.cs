using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents the default selection which uses reactions. This class is immutable!
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    public sealed class ReactionSelection<T> : Selection<T, SocketReaction>
    {
        #region Fields
        /// <summary>
        /// The possibilites of emotes to select from.
        /// </summary>
        public ImmutableList<IEmote> Emotes { get; }

        /// <summary>
        /// Gets the cancel emote if cancel is enabled in the <see cref="ReactionSelection{T}"/>.
        /// </summary>
        public IEmote CancelEmote { get; }

        /// <summary>
        /// Gets whether the <see cref="Selection{T, T1}"/> allows for cancellation.
        /// </summary>
        public bool AllowCancel { get; }
        #endregion

        #region Constructor
        internal ReactionSelection(ImmutableList<T> values, ImmutableList<SocketUser> users, 
            Embed selectionEmbed, Embed cancelledEmbed, Embed timeoutedEmbed, DeletionOption deletion,
            ImmutableList<IEmote> emotes, IEmote cancelEmote, bool allowCancel)
            : base(values, users, selectionEmbed, cancelledEmbed, timeoutedEmbed, deletion)
        {
            Emotes = emotes;
            CancelEmote = cancelEmote;
            AllowCancel = allowCancel;

            if (AllowCancel == true) {
                Emotes = Emotes.Add(CancelEmote);
            }
        }
        #endregion

        #region Methods
        public override async Task InitializeMessageAsync(IUserMessage message) 
            => await message.AddReactionsAsync(Emotes.ToArray());

        public override Task<Optional<InteractivityResult<T>>> ParseAsync(SocketReaction value, DateTime startTime)
        { 
            int index = Emotes.FindIndex(x => x.Equals(value.Emote));

            return Task.FromResult(Optional.Create( 
                index >= Values.Count
                ? new InteractivityResult<T>(default, DateTime.UtcNow - startTime, false, true)
                : new InteractivityResult<T>(Values[index], DateTime.UtcNow - startTime, false, false)
                ));
        }

        public override Task<bool> RunChecksAsync(BaseSocketClient client, IUserMessage message, SocketReaction value) 
            => Task.FromResult(Emotes.Contains(value.Emote));
        #endregion
    }
}
