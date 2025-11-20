namespace Presentation.CrystalReport.FormShow
{
    partial class frmHienThi_DanhSachTop3Store
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
            this.label11 = new System.Windows.Forms.Label();
            this.btnThoat = new Guna.UI2.WinForms.Guna2ImageButton();
            this.btnXem = new Guna.UI2.WinForms.Guna2Button();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.dtpMonthstar = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtpMothend = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 129);
            this.crystalReportViewer1.Margin = new System.Windows.Forms.Padding(4);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(800, 321);
            this.crystalReportViewer1.TabIndex = 3;
            this.crystalReportViewer1.ToolPanelWidth = 267;
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.guna2Panel1.Controls.Add(this.label1);
            this.guna2Panel1.Controls.Add(this.label2);
            this.guna2Panel1.Controls.Add(this.dtpMothend);
            this.guna2Panel1.Controls.Add(this.dtpMonthstar);
            this.guna2Panel1.Controls.Add(this.label11);
            this.guna2Panel1.Controls.Add(this.btnThoat);
            this.guna2Panel1.Controls.Add(this.btnXem);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Margin = new System.Windows.Forms.Padding(4);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(800, 129);
            this.guna2Panel1.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(3, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(294, 28);
            this.label11.TabIndex = 234;
            this.label11.Text = "Xem top 3 cửa hàng bán chạy";
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
            this.btnThoat.Location = new System.Drawing.Point(765, 2);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnThoat.Size = new System.Drawing.Size(32, 28);
            this.btnThoat.TabIndex = 233;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click_1);
            // 
            // btnXem
            // 
            this.btnXem.Anchor = System.Windows.Forms.AnchorStyles.Right;
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
            this.btnXem.Location = new System.Drawing.Point(680, 61);
            this.btnXem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(109, 36);
            this.btnXem.TabIndex = 232;
            this.btnXem.Text = "Xem";
            this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 28);
            this.label2.TabIndex = 242;
            this.label2.Text = "Tháng bắt đầu:";
            // 
            // dtpMonthstar
            // 
            this.dtpMonthstar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(221)))), ((int)(((byte)(226)))));
            this.dtpMonthstar.BorderRadius = 5;
            this.dtpMonthstar.BorderThickness = 2;
            this.dtpMonthstar.Checked = true;
            this.dtpMonthstar.CustomFormat = "MM/yyyy";
            this.dtpMonthstar.FillColor = System.Drawing.Color.White;
            this.dtpMonthstar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpMonthstar.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMonthstar.Location = new System.Drawing.Point(155, 47);
            this.dtpMonthstar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpMonthstar.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpMonthstar.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpMonthstar.Name = "dtpMonthstar";
            this.dtpMonthstar.ShowUpDown = true;
            this.dtpMonthstar.Size = new System.Drawing.Size(233, 29);
            this.dtpMonthstar.TabIndex = 241;
            this.dtpMonthstar.Value = new System.DateTime(2025, 11, 10, 1, 33, 5, 589);
            // 
            // dtpMothend
            // 
            this.dtpMothend.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(217)))), ((int)(((byte)(221)))), ((int)(((byte)(226)))));
            this.dtpMothend.BorderRadius = 5;
            this.dtpMothend.BorderThickness = 2;
            this.dtpMothend.Checked = true;
            this.dtpMothend.CustomFormat = "MM/yyyy";
            this.dtpMothend.FillColor = System.Drawing.Color.White;
            this.dtpMothend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpMothend.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpMothend.Location = new System.Drawing.Point(155, 80);
            this.dtpMothend.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpMothend.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpMothend.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpMothend.Name = "dtpMothend";
            this.dtpMothend.ShowUpDown = true;
            this.dtpMothend.Size = new System.Drawing.Size(233, 29);
            this.dtpMothend.TabIndex = 241;
            this.dtpMothend.Value = new System.DateTime(2025, 11, 10, 1, 33, 5, 589);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 28);
            this.label1.TabIndex = 242;
            this.label1.Text = "Tháng kết thúc:";
            // 
            // frmHienThi_DanhSachTop3Store
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.guna2Panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmHienThi_DanhSachTop3Store";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmHienThi_DanhSachTop3Store";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
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
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpMothend;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpMonthstar;
    }
}