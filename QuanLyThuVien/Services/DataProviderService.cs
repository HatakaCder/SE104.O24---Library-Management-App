using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;
using System.Configuration;
using System.Data;
using QuanLyThuVien.View;
using System.Data.Entity;
using QuanLyThuVien.ViewModel;
using QuanLyThuVien.Model;

namespace QuanLyThuVien.Services
{
    public class DataProviderService
    {
        QLTV_BETAEntities ObjContext;
        private static List<READER> ObjReadersList;
        private static List<BOOK> ObjBooksList;
        private static List<PHIEUMUON> Objphieumuon;
        private static List<PhieuTraDTO> Objphieutra;

        public DataProviderService()
        {
            ObjContext = new QLTV_BETAEntities();
            ObjReadersList = new List<READER>();
            ObjBooksList = new List<BOOK>();
            Objphieumuon = new List<PHIEUMUON>();
            Objphieutra = new List<PhieuTraDTO>();

        }
        #region LIBRARIAN

        public List<PhieuTraDTO> GetPHIEUTHUs()
        {
            Objphieutra = new List<PhieuTraDTO>();

            try
            {
                var ObjQuery = from phieutra in ObjContext.PHIEUTRAs
                               join phieumuon in ObjContext.PHIEUMUONs on phieutra.MaPhMuon equals phieumuon.MaPhMuon
                               join docgia in ObjContext.DOCGIAs on phieumuon.MaDG equals docgia.MaDG
                               join sach in ObjContext.SACHes on phieumuon.MaSach equals sach.MaSach
                               select new PhieuTraDTO
                               {
                                   MaPhTra = phieutra.MaPhTra,
                                   MaPhMuon = phieutra.MaPhMuon,
                                   NgayTra = (DateTime)phieutra.NgayTra,
                                   MaDG = docgia.MaDG,
                                   MaSach = sach.MaSach
                               };

                Objphieutra.AddRange(ObjQuery.ToList());
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving PHIEUTRAs.", ex);
            }

            return Objphieutra;
        }

