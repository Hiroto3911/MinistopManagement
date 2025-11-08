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

        public frmHienThi_ThongKe(IReportService reportService ,IUserSession userSession, IUnityContainer container)
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
                MessageBox.Show("Vui lòng chọn cửa hàng!", "Thông báo");
                return;
            }
            string storeID = cboCuaHangTK.SelectedValue.ToString();


            if (!int.TryParse(cboThang.SelectedItem?.ToString(), out int month)
                || month < 1 || month > 12)
            {
                MessageBox.Show("Tháng không hợp lệ!", "Thông báo");
                return;
            }


            if (!int.TryParse(txtNam.Text.Trim(), out int year) || year < 2000 || year > DateTime.Now.Year + 1)
            {
                MessageBox.Show("Năm không hợp lệ!", "Thông báo");
                return;
            }

            LoadDataCH(storeID, month, year);
        }



     


        private void btnXemDT_Click(object sender, EventArgs e)
        {
            if (cboCuaHangDT.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn cửa hàng!", "Thông báo");
                return;
            }

            string storeID = cboCuaHangDT.SelectedValue.ToString();
            if (rbDoanhThu.Checked)
            {
                DateTime fromDate = dtpTuDT.Value.Date;
                DateTime toDate = dtpDenDT.Value.Date;

                if (fromDate > toDate)
                {
                    MessageBox.Show("Từ ngày phải nhỏ hơn Đến ngày!", "Lỗi");
                    return;
                }

                // ✅ Gọi service lấy dữ liệu doanh thu
                var result = _reportService.GetRevenueByTimeResults(storeID, fromDate, toDate);

                if (result == null || result.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu trong khoảng thời gian này!", "Thông báo");
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
                    MessageBox.Show("Không có dữ liệu !", "Thông báo");
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
            dgvDuLieuDT.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void LoadDuLieuTaiChinh(List<StoreFinancialDto> ds)
        {
     
            dgvDuLieuDT.DataSource = ds;
            dgvDuLieuDT.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void LoadDuLieuLenChart(List<StoreRevenueByMonthResultDto> ds)
        {
            chartBieuDo.Series.Clear();
            Series series = new Series("DoanhThu")
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
            chartBieuDo.ChartAreas[0].AxisX.Title = "Tháng/Năm";
            chartBieuDo.ChartAreas[0].AxisY.Title = "Doanh thu (VNĐ)";
        }

        private void LoadDuLieuTaiChinhLenChart(List<StoreFinancialDto> ds)
        {
            chartBieuDo.Series.Clear();
            Series series = new Series("TaiChinh")
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
            chartBieuDo.ChartAreas[0].AxisX.Title = "Tháng/Năm";
            chartBieuDo.ChartAreas[0].AxisY.Title = "Tai Chinh (VNĐ)";
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


        private void LoadDataCH(string storeID , int month = 1, int year= 2025)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách cửa hàng =====
            using (var childContainer = _container.CreateChildContainer())
            {
                var storeService = childContainer.Resolve<IReportService>();
                var list = storeService.GetInventoryReport(storeID, month,year);
                if (list == null || list.Count == 0)
                {
                    dgvDuLieuTK.DataSource = null;
                    MessageBox.Show("Không có dữ liệu tồn kho trong tháng này!", "Thông báo");
                    return;
                }
                dgvDuLieuTK.DataSource = list;
                dgvDuLieuTK.AllowUserToAddRows = false;
                dgvDuLieuTK.ReadOnly = true;
                dgvDuLieuTK.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            }

            dgvDuLieuTK.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieuTK.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieuTK.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieuTK.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieuTK.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieuTK.RowTemplate.Height = 40;



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
    }
}
