using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using InteractivityAddon;
using InteractivityAddon.Confirmation;
using InteractivityAddon.Pagination;
using InteractivityAddon.Selection;
using Qmmands;

namespace ExampleBot_Qmmands.Modules
{
    public class ExampleModule : ModuleBase<ExampleCommandContext>
    {
        public InteractivityService _interactivity { get; set; }

        [Command("confirm")]
        public async Task ExampleConfirmationAsync()
        {
            var message = await Context.Channel.SendMessageAsync("Please confirm!");
            var request = new ConfirmationRequest(message, Context.User.Id);

            var result = await _interactivity.GetUserConfirmationAsync(request);

            if (result.Value == true) {
                await message.ModifyAsync(x => x.Content = "Confirmed :thumbsup:!");
            }
        }

        [Command("paginator")]
        public async Task ExamplePaginatorAsync()
        {
            var pages = new List<Embed>() {
                new EmbedBuilder().WithTitle("I").Build(),
                new EmbedBuilder().WithTitle("am").Build(),
                new EmbedBuilder().WithTitle("cool").Build(),
                new EmbedBuilder().WithTitle(":sunglasses: ").Build(),
            };

            var paginator = new PaginatorBuilder()
                .WithEmbeds(pages.ToArray())
                .WithSettings(PaginatorSettings.Default)
                .WithUsers(Context.User)
                .WithPaginatorFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
                .Build();

            await _interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));
        }

        [Command("delete")]
        public async Task ExampleReactAsync()
        {
            _interactivity.DelayedSendMessageAndDeleteAsync(Context.Channel, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20), "How are you? :D");

            _interactivity.DelayedDeleteMessageAsync(await Context.Channel.SendMessageAsync("Hello"), TimeSpan.FromSeconds(3));
        }

        [Command("nextmessage")]
        public async Task ReplyNextMessageAsync()
        {
            var result = await _interactivity.NextMessageAsync(x => x.Author == Context.User);

            if (result.IsSuccess == true) {
                _interactivity.DelayedSendMessageAndDeleteAsync(Context.Channel, deleteDelay: TimeSpan.FromSeconds(20), text: result.Value.Content, embed: result.Value.Embeds.FirstOrDefault());
            }


        }

        [Command("deleteall")]
        public async Task DeleteAllMessagesAsync()
        {
            await Context.Channel.SendMessageAsync("You can't send messages anymore!");
            await _interactivity.NextMessageAsync(x => false, async x => await x.DeleteAsync(), timeout: TimeSpan.FromSeconds(15));
            await Context.Channel.SendMessageAsync("You can now send messages!");
        }

        [Command("select")]
        public async Task SelectAsync()
        {
            var request = new SelectionRequest<string>(new List<string>() { "Hi", "How", "Hey", "Huh?!" }, selectingUser: Context.User.Id, allowCancel: true);

            var result = await _interactivity.GetUserSelectionAsync(request, Context.Channel, TimeSpan.FromSeconds(50));

            if (result.IsSuccess == true) {
                await Context.Channel.SendMessageAsync(result.Value);
            }
        }
    }
}
