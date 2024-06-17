using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel;
using QuanLyThuVien.Model;
using QuanLyThuVien.Services;
using QuanLyThuVien.Command;

namespace QuanLyThuVien.ViewModel
{
    public class SignInVM : INotifyPropertyChanged
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
        public SignInVM()
        {
            userService = new UserService();
            CurrentUser = new UserDTO();
            signInCommand = new RelayCommand(SignIn);
        }
        private UserDTO currentUser;

        public UserDTO CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged("CurrentUser"); }
        }
        private RelayCommand signInCommand;

        public RelayCommand SignInCommand
        {
            get { return signInCommand; }
        }
        public void SignIn()
        {
            try
            {
                var IsSignIn = userService.SignIn(currentUser);
                if (IsSignIn)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    var mw = Application.Current.MainWindow as LoginWindoww;
                    mw?.Close();
                }
                else
                {
                    MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
