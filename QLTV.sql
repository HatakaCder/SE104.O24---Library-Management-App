CREATE DATABASE QLTV_BETA
GO
USE QLTV_BETA
GO
SET DATEFORMAT DMY -- Cần thống nhất định dạng của ngày tháng trong CSDL
GO

CREATE TABLE DOCGIA
(
	MaDG		VARCHAR(5) PRIMARY KEY,
	LoaiDG		VARCHAR(255),
	HoTen		VARCHAR(50) NOT NULL,
	NgaySinh	DATE,
	DiaChi		VARCHAR(255),
	Email		VARCHAR(255) NOT NULL,
	GioiTinh	VARCHAR(3),
	NgayLapThe	DATE,
	SoDT		VARCHAR(10),
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

CREATE TABLE SACH 
(
	MaSach		VARCHAR(5) PRIMARY KEY,
	TenSach		VARCHAR(255) NOT NULL,
	TheLoai		VARCHAR(255) NOT NULL,
	TacGia		VARCHAR(255) NOT NULL,
	NamXB		SMALLINT NOT NULL,
	NhaXB		VARCHAR(255) NOT NULL,
	NgayNhap	DATE NOT NULL,
	TriGia		INT NOT NULL,
	TinhTrang	BIT NOT NULL,
	IsDeleted	BIT DEFAULT 0
);
GO

CREATE TABLE PHIEUMUON
(
	MaPhMuon	VARCHAR(5) NOT NULL PRIMARY KEY,
	MaDG		VARCHAR(5) NOT NULL FOREIGN KEY REFERENCES DOCGIA(MaDG),
	MaSach		VARCHAR(5) NOT NULL FOREIGN KEY REFERENCES SACH(MaSach),
	NgayMuon	DATE NOT NULL,
	NgayPhTra	DATE NOT NULL,
	IsDeleted	BIT DEFAULT 0
);
GO

SELECT * FROM PHIEUMUON

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
	-- Cần thống nhất các giá trị ràng buộc cần thiết (Tuổi tối đa, tuổi tối thiểu, số ngày mượn tối đa, ...)
	IDSearch	SMALLINT	DEFAULT 1, -- Biến này phục vụ trong quá trình cần lấy dữ liệu từ bảng PARAMETERS
	IDDocGia	INT 		DEFAULT 0,
	IDSach		INT 		DEFAULT 0,
	IDPhMuon	INT 		DEFAULT 0,
	IDPhThu		INT 		DEFAULT 0,
	IDPhTra		INT 		DEFAULT 5000,
	SoNgMuonTD	INT 		DEFAULT 30,
	TuoiTT		SMALLINT	DEFAULT 15,
	TuoiTD		SMALLINT	DEFAULT 60
)
GO

-- INSERT DATA INTO PARAMETERS
INSERT INTO PARAMETERs VALUES(DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT, DEFAULT);
GO

-- TẠO CÁC RÀNG BUỘC KHI THAO TÁC VỚI DỮ LIỆU TRONG BẢNG DOCGIA

-- Cập nhật IDDocGia khi thêm một độc giả mới vào CSDL 
CREATE TRIGGER INSERT_INTO_DOCGIA
ON DOCGIA
FOR INSERT
AS
BEGIN
	UPDATE PARAMETERs
	SET IDDocGia = IDDocGia + 1
END
GO

-- Giới tính của độc giả phải thuộc (nam, nữ)
ALTER TABLE DOCGIA ADD CONSTRAINT CHECK_GIOITINH CHECK(GioiTinh IN ('Nam', 'Nữ'));
GO

-- Tuổi của độc giả phải lớn hơn hoặc bằng độ tuổi tối thiểu (TuoiTT) và nhỏ hơn hoặc bằng độ tuổi tối đa trong quy định (TuoiTD)
CREATE TRIGGER INSERT_UPDATE_TUOI_DOCGIA
ON DOCGIA
FOR INSERT, UPDATE
AS
BEGIN
	DECLARE @NgaySinh DATE, @NgayLapThe DATE, @TuoiKC SMALLINT
	DECLARE @TuoiInf SMALLINT, @TuoiSup SMALLINT

	SELECT @NgaySinh = NgaySinh FROM INSERTED
	SELECT @NgayLapThe = NgayLapThe FROM INSERTED
	SELECT @TuoiInf = TuoiTT FROM PARAMETERs WHERE IDSearch = 1
	SELECT @TuoiSup = TuoiTD FROM PARAMETERs WHERE IDSearch = 1
	SELECT @TuoiKC = DATEDIFF(year, @NgayLapThe, @NgaySinh)

	IF (@TuoiKC > @TuoiSup OR @TuoiKC < @TuoiInf) 
	BEGIN
		RAISERROR('Tuổi của độc giả phải nằm trong độ tuổi đã có trong quy định', 16, 1)
		ROLLBACK TRAN
		RETURN
	END
END
GO

-- Định dạng của email phải là: expamle@example.com
ALTER TABLE DOCGIA ADD CONSTRAINT CHECK_EMAIL_PATTERN CHECK(Email LIKE ('%@%.com'));
GO

-- Số điện thoại phải đúng định dạng số điện thoại của Việt Nam
ALTER TABLE DOCGIA ADD CONSTRAINT CHECK_SDT CHECK(LEN(SoDT) = 10 AND SUBSTRING(SoDT, 0, 3) IN ('032', '033', '034', '035', '036', '037', '038', '039', '096', '097', '098', '086', '083', '084', '085', '081', '082', '088', '091', '094', '070', '079', '077', '076', '078', '090', '093', '089', '056', '058', '092', '059', '099'));
GO

-- Ngày lập thẻ phải nhỏ hơn hoặc đúng bằng ngày hiện tại
ALTER TABLE DOCGIA ADD CONSTRAINT CHECK_NGAYLT CHECK(NgayLapThe <= GETDATE());
GO

-- TẠO CÁC RÀNG BUỘC KHI THAO TÁC VỚI DỮ LIỆU TRONG BẢNG SACH

-- Cập nhật IDSach khi thêm một cuốn sách mới vào CSDL
CREATE TRIGGER INSERT_INTO_SACH
ON SACH
FOR INSERT
AS
BEGIN
	UPDATE PARAMETERs
	SET IDSach = IDSach + 1
END
GO

-- Năm xuất bản phải bé hơn hoặc bằng năm nhập
ALTER TABLE SACH ADD CONSTRAINT CHECK_NAMXB CHECK(NamXB <= YEAR(NgayNhap));
GO

-- Ngày nhập phải nhỏ hơn hoặc bằng ngày hiện tại
ALTER TABLE SACH ADD CONSTRAINT CHECK_NAMNHAP CHECK(NgayNhap <= GETDATE());
GO

-- Trị giá phải đúng định dạng số và là số không âm
ALTER TABLE SACH ADD CONSTRAINT CHECK_TRIGIA CHECK(TRIGIA >= 0);
GO

-- TẠO CÁC RÀNG BUỘC KHI THAO TÁC VỚI DỮ LIỆU TRONG BẢNG PHIEUMUON

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

-- TẠO CÁC RÀNG BUỘC KHI THAO TÁC VỚI DỮ LIỆU TRONG BẢNG PHIEUTRA
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
