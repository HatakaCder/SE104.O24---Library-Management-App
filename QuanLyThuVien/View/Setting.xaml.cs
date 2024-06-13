using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : UserControl
    {
        private readonly QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private string category_id = null;
        private Dictionary<string, string> addData = new Dictionary<string, string>();
        private string key = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuioasdfghjklzxcvbnm1234567890";
        private readonly Random random = new Random();
        public Setting()
        {
            InitializeComponent();
            loadData();
            loadDataCategory();
            soquyen.Text = "2";
            thecogiatri.Text = "2";
            songay.Text = "2";
            sotien.Text = "2";
            songaychomuon.Text = "2";
            dotuoi.Text = "2";
            dotuoitoithieu.Text = "13";
            soluong.Text = "2";
            nhapsach.Text = "2";


            Application.Current.Properties["Data"] = "2";
            Application.Current.Properties["ngaymuon"] = songay.Text;
            Application.Current.Properties["hethanthe"] = "2";
            Application.Current.Properties["soquyen"] = "2";
            Application.Current.Properties["songaychomuon"] = "2";
            Application.Current.Properties["soluong"] = "2";

            id_Category.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = matkhaucu.Password.Trim();

            MessageBox.Show($"Data: " + data);
        }

        private void loadData()
        {
            var data = _context.Category.Where(x => !x.IsDeleted.Value).ToList();
            category.ItemsSource = data;
        }

        private void loadDataCategory()
        {
            var datas = _context.Category.Select(x => new ComboBoxItem
            {
                Content = x.Ten,
                Tag = x.Id,
                IsTabStop = x.IsDeleted.Value
            }).Where(x => !x.IsTabStop).ToList();
            id_Category.ItemsSource = datas;
        }

        private void Data_Item(object sender, SelectionChangedEventArgs e)
        {
            if(category.SelectedItem != null)
            {
                id_Category.Visibility = Visibility.Visible;
                Category categorys = (Category)category.SelectedItem;

                foreach(var item in id_Category.Items)
                {
                    if(item is ComboBoxItem categoryItem && categoryItem.Content.ToString() == categorys.Ten)
                    {
                        id_Category.SelectedItem = item;
                        category_id = categoryItem.Tag.ToString();
                        break;
                    }
                }

                ten.Text = categorys.Ten;
            }
        }

        private void xulycategorychange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)id_Category.SelectedItem;
            if(comboBoxItem != null) {
                category_id = comboBoxItem.Tag.ToString();
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            addData.Add("ten", ten.Text);

            if (checkValueNull(addData))
            {
                var cehckName = _context.Category.Where(x => x.Ten == ten.Text).FirstOrDefault();
                if (cehckName != null)
                {
                    MessageBox.Show("Category đã tồn tại");
                    return;
                }

                var data = new Category()
                {
                    Id = rondomId(5, key),
                    Ten = ten.Text,
                    NgayLapThe = DateTime.Now,
                    IsDeleted = false,
                };

                _context.Category.Add(data);
                if(_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Add category Success");
                    loadData();
                    loadDataCategory();
                    return;
                }

                MessageBox.Show("Add category fald");
            }
        }

        private bool checkValueNull(Dictionary<string, string> check)
        {
            foreach(var item in check)
            {
                if (item.Value == null || item.Value.Equals(string.Empty))
                {
                    MessageBox.Show($"Dữ liệu ở {item.Key} đang bị null");
                    return false;
                }
            }

            return true;
        }

        private string rondomId(int n, string key)
        {
            StringBuilder stb = new StringBuilder();

            for(int i = 0; i < n; i++)
            {
                stb.Append(key[random.Next(key.Length)]);
            }

            return stb.ToString();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
            if(!checkValidate())
            {
                MessageBox.Show("id bị null");
                return;
            }

            var checkData = _context.Category.Where(x => x.Id == category_id).FirstOrDefault();
            if(checkData == null) {
                MessageBox.Show("id không tồn tại");
                return;
            }

            // Kiểm tra xem key "Tên" này đã có tronng Dictionnary chưa
            if (addData.ContainsKey("Tên"))
            {
                addData.Add("Tên", ten.Text);
            }
            
            if (checkValueNull(addData))
            {
                checkData.Ten = ten.Text;

                _context.Category.AddOrUpdate(checkData);
                if(_context.SaveChanges() > 0)
                {
                    id_Category.Visibility = Visibility.Visible;
                    MessageBox.Show("Update thành công");
                    loadData();
                    loadDataCategory();
                    return;
                }

                MessageBox.Show("Update Faid");
                return;
            }
        }

        private bool checkValidate()
        {
            if (category_id == null || category_id.Equals(string.Empty) || category_id == "")
            {
                MessageBox.Show("id bị null");
                return false;
            }

            return true;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (checkValidate())
            {
                var checkCategory = _context.Category.Where(x => x.Id == category_id).SingleOrDefault();
                if (checkCategory == null)
                {
                    MessageBox.Show("null");
                    return;
                }

                checkCategory.IsDeleted = true;

                _context.Category.AddOrUpdate(checkCategory);

                if(_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Delete thành công");
                    //category_id = id_Category.Items[0].ToString();
                    loadData();
                    loadDataCategory();
                    return;
                }
                MessageBox.Show("Delete Faild");

            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Đây là lấy dữ liệu luôn khi vừa nhập 1 ký tự nào đấy
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Lấy ra nội dung vừa được nhập vào TextBox
            TextBox textBox = sender as TextBox;
            string text = textBox.Text;
            if (int.TryParse(text, out int so))
            {
                AddOrEditMuonTra data = new AddOrEditMuonTra(text);
                Application.Current.Properties["Data"] = text; // Lưu dữ liệu "text" này vào trong "Application.Current.Properties", "["Data"]" này là đặt tên đại diện cho dữ liệu đấy
                return;
            }

            MessageBox.Show("Phải là số");
            return;
            

            

            // Hiển thị thông báo để kiểm tra xem dữ liệu có được lấy ra đúng không
            //MessageBox.Show("Dữ liệu vừa nhập: " + text);
        }

        private void gioihanngaymuon(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string data = textBox.Text;
            if(int.TryParse(data, out int so))
            {
                Application.Current.Properties["ngaymuon"] = so.ToString();
                return;
            }
            MessageBox.Show("Phải là số");
        }

        private void thecogiatri_docgia(object sender, TextChangedEventArgs e)
        {
            TextBox data = sender as TextBox;
            string dataItem = data.Text;

            if(int.TryParse(dataItem, out int so))
            {
                Application.Current.Properties["hethanthe"] = so.ToString();
                return;
            }

            MessageBox.Show("Phải là số");
            return;
        }

        private void soquyenmuon(object sender, TextChangedEventArgs e)
        {
            TextBox data = sender as TextBox;
            string dataItem = data.Text;

            if(int.TryParse(dataItem, out int so))
            {
                Application.Current.Properties["soquyen"] = so.ToString();
                return;
            }

            MessageBox.Show("Phải là số");
            return;
        }


    }
}
