using System;
using System.ComponentModel;

namespace DTO
{
    public class NhanVienDTO
    {
        [DisplayName("Mã nhân viên")]
        public int MaNV { get; set; }
        [DisplayName("Tên nhân viên")]
        public string TenNV { get; set; }
        [DisplayName("Giới tính")]
        public string GioiTinh { get; set; }
        [DisplayName("Ngày sinh")]
        public DateTime NgaySinh { get; set; }
        [DisplayName("Địa chỉ")]
        public string DiaChi { get; set; }

        public NhanVienDTO() { }

        public NhanVienDTO(int maNV, string tenNV, string gioiTinh, DateTime ngaySinh, string diaChi)
        {
            MaNV = maNV;
            TenNV = tenNV;
            GioiTinh = gioiTinh;
            NgaySinh = ngaySinh;
            DiaChi = diaChi;
        }
    }
}

