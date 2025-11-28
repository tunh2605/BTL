using System;
using System.ComponentModel;

namespace DTO
{
    public class LoaiPhongDTO
    {
        [DisplayName("Mã loại phòng")]
        public int MaLoaiPhong { get; set; }
        [DisplayName("Tên loại")]
        public string TenLoai { get; set; }
        [DisplayName("Giá phòng")]
        public decimal GiaPhong { get; set; }
        [DisplayName("Mô tả")]
        public string MoTa { get; set; }

        public LoaiPhongDTO() { }

        public LoaiPhongDTO(int maLoaiPhong, string tenLoai, decimal giaPhong, string moTa)
        {
            MaLoaiPhong = maLoaiPhong;
            TenLoai = tenLoai;
            GiaPhong = giaPhong;
            MoTa = moTa;
        }
    }
}
