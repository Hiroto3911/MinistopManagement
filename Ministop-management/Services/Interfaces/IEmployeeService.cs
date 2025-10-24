using Domain.DTO;
using Infrastructure.Data;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IEmployeeService
    {
        Result<IReadOnlyList<EmployeeDto>> GetAll();
        Result<IReadOnlyList<EmployeeDto>> GetAllEmployeeIsDelete();
        Result<EmployeeDto> GetEmployeeByID(string id);
        PagedResult<IReadOnlyList<EmployeeDto>> GetEmployee(int pageNumber, int pageSize);
        Result<bool> CreateEmployee(EmployeeDto employeeDto);
        Result<bool> UpdateEmployee(EmployeeDto employeeEdit);
        Result<bool> RemoveEmployee(string employeeId);
        Result<bool> RestoreEmployee(List<string> listRestoreId);
        PagedResult<IReadOnlyList<EmployeeDto>> GetEmployeeByStore(string storeId, int pageNumber, int pageSize);
        Result<bool> AnyStore(string storeId);
    }
}
