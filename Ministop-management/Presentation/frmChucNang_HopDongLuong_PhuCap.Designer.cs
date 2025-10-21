namespace Presentation
{
    partial class frmChucNang_HopDongLuong_PhuCap
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.guna2ImageButton4 = new Guna.UI2.WinForms.Guna2ImageButton();
            this.dgvDuLieu = new Guna.UI2.WinForms.Guna2DataGridView();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuLieu)).BeginInit();
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
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(889, 54);
            this.panel1.TabIndex = 172;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(26, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(438, 32);
            this.label11.TabIndex = 2;
            this.label11.Text = "Thêm phụ cấp cho nhân viên fulltime";
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
            this.guna2ImageButton4.Location = new System.Drawing.Point(841, 9);
            this.guna2ImageButton4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.guna2ImageButton4.Name = "guna2ImageButton4";
            this.guna2ImageButton4.PressedState.ImageSize = new System.Drawing.Size(64, 64);
            this.guna2ImageButton4.Size = new System.Drawing.Size(36, 35);
            this.guna2ImageButton4.TabIndex = 1;
            this.guna2ImageButton4.Click += new System.EventHandler(this.guna2ImageButton4_Click);
            // 
            // dgvDuLieu
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvDuLieu.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvDuLieu.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDuLieu.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvDuLieu.BackgroundColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDuLieu.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvDuLieu.ColumnHeadersHeight = 36;
            this.dgvDuLieu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDuLieu.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvDuLieu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDuLieu.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDuLieu.Location = new System.Drawing.Point(0, 54);
            this.dgvDuLieu.Name = "dgvDuLieu";
            this.dgvDuLieu.RowHeadersVisible = false;
            this.dgvDuLieu.RowHeadersWidth = 62;
            this.dgvDuLieu.RowTemplate.Height = 28;
            this.dgvDuLieu.Size = new System.Drawing.Size(889, 344);
            this.dgvDuLieu.TabIndex = 173;
            this.dgvDuLieu.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDuLieu.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvDuLieu.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvDuLieu.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvDuLieu.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvDuLieu.ThemeStyle.BackColor = System.Drawing.SystemColors.Control;
            this.dgvDuLieu.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDuLieu.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvDuLieu.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvDuLieu.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDuLieu.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvDuLieu.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvDuLieu.ThemeStyle.HeaderStyle.Height = 36;
            this.dgvDuLieu.ThemeStyle.ReadOnly = false;
            this.dgvDuLieu.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvDuLieu.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvDuLieu.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvDuLieu.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvDuLieu.ThemeStyle.RowsStyle.Height = 28;
            this.dgvDuLieu.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvDuLieu.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // guna2Button1
            // 
            this.guna2Button1.BorderColor = System.Drawing.Color.DimGray;
            this.guna2Button1.BorderThickness = 3;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(56)))), ((int)(((byte)(148)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(0, 353);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(889, 45);
            this.guna2Button1.TabIndex = 227;
            this.guna2Button1.Text = "Thêm";
            // 
            // frmChucNang_HopDongLuong_PhuCap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 398);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.dgvDuLieu);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmChucNang_HopDongLuong_PhuCap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmChucNang_HopDongLuong_PhuCap";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDuLieu)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private Guna.UI2.WinForms.Guna2ImageButton guna2ImageButton4;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDuLieu;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}