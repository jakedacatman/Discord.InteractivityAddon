# Discord.InteractivityAddon

[![NuGet](https://img.shields.io/nuget/vpre/Discord.InteractivityAddon.svg?style=plastic)](https://www.nuget.org/packages/Discord.InteractivityAddon)
[![BotFacility](https://img.shields.io/discord/512366986383065088.svg?style=flat-square&label=discord)](https://discord.gg/gAmSyVD)

This is an addon for the Discord API Wrapper [Discord.Net](https://github.com/discord-net/Discord.Net) which adds a lot of useful interactivity features.

## Credits
 - The package has been created with the support of [TwentyFourMinutes](https://github.com/TwentyFourMinutes). Check out his [UtilityAddon](https://github.com/TwentyFourMinutes/Discord.UtilityAddon) which provides some great features for your bot :)
 - The criterion system is inspired by foxbot's [Discord.Addons.Interactive](https://github.com/foxbot/Discord.Addons.Interactive) package.

## Installation
The package can be pulled easily from [NuGet](https://www.nuget.org/packages/Discord.InteractivityAddon).

## Features
 - Waiting for a specific message or reaction using a powerful criteria system
 - Applying a custom action to every message / reaction which passes your criteria
 - Send and delete messages & files with delays
 - A powerful Paginator
 - Selection from a custom list of objects
 - Confirmation of an action
 
## Usage
To use the features you need to add an `InteractivityService` to your service provider.

```cs
var provider = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(CommandService)
                .AddSingleton(new InteractivityService(Client, TimeSpan.FromMinutes(3)))
```
This addon does not include a custom `ModuleBase` to be able to support every command framework.

### Example Selection Command
```cs
[Command("select")]
public async Task ExampleSelectionAsync()
{
    var builder = new SelectionBuilder<string>()
                    .WithSettings(allowCancel: true)
                    .WithUsers(Context.User)
                    .WithValues(new[] { "Hi", "How", "Hey", "Huh?!" })
                    .WithAppearance(SelectionAppearance.Default
                    .WithSettings(deleteSelectionAfterCapturedResult: true));
                            
    var result = await _interactivity.GetUserSelectionAsync(builder.Build(), Context.Channel, TimeSpan.FromSeconds(50));

    if (result.IsSuccess == true) {
        await Context.Channel.SendMessageAsync(result.Value);
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
