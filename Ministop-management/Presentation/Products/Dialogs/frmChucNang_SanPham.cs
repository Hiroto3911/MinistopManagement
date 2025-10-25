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
    public partial class frmChucNang_SanPham : Form
    {
        bool _MouseDown;
        private Point offSet;
        public event EventHandler DataChanged;
        private readonly IProductService _ProductService;
        private readonly IUnityContainer _container;
        private string _ProductId;
        public frmChucNang_SanPham(IProductService productService,IUnityContainer container,string ProductId = null)
        {
            InitializeComponent();
            _ProductService = productService;
            _container = container;
            _ProductId = ProductId;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            offSet.X = e.X; offSet.Y = e.Y;
            _MouseDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offSet.X, currentScreenPos.Y - offSet.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _MouseDown = false;
        }
        
        private void LoadCboLSP()
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var productServices = childContainer.Resolve<IProductCategoryService>();
                var list = productServices.GetAll();
                if (list.Succeeded == false && list.Data == null) { return; }
                cboLoaiSP.DataSource = list.Data;
                cboLoaiSP.ValueMember = "CategoryID";
                cboLoaiSP.DisplayMember = "CategoryName";
            }
        }

        private void frmChucNang_SanPham_Load_1(object sender, EventArgs e)
        {
            cboLoaiSP.DropDownStyle = ComboBoxStyle.DropDownList;
            LoadCboLSP();
            //cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!string.IsNullOrEmpty(_ProductId))
            {
                var entity = _ProductService.GetProductByID(_ProductId);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtMaSP.Text = entity.Data.ProductId;
                cboLoaiSP.Text = entity.Data.CategoryId;
                txtTenSP.Text = entity.Data.ProductName;
                txtUnit.Text = entity.Data.Unit;
                txtGTC.Text = entity.Data.StandardPrice.ToString();
            }
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            //decimal  standardPrice = Convert.ToDecimal(txtGTC.Text);
            //var product = new ProductDto() { ProductId = txtMaSP.Text, CategoryId = cboLoaiSP.SelectedValue.ToString(), ProductName = txtTenSP.Text, Unit = txtUnit.Text,StandardPrice = standardPrice };
            //Result<bool> result;
            //if (string.IsNullOrEmpty(txtMaSP.Text))
            //{
            //    result = _ProductService.CreateProduct(product);
            //    DataChanged?.Invoke(this, EventArgs.Empty);
            //}
            //else
            //{
            //    product.ProductId = _ProductId;
            //    result = _ProductService.UpdateProduct(product);
            //    DataChanged?.Invoke(this, EventArgs.Empty);
            //}
            //if (result.Succeeded == false)
            //{
            //    MessageBox.Show($"{result.Message}", "Lỗi");
            //    return;
            //}

            //MessageBox.Show($"Luu thanh cong", "Thong bao");
            //this.Close();
            string loaiSP = cboLoaiSP.SelectedValue?.ToString() ?? "";
            string tenSP = txtTenSP.Text.Trim();
            string donVi = txtUnit.Text.Trim();
            string giaTieuChuanText = txtGTC.Text.Trim();

            // 1️⃣ Kiểm tra loại sản phẩm
            if (string.IsNullOrEmpty(loaiSP))
            {
                MessageBox.Show("Vui lòng chọn loại sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboLoaiSP.Focus();
                return;
            }

            // 2️⃣ Kiểm tra tên sản phẩm
            if (string.IsNullOrWhiteSpace(tenSP))
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return;
            }

            if (tenSP.Length > 100)
            {
                MessageBox.Show("Tên sản phẩm không được vượt quá 100 ký tự.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(tenSP, @"^[a-zA-Z0-9\s\-_À-ỹ]+$"))
            {
                MessageBox.Show("Tên sản phẩm chỉ được chứa chữ, số, khoảng trắng, gạch nối (-) hoặc gạch dưới (_).",
                                "Ký tự không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenSP.Focus();
                return;
            }

            // 3️⃣ Kiểm tra đơn vị
            if (string.IsNullOrWhiteSpace(donVi))
            {
                MessageBox.Show("Vui lòng nhập đơn vị tính.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnit.Focus();
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(donVi, @"^[a-zA-Z0-9\s\-_À-ỹ]+$"))
            {
                MessageBox.Show("Đơn vị chỉ được chứa chữ, số và khoảng trắng.", "Ký tự không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnit.Focus();
                return;
            }

            // 4️⃣ Kiểm tra giá tiêu chuẩn
            if (string.IsNullOrWhiteSpace(giaTieuChuanText))
            {
                MessageBox.Show("Vui lòng nhập giá tiêu chuẩn.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGTC.Focus();
                return;
            }

            if (!decimal.TryParse(giaTieuChuanText, out decimal standardPrice))
            {
                MessageBox.Show("Giá tiêu chuẩn phải là số hợp lệ.", "Lỗi định dạng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGTC.Focus();
                return;
            }

            if (standardPrice <= 0)
            {
                MessageBox.Show("Giá tiêu chuẩn phải lớn hơn 0.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGTC.Focus();
                return;
            }

            // 5️⃣ Tạo DTO
            var product = new ProductDto()
            {
                CategoryId = loaiSP,
                ProductName = tenSP,
                Unit = donVi,
                StandardPrice = standardPrice
            };

            Result<bool> result;

            // 6️⃣ Thêm mới hoặc cập nhật
            if (string.IsNullOrEmpty(_ProductId))
            {
                result = _ProductService.CreateProduct(product);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                product.ProductId = _ProductId;
                result = _ProductService.UpdateProduct(product);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }

            // 7️⃣ Kiểm tra kết quả xử lý
            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
