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
using testTensileMachineGraphics.Reports;
using System.Xml;
using System.IO;

namespace testTensileMachineGraphics.Windows
{
    /// <summary>
    /// Interaction logic for WindowForNManualCalculation.xaml
    /// </summary>
    public partial class WindowForNManualCalculation : Window
    {
        private double xconst = 1150 - 80 - 280;
        //private double xconst = 1250 + 105 - 300;
        private double yconst = 500;


        private GraphicPlotting plotting;
        private PrintScreen printScreen; 

  


        public WindowForNManualCalculation(GraphicPlotting pl, PrintScreen pS)
        {
            try
            {
                InitializeComponent();

                plotting = pl;
                printScreen = pS;
                printScreen.IsNManualWindowCreated = true;
                //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                if (pl.Ag < OptionsInPlottingMode.EndIntervalForN)
                {
                    //ovde u prozoru ispisi da je Ag manje od OptionsInPlottingMode.EndIntervalFor
                    return;
                }

                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = xconst;
                this.Top = yconst;


                if (plotting.Fs_FittingForManualNProperty != null && plotting.DeltaLsInProcForManualNProperty != null && plotting.PreassureForNManualProperty != null)
                {

                    //set textblocks
                    if (plotting.Fs_FittingForManualNProperty.Count > 0 && plotting.DeltaLsInProcForManualNProperty.Count > 0 && plotting.NManual > 0.0 && plotting.PreassureForNManualProperty.Count > 0)
                    {
                        plotting.NManual = Math.Round(plotting.NManual, 4);
                        plotting.PreassureForNManualProperty[0] = Math.Round(plotting.PreassureForNManualProperty[0], 0);
                        plotting.PreassureForNManualProperty[1] = Math.Round(plotting.PreassureForNManualProperty[1], 0);
                        plotting.PreassureForNManualProperty[2] = Math.Round(plotting.PreassureForNManualProperty[2], 0);
                        plotting.PreassureForNManualProperty[3] = Math.Round(plotting.PreassureForNManualProperty[3], 0);
                        plotting.PreassureForNManualProperty[4] = Math.Round(plotting.PreassureForNManualProperty[4], 0);

                        LastInputOutputSavedData.R1 = plotting.PreassureForNManualProperty[0].ToString();
                        LastInputOutputSavedData.R2 = plotting.PreassureForNManualProperty[1].ToString();
                        LastInputOutputSavedData.R3 = plotting.PreassureForNManualProperty[2].ToString();
                        LastInputOutputSavedData.R4 = plotting.PreassureForNManualProperty[3].ToString();
                        LastInputOutputSavedData.R5 = plotting.PreassureForNManualProperty[4].ToString();
                        LastInputOutputSavedData.Manualn = plotting.NManual.ToString();


                        string myXmlString = String.Empty;
                        //List<string> myXmlStrings = File.ReadAllLines(Constants.sampleReportFilepath).ToList();
                        List<string> myXmlStrings = File.ReadAllLines(Properties.Settings.Default.sampleReportFilepath).ToList();
                        if (myXmlStrings.Count == 0)
                        {
                            return;
                        }
                        foreach (string s in myXmlStrings)
                        {
                            myXmlString += s;
                        }

                        if (myXmlString.Contains(Constants.XML_roots_ROOT) == false)
                        {
                            MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                            return;
                        }

                        XmlDocument xml = new XmlDocument();
                        xml.LoadXml(myXmlString);
                        XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);

                        foreach (XmlNode xn in xnList)
                        {
                            xn[Constants.XML_R1].InnerText = LastInputOutputSavedData.R1;
                            xn[Constants.XML_R2].InnerText = LastInputOutputSavedData.R2;
                            xn[Constants.XML_R3].InnerText = LastInputOutputSavedData.R3;
                            xn[Constants.XML_R4].InnerText = LastInputOutputSavedData.R4;
                            xn[Constants.XML_R5].InnerText = LastInputOutputSavedData.R5;
                            xn[Constants.XML_manualN].InnerText = LastInputOutputSavedData.Manualn;
                            xn[Constants.XML_manualN_BeginInterval].InnerText = OptionsInPlottingMode.BeginIntervalForN.ToString();
                            xn[Constants.XML_manualN_EndInterval].InnerText = OptionsInPlottingMode.EndIntervalForN.ToString();
                        }

                        //xml.Save(Constants.sampleReportFilepath);
                        xml.Save(Properties.Settings.Default.sampleReportFilepath);



                        lblA1.Text = "A" + "\x2081 = " + OptionsInPlottingMode.BeginIntervalForN + " %";
                        lblA2.Text = "A" + "\x2082 = " + OptionsInPlottingMode.EndIntervalForN + " %";
                        lblF1.Text = "R" + "\x2081 = " + plotting.PreassureForNManualProperty[0] + " MPa";
                        lblF2.Text = "R" + "\x2082 = " + plotting.PreassureForNManualProperty[1] + " MPa";
                        lblF3.Text = "R" + "\x2083 = " + plotting.PreassureForNManualProperty[2] + " MPa";
                        lblF4.Text = "R" + "\x2084 = " + plotting.PreassureForNManualProperty[3] + " MPa";
                        lblF5.Text = "R" + "\x2085 = " + plotting.PreassureForNManualProperty[4] + " MPa";
                        lblNmanual.Text = "Nrucno = " + plotting.NManual;

                        LastInputOutputSavedData.Manualn = plotting.NManual.ToString();
                    }
                    else
                    {
                        resetLabels();
                    }
                }
                else
                {
                    resetLabels();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForNManualCalculation.xaml.cs] {public WindowForNManualCalculation(GraphicPlotting pl, PrintScreen pS)}", System.DateTime.Now);
            }
        }


        private void resetLabels() 
        {
            try
            {
                lblA1.Text = "A" + "\x2081 = ";
                lblA2.Text = "A" + "\x2082 = ";
                lblF1.Text = "R" + "\x2081 = ";
                lblF2.Text = "R" + "\x2082 = ";
                lblF3.Text = "R" + "\x2083 = ";
                lblF4.Text = "R" + "\x2084 = ";
                lblF5.Text = "R" + "\x2085 = ";
                lblNmanual.Text = "Nrucno = ";
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForNManualCalculation.xaml.cs] {private void resetLabels()}", System.DateTime.Now);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                printScreen.chbCalculateNManual.IsChecked = false;
                printScreen.IsNManualWindowCreated = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[WindowForNManualCalculation.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

    }
}
