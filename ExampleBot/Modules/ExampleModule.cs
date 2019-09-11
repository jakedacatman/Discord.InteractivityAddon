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
        public InteractivityService Interactivity { get; set; }

        [Command("confirm")]
        public async Task ExampleConfirmationAsync()
        {
            var request = new ConfirmationBuilder()
                .WithContent(new PageBuilder(text:"Please Confirm!"))
                .Build();

            var result = await Interactivity.GetUserConfirmationAsync(request, Context.Channel);

            if (result.Value == true) {
                await Context.Channel.SendMessageAsync("Confirmed :thumbsup:!");
            }
            else
            {
                await Context.Channel.SendMessageAsync("Declined :thumbsup:!");
            }
        }

        [Command("paginator")]
        public async Task ExamplePaginatorAsync()
        {
            var pages = new PageBuilder[] {
                PageBuilder.FromEmbed( new EmbedBuilder().WithTitle("I").Build()),
                PageBuilder.FromEmbed( new EmbedBuilder().WithTitle("am").Build()),
                PageBuilder.FromEmbed( new EmbedBuilder().WithTitle("cool").Build()),
                PageBuilder.FromEmbed(new EmbedBuilder().WithTitle(":sunglasses:").Build()),
                new PageBuilder("I am cool :crown:")
            };

            var paginator = new PaginatorBuilder()
                .WithPages(pages)
                .WithUsers(Context.User)
                .WithPaginatorFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
                .WithAppearance(PaginatorAppearanceBuilder.Default.WithDeletion(DeletionOption.Valid | DeletionOption.AfterCapturedContext))
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

        [Command("reactionselection")]
        public async Task ExampleReactionSelectionAsync()
        {
            var builder = new ReactionSelectionBuilder<string>()
                .WithValues("Hi", "How", "Hey", "Huh?!")
                .WithEmotes(new Emoji("💵"), new Emoji("🍭"), new Emoji("😩"), new Emoji("💠"))
                .WithUsers(Context.User)
                .WithDeletion(DeletionOption.AfterCapturedContext | DeletionOption.Invalids);

            var result = await Interactivity.SendSelectionAsync(builder.Build(), Context.Channel, TimeSpan.FromSeconds(50));

            if (result.IsSuccess == true) {
                await Context.Channel.SendMessageAsync(result.Value.ToString());
            }
        }

        [Command("messageselection")]
        public async Task ExampleMessageSelectionAsync()
        {
            var builder = new MessageSelectionBuilder<string>()
                .WithValues("Hi", "How", "Hey", "Huh?!")
                .WithUsers(Context.User)
                .WithDeletion(DeletionOption.AfterCapturedContext | DeletionOption.Invalids);

            var result = await Interactivity.SendSelectionAsync(builder.Build(), Context.Channel, TimeSpan.FromSeconds(50));

            if (result.IsSuccess == true)
            {
                await Context.Channel.SendMessageAsync(result.Value.ToString());
            }
        }
    }
}
