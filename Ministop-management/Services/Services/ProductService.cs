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
    public class ProductService : IProductService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ProductService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<ProductDto> GetProductByID(string id)
        {
            try
            {
                var ProductEntity = _ministopUnitOfWork.ProductRepository.Find(x => x.ProductID == id);
                if (ProductEntity == null)
                {
                    return new Result<ProductDto>(ErrorCodeEnum.PRD_ERR_001);
                }
                var result = _mapper.Map<ProductDto>(ProductEntity);
                return new Result<ProductDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<IReadOnlyList<ProductDto>> GetAllProductIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ProductRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<ProductDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<ProductDto>>(list);
        }
        public PagedResult<IReadOnlyList<ProductDto>> GetProduct(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.ProductRepository.GetCount(x => !x.IsDeleted);
            var ProductsEntity = _ministopUnitOfWork.ProductRepository.GetPagedResponse(pageNumber, pageSize);
            var ProductsDto = _mapper.Map<IReadOnlyList<ProductDto>>(ProductsEntity);
            return new PagedResult<IReadOnlyList<ProductDto>>(ProductsDto, pageNumber, pageSize, totalCount);
        }
        public Result<IReadOnlyList<ProductDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ProductRepository.GetAll();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<ProductDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<ProductDto>>(list);
        }
        public Result<bool> RestoreProduct(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var products = _ministopUnitOfWork.ProductRepository.GetAllIsDelete();
                var filter = products.Where(x => listRestoreId.Contains(x.ProductID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.ProductRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> CreateProduct(ProductDto ProductDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.ProductRepository.Any(x => x.ProductName == ProductDto.ProductName && x.Unit == ProductDto.Unit);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_006);
                }
                var ProductId = IdGenerator.CreateID("PRD");
                var currenUserId = _userSession.UserId;
                ProductDto.ProductId = ProductId;
                ProductDto.Created = _dateTimeService.NowUtc;
                ProductDto.CreatedBy = currenUserId;
                var ProductEntity = _mapper.Map<Product>(ProductDto);
                var succeeded = _ministopUnitOfWork.ProductRepository.Add(ProductEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_003);
                }
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> UpdateProduct(ProductDto ProductDto)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                //kiem tra
                var isDuplicate = _ministopUnitOfWork.ProductRepository.Any(x => x.ProductName == ProductDto.ProductName && x.Unit == ProductDto.Unit && x.ProductID != ProductDto.ProductId);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_004);
                }
                //tim de cap nhat
                var currentUserID = _userSession.UserId;
                var ProductEntity = _ministopUnitOfWork.ProductRepository.Find(x => x.ProductID == ProductDto.ProductId);
                if (ProductEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PRD_ERR_001);
                }
                ProductEntity.CategoryID = ProductDto.CategoryId;
                ProductEntity.ProductName = ProductDto.ProductName;
                ProductEntity.Unit = ProductDto.Unit;
                ProductEntity.StandardPrice = ProductDto.StandardPrice;
                ProductEntity.Status = ProductDto.Status;
                ProductEntity.LastModified = _dateTimeService.NowUtc;
                ProductEntity.LastModifiedBy = currentUserID;
                _ministopUnitOfWork.ProductRepository.Update(ProductEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemoveProduct(string ProductId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var ProductEntity = _ministopUnitOfWork.ProductRepository.Find(x => x.ProductID == ProductId);
                if (ProductEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_005);
                }
                ProductEntity.LastModified = _dateTimeService.NowUtc;
                ProductEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.ProductRepository.SoftDelete(ProductEntity, true);
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
