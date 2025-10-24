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
using System.Threading;
using System.Threading.Tasks;

namespace Services.Services
{
    public class StoreService : IStoreService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public StoreService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<StoreDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StoreRepository.GetAll(x => !x.IsDeleted);

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<StoreDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StoreDto>>(list);
        }
        public Result<IReadOnlyList<StoreDto>> GetAllStoreIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.StoreRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<StoreDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<StoreDto>>(list);
        }
        public Result<StoreDto> GetStoreByID(string id)
        {
            try
            {
                var storeEntity = _ministopUnitOfWork.StoreRepository.Find(x => x.StoreID == id);
                if (storeEntity == null)
                {
                    return new Result<StoreDto>(ErrorCodeEnum.STR_ERR_001);
                }
                var result = _mapper.Map<StoreDto>(storeEntity);
                return new Result<StoreDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StoreDto>> GetStore(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.StoreRepository.GetCount(x => !x.IsDeleted);
            var storesEntity = _ministopUnitOfWork.StoreRepository.GetPagedResponse(pageNumber, pageSize);
            var storesDto = _mapper.Map<IReadOnlyList<StoreDto>>(storesEntity);

            return new PagedResult<IReadOnlyList<StoreDto>>(storesDto, pageNumber, pageSize, totalCount);
        }
        public Result<bool> CreateStore(StoreDto storeDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.StoreRepository.Any(x => x.StoreName == storeDto.StoreName && x.Phone == storeDto.Phone);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_006);
                }
                var storeId = IdGenerator.CreateID("STR");
                var currentUserId = _userSession.UserId;
                storeDto.StoreId = storeId;
                storeDto.Created = _dateTimeService.NowUtc;
                storeDto.CreatedBy = currentUserId;
                var storeEntity = _mapper.Map<Store>(storeDto);
                var succeeded = _ministopUnitOfWork.StoreRepository.Add(storeEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> RestoreStore(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var stores = _ministopUnitOfWork.StoreRepository.GetAllIsDelete();
                var filter = stores.Where(x => listRestoreId.Contains(x.StoreID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.StoreRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> UpdateStore(StoreDto storeEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.StoreRepository.Any(x => x.StoreName == storeEdit.StoreName && x.Phone == storeEdit.Phone && x.StoreID != storeEdit.StoreId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_006);
                }
                var currentUserId = _userSession.UserId;
                var storeEntity = _ministopUnitOfWork.StoreRepository.Find(x => x.StoreID == storeEdit.StoreId);
                if (storeEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_001);
                }
                storeEntity.StoreName = storeEdit.StoreName;
                storeEntity.Phone = storeEdit.Phone;
                storeEntity.Address = storeEdit.Address;
                storeEntity.LastModified = _dateTimeService.NowUtc;
                storeEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.StoreRepository.Update(storeEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemoveStore(string storeId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var storeEntity = _ministopUnitOfWork.StoreRepository.Find(x => x.StoreID == storeId&& !x.IsDeleted);
                if (storeEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_001);
                }
                var emp = _ministopUnitOfWork.EmployeeRepository.GetAll(x => x.StoreID == storeId && !x.IsDeleted);
                var listEmp = emp.ToList();
                storeEntity.LastModified = _dateTimeService.NowUtc;
                storeEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.StoreRepository.SoftDelete(storeEntity, true);
                foreach (var item in listEmp) {

                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.EmployeeRepository.SoftDeleteRange(listEmp,true);
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
