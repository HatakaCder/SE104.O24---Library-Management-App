﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Utilities;
using System.Windows.Input;
using QuanLyThuVien.ViewModel;
using System.Windows;

namespace QuanLyThuVien.View
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
        public ICommand libraianCommand { get; set; }
        public ICommand UserCommand { get; set; }
        public ICommand ReportCommand { get; set; }
        public ICommand SettingsCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Book(object obj) => CurrentView = new BookVM();
        private void BookBorrow(object obj) => CurrentView = new BookBorrowVM();
        private void Reader(object obj) => CurrentView = new ReaderUCVM();
        private void Libarian(object obj) => CurrentView = new LibarianVM();
        private void User(object obj) => CurrentView = new UserVM();
        private void Report(object obj) => CurrentView = new Report();
        private void Setting(object obj) => CurrentView = new SettingVM();

        public NavigationVM()
        {
            HomeCommand = new RelayCommand(Home);
            BookCommand = new RelayCommand(Book);
            BookBorrowCommand = new RelayCommand(BookBorrow);
            ReaderCommand = new RelayCommand(Reader);
            libraianCommand = new RelayCommand(Libarian);
            UserCommand = new RelayCommand(User);
            ReportCommand = new RelayCommand(Report);
            SettingsCommand = new RelayCommand(Setting);

            // Startup Page
            CurrentView = new HomeVM();
        }
    }
}