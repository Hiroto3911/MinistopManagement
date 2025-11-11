using Domain.DTO;
using Domain.Entity;
using Services.Interfaces;
using Shared.Helpers;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_PhanCaLam : Form
    {
        private readonly IStoreService _storeService;
        private readonly IEmployeeService _employeeService;
        private readonly IShiftService _shiftService;
        private readonly IShiftAssignmentService _shiftAssignmentService;
        private readonly string _currentUserRole;
        private readonly string _currentStoreId;
        private readonly string _assignmentId;
        public event EventHandler DataChanged;

        private bool _isLoading = false;

        public frmChucNang_PhanCaLam(
            IStoreService storeService,
            IEmployeeService employeeService,
            IShiftService shiftService,
            IShiftAssignmentService shiftAssignmentService,
            string assignmentId = null,
            string currentUserRole = "Admin",
            string currentStoreId = null)
        {
            InitializeComponent();
            _storeService = storeService ?? throw new ArgumentNullException(nameof(storeService));
            _employeeService = employeeService ?? throw new ArgumentNullException(nameof(employeeService));
            _shiftService = shiftService ?? throw new ArgumentNullException(nameof(shiftService));
            _shiftAssignmentService = shiftAssignmentService ?? throw new ArgumentNullException(nameof(shiftAssignmentService));
            _assignmentId = assignmentId;
            _currentUserRole = currentUserRole;
            _currentStoreId = currentStoreId;
        }

        #region Form Load
        private void frmChucNang_PhanCaLam_Load(object sender, EventArgs e)
        {
            try
            {
                _isLoading = true;
                InitializeFormControls();

                if (!string.IsNullOrEmpty(_assignmentId))
                {
                    LoadAssignmentData();
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
            finally
            {
                _isLoading = false;
            }
        }
        #endregion

        #region Initialize Controls
        private void InitializeFormControls()
        {
            LoadStoreComboBox();
            LoadShiftComboBox();
            ConfigureRoleBasedRestrictions();
            AttachEventHandlers();
        }

        private void LoadStoreComboBox()
        {
            var result = _storeService.GetAll();
            if (result.Succeeded && result.Data != null && result.Data.Any())
            {
                var storeList = result.Data.ToList();
                cbCuaHang.DataSource = storeList;
                cbCuaHang.DisplayMember = "StoreName";
                cbCuaHang.ValueMember = "StoreId";

                if (_currentUserRole == "Quản lý cửa hàng" && !string.IsNullOrEmpty(_currentStoreId))
                {
                    // TÌM CỬA HÀNG THEO ID
                    var store = storeList.FirstOrDefault(s => s.StoreId == _currentStoreId);
                    if (store != null)
                    {
                        cbCuaHang.SelectedItem = store; // DÙNG SelectedItem ĐỂ CHẮC CHẮN
                        cbCuaHang.Enabled = false;      // DISABLE NGAY
                    }
                    else
                    {
                        // Nếu không tìm thấy (lỗi dữ liệu), vẫn disable + chọn đầu
                        cbCuaHang.SelectedIndex = 0;
                        cbCuaHang.Enabled = false;
                    }
                }
                else if (_currentUserRole == "Admin")
                {
                    cbCuaHang.SelectedIndex = 0;
                }
            }
            else
            {
                ShowErrorMessage("Không thể tải danh sách cửa hàng!");
                cbCuaHang.DataSource = null;
                btnLuu.Enabled = false;
            }
        }

        private void LoadShiftComboBox()
        {
            var result = _shiftService.GetAll();
            if (result.Succeeded && result.Data != null && result.Data.Any())
            {
                cbCaLam.DataSource = result.Data.ToList();
                cbCaLam.DisplayMember = "ShiftName";
                cbCaLam.ValueMember = "ShiftId";
            }
            else
            {
                ShowErrorMessage("Không thể tải danh sách ca làm!");
                cbCaLam.DataSource = null;
                btnLuu.Enabled = false;
            }
        }

        private void LoadEmployeeComboBox(string storeId)
        {
            cbNhanVien.DataSource = null;
            if (string.IsNullOrEmpty(storeId)) return;

            var result = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
            if (result.Succeeded && result.Data != null && result.Data.Any())
            {
                cbNhanVien.DataSource = result.Data.ToList();
                cbNhanVien.DisplayMember = "FullName";
                cbNhanVien.ValueMember = "EmployeeId";
            }
            else
            {
                cbNhanVien.DataSource = null;
                if (!_isLoading)
                    ShowWarningMessage("Không có nhân viên nào trong cửa hàng này!");
            }
        }

        private void ConfigureRoleBasedRestrictions()
        {
            if (_currentUserRole == "Nhân viên")
            {
                btnLuu.Enabled = false;
            }
        }

        private void AttachEventHandlers()
        {
            cbCuaHang.SelectedIndexChanged -= CuaHang_SelectedIndexChanged;
            cbCuaHang.SelectedIndexChanged += CuaHang_SelectedIndexChanged;
        }

        private void CuaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isLoading) return;
            if (cbCuaHang.SelectedValue == null) return;

            string storeId = cbCuaHang.SelectedValue.ToString();
            LoadEmployeeComboBox(storeId);
        }

        private void SetDefaultControlValues()
        {
            dtpNgayLamViec.Value = DateTime.Today;

            if (cbCuaHang.SelectedValue != null)
            {
                LoadEmployeeComboBox(cbCuaHang.SelectedValue.ToString());
            }
        }
        #endregion

        #region Load Assignment Data
        private void LoadAssignmentData()
        {
            if (string.IsNullOrEmpty(_assignmentId)) return;

            var result = _shiftAssignmentService.GetById(_assignmentId);
            if (!result.Succeeded || result.Data == null)
            {
                ShowWarningMessage("Không tìm thấy dữ liệu phân ca!");
                return;
            }

            var dto = result.Data;

            var empResult = _employeeService.GetEmployeeByID(dto.EmployeeId);
            string storeId = empResult.Succeeded && empResult.Data != null ? empResult.Data.StoreId : null;

            if (!string.IsNullOrEmpty(storeId))
            {
                cbCuaHang.SelectedValue = storeId;
            }

            if (cbCuaHang.SelectedValue != null)
            {
                LoadEmployeeComboBox(cbCuaHang.SelectedValue.ToString());
                if (cbNhanVien.Items.Count > 0)
                {
                    cbNhanVien.SelectedValue = dto.EmployeeId;
                }
            }

            cbCaLam.SelectedValue = dto.ShiftId;
            dtpNgayLamViec.Value = dto.WorkDate;
            txtGhiChu.Text = dto.Note ?? "";
        }
        #endregion

        #region Input Validation
        private bool ValidateInput()
        {
            if (cbCuaHang.SelectedValue == null)
            {
                ShowWarningMessage("Vui lòng chọn cửa hàng!");
                return false;
            }

            if (cbNhanVien.SelectedValue == null)
            {
                ShowWarningMessage("Vui lòng chọn nhân viên!");
                return false;
            }

            if (cbCaLam.SelectedValue == null)
            {
                ShowWarningMessage("Vui lòng chọn ca làm!");
                return false;
            }

            if (dtpNgayLamViec.Value < DateTime.Today.AddDays(-30))
            {
                ShowWarningMessage("Ngày làm việc không được quá 30 ngày trong quá khứ!");
                return false;
            }

            if (dtpNgayLamViec.Value > DateTime.Today.AddMonths(3))
            {
                ShowWarningMessage("Chỉ được phân ca tối đa 3 tháng tới!");
                return false;
            }

            var checkDuplicate = _shiftAssignmentService.CheckDuplicate(
                employeeId: cbNhanVien.SelectedValue.ToString(),
                shiftId: cbCaLam.SelectedValue.ToString(),
                workDate: dtpNgayLamViec.Value.Date,
                excludeId: _assignmentId
            );

            if (checkDuplicate.Succeeded && checkDuplicate.Data)
            {
                ShowWarningMessage("Nhân viên này đã được phân ca này vào ngày đã chọn!");
                return false;
            }

            return true;
        }
        #endregion

        #region Button Handlers
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var dto = new ShiftAssignmentDto
                {
                    Id = _assignmentId,
                    EmployeeId = cbNhanVien.SelectedValue.ToString(),
                    ShiftId = cbCaLam.SelectedValue.ToString(),
                    WorkDate = dtpNgayLamViec.Value.Date,
                    Note = txtGhiChu.Text.Trim()
                };

                var result = string.IsNullOrEmpty(_assignmentId)
                    ? _shiftAssignmentService.Create(dto)
                    : _shiftAssignmentService.Update(dto);

                if (result.Succeeded)
                {
                    ShowSuccessMessage(string.IsNullOrEmpty(_assignmentId)
                        ? "Phân ca thành công!"
                        : "Cập nhật phân ca thành công!");
                    DataChanged?.Invoke(this, EventArgs.Empty);
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