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
        private readonly IUserSession _userSession;
        public string _exportDetailID;

        public frmChucNang_ChiTietXuatKho(IStockExportDetailService stockExportDetailService, IUserSession userSession, string ExportDetailID = null)
        {
            InitializeComponent();
            _stockExportDetailService = stockExportDetailService;
            _userSession = userSession;
            _exportDetailID = ExportDetailID;

        }

        private void frmChucNang_ChiTietXuatKho_Load(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(_exportDetailID)) { return; }
            var entity = _stockExportDetailService.GetStockExportDetailByID(_exportDetailID);
            if (!entity.Succeeded && entity.Data == null) return;
            txtPhieuXuat.Text = entity.Data.ExportId;
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
            string exportID = txtPhieuXuat.Text.Trim();
            string productID = txtMaSP.Text.Trim();
            int quantity = Convert.ToInt32(txtSoLuong.Text.Trim());
            decimal unitPrice = Convert.ToDecimal(txtDonViGia.Text.Trim());

            var detailDto = new StockExportDetailDto()
            {

                ExportId = exportID,
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
                detailDto.Id = _exportDetailID;
                result = _stockExportDetailService.UpdateStockExportDetail(detailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
