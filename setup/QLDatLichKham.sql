\xEF\xBB\xBF/* =====================================================================
   DO AN CHUYEN DE ASP.NET - Website dat lich hen kham benh ngoai gio
   Sinh vien : Tong Duy Khanh - MSSV 470123159 - Lop VX23TTK12
   CSDL      : QLDatLichKham (SQL Server)
   Cach chay : Mo bang SSMS -> New Query -> dan toan bo -> Execute (F5)
   ===================================================================== */

USE master;
GO
IF DB_ID(N'QLDatLichKham') IS NOT NULL
BEGIN
    ALTER DATABASE QLDatLichKham SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLDatLichKham;
END
GO
CREATE DATABASE QLDatLichKham;
GO
USE QLDatLichKham;
GO

/* ---------------------------- 1. TAO BANG ---------------------------- */

CREATE TABLE VaiTro (
    MaVaiTro   INT IDENTITY(1,1) CONSTRAINT PK_VaiTro PRIMARY KEY,
    TenVaiTro  NVARCHAR(50) NOT NULL CONSTRAINT UQ_VaiTro_Ten UNIQUE
);

CREATE TABLE TaiKhoan (
    MaTK         INT IDENTITY(1,1) CONSTRAINT PK_TaiKhoan PRIMARY KEY,
    TenDangNhap  VARCHAR(50)  NOT NULL CONSTRAINT UQ_TaiKhoan_TenDN UNIQUE,
    MatKhauHash  VARCHAR(255) NOT NULL,
    MaVaiTro     INT NOT NULL CONSTRAINT FK_TaiKhoan_VaiTro REFERENCES VaiTro(MaVaiTro),
    TrangThai    BIT NOT NULL CONSTRAINT DF_TaiKhoan_TrangThai DEFAULT 1
);

CREATE TABLE BenhNhan (
    MaBN      INT IDENTITY(1,1) CONSTRAINT PK_BenhNhan PRIMARY KEY,
    MaTK      INT NOT NULL CONSTRAINT UQ_BenhNhan_MaTK UNIQUE
              CONSTRAINT FK_BenhNhan_TaiKhoan REFERENCES TaiKhoan(MaTK),
    HoTen     NVARCHAR(100) NOT NULL,
    NgaySinh  DATE NULL CONSTRAINT CK_BenhNhan_NgaySinh CHECK (NgaySinh <= GETDATE()),
    GioiTinh  NVARCHAR(5) NULL CONSTRAINT CK_BenhNhan_GioiTinh CHECK (GioiTinh IN (N'Nam', N'Nữ')),
    SDT       VARCHAR(15) NOT NULL CONSTRAINT CK_BenhNhan_SDT CHECK (SDT NOT LIKE '%[^0-9]%'),
    DiaChi    NVARCHAR(200) NULL
);

CREATE TABLE ChuyenKhoa (
    MaCK   INT IDENTITY(1,1) CONSTRAINT PK_ChuyenKhoa PRIMARY KEY,
    TenCK  NVARCHAR(100) NOT NULL CONSTRAINT UQ_ChuyenKhoa_Ten UNIQUE,
    MoTa   NVARCHAR(255) NULL
);

CREATE TABLE BacSi (
    MaBS       INT IDENTITY(1,1) CONSTRAINT PK_BacSi PRIMARY KEY,
    HoTen      NVARCHAR(100) NOT NULL,
    MaCK       INT NOT NULL CONSTRAINT FK_BacSi_ChuyenKhoa REFERENCES ChuyenKhoa(MaCK),
    HocVi      NVARCHAR(50) NULL,
    SDT        VARCHAR(15) NULL,
    TrangThai  BIT NOT NULL CONSTRAINT DF_BacSi_TrangThai DEFAULT 1
);

