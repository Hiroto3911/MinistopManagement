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
using Unity;

namespace Presentation
{
    public partial class frmThungRac_SP : Form
    {
        private readonly IProductService _productService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        public event EventHandler datachanged;
        
        public frmThungRac_SP(IProductService supplierService, IUnityContainer container, IUserSession userSession )
        {
            InitializeComponent();
            _productService = supplierService;
            _container = container;
            _userSession = userSession;
            LoadData();
        }
        private void LoadData()
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenLoaiSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("DonVi");
            dt.Columns.Add("GiaTieuChuan");
            using (var childContainer = _container.CreateChildContainer())
            {
                var productService = childContainer.Resolve<IProductService>();


                var list = productService.GetAllProductIsDelete();
                if (list.Succeeded == false && list.Data == null) { return; }
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.ProductId, item.CategoryId , item.ProductName, item.Unit,item.StandardPrice);
                }
            }

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = false;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvDuLieu.Columns["chkSelect"] == null)
            {


                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.HeaderText = "Chọn";
                chk.Name = "chkSelect";
                chk.Width = 50;
                dgvDuLieu.Columns.Add(chk);
            }
            foreach (DataGridViewColumn col in dgvDuLieu.Columns)
            {
                if (col.Name != "chkSelect")
                    col.ReadOnly = true;
            }
            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;



        }
        private List<string> GetSelectedProduct()
        {
            var list = new List<string>();
            foreach (DataGridViewRow row in dgvDuLieu.Rows)
            {
                var ischecked = Convert.ToBoolean(row.Cells["chkSelect"].Value);
                if (ischecked == true)
                {
                    list.Add(row.Cells["MaSanPham"].Value.ToString());
                }
            }
            return list;
        }

        private void btnKhoiPhuc_Click(object sender, EventArgs e)
        {
            var list = GetSelectedProduct();
            if (list == null || list.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn ít nhất một sản phẩm để khôi phục.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var result = _productService.RestoreProduct(list);
            if (result.Succeeded == false)
            {
                MessageBox.Show($"Khôi phục thất bại: {result.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Khôi phục thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            datachanged?.Invoke(this, EventArgs.Empty);
            this.Close();
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string productId = dgvDuLieu.Rows[e.RowIndex].Cells["MaSanPham"].Value.ToString();

            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "chkSelect")
            {

                dgvDuLieu.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void itbnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
