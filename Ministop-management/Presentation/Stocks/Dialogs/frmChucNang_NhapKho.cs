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
using System.Xml.Linq;
using Unity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Unity.Storage.RegistrationSet;

namespace Presentation
{
    public partial class frmChucNang_NhapKho : Form
    {
        public event EventHandler dataChanged;
        public readonly IStockImportService _stockImportService;
        public readonly ISupplierService _supplierService;
        public readonly IProductCategoryService _productCategoryService;
        public readonly IUserSession _userSession;
        public string _importID;
        public frmChucNang_NhapKho(IStockImportService stockImportService, ISupplierService supplierService, IProductCategoryService productCategoryService, IUserSession userSession, string ImportID = null)
        {
            InitializeComponent();
            _stockImportService = stockImportService;
            _userSession = userSession;
            _supplierService = supplierService;
            _productCategoryService = productCategoryService;
            _importID = ImportID;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            string storeId = txtMaCH.Text.Trim();
            string employeeId = txtMaNV.Text.Trim();
            DateTime importDate = dtpNgayNhap.Value;
            string SupplierId = txtMaNCC.Text.Trim();
            byte status = Convert.ToByte(cboTrangThai.SelectedValue.ToString());
            string note  = rtxtGhiChu.Text.Trim();  
            var importDto = new StockImportDto()
            {

                StoreId = storeId,
                EmployeeId = employeeId,
                ImportDate = importDate,
                SupplierId = SupplierId , 
                Status = status,
                Note = note
            };

            Result<bool> result;

            if (string.IsNullOrEmpty(_importID))
            {
                result = _stockImportService.CreatestockImport(importDto);
                dataChanged?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                importDto.ImportID = _importID;
                result = _stockImportService.UpdateStockImport(importDto);
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
        private void LoadDataCboTrangThai()
        {
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            Dictionary<string, byte> status;
            if (_userSession.Role == "Admin")
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
        private void LoadCboSupplier()
        {
            var data = _supplierService.GetAllSupplier();
            if (!data.Succeeded && data.Data == null) return;
            cboNhaCungCap.DataSource = data.Data;
            cboNhaCungCap.DisplayMember = "SupplierName";
            cboNhaCungCap.ValueMember = "SupplierID";
            
        }
   
        //private void LoadCboProductCategory()
        //{
        //    var data = _productCategoryService.GetAll();
        //    if (!data.Succeeded && data.Data == null) return;
        //    cboLoaiSP.DataSource = data.Data;
        //    cboLoaiSP.DisplayMember = "CategoryName";
        //    cboLoaiSP.ValueMember = "CategoryID";
        //}
        private void frmChucNang_NhapKho_Load(object sender, EventArgs e)
        {
          
           
          
            LoadCboSupplier();
            dtpNgayNhap.Value = DateTime.UtcNow.ToLocalTime();
            if (!string.IsNullOrEmpty(_importID))
            {
                var entity = _stockImportService.GetStockImportByID(_importID);
                if (!entity.Succeeded && entity.Data == null) return;
                cboTrangThai.Enabled = true;
                txtPhieuNhap.Text = entity.Data.ImportID;
                txtMaCH.Text = entity.Data.StoreId;
                txtMaNV.Text = entity.Data.EmployeeName;
                txtMaNCC.Text = entity.Data.SupplierId;
                dtpNgayNhap.Value = entity.Data.ImportDate;
                rtxtGhiChu.Text = entity.Data.Note;
                cboNhaCungCap.Enabled = false;
                if (_userSession.Role == "Quản lý cửa hàng")
                {
                    if (entity.Data.Status == 1)
                    {
                        Dictionary<string,byte> status = new Dictionary<string, byte>() {{ "Đã nhập hàng", 4 } };
                        cboTrangThai.DataSource = status.ToList();
                        cboTrangThai.DisplayMember = "Key";
                        cboTrangThai.ValueMember = "Value";
                    }
                    else
                    {
                        LoadDataCboTrangThai();
                    }
                }else if(_userSession.Role == "Admin")
                {
                    LoadDataCboTrangThai();
                }
               
            }
            else
            {
                LoadDataCboTrangThai();
                txtMaCH.Text =_userSession.IdStore;
                txtMaNV.Text =_userSession.UserId;
            }

        }

        private void cboNhaCungCap_SelectedValueChanged(object sender, EventArgs e)
        {
            txtMaNCC.Text = cboNhaCungCap.SelectedValue.ToString();
        }
    }
}
