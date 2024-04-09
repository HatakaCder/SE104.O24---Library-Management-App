CREATE DATABASE QLTV_BETA
GO
USE QLTV_BETA
GO
SET DATEFORMAT DMY -- dateformat: dd/mm/yyyy
GO

CREATE TABLE DOCGIA 
(
	MaDG		VARCHAR(5) NOT NULL PRIMARY KEY,
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
	TaiKhoan	VARCHAR(5) NOT NULL,
	MatKhau		VARCHAR(5) NOT NULL,
	-- Để phục vụ cho việc thay đổi tên tài khoản, mật khẩu, không nên để tài khoản và mật khẩu làm thuộc tính khóa chính
	MaDG		VARCHAR(5) FOREIGN KEY REFERENCES DOCGIA(MaDG),
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
	SoNgMuonTD	INT 	DEFAULT 30
)
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
		INSERT INTO PHIEUTHU VALUES(@MaPhTra, @SoNgQuaHan, @SoNgQuaHan * @IDPhTra)
	
		PRINT('Trả sách quá thời hạn, đã lập phiếu thu!')
	END
END;
GO

-- INSERT DATA INTO DOCGIA TABLE
INSERT INTO DOCGIA VALUES ('DG001', 'LDOCGIA_A', '29/11/2005', '12 Nguyễn Huy Tự, phường Đa Kao, quận 1, Tp. Hồ Chí Minh', 'thanhdangphan1510@gmail.com', '22/04/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG002', 'LDOCGIA_B', '01/07/2007', '104 Hoàng Diệu, phường 12, quận 4,Tp, Hồ Chí Minh', 'hungdung1234@gmail.com', '09/06/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG003', 'LDOCGIA_A', '19/1/2004', '26 Lê Thị Riêng, phường Bến Thành, quận 1, Tp. Hồ Chí Minh', 'dangphanthanh42@gmail.com', '13/12/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG004', 'LDOCGIA_A', '08/10/2003', '823 Nguyễn Trãi, phường.14, quận 5, Tp. Hồ Chí Minh', 'gmail1@gm.com', '02/01/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG005', 'LDOCGIA_C', '10/11/1999', '101 Vạn Kiếp, phường 3, quận Bình Thạnh, Tp. Hồ Chí Minh', 'email2@gmail.com', '15/3/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG006', 'LDOCGIA_A', '24/10/2004', '199 Vĩnh Viễn, phường 4, quận 10, Tp. Hồ Chí Minh', '30000000@gm.uit.edu.vn', '19/11/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG007', 'LDOCGIA_A', '07/06/2005', '40 Huỳnh Tấn Phát, huyện Nhà Bè, Tp. Hồ Chí Minh', '22522252@gm.uit.edu.vn', '04/04/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG008', 'LDOCGIA_A', '19/10/2005', '111/1A đường số 8, phường 11, quận Gò Vấp, Tp. Hồ Chí Minh', '23522352@gm.uit.edu.vn', '08/01/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG009', 'LDOCGIA_A', '24/4/2004', '383 Vĩnh Khánh, phường 8, quận 4, Tp. Hồ Chí Minh', '24521000@gm.uit.edu.vn', '15/8/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG010', 'LDOCGIA_A', '18/7/2007', 'Số 37 Vĩnh Khánh, phường 8, quận 4, Tp. Hồ Chí Minh', 'gmail_11@gmail.com', '27/12/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG011', 'LDOCGIA_A', '02/09/2001', '210 Tô Hiến Thành, phường 15, quận 10, Tp. Hồ Chí Minh', 'gmail_12@gmail.com', '26/9/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG012', 'LDOCGIA_C', '30/12/1998', '181/6 Xóm Chiếu, phường 16, quận 4, Tp. Hồ Chí Minh', 'gmail_13@gmail.com', '18/7/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG013', 'LDOCGIA_C', '24/10/1996', '720 CMT8, phường .5, quận Tân Bình, Tp. Hồ Chí Minh', 'gmail_14@gmail.com', '19/5/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG014', 'LDOCGIA_C', '19/7/1999', '453/50 Lê Văn Sỹ, phường 12, quận 3, Tp. Hồ Chí Minh', 'gmail_15@gmail.com', '01/11/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG015', 'LDOCGIA_C', '15/8/1999', '178/31 Nguyễn Văn Thương, P.25, Q.Bình Thạnh, Tp.HCM', 'gmail_16@gmail.com', '03/05/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG016', 'LDOCGIA_A', '17/5/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_17@gmail.com', '27/5/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG017', 'LDOCGIA_A', '23/4/2004', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_18@gmail.com', '27/3/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG018', 'LDOCGIA_A', '15/11/2004', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_19@gmail.com', '19/8/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG019', 'LDOCGIA_A', '18/9/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_20@gmail.com', '10/07/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG020', 'LDOCGIA_A', '12/11/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_21@gmail.com', '29/5/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG021', 'LDOCGIA_A', '22/10/2004', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_22@gmail.com', '07/03/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG022', 'LDOCGIA_A', '20/3/2002', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_23@gmail.com', '24/3/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG023', 'LDOCGIA_A', '05/06/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_24@gmail.com', '01/05/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG024', 'LDOCGIA_A', '09/01/2004', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_25@gmail.com', '03/04/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG025', 'LDOCGIA_A', '11/02/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_26@gmail.com', '04/11/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG026', 'LDOCGIA_A', '23/1/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_27@gmail.com', '03/07/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG027', 'LDOCGIA_A', '22/9/2006', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_28@gmail.com', '30/5/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG028', 'LDOCGIA_A', '23/8/2005', 'đường Tạ Quang Bửu, Khu phố 6, P. Linh Trung, TP Thủ Đức, TP Hồ Chí Minh', 'gmail_29@gmail.com', '27/7/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG029', 'LDOCGIA_C', '18/5/1998', '116 Vườn Chuối, phường 4, quận 3, Tp. Hồ Chí Minh', 'gmail_30@gmail.com', '04/03/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG030', 'LDOCGIA_C', '09/04/1999', '174 Nguyễn Thiện Thuật, phường 1, quận 3, Tp. Hồ Chí Minh', 'gmail_31@gmail.com', '07/09/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG031', 'LDOCGIA_C', '25/1/2004', 'Dương Tử Giang, phường 14, quận 5, Tp. Hồ Chí Minh', 'gmail_32@gmail.com', '16/10/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG032', 'LDOCGIA_C', '05/11/2000', '192 - 194 Nguyễn Tri Phương, phường 4, quận 10, Tp. Hồ Chí Minh', 'gmail_33@gmail.com', '04/11/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG033', 'LDOCGIA_A', '19/4/2005', 'Tô Vĩnh Diện, Đông Hoà, Dĩ An, Bình Dương', 'gmail_34@gmail.com', '23/10/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG034', 'LDOCGIA_D', '07/11/2002', '84/1 Hồ Thị Kỷ, phường 1, quận 10, Tp. Hồ Chí Minh', 'gmail_35@gmail.com', '24/1/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG035', 'LDOCGIA_D', '02/03/2003', 'Hẻm 372 Cách Mạng Tháng 8, phường 10, quận 3, Tp. Hồ Chí Minh', 'gmail_36@gmail.com', '20/4/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG036', 'LDOCGIA_A', '20/1/2003', 'Tô Vĩnh Diện, Đông Hoà, Dĩ An, Bình Dương', 'gmail_37@gmail.com', '16/10/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG037', 'LDOCGIA_C', '09/08/1998', '1-3 Cao Bá Nhạ, phường Nguyễn Cư Trinh, quận 1, Tp. Hồ Chí Minh', 'gmail_38@gmail.com', '07/07/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG038', 'LDOCGIA_C', '02/03/1997', '105 Trương Định, phường 6, quận 3, Tp. Hồ Chí Minh', 'gmail_39@gmail.com', '16/8/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG039', 'LDOCGIA_A', '02/10/2004', 'Tô Vĩnh Diện, Đông Hoà, Dĩ An, Bình Dương', 'gmail_40@gmail.com', '27/2/2024', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG040', 'LDOCGIA_E', '23/10/2006', '436 Nguyễn Thị Thập, phường Tân Quy, quận 7, Tp. Hồ Chí Minh', 'gmail_41@gmail.com', '20/11/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG041', 'LDOCGIA_C', '30/12/1998', '31 Yên Thế, phường 2, quận Tân Bình, Tp. Hồ Chí Minh', 'gmail_42@gmail.com', '16/4/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG042', 'LDOCGIA_D', '05/02/2002', '99 Châu Văn Liêm, phường 14, quận 5, Tp. Hồ Chí Minh', 'gmail_43@gmail.com', '06/03/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG043', 'LDOCGIA_C', '02/08/2000', '178/31 Nguyễn Văn Thương, P.25, Q.Bình Thạnh, Tp.HCM', 'gmail_44@gmail.com', '27/10/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG044', 'LDOCGIA_C', '15/6/2001', '178/31 Nguyễn Văn Thương, P.25, Q.Bình Thạnh, Tp.HCM', 'gmail_45@gmail.com', '03/01/2021', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG045', 'LDOCGIA_C', '26/6/2001', '178/31 Nguyễn Văn Thương, P.25, Q.Bình Thạnh, Tp.HCM', 'gmail_46@gmail.com', '20/7/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG046', 'LDOCGIA_C', '19/6/1997', '178/31 Nguyễn Văn Thương, P.25, Q.Bình Thạnh, Tp.HCM', 'gmail_47@gmail.com', '24/4/2023', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG047', 'LDOCGIA_C', '10/12/2000', '178/31 Nguyễn Văn Thương, P.25, Q.Bình Thạnh, Tp.HCM', 'gmail_48@gmail.com', '03/01/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG048', 'LDOCGIA_A', '23/7/2004', 'Tô Vĩnh Diện, Đông Hoà, Dĩ An, Bình Dương', 'gmail_49@gmail.com', '19/8/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG049', 'LDOCGIA_A', '20/8/2003', 'Tô Vĩnh Diện, Đông Hoà, Dĩ An, Bình Dương', 'gmail_50@gmail.com', '26/9/2022', DEFAULT);
INSERT INTO DOCGIA VALUES ('DG050', 'LDOCGIA_A', '26/11/2004', 'Tô Vĩnh Diện, Đông Hoà, Dĩ An, Bình Dương', 'gmail_51@gmail.com', '04/07/2024', DEFAULT);
GO

-- INSERT DATA INTO SACH TABLE
INSERT INTO SACH VALUES('SH001', 'Ten sach 1', 'Giao trinh', 'Tac gia 1', 2000, 'DHQG HCM', '24/02/2001', 75000, 1, DEFAULT);
INSERT INTO SACH VALUES('SH002', 'Ten sach 2', 'Sach tham khao', 'Tac gia 2', 2003, 'DHQG HCM', '15/03/2005', 100000, 1, DEFAULT);
INSERT INTO SACH VALUES('SH003', 'Ten sach 3', 'Truyen tranh', 'Tac gia 3', 2015, 'Nha xuat ban 1', '14/10/2019', 50000, 1, DEFAULT);
INSERT INTO SACH VALUES('SH004', 'Ten sach 4', 'Giao trinh', 'Tac gia 4', 2012, 'DHQG HA NOI', '23/03/2017', 80000, 1, DEFAULT);
INSERT INTO SACH VALUES('SH005', 'Ten sach 5', 'Giao trinh', 'Tac gia 5', 2012, 'DHQG HCM', '24/10/2017', 85000, 1, DEFAULT);
INSERT INTO SACH VALUES('SH006', 'Ten sach 6', 'Sach tham khao', 'Tac gia 6', 2017, 'DHQG HCM', '12/12/2020', 90000, 1, DEFAULT);
INSERT INTO SACH VALUES('SH007', 'Ten sach 7', 'Giao trinh', 'Tac gia 7', 2018, 'DHQG HA NOI', '03/03/2019', 90000, 1, DEFAULT);
GO

-- INSERT DATA INTO PHIEUMUON
INSERT INTO PHIEUMUON VALUES('PM001', 'DG001', 'SH004', '20/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM002', 'DG004', 'SH004', '21/02/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM003', 'DG001', 'SH001', '25/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM004', 'DG002', 'SH002', '19/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM005', 'DG003', 'SH006', '26/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM006', 'DG005', 'SH007', '19/02/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM007', 'DG004', 'SH002', '01/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM008', 'DG001', 'SH003', '25/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM009', 'DG003', 'SH001', '26/03/2024', NULL, DEFAULT);
INSERT INTO PHIEUMUON VALUES('PM010', 'DG005', 'SH003', '26/03/2024', NULL, DEFAULT);
GO

-- INSERT DATA INTO PHIEUTRA TABLE 
INSERT INTO PHIEUTRA VALUES('PT001', 'PM002', '01/10/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES('PT002', 'PM003', '19/04/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES('PT003', 'PM008', '19/11/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES('PT004', 'PM005', '01/04/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES('PT005', 'PM006', '29/03/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES('PT006', 'PM001', '22/05/2024', DEFAULT);
INSERT INTO PHIEUTRA VALUES('PT007', 'PM004', '01/04/2024', DEFAULT);
GO

-- INSERT DATA INTO ACCOUNT TABLE
INSERT INTO ACCOUNT VALUES('USER1', 'pass1', 'DG001', DEFAULT);
INSERT INTO ACCOUNT VALUES('USER2', 'pass2', 'DG002', DEFAULT);
INSERT INTO ACCOUNT VALUES('USER3', 'pass3', 'DG003', DEFAULT);
INSERT INTO ACCOUNT VALUES('USER4', 'pass4', 'DG004', DEFAULT);
INSERT INTO ACCOUNT VALUES('USER5', 'pass5', 'DG005', DEFAULT);
