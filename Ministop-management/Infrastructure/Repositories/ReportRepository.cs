using Infrastructure.Data;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {

        private readonly MinistopDataContextDataContext _context;
        public ReportRepository(MinistopDataContextDataContext context)
        {
            _context = context;
        }
        public List<SP_DanhSachCuaHangTheoThanhPhoResult> GetStoreByRegion(string region)
        {
            return _context.SP_DanhSachCuaHangTheoThanhPho(region).ToList();
        }
    }
}
