using System;
using System.Collections.Generic;
using System.Data;
using DTO;

namespace DAL
{
    public class NhanVienDAL
    {
        public List<NhanVienDTO> LayDanhSachNhanVien()
        {
            string query = "SELECT * FROM NHANVIEN";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            List<NhanVienDTO> list = new List<NhanVienDTO>();

            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nv = new NhanVienDTO(
                    Convert.ToInt32(row["MaNV"]),
                    row["TenNV"].ToString(),
                    row["GioiTinh"].ToString(),
                    Convert.ToDateTime(row["NgaySinh"]),
                    row["DiaChi"].ToString()
                );
                list.Add(nv);
            }
            return list;
        }

        public bool ThemNhanVien(NhanVienDTO nv)
        {
            string query = "INSERT INTO NHANVIEN (TenNV, GioiTinh, NgaySinh, DiaChi) VALUES (@ten, @gioitinh, @ngaysinh, @diachi)";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { nv.TenNV, nv.GioiTinh, nv.NgaySinh, nv.DiaChi });
            return result > 0;
        }

        public bool CapNhatNhanVien(NhanVienDTO nv)
        {
            string query = "UPDATE NHANVIEN SET TenNV=@ten, GioiTinh=@gioitinh, NgaySinh=@ngaysinh, DiaChi=@diachi WHERE MaNV=@manv";
            int result = DataProvider.Instance.ExecuteNonQuery(query,
                new object[] { nv.TenNV, nv.GioiTinh, nv.NgaySinh, nv.DiaChi, nv.MaNV });
            return result > 0;
        }

        public bool XoaNhanVien(int maNV)
        {
            string query = "DELETE FROM NHANVIEN WHERE MaNV=@manv";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { maNV });
            return result > 0;
        }

        public List<NhanVienDTO> TimKiemNhanVien(string keyword)
        {
            string query = "SELECT * FROM NHANVIEN WHERE MaNV LIKE '%' + @keyword + '%'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { keyword });
            List<NhanVienDTO> list = new List<NhanVienDTO>();

            foreach (DataRow row in data.Rows)
            {
                NhanVienDTO nv = new NhanVienDTO(
                    Convert.ToInt32(row["MaNV"]),
                    row["TenNV"].ToString(),
                    row["GioiTinh"].ToString(),
                    Convert.ToDateTime(row["NgaySinh"]),
                    row["DiaChi"].ToString()
                );
                list.Add(nv);
            }
            return list;
        }
    }
}

