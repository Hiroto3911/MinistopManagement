namespace Presentation
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.lblNguoiDung = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2ImageButton1 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2ImageButton3 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.guna2ImageButton4 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.thôngTinNgườiDùngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnCuaHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnNhanVien = new System.Windows.Forms.ToolStripMenuItem();
            this.mnKhoHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnBanHang = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSanPham = new System.Windows.Forms.ToolStripMenuItem();
            this.mnThongKe = new System.Windows.Forms.ToolStripMenuItem();
            this.báoCáoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.danhSáchCửaHàngTheoKhuVựcToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.danhSáchNhânViênTheoCửaHàngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phieuNhapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.phieuXuatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xemHợpĐồngLươngNhânViênToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xuấtPhiếuLươngNhânViênToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnResize = new Guna.UI2.WinForms.Guna2ImageButton();
            this.lblTime = new System.Windows.Forms.Label();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNguoiDung
            // 
            resources.ApplyResources(this.lblNguoiDung, "lblNguoiDung");
            this.lblNguoiDung.ForeColor = System.Drawing.Color.White;
            this.lblNguoiDung.Name = "lblNguoiDung";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.panel1.Controls.Add(this.guna2ImageButton1);
            this.panel1.Controls.Add(this.guna2ImageButton3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.lblNguoiDung);
            this.panel1.Controls.Add(this.guna2ImageButton4);
            this.panel1.Name = "panel1";
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseUp);
            // 
            // guna2ImageButton1
            // 
            resources.ApplyResources(this.guna2ImageButton1, "guna2ImageButton1");
            this.guna2ImageButton1.BackColor = System.Drawing.Color.Transparent;
            this.guna2ImageButton1.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton1.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton1.Image = global::Presentation.Properties.Resources.minimize;
            this.guna2ImageButton1.ImageOffset = new System.Drawing.Point(0, 0);
            this.guna2ImageButton1.ImageRotate = 0F;
            this.guna2ImageButton1.ImageSize = new System.Drawing.Size(32, 32);
            this.guna2ImageButton1.Name = "guna2ImageButton1";
            this.guna2ImageButton1.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton1.Click += new System.EventHandler(this.guna2ImageButton1_Click);
            // 
            // guna2ImageButton3
            // 
            resources.ApplyResources(this.guna2ImageButton3, "guna2ImageButton3");
            this.guna2ImageButton3.BackColor = System.Drawing.Color.Transparent;
            this.guna2ImageButton3.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton3.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton3.Image = global::Presentation.Properties.Resources.maximize;
            this.guna2ImageButton3.ImageOffset = new System.Drawing.Point(0, 0);
            this.guna2ImageButton3.ImageRotate = 0F;
            this.guna2ImageButton3.ImageSize = new System.Drawing.Size(32, 32);
            this.guna2ImageButton3.Name = "guna2ImageButton3";
            this.guna2ImageButton3.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton3.Click += new System.EventHandler(this.guna2ImageButton3_Click);
            // 
            // pictureBox1
            // 
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Image = global::Presentation.Properties.Resources.ministop_logo;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // guna2ImageButton4
            // 
            resources.ApplyResources(this.guna2ImageButton4, "guna2ImageButton4");
            this.guna2ImageButton4.BackColor = System.Drawing.Color.Transparent;
            this.guna2ImageButton4.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.Image = global::Presentation.Properties.Resources.cross;
            this.guna2ImageButton4.ImageOffset = new System.Drawing.Point(0, 0);
            this.guna2ImageButton4.ImageRotate = 0F;
            this.guna2ImageButton4.ImageSize = new System.Drawing.Size(32, 32);
            this.guna2ImageButton4.Name = "guna2ImageButton4";
            this.guna2ImageButton4.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.Click += new System.EventHandler(this.guna2ImageButton4_Click);
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // menuStrip1
            // 
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnHeThong,
            this.mnCuaHang,
            this.mnNhanVien,
            this.mnKhoHang,
            this.mnBanHang,
            this.mnSanPham,
            this.mnThongKe,
            this.báoCáoToolStripMenuItem});
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Stretch = false;
            // 
            // mnHeThong
            // 
            resources.ApplyResources(this.mnHeThong, "mnHeThong");
            this.mnHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.thôngTinNgườiDùngToolStripMenuItem,
            this.mnDangXuat});
            this.mnHeThong.Name = "mnHeThong";
            // 
            // thôngTinNgườiDùngToolStripMenuItem
            // 
            resources.ApplyResources(this.thôngTinNgườiDùngToolStripMenuItem, "thôngTinNgườiDùngToolStripMenuItem");
            this.thôngTinNgườiDùngToolStripMenuItem.Name = "thôngTinNgườiDùngToolStripMenuItem";
            // 
            // mnDangXuat
            // 
            resources.ApplyResources(this.mnDangXuat, "mnDangXuat");
            this.mnDangXuat.Name = "mnDangXuat";
            this.mnDangXuat.Click += new System.EventHandler(this.mnDangXuat_Click);
            // 
            // mnCuaHang
            // 
            resources.ApplyResources(this.mnCuaHang, "mnCuaHang");
            this.mnCuaHang.Image = global::Presentation.Properties.Resources.market;
            this.mnCuaHang.Name = "mnCuaHang";
            this.mnCuaHang.Click += new System.EventHandler(this.mnCuaHang_Click);
            // 
            // mnNhanVien
            // 
            resources.ApplyResources(this.mnNhanVien, "mnNhanVien");
            this.mnNhanVien.Image = global::Presentation.Properties.Resources.Employeemanagement1;
            this.mnNhanVien.Name = "mnNhanVien";
            this.mnNhanVien.Click += new System.EventHandler(this.mnNhanVien_Click);
            // 
            // mnKhoHang
            // 
            resources.ApplyResources(this.mnKhoHang, "mnKhoHang");
            this.mnKhoHang.Image = global::Presentation.Properties.Resources.in_stock;
            this.mnKhoHang.Name = "mnKhoHang";
            this.mnKhoHang.Click += new System.EventHandler(this.mnKhoHang_Click);
            // 
            // mnBanHang
            // 
            resources.ApplyResources(this.mnBanHang, "mnBanHang");
            this.mnBanHang.Image = global::Presentation.Properties.Resources.invoice;
            this.mnBanHang.Name = "mnBanHang";
            this.mnBanHang.Click += new System.EventHandler(this.mnBanHang_Click);
            // 
            // mnSanPham
            // 
            resources.ApplyResources(this.mnSanPham, "mnSanPham");
            this.mnSanPham.Image = global::Presentation.Properties.Resources.Product1;
            this.mnSanPham.Name = "mnSanPham";
            this.mnSanPham.Click += new System.EventHandler(this.mnSanPham_Click);
            // 
            // mnThongKe
            // 
            resources.ApplyResources(this.mnThongKe, "mnThongKe");
            this.mnThongKe.Image = global::Presentation.Properties.Resources.monitor;
            this.mnThongKe.Name = "mnThongKe";
            this.mnThongKe.Click += new System.EventHandler(this.mnThongKe_Click);
            // 
            // báoCáoToolStripMenuItem
            // 
            resources.ApplyResources(this.báoCáoToolStripMenuItem, "báoCáoToolStripMenuItem");
            this.báoCáoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.danhSáchCửaHàngTheoKhuVựcToolStripMenuItem,
            this.danhSáchNhânViênTheoCửaHàngToolStripMenuItem,
            this.phieuNhapToolStripMenuItem,
            this.phieuXuatToolStripMenuItem,
            this.xemHợpĐồngLươngNhânViênToolStripMenuItem,
            this.xuấtPhiếuLươngNhânViênToolStripMenuItem,
            this.xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem,
            this.xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem});
            this.báoCáoToolStripMenuItem.Image = global::Presentation.Properties.Resources.report;
            this.báoCáoToolStripMenuItem.Name = "báoCáoToolStripMenuItem";
            // 
            // danhSáchCửaHàngTheoKhuVựcToolStripMenuItem
            // 
            resources.ApplyResources(this.danhSáchCửaHàngTheoKhuVựcToolStripMenuItem, "danhSáchCửaHàngTheoKhuVựcToolStripMenuItem");
            this.danhSáchCửaHàngTheoKhuVựcToolStripMenuItem.Name = "danhSáchCửaHàngTheoKhuVựcToolStripMenuItem";
            this.danhSáchCửaHàngTheoKhuVựcToolStripMenuItem.Click += new System.EventHandler(this.danhSáchCửaHàngTheoKhuVựcToolStripMenuItem_Click);
            // 
            // danhSáchNhânViênTheoCửaHàngToolStripMenuItem
            // 
            resources.ApplyResources(this.danhSáchNhânViênTheoCửaHàngToolStripMenuItem, "danhSáchNhânViênTheoCửaHàngToolStripMenuItem");
            this.danhSáchNhânViênTheoCửaHàngToolStripMenuItem.Name = "danhSáchNhânViênTheoCửaHàngToolStripMenuItem";
            this.danhSáchNhânViênTheoCửaHàngToolStripMenuItem.Click += new System.EventHandler(this.danhSáchNhânViênTheoCửaHàngToolStripMenuItem_Click);
            // 
            // phieuNhapToolStripMenuItem
            // 
            resources.ApplyResources(this.phieuNhapToolStripMenuItem, "phieuNhapToolStripMenuItem");
            this.phieuNhapToolStripMenuItem.Name = "phieuNhapToolStripMenuItem";
            // 
            // phieuXuatToolStripMenuItem
            // 
            resources.ApplyResources(this.phieuXuatToolStripMenuItem, "phieuXuatToolStripMenuItem");
            this.phieuXuatToolStripMenuItem.Name = "phieuXuatToolStripMenuItem";
            // 
            // xemHợpĐồngLươngNhânViênToolStripMenuItem
            // 
            resources.ApplyResources(this.xemHợpĐồngLươngNhânViênToolStripMenuItem, "xemHợpĐồngLươngNhânViênToolStripMenuItem");
            this.xemHợpĐồngLươngNhânViênToolStripMenuItem.Name = "xemHợpĐồngLươngNhânViênToolStripMenuItem";
            this.xemHợpĐồngLươngNhânViênToolStripMenuItem.Click += new System.EventHandler(this.xemHợpĐồngLươngNhânViênToolStripMenuItem_Click);
            // 
            // xuấtPhiếuLươngNhânViênToolStripMenuItem
            // 
            resources.ApplyResources(this.xuấtPhiếuLươngNhânViênToolStripMenuItem, "xuấtPhiếuLươngNhânViênToolStripMenuItem");
            this.xuấtPhiếuLươngNhânViênToolStripMenuItem.Name = "xuấtPhiếuLươngNhânViênToolStripMenuItem";
            this.xuấtPhiếuLươngNhânViênToolStripMenuItem.Click += new System.EventHandler(this.xuấtPhiếuLươngNhânViênToolStripMenuItem_Click);
            // 
            // xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem
            // 
            resources.ApplyResources(this.xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem, "xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem");
            this.xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem.Name = "xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem";
            this.xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem.Click += new System.EventHandler(this.xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem_Click);
            // 
            // xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem
            // 
            resources.ApplyResources(this.xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem, "xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem");
            this.xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem.Name = "xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem";
            this.xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem.Click += new System.EventHandler(this.xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.BackColor = System.Drawing.SystemColors.Menu;
            this.panel2.Controls.Add(this.btnResize);
            this.panel2.Controls.Add(this.lblTime);
            this.panel2.Name = "panel2";
            // 
            // btnResize
            // 
            resources.ApplyResources(this.btnResize, "btnResize");
            this.btnResize.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnResize.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnResize.Image = global::Presentation.Properties.Resources.resize;
            this.btnResize.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnResize.ImageRotate = 0F;
            this.btnResize.ImageSize = new System.Drawing.Size(24, 24);
            this.btnResize.Name = "btnResize";
            this.btnResize.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnResize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnResize_MouseDown);
            this.btnResize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BtnResize_MouseMove);
            this.btnResize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnResize_MouseUp);
            // 
            // lblTime
            // 
            resources.ApplyResources(this.lblTime, "lblTime");
            this.lblTime.Name = "lblTime";
            // 
            // panelContainer
            // 
            resources.ApplyResources(this.panelContainer, "panelContainer");
            this.panelContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(63)))), ((int)(((byte)(128)))));
            this.panelContainer.Name = "panelContainer";
            // 
            // frmMain
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmMain";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Main_Load);
            this.Resize += new System.EventHandler(this.frmMain_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton4;
        private System.Windows.Forms.Label lblNguoiDung;
        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Panel panelContainer;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton3;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnHeThong;
        private System.Windows.Forms.ToolStripMenuItem thôngTinNgườiDùngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnDangXuat;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTime;
        private Guna.UI2.WinForms.Guna2ImageButton btnResize;
        private System.Windows.Forms.ToolStripMenuItem mnCuaHang;
        private System.Windows.Forms.ToolStripMenuItem mnNhanVien;
        private System.Windows.Forms.ToolStripMenuItem mnKhoHang;
        private System.Windows.Forms.ToolStripMenuItem mnBanHang;
        private System.Windows.Forms.ToolStripMenuItem mnSanPham;
        private System.Windows.Forms.ToolStripMenuItem mnThongKe;
        private System.Windows.Forms.ToolStripMenuItem báoCáoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danhSáchCửaHàngTheoKhuVựcToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem danhSáchNhânViênTheoCửaHàngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phieuNhapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem phieuXuatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xemHợpĐồngLươngNhânViênToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xuấtPhiếuLươngNhânViênToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xuấtDanhSáchLươngCủaNhânViênTrongCửaHàngToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xuấtDanhSáchTop3CửaHàngBánChạyToolStripMenuItem;
    }
}