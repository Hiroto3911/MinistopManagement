using Guna.UI2.WinForms;
using Services.Interfaces;
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
using Unity;
using Unity.Resolution;

namespace Presentation.Sales.Dialogs
{
    public partial class frmHienThi_ChiTietHoaDon : Form
    {
        public event EventHandler dataChanged;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        public string _invoiceID;
        private readonly string _supplierID;
        private string _status;
        private long _totalPage;

        public frmHienThi_ChiTietHoaDon(IInvoiceDetailService invoiceDetailService, IUserSession userSession, IUnityContainer container, string invoiceID = null, string status = null)
        {
            InitializeComponent();
            _invoiceDetailService = invoiceDetailService;
            _userSession = userSession;
            _container = container;
            _invoiceID = invoiceID;
            _status = status;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmHienThi_ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_invoiceID)) return;
            lblTieuDe.Text = $"Danh sách chi tiết hóa đơn {_invoiceID}";
            LoadData(_invoiceID);

        }
        private void LoadData(string _invoiceID, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhieuChiTiet");
            dt.Columns.Add("SanPham");
            dt.Columns.Add("SoLUong");
            dt.Columns.Add("DonGia");
            dt.Columns.Add("SoTienGiam");
            dt.Columns.Add("ThanhTien");
            using (var childContaner = _container.CreateChildContainer())
            {
                var stockImportDetailService = childContaner.Resolve<IInvoiceDetailService>();
                var list = stockImportDetailService.GetInvoiceDetail(_invoiceID, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.Id, item.ProductId, item.Quantity, item.UnitPrice, item.DiscountAmount, item.FinalUnitPrice);
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
            if (_userSession.Role == "Admin" || _status == "1") { btnThem.Enabled = false; return; }
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

                bool allowEditDelete = _status != "1";
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


        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData(_invoiceID, pageNumber);

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
                LoadData(_invoiceID, pageNumber);
            }
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string id = dgvDuLieu.Rows[e.RowIndex].Cells["MaPhieuChiTiet"].Value.ToString();
            var allowAction = _status == "1";
            if (allowAction) return;
            var pageNumber = Convert.ToInt32(txtSoTrang.Text);
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCP = _container.Resolve<frmChucNang_ChiTietHoaDon>(new ParameterOverride("invoiceID", _invoiceID), new ParameterOverride("invoiceDetailID", id));
                frmChucNangCP.dataChanged += (s, ev) =>
                {
                    LoadData(_invoiceID, pageNumber);
                };
                frmChucNangCP.ShowDialog();

            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu chi tiet {id}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _invoiceDetailService.RemoveInvoiceDetail(id);
                    MessageBox.Show("Xóa thành công!");
                    LoadData(_invoiceID, pageNumber); // tải lại dữ liệu
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var frmChucNangCP = _container.Resolve<frmChucNang_ChiTietHoaDon>(new ParameterOverride("invoiceID", _invoiceID));
            frmChucNangCP.dataChanged += (s, ev) =>
            {

                LoadData(_invoiceID);
            };
            frmChucNangCP.ShowDialog();
        }

      
    }
}
