namespace System.IO
{
    /// <summary>
    /// Provides extension methods for Stream objects.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all of the bytes of a <see cref="BinaryReader"/> object into a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
        /// <returns>A <see cref="T:byte[]"/> containing the data of the <see cref="BinaryReader"/>.</returns>
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                var buffer = new byte[bufferSize];
                int count;

                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                {
                    ms.Write(buffer, 0, count);
                }

                return ms.ToArray();
            }
        }
    }
}