CREATE TABLE KhungGio (
    MaKG         INT IDENTITY(1,1) CONSTRAINT PK_KhungGio PRIMARY KEY,
    MaBS         INT NOT NULL CONSTRAINT FK_KhungGio_BacSi REFERENCES BacSi(MaBS),
    NgayKham     DATE NOT NULL,
    GioBatDau    TIME(0) NOT NULL,
    GioKetThuc   TIME(0) NOT NULL,
    CONSTRAINT CK_KhungGio_Gio CHECK (GioKetThuc > GioBatDau),
    CONSTRAINT UQ_KhungGio UNIQUE (MaBS, NgayKham, GioBatDau)
);

CREATE TABLE LichHen (
    MaLH      INT IDENTITY(1,1) CONSTRAINT PK_LichHen PRIMARY KEY,
    MaBN      INT NOT NULL CONSTRAINT FK_LichHen_BenhNhan REFERENCES BenhNhan(MaBN),
    MaKG      INT NOT NULL CONSTRAINT FK_LichHen_KhungGio REFERENCES KhungGio(MaKG),
    LyDoKham  NVARCHAR(255) NULL,
    TrangThai NVARCHAR(20) NOT NULL CONSTRAINT DF_LichHen_TrangThai DEFAULT N'Chờ xác nhận',
    NgayTao   DATETIME NOT NULL CONSTRAINT DF_LichHen_NgayTao DEFAULT GETDATE(),
    CONSTRAINT CK_LichHen_TrangThai
        CHECK (TrangThai IN (N'Chờ xác nhận', N'Đã xác nhận', N'Đã khám', N'Đã hủy'))
);
GO

/* Chong trung lich: moi khung gio chi co 1 lich dang hieu luc.
   Lich da huy khong tinh, nen khung gio do co the dat lai. */
CREATE UNIQUE INDEX UX_LichHen_KhungGio
    ON LichHen(MaKG) WHERE TrangThai <> N'Đã hủy';
GO

/* Chi muc ho tro tim kiem */
CREATE INDEX IX_BenhNhan_HoTen ON BenhNhan(HoTen);
CREATE INDEX IX_BacSi_HoTen    ON BacSi(HoTen);
CREATE INDEX IX_LichHen_MaBN   ON LichHen(MaBN);
GO

/* ------------------------- 2. DU LIEU MAU ---------------------------- */
/* Mat khau mau: 123456, luu dang SHA-256 (chuoi hex in hoa).
   Trong ung dung ASP.NET phai bam mat khau cung thuat toan nay
   (hoac doi sang BCrypt thi cap nhat lai cot MatKhauHash). */

INSERT INTO VaiTro (TenVaiTro) VALUES (N'Admin'), (N'LeTan'), (N'BenhNhan');

DECLARE @h VARCHAR(64) = CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2);
INSERT INTO TaiKhoan (TenDangNhap, MatKhauHash, MaVaiTro) VALUES
 ('admin',   @h, 1),
 ('letan01', @h, 2),
 ('bn01',    @h, 3),
 ('bn02',    @h, 3),
 ('bn03',    @h, 3);

INSERT INTO BenhNhan (MaTK, HoTen, NgaySinh, GioiTinh, SDT, DiaChi) VALUES
 (3, N'Nguyễn Văn An',  '1990-05-12', N'Nam', '0901234567', N'Ninh Chử, Khánh Hòa'),
 (4, N'Trần Thị Bình',  '1985-11-03', N'Nữ',  '0912345678', N'Phan Rang, Khánh Hòa'),
 (5, N'Lê Hoàng Cường', '2001-02-20', N'Nam', '0923456789', N'Cam Ranh, Khánh Hòa');

INSERT INTO ChuyenKhoa (TenCK, MoTa) VALUES
 (N'Nội tổng quát',  N'Khám và điều trị các bệnh nội khoa thường gặp'),
 (N'Nhi khoa',       N'Khám và chăm sóc sức khỏe trẻ em'),
 (N'Tai Mũi Họng',   N'Khám các bệnh lý tai, mũi, họng'),
 (N'Da liễu',        N'Khám và điều trị các bệnh về da');

