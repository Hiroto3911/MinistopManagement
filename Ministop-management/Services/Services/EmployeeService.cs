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
    public class EmployeeService : IEmployeeService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public EmployeeService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<EmployeeDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.EmployeeRepository.GetAll();

            // Map sang Domain.Entity.Employee
            var list = _mapper.Map<IReadOnlyList<EmployeeDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<EmployeeDto>>(list);
        }

        public Result<IReadOnlyList<EmployeeDto>> GetAllEmployeeIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.EmployeeRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Employee
            var list = _mapper.Map<IReadOnlyList<EmployeeDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<EmployeeDto>>(list);
        }
        public Result<EmployeeDto> GetEmployeeByID(string id)
        {
            try
            {
                var EmployeeEntity = _ministopUnitOfWork.EmployeeRepository.Find(x => x.EmployeeID == id);
                if (EmployeeEntity == null)
                {
                    return new Result<EmployeeDto>(ErrorCodeEnum.EMP_ERR_001);
                }
                var result = _mapper.Map<EmployeeDto>(EmployeeEntity);
                return new Result<EmployeeDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<EmployeeDto>> GetEmployee(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.EmployeeRepository.GetCount(x => !x.IsDeleted);
            var EmployeesEntity = _ministopUnitOfWork.EmployeeRepository.GetPagedResponse(pageNumber, pageSize);
            var EmployeesDto = _mapper.Map<IReadOnlyList<EmployeeDto>>(EmployeesEntity);

            return new PagedResult<IReadOnlyList<EmployeeDto>>(EmployeesDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateEmployee(EmployeeDto EmployeeDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.EmployeeRepository.Any(x => x.FullName == EmployeeDto.FullName);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_006);
                }
                var EmployeeId = IdGenerator.CreateID("EMP");
                var currentUserId = _userSession.UserId;
                EmployeeDto.EmployeeId = EmployeeId;
                EmployeeDto.Created = _dateTimeService.NowUtc;
                EmployeeDto.CreatedBy = currentUserId;
                var EmployeeEntity = _mapper.Map<Employee>(EmployeeDto);
                var succeeded = _ministopUnitOfWork.EmployeeRepository.Add(EmployeeEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<bool> RestoreEmployee(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var Employees = _ministopUnitOfWork.EmployeeRepository.GetAllIsDelete();
                var filter = Employees.Where(x => listRestoreId.Contains(x.EmployeeID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.EmployeeRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> UpdateEmployee(EmployeeDto EmployeeEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.EmployeeRepository.Any(x => x.FullName == EmployeeEdit.FullName && x.EmployeeID != EmployeeEdit.EmployeeId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_006);
                }
                var currentUserId = _userSession.UserId;
                var EmployeeEntity = _ministopUnitOfWork.EmployeeRepository.Find(x => x.EmployeeID == EmployeeEdit.EmployeeId);
                if (EmployeeEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_001);
                }
                EmployeeEntity.FullName = EmployeeEdit.FullName;
                EmployeeEntity.Gender = EmployeeEdit.Gender;
                EmployeeEntity.BirthDate = EmployeeEdit.BirthDate;
                EmployeeEntity.Phone = EmployeeEdit.Phone;
                EmployeeEntity.Position = EmployeeEdit.Position;
                EmployeeEntity.EmploymentType = EmployeeEdit.EmploymentType;
                EmployeeEntity.PasswordHash = EmployeeEdit.PasswordHash;
                EmployeeEntity.LastModified = _dateTimeService.NowUtc;
                EmployeeEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.EmployeeRepository.Update(EmployeeEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveEmployee(string EmployeeId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;
                var EmployeeEntity = _ministopUnitOfWork.EmployeeRepository.Find(x => x.EmployeeID == EmployeeId);
                if (EmployeeEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_001);
                }
                EmployeeEntity.LastModified = _dateTimeService.NowUtc;
                EmployeeEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.EmployeeRepository.SoftDelete(EmployeeEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<EmployeeDto>> GetEmployeeByStore(string storeId, int pageNumber, int pageSize)
        {
            try
            {
                // Lấy tất cả nhân viên
                var employees = _ministopUnitOfWork.EmployeeRepository.GetPagedResponse(pageNumber, pageSize)
                    .Where(x => !x.IsDeleted && x.StoreID == storeId)
                    .ToList();

                var totalCount = _ministopUnitOfWork.EmployeeRepository.GetCount(
                    x => !x.IsDeleted && x.StoreID == storeId);

                var employeeDtos = _mapper.Map<IReadOnlyList<EmployeeDto>>(employees);

                return new PagedResult<IReadOnlyList<EmployeeDto>>(employeeDtos, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách nhân viên theo cửa hàng", ex);
            }
        }

    }
}
