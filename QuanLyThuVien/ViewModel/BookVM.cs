using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Command;
using System.CodeDom;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using QuanLyThuVien.View;
using System.Windows;
using System.Windows.Input;
using System.Windows.Forms;

namespace QuanLyThuVien.ViewModel
{
    public class BookVM : INotifyPropertyChanged
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
            currentBook = new BOOK();
            selectedBook = new BOOK();
            saveCommand = new RelayCommand(Save);
            savePopUpCommand = new RelayCommand(SavePopUp);
            searchCommand = new RelayCommand(Search);
            deleteCommand = new RelayCommand(Delete);
            updateCommand = new RelayCommand(Update);
            updatePopUpCommand = new RelayCommand(UpdatePopUp);
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

        private RelayCommand updatePopUpCommand;
        public RelayCommand UpdatePopUpCommand
        {
            get { return updatePopUpCommand; }
        }

        private RelayCommand savePopUpCommand;
        public RelayCommand SavePopUpCommand
        {
            get { return savePopUpCommand; }
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
            set { currentBook = value; OnPropertyChanged(nameof(UpdateBook)); }
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
        private BOOK selectedBook;
        public BOOK SelectedBook  {
            get { return selectedBook; }
            set { selectedBook = value; OnPropertyChanged(nameof(selectedBook)); }
        }

        private ObservableCollection<string> _bookTypes;
        public ObservableCollection<string> bookTypes
        {
            get { return _bookTypes; }
            set { _bookTypes = value; OnPropertyChanged(nameof(_bookTypes)); }
        }

        private AddBook addBookV;
        public AddBook AddBookV
        {
            get { return addBookV; }
            set { addBookV = value;  OnPropertyChanged(nameof(AddBookV)); }
        }

        private UpdateBook updateBookV;
        public UpdateBook UpdateBookV
        {
            get { return updateBookV; }
            set { updateBookV = value; OnPropertyChanged(nameof(UpdateBookV)); }
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

        public void Save()
        {
            try
            {
                var IsAdded = ObjDataOperation.Add_Book(CurrentBook);
                if (IsAdded)
                {
                    if (System.Windows.MessageBox.Show("Thêm sách thành công!") == MessageBoxResult.OK)
                    {
                        AddBookV.Close();
                    }
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SavePopUp()
        {

            CurrentBook = new BOOK();
            AddBookV = new AddBook(this);
            AddBookV.ShowDialog();
        }

        public void Update()
        {

            try
            {
                // BOOK book_temp = UpdateBook;
                var isUpdated = ObjDataOperation.update(CurrentBook);

                if (isUpdated)
                {
                    if (System.Windows.MessageBox.Show("Chỉnh sửa sách thành công!") == MessageBoxResult.OK)
                    {
                        UpdateBookV.Close();
                    }
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePopUp()
        {
            if (SelectedBook == null || SelectedBook.NamXB < 1)
            {
                System.Windows.MessageBox.Show("Bạn vẫn chưa chọn sách, vui lòng chọn sách để chỉnh sửa!");
                return;
            }

            CurrentBook.MaSach = SelectedBook.MaSach;
            CurrentBook.TenSach = SelectedBook.TenSach;
            CurrentBook.TheLoai = SelectedBook.TheLoai;
            CurrentBook.TacGia = SelectedBook.TacGia;
            CurrentBook.NamXB = SelectedBook.NamXB;
            CurrentBook.NhaXB = SelectedBook.NhaXB;
            CurrentBook.TriGia = SelectedBook.TriGia;
            CurrentBook.NgayNhap = SelectedBook.NgayNhap;

            UpdateBookV = new UpdateBook(this);
            UpdateBookV.ShowDialog();
        }

        public void Delete()
        {
            try
            {
                var SelectedBooks = ObjDataOperation.getSelectedBooks();

                if (SelectedBooks.Count == 0)
                {
                    
                    System.Windows.MessageBox.Show("Hãy chọn ít nhất một cuốn sách mà bạn muốn xóa.");
                    return;
                    
                }

                MessageBoxResult confirm = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa những cuốn sách này?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Cancel) return;

                var isSaved = ObjDataOperation.Delete_Book(SelectedBooks);
                if (isSaved)
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
                BooksList = new ObservableCollection<BOOK>(ObjDataOperation.search_book_by_TenSach(Items));
            }
            else if (Item_to_Search == "Thể loại")
            {
                BooksList = new ObservableCollection<BOOK>(ObjDataOperation.search_book_by_TheLoai(Items));
            }
            else if (Item_to_Search == "Tác giả")
            {
                BooksList = new ObservableCollection<BOOK>(ObjDataOperation.search_book_by_TacGia(Items));
            }
            else
            {
                BooksList = new ObservableCollection<BOOK>(ObjDataOperation.search_available_book());
            }
        }

        #region Test
        #endregion

        private System.Collections.IEnumerable sACHes;

        public System.Collections.IEnumerable SACHes { get => sACHes; set => SetProperty(ref sACHes, value); }
    }
}