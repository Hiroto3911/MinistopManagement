using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IMinistopUnitOfWork : IUnitOfWork 
    {
        IStoreRepository StoreRepository { get; }
        IStoreFixedExpenseRepository FixedExpenseRepository { get; }
        IEmployeeRepository EmployeeRepository { get; }
        IAllowanceRepository AllowanceRepository { get; }

        IShiftRepository ShiftRepository { get; }
    }
}
