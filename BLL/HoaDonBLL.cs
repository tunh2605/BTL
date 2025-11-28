using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace BLL
{
    public class HoaDonBLL
    {
        private HoaDonDAL dal = new HoaDonDAL();

        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            return dal.LayDanhSachHoaDon();
        }

        public HoaDonDTO LayHoaDonTheoMa(int maHD)
        {
            return dal.LayHoaDonTheoMa(maHD);
        }

        public bool CapNhatHoaDon(int maHD, int? maDatPhong, DateTime ngayLap, decimal tongTien)
        {
            HoaDonDTO hd = new HoaDonDTO
            {
                MaHD = maHD,
                NgayLap = ngayLap,
                TongTien = tongTien
            };
            return dal.CapNhatHoaDon(hd, maDatPhong);
        }

        public bool XoaHoaDon(int maHD)
        {
            return dal.XoaHoaDon(maHD);
        }

        public List<HoaDonDTO> TimKiemHoaDon(string keyword)
        {
            return dal.TimKiemHoaDon(keyword);
        }

        public bool KiemTraMaDatPhong(string text, out int? maDatPhong)
        {
            maDatPhong = null;
            if (string.IsNullOrWhiteSpace(text))
                return true;

            if (!int.TryParse(text, out int tmp))
                return false;

            maDatPhong = tmp;
            return true;
        }

        public bool KiemTraTongTien(string text, out decimal tongTien)
        {
            tongTien = 0;
            var vi = CultureInfo.GetCultureInfo("vi-VN");
            return decimal.TryParse(text, NumberStyles.Number, vi, out tongTien) ||
                   decimal.TryParse(text, NumberStyles.Number, CultureInfo.InvariantCulture, out tongTien);
        }
    }
}