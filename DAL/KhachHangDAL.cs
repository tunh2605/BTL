using DTO;
using System;
using System.Collections.Generic;
using System.Data;

namespace DAL
{
    public class KhachHangDAL
    {
        public List<KhachHangDTO> LayDanhSachKhachHang()
        {
            string query = "SELECT MaKH, TenKH, GioiTinh, SDT, CMND FROM KhachHang";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<KhachHangDTO> list = new List<KhachHangDTO>();
            foreach (DataRow row in data.Rows)
            {
                KhachHangDTO kh = new KhachHangDTO(
                    Convert.ToInt32(row["MaKH"]),
                    row["TenKH"].ToString(),
                    row["GioiTinh"].ToString(),
                    row["SDT"].ToString(),
                    row["CMND"].ToString()
                );
                list.Add(kh);
            }
            return list;
        }

        public KhachHangDTO LayKhachHangTheoMa(int maKH)
        {
            string query = "SELECT MaKH, TenKH, GioiTinh, SDT, CMND FROM KhachHang WHERE MaKH=@maKH";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { maKH });
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new KhachHangDTO(
                    Convert.ToInt32(row["MaKH"]),
                    row["TenKH"].ToString(),
                    row["GioiTinh"].ToString(),
                    row["SDT"].ToString(),
                    row["CMND"].ToString()
                );
            }
            return null;
        }

        public bool ThemKhachHang(int maKH, string tenKH, string gioiTinh, string sdt, string cmnd)
        {
            string query = "INSERT INTO KhachHang (MaKH, TenKH, GioiTinh, SDT, CMND) VALUES (@maKH, @tenKH, @gioiTinh, @sdt, @cmnd)";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { maKH, tenKH, gioiTinh, sdt, cmnd });
            return result > 0;
        }

        public bool CapNhatKhachHang(int maKH, string tenKH, string gioiTinh, string sdt, string cmnd)
        {
            string query = "UPDATE KhachHang SET TenKH=@tenKH, GioiTinh=@gioiTinh, SDT=@sdt, CMND=@cmnd WHERE MaKH=@maKH";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { tenKH, gioiTinh, sdt, cmnd, maKH });
            return result > 0;
        }

        public bool XoaKhachHang(int maKH)
        {
            string query = "DELETE FROM KhachHang WHERE MaKH=@maKH";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { maKH });
            return result > 0;
        }

        public List<KhachHangDTO> TimKiemKhachHangTheoMa(int maKH)
        {
            string query = "SELECT MaKH, TenKH, GioiTinh, SDT, CMND FROM KhachHang WHERE MaKH=@maKH";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { maKH });
            List<KhachHangDTO> list = new List<KhachHangDTO>();
            foreach (DataRow row in data.Rows)
            {
                KhachHangDTO kh = new KhachHangDTO(
                    Convert.ToInt32(row["MaKH"]),
                    row["TenKH"].ToString(),
                    row["GioiTinh"].ToString(),
                    row["SDT"].ToString(),
                    row["CMND"].ToString()
                );
                list.Add(kh);
            }
            return list;
        }

        public bool KiemTraMaKHTonTai(int maKH)
        {
            string query = "SELECT COUNT(*) FROM KhachHang WHERE MaKH=" + maKH;
            int count = (int)DataProvider.Instance.ExecuteScalar(query);
            return count > 0;
        }

        public List<int> LayDanhSachMaKHDaSuDung()
        {
            string query = "SELECT MaKH FROM KhachHang ORDER BY MaKH";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<int> list = new List<int>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(Convert.ToInt32(row["MaKH"]));
            }
            return list;
        }

        public bool KiemTraKhachHangDangDuocThamChieu(int maKH)
        {
            string query = "SELECT COUNT(*) FROM DatPhong WHERE MaKH=" + maKH;
            int count = (int)DataProvider.Instance.ExecuteScalar(query);
            return count > 0;
        }
    }
}