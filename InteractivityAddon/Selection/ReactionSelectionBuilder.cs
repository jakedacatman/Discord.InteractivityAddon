using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace InteractivityAddon.Selection.Appearance
{
    /// <summary>
    /// Represents a builder class for making a <see cref="ReactionSelection{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of values to select from</typeparam>
    public sealed class ReactionSelectionBuilder<T> : SelectionBuilder<T, SocketReaction, ReactionSelectionAppearanceBuilder, ReactionSelectionAppearance>
    {
        /// <summary>
        /// Gets or sets the emotes which are used to select values.
        /// </summary>
        public List<IEmote> Emotes { get; set; } = new List<IEmote>();

        /// <summary>
        /// Gets or sets the function to convert the values into possibilites.
        /// </summary>
        public Func<T, string> StringConverter { get; set; } = x => x.ToString();

        /// <summary>
        /// Gets or sets the title of the selection.
        /// </summary>
        public string Title { get; set; } = "Select one of these:";

        /// <summary>
        /// Gets or sets whether the <see cref="Selection{T, T1, TSelectionAppearance}"/> allows for cancellation.
        /// </summary>
        public bool AllowCancel { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the selectionembed will be added by a default value visualizer.
        /// </summary>
        public bool EnableDefaultSelectionDescription { get; set; } = true;

        /// <summary>
        /// Creates a new <see cref="ReactionSelectionBuilder{T}"/> with default values.
        /// </summary>
        public ReactionSelectionBuilder()
        {
        }

        /// <summary>
        /// Creates a new <see cref="ReactionSelectionBuilder{T}"/> with default values.
        /// </summary>
        public static ReactionSelectionBuilder<T> Default => new ReactionSelectionBuilder<T>();

        public override Selection<T, SocketReaction, ReactionSelectionAppearance> Build()
        {
            if (Emotes.Count < Values.Count) {
                throw new InvalidOperationException("Value count larger than emote count! Please add more Emotes to the selection!");
            }
            if (Emotes.Contains(Appearance.CancelEmote) == true) {
                throw new InvalidOperationException("Please remove the cancel emote from the selection emotes!");
            }

            var builder = new StringBuilder();

            for (int i = 0; i < Values.Count; i++) {
                string possibility = StringConverter.Invoke(Values[i]);
                builder.AppendLine($"{Emotes[i]} - {possibility}");
            }

            if (EnableDefaultSelectionDescription == true) {
                SelectionEmbed.AddField(Title, builder.ToString());
            }
            
            return new ReactionSelection<T>(
                SelectionEmbed.Build(),
                Values.ToImmutableList(),
                Emotes.ToImmutableList(),
                Appearance.Build() as ReactionSelectionAppearance,
                Users.ToImmutableList(),
                AllowCancel);
        }

        public ReactionSelectionBuilder<T> WithValues(List<T> values)
        {
            Values = values;
            return this;
        }

        public ReactionSelectionBuilder<T> WithValues(params T[] values)
        {
            Values = values.ToList();
            return this;
        }

        public ReactionSelectionBuilder<T> WithAppearance(ReactionSelectionAppearanceBuilder appearance)
        {
            Appearance = appearance;
            return this;
        }

        public ReactionSelectionBuilder<T> WithUsers(params SocketUser[] users)
        {
            Users = users.ToList();
            return this;
        }

        public ReactionSelectionBuilder<T> WithUsers(List<SocketUser> users)
        {
            Users = users;
            return this;
        }

        public ReactionSelectionBuilder<T> WithSelectionEmbed(EmbedBuilder embed)
        {
            SelectionEmbed = embed;
            return this;
        }

        public ReactionSelectionBuilder<T> WithSettings(bool allowCancel = true, bool enableDefaultSelectionDescription = false)
        {
            AllowCancel = allowCancel;
            EnableDefaultSelectionDescription = enableDefaultSelectionDescription;
            return this;
        }

        public ReactionSelectionBuilder<T> WithEmotes(params IEmote[] emotes)
        {
            Emotes = emotes.Distinct().ToList();
            return this;
        }

        public ReactionSelectionBuilder<T> WithEmotes(List<IEmote> emotes)
        {
            Emotes = emotes;
            return this;
        }

        public ReactionSelectionBuilder<T> WithStringConverter(Func<T, string> stringConverter)
        {
            StringConverter = stringConverter;
            return this;
        }

        public ReactionSelectionBuilder<T> WithTitle(string title)
        {
            Title = title;
            return this;
        }
    }
}
