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
using System.IO;
using testTensileMachineGraphics.PointViewModel;
using testTensileMachineGraphics.Reports;
using testTensileMachineGraphics.Options;
using System.Xml.Linq;

namespace testTensileMachineGraphics.OnlineModeFolder
{
    /// <summary>
    /// Interaction logic for ResultsInterface.xaml
    /// </summary>
    public partial class ResultsInterface : Window
    {


       
        ///// <summary>
        ///// odredjuje da li je epruvate za koju gledamo rezultat bila pravougaona
        ///// </summary>
        //public bool IsRectangle = false;
        ///// <summary>
        ///// odredjuje da li je epruvate za koju gledamo rezultat bila kruzna
        ///// </summary>
        //public bool IsCircle = false;

        public bool badCalculationHappened = false;

        public TextBox tfAGlobal = null;
        public TextBox tfBGlobal = null;
        public TextBox tfDGlobal = null;

        private string _au = string.Empty;
        private string _bu = string.Empty;
        private string _Du = string.Empty;

        public string au
        {
            get { return _au; }
            set { _au = value; }
        }
        public string bu
        {
            get { return _bu; }
            set { _bu = value; }
        }
        public string Du
        {
            get { return _Du; }
            set { _Du = value; }
        }


        private double lastLu = 0;
        private double newLu = 0;
        private double lastA = 0;
        private double newA = 0;
        public double ratioNewALastA = 0;

        private string _chbRp02Str = "False";
        private string _chbRt05Str = "False";
        private string _chbReLStr = "False";
        private string _chbReHStr = "False";

        private static LastCheckedRadioButtonInResultsInterface lastCheckedRbtn;
        /// <summary>
        /// readonly property [belezi informaciju koja opcija za radio dugmad je zadnja cekirana/izabrana]
        /// </summary>
        public static LastCheckedRadioButtonInResultsInterface LastCheckedRbtn
        {
            get { return lastCheckedRbtn; }
        }

        public string rbtnReHStr = String.Empty;
        public string rbtnReLStr = String.Empty;
        public string rbtnRt05Str = String.Empty;
        public string rbtnRp02Str = String.Empty;

        //private string rbtnReHStr = "False";
        //private string rbtnReLStr = "False";
        //private string rbtnRt05Str = "False";
        //private string rbtnRp02Str = "False";

        public static bool isCreatedResultsInterface = false;

