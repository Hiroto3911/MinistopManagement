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
            var storeDto = new StoreDto() { StoreName = txtTenCH.Text, Address = rtxtDiaChi.Text, Phone = txtSDT.Text };
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
    }
}
