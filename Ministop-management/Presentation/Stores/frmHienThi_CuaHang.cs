using Guna.UI2.WinForms;
using Services.Interfaces;
using Shared.Security;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Unity;
using Unity.Resolution;


namespace Presentation
{
    public partial class frmHienThi_CuaHang : Form
    {
        private readonly IStoreService _storeService;
        private readonly IStoreFixedExpenseServices _storeFixedExpenseServices;
        private readonly IUnityContainer _container;
        private readonly IUserSession _userSession;
        private readonly IEmployeeService _employeeService;
        private long _totalPageCH = 1;
        private long _totalPageCP = 1;
        private string _lang = Properties.Settings.Default.Language;
        private string _expenseId;

        public frmHienThi_CuaHang(IStoreService storeService, IStoreFixedExpenseServices storeFixedExpenseServices, IUnityContainer container, IEmployeeService employeeService, IUserSession userSession)
        {
            InitializeComponent();
            _storeService = storeService;
            _storeFixedExpenseServices = storeFixedExpenseServices;
            _container = container;
            _userSession = userSession;
            _employeeService = employeeService;
            LoadDataCH();
        }
        private void frmHienThi_CuaHang_Load(object sender, EventArgs e)
        {
            LoadCboCuaHang();
            if (_userSession.Role == "Quản lý cửa hàng")
            {
                tabControlCH.TabPages.Remove(tabCuaHang);
                cboCuaHang.SelectedValue = _userSession.IdStore;
            }
            else if (_userSession.Role == "Admin")
            {
                cboCuaHang.Enabled = true;
                btnThemCP.Visible = false;
            }
            LoadDataCP(cboCuaHang.SelectedValue.ToString());
        }

        #region ChucNangCuaHang
        private void dgvDuLieu_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string storeId;
            if (_lang == "en-US")
            {
                storeId = dgvDuLieu.Rows[e.RowIndex].Cells["StoreID"].Value.ToString();
            }
            else
            {
                storeId = dgvDuLieu.Rows[e.RowIndex].Cells["MaCuaHang"].Value.ToString();
            }

