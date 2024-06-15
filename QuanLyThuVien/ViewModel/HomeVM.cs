using MaterialDesignColors;
using QuanLyThuVien.Model;
using QuanLyThuVien.Sevice;
using QuanLyThuVien.Utilities;
using QuanLyThuVien.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace QuanLyThuVien.ViewModel
{
    public class HomeVM : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged_Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        DataProvider ObjDataProvider;

        CountService CountService;

        public HomeVM()
        {
            Console.WriteLine("DataContext is HomeVM");
            ObjDataProvider = new DataProvider();
            CountService = new CountService();
            LoadData();
            CurrentReader = new READER();
            saveCommand = new RelayCommand(Save);
            searchCommand = new RelayCommand(Search);
            updateCommand = new RelayCommand(Update);
            deleteCommand = new RelayCommand(Delete);
            LoadDataCommand = new RelayCommand(LoadData);
        }
        #region DisplayOperation
        private ObservableCollection<READER> readersList;

        public ICommand LoadDataCommand { get; private set; }

        public ObservableCollection<READER> ReadersList
        {
            get { return readersList; }
            set { readersList = value; OnPropertyChanged("ReadersList"); }
        }

        private ObservableCollection<BOOK> booksList;
        public ObservableCollection<BOOK> BooksList
        {
            get { return booksList; }
            set
            {
                booksList = value;
                OnPropertyChanged(nameof(BooksList));
            }
        }
        private void UpdateRecentBooks()
        {
            RecentBooks = new ObservableCollection<BOOK>(BooksList.Where(book => (DateTime.Now - book.NgayNhap).TotalDays <= 7));
            OnPropertyChanged(nameof(RecentBooks));
        }
        public ObservableCollection<BOOK> RecentBooks { get; set; }
        private ObservableCollection<PHIEUMUON> pHIEUMUONs;
        public ObservableCollection<PHIEUMUON> PHIEUMUONs
        {
            get { return pHIEUMUONs; }
            set {  pHIEUMUONs = value; OnPropertyChanged("PHIEUMUONs"); }
        }
        private ObservableCollection<PhieuTraDTO> pHIEUTHUS;
        public ObservableCollection<PhieuTraDTO> PHIEUTHUS
        {
            get { return pHIEUTHUS; }
            set { pHIEUTHUS = value; OnPropertyChanged("PHIEUTHUS"); }
        }

        #endregion
        private ObservableCollection<ListQuaHanModel> listQuaHan;

        public ObservableCollection<ListQuaHanModel> ListQuaHan
        {
            get { return listQuaHan; }
            set { listQuaHan = value; OnPropertyChanged("ListQuaHan"); }
        }
        private void LoadData()
        {
            BooksList = new ObservableCollection<BOOK>(ObjDataProvider.getAllBook());
            ReadersList = new ObservableCollection<READER>(ObjDataProvider.GetREADERs());
            pHIEUMUONs = new ObservableCollection<PHIEUMUON>(ObjDataProvider.GetPHIEUMUONs());
            pHIEUTHUS = new ObservableCollection<PhieuTraDTO>(ObjDataProvider.GetPHIEUTHUs());
            listQuaHan = new ObservableCollection<ListQuaHanModel>(ObjDataProvider.GetListQuaHan());
            Count = CountService.getCount();
            bCount = CountService.getCountBook();
            countBorrowed = CountService.getCountBookBorrowed();
            countOutDate = CountService.getOutDateBook();
            UpdateRecentBooks();

        }

        #region Count
        private int count;
        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged("Count"); }

        }
        private int countBorrowed;

        public int CountBorrowed
        {
            get { return countBorrowed; }
            set { countBorrowed = value; OnPropertyChanged("CountBorrowed"); }
        }

        private int bcount;
        public int bCount
        {
            get { return bcount; }
            set { bcount = value; OnPropertyChanged("bCount"); }

        }

        private int countOutDate;
        public int CountOutDate
        {
            get { return countOutDate; }
            set { countOutDate = value; OnPropertyChanged("CountOutDate"); }

        }


        private int getOverdueBooksCount;
        public int GetOverdueBooksCount
        {
            get { return getOverdueBooksCount; }
            set { getOverdueBooksCount = value; OnPropertyChanged("GetOverdueBooksCount"); }

        }
        #endregion

        private READER currentReader;

        public READER CurrentReader
        {
            get { return currentReader; }
            set { currentReader = value; OnPropertyChanged("CurrentReader"); }
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }


        #region SaveOperation
        private RelayCommand saveCommand;

        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }
        public void Save()
        {
            try
            {
                var IsSaved = ObjDataProvider.Add(CurrentReader);
                LoadData();
                if (IsSaved)
                {
                    Message = "Employee saved";
                }
                else
                {
                    Message = "Save operation failed";
                }
            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
        }
        #endregion

        #region SearchOperation
        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }
        public void Search()
        {
            try
            {
                int MaDG = int.Parse(currentReader.MaDG);
                var ObjReader = ObjDataProvider.Search(MaDG);
                if (ObjReader != null)
                {
                    CurrentReader.HoTen = ObjReader.HoTen;
                    CurrentReader.LoaiDG = ObjReader.LoaiDG;
                    CurrentReader.GioiTinh = ObjReader.GioiTinh;
                    CurrentReader.NgaySinh = ObjReader.NgaySinh;
                    CurrentReader.DiaChi = ObjReader.DiaChi;
                    CurrentReader.Email = ObjReader.Email;
                    CurrentReader.SoDT = ObjReader.SoDT;
                    CurrentReader.NgayLapThe = ObjReader.NgayLapThe;
                    CurrentReader.SoCCCD = ObjReader.SoCCCD;
                    CurrentReader.AnhDaiDien = ObjReader.AnhDaiDien;
                }
                else
                {
                    Message = "Reader Not found";
                }
            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
        }
        #endregion

        #region UpdateOperation
        private RelayCommand updateCommand;

        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
        }
        public void Update()
        {
            try
            {
                var IsUpdated = ObjDataProvider.Update(CurrentReader);
                if (IsUpdated)
                {
                    Message = "Reader Updated";
                    LoadData();
                }
                else
                {
                    Message = "Update Operation Failed";
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }
        #endregion

        #region DeleteOperation
        private RelayCommand deleteCommand;

        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
        }

        public void Delete()
        {
            try
            {
                int MaDG = int.Parse(currentReader.MaDG);
                var IsDeleted = ObjDataProvider.Delete(MaDG);
                if (IsDeleted)
                {
                    Message = "Reader deleted";
                    LoadData();
                }
                else
                {
                    Message = "Delete operation failed";
                }
            }
            catch (Exception ex)
            {

                Message = ex.Message;
            }
        }
        #endregion
    }
}