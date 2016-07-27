namespace System
{
    /// <summary>
    /// Provides extension methods designed to work with struct types
    /// </summary>
    public static class StructExtensions
    {
        /// <summary>
        /// Determines if the value falls within a range of acceptable values.
        /// </summary>
        /// <typeparam name="T">Specifies the data type to check.</typeparam>
        /// <param name="t">The instance of the value to compare.</param>
        /// <param name="lower">The lower bound value used in the comparison.</param>
        /// <param name="upper">The upper bound value used in the comparison.</param>
        /// <returns>true if the value falls within the range specified by the lower and upper boundaries; otherwise false.</returns>
        public static bool InRange<T>(this T t, T lower, T upper) where T : struct, IComparable<T>
        {
            if (t.CompareTo(lower) >= 0 && t.CompareTo(upper) <= 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if the value falls within a range of acceptable values.
        /// </summary>
        /// <typeparam name="T">Specifies the data type to check.</typeparam>
        /// <param name="t">The instance of the value to compare.</param>
        /// <param name="lower">The lower bound value used in the comparison.</param>
        /// <param name="upper">The upper bound value used in the comparison.</param>
        /// <returns>true if the value falls within the range specified by the lower and upper boundaries; otherwise false.</returns>
        public static bool InRange<T>(this T? t, T? lower, T? upper) where T : struct, IComparable<T>
        {
            if (t.HasValue && lower.HasValue && upper.HasValue)
            {
                // if everything has a value, then simply use the existing InRange method.
                return t.Value.InRange<T>(lower.Value, upper.Value);
            }
            else if (!t.HasValue && !lower.HasValue && !upper.HasValue)
            {
                // if nothing has a value, then null is always within the range of null.
                return true;
            }
            else
            {
                if (!t.HasValue)
                {
                    // the value of i is null.  if one of the other values is null, then i is within range.
                    return !lower.HasValue || !upper.HasValue;
                }
                else if (!lower.HasValue)
                {
                    // the value of i is not null, however lower has no value.  i must have a value less than or equal to upper
                    if (upper.HasValue)
                    {
                        return ((IComparable<T>)t.Value).CompareTo(upper.Value) <= 0;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (!upper.HasValue)
                {
                    // the value of i is not null, however upper has no value.  i must have a value greater than or equal to lower
                    if (lower.HasValue)
                    {
                        return ((IComparable<T>)t.Value).CompareTo(lower.Value) >= 0;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return false;
        }
    }
}
