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
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public ProductCategoryService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<ProductCategoryDto> GetProductCategoryByID(string id)
        {
            try
            {
                var ProductCategoryEntity = _ministopUnitOfWork.ProductCategoryRepository.Find(x => x.CategoryID == id);
                if (ProductCategoryEntity == null)
                {
                    return new Result<ProductCategoryDto>(ErrorCodeEnum.PCT_ERR_001);
                }
                var result = _mapper.Map<ProductCategoryDto>(ProductCategoryEntity);
                return new Result<ProductCategoryDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<IReadOnlyList<ProductCategoryDto>> GetAllProductCategoryIsDelete()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ProductCategoryRepository.GetAllIsDelete();

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<ProductCategoryDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<ProductCategoryDto>>(list);
        }
        public Result<IReadOnlyList<ProductCategoryDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.ProductCategoryRepository.GetAll(x => !x.IsDeleted);

            // Map sang Domain.Entity.Store
            var list = _mapper.Map<IReadOnlyList<ProductCategoryDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<ProductCategoryDto>>(list);
        }
        public Result<bool> RestoreProductCategory(List<string> listRestoreId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {

                var currentUserId = _userSession.UserId;

                var productCategorys = _ministopUnitOfWork.ProductCategoryRepository.GetAllIsDelete();
                var filter = productCategorys.Where(x => listRestoreId.Contains(x.CategoryID)).ToList();
                if (filter == null || filter.Count == 0)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_001);
                }
                foreach (var item in filter)
                {
                    item.IsDeleted = false;
                    item.LastModified = _dateTimeService.NowUtc;
                    item.LastModifiedBy = currentUserId;
                }
                _ministopUnitOfWork.ProductCategoryRepository.UpdateRange(filter, true);

                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);

            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<ProductCategoryDto>> GetProductCategory(int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.ProductCategoryRepository.GetCount(x => !x.IsDeleted);
            var ProductCategorysEntity = _ministopUnitOfWork.ProductCategoryRepository.GetPagedResponse(pageNumber, pageSize);
            var ProductCategoryDto = _mapper.Map<IReadOnlyList<ProductCategoryDto>>(ProductCategorysEntity);
            return new PagedResult<IReadOnlyList<ProductCategoryDto>>(ProductCategoryDto, pageNumber, pageSize, totalCount);
        }
        public Result<bool> CreateProductCategory(ProductCategoryDto ProductCategoryDto)
        {
            try
            {
                var isDuplicate = _ministopUnitOfWork.ProductCategoryRepository.Any(x => x.CategoryName == ProductCategoryDto.CategoryName);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_006);
                }
                var ProductCategoryId = IdGenerator.CreateID("PCT");
                var currenUserId = _userSession.UserId;
                ProductCategoryDto.CategoryId = ProductCategoryId;
                ProductCategoryDto.Created = _dateTimeService.NowUtc;
                ProductCategoryDto.CreatedBy = currenUserId;
                var ProductCategoryEntity = _mapper.Map<ProductCategory>(ProductCategoryDto);
                var succeeded = _ministopUnitOfWork.ProductCategoryRepository.Add(ProductCategoryEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_003);
                }
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> UpdateProductCategory(ProductCategoryDto ProductCategoryEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var isDuplicate = _ministopUnitOfWork.ProductCategoryRepository.Any(x => x.CategoryName == ProductCategoryEdit.CategoryName && x.Description == ProductCategoryEdit.Description);
                if (isDuplicate)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_004);
                }
                var currentUserID = _userSession.UserId;
                var ProductCategoryEntity = _ministopUnitOfWork.ProductCategoryRepository.Find(x => x.CategoryID == ProductCategoryEdit.CategoryId);
                if (ProductCategoryEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.STR_ERR_001);
                }
                ProductCategoryEntity.CategoryName = ProductCategoryEdit.CategoryName;
                ProductCategoryEntity.Description = ProductCategoryEdit.Description;
                ProductCategoryEntity.LastModified = _dateTimeService.NowUtc;
                ProductCategoryEntity.LastModifiedBy = currentUserID;
                _ministopUnitOfWork.ProductCategoryRepository.Update(ProductCategoryEntity, true);
                _ministopUnitOfWork.Commit();
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                throw ex;
            }
        }
        public Result<bool> RemoveProductCategory(string ProductCategoryId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var ProductCategoryEntity = _ministopUnitOfWork.ProductCategoryRepository.Find(x => x.CategoryID == ProductCategoryId);
                if (ProductCategoryEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PCT_ERR_005);
                }
                ProductCategoryEntity.LastModified = _dateTimeService.NowUtc;
                ProductCategoryEntity.LastModifiedBy = currentUserId;
                _ministopUnitOfWork.ProductCategoryRepository.SoftDelete(ProductCategoryEntity, true);
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
