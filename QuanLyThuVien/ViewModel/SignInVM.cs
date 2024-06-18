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
                    MainWindow mainWindow = new MainWindow(userService.getVaiTro(currentUser.TaiKhoan));
                    mainWindow.Show();
                    var mw = Application.Current.MainWindow as LoginWindoww;
                    mw?.Close();
                }
                else
                {
                    var res = MessageBox.Show("Tài khoản hoặc mật khẩu không đúng!, đăng nhập bằng tư cách độc giả?", "Login Failed", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (res == MessageBoxResult.Yes)
                    {
                        MainWindow mainWindow = new MainWindow(2);
                        mainWindow.Show();
                        var mw = Application.Current.MainWindow as LoginWindoww;
                        mw?.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
