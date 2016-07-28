using Cinderblock.Examples.Model.Commands;
using System;

namespace Cinderblock.Examples.Model
{
    public sealed class EmployeeCommandFactory : ICommandFactory
    {
        private readonly Employee employee;
        private readonly IEmployeeRepository repository;

        public EmployeeCommandFactory(Employee Employee, IEmployeeRepository repository)
        {
            if (Employee == null)
            {
                throw new ArgumentNullException("employee");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.employee = Employee;
            this.repository = repository;

            this.Initialize();
        }

        private void Initialize()
        {
            this.Save = new SaveEmployeeCommand(this.employee, this.repository);
            this.Delete = new DeleteEmployeeCommand(this.employee, this.repository);
        }

        public ICommand Save { get; private set; }
        public ICommand Delete { get; private set; }
    }
}
