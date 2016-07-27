namespace Cinderblock
{
    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    public interface IReturningCommand<R>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        bool CanExecute();

        /// <summary>
        /// Executes the command.
        /// </summary>
        R Execute();
    }
}
