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
    public partial class frmHienThi_NhanVien : Form
    {
        private readonly IAllowanceService _allowanceService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPage = 1;

        public frmHienThi_NhanVien(IAllowanceService allowanceService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _allowanceService = allowanceService;
            _container = container;
            _userSession = userSession;
            LoadData_NhanVien();
            LoadData_PhuCap();
        }

       
        private void LoadData_NhanVien()
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
        private void LoadData_PhuCap(int pageNumber = 1, int pageSize = 20)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách Phụ Cấp =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhuCap");       // AllowanceID
            dt.Columns.Add("TenPhuCap");      // AllowanceName
            dt.Columns.Add("MucMacDinh");     // DefaultAmount

            using (var childContainer = _container.CreateChildContainer())
            {
                var allowanceService = childContainer.Resolve<IAllowanceService>();
                var list = allowanceService.GetAllowance(pageNumber, pageSize);

                // Nếu lấy dữ liệu thất bại hoặc không có dữ liệu
                if (list.Succeeded == false || list.Data == null) { return; }

                // Tính tổng số trang
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                // Thêm từng dòng dữ liệu vào DataTable
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.AllowanceId, item.AllowanceName, item.DefaultAmount);
                }
            }

            // ===== 2️⃣ Gán dữ liệu lên DataGridView =====
            dgvDuLieu_PhuCap.DataSource = dt;
            dgvDuLieu_PhuCap.AllowUserToAddRows = false;
            dgvDuLieu_PhuCap.ReadOnly = true;
            dgvDuLieu_PhuCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 3️⃣ Thêm hai cột nút (Edit/Delete) nếu chưa có =====
            if (dgvDuLieu_PhuCap.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvDuLieu_PhuCap.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_PhuCap.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieu_PhuCap.Columns.Add(btnDelete);
            }

            // ===== 4️⃣ Áp dụng style cho bảng =====
            ApplyGridStyle();

            // ===== 5️⃣ Kích hoạt hoặc vô hiệu hoá nút phân trang =====
            btnTrangTruocPK.Enabled = pageNumber > 1;
            btnTrangSauPK.Enabled = pageNumber < _totalPage;
        }

        private void dgvDuLieu_PhuCap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy mã phụ cấp từ hàng được chọn
            string allowanceId = dgvDuLieu_PhuCap.Rows[e.RowIndex].Cells["MaPhuCap"].Value.ToString();

            if (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Edit")
            {
                // ===== 1️⃣ Mở form chỉnh sửa phụ cấp =====
                var frmChucNangPhuCap = _container.Resolve<frmChucNang_PhuCap>(new ParameterOverride("allowanceId", allowanceId));

                // Khi dữ liệu thay đổi, tự động load lại danh sách
                frmChucNangPhuCap.DataChanged += (s, ev) =>
                {
                    LoadData_PhuCap(); 
                };

                frmChucNangPhuCap.ShowDialog();
            }
            else if (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Delete")
            {
                // ===== 2️⃣ Xác nhận xóa phụ cấp =====
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phụ cấp {allowanceId}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _allowanceService.RemoveAllowance(allowanceId);
                    MessageBox.Show("Xóa thành công!");
                    LoadData_PhuCap(); // tải lại dữ liệu sau khi xóa
                }
            }
        }

        private void btnThemPhuCap_Click(object sender, EventArgs e)
        {
            var frmChucNangPK = _container.Resolve<frmChucNang_PhuCap>();
            frmChucNangPK.DataChanged += (s, ev) => LoadData_PhuCap();
            frmChucNangPK.ShowDialog();
        }

        private void btnTrangTruocPK_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangPK.Text);
            btnTrangTruocPK.Enabled = true;

            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrangPK.Text = pageNumber.ToString();
                LoadData_PhuCap(pageNumber);
            }
        }

        private void btnTrangSauPK_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangPK.Text);

            if (number > 1)
            {
                var pageNumber = --number;
                txtSoTrangPK.Text = pageNumber.ToString();
                LoadData_PhuCap(pageNumber);
            }
            else
            {
                btnTrangTruocPK.Enabled = false;
            }
        }

        #endregion

        #region thiet ke giao dien phu cap
        private void ApplyGridStyle()
        {
            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu_PhuCap.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu_PhuCap.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu_PhuCap.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu_PhuCap.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu_PhuCap.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu_PhuCap.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieu_PhuCap.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name;
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

        
    }
}
