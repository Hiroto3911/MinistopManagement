using CrystalDecisions.ReportAppServer;
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
using System.Xml.Linq;
using Unity;

namespace Presentation.Stocks.Dialogs
{
    //m
    public partial class frmHienThi_LichSuKhoHang : Form
    {
        public event EventHandler dataChanged;
        private readonly IStockHistoryService _stockHistoryService;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private string _stockDetailID;
        private long _totalPage;

        public frmHienThi_LichSuKhoHang(IStockHistoryService stockHistoryService, IUnityContainer container, IUserSession userSession, string stockDetailID = null)
        {
            InitializeComponent();
            _stockHistoryService = stockHistoryService;
            _userSession = userSession;
            _container = container;
            _stockDetailID = stockDetailID;
        }

        private void ibtnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmHienThi_LichSuKhoHang_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_stockDetailID)) return;
            // moi
            LoadData(_stockDetailID);
        }
        private void LoadData(string stockDetailID, int pageNumber = 1, int pageSize = 20)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MaLichSu");
            dt.Columns.Add("NgayThayDoi");
            dt.Columns.Add("LoaiThayDoi");
            dt.Columns.Add("SoLuong");
            dt.Columns.Add("MaThamChieu");
            using (var childContaner = _container.CreateChildContainer())
            {
                var stockExportDetailService = childContaner.Resolve<IStockHistoryService>();
                var list = stockExportDetailService.GetStockHistory(stockDetailID, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) return;
                _totalPage = (long)Math.Ceiling((double)list.TotalCount / pageSize);
                foreach (var item in list.Data)
                {
                    dt.Rows.Add(item.StockHistoryId, item.ChangeDate, item.ChangeType, item.QuantityChange, item.RefId);
                }
            }
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;
            dgvDuLieu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            btnTrangTruoc.Enabled = pageNumber > 1;
            btnTrangSau.Enabled = pageNumber <= _totalPage;

        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            btnTrangTruoc.Enabled = true;
            if (number <= _totalPage)
            {
                var pageNumber = ++number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData(_stockDetailID, pageNumber);
            }
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrang.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrang.Text = pageNumber.ToString();
                LoadData(_stockDetailID, pageNumber);

            }
            else
            {
                btnTrangTruoc.Enabled = false;
            }
        }
    }
}
