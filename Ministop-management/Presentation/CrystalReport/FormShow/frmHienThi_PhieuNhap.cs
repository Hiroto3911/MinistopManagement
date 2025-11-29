using CrystalDecisions.CrystalReports.Engine;
using Presentation.CrystalReport.DataSets;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.CrystalReport.FormShow
{
    public partial class frmHienThi_PhieuNhap : Form
    {
    

        private readonly IReportService _reportService;

        public frmHienThi_PhieuNhap(IReportService reportService)
        {
            InitializeComponent();
            _reportService = reportService;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            string importID = txtTienNuoc.Text;
            if (string.IsNullOrWhiteSpace(importID))
            {
                return;
            }
            var ds = new StockReportDataset();
            var import = _reportService.GetStockImportReport(importID);
            if (import == null)
            {
                MessageBox.Show($"Hien khong co phieu nhap nao co ma so {importID}");
                return;
            }
            foreach (var item in import)
            {
                ds.StockImport.AddStockImportRow(
                  item.ImportID,
                  item.ImportDate.ToShortDateString(),
                    item.StoreID,
                  item.SupplierID,
                  item.StoreName,
                  item.Address,
                  item.SupplierName,
                  item.EmployeeID,
                  item.FullName,
                  item.ProductName,
                  item.Quantity.ToString(),
                  item.UnitPrice.ToString(),
                  item.Total.ToString(),
                  item.TotalAmount.ToString()
                
                );
            }
            ReportDocument rpt = new ReportDocument();
            string reportPath = Path.Combine(Application.StartupPath, "CrystalReport",
    "Report",
    "StockReports",
    "Rpt_InPhieuNhap.rpt");
            rpt.Load(reportPath);
            rpt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }

      

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
