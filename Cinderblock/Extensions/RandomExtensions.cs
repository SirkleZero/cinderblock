namespace System.Security.Cryptography
{
    /// <summary>
    /// Provides extension methods that support random number generation.
    /// </summary>
    public static class RandomExtensions
    {
        #region System.Random extensions

        /// <summary>
        /// Returns the next <see cref="System.Decimal"/> value.
        /// </summary>
        /// <param name="rg"></param>
        /// <returns>A random <see cref="System.Decimal"/> value.</returns>
        public static decimal NextDecimal(this Random rg)
        {
            var sign = rg.Next(2) == 1;
            return rg.NextDecimal(sign);
        }

        /// <summary>
        /// Returns the next <see cref="System.Decimal"/> value.
        /// </summary>
        /// <param name="rg"></param>
        /// <param name="maxValue">The maximum value to use as a random number.</param>
        /// <returns>A random <see cref="System.Decimal"/> value.</returns>
        public static decimal NextDecimal(this Random rg, decimal maxValue)
        {
            return (rg.NextNonNegativeDecimal() / Decimal.MaxValue) * maxValue; ;
        }

        /// <summary>
        /// Returns the next <see cref="System.Decimal"/> value.
        /// </summary>
        /// <param name="rg"></param>
        /// <param name="minValue">The minimum value to use as a random number.</param>
        /// <param name="maxValue">The maximum value to use as a random number.</param>
        /// <returns>A random <see cref="System.Decimal"/> value.</returns>
        public static decimal NextDecimal(this Random rg, decimal minValue, decimal maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException("maxValue", "The maximum value specified cannot be smaller than the minimum value.");
            }

            var range = maxValue - minValue;
            return rg.NextDecimal(range) + minValue;
        }

        /// <summary>
        /// Returns the next <see cref="System.Double"/> value.
        /// </summary>
        /// <param name="rg"></param>
        /// <param name="maxValue">The maximum value to use as a random number.</param>
        /// <returns>A random <see cref="System.Decimal"/> value.</returns>
        public static long NextLong(this Random rg, long maxValue)
        {
            return (long)((rg.NextNonNegativeLong() / (double)Int64.MaxValue) * maxValue);
        }

        /// <summary>
        /// Returns the next <see cref="System.Double"/> value.
        /// </summary>
        /// <param name="rg"></param>
        /// <param name="minValue">The minimum value to use as a random number.</param>
        /// <param name="maxValue">The maximum value to use as a random number.</param>
        /// <returns>A random <see cref="System.Decimal"/> value.</returns>
        public static long NextLong(this Random rg, long minValue, long maxValue)
        {
            if (minValue >= maxValue)
            {
                throw new ArgumentOutOfRangeException("maxValue", "The maximum value specified cannot be smaller than the minimum value.");
            }

            long range = maxValue - minValue;
            return rg.NextLong(range) + minValue;
        }

        private static int NextInt32(this Random rg)
        {
            unchecked
            {
                var firstBits = rg.Next(0, 1 << 4) << 28;
                var lastBits = rg.Next(0, 1 << 28);

                return firstBits | lastBits;
            }
        }

        private static decimal NextDecimal(this Random rg, bool sign)
        {
            var scale = (byte)rg.Next(29);
            return new decimal(rg.NextInt32(),
                               rg.NextInt32(),
                               rg.NextInt32(),
                               sign,
                               scale);
        }

        private static decimal NextNonNegativeDecimal(this Random rg)
        {
            return rg.NextDecimal(false);
        }

        private static long NextNonNegativeLong(this Random rg)
        {
            var bytes = new byte[sizeof(long)];
            rg.NextBytes(bytes);

            // strip out the sign bit
            bytes[7] = (byte)(bytes[7] & 0x7f);
            return BitConverter.ToInt64(bytes, 0);
        }

        #endregion

        #region System.Security.Cryptography.RNGCryptoServiceProvider extensions

        /// <summary>
        /// Generates a cryptographically random <see cref="System.Int32"/> value.
        /// </summary>
        /// <param name="rg">The <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to use.</param>
        /// <returns>A cryptographically random numeric value.</returns>
        public static int Next(this RNGCryptoServiceProvider rg)
        {
            var randomNumber = new byte[4];
            rg.GetBytes(randomNumber);
            return Math.Abs(BitConverter.ToInt32(randomNumber, 0));
        }

        /// <summary>
        /// Generates a cryptographically random <see cref="System.Int32"/> value.
        /// </summary>
        /// <param name="rg">The <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to use.</param>
        /// <param name="minValue">The minimum value to use as a random number.</param>
        /// <param name="maxValue">The maximum value to use as a random number.</param>
        /// <returns>A cryptographically random numeric value.</returns>
        public static int Next(this RNGCryptoServiceProvider rg, int minValue, int maxValue)
        {
            return (rg.Next() % maxValue) + minValue;
        }

        /// <summary>
        /// Generates a cryptographically random <see cref="System.Double"/> value.
        /// </summary>
        /// <param name="rg">The <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to use.</param>
        /// <returns>A cryptographically random numeric value.</returns>
        public static double NextDouble(this RNGCryptoServiceProvider rg)
        {
            byte[] randomNumber = new byte[8];
            rg.GetBytes(randomNumber);
            return (double)BitConverter.ToUInt64(randomNumber, 0) / ulong.MaxValue;
        }

        /// <summary>
        /// Generates a cryptographically random <see cref="System.Double"/> value.
        /// </summary>
        /// <param name="rg">The <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to use.</param>
        /// <param name="minValue">The minimum value to use as a random number.</param>
        /// <param name="maxValue">The maximum value to use as a random number.</param>
        /// <returns>A cryptographically random numeric value.</returns>
        public static double NextDouble(this RNGCryptoServiceProvider rg, double minimum, double maximum)
        {
            return ((maximum - minimum) * rg.NextDouble()) + minimum;
        }

        /// <summary>
        /// Generates a cryptographically random salt value with a variant size.
        /// </summary>
        /// <param name="rg">The <see cref="System.Security.Cryptography.RNGCryptoServiceProvider"/> to use.</param>
        /// <param name="minimumBytes">The minimum number of bytes that the salt must contain.</param>
        /// <param name="maximumBytes">The maximum number of bytes that the salt can contain.</param>
        /// <returns>An array of bytes that represents the value of the salt.</returns>
        public static byte[] GenerateSalt(this RNGCryptoServiceProvider rg, int minimumBytes, int maximumBytes)
        {
            // Generate a random number for the size of the salt.
            var random = new Random();
            var saltSize = random.Next(minimumBytes, maximumBytes);

            //Create and populate random byte array
            var randomArray = new byte[saltSize];
            rg.GetNonZeroBytes(randomArray);

            return randomArray;
        }

        #endregion
    }
}
