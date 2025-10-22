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
            // Kiểm tra dữ liệu hợp lệ
            if (string.IsNullOrWhiteSpace(txtTenPhuCap.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phụ cấp!", "Thông báo");
                return;
            }

            if (!decimal.TryParse(txtMucTroCap.Text, out decimal amount) || amount < 0)
            {
                MessageBox.Show("Mức phụ cấp không hợp lệ!", "Thông báo");
                return;
            }

            var allowanceDto = new AllowanceDto()
            {
                AllowanceName = txtTenPhuCap.Text.Trim(),
                DefaultAmount = amount
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_allowanceId))
            {
                // Thêm mới
                result = _allowanceService.CreateAllowance(allowanceDto);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                // Cập nhật
                allowanceDto.AllowanceId = _allowanceId;
                result = _allowanceService.UpdateAllowance(allowanceDto);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (result.Succeeded == false)
            {
                MessageBox.Show($"{result.Message}", "Lỗi");
                return;
            }

            MessageBox.Show("Lưu thành công!", "Thông báo");
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
