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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for SignIn.xaml
    /// </summary>
    public partial class SignIn : UserControl
    {
        public SignIn()
        {
            InitializeComponent();
        }
        private void l_register_MouseEnter(object sender, MouseEventArgs e)
        {
            l_register.Foreground = (Brush)new BrushConverter().ConvertFromString("#505050");
        }

        private void l_register_MouseLeave(object sender, MouseEventArgs e)
        {
            l_register.Foreground = (Brush)new BrushConverter().ConvertFromString("#808080");
        }
        private void l_forgot_MouseEnter(object sender, MouseEventArgs e)
        {
            l_forgot.Foreground = (Brush)new BrushConverter().ConvertFromString("#505050");
        }

        private void l_forgot_MouseLeave(object sender, MouseEventArgs e)
        {
            l_forgot.Foreground = (Brush)new BrushConverter().ConvertFromString("#808080");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            PasswordUnmask.Visibility = Visibility.Visible;
            PasswordHidden.Visibility = Visibility.Hidden;
            PasswordUnmask.Text = PasswordHidden.Password;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            PasswordUnmask.Visibility = Visibility.Hidden;
            PasswordHidden.Visibility = Visibility.Visible;
            PasswordHidden.Password = PasswordUnmask.Text;
        }

        private void l_forgot_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as LoginWindoww;
            mainWindow?.SwitchToForgot();
        }

        private void l_register_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as LoginWindoww;
            mainWindow?.SwitchToSignUp();
        }
    }
}
