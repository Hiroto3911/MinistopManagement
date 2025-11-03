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
    public class SupplierService : ISupplierService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public SupplierService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<SupplierDto> GetSupplierByID(string id)
        {
            try
            {
                var SupplierEntity = _ministopUnitOfWork.SupplierRepository.Find(x => x.SupplierID == id);
                if (SupplierEntity == null)
                {
                    return new Result<SupplierDto>(ErrorCodeEnum.PCT_ERR_001);
                }
                var result = _mapper.Map<SupplierDto>(SupplierEntity);
                return new Result<SupplierDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<IReadOnlyList<SupplierDto>> GetAllSupplierIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.SupplierRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<SupplierDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<SupplierDto>>(list);
        }
        public Result<IReadOnlyList<SupplierDto>> GetAllSupplier()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.SupplierRepository.GetAll(x => !x.IsDeleted);

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<SupplierDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<SupplierDto>>(list);
        }
        public Result<bool> RestoreSupplier(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var suppliers = _ministopUnitOfWork.SupplierRepository.GetAllIsDelete();
                var filter = suppliers.Where(x => listRestoreId.Contains(x.SupplierID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.SLE_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.SupplierRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<SupplierDto>> GetSupplier(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.SupplierRepository.GetCount(x => !x.IsDeleted);
            var SupplierEntity = _ministopUnitOfWork.SupplierRepository.GetPagedResponse(pageNumber, pageSize);
            var SupplierDto = _mapper.Map<IReadOnlyList<SupplierDto>>(SupplierEntity);
            return new PagedResult<IReadOnlyList<SupplierDto>>(SupplierDto, pageNumber, pageSize, totalCount);
        }
        public Result<bool> CreateSupplier(SupplierDto SupplierDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.SupplierRepository.Any(x => x.SupplierName == SupplierDto.SupplierName && x.Phone == SupplierDto.Phone);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SLE_ERR_006);
                }
                var SuppliesId = IdGenerator.CreateID("SLE");
                var currenUseId = _userSession.UserId;
                SupplierDto.SupplierId = SuppliesId;
                SupplierDto.Created = _dateTimeService.NowUtc;
                SupplierDto.CreatedBy = currenUseId;
                var SuppliesEntity = _mapper.Map<Supplier>(SupplierDto);
                var succeeded = _ministopUnitOfWork.SupplierRepository.Add(SuppliesEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLE_ERR_003);
                }
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> UpdateSupplier(SupplierDto SupplierDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.SupplierRepository.Any(x => x.SupplierName == SupplierDto.SupplierName && x.Phone == SupplierDto.Phone && x.Address == SupplierDto.Address);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SLE_ERR_004);
                }
                var currentUserID = _userSession.UserId;
                var SupplierEntity = _ministopUnitOfWork.SupplierRepository.Find(x => x.SupplierID == SupplierDto.SupplierId);
                if (SupplierEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLE_ERR_001);
                }
                SupplierEntity.SupplierName = SupplierDto.SupplierName;
                SupplierEntity.Phone = SupplierDto.Phone;
                SupplierEntity.Address = SupplierDto.Address;
                SupplierEntity.LastModified = _dateTimeService.NowUtc;
                SupplierEntity.LastModifiedBy = currentUserID;
                _ministopUnitOfWork.SupplierRepository.Update(SupplierEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemoveSupplier(string SupplierId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var SupplierEntity = _ministopUnitOfWork.SupplierRepository.Find(x => x.SupplierID == SupplierId);
                if (SupplierEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLE_ERR_005);
                }
                SupplierEntity.LastModified = _dateTimeService.NowUtc;
                SupplierEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.SupplierRepository.SoftDelete(SupplierEntity, true);
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
