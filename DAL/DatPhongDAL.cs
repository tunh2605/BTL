using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DTO;

namespace DAL
{
    public class DatPhongDAL
    {

        public List<DatPhongDTO> LayDanhSachDatPhong()
        {
            string query = "exec getListDatPhong";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<DatPhongDTO> danhSach = new List<DatPhongDTO>();

            foreach (DataRow row in data.Rows)
            {
                DatPhongDTO dto = new DatPhongDTO(
                    Convert.ToInt32(row["MaDatPhong"]),
                    row["TenKH"].ToString(),
                    row["TenNV"].ToString(),
                    row["TenPhong"].ToString(),
                    Convert.ToDateTime(row["NgayDat"]),
                    Convert.ToDateTime(row["NgayDen"]),
                    Convert.ToDateTime(row["NgayDi"]),
                    Convert.ToDecimal(row["TongTien"])
                );

                dto.MaKH = Convert.ToInt32(row["MaKH"]);
                dto.MaNV = Convert.ToInt32(row["MaNV"]);
                dto.MaPhong = Convert.ToInt32(row["MaPhong"]);

                danhSach.Add(dto);
            }
            return danhSach;
        }

        // Lấy danh sách khách hàng
        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            string query = "SELECT MaKH, TenKH, GioiTinh, SDT, CMND FROM KhachHang";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<KhachHangDTO> danhSach = new List<KhachHangDTO>();

            foreach (DataRow row in data.Rows)
            {
                danhSach.Add(new KhachHangDTO(
                    Convert.ToInt32(row["MaKH"]),
                    row["TenKH"].ToString(),
                    row["GioiTinh"].ToString(),
                    row["SDT"].ToString(),
                    row["CMND"].ToString()
                ));
            }
            return danhSach;
        }

        // Lấy danh sách nhân viên
        public List<NhanVienDTO> LayDanhSachNhanVien()
        {
            string query = "SELECT MaNV, TenNV, GioiTinh, NgaySinh, DiaChi FROM NhanVien";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<NhanVienDTO> danhSach = new List<NhanVienDTO>();

            foreach (DataRow row in data.Rows)
            {
                danhSach.Add(new NhanVienDTO(
                    Convert.ToInt32(row["MaNV"]),
                    row["TenNV"].ToString(),
                    row["GioiTinh"].ToString(),
                    Convert.ToDateTime(row["NgaySinh"]),
                    row["DiaChi"].ToString()
                ));
            }
            return danhSach;
        }

        // Lấy danh sách phòng trống
        public List<PhongDTO> LayDanhSachPhongTrong()
        {
            string query = "exec getListPhongTrong";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<PhongDTO> danhSach = new List<PhongDTO>();

            foreach (DataRow row in data.Rows)
            {
                PhongDTO dto = new PhongDTO(
                    Convert.ToInt32(row["MaPhong"]),
                    row["TenPhong"].ToString(),
                    Convert.ToInt32(row["MaLoaiPhong"]),
                    row["TrangThai"].ToString()
                );
                dto.MaLoaiPhong = Convert.ToInt32(row["MaLoaiPhong"]);
                danhSach.Add(dto);
            }
            return danhSach;
        }

        // Lấy đơn giá phòng theo mã phòng
        public decimal LayDonGiaPhong(int maPhong)
        {
            string query = "exec getDonGia @maPhong";

            object result = DataProvider.Instance.ExecuteScalar(query, new object[] { maPhong });
            return result != null ? Convert.ToDecimal(result) : 0m;
        }

        // Kiểm tra trạng thái phòng
        public string KiemTraTrangThaiPhong(int maPhong)
        {
            string query = "SELECT TrangThai FROM Phong WHERE MaPhong = @maPhong ";
            object result = DataProvider.Instance.ExecuteScalar(query, new object[] { maPhong });
            return result != null ? result.ToString() : null;
        }

        // Lấy mã đặt phòng tiếp theo
        public int LayMaDatPhongTiepTheo()
        {
            string query = "SELECT MaDatPhong FROM DatPhong ORDER BY MaDatPhong";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data.Rows.Count == 0) return 1;

            List<int> used = new List<int>();
            foreach (DataRow row in data.Rows)
            {
                used.Add(Convert.ToInt32(row["MaDatPhong"]));
            }

