using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Services.Interfaces;
using Shared.ErrorCode;
using Shared.Helpers;
using Shared.Security;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class StockImportDetailSerivce : IStockImportDetailSerivce
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StockImportDetailSerivce(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StockImportDetailDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StockImportDetailRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<StockImportDetailDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StockImportDetailDto>>(list);
        }


        public Result<StockImportDetailDto> GetStockImportDetailByID(string id)
        {
            try
            {
                var stockImportEntity = _ministopUnitOfWork.StockImportDetailRepository.Find(x => x.ImportID == id);
                if (stockImportEntity == null)
                {
                    return new Result<StockImportDetailDto>(ErrorCodeEnum.SID_ERR_001);
                }
                var result = _mapper.Map<StockImportDetailDto>(stockImportEntity);
                return new Result<StockImportDetailDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StockImportDetailDto>> GetStockImportDetail(string stockImportId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StockImportDetailRepository.GetCount(x => x.ImportID == stockImportId);
            var stockImportDetailEntity = _ministopUnitOfWork.StockImportDetailRepository.GetPagedResponse((x => x.ImportID == stockImportId), pageNumber, pageSize);
            var stockImportDetailDto = _mapper.Map<IReadOnlyList<StockImportDetailDto>>(stockImportDetailEntity);

            return new PagedResult<IReadOnlyList<StockImportDetailDto>>(stockImportDetailDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreatestockImportDetail(StockImportDetailDto StockImportDetailDto)
        {
            try
            {

                var stockImportDetailId = IdGenerator.CreateID("SID");
                var currentUserId = _userSession.UserId;
                StockImportDetailDto.Id = stockImportDetailId;
                var stockImportEntity = _mapper.Map<StockImportDetail>(StockImportDetailDto);
                var succeeded = _ministopUnitOfWork.StockImportDetailRepository.Add(stockImportEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdateStockImportDetail(StockImportDetailDto stockImportDetailEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var stockImportEntity = _ministopUnitOfWork.StockImportDetailRepository.Find(x => x.Id == stockImportDetailEdit.Id);
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_001);
                }
                stockImportEntity.Quantity = stockImportDetailEdit.Quantity;
                _ministopUnitOfWork.StockImportDetailRepository.Update(stockImportEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveStockImportDetail(string id)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var stockImportEntity = _ministopUnitOfWork.StockImportDetailRepository.Find(x => x.Id == id);
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_001);
                }
                _ministopUnitOfWork.StockImportDetailRepository.Delete(stockImportEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
    }
}