        /// <summary>
        /// ako je true ucitaj iz fajla
        /// ako je false nemoj citati iz fajla jel se ta vrednost rucno postavlja
        /// </summary>
        /// <param name="loadRp02"></param>
        /// <param name="loadRm"></param>
        /// <param name="loadReL"></param>
        /// <param name="loadReH"></param>
        /// <param name="loadA"></param>
        public void SetTextBoxesForResultsInterface(bool loadRp02 = true, bool loadRm = true, bool loadReL = true, bool loadReH = true, bool loadA = true) 
        {
            try
            {
                setTextBoxesForResultsInterface(loadRp02, loadRm, loadReL, loadReH, loadA);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void SetTextBoxesForResultsInterface(bool loadRp02 = true, bool loadRm = true, bool loadReL = true, bool loadReH = true, bool loadA = true)}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// ako je true ucitaj iz fajla
        /// ako je false nemoj citati iz fajla jel se ta vrednost rucno postavlja
        /// </summary>
        /// <param name="loadRp02"></param>
        /// <param name="loadRm"></param>
        /// <param name="loadReL"></param>
        /// <param name="loadReH"></param>
        /// <param name="loadA"></param>
        private void setTextBoxesForResultsInterface(bool loadRp02 = true, bool loadRm = true, bool loadReL = true, bool loadReH = true, bool loadA = true)
        {
            try
            {
                
                int numberOfApperanceResultsInterface = 0;

                if (File.Exists(Constants.lastResultsInterfaceXml) == false)
                {
                    return;
                }

                bool isFoundRootElement = false;
                List<string> myXmlStrings = File.ReadAllLines(Constants.lastResultsInterfaceXml).ToList();
                if (myXmlStrings.Count == 0)
                {
                    return;
                }
                foreach (string s in myXmlStrings)
                {
                    if (s.Equals("ResultsInterface") || s.Contains("ResultsInterface"))
                    {
                        isFoundRootElement = true;
                        break;
                    }
                }
                if (isFoundRootElement == false)
                {
                    return;
                }
                if (this == null)
                {
                    return;
                }



                //get saved options in online mode
                // Create an XML reader for this file.
                string e2MinStr = String.Empty;
                string e2MaxStr = String.Empty;
                string e2AvgStr = String.Empty;
                string e4MinStr = String.Empty;
                string e4MaxStr = String.Empty;
                string e4AvgStr = String.Empty;


                XmlTextReader textReader = new XmlTextReader(Constants.lastResultsInterfaceXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("ResultsInterface"))
                        {
                            numberOfApperanceResultsInterface++;
                            if (numberOfApperanceResultsInterface == 2)
                            {
                                return;
                            }
                        }

                        if (textReader.Name.Equals("tfLu"))
                        {
                            tfLu.Text = textReader.ReadElementContentAsString();
                        }

                        #region Rp02

                        if (textReader.Name.Equals("chbRp02"))
                        {
                            string chbRp02Str = textReader.ReadElementContentAsString();
                            _chbRp02Str = chbRp02Str;
                            if (chbRp02Str.Equals("True"))
                            {
                               
                                //chbRp02.IsChecked = true;
                                
                                //rbtnRp02.IsEnabled = true;
                                if (_plotting != null)
                                {
                                    if (_plotting.A >= 0.2)
                                    {
                                        tfRp02.Foreground = Brushes.Black;
                                    }
                                    else
                                    {
                                        tfRp02.Foreground = Brushes.Beige;
                                        _plotting.tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                                        _plotting.SetRp02Point(OptionsInPlottingMode.xRange , OptionsInPlottingMode.yRange);
                                    }
                                }

                            }
                            else
                            {
                                //chbRp02.IsChecked = false;
                                //rbtnRp02.IsChecked = false;
                                //rbtnRp02.IsEnabled = false;
                                tfRp02.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("rbtnRp02"))
                        {
                            rbtnRp02Str = textReader.ReadElementContentAsString();
                            //if (chbRp02.IsChecked == true)
                            //{
                            //    if (rbtnRp02Str.Equals("True"))
                            //    {
                            //        rbtnRp02.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnRp02.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfRp02"))
                        {
                            if (loadRp02 == true)
                            {
                                tfRp02.Text = textReader.ReadElementContentAsString();
                                _plotting.tfRp02.Text = _plotting.Rp02RI.ToString();
                                string rp02Str = tfRp02.Text.Replace(',', '.');
                                double rp02 = 0;
                                bool isN = Double.TryParse(rp02Str, out rp02);
                                if (isN)
                                {
                                    _plotting.Rp02RI = rp02;
                                }
                            }
                        }

                        #endregion


                        #region Rt05

                        if (textReader.Name.Equals("chbRt05"))
                        {
                            string chbRt05Str = textReader.ReadElementContentAsString();
                            _chbRt05Str = chbRt05Str;
                            //if (chbRt05Str.Equals("True"))
                            //{
                            //    chbRt05.IsChecked = true;
                            //    //rbtnRt05.IsEnabled = true;
                            //    tfRt05.Foreground = Brushes.Black;

                            //}
                            //else
                            //{
                            //    chbRt05.IsChecked = false;
                            //    rbtnRt05.IsChecked = false;
                            //    //rbtnRt05.IsEnabled = false;
                            //    tfRt05.Foreground = Brushes.Beige;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnRt05"))
                        {
                            rbtnRt05Str = textReader.ReadElementContentAsString();
                            //if (chbRt05.IsChecked == true)
                            //{
                            //    if (rbtnRt05Str.Equals("True"))
                            //    {
                            //        rbtnRt05.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnRt05.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfRt05"))
                        {
                            tfRt05.Text = textReader.ReadElementContentAsString();
                            string rt05Str = tfRt05.Text.Replace(',', '.');
                            double rt05 = 0;
                            bool isN = Double.TryParse(rt05Str, out rt05);
                            if (isN)
                            {
                                _plotting.Rt05 = rt05;
                            }
                        }

                        #endregion

                        #region ReL

                        if (textReader.Name.Equals("chbReL"))
                        {
                            string chbReLStr = textReader.ReadElementContentAsString();
                            _chbReLStr = chbReLStr;
                            if (chbReLStr.Equals("True"))
                            {
                                //chbReL.IsChecked = true;
                                //rbtnReL.IsEnabled = true;
                                tfReL.Foreground = Brushes.Black;

                            }
                            else
                            {
                                //chbReL.IsChecked = false;
                                //rbtnReL.IsChecked = false;
                                //rbtnReL.IsEnabled = false;
                                tfReL.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("rbtnReL"))
                        {
                            rbtnReLStr = textReader.ReadElementContentAsString();
                            //if (chbReL.IsChecked == true)
                            //{
                            //    if (rbtnReLStr.Equals("True"))
                            //    {
                            //        rbtnReL.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnReL.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfReL"))
                        {
                            if (loadReL == true)
                            {
                                tfReL.Text = textReader.ReadElementContentAsString();
                                string reLStr = tfReL.Text.Replace(',', '.');
                                double reL = 0;
                                bool isN = Double.TryParse(reLStr, out reL);
                                if (isN)
                                {
                                    _plotting.ReL = reL;
                                }
                            }
                        }

                        #endregion


                        #region ReH

                        if (textReader.Name.Equals("chbReH"))
                        {
                            string chbReHStr = textReader.ReadElementContentAsString();
                            _chbReHStr = chbReHStr;
                            if (chbReHStr.Equals("True"))
                            {
                                //chbReH.IsChecked = true;
                                //rbtnReH.IsEnabled = true;
                                tfReH.Foreground = Brushes.Black;
                            }
                            else
                            {
                                //chbReH.IsChecked = false;
                                //rbtnReH.IsChecked = false;
                                //rbtnReH.IsEnabled = false;
                                tfReH.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("rbtnReH"))
                        {
                            rbtnReHStr = textReader.ReadElementContentAsString();
                            //if (chbReH.IsChecked == true)
                            //{
                            //    if (rbtnReHStr.Equals("True"))
                            //    {
                            //        rbtnReH.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnReH.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfReH"))
                        {
                            if (loadReH == true)
                            {
                                tfReH.Text = textReader.ReadElementContentAsString();
                                string reHStr = tfReH.Text.Replace(',', '.');
                                double reH = 0;
                                bool isN = Double.TryParse(reHStr, out reH);
                                if (isN)
                                {
                                    _plotting.ReH = reH;
                                }
                            }
                        }

                        #endregion


                        if (textReader.Name.Equals("tfRm"))
                        {
                            if (loadRm == true)
                            {
                                tfRm.Text = textReader.ReadElementContentAsString();
                            }

                        }

                        if (textReader.Name.Equals("tfF"))
                        {
                            tfF.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfFm"))
                        {
                            tfFm.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfAg"))
                        {
                            double ag = 0;
                            string agstr = textReader.ReadElementContentAsString();
                            bool isN = double.TryParse(agstr, out ag);
                            if (ag >= 0)
                            {
                                tfAg.Text = agstr;
                            }
                            else
                            {
                                tfAg.Text = string.Empty;
                            }
                        }

                        if (textReader.Name.Equals("tfAgt"))
                        {
                            tfAgt.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfRRm"))
                        {
                            tfRRm.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfA"))
                        {
                            if (loadA == true)
                            {
                                tfA.Text = textReader.ReadElementContentAsString();
                            }
                        }

                        if (textReader.Name.Equals("tfAt"))
                        {
                            tfAt.Text = textReader.ReadElementContentAsString();
                        }


                        if (textReader.Name.Equals("IsRectangle"))
                        {
                            string IsRectangleStr = textReader.ReadElementContentAsString();
                            if (IsRectangleStr.Equals("True"))
                            {
                                if (onMode != null)
                                {
                                    onMode.Plotting.IsRectangle = true;
                                    onMode.Plotting.IsCircle = false;
                                }
                                SetTextBoxes();
                                if (textReader.Name.Equals("tfau"))
                                {
                                    if (tfAGlobal != null)
                                    {
                                        tfAGlobal.Text = textReader.ReadElementContentAsString();

                                    }
                                }
                                if (textReader.Name.Equals("tfbu"))
                                {
                                    if (tfBGlobal != null)
                                    {
                                        tfBGlobal.Text = textReader.ReadElementContentAsString();
                                    }
                                }
                            }
                        }

                        if (textReader.Name.Equals("IsCircle"))
                        {
                            string IsCircleStr = textReader.ReadElementContentAsString();
                            if (IsCircleStr.Equals("True"))
                            {
                                if (onMode != null)
                                {
                                    onMode.Plotting.IsRectangle = false;
                                    onMode.Plotting.IsCircle = true;
                                }
                                SetTextBoxes();
                                if (textReader.Name.Equals("tfDu"))
                                {
                                    if (tfDGlobal != null)
                                    {
                                        tfDGlobal.Text = textReader.ReadElementContentAsString();
                                        tfDGlobal.IsReadOnly = false;
                                    }
                                }
                            }
                        }

                        if (textReader.Name.Equals("tfSu"))
                        {
                            //tfSu.Text = textReader.ReadElementContentAsString();
                        }

                        //ovo se automatski postavlja menjanjem tekstualnog polja tfSu
                        //if (textReader.Name.Equals("tfZ"))
                        //{
                        //    tfZ.Text = textReader.ReadElementContentAsString();
                        //}


                        if (textReader.Name.Equals("chbn"))
                        {
                            string chbnStr = textReader.ReadElementContentAsString();
                            if (chbnStr.Equals("True"))
                            {
                                chbn.IsChecked = true;
                                tfn.Foreground = Brushes.Black;

                            }
                            else
                            {
                                chbn.IsChecked = false;
                                tfn.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("tfn"))
                        {
                            tfn.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("chbRmax"))
                        {
                            string chbRmaxStr = textReader.ReadElementContentAsString();
                            if (chbRmaxStr.Equals("True"))
                            {
                                chbRmax.IsChecked = true;
                                tfRmax.Foreground = Brushes.Black;
                            }
                            else
                            {
                                chbRmax.IsChecked = false;
                                tfRmax.Foreground = Brushes.Beige;
                            }
                        }

                        if (textReader.Name.Equals("tfRmaxWithPoint"))
                        {
                            tfRmax.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("chbe2"))
                        {
                            string chbe2Str = textReader.ReadElementContentAsString();
                            if (chbe2Str.Equals("True"))
                            {
                                chbe2.IsChecked = true;
                                tfE2.Foreground = Brushes.Black;
                            }
                            else
                            {
                                chbe2.IsChecked = false;
                                tfE2.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("e2Min"))
                        {
                            e2MinStr = textReader.ReadElementContentAsString();
                        }


                        if (textReader.Name.Equals("e2Max"))
                        {
                            e2MaxStr = textReader.ReadElementContentAsString();
                        }
                        //tfE2.Text = e2MinStr + " - " + e2MaxStr;


                        if (textReader.Name.Equals("e2Avg"))
                        {
                            e2AvgStr = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("chbe4"))
                        {
                            string chbe4Str = textReader.ReadElementContentAsString();
                            if (chbe4Str.Equals("True"))
                            {
                                chbe4.IsChecked = true;
                                tfE4.Foreground = Brushes.Black;
                            }
                            else
                            {
                                chbe4.IsChecked = false;
                                tfE4.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("e4Min"))
                        {
                            e4MinStr = textReader.ReadElementContentAsString();
                        }


                        if (textReader.Name.Equals("e4Max"))
                        {
                            e4MaxStr = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("e4Avg"))
                        {
                            e4AvgStr = textReader.ReadElementContentAsString();
                        }
                        //tfE4.Text = e4MinStr + " - " + e4MaxStr;


                        if (textReader.Name.Equals("chbRe"))
                        {
                            string chbReStr = textReader.ReadElementContentAsString();
                            if (chbReStr.Equals("True"))
                            {
                                chbRe.IsChecked = true;
                                tfRe.Foreground = Brushes.Black;

                            }
                            else
                            {
                                chbRe.IsChecked = false;
                                tfRe.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("tfRe"))
                        {
                            //if (onMode.OnHeader!= null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                            //{
                                tfRe.Text = textReader.ReadElementContentAsString();
                                //ove tri linije koda moraju jel se nigde ne pamti pri gasenju aplikacije vrednost plotting clana Re
                                double re = -2;
                                bool isN = double.TryParse( tfRe.Text, out re);
                                onMode.Plotting.Re = re;
                            //}
                        }

                        if (textReader.Name.Equals("chbE"))
                        {
                            string chbEStr = textReader.ReadElementContentAsString();
                            if (chbEStr.Equals("True"))
                            {
                                chbE.IsChecked = true;
                                tfE.Foreground = Brushes.Black;

                            }
                            else
                            {
                                chbE.IsChecked = false;
                                tfE.Foreground = Brushes.Beige;
                            }
                        }


                        if (textReader.Name.Equals("tfE"))
                        {
                            tfE.Text = textReader.ReadElementContentAsString();
                        }

                    }//if (nType == XmlNodeType.Element)

                }//end of while loop

                //tfE2.Text = /*e2MinStr + " - " + e2MaxStr*/ e2AvgStr;
                tfE2.Text = /*e2MinStr + " - " + e2MaxStr*/ onMode.Plotting.E2e4CalculationAfterManualFitting.ArrayE2Interval.Max().ToString();
                //tfE4.Text = /*e4MinStr + " - " + e4MaxStr*/ e4AvgStr;
                tfE4.Text = /*e2MinStr + " - " + e2MaxStr*/ onMode.Plotting.E2e4CalculationAfterManualFitting.ArrayE4Interval.Max().ToString();

                tfLu.SelectAll();
                tfLu.Focus();

                textReader.Close();
            }
            catch (Exception ex)
            {
                ///MessageBox.Show(ex.Message,"setTextBoxesForResultsInterface");
                //Logger.WriteNode(ex.Message.ToString() + " {setTextBoxesForResultsInterface}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void setTextBoxesForResultsInterface(bool loadRp02 = true, bool loadRm = true, bool loadReL = true, bool loadReH = true, bool loadA = true)}", System.DateTime.Now);
            }
        }

        private OnlineMode onMode = null;

        public OnlineMode OnlineModeInstance
        {
            set
            {
                if (value != null)
                {
                    onMode = value;
                }
            }
        }

        private GraphicPlotting _plotting = null;

        public GraphicPlotting PlottingInstance
        {
            set
            {
                if (value != null)
                {
                    _plotting = value;
                }
            }
        }

        private PrintScreen _printScreen = null;

        public PrintScreen PrintScreenInstance
        {
            set
            {
                if (value != null)
                {
                    _printScreen = value;
                }
            }
        }

        private double s0;

        public double S0
        {
            set
            {
                if (value != null)
                {
                    s0 = value;
                }
            }
        }

        private string filePath;

        public string FilePath
        {
            set
            {
                if (value != null)
                {
                    filePath = value;
                }
            }
        }

       


        public void SetAuBuOrDu() 
        {
            try
            {
                if (tfAGlobal != null)
                {
                    tfAGlobal.Text = LastInputOutputSavedData.au;
                }

                if (tfBGlobal != null)
                {
                    tfBGlobal.Text = LastInputOutputSavedData.bu;
                }

                if (tfDGlobal != null)
                {
                    tfDGlobal.Text = LastInputOutputSavedData.Du;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void SetAuBuOrDu()}", System.DateTime.Now);
            }
        }


        public void SetLastCheckedRadioButton() 
        {
            //try
            //{
            //    if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rp02)
            //    {
            //        rbtnRp02.IsChecked = true;
            //    }
            //    if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rt05)
            //    {
            //        rbtnRt05.IsChecked = true;
            //    }
            //    if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReH)
            //    {
            //        rbtnReH.IsChecked = true;
            //    }
            //    if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReL)
            //    {
            //        rbtnReL.IsChecked = true;
            //    }
            //}
            //catch (Exception ex) 
            //{
            //    Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void SetLastCheckedRadioButton()}", System.DateTime.Now);
            //}
        }

        //private double xconst = 1250 + 105 - 300;
        private double xconst = 1250 + 245 - 130;
        //private double yconst = 60;
        private double yconst = 120;

        public ResultsInterface(OnlineMode onlineMode, GraphicPlotting plotting, PrintScreen ps)
        {
            try
            {
                InitializeComponent();
                onMode = onlineMode;
                _plotting = plotting;
                _printScreen = ps;

                //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = xconst;
                this.Top = yconst;


                //var o2 = "O₂";       // or "O\x2082"
                //var o2v2 = "glavno\x2082";  
                //var unit2 = "unit²"; // or "unit\xB2"
                var mm2 = "mm\xB2";
                //MessageBox.Show(o2v2);
                tblSuMeasure.Text = mm2;

                //var emax = "e\x2082     ";
                //lblEmax.Text = emax;

                setTextBoxesForResultsInterface();
                //setRadioButtons();

                if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                {
                    lblE2Part2.Text = "(R2)";
                }
                else
                {
                    lblE2Part2.Text = "(R3)";
                }


                bool isN = Double.TryParse(LastInputOutputSavedData.tfLu_ResultsInterface, out lastLu);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public ResultsInterface(OnlineMode onlineMode, GraphicPlotting plotting, PrintScreen ps)}", System.DateTime.Now);
            }
        }


        public void ClearAuBuOrDuPart()
        {
            try
            {
                clearAuBuOrDuPart();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void ClearAuBuOrDuPart()}", System.DateTime.Now);
            }
        }

        private void clearAuBuOrDuPart() 
        {
            try
            {
                gridTextboxsLeftPartUp.Children.Clear();
                gridTextboxsLeftPartDown.Children.Clear();
                gridTextboxsRightPartUp.Children.Clear();
                gridTextboxsRightPartDown.Children.Clear();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void clearAuBuOrDuPart()}", System.DateTime.Now);
            }
        }

        public void EmptyResultsInterface() 
        {
            tfLu.Text = string.Empty;
            tfRp02.Text = string.Empty;
            tfRt05.Text = string.Empty;
            tfReL.Text = string.Empty;
            tfReH.Text = string.Empty;
            tfRm.Text = string.Empty;
            tfRRm.Text = string.Empty;
            tfF.Text = string.Empty;
            tfFm.Text = string.Empty;
            tfAg.Text = string.Empty;
            tfAgt.Text = string.Empty;
            tfA.Text = string.Empty;
            tfAt.Text = string.Empty;
            tfSu.Text = string.Empty;
            tfZ.Text = string.Empty;
            tfLu.Text = string.Empty;
            tfn.Text = string.Empty;
            tfRmax.Text = string.Empty;
            tfE2.Text = string.Empty;
            tfE4.Text = string.Empty;
            tfRe.Text = string.Empty;
            tfE.Text = string.Empty;
        }

        /// <summary>
        /// ovde se sacuva informacija o LastInputOutputSavedData za Du  da bi se ako dodje do pamcenja pre kreiranja izvestaja moglo zapamtiti koliko je Du
        /// </summary>
        /// <param name="du"></param>
        public void SetTextBoxes(string du)
        {
            try
            {
                if (onMode != null)
                {
                    if (onMode.OnHeader != null)
                    {

                        if (onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true || _plotting.IsCircle == true)
                        {
                            clearAuBuOrDuPart();

                            TextBlock lblD = new TextBlock();
                            lblD.Height = 25;
                            lblD.Width = 30;
                            lblD.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblD.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            lblD.Text = "D\x1D64";//\x1D64 subscript latin letter u

                            Grid.SetRow(lblD, 0);
                            Grid.SetColumn(lblD, 1);


                            TextBox tfD = new TextBox();
                            tfD.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            tfD.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

                            tfD.Height = 25;

                            tfD.Width = 96;

                            if (onMode.IsStoppedOnlineSample == true)
                            {
                                tfD.Text = string.Empty;
                                //ovo se radi kasnije u metodi setResultsInterface(string su, string z) klase GraphicPlotting
                                //onMode.IsStoppedOnlineSample = false;
                            }
                            else
                            {
                                //tfD.Text = du;
                                //ne zelimo da se pamti nego uvek da se cisti
                                tfD.Text = string.Empty; ;
                            }


                            Grid.SetRow(tfD, 1);
                            Grid.SetColumn(tfD, 0);

                            TextBlock lblDmm = new TextBlock();
                            lblDmm.Height = 25;
                            lblDmm.Width = 30;
                            lblDmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblDmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            lblDmm.Text = " mm";
                            Grid.SetRow(lblDmm, 1);
                            Grid.SetColumn(lblDmm, 1);

                            gridTextboxsLeftPartUp.Children.Add(lblD);
                            gridTextboxsRightPartUp.Children.Add(tfD);
                            gridTextboxsRightPartUp.Children.Add(lblDmm);

                            tfDGlobal = tfD;
                            tfDGlobal.IsReadOnly = false;


                            tfDGlobal.TextChanged += new TextChangedEventHandler(tfDGlobal_TextChanged);
                            tfDGlobal.GotFocus += new RoutedEventHandler(tfDGlobal_GotFocus);
                            tfDGlobal.MouseLeave += new MouseEventHandler(tfDGlobal_MouseLeave);


                            tfDGlobal.Foreground = System.Windows.Media.Brushes.Black;
                            tfDGlobal.Background = System.Windows.Media.Brushes.AliceBlue;


                            //onMode.ResultsInterface.IsCircle = true;
                            //onMode.ResultsInterface.IsRectangle = false;

                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void SetTextBoxes(string du)}", System.DateTime.Now);
            }
        }

    

        /// <summary>
        /// ovde se sacuva informacija o LastInputOutputSavedData za au I bu  da bi se ako dodje do pamcenja pre kreiranja izvestaja moglo zapamtiti koliko je au I bu
        /// </summary>
        /// <param name="au"></param>
        /// <param name="bu"></param>
        public void SetTextBoxes(string au, string bu)
        {
            try
            {
                if (onMode != null)
                {
                    if (onMode.OnHeader != null)
                    {
                        if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true || _plotting.IsRectangle == true)
                        {
                            clearAuBuOrDuPart();

                            TextBlock lblA = new TextBlock();
                            lblA.Height = 25;
                            lblA.Width = 30;
                            lblA.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblA.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            var lblau = "a\x1D64";//\x1D64 subscript latin letter u
                            lblA.Text = lblau;

                            Grid.SetRow(lblA, 0);
                            Grid.SetColumn(lblA, 1);



                            TextBox tfA = new TextBox();
                            tfA.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            tfA.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

                            tfA.Height = 25;

                            tfA.Width = 96;


                            if (onMode.IsStoppedOnlineSample == true)
                            {
                                tfA.Text = string.Empty;
                            }
                            else
                            {
                                //ne zelimo da se pamti nego uvek da se cisti
                                //tfA.Text = au;
                                tfA.Text = string.Empty;
                            }




                            Grid.SetRow(tfA, 0);
                            Grid.SetColumn(tfA, 0);

                            TextBlock lblAmm = new TextBlock();
                            lblAmm.Height = 25;
                            lblAmm.Width = 30;
                            lblAmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblAmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            lblAmm.Text = " mm";
                            Grid.SetRow(lblAmm, 0);
                            Grid.SetColumn(lblAmm, 1);


                            TextBlock lblB = new TextBlock();
                            lblB.Height = 25;
                            lblB.Width = 30;
                            lblB.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblB.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            var lblbu = "b\x1D64";//\x1D64 subscript latin letter u
                            lblB.Text = lblbu;

                            Grid.SetRow(lblB, 1);
                            Grid.SetColumn(lblB, 1);



                            TextBox tfB = new TextBox();
                            tfB.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            tfB.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

                            tfB.Height = 25;

                            tfB.Width = 96;


                            if (onMode.IsStoppedOnlineSample == true)
                            {
                                tfB.Text = string.Empty;
                                //ovo se radi kasnije u metodi setResultsInterface(string su, string z) klase GraphicPlotting
                                //onMode.IsStoppedOnlineSample = false;
                            }
                            else
                            {
                                //ne zelimo da se pamti nego uvek da se cisti
                                //tfB.Text = bu;
                                tfB.Text = string.Empty;
                            }




                            Grid.SetRow(tfB, 1);
                            Grid.SetColumn(tfB, 0);

                            TextBlock lblBmm = new TextBlock();
                            lblBmm.Height = 25;
                            lblBmm.Width = 30;
                            lblBmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblBmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            lblBmm.Text = " mm";
                            Grid.SetRow(lblBmm, 1);
                            Grid.SetColumn(lblBmm, 1);

                            gridTextboxsLeftPartUp.Children.Add(lblA);
                            gridTextboxsRightPartUp.Children.Add(tfA);
                            gridTextboxsRightPartUp.Children.Add(lblAmm);
                            gridTextboxsLeftPartDown.Children.Add(lblB);
                            gridTextboxsRightPartDown.Children.Add(tfB);
                            gridTextboxsRightPartDown.Children.Add(lblBmm);


                            tfAGlobal = tfA;
                            tfBGlobal = tfB;

                            tfAGlobal.TextChanged += new TextChangedEventHandler(tfAGlobal_TextChanged);
                            tfBGlobal.TextChanged += new TextChangedEventHandler(tfBGlobal_TextChanged);
                            tfAGlobal.GotFocus += new RoutedEventHandler(tfAGlobal_GotFocus);
                            tfBGlobal.GotFocus += new RoutedEventHandler(tfBGlobal_GotFocus);
                            tfAGlobal.MouseLeave += new MouseEventHandler(tfAGlobal_MouseLeave);
                            tfBGlobal.MouseLeave += new MouseEventHandler(tfBGlobal_MouseLeave);


                            tfAGlobal.Foreground = System.Windows.Media.Brushes.Black;
                            tfBGlobal.Foreground = System.Windows.Media.Brushes.Black;
                            tfAGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
                            tfBGlobal.Background = System.Windows.Media.Brushes.AliceBlue;

                            //onMode.ResultsInterface.IsRectangle = true;
                            //onMode.ResultsInterface.IsCircle = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] { public void SetTextBoxes(string au, string bu)}", System.DateTime.Now);
            }
        }

       

      


        public void SetTextBoxes() 
        {
            try
            {
                if (onMode != null)
                {
                    if (onMode.OnHeader != null)
                    {
                        if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                        {

                            clearAuBuOrDuPart();

                            TextBlock lblA = new TextBlock();
                            lblA.Height = 25;
                            lblA.Width = 30;
                            lblA.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblA.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            var au = "a\x1D64";//\x1D64 subscript latin letter u
                            lblA.Text = au;

                            Grid.SetRow(lblA, 0);
                            Grid.SetColumn(lblA, 1);



                            TextBox tfA = new TextBox();
                            tfA.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            tfA.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

                            tfA.Height = 25;

                            tfA.Width = 96;

                            tfA.Text = String.Empty;


                            Grid.SetRow(tfA, 0);
                            Grid.SetColumn(tfA, 0);

                            TextBlock lblAmm = new TextBlock();
                            lblAmm.Height = 25;
                            lblAmm.Width = 30;
                            lblAmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblAmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            lblAmm.Text = " mm";
                            Grid.SetRow(lblAmm, 0);
                            Grid.SetColumn(lblAmm, 1);


                            TextBlock lblB = new TextBlock();
                            lblB.Height = 25;
                            lblB.Width = 30;
                            lblB.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblB.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            var bu = "b\x1D64";//\x1D64 subscript latin letter u
                            lblB.Text = bu;

                            Grid.SetRow(lblB, 1);
                            Grid.SetColumn(lblB, 1);



                            TextBox tfB = new TextBox();
                            tfB.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            tfB.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

                            tfB.Height = 25;

                            tfB.Width = 96;

                            tfB.Text = String.Empty;


                            Grid.SetRow(tfB, 1);
                            Grid.SetColumn(tfB, 0);

                            TextBlock lblBmm = new TextBlock();
                            lblBmm.Height = 25;
                            lblBmm.Width = 30;
                            lblBmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblBmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            lblBmm.Text = " mm";
                            Grid.SetRow(lblBmm, 1);
                            Grid.SetColumn(lblBmm, 1);

                            gridTextboxsLeftPartUp.Children.Add(lblA);
                            gridTextboxsRightPartUp.Children.Add(tfA);
                            gridTextboxsRightPartUp.Children.Add(lblAmm);
                            gridTextboxsLeftPartDown.Children.Add(lblB);
                            gridTextboxsRightPartDown.Children.Add(tfB);
                            gridTextboxsRightPartDown.Children.Add(lblBmm);


                            tfAGlobal = tfA;
                            tfBGlobal = tfB;


                            tfAGlobal.TextChanged += new TextChangedEventHandler(tfAGlobal_TextChanged);
                            tfBGlobal.TextChanged += new TextChangedEventHandler(tfBGlobal_TextChanged);
                            tfAGlobal.GotFocus += new RoutedEventHandler(tfAGlobal_GotFocus);
                            tfBGlobal.GotFocus += new RoutedEventHandler(tfBGlobal_GotFocus);
                            tfAGlobal.MouseLeave += new MouseEventHandler(tfAGlobal_MouseLeave);
                            tfBGlobal.MouseLeave += new MouseEventHandler(tfBGlobal_MouseLeave);

                            tfAGlobal.Foreground = System.Windows.Media.Brushes.Black;
                            tfBGlobal.Foreground = System.Windows.Media.Brushes.Black;
                            tfAGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
                            tfBGlobal.Background = System.Windows.Media.Brushes.AliceBlue;

                            //onMode.ResultsInterface.IsRectangle = true;
                            //onMode.ResultsInterface.IsCircle = false;


                        }


                        if (onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                        {

                            clearAuBuOrDuPart();

                            TextBlock lblD = new TextBlock();
                            lblD.Height = 25;
                            lblD.Width = 30;
                            lblD.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblD.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                            lblD.Text = "D\x1D64";//\x1D64 subscript latin letter u

                            Grid.SetRow(lblD, 0);
                            Grid.SetColumn(lblD, 1);


                            TextBox tfD = new TextBox();
                            tfD.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                            tfD.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right;

                            tfD.Height = 25;

                            tfD.Width = 96;

                            tfD.Text = String.Empty;


                            Grid.SetRow(tfD, 1);
                            Grid.SetColumn(tfD, 0);

                            TextBlock lblDmm = new TextBlock();
                            lblDmm.Height = 25;
                            lblDmm.Width = 30;
                            lblDmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                            lblDmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                            lblDmm.Text = " mm";
                            Grid.SetRow(lblDmm, 1);
                            Grid.SetColumn(lblDmm, 1);

                            gridTextboxsLeftPartUp.Children.Add(lblD);
                            gridTextboxsRightPartUp.Children.Add(tfD);
                            gridTextboxsRightPartUp.Children.Add(lblDmm);


                            tfDGlobal = tfD;
                            tfDGlobal.IsReadOnly = false;

                            tfDGlobal.TextChanged += new TextChangedEventHandler(tfDGlobal_TextChanged);
                            tfDGlobal.GotFocus += new RoutedEventHandler(tfDGlobal_GotFocus);
                            tfDGlobal.MouseLeave += new MouseEventHandler(tfDGlobal_MouseLeave);

                            tfDGlobal.Foreground = System.Windows.Media.Brushes.Black;
                            tfDGlobal.Background = System.Windows.Media.Brushes.AliceBlue;


                            //onMode.ResultsInterface.IsCircle = true;
                            //onMode.ResultsInterface.IsRectangle = false;



                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] { public void SetTextBoxes()}", System.DateTime.Now);
            }
        }

      

        public string GetCurrentInputOutputFile()
        {
            try
            {
                return getCurrentInputOutputFile();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] { public string GetCurrentInputOutputFile()}", System.DateTime.Now);
                return string.Empty;
            }
        }

        private string getCurrentInputOutputFile() 
        {
            try
            {
                // Get file name.
                string name = _plotting.tfFilepathPlotting.Text;


                //GetAutomaticAnimation file name
                string nameInputOutput = name.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";

                return nameInputOutput;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private string getCurrentInputOutputFile()}", System.DateTime.Now);
                return string.Empty;
            }
        }

        private void tfAGlobal_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                tfAGlobal.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfAGlobal_MouseLeave(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfAGlobal_GotFocus(object sender, RoutedEventArgs e)
        {
            //// MoveFocus takes a TraversalRequest as its argument.
            //TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            //// Gets the element with keyboard focus.
            //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            //// Change keyboard focus.
            //if (elementWithFocus != null)
            //{
            //    elementWithFocus.MoveFocus(request);
            //}
            //MessageBox.Show("11");

            //tfAGlobal.Text = "0";
            tfAGlobal.HorizontalContentAlignment = HorizontalAlignment.Center;
            tfAGlobal.Focus();
            tfAGlobal.SelectAll();
        }

        private void tfAGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
              


                if (onMode != null)
                {
                    if (onMode.OnHeader != null)
                    {
                        if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                        {
                            _au = tfAGlobal.Text;
                            _plotting.au = _au;

                            string currInOutFileName = getCurrentInputOutputFile();

                            if (File.Exists(currInOutFileName) == false)
                            {
                                return;
                            }



                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            // Write to the file name selected.
                            // ... You can write the text from a TextBox instead of a string literal.
                            if (File.Exists(currInOutFileName) == true)
                            {
                                File.Delete(currInOutFileName);
                            }
                            //ovo ce morati ici u for petlju
                            //if (dataListInputOutput.ElementAt(dataListInputOutput.Count - 2).Contains(Constants.au) == true)
                            //{
                            //    dataListInputOutput[dataListInputOutput.Count - 2] = Constants.au + "\t" + _au + "\t" + Constants.mm;
                            //}
                            for (int i = 0; i < dataListInputOutput.Count; i++)
                            {
                                if (dataListInputOutput.ElementAt(i).Split('\t').ElementAt(0).Trim().Equals(Constants.au) == true)
                                {
                                    dataListInputOutput[i] = Constants.au + "\t" + _au + "\t" + Constants.mm;
                                    //break;
                                }
                            }
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);


                            //ovde se sacuva informacija o LastInputOutputSavedData za au  da bi se ako dodje do pamcenja pre kreiranja izvestaja moglo zapamtiti koliko je au 
                            LastInputOutputSavedData.au = _au;


                            double suResult = -1;
                            double aglobal = -1;
                            double bglobal = -1;

                            string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                            bool isN = Double.TryParse(strtfAGlobal, out aglobal);
                            if (isN == false)
                            {
                                if (tfAGlobal.Text.Trim().Equals(String.Empty) == false)
                                {
                                    MessageBox.Show("Parametar a mora biti unet kao broj !");
                                }
                            }

                            if (tfBGlobal.Text.Equals(String.Empty) == true)
                            {
                                tfSu.Text = String.Empty;
                            }
                            else
                            {
                                string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                                bool isNN = Double.TryParse(strtfBGlobal, out bglobal);
                                if (isNN == false)
                                {
                                    if (tfBGlobal.Text.Trim().Equals(String.Empty) == false)
                                    {
                                        MessageBox.Show("Parametar b mora biti unet kao broj !");
                                        tfSu.Text = String.Empty;
                                    }
                                }
                                else
                                {
                                    suResult = aglobal * bglobal;
                                    suResult = Math.Round(suResult, 3);
                                    tfSu.Text = suResult.ToString();
                                    //LastInputOutputSavedData.tfSu_ResultsInterface = tfSu.Text;
                                }
                            }
                        }
                    }

                    onMode.WriteSampleReportOnlineXml();
                    tfAGlobal.Text.Trim();
                    tfAGlobal.Text = tfAGlobal.Text.Trim() + "  ";

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfAGlobal_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBGlobal_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                tfBGlobal.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfBGlobal_MouseLeave(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBGlobal_GotFocus(object sender, RoutedEventArgs e)
        {
            //// MoveFocus takes a TraversalRequest as its argument.
            //TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            //// Gets the element with keyboard focus.
            //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            //// Change keyboard focus.
            //if (elementWithFocus != null)
            //{
            //    elementWithFocus.MoveFocus(request);
            //}
            //MessageBox.Show("11");

            //tfBGlobal.Text = "0";
            tfBGlobal.HorizontalContentAlignment = HorizontalAlignment.Center;
            tfBGlobal.Focus();
            tfBGlobal.SelectAll();
        }

        private void tfBGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                

                if (onMode != null)
                {
                    if (onMode.OnHeader != null)
                    {
                        if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                        {

                            _bu = tfBGlobal.Text;
                            _plotting.bu = _bu;

                            string currInOutFileName = getCurrentInputOutputFile();

                            if (File.Exists(currInOutFileName) == false)
                            {
                                return;
                            }

                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            // Write to the file name selected.
                            // ... You can write the text from a TextBox instead of a string literal.
                            if (File.Exists(currInOutFileName) == true)
                            {
                                File.Delete(currInOutFileName);
                            }
                            //ovo ce morati ici u for petlju
                            //if (dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Contains(Constants.bu) == true)
                            //{
                            //    dataListInputOutput[dataListInputOutput.Count - 1] = Constants.bu + "\t" + _bu + "\t" + Constants.mm;
                            //}
                            for (int i = 0; i < dataListInputOutput.Count; i++)
                            {
                                if (dataListInputOutput.ElementAt(i).Split('\t').ElementAt(0).Trim().Equals(Constants.bu) == true)
                                {
                                    dataListInputOutput[i] = Constants.bu + "\t" + _bu + "\t" + Constants.mm;
                                    //break;
                                }
                            }
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);


                            //ovde se sacuva informacija o LastInputOutputSavedData za bu da bi se ako dodje do pamcenja pre kreiranja izvestaja moglo zapamtiti koliko je bu
                            LastInputOutputSavedData.bu = _bu;

                            double suResult = -1;
                            double bglobal = -1;
                            double aglobal = -1;

                            string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                            bool isN = Double.TryParse(strtfBGlobal, out bglobal);
                            if (isN == false)
                            {
                                if (tfBGlobal.Text.Trim().Equals(String.Empty) == false)
                                {
                                    MessageBox.Show("Parametar b mora biti unet kao broj !");
                                }
                            }

                            if (tfAGlobal.Text.Equals(String.Empty) == true)
                            {
                                tfSu.Text = String.Empty;
                            }
                            else
                            {
                                string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                                bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                                if (isNN == false)
                                {
                                    if (tfAGlobal.Text.Trim().Equals(String.Empty) == false)
                                    {
                                        MessageBox.Show("Parametar a mora biti unet kao broj !");
                                        tfSu.Text = String.Empty;
                                    }
                                }
                                else
                                {
                                    suResult = bglobal * aglobal;
                                    suResult = Math.Round(suResult, 3);
                                    tfSu.Text = suResult.ToString();
                                    //LastInputOutputSavedData.tfSu_ResultsInterface = tfSu.Text;
                                }
                            }
                        }
                    }

                    onMode.WriteSampleReportOnlineXml();
                    tfBGlobal.Text.Trim();
                    tfBGlobal.Text = tfBGlobal.Text.Trim() + "  ";

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfBGlobal_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDGlobal_MouseLeave(object sender, MouseEventArgs e) 
        {
            try
            {
                tfDGlobal.HorizontalContentAlignment = HorizontalAlignment.Right;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfDGlobal_MouseLeave(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDGlobal_GotFocus(object sender, RoutedEventArgs e)
        {

            //// MoveFocus takes a TraversalRequest as its argument.
            //TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            //// Gets the element with keyboard focus.
            //UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            //// Change keyboard focus.
            //if (elementWithFocus != null)
            //{
            //    elementWithFocus.MoveFocus(request);
            //}
            //tfDGlobal.Text = "0";
            tfDGlobal.HorizontalContentAlignment = HorizontalAlignment.Center;
            tfDGlobal.Focus();
            tfDGlobal.SelectAll();
        }

        private void tfDGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
               

                if (onMode != null)
                {
                    if (onMode.OnHeader != null)
                    {
                        if ((onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true))
                        {
                            _Du = tfDGlobal.Text;
                            _plotting.Du = _Du;

                            string currInOutFileName = getCurrentInputOutputFile();

                            if (File.Exists(currInOutFileName) == false)
                            {
                                return;
                            }

                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            // Write to the file name selected.
                            // ... You can write the text from a TextBox instead of a string literal.
                            if (File.Exists(currInOutFileName) == true)
                            {
                                File.Delete(currInOutFileName);
                            }
                            //ovo ce morati ici u for petlju
                            //if (dataListInputOutput.Last().Contains(Constants.Du2) == true)
                            //{
                            //    dataListInputOutput[dataListInputOutput.Count - 1] = Constants.Du2 + "\t" + _Du + "\t" + Constants.mm;
                            //}
                            for (int i = 0; i < dataListInputOutput.Count; i++)
                            {
                                if (dataListInputOutput.ElementAt(i).Split('\t').ElementAt(0).Trim().Equals(Constants.Du2) == true)
                                {
                                    dataListInputOutput[i] = Constants.Du2 + "\t" + _Du + "\t" + Constants.mm;
                                    //break;
                                }
                            }
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);

                            //ovde se sacuva informacija o LastInputOutputSavedData za Du da bi se ako dodje do pamcenja pre kreiranja izvestaja moglo zapamtiti koliko je Du
                            LastInputOutputSavedData.Du = _Du;

                            double suResult = -1;
                            double dglobal = -1;

                            string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                            bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                            if (isN == false)
                            {
                                if (tfDGlobal.Text.Trim().Equals(String.Empty) == false)
                                {
                                    MessageBox.Show("Parametar D mora biti unet kao broj !");
                                    tfSu.Text = String.Empty;
                                }
                                else
                                {
                                    tfSu.Text = String.Empty;
                                }
                            }
                            else
                            {
                                suResult = dglobal * dglobal * Math.PI / 4;
                                suResult = Math.Round(suResult, 3);
                                tfSu.Text = suResult.ToString();
                                //LastInputOutputSavedData.tfSu_ResultsInterface = tfSu.Text;
                            }
                        }
                    }

                    onMode.WriteSampleReportOnlineXml();
                    tfDGlobal.Text.Trim();
                    tfDGlobal.Text = tfDGlobal.Text.Trim() + "  ";
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfDGlobal_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ako je cekirano da postoji ekstenziometar cekiraj automatski checkbox polje za Rt05
        /// ako je cekirano da ne postoji ekstenziometar odcekiraj automatski checkbox polje za Rt05
        /// </summary>
        private void setChechboxRt05InRelationWithekstenziometer() 
        {
            try
            {
                if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                {
                    //chbRt05.IsChecked = true;
                    _printScreen.IsRt05 = true;
                    tfRt05.Foreground = Brushes.Black;
                    tfAgt.Foreground = Brushes.Black;
                    tfAt.Foreground = Brushes.Black;
                }

                if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
                {

                    chbRt05.IsChecked = false;
                    _printScreen.IsRt05 = false;
                    tfRt05.Foreground = Brushes.AliceBlue;
                    tfAgt.Foreground = Brushes.AliceBlue;
                    tfAt.Foreground = Brushes.AliceBlue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void setChechboxRt05InRelationWithekstenziometer()}", System.DateTime.Now);
            }
        }

        public void SetRadioButtons()
        {
            try
            {
                setRadioButtons();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void SetRadioButtons()}", System.DateTime.Now);
            }
        }

        private void setRadioButtons() 
        {
            try
            {
                if (double.IsNaN(rbtnRp02.Height) == true || double.IsNaN(rbtnRt05.Height) == true || double.IsNaN(rbtnReL.Height) == false || double.IsNaN(rbtnReH.Height) == false)
                {
                    tfF.Text = string.Empty;
                    tfRRm.Text = string.Empty;
                }


                if (chbRp02.IsChecked == true || _chbRp02Str.Equals("True") == true)
                {
                    if (rbtnRp02Str.Equals("True") && rbtnRp02.Width > 0)
                    {
                        if (double.IsNaN(rbtnRp02.Height) == false)
                        {
                            rbtnRp02.IsChecked = true;
                        }

                    }
                    else
                    {
                        if (double.IsNaN(rbtnRp02.Height) == false)
                        {
                            rbtnRp02.IsChecked = false;
                        }
                    }
                    chbRp02.IsChecked = true;
                    _printScreen.IsRp02 = true;
                }
                else
                {
                    chbRp02.IsChecked = false;
                    _printScreen.IsRp02 = false;
                }

                if (chbRt05.IsChecked == true || _chbRt05Str.Equals("True") == true)
                {
                    if (rbtnRt05Str.Equals("True") && rbtnRt05.Width > 0)
                    {
                        if (double.IsNaN(rbtnRt05.Height) == false)
                        {
                            rbtnRt05.IsChecked = true;
                        }
                    }
                    else
                    {
                        if (double.IsNaN(rbtnRt05.Height) == false)
                        {
                            rbtnRt05.IsChecked = false;
                        }
                    }
                    //chbRt05.IsChecked = true;
                    //_printScreen.IsRt05 = true;
                    setChechboxRt05InRelationWithekstenziometer();
                }
                else
                {
                    //chbRt05.IsChecked = false;
                    //_printScreen.IsRt05 = false;
                    setChechboxRt05InRelationWithekstenziometer();
                }

                if (chbReL.IsChecked == true || _chbReLStr.Equals("True") == true)
                {
                    if (rbtnReLStr.Equals("True") && rbtnReL.Width > 0)
                    {
                        if (double.IsNaN(rbtnReL.Height) == false)
                        {
                            rbtnReL.IsChecked = true;
                        }

                    }
                    else
                    {
                        if (double.IsNaN(rbtnReL.Height) == false)
                        {
                            rbtnReL.IsChecked = false;
                        }
                    }
                    chbReL.IsChecked = true;
                    _printScreen.IsReL = true;
                }
                else
                {
                    chbReL.IsChecked = false;
                    _printScreen.IsReL = false;
                    if (_plotting != null)
                    {
                        _plotting.chbReLVisibility.IsChecked = false;
                    }
                }

                if (chbReH.IsChecked == true || _chbReHStr.Equals("True") == true)
                {
                    if (rbtnReHStr.Equals("True") && rbtnReH.Width > 0)
                    {
                        if (double.IsNaN(rbtnReH.Height) == false)
                        {
                            rbtnReH.IsChecked = true;
                        }
                    }
                    else
                    {
                        if (double.IsNaN(rbtnReH.Height) == false)
                        {
                            rbtnReH.IsChecked = false;
                        }
                    }
                    chbReH.IsChecked = true;
                    _printScreen.IsReH = true;
                }
                else
                {
                    chbReH.IsChecked = false;
                    _printScreen.IsReH = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void setRadioButtons()}", System.DateTime.Now);
            }
            
        }

        private void updateLastInputOutputData() 
        {
            try
            {
                if (chbRp02.IsChecked == true)
                {
                    LastInputOutputSavedData.chbRp02_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbRp02_ResultsInterface = "False";
                }

                if (chbRt05.IsChecked == true)
                {
                    LastInputOutputSavedData.chbRt05_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbRt05_ResultsInterface = "False";
                }

                if (chbReL.IsChecked == true)
                {
                    LastInputOutputSavedData.chbReL_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbReL_ResultsInterface = "False";
                }

                if (chbReH.IsChecked == true)
                {
                    LastInputOutputSavedData.chbReH_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbReH_ResultsInterface = "False";
                }

                if (rbtnRp02.IsChecked == true)
                {
                    LastInputOutputSavedData.rbtnRp02_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.rbtnRt05_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReL_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReH_ResultsInterface = "False";
                }

                if (rbtnRt05.IsChecked == true)
                {
                    LastInputOutputSavedData.rbtnRt05_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.rbtnRp02_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReL_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReH_ResultsInterface = "False";
                }

                if (rbtnReL.IsChecked == true)
                {
                    LastInputOutputSavedData.rbtnRt05_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.rbtnRp02_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnRt05_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReH_ResultsInterface = "False";
                }

                if (rbtnReH.IsChecked == true)
                {
                    LastInputOutputSavedData.rbtnReH_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.rbtnRp02_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnRt05_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReL_ResultsInterface = "False";
                }


                if (chbn.IsChecked == true)
                {
                    LastInputOutputSavedData.chbn_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbn_ResultsInterface = "False";
                }

                if (chbRmax.IsChecked == true)
                {
                    LastInputOutputSavedData.chbRmax_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbRmax_ResultsInterface = "False";
                }

                if (chbe2.IsChecked == true)
                {
                    LastInputOutputSavedData.chbe2_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbe2_ResultsInterface = "False";
                }

                if (chbe4.IsChecked == true)
                {
                    LastInputOutputSavedData.chbe4_ResultsInterface = "True";
                }
                else
                {
                    LastInputOutputSavedData.chbe4_ResultsInterface = "False";
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void updateLastInputOutputData()}", System.DateTime.Now);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //ako nista nije cekirano onda podrazumevaj da se cekira Rp02
                if (rbtnRp02.IsChecked == false && rbtnRt05.IsChecked == false && rbtnReL.IsChecked == false && rbtnReH.IsChecked == false)
                {
                    if (chbRp02.IsChecked == true)
                    {
                        rbtnRp02.IsChecked = true;
                    }
                    else if (chbRt05.IsChecked == true)
                    {
                        rbtnRt05.IsChecked = true;
                    }
                    else if (chbReL.IsChecked == true)
                    {
                        rbtnReL.IsChecked = true;
                    }
                    else if (chbReH.IsChecked == true)
                    {
                        rbtnReH.IsChecked = true;
                    }
                }
                onMode.WriteXMLLastResultsInterface();
                ResultsInterface.isCreatedResultsInterface = false;
                updateLastInputOutputData();
                onMode.WriteSampleReportOnlineXml();


                if (onMode.Plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                {
                    onMode.Plotting.Printscreen.HideT1T2T3();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRp02_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbRp02.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                if (_plotting.A <= 0.5)
                {
                    chbRp02.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za racunanje Rp02.");
                    return;
                }
                //if (_plotting.A <= 0.2)
                //{
                //    MessageBox.Show("Ne postoji Rp02 !");
                //    chbRp02.IsChecked = false;
                //    return;
                //}
                //rbtnRp02.IsEnabled = true;
                //tfRp02.Text = _plotting.Rp02RI.ToString();
                tfRp02.Foreground = Brushes.Black;
                _printScreen.IsRp02 = true;
                _printScreen.setMarkers();
                LastInputOutputSavedData.chbRp02_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();

                if (_plotting != null)
                {
                    _plotting.chbRp02Visibility.IsChecked = true;
                    if (_plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                    {
                        _printScreen.HideT1T2T3();
                    }
                }
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRp02_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRp02_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbtnRp02.IsChecked == true)
                {
                    //odozdo programski oznaci radio button sledeci kod koga je parametar proracunat
                    if (chbRt05.IsChecked == true)
                    {
                        rbtnRt05.IsChecked = true;
                    }
                    else if (chbReL.IsChecked == true)
                    {
                        rbtnReL.IsChecked = true;
                    }
                    else if (chbReH.IsChecked == true)
                    {
                        rbtnReH.IsChecked = true;
                    }
                }


                rbtnRp02.IsChecked = false;
                //rbtnRp02.IsEnabled = false;
                //tfRp02.Text = String.Empty;
                tfRp02.Foreground = Brushes.AliceBlue;
                _printScreen.IsRp02 = false;
                _printScreen.setMarkers();
                LastInputOutputSavedData.chbRp02_ResultsInterface = "False";

                tfF.Text = string.Empty;
                tfRRm.Text = string.Empty;

                onMode.WriteSampleReportOnlineXml();

                if (_plotting != null)
                {
                    _plotting.chbRp02Visibility.IsChecked = false;
                    if (_plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                    {
                        _printScreen.HideT1T2T3();
                    }
                }
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRp02_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRt05_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbRt05.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                if (onMode != null && onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    MessageBox.Show("Ne postoji parametar Rt05 za merenja bez ekstenziometra !");
                    chbRt05.IsChecked = false;
                    return;
                }
                //rbtnRt05.IsEnabled = true;
                //tfRt05.Text = _plotting.Rt05.ToString();
                tfRt05.Foreground = Brushes.Black;
                _printScreen.IsRt05 = true;
                _printScreen.setMarkers();

                tfAgt.Foreground = Brushes.Black;
                tfAt.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbRt05_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                if (_plotting != null && _plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                {
                    _printScreen.HideT1T2T3();
                }

                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRt05_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRt05_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbtnRt05.IsChecked == true)
                {
                    //odozdo programski oznaci radio button sledeci kod koga je parametar proracunat
                    if (chbRp02.IsChecked == true)
                    {
                        rbtnRp02.IsChecked = true;
                    }
                    else if (chbReL.IsChecked == true)
                    {
                        rbtnReL.IsChecked = true;
                    }
                    else if (chbReH.IsChecked == true)
                    {
                        rbtnReH.IsChecked = true;
                    }
                }


                rbtnRt05.IsChecked = false;
                //rbtnRt05.IsEnabled = false;
                //tfRt05.Text = String.Empty;
                tfRt05.Foreground = Brushes.AliceBlue;
                _printScreen.IsRt05 = false;
                _printScreen.setMarkers();


                tfAgt.Foreground = Brushes.AliceBlue;
                tfAt.Foreground = Brushes.AliceBlue;
                LastInputOutputSavedData.chbRt05_ResultsInterface = "False";

                tfF.Text = string.Empty;
                tfRRm.Text = string.Empty;

                onMode.WriteSampleReportOnlineXml();
                if (_plotting != null && _plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                {
                    _printScreen.HideT1T2T3();
                }

                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRt05_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbReL_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbReL.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                //rbtnReL.IsEnabled = true;
                //tfReL.Text = _plotting.ReL.ToString();

                tfReL.Foreground = Brushes.Black;
                _printScreen.IsReL = true;
                _printScreen.setMarkers();
                LastInputOutputSavedData.chbReL_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();

                if (_plotting != null)
                {
                    _plotting.chbReLVisibility.IsChecked = true;
                    _plotting.tfReL.Text = _plotting.ReL.ToString();
                    if (_plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                    {
                        _printScreen.HideT1T2T3();
                    }
                }

                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbReL_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbReL_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_plotting != null)
                {
                    _plotting.chbReLVisibility.IsChecked = false;
                }

                if (rbtnReL.IsChecked == true)
                {
                    //odozdo programski oznaci radio button sledeci kod koga je parametar proracunat
                    if (chbRp02.IsChecked == true)
                    {
                        rbtnRp02.IsChecked = true;
                    }
                    else if (chbRt05.IsChecked == true)
                    {
                        rbtnRt05.IsChecked = true;
                    }
                    else if (chbReH.IsChecked == true)
                    {
                        rbtnReH.IsChecked = true;
                    }
                }


                rbtnReL.IsChecked = false;
                //rbtnReL.IsEnabled = false;
                //tfReL.Text = String.Empty;
                tfReL.Foreground = Brushes.AliceBlue;
                _printScreen.IsReL = false;
                _printScreen.setMarkers();
                LastInputOutputSavedData.chbReL_ResultsInterface = "False";

                tfF.Text = string.Empty;
                tfRRm.Text = string.Empty;

                onMode.WriteSampleReportOnlineXml();


                if (_plotting != null)
                {
                    _plotting.chbReLVisibility.IsChecked = false;
                    _plotting.tfReL.Text = Constants.NOTFOUNDMAXMIN;
                    //if (_plotting != null && _plotting.OnlineModeInstance != null && _plotting.OnlineModeInstance.ResultsInterface != null)
                    //{
                    //    _plotting.OnlineModeInstance.ResultsInterface.tfReL.Text = String.Empty;
                    //}
                    //tfReL.Text = string.Empty;
                    if (_plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                    {
                        _printScreen.HideT1T2T3();
                    }
                }

                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbReL_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbReH_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    chbReH.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                //rbtnReH.IsEnabled = true;
                //tfReH.Text = _plotting.ReH.ToString();

                tfReH.Foreground = Brushes.Black;
                _printScreen.IsReH = true;
                _printScreen.setMarkers();
                LastInputOutputSavedData.chbReH_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();

                if (_plotting != null)
                {
                    _plotting.chbReHVisibility.IsChecked = true;
                    _plotting.tfReH.Text = _plotting.ReH.ToString();
                    if (_plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                    {
                        _printScreen.HideT1T2T3();
                    }
                }

                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbReH_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }

        private void chbReH_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rbtnReH.IsChecked == true)
                {
                    //odozdo programski oznaci radio button sledeci kod koga je parametar proracunat
                    if (chbRp02.IsChecked == true)
                    {
                        rbtnRp02.IsChecked = true;
                    }
                    else if (chbRt05.IsChecked == true)
                    {
                        rbtnRt05.IsChecked = true;
                    }
                    else if (chbReL.IsChecked == true)
                    {
                        rbtnReL.IsChecked = true;
                    }
                }


                rbtnReH.IsChecked = false;
                //rbtnReH.IsEnabled = false;
                //tfReH.Text = String.Empty;
                tfReH.Foreground = Brushes.AliceBlue;
                _printScreen.IsReH = false;
                _printScreen.setMarkers();
                LastInputOutputSavedData.chbReH_ResultsInterface = "False";

                tfF.Text = string.Empty;
                tfRRm.Text = string.Empty;

                onMode.WriteSampleReportOnlineXml();

                if (_plotting != null)
                {
                    _plotting.chbReHVisibility.IsChecked = false;
                    _plotting.tfReH.Text = Constants.NOTFOUNDMAXMIN;
                    //tfReH.Text = string.Empty;
                    if (_plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                    {
                        _printScreen.HideT1T2T3();
                    }
                }

                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbReH_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void checkLastCheckedRadioButton() 
        {
            //try
            //{
            //    if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rp02)
            //    {
            //        rbtnRp02.IsChecked = true;
            //    }
            //    else if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rt05)
            //    {
            //        rbtnRt05.IsChecked = true;
            //    }
            //    else if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReL)
            //    {
            //        rbtnReL.IsChecked = true;
            //    }
            //    else if (lastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReH)
            //    {
            //        rbtnReH.IsChecked = true;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void checkLastCheckedRadioButton()}", System.DateTime.Now);
            //}
        }

        private void rbtnRp02_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    rbtnRp02.IsChecked = false;
                    return;
                }

                if (chbRp02.IsChecked == false)
                {
                    //MessageBox.Show("Morate čekirati da želite računanje izabranog parametra.");
                    rbtnRp02.IsChecked = false;
                    checkLastCheckedRadioButton();
                    return;
                }

                lastCheckedRbtn = LastCheckedRadioButtonInResultsInterface.Rp02;
                if (onMode.SaveDialogProperty != null)
                {
                    if (onMode.SaveDialogProperty.IsClickedToSaveFile == true)
                    {
                        onMode.SaveDialogProperty.IsClickedToSaveFile = false;
                        return;
                    }
                }

                onMode.setResultsInterfaceFandRRm();


                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void rbtnRp02_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnRt05_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    rbtnRt05.IsChecked = false;
                    MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }


                if (chbRt05.IsChecked == false)
                {
                    //MessageBox.Show("Morate čekirati da želite računanje izabranog parametra.");
                    rbtnRt05.IsChecked = false;
                    checkLastCheckedRadioButton();
                    return;
                }

                lastCheckedRbtn = LastCheckedRadioButtonInResultsInterface.Rt05;
                if (onMode.SaveDialogProperty != null)
                {
                    if (onMode.SaveDialogProperty.IsClickedToSaveFile == true)
                    {
                        onMode.SaveDialogProperty.IsClickedToSaveFile = false;
                        return;
                    }
                }

                onMode.setResultsInterfaceFandRRm();


                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void rbtnRt05_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnReL_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    rbtnReL.IsChecked = false;
                    MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }


                if (chbReL.IsChecked == false)
                {
                    //MessageBox.Show("Morate čekirati da želite računanje izabranog parametra.");
                    rbtnReL.IsChecked = false;
                    checkLastCheckedRadioButton();
                    return;
                }

                lastCheckedRbtn = LastCheckedRadioButtonInResultsInterface.ReL;
                if (onMode.SaveDialogProperty != null)
                {
                    if (onMode.SaveDialogProperty.IsClickedToSaveFile == true)
                    {
                        onMode.SaveDialogProperty.IsClickedToSaveFile = false;
                        return;
                    }
                }

                onMode.setResultsInterfaceFandRRm();


                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void rbtnReL_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnReH_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    rbtnReH.IsChecked = false;
                    MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }


                if (chbReH.IsChecked == false)
                {
                    //MessageBox.Show("Morate čekirati da želite računanje izabranog parametra.");
                    rbtnReH.IsChecked = false;
                    checkLastCheckedRadioButton();
                    return;
                }

                lastCheckedRbtn = LastCheckedRadioButtonInResultsInterface.ReH;
                if (onMode.SaveDialogProperty != null)
                {
                    if (onMode.SaveDialogProperty.IsClickedToSaveFile == true)
                    {
                        onMode.SaveDialogProperty.IsClickedToSaveFile = false;
                        return;
                    }
                }

                onMode.setResultsInterfaceFandRRm();


                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                badCalculationHappened = true;
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void rbtnReH_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfSu_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double su = Double.MaxValue;
                string suStr = tfSu.Text;
                suStr = suStr.Replace(',', '.');
                bool isN = Double.TryParse(suStr, out su);
                if (isN == false && suStr.Equals(string.Empty) == false)
                {
                    MessageBox.Show("Krajnu površinu epruvete niste uneli u obliku broja!");
                }
                else
                {
                    getS0Offline();
                    double r = (s0 - su) / s0;
                    r = Math.Round(r, 3);
                    r = r * 100;//Z se sada racuna u procentima, pa se zato mnozi sa 100
                    tfZ.Text = r.ToString();

                    //upisi promene za Su i Z u tekstualni fajl
                    string currInOutFileName = getCurrentInputOutputFile();

                    if (File.Exists(currInOutFileName) == true)
                    {
                        List<string> dataListInputOutput = new List<string>();
                        dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                        string firstPart = string.Empty, secondPart = string.Empty;
                        for (int i = 0; i < dataListInputOutput.Count; i++)
                        {
                            if (dataListInputOutput[i].Split('\t').Count() > 1)
                            {
                                firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                                secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                            }

                            if (firstPart.Contains(Constants.Su) == true)
                            {
                                dataListInputOutput[i] = Constants.Su + "\t" + su.ToString() + "\t" + Constants.mm2;
                            }
                            if (firstPart.Contains(Constants.Z) == true)
                            {
                                dataListInputOutput[i] = Constants.Z + "\t" + tfZ.Text;
                            }

                        }//end for loop


                        File.Delete(currInOutFileName);
                        File.WriteAllLines(currInOutFileName, dataListInputOutput);
                    }

                    //get last values for Su and Z
                    _plotting.SuStr = su.ToString();
                    _plotting.ZStr = tfZ.Text;

                    LastInputOutputSavedData.tfSu_ResultsInterface = tfSu.Text;
                    LastInputOutputSavedData.tfZ_ResultsInterface = tfZ.Text;
                    onMode.WriteSampleReportOnlineXml();
                    //onMode.WriteXMLLastResultsInterface();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfSu_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfSu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                tfSu.Foreground = Brushes.Black;

                //save in xml file
                onMode.WriteXMLLastResultsInterface();

                if (e.Key == Key.Tab || e.Key == Key.Enter)
                {
                    //tfLu.SelectAll();
                    //tfLu.Focus();
                    if (tfDGlobal != null)
                    {
                        tfDGlobal.SelectAll();
                        tfDGlobal.Focus();
                    }
                    if (tfAGlobal != null)
                    {
                        tfAGlobal.SelectAll();
                        tfAGlobal.Focus();
                    }
                    if (tfBGlobal != null)
                    {
                        tfBGlobal.SelectAll();
                        tfBGlobal.Focus();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfSu_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void getS0Offline()
        {
            try
            {
                if (File.Exists(filePath) == false)
                {
                    MessageBox.Show("Fajl sa izabranom putanjom " + filePath + " ne postoji!" + System.Environment.NewLine + " Proverite izabranu putanju fajla.", "Rezultati");
                    return;
                }
                List<string> data = File.ReadAllLines(filePath).ToList();


                for (int i = 0; i < data.Count; i++)
                {
                    List<string> curList = new List<string>();
                    curList = data[i].Split('\t').ToList();
                    for (int j = 0; j < curList.Count; j++)
                    {

                        if (curList[j].Contains("S0"))
                        {
                            for (int k = 0; k < curList.Count; k++)
                            {
                                string strS0 = curList[0].Split(':')[1].Trim();
                                double temps0 = 0.0;

                                bool isN = Double.TryParse(strS0, out temps0);
                                if (isN)
                                {
                                    s0 = temps0;
                                }
                            }
                        }

                    }
                }// end of main for loop
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void getS0Offline()}", System.DateTime.Now);
            }
        }


        public void AfterEnterClicked_Lu()
        {
            try
            {
                afterEnterClicked_Lu();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {public void AfterEnterClicked_Lu()}", System.DateTime.Now);
            }
        }

        private void afterEnterClicked_Lu() 
        {
            try
            {
                lastA = _plotting.A;

                if (_plotting.A_FirstCalculated == 0)
                {
                    MessageBox.Show("Ne mozete azurirati Lu jel izduzenje ne postoji (A = 0) !");
                    return;
                }


                MyPointCollection newFittingPoints = new MyPointCollection();
                newFittingPoints.Clear();

                double lu = Double.MaxValue;
                string luStr = tfLu.Text;
                luStr = luStr.Replace(',', '.');
                bool isN = Double.TryParse(luStr, out lu);
                if (isN == false && luStr.Equals(string.Empty) == false)
                {
                    MessageBox.Show("Krajnu dužinu epruvete niste uneli u obliku broja!");
                }
                else
                {
                    newLu = lu;
                    _plotting.Lu = lu;

                    double localL0 = 0;
                    bool isNumber = double.TryParse(LastInputOutputSavedData.tfL0, out localL0);
                    double _a = (newLu - localL0) / localL0 * 100;
                    _a = Math.Round(_a, 1);
                    newA = _a;
                }



                //if (newLu < lastLu - 5 || newLu > lastLu + 5)
                //{
                    //MessageBox.Show("Možete uneti samo +/- 5 milimetara korak od trenutne vrednosti Lu-a !");
                    //tfLu.Text = lastLu.ToString();
                    //return;
                //}

                newLu = Math.Round(newLu, 2);
                lastLu = Math.Round(lastLu, 2);
                if (newA == lastA)
                {
                    return;
                }


                //ratioNewALastA = newA / lastA;
                ratioNewALastA = newA / _plotting.A_FirstCalculated;
                //ratioNewLuLastLu = ((newLu - _plotting.L0) / _plotting.L0) / ((lastLu - _plotting.L0) / _plotting.L0);


                for (int i = 0; i < _plotting.DataReader.FittingRelativeElongation.Count; i++)
                {
                    _plotting.DataReader.FittingRelativeElongation[i] = _plotting.DataReader.FittingRelativeElongation[i] * /*ratioNewALastA*/1;
                    MyPoint point = new MyPoint(_plotting.DataReader.FittingPreassureInMPa[i], _plotting.DataReader.FittingRelativeElongation[i]);
                    newFittingPoints.Add(point);
                }



                
                _plotting.DrawFittingGraphic_AfterLuChanged(newFittingPoints, ratioNewALastA);

                string pathLuManual = _plotting.tfFilepathPlotting.Text.Split('.').ElementAt(0);
                pathLuManual += "LuManual.pe";
                List<string> datasLuManual = new List<string>();
                List<double> elongationLuManual = new List<double>();
                //ako prvi put azuriras Lu rucno napravi ponovni tekstualni fajl sa Preasure and Elongation i dodaj LuManual
              
                    List<string> datas;
                    string path = _plotting.tfFilepathPlotting.Text.Split('.').ElementAt(0);
                    path += ".pe";
                    List<double> preassure = new List<double>();
                    List<double> elongation = new List<double>();
                    
                        datas = File.ReadAllLines(path).ToList();
                        foreach (var data in datas)
                        {
                            List<string> pe = data.Split('\t').ToList();
                            double pr = 0;
                            double el = 0;
                            isN = double.TryParse(pe[0], out pr);
                            if (isN)
                            {
                                preassure.Add(pr);
                            }
                            isN = double.TryParse(pe[1], out el);
                            if (isN)
                            {
                                elongation.Add(el);
                            }
                        }
                 
                    double elon2 = 0;
                    foreach (var elon in elongation)
                    {
                        elon2 = elon * ratioNewALastA;
                        elon2 = Math.Round(elon2, 2);
                        elongationLuManual.Add(elon2);
                    }
                    int cnt = 0;
                    foreach (var item in elongationLuManual)
                    {
                        datasLuManual.Add(preassure[cnt] + "\t" + item);
                        cnt++;
                    }

                    if (File.Exists(pathLuManual) == true)
                    {
                        File.Delete(pathLuManual);
                    }

                    File.AppendAllLines(pathLuManual, datasLuManual);
                
                
                //lastA = newA;
                lastLu = newLu;

                //na izlaznim podacima postavi ovu vrednost na izlaznom grafiku a ne proracunatu
                //onMode.ResultsInterface.tfLu.Text = lastLu.ToString();

                //save in xml file
                onMode.WriteXMLLastResultsInterface();


                if (chbRp02.IsChecked == true)
                {
                    _plotting.chbRp02Visibility.IsChecked = true;
                }
                else
                {
                    _plotting.chbRp02Visibility.IsChecked = false;
                }

                if (chbReL.IsChecked == true)
                {
                    _plotting.chbReLVisibility.IsChecked = true;
                }
                else
                {
                    _plotting.chbReLVisibility.IsChecked = false;
                }

                if (chbReH.IsChecked == true)
                {
                    _plotting.chbReHVisibility.IsChecked = true;
                }
                else
                {
                    _plotting.chbReHVisibility.IsChecked = false;
                }

                _plotting.A = newA;
                _plotting.OnlineModeInstance.ResultsInterface.tfA.Text = newA.ToString();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void afterEnterClicked_Lu()}", System.DateTime.Now);
            }

        }

        private void tfLu_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Tab)
                {
                    afterEnterClicked_Lu();
                    //onMode.Plotting.Printscreen.HideT1T2T3();
                    //onMode.Plotting.DisableGraphicPlotting();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfLu_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfLu_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                double lu = Double.MaxValue;
                string luStr = tfLu.Text;
                luStr = luStr.Replace(',', '.');
                bool isN = Double.TryParse(luStr, out lu);
                if (isN == false && luStr.Equals(string.Empty) == false)
                {
                    MessageBox.Show("Krajnu dužinu epruvete niste uneli u obliku broja!");
                }
                else
                {
                    lastLu = lu;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfLu_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRp02_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za Rp02 u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.Rp02) == true)
                        {
                            dataListInputOutput[i] = Constants.Rp02 + "\t" + tfRp02.Text + "\t" + Constants.MPa;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values Rp02
                double rp02 = Double.MaxValue;
                string rp02Str = tfRp02.Text;
                rp02Str = rp02Str.Replace(',', '.');
                bool isN = Double.TryParse(rp02Str, out rp02);
                _plotting.Rp02RI = rp02;
                LastInputOutputSavedData.tfRp02_ResultsInterface = tfRp02.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfRp02_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
       
        }

        private void tfReL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za ReL u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.ReL) == true)
                        {
                            dataListInputOutput[i] = Constants.ReL + "\t" + tfReL.Text + "\t" + Constants.MPa;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values ReL
                double reL = Double.MaxValue;
                string reLStr = tfReL.Text;
                reLStr = reLStr.Replace(',', '.');
                bool isN = Double.TryParse(reLStr, out reL);
                _plotting.ReL = reL;
                LastInputOutputSavedData.tfReL_ResultsInterface = tfReL.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfReL_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfReH_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za ReH u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.ReH) == true)
                        {
                            dataListInputOutput[i] = Constants.ReH + "\t" + tfReH.Text + "\t" + Constants.MPa;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values ReH
                double reH = Double.MaxValue;
                string reHStr = tfReH.Text;
                reHStr = reHStr.Replace(',', '.');
                bool isN = Double.TryParse(reHStr, out reH);
                _plotting.ReH = reH;
                LastInputOutputSavedData.tfReH_ResultsInterface = tfReH.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfReH_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRm_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za Rm u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.Rm) == true)
                        {
                            dataListInputOutput[i] = Constants.Rm + "\t" + tfRm.Text + "\t" + Constants.MPa;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values Rm
                double rm = Double.MaxValue;
                string rmStr = tfRm.Text;
                rmStr = rmStr.Replace(',', '.');
                bool isN = Double.TryParse(rmStr, out rm);
                _plotting.Rm = rm;
                LastInputOutputSavedData.tfRm_ResultsInterface = tfRm.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfRm_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRRm_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za RRm u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.R_Rm) == true)
                        {
                            dataListInputOutput[i] = Constants.R_Rm + "\t" + tfRRm.Text;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values RRm
                double rrm = Double.MaxValue;
                string rrmStr = tfRRm.Text;
                rrmStr = rrmStr.Replace(',', '.');
                bool isN = Double.TryParse(rrmStr, out rrm);
                _plotting.RRm = rrm;
                LastInputOutputSavedData.tfRRm_ResultsInterface = tfRRm.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfRRm_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfF_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za F u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.F) == true)
                        {
                            dataListInputOutput[i] = Constants.F + "\t" + tfF.Text + "\t" + Constants.N;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values F
                double f = Double.MaxValue;
                string fStr = tfF.Text;
                fStr = fStr.Replace(',', '.');
                bool isN = Double.TryParse(fStr, out f);
                _plotting.F = f;
                LastInputOutputSavedData.tfF_ResultsInterface = tfF.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfF_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfFm_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za Fm u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.Fm) == true)
                        {
                            dataListInputOutput[i] = Constants.Fm + "\t" + tfFm.Text + "\t" + Constants.N;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values Fm
                double fm = Double.MaxValue;
                string fmStr = tfFm.Text;
                fmStr = fmStr.Replace(',', '.');
                bool isN = Double.TryParse(fmStr, out fm);
                _plotting.Fm = fm;
                LastInputOutputSavedData.tfFm_ResultsInterface = tfFm.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfFm_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfA_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                //upisi promene za A u tekstualni fajl
                string currInOutFileName = getCurrentInputOutputFile();

                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    string firstPart = string.Empty, secondPart = string.Empty;
                    for (int i = 0; i < dataListInputOutput.Count; i++)
                    {
                        if (dataListInputOutput[i].Split('\t').Count() > 1)
                        {
                            firstPart = dataListInputOutput[i].Split('\t').ElementAt(0).Trim();
                            secondPart = dataListInputOutput[i].Split('\t').ElementAt(1).Trim();
                        }

                        if (firstPart.Equals(Constants.A) == true)
                        {
                            dataListInputOutput[i] = Constants.A + "\t" + tfA.Text;
                        }

                    }//end for loop


                    File.Delete(currInOutFileName);
                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                }

                //get last values A
                double a = Double.MaxValue;
                string aStr = tfA.Text;
                aStr = aStr.Replace(',', '.');
                bool isN = Double.TryParse(aStr, out a);
                _plotting.A = a;
                LastInputOutputSavedData.tfA_ResultsInterface = tfA.Text;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void tfA_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        #region CheckedEventsN_Rmax_e2_e4

        private void chbn_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    chbn.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                if (_plotting.A <= OptionsInPlottingMode.EndIntervalForN)
                {
                    chbn.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za racunanje n-a.");
                    return;
                }

                tfn.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbn_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbn_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbn_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                tfn.Foreground = Brushes.AliceBlue;
                LastInputOutputSavedData.chbn_ResultsInterface = "False";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbn_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRmax_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                if (onMode.IsLessThanOne)
                {
                    chbRmax.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                tfRmax.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbRmax_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRmax_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRmax_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                tfRmax.Foreground = Brushes.AliceBlue;
                LastInputOutputSavedData.chbRmax_ResultsInterface = "False";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRmax_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void chbe2_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbe2.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                tfE2.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbe2_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbe2_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbe2_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                tfE2.Foreground = Brushes.AliceBlue;
                LastInputOutputSavedData.chbe2_ResultsInterface = "False";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbe2_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void chbe4_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbe4.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                tfE4.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbe4_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbe4_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbe4_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                tfE4.Foreground = Brushes.AliceBlue;
                LastInputOutputSavedData.chbe4_ResultsInterface = "False";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbe4_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRe_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbRe.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                tfRe.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbRe_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRe_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRe_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                tfRe.Foreground = Brushes.AliceBlue;
                LastInputOutputSavedData.chbRe_ResultsInterface = "False";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbRe_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }



        private void chbE_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.IsLessThanOne)
                {
                    chbE.IsChecked = false;
                    //MessageBox.Show("Isuvise malo izduzenje za analizu.");
                    return;
                }

                if (onMode != null && onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    MessageBox.Show("Ne postoji parametar E za merenja bez ekstenziometra !");
                    chbE.IsChecked = false;
                    return;
                }

                tfE.Foreground = Brushes.Black;
                LastInputOutputSavedData.chbE_ResultsInterface = "True";

                onMode.WriteSampleReportOnlineXml();
                onMode.WriteXMLLastResultsInterface();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[ResultsInterface.xaml.cs] {private void chbE_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbE_Unchecked(object sender, RoutedEventArgs e)
        {
            tfE.Foreground = Brushes.AliceBlue;
            LastInputOutputSavedData.chbE_ResultsInterface = "False";

            onMode.WriteSampleReportOnlineXml();
            onMode.WriteXMLLastResultsInterface();
        }

        #endregion

        #region MoveFocus

        private void tfRp02_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfRt05_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfReL_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfReH_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfRm_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfRRm_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfF_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfFm_GotFocus(object sender, RoutedEventArgs e)
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


        private void tfAg_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfAgt_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfA_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfAt_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfZ_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfn_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfRmax_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfE2_GotFocus(object sender, RoutedEventArgs e)
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

        private void tfE4_GotFocus(object sender, RoutedEventArgs e)
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

        private void rbtnRp02_GotFocus(object sender, RoutedEventArgs e)
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

        private void rbtnRt05_GotFocus(object sender, RoutedEventArgs e)
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

        private void rbtnReL_GotFocus(object sender, RoutedEventArgs e)
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

        private void rbtnReH_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbRp02_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbRt05_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbReL_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbReH_GotFocus(object sender, RoutedEventArgs e)
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


        private void chbn_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbRmax_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbe2_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbe4_GotFocus(object sender, RoutedEventArgs e)
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


        private void tfSu_GotFocus(object sender, RoutedEventArgs e)
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


        private void tfRe_GotFocus(object sender, RoutedEventArgs e)
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


        private void tfE_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbRe_GotFocus(object sender, RoutedEventArgs e)
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

        private void chbE_GotFocus(object sender, RoutedEventArgs e)
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


        #endregion




       
    }
}
