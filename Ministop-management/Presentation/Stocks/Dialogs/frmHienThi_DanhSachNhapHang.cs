using Services.Interfaces;
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

namespace Presentation
{
    public partial class frmHienThi_DanhSachNhapHang : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IStockImportService _stockImportService;

        public readonly IUserSession _userSession;
        public string _expenseID;
        public frmHienThi_DanhSachNhapHang(IStockImportService stockImportService, IUserSession userSession, string expenseID = null)
        {
            InitializeComponent();
            _stockImportService = stockImportService;
            _userSession = userSession;
            _expenseID = expenseID;
        }
        private void LoadData()
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Unit");
            dt.Columns.Add("StandardPrice");
          

            dt.Rows.Add("P001", "Nước suối Aquafina 500ml", "Chai", 7000);
            dt.Rows.Add("P002", "Mì Hảo Hảo Tôm Chua Cay", "Gói", 4500);
            dt.Rows.Add("P003", "Bánh Oreo Socola", "Hộp",12000);
            dt.Rows.Add("P004", "Coca-Cola 330ml", "Lon",10000);
            dt.Rows.Add("P005", "Pepsi 330ml", "Lon", 9500);
            dt.Rows.Add("P006", "Bánh mì sandwich", "Gói",15000);
            dt.Rows.Add("P007", "Khăn giấy Pulppy", "Gói",12000);
            dt.Rows.Add("P008", "Sữa tươi Vinamilk 180ml", "Hộp", 7000);
            dt.Rows.Add("P009", "Kem Merino Vani", "Cây", 8000);
            dt.Rows.Add("P010", "Mì Omachi Sườn Hầm Ngũ Quả", "Gói", 5500);

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvDuLieu.Columns["Action"] == null)
            {


                DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
                chk.HeaderText = "Chọn";
                chk.Name = "chkSelect";
                chk.Width = 50;
                dgvDuLieu.Columns.Add(chk);
            }

            dt.Columns.Add("Quanity");
            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

          

        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
