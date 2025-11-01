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
    public class StockExportDetailService : IStockExportDetailService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StockExportDetailService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StockExportDetailDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StockExportDetailRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<StockExportDetailDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StockExportDetailDto>>(list);
        }


        public Result<StockExportDetailDto> GetStockExportDetailByID(string id)
        {
            try
            {
                var stockExportEntity = _ministopUnitOfWork.StockExportDetailRepository.Find(x => x.Id == id);
                if (stockExportEntity == null)
                {
                    return new Result<StockExportDetailDto>(ErrorCodeEnum.SED_ERR_001);
                }
                var result = _mapper.Map<StockExportDetailDto>(stockExportEntity);
                return new Result<StockExportDetailDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StockExportDetailDto>> GetStockExportDetail(string stockExportId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StockExportDetailRepository.GetCount(x => x.ExportID == stockExportId);
            var stockExportDetailEntity = _ministopUnitOfWork.StockExportDetailRepository.GetPagedResponse((x => x.ExportID == stockExportId), pageNumber, pageSize);
            var stockExportDetailDto = _mapper.Map<IReadOnlyList<StockExportDetailDto>>(stockExportDetailEntity);

            return new PagedResult<IReadOnlyList<StockExportDetailDto>>(stockExportDetailDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreatestockExportDetaill(StockExportDetailDto StockExportDetailDto)
        {
            try
            {

                var stockImportDetailId = IdGenerator.CreateID("SED");
                var currentUserId = _userSession.UserId;
                StockExportDetailDto.Id = stockImportDetailId;
                var stockExportEntity = _mapper.Map<StockExportDetail>(StockExportDetailDto);
                var succeeded = _ministopUnitOfWork.StockExportDetailRepository.Add(stockExportEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SED_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdateStockExportDetail(StockExportDetailDto stockExportDetailEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var stockExportEntity = _ministopUnitOfWork.StockExportDetailRepository.Find(x => x.Id == stockExportDetailEdit.Id);
                if (stockExportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SED_ERR_001);
                }
                stockExportEntity.Quantity = stockExportDetailEdit.Quantity;
                _ministopUnitOfWork.StockExportDetailRepository.Update(stockExportEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveStockExportDetail(string id)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var stockExportEntity = _ministopUnitOfWork.StockExportDetailRepository.Find(x => x.Id == id);
                if (stockExportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SED_ERR_001);
                }
                _ministopUnitOfWork.StockExportDetailRepository.Delete(stockExportEntity, true);
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
