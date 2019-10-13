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
    /// The <see cref="PageBuilder"/> is used to create a <see cref="Page"/>.
    /// </summary>
    public class PageBuilder
    {
        /// <summary>
        /// Gets or sets the Text of the <see cref="PageBuilder"/>
        /// </summary>
        public string Text { get; set; } = null;

        /// <summary>
        /// Gets or sets the Color of the <see cref="PageBuilder"/>.
        /// </summary>
        public sys.Color? Color { get; set; } = null;

        /// <summary>
        /// Gets or sets the Description of the <see cref="PageBuilder"/>.
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// Gets or sets the Title of the <see cref="PageBuilder"/>.
        /// </summary>
        public string Title { get; set; } = null;

        /// <summary>
        /// Gets or sets the Thumbnailurl of the <see cref="PageBuilder"/>.
        /// </summary>
        public string ThumbnailUrl { get; set; } = null;

        /// <summary>
        /// Gets or sets the ImageUrl of the <see cref="PageBuilder"/>.
        /// </summary>
        public string ImageUrl { get; set; } = null;

        /// <summary>
        /// Gets or sets the Fields of the <see cref="PageBuilder"/>.
        /// </summary>
        public List<EmbedFieldBuilder> Fields { get; set; } = new List<EmbedFieldBuilder>();

        /// <summary>
        /// Creates a new <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="color">The Embed color of the <see cref="PageBuilder"/>.</param>
        /// <param name="description">The Embed description of the <see cref="PageBuilder"/>.</param>
        /// <param name="title">The Embed title of the <see cref="PageBuilder"/>.</param>
        /// <param name="thumbnailUrl">The Embed thumbnailurl of the <see cref="PageBuilder"/>.</param>
        /// <param name="imageUrl">The Embed imageUrl of the <see cref="PageBuilder"/>.</param>
        /// <param name="fields">The Embed fields of the <see cref="PageBuilder"/>.</param>
        

        /// <summary>
        /// Creates a new <see cref="PageBuilder"/> from an <see cref="Embed"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public static PageBuilder FromEmbed(Embed embed)
            => new PageBuilder()
                .WithColor((sys.Color)embed.Color)
                .WithDescription(embed.Description)
                .WithTitle(embed.Title)
                .WithThumbnailUrl(embed.Thumbnail?.Url)
                .WithImageUrl(embed.Image?.Url)
                .WithFields(embed.Fields.Select(x => x.ToBuilder()));

        /// <summary>
        /// Creates a new <see cref="PageBuilder"/> from an <see cref="EmbedBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static PageBuilder FromEmbedBuilder(EmbedBuilder builder)
            => new PageBuilder()
                .WithColor((sys.Color) builder.Color)
                .WithDescription(builder.Description)
                .WithTitle(builder.Title)
                .WithThumbnailUrl(builder.ThumbnailUrl)
                .WithImageUrl(builder.ImageUrl)
                .WithFields(builder.Fields);

        /// <summary>
        /// Sets the Text of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public PageBuilder WithText(string text)
        {
            Text = text;
            return this;
        }

        /// <summary>
        /// Sets the Color of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public PageBuilder WithColor(sys.Color color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// Sets the Description of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public PageBuilder WithDescription(string description)
        {
            Description = description;
            return this;
        }

        /// <summary>
        /// Sets the Title of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public PageBuilder WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Sets the Thumbnailurl of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="thumbnailUrl"></param>
        /// <returns></returns>
        public PageBuilder WithThumbnailUrl(string thumbnailUrl)
        {
            ThumbnailUrl = thumbnailUrl;
            return this;
        }

        /// <summary>
        /// Sets the ImageUrl of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public PageBuilder WithImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            return this;
        }

        /// <summary>
        /// Sets the Fields of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public PageBuilder WithFields(params EmbedFieldBuilder[] fields)
        {
            Fields = fields?.ToList();
            return this;
        }

        /// <summary>
        /// Sets the Fields of the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        public PageBuilder WithFields(IEnumerable<EmbedFieldBuilder> fields)
        {
            Fields = fields?.ToList();
            return this;
        }

        /// <summary>
        /// Adds a field to the <see cref="PageBuilder"/>.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="value">The value of the field.</param>
        /// <param name="isInline">Whether the field is Inline.</param>
        /// <returns></returns>
        public PageBuilder AddField(string name, object value, bool isInline = false)
        {
            if (Fields.Count == 24)
            {
                throw new InvalidOperationException("The field limit is 24!");
            }

            Fields.Add(new EmbedFieldBuilder()
            {
                Name = name,
                Value = value,
                IsInline = isInline
            });
            return this;
        }

        public Page Build(string defaultText = null, sys.Color? defaultColor = null,
            string defaultDescription = null, string defaultTitle = null, string defaultThumbnailUrl = null, string defaultImageUrl = null,
            List<EmbedFieldBuilder> defaultFields = null, EmbedFooterBuilder footer = null)
            => new Page(Text ?? defaultText,
                Color ?? defaultColor,
                Description ?? defaultDescription,
                Title ?? defaultTitle,
                ThumbnailUrl ?? defaultThumbnailUrl,
                ImageUrl ?? defaultImageUrl,
                Fields ?? defaultFields,
                footer);

        /// <summary>
        /// Creates an <see cref="Embed"/> from current <see cref="PageBuilder"/>.
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