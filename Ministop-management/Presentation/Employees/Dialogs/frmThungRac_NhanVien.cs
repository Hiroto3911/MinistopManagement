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
    public partial class frmThungRac_NhanVien : Form
    {
        private readonly IEmployeeService _employeeService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        public event EventHandler DataChanged;
        public event EventHandler datachanged;

        public frmThungRac_NhanVien(IEmployeeService employeeService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _employeeService = employeeService;
            _container = container;
            _userSession = userSession;
            LoadData();
        }

        #region Load dữ liệu
        private void LoadData()
        {
            // ===== 1️⃣ Tạo DataTable để hiển thị =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaNhanVien");
            dt.Columns.Add("MaCuaHang");
            dt.Columns.Add("TenNhanVien");
            dt.Columns.Add("GioiTinh");
            dt.Columns.Add("NgaySinh");
            dt.Columns.Add("SoDienThoai");
            dt.Columns.Add("ChucVu");
            dt.Columns.Add("LoaiNhanVien");
            dt.Columns.Add("MatKhauMaHoa");

            using (var childContainer = _container.CreateChildContainer())
            {
                var employeeService = childContainer.Resolve<IEmployeeService>();
                var result = employeeService.GetAllEmployeeIsDelete(); // ⚡ cần có trong service

                if (!result.Succeeded || result.Data == null) return;

                foreach (var emp in result.Data)
                {
                    dt.Rows.Add(
                    emp.EmployeeId,
                    emp.StoreId,
                    emp.FullName,
                    emp.Gender ? "Nam" : "Nữ",
                    emp.BirthDate.ToString("dd/MM/yyyy"),
                    emp.Phone,
                    emp.Position,
                    emp.EmploymentType,
                    emp.PasswordHash
                    );
                }
            }

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = false;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm cột checkbox nếu chưa có =====
            if (dgvDuLieu.Columns["chkSelect"] == null)
            {
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.HeaderText = "Chọn";
                chk.Name = "chkSelect";
                chk.Width = 50;
                dgvDuLieu.Columns.Add(chk);
            }

            foreach (DataGridViewColumn col in dgvDuLieu.Columns)
            {
                if (col.Name != "chkSelect")
                    col.ReadOnly = true;
            }

            // ===== 3️⃣ Giao diện =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;
        }
        #endregion

        #region Lấy danh sách nhân viên được chọn
        private List<string> GetSelectedEmployees()
        {
            var list = new List<string>();
            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);
                if (isChecked)
                {
                    list.Add(row.Cells["MaNhanVien"].Value.ToString());
                }
            }
            return list;
        }
        #endregion

        #region Sự kiện cell click
        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "chkSelect")
            {
                dgvDuLieu.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        #endregion

        #region Nút Khôi Phục
        private void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            var selectedList = GetSelectedEmployees();
            if (selectedList == null || selectedList.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một nhân viên để khôi phục.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _employeeService.RestoreEmployee(selectedList); // ⚡ cần có trong service
            if (!result.Succeeded)
            {
                MessageBox.Show($"Khôi phục thất bại: {result.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Khôi phục thành công!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DataChanged?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
        #endregion

        #region Nút Thoát
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
