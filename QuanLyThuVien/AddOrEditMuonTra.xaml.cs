﻿using QuanLyThuVien.Model;
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
    /// <summary>
    /// Interaction logic for AddOrEditMuonTra.xaml
    /// </summary>
    public partial class AddOrEditMuonTra : Window
    {
        private QLTV_BETAEntities _context = new QLTV_BETAEntities();
        public AddOrEditMuonTra()
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

        private void LoadMaPhieuMuon()
        {
            // Lấy danh sách các mã phiếu mượn có IsDeleted = false từ database
            var listMaPhieuMuon = _context.PHIEUMUON
                                        .Where(p => p.IsDeleted == false)
                                        .Select(p => p.MaPhMuon)
                                        .ToList();

            // Đặt ItemSource của ComboBox maphieumuon1 bằng danh sách vừa lấy
            maphieumuon1.ItemsSource = listMaPhieuMuon;
        }
        //Hàm tạo ID
        //Lấy id cao nhất hiện tại trong database để tạo id mới
        private int GetCurrentIdNumberFromDatabase(string prefix)
        {
            string entityConnectionString = @"metadata=res://*/Model.Model1.csdl|res://*/Model.Model1.ssdl|res://*/Model.Model1.msl;
                                            provider=System.Data.SqlClient;
                                            provider connection string='data source=.;
                                            initial catalog=QLTV_BETA;
                                            integrated security=True;
                                            encrypt=False;
                                            application name=EntityFramework;
                                            MultipleActiveResultSets=True'";

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
            // Lấy số ID hiện tại từ cơ sở dữ liệu
            int currentNumber = GetCurrentIdNumberFromDatabase(prefix);

            // Tăng số lên 1
            currentNumber++;

            // Định dạng số với độ dài yêu cầu (có thể dùng PadLeft)
            string numberPart = currentNumber.ToString().PadLeft(length, '0'); //Nếu là PM001 thì length là 3, PM01 thì length là 2.

            // Kết hợp tiền tố và số để tạo ID mới
            string newId = prefix + numberPart;

            return newId;
        }
        // Kiểm tra số lượng sách đang mượn của độc giả
        private bool KiemTraSoLuongSachDangMuon(string maDG)
        {
            int soSachDangMuon = _context.PHIEUMUON.Where(p => p.MaDG == maDG && p.IsDeleted == false).Count();
            int soSachMuonToiDa = _context.SETTING.Select(s => s.SoSachMuonToiDa).FirstOrDefault();

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

            int thoiHanThe = _context.SETTING.Select(s => s.ThoiHanThe).FirstOrDefault();
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
        //Đăng ký sách mới
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

        private bool KiemTraQuaHan(string maPhMuon)
        {
            // Lấy thông tin phiếu mượn từ cơ sở dữ liệu
            var phieuMuon = _context.PHIEUMUON
                                    .Where(pm => pm.MaPhMuon == maPhMuon)
                                    .FirstOrDefault();

            if (phieuMuon == null)
            {
                MessageBox.Show("Không tìm thấy thông tin phiếu mượn.");
                return false;
            }

            // Lấy ngày hiện tại
            DateTime ngayTra = DateTime.Today;

            // Kiểm tra xem có quá hạn trả sách hay không
            if (phieuMuon.NgayPhTra.HasValue && phieuMuon.NgayPhTra.Value < ngayTra)
            {
                // Tính số ngày quá hạn
                int soNgayQuaHan = (ngayTra - phieuMuon.NgayPhTra.Value).Days;

                // Lấy giá trị tiền phạt từ bảng SETTING
                int soTienNopTre = _context.SETTING
                                          .Select(s => s.SoTienNopTre)
                                          .FirstOrDefault();

                // Hiển thị MessageBox cùng với các nút "Thanh toán" và "Không thanh toán"
                MessageBoxResult result = MessageBox.Show($"Đã quá hạn {soNgayQuaHan} ngày, vui lòng thanh toán {soNgayQuaHan * soTienNopTre} VND.", "Thông báo", MessageBoxButton.YesNo);

                // Xử lý theo kết quả của MessageBox
                if (result == MessageBoxResult.Yes)
                {
                    // Nếu người dùng chọn "Thanh toán", cho phép tạo phiếu trả
                    return true;
                }
                else
                {
                    // Nếu người dùng chọn "Không thanh toán", không cho phép tạo phiếu trả
                    return false;
                }
            }

            // Nếu không quá hạn, cho phép thực hiện trả sách bình thường
            return true;
        }

        //Trả sách
        private void Button_Click_TraSach(object sender, RoutedEventArgs e)
        {
            // Lấy MaPhMuon từ ComboBox maphieumuon1
            string maPhMuon = maphieumuon1.SelectedItem as string;

            if (maPhMuon == null)
            {
                MessageBox.Show("Vui lòng chọn mã phiếu mượn.");
                return;
            }

            // Kiểm tra quá hạn khi trả sách
            if (KiemTraQuaHan(maPhMuon))
            {
                // Tạo MaPhTra mới
                string maPhTra = generateId("PT", 3);

                // Lấy ngày hiện tại làm NgayTra
                DateTime ngayTra = DateTime.Now;

                // Tạo đối tượng PHIEUTRA mới
                PHIEUTRA phieuTra = new PHIEUTRA
                {
                    MaPhTra = maPhTra,
                    MaPhMuon = maPhMuon,
                    NgayTra = ngayTra,
                    IsDeleted = false // Mặc định không bị xóa khi tạo mới
                };

                try
                {
                    // Thêm PHIEUTRA vào cơ sở dữ liệu
                    _context.PHIEUTRA.Add(phieuTra);
                    _context.SaveChanges();

                    // Hiển thị thông báo thành công
                    MessageBox.Show("Lập phiếu trả sách thành công!");

                    // Đóng form sau khi lưu thành công
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Đã xảy ra lỗi khi lập phiếu trả sách: {ex.Message}");
                }
            }
        }

    }
}