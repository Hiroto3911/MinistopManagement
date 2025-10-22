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

namespace Presentation
{
    public partial class frmHienThi_NhanVien : Form
    {
        private readonly IStoreService _storeService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPage = 1;

        public frmHienThi_NhanVien(IStoreService storeService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _storeService = storeService;
            _container = container;
            _userSession = userSession;
            LoadData();
        }

       
        private void LoadData()
        {
            // ===== 1️⃣ Tạo bảng dữ liệu mẫu cho Nhân Viên =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNhanVien");
            dt.Columns.Add("HoTen");
            dt.Columns.Add("GioiTinh");
            dt.Columns.Add("NgaySinh", typeof(DateTime));
            dt.Columns.Add("SoDienThoai");
            dt.Columns.Add("ChucVu");
            dt.Columns.Add("CuaHang");
            dt.Columns.Add("NgayVaoLam", typeof(DateTime));
         

            // ===== 2️⃣ Dữ liệu mẫu =====
            dt.Rows.Add("NV001", "Nguyễn Văn A", "Nam", new DateTime(1998, 3, 15), "0909123456", "Quản Lý", "Ministop Quận 1", new DateTime(2021, 6, 10));
            dt.Rows.Add("NV002", "Trần Thị B", "Nữ", new DateTime(2000, 7, 22), "0909765432", "Nhân Viên", "Ministop Quận 1", new DateTime(2022, 1, 5));
            dt.Rows.Add("NV003", "Lê Quốc C", "Nam", new DateTime(1999, 12, 2), "0912345678", "Nhân Viên", "Ministop Quận 3", new DateTime(2023, 3, 20));
            dt.Rows.Add("NV004", "Phạm Duy D", "Nam", new DateTime(1995, 5, 10), "0988777666", "Admin", "Ministop Quận 1", new DateTime(2020, 8, 12));
            dt.Rows.Add("NV005", "Hoàng Ngọc E", "Nữ", new DateTime(1997, 9, 28), "0911999888", "Nhân Viên", "Ministop Bình Thạnh", new DateTime(2024, 4, 15));

            dgvDuLieu_NhanVien.DataSource = dt;
            dgvDuLieu_NhanVien.AllowUserToAddRows = false;
            dgvDuLieu_NhanVien.ReadOnly = true;
            dgvDuLieu_NhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvDuLieu_NhanVien.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvDuLieu_NhanVien.Columns.Add(btnEdit);
            }

            if (dgvDuLieu_NhanVien.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieu_NhanVien.Columns.Add(btnDelete);
            }

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu_NhanVien.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu_NhanVien.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu_NhanVien.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu_NhanVien.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu_NhanVien.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu_NhanVien.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieu_NhanVien.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name;
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

        private void frmHienThi_NhanVien_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlNV.TabPages.Remove(tabCaLam);
                tabControlNV.TabPages.Remove(tabPhuCap);
               
            }else if( _userSession.Role == "Nhân viên")
            {
                tabControlNV.TabPages.Remove(tabCaLam);
                tabControlNV.TabPages.Remove(tabPhuCap);
                tabControlNV.TabPages.Remove(tabTinhLuong);
                tabControlNV.TabPages.Remove(tabHopDong);
                tabControlNV.TabPages.Remove(tabChamCongVang);
                tabControlNV.TabPages.Remove(tabNhanVien);

                
            }
        }

        #region Quản lý phụ cấp
        private void LoadData_PhuCap()
        {

        }
        #endregion
    }
}
