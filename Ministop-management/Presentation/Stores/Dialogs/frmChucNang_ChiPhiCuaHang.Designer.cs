namespace Presentation
{
    partial class frmChucNang_ChiPhiCuaHang
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.ibtnThoat = new Guna.UI2.WinForms.Guna2ImageButton();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.rtxtGhiChu = new System.Windows.Forms.RichTextBox();
            this.txtTenCuaHang = new Guna.UI2.WinForms.Guna2TextBox();
            this.dtpNgayLap = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.txtTienDien = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTienMatBang = new Guna.UI2.WinForms.Guna2TextBox();
            this.txtTienNuoc = new Guna.UI2.WinForms.Guna2TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.ibtnThoat);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(376, 35);
            this.panel1.TabIndex = 123;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(17, 6);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(224, 21);
            this.label11.TabIndex = 2;
            this.label11.Text = "Thông Tin Chi Phí Cửa Hàng";
            // 
            // ibtnThoat
            // 
            this.ibtnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ibtnThoat.BackColor = System.Drawing.Color.Transparent;
            this.ibtnThoat.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.ibtnThoat.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.ibtnThoat.Image = global::Presentation.Properties.Resources.cross;
            this.ibtnThoat.ImageOffset = new System.Drawing.Point(0, 0);
            this.ibtnThoat.ImageRotate = 0F;
            this.ibtnThoat.ImageSize = new System.Drawing.Size(32, 32);
            this.ibtnThoat.Location = new System.Drawing.Point(344, 6);
            this.ibtnThoat.Margin = new System.Windows.Forms.Padding(2);
            this.ibtnThoat.Name = "ibtnThoat";
            this.ibtnThoat.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.ibtnThoat.Size = new System.Drawing.Size(24, 23);
            this.ibtnThoat.TabIndex = 1;
            this.ibtnThoat.Click += new System.EventHandler(this.ibtnThoat_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 133);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 19);
            this.label7.TabIndex = 130;
            this.label7.Text = "Tiền mặt bằng:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 263);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 19);
            this.label6.TabIndex = 127;
            this.label6.Text = "Ghi chú:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 221);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 19);
            this.label4.TabIndex = 128;
            this.label4.Text = "Tiền nước:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 19);
            this.label5.TabIndex = 129;
            this.label5.Text = "Tiền điện:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 19);
            this.label1.TabIndex = 126;
            this.label1.Text = "Mã cửa hàng:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 19);
            this.label3.TabIndex = 125;
            this.label3.Text = "Ngày:";
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // btnLuu
            // 
            this.btnLuu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLuu.BorderColor = System.Drawing.Color.DimGray;
            this.btnLuu.BorderRadius = 10;
            this.btnLuu.BorderThickness = 3;
            this.btnLuu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLuu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLuu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.btnLuu.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Location = new System.Drawing.Point(130, 332);
            this.btnLuu.Margin = new System.Windows.Forms.Padding(2);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(93, 29);
            this.btnLuu.TabIndex = 139;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // rtxtGhiChu
            // 
            this.rtxtGhiChu.Location = new System.Drawing.Point(130, 265);
            this.rtxtGhiChu.Name = "rtxtGhiChu";
            this.rtxtGhiChu.Size = new System.Drawing.Size(219, 52);
            this.rtxtGhiChu.TabIndex = 192;
            this.rtxtGhiChu.Text = "";
            // 
            // txtTenCuaHang
            // 
            this.txtTenCuaHang.BorderRadius = 2;
            this.txtTenCuaHang.BorderThickness = 2;
            this.txtTenCuaHang.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTenCuaHang.DefaultText = "";
            this.txtTenCuaHang.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTenCuaHang.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTenCuaHang.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenCuaHang.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTenCuaHang.Enabled = false;
            this.txtTenCuaHang.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenCuaHang.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTenCuaHang.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTenCuaHang.Location = new System.Drawing.Point(130, 49);
            this.txtTenCuaHang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTenCuaHang.Name = "txtTenCuaHang";
            this.txtTenCuaHang.PlaceholderText = "";
            this.txtTenCuaHang.SelectedText = "";
            this.txtTenCuaHang.Size = new System.Drawing.Size(219, 36);
            this.txtTenCuaHang.TabIndex = 191;
            // 
            // dtpNgayLap
            // 
            this.dtpNgayLap.BorderRadius = 2;
            this.dtpNgayLap.BorderThickness = 2;
            this.dtpNgayLap.Checked = true;
            this.dtpNgayLap.CustomFormat = "MM/yyyy";
            this.dtpNgayLap.Enabled = false;
            this.dtpNgayLap.FillColor = System.Drawing.Color.White;
            this.dtpNgayLap.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtpNgayLap.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpNgayLap.Location = new System.Drawing.Point(130, 92);
            this.dtpNgayLap.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtpNgayLap.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtpNgayLap.Name = "dtpNgayLap";
            this.dtpNgayLap.ShowUpDown = true;
            this.dtpNgayLap.Size = new System.Drawing.Size(218, 36);
            this.dtpNgayLap.TabIndex = 190;
            this.dtpNgayLap.Value = new System.DateTime(2025, 10, 1, 8, 48, 24, 373);
            // 
            // txtTienDien
            // 
            this.txtTienDien.BorderRadius = 2;
            this.txtTienDien.BorderThickness = 2;
            this.txtTienDien.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienDien.DefaultText = "";
            this.txtTienDien.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTienDien.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTienDien.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienDien.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienDien.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienDien.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTienDien.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienDien.Location = new System.Drawing.Point(129, 178);
            this.txtTienDien.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTienDien.Name = "txtTienDien";
            this.txtTienDien.PlaceholderText = "";
            this.txtTienDien.SelectedText = "";
            this.txtTienDien.Size = new System.Drawing.Size(219, 36);
            this.txtTienDien.TabIndex = 187;
            // 
            // txtTienMatBang
            // 
            this.txtTienMatBang.BorderRadius = 2;
            this.txtTienMatBang.BorderThickness = 2;
            this.txtTienMatBang.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienMatBang.DefaultText = "";
            this.txtTienMatBang.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTienMatBang.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTienMatBang.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienMatBang.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienMatBang.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienMatBang.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTienMatBang.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienMatBang.Location = new System.Drawing.Point(129, 135);
            this.txtTienMatBang.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTienMatBang.Name = "txtTienMatBang";
            this.txtTienMatBang.PlaceholderText = "";
            this.txtTienMatBang.SelectedText = "";
            this.txtTienMatBang.Size = new System.Drawing.Size(219, 36);
            this.txtTienMatBang.TabIndex = 188;
            // 
            // txtTienNuoc
            // 
            this.txtTienNuoc.BorderRadius = 2;
            this.txtTienNuoc.BorderThickness = 2;
            this.txtTienNuoc.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTienNuoc.DefaultText = "";
            this.txtTienNuoc.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtTienNuoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtTienNuoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienNuoc.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtTienNuoc.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienNuoc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtTienNuoc.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtTienNuoc.Location = new System.Drawing.Point(129, 221);
            this.txtTienNuoc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTienNuoc.Name = "txtTienNuoc";
            this.txtTienNuoc.PlaceholderText = "";
            this.txtTienNuoc.SelectedText = "";
            this.txtTienNuoc.Size = new System.Drawing.Size(219, 36);
            this.txtTienNuoc.TabIndex = 189;
            // 
            // frmChucNang_ChiPhiCuaHang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 373);
            this.Controls.Add(this.rtxtGhiChu);
            this.Controls.Add(this.txtTenCuaHang);
            this.Controls.Add(this.dtpNgayLap);
            this.Controls.Add(this.txtTienDien);
            this.Controls.Add(this.txtTienMatBang);
            this.Controls.Add(this.txtTienNuoc);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmChucNang_ChiPhiCuaHang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCapNhat_ChiPhiCuaHang";
            this.Load += new System.EventHandler(this.frmChucNang_ChiPhiCuaHang_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton ibtnThoat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private System.Windows.Forms.RichTextBox rtxtGhiChu;
        private Guna.UI2.WinForms.Guna2TextBox txtTenCuaHang;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtpNgayLap;
        private Guna.UI2.WinForms.Guna2TextBox txtTienDien;
        private Guna.UI2.WinForms.Guna2TextBox txtTienMatBang;
        private Guna.UI2.WinForms.Guna2TextBox txtTienNuoc;
    }
}