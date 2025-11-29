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
    public partial class frmHienThi_PhieuXuat : Form
    {
        private readonly IReportService _reportService;

        public frmHienThi_PhieuXuat(IReportService reportService)
        {
            InitializeComponent();
            _reportService = reportService;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            string exportID = txtTienNuoc.Text;
            if (string.IsNullOrWhiteSpace(exportID))
            {
                return;
            }
            var ds = new StockReportDataset();
            var export = _reportService.GetStockExportReport(exportID);
            if (export == null)
            {
                MessageBox.Show($"Hien khong co phieu xuat nao co ma so {exportID}");
                return;
            }
            foreach (var item in export) // export = List<ExportReportDto>
            {
                ds.StockExport.AddStockExportRow(
                    item.ExportID,
                    item.ExportDate.ToShortDateString(),
                    item.StoreID,
                    item.StoreName,
                    item.Address,
                    item.EmployeeID,
                    item.FullName,
                    item.ProductName,
                    item.Quantity.ToString(),
                    item.UnitPrice.ToString(),
                    item.Total.ToString(),
                    item.TotalAmount.ToString(),
                    item.Reason,
                    item.TypeExport,
                    item.Status.ToString()


                );
            }

            ReportDocument rpt = new ReportDocument();
            string reportPath = Path.Combine(Application.StartupPath, "CrystalReport",
    "Report",
    "StockReports",
    "Rpt_InPhieuXuat.rpt");
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
