using System.Threading.Tasks;
using Discord.WebSocket;

namespace InteractivityAddon.Actions
{
    public sealed class DeleteReactions : IAction<SocketReaction>
    {
        public bool ApplyToInvalid { get; }
        public bool ApplyToValid { get; }

        public DeleteReactions(bool applyToInvalid, bool applyToValid)
        {
            ApplyToInvalid = applyToInvalid;
            ApplyToValid = applyToValid;
        }

        public async Task ApplyAsync(SocketReaction value) => await value.Message.Value.RemoveReactionAsync(value.Emote, value.User.Value);

    }
}
