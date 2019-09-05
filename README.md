# Discord.InteractivityAddon

[![NuGet](https://img.shields.io/nuget/vpre/Discord.InteractivityAddon.svg?style=plastic)](https://www.nuget.org/packages/Discord.InteractivityAddon)
[![BotFacility](https://img.shields.io/discord/512366986383065088.svg?style=flat-square&label=discord)](https://discord.gg/gAmSyVD)

This is an addon for the Discord API Wrapper [Discord.Net](https://github.com/discord-net/Discord.Net) which makes it easy to add interactivity to your discord bot.

## Installation
The package is available to download on [NuGet](https://www.nuget.org/packages/Discord.InteractivityAddon).

## Features
 - Waiting for a message / reaction which passes your filter
   - Run actions on filtered messages / reactions
 - Delayed sending and deleting messages & files
 - A powerful fully customizable Paginator
   - Send multi page messages
   - Move through pages using reactions
 - Fully customizable selection from a list of objects
   - Works with messages or reactions
   - Makes user input easy
   - For more customizability you can create your own child of the selection class 
 - Confirmation
 - Uptime counter
 
## Usage
To properly use the features this addon provides you need the `InteractivityService` to your service provider.

```cs
var provider = new ServiceCollection()
                .AddSingleton(new InteractivityService(Client, TimeSpan.FromMinutes(3)))
                ....
```
This addon does not include a custom `ModuleBase` in order to support every command framework. (Discord.Net.Command/Qmmands/...)

### Example: Waiting for Message
```cs
[Command("nextmessage")]
public async Task ExampleReplyNextMessageAsync()
{
    var result = await Interactivity.NextMessageAsync(x => x.Author == Context.User);

    if (result.IsSuccess == true) {
        Interactivity.DelayedSendMessageAndDeleteAsync(
                        Context.Channel,
                        deleteDelay: TimeSpan.FromSeconds(20), 
                        text: result.Value.Content, 
                        embed: result.Value.Embeds.FirstOrDefault());
    }
}
```

### Example: Selection
```cs
[Command("select")]
public async Task ExampleSelectionAsync()
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
```

### Example: Paginator
```cs
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
        .WithAppearance(PaginatorAppearanceBuilder.Default.WithDeletion(DeletionOption.Invalids))
        .Build();

    await Interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));
}
```

### Example: Confirmation
```cs
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
```
