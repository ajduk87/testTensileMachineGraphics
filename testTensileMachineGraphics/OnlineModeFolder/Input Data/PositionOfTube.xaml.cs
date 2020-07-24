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
    /// Interaction logic for PositionOfTube.xaml
    /// </summary>
    public partial class PositionOfTube : Window
    {

        private OnlineFileHeader onHeader;
        public OnlineFileHeader OnlineHeader
        {
            get { return onHeader; }
        }

        private double xconst = 1250 + 105;
        private double yconst = 60 + 207 + 10 + 210 + 10 + 307 + 10;

        private double xconst_InPrintScreenMode = 1068;
        private double yconst_InPrintScreenMode = 642;

        public PositionOfTube(OnlineFileHeader onlineHeader)
        {
            try
            {
                InitializeComponent();

                string customDegree = "\xB0";
                tlbCustomPravacValjanja.Text = customDegree;

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
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {public PositionOfTube(OnlineFileHeader onlineHeader)}", System.DateTime.Now);
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
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {private void Window_Closed(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCustomPravacValjanja_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double customPravacValjanjaDouble = Double.MinValue;
                string customPravacValjanjaStr = tfCustomPravacValjanja.Text;
                customPravacValjanjaStr = customPravacValjanjaStr.Replace(',', '.');
                bool isN = Double.TryParse(customPravacValjanjaStr, out customPravacValjanjaDouble);
                if (isN == false && tfCustomPravacValjanja.Text.Equals(String.Empty) == false)
                {
                    MessageBox.Show("Pravac ispitivanja valjanja mora biti unet u obliku broja!");
                    return;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {private void tfCustomPravacValjanja_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        #region SelectAll

        private void tfCustomPravacValjanja_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                tfCustomPravacValjanja.SelectAll();
                tfCustomPravacValjanja.Focus();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {private void tfCustomPravacValjanja_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCustomPravacValjanja_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfCustomSirinaTrake.SelectAll();
                    tfCustomSirinaTrake.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {private void tfCustomPravacValjanja_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCustomSirinaTrake_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfCustomDuzinaTrake.SelectAll();
                    tfCustomDuzinaTrake.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {private void tfCustomSirinaTrake_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCustomDuzinaTrake_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfCustomPravacValjanja.SelectAll();
                    tfCustomPravacValjanja.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[PositionOfTube.xaml.cs] {private void tfCustomDuzinaTrake_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

       

       

       
        //private void tfCustomPravacValjanja_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    double customPravacValjanjaDouble = Double.MinValue;
        //    string customPravacValjanjaStr = tfCustomPravacValjanja.Text;
        //    customPravacValjanjaStr = customPravacValjanjaStr.Replace(',','.');
        //    bool isN = Double.TryParse(customPravacValjanjaStr, out customPravacValjanjaDouble);
        //    if (isN == false && tfCustomPravacValjanja.Text.Equals(String.Empty) == false)
        //    {
        //        MessageBox.Show("Pravac ispitivanja mora biti unet u obliku broja!");
        //        return;
        //    }

        //}
    }
}
