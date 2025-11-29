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
        IProductCategoryRepository ProductCategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        ISupplierProductRepository SupplierProductRepository { get; }
        IPromotionRepository PromotionRepository { get; }
        IPromotionProductRepository PromotionProductRepository { get; }
        IReportRepository ReportRepository { get; }

        IStockDetailRepository StockDetailRepository { get; }
        IStockHistoryRepository StockHistoryRepository { get; }
        IStockImportRepository StockImportRepository { get; }
        IStockImportDetailRepository StockImportDetailRepository { get; }
        IStockExportRepository StockExportRepository { get; }
        IStockExportDetailRepository StockExportDetailRepository { get; }
        IStockCheckRepository StockCheckRepository { get; }
        IStockCheckDetailRepository StockCheckDetailRepository { get; }
        IInvoiceRepository InvoiceRepository { get; }
        IInvoiceDetailRepository InvoiceDetailRepository { get; }
        ISalaryContractRepository SalaryContractRepository { get; }
        IShiftAssignmentRepository ShiftAssignmentRepository { get; }
        ISalaryRepository SalaryRepository { get; }
        ISalaryContractAllowanceRepository SalaryContractAllowanceRepository { get; }
        IAbsenceRepository AbsenceRepository { get; }
        IReturnProductRepository ReturnProductRepository { get; }
        IReturnDetailRepository ReturnDetailRepository { get; }
        IPriceProposalRepository PriceProposalRepository { get; }
    }
}
