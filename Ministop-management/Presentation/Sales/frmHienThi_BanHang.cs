using Domain.DTO;
using Guna.UI2.WinForms;
using Presentation.CrystalReport.FormShow;
using Presentation.Sales.Dialogs;
using Services.Interfaces;
using Shared.Security;
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
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_BanHang : Form
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPageHD;
        string _invoiceID = string.Empty;
        public frmHienThi_BanHang(IInvoiceService invoiceService, IInvoiceDetailService invoiceDetailService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _invoiceService = invoiceService;
            _invoiceDetailService = invoiceDetailService;
            _container = container;
            _userSession = userSession;
            LoadDataHD(_invoiceID);
            
        }
       
        #region Invoice
        private void LoadDataHD(string storeID, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHoaDon");
            dt.Columns.Add("NgayLap");
            dt.Columns.Add("TongTien");
            dt.Columns.Add("TrangThai");

            using (var childContainer = _container.CreateChildContainer())
            {
                var invoiceService = childContainer.Resolve<IInvoiceService>();
                var list = invoiceService.GetInvoice(_userSession.IdStore, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPageHD = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.InvoiceId, item.InvoiceDate, item.FinalAmount, item.status);
                }
            }
            dgvDuLieuHD.DataSource = dt;
            dgvDuLieuHD.AllowUserToAddRows = false;
            dgvDuLieuHD.ReadOnly = true;
            dgvDuLieuHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvDuLieuHD.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieuHD.Columns.Add(btnDelete);
            }
            dgvDuLieuHD.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieuHD.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieuHD.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieuHD.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieuHD.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieuHD.RowTemplate.Height = 40;
            dgvDuLieuHD.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var grid = (Guna2DataGridView)s;
                var status = grid.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
                bool allowEditDelete = status != "1";
                // 👆 chỉ dòng cuối (dòng mới nhất) mới có nút

                if (grid.Columns[e.ColumnIndex].Name == "Delete")
                {
                    e.PaintBackground(e.CellBounds, true);

                    if (allowEditDelete)
                    {
                        // Chỉ vẽ nếu được phép
                        Color backColor = Color.IndianRed;

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
            btnTrangTruocHD.Enabled = pageNumber > 1;
            btnTrangSauHD.Enabled = pageNumber <= _totalPageHD;
        }


        private void dgvHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string invoiceID = dgvDuLieuHD.Rows[e.RowIndex].Cells["MaHoaDon"].Value.ToString();
            var status = dgvDuLieuHD.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
            var allowAction = status == "1";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrangHD.Text);
            if (dgvDuLieuHD.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa hóa đơn {invoiceID}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var check = _invoiceDetailService.Any(invoiceID);
                    if (check.Data)
                    {
                        DialogResult resultCon = MessageBox.Show($"Phiếu hóa đơn {invoiceID} hiện đang còn dữ liệu.\n Nếu bạn xác nhận xoá, hệ thống sẽ xóa các dữ liệu chi tiết bên trong phiếu ! Xin vui lòng cân nhăc trước khi ấn nút xác nhận.",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (resultCon != DialogResult.Yes) return;
                        var isSucceeded = _invoiceDetailService.RemoveRangeInvoiceDetailByInvoiceID(invoiceID);
                        if (!isSucceeded.Succeeded) { MessageBox.Show("Việc xóa các phiếu chi tiết đã xảy ra sự cố !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
                        _invoiceService.RemoveInvoice(invoiceID);
                        MessageBox.Show("Xóa thành công!");
                        LoadDataHD(_userSession.IdStore, pageNumber);
                        return;
                    }
                    _invoiceService.RemoveInvoice(invoiceID);
                    MessageBox.Show("Xóa thành công!");
                    LoadDataHD(_userSession.IdStore, pageNumber); // tải lại dữ liệu
                }
            }
        }

        private void btnTrangTruocHD_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangHD.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangHD.Text = pageNumber.ToString();
                LoadDataHD(_userSession.IdStore, pageNumber);

            }
            else
            {
                btnTrangTruocHD.Enabled = false;
            }
        }

        private void btnTrangSauHD_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangHD.Text);
            btnTrangTruocHD.Enabled = true;
            if (number <= _totalPageHD)
            {
                var pageNumber = ++number;
                txtSoTrangHD.Text = pageNumber.ToString();
                LoadDataHD(_userSession.IdStore, pageNumber);
            }
        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            var invoiceDto = new InvoiceDto()
            {
                StoreId = _userSession.IdStore,
                EmployeeId = _userSession.UserId,
                InvoiceDate = DateTime.UtcNow.ToLocalTime(),
                FinalAmount = 0,
                DiscountTotal = 0,
            };
            Result<string> result = _invoiceService.CreateInvoice(invoiceDto);
            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            LoadDataHD(_userSession.IdStore);
            _invoiceID = result.Data;
            btnHoanTat.Enabled = true;
            MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            HienThiCNCTHD(result.Data);
        }
        private void HienThiCNCTHD(string invoiceID)
        {
            var frmChucNangChiTietHoaDon = _container.Resolve<frmChucNang_ChiTietHoaDon>(new ParameterOverride("invoiceID", invoiceID));
            frmChucNangChiTietHoaDon.dataChanged += (s, e) =>
            {
                LoadDataHD(_userSession.IdStore);
            };
            frmChucNangChiTietHoaDon.ShowDialog();
        }

        private void frmHienThi_BanHang_DoubleClick(object sender, EventArgs e)
        {
            HienThiCNCTHD(_invoiceID);
        }

        private void dgvDuLieuHD_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDuLieuHD.CurrentCell == null || dgvDuLieuHD.Rows.Count == 0) return;
            int row = dgvDuLieuHD.CurrentCell.RowIndex;
            string invoiceID = dgvDuLieuHD.Rows[row].Cells["MaHoaDon"].Value.ToString();
            string status = dgvDuLieuHD.Rows[row].Cells["TrangThai"].Value.ToString();
            var frmChucNang = _container.Resolve<frmHienThi_ChiTietHoaDon>(new ParameterOverride("invoiceID", invoiceID), new ParameterOverride("status",status));
            frmChucNang.ShowDialog();
        }

        private void btnHoanTat_Click(object sender, EventArgs e)
        {
            if (_invoiceID == null) return;
            var isSucceeded = _invoiceService.UpdateInvoice(_invoiceID);
            if (isSucceeded.Succeeded && isSucceeded.Data)
            {
               
                btnHoanTat.Enabled = false;
                LoadDataHD(_userSession.IdStore);
                var frmHienThi = _container.Resolve<frmHienThi_HoaDon>(new ParameterOverride("invoiceID", _invoiceID));
                frmHienThi.ShowDialog();
                _invoiceID = null;
                MessageBox.Show("Chốt hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        #endregion

        //#region ReturnProduct
        //private void LoadDataTD(string storeID, int pageNumber = 1, int pageSize = 20)
        //{
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("MaPhieuTra");
        //    dt.Columns.Add("NgayLap");
        //    dt.Columns.Add("MaHoaDon");
        //    dt.Columns.Add("NgayTraHang");

        //    using (var childContainer = _container.CreateChildContainer())
        //    {
        //        var invoiceService = childContainer.Resolve<IInvoiceService>();
        //        var list = invoiceService.GetInvoice(_userSession.IdStore, pageNumber, pageSize);
        //        if (list.Succeeded == false && list.Data == null) { return; }
        //        _totalPageHD = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
        //        foreach (var item in list.Data)
        //        {
        //            dt.Rows.Add(item.InvoiceId, item.InvoiceDate, item.FinalAmount, item.status);
        //        }
        //    }
        //    dgvDuLieuHD.DataSource = dt;
        //    dgvDuLieuHD.AllowUserToAddRows = false;
        //    dgvDuLieuHD.ReadOnly = true;
        //    dgvDuLieuHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //    if (dgvDuLieuHD.Columns["Delete"] == null)
        //    {
        //        DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
        //        btnDelete.Name = "Delete";
        //        btnDelete.HeaderText = "Delete";
        //        btnDelete.Text = "Delete";
        //        btnDelete.UseColumnTextForButtonValue = true;
        //        dgvDuLieuHD.Columns.Add(btnDelete);
        //    }
        //    dgvDuLieuHD.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
        //    dgvDuLieuHD.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
        //    dgvDuLieuHD.ThemeStyle.HeaderStyle.ForeColor = Color.White;
        //    dgvDuLieuHD.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
        //    dgvDuLieuHD.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
        //    dgvDuLieuHD.RowTemplate.Height = 40;
        //    dgvDuLieuHD.CellPainting += (s, e) =>
        //    {
        //        if (e.RowIndex < 0) return;

        //        var grid = (Guna2DataGridView)s;
        //        var status = grid.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
        //        bool allowEditDelete = status != "1";
        //        // 👆 chỉ dòng cuối (dòng mới nhất) mới có nút

        //        if (grid.Columns[e.ColumnIndex].Name == "Delete")
        //        {
        //            e.PaintBackground(e.CellBounds, true);

        //            if (allowEditDelete)
        //            {
        //                // Chỉ vẽ nếu được phép
        //                Color backColor = Color.IndianRed;

        //                using (Brush b = new SolidBrush(backColor))
        //                    e.Graphics.FillRectangle(b, e.CellBounds);

        //                string text = grid.Columns[e.ColumnIndex].Name;
        //                TextRenderer.DrawText(
        //                    e.Graphics,
        //                    text,
        //                    new Font("Segoe UI", 9, FontStyle.Bold),
        //                    e.CellBounds,
        //                    Color.White,
        //                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
        //                );
        //            }

        //            e.Handled = true;
        //        }
        //    };
        //    btnTrangTruocHD.Enabled = pageNumber > 1;
        //    btnTrangSauHD.Enabled = pageNumber <= _totalPageHD;
        //}


        //private void dgvDuLieuTH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex < 0) return;
        //    string invoiceID = dgvDuLieuHD.Rows[e.RowIndex].Cells["MaHoaDon"].Value.ToString();
        //    var status = dgvDuLieuHD.Rows[e.RowIndex].Cells["TrangThai"].Value.ToString();
        //    var allowAction = status == "1";
        //    if (allowAction) return;
        //    var pageNumber = Convert.ToInt32(txtSoTrangHD.Text);
        //    if (dgvDuLieuHD.Columns[e.ColumnIndex].Name == "Delete")
        //    {
        //        DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa hóa đơn {invoiceID}?",
        //            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        //        if (result == DialogResult.Yes)
        //        {
        //            var check = _invoiceDetailService.Any(invoiceID);
        //            if (check.Data)
        //            {
        //                DialogResult resultCon = MessageBox.Show($"Phiếu hóa đơn {invoiceID} hiện đang còn dữ liệu.\n Nếu bạn xác nhận xoá, hệ thống sẽ xóa các dữ liệu chi tiết bên trong phiếu ! Xin vui lòng cân nhăc trước khi ấn nút xác nhận.",
        //                "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        //                if (resultCon != DialogResult.Yes) return;
        //                var isSucceeded = _invoiceDetailService.RemoveRangeInvoiceDetailByInvoiceID(invoiceID);
        //                if (!isSucceeded.Succeeded) { MessageBox.Show("Việc xóa các phiếu chi tiết đã xảy ra sự cố !", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
        //                _invoiceService.RemoveInvoice(invoiceID);
        //                MessageBox.Show("Xóa thành công!");
        //                LoadDataHD(_userSession.IdStore, pageNumber);
        //                return;
        //            }
        //            _invoiceService.RemoveInvoice(invoiceID);
        //            MessageBox.Show("Xóa thành công!");
        //            LoadDataHD(_userSession.IdStore, pageNumber); // tải lại dữ liệu
        //        }
        //    }
        //}

        //private void btnTrangTruocTH_Click(object sender, EventArgs e)
        //{
        //    int number = Convert.ToInt32(txtSoTrangHD.Text);
        //    if (number > 1)
        //    {

        //        var pageNumber = --number;
        //        txtSoTrangHD.Text = pageNumber.ToString();
        //        LoadDataHD(_userSession.IdStore, pageNumber);

        //    }
        //    else
        //    {
        //        btnTrangTruocHD.Enabled = false;
        //    }
        //}

        //private void btnTrangSauTH_Click(object sender, EventArgs e)
        //{
        //    int number = Convert.ToInt32(txtSoTrangHD.Text);
        //    btnTrangTruocHD.Enabled = true;
        //    if (number <= _totalPageHD)
        //    {
        //        var pageNumber = ++number;
        //        txtSoTrangHD.Text = pageNumber.ToString();
        //        LoadDataHD(_userSession.IdStore, pageNumber);
        //    }
        //}

        //private void btnThemTH_Click(object sender, EventArgs e)
        //{
        //    var invoiceDto = new InvoiceDto()
        //    {
        //        StoreId = _userSession.IdStore,
        //        EmployeeId = _userSession.UserId,
        //        InvoiceDate = DateTime.UtcNow.ToLocalTime(),
        //        FinalAmount = 0,
        //        DiscountTotal = 0,
        //    };
        //    Result<string> result = _invoiceService.CreateInvoice(invoiceDto);
        //    if (!result.Succeeded)
        //    {
        //        MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }
        //    LoadDataHD(_userSession.IdStore);
        //    _invoiceID = result.Data;
        //    btnHoanTat.Enabled = true;
        //    MessageBox.Show("Tạo hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //    HienThiCNCTHD(result.Data);
        //}
        //private void HienThiCNCTHD(string invoiceID)
        //{
        //    var frmChucNangChiTietHoaDon = _container.Resolve<frmChucNang_ChiTietHoaDon>(new ParameterOverride("invoiceID", invoiceID));
        //    frmChucNangChiTietHoaDon.dataChanged += (s, e) =>
        //    {
        //        LoadDataHD(_userSession.IdStore);
        //    };
        //    frmChucNangChiTietHoaDon.ShowDialog();
        //}

        //private void frmHienThi_BanHang_DoubleClick(object sender, EventArgs e)
        //{
        //    HienThiCNCTHD(_invoiceID);
        //}

        //private void dgvDuLieuTH_DoubleClick(object sender, EventArgs e)
        //{
        //    if (dgvDuLieuHD.CurrentCell == null || dgvDuLieuHD.Rows.Count == 0) return;
        //    int row = dgvDuLieuHD.CurrentCell.RowIndex;
        //    string invoiceID = dgvDuLieuHD.Rows[row].Cells["MaHoaDon"].Value.ToString();
        //    string status = dgvDuLieuHD.Rows[row].Cells["TrangThai"].Value.ToString();
        //    var frmChucNang = _container.Resolve<frmHienThi_ChiTietHoaDon>(new ParameterOverride("invoiceID", invoiceID), new ParameterOverride("status",status));
        //    frmChucNang.ShowDialog();
        //}

        //private void btnHoanTatTH_Click(object sender, EventArgs e)
        //{
        //    if (_invoiceID == null) return;
        //    var isSucceeded = _invoiceService.UpdateInvoice(_invoiceID);
        //    if (isSucceeded.Succeeded && isSucceeded.Data)
        //    {
        //        _invoiceID = null;
        //        btnHoanTat.Enabled = false;
        //        LoadDataHD(_userSession.IdStore);
        //        MessageBox.Show("Chốt hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }
        //}
       
        //#endregion 
    }
}
