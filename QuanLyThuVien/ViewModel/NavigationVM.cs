using QuanLyThuVien.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    // TÓM TẮT NỘI DUNG ĐOẠN CODE NÀY
    /* Đoạn code này đang tạo ra một cơ chế điều hướng (navigation) để cho phép người dùng di chuyển giữa các màn hình 
     * khác nhau trong ứng dụng, mỗi màn hình được đại diện bởi một ViewModel cụ thể.
     * Khai báo đường dẫn ở đây để ghép vào file "DataTemplate.xaml" để ghép đường dẫn này vào các View tương ứng
     */
    class NavigationVM : Utilities.ViewModelBase
    {
        private object _currentView; // Sử dụng để lưu trữ View hiện tại được hiển thị
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        // Các Command này được sử dụng để thực hiện chuyển đổi giũa các View
        public ICommand HomeCommand { get; set; }
        public ICommand BookCommand { get; set; }
        public ICommand BookBorrowCommand { get; set; }
        public ICommand ReaderCommand { get; set; }
        public ICommand LibraianCommand { get; set; }
        public ICommand UserCommand { get; set; }  
        public ICommand ReportCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand AddOrEditCommand { get; set; }


        // Mỗi phương thức sẽ thiết lập CurrentView thành một ViewModel cụ thể tương ứng với mỗi loại view
        private void Home(object obj) => CurrentView = new HomeVM();
        private void Book(object obj) => CurrentView = new BookVM();
        private void BookBorrow(object obj) => CurrentView = new BookBorrowVM();
        private void Reader(object obj) => CurrentView = new ReaderUCVM();
        private void Libarian(object obj) => CurrentView = new LibarianVM();
        private void User(object obj) => CurrentView = new UserVM();
        private void Report(object obj) => CurrentView = new Report();
        private void Setting(object obj) => CurrentView = new SettingVM();
        private void AddOrEdit(object obj) => CurrentView = new AddOrEditVM();


        // khởi tạo các ICommand và đặt CurrentView thành một view khởi đầu (trong trường hợp này là HomeVM).
        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            BookCommand = new RelayCommand(Book);
            BookBorrowCommand = new RelayCommand(BookBorrow);
            ReaderCommand = new RelayCommand(Reader);
            LibraianCommand = new RelayCommand(Libarian);
            UserCommand = new RelayCommand(User);
            ReportCommand = new RelayCommand(Report);
            SettingsCommand = new RelayCommand(Setting);
            AddOrEditCommand = new RelayCommand(AddOrEdit);

            // Startup Page
            CurrentView = new HomeVM(); // Đặt HomeVM thành View khởi đầu
        }
    }
}
