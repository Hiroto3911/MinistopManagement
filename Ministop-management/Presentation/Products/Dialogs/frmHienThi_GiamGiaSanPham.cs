using Services.Interfaces;
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
using Unity.Resolution;

namespace Presentation.Products
{
    public partial class frmHienThi_GiamGiaSanPham : Form
    {
        private readonly IUnityContainer _container;
        private readonly IPromotionProductService _promotionProductService;
        private readonly IProductService _productService;
        private string _promotionId;
        private long _totalPageGGSPP = 1;
        private int _currentPage = 1;
        private string _searchKeyword = "";
        private DataTable _originalDataTable;
        private Dictionary<string, Domain.DTO.PromotionProductDto> _existingPromotionProducts = new Dictionary<string, Domain.DTO.PromotionProductDto>();

        // Lưu tạm thời các sản phẩm được tích
        private Dictionary<string, TempProductData> _tempSelectedProducts = new Dictionary<string, TempProductData>();

        // Class lưu dữ liệu tạm thời
        private class TempProductData
        {
            public bool IsChecked { get; set; }
            public decimal SoTienGiam { get; set; }
            public int SoLuongToiThieu { get; set; }
            public string GhiChu { get; set; }
        }

        public frmHienThi_GiamGiaSanPham(IUnityContainer container, IPromotionProductService promotionService, IProductService productService, string promotionID = null)
        {
            InitializeComponent();
            _container = container;
            _promotionProductService = promotionService;
            _productService = productService;
            _promotionId = promotionID;

            LoadExistingPromotionProducts();
            InitializeSearchUI();
            LoadDataGGSP(_promotionId);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        // Khởi tạo sự kiện cho tìm kiếm
        private void InitializeSearchUI()
        {
            // Thiết lập placeholder cho TextBox
            if (txtTimkiem != null)
            {
                txtTimkiem.ForeColor = Color.Gray;
                txtTimkiem.Text = "Nhập tên sản phẩm cần tìm...";

                // Xử lý placeholder effect
                txtTimkiem.Enter += (s, e) =>
                {
                    if (txtTimkiem.Text == "Nhập tên sản phẩm cần tìm...")
                    {
                        txtTimkiem.Text = "";
                        txtTimkiem.ForeColor = Color.Black;
                    }
                };

                txtTimkiem.Leave += (s, e) =>
                {
                    if (string.IsNullOrWhiteSpace(txtTimkiem.Text))
                    {
                        txtTimkiem.Text = "Nhập tên sản phẩm cần tìm...";
                        txtTimkiem.ForeColor = Color.Gray;
                    }
                };

                // Xử lý sự kiện Enter
                txtTimkiem.KeyDown += (s, e) =>
                {
                    if (e.KeyCode == Keys.Enter)
                    {
                        BtnTimKiem_Click(s, e);
                    }
                };
            }

            // Xử lý sự kiện cho button tìm kiếm
            if (btnTimKiem != null)
            {
                btnTimKiem.Click += BtnTimKiem_Click;
            }
        }

        // Xử lý tìm kiếm
        private void BtnTimKiem_Click(object sender, EventArgs e)
        {
            if (txtTimkiem != null)
            {
                string searchText = txtTimkiem.Text.Trim();

                // Bỏ qua nếu đang là placeholder text hoặc rỗng
                if (searchText == "Nhập tên sản phẩm cần tìm..." || string.IsNullOrEmpty(searchText))
                {
                    // Load lại toàn bộ dữ liệu
                    _searchKeyword = "";
                    _currentPage = 1;
                    txtTrangGGSP.Text = "1";
                    LoadDataGGSP(_promotionId, 1);
                    return;
                }

                // Lưu từ khóa tìm kiếm
                _searchKeyword = searchText;

                // Load dữ liệu với từ khóa tìm kiếm
                LoadDataGGSPWithSearch(searchText);
            }
        }

        // Load dữ liệu với tìm kiếm
        private void LoadDataGGSPWithSearch(string searchKeyword)
        {
            // Lưu trạng thái trang hiện tại trước khi tìm kiếm
            if (dgvDuLieu.DataSource != null)
            {
                SaveCurrentPageState();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Chon", typeof(bool));
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("DonVi");
            dt.Columns.Add("GiaChuan");
            dt.Columns.Add("SoTienGiam", typeof(decimal));
            dt.Columns.Add("SoLuongToiThieu", typeof(int));
            dt.Columns.Add("GhiChu");

            using (var childContainer = _container.CreateChildContainer())
            {
                var productService = childContainer.Resolve<IProductService>();

                // Lấy TẤT CẢ sản phẩm
                var allProductsList = productService.GetProduct(1, int.MaxValue);

                if (!allProductsList.Succeeded || allProductsList.Data == null)
                {
                    dgvDuLieu.DataSource = null;
                    return;
                }

                // Tạo list tạm để lọc và sắp xếp
                var tempList = new List<dynamic>();

                foreach (var item in allProductsList.Data)
                {
                    // Lọc theo từ khóa tìm kiếm
                    if (!string.IsNullOrEmpty(searchKeyword))
                    {
                        bool matchName = item.ProductName != null &&
                                        item.ProductName.IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0;
                        bool matchId = item.ProductId != null &&
                                      item.ProductId.IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0;

                        if (!matchName && !matchId)
                        {
                            continue; // Bỏ qua sản phẩm không khớp
                        }
                    }

                    bool isChecked = false;
                    decimal soTienGiam = 0;
                    int soLuongToiThieu = 0;
                    string ghiChu = "";

                    // Kiểm tra trong danh sách tạm thời trước
                    if (_tempSelectedProducts.ContainsKey(item.ProductId))
                    {
                        var tempData = _tempSelectedProducts[item.ProductId];
                        isChecked = tempData.IsChecked;
                        soTienGiam = tempData.SoTienGiam;
                        soLuongToiThieu = tempData.SoLuongToiThieu;
                        ghiChu = tempData.GhiChu;
                    }
                    // Nếu không có trong danh sách tạm, kiểm tra trong promotion hiện tại
                    else if (_existingPromotionProducts.ContainsKey(item.ProductId))
                    {
                        isChecked = true;
                        var existingProduct = _existingPromotionProducts[item.ProductId];
                        soTienGiam = existingProduct.DiscountAmount;
                        soLuongToiThieu = existingProduct.MinQuantity;
                        ghiChu = existingProduct.Note ?? "";
                    }

                    tempList.Add(new
                    {
                        Chon = isChecked,
                        MaSanPham = item.ProductId,
                        TenSanPham = item.ProductName,
                        DonVi = item.Unit,
                        GiaChuan = item.StandardPrice,
                        SoTienGiam = soTienGiam,
                        SoLuongToiThieu = soLuongToiThieu,
                        GhiChu = ghiChu
                    });
                }

                // Kiểm tra kết quả tìm kiếm
                if (tempList.Count == 0)
                {
                    MessageBox.Show($"Không tìm thấy sản phẩm nào với từ khóa '{searchKeyword}'",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvDuLieu.DataSource = null;
                    return;
                }

                // Sắp xếp: Sản phẩm được tích lên đầu
                var sortedList = tempList.OrderByDescending(x => x.Chon).ToList();

                // Thêm vào DataTable - Hiển thị tất cả kết quả tìm kiếm (không phân trang)
                foreach (var item in sortedList)
                {
                    dt.Rows.Add(item.Chon, item.MaSanPham, item.TenSanPham, item.DonVi,
                        item.GiaChuan, item.SoTienGiam == 0 ? DBNull.Value : (object)item.SoTienGiam,
                        item.SoLuongToiThieu == 0 ? DBNull.Value : (object)item.SoLuongToiThieu, item.GhiChu);
                }
            }

            _originalDataTable = dt.Copy();

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thiết lập cột readonly
            foreach (DataGridViewColumn col in dgvDuLieu.Columns)
            {
                if (col.Name == "Chon" || col.Name == "SoTienGiam" || col.Name == "SoLuongToiThieu" || col.Name == "GhiChu")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }

            // Đặt header text
            dgvDuLieu.Columns["Chon"].HeaderText = "Chọn";
            dgvDuLieu.Columns["Chon"].Width = 50;
            dgvDuLieu.Columns["MaSanPham"].HeaderText = "Mã SP";
            dgvDuLieu.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dgvDuLieu.Columns["DonVi"].HeaderText = "Đơn Vị";
            dgvDuLieu.Columns["GiaChuan"].HeaderText = "Giá Chuẩn";
            dgvDuLieu.Columns["SoTienGiam"].HeaderText = "Số Tiền Giảm";
            dgvDuLieu.Columns["SoLuongToiThieu"].HeaderText = "SL Tối Thiểu";
            dgvDuLieu.Columns["GhiChu"].HeaderText = "Ghi Chú";

            // Format số tiền
            dgvDuLieu.Columns["GiaChuan"].DefaultCellStyle.Format = "N0";
            dgvDuLieu.Columns["SoTienGiam"].DefaultCellStyle.Format = "N0";

            // Giao diện đẹp
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

            // Ẩn các nút phân trang khi đang tìm kiếm
            btnTrangTruocGGSP.Enabled = false;
            btnTrangSauGGSP.Enabled = false;
            txtTrangGGSP.Text = $"Tìm thấy {dt.Rows.Count} SP";
        }

        private void LoadExistingPromotionProducts()
        {
            try
            {
                var result = _promotionProductService.GetPromotionProduct(_promotionId, 1, int.MaxValue);
                if (result.Succeeded && result.Data != null)
                {
                    _existingPromotionProducts.Clear();
                    foreach (var item in result.Data)
                    {
                        _existingPromotionProducts[item.ProductId] = item;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu promotion: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }

            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Chon")
            {
                dgvDuLieu.EndEdit();

                // Lưu trạng thái khi checkbox thay đổi
                SaveCurrentPageState();
            }
        }

        // Lưu trạng thái trang hiện tại
        private void SaveCurrentPageState()
        {
            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                string productId = row.Cells["MaSanPham"].Value.ToString();
                var chonCell = row.Cells["Chon"].Value;
                bool isChecked = chonCell != null && (bool)chonCell;

                if (isChecked)
                {
                    // Lưu trạng thái sản phẩm được tích
                    var soTienGiamCell = row.Cells["SoTienGiam"].Value;
                    var soLuongToiThieuCell = row.Cells["SoLuongToiThieu"].Value;

                    _tempSelectedProducts[productId] = new TempProductData
                    {
                        IsChecked = true,
                        SoTienGiam = soTienGiamCell != null && !string.IsNullOrEmpty(soTienGiamCell.ToString())
                            ? Convert.ToDecimal(soTienGiamCell) : 0,
                        SoLuongToiThieu = soLuongToiThieuCell != null && !string.IsNullOrEmpty(soLuongToiThieuCell.ToString())
                            ? Convert.ToInt32(soLuongToiThieuCell) : 0,
                        GhiChu = row.Cells["GhiChu"].Value?.ToString() ?? ""
                    };
                }
                else
                {
                    // Xóa khỏi danh sách tạm nếu bỏ tích
                    if (_tempSelectedProducts.ContainsKey(productId))
                    {
                        _tempSelectedProducts.Remove(productId);
                    }
                }
            }
        }

        private void LoadDataGGSP(string promotionId, int pageNumber = 1, int pageSize = 10)
        {
            // Lưu trạng thái trang hiện tại trước khi chuyển trang
            if (dgvDuLieu.DataSource != null)
            {
                SaveCurrentPageState();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Chon", typeof(bool));
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("DonVi");
            dt.Columns.Add("GiaChuan");
            dt.Columns.Add("SoTienGiam", typeof(decimal));
            dt.Columns.Add("SoLuongToiThieu", typeof(int));
            dt.Columns.Add("GhiChu");

            using (var childContainer = _container.CreateChildContainer())
            {
                var productService = childContainer.Resolve<IProductService>();

                // Lấy TẤT CẢ sản phẩm để sắp xếp
                var allProductsList = productService.GetProduct(1, int.MaxValue);

                if (!allProductsList.Succeeded || allProductsList.Data == null)
                {
                    dgvDuLieu.DataSource = null;
                    return;
                }

                // Tạo list tạm để sắp xếp TẤT CẢ sản phẩm
                var tempList = new List<dynamic>();

                foreach (var item in allProductsList.Data)
                {
                    bool isChecked = false;
                    decimal soTienGiam = 0;
                    int soLuongToiThieu = 0;
                    string ghiChu = "";

                    // Kiểm tra trong danh sách tạm thời trước
                    if (_tempSelectedProducts.ContainsKey(item.ProductId))
                    {
                        var tempData = _tempSelectedProducts[item.ProductId];
                        isChecked = tempData.IsChecked;
                        soTienGiam = tempData.SoTienGiam;
                        soLuongToiThieu = tempData.SoLuongToiThieu;
                        ghiChu = tempData.GhiChu;
                    }
                    // Nếu không có trong danh sách tạm, kiểm tra trong promotion hiện tại
                    else if (_existingPromotionProducts.ContainsKey(item.ProductId))
                    {
                        isChecked = true;
                        var existingProduct = _existingPromotionProducts[item.ProductId];
                        soTienGiam = existingProduct.DiscountAmount;
                        soLuongToiThieu = existingProduct.MinQuantity;
                        ghiChu = existingProduct.Note ?? "";
                    }

                    tempList.Add(new
                    {
                        Chon = isChecked,
                        MaSanPham = item.ProductId,
                        TenSanPham = item.ProductName,
                        DonVi = item.Unit,
                        GiaChuan = item.StandardPrice,
                        SoTienGiam = soTienGiam,
                        SoLuongToiThieu = soLuongToiThieu,
                        GhiChu = ghiChu
                    });
                }

                // Sắp xếp: Sản phẩm được tích lên đầu, sau đó sản phẩm chưa tích
                var sortedList = tempList.OrderByDescending(x => x.Chon).ToList();

                // Tính toán phân trang
                int totalProducts = sortedList.Count;
                _totalPageGGSPP = (long)Math.Ceiling((double)totalProducts / pageSize);

                // Lấy sản phẩm của trang hiện tại
                var pagedList = sortedList
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                // Thêm vào DataTable theo thứ tự đã sắp xếp và phân trang
                foreach (var item in pagedList)
                {
                    dt.Rows.Add(item.Chon, item.MaSanPham, item.TenSanPham, item.DonVi,
                        item.GiaChuan, item.SoTienGiam == 0 ? DBNull.Value : (object)item.SoTienGiam,
                        item.SoLuongToiThieu == 0 ? DBNull.Value : (object)item.SoLuongToiThieu, item.GhiChu);
                }
            }

            _originalDataTable = dt.Copy();

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thiết lập cột readonly
            foreach (DataGridViewColumn col in dgvDuLieu.Columns)
            {
                if (col.Name == "Chon" || col.Name == "SoTienGiam" || col.Name == "SoLuongToiThieu" || col.Name == "GhiChu")
                {
                    col.ReadOnly = false;
                }
                else
                {
                    col.ReadOnly = true;
                }
            }

            // Đặt header text
            dgvDuLieu.Columns["Chon"].HeaderText = "Chọn";
            dgvDuLieu.Columns["Chon"].Width = 50;
            dgvDuLieu.Columns["MaSanPham"].HeaderText = "Mã SP";
            dgvDuLieu.Columns["TenSanPham"].HeaderText = "Tên Sản Phẩm";
            dgvDuLieu.Columns["DonVi"].HeaderText = "Đơn Vị";
            dgvDuLieu.Columns["GiaChuan"].HeaderText = "Giá Chuẩn";
            dgvDuLieu.Columns["SoTienGiam"].HeaderText = "Số Tiền Giảm";
            dgvDuLieu.Columns["SoLuongToiThieu"].HeaderText = "SL Tối Thiểu";
            dgvDuLieu.Columns["GhiChu"].HeaderText = "Ghi Chú";

            // Format số tiền
            dgvDuLieu.Columns["GiaChuan"].DefaultCellStyle.Format = "N0";
            dgvDuLieu.Columns["SoTienGiam"].DefaultCellStyle.Format = "N0";

            // Giao diện đẹp
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

            btnTrangTruocGGSP.Enabled = pageNumber > 1;
            btnTrangSauGGSP.Enabled = pageNumber < _totalPageGGSPP;
        }

        private void dgvDuLieu_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Chỉ validate cho cột SoTienGiam và SoLuongToiThieu
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "SoTienGiam" ||
                dgvDuLieu.Columns[e.ColumnIndex].Name == "SoLuongToiThieu")
            {
                string input = e.FormattedValue.ToString().Trim();

                // Cho phép để trống
                if (string.IsNullOrEmpty(input))
                {
                    return;
                }

                // Kiểm tra chỉ được nhập số
                if (dgvDuLieu.Columns[e.ColumnIndex].Name == "SoTienGiam")
                {
                    decimal value;
                    if (!decimal.TryParse(input, out value) || value < 0)
                    {
                        MessageBox.Show("Vui lòng chỉ nhập số cho Số Tiền Giảm!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
                else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "SoLuongToiThieu")
                {
                    int value;
                    if (!int.TryParse(input, out value) || value < 0)
                    {
                        MessageBox.Show("Vui lòng chỉ nhập số nguyên cho Số Lượng Tối Thiểu!", "Cảnh báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        e.Cancel = true;
                    }
                }
            }
        }

        private void dgvDuLieu_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Lấy tên cột hiện tại
            string columnName = dgvDuLieu.Columns[dgvDuLieu.CurrentCell.ColumnIndex].Name;

            TextBox tb = e.Control as TextBox;
            if (tb != null)
            {
                // Xóa sự kiện cũ để tránh trùng lặp
                tb.KeyPress -= Tb_KeyPress;

                // CHỈ áp dụng validate số cho 2 cột này
                if (columnName == "SoTienGiam" || columnName == "SoLuongToiThieu")
                {
                    // Thêm sự kiện mới
                    tb.KeyPress += Tb_KeyPress;
                }
            }
        }

        private void Tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số, backspace, delete
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnTrangSauGGSP_Click(object sender, EventArgs e)
        {
            // Nếu đang tìm kiếm thì không cho chuyển trang
            if (!string.IsNullOrEmpty(_searchKeyword))
            {
                return;
            }

            int number = Convert.ToInt32(txtTrangGGSP.Text);
            btnTrangTruocGGSP.Enabled = true;
            if (number < _totalPageGGSPP)
            {
                var pageNumber = ++number;
                txtTrangGGSP.Text = pageNumber.ToString();
                _currentPage = pageNumber;
                LoadDataGGSP(_promotionId, pageNumber);
            }
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            // Lưu trạng thái trang hiện tại
            SaveCurrentPageState();

            // Lấy tất cả sản phẩm từ danh sách tạm thời
            List<Domain.DTO.PromotionProductDto> productsToAdd = new List<Domain.DTO.PromotionProductDto>();
            List<string> productsToRemove = new List<string>();

            // Lấy tất cả sản phẩm trong tất cả các trang
            using (var childContainer = _container.CreateChildContainer())
            {
                var productService = childContainer.Resolve<IProductService>();
                var allProducts = productService.GetProduct(1, int.MaxValue);

                if (allProducts.Succeeded && allProducts.Data != null)
                {
                    foreach (var product in allProducts.Data)
                    {
                        string productId = product.ProductId;
                        bool wasExisting = _existingPromotionProducts.ContainsKey(productId);
                        bool isInTemp = _tempSelectedProducts.ContainsKey(productId);

                        if (isInTemp && _tempSelectedProducts[productId].IsChecked)
                        {
                            // Sản phẩm được chọn
                            var tempData = _tempSelectedProducts[productId];

                            if (tempData.SoTienGiam <= 0)
                            {
                                MessageBox.Show($"Vui lòng nhập số tiền giảm cho sản phẩm {product.ProductName}!",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            if (tempData.SoLuongToiThieu <= 0)
                            {
                                MessageBox.Show($"Vui lòng nhập số lượng tối thiểu cho sản phẩm {product.ProductName}!",
                                    "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                            // Nếu là sản phẩm mới hoặc có thay đổi
                            if (!wasExisting ||
                                _existingPromotionProducts[productId].DiscountAmount != tempData.SoTienGiam ||
                                _existingPromotionProducts[productId].MinQuantity != tempData.SoLuongToiThieu ||
                                _existingPromotionProducts[productId].Note != tempData.GhiChu)
                            {
                                productsToAdd.Add(new Domain.DTO.PromotionProductDto
                                {
                                    PromotionId = _promotionId,
                                    ProductId = productId,
                                    ProductName = product.ProductName,
                                    DiscountAmount = tempData.SoTienGiam,
                                    MinQuantity = tempData.SoLuongToiThieu,
                                    Note = tempData.GhiChu
                                });
                            }
                        }
                        else if (wasExisting)
                        {
                            // Sản phẩm đã có trong promotion nhưng không có trong temp -> xóa
                            productsToRemove.Add(productId);
                        }
                    }
                }
            }

            if (productsToAdd.Count == 0 && productsToRemove.Count == 0)
            {
                MessageBox.Show("Không có thay đổi nào để lưu!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Thực hiện thêm/xóa/cập nhật
            try
            {
                int successCount = 0;
                int failCount = 0;
                int deleteCount = 0;
                StringBuilder errorMsg = new StringBuilder();

                // Xóa các sản phẩm bị bỏ chọn
                foreach (var productId in productsToRemove)
                {
                    var existingProduct = _existingPromotionProducts[productId];
                    var result = _promotionProductService.RemovePromotionProduct(existingProduct.Id);
                    if (result.Succeeded)
                    {
                        deleteCount++;
                    }
                    else
                    {
                        failCount++;
                        errorMsg.AppendLine($"- Xóa sản phẩm {productId} thất bại");
                    }
                }

                // Thêm hoặc cập nhật sản phẩm
                foreach (var product in productsToAdd)
                {
                    bool isExisting = _existingPromotionProducts.ContainsKey(product.ProductId);

                    Result<bool> result;
                    if (isExisting)
                    {
                        product.Id = _existingPromotionProducts[product.ProductId].Id;
                        result = _promotionProductService.UpdatePromotionProduct(product);
                    }
                    else
                    {
                        result = _promotionProductService.CreatePromotionProduct(product);
                    }

                    if (result.Succeeded)
                    {
                        successCount++;
                    }
                    else
                    {
                        failCount++;
                        errorMsg.AppendLine($"- {product.ProductName}");
                    }
                }

                string message = "";
                if (successCount > 0)
                    message += $"Lưu thành công {successCount} sản phẩm\n";
                if (deleteCount > 0)
                    message += $"Xóa thành công {deleteCount} sản phẩm\n";
                if (failCount > 0)
                    message += $"Thất bại {failCount} thao tác:\n{errorMsg}";

                MessageBox.Show(message.Trim(), "Kết quả", MessageBoxButtons.OK,
                    failCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                if (successCount > 0 || deleteCount > 0)
                {
                    // Xóa danh sách tạm thời
                    _tempSelectedProducts.Clear();

                    // Reload lại danh sách sản phẩm đã có trong promotion
                    LoadExistingPromotionProducts();

                    // Quay về trang 1
                    _currentPage = 1;
                    txtTrangGGSP.Text = "1";
                    LoadDataGGSP(_promotionId, 1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmHienThi_GiamGiaSanPham_Load(object sender, EventArgs e)
        {
            dgvDuLieu.CurrentCellDirtyStateChanged += (s, ev) =>
            {
                if (dgvDuLieu.IsCurrentCellDirty)
                {
                    dgvDuLieu.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };

            // Thêm sự kiện validate
            dgvDuLieu.CellValidating += dgvDuLieu_CellValidating;
            dgvDuLieu.EditingControlShowing += dgvDuLieu_EditingControlShowing;
        }

        private void btnTrangTruocGGSP_Click(object sender, EventArgs e)
        {
            // Nếu đang tìm kiếm thì không cho chuyển trang
            if (!string.IsNullOrEmpty(_searchKeyword))
            {
                return;
            }

            int number = Convert.ToInt32(txtTrangGGSP.Text);
            if (number > 1)
            {
                var pageNumber = --number;
                txtTrangGGSP.Text = pageNumber.ToString();
                _currentPage = pageNumber;
                LoadDataGGSP(_promotionId, pageNumber);
            }
            else
            {
                btnTrangTruocGGSP.Enabled = false;
            }
        }
    }
}