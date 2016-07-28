using System;
using System.Collections.Generic;

namespace Cinderblock.Examples.Model
{
    public class EmployeeFactory
    {
        private readonly IEmployeeRepository repository;

        public EmployeeFactory(IEmployeeRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.repository = repository;
        }

        public Employee Create(int primaryKey)
        {
            return this.repository.GetByPrimaryKey(primaryKey);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return this.repository.GetAllEmployees();
        }
    }
}
