﻿/*
Mọi người có thể thêm dữ liệu vào file CSDL_Excel.xlsx (mỗi người một ít, chủ yếu tự nghĩ ra á) rồi sau đó sẽ thêm vào file sql sau.
Khi thêm dữ liệu thì mọi người lưu ý định dạng ngày tháng dd/mm/yyyy
Và lưu ý các mối quan hệ khóa ngoại liên kết giữa các bảng. Thuộc tính nào chưa thể thêm vào thì mọi người cứ đặt là NULL.
*/

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
	LoaiDG		VARCHAR(255),
	GioiTinh	NVARCHAR(3),
	NgaySinh	DATE,
	DiaChi		NVARCHAR(255),
	Email		VARCHAR(255) NOT NULL,
	SoDT		VARCHAR(10) NOT NULL,
	NgayLapThe	DATE,
	SoCCCD		VARCHAR(12) NOT NULL,
	--AnhDaiDien	VARCHAR(30), -- Nếu thấy không cần thì có thể bỏ qua thuộc tính này (thuộc tính này sẽ lưu đường dẫn ảnh đại diện của người dùng đặt trong thư mục Debug, khi nào xem thông tin cụ thể của một người dùng thì mình lấy ra)
	IsDeleted	BIT DEFAULT 0
	/* Các loại độc giả
	Độc giả sinh viên: Bao gồm các sinh viên đang học tại trường hoặc các cơ sở giáo dục. - LDOCGIA_A
	Độc giả học sinh: Bao gồm học sinh đang theo học tại các trường cấp 1, 2, 3. - LDOCGIA_B
	Cán bộ giáo viên: Bao gồm giáo viên, nhân viên trong ngành giáo dục. - LDOCGIA_C
	Người đã đi làm: Độc giả không thuộc các nhóm trên, đã đi làm và có nhu cầu sử dụng thư viện. - LDOCGIA_D
	Các đối tượng khác: Có thể bao gồm người nước ngoài, người nghiên cứu, người quan tâm đến tài liệu thư viện. - LDOCGIA_E
	*/
);
GO
CREATE TABLE THELOAI
(
	MaTheLoai	VARCHAR(5) NOT NULL PRIMARY KEY,
	TenTheLoai	NVARCHAR(225),
	IsDeleted	BIT DEFAULT 0
);
GO
CREATE TABLE SACH 
(
	MaSach		VARCHAR(5) NOT NULL PRIMARY KEY,
	TenSach		NVARCHAR(255),
	MaTheLoai	VARCHAR(5) FOREIGN KEY REFERENCES THELOAI(MaTheLoai),
	TacGia		NVARCHAR(255),
	NamXB		SMALLINT,
	NhaXB		NVARCHAR(255),
	NgayNhap	DATE,
	TriGia		INT,
	TinhTrang	SMALLINT, -- Lưu số lượng sách còn lại
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
	--AnhDaiDien	VARCHAR(30), -- Nếu cảm thấy không cần thì có thể bỏ qua thuộc tính này (NULL)
	IsDeleted	BIT DEFAULT 0
)

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
	IDSetting		VARCHAR(5) PRIMARY KEY,
	TuoiToiThieu	INT NOT NULL,
	TuoiToiDa		INT NOT NULL,
	ThoiHanThe		INT NOT NULL,
	SoNgayMuonToiDa	INT NOT NULL,
	SoTienNopTre	INT NOT NULL,
	SoSachMuonToiDa	INT NOT NULL,
	SoLuongTheLoaiToiDa	INT NOT NULL,
	ThoiGianNhapSach	INT NOT NULL
);
GO



CREATE TABLE PARAMETERs
(
	IDDocGia	INT 	DEFAULT 0,
	IDSach		INT 	DEFAULT 0,
	IDPhMuon	INT 	DEFAULT 0,
	IDPhThu		INT 	DEFAULT 0,
	IDPhTra		INT 	DEFAULT 5000,
	SoNgMuonTD	INT 	DEFAULT 30,
	TuoiTThieu	INT		DEFAULT 15
)
GO

