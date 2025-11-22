using Domain.DTO;
using Guna.UI2.WinForms;
using Services.Interfaces;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_NhanVien : Form
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAllowanceService _allowanceService;
        private readonly IShiftService _shiftService;
        private readonly IShiftAssignmentService _shiftAssignmentService;
        private readonly ISalaryContractService _salaryContractService;
        private readonly ISalaryService _salaryService;
        private readonly ISalaryContractAllowanceService _salaryContractAllowanceService;
        private readonly IAbsenceService _absenceService;
        private readonly IStoreService _storeService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;

        private long _totalPage_NV = 1;
        private long _totalPage_HD = 1;
        private long _totalPage_PC = 1;
        private long _totalPage_CL = 1;
        private long _totalPage_PhanCong = 1;
        private long _totalPage_Vang = 1;

        public frmHienThi_NhanVien(
            IEmployeeService employeeService,
            IAllowanceService allowanceService,
            IShiftService shiftService,
            IShiftAssignmentService shiftAssignmentService,
            ISalaryContractService salaryContractService,
            IAbsenceService absenceService,
            ISalaryService salaryService,
            ISalaryContractAllowanceService salaryContractAllowanceService,
            IStoreService storeService,
            IUnityContainer container,
            IUserSession userSession)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _allowanceService = allowanceService;
            _shiftService = shiftService;
            _shiftAssignmentService = shiftAssignmentService;
            _salaryContractService = salaryContractService;
            _absenceService = absenceService;
            _salaryService = salaryService;
            _salaryContractAllowanceService = salaryContractAllowanceService;
            _storeService = storeService;
            _container = container;
            _userSession = userSession;
        }

        private void frmHienThi_NhanVien_Load(object sender, EventArgs e)
        {
            LoadDanhSachCuaHang();
            LoadData_NhanVien();

            cboChonCuaHang_NV.SelectedIndexChanged -= cboChonCuaHang_NV_SelectedIndexChanged;
            cboChonCuaHang_PC.SelectedIndexChanged -= cboChonCuaHang_PC_SelectedIndexChanged;
            cboChonCuaHang_HD.SelectedIndexChanged -= cboChonCuaHang_HD_SelectedIndexChanged;
            cboChonCuaHang_Vang.SelectedIndexChanged -= cboChonCuaHang_Vang_SelectedIndexChanged;

            string storeIdload = _userSession.Role == "Admin" ? null : _userSession.IdStore;

            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlNV.TabPages.Remove(tabCaLam);
                tabControlNV.TabPages.Remove(tabPhuCap);
                cboChonCuaHang_NV.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_NV.Enabled = false;
                cboChonCuaHang_HD.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_HD.Enabled = false;
                storeIdload = _userSession.IdStore;
                LoadData_NhanVienTheoCuaHang(storeIdload);
            }
            else if (_userSession.Role == "Admin")
            {
                cboChonCuaHang_NV.Enabled = true;
                cboChonCuaHang_PC.Enabled = true;
                cboChonCuaHang_HD.Enabled = true;
                cboChonCuaHang_Vang.Enabled = true;
                if (cboChonCuaHang_NV.SelectedValue != null)
                    storeIdload = cboChonCuaHang_NV.SelectedValue.ToString();
                btnTaoHopDongMoi.Visible = true;
                btnTaoHopDongMoi.Click += BtnTaoHopDongMoi_Click;
            }
            else if (_userSession.Role == "Nhân viên")
            {
                tabControlNV.TabPages.Remove(tabCaLam);
                tabControlNV.TabPages.Remove(tabPhuCap);
                tabControlNV.TabPages.Remove(tabTinhLuong);
                tabControlNV.TabPages.Remove(tabHopDong);
                tabControlNV.TabPages.Remove(tabChamCongVang);
                tabControlNV.TabPages.Remove(tabNhanVien);
                cboChonCuaHang_NV.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_NV.Enabled = false;
                storeIdload = _userSession.IdStore;
                LoadData_NhanVienTheoCuaHang(storeIdload);
            }

            cboChonCuaHang_PC.DataSource = cboChonCuaHang_NV.DataSource;
            cboChonCuaHang_PC.DisplayMember = "StoreName";
            cboChonCuaHang_PC.ValueMember = "StoreId";
            cboChonCuaHang_HD.DataSource = cboChonCuaHang_NV.DataSource;
            cboChonCuaHang_HD.DisplayMember = "StoreName";
            cboChonCuaHang_HD.ValueMember = "StoreId";
            cboChonCuaHang_Vang.DataSource = cboChonCuaHang_NV.DataSource;
            cboChonCuaHang_Vang.DisplayMember = "StoreName";
            cboChonCuaHang_Vang.ValueMember = "StoreId";

            if (_userSession.Role != "Admin")
            {
                cboChonCuaHang_PC.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_PC.Enabled = false;
                cboChonCuaHang_HD.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_HD.Enabled = false;
                cboChonCuaHang_Vang.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_Vang.Enabled = false;
            }
            else
            {
                cboChonCuaHang_PC.SelectedIndex = 0;
                cboChonCuaHang_HD.SelectedIndex = 0;
                cboChonCuaHang_Vang.SelectedIndex = 0;
            }

            cboChonCuaHang_NV.SelectedIndexChanged += cboChonCuaHang_NV_SelectedIndexChanged;
            cboChonCuaHang_PC.SelectedIndexChanged += cboChonCuaHang_PC_SelectedIndexChanged;
            cboChonCuaHang_HD.SelectedIndexChanged += cboChonCuaHang_HD_SelectedIndexChanged;
            cboChonCuaHang_Vang.SelectedIndexChanged += cboChonCuaHang_Vang_SelectedIndexChanged;

            dtpThangTinhLuong.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            LoadCboCuaHang_TinhLuong();
            LoadData_PhuCap();
            LoadData_CaLam();
            LoadData_PhanCong(GetCurrentStoreIdForPC());
            LoadData_HopDong(GetCurrentStoreIdForHD());
            InitializeContractContextMenu();
            LoadData_ChamCongVang(GetCurrentStoreIdForVang());

            if (_userSession.Role != "Admin" && !string.IsNullOrEmpty(_userSession.IdStore))
            {
                LoadData_NhanVienTheoCuaHang(_userSession.IdStore);
            }
        }

        #region Lọc nhân viên theo cửa hàng
        private void LoadDanhSachCuaHang()
        {
            try
            {
                var result = _storeService.GetAll();
                if (result != null && result.Succeeded && result.Data != null)
                {
                    cboChonCuaHang_NV.DataSource = result.Data.ToList();
                    cboChonCuaHang_NV.DisplayMember = "StoreName";
                    cboChonCuaHang_NV.ValueMember = "StoreId";
                }
                else
                {
                    MessageBox.Show("Không thể tải danh sách cửa hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cboChonCuaHang_NV.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải danh sách cửa hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cboChonCuaHang_NV_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboChonCuaHang_NV.SelectedValue == null) return;
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

            var list = _employeeService.GetEmployeeByStore(storeId, pageNumber, pageSize);
            if (!list.Succeeded || list.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách nhân viên theo cửa hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _totalPage_NV = (long)Math.Ceiling((double)list.TotalCount / pageSize);

            foreach (var item in list.Data)
            {
                string genderText = item.Gender ? "Nam" : "Nữ";
                dt.Rows.Add(item.EmployeeId, item.StoreId, item.FullName, genderText,
                            item.BirthDate.ToString("dd/MM/yyyy"), item.Phone, item.Position, item.EmploymentType);
            }

            dgvDuLieu_NhanVien.DataSource = dt;
            ApplyGridStyle(dgvDuLieu_NhanVien);
        }
        #endregion

        #region Quản lý nhân viên
        private void LoadData_NhanVien(int pageNumber = 1, int pageSize = 20)
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

            var list = _employeeService.GetEmployee(pageNumber, pageSize);
            if (!list.Succeeded || list.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _totalPage_NV = (long)Math.Ceiling((double)list.TotalCount / pageSize);

            foreach (var item in list.Data)
            {
                string genderText = item.Gender ? "Nam" : "Nữ";
                dt.Rows.Add(item.EmployeeId, item.StoreId, item.FullName, genderText,
                            item.BirthDate.ToString("dd/MM/yyyy"), item.Phone, item.Position, item.EmploymentType);
            }

            dgvDuLieu_NhanVien.DataSource = dt;
            dgvDuLieu_NhanVien.AllowUserToAddRows = false;
            dgvDuLieu_NhanVien.ReadOnly = true;
            dgvDuLieu_NhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_NhanVien.Columns["Sửa"] == null)
            {
                var btnSửa = new DataGridViewButtonColumn
                {
                    Name = "Sửa",
                    HeaderText = "Sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_NhanVien.Columns.Add(btnSửa);
            }
            if (dgvDuLieu_NhanVien.Columns["Xoá"] == null)
            {
                var btnXoá = new DataGridViewButtonColumn
                {
                    Name = "Xoá",
                    HeaderText = "Xoá",
                    Text = "Xoá",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_NhanVien.Columns.Add(btnXoá);
            }

            ApplyGridStyle(dgvDuLieu_NhanVien);
            btnTrangTruocNV.Enabled = pageNumber > 1;
            btnTrangSauNV.Enabled = pageNumber < _totalPage_NV;
        }

        private void dgvDuLieu_NhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string employeeId = dgvDuLieu_NhanVien.Rows[e.RowIndex].Cells["MaNhanVien"].Value.ToString();

            if (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Sửa")
            {
                var frmChucNangNhanVien = _container.Resolve<frmChucNang_NhanVien>(
                    new ParameterOverride("employeeId", employeeId),
                    new ParameterOverride("currentUserRole", _userSession.Role),
                    new ParameterOverride("currentStoreId", _userSession.IdStore));
                frmChucNangNhanVien.DataChanged += (s, ev) =>
                {
                    int currentPage = int.TryParse(txtSoTrangNV.Text, out int page) ? page : 1;
                    ReloadEmployeeData(currentPage);
                };
                frmChucNangNhanVien.ShowDialog();
            }
            else if (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Xoá")
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xóa nhân viên {employeeId}?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    var removeResult = _employeeService.RemoveEmployee(employeeId);
                    if (removeResult.Succeeded)
                    {
                        MessageBox.Show("Xóa nhân viên thành công!");
                        int currentPage = int.TryParse(txtSoTrangNV.Text, out int page) ? page : 1;
                        ReloadEmployeeData(currentPage);
                    }
                    else
                    {
                        MessageBox.Show($"Xóa nhân viên thất bại: {removeResult.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnThemNhanVien_Click(object sender, EventArgs e)
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var frmChucNangNV = childContainer.Resolve<frmChucNang_NhanVien>(
                    new ParameterOverride("employeeId", null),
                    new ParameterOverride("currentUserRole", _userSession.Role),
                    new ParameterOverride("currentStoreId", _userSession.IdStore));
                frmChucNangNV.DataChanged += (s, ev) =>
                {
                    int currentPage = int.TryParse(txtSoTrangNV.Text, out int page) ? page : 1;
                    ReloadEmployeeData(currentPage);
                };
                frmChucNangNV.ShowDialog();
            }
        }

        private void btnTrangSauNV_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangNV.Text);
            if (currentPage < _totalPage_NV)
            {
                int pageNumber = currentPage + 1;
                txtSoTrangNV.Text = pageNumber.ToString();
                LoadData_NhanVien(pageNumber);
            }
        }

        private void btnTrangTruocNV_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangNV.Text);
            if (currentPage > 1)
            {
                int pageNumber = currentPage - 1;
                txtSoTrangNV.Text = pageNumber.ToString();
                LoadData_NhanVien(pageNumber);
            }
        }

        private void ReloadEmployeeData(int pageNumber = 1, int pageSize = 20)
        {
            if (_userSession.Role == "Admin")
            {
                LoadData_NhanVien(pageNumber, pageSize);
            }
            else
            {
                LoadData_NhanVienTheoCuaHang(_userSession.IdStore, pageNumber, pageSize);
            }
        }

        private void ibtnThungRac_NV_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_NhanVien>();
            frmThungRac.datachanged += (s, ev) =>
            {
                int currentPage = int.TryParse(txtSoTrangNV.Text, out int page) ? page : 1;
                ReloadEmployeeData(currentPage);
            };
            frmThungRac.ShowDialog();
        }

        private void ibtnLamMoiNV_Click(object sender, EventArgs e)
        {
            LoadData_NhanVienTheoCuaHang(_userSession.IdStore);
        }
        #endregion

        #region Quản lý hợp đồng lương

        private void InitializeContractContextMenu()
        {
            ContextMenuStrip contextMenu = new ContextMenuStrip();
            ToolStripMenuItem menuViewAllowances = new ToolStripMenuItem("Xem phụ cấp");
            menuViewAllowances.Click += MenuViewAllowances_Click;
            contextMenu.Items.Add(menuViewAllowances);
            dgvDuLieu_HopDong.ContextMenuStrip = contextMenu;
        }

        private void MenuViewAllowances_Click(object sender, EventArgs e)
        {
            if (dgvDuLieu_HopDong.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hợp đồng để xem phụ cấp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var row = dgvDuLieu_HopDong.SelectedRows[0];
            string contractId = row.Cells["MaHopDong"].Value.ToString();
            string employeeId = row.Cells["MaNhanVien"].Value.ToString();

            // Kiểm tra nhân viên có phải Fulltime không
            var empResult = _employeeService.GetEmployeeByID(employeeId);
            if (!empResult.Succeeded || empResult.Data == null || empResult.Data.EmploymentType != "Fulltime")
            {
                MessageBox.Show("Chỉ nhân viên Fulltime mới có phụ cấp!", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Mở form xem phụ cấp (chế độ chỉ xem)
            var frmXemPhuCap = _container.Resolve<frmXemPhuCap_HopDongLuong>(
                new ParameterOverride("contractId", contractId));

            frmXemPhuCap.ShowDialog();
        }
        private string GetCurrentStoreIdForHD()
        {
            return _userSession.Role == "Admin"
                ? cboChonCuaHang_HD.SelectedValue?.ToString()
                : _userSession.IdStore;
        }

        private void cboChonCuaHang_HD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_userSession.Role != "Admin") return;
            if (cboChonCuaHang_HD.SelectedValue == null) return;
            LoadData_HopDong(GetCurrentStoreIdForHD(), 1);
        }

        private void LoadData_HopDong(string storeId = null, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHopDong");
            dt.Columns.Add("MaNhanVien");
            dt.Columns.Add("HoTen");
            dt.Columns.Add("LuongCoBan");
            dt.Columns.Add("LuongGio");
            dt.Columns.Add("NgayBatDau");
            dt.Columns.Add("NgayKetThuc");

            List<string> employeeIds = new List<string>();
            if (!string.IsNullOrEmpty(storeId))
            {
                var empResult = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
                if (empResult.Succeeded && empResult.Data != null)
                    employeeIds = empResult.Data.Select(x => x.EmployeeId).ToList();
            }

            var allContracts = _salaryContractService.GetPaged(1, 10000);
            if (!allContracts.Succeeded || allContracts.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách hợp đồng lương!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var filtered = string.IsNullOrEmpty(storeId)
                ? allContracts.Data
                : allContracts.Data.Where(x => employeeIds.Contains(x.EmployeeId)).ToList();

            var total = filtered.Count;
            var paged = filtered.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            _totalPage_HD = (long)Math.Ceiling((double)total / pageSize);

            foreach (var item in paged)
            {
                string hoTen = "N/A";
                var empResult = _employeeService.GetEmployeeByID(item.EmployeeId);
                if (empResult.Succeeded && empResult.Data != null)
                    hoTen = empResult.Data.FullName;

                dt.Rows.Add(
                    item.ContractId,
                    item.EmployeeId,
                    hoTen,
                    item.BasicSalary?.ToString("N0") ?? "-",
                    item.HourlyRate?.ToString("N0") ?? "-",
                    item.StartDate.ToString("dd/MM/yyyy"),
                    item.EndDate?.ToString("dd/MM/yyyy") ?? "Đang hiệu lực"
                );
            }

            dgvDuLieu_HopDong.DataSource = dt;
            dgvDuLieu_HopDong.AllowUserToAddRows = false;
            dgvDuLieu_HopDong.ReadOnly = true;
            dgvDuLieu_HopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_HopDong.Columns["Sửa"] == null)
            {
                var btnSửa = new DataGridViewButtonColumn
                {
                    Name = "Sửa",
                    HeaderText = "Sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_HopDong.Columns.Add(btnSửa);
            }
            if (dgvDuLieu_HopDong.Columns["Xoá"] == null)
            {
                var btnXoá = new DataGridViewButtonColumn
                {
                    Name = "Xoá",
                    HeaderText = "Xoá",
                    Text = "Xoá",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_HopDong.Columns.Add(btnXoá);
            }

            ApplyGridStyle(dgvDuLieu_HopDong);
            btnTrangTruocHD.Enabled = pageNumber > 1;
            btnTrangSauHD.Enabled = pageNumber < _totalPage_HD;
            txtSoTrangHD.Text = pageNumber.ToString();
        }

        private void dgvDuLieu_HopDong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string contractId = dgvDuLieu_HopDong.Rows[e.RowIndex].Cells["MaHopDong"].Value.ToString();

            if (dgvDuLieu_HopDong.Columns[e.ColumnIndex].Name == "Sửa")
            {
                var frmChucNang = _container.Resolve<frmChucNang_HopDongLuong>(
                    new ParameterOverride("contractId", contractId));
                frmChucNang.DataChanged += (s, ev) =>
                {
                    int currentPage = int.TryParse(txtSoTrangHD.Text, out int page) ? page : 1;
                    LoadData_HopDong(GetCurrentStoreIdForHD(), currentPage);
                };
                frmChucNang.ShowDialog();
            }
            else if (dgvDuLieu_HopDong.Columns[e.ColumnIndex].Name == "Xoá")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa hợp đồng {contractId}?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var result = _salaryContractService.SoftDelete(contractId);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("Xóa hợp đồng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        int currentPage = int.TryParse(txtSoTrangHD.Text, out int page) ? page : 1;
                        LoadData_HopDong(GetCurrentStoreIdForHD(), currentPage);
                    }
                    else
                    {
                        MessageBox.Show($"Xóa thất bại: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnTrangSauHD_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangHD.Text);
            if (currentPage < _totalPage_HD)
            {
                int pageNumber = currentPage + 1;
                txtSoTrangHD.Text = pageNumber.ToString();
                LoadData_HopDong(GetCurrentStoreIdForHD(), pageNumber);
            }
        }

        private void btnTrangTruocHD_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangHD.Text);
            if (currentPage > 1)
            {
                int pageNumber = currentPage - 1;
                txtSoTrangHD.Text = pageNumber.ToString();
                LoadData_HopDong(GetCurrentStoreIdForHD(), pageNumber);
            }
        }

        private bool HasValidCurrentContract(string employeeId)
        {
            var result = _salaryContractService.GetCurrentContractByEmployeeId(employeeId);
            if (!result.Succeeded || result.Data == null) return false;

            var contract = result.Data;
            return contract.EndDate == null || contract.EndDate >= DateTime.Today;
        }

        private void BtnTaoHopDongMoi_Click(object sender, EventArgs e)
        {
            // NGĂN MỞ KHI ĐANG TẠO HỢP ĐỒNG TỪ NHÂN VIÊN
            if (frmChucNang_HopDongLuong._isProcessingEmployeeContract)
                return;

            var frmHopDong = _container.Resolve<frmChucNang_HopDongLuong>(
                new ParameterOverride("isFromRenewButton", true));

            frmHopDong.DataChanged += (s, ev) => { /* load lại data */ };
            frmHopDong.ShowDialog();
        }

        private void ibtnThungRacHDL_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Quản lý phụ cấp
        private void LoadData_PhuCap(int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhuCap");
            dt.Columns.Add("TenPhuCap");
            dt.Columns.Add("MucMacDinh");

            var list = _allowanceService.GetAllowance(pageNumber, pageSize);
            if (!list.Succeeded || list.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách phụ cấp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _totalPage_PC = (long)Math.Ceiling((double)list.TotalCount / pageSize);

            foreach (var item in list.Data)
            {
                dt.Rows.Add(item.AllowanceId, item.AllowanceName, item.DefaultAmount);
            }

            dgvDuLieu_PhuCap.DataSource = dt;
            dgvDuLieu_PhuCap.AllowUserToAddRows = false;
            dgvDuLieu_PhuCap.ReadOnly = true;
            dgvDuLieu_PhuCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_PhuCap.Columns["Sửa"] == null)
            {
                var btnSửa = new DataGridViewButtonColumn { Name = "Sửa", HeaderText = "Sửa", Text = "Sửa", UseColumnTextForButtonValue = true };
                dgvDuLieu_PhuCap.Columns.Add(btnSửa);
            }
            if (dgvDuLieu_PhuCap.Columns["Xoá"] == null)
            {
                var btnXoá = new DataGridViewButtonColumn { Name = "Xoá", HeaderText = "Xoá", Text = "Xoá", UseColumnTextForButtonValue = true };
                dgvDuLieu_PhuCap.Columns.Add(btnXoá);
            }

            ApplyGridStyle(dgvDuLieu_PhuCap);
            btnTrangTruocPK.Enabled = pageNumber > 1;
            btnTrangSauPK.Enabled = pageNumber < _totalPage_PC;
        }

        private void ibtnLamMoiPC_Click(object sender, EventArgs e)
        {
            LoadData_PhuCap();
        }

        private void dgvDuLieu_PhuCap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string allowanceId = dgvDuLieu_PhuCap.Rows[e.RowIndex].Cells["MaPhuCap"].Value.ToString();

            if (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Sửa")
            {
                var frmChucNangPhuCap = _container.Resolve<frmChucNang_PhuCap>(
                    new ParameterOverride("allowanceId", allowanceId));
                frmChucNangPhuCap.DataChanged += (s, ev) =>
                {
                    int currentPage = int.TryParse(txtSoTrangPK.Text, out int page) ? page : 1;
                    LoadData_PhuCap(currentPage);
                };
                frmChucNangPhuCap.ShowDialog();
            }
            else if (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Xoá")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa phụ cấp {allowanceId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var removeResult = _allowanceService.RemoveAllowance(allowanceId);
                    if (removeResult.Succeeded)
                    {
                        MessageBox.Show("Xóa thành công!");
                        int currentPage = int.TryParse(txtSoTrangPK.Text, out int page) ? page : 1;
                        LoadData_PhuCap(currentPage);
                    }
                    else
                    {
                        MessageBox.Show($"Xóa phụ cấp thất bại: {removeResult.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnThemPhuCap_Click(object sender, EventArgs e)
        {
            var frmChucNangPK = _container.Resolve<frmChucNang_PhuCap>();
            frmChucNangPK.DataChanged += (s, ev) =>
            {
                int currentPage = int.TryParse(txtSoTrangPK.Text, out int page) ? page : 1;
                LoadData_PhuCap(currentPage);
            };
            frmChucNangPK.ShowDialog();
        }

        private void btnTrangSauPK_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangPK.Text);
            if (currentPage < _totalPage_PC)
            {
                int pageNumber = currentPage + 1;
                txtSoTrangPK.Text = pageNumber.ToString();
                LoadData_PhuCap(pageNumber);
            }
        }

        private void btnTrangTruocPK_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangPK.Text);
            if (currentPage > 1)
            {
                int pageNumber = currentPage - 1;
                txtSoTrangPK.Text = pageNumber.ToString();
                LoadData_PhuCap(pageNumber);
            }
        }

        private void ibtnThungRac_PC_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_PhuCap>();
            frmThungRac.datachanged += (s, ev) =>
            {
                int currentPage = int.TryParse(txtSoTrangPK.Text, out int page) ? page : 1;
                LoadData_PhuCap(currentPage);
            };
            frmThungRac.ShowDialog();
        }
        #endregion

        #region Quản lý ca làm
        private void LoadData_CaLam(int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaCaLam");
            dt.Columns.Add("TenCaLam");
            dt.Columns.Add("GioBatDau");
            dt.Columns.Add("GioKetThuc");

            var list = _shiftService.GetShift(pageNumber, pageSize);
            if (!list.Succeeded || list.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách ca làm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _totalPage_CL = (long)Math.Ceiling((double)list.TotalCount / pageSize);

            foreach (var item in list.Data)
            {
                dt.Rows.Add(item.ShiftId, item.ShiftName, item.StartTime, item.EndTime);
            }

            dgvDuLieu_CaLam.DataSource = dt;
            dgvDuLieu_CaLam.AllowUserToAddRows = false;
            dgvDuLieu_CaLam.ReadOnly = true;
            dgvDuLieu_CaLam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_CaLam.Columns["Sửa"] == null)
            {
                var btnSửa = new DataGridViewButtonColumn { Name = "Sửa", HeaderText = "Sửa", Text = "Sửa", UseColumnTextForButtonValue = true };
                dgvDuLieu_CaLam.Columns.Add(btnSửa);
            }
            if (dgvDuLieu_CaLam.Columns["Xoá"] == null)
            {
                var btnXoá = new DataGridViewButtonColumn { Name = "Xoá", HeaderText = "Xoá", Text = "Xoá", UseColumnTextForButtonValue = true };
                dgvDuLieu_CaLam.Columns.Add(btnXoá);
            }

            ApplyGridStyle(dgvDuLieu_CaLam);
            btnTrangTruocCL.Enabled = pageNumber > 1;
            btnTrangSauCL.Enabled = pageNumber < _totalPage_CL;
        }

        private void dgvDuLieu_CaLam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string shiftId = dgvDuLieu_CaLam.Rows[e.RowIndex].Cells["MaCaLam"].Value.ToString();

            if (dgvDuLieu_CaLam.Columns[e.ColumnIndex].Name == "Sửa")
            {
                var frmChucNangCaLam = _container.Resolve<frmChucNang_CaLamViec>(
                    new ParameterOverride("shiftId", shiftId));
                frmChucNangCaLam.DataChanged += (s, ev) =>
                {
                    int currentPage = int.TryParse(txtSoTrangCL.Text, out int page) ? page : 1;
                    LoadData_CaLam(currentPage);
                };
                frmChucNangCaLam.ShowDialog();
            }
            else if (dgvDuLieu_CaLam.Columns[e.ColumnIndex].Name == "Xoá")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa ca làm {shiftId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var removeResult = _shiftService.RemoveShift(shiftId);
                    if (removeResult.Succeeded)
                    {
                        MessageBox.Show("Xóa thành công!");
                        int currentPage = int.TryParse(txtSoTrangCL.Text, out int page) ? page : 1;
                        LoadData_CaLam(currentPage);
                    }
                    else
                    {
                        MessageBox.Show($"Xóa ca làm thất bại: {removeResult.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnThemCaLam_Click(object sender, EventArgs e)
        {
            var frmChucNangCL = _container.Resolve<frmChucNang_CaLamViec>();
            frmChucNangCL.DataChanged += (s, ev) =>
            {
                int currentPage = int.TryParse(txtSoTrangCL.Text, out int page) ? page : 1;
                LoadData_CaLam(currentPage);
            };
            frmChucNangCL.ShowDialog();
        }

        private void btnTrangSauCL_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangCL.Text);
            if (currentPage < _totalPage_CL)
            {
                int pageNumber = currentPage + 1;
                txtSoTrangCL.Text = pageNumber.ToString();
                LoadData_CaLam(pageNumber);
            }
        }

        private void btnTrangTruocCL_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangCL.Text);
            if (currentPage > 1)
            {
                int pageNumber = currentPage - 1;
                txtSoTrangCL.Text = pageNumber.ToString();
                LoadData_CaLam(pageNumber);
            }
        }
        #endregion

        #region Quản lý phân công ca làm
        private string GetCurrentStoreIdForPC()
        {
            return _userSession.Role == "Admin"
                ? cboChonCuaHang_PC.SelectedValue?.ToString()
                : _userSession.IdStore;
        }

        private void cboChonCuaHang_PC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_userSession.Role != "Admin") return;
            if (cboChonCuaHang_PC.SelectedValue == null) return;
            LoadData_PhanCong(GetCurrentStoreIdForPC(), 1);
        }

        private void LoadData_PhanCong(string storeId = null, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhanCong");
            dt.Columns.Add("MaNhanVien");
            dt.Columns.Add("TenNhanVien");
            dt.Columns.Add("MaCa");
            dt.Columns.Add("NgayLam");
            dt.Columns.Add("GhiChu");

            List<string> employeeIds = new List<string>();
            if (!string.IsNullOrEmpty(storeId))
            {
                var empResult = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
                if (empResult.Succeeded && empResult.Data != null)
                    employeeIds = empResult.Data.Select(x => x.EmployeeId).ToList();
            }

            var allAssignmentsResult = _shiftAssignmentService.GetPaged(1, 10000);
            if (!allAssignmentsResult.Succeeded || allAssignmentsResult.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách phân công ca làm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var filtered = string.IsNullOrEmpty(storeId)
                ? allAssignmentsResult.Data
                : allAssignmentsResult.Data.Where(x => employeeIds.Contains(x.EmployeeId)).ToList();

            var total = filtered.Count;
            var paged = filtered.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            _totalPage_PhanCong = (long)Math.Ceiling((double)total / pageSize);

            foreach (var item in paged)
            {
                string tenNhanVien = "N/A";
                string tenCa = "N/A";

                var empResult = _employeeService.GetEmployeeByID(item.EmployeeId);
                if (empResult.Succeeded && empResult.Data != null)
                    tenNhanVien = empResult.Data.FullName ?? "N/A";

                if (!string.IsNullOrEmpty(item.ShiftId))
                {
                    var shiftResult = _shiftService.GetShiftByID(item.ShiftId);
                    if (shiftResult.Succeeded && shiftResult.Data != null)
                        tenCa = shiftResult.Data.ShiftName ?? "N/A";
                }

                dt.Rows.Add(
                    item.Id,
                    item.EmployeeId,
                    tenNhanVien,
                    tenCa,
                    item.WorkDate.ToString("dd/MM/yyyy"),
                    string.IsNullOrEmpty(item.Note) ? "Ca linh hoạt" : item.Note
                );
            }

            dgvDuLieu_PhanCong.DataSource = dt;
            dgvDuLieu_PhanCong.AllowUserToAddRows = false;
            dgvDuLieu_PhanCong.ReadOnly = true;
            dgvDuLieu_PhanCong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_PhanCong.Columns["Sửa"] == null)
            {
                var btnSửa = new DataGridViewButtonColumn
                {
                    Name = "Sửa",
                    HeaderText = "Sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_PhanCong.Columns.Add(btnSửa);
            }
            if (dgvDuLieu_PhanCong.Columns["Xoá"] == null)
            {
                var btnXoá = new DataGridViewButtonColumn
                {
                    Name = "Xoá",
                    HeaderText = "Xoá",
                    Text = "Xoá",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_PhanCong.Columns.Add(btnXoá);
            }

            if (dgvDuLieu_PhanCong.Columns["MaPhanCong"] != null)
                dgvDuLieu_PhanCong.Columns["MaPhanCong"].Visible = false;

            ApplyGridStyle(dgvDuLieu_PhanCong);
            btnTrangTruocPC.Enabled = pageNumber > 1;
            btnTrangSauPC.Enabled = pageNumber < _totalPage_PhanCong;
            txtSoTrangPC.Text = pageNumber.ToString();
        }

        private void dgvDuLieu_PhanCong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string assignmentId = dgvDuLieu_PhanCong.Rows[e.RowIndex].Cells["MaPhanCong"].Value.ToString();

            if (dgvDuLieu_PhanCong.Columns[e.ColumnIndex].Name == "Sửa")
            {
                var frm = _container.Resolve<frmChucNang_PhanCaLam>(
                    new ParameterOverride("assignmentId", assignmentId));
                frm.DataChanged += (s, ev) =>
                {
                    string storeId = GetCurrentStoreIdForPC();
                    int page = int.TryParse(txtSoTrangPC.Text, out int p) ? p : 1;
                    LoadData_PhanCong(storeId, page);
                };
                frm.ShowDialog();
            }
            else if (dgvDuLieu_PhanCong.Columns[e.ColumnIndex].Name == "Xoá")
            {
                if (MessageBox.Show($"Xóa phân công {assignmentId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var result = _shiftAssignmentService.SoftDelete(assignmentId);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("Xóa thành công!");
                        string storeId = GetCurrentStoreIdForPC();
                        int page = int.TryParse(txtSoTrangPC.Text, out int p) ? p : 1;
                        LoadData_PhanCong(storeId, page);
                    }
                    else
                    {
                        MessageBox.Show($"Xóa thất bại: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnThemPhanCong_Click(object sender, EventArgs e)
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var frm = childContainer.Resolve<frmChucNang_PhanCaLam>(
                    new ParameterOverride("assignmentId", null),
                    new ParameterOverride("currentUserRole", _userSession.Role),
                    new ParameterOverride("currentStoreId", GetCurrentStoreIdForPC())
                );
                frm.DataChanged += (s, ev) =>
                {
                    string storeId = GetCurrentStoreIdForPC();
                    LoadData_PhanCong(storeId);
                };
                frm.ShowDialog();
            }
        }

        private void btnTrangSauPC_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangPC.Text);
            if (currentPage < _totalPage_PhanCong)
            {
                int pageNumber = currentPage + 1;
                txtSoTrangPC.Text = pageNumber.ToString();
                string storeId = GetCurrentStoreIdForPC();
                LoadData_PhanCong(storeId, pageNumber);
            }
        }

        private void btnTrangTruocPC_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangPC.Text);
            if (currentPage > 1)
            {
                int pageNumber = currentPage - 1;
                txtSoTrangPC.Text = pageNumber.ToString();
                string storeId = GetCurrentStoreIdForPC();
                LoadData_PhanCong(storeId, pageNumber);
            }
        }
        #endregion

        #region Chấm công vắng
        private string GetCurrentStoreIdForVang()
        {
            return _userSession.Role == "Admin"
                ? cboChonCuaHang_Vang.SelectedValue?.ToString()
                : _userSession.IdStore;
        }

        private void cboChonCuaHang_Vang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_userSession.Role != "Admin") return;
            if (cboChonCuaHang_Vang.SelectedValue == null) return;
            LoadData_ChamCongVang(GetCurrentStoreIdForVang(), 1);
        }

        private void LoadData_ChamCongVang(string storeId = null, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaVang");
            dt.Columns.Add("MaNhanVien");
            dt.Columns.Add("HoTen");
            dt.Columns.Add("CaLam");
            dt.Columns.Add("Ngay");
            dt.Columns.Add("Phep");
            dt.Columns.Add("LyDo");
            dt.Columns.Add("CoLuong");

            List<string> employeeIds = new List<string>();
            if (!string.IsNullOrEmpty(storeId))
            {
                var empResult = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
                if (empResult.Succeeded && empResult.Data != null)
                    employeeIds = empResult.Data.Select(x => x.EmployeeId).ToList();
            }

            var allAbsencesResult = _absenceService.GetAbsence(1, 10000);
            if (!allAbsencesResult.Succeeded || allAbsencesResult.Data == null || !allAbsencesResult.Data.Any())
            {
                dgvDuLieu_Vang.DataSource = dt;
                ApplyGridStyle(dgvDuLieu_Vang);
                _totalPage_Vang = 1;
                txtSoTrangVang.Text = "1";
                btnTrangTruocVang.Enabled = false;
                btnTrangSauVang.Enabled = false;
                return;
            }

            var allAbsences = allAbsencesResult.Data;
            var filtered = string.IsNullOrEmpty(storeId)
                ? allAbsences
                : allAbsences.Where(x => employeeIds.Contains(x.EmployeeId)).ToList();

            var totalRecords = filtered.Count;
            _totalPage_Vang = (long)Math.Ceiling(totalRecords / (double)pageSize);
            var pagedData = filtered
                .OrderByDescending(x => x.WorkDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var item in pagedData)
            {
                string hoTen = "N/A";
                string caLam = "N/A";

                var empResult = _employeeService.GetEmployeeByID(item.EmployeeId);
                if (empResult.Succeeded && empResult.Data != null)
                    hoTen = empResult.Data.FullName ?? "N/A";

                if (!string.IsNullOrEmpty(item.ShiftId))
                {
                    var shiftResult = _shiftService.GetShiftByID(item.ShiftId);
                    if (shiftResult.Succeeded && shiftResult.Data != null)
                        caLam = shiftResult.Data.ShiftName ?? "N/A";
                }

                dt.Rows.Add(
                    item.AbsenceId,
                    item.EmployeeId,
                    hoTen,
                    caLam,
                    item.WorkDate.ToString("dd/MM/yyyy"),
                    item.IsLeaveOfAbsence ? "Có phép" : "Không phép",
                    item.Reason ?? "-",
                    item.IsPaid ? "Có lương" : "Không lương"
                );
            }

            dgvDuLieu_Vang.DataSource = dt;
            dgvDuLieu_Vang.AllowUserToAddRows = false;
            dgvDuLieu_Vang.ReadOnly = true;
            dgvDuLieu_Vang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_Vang.Columns["Sửa"] == null)
            {
                var colSửa = new DataGridViewButtonColumn
                {
                    Name = "Sửa",
                    HeaderText = "Sửa",
                    Text = "Sửa",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_Vang.Columns.Add(colSửa);
            }
            if (dgvDuLieu_Vang.Columns["Xoá"] == null)
            {
                var colXoá = new DataGridViewButtonColumn
                {
                    Name = "Xoá",
                    HeaderText = "Xoá",
                    Text = "Xoá",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_Vang.Columns.Add(colXoá);
            }

            if (dgvDuLieu_Vang.Columns["MaVang"] != null)
                dgvDuLieu_Vang.Columns["MaVang"].Visible = false;

            ApplyGridStyle(dgvDuLieu_Vang);
            btnTrangTruocVang.Enabled = pageNumber > 1;
            btnTrangSauVang.Enabled = pageNumber < _totalPage_Vang;
            txtSoTrangVang.Text = $"{pageNumber} / {_totalPage_Vang}";
        }

        private void dgvDuLieu_Vang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string absenceId = dgvDuLieu_Vang.Rows[e.RowIndex].Cells["MaVang"].Value.ToString();

            if (dgvDuLieu_Vang.Columns[e.ColumnIndex].Name == "Sửa")
            {
                var frm = _container.Resolve<frmChucNang_ChamCongVang>(
                    new ParameterOverride("absenceId", absenceId),
                    new ParameterOverride("currentStoreId", GetCurrentStoreIdForVang()));
                frm.DataChanged += (s, ev) =>
                {
                    int page = int.TryParse(txtSoTrangVang.Text.Split('/')[0].Trim(), out int p) ? p : 1;
                    LoadData_ChamCongVang(GetCurrentStoreIdForVang(), page);
                };
                frm.ShowDialog();
            }
            else if (dgvDuLieu_Vang.Columns[e.ColumnIndex].Name == "Xoá")
            {
                if (MessageBox.Show($"Xóa chấm công vắng {absenceId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var result = _absenceService.RemoveAbsence(absenceId);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("Xóa thành công!");
                        int page = int.TryParse(txtSoTrangVang.Text.Split('/')[0].Trim(), out int p) ? p : 1;
                        LoadData_ChamCongVang(GetCurrentStoreIdForVang(), page);
                    }
                    else
                    {
                        MessageBox.Show($"Xóa thất bại: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnThemVang_Click(object sender, EventArgs e)
        {
            var frm = _container.Resolve<frmChucNang_ChamCongVang>(
                new ParameterOverride("absenceId", null),
                new ParameterOverride("currentStoreId", GetCurrentStoreIdForVang()));
            frm.DataChanged += (s, ev) =>
            {
                LoadData_ChamCongVang(GetCurrentStoreIdForVang());
            };
            frm.ShowDialog();
        }

        private void btnTrangSauVang_Click(object sender, EventArgs e)
        {
            int currentPage = int.TryParse(txtSoTrangVang.Text.Split('/')[0].Trim(), out int p) ? p : 1;
            if (currentPage < _totalPage_Vang)
            {
                LoadData_ChamCongVang(GetCurrentStoreIdForVang(), currentPage + 1);
            }
        }

        private void btnTrangTruocVang_Click(object sender, EventArgs e)
        {
            int currentPage = int.TryParse(txtSoTrangVang.Text.Split('/')[0].Trim(), out int p) ? p : 1;
            if (currentPage > 1)
            {
                LoadData_ChamCongVang(GetCurrentStoreIdForVang(), currentPage - 1);
            }
        }
        #endregion

        #region Tính lương
        private void LoadCboCuaHang_TinhLuong()
        {
            cboCuaHang_TinhLuong.DataSource = cboChonCuaHang_NV.DataSource;
            cboCuaHang_TinhLuong.DisplayMember = "StoreName";
            cboCuaHang_TinhLuong.ValueMember = "StoreId";
            if (_userSession.Role != "Admin")
            {
                cboCuaHang_TinhLuong.SelectedValue = _userSession.IdStore;
                cboCuaHang_TinhLuong.Enabled = false;
            }
        }

        private void btnTinhLuong_Click(object sender, EventArgs e)
        {
            string storeId = _userSession.Role == "Admin"
                ? cboCuaHang_TinhLuong.SelectedValue?.ToString()
                : _userSession.IdStore;
            string monthYear = dtpThangTinhLuong.Value.ToString("yyyy-MM");

            if (string.IsNullOrEmpty(storeId))
            {
                MessageBox.Show("Vui lòng chọn cửa hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            TinhLuongVaHienThi(storeId, monthYear);
        }

        private void TinhLuongVaHienThi(string storeId, string monthYear)
        {
            var empResult = _employeeService.GetEmployeeByStore(storeId, 1, 1000);
            if (!empResult.Succeeded || empResult.Data == null || !empResult.Data.Any())
            {
                MessageBox.Show("Không có nhân viên nào ở cửa hàng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dgvDuLieu_TinhLuong.DataSource = null;
                lblTongNhanVien.Text = "Tổng số nhân viên: 0";
                lblTongChiPhiLuong.Text = "Tổng chi phí lương: 0 ₫";
                return;
            }

            var employees = empResult.Data;
            var dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã NV");
            dt.Columns.Add("Họ tên");
            dt.Columns.Add("Loại NV");
            dt.Columns.Add("Lương cơ bản", typeof(decimal));
            dt.Columns.Add("Giờ làm", typeof(int));
            dt.Columns.Add("Phụ cấp", typeof(decimal));
            dt.Columns.Add("Thưởng", typeof(decimal));
            dt.Columns.Add("Khấu trừ (vắng)", typeof(decimal));
            dt.Columns.Add("Thực lãnh", typeof(decimal));

            decimal tongQuyLuong = 0;
            int stt = 1;

            foreach (var emp in employees)
            {
                var contractResult = _salaryContractService.GetCurrentContractByEmployeeId(emp.EmployeeId);
                if (!contractResult.Succeeded || contractResult.Data == null) continue;
                var contract = contractResult.Data;

                int totalHours = 0;
                decimal luongGio = 0;
                if (emp.EmploymentType == "Parttime")
                {
                    var assignments = _shiftAssignmentService.GetByEmployeeAndMonth(emp.EmployeeId, monthYear);
                    if (assignments.Succeeded && assignments.Data != null)
                    {
                        foreach (var ass in assignments.Data)
                        {
                            var shift = _shiftService.GetShiftByID(ass.ShiftId);
                            if (shift.Succeeded && shift.Data != null)
                            {
                                TimeSpan duration = shift.Data.EndTime - shift.Data.StartTime;
                                totalHours += (int)Math.Ceiling(duration.TotalHours);
                            }
                        }
                        luongGio = (contract.HourlyRate ?? 0) * totalHours;
                    }
                }

                decimal phuCap = 0;
                var allowances = _salaryContractAllowanceService.GetByContractId(contract.ContractId);
                if (allowances.Succeeded && allowances.Data != null)
                {
                    phuCap = allowances.Data.Sum(a => a.CustomAmount ?? 0);
                }

                decimal bonus = 0, manualDeduction = 0;
                var salaryResult = _salaryService.GetByContract(contract.ContractId);
                if (salaryResult.Succeeded && salaryResult.Data != null)
                {
                    var salaryThisMonth = salaryResult.Data.FirstOrDefault(s => s.MonthYear == monthYear);
                    if (salaryThisMonth != null)
                    {
                        bonus = salaryThisMonth.Bonus;
                        manualDeduction = salaryThisMonth.Deduction;
                    }
                }

                decimal truVang = 0;
                var absences = _absenceService.GetAll();
                if (absences.Succeeded && absences.Data != null)
                {
                    var vangKhongLuong = absences.Data
                        .Where(a => a.EmployeeId == emp.EmployeeId &&
                                   a.WorkDate.Year == dtpThangTinhLuong.Value.Year &&
                                   a.WorkDate.Month == dtpThangTinhLuong.Value.Month &&
                                   !a.IsPaid)
                        .ToList();

                    foreach (var v in vangKhongLuong)
                    {
                        var ass = _shiftAssignmentService.GetById(v.ShiftId);
                        if (ass.Succeeded && ass.Data != null)
                        {
                            var shift = _shiftService.GetShiftByID(ass.Data.ShiftId);
                            if (shift.Succeeded && shift.Data != null)
                            {
                                TimeSpan duration = shift.Data.EndTime - shift.Data.StartTime;
                                int hours = (int)Math.Ceiling(duration.TotalHours);
                                decimal luongCa = (contract.HourlyRate ?? 0) * hours;
                                truVang += luongCa;
                            }
                        }
                    }
                }

                decimal luongCoBan = emp.EmploymentType == "Fulltime" ? (contract.BasicSalary ?? 0) : 0;
                decimal thucLanh = luongCoBan + luongGio + phuCap + bonus - manualDeduction - truVang;
                tongQuyLuong += thucLanh;

                dt.Rows.Add(
                    stt++,
                    emp.EmployeeId,
                    emp.FullName,
                    emp.EmploymentType,
                    luongCoBan,
                    totalHours,
                    phuCap,
                    bonus,
                    truVang + manualDeduction,
                    thucLanh
                );
            }

            dgvDuLieu_TinhLuong.DataSource = dt;
            ApplyGridStyle(dgvDuLieu_TinhLuong);

            foreach (DataGridViewColumn col in dgvDuLieu_TinhLuong.Columns)
            {
                if (col.ValueType == typeof(decimal))
                {
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.ForeColor = Color.DarkBlue;
                }
            }

            dgvDuLieu_TinhLuong.Columns["Thực lãnh"].DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu_TinhLuong.Columns["Thực lãnh"].DefaultCellStyle.ForeColor = Color.DarkGreen;

            lblTongNhanVien.Text = $"Tổng số nhân viên: {employees.Count()} người";
            lblTongChiPhiLuong.Text = $"Tổng chi phí lương: {tongQuyLuong:N0} ₫";
        }
        #endregion

        #region Giao diện DataGridView
        private void ApplyGridStyle(Guna2DataGridView dgvDuLieu)
        {
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

            dgvDuLieu.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvDuLieu.Columns[e.ColumnIndex].Name == "Sửa" ||
                                        dgvDuLieu.Columns[e.ColumnIndex].Name == "Xoá"))
                {
                    e.PaintBackground(e.CellBounds, true);
                    Color backColor = dgvDuLieu.Columns[e.ColumnIndex].Name == "Sửa" ? Color.SeaGreen : Color.IndianRed;
                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);
                    string text = dgvDuLieu.Columns[e.ColumnIndex].Name;
                    TextRenderer.DrawText(e.Graphics, text, new Font("Segoe UI", 9, FontStyle.Bold),
                        e.CellBounds, Color.White, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
            };
        }


        #endregion

        
    }
}