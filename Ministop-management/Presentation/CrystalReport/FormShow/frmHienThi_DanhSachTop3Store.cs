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
    public partial class frmHienThi_DanhSachTop3Store : Form
    {
        private readonly IReportService _reportService;

        public frmHienThi_DanhSachTop3Store(IReportService reportService, string invoiceID = null)
        {
            InitializeComponent();
            _reportService = reportService;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {

            DateTime timestar = DateTime.Parse(dtpMonthstar.Text);
            DateTime timeend = DateTime.Parse(dtpMothend.Text);


            // Validate ngày tháng
            if (timestar > timeend)
            {
                MessageBox.Show("Ngày bắt đầu phải nhỏ hơn ngày kết thúc!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var ds = new StoreReportDataset();
                var import = _reportService.GetTop3BestSellingStores(timestar,timeend);
                if (import == null)
                {
                    MessageBox.Show($"Hien khong co hoa don nao co ma so ");
                    return;
                }
                foreach (var item in import)
                {
                    ds.Get_Top_3_Store.AddGet_Top_3_StoreRow(
                       item.StoreID,
                       item.StoreName,
                       item.Address,
                       item.Phone,
                       item.TotalInvoices.ToString(),
                       item.TotalRevenue.ToString(),
                       item.TotalProductsSold.ToString(),
                       item.AverageInvoiceValue.ToString()
                    );
                
                ReportDocument rpt = new ReportDocument();
                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport",
        "Report",
        "StoreReports",
        "Rpt_InTop3Store.rpt");
                rpt.Load(reportPath);
                rpt.SetDataSource(ds);
                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
        }



        private void btnThoat_Click(object sender, EventArgs e)
        {
        }

        private void btnThoat_Click_1(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}

