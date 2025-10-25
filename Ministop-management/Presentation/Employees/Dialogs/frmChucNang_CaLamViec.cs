using Domain.DTO;
using Services.Interfaces;
using Shared.Wrappers;
using System;
using System.Data;
using System.Linq;
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
            // === 1️⃣ Lấy dữ liệu cơ bản ===
            string maCa = txtMaCa.Text.Trim();
            string tenCa = txtTenCa.Text.Trim();

            // === 2️⃣ Validate dữ liệu ===

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

            // === 3️⃣ Chuẩn hóa giờ bắt đầu và kết thúc (loại bỏ giây) ===
            DateTime gioBatDau = new DateTime(
                DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                dtpGioBatDau.Value.Hour, dtpGioBatDau.Value.Minute, 0
            );
            DateTime gioKetThuc = new DateTime(
                DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                dtpGioKetThuc.Value.Hour, dtpGioKetThuc.Value.Minute, 0
            );

            if (gioKetThuc <= gioBatDau)
            {
                MessageBox.Show("Giờ kết thúc phải sau giờ bắt đầu!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpGioKetThuc.Focus();
                return;
            }

            // === 4️⃣ Kiểm tra trùng mã ca khi thêm mới ===
            var allShifts = _shiftService.GetAll().Data;
            if (string.IsNullOrEmpty(_shiftId))
            {
                if (allShifts.Any(x => x.ShiftId.Equals(maCa, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Mã ca đã tồn tại, vui lòng nhập mã khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaCa.Focus();
                    return;
                }
            }

            // === 5️⃣ Kiểm tra trùng thời gian với ca khác ===
            bool trungThoiGian = allShifts.Any(x =>
                !x.ShiftId.Equals(_shiftId, StringComparison.OrdinalIgnoreCase) && // bỏ qua ca hiện tại khi sửa
                x.StartTime.Hours == gioBatDau.Hour &&
                x.StartTime.Minutes == gioBatDau.Minute &&
                x.EndTime.Hours == gioKetThuc.Hour &&
                x.EndTime.Minutes == gioKetThuc.Minute
            );

            if (trungThoiGian)
            {
                MessageBox.Show("Đã tồn tại ca làm có cùng thời gian bắt đầu và kết thúc!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // === 6️⃣ Tạo DTO ===
            var shiftDto = new ShiftDto()
            {
                ShiftId = maCa,
                ShiftName = tenCa,
                StartTime = gioBatDau.TimeOfDay,
                EndTime = gioKetThuc.TimeOfDay
            };

            // === 7️⃣ Gọi service ===
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

            // === 8️⃣ Thành công ===
            DataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu ca làm việc thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
