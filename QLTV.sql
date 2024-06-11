CREATE DATABASE QLTV_BETA
GO
USE QLTV_BETA
GO
SET DATEFORMAT DMY
GO

CREATE TABLE DOCGIA
(
	MaDG		VARCHAR(5) PRIMARY KEY,
	LoaiDG		VARCHAR(255),
	NgaySinh	DATE,
	DiaChi		VARCHAR(255),
	Email		VARCHAR(255),
	NgayLapThe	DATE,
	IsDeleted	BIT DEFAULT 0
	/* Các loại độc giả
	Độc giả sinh viên: Bao gồm các sinh viên đang học tại trường hoặc các cơ sở giáo dục. - LDOCGIA_A
	Độc giả học sinh: Bao gồm học sinh đang theo học tại các trường cấp 1, 2, 3. - LDOCGIA_B
	Cán bộ giáo viên: Bao gồm giáo viên, nhân viên trong ngành giáo dục. - LDOCGIA_C
	Người đã đi làm: Độc giả không thuộc các nhóm trên, đã đi làm và có nhu cầu sử dụng thư viện. - LDOCGIA_D
	Các đối tượng khác: Có thể bao gồm người nước ngoài, người nghiên cứu, người quan tâm đến tài liệu thư viện. - LDOCGIA_E
	*/
);

CREATE TABLE THUTHU
(
	MaTT		VARCHAR(5) PRIMARY KEY,	
	NgaySinh	DATE,
	DiaChi		VARCHAR(255),
	Email		VARCHAR(255),
	IsDeleted	BIT DEFAULT 0
)
GO

CREATE TABLE SACH 
(
	MaSach		VARCHAR(5) PRIMARY KEY,
	TenSach		VARCHAR(255),
	TheLoai		VARCHAR(255),
	TacGia		VARCHAR(255),
	NamXB		SMALLINT,
	NhaXB		VARCHAR(255),
	NgayNhap	DATE,
	TriGia		INT,
	TinhTrang	BIT,
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
	SoTienThu	INT
);
GO

CREATE TABLE ACCOUNT
(
	ID			INT IDENTITY(1, 1) PRIMARY KEY,
	TaiKhoan	VARCHAR(100) NOT NULL,
	MatKhau		VARCHAR(50) NOT NULL,
	-- Để phục vụ cho việc thay đổi tên tài khoản, mật khẩu, không nên để tài khoản và mật khẩu làm thuộc tính khóa chính
	MaDG		VARCHAR(5) FOREIGN KEY REFERENCES DOCGIA(MaDG),
	IsDeleted	BIT DEFAULT 0
);
GO

CREATE TABLE THUTHU
(
	ID			INT IDENTITY(1, 1) PRIMARY KEY,
	TaiKhoan	VARCHAR(100) NOT NULL,
	MatKhau		VARCHAR(50) NOT NULL,
	SDT			VARCHAR(11) NOT NULL,
	Email		VARCHAR(50) NOT NULL,
	DiaChi		VARCHAR(50) NOT NULL
)
GO

CREATE TABLE PARAMETERs
(
	IDDocGia	INT 	DEFAULT 0,
	IDSach		INT 	DEFAULT 0,
	IDPhMuon	INT 	DEFAULT 0,
	IDPhThu		INT 	DEFAULT 0,
	IDPhTra		INT 	DEFAULT 5000,
	SoNgMuonTD	INT 	DEFAULT 30
)
GO

-- INSERT DATA INTO PARAMETERS
INSERT INTO PARAMETERs VALUES(DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT);
GO

-- Update IDDocGia when we insert a new row in table DOCGIA
CREATE TRIGGER INSERT_INTO_DOCGIA
ON DOCGIA
FOR INSERT
AS
BEGIN
	UPDATE PARAMETERs
	SET IDDocGia = IDDocGia + 1
END
GO

-- Update
CREATE TRIGGER INSERT_INTO_SACH
ON SACH
FOR INSERT
AS
BEGIN
	UPDATE PARAMETERs
	SET IDSach = IDSach + 1
END
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

	PRINT('Thông tin phiếu mượn đã được thêm/chỉnh sửa hoàn tất!')

	IF EXISTS (SELECT * FROM INSERTED) 
	BEGIN
		UPDATE PARAMETERs
		SET IDPhMuon = IDPhMuon + 1
	END
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
		INSERT INTO PHIEUTHU VALUES(@MaPhTra, @SoNgQuaHan, @SoNgQuaHan * @IDPhTra)
	
		PRINT('Trả sách quá thời hạn, đã lập phiếu thu!')

		-- Update IDPhTra
		UPDATE PARAMETERs
		SET IDPhTra = IDPhTra + 1
	END
END;
GO

-- Phần INSERT DATA ở đây chỉ nhằm phục vụ cho việc demo các chức năng sau này, còn trong quá trình mượn sách, việc INSERT sẽ được thực hiện thông qua mã C#
/*
Hoàn thành phần dữ liệu của các bảng trong file CSDL_Excel rồi sau đó sẽ thêm vào file chính thức này.
*/
