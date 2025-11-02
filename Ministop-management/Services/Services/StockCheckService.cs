using AutoMapper;
using Domain.DTO;
using Domain.Entity;
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
    public class StockCheckService : IStockCheckService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StockCheckService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StockCheckDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StockImportRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<StockCheckDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StockCheckDto>>(list);
        }


        public Result<StockCheckDto> GetStockCheckByID(string id)
        {
            try
            {
                var stockCheckEntity = _ministopUnitOfWork.StockCheckRepository.Find(x => x.CheckID == id);
                if (stockCheckEntity == null)
                {
                    return new Result<StockCheckDto>(ErrorCodeEnum.SCT_ERR_001);
                }
                var result = _mapper.Map<StockCheckDto>(stockCheckEntity);
                return new Result<StockCheckDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StockCheckDto>> GetStockCheck(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StockCheckRepository.GetCount(x => x.StoreID == storeId);
            var stockCheckEntity = _ministopUnitOfWork.StockCheckRepository.GetPagedResponse((x => x.StoreID == storeId), pageNumber, pageSize);
            var stockImportsDto = _mapper.Map<IReadOnlyList<StockCheckDto>>(stockCheckEntity);

            return new PagedResult<IReadOnlyList<StockCheckDto>>(stockImportsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreatestockCheck(StockCheckDto StockCheckDto)
        {
            try
            {

                var stockExportID = IdGenerator.CreateID("SIT");
                var currentUserId = _userSession.UserId;
                StockCheckDto.CheckId = stockExportID;
                var stockCheckEntity = _mapper.Map<StockCheck>(StockCheckDto);
                var succeeded = _ministopUnitOfWork.StockCheckRepository.Add(stockCheckEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SCT_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdateStockCheck(StockCheckDto stockImportEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var stockCheckEntity = _ministopUnitOfWork.StockCheckRepository.Find(x => x.CheckID == stockImportEdit.CheckId);
                if (stockCheckEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SCT_ERR_001);
                }
                stockCheckEntity.Status = stockImportEdit.Status;
                _ministopUnitOfWork.StockCheckRepository.Update(stockCheckEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveStockCheck(string id)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var stockCheckEntity = _ministopUnitOfWork.StockCheckRepository.Find(x => x.CheckID == id);
                if (stockCheckEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SCT_ERR_001);
                }
                _ministopUnitOfWork.StockCheckRepository.Delete(stockCheckEntity, true);
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
