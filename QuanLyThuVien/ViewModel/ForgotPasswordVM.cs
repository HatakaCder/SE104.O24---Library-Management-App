using QuanLyThuVien.Command;
using QuanLyThuVien.Model;
using QuanLyThuVien.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyThuVien.ViewModel
{
    public class ForgotPasswordVM : INotifyPropertyChanged
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

        private ForgotPasswordService service;
        private string otp;

        public ForgotPasswordVM()
        {
            service = new ForgotPasswordService();
            sendOtpCommand = new RelayCommand(GetOtp);
            confirmOtpCommand = new RelayCommand(ConfirmOtp);
        }

        private string currentOtp;

        public string CurrentOtp
        {
            get { return currentOtp; }
            set { currentOtp = value; OnPropertyChanged("CurrentOtp"); }
        }
        private string currentEmail;

        public string CurrentEmail
        {
            get { return currentEmail; }
            set { currentEmail = value; OnPropertyChanged("CurrentEmail"); }
        }

        private RelayCommand sendOtpCommand;

        public RelayCommand SendOtpCommand
        {
            get { return sendOtpCommand; }
            set { sendOtpCommand = value; }
        }
        private RelayCommand confirmOtpCommand;

        public RelayCommand ConfirmOtpCommand
        {
            get { return confirmOtpCommand; }
            set { confirmOtpCommand = value; }
        }
        public void GetOtp()
        {
            if (string.IsNullOrEmpty(CurrentEmail))
            {
                MessageBox.Show("Trường Email không được để trống!");
                return;
            }
            else
            {
                otp = service.SendOtp(CurrentEmail);
            }
        }
        public void ConfirmOtp()
        {
            if (CurrentOtp != otp || string.IsNullOrEmpty(otp))
            {
                MessageBox.Show("Sai mã OTP");
                return;
            }
            else
            {
                var mainWindow = Application.Current.MainWindow as LoginWindoww;
                mainWindow?.SwitchToReset(CurrentEmail);
            }
        }
    }
}
