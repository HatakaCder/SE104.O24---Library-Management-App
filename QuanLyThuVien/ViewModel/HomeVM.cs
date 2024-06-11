using MaterialDesignColors;
using QuanLyThuVien.Model;
using QuanLyThuVien.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public HomeVM()
        {
            ObjDataProvider = new DataProvider();
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
        private void LoadData()
        {
            ReadersList = new ObservableCollection<READER>(ObjDataProvider.GetREADERs());
            using(var ObjContext = new QLTV_BETAEntities1())
            {
                IDCount = ObjContext.DOCGIAs.Count();
            }    


        }
        private int _idCount;
        public int IDCount
        {
            get { return _idCount; }
            set
            {
                _idCount = value;
                OnPropertyChanged("IDCount");
            }
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
            {   int MaDG = int.Parse(currentReader.MaDG);
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
