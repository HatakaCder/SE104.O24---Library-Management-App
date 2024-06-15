using QuanLyThuVien.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    class NavigationVM : Utilities.ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand BookCommand { get; set; }
        public ICommand BookBorrowCommand { get; set; }
        public ICommand ReaderCommand { get; set; }
        public ICommand LibraianCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Book(object obj) => CurrentView = new BookVM();
        private void BookBorrow(object obj) => CurrentView = new BookBorrowVM();
        private void Reader(object obj) => CurrentView = new ReaderUCVM();
<<<<<<< HEAD
        private void Libarian(object obj) => CurrentView = new LibarianVM();
=======
        private void Libarian(object obj) => CurrentView = new LibrarianVM();
        private void User(object obj) => CurrentView = new UserVM();
>>>>>>> main
        private void Report(object obj) => CurrentView = new Report();
        private void Setting(object obj) => CurrentView = new SettingVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            BookCommand = new RelayCommand(Book);
            BookBorrowCommand = new RelayCommand(BookBorrow);
            ReaderCommand = new RelayCommand(Reader);
            LibraianCommand = new RelayCommand(Libarian);
            ReportCommand = new RelayCommand(Report);
            SettingsCommand = new RelayCommand(Setting);

            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}
