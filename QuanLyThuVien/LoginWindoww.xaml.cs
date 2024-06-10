using QuanLyThuVien.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for LoginWindoww.xaml
    /// </summary>
    public partial class LoginWindoww : Window
    {
        public LoginWindoww()
        {
            InitializeComponent();
            SwitchToSignIn();
        }
        public void SwitchToSignIn()
        {
            contentControl.Content = new SignIn();
        }

        public void SwitchToSignUp()
        {
            contentControl.Content = new SignUp();
        }
        public void SwitchToForgot()
        {
            contentControl.Content = new ForgotPassword();
        }
    }
}
