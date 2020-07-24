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
using System.Xml;

namespace testTensileMachineGraphics.OnlineModeFolder.Input_Data
{
    /// <summary>
    /// Interaction logic for ConditionsOfTesting.xaml
    /// </summary>
    public partial class ConditionsOfTesting : Window
    {
        private ForceRanges forceRanges;
        private OnlineFileHeader onHeader;
        public OnlineFileHeader OnlineHeader
        {
            get { return onHeader; }
        }

        private double xconst = 1250 + 105;
        private double yconst = 60 + 207 + 10 + 210 + 10;

        private double xconst_InPrintScreenMode = 1068;
        private double yconst_InPrintScreenMode = 515;

        public ConditionsOfTesting(OnlineFileHeader onlineHeader, ForceRanges fr)
        {
            try
            {
                InitializeComponent();

                var celzijusStepeni = "\xB0 C - ";//oznaka za celzijusov stepen // or "unit\xB2"


                var celzijusStepeni2 = "\xB0 C";


                tlbTemperaturaStepeni1.Text = celzijusStepeni2;

                onHeader = onlineHeader;
                forceRanges = fr;
                loadXMLFileForceRanges2();
                //rbtnNo.IsChecked = true;
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
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {public ConditionsOfTesting(OnlineFileHeader onlineHeader)}", System.DateTime.Now);
            }
        }

        private void loadXMLFileForceRanges2()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.ForceRangesonlineOptions);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("forceUpper1"))
                        {
                            ForceRangesOptions.forceUpper1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            cmbBegOpsegMasine.Items.Add(ForceRangesOptions.forceUpper1.ToString());
                        }

                        if (textReader.Name.Equals("forceUpper2"))
                        {
                            ForceRangesOptions.forceUpper2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            cmbBegOpsegMasine.Items.Add(ForceRangesOptions.forceUpper2.ToString());
                        }
                        if (textReader.Name.Equals("forceUpper3"))
                        {
                            ForceRangesOptions.forceUpper3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            cmbBegOpsegMasine.Items.Add(ForceRangesOptions.forceUpper3.ToString());
                        }
                        if (textReader.Name.Equals("forceUpper4"))
                        {
                            ForceRangesOptions.forceUpper4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            cmbBegOpsegMasine.Items.Add(ForceRangesOptions.forceUpper4.ToString());
                        }
                        if (textReader.Name.Equals("forceUpper5"))
                        {
                            ForceRangesOptions.forceUpper5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            cmbBegOpsegMasine.Items.Add(ForceRangesOptions.forceUpper5.ToString());
                        }
                        if (textReader.Name.Equals("forceUpper6"))
                        {
                            //ForceRangesOptions.forceUpper6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            //cmbBegOpsegMasine.Items.Add(ForceRangesOptions.forceUpper6.ToString());
                        }



                        if (textReader.Name.Equals("rangeChosen"))
                        {
                            ForceRangesOptions.rangeChosen = textReader.ReadElementContentAsInt();
                            switch (ForceRangesOptions.rangeChosen)
                            {
                                case 1:
                                    {

                                        cmbBegOpsegMasine.SelectedIndex = 0;
                                        break;
                                    }

                                case 2:
                                    {
                                        cmbBegOpsegMasine.SelectedIndex = 1;
                                        break;
                                    }


                                case 3:
                                    {

                                        cmbBegOpsegMasine.SelectedIndex = 2;
                                        break;
                                    }

                                case 4:
                                    {
                                        cmbBegOpsegMasine.SelectedIndex = 3;
                                        break;
                                    }

                                case 5:
                                    {

                                        cmbBegOpsegMasine.SelectedIndex = 4;
                                        break;
                                    }

                                //case 6:
                                //    {

                                //        cmbBegOpsegMasine.SelectedIndex = 5;
                                //        break;
                                //    }
                            }//end of switch
                        }

                    }
                }//end of while loop


                textReader.Close();
                //this.createOnlineGraphics(); ovo je glavna razlika u odnosu na onaj iz main poziva za loadoptions
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void LoadOnlineoptions()}", System.DateTime.Now);
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
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void Window_Closed(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

        #region SelectAll

        private void tfStandard_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                tfStandard.SelectAll();
                tfStandard.Focus();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfStandard_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfStandard_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfMetoda.SelectAll();
                    tfMetoda.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfStandard_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfMetoda_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfStandardZaN.SelectAll();
                    tfStandardZaN.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfMetoda_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfStandardZaN_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfTemperatura.SelectAll();
                    tfTemperatura.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfStandardZaN_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfTemperatura_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfMasina.SelectAll();
                    tfMasina.Focus();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfTemperatura_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfMasina_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfBegOpsegMasine.SelectAll();
                    tfBegOpsegMasine.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfMasina_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBegOpsegMasine_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfStandard.SelectAll();
                    tfStandard.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void tfBegOpsegMasine_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }



        private void rbtnYes_GotFocus(object sender, RoutedEventArgs e)
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
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void rbtnYes_GotFocus(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnNo_GotFocus(object sender, RoutedEventArgs e)
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
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void rbtnNo_GotFocus(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        private void rbtnYes_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                onHeader.OnlineModeInstance.IsEkstenziometerUsed = true;
                onHeader.OnlineModeInstance.dataReader.IsEkstenziometerUsed = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void rbtnYes_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnNo_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                onHeader.OnlineModeInstance.IsEkstenziometerUsed = false;
                onHeader.OnlineModeInstance.dataReader.IsEkstenziometerUsed = false;
                onHeader.OnlineModeInstance.ResultsInterface.chbRt05.IsChecked = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ConditionsOfTesting.xaml.cs] {private void rbtnNo_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void cmbBegOpsegMasine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tfBegOpsegMasine.Text = cmbBegOpsegMasine.SelectedItem.ToString();
            if (cmbBegOpsegMasine.SelectedIndex == 0)
            {
                forceRanges.forceRange1.IsChecked = true;
            }
            if (cmbBegOpsegMasine.SelectedIndex == 1)
            {
                forceRanges.forceRange2.IsChecked = true;
            }
            if (cmbBegOpsegMasine.SelectedIndex == 2)
            {
                forceRanges.forceRange3.IsChecked = true;
            }
            if (cmbBegOpsegMasine.SelectedIndex == 3)
            {
                forceRanges.forceRange4.IsChecked = true;
            }
            if (cmbBegOpsegMasine.SelectedIndex == 4)
            {
                forceRanges.forceRange5.IsChecked = true;
            }
            if (cmbBegOpsegMasine.SelectedIndex == 5)
            {
                forceRanges.forceRange6.IsChecked = true;
            }
        }


       

       



       

       

    }
}
