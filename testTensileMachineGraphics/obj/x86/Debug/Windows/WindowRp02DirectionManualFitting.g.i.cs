﻿#pragma checksum "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "906502A5CE4107B0D033A704D852D073"
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


namespace testTensileMachineGraphics.Windows {
    
    
    /// <summary>
    /// WindowRp02DirectionManualFitting
    /// </summary>
    public partial class WindowRp02DirectionManualFitting : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 12 "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtnRp02MovingByX;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtnRp02MovingByY;
        
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
            System.Uri resourceLocater = new System.Uri("/testTensileMachineGraphics;component/windows/windowrp02directionmanualfitting.xa" +
                    "ml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml"
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
            
            #line 4 "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml"
            ((testTensileMachineGraphics.Windows.WindowRp02DirectionManualFitting)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.rbtnRp02MovingByX = ((System.Windows.Controls.RadioButton)(target));
            
            #line 13 "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml"
            this.rbtnRp02MovingByX.Checked += new System.Windows.RoutedEventHandler(this.rbtnRp02MovingByX_Checked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.rbtnRp02MovingByY = ((System.Windows.Controls.RadioButton)(target));
            
            #line 20 "..\..\..\..\Windows\WindowRp02DirectionManualFitting.xaml"
            this.rbtnRp02MovingByY.Checked += new System.Windows.RoutedEventHandler(this.rbtnRp02MovingByY_Checked);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
