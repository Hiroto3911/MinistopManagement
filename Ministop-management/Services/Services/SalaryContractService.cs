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
    public class SalaryContractService : ISalaryContractService
    {
        private readonly IMinistopUnitOfWork _unitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public SalaryContractService(
            IMinistopUnitOfWork unitOfWork,
            IDateTimeService dateTimeService,
            IUserSession userSession,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<SalaryContractDto>> GetAll()
        {
            var entities = _unitOfWork.SalaryContractRepository.GetAll();
            var dtos = _mapper.Map<IReadOnlyList<SalaryContractDto>>(entities);
            return new Result<IReadOnlyList<SalaryContractDto>>(dtos);
        }

        public Result<IReadOnlyList<SalaryContractDto>> GetAllIsDeleted()
        {
            var entities = _unitOfWork.SalaryContractRepository.GetAllIsDelete();
            var dtos = _mapper.Map<IReadOnlyList<SalaryContractDto>>(entities);
            return new Result<IReadOnlyList<SalaryContractDto>>(dtos);
        }

        public Result<SalaryContractDto> GetById(string contractId)
        {
            try
            {
                var entity = _unitOfWork.SalaryContractRepository.Find(x => x.ContractID == contractId && !x.IsDeleted);
                if (entity == null)
                    return new Result<SalaryContractDto>(ErrorCodeEnum.SAL_ERR_001); // Không tìm thấy

                var dto = _mapper.Map<SalaryContractDto>(entity);
                return new Result<SalaryContractDto>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<SalaryContractDto> GetCurrentContractByEmployeeId(string employeeId)
        {
            try
            {
                var contract = _unitOfWork.SalaryContractRepository.GetCurrentContract(employeeId);
                if (contract == null)
                    return new Result<SalaryContractDto>(ErrorCodeEnum.SAL_ERR_001);

                var dto = _mapper.Map<SalaryContractDto>(contract);
                return new Result<SalaryContractDto>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<SalaryContractDto>> GetPaged(int pageNumber, int pageSize)
        {
            var total = _unitOfWork.SalaryContractRepository.GetCount(x => !x.IsDeleted);
            var entities = _unitOfWork.SalaryContractRepository.GetPagedResponse(pageNumber, pageSize)
                .Where(x => !x.IsDeleted).ToList();

            var dtos = _mapper.Map<IReadOnlyList<SalaryContractDto>>(entities);
            return new PagedResult<IReadOnlyList<SalaryContractDto>>(dtos, pageNumber, pageSize, total);
        }

        public PagedResult<IReadOnlyList<SalaryContractDto>> GetByEmployeeId(string employeeId, int pageNumber, int pageSize)
        {
            var total = _unitOfWork.SalaryContractRepository.GetCount(x => x.EmployeeID == employeeId && !x.IsDeleted);
            var entities = _unitOfWork.SalaryContractRepository.GetPagedResponse(pageNumber, pageSize)
                .Where(x => x.EmployeeID == employeeId && !x.IsDeleted).ToList();

            var dtos = _mapper.Map<IReadOnlyList<SalaryContractDto>>(entities);
            return new PagedResult<IReadOnlyList<SalaryContractDto>>(dtos, pageNumber, pageSize, total);
        }

        public Result<bool> Create(SalaryContractDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                bool employeeExists = _unitOfWork.EmployeeRepository.Any(x => x.EmployeeID == dto.EmployeeId && !x.IsDeleted);
                if (!employeeExists)
                    return new Result<bool>(ErrorCodeEnum.EMP_ERR_001);

                bool hasActiveContract = _unitOfWork.SalaryContractRepository.Any(x =>
                    x.EmployeeID == dto.EmployeeId && !x.IsDeleted && x.EndDate == null);

                if (hasActiveContract && dto.EndDate == null)
                    return new Result<bool>(ErrorCodeEnum.SAL_ERR_006);

                dto.ContractId = IdGenerator.CreateID("SAL");
                dto.Created = _dateTimeService.NowUtc;
                dto.CreatedBy = _userSession.UserId;

                var entity = _mapper.Map<SalaryContract>(dto);
                var added = _unitOfWork.SalaryContractRepository.Add(entity);
                if (added == null)
                    return new Result<bool>(ErrorCodeEnum.SAL_ERR_003);

                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public Result<bool> Update(SalaryContractDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = _unitOfWork.SalaryContractRepository.Find(x => x.ContractID == dto.ContractId && !x.IsDeleted);
                if (entity == null)
                    return new Result<bool>(ErrorCodeEnum.SAL_ERR_001);

                entity.BasicSalary = dto.BasicSalary;
                entity.HourlyRate = dto.HourlyRate;
                entity.StartDate = dto.StartDate;
                entity.EndDate = dto.EndDate;
                entity.LastModified = _dateTimeService.NowUtc;
                entity.LastModifiedBy = _userSession.UserId;

                _unitOfWork.SalaryContractRepository.Update(entity, true);

                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public Result<bool> SoftDelete(string contractId)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = _unitOfWork.SalaryContractRepository.Find(x => x.ContractID == contractId && !x.IsDeleted);
                if (entity == null)
                    return new Result<bool>(ErrorCodeEnum.SAL_ERR_001);

                entity.IsDeleted = true;
                entity.LastModified = _dateTimeService.NowUtc;
                entity.LastModifiedBy = _userSession.UserId;

                _unitOfWork.SalaryContractRepository.SoftDelete(entity, true);

                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public Result<bool> Restore(List<string> contractIds)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entities = _unitOfWork.SalaryContractRepository.GetAllIsDelete()
                    .Where(x => contractIds.Contains(x.ContractID)).ToList();

                if (!entities.Any())
                    return new Result<bool>(ErrorCodeEnum.SAL_ERR_001);

                foreach (var item in entities)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = _userSession.UserId;
                }

                _unitOfWork.SalaryContractRepository.UpdateRange(entities, true);

                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public Result<bool> AnyContractForEmployee(string employeeId)
        {
            try
            {
                var exists = _unitOfWork.SalaryContractRepository.Any(x => x.EmployeeID == employeeId && !x.IsDeleted);
                return new Result<bool>(exists);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
