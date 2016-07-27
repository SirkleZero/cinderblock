namespace System
{
    /// <summary>
    /// Represents various algorithms for email validation.
    /// </summary>
    public enum EmailValidationType
    {
        /// <summary>
        /// Represents a regular expression that looks like \w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*
        /// </summary>
        Lenient,
        /// <summary>
        /// Represents a regular expression that looks like ^(([^<>()[\]\\.,;:\s@\""]+(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$
        /// </summary>
        Strict
    }
}
