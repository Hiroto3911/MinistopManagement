using Services.Interfaces;
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
using Unity.Resolution;

namespace Presentation.Products
{
    public partial class frmHienThi_GiamGiaSanPham : Form
    {
        private readonly IUnityContainer _container;
        private readonly IPromotionProductService _promotionProductService;
        private string _promotionId;
        private long _totalPageGGSPP = 1;
        public frmHienThi_GiamGiaSanPham(IUnityContainer container, IPromotionProductService promotionService, string promotionID = null )
        {
            InitializeComponent();
            _container = container;
            _promotionProductService = promotionService;
            _promotionId = promotionID;
            LoadDataGGSP(_promotionId);
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }

            string promotionProductId = dgvDuLieu.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                var frmHienThi_GiamGiaSanPham = _container.Resolve<frmChucNang_GiamGiaSP>(new ParameterOverride("promotionProductId", promotionProductId));
                frmHienThi_GiamGiaSanPham.DataChanged += (s, ev) =>
                {
                    LoadDataGGSP(_promotionId);
                };
                frmHienThi_GiamGiaSanPham.ShowDialog();
            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult question = MessageBox.Show($"bạn có chắc chắn muốn xóa {promotionProductId} của nhà cung cấp này không!!", "xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (question == DialogResult.Yes)
                {
                    _promotionProductService.RemovePromotionProduct(promotionProductId);
                    MessageBox.Show("Xóa thành công");
                    LoadDataGGSP(_promotionId);
                }

            }
        }
        private void LoadDataGGSP(string promotionId, int pageNumber = 1, int pageSize = 5)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("MaPhieuGiamGia");
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("SoTienGiam");
            dt.Columns.Add("SoLuongToiThieu");
            dt.Columns.Add("GhiChu");
            using (var childContainer = _container.CreateChildContainer())
            {
                var promotionProductService = childContainer.Resolve<IPromotionProductService>();
                var list = promotionProductService.GetPromotionProduct(promotionId, pageNumber, pageSize);

                if (!list.Succeeded || list.Data == null)
                    return;

                _totalPageGGSPP = (long)Math.Ceiling((double)list.TotalCount / pageSize);

                foreach (var item in list.Data)
                {

                    dt.Rows.Add(item.Id, item.PromotionId, item.ProductId, item.ProductName, item.DiscountAmount, item.MinQuantity,item.Note);
                }
            }

            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Thêm nút Edit, Delete nếu chưa có
            if (dgvDuLieu.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
                {
                    Name = "Edit",
                    HeaderText = "Edit",
                    Text = "Edit",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu.Columns.Add(btnEdit);
            }

            if (dgvDuLieu.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
                {
                    Name = "Delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
                dgvDuLieu.Columns.Add(btnDelete);
            }

            // Giao diện đẹp
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

            // Vẽ lại nút Edit/Delete
            dgvDuLieu.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 &&
                    (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" || dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);
                    Color backColor = dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvDuLieu.Columns[e.ColumnIndex].Name;
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
            btnTrangTruocGGSP.Enabled = pageNumber > 1;
            btnTrangSauGGSP.Enabled = pageNumber < _totalPageGGSPP;
        }

        private void btnTrangSauGGSP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangGGSP.Text);
            btnTrangTruocGGSP.Enabled = true;
            if (number <= _totalPageGGSPP)
            {
                var pageNumber = ++number;
                txtTrangGGSP.Text = pageNumber.ToString();
                LoadDataGGSP(_promotionId, pageNumber);
            }
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            var frmPGG = _container.Resolve<frmChucNang_GiamGiaSP>(new ParameterOverride("promotionID", _promotionId));

            frmPGG.DataChanged += (s, ev) =>
            {
                LoadDataGGSP(_promotionId);
            };

            frmPGG.ShowDialog();
        }

        private void frmHienThi_GiamGiaSanPham_Load(object sender, EventArgs e)
        {

        }

        private void btnTrangTruocGGSP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtTrangGGSP.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtTrangGGSP.Text = pageNumber.ToString();
                LoadDataGGSP(_promotionId, pageNumber);

            }
            else
            {
                btnTrangTruocGGSP.Enabled = false;
            }
        }
    }
}
