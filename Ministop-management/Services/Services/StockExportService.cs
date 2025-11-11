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
    public class StockExportService : IStockExportService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StockExportService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StockExportDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StockImportRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<StockExportDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StockExportDto>>(list);
        }


        public Result<StockExportDto> GetStockExportByID(string id)
        {
            try
            {
                var stockExportEntity = _ministopUnitOfWork.StockExportRepository.Find(x => x.ExportID == id);
                if (stockExportEntity == null)
                {
                    return new Result<StockExportDto>(ErrorCodeEnum.SET_ERR_001);
                }
                var result = _mapper.Map<StockExportDto>(stockExportEntity);
                return new Result<StockExportDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StockExportDto>> GetStockExport(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StockExportRepository.GetCount(x => x.StoreID == storeId);
            var stockExportEntity = _ministopUnitOfWork.StockExportRepository.GetPagedResponse((x => x.StoreID == storeId), pageNumber, pageSize);
            var stockImportsDto = _mapper.Map<IReadOnlyList<StockExportDto>>(stockExportEntity);

            return new PagedResult<IReadOnlyList<StockExportDto>>(stockImportsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreatestockExport(StockExportDto StockExportDto)
        {
            try
            {

                var stockExportID = IdGenerator.CreateID("SET");
                var currentUserId = _userSession.UserId;
                StockExportDto.ExportId = stockExportID;
                var stockExportEntity = _mapper.Map<StockExport>(StockExportDto);
                var succeeded = _ministopUnitOfWork.StockExportRepository.Add(stockExportEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SET_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //n
        public Result<bool> UpdateStockExport(StockExportDto stockExportEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var stockExportEntity = _ministopUnitOfWork.StockExportRepository.Find(x => x.ExportID == stockExportEdit.ExportId);
                if (stockExportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SET_ERR_001);
                }
                stockExportEntity.Status = stockExportEdit.Status;
                stockExportEntity.TypeExport = stockExportEdit.TypeExport;
                stockExportEntity.Reason = stockExportEdit.Reason;
                _ministopUnitOfWork.StockExportRepository.Update(stockExportEntity, true);
                if (stockExportEdit.Status == 1)
                {
                    var list = _ministopUnitOfWork.StockExportDetailRepository.GetAll(x => x.ExportID == stockExportEntity.ExportID);
                    if (list == null || list.Count < 0) return new Result<bool>(ErrorCodeEnum.SET_ERR_005);
                    foreach (var item in list)
                    {
                        var result = _ministopUnitOfWork.StockDetailRepository.FindByID(x => x.ProductID == item.ProductID);
                        if (result == null) continue;
                        result.Quantity -= item.Quantity;
                        result.LastUpdate = _dateTimeService.NowUtc;
                        _ministopUnitOfWork.StockDetailRepository.Update(result, true);
                        CreateHistoryEntity(stockExportEntity.ExportID, result.StockDetailID, item.Quantity);
                    }
                }

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        //N
        private void CreateHistoryEntity(string refID, string stockDetailID, int quantityChange)
        {
            string id = IdGenerator.CreateID("STH");
            var entity = new StockHistory()
            {
                StockHistoryID = id,
                StockDetailID = stockDetailID,
                QuantityChange = quantityChange,
                RefID = refID,
                ChangeDate = _dateTimeService.NowUtc,
                ChangeType = "Xuất"
            };
            _ministopUnitOfWork.StockHistoryRepository.Add(entity);
        }
        public Result<bool> RemoveStockExport(string id)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var stockExportEntity = _ministopUnitOfWork.StockExportRepository.Find(x => x.ExportID == id);
                if (stockExportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SET_ERR_001);
                }
                _ministopUnitOfWork.StockExportRepository.Delete(stockExportEntity, true);
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
