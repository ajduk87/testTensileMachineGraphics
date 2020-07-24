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
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for OptionsOnlineManagingOfTTM.xaml
    /// full name class : Options Online Managing Of Test Tensile Machine
    /// </summary>
    public partial class OptionsOnlineManagingOfTTM : Window
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



        public OptionsOnlineManagingOfTTM(OnlineMode oMode)
        {
            try
            {
                InitializeComponent();
                _onlineMode = oMode;
                LoadOptionsOnlineManagingOfTTM();

                _onlineMode.IsOptionsForManagingOfTTM = true;

                if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                {
                    lblE2Part2.Text = "(R2)";
                }
                else
                {
                    lblE2Part2.Text = "(R3)";
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {public OptionsOnlineManagingOfTTM(OnlineMode oMode)}", System.DateTime.Now);
            }
        }


        public void LoadOptionsOnlineManagingOfTTM()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                if (File.Exists(Constants.onlineModeManagingOfTTMXml) == false)
                {
                    return;
                }

                XmlTextReader textReader = new XmlTextReader(Constants.onlineModeManagingOfTTMXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("Rmax"))
                        {
                            OptionsInOnlineManagingOfTTM.Rmax = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfRmax.Text = OptionsInOnlineManagingOfTTM.Rmax.ToString();
                            //tfRmax.Foreground = Brushes.Black;
                            tfRmax.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("Rmin"))
                        {
                            OptionsInOnlineManagingOfTTM.Rmin = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfRmin.Text = OptionsInOnlineManagingOfTTM.Rmin.ToString();
                            //tfRmin.Foreground = Brushes.Black;
                            tfRmin.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("eR2"))
                        {
                            OptionsInOnlineManagingOfTTM.eR2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfE2.Text = OptionsInOnlineManagingOfTTM.eR2.ToString();
                            //tfE2.Foreground = Brushes.Black;
                            tfE2.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("eR4"))
                        {
                            OptionsInOnlineManagingOfTTM.eR4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfE4.Text = OptionsInOnlineManagingOfTTM.eR4.ToString();
                            //tfE4.Foreground = Brushes.Black;
                            tfE4.Foreground = Brushes.White;
                        }


                    } //end of  if (nType == XmlNodeType.Element)
                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {public void LoadOptionsOnlineManagingOfTTM()}", System.DateTime.Now);
            }
        }





        private void writeXMLFileManagingOfTTM()
        {
            try
            {
                //write in xml file

                XElement xmlRoot = new XElement("OnlineModeManagingOfTTM",
                                                new XElement("OnlineModeManagingOfTTMCurrentOptions",
                                                            new XElement("Rmax", OptionsInOnlineManagingOfTTM.Rmax.ToString()),
                                                            new XElement("Rmin", OptionsInOnlineManagingOfTTM.Rmin.ToString()),
                                                            new XElement("eR2", OptionsInOnlineManagingOfTTM.eR2.ToString()),
                                                            new XElement("eR4", OptionsInOnlineManagingOfTTM.eR4.ToString())
                                                            )
                                                );

                xmlRoot.Save(Constants.onlineModeManagingOfTTMXml);
                //using (XmlWriter writer = XmlWriter.Create(Constants.onlineModeManagingOfTTMXml))
                //{
                //    writer.WriteStartDocument();
                //    writer.WriteStartElement("OnlineModeManagingOfTTM");


                //    writer.WriteStartElement("OnlineModeManagingOfTTMCurrentOptions");

                //    writer.WriteElementString("Rmax", OptionsInOnlineManagingOfTTM.Rmax.ToString());
                //    writer.WriteElementString("Rmin", OptionsInOnlineManagingOfTTM.Rmin.ToString());
                //    writer.WriteElementString("eR2", OptionsInOnlineManagingOfTTM.eR2.ToString());
                //    writer.WriteElementString("eR4", OptionsInOnlineManagingOfTTM.eR4.ToString());

                //    writer.WriteEndElement();


                //    writer.WriteEndElement();
                //    writer.WriteEndDocument();
                //    writer.Close();
                //}
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void writeXMLFileManagingOfTTM()}", System.DateTime.Now);
            }
        }



        #region tfRatioForceChangeOfR

        private void saveOptionstfRmax()
        {
            try
            {
                double rmax;
                string strrmax = tfRmax.Text.Replace(',', '.');
                bool isN = Double.TryParse(strrmax, out rmax);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos maksimalne dozvoljene promene napona!");
                }
                else
                {
                    OptionsInOnlineManagingOfTTM.Rmax = rmax;
                }

                writeXMLFileManagingOfTTM();
                //ovde treba postaviti horizontalne linije
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.SetHorizontalLineRmax(rmax);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfRmax()}", System.DateTime.Now);
            }
        }

        private void markSavedAsBlacktfRmax()
        {
            try
            {
                //tfRmax.Foreground = Brushes.Black;
                tfRmax.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfRmax()}", System.DateTime.Now);
            }
        }

        private void tfRmax_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRmax.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRmax_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRmax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRmax();
                    markSavedAsBlacktfRmax();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRmax_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region tfE2

        private void saveOptionstfE2()
        {
            try
            {
                double e2;
                string stre2 = tfE2.Text.Replace(',', '.');
                bool isN = Double.TryParse(stre2, out e2);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos maksimalne dozvoljene promene izduzenja ranga 2!");
                }
                else
                {
                    OptionsInOnlineManagingOfTTM.eR2 = e2;
                }

                writeXMLFileManagingOfTTM();
                //ovo se sada racuna vise se ne postavlja unosom
                //ovde treba postaviti horizontalne linije
                //if (_onlineMode.VXY != null)
                //{
                //    _onlineMode.VXY.SetHorizontalLineE2(e2);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfE2()}", System.DateTime.Now);
            }
        }

        private void markSavedAsBlacktfE2()
        {
            try
            {
                //tfE2.Foreground = Brushes.Black;
                tfE2.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfE2()}", System.DateTime.Now);
            }
        }


        private void tfE2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfE2.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfE2_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfE2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfE2();
                    markSavedAsBlacktfE2();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfE2_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        #region tfE4

        private void saveOptionstfE4()
        {
            try
            {
                double e4;
                string stre4 = tfE4.Text.Replace(',', '.');
                bool isN = Double.TryParse(stre4, out e4);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos maksimalne dozvoljene promene izduzenja ranga 4!");
                }
                else
                {
                    OptionsInOnlineManagingOfTTM.eR4 = e4;
                }

                writeXMLFileManagingOfTTM();
                //ovo se sada racuna vise se ne postavlja unosom
                //ovde treba postaviti horizontalne linije
                //if (_onlineMode.VXY != null)
                //{
                //    _onlineMode.VXY.SetHorizontalLineE4(e4);
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfE4()}", System.DateTime.Now);
            }
        }

        private void markSavedAsBlacktfE4()
        {
            try
            {
                //tfE4.Foreground = Brushes.Black;
                tfE4.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfE4()}", System.DateTime.Now);
            }
        }


        private void tfE4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfE4.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfE4_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfE4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfE4();
                    markSavedAsBlacktfE4();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfE4_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _onlineMode.IsOptionsForManagingOfTTM = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }


        #region tfRmin

        private void saveOptionstfRmin()
        {
            try
            {
                double rmin;
                string strrmin = tfRmin.Text.Replace(',', '.');
                bool isN = Double.TryParse(strrmin, out rmin);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje za unos minimalne dozvoljene promene napona!");
                }
                else
                {
                    OptionsInOnlineManagingOfTTM.Rmin = rmin;
                }

                writeXMLFileManagingOfTTM();
                //ovde treba postaviti horizontalne linije
                if (_onlineMode.VXY != null)
                {
                    _onlineMode.VXY.SetHorizontalLineRmax(rmin);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void saveOptionstfRmin()}", System.DateTime.Now);
            }
        }

        private void markSavedAsBlacktfRmin()
        {
            try
            {
                //tfRmin.Foreground = Brushes.Black;
                tfRmin.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void markSavedAsBlacktfRmin()}", System.DateTime.Now);
            }
        }

        private void tfRmin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRmin.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRmin_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRmin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRmin();
                    markSavedAsBlacktfRmin();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnlineChangeOfRAndE.xaml.cs] {private void tfRmin_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion
    }
}
