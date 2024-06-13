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

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for BookBorrow.xaml
    /// </summary>
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
            AddOrEditMuonTra muontra = new AddOrEditMuonTra(false);
            muontra.Show();
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
            // Lấy danh sách các MaDG có trong PHIEUMUON nhưng chỉ lấy một lần duy nhất cho mỗi MaDG
            var maDGs = _context.PHIEUMUON
                                .Where(pm => pm.IsDeleted==false)
                                .Select(pm => pm.MaDG)
                                .Distinct()  //Loại bỏ các giá trị trùng lặp
                                .ToList();

            // Lấy thông tin độc giả dựa trên MaDG
            var docGias = _context.DOCGIA
                                  .Where(dg => maDGs.Contains(dg.MaDG))
                                  .ToList();

            // Gán dữ liệu vào DataGrid
            docgia.ItemsSource = docGias;
        }

        private void loadSachByDocGia(DOCGIA docGia)
        {
            // Tạo danh sách để chứa kết quả
            var list = new List<object>();

            // Lấy tất cả các phiếu mượn của độc giả có mã MaDG và chưa bị xóa
            var dataPhieuMuon = _context.PHIEUMUON
                                       .Where(x => x.MaDG == docGia.MaDG && x.IsDeleted.Value == false)
                                       .ToList();

            // Lấy thời gian hiện tại
            DateTime currentDate = DateTime.Now;

            // Duyệt qua tất cả các phiếu mượn
            foreach (var data in dataPhieuMuon)
            {
                // Lấy thông tin sách
                var checkSach = _context.SACH
                                        .FirstOrDefault(x => x.MaSach == data.MaSach);

                // Nếu sách tồn tại
                if (checkSach != null)
                {
                    // Tạo đối tượng ẩn danh chứa thông tin cần thiết
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

                    // Thêm vào danh sách
                    list.Add(dataItem);
                }
            }

            // Gán dữ liệu vào DataGrid
            sach.ItemsSource = list;
        }

        private object selectedPhieuMuon; //Dùng để truyền Mã phiếu mượn vào form mượn sách.
        private void Data_Sach(object sender, SelectionChangedEventArgs e)
        {
            if (sach.SelectedItem != null)
            {
                selectedPhieuMuon = sach.SelectedItem;
            }
        }
        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                DOCGIA selectedDocGia = (DOCGIA)docgia.SelectedItem;

                // Gọi hàm loadSachByDocGia để load danh sách sách mượn của độc giả này
                loadSachByDocGia(selectedDocGia);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddOrEditMuonTra muontra;

            if (selectedPhieuMuon != null)
            {
                string maPhMuon = ((dynamic)selectedPhieuMuon).id;
                muontra = new AddOrEditMuonTra(true, maPhMuon);
            }
            else
            {
                muontra = new AddOrEditMuonTra(true);
            }

            muontra.Show();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PhieuThu pt = new PhieuThu();
            pt.Show();
        }
    }
}
