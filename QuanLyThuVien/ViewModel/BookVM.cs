using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Commands;
using System.CodeDom;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using QuanLyThuVien.View;
using System.Windows;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class BookVM: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        DataOperation ObjDataOperation;
        public BookVM()
        {
            ObjDataOperation = new DataOperation();
            LoadData();
            CurrentBook = new BOOK();
            saveCommand = new RelayCommand(Save);
            searchCommand = new RelayCommand(Search);
            deleteCommand = new RelayCommand(Delete);
            updateCommand = new RelayCommand(Update);
            selectedBooks = new ObservableCollection<BOOK>();
        }

        // Khai báo RelayCommands
        #region RelayCommand

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }

        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
        }

        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
        }
        #endregion

        // Khai báo một số biến cho việc Data Binding
        #region Variables for data binding
        // For loading all books
        private ObservableCollection<BOOK> bookList;
        public ObservableCollection<BOOK> BooksList
        {
            get { return bookList; }
            set { bookList = value; OnPropertyChanged(nameof(BooksList)); }
        }

        // For adding, updating book
        private BOOK currentBook;
        public BOOK CurrentBook
        {
            get { return currentBook; }
            set { currentBook = value; OnPropertyChanged(nameof(CurrentBook)); }
        }

        // For searching books
        private string items;
        public string Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(nameof(Items)); }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        // For deleting books
        private ObservableCollection<BOOK> _books;
        public ObservableCollection<BOOK> selectedBooks
        {
            get { return _books; }
            set { _books = value; OnPropertyChanged(nameof(selectedBooks)); }
        }

        private ObservableCollection<string> _bookTypes;
        public ObservableCollection<string> bookTypes
        {
            get { return _bookTypes; }
            set { _bookTypes = value; OnPropertyChanged(nameof(_bookTypes)); }
        }
        #endregion

        // Get all Book into BooksList
        public void LoadData()
        {
            BooksList = new ObservableCollection<BOOK>(ObjDataOperation.getAllBook());
            bookTypes = new ObservableCollection<string>(ObjDataOperation.getAllBookTypes());
        }

        #region SetProperty

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
        #endregion

        public void Update() 
        {
            if (selectedBooks.Count == 0) return;

            try
            {
                CurrentBook = selectedBooks[selectedBooks.Count - 1];
                var isUpdated = ObjDataOperation.update(CurrentBook);
                
                if (isUpdated)
                    LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete()
        {
            try
            {
                var isSaved = ObjDataOperation.Delete_Book(new List<BOOK>(selectedBooks));
                if (isSaved)
                    LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void Save()
        {
            try
            {
                var IsSaved = ObjDataOperation.Add(CurrentBook);
                if (IsSaved)
                    LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Search()
        {
            string Item_to_Search = SelectedItem.Split(':')[1];
            Item_to_Search = Item_to_Search.Substring(1, Item_to_Search.Length - 1);

            if (Item_to_Search == "Tất cả")
            {
                LoadData();
            }
            else if (Item_to_Search == "Tên sách")
            {
                BooksList = new ObservableCollection<BOOK>( ObjDataOperation.search_book_by_TenSach(Items));
            }
            else if (Item_to_Search == "Thể loại")
            {
                BooksList = new ObservableCollection<BOOK>(ObjDataOperation.search_book_by_TheLoai(Items));
            }
            else
            {
                BooksList = new ObservableCollection<BOOK>(ObjDataOperation.search_book_by_TacGia(Items));
            }
        }

        private System.Collections.IEnumerable sACHes;

        public System.Collections.IEnumerable SACHes { get => sACHes; set => SetProperty(ref sACHes, value); }
    }
}
