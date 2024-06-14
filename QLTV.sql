

CREATE DATABASE QLTV_BETA
GO
USE QLTV_BETA
GO
SET DATEFORMAT DMY -- dateformat: dd/mm/yyyy
GO

CREATE TABLE DOCGIA 
(
	MaDG		VARCHAR(5) NOT NULL PRIMARY KEY,
	HoTen		NVARCHAR(30),
	GioiTinh	NVARCHAR(3),
	NgaySinh	DATE,
	DiaChi		NVARCHAR(255),
	Email		VARCHAR(255) NOT NULL,
	SoDT		VARCHAR(10) NOT NULL,
	NgayLapThe	DATE,
	IsDeleted	BIT DEFAULT 0
);
GO
CREATE TABLE THELOAI
(
	TenTheLoai	NVARCHAR(225) PRIMARY KEY,
	IsDeleted	BIT DEFAULT 0
);
GO
CREATE TABLE SACH 
(
	MaSach		VARCHAR(5) NOT NULL PRIMARY KEY,
	TenSach		NVARCHAR(255),
	TenTheLoai	NVARCHAR(225) FOREIGN KEY REFERENCES THELOAI(TenTheLoai),
	TacGia		NVARCHAR(255),
	NamXB		SMALLINT,
	NhaXB		NVARCHAR(255),
	NgayNhap	DATE,
	TriGia		INT,
	TinhTrang	SMALLINT DEFAULT 1,
	IsDeleted	BIT DEFAULT 0
);
GO


CREATE TABLE PHIEUMUON
(
	MaPhMuon	VARCHAR(5) NOT NULL PRIMARY KEY,
	MaDG		VARCHAR(5) FOREIGN KEY REFERENCES DOCGIA(MaDG),
	MaSach		VARCHAR(5) FOREIGN KEY REFERENCES SACH(MaSach),
	NgayMuon	DATE,
	NgayPhTra	DATE,
	IsDeleted	BIT DEFAULT 0
);
GO

CREATE TABLE PHIEUTRA
(
	MaPhTra		VARCHAR(5) NOT NULL PRIMARY KEY,
	MaPhMuon	VARCHAR(5) FOREIGN KEY REFERENCES PHIEUMUON(MaPhMuon),
	NgayTra		DATE,
	IsDeleted	BIT DEFAULT 0
);
GO

CREATE TABLE PHIEUTHU
(
	--Phiếu thu chỉ được tạo khi sách trả quá hạn, tạo trigger để kiểm tra việc trả quá hạn và tự lập phiếu thu nếu có. Việc truy xuất phiếu thu sẽ được thực hiện thông qua MaPhTra trong bảng PHIEUTHU
	ID			INT IDENTITY(1, 1) PRIMARY KEY,
	MaPhTra		VARCHAR(5) FOREIGN KEY REFERENCES PHIEUTRA(MaPhTra),
	SoNgayQHan	SMALLINT,
	SoTienThu	INT,
	IsDeleted	BIT DEFAULT 0
);
GO

CREATE TABLE THUTHU
(
	MaTT		VARCHAR(5) PRIMARY KEY,
	HoTen		NVARCHAR(30),
	GioiTinh	NVARCHAR(3),
	NgayVLam	DATE,
	NgaySinh	DATE,
	DiaChi		NVARCHAR(255),
	Email		VARCHAR(255) NOT NULL,
	SoDT		VARCHAR(10) NOT NULL,
	IsDeleted	BIT DEFAULT 0
)
GO

CREATE TABLE ACCOUNT
(
	TaiKhoan	VARCHAR(30) PRIMARY KEY,
	MatKhau		VARCHAR(255) NOT NULL,
	-- Để phục vụ cho việc thay đổi tên tài khoản, mật khẩu, không nên để tài khoản và mật khẩu làm thuộc tính khóa chính
	MaDG		VARCHAR(5) FOREIGN KEY REFERENCES DOCGIA(MaDG),
	MaTT		VARCHAR(5) FOREIGN KEY REFERENCES THUTHU(MaTT),
	VaiTro		SMALLINT NOT NULL, -- biến này để phân biệt người dùng, thủ thư hay admin, bởi vì mình chỉ sử dụng một bảng này để lưu thông tin tài khoản cho tất cả nên cần có biến này. Mình có thể xét như sau: 3: Admin, 2: thủ thư, và 1: người dùng.
	IsDeleted	BIT DEFAULT 0
);
GO

