using QLTVDemo.Models;
using QuanLyThuVien.Model;
using QuanLyThuVien.Utilities;
using QuanLyThuVien.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace QuanLyThuVien.ViewModel
{
    public class LibarianVM : INotifyPropertyChanged
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

        public LibarianVM()
        {
            ObjDataProvider = new DataProvider();
            LoadData();
            deleteCommand = new RelayCommand(Delete);
            addCommand = new RelayCommand(AddLibrarian);

        }

        #region DisplayOperation
        private ObservableCollection<LIBRARIAN> librariansList;

        public ICommand LoadDataCommand { get; private set; }

        public ObservableCollection<LIBRARIAN> LibrariansList
        {
            get { return librariansList; }
            set { librariansList = value; OnPropertyChanged("LibrariansList"); }
        }
        private void LoadData()
        {
            LibrariansList = new ObservableCollection<LIBRARIAN>(ObjDataProvider.GetTHUTHUs());


        }
        #endregion
        private LIBRARIAN currentLibrarian;

        public LIBRARIAN CurrentLibrarian
        {
            get { return currentLibrarian; }
            set { currentLibrarian = value; OnPropertyChanged("CurrentLibrarian"); }
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; OnPropertyChanged("Message"); }
        }
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
                int MaTT = int.Parse(currentLibrarian.MaTT);
                var IsDeleted = ObjDataProvider.Delete(MaTT);
                if (IsDeleted)
                {
                    Message = "Lb deleted";
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

        private RelayCommand addCommand;

        public RelayCommand AddCommand
        {
            get { return addCommand; }
        }


        private string _maTT;
        public string MaTT
        {
            get { return _maTT; }
            set { _maTT = value; OnPropertyChanged(nameof(MaTT)); }
        }

        private DateTime _ngaySinh = DateTime.Now;
        public DateTime NgaySinh
        {
            get { return _ngaySinh; }
            set { _ngaySinh = value; OnPropertyChanged(nameof(NgaySinh)); }
        }

        private string _diaChi;
        public string DiaChi
        {
            get { return _diaChi; }
            set { _diaChi = value; OnPropertyChanged(nameof(DiaChi)); }
        }

        private string _email;
        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public void AddLibrarian()
        {
            AddLibrarian addlb = new AddLibrarian();
            addlb.Show();
            var newLibrarian = new LIBRARIAN
            {
                MaTT = MaTT,
                NgaySinh = NgaySinh,
                DiaChi = DiaChi,
                Email = Email
            };
            LibrariansList.Add(newLibrarian);

            // Clear the input fields
            MaTT = string.Empty;
            NgaySinh = DateTime.Now;
            DiaChi = string.Empty;
            Email = string.Empty;

        }

    }
}
