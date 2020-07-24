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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using testTensileMachineGraphics.PointViewModel;
using testTensileMachineGraphics.Options;
using System.Xml;
using System.IO;
using testTensileMachineGraphics.OnlineModeFolder;
using testTensileMachineGraphics.Reports;
using testTensileMachineGraphics.Windows;
using testTensileMachineGraphics.MessageBoxes;

namespace testTensileMachineGraphics
{



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// podrazumeva se da je prvi uzorak zapamcen kada se aplikacija zatvorila
        /// </summary>
        public bool IsThisFirstSample = true;

        /// <summary>
        /// informacija kada se ucitava prvi fajl u aplikaciji (ne klikom na dugme vec selektovanjem taba2) informacija da li postoji jer ako ne postoji fitovanje mora da se preskoci
        /// </summary>
        public bool currentFileNotExist = false;

        private bool isOnlineModeFinished = false;//da li je zavrsen i jedan online mod od kada je aplikacija ukljucena 
        public bool IsOnlineModeFinished
        {
            get { return isOnlineModeFinished; }
            set { isOnlineModeFinished = value; }
        }

        /// <summary>
        /// sadrzi informaciju o prozoru online mode-a
        /// </summary>
        private OnlineMode onlineMode;
        /// <summary>
        /// sadrzi informaciju o prozoru offline mode-a
        /// </summary>
        private GraphicPlotting plotting;
        /// <summary>
        ///  sadrzi informaciju o prozoru animacionog mode-a
        /// </summary>
        private GraphicAnimation animation;
        /// <summary>
        ///  sadrzi informaciju o prozoru stampajuceg mode-a
        /// </summary>
        private PrintScreen printScreen;

        /// <summary>
        /// sadrzi informaciju o prozoru online mode-a
        /// </summary>
        public OnlineMode OnlineMode 
        {
            get { return onlineMode; }
            set 
            {
                if (value != null)
                {
                    onlineMode = value;
                }
            }
        }

        /// <summary>
        /// sadrzi informaciju o prozoru offline mode-a
        /// </summary>
        public GraphicPlotting Plotting 
        {
            get { return plotting; }
        }

        /// <summary>
        /// sadrzi informaciju o prozoru prikaz grafika
        /// </summary>
        public PrintScreen PrintScreen
        {
            get { return printScreen; }
        }

        public GraphicAnimation Animation
        {
            get { return animation; }
            set 
            {
                if (value != null)
                {
                    animation = value;
                }
            }
        }

