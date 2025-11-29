namespace Presentation
{
    partial class frmChucNang_PhanCaLam
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmChucNang_PhanCaLam));
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.btnClose = new Guna.UI2.WinForms.Guna2ImageButton();
            this.btnLuu = new Guna.UI2.WinForms.Guna2Button();
            this.txtGhiChu = new Guna.UI2.WinForms.Guna2TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbCaLam = new Guna.UI2.WinForms.Guna2ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbNhanVien = new Guna.UI2.WinForms.Guna2ComboBox();
            this.cbCuaHang = new Guna.UI2.WinForms.Guna2ComboBox();
            this.chkThu2 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkThu3 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkThu4 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkThu5 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkThu6 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkThu7 = new Guna.UI2.WinForms.Guna2CheckBox();
            this.chkChuNhat = new Guna.UI2.WinForms.Guna2CheckBox();
            this.label1 = new System.Windows.Forms.Label();
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
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Name = "panel1";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Name = "label11";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.CheckedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnClose.HoverState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnClose.Image = global::Presentation.Properties.Resources.cross;
            this.btnClose.ImageOffset = new System.Drawing.Point(0, 0);
            this.btnClose.ImageRotate = 0F;
            this.btnClose.ImageSize = new System.Drawing.Size(32, 32);
            this.btnClose.Name = "btnClose";
            this.btnClose.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnClose.Click += new System.EventHandler(this.btnDong_Click);
            // 
            // btnLuu
            // 
            resources.ApplyResources(this.btnLuu, "btnLuu");
            this.btnLuu.BorderColor = System.Drawing.Color.DimGray;
            this.btnLuu.BorderRadius = 10;
            this.btnLuu.BorderThickness = 3;
            this.btnLuu.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLuu.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLuu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLuu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLuu.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.btnLuu.ForeColor = System.Drawing.Color.White;
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // txtGhiChu
            // 
            resources.ApplyResources(this.txtGhiChu, "txtGhiChu");
            this.txtGhiChu.BorderRadius = 2;
            this.txtGhiChu.BorderThickness = 2;
            this.txtGhiChu.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtGhiChu.DefaultText = "";
            this.txtGhiChu.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtGhiChu.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtGhiChu.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGhiChu.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtGhiChu.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGhiChu.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtGhiChu.Name = "txtGhiChu";
            this.txtGhiChu.PlaceholderText = "";
            this.txtGhiChu.SelectedText = "";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // cbCaLam
            // 
            resources.ApplyResources(this.cbCaLam, "cbCaLam");
            this.cbCaLam.BackColor = System.Drawing.Color.Transparent;
            this.cbCaLam.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cbCaLam.BorderRadius = 2;
            this.cbCaLam.BorderThickness = 2;
            this.cbCaLam.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbCaLam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCaLam.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbCaLam.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbCaLam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbCaLam.Name = "cbCaLam";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbNhanVien
            // 
            resources.ApplyResources(this.cbNhanVien, "cbNhanVien");
            this.cbNhanVien.BackColor = System.Drawing.Color.Transparent;
            this.cbNhanVien.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cbNhanVien.BorderRadius = 2;
            this.cbNhanVien.BorderThickness = 2;
            this.cbNhanVien.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbNhanVien.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNhanVien.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbNhanVien.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbNhanVien.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbNhanVien.Name = "cbNhanVien";
            // 
            // cbCuaHang
            // 
            resources.ApplyResources(this.cbCuaHang, "cbCuaHang");
            this.cbCuaHang.BackColor = System.Drawing.Color.Transparent;
            this.cbCuaHang.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(218)))), ((int)(((byte)(223)))));
            this.cbCuaHang.BorderRadius = 2;
            this.cbCuaHang.BorderThickness = 2;
            this.cbCuaHang.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbCuaHang.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCuaHang.FocusedColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbCuaHang.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.cbCuaHang.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(88)))), ((int)(((byte)(112)))));
            this.cbCuaHang.Name = "cbCuaHang";
            // 
            // chkThu2
            // 
            resources.ApplyResources(this.chkThu2, "chkThu2");
            this.chkThu2.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu2.CheckedState.BorderRadius = 0;
            this.chkThu2.CheckedState.BorderThickness = 0;
            this.chkThu2.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu2.Name = "chkThu2";
            this.chkThu2.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkThu2.UncheckedState.BorderRadius = 0;
            this.chkThu2.UncheckedState.BorderThickness = 0;
            this.chkThu2.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // chkThu3
            // 
            resources.ApplyResources(this.chkThu3, "chkThu3");
            this.chkThu3.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu3.CheckedState.BorderRadius = 0;
            this.chkThu3.CheckedState.BorderThickness = 0;
            this.chkThu3.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu3.Name = "chkThu3";
            this.chkThu3.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkThu3.UncheckedState.BorderRadius = 0;
            this.chkThu3.UncheckedState.BorderThickness = 0;
            this.chkThu3.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // chkThu4
            // 
            resources.ApplyResources(this.chkThu4, "chkThu4");
            this.chkThu4.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu4.CheckedState.BorderRadius = 0;
            this.chkThu4.CheckedState.BorderThickness = 0;
            this.chkThu4.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu4.Name = "chkThu4";
            this.chkThu4.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkThu4.UncheckedState.BorderRadius = 0;
            this.chkThu4.UncheckedState.BorderThickness = 0;
            this.chkThu4.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // chkThu5
            // 
            resources.ApplyResources(this.chkThu5, "chkThu5");
            this.chkThu5.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu5.CheckedState.BorderRadius = 0;
            this.chkThu5.CheckedState.BorderThickness = 0;
            this.chkThu5.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu5.Name = "chkThu5";
            this.chkThu5.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkThu5.UncheckedState.BorderRadius = 0;
            this.chkThu5.UncheckedState.BorderThickness = 0;
            this.chkThu5.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // chkThu6
            // 
            resources.ApplyResources(this.chkThu6, "chkThu6");
            this.chkThu6.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu6.CheckedState.BorderRadius = 0;
            this.chkThu6.CheckedState.BorderThickness = 0;
            this.chkThu6.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu6.Name = "chkThu6";
            this.chkThu6.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkThu6.UncheckedState.BorderRadius = 0;
            this.chkThu6.UncheckedState.BorderThickness = 0;
            this.chkThu6.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // chkThu7
            // 
            resources.ApplyResources(this.chkThu7, "chkThu7");
            this.chkThu7.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu7.CheckedState.BorderRadius = 0;
            this.chkThu7.CheckedState.BorderThickness = 0;
            this.chkThu7.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkThu7.Name = "chkThu7";
            this.chkThu7.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkThu7.UncheckedState.BorderRadius = 0;
            this.chkThu7.UncheckedState.BorderThickness = 0;
            this.chkThu7.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // chkChuNhat
            // 
            resources.ApplyResources(this.chkChuNhat, "chkChuNhat");
            this.chkChuNhat.CheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkChuNhat.CheckedState.BorderRadius = 0;
            this.chkChuNhat.CheckedState.BorderThickness = 0;
            this.chkChuNhat.CheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.chkChuNhat.Name = "chkChuNhat";
            this.chkChuNhat.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            this.chkChuNhat.UncheckedState.BorderRadius = 0;
            this.chkChuNhat.UncheckedState.BorderThickness = 0;
            this.chkChuNhat.UncheckedState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(125)))), ((int)(((byte)(137)))), ((int)(((byte)(149)))));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // frmChucNang_PhanCaLam
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkChuNhat);
            this.Controls.Add(this.chkThu7);
            this.Controls.Add(this.chkThu6);
            this.Controls.Add(this.chkThu5);
            this.Controls.Add(this.chkThu4);
            this.Controls.Add(this.chkThu3);
            this.Controls.Add(this.chkThu2);
            this.Controls.Add(this.cbCuaHang);
            this.Controls.Add(this.cbNhanVien);
            this.Controls.Add(this.cbCaLam);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.txtGhiChu);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmChucNang_PhanCaLam";
            this.Load += new System.EventHandler(this.frmChucNang_PhanCaLam_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton btnClose;
        private Guna.UI2.WinForms.Guna2Button btnLuu;
        private Guna.UI2.WinForms.Guna2TextBox txtGhiChu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private Guna.UI2.WinForms.Guna2ComboBox cbCaLam;
        private System.Windows.Forms.Label label5;
        private Guna.UI2.WinForms.Guna2ComboBox cbNhanVien;
        private Guna.UI2.WinForms.Guna2ComboBox cbCuaHang;
        private Guna.UI2.WinForms.Guna2CheckBox chkThu2;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2CheckBox chkChuNhat;
        private Guna.UI2.WinForms.Guna2CheckBox chkThu7;
        private Guna.UI2.WinForms.Guna2CheckBox chkThu6;
        private Guna.UI2.WinForms.Guna2CheckBox chkThu5;
        private Guna.UI2.WinForms.Guna2CheckBox chkThu4;
        private Guna.UI2.WinForms.Guna2CheckBox chkThu3;
    }
}