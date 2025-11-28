using DAL;
using DTO;
using System.Collections.Generic;

namespace BLL
{
    public class KhachHangBLL
    {
        private KhachHangDAL dal = new KhachHangDAL();

        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            return dal.LayDanhSachKhachHang();
        }

        public KhachHangDTO LayKhachHangTheoMa(int maKH)
        {
            return dal.LayKhachHangTheoMa(maKH);
        }

        public bool ThemKhachHang(int maKH, string tenKH, string gioiTinh, string sdt, string cmnd)
        {
            if (string.IsNullOrWhiteSpace(tenKH)) return false;
            if (dal.KiemTraMaKHTonTai(maKH)) return false;
            return dal.ThemKhachHang(maKH, tenKH, gioiTinh, sdt, cmnd);
        }

        public bool CapNhatKhachHang(int maKH, string tenKH, string gioiTinh, string sdt, string cmnd)
        {
            if (string.IsNullOrWhiteSpace(tenKH)) return false;
            return dal.CapNhatKhachHang(maKH, tenKH, gioiTinh, sdt, cmnd);
        }

        public bool XoaKhachHang(int maKH)
        {
            if (dal.KiemTraKhachHangDangDuocThamChieu(maKH)) return false;
            return dal.XoaKhachHang(maKH);
        }

        public List<KhachHangDTO> TimKiemKhachHangTheoMa(int maKH)
        {
            return dal.TimKiemKhachHangTheoMa(maKH);
        }

        public int LayMaKHTiepTheo()
        {
            var used = dal.LayDanhSachMaKHDaSuDung();
            if (used.Count == 0) return 1;
            used.Sort();
            int candidate = 1;
            foreach (var id in used)
            {
                if (id == candidate) candidate++;
                else if (id > candidate) break;
            }
            return candidate;
        }
    }
}