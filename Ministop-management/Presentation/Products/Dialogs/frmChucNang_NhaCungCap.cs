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
using System.Text.RegularExpressions;

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
            // ✅ Khởi tạo combobox loại SDT
            cboLoaiSDT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiSDT.Items.Clear();
            cboLoaiSDT.Items.Add("Việt Nam (+84)");
            cboLoaiSDT.Items.Add("Mỹ (+1)");
            cboLoaiSDT.Items.Add("Nhật Bản (+81)");
            cboLoaiSDT.Items.Add("Hàn Quốc (+82)");
            cboLoaiSDT.Items.Add("Trung Quốc (+86)");
            cboLoaiSDT.Items.Add("Khác (nhập thủ công)");
            cboLoaiSDT.SelectedIndex = 0; // Mặc định Việt Nam

            // ✅ Nếu là Edit -> load dữ liệu sẵn có
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

                // ✅ Tự chọn combobox tương ứng theo mã quốc gia
                if (!string.IsNullOrEmpty(entity.Data.Phone))
                {
                    if (entity.Data.Phone.StartsWith("+84")) cboLoaiSDT.SelectedItem = "Việt Nam (+84)";
                    else if (entity.Data.Phone.StartsWith("+1")) cboLoaiSDT.SelectedItem = "Mỹ (+1)";
                    else if (entity.Data.Phone.StartsWith("+81")) cboLoaiSDT.SelectedItem = "Nhật Bản (+81)";
                    else if (entity.Data.Phone.StartsWith("+82")) cboLoaiSDT.SelectedItem = "Hàn Quốc (+82)";
                    else if (entity.Data.Phone.StartsWith("+86")) cboLoaiSDT.SelectedItem = "Trung Quốc (+86)";
                    else cboLoaiSDT.SelectedItem = "Khác (nhập thủ công)";
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenNCC = txtTenNCC.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string quocGia = cboLoaiSDT.SelectedItem?.ToString() ?? "Việt Nam (+84)";

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

            // 3️⃣ Kiểm tra ký tự đặc biệt
            if (!Regex.IsMatch(tenNCC, @"^[a-zA-Z0-9\s\-_À-ỹ]+$"))
            {
                MessageBox.Show("Tên nhà cung cấp chỉ được chứa chữ, số, khoảng trắng, gạch nối (-) hoặc gạch dưới (_).", "Ký tự không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenNCC.Focus();
                return;
            }

            // 4️⃣ Làm sạch SDT (bỏ ký tự lạ)
            sdt = Regex.Replace(sdt, @"[^\d\+]", "");

            // ✅ Chuẩn hóa SDT theo quốc gia
            string prefix = "";
            if (quocGia.Contains("+"))
            {
                int idx = quocGia.IndexOf('+');
                if (idx >= 0)
                    prefix = quocGia.Substring(idx); // ví dụ "+84"
            }

            // ---- Xử lý từng quốc gia ----
            if (quocGia.Contains("Việt Nam"))
            {
                if (sdt.StartsWith("0"))
                    sdt = "+84" + sdt.Substring(1);
                else if (sdt.StartsWith("84") && !sdt.StartsWith("+84"))
                    sdt = "+" + sdt;
                else if (!sdt.StartsWith("+"))
                    sdt = "+84" + sdt;

                if (!Regex.IsMatch(sdt, @"^\+84\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại Việt Nam không hợp lệ. Nhập dạng 0xxxxxxxxx hoặc +84xxxxxxxxx.", "Lỗi dữ liệu");
                    txtSDT.Focus();
                    return;
                }
            }
            else if (quocGia.Contains("Mỹ"))
            {
                if (sdt.StartsWith("0"))
                    sdt = "+1" + sdt.Substring(1);
                else if (sdt.StartsWith("1") && !sdt.StartsWith("+1"))
                    sdt = "+" + sdt;
                else if (!sdt.StartsWith("+1"))
                    sdt = "+1" + sdt;

                if (!Regex.IsMatch(sdt, @"^\+1\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại Mỹ không hợp lệ. Vui lòng nhập 10 chữ số sau mã vùng (VD: 4155552671).", "Lỗi dữ liệu");
                    txtSDT.Focus();
                    return;
                }
            }
            else if (quocGia.Contains("Nhật Bản"))
            {
                if (sdt.StartsWith("0"))
                    sdt = "+81" + sdt.Substring(1);
                else if (!sdt.StartsWith("+81"))
                    sdt = "+81" + sdt;

                if (!Regex.IsMatch(sdt, @"^\+81\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại Nhật Bản không hợp lệ.", "Lỗi dữ liệu");
                    txtSDT.Focus();
                    return;
                }
            }
            else if (quocGia.Contains("Hàn Quốc"))
            {
                if (sdt.StartsWith("0"))
                    sdt = "+82" + sdt.Substring(1);
                else if (!sdt.StartsWith("+82"))
                    sdt = "+82" + sdt;

                if (!Regex.IsMatch(sdt, @"^\+82\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại Hàn Quốc không hợp lệ.", "Lỗi dữ liệu");
                    txtSDT.Focus();
                    return;
                }
            }
            else if (quocGia.Contains("Trung Quốc"))
            {
                if (sdt.StartsWith("0"))
                    sdt = "+86" + sdt.Substring(1);
                else if (!sdt.StartsWith("+86"))
                    sdt = "+86" + sdt;

                if (!Regex.IsMatch(sdt, @"^\+86\d{10,11}$"))
                {
                    MessageBox.Show("Số điện thoại Trung Quốc không hợp lệ.", "Lỗi dữ liệu");
                    txtSDT.Focus();
                    return;
                }
            }
            else // Khác (nhập thủ công)
            {
                if (!Regex.IsMatch(sdt, @"^\+?\d{6,15}$"))
                {
                    MessageBox.Show("Số điện thoại quốc tế không hợp lệ. Nhập từ 6–15 chữ số, có thể bắt đầu bằng +.", "Lỗi dữ liệu");
                    txtSDT.Focus();
                    return;
                }
            }

            // ✅ Tạo DTO và gọi service
            var supplier = new SupplierDto()
            {
                SupplierId = _SupplierId,
                SupplierName = tenNCC,
                Phone = sdt,
                Address = diaChi
            };

            Result<bool> result;
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
