using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using System.Threading;

namespace System
{
    /// <summary>
    /// An extension method class that extends the <see cref="DateTime"/> type.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Determines the number of years between the instance date and the current date and time.
        /// </summary>
        /// <param name="date">The date to compare to DateTime.Now.</param>
        /// <returns></returns>
        public static int Age(this DateTime date)
        {
            var today = DateTime.Today;
            var age = today.Year - date.Year;

            if (today.Month < date.Month || (today.Month == date.Month && today.Day < date.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// Determines if the <see cref="DateTime"/> value falls within the range of <see cref="SqlDateTime"/> min and max values.
        /// </summary>
        /// <param name="date">The date to compare.</param>
        /// <param name="type">The underlying <see cref="SqlDateTime"/> to compare to.</param>
        /// <returns>true if the date is a valid <see cref="SqlDateTime"/>; otherwise false.</returns>
        public static bool IsSqlDateTime(this DateTime date, SqlDbType type)
        {
            switch (type)
            {
                case SqlDbType.DateTime:
                    return date.InRange(new DateTime(1753, 1, 1), new DateTime(9999, 12, 31));
                case SqlDbType.DateTime2:
                    return date.InRange(new DateTime(0001, 1, 1), new DateTime(9999, 12, 31, 23, 59, 59));
                case SqlDbType.SmallDateTime:
                    return date.InRange(new DateTime(1900, 1, 1), new DateTime(2079, 6, 6));
                default:
                    throw new ArgumentOutOfRangeException("type", "The SqlDbType specified isn't a valid DateTime type.");
            }
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> that represents the start date of a month for a specified date.
        /// </summary>
        /// <param name="date">The date to calculate the start of the month for.</param>
        /// <returns>A <see cref="DateTime"/> that represents the start date of a month for the specified date.</returns>
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> that represents the start date of the week.
        /// </summary>
        /// <param name="date"></param>
        /// <returns>The <see cref="DateTime"/> that represents the start of the week.</returns>
        public static DateTime StartOfWeek(this DateTime date)
        {
            return date.StartOfWeek(Thread.CurrentThread.CurrentCulture);
        }

        /// <summary>
        /// Gets a <see cref="DateTime"/> that represents the start date of the week.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="culture">The <see cref="CultureInfo"/> object that is used to determine the first day of week.</param>
        /// <returns>The <see cref="DateTime"/> that represents the start of the week.</returns>
        public static DateTime StartOfWeek(this DateTime date, CultureInfo culture)
        {
            var firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;
            var currentDayOfWeek = date.DayOfWeek;
            return DateTime.Now.AddDays(-(currentDayOfWeek - firstDayOfWeek)).Date;
        }
    }
}
