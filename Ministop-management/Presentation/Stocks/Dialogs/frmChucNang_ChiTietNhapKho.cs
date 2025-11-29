using Domain.DTO;
using Services.Interfaces;
using Shared.Security;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Stocks.Dialogs
{
    public partial class frmChucNang_ChiTietNhapKho : Form
    {
        public event EventHandler dataChanged;
        public readonly IStockImportDetailSerivce _stockImportDetailService;
        private readonly IStockDetailService _stockDetailService;
        private readonly IUserSession _userSession;
        private string _importID;
        private string _importDetailID;

        public frmChucNang_ChiTietNhapKho(IStockImportDetailSerivce stockImportDetailService, IStockDetailService stockDetailService, IUserSession userSession, string importID, string importDetailID)
        {
            InitializeComponent();
            _stockImportDetailService = stockImportDetailService;
            _stockDetailService = stockDetailService;
            _userSession = userSession;
            _importID = importID;
            _importDetailID = importDetailID;
        }


        private void frmChucNang_ChiTietNhapKho_Load(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(_importDetailID)) { return; }
            var entity = _stockImportDetailService.GetStockImportDetailByID(_importDetailID);
            if (!entity.Succeeded && entity.Data == null) return;
            txtMaSP.Enabled = false;
            txtMaChiTiet.Text = entity.Data.Id;
            txtMaSP.Text = entity.Data.ProductId;
            txtSoluong.Text = entity.Data.Quantity.ToString();
            txtDonGia.Text = entity.Data.UnitPrice.ToString();


        }


        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSoluong.Text) ||
              !int.TryParse(txtSoluong.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show($"{lblQuantity.Text} {Properties.Messages.Message_ValidNumber}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoluong.Focus();
                return;
            }
            string importDetailID = txtMaChiTiet.Text.Trim();
            string productID = txtMaSP.Text.Trim();
            decimal unitPrice = Convert.ToDecimal(txtDonGia.Text.Trim());


            var detailDto = new StockImportDetailDto()
            {

                ImportId = _importID,
                ProductId = productID,
                Quantity = quantity,
                UnitPrice = unitPrice,

            };

            detailDto.Id = importDetailID;
            Result<bool> result = _stockImportDetailService.UpdateStockImportDetail(detailDto);
            dataChanged?.Invoke(this, EventArgs.Empty);

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"{Properties.Messages.Message_SavedSuccessfullLy}", $"{Properties.Messages.Message_Notification}", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }

       

    }
}
