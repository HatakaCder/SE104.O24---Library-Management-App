using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyThuVien.Class
{
    public class OTP
    {
        public string GenerateOTP(int length)
        {
            var random = new Random();
            string otp = "";
            for (int i = 0; i < length; i++)
            {
                otp += random.Next(0, 10).ToString();
            }
            return otp;
        }
        public void SendEmail(string toEmail, string otp)
        {
            try
            {
                string s_otp = "Your OTP code is " + otp + ".";
                var client = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("cpvm12321@gmail.com", "aeqo skuj zvdu pmwu"),
                    EnableSsl = true
                };
                client.Send("mailtrap@demomailtrap.com", toEmail, "QLTV Demo: OTP Code", s_otp);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi gửi email
                Console.WriteLine(ex.Message);
            }
        }
    }
}
