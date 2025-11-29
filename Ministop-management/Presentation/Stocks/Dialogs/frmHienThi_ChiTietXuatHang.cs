using Guna.UI2.WinForms;
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
using System.Windows.Forms;
using System.Xml.Linq;
using Unity;
using Unity.Resolution;

namespace Presentation.Stocks.Dialogs
{
    public partial class frmHienThi_ChiTietXuatHang : Form
    {
        public event EventHandler dataChanged;
        private readonly IStockExportDetailService _stockExportDetailService;
        private readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        private string _exportID;
        private string _status;
        private long _totalPage;
        private decimal _totalAmount ; 

        public frmHienThi_ChiTietXuatHang(IStockExportDetailService stockExportDetailService, IUserSession userSession, IUnityContainer container, string ExportID = null, string Status = null)
        {
            InitializeComponent();
            _stockExportDetailService = stockExportDetailService;
            _userSession = userSession;
            _container = container;
            _exportID = ExportID;
            _status = Status;
        }


        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmHienThi_ChiTietXuatHang_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_exportID)) return;
            if ( _status == "Duyệt" || _status == "Permitted" || _userSession.Role == "Admin")
            {
                btnThem.Enabled = false;
            }
            LoadData(_exportID);
        }
        private void LoadData(string exportID, int pageNumber = 1, int pageSize = 20)
        {
            _totalAmount = 0; 
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuChiTiet");
            dt.Columns.Add("SanPham");
            dt.Columns.Add("SoLUong");
            dt.Columns.Add("DonGia");
            dt.Columns.Add("ThanhTien");
            using (var childContaner = _container.CreateChildContainer())
            {
                var stockExportDetailService = childContaner.Resolve<IStockExportDetailService>();
                var list = stockExportDetailService.GetStockExportDetail(exportID, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.Id, item.ProductName, item.Quantity, item.UnitPrice, item.Total);
                    _totalAmount +=  item.Total;
                }
            }
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.Columns["MaPhieuChiTiet"].HeaderText = Properties.Resources.Grid_ID;
            dgvDuLieu.Columns["SanPham"].HeaderText = Properties.Resources.Grid_ProductName;
            dgvDuLieu.Columns["SoLuong"].HeaderText = Properties.Resources.Grid_Quantity;
            dgvDuLieu.Columns["DonGia"].HeaderText = Properties.Resources.Grid_Price;
            dgvDuLieu.Columns["ThanhTien"].HeaderText = Properties.Resources.Grid_Total;
            ApplyGridStyle(dgvDuLieu);
            lblTongTienXuat.Text = $"{Properties.Resources.Label_TotalAmount} {_totalAmount} {Properties.Resources.Label_Money}";
            btnTrangTruoc.Enabled = pageNumber > 1;
            btnTrangSau.Enabled = pageNumber <= _totalPage;

        }
        private void ApplyGridStyle(Guna2DataGridView dgvDuLieu)
        {
            if (_userSession.Role == "Admin" || _status == "Duyệt" || _status == "Permitted") return;
            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvDuLieu.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvDuLieu.Columns.Add(btnEdit);
            }
            if (dgvDuLieu.Columns["Delete"] == null)
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

            dgvDuLieu.CellPainting += (s, e) =>
            {

                bool allowEditDelete = _status == "Duyệt" || _status == "Permitted";
                if (e.RowIndex >= 0 && (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);
                    if (!allowEditDelete)
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
        private void btnThem_Click(object sender, EventArgs e)
        {
            var frmChucNangCP = _container.Resolve<frmChucNang_ChiTietXuatKho>(new ParameterOverride("ExportID", _exportID));
            frmChucNangCP.dataChanged += (s, ev) =>
            {

                LoadData(_exportID);
            };
            frmChucNangCP.ShowDialog();
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData(_exportID, pageNumber);

            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            btnTrangTruoc.Enabled = true;
            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData(_exportID, pageNumber);
            }
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string id = dgvDuLieu.Rows[e.RowIndex].Cells["MaPhieuChiTiet"].Value.ToString();
            var allowAction = _status == "Duyệt" || _status == "Permitted";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrang.Text);
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCP = _container.Resolve<frmChucNang_ChiTietXuatKho>(new ParameterOverride("ExportID", _exportID), new ParameterOverride("ExportDetailID", id));
                frmChucNangCP.dataChanged += (s, ev) =>
                {
                    LoadData(_exportID, pageNumber);
                };
                frmChucNangCP.ShowDialog();

            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"{Properties.Messages.Message_DeleteData} {id}?",
                    $"{Properties.Messages.Message_Confirm}", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _stockExportDetailService.RemoveStockExportDetail(id);
                     MessageBox.Show($"{Properties.Messages.Message_DeletedSuccessfully}");
                    LoadData(_exportID, pageNumber); // tải lại dữ liệu
                }
            }
        }
    }
}
