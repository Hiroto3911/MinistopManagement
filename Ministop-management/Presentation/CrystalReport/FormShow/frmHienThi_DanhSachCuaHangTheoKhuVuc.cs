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
    public partial class frmHienThi_DanhSachCuaHangTheoKhuVuc : Form
    {
        private readonly IReportService _reportService;

        public frmHienThi_DanhSachCuaHangTheoKhuVuc(IReportService reportService )
        {
            InitializeComponent();
            _reportService = reportService;
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            string region = cboKhuVuc.SelectedValue.ToString();
            if (string.IsNullOrWhiteSpace(region)) {
                return;
            }
            var ds = new StoreReportDataset();
            var stores = _reportService.GetStoreByRegion(region);
            if (stores == null) {
                MessageBox.Show($"Hien khong co cua nao trong khu vuc {region}");
                return;
            }
            foreach (var store in stores)
            {
                ds.StoreTable.AddStoreTableRow(
                   store.StoreId,
                   store.StoreName,
                   store.Address,
                   store.Phone
                );
            }
            ReportDocument rpt = new ReportDocument();
            string reportPath = Path.Combine(Application.StartupPath, "CrystalReport",
    "Report",
    "StoreReports",
    "Rpt_DanhSachCuaHangTheoKhuVuc.rpt");
            rpt.Load(reportPath);
            rpt.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rpt;
            crystalReportViewer1.Refresh();
        }

        private void frmHienThi_DanhSachCuaHangTheoKhuVuc_Load(object sender, EventArgs e)
        {
            var list = new List<string>() { "TP.HCM","Hà Nội"};
            cboKhuVuc.DataSource = list; 
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
