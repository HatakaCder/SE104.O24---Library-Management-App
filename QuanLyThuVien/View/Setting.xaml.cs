﻿using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
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
            var data = _context.THELOAI.Where(x => !x.IsDeleted.Value).ToList();
            category.ItemsSource = data;
        }

        private void loadDataCategory()
        {
            var datas = _context.THELOAI.Select(x => new ComboBoxItem
            {
                Content = x.TenTheLoai,
                Tag = x.MaTheLoai,
                IsTabStop = x.IsDeleted.Value
            }).Where(x => !x.IsTabStop).ToList();
            id_Category.ItemsSource = datas;
        }

        private void Data_Item(object sender, SelectionChangedEventArgs e)
        {
            if(category.SelectedItem != null)
            {
                id_Category.Visibility = Visibility.Visible;
                THELOAI categorys = (THELOAI)category.SelectedItem;

                foreach(var item in id_Category.Items)
                {
                    if(item is ComboBoxItem categoryItem && categoryItem.Content.ToString() == categorys.TenTheLoai)
                    {
                        id_Category.SelectedItem = item;
                        category_id = categoryItem.Tag.ToString();
                        break;
                    }
                }

                tenTheLoai.Text = categorys.TenTheLoai;
            }
        }

        private void xulycategorychange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)id_Category.SelectedItem;
            if(comboBoxItem != null) {
                category_id = comboBoxItem.Tag.ToString();
            }
            
        }
        private string connectionString = @"metadata=res://*/Model.Model1.csdl|res://*/Model.Model1.ssdl|res://*/Model.Model1.msl;
                                            provider=System.Data.SqlClient;
                                            provider connection string='data source=.;
                                            initial catalog=QLTV_BETA;
                                            integrated security=True;
                                            encrypt=False;
                                            application name=EntityFramework;
                                            MultipleActiveResultSets=True'";

        public string GenerateTLId()
        {
            string prefix = "TL";
            int length = 3; // Độ dài phần số (001, 002,...)
            int currentMaxNumber = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MAX(CAST(SUBSTRING(MaTheLoai, 3, LEN(MaTheLoai) - 2) AS INT)) FROM THELOAI";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        currentMaxNumber = Convert.ToInt32(result);
                    }
                }
            }

            int newNumber = currentMaxNumber + 1;
            string newId = prefix + newNumber.ToString().PadLeft(length, '0');
            return newId;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            addData.Add("ten", tenTheLoai.Text);

            if (checkValueNull(addData))
            {
                var cehckName = _context.THELOAI.Where(x => x.TenTheLoai == tenTheLoai.Text).FirstOrDefault();
                if (cehckName != null)
                {
                    MessageBox.Show("Category đã tồn tại");
                    return;
                }

                var data = new THELOAI()
                {
                    MaTheLoai = GenerateTLId(),
                    TenTheLoai = tenTheLoai.Text,
                    IsDeleted = false,
                };

                _context.THELOAI.Add(data);
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

            var checkData = _context.THELOAI.Where(x => x.MaTheLoai == category_id).FirstOrDefault();
            if(checkData == null) {
                MessageBox.Show("id không tồn tại");
                return;
            }

            // Kiểm tra xem key "Tên" này đã có trong THELOAI chưa
            if (addData.ContainsKey("Tên"))
            {
                addData.Add("Tên", tenTheLoai.Text);
            }
            
            if (checkValueNull(addData))
            {
                checkData.TenTheLoai = tenTheLoai.Text;

                _context.THELOAI.AddOrUpdate(checkData);
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
                var checkCategory = _context.THELOAI.Where(x => x.MaTheLoai == category_id).SingleOrDefault();
                if (checkCategory == null)
                {
                    MessageBox.Show("null");
                    return;
                }

                checkCategory.IsDeleted = true;

                _context.THELOAI.AddOrUpdate(checkCategory);

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
