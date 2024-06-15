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
using System.Runtime.Remoting.Proxies;

namespace QuanLyThuVien.ViewModel
{
    public class ReaderUCVM: INotifyPropertyChanged
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
        public ReaderUCVM()
        {
            ObjDataOperation = new DataOperation();
            LoadData();
            UpdateReader = new READER();
            AddReader = new READER();
            selectedReaders = new ObservableCollection<READER>();
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

        private READER updateReader;
        public READER UpdateReader
        {
            get { return updateReader; }
            set { updateReader = value; OnPropertyChanged(nameof(UpdateReader)); }
        }

        private READER addReader;
        public READER AddReader
        {
            get { return addReader; }
            set { addReader = value; OnPropertyChanged(nameof(AddReader)); }
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

        private ObservableCollection<READER> _selectedReaders;
        public ObservableCollection<READER> selectedReaders
        {
            get { return _selectedReaders; }
            set { _selectedReaders = value; OnPropertyChanged(nameof(selectedReaders)); }
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
            selectedReaders = new ObservableCollection<READER>(ObjDataOperation.getAllSelectedReaders());
        }

        public void Save() 
        {
            bool isSaved = ObjDataOperation.Add_Reader(AddReader);
            if (isSaved)
            {
                System.Windows.MessageBox.Show("Thêm độc giả thành công");
                LoadData();
            }
        }

        public void SavePopUp()
        {
            AddReader addReader = new AddReader(this);
            addReader.ShowDialog();
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
                var isUpdated = ObjDataOperation.update(UpdateReader);

                if (isUpdated)
                {
                    System.Windows.MessageBox.Show("Chỉnh sửa thông tin độc giả thành công!");
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
            UpdateReader updateReader = new UpdateReader(this);
            updateReader.ShowDialog();
        }

        public void Delete()
        {
            try
            {
                selectedReaders = new ObservableCollection<READER>(ObjDataOperation.getAllSelectedReaders());

                if (selectedReaders.Count == 0) return;

                MessageBoxResult confirm = System.Windows.MessageBox.Show("Bạn có chắc chắn muốn xóa những độc giả này?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if (confirm == MessageBoxResult.Cancel) return;

                var isDeleted = ObjDataOperation.Delete_Reader(new List<READER>(selectedReaders));
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
