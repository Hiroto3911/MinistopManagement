using Domain.DTO;
using Presentation.Stocks.Dialogs;
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
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml.Linq;
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_DanhSachNhapHang : Form
    {
        public event EventHandler dataChanged;
        public readonly IStockImportDetailSerivce _stockImportDetailSerivce;
        public readonly ISupplierProductService _supplierProductService;
        public readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        private readonly string _importID;
        public string _supplierID;
        private long _totalPage;

        public frmHienThi_DanhSachNhapHang(IStockImportDetailSerivce stockImportDetailSerivce, ISupplierProductService supplierProductService, IUnityContainer container, IUserSession userSession, string importID = null, string supplierID = null)
        {
            InitializeComponent();
            _stockImportDetailSerivce = stockImportDetailSerivce;
            _supplierProductService = supplierProductService;
            _userSession = userSession;
            _container = container;
            _importID = importID;
            _supplierID = supplierID;
        }
        private void frmHienThi_DanhSachNhapHang_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_supplierID)) return;
            LoadData(_supplierID);
            LoadThemStyle();
        }
        private void LoadData(string supplierId, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("GiaTuNhaCungCap");


            using (var childContainer = _container.CreateChildContainer())
            {
                var supplierProductService = childContainer.Resolve<ISupplierProductService>();
                var list = supplierProductService.GetSupplierProduct(supplierId, pageNumber, pageSize);

                if (!list.Succeeded || list.Data == null)
                    return;

                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                foreach (var item in list.Data)
                {

                    dt.Rows.Add(item.ProductId, item.ProductName, item.SupplyPrice);
                }
            }
            dt.Columns.Add("Chon", typeof(bool));
            dt.Columns.Add("SoLuong", typeof(int));
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.Columns["MaSanPham"].HeaderText = Properties.Resources.Grid_ProductID;
            dgvDuLieu.Columns["TenSanPham"].HeaderText = Properties.Resources.Grid_ProductName;
            dgvDuLieu.Columns["GiaTuNhaCungCap"].HeaderText = Properties.Resources.Grid_SupplierPrice;
            dgvDuLieu.Columns["Chon"].HeaderText = Properties.Resources.Grid_Choose;
            dgvDuLieu.Columns["SoLuong"].HeaderText = Properties.Resources.Grid_Quantity;
            dgvDuLieu.Columns["MaSanPham"].ReadOnly = true;
            dgvDuLieu.Columns["TenSanPham"].ReadOnly = true;
            dgvDuLieu.Columns["GiaTuNhaCungCap"].ReadOnly = true;
            dgvDuLieu.Columns["SoLuong"].ReadOnly = true;
            dgvDuLieu.Columns["Chon"].Width = 50;
            int colCheckIndex = dgvDuLieu.Columns["Chon"].Index;
            Rectangle rect = dgvDuLieu.GetCellDisplayRectangle(colCheckIndex, -1, true);
            System.Windows.Forms.CheckBox checkboxHeader = new System.Windows.Forms.CheckBox();
            checkboxHeader.Name = "checkboxHeader";
            checkboxHeader.Size = new Size(30, 30);
            checkboxHeader.Location = new Point(
               rect.X + (rect.Width +70) ,
               rect.Y + (rect.Height - checkboxHeader.Height) / 2
            );
            checkboxHeader.BackColor = Color.Transparent;
            checkboxHeader.FlatStyle = FlatStyle.Flat;
            checkboxHeader.CheckedChanged += new EventHandler(checkboxHeader_CheckedChanged);
            dgvDuLieu.Controls.Add(checkboxHeader);

            dgvDuLieu.AllowUserToAddRows = false;

            // Giao diện đẹp
            btnTrangTruoc.Enabled = pageNumber > 1;
            btnTrangSau.Enabled = pageNumber < _totalPage;

        }
        private void checkboxHeader_CheckedChanged(object sender, EventArgs e)
        {
            var headerBox = (System.Windows.Forms.CheckBox)sender;
            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                if (!row.IsNewRow)
                {
                    row.Cells["Chon"].Value = headerBox.Checked;
                }
                dgvDuLieu.EndEdit();
            }
        }
        private void LoadThemStyle()
        {
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;
        }

        private void dgvDuLieu_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDuLieu.Columns[e.ColumnIndex].Name != "Chon") return;
            bool isChecked = Convert.ToBoolean(dgvDuLieu.Rows[e.RowIndex].Cells["Chon"].Value ?? false);
            if (isChecked)
            {
                dgvDuLieu.Rows[e.RowIndex].Cells["SoLuong"].ReadOnly = !isChecked;
                dgvDuLieu.Rows[e.RowIndex].Cells["SoLuong"].Style.BackColor =
                    isChecked ? Color.White : Color.LightGray;

            }
            else
            {
                dgvDuLieu.Rows[e.RowIndex].Cells["SoLuong"].Value = DBNull.Value;
                dgvDuLieu.Rows[e.RowIndex].Cells["SoLuong"].ReadOnly = false;
            }

      
            //if (!int.TryParse(dgvDuLieu.Rows[e.RowIndex].Cells["SoLuong"].Value?.ToString(), out int qty) || qty <= 0)
            //{
            //    dgvDuLieu.Rows[e.RowIndex].Cells["SoLuong"].Style.BackColor = Color.LightCoral;
            //    MessageBox.Show($"Số lượng không hợp lệ: {dgvDuLieu.Rows[e.RowIndex].Cells["TenSanPham"].Value}");
            //    return ;
            //}

        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private List<ImportProductDto> ValidateAndBuildList()
        {
            var result = new List<ImportProductDto>();

            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                bool selected = row.Cells["Chon"].Value != DBNull.Value
                 && Convert.ToBoolean(row.Cells["Chon"].Value);

                if (!selected) continue;

                if (!int.TryParse(row.Cells["SoLuong"].Value?.ToString(), out int qty) || qty <= 0)
                {
                    row.Cells["SoLuong"].Style.BackColor = Color.LightCoral;
                    MessageBox.Show($"{Properties.Messages.Message_ValidNumber}: {row.Cells["TenSanPham"].Value}");
                    return null;
                }
                row.Cells["SoLuong"].Style.BackColor = Color.White;

                result.Add(new ImportProductDto
                {
                    ProductId = row.Cells["MaSanPham"].Value.ToString(),
                    Quantity = qty,
                    UnitPrice = Convert.ToDecimal(row.Cells["GiaTuNhaCungCap"].Value)
                });
            }

            if (result.Count == 0)
            {
                MessageBox.Show($"{Properties.Messages.Message_ChooseAtLeastOneProduct}");
                return null;
            }

            return result;
        }


        private void btnHoanTat_Click(object sender, EventArgs e)
        {
            var listImport = ValidateAndBuildList();
            if (listImport == null || listImport.Count == 0) return;
            foreach (var item in listImport)
            {
                var detailDto = new StockImportDetailDto()
                {

                    ImportId = _importID,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,


                };
                var result = _stockImportDetailSerivce.CreatestockImportDetail(detailDto);
                if (!result.Succeeded)
                {
                    MessageBox.Show(result.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            dataChanged?.Invoke(this, EventArgs.Empty);
            MessageBox.Show("Lưu phiếu nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void dgvDuLieu_CurrentCellDirtyStateChanged_1(object sender, EventArgs e)
        {
            if (dgvDuLieu.IsCurrentCellDirty &&
              dgvDuLieu.CurrentCell.OwningColumn.Name == "Chon")
                dgvDuLieu.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
