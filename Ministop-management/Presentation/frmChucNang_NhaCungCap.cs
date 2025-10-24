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
    public partial class frmChucNang_NhaCungCap : Form
    {
        public event EventHandler DataChanged;
        private readonly ISupplierService _SupplierService;
        private string _SupplierId;
        public frmChucNang_NhaCungCap(ISupplierService supplierService, string supplierId = null)
        {
            InitializeComponent();
            _SupplierService = supplierService;
            _SupplierId = supplierId;
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void frmChucNang_NhaCungCap_Load(object sender, EventArgs e)
        {
            //cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!string.IsNullOrEmpty(_SupplierId))
            {
                var entity = _SupplierService.GetSupplierByID(_SupplierId);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtMaNCC.Text = entity.Data.SupplierId;
                txtTenNCC.Text = entity.Data.SupplierName;
                txtSDT.Text = entity.Data.Phone;
                txtDiaChi.Text = entity.Data.Address;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            //var supplier = new SupplierDto() { SupplierId = txtMaNCC.Text, SupplierName = txtTenNCC.Text,Phone = txtSDT.Text,Address = txtDiaChi.Text };
            //Result<bool> result;
            //if (string.IsNullOrEmpty(txtMaNCC.Text))
            //{
            //    result = _SupplierService.CreateSupplier(supplier);
            //    DataChanged?.Invoke(this, EventArgs.Empty);
            //}
            //else
            //{
            //    supplier.SupplierId = _SupplierId;
            //    result = _SupplierService.UpdateSupplier(supplier);
            //    DataChanged?.Invoke(this, EventArgs.Empty);
            //}
            //if (result.Succeeded == false)
            //{
            //    MessageBox.Show($"{result.Message}", "Lỗi");
            //    return;
            //}

            //MessageBox.Show($"Luu thanh cong", "Thong bao");
            //this.Close();
            string tenNCC = txtTenNCC.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            // 1️⃣ Kiểm tra trống
        
            if (string.IsNullOrWhiteSpace(tenNCC))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 2️⃣ Giới hạn độ dài
            if (tenNCC.Length > 200)
            {
                MessageBox.Show("Tên nhà cung cấp không được vượt quá 200 ký tự.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }
            if (diaChi.Length > 200)
            {
                MessageBox.Show("Địa chỉ không được vượt quá 200 ký tự.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDiaChi.Focus();
                return;
            }

            // 3️⃣ Kiểm tra ký tự đặc biệt (mã và tên)
            if (!System.Text.RegularExpressions.Regex.IsMatch(tenNCC, @"^[a-zA-Z0-9\s\-_À-ỹ]+$"))
            {
                MessageBox.Show("Tên nhà cung cấp chỉ được chứa chữ, số, khoảng trắng, gạch nối (-) hoặc gạch dưới (_).", "Ký tự không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }

            // 4️⃣ Kiểm tra số điện thoại
            if (!System.Text.RegularExpressions.Regex.IsMatch(sdt, @"^(0|\+84)(\d{9})$"))
            {
                MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập dạng 0xxxxxxxxx hoặc +84xxxxxxxxx.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }

            // 5️⃣ Tạo đối tượng DTO
            var supplier = new SupplierDto()
            {
                SupplierId = _SupplierId,
                SupplierName = tenNCC,
                Phone = sdt,
                Address = diaChi
            };

            Result<bool> result;

            // 6️⃣ Thêm mới hoặc cập nhật
            if (string.IsNullOrEmpty(_SupplierId))
            {
                result = _SupplierService.CreateSupplier(supplier);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                supplier.SupplierId = _SupplierId;
                result = _SupplierService.UpdateSupplier(supplier);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }

            // 7️⃣ Kiểm tra kết quả
            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
