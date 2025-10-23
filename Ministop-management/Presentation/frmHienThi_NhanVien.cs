using Domain.DTO;
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
using Unity.Lifetime;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_NhanVien : Form
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAllowanceService _allowanceService;
        private readonly IShiftService _shiftService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPage = 1;

        public frmHienThi_NhanVien(IEmployeeService employeeService, IAllowanceService allowanceService, IShiftService shiftService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _allowanceService = allowanceService;
            _shiftService = shiftService;
            _container = container;
            _userSession = userSession;
        }

        private void frmHienThi_NhanVien_Load(object sender, EventArgs e)
        {
            LoadDanhSachCuaHang();
            LoadData_NhanVien();
            LoadData_PhuCap();
            LoadData_CaLam();
        }

        #region Lọc nhân viên
        private void LoadDanhSachCuaHang()
        {
            try
            {
                using (var childContainer = _container.CreateChildContainer())
                {
                    var storeService = childContainer.Resolve<IStoreService>();
                    var result = storeService.GetAll(); // trả về PagedResult<IReadOnlyList<StoreDto>>

                    if (result != null && result.Succeeded && result.Data != null)
                    {
                        cboChonCuaHang_NV.DataSource = result.Data.ToList(); // ⚡ Quan trọng: ToList()
                        cboChonCuaHang_NV.DisplayMember = "StoreName";
                        cboChonCuaHang_NV.ValueMember = "StoreId";
                        cboChonCuaHang_NV.SelectedIndex = -1; // chưa chọn gì
                    }
                    else
                    {
                        MessageBox.Show("Không thể tải danh sách cửa hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        cboChonCuaHang_NV.DataSource = null;
                    }
                }

                cboChonCuaHang_NV.SelectedIndexChanged += cboChonCuaHang_NV_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách cửa hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void cboChonCuaHang_NV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChonCuaHang_NV.SelectedValue == null)
                return;

            string storeId = cboChonCuaHang_NV.SelectedValue.ToString();
            LoadData_NhanVienTheoCuaHang(storeId);
        }

        private void LoadData_NhanVienTheoCuaHang(string storeId, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNhanVien");
            dt.Columns.Add("MaCuaHang");
            dt.Columns.Add("HoTen");
            dt.Columns.Add("GioiTinh");
            dt.Columns.Add("NgaySinh");
            dt.Columns.Add("SoDienThoai");
            dt.Columns.Add("ChucVu");
            dt.Columns.Add("LoaiNhanVien");

            using (var childContainer = _container.CreateChildContainer())
            {
                var employeeService = childContainer.Resolve<IEmployeeService>();
                var list = employeeService.GetEmployeeByStore(storeId, pageNumber, pageSize);

                if (list.Succeeded == false || list.Data == null) return;

                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                foreach (var item in list.Data)
                {
                    string genderText = item.Gender ? "Nam" : "Nữ";
                    dt.Rows.Add(item.EmployeeId,
                                item.StoreId,
                                item.FullName,
                                genderText,
                                item.BirthDate.ToString("dd/MM/yyyy"),
                                item.Phone,
                                item.Position,
                                item.EmploymentType);
                }
            }

            dgvDuLieu_NhanVien.DataSource = dt;
            ApplyGridStyle(dgvDuLieu_NhanVien);
        }

        #endregion

        #region Quản lý nhân viên
        private void LoadData_NhanVien(int pageNumber = 1, int pageSize = 20)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách Nhân Viên =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNhanVien");       // EmployeeID
            dt.Columns.Add("MaCuaHang");        // StoreID
            dt.Columns.Add("HoTen");            // FullName
            dt.Columns.Add("GioiTinh");         // Gender
            dt.Columns.Add("NgaySinh");         // BirthDate
            dt.Columns.Add("SoDienThoai");      // Phone
            dt.Columns.Add("ChucVu");           // Position
            dt.Columns.Add("LoaiNhanVien");     // EmploymentType
            dt.Columns.Add("MatKhau");          // PasswordHash (ẩn, chỉ để debug nếu cần)

            using (var childContainer = _container.CreateChildContainer())
            {
                var employeeService = childContainer.Resolve<IEmployeeService>();
                var list = employeeService.GetEmployee(pageNumber, pageSize);

                // Nếu lấy dữ liệu thất bại hoặc không có dữ liệu
                if (list.Succeeded == false || list.Data == null) { return; }

                // Tính tổng số trang
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                // Thêm từng dòng dữ liệu vào DataTable
                foreach (var item in list.Data)
                {
                    string genderText = item.Gender ? "Nam" : "Nữ";
                    dt.Rows.Add(item.EmployeeId,
                                item.StoreId,
                                item.FullName,
                                genderText,
                                item.BirthDate.ToString("dd/MM/yyyy"),
                                item.Phone,
                                item.Position,
                                item.EmploymentType,
                                item.PasswordHash);
                }
            }

            // ===== 2️⃣ Gán dữ liệu lên DataGridView =====
            dgvDuLieu_NhanVien.DataSource = dt;
            dgvDuLieu_NhanVien.AllowUserToAddRows = false;
            dgvDuLieu_NhanVien.ReadOnly = true;
            dgvDuLieu_NhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Ẩn cột mật khẩu (vì chỉ để lưu, không hiển thị)
            if (dgvDuLieu_NhanVien.Columns["MatKhau"] != null)
                dgvDuLieu_NhanVien.Columns["MatKhau"].Visible = false;

            // ===== 3️⃣ Thêm hai cột nút (Edit/Delete) nếu chưa có =====
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

            // ===== 4️⃣ Áp dụng style cho bảng =====
            ApplyGridStyle(dgvDuLieu_NhanVien);

            // ===== 5️⃣ Kích hoạt hoặc vô hiệu hoá nút phân trang =====
            btnTrangTruocNV.Enabled = pageNumber > 1;
            btnTrangSauNV.Enabled = pageNumber < _totalPage;
        }

        private void dgvDuLieu_NhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy mã nhân viên từ hàng được chọn
            string employeeId = dgvDuLieu_NhanVien.Rows[e.RowIndex].Cells["MaNhanVien"].Value.ToString();

            if (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Edit")
            {
                // ===== 1️⃣ Mở form chỉnh sửa nhân viên =====
                var frmChucNangNhanVien = _container.Resolve<frmChucNang_NhanVien>(
                    new ParameterOverride("employeeId", employeeId));

                // Khi dữ liệu thay đổi, tự động load lại danh sách
                frmChucNangNhanVien.DataChanged += (s, ev) => LoadData_NhanVien();

                frmChucNangNhanVien.ShowDialog();
            }
            else if (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Delete")
            {
                // ===== 2️⃣ Xác nhận xóa nhân viên =====
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {employeeId}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _employeeService.RemoveEmployee(employeeId);
                    MessageBox.Show("Xóa nhân viên thành công!");
                    LoadData_NhanVien(); // Tải lại dữ liệu sau khi xóa
                }
            }
        }

        private void btnThemNhanVien_Click(object sender, EventArgs e)
        {
            var frmChucNangNV = _container.Resolve<frmChucNang_NhanVien>();
            frmChucNangNV.DataChanged += (s, ev) => LoadData_NhanVien();
            frmChucNangNV.ShowDialog();
        }

        private void btnTrangSauNV_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangNV.Text);
            btnTrangTruocNV.Enabled = true;

            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrangNV.Text = pageNumber.ToString();
                LoadData_NhanVien(pageNumber);
            }
        }

        private void btnTrangTruocNV_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangNV.Text);

            if (number > 1)
            {
                var pageNumber = --number;
                txtSoTrangNV.Text = pageNumber.ToString();
                LoadData_NhanVien(pageNumber);
            }
            else
            {
                btnTrangTruocNV.Enabled = false;
            }
        }
        #endregion


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
            ApplyGridStyle(dgvDuLieu_PhuCap);

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

        #region Quản lý ca làm
        private void LoadData_CaLam(int pageNumber = 1, int pageSize = 20)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách Ca Làm =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaCaLam");      // ShiftID
            dt.Columns.Add("TenCaLam");     // ShiftName
            dt.Columns.Add("GioBatDau");    // StartTime
            dt.Columns.Add("GioKetThuc");   // EndTime

            using (var childContainer = _container.CreateChildContainer())
            {
                var shiftService = childContainer.Resolve<IShiftService>();
                var list = shiftService.GetShift(pageNumber, pageSize);

                // Nếu lấy dữ liệu thất bại hoặc không có dữ liệu
                if (list.Succeeded == false || list.Data == null) { return; }

                // Tính tổng số trang
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                // Thêm từng dòng dữ liệu vào DataTable
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.ShiftId, item.ShiftName, item.StartTime, item.EndTime);
                }
            }

            // ===== 2️⃣ Gán dữ liệu lên DataGridView =====
            dgvDuLieu_CaLam.DataSource = dt;
            dgvDuLieu_CaLam.AllowUserToAddRows = false;
            dgvDuLieu_CaLam.ReadOnly = true;
            dgvDuLieu_CaLam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 3️⃣ Thêm hai cột nút (Edit/Delete) nếu chưa có =====
            if (dgvDuLieu_CaLam.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvDuLieu_CaLam.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_CaLam.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieu_CaLam.Columns.Add(btnDelete);
            }

            // ===== 4️⃣ Áp dụng style cho bảng =====
            ApplyGridStyle(dgvDuLieu_CaLam);

            // ===== 5️⃣ Kích hoạt hoặc vô hiệu hoá nút phân trang =====
            btnTrangTruocCL.Enabled = pageNumber > 1;
            btnTrangSauCL.Enabled = pageNumber < _totalPage;
        }

        private void dgvDuLieu_CaLam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy mã ca làm từ hàng được chọn
            string shiftId = dgvDuLieu_CaLam.Rows[e.RowIndex].Cells["MaCaLam"].Value.ToString();

            if (dgvDuLieu_CaLam.Columns[e.ColumnIndex].Name == "Edit")
            {
                // ===== 1️⃣ Mở form chỉnh sửa ca làm =====
                var frmChucNangCaLam = _container.Resolve<frmChucNang_CaLamViec>(
                    new ParameterOverride("shiftId", shiftId));

                // Khi dữ liệu thay đổi, tự động load lại danh sách
                frmChucNangCaLam.DataChanged += (s, ev) =>
                {
                    LoadData_CaLam();
                };

                frmChucNangCaLam.ShowDialog();
            }
            else if (dgvDuLieu_CaLam.Columns[e.ColumnIndex].Name == "Delete")
            {
                // ===== 2️⃣ Xác nhận xóa ca làm =====
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa ca làm {shiftId}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _shiftService.RemoveShift(shiftId);
                    MessageBox.Show("Xóa thành công!");
                    LoadData_CaLam(); // tải lại dữ liệu sau khi xóa
                }
            }
        }

        private void btnThemCaLam_Click(object sender, EventArgs e)
        {
            var frmChucNangCL = _container.Resolve<frmChucNang_CaLamViec>();
            frmChucNangCL.DataChanged += (s, ev) => LoadData_CaLam();
            frmChucNangCL.ShowDialog();
        }

        private void btnTrangTruocCL_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCL.Text);
            btnTrangTruocCL.Enabled = true;

            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrangCL.Text = pageNumber.ToString();
                LoadData_CaLam(pageNumber);
            }
        }

        private void btnTrangSauCL_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCL.Text);

            if (number > 1)
            {
                var pageNumber = --number;
                txtSoTrangCL.Text = pageNumber.ToString();
                LoadData_CaLam(pageNumber);
            }
            else
            {
                btnTrangTruocCL.Enabled = false;
            }
        }
        #endregion

        #region Chỉnh giao diện cho từng bảng
        private void ApplyGridStyle(Guna2DataGridView dgvDuLieu)
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

        private void ibtnThungRac_NV_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_NhanVien>();
            frmThungRac.datachanged += (s, ev) => LoadData_NhanVien();
            frmThungRac.ShowDialog();
        }

        private void ibtnThungRac_PC_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_PhuCap>();
            frmThungRac.datachanged += (s, ev) => LoadData_NhanVien();
            frmThungRac.ShowDialog();
        }

        
    }
}
