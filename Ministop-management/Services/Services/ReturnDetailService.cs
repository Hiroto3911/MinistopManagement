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
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ReturnDetailRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<ReturnDetailDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<ReturnDetailDto>>(list);
        }
        public Result<bool> Any(string returnID)
        {
         
                bool isChecked = _ministopUnitOfWork.ReturnDetailRepository.Any((x => x.ReturnID == returnID));
                return new Result<bool>(isChecked);
           
        }
        public Result<ReturnDetailDto> GetReturnDetailByID(string id)
        {
            var returnEntity = _ministopUnitOfWork.ReturnDetailRepository.Find(x => x.Id == id);
            if (returnEntity == null)
            {
                return new Result<ReturnDetailDto>(ErrorCodeEnum.RTD_ERR_001);//chua sua error
            }
            var result = _mapper.Map<ReturnDetailDto>(returnEntity);
            return new Result<ReturnDetailDto>(result);
        }

        public PagedResult<IReadOnlyList<ReturnDetailDto>> GetReturnDetail(string returnID,int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.ReturnDetailRepository.GetCount(x => x.ReturnID == returnID);
            var InvoiceDetailEntity = _ministopUnitOfWork.ReturnDetailRepository.GetPagedResponse((x => x.ReturnID == returnID), pageNumber, pageSize);
            var InvoiceDetailsDto = _mapper.Map<IReadOnlyList<ReturnDetailDto>>(InvoiceDetailEntity);
            return new PagedResult<IReadOnlyList<ReturnDetailDto>>(InvoiceDetailsDto, pageNumber, pageSize, totalCount);
        }

        public Result<bool> CreateReturnDetail(ReturnDetailDto returnDetailDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                
                if (string.IsNullOrEmpty(returnDetailDto.Id))
                    returnDetailDto.Id = IdGenerator.CreateID("RTD");

                var entity = _mapper.Map<ReturnDetail>(returnDetailDto);
                if (entity == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.RTD_ERR_003);
                }

                var added = _ministopUnitOfWork.ReturnDetailRepository.Add(entity);
                if (added == null)
                {
                    _ministopUnitOfWork.Rollback();
                    return new Result<bool>(ErrorCodeEnum.RTD_ERR_003);
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
                    return new Result<bool>(ErrorCodeEnum.RTD_ERR_003);
                }

                var updatedEntity = _mapper.Map<ReturnDetail>(returnDetailsEdit);

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
                    return new Result<bool>(ErrorCodeEnum.RTD_ERR_003);
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
        public Result<bool> RemoveRangeReturnDetailByReturnID(string returnID)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var returnDetailEntity = _ministopUnitOfWork.ReturnDetailRepository.GetAll((x => x.ReturnID == returnID));
                if (returnDetailEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.RTD_ERR_001);
                }
                _ministopUnitOfWork.ReturnDetailRepository.DeleteRange(returnDetailEntity.ToList(), true);
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
