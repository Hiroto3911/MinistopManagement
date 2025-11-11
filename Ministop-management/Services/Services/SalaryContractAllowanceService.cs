using AutoMapper;
using Domain.DTO;
using Domain.Entity;
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

namespace Services.Services
{
    public class SalaryContractAllowanceService : ISalaryContractAllowanceService
    {
        private readonly IMinistopUnitOfWork _unitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public SalaryContractAllowanceService(
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

        public void BeginTransaction() => _unitOfWork.BeginTransaction();
        public void Commit() => _unitOfWork.Commit();
        public void Rollback() => _unitOfWork.Rollback();

        public Result<IReadOnlyList<SalaryContractAllowanceDto>> GetAll()
        {
            try
            {
                var entities = _unitOfWork.SalaryContractAllowanceRepository.GetAll();
                var dtos = _mapper.Map<IReadOnlyList<SalaryContractAllowanceDto>>(entities);
                return new Result<IReadOnlyList<SalaryContractAllowanceDto>>(dtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<SalaryContractAllowanceDto> GetById(string id)
        {
            try
            {
                var entity = _unitOfWork.SalaryContractAllowanceRepository.Find(x => x.Id == id);
                if (entity == null)
                    return new Result<SalaryContractAllowanceDto>(ErrorCodeEnum.SCA_ERR_001);

                var dto = _mapper.Map<SalaryContractAllowanceDto>(entity);
                return new Result<SalaryContractAllowanceDto>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<SalaryContractAllowanceDto>> GetPaged(int pageNumber, int pageSize)
        {
            var totalCount = _unitOfWork.SalaryContractAllowanceRepository.GetCount();
            var entities = _unitOfWork.SalaryContractAllowanceRepository.GetPagedResponse(pageNumber, pageSize);
            var dtos = _mapper.Map<IReadOnlyList<SalaryContractAllowanceDto>>(entities);
            return new PagedResult<IReadOnlyList<SalaryContractAllowanceDto>>(dtos, pageNumber, pageSize, totalCount);
        }

        public Result<IReadOnlyList<SalaryContractAllowanceDto>> GetByContractId(string contractId)
        {
            try
            {
                if (string.IsNullOrEmpty(contractId))
                    return new Result<IReadOnlyList<SalaryContractAllowanceDto>>(ErrorCodeEnum.SCA_ERR_002);

                // SỬA: GetWhere → GetAll().Where
                var entities = _unitOfWork.SalaryContractAllowanceRepository
                    .GetAll()
                    .Where(x => x.ContractID == contractId)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<SalaryContractAllowanceDto>>(entities);
                return new Result<IReadOnlyList<SalaryContractAllowanceDto>>(dtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<bool> Create(SalaryContractAllowanceDto dto)
        {
            try
            {
                // Kiểm tra trùng
                var isDuplicate = _unitOfWork.SalaryContractAllowanceRepository
                    .GetAll()
                    .Any(x => x.ContractID == dto.ContractId && x.AllowanceID == dto.AllowanceId);
                if (isDuplicate)
                    return new Result<bool>(ErrorCodeEnum.SCA_ERR_003);

                // Kiểm tra tồn tại
                var contractExists = _unitOfWork.SalaryContractRepository.Any(x => x.ContractID == dto.ContractId);
                var allowanceExists = _unitOfWork.AllowanceRepository.Any(x => x.AllowanceID == dto.AllowanceId);
                if (!contractExists) return new Result<bool>(ErrorCodeEnum.SCA_ERR_004);
                if (!allowanceExists) return new Result<bool>(ErrorCodeEnum.SCA_ERR_005);

                dto.Id = IdGenerator.CreateID("SCA");
                var entity = _mapper.Map<SalaryContract_Allowance>(dto);
                _unitOfWork.SalaryContractAllowanceRepository.Add(entity);

                // KHÔNG gọi SubmitChanges() ở đây → để UI gọi 1 lần
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<bool> Update(SalaryContractAllowanceDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = _unitOfWork.SalaryContractAllowanceRepository.Find(x => x.Id == dto.Id);
                if (entity == null)
                {
                    _unitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SCA_ERR_001);
                }

                var isDuplicate = _unitOfWork.SalaryContractAllowanceRepository
                    .GetAll()
                    .Any(x => x.ContractID == dto.ContractId
                           && x.AllowanceID == dto.ContractId
                           && x.Id != dto.Id);

                if (isDuplicate)
                {
                    _unitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SCA_ERR_003);
                }

                entity.CustomAmount = dto.CustomAmount;
                _unitOfWork.SalaryContractAllowanceRepository.Update(entity, true);
                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> Remove(string id)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = _unitOfWork.SalaryContractAllowanceRepository.Find(x => x.Id == id);
                if (entity == null)
                {
                    _unitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SCA_ERR_001);
                }

                _unitOfWork.SalaryContractAllowanceRepository.Delete(entity, true);
                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveByContractId(string contractId)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var items = _unitOfWork.SalaryContractAllowanceRepository
                    .GetAll()
                    .Where(x => x.ContractID == contractId)
                    .ToList();

                foreach (var item in items)
                {
                    _unitOfWork.SalaryContractAllowanceRepository.Delete(item, true);
                }

                _unitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

       
    }
}