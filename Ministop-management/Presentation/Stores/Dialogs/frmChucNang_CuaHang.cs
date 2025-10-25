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

            }
        }



        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string maCH = txtMaCH.Text.Trim();
            string tenCH = txtTenCH.Text.Trim();
            string diaChi = rtxtDiaChi.Text.Trim();
            string soDT = txtSDT.Text.Trim();
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
            if (!Regex.IsMatch(soDT, @"^\d{10}$")) // Ví dụ: chỉ cho phép đúng 10 số
            {
                MessageBox.Show("Số điện thoại phải gồm đúng 10 chữ số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSDT.Focus();
                return;
            }
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
    }
}
