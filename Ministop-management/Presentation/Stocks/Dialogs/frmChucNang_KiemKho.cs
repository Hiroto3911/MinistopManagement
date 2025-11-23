
using Domain.Entity;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation.Stocks.Dialogs
{
    public partial class frmChucNang_KiemKho : Form
    {

        public event EventHandler dataChanged;
        private readonly IStockCheckService _stockCheckService;
        private readonly IUserSession _userSession;
        private string _checkID;
        public frmChucNang_KiemKho(IStockCheckService stockCheckService, IUserSession userSession, string checkID = null)
        {
            InitializeComponent();
            _stockCheckService = stockCheckService;
            _userSession = userSession;
            _checkID = checkID;
        }



        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string storeId = txtMaCH.Text.Trim();
            string employeeId = txtMaNV.Text.Trim();
            DateTime checkDate = dtpNgayKiem.Value;
            byte status = Convert.ToByte(cboTrangThai.SelectedValue.ToString());

            var exportDto = new StockCheckDto()
            {

                StoreId = storeId,
                EmployeeId = employeeId,
                CheckDate = checkDate,
                Status = status
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_checkID))
            {
                result = _stockCheckService.CreatestockCheck(exportDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                exportDto.CheckId = _checkID;
                result = _stockCheckService.UpdateStockCheck(exportDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu phiếu kiểm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void LoadDataCboTrangThai()
        {

            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            Dictionary<string, byte> status;
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                cboTrangThai.Enabled = true;
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
        private void frmChucNang_KiemKho_Load(object sender, EventArgs e)
        {
            LoadDataCboTrangThai();

            if (!string.IsNullOrEmpty(_checkID))
            {
                var entity = _stockCheckService.GetStockCheckByID(_checkID);
                if (!entity.Succeeded && entity.Data == null) return;
                cboTrangThai.Enabled = true;
                txtPhieuKiem.Text = entity.Data.CheckId;
                txtMaCH.Text = entity.Data.StoreId;
                txtMaNV.Text = entity.Data.EmployeeId;
                dtpNgayKiem.Value = entity.Data.CheckDate;

            }
            else
            {
                dtpNgayKiem.Value = DateTime.UtcNow.ToLocalTime();
                txtMaCH.Text = _userSession.IdStore;
                txtMaNV.Text = _userSession.UserId;
            }
        }


    }
}
