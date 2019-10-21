using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using Interactivity.Extensions;

namespace Interactivity.Pagination
{
    /// <summary>
    /// Represents a builder class for making a <see cref="StaticPaginator"/>.
    /// </summary>
    public sealed class StaticPaginatorBuilder : PaginatorBuilder
    {
        /// <summary>
        /// Gets or sets the pages of the <see cref="Paginator"/>.
        /// </summary>
        public IList<PageBuilder> Pages { get; set; }

        public override Paginator Build(int startPage = 0)
        {
            for (int i = 0; i < Pages.Count; i++)
            {
                Pages[i].WithPaginatorFooter(Footer, i, Pages.Count - 1, Users);
            }

            return new StaticPaginator(Users?.ToReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Users)),
                                       Emotes?.ToReadOnlyDictionary() ?? throw new ArgumentNullException(nameof(Emotes)),
                                       CancelledEmbed?.Build() ?? throw new ArgumentNullException(nameof(CancelledEmbed)),
                                       TimeoutedEmbed?.Build() ?? throw new ArgumentNullException(nameof(TimeoutedEmbed)),
                                       Deletion,
                                       Pages?.Select(x => x.Build()).ToReadOnlyCollection() ?? throw new ArgumentNullException(nameof(Pages)),
                                       startPage);
        }

        #region WithValue
        public new StaticPaginatorBuilder WithUsers(params SocketUser[] users) => base.WithUsers(users) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder WithUsers(IEnumerable<SocketUser> users) => base.WithUsers(users) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder WithEmotes(Dictionary<IEmote, PaginatorAction> emotes) => base.WithEmotes(emotes) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder AddEmote(PaginatorAction action, IEmote emote) => base.AddEmote(action, emote) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder WithCancelledEmbed(EmbedBuilder embed) => base.WithCancelledEmbed(embed) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder WithTimoutedEmbed(EmbedBuilder embed) => base.WithTimoutedEmbed(embed) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder WithDeletion(DeletionOptions deletion) => base.WithDeletion(deletion) as StaticPaginatorBuilder;
        public new StaticPaginatorBuilder WithFooter(PaginatorFooter footer) => base.WithFooter(footer) as StaticPaginatorBuilder;
        public StaticPaginatorBuilder WithDefaultEmotes()
        {
            Emotes.Add(new Emoji("◀"), PaginatorAction.Backward);
            Emotes.Add(new Emoji("▶"), PaginatorAction.Forward);
            Emotes.Add(new Emoji("⏮"), PaginatorAction.SkipToStart);
            Emotes.Add(new Emoji("⏭"), PaginatorAction.SkipToEnd);
            Emotes.Add(new Emoji("🛑"), PaginatorAction.Exit);

            return this;
        }
        public StaticPaginatorBuilder WithPages(params PageBuilder[] pages)
        {
            Pages = pages.ToList();
            return this;
        }
        public StaticPaginatorBuilder WithPages(IEnumerable<PageBuilder> pages)
        {
            Pages = pages.ToList();
            return this;
        }
        public StaticPaginatorBuilder AddPage(PageBuilder page)
        {
            Pages.Add(page);
            return this;
        }
        #endregion
    }
}
