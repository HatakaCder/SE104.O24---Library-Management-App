using QuanLyThuVien.Model;
using System;
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
        public FormPhieuMuon()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
            LoadDataDocGia();
            LoadDataSach();
            LoadMaPhieuMuon();
        }

        //Hiển thị data độc giả để chọn trong combobox
        private void LoadDataDocGia()
        {
            try
            {
                var listDocGia = _context.DOCGIA.ToList();
                docgia.ItemsSource = listDocGia;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu độc giả: " + ex.Message);
            }
        }

        //Hiển thị data sách để chọn trong combobox
        private void LoadDataSach()
        {
            try
            {
                var listSach = _context.SACH.Where(s => s.IsDeleted == false).ToList();
                sach.ItemsSource = listSach;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu sách: " + ex.Message);
            }
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
            string entityConnectionString = @"metadata=res://*/Model.Model1.csdl|res://*/Model.Model1.ssdl|res://*/Model.Model1.msl;
                                     provider=System.Data.SqlClient;
                                     provider connection string=&quot;data source=LAPTOP_OF_LAN;
                                     initial catalog=QLTV_BETA;
                                     integrated security=True;
                                     encrypt=False;
                                     MultipleActiveResultSets=True;
                                     App=EntityFramework&quot;";

            string sqlConnectionString = new EntityConnectionStringBuilder(entityConnectionString).ProviderConnectionString;

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

        // Kiểm tra điều kiện số lượng sách đang mượn của độc giả
        private bool KiemTraSoLuongSachDangMuon(string maDG)
        {
            int soSachDangMuon = _context.PHIEUMUON.Where(p => p.MaDG == maDG && p.IsDeleted == false).Count();
            int soSachMuonToiDa = _context.SETTING.Select(s => s.SoSachMuonToiDa).FirstOrDefault().GetValueOrDefault();
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
            var docGia = _context.DOCGIA.Where(dg => dg.MaDG == maDG).FirstOrDefault();

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

            int thoiHanThe = _context.SETTING.Select(s => s.ThoiHanThe).FirstOrDefault().GetValueOrDefault();
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
            var listPhieuMuon = _context.PHIEUMUON.Where(pm => pm.MaDG == maDG && pm.IsDeleted == false).ToList();

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
            if (docgia.SelectedValue == null || sach.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn độc giả và sách.");
                return;
            }

            string maDG = docgia.SelectedValue.ToString();
            string maSach = sach.SelectedValue.ToString();

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
                _context.PHIEUMUON.Add(phieuMuon);
                _context.SaveChanges();
                MessageBox.Show("Đăng ký phiếu mượn thành công!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi đăng ký phiếu mượn: {ex.Message}");
            }
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

        //Trả sách
        private void Button_Click_TraSach(object sender, RoutedEventArgs e)
        {
            var selectedItem = maphieumuon1.SelectedItem;
            if (selectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn mã phiếu mượn.");
                return;
            }

            string maPhMuon = (selectedItem as dynamic).MaPhMuon;

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