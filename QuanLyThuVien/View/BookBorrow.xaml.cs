﻿using MaterialDesignThemes.Wpf;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using QuanLyThuVien.Model;
using QuanLyThuVien.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;

namespace QuanLyThuVien.View
{
    public partial class BookBorrow : UserControl
    {
        private readonly QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private readonly DateTime dateTime = DateTime.Now;
        private string timkiems = null;
        private List<SachDTO> list = new List<SachDTO>();
        public BookBorrow()
        {
            InitializeComponent();
            loadData();
            loadDataTimKiem();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var formPhieuMuon = new FormPhieuMuon();
            formPhieuMuon.Show();
        }


        private void docgia_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.OriginalSource is ScrollViewer scrollViewer && e.VerticalChange != 0)
            {
                var offset = scrollViewer.VerticalOffset;
                var sachScrollViewer = GetScrollViewer(sach);
                if (sachScrollViewer != null)
                {
                    sachScrollViewer.ScrollToVerticalOffset(offset);
                }
            }
        }
        private ScrollViewer GetScrollViewer(DependencyObject dep)
        {
            if (dep is ScrollViewer) return dep as ScrollViewer;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dep); i++)
            {
                var child = VisualTreeHelper.GetChild(dep, i);
                var result = GetScrollViewer(child);
                if (result != null) return result;
            }
            return null;
        }
        private void loadData()
        {
            var maDGs = _context.PHIEUMUON
                                .Where(pm => pm.IsDeleted==false)
                                .Select(pm => pm.MaDG)
                                .Distinct()  //Loại bỏ các giá trị trùng lặp
                                .ToList();

            var docGias = _context.DOCGIA
                                  .Where(dg => maDGs.Contains(dg.MaDG))
                                  .ToList();

            docgia.ItemsSource = docGias;
        }

        private void loadSachByDocGia(DOCGIA docGia)
        {
            var list = new List<object>();

            var dataPhieuMuon = _context.PHIEUMUON
                                       .Where(x => x.MaDG == docGia.MaDG && x.IsDeleted.Value == false)
                                       .ToList();

            DateTime currentDate = DateTime.Now;

            foreach (var data in dataPhieuMuon)
            {
                var checkSach = _context.SACH
                                        .FirstOrDefault(x => x.MaSach == data.MaSach);

                if (checkSach != null)
                {
                    var dataItem = new
                    {
                        id = checkSach.MaSach,
                        tentacgia = checkSach.TacGia,
                        tensach = checkSach.TenSach,
                        ngaytra = data.NgayPhTra.Value,
                        ngaymuon = data.NgayMuon.Value,
                        quahan = data.NgayPhTra < currentDate
                                 ? $"Sách đã quá hạn {Math.Abs((currentDate - data.NgayPhTra.Value).Days)} ngày"
                                 : $"Sách chưa quá hạn, vẫn còn {Math.Abs((data.NgayPhTra.Value - currentDate).Days)} ngày",
                         maPhMuon = data.MaPhMuon
                    };

                    list.Add(dataItem);
                }
            }
            sach.ItemsSource = list;
        }
        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                DOCGIA selectedDocGia = (DOCGIA)docgia.SelectedItem;
                loadSachByDocGia(selectedDocGia);
            }
        }

        private void xulytimkiemchange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)timkiem.SelectedItem;
            timkiems = comboBoxItem.Content.ToString();
            if (!timkiems.Equals(""))
            {
                var data = list.Where(x => x.tensach == timkiems).ToList();
                if (data.Any())
                {
                    List<DOCGIA> dataDocGia = new List<DOCGIA>();
                    for(var i = 0; i < data.LongCount(); i++)
                    {
                        var dataItem = data[i];
                        var checkPhieuMuon = _context.PHIEUMUON.Where(x => x.MaSach == dataItem.id).ToList();
                        foreach(var item in checkPhieuMuon)
                        {
                            var checkDocGia = _context.DOCGIA.Where(x => x.MaDG == item.MaDG).FirstOrDefault();
                            if(checkDocGia != null)
                            {
                                var docgiaItem = new DOCGIA()
                                {
                                    MaDG = checkDocGia.MaDG,
                                    HoTen = checkDocGia.HoTen,
                                    DiaChi = checkDocGia.DiaChi,
                                    Email = checkDocGia.Email,
                                    GioiTinh = checkDocGia.GioiTinh,
                                    LoaiDG = checkDocGia.LoaiDG,
                                    IsDeleted = checkDocGia.IsDeleted,
                                    NgayLapThe = checkDocGia.NgayLapThe,
                                    NgaySinh = checkDocGia.NgaySinh,
                                    SoDT = checkDocGia.SoDT,
                                    PHIEUMUON = checkDocGia.PHIEUMUON,
                                    ACCOUNT = checkDocGia.ACCOUNT
                                };

                                dataDocGia.Add(docgiaItem);
                            }
                        }
                        //var checkTacgia
                    }

                    docgia.ItemsSource = dataDocGia;
                }
                sach.ItemsSource = data;
            }
            else
            {
                sach.ItemsSource = list;
            }


        }

        private void loadDataTimKiem()
        {
            List<ComboBoxItem> listItem = new List<ComboBoxItem>();
            var data = _context.SACH.ToList();
            for(int i = 0; i < data.Count; i++)
            {
                var item = data[i];

                var comboBox = new ComboBoxItem
                {
                    Content = item.TenSach,
                    Tag = item.MaSach
                };

                listItem.Add(comboBox);
            }
            timkiem.ItemsSource = listItem;
        }

        private void findSerch(object sender, TextChangedEventArgs e)
        {
            TextBox text = sender as TextBox;
            string dataItemFind = text.Text;

            if (dataItemFind != null && dataItemFind != "" && !dataItemFind.Equals(""))
            {
                var data = list.Where(x => x.tensach.Contains(dataItemFind)).ToList();
                if (data.Any())
                {
                    List<DOCGIA> dataDocGia = new List<DOCGIA>();
                    for (var i = 0; i < data.LongCount(); i++)
                    {
                        var dataItem = data[i];
                        var checkPhieuMuon = _context.PHIEUMUON.Where(x => x.MaSach == dataItem.id && !x.IsDeleted.Value).ToList();
                        foreach (var item in checkPhieuMuon)
                        {
                            var checkDocGia = _context.DOCGIA.Where(x => x.MaDG == item.MaDG).FirstOrDefault();
                            if (checkDocGia != null)
                            {
                                var docgiaItem = new DOCGIA()
                                {
                                    MaDG = checkDocGia.MaDG,
                                    HoTen = checkDocGia.HoTen,
                                    DiaChi = checkDocGia.DiaChi,
                                    Email = checkDocGia.Email,
                                    GioiTinh = checkDocGia.GioiTinh,
                                    LoaiDG = checkDocGia.LoaiDG,
                                    IsDeleted = checkDocGia.IsDeleted,
                                    NgayLapThe = checkDocGia.NgayLapThe,
                                    NgaySinh = checkDocGia.NgaySinh,
                                    SoDT = checkDocGia.SoDT,
                                    PHIEUMUON = checkDocGia.PHIEUMUON,
                                    ACCOUNT = checkDocGia.ACCOUNT
                                };

                                dataDocGia.Add(docgiaItem);
                            }
                        }
                        //var checkTacgia
                    }

                    docgia.ItemsSource = dataDocGia;
                }
                sach.ItemsSource = data;
            }
            else
            {
                loadData();
                loadDataTimKiem();
            }

        }

        //Xuat du lieu PHIEUTHU ra file excel
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string fileName = "PHIEUTHULIST.xlsx";
                string filePath = @"D:\GITQLTV\SE104.O24---Library-Management-App\" + fileName;

                FileInfo file = new FileInfo(filePath);
                bool fileExists = file.Exists;

                using (ExcelPackage package = fileExists ? new ExcelPackage(file) : new ExcelPackage())
                {
                    ExcelWorksheet worksheet;
                    if (fileExists)
                    {
                        worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "PHIEUTHU");
                        if (worksheet != null)
                        {
                            worksheet.Cells["A2:E1048576"].Clear();
                        }
                    }
                    else
                    {

                        worksheet = package.Workbook.Worksheets.Add("PHIEUTHU");
                    }

                    worksheet.Cells["A1:E1"].Merge = true;
                    worksheet.Cells["A1"].Value = "Danh sách phiếu thu";
                    worksheet.Cells["A1"].Style.Font.Size = 18;
                    worksheet.Cells["A1"].Style.Font.Bold = true;
                    worksheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    worksheet.Cells["A3:E3"].Style.Font.Bold = true;
                    worksheet.Cells[3, 1].Value = "ID";
                    worksheet.Cells[3, 2].Value = "Mã Phiếu trả";
                    worksheet.Cells[3, 3].Value = "Họ tên độc giả";
                    worksheet.Cells[3, 4].Value = "Ngày quá hạn";
                    worksheet.Cells[3, 5].Value = "Số tiền thu";

                    var query = from pt in _context.PHIEUTHU
                                join ptr in _context.PHIEUTRA on pt.MaPhTra equals ptr.MaPhTra
                                join pm in _context.PHIEUMUON on ptr.MaPhMuon equals pm.MaPhMuon
                                join dg in _context.DOCGIA on pm.MaDG equals dg.MaDG
                                where pt.IsDeleted == false
                                select new
                                {
                                    ID = pt.ID,
                                    MaPhTra = pt.MaPhTra,
                                    HoTenDocGia = dg.HoTen,
                                    SoNgayQHan = pt.SoNgayQHan,
                                    SoTienThu = pt.SoTienThu
                                };

                    int row = 4;
                    foreach (var item in query)
                    {
                        worksheet.Cells[row, 1].Value = item.ID;
                        worksheet.Cells[row, 2].Value = item.MaPhTra;
                        worksheet.Cells[row, 3].Value = item.HoTenDocGia;
                        worksheet.Cells[row, 4].Value = item.SoNgayQHan;
                        worksheet.Cells[row, 5].Value = item.SoTienThu;
                        row++;
                    }

                    using (ExcelRange headerCells = worksheet.Cells["A3:E3"])
                    {
                        headerCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        headerCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        headerCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        headerCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    using (ExcelRange dataCells = worksheet.Cells[4, 1, row - 1, 5])
                    {
                        dataCells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        dataCells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        dataCells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        dataCells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    worksheet.Cells.AutoFitColumns();

                    package.Save();

                    MessageBox.Show("Đã cập nhật file Excel thành công: " + filePath, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra khi cập nhật file Excel: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}
