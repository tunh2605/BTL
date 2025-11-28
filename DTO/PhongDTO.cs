using System;
using System.ComponentModel;

namespace DTO
{
    public class PhongDTO
    {
        [DisplayName("Mã Phòng")]
        public int MaPhong { get; set; }
        [DisplayName("Tên Phòng")]
        public string TenPhong { get; set; }
        [DisplayName("Mã Loại Phòng")]
        public int MaLoaiPhong { get; set; }
        [DisplayName("Trạng Thái")]
        public string TrangThai { get; set; }

        public PhongDTO() { }

        public PhongDTO(int maPhong, string tenPhong, int maLoaiPhong, string trangThai)
        {
            MaPhong = maPhong;
            TenPhong = tenPhong;
            MaLoaiPhong = maLoaiPhong;
            TrangThai = trangThai;
        }
    }
}
