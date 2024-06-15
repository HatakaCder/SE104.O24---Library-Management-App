using QuanLyThuVien.Class;
using QuanLyThuVien.Model;
using QuanLyThuVien.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Command;
using System.Windows;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Contexts;

namespace QuanLyThuVien.ViewModel
{
    
    public class SignUpVM : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        UserService userService;
        private string otp = "";
        private string regexPattern = @"^(0[1-9])+([0-9]{8})\b$";
        private string selectedOption = "";

        public SignUpVM()
        {
            userService = new UserService();
            CurrentUser = new UserDTO();
            CurrentDocGia = new DocGiaDTO();
            signUpCommand = new RelayCommand(SignUp);
            sendOTPCommand = new RelayCommand(SendOTP);
            CurrentDocGia.NgaySinh = DateTime.Now;
            NamChecked = true;
        }
        private UserDTO currentUser;

        public UserDTO CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged("CurrentUser"); }
        }
        private DocGiaDTO currentDocGia;

        public DocGiaDTO CurrentDocGia
        {
            get { return currentDocGia; }
            set { currentDocGia = value; OnPropertyChanged("CurrentDocGia"); }
        }

        private string currentOtp;

        public string CurrentOtp
        {
            get { return currentOtp; }
            set { currentOtp = value; OnPropertyChanged("CurrentOtp");}
        }

        private string passwordConfirm;

        public string PasswordConfirm
        {
            get { return passwordConfirm; }
            set { passwordConfirm = value; OnPropertyChanged("PasswordConfirm"); }
        }
        private bool namChecked;

        public bool NamChecked
        {
            get { return namChecked; }
            set { namChecked = value; 
                if (value)
                {
                    selectedOption = "Nam";
                }
                OnPropertyChanged("NamChecked"); }
        }
        private bool nuChecked;

        public bool NuChecked
        {
            get { return nuChecked; }
            set { nuChecked = value;
                if (value)
                {
                    selectedOption = "Nu";
                }
                OnPropertyChanged("NuChecked"); }
        }

        private RelayCommand signUpCommand;

        public RelayCommand SignUpCommand
        {
            get { return signUpCommand; }
        }
        private RelayCommand sendOTPCommand;

        public RelayCommand SendOTPCommand
        {
            get { return sendOTPCommand; }
        }
        public void SignUp()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CurrentDocGia.HoTen))
                {
                    MessageBox.Show("Họ và tên không được để trống!");
                    return;
                }
                else if (!string.IsNullOrWhiteSpace(CurrentDocGia.SoDT))
                {
                    if (!Regex.IsMatch(CurrentDocGia.SoDT, regexPattern))
                    {
                        MessageBox.Show("Số điện thoại không đúng định dạng!");
                        return;
                    }
                }
                if (string.IsNullOrWhiteSpace(CurrentDocGia.Email))
                {
                    MessageBox.Show("Email không được để trống!");
                    return;
                } else
                {
                    bool exists = userService.IsEmailExisted(CurrentDocGia);
                    if (exists)
                    {
                        MessageBox.Show("Email đã được sử dụng bởi tài khoản khác!");
                        return;
                    }
                }
                if (string.IsNullOrWhiteSpace(CurrentUser.TaiKhoan))
                {
                    MessageBox.Show("Tên đăng nhập không được để trống!");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(CurrentUser.MatKhau))
                {
                    MessageBox.Show("Mật khẩu không được để trống!");
                    return;
                }
                else if (CurrentUser.MatKhau != PasswordConfirm)
                {
                    MessageBox.Show("Mật khẩu ở vùng xác nhận không khớp!");
                    return;

                } 
                else if (CurrentOtp != otp || string.IsNullOrWhiteSpace(otp))
                {
                    MessageBox.Show("Mã OTP không đúng!");
                    return;
                }
                CurrentDocGia.GioiTinh = selectedOption;
                var IsAdded = userService.Add(CurrentUser, CurrentDocGia);
                if (IsAdded)
                {
                    MessageBox.Show("Đăng ký tài khoản thành công!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void SendOTP()
        {
            if (string.IsNullOrWhiteSpace(CurrentDocGia.Email))
            {
                MessageBox.Show("Email không được để trống!");
                return;
            }
            OTP Otp = new OTP();
            otp = Otp.GenerateOTP(6);
            Otp.SendEmail(CurrentDocGia.Email, otp);
        }
    }
}
