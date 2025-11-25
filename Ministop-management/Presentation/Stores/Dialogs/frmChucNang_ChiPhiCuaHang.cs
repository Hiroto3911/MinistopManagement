using Domain.DTO;
using Guna.UI2.WinForms;
using Services.Interfaces;
using Shared.Security;
using Shared.Wrappers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_ChiPhiCuaHang : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IStoreFixedExpenseServices _storeFixedExpenseServices;
        public readonly IUserSession _userSession;
        public string _expenseID;
        public frmChucNang_ChiPhiCuaHang(IStoreFixedExpenseServices storeFixedExpenseServices, IUserSession userSession, string expenseID = null)
        {
            InitializeComponent();
            _storeFixedExpenseServices = storeFixedExpenseServices;
            _userSession = userSession;
            _expenseID = expenseID;
        }

        private void LoadDataCboTrangThai()
        {
            dtpNgayLap.Value = DateTime.UtcNow.ToLocalTime();
            cboTrangThai.Enabled = true;
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            Dictionary<string, byte> status;
            if (_userSession.Role == "Admin")
            {
                txtTienDien.Enabled = false;
                txtTienNuoc.Enabled = false;
                txtTienMatBang.Enabled = false;

                status = new Dictionary<string, byte>()
                {
                 {Properties.Resources.Status_Permitted,1 },
                 {Properties.Resources.Status_NotPermitted,0 }
                };
            }
            else
            {

                status = new Dictionary<string, byte>()
               {
                {Properties.Resources.Status_Draft,2 },
                 {Properties.Resources.Status_Pending,3 }
               };
            }
            cboTrangThai.DataSource = status.ToList();
            cboTrangThai.DisplayMember = "Key";
            cboTrangThai.ValueMember = "Value";
        }
        private void frmChucNang_ChiPhiCuaHang_Load(object sender, EventArgs e)
        {
            LoadDataCboTrangThai();
            if (!string.IsNullOrEmpty(_expenseID))
            {
                var entity = _storeFixedExpenseServices.GetStoreFixedExpenseByID(_expenseID);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtTenCuaHang.Text = entity.Data.StoreId;
                txtTienMatBang.Text = entity.Data.RentCost.ToString() ?? "0.0";
                txtTienDien.Text = entity.Data.ElectricityCost.ToString() ?? "0.0";
                txtTienNuoc.Text = entity.Data.WaterCost.ToString() ?? "0.0";
                rtxtGhiChu.Text = entity.Data.Note ?? "";

            }
            else
            {
                cboTrangThai.Enabled = false;
            }
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            decimal rentCost = 0;
            if (!string.IsNullOrWhiteSpace(txtTienMatBang.Text))
            {
                if (!decimal.TryParse(txtTienMatBang.Text, out rentCost) || rentCost < 0)
                {
                    MessageBox.Show($"{lblRentCost.Text} {Properties.Messages.Message_ValidNumber}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTienMatBang.Focus();
                    return;
                }
            }

            // Tiền điện bắt buộc nhập
            if (string.IsNullOrWhiteSpace(txtTienDien.Text) ||
                !decimal.TryParse(txtTienDien.Text, out decimal electricityCost) || electricityCost <= 0)
            {
                MessageBox.Show($"{lblElectricityCost.Text} {Properties.Messages.Message_ValidNumber}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTienDien.Focus();
                return;
            }

            // Tiền nước bắt buộc nhập
            if (string.IsNullOrWhiteSpace(txtTienNuoc.Text) ||
                !decimal.TryParse(txtTienNuoc.Text, out decimal waterCost) || waterCost <= 0)
            {
                MessageBox.Show($"{lblWaterCost.Text} {Properties.Messages.Message_ValidNumber}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTienNuoc.Focus();
                return;
            }
            if (Regex.IsMatch(rtxtGhiChu.Text.Trim(), @"[^a-zA-Z0-9\s\u00C0-\u1EF9,./-]"))
            {
                MessageBox.Show($"{lblNote.Text} {Properties.Messages.Message_SpecialCharacter}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtGhiChu.Focus();
                return;
            }
            var storeId = _userSession.IdStore;
            var note = rtxtGhiChu.Text.Trim();
            string monthYear = dtpNgayLap.Text;
            byte status = Convert.ToByte(cboTrangThai.SelectedValue.ToString()); 
            var expenseDto = new StoreFixedExpenseDto()
            {

                StoreId = storeId,
                RentCost = rentCost,          // giá trị có thể là 0 nếu trống
                WaterCost = waterCost,
                ElectricityCost = electricityCost,
                MonthYear = monthYear,
                Note = note,
                Status = status
            };

            Result<string> result;

            if (string.IsNullOrEmpty(_expenseID))
            {
                result = _storeFixedExpenseServices.CreateStoreFixedExpense(expenseDto);
                dataChanged?.Invoke(this, result.Data);
            }
            else
            {
                
                expenseDto.ExpenseId = _expenseID;
                result = _storeFixedExpenseServices.UpdateStoreFixedExpense(expenseDto);
                dataChanged?.Invoke(this, result.Data);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show($"{Properties.Messages.Message_SavedSuccessfullLy}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void txtTienMatBang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter)
            {
                txtTienDien.Focus();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void txtTienDien_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { txtTienMatBang.Focus(); }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter) { txtTienNuoc.Focus(); }
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void txtTienNuoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { txtTienDien.Focus(); }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Enter) { rtxtGhiChu.Focus(); }
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        private void rtxtGhiChu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) { txtTienNuoc.Focus(); }
            else if (e.KeyCode == Keys.Enter) { btnLuu_Click(sender, e); }
            else return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
