﻿#pragma checksum "..\..\..\..\Pages\MenuPage.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "C9CB43689327287A776606AFC74CD1EB1CF07B24"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using BookSmart.Pages;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
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


namespace BookSmart.Pages {
    
    
    /// <summary>
    /// MenuPage
    /// </summary>
    public partial class MenuPage : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 47 "..\..\..\..\Pages\MenuPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblMenuTitle;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\..\Pages\MenuPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stackPanelOptions;
        
        #line default
        #line hidden
        
        
        #line 59 "..\..\..\..\Pages\MenuPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReplacingBooks;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\..\Pages\MenuPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnIdentifyingAreas;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\..\..\Pages\MenuPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFindingCallNumbers;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\..\Pages\MenuPage.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblAbout;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/BookSmart;component/pages/menupage.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Pages\MenuPage.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.11.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.lblMenuTitle = ((System.Windows.Controls.Label)(target));
            return;
            case 2:
            this.stackPanelOptions = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.btnReplacingBooks = ((System.Windows.Controls.Button)(target));
            
            #line 60 "..\..\..\..\Pages\MenuPage.xaml"
            this.btnReplacingBooks.Click += new System.Windows.RoutedEventHandler(this.btnReplacingBooks_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnIdentifyingAreas = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\..\..\Pages\MenuPage.xaml"
            this.btnIdentifyingAreas.Click += new System.Windows.RoutedEventHandler(this.btnIdentifyingAreas_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnFindingCallNumbers = ((System.Windows.Controls.Button)(target));
            
            #line 68 "..\..\..\..\Pages\MenuPage.xaml"
            this.btnFindingCallNumbers.Click += new System.Windows.RoutedEventHandler(this.btnFindingCallNumbers_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.lblAbout = ((System.Windows.Controls.Label)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

