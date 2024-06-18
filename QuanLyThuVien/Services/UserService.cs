using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Class;

namespace QuanLyThuVien.Services
{
    public class UserService
    {
        QLTV_BETAEntities ObjContext;
        PasswordHashing Convert_pw;
        public UserService()
        {
            ObjContext = new QLTV_BETAEntities();
            Convert_pw = new PasswordHashing();
        }
        public bool Add(UserDTO objNewUser, DocGiaDTO objNewDG)
        {
            bool IsAdded = false;
            try
            {
                var ObjUser = new ACCOUNT(); // khởi tạo user mới
                var ObjDG = new DOCGIA();
                var ObjParameter = ObjContext.PARAMETERS.First(); // lấy giá trị đầu từ bảng parameters

                ObjDG.MaDG = "DG" + "00" + ObjParameter.IDDocGia.ToString();
                ObjDG.HoTen = objNewDG.HoTen;
                ObjDG.Email = objNewDG.Email;
                ObjDG.GioiTinh = objNewDG.GioiTinh;
                ObjDG.IsDeleted = false;
                ObjDG.NgayLapThe = DateTime.Now;
                ObjDG.NgaySinh = objNewDG.NgaySinh;
                if (objNewDG.SoDT != "")
                {
                    ObjDG.SoDT = objNewDG.SoDT;
                }
                if (objNewDG.DiaChi != "") { 
                    ObjDG.DiaChi = objNewDG.DiaChi;
                }

                ObjContext.DOCGIAs.Add(ObjDG);
                ObjParameter.IDDocGia += 1;
                ObjContext.SaveChanges();

                // gán giá trị của User mới vào biến ObjUser
                ObjUser.MaDG = ObjDG.MaDG;
                ObjUser.TaiKhoan = objNewUser.TaiKhoan;
                ObjUser.MatKhau = Convert_pw.HashPassword(objNewUser.MatKhau);
                ObjUser.IsDeleted = false;

                ObjContext.ACCOUNTs.Add(ObjUser); // thêm User mới vào bảng ACCOUNT

                // lưu thay đổi và kiểm tra dữ liệu đã được thêm chưa
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsAdded = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return IsAdded;
        }
        public bool SignIn(UserDTO objCurrentUser)
        {
            bool IsSignIn = false;
            try
            {
                var ObjUser = ObjContext.ACCOUNTs.Where(a => a.IsDeleted == false && a.TaiKhoan == objCurrentUser.TaiKhoan).FirstOrDefault();

                if (ObjUser != null)
                {
                    if (ObjUser.VaiTro == 1)
                    {
                        if (Convert_pw.VerifyPassword(objCurrentUser.MatKhau, ObjUser.MatKhau))
                        {
                            IsSignIn = true;
                        }
                    }
                    else if (ObjUser.VaiTro == 0)
                    {
                        if (objCurrentUser.MatKhau == ObjUser.MatKhau)
                        {
                            IsSignIn = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return IsSignIn;
        }
        public bool IsEmailExisted(DocGiaDTO objCurrentDG)
        {
            bool exists = ObjContext.DOCGIAs.Any(x => x.Email == objCurrentDG.Email);
            return exists;
        }
        public int getVaiTro(string taikhoan)
        {
            var ObjUser = ObjContext.ACCOUNTs.Where(a => a.IsDeleted == false && a.TaiKhoan == taikhoan).FirstOrDefault();
            return ObjUser.VaiTro;
        }
    }
}
