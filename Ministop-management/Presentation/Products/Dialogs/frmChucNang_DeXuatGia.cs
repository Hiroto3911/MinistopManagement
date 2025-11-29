using Domain.DTO;
using Services.Interfaces;
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

namespace Presentation
{
    public partial class frmChucNang_DeXuatGia : Form
    {
        public event EventHandler<string> dataChanged;
        public readonly IPriceProposalService _priceProposalService;
        public readonly IStockDetailService _stockDetailService;
        public readonly IUserSession _userSession;
        public string _priceProposalID;
        public string _productID;

        public frmChucNang_DeXuatGia(IPriceProposalService priceProposalService, IStockDetailService stockDetailService, IUserSession userSession, string priceProposalID = null, string productID = null)
        {
            InitializeComponent();
            _priceProposalService = priceProposalService;
            _stockDetailService = stockDetailService;
            _userSession = userSession;
            _priceProposalID = priceProposalID;
            _productID = productID;
        }

        private void LoadDataCboTrangThai()
        {
            dtpNgayDX.Value = DateTime.UtcNow.ToLocalTime();
            cboTrangThai.Enabled = true;
            cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
            Dictionary<string, byte> status;
            if (_userSession.Role == "Admin")
            {
                status = new Dictionary<string, byte>()
                {
                 {Properties.Resources.Status_Permitted,1 },
                 {Properties.Resources.Status_NotPermitted,0 }
                };
            }
            else
            {
                status = new Dictionary<string, byte>()
               {
                {Properties.Resources.Status_Draft,2 },
                 {Properties.Resources.Status_Pending,3 }
               };
            }
            cboTrangThai.DataSource = status.ToList();
            cboTrangThai.DisplayMember = "Key";
            cboTrangThai.ValueMember = "Value";
            
        }
        private void frmChucNang_ChiPhiCuaHang_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(_priceProposalID))
            {
                var entity = _priceProposalService.GetpriceProposaByID(_priceProposalID);
                if (entity.Succeeded == false && entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", $"{Properties.Messages.Message_Error}");
                    return;
                }
                txtCuaHang.Text = entity.Data.StoreId;
                txtMaQL.Text = entity.Data.ManagerId.ToString() ?? "";
                txtMaSP.Text = entity.Data.ProductId.ToString() ?? "";
                txtGiaCu.Text = entity.Data.OldPrice.ToString() ?? "0.0";
                txtGiaMoi.Text = entity.Data.NewPrice.ToString() ?? "0.0";

            }
            else
            {
                cboTrangThai.Enabled = false;
            }
        }
        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmChucNang_DeXuatGia_Load(object sender, EventArgs e)
        {
            LoadDataCboTrangThai();
            if (_userSession.Role == "Admin")
            {
                cboTrangThai.SelectedIndex = 0;
            }
            if (!string.IsNullOrEmpty(_priceProposalID))
            {
                var entity = _priceProposalService.GetpriceProposaByID(_priceProposalID);
                if (entity.Succeeded == false || entity.Data == null)
                {
                    MessageBox.Show($"{entity.Message}", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // Đổ dữ liệu vào form
                txtCuaHang.Text = entity.Data.StoreId;
                txtMaQL.Text = entity.Data.ManagerId ?? "";
                txtMaSP.Text = entity.Data.ProductId ?? "";
                txtGiaCu.Text = entity.Data.OldPrice.ToString();
                txtGiaMoi.Text = entity.Data.NewPrice.ToString();
                rtbLyDo.Text = entity.Data.Reason ?? "";

                if (_userSession.Role == "Admin")
                {
                    // Admin: Luôn chọn "Duyệt" (item đầu tiên), bỏ qua status từ DB
                    cboTrangThai.SelectedIndex = 0;
                }
                else
                {
                    // Quản lý: Set theo status từ DB
                    cboTrangThai.SelectedValue = entity.Data.Status;
                }

                // Load tên sản phẩm
                if (!string.IsNullOrEmpty(entity.Data.ProductId))
                {
                    var stockResult = _stockDetailService.GetStockDetailByProductID(entity.Data.ProductId);
                    if (stockResult.Succeeded && stockResult.Data != null)
                    {
                        txtTenSP.Text = stockResult.Data.ProductName;
                    }
                }

                // Admin chỉ duyệt, không sửa
                if (_userSession.Role == "Admin")
                {
                    txtMaSP.Enabled = false;
                    txtGiaMoi.Enabled = false;
                    rtbLyDo.Enabled = false;
                }
            }
            // Trường hợp THÊM MỚI - có _productID
            else if (!string.IsNullOrEmpty(_productID))
            {
                txtCuaHang.Text = _userSession.IdStore;
                txtMaQL.Text = _userSession.UserId;
                txtMaSP.Text = _productID;

                var entity = _stockDetailService.GetStockDetailByProductID(_productID);
                if (entity.Succeeded && entity.Data != null)
                {
                    txtGiaCu.Text = entity.Data.Price.ToString();
                    txtTenSP.Text = entity.Data.ProductName;
                }

                cboTrangThai.Enabled = false; // Thêm mới thì không cho chọn trạng thái
            }
            // Trường hợp mở form trống
            else
            {
                txtCuaHang.Text = _userSession.IdStore;
                txtMaQL.Text = _userSession.UserId;
                cboTrangThai.Enabled = false;
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // 1. Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(txtCuaHang.Text))
            {
                MessageBox.Show("Vui lòng nhập cửa hàng.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMaQL.Text))
            {
                MessageBox.Show("Vui lòng nhập mã quản lý.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMaSP.Text))
            {
                MessageBox.Show("Vui lòng nhập mã sản phẩm.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtGiaMoi.Text))
            {
                MessageBox.Show("Vui lòng nhập giá mới.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaMoi.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaMoi.Text, out decimal newPrice) || newPrice <= 0)
            {
                MessageBox.Show("Giá mới phải là số dương hợp lệ.", "Dữ liệu không hợp lệ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtGiaMoi.Focus();
                return;
            }

            if (!decimal.TryParse(txtGiaCu.Text, out decimal oldPrice))
            {
                oldPrice = 0; // mặc định nếu lỗi
            }

            if (string.IsNullOrWhiteSpace(rtbLyDo.Text))
            {
                MessageBox.Show("Vui lòng nhập lý do đề xuất giá.", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                rtbLyDo.Focus();
                return;
            }

            // 2. Xác định trạng thái
            byte status = 2; // Đang soạn (mặc định khi thêm mới)
            if (cboTrangThai.Enabled && cboTrangThai.SelectedValue != null)
            {
                status = (byte)cboTrangThai.SelectedValue;
            }

            // 3. Tạo DTO
            var priceProposalDto = new PriceProposalDto
            {
                ProposalId = _priceProposalID,
                StoreId = txtCuaHang.Text.Trim(),
                ProductId = txtMaSP.Text.Trim(),
                ManagerId = txtMaQL.Text.Trim(),
                OldPrice = oldPrice,
                NewPrice = newPrice,
                Reason = rtbLyDo.Text.Trim(),
                Status = status
            };

            // 4. Gọi service (có try-catch)
            Result<bool> result;
            if (string.IsNullOrEmpty(_priceProposalID))
            {
                result = _priceProposalService.CreatepriceProposal(priceProposalDto);
            }
            else
            {
                result = _priceProposalService.UpdatepriceProposalDto(priceProposalDto);
            }
            // 5. Xử lý kết quả
            if (result.Succeeded)
            {
                MessageBox.Show(
                    string.IsNullOrEmpty(_priceProposalID) ? "Tạo đề xuất giá thành công!" : "Cập nhật đề xuất giá thành công!",
                    "Thành công",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                dataChanged?.Invoke(this, _priceProposalID ?? "refresh");
                this.Close();
            }
            else
            {
                MessageBox.Show(result.Message ?? "Đã xảy ra lỗi không xác định.", $"{Properties.Messages.Message_Error}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void loadData(string productID)
        {
            var result = _stockDetailService.GetStockDetailByProductID(productID);
            if (result.Succeeded && result.Data != null)
            {
                txtTenSP.Text = result.Data.ProductName;
                txtGiaCu.Text = result.Data.Price.ToString();
            }
            else
            {
                txtTenSP.Text = string.Empty;
                txtGiaCu.Text = string.Empty;
            }
        }
        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
            string masp = txtMaSP.Text.Trim();
            if (string.IsNullOrEmpty(masp))
            {
                return;
            }
            loadData(masp);
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
