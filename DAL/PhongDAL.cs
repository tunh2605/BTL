using System;
using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAL
{
    public class PhongDAL
    {
        public List<PhongDTO> LayDanhSachPhong()
        {
            string query = "exec getListPhong";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<PhongDTO> list = new List<PhongDTO>();
            foreach (DataRow row in data.Rows)
            {
                PhongDTO p = new PhongDTO(
                    Convert.ToInt32(row["MaPhong"]),
                    row["TenPhong"].ToString(),
                    Convert.ToInt32(row["MaLoaiPhong"]),
                    row["TrangThai"].ToString()
                );
                list.Add(p);
            }
            return list;
        }

        public PhongDTO LayPhongTheoMa(int maPhong)
        {
            string query = "exec getPhong @maPhong";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { maPhong });
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new PhongDTO(
                    Convert.ToInt32(row["MaPhong"]),
                    row["TenPhong"].ToString(),
                    Convert.ToInt32(row["MaLoaiPhong"]),
                    row["TrangThai"].ToString()
                );
            }
            return null;
        }

        public bool ThemPhong(int maPhong, string tenPhong, int maLoaiPhong, string trangThai)
        {
            string query = "INSERT INTO Phong (MaPhong, TenPhong, MaLoaiPhong, TrangThai) VALUES (@maPhong, @tenPhong, @maLoaiPhong, @trangThai)";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { maPhong, tenPhong, maLoaiPhong, trangThai });
            return result > 0;
        }

        public bool CapNhatPhong(int maPhong, string tenPhong, int maLoaiPhong, string trangThai)
        {
            string query = "UPDATE Phong SET TenPhong=@tenPhong, MaLoaiPhong=@maLoaiPhong, TrangThai=@trangThai WHERE MaPhong=@maPhong";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { tenPhong, maLoaiPhong, trangThai, maPhong });
            return result > 0;
        }

        public bool XoaPhong(int maPhong)
        {
            string query = "DELETE FROM Phong WHERE MaPhong=@maPhong";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { maPhong });
            return result > 0;
        }

        public List<PhongDTO> TimKiemPhong(string keyword)
        {
            string query = "EXEC findPhong @keyword";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { keyword });
            List<PhongDTO> list = new List<PhongDTO>();
            foreach (DataRow row in data.Rows)
            {
                PhongDTO p = new PhongDTO(
                    Convert.ToInt32(row["MaPhong"]),
                    row["TenPhong"].ToString(),
                    Convert.ToInt32(row["MaLoaiPhong"]),
                    row["TrangThai"].ToString()
                );
                list.Add(p);
            }
            return list;
        }

        public bool KiemTraMaPhongTonTai(int maPhong)
        {
            string query = "SELECT COUNT(*) FROM Phong WHERE MaPhong=" + maPhong;
            int count = (int)DataProvider.Instance.ExecuteScalar(query);
            return count > 0;
        }

        public List<int> LayDanhSachMaPhongDaSuDung()
        {
            string query = "SELECT MaPhong FROM Phong ORDER BY MaPhong";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<int> list = new List<int>();
            foreach (DataRow row in data.Rows)
            {
                list.Add(Convert.ToInt32(row["MaPhong"]));
            }
            return list;
        }
    }
}