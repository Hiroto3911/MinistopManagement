using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class MinistopUnitOfWork : BaseUnitOfWork, IMinistopUnitOfWork
    {
        public MinistopUnitOfWork(
            MinistopDataContextDataContext context,
            IStoreRepository storeRepository,
            IStoreFixedExpenseRepository storeFixedExpenseRepository,
            IEmployeeRepository employeeRepository,
            IAllowanceRepository allowanceRepository,
            IShiftRepository shiftRepository,
            IProductCategoryRepository productCategoryRepository,
            IProductRepository productRepository,
            ISupplierRepository supplierRepository
            ) : base(context)
        {
            StoreRepository = storeRepository;
            FixedExpenseRepository = storeFixedExpenseRepository;
            EmployeeRepository = employeeRepository;
            AllowanceRepository = allowanceRepository;
            ShiftRepository = shiftRepository;
            ProductCategoryRepository = productCategoryRepository;
            ProductRepository = productRepository;
            SupplierRepository = supplierRepository;
        }
        public IStoreRepository StoreRepository { get; private set; }

        public IStoreFixedExpenseRepository FixedExpenseRepository { get; private set; }

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public IAllowanceRepository AllowanceRepository { get; private set; }

        public IShiftRepository ShiftRepository { get; private set; }
        public IProductCategoryRepository ProductCategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ISupplierRepository SupplierRepository { get; private set; }
    }
}
