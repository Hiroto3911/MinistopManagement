using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentation
{
    public partial class frmChucNang_SanPham : Form
    {
        bool MouseDown;
        private Point offSet;
        public frmChucNang_SanPham()
        {
            InitializeComponent();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            offSet.X = e.X; offSet.Y = e.Y;
            MouseDown = true;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (MouseDown == true)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new Point(currentScreenPos.X - offSet.X, currentScreenPos.Y - offSet.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseDown = false;
        }
        private void frmChucNang_SanPham_Load(object sender, EventArgs e)
        {
            cbo_loaisp.DropDownStyle = ComboBoxStyle.DropDownList;
            cbo_trangthai.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void frmChucNang_SanPham_Load_1(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
