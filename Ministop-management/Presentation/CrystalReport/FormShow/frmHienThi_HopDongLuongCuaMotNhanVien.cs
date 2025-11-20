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
    public partial class frmHienThi_HopDongLuongCuaMotNhanVien : Form
    {
        private readonly IReportService _reportService;
        private readonly IStoreService _storeService;
        private readonly IEmployeeService _employeeService;

        public frmHienThi_HopDongLuongCuaMotNhanVien(
            IReportService reportService,
            IStoreService storeService,
            IEmployeeService employeeService)
        {
            InitializeComponent();
            _reportService = reportService;
            _storeService = storeService;
            _employeeService = employeeService;
        }

        private void frmHienThi_HopDongLuongNhanVien_Load(object sender, EventArgs e)
        {
            LoadCuaHang();
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

        private void cboStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboStore.SelectedValue == null) return;
            string storeId = cboStore.SelectedValue.ToString();
            LoadNhanVien(storeId);
        }

        private void LoadNhanVien(string storeId)
        {
            var result = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
            if (result.Succeeded && result.Data != null)
            {
                cboEmployee.DataSource = result.Data.ToList();
                cboEmployee.DisplayMember = "FullName";
                cboEmployee.ValueMember = "EmployeeId";
                cboEmployee.SelectedIndex = -1;
            }
            else
            {
                cboEmployee.DataSource = null;
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (cboEmployee.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string employeeId = cboEmployee.SelectedValue.ToString();
            HienThiReport(employeeId);
        }

        private void HienThiReport(string employeeId)
        {
            try
            {
                var main = _reportService.GetSalaryContractMain(employeeId);
                var allowances = _reportService.GetSalaryContractAllowances(employeeId);

                if (main == null)
                {
                    MessageBox.Show("Không tìm thấy hợp đồng lương cho nhân viên này!");
                    return;
                }

                var ds = new EmployeeReportDataset();

                // === BẢNG CHÍNH ===
                var rowMain = ds.SalaryContractMain.NewSalaryContractMainRow();
                rowMain.ContractID = main.ContractID ?? "";
                rowMain.EmployeeID = main.EmployeeID ?? "";
                rowMain.FullName = main.FullName ?? "";
                rowMain.Position = main.Position ?? "";
                rowMain.EmploymentType = main.EmploymentType ?? "";
                rowMain.StoreName = main.StoreName ?? "Chưa xác định";

                rowMain.BasicSalary = main.BasicSalary.HasValue ? main.BasicSalary.Value.ToString("N0") : "0";
                rowMain.HourlyRate = main.HourlyRate.HasValue ? main.HourlyRate.Value.ToString("N0") : "0";
                rowMain.StartDate = main.StartDate.ToLongDateString();
                rowMain.EndDate = main.EndDate.ToString();
                rowMain.TotalAllowance = main.TotalAllowance.ToString("N0");
                rowMain.EstimatedTotalIncome = main.EstimatedTotalIncome.ToString("N0");

                ds.SalaryContractMain.AddSalaryContractMainRow(rowMain);

                // === BẢNG PHỤ CẤP ===
                foreach (var item in allowances)
                {
                    var row = ds.AllowanceDetail.NewAllowanceDetailRow();
                    row.AllowanceName = item.AllowanceName ?? "";
                    row.Amount = item.Amount.ToString("N0");
                    ds.AllowanceDetail.AddAllowanceDetailRow(row);
                }

                string reportPath = Path.Combine(Application.StartupPath,
                "CrystalReport",
                "Report",
                "EmployeeReports", "Rpt_HopDonLuongCuaMotNhanVien.rpt");
                if (!File.Exists(reportPath))
                {
                    MessageBox.Show("Không tìm thấy file báo cáo: " + reportPath);
                    return;
                }

                var rpt = new ReportDocument();
                rpt.Load(reportPath);
                rpt.SetDataSource(ds);

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

        private void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}