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
                    MessageBox.Show($"{entity.Message}", $"{Properties.Messages.Message_Error}");
                    return;
                }
                txtMaCH.Text = entity.Data.StoreId;
                txtTenCH.Text = entity.Data.StoreName;
                rtxtDiaChi.Text = entity.Data.Address;
                txtSDT.Text = entity.Data.Phone;
                string phone = entity.Data.Phone ?? "";
                XacDinhQuocGiaTuSoDienThoai(phone);
      

            }
        }


        private void KhoiTaoComboBoxLoaiSDT()
        {
            cboLoaiSDT.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiSDT.Items.Clear();
            Dictionary<string,int> nation = new Dictionary<string, int>()
            {
                { $"{Properties.Resources.Cbo_Others}",0},
                { $"{Properties.Resources.Cbo_VN}",1},
                { $"{Properties.Resources.Cbo_US}",2},
                { $"{Properties.Resources.Cbo_JP}",3},
                { $"{Properties.Resources.Cbo_Korea}",4},
                { $"{Properties.Resources.Cbo_Chinese}",5}
          

            };
            cboLoaiSDT.DataSource = nation.ToList();
            cboLoaiSDT.ValueMember = "Value";
            cboLoaiSDT.DisplayMember = "Key";
        }
        private void XacDinhQuocGiaTuSoDienThoai(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                cboLoaiSDT.SelectedValue = 0;
                return;
            }
            if (LaSoDienThoaiVietNam(phone))
            {
                cboLoaiSDT.SelectedValue = 1;
                return;
            }
            else if (LaSoDienThoaiMy(phone))
            {
                cboLoaiSDT.SelectedValue = 2;
                return;
            }
            else if (LaSoDienThoaiNhatBan(phone))
            {
                cboLoaiSDT.SelectedValue = 3;
                return;
            }
            else if (LaSoDienThoaiHanQuoc(phone))
            {
                cboLoaiSDT.SelectedValue = 4;
                return;
            }
            else if (LaSoDienThoaiTrungQuoc(phone))
            {
                cboLoaiSDT.SelectedValue = 5;
                return;
            }
            else
            {
                cboLoaiSDT.SelectedValue = 0;
                return;
            }
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
        private string KiemTraSoDienThoaiTheoQuocGia(string soDienThoai)
        {
            soDienThoai = soDienThoai.Trim();
            int quocGia = Convert.ToInt32(cboLoaiSDT.SelectedValue);

            // --- Việt Nam (+84) ---
            if (quocGia == 1)
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+84|0)[0-9]{9}$"))
                {
                    if (soDienThoai.StartsWith("0"))
                        soDienThoai = "+84" + soDienThoai.Substring(1);
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show($"{Properties.Messages.Message_InvalidVietnameseNumber}", $"{Properties.Messages.Message_Error}");
                    return null;
                }
            }

            // --- Mỹ (+1) ---
            else if (quocGia == 2)
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+1)?[0-9]{10}$"))
                {
                    if (!soDienThoai.StartsWith("+1"))
                        soDienThoai = "+1" + soDienThoai;
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show($"{Properties.Messages.Message_InvalidUSNumber}", $"{Properties.Messages.Message_Error}");
                    return null;
                }
            }

            // --- Nhật Bản (+81) ---
            else if (quocGia == 3)
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+81|0)[0-9]{9,10}$"))
                {
                    if (soDienThoai.StartsWith("0"))
                        soDienThoai = "+81" + soDienThoai.Substring(1);
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show($"{Properties.Messages.Message_InvalidJapaneseNumber}", $"{Properties.Messages.Message_Error}");
                    return null;
                }
            }

            // --- Hàn Quốc (+82) ---
            else if (quocGia ==4)
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+82|0)[0-9]{9,10}$"))
                {
                    if (soDienThoai.StartsWith("0"))
                        soDienThoai = "+82" + soDienThoai.Substring(1);
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show($"{Properties.Messages.Message_InvalidKoreanNumber}", $"{Properties.Messages.Message_Error}");
                    return null;
                }
            }

            // --- Trung Quốc (+86) ---
            else if (quocGia ==5)
            {
                if (Regex.IsMatch(soDienThoai, @"^(\+86|1)[0-9]{10}$"))
                {
                    if (!soDienThoai.StartsWith("+86"))
                        soDienThoai = "+86" + soDienThoai.TrimStart('1');
                    return soDienThoai;
                }
                else
                {
                    MessageBox.Show($"{Properties.Messages.Message_InvalidChineseNumber}", $"{Properties.Messages.Message_Error}");
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
                    MessageBox.Show($"{Properties.Messages.Message_InvalidPhoneNumber}", $"{Properties.Messages.Message_Error}");
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
                MessageBox.Show($"{lblStoreName.Text} {Properties.Messages.Message_NotNull}!", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCH.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(diaChi))
            {
                MessageBox.Show($"{lblAddress.Text} {Properties.Messages.Message_NotNull}!", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtDiaChi.Focus();
                return;
            }

            // Kiểm tra số điện thoại
            if (string.IsNullOrWhiteSpace(soDT))
            {
                MessageBox.Show($"{lblPhone.Text} {Properties.Messages.Message_NotNull}!", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
            if (string.IsNullOrEmpty(quocGia))
            {
                MessageBox.Show($"{Properties.Messages.Message_SelectNation}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiSDT.Focus();
                return;
            }

            // 3️⃣ Chuẩn hóa số điện thoại theo quốc gia
            string sdtChuanHoa = KiemTraSoDienThoaiTheoQuocGia(soDT);
            if (sdtChuanHoa == null)
                return;
            soDT = sdtChuanHoa;
           
            if (Regex.IsMatch(tenCH, @"[^a-zA-Z0-9\s\u00C0-\u1EF9]"))
            {
                MessageBox.Show($"{lblStoreName.Text} {Properties.Messages.Message_SpecialCharacter}!", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenCH.Focus();
                return;
            }

            if (Regex.IsMatch(diaChi, @"[^a-zA-Z0-9\s\u00C0-\u1EF9,./-]"))
            {
                MessageBox.Show($"{lblAddress.Text} {Properties.Messages.Message_SpecialCharacter}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show($"{result.Message}", $"{Properties.Messages.Message_Error}");
                return;
            }

            MessageBox.Show($"{Properties.Messages.Message_SavedSuccessfullLy}", $"{Properties.Messages.Message_Notification}", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
