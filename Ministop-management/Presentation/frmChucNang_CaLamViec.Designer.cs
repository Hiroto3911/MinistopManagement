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
            this.guna2ImageButton4 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.txt_tenca = new Guna.UI2.WinForms.Guna2TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtp_gioketthuc = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.dtp_giobatdau = new Guna.UI2.WinForms.Guna2DateTimePicker();
            this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
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
            this.panel1.Controls.Add(this.guna2ImageButton4);
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
            this.label11.Size = new System.Drawing.Size(275, 32);
            this.label11.TabIndex = 2;
            this.label11.Text = "Thông Tin Ca Làm VIệc";
            // 
            // guna2ImageButton4
            // 
            this.guna2ImageButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.guna2ImageButton4.BackColor = System.Drawing.Color.Transparent;
            this.guna2ImageButton4.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.Image = global::Presentation.Properties.Resources.cross;
            this.guna2ImageButton4.ImageOffset = new System.Drawing.Point(0, 0);
            this.guna2ImageButton4.ImageRotate = 0F;
            this.guna2ImageButton4.ImageSize = new System.Drawing.Size(32, 32);
            this.guna2ImageButton4.Location = new System.Drawing.Point(492, 9);
            this.guna2ImageButton4.Name = "guna2ImageButton4";
            this.guna2ImageButton4.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.Size = new System.Drawing.Size(36, 35);
            this.guna2ImageButton4.TabIndex = 1;
            this.guna2ImageButton4.Click += new System.EventHandler(this.guna2ImageButton4_Click);
            // 
            // txt_tenca
            // 
            this.txt_tenca.BorderRadius = 2;
            this.txt_tenca.BorderThickness = 2;
            this.txt_tenca.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txt_tenca.DefaultText = "";
            this.txt_tenca.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txt_tenca.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txt_tenca.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txt_tenca.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txt_tenca.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txt_tenca.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txt_tenca.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txt_tenca.Location = new System.Drawing.Point(194, 158);
            this.txt_tenca.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.txt_tenca.Name = "txt_tenca";
            this.txt_tenca.PlaceholderText = "";
            this.txt_tenca.SelectedText = "";
            this.txt_tenca.Size = new System.Drawing.Size(328, 55);
            this.txt_tenca.TabIndex = 129;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(18, 294);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 28);
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
            this.label1.Size = new System.Drawing.Size(74, 28);
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
            this.label3.Size = new System.Drawing.Size(123, 28);
            this.label3.TabIndex = 126;
            this.label3.Text = "Giờ bắt đầu:";
            // 
            // dtp_gioketthuc
            // 
            this.dtp_gioketthuc.BorderRadius = 2;
            this.dtp_gioketthuc.BorderThickness = 2;
            this.dtp_gioketthuc.Checked = true;
            this.dtp_gioketthuc.CustomFormat = "HH:mm";
            this.dtp_gioketthuc.FillColor = System.Drawing.Color.White;
            this.dtp_gioketthuc.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtp_gioketthuc.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_gioketthuc.Location = new System.Drawing.Point(194, 297);
            this.dtp_gioketthuc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtp_gioketthuc.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtp_gioketthuc.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtp_gioketthuc.Name = "dtp_gioketthuc";
            this.dtp_gioketthuc.ShowUpDown = true;
            this.dtp_gioketthuc.Size = new System.Drawing.Size(327, 55);
            this.dtp_gioketthuc.TabIndex = 132;
            this.dtp_gioketthuc.Value = new System.DateTime(2025, 10, 1, 8, 48, 24, 373);
            // 
            // dtp_giobatdau
            // 
            this.dtp_giobatdau.BorderRadius = 2;
            this.dtp_giobatdau.BorderThickness = 2;
            this.dtp_giobatdau.Checked = true;
            this.dtp_giobatdau.CustomFormat = "HH:mm";
            this.dtp_giobatdau.FillColor = System.Drawing.Color.White;
            this.dtp_giobatdau.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtp_giobatdau.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_giobatdau.Location = new System.Drawing.Point(194, 228);
            this.dtp_giobatdau.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dtp_giobatdau.MaxDate = new System.DateTime(9998, 12, 31, 0, 0, 0, 0);
            this.dtp_giobatdau.MinDate = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            this.dtp_giobatdau.Name = "dtp_giobatdau";
            this.dtp_giobatdau.ShowUpDown = true;
            this.dtp_giobatdau.Size = new System.Drawing.Size(327, 55);
            this.dtp_giobatdau.TabIndex = 133;
            this.dtp_giobatdau.Value = new System.DateTime(2025, 10, 1, 8, 48, 24, 373);
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
            // 
            // guna2TextBox1
            // 
            this.guna2TextBox1.BorderRadius = 2;
            this.guna2TextBox1.BorderThickness = 2;
            this.guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox1.DefaultText = "";
            this.guna2TextBox1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.Enabled = false;
            this.guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Location = new System.Drawing.Point(194, 89);
            this.guna2TextBox1.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PlaceholderText = "";
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(328, 55);
            this.guna2TextBox1.TabIndex = 136;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 95);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 28);
            this.label2.TabIndex = 135;
            this.label2.Text = "Mã ca:";
            // 
            // frmChucNang_CaLamViec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 466);
            this.Controls.Add(this.guna2TextBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txt_tenca);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtp_gioketthuc);
            this.Controls.Add(this.dtp_giobatdau);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmChucNang_CaLamViec";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmCapNhat_CaLamViec";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton4;
        private Guna.UI2.WinForms.Guna2TextBox txt_tenca;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtp_gioketthuc;
        private Guna.UI2.WinForms.Guna2DateTimePicker dtp_giobatdau;
        private System.DirectoryServices.DirectorySearcher directorySearcher1;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private System.Windows.Forms.Label label2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}