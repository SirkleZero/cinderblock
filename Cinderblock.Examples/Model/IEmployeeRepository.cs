using System.Collections.Generic;

namespace Cinderblock.Examples.Model
{
    public interface IEmployeeRepository
    {
        void Insert(Employee employee);
        void Update(Employee employee);
        void Delete(Employee employee);
        IEnumerable<Employee> GetAllEmployees();
        Employee GetByPrimaryKey(int primaryKey);
    }
}