CREATE TABLE SETTING
(
	ID					INT PRIMARY KEY,
	TuoiToiTieuDocGia	INT	DEFAULT 13,
	TuoiToiDaDocGia		INT	DEFAULT 70,
	TuoiToiTieuThuThu	INT	DEFAULT 13,
	TuoiToiDaThuThu		INT	DEFAULT 70,
	ThoiHanThe			INT DEFAULT 24,
	SoNgayMuonToiDa		INT DEFAULT 30,
	SoTienNopTre		INT DEFAULT 2000,
	SoSachMuonToiDa		INT	DEFAULT 5,
	SoLuongTheLoaiToiDa INT DEFAULT 30,
	ThoiGianNhapSach	INT DEFAULT 5
);
GO



CREATE TABLE PARAMETERS
(
	ID 			INT PRIMARY KEY,
	IDDocGia	SMALLINT DEFAULT 6,
	IDSach		SMALLINT DEFAULT 8,
	IDPhieuMuon	SMALLINT DEFAULT 11,
	IDPhieuTra	SMALLINT DEFAULT 9,
	IDMaTT		SMALLINT DEFAULT 3
)
GO

-- Luôn cần một dòng dữ liệu trong bảng PARAMETERs để truy xuất thông tin sau này
INSERT INTO PARAMETERS VALUES(1, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT);
GO

-- Tạo TRIGGER để tính ngày phải trả của cuốn sách
CREATE TRIGGER NGAY_PHAI_TRA_PHIEUMUON
ON PHIEUMUON
FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @MaPhMuon VARCHAR(5), @MAXDAY INT
	
	SELECT @MaPhMuon = MaPhMuon FROM INSERTED

	-- Lấy giá trị SoNgayMuonToiDa từ bảng SETTING
	SELECT @MAXDAY = SoNgayMuonToiDa FROM SETTING

	UPDATE PHIEUMUON
	SET NgayPhTra = DATEADD(day, @MAXDAY, NgayMuon)
	WHERE PHIEUMUON.MaPhMuon = @MaPhMuon

	PRINT('Thông tin phiếu mượn đã được hoàn tất!')
END;
GO

-- Tạo TRIGGER kiểm tra có trả sách quá hạn không, nếu có thì tạo phiếu thu trong bảng PHIEUTHU tương ứng với phiếu mượn, phiếu trả đó
CREATE TRIGGER KIEM_TRA_QUA_HAN_PHIEUTRA
ON PHIEUTRA
FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @NgayPhTra DATE, @NgayTra DATE, @SoTienNopTre INT

	SELECT @NgayPhTra = NgayPhTra
	FROM INSERTED INNER JOIN PHIEUMUON ON INSERTED.MaPhMuon = PHIEUMUON.MaPhMuon
	SELECT @NgayTra = NgayTra FROM INSERTED

	IF (@NgayTra > @NgayPhTra)
	BEGIN
		DECLARE @MaPhTra VARCHAR(5), @SoNgQuaHan INT

		SELECT @MaPhTra = MaPhTra FROM INSERTED
		SELECT @SoNgQuaHan = DATEDIFF(day, @NgayPhTra, @NgayTra)

		SELECT @SoTienNopTre = SoTienNopTre FROM SETTING

		INSERT INTO PHIEUTHU VALUES(@MaPhTra, @SoNgQuaHan, @SoNgQuaHan * @SoTienNopTre, DEFAULT);
	
		PRINT('Trả sách quá thời hạn, đã lập phiếu thu!')
	END
END;
GO

-- INSERT DATA: Dữ liệu để test các chức năng của app

-- DOCGIA TABLE
INSERT INTO DOCGIA VALUES ('DG001', N'Nguyễn Thanh Hưng', N'Nam', '2002-06-15', N'135 Nam Kỳ Khởi Nghĩa, Bến Nghé, Quận 1', 'hungnt@gmail.com', '0392511342', '2024-02-07', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG002', N'Trần Nguyễn Yến Nhi', N'Nữ', '2008-11-14', N'Đường Lê Lợi, phường Bến Thành, quận 1', 'nhitran08@gmail.com', '0867455258', '2024-03-07', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG003', N'Trần Lê Tuyết Mai', N'Nữ', '2004-10-14', N'số 2 Nguyễn Bỉnh Khiêm, Quận 1', 'lmaiq1@gmail.com', '0914567842', '2024-03-12', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG004', N'Lê Thị Ngọc Ánh', N'Nữ', '2004-12-02', N'số 2 Khu Him Lam, quận 7', 'anhngoc@gmail.com', '0392411748', '2024-04-14', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG005', N'Phan Minh Long', N'Nam', '2009-05-07', N'Số 3 Hòa Bình, phường 3, quận 11', 'longSia@gmail.com', '0974373212', '2024-05-07', DEFAULT);
GO

