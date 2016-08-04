namespace Cinderblock
{
    /// <summary>
    /// Provides an interface that defines data or behavioral refresh functionality.
    /// </summary>
    public interface IRefreshable
    {
        /// <summary>
        /// Performs a refresh operation appropriate to the data or functionality of the object.
        /// </summary>
        void Refresh();
    }
}