            used.Sort();
            int candidate = 1;
            foreach (var id in used)
            {
                if (id == candidate) candidate++;
                else if (id > candidate) break;
            }
            return candidate;
        }

        // Kiểm tra mã đặt phòng đã tồn tại
        public bool KiemTraMaDatPhongTonTai(int maDatPhong)
        {
            string query = "SELECT COUNT(*) FROM DatPhong WHERE MaDatPhong = @maDatPhong ";
            object result = DataProvider.Instance.ExecuteScalar(query, new object[] { maDatPhong });
            return Convert.ToInt32(result) > 0;
        }

        // Thêm đặt phòng mới
        public bool ThemDatPhong(DatPhongDTO dto, out string error)
        {
            error = string.Empty;
            try
            {
                string query = @"INSERT INTO DatPhong (MaDatPhong, MaKH, MaNV, MaPhong, NgayDat, NgayDen, NgayDi)
                                VALUES ( @maDatPhong , @maKH , @maNV , @maPhong , @ngayDat , @ngayDen , @ngayDi )";

                int result = DataProvider.Instance.ExecuteNonQuery(query, new object[]
                {
                    dto.MaDatPhong, dto.MaKH, dto.MaNV, dto.MaPhong,
                    dto.NgayDat, dto.NgayDen, dto.NgayDi
                });

                return result > 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Thêm chi tiết đặt phòng
        public bool ThemChiTietDatPhong(int maDatPhong, int maPhong, decimal donGia, out string error)
        {
            error = string.Empty;
            try
            {
                string query = @"INSERT INTO ChiTietDatPhong (MaDatPhong, MaPhong, DonGia)
                                VALUES ( @maDatPhong , @maPhong , @donGia )";

                int result = DataProvider.Instance.ExecuteNonQuery(query,
                    new object[] { maDatPhong, maPhong, donGia });

                return result > 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Cập nhật trạng thái phòng
        public bool CapNhatTrangThaiPhong(int maPhong, string trangThai, out string error)
        {
            error = string.Empty;
            try
            {
                string query = "UPDATE Phong SET TrangThai = @trangThai WHERE MaPhong = @maPhong ";
                int result = DataProvider.Instance.ExecuteNonQuery(query,
                    new object[] { trangThai, maPhong });

                return result > 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Lấy mã hóa đơn tiếp theo
        public int LayMaHoaDonTiepTheo()
        {
            string query = "SELECT ISNULL(MAX(MaHD), 0) + 1 FROM HoaDon";
            object result = DataProvider.Instance.ExecuteScalar(query);
            return Convert.ToInt32(result);
        }

        // Thêm hóa đơn
        public bool ThemHoaDon(int maDatPhong, DateTime ngayLap, decimal tongTien, out string error)
        {
            error = string.Empty;
            try
            {
                string query = @"INSERT INTO HoaDon (MaDatPhong, NgayLap, TongTien)
                                VALUES ( @maDatPhong , @ngayLap , @tongTien )";

                int result = DataProvider.Instance.ExecuteNonQuery(query,
                    new object[] { maDatPhong, ngayLap, tongTien });

                return result > 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Xóa đặt phòng (trả phòng)
        public bool XoaDatPhong(int maDatPhong, out string error)
        {
            error = string.Empty;
            try
            {
                // Cập nhật hóa đơn trước
                string query1 = "UPDATE HoaDon SET MaDatPhong = NULL WHERE MaDatPhong = @maDatPhong ";
                DataProvider.Instance.ExecuteNonQuery(query1, new object[] { maDatPhong });

                // Lấy danh sách phòng từ chi tiết để cập nhật trạng thái
                string query2 = "SELECT MaPhong FROM ChiTietDatPhong WHERE MaDatPhong = @maDatPhong ";
                DataTable dtPhong = DataProvider.Instance.ExecuteQuery(query2, new object[] { maDatPhong });

                foreach (DataRow row in dtPhong.Rows)
                {
                    int maPhong = Convert.ToInt32(row["MaPhong"]);
                    CapNhatTrangThaiPhong(maPhong, "Trống", out _);
                }

                // Xóa chi tiết đặt phòng
                string query3 = "DELETE FROM ChiTietDatPhong WHERE MaDatPhong = @maDatPhong ";
                DataProvider.Instance.ExecuteNonQuery(query3, new object[] { maDatPhong });

                // Xóa đặt phòng
                string query4 = "DELETE FROM DatPhong WHERE MaDatPhong = @maDatPhong ";
                int result = DataProvider.Instance.ExecuteNonQuery(query4, new object[] { maDatPhong });

                return result > 0;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        // Tìm kiếm 
        public List<DatPhongDTO> TimKiemDatPhong(string keyword)
        {
            string query = "exec findDatPhong @keyword";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { keyword });
            List<DatPhongDTO> danhSach = new List<DatPhongDTO>();

            foreach (DataRow row in data.Rows)
            {
                DatPhongDTO dto = new DatPhongDTO(
                    Convert.ToInt32(row["MaDatPhong"]),
                    row["TenKH"].ToString(),
                    row["TenNV"].ToString(),
                    row["TenPhong"].ToString(),
                    Convert.ToDateTime(row["NgayDat"]),
                    Convert.ToDateTime(row["NgayDen"]),
                    Convert.ToDateTime(row["NgayDi"]),
                    Convert.ToDecimal(row["TongTien"])
                );
                danhSach.Add(dto);
            }
            return danhSach;
        }

        // Tìm kiếm theo mã đặt phòng
        public List<DatPhongDTO> TimKiemTheoMa(int maDatPhong)
        {
            string query = "exec findDatPhongTheoMa @maDatPhong";

            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { maDatPhong });
            List<DatPhongDTO> danhSach = new List<DatPhongDTO>();

            foreach (DataRow row in data.Rows)
            {
                DatPhongDTO dto = new DatPhongDTO(
                    Convert.ToInt32(row["MaDatPhong"]),
                    row["TenKH"].ToString(),
                    row["TenNV"].ToString(),
                    row["TenPhong"].ToString(),
                    Convert.ToDateTime(row["NgayDat"]),
                    Convert.ToDateTime(row["NgayDen"]),
                    Convert.ToDateTime(row["NgayDi"]),
                    Convert.ToDecimal(row["TongTien"])
                );
                danhSach.Add(dto);
            }
            return danhSach;
        }
    }
}