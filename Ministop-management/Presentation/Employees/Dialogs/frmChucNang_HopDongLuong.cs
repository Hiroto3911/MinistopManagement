using Domain.DTO;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;
namespace Presentation
{
    public partial class frmChucNang_HopDongLuong : Form
    {
        private readonly ISalaryContractService _salaryContractService;
        private readonly IEmployeeService _employeeService;
        private readonly IUnityContainer _container;
        private readonly string _employeeId; // Optional employeeId từ luồng tạo nhân viên
        public event EventHandler DataChanged;
        public frmChucNang_HopDongLuong(
        ISalaryContractService salaryContractService,
        IEmployeeService employeeService,
        IUnityContainer container,
        string employeeId = null)
        {
            InitializeComponent();
            _salaryContractService = salaryContractService;
            _employeeService = employeeService;
            _container = container;
            _employeeId = employeeId;
            LoadEmployees();
            if (!string.IsNullOrEmpty(_employeeId))
            {
                cboMaNhanVien.SelectedValue = _employeeId;
                cboMaNhanVien.Enabled = false; // Không cho thay đổi nếu từ luồng tạo
            }
        }
        private void LoadEmployees()
        {
            var employees = _employeeService.GetAll();
            if (employees.Succeeded && employees.Data != null)
            {
                cboMaNhanVien.DataSource = employees.Data.ToList();
                cboMaNhanVien.DisplayMember = "EmployeeId";
                cboMaNhanVien.ValueMember = "EmployeeId";
            }
            else
            {
                MessageBox.Show("Không thể tải danh sách nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cboMaNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboMaNhanVien.SelectedValue == null) return;
            string selectedEmployeeId = cboMaNhanVien.SelectedValue.ToString();
            var employee = _employeeService.GetEmployeeByID(selectedEmployeeId);
            if (employee.Succeeded && employee.Data != null)
            {
                txtTenNhanVien.Text = employee.Data.FullName;
                ConfigureSalaryFields(employee.Data.EmploymentType);
            }
        }
        private void ConfigureSalaryFields(string employmentType)
        {
            if (employmentType == "Fulltime")
            {
                txtLuongCoBan.Enabled = true;
                txtDonGiaGio.Enabled = false;
                txtDonGiaGio.Text = string.Empty;
            }
            else if (employmentType == "Parttime")
            {
                txtLuongCoBan.Enabled = false;
                txtDonGiaGio.Enabled = true;
                txtLuongCoBan.Text = string.Empty;
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e) // Btn Lưu
        {
            if (!ValidateInput()) return;
            var contract = new SalaryContractDto
            {
                EmployeeId = cboMaNhanVien.SelectedValue.ToString(),
                StartDate = dtpNgayBatDau.Value,
                EndDate = dtpNgayKetThuc.Value
            };
            if (txtLuongCoBan.Enabled)
                contract.BasicSalary = decimal.Parse(txtLuongCoBan.Text);
            else
                contract.BasicSalary = null;
            if (txtDonGiaGio.Enabled)
                contract.HourlyRate = decimal.Parse(txtDonGiaGio.Text);
            else
                contract.HourlyRate = null;
            var result = _salaryContractService.Create(contract);
            if (result.Succeeded)
            {
                MessageBox.Show("Tạo hợp đồng lương thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
                var employee = _employeeService.GetEmployeeByID(contract.EmployeeId);
                if (employee.Succeeded && employee.Data.EmploymentType == "Fulltime")
                {
                    // Chuyển sang thêm phụ cấp
                    var frmPhuCap = _container.Resolve<frmChucNang_HopDongLuong_PhuCap>(
                    new ParameterOverride("contractId", contract.ContractId));
                    frmPhuCap.ShowDialog();
                }
                this.Close();
            }
            else
            {
                MessageBox.Show($"Lỗi: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ValidateInput()
        {
            if (cboMaNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtLuongCoBan.Enabled && !decimal.TryParse(txtLuongCoBan.Text, out _))
            {
                MessageBox.Show("Lương cơ bản không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtDonGiaGio.Enabled && !decimal.TryParse(txtDonGiaGio.Text, out _))
            {
                MessageBox.Show("Đơn giá giờ không hợp lệ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtpNgayBatDau.Value > dtpNgayKetThuc.Value)
            {
                MessageBox.Show("Ngày bắt đầu phải trước ngày kết thúc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}