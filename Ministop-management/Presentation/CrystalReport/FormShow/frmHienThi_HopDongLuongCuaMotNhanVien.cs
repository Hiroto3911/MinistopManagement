using CrystalDecisions.CrystalReports.Engine;
using Domain.DTO;
using Presentation.CrystalReport.DataSets;
using Services.Interfaces;
using Services.Services;
using Shared.Security;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Presentation.CrystalReport.FormShow
{
    public partial class frmHienThi_HopDongLuongNhanVien : Form
    {
        private readonly IReportService _reportService;
        private readonly IStoreService _storeService;
        private readonly IEmployeeService _employeeService;

        public frmHienThi_HopDongLuongNhanVien(
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
            LoadCboStores();
        }

        private void LoadCboStores()
        {
            var result = _storeService.GetAll();
            if (result.Data != null)
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
            LoadCboEmployees(storeId);
        }

        private void LoadCboEmployees(string storeId)
        {
            var employees = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
            if (employees.Data != null)
            {
                cboEmployee.DataSource = employees.Data.ToList();
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
            //ShowSalaryContractReport(employeeId);
        }

        //private void ShowSalaryContractReport(string employeeId)
        //{
        //    try
        //    {
        //        // 1. Lấy dữ liệu từ SP
        //        var contractData = _reportService.(employeeId);
        //        if (contractData == null || !contractData.Any())
        //        {
        //            MessageBox.Show("Không tìm thấy hợp đồng lương cho nhân viên này!", "Thông báo");
        //            return;
        //        }

        //        // 2. Tạo Dataset
        //        var ds = new EmployeeReportDataset.GetSalaryContractReportDataTable();

        //        // 3. Fill thông tin chính (chỉ 1 dòng)
        //        var mainRow = contractData.First();
        //        ds.SalaryContractReport.AddSalaryContractReportRow(
        //            mainRow.ContractID,
        //            mainRow.EmployeeID,
        //            mainRow.FullName,
        //            mainRow.Position,
        //            mainRow.EmploymentType,
        //            mainRow.StoreName ?? "",
        //            mainRow.BasicSalary ?? 0,
        //            mainRow.HourlyRate ?? 0,
        //            mainRow.StartDate,
        //            mainRow.EndDate ?? (object)DBNull.Value
        //        );

        //        // 4. Tính TotalAllowance và EstimatedTotalIncome
        //        decimal totalAllowance = contractData.Sum(x => x.TotalAllowance);
        //        decimal estimatedIncome = (mainRow.BasicSalary ?? 0) + totalAllowance;

        //        // 5. Tạo bảng phụ cấp (nếu có nhiều dòng)
        //        var allowanceTable = ds.AllowanceTable;
        //        foreach (var item in contractData)
        //        {
        //            // Giả sử SP trả về nhiều dòng phụ cấp → cần mở rộng SP
        //            // Hoặc lấy riêng từ Service
        //            // → Dùng cách 2: Lấy phụ cấp riêng
        //        }

        //        // → Cách tốt hơn: Dùng Service lấy phụ cấp
        //        var allowanceService = Program.ServiceProvider.GetService(typeof(ISalaryContractAllowanceService)) as ISalaryContractAllowanceService;
        //        var contractResult = new SalaryContractService(
        //            Program.ServiceProvider.GetService<IMinistopUnitOfWork>(),
        //            Program.ServiceProvider.GetService<IDateTimeService>(),
        //            Program.ServiceProvider.GetService<IUserSession>(),
        //            Program.ServiceProvider.GetService<IMapper>()
        //        ).GetCurrentContractByEmployeeId(employeeId);

        //        if (contractResult.Succeeded)
        //        {
        //            var allowances = allowanceService.GetByContractId(contractResult.Data.ContractId);
        //            if (allowances.Succeeded)
        //            {
        //                foreach (var a in allowances.Data)
        //                {
        //                    var allowanceEntity = allowanceService.GetById(a.AllowanceId);
        //                    if (allowanceEntity != null)
        //                    {
        //                        decimal amount = a.CustomAmount ?? allowanceEntity.;
        //                        allowanceTable.AddAllowanceTableRow(
        //                            allowanceEntity.AllowanceName,
        //                            amount
        //                        );
        //                        totalAllowance += amount;
        //                    }
        //                }
        //            }
        //        }

        //        // Cập nhật lại tổng
        //        estimatedIncome = (mainRow.BasicSalary ?? 0) + totalAllowance;

        //        // 6. Gán Parameter cho Report (Tổng tiền)
        //        ReportDocument rpt = new ReportDocument();
        //        string reportPath = Path.Combine(Application.StartupPath, "CrystalReport", "Report", "Rpt_HopDongLuong_NhanVien.rpt");

        //        if (!File.Exists(reportPath))
        //        {
        //            MessageBox.Show($"Không tìm thấy file: {reportPath}");
        //            return;
        //        }

        //        rpt.Load(reportPath);
        //        rpt.SetDataSource(ds);

        //        // Gán Parameter
        //        rpt.SetParameterValue("TotalAllowance", totalAllowance);
        //        rpt.SetParameterValue("EstimatedTotalIncome", estimatedIncome);

        //        crystalReportViewer1.ReportSource = rpt;
        //        crystalReportViewer1.Refresh();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Lỗi: " + ex.Message);
        //    }
        //}

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}