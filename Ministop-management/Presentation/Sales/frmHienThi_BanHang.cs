using Domain.DTO;
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
using Unity;
using Unity.Resolution;

namespace Presentation
{
    public partial class frmHienThi_BanHang : Form
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private long _totalPageHD = 1;
        string _invoiceID;
        public frmHienThi_BanHang(IInvoiceService invoiceService, IInvoiceDetailService invoiceDetailService, IUnityContainer container, IUserSession userSession, string invoiceID = null)
        {
            InitializeComponent();
            _invoiceService = invoiceService;
            _invoiceDetailService = invoiceDetailService;
            _container = container;
            _userSession = userSession;
            _invoiceID = invoiceID;
            LoadDataHD(_invoiceID);
        }
        private void LoadDataHD(string invoiceID, int pageNumber = 1, int pageSize = 2)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaHoaDon");
            dt.Columns.Add("NgayLap");
            dt.Columns.Add("TongTien");

            using (var childContainer = _container.CreateChildContainer())
            {
                var invoiceService = childContainer.Resolve<IInvoiceService>();
                var list = invoiceService.GetInvoice(invoiceID, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPageHD = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.InvoiceId, item.InvoiceDate, item.FinalAmount);
                }
            }
            dgvHD.DataSource = dt;
            dgvHD.AllowUserToAddRows = false;
            dgvHD.ReadOnly = true;
            dgvHD.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            if (dgvHD.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvHD.Columns.Add(btnDelete);
            }
            dgvHD.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvHD.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvHD.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvHD.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvHD.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvHD.RowTemplate.Height = 40;
            dgvHD.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvHD.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvHD.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    Color backColor = dgvHD.Columns[e.ColumnIndex].Name == "Edit"
                        ? Color.SeaGreen
                        : Color.IndianRed;

                    using (Brush b = new SolidBrush(backColor))
                        e.Graphics.FillRectangle(b, e.CellBounds);

                    string text = dgvHD.Columns[e.ColumnIndex].Name;
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
            btnTrangTruocHD.Enabled = pageNumber > 1;
            btnTrangSauHD.Enabled = pageNumber <= _totalPageHD;
        }



        private void guna2Button5_Click(object sender, EventArgs e)
        {
            frmChucNang_PhieuTraHang child = new frmChucNang_PhieuTraHang();
            child.Show();
        }

        private void dgvHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnTrangTruocHD_Click(object sender, EventArgs e)
        {

        }

        private void btnTrangSauHD_Click(object sender, EventArgs e)
        {

        }

        private void btnThemHD_Click(object sender, EventArgs e)
        {

        }
        private void HienThiCNCTHD(string invoiceID)
        {
            var frmChucNangChiTietHoaDon = _container.Resolve<frmChucNang_ChiTietHoaDon>(new ParameterOverride("invoiceID", invoiceID));
            frmChucNangChiTietHoaDon.ShowDialog();
        }

        private void frmHienThi_BanHang_DoubleClick(object sender, EventArgs e)
        {
            HienThiCNCTHD(_invoiceID);
        }
    }
}
