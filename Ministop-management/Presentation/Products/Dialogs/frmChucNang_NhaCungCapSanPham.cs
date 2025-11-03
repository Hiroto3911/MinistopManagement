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
            if (!ValidateInput(out var supplierProduct))
                return;

            var result = SaveSupplierProduct(supplierProduct);

            if (!result.Succeeded)
            {
                ShowError(result.Message ?? "Lưu không thành công.");
                return;
            }

            Datachanged?.Invoke(this, EventArgs.Empty);
            ShowInfo("Lưu thành công");
            this.Close();
        }
        private bool ValidateInput(out SupplierProductDto supplierProduct)
        {
            supplierProduct = null;

            string supplierId = txtMaNCC.Text?.Trim() ?? string.Empty;
            string productId = txtMaSP.Text?.Trim() ?? string.Empty;
            string priceText = txtGiaNCC.Text?.Trim() ?? string.Empty;
            string statusRaw = cboTrangThai.SelectedValue?.ToString()?.Trim() ?? string.Empty;

            if (!ValidateNotEmpty(supplierId, "Mã nhà cung cấp", txtMaNCC)) return false;
            if (!ValidateNotEmpty(productId, "Mã sản phẩm", txtMaSP)) return false;
            if (!ValidateNotEmpty(priceText, "Giá NCC", txtGiaNCC)) return false;

            if (!ValidateMaxLength(supplierId, 50, "Mã nhà cung cấp", txtMaNCC)) return false;
            if (!ValidateMaxLength(productId, 50, "Mã sản phẩm", txtMaSP)) return false;

            if (!TryParseDecimalFlexible(priceText, out var giaNCC))
            {
                ShowError("Giá không hợp lệ. Vui lòng nhập số thập phân hợp lệ.", txtGiaNCC);
                return false;
            }

            if (giaNCC < 0m)
            {
                ShowError("Giá không được nhỏ hơn 0.", txtGiaNCC);
                return false;
            }

            if (giaNCC > 1_000_000_000m)
            {
                ShowError("Giá không được lớn hơn 1,000,000,000.", txtGiaNCC);
                return false;
            }

            byte trangThai = ParseStatus(statusRaw);
            if (trangThai == 0)
            {
                ShowError("Trạng thái không hợp lệ. Vui lòng chọn trạng thái.", cboTrangThai);
                return false;
            }

            supplierProduct = new SupplierProductDto
            {
                SupplierId = supplierId,
                ProductId = productId,
                SupplyPrice = giaNCC,
                Status = trangThai
            };

            return true;
        }
        private bool ValidateNotEmpty(string value, string fieldName, Control control)
        {
            if (string.IsNullOrEmpty(value))
            {
                ShowError($"{fieldName} không được để trống.", control);
                return false;
            }
            return true;
        }
        private bool ValidateMaxLength(string value, int maxLength, string fieldName, Control control)
        {
            if (value.Length > maxLength)
            {
                ShowError($"{fieldName} tối đa {maxLength} ký tự.", control);
                return false;
            }
            return true;
        }
        private bool TryParseDecimalFlexible(string text, out decimal value)
        {
            value = 0m;
            if (string.IsNullOrWhiteSpace(text)) return false;

            var t = text.Trim();
            return decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out value)
                || decimal.TryParse(t, NumberStyles.Number, new CultureInfo("vi-VN"), out value)
                || decimal.TryParse(t, NumberStyles.Number, CultureInfo.CurrentCulture, out value);
        }
        private byte ParseStatus(string statusRaw)
        {
            if (string.IsNullOrEmpty(statusRaw)) return 0;

            if (string.Equals(statusRaw, "hoạt động", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusRaw, "hoat dong", StringComparison.OrdinalIgnoreCase))
                return 1;

            if (string.Equals(statusRaw, "tạm ngưng hoạt động", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(statusRaw, "tam ngung hoat dong", StringComparison.OrdinalIgnoreCase))
                return 2;

            return byte.TryParse(statusRaw, out var result) && (result == 1 || result == 2)
                ? result
                : (byte)0;
        }
        private Result<bool> SaveSupplierProduct(SupplierProductDto supplierProduct)
        {
            if (!string.IsNullOrEmpty(_supplierProductId))
            {
                supplierProduct.Id = _supplierProductId;
                return _supplierProductService.UpdateSupplierProduct(supplierProduct);
            }
            return _supplierProductService.CreateSupplierProduct(supplierProduct);
        }
        private void ShowError(string message, Control control = null)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            control?.Focus();
        }
        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        txtTenSP.Text = null;
                }
            }
            catch
            {
                txtTenSP.Text = "(Lỗi lấy dữ liệu)";
            }
        }

        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtTenSP_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
