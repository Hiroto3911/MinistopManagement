using Domain.DTO;
using Domain.Entity;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_HopDongLuong_PhuCap : Form
    {
        private readonly IAllowanceService _allowanceService;
        private readonly ISalaryContractAllowanceService _salaryContractAllowanceService;
        private readonly string _contractId;
        private readonly string _lang = Properties.Settings.Default.Language;

        public frmChucNang_HopDongLuong_PhuCap(
            IAllowanceService allowanceService,
            ISalaryContractAllowanceService salaryContractAllowanceService,
            string contractId)
        {
            InitializeComponent();
            _allowanceService = allowanceService;
            _salaryContractAllowanceService = salaryContractAllowanceService;
            _contractId = contractId;
            ApplyLanguage();
            LoadAllowances();
        }

        private void ApplyLanguage()
        {
            if (_lang != "en-US") return;

            this.Text = "Add Allowances to Contract";
            btnThem.Text = "Add Selected";
        }

        private void LoadAllowances()
        {
            var allowances = _allowanceService.GetAll();
            if (allowances.Succeeded && allowances.Data != null)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("AllowanceID", typeof(string));
                dt.Columns.Add(_lang == "en-US" ? "Allowance Name" : "Tên Phụ Cấp", typeof(string));
                dt.Columns.Add("chkSelect", typeof(bool));

                foreach (var item in allowances.Data)
                {
                    dt.Rows.Add(item.AllowanceId, item.AllowanceName, false);
                }

                dgvDuLieu_PhuCap.DataSource = dt;
                dgvDuLieu_PhuCap.AllowUserToAddRows = false;
                dgvDuLieu_PhuCap.ReadOnly = true;
                dgvDuLieu_PhuCap.Columns["chkSelect"].ReadOnly = false;
                dgvDuLieu_PhuCap.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                // Header text
                dgvDuLieu_PhuCap.Columns["chkSelect"].HeaderText = _lang == "en-US" ? "Select" : "Chọn";

                // Style
                dgvDuLieu_PhuCap.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
                dgvDuLieu_PhuCap.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
                dgvDuLieu_PhuCap.ThemeStyle.HeaderStyle.ForeColor = Color.White;
                dgvDuLieu_PhuCap.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvDuLieu_PhuCap.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
                dgvDuLieu_PhuCap.RowTemplate.Height = 40;
            }
            else
            {
                MessageBox.Show(
                    _lang == "en-US" ? "Cannot load allowance list!" : "Không thể tải danh sách phụ cấp!",
                    _lang == "en-US" ? "Error" : "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void bthThem_Click(object sender, EventArgs e)
        {
            var selectedRows = dgvDuLieu_PhuCap.Rows
                .Cast<DataGridViewRow>()
                .Where(r => r.Cells["chkSelect"].Value != null && Convert.ToBoolean(r.Cells["chkSelect"].Value))
                .ToList();

            if (!selectedRows.Any())
            {
                MessageBox.Show(
                    _lang == "en-US" ? "Please select at least one allowance!" : "Vui lòng chọn ít nhất một phụ cấp!",
                    _lang == "en-US" ? "Warning" : "Cảnh báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            bool success = true;
            string errorMsg = "";

            try
            {
                _salaryContractAllowanceService.BeginTransaction();

                foreach (DataGridViewRow row in selectedRows)
                {
                    string allowanceId = row.Cells["AllowanceID"].Value.ToString();
                    var scaDto = new SalaryContractAllowanceDto
                    {
                        ContractId = _contractId,
                        AllowanceId = allowanceId,
                        CustomAmount = null
                    };

                    var result = _salaryContractAllowanceService.Create(scaDto);
                    if (!result.Succeeded)
                    {
                        success = false;
                        errorMsg += $"{(_lang == "en-US" ? "Allowance" : "Phụ cấp")} {allowanceId}: {result.Message}\n";
                    }
                }

                if (success)
                {
                    _salaryContractAllowanceService.Commit();
                    MessageBox.Show(
                        _lang == "en-US" ? "Allowances added successfully!" : "Thêm phụ cấp thành công!",
                        _lang == "en-US" ? "Notification" : "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    _salaryContractAllowanceService.Rollback();
                    MessageBox.Show(
                        $"{(_lang == "en-US" ? "Some allowances were not added:\n" : "Một số phụ cấp không được thêm:\n")}{errorMsg}",
                        _lang == "en-US" ? "Error" : "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _salaryContractAllowanceService.Rollback();
                MessageBox.Show(
                    $"{(_lang == "en-US" ? "System error: " : "Lỗi hệ thống: ")}{ex.Message}",
                    _lang == "en-US" ? "Error" : "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgvDuLieu_PhuCap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var col = dgvDuLieu_PhuCap.Columns["chkSelect"];
            if (col != null && e.ColumnIndex == col.Index)
            {
                dgvDuLieu_PhuCap.EndEdit();
                var cell = dgvDuLieu_PhuCap.Rows[e.RowIndex].Cells["chkSelect"];
                cell.Value = !Convert.ToBoolean(cell.Value ?? false);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}