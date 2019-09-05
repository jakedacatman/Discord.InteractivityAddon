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
        /// Gets or sets the Color of the <see cref="Page"/>.
        /// </summary>
        public sys.Color? Color { get; set; }

        /// <summary>
        /// Gets or sets the Description of the <see cref="Page"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Title of the <see cref="Page"/>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Thumbnailurl of the <see cref="Page"/>.
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the ImageUrl of the <see cref="Page"/>.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the Fields of the <see cref="Page"/>.
        /// </summary>
        public List<EmbedFieldBuilder> Fields { get; set; }

        /// <summary>
        /// Creates a new <see cref="Page"/>.
        /// </summary>
        /// <param name="color">The Embed color of the <see cref="Page"/>.</param>
        /// <param name="description">The Embed description of the <see cref="Page"/>.</param>
        /// <param name="title">The Embed title of the <see cref="Page"/>.</param>
        /// <param name="thumbnailUrl">The Embed thumbnailurl of the <see cref="Page"/>.</param>
        /// <param name="imageUrl">The Embed imageUrl of the <see cref="Page"/>.</param>
        /// <param name="fields">The Embed fields of the <see cref="Page"/>.</param>
        public Page(sys.Color? color = null, string description = null, string title = null, string thumbnailUrl = null, string imageUrl = null, List<EmbedFieldBuilder> fields = null)
        {
            Color = color;
            Description = description;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            ImageUrl = imageUrl;
            Fields = fields;
        }

        /// <summary>
        /// Creates a new <see cref="Page"/> from an <see cref="Embed"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public static Page FromEmbed(Embed embed) 
            => new Page(embed.Color,
                embed.Description,
                embed.Title,
                embed.Thumbnail?.Url,
                embed.Image?.Url,
                embed.Fields.Select(x => x.ToBuilder()).ToList());

        /// <summary>
        /// Creates a new <see cref="Page"/> from an <see cref="EmbedBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static Page FromEmbedBuilder(EmbedBuilder builder) 
            => new Page(builder.Color,
                builder.Description,
                builder.Title,
                builder.ThumbnailUrl,
                builder.ImageUrl,
                builder.Fields);

        /// <summary>
        /// Sets the Color of the <see cref="Page"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public Page WithColor(sys.Color color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// Sets the Description of the <see cref="Page"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public Page WithDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the Title of the <see cref="Page"/>.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Page WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the Thumbnailurl of the <see cref="Page"/>.
        /// </summary>
        /// <param name="thumbnailUrl"></param>
        /// <returns></returns>
        public Page SetThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        /// <summary>
        /// Sets the ImageUrl of the <see cref="Page"/>.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public Page SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        /// <summary>
        /// Adds a field to the <see cref="Page"/>.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="isInline">Whether the field is Inline.</param>
        /// <returns></returns>
        public Page AddField(string name, object value, bool isInline = false)
        {
            if (Fields.Count == 24)
            {
                throw new InvalidOperationException("The field limit is 19!");
            }

            Fields.Add(new EmbedFieldBuilder()
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return this;
        }

        /// <summary>
        /// Creates an <see cref="Embed"/> from current <see cref="Page"/>.
        /// </summary>
        /// <param name="builder">The <see cref="PaginatorBuilder"/> to take default values from.</param>
        /// <returns></returns>
        internal Embed Build(PaginatorBuilder builder)
        {
            var embedBuilder = new EmbedBuilder()
            {
                Color = (Color?) Color ?? builder.Color,
                Description = Description ?? builder.Description,
                Title = Title ?? builder.Title,
                ThumbnailUrl = ThumbnailUrl ?? builder.ThumbnailUrl,
                ImageUrl = ImageUrl ?? builder.ImageUrl,
                Fields = Fields ?? builder.Fields,
            };

            var footerBuilder = new EmbedFooterBuilder();

            if (builder.Footer.HasFlag(PaginatorFooter.Users))
            {
                if (builder.IsUserRestricted == false)
                {
                    footerBuilder.Text += "Interactors : Everyone";
                }
                else
                {
                    if (builder.Users.Count == 1)
                    {
                        footerBuilder.IconUrl = builder.Users.First().GetAvatarUrl();
                        footerBuilder.Text += $"Interactors : {builder.Users.First()}";
                    }
                    else
                    {
                        var interactorBuilder = new StringBuilder().Append("Interactors : ");

                        foreach (var user in builder.Users)
                        {
                            interactorBuilder.Append($"{user}, ");
                        }

                        interactorBuilder.Remove(interactorBuilder.Length - 2, 2);

                        footerBuilder.Text += interactorBuilder.ToString();
                    }
                }
            }

            if (builder.Footer.HasFlag(PaginatorFooter.PageNumber))
            {
                footerBuilder.Text += $"\nPage {builder.Pages.FindIndex(x => x == this) + 1}/{builder.Pages.Count}";
            }

            if (builder.Footer.HasFlag(PaginatorFooter.None))
            {
                footerBuilder = null;
            }

            embedBuilder.WithFooter(footerBuilder);

            return embedBuilder.Build();
        }
    }
}