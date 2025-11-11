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
    public partial class frmHienThi_HoaDon : Form
    {
        private string _invoiceID;
        private readonly IReportService _reportService;

        public frmHienThi_HoaDon(IReportService reportService, string invoiceID = null)
        {
            InitializeComponent();
            _reportService = reportService;
            _invoiceID = invoiceID;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (_invoiceID == null)
            {




                string invoiceID = txtTienNuoc.Text;
                if (string.IsNullOrWhiteSpace(invoiceID))
                {
                    return;
                }
                var ds = new SaleReportDataset();
                var import = _reportService.GetInvoiceProductReport(invoiceID);
                if (import == null)
                {
                    MessageBox.Show($"Hien khong co hoa don nao co ma so {invoiceID}");
                    return;
                }
                foreach (var item in import)
                {
                    ds.InvoiceTable.AddInvoiceTableRow(
                        item.InvoiceID,
                        item.InvoiceDate.ToShortDateString(),
                        item.StoreID,
                        item.StoreName,
                        item.StoreAddress,
                        item.StorePhone,
                        item.EmployeeID,
                        item.EmployeeName,
                        item.ProductName,
                        item.Unit,
                        item.Quantity.ToString(),
                        item.UnitPrice.ToString(),
                        item.Total.ToString(),
                        (item.TotalAmount ?? 0).ToString()
                    );
                }
                ReportDocument rpt = new ReportDocument();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport",
        "Report",
        "SaleReports",
        "Rpt_InHoaDon.rpt");
                rpt.Load(reportPath);
                rpt.SetDataSource(ds);
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
        }



        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmHienThi_HoaDon_Load(object sender, EventArgs e)
        {
            if (_invoiceID != null)
            {
                txtTienNuoc.Enabled = false;
                btnXem.Enabled = false;
                var ds = new SaleReportDataset();
                var import = _reportService.GetInvoiceProductReport(_invoiceID);
                if (import == null)
                {
                    MessageBox.Show($"Hien khong co hoa don nao co ma so {_invoiceID}");
                    return;
                }
                foreach (var item in import)
                {
                    ds.InvoiceTable.AddInvoiceTableRow(
                        item.InvoiceID,
                        item.InvoiceDate.ToShortDateString(),
                        item.StoreID,
                        item.StoreName,
                        item.StoreAddress,
                        item.StorePhone,
                        item.EmployeeID,
                        item.EmployeeName,
                        item.ProductName,
                        item.Unit,
                        item.Quantity.ToString(),
                        item.UnitPrice.ToString(),
                        item.Total.ToString(),
                        (item.TotalAmount ?? 0).ToString()
                    );
                }
                ReportDocument rpt = new ReportDocument();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport",
        "Report",
        "SaleReports",
        "Rpt_InHoaDon.rpt");
                rpt.Load(reportPath);
                rpt.SetDataSource(ds);
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
            else
            {
                btnXem.Enabled = true;
                txtTienNuoc.Enabled = true;
            }
        }
    }
}
