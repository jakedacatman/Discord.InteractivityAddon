using System.Threading.Tasks;
using Discord.WebSocket;

namespace Interactivity.Extensions
{
    internal static class SocketReactionExtensions
    {
        public static async Task DeleteAsync(this SocketReaction reaction, BaseSocketClient client)
        {
            var channel = reaction.Channel;
            var message = reaction.Message.Value ?? await channel.GetMessageAsync(reaction.MessageId).ConfigureAwait(false) as SocketUserMessage;
            var user = reaction.User.Value ?? client.GetUser(reaction.UserId);

            await message.RemoveReactionAsync(reaction.Emote, user).ConfigureAwait(false);
        }
    }
}