            if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCH = _container.Resolve<frmChucNang_CuaHang>(new ParameterOverride("storeId", storeId));
                frmChucNangCH.DataChanged += (s, ev) =>
                {

                    LoadDataCH();
                };
                frmChucNangCH.ShowDialog();



            }
            else if (dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa cửa hàng {storeId}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    var isChecked = _employeeService.AnyStore(storeId);
                    if (isChecked.Data == true)
                    {
                        DialogResult resultCon = MessageBox.Show($"Cửa hàng {storeId} hiện đang còn dữ liệu và tài khoản hoạt động.\n Nếu bạn xác nhận xoá, hệ thống sẽ ngưng kích hoạt cửa hàng và các dữ liệu liên quan, thay vì xoá vĩnh viễn.",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (resultCon != DialogResult.Yes) return;
                        _storeService.RemoveSoftStore(storeId);
                        MessageBox.Show("Xóa thành công!");
                        LoadDataCH(); // tải lại dữ liệu
                        return;

                    }
                    _storeService.RemoveSoftStore(storeId);
                    MessageBox.Show("Xóa thành công!");
                    LoadDataCH(); // tải lại dữ liệu
                }
            }
        }
        private void LoadDataCH(int pageNumber = 1, int pageSize = 20)
        {
            // ===== 1️⃣ Tạo DataTable cho danh sách cửa hàng =====
            DataTable dt = new DataTable();
            if(_lang == "en-US")
            {
                dt.Columns.Add("StoreID");
                dt.Columns.Add("StoreName");
                dt.Columns.Add("Address");
                dt.Columns.Add("PhoneNumber");
            }
            else
            {
                dt.Columns.Add("MaCuaHang");
                dt.Columns.Add("TenCuaHang");
                dt.Columns.Add("DiaChi");
                dt.Columns.Add("SoDienThoai");
            }

                using (var childContainer = _container.CreateChildContainer())
                {
                    var storeService = childContainer.Resolve<IStoreService>();
                    var list = storeService.GetStore(pageNumber, pageSize);
                    if (list.Succeeded == false && list.Data == null) { return; }
                    _totalPageCH = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                    foreach (var item in list.Data)
                    {
                        dt.Rows.Add(item.StoreId, item.StoreName, item.Address, item.Phone);
                    }
                }
            // ===== 2️⃣ Dữ liệu mẫu (có thể thay bằng dữ liệu trong DB sau này) =====
            dgvDuLieu.DataSource = dt;
            dgvDuLieu.AllowUserToAddRows = false;
            dgvDuLieu.ReadOnly = true;

            // ===== 2️⃣ Thêm hai cột nút =====

            if (dgvDuLieu.Columns["Edit"] == null)
            {
                DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn();
                btnEdit.Name = "Edit";
                btnEdit.HeaderText = "Edit";
                btnEdit.Text = "Edit";
                btnEdit.UseColumnTextForButtonValue = true;
                dgvDuLieu.Columns.Add(btnEdit);
            }
            if (dgvDuLieu.Columns["Delete"] == null)
            {
                DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn();
                btnDelete.Name = "Delete";
                btnDelete.HeaderText = "Delete";
                btnDelete.Text = "Delete";
                btnDelete.UseColumnTextForButtonValue = true;
                dgvDuLieu.Columns.Add(btnDelete);
            }

            ApplyGridStyle(dgvDuLieu);
            btnTrangTruocCH.Enabled = pageNumber > 1;
            btnTrangSauCH.Enabled = pageNumber <= _totalPageCH;


        }



        private void btnThemCuaHang_Click(object sender, EventArgs e)
        {
            var frmChucNangCH = _container.Resolve<frmChucNang_CuaHang>();
            frmChucNangCH.DataChanged += (s, ev) => LoadDataCH();
            frmChucNangCH.ShowDialog();


        }

        private void btnTrangSauCH_Click(object sender, EventArgs e)
        {

            int number = Convert.ToInt32(txtSoTrangCH.Text);
            btnTrangTruocCH.Enabled = true;
            if (number <= _totalPageCH)
            {
                var pageNumber = ++number;
                txtSoTrangCH.Text = pageNumber.ToString();
                LoadDataCH(pageNumber);
            }


        }

        private void btnTrangTruocCH_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCH.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangCH.Text = pageNumber.ToString();
                LoadDataCH(pageNumber);

            }
            else
            {
                btnTrangTruocCH.Enabled = false;
            }
        }

        private void ibtnDuLieuBiXoa_Click(object sender, EventArgs e)
        {
            var frmThungRac = _container.Resolve<frmThungRac_CuaHang>();
            frmThungRac.datachanged += (s, ev) => LoadDataCH();
            frmThungRac.ShowDialog();
        }
        #endregion

        #region thiet ke giao dien Cua Hang
        private void ApplyGridStyle(Guna2DataGridView dgvDuLieu)
        {
            // ===== 3️⃣ Chỉnh style chung cho bảng =====
            dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(33, 150, 243);
            dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDuLieu.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvDuLieu.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9);
            dgvDuLieu.RowTemplate.Height = 40;

            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieu.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && (dgvDuLieu.Columns[e.ColumnIndex].Name == "Edit" ||
                                        dgvDuLieu.Columns[e.ColumnIndex].Name == "Delete"))
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
        }


        #endregion

        #region Chi phi co dinh cua hang
        private void cboCuaHang_SelectedValueChanged(object sender, EventArgs e)
        {
            LoadDataCP(cboCuaHang.SelectedValue.ToString());
        }
        private void LoadCboCuaHang()
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
        private void LoadDataCP(string storeId, int pageNumber = 1, int pageSize = 20)
        {

            // ===== 1️⃣ Tạo DataTable cho danh sách cửa hàng =====
            DataTable dt = new DataTable();
            if (_lang == "en-US")
            {
                dt.Columns.Add("ExpenseID");
                dt.Columns.Add("Store");
                dt.Columns.Add("RentCost");
                dt.Columns.Add("ElectricityCost");
                dt.Columns.Add("WaterCost");
                dt.Columns.Add("Note");
                dt.Columns.Add("Status");
            }
            else
            {
                dt.Columns.Add("MaChiPhi");
                dt.Columns.Add("CuaHang");
                dt.Columns.Add("TienThueMatBang");
                dt.Columns.Add("TienDien");
                dt.Columns.Add("TienNuoc");
                dt.Columns.Add("GhiChu");
                dt.Columns.Add("TrangThai");
            }

            using (var childContainer = _container.CreateChildContainer())
            {
                var expenseService = childContainer.Resolve<IStoreFixedExpenseServices>();
                var list = expenseService.GetStoreFixedExpense(storeId, pageNumber, pageSize);
                if (list.Succeeded == false && list.Data == null) { return; }
                _totalPageCP = (long)Math.Ceiling((double)(list.TotalCount / pageSize));
                if (_userSession.Role == "Admin")
                {
                    if (_lang == "en-US")
                    {
                        dt.Columns.Add("LastModifiedBy");
                        dt.Columns.Add("LastModified");
                    }
                    else
                    {
                        dt.Columns.Add("NguoiSuaDoiLanCuoi");
                        dt.Columns.Add("NgaySuaDoiLanCuoi");
                    }
                    foreach (var item in list.Data)
                    {
                        dt.Rows.Add(item.ExpenseId, item.StoreName, item.RentCost, item.ElectricityCost, item.WaterCost, item.Note, item.Status, item.LastModifiedBy , item.LastModified.ToString());
                    }
                }
                else
                {
                    foreach (var item in list.Data)
                    {
                        dt.Rows.Add(item.ExpenseId, item.StoreName, item.RentCost, item.ElectricityCost, item.WaterCost, item.Note,item.Status);
                    }
                }
            }
            // ===== 2️⃣ Dữ liệu mẫu (có thể thay bằng dữ liệu trong DB sau này) =====
            dgvDuLieuCP.DataSource = dt;
            dgvDuLieuCP.AllowUserToAddRows = false;
            dgvDuLieuCP.ReadOnly = true;
    

            // ===== 2️⃣ Thêm hai cột nút =====

            //if (_isEditable == true)
            //{

                if (dgvDuLieuCP.Columns["Edit"] == null)
                {
                    DataGridViewButtonColumn btnEdit = new DataGridViewButtonColumn
                    {
                        Name = "Edit",
                        HeaderText = "Edit",
                        Text = "Edit",
                        UseColumnTextForButtonValue = true
                    };
                    dgvDuLieuCP.Columns.Add(btnEdit);
                }

                if (dgvDuLieuCP.Columns["Delete"] == null && _userSession.Role != "Admin")
                {
                    DataGridViewButtonColumn btnDelete = new DataGridViewButtonColumn
                    {
                        Name = "Delete",
                        HeaderText = "Delete",
                        Text = "Delete",
                        UseColumnTextForButtonValue = true
                    };
                    dgvDuLieuCP.Columns.Add(btnDelete);
                }
            //}
            //else
            //{
                // Nếu đã chốt phiếu thì ẩn (hoặc xóa) hai cột này nếu có
                //if (dgvDuLieuCP.Columns["Edit"] != null)
                //    dgvDuLieuCP.Columns.Remove("Edit");
                //if (dgvDuLieuCP.Columns["Delete"] != null)
                //    dgvDuLieuCP.Columns.Remove("Delete");
            //}


            // ===== 4️⃣ Đổi màu nút Edit/Delete =====
            dgvDuLieuCP.RowPostPaint += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var grid = (Guna2DataGridView)s;

                var row = grid.Rows[e.RowIndex];
                string status;
                if (_lang == "en-US")
                {
                    status = row.Cells["Status"].Value?.ToString();
                }
                else
                {
                   status  = row.Cells["TrangThai"].Value?.ToString();
                }
                    

                if (status == "0") // bị từ chối
                {
                    using (Pen p = new Pen(Color.Red, 5)) // viền trái đỏ, dày 4px
                    {
                        int x = e.RowBounds.Left + 1;
                        int y1 = e.RowBounds.Top + 1;
                        int y2 = e.RowBounds.Bottom - 1;

                        e.Graphics.DrawLine(p, x, y1, x, y2);
                    }
                }
            };


            dgvDuLieuCP.CellPainting += (s, e) =>
            {
                if (e.RowIndex < 0) return;

                var grid = (Guna2DataGridView)s;
                string status;
                if (_lang == "en-US")
                {
                    status = grid.Rows[e.RowIndex].Cells["Status"].Value?.ToString();
                }
                else
                {
                    status = grid.Rows[e.RowIndex].Cells["TrangThai"].Value?.ToString();
                }
                bool allowEditDelete =  status != "1";
                // 👆 chỉ dòng cuối (dòng mới nhất) mới có nút
               
                if ((grid.Columns[e.ColumnIndex].Name == "Edit" || grid.Columns[e.ColumnIndex].Name == "Delete"))
                {
                    e.PaintBackground(e.CellBounds, true);

                    if (allowEditDelete)
                    {
                        // Chỉ vẽ nếu được phép
                        Color backColor = grid.Columns[e.ColumnIndex].Name == "Edit"
                            ? Color.SeaGreen
                            : Color.IndianRed;

                        using (Brush b = new SolidBrush(backColor))
                            e.Graphics.FillRectangle(b, e.CellBounds);

                        string text = grid.Columns[e.ColumnIndex].Name;
                        TextRenderer.DrawText(
                            e.Graphics,
                            text,
                            new Font("Segoe UI", 9, FontStyle.Bold),
                            e.CellBounds,
                            Color.White,
                            TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                        );
                    }

                    e.Handled = true;
                }
            };

            btnTrangTruocCP.Enabled = pageNumber > 1;
            btnTrangSauCP.Enabled = pageNumber <= _totalPageCP;


        }
        private void dgvDuLieuCP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string status;
            string expenseID;
            if (_lang == "en-US")
            {
                status = dgvDuLieuCP.Rows[e.RowIndex].Cells["Status"].Value?.ToString();
                expenseID = dgvDuLieuCP.Rows[e.RowIndex].Cells["ExpenseID"].Value.ToString();
            }
            else
            {
                status = dgvDuLieuCP.Rows[e.RowIndex].Cells["TrangThai"].Value?.ToString();
                expenseID = dgvDuLieuCP.Rows[e.RowIndex].Cells["MaChiPhi"].Value.ToString();
            }
 
             
            bool allowAction = status != "1";
            if (!allowAction) return;

            if (dgvDuLieuCP.Columns[e.ColumnIndex].Name == "Edit")
            {
                //MessageBox.Show($"Edit sản phẩm: {productId}", "Edit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var frmChucNangCP = _container.Resolve<frmChucNang_ChiPhiCuaHang>(new ParameterOverride("expenseID", expenseID));
                frmChucNangCP.dataChanged += (s, ev) =>
                {
                  
                    LoadDataCP(cboCuaHang.SelectedValue.ToString());
                };
                frmChucNangCP.ShowDialog();



            }
            else if (dgvDuLieuCP.Columns[e.ColumnIndex].Name == "Delete")
            {
                DialogResult result = MessageBox.Show($"Bạn có chắc muốn xóa phieu chi phi cửa hàng {expenseID}?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    _storeFixedExpenseServices.RemoveStoreFixedExpense(expenseID);
                    MessageBox.Show("Xóa thành công!");
                   
                    LoadDataCP(cboCuaHang.SelectedValue.ToString()); // tải lại dữ liệu
                }
            }
        }

        private void btnTrangSauCP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCP.Text);
            btnTrangTruocCP.Enabled = true;
            if (number <= _totalPageCP)
            {
                var pageNumber = ++number;
                txtSoTrangCP.Text = pageNumber.ToString();
                LoadDataCP(cboCuaHang.SelectedValue.ToString(), pageNumber);
            }
        }

        private void btnTrangTruocCP_Click(object sender, EventArgs e)
        {
            int number = Convert.ToInt32(txtSoTrangCP.Text);
            if (number > 1)
            {

                var pageNumber = --number;
                txtSoTrangCP.Text = pageNumber.ToString();
                LoadDataCP(cboCuaHang.SelectedValue.ToString(), pageNumber);

            }
            else
            {
                btnTrangTruocCP.Enabled = false;
            }
        }

        private void btnThemCP_Click(object sender, EventArgs e)
        {
            
            var frmChucNang = _container.Resolve<frmChucNang_ChiPhiCuaHang>();
            frmChucNang.dataChanged += (s, ev) => { LoadDataCP(cboCuaHang.SelectedValue.ToString()); };
            frmChucNang.ShowDialog();
            
        }
        
        #endregion


    }
}
