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

        private void frmChucNang_ChiPhiCuaHang_Load(object sender, EventArgs e)
        {
            //cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            if (!string.IsNullOrEmpty(_expenseID))
            {
                var entity = _storeFixedExpenseServices.GetStoreFixedExpenseByID(_expenseID);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", "Lỗi");
                    return;
                }
                txtTenCuaHang.Text = entity.Data.StoreId;
                txtTienMatBang.Text = entity.Data.RentCost.ToString()??"0.0";
                txtTienDien.Text = entity.Data.ElectricityCost.ToString() ?? "0.0";
                txtTienNuoc.Text = entity.Data.WaterCost.ToString() ?? "0.0";
                rtxtGhiChu.Text = entity.Data.Note ??"";

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
                    MessageBox.Show("Tiền mặt bằng phải là số hợp lệ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTienMatBang.Focus();
                    return;
                }
            }

            // Tiền điện bắt buộc nhập
            if (string.IsNullOrWhiteSpace(txtTienDien.Text) ||
                !decimal.TryParse(txtTienDien.Text, out decimal electricityCost) || electricityCost < 0)
            {
                MessageBox.Show("Tiền điện phải là số hợp lệ và không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTienDien.Focus();
                return;
            }

            // Tiền nước bắt buộc nhập
            if (string.IsNullOrWhiteSpace(txtTienNuoc.Text) ||
                !decimal.TryParse(txtTienNuoc.Text, out decimal waterCost) || waterCost < 0)
            {
                MessageBox.Show("Tiền nước phải là số hợp lệ và không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTienNuoc.Focus();
                return;
            }
            if (Regex.IsMatch(rtxtGhiChu.Text.Trim(), @"[^a-zA-Z0-9\s\u00C0-\u1EF9,./-]"))
            {
                MessageBox.Show("Ghi chú không được chứa ký tự đặc biệt lạ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtxtGhiChu.Focus();
                return;
            }
            var storeId = _userSession.IdStore;
            var note = rtxtGhiChu.Text.Trim();
            string monthYear = dtpNgayLap.Text;

            var expenseDto = new StoreFixedExpenseDto()
            {
          
                StoreId = storeId,
                RentCost = rentCost,          // giá trị có thể là 0 nếu trống
                WaterCost = waterCost,
                ElectricityCost = electricityCost,
                MonthYear = monthYear,
                Note = note
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
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu chi phí cửa hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
