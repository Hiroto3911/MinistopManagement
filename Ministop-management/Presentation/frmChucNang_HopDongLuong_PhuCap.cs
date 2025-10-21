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
    public partial class frmChucNang_HopDongLuong_PhuCap : Form
    {
        public frmChucNang_HopDongLuong_PhuCap()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("AllowancesID");
            dt.Columns.Add("AllowancesName");
         

            dt.Rows.Add("PC001", "Đi lại");
            dt.Rows.Add("PC002", "Ăn trưa");
            dt.Rows.Add("PC003", "Tiền bồi dưỡng");


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

           

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

       


        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        //private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.RowIndex < 0) return;

        //    string productId = dgvDuLieu.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();

        //    if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
        //    {
        //        //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        frmChucNang_SanPham chucNang = new frmChucNang_SanPham();
        //        chucNang.Show();
        //    }
        //    else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
        //    {
        //        DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm {productId}?",
        //            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

        //        if (result == DialogResult.Yes)
        //        {
        //            dgvDuLieu.Rows.RemoveAt(e.RowIndex);
        //        }
        //    }
        //}

    }
}
