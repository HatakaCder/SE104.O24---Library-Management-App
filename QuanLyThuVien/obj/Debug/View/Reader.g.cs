﻿#pragma checksum "..\..\..\View\Reader.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "AFE9C089E3500BF238BA24C773974578680647E1440FC855A84C16E12FD502EC"
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
    /// Reader
    /// </summary>
    public partial class Reader : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 133 "..\..\..\View\Reader.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dataTable;
        
        #line default
        #line hidden
        
        
        #line 221 "..\..\..\View\Reader.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btAddReader;
        
        #line default
        #line hidden
        
        
        #line 241 "..\..\..\View\Reader.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btUpdateReader;
        
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
            System.Uri resourceLocater = new System.Uri("/QuanLyThuVien;component/view/reader.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\Reader.xaml"
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
            this.dataTable = ((System.Windows.Controls.DataGrid)(target));
            
            #line 148 "..\..\..\View\Reader.xaml"
            this.dataTable.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.dataTable_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btAddReader = ((System.Windows.Controls.Button)(target));
            
            #line 229 "..\..\..\View\Reader.xaml"
            this.btAddReader.Click += new System.Windows.RoutedEventHandler(this.btAddReader_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btUpdateReader = ((System.Windows.Controls.Button)(target));
            
            #line 250 "..\..\..\View\Reader.xaml"
            this.btUpdateReader.Click += new System.Windows.RoutedEventHandler(this.btUpdateReader_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

