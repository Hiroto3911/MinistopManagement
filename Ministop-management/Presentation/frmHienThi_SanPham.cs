using Guna.UI2.WinForms;
using Shared.Security;
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

namespace Presentation
{
    public partial class frmHienThi_SanPham : Form
    {
        private readonly IUserSession _userSession;

        public frmHienThi_SanPham(IUserSession userSession)
        {
            InitializeComponent();
            _userSession = userSession;
            LoadData();
        }

        

       

        private void frmHienThi_SanPham_Load(object sender, EventArgs e)
        {
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlSP.TabPages.Remove(tabSanPham);
                tabControlSP.TabPages.Remove(tabLoaiSanPham);
                tabControlSP.TabPages.Remove(tabNhaCungCap);
                tabControlSP.TabPages.Remove(tabKhuyenMai);
            }

        }

        private void LoadData()
        {
            // ===== 1️⃣ Tạo dữ liệu mẫu =====
            DataTable dt = new DataTable();
            dt.Columns.Add("ProductID");
            dt.Columns.Add("CategoryID");
            dt.Columns.Add("ProductName");
            dt.Columns.Add("Unit");
            dt.Columns.Add("StandardPrice");
            dt.Columns.Add("Status");

            dt.Rows.Add("P001", "C001", "Nước suối Aquafina 500ml", "Chai", 7000, 1);
            dt.Rows.Add("P002", "C002", "Mì Hảo Hảo Tôm Chua Cay", "Gói", 4500, 1);
            dt.Rows.Add("P003", "C003", "Bánh Oreo Socola", "Hộp", 12000, 1);
            dt.Rows.Add("P004", "C001", "Coca-Cola 330ml", "Lon", 10000, 1);
            dt.Rows.Add("P005", "C001", "Pepsi 330ml", "Lon", 9500, 1);
            dt.Rows.Add("P006", "C004", "Bánh mì sandwich", "Gói", 15000, 1);
            dt.Rows.Add("P007", "C005", "Khăn giấy Pulppy", "Gói", 12000, 1);
            dt.Rows.Add("P008", "C006", "Sữa tươi Vinamilk 180ml", "Hộp", 7000, 1);
            dt.Rows.Add("P009", "C007", "Kem Merino Vani", "Cây", 8000, 1);
            dt.Rows.Add("P010", "C002", "Mì Omachi Sườn Hầm Ngũ Quả", "Gói", 5500, 1);

            dgvDuLieu_SanPham.DataSource = dt;
            dgvDuLieu_SanPham.AllowUserToAddRows = false;
            dgvDuLieu_SanPham.ReadOnly = true;
            dgvDuLieu_SanPham.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ===== 2️⃣ Thêm hai cột nút =====
            if (dgvDuLieu_SanPham.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvDuLieu_SanPham.Columns.Add(btnEdit);
            }

            if (dgvDuLieu_SanPham.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieu_SanPham.Columns.Add(btnDelete);
            }

            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu_SanPham.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu_SanPham.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu_SanPham.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu_SanPham.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu_SanPham.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu_SanPham.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieu_SanPham.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvDuLieu_SanPham.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu_SanPham.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvDuLieu_SanPham.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvDuLieu_SanPham.Columns[e.ColumnIndex].Name;
                    TextRenderer.DrawText(
                        e.Graphics,
                        text,
                        new Font("Segoe UI", 9, FontStyle.Bold),
                        e.CellBounds,
                        Color.White,
                        TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                    );

                    e.Handled = true;
                }
            };

            
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string productId = dgvDuLieu_SanPham.Rows[e.RowIndex].Cells["ProductID"].Value.ToString();

            if (dgvDuLieu_SanPham.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmChucNang_SanPham chucNang = new frmChucNang_SanPham();
                chucNang.Show();
            }
            else if (dgvDuLieu_SanPham.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa sản phẩm {productId}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    dgvDuLieu_SanPham.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

      
       

     

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            frmChucNang_SanPham chucNang = new frmChucNang_SanPham();
            chucNang.Show();
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            frmChucNang_PhieuGiamGia chucNang = new frmChucNang_PhieuGiamGia();
            chucNang.Show();
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            frmChucNang_NhaCungCap chucNang = new frmChucNang_NhaCungCap();
            chucNang.Show();
        }

        private void guna2Button15_Click(object sender, EventArgs e)
        {
            frmChucNang_NhaCungCapSanPham  chucNang = new frmChucNang_NhaCungCapSanPham();
            chucNang.Show();
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            frmChucNang_PhieuGiamGia chucNang = new frmChucNang_PhieuGiamGia();
            chucNang.Show();
        }

        private void guna2Button19_Click(object sender, EventArgs e)
        {
            frmChucNang_GiamGiaSP chucNang = new frmChucNang_GiamGiaSP();
            chucNang.Show();
        }

        private void guna2Button21_Click(object sender, EventArgs e)
        {
            frmChucNang_LoaiSanPham chucNang = new frmChucNang_LoaiSanPham();
            chucNang.Show();
        }
    }
}
