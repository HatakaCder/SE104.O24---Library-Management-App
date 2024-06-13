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
using System.Xml.Serialization; // Để sử dụng EPPlus
using System.IO;
using OfficeOpenXml;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for AddOrEditMuonTra.xaml
    /// </summary>
    public partial class AddOrEditMuonTra : Window
    {
        private readonly QLTV_BETAEntities _context = new QLTV_BETAEntities();
        public AddOrEditMuonTra()
        {
            InitializeComponent();
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
        }


        private string sach_id = null;
        private string docgia_id = null;
        private string IsDelete_check = null;
        DateTime currenDate = DateTime.Now;
        private Random random = new Random();
        private string key = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuioasdfghjklzxcvbnm1234567890";
        private string phieumuon_id = null;
        private bool checkUpdateOrAdd;
        private string PhieumuonId_PhieuTra = null;
        private string maphieutra_Id = null;
        private string IsDelete_PhieuTra = null;
        private int number = 0;
        private int count = 0;
        string quahan = "";
        private string sotien = "";
        long sum = 0;
        private string thehethan = "";
        private string soquyendcmuon = "";


        public AddOrEditMuonTra(bool check)
        {
            InitializeComponent();
            checkUpdateOrAdd = check;
            sotien = Application.Current.Properties["Data"] as string;
            quahan = Application.Current.Properties["ngaymuon"] as string;
            thehethan = Application.Current.Properties["hethanthe"] as string;
            soquyendcmuon = Application.Current.Properties["soquyen"] as string;
        }

        public AddOrEditMuonTra(string quahanInt)
        {
            InitializeComponent();
            this.quahan = quahanInt;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();

        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            this.Close();
        }


        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            loadDataDocgia();
            loadDataSach();
            loadPhieuTra();
            loadPhieuMuon();

            sotien = Application.Current.Properties["Data"] as string;
            quahan = Application.Current.Properties["ngaymuon"] as string;
            thehethan = Application.Current.Properties["hethanthe"] as string;
            soquyendcmuon = Application.Current.Properties["soquyen"] as string;

            if (checkUpdateOrAdd == false)
            {
                maphieutra.Visibility = Visibility.Collapsed; // Ẩn ComboBox đi
                maphieumuon.Visibility = Visibility.Collapsed; // Ẩn ComboBox đi
                titleBlock.Visibility = Visibility.Collapsed;
            }
            else if (checkUpdateOrAdd == true)
            {
                maphieutra.Visibility = Visibility.Visible; // Hiển thị lại ComboBox
                maphieumuon.Visibility = Visibility.Visible; // Hiển thị lại ComboBox 
                titleBlock.Visibility = Visibility.Visible;
            }

        }

        private void loadDataDocgia()
        {
            List<ComboBoxItem> data = new List<ComboBoxItem>();

            var dataDocGia = _context.DOCGIAs.ToList();
            for (int i = 0; i < dataDocGia.Count; i++)
            {
                var item = dataDocGia[i];

                var combobocItem = new ComboBoxItem
                {
                    Content = item.Email,
                    Tag = item.MaDG
                };
                data.Add(combobocItem);
            }


            docgia.ItemsSource = data;
        }

        private void loadPhieuTra()
        {
            List<ComboBoxItem> list = new List<ComboBoxItem>();

            var data = _context.PHIEUTRAs.Where(x => !x.IsDeleted.Value).ToList();
            if (data.Count > 0 || data.Any())
            {
                foreach (var item in data)
                {
                    var comboboxItem = new ComboBoxItem
                    {
                        Content = item.MaPhTra,
                        Tag = item.MaPhTra
                    };

                    list.Add(comboboxItem);
                }
            }


            maphieutra.ItemsSource = list;
        }

        private void loadPhieuMuon()
        {
            List<ComboBoxItem> list = new List<ComboBoxItem>();

            var data = _context.PHIEUMUONs.Where(x => !x.IsDeleted.Value).ToList();
            if (data.Count > 0 || data.Any())
            {
                foreach (var item in data)
                {
                    if (checkUpdateOrAdd == false)
                    {
                        var checkPhieuTra = _context.PHIEUTRAs.Where(x => x.MaPhMuon == item.MaPhMuon && !x.IsDeleted.Value).ToList();
                        if (!checkPhieuTra.Any())
                        {
                            var comboboxItem = new ComboBoxItem
                            {
                                Content = item.MaPhMuon,
                                Tag = item.MaPhMuon
                            };

                            list.Add(comboboxItem);
                        }
                    }
                    else if (checkUpdateOrAdd == true)
                    {
                        var comboboxItem = new ComboBoxItem
                        {
                            Content = item.MaPhMuon,
                            Tag = item.MaPhMuon
                        };

                        list.Add(comboboxItem);

                    }


                }
            }

            maphieumuon1.ItemsSource = list;
            maphieumuon.ItemsSource = list;
        }



        private void loadDataSach()
        {
            List<ComboBoxItem> data = new List<ComboBoxItem>();

            data = _context.SACHes.Select(x => new ComboBoxItem { Content = x.TenSach, Tag = x.MaSach }).ToList();

            sach.ItemsSource = data;
        }

        private bool checkDate()
        {
            var data = _context.PHIEUMUONs.Where(x => x.MaDG == docgia_id).ToList();
            for (var i = 0; i < data.Count; i++)
            {
                var item = data[i];
                if (item.NgayPhTra < currenDate)
                {
                    MessageBox.Show("Đã có sách quá hạn mượn cần phải trả lại để mượn tiếp");
                    return false;
                }

            }
            return true;
        }

        private bool checkNgayMuon()
        {
            var ngaymuonSach = ngaymuon.SelectedDate ?? DateTime.Now;
            TimeSpan songay = currenDate.Subtract(ngaymuonSach); // Sử dụng "Subtract" để tính toán số ngày chênh lệch, "TimeSpan" này sẽ chứa sự chênh lệch giữa "currenDate" và "ngaymuonSach"
            int songayInt = Math.Abs(songay.Days); // "songay.Days" trả về số ngày chênh lệch, và được hàm "Math.Abs()" này chuyển đổi thành số dương để đảm bảo tính toán chính xác

            string songays = Application.Current.Properties["ngaymuon"] as string;
            if (songayInt > int.Parse(songays))
            {
                return false;
            }
            return true;
        }

        private void SaveExcel()
        {
            // Tạo một tệp mới Excel
            FileInfo newFile = new FileInfo(@"D:\GITQLTV\SE104.O24---Library-Management-App\test.xlsx");
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var phieuthu = _context.PHIEUTHUs.Sum(x => x.SoTienThu);

                // Kiểm tra xem bảng tính đã tồn tại chưa
                bool worksheetExists = WorksheetExists(package, "test");

                if (!worksheetExists)
                {
                    ExcelWorksheet excel = package.Workbook.Worksheets.Add("test");

                    // Lưu trữ dữ liệu vào các ô trong bảng tính
                    // Cách 1
                    excel.Cells[1, 1].Value = "Tổng";
                    excel.Cells[1, 2].Value = phieuthu;

                    // Lưu trữ dữ liệu vào các ô trong bảng tính
                    // Cách 2
                    //excel.Cells["A1"].Value = "Tổng";
                    //excel.Cells["B1"].Value = phieuthu;
                }
                else
                {
                    // Truy cập vào bảng tính đã tồn tại
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["test"];

                    // Xác định dòng tiếp theo để thêm dữ liệu
                    int nextRow = worksheet.Dimension.End.Row + 1;

                    // Thêm dữ liệu mới vào bảng tính
                    worksheet.Cells[nextRow, 1].Value = "Data";
                    worksheet.Cells[nextRow, 2].Value = phieuthu;
                }


                // Lưu tệp Excel
                package.Save();

                MessageBox.Show("Xuất thành công");
            }
        }

        // Phương thức để kiểm tra xem một bảng tính có tồn tại trong một gói ExcelPackage hay không
        private bool WorksheetExists(ExcelPackage package, string worksheetName)
        {
            return package.Workbook.Worksheets.Any(sheet => sheet.Name == worksheetName);
        }

        private bool checkTheDocGia()
        {
            var data = _context.DOCGIAs.Where(x => x.MaDG == docgia_id).FirstOrDefault();
            TimeSpan ngayChenhLech = currenDate.Subtract(data.NgayLapThe.Value);
            //int songayChenchLechInt = Math.Abs(ngayChenhLech.Days);

            // Lấy ra số tháng cách biệt giữa thời gian hiện tại với thời gian người dùng chọn được lưu trong database
            int songayChenchLechInt = ((currenDate.Year - data.NgayLapThe.Value.Year) * 12) + currenDate.Month - data.NgayLapThe.Value.Month; // Lấy ra tháng cách biết gữa thời gian hiện tại và thời gian khách hàng chọn đc lưu trong Database

            if (songayChenchLechInt > int.Parse(thehethan))
            {
                MessageBox.Show("Thẻ đã quá hạn");
                return false;
            }
            return true;
        }

        private bool checkSelect()
        {
            if (docgia_id == null)
            {
                MessageBox.Show("Bạn chưa chọn tác giả");
                return false;
            }
            if (sach_id == null)
            {
                MessageBox.Show("Bạn chưa chọn sach");
                return false;
            }
            return true;
        }

        // Hàm Random
        private string generateId(int length, string chuoi)
        {
            StringBuilder sbd = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sbd.Append(chuoi[random.Next(chuoi.Length)]);
            }

            return sbd.ToString();
        }

        private void xulydocgiachange(object sender, SelectionChangedEventArgs e)
        {
            if (docgia.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)docgia.SelectedItem;
                docgia_id = selectedItem.Tag.ToString();
                return;
            }
            MessageBox.Show("Bạn chưa chọn tác giả");
            return;
        }

        private void xulysachchange(object sender, SelectionChangedEventArgs e)
        {
            if (sach.SelectedItem != null)
            {
                ComboBoxItem selectedItem = (ComboBoxItem)sach.SelectedItem;
                sach_id = selectedItem.Tag.ToString();
                return;
            }

            MessageBox.Show("Bạn chưa chọn sách");
            return;

        }

        private void xulychange(object sender, SelectionChangedEventArgs e)
        {
            if (IsDelete.SelectedItem != null) // Kiểm tra nếu người dùng click vào ComBoBoxItem
            {
                ComboBoxItem selectedItem = (ComboBoxItem)IsDelete.SelectedItem;
                IsDelete_check = selectedItem.Content.ToString();

            }
        }

        private bool checkComboBoxItem()
        {
            if (checkUpdateOrAdd == false)
            {
                if (IsDelete.SelectedItem == null || sach.SelectedItem == null || docgia.SelectedItem == null) // Kiểm tra nếu người dùng chưa click vào ComBoBoxItem
                {
                    MessageBox.Show("Bạn chưa chọn thể loại");
                    return false;
                }
            }
            else if (checkUpdateOrAdd == true)
            {
                if (IsDelete.SelectedItem == null || sach.SelectedItem == null || docgia.SelectedItem == null || maphieumuon.SelectedItem == null) // Kiểm tra nếu người dùng chưa click vào ComBoBoxItem
                {
                    MessageBox.Show("Bạn chưa chọn thể loại");
                    return false;
                }
            }

            return true;
        }

        private bool checkNgayMuonNgayTra()
        {
            DateTime ngaymuonsSach = ngaymuon.SelectedDate ?? DateTime.Now;
            DateTime ngaytraSach = ngaytra.SelectedDate ?? DateTime.Now;

            if (ngaytraSach < currenDate || ngaymuonsSach < currenDate || ngaytraSach < ngaymuonsSach)
            {
                MessageBox.Show("Ngày không hợp lệ");
                return false;
            }

            return true;
        }

        private bool checkSachDangMuon()
        {

            var checkDocGia = _context.DOCGIAs.Where(x => x.MaDG == docgia_id).FirstOrDefault();
            if (checkDocGia == null)
            {
                MessageBox.Show("Bạn chưa chọn Đọc giả");
                return false;
            }

            var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaDG == checkDocGia.MaDG && !x.IsDeleted.Value).ToList();
            var checkCout = _context.PHIEUMUONs.Where(x => x.MaDG == checkDocGia.MaDG && !x.IsDeleted.Value).Count();

            if (checkCout > int.Parse(soquyendcmuon))
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < checkPhieuMuon.Count; i++)
                {
                    var data = checkPhieuMuon[i];

                    var checkPhieuTra = _context.PHIEUTRAs.Where(x => x.MaPhMuon == data.MaPhMuon).FirstOrDefault();
                    if (checkPhieuTra == null)
                    {
                        var checkSach = _context.SACHes.Where(x => x.MaSach == data.MaSach).FirstOrDefault();
                        if (checkSach == null)
                        {
                            return false;
                        }

                        count++;
                        if (count == checkPhieuMuon.Count - 1)
                        {
                            str.AppendLine($"Cuốn sách {checkSach.TenSach} được mượn vào ngày {data.NgayMuon}");
                        }
                        else
                        {
                            str.AppendLine($"Cuốn sách {checkSach.TenSach} được mượn vào ngày {data.NgayMuon} và");
                        }

                        number++;

                    }
                }


                if (number > 2)
                {
                    str.AppendLine("Bạn đã mượn quá giới hạn được mượn yêu cầu trả sách lại một số cuốn để được mượn tiếp");
                    MessageBox.Show($"Đã có {number} cuốn sách đang mượn là những cuốn: {str.ToString()}");
                    return false;
                }
            }




            return true;
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

           // if (checkSachDangMuon() && checkSelect()  && checkNgayMuonNgayTra() && checkNgayMuon() && checkTheDocGia())
            {
                if (checkUpdateOrAdd == false)
                {
                    var data = new PHIEUMUON()
                    {
                        MaPhMuon = generateId(5, key),
                        MaDG = docgia_id,
                        MaSach = sach_id,
                        NgayMuon = ngaymuon.SelectedDate,
                        NgayPhTra = ngaytra.SelectedDate,
                        IsDeleted = IsDelete_check == "True" ? true : false

                    };

                    _context.PHIEUMUONs.Add(data);
                    if (_context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add thành công");
                        return;
                    }

                    MessageBox.Show("Add Faild");
                }
                else if (checkUpdateOrAdd != false)
                {
                    var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaPhMuon == phieumuon_id).FirstOrDefault();
                    if (checkPhieuMuon != null)
                    {
                        checkPhieuMuon.MaDG = docgia_id;
                        checkPhieuMuon.MaSach = sach_id;
                        checkPhieuMuon.NgayMuon = ngaymuon.SelectedDate;
                        checkPhieuMuon.NgayPhTra = ngaytra.SelectedDate;
                        checkPhieuMuon.IsDeleted = IsDelete_check == "True" ? true : false;
                    }

                    _context.PHIEUMUONs.AddOrUpdate(checkPhieuMuon);
                    if (_context.SaveChanges() < 0)
                    {
                        MessageBox.Show("Edit Faild");
                        return;
                    }

                    MessageBox.Show("Edit Thành Công");
                }

            }
        }

        private void xulyphieumuonchange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem checkCombobox = (ComboBoxItem)maphieumuon.SelectedItem;
            phieumuon_id = checkCombobox.Tag.ToString();

        }

        private void xulymaphieumuon_Idchange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)maphieumuon1.SelectedItem;
            PhieumuonId_PhieuTra = comboBoxItem.Tag.ToString();
        }

        private void xulyphieutrachange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)maphieutra.SelectedItem;
            maphieutra_Id = comboBoxItem.Tag.ToString();
        }

        private bool checkNgayQuaHam()
        {
            long sum = 0;
            if (PhieumuonId_PhieuTra == null)
            {
                MessageBox.Show("Chọn chưa chọn phiếu mượn");
                return false;
            }

            var checkPhieu = _context.PHIEUMUONs.Where(x => x.MaPhMuon == PhieumuonId_PhieuTra).FirstOrDefault();
            if (checkPhieu == null)
            {
                MessageBox.Show("Phiếu mượn không tồn tại");
                return false;
            }

            TimeSpan timspan = currenDate.Subtract(checkPhieu.NgayPhTra.Value);
            int songay = Math.Abs(timspan.Days);

            if (currenDate > checkPhieu.NgayPhTra)
            {
                // string quahans = Application.Current.Properties["songay"] as string; // Lấy ra dữ liệu lưu trong "Application.Current.Properties" còn "["Data"]" này là tên đại diện cho dữ liệu được lưu
                if (songay > int.Parse(quahan))
                {
                    sum = int.Parse(sotien) * songay;
                    MessageBoxResult resul = MessageBox.Show($"Bạn đã quá hạn số ngày là {songay} ngày, bạn mượn từ ngày {checkPhieu.NgayMuon} đến ngày {checkPhieu.NgayPhTra}, số tiền phạt là {sum}VNĐ", "Xác nhận", MessageBoxButton.OKCancel);
                    if (resul == MessageBoxResult.OK) // Xử lý khi người dùng ấn vào nút "OK"
                    {
                        var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaPhMuon == PhieumuonId_PhieuTra && !x.IsDeleted.Value).FirstOrDefault();
                        if (checkPhieuMuon == null)
                        {
                            MessageBox.Show("Phiếu mượn không tồn tại");
                            return false;
                        }

                        var data = new PHIEUTRA()
                        {
                            MaPhTra = generateId(5, key),
                            MaPhMuon = PhieumuonId_PhieuTra,
                            NgayTra = ngaytra1.SelectedDate ?? DateTime.Now,
                            IsDeleted = IsDelete_PhieuTra == "True" ? true : false,
                        };

                        checkPhieuMuon.IsDeleted = true;

                        _context.PHIEUTRAs.Add(data);
                        _context.PHIEUMUONs.AddOrUpdate(checkPhieuMuon);

                        if (_context.SaveChanges() > 0)
                        {
                            var checkPhieutra = _context.PHIEUTRAs.Where(x => x.MaPhTra == data.MaPhTra && !x.IsDeleted.Value).FirstOrDefault();
                            if (checkPhieutra == null)
                            {
                                MessageBox.Show("Phiếu trả không hợp lệ");
                                return false;
                            }

                            var dataPhieuThu = new PHIEUTHU
                            {
                                MaPhTra = checkPhieutra.MaPhTra,
                                SoNgayQHan = (short)songay,
                                SoTienThu = (int)sum

                            };

                            _context.PHIEUTHUs.Add(dataPhieuThu);


                            if (_context.SaveChanges() > 0)
                            {
                                MessageBox.Show("Thanh toán thành công");
                                return false;
                            }



                        }

                    }
                    return false;
                }

            }

            return true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            var checkPhieuMuon = _context.PHIEUMUONs.Where(x => x.MaPhMuon == PhieumuonId_PhieuTra).FirstOrDefault();
            if (checkUpdateOrAdd == false) // Add
            {
                if (checkNgayQuaHam() && checkValiDatePhieuTra())
                {

                    var data = new PHIEUTRA()
                    {
                        MaPhTra = generateId(5, key),
                        MaPhMuon = PhieumuonId_PhieuTra,
                        NgayTra = ngaytra1.SelectedDate ?? DateTime.Now,
                        IsDeleted = IsDelete_PhieuTra == "True" ? true : false,
                    };


                    if (checkPhieuMuon != null)
                    {
                        checkPhieuMuon.IsDeleted = true;
                        _context.PHIEUMUONs.AddOrUpdate(checkPhieuMuon);

                    }

                    _context.PHIEUTRAs.Add(data);
                    if (_context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add phiếu trả thành công");
                        return;
                    }

                    MessageBox.Show("Add phiếu trả Faild");
                    return;
                }
            }
            else if (checkUpdateOrAdd == true) // Upadte
            {
                if (checkValiDatePhieuTra())
                {
                    var checkUpdatePhieuTra = _context.PHIEUTRAs.Where(x => x.MaPhTra == maphieutra_Id).FirstOrDefault();
                    if (checkUpdatePhieuTra != null)
                    {
                        checkUpdatePhieuTra.IsDeleted = IsDelete_PhieuTra == "True" ? true : false;
                        checkUpdatePhieuTra.MaPhMuon = PhieumuonId_PhieuTra;
                        checkUpdatePhieuTra.NgayTra = ngaytra1.SelectedDate ?? DateTime.Now;

                        if (checkPhieuMuon != null)
                        {
                            checkPhieuMuon.IsDeleted = true;
                            _context.PHIEUMUONs.AddOrUpdate(checkPhieuMuon);

                        }

                        _context.PHIEUTRAs.AddOrUpdate(checkUpdatePhieuTra);

                        if (_context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Edit thành công");
                            return;
                        }

                        MessageBox.Show("Edit Faild");

                    }
                }
            }
        }

        private bool checkValiDatePhieuTra()
        {
            if (checkUpdateOrAdd == false)
            {
                if (maphieumuon1.SelectedItem == null || IsDelete1.SelectedItem == null)
                {
                    MessageBox.Show("Bạn chưa chọn phiếu mượn hoặc chưa chọn IsDelete");
                    return false;
                }
            }
            if (checkUpdateOrAdd == true)
            {
                if (maphieumuon1.SelectedItem == null || IsDelete1.SelectedItem == null || maphieutra.SelectedItem == null)
                {
                    MessageBox.Show("Bạn chưa chọn phiếu mượn hoặc chưa chọn IsDelete hoặc chưa chọn phiếu trả");
                    return false;
                }
            }


            return true;
        }

        private void xulychange1(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem comboBoxItem = (ComboBoxItem)IsDelete1.SelectedItem;
            IsDelete_PhieuTra = comboBoxItem.Content.ToString();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SaveExcel();
        }
    }
}
