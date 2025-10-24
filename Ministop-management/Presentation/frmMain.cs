using Services.Interfaces;
using Services.Services;
using Shared.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;


namespace Presentation
{
    public partial class frmMain : Form
    {
        //Keo form tu panel
        bool _MouseDown;
        private Point offSet;
        //Timer
        private Timer timer;
        //Resizing
        private bool isResizing = false;
        private Point lastMousePos;
        private readonly IUnityContainer _container;
        private readonly IIdentityService _identityService;
        private readonly IUserSession _userSession;

        public frmMain(IUnityContainer container, IIdentityService identityService, IUserSession userSession)
        {
            InitializeComponent();
            InitializeClock();
            _container = container;
            _identityService = identityService;
            _userSession = userSession;
            btnResize.Cursor = Cursors.SizeNWSE;
            btnResize.MouseDown += BtnResize_MouseDown;
            btnResize.MouseMove += BtnResize_MouseMove;
            btnResize.MouseUp += BtnResize_MouseUp;
        }
        #region Giao dien
        private void InitializeClock()
        {
            timer = new Timer();
            timer.Interval = 1000; // cập nhật mỗi giây
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Định dạng ngày giờ
            string thu = DateTime.Now.ToString("dddd"); // Thứ
            string ngay = DateTime.Now.ToString("dd");
            string thang = DateTime.Now.ToString("MM");
            string nam = DateTime.Now.ToString("yyyy");
            string gio = DateTime.Now.ToString("HH:mm");

            // Hiển thị ra label
            lblTime.Text = $"{thu}, ngày {ngay}/{thang}/{nam} - {gio}";
        }

        private void BtnResize_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isResizing = true;
                lastMousePos = Cursor.Position;
            }
        }

        private void BtnResize_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                int dx = Cursor.Position.X - lastMousePos.X;
                int dy = Cursor.Position.Y - lastMousePos.Y;

                this.Width += dx;
                this.Height += dy;

                lastMousePos = Cursor.Position;
            }
        }

        private void BtnResize_MouseUp(object sender, MouseEventArgs e)
        {
            isResizing = false;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCHITTEST = 0x84;
            const int HTCLIENT = 1;
            const int HTLEFT = 10;
            const int HTRIGHT = 11;
            const int HTTOP = 12;
            const int HTTOPLEFT = 13;
            const int HTTOPRIGHT = 14;
            const int HTBOTTOM = 15;
            const int HTBOTTOMLEFT = 16;
            const int HTBOTTOMRIGHT = 17;

            if (m.Msg == WM_NCHITTEST)
            {
                base.WndProc(ref m);

                if ((int)m.Result == HTCLIENT)
                {
                    Point cursor = PointToClient(Cursor.Position);
                    int grip = 10; // Resize area thickness

                    bool left = cursor.X <= grip;
                    bool right = cursor.X >= Width - grip;
                    bool top = cursor.Y <= grip;
                    bool bottom = cursor.Y >= Height - grip;

                    if (left && top)
                        m.Result = (IntPtr)HTTOPLEFT;
                    else if (right && top)
                        m.Result = (IntPtr)HTTOPRIGHT;
                    else if (left && bottom)
                        m.Result = (IntPtr)HTBOTTOMLEFT;
                    else if (right && bottom)
                        m.Result = (IntPtr)HTBOTTOMRIGHT;
                    else if (left)
                        m.Result = (IntPtr)HTLEFT;
                    else if (right)
                        m.Result = (IntPtr)HTRIGHT;
                    else if (top)
                        m.Result = (IntPtr)HTTOP;
                    else if (bottom)
                        m.Result = (IntPtr)HTBOTTOM;
                }
                return;
            }

            base.WndProc(ref m);
        }


        //Biến panel thành form border
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            offSet.X = e.X; offSet.Y = e.Y;
            _MouseDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offSet.X, currentScreenPos.Y - offSet.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            _MouseDown = false;
        }
        #endregion


        //Đóng cửa sổ
        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            lblNguoiDung.Text = $"Xin chào bạn {_userSession.Name}";
            if (_userSession.Role == "Admin")
            {
                mnBanHang.Visible = false;
            }
            else
            {

                if (_userSession.Role == "Nhân viên")
                {
                    mnSanPham.Visible = false;
                    mnCuaHang.Visible = false;
                    mnThongKe.Visible = false;
                }
                ;
            }

        }




        private void guna2ImageButton3_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
                foreach (Control ctrl in panelContainer.Controls)
                {
                    if (ctrl is Form child)
                    {
                        OpenChildForm(child);
                    }
                }
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                foreach (Control ctrl in panelContainer.Controls)
                {
                    if (ctrl is Form child)
                    {
                        OpenChildForm(child);
                    }
                }
            }

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void OpenChildForm(Form childForm)
        {
            // remove any existing controls
            panelContainer.Controls.Clear();

            // configure child
            childForm.TopLevel = false;                     // IMPORTANT
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            childForm.ShowInTaskbar = false;

            // add and show
            panelContainer.Controls.Add(childForm);
            childForm.Show();
            childForm.BringToFront();
        }


        private void frmMain_Resize(object sender, EventArgs e)
        {
            foreach (Control ctrl in panelContainer.Controls)
            {
                if (ctrl is Form child)
                {
                    child.Dock = DockStyle.Fill;
                }
            }
        }


        #region MenuItemClick

        private void mnDangXuat_Click(object sender, EventArgs e)
        {
            _identityService.LogOut();
            this.Close();
        }
        private void mnCuaHang_Click(object sender, EventArgs e)
        {
            var frmCuaHang = _container.Resolve<frmHienThi_CuaHang>();
            OpenChildForm(frmCuaHang);
        }

        private void mnNhanVien_Click(object sender, EventArgs e)
        {
            var frmCuaHang = _container.Resolve<frmHienThi_NhanVien>();
            OpenChildForm(frmCuaHang);
        }

        private void mnKhoHang_Click(object sender, EventArgs e)
        {
            var frmCuaHang = _container.Resolve<frmHienThi_KhoHang>();
            OpenChildForm(frmCuaHang);
        }

        private void mnBanHang_Click(object sender, EventArgs e)
        {
            var frmCuaHang = _container.Resolve<frmHienThi_BanHang>();
            OpenChildForm(frmCuaHang);
        }

        private void mnSanPham_Click(object sender, EventArgs e)
        {
            var frmCuaHang = _container.Resolve<frmHienThi_SanPham>();
            OpenChildForm(frmCuaHang);
        }

        private void mnThongKe_Click(object sender, EventArgs e)
        {
            var frmCuaHang = _container.Resolve<frmHienThi_ThongKe>();
            OpenChildForm(frmCuaHang);
        }
        #endregion
    }
}
