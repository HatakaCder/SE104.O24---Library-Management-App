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

        public PhieuThuExport()
        {
            InitializeComponent();
            Loaded += Window_Loaded_1;
            docgia.SelectionChanged += docgia_SelectionChanged;
        }

        //Kéo thả Form khi kéo border trên
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        //Nút close Form
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadDataDocGia();
        }

        //Chọn chế độ export
        private void chedo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (chedo.SelectedItem != null)
            {
                var selectedMode = (chedo.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (selectedMode == "Tất cả")
                {
                    docgia.IsEnabled = false;
                    docgia.SelectedIndex = -1;
                    txtHoTen.Visibility = Visibility.Collapsed;
                    txtNgaySinh.Visibility = Visibility.Collapsed;
                }
                else if (selectedMode == "Theo Mã độc giả")
                {
                    docgia.IsEnabled = true;
                }
            }
        }
        //Lấy data để hiển thị trên Combobox
        private void LoadDataDocGia()
        {
            try
            {
                var docGiaWithPhieuThu = (from pt in _context.PHIEUTHU
                                          join ptr in _context.PHIEUTRA on pt.MaPhTra equals ptr.MaPhTra
                                          join pm in _context.PHIEUMUON on ptr.MaPhMuon equals pm.MaPhMuon
                                          select pm.MaDG).Distinct().ToList();
                var newDocGiaList = _context.DOCGIA
                                           .Where(d => docGiaWithPhieuThu.Contains(d.MaDG))
                                           .ToList();

                Dispatcher.Invoke(() =>
                {
                    docgia.ItemsSource = null;
                    docgia.Items.Clear();
                    docgia.ItemsSource = newDocGiaList;
                    docgia.DisplayMemberPath = "MaDG";
                });

                _allDocGia = newDocGiaList; // Lưu trữ toàn bộ danh sách cho lọc tìm kiếm
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi lấy data độc giả: " + ex.Message);
            }
        }

        //Xử lí khi chọn đúng MaDG
        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                var selectedDocGia = docgia.SelectedItem as DOCGIA;

                if (selectedDocGia != null)
                {
                    txtHoTen.Visibility = Visibility.Visible;
                    txtNgaySinh.Visibility = Visibility.Visible;

                    txtHoTen.Text = selectedDocGia.HoTen;
                    txtNgaySinh.Text = selectedDocGia.NgaySinh?.ToString("dd/MM/yyyy") ?? string.Empty;
                }
            }
        }

        //Xử lí nhập
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

        //Tạo Excel
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

                    var selectedMode = (chedo.SelectedItem as ComboBoxItem)?.Content.ToString();

                    IQueryable<dynamic> query = null;

                    if (selectedMode == "Theo Mã độc giả" && docgia.SelectedItem is DOCGIA selectedDocGia && !string.IsNullOrEmpty(selectedDocGia.MaDG))
                    {
                        query = from pt in _context.PHIEUTHU
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
                    }
                    else if (selectedMode == "Tất cả")
                    {
                        query = from pt in _context.PHIEUTHU
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
                    }

                    if (query != null)
                    {
                        int rowIndex = 4;
                        int totalAmount = 0;

                        foreach (var item in query)
                        {
                            worksheet.Cells[rowIndex, 1].Value = item.ID;
                            worksheet.Cells[rowIndex, 2].Value = item.MaPhTra;
                            worksheet.Cells[rowIndex, 3].Value = item.HoTen;
                            worksheet.Cells[rowIndex, 4].Value = item.TenSach;
                            worksheet.Cells[rowIndex, 5].Value = item.SoNgayQHan;
                            worksheet.Cells[rowIndex, 6].Value = item.SoTienThu;
                            totalAmount += item.SoTienThu;
                            rowIndex++;
                        }

                        // Tính tổng số tiền thu và ghi vào dòng cuối cùng
                        worksheet.Cells[rowIndex, 1, rowIndex, 5].Merge = true;
                        worksheet.Cells[rowIndex, 1].Value = "Tổng tiền";
                        worksheet.Cells[rowIndex, 1].Style.Font.Bold = true;
                        worksheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

                        worksheet.Cells[rowIndex, 6].Value = totalAmount;

                        worksheet.Cells[rowIndex, 1, rowIndex, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowIndex, 1, rowIndex, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Double;

                        package.Save();
                    }
                }

                MessageBox.Show("Xuất Excel thành công!");

                // Đóng cửa sổ sau khi xuất Excel thành công
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi xuất Excel: " + ex.Message);
            }
        }


    }
}
