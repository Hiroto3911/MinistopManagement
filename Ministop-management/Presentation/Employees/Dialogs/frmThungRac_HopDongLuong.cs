using Services.Interfaces;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmThungRac_HopDongLuong : Form
    {
        private readonly ISalaryContractService _salaryContractService;
        private readonly IEmployeeService _employeeService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        public event EventHandler datachanged;

        public frmThungRac_HopDongLuong(
            ISalaryContractService salaryContractService,
            IEmployeeService employeeService,
            IUnityContainer container,
            IUserSession userSession)
        {
            InitializeComponent();
            _salaryContractService = salaryContractService;
            _employeeService = employeeService;
            _container = container;
            _userSession = userSession;

            LoadDeletedContracts();
        }

        private void LoadDeletedContracts()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ContractId", typeof(string));
            dt.Columns.Add("EmployeeId", typeof(string));
            dt.Columns.Add("FullName", typeof(string));
            dt.Columns.Add("BasicSalary", typeof(string));
            dt.Columns.Add("HourlyRate", typeof(string));
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("EndDate", typeof(string));

            var result = _salaryContractService.GetAllIsDeleted();
            if (result.Succeeded && result.Data != null)
            {
                foreach (var contract in result.Data)
                {
                    string fullName = "N/A";
                    var empResult = _employeeService.GetEmployeeByID(contract.EmployeeId);
                    if (empResult.Succeeded && empResult.Data != null)
                        fullName = empResult.Data.FullName;

                    dt.Rows.Add(
                        contract.ContractId,
                        contract.EmployeeId,
                        fullName,
                        contract.BasicSalary?.ToString("N0") ?? "-",
                        contract.HourlyRate?.ToString("N0") ?? "-",
                        contract.StartDate.ToString("dd/MM/yyyy"),
                        contract.EndDate?.ToString("dd/MM/yyyy") ?? "Đang hiệu lực"
                    );
                }
            }

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = false;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thêm cột checkbox chọn
            if (dgvDuLieu.Columns["chkSelect"] == null)
            {
                var chk = new DataGridViewCheckBoxColumn
                {
                    HeaderText = "Chọn",
                    Name = "chkSelect",
                    Width = 60
                };
                dgvDuLieu.Columns.Add(chk);
            }

            // Chỉ cho phép sửa cột checkbox
            foreach (DataGridViewColumn col in dgvDuLieu.Columns)
            {
                if (col.Name != "chkSelect")
                    col.ReadOnly = true;
            }

            // Style đẹp giống form thùng rác cửa hàng
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;
        }

        private List<string> GetSelectedContractIds()
        {
            var list = new List<string>();
            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                if (row.Cells["chkSelect"].Value != null && Convert.ToBoolean(row.Cells["chkSelect"].Value))
                {
                    list.Add(row.Cells["ContractId"].Value.ToString());
                }
            }
            return list;
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "chkSelect")
            {
                dgvDuLieu.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            var selectedIds = GetSelectedContractIds();
            if (!selectedIds.Any())
            {
                MessageBox.Show("Vui lòng chọn ít nhất một hợp đồng để khôi phục.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _salaryContractService.Restore(selectedIds);
            if (!result.Succeeded)
            {
                MessageBox.Show($"Khôi phục thất bại: {result.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Khôi phục hợp đồng thành công!", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            datachanged?.Invoke(this, EventArgs.Empty);
            LoadDeletedContracts(); // Refresh lại danh sách
        }

        private void btnXoaVinhVien_Click(object sender, EventArgs e)
        {
            var selectedIds = GetSelectedContractIds();
            if (!selectedIds.Any())
            {
                MessageBox.Show("Vui lòng chọn hợp đồng để xóa vĩnh viễn.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedIds.Count > 1)
            {
                MessageBox.Show("Hiện tại chỉ hỗ trợ xóa vĩnh viễn một hợp đồng mỗi lần.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn XÓA VĨNH VIỄN hợp đồng này?\nHành động này không thể hoàn tác!",
                "Xác nhận xóa vĩnh viễn", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                return;

            // GỌI HÀM XÓA CỨNG (bạn cần thêm vào service nếu chưa có)
            var result = _salaryContractService.HardDelete(selectedIds.First());
            if (!result.Succeeded)
            {
                MessageBox.Show($"Xóa thất bại: {result.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Xóa vĩnh viễn thành công!", "Thành công",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            LoadDeletedContracts();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}