using DAL;
using DTO;
using System;
using System.Data;

namespace DAL
{
    public class LoginDAL
    {
        private DataProvider dp = DataProvider.Instance;

        public TaiKhoanDTO GetTaiKhoan(string username, string password)
        {
            string query = "exec getTaiKhoan @username, @password";

            DataTable dt = dp.ExecuteQuery(query, new object[] { username, password });

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];

                return new TaiKhoanDTO
                {
                    MaNV = row["MaNV"] == DBNull.Value ? 0 : Convert.ToInt32(row["MaNV"]),
                    TenDangNhap = row["TenDangNhap"]?.ToString() ?? "",
                    Quyen = row["Quyen"]?.ToString() ?? "User"
                };
            }
            return null;
        }
    }
}
