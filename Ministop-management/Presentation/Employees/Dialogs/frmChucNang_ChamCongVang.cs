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
    public partial class frmChucNang_ChamCongVang : Form
    {
        private readonly IAbsenceService _absenceService;
        private readonly IEmployeeService _employeeService;
        private readonly IShiftService _shiftService;
        private readonly IShiftAssignmentService _shiftAssignmentService;
        private readonly IUnityContainer _container;
        private readonly string _absenceId;
        private readonly string _currentStoreId;

        public event EventHandler DataChanged;

        public frmChucNang_ChamCongVang(
            IAbsenceService absenceService,
            IEmployeeService employeeService,
            IShiftService shiftService,
            IShiftAssignmentService shiftAssignmentService,
            IUnityContainer container,
            string absenceId = null,
            string currentStoreId = null)
        {
            InitializeComponent();
            _absenceService = absenceService;
            _employeeService = employeeService;
            _shiftService = shiftService;
            _shiftAssignmentService = shiftAssignmentService;
            _container = container;
            _absenceId = absenceId;
            _currentStoreId = currentStoreId;
        }

        private void frmChucNangChamCongVang_Load(object sender, EventArgs e)
        {
            LoadComboBoxes();
            if (!string.IsNullOrEmpty(_absenceId))
                LoadAbsenceData();
        }

        private void LoadComboBoxes()
        {
            // Load nhân viên theo cửa hàng
            var empResult = _employeeService.GetEmployeeByStore(_currentStoreId, 1, 1000);
            if (empResult.Succeeded && empResult.Data != null)
            {
                cboMaNhanVien.DataSource = empResult.Data.ToList();
                cboMaNhanVien.DisplayMember = "EmployeeId";
                cboMaNhanVien.ValueMember = "EmployeeId";
                cboTenNhanVien.DataSource = empResult.Data.ToList();
                cboTenNhanVien.DisplayMember = "FullName";
                cboTenNhanVien.ValueMember = "EmployeeId";
            }

            // Load ca làm
            var shiftResult = _shiftService.GetAll();
            if (shiftResult.Succeeded && shiftResult.Data != null)
            {
                cboCaLam.DataSource = shiftResult.Data.ToList();
                cboCaLam.DisplayMember = "ShiftName";
                cboCaLam.ValueMember = "ShiftId";
            }

            // Đồng bộ combo
            cboMaNhanVien.SelectedIndexChanged += (s, ev) =>
            {
                if (cboMaNhanVien.SelectedValue != null)
                    cboTenNhanVien.SelectedValue = cboMaNhanVien.SelectedValue;
            };
            cboTenNhanVien.SelectedIndexChanged += (s, ev) =>
            {
                if (cboTenNhanVien.SelectedValue != null)
                    cboMaNhanVien.SelectedValue = cboTenNhanVien.SelectedValue;
            };
        }

        private void LoadAbsenceData()
        {
            var result = _absenceService.GetAbsenceByID(_absenceId);
            if (result.Succeeded && result.Data != null)
            {
                var data = result.Data;
                cboMaNhanVien.SelectedValue = data.EmployeeId;
                cboCaLam.SelectedValue = data.ShiftId;
                dtpNgay.Value = data.WorkDate;
                rdPhepCo.Checked = data.IsLeaveOfAbsence;
                rdPhepKhong.Checked = !data.IsLeaveOfAbsence;
                txtLyDo.Text = data.Reason;
                chkCoLuong.Checked = data.IsPaid;
            }
        }

        private bool ValidateInput()
        {
            if (cboMaNhanVien.SelectedValue == null)
            {
                MessageBox.Show("Chọn mã nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cboCaLam.SelectedValue == null)
            {
                MessageBox.Show("Chọn ca làm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (dtpNgay.Value > DateTime.Today)
            {
                MessageBox.Show("Ngày không được lớn hơn hôm nay!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Kiểm tra phân công tồn tại
            var assignmentExists = _shiftAssignmentService.GetAll().Data.Any(x =>
                x.EmployeeId == cboMaNhanVien.SelectedValue.ToString() &&
                x.ShiftId == cboCaLam.SelectedValue.ToString() &&
                x.WorkDate.Date == dtpNgay.Value.Date);

            if (!assignmentExists)
            {
                MessageBox.Show("Nhân viên chưa được phân công ca này vào ngày này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            var dto = new AbsenceDto
            {
                AbsenceId = _absenceId,
                EmployeeId = cboMaNhanVien.SelectedValue.ToString(),
                ShiftId = cboCaLam.SelectedValue.ToString(),
                WorkDate = dtpNgay.Value.Date,
                IsLeaveOfAbsence = rdPhepCo.Checked,
                Reason = txtLyDo.Text.Trim(),
                IsPaid = chkCoLuong.Checked
            };

            var result = string.IsNullOrEmpty(_absenceId)
                ? _absenceService.CreateAbsence(dto)
                : _absenceService.UpdateAbsence(dto);

            if (result.Succeeded)
            {
                MessageBox.Show(string.IsNullOrEmpty(_absenceId) ? "Thêm thành công!" : "Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
                Close();
            }
            else
            {
                MessageBox.Show($"Lỗi: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}