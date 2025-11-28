using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BLL
{
    public class DatPhongBLL
    {
        private DatPhongDAL dal = new DatPhongDAL();

        // Lấy danh sách đặt phòng
        public List<DatPhongDTO> LayDanhSachDatPhong()
        {
            return dal.LayDanhSachDatPhong();
        }

        // Lấy danh sách khách hàng
        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            return dal.LayDanhSachKhachHang();
        }

        // Lấy danh sách nhân viên
        public List<NhanVienDTO> LayDanhSachNhanVien()
        {
            return dal.LayDanhSachNhanVien();
        }

        // Lấy danh sách phòng trống
        public List<PhongDTO> LayDanhSachPhongTrong()
        {
            return dal.LayDanhSachPhongTrong();
        }

        // Lấy đơn giá phòng
        public decimal LayDonGiaPhong(int maPhong)
        {
            return dal.LayDonGiaPhong(maPhong);
        }

        // Lấy mã đặt phòng tiếp theo
        public int LayMaDatPhongTiepTheo()
        {
            return dal.LayMaDatPhongTiepTheo();
        }

        // Tính tổng tiền dựa trên đơn giá và số ngày
        public decimal TinhTongTien(decimal donGia, DateTime ngayDen, DateTime ngayDi)
        {
            int soNgay = (ngayDi.Date - ngayDen.Date).Days;
            if (soNgay < 1) soNgay = 1;
            return donGia * soNgay;
        }

        // Tính số ngày ở
        public int TinhSoNgay(DateTime ngayDen, DateTime ngayDi)
        {
            int soNgay = (ngayDi.Date - ngayDen.Date).Days;
            return soNgay < 1 ? 1 : soNgay;
        }

        // Validate dữ liệu đặt phòng
        public bool ValidateDatPhong(int maKH, int maNV, int maPhong, DateTime ngayDen, DateTime ngayDi, out string error)
        {
            error = string.Empty;

            if (maKH <= 0)
            {
                error = "Vui lòng chọn khách hàng!";
                return false;
            }

            if (maNV <= 0)
            {
                error = "Vui lòng chọn nhân viên!";
                return false;
            }

            if (maPhong <= 0)
            {
                error = "Vui lòng chọn phòng!";
                return false;
            }

            if (ngayDen.Date < DateTime.Now.Date)
            {
                error = "Ngày đến không được nhỏ hơn ngày hiện tại!";
                return false;
            }
            return true;
        }

        // Thêm đặt phòng 
        public bool ThemDatPhong(DatPhongDTO dto, out string error)
        {
            error = string.Empty;
            if (!ValidateDatPhong(dto.MaKH, dto.MaNV, dto.MaPhong, dto.NgayDen, dto.NgayDi, out error))
            {
                return false;
            }
            if (dto.MaDatPhong <= 0)
            {
                dto.MaDatPhong = dal.LayMaDatPhongTiepTheo();
            }
            if (dal.KiemTraMaDatPhongTonTai(dto.MaDatPhong))
            {
                error = "Mã đặt phòng đã tồn tại. Vui lòng bấm Làm mới để lấy mã khác.";
                return false;
            }
            string trangThai = dal.KiemTraTrangThaiPhong(dto.MaPhong);
            if (string.IsNullOrEmpty(trangThai))
            {
                error = "Không tìm thấy phòng đã chọn!";
                return false;
            }
            if (!string.Equals(trangThai, "Trống", StringComparison.OrdinalIgnoreCase))
            {
                error = "Phòng này hiện không còn trống. Vui lòng chọn phòng khác.";
                return false;
            }
            decimal donGia = dal.LayDonGiaPhong(dto.MaPhong);
            if (donGia <= 0)
            {
                error = "Không thể lấy đơn giá phòng!";
                return false;
            }
            if (!dal.ThemDatPhong(dto, out error))
            {
                return false;
            }
            if (!dal.ThemChiTietDatPhong(dto.MaDatPhong, dto.MaPhong, donGia, out error))
            {
                return false;
            }
            if (!dal.CapNhatTrangThaiPhong(dto.MaPhong, "Đang ở", out error))
            {
                return false;
            }
            int soNgay = TinhSoNgay(dto.NgayDen, dto.NgayDi);
            decimal tongTien = TinhTongTien(donGia, dto.NgayDen, dto.NgayDi);

            if (!dal.ThemHoaDon(dto.MaDatPhong, DateTime.Now, tongTien, out error))
            {
                return false;
            }

            return true;
        }

        // Trả phòng
        public bool XoaDatPhong(int maDatPhong, out string error)
        {
            error = string.Empty;

            if (maDatPhong <= 0)
            {
                error = "Mã đặt phòng không hợp lệ!";
                return false;
            }

            return dal.XoaDatPhong(maDatPhong, out error);
        }

        // Tìm kiếm 
        public List<DatPhongDTO> TimKiemDatPhong(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return LayDanhSachDatPhong();
            }
            if (int.TryParse(keyword, out int maDatPhong))
            {
                return dal.TimKiemTheoMa(maDatPhong);
            }
            else
            {           
                return dal.TimKiemDatPhong(keyword);
            }
        }
    }
}