using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that each user has only a limited amount of tries on that interactionrequest.
    /// </summary>
    public sealed class EnsureLimitedMessagesPerUser : ICriterion<SocketMessage>
    {
        private ImmutableDictionary<ulong, int> MessageCounter { get; }
        private int MessageLimit { get; }

        public EnsureLimitedMessagesPerUser(int messageLimit)
        {
            MessageCounter = new Dictionary<ulong, int>().ToImmutableDictionary();
            MessageLimit = messageLimit;
        }

        public Task<bool> JudgeAsync(SocketMessage value)
        {
            bool isFirstMessage = !MessageCounter.TryGetValue(value.Author.Id, out int count);

            MessageCounter.Add(value.Author.Id, count + 1);

            return isFirstMessage == true || count <= MessageLimit
                ? Task.FromResult(true)
                : Task.FromResult(false);
        }
    }
}
