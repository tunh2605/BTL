using System;

namespace DTO
{
    public class ChiTietDatPhongDTO
    {
        public int MaDatPhong { get; set; }
        public int MaPhong { get; set; }
        public decimal DonGia { get; set; }

        public ChiTietDatPhongDTO() { }

        public ChiTietDatPhongDTO(int maDatPhong, int maPhong, decimal donGia)
        {
            MaDatPhong = maDatPhong;
            MaPhong = maPhong;
            DonGia = donGia;
        }
    }
}
