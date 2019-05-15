using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using InteractivityAddon.Actions;
using InteractivityAddon.Confirmation;
using InteractivityAddon.Criterions;
using InteractivityAddon.Pagination;
using InteractivityAddon.Selection;

namespace InteractivityAddon
{
    /// <summary>
    /// A service containing methods to make your discordbot more interactive.
    /// </summary>
    public sealed class InteractivityService
    {
        private BaseSocketClient _client { get; }

        public TimeSpan DefaultTimeout { get; }
        public DateTime StartTime { get; }

        /// <summary>
        /// Creates a new instance of <see cref="InteractivityService"/>.
        /// </summary>
        /// <param name="client">Your instance of <see cref="BaseSocketClient"/>.</param>
        /// <param name="defaultTimeout">The default timeout for this <see cref="InteractivityService"/>.</param>
        public InteractivityService(BaseSocketClient client, TimeSpan? defaultTimeout = null)
        {
            _client = client;
            StartTime = DateTime.Now;

            DefaultTimeout = defaultTimeout ?? TimeSpan.FromSeconds(45);
            if (DefaultTimeout <= TimeSpan.Zero) {
                throw new Exception("Timespan cannot be negative or zero");
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="InteractivityService"/>.
        /// </summary>
        /// <param name="client">Your instance of <see cref="DiscordSocketClient"/>.</param>
        /// <param name="defaultTimeout">The default timeout for this <see cref="InteractivityService"/>.</param>
        public InteractivityService(DiscordSocketClient client, TimeSpan? defaultTimeout = null)
            : this((BaseSocketClient) client, defaultTimeout)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="InteractivityService"/>.
        /// </summary>
        /// <param name="client">Your instance of <see cref="DiscordShardedClient"/>.</param>
        /// <param name="defaultTimeout">The default timeout for this <see cref="InteractivityService"/>.</param>
        public InteractivityService(DiscordShardedClient client, TimeSpan? defaultTimeout = null)
            : this((BaseSocketClient) client, defaultTimeout)
        {
        }

        /// <summary>
        /// Get the time passed since the bot started
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetUptime() => DateTime.Now - StartTime;

        /// <summary>
        /// Sends a message to a channel delayed and deletes it after another delay.
        /// </summary>
        /// <param name="channel">The target channel.</param>
        /// <param name="sendDelay">The time to wait before sending the message.</param>
        /// <param name="deleteDelay">The time to wait between sending and deleting the message.</param>
        /// <param name="text">The message to be sent.</param>
        /// <param name="isTTS">Determines whether the message should be read aloud by Discord or not.</param>
        /// <param name="embed">The <see cref="Embed"/> to be sent.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns></returns>
        public void DelayedSendMessageAndDeleteAsync(IMessageChannel channel, TimeSpan? sendDelay = null, TimeSpan? deleteDelay = null,
            string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null) => _ = Task.Run(async () => {
                await Task.Delay(sendDelay ?? TimeSpan.Zero).ConfigureAwait(false);
                var msg = await channel.SendMessageAsync(text, isTTS, embed, options).ConfigureAwait(false);
                await Task.Delay(deleteDelay ?? DefaultTimeout).ConfigureAwait(false);
                await msg.DeleteAsync().ConfigureAwait(false);
            });

        /// <summary>
        /// Sends a file to a channel delayed and deletes it after another delay.
        /// </summary>
        /// <param name="channel">The target channel.</param>
        /// <param name="sendDelay">The time to wait before sending the file.</param>
        /// <param name="deleteDelay">The time to wait between sending and deleting the file.</param>
        /// <param name="filepath">The file path of the file.</param>
        /// <param name="text">The message to be sent.</param>
        /// <param name="isTTS">Whether the message should be read aloud by Discord or not.</param>
        /// <param name="embed">The <see cref="Embed"/> to be sent.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns></returns>
        public void DelayedSendFileAndDeleteAsync(IMessageChannel channel, TimeSpan? sendDelay = null, TimeSpan? deleteDelay = null,
            string filepath = null,
            string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null) => _ = Task.Run(async () => {
                await Task.Delay(sendDelay ?? TimeSpan.Zero).ConfigureAwait(false);
                var msg = await channel.SendFileAsync(filepath, text, isTTS, embed, options).ConfigureAwait(false);
                await Task.Delay(deleteDelay ?? DefaultTimeout).ConfigureAwait(false);
                await msg.DeleteAsync().ConfigureAwait(false);
            });

        /// <summary>
        /// Sends a file to a channel delayed and deletes it after another delay.
        /// </summary>
        /// <param name="channel">The target Channel.</param>
        /// <param name="sendDelay">The time to wait before sending the file.</param>
        /// <param name="deleteDelay">The time to wait between sending and deleting the file.</param>
        /// <param name="filestream">The <see cref="Stream"/> of the file to be sent.</param>
        /// <param name="filename">The name of the attachment.</param>
        /// <param name="text">The message to be sent.</param>
        /// <param name="isTTS">Whether the message should be read aloud by Discord or not.</param>
        /// <param name="embed">The <see cref="Embed"/> to be sent.</param>
        /// <param name="options">The options to be used when sending the request.</param>
        /// <returns></returns>
        public void DelayedSendFileAndDeleteAsync(IMessageChannel channel, TimeSpan? sendDelay = null, TimeSpan? deleteDelay = null,
            Stream filestream = null, string filename = null,
            string text = null, bool isTTS = false, Embed embed = null, RequestOptions options = null) => _ = Task.Run(async () => {
                await Task.Delay(sendDelay ?? TimeSpan.Zero).ConfigureAwait(false);
                var msg = await channel.SendFileAsync(filestream, filename, text, isTTS, embed, options).ConfigureAwait(false);
                await Task.Delay(deleteDelay ?? DefaultTimeout).ConfigureAwait(false);
                await msg.DeleteAsync().ConfigureAwait(false);
            });

        /// <summary>
        /// Deletes a message after a delay
        /// </summary>
        /// <param name="msg">The message to delete</param>
        /// <param name="deleteDelay">The time to wait before deleting the message</param>
        /// <returns></returns>
        public void DelayedDeleteMessageAsync(IMessage msg, TimeSpan? deleteDelay = null) => _ = Task.Run(async () => {
            await Task.Delay(deleteDelay ?? DefaultTimeout).ConfigureAwait(false);
            await msg.DeleteAsync().ConfigureAwait(false);
        });

        /// <summary>
        /// Retrieves the next incoming reaction that passes the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Predicate{SocketReaction}"/> which the reaction has to pass.</param>
        /// <param name="actions">The <see cref="Func{SocketReaction, Task}"/> which gets executed to all incoming reactions.</param>
        /// <param name="timeout">The time to wait before the methods retuns a timeout result.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<SocketReaction>> NextReactionAsync(Predicate<SocketReaction> filter = null, Func<SocketReaction, Task> actions = null,
            TimeSpan? timeout = null, CancellationToken token = default)
        {
            var filterCriteria = new Criteria<SocketReaction>();
            var funcActionCollection = new ActionCollection<SocketReaction>();

            if (filter != null) {
                filterCriteria.AddCriterion(new PredicateCriterion<SocketReaction>(filter));
            }
            if (actions != null) {
                funcActionCollection.AddAction(new FuncAction<SocketReaction>(actions, true, true));
            }

            return await NextReactionAsync(filterCriteria, funcActionCollection, timeout, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the next incoming reaction that passes the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Criteria{SocketReaction}"/> which the reaction has to pass.</param>
        /// <param name="actions">The <see cref="ActionCollection{SocketReaction}"/> which gets executed to incoming reactions.</param>
        /// <param name="timeout">The time to wait before the methods retuns a timeout result.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<SocketReaction>> NextReactionAsync(Criteria<SocketReaction> criteria = null, ActionCollection<SocketReaction> actions = null,
            TimeSpan? timeout = null, CancellationToken token = default)
        {
            criteria = criteria ?? new Criteria<SocketReaction>();
            actions = actions ?? new ActionCollection<SocketReaction>();

            var reactionSource = new TaskCompletionSource<InteractivityResult<SocketReaction>>();
            var cancelSource = new TaskCompletionSource<bool>();

            token.Register(() => cancelSource.SetResult(true));

            var reactionTask = reactionSource.Task;
            var cancelTask = cancelSource.Task;
            var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);

            async Task CheckReactionAsync(Cacheable<IUserMessage, ulong> _m, ISocketMessageChannel _c, SocketReaction reaction)
            {
                if (reaction.UserId == _client.CurrentUser.Id) { //Ignore own reactions
                    await actions.ApplyAsync(reaction, true).ConfigureAwait(false);
                    return;
                }

                if (await criteria.JudgeAsync(reaction).ConfigureAwait(false) == false) {
                    await actions.ApplyAsync(reaction, true).ConfigureAwait(false);
                    return;
                }

                await actions.ApplyAsync(reaction, false).ConfigureAwait(false);
                reactionSource.SetResult(new InteractivityResult<SocketReaction>(reaction, false, false));
            }

            _client.ReactionAdded += CheckReactionAsync;

            var result = await Task.WhenAny(reactionTask, cancelTask, timeoutTask).ConfigureAwait(false);

            _client.ReactionAdded -= CheckReactionAsync;

            return result == reactionTask
                ? await reactionTask.ConfigureAwait(false)
                : result == timeoutTask
                    ? new InteractivityResult<SocketReaction>(default, true, false)
                    : new InteractivityResult<SocketReaction>(default, false, true);
        }

        /// <summary>
        /// Retrieves the next incoming message that passes the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Predicate{SocketMessage}"/> which the message has to pass.</param>
        /// <param name="actions">The <see cref="Func{SocketMessage, Task}"/> which gets executed to all incoming messages.</param>
        /// <param name="timeout">The time to wait before the methods retuns a timeout result.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<SocketMessage>> NextMessageAsync(Predicate<SocketMessage> filter = null, Func<SocketMessage, Task> action = null,
            TimeSpan? timeout = null, CancellationToken token = default)
        {
            var filterCriteria = new Criteria<SocketMessage>();
            var funcActionCollection = new ActionCollection<SocketMessage>();

            if (filter != null) {
                filterCriteria.AddCriterion(new PredicateCriterion<SocketMessage>(filter));
            }

            if (action != null) {
                funcActionCollection.AddAction(new FuncAction<SocketMessage>(action, true, true));
            }

            return await NextMessageAsync(filterCriteria, funcActionCollection, timeout, token).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the next incoming message that passes the <paramref name="filter"/>.
        /// </summary>
        /// <param name="filter">The <see cref="Criteria{SocketMessage}"/> which the message has to pass.</param>
        /// <param name="actions">The <see cref="ActionCollection{SocketMessage}"/> which gets executed to incoming messages.</param>
        /// <param name="timeout">The time to wait before the methods retuns a timeout result.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the request.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<SocketMessage>> NextMessageAsync(Criteria<SocketMessage> criteria = null, ActionCollection<SocketMessage> actions = null,
            TimeSpan? timeout = null, CancellationToken token = default)
        {
            actions = actions ?? new ActionCollection<SocketMessage>();
            criteria = criteria ?? new Criteria<SocketMessage>();

            var messageSource = new TaskCompletionSource<InteractivityResult<SocketMessage>>();
            var cancelSource = new TaskCompletionSource<bool>();

            token.Register(() => cancelSource.SetResult(true));

            var messageTask = messageSource.Task;
            var cancelTask = cancelSource.Task;
            var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);

