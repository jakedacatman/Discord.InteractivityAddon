using Discord;

namespace InteractivityAddon.Confirmation
{
    /// <summary>
    /// Appearance options of a <see cref="ConfirmationRequest"/>
    /// </summary>
    public class ConfirmationAppearance
    {
        public IEmote ConfirmEmote { get; }
        public IEmote DeclineEmote { get; }

        internal IEmote[] Emotes => new IEmote[] { ConfirmEmote, DeclineEmote };

        public Embed TimeoutedEmbed { get; }
        public Embed CancelledEmbed { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ConfirmationAppearance"/>.
        /// </summary>
        /// <param name="confirmEmote">The <see cref="IEmote"/> to confirm the action.</param>
        /// <param name="declineEmote"> The <see cref="IEmote"/> to decline the action.</param>
        /// <param name="timeoutedEmbed">The <see cref="Embed"/> to be shown after the timeout occured.</param>
        /// <param name="cancelledEmbed">The <see cref="Embed"/> to be shown when the request got cancelled.</param>
        public ConfirmationAppearance(IEmote confirmEmote = null, IEmote declineEmote = null, Embed timeoutedEmbed = null, Embed cancelledEmbed = null)
        {
            ConfirmEmote = confirmEmote ?? Default.ConfirmEmote;
            DeclineEmote = declineEmote ?? Default.DeclineEmote;
            TimeoutedEmbed = timeoutedEmbed ?? Default.TimeoutedEmbed;
            CancelledEmbed = cancelledEmbed ?? Default.CancelledEmbed;
        }

        /// <summary>
        /// The default appearance of a <see cref="ConfirmationRequest"/>.
        /// </summary>
        public static ConfirmationAppearance Default => new ConfirmationAppearance(
            confirmEmote: new Emoji("✅"),
            declineEmote: new Emoji("❌"),
            timeoutedEmbed: new EmbedBuilder().WithColor(Color.Red).WithTitle("Timed out! :alarm_clock:").Build(),
            cancelledEmbed: new EmbedBuilder().WithColor(Color.Orange).WithTitle("Cancelled! :thumbsup:").Build()
            );

        /// <summary>
        /// Creates a default <see cref="ConfirmationAppearance"/> with a custom <paramref name="timeoutedEmbed"/>.
        /// </summary>
        /// <param name="timeoutedEmbed">Your custom <paramref name="timeoutedEmbed"/>.</param>
        /// <returns></returns>
        public static ConfirmationAppearance FromTimeoutedEmbed(Embed timeoutedEmbed) => new ConfirmationAppearance(timeoutedEmbed: timeoutedEmbed);

        /// <summary>
        /// Creates a default <see cref="ConfirmationAppearance"/> with a custom <paramref name="cancelledEmbed"/>.
        /// </summary>
        /// <param name="cancelledEmbed">Your custom <paramref name="cancelledEmbed"/>.</param>
        /// <returns></returns>
        public static ConfirmationAppearance FromCancelledEmbed(Embed cancelledEmbed) => new ConfirmationAppearance(cancelledEmbed: cancelledEmbed);

        internal ConfirmationAction ParseAction(IEmote emote) => emote.Equals(ConfirmEmote) 
                                                                 ? ConfirmationAction.Confirm 
                                                                 : ConfirmationAction.Decline;
    }
}
