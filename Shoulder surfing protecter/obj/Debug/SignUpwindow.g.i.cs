﻿#pragma checksum "..\..\SignUpwindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A8DBAFD0D93A1A65DACFACC035A78B55"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace Shoulder_surfing_protecter {
    
    
    /// <summary>
    /// SignUpwindow
    /// </summary>
    public partial class SignUpwindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 16 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas paintSurface;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdBar;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lbluser;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label lblId;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnDel;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAdd;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnReset;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\SignUpwindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnX;
        
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
            System.Uri resourceLocater = new System.Uri("/Shoulder surfing protecter;component/signupwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SignUpwindow.xaml"
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
            
            #line 4 "..\..\SignUpwindow.xaml"
            ((Shoulder_surfing_protecter.SignUpwindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.paintSurface = ((System.Windows.Controls.Canvas)(target));
            
            #line 16 "..\..\SignUpwindow.xaml"
            this.paintSurface.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.paintSurface_MouseDown1);
            
            #line default
            #line hidden
            
            #line 16 "..\..\SignUpwindow.xaml"
            this.paintSurface.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.paintSurface_MouseUp);
            
            #line default
            #line hidden
            return;
            case 3:
            this.grdBar = ((System.Windows.Controls.Grid)(target));
            return;
            case 4:
            this.lbluser = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.lblId = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.btnDel = ((System.Windows.Controls.Button)(target));
            
            #line 64 "..\..\SignUpwindow.xaml"
            this.btnDel.Click += new System.Windows.RoutedEventHandler(this.btnDel_Click_1);
            
            #line default
            #line hidden
            return;
            case 7:
            this.btnAdd = ((System.Windows.Controls.Button)(target));
            
            #line 65 "..\..\SignUpwindow.xaml"
            this.btnAdd.Click += new System.Windows.RoutedEventHandler(this.btnok_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.btnReset = ((System.Windows.Controls.Button)(target));
            
            #line 66 "..\..\SignUpwindow.xaml"
            this.btnReset.Click += new System.Windows.RoutedEventHandler(this.btndel_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.btnX = ((System.Windows.Controls.Button)(target));
            
            #line 67 "..\..\SignUpwindow.xaml"
            this.btnX.Click += new System.Windows.RoutedEventHandler(this.btnX_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

