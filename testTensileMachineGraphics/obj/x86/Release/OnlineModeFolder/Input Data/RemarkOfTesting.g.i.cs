﻿#pragma checksum "..\..\..\..\..\OnlineModeFolder\Input Data\RemarkOfTesting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4ECAE68427B95C3EEDC8CE515BD8B76B"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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


namespace testTensileMachineGraphics.OnlineModeFolder.Input_Data {
    
    
    /// <summary>
    /// RemarkOfTesting
    /// </summary>
    public partial class RemarkOfTesting : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\..\..\..\OnlineModeFolder\Input Data\RemarkOfTesting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock tblNapomena;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\..\..\OnlineModeFolder\Input Data\RemarkOfTesting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RichTextBox rtfNapomena;
        
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
            System.Uri resourceLocater = new System.Uri("/testTensileMachineGraphics;component/onlinemodefolder/input%20data/remarkoftesti" +
                    "ng.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\OnlineModeFolder\Input Data\RemarkOfTesting.xaml"
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
            
            #line 5 "..\..\..\..\..\OnlineModeFolder\Input Data\RemarkOfTesting.xaml"
            ((testTensileMachineGraphics.OnlineModeFolder.Input_Data.RemarkOfTesting)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.tblNapomena = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.rtfNapomena = ((System.Windows.Controls.RichTextBox)(target));
            
            #line 41 "..\..\..\..\..\OnlineModeFolder\Input Data\RemarkOfTesting.xaml"
            this.rtfNapomena.MouseEnter += new System.Windows.Input.MouseEventHandler(this.rtfNapomena_MouseEnter);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

