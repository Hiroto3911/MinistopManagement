namespace Presentation
{
    partial class frmChucNang_CaLamViec
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
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.btnThoat = new Guna.UI2.WinForms.Guna2ImageButton();
            this.txtTenCa = new Guna.UI2.WinForms.Guna2TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpGioKetThuc = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtpGioBatDau = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.txtMaCa = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(540, 54);
            this.panel1.TabIndex = 124;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(26, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(228, 28);
            this.label11.TabIndex = 2;
            this.label11.Text = "Thông Tin Ca Làm VIệc";
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
            this.btnThoat.Location = new System.Drawing.Point(492, 9);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnThoat.Size = new System.Drawing.Size(36, 35);
            this.btnThoat.TabIndex = 1;
            this.btnThoat.Click += new System.EventHandler(this.guna2ImageButton4_Click);
            // 
            // txtTenCa
            // 
            this.txtTenCa.BorderRadius = 2;
            this.txtTenCa.BorderThickness = 2;
            this.txtTenCa.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenCa.DefaultText = "";
            this.txtTenCa.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTenCa.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTenCa.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenCa.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenCa.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenCa.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenCa.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenCa.Location = new System.Drawing.Point(194, 158);
            this.txtTenCa.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.txtTenCa.Name = "txtTenCa";
            this.txtTenCa.PlaceholderText = "";
            this.txtTenCa.SelectedText = "";
            this.txtTenCa.Size = new System.Drawing.Size(328, 55);
            this.txtTenCa.TabIndex = 129;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(18, 294);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 23);
            this.label7.TabIndex = 128;
            this.label7.Text = "Giờ kết thúc:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(18, 162);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 23);
            this.label1.TabIndex = 127;
            this.label1.Text = "Tên ca:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(18, 228);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 126;
            this.label3.Text = "Giờ bắt đầu:";
            // 
            // dtpGioKetThuc
            // 
            this.dtpGioKetThuc.BorderRadius = 2;
            this.dtpGioKetThuc.BorderThickness = 2;
            this.dtpGioKetThuc.Checked = true;
            this.dtpGioKetThuc.CustomFormat = "HH:mm";
            this.dtpGioKetThuc.FillColor = System.Drawing.Color.White;
            this.dtpGioKetThuc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpGioKetThuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGioKetThuc.Location = new System.Drawing.Point(194, 297);
            this.dtpGioKetThuc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpGioKetThuc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpGioKetThuc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpGioKetThuc.Name = "dtpGioKetThuc";
            this.dtpGioKetThuc.ShowUpDown = true;
            this.dtpGioKetThuc.Size = new System.Drawing.Size(327, 55);
            this.dtpGioKetThuc.TabIndex = 132;
            this.dtpGioKetThuc.Value = new System.DateTime(2025, 10, 1, 0, 0, 0, 0);
            // 
            // dtpGioBatDau
            // 
            this.dtpGioBatDau.BorderRadius = 2;
            this.dtpGioBatDau.BorderThickness = 2;
            this.dtpGioBatDau.Checked = true;
            this.dtpGioBatDau.CustomFormat = "HH:mm";
            this.dtpGioBatDau.FillColor = System.Drawing.Color.White;
            this.dtpGioBatDau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpGioBatDau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGioBatDau.Location = new System.Drawing.Point(194, 228);
            this.dtpGioBatDau.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtpGioBatDau.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpGioBatDau.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpGioBatDau.Name = "dtpGioBatDau";
            this.dtpGioBatDau.ShowUpDown = true;
            this.dtpGioBatDau.Size = new System.Drawing.Size(327, 55);
            this.dtpGioBatDau.TabIndex = 133;
            this.dtpGioBatDau.Value = new System.DateTime(2025, 10, 1, 0, 0, 0, 0);
            // 
            // directorySearcher1
            // 
            this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
            this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
            // 
            // guna2Button1
            // 
            this.guna2Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2Button1.BorderColor = System.Drawing.Color.DimGray;
            this.guna2Button1.BorderRadius = 10;
            this.guna2Button1.BorderThickness = 3;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(194, 391);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(140, 45);
            this.guna2Button1.TabIndex = 134;
            this.guna2Button1.Text = "Lưu";
            this.guna2Button1.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // txtMaCa
            // 
            this.txtMaCa.BorderRadius = 2;
            this.txtMaCa.BorderThickness = 2;
            this.txtMaCa.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtMaCa.DefaultText = "";
            this.txtMaCa.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtMaCa.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtMaCa.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMaCa.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtMaCa.Enabled = false;
            this.txtMaCa.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtMaCa.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtMaCa.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtMaCa.Location = new System.Drawing.Point(194, 89);
            this.txtMaCa.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.txtMaCa.Name = "txtMaCa";
            this.txtMaCa.PlaceholderText = "";
            this.txtMaCa.SelectedText = "";
            this.txtMaCa.Size = new System.Drawing.Size(328, 55);
            this.txtMaCa.TabIndex = 136;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 23);
            this.label2.TabIndex = 135;
            this.label2.Text = "Mã ca:";
            // 
            // frmChucNang_CaLamViec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 466);
            this.Controls.Add(this.txtMaCa);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtTenCa);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpGioKetThuc);
            this.Controls.Add(this.dtpGioBatDau);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmChucNang_CaLamViec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCapNhat_CaLamViec";
            this.Load += new System.EventHandler(this.frmChucNang_CaLam_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton btnThoat;
        private Guna.UI2.WinForms.Guna2TextBox txtTenCa;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpGioKetThuc;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpGioBatDau;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private Guna.UI2.WinForms.Guna2TextBox txtMaCa;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}