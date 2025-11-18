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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using static Unity.Storage.RegistrationSet;

namespace Presentation
{
    public partial class frmChucNang_CuaHang : Form
    {
        public event EventHandler DataChanged;
        private readonly IStoreService _storeService;
        private readonly string _storeId;

        public frmChucNang_CuaHang(IStoreService storeService, string storeId = null)
        {
            InitializeComponent();
            _storeService = storeService;
            _storeId = storeId;
        }

        private void frmChucNang_CuaHang_Load(object sender, EventArgs e)
        {
            KhoiTaoComboBoxLoaiSDT();
            //cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!string.IsNullOrEmpty(_storeId))
            {
                var entity = _storeService.GetStoreByID(_storeId);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtMaCH.Text = entity.Data.StoreId;
                txtTenCH.Text = entity.Data.StoreName;
                rtxtDiaChi.Text = entity.Data.Address;
                txtSDT.Text = entity.Data.Phone;
                string phone = entity.Data.Phone ?? "";
                string quocGia = XacDinhQuocGiaTuSoDienThoai(phone);
                cboLoaiSDT.SelectedItem = quocGia;

            }
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
        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
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
        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maCH = txtMaCH.Text.Trim();
            string tenCH = txtTenCH.Text.Trim();
            string diaChi = rtxtDiaChi.Text.Trim();
            string soDT = txtSDT.Text.Trim();
            string quocGia = cboLoaiSDT.SelectedItem?.ToString();
            if (string.IsNullOrWhiteSpace(tenCH))
            {
                MessageBox.Show("Tên cửa hàng không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(diaChi))
            {
                MessageBox.Show("Địa chỉ không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtDiaChi.Focus();
                return;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrWhiteSpace(soDT))
            {
                MessageBox.Show("Số điện thoại không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (string.IsNullOrEmpty(quocGia))
            {
                MessageBox.Show("Vui lòng chọn quốc gia/vùng miền.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiSDT.Focus();
                return;
            }

            // 3️⃣ Chuẩn hóa số điện thoại theo quốc gia
            string sdtChuanHoa = KiemTraSoDienThoaiTheoQuocGia(soDT, quocGia);
            if (sdtChuanHoa == null)
                return;
            soDT = sdtChuanHoa;
           
            if (Regex.IsMatch(tenCH, @"[^a-zA-Z0-9\s\u00C0-\u1EF9]"))
            {
                MessageBox.Show("Tên cửa hàng không được chứa ký tự đặc biệt!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCH.Focus();
                return;
            }

            if (Regex.IsMatch(diaChi, @"[^a-zA-Z0-9\s\u00C0-\u1EF9,./-]"))
            {
                MessageBox.Show("Địa chỉ không được chứa ký tự đặc biệt lạ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtDiaChi.Focus();
                return;
            }
            var storeDto = new StoreDto() { StoreName = tenCH, Address = diaChi, Phone = soDT };
            Result<bool> result;
            if (string.IsNullOrEmpty(_storeId))
            {
                result = _storeService.CreateStore(storeDto);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                storeDto.StoreId = txtMaCH.Text;
                result = _storeService.UpdateStore(storeDto);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            if (result.Succeeded == false)
            {
                MessageBox.Show($"{result.Message}", "Lỗi");
                return;
            }

            MessageBox.Show($"Luu thanh cong", "Thong bao");
            this.Close();
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // chặn ký tự không hợp lệ
            }

            // Giới hạn độ dài (VD: 10 ký tự)
            if (txtSDT.Text.Length >= 10 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }

        }

        private void txtTenCH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                rtxtDiaChi.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void rtxtDiaChi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { txtTenCH.Focus(); }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter) { txtSDT.Focus(); }
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void txtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { rtxtDiaChi.Focus(); }
            else if (e.KeyCode == Keys.Enter) { btnLuu_Click(sender, e); }
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void frmChucNang_CuaHang_Shown(object sender, EventArgs e)
        {
           
        }
    }
}
