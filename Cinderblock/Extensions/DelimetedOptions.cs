namespace System.Collections.Generic
{
    /// <summary>
    /// An object that provides optional configuration information for <see cref="IEnumerable{T}.ToDelimetedFile{T}"/>.
    /// </summary>
    public sealed class DelimetedOptions
    {
        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="DelimetedOptions"/> class.</para>
        /// </summary>
        public DelimetedOptions() : this(',', false) { }

        /// <summary>
        /// 	<para>Initializes an instance of the <see cref="DelimetedOptions"/> class.</para>
        /// </summary>
        /// <param name="delimeter">The character that will be used to delimit data in the file.</param>
        /// <param name="includeHeader">Specifies if a column header will be included in the file.</param>
        public DelimetedOptions(char delimeter, bool includeHeader)
        {
            this.Delimeter = delimeter;
            this.IncludeHeader = includeHeader;
        }

        /// <summary>
        /// Gets the character that will be used to delimit data in the file.
        /// </summary>
        public char Delimeter { get; private set; }

        /// <summary>
        /// Gets a value that specifies if a column header will be included in the file.
        /// </summary>
        public bool IncludeHeader { get; private set; }
    }
}