INSERT INTO BacSi (HoTen, MaCK, HocVi, SDT) VALUES
 (N'Phạm Minh Tuấn',  1, N'Thạc sĩ, Bác sĩ',  '0934000001'),
 (N'Nguyễn Thị Hoa',  2, N'Bác sĩ CKI',       '0934000002'),
 (N'Võ Quốc Bảo',     3, N'Bác sĩ',           '0934000003'),
 (N'Đặng Thu Hà',     4, N'Bác sĩ CKI',       '0934000004'),
 (N'Lê Văn Sơn',      1, N'Bác sĩ',           '0934000005');

/* Khung gio ngoai gio: 17:00-21:00 (moi khung 30 phut) cho 7 ngay toi.
   Moi bac si trong 3 khung dau tien cua moi ngay de tao du lieu mau. */
DECLARE @d INT = 1;
WHILE @d <= 7
BEGIN
    DECLARE @ngay DATE = DATEADD(DAY, @d, CAST(GETDATE() AS DATE));
    INSERT INTO KhungGio (MaBS, NgayKham, GioBatDau, GioKetThuc)
    SELECT b.MaBS, @ngay, g.GioBD, g.GioKT
    FROM BacSi b
    CROSS JOIN (VALUES
        (CAST('17:00' AS TIME(0)), CAST('17:30' AS TIME(0))),
        (CAST('17:30' AS TIME(0)), CAST('18:00' AS TIME(0))),
        (CAST('18:00' AS TIME(0)), CAST('18:30' AS TIME(0))),
        (CAST('18:30' AS TIME(0)), CAST('19:00' AS TIME(0)))
    ) AS g(GioBD, GioKT);
    SET @d += 1;
END
GO

INSERT INTO LichHen (MaBN, MaKG, LyDoKham, TrangThai) VALUES
 (1, 1,  N'Đau đầu, mất ngủ kéo dài',  N'Đã xác nhận'),
 (2, 6,  N'Khám sức khỏe tổng quát',    N'Chờ xác nhận'),
 (3, 11, N'Viêm họng, ho nhiều ngày',   N'Chờ xác nhận'),
 (1, 16, N'Tái khám',                   N'Đã hủy');
GO

/* ----------------- 3. VIEW / THU TUC HO TRO CHUC NANG ----------------- */

/* View: chi tiet lich hen (dung cho man hinh quan ly va tra cuu) */
CREATE VIEW vw_LichHenChiTiet AS
SELECT lh.MaLH, bn.MaBN, bn.HoTen AS TenBenhNhan, bn.SDT,
       bs.MaBS, bs.HoTen AS TenBacSi, ck.TenCK,
       kg.NgayKham, kg.GioBatDau, kg.GioKetThuc,
       lh.LyDoKham, lh.TrangThai, lh.NgayTao
FROM LichHen lh
JOIN BenhNhan  bn ON bn.MaBN = lh.MaBN
JOIN KhungGio  kg ON kg.MaKG = lh.MaKG
JOIN BacSi     bs ON bs.MaBS = kg.MaBS
JOIN ChuyenKhoa ck ON ck.MaCK = bs.MaCK;
GO

/* Thu tuc: dat lich (kiem tra nghiep vu) */
CREATE PROCEDURE sp_DatLich
    @MaBN INT, @MaKG INT, @LyDoKham NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @gioKham DATETIME;
    SELECT @gioKham = CAST(NgayKham AS DATETIME) + CAST(GioBatDau AS DATETIME)
    FROM KhungGio WHERE MaKG = @MaKG;

    IF @gioKham IS NULL
        THROW 50001, N'Khung giờ không tồn tại.', 1;
    IF @gioKham <= GETDATE()
        THROW 50002, N'Không thể đặt lịch cho thời điểm đã qua.', 1;
    IF EXISTS (SELECT 1 FROM LichHen WHERE MaKG = @MaKG AND TrangThai <> N'Đã hủy')
        THROW 50003, N'Khung giờ này đã có người đặt.', 1;

    INSERT INTO LichHen (MaBN, MaKG, LyDoKham) VALUES (@MaBN, @MaKG, @LyDoKham);
    SELECT SCOPE_IDENTITY() AS MaLH;
