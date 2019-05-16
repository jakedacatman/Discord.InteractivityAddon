# Discord.InteractivityAddon

[![NuGet](https://img.shields.io/nuget/vpre/Discord.InteractivityAddon.svg?style=plastic)](https://www.nuget.org/packages/Discord.InteractivityAddon)
[![BotFacility](https://img.shields.io/discord/512366986383065088.svg?style=flat-square&label=discord)](https://discord.gg/gAmSyVD)

This is an addon for the Discord API Wrapper [Discord.Net](https://github.com/discord-net/Discord.Net) which makes it easy to add interactivity to your discord bot.

## Credits
 - The package has been created with the support of [TwentyFourMinutes](https://github.com/TwentyFourMinutes). Check out his [UtilityAddon](https://github.com/TwentyFourMinutes/Discord.UtilityAddon) which provides some great features for your bot :)
 - The criterion system is inspired by foxbot's [Discord.Addons.Interactive](https://github.com/foxbot/Discord.Addons.Interactive) package.

## Installation
The package can be pulled easily from [NuGet](https://www.nuget.org/packages/Discord.InteractivityAddon).

## Features
 - Waiting for a specific message or reaction using a powerful criteria system
 - Waiting for a message / reaction which passes your criteria
   - Run actions on filtered messages / reactions
 - Send and delete messages & files with delays
 - A powerful fully customizable Paginator
   - Send multi page messages
   - Move through pages using reactions
 - Fully customizable selection from a list of objects
   - Makes user input easy
   - Works via messages or reactions
   - For more customizability you can create your own child of the selection class 
 - Confirmation of an action
 - Uptime counter
 
## Usage
To properly use the features this addon provides you need the `InteractivityService` to your service provider.

```cs
var provider = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton(new InteractivityService(Client, TimeSpan.FromMinutes(3)))
```
This addon does not include a custom `ModuleBase` in order to support every command framework. (Discord.Net.Command/Qmmands/...)

### Example Selection Command
```cs
[Command("select")]
public async Task ExampleSelectionAsync()
{
    var builder = new ReactionSelectionBuilder<string>()
        .WithValues("Hi", "How", "Hey", "Huh?!")
        .WithEmotes(new Emoji("💵"), new Emoji("🍭"), new Emoji("😩"), new Emoji("💠"))
        .WithUsers(Context.User)
        .WithAppearance(ReactionSelectionAppearanceBuilder.Default.WithDeletion(DeletionOption.AfterCapturedContext);

    var result = await _interactivity.SendSelectionAsync(await builder.Build(), Context.Channel, TimeSpan.FromSeconds(50));

    if (result.IsSuccess == true) {
        await Context.Channel.SendMessageAsync(result.Value.ToString());
    }
}
```

### Example Paginator Command
```cs
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
        .WithUsers(Context.User)
        .WithPaginatorFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
        .Build();

    await _interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));
}
```

### Example Confirmation Command
```cs
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
```
