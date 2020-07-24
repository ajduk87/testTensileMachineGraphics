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
using testTensileMachineGraphics.Options;
using System.Threading;
using System.Xml;
using Microsoft.Research.DynamicDataDisplay.Charts;

namespace testTensileMachineGraphics.Windows
{
    /// <summary>
    /// Interaction logic for WindowForChosingPoints.xaml
    /// </summary>
    public partial class WindowForChosingPoints : Window
    {

        private bool isOriginalCheckboxCheckedProgramally = false;
        private bool isChangeRatioCheckboxCheckedProgramally = false;

        public bool IsOriginalCheckboxCheckedProgramally
        {
            get { return isOriginalCheckboxCheckedProgramally; }
            set { isOriginalCheckboxCheckedProgramally = value; }
        }

        public bool IsChangeRatioCheckboxCheckedProgramally
        {
            get { return isChangeRatioCheckboxCheckedProgramally; }
            set { isChangeRatioCheckboxCheckedProgramally = value; }
        }


        private GraphicPlotting _graphicPlotting;

        public GraphicPlotting GraphicPlotting
        {
            get { return _graphicPlotting; }
            set 
            {
                if(value != null)
                {
                    _graphicPlotting = value;
                }
            }
        }

        private const double xconst = 100;
        private const double yconst = 0;

        private double x;
        public double X 
        {
            get { return x; }
            set { x = value; }
        }

        private double y;
        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public void setOriginalCheckbox() 
        {
            try
            {
                isOriginalCheckboxCheckedProgramally = true;
                if (OptionsInPlottingMode.isOriginalCheckBoxChecked == true)
                {
                    chbOriginal.IsChecked = true;
                }
                else
                {
                    chbOriginal.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {public void setOriginalCheckbox()}", System.DateTime.Now);
            }
        }

    
        public void setChangeRatioCheckbox()
        {
            try
            {
                isChangeRatioCheckboxCheckedProgramally = true;
                if (OptionsInPlottingMode.isChangeRatioCheckBoxChecked == true)
                {
                    chbChangeRatio.IsChecked = true;
                }
                else
                {
                    chbChangeRatio.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {public void setChangeRatioCheckbox()}", System.DateTime.Now);
            }
        }

  
        public WindowForChosingPoints()
        {
            try
            {
                InitializeComponent();

                initializeOriginalCheckbox();


                if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                {
                    rbtnR2R4.Content = "R2/R4";
                }
                else
                {
                    rbtnR2R4.Content = "R3/R4";
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {public WindowForChosingPoints()}", System.DateTime.Now);
            }
        }


        private void initializeOriginalCheckbox() 
        {
            try
            {

                OptionsInPlottingMode opPlotting = new OptionsInPlottingMode();

                XmlTextReader textReader = new XmlTextReader(Constants.plottingModeOptionsXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("isOriginalCheckBoxChecked"))
                        {
                            string temp = textReader.ReadElementContentAsString();
                            if (temp.Equals("True"))
                            {
                                OptionsInPlottingMode.isOriginalCheckBoxChecked = true;
                            }
                            if (temp.Equals("False"))
                            {
                                OptionsInPlottingMode.isOriginalCheckBoxChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("isChangeRatioCheckBoxChecked"))
                        {
                            string temp = textReader.ReadElementContentAsString();
                            if (temp.Equals("True"))
                            {
                                OptionsInPlottingMode.isChangeRatioCheckBoxChecked = true;
                            }
                            if (temp.Equals("False"))
                            {
                                OptionsInPlottingMode.isChangeRatioCheckBoxChecked = false;
                            }
                        }

                    }
                }//end of while loop

                textReader.Close();
                //this.createOfflineGraphics(); ovo je glavna razlika u odnosu na onaj iz main poziva za loadoptions

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString() + " {initializeOriginalCheckbox}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void initializeOriginalCheckbox()}", System.DateTime.Now);
            }
        }

        public void setWindowForChosingPoints() 
        {
            try
            {
                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = x + xconst;
                this.Top = y + yconst;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {public void setWindowForChosingPoints()}", System.DateTime.Now);
            }

        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _graphicPlotting.IsWindowForChosingPointsShown = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnReH_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = true;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnReH_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnReH_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnReL_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = true;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnReL_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnReL_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnRp02_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = true;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnRp02_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnRp02_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnRm_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = true;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnRm_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnRm_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnA_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = true;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnA_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnA_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnT1_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = true;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnT1_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnT1_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnT2_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = true;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnT2_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnT2_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void rbtnT3_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = true;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnT3_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnT3_Unchecked(object sender, RoutedEventArgs e)
        {

        }


        private void rbtnR2R4_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = true;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void rbtnR2R4_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnR2R4_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        //private void rbtnOriginal_Checked(object sender, RoutedEventArgs e)
        //{
        //    _graphicPlotting.IsReHActive = false;
        //    _graphicPlotting.IsReLActive = false;
        //    _graphicPlotting.IsRp02Active = false;
        //    _graphicPlotting.IsRmActive = false;
        //    _graphicPlotting.IsAActive = false;
        //    _graphicPlotting.IsT1Active = false;
        //    _graphicPlotting.IsT2Active = false;
        //    _graphicPlotting.IsT3Active = false;
        //    _graphicPlotting.IsR2R4Active = false;
        //    _graphicPlotting.IsOriginalActive = true;
        //    this.Close();
        //}

        //private void rbtnOriginal_Unchecked(object sender, RoutedEventArgs e)
        //{

        //}

        private void chbOriginal_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                chbOriginal.IsChecked = true;
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = true;
                _graphicPlotting.IsChangeRatio = false;

                OptionsInPlottingMode.isOriginalCheckBoxChecked = true;
                _graphicPlotting.WriteXMLFileOffline();
                if (isOriginalCheckboxCheckedProgramally == false)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void chbOriginal_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbOriginal_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                chbOriginal.IsChecked = false;
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = true;
                _graphicPlotting.IsChangeRatio = false;


                OptionsInPlottingMode.isOriginalCheckBoxChecked = false;
                _graphicPlotting.WriteXMLFileOffline();
                if (isOriginalCheckboxCheckedProgramally == false)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void chbOriginal_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbChangeRatio_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                chbChangeRatio.IsChecked = true;
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = true;



                OptionsInPlottingMode.isChangeRatioCheckBoxChecked = true;
                _graphicPlotting.WriteXMLFileOffline();
                if (isChangeRatioCheckboxCheckedProgramally == false)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void chbChangeRatio_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbChangeRatio_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                chbChangeRatio.IsChecked = false;
                _graphicPlotting.IsReHActive = false;
                _graphicPlotting.IsReLActive = false;
                _graphicPlotting.IsRp02Active = false;
                _graphicPlotting.IsRmActive = false;
                _graphicPlotting.IsAActive = false;
                _graphicPlotting.IsT1Active = false;
                _graphicPlotting.IsT2Active = false;
                _graphicPlotting.IsT3Active = false;
                _graphicPlotting.IsR2R4Active = false;
                _graphicPlotting.IsOriginalActive = false;
                _graphicPlotting.IsChangeRatio = true;


                OptionsInPlottingMode.isChangeRatioCheckBoxChecked = false;
                _graphicPlotting.WriteXMLFileOffline();
                if (isChangeRatioCheckboxCheckedProgramally == false)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForChosingPoints.xaml.cs] {private void chbChangeRatio_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

       
    }
}
