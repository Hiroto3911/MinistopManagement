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
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private readonly IAbsenceService _absenceService;
       

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
            IAbsenceService absenceService, // THÊM
            IUnityContainer container,
            IUserSession userSession)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _allowanceService = allowanceService;
            _shiftService = shiftService;
            _shiftAssignmentService = shiftAssignmentService;
            _salaryContractService = salaryContractService;
            _absenceService = absenceService; // THÊM
            _container = container;
            _userSession = userSession;
        }

        private void frmHienThi_NhanVien_Load(object sender, EventArgs e)
        {
            LoadDanhSachCuaHang();
            LoadData_NhanVien();

            // Tạm ngắt event để tránh gọi nhiều lần
            cboChonCuaHang_NV.SelectedIndexChanged -= cboChonCuaHang_NV_SelectedIndexChanged;
            cboChonCuaHang_PC.SelectedIndexChanged -= cboChonCuaHang_PC_SelectedIndexChanged;
            cboChonCuaHang_HD.SelectedIndexChanged -= cboChonCuaHang_HD_SelectedIndexChanged; // THÊM

            string storeIdload = _userSession.Role == "Admin" ? null : _userSession.IdStore;

            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlNV.TabPages.Remove(tabCaLam);
                tabControlNV.TabPages.Remove(tabPhuCap);
                cboChonCuaHang_NV.SelectedValue = _userSession.IdStore;
                cboChonCuaHang_NV.Enabled = false;
                cboChonCuaHang_HD.SelectedValue = _userSession.IdStore; // THÊM
                cboChonCuaHang_HD.Enabled = false; // THÊM
                storeIdload = _userSession.IdStore;
                LoadData_NhanVienTheoCuaHang(storeIdload);
            }
            else if (_userSession.Role == "Admin")
            {
                cboChonCuaHang_NV.Enabled = true;
                cboChonCuaHang_PC.Enabled = true;
                cboChonCuaHang_HD.Enabled = true; // THÊM
                if (cboChonCuaHang_NV.SelectedValue != null)
                    storeIdload = cboChonCuaHang_NV.SelectedValue.ToString();
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

            // Thiết lập combo PC
            cboChonCuaHang_PC.DataSource = cboChonCuaHang_NV.DataSource;
            cboChonCuaHang_PC.DisplayMember = "StoreName";
            cboChonCuaHang_PC.ValueMember = "StoreId";

            // Thiết lập combo HD
            cboChonCuaHang_HD.DataSource = cboChonCuaHang_NV.DataSource;
            cboChonCuaHang_HD.DisplayMember = "StoreName";
            cboChonCuaHang_HD.ValueMember = "StoreId";
            // Thiết lập combo Vang
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

            // Khôi phục event
            cboChonCuaHang_NV.SelectedIndexChanged += cboChonCuaHang_NV_SelectedIndexChanged;
            cboChonCuaHang_PC.SelectedIndexChanged += cboChonCuaHang_PC_SelectedIndexChanged;
            cboChonCuaHang_HD.SelectedIndexChanged += cboChonCuaHang_HD_SelectedIndexChanged;
            cboChonCuaHang_Vang.SelectedIndexChanged -= cboChonCuaHang_Vang_SelectedIndexChanged;
            cboChonCuaHang_Vang.SelectedIndexChanged += cboChonCuaHang_Vang_SelectedIndexChanged;

            LoadData_PhuCap();
            LoadData_CaLam();
            LoadData_PhanCong(GetCurrentStoreIdForPC());
            LoadData_HopDong(GetCurrentStoreIdForHD()); // THÊM: LỌC THEO CỬA HÀNG
            LoadData_ChamCongVang(GetCurrentStoreIdForVang());
        }

        #region Lọc nhân viên theo cửa hàng
        private void LoadDanhSachCuaHang()
        {
            try
            {
                using (var childContainer = _container.CreateChildContainer())
                {
                    var storeService = childContainer.Resolve<IStoreService>();
                    var result = storeService.GetAll();
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
                cboChonCuaHang_NV.SelectedIndexChanged += cboChonCuaHang_NV_SelectedIndexChanged;
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

            using (var childContainer = _container.CreateChildContainer())
            {
                var employeeService = childContainer.Resolve<IEmployeeService>();
                var list = employeeService.GetEmployeeByStore(storeId, pageNumber, pageSize);
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

            using (var childContainer = _container.CreateChildContainer())
            {
                var employeeService = childContainer.Resolve<IEmployeeService>();
                var list = employeeService.GetEmployee(pageNumber, pageSize);
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
            }
            dgvDuLieu_NhanVien.DataSource = dt;
            dgvDuLieu_NhanVien.AllowUserToAddRows = false;
            dgvDuLieu_NhanVien.ReadOnly = true;
            dgvDuLieu_NhanVien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_NhanVien.Columns["Edit"] == null)
            {
                var btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_NhanVien.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_NhanVien.Columns["Delete"] == null)
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_NhanVien.Columns.Add(btnDelete);
            }
            ApplyGridStyle(dgvDuLieu_NhanVien);
            btnTrangTruocNV.Enabled = pageNumber > 1;
            btnTrangSauNV.Enabled = pageNumber < _totalPage_NV;
        }

        private void dgvDuLieu_NhanVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string employeeId = dgvDuLieu_NhanVien.Rows[e.RowIndex].Cells["MaNhanVien"].Value.ToString();

            if (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Edit")
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
            else if (dgvDuLieu_NhanVien.Columns[e.ColumnIndex].Name == "Delete")
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
        #endregion

        #region Quản lý hợp đồng lương
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

            using (var childContainer = _container.CreateChildContainer())
            {
                var contractService = childContainer.Resolve<ISalaryContractService>();
                var employeeService = childContainer.Resolve<IEmployeeService>();

                // Lấy danh sách nhân viên theo cửa hàng
                List<string> employeeIds = new List<string>();
                if (!string.IsNullOrEmpty(storeId))
                {
                    var empResult = employeeService.GetEmployeeByStore(storeId, 1, 1000);
                    if (empResult.Succeeded && empResult.Data != null)
                        employeeIds = empResult.Data.Select(x => x.EmployeeId).ToList();
                }

                // Lấy tất cả hợp đồng
                var allContracts = contractService.GetPaged(1, 10000);
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
                    var empResult = employeeService.GetEmployeeByID(item.EmployeeId);
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
            }

            dgvDuLieu_HopDong.DataSource = dt;
            dgvDuLieu_HopDong.AllowUserToAddRows = false;
            dgvDuLieu_HopDong.ReadOnly = true;
            dgvDuLieu_HopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_HopDong.Columns["Edit"] == null)
            {
                var btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_HopDong.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_HopDong.Columns["Delete"] == null)
            {
                var btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu_HopDong.Columns.Add(btnDelete);
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

            if (dgvDuLieu_HopDong.Columns[e.ColumnIndex].Name == "Edit")
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
            else if (dgvDuLieu_HopDong.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show($"Bạn có chắc muốn xóa hợp đồng {contractId}?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    using (var childContainer = _container.CreateChildContainer())
                    {
                        var contractService = childContainer.Resolve<ISalaryContractService>();
                        var result = contractService.SoftDelete(contractId);
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
        #endregion

        #region Quản lý phụ cấp
        private void LoadData_PhuCap(int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhuCap");
            dt.Columns.Add("TenPhuCap");
            dt.Columns.Add("MucMacDinh");

            using (var childContainer = _container.CreateChildContainer())
            {
                var allowanceService = childContainer.Resolve<IAllowanceService>();
                var list = allowanceService.GetAllowance(pageNumber, pageSize);
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
            }

            dgvDuLieu_PhuCap.DataSource = dt;
            dgvDuLieu_PhuCap.AllowUserToAddRows = false;
            dgvDuLieu_PhuCap.ReadOnly = true;
            dgvDuLieu_PhuCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_PhuCap.Columns["Edit"] == null)
            {
                var btnEdit = new DataGridViewButtonColumn { Name = "Edit", HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true };
                dgvDuLieu_PhuCap.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_PhuCap.Columns["Delete"] == null)
            {
                var btnDelete = new DataGridViewButtonColumn { Name = "Delete", HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true };
                dgvDuLieu_PhuCap.Columns.Add(btnDelete);
            }

            ApplyGridStyle(dgvDuLieu_PhuCap);
            btnTrangTruocPK.Enabled = pageNumber > 1;
            btnTrangSauPK.Enabled = pageNumber < _totalPage_PC;
        }

        private void dgvDuLieu_PhuCap_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string allowanceId = dgvDuLieu_PhuCap.Rows[e.RowIndex].Cells["MaPhuCap"].Value.ToString();

            if (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Edit")
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
            else if (dgvDuLieu_PhuCap.Columns[e.ColumnIndex].Name == "Delete")
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

            using (var childContainer = _container.CreateChildContainer())
            {
                var shiftService = childContainer.Resolve<IShiftService>();
                var list = shiftService.GetShift(pageNumber, pageSize);
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
            }

            dgvDuLieu_CaLam.DataSource = dt;
            dgvDuLieu_CaLam.AllowUserToAddRows = false;
            dgvDuLieu_CaLam.ReadOnly = true;
            dgvDuLieu_CaLam.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_CaLam.Columns["Edit"] == null)
            {
                var btnEdit = new DataGridViewButtonColumn { Name = "Edit", HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true };
                dgvDuLieu_CaLam.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_CaLam.Columns["Delete"] == null)
            {
                var btnDelete = new DataGridViewButtonColumn { Name = "Delete", HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true };
                dgvDuLieu_CaLam.Columns.Add(btnDelete);
            }

            ApplyGridStyle(dgvDuLieu_CaLam);
            btnTrangTruocCL.Enabled = pageNumber > 1;
            btnTrangSauCL.Enabled = pageNumber < _totalPage_CL;
        }

        private void dgvDuLieu_CaLam_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string shiftId = dgvDuLieu_CaLam.Rows[e.RowIndex].Cells["MaCaLam"].Value.ToString();

            if (dgvDuLieu_CaLam.Columns[e.ColumnIndex].Name == "Edit")
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
            else if (dgvDuLieu_CaLam.Columns[e.ColumnIndex].Name == "Delete")
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
            dt.Columns.Add("HoTen");
            dt.Columns.Add("CaLam");
            dt.Columns.Add("NgayLam");
            dt.Columns.Add("GhiChu");

            List<string> employeeIds = new List<string>();
            using (var childContainer = _container.CreateChildContainer())
            {
                var empService = childContainer.Resolve<IEmployeeService>();
                if (!string.IsNullOrEmpty(storeId))
                {
                    var empResult = empService.GetEmployeeByStore(storeId, 1, 1000);
                    if (empResult.Succeeded && empResult.Data != null)
                        employeeIds = empResult.Data.Select(x => x.EmployeeId).ToList();
                }

                var allAssignments = _shiftAssignmentService.GetPaged(1, 10000);
                if (!allAssignments.Succeeded || allAssignments.Data == null)
                {
                    MessageBox.Show("Không thể tải phân công ca làm!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var filtered = string.IsNullOrEmpty(storeId)
                    ? allAssignments.Data
                    : allAssignments.Data.Where(x => employeeIds.Contains(x.EmployeeId)).ToList();

                var total = filtered.Count;
                var paged = filtered.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                _totalPage_PhanCong = (long)Math.Ceiling((double)total / pageSize);

                foreach (var item in paged)
                {
                    var empNameProp = item.GetType().GetProperty("EmployeeName");
                    var shiftNameProp = item.GetType().GetProperty("ShiftName");
                    string empName = empNameProp?.GetValue(item)?.ToString() ?? "N/A";
                    string shiftName = shiftNameProp?.GetValue(item)?.ToString() ?? "N/A";
                    dt.Rows.Add(item.Id, item.EmployeeId, empName, shiftName,
                        item.WorkDate.ToString("dd/MM/yyyy"), item.Note ?? "");
                }
            }

            dgvDuLieu_PhanCong.DataSource = dt;
            dgvDuLieu_PhanCong.AllowUserToAddRows = false;
            dgvDuLieu_PhanCong.ReadOnly = true;
            dgvDuLieu_PhanCong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_PhanCong.Columns["Edit"] == null)
            {
                var btnEdit = new DataGridViewButtonColumn { Name = "Edit", HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true };
                dgvDuLieu_PhanCong.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_PhanCong.Columns["Delete"] == null)
            {
                var btnDelete = new DataGridViewButtonColumn { Name = "Delete", HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true };
                dgvDuLieu_PhanCong.Columns.Add(btnDelete);
            }

            ApplyGridStyle(dgvDuLieu_PhanCong);
            btnTrangTruocPC.Enabled = pageNumber > 1;
            btnTrangSauPC.Enabled = pageNumber < _totalPage_PhanCong;
            txtSoTrangPC.Text = pageNumber.ToString();
        }

        private void dgvDuLieu_PhanCong_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string assignmentId = dgvDuLieu_PhanCong.Rows[e.RowIndex].Cells["MaPhanCong"].Value.ToString();

            if (dgvDuLieu_PhanCong.Columns[e.ColumnIndex].Name == "Edit")
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
            else if (dgvDuLieu_PhanCong.Columns[e.ColumnIndex].Name == "Delete")
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
                    new ParameterOverride("currentStoreId", _userSession.IdStore)
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
            using (var childContainer = _container.CreateChildContainer())
            {
                var empService = childContainer.Resolve<IEmployeeService>();
                if (!string.IsNullOrEmpty(storeId))
                {
                    var empResult = empService.GetEmployeeByStore(storeId, 1, 1000);
                    if (empResult.Succeeded && empResult.Data != null)
                        employeeIds = empResult.Data.Select(x => x.EmployeeId).ToList();
                }

                var allAbsences = _absenceService.GetAbsence(pageNumber, pageSize);
                if (!allAbsences.Succeeded || allAbsences.Data == null)
                {
                    MessageBox.Show("Không thể tải danh sách chấm công vắng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var filtered = string.IsNullOrEmpty(storeId)
                    ? allAbsences.Data
                    : allAbsences.Data.Where(x => employeeIds.Contains(x.EmployeeId)).ToList();

                _totalPage_Vang = (long)Math.Ceiling((double)allAbsences.TotalCount / pageSize);

                foreach (var item in filtered)
                {
                    var empResult = empService.GetEmployeeByID(item.EmployeeId);
                    string hoTen = empResult.Succeeded && empResult.Data != null ? empResult.Data.FullName : "N/A";

                    var shiftResult = _shiftService.GetShiftByID(item.ShiftId);
                    string caLam = shiftResult.Succeeded && shiftResult.Data != null ? shiftResult.Data.ShiftName : "N/A";

                    dt.Rows.Add(
                        item.AbsenceId,
                        item.EmployeeId,
                        hoTen,
                        caLam,
                        item.WorkDate.ToString("dd/MM/yyyy"),
                        item.IsLeaveOfAbsence ? "Có" : "Không",
                        item.Reason ?? "",
                        item.IsPaid ? "Có lương" : "Không lương"
                    );
                }
            }

            dgvDuLieu_Vang.DataSource = dt;
            dgvDuLieu_Vang.AllowUserToAddRows = false;
            dgvDuLieu_Vang.ReadOnly = true;
            dgvDuLieu_Vang.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvDuLieu_Vang.Columns["Edit"] == null)
            {
                var btnEdit = new DataGridViewButtonColumn { Name = "Edit", HeaderText = "Edit", Text = "Edit", UseColumnTextForButtonValue = true };
                dgvDuLieu_Vang.Columns.Add(btnEdit);
            }
            if (dgvDuLieu_Vang.Columns["Delete"] == null)
            {
                var btnDelete = new DataGridViewButtonColumn { Name = "Delete", HeaderText = "Delete", Text = "Delete", UseColumnTextForButtonValue = true };
                dgvDuLieu_Vang.Columns.Add(btnDelete);
            }

            ApplyGridStyle(dgvDuLieu_Vang);
            btnTrangTruocVang.Enabled = pageNumber > 1;
            btnTrangSauVang.Enabled = pageNumber < _totalPage_Vang;
            txtSoTrangVang.Text = pageNumber.ToString();
        }

        private void dgvDuLieu_Vang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string absenceId = dgvDuLieu_Vang.Rows[e.RowIndex].Cells["MaVang"].Value.ToString();

            if (dgvDuLieu_Vang.Columns[e.ColumnIndex].Name == "Edit")
            {
                var frm = _container.Resolve<frmChucNang_ChamCongVang>(
                    new ParameterOverride("absenceId", absenceId),
                    new ParameterOverride("currentStoreId", GetCurrentStoreIdForVang()));
                frm.DataChanged += (s, ev) =>
                {
                    int page = int.TryParse(txtSoTrangVang.Text, out int p) ? p : 1;
                    LoadData_ChamCongVang(GetCurrentStoreIdForVang(), page);
                };
                frm.ShowDialog();
            }
            else if (dgvDuLieu_Vang.Columns[e.ColumnIndex].Name == "Delete")
            {
                if (MessageBox.Show($"Xóa chấm công vắng {absenceId}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var result = _absenceService.RemoveAbsence(absenceId);
                    if (result.Succeeded)
                    {
                        MessageBox.Show("Xóa thành công!");
                        int page = int.TryParse(txtSoTrangVang.Text, out int p) ? p : 1;
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
            int currentPage = Convert.ToInt32(txtSoTrangVang.Text);
            if (currentPage < _totalPage_Vang)
            {
                int pageNumber = currentPage + 1;
                txtSoTrangVang.Text = pageNumber.ToString();
                LoadData_ChamCongVang(GetCurrentStoreIdForVang(), pageNumber);
            }
        }

        private void btnTrangTruocVang_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(txtSoTrangVang.Text);
            if (currentPage > 1)
            {
                int pageNumber = currentPage - 1;
                txtSoTrangVang.Text = pageNumber.ToString();
                LoadData_ChamCongVang(GetCurrentStoreIdForVang(), pageNumber);
            }
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
                if (e.RowIndex >= 0 && (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);
                    Color backColor = dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" ? Color.SeaGreen : Color.IndianRed;
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

        private void btnThemHD_Click(object sender, EventArgs e)
        {
            var frmChucNang = _container.Resolve<frmChucNang_HopDongLuong>();
            frmChucNang.ShowDialog();
        }
    }
}