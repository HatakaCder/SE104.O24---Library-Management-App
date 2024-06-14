using QuanLyThuVien.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuanLyThuVien.Commands;
using System.Windows.Markup;

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
            CurrentReader = new READER();
            selectedReaders = new ObservableCollection<READER>();
            searchCommand = new RelayCommand(Search);
            saveCommand = new RelayCommand(Save);
            updateCommand = new RelayCommand(Update);
            deleteCommand = new RelayCommand(Delete);
        }

        #region RelayCommand
        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                saveCommand = value;
                OnPropertyChanged(nameof(SaveCommand));
            }
        }

        private RelayCommand deleteCommand;
        public RelayCommand DeleteCommand
        {
            get { return deleteCommand; }
            set { deleteCommand = value; OnPropertyChanged(nameof(DeleteCommand)); }
        }

        private RelayCommand searchCommand;
        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
            set { searchCommand = value; OnPropertyChanged(nameof(SearchCommand)); }
        }

        private RelayCommand updateCommand;
        public RelayCommand UpdateCommand
        {
            get { return updateCommand; }
            set { updateCommand = value; OnPropertyChanged(nameof(UpdateCommand)); }
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
            set { currentReader = value; OnPropertyChanged(nameof(CurrentReader)); }
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

        private void LoadData()
        {
            ReaderList = new ObservableCollection<READER>(ObjDataOperation.getAllReader());
        }

        public void Save() 
        {
            bool isSaved = ObjDataOperation.Add(currentReader);
            if (isSaved)
                LoadData();
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
            if (selectedReaders.Count == 0) return;

            try
            {
                currentReader = selectedReaders[selectedReaders.Count - 1];
                var isUpdated = ObjDataOperation.update(currentReader);

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
                var isDeleted = ObjDataOperation.Delete_Reader(new List<READER>(selectedReaders));
                if (isDeleted)
                    LoadData();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
