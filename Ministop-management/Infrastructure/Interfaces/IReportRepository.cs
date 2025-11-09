using Domain.DTO;
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
        List<SP_StoreRevenueByTimeResultResult> GetStoreRevenueByTime(string storeID, DateTime fromDate, DateTime toDate);
        List<InventoryReportDto> GetInventoryReport(string storeId, int month, int year);
        List<SP_StockExportResult> GetStockExportReport(string exportID);
        List<SP_StockImportReportResult> GetStockImportReport(string importID);
        List<sp_GetSalaryContractReportResult> GetSalaryContract(string EmployeeID);
        List<SP_InvoiceReportResult> GetInvoiceProductReport(string invoiceID);
        List<SP_StoreFinancialReportByMonthResult> GetStoreFinancialReportByMonth(string storeID);
        List<SalaryContractAllowanceReportDto> GetSalaryContractAllowances(string employeeId);
    }
}
