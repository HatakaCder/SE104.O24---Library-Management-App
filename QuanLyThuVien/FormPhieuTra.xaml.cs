using QuanLyThuVien.Model;
using System.Configuration;
using System;
using System.Data.Entity;
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
    public partial class FormPhieuTra : Window
    {
        private QLTV_BETAEntities _context = new QLTV_BETAEntities();

        public FormPhieuTra()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadMaPhieuMuon();
        }

        //Hiển thị mã phiếu mượn để chọn trong combobox cho phiếu trả
        private void LoadMaPhieuMuon()
        {
            try
            {
                var listPhieuMuon = _context.PHIEUMUON
                                            .Where(pm => pm.IsDeleted == false)
                                            .Select(pm => new { pm.MaPhMuon })
                                            .ToList();

                maphieumuon1.ItemsSource = listPhieuMuon;
                maphieumuon1.DisplayMemberPath = "MaPhMuon";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu phiếu mượn: " + ex.Message);
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
            while (_context.PHIEUTRA.Any(p => p.MaPhTra == newId));

            return newId;
        }

        //Kiểm tra sách có mượn quá hạn khi trả sách không
        private bool KiemTraQuaHan(string maPhMuon)
        {
            var phieuMuon = _context.PHIEUMUON
                                    .Where(pm => pm.MaPhMuon == maPhMuon)
                                    .FirstOrDefault();
            if (phieuMuon == null)
            {
                MessageBox.Show("Không tìm thấy thông tin phiếu mượn.");
                return false;
            }
            DateTime ngayTra = DateTime.Today;
            if (phieuMuon.NgayPhTra.HasValue && phieuMuon.NgayPhTra.Value < ngayTra)
            {
                int soNgayQuaHan = (ngayTra - phieuMuon.NgayPhTra.Value).Days;
                int soTienNopTre = _context.SETTING.Select(s => s.SoTienNopTre).FirstOrDefault().GetValueOrDefault();
                MessageBoxResult result = MessageBox.Show($"Đã quá hạn {soNgayQuaHan} ngày, vui lòng thanh toán {soNgayQuaHan * soTienNopTre} VND.", "Thông báo", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        // Xử lý sự kiện TextChanged của maphieumuon1
        private void maphieumuon1_TextChanged(object sender, TextChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tb = comboBox?.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;

            if (comboBox != null && tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                var filterText = tb.Text;

                var filteredList = _context.PHIEUMUON
                        .Include(pm => pm.DOCGIA)
                        .Include(pm => pm.SACH)
                        .Where(pm => pm.MaPhMuon.Contains(filterText))
                        .Select(pm => new
                        {
                            pm.MaPhMuon,
                            pm.DOCGIA.HoTen,
                            pm.DOCGIA.NgaySinh,
                            pm.SACH.TenSach
                        })
                        .ToList();
                comboBox.ItemsSource = filteredList;
                comboBox.DisplayMemberPath = "MaPhMuon"; // Hiển thị MaPhMuon
                comboBox.IsDropDownOpen = true;

                // Adjust selection and caret position
                tb.SelectionStart = filterText.Length;
                tb.SelectionLength = 0;
            }
        }

        // Xử lý sự kiện SelectionChanged của maphieumuon1
        private void maphieumuon1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (maphieumuon1.SelectedItem != null)
            {
                dynamic selectedPhieuMuon = maphieumuon1.SelectedItem;
                if (selectedPhieuMuon != null)
                {
                    // Hiển thị thông tin DOCGIA
                    txtHoTen.Text = selectedPhieuMuon.HoTen;
                    txtNgaySinh.Text = selectedPhieuMuon.NgaySinh?.ToString("dd/MM/yyyy") ?? string.Empty;

                    // Hiển thị thông tin SACH
                    txtTenSach.Text = selectedPhieuMuon.TenSach;
                }
            }
        }

        // Xử lý sự kiện Click của Button TraSach
        private void Button_Click_TraSach(object sender, RoutedEventArgs e)
        {
            dynamic selectedItem = maphieumuon1.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã phiếu mượn.");
                return;
            }

            string maPhMuon = selectedItem.MaPhMuon;

            if (KiemTraQuaHan(maPhMuon))
            {
                string maPhTra = generateId("PT", 3);
                DateTime ngayTra = DateTime.Now;

                PHIEUTRA phieuTra = new PHIEUTRA
                {
                    MaPhTra = maPhTra,
                    MaPhMuon = maPhMuon,
                    NgayTra = ngayTra,
                    IsDeleted = false
                };

                try
                {
                    _context.PHIEUTRA.Add(phieuTra);
                    var phieuMuon = _context.PHIEUMUON.FirstOrDefault(pm => pm.MaPhMuon == maPhMuon);
                    if (phieuMuon != null)
                    {
                        phieuMuon.IsDeleted = true;
                    }
                    _context.SaveChanges();

                    MessageBox.Show("Lập phiếu trả sách thành công!");

                    this.Close();
                }
                catch (Exception ex)
                {
                    // Bắt và hiển thị chi tiết lỗi InnerException để debug
                    string errorMessage = $"Đã xảy ra lỗi khi lập phiếu trả sách: {ex.Message}";
                    Exception innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        errorMessage += $"\nInner Exception: {innerException.Message}";
                        innerException = innerException.InnerException;
                    }
                    MessageBox.Show(errorMessage);
                }
            }
        }
    }
}
