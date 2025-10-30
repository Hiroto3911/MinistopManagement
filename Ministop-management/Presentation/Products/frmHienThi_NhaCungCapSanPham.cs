using Services.Interfaces;
using Services.Services;
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

namespace Presentation.Products
{
    public partial class frmHienThi_NhaCungCapSanPham : Form
    {
        private readonly IUnityContainer _container;
        private readonly ISupplierProductService _supplierProductService;
        private long _totalPageSPNCC = 1;
        private string _supplierID;
        private long _totalPageSNCCSP = 1;
        public frmHienThi_NhaCungCapSanPham(IUnityContainer container, ISupplierProductService supplierProductService, string supplierID = null)
        {
            InitializeComponent();
            _container = container;
            _supplierID = supplierID;
            _supplierProductService = supplierProductService;
            LoadDataNCCSP(_supplierID);
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var frmNCC = _container.Resolve<frmChucNang_NhaCungCapSanPham>(new ParameterOverride("supplierID", _supplierID));

            frmNCC.Datachanged += (s, ev) =>
            {
                LoadDataNCCSP(_supplierID);
            };

            frmNCC.ShowDialog();
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }

            string supplierProductId = dgvDuLieu.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                var formChucNang_NhaCungCapSanPham = _container.Resolve<frmChucNang_NhaCungCapSanPham>(new ParameterOverride("supplierProductId", supplierProductId));
                formChucNang_NhaCungCapSanPham.Datachanged += (s, ev) =>
                {
                    LoadDataNCCSP(_supplierID);
                };
                formChucNang_NhaCungCapSanPham.ShowDialog();
            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult question = MessageBox.Show($"bạn có chắc chắn muốn xóa {supplierProductId} của nhà cung cấp này không!!", "xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (question == DialogResult.Yes)
                {
                    _supplierProductService.RemoveSupplierProduct(supplierProductId);
                    MessageBox.Show("Xóa thành công");
                    LoadDataNCCSP(_supplierID);
                }

            }
        }

        private void LoadDataNCCSP(string supplierId, int pageNumber = 1, int pageSize = 5)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("MaNhaCungCap");
            dt.Columns.Add("TenSanPham"); 
            dt.Columns.Add("GiaTuNhaCungCap");
            dt.Columns.Add("TrangThai");

            using (var childContainer = _container.CreateChildContainer())
            {
                var supplierProductService = childContainer.Resolve<ISupplierProductService>();
                var list = supplierProductService.GetSupplierProduct(supplierId, pageNumber, pageSize);

                if (!list.Succeeded || list.Data == null)
                    return;

                _totalPageSPNCC = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                foreach (var item in list.Data)
                {

                    dt.Rows.Add(item.Id, item.SupplierId, item.ProductName, item.SupplyPrice, item.Status);
                }         
        }

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thêm nút Edit, Delete nếu chưa có
            if (dgvDuLieu.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu.Columns.Add(btnEdit);
            }

            if (dgvDuLieu.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu.Columns.Add(btnDelete);
            }

            // Giao diện đẹp
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

            // Vẽ lại nút Edit/Delete
            dgvDuLieu.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                    (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" || dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);
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

                    e.Handled = true;
                }
            };

            // Phân trang
            btnTrangTruocNCCSP.Enabled = pageNumber > 1;
            btnTrangSauNCCSP.Enabled = pageNumber < _totalPageSPNCC;
        }

        private void frmHienThi_NhaCungCapSanPham_Load(object sender, EventArgs e)
        {

        }

        private void btnTrangSauNCCSP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangNCCSP.Text);
            btnTrangTruocNCCSP.Enabled = true;
            if (number <= _totalPageSNCCSP)
            {
                var pageNumber = ++number;
                txtTrangNCCSP.Text = pageNumber.ToString();
                LoadDataNCCSP(_supplierID,pageNumber);
            }
        }

        private void btnTrangTruocNCCSP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangNCCSP.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtTrangNCCSP.Text = pageNumber.ToString();
                LoadDataNCCSP(_supplierID,pageNumber);

            }
            else
            {
                btnTrangTruocNCCSP.Enabled = false;
            }
        }
    }
}
