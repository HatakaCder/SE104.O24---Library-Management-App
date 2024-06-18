using QuanLyThuVien.Model;
using QuanLyThuVien.Services;
using QuanLyThuVien.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using QuanLyThuVien.View;
using QuanLyThuVien.Class;
using System.Windows.Media.Media3D;

namespace QuanLyThuVien.ViewModel
{
    public class LibrarianVM : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion 
        LibrarianService librarianService;
        private string selectedOption = "";
        private string task = "add";
        private string oldpw = "";
        private AddLibrarian addW;
        private UpdateLibrarian updateW;
        PasswordHashing passwordHashing;
        public LibrarianVM()
        {
            librarianService = new LibrarianService();
            LoadData();
            addCommand = new RelayCommand(Add);
            addPopUpCommand = new RelayCommand(AddPopUp);
            updateCommand = new RelayCommand(Update);
            updatePopUpCommand = new RelayCommand(UpdatePopUp);
            currentLibrarian = new LibrarianDTO();
            currentUser = new UserDTO();
            selectedLibrarian = new LibrarianDTO();
            NamChecked = true;
            passwordHashing = new PasswordHashing();
            deleteCommand = new RelayCommand(Delete);
            searchCommand = new RelayCommand(Search);

        }
        #region DisplayOperation
        private ObservableCollection<LibrarianDTO> librariansList;

        public ObservableCollection<LibrarianDTO> LibrariansList
        {
            get { return librariansList; }
            set { librariansList = value; OnPropertyChanged("LibrariansList"); }
        }
        private void LoadData()
        {
            LibrariansList = new ObservableCollection<LibrarianDTO>(librarianService.GetAll());
        }
        #endregion
        #region AddOperation
        private RelayCommand addCommand;

        public RelayCommand AddCommand
        {
            get { return addCommand; }
        }
        private RelayCommand addPopUpCommand;

        public RelayCommand AddPopUpCommand
        {
            get { return addPopUpCommand; }
        }
        private LibrarianDTO currentLibrarian;

        public LibrarianDTO CurrentLibrarian
        {
            get { return currentLibrarian; }
            set { currentLibrarian = value; OnPropertyChanged("CurrentLibrarian"); }
        }
        private UserDTO currentUser;

        public UserDTO CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; OnPropertyChanged("CurrentUser"); }
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
        public void AddPopUp()
        {
            CurrentLibrarian = new LibrarianDTO();
            CurrentLibrarian.GioiTinh = selectedOption;
            CurrentLibrarian.NgaySinh = DateTime.Now;
            addW = new AddLibrarian(this);
            addW.Show();
        }
        public void Add()
        {
            try
            {
                var IsAdded = librarianService.Add(CurrentLibrarian, CurrentUser);
                if (IsAdded)
                {
                    MessageBox.Show("Thêm thủ thư thành công!");
                    LoadData();
                    addW.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region UpdateOperation
        private LibrarianDTO selectedLibrarian;

        public LibrarianDTO SelectedLibrarian
        {
            get { return selectedLibrarian; }
            set { selectedLibrarian = value; OnPropertyChanged("SelectedLibrarian"); }
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
        public void UpdatePopUp()
        {
            try
            {
                CurrentLibrarian.Email = selectedLibrarian.Email;
                CurrentLibrarian.HoTen = selectedLibrarian.HoTen;
                CurrentLibrarian.GioiTinh = selectedLibrarian.GioiTinh;
                CurrentLibrarian.DiaChi = selectedLibrarian.DiaChi;
                CurrentLibrarian.NgaySinh = selectedLibrarian.NgaySinh;
                CurrentLibrarian.SoDT = selectedLibrarian.SoDT;
                CurrentLibrarian.MaTT = selectedLibrarian.MaTT;
                selectedOption = CurrentLibrarian.GioiTinh;
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
                ACCOUNT CurrentAcc = librarianService.findUserTT(selectedLibrarian.MaTT);
                CurrentUser.TaiKhoan = CurrentAcc.TaiKhoan;
                CurrentUser.MatKhau = CurrentAcc.MatKhau;
                oldpw = CurrentAcc.MatKhau;
                updateW = new UpdateLibrarian(this);
                updateW.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Update()
        {
            bool IsUpdated = false;
            try
            {
                bool changePw = oldpw != CurrentUser.MatKhau;
                CurrentLibrarian.GioiTinh = selectedOption;
                IsUpdated = librarianService.Update(CurrentLibrarian, CurrentUser, changePw);
                if (IsUpdated)
                {
                    MessageBox.Show("Chỉnh sửa thủ thư thành công!");
                    LoadData();
                    updateW.Close();
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
                List<string> maTTDeleted = new List<string>();
                foreach (var item in LibrariansList)
                {
                    if (item.IsChecked == true)
                    {
                        maTTDeleted.Add(item.MaTT);
                    }
                }
                if (maTTDeleted.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu cần xoá!");
                    return;
                }
                var result = MessageBox.Show("Bạn có chắc chắn muốn xoá?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No) return;
                bool IsDeleted = librarianService.Delete(maTTDeleted);
                if (IsDeleted)
                {
                    MessageBox.Show("Xoá dữ liệu được chọn thành công!");
                    LoadData();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region SearchOperation
        private string searchContent;

        public string SearchContent
        {
            get { return searchContent; }
            set { searchContent = value; OnPropertyChanged("SearchContent"); }
        }
        private RelayCommand searchCommand;

        public RelayCommand SearchCommand
        {
            get { return searchCommand; }
        }
        public void Search()
        {
            try
            {
                LibrariansList = new ObservableCollection<LibrarianDTO>(librarianService.Search(SearchContent, "Name"));
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion
    }
}