        public static double SetPointAtDoubleInstedComma(string str) 
        {
            try
            {
                string temporary = str;
                temporary = temporary.Replace(',', '.');
                double number = 0;
                bool isN = double.TryParse(temporary, out number);
                if (isN)
                {
                    return number;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {public static double SetPointAtDoubleInstedComma(string str) }", System.DateTime.Now);
                return 0;
            }
        }


        private void isEkstenziometerUsedInitial() 
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.lastOnlineHeaderXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {


                        if (textReader.Name.Equals("rbtnYes_ConditionsOfTesting"))
                        {
                            string isYes = textReader.ReadElementContentAsString();
                            if (isYes.Equals("True"))
                            {
                                onlineMode.IsEkstenziometerUsed = true;
                                onlineMode.dataReader.IsEkstenziometerUsed = true;
                            }
                            if (isYes.Equals("False"))
                            {
                                onlineMode.IsEkstenziometerUsed = false;
                                onlineMode.dataReader.IsEkstenziometerUsed = false;
                            }
                        }
                        if (textReader.Name.Equals("rbtnNo_ConditionsOfTesting"))
                        {
                            string isNo = textReader.ReadElementContentAsString();
                            if (isNo.Equals("True"))
                            {
                                onlineMode.IsEkstenziometerUsed = false;
                                onlineMode.dataReader.IsEkstenziometerUsed = false;
                            }
                            if (isNo.Equals("False"))
                            {
                                onlineMode.IsEkstenziometerUsed = true;
                                onlineMode.dataReader.IsEkstenziometerUsed = true;
                            }
                        }



                    }
                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {private void isEkstenziometerUsedInitial()  }", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ucitava opcije koje su zapamcene zadnje u online modu
        /// svodi se na citanje xml fajla na putanji System.Environment.CurrentDirectory + "\\configuration\\onlineModeOptions.xml", koja je definisana u promenljivoj Constants.onlineModeOptionsXml
        /// i popunjavanje strukture OptionsInOnlineMode, 
        /// pa se na osnovu sadrzaja strukture OptionsInOnlineMode popunjava prozor [Options]OptionsOnline.xaml
        /// </summary>
        public void LoadOnlineoptions() 
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.onlineModeOptionsXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("refreshTimeInterval"))
                        {
                            OptionsInOnlineMode.refreshTimeInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("Resolution"))
                        {
                            OptionsInOnlineMode.Resolution = textReader.ReadElementContentAsInt();
                        }
                        if (textReader.Name.Equals("L0"))
                        {
                            OptionsInOnlineMode.L0 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("S0"))
                        {
                            OptionsInOnlineMode.S0 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("nutnDivide"))
                        {
                            OptionsInOnlineMode.nutnDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCalForceDivide.Text = OptionsInOnlineMode.nutnDivide.ToString();
                            //onlineMode.OptionsOnline.tfCalForceDivide.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCalForceDivide.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("nutnMultiple"))
                        {
                            OptionsInOnlineMode.nutnMultiple = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCalForceMultiple.Text = OptionsInOnlineMode.nutnMultiple.ToString();
                            //onlineMode.OptionsOnline.tfCalForceMultiple.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCalForceMultiple.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmDivide"))
                        {
                            OptionsInOnlineMode.mmDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCalElonDivide.Text = OptionsInOnlineMode.mmDivide.ToString();
                            //onlineMode.OptionsOnline.tfCalElonDivide.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCalElonDivide.Foreground = Brushes.White;

                        }
                        if (textReader.Name.Equals("mmCoeff"))
                        {
                            OptionsInOnlineMode.mmCoeff = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCalElonMultiple.Text = OptionsInOnlineMode.mmCoeff.ToString();
                            //onlineMode.OptionsOnline.tfCalElonMultiple.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCalElonMultiple.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmDivideWithEkstenziometer"))
                        {
                            OptionsInOnlineMode.mmDivideWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCalElonDivide2.Text = OptionsInOnlineMode.mmDivideWithEkstenziometer.ToString();
                            //onlineMode.OptionsOnline.tfCalElonDivide2.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCalElonDivide2.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmCoeffWithEkstenziometer"))
                        {
                            OptionsInOnlineMode.mmCoeffWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCalElonMultiple2.Text = OptionsInOnlineMode.mmCoeffWithEkstenziometer.ToString();
                            //onlineMode.OptionsOnline.tfCalElonMultiple2.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCalElonMultiple2.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("isAutoChecked"))
                        {
                            string isAuto = textReader.ReadElementContentAsString();
                            if (isAuto.Equals("True"))
                            {
                                OptionsInOnlineMode.isAutoChecked = true;
                                //onlineMode.rbtnAuto.IsChecked = true;

                                OptionsInOnlineMode.isManualChecked = false;
                                //onlineMode.rbtnManual.IsChecked = false;
                                //onlineMode.tfRatioForce.IsReadOnly = false;
                                //onlineMode.tfRatioElongation.IsReadOnly = false;

                                OptionsInOnlineMode.xRange = 0.95;
                                OptionsInOnlineMode.yRange = 0.95;
                            }
                            if (isAuto.Equals("False"))
                            {
                                OptionsInOnlineMode.isAutoChecked = false;
                                //onlineMode.rbtnAuto.IsChecked = false;

                                OptionsInOnlineMode.isManualChecked = true;
                                //onlineMode.rbtnManual.IsChecked = true;
                                //onlineMode.tfRatioForce.IsReadOnly = true;
                                //onlineMode.tfRatioElongation.IsReadOnly = true;
                            }
                        }
                        if (textReader.Name.Equals("isManualChecked"))
                        {
                            string isManual = textReader.ReadElementContentAsString();
                            if (isManual.Equals("True"))
                            {
                                OptionsInOnlineMode.isManualChecked = true;
                                //onlineMode.rbtnManual.IsChecked = true;
                                //onlineMode.tfRatioForce.IsReadOnly = true;
                                //onlineMode.tfRatioElongation.IsReadOnly = true;
                            }
                            if (isManual.Equals("False"))
                            {
                                OptionsInOnlineMode.isManualChecked = false;
                                //onlineMode.rbtnManual.IsChecked = false;
                                //onlineMode.tfRatioForce.IsReadOnly = false;
                                //onlineMode.tfRatioElongation.IsReadOnly = false;

                                OptionsInOnlineMode.xRange = 0.95;
                                OptionsInOnlineMode.yRange = 0.95;
                            }
                        }

                        if (textReader.Name.Equals("ratioElongation"))
                        {
                            OptionsInOnlineMode.xRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfRatioElongation.Text = OptionsInOnlineMode.xRange.ToString();
                            //onlineMode.OptionsOnline.tfRatioElongation.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfRatioElongation.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("ratioForce"))
                        {
                            OptionsInOnlineMode.yRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfRatioForce.Text = OptionsInOnlineMode.yRange.ToString();
                            //onlineMode.OptionsOnline.tfRatioForce.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfRatioForce.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("onlineWriteEndTimeInterval"))
                        {
                            OptionsInOnlineMode.onlineWriteEndTimeInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfEndOnlineWrite.Text = OptionsInOnlineMode.onlineWriteEndTimeInterval.ToString();
                            //onlineMode.OptionsOnline.tfEndOnlineWrite.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfEndOnlineWrite.Foreground = Brushes.White;
                        }


                        if (textReader.Name.Equals("calculateMaxForceForTf"))
                        {
                            string calculateMaxForceForTfStr = textReader.ReadElementContentAsString();
                            if (calculateMaxForceForTfStr.Equals("True"))
                            {
                                OptionsInOnlineMode.calculateMaxForceForTf = true;
                                onlineMode.tfMaxForceInKN.Text = Constants.ZERO;
                            }
                            if (calculateMaxForceForTfStr.Equals("False"))
                            {
                                OptionsInOnlineMode.calculateMaxForceForTf = false;
                                onlineMode.tfMaxForceInKN.Text = String.Empty;
                            }
                        }

                        if (textReader.Name.Equals("isCalibration"))
                        {
                            string isCalibrationStr = textReader.ReadElementContentAsString();
                            if (isCalibrationStr.Equals("True"))
                            {
                                OptionsInOnlineMode.isCalibration = true;
                            }
                            if (isCalibrationStr.Equals("False"))
                            {
                                OptionsInOnlineMode.isCalibration = false;
                            }
                        }

                        if (textReader.Name.Equals("timeIntervalForCalculationOfChangedParameters"))
                        {
                            OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("isE2E4BorderSelected"))
                        {
                            string isE2E4BorderSelectedstr = textReader.ReadElementContentAsString();
                            if (isE2E4BorderSelectedstr.Equals("True"))
                            {
                                OptionsInOnlineMode.isE2E4BorderSelected = true;
                            }
                            if (isE2E4BorderSelectedstr.Equals("False"))
                            {
                                OptionsInOnlineMode.isE2E4BorderSelected = false;

                            }
                        }


                        if (textReader.Name.Equals("E2E4Border"))
                        {
                            OptionsInOnlineMode.E2E4Border = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfE2E4Border.Text = OptionsInOnlineMode.E2E4Border.ToString();
                            //onlineMode.OptionsOnline.tfE2E4Border.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfE2E4Border.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("E3E4Border"))
                        {
                            OptionsInOnlineMode.E3E4Border = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfE3E4Border.Text = OptionsInOnlineMode.E3E4Border.ToString();
                            //this.OptionsOnline.tfE3E4Border.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfE3E4Border.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("COM"))
                        {
                            OptionsInOnlineMode.COM = Convert.ToInt32(textReader.ReadElementContentAsString());
                            onlineMode.OptionsOnline.tfCOM.Text = OptionsInOnlineMode.COM.ToString();
                            //onlineMode.OptionsOnline.tfCOM.Foreground = Brushes.Black;
                            onlineMode.OptionsOnline.tfCOM.Foreground = Brushes.White;
                        }

                    }
                }//end of while loop


                textReader.Close();
                onlineMode.createOnlineGraphics();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {public void LoadOnlineoptions()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ucitava opcije koje su zapamcene zadnje u offline modu
        /// svodi se na citanje xml fajla na putanji System.Environment.CurrentDirectory + "\\configuration\\plottingModeOptions.xml", koja je definisana u promenljivoj Constants.plottingModeOptionsXml
        /// i popunjavanje strukture OptionsInPlottingMode, 
        /// pa se na osnovu sadrzaja strukture OptionsInPlottingMode popunjava prozor [Options]OptionsPlotting.xaml
        /// </summary>
        public void LoadPlottingoptions() 
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
                            //if (textReader.Name.Equals("isContinuousDisplay"))
                            //{
                            //    string temp = textReader.ReadElementContentAsString();
                            //    if (temp.Equals("True"))
                            //    {
                            //        OptionsInPlottingMode.isContinuousDisplay = true;
                            //    }
                            //    if (temp.Equals("False"))
                            //    {
                            //        OptionsInPlottingMode.isContinuousDisplay = false;
                            //    }

                            //}
                            //if (textReader.Name.Equals("isDiscreteDisplay"))
                            //{
                            //    string temp = textReader.ReadElementContentAsString();
                            //    if (temp.Equals("True"))
                            //    {
                            //        OptionsInPlottingMode.isDiscreteDisplay = true;
                            //    }
                            //    if (temp.Equals("False"))
                            //    {
                            //        OptionsInPlottingMode.isDiscreteDisplay = false;
                            //    }
                            //}
                            //if (textReader.Name.Equals("disResolution"))
                            //{
                            //    OptionsInPlottingMode.disResolution = textReader.ReadElementContentAsInt();
                            //}
                            if (textReader.Name.Equals("Resolution"))
                            {
                                OptionsInPlottingMode.Resolution = textReader.ReadElementContentAsInt();
                                plotting.OptionsPlotting.tfResolution.Text = OptionsInPlottingMode.Resolution.ToString();
                                //plotting.OptionsPlotting.tfResolution.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfResolution.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("DerivationResolution"))
                            {
                                OptionsInPlottingMode.DerivationResolution = textReader.ReadElementContentAsInt();
                                plotting.OptionsPlotting.tfDerivationResolution.Text = OptionsInPlottingMode.DerivationResolution.ToString();
                                //plotting.OptionsPlotting.tfDerivationResolution.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfDerivationResolution.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("nutnDivide"))
                            {
                                OptionsInPlottingMode.nutnDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfCalForceDivide.Text = OptionsInPlottingMode.nutnDivide.ToString();
                                //plotting.OptionsPlotting.tfCalForceDivide.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfCalForceDivide.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("nutnMultiple"))
                            {
                                OptionsInPlottingMode.nutnMultiple = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfCalForceMultiple.Text = OptionsInPlottingMode.nutnMultiple.ToString();
                                //plotting.OptionsPlotting.tfCalForceMultiple.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfCalForceMultiple.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("mmDivide"))
                            {
                                OptionsInPlottingMode.mmDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfCalElonDivide.Text = OptionsInPlottingMode.mmDivide.ToString();
                                //plotting.OptionsPlotting.tfCalElonDivide.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfCalElonDivide.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("mmCoeff"))
                            {
                                OptionsInPlottingMode.mmCoeff = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfCalElonMultiple.Text = OptionsInPlottingMode.mmCoeff.ToString();
                                //plotting.OptionsPlotting.tfCalElonMultiple.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfCalElonMultiple.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("mmDivideWithEkstenziometer"))
                            {
                                OptionsInPlottingMode.mmDivideWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfCalElonDivide2.Text = OptionsInPlottingMode.mmDivideWithEkstenziometer.ToString();
                                //plotting.OptionsPlotting.tfCalElonDivide2.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfCalElonDivide2.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("mmCoeffWithEkstenziometer"))
                            {
                                OptionsInPlottingMode.mmCoeffWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfCalElonMultiple2.Text = OptionsInPlottingMode.mmCoeffWithEkstenziometer.ToString();
                                //plotting.OptionsPlotting.tfCalElonMultiple2.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfCalElonMultiple2.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("isAutoChecked"))
                            {
                                string isAuto = textReader.ReadElementContentAsString();
                                if (isAuto.Equals("True"))
                                {
                                    OptionsInPlottingMode.isAutoChecked = true;

                                    OptionsInPlottingMode.isManualChecked = false;

                                    OptionsInPlottingMode.xRange = 0.95;
                                    OptionsInPlottingMode.yRange = 0.95;
                                }
                                if (isAuto.Equals("False"))
                                {
                                    OptionsInPlottingMode.isAutoChecked = false;

                                    OptionsInPlottingMode.isManualChecked = true;
                                }
                            }
                            if (textReader.Name.Equals("isManualChecked"))
                            {
                                string isManual = textReader.ReadElementContentAsString();
                                if (isManual.Equals("True"))
                                {
                                    OptionsInPlottingMode.isManualChecked = true;
                                }
                                if (isManual.Equals("False"))
                                {
                                    OptionsInPlottingMode.isManualChecked = false;


                                    OptionsInPlottingMode.xRange = 0.95;
                                    OptionsInPlottingMode.yRange = 0.95;
                                }
                            }


                            if (textReader.Name.Equals("PrikaziOriginalAfterRatioChanging"))
                            {
                                string PrikaziOriginalAfterRatioChanging = textReader.ReadElementContentAsString();
                                if (PrikaziOriginalAfterRatioChanging.Equals("True"))
                                {
                                    OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging = true;
                                }
                                if (PrikaziOriginalAfterRatioChanging.Equals("False"))
                                {
                                    OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging = false;
                                }
                            }
                            if (textReader.Name.Equals("PrikaziFitovaniAfterRatioChanging"))
                            {
                                string PrikaziFitovaniAfterRatioChanging = textReader.ReadElementContentAsString();
                                if (PrikaziFitovaniAfterRatioChanging.Equals("True"))
                                {
                                    OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging = true;
                                }
                                if (PrikaziFitovaniAfterRatioChanging.Equals("False"))
                                {
                                    OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging = false;
                                }
                            }



                            if (textReader.Name.Equals("ratioElongation"))
                            {
                                OptionsInPlottingMode.xRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.tfRatioElongation.Text = OptionsInPlottingMode.xRange.ToString();
                                plotting.tfRatioElongation.Foreground = Brushes.Black;
                                //plotting.tfRatioElongation.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("ratioForce"))
                            {
                                OptionsInPlottingMode.yRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.tfRatioForce.Text = OptionsInPlottingMode.yRange.ToString();
                                plotting.tfRatioForce.Foreground = Brushes.Black;
                                //plotting.tfRatioForce.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("filePath"))
                            {
                                OptionsInPlottingMode.filePath = textReader.ReadElementContentAsString();
                                plotting.tfFilepathPlotting.Text = OptionsInPlottingMode.filePath;
                                plotting.tfFilepathPlotting.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("isFittingChecked"))
                            {
                                string temp = textReader.ReadElementContentAsString();
                                if (temp.Equals("True"))
                                {
                                    OptionsInPlottingMode.isFittingChecked = true;
                                }
                                if (temp.Equals("False"))
                                {
                                    OptionsInPlottingMode.isFittingChecked = false;
                                }
                            }
                            if (textReader.Name.Equals("isAutoFittingChecked"))
                            {
                                string temp = textReader.ReadElementContentAsString();
                                if (temp.Equals("True"))
                                {
                                    OptionsInPlottingMode.isAutoFittingChecked = true;
                                    OptionsInPlottingMode.isManualFittingChecked = false;
                                }
                                if (temp.Equals("False"))
                                {
                                    OptionsInPlottingMode.isAutoFittingChecked = false;
                                    OptionsInPlottingMode.isManualFittingChecked = true;
                                }
                            }
                            //if (textReader.Name.Equals("isManualFittingChecked"))
                            //{
                            //    string temp = textReader.ReadElementContentAsString();
                            //    if (temp.Equals("True"))
                            //    {
                            //        OptionsInPlottingMode.isAutoFittingChecked = false;
                            //        OptionsInPlottingMode.isManualFittingChecked = true;
                            //    }
                            //    if (temp.Equals("False"))
                            //    {
                            //        OptionsInPlottingMode.isAutoFittingChecked = true;
                            //        OptionsInPlottingMode.isManualFittingChecked = false;
                            //    }
                            //}
                            if (textReader.Name.Equals("isManualFittingChecked"))
                            {
                                OptionsInPlottingMode.isManualFittingChecked = true;
                            }
                            if (textReader.Name.Equals("pointCrossheadX"))
                            {
                                OptionsInPlottingMode.pointCrossheadX = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX,1);
                                plotting.tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                                plotting.tffittingCrossheadPointX.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("pointCrossheadY"))
                            {
                                OptionsInPlottingMode.pointCrossheadY = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY,0);
                                plotting.tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();
                                plotting.tffittingCrossheadPointY.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("procentAuto1"))
                            {
                                OptionsInPlottingMode.procentAuto1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tffittingAutoProcent1.Text = OptionsInPlottingMode.procentAuto1.ToString();
                                //plotting.OptionsPlotting.tffittingAutoProcent1.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tffittingAutoProcent1.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("procentAuto2"))
                            {
                                OptionsInPlottingMode.procentAuto2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tffittingAutoProcent2.Text = OptionsInPlottingMode.procentAuto2.ToString();
                                //plotting.OptionsPlotting.tffittingAutoProcent2.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tffittingAutoProcent2.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("procentAuto3"))
                            {
                                OptionsInPlottingMode.procentAuto3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tffittingAutoProcent3.Text = OptionsInPlottingMode.procentAuto3.ToString();
                                //plotting.OptionsPlotting.tffittingAutoProcent3.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tffittingAutoProcent3.Foreground = Brushes.White;

                            if (textReader.Name.Equals("pointManualX1"))
                            {
                                OptionsInPlottingMode.pointManualX1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualX1 = Math.Round(OptionsInPlottingMode.pointManualX1,6);
                                plotting.tffittingManPoint1X.Text = OptionsInPlottingMode.pointManualX1.ToString();
                                plotting.tffittingManPoint1X.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("pointManualY1"))
                            {
                                OptionsInPlottingMode.pointManualY1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualY1 = Math.Round(OptionsInPlottingMode.pointManualY1,0);
                                plotting.tffittingManPoint1Y.Text = OptionsInPlottingMode.pointManualY1.ToString();
                                plotting.tffittingManPoint1Y.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("pointManualX2"))
                            {
                                OptionsInPlottingMode.pointManualX2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualX2 = Math.Round(OptionsInPlottingMode.pointManualX2,6);
                                plotting.tffittingManPoint2X.Text = OptionsInPlottingMode.pointManualX2.ToString();
                                plotting.tffittingManPoint2X.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("pointManualY2"))
                            {
                                OptionsInPlottingMode.pointManualY2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualY2 = Math.Round(OptionsInPlottingMode.pointManualY2,0);
                                plotting.tffittingManPoint2Y.Text = OptionsInPlottingMode.pointManualY2.ToString();
                                plotting.tffittingManPoint2Y.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("pointManualX3"))
                            {
                                OptionsInPlottingMode.pointManualX3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3,6);
                                plotting.tffittingManPoint3X.Text = OptionsInPlottingMode.pointManualX3.ToString();
                                plotting.tffittingManPoint3X.Foreground = Brushes.Black;
                            }
                            if (textReader.Name.Equals("pointManualY3"))
                            {
                                OptionsInPlottingMode.pointManualY3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3,0);
                                plotting.tffittingManPoint3Y.Text = OptionsInPlottingMode.pointManualY3.ToString();
                                plotting.tffittingManPoint3Y.Foreground = Brushes.Black;
                            }
                           
                            
                           
                            }
                            if (textReader.Name.Equals("isShowOriginalDataGraphic"))
                            {
                                string temp = textReader.ReadElementContentAsString();
                                if (temp.Equals("True"))
                                {
                                    OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                                }
                                if (temp.Equals("False"))
                                {
                                    OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                                }
                            }

                            if (textReader.Name.Equals("fittingAutoMaxXValue"))
                            {
                                OptionsInPlottingMode.fittingAutoMaxXValue = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfFittingAutoMaxXValue.Text = OptionsInPlottingMode.fittingAutoMaxXValue.ToString();
                                //plotting.OptionsPlotting.tfFittingAutoMaxXValue.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfFittingAutoMaxXValue.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("ReHXRange"))
                            {
                                OptionsInPlottingMode.ReHXRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfReHXRange.Text = OptionsInPlottingMode.ReHXRange.ToString();
                                //plotting.OptionsPlotting.tfReHXRange.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfReHXRange.Foreground = Brushes.White;
                            }
                            
                            if (textReader.Name.Equals("YieldInterval"))
                            {
                                OptionsInPlottingMode.YieldInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfYield.Text = OptionsInPlottingMode.YieldInterval.ToString();
                                //plotting.OptionsPlotting.tfYield.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfYield.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("MaxSubBetweenReLAndReLF"))
                            {
                                OptionsInPlottingMode.MaxSubBetweenReLAndReLF = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfMaxSubsReLFReL.Text = OptionsInPlottingMode.MaxSubBetweenReLAndReLF.ToString();
                                //plotting.OptionsPlotting.tfMaxSubsReLFReL.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfMaxSubsReLFReL.Foreground = Brushes.White;
                            }

                            //if (textReader.Name.Equals("MinSubBetweenReLAndRpmax"))
                            //{
                            //    OptionsInPlottingMode.MinSubBetweenReLAndRpmax = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            //    plotting.tfMinSubsReLRpmax.Text = OptionsInPlottingMode.MinSubBetweenReLAndRpmax.ToString();
                            //    plotting.tfMinSubsReLRpmax.Foreground = Brushes.Black;
                            //}

                            if (textReader.Name.Equals("RationReLRpmaxForOnlyReL"))
                            {
                                OptionsInPlottingMode.RationReLRpmaxForOnlyReL = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfRationReLRpmaxForOnlyReL.Text = OptionsInPlottingMode.RationReLRpmaxForOnlyReL.ToString();
                                //plotting.OptionsPlotting.tfRationReLRpmaxForOnlyReL.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfRationReLRpmaxForOnlyReL.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("MinPossibleValueForOnlyReLElongationInProcent"))
                            {
                                OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = Math.Round(OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent, 1);
                                plotting.OptionsPlotting.tfXReLForOnlyReL.Text = OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent.ToString();
                                //plotting.OptionsPlotting.tfXReLForOnlyReL.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfXReLForOnlyReL.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("OnlyReLPreassureUnit"))
                            {
                                OptionsInPlottingMode.OnlyReLPreassureUnit = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfOnlyReLPreassureUnit.Text = OptionsInPlottingMode.OnlyReLPreassureUnit.ToString();
                                //plotting.OptionsPlotting.tfOnlyReLPreassureUnit.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfOnlyReLPreassureUnit.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("currK"))
                            {
                                plotting.CurrK = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            }

                            if (textReader.Name.Equals("currN"))
                            {
                                plotting.CurrN = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            }

                            if (textReader.Name.Equals("UkljuciNepodrazumevaniModKidanja"))
                            {
                                string temp = textReader.ReadElementContentAsString();
                                if (temp.Equals("True"))
                                {
                                    OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja = true;
                                }
                                if (temp.Equals("False"))
                                {
                                    OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja = false;
                                }
                            }

                            if (textReader.Name.Equals("TearingPointCoeff"))
                            {
                                OptionsInPlottingMode.TearingPointCoeff = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfTearingPointCoeff.Text = OptionsInPlottingMode.TearingPointCoeff.ToString();
                                //plotting.OptionsPlotting.tfTearingPointCoeff.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfTearingPointCoeff.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("TearingMinFallPreassure"))
                            {
                                OptionsInPlottingMode.TearingMinFallPreassure = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfTearingMinFallPreassure.Text = OptionsInPlottingMode.TearingMinFallPreassure.ToString();
                                //plotting.OptionsPlotting.tfTearingMinFallPreassure.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfTearingMinFallPreassure.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("ResolutionForTearing"))
                            {
                                OptionsInPlottingMode.ResolutionForTearing = textReader.ReadElementContentAsInt();
                                plotting.OptionsPlotting.tfResolutionForTearing.Text = OptionsInPlottingMode.ResolutionForTearing.ToString();
                                //plotting.OptionsPlotting.tfResolutionForTearing.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfResolutionForTearing.Foreground = Brushes.White;
                            }


                            if (textReader.Name.Equals("DefaultPreassureOfTearingInProcent"))
                            {
                                OptionsInPlottingMode.DefaultPreassureOfTearingInProcent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfDefaultPreassureOfTearing.Text = OptionsInPlottingMode.DefaultPreassureOfTearingInProcent.ToString();
                                //plotting.OptionsPlotting.tfDefaultPreassureOfTearing.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfDefaultPreassureOfTearing.Foreground = Brushes.White;
                            }

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


                            if (textReader.Name.Equals("BeginIntervalForN"))
                            {
                                OptionsInPlottingMode.BeginIntervalForN = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfBeginIntervalForN.Text = OptionsInPlottingMode.BeginIntervalForN.ToString();
                                //plotting.OptionsPlotting.tfBeginIntervalForN.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfBeginIntervalForN.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("EndIntervalForN"))
                            {
                                OptionsInPlottingMode.EndIntervalForN = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsPlotting.tfEndIntervalForN.Text = OptionsInPlottingMode.EndIntervalForN.ToString();
                                //plotting.OptionsPlotting.tfEndIntervalForN.Foreground = Brushes.Black;
                                plotting.OptionsPlotting.tfEndIntervalForN.Foreground = Brushes.White;
                            }



                            if (textReader.Name.Equals("YungXelas"))
                            {
                                OptionsInPlottingMode.ReEqualsRp = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsForYungsModuo.tfXelas.Text = OptionsInPlottingMode.ReEqualsRp.ToString();
                                //plotting.OptionsForYungsModuo.tfXelas.Foreground = Brushes.Black;
                                plotting.OptionsForYungsModuo.tfXelas.Foreground = Brushes.White;
                            }

                            if (textReader.Name.Equals("YungPrSpustanja"))
                            {
                                OptionsInPlottingMode.YungPrSpustanja = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                plotting.OptionsForYungsModuo.tfprocspustanja.Text = OptionsInPlottingMode.YungPrSpustanja.ToString();
                                //this.OptionsForYungsModuo.tfprocspustanja.Foreground = Brushes.Black;
                                plotting.OptionsForYungsModuo.tfprocspustanja.Foreground = Brushes.White;
                            }
                                
                        }
                    }//end of while loop

                    textReader.Close();
                    plotting.createOfflineGraphics();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {public void LoadPlottingoptions()}", System.DateTime.Now);

            }
        }

        /// <summary>
        /// ucitava opcije koje su zapamcene zadnje u animacionom modu
        /// svodi se na citanje xml fajla na putanji System.Environment.CurrentDirectory + "\\configuration\\animationModeOptions.xml", koja se trenutno nalazi u kodu same metode
        /// i popunjavanje strukture OptionsInAnimation, 
        /// pa se na osnovu sadrzaja strukture OptionsInAnimation popunjava prozor [Options]OptionsAnimation.xaml
        /// </summary>
        private void LoadAnimationoptions()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(System.Environment.CurrentDirectory + "\\configuration\\animationModeOptions.xml");

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {
                        if (textReader.Name.Equals("isContinuousDisplay"))
                        {
                            string temp = textReader.ReadElementContentAsString();
                            if (temp.Equals("True"))
                            {
                                OptionsInAnimation.isContinuousDisplay = true;
                            }
                            if (temp.Equals("False"))
                            {
                                OptionsInAnimation.isContinuousDisplay = false;
                            }

                        }
                        if (textReader.Name.Equals("isDiscreteDisplay"))
                        {
                            string temp = textReader.ReadElementContentAsString();
                            if (temp.Equals("True"))
                            {
                                OptionsInAnimation.isDiscreteDisplay = true;
                            }
                            if (temp.Equals("False"))
                            {
                                OptionsInAnimation.isDiscreteDisplay = false;
                            }
                        }
                        if (textReader.Name.Equals("refreshTimeInterval"))
                        {
                            OptionsInAnimation.refreshTimeInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("conResolution"))
                        {
                            OptionsInAnimation.conResolution = textReader.ReadElementContentAsInt();
                        }
                        if (textReader.Name.Equals("disResolution"))
                        {
                            OptionsInAnimation.disResolution = textReader.ReadElementContentAsInt();
                        }



                        if (textReader.Name.Equals("nutnDivide"))
                        {
                            OptionsInAnimation.nutnDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("nutnMultiple"))
                        {
                            OptionsInAnimation.nutnMultiple = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("mmDivide"))
                        {
                            OptionsInAnimation.mmDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("mmCoeff"))
                        {
                            OptionsInAnimation.mmCoeff = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("filePath"))
                        {
                            OptionsInAnimation.filePath = textReader.ReadElementContentAsString();
                        }
                    }
                }//end of while loop
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {private void LoadAnimationoptions()}", System.DateTime.Now);
            }
        }

        public TabItem Tab_firstInner
        {
            get { return tab_firstInner; }
        }

        private double xconst = 10;
        private double yconst = 10;
        /// <summary>
        /// pri pokretanju nista nije ucitano u izlaznim podacima
        /// tek kada se prvi put klikne na tab2 dolazi na ucitavanje zadnje pokidanog uzorka
        /// </summary>
        private bool isSecondTabWasClicked = false;
        /// <summary>
        /// pri pokretanju nista nije ucitano u izlaznim podacima
        /// tek kada se prvi put klikne na tab2 dolazi na ucitavanje zadnje pokidanog uzorka
        /// </summary>
        public bool IsSecondTabWasClicked
        {
            get { return isSecondTabWasClicked; }
            set { isSecondTabWasClicked = value; }
        }

        private void createAllSettingsFile() 
        {
            try
            {
                FileInfo fiOnlineFilePath = new FileInfo(Properties.Settings.Default.onlineFilepath);
                if (Directory.Exists(fiOnlineFilePath.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiOnlineFilePath.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.onlineFilepath) == false)
                {
                    File.Create(Properties.Settings.Default.onlineFilepath);
                }

                FileInfo fiE2e4Filepath = new FileInfo(Properties.Settings.Default.e2e4Filepath);
                if (Directory.Exists(fiE2e4Filepath.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiE2e4Filepath.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.e2e4Filepath) == false)
                {
                    File.Create(Properties.Settings.Default.e2e4Filepath);
                }

                FileInfo fiInputOutputFilepath = new FileInfo(Properties.Settings.Default.inputOutputFilepath);
                if (Directory.Exists(fiInputOutputFilepath.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiInputOutputFilepath.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.inputOutputFilepath) == false)
                {
                    File.Create(Properties.Settings.Default.inputOutputFilepath);
                }

                FileInfo fiSampleReportFilepath = new FileInfo(Properties.Settings.Default.sampleReportFilepath);
                if (Directory.Exists(fiSampleReportFilepath.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSampleReportFilepath.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.sampleReportFilepath) == false)
                {
                    File.Create(Properties.Settings.Default.sampleReportFilepath);
                }

                FileInfo fiSampleReportGraphicFilepath = new FileInfo(Properties.Settings.Default.sampleReportGraphicFilepath);
                if (Directory.Exists(fiSampleReportGraphicFilepath.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSampleReportGraphicFilepath.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.sampleReportGraphicFilepath) == false)
                {
                    File.Create(Properties.Settings.Default.sampleReportGraphicFilepath);
                }


                FileInfo fiSampleReportGraphicFilepathChangeOfR = new FileInfo(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR);
                if (Directory.Exists(fiSampleReportGraphicFilepathChangeOfR.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSampleReportGraphicFilepathChangeOfR.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR) == false)
                {
                    File.Create(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR);
                }

                FileInfo fiSampleReportGraphicFilepathChangeOfE = new FileInfo(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE);
                if (Directory.Exists(fiSampleReportGraphicFilepathChangeOfE.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSampleReportGraphicFilepathChangeOfE.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE) == false)
                {
                    File.Create(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE);
                }

                FileInfo fiUnsavedFilepath = new FileInfo(Properties.Settings.Default.unsavedFilepath);
                if (Directory.Exists(fiUnsavedFilepath.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiUnsavedFilepath.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.unsavedFilepath) == false)
                {
                    File.Create(Properties.Settings.Default.unsavedFilepath);
                }


                FileInfo fiUnsavedFilepathPreassureElongation = new FileInfo(Properties.Settings.Default.unsavedFilepathPreassureElongation);
                if (Directory.Exists(fiUnsavedFilepathPreassureElongation.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiUnsavedFilepathPreassureElongation.DirectoryName);
                }
                if (File.Exists(Properties.Settings.Default.unsavedFilepathPreassureElongation) == false)
                {
                    File.Create(Properties.Settings.Default.unsavedFilepathPreassureElongation);
                }



                FileInfo fiSumReportDir = new FileInfo(Properties.Settings.Default.SumReportDir);
                if (Directory.Exists(fiSumReportDir.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSumReportDir.DirectoryName);
                }

                FileInfo fiSaveDirectory = new FileInfo(Properties.Settings.Default.SaveDirectory);
                if (Directory.Exists(fiSaveDirectory.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSaveDirectory.DirectoryName);
                }

                FileInfo fiSaveDirectoryForReports = new FileInfo(Properties.Settings.Default.SaveDirectoryForReports);
                if (Directory.Exists(fiSaveDirectoryForReports.DirectoryName) == false)
                {
                    Directory.CreateDirectory(fiSaveDirectoryForReports.DirectoryName);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {private void createAllSettingsFile()}", System.DateTime.Now);
            }
        }

        private void loadXMLFileForceRangesCurrent()
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

                        if (textReader.Name.Equals("forceLowerCurrent"))
                        {
                            ForceRangesOptions.forceLowerCurrent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("forceUpperCurrent"))
                        {
                            ForceRangesOptions.forceUpperCurrent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
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

        FileInfo[] Files;

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                loadXMLFileForceRangesCurrent();
                FooterOptions.LoadOptionsFooteriNITIAL();

                Constants.onlineFilepath = Properties.Settings.Default.onlineFilepath;
                Constants.e2e4Filepath = Properties.Settings.Default.e2e4Filepath;
                Constants.inputOutputFilepath = Properties.Settings.Default.inputOutputFilepath;
                Constants.sampleReportFilepath = Properties.Settings.Default.sampleReportFilepath;
                Constants.sampleReportGraphicFilepath = Properties.Settings.Default.sampleReportGraphicFilepath;
                Constants.sampleReportGraphicFilepathChangeOfR = Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR;
                Constants.sampleReportGraphicFilepathChangeOfE = Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE;
                Constants.unsavedFilepath = Properties.Settings.Default.unsavedFilepath;

                //this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.95);
                //this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.95);
                //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                //this.WindowStartupLocation = WindowStartupLocation.Manual;
                //this.Left = xconst;
                //this.Top = yconst;
                Logger.CreateLogFile();
                Logger.WriteNode("Aplikacija ukljucena", System.DateTime.Now);

                createAllSettingsFile();
                
                DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.SaveDirectory);//Assuming Test is your Folder
                Files = d.GetFiles("*LuManual.pe"); //Getting Text files
                foreach (var file in Files)
                {
                    File.Delete(file.FullName);
                }

                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                this.SourceInitialized += (s, a) => this.WindowState = WindowState.Maximized;
                //this.Show();

                //hardverski mod
                onlineMode = new OnlineMode(false);
                //simulacioni mode
                //onlineMode = new OnlineMode(true);

                tab_first.Content = onlineMode;
                plotting = new GraphicPlotting(onlineMode);
                tab_firstInner.Content = plotting;
                onlineMode.Plotting = plotting;

                animation = new GraphicAnimation();
                tab_secondInner.Content = animation;

                printScreen = new PrintScreen(plotting);
                tab_third.Content = printScreen;
                plotting.Printscreen = printScreen;
                printScreen.chbChangeOfRAndE.IsEnabled = false;

                //kada se pokrene aplikacija prvo se prikazuje print screen prozor
                tab_third.IsSelected = true;

                //proveri na pokretanju programa koji je pre iskljucenja izduzenje korisceno
                isEkstenziometerUsedInitial();

                //ucitaj zadnje izvrseno kidanje
                isSecondTabWasClicked = true;
                plotting.setRadioButtons();
                plotting.setFittingCheckbox();
                if (firstImeClicked == false)
                {
                    if (isOnlineModeFinished == false)
                    {
                        plotting.btnPlottingModeClick();
                        plotting.btnPlottingModeClick();
                        //if (currentFileNotExist == true)
                        //{

                        //}
                        //else
                        //{
                        //    plotting.DrawFittingGraphic();
                        //}
                        //ukloni oroginalan grafik iz offline moda ako je odcekirana opcija za prikazivanje originalnog grafika
                        if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                        {
                            plotting.DeleteOfflineModeOnly();
                        }
                    }
                    firstImeClicked = true;
                }

                //i postavi sve ostale parametre
                this.printScreen.btnSampleDataPrintMode.IsDefault = true;
                this.printScreen.btnSampleDataPrintMode.Focus();
                if (plotting.IsLuManualChanged == false)
                {
                    if (printScreen.IsPrintScreenEmpty == false)
                    {
                        this.printScreen.setMarkers();
                    }
                }




                OptionsInOnlineMode.isManualChecked = true;

                //ovo ide da bi se lepo videle nule na online grafiku po pokretanju aplikacije
                onlineMode.CreateOnlineGraphicsInitial();

                //ovo ide jos jedanput pri pokretamnju aplikacije
                //plotting.btnPlottingModeClick();
                //if (currentFileNotExist == true)
                //{

                //}
                //else
                //{
                //    plotting.DrawFittingGraphic();
                //}
                //ukloni oroginalan grafik iz offline moda ako je odcekirana opcija za prikazivanje originalnog grafika
                //if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                //{
                //plotting.DeleteOfflineModeOnly();
                //}
                //LoadPlottingoptions();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {public MainWindow()}", System.DateTime.Now);
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                onlineMode.WriteXMLLastResultsInterface();

                string temp = LastInputOutputSavedData.tfBrUzorka_GeneralData;
                temp = temp.Replace('/', '_');
                string path = Properties.Settings.Default.SaveDirectory + temp + ".pdf";
                if (File.Exists(path) == false)
                {
                    MessageBox.Show("Niste zapamtili zadnje ispitivani uzorak !");


                    //ako nije zapamcen poslednje pokidan uzorak pitaj korisnika da li zeli da ga zapamti
                    if (onlineMode.IsCurrentSampleSaved == false)
                    {

                        MessageOnlineModeYesNo mYesNo = new MessageOnlineModeYesNo();
                        mYesNo.ShowDialog();
                        if (mYesNo.IsYesChosen)
                        {
                            bool isSavedChangeRandE = false;
                            if (plotting.Printscreen.chbChangeOfRAndE.IsChecked == true)
                            {
                                isSavedChangeRandE = true;
                            }
                            else
                            {
                                isSavedChangeRandE = false;
                            }
                            bool isManualNCalculated = false;
                            if (plotting.Printscreen.chbCalculateNManual.IsChecked == true)
                            {
                                isManualNCalculated = true;
                            }
                            else
                            {
                                isManualNCalculated = false;
                            }
                            SaveDialogForm saveDialog = new SaveDialogForm(isSavedChangeRandE, isManualNCalculated, this.Plotting.Printscreen);
                            onlineMode.SaveDialogProperty = saveDialog;
                            string currFileNamePath = saveDialog.saveFileDialog1.FileName;


                            if (onlineMode.SaveDialogProperty.IsClickedToSaveFile == true)
                            {
                                this.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                                this.Plotting.tfFilepathPlottingKeyDown();
                                //ispod linija ima smisla samo kada se nastavlja sa radom programa
                                //kada se program zatvara informacija o ovoj promenljivoj {onlineMode.IsCurrentSampleSaved} vise nije potrebna pa se i ne postavlja
                                //onlineMode.IsCurrentSampleSaved = true;
                                //da bi se proracunalo sve
                                this.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                                this.tab_second.IsSelected = true;
                            }
                        }
                    }

                }

                Logger.WriteNode("Aplikacija iskljucena", System.DateTime.Now);
                System.Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
            
        }

        public bool IWantLoadYungFromFile = true;

        /// <summary>
        /// ako jos uvek nije bilo kliknuto na drugi tab firstImeClicked je false je Clicked asocira na proslo vreme tj klikuto je prvi put na drugi tab 
        /// </summary>
        private bool firstImeClicked = false;//samo jednom produciraj klik na dugme ucitaj tekuci grafik
        /// <summary>
        /// readonly property
        /// </summary>
        public bool FirstImeClicked
        {
            get { return firstImeClicked; }
            set { firstImeClicked = value; }
        }

      

       
        private void tabcontrol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //ovo mora biti private clan
                //int lastSelectedTab = 0;

                if (tab_first.IsSelected)
                {
                    //ako je u tabu 2 kliknuta Animacija cim kliknes na neki drugi tab 
                    //koji nije Analiza dijagrama ti odmat u tabu 2 vrati 
                    //da je selektovani podtab zapravo prvi podtab, pa kad korisnik ponovo klikne na rab 2 da mu se pojavi prvi podtab selektovan 
                    tab_firstInner.IsSelected = true;
                    onlineMode.btnStartSample.Foreground = Brushes.White;
                    onlineMode.btnStartSample.Background = Brushes.LawnGreen;
                }
                if (tab_second.IsSelected)
                {
                    if (onlineMode.IsLessThanOne == true)
                    {
                        MessageBox.Show("Isuvise malo izduzenje za analizu.");
                        this.tab_third.IsSelected = true;
                        return;
                    }
                    if (printScreen.IsPrintScreenEmpty == true)
                    {
                        plotting.SimulateTabPressFortfFilepathPlotting(); plotting.SimulateTabPressFortfFilepathPlotting();


                        if (onlineMode.OnHeader != null)
                        {
                            onlineMode.OnHeader.Close();
                        }

                        printScreen.btnSampleDataPrintMode.IsEnabled = true;
                        printScreen.IsPrintScreenEmpty = false;
                    }
                    plotting.btnPlottingModeClick();
                    plotting.btnPlottingModeClick();
                    ////u ovoj thrn grani prelazimo na prozor koji se nalazi u offline modu
                    //isSecondTabWasClicked = true;
                    //plotting.setRadioButtons();
                    //plotting.setFittingCheckbox();
                    //if (firstImeClicked == false)
                    //{
                    //    if (isOnlineModeFinished == false)
                    //    {
                    //        plotting.btnPlottingModeClick();
                    //        //if (currentFileNotExist == true)
                    //        //{

                    //        //}
                    //        //else
                    //        //{
                    //        //    plotting.DrawFittingGraphic();
                    //        //}
                    //        //ukloni oroginalan grafik iz offline moda ako je odcekirana opcija za prikazivanje originalnog grafika
                    //        if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                    //        {
                    //            plotting.DeleteOfflineModeOnly();
                    //        }
                    //    }
                    //    firstImeClicked = true;
                    //}
                    //plotting.setPointAtGraphicY1(OptionsInPlottingMode.pointManualY1);
                    //plotting.setPointAtGraphicY2(OptionsInPlottingMode.pointManualY2);
                    //plotting.setPointAtGraphicY3(OptionsInPlottingMode.pointManualY3);
                }

                if (tab_third.IsSelected)
                {


                    //ako je u tabu 2 kliknuta Animacija cim kliknes na neki drugi tab 
                    //koji nije Analiza dijagrama ti odmat u tabu 2 vrati 
                    //da je selektovani podtab zapravo prvi podtab, pa kad korisnik ponovo klikne na rab 2 da mu se pojavi prvi podtab selektovan 
                    tab_firstInner.IsSelected = true;
                    //this.printScreen.btnSampleDataPrintMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    this.printScreen.btnSampleDataPrintMode.IsDefault = true;
                    this.printScreen.btnSampleDataPrintMode.Focus();
                    if (plotting.IsLuManualChanged == false)
                    {
                        if (printScreen.IsPrintScreenEmpty == false)
                        {
                            this.printScreen.setMarkers();
                        }
                    }

                    //ako je u analizi dijagrama razmera postavljena na automatsku, klikom na treci tab postavi na rucno programskim putem
                    if (this.plotting != null)
                    {
                        this.plotting.SetManualGraphicRatio();
                    }


                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MainWindow.xaml.cs] {private void tabcontrol_SelectionChanged(object sender, SelectionChangedEventArgs e)}", System.DateTime.Now);
            }

         }

       

    }
}