namespace Cinderblock
{
    /// <summary>
    /// Provides an interface that exposes a generically typed command factory object.
    /// </summary>
    /// <typeparam name="TCommandFactory">The generic type that represents the command factory used to provide a common command interface.</typeparam>
    public interface ICommanded<TCommandFactory> where TCommandFactory : ICommandFactory
    {
        /// <summary>
        /// Gets the command factory associated with the object.
        /// </summary>
        TCommandFactory Commands { get; }
    }
}
