using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DTO;

namespace DAL
{
    public class BaoCaoDoanhThuDAL
    {
        // Lấy báo cáo doanh thu theo tháng và năm
        public List<BaoCaoDoanhThuDTO> LayBaoCaoDoanhThu(int month, int year)
        {
            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                string query = "exec getListBaoCao @startDate, @endDate";

                DataTable data = DataProvider.Instance.ExecuteQuery(query,
                    new object[] { startDate, endDate });

                List<BaoCaoDoanhThuDTO> danhSach = new List<BaoCaoDoanhThuDTO>();

                foreach (DataRow row in data.Rows)
                {
                    BaoCaoDoanhThuDTO dto = new BaoCaoDoanhThuDTO(
                        Convert.ToInt32(row["MaHD"]),
                        row["MaDatPhong"] != DBNull.Value ? (int?)Convert.ToInt32(row["MaDatPhong"]) : null,
                        row["NgayLap"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NgayLap"]) : null,
                        row["TongTien"] != DBNull.Value ? (decimal?)Convert.ToDecimal(row["TongTien"]) : null
                    );
                    danhSach.Add(dto);
                }

                return danhSach;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo doanh thu: " + ex.Message);
            }
        }

        // Tính tổng doanh thu theo tháng và năm
        public decimal TinhTongDoanhThu(int month, int year)
        {
            try
            {
                string query = "exec TinhTongDoanhThu @month, @year";
                object result = DataProvider.Instance.ExecuteScalar(query, new object[] { month, year });
                return result != null ? Convert.ToDecimal(result) : 0m;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi tính tổng doanh thu: " + ex.Message);
            }
        }

        // Lấy báo cáo theo khoảng thời gian
        public List<BaoCaoDoanhThuDTO> LayBaoCaoTheoKhoangThoiGian(DateTime tuNgay, DateTime denNgay)
        {
            try
            {
                string query = "exec GetBaoCaoDoanhThu @tuNgay, @denNgay";
                DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { tuNgay, denNgay });

                List<BaoCaoDoanhThuDTO> danhSach = new List<BaoCaoDoanhThuDTO>();

                foreach (DataRow row in data.Rows)
                {
                    BaoCaoDoanhThuDTO dto = new BaoCaoDoanhThuDTO(
                        Convert.ToInt32(row["MaHD"]),
                        row["MaDatPhong"] != DBNull.Value ? (int?)Convert.ToInt32(row["MaDatPhong"]) : null,
                        row["NgayLap"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["NgayLap"]) : null,
                        row["TongTien"] != DBNull.Value ? (decimal?)Convert.ToDecimal(row["TongTien"]) : null
                    );
                    danhSach.Add(dto);
                }

                return danhSach;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy báo cáo theo khoảng thời gian: " + ex.Message);
            }
        }

        // Lấy doanh thu theo tháng
        public DataTable LayDoanhThuTheoThang(int year)
        {
            try
            {
                string query = "exec getBaoCaoTheoThang @year";
                return DataProvider.Instance.ExecuteQuery(query, new object[] { year });
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy doanh thu theo tháng: " + ex.Message);
            }
        }
    }
}