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

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int vaitro;
        public MainWindow(int vaitro)
        {
            InitializeComponent();
            this.vaitro = vaitro;
        }

        private void Btn_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_Checked_1(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Log Out", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                LoginWindoww loginWindoww = new LoginWindoww();
                loginWindoww.Show();
                this.Close();
            }
        }
    }
}
