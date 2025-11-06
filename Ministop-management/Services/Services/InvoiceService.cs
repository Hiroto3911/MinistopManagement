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
    public class InvoiceService : IInvoiceService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;
        private readonly IDateTimeService _dateTimeService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;
        public InvoiceService(IMinistopUnitOfWork ministopUnitOfWork, IDateTimeService dateTimeService, IUserSession userSession, IMapper mapper)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
            _dateTimeService = dateTimeService;
            _userSession = userSession;
            _mapper = mapper;
        }
        public Result<IReadOnlyList<InvoiceDto>> GetAll()
        {
            // Lấy danh sách từ DBML (entity của LINQ to SQL)
            var dbList = _ministopUnitOfWork.InvoiceRepository.GetAll();

            // Map sang Domain.Entity.Shift
            var list = _mapper.Map<IReadOnlyList<InvoiceDto>>(dbList);

            // Trả về Result
            return new Result<IReadOnlyList<InvoiceDto>>(list);
        }


        public Result<InvoiceDto> GetInvoiceByID(string id)
        {
            try
            {
                var InvoiceEntity = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == id);
                if (InvoiceEntity == null)
                {
                    return new Result<InvoiceDto>(ErrorCodeEnum.STR_ERR_001);//chua sua error
                }
                var result = _mapper.Map<InvoiceDto>(InvoiceEntity);
                return new Result<InvoiceDto>(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PagedResult<IReadOnlyList<InvoiceDto>> GetInvoice(string storeId, int pageNumber, int pageSize)
        {
            var totalCount = _ministopUnitOfWork.InvoiceRepository.GetCount(x => x.StoreID == storeId);
            var invoiceEntity = _ministopUnitOfWork.InvoiceRepository.GetPagedResponse(pageNumber, pageSize);
            var invoicetDto = _mapper.Map<IReadOnlyList<InvoiceDto>>(invoiceEntity);
            return new PagedResult<IReadOnlyList<InvoiceDto>>(invoicetDto, pageNumber, pageSize, totalCount);
        }
        public Result<bool> CreateInvoice(InvoiceDto InvoiceDto)
        {
            try
            {
                var InvoiceId = IdGenerator.CreateID("IVC");
                InvoiceDto.InvoiceId = InvoiceId;
                var InvoiceEntity = _mapper.Map<Invoice>(InvoiceDto);
                var succeeded = _ministopUnitOfWork.InvoiceRepository.Add(InvoiceEntity);
                if (succeeded == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_003);
                }
                return new Result<bool>(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Result<bool> RemoveInvoice(string InvoiceId)
        {
            _ministopUnitOfWork.BeginTransaction();
            try
            {
                var InvoiceEntity = _ministopUnitOfWork.InvoiceRepository.Find(x => x.InvoiceID == InvoiceId);
                if (InvoiceEntity == null)
                {
                    return new Result<bool>(ErrorCodeEnum.SFT_ERR_001);
                }
                _ministopUnitOfWork.InvoiceRepository.Delete(InvoiceEntity, true);
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
