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
using testTensileMachineGraphics.Reports;

namespace testTensileMachineGraphics.OnlineModeFolder.Input_Data
{
    /// <summary>
    /// Interaction logic for GeneralData.xaml
    /// </summary>
    public partial class GeneralData : Window
    {
        private OnlineFileHeader onHeader;
        public OnlineFileHeader OnlineHeader
        {
            get { return onHeader; }
        }

        private double xconst = 1250 + 105;
        private double yconst = 60;


        public double xconst_InPrintScreenMode = 1068;
        public double yconst_InPrintScreenMode = 615;

        public GeneralData(OnlineFileHeader onlineHeader)
        {
            try
            {
                InitializeComponent();

                onHeader = onlineHeader;

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
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {public GeneralData(OnlineFileHeader onlineHeader)}", System.DateTime.Now);
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
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void Window_Closed(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBrUzorka_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfBrUzorkaNumberOfSample.Text = "1";


                LastInputOutputSavedData.tfBrUzorka_GeneralData = tfBrUzorka.Text + "/" + tfBrUzorkaNumberOfSample.Text;
                //onHeader.OnlineModeInstance.WriteXMLLastOnlineHeader();

                onHeader.OnlineModeInstance.Plotting.Printscreen.SetRedColorForExistingSample();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfBrUzorka_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

     
        #region SelectAll

        private void tfBrUzorka_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfBrUzorkaNumberOfSample.SelectAll();
                    tfBrUzorkaNumberOfSample.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfBrUzorka_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBrUzorkaNumberOfSample_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfSarza.SelectAll();
                    tfSarza.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfBrUzorkaNumberOfSample_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfSarza_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfRadniNalog.SelectAll();
                    tfRadniNalog.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfSarza_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRadniNalog_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfNaručilac.SelectAll();
                    tfNaručilac.Focus();
                }
            }
            catch(Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfRadniNalog_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfNaručilac_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfOperator.SelectAll();
                    tfOperator.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfNaručilac_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfOperator_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfBrZbIzvestaja.SelectAll();
                    tfBrZbIzvestaja.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfOperator_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfBrZbIzvestaja_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfBrUzorka.SelectAll();
                    tfBrUzorka.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfBrZbIzvestaja_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        private void tfBrUzorkaNumberOfSample_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                LastInputOutputSavedData.tfBrUzorka_GeneralData = tfBrUzorka.Text + "/" + tfBrUzorkaNumberOfSample.Text;
                //onHeader.OnlineModeInstance.WriteXMLLastOnlineHeader();

                onHeader.OnlineModeInstance.Plotting.Printscreen.SetRedColorForExistingSample();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GeneralData.xaml.cs] {private void tfBrUzorkaNumberOfSample_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfOperator_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void tfBrZbIzvestaja_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tfBrZbIzvestaja.Text.Contains('/') == true || tfBrZbIzvestaja.Text.Contains('\\') == true || tfBrZbIzvestaja.Text.Contains(':') == true || tfBrZbIzvestaja.Text.Contains('*') == true || tfBrZbIzvestaja.Text.Contains('?') == true || tfBrZbIzvestaja.Text.Contains('<') == true || tfBrZbIzvestaja.Text.Contains('>') == true || tfBrZbIzvestaja.Text.Contains('|') == true)
            {
                MessageBox.Show("Ime fajla ne moze da sadrzi sledece karaktere \\/:*?<>|");
                tfBrZbIzvestaja.Text = tfBrZbIzvestaja.Text.Substring(0,tfBrZbIzvestaja.Text.Length - 1);
            }
        }

       

        

     

       

       
        


     

    }
}
