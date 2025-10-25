using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Description;
using System.Windows.Forms;
using Unity;

namespace Presentation
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Chỉnh ứng dụng chạy full DPI
            if(System.Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            IUnityContainer container = new UnityContainer();
            ServicesRegistration.AddServiceTier(container);
            #region Them Giao Dien 
            container.RegisterType<frmMain>();
            // frm hien thi
            container.RegisterType<frmHienThi_CuaHang>();
            container.RegisterType<frmHienThi_NhanVien>();
            container.RegisterType<frmHienThi_SanPham>();
            container.RegisterType<frmHienThi_BanHang>();
            container.RegisterType<frmHienThi_KhoHang>();
            container.RegisterType<frmHienThi_ThongKe>();

            // frm Thung rac
            container.RegisterType<frmThungRac_CuaHang>();
            // frm chuc nang
            container.RegisterType<frmChucNang_CuaHang>();
            container.RegisterType<frmChucNang_PhuCap>();

            // frm dang nhap 
            container.RegisterType<frmDangNhap>();
            #endregion
            var frmDangNhap = container.Resolve<frmDangNhap>();
            Application.Run(frmDangNhap);
        }
        //Thêm Dll để hiện thị chương trình full DPI
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
