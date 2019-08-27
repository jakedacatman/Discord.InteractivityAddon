using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using InteractivityAddon;
using InteractivityAddon.Confirmation;
using InteractivityAddon.Pagination;
using InteractivityAddon.Selection;
using InteractivityAddon.Selection.Appearance;
using Qmmands;

namespace ExampleBot_Qmmands.Modules
{
    public class ExampleModule : ModuleBase<ExampleCommandContext>
    {
        public InteractivityService Interactivity { get; set; }

        [Command("confirm")]
        public async Task ExampleConfirmationAsync()
        {
            var message = await Context.Channel.SendMessageAsync("Please confirm!");
            var request = new ConfirmationRequest(message, Context.User.Id);

            var result = await Interactivity.GetUserConfirmationAsync(request);

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
                new EmbedBuilder().WithTitle(":sunglasses:").Build(),
            };

            var paginator = new PaginatorBuilder()
                .WithEmbeds(pages.ToArray())
                .WithUsers(Context.User)
                .WithPaginatorFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
                .WithAppearance(PaginatorAppearanceBuilder.Default.WithCancelledEmbed(new EmbedBuilder()).WithDeletion(DeletionOption.Invalids))
                .Build();

            await Interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));
        }

        [Command("delete")]
        public async Task ExampleReactAsync()
        {
            Interactivity.DelayedSendMessageAndDeleteAsync(Context.Channel, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20), "How are you? :D");

            Interactivity.DelayedDeleteMessageAsync(await Context.Channel.SendMessageAsync("Hello"), TimeSpan.FromSeconds(3));
        }

        [Command("nextmessage")]
        public async Task ExampleReplyNextMessageAsync()
        {
            var result = await Interactivity.NextMessageAsync(x => x.Author == Context.User);

            if (result.IsSuccess == true) {
                Interactivity.DelayedSendMessageAndDeleteAsync(Context.Channel, deleteDelay: TimeSpan.FromSeconds(20), text: result.Value.Content, embed: result.Value.Embeds.FirstOrDefault());
            }
        }

        [Command("deleteall")]
        public async Task ExampleDeleteAllMessagesAsync()
        {
            await Context.Channel.SendMessageAsync("You can't send messages anymore!");
            await Interactivity.NextMessageAsync(x => false, async (x,v) => await x.DeleteAsync(), timeout: TimeSpan.FromSeconds(15));
            await Context.Channel.SendMessageAsync("You can now send messages!");
        }

        [Command("select")]
        public async Task ExampleSelectionAsync()
        {
            var builder = new ReactionSelectionBuilder<string>()
                .WithValues("Hi", "How", "Hey", "Huh?!")
                .WithEmotes(new Emoji("💵"), new Emoji("🍭"), new Emoji("😩"), new Emoji("💠"))
                .WithUsers(Context.User)
                .WithAppearance(ReactionSelectionAppearanceBuilder.Default.WithDeletion(DeletionOption.AfterCapturedContext | DeletionOption.Invalids));

            var result = await Interactivity.SendSelectionAsync(builder.Build(), Context.Channel, TimeSpan.FromSeconds(50));

            if (result.IsSuccess == true) {
                await Context.Channel.SendMessageAsync(result.Value.ToString());
            }
        }
    }
}
