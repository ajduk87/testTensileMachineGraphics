using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace testTensileMachineGraphics.OnlineModeFolder.Input_Data
{
    /// <summary>
    /// Interaction logic for MaterialForTesting.xaml
    /// </summary>
    public partial class MaterialForTesting : Window
    {

        private OnlineFileHeader onHeader;
        public OnlineFileHeader OnlineHeader
        {
            get { return onHeader; }
        }

        private double xconst = 1250 + 105;
        private double yconst = 60 + 207 + 10;

        private double xconst_InPrintScreenMode = 1068;
        private double yconst_InPrintScreenMode = 612;


        public MaterialForTesting(OnlineFileHeader onlineHeader)
        {
            try
            {
                InitializeComponent();

                onHeader = onlineHeader;

                //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (onHeader.OnlineModeInstance.Plotting.Printscreen.IsPrintScreenEmpty == true)
                {
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                    this.Left = xconst;
                    this.Top = yconst;
                }
                else
                {
                    //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                    this.Left = xconst_InPrintScreenMode;
                    this.Top = yconst_InPrintScreenMode;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {public MaterialForTesting(OnlineFileHeader onlineHeader)}", System.DateTime.Now);
            }
        }

       

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                onHeader.GeneralData.Close();
                onHeader.ConditionsOfTesting.Close();
                onHeader.MaterialForTesting.Close();
                onHeader.PositionOfTube.Close();
                onHeader.RemarkOfTesting.Close();
                onHeader.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {private void Window_Closed(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

       

        #region SelectAll

        private void tfProizvodjac_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                tfProizvodjac.SelectAll();
                tfProizvodjac.Focus();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {private void tfProizvodjac_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfProizvodjac_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfDobavljac.SelectAll();
                    tfDobavljac.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {private void tfProizvodjac_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDobavljac_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfPolazniKvalitet.SelectAll();
                    tfPolazniKvalitet.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {private void tfDobavljac_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfPolazniKvalitet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfNazivnaDebljina.SelectAll();
                    tfNazivnaDebljina.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {private void tfPolazniKvalitet_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfNazivnaDebljina_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {

                    tfProizvodjac.SelectAll();
                    tfProizvodjac.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MaterialForTesting.xaml.cs] {private void tfNazivnaDebljina_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void rbtnValjani_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                // MoveFocus takes a TraversalRequest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
            }
            catch (Exception ex)
            {
                
            }
        }


        private void rbtnVučeni_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                // MoveFocus takes a TraversalRequest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
            }
            catch (Exception ex) 
            {

            }
        }

        private void rbtnKovani_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                // MoveFocus takes a TraversalRequest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
            }
            catch (Exception ex) 
            {

            }
        }

        private void rbtnLiveni_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                // MoveFocus takes a TraversalRequest as its argument.
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    elementWithFocus.MoveFocus(request);
                }
            }
            catch (Exception ex) 
            {

            }
        }


        #endregion

        

        
      


        

       

       

       
    }
}
