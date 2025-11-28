using System;

namespace DTO
{
    public class TaiKhoanDTO
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public int MaNV { get; set; }
        public string Quyen { get; set; }

        public TaiKhoanDTO() { }

        public TaiKhoanDTO( int maNV, string matKhau)
        {
            MaNV = maNV;
            MatKhau = matKhau;
        }

        public TaiKhoanDTO(string tenDangNhap, string matKhau, int maNV, string quyen)
        {
            TenDangNhap = tenDangNhap;
            MatKhau = matKhau;
            MaNV = maNV;
            Quyen = quyen;
        }
    }
}
