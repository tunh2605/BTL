using System;
using System.ComponentModel;

namespace DTO
{
    public class BaoCaoDoanhThuDTO
    {
        [DisplayName("Mã hóa đơn")]
        public int MaHD { get; set; }

        [DisplayName("Mã đặt phòng")]
        public int? MaDatPhong { get; set; }

        [DisplayName("Ngày lập")]
        public DateTime? NgayLap { get; set; }

        [DisplayName("Tổng tiền")]
        public decimal? TongTien { get; set; }

        public BaoCaoDoanhThuDTO()
        {
        }

        public BaoCaoDoanhThuDTO(int maHD, int? maDatPhong, DateTime? ngayLap, decimal? tongTien)
        {
            MaHD = maHD;
            MaDatPhong = maDatPhong;
            NgayLap = ngayLap;
            TongTien = tongTien;
        }
    }
}