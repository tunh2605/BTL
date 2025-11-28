using DAL;
using DTO;
using System.Collections.Generic;

namespace BLL
{
    public class PhongBLL
    {
        private PhongDAL dal = new PhongDAL();

        public List<PhongDTO> LayDanhSachPhong()
        {
            return dal.LayDanhSachPhong();
        }

        public PhongDTO LayPhongTheoMa(int maPhong)
        {
            return dal.LayPhongTheoMa(maPhong);
        }

        public bool ThemPhong(int maPhong, string tenPhong, int maLoaiPhong, string trangThai)
        {
            return dal.ThemPhong(maPhong, tenPhong, maLoaiPhong, trangThai);
        }

        public bool CapNhatPhong(int maPhong, string tenPhong, int maLoaiPhong, string trangThai)
        {
            return dal.CapNhatPhong(maPhong, tenPhong, maLoaiPhong, trangThai);
        }

        public bool XoaPhong(int maPhong)
        {
            return dal.XoaPhong(maPhong);
        }

        public List<PhongDTO> TimKiemPhong(string keyword)
        {
            return dal.TimKiemPhong(keyword);
        }

        public bool KiemTraMaPhongTonTai(int maPhong)
        {
            return dal.KiemTraMaPhongTonTai(maPhong);
        }

        public int LayMaPhongTiepTheo()
        {
            List<int> used = dal.LayDanhSachMaPhongDaSuDung();
            if (used.Count == 0) return 1;

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