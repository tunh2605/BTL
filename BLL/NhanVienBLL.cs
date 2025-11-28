using System.Collections.Generic;
using DTO;
using DAL;
using System.ComponentModel;

namespace BLL
{
    public class NhanVienBLL
    {
        private NhanVienDAL _dal = new NhanVienDAL();

        public List<NhanVienDTO> LayDanhSachNhanVien()
        {
            return _dal.LayDanhSachNhanVien();
        }

        public bool ThemNhanVien(NhanVienDTO nv)
        {
            return _dal.ThemNhanVien(nv);
        }

        public bool CapNhatNhanVien(NhanVienDTO nv)
        {
            return _dal.CapNhatNhanVien(nv);
        }

        public bool XoaNhanVien(int maNV)
        {
            return _dal.XoaNhanVien(maNV);
        }

        public List<NhanVienDTO> TimKiemNhanVien(string keyword)
        {
            return _dal.TimKiemNhanVien(keyword);
        }
    }
}

