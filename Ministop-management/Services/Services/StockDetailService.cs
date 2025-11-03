using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Services.Interfaces;
using Shared.Security;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class StockDetailService : IStockDetailService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IUserSession _userSession;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;

        public StockDetailService(IMinistopUnitOfWork ministopUnitOfWork, IUserSession userSession, IDateTimeService dateTimeService, IMapper mapper)
        {

            _ministopUnitOfWork = ministopUnitOfWork;
            _userSession = userSession;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
        }

        public PagedResult<IReadOnlyList<StockDetailDto>> GetStockDetails(string storeId, int pageNumber = 1, int pageSize = 20)
        {
            long totalCount = _ministopUnitOfWork.StockDetailRepository.GetCount(x => x.StoreID == storeId);
            var list = _ministopUnitOfWork.StockDetailRepository.GetPagedResponse((x => x.StoreID == storeId), pageNumber, pageSize);
            return new PagedResult<IReadOnlyList<StockDetailDto>>(list, pageNumber, pageSize, totalCount);

        }
    }
}
