/*
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
	HoTen		VARCHAR(30),
	LoaiDG		VARCHAR(255),
	GioiTinh	VARCHAR(3),
	NgaySinh	DATE,
	DiaChi		VARCHAR(255),
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

CREATE TABLE SACH 
(
	MaSach		VARCHAR(5) NOT NULL PRIMARY KEY,
	TenSach		VARCHAR(255),
	TheLoai		VARCHAR(255),
	TacGia		VARCHAR(255),
	NamXB		SMALLINT,
	NhaXB		VARCHAR(255),
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
	HoTen		VARCHAR(30),
	GioiTinh	VARCHAR(3),
	NgayVLam	DATE,
	NgaySinh	DATE,
	DiaChi		VARCHAR(255),
	Email		VARCHAR(255) NOT NULL,
	SoDT		VARCHAR(10) NOT NULL,
	--AnhDaiDien	VARCHAR(30), -- Nếu cảm thấy không cần thì có thể bỏ qua thuộc tính này (NULL)
	IsDeleted	BIT DEFAULT 0
)

CREATE TABLE ACCOUNT
(
	ID			INT IDENTITY(1, 1) PRIMARY KEY,
	TaiKhoan	VARCHAR(30) NOT NULL,
	MatKhau		VARCHAR(30) NOT NULL,
	-- Để phục vụ cho việc thay đổi tên tài khoản, mật khẩu, không nên để tài khoản và mật khẩu làm thuộc tính khóa chính
	MaDG		VARCHAR(5) FOREIGN KEY REFERENCES DOCGIA(MaDG),
	MaTT		VARCHAR(5) FOREIGN KEY REFERENCES THUTHU(MaTT),
	VaiTro		SMALLINT NOT NULL, -- biến này để phân biệt người dùng, thủ thư hay admin, bởi vì mình chỉ sử dụng một bảng này để lưu thông tin tài khoản cho tất cả nên cần có biến này. Mình có thể xét như sau: 3: Admin, 2: thủ thư, và 1: người dùng.
	IsDeleted	BIT DEFAULT 0
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
INSERT INTO DOCGIA VALUES ('DG001', 'Nguyễn Thanh Hưng', 'LDOCGIA_A', 'Nam', '15/06/2002', '135 Nam Kỳ Khởi Nghĩa, Bến Nghé, Quận 1', 'hungnt@gmail.com', '0392511342', '7/02/2024', '011202534234', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG002', 'Trần Nguyễn Yến Nhi', 'LDOCGIA_B', 'Nữ', '14/11/2008', 'Đường Lê Lợi, phường Bến Thành, quận 1', 'nhitran08@gmail.com', '0867455258', '7/03/2024', '044208234095', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG003', 'Trần Lê Tuyết Mai', 'LDOCGIA_A', 'Nữ', '14/10/2004', 'số 2 Nguyễn Bỉnh Khiêm, Quận 1', 'lmaiq1@gmail.com', '0914567842', '12/03/2024', '051204001002', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG004', 'Lê Thị Ngọc Ánh', 'LDOCGIA_A', 'Nữ', '02/12/2004', 'số 2 Khu Him Lam, quận 7', 'anhngoc@gmail.com', '0392411748', '14/04/2024', '034204543876', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG005', 'Phan Minh Long', 'LDOCGIA_B', 'Nam', '07/05/2009', 'Số 3 Hòa Bình, phường 3, quận 11', 'longSia@gmail.com', '0974373212', '7/05/2024', '034209112334', DEFAULT);
GO

-- SACH TABLE
INSERT INTO  SACH VALUES ('SS001', 'Giáo trình toán cao cấp', 'Giáo trình, toán', 'Nhóm tác giả từ UIT', 2015, 'NXB Đại học quốc gia HCM', '12/6/2020', 60000, 20, DEFAULT);
INSERT INTO  SACH VALUES ('SS002', 'Giáo trình Hệ điều hành', 'Giáo trình', 'Nhóm tác giả từ UIT', 2015, 'NXB Đại học quốc gia HCM', '12/5/2020', 53000, 18, DEFAULT);
INSERT INTO  SACH VALUES ('SS003', 'Toán học và ứng dụng', 'Sách tham khảo, toán', 'Nhóm tác giả từ viện toán học Việt Nam', 2014, 'Viện toán học Việt Nam', '12/7/2020', 72000, 3, DEFAULT);
INSERT INTO  SACH VALUES ('SS004', 'Cẩm nang chăm sóc sức khỏe', 'Sách tham khảo', 'Nguyễn Ngọc Hân, Đặng Huy Hoàng', 2014, 'NXB thông tin và truyền thông', '12/5/2020', 50000, 20, DEFAULT);
INSERT INTO  SACH VALUES ('SS005', 'Ngôn ngữ lập trình C#', 'Sách tham khảo, tin học', 'Phan Văn Thúc', 2010, 'NXB Giáo dục', '12/5/2020', 54000, 10, DEFAULT);
INSERT INTO  SACH VALUES ('SS006', 'Đại số tuyến tính', 'Giáo trình, toán', 'Phạm Công Ngô', 2015, 'NXB Đại học quốc gia HCM', '12/6/2020', 65000, 12, DEFAULT);
INSERT INTO  SACH VALUES ('SS007', 'Vật lý đại cương', 'Giáo trình, vật lý', 'Trần Văn Lượng', 2014, 'NXB Đại học quốc gia HCM', '12/6/2020', 65000, 10, DEFAULT);
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
INSERT INTO THUTHU VALUES ('TT001', 'Lương Ngọc Huyền', 'Nữ', '5/01/2024', '12/03/2003', 'Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', 'huyenlb@gmail.com', '0369442256', DEFAULT);
INSERT INTO THUTHU VALUES ('TT002', 'Cao Văn Thành', 'Nam', '12/01/2024', '22/05/2003', 'Đường Lê Duẩn, Bến Nghé, Quận 1, Thành phố Hồ Chí Minh', 'thanhcv03@gmail.com', '0973464756', DEFAULT);
GO

-- ACOUNT TABLE
INSERT INTO ACCOUNT VALUES ('TaiKhoan1', 'MatKhau1', 'DG001', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan2', 'MatKhau2', 'DG002', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan3', 'MatKhau3', 'DG003', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan4', 'MatKhau4', 'DG004', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan5', 'MatKhau5', 'DG005', NULL, 1, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan6', 'MatKhau6', NULL, 'TT001', 2, DEFAULT);
INSERT INTO ACCOUNT VALUES ('TaiKhoan7', 'MatKhau7', NULL, 'TT002', 2, DEFAULT);