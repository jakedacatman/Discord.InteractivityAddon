using System.Collections.Immutable;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Criterions
{
    /// <summary>
    /// Ensures that the <see cref="SocketReaction"/> contains a specific <see cref="IEmote"/>.
    /// </summary>
    public sealed class EnsureReactionEmote : ICriterion<SocketReaction>
    {
        public ImmutableList<IEmote> AllowedEmotes { get; }

        public EnsureReactionEmote(params IEmote[] allowedEmotes)
        {
            AllowedEmotes = allowedEmotes.ToImmutableList();
        }

        public Task<bool> JudgeAsync(SocketReaction value)
            => Task.FromResult(AllowedEmotes.Contains(value.Emote));
    }
}