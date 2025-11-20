using CrystalDecisions.CrystalReports.Engine;
using Domain.DTO;
using Presentation.CrystalReport.DataSets;
using Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Presentation.CrystalReport.FormShow
{
    public partial class frmHienThi_DanhSachLuong_CuaHang : Form
    {
        private readonly IReportService _reportService;
        private readonly IStoreService _storeService;

        public frmHienThi_DanhSachLuong_CuaHang(
            IReportService reportService,
            IStoreService storeService)
        {
            InitializeComponent();
            _reportService = reportService;
            _storeService = storeService;
        }

        private void frmHienThi_DanhSachLuong_CuaHang_Load(object sender, EventArgs e)
        {
            LoadCuaHang();
            dtpMonth.Value = DateTime.Today;
        }

        private void LoadCuaHang()
        {
            var result = _storeService.GetAll();
            if (result.Succeeded && result.Data != null)
            {
                cboStore.DataSource = result.Data.ToList();
                cboStore.DisplayMember = "StoreName";
                cboStore.ValueMember = "StoreId";
                cboStore.SelectedIndex = -1;
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (cboStore.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cửa hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string storeId = cboStore.SelectedValue.ToString();
            string monthYear = dtpMonth.Value.ToString("yyyy-MM");

            HienThiReport(storeId, monthYear);
        }

        private void HienThiReport(string storeId, string monthYear)
        {
            try
            {
                var list = _reportService.GetSalaryListByStore(storeId, monthYear);

                if (list == null || !list.Any())
                {
                    MessageBox.Show($"Không có dữ liệu lương tháng {monthYear} cho cửa hàng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var ds = new EmployeeReportDataset();

                foreach (var item in list)
                {
                    var row = ds.SalaryList.NewSalaryListRow();
                    row.SalaryID = item.SalaryID ?? "";
                    row.EmployeeID = item.EmployeeID ?? "";
                    row.FullName = item.FullName ?? "";
                    row.Position = item.Position ?? "";
                    row.EmploymentType = item.EmploymentType ?? "";
                    row.BasicSalary = item.BasicSalary.HasValue ? item.BasicSalary.Value.ToString("N0") : "0";
                    row.HourlyRate = item.HourlyRate.HasValue ? item.HourlyRate.Value.ToString("N0") : "0";
                    row.Bonus = item.Bonus.ToString("N0");
                    row.Deduction = item.Deduction.ToString("N0");
                    row.TotalIncome = item.TotalIncome; // decimal → đã fix Sum trong .rpt
                    ds.SalaryList.AddSalaryListRow(row);
                }

                string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "Report", "EmployeeReports", "Rpt_DanhSachLuong_CuaHang.rpt");
                if (!File.Exists(reportPath))
                {
                    MessageBox.Show("Không tìm thấy file báo cáo: " + reportPath);
                    return;
                }

                var rpt = new ReportDocument();
                rpt.Load(reportPath);
                rpt.SetDataSource(ds);

                // Truyền tên cửa hàng vào Parameter
                var storeName = cboStore.Text;
                rpt.SetParameterValue("Parameter_StoreName", storeName);
                rpt.SetParameterValue("Parameter_MonthYear", "Tháng " + dtpMonth.Value.ToString("MM/yyyy"));

                crystalReportViewer1.ReportSource = rpt;
                crystalReportViewer1.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}