-- Luôn cần một dòng dữ liệu trong bảng PARAMETERs để truy xuất thông tin sau này
INSERT INTO PARAMETERs VALUES(DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT);
GO

-- Tạo TRIGGER để tính ngày phải trả của cuốn sách
CREATE TRIGGER NGAY_PHAI_TRA_PHIEUMUON
ON PHIEUMUON
FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @MaPhMuon VARCHAR(5), @MAXDAY INT = 30
	
	SELECT @MaPhMuon = MaPhMuon FROM INSERTED

	-- Tính toán hạn trả trong phiếu mượn
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
	-- Lấy dữ liệu để kiểm tra điều kiện
	DECLARE @NgayPhTra DATE, @NgayTra DATE

	SELECT @NgayPhTra = NgayPhTra
	FROM INSERTED INNER JOIN PHIEUMUON ON INSERTED.MaPhMuon = PHIEUMUON.MaPhMuon
	SELECT @NgayTra = NgayTra FROM INSERTED

	IF (@NgayTra > @NgayPhTra)
	BEGIN
		DECLARE @MaPhTra VARCHAR(5), @IDPhTra INT = 5000, @SoNgQuaHan INT

		SELECT @MaPhTra = MaPhTra FROM INSERTED
		SELECT @SoNgQuaHan = DATEDIFF(day, @NgayPhTra, @NgayTra)

		-- Tổng tiền = Số ngày * Tiền nộp 1 ngày
		INSERT INTO PHIEUTHU VALUES(@MaPhTra, @SoNgQuaHan, @SoNgQuaHan * @IDPhTra, DEFAULT);
	
		PRINT('Trả sách quá thời hạn, đã lập phiếu thu!')
	END
END;
GO
-- INSERT DATA: Dữ liệu để test các chức năng của app

-- DOCGIA TABLE
INSERT INTO DOCGIA VALUES ('DG001', N'Nguyễn Thanh Hưng', 'LDOCGIA_A', N'Nam', '2002-06-15', N'135 Nam Kỳ Khởi Nghĩa, Bến Nghé, Quận 1', 'hungnt@gmail.com', '0392511342', '2024-02-07', '011202534234', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG002', N'Trần Nguyễn Yến Nhi', 'LDOCGIA_B', N'Nữ', '2008-11-14', N'Đường Lê Lợi, phường Bến Thành, quận 1', 'nhitran08@gmail.com', '0867455258', '2024-03-07', '044208234095', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG003', N'Trần Lê Tuyết Mai', 'LDOCGIA_A', N'Nữ', '2004-10-14', N'số 2 Nguyễn Bỉnh Khiêm, Quận 1', 'lmaiq1@gmail.com', '0914567842', '2024-03-12', '051204001002', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG004', N'Lê Thị Ngọc Ánh', 'LDOCGIA_A', N'Nữ', '2004-12-02', N'số 2 Khu Him Lam, quận 7', 'anhngoc@gmail.com', '0392411748', '2024-04-14', '034204543876', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG005', N'Phan Minh Long', 'LDOCGIA_B', N'Nam', '2009-05-07', N'Số 3 Hòa Bình, phường 3, quận 11', 'longSia@gmail.com', '0974373212', '2024-05-07', '034209112334', DEFAULT);
GO

