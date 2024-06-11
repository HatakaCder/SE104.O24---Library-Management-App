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

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for AddLibrarian.xaml
    /// </summary>
    public partial class AddLibrarian : Window
    {
        public AddLibrarian()
        {
            InitializeComponent();
        }

        public int MaTT { get; internal set; }

        internal void Showdialog()
        {
            throw new NotImplementedException();
        }
    }
}
