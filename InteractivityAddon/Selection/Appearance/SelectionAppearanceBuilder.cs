using System;
using Discord;

namespace InteractivityAddon.Selection
{
    /// <summary>
    /// Represents a builder class for creating a <see cref="SelectionAppearance"/>.
    /// </summary>
    public sealed class SelectionAppearanceBuilder
    {
        private EmbedBuilder cancelledEmbed = new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:");
        private EmbedBuilder timeoutedEmbed = new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:");

        /// <summary>
        /// Gets or sets the <see cref="EmbedBuilder"/> which the <see cref="Selection"/> gets modified to after cancellation.
        /// </summary>
        public EmbedBuilder CancelledEmbed
        {
            get => cancelledEmbed;
            set => cancelledEmbed = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets or sets the <see cref="EmbedBuilder"/> which the <see cref="Selection"/> gets modified to after a timeout.
        /// </summary>
        public EmbedBuilder TimeoutedEmbed
        {
            get => timeoutedEmbed;
            set => timeoutedEmbed = value ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets or sets the string which can be selected to cancel the <see cref="Selection{T}"/>.
        /// </summary>
        public string CancelString { get; set; } = "Cancel";

        /// <summary>
        /// Gets or sets whether the <see cref="Selection"/> will delete invalid messages.
        /// </summary>
        public bool DeleteInvalidMessages { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the <see cref="Selection"/> will delete the valid message.
        /// </summary>
        public bool DeleteValidMessage { get; set; } = false;

        /// <summary>
        /// Gets or sets whether the <see cref="Selection"/> will get deleted after a <see cref="InteractivityResult{T}"/> has been captured.
        /// </summary>
        public bool DeleteSelectionAfterCapturedResult { get; set; } = false;


        public SelectionAppearance Build() => new SelectionAppearance(
            CancelledEmbed.Build(),
            TimeoutedEmbed.Build(),
            CancelString,
            DeleteInvalidMessages,
            DeleteValidMessage,
            DeleteSelectionAfterCapturedResult);

        /// <summary>
        /// Creates a new instane of <see cref="SelectionAppearance"/> with the default values.
        /// </summary>
        public SelectionAppearanceBuilder() { }

        /// <summary>
        /// Creates a new instane of <see cref="SelectionAppearance"/> with the default values.
        /// </summary>
        public static SelectionAppearanceBuilder Default => new SelectionAppearanceBuilder();

        /// <summary>
        /// Sets the cancelledembed of the <see cref="Selection"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public SelectionAppearanceBuilder WithCancelledEmbed(EmbedBuilder embed)
        {
            CancelledEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the timeoutedembed of the <see cref="Selection"/>.
        /// </summary>
        /// <param name="embed"></param>
        /// <returns></returns>
        public SelectionAppearanceBuilder WithTimeoutedEmbed(EmbedBuilder embed)
        {
            TimeoutedEmbed = embed;
            return this;
        }

        /// <summary>
        /// Sets the deletion settings of the <see cref="Selection{T}"/>.
        /// </summary>
        /// <param name="deleteInvalidMessages"></param>
        /// <param name="deleteValidMessage"></param>
        /// <param name="deleteSelectionAfterCapturedResult"></param>
        /// <returns></returns>
        public SelectionAppearanceBuilder WithSettings(bool deleteInvalidMessages = false, bool deleteValidMessage = false, bool deleteSelectionAfterCapturedResult = false)
        {
            DeleteInvalidMessages = deleteInvalidMessages;
            DeleteValidMessage = deleteValidMessage;
            DeleteSelectionAfterCapturedResult = deleteSelectionAfterCapturedResult;
            return this;
        }
    }
}
