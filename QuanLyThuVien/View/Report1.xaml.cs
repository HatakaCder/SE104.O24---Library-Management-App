using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyThuVien.Model;
using QuanLyThuVien.ViewModel;
using Excel = Microsoft.Office.Interop.Excel;

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for Report1.xaml
    /// </summary>
    public partial class Report1 : UserControl
    {
        private Report1VM vm;
        private DateTime dt;
        private string type;
        public Report1(DateTime dt, string type)
        {
            vm = new Report1VM(dt, type);
            InitializeComponent();
            this.DataContext = vm;
            this.dt = dt;
            this.type = type;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application excelApp = new Excel.Application();

            Excel.Workbook workbook = excelApp.Workbooks.Add();

            Excel.Worksheet worksheet = (Excel.Worksheet)workbook.ActiveSheet;
            worksheet.Cells[1, 1] = "BM7.1.Báo Cáo Thông Kê Tình Hình Mượn Sách Theo Thể Loại";
            string format = "";
            if (type == "Ngày")
            {
                format = "dd/MM/yyyy";
            }
            else if (type == "Tháng") format = "MM/yyyy";
            else format = "yyyy";
            worksheet.Cells[2, 1] = type + " " + dt.ToString(format) + ":";

            worksheet.Cells[4, 1] = "STT";
            worksheet.Cells[4, 2] = "Tên Thể Loại";
            worksheet.Cells[4, 3] = "Số Lượt Mượn";
            worksheet.Cells[4, 4] = "Tỉ Lệ";

            for (int i = 0; i < Table.Items.Count; i++)
            {
                var item = (Report1DTO)Table.Items[i];
                worksheet.Cells[i + 5, 1] = item.STT.ToString();
                worksheet.Cells[i + 5, 2] = item.TenTheLoai;
                worksheet.Cells[i + 5, 3] = item.SoLuotMuon.ToString();
                worksheet.Cells[i + 5, 4] = item.TyLe.ToString();
            }
            worksheet.Cells[6 + Table.Items.Count , 4] = "Tổng số lượt mượn: " + txtBorrowedBooks.Text;
            worksheet.Columns.AutoFit();
            string filePath = @"C:\Users\Admin\Desktop\a\report_thong_ke_muon_sach_theo_TL.xlsx";
            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show("File Excel đã được tạo thành công!", "Thông báo");
        }
    }
}
