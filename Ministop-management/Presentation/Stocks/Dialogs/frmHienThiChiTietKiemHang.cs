using CrystalDecisions.ReportAppServer;
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
    public partial class frmHienThiChiTietKiemHang : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IStockCheckDetailService _stockCheckDetailService;
        public readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        public string _checkID;
        private string _status;
        private long _totalPage;
        public frmHienThiChiTietKiemHang(IStockCheckDetailService stockCheckDetailService, IUserSession userSession, IUnityContainer container, string checkID = null, string Status = null)
        {
            InitializeComponent();
            _stockCheckDetailService = stockCheckDetailService;
            _container = container;
            _userSession = userSession;
            _checkID = checkID;
            _status = Status;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmHienThiChiTietKiemHang_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_checkID)) return;
            if (_status == "Duyệt" || _userSession.Role == "Admin"|| _userSession.Role == "Quản lý cửa hàng")
            {
                btnThem.Enabled = false;
            }
            LoadData(_checkID);
        }
        private void LoadData(string checkID, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuChiTiet");
            dt.Columns.Add("SanPham");
            dt.Columns.Add("SoLuongHeThong");
            dt.Columns.Add("SoLuongThucTe");
            dt.Columns.Add("ChenhLech");
            dt.Columns.Add("Ghi chu");
            using (var childContaner = _container.CreateChildContainer())
            {
                var checkExportDetailService = childContaner.Resolve<IStockCheckDetailService>();
                var list = checkExportDetailService.GetStockCheckDetail(checkID, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.Id, item.ProductName, item.QuantitySystem, item.QuantityActual, item.QuantityVariance,item.Note);
                }
            }
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ApplyGridStyle(dgvDuLieu);
            btnTrangTruoc.Enabled = pageNumber > 1;
            btnTrangSau.Enabled = pageNumber <= _totalPage;

        }
        private void ApplyGridStyle(Guna2DataGridView dgvDuLieu)
        {
            if (_userSession.Role == "Admin" || _status == "Duyệt") return;
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

                bool allowEditDelete = _status != "Duyệt";
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
        private void btnThem_Click(object sender, EventArgs e)
        {
            var frmChucNangCP = _container.Resolve<frmChucNang_ChiTietKiemKho>(new ParameterOverride("checkID", _checkID));
            frmChucNangCP.dataChanged += (s, ev) =>
            {

                LoadData(_checkID);
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
                LoadData(_checkID, pageNumber);

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
                LoadData(_checkID, pageNumber);
            }
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string id = dgvDuLieu.Rows[e.RowIndex].Cells["MaPhieuChiTiet"].Value.ToString();
            var allowAction = _status == "Duyệt";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrang.Text);
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCP = _container.Resolve<frmChucNang_ChiTietKiemKho>(new ParameterOverride("checkID", _checkID), new ParameterOverride("checkDetailID", id));
                frmChucNangCP.dataChanged += (s, ev) =>
                {
                    LoadData(_checkID, pageNumber);
                };
                frmChucNangCP.ShowDialog();

            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu chi tiet {id}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _stockCheckDetailService.RemoveStockCheckDetail(id);
                    MessageBox.Show("Xóa thành công!");
                    LoadData(_checkID, pageNumber); // tải lại dữ liệu
                }
            }
        }
    }
}
