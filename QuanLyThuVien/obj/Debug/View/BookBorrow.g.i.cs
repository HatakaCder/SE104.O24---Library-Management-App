﻿#pragma checksum "..\..\..\View\BookBorrow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "9D12FA2FF62B8F70BBCF282B69FC0E222C752AF0D2C6C406E096DB27720F4292"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
using QuanLyThuVien.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace QuanLyThuVien.View {
    
    
    /// <summary>
    /// BookBorrow
    /// </summary>
    public partial class BookBorrow : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 117 "..\..\..\View\BookBorrow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox timkiem;
        
        #line default
        #line hidden
        
        
        #line 141 "..\..\..\View\BookBorrow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid docgia;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\View\BookBorrow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid sach;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/QuanLyThuVien;component/view/bookborrow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\BookBorrow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 99 "..\..\..\View\BookBorrow.xaml"
            ((System.Windows.Controls.TextBox)(target)).TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.findSerch);
            
            #line default
            #line hidden
            return;
            case 2:
            this.timkiem = ((System.Windows.Controls.ComboBox)(target));
            
            #line 117 "..\..\..\View\BookBorrow.xaml"
            this.timkiem.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.xulytimkiemchange);
            
            #line default
            #line hidden
            return;
            case 3:
            this.docgia = ((System.Windows.Controls.DataGrid)(target));
            
            #line 155 "..\..\..\View\BookBorrow.xaml"
            this.docgia.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.docgia_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 155 "..\..\..\View\BookBorrow.xaml"
            this.docgia.AddHandler(System.Windows.Controls.ScrollViewer.ScrollChangedEvent, new System.Windows.Controls.ScrollChangedEventHandler(this.docgia_ScrollChanged));
            
            #line default
            #line hidden
            return;
            case 4:
            this.sach = ((System.Windows.Controls.DataGrid)(target));
            
            #line 193 "..\..\..\View\BookBorrow.xaml"
            this.sach.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Data_Sach);
            
            #line default
            #line hidden
            return;
            case 5:
            
            #line 283 "..\..\..\View\BookBorrow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 292 "..\..\..\View\BookBorrow.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_2);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

