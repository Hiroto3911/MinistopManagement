using Domain.DTO;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.UnitOfWorks;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class ReportService : IReportService
    {
        private readonly IMinistopUnitOfWork _ministopUnitOfWork;

        public ReportService(IMinistopUnitOfWork ministopUnitOfWork)
        {
            _ministopUnitOfWork = ministopUnitOfWork;
        }
        public List<StoreDto> GetStoreByRegion(string region)
        {
            var list = _ministopUnitOfWork.ReportRepository.GetStoreByRegion(region);
            var stores = list.Select(x => new StoreDto { StoreId = x.StoreID, StoreName = x.StoreName, Address = x.Address, Phone = x.Phone });
            return stores.ToList();
        }

        public  List<EmployeeDto> GetEmployeesByStore(string storeId)
        {
            var list = _ministopUnitOfWork.ReportRepository.GetEmployeesByStore(storeId);

            var employees = list.Select(x => new EmployeeDto
            {
                EmployeeId = x.EmployeeID,
                FullName = x.FullName,
                Gender = x.Gender,
                BirthDate = x.BirthDate,
                Phone = x.Phone,
                Position = x.Position,
                EmploymentType = x.EmploymentType,
            });

            return employees.ToList();
        }
        public  List<StoreRevenueByMonthResultDto> GetRevenueByTimeResults(string storeID, DateTime fromDate, DateTime toDate)
        {
            var list =  _ministopUnitOfWork.ReportRepository.GetStoreRevenueByTime(storeID, fromDate, toDate);
            return list.Select(x => new StoreRevenueByMonthResultDto
            {
                StoreID = x.StoreID,
                StoreName = x.StoreName,
                Month = x.Month?? 1,
                Year = x.Year?? 2025,
                Revenue = x.Revenue ??0,
            }).ToList();
        }
        public List<InventoryReportDto> GetInventoryReport(string storeId, int month, int year)
        {
            return _ministopUnitOfWork.ReportRepository.GetInventoryReport(storeId, month, year);
        }
        public List<StockImportReportDto> GetStockImportReport(string importID)
        {
            var list = _ministopUnitOfWork.ReportRepository.GetStockImportReport(importID);

            var result = list.Select(x => new StockImportReportDto
            {
                ImportID = x.ImportID,
                ImportDate = x.ImportDate,
                StoreID = x.StoreID,
                SupplierID = x.SupplierID,
                StoreName = x.StoreName,
                Address = x.Address,
                SupplierName = x.SupplierName,
                EmployeeID = x.EmployeeID,
                FullName = x.FullName,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Total = x.Total,
                TotalAmount = x.totalAmount
            });

            return result.ToList();
        }


        public List<ExportReportDto> GetStockExportReport(string exportID)
        {
            var list = _ministopUnitOfWork.ReportRepository.GetStockExportReport(exportID);

            var result = list.Select(x => new ExportReportDto
            {
                ExportID = x.ExportID,
                ExportDate = x.ExportDate,
                StoreID = x.StoreID,
                StoreName = x.StoreName,
                Address = x.Address,
                EmployeeID = x.EmployeeID,
                FullName = x.FullName,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Total = x.Total,
                TotalAmount = x.totalAmount
            });

            return result.ToList();
        }

        public List<sp_GetSalaryContractReportResult> GetSalaryContract(string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
                return new List<sp_GetSalaryContractReportResult>();

            return _ministopUnitOfWork.ReportRepository.GetSalaryContract(employeeId);
        }
        public List<InvoiceReportDto> GetInvoiceProductReport(string invoiceID)
        {
            var list = _ministopUnitOfWork.ReportRepository.GetInvoiceProductReport(invoiceID);

            var result = list.Select(x => new InvoiceReportDto
            {
                InvoiceID = x.InvoiceID,
                InvoiceDate = x.InvoiceDate,
                StoreID = x.StoreID,
                StoreName = x.StoreName,
                StoreAddress = x.StoreAddress,
                StorePhone = x.StorePhone,
                EmployeeID = x.EmployeeID,
                EmployeeName = x.EmployeeName,
                ProductName = x.ProductName,
                Unit = x.Unit,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                Total = x.Total,
                TotalAmount = x.TotalAmount
            }).ToList(); // <-- sửa đúng ở đây

            return result;
        }
    }
}
