using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that the <see cref="SocketReaction"/> is added to a specific <see cref="IMessage"/>.
    /// </summary>
    public sealed class EnsureReactionMessage : ICriterion<SocketReaction>
    {
        public ulong AllowedMessageId { get; }

        public EnsureReactionMessage(ulong messageId)
        {
            AllowedMessageId = messageId;
        }

        public EnsureReactionMessage(IMessage message)
        {
            AllowedMessageId = message.Id;
        }

        public Task<bool> JudgeAsync(SocketReaction value)
            => Task.FromResult(value.MessageId == AllowedMessageId);
    }
}