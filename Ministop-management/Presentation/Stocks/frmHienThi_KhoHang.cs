using CrystalDecisions.ReportAppServer;
using Guna.UI2.WinForms;
using Presentation.Stocks.Dialogs;
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
using System.Web.Services.Description;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Presentation
{
    public partial class frmHienThi_KhoHang : Form
    {
        private readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        private readonly IStockDetailService _stockDetailService;
        private readonly IStockHistoryService _stockHistoryService;
        private readonly IStockImportService _stockImportService;
        private readonly IStockImportDetailSerivce _stockImportDetailSerivce;
        private readonly IStockExportService _stockExportService;
        private readonly IStockExportDetailService _stockExportDetailService;
        private readonly IStockCheckService _stockCheckService;
        private readonly IStockCheckDetailService _stockCheckDetailService;
        private long _totalPageStockDetail;
        private long _totalPageStockImport;
        private long _totalPageStockExport;
        private long _totalPageStockCheck;
        private int _totalCountImport;
        private int _totalCountExport;
        private int _totalCountWarming= 0;

        public frmHienThi_KhoHang
            (IUserSession userSession, IUnityContainer container,
            IStockDetailService stockDetailService, IStockHistoryService stockHistoryService,
            IStockImportService stockImportService, IStockImportDetailSerivce stockImportDetailSerivce,
            IStockExportService stockExportService, IStockExportDetailService stockExportDetailService,
            IStockCheckService stockCheckService, IStockCheckDetailService stockCheckDetailService
            )
        {
            InitializeComponent();
            _userSession = userSession;
            _container = container;
            _stockDetailService = stockDetailService;
            _stockHistoryService = stockHistoryService;
            _stockImportService = stockImportService;
            _stockImportDetailSerivce = stockImportDetailSerivce;
            _stockExportService = stockExportService;
            _stockExportDetailService = stockExportDetailService;
            _stockCheckService = stockCheckService;
            _stockCheckDetailService = stockCheckDetailService;

        }
        private void frmHienThi_KhoHang_Load(object sender, EventArgs e)
        {
            LoadCboCuaHang(cboCuaHangKH);
            LoadCboCuaHang(cboCuaHangNH);
            LoadCboCuaHang(cboCuaHangXH);
            if (_userSession.Role == "Nhân viên")
            {
                tabControlKH.TabPages.Remove(tabChiTietKho);
                tabControlKH.TabPages.Remove(tabNhapHang);
                cboCuaHangKH.SelectedValue = _userSession.IdStore;
                cboCuaHangNH.SelectedValue = _userSession.IdStore;
                cboCuaHangXH.SelectedValue = _userSession.IdStore;
                LoadDataStockDetail(_userSession.IdStore);


            }
            else if (_userSession.Role == "Admin")
            {
                tabControlKH.TabPages.Remove(tabChiTietKho);
                cboCuaHangKH.Enabled = true; 
                cboCuaHangNH .Enabled = true;
                cboCuaHangXH .Enabled = true;
                LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString());
                btnThemKH.Visible = false;
                btnThemNH.Visible = false;
                btnThemXH.Visible = false;
            }
            else if (_userSession.Role == "Quản lý cửa hàng")
            {
                cboCuaHangKH.SelectedValue = _userSession.IdStore;
                cboCuaHangNH.SelectedValue = _userSession.IdStore;
                cboCuaHangXH.SelectedValue = _userSession.IdStore;
                LoadDataStockDetail(_userSession.IdStore);
                LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString());
                btnThemKH.Visible = false;
                btnThemXH.Visible = false;
            }
            LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString());
            LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString());

        }
        private void LoadCboCuaHang(Guna2ComboBox cboCuaHang)
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var storeServices = childContainer.Resolve<IStoreService>();
                var list = storeServices.GetAll();
                if (list.Succeeded == false && list.Data == null) { return; }
                cboCuaHang.DataSource = list.Data;
                cboCuaHang.ValueMember = "StoreID";
                cboCuaHang.DisplayMember = "StoreName";
            }

        }
        private void ApplyGridStyle(Guna2DataGridView dgvDuLieu, string statusNotAllowed ="1", string roleNotAllowed = "")
        {

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvDuLieu.Columns["Edit"] == null && _userSession.Role != roleNotAllowed)
            {

                    DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                    btnEdit.Name = "Edit";
                    btnEdit.HeaderText = "Edit";
                    btnEdit.Text = "Edit";
                    btnEdit.UseColumnTextForButtonValue = true;
                    dgvDuLieu.Columns.Add(btnEdit);
            }
            if (dgvDuLieu.Columns["Delete"] == null && _userSession.Role != "Admin")
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieu.Columns.Add(btnDelete);
            }
            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;


            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieu.RowPostPaint += (s, e) =>
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
            dgvDuLieu.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var grid = (Guna2DataGridView)s;
                var status = grid.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
                bool allowEditDelete = status != statusNotAllowed;
                if (e.RowIndex >= 0 && (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);
                    if (allowEditDelete)
                    {
                        Color backColor = dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                        using (Brush b = new SolidBrush(backColor))
                            e.Graphics.FillRectangle(b, e.CellBounds);

                        string text = dgvDuLieu.Columns[e.ColumnIndex].Name;
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
        }

        #region StockDetail 
        private void ibtnLoadDuLieu_Click(object sender, EventArgs e)
        {
            LoadDataStockDetail(_userSession.IdStore);
        }
        public void LoadDataStockDetail(string storeId, int pageNumber = 1, int pageSize = 20, int quantitywarming= 50)
        {
            _totalCountWarming = 0;
            DataTable dt = new DataTable();
            dt.Columns.Add("MaChiTietKho");
            dt.Columns.Add("SanPham");
            dt.Columns.Add("SoLuong");
            dt.Columns.Add("GiaBan");
            dt.Columns.Add("LanCuoiCapNhap");
            using (var childContaner = _container.CreateChildContainer())
            {
                var stockDetailService = childContaner.Resolve<IStockDetailService>();
                var list = stockDetailService.GetStockDetails(storeId, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPageStockDetail = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.StockDetailId, item.ProductName, item.Quantity, item.Price, item.LastUpdate.ToShortDateString());
                    if(item.Quantity< quantitywarming)
                    {
                        _totalCountWarming++;
                    }
                }
            }
            lblSoLanCanhBao.Text = _totalCountWarming.ToString();
            dgvDuLieuCT.DataSource = dt;
            dgvDuLieuCT.AllowUserToAddRows = false;
            dgvDuLieuCT.ReadOnly = true;
            dgvDuLieuCT.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDuLieuCT.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieuCT.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieuCT.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieuCT.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieuCT.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieuCT.RowTemplate.Height = 40;
            dgvDuLieuCT.RowPostPaint += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                var grid = (Guna2DataGridView)s;
                var row = grid.Rows[e.RowIndex];
                long quantity =Convert.ToInt64(row.Cells["SoLuong"].Value.ToString());
                if(quantity < quantitywarming)
                {
                    //using (Pen p = new Pen(Color.Red, 4))
                    //{
                    //    int x = e.RowBounds.Left + 1;
                    //    int y = e.RowBounds.Top + 1;
                    //    int y2 = e.RowBounds.Bottom - 1;
                    //    e.Graphics.DrawLine(p,x,y,x,y2);
                    //}
         
                    row.DefaultCellStyle.BackColor = Color.IndianRed;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
            };
            GetCountExport();
            GetCountImport();
            btnTrangTruocCT.Enabled = pageNumber > 1;
            btnTrangSauCT.Enabled = pageNumber <= _totalPageStockDetail;
        }
        private void btnTrangSauCT_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCT.Text);
            btnTrangTruocCT.Enabled = true;
            if (number <= _totalPageStockDetail)
            {
                var pageNumber = ++number;
                txtSoTrangCT.Text = pageNumber.ToString();
                LoadDataStockDetail(_userSession.IdStore, pageNumber);
            }
        }
        private void GetCountExport()
        {
            using(var childContainer  = _container.CreateChildContainer())
            {
                var date = DateTime.UtcNow.ToLocalTime();
                var exportService = _container.Resolve<IStockExportDetailService>();
                var count = exportService.GetCount(_userSession.IdStore, date);
                lblSoLanXuat.Text = count.Data.ToString();
            }
        }
        private void GetCountImport()
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var date = DateTime.UtcNow.ToLocalTime();
                var importService = _container.Resolve<IStockImportDetailSerivce>();
                var count = importService.GetCount(_userSession.IdStore, date);
                lblSoLanNhap.Text = count.Data.ToString();
            }
        }
        private void btnTrangTruocCT_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCT.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangCT.Text = pageNumber.ToString();
                LoadDataStockDetail(_userSession.IdStore, pageNumber);

            }
            else
            {
                btnTrangTruocCT.Enabled = false;
            }
        }

        private void dgvDuLieuCT_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDuLieuCT.CurrentCell == null || dgvDuLieuCT.Rows.Count == 0) return;
            int row = dgvDuLieuCT.CurrentCell.RowIndex;
            string stockDetailID = dgvDuLieuCT.Rows[row].Cells["MaChiTietKho"].Value.ToString();
            var frmChucNang = _container.Resolve<frmHienThi_LichSuKhoHang>(new ParameterOverride("stockDetailID", stockDetailID));
            frmChucNang.ShowDialog();
        }
        private void ibtnSoLanNhap_Click(object sender, EventArgs e)
        {
            var frmHienThi = _container.Resolve<frmHienThi_ThongBao>(new ParameterOverride("storeID", _userSession.IdStore), new ParameterOverride("date", DateTime.UtcNow.ToLocalTime()), new ParameterOverride("type", "IMPORT"));
            frmHienThi.ShowDialog();
        }

        private void ibtnSoLanXuat_Click(object sender, EventArgs e)
        {
            var frmHienThi = _container.Resolve<frmHienThi_ThongBao>(new ParameterOverride("storeID", _userSession.IdStore), new ParameterOverride("date", DateTime.UtcNow.ToLocalTime()), new ParameterOverride("type", "EXPORT"));
            frmHienThi.ShowDialog();
        }
        #endregion

        #region StockImport
        public void LoadDataStockImport(string storeId, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuNhap");
            dt.Columns.Add("MaNhaCungCap");
            dt.Columns.Add("NhaCungCap");
            dt.Columns.Add("NguoiLapPhieu");
            dt.Columns.Add("TrangThai");
            dt.Columns.Add("NgayNhap");
            using (var childContaner = _container.CreateChildContainer())
            {
                var stockDetailService = childContaner.Resolve<IStockImportService>();
                var list = stockDetailService.GetStockImport(storeId, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPageStockImport = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.ImportID, item.SupplierId, item.SupplierName, item.EmployeeName, item.Status, item.ImportDate.ToShortDateString());
                }
            }
            dgvDuLieuNH.DataSource = dt;
            dgvDuLieuNH.AllowUserToAddRows = false;
            dgvDuLieuNH.ReadOnly = true;
            dgvDuLieuNH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyGridStyle(dgvDuLieuNH,"4");
            btnTrangTruocNH.Enabled = pageNumber > 1;
            btnTrangSauNH.Enabled = pageNumber <= _totalPageStockImport;
        }
        private void dgvDuLieuNH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string importID = dgvDuLieuNH.Rows[e.RowIndex].Cells["MaPhieuNhap"].Value.ToString();
            var status = dgvDuLieuNH.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            var allowAction = status == "4";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrangNH.Text);
            if (dgvDuLieuNH.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCP = _container.Resolve<frmChucNang_NhapKho>(new ParameterOverride("ImportID", importID));
                frmChucNangCP.dataChanged += (s, ev) =>
                {

                    LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString(), pageNumber);
                };
                frmChucNangCP.ShowDialog();

            }
            else if (dgvDuLieuNH.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu {importID}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var check = _stockImportDetailSerivce.Any(importID);
                    if (check.Data)
                    {
                        DialogResult resultCon = MessageBox.Show($"Phiếu {importID} hiện đang còn dữ liệu.\n Nếu bạn xác nhận xoá, hệ thống sẽ xóa các dữ liệu chi tiết bên trong phiếu ! Xin vui lòng cân nhăc trước khi ấn nút xác nhận.",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (resultCon != DialogResult.Yes) return;
                        var isSucceeded = _stockImportDetailSerivce.RemoveRangeStockImportDetailByImportID(importID);
                        if (!isSucceeded.Succeeded) { MessageBox.Show("Việc xóa các phiếu chi tiết đã xảy ra sự cố !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        _stockImportService.RemoveStockImport(importID);
                        MessageBox.Show("Xóa thành công!");
                        LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString(), pageNumber);
                        return;
                    }
                    _stockImportService.RemoveStockImport(importID);
                    MessageBox.Show("Xóa thành công!");
                    LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString(), pageNumber); // tải lại dữ liệu
                }
            }
        }


        private void dgvDuLieuNH_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDuLieuNH.CurrentCell == null || dgvDuLieuNH.Rows.Count == 0) return;
            var row = dgvDuLieuNH.CurrentCell.RowIndex;
            string importID = dgvDuLieuNH.Rows[row].Cells["MaPhieuNhap"].Value.ToString();
            var status = dgvDuLieuNH.Rows[row].Cells["TrangThai"].Value.ToString();
            string supllierID = dgvDuLieuNH.Rows[row].Cells["MaNhaCungCap"].Value.ToString();
            var frmHienThi = _container.Resolve<frmHienThi_ChiTietNhapHang>(new ParameterOverride("ImportID", importID), new ParameterOverride("Status", status), new ParameterOverride("supplierID", supllierID));
            frmHienThi.ShowDialog();
        }
        private void btnThemNH_Click(object sender, EventArgs e)
        {
            var frmChucNangCP = _container.Resolve<frmChucNang_NhapKho>();
            frmChucNangCP.dataChanged += (s, ev) =>
            {

                LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString());
            };
            frmChucNangCP.ShowDialog();
        }

        private void btnTrangTruocNH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangNH.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangNH.Text = pageNumber.ToString();
                LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString(), pageNumber);

            }
            else
            {
                btnTrangTruocNH.Enabled = false;
            }
        }

        private void btnTrangSauNH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangNH.Text);
            btnTrangTruocNH.Enabled = true;
            if (number <= _totalPageStockImport)
            {
                var pageNumber = ++number;
                txtSoTrangNH.Text = pageNumber.ToString();
                LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString(), pageNumber);
            }
        }

        private void cboCuaHangNH_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDataStockImport(cboCuaHangNH.SelectedValue.ToString());
        }
        #endregion

        #region StockExport 
        public void LoadDataStockExport(string storeId, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuXuat");
            dt.Columns.Add("NguoiLapPhieu");
            dt.Columns.Add("LoaiXuat");
            dt.Columns.Add("TrangThai");
            dt.Columns.Add("NgayXuat");
            dt.Columns.Add("LyDo");
            using (var childContaner = _container.CreateChildContainer())
            {
                var stockDetailService = childContaner.Resolve<IStockExportService>();
                var list = stockDetailService.GetStockExport(storeId, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPageStockExport = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.ExportId, item.EmployeeName, item.TypeExport, item.Status, item.ExportDate.ToShortDateString(), item.Reason);
                }
            }
            dgvDuLieuXH.DataSource = dt;
            dgvDuLieuXH.AllowUserToAddRows = false;
            dgvDuLieuXH.ReadOnly = true;
            dgvDuLieuXH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyGridStyle(dgvDuLieuXH,"1","Admin");
            btnTrangTruocXH.Enabled = pageNumber > 1;
            btnTrangSauXH.Enabled = pageNumber <= _totalPageStockExport;
        }
        private void dgvDuLieuXH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string exportID = dgvDuLieuXH.Rows[e.RowIndex].Cells["MaPhieuXuat"].Value.ToString();
            var status = dgvDuLieuXH.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            var allowAction = status == "1";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrangNH.Text);
            if (dgvDuLieuXH.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNang = _container.Resolve<frmChucNang_XuatKho>(new ParameterOverride("ExportID", exportID));
                frmChucNang.dataChanged += (s, ev) =>
                {
                    LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString(), pageNumber);
                };
                frmChucNang.ShowDialog();

            }
            else if (dgvDuLieuXH.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu xuat {exportID}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    var check = _stockExportDetailService.Any(exportID);
                    if (check.Data)
                    {
                        DialogResult resultCon = MessageBox.Show($"Phiếu {exportID} hiện đang còn dữ liệu.\n Nếu bạn xác nhận xoá, hệ thống sẽ xóa các dữ liệu chi tiết bên trong phiếu ! Xin vui lòng cân nhăc trước khi ấn nút xác nhận.",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (resultCon != DialogResult.Yes) return;
                        var isSucceeded = _stockExportDetailService.RemoveRangeStockExportDetailByExportID(exportID);
                        if (!isSucceeded.Succeeded) { MessageBox.Show("Việc xóa các phiếu chi tiết đã xảy ra sự cố !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        _stockExportService.RemoveStockExport(exportID);
                        MessageBox.Show("Xóa thành công!");
                        LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString(), pageNumber); // tải lại dữ liệu
                        return;
                    }
                    _stockExportService.RemoveStockExport(exportID);
                    MessageBox.Show("Xóa thành công!");
                    LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString(), pageNumber); // tải lại dữ liệu


                }
            }
        }

        private void dgvDuLieuXH_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDuLieuXH.CurrentCell == null || dgvDuLieuXH.Rows.Count == 0) return;
            var row = dgvDuLieuXH.CurrentCell.RowIndex;
            string exportID = dgvDuLieuXH.Rows[row].Cells["MaPhieuXuat"].Value.ToString();
            var status = dgvDuLieuXH.Rows[row].Cells["TrangThai"].Value.ToString();
            var frmHienThi = _container.Resolve<frmHienThi_ChiTietXuatHang>(new ParameterOverride("ExportID", exportID),new  ParameterOverride("Status", status));
            frmHienThi.ShowDialog();
        }
        private void btnThemXH_Click(object sender, EventArgs e)
        {
            var frmChucNang = _container.Resolve<frmChucNang_XuatKho>();
            frmChucNang.dataChanged += (s, ev) =>
            {

                LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString());
            };
            frmChucNang.ShowDialog();
        }

        private void btnTrangTruocXH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangXH.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangXH.Text = pageNumber.ToString();
                LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString(), pageNumber);

            }
            else
            {
                btnTrangTruocXH.Enabled = false;
            }
        }

        private void btnTrangSauXH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangXH.Text);
            btnTrangTruocXH.Enabled = true;
            if (number <= _totalPageStockImport)
            {
                var pageNumber = ++number;
                txtSoTrangXH.Text = pageNumber.ToString();
                LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString(), pageNumber);
            }
        }
        private void cboCuaHangXH_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDataStockExport(cboCuaHangXH.SelectedValue.ToString());
        }
        #endregion

        #region StockCheck
        public void LoadDataStockCheck(string storeId, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuKiem");
            dt.Columns.Add("NguoiLapPhieu");
            dt.Columns.Add("TrangThai");
            dt.Columns.Add("NgayXuat");

            using (var childContaner = _container.CreateChildContainer())
            {
                var stockDetailService = childContaner.Resolve<IStockCheckService>();
                var list = stockDetailService.GetStockCheck(storeId, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPageStockCheck = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.CheckId, item.EmployeeName, item.Status, item.CheckDate.ToShortDateString());
                }
            }
            dgvDuLieuKH.DataSource = dt;
            dgvDuLieuKH.AllowUserToAddRows = false;
            dgvDuLieuKH.ReadOnly = true;
            dgvDuLieuKH.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyGridStyle(dgvDuLieuKH,"1","Admin");
            btnTrangTruocKH.Enabled = pageNumber > 1;
            btnTrangSauKH.Enabled = pageNumber <= _totalPageStockCheck;
        }
        private void dgvDuLieuKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string CheckID = dgvDuLieuKH.Rows[e.RowIndex].Cells["MaPhieuKiem"].Value.ToString();
            var status = dgvDuLieuKH.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            var allowAction = status == "1";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrangNH.Text);
            if (dgvDuLieuKH.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNang = _container.Resolve<frmChucNang_KiemKho>(new ParameterOverride("checkID", CheckID));
                frmChucNang.dataChanged += (s, ev) =>
                {

                    LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString(), pageNumber);
                };
                frmChucNang.ShowDialog();

            }
            else if (dgvDuLieuKH.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu kiem {CheckID}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var check = _stockCheckDetailService.Any(CheckID);
                    if (check.Data)
                    {
                        DialogResult resultCon = MessageBox.Show($"Phiếu {CheckID} hiện đang còn dữ liệu.\n Nếu bạn xác nhận xoá, hệ thống sẽ xóa các dữ liệu chi tiết bên trong phiếu ! Xin vui lòng cân nhăc trước khi ấn nút xác nhận.",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (resultCon != DialogResult.Yes) return;
                        var isSucceeded = _stockCheckDetailService.RemoveRangeStockCheckDetailByCheckID(CheckID);
                        if (!isSucceeded.Succeeded) { MessageBox.Show("Việc xóa các phiếu chi tiết đã xảy ra sự cố !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        _stockCheckService.RemoveStockCheck(CheckID);
                        MessageBox.Show("Xóa thành công!");
                        LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString(), pageNumber);// tải lại dữ liệu
                        return;
                    }
                    _stockCheckService.RemoveStockCheck(CheckID);
                    MessageBox.Show("Xóa thành công!");
                    LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString(), pageNumber);// tải lại dữ liệu

                }
            }
        }

        private void dgvDuLieuKH_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDuLieuKH.CurrentCell == null || dgvDuLieuKH.Rows.Count == 0) return;
            var row = dgvDuLieuKH.CurrentCell.RowIndex;
            string CheckID = dgvDuLieuKH.Rows[row].Cells["MaPhieuKiem"].Value.ToString();
            string status = dgvDuLieuKH.Rows[row].Cells["TrangThai"].Value.ToString();
            var frmHienThi = _container.Resolve<frmHienThiChiTietKiemHang>(new ParameterOverride("checkID", CheckID), new ParameterOverride("Status",status));
            frmHienThi.ShowDialog();
        }

        private void btnThemKH_Click(object sender, EventArgs e)
        {
            var frmChucNang = _container.Resolve<frmChucNang_KiemKho>();
            frmChucNang.dataChanged += (s, ev) =>
            {

                LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString());
            };
            frmChucNang.ShowDialog();
        }

        private void btnTrangSauKH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangKH.Text);
            btnTrangTruocKH.Enabled = true;
            if (number <= _totalPageStockCheck)
            {
                var pageNumber = ++number;
                txtSoTrangKH.Text = pageNumber.ToString();
                LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString(), pageNumber);
            }
        }

        private void btnTrangTruocKH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangKH.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangKH.Text = pageNumber.ToString();
                LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString(), pageNumber);

            }
            else
            {
                btnTrangTruocKH.Enabled = false;
            }
        }

        private void cboCuaHangKH_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDataStockCheck(cboCuaHangKH.SelectedValue.ToString());
        }












        #endregion

      
    }
}
