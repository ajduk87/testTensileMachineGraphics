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
    /// Interaction logic for RemarkOfTesting.xaml
    /// </summary>
    public partial class RemarkOfTesting : Window
    {

        private OnlineFileHeader onHeader;
        public OnlineFileHeader OnlineHeader
        {
            get { return onHeader; }
        }

        private double xconst = 1250 + 300 + 85;
        private double yconst = 60 + 434 + 10;

        private double xconst_InPrintScreenMode = 1068;
        private double yconst_InPrintScreenMode = 647;


        public RemarkOfTesting(OnlineFileHeader onlineHeader)
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
                Logger.WriteNode(ex.Message.ToString() + "[RemarkOtTesting.xaml.cs] {public RemarkOfTesting(OnlineFileHeader onlineHeader)}", System.DateTime.Now);
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
                Logger.WriteNode(ex.Message.ToString() + "[RemarkOtTesting.xaml.cs] {private void Window_Closed(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

        private void rtfNapomena_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                rtfNapomena.SelectAll();
                rtfNapomena.Focus();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[RemarkOtTesting.xaml.cs] {private void rtfNapomena_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }
    }
}
