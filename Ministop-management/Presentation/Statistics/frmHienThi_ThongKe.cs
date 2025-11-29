using Domain.DTO;
using Guna.UI2.WinForms;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;
using Unity;

namespace Presentation
{
    public partial class frmHienThi_ThongKe : Form
    {
        private readonly IReportService _reportService;
        private readonly IUserSession _userSession;
        private readonly IUnityContainer _container;
        private readonly int _totalPageTK;

        public frmHienThi_ThongKe(IReportService reportService, IUserSession userSession, IUnityContainer container)
        {
            InitializeComponent();
            _reportService = reportService;
            _userSession = userSession;
            _container = container;
        }

        private void frmHienThi_ThongKe_Load(object sender, EventArgs e)
        {
            LoadCboCuaHang(cboCuaHangDT);
            LoadCboCuaHang(cboCuaHangTK);
            cboThang.DataSource = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            dtpTuDT.Value = DateTime.Now.AddMonths(-1); // Gợi ý mặc định ✅
            dtpDenDT.Value = DateTime.Now;
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                cboCuaHangDT.Enabled = false;
                cboCuaHangTK.Enabled = false;
                cboCuaHangDT.SelectedValue = _userSession.IdStore;
                cboCuaHangTK.SelectedValue = _userSession.IdStore;

            }

        }
        #region Revenue&Financial
        private void LoadCboCuaHang(Guna2ComboBox cboCuaHang)
        {
            using (var childContainer = _container.CreateChildContainer())
            {
                var storeServices = childContainer.Resolve<IStoreService>();
                var list = storeServices.GetAll();
                if (list.Succeeded == false && list.Data == null) { return; }
                cboCuaHang.DataSource = list.Data;
                cboCuaHang.ValueMember = "StoreID";
                cboCuaHang.DisplayMember = "StoreName";
            }

        }

        private void btnXemTK_Click(object sender, EventArgs e)
        {

            if (cboCuaHangTK.SelectedValue == null)
            {
                MessageBox.Show($"{Properties.Messages.Message_SelectStore}", $"{Properties.Messages.Message_Notification}");
                return;
            }
            string storeID = cboCuaHangTK.SelectedValue.ToString();


            if (!int.TryParse(cboThang.SelectedItem?.ToString(), out int month)
                || month < 1 || month > 12)
            {
                MessageBox.Show($"{Properties.Messages.Message_InvalidMonth}", $"{Properties.Messages.Message_Error}");
                return;
            }


            if (!int.TryParse(txtNam.Text.Trim(), out int year) || year < 2000 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show($"{Properties.Messages.Message_InvalidYear}", $"{Properties.Messages.Message_Error}");
                return;
            }

            LoadDataCH(storeID, month, year);
        }






        private void btnXemDT_Click(object sender, EventArgs e)
        {
            if (cboCuaHangDT.SelectedValue == null)
            {
                MessageBox.Show($"{Properties.Messages.Message_SelectStore}", $"{Properties.Messages.Message_Notification}");
                return;
            }

            string storeID = cboCuaHangDT.SelectedValue.ToString();
            if (rbDoanhThu.Checked)
            {
                DateTime fromDate = dtpTuDT.Value.Date;
                DateTime toDate = dtpDenDT.Value.Date;

                if (fromDate > toDate)
                {
                    MessageBox.Show($"{Properties.Messages.Message_ToDayMoreThanFromDay}", $"{Properties.Messages.Message_Notification}");
                    return;
                }

                // ✅ Gọi service lấy dữ liệu doanh thu
                var result = _reportService.GetRevenueByTimeResults(storeID, fromDate, toDate);

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show($"{Properties.Messages.Message_NotDataAtThisTime}", $"{Properties.Messages.Message_Notification}");
                    chartBieuDo.Series.Clear();
                    return;
                }

                LoadDuLieuLenChart(result);
                LoadDuLieuDaLocTheoTgianMongMuon(storeID, fromDate, toDate);
            }
            else
            {
                var result = _reportService.GetStoreFinancialReportByMonth(storeID);

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show($"{Properties.Messages.Message_NoData}", $"{Properties.Messages.Message_Notification}");
                    chartBieuDo.Series.Clear();
                    return;
                }

