using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that the <see cref="SocketReaction"/> is from one of the users in <see cref="AllowedUserIds"/>.
    /// </summary>
    public sealed class EnsureReactionUser : ICriterion<SocketReaction>
    {
        public ImmutableList<ulong> AllowedUserIds { get; }

        public EnsureReactionUser(params ulong[] userIds)
        {
            AllowedUserIds = userIds.ToImmutableList();
        }

        public EnsureReactionUser(params SocketUser[] users)
        {
            var allowedUserIds = new List<ulong>();
            foreach (var user in users) {
                allowedUserIds.Add(user.Id);
            }
            AllowedUserIds = allowedUserIds.ToImmutableList();
        }

        public EnsureReactionUser(ImmutableList<ulong> allowedUserIds)
        {
            AllowedUserIds = allowedUserIds;
        }

        public Task<bool> JudgeAsync(SocketReaction value)
            => Task.FromResult(AllowedUserIds.Contains(value.UserId) == true);
    }
}