
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
            container.RegisterType<IStoreService, StoreService>(new PerResolveLifetimeManager());
            container.RegisterType<IAllowanceService, AllowanceService>(new PerResolveLifetimeManager());
            container.RegisterType<IShiftService, ShiftService>(new PerResolveLifetimeManager());
            Infrastructure.ServicesRegistration.AddInfrastructureTier(container);
        }
    }
}
