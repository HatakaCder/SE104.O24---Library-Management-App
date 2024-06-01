USE [QLTV_BETA]
GO
/****** Object:  Table [dbo].[ACCOUNT]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ACCOUNT](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[TaiKhoan] [varchar](5) NOT NULL,
	[MatKhau] [varchar](5) NOT NULL,
	[MaDG] [varchar](5) NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Category]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[Id] [varchar](5) NOT NULL,
	[Ten] [varchar](255) NULL,
	[NgayLapThe] [date] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DOCGIA]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DOCGIA](
	[MaDG] [varchar](5) NOT NULL,
	[LoaiDG] [varchar](255) NULL,
	[NgaySinh] [date] NULL,
	[DiaChi] [varchar](255) NULL,
	[Email] [varchar](255) NULL,
	[NgayLapThe] [date] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaDG] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PARAMETERs]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PARAMETERs](
	[IDDocGia] [int] NULL,
	[IDSach] [int] NULL,
	[IDPhMuon] [int] NULL,
	[IDPhThu] [int] NULL,
	[IDPhTra] [int] NULL,
	[SoNgMuonTD] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIEUMUON]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIEUMUON](
	[MaPhMuon] [varchar](5) NOT NULL,
	[MaDG] [varchar](5) NULL,
	[MaSach] [varchar](5) NULL,
	[NgayMuon] [date] NULL,
	[NgayPhTra] [date] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhMuon] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIEUTHU]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIEUTHU](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[MaPhTra] [varchar](5) NULL,
	[SoNgayQHan] [smallint] NULL,
	[SoTienThu] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PHIEUTRA]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PHIEUTRA](
	[MaPhTra] [varchar](5) NOT NULL,
	[MaPhMuon] [varchar](5) NULL,
	[NgayTra] [date] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhTra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SACH]    Script Date: 5/22/2024 12:50:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SACH](
	[MaSach] [varchar](5) NOT NULL,
	[TenSach] [varchar](255) NULL,
	[TheLoai] [varchar](255) NULL,
	[TacGia] [varchar](255) NULL,
	[NamXB] [smallint] NULL,
	[NhaXB] [varchar](255) NULL,
	[NgayNhap] [date] NULL,
	[TriGia] [int] NULL,
	[TinhTrang] [bit] NULL,
	[IsDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[ACCOUNT] ON 

INSERT [dbo].[ACCOUNT] ([ID], [TaiKhoan], [MatKhau], [MaDG], [IsDeleted]) VALUES (1, N'USER1', N'pass1', N'DG001', 0)
INSERT [dbo].[ACCOUNT] ([ID], [TaiKhoan], [MatKhau], [MaDG], [IsDeleted]) VALUES (2, N'USER2', N'pass2', N'DG002', 0)
INSERT [dbo].[ACCOUNT] ([ID], [TaiKhoan], [MatKhau], [MaDG], [IsDeleted]) VALUES (3, N'USER3', N'pass3', N'DG003', 0)
INSERT [dbo].[ACCOUNT] ([ID], [TaiKhoan], [MatKhau], [MaDG], [IsDeleted]) VALUES (4, N'USER4', N'pass4', N'DG004', 0)
INSERT [dbo].[ACCOUNT] ([ID], [TaiKhoan], [MatKhau], [MaDG], [IsDeleted]) VALUES (5, N'USER5', N'pass5', N'DG005', 0)
SET IDENTITY_INSERT [dbo].[ACCOUNT] OFF
GO
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'4Rz4k', N'aaasssll', CAST(N'2024-05-22' AS Date), 0)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'abc12', N'ABC', CAST(N'2022-10-10' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'abc13', N'ABC2', CAST(N'2022-10-10' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'borlK', N'Alo alo', CAST(N'2024-05-22' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'fjd77', N'ABCmmc', CAST(N'2024-05-22' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'nbjZH', N'lllolollo', CAST(N'2024-05-22' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'PgTfV', N'akamnsns', CAST(N'2024-05-22' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'QYlZj', N'akakakasss', CAST(N'2024-05-22' AS Date), 1)
INSERT [dbo].[Category] ([Id], [Ten], [NgayLapThe], [IsDeleted]) VALUES (N'Ts2YU', N'ssss', CAST(N'2024-05-22' AS Date), 1)
GO
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG001', N'LDOCGIA_A', CAST(N'2005-11-29' AS Date), N'12 Nguy?n Huy T?, phu?ng Ða Kao, qu?n 1, Tp. H? Chí Minh', N'thanhdangphan1510@gmail.com', CAST(N'2023-04-22' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG002', N'LDOCGIA_B', CAST(N'2007-07-01' AS Date), N'104 Hoàng Di?u, phu?ng 12, qu?n 4,Tp, H? Chí Minh', N'hungdung1234@gmail.com', CAST(N'2022-06-09' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG003', N'LDOCGIA_A', CAST(N'2004-01-19' AS Date), N'26 Lê Th? Riêng, phu?ng B?n Thành, qu?n 1, Tp. H? Chí Minh', N'dangphanthanh42@gmail.com', CAST(N'2022-12-13' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG004', N'LDOCGIA_A', CAST(N'2003-10-08' AS Date), N'823 Nguy?n Trãi, phu?ng.14, qu?n 5, Tp. H? Chí Minh', N'gmail1@gm.com', CAST(N'2024-01-02' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG005', N'LDOCGIA_C', CAST(N'1999-11-10' AS Date), N'101 V?n Ki?p, phu?ng 3, qu?n Bình Th?nh, Tp. H? Chí Minh', N'email2@gmail.com', CAST(N'2022-03-15' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG006', N'LDOCGIA_A', CAST(N'2004-10-24' AS Date), N'199 Vinh Vi?n, phu?ng 4, qu?n 10, Tp. H? Chí Minh', N'30000000@gm.uit.edu.vn', CAST(N'2022-11-19' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG007', N'LDOCGIA_A', CAST(N'2005-06-07' AS Date), N'40 Hu?nh T?n Phát, huy?n Nhà Bè, Tp. H? Chí Minh', N'22522252@gm.uit.edu.vn', CAST(N'2023-04-04' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG008', N'LDOCGIA_A', CAST(N'2005-10-19' AS Date), N'111/1A du?ng s? 8, phu?ng 11, qu?n Gò V?p, Tp. H? Chí Minh', N'23522352@gm.uit.edu.vn', CAST(N'2021-01-08' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG009', N'LDOCGIA_A', CAST(N'2004-04-24' AS Date), N'383 Vinh Khánh, phu?ng 8, qu?n 4, Tp. H? Chí Minh', N'24521000@gm.uit.edu.vn', CAST(N'2023-08-15' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG010', N'LDOCGIA_A', CAST(N'2007-07-18' AS Date), N'S? 37 Vinh Khánh, phu?ng 8, qu?n 4, Tp. H? Chí Minh', N'gmail_11@gmail.com', CAST(N'2023-12-27' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG011', N'LDOCGIA_A', CAST(N'2001-09-02' AS Date), N'210 Tô Hi?n Thành, phu?ng 15, qu?n 10, Tp. H? Chí Minh', N'gmail_12@gmail.com', CAST(N'2022-09-26' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG012', N'LDOCGIA_C', CAST(N'1998-12-30' AS Date), N'181/6 Xóm Chi?u, phu?ng 16, qu?n 4, Tp. H? Chí Minh', N'gmail_13@gmail.com', CAST(N'2021-07-18' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG013', N'LDOCGIA_C', CAST(N'1996-10-24' AS Date), N'720 CMT8, phu?ng .5, qu?n Tân Bình, Tp. H? Chí Minh', N'gmail_14@gmail.com', CAST(N'2024-05-19' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG014', N'LDOCGIA_C', CAST(N'1999-07-19' AS Date), N'453/50 Lê Van S?, phu?ng 12, qu?n 3, Tp. H? Chí Minh', N'gmail_15@gmail.com', CAST(N'2024-11-01' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG015', N'LDOCGIA_C', CAST(N'1999-08-15' AS Date), N'178/31 Nguyê~n Van Thuong, P.25, Q.Bi`nh Tha?nh, Tp.HCM', N'gmail_16@gmail.com', CAST(N'2022-05-03' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG016', N'LDOCGIA_A', CAST(N'2005-05-17' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_17@gmail.com', CAST(N'2023-05-27' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG017', N'LDOCGIA_A', CAST(N'2004-04-23' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_18@gmail.com', CAST(N'2024-03-27' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG018', N'LDOCGIA_A', CAST(N'2004-11-15' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_19@gmail.com', CAST(N'2023-08-19' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG019', N'LDOCGIA_A', CAST(N'2005-09-18' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_20@gmail.com', CAST(N'2022-07-10' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG020', N'LDOCGIA_A', CAST(N'2005-11-12' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_21@gmail.com', CAST(N'2024-05-29' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG021', N'LDOCGIA_A', CAST(N'2004-10-22' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_22@gmail.com', CAST(N'2023-03-07' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG022', N'LDOCGIA_A', CAST(N'2002-03-20' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_23@gmail.com', CAST(N'2024-03-24' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG023', N'LDOCGIA_A', CAST(N'2005-06-05' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_24@gmail.com', CAST(N'2022-05-01' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG024', N'LDOCGIA_A', CAST(N'2004-01-09' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_25@gmail.com', CAST(N'2023-04-03' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG025', N'LDOCGIA_A', CAST(N'2005-02-11' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_26@gmail.com', CAST(N'2023-11-04' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG026', N'LDOCGIA_A', CAST(N'2005-01-23' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_27@gmail.com', CAST(N'2024-07-03' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG027', N'LDOCGIA_A', CAST(N'2006-09-22' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_28@gmail.com', CAST(N'2024-05-30' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG028', N'LDOCGIA_A', CAST(N'2005-08-23' AS Date), N'du?ng T? Quang B?u, Khu ph? 6, P. Linh Trung, TP Th? Ð?c, TP H? Chí Minh', N'gmail_29@gmail.com', CAST(N'2021-07-27' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG029', N'LDOCGIA_C', CAST(N'1998-05-18' AS Date), N'116 Vu?n Chu?i, phu?ng 4, qu?n 3, Tp. H? Chí Minh', N'gmail_30@gmail.com', CAST(N'2024-03-04' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG030', N'LDOCGIA_C', CAST(N'1999-04-09' AS Date), N'174 Nguy?n Thi?n Thu?t, phu?ng 1, qu?n 3, Tp. H? Chí Minh', N'gmail_31@gmail.com', CAST(N'2022-09-07' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG031', N'LDOCGIA_C', CAST(N'2004-01-25' AS Date), N'Duong T? Giang, phu?ng 14, qu?n 5, Tp. H? Chí Minh', N'gmail_32@gmail.com', CAST(N'2023-10-16' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG032', N'LDOCGIA_C', CAST(N'2000-11-05' AS Date), N'192 - 194 Nguy?n Tri Phuong, phu?ng 4, qu?n 10, Tp. H? Chí Minh', N'gmail_33@gmail.com', CAST(N'2021-11-04' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG033', N'LDOCGIA_A', CAST(N'2005-04-19' AS Date), N'Tô Vinh Di?n, Ðông Hoà, Di An, Bình Duong', N'gmail_34@gmail.com', CAST(N'2021-10-23' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG034', N'LDOCGIA_D', CAST(N'2002-11-07' AS Date), N'84/1 H? Th? K?, phu?ng 1, qu?n 10, Tp. H? Chí Minh', N'gmail_35@gmail.com', CAST(N'2024-01-24' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG035', N'LDOCGIA_D', CAST(N'2003-03-02' AS Date), N'H?m 372 Cách M?ng Tháng 8, phu?ng 10, qu?n 3, Tp. H? Chí Minh', N'gmail_36@gmail.com', CAST(N'2023-04-20' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG036', N'LDOCGIA_A', CAST(N'2003-01-20' AS Date), N'Tô Vinh Di?n, Ðông Hoà, Di An, Bình Duong', N'gmail_37@gmail.com', CAST(N'2023-10-16' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG037', N'LDOCGIA_C', CAST(N'1998-08-09' AS Date), N'1-3 Cao Bá Nh?, phu?ng Nguy?n Cu Trinh, qu?n 1, Tp. H? Chí Minh', N'gmail_38@gmail.com', CAST(N'2022-07-07' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG038', N'LDOCGIA_C', CAST(N'1997-03-02' AS Date), N'105 Truong Ð?nh, phu?ng 6, qu?n 3, Tp. H? Chí Minh', N'gmail_39@gmail.com', CAST(N'2023-08-16' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG039', N'LDOCGIA_A', CAST(N'2004-10-02' AS Date), N'Tô Vinh Di?n, Ðông Hoà, Di An, Bình Duong', N'gmail_40@gmail.com', CAST(N'2024-02-27' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG040', N'LDOCGIA_E', CAST(N'2006-10-23' AS Date), N'436 Nguy?n Th? Th?p, phu?ng Tân Quy, qu?n 7, Tp. H? Chí Minh', N'gmail_41@gmail.com', CAST(N'2022-11-20' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG041', N'LDOCGIA_C', CAST(N'1998-12-30' AS Date), N'31 Yên Th?, phu?ng 2, qu?n Tân Bình, Tp. H? Chí Minh', N'gmail_42@gmail.com', CAST(N'2022-04-16' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG042', N'LDOCGIA_D', CAST(N'2002-02-05' AS Date), N'99 Châu Van Liêm, phu?ng 14, qu?n 5, Tp. H? Chí Minh', N'gmail_43@gmail.com', CAST(N'2021-03-06' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG043', N'LDOCGIA_C', CAST(N'2000-08-02' AS Date), N'178/31 Nguyê~n Van Thuong, P.25, Q.Bi`nh Tha?nh, Tp.HCM', N'gmail_44@gmail.com', CAST(N'2023-10-27' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG044', N'LDOCGIA_C', CAST(N'2001-06-15' AS Date), N'178/31 Nguyê~n Van Thuong, P.25, Q.Bi`nh Tha?nh, Tp.HCM', N'gmail_45@gmail.com', CAST(N'2021-01-03' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG045', N'LDOCGIA_C', CAST(N'2001-06-26' AS Date), N'178/31 Nguyê~n Van Thuong, P.25, Q.Bi`nh Tha?nh, Tp.HCM', N'gmail_46@gmail.com', CAST(N'2022-07-20' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG046', N'LDOCGIA_C', CAST(N'1997-06-19' AS Date), N'178/31 Nguyê~n Van Thuong, P.25, Q.Bi`nh Tha?nh, Tp.HCM', N'gmail_47@gmail.com', CAST(N'2023-04-24' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG047', N'LDOCGIA_C', CAST(N'2000-12-10' AS Date), N'178/31 Nguyê~n Van Thuong, P.25, Q.Bi`nh Tha?nh, Tp.HCM', N'gmail_48@gmail.com', CAST(N'2022-01-03' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG048', N'LDOCGIA_A', CAST(N'2004-07-23' AS Date), N'Tô Vinh Di?n, Ðông Hoà, Di An, Bình Duong', N'gmail_49@gmail.com', CAST(N'2022-08-19' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG049', N'LDOCGIA_A', CAST(N'2003-08-20' AS Date), N'Tô Vinh Di?n, Ðông Hoà, Di An, Bình Duong', N'gmail_50@gmail.com', CAST(N'2022-09-26' AS Date), 0)
INSERT [dbo].[DOCGIA] ([MaDG], [LoaiDG], [NgaySinh], [DiaChi], [Email], [NgayLapThe], [IsDeleted]) VALUES (N'DG050', N'LDOCGIA_A', CAST(N'2004-11-26' AS Date), N'Tô Vinh Di?n, Ðông Hoà, Di An, Bình Duong', N'gmail_51@gmail.com', CAST(N'2024-07-04' AS Date), 0)
GO
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'4Q5wC', N'DG009', N'SH003', CAST(N'2024-05-24' AS Date), CAST(N'2024-06-23' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'5hATx', N'DG007', N'SH002', CAST(N'2024-05-24' AS Date), CAST(N'2024-06-23' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'noTKL', N'DG001', N'SH001', CAST(N'2024-05-23' AS Date), CAST(N'2024-06-22' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'ozk7S', N'DG034', N'SH003', CAST(N'2024-05-24' AS Date), CAST(N'2024-06-23' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM001', N'DG036', N'SH004', CAST(N'2024-05-24' AS Date), CAST(N'2024-06-23' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM002', N'DG008', N'SH004', CAST(N'2024-05-24' AS Date), CAST(N'2024-06-23' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM003', N'DG001', N'SH001', CAST(N'2024-03-25' AS Date), CAST(N'2024-04-24' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM004', N'DG002', N'SH002', CAST(N'2024-03-19' AS Date), CAST(N'2024-04-18' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM005', N'DG003', N'SH006', CAST(N'2024-03-26' AS Date), CAST(N'2024-04-25' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM006', N'DG005', N'SH007', CAST(N'2024-02-19' AS Date), CAST(N'2024-03-20' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM007', N'DG004', N'SH002', CAST(N'2024-03-01' AS Date), CAST(N'2024-03-31' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM008', N'DG001', N'SH003', CAST(N'2024-03-25' AS Date), CAST(N'2024-04-24' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM009', N'DG003', N'SH001', CAST(N'2024-03-26' AS Date), CAST(N'2024-04-25' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'PM010', N'DG005', N'SH003', CAST(N'2024-03-26' AS Date), CAST(N'2024-04-25' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'VxCaB', N'DG003', N'SH003', CAST(N'2024-05-24' AS Date), CAST(N'2024-06-23' AS Date), 0)
INSERT [dbo].[PHIEUMUON] ([MaPhMuon], [MaDG], [MaSach], [NgayMuon], [NgayPhTra], [IsDeleted]) VALUES (N'wiP7V', N'DG002', N'SH004', CAST(N'2024-05-20' AS Date), CAST(N'2024-06-19' AS Date), 0)
GO
SET IDENTITY_INSERT [dbo].[PHIEUTHU] ON 

INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (1, N'PT001', 193, 965000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (2, N'PT003', 209, 1045000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (3, N'PT005', 9, 45000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (4, N'PT006', 33, 165000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (5, N'PT001', 27, 135000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (6, N'PT004', 28, 140000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (7, N'I4tFE', 55, 275000)
INSERT [dbo].[PHIEUTHU] ([ID], [MaPhTra], [SoNgayQHan], [SoTienThu]) VALUES (8, N'PT002', 29, 145000)
SET IDENTITY_INSERT [dbo].[PHIEUTHU] OFF
GO
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'ASST2', N'wiP7V', CAST(N'2024-05-20' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'I4tFE', N'PM007', CAST(N'2024-05-25' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT001', N'PM008', CAST(N'2024-05-21' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT002', N'PM005', CAST(N'2024-05-24' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT003', N'PM008', CAST(N'2024-11-19' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT004', N'PM005', CAST(N'2024-05-23' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT005', N'PM006', CAST(N'2024-03-29' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT006', N'PM001', CAST(N'2024-05-22' AS Date), 0)
INSERT [dbo].[PHIEUTRA] ([MaPhTra], [MaPhMuon], [NgayTra], [IsDeleted]) VALUES (N'PT007', N'PM004', CAST(N'2024-04-01' AS Date), 0)
GO
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH001', N'Ten sach 1', N'Giao trinh', N'Tac gia 1', 2000, N'DHQG HCM', CAST(N'2001-02-24' AS Date), 75000, 1, 0)
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH002', N'Ten sach 2', N'Sach tham khao', N'Tac gia 2', 2003, N'DHQG HCM', CAST(N'2005-03-15' AS Date), 100000, 1, 0)
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH003', N'Ten sach 3', N'Truyen tranh', N'Tac gia 3', 2015, N'Nha xuat ban 1', CAST(N'2019-10-14' AS Date), 50000, 1, 0)
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH004', N'Ten sach 4', N'Giao trinh', N'Tac gia 4', 2012, N'DHQG HA NOI', CAST(N'2017-03-23' AS Date), 80000, 1, 0)
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH005', N'Ten sach 5', N'Giao trinh', N'Tac gia 5', 2012, N'DHQG HCM', CAST(N'2017-10-24' AS Date), 85000, 1, 0)
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH006', N'Ten sach 6', N'Sach tham khao', N'Tac gia 6', 2017, N'DHQG HCM', CAST(N'2020-12-12' AS Date), 90000, 1, 0)
INSERT [dbo].[SACH] ([MaSach], [TenSach], [TheLoai], [TacGia], [NamXB], [NhaXB], [NgayNhap], [TriGia], [TinhTrang], [IsDeleted]) VALUES (N'SH007', N'Ten sach 7', N'Giao trinh', N'Tac gia 7', 2018, N'DHQG HA NOI', CAST(N'2019-03-03' AS Date), 90000, 1, 0)
GO
ALTER TABLE [dbo].[ACCOUNT] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[Category] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[DOCGIA] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[PARAMETERs] ADD  DEFAULT ((0)) FOR [IDDocGia]
GO
ALTER TABLE [dbo].[PARAMETERs] ADD  DEFAULT ((0)) FOR [IDSach]
GO
ALTER TABLE [dbo].[PARAMETERs] ADD  DEFAULT ((0)) FOR [IDPhMuon]
GO
ALTER TABLE [dbo].[PARAMETERs] ADD  DEFAULT ((0)) FOR [IDPhThu]
GO
ALTER TABLE [dbo].[PARAMETERs] ADD  DEFAULT ((5000)) FOR [IDPhTra]
GO
ALTER TABLE [dbo].[PARAMETERs] ADD  DEFAULT ((30)) FOR [SoNgMuonTD]
GO
ALTER TABLE [dbo].[PHIEUMUON] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[PHIEUTRA] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[SACH] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[ACCOUNT]  WITH CHECK ADD FOREIGN KEY([MaDG])
REFERENCES [dbo].[DOCGIA] ([MaDG])
GO
ALTER TABLE [dbo].[PHIEUMUON]  WITH CHECK ADD FOREIGN KEY([MaDG])
REFERENCES [dbo].[DOCGIA] ([MaDG])
GO
ALTER TABLE [dbo].[PHIEUMUON]  WITH CHECK ADD FOREIGN KEY([MaSach])
REFERENCES [dbo].[SACH] ([MaSach])
GO
ALTER TABLE [dbo].[PHIEUTHU]  WITH CHECK ADD FOREIGN KEY([MaPhTra])
REFERENCES [dbo].[PHIEUTRA] ([MaPhTra])
GO
ALTER TABLE [dbo].[PHIEUTRA]  WITH CHECK ADD FOREIGN KEY([MaPhMuon])
REFERENCES [dbo].[PHIEUMUON] ([MaPhMuon])
GO
