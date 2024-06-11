// using Q.Models;'
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Timers;
using System.ComponentModel;
using System.Reflection;
using System.Net.Mail;

namespace QuanLyThuVien.Model
{
    public class DataOperation
    {
        QLTV_BETAEntities ObjContext;
        private static List<READER> ObjReadersList;
        private static List<BOOK> ObjBooksList;

        public DataOperation()
        {
            ObjContext = new QLTV_BETAEntities();
            ObjReadersList = new List<READER>();
            ObjBooksList = new List<BOOK>();
        }

        #region getAllBook
        public List<BOOK> getAllBook()
        {
            List<BOOK> ObjBookList = new List<BOOK>();

            try
            {
                var ObjQuery = from sach in ObjContext.SACHes select sach;

                foreach (var sach in ObjQuery)
                {
                    ObjBookList.Add(new BOOK
                    {
                        MaSach = sach.MaSach,
                        TenSach = sach.TenSach,
                        TheLoai = sach.TheLoai,
                        TacGia = sach.TacGia,
                        NamXB = (short)sach.NamXB,
                        NhaXB = sach.NhaXB,
                        TriGia = (int)sach.TriGia,
                        NgayNhap = DateTime.Parse(sach.NgayNhap.ToString()),
                        TinhTrang = (short)sach.TinhTrang
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjBookList;
        }
        #endregion

        #region getAllReader
        public List<READER> getAllReader()
        {
            List<READER> ObjReadersList = new List<READER>();

            try
            {
                var ObjQuery = from reader in ObjContext.DOCGIAs select reader;

                foreach (var reader in ObjQuery)
                {
                    ObjReadersList.Add(new READER
                    {
                        MaDG = reader.MaDG,
                        HoTen = reader.HoTen,
                        LoaiDG = reader.LoaiDG,
                        GioiTinh = reader.GioiTinh,
                        NgaySinh = DateTime.Parse(reader.NgaySinh.ToString()),
                        DiaChi = reader.DiaChi,
                        Email = reader.Email,
                        SoDT = reader.SoDT,
                        NgayLapThe = DateTime.Parse(reader.NgayLapThe.ToString()),
                        SoCCCD = reader.SoCCCD,
                        //AnhDaiDien = reader.AnhDaiDien
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjReadersList;
        }
        #endregion

        #region Check_Reader_Information
        private int CheckInformation(READER objReader)
        {
            // Giới tính phải thuộc nam hoặc nữ
            if (objReader.GioiTinh != "Nam" && objReader.GioiTinh != "Nữ" && objReader.GioiTinh != "nam" && objReader.GioiTinh != "nữ") return 1;

            // Kiểm tra định dạng ngày
            string formatString = "yyyy-MM-dd";
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            DateTime parsedDateTime;

            // Ngày sinh
            if (DateTime.TryParseExact(objReader.NgaySinh.ToString(), formatString, cultureInfo,
                                         DateTimeStyles.None, out parsedDateTime) == false)
                return 2;
            // Ngày lập thẻ
            if (DateTime.TryParseExact(objReader.NgayLapThe.ToString(), formatString, cultureInfo,
                                         DateTimeStyles.None, out parsedDateTime) == false)
                return 3;

            // Kiểm tra độ tuổi
            int Age = DateTime.Now.Year - objReader.NgaySinh.Year;
            if (Age < 15) return 4;

            // Kiểm tra định dạng email
            try
            {
                MailAddress mailAddress = new MailAddress(objReader.Email);
            }
            catch (FormatException)
            {
                return 5;
            }

            // Kiểm tra số điện thoại
            List<string> ValidNum = new List<string>() {"032", "033", "034", "035", "036", "037", "038", "039", "096", "097", "098", "086", "083", "084", "085", "081", "082", "088", "091", "094", "070", "079", "077", "076", "078", "090", "093", "089", "056", "058", "092", "059", "099" };

            if (objReader.SoDT == null || ValidNum.Find(x => x == objReader.SoDT.Substring(0, 3)) == null) return 6;

            return 0;
        }
        #endregion

        #region AddReader_Operation

        public bool Add(READER objReader)
        {
            bool IsAdded = false;

            #region Check_Reader_Information
            int check = CheckInformation(objReader);

            switch (check)
            {
                case 1:
                    throw new ArgumentException("Giới tính chưa hợp lệ.");
                case 2:
                    throw new ArgumentException("Định dạng ngày sinh chưa hợp lệ.");
                case 3:
                    throw new ArgumentException("Định dạng ngày lập thẻ chưa hợp lệ.");
                case 4:
                    throw new ArgumentException("Chưa đủ tuổi đăng ký thẻ độc giả.");
                case 5:
                    throw new ArgumentException("Định dạng email chưa hợp lệ.");
                case 6:
                    throw new ArgumentException("Số điện thoại chưa hợp lệ.");
            }
            #endregion

            #region Insert_new_reader_into_database
            try
            {
                var ObjReader = new DOCGIA();
                ObjReader.MaDG = objReader.MaDG;
                ObjReader.HoTen = objReader.HoTen;
                ObjReader.LoaiDG = objReader.LoaiDG;
                ObjReader.GioiTinh = objReader.GioiTinh;
                ObjReader.DiaChi = objReader.DiaChi;
                ObjReader.Email = objReader.Email;
                ObjReader.NgayLapThe = objReader.NgayLapThe;
                //ObjReader.AnhDaiDien = objReader.AnhDaiDien;
                ObjReader.SoDT = objReader.SoDT;
                ObjReader.SoCCCD = objReader.SoCCCD;
                ObjReader.NgaySinh = objReader.NgaySinh;
                ObjContext.DOCGIAs.Add(ObjReader);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            return IsAdded;
        }

        #endregion

        #region Check_Book_Information
        private int CheckInformation(BOOK objBook)
        {
            // Năm xuất bản bé hơn hoặc bằng năm nhập
            int NamXB_Less_NamNhap = objBook.NgayNhap.Year - objBook.NamXB; // >= 0
            if (NamXB_Less_NamNhap < 0) return 1;
            
            // Ngày nhập phải nhỏ hơn hoặc bằng ngày hiện tại
            int NamXB_Less_Now = DateTime.Now.Subtract(objBook.NgayNhap).CompareTo(TimeSpan.Zero); // >= 0
            if (NamXB_Less_Now < 0) return 2;
            
            // Năm xuất bản là số nguyên dương
            float namXB = objBook.NamXB;
            if (namXB < 0 || int.Parse(namXB.ToString()) - namXB != 0) return 3;
            
            // Trị giá là số nguyên dương
            float triGia = objBook.TriGia;
            if (triGia < 0 || int.Parse(triGia.ToString()) - triGia != 0) return 4;
            
            // Kiểm tra định dạng ngày tháng
            string formatString = "yyyy-MM-dd";
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            DateTime parsedDateTime;

            if (DateTime.TryParseExact(objBook.NgayNhap.ToString(), formatString, cultureInfo,
                                         DateTimeStyles.None, out parsedDateTime) == false)
                return 5;
            
            return 0;
        }
        #endregion
        
        #region AddBook_Operation

        public bool Add(BOOK objBook)
        {
            bool IsAdded = false;

            #region Check_Book_Information
            int check = CheckInformation(objBook);

            switch (check) {
                case 1:
                    throw new ArgumentException("Năm xuất bản phải bé hơn hoặc bằng năm nhập.");
                case 2:
                    throw new ArgumentException("Ngày nhập phải bé hơn hoặc bằng ngày hiện tại.");
                case 3:
                    throw new ArgumentException("Năm xuất bản phải là số nguyên dương.");
                case 4:
                    throw new ArgumentException("Trị giá phải là số nguyên dương.");
                case 5:
                    throw new ArgumentException("Định dạng kiểu ngày tháng chưa đúng.");
            }
            #endregion

            #region Insert_new_book_into_database
            try
            {
                var ObjBook = new SACH();
                ObjBook.MaSach = objBook.MaSach;
                ObjBook.TenSach = objBook.TenSach;
                ObjBook.TheLoai = objBook.TheLoai;
                ObjBook.TacGia = objBook.TacGia;
                ObjBook.NamXB = objBook.NamXB;
                ObjBook.NhaXB = objBook.NhaXB;
                ObjBook.NgayNhap = objBook.NgayNhap;
                ObjBook.TinhTrang = objBook.TinhTrang;
                ObjContext.SACHes.Add(ObjBook);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            return IsAdded;
        }

        #endregion

        #region UpdateReader_Operation

        public bool update(READER objReaderToUpdate)
        {
            bool IsUpdated = false;

            try
            {
                DOCGIA docgia = ObjContext.DOCGIAs.Find(objReaderToUpdate.MaDG);

                if (docgia != null)
                {
                    docgia.HoTen = objReaderToUpdate.HoTen;
                    docgia.LoaiDG = objReaderToUpdate.LoaiDG;
                    docgia.GioiTinh = objReaderToUpdate.GioiTinh;
                    docgia.NgaySinh = objReaderToUpdate.NgaySinh;
                    docgia.DiaChi = objReaderToUpdate.DiaChi;
                    docgia.Email = objReaderToUpdate.Email;
                    docgia.SoDT = objReaderToUpdate.SoDT;
                    docgia.NgayLapThe = objReaderToUpdate.NgayLapThe;
                    docgia.SoCCCD = objReaderToUpdate.SoCCCD;

                    var NoOfRowsAffected = ObjContext.SaveChanges();
                    IsUpdated = NoOfRowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsUpdated;
        }
        #endregion

        #region UpdateBook_Operation

        public bool update(BOOK objBookToUpdate)
        {
            bool IsUpdated = false;

            try
            {
                var sach = ObjContext.SACHes.Find(objBookToUpdate.MaSach);

                if (sach != null)
                {
                    sach.TenSach = objBookToUpdate.TenSach;
                    sach.TheLoai = objBookToUpdate.TheLoai;
                    sach.TacGia = objBookToUpdate.TacGia;
                    sach.NamXB = objBookToUpdate.NamXB;
                    sach.NhaXB = objBookToUpdate.NhaXB;
                    sach.NgayNhap = objBookToUpdate.NgayNhap;
                    sach.TriGia = objBookToUpdate.TriGia;
                    sach.TinhTrang = objBookToUpdate.TinhTrang;

                    var NoOfRowAffected = ObjContext.SaveChanges();
                    IsUpdated = NoOfRowAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsUpdated;
        }

        #endregion

        #region ReaderDelete_Operation
        public bool Delete_Reader(string id)
        {
            bool IsDeleted = false;

            try
            {
                var objReaderToDelete = ObjContext.DOCGIAs.Find(id);
                ObjContext.DOCGIAs.Remove(objReaderToDelete);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsDeleted = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsDeleted;
        }
        #endregion

        #region BookDelete_Operation
        public bool Delete_Book(string id)
        {
            bool IsDeleted = false;
            
            try
            {
                var objReaderToDelete = ObjContext.SACHes.Find(id);
                ObjContext.SACHes.Remove(objReaderToDelete);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsDeleted = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsDeleted;
        }
        #endregion

        #region SearchReader_Operation
        public READER Search_Reader(string id)
        {
            READER reader = null;

            try
            {
                var ReaderToFind = ObjContext.DOCGIAs.Find(id);

                if (ReaderToFind != null)
                {
                    reader = new READER()
                    {
                        HoTen = ReaderToFind.HoTen,
                        LoaiDG = ReaderToFind.LoaiDG,
                        GioiTinh = ReaderToFind.GioiTinh,
                        DiaChi = ReaderToFind.DiaChi,
                        Email = ReaderToFind.Email,
                        NgayLapThe = (DateTime)ReaderToFind.NgayLapThe,
                        //AnhDaiDien = ReaderToFind.AnhDaiDien,
                        SoDT = ReaderToFind.SoDT,
                        SoCCCD = ReaderToFind.SoCCCD,
                        NgaySinh = (DateTime)ReaderToFind.NgaySinh
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return reader;
        }
        #endregion

        #region SearchBook_Operation
        public BOOK Search_Book(string id)
        {
            BOOK book = null;

            try
            {
                var BookToFind = ObjContext.SACHes.Find(id);

                if (BookToFind != null)
                {
                    book = new BOOK()
                    {
                        MaSach = BookToFind.MaSach,
                        TenSach = BookToFind.TenSach,
                        TheLoai = BookToFind.TheLoai,
                        TacGia = BookToFind.TacGia,
                        NamXB = (short)BookToFind.NamXB,
                        NhaXB = BookToFind.NhaXB,
                        NgayNhap = (DateTime)BookToFind.NgayNhap,
                        TinhTrang = (short)BookToFind.TinhTrang
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return book;
        }
        #endregion

        #region SearchBook_by_TheLoai
        public List<BOOK> search_book_by_TheLoai(string TheLoai)
        {
            List<BOOK> books = new List<BOOK>();

            books = ObjBooksList.FindAll(x => x.TheLoai == TheLoai);

            return books;
        }
        #endregion

        #region SearchBook_by_TenSach
        public List<BOOK> search_book_by_TenSach(string TenSach)
        {
            List<BOOK> books = new List<BOOK>();

            books = ObjBooksList.FindAll(x => x.TenSach.Contains(TenSach));

            return books;
        }
        #endregion

        #region Search_by_TacGia
        public List<BOOK> search_book_by_TacGia(string TacGia)
        {
            List<BOOK> books = new List<BOOK>();

            books = ObjBooksList.FindAll(x => x.TacGia == TacGia);

            return books;
        }
        #endregion
    }
}
