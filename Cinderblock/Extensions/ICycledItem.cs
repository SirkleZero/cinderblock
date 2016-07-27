namespace System.Collections.Generic
{
    /// <summary>
    /// Provides an interface that is used to wrap an object with meta information from a Cycle operation.
    /// </summary>
    /// <typeparam name="T">The type of generic <see cref="{T}"/> being wrapped.</typeparam>
    /// <typeparam name="TIdentifier">The type of generic <see cref="{TIdentifier}"/> that represents the meta information type wrapping the target.</typeparam>
    public interface ICycledItem<T, TIdentifier>
    {
        /// <summary>
        /// Gets the generic item of type <see cref="{T}"/> that is wrapped.
        /// </summary>
        T Item { get; }
        /// <summary>
        /// Gets the identifying meta information attached to the wrapped object.
        /// </summary>
        TIdentifier Identifier { get; }
    }
}
