using System;
using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAL
{
    public class LoaiPhongDAL
    {
        public List<LoaiPhongDTO> LayDanhSachLoaiPhong()
        {
            string query = "SELECT MaLoaiPhong, TenLoai, GiaPhong, MoTa FROM LoaiPhong";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<LoaiPhongDTO> list = new List<LoaiPhongDTO>();

            foreach (DataRow row in data.Rows)
            {
                list.Add(new LoaiPhongDTO(
                    Convert.ToInt32(row["MaLoaiPhong"]),
                    row["TenLoai"].ToString(),
                    Convert.ToDecimal(row["GiaPhong"]),
                    row["MoTa"].ToString()
                ));
            }
            return list;
        }

        public void ThemLoaiPhong(string tenLoai, decimal giaPhong, string moTa)
        {
            string query = "INSERT INTO LoaiPhong (TenLoai, GiaPhong, MoTa) VALUES (@tenLoai, @giaPhong, @moTa)";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { tenLoai, giaPhong, moTa });
        }

        public void CapNhatLoaiPhong(int maLoai, string tenLoai, decimal giaPhong, string moTa)
        {
            string query = "UPDATE LoaiPhong SET TenLoai = @tenLoai, GiaPhong = @giaPhong, MoTa = @moTa WHERE MaLoaiPhong = @maLoai";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { tenLoai, giaPhong, moTa, maLoai });
        }

        public void XoaLoaiPhong(int maLoai)
        {
            string query = "DELETE FROM LoaiPhong WHERE MaLoaiPhong = @maLoai";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { maLoai });
        }

        public int LayMaLoaiPhongTiepTheo()
        {
            string query = "SELECT ISNULL(MAX(MaLoaiPhong), 0) + 1 FROM LoaiPhong";
            object result = DataProvider.Instance.ExecuteScalar(query);
            return Convert.ToInt32(result);
        }
    }
}