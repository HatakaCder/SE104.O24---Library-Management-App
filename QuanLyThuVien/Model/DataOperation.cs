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
using System.Diagnostics.Eventing.Reader;
using System.Media;
using QuanLyThuVien.View;

namespace QuanLyThuVien.Model
{
    public class DataOperation
    {
        QLTV_BETAEntities ObjContext;
        // Phục vụ cho việc tìm kiếm sách, độc giả thay vì cứ mỗi lần gọi là phải load từ CSDL toàn bộ danh sách
        private static List<READER> ObjReadersList;
        private static List<BOOK> ObjBooksList;

        public DataOperation()
        {
            ObjContext = new QLTV_BETAEntities();
            ObjReadersList = new List<READER>();
            ObjBooksList = new List<BOOK>();
        }

        public List<string> getAllBookTypes()
        {
            List<string> bookTypes = new List<string>();
            foreach (var book_type in ObjContext.THELOAI)
            {
                bookTypes.Add(book_type.TenTheLoai);
            }
            return bookTypes;
        }
        public List<BOOK> getAllBook()
        {
            ObjBooksList.Clear();

            try
            {
                var ObjQuery = from sach in ObjContext.SACH select sach;

                foreach (var sach in ObjQuery)
                {
                    if ((bool)sach.IsDeleted) continue;
                    ObjBooksList.Add(new BOOK
                    {
                        MaSach = sach.MaSach,
                        TenSach = sach.TenSach,
                        TacGia = sach.TacGia,
                        NamXB = (short)sach.NamXB,
                        TheLoai = sach.TenTheLoai,
                        NhaXB = sach.NhaXB,
                        TriGia = (int)sach.TriGia,
                        NgayNhap = DateTime.Parse(sach.NgayNhap.ToString()),
                        TinhTrang = (short)sach.TinhTrang,
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjBooksList;
        }
        public List<BOOK> getSelectedBooks()
        {
            List<BOOK> books;
            books = ObjBooksList.FindAll(x => x.IsSelected);
            return books;
        }

        public List<READER> getAllSelectedReaders()
        {
            List<READER> readers;
            readers = ObjReadersList.FindAll(x => x.IsSelected);
            return readers;
        }

        #region Check_Book_Information
        private int CheckInformation(BOOK objBook)
        {
            if (objBook.NhaXB == null || objBook.TheLoai == null || objBook.TenSach == null || objBook.TacGia == null) return 6;
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
            string NgayNhap = objBook.NgayNhap.ToString(formatString);
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            DateTime parsedDateTime;

            if (DateTime.TryParseExact(NgayNhap, formatString, cultureInfo,
                                         DateTimeStyles.None, out parsedDateTime) == false)
                return 5;

            return 0;
        }
        #endregion

        #region AddBook_Operation

        public bool Add_Book(BOOK objBook)
        {
            bool IsAdded = false;
            objBook.NgayNhap = DateTime.Now;

            #region Check_Book_Information
            int check = CheckInformation(objBook);

            switch (check)
            {
                case 1:
                    System.Windows.MessageBox.Show("Năm xuất bản phải bé hơn hoặc bằng năm nhập!");
                    return false;
                case 2:
                    System.Windows.MessageBox.Show("Ngày nhập phải bé hơn hoặc bằng ngày hiện tại!");
                    return false;
                case 3:
                    System.Windows.MessageBox.Show("Năm xuất bản phải là số nguyên dương!");
                    return false;
                case 4:
                    System.Windows.MessageBox.Show("Trị giá phải là số nguyên dương!");
                    return false;
                case 5:
                    System.Windows.MessageBox.Show("Định dạng kiểu ngày tháng chưa đúng!");
                    return false;
                case 6:
                    System.Windows.MessageBox.Show("Vui lòng điền đẩy đủ thông tin để thêm sách!");
                    return false;
            }
            #endregion

            #region Insert_new_book_into_database
            try
            {
                var ObjBook = new SACH();
                ObjBook.TenSach = objBook.TenSach;
                ObjBook.TenTheLoai = objBook.TheLoai;
                ObjBook.TacGia = objBook.TacGia;
                ObjBook.NamXB = objBook.NamXB;
                ObjBook.NhaXB = objBook.NhaXB;
                ObjBook.NgayNhap = objBook.NgayNhap;
                ObjBook.TriGia = objBook.TriGia;
                ObjBook.TinhTrang = 1;
                ObjBook.IsDeleted = false;
                objBook.IsSelected = false;

                var ObjQuery = from IdItems in ObjContext.PARAMETERS select IdItems;

                foreach (var IDItems in ObjQuery)
                {
                    string idSach = IDItems.IDSach.ToString();
                    while (idSach.Length < 3) idSach = '0' + idSach;

                    ObjBook.MaSach = "SS" + idSach;
                    objBook.MaSach = "SS" + idSach;

                    break;
                }

                ObjContext.SACH.Add(ObjBook);
                ObjBooksList.Add(objBook);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;

                // Update IDSach in PARAMETERS table
                var para = ObjContext.PARAMETERS.Find(1);
                para.IDSach += 1;
                ObjContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            return IsAdded;
        }

        #endregion

        #region SearchBook_Operation
        public BOOK Search_Book(string id)
        {
            BOOK book = null;

            try
            {
                var BookToFind = ObjContext.SACH.Find(id);

                if (BookToFind != null)
                {
                    book = new BOOK()
                    {
                        MaSach = BookToFind.MaSach,
                        TenSach = BookToFind.TenSach,
                        TheLoai = BookToFind.TenTheLoai,
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

        public List<BOOK> search_book_by_TenSach(string tensach)
        {
            List<BOOK> books;
            books = ObjBooksList.FindAll(x => x.TenSach.ToLower().Contains(tensach.ToLower()));
            return books;
        }

        public List<BOOK> search_book_by_TheLoai(string theloai)
        {
            List<BOOK> books;
            books = ObjBooksList.FindAll(x => x.TheLoai.ToLower().Contains(theloai.ToLower()));
            return books;
        }

        public List<BOOK> search_book_by_TacGia(string tacgia)
        {
            List<BOOK> books;
            books = ObjBooksList.FindAll(x => x.TacGia.ToLower().Contains(tacgia.ToLower()));
            return books;
        }

        public List<BOOK> search_available_book()
        {
            List<BOOK> books;
            books = ObjBooksList.FindAll(x => x.TinhTrang == 1);
            return books;
        }
        #endregion

        #region BookDelete_Operation
        public bool Delete_Book(List<BOOK> need_to_delete)
        {
            bool IsDeleted = false;

            try
            {
                foreach (var book in need_to_delete)
                {
                    var objBookToDelete = ObjContext.SACH.Find(book.MaSach);
                    objBookToDelete.IsDeleted = true;
                    ObjBooksList.Remove(book);
                }
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

        #region UpdateBook_Operation

        public bool update(BOOK objBookToUpdate)
        {
            bool IsUpdated = false;

            #region Check_Book_Information
            int check = CheckInformation(objBookToUpdate);

            switch (check)
            {
                case 1:
                    System.Windows.MessageBox.Show("Năm xuất bản phải bé hơn hoặc bằng năm nhập!");
                    return false;
                case 2:
                    System.Windows.MessageBox.Show("Ngày nhập phải bé hơn hoặc bằng ngày hiện tại!");
                    return false;
                case 3:
                    System.Windows.MessageBox.Show("Năm xuất bản phải là số nguyên dương!");
                    return false;
                case 4:
                    System.Windows.MessageBox.Show("Trị giá phải là số nguyên dương!");
                    return false;
                case 5:
                    System.Windows.MessageBox.Show("Định dạng kiểu ngày tháng chưa đúng!");
                    return false;
                case 6:
                    System.Windows.MessageBox.Show("Vui lòng điền đẩy đủ thông tin để thêm sách!");
                    return false;
            }
            #endregion

            try
            {
                var sach = ObjContext.SACH.Find(objBookToUpdate.MaSach);

                if (sach != null)
                {
                    sach.TenSach = objBookToUpdate.TenSach;
                    sach.TenTheLoai = objBookToUpdate.TheLoai;
                    sach.TacGia = objBookToUpdate.TacGia;
                    sach.NamXB = objBookToUpdate.NamXB;
                    sach.NhaXB = objBookToUpdate.NhaXB;
                    sach.NgayNhap = objBookToUpdate.NgayNhap;
                    sach.TriGia = objBookToUpdate.TriGia;
                    sach.TinhTrang = objBookToUpdate.TinhTrang;

                    var NoOfRowAffected = ObjContext.SaveChanges();
                    IsUpdated = NoOfRowAffected > 0;

                    // Update book in ObjBooksList
                    int index = ObjBooksList.FindIndex(x => x.MaSach == objBookToUpdate.MaSach);
                    ObjBooksList[index].TenSach = objBookToUpdate.TenSach;
                    ObjBooksList[index].TheLoai = objBookToUpdate.TheLoai;
                    ObjBooksList[index].TacGia = objBookToUpdate.TacGia;
                    ObjBooksList[index].NamXB = objBookToUpdate.NamXB;
                    ObjBooksList[index].NhaXB = objBookToUpdate.NhaXB;
                    ObjBooksList[index].NgayNhap = objBookToUpdate.NgayNhap;
                    ObjBooksList[index].TriGia = objBookToUpdate.TriGia;
                    ObjBooksList[index].TinhTrang = objBookToUpdate.TinhTrang;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return IsUpdated;
        }

        #endregion

        public List<READER> getAllReader()
        {
            ObjReadersList.Clear();

            try
            {
                var ObjQuery = from reader in ObjContext.DOCGIA select reader;

                foreach (var reader in ObjQuery)
                {
                    if ((bool)reader.IsDeleted) continue;

                    ObjReadersList.Add(new READER
                    {
                        MaDG = reader.MaDG,
                        HoTen = reader.HoTen,
                        GioiTinh = reader.GioiTinh,
                        NgaySinh = DateTime.Parse(reader.NgaySinh.ToString()),
                        DiaChi = reader.DiaChi,
                        Email = reader.Email,
                        SoDT = reader.SoDT,
                        NgayLapThe = DateTime.Parse(reader.NgayLapThe.ToString()),
                        IsSelected = false
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ObjReadersList;
        }

        #region Check_Reader_Information
        private int CheckInformation(READER objReader)
        {
            if (objReader.HoTen == null || objReader.GioiTinh == null || objReader.NgaySinh == null || objReader.SoDT == null || objReader.Email == null) return 7;

            // Giới tính phải thuộc nam hoặc nữ
            if (objReader.GioiTinh != "Nam" && objReader.GioiTinh != "Nữ" && objReader.GioiTinh != "nam" && objReader.GioiTinh != "nữ") return 1;

            // Kiểm tra định dạng ngày
            string formatString = "yyyy-MM-dd";
            string NgaySinh = objReader.NgaySinh.ToString(formatString);
            string NgayLapThe = objReader.NgayLapThe.ToString(formatString);
            CultureInfo cultureInfo = CultureInfo.InvariantCulture;
            DateTime parsedDateTime;

            // Ngày sinh
            if (DateTime.TryParseExact(NgaySinh, formatString, cultureInfo,
                                         DateTimeStyles.None, out parsedDateTime) == false)
                return 2;
            // Ngày lập thẻ
            if (DateTime.TryParseExact(NgayLapThe, formatString, cultureInfo,
                                         DateTimeStyles.None, out parsedDateTime) == false)
                return 3;

            // Kiểm tra độ tuổi
            int Age = DateTime.Now.Year - objReader.NgaySinh.Year;

            var para = ObjContext.SETTING.FirstOrDefault();
            int maxAge = (int)para.TuoiToiDaDocGia, minAge = (int)para.TuoiToiTieuDocGia;

            if (Age < minAge || Age > maxAge) return 4;

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
            List<string> ValidNum = new List<string>() { "032", "033", "034", "035", "036", "037", "038", "039", "096", "097", "098", "086", "083", "084", "085", "081", "082", "088", "091", "094", "070", "079", "077", "076", "078", "090", "093", "089", "056", "058", "092", "059", "099" };

            if (objReader.SoDT == null || ValidNum.Find(x => x == objReader.SoDT.Substring(0, 3)) == null) return 6;

            return 0;
        }
        #endregion

        #region AddReader_Operation

        public bool Add_Reader(READER objReader)
        {
            bool IsAdded = false;

            #region Check_Reader_Information
            objReader.NgayLapThe = DateTime.Now;
            int check = CheckInformation(objReader);

            switch (check)
            {
                case 1:
                    System.Windows.MessageBox.Show("Giới tính chưa hợp lệ!");
                    return false;
                case 2:
                    System.Windows.MessageBox.Show("Định dạng ngày sinh chưa hợp lệ!");
                    return false;
                case 3:
                    System.Windows.MessageBox.Show("Định dạng ngày lập thẻ chưa hợp lệ!");
                    return false;
                case 4:
                    System.Windows.MessageBox.Show("Độ tuổi đăng ký thẻ độc giả không đúng với quy định!");
                    return false;
                case 5:
                    System.Windows.MessageBox.Show("Định dạng email chưa hợp lệ!");
                    return false;
                case 6:
                    System.Windows.MessageBox.Show("Số điện thoại chưa hợp lệ!");
                    return false;
                case 7:
                    System.Windows.MessageBox.Show("Vui lòng điền đầy đủ thông tin để thêm độc giả!");
                    return false;
            }
            #endregion

            #region Insert_new_reader_into_database
            try
            {
                var ObjReader = new DOCGIA();
                ObjReader.HoTen = objReader.HoTen;
                ObjReader.GioiTinh = objReader.GioiTinh;
                ObjReader.DiaChi = objReader.DiaChi;
                ObjReader.Email = objReader.Email;
                ObjReader.NgayLapThe = objReader.NgayLapThe;
                ObjReader.SoDT = objReader.SoDT;
                ObjReader.NgaySinh = objReader.NgaySinh;
                ObjReader.IsDeleted = false;
                objReader.IsSelected = false;

                var ObjQuery = from IdItems in ObjContext.PARAMETERS select IdItems;

                foreach (var IDItems in ObjQuery)
                {
                    string idDocgia = IDItems.IDDocGia.ToString();
                    while (idDocgia.Length < 3) idDocgia = '0' + idDocgia;

                    ObjReader.MaDG = "DG" + idDocgia;
                    objReader.MaDG = "DG" + idDocgia;
                    break;
                }

                ObjContext.DOCGIA.Add(ObjReader);
                ObjReadersList.Add(objReader);

                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;

                // Update IDDocGia in PARAMETERS table
                var para = ObjContext.PARAMETERS.Find(1);
                para.IDDocGia += 1;
                ObjContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            return IsAdded;
        }

        #endregion

        #region SearchReader_Operation
        public READER Search_Reader(string id)
        {
            READER reader = null;

            try
            {
                var ReaderToFind = ObjContext.DOCGIA.Find(id);

                if (ReaderToFind != null)
                {
                    reader = new READER()
                    {
                        HoTen = ReaderToFind.HoTen,
                        GioiTinh = ReaderToFind.GioiTinh,
                        DiaChi = ReaderToFind.DiaChi,
                        Email = ReaderToFind.Email,
                        NgayLapThe = (DateTime)ReaderToFind.NgayLapThe,
                        //AnhDaiDien = ReaderToFind.AnhDaiDien,
                        SoDT = ReaderToFind.SoDT,
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

        public List<READER> search_reader_by_Name(string Name)
        {
            List<READER> readers;
            readers = ObjReadersList.FindAll(x => x.HoTen.ToLower().Contains(Name.ToLower()));
            return readers;
        }

        public List<READER> search_reader_by_NgayLapThe(string NgayLapThe)
        {
            List<READER> readers;
            readers = ObjReadersList.FindAll(x => x.NgayLapThe.ToString().ToLower().Contains(NgayLapThe.ToLower()));
            return readers;
        }
        #endregion

        #region UpdateReader_Operation

        public bool update(READER objReaderToUpdate)
        {
            bool IsUpdated = false;

            #region Check_Reader_Information
            int check = CheckInformation(objReaderToUpdate);

            switch (check)
            {
                case 1:
                    System.Windows.MessageBox.Show("Giới tính chưa hợp lệ!");
                    return false;
                case 2:
                    System.Windows.MessageBox.Show("Định dạng ngày sinh chưa hợp lệ!");
                    return false;
                case 3:
                    System.Windows.MessageBox.Show("Định dạng ngày lập thẻ chưa hợp lệ!");
                    return false;
                case 4:
                    System.Windows.MessageBox.Show("Chưa đủ tuổi đăng ký thẻ độc giả!");
                    return false;
                case 5:
                    System.Windows.MessageBox.Show("Định dạng email chưa hợp lệ!");
                    return false;
                case 6:
                    System.Windows.MessageBox.Show("Số điện thoại chưa hợp lệ!");
                    return false;
                case 7:
                    System.Windows.MessageBox.Show("Vui lòng điền đầy đủ thông tin để thêm độc giả!");
                    return false;
            }
            #endregion

            try
            {
                DOCGIA docgia = ObjContext.DOCGIA.Find(objReaderToUpdate.MaDG);

                if (docgia != null)
                {
                    docgia.HoTen = objReaderToUpdate.HoTen;
                    docgia.GioiTinh = objReaderToUpdate.GioiTinh;
                    docgia.NgaySinh = objReaderToUpdate.NgaySinh;
                    docgia.DiaChi = objReaderToUpdate.DiaChi;
                    docgia.Email = objReaderToUpdate.Email;
                    docgia.SoDT = objReaderToUpdate.SoDT;
                    docgia.NgayLapThe = objReaderToUpdate.NgayLapThe;

                    var NoOfRowsAffected = ObjContext.SaveChanges();
                    IsUpdated = NoOfRowsAffected > 0;

                    // Update reader in ObjReadersList
                    int index = ObjReadersList.FindIndex(x => x.MaDG == objReaderToUpdate.MaDG);
                    ObjReadersList[index].HoTen = objReaderToUpdate.HoTen;
                    ObjReadersList[index].GioiTinh = objReaderToUpdate.GioiTinh;
                    ObjReadersList[index].NgaySinh = objReaderToUpdate.NgaySinh;
                    ObjReadersList[index].DiaChi = objReaderToUpdate.DiaChi;
                    ObjReadersList[index].Email = objReaderToUpdate.Email;
                    ObjReadersList[index].SoDT = objReaderToUpdate.SoDT;
                    ObjReadersList[index].NgayLapThe = objReaderToUpdate.NgayLapThe;
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
        public bool Delete_Reader(List<READER> need_to_delete)
        {
            bool IsDeleted = false;

            try
            {
                foreach (var reader in need_to_delete)
                {
                    var objReaderToDelete = ObjContext.DOCGIA.Find(reader.MaDG);
                    objReaderToDelete.IsDeleted = true;
                    ObjReadersList.Remove(reader);
                }
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

    }
}