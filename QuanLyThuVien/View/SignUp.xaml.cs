using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : UserControl
    {
        SignUpVM ViewModel;
        public SignUp()
        {
            InitializeComponent();
            ViewModel = new SignUpVM();
            this.DataContext = ViewModel;
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
