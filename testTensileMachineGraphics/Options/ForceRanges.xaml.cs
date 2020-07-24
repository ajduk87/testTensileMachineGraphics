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
using System.Xml.Linq;
using System.Xml;
using System.IO;
using testTensileMachineGraphics.OnlineModeFolder;

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for ForceRanges.xaml
    /// </summary>
    public partial class ForceRanges : Window
    {

        private OnlineMode onlineMode;

        public ForceRanges(OnlineMode onlineM)
        {
            InitializeComponent();
            onlineMode = onlineM;

            if (File.Exists(Constants.ForceRangesonlineOptions))
            {
                loadXMLFileForceRanges();
                setStringEmptyAfterLoading();
            }


        }

        private void setStringEmptyAfterLoading()
        {
            if (ForceRangesOptions.forceLower1 == 0 && ForceRangesOptions.forceUpper1 == 0 && ForceRangesOptions.nutnMultiple1 == 10 && ForceRangesOptions.nutnDivide1 == 10)
            {
                tfFrom1.Text = string.Empty;
                tfTo1.Text = string.Empty;
                tfCalForceMultiple1.Text = string.Empty;
                tfCalForceDivide1.Text = string.Empty;
                forceRange1.IsChecked = false;
                forceRange1.IsEnabled = false;
            }

            if (ForceRangesOptions.forceLower2 == 0 && ForceRangesOptions.forceUpper2 == 0 && ForceRangesOptions.nutnMultiple2 == 10 && ForceRangesOptions.nutnDivide2 == 10)
            {
                tfFrom2.Text = string.Empty;
                tfTo2.Text = string.Empty;
                tfCalForceMultiple2.Text = string.Empty;
                tfCalForceDivide2.Text = string.Empty;
                forceRange2.IsChecked = false;
                forceRange2.IsEnabled = false;
            }

            if (ForceRangesOptions.forceLower3 == 0 && ForceRangesOptions.forceUpper3 == 0 && ForceRangesOptions.nutnMultiple3 == 10 && ForceRangesOptions.nutnDivide3 == 10)
            {
                tfFrom3.Text = string.Empty;
                tfTo3.Text = string.Empty;
                tfCalForceMultiple3.Text = string.Empty;
                tfCalForceDivide3.Text = string.Empty;
                forceRange3.IsChecked = false;
                forceRange3.IsEnabled = false;
            }

            if (ForceRangesOptions.forceLower4 == 0 && ForceRangesOptions.forceUpper4 == 0 && ForceRangesOptions.nutnMultiple4 == 10 && ForceRangesOptions.nutnDivide4 == 10)
            {
                tfFrom4.Text = string.Empty;
                tfTo4.Text = string.Empty;
                tfCalForceMultiple4.Text = string.Empty;
                tfCalForceDivide4.Text = string.Empty;
                forceRange4.IsChecked = false;
                forceRange4.IsEnabled = false;
            }

            if (ForceRangesOptions.forceLower5 == 0 && ForceRangesOptions.forceUpper5 == 0 && ForceRangesOptions.nutnMultiple5 == 10 && ForceRangesOptions.nutnDivide5 == 10)
            {
                tfFrom5.Text = string.Empty;
                tfTo5.Text = string.Empty;
                tfCalForceMultiple5.Text = string.Empty;
                tfCalForceDivide5.Text = string.Empty;
                forceRange5.IsChecked = false;
                forceRange5.IsEnabled = false;
            }

            if (ForceRangesOptions.forceLower6 == 0 && ForceRangesOptions.forceUpper6 == 0 && ForceRangesOptions.nutnMultiple6 == 10 && ForceRangesOptions.nutnDivide6 == 10)
            {
                tfFrom6.Text = string.Empty;
                tfTo6.Text = string.Empty;
                tfCalForceMultiple6.Text = string.Empty;
                tfCalForceDivide6.Text = string.Empty;
                forceRange6.IsChecked = false;
                forceRange6.IsEnabled = false;
            }
        }

        private void loadXMLFileForceRanges()
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

                        if (textReader.Name.Equals("forceLower1"))
                        {
                            ForceRangesOptions.forceLower1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfFrom1.Text = ForceRangesOptions.forceLower1.ToString();
                            tfFrom1.Foreground = Brushes.White;
                            tfFrom1.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("forceUpper1"))
                        {
                            ForceRangesOptions.forceUpper1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfTo1.Text = ForceRangesOptions.forceUpper1.ToString();
                            tfTo1.Foreground = Brushes.White;
                            tfTo1.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnMultiple1"))
                        {
                            ForceRangesOptions.nutnMultiple1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceMultiple1.Text = ForceRangesOptions.nutnMultiple1.ToString();
                            tfCalForceMultiple1.Foreground = Brushes.White;
                            tfCalForceMultiple1.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnDivide1"))
                        {
                            ForceRangesOptions.nutnDivide1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceDivide1.Text = ForceRangesOptions.nutnDivide1.ToString();
                            tfCalForceDivide1.Foreground = Brushes.White;
                            tfCalForceDivide1.Background = Brushes.Black;
                        }



                        if (textReader.Name.Equals("forceLower2"))
                        {
                            ForceRangesOptions.forceLower2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfFrom2.Text = ForceRangesOptions.forceLower2.ToString();
                            tfFrom2.Foreground = Brushes.White;
                            tfFrom2.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("forceUpper2"))
                        {
                            ForceRangesOptions.forceUpper2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfTo2.Text = ForceRangesOptions.forceUpper2.ToString();
                            tfTo2.Foreground = Brushes.White;
                            tfTo2.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnMultiple2"))
                        {
                            ForceRangesOptions.nutnMultiple2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceMultiple2.Text = ForceRangesOptions.nutnMultiple2.ToString();
                            tfCalForceMultiple2.Foreground = Brushes.White;
                            tfCalForceMultiple2.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnDivide2"))
                        {
                            ForceRangesOptions.nutnDivide2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceDivide2.Text = ForceRangesOptions.nutnDivide2.ToString();
                            tfCalForceDivide2.Foreground = Brushes.White;
                            tfCalForceDivide2.Background = Brushes.Black;
                        }



                        if (textReader.Name.Equals("forceLower3"))
                        {
                            ForceRangesOptions.forceLower3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfFrom3.Text = ForceRangesOptions.forceLower3.ToString();
                            tfFrom3.Foreground = Brushes.White;
                            tfFrom3.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("forceUpper3"))
                        {
                            ForceRangesOptions.forceUpper3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfTo3.Text = ForceRangesOptions.forceUpper3.ToString();
                            tfTo3.Foreground = Brushes.White;
                            tfTo3.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnMultiple3"))
                        {
                            ForceRangesOptions.nutnMultiple3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceMultiple3.Text = ForceRangesOptions.nutnMultiple3.ToString();
                            tfCalForceMultiple3.Foreground = Brushes.White;
                            tfCalForceMultiple3.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnDivide3"))
                        {
                            ForceRangesOptions.nutnDivide3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceDivide3.Text = ForceRangesOptions.nutnDivide3.ToString();
                            tfCalForceDivide3.Foreground = Brushes.White;
                            tfCalForceDivide3.Background = Brushes.Black;
                        }




                        if (textReader.Name.Equals("forceLower4"))
                        {
                            ForceRangesOptions.forceLower4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfFrom4.Text = ForceRangesOptions.forceLower4.ToString();
                            tfFrom4.Foreground = Brushes.White;
                            tfFrom4.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("forceUpper4"))
                        {
                            ForceRangesOptions.forceUpper4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfTo4.Text = ForceRangesOptions.forceUpper4.ToString();
                            tfTo4.Foreground = Brushes.White;
                            tfTo4.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnMultiple4"))
                        {
                            ForceRangesOptions.nutnMultiple4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceMultiple4.Text = ForceRangesOptions.nutnMultiple4.ToString();
                            tfCalForceMultiple4.Foreground = Brushes.White;
                            tfCalForceMultiple4.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnDivide4"))
                        {
                            ForceRangesOptions.nutnDivide4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceDivide4.Text = ForceRangesOptions.nutnDivide4.ToString();
                            tfCalForceDivide4.Foreground = Brushes.White;
                            tfCalForceDivide4.Background = Brushes.Black;
                        }




                        if (textReader.Name.Equals("forceLower5"))
                        {
                            ForceRangesOptions.forceLower5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfFrom5.Text = ForceRangesOptions.forceLower5.ToString();
                            tfFrom5.Foreground = Brushes.White;
                            tfFrom5.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("forceUpper5"))
                        {
                            ForceRangesOptions.forceUpper5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfTo5.Text = ForceRangesOptions.forceUpper5.ToString();
                            tfTo5.Foreground = Brushes.White;
                            tfTo5.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnMultiple5"))
                        {
                            ForceRangesOptions.nutnMultiple5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceMultiple5.Text = ForceRangesOptions.nutnMultiple5.ToString();
                            tfCalForceMultiple5.Foreground = Brushes.White;
                            tfCalForceMultiple5.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnDivide5"))
                        {
                            ForceRangesOptions.nutnDivide5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceDivide5.Text = ForceRangesOptions.nutnDivide5.ToString();
                            tfCalForceDivide5.Foreground = Brushes.White;
                            tfCalForceDivide5.Background = Brushes.Black;
                        }




                        if (textReader.Name.Equals("forceLower6"))
                        {
                            ForceRangesOptions.forceLower6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfFrom6.Text = ForceRangesOptions.forceLower6.ToString();
                            tfFrom6.Foreground = Brushes.White;
                            tfFrom6.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("forceUpper6"))
                        {
                            ForceRangesOptions.forceUpper6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfTo6.Text = ForceRangesOptions.forceUpper6.ToString();
                            tfTo6.Foreground = Brushes.White;
                            tfTo6.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnMultiple6"))
                        {
                            ForceRangesOptions.nutnMultiple6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceMultiple6.Text = ForceRangesOptions.nutnMultiple6.ToString();
                            tfCalForceMultiple6.Foreground = Brushes.White;
                            tfCalForceMultiple6.Background = Brushes.Black;
                        }
                        if (textReader.Name.Equals("nutnDivide6"))
                        {
                            ForceRangesOptions.nutnDivide6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            tfCalForceDivide6.Text = ForceRangesOptions.nutnDivide6.ToString();
                            tfCalForceDivide6.Foreground = Brushes.White;
                            tfCalForceDivide6.Background = Brushes.Black;
                        }

                        if (textReader.Name.Equals("forceLowerCurrent"))
                        {
                            ForceRangesOptions.forceLowerCurrent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());                           
                        }

                        if (textReader.Name.Equals("forceUpperCurrent"))
                        {
                            ForceRangesOptions.forceUpperCurrent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());                           
                        }


                        if (textReader.Name.Equals("rangeChosen"))
                        {
                          ForceRangesOptions.rangeChosen = textReader.ReadElementContentAsInt();
                          switch (ForceRangesOptions.rangeChosen)
                          {
                              case 1: {
                                  if (forceRange1.IsEnabled == true)
                                  {
                                      forceRange1.IsChecked = true;
                                  }
                                  break;
                               }

                              case 2:
                                  {
                                      if (forceRange2.IsEnabled == true)
                                      {
                                          forceRange2.IsChecked = true;
                                      }
                                      break;
                                  }


                              case 3:
                                  {
                                      if (forceRange3.IsEnabled == true)
                                      {
                                          forceRange3.IsChecked = true;
                                      }
                                      break;
                                  }

                              case 4:
                                  {
                                      if (forceRange4.IsEnabled == true)
                                      {
                                          forceRange4.IsChecked = true;
                                      }
                                      break;
                                  }

                              case 5:
                                  {
                                      if (forceRange5.IsEnabled == true)
                                      {
                                          forceRange5.IsChecked = true;
                                      }
                                      break;
                                  }

                              case 6:
                                  {
                                      if (forceRange6.IsEnabled == true)
                                      {
                                          forceRange6.IsChecked = true;
                                      }
                                      break;
                                  }
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

        private void writeXMLFileForceRanges()
        {
            try
            {
                //write in xml file

                XElement xmlRoot = new XElement("ForceRangesOptions",
                                                new XElement("ForceRangesCurrentOptions",
                                                            new XElement("forceLower1", ForceRangesOptions.forceLower1.ToString()),
                                                            new XElement("forceUpper1", ForceRangesOptions.forceUpper1.ToString()),
                                                            new XElement("nutnMultiple1", ForceRangesOptions.nutnMultiple1.ToString()),
                                                            new XElement("nutnDivide1", ForceRangesOptions.nutnDivide1.ToString()),
                                                           
                                                            new XElement("forceLower2", ForceRangesOptions.forceLower2.ToString()),
                                                            new XElement("forceUpper2", ForceRangesOptions.forceUpper2.ToString()),
                                                            new XElement("nutnMultiple2", ForceRangesOptions.nutnMultiple2.ToString()),
                                                            new XElement("nutnDivide2", ForceRangesOptions.nutnDivide2.ToString()),

                                                            new XElement("forceLower3", ForceRangesOptions.forceLower3.ToString()),
                                                            new XElement("forceUpper3", ForceRangesOptions.forceUpper3.ToString()),
                                                            new XElement("nutnMultiple3", ForceRangesOptions.nutnMultiple3.ToString()),
                                                            new XElement("nutnDivide3", ForceRangesOptions.nutnDivide3.ToString()),

                                                            new XElement("forceLower4", ForceRangesOptions.forceLower4.ToString()),
                                                            new XElement("forceUpper4", ForceRangesOptions.forceUpper4.ToString()),
                                                            new XElement("nutnMultiple4", ForceRangesOptions.nutnMultiple4.ToString()),
                                                            new XElement("nutnDivide4", ForceRangesOptions.nutnDivide4.ToString()),

                                                            new XElement("forceLower5", ForceRangesOptions.forceLower5.ToString()),
                                                            new XElement("forceUpper5", ForceRangesOptions.forceUpper5.ToString()),
                                                            new XElement("nutnMultiple5", ForceRangesOptions.nutnMultiple5.ToString()),
                                                            new XElement("nutnDivide5", ForceRangesOptions.nutnDivide5.ToString()),

                                                            new XElement("forceLower6", ForceRangesOptions.forceLower6.ToString()),
                                                            new XElement("forceUpper6", ForceRangesOptions.forceUpper6.ToString()),
                                                            new XElement("nutnMultiple6", ForceRangesOptions.nutnMultiple6.ToString()),
                                                            new XElement("nutnDivide6", ForceRangesOptions.nutnDivide6.ToString()),

                                                            new XElement("rangeChosen", ForceRangesOptions.rangeChosen.ToString()),
                                                            new XElement("forceLowerCurrent", ForceRangesOptions.forceLowerCurrent.ToString()),
                                                            new XElement("forceUpperCurrent", ForceRangesOptions.forceUpperCurrent.ToString())
                                                            )
                                                );

                xmlRoot.Save(Constants.ForceRangesonlineOptions);                
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void writeXMLFile()}", System.DateTime.Now);
            }
        }

        private void isSomethingEmptySetToDefaultValues() 
        {
            if (tfFrom1.Text.Equals(string.Empty) || tfTo1.Text.Equals(string.Empty) || tfCalForceDivide1.Text.Equals(string.Empty) || tfCalForceMultiple1.Text.Equals(string.Empty))
            {
                ForceRangesOptions.forceLower1 = 0;
                ForceRangesOptions.forceUpper1 = 0;
                ForceRangesOptions.nutnDivide1 = 10;
                ForceRangesOptions.nutnMultiple1 = 10;
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            isSomethingEmptySetToDefaultValues();

            writeXMLFileForceRanges();



            onlineMode.OptionsOnline.tfCalForceDivide.Foreground = Brushes.White;
            onlineMode.OptionsOnline.tfCalForceMultiple.Foreground = Brushes.White;
            onlineMode.OptionsOnline.WriteXMLOnlineFile();
        }

        private void forceRange1_Checked(object sender, RoutedEventArgs e)
        {
            ForceRangesOptions.rangeChosen = 1;
            onlineMode.OptionsOnline.tfCalForceDivide.Text = tfCalForceDivide1.Text;
            onlineMode.OptionsOnline.tfCalForceMultiple.Text = tfCalForceMultiple1.Text;


            //konvertuj vrednosti textboxova u brojeve i zapamti u online opcijama
            double tempLower = 0;
            double tempUpper = 0;
            double tempNutnDivide = 10;
            double tempNutnMultiple = 10;
            bool isN = double.TryParse(tfCalForceDivide1.Text, out tempNutnDivide);
            bool isN2 = double.TryParse(tfCalForceMultiple1.Text, out tempNutnMultiple);
            bool isN3 = double.TryParse(tfFrom1.Text, out tempLower);
            bool isN4 = double.TryParse(tfTo1.Text, out tempUpper);

            if (isN == true && isN2 == true && isN3 == true && isN4 == true)
            {
                OptionsInOnlineMode.nutnDivide = tempNutnDivide;
                OptionsInOnlineMode.nutnMultiple = tempNutnMultiple;
                ForceRangesOptions.forceLowerCurrent = tempLower;
                ForceRangesOptions.forceUpperCurrent = tempUpper;
                Constants.TRENUTNIOPSEGSILE_ONLINE = "Trenutni opseg se krece od " + ForceRangesOptions.forceLowerCurrent + " [kN] do " + ForceRangesOptions.forceUpperCurrent + " [kN]";
                onlineMode.OptionsOnline.lblOpseg.Content = Constants.TRENUTNIOPSEGSILE_ONLINE;
            }
            else
            {
                MessageBox.Show("Nisu ispravno ueti svi parametri opsega 1 !!!");
            }
            writeXMLFileForceRanges();
        }

        private void forceRange2_Checked(object sender, RoutedEventArgs e)
        {
            ForceRangesOptions.rangeChosen = 2;
            onlineMode.OptionsOnline.tfCalForceDivide.Text = tfCalForceDivide2.Text;
            onlineMode.OptionsOnline.tfCalForceMultiple.Text = tfCalForceMultiple2.Text;

            //konvertuj vrednosti textboxova u brojeve i zapamti u online opcijama
            double tempLower = 0;
            double tempUpper = 0;
            double tempNutnDivide = 10;
            double tempNutnMultiple = 10;
            bool isN = double.TryParse(tfCalForceDivide2.Text, out tempNutnDivide);
            bool isN2 = double.TryParse(tfCalForceMultiple2.Text, out tempNutnMultiple);
            bool isN3 = double.TryParse(tfFrom2.Text, out tempLower);
            bool isN4 = double.TryParse(tfTo2.Text, out tempUpper);

            if (isN == true && isN2 == true && isN3 == true && isN4 == true)
            {
                OptionsInOnlineMode.nutnDivide = tempNutnDivide;
                OptionsInOnlineMode.nutnMultiple = tempNutnMultiple;
                ForceRangesOptions.forceLowerCurrent = tempLower;
                ForceRangesOptions.forceUpperCurrent = tempUpper;
                Constants.TRENUTNIOPSEGSILE_ONLINE = "Trenutni opseg se krece od " + ForceRangesOptions.forceLowerCurrent + " [kN] do " + ForceRangesOptions.forceUpperCurrent + " [kN]";
                onlineMode.OptionsOnline.lblOpseg.Content = Constants.TRENUTNIOPSEGSILE_ONLINE;
            }
            else
            {
                MessageBox.Show("Nisu ispravno ueti svi parametri opsega 2 !!!");
            }
            writeXMLFileForceRanges();
        }

        private void forceRange3_Checked(object sender, RoutedEventArgs e)
        {
            ForceRangesOptions.rangeChosen = 3;
            onlineMode.OptionsOnline.tfCalForceDivide.Text = tfCalForceDivide3.Text;
            onlineMode.OptionsOnline.tfCalForceMultiple.Text = tfCalForceMultiple3.Text;

            //konvertuj vrednosti textboxova u brojeve i zapamti u online opcijama
            double tempLower = 0;
            double tempUpper = 0;
            double tempNutnDivide = 10;
            double tempNutnMultiple = 10;
            bool isN = double.TryParse(tfCalForceDivide3.Text, out tempNutnDivide);
            bool isN2 = double.TryParse(tfCalForceMultiple3.Text, out tempNutnMultiple);
            bool isN3 = double.TryParse(tfFrom3.Text, out tempLower);
            bool isN4 = double.TryParse(tfTo3.Text, out tempUpper);

            if (isN == true && isN2 == true && isN3 == true && isN4 == true)
            {
                OptionsInOnlineMode.nutnDivide = tempNutnDivide;
                OptionsInOnlineMode.nutnMultiple = tempNutnMultiple;
                ForceRangesOptions.forceLowerCurrent = tempLower;
                ForceRangesOptions.forceUpperCurrent = tempUpper;
                Constants.TRENUTNIOPSEGSILE_ONLINE = "Trenutni opseg se krece od " + ForceRangesOptions.forceLowerCurrent + " [kN] do " + ForceRangesOptions.forceUpperCurrent + " [kN]";
                onlineMode.OptionsOnline.lblOpseg.Content = Constants.TRENUTNIOPSEGSILE_ONLINE;
            }
            else
            {
                MessageBox.Show("Nisu ispravno ueti svi parametri opsega 3 !!!");
            }
            writeXMLFileForceRanges();
        }

        private void forceRange4_Checked(object sender, RoutedEventArgs e)
        {
            ForceRangesOptions.rangeChosen = 4;
            onlineMode.OptionsOnline.tfCalForceDivide.Text = tfCalForceDivide4.Text;
            onlineMode.OptionsOnline.tfCalForceMultiple.Text = tfCalForceMultiple4.Text;

            //konvertuj vrednosti textboxova u brojeve i zapamti u online opcijama
            double tempLower = 0;
            double tempUpper = 0;
            double tempNutnDivide = 10;
            double tempNutnMultiple = 10;
            bool isN = double.TryParse(tfCalForceDivide4.Text, out tempNutnDivide);
            bool isN2 = double.TryParse(tfCalForceMultiple4.Text, out tempNutnMultiple);
            bool isN3 = double.TryParse(tfFrom4.Text, out tempLower);
            bool isN4 = double.TryParse(tfTo4.Text, out tempUpper);

            if (isN == true && isN2 == true && isN3 == true && isN4 == true)
            {
                OptionsInOnlineMode.nutnDivide = tempNutnDivide;
                OptionsInOnlineMode.nutnMultiple = tempNutnMultiple;
                ForceRangesOptions.forceLowerCurrent = tempLower;
                ForceRangesOptions.forceUpperCurrent = tempUpper;
                Constants.TRENUTNIOPSEGSILE_ONLINE = "Trenutni opseg se krece od " + ForceRangesOptions.forceLowerCurrent + " [kN] do " + ForceRangesOptions.forceUpperCurrent + " [kN]";
                onlineMode.OptionsOnline.lblOpseg.Content = Constants.TRENUTNIOPSEGSILE_ONLINE;
            }
            else
            {
                MessageBox.Show("Nisu ispravno ueti svi parametri opsega 4 !!!");
            }
            writeXMLFileForceRanges();
        }

        private void forceRange5_Checked(object sender, RoutedEventArgs e)
        {
            ForceRangesOptions.rangeChosen = 5;
            onlineMode.OptionsOnline.tfCalForceDivide.Text = tfCalForceDivide5.Text;
            onlineMode.OptionsOnline.tfCalForceMultiple.Text = tfCalForceMultiple5.Text;

            //konvertuj vrednosti textboxova u brojeve i zapamti u online opcijama
            double tempLower = 0;
            double tempUpper = 0;
            double tempNutnDivide = 10;
            double tempNutnMultiple = 10;
            bool isN = double.TryParse(tfCalForceDivide5.Text, out tempNutnDivide);
            bool isN2 = double.TryParse(tfCalForceMultiple5.Text, out tempNutnMultiple);
            bool isN3 = double.TryParse(tfFrom5.Text, out tempLower);
            bool isN4 = double.TryParse(tfTo5.Text, out tempUpper);

            if (isN == true && isN2 == true && isN3 == true && isN4 == true)
            {
                OptionsInOnlineMode.nutnDivide = tempNutnDivide;
                OptionsInOnlineMode.nutnMultiple = tempNutnMultiple;
                ForceRangesOptions.forceLowerCurrent = tempLower;
                ForceRangesOptions.forceUpperCurrent = tempUpper;
                Constants.TRENUTNIOPSEGSILE_ONLINE = "Trenutni opseg se krece od " + ForceRangesOptions.forceLowerCurrent + " [kN] do " + ForceRangesOptions.forceUpperCurrent + " [kN]";
                onlineMode.OptionsOnline.lblOpseg.Content = Constants.TRENUTNIOPSEGSILE_ONLINE;
            }
            else
            {
                MessageBox.Show("Nisu ispravno ueti svi parametri opsega 5 !!!");
            }
            writeXMLFileForceRanges();
        }

        private void forceRange6_Checked(object sender, RoutedEventArgs e)
        {
            ForceRangesOptions.rangeChosen = 6;
            onlineMode.OptionsOnline.tfCalForceDivide.Text = tfCalForceDivide6.Text;
            onlineMode.OptionsOnline.tfCalForceMultiple.Text = tfCalForceMultiple6.Text;

            //konvertuj vrednosti textboxova u brojeve i zapamti u online opcijama
            double tempLower = 0;
            double tempUpper = 0;
            double tempNutnDivide = 10;
            double tempNutnMultiple = 10;
            bool isN = double.TryParse(tfCalForceDivide6.Text, out tempNutnDivide);
            bool isN2 = double.TryParse(tfCalForceMultiple6.Text, out tempNutnMultiple);
            bool isN3 = double.TryParse(tfFrom6.Text, out tempLower);
            bool isN4 = double.TryParse(tfTo6.Text, out tempUpper);

            if (isN == true && isN2 == true && isN3 == true && isN4 == true)
            {
                OptionsInOnlineMode.nutnDivide = tempNutnDivide;
                OptionsInOnlineMode.nutnMultiple = tempNutnMultiple;
                ForceRangesOptions.forceLowerCurrent = tempLower;
                ForceRangesOptions.forceUpperCurrent = tempUpper;
                Constants.TRENUTNIOPSEGSILE_ONLINE = "Trenutni opseg se krece od " + ForceRangesOptions.forceLowerCurrent + " [kN] do " + ForceRangesOptions.forceUpperCurrent + " [kN]";
                onlineMode.OptionsOnline.lblOpseg.Content = Constants.TRENUTNIOPSEGSILE_ONLINE;
            }
            else
            {
                MessageBox.Show("Nisu ispravno ueti svi parametri opsega 6 !!!");
            }
            writeXMLFileForceRanges();
        }



        #region tfTo1


        private void tfTo1_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                tfTo1.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void tfTo1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTo1_Online();
                    markSavedOnlineOptionsAsBlacktfTo1_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfTo1_Online()
        {
            try
            {
                //tfTo1.Foreground = Brushes.Black;
                tfTo1.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfTo1_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfTo1.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu gornjeg opsega sile br.1!");
                }
                else
                {
                    ForceRangesOptions.forceUpper1 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }



        #endregion

        #region tfFrom1

        private void tfFrom1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfFrom1.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfFrom1_Online()
        {
            try
            {
                //tfFrom1.Foreground = Brushes.Black;
                tfFrom1.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
               
            }
        }

        private void saveOptionstfFrom1_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfFrom1.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu donjeg opsega sile br.1!");
                }
                else
                {
                    ForceRangesOptions.forceLower1 = forceDiv;                    
                }


                writeXMLFileForceRanges();              
            }
            catch (Exception ex)
            {
                
            }
        }

        private void tfFrom1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFrom1_Online();
                    markSavedOnlineOptionsAsBlacktfFrom1_Online();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        #endregion

        #region tfCalForceDivide1

        private void tfCalForceDivide1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide1.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceDivide1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceDivide1_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide1_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide1_Online()
        {
            try
            {
                //tfCalForceDivide1.Foreground = Brushes.Black;
                tfCalForceDivide1.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceDivide1_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide1.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje delioca sile sile br.1!");
                }
                else
                {
                    ForceRangesOptions.nutnDivide1 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceMultiple1

        private void tfCalForceMultiple1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple1.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceMultiple1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceMultiple1_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple1_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple1_Online()
        {
            try
            {
                //tfCalForceMultiple1.Foreground = Brushes.Black;
                tfCalForceMultiple1.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceMultiple1_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceMultiple1.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje mnozioca sile sile br.1!");
                }
                else
                {
                    ForceRangesOptions.nutnMultiple1 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfFrom2

        private void tfFrom2_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                tfFrom2.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfFrom2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFrom2_Online();
                    markSavedOnlineOptionsAsBlacktfFrom2_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfFrom2_Online()
        {
            try
            {
                //tfFrom2.Foreground = Brushes.Black;
                tfFrom2.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfFrom2_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfFrom2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu donjeg opsega sile br.2!");
                }
                else
                {
                    ForceRangesOptions.forceLower2 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfTo2

        private void tfTo2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfTo2.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfTo2_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTo2_Online();
                    markSavedOnlineOptionsAsBlacktfTo2_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfTo2_Online()
        {
            try
            {
                //tfTo2.Foreground = Brushes.Black;
                tfTo2.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfTo2_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfTo2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu gornjeg opsega sile br.2!");
                }
                else
                {
                    ForceRangesOptions.forceUpper2 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceDivide2

        private void tfCalForceDivide2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide2.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceDivide2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceDivide2_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide2_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide2_Online()
        {
            try
            {
                //tfCalForceDivide2.Foreground = Brushes.Black;
                tfCalForceDivide2.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceDivide2_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje delioca sile sile br.2!");
                }
                else
                {
                    ForceRangesOptions.nutnDivide2 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceMultiple2

        private void tfCalForceMultiple2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple2.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceMultiple2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceMultiple2_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple2_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple2_Online()
        {
            try
            {
                //tfCalForceMultiple2.Foreground = Brushes.Black;
                tfCalForceMultiple2.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceMultiple2_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceMultiple2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje mnozioca sile sile br.2!");
                }
                else
                {
                    ForceRangesOptions.nutnMultiple2 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfFrom3

        private void tfFrom3_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                tfFrom3.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfFrom3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFrom3_Online();
                    markSavedOnlineOptionsAsBlacktfFrom3_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfFrom3_Online()
        {
            try
            {
                //tfFrom3.Foreground = Brushes.Black;
                tfFrom3.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfFrom3_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfFrom3.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu donjeg opsega sile br.3!");
                }
                else
                {
                    ForceRangesOptions.forceLower3 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfTo3

        private void tfTo3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfTo3.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfTo3_KeyDown(object sender, KeyEventArgs e)
        {

            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTo3_Online();
                    markSavedOnlineOptionsAsBlacktfTo3_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfTo3_Online()
        {
            try
            {
                //tfTo3.Foreground = Brushes.Black;
                tfTo3.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfTo3_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfTo3.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu gornjeg opsega sile br.3!");
                }
                else
                {
                    ForceRangesOptions.forceUpper3 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceDivide3

        private void tfCalForceDivide3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide3.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceDivide3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceDivide3_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide3_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide3_Online()
        {
            try
            {
                //tfCalForceDivide3.Foreground = Brushes.Black;
                tfCalForceDivide3.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceDivide3_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide3.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje delioca sile sile br.3!");
                }
                else
                {
                    ForceRangesOptions.nutnDivide3 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceMultiple3

        private void tfCalForceMultiple3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple3.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceMultiple3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceMultiple3_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple3_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple3_Online()
        {
            try
            {
                //tfCalForceMultiple3.Foreground = Brushes.Black;
                tfCalForceMultiple3.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceMultiple3_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceMultiple3.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje mnozioca sile sile br.3!");
                }
                else
                {
                    ForceRangesOptions.nutnMultiple3 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfFrom4

        private void tfFrom4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfFrom4.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfFrom4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFrom4_Online();
                    markSavedOnlineOptionsAsBlacktfFrom4_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfFrom4_Online()
        {
            try
            {
                //tfFrom4.Foreground = Brushes.Black;
                tfFrom4.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfFrom4_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfFrom4.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu donjeg opsega sile br.4!");
                }
                else
                {
                    ForceRangesOptions.forceLower4 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfTo4

        private void tfTo4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfTo4.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfTo4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTo4_Online();
                    markSavedOnlineOptionsAsBlacktfTo4_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfTo4_Online()
        {
            try
            {
                //tfTo4.Foreground = Brushes.Black;
                tfTo4.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfTo4_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfTo4.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu gornjeg opsega sile br.4!");
                }
                else
                {
                    ForceRangesOptions.forceUpper4 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceDivide4

        private void tfCalForceDivide4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide4.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceDivide4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceDivide4_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide4_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfCalForceDivide4_Online()
        {
            try
            {
                //tfCalForceDivide4.Foreground = Brushes.Black;
                tfCalForceDivide4.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceDivide4_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide4.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje delioca sile sile br.4!");
                }
                else
                {
                    ForceRangesOptions.nutnDivide4 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceMultiple4

        private void tfCalForceMultiple4_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple4.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceMultiple4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceMultiple4_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple4_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple4_Online()
        {
            try
            {
                //tfCalForceMultiple4.Foreground = Brushes.Black;
                tfCalForceMultiple4.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceMultiple4_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceMultiple4.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje mnozioca sile sile br.4!");
                }
                else
                {
                    ForceRangesOptions.nutnMultiple4 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfFrom5

        private void tfFrom5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfFrom5.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfFrom5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFrom5_Online();
                    markSavedOnlineOptionsAsBlacktfFrom5_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfFrom5_Online()
        {
            try
            {
                //tfFrom5.Foreground = Brushes.Black;
                tfFrom5.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfFrom5_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfFrom5.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu donjeg opsega sile br.5!");
                }
                else
                {
                    ForceRangesOptions.forceLower5 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfTo5

        private void tfTo5_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                tfTo5.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfTo5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTo5_Online();
                    markSavedOnlineOptionsAsBlacktfTo5_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfTo5_Online()
        {
            try
            {
                //tfTo5.Foreground = Brushes.Black;
                tfTo5.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfTo5_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfTo5.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu gornjeg opsega sile br.5!");
                }
                else
                {
                    ForceRangesOptions.forceUpper5 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceDivide5

        private void tfCalForceDivide5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide5.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceDivide5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceDivide5_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide5_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide5_Online()
        {
            try
            {
                //tfCalForceDivide5.Foreground = Brushes.Black;
                tfCalForceDivide5.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceDivide5_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide5.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje delioca sile sile br.5!");
                }
                else
                {
                    ForceRangesOptions.nutnDivide5 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceMultiple5

        private void tfCalForceMultiple5_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple5.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceMultiple5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceMultiple5_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple5_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple5_Online()
        {
            try
            {
                //tfCalForceMultiple5.Foreground = Brushes.Black;
                tfCalForceMultiple5.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceMultiple5_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceMultiple5.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje mnozioca sile sile br.5!");
                }
                else
                {
                    ForceRangesOptions.nutnMultiple5 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfFrom6

        private void tfFrom6_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfFrom6.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfFrom6_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFrom6_Online();
                    markSavedOnlineOptionsAsBlacktfFrom6_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfFrom6_Online()
        {
            try
            {
                //tfFrom6.Foreground = Brushes.Black;
                tfFrom6.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfFrom6_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfFrom6.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu donjeg opsega sile br.6!");
                }
                else
                {
                    ForceRangesOptions.forceLower6 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfTo6

        private void tfTo6_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                tfTo6.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfTo6_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTo6_Online();
                    markSavedOnlineOptionsAsBlacktfTo6_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }


        private void markSavedOnlineOptionsAsBlacktfTo6_Online()
        {
            try
            {
                //tfTo6.Foreground = Brushes.Black;
                tfTo6.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptionstfTo6_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfTo6.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u granicu gornjeg opsega sile br.6!");
                }
                else
                {
                    ForceRangesOptions.forceUpper6 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceDivide6

        private void tfCalForceDivide6_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide6.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceDivide6_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceDivide6_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide6_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide6_Online()
        {
            try
            {
                //tfCalForceDivide6.Foreground = Brushes.Black;
                tfCalForceDivide6.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceDivide6_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide6.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje delioca sile sile br.6!");
                }
                else
                {
                    ForceRangesOptions.nutnDivide6 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region tfCalForceMultiple6

        private void tfCalForceMultiple6_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple6.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {

            }
        }

        private void tfCalForceMultiple6_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptiontfCalForceMultiple6_Online();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple6_Online();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple6_Online()
        {
            try
            {
                //tfCalForceMultiple6.Foreground = Brushes.Black;
                tfCalForceMultiple6.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void saveOptiontfCalForceMultiple6_Online()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceMultiple6.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    MessageBox.Show("Trebate uneti broj u polje mnozioca sile sile br.6!");
                }
                else
                {
                    ForceRangesOptions.nutnMultiple6 = forceDiv;
                }


                writeXMLFileForceRanges();
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}
