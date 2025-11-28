// BLL/TaiKhoanBLL.cs
using DAL;
using DTO;

namespace BLL
{
    public class TaiKhoanBLL
    {
        private TaiKhoanDAL dal = new TaiKhoanDAL();

        public bool DoiMatKhau(int maNV, string matKhauCu, string matKhauMoi)
        {
            if (string.IsNullOrEmpty(matKhauCu) || string.IsNullOrEmpty(matKhauMoi))
                return false;

            var taiKhoan = dal.LayTaiKhoanTheoMaNV(maNV);
            if (taiKhoan == null || taiKhoan.MatKhau != matKhauCu)
                return false;

            return dal.CapNhatMatKhau(maNV, matKhauMoi);
        }
    }
}