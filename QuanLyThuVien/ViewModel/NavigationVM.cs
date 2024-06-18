using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Utilities;
using System.Windows.Input;
using QuanLyThuVien.ViewModel;
using System.Windows;
using System.ComponentModel;

namespace QuanLyThuVien.ViewModel
{
    class NavigationVM : Utilities.ViewModelBase
    {
        private object _currentView;
        private bool isCrudEnabled = true;
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
        public ICommand UserCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Book(object obj) => CurrentView = new BookVM(isCrudEnabled);
        private void BookBorrow(object obj) => CurrentView = new BookBorrowVM();
        private void Reader(object obj) => CurrentView = new ReaderUCVM();
        private void Libarian(object obj) => CurrentView = new LibrarianVM();
        private void User(object obj) => CurrentView = new UserVM();
        private void Report(object obj) => CurrentView = new Report();
        private void Setting(object obj) => CurrentView = new SettingVM();

        public NavigationVM(int vaitro)
        {

            if (vaitro == 2)
            {
                isCrudEnabled = false;
            }
            else
            {
                if (vaitro == 0)
                {
                    LibraianCommand = new RelayCommand(Libarian);
                }
                HomeCommand = new RelayCommand(Home);
                BookBorrowCommand = new RelayCommand(BookBorrow);
                ReaderCommand = new RelayCommand(Reader);
                UserCommand = new RelayCommand(User);
                ReportCommand = new RelayCommand(Report);
                SettingsCommand = new RelayCommand(Setting);
            }
            BookCommand = new RelayCommand(Book);

            // Startup Page
            if (vaitro == 2)
            {
                CurrentView = new BookVM(false);
            }
            else
            {
                CurrentView = new HomeVM();
            }
        }
    }
}
