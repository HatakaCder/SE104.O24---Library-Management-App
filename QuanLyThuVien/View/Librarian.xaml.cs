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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyThuVien.ViewModel;

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for Librarian.xaml
    /// </summary>
    public partial class Librarian : UserControl
    {
        private LibrarianVM vm;
        public Librarian()
        {
            InitializeComponent();
            vm = new LibrarianVM();
            this.DataContext = vm;

            // Đặt AutoGeneratingColumn để bật AutoSize
            myDataGrid.AutoGeneratingColumn += (sender, e) =>
            {
                if (e.PropertyType == typeof(string))
                {
                    var column = new DataGridTextColumn();
                    column.Binding = new Binding(e.PropertyName);
                    column.Header = e.PropertyName;
                    column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
                    e.Column = column;
                }
            };

            // Đặt AutoGeneratingColumn để bật AutoSize
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void myDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
