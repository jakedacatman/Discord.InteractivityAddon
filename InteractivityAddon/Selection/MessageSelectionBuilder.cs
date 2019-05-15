using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a builder class for making a <see cref="MessageSelection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of values to select from</typeparam>
    public sealed class MessageSelectionBuilder<T> : SelectionBuilder<T, SocketMessage, MessageSelectionAppearanceBuilder, MessageSelectionAppearance>
    {
        /// <summary>
        /// Gets or sets the function to convert the values into possibilites.
        /// </summary>
        public Func<T, Task<string>> StringConverter { get; set; } = x => Task.FromResult(x.ToString());

        /// <summary>
        /// Gets or sets whether the <see cref="Selection{T, T1, TSelectionAppearance}"/> allows for cancellation.
        /// </summary>
        public bool AllowCancel { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the selectionembed will be added by a default value visualizer.
        /// </summary>
        public bool EnableDefaultSelectionDescription { get; set; } = true;

        /// <summary>
        /// Gets or sets the title of the <see cref="Selection{T, T1, TSelectionAppearance}"/>.
        /// </summary>
        public string Title { get; set; } = "Select one of these";

        /// <summary>
        /// Creates a new <see cref="MessageSelectionBuilder{T}"/> with default values.
        /// </summary>
        public MessageSelectionBuilder() { }

        /// <summary>
        /// Creates a new <see cref="MessageSelectionBuilder{T}"/> with default values.
        /// </summary>
        public static MessageSelectionBuilder<T> Default() => new MessageSelectionBuilder<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async override Task<Selection<T, SocketMessage, MessageSelectionAppearance>> Build()
        {
            if (Values.Count == 0) {
                throw new InvalidOperationException("Your Selection needs at least one value");
            }

            var possibilities = new List<string>();
            var sBuilder = new StringBuilder();

            for (int i = 0; i < Values.Count; i++) {
                string possibility = await StringConverter.Invoke(Values[i]);
                sBuilder.AppendLine($"#{i + 1} - {possibility}");
                possibilities.Add($"{i + 1}");
                possibilities.Add($"#{i + 1}");
                possibilities.Add(possibility);
                possibilities.Add($"#{i + 1} - {possibility}");
            }
            if (AllowCancel == true) {
                sBuilder.Append($"#{Values.Count + 1} - {Appearance.CancelDisplayName}");
                possibilities.Add($"{Values.Count + 1}");
                possibilities.Add($"#{Values.Count + 1}");
                possibilities.Add(Appearance.CancelDisplayName);
                possibilities.Add($"#{Values.Count + 1} - {Appearance.CancelDisplayName}");
            }

            if (EnableDefaultSelectionDescription == true) {
                SelectionEmbed.AddField(Title, sBuilder.ToString());
            }

            return new MessageSelection<T>(
                Values.ToImmutableList(),
                possibilities.ToImmutableList(),
                Users.ToImmutableList(),
                Appearance.Build(),
                SelectionEmbed.Build());
        }

        public MessageSelectionBuilder<T> WithValues(List<T> values)
        {
            Values = values;
            return this;
        }

        public MessageSelectionBuilder<T> WithValues(params T[] values)
        {
            Values = values.ToList();
            return this;
        }

        public MessageSelectionBuilder<T> WithAppearance(MessageSelectionAppearanceBuilder appearance)
        {
            Appearance = appearance;
            return this;
        }

        public MessageSelectionBuilder<T> WithUsers(params SocketUser[] users)
        {
            Users = users.ToList();
            return this;
        }

        public MessageSelectionBuilder<T> WithUsers(List<SocketUser> users)
        {
            Users = users;
            return this;
        }

        public MessageSelectionBuilder<T> WithSelectionEmbed(EmbedBuilder embed)
        {
            SelectionEmbed = embed;
            return this;
        }

        public MessageSelectionBuilder<T> WithStringConverter(Func<T, Task<string>> stringConverter)
        {
            StringConverter = stringConverter;
            return this;
        }

        public MessageSelectionBuilder<T> WithSettings(bool allowCancel = true, bool enableDefaultSelectionDescription = false)
        {
            AllowCancel = allowCancel;
            EnableDefaultSelectionDescription = enableDefaultSelectionDescription;
            return this;
        }

        public MessageSelectionBuilder<T> WithTitle(string title)
        {
            Title = title;
            return this;
        }
    }
}
