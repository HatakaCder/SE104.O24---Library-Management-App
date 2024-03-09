create database qltv_beta

use qltv_beta

create table docgia (
	madg varchar(5) not null primary key,
	loaidg varchar(255),
	ngaysinh date,
	diachi varchar(255),
	email varchar(255),
	ngaylapthe date
);

create table sach (
	masach varchar(5) not null primary key,
	tensach varchar(255),
	theloai varchar(255),
	tacgia varchar(255),
	namxb smallint,
	nhaxb varchar(255),
	ngaynhap date,
	trigia int
);

create table phieumuon (
	maphmuon varchar(5),
	madocgia varchar(5),
	ngaymuon date
);

create table chitietphmuon (
	maphmuon varchar(5),
	masaach varchar(5)
);

create table phieutra (
	maphtra varchar(5),
	madg varchar(5),
	ngaytra date
);

create table phieuthu (
	maphthu varchar(5),
	madg varchar(5)
);

create table account (
	taikhoan varchar(5),
	matkhau varchar(5)
);