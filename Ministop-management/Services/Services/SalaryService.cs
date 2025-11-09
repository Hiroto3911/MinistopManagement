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
    public class SalaryService : ISalaryService
    {
        private readonly IMinistopUnitOfWork _unitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public SalaryService(
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

        public Result<IReadOnlyList<SalaryDto>> GetAll()
        {
            var dbList = _unitOfWork.SalaryRepository.GetAll();
            var list = _mapper.Map<IReadOnlyList<SalaryDto>>(dbList);
            return new Result<IReadOnlyList<SalaryDto>>(list);
        }

        public Result<SalaryDto> GetSalaryByID(string id)
        {
            try
            {
                var entity = _unitOfWork.SalaryRepository.Find(x => x.SalaryID == id);
                if (entity == null)
                {
                    return new Result<SalaryDto>(ErrorCodeEnum.SLL_ERR_001);
                }
                var result = _mapper.Map<SalaryDto>(entity);
                return new Result<SalaryDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<SalaryDto>> GetPaged(int pageNumber, int pageSize)
        {
            var totalCount = _unitOfWork.SalaryRepository.GetCount();
            var entities = _unitOfWork.SalaryRepository.GetPagedResponse(pageNumber, pageSize);
            var dtos = _mapper.Map<IReadOnlyList<SalaryDto>>(entities);
            return new PagedResult<IReadOnlyList<SalaryDto>>(dtos, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateSalary(SalaryDto dto)
        {
            try
            {
                // Kiểm tra trùng: ContractID + MonthYear
                var isDuplicate = _unitOfWork.SalaryRepository.Any(x =>
                    x.ContractID == dto.ContractId &&
                    x.MonthYear == dto.MonthYear);

                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SLL_ERR_006);
                }

                var salaryId = IdGenerator.CreateID("SLL");
                var currentUserId = _userSession.UserId;

                dto.SalaryId = salaryId;

                var entity = _mapper.Map<Salary>(dto);
                var succeeded = _unitOfWork.SalaryRepository.Add(entity);

                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLL_ERR_003);
                }

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<bool> UpdateSalary(SalaryDto dto)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var entity = _unitOfWork.SalaryRepository.Find(x => x.SalaryID == dto.SalaryId);

                if (entity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLL_ERR_001);
                }

                // Kiểm tra trùng (ngoại trừ chính nó)
                var isDuplicate = _unitOfWork.SalaryRepository.Any(x =>
                    x.ContractID == dto.ContractId &&
                    x.MonthYear == dto.MonthYear &&
                    x.SalaryID != dto.SalaryId);

                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SLL_ERR_006);
                }

                entity.Bonus = dto.Bonus;
                entity.Deduction = dto.Deduction;
                entity.Status = dto.Status;

                _unitOfWork.SalaryRepository.Update(entity, true);
                _unitOfWork.Commit();

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveSalary(string salaryId)
        {
            _unitOfWork.BeginTransaction();
            try
            {
                var entity = _unitOfWork.SalaryRepository.Find(x => x.SalaryID == salaryId);
                if (entity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLL_ERR_001);
                }

                _unitOfWork.SalaryRepository.Delete(entity, true);
                _unitOfWork.Commit();

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<IReadOnlyList<SalaryDto>> GetByContract(string contractId)
        {
            try
            {
                var entities = _unitOfWork.SalaryRepository.GetAll()
                    .Where(x => x.ContractID == contractId)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<SalaryDto>>(entities);
                return new Result<IReadOnlyList<SalaryDto>>(dtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<IReadOnlyList<SalaryDto>> GetByMonthYear(string monthYear)
        {
            try
            {
                var entities = _unitOfWork.SalaryRepository.GetAll()
                    .Where(x => x.MonthYear == monthYear)
                    .ToList();

                var dtos = _mapper.Map<IReadOnlyList<SalaryDto>>(entities);
                return new Result<IReadOnlyList<SalaryDto>>(dtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
