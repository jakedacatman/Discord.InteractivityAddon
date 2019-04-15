using System.Threading.Tasks;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that the <see cref="SocketMessage"/> is in a specific <see cref="ISocketMessageChannel"/>.
    /// </summary>
    public sealed class EnsureMessageChannel : ICriterion<SocketMessage>
    {
        public ulong AllowedChannelId { get; }

        public EnsureMessageChannel(ulong channelId)
        {
            AllowedChannelId = channelId;
        }

        public EnsureMessageChannel(ISocketMessageChannel channel)
        {
            AllowedChannelId = channel.Id;
        }

        public Task<bool> JudgeAsync(SocketMessage value)
            => Task.FromResult(AllowedChannelId == value.Channel.Id);
    }
}