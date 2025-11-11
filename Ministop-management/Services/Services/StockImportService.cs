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
    public class StockImportService : IStockImportService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StockImportService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StockImportDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StockImportRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<StockImportDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StockImportDto>>(list);
        }


        public Result<StockImportDto> GetStockImportByID(string id)
        {
            try
            {
                var stockImportEntity = _ministopUnitOfWork.StockImportRepository.Find(x => x.ImportID == id);
                if (stockImportEntity == null)
                {
                    return new Result<StockImportDto>(ErrorCodeEnum.SIT_ERR_001);
                }
                var result = _mapper.Map<StockImportDto>(stockImportEntity);
                return new Result<StockImportDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StockImportDto>> GetStockImport(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StockImportRepository.GetCount(x => x.StoreID == storeId);
            var stockImportEntity = _ministopUnitOfWork.StockImportRepository.GetPagedResponse((x=> x.StoreID == storeId),pageNumber, pageSize);
            var stockImportsDto = _mapper.Map<IReadOnlyList<StockImportDto>>(stockImportEntity);

            return new PagedResult<IReadOnlyList<StockImportDto>>(stockImportsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreatestockImport(StockImportDto StockImportDto)
        {
            try
            {
               
                var stockImportId = IdGenerator.CreateID("SIT");
                var currentUserId = _userSession.UserId;
                StockImportDto.ImportID = stockImportId;
                var stockImportEntity = _mapper.Map<StockImport>(StockImportDto);
                var succeeded = _ministopUnitOfWork.StockImportRepository.Add(stockImportEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SIT_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdateStockImport(StockImportDto stockImportEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var stockImportEntity = _ministopUnitOfWork.StockImportRepository.Find(x => x.ImportID == stockImportEdit.ImportID);
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SIT_ERR_001);
                }
                stockImportEntity.Status = stockImportEdit.Status;
                stockImportEntity.Note = stockImportEdit.Note;
                _ministopUnitOfWork.StockImportRepository.Update(stockImportEntity, true);
                if (stockImportEdit.Status == 4)
                {
                    var list = _ministopUnitOfWork.StockImportDetailRepository.GetAll(x => x.ImportID == stockImportEntity.ImportID);
                    if (list == null || list.Count < 0) return new Result<bool>(ErrorCodeEnum.SET_ERR_005);
                    foreach (var item in list)
                    {
                        var result = _ministopUnitOfWork.StockDetailRepository.FindByID(x => x.ProductID == item.ProductID);
                        string detailID;
                        if (result == null) {
                            detailID = IdGenerator.CreateID("SDD");
                            var stockDetail = new StockDetail() {StockDetailID = detailID, ProductID = item.ProductID ,Quantity = item.Quantity,Price = item.UnitPrice, LastUpdate = _dateTimeService.NowUtc, StoreID = stockImportEdit.StoreId};
                            _ministopUnitOfWork.StockDetailRepository.Add(stockDetail);
                        }
                        else
                        {
                            detailID = result.StockDetailID;
                            result.Quantity += item.Quantity;
                            result.LastUpdate = _dateTimeService.NowUtc;

                            _ministopUnitOfWork.StockDetailRepository.Update(result, true);
                        }
                        CreateHistoryEntity(stockImportEntity.ImportID, detailID, item.Quantity);
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
                ChangeType = "Nhập"
            };
            _ministopUnitOfWork.StockHistoryRepository.Add(entity);
        }
        public Result<bool> RemoveStockImport(string id)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var stockImportEntity = _ministopUnitOfWork.StockImportRepository.Find(x => x.ImportID == id);
                if (stockImportEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SIT_ERR_001);
                }
                _ministopUnitOfWork.StockImportRepository.Delete(stockImportEntity, true);
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
