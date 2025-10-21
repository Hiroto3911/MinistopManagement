using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Wrappers
{
    public class PagedResult<T> : Result<T>
    {
        public int PageNumber { get; set; }
        public long TotalCount { get; set; }
        public int PageSize { get; set; }
        public PagedResult(T data, int pageNumber, int pageSize, long totalCount)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Succeeded = true;
            Message = null;
            Errors = null;
        }
        public PagedResult(IEnumerable<T> data, int pageNumber, int pageSize, long totalCount)
        {


            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Succeeded = true;
            Message = null;
            Errors = null;
        }
    }
}
