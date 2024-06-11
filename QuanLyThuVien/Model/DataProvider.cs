using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;
using System.Configuration;
using System.Data;
using QuanLyThuVien.View;
using QLTVDemo.Models;

namespace QuanLyThuVien.Model
{
    public class DataProvider
    {
        QLTV_BETAEntities1 ObjContext;

        public DataProvider()
        {
            ObjContext = new QLTV_BETAEntities1();
        }
        #region LIBRARIAN
        public List<LIBRARIAN> GetTHUTHUs()
        {
            List<LIBRARIAN> ObjThuthuList = new List<LIBRARIAN>();
            try
            {
                var ObjQuery = from thuthu in ObjContext.THUTHUs select thuthu;
                foreach(var thuthu in ObjQuery)
                {
                    ObjThuthuList.Add(new LIBRARIAN
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
                foreach(var reader in ObjQuery){
                    ObjReaderList.Add(new READER
                    {
                        MaDG = reader.MaDG,
                        HoTen = reader.HoTen,
                        LoaiDG = reader.LoaiDG,
                        GioiTinh = reader.GioiTinh,
                        NgaySinh = (DateTime)reader.NgaySinh,
                        DiaChi = reader.DiaChi,
                        Email = reader.Email,
                        NgayLapThe = (DateTime)reader.NgayLapThe,
                        SoDT = reader.SoDT,   
                        SoCCCD = reader.SoCCCD,
                        AnhDaiDien  = reader.AnhDaiDien,
                    });
                }

            }
            catch (Exception ex)
            {
                throw ex;
                    
            }
            return ObjReaderList;               
        }
        public bool Add(READER objNewReader)
        {
            bool IsAdded = false;
            try
            {
                var ObjReader = new DOCGIA();
                ObjReader.MaDG = objNewReader.MaDG;
                ObjReader.HoTen = objNewReader.HoTen;
                ObjReader.LoaiDG = objNewReader.LoaiDG;
                ObjReader.GioiTinh = objNewReader.GioiTinh;
                ObjReader.NgaySinh = objNewReader.NgaySinh;
                ObjReader.DiaChi = objNewReader.DiaChi;
                ObjReader.Email = objNewReader.Email;
                ObjReader.NgayLapThe = objNewReader.NgayLapThe;
                ObjReader.SoDT = objNewReader.SoDT;
                ObjReader.SoCCCD = objNewReader.SoCCCD;
                ObjReader.AnhDaiDien = objNewReader.AnhDaiDien;

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

        public  bool Update(READER objReaderToUpdate)
        {
            bool IsUpdated = false;
            try
            {
                var ObjReader = ObjContext.DOCGIAs.Find(objReaderToUpdate.MaDG);
                ObjReader.MaDG = objReaderToUpdate.MaDG;
                ObjReader.HoTen = objReaderToUpdate.HoTen;
                ObjReader.LoaiDG = objReaderToUpdate.LoaiDG;
                ObjReader.GioiTinh = objReaderToUpdate.GioiTinh;
                ObjReader.NgaySinh = objReaderToUpdate.NgaySinh;
                ObjReader.DiaChi = objReaderToUpdate.DiaChi;
                ObjReader.Email = objReaderToUpdate.Email;
                ObjReader.NgayLapThe = objReaderToUpdate.NgayLapThe;
                ObjReader.SoDT = objReaderToUpdate.SoDT;
                ObjReader.SoCCCD = objReaderToUpdate.SoCCCD;
                ObjReader.AnhDaiDien = objReaderToUpdate.AnhDaiDien;


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
                        LoaiDG = ObjReaderToFind.LoaiDG,
                        GioiTinh = ObjReaderToFind.GioiTinh,
                        NgaySinh = (DateTime)ObjReaderToFind.NgaySinh,
                        DiaChi = ObjReaderToFind.DiaChi,
                        Email = ObjReaderToFind.Email,
                        NgayLapThe = (DateTime)ObjReaderToFind.NgayLapThe,
                        SoDT = ObjReaderToFind.SoDT,
                        SoCCCD = ObjReaderToFind.SoCCCD,
                        AnhDaiDien = ObjReaderToFind.AnhDaiDien,
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
