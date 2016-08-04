namespace System
{
    /// <summary>
    /// Represents various algorithms for email validation.
    /// </summary>
    public enum EmailValidationType
    {
        /// <summary>
        /// Represents a regular expression that implements a linient email comparison.
        /// </summary>
        Lenient,
        /// <summary>
        /// Represents a regular expression that implements a strict email comparison.
        /// </summary>
        Strict
    }
}
