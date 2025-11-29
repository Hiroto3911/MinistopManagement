using Domain.DTO;
using Services.Interfaces;
using Services.Services;
using Shared.Security;
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

namespace Presentation
{
    public partial class frmChucNang_ChiTietXuatKho : Form
    {
        public event EventHandler dataChanged;
        private readonly IStockExportDetailService _stockExportDetailService;
        //moi
        private readonly IProductService _productService;
        private readonly IStockDetailService _stockDetailService;

        private readonly IUserSession _userSession;
        public string _exportDetailID;
        private readonly string _exportID;

        public frmChucNang_ChiTietXuatKho(IStockExportDetailService stockExportDetailService, IProductService productService, IStockDetailService stockDetailService, IUserSession userSession, string ExportID = null, string ExportDetailID = null)
        {
            InitializeComponent();
            _stockExportDetailService = stockExportDetailService;
            _productService = productService;
            _userSession = userSession;
            _exportDetailID = ExportDetailID;
            _stockDetailService = stockDetailService;
            _exportID = ExportID;

        }

        private void frmChucNang_ChiTietXuatKho_Load(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(_exportDetailID)) { return; }
            var entity = _stockExportDetailService.GetStockExportDetailByID(_exportDetailID);
            if (!entity.Succeeded && entity.Data == null) return;
            txtMaSP.Enabled = false;
            txtDonViGia.Enabled = false;
            txtPhieuXuat.Text = entity.Data.Id;
            txtMaSP.Text = entity.Data.ProductId;
            txtTenSP.Text = entity.Data.ProductName;
            txtSoLuong.Text = entity.Data.Quantity.ToString();
            txtDonViGia.Text = entity.Data.UnitPrice.ToString();
        }


        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string exportDetailID = txtPhieuXuat.Text.Trim();
            string productID = txtMaSP.Text.Trim();
            
            if (string.IsNullOrEmpty(txtDonViGia.Text.Trim()))
            {
                MessageBox.Show($"{Properties.Messages.Message_ProductNotInStock}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSP.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSoLuong.Text) ||
            !int.TryParse(txtSoLuong.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show($"{lblQuantity.Text} {Properties.Messages.Message_ValidNumber}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                return;
            }
            if (!CheckQuantityStock(productID,quantity))
            {
                MessageBox.Show($"{Properties.Messages.Message_QuantityProductNotEnough}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                return;
            }
            decimal unitPrice = Convert.ToDecimal(txtDonViGia.Text.Trim());

            var detailDto = new StockExportDetailDto()
            {

                ExportId = _exportID,
                ProductId = productID,
                Quantity = quantity,
                UnitPrice = unitPrice,
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_exportDetailID))
            {
                result = _stockExportDetailService.CreatestockExportDetaill(detailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                detailDto.Id = exportDetailID;
                result = _stockExportDetailService.UpdateStockExportDetail(detailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"{Properties.Messages.Message_SavedSuccessfullLy}", $"{Properties.Messages.Message_Notification}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
            //m
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP))
            {
                return;
            }
            var result = _stockDetailService.GetStockDetailByProductID(maSP);
            if (result.Succeeded && result.Data != null)
            {
                txtTenSP.Text = result.Data.ProductName;
                txtDonViGia.Text = result.Data.Price.ToString();

            }
            else
            {
                txtTenSP.Text = string.Empty;
                txtDonViGia.Text = string.Empty;
            }

        }
        private bool CheckQuantityStock(string productID,int quantity)
        {
           var check = _stockDetailService.GetStockDetailByProductID(productID);
            if(check.Succeeded && check.Data != null)
            {
                bool isSatisfied = check.Data.Quantity >= quantity;
                return isSatisfied;
            }
           return false;
        }
    }
}
