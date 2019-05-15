using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using InteractivityAddon.Extensions;

namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Represents a page of a <see cref="Paginator"/>.
    /// </summary>
    public sealed class Page
    {
        /// <summary>
        /// Gets or sets the Embed color of the <see cref="Page"/>.
        /// </summary>
        public Color? Color { get; set; }

        /// <summary>
        /// Gets or sets the Embed description of the <see cref="Page"/>.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Embed title of the <see cref="Page"/>.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the Embed thumbnailurl of the <see cref="Page"/>.
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Gets or sets the Embed imageUrl of the <see cref="Page"/>.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the Embed fields of the <see cref="Page"/>.
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
        public Page(Color? color = null, string description = null, string title = null, string thumbnailUrl = null, string imageUrl = null, List<EmbedFieldBuilder> fields = null)
        {
            Color = color;
            Description = description;
            Title = title;
            ThumbnailUrl = thumbnailUrl;
            ImageUrl = imageUrl;
            Fields = fields;
        }

        /// <summary>
        /// Creates a <see cref="Page"/> from an <see cref="Embed"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public static Page FromEmbed(Embed embed)
        {
            var fieldBuilders = new List<EmbedFieldBuilder>();

            foreach (var field in embed.Fields) {
                fieldBuilders.Add(field.ToBuilder());
            }

            return new Page(
            color: embed.Color,
            description: embed.Description,
            title: embed.Title,
            thumbnailUrl: embed.Thumbnail?.Url,
            imageUrl: embed.Image?.Url,
            fields: fieldBuilders);
        }

        /// <summary>
        /// Creates an <see cref="Embed"/> from current <see cref="Page"/>.
        /// </summary>
        /// <param name="builder">The <see cref="PaginatorBuilder"/> to take default values from.</param>
        /// <returns></returns>
        public Embed Build(PaginatorBuilder builder)
        {
            var embedBuilder = new EmbedBuilder() {
                Color = Color ?? builder.Color,
                Description = Description ?? builder.Description,
                Title = Title ?? builder.Title,
                ThumbnailUrl = ThumbnailUrl ?? builder.ThumbnailUrl,
                ImageUrl = ImageUrl ?? builder.ImageUrl,
                Fields = Fields ?? builder.Fields,
            };

            var footerBuilder = new EmbedFooterBuilder();

            if (builder.Footer.HasFlag(PaginatorFooter.Users)) {
                if (builder.IsUserRestricted == false) {
                    footerBuilder.Text += "Interactors : Everyone";
                }
                else {
                    if (builder.Users.Count == 1) {
                        footerBuilder.IconUrl = builder.Users.First().GetAvatarUrl();
                        footerBuilder.Text += $"Interactors : {builder.Users.First()}";
                    }
                    else {
                        var interactorBuilder = new StringBuilder().Append("Interactors : ");
                        
                        foreach(var user in builder.Users) {
                            interactorBuilder.Append($"{user}, ");
                        }

                        interactorBuilder.Remove(interactorBuilder.Length - 2, 2);

                        footerBuilder.Text += interactorBuilder.ToString();
                    }
                }
            }

            if (builder.Footer.HasFlag(PaginatorFooter.PageNumber)) {
                footerBuilder.Text += $"\nPage {builder.Pages.FindIndex(x => x == this) + 1}/{builder.Pages.Count}";
            }

            if (builder.Footer.HasFlag(PaginatorFooter.None)) {
                footerBuilder = null;
            }

            embedBuilder.WithFooter(footerBuilder);

            return embedBuilder.Build();
        }
    }
}
