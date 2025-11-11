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
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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

        public Result<int> GetCount(string storeID, DateTime dateNow)
        {
            // Lấy  từ DBML (entity của LINQ to SQL)
            var importList = _ministopUnitOfWork.StockImportRepository
                     .GetAll(x => x.StoreID == storeID
                               && x.ImportDate.Month == dateNow.Month
                               && x.ImportDate.Year == dateNow.Year);

            if (!importList.Any())
                return new Result<int>(0);

            var importIDs = importList.Select(x => x.ImportID).ToList();

            // Đếm tổng số dòng chi tiết nhập ứng với tất cả ImportID
            var totalCount = _ministopUnitOfWork.StockImportDetailRepository
                                .GetCount(x => importIDs.Contains(x.ImportID));

            return new Result<int>(totalCount);
        }
        public PagedResult<IReadOnlyList<StockImportDetailDto>> GetImportDetails(string storeID, int month, int year, int pageNumber, int pageSize)
        {
            var exportList = _ministopUnitOfWork.StockImportRepository
                   .GetAll(x => x.StoreID == storeID
                             && x.ImportDate.Month == month
                             && x.ImportDate.Year == year);
            var importIDs = exportList.Select(x => x.ImportID).ToList();
            var totalCount = _ministopUnitOfWork.StockImportDetailRepository
                                .GetCount(x => importIDs.Contains(x.ImportID));
            var list = _ministopUnitOfWork.StockImportDetailRepository
                                .GetPagedResponse(x => importIDs.Contains(x.ImportID), pageNumber, pageSize);

            return new PagedResult<IReadOnlyList<StockImportDetailDto>>(list, pageNumber, pageSize, totalCount);
        }
        public Result<StockImportDetailDto> GetStockImportDetailByID(string id)
        {
            try
            {
                var stockImportEntity = _ministopUnitOfWork.StockImportDetailRepository.Find(x => x.Id == id);
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
                var isDuplicate = _ministopUnitOfWork.StockImportDetailRepository.Any(x=> x.ImportID == StockImportDetailDto.ImportId && x.ProductID == StockImportDetailDto.ProductId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_002);
                }
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
        public Result<bool> RemoveRangeStockImportDetailByImportID(string importID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                
                var stockImportEntity = _ministopUnitOfWork.StockImportDetailRepository.GetAll((x=> x.ImportID == importID));
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SID_ERR_001);
                }
                _ministopUnitOfWork.StockImportDetailRepository.DeleteRange(stockImportEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> Any(string importId)
        {
            try
            {
                bool isChecked = _ministopUnitOfWork.StockImportDetailRepository.Any((x => x.ImportID == importId));
                return new Result<bool>(isChecked);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
