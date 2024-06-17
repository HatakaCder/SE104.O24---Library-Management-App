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
using QuanLyThuVien.ViewModel;

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : UserControl
    {
        ForgotPasswordVM userControl;
        public ForgotPassword()
        {
            InitializeComponent();
            userControl = new ForgotPasswordVM();
            this.DataContext = userControl;

        }
        private void l_signin_MouseLeave(object sender, MouseEventArgs e)
        {
            l_signin.Foreground = (Brush)new BrushConverter().ConvertFromString("#808080");
        }

        private void l_signin_MouseEnter(object sender, MouseEventArgs e)
        {
            l_signin.Foreground = (Brush)new BrushConverter().ConvertFromString("#505050");
        }

        private void l_signin_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as LoginWindoww;
            mainWindow?.SwitchToSignIn();
        }
    }
}
