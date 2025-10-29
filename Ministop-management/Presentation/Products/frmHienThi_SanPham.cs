using Guna.UI2.WinForms;
using Presentation.Products;
using Services.Interfaces;
using Services.Services;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_SanPham : Form
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductService _productService;
        private readonly ISupplierService _supllierService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPagePCT = 1;
        private long _totalPageNCC = 1;
        private long _totalPageSP = 1;

        public frmHienThi_SanPham(IProductCategoryService ProductCategoryService, IUnityContainer container, IUserSession userSession,IProductService ProductService, ISupplierService SupllierService)
        {
            InitializeComponent();
            _productCategoryService = ProductCategoryService;
            _productService = ProductService;
            _supllierService = SupllierService;
            _container = container;
            _userSession = userSession;
            LoadDataPCT();
            LoadDataNCC();
            LoadDataSP();
        }
       



        private void frmHienThi_SanPham_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlSP.TabPages.Remove(tabSanPham);
                tabControlSP.TabPages.Remove(tabLoaiSanPham);
                tabControlSP.TabPages.Remove(tabNhaCungCap);
                tabControlSP.TabPages.Remove(tabKhuyenMai);
            }

        }


        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            frmChucNang_PhieuGiamGia chucNang = new frmChucNang_PhieuGiamGia();
            chucNang.Show();
        }

      

        

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            frmChucNang_PhieuGiamGia chucNang = new frmChucNang_PhieuGiamGia();
            chucNang.Show();
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            frmChucNang_GiamGiaSP chucNang = new frmChucNang_GiamGiaSP();
            chucNang.Show();
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
           
        }

        private void guna2TextBox12_TextChanged(object sender, EventArgs e)
        {

        }

        #region loai san pham
        private void LoadDataPCT(int pageNumber = 1, int pageSize = 2)
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLoaiSanPham");
            dt.Columns.Add("TenLoaiSanPham");
            dt.Columns.Add("MoTa");
            using (var childContainer = _container.CreateChildContainer())
            {
                var ProductCategoryService = childContainer.Resolve<IProductCategoryService>();
                var list = ProductCategoryService.GetProductCategory(pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPagePCT = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.CategoryId, item.CategoryName, item.Description);
                }
            }
            dgvloaisanpham.DataSource = dt;
            dgvloaisanpham.AllowUserToAddRows = false;
            dgvloaisanpham.ReadOnly = true;
            dgvloaisanpham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvloaisanpham.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvloaisanpham.Columns.Add(btnEdit);
            }

            if (dgvloaisanpham.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvloaisanpham.Columns.Add(btnDelete);
            }

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvloaisanpham.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvloaisanpham.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvloaisanpham.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvloaisanpham.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvloaisanpham.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvloaisanpham.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvloaisanpham.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvloaisanpham.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvloaisanpham.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvloaisanpham.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvloaisanpham.Columns[e.ColumnIndex].Name;
                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        new Font("Segoe UI", 9, FontStyle.Bold),
                        e.CellBounds,
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    e.Handled = true;
                }
            };
            btnTrangTruocPCT.Enabled = pageNumber > 1;
            btnTrangSauPCT.Enabled = pageNumber <= _totalPagePCT;

        }
        private void btnTrangSauPCT_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangLSP.Text);
            btnTrangTruocPCT.Enabled = true;
            if (number <= _totalPagePCT)
            {
                var pageNumber = ++number;
                txtTrangLSP.Text = pageNumber.ToString();
                LoadDataPCT(pageNumber);
            }
        }

        private void btnTrangTruocPCT_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangLSP.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtTrangLSP.Text = pageNumber.ToString();
                LoadDataPCT(pageNumber);

            }
            else
            {
                btnTrangTruocPCT.Enabled = false;
            }
        }

        private void ibtnDuLieuBiXoaPCT_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_LSP>();
            frmThungRac.datachanged += (s, ev) => LoadDataPCT();
            frmThungRac.ShowDialog();
        }
       
        //xong
        private void dgvloaisanpham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string ProductCategoryId = dgvloaisanpham.Rows[e.RowIndex].Cells["MaLoaiSanPham"].Value.ToString();
            if (dgvloaisanpham.Columns[e.ColumnIndex].Name == "Edit")
            {
                var frmChucNangLoaiSanPham = _container.Resolve<frmChucNang_LoaiSanPham>(new ParameterOverride("productCategoryId", ProductCategoryId));
                frmChucNangLoaiSanPham.DataChanged += (s, ev) =>
                {
                    LoadDataPCT();
                };
                frmChucNangLoaiSanPham.ShowDialog();
            }
            else if (dgvloaisanpham.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa loại sản phẩm {ProductCategoryId}?","Xác nhận",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
                if(result == DialogResult.Yes)
                {
                    _productCategoryService.RemoveProductCategory(ProductCategoryId);
                    MessageBox.Show("Xóa thành công");
                    LoadDataPCT();
                }
            }
        }
      
        private void btnthemPCT_Click(object sender, EventArgs e)
        {
            var frmChucNangLoaiSanPham = _container.Resolve<frmChucNang_LoaiSanPham>();
            frmChucNangLoaiSanPham.DataChanged += (s, ev) => LoadDataPCT();
            frmChucNangLoaiSanPham.ShowDialog();
        }
        #endregion

        
        #region nha cung cap
        private void LoadDataNCC(int pageNumber = 1, int pageSize = 2)
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNhaCungCap");
            dt.Columns.Add("TenNhaCungCap");
            dt.Columns.Add("SoDienThoai");
            dt.Columns.Add("DiaChi");
            using (var childContainer = _container.CreateChildContainer())
            {
                var SupplierService  = childContainer.Resolve<ISupplierService>();
                var list = SupplierService.GetSupplier(pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPageNCC = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.SupplierId, item.SupplierName, item.Phone,item.Address);
                }
            }
            dgvnhacungcap.DataSource = dt;
            dgvnhacungcap.AllowUserToAddRows = false;
            dgvnhacungcap.ReadOnly = true;
            dgvnhacungcap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvnhacungcap.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvnhacungcap.Columns.Add(btnEdit);
            }

            if (dgvnhacungcap.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvnhacungcap.Columns.Add(btnDelete);
            }

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvnhacungcap.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvnhacungcap.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvnhacungcap.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvnhacungcap.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvnhacungcap.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvnhacungcap.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvnhacungcap.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvnhacungcap.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvnhacungcap.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvnhacungcap.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvnhacungcap.Columns[e.ColumnIndex].Name;
                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        new Font("Segoe UI", 9, FontStyle.Bold),
                        e.CellBounds,
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    e.Handled = true;
                }
            };
            btnTrangTruocNCC.Enabled = pageNumber > 1;
            btnTrangSauNCC.Enabled = pageNumber <= _totalPageNCC;

        }

        private void dgvnhacungcap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string suppplierid = dgvnhacungcap.Rows[e.RowIndex].Cells["MaNhaCungCap"].Value.ToString();
            if (dgvnhacungcap.Columns[e.ColumnIndex].Name == "Edit")
            {
                var frmChucNang_NhaCungCap = _container.Resolve<frmChucNang_NhaCungCap>(new ParameterOverride("supplierId", suppplierid));
                frmChucNang_NhaCungCap.DataChanged += (s, ev) =>
                {
                    LoadDataNCC();
                };
                frmChucNang_NhaCungCap.ShowDialog();
            }
            else if (dgvnhacungcap.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa loại sản phẩm {suppplierid}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    _supllierService.RemoveSupplier(suppplierid);
                    MessageBox.Show("Xóa thành công");
                    LoadDataNCC();
                }
            }
        }
        

        private void dgvSanPham_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;
            string productid = dgvSanPham.Rows[e.RowIndex].Cells["MaSanPham"].Value.ToString();
            if (dgvSanPham.Columns[e.ColumnIndex].Name == "Edit")
            {
                var frmChucNang_SanPham = _container.Resolve<frmChucNang_SanPham>(new ParameterOverride("ProductId", productid));
                frmChucNang_SanPham.DataChanged += (s, ev) =>
                {
                    LoadDataSP();
                };
                frmChucNang_SanPham.ShowDialog();
            }
            else if (dgvSanPham.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa sản phẩm {productid}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    _productService.RemoveProduct(productid);
                    MessageBox.Show("Xóa thành công");
                    LoadDataSP();
                }
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var frmChucNangNhaCungCap = _container.Resolve<frmChucNang_NhaCungCap>();
            frmChucNangNhaCungCap.DataChanged += (s, ev) => LoadDataNCC();
            frmChucNangNhaCungCap.ShowDialog();
        }

        private void ibtnThungRacNCC_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_NCC>();
            frmThungRac.datachanged += (s, ev) => LoadDataNCC();
            frmThungRac.ShowDialog();
        }

        private void btnTrangTruocNCC_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangNCC.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangNCC.Text = pageNumber.ToString();
                LoadDataNCC(pageNumber);

            }
            else
            {
                btnTrangTruocNCC.Enabled = false;
            }
        }

        private void btnTrangSauNCC_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangNCC.Text);
            btnTrangTruocNCC.Enabled = true;
            if (number <= _totalPageNCC)
            {
                var pageNumber = ++number;
                 txtSoTrangNCC.Text = pageNumber.ToString();
                LoadDataNCC(pageNumber);
            }
        }
        #endregion
        #region san pham
        private void LoadDataSP(int pageNumber = 1, int pageSize = 10)
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenLoaiSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("DonVi");
            dt.Columns.Add("GiaTieuChuan");
            using (var childContainer = _container.CreateChildContainer())
            {
                var ProductService = childContainer.Resolve<IProductService>();
                var list = ProductService.GetProduct(pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPageSP = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.ProductId, item.CategoryId, item.ProductName,item.Unit,item.StandardPrice);
                }
            }
            dgvSanPham.DataSource = dt;
            dgvSanPham.AllowUserToAddRows = false;
            dgvSanPham.ReadOnly = true;
            dgvSanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvSanPham.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvSanPham.Columns.Add(btnEdit);
            }

            if (dgvSanPham.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvSanPham.Columns.Add(btnDelete);
            }

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvSanPham.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvSanPham.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvSanPham.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvSanPham.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvSanPham.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvSanPham.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvSanPham.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvSanPham.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvSanPham.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvSanPham.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvSanPham.Columns[e.ColumnIndex].Name;
                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        new Font("Segoe UI", 9, FontStyle.Bold),
                        e.CellBounds,
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    e.Handled = true;
                }
            };
            btnTrangTruocSP.Enabled = pageNumber > 1;
            btnTrangSauSP.Enabled = pageNumber <= _totalPageSP;

        }
        #endregion

        private void btnThemSP_Click(object sender, EventArgs e)
        {
            var frmChucNangSP = _container.Resolve<frmChucNang_SanPham>();
            frmChucNangSP.DataChanged += (s, ev) => LoadDataSP();
            frmChucNangSP.ShowDialog();
        }

        private void ibtnThungRacSP_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_SP>();
            frmThungRac.datachanged += (s, ev) => LoadDataSP();
            frmThungRac.ShowDialog();
        }

        private void btnTrangTruocSP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangSP.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtTrangSP.Text = pageNumber.ToString();
                LoadDataSP(pageNumber);

            }
            else
            {
                btnTrangTruocSP.Enabled = false;
            }
        }

        private void btnTrangSauSP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangSP.Text);
            btnTrangTruocSP.Enabled = true;
            if (number <= _totalPageSP)
            {
                var pageNumber = ++number;
                txtTrangSP.Text = pageNumber.ToString();
                LoadDataSP(pageNumber);
            }
        }

        private void dgvnhacungcap_DoubleClick(object sender, EventArgs e)
        {
            int row=dgvnhacungcap.CurrentCell.RowIndex;
            string supplierId = dgvnhacungcap.Rows[row].Cells["MaNhaCungCap"].Value.ToString();
            var frmChucNangLoaiSanPham = _container.Resolve<frmHienThi_NhaCungCapSanPham>(new ParameterOverride("supplierID", supplierId));
            frmChucNangLoaiSanPham.ShowDialog();
        }
    }
}
