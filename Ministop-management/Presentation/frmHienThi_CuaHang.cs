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
using Unity.Lifetime;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_CuaHang : Form
    {
        private readonly IStoreService _storeService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPage = 1;

        public frmHienThi_CuaHang(IStoreService storeService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _storeService = storeService;
            _container = container;
            _userSession = userSession;
            LoadData_CuaHang();
        }
        private void frmHienThi_CuaHang_Load(object sender, EventArgs e)
        {
           
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlCH.TabPages.Remove(tabCuaHang);
            }
           
        }
        #region ChucNangCuaHang
        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string storeId = dgvDuLieu.Rows[e.RowIndex].Cells["MaCuaHang"].Value.ToString();

            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCH = _container.Resolve<frmChucNang_CuaHang>(new ParameterOverride("storeId", storeId));
                frmChucNangCH.DataChanged += (s, ev) =>
                {

                    LoadData_CuaHang();
                };
                frmChucNangCH.ShowDialog();

            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa cửa hàng {storeId}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _storeService.RemoveStore(storeId);
                    MessageBox.Show("Xóa thành công!");
                    LoadData_CuaHang(); // tải lại dữ liệu
                }
            }
        }
        private void LoadData_CuaHang(int pageNumber = 1, int pageSize = 20)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách cửa hàng =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaCuaHang");
            dt.Columns.Add("TenCuaHang");
            dt.Columns.Add("DiaChi");
            dt.Columns.Add("SoDienThoai");
            using (var childContainer = _container.CreateChildContainer())
            {
                var storeService = childContainer.Resolve<IStoreService>();
                var list = storeService.GetStore(pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPage = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.StoreId, item.StoreName, item.Address, item.Phone);
                }
            }
            // ===== 2️⃣ Dữ liệu mẫu (có thể thay bằng dữ liệu trong DB sau này) =====
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

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
            ApplyGridStyle();
            btnTrangTruocCH.Enabled = pageNumber > 1;
            btnTrangSauCH.Enabled = pageNumber <= _totalPage;


        }



        private void btnThemCuaHang_Click(object sender, EventArgs e)
        {
            var frmChucNangCH = _container.Resolve<frmChucNang_CuaHang>();
            frmChucNangCH.DataChanged += (s, ev) => LoadData_CuaHang();
            frmChucNangCH.ShowDialog();
        }

        private void btnTrangSauCH_Click(object sender, EventArgs e)
        {

            int number = Convert.ToInt32(txtSoTrangCH.Text);
            btnTrangTruocCH.Enabled = true;
            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrangCH.Text = pageNumber.ToString();
                LoadData_CuaHang(pageNumber);
            }


        }

        private void btnTrangTruocCH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCH.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangCH.Text = pageNumber.ToString();
                LoadData_CuaHang(pageNumber);

            }
            else
            {
                btnTrangTruocCH.Enabled = false;
            }
        }
        #endregion

        #region thiet ke giao dien Cua Hang
        private void ApplyGridStyle()
        {
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
                if (e.RowIndex >= 0 && (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
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
        }


        #endregion

        private void ibtnDuLieuBiXoa_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_CuaHang>();
            frmThungRac.datachanged += (s, ev) => LoadData_CuaHang();
            frmThungRac.ShowDialog();
        }
    }
}
