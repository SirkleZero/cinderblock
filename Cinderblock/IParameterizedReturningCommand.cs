namespace Cinderblock
{
    #region IParameterizedReturningCommand<R,T1>

    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    /// <typeparam name="R">The return type of the command.</typeparam>
    /// <typeparam name="T1">The generic type used to parameterize the command.</typeparam>
    public interface IParameterizedReturningCommand<R, T1>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        /// <param name="parameter">The generic type used to parameterize the command.</param>
        bool CanExecute(T1 parameter);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The generic type used to parameterize the command.</param>
        R Execute(T1 parameter);
    }

    #endregion

    #region IParameterizedReturningCommand<R, T1, T2>

    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    /// <typeparam name="R">The return type of the command.</typeparam>
    /// <typeparam name="T1">The generic type used for the first argument to parameterize the command.</typeparam>
    /// <typeparam name="T2">The generic type used for the second argument to parameterize the command.</typeparam>
    public interface IParameterizedReturningCommand<R, T1, T2>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        bool CanExecute(T1 parameter1, T2 parameter2);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        R Execute(T1 parameter1, T2 parameter2);
    }

    #endregion

    #region IParameterizedReturningCommand<R, T1, T2, T3>

    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    /// <typeparam name="R">The return type of the command.</typeparam>
    /// <typeparam name="T1">The generic type used for the first argument to parameterize the command.</typeparam>
    /// <typeparam name="T2">The generic type used for the second argument to parameterize the command.</typeparam>
    /// <typeparam name="T3">The generic type used for the third argument to parameterize the command.</typeparam>
    public interface IParameterizedReturningCommand<R, T1, T2, T3>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        bool CanExecute(T1 parameter1, T2 parameter2, T3 parameter3);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        R Execute(T1 parameter1, T2 parameter2, T3 parameter3);
    }

    #endregion

    #region IParameterizedReturningCommand<R, T1, T2, T3, T4>

    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    /// <typeparam name="R">The return type of the command.</typeparam>
    /// <typeparam name="T1">The generic type used for the first argument to parameterize the command.</typeparam>
    /// <typeparam name="T2">The generic type used for the second argument to parameterize the command.</typeparam>
    /// <typeparam name="T3">The generic type used for the third argument to parameterize the command.</typeparam>
    /// <typeparam name="T4">The generic type used for the fourth argument to parameterize the command.</typeparam>
    public interface IParameterizedReturningCommand<R, T1, T2, T3, T4>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        /// <param name="parameter4">The generic type used for the fourth argument to parameterize the command.</param>
        bool CanExecute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        /// <param name="parameter4">The generic type used for the fourth argument to parameterize the command.</param>
        R Execute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4);
    }

    #endregion

    #region IParameterizedReturningCommand<R, T1, T2, T3, T4, T5>

    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    /// <typeparam name="R">The return type of the command.</typeparam>
    /// <typeparam name="T1">The generic type used for the first argument to parameterize the command.</typeparam>
    /// <typeparam name="T2">The generic type used for the second argument to parameterize the command.</typeparam>
    /// <typeparam name="T3">The generic type used for the third argument to parameterize the command.</typeparam>
    /// <typeparam name="T4">The generic type used for the fourth argument to parameterize the command.</typeparam>
    /// <typeparam name="T5">The generic type used for the fifth argument to parameterize the command.</typeparam>
    public interface IParameterizedReturningCommand<R, T1, T2, T3, T4, T5>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        /// <param name="parameter4">The generic type used for the fourth argument to parameterize the command.</param>
        /// <param name="parameter5">The generic type used for the fifth argument to parameterize the command.</param>
        bool CanExecute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        /// <param name="parameter4">The generic type used for the fourth argument to parameterize the command.</param>
        /// <param name="parameter5">The generic type used for the fifth argument to parameterize the command.</param>
        R Execute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5);
    }

    #endregion

    #region IParameterizedReturningCommand<R, T1, T2, T3, T4, T5, T6>

    /// <summary>
    /// Provides a command interface that defines behaviors within the system.
    /// </summary>
    /// <typeparam name="R">The return type of the command.</typeparam>
    /// <typeparam name="T1">The generic type used for the first argument to parameterize the command.</typeparam>
    /// <typeparam name="T2">The generic type used for the second argument to parameterize the command.</typeparam>
    /// <typeparam name="T3">The generic type used for the third argument to parameterize the command.</typeparam>
    /// <typeparam name="T4">The generic type used for the fourth argument to parameterize the command.</typeparam>
    /// <typeparam name="T5">The generic type used for the fifth argument to parameterize the command.</typeparam>
    /// <typeparam name="T6">The generic type used for the fifth argument to parameterize the command.</typeparam>
    public interface IParameterizedReturningCommand<R, T1, T2, T3, T4, T5, T6>
    {
        /// <summary>
        /// Determines if the command meets its requirements for execution.
        /// </summary>
        /// <returns>true if the command can execute; otherwise false.</returns>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        /// <param name="parameter4">The generic type used for the fourth argument to parameterize the command.</param>
        /// <param name="parameter5">The generic type used for the fifth argument to parameterize the command.</param>
        /// <param name="parameter6">The generic type used for the sixth argument to parameterize the command.</param>
        bool CanExecute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5, T6 parameter6);

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter1">The generic type used for the first argument to parameterize the command.</param>
        /// <param name="parameter2">The generic type used for the second argument to parameterize the command.</param>
        /// <param name="parameter3">The generic type used for the third argument to parameterize the command.</param>
        /// <param name="parameter4">The generic type used for the fourth argument to parameterize the command.</param>
        /// <param name="parameter5">The generic type used for the fifth argument to parameterize the command.</param>
        /// <param name="parameter6">The generic type used for the sixth argument to parameterize the command.</param>
        R Execute(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5, T6 parameter6);
    }

    #endregion
}
