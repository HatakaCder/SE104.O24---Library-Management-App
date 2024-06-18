using QuanLyThuVien.ViewModel;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using QuanLyThuVien.Model;
using System.IO;



namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for Report2.xaml
    /// </summary>
    public partial class Report2 : UserControl
    {
        private Report2VM vm;
        private DateTime dt;
        private string type;
        public Report2(DateTime dt, string type)
        {
            vm = new Report2VM(dt, type);
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
            worksheet.Cells[1, 1] = "BM7.2 Báo Cáo Thống Kê Sách Trả Trễ";
            string format = "";
            if (type == "Ngày")
            {
                format = "dd/MM/yyyy";
            }
            else if (type == "Tháng") format = "MM/yyyy";
            else format = "yyyy";
            worksheet.Cells[2, 1] = type + " " + dt.ToString(format) + ":";

            worksheet.Cells[4, 1] = "STT";
            worksheet.Cells[4, 2] = "Tên Sách";
            worksheet.Cells[4, 3] = "Ngày Mượn";
            worksheet.Cells[4, 4] = "Số Ngày Trả Trễ";

            worksheet.Cells[5, 1] = "1";
            worksheet.Cells[6, 1] = "2";
            ;

            for (int i = 0; i < Table.Items.Count; i++)
            {
                var item = (Report2DTO)Table.Items[i];
                worksheet.Cells[i + 5, 1] = item.STT.ToString();
                worksheet.Cells[i + 5, 2] = item.TenSach;
                worksheet.Cells[i + 5, 3] = item.NgayMuon.ToString();
                worksheet.Cells[i + 5, 4] = item.SoNgayTraTre.ToString();
            }
            worksheet.Columns.AutoFit();
            string filePath = @"C:\Users\Admin\Desktop\a\report_thong_ke_sach_tra_tre.xlsx";
            workbook.SaveAs(filePath);
            workbook.Close();
            excelApp.Quit();

            MessageBox.Show("File Excel đã được tạo thành công!", "Thông báo");
        }
    }
}
