using Discord;

namespace InteractivityAddon.Extensions
{
    internal static class EmbedFieldExtensions
    {
        public static EmbedFieldBuilder ToBuilder(this EmbedField field) => new EmbedFieldBuilder()
                .WithIsInline(field.Inline)
                .WithName(field.Name)
                .WithValue(field.Value);
    }
}
