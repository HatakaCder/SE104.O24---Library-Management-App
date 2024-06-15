using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLyThuVien.Commands
{
<<<<<<< HEAD
    public class RelayCommand: ICommand
=======
    public class RelayCommand : ICommand
>>>>>>> main
    {
        public event EventHandler CanExecuteChanged;
        private Action DoWork;
        public RelayCommand(Action work)
        {
            DoWork = work;
        }

        public bool CanExecute(object parameter)
        {
<<<<<<< HEAD
            //return DoWork != null;
=======
>>>>>>> main
            return true;
        }

        public void Execute(object parameter)
        {
            DoWork();
        }
    }
}
