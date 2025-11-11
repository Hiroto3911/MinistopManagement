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
            // 1. Kiểm tra chọn nhân viên
            if (cboMaNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            const decimal MIN_BASIC_SALARY_HCM = 4_960_000m;   // 4.96 triệu
            const decimal MIN_HOURLY_RATE_HCM = 22_500m;      // 22.500 ₫/giờ

            // Làm sạch dữ liệu nhập (loại bỏ dấu chấm, phẩy)
            string luongCoBanText = txtLuongCoBan.Text.Replace(",", "").Replace(".", "").Trim();
            string donGiaGioText = txtDonGiaGio.Text.Replace(",", "").Replace(".", "").Trim();

            decimal? basicSalary = null;
            decimal? hourlyRate = null;

            // 2. Validate Lương cơ bản (Full-time) - BẮT BUỘC >= 4.960.000
            if (txtLuongCoBan.Enabled)
            {
                if (string.IsNullOrWhiteSpace(luongCoBanText) || !decimal.TryParse(luongCoBanText, out decimal salary))
                {
                    MessageBox.Show("Vui lòng nhập lương cơ bản hợp lệ!\nVí dụ: 5000000", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtLuongCoBan.Focus();
                    return false;
                }

                if (salary < MIN_BASIC_SALARY_HCM)
                {
                    var result = MessageBox.Show(
                        $"CẢNH BÁO VI PHẠM PHÁP LUẬT!\n\n" +
                        $"Lương cơ bản: {salary:#,##0} ₫\n" +
                        $"Phải ≥ 4.960.000 ₫ (Vùng I - TP.HCM)\n" +
                        $"Theo Nghị định 74/2024/NĐ-CP\n\n" +
                        $"Bạn có chắc muốn lưu hợp đồng vi phạm luật lao động?",
                        "Vi phạm lương tối thiểu TP.HCM",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);

                    if (result == DialogResult.No)
                        return false;
                }

                basicSalary = salary;
                txtLuongCoBan.Text = salary.ToString("N0");
            }

            // 3. Validate Đơn giá giờ (Part-time) - BẮT BUỘC >= 22.500 ₫/giờ
            if (txtDonGiaGio.Enabled)
            {
                if (string.IsNullOrWhiteSpace(donGiaGioText) || !decimal.TryParse(donGiaGioText, out decimal rate))
                {
                    MessageBox.Show("Vui lòng nhập đơn giá giờ hợp lệ!\nVí dụ: 25000", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDonGiaGio.Focus();
                    return false;
                }

                if (rate < MIN_HOURLY_RATE_HCM)
                {
                    var result = MessageBox.Show(
                        $"CẢNH BÁO VI PHẠM PHÁP LUẬT!\n\n" +
                        $"Đơn giá giờ: {rate:#,##0} ₫/giờ\n" +
                        $"Phải ≥ 22.500 ₫/giờ (Vùng I - TP.HCM)\n" +
                        $"Theo Nghị định 74/2024/NĐ-CP\n\n" +
                        $"Tiếp tục lưu hợp đồng không hợp lệ?",
                        "Vi phạm đơn giá giờ tối thiểu",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Stop);

                    if (result == DialogResult.No)
                        return false;
                }

                hourlyRate = rate;
                txtDonGiaGio.Text = rate.ToString("N0"); // Format đẹp
            }

            // 4. Kiểm tra ngày hợp đồng
            if (dtpNgayBatDau.Value.Date > dtpNgayKetThuc.Value.Date)
            {
                MessageBox.Show("Ngày bắt đầu phải trước hoặc bằng ngày kết thúc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // 5. Gán lại giá trị đã validate vào DTO (ở phần lưu)
            // (Sẽ dùng trong guna2Button1_Click)

            return true;
        }
        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}