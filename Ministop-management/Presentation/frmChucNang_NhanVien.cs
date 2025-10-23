using Domain.DTO;
using Services.Interfaces;
using Services.Services;
using Shared.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_NhanVien : Form
    {
        private readonly IEmployeeService _employeeService;
        private readonly IStoreService _storeService;
        private readonly string _employeeId; // null = thêm mới
        public event EventHandler DataChanged;

        public frmChucNang_NhanVien(IEmployeeService employeeService, IStoreService storeService, string employeeId = null)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _storeService = storeService;
            _employeeId = employeeId;
        }

        #region Form Load
        private void frmChucNang_NhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                LoadComboBoxData();

                if (!string.IsNullOrEmpty(_employeeId))
                {
                    LoadEmployeeData();
                }
                else
                {
                    cbChucVu.SelectedIndex = 0;
                    cbLoaiNhanVien.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Load Data
        private void LoadComboBoxData()
        {
            // ===== Load danh sách cửa hàng =====
            var result = _storeService.GetAll();

            if (result.Succeeded && result.Data != null)
            {
                cbTenCuaHang.DataSource = result.Data.ToList(); // ⚡ Quan trọng: phải là danh sách
                cbTenCuaHang.DisplayMember = "StoreName";
                cbTenCuaHang.ValueMember = "StoreId";
            }
            else
            {
                MessageBox.Show("Không thể tải danh sách cửa hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbTenCuaHang.DataSource = null;
            }

            // ===== Load chức vụ và loại nhân viên =====
            cbChucVu.Items.Clear();
            cbChucVu.Items.AddRange(new string[] { "Nhân viên", "Quản lý", "Admin" });

            cbLoaiNhanVien.Items.Clear();
            cbLoaiNhanVien.Items.AddRange(new string[] { "Toàn thời gian", "Bán thời gian" });
        }


        private void LoadEmployeeData()
        {
            var result = _employeeService.GetEmployeeByID(_employeeId);
            if (result.Succeeded && result.Data != null)
            {
                var emp = result.Data;

                // Gán giá trị
                cbTenCuaHang.SelectedValue = emp.StoreId;
                txtMaNhanVien.Text = emp.EmployeeId;
                txtTenNhanVien.Text = emp.FullName;
                txtSoDienThoai.Text = emp.Phone;
                cbChucVu.Text = emp.Position;
                cbLoaiNhanVien.Text = emp.EmploymentType;
                dtpNgaySinh.Value = emp.BirthDate;

                if (emp.Gender) rdNam.Checked = true;
                else rdNu.Checked = true;
            }
            else
            {
                MessageBox.Show("Không tìm thấy dữ liệu nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Validate Input
        private bool ValidateInput()
        {
            if (cbTenCuaHang.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cửa hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtSoDienThoai.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!rdNam.Checked && !rdNu.Checked)
            {
                MessageBox.Show("Vui lòng chọn giới tính!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbChucVu.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn chức vụ!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (cbLoaiNhanVien.SelectedIndex < 0)
            {
                MessageBox.Show("Vui lòng chọn loại nhân viên!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text) && string.IsNullOrEmpty(_employeeId))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        #endregion

        #region Button: Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                var emp = new EmployeeDto
                {
                    EmployeeId = txtMaNhanVien.Text.Trim(),
                    StoreId = cbTenCuaHang.SelectedValue.ToString(),
                    FullName = txtTenNhanVien.Text.Trim(),
                    Gender = rdNam.Checked,
                    BirthDate = dtpNgaySinh.Value,
                    Phone = txtSoDienThoai.Text.Trim(),
                    Position = cbChucVu.Text,
                    EmploymentType = cbLoaiNhanVien.Text,
                    PasswordHash = string.IsNullOrEmpty(_employeeId)
                        ? HashPasswordSHA256.Hash(txtMatKhau.Text)
                        : txtMatKhau.Text // nếu đang sửa mà không thay mật khẩu thì giữ nguyên
                };

                var result = string.IsNullOrEmpty(_employeeId)
                    ? _employeeService.CreateEmployee(emp)
                    : _employeeService.UpdateEmployee(emp);

                if (result.Succeeded)
                {
                    MessageBox.Show(string.IsNullOrEmpty(_employeeId)
                        ? "Thêm nhân viên thành công!"
                        : "Cập nhật nhân viên thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    DataChanged?.Invoke(this, EventArgs.Empty);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lỗi: " + result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Button: Đóng
        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region Chuyển sang hợp đồng lương
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmChucNang_HopDongLuong hopDongForm = new frmChucNang_HopDongLuong();
            hopDongForm.ShowDialog();
            this.Show();
        }
        #endregion
    }
}
