
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
            Infrastructure.ServicesRegistration.AddInfrastructureTier(container);
        }
    }
}
