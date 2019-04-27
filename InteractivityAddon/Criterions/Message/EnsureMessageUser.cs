using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that the <see cref="SocketMessage"/> is from a specific user.
    /// </summary>
    public sealed class EnsureMessageUser : ICriterion<SocketMessage>
    {
        public ImmutableList<ulong> AllowedUserIds { get; }

        public EnsureMessageUser(params ulong[] userIds)
        {
            AllowedUserIds = userIds.ToImmutableList();
        }

        public EnsureMessageUser(List<ulong> userIds)
        {
            AllowedUserIds = userIds.ToImmutableList();
        }

        public EnsureMessageUser(params SocketUser[] users)
        {
            var allowedUserIds = new List<ulong>();
            foreach (var user in users) {
                allowedUserIds.Add(user.Id);
            }
            AllowedUserIds = allowedUserIds.ToImmutableList();
        }

        public EnsureMessageUser(ImmutableList<ulong> userIds)
        {
            AllowedUserIds = userIds;
        }

        public Task<bool> JudgeAsync(SocketMessage value)
            => Task.FromResult(AllowedUserIds.Contains(value.Author.Id));
    }
}