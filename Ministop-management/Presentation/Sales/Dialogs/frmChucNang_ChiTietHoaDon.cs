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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace Presentation
{
    public partial class frmChucNang_ChiTietHoaDon : Form
    {
        public event EventHandler dataChanged;
        public readonly IInvoiceDetailService _invoiceDetailService;
        public readonly IUserSession _userSession;
        public readonly IUnityContainer _container;
        public string _invoiceID;
        public string _invoiceDetailID;
        public readonly IStockDetailService _stockDetailService;
        public frmChucNang_ChiTietHoaDon(IInvoiceDetailService invoiceDetailService, IUserSession userSession, IUnityContainer container, IStockDetailService stockDetailService, string invoiceID = null, string invoiceDetailID = null)
        {
            InitializeComponent();
            _invoiceDetailService = invoiceDetailService;
            _userSession = userSession;
            _invoiceID = invoiceID;
            _container = container;
            _invoiceDetailID = invoiceDetailID;
            _stockDetailService = stockDetailService;
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDonGia.Text.Trim()))
            {
                MessageBox.Show("Sản phẩm này không tồn tại trong kho hàng. Vui lòng nhập lại mã!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSP.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(txtSL.Text) ||
           !int.TryParse(txtSL.Text, out int soluong) || soluong <= 0)
            {
                MessageBox.Show("So luong phải là số hợp lệ và không được để trống hoặc bằng 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSL.Focus();
                return;
            }
            if (!CheckQuantityStock(txtMaSP.Text, soluong))
            {
                MessageBox.Show("Số lượng trong kho không đủ đế đáp ứng số lượng xuất của bạn vui lòng điều chỉnh lại số lượng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSL.Focus();
                return;
            }

            var gia = Convert.ToDecimal(txtDonGia.Text);
            var invoiceDetailDto = new InvoiceDetailDto()
            {

                ProductId = txtMaSP.Text,
                InvoiceId = _invoiceID,
                Quantity = soluong,
                UnitPrice = gia,
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_invoiceDetailID))
            {
                result = _invoiceDetailService.CreateInvoiceDetail(invoiceDetailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                invoiceDetailDto.Id = _invoiceDetailID;
                result = _invoiceDetailService.UpdateInvoiceDetail(invoiceDetailDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu chi phí hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void frmChucNang_ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_invoiceID))
            {
                return;
            }
            if (!string.IsNullOrEmpty(_invoiceDetailID))
            {
                var entity = _invoiceDetailService.GetInvoiceDetailByID(_invoiceDetailID);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtMaSP.Text = entity.Data.ProductId;
                UpdateFieldProduct(txtMaSP.Text);
                txtSL.Text = entity.Data.Quantity.ToString();
                txtKM.Text = entity.Data.DiscountAmount.ToString();
                txtThanhTien.Text = entity.Data.FinalUnitPrice.ToString();

            }
        }


        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP))
            {
                return;
            }
            UpdateFieldProduct(maSP);
        }
        private void UpdateFieldProduct(string productID)
        {
            var result = _stockDetailService.GetStockDetailByProductID(productID);
            if (result.Succeeded && result.Data != null)
            {
                txtTenSP.Text = result.Data.ProductName;
                txtDonGia.Text = result.Data.Price.ToString();
            }
            else
            {
                txtTenSP.Text = string.Empty;
                txtDonGia.Text = string.Empty;
            }
        }

        private bool CheckQuantityStock(string productID, int quantity)
        {
            var check = _stockDetailService.GetStockDetailByProductID(productID);
            if (check.Succeeded && check.Data != null)
            {
                bool isSatisfied = check.Data.Quantity >= quantity;
                return isSatisfied;
            }
            return false;
        }

       
    }
}
