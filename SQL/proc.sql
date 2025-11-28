-- Phong

---------------------------

CREATE PROCEDURE findPhong
    @keyword NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.MaPhong, 
        p.TenPhong, 
        p.MaLoaiPhong,
        ISNULL(lp.TenLoai, '') AS Loai,
        p.TrangThai
    FROM Phong p
    LEFT JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
    WHERE CAST(p.MaPhong AS NVARCHAR(50)) LIKE '%' + @keyword + '%'
       OR p.TenPhong LIKE '%' + @keyword + '%'
       OR lp.TenLoai LIKE '%' + @keyword + '%';
END
GO

---------------------------

CREATE PROCEDURE getPhong
    @maPhong INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.MaPhong,
        p.TenPhong,
        p.MaLoaiPhong,
        ISNULL(lp.TenLoai, '') AS Loai,
        p.TrangThai
    FROM Phong p
    LEFT JOIN LoaiPhong lp 
        ON p.MaLoaiPhong = lp.MaLoaiPhong
    WHERE p.MaPhong = @maPhong;
END
GO

---------------------------

CREATE PROCEDURE getListPhong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.MaPhong,
        p.TenPhong,
        p.MaLoaiPhong,
        ISNULL(lp.TenLoai, '') AS Loai,
        p.TrangThai
    FROM Phong p
    LEFT JOIN LoaiPhong lp 
        ON p.MaLoaiPhong = lp.MaLoaiPhong;
END
GO

-- BaoCaoDoanhThu

CREATE PROCEDURE getListBaoCao
    @startDate DATE,
    @endDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaHD, MaDatPhong, NgayLap, TongTien
    FROM HoaDon
    WHERE CAST(NgayLap AS DATE) >= @startDate
      AND CAST(NgayLap AS DATE) <= @endDate
    ORDER BY NgayLap;
END
GO

---------------------------

CREATE PROCEDURE TinhTongDoanhThu
    @month INT,
    @year INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @startDate DATE = DATEFROMPARTS(@year, @month, 1);
    DECLARE @endDate DATE = EOMONTH(@startDate);

    SELECT ISNULL(SUM(TongTien), 0) AS TongDoanhThu
    FROM HoaDon
    WHERE CAST(NgayLap AS DATE) >= @startDate
      AND CAST(NgayLap AS DATE) <= @endDate;
END
GO

---------------------------

CREATE PROCEDURE getBaoCaoDoanhThu
    @tuNgay DATE,
    @denNgay DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaHD, MaDatPhong, NgayLap, TongTien
    FROM HoaDon
    WHERE CAST(NgayLap AS DATE) >= @tuNgay
      AND CAST(NgayLap AS DATE) <= @denNgay
    ORDER BY NgayLap;
END
GO

---------------------------

CREATE PROCEDURE GetBaoCaoTheoThang
    @year INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MONTH(NgayLap) AS Thang, 
           ISNULL(SUM(TongTien), 0) AS DoanhThu
    FROM HoaDon
    WHERE YEAR(NgayLap) = @year
    GROUP BY MONTH(NgayLap)
    ORDER BY MONTH(NgayLap);
END
GO

-- DatPhong

CREATE OR ALTER PROCEDURE getListDatPhong
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dp.MaDatPhong,
           dp.MaKH,          
           kh.TenKH, 
           dp.MaNV,          
           nv.TenNV,
           dp.MaPhong,        
           p.TenPhong, 
           dp.NgayDat, 
           dp.NgayDen, 
           dp.NgayDi, 
           hd.TongTien
    FROM DatPhong dp
    JOIN KhachHang kh ON dp.MaKH = kh.MaKH
    JOIN NhanVien nv ON dp.MaNV = nv.MaNV
    JOIN Phong p ON dp.MaPhong = p.MaPhong
    JOIN HoaDon hd ON dp.MaDatPhong = hd.MaDatPhong;
END
GO

---------------------------

CREATE PROCEDURE getListPhongTrong
AS
BEGIN
    SET NOCOUNT ON;

    SELECT p.MaPhong, 
           p.TenPhong, 
           p.MaLoaiPhong, 
           p.TrangThai, 
           lp.GiaPhong
    FROM Phong p
    JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
    WHERE p.TrangThai = N'Trống';
END
GO

---------------------------

CREATE PROCEDURE findDatPhong
    @keyword NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT dp.MaDatPhong, 
           kh.TenKH, 
           nv.TenNV, 
           p.TenPhong, 
           dp.NgayDat, 
           dp.NgayDen, 
           dp.NgayDi, 
           ISNULL(hd.TongTien, 0) AS TongTien
    FROM DatPhong dp
    JOIN KhachHang kh ON dp.MaKH = kh.MaKH
    JOIN NhanVien nv ON dp.MaNV = nv.MaNV
    JOIN Phong p ON dp.MaPhong = p.MaPhong
    LEFT JOIN HoaDon hd ON dp.MaDatPhong = hd.MaDatPhong
    WHERE kh.TenKH LIKE N'%' + @keyword + '%' 
       OR nv.TenNV LIKE N'%' + @keyword + '%' 
       OR p.TenPhong LIKE N'%' + @keyword + '%'
    ORDER BY dp.NgayDat DESC;
END
GO

---------------------------

CREATE PROCEDURE findDatPhongTheoMa
    @maDatPhong INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT dp.MaDatPhong, 
           kh.TenKH, 
           nv.TenNV, 
           p.TenPhong, 
           dp.NgayDat, 
           dp.NgayDen, 
           dp.NgayDi, 
           ISNULL(hd.TongTien, 0) AS TongTien
    FROM DatPhong dp
    JOIN KhachHang kh ON dp.MaKH = kh.MaKH
    JOIN NhanVien nv ON dp.MaNV = nv.MaNV
    JOIN Phong p ON dp.MaPhong = p.MaPhong
    LEFT JOIN HoaDon hd ON dp.MaDatPhong = hd.MaDatPhong
    WHERE dp.MaDatPhong = @maDatPhong
    ORDER BY dp.NgayDat DESC;
END
GO

---------------------------

CREATE PROCEDURE getDonGia
    @MaPhong INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT lp.GiaPhong
    FROM Phong p
    JOIN LoaiPhong lp ON p.MaLoaiPhong = lp.MaLoaiPhong
    WHERE p.MaPhong = @MaPhong;
END
GO

-- HoaDon

CREATE PROCEDURE findHoaDon
    @Keyword NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM HoaDon
    WHERE CAST(MaHD AS NVARCHAR) LIKE N'%' + @Keyword + '%'
       OR CAST(MaDatPhong AS NVARCHAR) LIKE N'%' + @Keyword + '%';
END
GO

-- TaiKhoan

CREATE PROCEDURE getTaiKhoan
    @Username NVARCHAR(50),
    @Password NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT MaNV, TenDangNhap, Quyen
    FROM TaiKhoan
    WHERE TenDangNhap = @Username
      AND MatKhau = @Password;
END
GO





