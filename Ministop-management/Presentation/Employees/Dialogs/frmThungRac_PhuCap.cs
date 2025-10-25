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
    public partial class frmThungRac_PhuCap : Form
    {
        private readonly IAllowanceService _allowanceService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;

        public event EventHandler datachanged;
        public frmThungRac_PhuCap(IAllowanceService allowanceService, IUnityContainer container, IUserSession userSession)
        {
            InitializeComponent();
            _allowanceService = allowanceService;
            _container = container;
            _userSession = userSession;

            LoadData();
        }

        private void LoadData()
        {
            // ===== 1️⃣ Tạo cấu trúc DataTable =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaPhuCap");
            dt.Columns.Add("TenPhuCap");
            dt.Columns.Add("MucMacDinh");

            using (var childContainer = _container.CreateChildContainer())
            {
                var allowanceService = childContainer.Resolve<IAllowanceService>();

                var list = allowanceService.GetAllAllowanceIsDelete();
                if (list.Succeeded == false || list.Data == null)
                    return;

                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.AllowanceId, item.AllowanceName, item.DefaultAmount);
                }
            }

            // ===== 2️⃣ Gán dữ liệu lên DataGridView =====
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = false;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 3️⃣ Thêm cột checkbox (nếu chưa có) =====
            if (dgvDuLieu.Columns["chkSelect"] == null)
            {
                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.HeaderText = "Chọn";
                chk.Name = "chkSelect";
                chk.Width = 60;
                dgvDuLieu.Columns.Add(chk);
            }

            // ===== 4️⃣ Chỉ cho phép tick cột chọn =====
            foreach (DataGridViewColumn col in dgvDuLieu.Columns)
            {
                if (col.Name != "chkSelect")
                    col.ReadOnly = true;
            }

            // ===== 5️⃣ Style đẹp =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;
        }

        private List<string> GetSelectedAllowances()
        {
            var list = new List<string>();
            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                bool isChecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);
                if (isChecked)
                    list.Add(row.Cells["MaPhuCap"].Value.ToString());
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

        private void itbnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            var list = GetSelectedAllowances();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một phụ cấp để khôi phục.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = _allowanceService.RestoreAllowance(list);
            if (result.Succeeded == false)
            {
                MessageBox.Show($"Khôi phục thất bại: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Khôi phục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            datachanged?.Invoke(this, EventArgs.Empty);
            this.Close();
        }
    }
}
