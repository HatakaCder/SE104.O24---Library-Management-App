using QuanLyThuVien.Model;
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
using System.Collections.ObjectModel;

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for Reader.xaml
    /// </summary>
    public partial class Reader : UserControl
    {
        public Reader()
        {
            InitializeComponent();
        }

        private void btAddReader_Click(object sender, RoutedEventArgs e)
        {
            AddReader addReader = new AddReader();
            addReader.Show();
        }

        private void dataTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ReaderUCVM reader = new ReaderUCVM();
            reader.selectedReaders = new ObservableCollection<READER>(dataTable.SelectedItems.Cast<READER>());
        }

        private void btUpdateReader_Click(object sender, RoutedEventArgs e)
        {
            UpdateReader updateReader = new UpdateReader();
            updateReader.Show();
        }
    }
}
