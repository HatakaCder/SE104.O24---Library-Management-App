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
            Message1 = BooksList.Count.ToString();
        }

        private void LoadData()
        {
            BooksList = new ObservableCollection<BOOK>(ObjDataOperation.getAllBook());
        }

        #region DisplayData
        private ObservableCollection<BOOK> bookList;
        public ObservableCollection<BOOK> BooksList
        {
            get { return bookList; }
            set { bookList = value; OnPropertyChanged("ReaderList"); }
        }
        #endregion

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

        private BOOK currentBook;
        public BOOK CurrentBook
        {
            get { return currentBook; }
            set { currentBook = value; OnPropertyChanged("CurrentBook"); }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set {  message = value; OnPropertyChanged("Message"); } 
        }

        
        private RelayCommand deleteCommand;
        private RelayCommand relayCommand;

        #region SaveCommand
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }
        public void Save()
        {
            try
            {
                var IsSaved = ObjDataOperation.Add(CurrentBook);
                LoadData();
                if (IsSaved)
                    Message = "Book saved";
                else
                    Message = "Save operation failed";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region SearchCommand
        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }

        public void Search()
        {
            BOOK book = null;
            try
            {
                var ObjBook = ObjDataOperation.Search_Book(CurrentBook.MaSach);

                if (ObjBook != null)
                {
                    book = new BOOK()
                    {
                        MaSach = ObjBook.MaSach,
                        TenSach = ObjBook.TenSach,
                        TheLoai = ObjBook.TheLoai,
                        TacGia = ObjBook.TacGia,
                        NamXB = (short)ObjBook.NamXB,
                        NhaXB = ObjBook.NhaXB,
                        NgayNhap = (DateTime)ObjBook.NgayNhap,
                        TinhTrang = (short)ObjBook.TinhTrang
                    };
                    Message = "Search sucessfully";
                    

                    BooksList = new ObservableCollection<BOOK>() { book};
                }
                else
                    Message = "Search failed";
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Message1 = BooksList.Count.ToString();
        }
        #endregion

        // Một số biến để kiểm tra lỗi sai
        #region Test_variables
        private string message1;
        public string Message1
        {
            get { return message1; }
            set { message1 = value; OnPropertyChanged("Message1"); }
        }
        #endregion

        private System.Collections.IEnumerable sACHes;

        public System.Collections.IEnumerable SACHes { get => sACHes; set => SetProperty(ref sACHes, value); }
    }
}
