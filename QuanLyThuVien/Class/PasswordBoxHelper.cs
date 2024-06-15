using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace QuanLyThuVien.Class
{
        public static class PasswordBoxHelper
        {
            public static readonly DependencyProperty BoundPasswordProperty =
                DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxHelper), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

            public static readonly DependencyProperty BindPasswordProperty =
                DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, OnBindPasswordChanged));

            private static readonly DependencyProperty UpdatingPasswordProperty =
                DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false));

            public static string GetBoundPassword(DependencyObject dp)
            {
                return (string)dp.GetValue(BoundPasswordProperty);
            }

            public static void SetBoundPassword(DependencyObject dp, string value)
            {
                dp.SetValue(BoundPasswordProperty, value);
            }

            public static bool GetBindPassword(DependencyObject dp)
            {
                return (bool)dp.GetValue(BindPasswordProperty);
            }

            public static void SetBindPassword(DependencyObject dp, bool value)
            {
                dp.SetValue(BindPasswordProperty, value);
            }

            private static bool GetUpdatingPassword(DependencyObject dp)
            {
                return (bool)dp.GetValue(UpdatingPasswordProperty);
            }

            private static void SetUpdatingPassword(DependencyObject dp, bool value)
            {
                dp.SetValue(UpdatingPasswordProperty, value);
            }

            private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
            {
                PasswordBox passwordBox = dp as PasswordBox;

                if (passwordBox != null)
                {
                    passwordBox.PasswordChanged -= PasswordChanged;

                    if (!GetUpdatingPassword(passwordBox))
                    {
                        passwordBox.Password = (string)e.NewValue;
                    }

                    passwordBox.PasswordChanged += PasswordChanged;
                }
            }

            private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
            {
                PasswordBox passwordBox = dp as PasswordBox;

                if (passwordBox != null)
                {
                    bool wasBound = (bool)e.OldValue;
                    bool needToBind = (bool)e.NewValue;

                    if (wasBound)
                    {
                        passwordBox.PasswordChanged -= PasswordChanged;
                    }

                    if (needToBind)
                    {
                        passwordBox.PasswordChanged += PasswordChanged;
                    }
                }
            }

            private static void PasswordChanged(object sender, RoutedEventArgs e)
            {
                PasswordBox passwordBox = sender as PasswordBox;

                SetUpdatingPassword(passwordBox, true);
                SetBoundPassword(passwordBox, passwordBox.Password);
                SetUpdatingPassword(passwordBox, false);
            }
        }
}
