using System;

namespace Cinderblock.Examples.Model.Commands
{
    public sealed class SaveEmployeeCommand : ICommand
    {
        private readonly Employee employee;
        private readonly IEmployeeRepository repository;

        public SaveEmployeeCommand(Employee employee, IEmployeeRepository repository)
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
            return true;
        }

        public void Execute()
        {
            if (this.CanExecute())
            {
                if (this.employee.PrimaryKey.HasValue)
                {
                    this.repository.Update(this.employee);
                }
                else
                {
                    this.repository.Insert(this.employee);
                }
            }
        }
    }
}
