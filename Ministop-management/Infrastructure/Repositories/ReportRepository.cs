using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Model.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
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
        public List<SP_GetEmployeesByStoreResult> GetEmployeesByStore(string store)
        {
            return _context.SP_GetEmployeesByStore(store).ToList();
        }
        public List<SP_StoreRevenueByTimeResultResult> GetStoreRevenueByTime(string storeID, DateTime fromDate, DateTime toDate)
        {
            return _context.SP_StoreRevenueByTimeResult(storeID,fromDate,toDate).ToList();
        }
        public List<SP_StockImportReportResult> GetStockImportReport(string importID)
        {
            return _context.SP_StockImportReport(importID).ToList();
        }
        public List<SP_StockExportResult> GetStockExportReport(string exportID)
        {
            return _context.SP_StockExport(exportID).ToList();
        }
        public List<InventoryReportDto> GetInventoryReport(string storeId, int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);
            var products = _context.Products.Where(p => !p.IsDeleted && _context.StockDetails.Any(x=> x.StoreID == storeId&& x.ProductID == p.ProductID));
            var report = from p in products
                         let opening = (
                            from sd in _context.StockDetails
                            join sh in _context.StockHistories on sd.StockDetailID equals sh.StockDetailID
                            where sd.StoreID == storeId
                               && sd.ProductID == p.ProductID
                               && sh.ChangeDate < startDate
                            select (int?)sh.QuantityChange
                         ).Sum() ?? 0

                         let importInPeriod = (
                            from sim in _context.StockImports
                            join sid in _context.StockImportDetails on sim.ImportID equals sid.ImportID
                            where sim.StoreID == storeId
                              && sid.ProductID == p.ProductID
                              && sim.ImportDate >= startDate
                              && sim.ImportDate <= endDate
                            select (int?)sid.Quantity
                         ).Sum() ?? 0

                         let exportInPeriod = (
                            from se in _context.StockExports
                            join sed in _context.StockExportDetails on se.ExportID equals sed.ExportID
                            where se.StoreID == storeId
                              && sed.ProductID == p.ProductID
                              && se.ExportDate >= startDate
                              && se.ExportDate <= endDate
                            select (int?)sed.Quantity
                         ).Sum() ?? 0

                         let saleInPeriod = (
                            from i in _context.Invoices
                            join id in _context.InvoiceDetails on i.InvoiceID equals id.InvoiceID
                            where i.StoreID == storeId
                              && id.ProductID == p.ProductID
                              && i.InvoiceDate >= startDate
                              && i.InvoiceDate <= endDate
                            select (int?)id.Quantity
                         ).Sum() ?? 0

                         let checkPlus = (
                            from sc in _context.StockChecks
                            join scd in _context.StockCheckDetails on sc.CheckID equals scd.CheckID
                            where sc.StoreID == storeId
                              && scd.ProductID == p.ProductID
                              && sc.CheckDate >= startDate
                              && sc.CheckDate <= endDate
                              && scd.QuantityActual > scd.QuantitySystem
                            select (int?)(scd.QuantityActual - scd.QuantitySystem)
                         ).Sum() ?? 0

                         let checkMinus = (
                            from sc in _context.StockChecks
                            join scd in _context.StockCheckDetails on sc.CheckID equals scd.CheckID
                            where sc.StoreID == storeId
                              && scd.ProductID == p.ProductID
                              && sc.CheckDate >= startDate
                              && sc.CheckDate <= endDate
                              && scd.QuantityActual < scd.QuantitySystem
                            select (int?)(scd.QuantityActual - scd.QuantitySystem)
                         ).Sum() ?? 0

                         let totalOut = exportInPeriod + saleInPeriod
                         let finalStock = opening + importInPeriod - totalOut + (checkPlus + checkMinus)

                         select new InventoryReportDto
                         {
                             ProductID = p.ProductID,
                             ProductName = p.ProductName,
                             Unit = p.Unit,
                             OpeningStock = opening,
                             ImportInPeriod = importInPeriod,
                             ExportInPeriod = exportInPeriod,
                             SaleInPeriod = saleInPeriod,
                             TotalExport = totalOut,
                             CheckIncrease = checkPlus,
                             CheckDecrease = checkMinus,
                             ClosingStock = finalStock
                         };

            return report.ToList();
        }

        public List<SP_InvoiceReportResult> GetInvoiceProductReport(string invoiceID)
        {
            return _context.SP_InvoiceReport(invoiceID).ToList();
        }
        public List<SP_StoreFinancialReportByMonthResult> GetStoreFinancialReportByMonth(string storeID)
        {
            return _context.SP_StoreFinancialReportByMonth(storeID).ToList();
        }

        public List<sp_GetSalaryContractReportResult> GetSalaryContract(string EmployeeID)
        {
            return _context.sp_GetSalaryContractReport(EmployeeID).ToList();
        }

        public List<SalaryContractAllowanceReportDto> GetSalaryContractAllowances(string employeeId)
        {
            var result = new List<SalaryContractAllowanceReportDto>();

            using (var connection = new SqlConnection(_context.Connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetSalaryContractReport", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);

                    using (var reader = command.ExecuteReader())
                    {
                        // Bỏ qua bảng đầu tiên
                        reader.NextResult();

                        // Đọc bảng phụ cấp
                        while (reader.Read())
                        {
                            result.Add(new SalaryContractAllowanceReportDto
                            {
                                AllowanceName = reader["AllowanceName"]?.ToString() ?? "",
                                Amount = Convert.ToDecimal(reader["Amount"])
                            });
                        }
                    }
                }
            }

            return result;
        }
        public List<FrequentlyLostProductDto> GetFrequentlyLostProductsByStoreAndDateRange(
     string storeId,
     DateTime? fromDate = null,
     DateTime? toDate = null,
     int threshold = 2)
        {
            var query = _context.StockCheckDetails
                .Where(detail => (detail.QuantityActual - detail.QuantitySystem) < 0
                                 && detail.StockCheck.StoreID == storeId);

            if (fromDate.HasValue)
                query = query.Where(detail => detail.StockCheck.CheckDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(detail => detail.StockCheck.CheckDate <= toDate.Value);

            var result = query
                .GroupBy(detail => new { detail.ProductID, detail.Product.ProductName })
                .Select(g => new
                {
                    g.Key.ProductID,
                    g.Key.ProductName,
                    TimesLost = g.Count()
                })
                .Where(x => x.TimesLost >= threshold)
                .OrderByDescending(x => x.TimesLost)
                .ToList();

            return result.Select(x => new FrequentlyLostProductDto
            {
                ProductID = x.ProductID,
                ProductName = x.ProductName,
                TimesLost = x.TimesLost
            }).ToList();
        }




        public SalarySlipMainDto GetSalarySlipMain(string employeeId, string monthYear)
        {
            var result = new SalarySlipMainDto();

            using (var connection = new SqlConnection(_context.Connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetSalarySlip_ByEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.Parameters.AddWithValue("@MonthYear", monthYear);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new SalarySlipMainDto
                            {
                                SalaryID = reader["SalaryID"]?.ToString(),
                                ContractID = reader["ContractID"]?.ToString(),
                                EmployeeID = reader["EmployeeID"]?.ToString(),
                                FullName = reader["FullName"]?.ToString(),
                                Position = reader["Position"]?.ToString(),
                                EmploymentType = reader["EmploymentType"]?.ToString(),
                                StoreName = reader["StoreName"]?.ToString(),
                                MonthYear = reader["MonthYear"]?.ToString(),
                                BasicSalary = reader["BasicSalary"] as decimal?,
                                HourlyRate = reader["HourlyRate"] as decimal?,
                                Bonus = Convert.ToDecimal(reader["Bonus"]),
                                Deduction = Convert.ToDecimal(reader["Deduction"]),
                                TotalHoursWorked = Convert.ToInt32(reader["TotalHoursWorked"]),
                                TotalAllowance = Convert.ToDecimal(reader["TotalAllowance"])
                            };
                        }
                    }
                }
            }
            return result;
        }

        public List<SalarySlipAllowanceDto> GetSalarySlipAllowances(string employeeId)
        {
            var result = new List<SalarySlipAllowanceDto>();

            using (var connection = new SqlConnection(_context.Connection.ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("sp_GetSalarySlip_ByEmployee", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmployeeID", employeeId);
                    command.Parameters.AddWithValue("@MonthYear", "2000-01"); // Bất kỳ tháng nào, vì bảng phụ cấp không dùng

                    using (var reader = command.ExecuteReader())
                    {
                        // Bỏ qua bảng chính
                        reader.NextResult();

                        // Đọc bảng phụ cấp
                        while (reader.Read())
                        {
                            result.Add(new SalarySlipAllowanceDto
                            {
                                AllowanceName = reader["AllowanceName"]?.ToString() ?? "",
                                Amount = Convert.ToDecimal(reader["Amount"])
                            });
                        }
                    }
                }
            }
            return result;
        }

        public List<SalaryListDto> GetSalaryListByStore(string storeId, string monthYear)
        {
            return _context.sp_GetSalaryList_ByStore(storeId, monthYear)
                .Select(x => new SalaryListDto
                {
                    SalaryID = x.SalaryID,
                    EmployeeID = x.EmployeeID,
                    FullName = x.FullName,
                    Position = x.Position,
                    EmploymentType = x.EmploymentType,
                    BasicSalary = x.BasicSalary,
                    HourlyRate = x.HourlyRate,
                    Bonus = x.Bonus,
                    Deduction = x.Deduction,
                    TotalIncome = x.TotalIncome ?? 0
                }).ToList();
        }
        public List<sp_GetTop3BestSellingStoresResult> GetTop3BestSellingStores(DateTime stardate, DateTime enddate)
        {
            return _context.sp_GetTop3BestSellingStores(stardate,enddate).ToList();
        }
    }

}
