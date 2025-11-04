using CrystalDecisions.ReportAppServer;
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_ChiTietKiemKho : Form
    {
        public event EventHandler dataChanged;
        public readonly IStockCheckDetailService _stockCheckDetailService;
        private readonly IStockDetailService _stockDetailService;
        private readonly IUserSession _userSession;
        private string _checkID;
        private string _checkDetailID;

        public frmChucNang_ChiTietKiemKho(IStockCheckDetailService stockCheckDetailService, IStockDetailService stockDetailService, IUserSession userSession, string checkID = null, string checkDetailID = null)
        {
            InitializeComponent();
            _stockCheckDetailService = stockCheckDetailService;
            _stockDetailService = stockDetailService;
            _userSession = userSession;
            _checkID = checkID;
            _checkDetailID = checkDetailID;
        }


        private void frmChucNang_ChiTietKiemKho_Load(object sender, EventArgs e)
        {


            if (string.IsNullOrEmpty(_checkDetailID)) { return; }
            var entity = _stockCheckDetailService.GetStockCheckDetailByID(_checkDetailID);
            if (!entity.Succeeded && entity.Data == null) return;
            txtMaSP.Enabled = false;
            txtMaChiTiet.Text = entity.Data.Id;
            txtMaSP.Text = entity.Data.ProductId;
            txtTenSP.Text = entity.Data.ProductName;
            txtSLHeThong.Text = entity.Data.QuantitySystem.ToString();
            txtSLThucTe.Text = entity.Data.QuantityActual.ToString();
            rtxtGhiChu.Text = entity.Data.Note.ToString();
        }


        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string exportDetailID = txtMaChiTiet.Text.Trim();
            string productID = txtMaSP.Text.Trim();
            int quantitySystem = Convert.ToInt32(txtSLHeThong.Text.Trim());
            if (string.IsNullOrWhiteSpace(txtSLThucTe.Text) ||
               !int.TryParse(txtSLThucTe.Text, out int quantityActual) || quantityActual <= 0)
            {
                MessageBox.Show("So luong thuc te phải là số hợp lệ và không được để trống hoặc bằng 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSLThucTe.Focus();
                return;
            }
            if (Regex.IsMatch(rtxtGhiChu.Text.Trim(), @"[^a-zA-Z0-9\s\u00C0-\u1EF9,./-]"))
            {
                MessageBox.Show("Ghi chú không được chứa ký tự đặc biệt lạ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtGhiChu.Focus();
                return;
            }
            string note = rtxtGhiChu.Text.Trim();

            var detailDto = new StockCheckDetailDto()
            {

                CheckId = _checkID,
                ProductId = productID,
                QuantityActual = quantityActual,
                QuantitySystem = quantitySystem,
                Note = note


            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_checkDetailID))
            {
                result = _stockCheckDetailService.CreateStockCheckDetail(detailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                detailDto.Id = exportDetailID;
                result = _stockCheckDetailService.UpdateStockCheckDetail(detailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu phiếu kiem thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                txtSLHeThong.Text = result.Data.Quantity.ToString();

            }
            else
            {
                txtTenSP.Text = string.Empty;
                txtSLHeThong.Text = string.Empty;
            }

        }
    }
}
