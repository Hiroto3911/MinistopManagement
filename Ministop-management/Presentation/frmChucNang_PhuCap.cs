using Domain.DTO;
using Services.Interfaces;
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
    public partial class frmChucNang_PhuCap : Form
    {
        public event EventHandler DataChanged;

        private readonly IAllowanceService _allowanceService;
        private readonly string _allowanceId;

        public frmChucNang_PhuCap(IAllowanceService allowanceService, string allowanceId = null)
        {
            InitializeComponent();
            _allowanceService = allowanceService;
            _allowanceId = allowanceId;
        }

        private void frmChucNang_PhuCap_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_allowanceId))
            {
                var entity = _allowanceService.GetAllowanceByID(_allowanceId);
                if (entity.Succeeded == false || entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }

                // Gán dữ liệu lên form
                txtTenPhuCap.Text = entity.Data.AllowanceName;
                txtMucTroCap.Text = entity.Data.DefaultAmount.ToString("0.##");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // === 1️⃣ Kiểm tra tên phụ cấp ===
            string tenPhuCap = txtTenPhuCap.Text.Trim();
            if (string.IsNullOrWhiteSpace(tenPhuCap))
            {
                MessageBox.Show("Vui lòng nhập tên phụ cấp!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhuCap.Focus();
                return;
            }

            if (tenPhuCap.Length > 100)
            {
                MessageBox.Show("Tên phụ cấp không được vượt quá 100 ký tự!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenPhuCap.Focus();
                return;
            }

            // Kiểm tra trùng tên (chỉ khi thêm mới)
            if (string.IsNullOrEmpty(_allowanceId))
            {
                var existing = _allowanceService.GetAll()
                                                .Data
                                                .FirstOrDefault(a => a.AllowanceName.Equals(tenPhuCap, StringComparison.OrdinalIgnoreCase) && a.IsDeleted == false);
                if (existing != null)
                {
                    MessageBox.Show("Tên phụ cấp đã tồn tại, vui lòng nhập tên khác!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenPhuCap.Focus();
                    return;
                }
            }

            // === 2️⃣ Kiểm tra mức trợ cấp ===
            if (!decimal.TryParse(txtMucTroCap.Text.Trim(), out decimal amount))
            {
                MessageBox.Show("Vui lòng nhập mức phụ cấp hợp lệ (chỉ số)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMucTroCap.Focus();
                return;
            }

            if (amount < 0)
            {
                MessageBox.Show("Mức phụ cấp không được âm!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMucTroCap.Focus();
                return;
            }

            if (amount > 10000000) // ví dụ giới hạn 10 triệu
            {
                MessageBox.Show("Mức phụ cấp vượt quá giới hạn cho phép (10,000,000)!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMucTroCap.Focus();
                return;
            }

            // === 3️⃣ Chuẩn bị dữ liệu DTO ===
            var allowanceDto = new AllowanceDto()
            {
                AllowanceId = _allowanceId,
                AllowanceName = tenPhuCap,
                DefaultAmount = amount
            };

            // === 4️⃣ Gọi service ===
            Result<bool> result;

            if (string.IsNullOrEmpty(_allowanceId))
                result = _allowanceService.CreateAllowance(allowanceDto);
            else
                result = _allowanceService.UpdateAllowance(allowanceDto);

            if (!result.Succeeded)
            {
                MessageBox.Show($"{result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // === 5️⃣ Sau khi lưu ===
            DataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu phụ cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
