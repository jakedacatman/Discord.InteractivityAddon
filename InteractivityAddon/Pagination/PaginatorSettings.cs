namespace InteractivityAddon.Pagination
{
    /// <summary>
    /// Settings of a <see cref="Paginator"/>.
    /// </summary>
    public sealed class PaginatorSettings
    {
        /// <summary>
        /// Determites wether to delete reactions which are not associated with the <see cref="Paginator"/>.
        /// </summary>
        public bool RemoveOtherReactions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="PaginatorSettings"/>.
        /// </summary>
        /// <param name="isUserRestricted">Determited whether everyone can interact with the <see cref="Paginator"/>.</param>
        /// <param name="users">Determites which users can interact with the <see cref="Paginator"/>.</param>
        /// <param name="removeOtherReactions">Determites wether to delete reactions which are not associated with the <see cref="Paginator"/>.</param>
        public PaginatorSettings(bool removeOtherReactions)
        {
            RemoveOtherReactions = removeOtherReactions;
        }

        /// <summary>
        /// The default settings for a <see cref="Paginator"/>.
        /// </summary>
        public static PaginatorSettings Default => new PaginatorSettings(true);

        /// <summary>
        /// Sets wether to delete reactions which are not associated with the <see cref="Paginator"/>.
        /// </summary>
        /// <param name="removeOtherReactions"></param>
        /// <returns></returns>
        public PaginatorSettings WithRemoveOtherReactions(bool removeOtherReactions)
        {
            RemoveOtherReactions = removeOtherReactions;
            return this;
        }
    }
}