-- SACH TABLE
INSERT INTO SACH VALUES ('SS001', N'Giáo trình toán cao cấp', 'TL005', N'Nhóm tác giả từ UIT', 2015, N'NXB Đại học quốc gia HCM', '2020-06-12', 60000, 20, DEFAULT);
INSERT INTO SACH VALUES ('SS002', N'Giáo trình Hệ điều hành', 'TL001', N'Nhóm tác giả từ UIT', 2015, N'NXB Đại học quốc gia HCM', '2020-05-12', 53000, 18, DEFAULT);
INSERT INTO SACH VALUES ('SS003', N'Toán học và ứng dụng', 'TL002', N'Nhóm tác giả từ viện toán học Việt Nam', 2014, N'Viện toán học Việt Nam', '2020-07-12', 72000, 3, DEFAULT);
INSERT INTO SACH VALUES ('SS004', N'Cẩm nang chăm sóc sức khỏe', 'TL002', N'Nguyễn Ngọc Hân, Đặng Huy Hoàng', 2014, N'NXB thông tin và truyền thông', '2020-05-12', 50000, 20, DEFAULT);
INSERT INTO SACH VALUES ('SS005', N'Ngôn ngữ lập trình C#', 'TL003', N'Phan Văn Thúc', 2010, N'NXB Giáo dục', '2020-05-12', 54000, 10, DEFAULT);
INSERT INTO SACH VALUES ('SS006', N'Đại số tuyến tính', 'TL005', N'Phạm Công Ngô', 2015, N'NXB Đại học quốc gia HCM', '2020-06-12', 65000, 12, DEFAULT);
INSERT INTO SACH VALUES ('SS007', N'Vật lý đại cương', 'TL004', N'Trần Văn Lượng', 2014, N'NXB Đại học quốc gia HCM', '2020-06-12', 65000, 10, DEFAULT);
GO

-- THELOAI TABLE
INSERT INTO THELOAI VALUES ('TL001', N'Giáo trình', DEFAULT);
INSERT INTO THELOAI VALUES ('TL002', N'Sách tham khảo', DEFAULT);
INSERT INTO THELOAI VALUES ('TL003', N'Tin học', DEFAULT);
INSERT INTO THELOAI VALUES ('TL004', N'Vật lý', DEFAULT);
INSERT INTO THELOAI VALUES ('TL005', N'Toán', DEFAULT);
INSERT INTO THELOAI VALUES ('TL006', N'Y học', DEFAULT);
INSERT INTO THELOAI VALUES ('TL007', N'Văn học', DEFAULT);
INSERT INTO THELOAI VALUES ('TL008', N'Lịch sử', DEFAULT);
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
INSERT INTO PHIEUTRA VALUES ('PT001', 'PM003', '2024-05-21', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM003';
INSERT INTO PHIEUTRA VALUES ('PT002', 'PM005', '2024-05-22', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM005';
INSERT INTO PHIEUTRA VALUES ('PT003', 'PM007', '2024-05-10', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM007';
INSERT INTO PHIEUTRA VALUES ('PT004', 'PM002', '2024-05-15', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM002';
INSERT INTO PHIEUTRA VALUES ('PT005', 'PM001', '2024-05-18', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM001';
INSERT INTO PHIEUTRA VALUES ('PT006', 'PM004', '2024-05-15', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM004';
INSERT INTO PHIEUTRA VALUES ('PT007', 'PM006', '2024-05-14', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM006';
INSERT INTO PHIEUTRA VALUES ('PT008', 'PM009', '2024-05-20', DEFAULT);
UPDATE PHIEUMUON SET IsDeleted = 1 WHERE MaPhMuon = 'PM009';
GO

-- THUTHU TABLE
INSERT INTO THUTHU VALUES ('TT001', N'Lương Ngọc Huyền', N'Nữ', '5/01/2024', '12/03/2003', N'Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', 'huyenlb@gmail.com', '0369442256', DEFAULT);
INSERT INTO THUTHU VALUES ('TT002', N'Cao Văn Thành', N'Nam', '12/01/2024', '22/05/2003', N'Đường Lê Duẩn, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 'thanhcv03@gmail.com', '0973464756', DEFAULT);
GO

-- ACCOUNT TABLE
INSERT INTO ACCOUNT VALUES ('TaiKhoan1', 'MatKhau1', 'DG001', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan2', 'MatKhau2', 'DG002', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan3', 'MatKhau3', 'DG003', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan4', 'MatKhau4', 'DG004', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan5', 'MatKhau5', 'DG005', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan6', 'MatKhau6', NULL, 'TT001', 2, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan7', 'MatKhau7', NULL, 'TT002', 2, DEFAULT);

--SETTING TABLE
INSERT INTO SETTING VALUES ('ST001', 13, 70, 24, 30, 2000, 5, 30, 5)
