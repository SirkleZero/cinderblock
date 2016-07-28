using System;
using System.Diagnostics;

namespace Cinderblock.Examples.Model
{
    [DebuggerDisplay("Name = {FullName}")]
    public class Employee : IKeyed<int>, ICommanded<EmployeeCommandFactory>, IEquatable<Employee>
    {
        private readonly IEmployeeRepository repository;

        internal Employee(IEmployeeRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
            this.Commands = new EmployeeCommandFactory(this, this.repository);
        }

        public EmployeeCommandFactory Commands { get; private set; }
        public int? PrimaryKey { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return string.Concat(this.FirstName, " ", this.LastName); } }
        public DateTime Birthdate { get; set; }

        #region IEquatable<Employee>

        /// <summary>
        ///     <para>Determines if two <see cref="Employee"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="Employee"/> object to compare.</param>
        /// <param name="y">The second <see cref="Employee"/> object to compare.</param>
        /// <returns>true if the first <see cref="Employee"/> is equal to the second <see cref="Employee"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public static bool Equals(Employee x, Employee y)
        {
            if ((object)x == (object)y)
            {
                return true;
            }
            if ((object)x == null || (object)y == null)
            {
                return false;
            }
            return x.PrimaryKey.Equals(y.PrimaryKey) && x.FirstName.Equals(y.FirstName, StringComparison.OrdinalIgnoreCase) &&
                x.LastName.Equals(y.LastName, StringComparison.OrdinalIgnoreCase) && x.Birthdate.Equals(y.Birthdate);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>.</para>
        /// </summary>
        /// <param name="objectToCompare">The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/>.</param>
        /// <returns>true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/>; otherwise false.</returns>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object objectToCompare)
        {
            return Employee.Equals(this, objectToCompare as Employee);
        }

        /// <summary>
        ///     <para>Determines whether the specified <see cref="Employee"/> is equal to the current <see cref="Employee"/>.</para>
        /// </summary>
        /// <param name="other">The <see cref="Employee"/> to compare with the current <see cref="Employee"/>.</param>
        /// <returns>true if the specified <see cref="Employee"/> is equal to the current <see cref="Employee"/>; otherwise false.</returns>
        public bool Equals(Employee other)
        {
            return Employee.Equals(this, other);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="Employee"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="Employee"/> object to compare.</param>
        /// <param name="y">The second <see cref="Employee"/> object to compare.</param>
        /// <returns>true if the first <see cref="Employee"/> is equal to the second <see cref="Employee"/>; otherwise false.</returns>
        public static bool operator ==(Employee x, Employee y)
        {
            return Employee.Equals(x, y);
        }

        /// <summary>
        ///     <para>Determines if two <see cref="Employee"/> objects have the same value.</para>
        /// </summary>
        /// <param name="x">The first <see cref="Employee"/> object to compare.</param>
        /// <param name="y">The second <see cref="Employee"/> object to compare.</param>
        /// <returns>false if the first <see cref="Employee"/> is equal to the second <see cref="Employee"/>; otherwise true.</returns>
        public static bool operator !=(Employee x, Employee y)
        {
            return !Employee.Equals(x, y);
        }

        #endregion

        #region GetHashCode()

        /// <summary>
        ///     <para>Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"/> is suitable for use in hashing algorithms and data structures like a hash table.</para>
        /// </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object"/>.</returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.PrimaryKey.GetHashCode() ^ this.FirstName.GetHashCode() ^ this.LastName.GetHashCode() ^ this.Birthdate.GetHashCode();
        }

        #endregion
    }
}
