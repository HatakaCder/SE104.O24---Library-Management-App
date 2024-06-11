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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLyThuVien.View
{
    /// <summary>
    /// Interaction logic for Librarian.xaml
    /// </summary>
    public partial class Librarian : UserControl
    {
        public Librarian()
        {
            InitializeComponent();
        }
        private void Checklb(object sender, RoutedEventArgs e)
        {

        }
        private void CheckAlllb(object sender, RoutedEventArgs e)
        {
            foreach (var item in Data.Items)
            {
                var row = Data.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (row != null)
                {
                    CheckBox checkBox = FindVisualChild<CheckBox>(row);

                    if (checkBox != null)
                    {
                        checkBox.IsChecked = true;
                    }
                }
            }
        }
        private void UncheckAlllb(object sender, RoutedEventArgs e)
        {
            foreach (var item in Data.Items)
            {
                var row = Data.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (row != null)
                {
                    CheckBox checkBox = FindVisualChild<CheckBox>(row);

                    if (checkBox != null)
                    {
                        checkBox.IsChecked = false;
                    }
                }
            }
        }
        private static T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);

                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    T childOfChild = FindVisualChild<T>(child);

                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }
    }
}
