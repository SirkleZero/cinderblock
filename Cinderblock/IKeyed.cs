namespace Cinderblock
{
    /// <summary>
    /// Provides an interface that specifies a primary key or unique identifier for an object when stored in a storage medium such as a database or file.
    /// </summary>
    /// <typeparam name="TKey">The generic type that represents the type of data that the key is.</typeparam>
    public interface IKeyed<TKey> where TKey : struct
    {
        /// <summary>
        /// Gets a value that uniquely identifies an instance of the object in a storage medium.
        /// </summary>
        TKey? PrimaryKey { get; }
    }
}
