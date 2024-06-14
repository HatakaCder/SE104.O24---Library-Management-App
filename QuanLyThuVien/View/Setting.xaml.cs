using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyThuVien.View
{
    public partial class Setting : UserControl
    {
        private readonly QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private string category_id = null;
        private readonly Dictionary<string, string> addData = new Dictionary<string, string>();
        private readonly Random random = new Random();
        private string connectionString = @"Data Source=.;Initial Catalog=QLTV_BETA;Integrated Security=True";

        public Setting()
        {
            InitializeComponent();
            LoadData();
            LoadDataCategory();
        }

        private void LoadData()
        {
            // Load settings from database
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

            // Display categories
            var data = _context.THELOAI.Where(x => !x.IsDeleted.Value).ToList();
            category.ItemsSource = data;
        }

        //Luu quy dinh vao Database
        private void Button_Click_Luu(object sender, RoutedEventArgs e)
        {
            // Validate input
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
                // Save settings
                int tuoiToiThieu = int.Parse(TuoiToiThieu.Text);
                int tuoiToiDa = int.Parse(TuoiToiDa.Text);
                int thoiHanThe = int.Parse(ThoiHanThe.Text);
                int soNgayMuonToiDa = int.Parse(SoNgayMuonToiDa.Text);
                int soTienNopTre = int.Parse(SoTienNopTre.Text);
                int soSachMuonToiDa = int.Parse(SoSachMuonToiDa.Text);
                int soLuongTheLoaiToiDa = int.Parse(SoLuongTheLoaiToiDa.Text);
                int thoiGianNhapSach = int.Parse(ThoiGianNhapSach.Text);

                if (tuoiToiThieu <= 0)
                {
                    MessageBox.Show("Tuổi tối thiểu phải lớn hơn 0.");
                    return;
                }

                if (tuoiToiDa <= 0 || tuoiToiDa <= tuoiToiThieu)
                {
                    MessageBox.Show("Tuổi tối đa phải lớn hơn 0 và lớn hơn Tuổi tối thiểu.");
                    return;
                }

                if (thoiHanThe <= 0)
                {
                    MessageBox.Show("Thời hạn thẻ phải lớn hơn 0.");
                    return;
                }

                if (soNgayMuonToiDa <= 0)
                {
                    MessageBox.Show("Số ngày mượn tối đa phải lớn hơn 0.");
                    return;
                }

                if (soTienNopTre < 0)
                {
                    MessageBox.Show("Số tiền nộp trễ không được âm.");
                    return;
                }

                if (soSachMuonToiDa <= 1)
                {
                    MessageBox.Show("Số sách mượn tối đa phải lớn hơn 1.");
                    return;
                }

                if (soLuongTheLoaiToiDa <= 0)
                {
                    MessageBox.Show("Số lượng thể loại tối đa phải lớn hơn 0.");
                    return;
                }

                if (thoiGianNhapSach <= 0)
                {
                    MessageBox.Show("Thời gian nhập sách phải lớn hơn 0.");
                    return;
                }

                var setting = _context.SETTING.FirstOrDefault() ?? new SETTING();

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

        private void LoadDataCategory()
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
            if (category.SelectedItem != null)
            {
                var selectedCategory = (THELOAI)category.SelectedItem;
                THELOAI categorys = (THELOAI)category.SelectedItem;
                foreach (var item in id_Category.Items)
                {
                    if (item is ComboBoxItem categoryItem && categoryItem.Content.ToString() == categorys.TenTheLoai)
                    {
                        id_Category.SelectedItem = item;
                        category_id = categoryItem.Tag.ToString();
                        break;
                    }
                }

                tenTheLoai.Text = selectedCategory.TenTheLoai;
            }
        }
        private void xulycategorychange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)id_Category.SelectedItem;
            if (comboBoxItem != null)
            {
                category_id = comboBoxItem.Tag.ToString();
                tenTheLoai.Text = comboBoxItem.Content.ToString(); // Hiển thị tên thể loại lên TextBox tenTheLoai
            }
        }

        private string GenerateTLId()
        {
            const string prefix = "TL";
            const int length = 3;
            int currentMaxNumber = 0;

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var query = "SELECT MAX(CAST(SUBSTRING(MaTheLoai, 3, LEN(MaTheLoai) - 2) AS INT)) FROM THELOAI";

                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        currentMaxNumber = Convert.ToInt32(result);
                    }
                }
            }

            int newNumber = currentMaxNumber + 1;
            return $"{prefix}{newNumber.ToString().PadLeft(length, '0')}";
        }
        private bool CheckValueNull(Dictionary<string, string> check)
        {
            foreach (var item in check)
            {
                if (string.IsNullOrWhiteSpace(item.Value))
                {
                    MessageBox.Show($"Dữ liệu ở {item.Key} đang bị null");
                    return false;
                }
            }

            return true;
        }
        private void Button_Click_Them(object sender, RoutedEventArgs e)
        {
            addData["ten"] = tenTheLoai.Text;

            if (CheckValueNull(addData))
            {
                // Kiểm tra xem thể loại đã tồn tại hay chưa
                var checkName = _context.THELOAI.FirstOrDefault(x => x.TenTheLoai == tenTheLoai.Text);
                if (checkName != null)
                {
                    MessageBox.Show("Thể loại đã tồn tại");
                    return;
                }

                // Tạo thể loại mới
                var newCategory = new THELOAI
                {
                    MaTheLoai = GenerateTLId(),
                    TenTheLoai = tenTheLoai.Text,
                    IsDeleted = false,
                };

                // Thêm thể loại mới vào cơ sở dữ liệu
                _context.THELOAI.Add(newCategory);
                if (_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Add category Success");
                    LoadData();
                    LoadDataCategory();
                    return;
                }

                MessageBox.Show("Add category failed");
            }
        }

        private void Button_Click_Sua(object sender, RoutedEventArgs e)
        {
            if (!CheckValidate())
            {
                MessageBox.Show("id bị null");
                return;
            }

            var categoryData = _context.THELOAI.FirstOrDefault(x => x.MaTheLoai == category_id);
            if (categoryData == null)
            {
                MessageBox.Show("id không tồn tại");
                return;
            }

            addData["Tên"] = tenTheLoai.Text;

            if (CheckValueNull(addData))
            {
                categoryData.TenTheLoai = tenTheLoai.Text;
                _context.THELOAI.AddOrUpdate(categoryData);
                if (_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Update thành công");
                    LoadData();
                    LoadDataCategory();
                    return;
                }

                MessageBox.Show("Update failed");
            }
        }

        private bool CheckValidate()
        {
            if (string.IsNullOrWhiteSpace(category_id))
            {
                MessageBox.Show("id bị null");
                return false;
            }

            return true;
        }

        private void Button_Click_Xoa(object sender, RoutedEventArgs e)
        {
            if (CheckValidate())
            {
                var categoryData = _context.THELOAI.SingleOrDefault(x => x.MaTheLoai == category_id);
                if (categoryData == null)
                {
                    MessageBox.Show("null");
                    return;
                }

                categoryData.IsDeleted = true;
                _context.THELOAI.AddOrUpdate(categoryData);

                if (_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Delete thành công");
                    LoadData();
                    LoadDataCategory();
                    return;
                }
                MessageBox.Show("Delete failed");
            }
        }

        private void gioihanngaymuon(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int days))
            {
                Application.Current.Properties["ngaymuon"] = days.ToString();
            }
            else
            {
                MessageBox.Show("Phải là số");
            }
        }

        private void thecogiatri_docgia(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int value))
            {
                Application.Current.Properties["hethanthe"] = value.ToString();
            }
            else
            {
                MessageBox.Show("Phải là số");
            }
        }

        private void soquyenmuon(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox && int.TryParse(textBox.Text, out int quantity))
            {
                Application.Current.Properties["soquyen"] = quantity.ToString();
            }
            else
            {
                MessageBox.Show("Phải là số");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = matkhaucu.Password.Trim();
            MessageBox.Show($"Data: " + data);
        }
    }
}
