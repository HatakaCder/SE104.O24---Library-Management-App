using QuanLyThuVien.Class;
using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace QuanLyThuVien.Services
{
    public class LibrarianService
    {
        QLTV_BETAEntities ObjContext;
        List<LibrarianDTO> ObjLibrariansList;
        PasswordHashing Convert_pw;
        string datePattern = @"^(0[1-9]|1[0-2])/(0[1-9]|[12][0-9]|3[01])/(\d{4})$";
        string passwordPattern = @"^(?=.*[A-Z])(?=.*[\W_]).{8,}$";
        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        int minT, maxT;
        public LibrarianService()
        {
            ObjContext = new QLTV_BETAEntities();
            ObjLibrariansList = new List<LibrarianDTO>();
            Convert_pw = new PasswordHashing();
            minT = ObjContext.SETTINGs.FirstOrDefault().TuoiToiTieuThuThu ?? 18;
            maxT = ObjContext.SETTINGs.FirstOrDefault().TuoiToiDaThuThu ?? 45;
        }
        public List<LibrarianDTO> GetAll()
        {
            ObjLibrariansList = new List<LibrarianDTO>();
            try
            {
                var ObjQuery = from thuthu in ObjContext.THUTHUs select thuthu;
                foreach (var thuthu in ObjQuery)
                {
                    if (thuthu.IsDeleted == false)
                    {
                        ObjLibrariansList.Add(new LibrarianDTO
                        {
                            MaTT = thuthu.MaTT,
                            HoTen = thuthu.HoTen,
                            GioiTinh = thuthu.GioiTinh,
                            NgayVLam = (DateTime)thuthu.NgayVLam,
                            NgaySinh = (DateTime)thuthu.NgaySinh,
                            DiaChi = thuthu.DiaChi,
                            Email = thuthu.Email,
                            SoDT = thuthu.SoDT,
                            IsChecked = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ObjLibrariansList;
        }
        public bool Add(LibrarianDTO objNewLibrarian, UserDTO objNewUser)
        {
            bool IsAdded = false;
            try
            {
                Regex regex = new Regex(datePattern);

                if (string.IsNullOrWhiteSpace(objNewLibrarian.HoTen))
                {
                    MessageBox.Show("Họ tên không được để trống!");
                    return IsAdded;
                }
                if (string.IsNullOrWhiteSpace(objNewLibrarian.Email))
                {
                    MessageBox.Show("Email không được để trống!");
                    return IsAdded;
                } else
                {
                    if (!Regex.IsMatch(objNewLibrarian.Email, emailPattern))
                    {
                        MessageBox.Show("Email không hợp lệ!");
                        return IsAdded;
                    }
                }

                if (string.IsNullOrWhiteSpace(objNewLibrarian.SoDT))
                {
                    MessageBox.Show("Số điện thoại không được để trống!");
                    return IsAdded;
                }

                if (string.IsNullOrWhiteSpace(objNewUser.TaiKhoan))
                {
                    MessageBox.Show("Tên tài khoản không được để trống!");
                    return IsAdded;
                }

                if (string.IsNullOrWhiteSpace(objNewUser.MatKhau))
                {
                    MessageBox.Show("Mật khẩu không được để trống!");
                    return IsAdded;
                }

                if (!regex.IsMatch(objNewLibrarian.NgaySinh.ToString("dd/MM/yyyy")))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ!");
                    return IsAdded;
                } else {
                    int age = DateTime.Now.Year - objNewLibrarian.NgaySinh.Year;
                    if (age < minT || age > maxT)
                    {
                        MessageBox.Show("Độ tuổi không hợp lệ theo quy định!");
                        return IsAdded;
                    }
                }
                if (ObjLibrariansList.FirstOrDefault(l => l.Email == objNewLibrarian.Email) != null)
                {
                    MessageBox.Show("Email này đã tồn tại!");
                    return IsAdded;
                }
                else if (ObjContext.ACCOUNTs.FirstOrDefault(l => l.TaiKhoan == objNewUser.TaiKhoan) != null)
                {
                    MessageBox.Show("Tài khoản này đã tồn tại!");
                    return IsAdded;
                } else if (!Regex.IsMatch(objNewUser.MatKhau, passwordPattern))
                {
                    MessageBox.Show("Mật khẩu không hợp lệ!");
                    return IsAdded;
                }

                var ObjLibrarian = new THUTHU();
                var ObjUser = new ACCOUNT();
                var ObjParameter = ObjContext.PARAMETERS.First();

                ObjLibrarian.MaTT = "TT" + "00" + ObjParameter.IDMaTT.ToString();
                ObjLibrarian.HoTen = objNewLibrarian.HoTen;
                ObjLibrarian.Email = objNewLibrarian.Email;
                ObjLibrarian.DiaChi = objNewLibrarian.DiaChi;
                ObjLibrarian.GioiTinh = objNewLibrarian.GioiTinh;
                ObjLibrarian.IsDeleted = false;
                ObjLibrarian.NgaySinh = objNewLibrarian.NgaySinh;
                ObjLibrarian.NgayVLam = DateTime.Now;
                ObjLibrarian.SoDT = objNewLibrarian.SoDT;

                ObjUser.MaTT = ObjLibrarian.MaTT;
                ObjUser.TaiKhoan = objNewUser.TaiKhoan;
                ObjUser.VaiTro = 1;
                ObjUser.MatKhau = Convert_pw.HashPassword(objNewUser.MatKhau);
                ObjUser.IsDeleted = false;

                ObjContext.THUTHUs.Add(ObjLibrarian);
                ObjContext.ACCOUNTs.Add(ObjUser);

                ObjParameter.IDMaTT += 1;

                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return IsAdded;
        }

        public bool Update(LibrarianDTO obj_l, UserDTO obj_u, bool changePw)
        {
            bool IsUpdated = false;

            try
            {
                Regex regex = new Regex(datePattern);
                
                if (string.IsNullOrWhiteSpace(obj_l.HoTen))
                {
                    MessageBox.Show("Họ tên không được để trống!");
                    return IsUpdated;
                }
                if (string.IsNullOrWhiteSpace(obj_l.Email))
                {
                    MessageBox.Show("Email không được để trống!");
                    return IsUpdated;
                }

                if (string.IsNullOrWhiteSpace(obj_l.SoDT))
                {
                    MessageBox.Show("Số điện thoại không được để trống!");
                    return IsUpdated;
                }

                if (string.IsNullOrWhiteSpace(obj_u.TaiKhoan))
                {
                    MessageBox.Show("Tên tài khoản không được để trống!");
                    return IsUpdated;
                }

                if (string.IsNullOrWhiteSpace(obj_u.MatKhau))
                {
                    MessageBox.Show("Mật khẩu không được để trống!");
                    return IsUpdated;
                }
                if (!regex.IsMatch(obj_l.NgaySinh.ToString("dd/MM/yyyy")))
                {
                    MessageBox.Show("Ngày sinh không hợp lệ!");
                    return IsUpdated;
                }
                int age = DateTime.Now.Year - obj_l.NgaySinh.Year;
                if (changePw)
                {
                    if (Regex.IsMatch(obj_u.MatKhau, passwordPattern))
                    {
                        MessageBox.Show("Mật khẩu không hợp lệ!");
                        return IsUpdated;
                    }
                }
                if (age < minT || age > maxT)
                {
                    MessageBox.Show("Độ tuổi không hợp lệ theo quy định!");
                    return IsUpdated;
                }


                var ObjLibrarian = ObjContext.THUTHUs.FirstOrDefault(l => l.MaTT == obj_l.MaTT);
                var ObjUser = ObjContext.ACCOUNTs.FirstOrDefault(l => l.MaTT == obj_l.MaTT);

                ObjLibrarian.HoTen = obj_l.HoTen;
                ObjLibrarian.Email = obj_l.Email;
                ObjLibrarian.DiaChi = obj_l.DiaChi;
                ObjLibrarian.GioiTinh = obj_l.GioiTinh;
                ObjLibrarian.NgaySinh = obj_l.NgaySinh;
                ObjLibrarian.NgayVLam = DateTime.Now;
                ObjLibrarian.SoDT = obj_l.SoDT;
                if (changePw)
                {
                    ObjUser.MatKhau = Convert_pw.HashPassword(obj_u.MatKhau);
                }
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsUpdated = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return IsUpdated;
        }
        public ACCOUNT findUserTT(string MaTT)
        {
            ACCOUNT us = ObjContext.ACCOUNTs.FirstOrDefault(l => l.MaTT == MaTT);
            return us;
        }

        public bool Delete(List<string> listId)
        {
            bool IsDeleted = false;
            try
            {
                foreach (var item in listId)
                {
                    var obj = ObjContext.THUTHUs.Find(item);
                    obj.ACCOUNTs.First().IsDeleted = true;
                    obj.IsDeleted = true;
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

        public List<LibrarianDTO> Search(string text, string type)
        {
            List<LibrarianDTO> results = new List<LibrarianDTO>();
            text = text.ToLower();
            if (type == "Name")
            {
                results = ObjLibrariansList.Where(p => p.HoTen.ToLower().Contains(text)).ToList();
            }
            return results;
        }
    }
}