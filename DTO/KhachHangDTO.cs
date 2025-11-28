using System;
using System.ComponentModel;

namespace DTO
{
    public class KhachHangDTO
    {
        [DisplayName("Mã Khách Hàng") ]
        public int MaKH { get; set; }
        [DisplayName("Tên Khách Hàng")] 
        public string TenKH { get; set; }
        [DisplayName("Giới Tính")]
        public string GioiTinh { get; set; }
        [DisplayName("Số Điện Thoại")]
        public string SDT { get; set; }
        [DisplayName("Chứng Minh Nhân Dân")]
        public string CMND { get; set; }

        public KhachHangDTO() { }

        public KhachHangDTO(int maKH, string tenKH, string gioiTinh, string sdt, string cmnd)
        {
            MaKH = maKH;
            TenKH = tenKH;
            GioiTinh = gioiTinh;
            SDT = sdt;
            CMND = cmnd;
        }
    }
}
