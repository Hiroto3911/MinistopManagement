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
    public partial class frmHienThi_KhoHang : Form
    {
        private readonly IUserSession _userSession;

        public frmHienThi_KhoHang(IUserSession userSession)
        {
            InitializeComponent();
            _userSession = userSession;
            LoadData();
        }

   

      

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            frmChucNang_XuatKho chucNang = new frmChucNang_XuatKho();
            chucNang.Show();
        }

        

     

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            frmChucNang_ChiTietKiemKho chucNang = new frmChucNang_ChiTietKiemKho();
            chucNang.Show();
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            frmChucNang_NhapKho chucNang = new frmChucNang_NhapKho();
            chucNang.Show();
        }
        private void LoadData()
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu cho bảng Chi tiết kho =====
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("SoLuongTon", typeof(int));
            dt.Columns.Add("GiaBan", typeof(decimal));
            dt.Columns.Add("LanCapNhatCuoi", typeof(DateTime));

            // ===== 2️⃣ Dữ liệu mẫu =====
            dt.Rows.Add(1,"Nước suối Aquafina 500ml", 120, 7000, DateTime.Now.AddDays(-2));
            dt.Rows.Add(2,"Mì Hảo Hảo Tôm Chua Cay", 300, 4500, DateTime.Now.AddDays(-1));
            dt.Rows.Add(3,"Bánh Oreo Socola", 80, 12000, DateTime.Now.AddDays(-5));
            dt.Rows.Add(4,"Coca-Cola 330ml", 200, 10000, DateTime.Now.AddHours(-12));
            dt.Rows.Add(5,"Pepsi 330ml", 150, 9500, DateTime.Now.AddDays(-3));
            dt.Rows.Add(6,"Bánh mì sandwich", 60, 15000, DateTime.Now.AddDays(-7));
            dt.Rows.Add(7,"Khăn giấy Pulppy", 90, 12000, DateTime.Now.AddDays(-4));
            dt.Rows.Add(8,"Sữa tươi Vinamilk 180ml", 220, 7000, DateTime.Now.AddDays(-2));
            dt.Rows.Add(9,"Kem Merino Vani", 130, 8000, DateTime.Now.AddDays(-1));
            dt.Rows.Add(10,"Mì Omachi Sườn Hầm Ngũ Quả", 170, 5500, DateTime.Now.AddHours(-10));

            // ===== 3️⃣ Gán dữ liệu vào DataGridView =====
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

          

            // ===== 5️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

        }

        private void dgvDuLieu_DoubleClick(object sender, EventArgs e)
        {
            if(dgvDuLieu.CurrentCell == null && dgvDuLieu.Rows.Count ==0)
            {
                return;
            }
            //var dong = dgvDuLieu.CurrentCell.RowIndex; 
            LoadDetailData();


        }

        private void LoadDetailData()
        {
            frmHienThi_LichSuKho chucNang = new frmHienThi_LichSuKho(); 
            chucNang.ShowDialog();
        }

        private void frmHienThi_KhoHang_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Nhân viên")
            {
                tabControlKH.TabPages.Remove(tabChiTietKho);
                tabControlKH.TabPages.Remove(tabNhapHang);
            }
        }
    }
}
