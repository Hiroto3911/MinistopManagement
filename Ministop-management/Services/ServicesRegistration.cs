
using AutoMapper;
using Services.Interfaces;
using Services.Services;
using System.Configuration.Assemblies;
using System.Reflection;
using Unity;
using Unity.Lifetime;

namespace Services
{
    public class ServicesRegistration
    {
        public static void AddServiceTier(IUnityContainer container)
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(Assembly.GetExecutingAssembly());
            });
            var mapper = mapperConfig.CreateMapper();
            container.RegisterInstance<IMapper>(mapper);
            container.RegisterSingleton<IDateTimeService, DateTimeService>();
            container.RegisterType<IIdentityService, IdentityService>();
            container.RegisterType<IEmployeeService, EmployeeService>(new PerResolveLifetimeManager());
            container.RegisterType<IStoreService, StoreService>(new PerResolveLifetimeManager());
            container.RegisterType<IAllowanceService, AllowanceService>(new PerResolveLifetimeManager());
            container.RegisterType<IShiftService, ShiftService>(new PerResolveLifetimeManager());
            container.RegisterType<IStoreFixedExpenseServices, StoreFixedExpenseServices>(new PerResolveLifetimeManager());
            container.RegisterType<IProductCategoryService, ProductCategoryService>(new PerResolveLifetimeManager());
            container.RegisterType<IProductService, ProductService>(new PerResolveLifetimeManager());
            container.RegisterType<ISupplierService, SupplierService>(new PerResolveLifetimeManager());
            container.RegisterType<IReportService, ReportService>(new PerResolveLifetimeManager());
<<<<<<< HEAD
            container.RegisterType<ISalaryContractService, SalaryContractService>(new PerResolveLifetimeManager());
            container.RegisterType<IShiftAssignmentService, ShiftAssignmentService>(new PerResolveLifetimeManager());
            container.RegisterType<ISalaryService, SalaryService>(new PerResolveLifetimeManager());
            container.RegisterType<ISalaryContractAllowanceService, SalaryContractAllowanceService>(new PerResolveLifetimeManager());
            container.RegisterType<IAbsenceService, AbsenceService>(new PerResolveLifetimeManager());
=======
            container.RegisterType<IStockDetailService, StockDetailService>(new PerResolveLifetimeManager());
            container.RegisterType<IStockHistoryService, StockHistoryService>(new PerResolveLifetimeManager());
            container.RegisterType<IStockImportService, StockImportService>(new PerResolveLifetimeManager());
            container.RegisterType<IStockImportDetailSerivce, StockImportDetailSerivce>(new PerResolveLifetimeManager());
            container.RegisterType<IStockExportService, StockExportService>(new PerResolveLifetimeManager());
            container.RegisterType<IStockExportDetailService, StockExportDetailService>(new PerResolveLifetimeManager());
            container.RegisterType<IStockCheckService, StockCheckService>(new PerResolveLifetimeManager());
            container.RegisterType<IStockCheckDetailService, StockCheckDetailService>(new PerResolveLifetimeManager());
            container.RegisterType<ISupplierProductService, SupplierProductService>(new PerResolveLifetimeManager());
            container.RegisterType<IPromotionService, PromotionService>(new PerResolveLifetimeManager());
            container.RegisterType<IPromotionProductService, PromotionProductService>(new PerResolveLifetimeManager());
            container.RegisterType<IInvoiceService, InvoiceService>(new PerResolveLifetimeManager());
            container.RegisterType<IInvoiceDetailService, InvoiceDetailService>(new PerResolveLifetimeManager());
>>>>>>> develop
            Infrastructure.ServicesRegistration.AddInfrastructureTier(container);
        }
    }
}
