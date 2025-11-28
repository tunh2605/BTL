using DTO;
using System;
using System.Data;

namespace DAL
{
    public class TaiKhoanDAL
    {
        public TaiKhoanDTO LayTaiKhoanTheoMaNV(int maNV)
        {
            string query = "SELECT MaNV, MatKhau FROM TaiKhoan WHERE MaNV=@maNV";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { maNV });
            if (data.Rows.Count > 0)
            {
                DataRow row = data.Rows[0];
                return new TaiKhoanDTO(
                    Convert.ToInt32(row["MaNV"]),
                    row["MatKhau"].ToString()
                );
            }
            return null;
        }

        public bool CapNhatMatKhau(int maNV, string matKhauMoi)
        {
            string query = "UPDATE TaiKhoan SET MatKhau=@matKhau WHERE MaNV=@maNV";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { matKhauMoi, maNV });
            return result > 0;
        }
    }
}