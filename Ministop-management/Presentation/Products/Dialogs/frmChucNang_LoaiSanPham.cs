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

namespace Presentation
{
    public partial class frmChucNang_LoaiSanPham : Form
    {
        private ErrorProvider errorProvider1;
        public event EventHandler DataChanged;
        private readonly IProductCategoryService _ProductCategory;
        private string _ProductCategoryId;
        public frmChucNang_LoaiSanPham(IProductCategoryService productCategory, string productCategoryId= null)
        {
            InitializeComponent();
            _ProductCategory = productCategory;
            _ProductCategoryId = productCategoryId;
        }

        

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool ValidateInput()
        {
            // Xóa các thông báo lỗi cũ (nếu có)
            errorProvider1.Clear();

            bool isValid = true;

            // Kiểm tra tên loại sản phẩm
            if (string.IsNullOrWhiteSpace(txtTenlsp.Text))
            {
                errorProvider1.SetError(txtTenlsp, "Vui lòng nhập tên loại sản phẩm");
                isValid = false;
            }
            else if (txtTenlsp.Text.Length < 3)
            {
                errorProvider1.SetError(txtTenlsp, "Tên loại sản phẩm phải có ít nhất 3 ký tự");
                isValid = false;
            }
            else if (txtTenlsp.Text.Length > 100)
            {
                errorProvider1.SetError(txtTenlsp, "Tên loại sản phẩm không được vượt quá 100 ký tự");
                isValid = false;
            }

            // Kiểm tra mô tả (tùy chọn nhưng nếu có thì kiểm tra độ dài)
            if (!string.IsNullOrWhiteSpace(rtb_Mota.Text) && rtb_Mota.Text.Length > 200)
            {
                errorProvider1.SetError(rtb_Mota, "Mô tả không được vượt quá 200 ký tự");
                isValid = false;
            }

            return isValid;
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            //var productCategory = new ProductCategoryDto() { CategoryName = txtTenlsp.Text, Description = rtb_Mota.Text };
            //Result<bool> result;
            //if (string.IsNullOrEmpty(_ProductCategoryId))
            //{
            //    result = _ProductCategory.CreateProductCategory(productCategory);
            //    DataChanged?.Invoke(this, EventArgs.Empty);
            //}
            //else
            //{
            //    productCategory.CategoryId = _ProductCategoryId;
            //    result = _ProductCategory.UpdateProductCategory(productCategory);
            //    DataChanged?.Invoke(this, EventArgs.Empty);
            //}
            //if (result.Succeeded == false)
            //{
            //    MessageBox.Show($"{result.Message}", "Lỗi");
            //    return;
            //}

            //MessageBox.Show($"Luu thanh cong", "Thong bao");
            //this.Close();
            // Kiểm tra dữ liệu đầu vào
            string tenLoai = txtTenlsp.Text.Trim();
            string moTa = rtb_Mota.Text.Trim();

            // 1️⃣ Kiểm tra trống
            if (string.IsNullOrWhiteSpace(tenLoai))
            {
                MessageBox.Show("Vui lòng nhập tên loại sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenlsp.Focus();
                return;
            }

            // 2️⃣ Kiểm tra độ dài
            if (tenLoai.Length > 100)
            {
                MessageBox.Show("Tên loại sản phẩm không được vượt quá 100 ký tự.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenlsp.Focus();
                return;
            }

            if (moTa.Length > 200)
            {
                MessageBox.Show("Mô tả không được vượt quá 200 ký tự.", "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtb_Mota.Focus();
                return;
            }

            // 3️⃣ Kiểm tra ký tự đặc biệt (chỉ cho phép chữ, số, dấu cách, dấu gạch nối và gạch dưới)
            if (!System.Text.RegularExpressions.Regex.IsMatch(tenLoai, @"^[a-zA-Z0-9\s\-_À-ỹ]+$"))
            {
                MessageBox.Show("Tên loại sản phẩm chỉ được chứa chữ, số, khoảng trắng, gạch nối (-) hoặc gạch dưới (_).",
                                "Ký tự không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenlsp.Focus();
                return;
            }

            // 4️⃣ Tạo DTO
            var productCategory = new ProductCategoryDto()
            {
                CategoryName = tenLoai,
                Description = moTa
            };

            Result<bool> result;

            // 5️⃣ Thêm mới hoặc cập nhật
            if (string.IsNullOrEmpty(_ProductCategoryId))
            {
                result = _ProductCategory.CreateProductCategory(productCategory);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                productCategory.CategoryId = _ProductCategoryId;
                result = _ProductCategory.UpdateProductCategory(productCategory);
                DataChanged?.Invoke(this, EventArgs.Empty);
            }

            // 6️⃣ Kiểm tra kết quả
            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();

        }

        private void frmChucNang_LoaiSanPham_Load(object sender, EventArgs e)
        {
            //cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!string.IsNullOrEmpty(_ProductCategoryId))
            {
                var entity = _ProductCategory.GetProductCategoryByID(_ProductCategoryId);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtTenlsp.Text = entity.Data.CategoryName;
                rtb_Mota.Text = entity.Data.Description;
                _ProductCategoryId = entity.Data.CategoryId;
            }
        }

        private void txtTenlsp_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void rtb_Mota_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void frmChucNang_LoaiSanPham_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
