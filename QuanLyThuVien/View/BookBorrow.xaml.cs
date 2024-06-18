using MaterialDesignThemes.Wpf;
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
using System.Globalization;

namespace QuanLyThuVien.View
{
    public partial class BookBorrow : UserControl
    {
        private readonly QLTV_BETAEntities _context = new QLTV_BETAEntities();
        private List<SachDTO> list = new List<SachDTO>();
        public BookBorrow()
        {
            InitializeComponent();
            loadData();
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

        //Load dữ liệu Dộc giả hiện lên grid bên trái kèm tìm kiếm
        private void loadData(string searchText = "")
        {
            searchText = RemoveDiacritics(searchText.ToLower());
            var maDGs = _context.PHIEUMUONs
                                .Where(pm => pm.IsDeleted == false)
                                .Select(pm => pm.MaDG)
                                .Distinct()  //Loại bỏ các giá trị trùng lặp
                                .ToList();

            var docGias = _context.DOCGIAs
                           .Where(dg => maDGs.Contains(dg.MaDG))
                           .ToList()
                           .Where(dg => string.IsNullOrEmpty(searchText) || RemoveDiacritics(dg.HoTen.ToLower()).Contains(searchText))
                           .ToList();

            docgia.ItemsSource = docGias;
        }

        //Load dữ liệu sách hiện lên list bên phải khi nhấn vào độc giả
        private void loadSachByDocGia(DOCGIA docGia, string searchText = "")
        {
            var list = new List<object>();

            var dataPhieuMuon = _context.PHIEUMUONs
                                       .Where(x => x.MaDG == docGia.MaDG && x.IsDeleted.Value == false)
                                       .ToList();

            DateTime currentDate = DateTime.Now;
            var sachList = _context.SACHes.ToList();
            int soTienNopTre = _context.SETTINGs.FirstOrDefault()?.SoTienNopTre ?? 0;

            foreach (var data in dataPhieuMuon)
            {
                var checkSach = sachList.FirstOrDefault(x => x.MaSach == data.MaSach);

                if (checkSach != null)
                {
                    if (data.NgayPhTra != null)
                    {
                        TimeSpan difference = currentDate - data.NgayPhTra.Value;
                        int soNgayQuaHan = (int)Math.Ceiling(difference.TotalDays);

                        if (soNgayQuaHan > 0)
                        {
                            int soTienPhat = soNgayQuaHan * soTienNopTre;
                            var dataItem = new
                            {
                                MaPhMuon = data.MaPhMuon,
                                tentacgia = checkSach.TacGia,
                                tensach = checkSach.TenSach,
                                ngaytra = data.NgayPhTra.Value.ToString("dd/MM/yyyy"),
                                ngaymuon = data.NgayMuon.Value.ToString("dd/MM/yyyy"),
                                quahan = $"{soNgayQuaHan} ngày, {soTienPhat} đồng",
                                maPhMuon = data.MaPhMuon
                            };
                            list.Add(dataItem);
                        }
                        else
                        {
                            var dataItem = new
                            {
                                MaPhMuon = data.MaPhMuon,
                                tentacgia = checkSach.TacGia,
                                tensach = checkSach.TenSach,
                                ngaytra = data.NgayPhTra.Value.ToString("dd/MM/yyyy"),
                                ngaymuon = data.NgayMuon.Value.ToString("dd/MM/yyyy"),
                                quahan = "Sách chưa quá hạn",
                                maPhMuon = data.MaPhMuon
                            };
                            list.Add(dataItem);
                        }
                    }
                    else
                    {
                        var dataItem = new
                        {
                            MaPhMuon = data.MaPhMuon,
                            tentacgia = checkSach.TacGia,
                            tensach = checkSach.TenSach,
                            ngaytra = "(Chưa xác định)",
                            ngaymuon = data.NgayMuon.Value,
                            quahan = "(Chưa xác định)",
                            maPhMuon = data.MaPhMuon
                        };
                        list.Add(dataItem);
                    }
                }
            }

            sach.ItemsSource = list;
        }


        //Khi nhấn vào tên độc giả ở grid bên trái, load danh sách các sách mà độc giả đang mượn
        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                DOCGIA selectedDocGia = (DOCGIA)docgia.SelectedItem;
                string searchText = timKiemBox.Text;
                loadSachByDocGia(selectedDocGia, searchText);
            }
        }

        //Chức năng nhập tìm kiếm
        private void timKiemBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (timkiemSelector.SelectedItem != null)
            {
                var selectedOption = (timkiemSelector.SelectedItem as ComboBoxItem)?.Content.ToString();
                string searchText = timKiemBox.Text;

                if (selectedOption == "Tên độc giả")
                {
                    loadData(searchText);
                }
                else if (selectedOption == "Tên sách" && docgia.SelectedItem != null)
                {
                    DOCGIA selectedDocGia = (DOCGIA)docgia.SelectedItem;
                    loadSachByDocGia(selectedDocGia, searchText);
                }
            }
        }

        //Dùng để chọn tiêu chí tìm kiếm. Gọi lại tìm kiếm khi tiêu chí tìm kiếm thay đổi
        private void timkiemSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timKiemBox_TextChanged(sender, null);
        }

        //Chức năng bỏ dấu của chuỗi tìm kiếm, dùng để tìm kiếm khi nhập không dấu
        private string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        //Mở Form điền phiếu mượn
        private void Button_Click_TaoPhieuMuon(object sender, RoutedEventArgs e)
        {
            var formPhieuMuon = new FormPhieuMuon();
            formPhieuMuon.Show();
        }

        //Mở form tạo phiếu trả
        private void Button_Click_TaoPhieuTra(object sender, RoutedEventArgs e)
        {
            var formPhieuTra = new FormPhieuTra();
            formPhieuTra.Show();
        }

        //Xuat du lieu PHIEUTHU ra file excel
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var phieuThuExport = new PhieuThuExport();
            phieuThuExport.Show();
        }



    }
}