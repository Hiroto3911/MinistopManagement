using Domain.DTO;
using Domain.Entity;
using Services.Interfaces;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmXemPhuCap_HopDongLuong : Form
    {
        private readonly IAllowanceService _allowanceService;
        private readonly ISalaryContractAllowanceService _salaryContractAllowanceService;
        private readonly string _contractId;
        private DataTable _dt;

        public frmXemPhuCap_HopDongLuong(
            IAllowanceService allowanceService,
            ISalaryContractAllowanceService salaryContractAllowanceService,
            string contractId)
        {
            InitializeComponent();
            _allowanceService = allowanceService;
            _salaryContractAllowanceService = salaryContractAllowanceService;
            _contractId = contractId;

            this.Text = "Cập nhật phụ cấp hợp đồng";
            this.StartPosition = FormStartPosition.CenterParent;

            LoadAllowancesWithCurrentState();
        }

        private void LoadAllowancesWithCurrentState()
        {
            var allowancesResult = _allowanceService.GetAll();
            var currentResult = _salaryContractAllowanceService.GetByContractId(_contractId);

            if (!allowancesResult.Succeeded || allowancesResult.Data == null)
            {
                MessageBox.Show("Không thể tải danh sách phụ cấp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _dt = new DataTable();
            _dt.Columns.Add("AllowanceID", typeof(string));
            _dt.Columns.Add("AllowanceName", typeof(string));
            _dt.Columns.Add("Selected", typeof(bool));      // Người dùng chọn
            _dt.Columns.Add("WasSelected", typeof(bool));   // Trạng thái ban đầu trong DB

            var currentlyAssigned = currentResult.Succeeded && currentResult.Data != null
                ? currentResult.Data.Select(x => x.AllowanceId).ToHashSet()
                : new System.Collections.Generic.HashSet<string>();

            foreach (var item in allowancesResult.Data)
            {
                bool isAssigned = currentlyAssigned.Contains(item.AllowanceId);
                _dt.Rows.Add(item.AllowanceId, item.AllowanceName, isAssigned, isAssigned);
            }

            dgvPhuCap.DataSource = _dt;
            dgvPhuCap.AllowUserToAddRows = false;

            // Chỉ cho phép sửa cột checkbox
            dgvPhuCap.Columns["Selected"].ReadOnly = false;
            dgvPhuCap.Columns["WasSelected"].Visible = false;
            dgvPhuCap.Columns["AllowanceID"].Visible = false;

            dgvPhuCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Style đẹp như form thêm
            dgvPhuCap.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvPhuCap.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPhuCap.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPhuCap.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvPhuCap.RowTemplate.Height = 45;
            dgvPhuCap.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        // Click vào checkbox để tick/untick
        private void dgvPhuCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvPhuCap.Columns[e.ColumnIndex].Name == "Selected")
            {
                dgvPhuCap.EndEdit(); // Quan trọng!
                var cell = dgvPhuCap.Rows[e.RowIndex].Cells["Selected"];
                cell.Value = !Convert.ToBoolean(cell.Value ?? false);
            }
        }

        // Nút Lưu (bạn đổi tên btnDong thành btnLuu hoặc thêm nút mới)
        private void btnLuu_Click(object sender, EventArgs e)
        {
            var nowChecked = _dt.AsEnumerable()
                .Where(r => r.Field<bool>("Selected"))
                .Select(r => r.Field<string>("AllowanceID"))
                .ToList();

            var wasChecked = _dt.AsEnumerable()
                .Where(r => r.Field<bool>("WasSelected"))
                .Select(r => r.Field<string>("AllowanceID"))
                .ToList();

            var toAdd = nowChecked.Except(wasChecked).ToList();
            var toRemove = wasChecked.Except(nowChecked).ToList();

            if (!toAdd.Any() && !toRemove.Any())
            {
                MessageBox.Show("Không có thay đổi nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            try
            {
                _salaryContractAllowanceService.BeginTransaction();

                bool success = true;
                string err = "";

                // THÊM MỚI
                foreach (var id in toAdd)
                {
                    var dto = new SalaryContractAllowanceDto
                    {
                        ContractId = _contractId,
                        AllowanceId = id
                    };
                    var res = _salaryContractAllowanceService.Create(dto);
                    if (!res.Succeeded)
                    {
                        success = false;
                        err += $"Thêm {id}: {res.Message}\n";
                    }
                }

                // XÓA BỎ
                foreach (var id in toRemove)
                {
                    var current = _salaryContractAllowanceService.GetByContractId(_contractId);
                    var item = current.Data?.FirstOrDefault(x => x.AllowanceId == id);
                    if (item != null)
                    {
                        var res = _salaryContractAllowanceService.Remove(item.Id);
                        if (!res.Succeeded)
                        {
                            success = false;
                            err += $"Xóa {id}: {res.Message}\n";
                        }
                    }
                }

                if (success)
                {
                    _salaryContractAllowanceService.Commit();
                    MessageBox.Show("Cập nhật phụ cấp thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    _salaryContractAllowanceService.Rollback();
                    MessageBox.Show($"Có lỗi xảy ra:\n{err}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _salaryContractAllowanceService.Rollback();
                MessageBox.Show($"Lỗi hệ thống: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}