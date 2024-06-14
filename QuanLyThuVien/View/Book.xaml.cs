using QuanLyThuVien.Model;
using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Book.xaml
    /// </summary>
    public partial class Book : UserControl
    {
        public Book()
        {
            InitializeComponent();
        }

        private void dgView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BookVM bookVm = (BookVM)DataContext;
            bookVm.selectedBooks = new ObservableCollection<BOOK>(dgView.SelectedItems.Cast<BOOK>());
        }

        private void btThemSach_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.Show();
        }

        private void btSuaSach_Click(object sender, RoutedEventArgs e)
        {
            UpdateBook updateBook = new UpdateBook();
            updateBook.Show();
        }
    }
}
