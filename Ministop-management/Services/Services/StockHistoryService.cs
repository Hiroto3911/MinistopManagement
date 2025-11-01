using AutoMapper;
using Domain.DTO;
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
    public class StockHistoryService : IStockHistoryService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IUserSession _userSession;
        private readonly IDateTimeService _dateTimeService;
        private readonly IMapper _mapper;

        public StockHistoryService(IMinistopUnitOfWork ministopUnitOfWork, IUserSession userSession, IDateTimeService dateTimeService, IMapper mapper)
        {

            _ministopUnitOfWork = ministopUnitOfWork;
            _userSession = userSession;
            _dateTimeService = dateTimeService;
            _mapper = mapper;
        }

        public PagedResult<IReadOnlyList<StockHistoryDto>> GetStockHistory(string stockDetailId, int pageNumber = 1, int pageSize = 20)
        {
            long totalCount = _ministopUnitOfWork.StockDetailRepository.GetCount(x => x.StockDetailID == stockDetailId);
            var list = _ministopUnitOfWork.StockHistoryRepository.GetPagedResponse((x => x.StockDetailID == stockDetailId), pageNumber, pageSize);
            var stockHistoryDtos = _mapper.Map<IReadOnlyList<StockHistoryDto>>(list);
            return new PagedResult<IReadOnlyList<StockHistoryDto>>(stockHistoryDtos, pageNumber, pageSize, totalCount);

        }
    }
}
