using System;
using System.ComponentModel;

namespace DTO
{
    public class DatPhongDTO
    {
        [DisplayName("Mã đặt phòng")]
        public int MaDatPhong { get; set; }

        [DisplayName("Khách hàng")]
        public string TenKH { get; set; }

        [DisplayName("Nhân viên")]
        public string TenNV { get; set; }

        [DisplayName("Phòng")]
        public string TenPhong { get; set; }

        [DisplayName("Ngày đặt")]
        public DateTime NgayDat { get; set; }

        [DisplayName("Ngày đến")]
        public DateTime NgayDen { get; set; }

        [DisplayName("Ngày đi")]
        public DateTime NgayDi { get; set; }

        [DisplayName("Tổng tiền")]
        public decimal TongTien { get; set; }

        [Browsable(false)]
        public int MaKH { get; set; }

        [Browsable(false)]
        public int MaNV { get; set; }

        [Browsable(false)]
        public int MaPhong { get; set; }

        public DatPhongDTO() { }

        public DatPhongDTO(int maDatPhong, int maKH, int maNV, int maPhong,
                          DateTime ngayDat, DateTime ngayDen, DateTime ngayDi)
        {
            MaDatPhong = maDatPhong;
            MaKH = maKH;
            MaNV = maNV;
            MaPhong = maPhong;
            NgayDat = ngayDat;
            NgayDen = ngayDen;
            NgayDi = ngayDi;
        }

        public DatPhongDTO(int maDatPhong, string tenKH, string tenNV,
                          string tenPhong, DateTime ngayDat, DateTime ngayDen,
                          DateTime ngayDi, decimal tongTien)
        {
            MaDatPhong = maDatPhong;
            TenKH = tenKH;
            TenNV = tenNV;
            TenPhong = tenPhong;
            NgayDat = ngayDat;
            NgayDen = ngayDen;
            NgayDi = ngayDi;
            TongTien = tongTien;
        }
    }
}