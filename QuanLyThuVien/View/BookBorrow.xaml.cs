using MaterialDesignThemes.Wpf;
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
            List<DOCGIA> list = new List<DOCGIA>();
            var data = _context.DOCGIAs.ToList();

            for(int i = 0; i < data.LongCount(); i++)
            {
                var dataItem = data[i];

                var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaDG == dataItem.MaDG && !x.IsDeleted.Value).ToList();
                
                if (checkPhieuMuon.Any())
                {
                    foreach(var item in checkPhieuMuon)
                    {
                        var checlkPhieuTra = _context.PHIEUTRAs.Where(x => x.MaPhMuon == item.MaPhMuon).ToList();
                        if (checlkPhieuTra.Any())
                        {
                            var docgiaItem = new DOCGIA()
                            {
                                MaDG = dataItem.MaDG,
                                DiaChi = dataItem.DiaChi,
                                Email = dataItem.Email,
                                LoaiDG = dataItem.LoaiDG,
                                IsDeleted = dataItem.IsDeleted,
                                NgayLapThe = dataItem.NgayLapThe,
                                NgaySinh = dataItem.NgaySinh,
                                PHIEUMUONs = dataItem.PHIEUMUONs,
                                ACCOUNTs = dataItem.ACCOUNTs
                            };

                            list.Add(docgiaItem);
                        }
                    }
                    
                }
                
            }

            docgia.ItemsSource = list;
        }

        private void loadSachByDocGia(string maDG)
        {
            List<SachDTO> list = new List<SachDTO>();
            var dataPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaDG == maDG && !x.IsDeleted.Value).ToList();

            foreach (var data in dataPhieuMuon)
            {
                var checkPhieuTra = _context.PHIEUTRAs.Where(x => x.MaPhMuon == data.MaPhMuon).ToList();
                if (checkPhieuTra.Any())
                {
                    var checkSach = _context.SACHes.Where(x => x.MaSach == data.MaSach).FirstOrDefault();
                    var checkTacgia = _context.DOCGIAs.Where(x => x.MaDG == data.MaDG).FirstOrDefault();

                    if (checkSach != null && checkTacgia != null)
                    {
                        var dataItem = new SachDTO();
                        if (data.NgayPhTra < dateTime)
                        {
                            TimeSpan chenhlech = dateTime.Subtract(data.NgayPhTra.Value);
                            int chuyenDoiInt = Math.Abs(chenhlech.Days);
                            dataItem.quahan = "Sách đã quá hạn " + chuyenDoiInt + " ngày";
                        }
                        else
                        {
                            TimeSpan chenhlechChuaQuaHan = data.NgayPhTra.Value.Subtract(dateTime);
                            int chuyenDoiIntChuaQuaHan = Math.Abs(chenhlechChuaQuaHan.Days);
                            dataItem.quahan = "Sách chưa quá hạn, vẫn còn " + chuyenDoiIntChuaQuaHan + " ngày";
                        }

                        dataItem.id = checkSach.MaSach;
                        dataItem.tentacgia = checkTacgia.Email;
                        dataItem.tensach = checkSach.TenSach;
                        dataItem.ngaytra = data.NgayPhTra.Value;
                        dataItem.ngaymuon = data.NgayMuon.Value;

                        list.Add(dataItem);
                    }
                }
            }

            sach.ItemsSource = list;
        }

        private void docgia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                DOCGIA selectedDocGia = (DOCGIA)docgia.SelectedItem;
                loadSachByDocGia(selectedDocGia.MaDG);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddOrEditMuonTra muontra = new AddOrEditMuonTra(true);
            muontra.Show();
        }

        private void Data_Sach(object sender, SelectionChangedEventArgs e)
        {

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
                        var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaSach == dataItem.id).ToList();
                        foreach(var item in checkPhieuMuon)
                        {
                            var checkDocGia = _context.DOCGIAs.Where(x => x.MaDG == item.MaDG).FirstOrDefault();
                            if(checkDocGia != null)
                            {
                                var docgiaItem = new DOCGIA()
                                {
                                    MaDG = checkDocGia.MaDG,
                                    DiaChi = checkDocGia.DiaChi,
                                    Email = checkDocGia.Email,
                                    LoaiDG = checkDocGia.LoaiDG,
                                    IsDeleted = checkDocGia.IsDeleted,
                                    NgayLapThe = checkDocGia.NgayLapThe,
                                    NgaySinh = checkDocGia.NgaySinh,
                                    PHIEUMUONs = checkDocGia.PHIEUMUONs,
                                    ACCOUNTs = checkDocGia.ACCOUNTs
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
            var data = _context.SACHes.ToList();
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
                        var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaSach == dataItem.id && !x.IsDeleted.Value).ToList();
                        foreach (var item in checkPhieuMuon)
                        {
                            var checkDocGia = _context.DOCGIAs.Where(x => x.MaDG == item.MaDG).FirstOrDefault();
                            if (checkDocGia != null)
                            {
                                var docgiaItem = new DOCGIA()
                                {
                                    MaDG = checkDocGia.MaDG,
                                    DiaChi = checkDocGia.DiaChi,
                                    Email = checkDocGia.Email,
                                    LoaiDG = checkDocGia.LoaiDG,
                                    IsDeleted = checkDocGia.IsDeleted,
                                    NgayLapThe = checkDocGia.NgayLapThe,
                                    NgaySinh = checkDocGia.NgaySinh,
                                    PHIEUMUONs = checkDocGia.PHIEUMUONs,
                                    ACCOUNTs = checkDocGia.ACCOUNTs
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
