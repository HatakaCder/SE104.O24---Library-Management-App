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

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for Report.xaml
    /// </summary>
    public partial class Report : UserControl
    {
        public Report()
        {
            InitializeComponent();
            cbTypeReport.SelectedIndex = 0;
            cbTypeTime.SelectedIndex = 0;
            dpDate.SelectedDate = DateTime.Now;
            show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            show();
        }
        private void show()
        {
            ComboBoxItem selectedReportItem = (ComboBoxItem)cbTypeReport.SelectedItem;
            string typeReport = selectedReportItem.Content.ToString();
            ComboBoxItem selectedTypeItem = (ComboBoxItem)cbTypeTime.SelectedItem;
            string typeTime = selectedTypeItem.Content.ToString();
            string dateFormat = "";
            if (typeTime == "Ngày") dateFormat = "dd/MM/yyyy";
            else if (typeTime == "Tháng") dateFormat = "MM/yyyy";
            else if (typeTime == "Năm") dateFormat = "yyyy";
            DateTime selectedDate = dpDate.SelectedDate ?? DateTime.MinValue;
            if (typeReport == "Báo cáo thống kê tình hình mượn sách theo loại")
            {
                contentControl.Content = new Report1(selectedDate, typeTime);
                txtHeader.Text = typeReport + " " + typeTime.ToLower() + " " + selectedDate.ToString(dateFormat);
            }
            else if (typeReport == "Báo cáo thống kê sách trả trễ")
            {
                contentControl.Content = new Report2();
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
