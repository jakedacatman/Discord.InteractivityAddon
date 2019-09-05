using System.Collections.Generic;
using sys = System.Drawing;
using Discord;
using System;
using InteractivityAddon.Pagination;
using System.Linq;
using System.Text;
using InteractivityAddon.Extensions;

namespace InteractivityAddon
{
    /// <summary>
    /// Represents a page
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Gets the text of the <see cref="Page"/>.
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Gets the Embed of the <see cref="Page"/>.
        /// </summary>
        public Embed Embed { get; }

        /// <summary>
        /// Creates a new <see cref="Page"/>.
        /// </summary>
        /// <param name="color">The Embed color of the <see cref="Page"/>.</param>
        /// <param name="description">The Embed description of the <see cref="Page"/>.</param>
        /// <param name="title">The Embed title of the <see cref="Page"/>.</param>
        /// <param name="thumbnailUrl">The Embed thumbnailurl of the <see cref="Page"/>.</param>
        /// <param name="imageUrl">The Embed imageUrl of the <see cref="Page"/>.</param>
        /// <param name="fields">The Embed fields of the <see cref="Page"/>.</param>
        internal Page(string text = null, sys.Color? color = null, 
            string description = null, string title = null, string thumbnailUrl = null, string imageUrl = null, 
            List<EmbedFieldBuilder> fields = null, EmbedFooterBuilder footer = null)
        {
            Text = text;
            fields ??= new List<EmbedFieldBuilder>();
            footer ??= new EmbedFooterBuilder();

            if (color == null &&
                description == null &&
                title == null &&
                thumbnailUrl == null &&
                imageUrl == null &&
                fields.Count == 0 &&
                footer.IconUrl == null &&
                footer.Text == null)
            {
                return;
            }

            Embed = new EmbedBuilder()
            {
                Color = (Color?) color,
                Description = description,
                Title = title,
                ThumbnailUrl = thumbnailUrl,
                ImageUrl = imageUrl,
                Fields = fields ?? new List<EmbedFieldBuilder>(),
                Footer = footer ?? new EmbedFooterBuilder()
            }
            .Build();
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> from an <see cref="Embed"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public static Page FromEmbed(Embed embed)
            => new Page(color: embed.Color,
                description: embed.Description,
                title: embed.Title,
                thumbnailUrl: embed.Thumbnail?.Url,
                imageUrl: embed.Image?.Url,
                fields: embed.Fields.Select(x => x.ToBuilder()).ToList());

        /// <summary>
        /// Creates a new <see cref="Page"/> from an <see cref="EmbedBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Page FromEmbedBuilder(EmbedBuilder builder)
            => new Page(color: builder.Color,
                description: builder.Description,
                title: builder.Title,
                thumbnailUrl: builder.ThumbnailUrl,
                imageUrl: builder.ImageUrl,
                fields: builder.Fields);
    }
}