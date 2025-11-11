using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Services.Interfaces;
using Shared.ErrorCode;
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
        //n
        public Result<StockDetailDto> GetStockDetailByProductID(string id)
        {
            try
            {
                var search = _ministopUnitOfWork.StockDetailRepository.Find(x =>x.StoreID ==_userSession.IdStore && x.ProductID == id);
                if (search == null) return new Result<StockDetailDto>(ErrorCodeEnum.SDD_ERR_001);
                var stockDetailDto = _mapper.Map<StockDetailDto>(search);
                return new Result<StockDetailDto>(stockDetailDto);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<IReadOnlyList<StockDetailDto>> GetStockDetailByProductName(string productName)
        {
            try
            {
                // Tìm tất cả sản phẩm có tên chứa từ khóa
                var products = _ministopUnitOfWork.ProductRepository
                    .GetAll(p => p.ProductName.Contains(productName) && !p.IsDeleted)
                    .Select(p => new { p.ProductID, p.ProductName })
                    .ToList();

                if (!products.Any())
                    return new Result<IReadOnlyList<StockDetailDto>>(ErrorCodeEnum.SDD_ERR_001);

                // Lấy danh sách productId
                var productIds = products.Select(p => p.ProductID).ToList();

                // Lấy StockDetail tương ứng với các ProductId đó
                var stockDetails = _ministopUnitOfWork.StockDetailRepository
                    .GetAll(x => x.StoreID == _userSession.IdStore && productIds.Contains(x.ProductID))
                    .Select(x => new StockDetailDto
                    {
                        StoreId = x.StoreID,
                        StockDetailId = x.StockDetailID,
                        ProductId = x.ProductID,
                        ProductName = products.First(p => p.ProductID == x.ProductID).ProductName,
                        Quantity = x.Quantity,
                        Price = x.Price,
                        LastUpdate = x.LastUpdate
                    })
                    .ToList();

                if (!stockDetails.Any())
                    return new Result<IReadOnlyList<StockDetailDto>>(ErrorCodeEnum.SDD_ERR_001);

                return new Result<IReadOnlyList<StockDetailDto>>(stockDetails);
            }
            catch (Exception ex)
            {
                throw ex; // Không throw ex để tránh mất stacktrace
            }
        }

    }
}
