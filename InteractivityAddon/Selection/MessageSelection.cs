using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents the default selection which uses messages. This class is immutable!
    /// </summary>
    /// <typeparam name="T">The type of values to select from.</typeparam>
    public sealed class MessageSelection<T> : Selection<T, SocketMessage, MessageSelectionAppearance>
    {
        /// <summary>
        /// The possibilites to select from.
        /// </summary>
        public ImmutableList<string> Possibilities { get; }

        internal MessageSelection(ImmutableList<T> values, ImmutableList<string> possabilies, ImmutableList<SocketUser> users, MessageSelectionAppearance appearance, Embed selectionEmbed)
            : base(values, users, appearance, selectionEmbed)
        {
            Possibilities = possabilies;
        }

        public override Task<Optional<InteractivityResult<T>>> ParseAsync(SocketMessage value)
        {
            int index = Possibilities.FindIndex(x => x == value.Content) / 4;

            return Task.FromResult(Optional.Create(
                index >= Values.Count
                ? new InteractivityResult<T>(default, false, true)
                : new InteractivityResult<T>(Values[index], false, false)
                ));
        }

        public override Task<bool> RunChecksAsync(BaseSocketClient client, IUserMessage message, SocketMessage value) => Task.FromResult(Possibilities.Contains(value.Content));
    }
}
