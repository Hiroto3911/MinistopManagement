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
    public class StockCheckDetailService : IStockCheckDetailService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StockCheckDetailService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StockCheckDetailDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StockCheckDetailRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<StockCheckDetailDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StockCheckDetailDto>>(list);
        }


        public Result<StockCheckDetailDto> GetStockCheckDetailByID(string id)
        {
            try
            {
                var stockCheckEntity = _ministopUnitOfWork.StockCheckDetailRepository.Find(x => x.Id == id);
                if (stockCheckEntity == null)
                {
                    return new Result<StockCheckDetailDto>(ErrorCodeEnum.SCD_ERR_001);
                }
                var result = _mapper.Map<StockCheckDetailDto>(stockCheckEntity);
                return new Result<StockCheckDetailDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StockCheckDetailDto>> GetStockCheckDetail(string stockCheckID, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StockCheckDetailRepository.GetCount(x => x.CheckID == stockCheckID);
            var stockCheckDetailEntity = _ministopUnitOfWork.StockCheckDetailRepository.GetPagedResponse((x => x.CheckID == stockCheckID), pageNumber, pageSize);
            var stockImportDetailDto = _mapper.Map<IReadOnlyList<StockCheckDetailDto>>(stockCheckDetailEntity);

            return new PagedResult<IReadOnlyList<StockCheckDetailDto>>(stockImportDetailDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateStockCheckDetail(StockCheckDetailDto stockCheckDetailDto)
        {
            try
            {

                var stockCheckDetailId = IdGenerator.CreateID("SCD");
                var currentUserId = _userSession.UserId;
                stockCheckDetailDto.Id = stockCheckDetailId;
                var stockCheckDetailEntity = _mapper.Map<StockCheckDetail>(stockCheckDetailDto);
                var succeeded = _ministopUnitOfWork.StockCheckDetailRepository.Add(stockCheckDetailEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SCD_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdateStockCheckDetail(StockCheckDetailDto stockCheckDetailEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var stockImportEntity = _ministopUnitOfWork.StockCheckDetailRepository.Find(x => x.Id == stockCheckDetailEdit.Id);
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SCD_ERR_001);
                }
                stockImportEntity.QuantityActual = stockCheckDetailEdit.QuantityActual;
                _ministopUnitOfWork.StockCheckDetailRepository.Update(stockImportEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveStockCheckDetail(string id)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var stockImportEntity = _ministopUnitOfWork.StockCheckDetailRepository.Find(x => x.Id == id);
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SCD_ERR_001);
                }
                _ministopUnitOfWork.StockCheckDetailRepository.Delete(stockImportEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemoveRangeStockCheckDetailByCheckID(string checkID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {


                var stockCheckEntities = _ministopUnitOfWork.StockCheckDetailRepository.GetAll((x => x.CheckID == checkID));
                if (stockCheckEntities == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_001);
                }
                _ministopUnitOfWork.StockCheckDetailRepository.DeleteRange(stockCheckEntities, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> Any(string checkId)
        {
            try
            {
                bool isChecked = _ministopUnitOfWork.StockCheckDetailRepository.Any((x => x.CheckID == checkId));
                return new Result<bool>(isChecked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
