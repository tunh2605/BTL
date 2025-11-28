using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class LoaiPhongBLL
    {
        private LoaiPhongDAL dal = new LoaiPhongDAL();

        public List<LoaiPhongDTO> LayDanhSachLoaiPhong()
        {
            try
            {
                return dal.LayDanhSachLoaiPhong();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy danh sách loại phòng: " + ex.Message);
            }
        }

        public void ThemLoaiPhong(string tenLoai, decimal giaPhong, string moTa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenLoai))
                    throw new Exception("Tên loại phòng không được để trống!");

                if (giaPhong <= 0)
                    throw new Exception("Giá phòng phải lớn hơn 0!");

                dal.ThemLoaiPhong(tenLoai, giaPhong, moTa);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi thêm loại phòng: " + ex.Message);
            }
        }

        public void CapNhatLoaiPhong(int maLoai, string tenLoai, decimal giaPhong, string moTa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tenLoai))
                    throw new Exception("Tên loại phòng không được để trống!");

                if (giaPhong <= 0)
                    throw new Exception("Giá phòng phải lớn hơn 0!");

                dal.CapNhatLoaiPhong(maLoai, tenLoai, giaPhong, moTa);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật loại phòng: " + ex.Message);
            }
        }

        public void XoaLoaiPhong(int maLoai)
        {
            try
            {
                dal.XoaLoaiPhong(maLoai);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi xóa loại phòng: " + ex.Message);
            }
        }

        public int LayMaLoaiPhongTiepTheo()
        {
            try
            {
                return dal.LayMaLoaiPhongTiepTheo();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy mã loại phòng tiếp theo: " + ex.Message);
            }
        }
    }
}