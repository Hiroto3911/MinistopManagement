using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Infrastructure.UnitOfWorks;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Infrastructure
{
    public class ServicesRegistration
    {
        public static void AddInfrastructureTier(IUnityContainer container)
        {
            // Đăng ký UserSession toàn cục (Singleton)
            container.RegisterSingleton<IUserSession, UserSession>();

            // DBML DataContext (Transient)
            container.RegisterFactory<MinistopDataContextDataContext>(c =>
            {
                var connectionString = "Data Source=LAPTOP-2LEA9U48\\SQLEXPRESS;Initial Catalog=MinistopManagement;Integrated Security=True;";
                return new MinistopDataContextDataContext(connectionString);
            },new PerResolveLifetimeManager() );


            // UnitOfWork
            container.RegisterType<IUnitOfWork, BaseUnitOfWork>( new PerResolveLifetimeManager());
            container.RegisterType<IMinistopUnitOfWork, MinistopUnitOfWork>(new PerResolveLifetimeManager());

            // Repository
            container.RegisterType<IStoreRepository, StoreRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IEmployeeRepository, EmployeeRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IStoreFixedExpenseRepository, StoreFixedExpenseRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IAllowanceRepository, AllowanceRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IShiftRepository, ShiftRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IProductCategoryRepository, ProductCategoryRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IProductRepository, ProductRepository>(new PerResolveLifetimeManager());
            container.RegisterType<ISupplierRepository, SupplierRepository>(new PerResolveLifetimeManager());
            container.RegisterType<ISupplierProductRepository, SupplierProductRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IPromotionRepository, PromotionRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IPromotionProductRepository, PromotionProductReponsitory>(new PerResolveLifetimeManager());
            container.RegisterType<IReportRepository, ReportRepository>(new PerResolveLifetimeManager());

        }
    }
}
