namespace System.Collections.Generic
{
    /// <summary>
    /// A wrapper class that provides meta information to an object of a Cycle operation.
    /// </summary>
    /// <typeparam name="T">The type of generic <see cref="{T}"/> being wrapped.</typeparam>
    /// <typeparam name="TIdentifier">The type of generic <see cref="{TIdentifier}"/> that represents the meta information type wrapping the target.</typeparam>
    public sealed class CycledItem<T, TIdentifier> : ICycledItem<T, TIdentifier>
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="CycledItem"/> class.</para>
        /// </summary>
        /// <param name="item">The generic item of type <see cref="{T}"/> that is wrapped.</param>
        /// <param name="identifier">The identifying meta information attached to the wrapped object.</param>
        /// <exception cref="ArgumentNullException">
        /// 	<para>The argument <paramref name="item"/> is <langword name="null"/>.</para>
        /// 	<para>-or-</para>
        /// 	<para>The argument <paramref name="identifier"/> is <langword name="null"/>.</para>
        /// </exception>
        public CycledItem(T item, TIdentifier identifier)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (identifier == null)
            {
                throw new ArgumentNullException("identifier");
            }

            this.Item = item;
            this.Identifier = identifier;
        }

        #region CycledItem<T> Members

        /// <summary>
        /// Gets the generic item of type <see cref="{T}"/> that is wrapped.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Gets the identifying meta information attached to the wrapped object.
        /// </summary>
        public TIdentifier Identifier { get; private set; }

        #endregion
    }
}
