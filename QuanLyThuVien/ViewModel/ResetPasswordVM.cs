using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Services;
using System.Windows.Forms;
using QuanLyThuVien.Command;

namespace QuanLyThuVien.ViewModel
{
    public class ResetPasswordVM : INotifyPropertyChanged
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
        private string email;
        public ResetPasswordVM(string email)
        {
            service = new ForgotPasswordService();
            changePasswordCommand = new RelayCommand(ChangePassword);
            this.email = email;
        }
        private string currentPassword;

        public string CurrentPassword
        {
            get { return currentPassword; }
            set { currentPassword = value; }
        }
        private string confirmPassword;

        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set { confirmPassword = value; }
        }
        private RelayCommand changePasswordCommand;

        public RelayCommand ChangePasswordCommand
        {
            get { return changePasswordCommand; }
            set { changePasswordCommand = value; }
        }


        public void ChangePassword()
        {
            try
            {
                if (ConfirmPassword != CurrentPassword) {
                    MessageBox.Show("Mật khẩu giữa 2 miền không khớp!");
                    return;
                }
                bool IsChanged = service.ChangePassword(email, CurrentPassword);
                if (IsChanged)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
