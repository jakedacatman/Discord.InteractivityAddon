using System.Threading.Tasks;
using Discord.WebSocket;

namespace InteractivityAddon.Actions
{
    /// <summary>
    /// Deletes Valid or Invalid messages
    /// </summary>
    public sealed class DeleteMessages : IAction<SocketMessage>
    {
        public bool ApplyToInvalid { get; }
        public bool ApplyToValid { get; }

        public DeleteMessages(bool invalidMessages, bool validMessages)
        {
            ApplyToInvalid = invalidMessages;
            ApplyToValid = validMessages;
        }

        public async Task ApplyAsync(SocketMessage value) => await value.DeleteAsync();
    }
}
