using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IReportRepository
    {
        List<SP_DanhSachCuaHangTheoThanhPhoResult> GetStoreByRegion(string region);
        List<SP_GetEmployeesByStoreResult> GetEmployeesByStore(string store);
    }
}
