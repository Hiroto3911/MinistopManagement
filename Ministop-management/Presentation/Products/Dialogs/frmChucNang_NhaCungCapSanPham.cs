using Domain.DTO;
using Services.Interfaces;
using Services.Services;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
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
            bool TryParseDecimalFlexible(string text, out decimal value)
            {
                value = 0m;
                if (string.IsNullOrWhiteSpace(text)) return false;
                var t = text.Trim();
                if (decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out value)) return true;
                if (decimal.TryParse(t, NumberStyles.Number, new CultureInfo("vi-VN"), out value)) return true;
                if (decimal.TryParse(t, NumberStyles.Number, CultureInfo.CurrentCulture, out value)) return true;
                return false;
            }
            var supplierId = txtMaNCC.Text?.Trim() ?? string.Empty;
            var productId = txtMaSP.Text?.Trim() ?? string.Empty;
            var priceText = txtGiaNCC.Text?.Trim() ?? string.Empty;
            var statusRaw = cboTrangThai.SelectedValue?.ToString()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(supplierId))
            {
                MessageBox.Show("Mã nhà cung cấp không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNCC.Focus();
                return;
            }

            if (string.IsNullOrEmpty(productId))
            {
                MessageBox.Show("Mã sản phẩm không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSP.Focus();
                return;
            }

            if (string.IsNullOrEmpty(priceText))
            {
                MessageBox.Show("Giá NCC không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaNCC.Focus();
                return;
            }
            const int MaxIdLength = 50;
            if (supplierId.Length > MaxIdLength)
            {
                MessageBox.Show($"Mã nhà cung cấp tối đa {MaxIdLength} ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNCC.Focus();
                return;
            }
            if (productId.Length > MaxIdLength)
            {
                MessageBox.Show($"Mã sản phẩm tối đa {MaxIdLength} ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaSP.Focus();
                return;
            }
            if (!TryParseDecimalFlexible(priceText, out var giaNCC))
            {
                MessageBox.Show("Giá không hợp lệ. Vui lòng nhập số thập phân hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaNCC.Focus();
                return;
            }
            if (giaNCC < 0m)
            {
                MessageBox.Show("Giá không được nhỏ hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaNCC.Focus();
                return;
            }
            const decimal MaxAllowedPrice = 1000000000m;
            if (giaNCC > MaxAllowedPrice)
            {
                MessageBox.Show($"Giá không được lớn hơn {MaxAllowedPrice:N0}.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaNCC.Focus();
                return;
            }
            byte trangthai = 0;
            if (string.Equals(statusRaw, "hoạt động", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusRaw, "hoat dong", StringComparison.OrdinalIgnoreCase))
            {
                trangthai = 1;
            }
            else if (string.Equals(statusRaw, "tạm ngưng hoạt động", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(statusRaw, "tam ngung hoat dong", StringComparison.OrdinalIgnoreCase))
            {
                trangthai = 2;
            }
            else if (byte.TryParse(statusRaw, out var stParsed) && (stParsed == 1 || stParsed == 2))
            {
                trangthai = stParsed;
            }
            else
            {
                MessageBox.Show("Trạng thái không hợp lệ. Vui lòng chọn trạng thái.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cboTrangThai.Focus();
                return;
            }
          
            var supplierProduct = new SupplierProductDto
            {
                SupplierId = supplierId,
                ProductId = productId,
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
                MessageBox.Show(result.Message ?? "Lưu không thành công.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Datachanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            List<string> list = new List<string>() { "hoạt động", "tạm ngưng hoạt động" };
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
                txtMaSP.Enabled = false;
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
