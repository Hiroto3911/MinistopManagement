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
using System.Xml.Linq;
using Unity;

namespace Presentation
{
    public partial class frmChucNang_GiamGiaSP : Form
    {
        private ErrorProvider errorProvider1;
        public event EventHandler DataChanged;
        private readonly IPromotionProductService _promotionProductService;
        private readonly IUnityContainer _container;
        private string _promotionProductId;
        private string _promotionID;
        public frmChucNang_GiamGiaSP(IPromotionProductService promotionProductService,IUnityContainer container, string promotionProductId = null, string promotionID = null)
        {
            InitializeComponent();
            _promotionProductService = promotionProductService;
            _promotionProductId = promotionProductId;
            _container = container;
            _promotionID = promotionID;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string MaGG = txtMaGG.Text.Trim();
            string MaSP = txtMaSP.Text.Trim();
            string TenSP = txtTenSP.Text.Trim();
            string note = rtbGhiChu.Text.Trim();

            if (string.IsNullOrEmpty(MaGG))
            {
                MessageBox.Show("Vui lòng nhập mã giảm giá.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaGG.Focus();
                return;
            }

            if (string.IsNullOrEmpty(MaSP))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSP.Focus();
                return;
            }

            if (string.IsNullOrEmpty(TenSP))
            {
                MessageBox.Show("Tên sản phẩm không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return;
            }

            if (!decimal.TryParse(txtSoTienGiam.Text.Trim(), out decimal SoTien))
            {
                MessageBox.Show("Số tiền giảm không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTienGiam.Focus();
                return;
            }

            if (SoTien < 0)
            {
                MessageBox.Show("Số tiền giảm không được âm.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoTienGiam.Focus();
                return;
            }

            if (!int.TryParse(txtSoLuong.Text.Trim(), out int SLTT))
            {
                MessageBox.Show("Số lượng tối thiểu không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }

            if (SLTT <= 0)
            {
                MessageBox.Show("Số lượng tối thiểu phải lớn hơn 0.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtSoLuong.Focus();
                return;
            }
            var promotionProduct = new PromotionProductDto
            {
                Id = _promotionProductId,
                PromotionId = MaGG,
                ProductId = MaSP,
                ProductName = TenSP,
                DiscountAmount = SoTien,
                MinQuantity = SLTT,
                Note = note,
            };
            Result<bool> result = string.IsNullOrEmpty(_promotionProductId)
                ? _promotionProductService.CreatePromotionProduct(promotionProduct)
                : _promotionProductService.UpdatePromotionProduct(promotionProduct);

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
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
        private void frmChucNang_GiamGiaSP_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_promotionID))
                txtMaGG.Text = _promotionID;

            if (!string.IsNullOrEmpty(_promotionProductId))
            {
                var entity = _promotionProductService.GetPromotionProductByID(_promotionProductId);
                if (entity.Succeeded == false || entity.Data == null)
                {
                    MessageBox.Show(entity.Message, "Lỗi");
                    return;
                }

                txtMaSP.Enabled = false;
                txtMaGG.Text = entity.Data.PromotionId;
                txtMaSP.Text = entity.Data.ProductId;
                txtSoTienGiam.Text = entity.Data.DiscountAmount.ToString();
                txtSoLuong.Text = entity.Data.MinQuantity.ToString();
                rtbGhiChu.Text = entity.Data.Note.ToString();
                HienThiTenSanPham(entity.Data.ProductId);
            }
            txtMaSP.Leave += (s, ev) => HienThiTenSanPham(txtMaSP.Text.Trim());
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTenSP_TextChanged(object sender, EventArgs e)
        {
            HienThiTenSanPham(txtMaSP.Text);
        }
    }
}
