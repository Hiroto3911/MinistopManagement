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
    public partial class frmHienThi_DanhSachNhanVienTheoCuaHang : Form
    {
        private readonly IReportService _reportService;
        private readonly IStoreService _storeService;
        public frmHienThi_DanhSachNhanVienTheoCuaHang(IReportService reportService, IStoreService storeService)
        {
            InitializeComponent();
            _reportService = reportService;
            _storeService = storeService;
        }

        private void frmHienThi_DanhSachNhanVienTheoCuaHang_Load(object sender, EventArgs e)
        {
            // Load danh sách cửa hàng lên combo
            var result = _storeService.GetAll();
            if (result != null && result.Data != null)
            {
                cboStore.DataSource = result.Data.ToList();
                cboStore.DisplayMember = "StoreName";
                cboStore.ValueMember = "StoreId";
            }
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            if (cboStore.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cửa hàng!");
                return;
            }

            string storeId = cboStore.SelectedValue.ToString();

            var ds = new EmployeeReportDataset();

            // ✅ Lấy trực tiếp từ ReportRepository để có thông tin Store
            var employees = _reportService.GetEmployeesByStore(storeId);

            if (employees == null || !employees.Any())
            {
                MessageBox.Show("Hiện không có nhân viên nào thuộc cửa hàng này!");
                return;
            }

            foreach (var emp in employees)
            {
                ds.EmployeeTableByStore.AddEmployeeTableByStoreRow(
                    emp.EmployeeId,
                    emp.FullName,
                    emp.Gender ? "Nam" : "Nữ",
                    emp.BirthDate.ToString("dd/MM/yyyy"),
                    emp.Phone,
                    emp.Position,
                    emp.EmploymentType
                );
            }

            ReportDocument rpt = new ReportDocument();
            string reportPath = Path.Combine(Application.StartupPath,
                "CrystalReport",
                "Report",
                "EmployeeReports",
                "Rpt_DanhSachNhanVienTheoCuaHang.rpt");

            if (!File.Exists(reportPath))
            {
                MessageBox.Show($"Không tìm thấy file report: {reportPath}");
                return;
            }

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
