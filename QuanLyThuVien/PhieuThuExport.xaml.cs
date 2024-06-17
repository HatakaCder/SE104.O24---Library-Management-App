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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data.Entity;

namespace QuanLyThuVien
{
    public partial class PhieuThuExport : Window
    {
        private QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private List<DOCGIA> _allDocGia;
        private List<string> _validMaDGs;

        public PhieuThuExport()
        {
            InitializeComponent();
            Loaded += Window_Loaded_1;
            docgia.SelectionChanged += docgia_SelectionChanged;
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
            LoadDataDocGia();
        }

        private void LoadDataDocGia()
        {
            try
            {
                // Truy vấn danh sách MaDG có trong bảng PHIEUTHU
                var docGiaWithPhieuThu = (from pt in _context.PHIEUTHU
                                          join ptr in _context.PHIEUTRA on pt.MaPhTra equals ptr.MaPhTra
                                          join pm in _context.PHIEUMUON on ptr.MaPhMuon equals pm.MaPhMuon
                                          select pm.MaDG).Distinct().ToList();

                // Truy vấn danh sách thông tin độc giả tương ứng
                var newDocGiaList = _context.DOCGIA
                                           .Where(d => docGiaWithPhieuThu.Contains(d.MaDG))
                                           .ToList();

                // Thêm lựa chọn "Tất cả"
                var allOption = new DOCGIA { MaDG = "Tất cả" };
                newDocGiaList.Insert(0, allOption);

                // Xóa bỏ các items hiện tại của ComboBox trước khi gán mới ItemsSource
                Dispatcher.Invoke(() =>
                {
                    docgia.ItemsSource = null;
                    docgia.Items.Clear();
                    docgia.ItemsSource = newDocGiaList;
                    docgia.DisplayMemberPath = "MaDG";
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lấy data độc giả: " + ex.Message);
            }
        }

        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                var selectedDocGia = docgia.SelectedItem as DOCGIA;

                if (selectedDocGia != null && selectedDocGia.MaDG == "Tất cả")
                {
                    txtHoTen.Visibility = Visibility.Collapsed;
                    txtNgaySinh.Visibility = Visibility.Collapsed;
                }
                else if (selectedDocGia != null)
                {
                    txtHoTen.Visibility = Visibility.Visible;
                    txtNgaySinh.Visibility = Visibility.Visible;

                    txtHoTen.Text = selectedDocGia.HoTen;
                    txtNgaySinh.Text = selectedDocGia.NgaySinh?.ToString("dd/MM/yyyy") ?? string.Empty;
                }
            }
        }

        private void docgia_TextChanged(object sender, TextChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tb = comboBox?.Template.FindName("PART_EditableTextBox", comboBox) as TextBox;

            if (comboBox != null && tb != null && !string.IsNullOrEmpty(tb.Text))
            {
                var filterText = tb.Text;

                try
                {
                    if (_allDocGia != null)
                    {
                        var filteredList = _allDocGia
                            .Where(d => d.MaDG.IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0)
                            .ToList();

                        comboBox.ItemsSource = filteredList;
                        comboBox.IsDropDownOpen = true;

                        // Adjust selection and caret position
                        tb.SelectionStart = filterText.Length;
                        tb.SelectionLength = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi lọc danh sách độc giả: " + ex.Message);
                }
            }
        }


        private void Button_Click_ExcelExport(object sender, RoutedEventArgs e)
        {
            string fileName = "PHIEUTHULIST.xlsx";
            string filePath = @"D:\GITQLTV\SE104.O24---Library-Management-App\" + fileName;

            try
            {
                string directory = System.IO.Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                FileInfo file = new FileInfo(filePath);
                bool fileExists = file.Exists;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (ExcelPackage package = new ExcelPackage(file))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "PHIEUTHU");

                    if (worksheet == null)
                    {
                        worksheet = package.Workbook.Worksheets.Add("PHIEUTHU");
                    }
                    else
                    {
                        worksheet.Cells[worksheet.Dimension.Address].Clear();
                    }

                    worksheet.Cells["A1:F1"].Merge = true;
                    worksheet.Cells["A1"].Value = "Danh sách phiếu thu";
                    worksheet.Cells["A1"].Style.Font.Size = 18;
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells["A3:F3"].Style.Font.Bold = true;
                    worksheet.Cells[3, 1].Value = "ID";
                    worksheet.Cells[3, 2].Value = "Mã Phiếu trả";
                    worksheet.Cells[3, 3].Value = "Họ tên độc giả";
                    worksheet.Cells[3, 4].Value = "Tên sách mượn";
                    worksheet.Cells[3, 5].Value = "Ngày quá hạn";
                    worksheet.Cells[3, 6].Value = "Số tiền thu";

                    if (docgia.SelectedItem is DOCGIA selectedDocGia && selectedDocGia.MaDG != "Tất cả")
                    {
                        var query = from pt in _context.PHIEUTHU
                                    join ptr in _context.PHIEUTRA on pt.MaPhTra equals ptr.MaPhTra
                                    join pm in _context.PHIEUMUON on ptr.MaPhMuon equals pm.MaPhMuon
                                    join dg in _context.DOCGIA on pm.MaDG equals dg.MaDG
                                    join sach in _context.SACH on pm.MaSach equals sach.MaSach
                                    where dg.MaDG == selectedDocGia.MaDG
                                    select new
                                    {
                                        pt.ID,
                                        pt.MaPhTra,
                                        dg.HoTen,
                                        sach.TenSach,
                                        pt.SoNgayQHan,
                                        pt.SoTienThu
                                    };

                        int rowIndex = 4;
                        foreach (var item in query)
                        {
                            worksheet.Cells[rowIndex, 1].Value = item.ID;
                            worksheet.Cells[rowIndex, 2].Value = item.MaPhTra;
                            worksheet.Cells[rowIndex, 3].Value = item.HoTen;
                            worksheet.Cells[rowIndex, 4].Value = item.TenSach;
                            worksheet.Cells[rowIndex, 5].Value = item.SoNgayQHan;
                            worksheet.Cells[rowIndex, 6].Value = item.SoTienThu;
                            rowIndex++;
                        }
                    }
                    else
                    {
                        var query = from pt in _context.PHIEUTHU
                                    join ptr in _context.PHIEUTRA on pt.MaPhTra equals ptr.MaPhTra
                                    join pm in _context.PHIEUMUON on ptr.MaPhMuon equals pm.MaPhMuon
                                    join dg in _context.DOCGIA on pm.MaDG equals dg.MaDG
                                    join sach in _context.SACH on pm.MaSach equals sach.MaSach
                                    select new
                                    {
                                        pt.ID,
                                        pt.MaPhTra,
                                        dg.HoTen,
                                        sach.TenSach,
                                        pt.SoNgayQHan,
                                        pt.SoTienThu
                                    };

                        int rowIndex = 4;
                        foreach (var item in query)
                        {
                            worksheet.Cells[rowIndex, 1].Value = item.ID;
                            worksheet.Cells[rowIndex, 2].Value = item.MaPhTra;
                            worksheet.Cells[rowIndex, 3].Value = item.HoTen;
                            worksheet.Cells[rowIndex, 4].Value = item.TenSach;
                            worksheet.Cells[rowIndex, 5].Value = item.SoNgayQHan;
                            worksheet.Cells[rowIndex, 6].Value = item.SoTienThu;
                            rowIndex++;
                        }
                    }

                    worksheet.Cells.AutoFitColumns();

                    package.Save();

                    MessageBox.Show("Danh sách phiếu thu đã được xuất thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi xuất danh sách phiếu thu: " + ex.Message);
            }
        }

    }


}
