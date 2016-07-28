using System;

namespace Cinderblock.Examples.Model.Commands
{
    public sealed class DeleteEmployeeCommand : ICommand
    {
        private readonly Employee employee;
        private readonly IEmployeeRepository repository;

        public DeleteEmployeeCommand(Employee employee, IEmployeeRepository repository)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("employee");
            }
            if (repository == null)
            {
                throw new ArgumentNullException("repository");
            }

            this.employee = employee;
            this.repository = repository;
        }

        public bool CanExecute()
        {
            return this.employee.PrimaryKey.HasValue;
        }

        public void Execute()
        {
            if (this.CanExecute())
            {
                this.repository.Delete(this.employee);
            }
        }
    }
}
