using Domain.DTO;
using Services.Interfaces;
using Services.Services;
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
using static Unity.Storage.RegistrationSet;

namespace Presentation
{
    public partial class frmChucNang_XuatKho : Form
    {
        public event EventHandler dataChanged;
        private readonly IStockExportService _stockExportService;
        private readonly IUserSession _userSession;
        private string _exportID;
        public frmChucNang_XuatKho(IStockExportService stockExportService, IUserSession userSession, string ExportID = null)
        {
            InitializeComponent();
            _stockExportService = stockExportService;
            _userSession = userSession;
            _exportID = ExportID;
        }



        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string storeId = txtMaCH.Text.Trim();
            string employeeId = txtMaNV.Text.Trim();
            DateTime exportDate = dtpNgayXuat.Value;
            string typeExport = cboLoaiXuat.Text.Trim();
            string reason = rtxtLyDo.Text.Trim();
            byte status = Convert.ToByte(cboTrangThai.SelectedValue.ToString());
        
            var exportDto = new StockExportDto()
            {

                StoreId = storeId,
                EmployeeId = employeeId,
                ExportDate = exportDate,
                TypeExport = typeExport,
                Reason = reason,
                Status = status
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_exportID))
            {
                result = _stockExportService.CreatestockExport(exportDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                exportDto.ExportId = _exportID;
                result = _stockExportService.UpdateStockExport(exportDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }

            if (!result.Succeeded)
            {
                MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Lưu phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void LoadCboTypeExport()
        {
            List<string> typeExport = new List<string>() { "Hư hỏng","Hết hạn","Thất thoát", "Hủy hàng" };
            cboLoaiXuat.DataSource = typeExport;

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
                    {"Duyệt",1 },
                    { "Không duyệt",0 }
                };
            }
            else
            {

                status = new Dictionary<string, byte>()
                {
                  {"Đang soạn",2 },
                  {"Chờ duyệt",3 }
                };
            }
            cboTrangThai.DataSource = status.ToList();
            cboTrangThai.DisplayMember = "Key";
            cboTrangThai.ValueMember = "Value";
        }
        private void frmChucNang_XuatKho_Load(object sender, EventArgs e)
        {

            LoadCboTypeExport();
            LoadDataCboTrangThai();
            dtpNgayXuat.Value = DateTime.UtcNow.ToLocalTime();
            if (!string.IsNullOrEmpty(_exportID))
            {
                var entity = _stockExportService.GetStockExportByID(_exportID);
                if (!entity.Succeeded && entity.Data == null) return;
                cboTrangThai.Enabled = true;
                txtPhieuXuat.Text = entity.Data.ExportId;
                txtMaCH.Text = entity.Data.StoreId;
                txtMaNV.Text = entity.Data.EmployeeId;
                cboLoaiXuat.SelectedItem = entity.Data.TypeExport;
                dtpNgayXuat.Value = entity.Data.ExportDate;
                rtxtLyDo.Text = entity.Data.Reason;

            }
            else
            {
                
                txtMaCH.Text = _userSession.IdStore;
                txtMaNV.Text = _userSession.UserId;
            }
        }
    }
}
