using System;
using System.Collections.Generic;
using System.Linq;
using System.ListOfStringExtensions;
using System.Text;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// The <see cref="SelectionBuilder{T}"/> is used to create a <see cref="Selection{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class SelectionBuilder<T>
    {
        /// <summary>
        /// Gets or sets the values to select from in the <see cref="Selection{T}"/>.
        /// </summary>
        public List<T> Values { get; set; } = new List<T>();

        /// <summary>
        /// Gets or sets the <see cref="Func{T, string}"/> to convert the values to a <see cref="string"/> in the <see cref="Selection{T}"/>.
        /// </summary>
        public Func<T, string> StringConverter { get; set; } = x => x.ToString();

        /// <summary>
        /// Gets or sets the appearance of the <see cref="Selection{T}"/>.
        /// </summary>
        public SelectionAppearance Appearance { get; set; } = SelectionAppearance.Default;

        /// <summary>
        ///  Determites whether everyone can interact with the <see cref="Selection{T}"/>.
        /// </summary>
        public bool IsUserRestricted => Users.Count > 0;

        /// <summary>
        /// Gets or sets which users can interact with the <see cref="Selection{T}"/>.
        /// </summary>
        public List<SocketUser> Users { get; set; } = new List<SocketUser>();

        /// <summary>
        /// Gets or sets whether the <see cref="Selection{T}"/> allows cancellation.
        /// </summary>
        public bool AllowCancel { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the <see cref="Selection{T}"/> is case sensitive.
        /// </summary>
        public bool IsCaseSensitive { get; set; } = false;

        /// <summary>
        /// Gets or sets the title of the <see cref="Selection{T}"/>.
        /// </summary>
        public string Title { get; set; } = "Select one of these";

        /// <summary>
        /// Gets or sets the embed which the <see cref="Selection{T}"/> is made of.
        /// </summary>
        public EmbedBuilder SelectionEmbed { get; set; } = new EmbedBuilder().WithColor(Color.Blue);


        /// <summary>
        /// Build the <see cref="SelectionBuilder{T}"/> to get a <see cref="Selection{T}"/> instance.
        /// </summary>
        /// <returns></returns>
        public Selection<T> Build()
        {
            if (Values.Count == 0) {
                throw new InvalidOperationException("Your Selection needs at least one value");
            }

            var sBuilder = new StringBuilder();
            var possibilities = new List<string>();

            for (int i = 0; i < Values.Count; i++) {
                string possibility = StringConverter.Invoke(Values[i]);
                sBuilder.Append($"#{i + 1} - {possibility}\n");
                possibilities.Add($"{i + 1}");
                possibilities.Add($"#{i + 1}");
                possibilities.Add(possibility);
                possibilities.Add($"#{i + 1} - {possibility}");
            }
            if (AllowCancel == true) {
                possibilities.Add(Appearance.CancelString);
            }
            if (IsCaseSensitive == false) {
                possibilities.ToLowerAll();
            }

            SelectionEmbed.AddField(Title, sBuilder.ToString());

            return new Selection<T>(Values.DeepClone(),
                                    possibilities.DeepClone(),
                                    SelectionEmbed.Build(),
                                    Appearance.DeepClone(),
                                    Users.ToArray().ToList(),
                                    AllowCancel,
                                    IsCaseSensitive);
        }

        /// <summary>
        /// Adds values to the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithValues(params T[] values)
        {
            Values.AddRange(values);
            return this;
        }

        /// <summary>
        /// Adds values to the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithValues(List<T> values)
        {
            Values.AddRange(values);
            return this;
        }

        /// <summary>
        /// Sets the stringconverter of the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="converter"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithStringConverter(Func<T, string> converter)
        {
            StringConverter = converter;
            return this;
        }

        /// <summary>
        /// Sets the appearance of the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="appearance"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithAppearance(SelectionAppearance appearance)
        {
            Appearance = appearance;
            return this;
        }

        /// <summary>
        /// Sets which users can interact with the <see cref="Selection{T}"/>. 
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithUsers(params SocketUser[] users)
        {
            Users = users.ToList();
            return this;
        }

        /// <summary>
        /// Sets some settings of the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="isCaseSensitive"></param>
        /// <param name="allowCancel"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithSettings(bool isCaseSensitive = false, bool allowCancel = true)
        {
            IsCaseSensitive = isCaseSensitive;
            AllowCancel = allowCancel;
            return this;
        }

        /// <summary>
        /// Sets the embed which the <see cref="Selection{T}"/> is made of.
        /// </summary>
        /// <param name="selectionEmbed"></param>
        /// <returns></returns>
        public SelectionBuilder<T> WithSelectionEmbed(EmbedBuilder selectionEmbed)
        {
            SelectionEmbed = selectionEmbed;
            return this;
        }
    }
}
