using Domain.DTO;
using Services.Interfaces;
using Shared.Wrappers;
using System;
using System.Text.RegularExpressions;
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
            KhoiTaoComboBoxLoaiSDT();

            if (!string.IsNullOrEmpty(_SupplierId))
                LoadThongTinNhaCungCap(_SupplierId);
        }

        private void KhoiTaoComboBoxLoaiSDT()
        {
            cboLoaiSDT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiSDT.Items.Clear();
            cboLoaiSDT.Items.Add("Việt Nam (+84)");
            cboLoaiSDT.Items.Add("Mỹ (+1)");
            cboLoaiSDT.Items.Add("Nhật Bản (+81)");
            cboLoaiSDT.Items.Add("Hàn Quốc (+82)");
            cboLoaiSDT.Items.Add("Trung Quốc (+86)");
            cboLoaiSDT.Items.Add("Khác (nhập thủ công)");
            cboLoaiSDT.SelectedIndex = 0;
        }

        private void LoadThongTinNhaCungCap(string supplierId)
        {
            var entity = _SupplierService.GetSupplierByID(supplierId);
            if (!entity.Succeeded || entity.Data == null)
            {
                MessageBox.Show($"{entity.Message}", "Lỗi");
                return;
            }

            txtMaNCC.Text = entity.Data.SupplierId;
            txtTenNCC.Text = entity.Data.SupplierName;
            txtSDT.Text = entity.Data.Phone;
            txtDiaChi.Text = entity.Data.Address;

            string phone = entity.Data.Phone ?? "";
            if (phone.StartsWith("+84")) cboLoaiSDT.SelectedItem = "Việt Nam (+84)";
            else if (phone.StartsWith("+1")) cboLoaiSDT.SelectedItem = "Mỹ (+1)";
            else if (phone.StartsWith("+81")) cboLoaiSDT.SelectedItem = "Nhật Bản (+81)";
            else if (phone.StartsWith("+82")) cboLoaiSDT.SelectedItem = "Hàn Quốc (+82)";
            else if (phone.StartsWith("+86")) cboLoaiSDT.SelectedItem = "Trung Quốc (+86)";
            else cboLoaiSDT.SelectedItem = "Khác (nhập thủ công)";
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenNCC = txtTenNCC.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string quocGia = cboLoaiSDT.SelectedItem?.ToString() ?? "Việt Nam (+84)";

            if (!KiemTraDuLieuNhap(tenNCC, sdt, diaChi))
                return;

            string sdtChuanHoa = ChuanHoaSoDienThoai(sdt, quocGia);
            if (sdtChuanHoa == null)
                return;

            var supplier = new SupplierDto
            {
                SupplierId = _SupplierId,
                SupplierName = tenNCC,
                Phone = sdtChuanHoa,
                Address = diaChi
            };

            LuuNhaCungCap(supplier);
        }

        private bool KiemTraDuLieuNhap(string tenNCC, string sdt, string diaChi)
        {
            if (string.IsNullOrWhiteSpace(tenNCC))
            {
                MessageBox.Show("Vui lòng nhập tên nhà cung cấp.", "Thiếu thông tin");
                txtTenNCC.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(sdt))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại.", "Thiếu thông tin");
                txtSDT.Focus();
                return false;
            }

            if (tenNCC.Length > 200)
            {
                MessageBox.Show("Tên nhà cung cấp không được vượt quá 200 ký tự.", "Lỗi dữ liệu");
                txtTenNCC.Focus();
                return false;
            }
            if (diaChi.Length > 200)
            {
                MessageBox.Show("Địa chỉ không được vượt quá 200 ký tự.", "Lỗi dữ liệu");
                txtDiaChi.Focus();
                return false;
            }

            if (!Regex.IsMatch(tenNCC, @"^[a-zA-Z0-9\s\-_À-ỹ]+$"))
            {
                MessageBox.Show("Tên nhà cung cấp chỉ được chứa chữ, số, khoảng trắng, gạch nối (-) hoặc gạch dưới (_).", "Ký tự không hợp lệ");
                txtTenNCC.Focus();
                return false;
            }

            return true;
        }

        private string ChuanHoaSoDienThoai(string sdt, string quocGia)
        {
            sdt = Regex.Replace(sdt, @"[^\d\+]", "");

            if (quocGia.Contains("Việt Nam"))
            {
                sdt = ChuanHoaTheoMa("+84", sdt);
                if (!Regex.IsMatch(sdt, @"^\+84\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại Việt Nam không hợp lệ.");
                    txtSDT.Focus();
                    return null;
                }
            }
            else if (quocGia.Contains("Mỹ"))
            {
                sdt = ChuanHoaTheoMa("+1", sdt);
                if (!Regex.IsMatch(sdt, @"^\+1\d{10}$"))
                {
                    MessageBox.Show("Số điện thoại Mỹ không hợp lệ.");
                    txtSDT.Focus();
                    return null;
                }
            }
            else if (quocGia.Contains("Nhật Bản"))
            {
                sdt = ChuanHoaTheoMa("+81", sdt);
                if (!Regex.IsMatch(sdt, @"^\+81\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại Nhật Bản không hợp lệ.");
                    txtSDT.Focus();
                    return null;
                }
            }
            else if (quocGia.Contains("Hàn Quốc"))
            {
                sdt = ChuanHoaTheoMa("+82", sdt);
                if (!Regex.IsMatch(sdt, @"^\+82\d{9,10}$"))
                {
                    MessageBox.Show("Số điện thoại Hàn Quốc không hợp lệ.");
                    txtSDT.Focus();
                    return null;
                }
            }
            else if (quocGia.Contains("Trung Quốc"))
            {
                sdt = ChuanHoaTheoMa("+86", sdt);
                if (!Regex.IsMatch(sdt, @"^\+86\d{10,11}$"))
                {
                    MessageBox.Show("Số điện thoại Trung Quốc không hợp lệ.");
                    txtSDT.Focus();
                    return null;
                }
            }
            else
            {
                if (!Regex.IsMatch(sdt, @"^\+?\d{6,15}$"))
                {
                    MessageBox.Show("Số điện thoại quốc tế không hợp lệ.");
                    txtSDT.Focus();
                    return null;
                }
            }

            return sdt;
        }

        private string ChuanHoaTheoMa(string maQuocGia, string sdt)
        {
            if (sdt.StartsWith("0"))
                return maQuocGia + sdt.Substring(1);
            if (sdt.StartsWith(maQuocGia.TrimStart('+')))
                return "+" + sdt;
            if (!sdt.StartsWith("+"))
                return maQuocGia + sdt;
            return sdt;
        }

        private void LuuNhaCungCap(SupplierDto supplier)
        {
            Result<bool> result;
            if (string.IsNullOrEmpty(_SupplierId))
                result = _SupplierService.CreateSupplier(supplier);
            else
                result = _SupplierService.UpdateSupplier(supplier);

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi");
                return;
            }

            DataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu thành công.", "Thông báo");
            this.Close();
        }
    }
}
