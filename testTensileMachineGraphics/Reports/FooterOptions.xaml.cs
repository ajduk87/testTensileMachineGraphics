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
using testTensileMachineGraphics.Options;
using System.Xml;

namespace testTensileMachineGraphics.Reports
{
    /// <summary>
    /// Interaction logic for FooterOptions.xaml
    /// </summary>
    public partial class FooterOptions : Window
    {

        public static void LoadOptionsFooteriNITIAL()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.FooterOptions);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {


                        if (textReader.Name.Equals("brojObrasca"))
                        {
                            OptionsFooter.brojObrasca = textReader.ReadElementContentAsString();
                           
                        }

                        if (textReader.Name.Equals("brojVerzije"))
                        {
                            OptionsFooter.brojVerzije = textReader.ReadElementContentAsString();
                            
                        }

                        if (textReader.Name.Equals("Godina"))
                        {
                            OptionsFooter.Godina = textReader.ReadElementContentAsString();
                        
                        }


                    } //end of  if (nType == XmlNodeType.Element)
                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {

            }
        }

        public void LoadOptionsFooter()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.FooterOptions);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {


                        if (textReader.Name.Equals("brojObrasca"))
                        {
                            OptionsFooter.brojObrasca = textReader.ReadElementContentAsString();
                            tfBrojObrasca.Text = OptionsFooter.brojObrasca;
                            tfBrojObrasca.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("brojVerzije"))
                        {
                            OptionsFooter.brojVerzije = textReader.ReadElementContentAsString();
                            tfBrojVerzije.Text = OptionsFooter.brojVerzije;
                            tfBrojVerzije.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("Godina"))
                        {
                            OptionsFooter.Godina = textReader.ReadElementContentAsString();
                            tfGodina.Text = OptionsFooter.Godina;
                            tfGodina.Foreground = Brushes.White;
                        }
                                               

                    } //end of  if (nType == XmlNodeType.Element)
                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
               
            }
        }



        public FooterOptions()
        {
            InitializeComponent();

            LoadOptionsFooter();
        }


        private void markSavedBrojObrasca()
        {
            try
            {
                //tfFrom1.Foreground = Brushes.Black;
                tfBrojObrasca.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedBrojVerzije()
        {
            try
            {
                //tfFrom1.Foreground = Brushes.Black;
                tfBrojVerzije.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }

        private void markSavedGodina()
        {
            try
            {
                //tfFrom1.Foreground = Brushes.Black;
                tfGodina.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {

            }
        }


        private void saveBrojObrasca()
        {
            try
            {

                OptionsFooter.brojObrasca = tfBrojObrasca.Text;

                writeXMLFileFooterOptions();
            }
            catch (Exception ex)
            {

            }
        }

        private void saveBrojVerzije()
        {
            try
            {

                OptionsFooter.brojVerzije = tfBrojVerzije.Text;

                writeXMLFileFooterOptions();
            }
            catch (Exception ex)
            {

            }
        }

        private void saveGodina()
        {
            try
            {

                OptionsFooter.Godina = tfGodina.Text;

                writeXMLFileFooterOptions();
            }
            catch (Exception ex)
            {

            }
        }



        private void tfBrojObrasca_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveBrojObrasca();
                    markSavedBrojObrasca();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void tfBrojVerzije_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveBrojVerzije();
                    markSavedBrojVerzije();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void tfGodina_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveGodina();
                    markSavedGodina();
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void writeXMLFileFooterOptions()
        {
            try
            {
                //write in xml file

                XElement xmlRoot = new XElement("FooterOptions",
                                                new XElement("FooterCurrentOptions",
                                                            new XElement("brojObrasca", OptionsFooter.brojObrasca.ToString()),
                                                            new XElement("brojVerzije", OptionsFooter.brojVerzije.ToString()),
                                                            new XElement("Godina", OptionsFooter.Godina.ToString())                                                            
                                                ));

                xmlRoot.Save(Constants.FooterOptions);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsOnline.xaml.cs] {private void writeXMLFile()}", System.DateTime.Now);
            }
        }

        private void tfBrojObrasca_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfBrojObrasca.Foreground = Brushes.Red;
        }

        private void tfBrojVerzije_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfBrojVerzije.Foreground = Brushes.Red;
        }

        private void tfGodina_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfGodina.Foreground = Brushes.Red;
        }
    }
}
