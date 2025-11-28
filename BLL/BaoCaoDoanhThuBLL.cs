using System;
using System.Collections.Generic;
using System.Data;
using DAL;
using DTO;

namespace BLL
{
    public class BaoCaoDoanhThuBLL
    {
        private BaoCaoDoanhThuDAL dal = new BaoCaoDoanhThuDAL();

        public List<BaoCaoDoanhThuDTO> LayBaoCaoDoanhThu(int month, int year)
        {
            // Validate input
            if (month < 1 || month > 12)
                throw new ArgumentException("Tháng không hợp lệ. Vui lòng chọn từ 1-12.");

            if (year < 2000 || year > DateTime.Now.Year + 10)
                throw new ArgumentException("Năm không hợp lệ.");

            return dal.LayBaoCaoDoanhThu(month, year);
        }

        public decimal TinhTongDoanhThu(int month, int year)
        {
            if (month < 1 || month > 12)
                throw new ArgumentException("Tháng không hợp lệ.");

            if (year < 2000 || year > DateTime.Now.Year + 10)
                throw new ArgumentException("Năm không hợp lệ.");

            return dal.TinhTongDoanhThu(month, year);
        }

        public string FormatTien(decimal tien)
        {
            return tien.ToString("N0") + " VNĐ";
        }

        // Lấy báo cáo theo khoảng thời gian
        public List<BaoCaoDoanhThuDTO> LayBaoCaoTheoKhoangThoiGian(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay > denNgay)
                throw new ArgumentException("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.");

            if (denNgay > DateTime.Now)
                throw new ArgumentException("Ngày kết thúc không được vượt quá ngày hiện tại.");

            return dal.LayBaoCaoTheoKhoangThoiGian(tuNgay, denNgay);
        }

        // Lấy doanh thu theo tháng trong năm
        public DataTable LayDoanhThuTheoThang(int year)
        {
            if (year < 2000 || year > DateTime.Now.Year + 10)
                throw new ArgumentException("Năm không hợp lệ.");

            return dal.LayDoanhThuTheoThang(year);
        }

        // Kiểm tra có dữ liệu kh
        public bool KiemTraCoDuLieu(int month, int year)
        {
            var danhSach = dal.LayBaoCaoDoanhThu(month, year);
            return danhSach != null && danhSach.Count > 0;
        }
    }
}
