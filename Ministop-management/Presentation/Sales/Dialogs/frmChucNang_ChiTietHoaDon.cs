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
            var soluong = Convert.ToInt32(txtSL.Text);
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
                txtTenSP.Text = GetNameProductByID(entity.Data.ProductId);
                txtSL.Text = entity.Data.Quantity.ToString();
                txtDonGia.Text = entity.Data.UnitPrice.ToString();
                txtThanhTien.Text = entity.Data.DiscountAmount.ToString();

            }
        }
        private string GetNameProductByID(string productID)
        {
            using (var childContaner = _container.CreateChildContainer())
            {
                var productService = childContaner.Resolve<IProductService>();
                var list = productService.GetProductByID(productID);
                if (list.Succeeded == false && list.Data == null) return "";
                return list.Data.ProductName;
            }
        }

        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(maSP))
            {
                return;
            }
            var result = _stockDetailService.GetStockDetailByProductID(maSP);
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
    }
}
