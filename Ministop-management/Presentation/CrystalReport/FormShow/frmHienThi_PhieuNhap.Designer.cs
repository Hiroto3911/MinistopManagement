namespace Presentation.CrystalReport.FormShow
{
    partial class frmHienThi_PhieuNhap
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
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.btnXem = new Guna.UI2.WinForms.Guna2Button();
            this.lblTime = new System.Windows.Forms.Label();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.btnThoat = new Guna.UI2.WinForms.Guna2ImageButton();
            this.txtTienNuoc = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.guna2Panel1.Controls.Add(this.txtTienNuoc);
            this.guna2Panel1.Controls.Add(this.label11);
            this.guna2Panel1.Controls.Add(this.btnThoat);
            this.guna2Panel1.Controls.Add(this.btnXem);
            this.guna2Panel1.Controls.Add(this.lblTime);
            this.guna2Panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(800, 63);
            this.guna2Panel1.TabIndex = 2;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(8, 6);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 21);
            this.label11.TabIndex = 234;
            this.label11.Text = "Phiếu nhập";
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
            this.btnXem.Location = new System.Drawing.Point(710, 29);
            this.btnXem.Margin = new System.Windows.Forms.Padding(2);
            this.btnXem.Name = "btnXem";
            this.btnXem.Size = new System.Drawing.Size(82, 29);
            this.btnXem.TabIndex = 232;
            this.btnXem.Text = "Xem";
            this.btnXem.Click += new System.EventHandler(this.btnXem_Click);
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(8, 33);
            this.lblTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(90, 21);
            this.lblTime.TabIndex = 230;
            this.lblTime.Text = "Phiếu nhập";
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(800, 450);
            this.crystalReportViewer1.TabIndex = 3;
            // 
            // guna2BorderlessForm1
            // 
            this.guna2BorderlessForm1.BorderRadius = 5;
            this.guna2BorderlessForm1.ContainerControl = this;
            this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            this.guna2BorderlessForm1.TransparentWhileDrag = true;
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
            this.btnThoat.Location = new System.Drawing.Point(774, 2);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.btnThoat.Size = new System.Drawing.Size(24, 23);
            this.btnThoat.TabIndex = 233;
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
            this.txtTienNuoc.Location = new System.Drawing.Point(115, 33);
            this.txtTienNuoc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTienNuoc.Name = "txtTienNuoc";
            this.txtTienNuoc.PlaceholderText = "";
            this.txtTienNuoc.SelectedText = "";
            this.txtTienNuoc.Size = new System.Drawing.Size(219, 30);
            this.txtTienNuoc.TabIndex = 236;
            // 
            // frmHienThi_PhieuNhap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.guna2Panel1);
            this.Controls.Add(this.crystalReportViewer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmHienThi_PhieuNhap";
            this.Text = "frmHienThi_PhieuNhap";
            this.guna2Panel1.ResumeLayout(false);
            this.guna2Panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton btnThoat;
        private Guna.UI2.WinForms.Guna2Button btnXem;
        private System.Windows.Forms.Label lblTime;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private Guna.UI2.WinForms.Guna2TextBox txtTienNuoc;
    }
}