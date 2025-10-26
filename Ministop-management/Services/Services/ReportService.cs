using Domain.DTO;
using Infrastructure.Interfaces;
using Infrastructure.UnitOfWorks;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;

        public ReportService(IMinistopUnitOfWork ministopUnitOfWork)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
        }
        public List<StoreDto> GetStoreByRegion(string region)
        {
            var list = _ministopUnitOfWork.ReportRepository.GetStoreByRegion(region);
            var stores = list.Select(x => new StoreDto { StoreId = x.StoreID, StoreName = x.StoreName, Address = x.Address, Phone = x.Phone });
            return stores.ToList();
        }
    }
}
