using QuanLyThuVien.Model;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
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
using System.IO;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Data.Entity.Core.EntityClient;

namespace QuanLyThuVien
{

    public partial class FormPhieuMuon : Window
    {
        private QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private List<DOCGIA> _allDocGia;
        private List<string> _validMaDGs;
        public FormPhieuMuon()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
            docgia.SelectionChanged += docgia_SelectionChanged;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataDocGia();
            LoadDataSach();
        }

        //Di chuyển Form khi kéo border ở trên
        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        //Nút tắt Form
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

            this.Close();
        }

        //Hiển thị data độc giả để chọn trong combobox
        private void LoadDataDocGia()
        {
            try
            {
                _allDocGia = _context.DOCGIAs.ToList();
                _validMaDGs = _allDocGia.Select(d => d.MaDG).ToList();
                docgia.ItemsSource = _allDocGia;
                docgia.DisplayMemberPath = "MaDG";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lấy data độc giả: " + ex.Message);
            }
        }

        //Hiển thị data sách để chọn trong combobox
        private void LoadDataSach()
        {
            try
            {
                var listSach = _context.SACHes.Where(s => s.IsDeleted == false).ToList();
                sach.ItemsSource = listSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message);
            }
        }

        //TÌm kiếm trong lúc nhập
        private void docgia_TextChanged(object sender, TextChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tb = comboBox?.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;

            if (comboBox != null && tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                var filterText = tb.Text;

                var filteredList = _allDocGia
                    .Where(d => d.MaDG.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                comboBox.ItemsSource = filteredList;
                comboBox.IsDropDownOpen = true;
                tb.SelectionStart = filterText.Length;
                tb.SelectionLength = 0;
            }
        }

        //Xử lí khi chọn đúng MaDG
        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                DOCGIA selectedDocGia = (DOCGIA)docgia.SelectedItem;
                if (selectedDocGia != null)
                {
                    txtHoTen.Text = selectedDocGia.HoTen;
                    if (selectedDocGia.NgaySinh != null)
                    {
                        txtNgaySinh.Text = selectedDocGia.NgaySinh.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtNgaySinh.Text = string.Empty;
                    }
                }
            }
        }

        //Tìm kiếm lúc nhập sách
        private void sach_TextChanged(object sender, TextChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tb = comboBox?.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;

            if (comboBox != null && tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                var filterText = tb.Text;

                var filteredList = _context.SACHes
                    .Where(s => s.TenSach.Contains(filterText) && s.IsDeleted == false)
                    .ToList();
                comboBox.ItemsSource = filteredList;
                comboBox.IsDropDownOpen = true;
                tb.SelectionStart = filterText.Length;
                tb.SelectionLength = 0;
            }
        }


        //Hàm tạo ID
        //Lấy id cao nhất hiện tại trong database để tạo id mới
        private int GetCurrentIdNumberFromDatabase(string prefix)
        {
            try
            {
                string entityConnectionStringName = "QLTV_BETAEntities"; // Tên của chuỗi kết nối trong file .config
                var entityBuilder = new EntityConnectionStringBuilder(ConfigurationManager.ConnectionStrings[entityConnectionStringName].ConnectionString);
                string sqlConnectionString = entityBuilder.ProviderConnectionString;

                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    connection.Open();
                    string sql = "SELECT ISNULL(MAX(CAST(SUBSTRING(MaPhMuon, 3, LEN(MaPhMuon) - 2) AS INT)), 0) " +
                                 "FROM PHIEUMUON WHERE MaPhMuon LIKE @prefix + '%'";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@prefix", prefix);
                        var result = command.ExecuteScalar();
                        return (result == DBNull.Value) ? 0 : Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lấy dữ liệu từ cơ sở dữ liệu: " + ex.Message);
                return 0;
            }
        }

        // Hàm Random
        public string generateId(string prefix, int length) //prefix là PM cho Phiếu mượn, PT cho phiếu trả
        {
            int currentNumber = GetCurrentIdNumberFromDatabase(prefix);
            string newId;
            do
            {
                currentNumber++;
                string numberPart = currentNumber.ToString().PadLeft(length, '0');
                newId = prefix + numberPart;
            }
            while (_context.PHIEUTRAs.Any(p => p.MaPhTra == newId));

            return newId;
        }

        // Kiểm tra điều kiện số lượng sách đang mượn của độc giả
        private bool KiemTraSoLuongSachDangMuon(string maDG)
        {
            int soSachDangMuon = _context.PHIEUMUONs.Where(p => p.MaDG == maDG && p.IsDeleted == false).Count();
            int soSachMuonToiDa = _context.SETTINGs.Select(s => s.SoSachMuonToiDa).FirstOrDefault().GetValueOrDefault();
            if (soSachDangMuon >= soSachMuonToiDa)
            {
                MessageBox.Show("Vui lòng trả sách cũ trước khi mượn thêm.");
                return false;
            }

            return true;
        }

        // Kiểm tra hạn của thẻ độc giả
        private bool KiemTraTheDocGia(string maDG)
        {
            var docGia = _context.DOCGIAs.Where(dg => dg.MaDG == maDG).FirstOrDefault();

            if (docGia == null)
            {
                MessageBox.Show("Không tìm thấy độc giả.");
                return false;
            }

            DateTime ngayLapThe;
            if (docGia.NgayLapThe.HasValue)
            {
                ngayLapThe = docGia.NgayLapThe.Value;
            }
            else
            {
                MessageBox.Show("Độc giả này chưa có ngày lập thẻ.");
                return false;
            }

            int thoiHanThe = _context.SETTINGs.Select(s => s.ThoiHanThe).FirstOrDefault().GetValueOrDefault();
            DateTime ngayHetHan = ngayLapThe.AddMonths(thoiHanThe);

            if (DateTime.Today > ngayHetHan)
            {
                MessageBox.Show("Thẻ của bạn đã hết hạn, vui lòng đăng ký mới.");
                return false;
            }

            return true;
        }

        //Kiếm tra sách mượn quá hạn
        private bool KiemTraSachMuonQuaHan(string maDG)
        {
            DateTime ngayHienTai = DateTime.Today;
            var listPhieuMuon = _context.PHIEUMUONs.Where(pm => pm.MaDG == maDG && pm.IsDeleted == false).ToList();

            foreach (var phieuMuon in listPhieuMuon)
            {
                if (phieuMuon.NgayPhTra.HasValue && phieuMuon.NgayPhTra.Value < ngayHienTai)
                {
                    MessageBox.Show($"Độc giả có sách mượn quá hạn với mã phiếu mượn {phieuMuon.MaPhMuon}");
                    return true;
                }
            }

            return false;
        }

        //Đăng ký mượn sách
        private void Button_Click_DangKy(object sender, RoutedEventArgs e)
        {
            if (docgia.SelectedItem == null || sach.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả và sách.");
                return;
            }

            string maDG = ((DOCGIA)docgia.SelectedItem).MaDG;
            string tenSach = ((SACH)sach.SelectedItem).TenSach;
            string maSach = _context.SACHes.Where(s => s.TenSach == tenSach && s.IsDeleted == false)
                                         .Select(s => s.MaSach)
                                         .FirstOrDefault();
            if (string.IsNullOrEmpty(maSach))
            {
                MessageBox.Show("Không tìm thấy mã sách tương ứng.");
                return;
            }
            if (!_validMaDGs.Contains(maDG))
            {
                MessageBox.Show("Mã độc giả không hợp lệ.");
                return;
            }
            if (!KiemTraSoLuongSachDangMuon(maDG))
            {
                return;
            }
            if (!KiemTraTheDocGia(maDG))
            {
                return;
            }
            if (KiemTraSachMuonQuaHan(maDG))
            {
                return;
            }
            string maPhMuon = generateId("PM", 3);
            DateTime ngayMuon = DateTime.Now;
            PHIEUMUON phieuMuon = new PHIEUMUON
            {
                MaPhMuon = maPhMuon,
                MaDG = maDG,
                MaSach = maSach,
                NgayMuon = ngayMuon,
                IsDeleted = false
            };
            try
            {
                _context.PHIEUMUONs.Add(phieuMuon);
                _context.SaveChanges();
                MessageBox.Show("Đăng ký phiếu mượn thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi đăng ký phiếu mượn: {ex.Message}");
            }
        }
    }
}