using Domain.DTO;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IReportService
    {
        List<StoreDto> GetStoreByRegion(string region);
        List<EmployeeDto> GetEmployeesByStore(string store);
        List<StoreRevenueByMonthResultDto> GetRevenueByTimeResults(string storeID, DateTime fromDate, DateTime toDate);
        List<InventoryReportDto> GetInventoryReport(string storeId, int month, int year);
        List<StockImportReportDto> GetStockImportReport(string importID);
        List<ExportReportDto> GetStockExportReport(string exportID);
        List<sp_GetSalaryContractReportResult> GetSalaryContract(string employeeId);
        List<InvoiceReportDto> GetInvoiceProductReport(string invoiceID);
        List<StoreFinancialDto> GetStoreFinancialReportByMonth(string storeID);
      


    }
}
