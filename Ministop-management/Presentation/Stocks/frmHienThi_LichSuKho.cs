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
    public partial class frmHienThi_LichSuKho : Form
    {
        public frmHienThi_LichSuKho()
        {
            InitializeComponent();
            LoadDetailData();
        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void LoadDetailData()
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu cho lịch sử kho =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaThamChieu", typeof(string));
            dt.Columns.Add("LoaiThayDoi", typeof(string));
            dt.Columns.Add("NgayThayDoi", typeof(DateTime));
            dt.Columns.Add("SoLuongThayDoi", typeof(int));

            string[] loaiThayDoi = { "Nhập hàng", "Xuất hàng" };
            Random rnd = new Random();

            // Giả lập ngẫu nhiên 3-5 bản ghi lịch sử
            int soDong = rnd.Next(3, 6);
            for (int i = 0; i < soDong; i++)
            {
                string maThamChieu = $"LS{rnd.Next(100, 999)}"; // ví dụ: LS123
                string loai = loaiThayDoi[rnd.Next(0, 2)];
                DateTime ngay = DateTime.Now.AddDays(-rnd.Next(1, 30));
                int soLuong = rnd.Next(10, 200);
                dt.Rows.Add(maThamChieu, loai, ngay, soLuong);
            }
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

    }
}
