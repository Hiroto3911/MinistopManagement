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
        private readonly string _lang = Properties.Settings.Default.Language;
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

            ApplyLanguage();
            this.StartPosition = FormStartPosition.CenterParent;
            LoadAllowancesWithCurrentState();
        }

        private void ApplyLanguage()
        {
            this.Text = _lang == "en-US" ? "Update Contract Allowances" : "Cập nhật phụ cấp hợp đồng";

            // Giả sử bạn có 2 nút: btnLuu và btnDong
            if (this.Controls.Find("btnLuu", true).FirstOrDefault() is Button btnLuu)
                btnLuu.Text = _lang == "en-US" ? "Save" : "Lưu";

            if (this.Controls.Find("btnDong", true).FirstOrDefault() is Button btnDong)
                btnDong.Text = _lang == "en-US" ? "Close" : "Đóng";
        }

        private void LoadAllowancesWithCurrentState()
        {
            var allowancesResult = _allowanceService.GetAll();
            var currentResult = _salaryContractAllowanceService.GetByContractId(_contractId);

            if (!allowancesResult.Succeeded || allowancesResult.Data == null)
            {
                MessageBox.Show(
                    _lang == "en-US" ? "Cannot load allowance list!" : "Không thể tải danh sách phụ cấp!",
                    _lang == "en-US" ? "Error" : "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            var currentlyAssigned = (currentResult.Succeeded && currentResult.Data != null)
                ? currentResult.Data.Select(x => x.AllowanceId).ToHashSet()
                : new System.Collections.Generic.HashSet<string>();

            _dt = new DataTable();
            _dt.Columns.Add("AllowanceID", typeof(string));
            _dt.Columns.Add("AllowanceName", typeof(string));
            _dt.Columns.Add("Selected", typeof(bool));
            _dt.Columns.Add("WasSelected", typeof(bool));

            foreach (var item in allowancesResult.Data)
            {
                bool isAssigned = currentlyAssigned.Contains(item.AllowanceId);
                _dt.Rows.Add(item.AllowanceId, item.AllowanceName, isAssigned, isAssigned);
            }

            dgvPhuCap.DataSource = _dt;
            dgvPhuCap.AllowUserToAddRows = false;

            // Cấu hình cột
            dgvPhuCap.Columns["AllowanceID"].Visible = false;
            dgvPhuCap.Columns["WasSelected"].Visible = false;
            dgvPhuCap.Columns["Selected"].HeaderText = _lang == "en-US" ? "Select" : "Chọn";
            dgvPhuCap.Columns["AllowanceName"].HeaderText = _lang == "en-US" ? "Allowance Name" : "Tên Phụ Cấp";

            dgvPhuCap.Columns["Selected"].ReadOnly = false;
            dgvPhuCap.ReadOnly = false;
            foreach (DataGridViewColumn col in dgvPhuCap.Columns)
                if (col.Name != "Selected") col.ReadOnly = true;

            dgvPhuCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Style đẹp
            dgvPhuCap.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvPhuCap.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvPhuCap.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPhuCap.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvPhuCap.RowTemplate.Height = 45;
            dgvPhuCap.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
        }

        private void dgvPhuCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvPhuCap.Columns[e.ColumnIndex].Name == "Selected")
            {
                dgvPhuCap.EndEdit();
                var cell = dgvPhuCap.Rows[e.RowIndex].Cells["Selected"];
                cell.Value = !Convert.ToBoolean(cell.Value ?? false);
            }
        }

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
                MessageBox.Show(
                    _lang == "en-US" ? "No changes detected." : "Không có thay đổi nào.",
                    _lang == "en-US" ? "Notification" : "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            try
            {
                _salaryContractAllowanceService.BeginTransaction();
                bool success = true;
                string err = "";

                // Thêm mới
                foreach (var id in toAdd)
                {
                    var dto = new SalaryContractAllowanceDto
                    {
                        ContractId = _contractId,
                        AllowanceId = id,
                        CustomAmount = null
                    };
                    var res = _salaryContractAllowanceService.Create(dto);
                    if (!res.Succeeded)
                    {
                        success = false;
                        err += $"{(_lang == "en-US" ? "Add" : "Thêm")} {id}: {res.Message}\n";
                    }
                }

                // Xóa bỏ - Lấy lại danh sách hiện tại để lấy Id chính xác
                if (toRemove.Any())
                {
                    var current = _salaryContractAllowanceService.GetByContractId(_contractId);
                    if (current.Succeeded && current.Data != null)
                    {
                        var itemsToDelete = current.Data.Where(x => toRemove.Contains(x.AllowanceId));
                        foreach (var item in itemsToDelete)
                        {
                            var res = _salaryContractAllowanceService.Remove(item.Id);
                            if (!res.Succeeded)
                            {
                                success = false;
                                err += $"{(_lang == "en-US" ? "Remove" : "Xóa")} {item.AllowanceId}: {res.Message}\n";
                            }
                        }
                    }
                }

                if (success)
                {
                    _salaryContractAllowanceService.Commit();
                    MessageBox.Show(
                        _lang == "en-US" ? "Allowances updated successfully!" : "Cập nhật phụ cấp thành công!",
                        _lang == "en-US" ? "Success" : "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    _salaryContractAllowanceService.Rollback();
                    MessageBox.Show(
                        $"{(_lang == "en-US" ? "Some errors occurred:\n" : "Có lỗi xảy ra:\n")}{err}",
                        _lang == "en-US" ? "Error" : "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _salaryContractAllowanceService.Rollback();
                MessageBox.Show(
                    $"{(_lang == "en-US" ? "System error: " : "Lỗi hệ thống: ")}{ex.Message}",
                    _lang == "en-US" ? "Error" : "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}