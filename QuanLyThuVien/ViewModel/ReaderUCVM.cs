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
using System.Runtime.Remoting.Proxies;

namespace QuanLyThuVien.ViewModel
{
    public class ReaderUCVM : INotifyPropertyChanged
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
        private string selectedOption = "";
        public ReaderUCVM()
        {
            ObjDataOperation = new DataOperation();
            LoadData();
            NamChecked = true;
            NuChecked = false;
            currentReader = new READER();
            selectedReader = new READER();
            searchCommand = new RelayCommand(Search);
            saveCommand = new RelayCommand(Save);
            savePopUpCommand = new RelayCommand(SavePopUp);
            updateCommand = new RelayCommand(Update);
            updatePopUpCommand = new RelayCommand(UpdatePopUp);
            deleteCommand = new RelayCommand(Delete);
        }

        #region RelayCommand
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
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

        #region Variables for data binding
        private ObservableCollection<READER> readerList;
        public ObservableCollection<READER> ReaderList
        {
            get { return readerList; }
            set { readerList = value; OnPropertyChanged("ReaderList"); }
        }

        private READER currentReader;
        public READER CurrentReader
        {
            get { return currentReader; }
            set
            {
                currentReader = value;
                OnPropertyChanged(nameof(CurrentReader));
            }
        }

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

        private READER selectedReader;
        public READER SelectedReader
        {
            get { return selectedReader; }
            set { selectedReader = value; OnPropertyChanged(nameof(SelectedReader)); }
        }

        private AddReader addReaderV;
        public AddReader AddReaderV
        {
            get { return addReaderV; }
            set { addReaderV = value; OnPropertyChanged(nameof(AddReaderV)); }
        }

        private UpdateReader updateReaderV;
        public UpdateReader UpdateReaderV
        {
            get { return updateReaderV; }
            set { updateReaderV = value; OnPropertyChanged(nameof(UpdateReaderV)); }
        }

        private bool namChecked;

        public bool NamChecked
        {
            get { return namChecked; }
            set
            {
                namChecked = value;
                if (value)
                {
                    selectedOption = "Nam";
                }
                OnPropertyChanged("NamChecked");
            }
        }
        private bool nuChecked;

        public bool NuChecked
        {
            get { return nuChecked; }
            set
            {
                nuChecked = value;
                if (value)
                {
                    selectedOption = "Nữ";
                }
                OnPropertyChanged("NuChecked");
            }
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

        private void LoadData()
        {
            ReaderList = new ObservableCollection<READER>(ObjDataOperation.getAllReader());
        }

        public void Save()
        {
            if (selectedOption == "Nam") CurrentReader.GioiTinh = "Nam";
            else CurrentReader.GioiTinh = "Nữ";
            bool isSaved = ObjDataOperation.Add_Reader(CurrentReader);
            if (isSaved)
            {
                if (System.Windows.MessageBox.Show("Thêm độc giả thành công") == MessageBoxResult.OK)
                    AddReaderV.Close();

                LoadData();
            }
        }

        public void SavePopUp()
        {
            CurrentReader = new READER();
            addReaderV = new AddReader(this);
            addReaderV.ShowDialog();
        }

        public void Search()
        {
            string Item_to_Search = SelectedItem.Split(':')[1];
            Item_to_Search = Item_to_Search.Substring(1, Item_to_Search.Length - 1);

            if (Item_to_Search == "Tất cả")
            {
                LoadData();
            }
            else if (Item_to_Search == "Họ tên")
            {
                ReaderList = new ObservableCollection<READER>(ObjDataOperation.search_reader_by_Name(Items));
            }
            else
            {
                ReaderList = new ObservableCollection<READER>(ObjDataOperation.search_reader_by_NgayLapThe(Items));
            }
        }

        public void Update()
        {
            try
            {
                var isUpdated = ObjDataOperation.update(CurrentReader);

                if (isUpdated)
                {
                    if (System.Windows.MessageBox.Show("Chỉnh sửa thông tin độc giả thành công!") == MessageBoxResult.OK)
                        UpdateReaderV.Close();
                    SelectedReader = new READER();
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
            if (SelectedReader == null || SelectedReader.SoDT == null)
            {
                System.Windows.MessageBox.Show("Bạn vẫn chưa chọn độc giả, hãy chọn độc giả mà bạn muốn chỉnh sửa thông tin!");
                return;
            }

            CurrentReader.MaDG = SelectedReader.MaDG;
            CurrentReader.HoTen = SelectedReader.HoTen;
            CurrentReader.NgaySinh = SelectedReader.NgaySinh;
            CurrentReader.DiaChi = SelectedReader.DiaChi;
            CurrentReader.SoDT = SelectedReader.SoDT;
            CurrentReader.Email = SelectedReader.Email;
            CurrentReader.GioiTinh = SelectedReader.GioiTinh;
            CurrentReader.NgayLapThe = SelectedReader.NgayLapThe;
            selectedOption = SelectedReader.GioiTinh;

            if (selectedOption == "Nam")
            {
                NamChecked = true;
                NuChecked = false;
            }
            else
            {
                NamChecked = false;
                NuChecked = true;
            }

            UpdateReaderV = new UpdateReader(this);
            UpdateReaderV.ShowDialog();
        }

        public void Delete()
        {
            try
            {
                var SelectedReaders = ObjDataOperation.getAllSelectedReaders();

                if (SelectedReaders.Count == 0)
                {
                    System.Windows.MessageBox.Show("Hãy chọn ít nhất một độc giả mà bạn muốn xóa.");
                    return;
                }

                MessageBoxResult confirm = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa những độc giả này?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Cancel) return;

                var isDeleted = ObjDataOperation.Delete_Reader(SelectedReaders);
                if (isDeleted)
                    LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private System.Collections.IEnumerable dOCGIAs;

        public System.Collections.IEnumerable DOCGIAs { get => dOCGIAs; set => SetProperty(ref dOCGIAs, value); }
    }
}