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
        private string selectedCategoryName = null;
        private readonly Dictionary<string, string> addData = new Dictionary<string, string>();
        private string connectionString = @"Data Source=.;Initial Catalog=QLTV_BETA;Integrated Security=True";

        public Setting()
        {
            InitializeComponent();
            LoadData();
            LoadDataCategory();
        }

        private void LoadData()
        {
            // Load SETTING từ database để hiển thị quy định lên màn hình
            var setting = _context.SETTING.FirstOrDefault();
            if (setting != null)
            {
                TuoiToiTieuDocGia.Text = setting.TuoiToiTieuDocGia.ToString();
                TuoiToiDaDocGia.Text = setting.TuoiToiDaDocGia.ToString();
                TuoiToiTieuThuThu.Text = setting.TuoiToiTieuThuThu.ToString();
                TuoiToiDaThuThu.Text = setting.TuoiToiDaThuThu.ToString();
                ThoiHanThe.Text = setting.ThoiHanThe.ToString();
                SoNgayMuonToiDa.Text = setting.SoNgayMuonToiDa.ToString();
                SoTienNopTre.Text = setting.SoTienNopTre.ToString();
                SoSachMuonToiDa.Text = setting.SoSachMuonToiDa.ToString();
                SoLuongTheLoaiToiDa.Text = setting.SoLuongTheLoaiToiDa.ToString();
                ThoiGianNhapSach.Text = setting.ThoiGianNhapSach.ToString();
            }
            var data = _context.THELOAI.Where(x => !x.IsDeleted.Value).ToList();
            category.ItemsSource = data;
        }
        //Luu quy dinh vao Database
        private void Button_Click_Luu(object sender, RoutedEventArgs e)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(TuoiToiTieuDocGia.Text) ||
                string.IsNullOrWhiteSpace(TuoiToiDaDocGia.Text) ||
                string.IsNullOrWhiteSpace(TuoiToiTieuThuThu.Text) ||
                string.IsNullOrWhiteSpace(TuoiToiDaThuThu.Text) ||
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
                int tuoiToiTieuDocGia = int.Parse(TuoiToiTieuDocGia.Text);
                int tuoiToiDaDocGia = int.Parse(TuoiToiDaDocGia.Text);
                int tuoiToiTieuThuThu = int.Parse(TuoiToiTieuThuThu.Text);
                int tuoiToiDaThuThu = int.Parse(TuoiToiDaThuThu.Text);
                int thoiHanThe = int.Parse(ThoiHanThe.Text);
                int soNgayMuonToiDa = int.Parse(SoNgayMuonToiDa.Text);
                int soTienNopTre = int.Parse(SoTienNopTre.Text);
                int soSachMuonToiDa = int.Parse(SoSachMuonToiDa.Text);
                int soLuongTheLoaiToiDa = int.Parse(SoLuongTheLoaiToiDa.Text);
                int thoiGianNhapSach = int.Parse(ThoiGianNhapSach.Text);

                if (tuoiToiTieuDocGia <= 0)
                {
                    MessageBox.Show("Tuổi tối thiểu phải lớn hơn 0.");
                    return;
                }
                if (tuoiToiDaDocGia <= 0 || tuoiToiDaDocGia <= tuoiToiTieuDocGia)
                {
                    MessageBox.Show("Tuổi tối đa phải lớn hơn 0 và lớn hơn Tuổi tối thiểu.");
                    return;
                }
                if (tuoiToiTieuThuThu <= 0)
                {
                    MessageBox.Show("Tuổi tối thiểu phải lớn hơn 0.");
                    return;
                }
                if (tuoiToiDaThuThu <= 0 || tuoiToiDaThuThu <= tuoiToiTieuThuThu)
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

                setting.TuoiToiTieuDocGia = tuoiToiTieuDocGia;
                setting.TuoiToiDaDocGia = tuoiToiDaDocGia;
                setting.TuoiToiTieuThuThu = tuoiToiTieuThuThu;
                setting.TuoiToiDaThuThu = tuoiToiDaThuThu;
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

        //Hiển thị Thể loại lên màn hình
        private void LoadDataCategory()
        {
            var datas = _context.THELOAI.Where(x => x.IsDeleted == false).Select(x => new ComboBoxItem
            {
                Content = x.TenTheLoai,
                Tag = x.TenTheLoai,
            }).ToList();
            id_Category.ItemsSource = datas;
        }

        //Hiển thị thể loại trên combobox
        private void Data_Item(object sender, SelectionChangedEventArgs e)
        {
            if (category.SelectedItem != null)
            {
                var selectedCategory = (THELOAI)category.SelectedItem;
                foreach (var item in id_Category.Items)
                {
                    if (item is ComboBoxItem categoryItem && categoryItem.Content.ToString() == selectedCategory.TenTheLoai)
                    {
                        id_Category.SelectedItem = item;
                        selectedCategoryName = selectedCategory.TenTheLoai;
                        break;
                    }
                }

                tenTheLoai.Text = selectedCategory.TenTheLoai;
            }
        }

        //Khi chọn thể loại trong combobox, thể loại trong ô grid cũng được chọn theo
        private void xulycategorychange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)id_Category.SelectedItem;
            if (comboBoxItem != null)
            {
                selectedCategoryName = comboBoxItem.Tag.ToString();
                tenTheLoai.Text = comboBoxItem.Content.ToString();
            }
        }

        //Kiểm tra có nhập dữ liệu rỗng không
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

        //Thêm thể loại. Nếu thể loại đã tồn tại thì thông báo đã tồn tại. Nếu chưa thì thêm. Nếu nhập rỗng thì không cho thêm
        private void Button_Click_Them(object sender, RoutedEventArgs e)
        {
            addData["ten"] = tenTheLoai.Text;

            if (CheckValueNull(addData))
            {
                var checkName = _context.THELOAI.FirstOrDefault(x => x.TenTheLoai == tenTheLoai.Text);
                if (checkName != null)
                {
                    MessageBox.Show("Thể loại đã tồn tại");
                    return;
                }
                var newCategory = new THELOAI
                {
                    TenTheLoai = tenTheLoai.Text,
                    IsDeleted = false,
                };
                int currentCategoryCount = _context.THELOAI.Count(x => x.IsDeleted == false);
                int? maxCategoryCountNullable = _context.SETTING.Select(s => (int?)s.SoLuongTheLoaiToiDa).FirstOrDefault();
                int maxCategoryCount = maxCategoryCountNullable ?? 20; // Gán giá trị mặc định nếu null
                if (currentCategoryCount >= maxCategoryCount)
                {
                    MessageBox.Show("Số lượng thể loại đã đạt giới hạn tối đa.");
                    return;
                }

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

        //Sửa thể loại đã chọn
        private void Button_Click_Sua(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedCategoryName))
            {
                MessageBox.Show("ID bị null");
                return;
            }
            var categoryData = _context.THELOAI.FirstOrDefault(x => x.TenTheLoai == selectedCategoryName);
            if (categoryData == null)
            {
                MessageBox.Show("ID không tồn tại");
                return;
            }

            addData["Tên"] = tenTheLoai.Text;
            if (CheckValueNull(addData))
            {
                categoryData.TenTheLoai = tenTheLoai.Text;
                _context.THELOAI.AddOrUpdate(categoryData);
                if (_context.SaveChanges() > 0)
                {
                    MessageBox.Show("Cập nhật thể loại thành công");
                    LoadData();
                    LoadDataCategory();
                    return;
                }
                MessageBox.Show("Cập nhật thể loại thất bại");
            }
        }

        //Xóa thể loại
        private void Button_Click_Xoa(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(selectedCategoryName))
            {
                MessageBox.Show("ID bị null");
                return;
            }
            var categoryData = _context.THELOAI.SingleOrDefault(x => x.TenTheLoai == selectedCategoryName);
            if (categoryData == null)
            {
                MessageBox.Show("Thể loại không tồn tại");
                return;
            }

            categoryData.IsDeleted = true;
            _context.THELOAI.AddOrUpdate(categoryData);

            if (_context.SaveChanges() > 0)
            {
                MessageBox.Show("Xóa thể loại thành công");
                LoadData();
                LoadDataCategory();
                return;
            }
            MessageBox.Show("Xóa thể loại thất bại");
        }

        //Chức năng sửa mật khẩu
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var data = matkhaucu.Password.Trim();
            MessageBox.Show($"Data: " + data);
        }
    }
}