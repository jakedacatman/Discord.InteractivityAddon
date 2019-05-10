using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// The <see cref="PaginatorBuilder"/> is used to create a <see cref="Paginator"/>.
    /// </summary>
    public sealed class PaginatorBuilder
    {
        /// <summary>
        /// Gets or sets the Pages which are used by the <see cref="Paginator"/>.
        /// </summary>
        public List<Page> Pages { get; set; } = new List<Page>();

        /// <summary>
        /// Gets or sets the appearance of the <see cref="Paginator"/>.
        /// </summary>
        public PaginatorAppearanceBuilder Appearance { get; set; } = PaginatorAppearanceBuilder.Default;

        /// <summary>
        /// Determites whether everyone can interact with the <see cref="Paginator"/>.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;

        /// <summary>
        /// Gets or sets which users can interact with the <see cref="Paginator"/>.
        /// </summary>
        public List<SocketUser> Users { get; set; } = new List<SocketUser>();

        /// <summary>
        /// Gets or sets the default Embed color for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        public Color? Color { get; set; } = Discord.Color.Green;

        /// <summary>
        /// Gets or sets the default Embed description for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// Gets or sets the default Embed title for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        public string Title { get; set; } = null;

        /// <summary>
        /// Gets or sets the default Embed thumbnailurl for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        public string ThumbnailUrl { get; set; } = null;

        /// <summary>
        /// Gets or sets the default Embed imageUrl for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        public string ImageUrl { get; set; } = null;

        /// <summary>
        /// Gets or sets the default Embed fields for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        public List<EmbedFieldBuilder> Fields { get; set; } = new List<EmbedFieldBuilder>();

        /// <summary>
        /// Gets or sets the footer of the <see cref="Paginator"/>.
        /// </summary>
        public PaginatorFooter Footer { get; set; } = PaginatorFooter.PageNumber;

        /// <summary>
        /// Build the <see cref="PaginatorBuilder"/> to get a <see cref="Paginator"/> instance.
        /// </summary>
        public Paginator Build(int startPage = 0)
        {
            if (Pages.Count == 0) {
                throw new InvalidOperationException("Your Builder needs at least one page!");
            }

            var embedPages = new List<Embed>();

            foreach (var page in Pages) {
                embedPages.Add(page.Build(this));
            }

            return new Paginator(embedPages, 
                                 startPage, 
                                 Users.ToArray().ToList(), 
                                 Appearance.Build());
        }

        /// <summary>
        /// Adds pages to the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="pages"></param>
        /// <returns></returns>
        public PaginatorBuilder WithPages(params Page[] pages)
        {
            Pages.AddRange(pages);
            return this;
        }

        /// <summary>
        /// Adds embeds to the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="embeds"></param>
        /// <returns></returns>
        public PaginatorBuilder WithEmbeds(params Embed[] embeds)
        {
            foreach (var embed in embeds) {
                Pages.Add(Page.FromEmbed(embed));
            }
            return this;
        }

        /// <summary>
        /// Add <see cref="EmbedField"/>s to the default Embed fields for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public PaginatorBuilder WithFields(params EmbedFieldBuilder[] fields)
        {
            Fields.AddRange(fields);
            return this;
        }

        /// <summary>
        /// Sets the appearance of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="appearance"></param>
        /// <returns></returns>
        public PaginatorBuilder WithAppearance(PaginatorAppearanceBuilder appearance)
        {
            Appearance = appearance;
            return this;
        }

        /// <summary>
        /// Sets which users can interact with the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public PaginatorBuilder WithUsers(params SocketUser[] users)
        {
            Users = users.ToList();
            return this;
        }

        /// <summary>
        /// Sets the default Embed color for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public PaginatorBuilder WithColor(Color color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// Sets the default Embed description for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public PaginatorBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the default Embed title for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public PaginatorBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the default Embed imageUrl for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="thumbnailUrl"></param>
        /// <returns></returns>
        public PaginatorBuilder WithThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        /// <summary>
        /// Sets the default Embed imageUrl for all Pages of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public PaginatorBuilder WithImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        /// <summary>
        /// Sets the footer of the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="footer"></param>
        /// <returns></returns>
        public PaginatorBuilder WithPaginatorFooter(PaginatorFooter footer)
        {
            Footer = footer;
            return this;
        }
    }
}
