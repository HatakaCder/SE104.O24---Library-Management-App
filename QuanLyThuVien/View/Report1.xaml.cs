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
    /// Interaction logic for Report1.xaml
    /// </summary>
    public partial class Report1 : UserControl
    {
        private Report1VM vm;
        public Report1(DateTime dt, string type)
        {
            vm = new Report1VM(dt, type);
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
