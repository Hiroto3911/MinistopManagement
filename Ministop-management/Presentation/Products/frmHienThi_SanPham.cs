using Guna.UI2.WinForms;
using Presentation.Products;
using Presentation.Products.Dialogs;
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
        private readonly IPromotionService _promotionService;
        private readonly IPriceProposalService _priceProposalService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPagePCT = 1;
        private long _totalPageNCC = 1;
        private long _totalPageSP = 1;
        private long _totalPagePGG = 1;
        private long _totalPageDXG;

        public frmHienThi_SanPham(IProductCategoryService ProductCategoryService, IPriceProposalService priceProposalService, IUnityContainer container, IUserSession userSession,IProductService ProductService, ISupplierService SupllierService, IPromotionService promotionService)
        {
            InitializeComponent();
            _productCategoryService = ProductCategoryService;
            _productService = ProductService;
            _supllierService = SupllierService;
            _promotionService = promotionService;
            _priceProposalService = priceProposalService;
            _container = container;
            _userSession = userSession;
 
        }
       



        private void frmHienThi_SanPham_Load(object sender, EventArgs e)
        {
            LoadTab(tabControlSP.SelectedTab);
            LoadCboCuaHang();
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlSP.TabPages.Remove(tabSanPham);
                tabControlSP.TabPages.Remove(tabLoaiSanPham);
                tabControlSP.TabPages.Remove(tabNhaCungCap);
                tabControlSP.TabPages.Remove(tabKhuyenMai);
                cboCuaHangDXG.Enabled = false;
                cboCuaHangDXG.SelectedValue = _userSession.IdStore;
            }

        }

        #region loai san pham
        private void LoadDataPCT(int pageNumber = 1, int pageSize = 5)
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
                    LoadDataPGG();
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
        private void LoadDataNCC(int pageNumber = 1, int pageSize = 5)
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
            int row = dgvnhacungcap.CurrentCell.RowIndex;
            string supplierId = dgvnhacungcap.Rows[row].Cells["MaNhaCungCap"].Value.ToString();
            var frmChucNangLoaiSanPham = _container.Resolve<frmHienThi_NhaCungCapSanPham>(new ParameterOverride("supplierID", supplierId));
            frmChucNangLoaiSanPham.ShowDialog();
        }
        #endregion

        #region PhieuGiamGia
        private void dgvPGG_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string promotionId = dgvPGG.Rows[e.RowIndex].Cells["MaPhieuGiamGia"].Value.ToString(); 
            if (dgvPGG.Columns[e.ColumnIndex].Name == "Edit")
            {
                var frmChucNangPhieuGiamGia = _container.Resolve<frmChucNang_PhieuGiamGia>(new ParameterOverride("promotionId", promotionId));
                frmChucNangPhieuGiamGia.DataChanged += (s, ev) =>
                {
                    LoadDataPGG();
                };
                frmChucNangPhieuGiamGia.ShowDialog();
            }
            else if (dgvPGG.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa loại sản phẩm {promotionId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    _promotionService.RemovePromotion(promotionId);
                    MessageBox.Show("Xóa thành công");
                    LoadDataPGG();
                }
            }
        }
        private void LoadDataPGG(int pageNumber = 1, int pageSize = 5)
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuGiamGia");
            dt.Columns.Add("TenPhieuGiamGia");
            dt.Columns.Add("NgayBatDau");
            dt.Columns.Add("NgayKetThuc");
            dt.Columns.Add("MucDoUuTien");
            dt.Columns.Add("TrangThai");
            using (var childContainer = _container.CreateChildContainer())
            {
                var promotionService = childContainer.Resolve<IPromotionService>();
                var list = promotionService.GetPromotion(pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPagePGG = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.PromotionId, item.PromotionName, item.StartDate, item.EndDate,item.Priority,item.Status);
                }
            }
            dgvPGG.DataSource = dt;
            dgvPGG.AllowUserToAddRows = false;
            dgvPGG.ReadOnly = true;
            dgvPGG.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvPGG.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvPGG.Columns.Add(btnEdit);
            }

            if (dgvPGG.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvPGG.Columns.Add(btnDelete);
            }

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvPGG.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvPGG.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvPGG.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPGG.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPGG.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvPGG.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvPGG.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvPGG.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvPGG.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvPGG.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvPGG.Columns[e.ColumnIndex].Name;
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
            btnTrangTruocKM.Enabled = pageNumber > 1;
            btnTrangSauKM.Enabled = pageNumber <= _totalPageNCC;

        }




        private void btnThemKM_Click(object sender, EventArgs e)
        {
            var frmChucNangPhieuGiamGia = _container.Resolve<frmChucNang_PhieuGiamGia>();
            frmChucNangPhieuGiamGia.DataChanged += (s, ev) => LoadDataPGG();
            frmChucNangPhieuGiamGia.ShowDialog();
        }

        private void ibtnThungRacPGG_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_GG>();
            frmThungRac.datachanged += (s, ev) => LoadDataPGG();
            frmThungRac.ShowDialog();
        }

        private void dgvPGG_DoubleClick(object sender, EventArgs e)
        {
            int row = dgvPGG.CurrentCell.RowIndex;
            string promotionId = dgvPGG.Rows[row].Cells["MaPhieuGiamGia"].Value.ToString();
            var frmChucNangGiamGiaSP = _container.Resolve<frmHienThi_GiamGiaSanPham>(new ParameterOverride("promotionID", promotionId));
            frmChucNangGiamGiaSP.ShowDialog();
        }

        private void btnTrangTruocKM_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangKM.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtTrangKM.Text = pageNumber.ToString();
                LoadDataPGG(pageNumber);

            }
            else
            {
                btnTrangTruocKM.Enabled = false;
            }
        }

        private void btnTrangSauKM_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangKM.Text);
            btnTrangTruocKM.Enabled = true;
            if (number <= _totalPagePGG)
            {
                var pageNumber = ++number;
                txtTrangKM.Text = pageNumber.ToString();
                LoadDataPGG(pageNumber);
            }
        }
        #endregion

        #region DeXuatGia

        #endregion

     
        private void cboCuaHang_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString());
        }
        private void LoadCboCuaHang()
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var storeServices = childContainer.Resolve<IStoreService>();
                var list = storeServices.GetAll();
                if (list.Succeeded == false && list.Data == null) { return; }
                cboCuaHangDXG.DataSource = list.Data;
                cboCuaHangDXG.ValueMember = "StoreID";
                cboCuaHangDXG.DisplayMember = "StoreName";
            }

        }
        private void LoadDataDXG(string storeId, int pageNumber = 1, int pageSize = 2)
        {

            // ===== 1️⃣ Tạo DataTable cho danh sách cửa hàng =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaDeXuat");
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("GiaCu");
            dt.Columns.Add("GiaMoi");
            dt.Columns.Add("LyDo");
            dt.Columns.Add("TrangThai");
            using (var childContainer = _container.CreateChildContainer())
            {
                var expenseService = childContainer.Resolve<IPriceProposalService>();
                var list = expenseService.GetpriceProposal(storeId, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPageDXG = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                    foreach (var item in list.Data)
                    {
                        dt.Rows.Add(item.ProposalId, item.ProductId, item.ProductName, item.OldPrice, item.NewPrice, item.Reason, item.Status);
                    }
                
            }
            // ===== 2️⃣ Dữ liệu mẫu (có thể thay bằng dữ liệu trong DB sau này) =====
            dgvDuLieuDXG.DataSource = dt;
            dgvDuLieuDXG.AllowUserToAddRows = false;
            dgvDuLieuDXG.ReadOnly = true;

            // ===== 2️⃣ Thêm hai cột nút =====

            //if (_isEditable == true)
            //{

            if (dgvDuLieuDXG.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Sửa",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieuDXG.Columns.Add(btnEdit);
            }

            if (dgvDuLieuDXG.Columns["Delete"] == null && _userSession.Role != "Admin")
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Xóa",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieuDXG.Columns.Add(btnDelete);

                //ApplyGridStyle(dgvDuLieuDXG);
            }
            //}
            //else
            //{
            // Nếu đã chốt phiếu thì ẩn (hoặc xóa) hai cột này nếu có
            //if (dgvDuLieuCP.Columns["Edit"] != null)
            //    dgvDuLieuCP.Columns.Remove("Edit");
            //if (dgvDuLieuCP.Columns["Delete"] != null)
            //    dgvDuLieuCP.Columns.Remove("Delete");
            //}

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieuDXG.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieuDXG.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieuDXG.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieuDXG.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieuDXG.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieuDXG.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieuDXG.RowPostPaint += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var grid = (Guna2DataGridView)s;
                var row = grid.Rows[e.RowIndex];
                var status = row.Cells["TrangThai"].Value?.ToString();

                if (status == "0") // bị từ chối
                {
                    using (Pen p = new Pen(Color.Red, 5)) // viền trái đỏ, dày 4px
                    {
                        int x = e.RowBounds.Left + 1;
                        int y1 = e.RowBounds.Top + 1;
                        int y2 = e.RowBounds.Bottom - 1;

                        e.Graphics.DrawLine(p, x, y1, x, y2);
                    }
                }
            };


            dgvDuLieuDXG.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var grid = (Guna2DataGridView)s;
                var status = grid.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
                bool allowEditDelete = status != "1";
                // 👆 chỉ dòng cuối (dòng mới nhất) mới có nút

                if ((grid.Columns[e.ColumnIndex].Name == "Edit" || grid.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    if (allowEditDelete)
                    {
                        // Chỉ vẽ nếu được phép
                        Color backColor = grid.Columns[e.ColumnIndex].Name == "Edit"
                            ? Color.SeaGreen
                            : Color.IndianRed;

                        using (Brush b = new SolidBrush(backColor))
                            e.Graphics.FillRectangle(b, e.CellBounds);

                        string text = grid.Columns[e.ColumnIndex].Name;
                        TextRenderer.DrawText(
                            e.Graphics,
                            text,
                            new Font("Segoe UI", 9, FontStyle.Bold),
                            e.CellBounds,
                            Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                        );
                    }

                    e.Handled = true;
                }
            };

            btnTrangTruocDXG.Enabled = pageNumber > 1;
            btnTrangSauDXG.Enabled = pageNumber <= _totalPageDXG;


        }
        private void dgvDuLieuDXG_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string status = dgvDuLieuDXG.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            string priceProposalID = dgvDuLieuDXG.Rows[e.RowIndex].Cells["MaDeXuat"].Value.ToString();
            bool allowAction = status != "1";
            if (!allowAction) return;

            if (dgvDuLieuDXG.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCP = _container.Resolve<frmChucNang_DeXuatGia>(new ParameterOverride("priceProposalID", priceProposalID));
                frmChucNangCP.dataChanged += (s, ev) =>
                {
  
                    LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString());
                };
                frmChucNangCP.ShowDialog();



            }
            else if (dgvDuLieuDXG.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu de xuat gia {priceProposalID}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _priceProposalService.RemovepriceProposalDto(priceProposalID);
                    MessageBox.Show("Xóa thành công!");

                    LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString()); // tải lại dữ liệu
                }
            }
        }

        private void btnTrangSauDXG_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangDXG.Text);
            btnTrangTruocDXG.Enabled = true;
            if (number <= _totalPageDXG)
            {
                var pageNumber = ++number;
                txtSoTrangDXG.Text = pageNumber.ToString();
                LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString(), pageNumber);
            }
        }

        private void btnTrangTruocDXG_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangDXG.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangDXG.Text = pageNumber.ToString();
                LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString(), pageNumber);

            }
            else
            {
                btnTrangTruocDXG.Enabled = false;
            }
        }

        private void btnThemDXG_Click(object sender, EventArgs e)
        {

            var frmChucNang = _container.Resolve<frmChucNang_DeXuatGia>();
            frmChucNang.dataChanged += (s, ev) => { LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString()); };
            frmChucNang.ShowDialog();

        }

        private void tabControlSP_SelectedIndexChanged(object sender, EventArgs e)
        {
           var tab = tabControlSP.SelectedTab;
            if(tab.Tag == null)
            {
                LoadTab(tab);
                tab.Tag = "Loaded";
            }
        }

        private void LoadTab(TabPage tab)
        {
            switch (tab.Name)
            {
                case "tabDeXuatGia":
                    LoadDataDXG(cboCuaHangDXG.SelectedValue.ToString());
                    if (_userSession.Role == "Admin")
                    {
                        btnThemDXG.Enabled = false;
                    }
                    break;
                case "tabNhaCungCap":
                    LoadDataNCC();
                    break;
                case "tabKhuyenMai":
                    LoadDataPGG();
                    break;
                case "tabLoaiSanPham":
                    LoadDataPCT();
                    break;
                case "tabSanPham":
                    LoadDataSP();
                    break;
               
            }
        }
    }
}
