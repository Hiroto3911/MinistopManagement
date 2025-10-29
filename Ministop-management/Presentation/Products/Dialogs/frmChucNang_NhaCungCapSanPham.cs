using Domain.DTO;
using Services.Interfaces;
using Services.Services;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Unity;

namespace Presentation
{
    public partial class frmChucNang_NhaCungCapSanPham : Form
    {
        private readonly IUnityContainer _container;
        public event EventHandler Datachanged;
        private readonly SupplierProductService _supplierProductService;
        private string _supplierProductId;
        private readonly string _supplierID;

        public frmChucNang_NhaCungCapSanPham(SupplierProductService supplierProductService, IUnityContainer container, string supplierProductId = null, string supplierID = null)
        {
            InitializeComponent();
            _supplierProductService = supplierProductService;
            _container = container;
            _supplierProductId = supplierProductId;
            _supplierID = supplierID;
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            decimal giaNCC;
            if (!decimal.TryParse(txtGiaNCC.Text, out giaNCC))
            {
                MessageBox.Show("Giá không hợp lệ.", "Lỗi");
                return;
            }

            byte trangthai = 0;
            if (cboTrangThai.SelectedValue?.ToString() == "hoạt động")
                trangthai = 1;
            else if (cboTrangThai.SelectedValue?.ToString() == "tạm ngưng hoạt động")
                trangthai = 2;

            var supplierProduct = new SupplierProductDto()
            {
                SupplierId = txtMaNCC.Text.Trim(),
                ProductId = txtMaSP.Text.Trim(),
                SupplyPrice = giaNCC,
                Status = trangthai
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_supplierProductId))
            {
                result = _supplierProductService.CreateSupplierProduct(supplierProduct);
            }
            else
            {
                supplierProduct.Id = _supplierProductId;
                result = _supplierProductService.UpdateSupplierProduct(supplierProduct);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi");
                return;
            }

            Datachanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu thành công", "Thông báo");
            this.Close();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChucNang_NhaCungCapSanPham_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_supplierID))
                txtMaNCC.Text = _supplierID;

            List<string> list = new List<string>() { "hoạt động", "tạm ngưng hoạt động", "ngưng hoạt động" };
            cboTrangThai.DataSource = list;

            txtMaSP.TextChanged += (s, e2) =>
            {
                if (!string.IsNullOrWhiteSpace(txtMaSP.Text))
                    HienThiTenSanPham(txtMaSP.Text.Trim());
                else
                    txtTenSP.Text = string.Empty;
            };

            txtMaSP.Leave += (s, e2) =>
            {
                if (!string.IsNullOrWhiteSpace(txtMaSP.Text))
                    HienThiTenSanPham(txtMaSP.Text.Trim());
                else
                    txtTenSP.Text = string.Empty;
            };

            if (!string.IsNullOrEmpty(_supplierProductId))
            {
                var entity = _supplierProductService.GetSupplierProductByID(_supplierProductId);
                if (entity.Succeeded == false || entity.Data == null)
                {
                    MessageBox.Show(entity.Message, "Lỗi");
                    return;
                }

                txtMaNCC.Text = entity.Data.SupplierId;
                txtMaSP.Text = entity.Data.ProductId;
                txtGiaNCC.Text = entity.Data.SupplyPrice.ToString();
                cboTrangThai.Text = entity.Data.Status.ToString();
                HienThiTenSanPham(entity.Data.ProductId);
            }
        }

        private void HienThiTenSanPham(string maSP)
        {
            if (string.IsNullOrWhiteSpace(maSP))
            {
                txtTenSP.Text = "";
                return;
            }

            try
            {
                using (var childContainer = _container.CreateChildContainer())
                {
                    var productService = childContainer.Resolve<IProductService>();
                    var result = productService.GetProductByID(maSP);

                    if (result != null && result.Succeeded && result.Data != null)
                        txtTenSP.Text = result.Data.ProductName;
                    else
                        txtTenSP.Text = "(Không tìm thấy)";
                }
            }
            catch
            {
                txtTenSP.Text = "(Lỗi lấy dữ liệu)";
            }
        }
    }
}
