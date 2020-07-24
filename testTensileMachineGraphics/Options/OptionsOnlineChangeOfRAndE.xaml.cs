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
using testTensileMachineGraphics.OnlineModeFolder;
using testTensileMachineGraphics.Windows;
using System.Xml;
using System.Xml.Linq;

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for OptionsOnlineChangeOfRAndE.xaml
    /// </summary>
    public partial class OptionsOnlineChangeOfRAndE : Window
    {


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

        //private VelocityOfChangeParametersXY vXYinoptions;

        //public VelocityOfChangeParametersXY VXYinoptions
        //{
        //    set
        //    {
        //        if (value != null)
        //        {
        //            vXYinoptions = value;
        //        }
        //    }
        //}


        private void writeXMLFileChangeOfRAndE()
        {
            try
            {
                //write in xml file

                XElement xmlRoot = new XElement("OnlineModeChangeOfRAndE",
                                                new XElement("OnlineModeChangeOfRAndECurrentOptions",
                                                            new XElement("isAutoCheckedChangeR", OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR.ToString()),
                                                            new XElement("isManualCheckedChangeR", OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR.ToString()),
                                                            new XElement("xRangeChangeR", OptionsInOnlineModeChangeOfRAndE.xRangeChangeR.ToString()),
                                                            new XElement("yRangeChangeR", OptionsInOnlineModeChangeOfRAndE.yRangeChangeR.ToString()),

                                                            new XElement("isAutoCheckedChangeElongation", OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation.ToString()),
                                                            new XElement("isManualCheckedChangeElongation", OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation.ToString()),
                                                            new XElement("xRangeChangeElongation", OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation.ToString()),
                                                            new XElement("yRangeChangeElongation", OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation.ToString())
                                                            )
                                               );


                xmlRoot.Save(Constants.onlineModeChangeOfRAndEOptionsXml);

                //using (XmlWriter writer = XmlWriter.Create(Constants.onlineModeChangeOfRAndEOptionsXml))
                //{
                //    writer.WriteStartDocument();
                //    writer.WriteStartElement("OnlineModeChangeOfRAndE");


                //    writer.WriteStartElement("OnlineModeChangeOfRAndECurrentOptions");

                //    writer.WriteElementString("isAutoCheckedChangeR", OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR.ToString());
                //    writer.WriteElementString("isManualCheckedChangeR", OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR.ToString());
                //    writer.WriteElementString("xRangeChangeR", OptionsInOnlineModeChangeOfRAndE.xRangeChangeR.ToString());
                //    writer.WriteElementString("yRangeChangeR", OptionsInOnlineModeChangeOfRAndE.yRangeChangeR.ToString());

                //    writer.WriteElementString("isAutoCheckedChangeElongation", OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation.ToString());
                //    writer.WriteElementString("isManualCheckedChangeElongation", OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation.ToString());
                //    writer.WriteElementString("xRangeChangeElongation", OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation.ToString());
                //    writer.WriteElementString("yRangeChangeElongation", OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation.ToString());

                //    writer.WriteEndElement();


                //    writer.WriteEndElement();
                //    writer.WriteEndDocument();
                //    writer.Close();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void writeXMLFileChangeOfRAndE()}", System.DateTime.Now);
            }
        }


        public void LoadOptionsOnlineChangeOfRAndE()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.onlineModeChangeOfRAndEOptionsXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {


                        if (textReader.Name.Equals("isAutoCheckedChangeR"))
                        {
                            string isAuto = textReader.ReadElementContentAsString();
                            if (isAuto.Equals("True"))
                            {
                                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR = true;


                                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR = false;


                                OptionsInOnlineModeChangeOfRAndE.xRangeChangeR = 0.95;
                                OptionsInOnlineModeChangeOfRAndE.yRangeChangeR = 0.95;
                            }
                            if (isAuto.Equals("False"))
                            {
                                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR = false;


                                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR = true;

                            }
                        }


                        if (textReader.Name.Equals("xRangeChangeR"))
                        {
                            OptionsInOnlineModeChangeOfRAndE.xRangeChangeR = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfRatioTauChangeOfR.Text = OptionsInOnlineModeChangeOfRAndE.xRangeChangeR.ToString();
                            //tfRatioElongationChangeOfR.Foreground = Brushes.Black;
                            tfRatioTauChangeOfR.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("yRangeChangeR"))
                        {
                            OptionsInOnlineModeChangeOfRAndE.yRangeChangeR = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfRatioForceChangeOfR.Text = OptionsInOnlineModeChangeOfRAndE.yRangeChangeR.ToString();
                            //tfRatioForceChangeOfR.Foreground = Brushes.Black;
                            tfRatioForceChangeOfR.Foreground = Brushes.White;
                        }




                        if (textReader.Name.Equals("isAutoCheckedChangeElongation"))
                        {
                            string isAuto = textReader.ReadElementContentAsString();
                            if (isAuto.Equals("True"))
                            {
                                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation = true;


                                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation = false;


                                OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation = 0.95;
                                OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation = 0.95;
                            }
                            if (isAuto.Equals("False"))
                            {
                                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation = false;


                                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation = true;

                            }
                        }


                        if (textReader.Name.Equals("xRangeChangeElongation"))
                        {
                            OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfRatioTauChangeOfElongation.Text = OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation.ToString();
                            //tfRatioElongationChangeOfElongation.Foreground = Brushes.Black;
                            tfRatioTauChangeOfElongation.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("yRangeChangeElongation"))
                        {
                            OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfRatioElonChangeOfElongation.Text = OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation.ToString();
                            //tfRatioElonChangeOfElongation.Foreground = Brushes.Black;
                            tfRatioElonChangeOfElongation.Foreground = Brushes.White;
                        }


                    } //end of  if (nType == XmlNodeType.Element)
                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {public void LoadOptionsOnlineChangeOfRAndE()}", System.DateTime.Now);
            }
        }

        public OptionsOnlineChangeOfRAndE(OnlineMode oMode)
        {
            try
            {
                InitializeComponent();
                _onlineMode = oMode;

                LoadOptionsOnlineChangeOfRAndE();
                _onlineMode.IsOptionsForChangeGraphic = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {public OptionsOnlineChangeOfRAndE(OnlineMode oMode)}", System.DateTime.Now);
            }
        }


        public void setRadioButtons2() 
        {
            try
            {

                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR)
                {

                    rbtnManualChangeOfR.IsChecked = false;

                    rbtnAutoChangeOfR.IsChecked = true;
                    rbtnAutoChangeOfR.IsChecked = true;

                    tfRatioForceChangeOfR.IsReadOnly = true;
                    tfRatioTauChangeOfR.IsReadOnly = true;
                }
                else
                {
                    rbtnAutoChangeOfR.IsChecked = false;

                    rbtnManualChangeOfR.IsChecked = true;
                    tfRatioForceChangeOfR.IsReadOnly = false;
                    tfRatioTauChangeOfR.IsReadOnly = false;
                }



                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation)
                {

                    rbtnManualChangeOfElongation.IsChecked = false;

                    rbtnAutoChangeOfElongation.IsChecked = true;
                    rbtnAutoChangeOfElongation.IsChecked = true;

                    tfRatioElonChangeOfElongation.IsReadOnly = true;
                    tfRatioTauChangeOfElongation.IsReadOnly = true;
                }
                else
                {
                    rbtnAutoChangeOfElongation.IsChecked = false;

                    rbtnManualChangeOfElongation.IsChecked = true;
                    tfRatioElonChangeOfElongation.IsReadOnly = false;
                    tfRatioTauChangeOfElongation.IsReadOnly = false;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString() + " {setRadioButton2}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {public void setRadioButtons2()}", System.DateTime.Now);
            }
        }


        #region rbtnAutoChangeOfR

        private void rbtnAutoChangeOfR_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR = true;

                tfRatioForceChangeOfR.IsReadOnly = true;
                tfRatioTauChangeOfR.IsReadOnly = true;

                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR = false;
                rbtnManualChangeOfR.IsChecked = false;

                writeXMLFileChangeOfRAndE();
                OptionsInOnlineModeChangeOfRAndE.xRangeChangeR = 0.95;
                OptionsInOnlineModeChangeOfRAndE.yRangeChangeR = 0.95;

                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfRGraphic();
                    _onlineMode.VXY.CreateChangeOfRGraphic_Fitting();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void rbtnAutoChangeOfR_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnAutoChangeOfR_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR = false;

                tfRatioForceChangeOfR.IsReadOnly = false;
                tfRatioTauChangeOfR.IsReadOnly = false;

                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR = true;
                rbtnManualChangeOfR.IsChecked = true;

                double xRange = 0;
                string strratioElongation = tfRatioTauChangeOfR.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioElongation, out xRange);
                if (isN)
                {
                    OptionsInOnlineModeChangeOfRAndE.xRangeChangeR = xRange;
                }
                double yRange = 0;
                string strratioForce = tfRatioForceChangeOfR.Text.Replace(',', '.');
                isN = Double.TryParse(strratioForce, out yRange);
                if (isN)
                {
                    OptionsInOnlineModeChangeOfRAndE.yRangeChangeR = yRange;
                }

                writeXMLFileChangeOfRAndE();
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfRGraphic();
                    _onlineMode.VXY.CreateChangeOfRGraphic_Fitting();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void rbtnAutoChangeOfR_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

       

        #region tfRatioForceChangeOfR

       
        private void saveOptionstfRatioForceChangeOfR()
        {
            try
            {
                double ratioForce;
                string strratioForce = tfRatioForceChangeOfR.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioForce, out ratioForce);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos razmere napona!");
                }
                else
                {
                    OptionsInOnlineModeChangeOfRAndE.yRangeChangeR = ratioForce;
                }

                writeXMLFileChangeOfRAndE();
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfRGraphic();
                    _onlineMode.VXY.CreateChangeOfRGraphic_Fitting();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfRatioForceChangeOfR()}", System.DateTime.Now);
            }
        }


        private void markSavedAsBlacktfRatioForceChangeOfR()
        {
            try
            {
                //tfRatioForceChangeOfR.Foreground = Brushes.Black;
                tfRatioForceChangeOfR.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfRatioForceChangeOfR()}", System.DateTime.Now);
            }
        }

        private void tfRatioForceChangeOfR_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRatioForceChangeOfR.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioForceChangeOfR_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioForceChangeOfR_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRatioForceChangeOfR();
                    markSavedAsBlacktfRatioForceChangeOfR();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioForceChangeOfR_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        #region tfRatioTauChangeOfR

        private void saveOptionstfRatioTauChangeOfR()
        {
            try
            {
                double ratioTau;
                string strratioTau = tfRatioTauChangeOfR.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioTau, out ratioTau);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos razmere vremena!");
                }
                else
                {
                    OptionsInOnlineModeChangeOfRAndE.xRangeChangeR = ratioTau;
                }

                writeXMLFileChangeOfRAndE();
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfRGraphic();
                    _onlineMode.VXY.CreateChangeOfRGraphic_Fitting();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfRatioTauChangeOfR()}", System.DateTime.Now);
            }
        }



        private void markSavedAsBlackttfRatioTauChangeOfR()
        {
            try
            {
                //tfRatioTauChangeOfR.Foreground = Brushes.Black;
                tfRatioTauChangeOfR.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlackttfRatioTauChangeOfR()}", System.DateTime.Now);
            }
        }

        private void tfRatioTauChangeOfR_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRatioTauChangeOfR.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioTauChangeOfR_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioTauChangeOfR_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRatioTauChangeOfR();
                    markSavedAsBlackttfRatioTauChangeOfR();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioTauChangeOfR_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion

       


        #region rbtnAutoChangeOfElongation

        private void rbtnAutoChangeOfElongation_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation = true;

                tfRatioElonChangeOfElongation.IsReadOnly = true;
                tfRatioTauChangeOfElongation.IsReadOnly = true;

                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation = false;
                rbtnManualChangeOfElongation.IsChecked = false;

                writeXMLFileChangeOfRAndE();
                OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation = 0.95;
                OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation = 0.95;

                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfEGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void rbtnAutoChangeOfElongation_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnAutoChangeOfElongation_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation = false;

                tfRatioElonChangeOfElongation.IsReadOnly = false;
                tfRatioTauChangeOfElongation.IsReadOnly = false;

                OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation = true;
                rbtnManualChangeOfElongation.IsChecked = true;

                double xRange = 0;
                string strratioElongation = tfRatioTauChangeOfElongation.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioElongation, out xRange);
                if (isN)
                {
                    OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation = xRange;
                }
                double yRange = 0;
                string strratioForce = tfRatioElonChangeOfElongation.Text.Replace(',', '.');
                isN = Double.TryParse(strratioForce, out yRange);
                if (isN)
                {
                    OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation = yRange;
                }

                writeXMLFileChangeOfRAndE();

                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfEGraphic();
                    _onlineMode.VXY.CreateChangeOfEGraphic_Fitting();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void rbtnAutoChangeOfElongation_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

      


        #region tfRatioElonChangeOfElongation

        private void saveOptionstfRatioElonChangeOfElongation()
        {
            try
            {
                double ratioElongation;
                string strratioElongation = tfRatioElonChangeOfElongation.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioElongation, out ratioElongation);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos razmere elongacije!");
                }
                else
                {
                    OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation = ratioElongation;
                }

                writeXMLFileChangeOfRAndE();
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfEGraphic();
                    _onlineMode.VXY.CreateChangeOfEGraphic_Fitting();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfRatioElonChangeOfElongation()}", System.DateTime.Now);
            }
        }


        private void markSavedAsBlacktfRatioElonChangeOfElongation()
        {
            try
            {
                //tfRatioElonChangeOfElongation.Foreground = Brushes.Black;
                tfRatioElonChangeOfElongation.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfRatioElonChangeOfElongation()}", System.DateTime.Now);
            }
        }


        private void tfRatioElonChangeOfElongation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRatioElonChangeOfElongation.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioElonChangeOfElongation_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioElonChangeOfElongation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRatioElonChangeOfElongation();
                    markSavedAsBlacktfRatioElonChangeOfElongation();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioElonChangeOfElongation_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

      


        #region tfRatioTauChangeOfElongation

        private void saveOptionstfRatioTauChangeOfElongation()
        {
            try
            {
                double ratioTau;
                string strratioTau = tfRatioTauChangeOfElongation.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioTau, out ratioTau);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos razmere vremena!");
                }
                else
                {
                    OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation = ratioTau;
                }

                writeXMLFileChangeOfRAndE();
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.CreateChangeOfEGraphic();
                    _onlineMode.VXY.CreateChangeOfEGraphic_Fitting();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfRatioTauChangeOfElongation()}", System.DateTime.Now);
            }
        }


        private void markSavedAsBlacktfRatioTauChangeOfElongation()
        {
            try
            {
                //tfRatioTauChangeOfElongation.Foreground = Brushes.Black;
                tfRatioTauChangeOfElongation.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfRatioTauChangeOfElongation()}", System.DateTime.Now);
            }
        }


        private void tfRatioTauChangeOfElongation_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRatioTauChangeOfElongation.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioTauChangeOfElongation_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioTauChangeOfElongation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRatioTauChangeOfElongation();
                    markSavedAsBlacktfRatioTauChangeOfElongation();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRatioTauChangeOfElongation_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _onlineMode.IsOptionsForChangeGraphic = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

    }
}
