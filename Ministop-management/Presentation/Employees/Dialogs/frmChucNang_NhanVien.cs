using Domain.DTO;
using Services.Interfaces;
using Shared.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmChucNang_NhanVien : Form
    {
        private readonly IEmployeeService _employeeService;
        private readonly IStoreService _storeService;
        private readonly IUnityContainer _container;
        private readonly string _employeeId;
        private readonly string _currentUserRole;
        private readonly string _currentStoreId;
        public event EventHandler DataChanged;
        public frmChucNang_NhanVien(
            IEmployeeService employeeService,
            IStoreService storeService,
            IUnityContainer container,
            string employeeId = null,
            string currentUserRole = "Admin",
            string currentStoreId = null)
        {
            InitializeComponent();
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _storeService = storeService ?? throw new ArgumentNullException(nameof(storeService));
            _container = container;
            _employeeId = employeeId;
            _currentUserRole = currentUserRole;
            _currentStoreId = currentStoreId;
        }
        #region Form Load
        private void frmChucNang_NhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeFormControls();
                if (!string.IsNullOrEmpty(_employeeId))
                {
                    LoadEmployeeData();
                }
                else
                {
                    SetDefaultControlValues();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
        }
        #endregion
        #region Initialize Controls
        private void InitializeFormControls()
        {
            LoadStoreComboBox();
            InitializeRoleComboBox();
            InitializeEmploymentTypeComboBox();
            ConfigureRoleBasedRestrictions();
            AttachEventHandlers();
        }
        private void LoadStoreComboBox()
        {
            var result = _storeService.GetAll();
            if (result.Succeeded && result.Data != null && result.Data.Any())
            {
                cbTenCuaHang.DataSource = result.Data.ToList();
                cbTenCuaHang.DisplayMember = "StoreName";
                cbTenCuaHang.ValueMember = "StoreId";
            }
            else
            {
                ShowErrorMessage("Không thể tải danh sách cửa hàng!");
                cbTenCuaHang.DataSource = null;
                btnLuu.Enabled = false;
            }
        }
        private void InitializeRoleComboBox()
        {
            // Admin không được thêm Admin khác
            if (_currentUserRole == "Admin")
            {
                cbChucVu.Items.AddRange(new[] { "Nhân viên", "Quản lý cửa hàng" });
            }
            else
            {
                cbChucVu.Items.AddRange(new[] { "Nhân viên", "Quản lý cửa hàng", "Admin" });
            }
        }
        private void InitializeEmploymentTypeComboBox()
        {
            cbLoaiNhanVien.Items.AddRange(new[] { "Fulltime", "Parttime" });
        }
        private void ConfigureRoleBasedRestrictions()
        {
            if (_currentUserRole == "Quản lý cửa hàng")
            {
                cbTenCuaHang.SelectedValue = _currentStoreId;
                cbTenCuaHang.Enabled = false;
                cbChucVu.Items.Remove("Admin");
                cbChucVu.Items.Remove("Quản lý cửa hàng");
            }
            else if (_currentUserRole == "Nhân viên")
            {
                cbTenCuaHang.SelectedValue = _currentStoreId;
                cbTenCuaHang.Enabled = false;
                cbChucVu.Items.Remove("Admin");
                cbChucVu.Items.Remove("Quản lý cửa hàng");
                btnLuu.Enabled = false;
            }
        }
        private void AttachEventHandlers()
        {
            cbChucVu.SelectedIndexChanged += RoleChanged_DisableEmploymentType;
            txtSoDienThoai.KeyPress += ValidatePhoneInput;
        }
        private void SetDefaultControlValues()
        {
            if (cbChucVu.Items.Count > 0)
                cbChucVu.SelectedIndex = 0;
            if (cbLoaiNhanVien.Items.Count > 0)
                cbLoaiNhanVien.SelectedIndex = 0;
        }
        #endregion
        #region Load Employee Data
        private void LoadEmployeeData()
        {
            if (string.IsNullOrEmpty(_employeeId))
            {
                ShowWarningMessage("Mã nhân viên không hợp lệ!");
                return;
            }
            var result = _employeeService.GetEmployeeByID(_employeeId);
            if (result.Succeeded && result.Data != null)
            {
                PopulateEmployeeData(result.Data);
            }
            else
            {
                ShowWarningMessage("Không tìm thấy dữ liệu nhân viên!");
            }
        }
        private void PopulateEmployeeData(EmployeeDto employee)
        {
            cbTenCuaHang.SelectedValue = employee.StoreId;
            txtMaNhanVien.Text = employee.EmployeeId;
            txtTenNhanVien.Text = employee.FullName;
            txtSoDienThoai.Text = employee.Phone;
            cbChucVu.Text = employee.Position;
            cbLoaiNhanVien.Text = employee.EmploymentType;
            dtpNgaySinh.Value = employee.BirthDate;
            txtMatKhau.PlaceholderText = "*";
            rdNam.Checked = employee.Gender;
            rdNu.Checked = !employee.Gender;
            RoleChanged_DisableEmploymentType(null, null);
        }
        #endregion
        #region Input Validation
        private bool ValidateInput()
        {
            if (cbTenCuaHang.SelectedValue == null)
            {
                ShowWarningMessage("Vui lòng chọn cửa hàng!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                ShowWarningMessage("Vui lòng nhập tên nhân viên!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text) || txtSoDienThoai.Text.Length != 10 || !txtSoDienThoai.Text.StartsWith("0"))
            {
                ShowWarningMessage("Số điện thoại phải đúng 10 chữ số và bắt đầu bằng 0!");
                return false;
            }
            if (!rdNam.Checked && !rdNu.Checked)
            {
                ShowWarningMessage("Vui lòng chọn giới tính!");
                return false;
            }
            // Kiểm tra độ tuổi theo quy định Việt Nam (nam: 62, nữ: 60 - cập nhật 2025)
            int retirementAge = rdNam.Checked ? 62 : 60;
            int age = DateTime.Today.Year - dtpNgaySinh.Value.Year;
            if (dtpNgaySinh.Value > DateTime.Today.AddYears(-age)) age--;
            if (age < 18)
            {
                ShowWarningMessage("Nhân viên phải đủ 18 tuổi để đi làm!");
                return false;
            }
            if (age > retirementAge)
            {
                ShowWarningMessage($"Nhân viên đã vượt quá tuổi nghỉ hưu ({retirementAge} tuổi đối với {(rdNam.Checked ? "nam" : "nữ")})!");
                return false;
            }
            if (cbChucVu.SelectedIndex < 0)
            {
                ShowWarningMessage("Vui lòng chọn chức vụ!");
                return false;
            }
            string selectedPosition = cbChucVu.Text;
            int userLevel = GetRoleLevel(_currentUserRole);
            int selectedLevel = GetRoleLevel(selectedPosition);
            if (selectedLevel >= userLevel)
            {
                ShowWarningMessage("Bạn không thể tạo nhân viên có chức vụ bằng hoặc cao hơn bạn!");
                return false;
            }
            if (_currentUserRole == "Quản lý cửa hàng" && cbTenCuaHang.SelectedValue?.ToString() != _currentStoreId)
            {
                ShowWarningMessage("Bạn không có quyền sửa nhân viên của cửa hàng khác!");
                return false;
            }
            if (cbLoaiNhanVien.SelectedIndex < 0)
            {
                ShowWarningMessage("Vui lòng chọn loại nhân viên!");
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text) && string.IsNullOrEmpty(_employeeId))
            {
                ShowWarningMessage("Vui lòng nhập mật khẩu!");
                return false;
            }
            return true;
        }
        private int GetRoleLevel(string role)
        {
            switch (role)
            {
                case "Admin": return 3;
                case "Quản lý cửa hàng": return 2;
                case "Nhân viên": return 1;
                default: return 0;
            }
        }
        #endregion
        #region Event Handlers
        private void RoleChanged_DisableEmploymentType(object sender, EventArgs e)
        {
            if (cbChucVu.SelectedItem == null) return;
            string role = cbChucVu.SelectedItem.ToString();
            bool isRestrictedRole = role == "Quản lý cửa hàng" || role == "Admin";
            cbLoaiNhanVien.SelectedIndex = isRestrictedRole ? 0 : cbLoaiNhanVien.SelectedIndex;
            cbLoaiNhanVien.Enabled = !isRestrictedRole;
        }
        private void ValidatePhoneInput(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
            if (txtSoDienThoai.Text.Length >= 10 && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
        #endregion
        #region Button Handlers
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            try
            {
                var employee = CreateEmployeeDto();
                var result = string.IsNullOrEmpty(_employeeId)
                    ? _employeeService.CreateEmployee(employee)
                    : _employeeService.UpdateEmployee(employee);
                if (result.Succeeded)
                {
                    ShowSuccessMessage(string.IsNullOrEmpty(_employeeId)
                        ? "Thêm nhân viên thành công!"
                        : "Cập nhật nhân viên thành công!");
                    DataChanged?.Invoke(this, EventArgs.Empty);
                    if (string.IsNullOrEmpty(_employeeId))
                    {
                        // Chuyển sang tạo hợp đồng lương
                        var frmHopDong = _container.Resolve<frmChucNang_HopDongLuong>(
                            new ParameterOverride("employeeId", employee.EmployeeId));
                        frmHopDong.DataChanged += (s, ev) => DataChanged?.Invoke(this, EventArgs.Empty);
                        frmHopDong.ShowDialog();
                    }
                    Close();
                }
                else
                {
                    ShowErrorMessage($"Lỗi: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Đã xảy ra lỗi: {ex.Message}");
            }
        }
        private EmployeeDto CreateEmployeeDto()
        {
            var employee = new EmployeeDto
            {
                EmployeeId = txtMaNhanVien.Text.Trim(),
                StoreId = cbTenCuaHang.SelectedValue.ToString(),
                FullName = txtTenNhanVien.Text.Trim(),
                Gender = rdNam.Checked,
                BirthDate = dtpNgaySinh.Value,
                Phone = txtSoDienThoai.Text.Trim(),
                Position = cbChucVu.Text,
                EmploymentType = cbLoaiNhanVien.Text
            };
            if (!string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                employee.PasswordHash = HashPasswordSHA256.Hash(txtMatKhau.Text);
            }
            else if (!string.IsNullOrEmpty(_employeeId))
            {
                var existingEmployee = _employeeService.GetEmployeeByID(_employeeId);
                if (existingEmployee.Succeeded && existingEmployee.Data != null)
                {
                    employee.PasswordHash = existingEmployee.Data.PasswordHash;
                }
            }
            return employee;
        }
        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
        #endregion
        #region Message Helpers
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ShowWarningMessage(string message)
        {
            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion
    }
}