            async Task CheckMessageAsync(SocketMessage s)
            {
                if (s.Author.Id == _client.CurrentUser.Id) { //Ignore own messages
                    return;
                }

                if (await criteria.JudgeAsync(s).ConfigureAwait(false) == false) {
                    await actions.ApplyAsync(s, true).ConfigureAwait(false);
                    return;
                }

                await actions.ApplyAsync(s, false).ConfigureAwait(false);
                messageSource.SetResult(new InteractivityResult<SocketMessage>(s, false, false));
            }

            _client.MessageReceived += CheckMessageAsync;

            var result = await Task.WhenAny(messageTask, timeoutTask, cancelTask).ConfigureAwait(false);

            _client.MessageReceived -= CheckMessageAsync;

            return result == messageTask
                ? await messageTask.ConfigureAwait(false)
                : result == timeoutTask
                    ? new InteractivityResult<SocketMessage>(default, true, false)
                    : new InteractivityResult<SocketMessage>(default, false, true);
        }

        /// <summary>
        /// Waits for a user/users to confirm something.
        /// </summary>
        /// <param name="request">The <see cref="ConfirmationRequest"/> containing required informations about the confirmation.</param>
        /// <param name="timeout">The time before the confirmation returns a timeout result.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the confirmation.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<bool>> GetUserConfirmationAsync(ConfirmationRequest request,
            TimeSpan? timeout = null, CancellationToken token = default)
        {
            var confirmationSource = new TaskCompletionSource<InteractivityResult<bool>>();
            var cancelSource = new TaskCompletionSource<bool>();

            token.Register(() => cancelSource.SetResult(true));

            var confirmationTask = confirmationSource.Task;
            var cancelTask = cancelSource.Task;
            var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);

            async Task CheckReactionAsync(Cacheable<IUserMessage, ulong> _m, ISocketMessageChannel _c, SocketReaction reaction)
            {
                if (reaction.UserId == _client.CurrentUser.Id) {
                    return;
                }

                if (reaction.MessageId != request.Message.Id) {
                    return;
                }

                if (await request.GetCriterions().JudgeAsync(reaction).ConfigureAwait(false) == false) {
                    await request.GetActions().ApplyAsync(reaction, true).ConfigureAwait(false);
                    return;
                }

                await request.GetActions().ApplyAsync(reaction, false).ConfigureAwait(false);

                var action = request.Appearance.ParseAction(reaction.Emote);

                if (action == ConfirmationAction.Decline) {
                    confirmationSource.SetResult(new InteractivityResult<bool>(false, false, true));
                    return;
                }
                if (action == ConfirmationAction.Confirm) {
                    confirmationSource.SetResult(new InteractivityResult<bool>(true, false, false));
                    return;
                }
            }

            _client.ReactionAdded += CheckReactionAsync;

            await request.Message.AddReactionsAsync(request.Appearance.Emotes).ConfigureAwait(false);
            var task_result = await Task.WhenAny(confirmationTask, cancelTask, timeoutTask).ConfigureAwait(false);

            _client.ReactionAdded -= CheckReactionAsync;

            var result = task_result == confirmationTask
                    ? await confirmationTask.ConfigureAwait(false)
                    : task_result == timeoutTask
                        ? new InteractivityResult<bool>(default, true, false)
                        : new InteractivityResult<bool>(default, false, true);

            if (result.IsCancelled == true) {
                await request.Message.ModifyAsync(x => x.Embed = request.Appearance.CancelledEmbed).ConfigureAwait(false);
            }

            if (result.IsTimeouted == true) {
                await request.Message.ModifyAsync(x => x.Embed = request.Appearance.TimeoutedEmbed).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// Sends a user selection into a discord channel.
        /// </summary>
        /// <typeparam name="T">The type of values the user can select from.</typeparam>
        /// <typeparam name="TAppearance">The appearance class of the selection</typeparam>
        /// <param name="selection">The selection to send in the channel.</param>
        /// <param name="channel">The <see cref="IMessageChannel"/> to send the selection to.</param>
        /// <param name="timeout">The time until the selection times out.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the selection.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<T>> SendSelectionAsync<T, TAppearance>(Selection<T, SocketMessage, TAppearance> selection, IMessageChannel channel,
            TimeSpan? timeout = null, CancellationToken token = default) where TAppearance : SelectionAppearance
        {
            var selectionSource = new TaskCompletionSource<InteractivityResult<T>>();
            var cancelSource = new TaskCompletionSource<bool>();

            token.Register(() => cancelSource.SetResult(true));

            var selectionTask = selectionSource.Task;
            var cancelTask = cancelSource.Task;
            var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);

            var msg = await channel.SendMessageAsync(embed: selection.SelectionEmbed).ConfigureAwait(false);

            async Task CheckMessageAsync(SocketMessage s)
            {
                if (s.Channel != channel) {
                    return;
                }
                if (s.Author.Id == _client.CurrentUser.Id) {
                    return;
                }
                if (await selection.HandleResponseAsync(_client, msg, s).ConfigureAwait(false) == false) {
                    return;
                }

                var sResult = await selection.ParseAsync(s).ConfigureAwait(false);
                if (sResult.IsSpecified == false) {
                    return;
                }

                selectionSource.SetResult(sResult.Value);
                await selection.RunActionsAsync(_client, msg, s, true).ConfigureAwait(false);
            }

            _client.MessageReceived += CheckMessageAsync;
            await selection.InitializeMessageAsync(msg);
            var task_result = await Task.WhenAny(selectionTask, timeoutTask, cancelTask).ConfigureAwait(false);
            _client.MessageReceived -= CheckMessageAsync;

            var result = task_result == selectionTask
                                ? await selectionTask.ConfigureAwait(false)
                                : task_result == timeoutTask
                                    ? new InteractivityResult<T>(default, true, false)
                                    : new InteractivityResult<T>(default, false, true);

            if (selection.Appearance.Deletion.HasFlag(DeletionOption.AfterCapturedContext) == true) {
                await msg.DeleteAsync().ConfigureAwait(false);
            }
            else if (result.IsCancelled == true) {
                await msg.ModifyAsync(x => x.Embed = selection.Appearance.CancelledEmbed).ConfigureAwait(false);
            }
            else if (result.IsTimeouted == true) {
                await msg.ModifyAsync(x => x.Embed = selection.Appearance.TimeoutedEmbed).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// Sends a user selection into a discord channel.
        /// </summary>
        /// <typeparam name="T">The type of values the user can select from.</typeparam>
        /// <typeparam name="TAppearance">The appearance class of the selection</typeparam>
        /// <param name="selection">The selection to send in the channel.</param>
        /// <param name="channel">The <see cref="IMessageChannel"/> to send the selection to.</param>
        /// <param name="timeout">The time until the selection times out.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the selection.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<T>> SendSelectionAsync<T, TAppearance>(Selection<T, SocketReaction, TAppearance> selection, IMessageChannel channel,
            TimeSpan? timeout = null, CancellationToken token = default) where TAppearance : SelectionAppearance
        {
            var selectionSource = new TaskCompletionSource<InteractivityResult<T>>();
            var cancelSource = new TaskCompletionSource<bool>();

            token.Register(() => cancelSource.SetResult(true));

            var selectionTask = selectionSource.Task;
            var cancelTask = cancelSource.Task;
            var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);

            var msg = await channel.SendMessageAsync(embed: selection.SelectionEmbed).ConfigureAwait(false);

            async Task CheckReactionAsync(Cacheable<IUserMessage, ulong> m, ISocketMessageChannel c, SocketReaction r)
            {
                if (m.Id != msg.Id) {
                    return;
                }
                if (r.UserId == _client.CurrentUser.Id) {
                    return;
                }
                if (await selection.HandleResponseAsync(_client, msg, r).ConfigureAwait(false) == false) {
                    return;
                }

                var sResult = await selection.ParseAsync(r).ConfigureAwait(false);
                if (sResult.IsSpecified == false) {
                    return;
                }

                selectionSource.SetResult(sResult.Value);
                await selection.RunActionsAsync(_client, msg, r, true).ConfigureAwait(false);
            }

            _client.ReactionAdded += CheckReactionAsync;
            await selection.InitializeMessageAsync(msg);
            var task_result = await Task.WhenAny(selectionTask, timeoutTask, cancelTask).ConfigureAwait(false);
            _client.ReactionAdded -= CheckReactionAsync;

            var result = task_result == selectionTask
                                ? await selectionTask.ConfigureAwait(false)
                                : task_result == timeoutTask
                                    ? new InteractivityResult<T>(default, true, false)
                                    : new InteractivityResult<T>(default, false, true);

            if (selection.Appearance.Deletion.HasFlag(DeletionOption.AfterCapturedContext) == true) {
                await msg.DeleteAsync().ConfigureAwait(false);
            }
            else if (result.IsCancelled == true) {
                await msg.ModifyAsync(x => x.Embed = selection.Appearance.CancelledEmbed).ConfigureAwait(false);
            }
            else if (result.IsTimeouted == true) {
                await msg.ModifyAsync(x => x.Embed = selection.Appearance.TimeoutedEmbed).ConfigureAwait(false);
            }

            return result;
        }

        /// <summary>
        /// Sends a page with multiple pages which the user can move through via reactions.
        /// </summary>
        /// <param name="paginator">The paginator to send.</param>
        /// <param name="channel">The <see cref="IMessageChannel"/> to send the paginator to.</param>
        /// <param name="timeout">The time until the paginator times out.</param>
        /// <param name="token">The <see cref="CancellationToken"/> to cancel the paginator.</param>
        /// <returns></returns>
        public async Task<InteractivityResult<object>> SendPaginatorAsync(Paginator paginator, IMessageChannel channel,
            TimeSpan? timeout = null, CancellationToken token = default)
        {
            var resultSource = new TaskCompletionSource<InteractivityResult<object>>();
            var cancelSource = new TaskCompletionSource<bool>();

            token.Register(() => cancelSource.SetResult(true));

            var resultTask = resultSource.Task;
            var cancelTask = cancelSource.Task;
            var timeoutTask = Task.Delay(timeout ?? DefaultTimeout);

            var msg = await channel.SendMessageAsync(embed: paginator.CurrentPage).ConfigureAwait(false);

            async Task CheckReactionAsync(Cacheable<IUserMessage, ulong> cachedMessage, ISocketMessageChannel _c, SocketReaction reaction)
            {
                if (reaction.UserId == _client.CurrentUser.Id) {
                    return;
                }

                if (reaction.MessageId != msg.Id) {
                    return;
                }

                if (await paginator.GetCriteria().JudgeAsync(reaction).ConfigureAwait(false) == false) {
                    await paginator.GetActions().ApplyAsync(reaction, true).ConfigureAwait(false);
                    return;
                }

                await paginator.GetActions().ApplyAsync(reaction, false).ConfigureAwait(false);

                var action = paginator.Appearance.ParseAction(reaction.Emote);

                if (action == PaginatorAction.Exit) {
                    resultSource.SetResult(new InteractivityResult<object>(default, false, true));
                    return;
                }
                if (action != PaginatorAction.None) {
                    paginator.ApplyAction(action, out bool pageChanged);

                    if (pageChanged == true) {
                        await msg.ModifyAsync(x => x.Embed = paginator.CurrentPage).ConfigureAwait(false);
                    }
                }
            }

            _client.ReactionAdded += CheckReactionAsync;

            await msg.AddReactionsAsync(paginator.Appearance.Emotes).ConfigureAwait(false);
            var task_result = await Task.WhenAny(resultTask, cancelTask, timeoutTask).ConfigureAwait(false);

            _client.ReactionAdded -= CheckReactionAsync;

            await msg.RemoveAllReactionsAsync().ConfigureAwait(false);

            var result = task_result == resultTask
                                                ? await resultTask.ConfigureAwait(false)
                                                : task_result == timeoutTask
                                                    ? new InteractivityResult<object>(default, true, false)
                                                    : new InteractivityResult<object>(default, false, true);

            if (paginator.Appearance.Deletion.HasFlag(DeletionOption.AfterCapturedContext) == true) {
                await msg.DeleteAsync();
            }
            else if (result.IsCancelled == true) {
                await msg.ModifyAsync(x => x.Embed = paginator.Appearance.CancelledEmbed).ConfigureAwait(false);
            }
            else if (result.IsTimeouted == true) {
                await msg.ModifyAsync(x => x.Embed = paginator.Appearance.TimeoutedEmbed).ConfigureAwait(false);
            }

            return result;
        }
    }
}