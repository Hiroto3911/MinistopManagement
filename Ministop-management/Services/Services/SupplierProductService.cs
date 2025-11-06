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
    public class SupplierProductService : ISupplierProductService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public SupplierProductService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public PagedResult<IReadOnlyList<SupplierProductDto>> GetProductExpense(string supplierId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.SupplierProductRepository.GetCount(x => x.SupplierID == supplierId);
            var supplierProductDto = _ministopUnitOfWork.SupplierProductRepository.GetPagedResponse((x => x.SupplierID == supplierId), pageNumber, pageSize);
            return new PagedResult<IReadOnlyList<SupplierProductDto>>(supplierProductDto, pageNumber, pageSize, totalCount);
        }
        public Result<IReadOnlyList<SupplierProductDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ShiftRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<SupplierProductDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<SupplierProductDto>>(list);
        }


        public Result<SupplierProductDto> GetSupplierProductByID(string id)
        {
            try
            {
                var SupplierProductEntity = _ministopUnitOfWork.SupplierProductRepository.Find(x => x.Id == id);
                if (SupplierProductEntity == null)
                {
                    return new Result<SupplierProductDto>(ErrorCodeEnum.SLPRD_ERR_001);
                }
                var result = _mapper.Map<SupplierProductDto>(SupplierProductEntity);
                return new Result<SupplierProductDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<SupplierProductDto>> GetSupplierProduct(string supplierId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.SupplierProductRepository
                .GetCount(x => x.SupplierID == supplierId);
            var supplierProductEntities = _ministopUnitOfWork.SupplierProductRepository
                .GetPagedResponse(x => x.SupplierID == supplierId, pageNumber, pageSize);
            return new PagedResult<IReadOnlyList<SupplierProductDto>>(
                supplierProductEntities,
                pageNumber,
                pageSize,
                totalCount
            );
        }

        public Result<bool> CreateSupplierProduct(SupplierProductDto SupplierProductDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.SupplierProductRepository.Any(x => x.SupplierID == SupplierProductDto.SupplierId && x.ProductID == SupplierProductDto.ProductId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.SLPRD_ERR_006);
                }
                var SupplierProductId = IdGenerator.CreateID("SPP");
                var currentUserId = _userSession.UserId;
                SupplierProductDto.Id = SupplierProductId;

                var SupplierProductEntity = _mapper.Map<SupplierProduct>(SupplierProductDto);
                var succeeded = _ministopUnitOfWork.SupplierProductRepository.Add(SupplierProductEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLPRD_ERR_003);
                }
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Result<bool> UpdateSupplierProduct(SupplierProductDto supplierProductEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            { 
                var currentUserId = _userSession.UserId;
                var SupplierProductEntity = _ministopUnitOfWork.SupplierProductRepository.Find(x => x.Id == supplierProductEdit.Id);
                if (SupplierProductEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLPRD_ERR_001);
                }
                SupplierProductEntity.SupplyPrice = supplierProductEdit.SupplyPrice;
                SupplierProductEntity.Status = supplierProductEdit.Status;

                _ministopUnitOfWork.SupplierProductRepository.Update(SupplierProductEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }

        public Result<bool> RemoveSupplierProduct(string SupplierProductId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var SupplierProductEntity = _ministopUnitOfWork.SupplierProductRepository.Find(x => x.Id == SupplierProductId);
                if (SupplierProductEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SLPRD_ERR_001);
                }
                _ministopUnitOfWork.SupplierProductRepository.Delete(SupplierProductEntity, true);
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
