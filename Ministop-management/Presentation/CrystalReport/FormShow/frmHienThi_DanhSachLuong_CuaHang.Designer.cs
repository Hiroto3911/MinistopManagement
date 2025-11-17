namespace Presentation.CrystalReport.FormShow
{
    partial class frmHienThi_DanhSachLuong_CuaHang
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
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpMonth = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.btnThoat = new Guna.UI2.WinForms.Guna2ImageButton();
            this.btnXem = new Guna.UI2.WinForms.Guna2Button();
            this.cboStore = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblTime = new System.Windows.Forms.Label();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 138);
            this.crystalReportViewer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(1200, 662);
            this.crystalReportViewer1.TabIndex = 5;
            this.crystalReportViewer1.ToolPanelWidth = 300;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.guna2Panel1.Controls.Add(this.label2);
            this.guna2Panel1.Controls.Add(this.dtpMonth);
            this.guna2Panel1.Controls.Add(this.label11);
            this.guna2Panel1.Controls.Add(this.btnThoat);
            this.guna2Panel1.Controls.Add(this.btnXem);
            this.guna2Panel1.Controls.Add(this.cboStore);
            this.guna2Panel1.Controls.Add(this.lblTime);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(1200, 138);
            this.guna2Panel1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 32);
            this.label2.TabIndex = 240;
            this.label2.Text = "Tháng:";
            // 
            // dtpMonth
            // 
            this.dtpMonth.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(221)))), ((int)(((byte)(226)))));
            this.dtpMonth.BorderRadius = 5;
            this.dtpMonth.BorderThickness = 2;
            this.dtpMonth.Checked = true;
            this.dtpMonth.CustomFormat = "MM/yyyy";
            this.dtpMonth.FillColor = System.Drawing.Color.White;
            this.dtpMonth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpMonth.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonth.Location = new System.Drawing.Point(181, 94);
            this.dtpMonth.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpMonth.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpMonth.Name = "dtpMonth";
            this.dtpMonth.ShowUpDown = true;
            this.dtpMonth.Size = new System.Drawing.Size(262, 36);
            this.dtpMonth.TabIndex = 239;
            this.dtpMonth.Value = new System.DateTime(2025, 11, 10, 1, 33, 5, 589);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(12, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(325, 32);
            this.label11.TabIndex = 234;
            this.label11.Text = "Danh sách lương nhân viên";
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.BackColor = System.Drawing.Color.Transparent;
            this.btnThoat.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnThoat.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnThoat.Image = global::Presentation.Properties.Resources.cross;
            this.btnThoat.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnThoat.ImageRotate = 0F;
            this.btnThoat.ImageSize = new System.Drawing.Size(32, 32);
            this.btnThoat.Location = new System.Drawing.Point(1161, 3);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnThoat.Size = new System.Drawing.Size(36, 35);
            this.btnThoat.TabIndex = 233;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnXem
            // 
            this.btnXem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXem.BorderColor = System.Drawing.Color.DimGray;
            this.btnXem.BorderRadius = 10;
            this.btnXem.BorderThickness = 2;
            this.btnXem.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnXem.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnXem.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnXem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnXem.FillColor = System.Drawing.Color.ForestGreen;
            this.btnXem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXem.ForeColor = System.Drawing.Color.White;
            this.btnXem.Location = new System.Drawing.Point(1074, 44);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(123, 44);
            this.btnXem.TabIndex = 232;
            this.btnXem.Text = "Xem";
            this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
            // 
            // cboStore
            // 
            this.cboStore.BackColor = System.Drawing.Color.Transparent;
            this.cboStore.BorderRadius = 5;
            this.cboStore.BorderThickness = 2;
            this.cboStore.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStore.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboStore.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cboStore.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cboStore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cboStore.ItemHeight = 30;
            this.cboStore.Location = new System.Drawing.Point(181, 51);
            this.cboStore.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboStore.Name = "cboStore";
            this.cboStore.Size = new System.Drawing.Size(262, 36);
            this.cboStore.TabIndex = 231;
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(12, 51);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(173, 32);
            this.lblTime.TabIndex = 230;
            this.lblTime.Text = "Tên cửa hàng: ";
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // frmHienThi_DanhSachLuong_CuaHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmHienThi_DanhSachLuong_CuaHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmHienThi_DanhSachLuong_CuaHang";
            this.Load += new System.EventHandler(this.frmHienThi_DanhSachLuong_CuaHang_Load);
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton btnThoat;
        private Guna.UI2.WinForms.Guna2Button btnXem;
        private Guna.UI2.WinForms.Guna2ComboBox cboStore;
        private System.Windows.Forms.Label lblTime;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpMonth;
    }
}