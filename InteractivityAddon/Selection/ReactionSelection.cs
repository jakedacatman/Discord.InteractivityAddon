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
    public sealed class ReactionSelection<T> : Selection<T, SocketReaction, ReactionSelectionAppearance>
    {
        /// <summary>
        /// The possibilites of emotes to select from.
        /// </summary>
        public ImmutableList<IEmote> Possabilities { get; }

        /// <summary>
        /// Gets whether the <see cref="Selection{T, T1, TSelectionAppearance}"/> allows for cancellation.
        /// </summary>
        public bool AllowCancel { get; }

        internal ReactionSelection(Embed selectionEmbed, ImmutableList<T> values, ImmutableList<IEmote> possabilities, ReactionSelectionAppearance appearance, ImmutableList<SocketUser> users, bool allowCancel)
            : base(values, users, appearance, selectionEmbed)
        {
            Possabilities = possabilities;
            AllowCancel = allowCancel;

            if (AllowCancel == true) {
                Possabilities = Possabilities.Add(Appearance.CancelEmote);
            }
        }

        public override async Task InitializeMessageAsync(IUserMessage message) => await message.AddReactionsAsync(Possabilities.ToArray());

        public override Task<Optional<InteractivityResult<T>>> ParseAsync(SocketReaction value)
        { 
            int index = Possabilities.FindIndex(x => x.Equals(value.Emote));

            return Task.FromResult(Optional.Create( 
                index >= Values.Count
                ? new InteractivityResult<T>(default, false, true)
                : new InteractivityResult<T>(Values[index], false, false)
                ));
        }

        public override Task<bool> RunChecksAsync(BaseSocketClient client, IUserMessage message, SocketReaction value) => Task.FromResult(Possabilities.Contains(value.Emote));
    }
}
