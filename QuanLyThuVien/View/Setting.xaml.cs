using QuanLyThuVien.Model;
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
        private readonly Random random = new Random();
        public Setting()
        {
            InitializeComponent();    
            loadData();
            loadDataCategory();

            id_Category.Visibility = Visibility.Collapsed;
        }       

        private void loadData()
        {
            //Lấy Setting từ Database
            var setting = _context.SETTING.FirstOrDefault();
            if (setting != null)
            {
                TuoiToiThieu.Text = setting.TuoiToiThieu.ToString();
                TuoiToiDa.Text = setting.TuoiToiDa.ToString();
                ThoiHanThe.Text = setting.ThoiHanThe.ToString();
                SoNgayMuonToiDa.Text = setting.SoNgayMuonToiDa.ToString();
                SoTienNopTre.Text = setting.SoTienNopTre.ToString();
                SoSachMuonToiDa.Text = setting.SoSachMuonToiDa.ToString();
                SoLuongTheLoaiToiDa.Text = setting.SoLuongTheLoaiToiDa.ToString();
                ThoiGianNhapSach.Text = setting.ThoiGianNhapSach.ToString();
            }

            //Hiển thị danh sách thể loại
            var data = _context.THELOAI.Where(x => !x.IsDeleted.Value).ToList();
            category.ItemsSource = data;
        }

        private void Button_Click_Luu(object sender, RoutedEventArgs e)
        {
            // Kiểm tra có ô nào trống không
            if (string.IsNullOrWhiteSpace(TuoiToiThieu.Text) ||
                string.IsNullOrWhiteSpace(TuoiToiDa.Text) ||
                string.IsNullOrWhiteSpace(ThoiHanThe.Text) ||
                string.IsNullOrWhiteSpace(SoNgayMuonToiDa.Text) ||
                string.IsNullOrWhiteSpace(SoTienNopTre.Text) ||
                string.IsNullOrWhiteSpace(SoSachMuonToiDa.Text) ||
                string.IsNullOrWhiteSpace(SoLuongTheLoaiToiDa.Text) ||
                string.IsNullOrWhiteSpace(ThoiGianNhapSach.Text))
            {
                MessageBox.Show("Không được bỏ trống bất kỳ ô nào.");
                return;
            }

            try
            {
                int tuoiToiThieu = int.Parse(TuoiToiThieu.Text);
                int tuoiToiDa = int.Parse(TuoiToiDa.Text);
                int thoiHanThe = int.Parse(ThoiHanThe.Text);
                int soNgayMuonToiDa = int.Parse(SoNgayMuonToiDa.Text);
                int soTienNopTre = int.Parse(SoTienNopTre.Text);
                int soSachMuonToiDa = int.Parse(SoSachMuonToiDa.Text);
                int soLuongTheLoaiToiDa = int.Parse(SoLuongTheLoaiToiDa.Text);
                int thoiGianNhapSach = int.Parse(ThoiGianNhapSach.Text);

                var setting = _context.SETTING.FirstOrDefault();

                if (setting == null)
                {
                    setting = new SETTING();
                    _context.SETTING.Add(setting);
                }

                setting.TuoiToiThieu = tuoiToiThieu;
                setting.TuoiToiDa = tuoiToiDa;
                setting.ThoiHanThe = thoiHanThe;
                setting.SoNgayMuonToiDa = soNgayMuonToiDa;
                setting.SoTienNopTre = soTienNopTre;
                setting.SoSachMuonToiDa = soSachMuonToiDa;
                setting.SoLuongTheLoaiToiDa = soLuongTheLoaiToiDa;
                setting.ThoiGianNhapSach = thoiGianNhapSach;

                _context.SETTING.AddOrUpdate(setting);
                _context.SaveChanges();

                MessageBox.Show("Đã lưu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = matkhaucu.Password.Trim();

            MessageBox.Show($"Data: " + data);
        }
    }
}
