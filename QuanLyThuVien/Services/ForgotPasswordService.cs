
using QuanLyThuVien.Class;
using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace QuanLyThuVien.Services
{
    public class ForgotPasswordService
    {
        QLTV_BETAEntities ObjContext;
        PasswordHashing ph;
        public ForgotPasswordService()
        {
            ObjContext = new QLTV_BETAEntities();
            ph = new PasswordHashing();
        }
        public string SendOtp(string email)
        {
            string otp;
            try
            {
                OTP Otp = new OTP();
                otp = Otp.GenerateOTP(6);
                Otp.SendEmail(email, otp);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return otp;
        }
        public bool ChangePassword(string email, string password)
        {
            bool IsChanged = false;
            try
            {
                var obj_dg = ObjContext.DOCGIA.FirstOrDefault(x => x.Email == email);
                var obj_acc = ObjContext.ACCOUNT.FirstOrDefault(x => x.MaDG == obj_dg.MaDG);
                obj_acc.MatKhau = ph.HashPassword(password);
                var NoOfRowsAffected = ObjContext.SaveChanges();
                IsChanged = NoOfRowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsChanged;
        }
    }
}
