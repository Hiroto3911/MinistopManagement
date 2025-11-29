using Domain.DTO;
using Domain.Entity;
using Services.Interfaces;
using Shared.Helpers;
using System;
using System.Collections.Generic;
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

        // Nhận tuần hiện tại từ form chính
        public DateTime WeekStartDate { get; set; }  // Thứ 2
        public DateTime WeekEndDate { get; set; }    // Chủ nhật

        public event EventHandler DataChanged;

        public frmChucNang_PhanCaLam(
            IStoreService storeService,
            IEmployeeService employeeService,
            IShiftService shiftService,
            IShiftAssignmentService shiftAssignmentService,
            DateTime weekStart,
            DateTime weekEnd,
            string currentUserRole = "Admin",
            string currentStoreId = null)
        {
            InitializeComponent();

            _storeService = storeService;
            _employeeService = employeeService;
            _shiftService = shiftService;
            _shiftAssignmentService = shiftAssignmentService;
            _currentUserRole = currentUserRole;
            _currentStoreId = currentStoreId;

            WeekStartDate = weekStart.Date;
            WeekEndDate = weekEnd.Date;
        }

        private void frmChucNang_PhanCaLam_Load(object sender, EventArgs e)
        {
            InitializeFormControls();
            UpdateWeekDayLabels(); // Cập nhật ngày thực tế lên 7 checkbox
        }

        private void InitializeFormControls()
        {
            LoadStoreComboBox();
            LoadShiftComboBox();
            ConfigureRoleBasedRestrictions();

            // Gắn sự kiện đổi cửa hàng
            cbCuaHang.SelectedIndexChanged += cbCuaHang_SelectedIndexChanged;
        }

        private void LoadStoreComboBox()
        {
            var result = _storeService.GetAll();
            if (result.Succeeded && result.Data?.Any() == true)
            {
                var list = result.Data.ToList();
                cbCuaHang.DataSource = list;
                cbCuaHang.DisplayMember = "StoreName";
                cbCuaHang.ValueMember = "StoreId";

                if (_currentUserRole == "Quản lý cửa hàng" && !string.IsNullOrEmpty(_currentStoreId))
                {
                    var store = list.FirstOrDefault(s => s.StoreId == _currentStoreId);
                    if (store != null)
                    {
                        cbCuaHang.SelectedItem = store;
                        cbCuaHang.Enabled = false;
                    }
                }
                else cbCuaHang.SelectedIndex = 0;
            }
        }

        private void LoadShiftComboBox()
        {
            var result = _shiftService.GetAll();
            if (result.Succeeded && result.Data?.Any() == true)
            {
                cbCaLam.DataSource = result.Data.ToList();
                cbCaLam.DisplayMember = "ShiftName";
                cbCaLam.ValueMember = "ShiftId";
            }
        }

        private void ConfigureRoleBasedRestrictions()
        {
            if (_currentUserRole == "Nhân viên")
                btnLuu.Enabled = false;
        }

        // CẬP NHẬT NGÀY THỰC TẾ LÊN 7 CHECKBOX (từ Designer)
        private void UpdateWeekDayLabels()
        {
            var checkboxes = new[]
            {
                chkThu2, chkThu3, chkThu4, chkThu5, chkThu6, chkThu7, chkChuNhat
            };

            for (int i = 0; i < 7; i++)
            {
                DateTime date = WeekStartDate.AddDays(i);
                if (checkboxes[i] != null)
                {
                    checkboxes[i].Text = checkboxes[i].Text.Split('\n').First() + $"\n{date:dd/MM}";
                    checkboxes[i].Tag = date; // Lưu ngày vào Tag để dùng khi lưu
                    checkboxes[i].Checked = true; // Tích sẵn
                }
            }
        }

        private void cbCuaHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCuaHang.SelectedValue == null) return;
            LoadEmployeeComboBox(cbCuaHang.SelectedValue.ToString());
        }

        private void LoadEmployeeComboBox(string storeId)
        {
            cbNhanVien.DataSource = null;
            var result = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
            if (result.Succeeded && result.Data?.Any() == true)
            {
                var parttimeOnly = result.Data.Where(x => x.EmploymentType == "Parttime").ToList();
                if (!parttimeOnly.Any())
                {
                    cbNhanVien.Items.Add("(Không có nhân viên Part-time)");
                    cbNhanVien.Enabled = false;
                    return;
                }
                cbNhanVien.DataSource = parttimeOnly;
                cbNhanVien.DisplayMember = "FullName";
                cbNhanVien.ValueMember = "EmployeeId";
                cbNhanVien.Enabled = true;
            }
            else
            {
                cbNhanVien.Items.Add("(Không có nhân viên)");
                cbNhanVien.Enabled = false;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (cbCuaHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cửa hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbNhanVien.SelectedValue == null || !cbNhanVien.Enabled)
            {
                MessageBox.Show("Vui lòng chọn nhân viên Part-time!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbCaLam.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn ca làm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var employeeId = cbNhanVien.SelectedValue.ToString();
            var shiftId = cbCaLam.SelectedValue.ToString();
            var note = txtGhiChu.Text.Trim();

            var assignments = new List<ShiftAssignmentDto>();
            int successCount = 0;

            var dayCheckboxes = new[] { chkThu2, chkThu3, chkThu4, chkThu5, chkThu6, chkThu7, chkChuNhat };

            foreach (var chk in dayCheckboxes)
            {
                if (chk == null || !chk.Checked) continue;

                var workDate = (DateTime)chk.Tag;

                var dup = _shiftAssignmentService.CheckDuplicate(employeeId, shiftId, workDate);
                if (dup.Succeeded && dup.Data) continue; // Bỏ qua ngày trùng

                assignments.Add(new ShiftAssignmentDto
                {
                    EmployeeId = employeeId,
                    ShiftId = shiftId,
                    WorkDate = workDate,
                    Note = note
                });
                successCount++;
            }

            if (assignments.Count == 0)
            {
                MessageBox.Show("Không có ngày nào được lưu (toàn bộ ngày đã có ca hoặc bị bỏ chọn).", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _shiftAssignmentService.CreateBatch(assignments);
            if (result.Succeeded)
            {
                MessageBox.Show($"Đã phân công thành công {successCount} ngày!", "Thành công",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataChanged?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            else
            {
                MessageBox.Show("Lỗi: " + result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e) => this.Close();
    }
}