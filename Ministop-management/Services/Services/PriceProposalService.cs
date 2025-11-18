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
    public class PriceProposalService : IPriceProposalService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public PriceProposalService(
            IMinistopUnitOfWork ministopUnitOfWork,
            IUserSession userSession,
            IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _userSession = userSession;
            _mapper = mapper;
        }

        #region Các phương thức đọc dữ liệu

        public Result<IReadOnlyList<PriceProposalDto>> GetAll()
        {
            var dbList = _ministopUnitOfWork.PriceProposalRepository.GetAll();
            var list = _mapper.Map<IReadOnlyList<PriceProposalDto>>(dbList);
            return new Result<IReadOnlyList<PriceProposalDto>>(list);
        }

        public Result<PriceProposalDto> GetpriceProposaByID(string id)
        {
            var priceProposalEntity = _ministopUnitOfWork.PriceProposalRepository
                .Find(x => x.ProposalID == id);
            if (priceProposalEntity == null)
            {
                return new Result<PriceProposalDto>(ErrorCodeEnum.PRD_ERR_001);
            }
            var result = _mapper.Map<PriceProposalDto>(priceProposalEntity);
            return new Result<PriceProposalDto>(result);
        }

        public PagedResult<IReadOnlyList<PriceProposalDto>> GetpriceProposal(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.PriceProposalRepository.GetCount(x => x.StoreID == storeId);
            var priceProposalsEntity = _ministopUnitOfWork.PriceProposalRepository.GetPagedResponse((x => x.StoreID == storeId), pageNumber, pageSize);
            var priceProposalsDto = _mapper.Map<IReadOnlyList<PriceProposalDto>>(priceProposalsEntity);

            return new PagedResult<IReadOnlyList<PriceProposalDto>>(priceProposalsDto, pageNumber, pageSize, totalCount);
        }
        #endregion

        #region Tạo mới đề xuất giá

        public Result<bool> CreatepriceProposal(PriceProposalDto priceProposalDto)
        {
            try
            {
                var currentUserId = _userSession.UserId;
                var stockDetail = _ministopUnitOfWork.StockDetailRepository.Find(
                    x => x.ProductID == priceProposalDto.ProductId &&
                         x.StoreID == priceProposalDto.StoreId);

                if (stockDetail == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PPR_ERR_001);
                }

                var hasPendingProposal = _ministopUnitOfWork.PriceProposalRepository.Any(
                    x => x.ProductID == priceProposalDto.ProductId &&
                         x.StoreID == priceProposalDto.StoreId &&
                         x.Status != 1);

                if (hasPendingProposal)
                {
                    return new Result<bool>(ErrorCodeEnum.PPR_ERR_002);
                }
                var proposalId = IdGenerator.CreateID("PPD");
                var priceProposalEntity = _mapper.Map<PriceProposal>(priceProposalDto);
                priceProposalEntity.ProposalID = proposalId;
                priceProposalEntity.ProposalDate = DateTime.UtcNow.ToLocalTime();
                var success = _ministopUnitOfWork.PriceProposalRepository.Add(priceProposalEntity);
                if (success == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PPR_ERR_003);
                }
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                return new Result<bool>(ErrorCodeEnum.PPR_ERR_003);
            }
        }

        #endregion

        #region Cập nhật đề xuất giá

        public Result<bool> UpdatepriceProposalDto(PriceProposalDto priceProposalEdit)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var currentUserId = _userSession.UserId;
                var priceProposalEntity = _ministopUnitOfWork.PriceProposalRepository
                    .Find(x => x.ProposalID == priceProposalEdit.ProposalId);
                if (priceProposalEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.PPR_ERR_002);
                }
                if (priceProposalEdit.Status == 1)
                {
                    var stockDetail = _ministopUnitOfWork.StockDetailRepository.FindByID(
                    x => x.StoreID == priceProposalEntity.StoreID && x.ProductID == priceProposalEntity.ProductID);
                    if (stockDetail == null)
                    {
                        return new Result<bool>(ErrorCodeEnum.PPR_ERR_004);
                    }
                    stockDetail.Price = priceProposalEdit.NewPrice;
                    _ministopUnitOfWork.StockDetailRepository.Update(stockDetail, true);
                }
                else
                {
                    priceProposalEntity.NewPrice = priceProposalEdit.NewPrice;
                    priceProposalEntity.Reason = priceProposalEdit.Reason;
                }
                priceProposalEntity.Status = priceProposalEdit.Status;
                _ministopUnitOfWork.PriceProposalRepository.Update(priceProposalEntity, true);
                _ministopUnitOfWork.Commit();

                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                _ministopUnitOfWork.Rollback();
                return new Result<bool>(ErrorCodeEnum.PRD_ERR_001);
            }
        }

        #endregion

        #region Xóa đề xuất giá

        public Result<bool> RemovepriceProposalDto(string priceProposalID)
        {
            _ministopUnitOfWork.BeginTransaction();

            var currentUserId = _userSession.UserId;
            var priceProposalEntity = _ministopUnitOfWork.PriceProposalRepository
                .Find(x => x.ProposalID == priceProposalID);

            if (priceProposalEntity == null)
            {
                _ministopUnitOfWork.Rollback();
                return new Result<bool>(ErrorCodeEnum.PPR_ERR_001);
            }

            _ministopUnitOfWork.PriceProposalRepository.Delete(priceProposalEntity);
            _ministopUnitOfWork.Commit();

            return new Result<bool>(true);
        }

        #endregion

        public Result<StoreFixedExpenseDto> GetStoreFixedExpenseByID(string id)
        {
            try
            {
                var storeEntity = _ministopUnitOfWork.FixedExpenseRepository.Find(x => x.ExpenseID == id);
                if (storeEntity == null)
                {
                    return new Result<StoreFixedExpenseDto>(ErrorCodeEnum.SFE_ERR_001);
                }
                var result = _mapper.Map<StoreFixedExpenseDto>(storeEntity);
                return new Result<StoreFixedExpenseDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<StoreFixedExpenseDto>> GetStoreFixedExpense(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.FixedExpenseRepository.GetCount(x => x.StoreID == storeId && !x.IsDeleted);
            var storesDto = _ministopUnitOfWork.FixedExpenseRepository.GetPagedResponse((x => x.StoreID == storeId && !x.IsDeleted), pageNumber, pageSize);
            return new PagedResult<IReadOnlyList<StoreFixedExpenseDto>>(storesDto, pageNumber, pageSize, totalCount);
        }

    }
}