        public List<THUTHU> GetTHUTHUs()
        {
            List<THUTHU> ObjThuthuList = new List<THUTHU>();
            try
            {
                var ObjQuery = from thuthu in ObjContext.THUTHUs select thuthu;
                foreach (var thuthu in ObjQuery)
                {
                    ObjThuthuList.Add(new THUTHU
                    {
                        MaTT = thuthu.MaTT,
                        NgaySinh = (DateTime)thuthu.NgaySinh,
                        DiaChi = thuthu.DiaChi,
                        Email = thuthu.Email,
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjThuthuList;
        }
        public bool Delete(int id)
        {
            bool IsDeleted = false;
            try
            {
                var ObjReaderToDelete = ObjContext.DOCGIAs.Find(id);
                ObjContext.DOCGIAs.Remove(ObjReaderToDelete);
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
        #region READER
        public List<READER> GetREADERs()
        {
            List<READER> ObjReaderList = new List<READER>();
            try
            {
                var ObjQuery = from reader in ObjContext.DOCGIAs select reader;
                foreach (var reader in ObjQuery)
                {
                    ObjReaderList.Add(new READER
                    {
                        MaDG = reader.MaDG,
                        HoTen = reader.HoTen,
                        GioiTinh = reader.GioiTinh,
                        NgaySinh = (DateTime)reader.NgaySinh,
                        DiaChi = reader.DiaChi,
                        Email = reader.Email,
                        NgayLapThe = (DateTime)reader.NgayLapThe,
                        SoDT = reader.SoDT,
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return ObjReaderList;
        }
        public List<ListQuaHanModel> GetListQuaHan()
        {
            List<ListQuaHanModel> ObjList = new List<ListQuaHanModel>();
            try
            {
                var x = ObjContext.SETTINGs.FirstOrDefault().SoTienNopTre;
                var query = from phieumuon in ObjContext.PHIEUMUONs
                            where phieumuon.NgayPhTra < DateTime.Now && phieumuon.IsDeleted == false
                            select new ListQuaHanModel
                            {
                                MaPhMuon = phieumuon.MaPhMuon,
                                MaDG = phieumuon.MaDG,
                                MaSach = phieumuon.MaSach,
                                DateQuaHan = DbFunctions.DiffDays(phieumuon.NgayPhTra, DateTime.Now) ?? 0,
                                TienPhat = x * DbFunctions.DiffDays(phieumuon.NgayPhTra, DateTime.Now) ?? 0
                            };
                ObjList.AddRange(query);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ObjList;
        }
        public List<BOOK> getAllBook()
        {
            ObjBooksList.Clear();

            try
            {
                var ObjQuery = from sach in ObjContext.SACHes select sach;

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

        public List<PHIEUMUON> GetPHIEUMUONs()
        {
            Objphieumuon.Clear();

            try
            {
                var ObjQuery = from phieumuon in ObjContext.PHIEUMUONs select phieumuon;

                foreach (var phieumuon in ObjQuery)
                {
                    if ((bool)phieumuon.IsDeleted) continue;
                    Objphieumuon.Add(new PHIEUMUON
                    {
                        MaPhMuon = phieumuon.MaPhMuon,
                        MaDG = phieumuon.MaDG,
                        MaSach = phieumuon.MaSach,
                        NgayMuon = (DateTime)phieumuon.NgayMuon,
                        NgayPhTra = (DateTime)phieumuon.NgayPhTra,
                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Objphieumuon;
        }

        public List<BOOK> getSelectedBooks()
        {
            List<BOOK> books;
            books = ObjBooksList.FindAll(x => x.IsSelected);
            return books;
        }
        public bool Add(READER objNewReader)
        {
            bool IsAdded = false;
            try
            {
                var ObjReader = new DOCGIA();
                ObjReader.MaDG = objNewReader.MaDG;
                ObjReader.HoTen = objNewReader.HoTen;
                ObjReader.GioiTinh = objNewReader.GioiTinh;
                ObjReader.NgaySinh = objNewReader.NgaySinh;
                ObjReader.DiaChi = objNewReader.DiaChi;
                ObjReader.Email = objNewReader.Email;
                ObjReader.NgayLapThe = objNewReader.NgayLapThe;
                ObjReader.SoDT = objNewReader.SoDT;

                ObjContext.DOCGIAs.Add(ObjReader);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsAdded;
        }

        public bool Update(READER objReaderToUpdate)
        {
            bool IsUpdated = false;
            try
            {
                var ObjReader = ObjContext.DOCGIAs.Find(objReaderToUpdate.MaDG);
                ObjReader.MaDG = objReaderToUpdate.MaDG;
                ObjReader.HoTen = objReaderToUpdate.HoTen;
                ObjReader.GioiTinh = objReaderToUpdate.GioiTinh;
                ObjReader.NgaySinh = objReaderToUpdate.NgaySinh;
                ObjReader.DiaChi = objReaderToUpdate.DiaChi;
                ObjReader.Email = objReaderToUpdate.Email;
                ObjReader.NgayLapThe = objReaderToUpdate.NgayLapThe;
                ObjReader.SoDT = objReaderToUpdate.SoDT;



                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsUpdated = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsUpdated;
        }

        public bool DeleteLb(int id)
        {
            bool IsDeleted = false;
            try
            {
                var ObjLbToDelete = ObjContext.THUTHUs.Find(id);
                ObjContext.THUTHUs.Remove(ObjLbToDelete);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsDeleted = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return IsDeleted;
        }

        public READER Search(int id)
        {
            READER ObjReader = null;
            try
            {
                var ObjReaderToFind = ObjContext.DOCGIAs.Find(id);
                if (ObjReaderToFind != null)
                {
                    ObjReader = new READER()
                    {
                        MaDG = ObjReaderToFind.MaDG,
                        HoTen = ObjReaderToFind.HoTen,
                        GioiTinh = ObjReaderToFind.GioiTinh,
                        NgaySinh = (DateTime)ObjReaderToFind.NgaySinh,
                        DiaChi = ObjReaderToFind.DiaChi,
                        Email = ObjReaderToFind.Email,
                        NgayLapThe = (DateTime)ObjReaderToFind.NgayLapThe,
                        SoDT = ObjReaderToFind.SoDT,
                    };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return ObjReader;
        }
        #endregion
    }
}