using Domain.DTO;
using Services.Interfaces;
using Services.Services;
using Shared.Wrappers;
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
    public partial class frmChucNang_CaLamViec : Form
    {
        public event EventHandler DataChanged;

        private readonly IShiftService _shiftService;
        private readonly string _shiftId;

        public frmChucNang_CaLamViec(IShiftService shiftService, string shiftId = null)
        {
            InitializeComponent();
            _shiftService = shiftService;
            _shiftId = shiftId;

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChucNang_CaLam_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_shiftId))
            {
                var result = _shiftService.GetShiftByID(_shiftId);
                if (!result.Succeeded || result.Data == null)
                {
                    MessageBox.Show($"{result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                txtMaCa.Text = result.Data.ShiftId;
                txtTenCa.Text = result.Data.ShiftName;
                dtpGioBatDau.Value = DateTime.Today.Add(result.Data.StartTime);
                dtpGioKetThuc.Value = DateTime.Today.Add(result.Data.EndTime);
                txtMaCa.Enabled = false; // không cho sửa mã ca khi update
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // === 1️⃣ Kiểm tra mã ca ===
            string maCa = txtMaCa.Text.Trim();
            //if (string.IsNullOrWhiteSpace(maCa))
            //{
            //    MessageBox.Show("Vui lòng nhập mã ca!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtMaCa.Focus();
            //    return;
            //}

            //if (maCa.Length > 50)
            //{
            //    MessageBox.Show("Mã ca không được vượt quá 50 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    txtMaCa.Focus();
            //    return;
            //}

            // === 2️⃣ Kiểm tra tên ca ===
            string tenCa = txtTenCa.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenCa))
            {
                MessageBox.Show("Vui lòng nhập tên ca!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCa.Focus();
                return;
            }

            if (tenCa.Length > 100)
            {
                MessageBox.Show("Tên ca không được vượt quá 100 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCa.Focus();
                return;
            }

            // === 3️⃣ Kiểm tra trùng mã ca khi thêm mới ===
            if (string.IsNullOrEmpty(_shiftId))
            {
                var existing = _shiftService.GetAll()
                                            .Data
                                            .FirstOrDefault(x => x.ShiftId.Equals(maCa, StringComparison.OrdinalIgnoreCase));
                if (existing != null)
                {
                    MessageBox.Show("Mã ca đã tồn tại, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaCa.Focus();
                    return;
                }
            }

            // === 4️⃣ Kiểm tra giờ bắt đầu và kết thúc ===
            DateTime gioBatDau = dtpGioBatDau.Value;
            DateTime gioKetThuc = dtpGioKetThuc.Value;

            if (gioKetThuc <= gioBatDau)
            {
                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpGioKetThuc.Focus();
                return;
            }

            // === 5️⃣ Tạo DTO ===
            var shiftDto = new ShiftDto()
            {
                ShiftId = maCa,
                ShiftName = tenCa,
                StartTime = gioBatDau.TimeOfDay,
                EndTime = gioKetThuc.TimeOfDay
            };

            // === 6️⃣ Gọi service ===
            Result<bool> result;

            if (string.IsNullOrEmpty(_shiftId))
                result = _shiftService.CreateShift(shiftDto);
            else
                result = _shiftService.UpdateShift(shiftDto);

            if (!result.Succeeded)
            {
                MessageBox.Show($"{result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // === 7️⃣ Hoàn tất ===
            DataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

    }
}
