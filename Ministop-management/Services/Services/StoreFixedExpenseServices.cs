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
    public class StoreFixedExpenseServices : IStoreFixedExpenseServices
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StoreFixedExpenseServices(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StoreFixedExpenseDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.FixedExpenseRepository.GetAll();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<StoreFixedExpenseDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StoreFixedExpenseDto>>(list);
        }
        public Result<IReadOnlyList<StoreFixedExpenseDto>> GetAllStoreFixedExpenseIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.FixedExpenseRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<StoreFixedExpenseDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StoreFixedExpenseDto>>(list);
        }
        public Result<StoreFixedExpenseDto> GetStoreFixedExpenseByID(string id)
        {
            try
            {
                var storeEntity = _ministopUnitOfWork.FixedExpenseRepository.Find(x => x.ExpenseID == id);
                if (storeEntity == null)
                {
                    return new Result<StoreFixedExpenseDto>(ErrorCodeEnum.SFE_ERR_001);
                }
                var result = _mapper.Map<StoreFixedExpenseDto>(storeEntity);
                return new Result<StoreFixedExpenseDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StoreFixedExpenseDto>> GetStoreFixedExpense(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.FixedExpenseRepository.GetCount(x => !x.IsDeleted);
            var storesEntity = _ministopUnitOfWork.FixedExpenseRepository.GetPagedResponse(pageNumber, pageSize);
            var storesDto = _mapper.Map<IReadOnlyList<StoreFixedExpenseDto>>(storesEntity);

            return new PagedResult<IReadOnlyList<StoreFixedExpenseDto>>(storesDto, pageNumber, pageSize, totalCount);
        }
        public PagedResult<IReadOnlyList<StoreFixedExpenseDto>> GetStoreFixedExpense(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.FixedExpenseRepository.GetCount(x => x.StoreID == storeId && !x.IsDeleted);
            var storesDto = _ministopUnitOfWork.FixedExpenseRepository.GetPagedResponse((x => x.StoreID == storeId && !x.IsDeleted), pageNumber, pageSize);
            return new PagedResult<IReadOnlyList<StoreFixedExpenseDto>>(storesDto, pageNumber, pageSize, totalCount);
        }
        public Result<string> CreateStoreFixedExpense(StoreFixedExpenseDto expenseDto)
        {
            try
            {
                if (_userSession.Role != "Quản lý cửa hàng") return new Result<string>(ErrorCodeEnum.SFE_ERR_007);
                var isDuplicated = _ministopUnitOfWork.FixedExpenseRepository.Any(x => x.MonthYear == expenseDto.MonthYear && x.StoreID == expenseDto.StoreId);
                if (isDuplicated)
                {
                    return new Result<string>(ErrorCodeEnum.SFE_ERR_008);
                }
                var expenseId = IdGenerator.CreateID("SFE");
                var currentUserId = _userSession.UserId;
                expenseDto.ExpenseId = expenseId;

                expenseDto.Created = _dateTimeService.NowUtc;
                expenseDto.CreatedBy = currentUserId;
                var expenseEntity = _mapper.Map<StoreFixedExpense>(expenseDto);
                var succeeded = _ministopUnitOfWork.FixedExpenseRepository.Add(expenseEntity);
                if (succeeded == null)
                {
                    return new Result<string>(ErrorCodeEnum.SFE_ERR_003);
                }
                return new Result<string>(succeeded.ExpenseID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> RestoreStoreFixedExpense(List<string> listExpensId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var stores = _ministopUnitOfWork.FixedExpenseRepository.GetAllIsDelete();
                var filter = stores.Where(x => listExpensId.Contains(x.ExpenseID.ToString())).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.SFE_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.FixedExpenseRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<string> UpdateStoreFixedExpense(StoreFixedExpenseDto expenseEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var expenseEntity = _ministopUnitOfWork.FixedExpenseRepository.Find(x => x.ExpenseID == expenseEdit.ExpenseId);
                if (expenseEntity == null)
                {
                    return new Result<string>(ErrorCodeEnum.STR_ERR_001);
                }
                expenseEntity.RentCost = expenseEdit.RentCost;
                expenseEntity.ElectricityCost = expenseEdit.ElectricityCost;
                expenseEntity.WaterCost = expenseEdit.WaterCost;
                expenseEntity.Note  = expenseEdit.Note;
                expenseEntity.Status = expenseEdit.Status;
                expenseEntity.LastModified = _dateTimeService.NowUtc;
                expenseEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.FixedExpenseRepository.Update(expenseEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<string>(expenseEntity.ExpenseID);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemoveStoreFixedExpense(string expenseId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var expenseEntity = _ministopUnitOfWork.FixedExpenseRepository.Find(x => x.ExpenseID == expenseId);
                if (expenseEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFE_ERR_001);
                }
                //expenseEntity.LastModified = _dateTimeService.NowUtc;
                //expenseEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.FixedExpenseRepository.Delete(expenseEntity, true);
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
