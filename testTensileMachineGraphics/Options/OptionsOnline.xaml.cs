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
using System.Xml;
using testTensileMachineGraphics.OnlineModeFolder;
using System.Xml.Linq;

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for OptionsOnline.xaml
    /// </summary>
    public partial class OptionsOnline : Window
    {
        public static bool isCreatedOptionsOnline = false;
        
        

        private OnlineMode _onlineMode;

        public OnlineMode OnlineModeInstance 
        {
            set 
            {
                if (value != null)
                {
                    _onlineMode = value;
                }
            }
        }



        private double xconst = 0.7 * 1000;
        private double yconst = 65;


      

        public OptionsOnline(OnlineMode onmode)
        {
            try
            {
                InitializeComponent();
                _onlineMode = onmode;
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                //this.WindowStartupLocation = WindowStartupLocation.Manual;
                //this.Left = xconst;
                //this.Top = yconst;

                setComboBox();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {public OptionsOnline(OnlineMode onmode)}", System.DateTime.Now);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                isCreatedOptionsOnline = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

        private void setComboBox() 
        {
            try
            {
                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 200)
                {
                    cmbI200.IsSelected = true;
                }
                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 400)
                {
                    cmbI400.IsSelected = true;
                }
                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 600)
                {
                    cmbI600.IsSelected = true;
                }
                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 800)
                {
                    cmbI800.IsSelected = true;
                }
                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 1000)
                {
                    cmbI1000.IsSelected = true;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void setComboBox()}", System.DateTime.Now);
            }
        }


        public void setCheckboxes()
        {
            try
            {
                if (OptionsInOnlineMode.calculateMaxForceForTf)
                {
                    this.chbShowMaxForce.IsChecked = true;
                }
                else
                {
                    this.chbShowMaxForce.IsChecked = false;
                }

                if (OptionsInOnlineMode.isCalibration)
                {
                    this.chbIsCalibration.IsChecked = true;
                }
                else
                {
                    this.chbIsCalibration.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString() + " {setCheckboxes}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {public void setCheckboxes()}", System.DateTime.Now);
            }
        }

        #region optionsOnlineMode

        private void markSavedOnlineOptionsAsBlack()
        {
            try
            {
                //tfCalForceDivide.Foreground = Brushes.Black;
                //tfCalForceMultiple.Foreground = Brushes.Black;
                //tfCalElonDivide.Foreground = Brushes.Black;
                //tfCalElonMultiple.Foreground = Brushes.Black;

                //tfEndOnlineWrite.Foreground = Brushes.Black;

                tfCalForceDivide.Foreground = Brushes.White;
                tfCalForceMultiple.Foreground = Brushes.White;
                tfCalElonDivide.Foreground = Brushes.White;
                tfCalElonMultiple.Foreground = Brushes.White;

                tfEndOnlineWrite.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlack()}", System.DateTime.Now);
            }
        }




     

        public void WriteXMLOnlineFile()
        {
            try
            {
                writeXMLFile();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {public void WriteXMLOnlineFile()}", System.DateTime.Now);
            }
        }

        private void writeXMLFile()
        {
            try
            {
                //write in xml file

                XElement xmlRoot = new XElement("OnlineMode",
                                                new XElement("OnlineModeCurrentOptions",
                                                            new XElement("refreshTimeInterval", OptionsInOnlineMode.refreshTimeInterval.ToString()),
                                                            new XElement("Resolution", OptionsInOnlineMode.Resolution.ToString()),
                                                            new XElement("L0", OptionsInOnlineMode.L0.ToString()),
                                                            new XElement("S0", OptionsInOnlineMode.S0.ToString()),
                                                            new XElement("nutnDivide", OptionsInOnlineMode.nutnDivide.ToString()),
                                                            new XElement("nutnMultiple", OptionsInOnlineMode.nutnMultiple.ToString()),
                                                            new XElement("mmDivide", OptionsInOnlineMode.mmDivide.ToString()),
                                                            new XElement("mmCoeff", OptionsInOnlineMode.mmCoeff.ToString()),
                                                            new XElement("mmDivideWithEkstenziometer", OptionsInOnlineMode.mmDivideWithEkstenziometer.ToString()),
                                                            new XElement("mmCoeffWithEkstenziometer", OptionsInOnlineMode.mmCoeffWithEkstenziometer.ToString()),
                                                            new XElement("isAutoChecked", OptionsInOnlineMode.isAutoChecked.ToString()),
                                                            new XElement("isManualChecked", OptionsInOnlineMode.isManualChecked.ToString()),
                                                            new XElement("ratioElongation", OptionsInOnlineMode.xRange.ToString()),
                                                            new XElement("ratioForce", OptionsInOnlineMode.yRange.ToString()),
                                                            new XElement("onlineWriteEndTimeInterval", OptionsInOnlineMode.onlineWriteEndTimeInterval.ToString()),
                                                            new XElement("calculateMaxForceForTf", OptionsInOnlineMode.calculateMaxForceForTf.ToString()),
                                                            new XElement("isCalibration", OptionsInOnlineMode.isCalibration.ToString()),
                                                            new XElement("timeIntervalForCalculationOfChangedParameters", OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters.ToString()),
                                                            new XElement("isE2E4BorderSelected", OptionsInOnlineMode.isE2E4BorderSelected.ToString()),
                                                            new XElement("E2E4Border", OptionsInOnlineMode.E2E4Border.ToString()),
                                                            new XElement("E3E4Border", OptionsInOnlineMode.E3E4Border.ToString()),
                                                            new XElement("COM", OptionsInOnlineMode.COM.ToString())
                                                            )
                                                );

                xmlRoot.Save(Constants.onlineModeOptionsXml);
                //using (XmlWriter writer = XmlWriter.Create(Constants.onlineModeOptionsXml))
                //{
                //    writer.WriteStartDocument();
                //    writer.WriteStartElement("OnlineMode");


                //    writer.WriteStartElement("OnlineModeCurrentOptions");


                //    writer.WriteElementString("refreshTimeInterval", OptionsInOnlineMode.refreshTimeInterval.ToString());

                //    writer.WriteElementString("Resolution", OptionsInOnlineMode.Resolution.ToString());
                //    //writer.WriteElementString("L0", OptionsInOnlineMode.L0.ToString());
                //    //writer.WriteElementString("S0", OptionsInOnlineMode.S0.ToString());
                //    writer.WriteElementString("nutnDivide", OptionsInOnlineMode.nutnDivide.ToString());
                //    writer.WriteElementString("nutnMultiple", OptionsInOnlineMode.nutnMultiple.ToString());
                //    writer.WriteElementString("mmDivide", OptionsInOnlineMode.mmDivide.ToString());
                //    writer.WriteElementString("mmCoeff", OptionsInOnlineMode.mmCoeff.ToString());
                //    writer.WriteElementString("isAutoChecked", OptionsInOnlineMode.isAutoChecked.ToString());
                //    writer.WriteElementString("isManualChecked", OptionsInOnlineMode.isManualChecked.ToString());
                //    writer.WriteElementString("ratioElongation", OptionsInOnlineMode.xRange.ToString());
                //    writer.WriteElementString("ratioForce", OptionsInOnlineMode.yRange.ToString());
                //    writer.WriteElementString("onlineWriteEndTimeInterval", OptionsInOnlineMode.onlineWriteEndTimeInterval.ToString());
                //    writer.WriteElementString("calculateMaxForceForTf", OptionsInOnlineMode.calculateMaxForceForTf.ToString());
                //    writer.WriteElementString("timeIntervalForCalculationOfChangedParameters", OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters.ToString());
                //    writer.WriteElementString("E2E4Border", OptionsInOnlineMode.E2E4Border.ToString());


                //    writer.WriteEndElement();


                //    writer.WriteEndElement();
                //    writer.WriteEndDocument();
                //    writer.Close();
                //}
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void writeXMLFile()}", System.DateTime.Now);
            }
        }

        //#region L0
        //private void saveOptionsL0_Online()
        //{
        //    double L0;
        //    string strL0 = tfL0.Text.Replace(',', '.');
        //    bool isN = Double.TryParse(strL0, out L0);

        //    if (isN == false)
        //    {
        //        MessageBox.Show("Trebate uneti broj u polje početne dužine [L0] !");
        //    }
        //    else
        //    {
        //        OptionsInOnlineMode.L0 = L0;
        //    }

        //    writeXMLFile();
        //    _onlineMode.createOnlineGraphics();
        //    _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
        //}

        //private void markSavedOnlineOptionsAsBlackL0_Online()
        //{
        //    tfL0.Foreground = Brushes.Black;
        //}

        //private void tfL0_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    tfL0.Foreground = Brushes.Red;
        //}

        //private void tfL0_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Tab)
        //    {
        //        saveOptionsL0_Online();
        //        markSavedOnlineOptionsAsBlackL0_Online();
        //    }
        //}


        //#endregion

        //#region S0

        //private void saveOptionsS0_Online()
        //{
        //    double S0;
        //    string strS0 = tfS0.Text.Replace(',', '.');
        //    bool isN = Double.TryParse(strS0, out S0);

        //    if (isN == false)
        //    {
        //        MessageBox.Show("Trebate uneti broj u polje početne površine [S0] !");
        //    }
        //    else
        //    {
        //        OptionsInOnlineMode.S0 = S0;
        //    }


        //    writeXMLFile();
        //    _onlineMode.createOnlineGraphics();
        //    _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
        //}

        //private void markSavedOnlineOptionsAsBlackS0_Online()
        //{
        //    tfS0.Foreground = Brushes.Black;
        //}


        //private void tfS0_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    tfS0.Foreground = Brushes.Red;
        //}

        //private void tfS0_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Tab)
        //    {
        //        saveOptionsS0_Online();
        //        markSavedOnlineOptionsAsBlackS0_Online();
        //    }
        //}

        //#endregion

        #region ForceDivide

        private void saveOptionstfCalForceDivide_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje sile!");
                }
                else
                {
                    OptionsInOnlineMode.nutnDivide = forceDiv;
                    OptionsInPlottingMode.nutnDivide = forceDiv;
                    _onlineMode.Plotting.OptionsPlotting.tfCalForceDivide.Text = forceDiv.ToString();
                }


                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCalForceDivide_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide_Online()
        {
            try
            {
                //tfCalForceDivide.Foreground = Brushes.Black;
                tfCalForceDivide.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalForceDivide_Online()}", System.DateTime.Now);
            }
        }

        private void tfCalForceDivide_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalForceDivide_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfCalForceDivide_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCalForceDivide_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalForceDivide_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion


        #region ForceMultiple

        private void saveOptionstfCalForceMultiple_Online()
        {
            try
            {
                double forceMul;
                string strforceMul = tfCalForceMultiple.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceMul, out forceMul);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje sile!");
                }
                else
                {
                    OptionsInOnlineMode.nutnMultiple = forceMul;
                    OptionsInPlottingMode.nutnMultiple = forceMul;
                    _onlineMode.Plotting.OptionsPlotting.tfCalForceMultiple.Text = forceMul.ToString();
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCalForceMultiple_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple_Online()
        {
            try
            {
                //tfCalForceMultiple.Foreground = Brushes.Black;
                tfCalForceMultiple.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalForceMultiple_Online()}", System.DateTime.Now);
            }
        }

        private void tfCalForceMultiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalForceMultiple_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalForceMultiple_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCalForceMultiple_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalForceMultiple_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion




        #region ElongationDivide

        private void saveOptionstfCalElonDivide_Online()
        {
            try
            {
                double elonDiv;
                string strelonDiv = tfCalElonDivide.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonDiv, out elonDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje izduženja!");
                }
                else
                {
                    OptionsInOnlineMode.mmDivide = elonDiv;
                    OptionsInPlottingMode.mmDivide = elonDiv;
                    _onlineMode.Plotting.OptionsPlotting.tfCalElonDivide.Text = elonDiv.ToString();
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCalElonDivide_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonDivide_Online()
        {
            try
            {
                //tfCalElonDivide.Foreground = Brushes.Black;
                tfCalElonDivide.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonDivide_Online()}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonDivide.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonDivide_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfCalElonDivide_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCalElonDivide_Online();
                    markSavedOnlineOptionsAsBlacktfCalElonDivide_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonDivide_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ElongationMultiple

        private void saveOptionstfCalElonMultiple_Online()
        {
            try
            {
                double elonMul;
                string strelonMul = tfCalElonMultiple.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonMul, out elonMul);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje relativnog izduženja!");
                }
                else
                {
                    OptionsInOnlineMode.mmCoeff = elonMul;
                    OptionsInPlottingMode.mmCoeff = elonMul;
                    _onlineMode.Plotting.OptionsPlotting.tfCalElonMultiple.Text = elonMul.ToString();
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCalElonMultiple_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonMultiple_Online()
        {
            try
            {
                //tfCalElonMultiple.Foreground = Brushes.Black;
                tfCalElonMultiple.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonMultiple_Online()}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonMultiple.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonMultiple_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCalElonMultiple_Online();
                    markSavedOnlineOptionsAsBlacktfCalElonMultiple_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonMultiple_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ElongationDivideWithEkstenziometer


        private void saveOptionstfCalElonDivide2_Online()
        {
            try
            {
                double elonDiv;
                string strelonDiv = tfCalElonDivide2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonDiv, out elonDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje izduženja[ekstenziometar]!");
                }
                else
                {
                    OptionsInOnlineMode.mmDivideWithEkstenziometer = elonDiv;
                    OptionsInPlottingMode.mmDivideWithEkstenziometer = elonDiv;
                    _onlineMode.Plotting.OptionsPlotting.tfCalElonDivide2.Text = elonDiv.ToString();
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCalElonDivide2_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonDivide2_Online()
        {
            try
            {
                //tfCalElonDivide2.Foreground = Brushes.Black;
                tfCalElonDivide2.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonDivide2_Online()}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonDivide2.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonDivide2_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCalElonDivide2_Online();
                    markSavedOnlineOptionsAsBlacktfCalElonDivide2_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonDivide2_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ElongationMultipleWithEkstenziometer

        private void saveOptionstfCalElonMultiple2_Online()
        {
            try
            {
                double elonMul;
                string strelonMul = tfCalElonMultiple2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonMul, out elonMul);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje relativnog izduženja[ekstenziometar]!");
                }
                else
                {
                    OptionsInOnlineMode.mmCoeffWithEkstenziometer = elonMul;
                    OptionsInPlottingMode.mmCoeffWithEkstenziometer = elonMul;
                    _onlineMode.Plotting.OptionsPlotting.tfCalElonDivide2.Text = elonMul.ToString();
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCalElonMultiple2_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonMultiple2_Online()
        {
            try
            {
                //tfCalElonMultiple2.Foreground = Brushes.Black;
                tfCalElonMultiple2.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonMultiple2_Online()}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonMultiple2.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonMultiple2_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCalElonMultiple2_Online();
                    markSavedOnlineOptionsAsBlacktfCalElonMultiple2_Online();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCalElonMultiple2_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        #region radioButtonforRange

        private void rbtnAuto_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineMode.isAutoChecked = true;

                tfRatioForce.IsReadOnly = true;
                tfRatioElongation.IsReadOnly = true;

                OptionsInOnlineMode.isManualChecked = false;
                rbtnManual.IsChecked = false;


                writeXMLFile();
                //OptionsInOnlineMode.xRange = 0.95;
                //OptionsInOnlineMode.yRange = 0.95;
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void rbtnAuto_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }

        private void rbtnAuto_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineMode.isAutoChecked = false;

                tfRatioForce.IsReadOnly = false;
                tfRatioElongation.IsReadOnly = false;

                OptionsInOnlineMode.isManualChecked = true;
                rbtnManual.IsChecked = true;
                double xRange = 0;
                string strratioElongation = tfRatioElongation.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioElongation, out xRange);
                if (isN)
                {
                    OptionsInOnlineMode.xRange = xRange;
                }
                double yRange = 0;
                string strratioForce = tfRatioForce.Text.Replace(',', '.');
                isN = Double.TryParse(strratioForce, out yRange);
                if (isN)
                {
                    OptionsInOnlineMode.yRange = yRange;
                }

                writeXMLFile();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void rbtnAuto_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion

        #region ratioForce

        public void SaveOptionstfRatioForce_Online()
        {
            try
            {
                saveOptionstfRatioForce_Online();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {public void SaveOptionstfRatioForce_Online()}", System.DateTime.Now);
            }
        }

        public void MarkSavedOnlineOptionsAsBlacktfRatioForce_Online()
        {
            try
            {
                markSavedOnlineOptionsAsBlacktfRatioForce_Online();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {public void MarkSavedOnlineOptionsAsBlacktfRatioForce_Online()}", System.DateTime.Now);
            }
        }

        private void saveOptionstfRatioForce_Online()
        {
            try
            {
                double ratioForce;
                string strratioForce = tfRatioForce.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioForce, out ratioForce);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos razmere napona!");
                }
                else
                {
                    OptionsInOnlineMode.yRange = ratioForce;
                    OptionsInPlottingMode.yRange = ratioForce;
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.Plotting.createOfflineGraphics();

                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfRatioForce_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfRatioForce_Online()
        {
            try
            {
                //tfRatioForce.Foreground = Brushes.Black;
                tfRatioForce.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfRatioForce_Online()}", System.DateTime.Now);
            }
        }

        private void tfRatioForce_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRatioForce.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfRatioForce_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfRatioForce_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRatioForce_Online();
                    markSavedOnlineOptionsAsBlacktfRatioForce_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfRatioForce_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ratioElongation

        private void saveOptionstfRatioElongation_Online()
        {
            try
            {
                double ratioElongation;
                string strratioElongation = tfRatioElongation.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioElongation, out ratioElongation);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje razmere relativnog izduženja!");
                }
                else
                {
                    OptionsInOnlineMode.xRange = ratioElongation;
                    OptionsInPlottingMode.xRange = ratioElongation;
                }

                writeXMLFile();
                _onlineMode.Plotting.WriteXMLFileOffline();
                _onlineMode.createOnlineGraphics();
                _onlineMode.Plotting.createOfflineGraphics();

                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfRatioElongation_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfRatioElongation_Online()
        {
            try
            {
                //tfRatioElongation.Foreground = Brushes.Black;
                tfRatioElongation.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfRatioElongation_Online()}", System.DateTime.Now);
            }
        }

        private void tfRatioElongation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRatioElongation.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfRatioElongation_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioElongation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRatioElongation_Online();
                    markSavedOnlineOptionsAsBlacktfRatioElongation_Online();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfRatioElongation_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region endTimeWriteOnline

        private void saveOptionstfEndOnlineWrite_Online()
        {
            try
            {
                double endOnlineWriteTimeInterval;
                string strOnlineTimeEndInterval = tfEndOnlineWrite.Text.Replace(',', '.');
                bool isN = Double.TryParse(strOnlineTimeEndInterval, out endOnlineWriteTimeInterval);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje Interval kraja online upisa!");
                }
                else
                {
                    if (endOnlineWriteTimeInterval < 1000)
                    {
                        MessageBox.Show("Trebate uneti broj u polje Interval kraja online upisa, koji je jednak ili veći od 1000 ms!");
                        tfEndOnlineWrite.Text = String.Empty;
                        OptionsInOnlineMode.onlineWriteEndTimeInterval = 1000;
                    }

                    OptionsInOnlineMode.onlineWriteEndTimeInterval = endOnlineWriteTimeInterval;
                }

                writeXMLFile();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfEndOnlineWrite_Online()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfEndOnlineWrite_Online()
        {
            try
            {
                //tfEndOnlineWrite.Foreground = Brushes.Black;
                tfEndOnlineWrite.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfEndOnlineWrite_Online()}", System.DateTime.Now);
            }
        }

        private void tfEndOnlineWrite_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfEndOnlineWrite.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfEndOnlineWrite_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfEndOnlineWrite_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfEndOnlineWrite_Online();
                    markSavedOnlineOptionsAsBlacktfEndOnlineWrite_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfEndOnlineWrite_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        #region tfE2E4Border


        private void saveOptionstfE2E4Border_Online()
        {
            try
            {
                double e2E4Border;
                string stre2E4Border = tfE2E4Border.Text.Replace(',', '.');
                bool isN = Double.TryParse(stre2E4Border, out e2E4Border);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos granice tečenja!");
                }
                else
                {
                    OptionsInOnlineMode.E2E4Border = e2E4Border;
                }

                writeXMLFile();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfE2E4Border_Online()}", System.DateTime.Now);
            }
        }


        private void markSavedOnlineOptionsAsBlacktfE2E4Border_Online()
        {
            try
            {
                //tfE2E4Border.Foreground = Brushes.Black;
                tfE2E4Border.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfE2E4Border_Online()}", System.DateTime.Now);
            }
        }


        private void tfE2E4Border_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfE2E4Border.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfE2E4Border_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void setE2E4E3E4BordersInPlottingGraph() 
        {
            try
            {
                if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                {
                    double newx = 0;
                    for (int i = 0; i < _onlineMode.Plotting.DataReader.RelativeElongation.Count; i++)
                    {


                        if (_onlineMode.Plotting.DataReader.RelativeElongation[i] >= OptionsInOnlineMode.E2E4Border)
                        {
                            _onlineMode.Plotting.DataReader.PreassureInMPa[i] = Math.Round(_onlineMode.Plotting.DataReader.PreassureInMPa[i], 0);
                            _onlineMode.Plotting.setPointAtGraphicR2R4(_onlineMode.Plotting.DataReader.RelativeElongation[i], _onlineMode.Plotting.DataReader.PreassureInMPa[i]);
                            //setPointAtGraphicR2R4(x, y);

                            newx = _onlineMode.Plotting.DataReader.RelativeElongation[i];
                            break;
                        }

                    }

                    //NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                    _onlineMode.Plotting.E2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, _onlineMode.Plotting.Rp02RI, _onlineMode);
                    _onlineMode.Plotting.E2e4CalculationAfterManualFitting.DivideE2AndE4Interval(_onlineMode.Plotting.E2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E2E4Border, _onlineMode.Plotting.XTranslateAmountFittingMode, true);
                    _onlineMode.Plotting.SetE2E4MinMaxAvg(_onlineMode.Plotting.E2e4CalculationAfterManualFitting);
                    _onlineMode.Plotting.SetResultsInterfaceForManualSetPoint();
                }
                else
                {
                    double newx = 0;
                    for (int i = 0; i < _onlineMode.Plotting.DataReader.RelativeElongation.Count; i++)
                    {


                        if (_onlineMode.Plotting.DataReader.RelativeElongation[i] >= OptionsInOnlineMode.E3E4Border)
                        {
                            _onlineMode.Plotting.DataReader.PreassureInMPa[i] = Math.Round(_onlineMode.Plotting.DataReader.PreassureInMPa[i], 0);
                            _onlineMode.Plotting.setPointAtGraphicR2R4(_onlineMode.Plotting.DataReader.RelativeElongation[i], _onlineMode.Plotting.DataReader.PreassureInMPa[i]);
                            //setPointAtGraphicR2R4(x, y);

                            newx = _onlineMode.Plotting.DataReader.RelativeElongation[i];
                            break;
                        }

                    }

                    //NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                    _onlineMode.Plotting.E2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, _onlineMode.Plotting.Rp02RI, _onlineMode);
                    _onlineMode.Plotting.E2e4CalculationAfterManualFitting.DivideE2AndE4Interval(_onlineMode.Plotting.E2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, _onlineMode.Plotting.XTranslateAmountFittingMode, false);
                    _onlineMode.Plotting.SetE2E4MinMaxAvg(_onlineMode.Plotting.E2e4CalculationAfterManualFitting);
                    _onlineMode.Plotting.SetResultsInterfaceForManualSetPoint();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void setE2E4E3E4BordersInPlottingGraph()}", System.DateTime.Now);
            }
        }

        private void tfE2E4Border_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfE2E4Border_Online();
                    markSavedOnlineOptionsAsBlacktfE2E4Border_Online();

                    setE2E4E3E4BordersInPlottingGraph();

                    _onlineMode.Plotting.tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E2E4Border.ToString();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfE2E4Border_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion



        #endregion

        private void chbShowMaxForce_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                _onlineMode.tfMaxForceInKN.Text = Constants.ZERO;
                OptionsInOnlineMode.calculateMaxForceForTf = true;
                writeXMLFile();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void chbShowMaxForce_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbShowMaxForce_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _onlineMode.tfMaxForceInKN.Text = String.Empty;
                OptionsInOnlineMode.calculateMaxForceForTf = false;
                writeXMLFile();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void chbShowMaxForce_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbIsCalibration_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineMode.isCalibration = true;
                writeXMLFile();

                //omoguci promenu kalibracionih podataka
                tfCalForceDivide.IsReadOnly = false;
                tfCalForceMultiple.IsReadOnly = false;
                tfCalElonDivide.IsReadOnly = false;
                tfCalElonMultiple.IsReadOnly = false;
                tfCalElonDivide2.IsReadOnly = false;
                tfCalElonMultiple2.IsReadOnly = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void chbIsCalibration_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbIsCalibration_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineMode.isCalibration = false;
                writeXMLFile();

                //onemoguci promenu kalibracionih podataka
                tfCalForceDivide.IsReadOnly = true;
                tfCalForceMultiple.IsReadOnly = true;
                tfCalElonDivide.IsReadOnly = true;
                tfCalElonMultiple.IsReadOnly = true;
                tfCalElonDivide2.IsReadOnly = true;
                tfCalElonMultiple2.IsReadOnly = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void chbIsCalibration_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #region combobox

        private void cmbParChangedInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbI200.IsSelected == true)
                {
                    OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = 200;
                }
                if (cmbI400.IsSelected == true)
                {
                    OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = 400;
                }
                if (cmbI600.IsSelected == true)
                {
                    OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = 600;
                }
                if (cmbI800.IsSelected == true)
                {
                    OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = 800;
                }
                if (cmbI1000.IsSelected == true)
                {
                    OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = 1000;
                }

                writeXMLFile();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void cmbParChangedInterval_SelectionChanged(object sender, SelectionChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region tfE3E4Border

        private void saveOptionstfE3E4Border_Online()
        {
            try
            {
                double e3E4Border;
                string stre3E4Border = tfE3E4Border.Text.Replace(',', '.');
                bool isN = Double.TryParse(stre3E4Border, out e3E4Border);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos granice tečenja e(R3)/e(R4) !");
                }
                else
                {
                    OptionsInOnlineMode.E3E4Border = e3E4Border;
                }

                writeXMLFile();
                _onlineMode.createOnlineGraphics();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfE3E4Border_Online()}", System.DateTime.Now);
            }
        }


        private void markSavedOnlineOptionsAsBlacktfE3E4Border_Online()
        {
            try
            {
                //tfE3E4Border.Foreground = Brushes.Black;
                tfE3E4Border.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfE3E4Border_Online()}", System.DateTime.Now);
            }
        }

        private void tfE3E4Border_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfE3E4Border.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfE3E4Border_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfE3E4Border_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfE3E4Border_Online();
                    markSavedOnlineOptionsAsBlacktfE3E4Border_Online();

                    setE2E4E3E4BordersInPlottingGraph();

                    _onlineMode.Plotting.tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E3E4Border.ToString();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfE3E4Border_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region tfCOM

        private void saveOptionstfCOM_Online()
        {
            try
            {
                int com;
                string strCOM = tfCOM.Text.Replace(',', '.');
                bool isN = Int32.TryParse(strCOM, out com);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti ceo broj u polje za unos broja serijskog porta COM-a !");
                }
                else
                {
                    OptionsInOnlineMode.COM = com;
                }

                writeXMLFile();
                _onlineMode.counterWhoDetermitedOneSecond = Convert.ToInt32(_onlineMode.milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void saveOptionstfCOM_Online()}", System.DateTime.Now);
            }
        }


        private void markSavedOnlineOptionsAsBlackCOM_Online()
        {
            try
            {
                //tfCOM.Foreground = Brushes.Black;
                tfCOM.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void markSavedOnlineOptionsAsBlackCOM_Online()}", System.DateTime.Now);
            }
        }

        private void tfCOM_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCOM.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCOM_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfCOM_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfCOM_Online();
                    markSavedOnlineOptionsAsBlackCOM_Online();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void tfCOM_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        

        #endregion

        private void rbtnE2E4_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineMode.isE2E4BorderSelected = true;
                writeXMLFile();
                tfE2E4Border.IsEnabled = true;
                tfE3E4Border.IsEnabled = false;

                _onlineMode.Plotting.lblR2R3R4Border.Text = "R2/R4";
                _onlineMode.Plotting.tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E2E4Border.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void rbtnE2E4_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

       

        private void rbtnE3E4_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineMode.isE2E4BorderSelected = false;
                writeXMLFile();
                tfE2E4Border.IsEnabled = false;
                tfE3E4Border.IsEnabled = true;

                _onlineMode.Plotting.lblR2R3R4Border.Text = "R3/R4";
                _onlineMode.Plotting.tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E3E4Border.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void rbtnE3E4_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }



        private void btnForceRange_Click(object sender, RoutedEventArgs e)
        {
            ForceRanges fr = new ForceRanges(_onlineMode);
            _onlineMode.OnHeader.ForceRanges = fr;
            fr.ShowDialog();
        }

       
       

      

    }//end of class OptionsOnline
} //emd of namespace
