using AutoMapper;
using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Services.Interfaces;
using Shared.ErrorCode;
using Shared.Helpers;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReturnDetailService : IReturnDetailService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IMapper _mapper;

        public ReturnDetailService(IMinistopUnitOfWork ministopUnitOfWork, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _mapper = mapper;
        }

        public Result<IReadOnlyList<ReturnDetailDto>> GetAll()
        {
            try
            {
                var list = _ministopUnitOfWork.ReturnDetailRepository.GetAll();
                var dto = _mapper.Map<IReadOnlyList<ReturnDetailDto>>(list);
                return new Result<IReadOnlyList<ReturnDetailDto>>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<ReturnDetailDto> GetReturnDetailByID(string id)
        {
            try
            {
                var entity = _ministopUnitOfWork.ReturnDetailRepository.Find(x => x.Id == id);
                if (entity == null)
                    return new Result<ReturnDetailDto>(ErrorCodeEnum.SFT_ERR_003);

                var dto = _mapper.Map<ReturnDetailDto>(entity);
                return new Result<ReturnDetailDto>(dto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PagedResult<IReadOnlyList<ReturnDetailDto>> GetReturnDetail(int pageNumber, int pageSize)
        {
            try
            {
                var query = _ministopUnitOfWork.ReturnDetailRepository.GetAll().AsQueryable();
                var totalCount = query.Count();
                var paged = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var dto = _mapper.Map<IReadOnlyList<ReturnDetailDto>>(paged);
                return new PagedResult<IReadOnlyList<ReturnDetailDto>>(dto, pageNumber, pageSize, totalCount);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Result<bool> CreateReturnDetail(ReturnDetailDto returnDetailDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                if (returnDetailDto == null)
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);

                // Tạo ID nếu chưa có
                if (string.IsNullOrEmpty(returnDetailDto.Id))
                    returnDetailDto.Id = IdGenerator.CreateID("RTD");

                var entity = _mapper.Map<ReturnDetail>(returnDetailDto);
                if (entity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                var added = _ministopUnitOfWork.ReturnDetailRepository.Add(entity);
                if (added == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
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

        public Result<bool> UpdateReturnDetail(ReturnDetailDto returnDetailsEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var existing = _ministopUnitOfWork.ReturnDetailRepository.Find(x => x.Id == returnDetailsEdit.Id);
                if (existing == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                var updatedEntity = _mapper.Map(returnDetailsEdit, existing);

                _ministopUnitOfWork.ReturnDetailRepository.Update(updatedEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveReturnDetail(string returnID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var entity = _ministopUnitOfWork.ReturnDetailRepository.Find(x => x.Id == returnID);
                if (entity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }

                _ministopUnitOfWork.ReturnDetailRepository.Delete(entity, true);
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
