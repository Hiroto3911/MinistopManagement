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
                var connectionString = "Data Source=LATI7480\\SQLEXPRESS;Initial Catalog=MinistopManagement;Integrated Security=True;";
                return new MinistopDataContextDataContext(connectionString);
            },new PerResolveLifetimeManager() );


            // UnitOfWork
            container.RegisterType<IUnitOfWork, BaseUnitOfWork>( new PerResolveLifetimeManager());
            container.RegisterType<IMinistopUnitOfWork, MinistopUnitOfWork>(new PerResolveLifetimeManager());

            // Repository
            container.RegisterType<IStoreRepository, StoreRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IEmployeeRepository, EmployeeRepository>();
            container.RegisterType<IStoreFixedExpenseRepository, StoreFixedExpenseRepository>();
            container.RegisterType<IAllowanceRepository, AllowanceRepository>(new PerResolveLifetimeManager());
            container.RegisterType<IShiftRepository, ShiftRepository>(new PerResolveLifetimeManager());

        }
    }
}
