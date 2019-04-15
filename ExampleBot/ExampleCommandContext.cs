using Discord.WebSocket;
using Qmmands;

namespace ExampleBot_Qmmands
{
    public class ExampleCommandContext : ICommandContext
    {
        public ISocketMessageChannel Channel { get; }
        public SocketUser User { get; }
        public SocketGuild Guild { get; }

        public ExampleCommandContext(ISocketMessageChannel channel, SocketUser user, SocketGuild guild)
        {
            Channel = channel;
            User = user;
            Guild = guild;
        }
        public ExampleCommandContext(SocketMessage message)
        {
            Channel = message.Channel;
            User = message.Author;
            Guild = (User as SocketGuildUser).Guild;
        }
    }
}
