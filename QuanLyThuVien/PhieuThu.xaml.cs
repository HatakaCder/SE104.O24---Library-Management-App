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
using System.Windows.Shapes;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for PhieuThu.xaml
    /// </summary>
    public partial class PhieuThu : Window
    {
        public PhieuThu()
        {
            InitializeComponent();
        }

        private readonly QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private string maphieutra_Id = null;

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            loadPhieuTra();
        }
        private void loadPhieuTra()
        {
            List<ComboBoxItem> list = new List<ComboBoxItem>();

            var data = _context.PHIEUTRAs.Where(x => !x.IsDeleted.Value).ToList();
            if (data.Count > 0 || data.Any())
            {
                foreach (var item in data)
                {
                    var comboboxItem = new ComboBoxItem
                    {
                        Content = item.MaPhTra,
                        Tag = item.MaPhTra
                    };

                    list.Add(comboboxItem);
                }
            }


            maphieutra.ItemsSource = list;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            int songayQuaHan = 0;
            int sotienPhat = 0;
            if(maphieutra_Id == null || maphieutra_Id == "")
            {
                MessageBox.Show("Vui lòng chọn mã phiếu mượn");
                return;
            }

            if(!int.TryParse(quahan.Text, out songayQuaHan) || !int.TryParse(sotien.Text, out sotienPhat))
            {
                MessageBox.Show("Ngày quá hạn và số tiền phải là số");
                return;
            }

            var checkPhieuTra = _context.PHIEUTRAs.Where(x => x.MaPhTra == maphieutra_Id && !x.IsDeleted.Value).FirstOrDefault();
            if(checkPhieuTra == null)
            {
                MessageBox.Show("Mã phiếu trả không hợp lệ");
                return;
            }

            var phieuThu = new PHIEUTHU
            {
                MaPhTra = checkPhieuTra.MaPhTra,
                SoNgayQHan = (short)songayQuaHan,
                SoTienThu = (short)sotienPhat,
            };

            _context.PHIEUTHUs.Add(phieuThu);
            if(_context.SaveChanges() > 0)
            {
                MessageBox.Show("Add thành công");
                return;
            }


            MessageBox.Show("Add Faild");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void xulyphieutrachange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)maphieutra.SelectedItem;
            maphieutra_Id = comboBoxItem.Tag.ToString();
        }

        private void xulyphieumuonchange(object sender, SelectionChangedEventArgs e)
        {

        }

        private void xulychange1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox data = sender as TextBox;
            string dataItem = data.Text;

            if(!int.TryParse(dataItem, out int so))
            {
                MessageBox.Show("Phải là số");
                return;
            }
        }

        private void TextBox_TextChanged_Sotien(object sender, TextChangedEventArgs e)
        {
            TextBox data = sender as TextBox;
            string dataItem = data.Text;

            if (!int.TryParse(dataItem, out int so))
            {
                MessageBox.Show("Phải là số");
                return;
            }
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }
    }
}
