using System;
using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAL
{
    public class HoaDonDAL
    {
        public List<HoaDonDTO> LayDanhSachHoaDon()
        {
            string query = "SELECT * FROM HoaDon";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<HoaDonDTO> list = new List<HoaDonDTO>();
            foreach (DataRow row in data.Rows)
            {
                HoaDonDTO hd = new HoaDonDTO(
                    Convert.ToInt32(row["MaHD"]),
                    row["MaDatPhong"] != DBNull.Value ? Convert.ToInt32(row["MaDatPhong"]) : 0,
                    row["NgayLap"] != DBNull.Value ? Convert.ToDateTime(row["NgayLap"]) : DateTime.Now,
                    row["TongTien"] != DBNull.Value ? Convert.ToDecimal(row["TongTien"]) : 0
                );
                list.Add(hd);
            }
            return list;
        }

        public HoaDonDTO LayHoaDonTheoMa(int maHD)
        {
            string query = "SELECT * FROM HoaDon WHERE MaHD=@maHD";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { maHD });
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new HoaDonDTO(
                    Convert.ToInt32(row["MaHD"]),
                    row["MaDatPhong"] != DBNull.Value ? Convert.ToInt32(row["MaDatPhong"]) : 0,
                    row["NgayLap"] != DBNull.Value ? Convert.ToDateTime(row["NgayLap"]) : DateTime.Now,
                    row["TongTien"] != DBNull.Value ? Convert.ToDecimal(row["TongTien"]) : 0
                );
            }
            return null;
        }

        public bool CapNhatHoaDon(HoaDonDTO hd, int? maDatPhong)
        {
            string query = "UPDATE HoaDon SET MaDatPhong=@maDatPhong, NgayLap=@ngayLap, TongTien=@tongTien WHERE MaHD=@maHD";
            object maDatPhongValue = maDatPhong.HasValue ? (object)maDatPhong.Value : DBNull.Value;
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { maDatPhongValue, hd.NgayLap, hd.TongTien, hd.MaHD });
            return result > 0;
        }

        public bool XoaHoaDon(int maHD)
        {
            string query = "DELETE FROM HoaDon WHERE MaHD=@maHD";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { maHD });
            return result > 0;
        }

        public List<HoaDonDTO> TimKiemHoaDon(string keyword)
        {
            string query = "exec findHoaDon @keyword";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] {keyword});
            List<HoaDonDTO> list = new List<HoaDonDTO>();
            foreach (DataRow row in data.Rows)
            {
                HoaDonDTO hd = new HoaDonDTO(
                    Convert.ToInt32(row["MaHD"]),
                    row["MaDatPhong"] != DBNull.Value ? Convert.ToInt32(row["MaDatPhong"]) : 0,
                    row["NgayLap"] != DBNull.Value ? Convert.ToDateTime(row["NgayLap"]) : DateTime.Now,
                    row["TongTien"] != DBNull.Value ? Convert.ToDecimal(row["TongTien"]) : 0
                );
                list.Add(hd);
            }
            return list;
        }
    }
}