-- THELOAI TABLE
INSERT INTO THELOAI VALUES (N'Tin học', DEFAULT);
INSERT INTO THELOAI VALUES (N'Vật lý', DEFAULT);
INSERT INTO THELOAI VALUES (N'Toán', DEFAULT);
INSERT INTO THELOAI VALUES (N'Y học', DEFAULT);
INSERT INTO THELOAI VALUES (N'Văn học', DEFAULT);
INSERT INTO THELOAI VALUES (N'Lịch sử', DEFAULT);
GO

-- SACH TABLE
INSERT INTO SACH VALUES ('SS001', N'Giáo trình toán cao cấp', N'Toán', N'Nhóm tác giả từ UIT', 2015, N'NXB Đại học quốc gia HCM', '2020-06-12', 60000, DEFAULT, DEFAULT);
INSERT INTO SACH VALUES ('SS002', N'Giáo trình Hệ điều hành', N'Tin học', N'Nhóm tác giả từ UIT', 2015, N'NXB Đại học quốc gia HCM', '2020-05-12', 53000, DEFAULT, DEFAULT);
INSERT INTO SACH VALUES ('SS003', N'Toán học và ứng dụng', N'Toán', N'Nhóm tác giả từ viện toán học Việt Nam', 2014, N'Viện toán học Việt Nam', '2020-07-12', 72000, DEFAULT, DEFAULT);
INSERT INTO SACH VALUES ('SS004', N'Cẩm nang chăm sóc sức khỏe', N'Y học', N'Nguyễn Ngọc Hân, Đặng Huy Hoàng', 2014, N'NXB thông tin và truyền thông', '2020-05-12', 50000, DEFAULT, DEFAULT);
INSERT INTO SACH VALUES ('SS005', N'Ngôn ngữ lập trình C#', N'Tin học', N'Phan Văn Thúc', 2010, N'NXB Giáo dục', '2020-05-12', 54000, DEFAULT, DEFAULT);
INSERT INTO SACH VALUES ('SS006', N'Đại số tuyến tính', N'Toán', N'Phạm Công Ngô', 2015, N'NXB Đại học quốc gia HCM', '2020-06-12', 65000, DEFAULT, DEFAULT);
INSERT INTO SACH VALUES ('SS007', N'Vật lý đại cương', N'Vật lý', N'Trần Văn Lượng', 2014, N'NXB Đại học quốc gia HCM', '2020-06-12', 65000, DEFAULT, DEFAULT);
GO

-- PHIEUMUON TABLE
INSERT INTO PHIEUMUON VALUES ('PM001', 'DG001', 'SS001', '7/05/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM002', 'DG005', 'SS004', '7/05/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM003', 'DG005', 'SS003', '7/05/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM004', 'DG003', 'SS006', '15/04/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM005', 'DG002', 'SS004', '5/04/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM006', 'DG002', 'SS003', '5/04/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM007', 'DG004', 'SS005', '25/04/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM008', 'DG001', 'SS002', '14/05/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM009', 'DG001', 'SS006', '14/05/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES ('PM010', 'DG002', 'SS005', '7/04/2024', NULL, DEFAULT);
GO

-- PHIEUTRA TABLE
INSERT INTO PHIEUTRA VALUES ('PT001', 'PM003', '21/05/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT002', 'PM005', '22/5/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT003', 'PM007', '10/5/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT004', 'PM002', '15/05/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT005', 'PM001', '18/05/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT006', 'PM004', '15/05/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT007', 'PM006', '14/05/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES ('PT008', 'PM009', '20/05/2024', DEFAULT);
GO

-- THUTHU TABLE
INSERT INTO THUTHU VALUES ('TT001', N'Lương Ngọc Huyền', N'Nữ', '5/01/2024', '12/03/2003', N'Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', 'huyenlb@gmail.com', '0369442256', DEFAULT);
INSERT INTO THUTHU VALUES ('TT002', N'Cao Văn Thành', N'Nam', '12/01/2024', '22/05/2003', N'Đường Lê Duẩn, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 'thanhcv03@gmail.com', '0973464756', DEFAULT);
GO

-- ACOUNT TABLE
INSERT INTO ACCOUNT VALUES ('TaiKhoan1', 'MatKhau1', 'DG001', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan2', 'MatKhau2', 'DG002', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan3', 'MatKhau3', 'DG003', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan4', 'MatKhau4', 'DG004', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan5', 'MatKhau5', 'DG005', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan6', 'MatKhau6', NULL, 'TT001', 2, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan7', 'MatKhau7', NULL, 'TT002', 2, DEFAULT);

--SETTING TABLE
INSERT INTO SETTING VALUES (1, 13, 70, 13,70, 24, 30, 2000, 5, 30, 5)