END
GO

/* Thu tuc: huy lich (truoc gio kham toi thieu 2 gio) */
CREATE PROCEDURE sp_HuyLich
    @MaLH INT, @MaBN INT
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @gioKham DATETIME;
    SELECT @gioKham = CAST(kg.NgayKham AS DATETIME) + CAST(kg.GioBatDau AS DATETIME)
    FROM LichHen lh JOIN KhungGio kg ON kg.MaKG = lh.MaKG
    WHERE lh.MaLH = @MaLH AND lh.MaBN = @MaBN
      AND lh.TrangThai IN (N'Chờ xác nhận', N'Đã xác nhận');

    IF @gioKham IS NULL
        THROW 50004, N'Không tìm thấy lịch hẹn có thể hủy.', 1;
    IF DATEDIFF(MINUTE, GETDATE(), @gioKham) < 120
        THROW 50005, N'Chỉ được hủy lịch trước giờ khám tối thiểu 2 giờ.', 1;

    UPDATE LichHen SET TrangThai = N'Đã hủy' WHERE MaLH = @MaLH;
END
GO

/* Thu tuc: tim kiem tuong doi (LIKE %tu khoa%) */
CREATE PROCEDURE sp_TimLichHen
    @TuKhoa NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT * FROM vw_LichHenChiTiet
    WHERE TenBenhNhan LIKE N'%' + @TuKhoa + N'%'
       OR TenBacSi    LIKE N'%' + @TuKhoa + N'%'
       OR SDT         LIKE N'%' + @TuKhoa + N'%'
       OR CAST(MaLH AS NVARCHAR(20)) = @TuKhoa
    ORDER BY NgayKham, GioBatDau;
END
GO

/* Thu tuc: thong ke so lich hen theo thang cua mot nam */
CREATE PROCEDURE sp_ThongKeTheoThang
    @Nam INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT MONTH(NgayKham) AS Thang,
           COUNT(*) AS TongLich,
           SUM(CASE WHEN TrangThai = N'Đã khám' THEN 1 ELSE 0 END) AS DaKham,
           SUM(CASE WHEN TrangThai = N'Đã hủy'  THEN 1 ELSE 0 END) AS DaHuy
    FROM vw_LichHenChiTiet
    WHERE YEAR(NgayKham) = @Nam
    GROUP BY MONTH(NgayKham)
    ORDER BY Thang;
END
GO

/* View: thong ke theo bac si va chuyen khoa */
CREATE VIEW vw_ThongKeBacSi AS
SELECT bs.MaBS, bs.HoTen AS TenBacSi, ck.TenCK,
       COUNT(lh.MaLH) AS SoLichHen
FROM BacSi bs
JOIN ChuyenKhoa ck ON ck.MaCK = bs.MaCK
LEFT JOIN KhungGio kg ON kg.MaBS = bs.MaBS
LEFT JOIN LichHen lh ON lh.MaKG = kg.MaKG AND lh.TrangThai <> N'Đã hủy'
GROUP BY bs.MaBS, bs.HoTen, ck.TenCK;
GO

/* ------------------------- 4. KIEM TRA NHANH -------------------------- */
SELECT N'VaiTro' AS Bang, COUNT(*) AS SoDong FROM VaiTro UNION ALL
SELECT N'TaiKhoan',   COUNT(*) FROM TaiKhoan   UNION ALL
SELECT N'BenhNhan',   COUNT(*) FROM BenhNhan   UNION ALL
SELECT N'ChuyenKhoa', COUNT(*) FROM ChuyenKhoa UNION ALL
SELECT N'BacSi',      COUNT(*) FROM BacSi      UNION ALL
SELECT N'KhungGio',   COUNT(*) FROM KhungGio   UNION ALL
SELECT N'LichHen',    COUNT(*) FROM LichHen;

EXEC sp_TimLichHen N'An';
EXEC sp_ThongKeTheoThang 2026;
GO
