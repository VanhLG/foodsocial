CREATE DATABASE DB_G09FOOD
GO
drop database DB_G09FOOD
USE DB_G09FOOD
GO

-- Tạo bảng NguoiDung
CREATE TABLE NguoiDung (
    MaNguoiDung INT PRIMARY KEY IDENTITY(1,1),
    TenNguoiDung NVARCHAR(100) UNIQUE NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    MatKhau NVARCHAR(256) NOT NULL,
    AnhDaiDien NVARCHAR(MAX),
    TieuSu NVARCHAR(500),
    NgayTao DATETIME DEFAULT GETDATE()
);

-- Tạo bảng LoaiMonAn
CREATE TABLE LoaiMonAn (
    MaLoaiMonAn INT PRIMARY KEY IDENTITY(1,1),
    TenLoaiMonAn NVARCHAR(100) UNIQUE NOT NULL
);

-- Tạo bảng BaiViet
CREATE TABLE BaiViet (
    MaBaiViet INT PRIMARY KEY IDENTITY(1,1),
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NoiDung NVARCHAR(MAX) NOT NULL,
    AnhBaiViet NVARCHAR(MAX),
    MaLoaiMonAn INT FOREIGN KEY REFERENCES LoaiMonAn(MaLoaiMonAn),
    NgayTao DATETIME DEFAULT GETDATE()
);
ALTER TABLE BaiViet
ADD luotthich INT DEFAULT 0;



-- Tạo bảng BinhLuan
CREATE TABLE BinhLuan (
    MaBinhLuan INT PRIMARY KEY IDENTITY(1,1),
    MaBaiViet INT FOREIGN KEY REFERENCES BaiViet(MaBaiViet),
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NoiDung NVARCHAR(MAX) NOT NULL,
    NgayTao DATETIME DEFAULT GETDATE()
);

-- Tạo bảng Thich
CREATE TABLE Thich (
    MaThich INT PRIMARY KEY IDENTITY(1,1),
    MaBaiViet INT FOREIGN KEY REFERENCES BaiViet(MaBaiViet),
    MaNguoiDung INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
);

insert into Thich
values (1,2),(2,1),(2,2),(2,3)
delete Thich
select * from Thich
select * from BaiViet
select * from BinhLuan
-- Tạo bảng TheoDoi
CREATE TABLE TheoDoi (
    MaTheoDoi INT PRIMARY KEY IDENTITY(1,1),
    MaNguoiTheoDoi INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    MaNguoiDuocTheoDoi INT FOREIGN KEY REFERENCES NguoiDung(MaNguoiDung),
    NgayTao DATETIME DEFAULT GETDATE(),
    UNIQUE(MaNguoiTheoDoi, MaNguoiDuocTheoDoi)
);

INSERT INTO NguoiDung (TenNguoiDung, Email, MatKhau, AnhDaiDien, TieuSu)
VALUES 
    (N'tramy', N'tramy@gmail.com', 'tramy123', '/User/img/avatar2.jpg', N'2010A02'),
    (N'anhphu', N'anhphu@gmail.com', 'anhphu123', '/User/img/avatar1.jpg', N'2110A02'),
    (N'trongninh', N'trongninh@gmail.com', 'trongninh123', '/User/img/avatar3.jpg', N'2110A03');

SELECT * FROM NguoiDung

INSERT INTO LoaiMonAn (TenLoaiMonAn)
VALUES 
    (N'Quán Cà Phê'),
    (N'Quán Trà Sữa'),
    (N'Nhà Hàng Hàn Quốc'),
    (N'Nhà Hàng Trung Quốc'),
    (N'Nhà Hàng Nhật Bản');

SELECT * FROM LoaiMonAn

INSERT INTO BaiViet (MaNguoiDung, NoiDung, AnhBaiViet, MaLoaiMonAn)
VALUES 
    (1, N'Tôi rất thích kimchi của Hàn Quốc, nó rất cay và ngon.','/Post/img/han.jpg', 3),
    (2, N'Sushi là món ăn tuyệt vời, đặc biệt là sushi cá hồi.', '/Post/img/nhat.jpg', 5),
    (3, N'Món vịt quay Bắc Kinh là món ăn nổi tiếng của Trung Quốc.', '/Post/img/trung.jpg', 4);

SELECT * FROM BaiViet

insert into Thich
values (1,3)
go
create proc prSoLuotLike @MaBaiViet int,@soLike int out
as
 begin
 select @soLike= count(*) from Thich where MaBaiViet= @MaBaiViet
 end
 go
 create proc prSoLuotLike2 @MaBaiViet int
as
 begin
 select count(*) from Thich where MaBaiViet= @MaBaiViet
 end


DECLARE @SoLuong INT;

EXEC dbo.prSoLuotLike @MaBaiViet = 2, @soLike = @SoLuong OUT;

SELECT @SoLuong

go
CREATE TRIGGER trgThichUpdateLuotThich
ON Thich
AFTER INSERT, DELETE
AS
BEGIN
    -- Kiểm tra khi có dữ liệu được thêm vào
    IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Tăng giá trị của luotthich khi có dữ liệu được thêm vào
        UPDATE BaiViet
        SET luotthich = luotthich + 1
        FROM BaiViet
        INNER JOIN inserted ON BaiViet.MaBaiViet = inserted.MaBaiViet;
    END;

    -- Kiểm tra khi có dữ liệu bị xóa đi
    IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Giảm giá trị của luotthich khi có dữ liệu bị xóa đi
        UPDATE BaiViet
        SET luotthich = luotthich - 1
        FROM BaiViet
        INNER JOIN deleted ON BaiViet.MaBaiViet = deleted.MaBaiViet;

        -- Đảm bảo luotthich không bị âm
        UPDATE BaiViet
        SET luotthich = 0
        WHERE luotthich < 0;
    END;
END;
go
CREATE PROCEDURE sp_InsertThich
    @MaBaiViet INT,
    @MaNguoiDung INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Thich (MaBaiViet, MaNguoiDung)
    VALUES (@MaBaiViet, @MaNguoiDung);
END;

exec sp_InsertThich 3,3
drop trigger trgThichUpdateLuotThich