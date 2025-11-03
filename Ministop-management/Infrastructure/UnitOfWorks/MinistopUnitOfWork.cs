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
            ISupplierRepository supplierRepository,
            IReportRepository reportRepository,
             IStockDetailRepository stockDetailRepository,
            IStockHistoryRepository stockHistoryRepository,
            IStockImportRepository stockImportRepository,
            IStockImportDetailRepository stockImportDetailRepository,
            IStockExportRepository stockExportRepository,
            IStockExportDetailRepository stockExportDetailRepository,
            IStockCheckRepository stockCheckRepository,
            IStockCheckDetailRepository stockCheckDetailRepository,
            ISupplierProductRepository supplierProductRepository,
            IPromotionRepository promotionRepository,
            IPromotionProductRepository promotionProductRepository

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
            SupplierProductRepository = supplierProductRepository;
            ReportRepository = reportRepository;
            PromotionRepository = promotionRepository;
            PromotionProductRepository = promotionProductRepository;
            ReportRepository = reportRepository;
            StockDetailRepository = stockDetailRepository;
            StockHistoryRepository = stockHistoryRepository;
            StockImportRepository = stockImportRepository;
            StockImportDetailRepository = stockImportDetailRepository;
            StockExportRepository = stockExportRepository;
            StockExportDetailRepository = stockExportDetailRepository;
            StockCheckRepository = stockCheckRepository;
            StockCheckDetailRepository = stockCheckDetailRepository;

        }
        public IStoreRepository StoreRepository { get; private set; }

        public IStoreFixedExpenseRepository FixedExpenseRepository { get; private set; }

        public IEmployeeRepository EmployeeRepository { get; private set; }

        public IAllowanceRepository AllowanceRepository { get; private set; }

        public IShiftRepository ShiftRepository { get; private set; }
        public IProductCategoryRepository ProductCategoryRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public ISupplierRepository SupplierRepository { get; private set; }
        public ISupplierProductRepository SupplierProductRepository { get; private set; }
        public IPromotionRepository PromotionRepository { get; private set; }
        public IPromotionProductRepository PromotionProductRepository { get; private set; }

        public IReportRepository ReportRepository { get; private set; }

        public IStockDetailRepository StockDetailRepository { get; private set; }

        public IStockHistoryRepository StockHistoryRepository { get; private set; }

        public IStockImportRepository StockImportRepository { get; private set; }

        public IStockImportDetailRepository StockImportDetailRepository { get; private set; }

        public IStockExportRepository StockExportRepository { get; private set; }

        public IStockExportDetailRepository StockExportDetailRepository { get; private set; }

        public IStockCheckRepository StockCheckRepository { get; private set; }

        public IStockCheckDetailRepository StockCheckDetailRepository { get; private set; }
    }
}
