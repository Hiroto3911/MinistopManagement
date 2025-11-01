using Domain.DTO;
using Services.Interfaces;
using Shared.Wrappers;
using System;
using System.Linq;
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
            string quocGia = XacDinhQuocGiaTuSoDienThoai(phone);
            cboLoaiSDT.SelectedItem = quocGia;
        }
        private string XacDinhQuocGiaTuSoDienThoai(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return "Khác (nhập thủ công)";

            if (LaSoDienThoaiVietNam(phone))
                return "Việt Nam (+84)";
            else if (LaSoDienThoaiMy(phone))
                return "Mỹ (+1)";
            else if (LaSoDienThoaiNhatBan(phone))
                return "Nhật Bản (+81)";
            else if (LaSoDienThoaiHanQuoc(phone))
                return "Hàn Quốc (+82)";
            else if (LaSoDienThoaiTrungQuoc(phone))
                return "Trung Quốc (+86)";
            else
                return "Khác (nhập thủ công)";
        }
        private bool LaSoDienThoaiVietNam(string phone)
        {
            return phone.StartsWith("+84");
        }

        private bool LaSoDienThoaiMy(string phone)
        {
            return phone.StartsWith("+1");
        }

        private bool LaSoDienThoaiNhatBan(string phone)
        {
            return phone.StartsWith("+81");
        }

        private bool LaSoDienThoaiHanQuoc(string phone)
        {
            return phone.StartsWith("+82");
        }

        private bool LaSoDienThoaiTrungQuoc(string phone)
        {
            return phone.StartsWith("+86");
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string tenNCC = txtTenNCC.Text.Trim();
            string sdt = txtSDT.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();
            string quocGia = cboLoaiSDT.SelectedItem?.ToString();

            // 1️⃣ Kiểm tra dữ liệu cơ bản
            if (!KiemTraDuLieuNhap(tenNCC, sdt, diaChi))
                return;

            // 2️⃣ Kiểm tra chọn quốc gia
            if (string.IsNullOrEmpty(quocGia))
            {
                MessageBox.Show("Vui lòng chọn quốc gia/vùng miền.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiSDT.Focus();
                return;
            }

            // 3️⃣ Chuẩn hóa số điện thoại theo quốc gia
            string sdtChuanHoa = KiemTraSoDienThoaiTheoQuocGia(sdt, quocGia);
            if (sdtChuanHoa == null)
                return;

            // ⚡ Gán lại vào textbox để hiển thị chuẩn hóa (tuỳ chọn)
            txtSDT.Text = sdtChuanHoa;

            // 4️⃣ Lưu vào database
            var supplier = new SupplierDto
            {
                SupplierId = _SupplierId,
                SupplierName = tenNCC,
                Phone = sdtChuanHoa,
                Address = diaChi
            };

            var result = string.IsNullOrEmpty(_SupplierId)
                ? _SupplierService.CreateSupplier(supplier)
                : _SupplierService.UpdateSupplier(supplier);

            // 5️⃣ Kiểm tra kết quả
            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 6️⃣ Gọi sự kiện và thông báo
            DataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
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

            if (string.IsNullOrWhiteSpace(diaChi))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ.", "Thiếu thông tin");
                txtDiaChi.Focus();
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

        private string KiemTraSoDienThoaiTheoQuocGia(string soDienThoai, string quocGia)
        {
            soDienThoai = soDienThoai.Trim();

            // --- Việt Nam (+84) ---
            if (quocGia == "Việt Nam (+84)")
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+84|0)[0-9]{9}$"))
                {
                    if (soDienThoai.StartsWith("0"))
                        soDienThoai = "+84" + soDienThoai.Substring(1);
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show("Số điện thoại Việt Nam không hợp lệ. VD: +84912345678 hoặc 0912345678", "Lỗi");
                    return null;
                }
            }

            // --- Mỹ (+1) ---
            else if (quocGia == "Mỹ (+1)")
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+1)?[0-9]{10}$"))
                {
                    if (!soDienThoai.StartsWith("+1"))
                        soDienThoai = "+1" + soDienThoai;
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show("Số điện thoại Mỹ không hợp lệ. VD: +11234567890 hoặc 1234567890", "Lỗi");
                    return null;
                }
            }

            // --- Nhật Bản (+81) ---
            else if (quocGia == "Nhật Bản (+81)")
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+81|0)[0-9]{9,10}$"))
                {
                    if (soDienThoai.StartsWith("0"))
                        soDienThoai = "+81" + soDienThoai.Substring(1);
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show("Số điện thoại Nhật Bản không hợp lệ. VD: +819012345678 hoặc 09012345678", "Lỗi");
                    return null;
                }
            }

            // --- Hàn Quốc (+82) ---
            else if (quocGia == "Hàn Quốc (+82)")
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+82|0)[0-9]{9,10}$"))
                {
                    if (soDienThoai.StartsWith("0"))
                        soDienThoai = "+82" + soDienThoai.Substring(1);
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show("Số điện thoại Hàn Quốc không hợp lệ. VD: +821012345678 hoặc 01012345678", "Lỗi");
                    return null;
                }
            }

            // --- Trung Quốc (+86) ---
            else if (quocGia == "Trung Quốc (+86)")
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+86|1)[0-9]{10}$"))
                {
                    if (!soDienThoai.StartsWith("+86"))
                        soDienThoai = "+86" + soDienThoai.TrimStart('1');
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show("Số điện thoại Trung Quốc không hợp lệ. VD: +8613712345678 hoặc 13712345678", "Lỗi");
                    return null;
                }
            }

            // --- Khác ---
            else
            {
                if (Regex.IsMatch(soDienThoai, @"^\+?[0-9]{9,15}$"))
                {
                    if (!soDienThoai.StartsWith("+"))
                        soDienThoai = "+" + soDienThoai;
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng kiểm tra lại định dạng.", "Lỗi");
                    return null;
                }
            }
        }
    }
}