                LoadDuLieuTaiChinhLenChart(result);
                LoadDuLieuTaiChinh(result);
            }
        }

        private void LoadDuLieuDaLocTheoTgianMongMuon(string maCH, DateTime tuNgay, DateTime denNgay)
        {
            var data = _reportService.GetRevenueByTimeResults(maCH, tuNgay, denNgay);
            dgvDuLieuDT.DataSource = data;

        }
        private void LoadDuLieuTaiChinh(List<StoreFinancialDto> ds)
        {

            dgvDuLieuDT.DataSource = ds;
        }
        private void LoadDuLieuLenChart(List<StoreRevenueByMonthResultDto> ds)
        {
            chartBieuDo.Series.Clear();
            Series series = new Series($"{Properties.Resources.Chart_Revenue}")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            foreach (var i in ds)
            {
                string label = $"{i.Month}/{i.Year}";
                series.Points.AddXY(label, i.Revenue);
            }

            chartBieuDo.Series.Add(series);
            chartBieuDo.ChartAreas[0].AxisX.Title = $"{Properties.Resources.Chart_MonthYear}";
            chartBieuDo.ChartAreas[0].AxisY.Title = $"{Properties.Resources.Chart_MonthYear} {Properties.Resources.Label_Money}";
        }

        private void LoadDuLieuTaiChinhLenChart(List<StoreFinancialDto> ds)
        {
            chartBieuDo.Series.Clear();
            Series series = new Series($"{Properties.Resources.Chart_Financial}")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true
            };

            foreach (var i in ds)
            {
                string label = $"{i.Month}/{i.Year}";
                series.Points.AddXY(label, i.Financial);
            }

            chartBieuDo.Series.Add(series);
            chartBieuDo.ChartAreas[0].AxisX.Title = $"{Properties.Resources.Chart_MonthYear}";
            chartBieuDo.ChartAreas[0].AxisY.Title = $"{Properties.Resources.Chart_MonthYear} {Properties.Resources.Label_Money}";
        }
        private void ibtnLamMoiDT_Click(object sender, EventArgs e)
        {
            chartBieuDo.Series.Clear();
            dgvDuLieuDT.DataSource = null;
            cboCuaHangDT.SelectedIndex = 0;
            dtpTuDT.Value = DateTime.Now.AddMonths(-1);
            dtpDenDT.Value = DateTime.Now;
        }


        private void ibtnLoadDuLieuTK_Click(object sender, EventArgs e)
        {

        }
        private void rbDoanhThu_CheckedChanged(object sender, EventArgs e)
        {
            dtpDenDT.Enabled = true;
            dtpTuDT.Enabled = true;

        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            dtpDenDT.Enabled = false;
            dtpTuDT.Enabled = false;
        }
        #endregion
        #region Inventory 

        private void LoadDataCH(string storeID, int month = 1, int year = 2025, int quantitywarming = 50)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách cửa hàng =====
            DataTable dt = new DataTable();
            dt.Columns.Add("MaSanPham");
            dt.Columns.Add("TenSanPham");
            dt.Columns.Add("DonVi");
            dt.Columns.Add("TonDauKy");
            dt.Columns.Add("SoLanNhapHang");
            dt.Columns.Add("SoLanXuatHang");
            dt.Columns.Add("SoLanBanHang");
            dt.Columns.Add("SoLanKiemHangThieu");
            dt.Columns.Add("TonCuoiKy");

            using (var childContainer = _container.CreateChildContainer())
            {

                var storeService = childContainer.Resolve<IReportService>();
                var list = storeService.GetInventoryReport(storeID, month, year);
                if (list == null || list.Count == 0)
                {
                    MessageBox.Show($"{Properties.Messages.Message_NoData}", $"{Properties.Messages.Message_Notification}");
                    return;
                }
                foreach (var item in list)
                {
                    dt.Rows.Add(item.ProductID, item.ProductName, item.Unit, item.OpeningStock, item.ImportInPeriod
                               , item.ExportInPeriod, item.SaleInPeriod, item.CheckDecrease, item.ClosingStock);

                }
                dgvDuLieuTK.DataSource = dt;
                dgvDuLieuTK.AllowUserToAddRows = false;
                dgvDuLieuTK.Columns["MaSanPham"].HeaderText = Properties.Resources.Grid_ProductID;
                dgvDuLieuTK.Columns["TenSanPham"].HeaderText = Properties.Resources.Grid_ProductName;
                dgvDuLieuTK.Columns["DonVi"].HeaderText = Properties.Resources.Grid_Unit;
                dgvDuLieuTK.Columns["TonDauKy"].HeaderText = Properties.Resources.Grid_OpeningStock;
                dgvDuLieuTK.Columns["SoLanNhapHang"].HeaderText = Properties.Resources.Grid_ImportInPeriod;
                dgvDuLieuTK.Columns["SoLanXuatHang"].HeaderText = Properties.Resources.Grid_ExportInPeriod;
                dgvDuLieuTK.Columns["SoLanBanHang"].HeaderText = Properties.Resources.Grid_SaleInPeriod;
                dgvDuLieuTK.Columns["SoLanKiemHangThieu"].HeaderText = Properties.Resources.Grid_CheckDecrease;
                dgvDuLieuTK.Columns["TonCuoiKy"].HeaderText = Properties.Resources.Grid_ClosingStock;

                dgvDuLieuTK.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
                dgvDuLieuTK.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
                dgvDuLieuTK.ThemeStyle.HeaderStyle.ForeColor = Color.White;
                dgvDuLieuTK.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                dgvDuLieuTK.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
                dgvDuLieuTK.RowTemplate.Height = 40;
                dgvDuLieuTK.RowPostPaint += (s, e) =>
                {
                    if (e.RowIndex < 0) return;
                    var grid = (Guna2DataGridView)s;
                    var row = grid.Rows[e.RowIndex];
                    long quantity = Convert.ToInt64(row.Cells["TonCuoiKy"].Value.ToString());
                    if (quantity < quantitywarming)
                    {
                        //using (Pen p = new Pen(Color.Red, 4))
                        //{
                        //    int x = e.RowBounds.Left + 1;
                        //    int y = e.RowBounds.Top + 1;
                        //    int y2 = e.RowBounds.Bottom - 1;
                        //    e.Graphics.DrawLine(p,x,y,x,y2);
                        //}

                        row.DefaultCellStyle.BackColor = Color.IndianRed;
                        row.DefaultCellStyle.ForeColor = Color.White;
                    }
                };
            }

        }

        #endregion

        private void label12_Click(object sender, EventArgs e)
        {

        }
    }
}
