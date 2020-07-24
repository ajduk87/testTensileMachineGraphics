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
using testTensileMachineGraphics.PointViewModel;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.ComponentModel;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System.IO;
using testTensileMachineGraphics.Options;
using testTensileMachineGraphics.MessageBoxes;
using System.Threading;
using System.Xml;
using Microsoft.Research.DynamicDataDisplay.Charts;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using testTensileMachineGraphics.Windows;
using testTensileMachineGraphics.OnlineModeFolder;
using testTensileMachineGraphics.Reports;
using System.Xml.Linq;
 

namespace testTensileMachineGraphics
{
    /// <summary>
    /// Interaction logic for GraphicPlotting.xaml
    /// </summary>
    public partial class GraphicPlotting : UserControl
    {

        public bool IsMessageShownForRangeReHX = false;
        public bool BadCalculationHappened = false;

        private bool enableOnlyRp02ReLReHRm = false;



        /// <summary>
        /// kada ne zelis da ti se u metodi drawfittingGraphic racuna A, vec si ga proracuno klikom Tab-a u Lu tekstualnom polju 
        /// </summary>
        public bool IDontWantShowT1T2T3AtPrintScreen = false;

        public bool IWantToBackFirstCalculateYung = true;

        /// <summary>
        /// vrednost true je ako se yung proracuna prvi put
        /// </summary>
        public bool IsYungFirstTimeCalculate = true;
        /// <summary>
        /// vrednost Yunga kada se prvi put proracuna
        /// </summary>
        public double YungFirstTimeCalculated = 0;


        public bool IsEverT1T2orT3ManualSetted = false;

        public double AManualClickedValue = 0;

        private List<double> lastPreassures = new List<double>();
        private List<double> lastElongations = new List<double>();

        public bool IsSecondAndMoreLuManual = false;
        public int NumberOfLuManual = 0;

        public double Border005Global = 0.0;

        private bool isOptionsForYungModuoOpen = false;
        public bool IsOptionsForYungModuoOpen
        {
            get { return isOptionsForYungModuoOpen; }
            set 
            {
                if (value != null)
                {
                    isOptionsForYungModuoOpen = value;
                }
            }
        }

       

        public int IndexOfPointClosestToRedProperty = 0;

        /// <summary>
        /// ako je u offline promenjen neki od kalibracionih elemenata izduzenja
        /// tada ja vrednost ove promenljive postavljena na true
        /// </summary>
        public bool IsChangedElongationCalibrationparameter = false;

        private ReDrawingInOfflineMode redrawing;
        public ReDrawingInOfflineMode Redrawing
        {
            get { return redrawing; }
        }



        #region NmanualParameters

        public List<double> PreassureForNManualProperty;
        public List<double> Fs_FittingForManualNProperty;
        public List<double> DeltaLsInProcForManualNProperty;
        public double NManual;
        public NHardeningExponent NHardeningExponentManual;

        #endregion



        public E2E4CalculationAfterManualFitting E2e4CalculationAfterManualFitting
        {
            get { return e2e4CalculationAfterManualFitting; }
            set 
            {
                if (value != null)
                {
                    e2e4CalculationAfterManualFitting = value;
                }
            }
        }

        /// <summary>
        /// zbog rucnog fitovanja, mora ponovo da se preracunaju vrednosti e2min,e2max,e4min i e4max 
        /// kao i da se zbog promene rp02 ponovo fituju grafici promene napona i izduzenja
        /// </summary>
        private E2E4CalculationAfterManualFitting e2e4CalculationAfterManualFitting;



        private bool isClickedByMouse_Plotting_Rp02 = false;
        public bool IsClickedByMouse_Plotting_Rp02
        {
            get { return isClickedByMouse_Plotting_Rp02; }
            set { isClickedByMouse_Plotting_Rp02 = value; }
        }
        private bool isClickedByMouse_Plotting_ReL = false;
        public bool IsClickedByMouse_Plotting_ReL
        {
            get { return isClickedByMouse_Plotting_ReL; }
            set { isClickedByMouse_Plotting_ReL = value; }
        }
        private bool isClickedByMouse_Plotting_ReH = false;
        public bool IsClickedByMouse_Plotting_ReH
        {
            get { return isClickedByMouse_Plotting_ReH; }
            set { isClickedByMouse_Plotting_ReH = value; }
        }
        private bool isClickedByMouse_Plotting_Rm = false;
        public bool IsClickedByMouse_Plotting_Rm
        {
            get { return isClickedByMouse_Plotting_Rm; }
            set { isClickedByMouse_Plotting_Rm = value; }
        }
        private bool isClickedByMouse_Plotting_A = false;
        public bool IsClickedByMouse_Plotting_A
        {
            get { return isClickedByMouse_Plotting_A; }
            set { isClickedByMouse_Plotting_A = value; }
        }


        /// <summary>
        /// zbog rucnog unosa Lu-a mora da se pamte x vrednosti parametara ReH, ReL, Rp02, Rm, A, Ag, Agt, At [tj vrednosti markera (trouglici) [ReH, ReL, Rp02, Rm] i brojcane vrednosti rezultata vezanih za x osu [A, Ag, Agt, At]]
        /// </summary>
        #region LuMaunalEnteringSavings

        public double ReH_X { get { return _reH_X; } }
        private double _reH_X = 0;
        private double _reL_X = 0;
        private double _rp02_X = 0;
        private double _rm_X = 0;

        private double _a_X = 0;
        private double _ag_X = 0;
        private double _agt_X = 0;
        private double _at_X = 0;



        private bool isReHWantToTranslate = true;
        private bool isLuManualChanged = false;
        public bool IsLuManualChanged
        {
            get { return isLuManualChanged; }
            set 
            {
                isLuManualChanged = value;
            }
        }

        #endregion

        /// <summary>
        /// racunanje koeficijenata ocvrscenja n
        /// </summary>
        private NHardeningExponent nHardeningExponent;
        public NHardeningExponent NHardeningExponent
        {
            get { return nHardeningExponent; }
            set 
            {
                if (value != null)
                {
                    nHardeningExponent = value;
                }
            }
        }

        //private PrintScreen printscreen = new PrintScreen(onMode);
        private PrintScreen printscreen;

        public PrintScreen Printscreen
        {
            get { return printscreen; }
            set 
            {
                if (value != null)
                {
                    printscreen = value;
                }
            }
        }

        private OnlineMode onMode = null;

        public OnlineMode OnlineModeInstance
        {
            get { return onMode; }
            set
            {
                if (value != null)
                {
                    onMode = value;
                }
            }
        }

        private double l0;
        public double L0 
        {
            get { return l0; }
            set { l0 = value; }
        }

        private double lu;
        public double Lu
        {
            get { return lu; }
            set { lu = value; }
        }

        private double s0;
        public double S0
        {
            get { return s0; }
            set { s0 = value; }
        }

        private double su;
        public double Su
        {
            get { return su; }
            set { su = value; }
        }

        private double reL;
        public double ReL
        {
            get { return reL; }
            set { reL = value; }
        }
        private int reLIndex;
        public int ReLIndex
        {
            get { return reLIndex; }
            set { reLIndex = value; }
        }

        private double reH;
        public double ReH
        {
            get { return reH; }
            set { reH = value; }
        }
        private int reHIndex;
        public int ReHIndex
        {
            get { return reHIndex; }
            set { reHIndex = value; }
        }

        private double rm;
        public double Rm
        {
            get { return rm; }
            set { rm = value; }
        }

        private double fm;
        public double Fm
        {
            get { return fm; }
            set { fm = value; }
        }

        private double rp02;
        public double Rp02RI
        {
            get { return rp02; }
            set { rp02 = value; }
        }
        private double _rp02XValue;
        public double Rp02RIXValue
        {
            get { return _rp02XValue; }
            set { _rp02XValue = value; }
        }
        private int rp02Index = 0;
        public int Rp02Index
        {
            get { return rp02Index; }
            set { rp02Index = value; }
        }

        private int rp005Index = 0;
        public int Rp005Index
        {
            get { return rp005Index; }
            set { rp005Index = value; }
        }
        private double rt05;
        public double Rt05
        {
            get { return rt05; }
            set { rt05 = value; }
        }

        private double axManual;
        private double ayManual;
        private double a;
        public double A
        {
            get { return a; }
            set { a = value; }
        }
        public double A_FirstCalculated { get; set; }

        private double at;
        public double At
        {
            get { return at; }
            set { at = value; }
        }


        private double ag;
        public double Ag
        {
            get { return ag; }
            set { ag = value; }
        }

        private double agt;
        public double Agt
        {
            get { return agt; }
            set { agt = value; }
        }

        private bool isRectangle = false;
        public bool IsRectangle
        {
            get { return isRectangle; }
            set { isRectangle = value; }
        }

        private string _au;
        public string au 
        {
            get { return _au; }
            set { _au = value; }
        }

        private string _bu;
        public string bu
        {
            get { return _bu; }
            set { _bu = value; }
        }

        private bool isCircle = false;
        public bool IsCircle
        {
            get { return isCircle; }
            set { isCircle = value; }
        }

        private string _Du;
        public string Du
        {
            get { return _Du; }
            set { _Du = value; }
        }

        private double f;
        public double F
        {
            get { return f; }
            set { f = value; }
        }

      

        private string suStr;
        public string SuStr 
        {
            get { return suStr; }
            set { suStr = value; }
        }
        private string zStr;
        public string ZStr
        {
            get { return zStr; }
            set { zStr = value; }
        }

        /// <summary>
        /// maksimalna Promena napona u jednoj sekundi
        /// </summary>
        private double rmaxwithPoint = -1;
        /// <summary>
        /// maksimalna Promena napona u jednoj sekundi
        /// </summary>
        public double RmaxwithPoint
        {
            get { return rmaxwithPoint; }
            set { rmaxwithPoint = value; }
        }



        ///// <summary>
        ///// maksimalna Promena izduzenja u jednoj sekundi
        ///// </summary>
        //private double emaxwithPoint = -1;
        ///// <summary>
        ///// maksimalna Promena izduzenja u jednoj sekundi
        ///// </summary>
        //public double EmaxwithPoint
        //{
        //    get { return emaxwithPoint; }
        //    set { emaxwithPoint = value; }
        //}

        private double e2MinValue = -1;
        public double E2MinValue
        {
            get { return e2MinValue; }
            set { e2MinValue = value; }
        }

        private double e2MaxValue = -1;
        public double E2MaxValue
        {
            get { return e2MaxValue; }
            set { e2MaxValue = value; }
        }

        private double e2AvgValue = -1;
        public double E2AvgValue
        {
            get { return e2AvgValue; }
            set { e2AvgValue = value; }
        }

        private double e4AvgValue = -1;
        public double E4AvgValue
        {
            get { return e4AvgValue; }
            set { e4AvgValue = value; }
        }

        private double e4MinValue = -1;
        public double E4MinValue
        {
            get { return e4MinValue; }
            set { e4MinValue = value; }
        }

        private double e4MaxValue = -1;
        public double E4MaxValue
        {
            get { return e4MaxValue; }
            set { e4MaxValue = value; }
        }

        private double borderElastic = -1;
        public double BorderElastic
        {
            get { return borderElastic; }
            set { borderElastic = value; }
        }
        /// <summary>
        /// Jungov moduo elasticnosti 10^9 N/m^2
        /// </summary>
        private double yungsModuo = -1;
        /// <summary>
        /// Jungov moduo elasticnosti 10^9 N/m^2
        /// </summary>
        public double YungsModuo
        {
            get { return yungsModuo; }
            set { yungsModuo = value; }
        }

        /// <summary>
        /// granica elasticnosti
        /// </summary>
        private double re = -1;
        /// <summary>
        /// granica elasticnosti
        /// </summary>
        public double Re
        {
            get { return re; }
            set { re = value; }
        }

        //public long NumberOfClickOnOriginalRadioButton
        //{
        //    get { return numberOfClickOnOriginalRadioButton; }
        //    set { numberOfClickOnOriginalRadioButton = value; }
        //}
        //private long numberOfClickOnOriginalRadioButton = 0;

        private double _RRm;
        public double RRm
        {
            get { return _RRm; }
            set { _RRm = value; }
        }

        private double ReHXFromoriginalData = 0;
        private double ReLXFromoriginalData = 0;
        private double RmXFromoriginalData = 0;
        private int indexReHFromoriginalData = 0;
        private int indexReLFromoriginalData = 0;
        private int indexRmFromoriginalData = 0;
        //private double ReH = 0;
        //private double ReL = 0;
        //private double Rm = 0;
        private MarkerTriangleCurrentValues mkrTriangleCurrentValues;
        public MarkerTriangleCurrentValues MkrTriangleCurrentValues
        {
            get 
            {
                return mkrTriangleCurrentValues;
            }
            set 
            {
                if (value != null)
                {
                    mkrTriangleCurrentValues = value;
                }
            }
        }

        private bool isRationChanged = false;
        public bool IsWindowForChosingPointsShown = false;
        public bool IsWindowForMovingRp02DirectionShown = false;
        public bool T3movingDirectionByYAxis = true;//T3 se postavlja samo po Y osi
        public bool Rp02movingDirectionByYAxis = false;//Rp02 se pomera samo po X osi u rucnom modu postavljanja

        /// <summary>
        /// ucitavanje opcija offline moda
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
                       
                        if (textReader.Name.Equals("Resolution"))
                        {
                            OptionsInPlottingMode.Resolution = textReader.ReadElementContentAsInt();
                            this.OptionsPlotting.tfResolution.Text = OptionsInPlottingMode.Resolution.ToString();
                            //this.OptionsPlotting.tfResolution.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfResolution.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("DerivationResolution"))
                        {
                            OptionsInPlottingMode.DerivationResolution = textReader.ReadElementContentAsInt();
                            this.OptionsPlotting.tfDerivationResolution.Text = OptionsInPlottingMode.DerivationResolution.ToString();
                            //this.OptionsPlotting.tfDerivationResolution.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfDerivationResolution.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("nutnDivide"))
                        {
                            OptionsInPlottingMode.nutnDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfCalForceDivide.Text = OptionsInPlottingMode.nutnDivide.ToString();
                            //this.OptionsPlotting.tfCalForceDivide.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfCalForceDivide.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("nutnMultiple"))
                        {
                            OptionsInPlottingMode.nutnMultiple = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfCalForceMultiple.Text = OptionsInPlottingMode.nutnMultiple.ToString();
                            //this.OptionsPlotting.tfCalForceMultiple.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfCalForceMultiple.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmDivide"))
                        {
                            OptionsInPlottingMode.mmDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfCalElonDivide.Text = OptionsInPlottingMode.mmDivide.ToString();
                            //this.OptionsPlotting.tfCalElonDivide.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfCalElonDivide.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmCoeff"))
                        {
                            OptionsInPlottingMode.mmCoeff = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfCalElonMultiple.Text = OptionsInPlottingMode.mmCoeff.ToString();
                            //this.OptionsPlotting.tfCalElonMultiple.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfCalElonMultiple.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmDivideWithEkstenziometer"))
                        {
                            OptionsInPlottingMode.mmDivideWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfCalElonDivide2.Text = OptionsInPlottingMode.mmDivideWithEkstenziometer.ToString();
                            //this.OptionsPlotting.tfCalElonDivide2.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfCalElonDivide2.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmCoeffWithEkstenziometer"))
                        {
                            OptionsInPlottingMode.mmCoeffWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfCalElonMultiple2.Text = OptionsInPlottingMode.mmCoeffWithEkstenziometer.ToString();
                            //this.OptionsPlotting.tfCalElonMultiple2.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfCalElonMultiple2.Foreground = Brushes.White;
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
                            this.tfRatioElongation.Text = OptionsInPlottingMode.xRange.ToString();
                            this.tfRatioElongation.Foreground = Brushes.Black;
                            //this.tfRatioElongation.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("ratioForce"))
                        {
                            OptionsInPlottingMode.yRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.tfRatioForce.Text = OptionsInPlottingMode.yRange.ToString();
                            this.tfRatioForce.Foreground = Brushes.Black;
                            //this.tfRatioForce.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("filePath"))
                        {
                            OptionsInPlottingMode.filePath = textReader.ReadElementContentAsString();
                            this.tfFilepathPlotting.Text = OptionsInPlottingMode.filePath;
                            this.tfFilepathPlotting.Foreground = Brushes.Black;
                            //this.tfFilepathPlotting.Foreground = Brushes.White;
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
                            this.tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                            this.tffittingCrossheadPointX.Foreground = Brushes.Black;
                            //this.tffittingCrossheadPointX.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("pointCrossheadY"))
                        {
                            OptionsInPlottingMode.pointCrossheadY = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                            this.tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();
                            this.tffittingCrossheadPointY.Foreground = Brushes.Black;
                            //this.tffittingCrossheadPointY.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("pointAutoX1"))
                        {
                            OptionsInPlottingMode.pointAutoX1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointAutoX1 = Math.Round(OptionsInPlottingMode.pointAutoX1, 6);
                        }
                        if (textReader.Name.Equals("pointAutoY1"))
                        {
                            OptionsInPlottingMode.pointAutoY1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointAutoY1 = Math.Round(OptionsInPlottingMode.pointAutoY1, 0);
                        }
                        if (textReader.Name.Equals("pointAutoX2"))
                        {
                            OptionsInPlottingMode.pointAutoX2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointAutoX2 = Math.Round(OptionsInPlottingMode.pointAutoX2, 6);
                        }
                        if (textReader.Name.Equals("pointAutoY2"))
                        {
                            OptionsInPlottingMode.pointAutoY2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointAutoY2 = Math.Round(OptionsInPlottingMode.pointAutoY2, 0);
                        }
                        if (textReader.Name.Equals("pointAutoX3"))
                        {
                            OptionsInPlottingMode.pointAutoX3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointAutoX3 = Math.Round(OptionsInPlottingMode.pointAutoX3, 6);
                        }
                        if (textReader.Name.Equals("pointAutoY3"))
                        {
                            OptionsInPlottingMode.pointAutoY3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.pointAutoY3 = Math.Round(OptionsInPlottingMode.pointAutoY3, 0);
                        }
                        if (textReader.Name.Equals("procentAuto1"))
                        {
                            OptionsInPlottingMode.procentAuto1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tffittingAutoProcent1.Text = OptionsInPlottingMode.procentAuto1.ToString();
                            //this.OptionsPlotting.tffittingAutoProcent1.Foreground = Brushes.Black;
                            this.OptionsPlotting.tffittingAutoProcent1.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("procentAuto2"))
                        {
                            OptionsInPlottingMode.procentAuto2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tffittingAutoProcent2.Text = OptionsInPlottingMode.procentAuto2.ToString();
                            //this.OptionsPlotting.tffittingAutoProcent2.Foreground = Brushes.Black;
                            this.OptionsPlotting.tffittingAutoProcent2.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("procentAuto3"))
                        {
                            OptionsInPlottingMode.procentAuto3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tffittingAutoProcent3.Text = OptionsInPlottingMode.procentAuto3.ToString();
                            //this.OptionsPlotting.tffittingAutoProcent3.Foreground = Brushes.Black;
                            this.OptionsPlotting.tffittingAutoProcent3.Foreground = Brushes.White;

                            if (textReader.Name.Equals("pointManualX1"))
                            {
                                OptionsInPlottingMode.pointManualX1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualX1 = Math.Round(OptionsInPlottingMode.pointManualX1, 6);
                                this.tffittingManPoint1X.Text = OptionsInPlottingMode.pointManualX1.ToString();
                                this.tffittingManPoint1X.Foreground = Brushes.Black;
                                //this.tffittingManPoint1X.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("pointManualY1"))
                            {
                                OptionsInPlottingMode.pointManualY1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualY1 = Math.Round(OptionsInPlottingMode.pointManualY1, 0);
                                this.tffittingManPoint1Y.Text = OptionsInPlottingMode.pointManualY1.ToString();
                                //this.tffittingManPoint1Y.Foreground = Brushes.Black;
                                this.tffittingManPoint1Y.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("pointManualX2"))
                            {
                                OptionsInPlottingMode.pointManualX2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualX2 = Math.Round(OptionsInPlottingMode.pointManualX2, 6);
                                this.tffittingManPoint2X.Text = OptionsInPlottingMode.pointManualX2.ToString();
                                //this.tffittingManPoint2X.Foreground = Brushes.Black;
                                this.tffittingManPoint2X.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("pointManualY2"))
                            {
                                OptionsInPlottingMode.pointManualY2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualY2 = Math.Round(OptionsInPlottingMode.pointManualY2, 0);
                                this.tffittingManPoint2Y.Text = OptionsInPlottingMode.pointManualY2.ToString();
                                //this.tffittingManPoint2Y.Foreground = Brushes.Black;
                                this.tffittingManPoint2Y.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("pointManualX3"))
                            {
                                OptionsInPlottingMode.pointManualX3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3,6);
                                this.tffittingManPoint3X.Text = OptionsInPlottingMode.pointManualX3.ToString();
                                //this.tffittingManPoint3X.Foreground = Brushes.Black;
                                this.tffittingManPoint3X.Foreground = Brushes.White;
                            }
                            if (textReader.Name.Equals("pointManualY3"))
                            {
                                OptionsInPlottingMode.pointManualY3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                                OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);
                                this.tffittingManPoint3Y.Text = OptionsInPlottingMode.pointManualY3.ToString();
                                //this.tffittingManPoint3Y.Foreground = Brushes.Black;
                                this.tffittingManPoint3Y.Foreground = Brushes.White;
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
                            this.OptionsPlotting.tfFittingAutoMaxXValue.Text = OptionsInPlottingMode.fittingAutoMaxXValue.ToString();
                            //this.OptionsPlotting.tfFittingAutoMaxXValue.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfFittingAutoMaxXValue.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("ReHXRange"))
                        {
                            OptionsInPlottingMode.ReHXRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfReHXRange.Text = OptionsInPlottingMode.ReHXRange.ToString();
                            //this.OptionsPlotting.tfReHXRange.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfReHXRange.Foreground = Brushes.White;
                        }
                        
                        if (textReader.Name.Equals("YieldInterval"))
                        {
                            OptionsInPlottingMode.YieldInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfYield.Text = OptionsInPlottingMode.YieldInterval.ToString();
                            //this.OptionsPlotting.tfYield.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfYield.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("MaxSubBetweenReLAndReLF"))
                        {
                            OptionsInPlottingMode.MaxSubBetweenReLAndReLF = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfMaxSubsReLFReL.Text = OptionsInPlottingMode.MaxSubBetweenReLAndReLF.ToString();
                            //this.OptionsPlotting.tfMaxSubsReLFReL.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfMaxSubsReLFReL.Foreground = Brushes.White;
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
                            this.OptionsPlotting.tfRationReLRpmaxForOnlyReL.Text = OptionsInPlottingMode.RationReLRpmaxForOnlyReL.ToString();
                            //this.OptionsPlotting.tfRationReLRpmaxForOnlyReL.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfRationReLRpmaxForOnlyReL.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("MinPossibleValueForOnlyReLElongationInProcent"))
                        {
                            OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = Math.Round(OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent, 1);
                            this.OptionsPlotting.tfXReLForOnlyReL.Text = OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent.ToString();
                            //this.OptionsPlotting.tfXReLForOnlyReL.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfXReLForOnlyReL.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("OnlyReLPreassureUnit"))
                        {
                            OptionsInPlottingMode.OnlyReLPreassureUnit = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfOnlyReLPreassureUnit.Text = OptionsInPlottingMode.OnlyReLPreassureUnit.ToString();
                            //this.OptionsPlotting.tfOnlyReLPreassureUnit.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfOnlyReLPreassureUnit.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("currK"))
                        {
                            this.CurrK = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("currN"))
                        {
                            this.CurrN = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
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
                            this.OptionsPlotting.tfTearingPointCoeff.Text = OptionsInPlottingMode.TearingPointCoeff.ToString();
                            //this.OptionsPlotting.tfTearingPointCoeff.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfTearingPointCoeff.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("TearingMinFallPreassure"))
                        {
                            OptionsInPlottingMode.TearingMinFallPreassure = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfTearingMinFallPreassure.Text = OptionsInPlottingMode.TearingMinFallPreassure.ToString();
                            //this.OptionsPlotting.tfTearingMinFallPreassure.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfTearingMinFallPreassure.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("ResolutionForTearing"))
                        {
                            OptionsInPlottingMode.ResolutionForTearing = textReader.ReadElementContentAsInt();
                            this.OptionsPlotting.tfResolutionForTearing.Text = OptionsInPlottingMode.ResolutionForTearing.ToString();
                            //this.OptionsPlotting.tfResolutionForTearing.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfResolutionForTearing.Foreground = Brushes.White;
                        }



                        if (textReader.Name.Equals("DefaultPreassureOfTearingInProcent"))
                        {
                            OptionsInPlottingMode.DefaultPreassureOfTearingInProcent = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfDefaultPreassureOfTearing.Text = OptionsInPlottingMode.DefaultPreassureOfTearingInProcent.ToString();
                            //this.OptionsPlotting.tfDefaultPreassureOfTearing.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfDefaultPreassureOfTearing.Foreground = Brushes.White;
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
                            this.OptionsPlotting.tfBeginIntervalForN.Text = OptionsInPlottingMode.BeginIntervalForN.ToString();
                            //this.OptionsPlotting.tfBeginIntervalForN.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfBeginIntervalForN.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("EndIntervalForN"))
                        {
                            OptionsInPlottingMode.EndIntervalForN = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsPlotting.tfEndIntervalForN.Text = OptionsInPlottingMode.EndIntervalForN.ToString();
                            //this.OptionsPlotting.tfEndIntervalForN.Foreground = Brushes.Black;
                            this.OptionsPlotting.tfEndIntervalForN.Foreground = Brushes.White;
                        }


                        if (textReader.Name.Equals("YungXelas"))
                        {
                            OptionsInPlottingMode.ReEqualsRp = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.optionsForYungsModuo.tfXelas.Text = OptionsInPlottingMode.ReEqualsRp.ToString();
                            //this.optionsForYungsModuo.tfXelas.Foreground = Brushes.Black;
                            this.optionsForYungsModuo.tfXelas.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("YungPrSpustanja"))
                        {
                            OptionsInPlottingMode.YungPrSpustanja = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.optionsForYungsModuo.tfprocspustanja.Text = OptionsInPlottingMode.YungPrSpustanja.ToString();
                            //this.optionsForYungsModuo.tfprocspustanja.Foreground = Brushes.Black;
                            this.optionsForYungsModuo.tfprocspustanja.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("YungLastCalculated"))
                        {
                            YungsModuo = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            //this.tfRatioForce.Foreground = Brushes.White;
                        }

                    }
                }//end of while loop

                textReader.Close();
                //this.createOfflineGraphics(); ovo je glavna razlika u odnosu na onaj iz main poziva za loadoptions

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void LoadPlottingoptions()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ucitaj maksimalnu promenu napona iz animacionog fajla da bi tu vrednost kasnije postavio u izlazni interfejs [resultsInterface]
        /// </summary>
        /// <param name="filePath">napomena: treba extenziju sa .txt promeniti na .anim ekstenziju</param>
        //private double loadAnimationSettings_OnlyMaxChangeOfPreassures(string fileP)
        //{
        //    try
        //    {
        //        string filePath = fileP.Replace(".txt",".anim");
        //        List<string> lines = File.ReadAllLines(filePath).ToList();
        //        int cnt = 0;
        //        bool isFound = false;
        //        while (isFound == false && cnt < 50)
        //        {
        //            string stringForCheck = lines[cnt].Split(':')[0];
        //            if (Constants.ANIMATIONFILEHEADER_L0.Contains(stringForCheck) == true)
        //            {
        //                isFound = true;
        //                double l0;
        //                string strL0 = lines[cnt].Split(':')[1];
        //                strL0 = strL0.Replace(',', '.');
        //                bool isN = Double.TryParse(strL0, out l0);
        //                if (isN == true)
        //                {
        //                    OptionsInAnimation.l0 = l0;
        //                }
        //            }
        //            cnt++;
        //        }
        //        if (cnt > 50)
        //        {
        //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
        //        }

        //        /* get max change of preassure */
        //        double maxChangeOfPreassure = Double.MinValue;
        //        double elongationForMaxChangeOfPreassure = Double.MinValue;
        //        cnt = 0;
        //        isFound = false;
        //        while (isFound == false && cnt < 50)
        //        {
        //            string stringForCheck = lines[cnt].Split(':')[0];
        //            if (Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure.Contains(stringForCheck) == true)
        //            {
        //                isFound = true;

        //                List<string> strmaxChangeOfPreassureANDelongationForMaxChangeOfPreassure = lines[cnt].Split(':')[1].Split('~').ToList();
        //                string strmaxChangeOfPreassure = strmaxChangeOfPreassureANDelongationForMaxChangeOfPreassure.ElementAt(0);
        //                string strelongationForMaxChangeOfPreassure = strmaxChangeOfPreassureANDelongationForMaxChangeOfPreassure.ElementAt(1);

        //                strmaxChangeOfPreassure = strmaxChangeOfPreassure.Replace(',', '.');
        //                bool isN = Double.TryParse(strmaxChangeOfPreassure, out maxChangeOfPreassure);
        //                if (isN == true)
        //                {
        //                    OptionsInAnimation.maxChangeOfPreassure = maxChangeOfPreassure;
        //                }

        //                strelongationForMaxChangeOfPreassure = strelongationForMaxChangeOfPreassure.Replace(',', '.');
        //                bool isNN = Double.TryParse(strelongationForMaxChangeOfPreassure, out elongationForMaxChangeOfPreassure);
        //                if (isNN == true)
        //                {
        //                    OptionsInAnimation.elongationForMaxChangeOfPreassure = elongationForMaxChangeOfPreassure;
        //                }
        //            }
        //            cnt++;
        //        }
        //        if (cnt > 50)
        //        {
        //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
        //        }

        //        if (isFound == true)
        //        {
        //            return maxChangeOfPreassure;
        //        }
        //        else
        //        {
        //            return Double.MinValue;
        //        }

        //        /* get max change of preassure */
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Neispravan format animacionog fajla!","GREŠKA U ANIMACIONOM FAJLU");
        //        return Double.MinValue;
        //    }
        //}

        private OptionsForYungsModuo optionsForYungsModuo;

        public OptionsForYungsModuo OptionsForYungsModuo
        {
            get { return optionsForYungsModuo; }
        }

        private OptionsPlotting optionsPlotting;

        public OptionsPlotting OptionsPlotting
        {
            get { return optionsPlotting; }
        }

        private double xTranslateAmountFittingMode = Double.MinValue;
        public double XTranslateAmountFittingMode
        {
            get { return xTranslateAmountFittingMode; }
            set { xTranslateAmountFittingMode = value; }
        }

        private List<double> coefficientsForCrosshead =  new List<double>();
        private Derivation currDerivation;

        private bool isFoundMaxAndMin = false;
        private bool isFoundMinFalseAndMin = false;
        private bool isFoundOnlyReLCase = false;
        public bool IsFoundOnlyReLCase
        {
            get { return isFoundOnlyReLCase; }
            set { isFoundOnlyReLCase = value; }
        }

        MyPointCollection pointsDiscreteForMutualPoint;
        /// <summary>
        /// sadrzi informacije o tackama fitovanog grafika 
        /// koje treba da se menjaju i kada se promene opcije offline moda
        /// i kada se A postavlja rucno
        /// </summary>
        MyPointCollection pointsForFittingGraphic;
        private double xSubstractionForFittingLine;

        //fitting line parameters
        private double currK = -1;
        private double currN = -1;
        public double CurrK 
        {
            get { return currK; }
            set 
            {
               
                    currK = value;
                
            }
        }
        public double CurrN
        {
            get { return currN; }
            set
            {
                
                    currN = value;
                
            }
        }

        private bool isGraphicPlottingLoaded = false;
        private bool isGraphicFittingLoaded = false;//ako je ucitan transliraj ReL,ReH i Rm za vrednost transliranja po X osi 

        public bool IsGraphicPlottingLoaded 
        {
            get { return isGraphicPlottingLoaded; }
            set { isGraphicPlottingLoaded = value; }
        }

        private double currMaxPreassure = 0.0;
        private double currMaxRelativeElongation = 0.0;


        #region ManualFittingModeChosing

        private bool isNothingActive = false;
        public bool IsNothingActive
        {
            get { return isNothingActive; }
            set { isNothingActive = value; }
        }

        private bool isReHActive = false;
        public bool IsReHActive
        {
            get { return isReHActive; }
            set { isReHActive = value; }
        }

        private bool isReLActive = false;
        public bool IsReLActive
        {
            get { return isReLActive; }
            set { isReLActive = value; }
        }

        private bool isRp02Active = false;
        public bool IsRp02Active
        {
            get { return isRp02Active; }
            set { isRp02Active = value; }
        }

        private bool isRmActive = false;
        public bool IsRmActive
        {
            get { return isRmActive; }
            set { isRmActive = value; }
        }

        private bool isAActive = false;
        public bool IsAActive
        {
            get { return isAActive; }
            set { isAActive = value; }
        }

        private bool isT1Active = false;
        public bool IsT1Active
        {
            get { return isT1Active; }
            set { isT1Active = value; }
        }

        private bool isT2Active = false;
        public bool IsT2Active
        {
            get { return isT2Active; }
            set { isT2Active = value; }
        }

        private bool isT3Active = false;
        public bool IsT3Active
        {
            get { return isT3Active; }
            set { isT3Active = value; }
        }

        private bool isR2R4Active = false;
        public bool IsR2R4Active
        {
            get { return isR2R4Active; }
            set { isR2R4Active = value; }
        }

        private bool isOriginalActive = false;
        public bool IsOriginalActive
        {
            get { return isOriginalActive; }
            set { isOriginalActive = value; }
        }

        private bool isChangeRatio = false;
        public bool IsChangeRatio
        {
            get { return isChangeRatio; }
            set { isChangeRatio = value; }
        }

        #endregion

        public CursorCoordinateGraph mouseTrack;


        #region PropertiesRelatedWithGraphicPlotting

        public ChartPlotter Plotter
        {
            get { return plotter; }
            set 
            {
                if (value != null)
                {
                    plotter = value;
                }
            }
        }


        public MarkerPointsGraph MarkerGraph
        {
            get { return _MarkerGraph; }
            set 
            {
                if (value != null)
                {
                    _MarkerGraph = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText
        //{
        //    get { return _MarkerGraphText; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText = value;
        //        }
        //    }
        //}

        public MarkerPointsGraph MarkerGraph2
        {
            get { return _MarkerGraph2; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph2 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText2
        //{
        //    get { return _MarkerGraphText2; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText2 = value;
        //        }
        //    }
        //}

        public MarkerPointsGraph MarkerGraph3
        {
            get { return _MarkerGraph3; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph3 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText3
        //{
        //    get { return _MarkerGraphText3; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText3 = value;
        //        }
        //    }
        //}


        public MarkerPointsGraph MarkerGraph4
        {
            get { return _MarkerGraph4; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph4 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText4
        //{
        //    get { return _MarkerGraphText4; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText4 = value;
        //        }
        //    }
        //}

        public MarkerPointsGraph MarkerGraph5
        {
            get { return _MarkerGraph5; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph5 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText5
        //{
        //    get { return _MarkerGraphText5; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText5 = value;
        //        }
        //    }
        //}


        public MarkerPointsGraph MarkerGraph6
        {
            get { return _MarkerGraph6; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph6 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText6
        //{
        //    get { return _MarkerGraphText6; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText6 = value;
        //        }
        //    }
        //}

        public MarkerPointsGraph MarkerGraph7
        {
            get { return _MarkerGraph7; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph7 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText7
        //{
        //    get { return _MarkerGraphText7; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText7 = value;
        //        }
        //    }
        //}


        public MarkerPointsGraph MarkerGraph8
        {
            get { return _MarkerGraph8; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph8 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText8
        //{
        //    get { return _MarkerGraphText8; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText8 = value;
        //        }
        //    }
        //}

        public MarkerPointsGraph MarkerGraph9
        {
            get { return _MarkerGraph9; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph9 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText9
        //{
        //    get { return _MarkerGraphText9; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText9 = value;
        //        }
        //    }
        //}

        public MarkerPointsGraph MarkerGraph10
        {
            get { return _MarkerGraph10; }
            set
            {
                if (value != null)
                {
                    _MarkerGraph10 = value;
                }
            }
        }

        //public MarkerPointsGraph MarkerGraphText10
        //{
        //    get { return _MarkerGraphText10; }
        //    set
        //    {
        //        if (value != null)
        //        {
        //            _MarkerGraphText10 = value;
        //        }
        //    }
        //}

        #endregion

        #region XYMarkers

        private List<double> xMarkers = new List<double>();
        public List<double> XMarkers
        {
            get { return xMarkers; }
            set 
            {
                if (value != null)
                {
                    xMarkers = value;
                }
            }
        }
        private List<double> yMarkers = new List<double>();
        public List<double> YMarkers
        {
            get { return yMarkers; }
            set
            {
                if (value != null)
                {
                    yMarkers = value;
                }
            }
        }
        private List<double> xMarkersText = new List<double>();
        public List<double> XMarkersText
        {
            get { return xMarkersText; }
            set
            {
                if (value != null)
                {
                    xMarkersText = value;
                }
            }
        }
        private List<double> yMarkersText = new List<double>();
        public List<double> YMarkersText
        {
            get { return yMarkersText; }
            set
            {
                if (value != null)
                {
                    yMarkersText = value;
                }
            }
        }
        private List<double> xMarkers2 = new List<double>();
        public List<double> XMarkers2
        {
            get { return xMarkers2; }
            set
            {
                if (value != null)
                {
                    xMarkers2 = value;
                }
            }
        }
        private List<double> yMarkers2 = new List<double>();
        public List<double> YMarkers2
        {
            get { return yMarkers2; }
            set
            {
                if (value != null)
                {
                    yMarkers2 = value;
                }
            }
        }
        private List<double> xMarkersText2 = new List<double>();
        public List<double> XMarkersText2
        {
            get { return xMarkersText2; }
            set
            {
                if (value != null)
                {
                    xMarkersText2 = value;
                }
            }
        }
        private List<double> yMarkersText2 = new List<double>();
        public List<double> YMarkersText2
        {
            get { return yMarkersText2; }
            set
            {
                if (value != null)
                {
                    yMarkersText2 = value;
                }
            }
        }
        private List<double> xMarkers3 = new List<double>();
        public List<double> XMarkers3
        {
            get { return xMarkers3; }
            set
            {
                if (value != null)
                {
                    xMarkers3 = value;
                }
            }
        }
        private List<double> yMarkers3 = new List<double>();
        public List<double> YMarkers3
        {
            get { return yMarkers3; }
            set
            {
                if (value != null)
                {
                    yMarkers3 = value;
                }
            }
        }
        private List<double> xMarkersText3 = new List<double>();
        public List<double> XMarkersText3
        {
            get { return xMarkersText3; }
            set
            {
                if (value != null)
                {
                    xMarkersText3 = value;
                }
            }
        }
        private List<double> yMarkersText3 = new List<double>();
        public List<double> YMarkersText3
        {
            get { return yMarkersText3; }
            set
            {
                if (value != null)
                {
                    yMarkersText3 = value;
                }
            }
        }
        private List<double> xMarkers4 = new List<double>();
        public List<double> XMarkers4
        {
            get { return xMarkers4; }
            set
            {
                if (value != null)
                {
                    xMarkers4 = value;
                }
            }
        }
        private List<double> yMarkers4 = new List<double>();
        public List<double> YMarkers4
        {
            get { return yMarkers4; }
            set
            {
                if (value != null)
                {
                    yMarkers4 = value;
                }
            }
        }
        private List<double> xMarkersText4 = new List<double>();
        public List<double> XMarkersText4
        {
            get { return xMarkersText4; }
            set
            {
                if (value != null)
                {
                    xMarkersText4 = value;
                }
            }
        }
        private List<double> yMarkersText4 = new List<double>();
        public List<double> YMarkersText4
        {
            get { return yMarkersText4; }
            set
            {
                if (value != null)
                {
                    yMarkersText4 = value;
                }
            }
        }
        private List<double> xMarkers5 = new List<double>();
        public List<double> XMarkers5
        {
            get { return xMarkers5; }
            set
            {
                if (value != null)
                {
                    xMarkers5 = value;
                }
            }
        }
        private List<double> yMarkers5 = new List<double>();
        public List<double> YMarkers5
        {
            get { return yMarkers5; }
            set
            {
                if (value != null)
                {
                    yMarkers5 = value;
                }
            }
        }
        private List<double> xMarkersText5 = new List<double>();
        public List<double> XMarkersText5
        {
            get { return xMarkersText5; }
            set
            {
                if (value != null)
                {
                    xMarkersText5 = value;
                }
            }
        }
        private List<double> yMarkersText5 = new List<double>();
        public List<double> YMarkersText5
        {
            get { return yMarkersText5; }
            set
            {
                if (value != null)
                {
                    yMarkersText5 = value;
                }
            }
        }
        private List<double> xMarkers6 = new List<double>();
        public List<double> XMarkers6
        {
            get { return xMarkers6; }
            set
            {
                if (value != null)
                {
                    xMarkers6 = value;
                }
            }
        }
        private List<double> yMarkers6 = new List<double>();
        public List<double> YMarkers6
        {
            get { return yMarkers6; }
            set
            {
                if (value != null)
                {
                    yMarkers6 = value;
                }
            }
        }
        private List<double> xMarkersText6 = new List<double>();
        public List<double> XMarkersText6
        {
            get { return xMarkersText6; }
            set
            {
                if (value != null)
                {
                    xMarkersText6 = value;
                }
            }
        }
        private List<double> yMarkersText6 = new List<double>();
        public List<double> YMarkersText6
        {
            get { return yMarkersText6; }
            set
            {
                if (value != null)
                {
                    yMarkersText6 = value;
                }
            }
        }
        private List<double> xMarkers7 = new List<double>();
        public List<double> XMarkers7
        {
            get { return xMarkers7; }
            set
            {
                if (value != null)
                {
                    xMarkers7 = value;
                }
            }
        }
        private List<double> yMarkers7 = new List<double>();
        public List<double> YMarkers7
        {
            get { return yMarkers7; }
            set
            {
                if (value != null)
                {
                    yMarkers7 = value;
                }
            }
        }
        private List<double> xMarkersText7 = new List<double>();
        public List<double> XMarkersText7
        {
            get { return xMarkersText7; }
            set
            {
                if (value != null)
                {
                    xMarkersText7 = value;
                }
            }
        }
        private List<double> yMarkersText7 = new List<double>();
        public List<double> YMarkersText7
        {
            get { return yMarkersText7; }
            set
            {
                if (value != null)
                {
                    yMarkersText7 = value;
                }
            }
        }
        private List<double> xMarkers8 = new List<double>();
        public List<double> XMarkers8
        {
            get { return xMarkers8; }
            set
            {
                if (value != null)
                {
                    xMarkers8 = value;
                }
            }
        }
        private List<double> yMarkers8 = new List<double>();
        public List<double> YMarkers8
        {
            get { return yMarkers8; }
            set
            {
                if (value != null)
                {
                    yMarkers8 = value;
                }
            }
        }
        private List<double> xMarkersText8 = new List<double>();
        public List<double> XMarkersText8
        {
            get { return xMarkersText8; }
            set
            {
                if (value != null)
                {
                    xMarkersText8 = value;
                }
            }
        }
        private List<double> yMarkersText8 = new List<double>();
        public List<double> YMarkersText8
        {
            get { return yMarkersText8; }
            set
            {
                if (value != null)
                {
                    yMarkersText8 = value;
                }
            }
        }
        private List<double> xMarkers9 = new List<double>();
        public List<double> XMarkers9
        {
            get { return xMarkers9; }
            set
            {
                if (value != null)
                {
                    xMarkers9 = value;
                }
            }
        }
        private List<double> yMarkers9 = new List<double>();
        public List<double> YMarkers9
        {
            get { return yMarkers9; }
            set
            {
                if (value != null)
                {
                    yMarkers9 = value;
                }
            }
        }
        private List<double> xMarkersText9 = new List<double>();
        public List<double> XMarkersText9
        {
            get { return xMarkersText9; }
            set
            {
                if (value != null)
                {
                    xMarkersText9 = value;
                }
            }
        }
        private List<double> yMarkersText9 = new List<double>();
        public List<double> YMarkersText9
        {
            get { return yMarkersText9; }
            set
            {
                if (value != null)
                {
                    yMarkersText9 = value;
                }
            }
        }
        private List<double> xMarkers10 = new List<double>();
        public List<double> XMarkers10
        {
            get { return xMarkers10; }
            set
            {
                if (value != null)
                {
                    xMarkers10 = value;
                }
            }
        }
        private List<double> yMarkers10 = new List<double>();
        public List<double> YMarkers10
        {
            get { return yMarkers10; }
            set
            {
                if (value != null)
                {
                    yMarkers10 = value;
                }
            }
        }
        private List<double> xMarkersText10 = new List<double>();
        public List<double> XMarkersText10
        {
            get { return xMarkersText10; }
            set
            {
                if (value != null)
                {
                    xMarkersText10 = value;
                }
            }
        }
        private List<double> yMarkersText10 = new List<double>();
        public List<double> YMarkersText10
        {
            get { return yMarkersText10; }
            set
            {
                if (value != null)
                {
                    yMarkersText10 = value;
                }
            }
        }

        #endregion



        private DataReader dataReader;
        public DataReader DataReader
        {
            get { return dataReader; }
            set
            {
                if (value != null)
                {
                    dataReader = value;
                }
            }
        }
        /// <summary>
        /// tacke originalnog nefitovanog grafika
        /// </summary>
        public MyPointCollection points;
        public MyPointCollection PointsOfFittingLine
        {
            get { return pointsOfFittingLine; }
            set 
            {
                if (value != null)
                {
                    pointsOfFittingLine = value;
                }
            }
        }
        private MyPointCollection pointsOfFittingLine;

        private int indexClosestToRedPointGlobal = 0;
     

        private List<string> distances;


        public void DeleteOfflineModeOnly()
        {
            try
            {
                deleteOfflineModeOnly();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DeleteOfflineModeOnly()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// kada nije cekirano da u offline modu zelimo da zadrzimo originalan grafik tada ovom metodom brisemo zeleni grafik
        /// </summary>
       
        private void deleteOfflineModeOnly()
        {
            try
            {
                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Offline Mode").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Offline Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteOfflineModeOnly()}", System.DateTime.Now);
            }
        }

        //uklanjanje tacaka fitovanja
        public void DeleteT1T2T3() 
        {
            try
            {
                _MarkerGraph.DataSource = null;
                _MarkerGraph2.DataSource = null;
                _MarkerGraph3.DataSource = null;

                //_MarkerGraphText.DataSource = null;
                //_MarkerGraphText2.DataSource = null;
                //_MarkerGraphText3.DataSource = null;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DeleteT1T2T3()}", System.DateTime.Now);
            }
        }

        //onemoguci opcije fitovanja ako polje za potvrdu Ukljuci fitovanje nije cekirano
        public void disableFittingPart() 
        {
            try
            {
                chbFittingMode.IsEnabled = false;
                tffittingCrossheadPointX.IsReadOnly = false;
                tffittingCrossheadPointY.IsReadOnly = false;
                tffittingManPoint1X.IsReadOnly = false;
                tffittingManPoint1Y.IsReadOnly = false;
                tffittingManPoint2X.IsReadOnly = false;
                tffittingManPoint2Y.IsReadOnly = false;
                tffittingManPoint3X.IsReadOnly = false;
                tffittingManPoint3Y.IsReadOnly = false;
                optionsPlotting.chbShowOriginalData.IsEnabled = false;
                btnTurnOnFitting.IsEnabled = false;
                btnCalculateTotalRelElong.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void disableFittingPart()}", System.DateTime.Now);
            }
        }

        //omoguci opcije fitovanja ako polje za potvrdu Ukljuci fitovanje je cekirano
        private void enableFittingPart()
        {
            try
            {
                chbFittingMode.IsEnabled = true;
                tffittingCrossheadPointX.IsReadOnly = true;
                tffittingCrossheadPointY.IsReadOnly = true;
                tffittingManPoint1X.IsReadOnly = true;
                tffittingManPoint1Y.IsReadOnly = true;
                tffittingManPoint2X.IsReadOnly = true;
                tffittingManPoint2Y.IsReadOnly = true;
                tffittingManPoint3X.IsReadOnly = true;
                tffittingManPoint3Y.IsReadOnly = true;
                optionsPlotting.chbShowOriginalData.IsEnabled = true;
                btnTurnOnFitting.IsEnabled = true;
                btnCalculateTotalRelElong.IsEnabled = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void enableFittingPart()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// kreira prazan grafik koji se se popuniti kada se ucita zeljeni fajl u offline modu
        /// </summary>
        public void createOfflineGraphics()
        {
            try
            {
                //isOnlineLine = true;
                currMaxPreassure = -1;
                currMaxRelativeElongation = -1;
                distances = new List<string>();

                deleteOfflineModeOnly();



                //ukoliko postoji jedan obrisi ga, jel se uskoro da se napravi novi prazan
                //u koji se moci da se cuvaju informacije o sledecem ucitanom upisu
                int maxPoints = 300000;
                points = new MyPointCollection(maxPoints);
                points.Clear();

                var ds = new EnumerableDataSource<MyPoint>(points);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                //dodavanje praznog offline grafika
                plotter.AddLineGraph(ds, Colors.Green, 2, "Offline Mode"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"

                //ukoliko je cekirano rucno postavljanje razmere 
                //ovde se postavljaju granice grafika koje se kasnije ne mogu vise menjati
                if (OptionsInPlottingMode.isManualChecked)
                {

                    plotter.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                    restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);


                    plotter.Viewport.Restrictions.Add(restr);


                    printscreen.plotterPrint.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restrPrint = new ViewportAxesRangeRestriction();
                    restrPrint.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                    restrPrint.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);


                    printscreen.plotterPrint.Viewport.Restrictions.Add(restrPrint);

                }

                //ukoliko je cekirano automatsko postavljanje razmere ulazi ovde
                if (OptionsInPlottingMode.isAutoChecked)
                {
                    //razmera se manja u odnosu na zadnju isctanu tacku online grafika
                    plotter.FitToView();
                    plotter.Viewport.Restrictions.Clear();

                    printscreen.plotterPrint.FitToView();
                    printscreen.plotterPrint.Viewport.Restrictions.Clear();

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void createOfflineGraphics()}", System.DateTime.Now);
            }
            


        }

        /// <summary>
        /// kreira prazan grafik koji se se popuniti kada se ucita zeljeni fajl u offline modu
        /// ali samo pri pokretanju programa se ovaj metod poziva
        /// </summary>
        private void createOfflineGraphicsInitial()
        {
            try
            {
                //isOnlineLine = true;
                currMaxPreassure = -1;
                currMaxRelativeElongation = -1;
                int maxPoints = 300000;
                points = new MyPointCollection(maxPoints);

                var ds = new EnumerableDataSource<MyPoint>(points);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);




                plotter.AddLineGraph(ds, Colors.Green, 2, "Offline Mode"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"

                if (OptionsInPlottingMode.isManualChecked)
                {
                    // ako je razmera postavljena unapred postavi restrikciju na ose grafika
                    plotter.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                    restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);
                    plotter.Viewport.Restrictions.Add(restr);


                    printscreen.plotterPrint.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restrPrint = new ViewportAxesRangeRestriction();
                    restrPrint.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                    restrPrint.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);


                    printscreen.plotterPrint.Viewport.Restrictions.Add(restrPrint);

                }

                if (OptionsInPlottingMode.isAutoChecked)
                {
                    // u toku samog iscrtavanja prilagodjavace se opsezi grafika na osnovu iscrtanih tacaka
                    plotter.FitToView();
                    plotter.Viewport.Restrictions.Clear();

                    printscreen.plotterPrint.FitToView();
                    printscreen.plotterPrint.Viewport.Restrictions.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void createOfflineGraphicsInitial()}", System.DateTime.Now);
            }

        }

        private void disableManualMenu()
        {
            try
            {
                chbFititngManualMode.IsChecked = true;
                enableOnlyRp02ReLReHRm = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void disableManualMenu()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// posle menjanja Lu-a mozes samo da ucitas drugi fajl 
        /// ili da ponovo ucitas promenjeni fajl klikom na dugme ucitaj izabrani fajl
        /// </summary>
        private void disableGraphicPlotting() 
        {
            try
            {
                chbAutomaticGoToPrintMode.IsEnabled = false;
                chbShowChangeOfRAndEe.IsEnabled = false;
                rbtnAutoOff.IsEnabled = false;
                rbtnManual.IsEnabled = false;
                tfRatioForce.IsEnabled = false;
                tfRatioElongation.IsEnabled = false;
                chbFittingMode.IsEnabled = false;
                disableManualMenu();
                chbFititngManualMode.IsEnabled = false;
                btnTurnOnFitting.IsEnabled = false;
                btnShowOfflineOptions.IsEnabled = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void disableGraphicPlotting()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// posle menjanja Lu-a mozes samo da ucitas drugi fajl 
        /// ili da ponovo ucitas promenjeni fajl 
        /// klikom na dugme 1.UCITAJ IZABRANI FAJL treba Analizu dijagrama ponovo osposobiti
        /// </summary>
        public void DisableGraphicPlotting()
        {
            try
            {
                disableGraphicPlotting();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DisableGraphicPlotting()}", System.DateTime.Now);
            }
        }


        public void EnableGraphicPlotting()
        {
            try
            {
                enableGraphicPlotting();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void EnableGraphicPlotting()}", System.DateTime.Now);
            }
        }


       
        /// <summary>
        /// posle menjanja Lu-a mozes samo da ucitas drugi fajl 
        /// ili da ponovo ucitas promenjeni fajl 
        /// klikom na dugme 1.UCITAJ IZABRANI FAJL treba Analizu dijagrama ponovo osposobiti
        /// </summary>
        private void enableGraphicPlotting()
        {
            try
            {
                chbAutomaticGoToPrintMode.IsEnabled = true;
                chbShowChangeOfRAndEe.IsEnabled = true;
                rbtnAutoOff.IsEnabled = true;
                rbtnManual.IsEnabled = true;
                tfRatioForce.IsEnabled = true;
                tfRatioElongation.IsEnabled = true;
                chbFittingMode.IsEnabled = true;
                enableOnlyRp02ReLReHRm = false;
                chbFititngManualMode.IsEnabled = true;
                btnTurnOnFitting.IsEnabled = true;
                btnShowOfflineOptions.IsEnabled = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void enableGraphicPlotting()}", System.DateTime.Now);
            }
        }
       
        public GraphicPlotting(OnlineMode onlineMode)
        {
            try
            {
                InitializeComponent();
                onMode = onlineMode;
                printscreen = new PrintScreen(this);
                //this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.80);
                //this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.80);
                //setStatusLabels();
                this.DataContext = this;

                //ove markere prikazuju trouglice koji predstavljaju Rp02,Rmax,ReH,ReL,A,T1,T2 i T3 
                xMarkers.Add(-1);
                yMarkers.Add(-1);
                xMarkersText.Add(-1);
                yMarkersText.Add(-1);
                xMarkers2.Add(-1);
                yMarkers2.Add(-1);
                xMarkersText2.Add(-1);
                yMarkersText2.Add(-1);
                xMarkers3.Add(-1);
                yMarkers3.Add(-1);
                xMarkersText3.Add(-1);
                yMarkersText3.Add(-1);
                xMarkers4.Add(-1);
                yMarkers4.Add(-1);
                xMarkersText4.Add(-1);
                yMarkersText4.Add(-1);
                xMarkers5.Add(-1);
                yMarkers5.Add(-1);
                xMarkersText5.Add(-1);
                yMarkersText5.Add(-1);
                xMarkers6.Add(-1);
                yMarkers6.Add(-1);
                xMarkersText6.Add(-1);
                yMarkersText6.Add(-1);
                xMarkers7.Add(-1);
                yMarkers7.Add(-1);
                xMarkersText7.Add(-1);
                yMarkersText7.Add(-1);
                xMarkers8.Add(-1);
                yMarkers8.Add(-1);
                xMarkersText8.Add(-1);
                yMarkersText8.Add(-1);
                xMarkers9.Add(-1);
                yMarkers9.Add(-1);
                xMarkersText9.Add(-1);
                yMarkersText9.Add(-1);
                xMarkers10.Add(-1);
                yMarkers10.Add(-1);
                xMarkersText10.Add(-1);
                yMarkersText10.Add(-1);

                //cuva poslednje vrednosti markera jel se oni mogu odcekirati
                //kada se odcekiraju ovde se pamte njihove vrednoti po x i y osi
                //da bi kada se ponovo cekiraju mogli vratiti 
                mkrTriangleCurrentValues = new MarkerTriangleCurrentValues();
                setMarkerTriangleVisibilityCheckbox();

                mouseTrack = new CursorCoordinateGraph();
                plotter.Children.Add(mouseTrack);
                mouseTrack.ShowHorizontalLine = false;
                mouseTrack.ShowVerticalLine = false;
                currDerivation = new Derivation();

                btnCalculateTotalRelElong.IsEnabled = false;

                optionsPlotting = new OptionsPlotting();
                optionsPlotting.GraphicPlotting = this;
                optionsForYungsModuo = new OptionsForYungsModuo();
                optionsForYungsModuo.Plotting = this;
                LoadPlottingoptions();
                optionsPlotting.Close();



                createOfflineGraphicsInitial();

                //podrazumevano ponasanje pri pokretanju aplikacije je da se omoguci odmah fitovanje zadnje pokidanog uzorka po ugasenoj aplikaciji
                chbFittingMode.IsChecked = true;

                if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                {
                    lblR2R3R4Border.Text = "R2/R4";
                    tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E2E4Border.ToString();
                }
                else
                {
                    lblR2R3R4Border.Text = "R3/R4";
                    tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E3E4Border.ToString();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public GraphicPlotting(OnlineMode onlineMode)}", System.DateTime.Now);
            }
            
        }

        /// <summary>
        /// cekiraj sve vrednosti radio dugmadi na offline modu tj na samom prozoru, misli se na sam offline prozor a ne na prozoyr opcija offline moda
        /// </summary>
        public void setRadioButtons()
        {
            try
            {
                if (OptionsInPlottingMode.isAutoChecked)
                {
                    this.rbtnAutoOff.IsChecked = true;

                    rbtnManual.IsChecked = false;
                    tfRatioForce.IsReadOnly = true;
                    tfRatioElongation.IsReadOnly = true;
                }
                else
                {
                    rbtnAutoOff.IsChecked = false;

                    rbtnManual.IsChecked = true;
                    tfRatioForce.IsReadOnly = false;
                    tfRatioElongation.IsReadOnly = false;
                }



                if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging)
                {
                    this.chbPrikaziOriginalAfterRatioChanging.IsChecked = true;
                }
                else
                {
                    this.chbPrikaziOriginalAfterRatioChanging.IsChecked = false;
                }
                if (OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging)
                {
                    this.chbPrikaziFitovaniAfterRatioChanging.IsChecked = true;
                }
                else
                {
                    this.chbPrikaziFitovaniAfterRatioChanging.IsChecked = false;
                }







                if (OptionsInPlottingMode.isAutoFittingChecked)
                {
                  
                }
                else
                {
                   
                }


                if (OptionsInPlottingMode.isShowOriginalDataGraphic)
                {
                    optionsPlotting.chbShowOriginalData.IsChecked = true;
                }

              
                if (OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja)
                {
                    optionsPlotting.chbNepodrazumevaniModkidanja.IsChecked = true;
                    optionsPlotting.tfResolutionForTearing.IsReadOnly = false;
                    optionsPlotting.tfTearingPointCoeff.IsReadOnly = false;
                    ////optionsPlotting.tfTearingMinFallPreassure.IsReadOnly = true;
                    //optionsPlotting.tfDefaultPreassureOfTearing.IsReadOnly = false;
                    //optionsPlotting.tfTearingMinFallPreassure.IsReadOnly = true;
                    optionsPlotting.tfDefaultPreassureOfTearing.IsReadOnly = true;
                }
                else
                {
                    optionsPlotting.chbNepodrazumevaniModkidanja.IsChecked = false;
                    optionsPlotting.tfResolutionForTearing.IsReadOnly = true;
                    optionsPlotting.tfTearingPointCoeff.IsReadOnly = true;
                    ////optionsPlotting.tfTearingMinFallPreassure.IsReadOnly = false;
                    //optionsPlotting.tfDefaultPreassureOfTearing.IsReadOnly = true;
                    //optionsPlotting.tfTearingMinFallPreassure.IsReadOnly = false;
                    optionsPlotting.tfDefaultPreassureOfTearing.IsReadOnly = false;
                }


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void setRadioButtons()}", System.DateTime.Now);
            }
        }

        public void setMarkerTriangleVisibilityCheckbox() 
        {
            try
            {
                if (onMode != null && onMode.ResultsInterface != null && onMode.ResultsInterface.chbReL.IsChecked == true)
                {
                    chbReLVisibility.IsChecked = true;
                }
                if (onMode != null && onMode.ResultsInterface != null && onMode.ResultsInterface.chbReL.IsChecked == false)
                {
                    chbReLVisibility.IsChecked = false;
                }
                if (onMode != null && onMode.ResultsInterface != null && onMode.ResultsInterface.chbReH.IsChecked == true)
                {
                    chbReHVisibility.IsChecked = true;
                }
                if (onMode != null && onMode.ResultsInterface != null && onMode.ResultsInterface.chbReH.IsChecked == false)
                {
                    chbReHVisibility.IsChecked = false;
                }
                chbRmVisibility.IsChecked = true;

                if (onMode != null && onMode.ResultsInterface != null && onMode.ResultsInterface.chbRp02.IsChecked == true)
                {
                    chbRp02Visibility.IsChecked = true;
                }
                if (onMode != null && onMode.ResultsInterface != null && onMode.ResultsInterface.chbRp02.IsChecked == false)
                {
                    chbRp02Visibility.IsChecked = false;
                }
                //chbRp02Visibility.IsEnabled = false;
                chbAVisibility.IsChecked = true;
                //chbAVisibility.IsEnabled = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void setMarkerTriangleVisibilityCheckbox()}", System.DateTime.Now);
            }
        }

        public void setFittingCheckbox() 
        {
            try
            {
                if (OptionsInPlottingMode.isFittingChecked)
                {
                    chbFittingMode.IsChecked = true;
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        tffittingManPoint1X.IsReadOnly = false;
                        tffittingManPoint1Y.IsReadOnly = false;
                        tffittingManPoint2X.IsReadOnly = false;
                        tffittingManPoint2Y.IsReadOnly = false;
                        tffittingManPoint3X.IsReadOnly = false;
                        tffittingManPoint3Y.IsReadOnly = false;
                    }
                }
                else
                {
                    chbFittingMode.IsChecked = false;


                    tffittingCrossheadPointX.IsReadOnly = false;
                    tffittingCrossheadPointY.IsReadOnly = false;
                    tffittingManPoint1X.IsReadOnly = false;
                    tffittingManPoint1Y.IsReadOnly = false;
                    tffittingManPoint2X.IsReadOnly = false;
                    tffittingManPoint2Y.IsReadOnly = false;
                    tffittingManPoint3X.IsReadOnly = false;
                    tffittingManPoint3Y.IsReadOnly = false;
                    optionsPlotting.chbShowOriginalData.IsEnabled = false;
                    btnTurnOnFitting.IsEnabled = false;
                    tfReL.IsReadOnly = false;
                    tfReH.IsReadOnly = false;
                    tfRp02.IsReadOnly = false;
                    //chbRp02Visibility.IsEnabled = false;
                    //chbRp02Visibility.IsChecked = true;
                }

                if (OptionsInPlottingMode.isShowOriginalDataGraphic)
                {
                    optionsPlotting.chbShowOriginalData.IsChecked = true;
                }
                else
                {
                    optionsPlotting.chbShowOriginalData.IsChecked = false;
                }

                if (OptionsInPlottingMode.isManualFittingChecked == true)
                {
                    chbFititngManualMode.IsChecked = true;
                }
                else
                {
                    chbFititngManualMode.IsChecked = false;
                }

            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void setFittingCheckbox()}", System.DateTime.Now);
            }
        }


        //private void setTextMarkert4()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(xMarkersText4);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(yMarkersText4);
        //    _MarkerGraphText4.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    xMarkersText4[0] = xMarkers4[0] + OptionsInPlottingMode.xRange / 120;
        //    yMarkersText4[0] = yMarkers4[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "Rm";
        //    _MarkerGraphText4.Marker = mkrText;
        //}

        /// <summary>
        /// postavljanje maksimuma grafika
        /// </summary>
        private void setCrossheadingPoint() 
        {
            try
            {
                OptionsInPlottingMode.pointCrossheadX = dataReader.RelativeElongation[getRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadY = dataReader.PreassureInMPa[getRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX, 1);
                tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = OptionsInPlottingMode.pointCrossheadX;
                yMarkers4[0] = OptionsInPlottingMode.pointCrossheadY;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                //setTextMarkert4();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setCrossheadingPoint()}", System.DateTime.Now); 
            }
        }

       

        private void setXmax(out double xmax) 
        {
            try
            {
                double[] relativeElongationArray = dataReader.RelativeElongation.ToArray();
                xmax = relativeElongationArray.Max();
            }
            catch (Exception ex) 
            {
                xmax = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setXmax(out double xmax)}", System.DateTime.Now);
            }
        }

        public void SetRatioAndCalibrationAfterOnlineWriting() 
        {
            try
            {
                OptionsInPlottingMode.nutnDivide = OptionsInOnlineMode.nutnDivide;
                OptionsInPlottingMode.nutnMultiple = OptionsInOnlineMode.nutnMultiple;
                OptionsInPlottingMode.mmDivide = OptionsInOnlineMode.mmDivide;
                OptionsInPlottingMode.mmCoeff = OptionsInOnlineMode.mmCoeff;
                OptionsInPlottingMode.isAutoChecked = OptionsInOnlineMode.isAutoChecked;
                OptionsInPlottingMode.isManualChecked = OptionsInOnlineMode.isManualChecked;
                OptionsInPlottingMode.xRange = OptionsInOnlineMode.xRange;
                OptionsInPlottingMode.yRange = OptionsInOnlineMode.yRange;
                optionsPlotting.tfCalForceDivide.Text = OptionsInPlottingMode.nutnDivide.ToString();
                optionsPlotting.tfCalForceMultiple.Text = OptionsInPlottingMode.nutnMultiple.ToString();
                optionsPlotting.tfCalElonDivide.Text = OptionsInPlottingMode.mmDivide.ToString();
                optionsPlotting.tfCalElonMultiple.Text = OptionsInPlottingMode.mmCoeff.ToString();
                tfRatioElongation.Text = OptionsInPlottingMode.xRange.ToString();
                tfRatioForce.Text = OptionsInPlottingMode.yRange.ToString();
                writeXMLFileOffline();
                //optionsPlotting.tfCalForceDivide.Foreground = Brushes.Black;
                //optionsPlotting.tfCalForceMultiple.Foreground = Brushes.Black;
                //optionsPlotting.tfCalElonDivide.Foreground = Brushes.Black;
                //optionsPlotting.tfCalElonMultiple.Foreground = Brushes.Black;
                optionsPlotting.tfCalForceDivide.Foreground = Brushes.White;
                optionsPlotting.tfCalForceMultiple.Foreground = Brushes.White;
                optionsPlotting.tfCalElonDivide.Foreground = Brushes.White;
                optionsPlotting.tfCalElonMultiple.Foreground = Brushes.White;
                tfRatioForce.Foreground = Brushes.Black;
                tfRatioElongation.Foreground = Brushes.Black;
                //tfRatioForce.Foreground = Brushes.White;
                //tfRatioElongation.Foreground = Brushes.White;

                if (OptionsInPlottingMode.isAutoChecked)
                {
                    rbtnAutoOff.IsChecked = true;
                }
                else
                {
                    rbtnAutoOff.IsChecked = false;
                }
                if (OptionsInPlottingMode.isManualChecked)
                {
                    rbtnManual.IsChecked = true;
                }
                else
                {
                    rbtnManual.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetRatioAndCalibrationAfterOnlineWriting()}", System.DateTime.Now);
            }
        }

        public void SetRpmaxAndFmaxReHANDReL() 
        {
            try
            {
                double rpmax = Double.MinValue;
                setRpmaxAndFmax(out rpmax);

                double min, max;
                int indexOfmin = 0, indexOfmax = 0;
                max = MaxMinReHReL(points, out min, out indexOfmin, out indexOfmax);



                if (isFoundMaxAndMin == false)
                {
                    tfReL.Text = Constants.NOTFOUNDMAXMIN;
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = string.Empty;
                    }
                    ReL = -1;
                    tfReH.Text = Constants.NOTFOUNDMAXMIN;
                    ReH = -1;
                }
                else
                {
                    min = Math.Round(min, 0);
                    tfReL.Text = min.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = min.ToString();
                    }
                    ReL = min;
                    setReLPoint(indexOfmin);
                    max = Math.Round(max, 0);
                    tfReH.Text = max.ToString();
                    ReH = max;
                    setReHPoint(indexOfmax);
                }


                double onlyReL = Double.MaxValue;
                if (isFoundMaxAndMin == false)
                {
                    onlyReL = findMinReLOnly(out indexOfmin);
                }

                if (isFoundOnlyReLCase == true)
                {
                    onlyReL = Math.Round(onlyReL, 0);
                    ReL = onlyReL;
                    tfReL.Text = onlyReL.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = onlyReL.ToString();
                    }
                    setReLPoint(indexOfmin);
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetRpmaxAndFmaxReHANDReL()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje maksimalne sile i proracunatog napona
        /// </summary>
        /// <param name="rpmax"></param>
        public void setRpmaxAndFmax(out double rpmax) 
        {
            try
            {
                double maxforce = Double.MinValue;
                double maxpreassure = Double.MinValue;
                for (int i = 0; i < dataReader.ForceInKN_Offline.Count; i++)
                {
                    if (dataReader.ForceInKN_Offline[i] > maxforce)
                    {
                        maxforce = dataReader.ForceInKN_Offline[i];
                    }
                }

                for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                {
                    if (dataReader.PreassureInMPa[i] > maxpreassure)
                    {
                        if (dataReader.RelativeElongation[i] <= OptionsInPlottingMode.ReHXRange)
                        {
                            continue;
                        }
                        maxpreassure = dataReader.PreassureInMPa[i];
                        indexRmFromoriginalData = i;
                        RmXFromoriginalData = dataReader.RelativeElongation[i];
                        maxpreassure = Math.Round(maxpreassure, 0);
                        //maxforce = maxpreassure * dataReader.S0Offline;
                    }
                }

                maxforce = maxforce * 1000;//calculate in N not in kN
                maxforce = Math.Round(maxforce, 0);
                //tfFm.Text = maxforce.ToString(); //this was calculate in kN

                Fm = maxforce;
                tfFm.Text = maxforce.ToString();//this is calculated in N
                maxpreassure = Math.Round(maxpreassure, 0);
                tfRm.Text = maxpreassure.ToString();
                if ((Double.IsNegativeInfinity(maxpreassure) == true || Double.IsPositiveInfinity(maxpreassure) == true || maxpreassure == Double.MinValue || maxpreassure == Double.MaxValue))
                {
                    tfRm.Text = String.Empty;
                }
                rpmax = maxpreassure;
                Rm = rpmax;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRpmaxAndFmax(out double rpmax)}", System.DateTime.Now);
                rpmax = 0;
            }
        }

        /// <summary>
        /// uzmi indeks gde se nalazi maksimum u nizu tacaka koje prikazuju grafik offline modu
        /// </summary>
        /// <returns></returns>
        public int GetRpmaxIndex()
        {
            try
            {
                return getRpmaxIndex();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public int GetRpmaxIndex()}", System.DateTime.Now);
                return 0;                
            }
        }

        private int getRpmaxIndex()
        {
            try
            {
                if (dataReader == null)
                {
                    return 0;
                }

                int RpmaxIndex = 0;
                double maxpreassure = Double.MinValue;

                int index = 0;

                for (index = 0; index < dataReader.RelativeElongation.Count; index++)
                {
                    if (dataReader.RelativeElongation[index] <= OptionsInPlottingMode.ReHXRange)
                    {
                    }
                    else
                    {
                        break;
                    }
                }



                for (int i = index; i < dataReader.PreassureInMPa.Count; i++)
                {
                    if (dataReader.PreassureInMPa[i] > maxpreassure)
                    {
                        maxpreassure = dataReader.PreassureInMPa[i];
                        RpmaxIndex = i;
                    }
                }

                return RpmaxIndex;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private int getRpmaxIndex()}", System.DateTime.Now);
                return 0;                
            }

        }

        private int getRpmaxIndex(double Rm)
        {
            try
            {
                //if (dataReader == null)
                //{
                //    return 0;
                //}

                int RpmaxIndex = 0;
                //double maxpreassure = Double.MinValue;


                for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                {
                    //if (dataReader.PreassureInMPa[i] > maxpreassure)
                    if (dataReader.PreassureInMPa[i] > Rm)
                    {
                        //maxpreassure = dataReader.PreassureInMPa[i];
                        RpmaxIndex = i;
                        return RpmaxIndex;
                    }
                }

                return 0;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private int getRpmaxIndex(double Rm)}", System.DateTime.Now);
                return 0;
            }

        }

        public void btnPlottingModeClick() 
        {
            try
            {
                btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void btnPlottingModeClick()}", System.DateTime.Now);
            }
        }

        public void calculateParametersAfterOnlineWriting()
        {
            try
            {
                coefficientsForCrosshead.Clear();
                currDerivation.Clear();
                isFoundMaxAndMin = false;
                isFoundMinFalseAndMin = false;
                isFoundOnlyReLCase = false;
                optionsPlotting.chbShowOriginalData.IsEnabled = true;
                tfRp02.Text = String.Empty;
                Rp02RI = -1;
                //chbRp02Visibility.IsEnabled = false;
                //chbRp02Visibility.IsChecked = true;
                _MarkerGraph7.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                tfA.Text = String.Empty;
                A = -1;
                chbAVisibility.IsChecked = true;
                //chbAVisibility.IsEnabled = false;
                deleteFittingPath();

                createOfflineGraphics();
                isGraphicPlottingLoaded = true;
                enableFittingPart();


                lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;
                btnCalculateTotalRelElong.IsEnabled = false;
                dataReader = new DataReader(OptionsInPlottingMode.filePath);
                dataReader.ReadData();

                if (dataReader.FileNotExist == true)
                {
                    MessageBox.Show("Putanja fajla koji se učitava je neispravna!!!");
                    return;
                }
                if (dataReader.IsL0Offline == false || dataReader.IsS0Offline == false)
                {
                    MessageBox.Show("Neispravan format fajla koji se učitava!!!" + System.Environment.NewLine + "Pogrešan format zaglavlja učitanog fajla!!!");
                    return;
                }

                double rpmax = Double.MinValue;
                setRpmaxAndFmax(out rpmax);
                OptionsInPlottingMode.MaxPossibleValueForOnlyReLPreassureInMPa = rpmax * OptionsInPlottingMode.RationReLRpmaxForOnlyReL;
                double xmax = Double.MinValue;
                setXmax(out xmax);
                //OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = xmax * OptionsInPlottingMode.RationXReLXmaxForOnlyReL;

                currDerivation.findCoeffs(dataReader.PreassureInMPa, dataReader.RelativeElongation, OptionsInPlottingMode.DerivationResolution);

                currMaxRelativeElongation = dataReader.RelativeElongation[dataReader.RelativeElongation.Count - 1];

                if (OptionsInPlottingMode.isFittingChecked == true)
                {
                    if (OptionsInPlottingMode.isManualFittingChecked)
                    {
                        setPointAtGraphicY1(OptionsInPlottingMode.pointManualY1);
                        setPointAtGraphicY2(OptionsInPlottingMode.pointManualY2);
                        setPointAtGraphicY3(OptionsInPlottingMode.pointManualY3);

                    }
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        //findandsetCrossheadPoint(currDerivation.Coeffs);
                        optionsPlotting.saveOptionstffittingAutoProcent1_Offline();
                        optionsPlotting.saveOptionstffittingAutoProcent2_Offline();
                        optionsPlotting.saveOptionstffittingAutoProcent3_Offline();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent1();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent2();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent3();
                        setCrossheadingPoint();

                    }
                }

                double min, max;
                int indexOfmin = 0, indexOfmax = 0;
                max = MaxMinReHReL(points, out min, out indexOfmin, out indexOfmax);




                if (isFoundMaxAndMin == false)
                {
                    tfReL.Text = Constants.NOTFOUNDMAXMIN;
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = string.Empty;
                    }
                    ReL = -1;
                    tfReH.Text = Constants.NOTFOUNDMAXMIN;
                    ReH = -1;
                }
                else
                {
                    min = Math.Round(min, 0);
                    ReL = min;
                    tfReL.Text = min.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = min.ToString();
                    }
                    setReLPoint(indexOfmin);
                    max = Math.Round(max, 0);
                    tfReH.Text = max.ToString();
                    ReH = max;
                    setReHPoint(indexOfmax);
                }


                double onlyReL = Double.MaxValue;
                if (isFoundMaxAndMin == false)
                {
                    onlyReL = findMinReLOnly(out indexOfmin);
                }

                if (isFoundOnlyReLCase == true)
                {
                    onlyReL = Math.Round(onlyReL, 0);
                    ReL = onlyReL;
                    tfReL.Text = onlyReL.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = onlyReL.ToString();
                    }
                    setReLPoint(indexOfmin);
                }

                setManualPointsToAutoPointsValue();
                //if autofitting mode checked calculate currK and currN, and used them when user clicks on button btnTurnOnFitting
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void calculateParametersAfterOnlineWriting()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// posle proracunatih vrednosti u zavisnosti od procenata postavljenih na automatsko firovanju izracunaj i ovom metodom postavi ispravne vrednosti u odgovarajuca tekstualna polja
        /// </summary>
        private void setManualFittingPoint() 
        {
            try
            {
                OptionsInPlottingMode.pointManualX1 = Math.Round(OptionsInPlottingMode.pointManualX1, 6);
                OptionsInPlottingMode.pointManualX2 = Math.Round(OptionsInPlottingMode.pointManualX2, 6);
                OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                OptionsInPlottingMode.pointManualY1 = Math.Round(OptionsInPlottingMode.pointManualY1, 0);
                OptionsInPlottingMode.pointManualY2 = Math.Round(OptionsInPlottingMode.pointManualY2, 0);
                OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);

                tffittingManPoint1X.Text = OptionsInPlottingMode.pointManualX1.ToString();
                tffittingManPoint1Y.Text = OptionsInPlottingMode.pointManualY1.ToString();
                tffittingManPoint2X.Text = OptionsInPlottingMode.pointManualX2.ToString();
                tffittingManPoint2Y.Text = OptionsInPlottingMode.pointManualY2.ToString();
                tffittingManPoint3X.Text = OptionsInPlottingMode.pointManualX3.ToString();
                tffittingManPoint3Y.Text = OptionsInPlottingMode.pointManualY3.ToString();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setManualFittingPoint()}", System.DateTime.Now);
            }
        }


        private bool useManualT1orT2 = false;
        private void btnPlottingMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DirectoryInfo d = new DirectoryInfo(Properties.Settings.Default.SaveDirectory);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles("*LuManual.pe"); //Getting Text files
                foreach (var file in Files)
                {
                    File.Delete(file.FullName);
                }

                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfLu.IsReadOnly = false;
                }
                useManualT1orT2 = false;
                NumberOfLuManual = 0;
                //prikazi vidljivim tacke T1, T2 i T3 na grafiku za prikaz dijagrama
                IDontWantShowT1T2T3AtPrintScreen = false;
               
                enableGraphicPlotting();
  
                IsSecondAndMoreLuManual = false;
                if (printscreen.chbChangeOfRAndE.IsChecked == true)
                {
                    printscreen.chbChangeOfRAndE.IsChecked = false;
                }
                if (chbShowChangeOfRAndEe.IsChecked == true)
                {
                    chbShowChangeOfRAndEe.IsChecked = false;
                }
               
                //RmaxwithPoint = loadAnimationSettings_OnlyMaxChangeOfPreassures(OptionsInPlottingMode.filePath);
                

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                //proveri da li mogu da se smanji broj poziva ove dve linije ispod
                dataReader = new DataReader(OptionsInPlottingMode.filePath);
                dataReader.ReadData();
                l0 = dataReader.getL0Offline();
                s0 = dataReader.getS0Offline();
                redrawing = new ReDrawingInOfflineMode(dataReader);
                setMarkerTriangleVisibilityCheckbox();
                coefficientsForCrosshead.Clear();
                currDerivation.Clear();
                isFoundMaxAndMin = false;
                isFoundMinFalseAndMin = false;
                isFoundOnlyReLCase = false;
                optionsPlotting.chbShowOriginalData.IsEnabled = true;
                tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                Rp02RI = -1;
                //chbRp02Visibility.IsEnabled = false;
                //chbRp02Visibility.IsChecked = true;

                /*ovo mora da se pretvori u metodu*/
                _MarkerGraph7.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                _MarkerGraph8.DataSource = null;
                //_MarkerGraphText8.DataSource = null;
                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;
                /*ovo mora da se pretvori u metodu*/

                tfA.Text = String.Empty;
                A = -1;
                chbAVisibility.IsChecked = true;
                //chbAVisibility.IsEnabled = false;
                deleteFittingPath();

                createOfflineGraphics();
                isGraphicPlottingLoaded = true;
                enableFittingPart();

                double kForAdding = 0;
                double currk = 0;
                lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;
                btnCalculateTotalRelElong.IsEnabled = false;
                dataReader = new DataReader(OptionsInPlottingMode.filePath);
                dataReader.ReadData();
                int progressBarValue = 0;

                if (dataReader.FileNotExist == true)
                {
                    MessageBox.Show("Putanja fajla koji se učitava je neispravna!!!");
                    window.currentFileNotExist = true;
                    return;
                }
                if (dataReader.IsL0Offline == false || dataReader.IsS0Offline == false)
                {
                    MessageBox.Show("Neispravan format fajla koji se učitava!!!" + System.Environment.NewLine + "Pogrešan format zaglavlja učitanog fajla!!!");
                    return;
                }

                double rpmax = Double.MinValue;
                setRpmaxAndFmax(out rpmax);
                setRmPoint();
                OptionsInPlottingMode.MaxPossibleValueForOnlyReLPreassureInMPa = rpmax * OptionsInPlottingMode.RationReLRpmaxForOnlyReL;
                double xmax = Double.MinValue;
                setXmax(out xmax);
                //OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = xmax * OptionsInPlottingMode.RationXReLXmaxForOnlyReL;

                currDerivation.findCoeffs(dataReader.PreassureInMPa, dataReader.RelativeElongation, OptionsInPlottingMode.DerivationResolution);


                OfflineProgressInformation offProgressInfo = new OfflineProgressInformation();
                if (onMode.IsStoppedOnlineSample == false)
                {
                    offProgressInfo.Show();
                }
                int step = dataReader.RelativeElongation.Count / (int)offProgressInfo.pbar.Maximum;
                int makerStep = OptionsInPlottingMode.Resolution;
                int numOfDrawnMarkers = 0;
                currMaxPreassure = dataReader.PreassureInMPa[0];

                //create points
                for (int i = 0; i < dataReader.RelativeElongation.Count; )
                {
                    if (dataReader.PreassureInMPa[i] > currMaxPreassure)
                    {
                        currMaxPreassure = dataReader.PreassureInMPa[i];
                    }

                    if (i % (makerStep) == 0)
                    {
                        if (dataReader.RelativeElongation.Count > offProgressInfo.pbar.Maximum)
                        {
                            if (i % step == 0)
                            {

                                updateLabelInfo(offProgressInfo, dataReader.RelativeElongation.Count / makerStep, numOfDrawnMarkers);
                                updateProgressBar(offProgressInfo, progressBarValue++);
                            }
                        }

                        points.Add(new MyPoint(dataReader.PreassureInMPa[i], dataReader.RelativeElongation[i]));
                        numOfDrawnMarkers++;
                    }

                    i = i + 1;

                }
                //add last point from original data
                points.Add(new MyPoint(dataReader.PreassureInMPa[dataReader.PreassureInMPa.Count - 1], dataReader.RelativeElongation[dataReader.RelativeElongation.Count - 1]));

                currMaxRelativeElongation = dataReader.RelativeElongation[dataReader.RelativeElongation.Count - 1];

                //File.WriteAllLines(System.Environment.CurrentDirectory + "\\Distances.txt", distances);

                updateProgressBar(offProgressInfo, (int)offProgressInfo.pbar.Maximum);
                offProgressInfo.Close();


                double min, max;
                int indexOfmin = 0, indexOfmax = 0;
                max = MaxMinReHReL(points, out min, out indexOfmin, out indexOfmax);
               


                if (isFoundMaxAndMin == false)
                {
                    tfReL.Text = Constants.NOTFOUNDMAXMIN;
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = string.Empty;
                    }
                    ReL = -1;
                    tfReH.Text = Constants.NOTFOUNDMAXMIN;
                    ReH = -1;
                }
                else
                {
                    min = Math.Round(min,0);
                    
                    tfReL.Text = min.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = min.ToString();
                    }
                    ReL = min;
                    //ovo je vazna linija koda za postavljanje ReL kad je lazni jednak pravom ReL (tj ne postoji lokalni maksimum)
                    //ova instrukcija ispod vazi samo kada se pravi ReL trazi kao lokalni maksimum  
                    //indexOfmin = indexOfmin - OptionsInPlottingMode.DerivationResolution;//vrati se unazad za jedno mesto da bi se izracunat lazni ReL poklapao sa postavljenim markerom ReL(narandzasti trougao). Marker ce tu i ostati ako se ne pronadje pravi ReL. Jedno mesto odredjeno je rezolucijom izvoda kojom se racuna porast i pad krive
                    setReLPoint(indexOfmin);
                    max = Math.Round(max, 0);
                    tfReH.Text = max.ToString();
                    ReH = max;
                    setReHPoint(indexOfmax);
                }


                //double onlyReL = Double.MaxValue;
                //if (isFoundMaxAndMin == false)
                //{
                //    onlyReL = findMinReLOnly(out indexOfmin);
                //}

                //if (isFoundOnlyReLCase == true)
                //{
                //    onlyReL = Math.Round(onlyReL, 0);
                //    ReL = onlyReL;
                //    tfReL.Text = onlyReL.ToString();
                //    setReLPoint(indexOfmin);
                //}

                //setManualPointsToAutoPointsValue();
                //if autofitting mode checked calculate currK and currN, and used them when user clicks on button btnTurnOnFitting

                if (OptionsInPlottingMode.isFittingChecked == true)
                {
                    if (OptionsInPlottingMode.isManualFittingChecked)
                    {
                        //ovo postavljaj samo kada ide rucna promena Lu-a
                        if (isLuManualChanged == false)
                        {
                            setPointAtGraphicY1(OptionsInPlottingMode.pointManualY1);
                            setPointAtGraphicY2(OptionsInPlottingMode.pointManualY2);
                            setPointAtGraphicY3(OptionsInPlottingMode.pointManualY3);
                        }

                    }
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        //findandsetCrossheadPoint(currDerivation.Coeffs);
                        optionsPlotting.saveOptionstffittingAutoProcent1_Offline();
                        optionsPlotting.saveOptionstffittingAutoProcent2_Offline();
                        optionsPlotting.saveOptionstffittingAutoProcent3_Offline();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent1();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent2();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent3();
                        setCrossheadingPoint();

                    }
                }

                setManualFittingPoint();
                //ako je doslo do klika na crveno dugme (onMode.IsStoppedOnlineSample == true) ili do kraja ispisa (window.IsOnlineModeFinished == true) ne prikazuj u ovoj fazi prozore 
                if (onMode.IsStoppedOnlineSample == true || window.IsOnlineModeFinished == true)
                {
                }
                else
                {
                    //kod fitovanja i cekiranja koriste se podaci iz ulazno izlaznih interfejsa pa ih je neophodno postaviti programski ako to vec korisnik nije uradio
                    //ali ovo samo radi kada se prelazi automatski u print mod, inace ne prikazuj prozore
                    if (chbAutomaticGoToPrintMode.IsChecked == true)
                    {
                        if (onMode != null)
                        {
                            onMode.btnSampleData.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                            onMode.ResultsInterface.Show();
                        }
                    }
                }


                //set R2R4 exacly at the green (original) graphic
                for (int ind = 0; ind < dataReader.RelativeElongation.Count; ind++)
                {
                    if (dataReader.RelativeElongation[ind] >= OptionsInOnlineMode.E2E4Border)
                    {
                        dataReader.PreassureInMPa[ind] = Math.Round(dataReader.PreassureInMPa[ind], 0);
                        setPointAtGraphicR2R4(dataReader.RelativeElongation[ind], dataReader.PreassureInMPa[ind]);

                        break;
                    }

                }

               
                drawFittingGraphic();
            

                double onlyReL = Double.MaxValue;
                if (isFoundMaxAndMin == false)
                {
                    onlyReL = findMinReLOnly(out indexOfmin);
                }

                if (isFoundOnlyReLCase == true)
                {
                    onlyReL = Math.Round(onlyReL, 0);
                    ReL = onlyReL;
                    tfReL.Text = onlyReL.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = onlyReL.ToString();
                    }
                    setReLPoint(indexOfmin);
                }

                setManualPointsToAutoPointsValue();
                

                if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                {
                    deleteOfflineModeOnly();
                }

                setLastCheckedRadioBtnInResultsInterface();

                /*  ovo je prvi deo metode setManualSettedTrianglePoints()  */
                ////if set manaul point for Rp02,A,Rm,ReH i ReL set on Result interface and on graphic
                ////set manual Rp02
                // string currInOutFileName = getCurrentInputOutputFile();
                // if (File.Exists(currInOutFileName) == true)
                // {

                //     List<string> dataListInputOutput = new List<string>();
                //     dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                //     for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                //     {
                //         if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.Rp02Manual) == true)
                //         {
                //             string Rp02YStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                //             string Rp02XStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(4).Trim();
                //             Rp02YStr = Rp02YStr.Replace(',','.');
                //             Rp02XStr = Rp02XStr.Replace(',', '.');
                //             double rp02manual = 0;
                //             double rp02manualX = 0;
                //             bool isN = Double.TryParse(Rp02YStr, out rp02manual);
                //             if (isN)
                //             {
                //                 Rp02RI = rp02manual;
                //                 setResultsInterfaceForManualSetPoint();
                //             }
                //             bool isNN = Double.TryParse(Rp02XStr, out rp02manualX);
                //             if (isNN)
                //             {
                //                 setRp02PointOutsideGraphic(rp02manualX, rp02manual);
                //             }
                //         }
                //     }

                // }

                //setManualSettedTrianglePoints();

                //IWantToBackFirstCalculateYung = true;
                //calculateYungsModuo();
            
                
        
                //double t1y = 0, t1x = 0, t2x = 0, t2y = 0, t3x = 0, t3y = 0;
                //bool isN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                //isN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                //isN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                //isN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                //isN = double.TryParse(tffittingManPoint3X.Text, out t3x);
                //isN = double.TryParse(tffittingManPoint3Y.Text, out t3y);
                //setPointAtGraphicX1_withXY(t1x, t1y);
                //setPointAtGraphicX2_withXY(t2x, t2y);
                //setPointAtGraphicX3_withXY(t3x, t3y);
                //na kraju opet proracunaj jel ce tek drugi put dobro postavlja visina Rel-a u slucaju kada postoji samo konstantan deo tj samo ReL postoji

                               
            }
            catch (Exception ex) 
            {
                ////MessageBox.Show(ex.Message, "btnPlottingMode()");
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void btnPlottingMode_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
                //onMode.btnSampleData.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }
        }


        private void setT1T2T3FromTextboxes() 
        {
            try
            {
                double t1y = 0, t1x = 0, t2x = 0, t2y = 0, t3x = 0, t3y = 0;
                bool isN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                isN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                isN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                isN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                isN = double.TryParse(tffittingManPoint3X.Text, out t3x);
                isN = double.TryParse(tffittingManPoint3Y.Text, out t3y);

                setPointAtGraphicX1_withXY(t1x, t1y);
                setPointAtGraphicX2_withXY(t2x, t2y);
                setPointAtGraphicX3_withXY(t3x, t3y);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setT1T2T3FromTextboxes()}", System.DateTime.Now);
            }
        }
       
        /// <summary>
        /// postavljanje tacaka Rp02,A,Rm,ReL,ReH ako su u nekom trenutku postavljene rucno
        /// ova metoda se poziva prilikom ucitavanja predhodno zapamcenog fajla
        /// </summary>
        private void setManualSettedTrianglePoints() 
        {
            try
            {
                string currInOutFileName = getCurrentInputOutputFile();
                //if set manaul point for Rp02,A,Rm,ReH i ReL set on Result interface and on graphic
                //set manual Rp02

                if (File.Exists(currInOutFileName) == true)
                {

                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                    {
                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.Rp02Manual) == true)
                        {
                            string Rp02YStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                            string Rp02XStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(4).Trim();
                            Rp02YStr = Rp02YStr.Replace(',', '.');
                            Rp02XStr = Rp02XStr.Replace(',', '.');
                            double rp02manual = 0;
                            double rp02manualX = 0;
                            bool isN = Double.TryParse(Rp02YStr, out rp02manual);
                            if (isN)
                            {
                                Rp02RI = rp02manual;
                                setResultsInterfaceForManualSetPoint();
                            }
                            bool isNN = Double.TryParse(Rp02XStr, out rp02manualX);
                            if (isNN)
                            {
                                setRp02PointOutsideGraphic(rp02manualX, rp02manual);
                            }
                        }
                    }

                }


                //set manual A
                if (File.Exists(currInOutFileName) == true)
                {
                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    double ax = 0;
                    bool isManualAFound = false;
                    for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                    {
                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.AManual) == true)
                        {
                            string AXStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                            AXStr = AXStr.Replace(',', '.');

                            bool isN = Double.TryParse(AXStr, out ax);
                            if (isN)
                            {
                                A = ax;
                                isManualAFound = true;
                            }
                        }
                    }
                    setResultsInterfaceForManualSetPoint();
                    if (IsChangedElongationCalibrationparameter == true)
                    {
                        IsChangedElongationCalibrationparameter = false;
                    }
                    else
                    {
                        if (isManualAFound == true)
                        {
                            setAPoint(ax);
                        }
                    }
                }

                //set manual ReH setReHPoint(x,y)
                if (File.Exists(currInOutFileName) == true)
                {

                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    double reHmanual = 0;
                    double reHmanualX = 0;
                    for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                    {
                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.ReHManual) == true)
                        {
                            string ReHYStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                            string ReHXStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(4).Trim();
                            ReHYStr = ReHYStr.Replace(',', '.');
                            ReHXStr = ReHXStr.Replace(',', '.');

                            bool isN = Double.TryParse(ReHYStr, out reHmanual);
                            if (isN)
                            {
                                ReH = reHmanual;
                                setResultsInterfaceForManualSetPoint();
                            }
                            bool isNN = Double.TryParse(ReHXStr, out reHmanualX);
                            if (isNN)
                            {
                                setReHPoint(reHmanualX, reHmanual);
                            }
                        }
                    }
                    //setResultsInterfaceForManualSetPoint();
                    //setReHPoint(reHmanualX, reHmanual);
                }


                //set manual ReL setReLPoint(x,y)
                if (File.Exists(currInOutFileName) == true)
                {

                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    double reLmanual = 0;
                    double reLmanualX = 0;
                    for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                    {
                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.ReLManual) == true)
                        {
                            string ReLYStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                            string ReLXStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(4).Trim();
                            ReLYStr = ReLYStr.Replace(',', '.');
                            ReLXStr = ReLXStr.Replace(',', '.');
                            bool isN = Double.TryParse(ReLYStr, out reLmanual);
                            if (isN)
                            {
                                ReL = reLmanual;
                                setResultsInterfaceForManualSetPoint();
                            }
                            bool isNN = Double.TryParse(ReLXStr, out reLmanualX);
                            if (isNN)
                            {
                                setReLPoint(reLmanualX, reLmanual);
                                printscreen.setReLPoint_Manual(reLmanualX, reLmanual);
                            }
                        }
                    }
                    //setResultsInterfaceForManualSetPoint();
                    //setReLPoint(reLmanualX, reLmanual);
                }


                //set manual Rm setRmPoint(x,y) and Fm
                if (File.Exists(currInOutFileName) == true)
                {

                    List<string> dataListInputOutput = new List<string>();
                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    double rmmanual = 0;
                    double rmmanualX = 0;
                    bool isNN = false;
                    for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                    {
                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.RmManual) == true)
                        {
                            string RmYStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                            string RmXStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(4).Trim();
                            RmYStr = RmYStr.Replace(',', '.');
                            RmXStr = RmXStr.Replace(',', '.');
                            bool isN = Double.TryParse(RmYStr, out rmmanual);
                            if (isN)
                            {
                                Rm = rmmanual;
                                setResultsInterfaceForManualSetPoint();
                            }
                            isNN = Double.TryParse(RmXStr, out rmmanualX);
                            //if (isNN)
                            //{
                            //    setRmPoint(rmmanualX, rmmanual);
                            //}
                        }

                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.FmManual) == true)
                        {
                            string FmYStr = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                            FmYStr = FmYStr.Replace(',', '.');
                            double fmmanual = 0;
                            bool isN = Double.TryParse(FmYStr, out fmmanual);
                            if (isN)
                            {
                                Fm = fmmanual;
                                setResultsInterfaceForManualSetPoint();
                            }
                        }

                    }

                    if (isNN)
                    {
                        MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                        if (window.IsSecondTabWasClicked == true)
                        {
                            window.IsSecondTabWasClicked = false;
                        }
                        else
                        {
                            setRmPoint(rmmanualX, rmmanual);
                        }
                    }
                }

                //and finally set textboxes
                tfReL.Text = ReL.ToString();
                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfReL.Text = ReL.ToString();
                }
                tfReH.Text = ReH.ToString();
                tfRm.Text = Rm.ToString();
                tfFm.Text = Fm.ToString();
                if (Rp02RI > 0)
                {
                    tfRp02.Text = Rp02RI.ToString();
                }
                else
                {
                    tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                }
                if (isAActive == true)
                {
                    AManualClickedValue = Math.Round(AManualClickedValue, 2);
                    tfA.Text = AManualClickedValue.ToString();
                    a = AManualClickedValue;
                }
                else
                {
                    tfA.Text = A.ToString();
                    a = A;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setManualSettedTrianglePoints()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ako se ne zatvori prozor ResultsInterface ostaje nezapamcena zadnja izabrana opcija(misli se na grupu radio dugmadi),
        /// pa se ovom metodom ovaj problem prevazilazi
        /// </summary>
        private void setLastCheckedRadioBtnInResultsInterface()
        {
            //try
            //{
            //    if (onMode.ResultsInterface == null)
            //    {
            //        return;
            //    }



            //    if (ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rp02)
            //    {
            //        onMode.ResultsInterface.rbtnRp02.IsChecked = true;
            //    }

            //    if (ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rt05)
            //    {
            //        onMode.ResultsInterface.rbtnRt05.IsChecked = true;
            //    }

            //    if (ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReH)
            //    {
            //        onMode.ResultsInterface.rbtnReH.IsChecked = true;
            //    }

            //    if (ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReL)
            //    {
            //        onMode.ResultsInterface.rbtnReL.IsChecked = true;
            //    }
            //}
            //catch (Exception ex) 
            //{
            //    Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setLastCheckedRadioBtnInResultsInterface()}", System.DateTime.Now);
            //}
        }

        private void findandsetCrossheadPoint(List<double> coeffs) 
        {
            try
            {
                int currCrossheadPoint = -1;
                int indexOfCrossheadPoint = 0;
                if (points.Count < 500)
                {
                    currCrossheadPoint = Constants.CROSSHEADPOINT1;
                    for (int i = 0; i < coeffs.Count; i++)
                    {
                        if ((coeffs.ElementAt(i) > 0.0 || coeffs.ElementAt(i) == 0.0) && (coeffs.ElementAt(i) < 1.0 || coeffs.ElementAt(i) == 1.0) /*&& points.ElementAt(i).YAxisValue > 6*/)
                        {
                            currCrossheadPoint--;
                            if (currCrossheadPoint == 0)
                            {
                                indexOfCrossheadPoint = i;
                                break;
                            }
                        }
                        else
                        {
                            currCrossheadPoint = Constants.CROSSHEADPOINT1;
                        }

                    }
                }
                else
                {
                    currCrossheadPoint = Constants.CROSSHEADPOINT2;
                    for (int i = 0; i < coeffs.Count; i++)
                    {
                        if ((coeffs.ElementAt(i) > 0.0 || coeffs.ElementAt(i) == 0.0) && (coeffs.ElementAt(i) < 1.0 || coeffs.ElementAt(i) == 1.0)/* && points.ElementAt(i).YAxisValue > 6*/)
                        {
                            currCrossheadPoint--;
                            if (currCrossheadPoint == 0)
                            {
                                indexOfCrossheadPoint = i;
                                //MessageBox.Show(indexOfCrossheadPoint.ToString());
                                break;
                            }
                        }
                        else
                        {
                            currCrossheadPoint = Constants.CROSSHEADPOINT2;
                        }

                    }
                }


                //tffittingCrossheadPointX.Text = points.ElementAt(indexOfCrossheadPoint).XAxisValue.ToString();
                //tffittingCrossheadPointY.Text = points.ElementAt(indexOfCrossheadPoint).YAxisValue.ToString();
                currDerivation.XAxisValues[indexOfCrossheadPoint] = Math.Round(currDerivation.XAxisValues[indexOfCrossheadPoint], 1);
                tffittingCrossheadPointX.Text = currDerivation.XAxisValues[indexOfCrossheadPoint].ToString();
                currDerivation.YAxisValues[indexOfCrossheadPoint] = Math.Round(currDerivation.YAxisValues[indexOfCrossheadPoint], 0);
                tffittingCrossheadPointY.Text = currDerivation.YAxisValues[indexOfCrossheadPoint].ToString();
                OptionsInPlottingMode.pointCrossheadX = currDerivation.XAxisValues[indexOfCrossheadPoint];
                OptionsInPlottingMode.pointCrossheadY = currDerivation.YAxisValues[indexOfCrossheadPoint];
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void findandsetCrossheadPoint(List<double> coeffs)}", System.DateTime.Now);
            }
        }

        private void updateLabelInfo(OfflineProgressInformation info, int count,int currNumOfPointDraw)
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    info.lblInformation.Content = "Ukupan broj tačaka za iscrtavanje je " + count + ". Do sada je iscrtano " + currNumOfPointDraw + " tačaka."; // Do all the ui thread updates here
                }));
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void updateLabelInfo(OfflineProgressInformation info, int count,int currNumOfPointDraw)}", System.DateTime.Now);
            }
        }

        private void updateProgressBar(OfflineProgressInformation info, double value)
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    info.pbar.Value = value; // Do all the ui thread updates here
                }));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void updateProgressBar(OfflineProgressInformation info, double value)}", System.DateTime.Now);
            }
        }


        public void WriteXMLFileOffline() 
        {
            try
            {
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void WriteXMLFileOffline()}", System.DateTime.Now);
            }
        }

        private void writeXMLFileOffline()
        {
            try
            {
                //write in xml file

                double xSubstractionForFitting = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);

                XElement xmlRoot = new XElement("PlottingMode",
                                                new XElement("PlottingModeCurrentOptions",
                                                            new XElement("Resolution", OptionsInPlottingMode.Resolution.ToString()),
                                                            new XElement("DerivationResolution", OptionsInPlottingMode.DerivationResolution.ToString()),
                                                            new XElement("nutnDivide", OptionsInPlottingMode.nutnDivide.ToString()),
                                                            new XElement("nutnMultiple", OptionsInPlottingMode.nutnMultiple.ToString()),
                                                            new XElement("mmDivide", OptionsInPlottingMode.mmDivide.ToString()),
                                                            new XElement("mmCoeff", OptionsInPlottingMode.mmCoeff.ToString()),
                                                            new XElement("mmDivideWithEkstenziometer", OptionsInPlottingMode.mmDivideWithEkstenziometer.ToString()),
                                                            new XElement("mmCoeffWithEkstenziometer", OptionsInPlottingMode.mmCoeffWithEkstenziometer.ToString()),
                                                            new XElement("isAutoChecked", OptionsInPlottingMode.isAutoChecked.ToString()),
                                                            new XElement("isManualChecked", OptionsInPlottingMode.isManualChecked.ToString()),
                                                            new XElement("PrikaziOriginalAfterRatioChanging", OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging.ToString()),
                                                            new XElement("PrikaziFitovaniAfterRatioChanging", OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging.ToString()),
                                                            new XElement("ratioElongation", OptionsInPlottingMode.xRange.ToString()),
                                                            new XElement("ratioForce", OptionsInPlottingMode.yRange.ToString()),
                                                            new XElement("filePath", OptionsInPlottingMode.filePath),

                                                            new XElement("isFittingChecked", OptionsInPlottingMode.isFittingChecked.ToString()),
                                                            new XElement("isAutoFittingChecked", OptionsInPlottingMode.isAutoFittingChecked.ToString()),
                                                            new XElement("isManualFittingChecked", OptionsInPlottingMode.isManualFittingChecked.ToString()),

                                                            new XElement("pointCrossheadX", OptionsInPlottingMode.pointCrossheadX.ToString()),
                                                            new XElement("pointCrossheadY", OptionsInPlottingMode.pointCrossheadY.ToString()),
                                                            new XElement("pointAutoX1", OptionsInPlottingMode.pointAutoX1.ToString()),
                                                            new XElement("pointAutoY1", OptionsInPlottingMode.pointAutoY1.ToString()),
                                                            new XElement("pointAutoX2", OptionsInPlottingMode.pointAutoX2.ToString()),
                                                            new XElement("pointAutoY2", OptionsInPlottingMode.pointAutoY2.ToString()),
                                                            new XElement("pointAutoX3", OptionsInPlottingMode.pointAutoX3.ToString()),
                                                            new XElement("pointAutoY3", OptionsInPlottingMode.pointAutoY3.ToString()),
                                                            new XElement("procentAuto1", OptionsInPlottingMode.procentAuto1.ToString()),
                                                            new XElement("procentAuto2", OptionsInPlottingMode.procentAuto2.ToString()),
                                                            new XElement("procentAuto3", OptionsInPlottingMode.procentAuto3.ToString()),



                                                            new XElement("pointManualX1", OptionsInPlottingMode.pointManualX1.ToString()),
                                                            new XElement("pointManualY1", OptionsInPlottingMode.pointManualY1.ToString()),
                                                            new XElement("pointManualX2", OptionsInPlottingMode.pointManualX2.ToString()),
                                                            new XElement("pointManualY2", OptionsInPlottingMode.pointManualY2.ToString()),
                                                            new XElement("pointManualX3", OptionsInPlottingMode.pointManualX3.ToString()),
                                                            new XElement("pointManualY3", OptionsInPlottingMode.pointManualY3.ToString()),
                                                            new XElement("isShowOriginalDataGraphic", OptionsInPlottingMode.isShowOriginalDataGraphic.ToString()),

                                                            new XElement("fittingAutoMaxXValue", OptionsInPlottingMode.fittingAutoMaxXValue.ToString()),

                                                            new XElement("ReHXRange", OptionsInPlottingMode.ReHXRange.ToString()),
                                                            new XElement("YieldInterval", OptionsInPlottingMode.YieldInterval.ToString()),
                                                            new XElement("MaxSubBetweenReLAndReLF", OptionsInPlottingMode.MaxSubBetweenReLAndReLF.ToString()),
                                                            new XElement("RationReLRpmaxForOnlyReL", OptionsInPlottingMode.RationReLRpmaxForOnlyReL.ToString()),
                                                            new XElement("MinPossibleValueForOnlyReLElongationInProcent", OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent.ToString()),
                                                            new XElement("OnlyReLPreassureUnit", OptionsInPlottingMode.OnlyReLPreassureUnit.ToString()),
                    //(xSubstractionForFitting < 0.15 || xSubstractionForFitting == 0.15)  ? new XElement("currK", "Infinity") : new XElement("currK", currK.ToString()),
                    //(xSubstractionForFitting < 0.15 || xSubstractionForFitting == 0.15)  ? new XElement("currN", "Infinity") : new XElement("currN", currN.ToString()),
                                                            Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK) ? new XElement("currK", "Infinity") : new XElement("currK", currK.ToString()),
                                                            Double.IsPositiveInfinity(currN) || Double.IsNegativeInfinity(currN) ? new XElement("currN", "Infinity") : new XElement("currN", currN.ToString()),

                                                            new XElement("UkljuciNepodrazumevaniModKidanja", OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja.ToString()),
                                                            new XElement("TearingPointCoeff", OptionsInPlottingMode.TearingPointCoeff.ToString()),
                                                            new XElement("TearingMinFallPreassure", OptionsInPlottingMode.TearingMinFallPreassure.ToString()),
                                                            new XElement("ResolutionForTearing", OptionsInPlottingMode.ResolutionForTearing.ToString()),
                                                            new XElement("DefaultPreassureOfTearingInProcent", OptionsInPlottingMode.DefaultPreassureOfTearingInProcent.ToString()),

                                                            new XElement("isOriginalCheckBoxChecked", OptionsInPlottingMode.isOriginalCheckBoxChecked.ToString()),
                                                            new XElement("isChangeRatioCheckBoxChecked", OptionsInPlottingMode.isChangeRatioCheckBoxChecked.ToString()),

                                                            new XElement("BeginIntervalForN", OptionsInPlottingMode.BeginIntervalForN.ToString()),
                                                            new XElement("EndIntervalForN", OptionsInPlottingMode.EndIntervalForN.ToString()),

                                                            new XElement("YungXelas", OptionsInPlottingMode.ReEqualsRp.ToString()),
                                                            new XElement("YungPrSpustanja", OptionsInPlottingMode.YungPrSpustanja.ToString()),
                                                            new XElement("YungLastCalculated", YungsModuo.ToString())

                                                            )
                                               );

                xmlRoot.Save(Constants.plottingModeOptionsXml);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void writeXMLFileOffline()}", System.DateTime.Now);
            }

            //using (XmlWriter writer = XmlWriter.Create(Constants.plottingModeOptionsXml))
            //{
            //    writer.WriteStartDocument();
            //    writer.WriteStartElement("PlottingMode");


            //    writer.WriteStartElement("PlottingModeCurrentOptions");




            //    writer.WriteElementString("Resolution", OptionsInPlottingMode.Resolution.ToString());
            //    writer.WriteElementString("DerivationResolution", OptionsInPlottingMode.DerivationResolution.ToString());
            //    writer.WriteElementString("nutnDivide", OptionsInPlottingMode.nutnDivide.ToString());
            //    writer.WriteElementString("nutnMultiple", OptionsInPlottingMode.nutnMultiple.ToString());
            //    writer.WriteElementString("mmDivide", OptionsInPlottingMode.mmDivide.ToString());
            //    writer.WriteElementString("mmCoeff", OptionsInPlottingMode.mmCoeff.ToString());
            //    writer.WriteElementString("isAutoChecked", OptionsInPlottingMode.isAutoChecked.ToString());
            //    writer.WriteElementString("isManualChecked", OptionsInPlottingMode.isManualChecked.ToString());
            //    writer.WriteElementString("PrikaziOriginalAfterRatioChanging", OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging.ToString());
            //    writer.WriteElementString("PrikaziFitovaniAfterRatioChanging", OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging.ToString());
            //    writer.WriteElementString("ratioElongation", OptionsInPlottingMode.xRange.ToString());
            //    writer.WriteElementString("ratioForce", OptionsInPlottingMode.yRange.ToString());
            //    writer.WriteElementString("filePath", OptionsInPlottingMode.filePath);

            //    writer.WriteElementString("isFittingChecked", OptionsInPlottingMode.isFittingChecked.ToString());
            //    writer.WriteElementString("isAutoFittingChecked", OptionsInPlottingMode.isAutoFittingChecked.ToString());
            //    writer.WriteElementString("isManualFittingChecked", OptionsInPlottingMode.isManualFittingChecked.ToString());

            //    writer.WriteElementString("pointCrossheadX", OptionsInPlottingMode.pointCrossheadX.ToString());
            //    writer.WriteElementString("pointCrossheadY", OptionsInPlottingMode.pointCrossheadY.ToString());
            //    writer.WriteElementString("pointAutoX1", OptionsInPlottingMode.pointAutoX1.ToString());
            //    writer.WriteElementString("pointAutoY1", OptionsInPlottingMode.pointAutoY1.ToString());
            //    writer.WriteElementString("pointAutoX2", OptionsInPlottingMode.pointAutoX2.ToString());
            //    writer.WriteElementString("pointAutoY2", OptionsInPlottingMode.pointAutoY2.ToString());
            //    writer.WriteElementString("pointAutoX3", OptionsInPlottingMode.pointAutoX3.ToString());
            //    writer.WriteElementString("pointAutoY3", OptionsInPlottingMode.pointAutoY3.ToString());
            //    writer.WriteElementString("procentAuto1", OptionsInPlottingMode.procentAuto1.ToString());
            //    writer.WriteElementString("procentAuto2", OptionsInPlottingMode.procentAuto2.ToString());
            //    writer.WriteElementString("procentAuto3", OptionsInPlottingMode.procentAuto3.ToString());
        
        

            //    writer.WriteElementString("pointManualX1", OptionsInPlottingMode.pointManualX1.ToString());
            //    writer.WriteElementString("pointManualY1", OptionsInPlottingMode.pointManualY1.ToString());
            //    writer.WriteElementString("pointManualX2", OptionsInPlottingMode.pointManualX2.ToString());
            //    writer.WriteElementString("pointManualY2", OptionsInPlottingMode.pointManualY2.ToString());
            //    writer.WriteElementString("pointManualX3", OptionsInPlottingMode.pointManualX3.ToString());
            //    writer.WriteElementString("pointManualY3", OptionsInPlottingMode.pointManualY3.ToString());
            //    writer.WriteElementString("isShowOriginalDataGraphic", OptionsInPlottingMode.isShowOriginalDataGraphic.ToString());

            //    writer.WriteElementString("YieldInterval", OptionsInPlottingMode.YieldInterval.ToString());// max substraction for ReH determination
            //    writer.WriteElementString("MaxSubBetweenReLAndReLF", OptionsInPlottingMode.MaxSubBetweenReLAndReLF.ToString());
            //    writer.WriteElementString("RationReLRpmaxForOnlyReL", OptionsInPlottingMode.RationReLRpmaxForOnlyReL.ToString());
            //    writer.WriteElementString("RationXReLXmaxForOnlyReL", OptionsInPlottingMode.RationXReLXmaxForOnlyReL.ToString());
            //    writer.WriteElementString("OnlyReLPreassureUnit", OptionsInPlottingMode.OnlyReLPreassureUnit.ToString());

            //    double xSubstractionForFitting = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);

            //    if (xSubstractionForFitting < 0.15 || xSubstractionForFitting == 0.15)
            //    {
            //        writer.WriteElementString("currK", "Infinity");
            //        writer.WriteElementString("currN", "Infinity");
            //    }
            //    else
            //    {
            //        writer.WriteElementString("currK", currK.ToString());
            //        writer.WriteElementString("currN", currN.ToString());
            //        if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
            //        {
            //            writer.WriteElementString("currK", "Infinity");
            //        }
            //        if (Double.IsPositiveInfinity(currN) || Double.IsNegativeInfinity(currN))
            //        {
            //            writer.WriteElementString("currN", "Infinity");
            //        }
            //    }

            //    writer.WriteElementString("UkljuciNepodrazumevaniModKidanja", OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja.ToString());
            //    writer.WriteElementString("TearingPointCoeff", OptionsInPlottingMode.TearingPointCoeff.ToString());
            //    writer.WriteElementString("TearingMinFallPreassure", OptionsInPlottingMode.TearingMinFallPreassure.ToString());
            //    writer.WriteElementString("ResolutionForTearing", OptionsInPlottingMode.ResolutionForTearing.ToString());
            //    writer.WriteElementString("DefaultPreassureOfTearingInProcent", OptionsInPlottingMode.DefaultPreassureOfTearingInProcent.ToString());

            //    writer.WriteElementString("isOriginalCheckBoxChecked", OptionsInPlottingMode.isOriginalCheckBoxChecked.ToString());


            //    writer.WriteElementString("BeginIntervalForN", OptionsInPlottingMode.BeginIntervalForN.ToString());
            //    writer.WriteElementString("EndIntervalForN", OptionsInPlottingMode.EndIntervalForN.ToString());

            //    writer.WriteEndElement();


            //    writer.WriteEndElement();
            //    writer.WriteEndDocument();
            //    writer.Close();
            //}
        }

        public void DeleteNonFittingPath()
        {
            try
            {
                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Offline Mode").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Offline Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }


                _MarkerGraph7.DataSource = null;// brisati Rp02
                _MarkerGraph8.DataSource = null;// brisati A
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DeleteNonFittingPath()}", System.DateTime.Now);
            }
        }

        public void DeleteFittingPath() 
        {
            try
            {
                deleteFittingPath();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DeleteFittingPath()}", System.DateTime.Now);
            }
        }

        private void deleteFittingPath() 
        {
            try
            {
                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }


                _MarkerGraph.DataSource = null;
                _MarkerGraph2.DataSource = null;
                _MarkerGraph3.DataSource = null;
                _MarkerGraph4.DataSource = null;
                _MarkerGraph5.DataSource = null;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraph7.DataSource = null;ne brisati Rp02
                //_MarkerGraph8.DataSource = null;ne brisati A
                //_MarkerGraphText.DataSource = null;
                //_MarkerGraphText2.DataSource = null;
                //_MarkerGraphText3.DataSource = null;
                //_MarkerGraphText4.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                //_MarkerGraphText6.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                //_MarkerGraphText8.DataSource = null;

                DeleteConnectDiscreteDisplays();
                DeleteConnectTwoDiscreteDisplays();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteFittingPath()}", System.DateTime.Now);
            }
        }

        public void DeleteAFittingAndOfflineLines()
        {
            try
            {
                deleteAFittingAndOfflineLines();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DeleteAFittingAndOfflineLines()}", System.DateTime.Now);
            }
        }

        private void deleteAFittingAndOfflineLines() 
        {
            try
            {
                _MarkerGraph7.DataSource = null;
                _MarkerGraph8.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                //_MarkerGraphText8.DataSource = null;

                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Offline Mode").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Offline Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }

                var numberOfOffline2 = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Count();
                if (numberOfOffline2 > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Single();
                    plotter.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteAFittingAndOfflineLines()}", System.DateTime.Now);
            }
        }

        public void deleteTotalRelativeElongation_RedLine() 
        {
            try
            {
                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Ukupno izduženje").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Ukupno izduženje").Single();
                    plotter.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void deleteTotalRelativeElongation_RedLine()}", System.DateTime.Now);
            }
        }

      

      

        

        #region radioButtonforRange

        private void rbtnAutoOff_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                //double ratioForce = 1.1 * onMode.MaxPreassure;
                double ratioForce = oldYRange;
                ratioForce = Math.Round(ratioForce, 0);
                tfRatioForce.Text = ratioForce.ToString();
                //double ratioElongation = 1.1 * onMode.MaxElongation;
                double ratioElongation = oldXRange;
                ratioElongation = Math.Round(ratioElongation, 0);
                tfRatioElongation.Text = ratioElongation.ToString();
                //drawFittingGraphic();

                isGraphicPlottingLoaded = false;
                disableFittingPart();
                OptionsInPlottingMode.isAutoChecked = false;

                tfRatioForce.IsReadOnly = false;
                tfRatioElongation.IsReadOnly = false;
                chbPrikaziOriginalAfterRatioChanging.IsEnabled = true;
                chbPrikaziFitovaniAfterRatioChanging.IsEnabled = true;

                OptionsInPlottingMode.isManualChecked = true;
                double xRange = 0;
                string strratioElongation = tfRatioElongation.Text.Replace(',', '.');
                bool isN = Double.TryParse(strratioElongation, out xRange);
                if (isN)
                {
                    OptionsInPlottingMode.xRange = xRange;
                }
                double yRange = 0;
                string strratioForce = tfRatioForce.Text.Replace(',', '.');
                isN = Double.TryParse(strratioForce, out yRange);
                if (isN)
                {
                    OptionsInPlottingMode.yRange = yRange;
                }

                writeXMLFileOffline();

                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;

                var numberOfOfflineFM = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOfflineFM > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;

                createOfflineGraphics();

                deleteFittingPath();

                tfReL.Text = String.Empty;
                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfReL.Text = String.Empty;
                }
                ReL = -1;
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                tfReH.Text = String.Empty;
                ReH = -1;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;
                tfRm.Text = String.Empty;
                Rm = -1;
                tfFm.Text = String.Empty;
                Fm = -1;

                _MarkerGraph7.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                tfRp02.Text = String.Empty;
                Rp02RI = -1;

                _MarkerGraph8.DataSource = null;
                //_MarkerGraphText8.DataSource = null;
                tfA.Text = String.Empty;
                A = -1;

                //R2/R4 border
                _MarkerGraph9.DataSource = null;

                btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void rbtnAutoOff_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private double oldXRange = 0;
        private double oldYRange = 0;

        private void rbtnAutoOff_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                oldXRange = OptionsInPlottingMode.xRange;
                oldYRange = OptionsInPlottingMode.yRange;

                isGraphicPlottingLoaded = false;
                disableFittingPart();
                OptionsInPlottingMode.isAutoChecked = true;

                tfRatioForce.IsReadOnly = true;
                tfRatioElongation.IsReadOnly = true;
                //chbPrikaziOriginalAfterRatioChanging.IsEnabled = false;
                //chbPrikaziFitovaniAfterRatioChanging.IsEnabled = false;


                OptionsInPlottingMode.isManualChecked = false;


                writeXMLFileOffline();
                //OptionsInPlottingMode.xRange = 0.95;
                //OptionsInPlottingMode.yRange = 0.95;

                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;

                var numberOfOfflineFM = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOfflineFM > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;

                createOfflineGraphics();

                deleteFittingPath();

                tfReL.Text = String.Empty;
                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfReL.Text = String.Empty;
                }
                ReL = -1;
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                tfReH.Text = String.Empty;
                ReH = -1;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;
                tfRm.Text = String.Empty;
                Rm = -1;
                tfFm.Text = String.Empty;
                Fm = -1;
                _MarkerGraph7.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                Rp02RI = -1;
                _MarkerGraph8.DataSource = null;
                //_MarkerGraphText8.DataSource = null;
                A = -1;


                btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void rbtnAutoOff_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }

        #endregion

       

        #region ratioForce

        private void saveOptionstfRatioForce_Offline()
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
                    OptionsInPlottingMode.yRange = ratioForce;
                }

                writeXMLFileOffline();
                createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstfRatioForce_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfRatioForce_Offline()
        {
            try
            {
                tfRatioForce.Foreground = Brushes.Black;
                //tfRatioForce.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfRatioForce_Offline()}", System.DateTime.Now);
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
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfRatioForce_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioForce_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    isRationChanged = true;
                    //MainWindow window = (MainWindow)Window.GetWindow(this);
                    //if (window.IsSecondTabWasClicked == true)
                    //{
                    //    window.IsSecondTabWasClicked = false;
                    //    return;
                    //}
                    isGraphicPlottingLoaded = false;
                    disableFittingPart();
                    saveOptionstfRatioForce_Offline();
                    markSavedOnlineOptionsAsBlacktfRatioForce_Offline();
                    deleteFittingPath();

                    tfReL.Text = String.Empty;
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = String.Empty;
                    }
                    ReL = -1;
                    _MarkerGraph5.DataSource = null;
                    //_MarkerGraphText5.DataSource = null;
                    tfReH.Text = String.Empty;
                    ReH = -1;
                    _MarkerGraph6.DataSource = null;
                    //_MarkerGraphText6.DataSource = null;


                    //ako zelimo samo originalan da prikazemo
                    if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging && OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging == false)
                    {
                        this.btnPlottingModeClick();
                    }

                    //ako zelimo fitovan prvo moramo ucitati originalan
                    if (OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging)
                    {
                        this.btnPlottingModeClick();
                        if (isGraphicPlottingLoaded)
                        {
                            //ako treba i originalan da se prikaze pored fitovanog postavi polje isShowOriginalDataGraphic na true
                            if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging)
                            {
                                OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                            }
                            else
                            {
                                OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                            }
                            btnTurnOnFitting.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                        }

                    }

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfRatioForce_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        public void ChangePlottingRatioForce() 
        {
            try
            {
                isRationChanged = true;
                //MainWindow window = (MainWindow)Window.GetWindow(this);
                //if (window.IsSecondTabWasClicked == true)
                //{
                //    window.IsSecondTabWasClicked = false;
                //    return;
                //}
                isGraphicPlottingLoaded = false;
                disableFittingPart();
                saveOptionstfRatioForce_Offline();
                markSavedOnlineOptionsAsBlacktfRatioForce_Offline();
                deleteFittingPath();

                tfReL.Text = String.Empty;
                onMode.ResultsInterface.tfReL.Text = String.Empty;
                ReL = -1;
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                tfReH.Text = String.Empty;
                ReH = -1;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;


                //ako zelimo samo originalan da prikazemo
                if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging && OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging == false)
                {
                    this.btnPlottingModeClick();
                }

                //ako zelimo fitovan prvo moramo ucitati originalan
                if (OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging)
                {
                    this.btnPlottingModeClick();
                    if (isGraphicPlottingLoaded)
                    {
                        //ako treba i originalan da se prikaze pored fitovanog postavi polje isShowOriginalDataGraphic na true
                        if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging)
                        {
                            OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                        }
                        else
                        {
                            OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                        }
                        btnTurnOnFitting.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    }

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void ChangePlottingRatioForce()}", System.DateTime.Now);
            }
        }

        #endregion

      

        #region ratioElongation

        private void saveOptionstfRatioElongation_Offline()
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
                    OptionsInPlottingMode.xRange = ratioElongation;
                }

                writeXMLFileOffline();
                createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstfRatioElongation_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfRatioElongation_Offline()
        {
            try
            {
                tfRatioElongation.Foreground = Brushes.Black;
                //tfRatioElongation.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfRatioElongation_Offline()}", System.DateTime.Now);
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
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfRatioElongation_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRatioElongation_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    isRationChanged = true;
                    //MainWindow window = (MainWindow)Window.GetWindow(this);
                    //if (window.IsSecondTabWasClicked == true)
                    //{
                    //    window.IsSecondTabWasClicked = false;
                    //    return;
                    //}
                    isGraphicPlottingLoaded = false;
                    disableFittingPart();
                    saveOptionstfRatioElongation_Offline();
                    markSavedOnlineOptionsAsBlacktfRatioElongation_Offline();
                    deleteFittingPath();

                    tfReL.Text = String.Empty;
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = String.Empty;
                    }
                    ReL = -1;
                    _MarkerGraph5.DataSource = null;
                    //_MarkerGraphText5.DataSource = null;
                    tfReH.Text = String.Empty;
                    ReH = -1;
                    _MarkerGraph6.DataSource = null;
                    //_MarkerGraphText6.DataSource = null;

                    //ako zelimo samo originalan da prikazemo
                    if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging && OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging == false)
                    {
                        this.btnPlottingModeClick();
                    }

                    //ako zelimo fitovan prvo moramo ucitati originalan
                    if (OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging)
                    {
                        this.btnPlottingModeClick();
                        if (isGraphicPlottingLoaded)
                        {
                            //ako treba i originalan da se prikaze pored fitovanog postavi polje isShowOriginalDataGraphic na true
                            if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging)
                            {
                                OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                            }
                            else
                            {
                                OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                            }
                            btnTurnOnFitting.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfRatioElongation_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        public void ChangePlottingRatioElongation()
        {
            try
            {
                isRationChanged = true;
                //MainWindow window = (MainWindow)Window.GetWindow(this);
                //if (window.IsSecondTabWasClicked == true)
                //{
                //    window.IsSecondTabWasClicked = false;
                //    return;
                //}
                isGraphicPlottingLoaded = false;
                disableFittingPart();
                saveOptionstfRatioElongation_Offline();
                markSavedOnlineOptionsAsBlacktfRatioElongation_Offline();
                deleteFittingPath();

                tfReL.Text = String.Empty;
                onMode.ResultsInterface.tfReL.Text = String.Empty;
                ReL = -1;
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                tfReH.Text = String.Empty;
                ReH = -1;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;

                //ako zelimo samo originalan da prikazemo
                if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging && OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging == false)
                {
                    this.btnPlottingModeClick();
                }

                //ako zelimo fitovan prvo moramo ucitati originalan
                if (OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging)
                {
                    this.btnPlottingModeClick();
                    if (isGraphicPlottingLoaded)
                    {
                        //ako treba i originalan da se prikaze pored fitovanog postavi polje isShowOriginalDataGraphic na true
                        if (OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging)
                        {
                            OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                        }
                        else
                        {
                            OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                        }
                        btnTurnOnFitting.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void ChangePlottingRatioElongation()}", System.DateTime.Now);
            }
        }

        #endregion

        #region filepath

        private void btnChooseDatabasePath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = String.Empty;
                string extensionTxt = "txt";
                bool _okDatabasePath = false;
                System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
                openDlg.Filter = "Text (*.txt)|*.txt";

                // Show open file dialog box 
                System.Windows.Forms.DialogResult result = openDlg.ShowDialog();

                // Process open file dialog box results 
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = openDlg.FileName;
                    string ext = System.IO.Path.GetExtension(openDlg.FileName);
                    string check = ext.Substring(1, ext.Length - 1);

                    if (extensionTxt.Equals(check))
                    {
                        _okDatabasePath = true;
                    }
                    else
                    {
                        _okDatabasePath = false;
                    }

                    if (_okDatabasePath == false)
                    {
                        filePath = String.Empty;
                        System.Windows.Forms.MessageBox.Show("Izabrani fajl " + filePath + " nije tekstualni fajl! Molimo vas učitajte fajl sa ispravnom ekstenzijom!", "POKUŠAJ UČITAVANJA NEISPRAVNOG FORMATA TEKSTUALNOG FAJLA");
                        return;
                    }
                }

                tfFilepathPlotting.Text = filePath;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void btnChooseDatabasePath_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void saveOptionstfFilepathPlotting_Offline()
        {
            try
            {
                OptionsInPlottingMode.filePath = tfFilepathPlotting.Text;


                writeXMLFileOffline();
                createOfflineGraphics();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstfFilepathPlotting_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfFilepathPlotting_Offline()
        {
            try
            {
                tfFilepathPlotting.Foreground = Brushes.Black;
                //tfFilepathPlotting.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfFilepathPlotting_Offline()}", System.DateTime.Now);
            }
        }

        private void tfFilepathPlotting_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfFilepathPlotting.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfFilepathPlotting_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        public void tfFilepathPlottingKeyDown() 
        {
            try
            {
                isGraphicPlottingLoaded = false;
                disableFittingPart();
                saveOptionstfFilepathPlotting_Offline();
                markSavedOnlineOptionsAsBlacktfFilepathPlotting_Offline();
                deleteFittingPath();

                tfReL.Text = String.Empty;
                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfReL.Text = String.Empty;
                }
                ReL = -1;
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                tfReH.Text = String.Empty;
                ReH = -1;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;
                tfRm.Text = String.Empty;
                Rm = -1;
                tfFm.Text = String.Empty;
                Fm = -1;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void tfFilepathPlottingKeyDown()}", System.DateTime.Now);
            }
        }

        public void SimulateTabPressFortfFilepathPlotting() 
        {
            try
            {
                isGraphicPlottingLoaded = false;
                disableFittingPart();
                saveOptionstfFilepathPlotting_Offline();
                markSavedOnlineOptionsAsBlacktfFilepathPlotting_Offline();
                deleteFittingPath();

                tfReL.Text = String.Empty;
                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfReL.Text = String.Empty;
                }
                ReL = -1;
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                tfReH.Text = String.Empty;
                ReH = -1;
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;
                tfRm.Text = String.Empty;
                Rm = -1;
                tfFm.Text = String.Empty;
                Fm = -1;

                btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                //set input for loaded file
                setInputForLoadedFile(OptionsInPlottingMode.filePath);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SimulateTabPressFortfFilepathPlotting()}", System.DateTime.Now);
            }
        }

        private void tfFilepathPlotting_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    isGraphicPlottingLoaded = false;
                    disableFittingPart();
                    saveOptionstfFilepathPlotting_Offline();
                    markSavedOnlineOptionsAsBlacktfFilepathPlotting_Offline();
                    deleteFittingPath();

                    tfReL.Text = String.Empty;
                    ReL = -1;
                    _MarkerGraph5.DataSource = null;
                    //_MarkerGraphText5.DataSource = null;
                    tfReH.Text = String.Empty;
                    ReH = -1;
                    _MarkerGraph6.DataSource = null;
                    //_MarkerGraphText6.DataSource = null;
                    tfRm.Text = String.Empty;
                    Rm = -1;
                    tfFm.Text = String.Empty;
                    Fm = -1;

                    btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                    //set input for loaded file
                    setInputForLoadedFile(OptionsInPlottingMode.filePath);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfFilepathPlotting_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void setInputForLoadedFile(string filePathTxt) 
        {
            try
            {
                if (onMode.OnHeader == null)
                {
                    return;
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                if (this.Printscreen.IsPrintScreenEmpty == true && window.tab_second.IsSelected == true)
                {
                    return;
                }

                //GetAutomaticAnimation file name
                string nameInputOutput = filePathTxt.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";
                List<string> dataListInput = new List<string>();
                dataListInput = File.ReadAllLines(nameInputOutput).ToList();

                for (int i = 0; i < dataListInput.Count; i++)
                {
                    List<string> currString = dataListInput[i].Split('\t').ToList();
                    if (currString.Count < 2) continue;

                    #region GeneralData

                    if (Constants.ONLINEFILEHEADER_OPERATOR.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfOperator_GeneralData = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_BRZBIZVESTAJA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_BRUZORKA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfBrUzorka_GeneralData = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_SARZA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfSarza_GeneralData = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_RADNINALOG.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfRadniNalog_GeneralData = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_NARUCILAC.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfNarucilac_GeneralData = currString[1];
                    }

                    #endregion

                    #region ConditionsOfTesting

                    if (Constants.ONLINEFILEHEADER_STANDARD.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfStandard_ConditionsOfTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_METODA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfMetoda_ConditionsOfTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_STANDARDZAN.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_MASINA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfMasina_ConditionsOfTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_OPSEGMASINE.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_TEMPERATURA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_EKSTENZIOMETAR.Equals(currString[0]))
                    {
                        if (Constants.DA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnYes_ConditionsOfTesting = "True";
                            LastInputOutputSavedData.rbtnNo_ConditionsOfTesting = "False";
                        }
                        if (Constants.NE.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnNo_ConditionsOfTesting = "True";
                            LastInputOutputSavedData.rbtnYes_ConditionsOfTesting = "False";
                        }
                    }

                    #endregion

                    #region MaterialForTesting

                    if (Constants.ONLINEFILEHEADER_PROIZVODJAC.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfProizvodjac_MaterialForTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_DOBAVLJAC.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfDobavljac_MaterialForTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_POLAZNIKVALITET.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_NAZIVNADEBLJINA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_NACINPRERADE.Equals(currString[0]))
                    {
                        if (Constants.VALJANI.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnValjani_MaterialForTesting = "True";
                            LastInputOutputSavedData.rbtnVuceni_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnKovani_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnLiveni_MaterialForTesting = "False";
                        }
                        if (Constants.VUČENI.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnValjani_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnVuceni_MaterialForTesting = "True";
                            LastInputOutputSavedData.rbtnKovani_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnLiveni_MaterialForTesting = "False";
                        }
                        if (Constants.KOVANI.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnValjani_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnVuceni_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnKovani_MaterialForTesting = "True";
                            LastInputOutputSavedData.rbtnLiveni_MaterialForTesting = "False";
                        }
                        if (Constants.LIVENI.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnValjani_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnVuceni_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnKovani_MaterialForTesting = "False";
                            LastInputOutputSavedData.rbtnLiveni_MaterialForTesting = "True";
                        }
                    }

                    #endregion

                    #region Epruveta_OnlineHeader

                    if (Constants.ONLINEFILEHEADER_EPRUVETAOBLIK.Equals(currString[0]))
                    {
                        if (Constants.OBRADJENA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvOblikObradjena = "True";
                            LastInputOutputSavedData.rbtnEpvOblikNeobradjena = "False";
                        }
                        if (Constants.NEOBRADJENA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvOblikNeobradjena = "True";
                            LastInputOutputSavedData.rbtnEpvOblikObradjena = "False";
                        }
                    }

                    if (Constants.ONLINEFILEHEADER_TIP.Equals(currString[0]))
                    {
                        if (Constants.PROPORCIONALNA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvTipProporcionalna = "True";
                            LastInputOutputSavedData.rbtnEpvTipNeproporcionalna = "False";
                        }
                        if (Constants.NEPROPORCIONALNA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvTipNeproporcionalna = "True";
                            LastInputOutputSavedData.rbtnEpvTipProporcionalna = "False";
                        }
                    }

                    if (Constants.ONLINEFILEHEADER_K.Equals(currString[0]))
                    {
                        if (Constants.K1.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvK1 = "True";
                            LastInputOutputSavedData.rbtnEpvK2 = "False";
                        }
                        if (Constants.K2.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvK2 = "True";
                            LastInputOutputSavedData.rbtnEpvK1 = "False";
                        }
                    }

                    if (Constants.ONLINEFILEHEADER_VRSTAEPRUVETE.Equals(currString[0]))
                    {
                        if (Constants.PRAVOUGAONA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "True";
                            LastInputOutputSavedData.rbtnEpvVrstaKruzna = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaCevasta = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaDeocev = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "False";
                        }
                        if (Constants.KRUZNA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaKruzna = "True";
                            LastInputOutputSavedData.rbtnEpvVrstaCevasta = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaDeocev = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "False";
                        }
                        if (Constants.CEVASTA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaKruzna = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaCevasta = "True";
                            LastInputOutputSavedData.rbtnEpvVrstaDeocev = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "False";
                        }
                        if (Constants.DEOCEVI.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaKruzna = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaCevasta = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaDeocev = "True";
                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "False";
                        }
                        if (Constants.SESTAUGAONA.Equals(currString[1]))
                        {
                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaKruzna = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaCevasta = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaDeocev = "False";
                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "True";
                        }
                    }

                    //citanje pocetnih dimenzija a0,b0,d0,D0
                    if (Constants.a0.Equals(currString[0]))
                    {
                        if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                        {
                            LastInputOutputSavedData.a0Pravougaona = currString[1];
                        }
                        if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                        {
                            LastInputOutputSavedData.a0Cevasta = currString[1];
                        }
                        if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                        {
                            LastInputOutputSavedData.a0Deocev = currString[1];
                        }
                    }

                    if (Constants.b0.Equals(currString[0]))
                    {
                        if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                        {
                            LastInputOutputSavedData.b0Pravougaona = currString[1];
                        }
                        if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                        {
                            LastInputOutputSavedData.b0Deocev = currString[1];
                        }
                    }

                    if (Constants.d0.Equals(currString[0]))
                    {
                        if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
                        {
                            LastInputOutputSavedData.d0Sestaugaona = currString[1];
                        }
                    }

                    if (Constants.D0.Equals(currString[0]))
                    {
                        if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                        {
                            LastInputOutputSavedData.D0Kruzna = currString[1];
                        }
                        if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                        {
                            LastInputOutputSavedData.D0Cevasta = currString[1];
                        }
                        if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                        {
                            LastInputOutputSavedData.D0Deocev = currString[1];
                        }
                    }

                    //citanje pocetnih dimenzija au,bu,du,Du
                    if (Constants.au.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.au = currString[1];
                    }
                    if (Constants.bu.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.bu = currString[1];
                    }
                    if (Constants.du.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.du = currString[1];
                    }
                    if (Constants.Du.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.Du = currString[1];
                    }


                    if (Constants.ONLINEFILEHEADER_S0.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfS0 = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_L0.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfL0 = currString[1];
                    }
                    if (Constants.ONLINEFILEHEADER_LC.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfLc = currString[1];
                    }

                    #endregion

                    #region PositionOfTube

                    if (Constants.ONLINEFILEHEADER_PRAVACVALJANJA.Equals(currString[0]))
                    {
                        string numberOfDegreeCustomPravacValjanja = String.Empty;
                        if (currString[1].Length >= 2)
                        {
                            numberOfDegreeCustomPravacValjanja = currString[1].ElementAt(0).ToString() + currString[1].ElementAt(1).ToString();
                            numberOfDegreeCustomPravacValjanja = numberOfDegreeCustomPravacValjanja.Trim();
                        }
                        LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube = numberOfDegreeCustomPravacValjanja;
                    }

                    if (Constants.ONLINEFILEHEADER_SIRINATRAKE.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube = currString[1];
                    }


                    if (Constants.ONLINEFILEHEADER_DUZINATRAKE.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube = currString[1];
                    }


                    #endregion

                    #region Remarks

                    if (Constants.ONLINEFILEHEADER_NAPOMENA.Equals(currString[0]))
                    {
                        LastInputOutputSavedData.rtfNapomena_RemarkOfTesting = currString[1];
                    }

                    #endregion

                }// end for loop


                //write in xml file

                XElement xmlRoot = new XElement("OnlineHeader",
                                                new XElement("OnlineHeaderLastWritten",
                    // GeneralData
                                                            new XElement("tfOperator_GeneralData", LastInputOutputSavedData.tfOperator_GeneralData),
                                                            new XElement("tfBrZbIzvestaja_GeneralData", LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData),
                                                            new XElement("tfBrUzorka_GeneralData", LastInputOutputSavedData.tfBrUzorka_GeneralData),
                                                            new XElement("tfSarza_GeneralData", LastInputOutputSavedData.tfSarza_GeneralData),
                                                            new XElement("tfRadniNalog_GeneralData", LastInputOutputSavedData.tfRadniNalog_GeneralData),
                                                            new XElement("tfNaručilac_GeneralData", LastInputOutputSavedData.tfNarucilac_GeneralData),

                                                            //ConditionsOfTesting
                                                            new XElement("tfStandard_ConditionsOfTesting", LastInputOutputSavedData.tfStandard_ConditionsOfTesting),
                                                            new XElement("tfMetoda_ConditionsOfTesting", LastInputOutputSavedData.tfMetoda_ConditionsOfTesting),
                                                            new XElement("tfStandardZaN_ConditionsOfTesting", LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting),
                                                            new XElement("tfMasina_ConditionsOfTesting", LastInputOutputSavedData.tfMasina_ConditionsOfTesting),
                                                            new XElement("tfBegOpsegMasine_ConditionsOfTesting", LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting),
                                                            new XElement("tfTemperatura_ConditionsOfTesting", LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting),
                                                            LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True") ? new XElement("rbtnYes_ConditionsOfTesting", "True") : new XElement("rbtnYes_ConditionsOfTesting", "False"),
                                                            LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True") ? new XElement("rbtnNo_ConditionsOfTesting", "True") : new XElement("rbtnNo_ConditionsOfTesting", "False"),

                                                            //MaterialForTesting
                                                            new XElement("tfProizvodjac_MaterialForTesting", LastInputOutputSavedData.tfProizvodjac_MaterialForTesting),
                                                            new XElement("tfDobavljac_MaterialForTesting", LastInputOutputSavedData.tfDobavljac_MaterialForTesting),
                                                            new XElement("tfPolazniKvalitet_MaterialForTesting", LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting),
                                                            new XElement("tfNazivnaDebljina_MaterialForTesting", LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting),
                                                            LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True") ? new XElement("rbtnValjani_MaterialForTesting", "True") : new XElement("rbtnValjani_MaterialForTesting", "False"),
                                                            LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True") ? new XElement("rbtnVučeni_MaterialForTesting", "True") : new XElement("rbtnVučeni_MaterialForTesting", "False"),
                                                            LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True") ? new XElement("rbtnKovani_MaterialForTesting", "True") : new XElement("rbtnKovani_MaterialForTesting", "False"),
                                                            LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True") ? new XElement("rbtnLiveni_MaterialForTesting", "True") : new XElement("rbtnLiveni_MaterialForTesting", "False"),

                                                            //Epruveta_OnlineHeader
                                                            LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True") ? new XElement("rbtnEpvOblikObradjena", "True") : new XElement("rbtnEpvOblikObradjena", "False"),
                                                            LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True") ? new XElement("rbtnEpvOblikNeobradjena", "True") : new XElement("rbtnEpvOblikNeobradjena", "False"),
                                                            LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True") ? new XElement("rbtnEpvTipProporcionalna", "True") : new XElement("rbtnEpvTipProporcionalna", "False"),
                                                            LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True") ? new XElement("rbtnEpvTipNeproporcionalna", "True") : new XElement("rbtnEpvTipNeproporcionalna", "False"),
                                                            LastInputOutputSavedData.rbtnEpvK1.Equals("True") ? new XElement("rbtnEpvK1", "True") : new XElement("rbtnEpvK1", "False"),
                                                            LastInputOutputSavedData.rbtnEpvK2.Equals("True") ? new XElement("rbtnEpvK2", "True") : new XElement("rbtnEpvK2", "False"),
                                                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement("rbtnEpvVrstaPravougaona", "True") : new XElement("rbtnEpvVrstaPravougaona", "False"),
                                                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement("a", LastInputOutputSavedData.a0Pravougaona) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement("b", LastInputOutputSavedData.b0Pravougaona) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement("rbtnEpvVrstaKruzni", "True") : new XElement("rbtnEpvVrstaKruzni", "False"),
                                                            LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement("D", LastInputOutputSavedData.D0Kruzna) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement("rbtnEpvVrstaCevasti", "True") : new XElement("rbtnEpvVrstaCevasti", "False"),
                                                            LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement("D", LastInputOutputSavedData.D0Cevasta) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement("a", LastInputOutputSavedData.a0Cevasta) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement("rbtnEpvVrstaDeocev", "True") : new XElement("rbtnEpvVrstaDeocev", "False"),
                                                            LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement("D", LastInputOutputSavedData.D0Deocev) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement("a", LastInputOutputSavedData.a0Deocev) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement("b", LastInputOutputSavedData.b0Deocev) : null,
                                                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True") ? new XElement("rbtnEpvVrstaSestaugaona", "True") : new XElement("rbtnEpvVrstaSestaugaona", "False"),
                                                            LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True") ? new XElement("d", LastInputOutputSavedData.d0Sestaugaona) : null,
                                                            new XElement("tfS0", LastInputOutputSavedData.tfS0),
                                                            new XElement("tfL0", LastInputOutputSavedData.tfL0),
                                                            new XElement("tfLc", LastInputOutputSavedData.tfLc),

                                                            // PositionOfTube
                                                            new XElement("tfCustomPravacValjanja_PositionOfTube", LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube),
                                                            new XElement("tfCustomSirinaTrake_PositionOfTube", LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube),
                                                            new XElement("tfCustomDuzinaTrake_PositionOfTube", LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube),

                                                            // Remarks
                                                            (LastInputOutputSavedData.rtfNapomena_RemarkOfTesting.Length <= Constants.MAXREMARKSTESTINGLENGTH) ? new XElement("rtfNapomena_RemarkOfTesting", LastInputOutputSavedData.rtfNapomena_RemarkOfTesting) : new XElement("rtfNapomena_RemarkOfTesting", String.Empty)

                                                            )
                                                );

                xmlRoot.Save(Constants.lastOnlineHeaderXml);


                //using (XmlWriter writer = XmlWriter.Create(Constants.lastOnlineHeaderXml))
                //{
                //    writer.WriteStartDocument();
                //    writer.WriteStartElement("OnlineHeader");


                //    writer.WriteStartElement("OnlineHeaderLastWritten");

                #region GeneralData

                //    //writer.WriteElementString("tfbrIzvestaja_GeneralData", onHeader.GeneralData.tfbrIzvestaja.Text);
                //    writer.WriteElementString("tfOperator_GeneralData", LastInputOutputSavedData.tfOperator_GeneralData);
                //    writer.WriteElementString("tfBrZbIzvestaja_GeneralData", LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData);
                //    writer.WriteElementString("tfBrUzorka_GeneralData", LastInputOutputSavedData.tfBrUzorka_GeneralData);
                //    writer.WriteElementString("tfSarza_GeneralData", LastInputOutputSavedData.tfSarza_GeneralData);
                //    writer.WriteElementString("tfRadniNalog_GeneralData", LastInputOutputSavedData.tfRadniNalog_GeneralData);
                //    writer.WriteElementString("tfNaručilac_GeneralData", LastInputOutputSavedData.tfNarucilac_GeneralData);

                //refresh input windows if windows are opened
                if (onMode.OnHeader != null)
                {
                    onMode.OnHeader.GeneralData.tfOperator.Text = LastInputOutputSavedData.tfOperator_GeneralData;
                    onMode.OnHeader.GeneralData.tfBrZbIzvestaja.Text = LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData;
                    List<string> brUzorka = LastInputOutputSavedData.tfBrUzorka_GeneralData.Split('/').ToList();
                    if (brUzorka.Count == 2)
                    {
                        onMode.OnHeader.GeneralData.tfBrUzorka.Text = brUzorka.ElementAt(0);
                        onMode.OnHeader.GeneralData.tfBrUzorkaNumberOfSample.Text = brUzorka.ElementAt(1);
                    }
                    onMode.OnHeader.GeneralData.tfSarza.Text = LastInputOutputSavedData.tfSarza_GeneralData;
                    onMode.OnHeader.GeneralData.tfRadniNalog.Text = LastInputOutputSavedData.tfRadniNalog_GeneralData;
                    onMode.OnHeader.GeneralData.tfNaručilac.Text = LastInputOutputSavedData.tfNarucilac_GeneralData;
                }


                #endregion

                #region ConditionsOfTesting

                //    writer.WriteElementString("tfStandard_ConditionsOfTesting", LastInputOutputSavedData.tfStandard_ConditionsOfTesting);
                //    writer.WriteElementString("tfMetoda_ConditionsOfTesting", LastInputOutputSavedData.tfMetoda_ConditionsOfTesting);
                //    writer.WriteElementString("tfStandardZaN_ConditionsOfTesting", LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting);
                //    writer.WriteElementString("tfMasina_ConditionsOfTesting", LastInputOutputSavedData.tfMasina_ConditionsOfTesting);
                //    writer.WriteElementString("tfBegOpsegMasine_ConditionsOfTesting", LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting);
                //    //writer.WriteElementString("tfEndOpsegMasine_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfEndOpsegMasine.Text);
                //    writer.WriteElementString("tfTemperatura_ConditionsOfTesting", LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting);

                //    if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnYes_ConditionsOfTesting", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnYes_ConditionsOfTesting", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnNo_ConditionsOfTesting", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnNo_ConditionsOfTesting", "False");
                //    }


                //refresh input windows if windows are opened
                if (onMode.OnHeader != null)
                {
                    onMode.OnHeader.ConditionsOfTesting.tfStandard.Text = LastInputOutputSavedData.tfStandard_ConditionsOfTesting;
                    onMode.OnHeader.ConditionsOfTesting.tfMetoda.Text = LastInputOutputSavedData.tfMetoda_ConditionsOfTesting;
                    onMode.OnHeader.ConditionsOfTesting.tfStandardZaN.Text = LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting;
                    onMode.OnHeader.ConditionsOfTesting.tfMasina.Text = LastInputOutputSavedData.tfMasina_ConditionsOfTesting;
                    onMode.OnHeader.ConditionsOfTesting.tfBegOpsegMasine.Text = LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting;
                    onMode.OnHeader.ConditionsOfTesting.tfTemperatura.Text = LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting;

                    if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                    {
                        onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked = false;
                    }
                    if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
                    {
                        onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked = false;
                    }
                }

                #endregion

                #region MaterialForTesting

                //    writer.WriteElementString("tfProizvodjac_MaterialForTesting", LastInputOutputSavedData.tfProizvodjac_MaterialForTesting);
                //    writer.WriteElementString("tfDobavljac_MaterialForTesting", LastInputOutputSavedData.tfDobavljac_MaterialForTesting);
                //    writer.WriteElementString("tfPolazniKvalitet_MaterialForTesting", LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting);
                //    writer.WriteElementString("tfNazivnaDebljina_MaterialForTesting", LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting);

                //    if (LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnValjani_MaterialForTesting", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnValjani_MaterialForTesting", "False");
                //    }


                //    if (LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnVučeni_MaterialForTesting", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnVučeni_MaterialForTesting", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnKovani_MaterialForTesting", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnKovani_MaterialForTesting", "False");
                //    }


                //    if (LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnLiveni_MaterialForTesting", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnLiveni_MaterialForTesting", "False");
                //    }



                //refresh input windows if windows are opened
                if (onMode.OnHeader != null)
                {
                    onMode.OnHeader.MaterialForTesting.tfProizvodjac.Text = LastInputOutputSavedData.tfProizvodjac_MaterialForTesting;
                    onMode.OnHeader.MaterialForTesting.tfDobavljac.Text = LastInputOutputSavedData.tfDobavljac_MaterialForTesting;
                    onMode.OnHeader.MaterialForTesting.tfPolazniKvalitet.Text = LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting;
                    onMode.OnHeader.MaterialForTesting.tfNazivnaDebljina.Text = LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting;

                    if (LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True"))
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnValjani.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnValjani.IsChecked = false;
                    }


                    if (LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True"))
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnVučeni.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnVučeni.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True"))
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnKovani.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnKovani.IsChecked = false;
                    }


                    if (LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True"))
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnLiveni.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.MaterialForTesting.rbtnLiveni.IsChecked = false;
                    }

                }

                #endregion


                #region Epruveta_OnlineHeader



                //    if (LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvOblikObradjena", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvOblikObradjena", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvOblikNeobradjena", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvOblikNeobradjena", "False");
                //    }



                //    if (LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvTipProporcionalna", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvTipProporcionalna", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvTipNeproporcionalna", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvTipNeproporcionalna", "False");
                //    }


                //    if (LastInputOutputSavedData.rbtnEpvK1.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvK1", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvK1", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnEpvK2.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvK2", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvK2", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaPravougaona", "True");
                //        writer.WriteElementString("a", LastInputOutputSavedData.a0Pravougaona);
                //        writer.WriteElementString("b", LastInputOutputSavedData.b0Pravougaona);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaPravougaona", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaKruzni", "True");
                //        writer.WriteElementString("D", LastInputOutputSavedData.D0Kruzna);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaKruzni", "False");
                //    }

                //    if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaCevasti", "True");
                //        writer.WriteElementString("D", LastInputOutputSavedData.D0Cevasta);
                //        writer.WriteElementString("a", LastInputOutputSavedData.a0Cevasta);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaCevasti", "False");
                //    }


                //    if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaDeocev", "True");
                //        writer.WriteElementString("D", LastInputOutputSavedData.D0Deocev);
                //        writer.WriteElementString("a", LastInputOutputSavedData.a0Deocev);
                //        writer.WriteElementString("b", LastInputOutputSavedData.b0Deocev);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaDeocev", "False");
                //    }


                //    if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaSestaugaona", "True");
                //        writer.WriteElementString("d", LastInputOutputSavedData.d0Sestaugaona);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnEpvVrstaSestaugaona", "False");
                //    }

                //    writer.WriteElementString("tfS0", LastInputOutputSavedData.tfS0);
                //    writer.WriteElementString("tfL0", LastInputOutputSavedData.tfL0);
                //    writer.WriteElementString("tfLc", LastInputOutputSavedData.tfLc);


                //refresh input windows if windows are opened
                if (onMode.OnHeader != null)
                {
                    if (LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvOblikObradjena.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvOblikObradjena.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvOblikNeobradjena.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvOblikNeobradjena.IsChecked = false;
                    }



                    if (LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvTipProporcionalna.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvTipProporcionalna.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvTipNeproporcionalna.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvTipNeproporcionalna.IsChecked = false;
                    }


                    if (LastInputOutputSavedData.rbtnEpvK1.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvK1.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvK1.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnEpvK2.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvK2.IsChecked = true;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvK2.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked = true;
                        onMode.OnHeader.tfAGlobal.Text = LastInputOutputSavedData.a0Pravougaona;
                        onMode.OnHeader.tfBGlobal.Text = LastInputOutputSavedData.b0Pravougaona;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked = true;
                        onMode.OnHeader.tfDGlobal.Text = LastInputOutputSavedData.D0Kruzna;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked = false;
                    }

                    if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvVrstaCevasti.IsChecked = true;
                        onMode.OnHeader.tfDGlobal.Text = LastInputOutputSavedData.D0Cevasta;
                        onMode.OnHeader.tfAGlobal.Text = LastInputOutputSavedData.a0Cevasta;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvVrstaCevasti.IsChecked = false;
                    }


                    if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvVrstaDeocev.IsChecked = true;
                        onMode.OnHeader.tfDGlobal.Text = LastInputOutputSavedData.D0Deocev;
                        onMode.OnHeader.tfAGlobal.Text = LastInputOutputSavedData.a0Deocev;
                        onMode.OnHeader.tfBGlobal.Text = LastInputOutputSavedData.b0Deocev;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvVrstaDeocev.IsChecked = false;
                    }


                    if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
                    {
                        onMode.OnHeader.rbtnEpvVrstaSestaugaona.IsChecked = true;
                        onMode.OnHeader.tfDGlobal.Text = LastInputOutputSavedData.d0Sestaugaona;
                    }
                    else
                    {
                        onMode.OnHeader.rbtnEpvVrstaSestaugaona.IsChecked = false;
                    }

                    onMode.OnHeader.tfS0.Text = LastInputOutputSavedData.tfS0;
                    onMode.OnHeader.tfL0.Text = LastInputOutputSavedData.tfL0;
                    onMode.OnHeader.tfLc.Text = LastInputOutputSavedData.tfLc;

                }

                #endregion


                #region PositionOfTube


                //    writer.WriteElementString("tfCustomPravacValjanja_PositionOfTube", LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube);
                //    writer.WriteElementString("tfCustomSirinaTrake_PositionOfTube", LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube);
                //    writer.WriteElementString("tfCustomDuzinaTrake_PositionOfTube", LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube);

                //refresh input windows if windows are opened
                if (onMode.OnHeader != null)
                {
                    onMode.OnHeader.PositionOfTube.tfCustomPravacValjanja.Text = LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube;
                    onMode.OnHeader.PositionOfTube.tfCustomSirinaTrake.Text = LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube;
                    onMode.OnHeader.PositionOfTube.tfCustomDuzinaTrake.Text = LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube;
                }

                #endregion

                #region Remarks

                //    //TextRange textRange = new TextRange(onHeader.RemarkOfTesting.rtfNapomena.Document.ContentStart, onHeader.RemarkOfTesting.rtfNapomena.Document.ContentEnd);

                //    //string textRangeStr = textRange.Text;
                //    //textRangeStr = textRangeStr.Replace("\r\n", String.Empty);
                if (LastInputOutputSavedData.rtfNapomena_RemarkOfTesting.Length <= Constants.MAXREMARKSTESTINGLENGTH)
                {
                    //writer.WriteElementString("rtfNapomena_RemarkOfTesting", LastInputOutputSavedData.rtfNapomena_RemarkOfTesting);
                }
                else
                {
                    MessageBox.Show("Dužina napomene može biti najviše " + Constants.MAXREMARKSTESTINGLENGTH + " znakova.");
                }

                //refresh input windows if windows are opened
                // create a paragraph with text
                // create a flowdocument
                FlowDocument mcflowdoc = new FlowDocument();
                Paragraph para = new Paragraph();
                para.Inlines.Add(new Run(LastInputOutputSavedData.rtfNapomena_RemarkOfTesting));

                // add the paragraph to blocks of paragraph
                mcflowdoc.Blocks.Add(para);

                // set contents
                onMode.OnHeader.RemarkOfTesting.rtfNapomena.Document = mcflowdoc;

                #endregion


                //    writer.WriteEndElement();


                //    writer.WriteEndElement();
                //    writer.WriteEndDocument();
                //    writer.Close();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setInputForLoadedFile(string filePathTxt)}", System.DateTime.Now);
            }

        }

        #endregion

        #region fittingCheckboxes

        private void chbFittingMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.isFittingChecked = true;



                if (OptionsInPlottingMode.isAutoFittingChecked)
                {
                    chbFititngManualMode.IsEnabled = true;
                    tffittingCrossheadPointX.IsReadOnly = false;
                    tffittingCrossheadPointY.IsReadOnly = false;
                    //tffittingAutoProcent1.IsReadOnly = true;
                    //tffittingAutoPointY1.IsReadOnly = true;
                    //tffittingAutoProcent2.IsReadOnly = true;
                    //tffittingAutoPointY2.IsReadOnly = true;
                    //tffittingAutoProcent3.IsReadOnly = true;
                    //tffittingAutoPointY3.IsReadOnly = true;

                    tffittingManPoint1X.IsReadOnly = false;
                    tffittingManPoint1Y.IsReadOnly = false;
                    tffittingManPoint2X.IsReadOnly = false;
                    tffittingManPoint2Y.IsReadOnly = false;
                    tffittingManPoint3X.IsReadOnly = false;
                    tffittingManPoint3Y.IsReadOnly = false;
                }
                if (OptionsInPlottingMode.isManualFittingChecked)
                {
                    chbFititngManualMode.IsEnabled = true;
                    tffittingManPoint1X.IsReadOnly = true;
                    tffittingManPoint1Y.IsReadOnly = true;
                    tffittingManPoint2X.IsReadOnly = true;
                    tffittingManPoint2Y.IsReadOnly = true;
                    tffittingManPoint3X.IsReadOnly = true;
                    tffittingManPoint3Y.IsReadOnly = true;

                    tffittingCrossheadPointX.IsReadOnly = false;
                    tffittingCrossheadPointY.IsReadOnly = false;
                    //tffittingAutoProcent1.IsReadOnly = false;
                    //tffittingAutoPointY1.IsReadOnly = false;
                    //tffittingAutoProcent2.IsReadOnly = false;
                    //tffittingAutoPointY2.IsReadOnly = false;
                    //tffittingAutoProcent3.IsReadOnly = false;
                    //tffittingAutoPointY3.IsReadOnly = false;
                }
                optionsPlotting.chbShowOriginalData.IsEnabled = true;
                //btnTurnOnFittingLine.IsReadOnly = true;
                btnTurnOnFitting.IsEnabled = true;
                optionsPlotting.tfYield.IsReadOnly = true;
                tfReL.IsReadOnly = true;
                tfReH.IsReadOnly = true;
                tfRp02.IsReadOnly = true;
                //chbRp02Visibility.IsEnabled = true;
                //chbRp02Visibility.IsChecked = true;
                tfA.IsReadOnly = true;
                chbAVisibility.IsEnabled = true;
                chbAVisibility.IsChecked = true;
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbFittingMode_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbFittingMode_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.isFittingChecked = false;
                chbFititngManualMode.IsEnabled = false;


                tffittingCrossheadPointX.IsReadOnly = false;
                tffittingCrossheadPointY.IsReadOnly = false;
                tffittingManPoint1X.IsReadOnly = false;
                tffittingManPoint1Y.IsReadOnly = false;
                tffittingManPoint2X.IsReadOnly = false;
                tffittingManPoint2Y.IsReadOnly = false;
                tffittingManPoint3X.IsReadOnly = false;
                tffittingManPoint3Y.IsReadOnly = false;
                optionsPlotting.chbShowOriginalData.IsEnabled = false;
                btnTurnOnFitting.IsEnabled = false;
                //tfReL.IsReadOnly = false;
                //tfReH.IsReadOnly = false;
                tfRp02.IsReadOnly = false;
                //chbRp02Visibility.IsEnabled = false;
                //chbRp02Visibility.IsChecked = true;
                tfA.IsReadOnly = false;
                //chbAVisibility.IsEnabled = false;
                chbAVisibility.IsChecked = true;

                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbFittingMode_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        

        private void chbShowFittingLine_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                writeXMLFileOffline();
                if (isGraphicPlottingLoaded == true)
                {
                    if (OptionsInPlottingMode.isManualFittingChecked)
                    {
                        setPointAtGraphicY1(OptionsInPlottingMode.pointManualY1);
                        setPointAtGraphicY2(OptionsInPlottingMode.pointManualY2);
                        ConnectDiscreteDisplays();
                    }
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        optionsPlotting.setPointAtGraphictffittingAutoProcent1();
                        optionsPlotting.setPointAtGraphictffittingAutoProcent2();
                        ConnectDiscreteDisplays();
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbShowFittingLine_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbShowFittingLine_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                writeXMLFileOffline();

                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbShowFittingLine_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion

        #region chbFititngManualMode

        private void chbFititngManualMode_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.isAutoFittingChecked = false;
                OptionsInPlottingMode.isManualFittingChecked = true;
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbFititngManualMode_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbFititngManualMode_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.isAutoFittingChecked = true;
                OptionsInPlottingMode.isManualFittingChecked = false;
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbFititngManualMode_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }
        #endregion

        #region kindOfFitting


        private void calculateDefaultT1() 
        {
            try
            {
                double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto1 / 100;//OptionsInPlottingMode.pointCrossheadY is Rm

                int index = GetClosestPointIndex(criteriaPreassure);
                OptionsInPlottingMode.pointAutoX1 = DataReader.RelativeElongation[index];
                OptionsInPlottingMode.pointAutoY1 = DataReader.PreassureInMPa[index];
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateDefaultT1()}", System.DateTime.Now);
            }
        }

        private void calculateDefaultT2()
        {
            try
            {
                double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto2 / 100;//OptionsInPlottingMode.pointCrossheadY is Rm

                int index = GetClosestPointIndex(criteriaPreassure);
                OptionsInPlottingMode.pointAutoX2 = DataReader.RelativeElongation[index];
                OptionsInPlottingMode.pointAutoY2 = DataReader.PreassureInMPa[index];
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateDefaultT2()}", System.DateTime.Now);
            }
        }

        private void calculateDefaultT3()
        {
            try
            {
                double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto3 / 100;//OptionsInPlottingMode.pointCrossheadY is Rm

                int index = GetClosestPointIndex(criteriaPreassure);
                OptionsInPlottingMode.pointAutoX3 = DataReader.RelativeElongation[index];
                OptionsInPlottingMode.pointAutoY3 = DataReader.PreassureInMPa[index];
                OptionsInPlottingMode.pointAutoX3 = Math.Round(OptionsInPlottingMode.pointAutoX3, 6);
                OptionsInPlottingMode.pointAutoY3 = Math.Round(OptionsInPlottingMode.pointAutoY3, 0);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateDefaultT3()}", System.DateTime.Now);
            }
        }

        public void SetManualPointsToAutoPointsValue()
        {
            try
            {
                setManualPointsToAutoPointsValue();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetManualPointsToAutoPointsValue()}", System.DateTime.Now);
            }
        }

        private void setManualPointsToAutoPointsValue() 
        {
            try
            {
                calculateDefaultT1();
                calculateDefaultT2();
                calculateDefaultT3();

                OptionsInPlottingMode.pointManualX1 = OptionsInPlottingMode.pointAutoX1;
                OptionsInPlottingMode.pointManualY1 = OptionsInPlottingMode.pointAutoY1;
                OptionsInPlottingMode.pointManualX2 = OptionsInPlottingMode.pointAutoX2;
                OptionsInPlottingMode.pointManualY2 = OptionsInPlottingMode.pointAutoY2;
                OptionsInPlottingMode.pointManualX3 = OptionsInPlottingMode.pointAutoX3;
                OptionsInPlottingMode.pointManualY3 = OptionsInPlottingMode.pointAutoY3;

                OptionsInPlottingMode.pointManualX1 = Math.Round(OptionsInPlottingMode.pointManualX1, 6);
                OptionsInPlottingMode.pointManualY1 = Math.Round(OptionsInPlottingMode.pointManualY1, 0);
                OptionsInPlottingMode.pointManualX2 = Math.Round(OptionsInPlottingMode.pointManualX2, 6);
                OptionsInPlottingMode.pointManualY2 = Math.Round(OptionsInPlottingMode.pointManualY2, 0);
                OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);

                tffittingManPoint1X.Text = OptionsInPlottingMode.pointManualX1.ToString();
                tffittingManPoint1Y.Text = OptionsInPlottingMode.pointManualY1.ToString();
                tffittingManPoint2X.Text = OptionsInPlottingMode.pointManualX2.ToString();
                tffittingManPoint2Y.Text = OptionsInPlottingMode.pointManualY2.ToString();
                tffittingManPoint3X.Text = OptionsInPlottingMode.pointManualX3.ToString();
                tffittingManPoint3Y.Text = OptionsInPlottingMode.pointManualY3.ToString();


                tffittingManPoint1X.Foreground = Brushes.Black;
                tffittingManPoint1Y.Foreground = Brushes.Black;
                tffittingManPoint2X.Foreground = Brushes.Black;
                tffittingManPoint2Y.Foreground = Brushes.Black;
                tffittingManPoint3X.Foreground = Brushes.Black;
                tffittingManPoint3Y.Foreground = Brushes.Black;

                setPointAtGraphicX1_withXY(OptionsInPlottingMode.pointManualX1, OptionsInPlottingMode.pointManualY1);
                setPointAtGraphicX2_withXY(OptionsInPlottingMode.pointManualX2, OptionsInPlottingMode.pointManualY2);
                setPointAtGraphicX3_withXY(OptionsInPlottingMode.pointManualX3, OptionsInPlottingMode.pointManualY3);

                //tffittingManPoint1X.Foreground = Brushes.White;
                //tffittingManPoint1Y.Foreground = Brushes.White;
                //tffittingManPoint2X.Foreground = Brushes.White;
                //tffittingManPoint2Y.Foreground = Brushes.White;
                //tffittingManPoint3X.Foreground = Brushes.White;
                //tffittingManPoint3Y.Foreground = Brushes.White;
            }
            catch(Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setManualPointsToAutoPointsValue()}", System.DateTime.Now);
            }
        }

       

        #endregion

        #region manualFitting

        public void CalculateCurrKandN()
        {
            try
            {
                calculateCurrKandN();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void CalculateCurrKandN()}", System.DateTime.Now);
            }
        }

        private void calculateCurrKandN_AfterLuChanged(double x1, double y1, double x2, double y2)
        {
            try
            {
                double xSubstraction = x2 - x1;
                //if ((xSubstraction > -0.15 && xSubstraction < 0.15) || xSubstraction == -0.15 || xSubstraction == 0.15)
                //{
                //    writeXMLFileOffline();
                //}
                //else
                //{
                if (xSubstraction < 0.01)
                {
                    currK = double.PositiveInfinity;
                    currN = double.PositiveInfinity;
                }
                else
                {
                    double k = (y2 - y1) / (x2 - x1);
                    currK = k;
                    currK = Math.Round(currK, 6);
                    double n = y2 - k * x2;
                    currN = n;
                    currN = Math.Round(currN, 6);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateCurrKandN_AfterLuChanged(double x1, double y1, double x2, double y2)}", System.DateTime.Now);
            }
        }

        private void calculateCurrKandN()
        {
            try
            {
                double xSubstraction = xMarkers2[0] - xMarkers[0];
                //if ((xSubstraction > -0.15 && xSubstraction < 0.15) || xSubstraction == -0.15 || xSubstraction == 0.15)
                //{
                //    writeXMLFileOffline();
                //}
                //else
                //{
                if (xSubstraction < 0.01)
                {
                    currK = double.PositiveInfinity;
                    currN = double.PositiveInfinity;
                }
                else
                {
                    double k = (yMarkers2[0] - yMarkers[0]) / (xMarkers2[0] - xMarkers[0]);
                    currK = k;
                    currK = Math.Round(currK, 6);
                    double n = yMarkers2[0] - k * xMarkers2[0];
                    currN = n;
                    currN = Math.Round(currN, 6);
                    writeXMLFileOffline();
                }
                // }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateCurrKandN()}", System.DateTime.Now);
            }
        }

        #region fittingManPoint1X

        private void saveOptionstffittingManPoint1X_Offline(out double pointManualFittingX1)
        {
            try
            {
                double pointManualX1;
                string strpointManualX1 = tffittingManPoint1X.Text.Replace(',', '.');
                bool isN = Double.TryParse(strpointManualX1, out pointManualX1);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje x koordinate prve tačke ručnog fitovanja!");
                }
                else
                {
                    OptionsInPlottingMode.pointManualX1 = pointManualX1;
                }

                writeXMLFileOffline();

                pointManualFittingX1 = pointManualX1;
            }
            catch (Exception ex)
            {
                pointManualFittingX1 = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstffittingManPoint1X_Offline(out double pointManualFittingX1)}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingManPoint1X_Offline()
        {
            try
            {
                tffittingManPoint1X.Foreground = Brushes.Black;
                //tffittingManPoint1X.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingManPoint1X_Offline()}", System.DateTime.Now);
            }
        }

        public void tffittingManPoint1X_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingManPoint1X.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void tffittingManPoint1X_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint1X_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    double pfX1 = 0;
                    saveOptionstffittingManPoint1X_Offline(out pfX1);
                    markSavedOnlineOptionsAsBlacktffittingManPoint1X_Offline();
                    setPointAtGraphicX1(pfX1);

                    //calculateCurrKandN();
                    drawFittingGraphic();

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint1X_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        public void SetPointAtGraphicX1_withXY(double pfX1, double pfY1)
        {
            try
            {
                setPointAtGraphicX1_withXY(pfX1, pfY1);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetPointAtGraphicX1_withXY(double pfX1, double pfY1)}", System.DateTime.Now);
            }
        }

        private void setPointAtGraphicX1_withXY(double pfX1, double pfY1)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers);
                _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX1);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY1);

                xMarkers[0] = pfX1;
                yMarkers[0] = pfY1;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Blue);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph.Marker = mkr;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicX1_withXY(double pfX1, double pfY1)}", System.DateTime.Now);
            }

        }

        private void setPointAtGraphicX1(double pfX1) 
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers);
                _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX1);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY1);

                xMarkers[0] = pfX1;
                yMarkers[0] = OptionsInPlottingMode.pointManualY1;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Blue);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph.Marker = mkr;

                //optionsPlotting.setTextMarkert1();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicX1(double pfX1)}", System.DateTime.Now);
            }
        }

        #endregion


        #region fittingManPoint1Y

        private void saveOptionstffittingManPoint1Y_Offline(out double pointManualFittingY1)
        {
            try
            {
                double pointManualY1;
                string strpointManualY1 = tffittingManPoint1Y.Text.Replace(',', '.');
                bool isN = Double.TryParse(strpointManualY1, out pointManualY1);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje y koordinate prve tačke ručnog fitovanja!");
                }
                else
                {
                    OptionsInPlottingMode.pointManualY1 = pointManualY1;
                }

                writeXMLFileOffline();

                pointManualFittingY1 = pointManualY1;
            }
            catch (Exception ex)
            {
                pointManualFittingY1 = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstffittingManPoint1Y_Offline(out double pointManualFittingY1)}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingManPoint1Y_Offline()
        {
            try
            {
                tffittingManPoint1Y.Foreground = Brushes.Black;
                //tffittingManPoint1Y.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingManPoint1Y_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint1Y_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingManPoint1Y.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint1Y_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint1Y_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    double pfY1 = 0;
                    saveOptionstffittingManPoint1Y_Offline(out pfY1);
                    markSavedOnlineOptionsAsBlacktffittingManPoint1Y_Offline();
                    setPointAtGraphicY1(pfY1);

                    //calculateCurrKandN();
                    drawFittingGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint1Y_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void setMutualPoint(double x, double y)
        {
            try
            {
                // first empty source for graph plotting
                //_MarkerGraph.DataSource = null;
                // then create new graph
                //xMarkers = new List<double>();
                //yMarkers = new List<double>();
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers3);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers3);
                _MarkerGraph3.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Add(OptionsInPlottingMode.pointManualX1);
                //yMarkers.Add(pfY1);

                xMarkers3[0] = x;
                yMarkers3[0] = y;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = 4;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph3.Marker = mkr;

                //optionsPlotting.setTextMarkert1();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setMutualPoint(double x, double y)}", System.DateTime.Now);
            }
        }

        private void setPointAtGraphicY1(double pfY1)
        {
            try
            {
                // first empty source for graph plotting
                //_MarkerGraph.DataSource = null;
                // then create new graph
                //xMarkers = new List<double>();
                //yMarkers = new List<double>();
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers);
                _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Add(OptionsInPlottingMode.pointManualX1);
                //yMarkers.Add(pfY1);

                xMarkers[0] = OptionsInPlottingMode.pointManualX1;
                yMarkers[0] = pfY1;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Blue);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph.Marker = mkr;

                //optionsPlotting.setTextMarkert1();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicY1(double pfY1)}", System.DateTime.Now);
            }
        }

        #endregion

       
       
        #region fittingManPoint2X

        private void saveOptionstffittingManPoint2X_Offline(out double pointManualFittingX2)
        {
            try
            {
                double pointManualX2;
                string strpointManualX2 = tffittingManPoint2X.Text.Replace(',', '.');
                bool isN = Double.TryParse(strpointManualX2, out pointManualX2);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje x koordinate druge tačke ručnog fitovanja!");
                }
                else
                {
                    OptionsInPlottingMode.pointManualX2 = pointManualX2;
                }

                writeXMLFileOffline();

                pointManualFittingX2 = pointManualX2;
            }
            catch (Exception ex)
            {
                pointManualFittingX2 = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstffittingManPoint2X_Offline(out double pointManualFittingX2)}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingManPoint2X_Offline()
        {
            try
            {
                tffittingManPoint2X.Foreground = Brushes.Black;
                //tffittingManPoint2X.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingManPoint2X_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint2X_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingManPoint2X.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint2X_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint2X_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    double pfX2 = 0;
                    saveOptionstffittingManPoint2X_Offline(out pfX2);
                    markSavedOnlineOptionsAsBlacktffittingManPoint2X_Offline();
                    setPointAtGraphicX2(pfX2);

                    //calculateCurrKandN();
                    drawFittingGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint2X_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        public void SetPointAtGraphicX2_withXY(double pfX1, double pfY1)
        {
            try
            {
                setPointAtGraphicX2_withXY(pfX1, pfY1);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetPointAtGraphicX2_withXY(double pfX1, double pfY1)}", System.DateTime.Now);
            }
        }

        private void setPointAtGraphicX2_withXY(double pfX2, double pfY2)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers2);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers2);
                _MarkerGraph2.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX2);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY2);
                xMarkers2[0] = pfX2;
                yMarkers2[0] = pfY2;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Yellow);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph2.Marker = mkr;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicX2_withXY(double pfX2, double pfY2)}", System.DateTime.Now);
            }

        }

        private void setPointAtGraphicX2(double pfX2)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers2);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers2);
                _MarkerGraph2.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX2);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY2);
                xMarkers2[0] = pfX2;
                yMarkers2[0] = OptionsInPlottingMode.pointManualY2;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Yellow);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph2.Marker = mkr;

                //optionsPlotting.setTextMarkert2();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicX2(double pfX2)}", System.DateTime.Now);
            }
        }

        #endregion

        #region fittingManPoint2Y

        private void saveOptionstffittingManPoint2Y_Offline(out double pointManualFittingY2)
        {
            try
            {
                double pointManualY2;
                string strpointManualY2 = tffittingManPoint2Y.Text.Replace(',', '.');
                bool isN = Double.TryParse(strpointManualY2, out pointManualY2);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje y koordinate druge tačke ručnog fitovanja!");
                }
                else
                {
                    OptionsInPlottingMode.pointManualY2 = pointManualY2;
                }

                writeXMLFileOffline();

                pointManualFittingY2 = pointManualY2;
            }
            catch (Exception ex)
            {
                pointManualFittingY2 = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstffittingManPoint2Y_Offline(out double pointManualFittingY2)}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingManPoint2Y_Offline()
        {
            try
            {
                tffittingManPoint2Y.Foreground = Brushes.Black;
                //tffittingManPoint2Y.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingManPoint2Y_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint2Y_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingManPoint2Y.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint2Y_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint2Y_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    double pfY2 = 0;
                    saveOptionstffittingManPoint2Y_Offline(out pfY2);
                    markSavedOnlineOptionsAsBlacktffittingManPoint2Y_Offline();
                    setPointAtGraphicY2(pfY2);

                    //calculateCurrKandN();
                    drawFittingGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint2Y_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        public void setPointAtGraphicY2(double pfY2)
        {
            try
            {
                //// first empty source for graph plotting
                //_MarkerGraph.DataSource = null;
                //// then create new graph
                //xMarkers = new List<double>();
                //yMarkers = new List<double>();
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers2);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers2);
                _MarkerGraph2.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Add(OptionsInPlottingMode.pointManualX2);
                //yMarkers.Add(pfY2);

                xMarkers2[0] = OptionsInPlottingMode.pointManualX2;
                yMarkers2[0] = pfY2;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Yellow);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph2.Marker = mkr;

                //optionsPlotting.setTextMarkert2();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void setPointAtGraphicY2(double pfY2)}", System.DateTime.Now);
            }
        }

        #endregion

        

        #region fittingManPoint3X

        private void saveOptionstffittingManPoint3X_Offline(out double pointManualFittingX3)
        {
            try
            {
                double pointManualX3;
                string strpointManualX3 = tffittingManPoint3X.Text.Replace(',', '.');
                bool isN = Double.TryParse(strpointManualX3, out pointManualX3);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje x koordinate treće tačke ručnog fitovanja!");
                }
                else
                {
                    OptionsInPlottingMode.pointManualX3 = pointManualX3;
                }

                writeXMLFileOffline();

                pointManualFittingX3 = pointManualX3;
            }
            catch (Exception ex)
            {
                pointManualFittingX3 = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstffittingManPoint3X_Offline(out double pointManualFittingX3)}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingManPoint3X_Offline()
        {
            try
            {
                tffittingManPoint3X.Foreground = Brushes.Black;
                //tffittingManPoint3X.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingManPoint3X_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint3X_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingManPoint3X.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint3X_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint3X_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    double pfX3 = 0;
                    saveOptionstffittingManPoint3X_Offline(out pfX3);
                    markSavedOnlineOptionsAsBlacktffittingManPoint3X_Offline();
                    setPointAtGraphicX3(pfX3);

                    //calculateCurrKandN();
                    drawFittingGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint3X_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        public void SetPointAtGraphicX3_withXY(double pfX1, double pfY1)
        {
            try
            {
                setPointAtGraphicX3_withXY(pfX1, pfY1);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetPointAtGraphicX3_withXY(double pfX1, double pfY1)}", System.DateTime.Now);
            }
        }

        private void setPointAtGraphicX3_withXY(double pfX3, double pfY3)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers3);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers3);
                _MarkerGraph3.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX3);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY3);
                xMarkers3[0] = pfX3;
                yMarkers3[0] = pfY3;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Red), 1);
                _MarkerGraph3.Marker = mkr;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicX3_withXY(double pfX3, double pfY3)}", System.DateTime.Now);
            }

        }

        private void setPointAtGraphicX3(double pfX3)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers3);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers3);
                _MarkerGraph3.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX3);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY3);
                xMarkers3[0] = pfX3;
                yMarkers3[0] = OptionsInPlottingMode.pointManualY3;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Red), 1);
                _MarkerGraph3.Marker = mkr;

                //optionsPlotting.setTextMarkert3();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setPointAtGraphicX3(double pfX3)}", System.DateTime.Now);
            }
        }

        #endregion

       

        #region fittingManPoint3Y

        private void saveOptionstffittingManPoint3Y_Offline(out double pointManualFittingY3)
        {
            try
            {
                double pointManualY3;
                string strpointManualY3 = tffittingManPoint3Y.Text.Replace(',', '.');
                bool isN = Double.TryParse(strpointManualY3, out pointManualY3);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje y koordinate treće tačke ručnog fitovanja!");
                }
                else
                {
                    OptionsInPlottingMode.pointManualY3 = pointManualY3;
                }

                writeXMLFileOffline();

                pointManualFittingY3 = pointManualY3;
            }
            catch (Exception ex)
            {
                pointManualFittingY3 = 0;
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void saveOptionstffittingManPoint3Y_Offline(out double pointManualFittingY3)}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingManPoint3Y_Offline()
        {
            try
            {
                tffittingManPoint3Y.Foreground = Brushes.Black;
                //tffittingManPoint3Y.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingManPoint3Y_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint3Y_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingManPoint3Y.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint3Y_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingManPoint3Y_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    double pfY3 = 0;
                    saveOptionstffittingManPoint3Y_Offline(out pfY3);
                    markSavedOnlineOptionsAsBlacktffittingManPoint3Y_Offline();
                    setPointAtGraphicY3(pfY3);

                    //calculateCurrKandN();
                    drawFittingGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tffittingManPoint3Y_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        public void setPointAtGraphicY3(double pfY3)
        {
            try
            {
                //// first empty source for graph plotting
                //_MarkerGraph.DataSource = null;
                //// then create new graph
                //xMarkers = new List<double>();
                //yMarkers = new List<double>();
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers3);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers3);
                _MarkerGraph3.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Add(OptionsInPlottingMode.pointManualX2);
                //yMarkers.Add(pfY2);

                xMarkers3[0] = OptionsInPlottingMode.pointManualX3;
                yMarkers3[0] = pfY3;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph3.Marker = mkr;

                //optionsPlotting.setTextMarkert3();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void setPointAtGraphicY3(double pfY3)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ManPointR2R4


        public void setPointAtGraphicR2R4(double x, double y)
        {
            try
            {
                //// first empty source for graph plotting
                //_MarkerGraph.DataSource = null;
                //// then create new graph
                //xMarkers = new List<double>();
                //yMarkers = new List<double>();
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers9);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers9);
                _MarkerGraph9.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Add(OptionsInPlottingMode.pointManualX2);
                //yMarkers.Add(pfY2);

                xMarkers9[0] = x;
                yMarkers9[0] = y;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph9.Marker = mkr;

                //optionsPlotting.setTextMarkert9();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void setPointAtGraphicR2R4(double x, double y)}", System.DateTime.Now);
            }
        }


        #endregion

        public int GetClosestPointIndex(double preassureCriteria) 
        {
            try
            {
                return getClosestPointIndex(preassureCriteria);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public int GetClosestPointIndex(double preassureCriteria)}", System.DateTime.Now);
                return 0;               
            }
        }

        private int getClosestPointIndex(double preassureCriteria)
        {
            try
            {
                int closestPointIndex = 0;
                double min = double.MaxValue;
                for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                {
                    if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.fittingAutoMaxXValue)
                    {
                        return closestPointIndex;
                    }
                    double currSubs = Math.Abs(preassureCriteria - dataReader.PreassureInMPa[i]);
                    if (currSubs < min)
                    {
                        min = currSubs;
                        closestPointIndex = i;
                    }
                }

                return closestPointIndex;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private int getClosestPointIndex(double preassureCriteria)}", System.DateTime.Now);
                return 0;                
            }
        }

  

        private void ConnectTwoDiscreteDisplays(MyPoint p1, MyPoint p2) 
        {
            try
            {
                MyPointCollection pointsDiscrete;


                var numberOfOnline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Ukupno izduženje").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Ukupno izduženje").Single();

                    plotter.Children.Remove(lineToRemove);
                }

                pointsDiscrete = new MyPointCollection(Constants.TOTAL_POINTSDef);

                var ds = new EnumerableDataSource<MyPoint>(pointsDiscrete);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);

                plotter.AddLineGraph(ds, Colors.Red, 2, "Ukupno izduženje"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"

                pointsDiscrete.Add(p1);
                pointsDiscrete.Add(p2);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void ConnectTwoDiscreteDisplays(MyPoint p1, MyPoint p2)}", System.DateTime.Now);
            }
        }

        public void ConnectDiscreteDisplays()
        {
            try
            {
                MyPointCollection pointsDiscrete;


                var numberOfOnline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();

                    plotter.Children.Remove(lineToRemove);
                }

                pointsDiscrete = new MyPointCollection(Constants.TOTAL_POINTSDef);

                var ds = new EnumerableDataSource<MyPoint>(pointsDiscrete);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);

                plotter.AddLineGraph(ds, Colors.Gray, 2, "Fitting Mode"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"

                double xSubstraction = xMarkers2[0] - xMarkers[0];
                xSubstractionForFittingLine = xSubstraction;
                if ((xSubstraction > -0.01 && xSubstraction < 0.01) || xSubstraction == -0.01 || xSubstraction == 0.01)
                {

                    double yn = 0;
                    double xConst = xMarkers[0];
                    while (1.1 * currMaxPreassure > yn)
                    {
                        yn = yn + (currMaxPreassure / 100);
                        pointsDiscrete.Add(new MyPoint(yn, xConst));
                    }
                }
                else
                {
                    double k = (yMarkers2[0] - yMarkers[0]) / (xMarkers2[0] - xMarkers[0]);
                    currK = k;
                    double n = yMarkers2[0] - k * xMarkers2[0];
                    currN = n;
                    writeXMLFileOffline();
                    int cn = 1;

                    double xn = 0;
                    double yn = k * xn + n;

                    //ako je ipak k beskonacno
                    if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                    {
                        double yn2 = 0;
                        double xConst = xMarkers[0];
                        while (1.1 * currMaxPreassure > yn2)
                        {
                            yn2 = yn2 + (currMaxPreassure / 100);
                            pointsDiscrete.Add(new MyPoint(yn2, xConst));
                        }
                    }
                    //ako je ipak k beskonacno
                    else
                    {//a ako ipak je konacan broj k nastavi dalje

                        if (OptionsInPlottingMode.isAutoChecked)
                        {
                            double rpmax = Double.MinValue;
                            string strrpmax = tfRm.Text.Replace(',', '.');
                            bool isN = Double.TryParse(strrpmax, out rpmax);
                            if (isN == false)
                            {
                                MessageBox.Show("U polju za Rpmax mora biti broj prikazan!");
                            }
                            while (xn <= 1.1 * currMaxRelativeElongation && yn < 1.5 * rpmax)
                            {
                                yn = k * xn + n;
                                if (yn >= Double.MaxValue) break;
                                if (yn < -10000) break;
                                pointsDiscrete.Add(new MyPoint(yn, xn));

                                cn++;
                                xn = cn * 0.2;
                            }
                        }

                        if (OptionsInPlottingMode.isManualChecked)
                        {
                            while (xn <= OptionsInPlottingMode.xRange)
                            {
                                yn = k * xn + n;
                                if (yn >= Double.MaxValue) break;
                                if (yn < -10000) break;
                                pointsDiscrete.Add(new MyPoint(yn, xn));

                                cn++;
                                xn = cn * 0.2;
                            }
                        }
                    }
                }

                pointsDiscreteForMutualPoint = pointsDiscrete;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void ConnectDiscreteDisplays()}", System.DateTime.Now);
            }
            
        }

        private void DeleteConnectTwoDiscreteDisplays() 
        {
            try
            {
                MyPointCollection pointsDiscrete;
                //dataReader = new DataReader(OptionsInPlottingMode.filePath);
                //dataReader.ReadData();

                var numberOfOnline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == Constants.TOTALELONGATIONCAPTION).Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == Constants.TOTALELONGATIONCAPTION).Single();

                    plotter.Children.Remove(lineToRemove);
                }

                pointsDiscrete = new MyPointCollection(Constants.TOTAL_POINTSDef);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void DeleteConnectTwoDiscreteDisplays()}", System.DateTime.Now);
            }
        }

        public void DeleteConnectDiscreteDisplays()
        {
            try
            {
                MyPointCollection pointsDiscrete;
                //dataReader = new DataReader(OptionsInPlottingMode.filePath);
                //dataReader.ReadData();

                var numberOfOnline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();

                    plotter.Children.Remove(lineToRemove);
                }

                pointsDiscrete = new MyPointCollection(Constants.TOTAL_POINTSDef);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DeleteConnectDiscreteDisplays()}", System.DateTime.Now);
            }

        }
       



        //private void btnTurnOnFittingLine_Click(object sender, RoutedEventArgs e)
        //{
        //    ConnectDiscreteDisplays();
        //}

        public void SetResultsInterfaceForManualSetPoint()
        {
            try
            {
                setResultsInterfaceForManualSetPoint();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetResultsInterfaceForManualSetPoint()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// metoda kojom se uskladjuje prikaz izlaznog interfejsa(ResultsInterface) posle rucnog postavljanja neke od tacaka
        /// </summary>
        private void setResultsInterfaceForManualSetPoint() 
        {
            try
            {
                /*ovo mora ici u posebnu metodu*/
                //set results interface
                if (ResultsInterface.isCreatedResultsInterface == false)
                {
                    //sta god je rucno postavljeno nemoj da citas iz xml fajla jel vise za tu vrednost, koja je rucno postavljena ne vazi nova vrednost
                    if (isRp02Active && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        onMode.loadFirstAfterRunResultsInterface(false, true, true, true, true);
                    }
                    else if (isRmActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        onMode.loadFirstAfterRunResultsInterface(true, false, true, true, true);
                    }
                    else if (isReLActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        onMode.loadFirstAfterRunResultsInterface(true, true, false, true, true);
                    }
                    else if (isReHActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        onMode.loadFirstAfterRunResultsInterface(true, true, true, false, true);
                    }
                    else if (isAActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        onMode.loadFirstAfterRunResultsInterface(true, true, true, true, false);
                    }
                    else
                    {
                        onMode.loadFirstAfterRunResultsInterface();
                    }

                    //onMode.ResultsInterface.Show();
                }
                //onMode.setResultsInterface(string.Empty, string.Empty);
                onMode.setResultsInterface(suStr, zStr);
                if (OnlineFileHeader.isCreatedOnlineHeader == false)
                {
                    onMode.showInputData(false);
                }
                onMode.ResultsInterface.SetRadioButtons();
                //onMode.ResultsInterface.SetTextBoxes();

                if (isRectangle == true)
                {
                    onMode.ResultsInterface.SetTextBoxes(_au, _bu);
                }
                if (isCircle == true)
                {
                    onMode.ResultsInterface.SetTextBoxes(_Du);
                }

                /*ovo mora ici u posebnu metodu*/
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setResultsInterfaceForManualSetPoint()}", System.DateTime.Now);
            }
        }
        
        private void plotter_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
          
        }


        private void plotter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                IsEverT1T2orT3ManualSetted = true;
                IsYungFirstTimeCalculate = false;
                IWantToBackFirstCalculateYung = false;
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                window.IWantLoadYungFromFile = false;

                if (OptionsInPlottingMode.isManualChecked)
                {

                }
                if (OptionsInPlottingMode.isAutoChecked)
                {
                    //return;
                }

                if (OptionsInPlottingMode.isManualFittingChecked == true)
                {

                    Point mousePos = mouseTrack.Position;
                    var transform = plotter.Viewport.Transform;
                    Point mousePosInData = mousePos.ScreenToData(transform);


                    WindowForChosingPoints windowForChosingPoints = new WindowForChosingPoints();
                    windowForChosingPoints.X = (double)mousePos.X;
                    windowForChosingPoints.Y = (double)mousePos.Y;
                    windowForChosingPoints.setWindowForChosingPoints();
                    windowForChosingPoints.GraphicPlotting = this;
                    //ove dve metode su glavne za dodavanje checkbox elemenata, menija kojima se tacke rucno postavljaju
                    windowForChosingPoints.setOriginalCheckbox();
                    windowForChosingPoints.setChangeRatioCheckbox();
                    windowForChosingPoints.IsOriginalCheckboxCheckedProgramally = false;
                    windowForChosingPoints.IsChangeRatioCheckboxCheckedProgramally = false;

                    if (enableOnlyRp02ReLReHRm == true)
                    {
                        windowForChosingPoints.rbtnT1.IsEnabled = false;
                        windowForChosingPoints.rbtnT2.IsEnabled = false;
                        windowForChosingPoints.rbtnT3.IsEnabled = false;
                        windowForChosingPoints.chbOriginal.IsEnabled = false;
                        windowForChosingPoints.chbChangeRatio.IsEnabled = false;
                    }
                    else
                    {
                        windowForChosingPoints.rbtnT1.IsEnabled = true;
                        windowForChosingPoints.rbtnT2.IsEnabled = true;
                        windowForChosingPoints.rbtnT3.IsEnabled = true;
                        windowForChosingPoints.chbOriginal.IsEnabled = true;
                        windowForChosingPoints.chbChangeRatio.IsEnabled = true;
                    }

                    if (IsWindowForChosingPointsShown == false)
                    {
                        IsWindowForChosingPointsShown = true;
                        windowForChosingPoints.ShowDialog();

                    }


                    //if (mousePosInData.X >= OptionsInPlottingMode.pointManualX1 - 100 && mousePosInData.X <= OptionsInPlottingMode.pointManualX1 + 100 && mousePosInData.Y >= OptionsInPlottingMode.pointManualY1 - 100 && mousePosInData.Y <= OptionsInPlottingMode.pointManualY1 + 100)
                    //{
                    //    isT1Active = true;
                    //    isT2Active = false;
                    //    isT3Active = false;
                    //}
                    //if (mousePosInData.X >= OptionsInPlottingMode.pointManualX2 - 100 && mousePosInData.X <= OptionsInPlottingMode.pointManualX2 + 100 && mousePosInData.Y >= OptionsInPlottingMode.pointManualY2 - 100 && mousePosInData.Y <= OptionsInPlottingMode.pointManualY2 + 100)
                    //{
                    //    isT1Active = false;
                    //    isT2Active = true;
                    //    isT3Active = false;
                    //}
                    //if (mousePosInData.X >= OptionsInPlottingMode.pointManualX3 - 100 && mousePosInData.X <= OptionsInPlottingMode.pointManualX3 + 100 && mousePosInData.Y >= OptionsInPlottingMode.pointManualY3 - 100 && mousePosInData.Y <= OptionsInPlottingMode.pointManualY3 + 100)
                    //{
                    //    isT1Active = false;
                    //    isT2Active = false;
                    //    isT3Active = true;
                    //}

                    if (isReHActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        double ReHX = mousePosInData.X;
                        double ReHY = mousePosInData.Y;
                        ReHX = Math.Round(ReHX, 1);
                        ReHY = Math.Round(ReHY, 0);

                        string currInOutFileName = getCurrentInputOutputFile();
                        if (File.Exists(currInOutFileName) == true)
                        {
                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            dataListInputOutput.Add(Constants.ReHManual + '\t' + ReHY + '\t' + Constants.MPa + '\t' + Constants.ReHX + '\t' + ReHX + '\t' + Constants.procent);
                            File.Delete(currInOutFileName);
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);
                        }


                        //set ReH exacly at the green (original) graphic



                        //for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                        //{

                        //    if (OptionsInPlottingMode.isManualFittingChecked)
                        //    {

                        //        if (dataReader.PreassureInMPa[i] >= ReHY)
                        //        {
                        //            dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i],0);
                        //            tfReH.Text = dataReader.PreassureInMPa[i].ToString();
                        //            setReHPoint(dataReader.RelativeElongation[i], dataReader.PreassureInMPa[i]);
                        //            break;
                        //        }
                        //    }
                        //}

                        //trazi najblize rastojanje od kliknute tacke i na fitovanom grafiku postavi (ako nije ucitan fitovani onda na originalnom grafiku)
                        //NAPOMENA : X JE MNOGO MANJE OD Y PA SE ZA RACUNANJE RASTOJANJA X MNOZI SA Rm-om
                        double minDistance = Double.MaxValue;
                        int imin = 0;
                        if (isGraphicFittingLoaded == false)
                        {
                            for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                            {
                                if (OptionsInPlottingMode.isManualFittingChecked)
                                {
                                    double currDistance = Math.Sqrt(Math.Pow((ReHX * Rm) - (dataReader.RelativeElongation[i] * Rm), 2) + Math.Pow((ReHY) - (dataReader.PreassureInMPa[i]), 2));
                                    if (currDistance < minDistance)
                                    {
                                        dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 0);
                                        imin = i;
                                        minDistance = currDistance;
                                    }
                                }
                            }

                            tfReH.Text = dataReader.PreassureInMPa[imin].ToString();
                            ReH = dataReader.PreassureInMPa[imin];
                            setReHPoint(dataReader.RelativeElongation[imin], dataReader.PreassureInMPa[imin]);
                        }
                        else
                        {
                            for (int i = 0; i < dataReader.FittingPreassureInMPa.Count; i++)
                            {
                                if (OptionsInPlottingMode.isManualFittingChecked)
                                {
                                    double currDistance = Math.Sqrt(Math.Pow((ReHX * Rm) - (dataReader.FittingRelativeElongation[i] * Rm), 2) + Math.Pow((ReHY) - (dataReader.FittingPreassureInMPa[i]), 2));
                                    if (currDistance < minDistance)
                                    {
                                        dataReader.FittingPreassureInMPa[i] = Math.Round(dataReader.FittingPreassureInMPa[i], 0);
                                        imin = i;
                                        minDistance = currDistance;
                                    }
                                }
                            }

                            tfReH.Text = dataReader.FittingPreassureInMPa[imin].ToString();
                            ReH = dataReader.FittingPreassureInMPa[imin];

                            setReHPoint(dataReader.FittingRelativeElongation[imin], dataReader.FittingPreassureInMPa[imin]);
                        }
                        chbReHVisibility.IsChecked = true;


                        setResultsInterfaceForManualSetPoint();

                    }//end of ReH

                    if (isReLActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        double ReLX = mousePosInData.X;
                        double ReLY = mousePosInData.Y;

                        if (ReLX < 0)
                        {
                            ReLX = 0;
                        }

                        ReL = Math.Round(ReLX, 1);
                        ReLY = Math.Round(ReLY, 0);

                        string currInOutFileName = getCurrentInputOutputFile();
                        if (File.Exists(currInOutFileName) == true)
                        {
                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            dataListInputOutput.Add(Constants.ReLManual + '\t' + ReLY + '\t' + Constants.MPa + '\t' + Constants.ReLX + '\t' + ReLX + '\t' + Constants.procent);
                            File.Delete(currInOutFileName);
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);
                        }

                        //set ReL exacly at the green (original) graphic

                        if (isGraphicFittingLoaded == false)
                        {

                            for (int i = 0; i < dataReader.RelativeElongation.Count; i++)
                            {

                                if (OptionsInPlottingMode.isManualFittingChecked)
                                {

                                    if (dataReader.RelativeElongation[i] >= ReLX)
                                    {
                                        ReL = dataReader.PreassureInMPa[i];
                                        dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 0);


                                        tfReL.Text = dataReader.PreassureInMPa[i].ToString();
                                        if (onMode != null && onMode.ResultsInterface != null)
                                        {
                                            onMode.ResultsInterface.tfReL.Text = dataReader.PreassureInMPa[i].ToString();
                                        }
                                        ReL = dataReader.PreassureInMPa[i];
                                        setReLPoint(dataReader.RelativeElongation[i], dataReader.PreassureInMPa[i]);
                                        printscreen.setReLPoint_Manual(dataReader.RelativeElongation[i], dataReader.PreassureInMPa[i]);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (pointsOfFittingLine != null)
                            {

                                for (int i = 0; i < dataReader.FittingRelativeElongation.Count; i++)
                                {

                                    if (OptionsInPlottingMode.isManualFittingChecked)
                                    {

                                        if (dataReader.FittingRelativeElongation[i] >= ReLX)
                                        {

                                            dataReader.FittingPreassureInMPa[i] = Math.Round(dataReader.FittingPreassureInMPa[i], 0);

                                            tfReL.Text = dataReader.FittingPreassureInMPa[i].ToString();
                                            onMode.ResultsInterface.tfReL.Text = dataReader.FittingPreassureInMPa[i].ToString();
                                            ReL = dataReader.FittingPreassureInMPa[i];
                                            setReLPoint(dataReader.FittingRelativeElongation[i], dataReader.FittingPreassureInMPa[i]);
                                            printscreen.setReLPoint_Manual(dataReader.FittingRelativeElongation[i], dataReader.FittingPreassureInMPa[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        chbReLVisibility.IsChecked = true;


                        setResultsInterfaceForManualSetPoint();


                    }//end of ReL

                    if (isRmActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        double RmX = mousePosInData.X;
                        double RmY = mousePosInData.Y;

                        if (RmX < 0)
                        {
                            RmX = 0;
                        }

                        RmX = Math.Round(RmX, 1);
                        RmY = Math.Round(RmY, 0);

                        string currInOutFileName = getCurrentInputOutputFile();
                        if (File.Exists(currInOutFileName) == true)
                        {
                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            dataListInputOutput.Add(Constants.RmManual + '\t' + RmY + '\t' + Constants.MPa + '\t' + Constants.RmX + '\t' + RmX + '\t' + Constants.procent);
                            File.Delete(currInOutFileName);
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);
                        }
                        //set Rm exacly at the green (original) graphic


                        if (isGraphicFittingLoaded == false)
                        {
                            for (int i = 0; i < dataReader.RelativeElongation.Count; i++)
                            {

                                if (OptionsInPlottingMode.isManualFittingChecked)
                                {

                                    if (dataReader.RelativeElongation[i] >= RmX)
                                    {
                                        dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 0);
                                        tfRm.Text = dataReader.PreassureInMPa[i].ToString();
                                        Rm = dataReader.PreassureInMPa[i];
                                        if (OptionsInOnlineMode.isCalibration == true)
                                        {
                                            double force = dataReader.PreassureInMPa[i] * dataReader.S0Offline;//calculate in N not in kN
                                            //force = force * 1000;//ovo je suvisno
                                            force = Math.Round(force, 0);
                                            tfFm.Text = force.ToString();
                                            Fm = force;
                                        }
                                        //setRmPoint(dataReader.RelativeElongation[i], dataReader.PreassureInMPa[i]);
                                        setManualSettedTrianglePoints();
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dataReader.FittingRelativeElongation.Count; i++)
                            {

                                if (OptionsInPlottingMode.isManualFittingChecked)
                                {

                                    if (dataReader.FittingRelativeElongation[i] >= RmX)
                                    {
                                        dataReader.FittingPreassureInMPa[i] = Math.Round(dataReader.FittingPreassureInMPa[i], 0);
                                        tfRm.Text = dataReader.FittingPreassureInMPa[i].ToString();
                                        Rm = dataReader.FittingPreassureInMPa[i];
                                        if (OptionsInOnlineMode.isCalibration == true)
                                        {
                                            double force = dataReader.FittingPreassureInMPa[i] * dataReader.S0Offline;//calculate in N not in kN
                                            //force = force * 1000;//ovo je suvisno
                                            force = Math.Round(force, 0);
                                            tfFm.Text = force.ToString();
                                            Fm = force;
                                        }
                                        //setRmPoint(dataReader.FittingRelativeElongation[i], dataReader.FittingPreassureInMPa[i]);
                                        setManualSettedTrianglePoints();
                                        break;
                                    }
                                }
                            }
                        }
                        chbRmVisibility.IsChecked = true;

                        setResultsInterfaceForManualSetPoint();

                        //set manual Fm
                        Fm = Math.Round(Fm, 0);
                        if (File.Exists(currInOutFileName) == true)
                        {
                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            dataListInputOutput.Add(Constants.FmManual + '\t' + Fm + '\t' + Constants.N);
                            File.Delete(currInOutFileName);
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);
                        }


                        ag = findAg(Rm, RmX);
                        ag = Math.Round(ag, 1);
                        _ag_X = ag;
                        mkrTriangleCurrentValues.AgXValue = ag;


                        setResultsInterfaceForManualSetPoint();

                        //setRmPoint(mkrTriangleCurrentValues.RmXValue, mkrTriangleCurrentValues.RmYValue);

                    }//end of Rm


                    if (isRp02Active && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        double Rp02X = mousePosInData.X;
                        double Rp02Y = mousePosInData.Y;
                        Rp02X = Math.Round(Rp02X, 1);
                        Rp02Y = Math.Round(Rp02Y, 0);

                        string currInOutFileName = getCurrentInputOutputFile();
                        if (File.Exists(currInOutFileName) == true)
                        {
                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                            dataListInputOutput.Add(Constants.Rp02Manual + '\t' + Rp02Y + '\t' + Constants.MPa + '\t' + Constants.Rp02X + '\t' + Rp02X + '\t' + Constants.procent);
                            File.Delete(currInOutFileName);
                            File.WriteAllLines(currInOutFileName, dataListInputOutput);
                        }


                        setRp02PointOutsideGraphic(Rp02X, Rp02Y);
                        Rp02Y = Math.Round(Rp02Y, 0);
                        tfRp02.Text = Rp02Y.ToString();
                        Rp02RI = Rp02Y;
                        chbRp02Visibility.IsEnabled = true;
                        chbRp02Visibility.IsChecked = true;

                        //set Rp02 exacly at the blue (fitting) graphic
                        int i = 0;
                        for (i = 0; i < dataReader.FittingRelativeElongation.Count; i++)
                        {

                            if (OptionsInPlottingMode.isManualFittingChecked)
                            {

                                if (dataReader.FittingRelativeElongation[i] >= Rp02X)
                                {
                                    dataReader.FittingPreassureInMPa[i] = Math.Round(dataReader.FittingPreassureInMPa[i], 0);
                                    tfRp02.Text = dataReader.FittingPreassureInMPa[i].ToString();
                                    Rp02RI = dataReader.FittingPreassureInMPa[i];
                                    chbRp02Visibility.IsEnabled = true;
                                    chbRp02Visibility.IsChecked = true;
                                    break;
                                }
                            }
                        }



                        if (pointsOfFittingLine != null)
                        {
                            //desava se da kada testiram kidanje koje je prekinuto maltene na samom pocetku
                            //da i postane jednako dataReader.FittingRelativeElongation.Count
                            //i tada program puca.
                            //Da do toga ne bi doslo postavljamo Rp02 na vrensot zadnje iscrtane tacke
                            if (i >= dataReader.FittingRelativeElongation.Count)
                            {
                                i = dataReader.FittingRelativeElongation.Count - 1;
                            }
                            setRp02Point(dataReader.FittingRelativeElongation[i], dataReader.FittingPreassureInMPa[i]);
                        }
                        yMarkers7[0] = Math.Round(yMarkers7[0], 0);
                        tfRp02.Text = yMarkers7[0].ToString();
                        Rp02RI = yMarkers7[0];

                        /*ovo mora ici u posebnu metodu*/
                        //set results interface
                        //if (ResultsInterface.isCreatedResultsInterface == false)
                        //{
                        //    onMode.loadFirstAfterRunResultsInterface();
                        //}
                        //onMode.setResultsInterface(string.Empty, string.Empty);
                        //if (OnlineFileHeader.isCreatedOnlineHeader == false)
                        //{
                        //    onMode.showInputData();
                        //}
                        //onMode.ResultsInterface.SetRadioButtons();
                        //onMode.ResultsInterface.SetTextBoxes();
                        setResultsInterfaceForManualSetPoint();
                        /*ovo mora ici u posebnu metodu*/


                        chbRp02Visibility.IsEnabled = true;
                        chbRp02Visibility.IsChecked = true;
                        //calculateCurrKandN();

                        //ovde nikako ne treba ponovo pozivati iscrtavanje fitovanog grafika jel pomera i tacku T3 klikom na tacku Rp02 
                        //ne iscrtavati ponovo fitovani grafik kada se pomeraju fitovanog grafika [Rp02 i A]
                        //T3movingDirectionByYAxis = false;
                        //drawFittingGraphic(T3movingDirectionByYAxis);
                        //chbRp02Visibility.IsChecked = true;
                        //if (onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                        //{
                        //    calculateYungsModuo();
                        //}
                        //if (onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                        //{
                        //    _rp02XValue = Rp02X;
                        //    calculateYungsModuo(0.005, true);
                        //}
                    }//end of Rp02

                    if (isAActive && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        double newNGlobal;
                        double currKGlobal = 0;


                        double AXprim = mousePosInData.X;
                        double AY = mousePosInData.Y;

                        MyPoint greenLastPoint = new MyPoint(0,0);
                        int indexEnd = 0;

                        double currentDistance = Double.MaxValue;
                        double mintDistance = Double.MaxValue;

                        for (int j = 0; j < points.Count; j++) 
                        {
                            if (OptionsInPlottingMode.isManualFittingChecked)
                            {
                                currentDistance = Math.Sqrt(Math.Pow(Rm * AXprim - Rm * points[j].XAxisValue, 2) + Math.Pow(AY - points[j].YAxisValue, 2));
                                if (currentDistance <= mintDistance)
                                {
                                    mintDistance = currentDistance;
                                    //greenLastPoint = new MyPoint(dataReader.PreassureInMPa[j],dataReader.RelativeElongation[j]);
                                    greenLastPoint = new MyPoint(points[j].YAxisValue, points[j].XAxisValue);
                                    indexEnd = j;
                                }
                                //if (AXprim <= points[j].XAxisValue)
                                //{
                                //    greenLastPoint = points[j];
                                //    indexEnd = j;
                                //    break;
                                //}
                            }
                        }

                        //MessageBox.Show(greenLastPoint.XAxisValue + " " + greenLastPoint.YAxisValue);

                        double t1x = 0;
                        double t1y = 0;
                        double t2x = 0;
                        double t2y = 0;
                        double t3x = 0;
                        double t3y = 0;

                        double k = 0;
                     

                        double NForFittingLine = Double.MinValue;
                        deleteOnlyFittingLine();
                        deleteOnlyFittingLinePrintScreen();
                        calculateCurrKandN();//pre nego sto pocnes sa iscrtavanjem fitovanog grafika proracunaj parametre linije na osnovu koje radis fitovanje
                        dataReader.ClearFittingData();

                        double xTranslateAmount;
                        //ovo se odnosi samo kada se ucitava prethodno zapamcen fajl
                        bool foundIsCircleOrRectangle = false;





                        //double currSubstraction;
                        //double minsub = Double.MaxValue;
                        double currSubDistance;
                        double minsubDistance = Double.MaxValue;
                        int indexOfPointClosestToRed = 0;

                        MyPointCollection fittingPoints;
                        MyPoint pointOfTearing = new MyPoint(0, 0);


                        int maxPoints = 300000;
                        fittingPoints = new MyPointCollection(maxPoints);
                        pointsForFittingGraphic = fittingPoints;
                        fittingPoints.Clear();

                        var ds = new EnumerableDataSource<MyPoint>(fittingPoints);
                        ds.SetXMapping(x => x.XAxisValue);
                        ds.SetYMapping(y => y.YAxisValue);

                        //deleteFittingLineMode();


                        plotter.AddLineGraph(ds, Colors.Blue, 2, "Fitting Line"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                        isGraphicFittingLoaded = true;
                        //printscreen.plotterPrint.AddLineGraph(ds, Colors.Blue, 2, "grafik");
                        
                        printscreen.UpdatePrintScreen(fittingPoints);
                        
                        //MainWindow window = (MainWindow)MainWindow.GetWindow(this);






                        if (OptionsInPlottingMode.isManualChecked)
                        {

                            plotter.Viewport.AutoFitToView = true;
                            ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                            restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                            restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);


                            plotter.Viewport.Restrictions.Add(restr);

                        }

                        if (OptionsInPlottingMode.isAutoChecked)
                        {

                            plotter.FitToView();
                            plotter.Viewport.Restrictions.Clear();
                        }


                        int i;
                        /******   odredjivanje mesta sta se uzima za crvenu tacku kada se obelezi da je crvena tacka(T3) van zelenog grafika VAZNA STVAR     ********/
                        for (i = 0; i < dataReader.PreassureInMPa.Count; i++)
                        {
                            if (i % (OptionsInPlottingMode.Resolution) == 0)
                            {
                                //yOfLine = currK * dataReader.RelativeElongation[i] + currN;
                                //currSubDistance = Math.Abs(yOfLine - dataReader.PreassureInMPa[i]);
                                //ovo je vazilo do 10 oktobra 2014 godine
                                //if (OptionsInPlottingMode.isManualFittingChecked)
                                //{
                                //   if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.pointManualX3)
                                //    {
                                //        indexOfPointClosestToRed = i;
                                //        break;
                                //    }
                                //}
                                //else
                                //{
                                //    if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.pointAutoX3)
                                //    {
                                //        indexOfPointClosestToRed = i;
                                //        break;
                                //    }
                                //}

                                if (OptionsInPlottingMode.isManualFittingChecked)
                                {
                                    if (T3movingDirectionByYAxis)
                                    {
                                        if (dataReader.PreassureInMPa[i] >= OptionsInPlottingMode.pointManualY3)
                                        {
                                            indexOfPointClosestToRed = i;
                                            OptionsInPlottingMode.pointManualX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                                            OptionsInPlottingMode.pointManualY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                                            t3x = OptionsInPlottingMode.pointManualX3;
                                            t3y = OptionsInPlottingMode.pointManualY3;
                                            break;
                                        }
                                    }
                                    if (T3movingDirectionByYAxis == false)
                                    {
                                        if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.pointManualX3)
                                        {
                                            indexOfPointClosestToRed = i;
                                            OptionsInPlottingMode.pointManualX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                                            OptionsInPlottingMode.pointManualY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                                            t3x = OptionsInPlottingMode.pointManualX3;
                                            t3y = OptionsInPlottingMode.pointManualY3;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dataReader.PreassureInMPa[i] >= OptionsInPlottingMode.pointAutoY3)
                                    {
                                        indexOfPointClosestToRed = i;
                                        OptionsInPlottingMode.pointAutoX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                                        OptionsInPlottingMode.pointAutoY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                                        t3x = OptionsInPlottingMode.pointAutoX3;
                                        t3y = OptionsInPlottingMode.pointAutoY3;
                                        //mozda ove dve linije koda treba obrisati
                                        OptionsInPlottingMode.pointManualX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                                        OptionsInPlottingMode.pointManualY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                                        t3x = OptionsInPlottingMode.pointManualX3;
                                        t3y = OptionsInPlottingMode.pointManualY3;
                                        break;
                                    }
                                }
                            }
                        }

                        //postavi na grafiku tacku najblizu oznacenoj za T3
                        setPointAtGraphicX3(dataReader.RelativeElongation[indexOfPointClosestToRed]);
                        setPointAtGraphicY3(dataReader.PreassureInMPa[indexOfPointClosestToRed]);

                        //setManualPointsToAutoPointsValue();

                        if (OptionsInPlottingMode.isManualFittingChecked)
                        {
                            double currNAnother = Double.MaxValue;//ovo je n koje prolazi kroz crvenu tacku
                            currK = Math.Round(currK, 6);
                            OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                            OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);
                            currNAnother = OptionsInPlottingMode.pointManualY3 - currK * OptionsInPlottingMode.pointManualX3;
                            t3x = OptionsInPlottingMode.pointManualX3;
                            t3y = OptionsInPlottingMode.pointManualY3;

                            double xSubstractionForFitting = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                            t1x = OptionsInPlottingMode.pointManualX1;
                            t2x = OptionsInPlottingMode.pointManualX2;

                            if (xSubstractionForFitting < 0.01 || xSubstractionForFitting == 0.01) //ovde je koeficijent pravca beskonacan (prava je paralelna sa Y-osom)
                            {
                                xTranslateAmount = OptionsInPlottingMode.pointManualX3;
                                t3x = OptionsInPlottingMode.pointManualX3;
                            }
                            else
                            {
                                currNAnother = Math.Round(currNAnother, 6);
                                currK = Math.Round(currK, 6);
                                xTranslateAmount = (-1) * (currNAnother / currK);
                                xTranslateAmount = Math.Round(xTranslateAmount, 6);
                                xTranslateAmount = checkCurrKForInfinity(xTranslateAmount);

                            }

                        }
                        else
                        {
                            double currNAnother = Double.MaxValue;//ovo je n koje prolazi kroz crvenu tacku
                            OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                            OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);
                            currNAnother = OptionsInPlottingMode.pointAutoY3 - currK * OptionsInPlottingMode.pointAutoX3;
                            t3y = OptionsInPlottingMode.pointAutoY3;
                            t3x = OptionsInPlottingMode.pointAutoX3;
                            NForFittingLine = currNAnother;

                            double xSubstractionForFitting = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                            t1x = OptionsInPlottingMode.pointAutoX1;
                            t2x = OptionsInPlottingMode.pointAutoX2;

                            if (xSubstractionForFitting < 0.01 || xSubstractionForFitting == 0.01)
                            {
                                xTranslateAmount = OptionsInPlottingMode.pointAutoX3;
                                t3x = OptionsInPlottingMode.pointAutoX3;
                            }
                            else
                            {
                                currNAnother = Math.Round(currNAnother, 6);
                                currK = Math.Round(currK, 6);
                                xTranslateAmount = (-1) * (currNAnother / currK);
                                xTranslateAmount = checkCurrKForInfinity(xTranslateAmount);
                            }
                        }


                        xTranslateAmountFittingMode = xTranslateAmount;
                        //ova linija pravi problem
                        //onMode.VXY.FittingArrayElongationOfEndOfInterval(onMode.IndexFromChangedParametersFitting, OptionsInOnlineMode.E2E4Border, XTranslateAmountFittingMode);

                        fittingPoints.Add(new MyPoint(0, 0));
                        dataReader.FittingRelativeElongation.Add(0);//dodaj informaciju o relativnom izduzenju fitovanog grafika
                        dataReader.FittingPreassureInMPa.Add(0);//dodaj informaciju o naponu fitovanog grafika
                        double translatedXValue = dataReader.RelativeElongation[indexOfPointClosestToRed] - xTranslateAmount;
                        fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[indexOfPointClosestToRed], translatedXValue));
                        dataReader.FittingRelativeElongation.Add(translatedXValue);//dodaj informaciju o relativnom izduzenju fitovanog grafika
                        dataReader.FittingPreassureInMPa.Add(dataReader.PreassureInMPa[indexOfPointClosestToRed]);//dodaj informaciju o naponu fitovanog grafika

                        double A = 0;
                        //set relative elongation A


                        //this.indexEnd = dataReader.RelativeElongation.Count;
                        //calculateTotalRelElong(out pointOfTearing, out A);







                        indexOfPointClosestToRed = indexOfPointClosestToRed / OptionsInPlottingMode.Resolution;



                        for (i = indexOfPointClosestToRed; i <= indexEnd/*dataReader.RelativeElongation.Count*/; i++)
                        {
                           
                            //double translatedXValueFromOriginal = dataReader.RelativeElongation[i] - xTranslateAmount;
                            double translatedXValueFromOriginal = points[i].XAxisValue - xTranslateAmount;
                            dataReader.FittingRelativeElongation.Add(translatedXValueFromOriginal);//dodaj informaciju o relativnom izduzenju fitovanog grafika
                            //dataReader.FittingPreassureInMPa.Add(dataReader.PreassureInMPa[i]);//dodaj informaciju o naponu fitovanog grafika
                            dataReader.FittingPreassureInMPa.Add(points[i].YAxisValue);//dodaj informaciju o naponu fitovanog grafika

                           
                                

                                
                            //fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[i], translatedXValueFromOriginal));
                            fittingPoints.Add(new MyPoint(points[i].YAxisValue, translatedXValueFromOriginal));
                               

                        }

                        bool isN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                        bool isN2 = double.TryParse(tffittingManPoint2Y.Text, out t2y);

                        if (isN && isN2)
                        {
                            k = (t2y - t1y) / (t2x - t1x);
                        }
                        greenLastPoint.XAxisValue = greenLastPoint.XAxisValue - xTranslateAmount;
                        setAPoint(greenLastPoint.XAxisValue - greenLastPoint.YAxisValue / k);
                        printscreen.setAPoint(greenLastPoint.XAxisValue - greenLastPoint.YAxisValue / k);
                        this.A = greenLastPoint.XAxisValue - greenLastPoint.YAxisValue / k;
                        this.A = Math.Round(this.A, 1);
                        tfA.Text = this.A.ToString();
                        OnlineModeInstance.ResultsInterface.tfA.Text = this.A.ToString();
                        Lu = (1 + this.A / 100) * L0;
                        onMode.ResultsInterface.tfLu.Text = Lu.ToString();


                    //    double oldAX = A;

                    //    double AX = 0;

                    //    MyPoint pointOfTearing = new MyPoint(mousePosInData.Y, mousePosInData.X);
                    //    calculateTotalRelElongManual(pointOfTearing, out AX);

                    //    /***********************************************************************************************/

                    //    //if (AX < mkrTriangleCurrentValues.RmXValue)
                    //    if (mousePosInData.X < mkrTriangleCurrentValues.RmXValue)
                    //    {
                    //        _MarkerGraph4.Visibility = Visibility.Hidden;
                    //        printscreen._MarkerGraph4.Visibility = Visibility.Hidden;
                    //    }
                    //    else
                    //    {
                    //        _MarkerGraph4.Visibility = Visibility.Visible;
                    //        printscreen._MarkerGraph4.Visibility = Visibility.Visible;
                    //    }

                    //    //if (AX < mkrTriangleCurrentValues.ReLXValue)
                    //    if (mousePosInData.X < mkrTriangleCurrentValues.ReLXValue)
                    //    {
                    //        _MarkerGraph5.Visibility = Visibility.Hidden;
                    //        printscreen._MarkerGraph5.Visibility = Visibility.Hidden;
                    //    }
                    //    else
                    //    {
                    //        _MarkerGraph5.Visibility = Visibility.Visible;
                    //        printscreen._MarkerGraph5.Visibility = Visibility.Visible;
                    //    }

                    //    //if (AX < mkrTriangleCurrentValues.ReHXValue)
                    //    if (mousePosInData.X < mkrTriangleCurrentValues.ReHXValue)
                    //    {
                    //        _MarkerGraph6.Visibility = Visibility.Hidden;
                    //        printscreen._MarkerGraph6.Visibility = Visibility.Hidden;
                    //    }
                    //    else
                    //    {
                    //        _MarkerGraph6.Visibility = Visibility.Visible;
                    //        printscreen._MarkerGraph6.Visibility = Visibility.Visible;
                    //    }

                    //    //if (AX < mkrTriangleCurrentValues.Rp02XValue)
                    //    if (mousePosInData.X < mkrTriangleCurrentValues.Rp02XValue)
                    //    {
                    //        _MarkerGraph7.Visibility = Visibility.Hidden;
                    //        printscreen._MarkerGraph7.Visibility = Visibility.Hidden;
                    //    }
                    //    else
                    //    {
                    //        _MarkerGraph7.Visibility = Visibility.Visible;
                    //        printscreen._MarkerGraph7.Visibility = Visibility.Visible;
                    //    }

                    //    //if (AX < mkrTriangleCurrentValues.AgXValue)
                    //    if (mousePosInData.X < mkrTriangleCurrentValues.AgXValue)
                    //    {
                    //        printscreen._MarkerGraph10.Visibility = Visibility.Hidden;
                    //    }
                    //    else
                    //    {
                    //        printscreen._MarkerGraph10.Visibility = Visibility.Visible;
                    //    }

                    //    //if (AX < 0.5)
                    //    if (mousePosInData.X < 0.5)
                    //    {
                    //        printscreen._MarkerGraph9.Visibility = Visibility.Hidden;
                    //    }
                    //    else
                    //    {
                    //        printscreen._MarkerGraph9.Visibility = Visibility.Visible;
                    //    }

                    //    /***********************************************************************************************/

                    //    if (pointsOfFittingLine != null)
                    //    {
                    //        setAPoint_Manual(AX);
                    //    }
                    //    AX = Math.Round(AX, 1);
                    //    if (isAActive == true)
                    //    {
                    //        AManualClickedValue = Math.Round(AManualClickedValue, 2);
                    //        tfA.Text = AManualClickedValue.ToString();
                    //        //a = AManualClickedValue;
                    //        //AX = AManualClickedValue;
                    //    }
                    //    else
                    //    {
                    //        tfA.Text = AX.ToString();
                    //        A = AX;
                    //    }

                    //    //kada je nova vrednost manja tada se smanjuje fitovani grafik
                    //    if (AX < oldAX)
                    //    {
                    //        deleteOnlyFittingLine();

                    //        int howManyPointsToRemove = 1;
                    //        while (pointsForFittingGraphic[pointsForFittingGraphic.Count - howManyPointsToRemove].XAxisValue > AX)
                    //        {
                    //            howManyPointsToRemove++;
                    //            //ako se dobije da je index -1
                    //            if (howManyPointsToRemove > pointsForFittingGraphic.Count)
                    //            {
                    //                howManyPointsToRemove--;
                    //                break;
                    //            }
                    //        }
                    //        MyPointCollection currFittingpoint = new MyPointCollection(points.Count);

                    //        for (int i = 0; i < (pointsForFittingGraphic.Count - howManyPointsToRemove); i++)
                    //        {
                    //            currFittingpoint.Add(pointsForFittingGraphic[i]);
                    //        }

                    //        /*******************    ovo moze ici i u metodu    ************************/
                    //        //dodaj jos nekoliko tacaka jer kidanje ne ide pod pravim uglom u odnosu na A vec paralelno sa linijom fitovanja
                    //        //index nam govori odakle krecemo od points kolekcije koja sadrzi podatke o originalnom grafiku
                    //        int index = 0;
                    //        foreach (var point in points)
                    //        {
                    //            index++;
                    //            if ((point.XAxisValue - xTranslateAmountFittingMode) >= AX)
                    //            {
                    //                break;
                    //            }
                    //        }

                    //        double newN = -1 * AX * currK;
                    //        double currDifferenceinY = 0;
                    //        //prvo racunamo razliku po Y osi samo izmedju zadnje ucrtane tacke koja je normalna na A i prave koja je paralelna sa linijom fitovanja a sece x-osu u tacki A
                    //        double originalY = -1;
                    //        if (index <= points.Count - 1)
                    //        {
                    //            originalY = currK * points[index].XAxisValue + newN;
                    //        }
                    //        if (index <= points.Count - 1)
                    //        {
                    //            currDifferenceinY = Math.Abs(originalY - points[index].YAxisValue);
                    //        }
                    //        for (int i = index + 1; i < points.Count; i++)
                    //        {
                    //            double nextDifferenceinY = 0;
                    //            originalY = currK * points[i].XAxisValue + newN;
                    //            nextDifferenceinY = Math.Abs(originalY - points[i].YAxisValue);
                    //            if (nextDifferenceinY <= currDifferenceinY)
                    //            {
                    //                currFittingpoint.Add(points[i]);
                    //                currDifferenceinY = nextDifferenceinY;
                    //            }
                    //            else
                    //            {
                    //                break;
                    //            }
                    //        }


                    //        //postavi novo at
                    //        if (currFittingpoint != null && currFittingpoint.Count > 2)
                    //        {
                    //            at = currFittingpoint.Last().XAxisValue;
                    //            at = Math.Round(at, 1);
                    //            mkrTriangleCurrentValues.AtXValue = at;
                    //        }

                    //        /*******************    ovo moze ici i u metodu    ************************/


                    //        var ds = new EnumerableDataSource<MyPoint>(currFittingpoint);
                    //        ds.SetXMapping(x => x.XAxisValue);
                    //        ds.SetYMapping(y => y.YAxisValue);
                    //        plotter.AddLineGraph(ds, Colors.Blue, 2, "Fitting Line"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                    //        isGraphicFittingLoaded = true;
                    //        //printscreen.plotterPrint.AddLineGraph(ds, Colors.Blue, 2, "grafik");
                    //        printscreen.UpdatePrintScreen(currFittingpoint);
                    //        if (currFittingpoint.Count > 2)
                    //        {
                    //            pointsForFittingGraphic = currFittingpoint;
                    //        }

                    //    }
                    //    //kada je nova vrednost veca tada se povecava fitovani grafik
                    //    if (AX > oldAX)
                    //    {
                    //        deleteOnlyFittingLine();


                    //        MyPointCollection currFittingpoint = new MyPointCollection();
                    //        foreach (var point in pointsForFittingGraphic)
                    //        {
                    //            currFittingpoint.Add(point);
                    //        }


                    //        //dodaj sada dodatne tacke
                    //        for (int i = 0; i < points.Count; i++)
                    //        {
                    //            if (points[i].XAxisValue - xTranslateAmountFittingMode >= pointsForFittingGraphic.Last().XAxisValue && points[i].XAxisValue - xTranslateAmountFittingMode <= AX)
                    //            {
                    //                currFittingpoint.Add(new MyPoint(points[i].YAxisValue, points[i].XAxisValue - xTranslateAmountFittingMode));
                    //            }
                    //        }

                    //        /*******************    ovo moze ici i u metodu    ************************/
                    //        //dodaj jos nekoliko tacaka jer kidanje ne ide pod pravim uglom u odnosu na A vec paralelno sa linijom fitovanja
                    //        //index nam govori odakle krecemo od points kolekcije koja sadrzi podatke o originalnom grafiku
                    //        int index = 0;
                    //        foreach (var point in points)
                    //        {
                    //            index++;
                    //            if ((point.XAxisValue - xTranslateAmountFittingMode) >= AX)
                    //            {
                    //                break;
                    //            }
                    //        }

                    //        double newN = -1 * AX * currK;
                    //        newNGlobal = newN;
                    //        currKGlobal = currK;
                    //        double currDifferenceinY = 0;
                    //        //prvo racunamo razliku po Y osi samo izmedju zadnje ucrtane tacke koja je normalna na A i prave koja je paralelna sa linijom fitovanja a sece x-osu u tacki A
                    //        if (points.Count <= index)
                    //        {
                    //            index = points.Count - 1;
                    //        }
                    //        double originalY = currK * points[index].XAxisValue + newN;
                    //        currDifferenceinY = Math.Abs(originalY - points[index].YAxisValue);
                    //        for (int i = index + 1; i < points.Count; i++)
                    //        {
                    //            double nextDifferenceinY = 0;
                    //            originalY = currK * points[i].XAxisValue + newN;
                    //            nextDifferenceinY = Math.Abs(originalY - points[i].YAxisValue);
                    //            if (nextDifferenceinY <= currDifferenceinY)
                    //            {
                    //                currFittingpoint.Add(points[i]);
                    //                currDifferenceinY = nextDifferenceinY;
                    //            }
                    //            else
                    //            {
                    //                break;
                    //            }
                    //        }

                    //        //postavi novo at
                    //        at = currFittingpoint.Last().XAxisValue;
                    //        at = Math.Round(at, 1);
                    //        mkrTriangleCurrentValues.AtXValue = at;

                    //        /*******************    ovo moze ici i u metodu    ************************/

                    //        var ds = new EnumerableDataSource<MyPoint>(currFittingpoint);
                    //        ds.SetXMapping(x => x.XAxisValue);
                    //        ds.SetYMapping(y => y.YAxisValue);
                    //        plotter.AddLineGraph(ds, Colors.Blue, 2, "Fitting Line"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                    //        isGraphicFittingLoaded = true;
                    //        //printscreen.plotterPrint.AddLineGraph(ds, Colors.Blue, 2, "grafik");
                    //        printscreen.UpdatePrintScreen(currFittingpoint);
                    //        pointsForFittingGraphic = currFittingpoint;
                    //    }

                    //    string currInOutFileName = getCurrentInputOutputFile();
                    //    if (File.Exists(currInOutFileName) == true)
                    //    {
                    //        List<string> dataListInputOutput = new List<string>();
                    //        dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                    //        dataListInputOutput.Add(Constants.AManual + '\t' + AX + '\t' + Constants.procent);
                    //        File.Delete(currInOutFileName);
                    //        File.WriteAllLines(currInOutFileName, dataListInputOutput);
                    //    }


                    ////    /*ovo mora ici u posebnu metodu*/
                    ////    //set results interface
                    ////    //if (resultsinterface.iscreatedresultsinterface == false)
                    ////    //{
                    ////    //    onmode.loadfirstafterrunresultsinterface();
                    ////    //}
                    ////    //onmode.setresultsinterface(string.empty, string.empty);
                    ////    //if (onlinefileheader.iscreatedonlineheader == false)
                    ////    //{
                    ////    //    onmode.showinputdata();
                    ////    //}
                    ////    //setresultsinterfaceformanualsetpoint();

                    ////    /*ovo mora ici u posebnu metodu*/


                    ////    //ove ispod dve linije koda su presenjene u metodu iznad { u metodu setResultsInterfaceForManualSetPoint(); }
                    ////    //onMode.ResultsInterface.SetRadioButtons();
                    ////    //onMode.ResultsInterface.SetTextBoxes();
                    ////    chbAVisibility.IsEnabled = true;
                    ////    chbAVisibility.IsChecked = true;


                    ////    //ovde nikako ne treba ponovo pozivati iscrtavanje fitovanog grafika jel pomera i tacku T3 klikom na tacku A 
                    ////    //ne iscrtavati ponovo fitovani grafik kada se pomeraju fitovanog grafika [Rp02 i A]
                    ////    //T3movingDirectionByYAxis = false;
                    ////    //drawFittingGraphic(T3movingDirectionByYAxis);

                    ////    chbAVisibility.IsChecked = true;


                    ////    /************ Ponovno racunanje i iscrtavanje posle promene izduzenja A u offline modu **************/
                    //    //NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                    //    e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                    //    e2e4CalculationAfterManualFitting.CalculateIndexFromChangedParametersFittingUntilA(A);
                    //    e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, xTranslateAmountFittingMode, false);
                    //    setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                    //    setResultsInterfaceForManualSetPoint();

                    //    //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                    //    RmaxwithPoint = e2e4CalculationAfterManualFitting.calculateRmaxWithPoint();
                    //    e2e4CalculationAfterManualFitting.RecalculateChangeOfRFittingPoints(this.Rp02RI, A);
                    //    if (onMode.VXY != null)
                    //    {
                    //        onMode.VXY.DeleteChangeOfRGraphic_Fitting();
                    //        onMode.VXY.PointsChangeOfRFitting = e2e4CalculationAfterManualFitting.PointsChangeOfRFitting;
                    //        onMode.VXY.CreateChangeOfRGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfRFitting);
                    //        //onMode.VXY.createChangeOfRGraphicFitting();
                    //        //onMode.VXY.DeleteChangeOfRGraphic();

                    //        onMode.VXY.DeleteChangeOfEGraphic_Fitting();
                    //        onMode.VXY.PointsChangeOfEFitting = e2e4CalculationAfterManualFitting.PointsChangeOfEFitting;
                    //        onMode.VXY.CreateChangeOfEGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfEFitting, true);
                    //        //onMode.VXY.createChangeOfEGraphicFitting();
                    //        //onMode.VXY.DeleteChangeOfEGraphic();
                    //    }
                    //    /************ Ponovno racunanje i iscrtavanje posle promene izduzenja A u offline modu **************/
                    //    double k = (pointsForFittingGraphic[pointsForFittingGraphic.Count - 2].YAxisValue - pointsForFittingGraphic[pointsForFittingGraphic.Count - 1].YAxisValue) / (pointsForFittingGraphic[pointsForFittingGraphic.Count - 2].XAxisValue - pointsForFittingGraphic[pointsForFittingGraphic.Count - 1].XAxisValue);
                    //    double n = (pointsForFittingGraphic[pointsForFittingGraphic.Count - 2].YAxisValue * pointsForFittingGraphic[pointsForFittingGraphic.Count - 1].XAxisValue - pointsForFittingGraphic[pointsForFittingGraphic.Count - 1].YAxisValue * pointsForFittingGraphic[pointsForFittingGraphic.Count - 2].XAxisValue) / (pointsForFittingGraphic[pointsForFittingGraphic.Count - 1].XAxisValue - pointsForFittingGraphic[pointsForFittingGraphic.Count - 2].XAxisValue);
                    //    pointsForFittingGraphic.Add(new MyPoint(k * AX + n, AX));
                    //    pointsForFittingGraphic.RemoveAt(pointsForFittingGraphic.Count - 1);
                    //    axManual = AX;
                    //    ayManual = mousePosInData.Y;
                    //    double xx = 0;
                    //    double yy = 0;
                    //    List<string> datas;
                    //    string path = tfFilepathPlotting.Text.Split('.').ElementAt(0);
                    //    if (isLuManualChanged == true)
                    //    {
                    //        path += "LuManual.pe";
                    //    }
                    //    else
                    //    {
                    //        path += ".pe";
                    //    }
                    //    List<double> preassure = new List<double>();
                    //    List<double> elongation = new List<double>();
                    //    if (this.OnlineModeInstance.dataReader.PreassureInMPa.Count == 0 || isLuManualChanged == true)
                    //    {
                    //        datas = File.ReadAllLines(path).ToList();
                    //        foreach (var data in datas)
                    //        {
                    //            List<string> pe = data.Split('\t').ToList();
                    //            double pr = 0;
                    //            double el = 0;
                    //            bool isN = double.TryParse(pe[0], out pr);
                    //            if (isN)
                    //            {
                    //                preassure.Add(pr);
                    //            }
                    //            isN = double.TryParse(pe[1], out el);
                    //            if (isN)
                    //            {
                    //                elongation.Add(el);
                    //            }
                    //        }
                    //    }

                    //    //nadji najblizu tacku na zelenom grafiku 
                    //    int indexOfRightA = preassure.Count - 1;
                    //    for (indexOfRightA = preassure.Count - 1; indexOfRightA >= 0; indexOfRightA--)
                    //    {
                    //        if (preassure[indexOfRightA] >= mousePosInData.Y)
                    //        {
                    //            yy = preassure[indexOfRightA];
                    //            xx = elongation[indexOfRightA];
                    //            xx = xx - xTranslateAmountFittingMode;
                    //            break;
                    //        }
                    //    }



                    //    double t1x = 0;
                    //    bool isNN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                    //    double t1y = 0;
                    //    isNN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                    //    double t2x = 0;
                    //    isNN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                    //    double t2y = 0;
                    //    isNN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                    //    currKGlobal = (t2y - t1y) / (t2x - t1x);

                    //    AX = (-1) * (yy - currKGlobal * xx) / currKGlobal;
                    //    AX = Math.Round(AX, 2);
                    //    AManualClickedValue = AX;
                    //    tfA.Text = AManualClickedValue.ToString();
                    //    A = AManualClickedValue;

                       
                    //    drawFittingGraphic(true, 0, true, xx, mousePosInData.Y);
                        
                    //drawFittingGraphic(true, 0, true, mousePosInData.X, mousePosInData.Y);
                    //    //drawFittingGraphic(true, 0, true, xx, yy);

                    //    //pointsForFittingGraphic.Add(new MyPoint(mousePosInData.Y, mousePosInData.X));
                    //    this.Lu = (1 + this.A / 100) * L0;
                    //    onMode.ResultsInterface.tfLu.Text = this.Lu.ToString();
                    //    onMode.ResultsInterface.tfLu.IsReadOnly = true;


                    }//end of A

                    if (isT1Active && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        useManualT1orT2 = true;
                        //u slucaju da je cekirana Promena razmere
                        //odcekiraj je da bi posle mogao da brzo proracunas fitovani grafik
                        //jer ako je cekirana Promena razmere zbog ne postojecih restrikcija razmere grafika spor je odziv prikaza grafika u print modu
                        if (windowForChosingPoints.chbChangeRatio.IsChecked == true)
                        {
                            windowForChosingPoints.chbChangeRatio.IsChecked = false;
                        }



                        OptionsInPlottingMode.pointManualX1 = mousePosInData.X;
                        OptionsInPlottingMode.pointManualY1 = mousePosInData.Y;
                        setPointAtGraphicY1(mousePosInData.Y);
                        OptionsInPlottingMode.pointManualX1 = Math.Round(OptionsInPlottingMode.pointManualX1, 6);
                        tffittingManPoint1X.Text = OptionsInPlottingMode.pointManualX1.ToString();
                        OptionsInPlottingMode.pointManualY1 = Math.Round(OptionsInPlottingMode.pointManualY1, 0);
                        tffittingManPoint1Y.Text = OptionsInPlottingMode.pointManualY1.ToString();



                        //set T1 exacly at the green (original) graphic
                        int indexOfPointWithMinDistanceFromT1 = 0;


                        for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                        {

                            if (OptionsInPlottingMode.isManualFittingChecked)
                            {
                                //druga tacka sa priblizno istim naponom - gleda se napon kliknute tacke(tacke koja moze biti van grafika) i napon jedne od tacke grafika
                                if (dataReader.PreassureInMPa[i] >= OptionsInPlottingMode.pointManualY1)
                                {
                                    indexOfPointWithMinDistanceFromT1 = i;
                                    break;
                                }
                            }
                        }


                        OptionsInPlottingMode.pointManualX1 = dataReader.RelativeElongation[indexOfPointWithMinDistanceFromT1];
                        OptionsInPlottingMode.pointManualY1 = dataReader.PreassureInMPa[indexOfPointWithMinDistanceFromT1];
                        OptionsInPlottingMode.pointManualX1 = Math.Round(OptionsInPlottingMode.pointManualX1, 6);
                        OptionsInPlottingMode.pointManualY1 = Math.Round(OptionsInPlottingMode.pointManualY1, 0);
                        tffittingManPoint1X.Text = OptionsInPlottingMode.pointManualX1.ToString();
                        tffittingManPoint1Y.Text = OptionsInPlottingMode.pointManualY1.ToString();

                        double pfX1 = 0;
                        saveOptionstffittingManPoint1X_Offline(out pfX1);
                        markSavedOnlineOptionsAsBlacktffittingManPoint1X_Offline();


                        double pfY1 = 0;
                        saveOptionstffittingManPoint1Y_Offline(out pfY1);
                        markSavedOnlineOptionsAsBlacktffittingManPoint1Y_Offline();



                        setPointAtGraphicX1(dataReader.RelativeElongation[indexOfPointWithMinDistanceFromT1]);
                        setPointAtGraphicY1(dataReader.PreassureInMPa[indexOfPointWithMinDistanceFromT1]);


                        calculateCurrKandN();
                        drawFittingGraphic();


                    }
                    if (isT2Active && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        useManualT1orT2 = true;
                        //u slucaju da je cekirana Promena razmere
                        //odcekiraj je da bi posle mogao da brzo proracunas fitovani grafik
                        //jer ako je cekirana Promena razmere zbog ne postojecih restrikcija razmere grafika spor je odziv prikaza grafika u print modu
                        if (windowForChosingPoints.chbChangeRatio.IsChecked == true)
                        {
                            windowForChosingPoints.chbChangeRatio.IsChecked = false;
                        }


                        OptionsInPlottingMode.pointManualX2 = mousePosInData.X;
                        OptionsInPlottingMode.pointManualY2 = mousePosInData.Y;
                        setPointAtGraphicY2(mousePosInData.Y);
                        OptionsInPlottingMode.pointManualX2 = Math.Round(OptionsInPlottingMode.pointManualX2, 6);
                        tffittingManPoint2X.Text = OptionsInPlottingMode.pointManualX2.ToString();
                        OptionsInPlottingMode.pointManualY2 = Math.Round(OptionsInPlottingMode.pointManualY2, 0);
                        tffittingManPoint2Y.Text = OptionsInPlottingMode.pointManualY2.ToString();



                        //set T2 exacly at the green (original) graphic
                        int indexOfPointWithMinDistanceFromT2 = 0;


                        for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                        {

                            if (OptionsInPlottingMode.isManualFittingChecked)
                            {
                                //druga tacka sa priblizno istim naponom - gleda se napon kliknute tacke(tacke koja moze biti van grafika) i napon jedne od tacke grafika
                                if (dataReader.PreassureInMPa[i] >= OptionsInPlottingMode.pointManualY2)
                                {
                                    indexOfPointWithMinDistanceFromT2 = i;
                                    break;
                                }
                            }
                        }


                        OptionsInPlottingMode.pointManualX2 = dataReader.RelativeElongation[indexOfPointWithMinDistanceFromT2];
                        OptionsInPlottingMode.pointManualY2 = dataReader.PreassureInMPa[indexOfPointWithMinDistanceFromT2];
                        OptionsInPlottingMode.pointManualX2 = Math.Round(OptionsInPlottingMode.pointManualX2, 6);
                        OptionsInPlottingMode.pointManualY2 = Math.Round(OptionsInPlottingMode.pointManualY2, 0);
                        tffittingManPoint2X.Text = OptionsInPlottingMode.pointManualX2.ToString();
                        tffittingManPoint2Y.Text = OptionsInPlottingMode.pointManualY2.ToString();

                        double pfX2 = 0;
                        saveOptionstffittingManPoint2X_Offline(out pfX2);
                        markSavedOnlineOptionsAsBlacktffittingManPoint2X_Offline();


                        double pfY2 = 0;
                        saveOptionstffittingManPoint2Y_Offline(out pfY2);
                        markSavedOnlineOptionsAsBlacktffittingManPoint2Y_Offline();



                        setPointAtGraphicX2(dataReader.RelativeElongation[indexOfPointWithMinDistanceFromT2]);
                        setPointAtGraphicY2(dataReader.PreassureInMPa[indexOfPointWithMinDistanceFromT2]);


                        //calculateCurrKandN();
                        drawFittingGraphic();

                    }
                    if (isT3Active && OptionsInPlottingMode.isManualFittingChecked)
                    {


                        //u slucaju da je cekirana Promena razmere
                        //odcekiraj je da bi posle mogao da brzo proracunas fitovani grafik
                        //jer ako je cekirana Promena razmere zbog ne postojecih restrikcija razmere grafika spor je odziv prikaza grafika u print modu
                        if (windowForChosingPoints.chbChangeRatio.IsChecked == true)
                        {
                            windowForChosingPoints.chbChangeRatio.IsChecked = false;
                        }

                        OptionsInPlottingMode.pointManualX3 = mousePosInData.X;
                        OptionsInPlottingMode.pointManualY3 = mousePosInData.Y;
                        setPointAtGraphicY3(mousePosInData.Y);
                        OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                        tffittingManPoint3X.Text = OptionsInPlottingMode.pointManualX3.ToString();
                        OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);
                        tffittingManPoint3Y.Text = OptionsInPlottingMode.pointManualY3.ToString();

                        double pfX3 = 0;
                        saveOptionstffittingManPoint3X_Offline(out pfX3);
                        markSavedOnlineOptionsAsBlacktffittingManPoint3X_Offline();
                        //setPointAtGraphicX3(pfX3);

                        double pfY3 = 0;
                        saveOptionstffittingManPoint3Y_Offline(out pfY3);
                        markSavedOnlineOptionsAsBlacktffittingManPoint3Y_Offline();
                        //setPointAtGraphicY3(pfY3);

                        //WindowT3DirectionManualFitting windowT3DirectionManualFitting = new WindowT3DirectionManualFitting();
                        //windowT3DirectionManualFitting.X = (double)mousePos.X;
                        //windowT3DirectionManualFitting.Y = (double)mousePos.Y;
                        //windowT3DirectionManualFitting.setWindowForChosingPoints();
                        //windowT3DirectionManualFitting.GraphicPlotting = this;
                        //if (IsWindowForMovingT3DirectionShown == false)
                        //{
                        //    IsWindowForMovingT3DirectionShown = true;
                        //    windowT3DirectionManualFitting.ShowDialog();

                        //}

                        //calculateCurrKandN();
                        T3movingDirectionByYAxis = true;
                        drawFittingGraphic(T3movingDirectionByYAxis);

                        OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                        tffittingManPoint3X.Text = OptionsInPlottingMode.pointManualX3.ToString();
                        OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);
                        tffittingManPoint3Y.Text = OptionsInPlottingMode.pointManualY3.ToString();

                        saveOptionstffittingManPoint3X_Offline(out pfX3);
                        markSavedOnlineOptionsAsBlacktffittingManPoint3X_Offline();
                        saveOptionstffittingManPoint3Y_Offline(out pfY3);
                        markSavedOnlineOptionsAsBlacktffittingManPoint3Y_Offline();
                    }//end of T3 active point (manual mode)

                    if (isR2R4Active && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        double x = mousePosInData.X;
                        double y = mousePosInData.Y;
                        double newx = x;//mada newx gotovo da se ne razlikuje od originalnog x-a; ali po y osi moze biti velika razlika

                        //NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                        e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);

                        if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                        {
                            //set R2R4 exacly at the green (original) graphic
                            //for (int i = 0; i < dataReader.RelativeElongation.Count; i++)
                            //{

                            if (OptionsInPlottingMode.isManualFittingChecked)
                            {
                                //if (dataReader.RelativeElongation[i] >= x)
                                //{
                                //dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 0);
                                //setPointAtGraphicR2R4(dataReader.RelativeElongation[i], dataReader.PreassureInMPa[i]);
                                setPointAtGraphicR2R4(x, y);
                                OptionsInOnlineMode.E2E4Border = x;
                                OptionsInOnlineMode.E2E4Border = Math.Round(OptionsInOnlineMode.E2E4Border, 2);
                                onMode.OptionsOnline.tfE2E4Border.Text = OptionsInOnlineMode.E2E4Border.ToString();
                                onMode.OptionsOnline.WriteXMLOnlineFile();
                                lblR2R3R4Border.Text = "R2/R4";
                                tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E2E4Border.ToString();
                                //newx = dataReader.RelativeElongation[i];
                                //break;
                                //}
                            }
                            //}


                            //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                            e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E2E4Border, xTranslateAmountFittingMode, true);
                            setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                            setResultsInterfaceForManualSetPoint();

                        }
                        else
                        {

                            if (OptionsInPlottingMode.isManualFittingChecked)
                            {
                                setPointAtGraphicR2R4(x, y);
                                OptionsInOnlineMode.E3E4Border = x;
                                OptionsInOnlineMode.E3E4Border = Math.Round(OptionsInOnlineMode.E3E4Border, 2);
                                onMode.OptionsOnline.tfE3E4Border.Text = OptionsInOnlineMode.E3E4Border.ToString();
                                onMode.OptionsOnline.WriteXMLOnlineFile();
                                lblR2R3R4Border.Text = "R3/R4";
                                tfR2R4ManualOfflineMode.Text = OptionsInOnlineMode.E3E4Border.ToString();
                            }


                            //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);

                            e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, xTranslateAmountFittingMode, false);
                            setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                            setResultsInterfaceForManualSetPoint();

                        }

                    }//end of R2/R4 active point (manual mode)


                    if (isOriginalActive && OptionsInPlottingMode.isManualFittingChecked && windowForChosingPoints.chbOriginal.IsEnabled == true)
                    {
                        //numberOfClickOnOriginalRadioButton++;



                        //if (numberOfClickOnOriginalRadioButton % 2 == 1)
                        if (windowForChosingPoints.chbOriginal.IsChecked == false)
                        {

                            deleteOfflineModeOnly();

                            drawFittingGraphic();

                            //if (isLuManualChanged == true)
                            //{
                            //    onMode.ResultsInterface.AfterEnterClicked_Lu();
                            //}
                            _MarkerGraph.DataSource = null;
                            //_MarkerGraphText.DataSource = null;
                            _MarkerGraph2.DataSource = null;
                            //_MarkerGraphText2.DataSource = null;
                            _MarkerGraph3.DataSource = null;
                            //_MarkerGraphText3.DataSource = null;

                            optionsPlotting.EvenOriginalClick();


                        }
                        else
                        {
                            optionsPlotting.OddOriginalClick();
                            btnPlottingModeClick();
                            drawFittingGraphic();
                        }

                    }


                    if (isChangeRatio && OptionsInPlottingMode.isManualFittingChecked)
                    {
                        if (windowForChosingPoints.chbChangeRatio.IsChecked == true)
                        {
                            plotter.FitToView();
                            plotter.Viewport.Restrictions.Clear();

                            printscreen.plotterPrint.FitToView();
                            printscreen.plotterPrint.Viewport.Restrictions.Clear();
                        }
                        else
                        {
                            // ako je razmera postavljena unapred postavi restrikciju na ose grafika
                            plotter.Viewport.AutoFitToView = true;
                            ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                            restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                            restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);
                            plotter.Viewport.Restrictions.Add(restr);


                            printscreen.plotterPrint.Viewport.AutoFitToView = true;
                            ViewportAxesRangeRestriction restrPrint = new ViewportAxesRangeRestriction();
                            restrPrint.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                            restrPrint.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);

                            printscreen.plotterPrint.Viewport.Restrictions.Add(restrPrint);
                        }
                    }


                }
                else
                {
                    // MessageBox.Show("Mod miša je isključen!");
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void plotter_MouseDoubleClick(object sender, MouseButtonEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        private void deleteOnlyFittingLine() 
        {
            try
            {

                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteOnlyFittingLine()}", System.DateTime.Now);
            }
        }

        private void deleteOnlyFittingLinePrintScreen()
        {

            try
            {
                var numberOfOffline = printscreen.plotterPrint.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "grafik").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = printscreen.plotterPrint.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "grafik").Single();
                    printscreen.plotterPrint.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteOnlyFittingLinePrintScreen()}", System.DateTime.Now);
            }
        }

        private void deleteFittingLineMode() 
        {
            try
            {
                var numberOfOfflineGray = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Count();
                if (numberOfOfflineGray > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }


                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Fitting Line").Single();
                    plotter.Children.Remove(lineToRemove);

                }
                isGraphicFittingLoaded = false;

                _MarkerGraph.DataSource = null;
                //_MarkerGraphText.DataSource = null;
                _MarkerGraph2.DataSource = null;
                //_MarkerGraphText2.DataSource = null;
                _MarkerGraph3.DataSource = null;
                //_MarkerGraphText3.DataSource = null;
                _MarkerGraph4.DataSource = null;
                //_MarkerGraphText4.DataSource = null;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteFittingLineMode()}", System.DateTime.Now);
            }
        }

        public void drawFittingGraphic(string filePath, int numberOfCallForDrawFitting = 0)
        {
            try
            {
                OptionsInPlottingMode.filePath = filePath;
                tfFilepathPlotting.Text = filePath;
                dataReader = new DataReader(OptionsInPlottingMode.filePath);
                dataReader.ReadData();
                drawFittingGraphic(true, numberOfCallForDrawFitting);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void drawFittingGraphic(string filePath, int numberOfCallForDrawFitting = 0)}", System.DateTime.Now);
            }
        }



        public void DrawFittingGraphic(bool T3movingDirectionByYAxis = true, int numberOfCallForDrawFitting = 0, bool isAClicked = false, double AClickedXValue = 0, double AClickedYValue = 0)
        {
            try
            {
                drawFittingGraphic(T3movingDirectionByYAxis, numberOfCallForDrawFitting, isAClicked, AClickedXValue, AClickedYValue);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DrawFittingGraphic(bool T3movingDirectionByYAxis = true, int numberOfCallForDrawFitting = 0, bool isAClicked = false, double AClickedXValue = 0, double AClickedYValue = 0)}", System.DateTime.Now);
            }
        }

        public void TranslateReHRelAndRmtoFittingGraphic(int indexClosestToRed, double xTranslateAmount)
        {
            try
            {
                translateReHRelAndRmtoFittingGraphic(indexClosestToRed, xTranslateAmount);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void TranslateReHRelAndRmtoFittingGraphic(int indexClosestToRed, double xTranslateAmount)}", System.DateTime.Now);
            }
        }

        private void translateReHRelAndRmtoFittingGraphic(int indexClosestToRed, double xTranslateAmount) 
        {
            try
            {
                _MarkerGraph6.DataSource = null;//Rm
                //_MarkerGraphText6.DataSource = null;
                chbRmVisibility.IsChecked = true;
                //chbRmVisibility.IsEnabled = false;
                _MarkerGraph4.DataSource = null;//ReH
                //_MarkerGraphText4.DataSource = null;
                //chbReHVisibility.IsChecked = true;
                //chbReHVisibility.IsEnabled = false;
                _MarkerGraph5.DataSource = null;//ReL
                //_MarkerGraphText5.DataSource = null;
                //chbReLVisibility.IsChecked = true;
                //chbReLVisibility.IsEnabled = false;

                if (indexClosestToRed < indexReHFromoriginalData)
                {
                    if (isFoundMaxAndMin == false)//ako nije pronadjen maksimum nema ni ReH
                    {
                        //return;
                    }
                    else
                    {
                        setReHPoint(ReHXFromoriginalData - xTranslateAmount, ReH);
                        _reH_X = ReHXFromoriginalData - xTranslateAmount;
                        //chbReHVisibility.IsChecked = true;
                        chbReHVisibility.IsEnabled = true;
                    }
                }
                if (indexClosestToRed < indexReLFromoriginalData)
                {
                    if (isFoundMaxAndMin == false && isFoundMinFalseAndMin == false && isFoundOnlyReLCase == false)//ako nije ispunjen nijedan uslov za postojanje ReL nema sta da se translira na fitovani grafik
                    {
                        //return;
                    }
                    else
                    {
                        if (isFoundOnlyReLCase == true)
                        {
                            setReLPoint(mkrTriangleCurrentValues.ReLXValue, mkrTriangleCurrentValues.ReLYValue);
                            printscreen.setReLPoint_Manual(mkrTriangleCurrentValues.ReLXValue, mkrTriangleCurrentValues.ReLYValue);
                        }
                        else
                        {
                            setReLPoint(ReLXFromoriginalData - xTranslateAmount, ReL);
                            printscreen.setReLPoint_Manual(ReLXFromoriginalData - xTranslateAmount, ReL);
                        }
                        _reL_X = ReLXFromoriginalData - xTranslateAmount;
                        //chbReLVisibility.IsChecked = true;
                        chbReLVisibility.IsEnabled = true;
                    }
                }
                if (indexClosestToRed < indexRmFromoriginalData)
                {
                    setRmPoint(RmXFromoriginalData - xTranslateAmount, Rm);
                    _rm_X = RmXFromoriginalData - xTranslateAmount;
                    chbRmVisibility.IsChecked = true;
                    chbRmVisibility.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void translateReHRelAndRmtoFittingGraphic(int indexClosestToRed, double xTranslateAmount)}", System.DateTime.Now);
            }
        }

        private double checkCurrKForInfinity(double xTranslateAmountNonInfinity) 
        {
            try
            {
                double xTranslateAmount = Double.MinValue;
                //ako je ipak k beskonacno
                if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                {
                    OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                    xTranslateAmount = OptionsInPlottingMode.pointManualX3;
                    return xTranslateAmount;
                }
                //ako je ipak k beskonacno
                else
                {
                    xTranslateAmountNonInfinity = Math.Round(xTranslateAmountNonInfinity, 6);
                    return xTranslateAmountNonInfinity;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double checkCurrKForInfinity(double xTranslateAmountNonInfinity)}", System.DateTime.Now);
                return 0;
            }
        }

        private void calcuateOnlyFittingParameters_Rp02_AfterLuChanged(MyPointCollection fittingPoints) 
        {
            try
            {
                //ako se ne korisit ekstenziometar
                if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("False"))
                {
                    double k_AfterLuChanged = fittingPoints[1].YAxisValue / fittingPoints[1].XAxisValue;
                    double n_AfterLuChanged = -0.2 * k_AfterLuChanged;
                    int indexRp02_AfterLuChanged = 0;
                    double min_AfterLuChanged = Double.MaxValue;
                    for (int i = 0; i < fittingPoints.Count; i++)
                    {
                        if (fittingPoints[i].XAxisValue > OptionsInPlottingMode.ReHXRange || fittingPoints[i].YAxisValue - (k_AfterLuChanged * fittingPoints[i].XAxisValue + n_AfterLuChanged) < 0)
                        {
                            break;
                        }
                        if (fittingPoints[i].YAxisValue - (k_AfterLuChanged * fittingPoints[i].XAxisValue + n_AfterLuChanged) < min_AfterLuChanged)
                        {
                            min_AfterLuChanged = fittingPoints[i].YAxisValue - (k_AfterLuChanged * fittingPoints[i].XAxisValue + n_AfterLuChanged);
                            indexRp02_AfterLuChanged = i;
                        }
                    }
                    _rp02_X = fittingPoints[indexRp02_AfterLuChanged].XAxisValue;
                    Rp02RI = fittingPoints[indexRp02_AfterLuChanged].YAxisValue;
                    printscreen.Rp02X = _rp02_X;
                    printscreen.Rp02Y = Rp02RI;

                    //postavi vidljiva tekstualna polja i promeni zadnje unet podatak tj strukturu LastInputOutputSavedData jel se na osnovu nje kreira izvestaj
                    Rp02RI = Math.Round(Rp02RI, 0);
                    onMode.ResultsInterface.tfRp02.Text = Rp02RI.ToString();
                    tfRp02.Text = Rp02RI.ToString();
                    LastInputOutputSavedData.tfRp02_ResultsInterface = Rp02RI.ToString();
                }
                else
                {
                    int indexRp02_AfterLuChanged2 = 0;
                    foreach (var point in fittingPoints)
                    {
                        indexRp02_AfterLuChanged2++;
                        if (point.XAxisValue >= 0.2)
                        {
                            Rp02RI = point.YAxisValue;
                            break;
                        }
                    }


                    _rp02_X = fittingPoints[indexRp02_AfterLuChanged2].XAxisValue;
                    Rp02RI = fittingPoints[indexRp02_AfterLuChanged2].YAxisValue;
                    printscreen.Rp02X = _rp02_X;
                    printscreen.Rp02Y = Rp02RI;

                    //postavi vidljiva tekstualna polja i promeni zadnje unet podatak tj strukturu LastInputOutputSavedData jel se na osnovu nje kreira izvestaj
                    Rp02RI = Math.Round(Rp02RI, 0);
                    tfRp02.Text = Rp02RI.ToString();
                    onMode.ResultsInterface.tfRp02.Text = Rp02RI.ToString();
                    tfRp02.Text = Rp02RI.ToString();
                    LastInputOutputSavedData.tfRp02_ResultsInterface = Rp02RI.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calcuateOnlyFittingParameters_Rp02_AfterLuChanged(MyPointCollection fittingPoints)}", System.DateTime.Now);
            }
        }

        public void DrawFittingGraphic_AfterLuChanged(MyPointCollection newFittingPoints, double ratioNewLuLastLu)
        {
            try
            {
                
                drawFittingGraphic_AfterLuChanged(newFittingPoints, ratioNewLuLastLu);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void DrawFittingGraphic_AfterLuChanged(MyPointCollection newFittingPoints, double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }

      

        /// <summary>
        /// ovde se iscrtava fitovani grafik samo kada se u izlaznom prozoru rucno promeni vrednost Lu-a
        /// </summary>
        /// <param name="newFittingPoints"></param>
        /// <param name="ratioNewALastA"></param>
        private void drawFittingGraphic_AfterLuChanged(MyPointCollection newFittingPoints, double ratioNewALastA) 
        {

            try
            {


                isLuManualChanged = true;
                printscreen.chbCalculateNManual.IsChecked = false;

                //i ponisti da je bilo sta radjeno u rucnom modu da bi posle programskog klika na dugme UCITAJ FAJL mogao na kraju drawFitting metode da dobro postavi tacke T1 i T2
                isT1Active = false;
                isT2Active = false;
                isT3Active = false;


                if (onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    OptionsInOnlineMode.mmCoeff = OptionsInOnlineMode.mmCoeff * ratioNewALastA;
                    OptionsInPlottingMode.mmCoeff = OptionsInPlottingMode.mmCoeff * ratioNewALastA;
                    OptionsInOnlineMode.mmCoeff = Math.Round(OptionsInOnlineMode.mmCoeff,3);
                    OptionsInPlottingMode.mmCoeff = Math.Round(OptionsInPlottingMode.mmCoeff,3);
                   


                    onMode.OptionsOnline.WriteXMLOnlineFile();
                    this.WriteXMLFileOffline();
                    onMode.createOnlineGraphics();
                }

                if (onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                {
                    OptionsInOnlineMode.mmCoeffWithEkstenziometer = OptionsInOnlineMode.mmCoeffWithEkstenziometer * ratioNewALastA;
                    OptionsInPlottingMode.mmCoeffWithEkstenziometer = OptionsInPlottingMode.mmCoeffWithEkstenziometer * ratioNewALastA;
                    OptionsInOnlineMode.mmCoeffWithEkstenziometer = Math.Round(OptionsInOnlineMode.mmCoeffWithEkstenziometer,3);
                    OptionsInPlottingMode.mmCoeffWithEkstenziometer = Math.Round(OptionsInPlottingMode.mmCoeffWithEkstenziometer,3);
                 


                    onMode.OptionsOnline.WriteXMLOnlineFile();
                    this.WriteXMLFileOffline();
                    onMode.createOnlineGraphics();
                }


                btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                //btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                //vrati koeficijente na staro
                if (onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    OptionsInOnlineMode.mmCoeff = OptionsInOnlineMode.mmCoeff / ratioNewALastA;
                    OptionsInPlottingMode.mmCoeff = OptionsInPlottingMode.mmCoeff / ratioNewALastA;
                    OptionsInOnlineMode.mmCoeff = Math.Round(OptionsInOnlineMode.mmCoeff, 3);
                    OptionsInPlottingMode.mmCoeff = Math.Round(OptionsInPlottingMode.mmCoeff, 3);
                    onMode.OptionsOnline.tfCalElonMultiple.Text = OptionsInOnlineMode.mmCoeff.ToString();
                    this.OptionsPlotting.tfCalElonMultiple.Text = OptionsInPlottingMode.mmCoeff.ToString();


                    onMode.OptionsOnline.WriteXMLOnlineFile();
                    this.WriteXMLFileOffline();
                    onMode.createOnlineGraphics();
                }

                if (onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                {
                    OptionsInOnlineMode.mmCoeffWithEkstenziometer = OptionsInOnlineMode.mmCoeffWithEkstenziometer / ratioNewALastA;
                    OptionsInPlottingMode.mmCoeffWithEkstenziometer = OptionsInPlottingMode.mmCoeffWithEkstenziometer / ratioNewALastA;
                    OptionsInOnlineMode.mmCoeffWithEkstenziometer = Math.Round(OptionsInOnlineMode.mmCoeffWithEkstenziometer, 3);
                    OptionsInPlottingMode.mmCoeffWithEkstenziometer = Math.Round(OptionsInPlottingMode.mmCoeffWithEkstenziometer, 3);
                    onMode.OptionsOnline.tfCalElonMultiple.Text = OptionsInOnlineMode.mmCoeffWithEkstenziometer.ToString();
                    this.OptionsPlotting.tfCalElonMultiple2.Text = OptionsInPlottingMode.mmCoeffWithEkstenziometer.ToString();


                    onMode.OptionsOnline.WriteXMLOnlineFile();
                    this.WriteXMLFileOffline();
                    onMode.createOnlineGraphics();
                }


                setT1T2T3FromTextboxes();

                //isLuManualChanged = true;
                //IDontWantShowT1T2T3AtPrintScreen = true;

                //double NForFittingLine = Double.MinValue;
                //deleteOnlyFittingLine();
                //deleteOnlyFittingLinePrintScreen();
                ////calculateCurrKandN();//pre nego sto pocnes sa iscrtavanjem fitovanog grafika proracunaj parametre linije na osnovu koje radis fitovanje
                ////dataReader.ClearFittingData();

                //double xTranslateAmount;



                //MyPointCollection fittingPoints;
                //MyPoint pointOfTearing;


                ////double currSubstraction;
                ////double minsub = Double.MaxValue;
                ////double currSubDistance;
                ////double minsubDistance = Double.MaxValue;
                ////int indexOfPointClosestToRed = 0;


                //int maxPoints = 300000;
                //fittingPoints = new MyPointCollection(maxPoints);
                //fittingPoints.Clear();

                //fittingPoints = newFittingPoints;
                //pointsOfFittingLine = fittingPoints;

                //var ds = new EnumerableDataSource<MyPoint>(fittingPoints);
                //ds.SetXMapping(x => x.XAxisValue);
                //ds.SetYMapping(y => y.YAxisValue);

                ////deleteFittingLineMode();

                //plotter.AddLineGraph(ds, Colors.Blue, 2, "Fitting Line"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                //isGraphicFittingLoaded = true;
                ////printscreen.plotterPrint.AddLineGraph(ds, Colors.Blue, 2, "grafik");
                //printscreen.UpdatePrintScreen(fittingPoints);


                ////racunanje ponovo automatskog i rucnog N-a
                //double s0Off = dataReader.S0Offline;
                //double l0Off = dataReader.L0Offline;
                //int begIndex = 0, endIndex = dataReader.FittingRelativeElongation.Count;
                //for (int ii = 0; ii < dataReader.FittingRelativeElongation.Count; ii++)
                //{
                //    if (dataReader.FittingRelativeElongation[ii] > OptionsInPlottingMode.BeginIntervalForN)
                //    {
                //        begIndex = ii;
                //        break;
                //    }
                //}
                //for (int ii = 0; ii < dataReader.FittingRelativeElongation.Count; ii++)
                //{
                //    double endintervalForN = OptionsInPlottingMode.EndIntervalForN;
                //    if (ag < OptionsInPlottingMode.EndIntervalForN)
                //    {
                //        //endintervalForN = ag;
                //        endintervalForN = OptionsInPlottingMode.BeginIntervalForN;
                //    }
                //    if (dataReader.FittingRelativeElongation[ii] > endintervalForN)
                //    {
                //        endIndex = ii;
                //        break;
                //    }
                //}
                //if (PreassureForNManualProperty != null)
                //{
                //    PreassureForNManualProperty.Clear();
                //}
                //List<double> Fs_Fitting = new List<double>();
                //List<double> Fs_FittingForManualN = new List<double>();
                //List<double> deltaLsInProc = new List<double>();
                //List<double> deltaLsInProcForManualN = new List<double>();
                //Fs_Fitting = getFs_Fitting(s0Off, begIndex, endIndex, ref deltaLsInProc);
                //Fs_FittingForManualN = getFs_FittingForManualN(s0Off, begIndex, endIndex, ref deltaLsInProcForManualN, xTranslateAmountFittingMode);
                //Fs_FittingForManualNProperty = Fs_FittingForManualN;
                //DeltaLsInProcForManualNProperty = deltaLsInProcForManualN;

                //double mE = findME(fittingPoints);

                //nHardeningExponent = new NHardeningExponent(Fs_Fitting, deltaLsInProc, s0Off, l0Off, A, mE);

                //double nHardeningExponent_Value = 0;
                //nHardeningExponent_Value = nHardeningExponent.Get_N();
                //LastInputOutputSavedData.tfn_ResultsInterface = nHardeningExponent.Get_N().ToString();
                //nHardeningExponent_Value = Math.Round(nHardeningExponent_Value, 4);
                //this.onMode.ResultsInterface.tfn.Text = nHardeningExponent_Value.ToString();
                //if (printscreen.chbCalculateNManual.IsChecked == true)
                //{
                //    printscreen.NManualCalculation.Close();
                //    printscreen.chbCalculateNManual.IsChecked = false;
                //    printscreen.DeleteOnlyNManualMarkers();
                //}


                //NHardeningExponentManual = new NHardeningExponent(Fs_FittingForManualNProperty, DeltaLsInProcForManualNProperty, s0Off, l0Off, A, mE);
                //NManual = NHardeningExponentManual.Get_N();





                //if (OptionsInPlottingMode.isManualChecked)
                //{

                //    plotter.Viewport.AutoFitToView = true;
                //    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                //    restr.YRange = new DisplayRange(-0.5, OptionsInPlottingMode.yRange);
                //    restr.XRange = new DisplayRange(-0.5, OptionsInPlottingMode.xRange);


                //    plotter.Viewport.Restrictions.Add(restr);

                //}

                //if (OptionsInPlottingMode.isAutoChecked)
                //{

                //    plotter.FitToView();
                //    plotter.Viewport.Restrictions.Clear();
                //}



                ////posto posle promene Lu-a ne zelimo da menjamo koeficijent pravca(tj da se zadrzi isti kao pri pocetnom proracunu)
                ////i da tek od tacke Rp02 pocnemo po X-osi da sirimo i skupljamo grafik ne racunamo ponovo Rp02 vec ga ostavljamo kakav je bio i od njega pocinjemo da skuplamo/sirimo grafik
                ////_rp02_X = _rp02_X * ratioNewLuLastLu;
                //_rp02_X = Rp02RIXValue;
                ////calcuateOnlyFittingParameters_Rp02_AfterLuChanged(fittingPoints);

                //if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                //{
                //    rt05 = findRt05_AfterLuChanged(fittingPoints);
                //    rt05 = Math.Round(rt05,0);
                //    onMode.ResultsInterface.tfRt05.Text = rt05.ToString();
                //}
                

                //if (chbRp02Visibility.IsChecked == true && chbRp02Visibility.IsEnabled == true)
                //{
                //    setRp02Point(_rp02_X, Rp02RI);
                //}
                //else
                //{
                //    mkrTriangleCurrentValues.Rp02XValue = _rp02_X;
                //    mkrTriangleCurrentValues.Rp02YValue = Rp02RI;
                //}
                //_reL_X = _reL_X * ratioNewALastA;
                //printscreen.ReLX = _reL_X;
                //if (chbReLVisibility.IsChecked == true && chbReLVisibility.IsEnabled == true)
                //{
                //    setReLPoint(_reL_X, ReL);
                //    printscreen.setReLPoint_Manual(_reL_X, ReL);
                //}
                //else
                //{
                //    mkrTriangleCurrentValues.ReLXValue = _reL_X;
                //    mkrTriangleCurrentValues.ReLYValue = ReL;
                //}
                //_reH_X = _reH_X * ratioNewALastA;
                //printscreen.ReHX = _reH_X;
                //if (chbReHVisibility.IsChecked == true && chbReHVisibility.IsEnabled == true)
                //{
                //    setReHPoint(_reH_X, ReH);
                //}
                //else
                //{
                //    mkrTriangleCurrentValues.ReHXValue = _reH_X;
                //    mkrTriangleCurrentValues.ReHYValue = ReH;
                //}
                //_rm_X = _rm_X * ratioNewALastA;
                //if (chbRmVisibility.IsChecked == true && chbRmVisibility.IsEnabled == true)
                //{
                //    setRmPoint(_rm_X, Rm);
                //}
                //else
                //{
                //    mkrTriangleCurrentValues.RmXValue = _rm_X;
                //    mkrTriangleCurrentValues.RmYValue = Rm;
                //}
                //_a_X = _a_X * ratioNewALastA;
                //if (chbAVisibility.IsChecked == true && chbAVisibility.IsEnabled == true)
                //{
                //    setAPoint(_a_X);
                //}
                //else
                //{
                //    mkrTriangleCurrentValues.AXValue = _a_X;
                //}


                ////from here set not resultsInterface textboxes ,here you set in plotting window 
                //mkrTriangleCurrentValues.AXValue = Math.Round(mkrTriangleCurrentValues.AXValue, 1);
                //tfA.Text = mkrTriangleCurrentValues.AXValue.ToString();

                ////here set resultsInterface textboxes
                //mkrTriangleCurrentValues.AXValue = Math.Round(mkrTriangleCurrentValues.AXValue, 1);
                //onMode.ResultsInterface.tfA.Text = mkrTriangleCurrentValues.AXValue.ToString();

               
                //_ag_X = _ag_X * ratioNewALastA;
                //_ag_X = Math.Round(_ag_X, 1);
                //onMode.ResultsInterface.tfAg.Text = _ag_X.ToString();
     

                //_agt_X = _agt_X * ratioNewALastA;
                //_agt_X = Math.Round(_agt_X, 1);
                //onMode.ResultsInterface.tfAgt.Text = _agt_X.ToString();

                //_at_X = _at_X * ratioNewALastA;
                //_at_X = Math.Round(_at_X, 1);
                //onMode.ResultsInterface.tfAt.Text = _at_X.ToString();

                ////NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                //RmaxwithPoint = e2e4CalculationAfterManualFitting.calculateRmaxWithPoint();
                //if (onMode.VXY != null)
                //{
                //    onMode.VXY.PointsChangeOfRFitting = e2e4CalculationAfterManualFitting.PointsChangeOfRFitting;
                //    onMode.VXY.CreateChangeOfRGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfRFitting);
                //    onMode.VXY.DeleteChangeOfRGraphic();
                //    onMode.VXY.PointsChangeOfEFitting = e2e4CalculationAfterManualFitting.PointsChangeOfEFitting;
                //    onMode.VXY.CreateChangeOfEGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfEFitting);
                //    onMode.VXY.DeleteChangeOfEGraphic();
                //}
                //if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                //{
                //    e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E2E4Border, xTranslateAmountFittingMode, true);
                //    e2e4CalculationAfterManualFitting.MultipleArrayOfEAndRChange(ratioNewALastA);
                //    setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                //}
                //else
                //{
                //    e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, xTranslateAmountFittingMode, false);
                //    e2e4CalculationAfterManualFitting.MultipleArrayOfEAndRChange(ratioNewALastA);
                //    setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                //}

                //onMode.ResultsInterface.tfE2.Text = /*e2MinValue + " - " +  e2MaxValue.ToString()*/ e2AvgValue.ToString();
                //onMode.ResultsInterface.tfE4.Text = /*e4MinValue + " - " +  e4MaxValue.ToString()*/ e4AvgValue.ToString();


                ////set printscreen markers
                //if (LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True"))
                //{
                //    printscreen.setRp02Point_Manual(_rp02_X, Rp02RI);
                //}
                //if (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True") && LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                //{
                //    printscreen.SetRt05Point_Lu(rt05);
                //}
                //if (LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True"))
                //{
                //    printscreen.setReLPoint_Manual(_reL_X, ReL);
                //}
                //if (LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True"))
                //{
                //    printscreen.setReHPoint_Manual(_reH_X, ReH);
                //}
                //printscreen.setRmPoint_Manual(_rm_X, Rm);

                //printscreen.setAPoint_Manual(_a_X);
                //printscreen.setAgPoint_Manual(_ag_X);


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "drawFittingGraphic_AfterLuChanged()");
                //Logger.WriteNode(ex.Message.ToString() , System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void drawFittingGraphic_AfterLuChanged(MyPointCollection newFittingPoints, double ratioNewALastA)}", System.DateTime.Now);
            }
        }

        private string getCurrentInputOutputFile()
        {
            try
            {
                //if (onMode.IsStoppedOnlineSample == true)
                //{
                //    onMode.IsStoppedOnlineSample = false;
                //    return Constants.inputOutputFilepath;
                //}

                // Get file name.
                string name = OptionsInPlottingMode.filePath;

                //GetAutomaticAnimation file name
                string nameInputOutput = name.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";

                return nameInputOutput;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private string getCurrentInputOutputFile()}", System.DateTime.Now);
                return string.Empty;
            }
        }

        public void ReCalculateAutoAndManualN() 
        {
            try
            {
                double s0Off = dataReader.S0Offline;
                double l0Off = dataReader.L0Offline;

                List<double> Fs_Fitting = new List<double>();
                List<double> Fs_FittingForManualN = new List<double>();
                int begIndex = 0, endIndex = dataReader.FittingRelativeElongation.Count;
                for (int ii = 0; ii < dataReader.FittingRelativeElongation.Count; ii++)
                {
                    if (dataReader.FittingRelativeElongation[ii] > OptionsInPlottingMode.BeginIntervalForN)
                    {
                        begIndex = ii;
                        break;
                    }
                }

                for (int ii = 0; ii < dataReader.FittingRelativeElongation.Count; ii++)
                {
                    double endintervalForN = OptionsInPlottingMode.EndIntervalForN;
                    if (ag < OptionsInPlottingMode.EndIntervalForN)
                    {
                        //endintervalForN = ag;
                        endintervalForN = OptionsInPlottingMode.BeginIntervalForN;
                    }
                    if (dataReader.FittingRelativeElongation[ii] > endintervalForN)
                    {
                        endIndex = ii;
                        break;
                    }
                }





                if (PreassureForNManualProperty != null)
                {
                    PreassureForNManualProperty.Clear();
                }
                List<double> deltaLsInProc = new List<double>();
                List<double> deltaLsInProcForManualN = new List<double>();
                Fs_Fitting = getFs_Fitting(s0Off, begIndex, endIndex, ref deltaLsInProc);
                Fs_FittingForManualN = getFs_FittingForManualN(s0Off, begIndex, endIndex, ref deltaLsInProcForManualN, xTranslateAmountFittingMode);
                Fs_FittingForManualNProperty = Fs_FittingForManualN;
                DeltaLsInProcForManualNProperty = deltaLsInProcForManualN;

                double mE = findME(pointsForFittingGraphic);

                nHardeningExponent = new NHardeningExponent(Fs_Fitting, deltaLsInProc, s0Off, l0Off, A, mE);

                //double nHardeningExponent_Value = 0;
                //nHardeningExponent_Value = nHardeningExponent.Get_N();
                LastInputOutputSavedData.tfn_ResultsInterface = nHardeningExponent.Get_N().ToString();



                NHardeningExponentManual = new NHardeningExponent(Fs_FittingForManualNProperty, DeltaLsInProcForManualNProperty, s0Off, l0Off, A, mE);
                NManual = NHardeningExponentManual.Get_N();

                double nAuto = Math.Round(nHardeningExponent.N, 4);
                onMode.ResultsInterface.tfn.Text = nAuto.ToString();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void ReCalculateAutoAndManualN()}", System.DateTime.Now);
            }
            
        }

        private double Rp005(MyPointCollection fittingPoints, int indexOfPointClosestToRed, double xTranslateAmount, out double Rp005XValue, out int Rp005index, double reEqualsRp)
        {
            try
            {
                Rp005index = 0;
                Rp005XValue = 0;
                int indexOfRp005 = 0;
                double xSubstractionForRp005 = Double.MaxValue;
                if (OptionsInPlottingMode.isManualFittingChecked)
                {
                    xSubstractionForRp005 = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                }
                else
                {
                    xSubstractionForRp005 = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                }


                if (xSubstractionForRp005 < 0.01 || xSubstractionForRp005 == 0.01)
                {

                    double minXSubs = Double.MaxValue;
                    double xSubs = -1;

                    for (int i = 0; i < fittingPoints.Count - 1; i++)
                    {
                        if (fittingPoints[i].XAxisValue > 1) break;
                        //xSubs = Math.Abs(0.05 - fittingPoints[i].XAxisValue);
                        xSubs = Math.Abs(reEqualsRp - fittingPoints[i].XAxisValue);
                        if (xSubs < minXSubs)
                        {
                            minXSubs = xSubs;
                            indexOfRp005 = i;
                        }
                    }
                    Rp005XValue = fittingPoints[indexOfRp005].XAxisValue;
                    return fittingPoints[indexOfRp005].YAxisValue;
                }
                else
                {

                    double Rp005K = currK;
                    //double Rp005N = -currK * 0.05;
                    double Rp005N = -currK * reEqualsRp;

                    //ako je k ipak beskonacno
                    if (Double.IsPositiveInfinity(Rp005K) || Double.IsNegativeInfinity(Rp005K))
                    {
                        double minXSubs = Double.MaxValue;
                        double xSubs = -1;

                        for (int i = 0; i < fittingPoints.Count - 1; i++)
                        {
                            if (fittingPoints[i].XAxisValue > 1) break;
                            //xSubs = Math.Abs(0.05 - fittingPoints[i].XAxisValue);
                            xSubs = Math.Abs(reEqualsRp - fittingPoints[i].XAxisValue);
                            if (xSubs < minXSubs)
                            {
                                minXSubs = xSubs;
                                indexOfRp005 = i;
                            }
                        }
                        Rp005XValue = fittingPoints[indexOfRp005].XAxisValue;
                        return fittingPoints[indexOfRp005].YAxisValue;
                    }
                    //ako je k ipak beskonacno

                    double yIdeal = Rp005K * 0 + Rp005N;

                    double minYSubs = Double.MaxValue;
                    double ySubs = -1;
                    //int indexOfRp005 = -1;
                    for (int i = 0; i < fittingPoints.Count - 1; i++)
                    {
                        yIdeal = Rp005K * fittingPoints[i].XAxisValue + Rp005N;
                        ySubs = Math.Abs(yIdeal - fittingPoints[i].YAxisValue);
                        //if (ySubs < minYSubs && fittingPoints[i].XAxisValue >= 0.05 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount /*&& fittingPoints[i].XAxisValue < 0.5*/)
                        if (ySubs < minYSubs && fittingPoints[i].XAxisValue >= reEqualsRp + OptionsInPlottingMode.pointManualX3 - xTranslateAmount /*&& fittingPoints[i].XAxisValue < 0.5*/)
                        {
                            minYSubs = ySubs;
                            indexOfRp005 = i;
                        }
                    }

                    int indexFromGreenOriginalDataBegin = Int32.MinValue;

                    if (indexOfRp005 > 2)
                    {
                        indexFromGreenOriginalDataBegin = (indexOfRp005 - 2) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    }
                    else
                    {
                        indexFromGreenOriginalDataBegin = (indexOfRp005) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    }
                    int indexFromGreenOriginalDataEnd = (indexOfRp005 + 2) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    List<double> fittingRelativeElongation = new List<double>();
                    List<double> fittingPreasssureInMPa = new List<double>();//from original data


                    for (int i = indexFromGreenOriginalDataBegin; i < indexFromGreenOriginalDataEnd; i++)
                    {
                        fittingRelativeElongation.Add(dataReader.RelativeElongation[i] - xTranslateAmount);
                        fittingPreasssureInMPa.Add(dataReader.PreassureInMPa[i]);
                    }

                    //most precise calculate Rp005
                    int indexOfRp005Precise = 0;
                    for (int i = 0; i < fittingRelativeElongation.Count; i++)
                    {
                        yIdeal = Rp005K * fittingRelativeElongation[i] + Rp005N;
                        ySubs = Math.Abs(yIdeal - fittingPreasssureInMPa[i]);
                        //if (ySubs < minYSubs && fittingPoints[i].XAxisValue >= 0.05 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount)
                        if (ySubs < minYSubs && fittingPoints[i].XAxisValue >= reEqualsRp + OptionsInPlottingMode.pointManualX3 - xTranslateAmount)
                        {
                            minYSubs = ySubs;
                            indexOfRp005Precise = i;
                            indexOfRp005 = indexOfRp005Precise;
                        }
                    }

                    //return fittingPoints[indexOfRp005].YAxisValue;
                    //if (fittingPoints[indexOfRp005Precise].XAxisValue < 0.05 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount)
                    //{
                    //    Rp005XValue = 0.05 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;
                    //}
                    //else
                    //{
                    //    Rp005XValue = fittingPoints[indexOfRp005Precise].XAxisValue;
                    //}

                    //Rp005XValue = 0.05 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;//probaj sa ovom fiksnom vrednoscu za X osu
                    Rp005XValue = reEqualsRp + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;//probaj sa ovom fiksnom vrednoscu za X osu

                    return fittingPreasssureInMPa[indexOfRp005Precise];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double Rp005(MyPointCollection fittingPoints, int indexOfPointClosestToRed, double xTranslateAmount, out double Rp005XValue, out int Rp005index, double reEqualsRp)}", System.DateTime.Now);
                Rp005XValue = 0;
                Rp005index = 0;
                return 0;
            }
        }

        public double RecalculateYungsModuo(bool modifiedRe = false, bool withtraverse = false)
        {
            try
            {
                if (modifiedRe == true)
                {
                    drawFittingGraphic();
                }

                //Re = (100 - OptionsInPlottingMode.YungPrSpustanja) / 100 * border005;
                //double localRe = (100 - OptionsInPlottingMode.YungPrSpustanja) / 100 * originalRe;
                double localRe = (100 - OptionsInPlottingMode.YungPrSpustanja) / 100 * Border005Global;
                double E = recalculateYungsModuo2(Border005Global, localRe, withtraverse);
                Re = localRe;
                Re = Math.Round(Re, 0);
                E = Math.Round(E, 0);

                onMode.ResultsInterface.tfRe.Text = Re.ToString();
                onMode.ResultsInterface.tfE.Text = E.ToString();


                return E;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public double RecalculateYungsModuo(bool modifiedRe = false, bool withtraverse = false)}", System.DateTime.Now);
                return 0;
            }
        }

        private double originalRe = 0;

        private double recalculateYungsModuo2(double border005, double localRe, bool withtraverse = false)
        {
            try
            {
                //double Rex = -1;
                //re = (100 - OptionsInPlottingMode.YungPrSpustanja)/100 * border005;
                //re = (100 - OptionsInPlottingMode.YungPrSpustanja)/100 * Re;
                double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                /*int indexOfPointFirstPointgreaterThenRe = 0;
                for (int i = 0; i < dataReader.FittingPreassureInMPa.Count; i++)
                {
                    if (dataReader.FittingPreassureInMPa[i] > re)
                    {
                        indexOfPointFirstPointgreaterThenRe = i;
                        break;
                    }
                }

                int xdistance = 1;
                x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - 1];
                x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                while (x1 == x2)
                {
                    x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - xdistance];
                    x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                    xdistance++;
                }
                x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - xdistance];
                x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                y1 = dataReader.FittingPreassureInMPa[indexOfPointFirstPointgreaterThenRe - xdistance];
                y2 = dataReader.FittingPreassureInMPa[indexOfPointFirstPointgreaterThenRe];
                //x1 = x1 - xTranslateAmountFittingMode;
                //x2 = x2 - xTranslateAmountFittingMode;

                //y-oni su u MPa tj 10^6 podeljeno sa procentima tj 10^(-2)
                //kada se podeli dobije se 10^8
                //ovako se vise ne racuna
                y1 = 0;
                x1 = 0;
                //yungsModuo = (y2 - y1) / (x2 - x1) * 100;
                yungsModuo = Re / (0.01 * x2);
                /*double l0 = 0.0;
                bool isN = Double.TryParse(LastInputOutputSavedData.tfL0, out l0);
                yungsModuo = y2 * l0 / (OptionsInPlottingMode.YungXelas / 100 * l0);*/
                //yungsModuo = yungsModuo / 10;//convert to GPa
                //zaokruzi na hednu decimalu GN/m^2 ili 10^9N/m^2
                double k = 0;
                double t1y = 0, t1x = 0, t2x = 0, t2y = 0, t3x = 0, t3y = 0;
                bool isN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                isN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                isN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                isN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                isN = double.TryParse(tffittingManPoint3X.Text, out t3x);
                isN = double.TryParse(tffittingManPoint3Y.Text, out t3y);
                if (t1x == 0)
                {
                    t1y = 0;
                }
                if (t1x == 0 && t2x == 0)
                {
                    t2x = 0.1;
                    tffittingManPoint2X.Text = t2x.ToString();
                    tffittingManPoint2X.Foreground = Brushes.Black;
                }
                k = (t2y - t1y) / (t2x - t1x);
                currK = k;
                //yungsModuo = (dataReader.FittingPreassureInMPa[1] - dataReader.FittingPreassureInMPa[0]) / (dataReader.FittingRelativeElongation[1] - dataReader.FittingRelativeElongation[0]) * 100;
                if (withtraverse == false)
                {
                    y1 = k * 0.02;
                    x1 = 0.02;
                    double rp02 = 0;
                    isN = double.TryParse(tfRp02.Text, out rp02);
                    y2 = rp02;
                    x2 = y2 / k;
                    y1 = Math.Round(y1, 4);
                    x1 = Math.Round(x1, 4);
                    y2 = Math.Round(y2, 4);
                    x2 = Math.Round(x2, 4);
                    yungsModuo = (y2 - y1) / (x2 - x1) * 100;
                }
                else
                {
                    //y2 = rp02;
                    //y2 = Border005Global;
                    y2 = localRe;
                    //x2 = y2 / k;
                    x2 = OptionsInPlottingMode.ReEqualsRp;
                    y2 = Math.Round(y2, 4);
                    x2 = Math.Round(x2, 4);
                    //double xRp02ForYung = Math.Abs(_rp02XValue - x2);
                    //xRp02ForYung = Math.Round(xRp02ForYung, 4);
                    //yungsModuo = rp02 / xRp02ForYung * 100;
                    yungsModuo = y2 / x2 * 100;
                }

                if (onMode.ResultsInterface != null)
                {
                    if (yungsModuo == -1 || yungsModuo.IsNaN() == true)
                    {
                        if (onMode.ResultsInterface.ratioNewALastA != 0)
                        {
                            yungsModuo = yungsModuo * (1 / onMode.ResultsInterface.ratioNewALastA);
                        }
                    }
                    else
                    {
                        if (onMode.ResultsInterface.ratioNewALastA != 0 || yungsModuo.IsNaN() == true)
                        {
                            yungsModuo = this.YungsModuo * (1 / onMode.ResultsInterface.ratioNewALastA);
                        }
                    }
                }

                yungsModuo = Math.Round(yungsModuo, 0);
                this.YungsModuo = yungsModuo;

                re = Math.Round(localRe, 0);
                LastInputOutputSavedData.Re_ResultsInterface = re.ToString();
                LastInputOutputSavedData.E_ResultsInterface = yungsModuo.ToString();
                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfE.Text = this.YungsModuo.ToString();
                }


                //double preassureForT2 = 0.9 * localRe;
                //double autoProcent2 = preassureForT2 / rm * 100;
                //autoProcent2 = Math.Round(autoProcent2, 0);
                //OptionsInPlottingMode.procentAuto2 = autoProcent2;
                //OptionsInPlottingMode.procentAuto3 = OptionsInPlottingMode.procentAuto2 + 5;
                //double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto2 / 100;

                //int index = this.GetClosestPointIndex(criteriaPreassure);
                //OptionsInPlottingMode.pointAutoX2 = this.DataReader.RelativeElongation[index];
                //OptionsInPlottingMode.pointAutoY2 = this.DataReader.PreassureInMPa[index];


                this.WriteXMLFileOffline();

                return yungsModuo;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double recalculateYungsModuo2(double border005, double localRe, bool withtraverse = false)}", System.DateTime.Now);
                return 0;
            }
        }

        private double recalculateYungsModuo(double border005, double localRe)
        {
            try
            {
                double Rex = -1;
                //re = (100 - OptionsInPlottingMode.YungPrSpustanja)/100 * border005;
                //re = (100 - OptionsInPlottingMode.YungPrSpustanja) / 100 * Re;

                double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                int indexOfPointFirstPointgreaterThenRe = 0;
                for (int i = 0; i < dataReader.FittingPreassureInMPa.Count; i++)
                {
                    if (dataReader.FittingPreassureInMPa[i] > localRe)
                    {
                        indexOfPointFirstPointgreaterThenRe = i;
                        break;
                    }
                }
                indexOfPointFirstPointgreaterThenRe = indexOfPointFirstPointgreaterThenRe - 1;
                if (indexOfPointFirstPointgreaterThenRe - 1 < 0)
                {
                    re = 0;
                    re = Math.Round(re, 0);
                    return 0;
                }
                int xdistance = 1;
                x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - 1];
                x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                while (x1 == x2)
                {
                    x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - xdistance];
                    x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                    xdistance++;
                }
                x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - xdistance];
                x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                y1 = dataReader.FittingPreassureInMPa[indexOfPointFirstPointgreaterThenRe - xdistance];
                y2 = dataReader.FittingPreassureInMPa[indexOfPointFirstPointgreaterThenRe];
                //x1 = x1 - xTranslateAmountFittingMode;
                //x2 = x2 - xTranslateAmountFittingMode;

                //y-oni su u MPa tj 10^6 podeljeno sa procentima tj 10^(-2)
                //kada se podeli dobije se 10^8
                //ovako se vise ne racuna
                y1 = 0;
                x1 = 0;
                ///yungsModuo = (y2 - y1) / (x2 - x1) * 100;
                yungsModuo = localRe / (0.01 * x2);
                /*double l0 = 0.0;
                bool isN = Double.TryParse(LastInputOutputSavedData.tfL0, out l0);
                yungsModuo = y2 * l0 / (OptionsInPlottingMode.YungXelas / 100 * l0);*/
                //yungsModuo = yungsModuo / 10;//convert to GPa
                //zaokruzi na hednu decimalu GN/m^2 ili 10^9N/m^2
                yungsModuo = Math.Round(yungsModuo, 0);

                re = Math.Round(re, 0);
                LastInputOutputSavedData.Re_ResultsInterface = re.ToString();
                LastInputOutputSavedData.E_ResultsInterface = yungsModuo.ToString();



                return yungsModuo;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double recalculateYungsModuo(double border005, double localRe)}", System.DateTime.Now);
                return 0;
            }
        }


        private double calculateYungsModuo(double border005 = 0.005, bool withtraverse = false)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (window.IWantLoadYungFromFile == true)
                {
                    window.IWantLoadYungFromFile = false;
                    YungFirstTimeCalculated = this.YungsModuo;
                    return this.YungsModuo;
                }


                //proveri da li se koristio ekstenziometar pa ako jeste postavi vrednost promenljive withtraverse
                if (onMode != null && onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                {
                    withtraverse = false;
                }
                else if (onMode != null && onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    withtraverse = true;
                }


                //double Rex = -1;
                //re = (100 - OptionsInPlottingMode.YungPrSpustanja)/100 * border005;
                //re = (100 - OptionsInPlottingMode.YungPrSpustanja)/100 * Re;
                double x1 = 0, x2 = 0, y1 = 0, y2 = 0;
                /*int indexOfPointFirstPointgreaterThenRe = 0;
                for (int i = 0; i < dataReader.FittingPreassureInMPa.Count; i++)
                {
                    if (dataReader.FittingPreassureInMPa[i] > re)
                    {
                        indexOfPointFirstPointgreaterThenRe = i;
                        break;
                    }
                }

                int xdistance = 1;
                x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - 1];
                x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                while (x1 == x2)
                {
                    x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - xdistance];
                    x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                    xdistance++;
                }
                x1 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe - xdistance];
                x2 = dataReader.FittingRelativeElongation[indexOfPointFirstPointgreaterThenRe];
                y1 = dataReader.FittingPreassureInMPa[indexOfPointFirstPointgreaterThenRe - xdistance];
                y2 = dataReader.FittingPreassureInMPa[indexOfPointFirstPointgreaterThenRe];
                //x1 = x1 - xTranslateAmountFittingMode;
                //x2 = x2 - xTranslateAmountFittingMode;

                //y-oni su u MPa tj 10^6 podeljeno sa procentima tj 10^(-2)
                //kada se podeli dobije se 10^8
                //ovako se vise ne racuna
                y1 = 0;
                x1 = 0;
                //yungsModuo = (y2 - y1) / (x2 - x1) * 100;
                yungsModuo = Re / (0.01 * x2);
                /*double l0 = 0.0;
                bool isN = Double.TryParse(LastInputOutputSavedData.tfL0, out l0);
                yungsModuo = y2 * l0 / (OptionsInPlottingMode.YungXelas / 100 * l0);*/
                //yungsModuo = yungsModuo / 10;//convert to GPa
                //zaokruzi na hednu decimalu GN/m^2 ili 10^9N/m^2
                double k = 0;
                double t1y = 0, t1x = 0, t2x = 0, t2y = 0, t3x = 0, t3y = 0;
                bool isN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                isN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                isN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                isN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                isN = double.TryParse(tffittingManPoint3X.Text, out t3x);
                isN = double.TryParse(tffittingManPoint3Y.Text, out t3y);

                if (t1x == 0)
                {
                    t1y = 0;
                }
                if (t1x == 0 && t2x == 0)
                {
                    t2x = 0.1;
                    tffittingManPoint2X.Text = t2x.ToString();
                    tffittingManPoint2X.Foreground = Brushes.Black;
                }
                //k = (t2y - t1y)/(t2x - t1x);
                if (isT1Active == true || isT2Active == true)
                {
                    k = (OptionsInPlottingMode.pointManualY2 - OptionsInPlottingMode.pointManualY1) / (OptionsInPlottingMode.pointManualX2 - OptionsInPlottingMode.pointManualX1);
                }
                else
                {
                    k = (OptionsInPlottingMode.pointAutoY2 - OptionsInPlottingMode.pointAutoY1) / (OptionsInPlottingMode.pointAutoX2 - OptionsInPlottingMode.pointAutoX1);
                }
                currK = k;
                //yungsModuo = (dataReader.FittingPreassureInMPa[1] - dataReader.FittingPreassureInMPa[0]) / (dataReader.FittingRelativeElongation[1] - dataReader.FittingRelativeElongation[0]) * 100;
                if (withtraverse == false)
                {
                    y1 = k * 0.02;
                    x1 = 0.02;
                    double rp02 = 0;
                    isN = double.TryParse(tfRp02.Text, out rp02);
                    y2 = rp02;
                    x2 = y2 / k;
                    y1 = Math.Round(y1, 4);
                    y2 = Math.Round(y2, 4);
                    x1 = Math.Round(x1, 4);
                    x2 = Math.Round(x2, 4);
                    yungsModuo = (y2 - y1) / (x2 - x1) * 100;

                    //i kada je ukljucen ekstenziometar racunaj Re
                    re = (100 - OptionsInPlottingMode.YungPrSpustanja) * Rp02RI / 100;
                    re = Math.Round(re, 0);
                    originalRe = re;
                    y2 = re;
                    Re = re;
                }
                else
                {
                    //y2 = rp02;
                    re = (100 - OptionsInPlottingMode.YungPrSpustanja) * Rp02RI / 100;
                    re = Math.Round(re, 0);
                    originalRe = re;
                    y2 = re;
                    Re = re;
                    //x2 = y2 / k;
                    x2 = OptionsInPlottingMode.ReEqualsRp;
                    y2 = Math.Round(y2, 4);
                    Border005Global = Math.Round(Border005Global, 4);
                    x2 = Math.Round(x2, 4);
                    //double xRp02ForYung = Math.Abs(_rp02XValue - x2);
                    //xRp02ForYung = Math.Round(xRp02ForYung, 4);
                    //yungsModuo = rp02 / xRp02ForYung * 100;
                    yungsModuo = y2 / x2 * 100;
                }

                if (onMode.ResultsInterface != null)
                {
                    if (yungsModuo == -1 || yungsModuo.IsNaN() == true)
                    {
                        if (onMode.ResultsInterface.ratioNewALastA != 0)
                        {
                            yungsModuo = yungsModuo * (1 / onMode.ResultsInterface.ratioNewALastA);
                        }
                    }
                    else
                    {
                        if (onMode.ResultsInterface.ratioNewALastA != 0 || yungsModuo.IsNaN() == true)
                        {
                            yungsModuo = this.YungsModuo * (1 / onMode.ResultsInterface.ratioNewALastA);
                        }
                    }
                }

                yungsModuo = Math.Round(yungsModuo, 0);
                this.YungsModuo = yungsModuo;

                LastInputOutputSavedData.Re_ResultsInterface = re.ToString();
                LastInputOutputSavedData.E_ResultsInterface = yungsModuo.ToString();
                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.tfE.Text = this.YungsModuo.ToString();
                    onMode.ResultsInterface.tfRe.Text = this.Re.ToString();
                }


                //double preassureForT2 = 0.9 * re;
                //double autoProcent2 = preassureForT2 / rm * 100;
                //autoProcent2 = Math.Round(autoProcent2, 0);
                //OptionsInPlottingMode.procentAuto2 = autoProcent2;
                //OptionsInPlottingMode.procentAuto3 = OptionsInPlottingMode.procentAuto2 + 5;
                //double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto2 / 100;

                //int index = this.GetClosestPointIndex(criteriaPreassure);
                //OptionsInPlottingMode.pointAutoX2 = this.DataReader.RelativeElongation[index];
                //OptionsInPlottingMode.pointAutoY2 = this.DataReader.PreassureInMPa[index];




                this.WriteXMLFileOffline();

                if (IsYungFirstTimeCalculate == true)
                {
                    YungFirstTimeCalculated = yungsModuo;
                }

                if (IWantToBackFirstCalculateYung == true)
                {
                    IWantToBackFirstCalculateYung = false;
                    YungsModuo = this.YungFirstTimeCalculated;
                    this.WriteXMLFileOffline();
                    YungsModuo = this.YungFirstTimeCalculated;
                    onMode.ResultsInterface.tfE.Text = this.YungFirstTimeCalculated.ToString();
                    return this.YungFirstTimeCalculated;
                }

                return yungsModuo;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double calculateYungsModuo(double border005 = 0.005, bool withtraverse = false)}", System.DateTime.Now);
                return 0;
            }
        }

        MyPointCollection fittingPoints_RecalculateYungsModuo;
        int indexOfPointClosestToRed_RecalculateYungsModuo;

        private void setResultsInterfaceIfALessThanzeropointtwo() 
        {
            try
            {
                tfRp02.Foreground = Brushes.Beige;
                tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                setRp02Point(OptionsInPlottingMode.xRange, OptionsInPlottingMode.yRange);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setResultsInterfaceIfALessThanzeropointtwo()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ispravi pravu liniju kako treba da ne bi se stvarala prava horizontalna linija koja ne postoji ali se greskom javlja
        /// </summary>
        private void correctFittingLine(ref MyPointCollection fittingPoints)
        {
            try
            {
                if (fittingPoints.Count <= 2)
                {
                    return;
                }
                double xCorrection = fittingPoints[2].XAxisValue - fittingPoints[1].XAxisValue;

                for (int i = 2; i < fittingPoints.Count; i++)
                {
                    fittingPoints[i].XAxisValue = fittingPoints[i].XAxisValue - xCorrection;
                }

                for (int i = 2; i < dataReader.FittingRelativeElongation.Count; i++)
                {
                    dataReader.FittingRelativeElongation[i] = dataReader.FittingRelativeElongation[i] - xCorrection;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void correctFittingLine(ref MyPointCollection fittingPoints)}", System.DateTime.Now);
            }
        }

        private void setResultInterfaceElongationLessThanOne()
        {
 
        }

        private int indexEnd;

        private void drawFittingGraphic(bool T3movingDirectionByYAxis = true, int numberOfCallForDrawFitting = 0, bool isAClicked = false, double AClickedXValue = 0, double AClickedYValue = 0) 
        {
            try
            {

                double t1x = 0;
                double t1y = 0;
                double t2x = 0;
                double t2y = 0;
                double t3x = 0;
                double t3y = 0;

                if (OptionsInOnlineMode.isAutoChecked == true && numberOfCallForDrawFitting % 3 != 0)
                {
                    return;
                }
                

                if (OptionsInOnlineMode.isManualChecked == true)
                {
                    if ((numberOfCallForDrawFitting == 0 || numberOfCallForDrawFitting == 1) && OptionsInOnlineMode.isAutoChecked == true)
                    {
                        return;
                    }
                }

                double NForFittingLine  = Double.MinValue;
                deleteOnlyFittingLine();
                deleteOnlyFittingLinePrintScreen();
                calculateCurrKandN();//pre nego sto pocnes sa iscrtavanjem fitovanog grafika proracunaj parametre linije na osnovu koje radis fitovanje
                dataReader.ClearFittingData();
                
                double xTranslateAmount;
                //ovo se odnosi samo kada se ucitava prethodno zapamcen fajl
                bool foundIsCircleOrRectangle = false;
               

                


                //double currSubstraction;
                //double minsub = Double.MaxValue;
                double currSubDistance;
                double minsubDistance = Double.MaxValue;
                int indexOfPointClosestToRed = 0;

                MyPointCollection fittingPoints;
                MyPoint pointOfTearing = new MyPoint(0, 0);


                int maxPoints = 300000;
                fittingPoints = new MyPointCollection(maxPoints);
                pointsForFittingGraphic = fittingPoints;
                fittingPoints.Clear();

                var ds = new EnumerableDataSource<MyPoint>(fittingPoints);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);

                //deleteFittingLineMode();
     

                plotter.AddLineGraph(ds, Colors.Blue, 2, "Fitting Line"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                isGraphicFittingLoaded = true;
                //printscreen.plotterPrint.AddLineGraph(ds, Colors.Blue, 2, "grafik");
                if (numberOfCallForDrawFitting % 3 == 0)
                {
                    printscreen.UpdatePrintScreen(fittingPoints);                
                }
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
               
                
           
    
                            

                if (OptionsInPlottingMode.isManualChecked)
                {

                    plotter.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInPlottingMode.yRange);
                    restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInPlottingMode.xRange);


                    plotter.Viewport.Restrictions.Add(restr);

                }

                if (OptionsInPlottingMode.isAutoChecked)
                {

                    plotter.FitToView();
                    plotter.Viewport.Restrictions.Clear();
                }


                int i;
                /******   odredjivanje mesta sta se uzima za crvenu tacku kada se obelezi da je crvena tacka(T3) van zelenog grafika VAZNA STVAR     ********/
                for (i = 0; i < dataReader.PreassureInMPa.Count; i++)
                {
                    //yOfLine = currK * dataReader.RelativeElongation[i] + currN;
                    //currSubDistance = Math.Abs(yOfLine - dataReader.PreassureInMPa[i]);
                    //ovo je vazilo do 10 oktobra 2014 godine
                    //if (OptionsInPlottingMode.isManualFittingChecked)
                    //{
                    //   if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.pointManualX3)
                    //    {
                    //        indexOfPointClosestToRed = i;
                    //        break;
                    //    }
                    //}
                    //else
                    //{
                    //    if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.pointAutoX3)
                    //    {
                    //        indexOfPointClosestToRed = i;
                    //        break;
                    //    }
                    //}

                    if (OptionsInPlottingMode.isManualFittingChecked)
                    {
                        if (T3movingDirectionByYAxis)
                        {
                            if (dataReader.PreassureInMPa[i] >= OptionsInPlottingMode.pointManualY3)
                            {
                                indexOfPointClosestToRed = i;
                                OptionsInPlottingMode.pointManualX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                                OptionsInPlottingMode.pointManualY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                                t3x = OptionsInPlottingMode.pointManualX3;
                                t3y = OptionsInPlottingMode.pointManualY3;
                                break;
                            }
                        }
                        if (T3movingDirectionByYAxis == false)
                        {
                            if (dataReader.RelativeElongation[i] >= OptionsInPlottingMode.pointManualX3)
                            {
                                indexOfPointClosestToRed = i;
                                OptionsInPlottingMode.pointManualX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                                OptionsInPlottingMode.pointManualY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                                t3x = OptionsInPlottingMode.pointManualX3;
                                t3y = OptionsInPlottingMode.pointManualY3;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (dataReader.PreassureInMPa[i] >= OptionsInPlottingMode.pointAutoY3)
                        {
                            indexOfPointClosestToRed = i;
                            OptionsInPlottingMode.pointAutoX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                            OptionsInPlottingMode.pointAutoY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                            t3x = OptionsInPlottingMode.pointAutoX3;
                            t3y = OptionsInPlottingMode.pointAutoY3;
                            //mozda ove dve linije koda treba obrisati
                            OptionsInPlottingMode.pointManualX3 = dataReader.RelativeElongation[indexOfPointClosestToRed];
                            OptionsInPlottingMode.pointManualY3 = dataReader.PreassureInMPa[indexOfPointClosestToRed];
                            t3x = OptionsInPlottingMode.pointManualX3;
                            t3y = OptionsInPlottingMode.pointManualY3;
                            break;
                        }
                    }
                }

                //postavi na grafiku tacku najblizu oznacenoj za T3
                setPointAtGraphicX3(dataReader.RelativeElongation[indexOfPointClosestToRed]);
                setPointAtGraphicY3(dataReader.PreassureInMPa[indexOfPointClosestToRed]);

                //setManualPointsToAutoPointsValue();

                if (OptionsInPlottingMode.isManualFittingChecked)
                {
                    double currNAnother = Double.MaxValue;//ovo je n koje prolazi kroz crvenu tacku
                    currK = Math.Round(currK, 6);
                    OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3,6);
                    OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3,0);
                    currNAnother = OptionsInPlottingMode.pointManualY3 - currK * OptionsInPlottingMode.pointManualX3;
                    t3x = OptionsInPlottingMode.pointManualX3;
                    t3y = OptionsInPlottingMode.pointManualY3;

                    double xSubstractionForFitting = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                    t1x = OptionsInPlottingMode.pointManualX1;
                    t2x = OptionsInPlottingMode.pointManualX2;

                    if (xSubstractionForFitting < 0.01 || xSubstractionForFitting == 0.01) //ovde je koeficijent pravca beskonacan (prava je paralelna sa Y-osom)
                    {
                        xTranslateAmount = OptionsInPlottingMode.pointManualX3;
                        t3x = OptionsInPlottingMode.pointManualX3;
                    }
                    else
                    {
                        currNAnother = Math.Round(currNAnother,6);
                        currK = Math.Round(currK, 6);
                        xTranslateAmount = (-1) * (currNAnother / currK);
                        xTranslateAmount = Math.Round(xTranslateAmount, 6);
                        xTranslateAmount = checkCurrKForInfinity(xTranslateAmount);

                    }

                }
                else
                {
                    double currNAnother = Double.MaxValue;//ovo je n koje prolazi kroz crvenu tacku
                    OptionsInPlottingMode.pointManualX3 = Math.Round(OptionsInPlottingMode.pointManualX3, 6);
                    OptionsInPlottingMode.pointManualY3 = Math.Round(OptionsInPlottingMode.pointManualY3, 0);
                    currNAnother = OptionsInPlottingMode.pointAutoY3 - currK * OptionsInPlottingMode.pointAutoX3;
                    t3y = OptionsInPlottingMode.pointAutoY3;
                    t3x = OptionsInPlottingMode.pointAutoX3;
                    NForFittingLine = currNAnother;

                    double xSubstractionForFitting = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                    t1x = OptionsInPlottingMode.pointAutoX1;
                    t2x = OptionsInPlottingMode.pointAutoX2;

                    if (xSubstractionForFitting < 0.01 || xSubstractionForFitting == 0.01)
                    {
                        xTranslateAmount = OptionsInPlottingMode.pointAutoX3;
                        t3x = OptionsInPlottingMode.pointAutoX3;
                    }
                    else
                    {
                        currNAnother = Math.Round(currNAnother, 6);
                        currK = Math.Round(currK, 6);
                        xTranslateAmount = (-1) * (currNAnother / currK);
                        xTranslateAmount = checkCurrKForInfinity(xTranslateAmount);
                    }
                }
               

                xTranslateAmountFittingMode = xTranslateAmount;
                //ova linija pravi problem
                //onMode.VXY.FittingArrayElongationOfEndOfInterval(onMode.IndexFromChangedParametersFitting, OptionsInOnlineMode.E2E4Border, XTranslateAmountFittingMode);

                fittingPoints.Add(new MyPoint(0, 0));
                dataReader.FittingRelativeElongation.Add(0);//dodaj informaciju o relativnom izduzenju fitovanog grafika
                dataReader.FittingPreassureInMPa.Add(0);//dodaj informaciju o naponu fitovanog grafika
                double translatedXValue = dataReader.RelativeElongation[indexOfPointClosestToRed] - xTranslateAmount;             
                fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[indexOfPointClosestToRed], translatedXValue));
                dataReader.FittingRelativeElongation.Add(translatedXValue);//dodaj informaciju o relativnom izduzenju fitovanog grafika
                dataReader.FittingPreassureInMPa.Add(dataReader.PreassureInMPa[indexOfPointClosestToRed]);//dodaj informaciju o naponu fitovanog grafika

                double A = 0;
                //set relative elongation A


                this.indexEnd = dataReader.RelativeElongation.Count;
                calculateTotalRelElong(out pointOfTearing, out A);
                
                
                if (isAClicked == true)
                {
                    A = axManual;
                    AManualClickedValue = axManual;
                }






                for (i = indexOfPointClosestToRed; i < this.indexEnd/*dataReader.RelativeElongation.Count*/; i++)
                {
                    //od tacke kidanja ne prikazuj vise fitovani grafik
                    if ((dataReader.RelativeElongation[i]-xTranslateAmount) > pointOfTearing.XAxisValue)
                    {
                        break;
                    }

                    double translatedXValueFromOriginal = dataReader.RelativeElongation[i] - xTranslateAmount;
                    dataReader.FittingRelativeElongation.Add(translatedXValueFromOriginal);//dodaj informaciju o relativnom izduzenju fitovanog grafika
                    dataReader.FittingPreassureInMPa.Add(dataReader.PreassureInMPa[i]);//dodaj informaciju o naponu fitovanog grafika

                    if (i % (OptionsInPlottingMode.Resolution) == 0)
                    {
                        if (isAClicked == true && translatedXValueFromOriginal > AClickedXValue)
                        {
                            fittingPoints.Add(new MyPoint(AClickedYValue, AClickedXValue));
                            AManualClickedValue = AClickedXValue;
                            break;
                        }

                        //if (translatedXValueFromOriginal > xMarkers4[0] && dataReader.PreassureInMPa[i] < 0.1 * this.Rm)
                        //{
                            fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[i], translatedXValueFromOriginal));
                        //}

                        //only for testing purpose. THIS MUST BE DELETED OR COMMENTED
                        //if (dataReader.PreassureInMPa[i] < 191 && dataReader.RelativeElongation[i] > 30)
                        //{
                        //    double translatedXValueFromOriginalTest = 36;
                        //    //test with 1 point
                        //    fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[i], translatedXValueFromOriginalTest));
                        //}
                        //else
                        //{
                        //    double translatedXValueFromOriginal = dataReader.RelativeElongation[i] - xTranslateAmount;
                        //    fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[i], translatedXValueFromOriginal));
                        //}
                        //only for testing purpose. THIS MUST BE DELETED OR COMMENTED
                    }
                }


                //ispeglaj izmedju zadnje tacke fitovanog grafika i tacke kidanja tako sto ces iscrtavati svaku iz tekstualnog fajla pocevsi od poslednje iscrtane pa sve do tacke kidanja
                //if (i - 2 * OptionsInPlottingMode.Resolution > 0)
                //{
                //    for (int idetails = i - 2 * OptionsInPlottingMode.Resolution; idetails < dataReader.RelativeElongation.Count; idetails++)
                //    {
                //        //od tacke kidanja ne prikazuj vise fitovani grafik
                //        if ((dataReader.RelativeElongation[idetails] - xTranslateAmount) > pointOfTearing.XAxisValue)
                //        {
                //            break;
                //        }

                //        double translatedXValueFromOriginal = dataReader.RelativeElongation[idetails] - xTranslateAmount;
                //        fittingPoints.Add(new MyPoint(dataReader.PreassureInMPa[idetails], translatedXValueFromOriginal));
                        

                //    }
                //}

                if (chbTurnOnFittingCorrection.IsChecked == true)
                {
                    correctFittingLine(ref fittingPoints);
                }

                //add finally tearing point
                if (isAClicked == true)
                {
                    fittingPoints.Add(new MyPoint(AClickedYValue, AClickedXValue));
                    pointOfTearing.XAxisValue = AClickedXValue;
                    pointOfTearing.YAxisValue = AClickedYValue;
                    A = pointOfTearing.XAxisValue;
                    AManualClickedValue = A;
                }
                else
                {
                    //if (fittingPoints[fittingPoints.Count - 1].YAxisValue > 0.1 * this.Rm && pointOfTearing.YAxisValue < OptionsInPlottingMode.DefaultPreassureOfTearingInProcent / 100 * this.Rm)
                    if (chbTurnOnFittingCorrection.IsChecked == true)
                    {
                        
                    }
                    else
                    {
                        fittingPoints.Add(pointOfTearing);
                    }
                }

                int counter = 0;
                if (chbTurnOnFittingCorrection.IsChecked == true)
                {                    
                    foreach (var point in fittingPoints)
                    {
                        if (point.XAxisValue != null)
                        {
                            counter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    A = fittingPoints[counter - 1].XAxisValue;
                    A = Math.Round(A, 1);
                }

                if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                {
                    deleteOfflineModeOnly();
                    deleteFittingPath();//ali ne i fitovanu liniju (liniju plave boje)
                }

                pointsOfFittingLine = fittingPoints;

                
                //set graphic representation of relative elongation A (red triangle marker)
                if (pointsOfFittingLine != null)
                {
                    setAPoint(A);
                }
               


                // da bi se azurirao print screen koji se koristi za iscrtavanje grafika u pojedniacnom izvestaju
                if (isRationChanged == true)
                {
                    //ako si promeni razmeru ne prelazi na print screen (tab3) i registruj da za promenu ramere nisi presao na tab3 postavljanjem promenljive isRationChanged na false 
                    isRationChanged = false;
                    return;
                }
                else
                {
                    //ako je doslo do klika na crveno dugme (onMode.IsStoppedOnlineSample == true) ili do kraja ispisa (window.IsOnlineModeFinished == true) ne prikazuj u ovoj fazi prozore ili je iskljuceno automatsko prebacivanje u print mod
                    if (onMode.IsStoppedOnlineSample == true || window.IsOnlineModeFinished == true || chbAutomaticGoToPrintMode.IsChecked == false)
                    {
                    }
                    else
                    {
                        if (OptionsInOnlineMode.isManualChecked)
                        {
                            window.tab_third.IsSelected = true;
                        }
                    }
                }

                fittingPoints_RecalculateYungsModuo = fittingPoints;
                indexOfPointClosestToRed_RecalculateYungsModuo = indexOfPointClosestToRed;
             

                //odavde pocinje racunanje parametara koji se upisuju u izlazni prozor (zuti prozor)
                int indexOfRp02 = 0;
                double rp02XValue = Double.MinValue;
                double rp02 = Rp02(fittingPoints, indexOfPointClosestToRed, xTranslateAmount, out rp02XValue,out rp02Index);
                int indexOfRp005 = 0;
                double rp005XValue = Double.MinValue;
                //OptionsInPlottingMode.ReEqualsRp = 0.01;
                //Re = Rp005(fittingPoints, indexOfPointClosestToRed, xTranslateAmount, out rp005XValue, out rp005Index, OptionsInPlottingMode.ReEqualsRp);
                //Re = Math.Round(Re,0);
                indexClosestToRedPointGlobal = indexOfPointClosestToRed;

                //racunanje 0.05/*ovo nula pet namesta ce se u opcijama [opcija OptionsInPlottingMode.YungXelas]*/ tacke napona na osnovu koje se racuna
                int indexOfBorder005 = 0;
                double border005XValue = Double.MinValue;

                if (onMode.FirstImeClickedAtGreenButton == true)
                {
                    //OptionsInPlottingMode.ReEqualsRp = Rp02RIXValue;
                    OptionsInPlottingMode.ReEqualsRp = Math.Round(OptionsInPlottingMode.ReEqualsRp, 4);
                    optionsForYungsModuo.tfXelas.Text = OptionsInPlottingMode.ReEqualsRp.ToString();
                    /*save tfXelas*/
                    optionsForYungsModuo.tfXelas.Foreground = Brushes.White;
                    optionsForYungsModuo.SaveOptionstfXelas_Offline();
                }


                if (A < OptionsInPlottingMode.ReHXRange)
                {
                    if (IsMessageShownForRangeReHX == false)
                    {
                        MessageBox.Show("Nisu izracunati svi parametri. Morate podesiti opciju Podrucje tecenja (ReL-a,ReH-a i Rp02-a) !");
                        IsMessageShownForRangeReHX = true;
                    }
                }

                double border005 = Border005(fittingPoints, indexOfPointClosestToRed, xTranslateAmount, OptionsInPlottingMode.ReEqualsRp/*0.05*/, out border005XValue, out indexOfBorder005);
                Border005Global = border005;

                //indexOfRp02 - indeks koji se trazi na tekstualnom fajlu koji je fitovan
                for (int iRp02 = 0; iRp02 < dataReader.FittingPreassureInMPa.Count; iRp02++)
                {
                    if (dataReader.FittingPreassureInMPa[iRp02] > rp02)
                    {
                        iRp02--;
                        indexOfRp02 = iRp02;
                        break;
                    }
                }

                //rp02 = Math.Round(rp02, 0);
                //if (rp02 > 0)
                //{
                //    tfRp02.Text = Rp02RI.ToString();
                //}
                //else 
                //{
                //    tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                //}
                Rp02RI = rp02;


                _rp02XValue = rp02XValue;
                //chbRp02Visibility.IsEnabled = true;
                //chbRp02Visibility.IsChecked = true;
                //setRp02Point(indexOfRp02, dataReader, rp02, rp02XValue);
                //setRp02Point(indexOfRp02, dataReader, Rp02RI, Rp02RIXValue);
                _rp02_X = rp02XValue;


                rt05 = findRt05(pointsOfFittingLine);

                at = findAt(pointOfTearing);
                _at_X = at;
                mkrTriangleCurrentValues.AtXValue = at;

                int index = getRpmaxIndex();
                if (index >= dataReader.RelativeElongation.Count)
                {
                    //upisi u log fajl if (index >= dataReader.RelativeElongation.Count)
                    Logger.WriteNode("if (index >= dataReader.RelativeElongation.Count)", System.DateTime.Now);
                }
                agt = dataReader.RelativeElongation[index] - xTranslateAmountFittingMode;
                agt = Math.Round(agt, 1);
                _agt_X = agt;

                ag = findAg(yMarkers4[0],xMarkers4[0]);
                ag = Math.Round(ag, 1);
                _ag_X = ag;
                mkrTriangleCurrentValues.AgXValue = ag;



                //lu = dataReader.L0Offline * (1 + (at/100));
                //ovo je zskrpa
                if (A != a)
                {
                    a = A;
                }
                lu = dataReader.L0Offline * (1 + (a / 100));

                double s0Off = dataReader.S0Offline;
                double l0Off = dataReader.L0Offline;

                List<double> Fs_Fitting = new List<double>();
                List<double> Fs_FittingForManualN = new List<double>();
                int begIndex = 0, endIndex = dataReader.FittingRelativeElongation.Count;
                for (int ii = 0; ii < dataReader.FittingRelativeElongation.Count; ii++)
                {
                    if (dataReader.FittingRelativeElongation[ii] > OptionsInPlottingMode.BeginIntervalForN)
                    {
                        begIndex = ii;
                        break;
                    }
                }

                for (int ii = 0; ii < dataReader.FittingRelativeElongation.Count; ii++)
                {
                    double endintervalForN = OptionsInPlottingMode.EndIntervalForN;
                    if (ag < OptionsInPlottingMode.EndIntervalForN)
                    {
                        //endintervalForN = ag;
                        endintervalForN = OptionsInPlottingMode.BeginIntervalForN;
                    }
                    if (dataReader.FittingRelativeElongation[ii] > endintervalForN)
                    {
                        endIndex = ii;
                        break;
                    }
                }



                if (numberOfCallForDrawFitting % 3 == 0)
                {
                    if (A > 15)
                    {

                        if (PreassureForNManualProperty != null)
                        {
                            PreassureForNManualProperty.Clear();
                        }
                        List<double> deltaLsInProc = new List<double>();
                        List<double> deltaLsInProcForManualN = new List<double>();
                        Fs_Fitting = getFs_Fitting(s0Off, begIndex, endIndex, ref deltaLsInProc);
                        Fs_FittingForManualN = getFs_FittingForManualN(s0Off, begIndex, endIndex, ref deltaLsInProcForManualN, xTranslateAmountFittingMode);
                        Fs_FittingForManualNProperty = Fs_FittingForManualN;
                        DeltaLsInProcForManualNProperty = deltaLsInProcForManualN;

                        double mE = findME(fittingPoints);

                        nHardeningExponent = new NHardeningExponent(Fs_Fitting, deltaLsInProc, s0Off, l0Off, A, mE);

                        //double nHardeningExponent_Value = 0;
                        //nHardeningExponent_Value = nHardeningExponent.Get_N();
                        LastInputOutputSavedData.tfn_ResultsInterface = nHardeningExponent.Get_N().ToString();



                        NHardeningExponentManual = new NHardeningExponent(Fs_FittingForManualNProperty, DeltaLsInProcForManualNProperty, s0Off, l0Off, A, mE);
                        NManual = NHardeningExponentManual.Get_N();
                    }
                    else 
                    {
                        NManual = 0;
                    }
                }


                //transliraj sa originalnog na fitovani grafik
                IndexOfPointClosestToRedProperty = indexOfPointClosestToRed;
                //ne transliraj nista jel je vec sve postavljeno u drawFittingGraphic_AfterLuChanged pa ovde ne sme da se postavlja ponovo
                if (isReHWantToTranslate == false)
                {
                    isReHWantToTranslate = true;
                }
                else
                {
                    translateReHRelAndRmtoFittingGraphic(indexOfPointClosestToRed, xTranslateAmount);
                }


                //e2MinValue = onMode.VXY.ArrayE2Interval.Min();
                //e2MaxValue = onMode.VXY.ArrayE2Interval.Max();
                //e4MinValue = onMode.VXY.ArrayE4Interval.Min();
                //e4MaxValue = onMode.VXY.ArrayE4Interval.Max();
                //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                //RmaxwithPoint = e2e4CalculationAfterManualFitting.calculateRmaxWithPoint();
                //if (onMode.VXY != null)
                //{
                //    onMode.VXY.PointsChangeOfRFitting = e2e4CalculationAfterManualFitting.PointsChangeOfRFitting;
                //    onMode.VXY.CreateChangeOfRGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfRFitting);
                //    onMode.VXY.DeleteChangeOfRGraphic();
                //    onMode.VXY.PointsChangeOfEFitting = e2e4CalculationAfterManualFitting.PointsChangeOfEFitting;
                //    onMode.VXY.CreateChangeOfEGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfEFitting);
                //    onMode.VXY.DeleteChangeOfEGraphic();
                //}

                if (isLuManualChanged == false)
                {

                    /************ Ponovno racunanje i iscrtavanje posle promene izduzenja A u offline modu **************/
                    //NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                    e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                    e2e4CalculationAfterManualFitting.CalculateIndexFromChangedParametersFittingUntilA(A);
                    e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, xTranslateAmountFittingMode, false);
                    setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                    setResultsInterfaceForManualSetPoint();

                    //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);

               

                    RmaxwithPoint = e2e4CalculationAfterManualFitting.calculateRmaxWithPoint();
                    e2e4CalculationAfterManualFitting.RecalculateChangeOfRFittingPoints(this.Rp02RI, A);

                    if (onMode.VXY != null && onMode.chbStartSampleShowChangedPar.IsChecked == true && isLuManualChanged == false)
                    {
                        onMode.VXY.DeleteChangeOfRGraphic_Fitting();
                        onMode.VXY.PointsChangeOfRFitting = e2e4CalculationAfterManualFitting.PointsChangeOfRFitting;
                        onMode.VXY.CreateChangeOfRGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfRFitting);
                        //onMode.VXY.createChangeOfRGraphicFitting();
                        onMode.VXY.DeleteChangeOfRGraphic();

                        onMode.VXY.DeleteChangeOfEGraphic_Fitting();
                        onMode.VXY.PointsChangeOfEFitting = e2e4CalculationAfterManualFitting.PointsChangeOfEFitting;
                        onMode.VXY.CreateChangeOfEGraphic_Fitting(e2e4CalculationAfterManualFitting.PointsChangeOfEFitting, true);
                        //onMode.VXY.createChangeOfEGraphicFitting();
                        onMode.VXY.DeleteChangeOfEGraphic();
                    }
                    /************ Ponovno racunanje i iscrtavanje posle promene izduzenja A u offline modu **************/



                    if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                    {

                        e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E2E4Border, xTranslateAmount, true);
                        setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);

                    }
                    else
                    {


                        e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, xTranslateAmount, false);
                        setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);

                    }

                }
              
                //if (onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                //{
                    //if (window.FirstImeClicked == true)
                    //{
                        bool withTraverse = true;
                        if (onMode != null && onMode.OnHeader != null && onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                        {
                            withTraverse = true;
                        }
                        calculateYungsModuo(border005, withTraverse);
                        window.FirstImeClicked = false;
                    //}
                    if (isT3Active == true)
                    {
                    }
                    else
                    {
                        if ((numberOfCallForDrawFitting % 3 == 0 && numberOfCallForDrawFitting > 0) || isT2Active == true || isT1Active == true)
                        {
                            calculateYungsModuo(border005);
                            if (isT2Active == true)
                            {
                                isT2Active = false;
                            }
                            if (isT3Active == true)
                            {
                                isT3Active = false;
                            }
                        }
                    }
                //}
                if (onMode.OnHeader.ConditionsOfTesting != null && onMode.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    if (window.FirstImeClicked == true)
                    {
                        calculateYungsModuo(border005, true);
                        window.FirstImeClicked = false;
                    }

                    if (isT3Active == true)
                    {
                    }
                    else
                    {
                        if ((numberOfCallForDrawFitting % 3 == 0 && numberOfCallForDrawFitting > 0) || isT2Active == true || isT1Active == true)
                        {
                            calculateYungsModuo(border005, true);
                            if (isT2Active == true)
                            {
                                isT2Active = false;
                            }
                            if (isT3Active == true)
                            {
                                isT3Active = false;
                            }
                        }
                    }
                }


                if (IsEverT1T2orT3ManualSetted == true)
                {
                    IsEverT1T2orT3ManualSetted = false;
                }



                //resetuj vrednosti
                //koje odreduju dinamicki deo ResultsInterface-a [au,bu ili Du ili nista]
                //isRectangle = false;
                //isCircle = false;

                //ovo samo vazi kada se radi o online modu
                //ovo ne proveravaj kada ucitavas prethodno zapamcen fajl
                if (onMode != null && onMode.IsStoppedOnlineSample == true)
                {
                    if (onMode.OnHeader != null)
                    {
                        if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                        {
                            isRectangle = true;
                        }
                        else
                        {
                            isRectangle = false;
                        }
                        if (onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                        {
                            isCircle = true;
                        }
                        else
                        {
                            isCircle = false;
                        }


                        string currInOutFileName = getCurrentInputOutputFile();
                        if (File.Exists(currInOutFileName) == true)
                        {

                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();

                            //ovo mora u for petlju
                            //if (dataListInputOutput.ElementAt(dataListInputOutput.Count - 2).Contains(Constants.au) == true && dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Contains(Constants.bu) == true)
                            //{


                            //    _au = dataListInputOutput.ElementAt(dataListInputOutput.Count - 2).Split('\t').ElementAt(1).Trim();
                            //    _bu = dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Split('\t').ElementAt(1).Trim();
                            //    //_au = LastInputOutputSavedData.au;
                            //    //_bu = LastInputOutputSavedData.bu;

                            //}
                            for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                            {
                                if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.au) == true)
                                {
                                    for (int iii = ii; iii < dataListInputOutput.Count; iii++)
                                    {
                                        if (dataListInputOutput.ElementAt(iii).Split('\t').ElementAt(0).Trim().Equals(Constants.bu) == true)
                                        {
                                            _au = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                                            _bu = dataListInputOutput.ElementAt(iii).Split('\t').ElementAt(1).Trim();
                                            //break;
                                        }
                                    }
                                }
                                //break;
                            }
                            //if (dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Contains(Constants.Du2) == true)
                            // {


                            //        _Du = dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Split('\t').ElementAt(1).Trim();
                            //        // _Du = LastInputOutputSavedData.Du;

                            // }
                            for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                            {
                                if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.Du2) == true)
                                {
                                    _Du = dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim();
                                    //break;
                                }
                            }

                        }
                    }
                }




                //save last resultInterface content
                if (onMode.ResultsInterface != null)
                {
                    onMode.WriteXMLLastResultsInterface();
                }


                onMode.WriteXMLLastOnlineHeader();
              
               

                //save last input data (za sada se ovo nigde ne koristi)
                LastInputOutputSavedData.GetData();


                if (onMode.ResultsInterface != null)
                {

                    //pre nego sto krense da postavljas dinamicki deo izlaznog prozora proveri da li je bila cekirana pravougaona, kruzna ili neka treca vrsta epruvete
                    //ovo samo vazi kada se radi o online modu
                    //ovo ne proveravaj kada ucitavas prethodno zapamcen fajl
                    if (onMode != null && onMode.IsStoppedOnlineSample == true)
                    {
                        if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                        {
                            if ((onMode.ResultsInterface != null))
                            {
                                //onMode.ResultsInterface.IsRectangle = true;
                                isRectangle = true;
                                //onMode.ResultsInterface.IsCircle = false;
                                isCircle = false;
                            }
                        }
                        else if (onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                        {
                            if (onMode.ResultsInterface != null)
                            {
                                //onMode.ResultsInterface.IsCircle = true;
                                isCircle = true;
                                //onMode.ResultsInterface.IsRectangle = false;
                                isRectangle = false;
                            }
                        }
                        else
                        {
                            if (onMode.ResultsInterface != null)
                            {
                                //onMode.ResultsInterface.IsRectangle = false;
                                isRectangle = false;
                                //onMode.ResultsInterface.IsCircle = false;
                                isCircle = false;
                            }
                        }
                    }


                    string currInOutFileName = getCurrentInputOutputFile();

                    if (File.Exists(currInOutFileName) == true)
                    {
                        List<string> dataListInputOutput = new List<string>();
                        dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();

                        //ovo ce morati ici u for petlju
                        //if (dataListInputOutput.ElementAt(dataListInputOutput.Count - 2).Contains(Constants.au) == true && dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Contains(Constants.bu) == true)
                        //{
                        //    isRectangle = true;
                        //    isCircle = false;
                        //}
                        //for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                        //{
                        //    if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.au) == true)
                        //    {
                        //        for (int iii = ii; iii < dataListInputOutput.Count; iii++)
                        //        {
                        //            if (dataListInputOutput.ElementAt(iii).Split('\t').ElementAt(0).Trim().Equals(Constants.bu) == true)
                        //            {
                                        if (onMode.IsStoppedOnlineSample == true && numberOfCallForDrawFitting % 3 == 0)
                                        {
                                            if (onMode.OnHeader != null)
                                            {
                                                if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                                                {
                                                    if ((onMode.ResultsInterface != null))
                                                    {
                                                        //onMode.ResultsInterface.IsRectangle = true;
                                                        IsRectangle = true;
                                                        //onMode.ResultsInterface.IsCircle = false;
                                                        IsCircle = false;
                                                    }
                                                }
                                                else if (onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                                                {
                                                    if (onMode.ResultsInterface != null)
                                                    {
                                                        //onMode.ResultsInterface.IsCircle = true;
                                                        IsCircle = true;
                                                        //onMode.ResultsInterface.IsRectangle = false;
                                                        IsRectangle = false;
                                                    }
                                                }
                                                else
                                                {
                                                    if (onMode.ResultsInterface != null)
                                                    {
                                                        //onMode.ResultsInterface.IsRectangle = false;
                                                        isRectangle = false;
                                                        //onMode.ResultsInterface.IsCircle = false;
                                                        isCircle = false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //if (numberOfCallForDrawFitting != 3)
                                            //{
                                                //ovde ulazi samo ako se radi o ucitavanju vec zapamcenog txt fajla
                                                //tj. posle pritiska na dugme tab kada je kursor postavljen na textbox tfFilepathPlotting klase GraphicPlotting  
                                                int ii = 0;
                                                for (ii = 0; ii < dataListInputOutput.Count; ii++)
                                                {
                                                    //if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.au) == true)
                                                    //{
                                                    //    for (int iii = ii; iii < dataListInputOutput.Count; iii++)
                                                    //    {
                                                    //        if (dataListInputOutput.ElementAt(iii).Split('\t').ElementAt(0).Trim().Equals(Constants.bu) == true)
                                                    //        {
                                                    //            isRectangle = true;
                                                    //            isCircle = false;
                                                    //        }
                                                    //    }
                                                    //}
                                                    if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.VRSTAEPRUVETE) == true)
                                                    {
                                                        if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim().Equals(Constants.PRAVOUGAONA) == true)
                                                        {
                                                            isRectangle = true;
                                                            isCircle = false;
                                                            foundIsCircleOrRectangle = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (ii == dataListInputOutput.Count && foundIsCircleOrRectangle == false)
                                                {
                                                    isCircle = false;
                                                    isRectangle = false;
                                                }

                                              
                                            //}
                                        }
                                        //break;
                                    //}
                                //}
                                //break;
                            //}
                        //}
                        //ovo ce morati ici u for petlju
                        //if (dataListInputOutput.ElementAt(dataListInputOutput.Count - 1).Contains(Constants.Du2) == true)
                        //{
                        //    isCircle = true;
                        //    isRectangle = false;
                        //}
                        //for (int ii = 0; ii < dataListInputOutput.Count; ii++)
                        //{
                        //    if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.Du2) == true)
                        //    {
                              if (onMode.IsStoppedOnlineSample == true && numberOfCallForDrawFitting % 3 == 0)
                                {

                                    if (onMode.OnHeader != null)
                                    {
                                        if (onMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                                        {
                                            if ((onMode.ResultsInterface != null))
                                            {
                                                //onMode.ResultsInterface.IsCircle = true;
                                                IsCircle = true;
                                                //onMode.ResultsInterface.IsRectangle = false;
                                                IsRectangle = false;
                                               
                                            }
                                        }
                                        else if (onMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                                        {
                                            if (onMode.ResultsInterface != null)
                                            {
                                                //onMode.ResultsInterface.IsCircle = false;
                                                IsCircle = false;
                                                //onMode.ResultsInterface.IsRectangle = true;
                                                IsRectangle = true;
                                            }
                                        }
                                        else
                                        {
                                            if (onMode.ResultsInterface != null)
                                            {
                                                //onMode.ResultsInterface.IsRectangle = false;
                                                isRectangle = false;
                                                //onMode.ResultsInterface.IsCircle = false;
                                                isCircle = false;
                                            }
                                        }
                                    }
                                }
                                else
                                {

                                    //if (numberOfCallForDrawFitting != 3)
                                    //{
                                        //ovde ulazi samo ako se radi o ucitavanju vec zapamcenog txt fajla
                                        //tj. posle pritiska na dugme tab kada je kursor postavljen na textbox tfFilepathPlotting klase GraphicPlotting  
                                        int ii = 0;
                                        for (ii = 0; ii < dataListInputOutput.Count; ii++)
                                        {
                                            //if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.Du2) == true)
                                            //{
                                            //    isCircle = true;
                                            //    isRectangle = false;
                                            //}
                                            if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(0).Trim().Equals(Constants.VRSTAEPRUVETE) == true)
                                            {
                                                if (dataListInputOutput.ElementAt(ii).Split('\t').ElementAt(1).Trim().Equals(Constants.KRUZNA) == true)
                                                {
                                                    isCircle = true;
                                                    isRectangle = false;
                                                    foundIsCircleOrRectangle = true;
                                                    break;
                                                }
                                            }
                                        }

                                        if (ii == dataListInputOutput.Count && foundIsCircleOrRectangle == false)
                                        {
                                            isCircle = false;
                                            isRectangle = false;
                                        }

                                    //}
                                }
                                //break;
                            //}
                        //}

                        //pokupi i informaciju i postavi au i bu(ako je pravougaona) ili samo Du(ako je kruzna)
                        onMode.ResultsInterface.SetRadioButtons();

                        if (isRectangle == true)
                        {
                            onMode.ResultsInterface.SetTextBoxes(_au, _bu);
                        }
                        if (isCircle == true)
                        {
                            onMode.ResultsInterface.SetTextBoxes(_Du);
                        }
                        if (isRectangle == false && isCircle == false)
                        {
                            //ukloni prethodno stavljanje au i bu ili du
                            onMode.ResultsInterface.ClearAuBuOrDuPart();
                        }


                        //set results interface
                        if (ResultsInterface.isCreatedResultsInterface == false)
                        {
                            onMode.loadFirstAfterRunResultsInterface();
                        }
                        //string su = string.Empty, z = string.Empty;
                        string firstPart = string.Empty, secondPart = string.Empty;
                        foreach (string s in dataListInputOutput)
                        {
                            if (s.Split('\t').Count() > 1)
                            {
                                firstPart = s.Split('\t').ElementAt(0).Trim();
                                secondPart = s.Split('\t').ElementAt(1).Trim();
                            
                                if (firstPart.Contains(Constants.Su) == true)
                                {
                                    suStr = secondPart;
                                }
                                if (firstPart.Contains(Constants.Z) == true)
                                {
                                    zStr = secondPart;
                                }
                            }
                        }
                        onMode.setResultsInterface(suStr, zStr, numberOfCallForDrawFitting);
                        if (OnlineFileHeader.isCreatedOnlineHeader == false)
                        {
                            onMode.showInputData();
                        }
                    }
                }


              
                
                //resetuj projac prolaska kroz ovu metodu
                //jer on ne sme biti veci od 3
                if (numberOfCallForDrawFitting % 3 == 0)
                {

                   
                   


                    ////set R2R4 exacly at the green (original) graphic
                    //for (int ind = 0; ind < dataReader.RelativeElongation.Count; ind++)
                    //{
                    //    if (dataReader.RelativeElongation[ind] >= OptionsInOnlineMode.E2E4Border)
                    //    {
                    //        dataReader.PreassureInMPa[ind] = Math.Round(dataReader.PreassureInMPa[ind], 0);
                    //        setPointAtGraphicR2R4(dataReader.RelativeElongation[ind], dataReader.PreassureInMPa[ind]);                     

                    //        break;
                    //    }

                    //}

                    if (onMode.ResultsInterface.tfAGlobal != null)
                    {
                        onMode.ResultsInterface.tfAGlobal.IsReadOnly = false;
                    }
                    if (onMode.ResultsInterface.tfBGlobal != null)
                    {
                        onMode.ResultsInterface.tfBGlobal.IsReadOnly = false;
                    }
                    if (onMode.ResultsInterface.tfDGlobal != null)
                    {
                        onMode.ResultsInterface.tfDGlobal.IsReadOnly = false;
                        //onMode.ResultsInterface.tfDGlobal.IsEnabled = false;
                    }
                   
                    numberOfCallForDrawFitting = 0;
                }

                if (isAClicked == true)
                {
                   
                    bool isNN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                 
                    isNN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                 
                    isNN = double.TryParse(tffittingManPoint2X.Text, out t2x);
              
                    isNN = double.TryParse(tffittingManPoint2Y.Text, out t2y);

                    double AX = (-1) * (AClickedYValue - ((t2y - t1y) / (t2x - t1x)) * AClickedXValue) / ((t2y - t1y) / (t2x - t1x));
                    AX = Math.Round(AX, 2);
                    AManualClickedValue = AX;
                    setAPoint_Manual(AX);
                    A = AX;
                }

               

                if (OptionsInPlottingMode.isAutoFittingChecked == true && OptionsInPlottingMode.isManualFittingChecked == false)
                {
                    setManualPointsToAutoPointsValue();
                }

                printscreen.SetT1Point();
                printscreen.SetT2Point();
                printscreen.SetT3Point();



                //rp02Index - indeks koji se trazi na originalnom tekstualnom fajlu 
                int iRp02Local = 0;
                if (tfReH.Text.Equals(Constants.NOTFOUNDMAXMIN) == true)
                {
                    for (iRp02Local = 0; iRp02Local < dataReader.FittingPreassureInMPa.Count; iRp02Local++)
                    {
                        if (dataReader.FittingPreassureInMPa[iRp02Local] > Rp02RI /*&& dataReader.FittingRelativeElongation[iRp02Local] > _reH_X*/)
                        {
                            iRp02Local--;
                            rp02Index = iRp02Local;
                            Rp02RIXValue = dataReader.FittingRelativeElongation[iRp02Local];
                            //Rp02RIXValue = dataReader.RelativeElongation[iRp02Local] - xTranslateAmount;
                            //Rp02RIXValue = Rp02RIXValue /*+ 0.2*/;
                            mkrTriangleCurrentValues.Rp02XValue = Rp02RIXValue;
                            mkrTriangleCurrentValues.Rp02YValue = Rp02RI;
                            break;
                        }
                    }
                }
                else
                {
                    for (iRp02Local = 0; iRp02Local < dataReader.FittingPreassureInMPa.Count; iRp02Local++)
                    {
                        if (dataReader.FittingPreassureInMPa[iRp02Local] <= Rp02RI && dataReader.FittingRelativeElongation[iRp02Local] > _reH_X)
                        {
                            iRp02Local--;
                            rp02Index = iRp02Local;
                            Rp02RIXValue = dataReader.FittingRelativeElongation[iRp02Local];
                            //Rp02RIXValue = dataReader.RelativeElongation[iRp02Local] - xTranslateAmount;
                            //Rp02RIXValue = Rp02RIXValue /*+ 0.2*/;
                            mkrTriangleCurrentValues.Rp02XValue = Rp02RIXValue;
                            mkrTriangleCurrentValues.Rp02YValue = Rp02RI;
                            break;
                        }
                    }
                }
                setRp02PointCalculated(iRp02Local,Rp02RIXValue, Rp02RI);
                //za svaki slucaj ponovo proracunaj parametre fitovanja
                //calcuateOnlyFittingParameters_Rp02();
                if (A < 0.2)
                {
                    setResultsInterfaceIfALessThanzeropointtwo();
                }

                //ovde proveri da li se desio slucaj kada postoji samo ReL
                //ovaj deo koda bio je van metode a sada je u njoj tako da tamo gde je van metode izvrsava se dvaput
                //gde god nadjes isti ovakav kod moze da se brise jel dolazi samo do dupliranja
                int indexOfmin = -1;
                double onlyReL = Double.MaxValue;
                if (isFoundMaxAndMin == false)
                {
                    onlyReL = findMinReLOnly(out indexOfmin);
                }

                if (isFoundOnlyReLCase == true)
                {
                    onlyReL = Math.Round(onlyReL, 0);
                    ReL = onlyReL;
                    tfReL.Text = onlyReL.ToString();
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = onlyReL.ToString();
                    }
                    setReLPoint(indexOfmin);
                }

                //ako je A razlicito sa onim sto je postavljeno u tesktualnom polju za A u Analizi grafika ti ovde
                //to izjednaci
                double ArelativeElongation = 0;
                bool isNumber = double.TryParse(tfA.Text, out ArelativeElongation);
                ArelativeElongation = Math.Round(ArelativeElongation,1);
                if (this.A != ArelativeElongation)
                {
                    this.A = ArelativeElongation;
                }

                printscreen.setMarkers();




                if (Rp02RI > 0)
                {
                    tfRp02.Text = Rp02RI.ToString();
                }
                else
                {
                    tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                }

                //bilo da je radjeno rucno ili je radjeno automatski prikazi zadnju vrednost
                if (isT1Active == true)
                {
                    isT1Active = false;
                }
                if (isT2Active == true)
                {
                    isT2Active = false;
                }
                if (isT3Active == true)
                {
                    isT3Active = false;
                }


               
                if (isT1Active == false && isT2Active == false && isT3Active == false)
                {
                    setT1T2T3FromTextboxes();
                }    
                

                if (isLuManualChanged == false)
                {
                    A_FirstCalculated = A;
                }

                if (A <= 0.2)
                {
                    onMode.ResultsInterface.chbRp02.IsChecked = false;
                }

                

                if (onMode.ResultsInterface.badCalculationHappened == true)
                {
                    if (BadCalculationHappened == false)
                    {
                        MessageBox.Show("Nisu izracunati svi parametri. Morate restartovati aplikaciju!");
                        BadCalculationHappened = true;
                    }
                }

                onMode.ResultsInterface.tfA.Text = A.ToString();
                tfA.Text = A.ToString();

                //setT1T2T3FromTextboxes();

                //if (fm < 1000 * ForceRangesOptions.forceLowerCurrent || fm > 1000 * ForceRangesOptions.forceUpperCurrent)
                //{
                //    onMode.ResultsInterface.tfFm.Foreground = Brushes.Red;
                //}
                //else
                //{
                //    onMode.ResultsInterface.tfFm.Foreground = Brushes.Black;
                //}



              

                List<double> listMPas = new List<double>();
                foreach (var point in fittingPoints)
                {
                    listMPas.Add(point.YAxisValue);
                }
                double MaxRm = listMPas.Max();
                index = listMPas.IndexOf(MaxRm);
                setRmPoint(fittingPoints[index].XAxisValue, fittingPoints[index].YAxisValue);
                printscreen.setRmPoint2(fittingPoints[index].XAxisValue, fittingPoints[index].YAxisValue);



                double k = 0;
                t1y = 0; t1x = 0; t2x = 0; t2y = 0; t3x = 0; t3y = 0;
                bool isN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                isN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                isN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                isN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                isN = double.TryParse(tffittingManPoint3X.Text, out t3x);
                isN = double.TryParse(tffittingManPoint3Y.Text, out t3y);

                if (t1x == 0)
                {
                    t1y = 0;
                }
                if (t1x == 0 && t2x == 0)
                {
                    t2x = 0.1;
                    tffittingManPoint2X.Text = t2x.ToString();
                    tffittingManPoint2X.Foreground = Brushes.Black;
                }
                //k = (t2y - t1y)/(t2x - t1x);
                //if (isT1Active == true || isT2Active == true)
                if(useManualT1orT2 == true)
                {
                    k = (OptionsInPlottingMode.pointManualY2 - OptionsInPlottingMode.pointManualY1) / (OptionsInPlottingMode.pointManualX2 - OptionsInPlottingMode.pointManualX1);
                }
                else
                {
                    k = (OptionsInPlottingMode.pointAutoY2 - OptionsInPlottingMode.pointAutoY1) / (OptionsInPlottingMode.pointAutoX2 - OptionsInPlottingMode.pointAutoX1);
                }


                if (fittingPoints[fittingPoints.Count - 1].XAxisValue < xMarkers4[0] + xTranslateAmount)
                {
                    double newx = (Rm / k) + this.A;
                    MyPoint lastOne = new MyPoint(this.Rm, newx);
                    //fittingPoints.Add(lastOne);
                }

                List<string> splitPath = tfFilepathPlotting.Text.Split('.').ToList();
                if (File.Exists(splitPath[0] + ".e2e4") == true)
                {
                    List<string> allChanges = File.ReadAllLines(splitPath[0] + ".e2e4").ToList();
                    List<double> allNumberOfChanges = new List<double>();
                    foreach (var item in allChanges)
                    {
                        List<string> itemList = item.Split('*').ToList();
                        double number = 0;
                        isN = double.TryParse(itemList[0], out number);
                        if (isN)
                        {
                            allNumberOfChanges.Add(number);
                        }
                    }

                    this.RmaxwithPoint = allNumberOfChanges.Max();
                    OnlineModeInstance.ResultsInterface.tfRmax.Text = this.RmaxwithPoint.ToString();
                }


                if (numberOfCallForDrawFitting % 3 == 0)
                {
                    
                        counter = 0;
                        foreach (var point in fittingPoints)
                        {
                            if (point.XAxisValue != null)
                            {
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        setAPoint(fittingPoints[counter - 1].XAxisValue - fittingPoints[counter - 1].YAxisValue/k);
                        printscreen.setAPoint(fittingPoints[counter - 1].XAxisValue - fittingPoints[counter - 1].YAxisValue / k);
                        this.A = fittingPoints[counter - 1].XAxisValue - fittingPoints[counter - 1].YAxisValue / k;
                        this.A = Math.Round(this.A,1);
                        tfA.Text = this.A.ToString();
                        OnlineModeInstance.ResultsInterface.tfA.Text = this.A.ToString();

                }

                if (File.Exists(Constants.RmaxWithPoint) == true)
                {
                    File.Delete(Constants.RmaxWithPoint);
                }
                List<string> rmaxwithpoint = new List<string>();
                rmaxwithpoint.Add(RmaxwithPoint.ToString());
                File.AppendAllLines(Constants.RmaxWithPoint, rmaxwithpoint);


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "drawFittingGraphic()");
                //MessageBox.Show(" Aplikaciji nisu prosledjeni svi podaci! Molimo vas ako niste, restartujte aplikaciju !");
                onMode.ResultsInterface.tfA.Text = A.ToString();
                tfA.Text = A.ToString();
                //Logger.WriteNode(ex.Message.ToString() + "  {drawFittingGraphic()}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void drawFittingGraphic(bool T3movingDirectionByYAxis = true, int numberOfCallForDrawFitting = 0, bool isAClicked = false, double AClickedXValue = 0, double AClickedYValue = 0))}", System.DateTime.Now);
                printscreen.SetT1Point();
                printscreen.SetT2Point();
                printscreen.SetT3Point();
            }
           
        }
       


        private void calcuateOnlyFittingParameters_Rp02() 
        {
            try
            {
                //odavde pocinje racunanje parametara koji se upisuju u izlazni prozor (zuti prozor)
                int indexOfRp02 = 0;
                double rp02XValue = Double.MinValue;
                double rp02 = Rp02(pointsOfFittingLine, indexClosestToRedPointGlobal, xTranslateAmountFittingMode, out rp02XValue, out rp02Index);

                for (int iRp02 = 0; iRp02 < dataReader.FittingPreassureInMPa.Count; iRp02++)
                {
                    if (dataReader.FittingPreassureInMPa[iRp02] > rp02)
                    {
                        iRp02--;
                        indexOfRp02 = iRp02;
                        break;
                    }
                }

                rp02 = Math.Round(rp02, 0);
                tfRp02.Text = rp02.ToString();
                Rp02RI = rp02;
                _rp02XValue = rp02XValue;
                chbRp02Visibility.IsEnabled = true;
                //chbRp02Visibility.IsChecked = true;
                //setRp02Point(indexOfRp02, dataReader, rp02, rp02XValue);
                //setRp02Point(indexOfRp02, dataReader, Rp02RI, Rp02RIXValue);
                _rp02_X = rp02XValue;
               

            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calcuateOnlyFittingParameters_Rp02()}", System.DateTime.Now);
            }
        }

        public void SetE2E4MinMaxAvg(E2E4CalculationAfterManualFitting e2e4CalculationAfterManualFitting)
        {
            try
            {
                setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetE2E4MinMaxAvg(E2E4CalculationAfterManualFitting e2e4CalculationAfterManualFitting)}", System.DateTime.Now);
            }
        }

        private void setE2E4MinMaxAvg(E2E4CalculationAfterManualFitting e2e4CalculationAfterManualFitting) 
        {
            try
            {
                if (isLuManualChanged == false)//ne racunaj brzine promene parametara kada rucno menjas Lu
                {
                    if (e2e4CalculationAfterManualFitting.ArrayE2Interval.Count > 0)
                    {
                        e2MinValue = e2e4CalculationAfterManualFitting.ArrayE2Interval.Min();
                        e2MaxValue = e2e4CalculationAfterManualFitting.ArrayE2Interval.Max();
                        e2AvgValue = e2e4CalculationAfterManualFitting.ArrayE2Interval.Average();
                        //samo je ovo potrebno zaokruziti jer se deli sa brojem elemenata 
                        e2AvgValue = Math.Round(e2AvgValue, 5);
                    }
                    else
                    {
                        e2MinValue = -1;
                        e2MaxValue = -1;
                        e2AvgValue = -1;
                    }
                    if (e2e4CalculationAfterManualFitting.ArrayE4Interval.Count > 0)
                    {
                        e4MinValue = e2e4CalculationAfterManualFitting.ArrayE4Interval.Min();
                        e4MaxValue = e2e4CalculationAfterManualFitting.ArrayE4Interval.Max();
                        e4AvgValue = e2e4CalculationAfterManualFitting.ArrayE4Interval.Average();
                        //samo je ovo potrebno zaokruziti jer se deli sa brojem elemenata 
                        e4AvgValue = Math.Round(e4AvgValue, 5);
                    }
                    else
                    {
                        e4MinValue = -1;
                        e4MaxValue = -1;
                        e4AvgValue = -1;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "setE2E4MinMax()");
                //Logger.WriteNode(ex.Message.ToString() + " {setE2E4MinMax()}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setE2E4MinMaxAvg(E2E4CalculationAfterManualFitting e2e4CalculationAfterManualFitting)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// pronadji vrednost me koja se moze koristi kod novijeg standarda za racunanje n-a
        /// trenutno se za racunanje n-a koristi vrednost koja ne korisit m-a
        /// </summary>
        /// <param name="fittingPoints"></param>
        /// <returns></returns>
        private double findME(MyPointCollection fittingPoints) 
        {
            try
            {
                double mE = 0;
                mE = (fittingPoints[1].YAxisValue - fittingPoints[0].YAxisValue) / (fittingPoints[1].XAxisValue - fittingPoints[0].XAxisValue);
                return mE;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findME(MyPointCollection fittingPoints)}", System.DateTime.Now);
                return 0;
            }
        }

        private List<double> getFs_Fitting(double s0, int begIndex, int endIndex,ref List<double> deltaLsInProc) 
        {
            try
            {
                List<double> FsFitting = new List<double>();
                double tempFsFitting = 0;
                double tempdeltaLsInProc = 0;
                int substruction = endIndex - begIndex;
                int counterForOneNSample = substruction / 200;//ovo znaci uzmi 200 tacaka za racunanje n-a

                for (int i = begIndex; i < endIndex; i++)
                {
                    counterForOneNSample--;
                    if (counterForOneNSample == 0)
                    {
                        tempFsFitting = dataReader.FittingPreassureInMPa[i] * s0 / 1000;
                        FsFitting.Add(tempFsFitting);
                        tempdeltaLsInProc = dataReader.FittingRelativeElongation[i];
                        deltaLsInProc.Add(tempdeltaLsInProc);
                        counterForOneNSample = substruction / 200;//ovo znaci uzmi 200 tacaka za racunanje n-a
                    }
                }
                return FsFitting;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private List<double> getFs_Fitting(double s0, int begIndex, int endIndex,ref List<double> deltaLsInProc)}", System.DateTime.Now);
                return new List<double>();
            }
        }

        private List<double> getFs_FittingForManualN(double s0, int begIndex, int endIndex, ref List<double> deltaLsInProc, double xTranslateAmount)
        {
            try
            {
                List<double> FsFitting = new List<double>();
                double tempFsFitting = 0;
                double tempdeltaLsInProc = 0;

                if (PreassureForNManualProperty == null)
                {
                    PreassureForNManualProperty = new List<double>();
                }

                //double endForce = dataReader.FittingPreassureInMPa[endIndex] * s0 / 1000;
                //double beginForce = dataReader.FittingPreassureInMPa[begIndex] * s0 / 1000;
                double qxqxqxq = 1;
                if (ag < OptionsInPlottingMode.EndIntervalForN)
                {
                    qxqxqxq = ag / OptionsInPlottingMode.BeginIntervalForN;
                    return new List<double>();
                }
                else
                {
                    qxqxqxq = OptionsInPlottingMode.EndIntervalForN / OptionsInPlottingMode.BeginIntervalForN;
                }
                qxqxqxq = Math.Round(qxqxqxq, 3);

                double qxq = Math.Sqrt(qxqxqxq);
                qxq = Math.Round(qxq, 3);

                double q = Math.Sqrt(qxq);
                q = Math.Round(q, 3);

                deltaLsInProc.Add(OptionsInPlottingMode.BeginIntervalForN);
                deltaLsInProc.Add(OptionsInPlottingMode.BeginIntervalForN * q);
                deltaLsInProc.Add(OptionsInPlottingMode.BeginIntervalForN * q * q);
                deltaLsInProc.Add(OptionsInPlottingMode.BeginIntervalForN * q * q * q);
                deltaLsInProc.Add(OptionsInPlottingMode.BeginIntervalForN * q * q * q * q);


                int index = 0;

                for (int i = 0; i < 5; i++)
                {
                    double x = 0;
                    double y = 0;

                    //while ((deltaLsInProc[i] + xTranslateAmount) > dataReader.RelativeElongation[index])
                    //{
                    //    index++;
                    //}
                    if (currK != double.PositiveInfinity && currN != double.PositiveInfinity)
                    {
                        ////double newN = -1 * deltaLsInProc[i] * currK;
                        //x = (currK * currN + deltaLsInProc[i]) / (1 - currK * currK);
                        ////x = (currK * newN + deltaLsInProc[i]) / (1 - currK * currK);
                        //y = currN + currK * x;
                        ////y = newN + currK * x;
                        //FsFitting.Add(y * s0 / 1000);
                        //PreassureForNManualProperty.Add(y);
                        //deltaLsInProc[i] = x;
                        ////FsFitting.Add(dataReader.PreassureInMPa[index] * s0/1000);
                        ////PreassureForNManualProperty.Add(dataReader.PreassureInMPa[index]);
                        ////deltaLsInProc[i] = dataReader.RelativeElongation[index] - xTranslateAmount;

                        while ((deltaLsInProc[i]) > dataReader.FittingRelativeElongation[index])
                        {
                            index++;
                            if (index == dataReader.FittingRelativeElongation.Count)
                            {
                                index--;
                                break;
                            }
                        }
                        //dodaje se tek kasnije jel ovde prava fitovanja nije linija normalna na X-osu
                        //vec sece X-osu pod nekim kosim uglom tako da, da bismo dobili tacne vrednosti napona i izduzenja 
                        //moramo pronaci nove napone i izduzenja
                        //gde posle toga dodajemo sile i napone u listu, a izduzenja prepravljamo na nova
                        //FsFitting.Add(dataReader.FittingPreassureInMPa[index] * s0 / 1000);
                        //PreassureForNManualProperty.Add(dataReader.FittingPreassureInMPa[index]);
                        deltaLsInProc[i] = dataReader.FittingRelativeElongation[index];


                        /*******************    ovo moze ici i u metodu    ************************/
                        //dodaj jos nekoliko tacaka jer tacka rucnog nalazenja N-a ne ide pod pravim uglom vec paralelno sa linijom fitovanja
                        //index nam govori odakle krecemo od points kolekcije koja sadrzi podatke o originalnom grafiku
                        int ind = 0;
                        foreach (var point in pointsOfFittingLine)
                        {
                            ind++;
                            if ((point.XAxisValue) >= deltaLsInProc[i])
                            {
                                break;
                            }
                        }

                        MyPoint currentPoint = new MyPoint(0, 0);
                        double newN = -1 * deltaLsInProc[i] * currK;
                        double currDifferenceinY = 0;
                        //prvo racunamo razliku po Y osi samo izmedju zadnje ucrtane tacke koja je normalna na tacku rucnog nalazenja N-a i prave koja je paralelna sa linijom fitovanja a sece x-osu u tacki rucnog nalazenja N-a
                        double originalY = -1;
                        if (ind >= pointsOfFittingLine.Count)
                        {
                            originalY = currK * pointsOfFittingLine.Last().XAxisValue + newN;
                            currDifferenceinY = Math.Abs(originalY - pointsOfFittingLine.Last().YAxisValue);
                        }
                        else
                        {
                            originalY = currK * pointsOfFittingLine[ind].XAxisValue + newN;
                            currDifferenceinY = Math.Abs(originalY - pointsOfFittingLine[ind].YAxisValue);
                        }

                        for (int ii = ind + 1; ii < pointsOfFittingLine.Count; ii++)
                        {

                            double nextDifferenceinY = 0;
                            originalY = currK * pointsOfFittingLine[ii].XAxisValue + newN;
                            nextDifferenceinY = Math.Abs(originalY - pointsOfFittingLine[ii].YAxisValue);
                            if (nextDifferenceinY <= currDifferenceinY)
                            {
                                currDifferenceinY = nextDifferenceinY;
                            }
                            else
                            {
                                currentPoint = pointsOfFittingLine[ii];
                                break;
                            }
                        }
                        /*******************    ovo moze ici i u metodu    ************************/

                        x = currentPoint.XAxisValue;
                        y = currentPoint.YAxisValue;
                        //dodaje se tek kasnije jel ovde prava fitovanja nije linija normalna na X-osu
                        //vec sece X-osu pod nekim kosim uglom tako da, da bismo dobili tacne vrednosti napona i izduzenja 
                        //moramo pronaci nove napone i izduzenja
                        //OVDE DODAJEMO TA NOVA IZDUZENJA
                        FsFitting.Add(y * s0 / 1000);
                        PreassureForNManualProperty.Add(y);
                        //ovde izduzenja prepravljamo na nova
                        deltaLsInProc[i] = x;



                    }
                    else if (currK == double.PositiveInfinity)
                    {
                        for (i = 0; i < 5; i++)
                        {
                            while ((deltaLsInProc[i]) > dataReader.FittingRelativeElongation[index])
                            {
                                index++;
                                if (index == dataReader.FittingRelativeElongation.Count)
                                {
                                    //upisi u log file [private List<double> getFs_FittingForManualN(double s0, int begIndex, int endIndex, ref List<double> deltaLsInProc, double xTranslateAmount)]{f (index == dataReader.FittingRelativeElongation.Count)} break;
                                    break;
                                }
                            }
                            FsFitting.Add(dataReader.FittingPreassureInMPa[index] * s0 / 1000);
                            PreassureForNManualProperty.Add(dataReader.FittingPreassureInMPa[index]);
                            deltaLsInProc[i] = dataReader.FittingRelativeElongation[index];
                        }
                    }
                }

                return FsFitting;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private List<double> getFs_FittingForManualN(double s0, int begIndex, int endIndex, ref List<double> deltaLsInProc, double xTranslateAmount)}", System.DateTime.Now);
                return new List<double>();
            }

        }

        private double findAg()
        {
            try
            {
                int rmIndex = getRpmaxIndex();
                double rmX = dataReader.RelativeElongation[rmIndex];
                double rmY = dataReader.PreassureInMPa[rmIndex];
                double ag02K = currK;
                double ag02N = rmY - currK * (rmX - xTranslateAmountFittingMode);
                double ag = Double.MinValue;

                if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                {
                    ag = agt;
                }
                else
                {
                    //ag = (-1) * ag02N / ag02K;
                    //ag = Math.Round(ag, 1);
                    double t1x = 0;
                    bool isNN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                    double t1y = 0;
                    isNN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                    double t2x = 0;
                    isNN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                    double t2y = 0;
                    isNN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                    double currKGlobal = (t2y - t1y) / (t2x - t1x);

                    ag = (-1) * (rmY - currKGlobal * rmX) / currKGlobal;
                    ag = Math.Round(ag, 2);
                }

                return ag;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findAg()}", System.DateTime.Now);
                return 0;
            }
        }

        /// <summary>
        /// ovo se korisit pri rucnom postavljanju maksimuma
        /// </summary>
        /// <param name="Rm"></param>
        /// <returns></returns>
        private double findAg(double Rm, double Rmx)
        {
            try
            {
                int rmIndex = getRpmaxIndex(Rm);
                //double rmX = dataReader.RelativeElongation[rmIndex];
                //double rmY = dataReader.PreassureInMPa[rmIndex];
                double rmX = Rmx;
                double rmY = Rm;
                double ag02K = currK;
                double ag02N = rmY - currK * (rmX - xTranslateAmountFittingMode);
                double ag = Double.MinValue;

                if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                {
                    ag = agt;
                }
                else
                {
                    //ag = (-1) * ag02N / ag02K;
                    //ag = Math.Round(ag, 1);
                    double t1x = 0;
                    bool isNN = double.TryParse(tffittingManPoint1X.Text, out t1x);
                    double t1y = 0;
                    isNN = double.TryParse(tffittingManPoint1Y.Text, out t1y);
                    double t2x = 0;
                    isNN = double.TryParse(tffittingManPoint2X.Text, out t2x);
                    double t2y = 0;
                    isNN = double.TryParse(tffittingManPoint2Y.Text, out t2y);
                    double currKGlobal = (t2y - t1y) / (t2x - t1x);

                    ag = (-1) * (rmY - currKGlobal * rmX) / currKGlobal;
                    ag = Math.Round(ag, 2);
                }

                mkrTriangleCurrentValues.RmXValue = rmX;
                mkrTriangleCurrentValues.RmYValue = rmY;

                return ag;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findAg(double Rm, double Rmx)}", System.DateTime.Now);
                return 0;
            }
        }


        private double findAt(MyPoint pointOfTearing) 
        {
            try
            {
                double at = pointOfTearing.XAxisValue;
                at = Math.Round(at, 1);
                return at;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findAt(MyPoint pointOfTearing)}", System.DateTime.Now);
                return 0;
            }
        }

        private double findRt05_AfterLuChanged(MyPointCollection fittingPoints)
        {
            try
            {
                double result = 0;

                foreach (var point in fittingPoints)
                {
                    if (point.XAxisValue >= 0.5)
                    {
                        result = point.YAxisValue;
                        break;
                    }
                }

                result = Math.Round(result, 1);

                return result;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findRt05_AfterLuChanged(MyPointCollection fittingPoints))}", System.DateTime.Now);
                return 0;
            }
        }

        private double findRt05(MyPointCollection fittingPoints)
        {
            try
            {
                double rt05;
                int i;
                if (fittingPoints[1].XAxisValue < 0.5)//ako je odseceni deo manji po x - osi od 0.5
                {
                    for (i = 0; i < dataReader.FittingPreassureInMPa.Count; i++)
                    {
                        if (dataReader.FittingRelativeElongation[i] > 0.5)
                        {
                            rt05 = dataReader.FittingPreassureInMPa[i];
                            rt05 = Math.Round(rt05, 1);
                            return rt05;
                        }
                    }
                }

                if (fittingPoints[1].XAxisValue == 0.5) //ako je tacno po x-osi odseceni deo tacno 0.5
                {
                    rt05 = fittingPoints[1].YAxisValue;
                    rt05 = Math.Round(rt05, 1);
                    return rt05;
                }

                if (fittingPoints[1].XAxisValue > 0.5)//ako je odseceni deo veci po x - osi od 0.5
                {
                    double kOfFittingLine = (fittingPoints[1].YAxisValue - fittingPoints[0].YAxisValue) / (fittingPoints[1].XAxisValue - fittingPoints[0].XAxisValue);
                    rt05 = kOfFittingLine * 0.5;
                    rt05 = Math.Round(rt05, 1);
                    return rt05;
                }
                return Double.MinValue;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findRt05(MyPointCollection fittingPoints)}", System.DateTime.Now);
                return 0;
            }
        }

        private double Border005(MyPointCollection fittingPoints, int indexOfPointClosestToRed, double xTranslateAmount, double borderX, out double Border005XValue, out int Border005index)
        {
            try
            {
                Border005index = 0;
                Border005XValue = 0;
                int indexOfBorder005 = 0;
                double xSubstractionForBorder005 = Double.MaxValue;
                if (OptionsInPlottingMode.isManualFittingChecked)
                {
                    xSubstractionForBorder005 = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                }
                else
                {
                    xSubstractionForBorder005 = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                }


                if (xSubstractionForBorder005 < 0.01 || xSubstractionForBorder005 == 0.01)
                {

                    double minXSubs = Double.MaxValue;
                    double xSubs = -1;

                    for (int i = 0; i < fittingPoints.Count - 1; i++)
                    {
                        if (fittingPoints[i].XAxisValue > 1) break;
                        xSubs = Math.Abs(borderX - fittingPoints[i].XAxisValue);
                        if (xSubs < minXSubs)
                        {
                            minXSubs = xSubs;
                            indexOfBorder005 = i;
                        }
                    }
                    Border005XValue = fittingPoints[indexOfBorder005].XAxisValue;
                    return fittingPoints[indexOfBorder005].YAxisValue;
                }
                else
                {

                    double Border005K = currK;
                    double Border005N = -currK * borderX;

                    //ako je k ipak beskonacno
                    if (Double.IsPositiveInfinity(Border005K) || Double.IsNegativeInfinity(Border005K))
                    {
                        double minXSubs = Double.MaxValue;
                        double xSubs = -1;

                        for (int i = 0; i < fittingPoints.Count - 1; i++)
                        {
                            if (fittingPoints[i].XAxisValue > 1) break;
                            xSubs = Math.Abs(borderX - fittingPoints[i].XAxisValue);
                            if (xSubs < minXSubs)
                            {
                                minXSubs = xSubs;
                                indexOfBorder005 = i;
                            }
                        }
                        Border005XValue = fittingPoints[indexOfBorder005].XAxisValue;
                        return fittingPoints[indexOfBorder005].YAxisValue;
                    }
                    //ako je k ipak beskonacno

                    double yIdeal = Border005K * 0 + Border005N;

                    double minYSubs = Double.MaxValue;
                    double ySubs = -1;
                    //int indexOfBorder005 = -1;
                    for (int i = 0; i < fittingPoints.Count - 1; i++)
                    {
                        yIdeal = Border005K * fittingPoints[i].XAxisValue + Border005N;
                        ySubs = Math.Abs(yIdeal - fittingPoints[i].YAxisValue);
                        if (ySubs < minYSubs && fittingPoints[i].XAxisValue >= borderX + OptionsInPlottingMode.pointManualX3 - xTranslateAmount /*&& fittingPoints[i].XAxisValue < 0.5*/)
                        {
                            minYSubs = ySubs;
                            indexOfBorder005 = i;
                        }
                    }

                    int indexFromGreenOriginalDataBegin = Int32.MinValue;

                    if (indexOfBorder005 > 2)
                    {
                        indexFromGreenOriginalDataBegin = (indexOfBorder005 - 2) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    }
                    else
                    {
                        indexFromGreenOriginalDataBegin = (indexOfBorder005) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    }
                    int indexFromGreenOriginalDataEnd = (indexOfBorder005 + 2) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    List<double> fittingRelativeElongation = new List<double>();
                    List<double> fittingPreasssureInMPa = new List<double>();//from original data


                    for (int i = indexFromGreenOriginalDataBegin; i < indexFromGreenOriginalDataEnd; i++)
                    {
                        fittingRelativeElongation.Add(dataReader.RelativeElongation[i] - xTranslateAmount);
                        fittingPreasssureInMPa.Add(dataReader.PreassureInMPa[i]);
                    }

                    //most precise calculate Border005
                    int indexOfBorder005Precise = 0;
                    for (int i = 0; i < fittingRelativeElongation.Count; i++)
                    {
                        yIdeal = Border005K * fittingRelativeElongation[i] + Border005N;
                        ySubs = Math.Abs(yIdeal - fittingPreasssureInMPa[i]);
                        if (ySubs < minYSubs && fittingRelativeElongation[i] >= borderX + OptionsInPlottingMode.pointManualX3 - xTranslateAmount/*fittingPoints[i].XAxisValue >= borderX + OptionsInPlottingMode.pointManualX3 - xTranslateAmount*/)
                        {
                            minYSubs = ySubs;
                            indexOfBorder005Precise = i;
                            indexOfBorder005 = indexOfBorder005Precise;
                        }
                    }

                    //return fittingPoints[indexOfBorder005].YAxisValue;
                    //if (fittingPoints[indexOfBorder005Precise].XAxisValue < borderX + OptionsInPlottingMode.pointManualX3 - xTranslateAmount)
                    //{
                    //    Border005XValue = borderX + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;
                    //}
                    //else
                    //{
                    //    Border005XValue = fittingPoints[indexOfBorder005Precise].XAxisValue;
                    //}

                    Border005XValue = borderX + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;//probaj sa ovom fiksnom vrednoscu za X osu

                    return fittingPreasssureInMPa[indexOfBorder005Precise];

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double Border005(MyPointCollection fittingPoints, int indexOfPointClosestToRed, double xTranslateAmount, double borderX, out double Border005XValue, out int Border005index)}", System.DateTime.Now);
                Border005XValue = 0;
                Border005index = 0;
                return 0;
            }
        }

        private double Rp02(MyPointCollection fittingPoints, int indexOfPointClosestToRed, double xTranslateAmount, out double rp02XValue,out int Rp02index)
        {
            try
            {
                Rp02index = 0;
                rp02XValue = 0;
                int indexOfRp02 = 0;
                double xSubstractionForRp02 = Double.MaxValue;
                if (OptionsInPlottingMode.isManualFittingChecked)
                {
                    xSubstractionForRp02 = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                }
                else
                {
                    xSubstractionForRp02 = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                }


                if (xSubstractionForRp02 < 0.01 || xSubstractionForRp02 == 0.01)
                {

                    double minXSubs = Double.MaxValue;
                    double xSubs = -1;

                    for (int i = 0; i < fittingPoints.Count - 1; i++)
                    {
                        if (fittingPoints[i].XAxisValue > 1) break;
                        xSubs = Math.Abs(0.2 - fittingPoints[i].XAxisValue);
                        if (xSubs < minXSubs)
                        {
                            minXSubs = xSubs;
                            indexOfRp02 = i;
                        }
                    }
                    rp02XValue = fittingPoints[indexOfRp02].XAxisValue;
                    return fittingPoints[indexOfRp02].YAxisValue;
                }
                else
                {

                    double rp02K = currK;
                    double rp02N = -currK * 0.2;

                    //ako je k ipak beskonacno
                    if (Double.IsPositiveInfinity(rp02K) || Double.IsNegativeInfinity(rp02K))
                    {
                        double minXSubs = Double.MaxValue;
                        double xSubs = -1;

                        for (int i = 0; i < fittingPoints.Count - 1; i++)
                        {
                            if (fittingPoints[i].XAxisValue > 1) break;
                            xSubs = Math.Abs(0.2 - fittingPoints[i].XAxisValue);
                            if (xSubs < minXSubs)
                            {
                                minXSubs = xSubs;
                                indexOfRp02 = i;
                            }
                        }
                        rp02XValue = fittingPoints[indexOfRp02].XAxisValue;
                        return fittingPoints[indexOfRp02].YAxisValue;
                    }
                    //ako je k ipak beskonacno

                    double yIdeal = rp02K * 0 + rp02N;

                    double minYSubs = Double.MaxValue;
                    double ySubs = -1;
                    //int indexOfRp02 = -1;
                    for (int i = 0; i < fittingPoints.Count - 1; i++)
                    {
                        yIdeal = rp02K * fittingPoints[i].XAxisValue + rp02N;
                        ySubs = Math.Abs(yIdeal - fittingPoints[i].YAxisValue);
                        if (ySubs < minYSubs && fittingPoints[i].XAxisValue >= 0.2 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount /*&& fittingPoints[i].XAxisValue < 0.5*/)
                        {
                            minYSubs = ySubs;
                            indexOfRp02 = i;
                        }
                    }

                    int indexFromGreenOriginalDataBegin = Int32.MinValue;

                    if (indexOfRp02 > 2)
                    {
                        indexFromGreenOriginalDataBegin = (indexOfRp02 - 2) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    }
                    else
                    {
                        indexFromGreenOriginalDataBegin = (indexOfRp02) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    }
                    int indexFromGreenOriginalDataEnd = (indexOfRp02 + 2) * OptionsInPlottingMode.Resolution + indexOfPointClosestToRed;
                    List<double> fittingRelativeElongation = new List<double>();
                    List<double> fittingPreasssureInMPa = new List<double>();//from original data


                    for (int i = indexFromGreenOriginalDataBegin; i < indexFromGreenOriginalDataEnd; i++)
                    {
                        fittingRelativeElongation.Add(dataReader.RelativeElongation[i] - xTranslateAmount);
                        fittingPreasssureInMPa.Add(dataReader.PreassureInMPa[i]);
                    }

                    //most precise calculate Rp02
                    int indexOfRp02Precise = 0;
                    for (int i = 0; i < fittingRelativeElongation.Count; i++)
                    {
                        yIdeal = rp02K * fittingRelativeElongation[i] + rp02N;
                        ySubs = Math.Abs(yIdeal - fittingPreasssureInMPa[i]);
                        if (ySubs < minYSubs && fittingRelativeElongation[i] >= 0.2 /*fittingPoints[i].XAxisValue >= 0.2*/ + OptionsInPlottingMode.pointManualX3 - xTranslateAmount)
                        {
                            minYSubs = ySubs;
                            indexOfRp02Precise = i;
                            indexOfRp02 = indexOfRp02Precise;
                        }
                    }

                    //return fittingPoints[indexOfRp02].YAxisValue;
                    //if (fittingPoints[indexOfRp02Precise].XAxisValue < 0.2 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount)
                    //{
                    //    rp02XValue = 0.2 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;
                    //}
                    //else
                    //{
                    //    rp02XValue = fittingPoints[indexOfRp02Precise].XAxisValue;
                    //}

                    rp02XValue = 0.2 + OptionsInPlottingMode.pointManualX3 - xTranslateAmount;//probaj sa ovom fiksnom vrednoscu za X osu

                    return fittingPreasssureInMPa[indexOfRp02Precise];
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double Rp02(MyPointCollection fittingPoints, int indexOfPointClosestToRed, double xTranslateAmount, out double rp02XValue,out int Rp02index)}", System.DateTime.Now);
                rp02XValue = 0;
                Rp02index = 0;
                return 0;
            }
        }

        private void checkAndCorrectBegEndind(out int begind,out int endind,int beg,int end) 
        {
            try
            {
                begind = beg;
                endind = end;
                if (end >= dataReader.PreassureInMPa.Count)
                {
                    endind = dataReader.PreassureInMPa.Count - 1;
                }
                if (beg < 0)
                {
                    begind = 0;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void checkAndCorrectBegEndind(out int begind,out int endind,int beg,int end)}", System.DateTime.Now);
                begind = 0;
                endind = 0;
            }
        }

        private double getTheMostAccurateMax(int begind, int endind) 
        {
            try
            {
                checkAndCorrectBegEndind(out begind, out endind, begind, endind);

                double max = Double.MinValue;
                for (int i = begind; i <= endind; i++)
                {
                    if (dataReader.PreassureInMPa[i] > max)
                    {
                        max = dataReader.PreassureInMPa[i];
                        ReH = max;
                        indexReHFromoriginalData = i;
                        ReHXFromoriginalData = dataReader.RelativeElongation[i];
                    }
                }
                return max;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double getTheMostAccurateMax(int begind, int endind)}", System.DateTime.Now);
                return 0;
            }
        }


        private double getTheMostAccurateMinFalse(int begind, int endind) 
        {
            try
            {
                checkAndCorrectBegEndind(out begind, out endind, begind, endind);

                double min = Double.MaxValue;
                for (int i = begind; i <= endind; i++)
                {
                    if (dataReader.PreassureInMPa[i] < min)
                    {
                        min = dataReader.PreassureInMPa[i];
                        ReL = min;
                        indexReLFromoriginalData = i;
                        ReLXFromoriginalData = dataReader.RelativeElongation[i];
                    }
                }
                return min;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double getTheMostAccurateMinFalse(int begind, int endind))}", System.DateTime.Now);
                return 0;
            }
        }


        /// <summary>
        /// ako se smatra da je pravi ReL prvi lokalni minimum
        /// </summary>
        /// <param name="begind"></param>
        /// <param name="endind"></param>
        /// <returns></returns>
        private double getTheMostAccurateMinTrue(int begind, int endind, out int indexOfTrueReL)
        {
            try
            {
                indexOfTrueReL = 0;
                checkAndCorrectBegEndind(out begind, out endind, begind, endind);

                double min = Double.MaxValue;
                List<double> listForReLWithMinimumValue = new List<double>();

                for (int i = begind; i < endind; i++)
                {
                    double currValue;
                    currValue = dataReader.PreassureInMPa[i];
                    listForReLWithMinimumValue.Add(currValue);
                }

                min = listForReLWithMinimumValue.Min();
                indexOfTrueReL = begind + listForReLWithMinimumValue.IndexOf(min);
                ReLXFromoriginalData = dataReader.PreassureInMPa[indexOfTrueReL];

                return min;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double getTheMostAccurateMinTrue(int begind, int endind, out int indexOfTrueReL)}", System.DateTime.Now);
                indexOfTrueReL = 0;
                return 0;
            }
        }

        /// <summary>
        /// ako se smatra da je pravi ReL prvi lokalni maksimum
        /// </summary>
        /// <param name="begind"></param>
        /// <param name="endind"></param>
        /// <returns></returns>
        //private double getTheMostAccurateMinTrue2(int begind, int endind)
        //{
        //    checkAndCorrectBegEndind(out begind, out endind, begind, endind);

        //    double min = Double.MaxValue;
        //    double localMax = Double.MinValue;//ovde ide znak vece jel se trazi lokani maksimum koji u globalu predstavlja minimum
        //    for (int i = begind; i <= endind; i++)
        //    {
        //        if (dataReader.PreassureInMPa[i] > localMax) ///ovde ide znak vece jel se trazi lokani maksimum koji u globalu predstavlja minimum
        //        {
        //            localMax = dataReader.PreassureInMPa[i];
        //            min = localMax;
        //            ReL = min;
        //            indexReLFromoriginalData = i;
        //            ReLXFromoriginalData = dataReader.RelativeElongation[i];
        //        }
        //    }
        //    return min;
        //}

       

           private double getMaxMinFromOriginalData (out double min, out int indexOfmin, out int indexOfmax)
            {
                try
                {
                    indexOfmin = 0;
                    indexOfmax = 0;
                    double max = Double.MinValue;
                    double minFalse = Double.MaxValue;
                    min = Double.MaxValue;
                    double currentFallOfPreassure = 0;
                    double currentRiseOfPreassure = 0;
                    double yieldInterval = double.MinValue;
                    string strtfYield = optionsPlotting.tfYield.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfYield, out yieldInterval);

                    int indexFromMaximum = -1;
                    int indexFromFalseMinimum = -1;
                    int indexFromTrueMinimum = -1;

                    for (int i = 0; i < dataReader.PreassureInMPa.Count; i++)
                    {
                        if (i % OptionsInPlottingMode.DerivationResolution == 0)
                        {
                            if (i > 1)
                            {
                                if (dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution] <= 0 || (dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution] > 0 && dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution] < 0.02 * OptionsInPlottingMode.YieldInterval /*greska senzora*/))
                                {
                                    currentFallOfPreassure = currentFallOfPreassure + Math.Abs(dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution]);
                                }
                                else
                                {


                                    if (currentFallOfPreassure >= yieldInterval)
                                    {
                                        minFalse = max - currentFallOfPreassure;
                                        min = minFalse;
                                        ReL = min;
                                        indexOfmin = i;
                                        indexReLFromoriginalData = i;
                                        ReLXFromoriginalData = dataReader.RelativeElongation[indexReLFromoriginalData];
                                        indexFromFalseMinimum = i;
                                        //MessageBox.Show("MAX:   "   + max + " MIN:  " + minFalse );
                                        isFoundMaxAndMin = true;

                                        break;
                                    }
                                    else
                                    {
                                        max = dataReader.PreassureInMPa[i];
                                        ReH = max;
                                        indexOfmax = i;
                                        indexReHFromoriginalData = i;
                                        ReHXFromoriginalData = dataReader.RelativeElongation[indexReHFromoriginalData];
                                        indexFromMaximum = i;
                                        currentFallOfPreassure = 0;
                                    }
                                }
                            }
                        }
                    } // end of for loop

                    if (indexFromFalseMinimum != -1 && ((indexReHFromoriginalData < getRpmaxIndex() && indexReHFromoriginalData != 0 && ReH < 0.95 * Rm && ReH > 0) || (indexReLFromoriginalData < getRpmaxIndex() && indexReLFromoriginalData != 0 && ReL > 0)))
                    {
                        for (int i = indexFromFalseMinimum; i < dataReader.PreassureInMPa.Count; i++)
                        {
                            if (i % OptionsInPlottingMode.DerivationResolution == 0)
                            {
                                if (dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution] >= 0 || (dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution] < 0 && dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution] > -0.02 * OptionsInPlottingMode.YieldInterval /*greska senzora*/))
                                {
                                    currentRiseOfPreassure = currentRiseOfPreassure + Math.Abs(dataReader.PreassureInMPa[i] - dataReader.PreassureInMPa[i - OptionsInPlottingMode.DerivationResolution]);
                                }
                                else
                                {
                                    if (currentRiseOfPreassure < OptionsInPlottingMode.MaxSubBetweenReLAndReLF || currentRiseOfPreassure == OptionsInPlottingMode.MaxSubBetweenReLAndReLF)
                                    {
                                        min = dataReader.PreassureInMPa[i];
                                        ReL = min;
                                        indexOfmin = i;
                                        indexReLFromoriginalData = i;
                                        ReLXFromoriginalData = dataReader.RelativeElongation[indexReLFromoriginalData];
                                        indexFromTrueMinimum = i;

                                        //sada trazi prvi lokalni minimum
                                        for (int j = indexFromTrueMinimum; j < dataReader.PreassureInMPa.Count; j++)
                                        {
                                            if (dataReader.PreassureInMPa[j + OptionsInPlottingMode.DerivationResolution] - dataReader.PreassureInMPa[j] <= 0 /* || (dataReader.PreassureInMPa[j] - dataReader.PreassureInMPa[j - OptionsInPlottingMode.DerivationResolution] > 0 && dataReader.PreassureInMPa[j] - dataReader.PreassureInMPa[j - OptionsInPlottingMode.DerivationResolution] < 0.02 * OptionsInPlottingMode.YieldInterval /*greska senzora)*/)
                                            {

                                            }
                                            else
                                            {
                                                min = dataReader.PreassureInMPa[j];
                                                ReL = min;
                                                indexOfmin = j;
                                                indexReLFromoriginalData = j;
                                                ReLXFromoriginalData = dataReader.RelativeElongation[indexReLFromoriginalData];
                                                indexFromTrueMinimum = j;
                                                break;
                                            }
                                        }

                                        isFoundMinFalseAndMin = true;
                                        //MessageBox.Show( "true MIN:  " + min);
                                        break;
                                    }
                                    else
                                    {
                                        isFoundMinFalseAndMin = false;
                                        isFoundOnlyReLCase = false;
                                    }
                                }
                            }
                        } // end of for loop
                    }
                    else
                    {
                        isFoundMaxAndMin = false;
                        isFoundMinFalseAndMin = false;
                        isFoundOnlyReLCase = false;
                    }

                    if (isFoundMaxAndMin)
                    {
                        //nema puta nikakva rezolucija jel uimamo indeks originalnog podatka
                        int beg = (indexFromMaximum - 2 * OptionsInPlottingMode.DerivationResolution) * 1;
                        int end = (indexFromMaximum + 2 * OptionsInPlottingMode.DerivationResolution) * 1;
                        double themostaccurateMax = getTheMostAccurateMax(beg, end);
                        max = themostaccurateMax;

                        if (isFoundMinFalseAndMin == false)
                        {
                            int beg2 = (indexFromFalseMinimum - 2 * OptionsInPlottingMode.DerivationResolution) * 1;
                            int end2 = (indexFromFalseMinimum + 2 * OptionsInPlottingMode.DerivationResolution) * 1;
                            min = getTheMostAccurateMinFalse(beg2, end2);
                        }
                        else
                        {
                            int beg3 = (indexFromTrueMinimum - 2 * OptionsInPlottingMode.DerivationResolution) * 1;
                            int end3 = (indexFromTrueMinimum + 2 * OptionsInPlottingMode.DerivationResolution) * 1;
                            int indexOfTrueReL = 0;
                            min = getTheMostAccurateMinTrue(beg3, end3, out indexOfTrueReL);
                            ReL = min;
                            indexOfmin = indexOfTrueReL;
                            indexReLFromoriginalData = indexOfTrueReL;
                            ReLXFromoriginalData = dataReader.RelativeElongation[indexReLFromoriginalData];
                            indexFromTrueMinimum = indexOfTrueReL;
                        }

                        return themostaccurateMax;


                    }

                    return max;
                }
                catch (Exception ex)
                {
                    Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double getMaxMinFromOriginalData (out double min, out int indexOfmin, out int indexOfmax)}", System.DateTime.Now);
                    indexOfmin = 0;
                    indexOfmax = 0;
                    min = 0;
                    return 0;
                }
        }

        private double findMinReLOnly(out int indexOfmin)
        {
            try
            {
                if (rp02Index <= 0)
                {
                    indexOfmin = 0;
                    return Double.MaxValue;
                }



                indexOfmin = 0;
                List<double> listxDistance = new List<double>();
                List<double> listyValue = new List<double>();
                //u ovoj listi nalaze se samo dva elementa pocetni i krajni index prozora 
                List<int> Indexes = new List<int>();
                double currXdistance = 0;
                double sumYvalue = 0;
                double currYvalue = 0;
                double onlyReLPreassureUnitCounter = 1;

                int cnt = 0;
                int currentIndexRp02 = rp02Index;
                while (dataReader.FittingPreassureInMPa[currentIndexRp02] > Rp02RI - OptionsInPlottingMode.OnlyReLPreassureUnit)
                {
                    currentIndexRp02--;
                    if (currentIndexRp02 < 0)
                    {
                        return Double.MaxValue;
                    }
                }
                Indexes.Add(currentIndexRp02);
                currentIndexRp02 = rp02Index;
                while (dataReader.FittingPreassureInMPa[currentIndexRp02] < Rp02RI + OptionsInPlottingMode.OnlyReLPreassureUnit)
                {
                    currentIndexRp02++;
                    if (currentIndexRp02 > dataReader.FittingPreassureInMPa.Count - 1)
                    {
                        return Double.MaxValue;
                    }
                }
                Indexes.Add(currentIndexRp02);
                //ovde se nalazi indeksi izdeljeni po y osi
                //int indexOfRm = getRpmaxIndex();
                //for (int i = 0; i < indexOfRm; i++)
                //{
                //    if (    (dataReader.PreassureInMPa[i] > cnt * OptionsInPlottingMode.OnlyReLPreassureUnit || dataReader.PreassureInMPa[i] == cnt * OptionsInPlottingMode.OnlyReLPreassureUnit)   &&   (dataReader.PreassureInMPa[i] < (cnt + 1) * OptionsInPlottingMode.OnlyReLPreassureUnit || dataReader.PreassureInMPa[i] == (cnt + 1) * OptionsInPlottingMode.OnlyReLPreassureUnit))
                //    {

                //    }
                //    else
                //    {
                //        Indexes.Add(i);
                //        cnt++;
                //    }
                //}

                for (int i = 0; i < Indexes.Count - 1; i++)
                {
                    listxDistance.Add(dataReader.FittingRelativeElongation[Indexes[i + 1]] - dataReader.FittingRelativeElongation[Indexes[i]]);
                    //napon mora biti veci od napona tacke T2 pa se uzima gornja granica
                    listyValue.Add(dataReader.FittingPreassureInMPa[Indexes[i + 1]]);
                }

                if (listxDistance.Count == 0)
                {
                    indexOfmin = 0;
                    indexReLFromoriginalData = indexOfmin;
                    return Double.MaxValue;
                }

                List<double> onlyRelCandidates = new List<double>();//lista vrednosti kandidata koje mogu biti vrednosti za Rel (u slucaju kada postoji samo ReL)
                List<int> indexForOnlyRelCandidates = new List<int>();//indeksi gde se koji od kandidata nalazi u originalnom tekstualnom fajlu, na osnonu kog je crtan zeleni(nefitovani) grafik
                for (int i = 0; i < listxDistance.Count; i++)
                {
                    if ((listxDistance[i] > /*minimalne vrednosti po defaultu 0.05 * Xmax*/ OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent || listxDistance[i] == OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent) && (listyValue[i] < /*maksimalne vrednosti po defaultu 0.95 * Rpmax*/ OptionsInPlottingMode.MaxPossibleValueForOnlyReLPreassureInMPa || listyValue[i] == OptionsInPlottingMode.MaxPossibleValueForOnlyReLPreassureInMPa))
                    {
                        onlyRelCandidates.Add(listyValue[i]);
                        indexForOnlyRelCandidates.Add(i);
                        //isFoundOnlyReLCase = true;
                    }
                }

                double[] onlyRelCandidatesArray = onlyRelCandidates.ToArray();

                if (onlyRelCandidatesArray.Length > 0) //ako nema kandidata odmah izadji
                {
                    int i;
                    int firstIndex = -1, secondIndex = -1;
                    for (i = 0; i < onlyRelCandidatesArray.Count(); i++)
                    {
                        if (onlyRelCandidatesArray[i] > OptionsInPlottingMode.pointManualY2)
                        {
                            //indexOfmin = indexForOnlyRelCandidates[i]*OptionsInPlottingMode.DerivationResolution;
                            if (i + 1 > onlyRelCandidatesArray.Count() - 1)//ukoliko postoji samo jedan element kod velikih intervala onda postavi i krajnji i pocetni index na istu vrednost da bi se program ispravno zavrsio
                            {
                                //ove dve linije su bile validne dok je postojala lista prozora
                                //firstIndex = Indexes.ElementAt(indexForOnlyRelCandidates[i]);
                                //secondIndex = Indexes.ElementAt(indexForOnlyRelCandidates[i]);

                                //ovo vazi samo kada radimo sa jednim prozorom u cijoj je sredini Rp02
                                //sada uvek samo ovde ulazi jel se radi sa jednim prozorom
                                firstIndex = Indexes.ElementAt(0);
                                secondIndex = Indexes.ElementAt(1);
                                isFoundOnlyReLCase = true;
                                //IsClickedByMouse_Plotting_ReL = true;
                                IsClickedByMouse_Plotting_ReL = false;

                                break;
                            }
                            //ovde nikad ne ulazi jel se trenutno radi samo sa jednim prozorom u cijoj je sredini Rp02
                            isFoundOnlyReLCase = true;
                            IsClickedByMouse_Plotting_ReL = true;
                            firstIndex = Indexes.ElementAt(indexForOnlyRelCandidates[i]);
                            secondIndex = Indexes.ElementAt(indexForOnlyRelCandidates[i + 1]);
                            indexOfmin = 0;
                            indexReLFromoriginalData = indexOfmin;
                            break;
                        }
                    }

                    if (isFoundOnlyReLCase == true)
                    {

                        for (int j = firstIndex; j < secondIndex; /*j++*/)
                        {
                            int indexOfPreviousElement = j - OptionsInPlottingMode.DerivationResolution;
                            if (indexOfPreviousElement < 0)
                            {
                                indexOfPreviousElement = 0;
                            }
                            if (j > dataReader.FittingPreassureInMPa.Count - 1)
                            {
                                indexOfmin = firstIndex + (int)(secondIndex - firstIndex) / 2;
                                break;
                            }
                            if (dataReader.FittingPreassureInMPa[j] - dataReader.FittingPreassureInMPa[indexOfPreviousElement] > 0 || (dataReader.FittingPreassureInMPa[j] - dataReader.FittingPreassureInMPa[indexOfPreviousElement] < 0 && dataReader.FittingPreassureInMPa[j] - dataReader.FittingPreassureInMPa[indexOfPreviousElement] > (-1) * 0.02 * OptionsInPlottingMode.YieldInterval /*greska senzora*/)) // racunaj porast napona [ ako je vrednost napona narednog elementa veca od prethodnog onda je ustanovljen porast napona ]
                            {
                                //indexOfmin = 0;
                                indexOfmin = firstIndex + (int)(secondIndex - firstIndex) / 2;
                                indexReLFromoriginalData = indexOfmin;
                            }
                            else
                            {
                                //indexOfmin = j;
                                indexOfmin = firstIndex + (int)(secondIndex - firstIndex) / 2;
                                indexReLFromoriginalData = indexOfmin;
                                ReLXFromoriginalData = dataReader.FittingRelativeElongation[j] + xTranslateAmountFittingMode;

                                //ovo je vazno da se zapamti je sada su ovo zadnje zabelezene koordinate za marker ReL
                                string currInOutFileName = getCurrentInputOutputFile();
                                if (File.Exists(currInOutFileName) == true)
                                {
                                    double ReLY = dataReader.FittingPreassureInMPa[indexOfmin];
                                    double ReLX = dataReader.FittingRelativeElongation[indexOfmin];
                                    List<string> dataListInputOutput = new List<string>();
                                    dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                                    dataListInputOutput.Add(Constants.ReLManual + '\t' + ReLY + '\t' + Constants.MPa + '\t' + Constants.ReLX + '\t' + ReLX + '\t' + Constants.procent);
                                    File.Delete(currInOutFileName);
                                    File.WriteAllLines(currInOutFileName, dataListInputOutput);
                                }

                                //postavi vrednosti koje ce se koristiti za racunanje posle promene Lu-a
                                _reL_X = dataReader.FittingRelativeElongation[indexOfmin];

                                //postavi vrednosti markera za print screen
                                printscreen.ReLX = dataReader.FittingRelativeElongation[indexOfmin];
                                printscreen.ReLY = dataReader.FittingPreassureInMPa[indexOfmin];
                                this.setReLPoint(printscreen.ReLX, printscreen.ReLY);
                                printscreen.SetReLPointCalculated(printscreen.ReLX, printscreen.ReLY);
                                return dataReader.FittingPreassureInMPa[indexOfmin];
                            }
                            j = j + OptionsInPlottingMode.DerivationResolution;
                            if (j > dataReader.FittingPreassureInMPa.Count - 1)
                            {
                                j = dataReader.FittingPreassureInMPa.Count - 1;
                            }
                        }
                        //indexOfmin = 0;
                        indexOfmin = firstIndex + (int)(secondIndex - firstIndex) / 2;
                        indexReLFromoriginalData = indexOfmin;
                        //return Double.MaxValue;

                        //ovo je vazno da se zapamti je sada su ovo zadnje zabelezene koordinate za marker ReL
                        string currInOutFileName2 = getCurrentInputOutputFile();
                        if (File.Exists(currInOutFileName2) == true)
                        {
                            double ReLY = dataReader.FittingPreassureInMPa[indexOfmin];
                            double ReLX = dataReader.FittingRelativeElongation[indexOfmin];
                            List<string> dataListInputOutput = new List<string>();
                            dataListInputOutput = File.ReadAllLines(currInOutFileName2).ToList();
                            dataListInputOutput.Add(Constants.ReLManual + '\t' + ReLY + '\t' + Constants.MPa + '\t' + Constants.ReLX + '\t' + ReLX + '\t' + Constants.procent);
                            File.Delete(currInOutFileName2);
                            File.WriteAllLines(currInOutFileName2, dataListInputOutput);
                        }

                        //postavi vrednosti koje ce se koristiti za racunanje posle promene Lu-a
                        _reL_X = dataReader.FittingRelativeElongation[indexOfmin];

                        //postavi vrednosti markera za print screen
                        printscreen.ReLX = dataReader.FittingRelativeElongation[indexOfmin];
                        printscreen.ReLY = dataReader.FittingPreassureInMPa[indexOfmin];
                        this.setReLPoint(printscreen.ReLX, printscreen.ReLY);
                        printscreen.SetReLPointCalculated(printscreen.ReLX, printscreen.ReLY);
                        return dataReader.FittingPreassureInMPa[indexOfmin];
                    }
                    else
                    {
                        indexOfmin = 0;
                        indexReLFromoriginalData = indexOfmin;
                        return Double.MaxValue;
                    }
                    //indexOfmin = 0;
                    //indexReLFromoriginalData = indexOfmin;

                }
                else
                {
                    indexOfmin = 0;
                    indexReLFromoriginalData = indexOfmin;
                    return Double.MaxValue;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double findMinReLOnly(out int indexOfmin)}", System.DateTime.Now);
                indexOfmin = 0;
                return 0;
            }
        }

     

        private double MaxMinReHReL (MyPointCollection points, out double min,out int indexOfmin,out int indexOfmax)
        {
            try
            {
                double max = Double.MinValue;

                max = getMaxMinFromOriginalData(out min, out indexOfmin, out indexOfmax);
                reHIndex = indexOfmax;
                reLIndex = indexOfmin;
                return max;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private double MaxMinReHReL (MyPointCollection points, out double min,out int indexOfmin,out int indexOfmax)}", System.DateTime.Now);
                min = 0;
                indexOfmin = 0;
                indexOfmax = 0;
                return 0;
            }

        }

        private void btnTurnOnFitting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isGraphicPlottingLoaded == false)
                {
                    MessageBox.Show("Morate prvo učitati grafik sa originalnim podacima!");
                    return;
                }
                drawFittingGraphic(T3movingDirectionByYAxis, 3);
                calcuateOnlyFittingParameters_Rp02();
                optionsPlotting.chbShowOriginalData.IsEnabled = false;

                btnCalculateTotalRelElong.IsEnabled = true;



                //MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                //if (OptionsInPlottingMode.isShowOriginalDataGraphic == true)
                //{
                //    deleteOfflineModeOnly();

                //    _MarkerGraph.DataSource = null;
                //    _MarkerGraph2.DataSource = null;
                //    _MarkerGraph3.DataSource = null;

                //    _MarkerGraphText.DataSource = null;
                //    _MarkerGraphText2.DataSource = null;
                //    _MarkerGraphText3.DataSource = null;
                //}

                //window.Plotting.plotter.SaveScreenshot(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");

                //PDFSampleReport pdfSampleReport = new PDFSampleReport(11, 8.5);
                //pdfSampleReport.CreateReport();
                //File.Delete(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");


                //if (OptionsInPlottingMode.isShowOriginalDataGraphic == true)
                //{
                //    // window.Plotting.btnPlottingModeClick();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void btnTurnOnFitting_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


       

        #region calculateTotalRelativeElongation

        private void calculateTotalRelElongManual(MyPoint pointOfTearing, out double A)
        {
            try
            {
                A = Double.MinValue;


                if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                {
                    //find total relative elongation in procent
                    double totalRelativeElongation = pointOfTearing.XAxisValue;
                    A = totalRelativeElongation;

                    lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();
                }
                else
                {
                    //find new N
                    double KTearingLine = currK;
                    double NTearingLine = pointOfTearing.YAxisValue - KTearingLine * pointOfTearing.XAxisValue;

                    //find total relative elongation in procent
                    double totalRelativeElongation = -(NTearingLine / KTearingLine);
                    A = totalRelativeElongation;

                    lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();
                }

                A = Math.Round(A, 1);
                if (isAActive == true)
                {
                    AManualClickedValue = Math.Round(AManualClickedValue, 2);
                    tfA.Text = AManualClickedValue.ToString();
                    a = AManualClickedValue;
                }
                else
                {
                    tfA.Text = A.ToString();
                    a = A;
                }

                chbAVisibility.IsEnabled = true;
                chbAVisibility.IsChecked = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateTotalRelElongManual(MyPoint pointOfTearing, out double A)}", System.DateTime.Now);
                A = 0;
            }

        }

        private void calculateTotalRelElong(out MyPoint pointOfTearing, out double A)
        {
            try
            {
                A = Double.MinValue;
                pointOfTearing = new MyPoint(0, 0);

                this.indexEnd = dataReader.RelativeElongation.Count;

                if (OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja == true)
                {
                    //find tearing point
                    double firstDistanceInMPa = 0.0;
                    double secondDistanceInMPa = 0.0;
                    MyPoint pointTearing = new MyPoint(0, 0);

                    List<string> distances = new List<string>();

                    //only for testing purpose. THIS MUST BE DELETED OR COMMENTED
                    //double firstDistance = 0.0;
                    //double secondDistance = 0.0;
                    //for (int i = pointsOfFittingLine.Count - 1; i > 0; i--)
                    //{
                    //    firstDistance = pointsOfFittingLine[i - 1].YAxisValue - pointsOfFittingLine[i].YAxisValue;
                    //    secondDistance = pointsOfFittingLine[i - 2].YAxisValue - pointsOfFittingLine[i - 1].YAxisValue;
                    //    if (secondDistance * OptionsInPlottingMode.TearingPointCoeff < firstDistance)
                    //    {
                    //        int cnt = 1;
                    //        if (pointsOfFittingLine[i - 2].YAxisValue - pointsOfFittingLine[i - 1].YAxisValue > OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama*/)
                    //        {
                    //            pointTearing = pointsOfFittingLine[i - 1];
                    //            pointOfTearing = pointTearing;
                    //        }
                    //        else
                    //        {
                    //            while (pointsOfFittingLine[i - (cnt + 1)].YAxisValue - pointsOfFittingLine[i - 1].YAxisValue < OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama*/)
                    //            {
                    //                cnt++;
                    //                if ((cnt + 1) > 0.9 * pointsOfFittingLine.Count)
                    //                {
                    //                    MessageBox.Show("Postavili ste preveliki minimalan pad kod kidanja!" + System.Environment.NewLine + "Molimo vas smanjite minimalan pad kod kidanja, jer dobijeni rezultat nije tacan!!!");
                    //                    break;
                    //                }
                    //            }
                    //            cnt--;
                    //            pointTearing = pointsOfFittingLine[i - (cnt + 1)];
                    //            pointOfTearing = pointTearing;
                    //        }

                    //        break;
                    //    }
                    //    distances.Add(firstDistance.ToString());
                    //}
                    //only for testing purpose. THIS MUST BE DELETED OR COMMENTED


                    /**     THIS IS FOR PRODUCTION - FOR LOOP    **/
                    for (int i = dataReader.PreassureInMPa.Count - 1; i > 0; i--)
                    {
                        if (i < 0 || i - 1 * OptionsInPlottingMode.ResolutionForTearing < 0 || i - 2 * OptionsInPlottingMode.ResolutionForTearing < 0)
                        {
                            //upisi u log [private void calculateTotalRelElong(out MyPoint pointOfTearing, out double A)] {if (i < 0 || i - 1 * OptionsInPlottingMode.ResolutionForTearing < 0 || i - 2 * OptionsInPlottingMode.ResolutionForTearing < 0)}
                            Logger.WriteNode("[private void calculateTotalRelElong(out MyPoint pointOfTearing, out double A)] {if (i < 0 || i - 1 * OptionsInPlottingMode.ResolutionForTearing < 0 || i - 2 * OptionsInPlottingMode.ResolutionForTearing < 0)}", System.DateTime.Now);
                            break;
                        }
                        if (i % OptionsInPlottingMode.ResolutionForTearing == 0)
                        {
                            firstDistanceInMPa = dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i];
                            secondDistanceInMPa = dataReader.PreassureInMPa[i - 2 * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing];

                            if (secondDistanceInMPa * OptionsInPlottingMode.TearingPointCoeff < firstDistanceInMPa)
                            {
                                int cnt = 1;
                                if (Math.Abs(dataReader.PreassureInMPa[i - 2 * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing]) > OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama koja mora biti veca od greske senzora*/)
                                {
                                    pointTearing = new MyPoint(dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing], dataReader.RelativeElongation[i - 1 * OptionsInPlottingMode.ResolutionForTearing] - xTranslateAmountFittingMode);
                                    pointOfTearing = pointTearing;
                                }
                                else
                                {
                                    while (Math.Abs(dataReader.PreassureInMPa[i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing]) < OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama koja mora biti veca od greske senzora*/)
                                    {
                                        cnt++;
                                        if ((cnt + 1) * OptionsInPlottingMode.ResolutionForTearing > 0.9 * dataReader.PreassureInMPa.Count)
                                        {
                                            MessageBox.Show("Postavili ste preveliki minimalan pad kod kidanja!" + System.Environment.NewLine + "Molimo vas smanjite minimalan pad kod kidanja, jer dobijeni rezultat nije tacan!!!");
                                            break;
                                        }
                                    }
                                    cnt--;
                                    pointTearing = new MyPoint(dataReader.PreassureInMPa[i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing], dataReader.RelativeElongation[i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing] - xTranslateAmountFittingMode);
                                    pointOfTearing = pointTearing;
                                    this.indexEnd = i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing;
                                }
                                break;
                            }
                            distances.Add(firstDistanceInMPa.ToString());
                        }
                    }
                    /**     THIS IS FOR PRODUCTION - FOR LOOP    **/

                    //File.WriteAllLines(System.Environment.CurrentDirectory + "\\Distances.txt", distances);

                    double xSubstraction;
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                    }
                    else
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                    }

                    if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                    {
                        //find total relative elongation in procent
                        double totalRelativeElongation = pointTearing.XAxisValue;
                        A = totalRelativeElongation;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);

                    }
                    else
                    {
                        //find new N
                        double KTearingLine = currK;
                        double NTearingLine = pointTearing.YAxisValue - KTearingLine * pointTearing.XAxisValue;

                        //find total relative elongation in procent
                        double totalRelativeElongation = -(NTearingLine / KTearingLine);
                        A = totalRelativeElongation;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);
                    }
                }
                else
                {
                    //MessageBox.Show("Treba implementirati podrazumevani mod kidanja!");

                    //find tearing point
                    double currSubsDistanceInMPa = 0.0;
                    double minSubsInMPa = Double.MaxValue;
                    MyPoint pointTearing = new MyPoint(0, 0);
                    List<double> subsFromDefaultInRpmaxInProcent = new List<double>();//koliko se razlikuju naponi od postavljenog napona 

                    double rpmax = Double.MinValue;
                    setRpmaxAndFmax(out rpmax);

                    // provera se vrsi od mesta gde se dogodio maksimum pa sve do kraja i tada je tacka kidanja ona koja je najbliza npr 80(ovo se podesi u opcijama) vrednosti rpmax-u
                    // ulazni podaci su iz originalnog tekstualnog fajla
                    for (int i = getRpmaxIndex(); i < dataReader.PreassureInMPa.Count; i++)
                    {
                        if (i == 0)
                        {
                            continue;
                        }
                        currSubsDistanceInMPa = Math.Abs(OptionsInPlottingMode.DefaultPreassureOfTearingInProcent / 100 * rpmax - dataReader.PreassureInMPa[i]);
                        subsFromDefaultInRpmaxInProcent.Add(currSubsDistanceInMPa);
                        int cnt = 1;
                        if (currSubsDistanceInMPa < minSubsInMPa)
                        {
                            pointTearing = new MyPoint(dataReader.PreassureInMPa[i - 1], dataReader.RelativeElongation[i - 1] - xTranslateAmountFittingMode);
                            pointOfTearing = pointTearing;
                            minSubsInMPa = currSubsDistanceInMPa;


                            if (i - (cnt + 1) < 0 || (i - 1) < 0)
                            {
                                continue;
                            }
                            while (Math.Abs(dataReader.PreassureInMPa[i - (cnt + 1)] - dataReader.PreassureInMPa[i - 1]) < OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama koja mora biti veca od greske senzora*/)
                            {
                                cnt++;
                                if ((cnt + 1) > 0.9 * dataReader.PreassureInMPa.Count)
                                {
                                    MessageBox.Show("Postavili ste preveliki minimalan pad kod kidanja!" + System.Environment.NewLine + "Molimo vas smanjite minimalan pad kod kidanja, jer dobijeni rezultat ne bi bio tacan!!!");
                                    break;
                                }
                            }
                            cnt--;

                            pointTearing = new MyPoint(dataReader.PreassureInMPa[i - (cnt + 1)], dataReader.RelativeElongation[i - (cnt + 1)] - xTranslateAmountFittingMode);
                            pointOfTearing = pointTearing;
                            this.indexEnd = i - (cnt + 1);
                        }
                        if ((cnt + 2) > 0.9 * dataReader.PreassureInMPa.Count)
                        {
                            if (A < 0)
                            {
                                A = 0;
                            }
                            return;
                        }
                    }//end of FOR LOOP



                    double xSubstraction;
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                    }
                    else
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                    }

                    if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                    {
                        //find total relative elongation in procent
                        double totalRelativeElongation = pointTearing.XAxisValue;
                        A = totalRelativeElongation;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);

                    }
                    else
                    {
                        //find new N
                        double KTearingLine = currK;
                        double NTearingLine = pointTearing.YAxisValue - KTearingLine * pointTearing.XAxisValue;

                        //find total relative elongation in procent
                        double totalRelativeElongation = -(NTearingLine / KTearingLine);
                        A = totalRelativeElongation;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);
                    }
                }

                if (A < 0)
                {
                    A = 0;
                }
                A = Math.Round(A, 1);
                if (isAActive == true)
                {
                    AManualClickedValue = Math.Round(AManualClickedValue, 2);
                    tfA.Text = AManualClickedValue.ToString();
                    a = AManualClickedValue;
                }
                else
                {
                    tfA.Text = A.ToString();
                    a = A;
                }

                chbAVisibility.IsEnabled = true;
                chbAVisibility.IsChecked = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void calculateTotalRelElong(out MyPoint pointOfTearing, out double A)}", System.DateTime.Now);
                A = 0;
                pointOfTearing = new MyPoint(0, 0);
            }

        }

        private void btnCalculateTotalRelElong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double A = Double.MinValue;

                if (OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja == true)
                {
                    //find tearing point
                    double firstDistanceInMPa = 0.0;
                    double secondDistanceInMPa = 0.0;
                    MyPoint pointTearing = new MyPoint(0, 0);

                    List<string> distances = new List<string>();

                    //only for testing purpose. THIS MUST BE DELETED OR COMMENTED
                    //double firstDistance = 0.0;
                    //double secondDistance = 0.0;
                    //for (int i = pointsOfFittingLine.Count - 1; i > 0; i--)
                    //{
                    //    firstDistance = pointsOfFittingLine[i - 1].YAxisValue - pointsOfFittingLine[i].YAxisValue;
                    //    secondDistance = pointsOfFittingLine[i - 2].YAxisValue - pointsOfFittingLine[i - 1].YAxisValue;
                    //    if (secondDistance * OptionsInPlottingMode.TearingPointCoeff < firstDistance)
                    //    {
                    //        int cnt = 1;
                    //        if (pointsOfFittingLine[i - 2].YAxisValue - pointsOfFittingLine[i - 1].YAxisValue > OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama*/)
                    //        {
                    //            pointTearing = pointsOfFittingLine[i - 1];
                    //        }
                    //        else
                    //        {
                    //            while (pointsOfFittingLine[i - (cnt + 1)].YAxisValue - pointsOfFittingLine[i - 1].YAxisValue < OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama*/)
                    //            {
                    //                cnt++;
                    //                if ((cnt + 1) > 0.9 * pointsOfFittingLine.Count)
                    //                {
                    //                    MessageBox.Show("Postavili ste preveliki minimalan pad kod kidanja!" + System.Environment.NewLine + "Molimo vas smanjite minimalan pad kod kidanja, jer dobijeni rezultat nije tacan!!!");
                    //                    break;
                    //                }
                    //            }
                    //            cnt--;
                    //            pointTearing = pointsOfFittingLine[i - (cnt + 1)];
                    //        }

                    //        break;
                    //    }
                    //    distances.Add(firstDistance.ToString());
                    //}
                    //only for testing purpose. THIS MUST BE DELETED OR COMMENTED


                    /**     THIS IS FOR PRODUCTION - FOR LOOP    **/
                    for (int i = dataReader.PreassureInMPa.Count - 1; i > 0; i--)
                    {
                        if (i % OptionsInPlottingMode.ResolutionForTearing == 0)
                        {
                            firstDistanceInMPa = dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i];
                            secondDistanceInMPa = dataReader.PreassureInMPa[i - 2 * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing];

                            if (secondDistanceInMPa * OptionsInPlottingMode.TearingPointCoeff < firstDistanceInMPa)
                            {
                                int cnt = 1;
                                if (Math.Abs(dataReader.PreassureInMPa[i - 2 * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing]) > OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama koja mora biti veca od greske senzora*/)
                                {
                                    pointTearing = new MyPoint(dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing], dataReader.RelativeElongation[i - 1 * OptionsInPlottingMode.ResolutionForTearing] - xTranslateAmountFittingMode);
                                }
                                else
                                {
                                    while (Math.Abs(dataReader.PreassureInMPa[i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing] - dataReader.PreassureInMPa[i - 1 * OptionsInPlottingMode.ResolutionForTearing]) < OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama koja mora biti veca od greske senzora*/)
                                    {
                                        cnt++;
                                        if ((cnt + 1) * OptionsInPlottingMode.ResolutionForTearing > 0.9 * dataReader.PreassureInMPa.Count)
                                        {
                                            MessageBox.Show("Postavili ste preveliki minimalan pad kod kidanja!" + System.Environment.NewLine + "Molimo vas smanjite minimalan pad kod kidanja, jer dobijeni rezultat nije tacan!!!");
                                            break;
                                        }
                                    }
                                    cnt--;
                                    pointTearing = new MyPoint(dataReader.PreassureInMPa[i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing], dataReader.RelativeElongation[i - (cnt + 1) * OptionsInPlottingMode.ResolutionForTearing] - xTranslateAmountFittingMode);
                                }
                                break;
                            }
                            distances.Add(firstDistanceInMPa.ToString());
                        }
                    }
                    /**     THIS IS FOR PRODUCTION - FOR LOOP    **/

                    //File.WriteAllLines(System.Environment.CurrentDirectory + "\\Distances.txt", distances);

                    double xSubstraction;
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                    }
                    else
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                    }

                    if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                    {
                        //find total relative elongation in procent
                        double totalRelativeElongation = pointTearing.XAxisValue;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);

                    }
                    else
                    {
                        //find new N
                        double KTearingLine = currK;
                        double NTearingLine = pointTearing.YAxisValue - KTearingLine * pointTearing.XAxisValue;

                        //find total relative elongation in procent
                        double totalRelativeElongation = -(NTearingLine / KTearingLine);
                        A = totalRelativeElongation;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);
                    }
                }
                else
                {
                    //MessageBox.Show("Treba implementirati podrazumevani mod kidanja!");

                    //find tearing point
                    double currSubsDistanceInMPa = 0.0;
                    double minSubsInMPa = Double.MaxValue;
                    MyPoint pointTearing = new MyPoint(0, 0);
                    List<double> subsFromDefaultInRpmaxInProcent = new List<double>();//koliko se razlikuju naponi od postavljenog napona 

                    double rpmax = Double.MinValue;
                    setRpmaxAndFmax(out rpmax);

                    // provera se vrsi od mesta gde se dogodio maksimum pa sve do kraja i tada je tacka kidanja ona koja je najbliza npr 80(ovo se podesi u opcijama) vrednosti rpmax-u
                    // ulazni podaci su iz originalnog tekstualnog fajla
                    for (int i = getRpmaxIndex(); i < dataReader.PreassureInMPa.Count; i++)
                    {
                        currSubsDistanceInMPa = Math.Abs(OptionsInPlottingMode.DefaultPreassureOfTearingInProcent / 100 * rpmax - dataReader.PreassureInMPa[i]);
                        subsFromDefaultInRpmaxInProcent.Add(currSubsDistanceInMPa);
                        int cnt = 1;
                        if (currSubsDistanceInMPa < minSubsInMPa)
                        {
                            pointTearing = new MyPoint(dataReader.PreassureInMPa[i - 1], dataReader.RelativeElongation[i - 1] - xTranslateAmountFittingMode);
                            minSubsInMPa = currSubsDistanceInMPa;



                            while (Math.Abs(dataReader.PreassureInMPa[i - (cnt + 1)] - dataReader.PreassureInMPa[i - 1]) < OptionsInPlottingMode.TearingMinFallPreassure /*lazna tacka podesavace se u opcijama koja mora biti veca od greske senzora*/)
                            {
                                cnt++;
                                if ((cnt + 1) > 0.9 * dataReader.PreassureInMPa.Count)
                                {
                                    MessageBox.Show("Postavili ste preveliki minimalan pad kod kidanja!" + System.Environment.NewLine + "Molimo vas smanjite minimalan pad kod kidanja, jer dobijeni rezultat ne bi bio tacan!!!");
                                    break;
                                }
                            }
                            cnt--;

                            pointTearing = new MyPoint(dataReader.PreassureInMPa[i - (cnt + 1)], dataReader.RelativeElongation[i - (cnt + 1)] - xTranslateAmountFittingMode);
                        }
                        if ((cnt + 2) > 0.9 * dataReader.PreassureInMPa.Count)
                        {
                            return;
                        }
                    }//end of FOR LOOP



                    double xSubstraction;
                    if (OptionsInPlottingMode.isAutoFittingChecked)
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointAutoX1 - OptionsInPlottingMode.pointAutoX2);
                    }
                    else
                    {
                        xSubstraction = Math.Abs(OptionsInPlottingMode.pointManualX1 - OptionsInPlottingMode.pointManualX2);
                    }

                    if (Double.IsPositiveInfinity(currK) || Double.IsNegativeInfinity(currK))
                    {
                        //find total relative elongation in procent
                        double totalRelativeElongation = pointTearing.XAxisValue;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);

                    }
                    else
                    {
                        //find new N
                        double KTearingLine = currK;
                        double NTearingLine = pointTearing.YAxisValue - KTearingLine * pointTearing.XAxisValue;

                        //find total relative elongation in procent
                        double totalRelativeElongation = -(NTearingLine / KTearingLine);
                        A = totalRelativeElongation;

                        lblCalculateTotalRelElong.Text = Constants.CALCULATETOTALRELATIVEELONGATION + "  " + totalRelativeElongation.ToString();

                        // draw line which determine total elongation
                        //MyPoint pAtXAxis = new MyPoint(0, totalRelativeElongation);
                        //ConnectTwoDiscreteDisplays(pointTearing, pAtXAxis);
                    }
                }

                A = Math.Round(A, 1);
                if (isAActive == true)
                {
                    AManualClickedValue = Math.Round(AManualClickedValue, 2);
                    tfA.Text = AManualClickedValue.ToString();
                    a = AManualClickedValue;
                }
                else
                {
                    tfA.Text = A.ToString();
                    a = A;
                }

                chbAVisibility.IsEnabled = true;
                chbAVisibility.IsChecked = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void btnCalculateTotalRelElong_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


     

       

      

        #endregion

    

        private void btnShowOfflineOptions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (OptionsPlotting.isCreatedOptionsPlotting)
                {
                    MessageBox.Show("Otvoren je prozor opcija u offline modu!");
                    return;
                }
                optionsPlotting = new OptionsPlotting();
                OptionsPlotting.isCreatedOptionsPlotting = true;
                optionsPlotting.GraphicPlotting = this;
                LoadPlottingoptions();
                setRadioButtons();
                //chbFititngManualMode.IsChecked = false;

                //optionsPlotting.tfTearingMinFallPreassure.Text = "" + System.Environment.NewLine + "";

                if (onMode.OptionsOnline.chbIsCalibration.IsChecked == true || OptionsInOnlineMode.isCalibration == true)
                {
                    optionsPlotting.tfCalForceDivide.IsReadOnly = false;
                    optionsPlotting.tfCalForceMultiple.IsReadOnly = false;
                    optionsPlotting.tfCalElonMultiple.IsReadOnly = false;
                    optionsPlotting.tfCalElonDivide.IsReadOnly = false;
                    optionsPlotting.tfCalElonMultiple2.IsReadOnly = false;
                    optionsPlotting.tfCalElonDivide2.IsReadOnly = false;
                }
                else
                {
                    optionsPlotting.tfCalForceDivide.IsReadOnly = true;
                    optionsPlotting.tfCalForceMultiple.IsReadOnly = true;
                    optionsPlotting.tfCalElonMultiple.IsReadOnly = true;
                    optionsPlotting.tfCalElonDivide.IsReadOnly = true;
                    optionsPlotting.tfCalElonMultiple2.IsReadOnly = true;
                    optionsPlotting.tfCalElonDivide2.IsReadOnly = true;
                }

                optionsPlotting.Show();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void btnShowOfflineOptions_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }



        #region PrikazPonovoPoslePromeneRazmere

        private void chbPrikaziOriginalAfterRatioChanging_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging = true;
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbPrikaziOriginalAfterRatioChanging_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbPrikaziOriginalAfterRatioChanging_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.PrikaziOriginalAfterRatioChanging = false;
                writeXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbPrikaziOriginalAfterRatioChanging_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbPrikaziFitovaniAfterRatioChanging_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging = true;
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbPrikaziFitovaniAfterRatioChanging_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbPrikaziFitovaniAfterRatioChanging_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.PrikaziFitovaniAfterRatioChanging = false;
                writeXMLFileOffline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbPrikaziFitovaniAfterRatioChanging_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        private void plotter_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    this.drawFittingGraphic(tfFilepathPlotting.Text);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void plotter_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #region setTriangleMarkersAndTextMarkersfrom5to10


        //private void setTextMarkert8()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(xMarkersText8);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(yMarkersText8);
        //    _MarkerGraphText8.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    xMarkersText8[0] = xMarkers8[0] + OptionsInPlottingMode.xRange / 120;
        //    yMarkersText8[0] = yMarkers8[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "A";
        //    _MarkerGraphText8.Marker = mkrText;
        //}


        ///// <summary>
        ///// postavljanje Ag (automatski i rucni mod) samo vrednost relativnog izduzenja
        ///// </summary>
        //private void setAgPoint(double Ag)
        //{


        //    EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers10);
        //    EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers10);
        //    _MarkerGraph10.DataSource = new CompositeDataSource(gX, gY);

        //    //no scaling - identity mapping
        //    gX.XMapping = xx => xx;
        //    gY.YMapping = yy => yy;


        //    xMarkers10[0] = Ag;
        //    mkrTriangleCurrentValues.AXValue = xMarkers10[0];
        //    _ag_X = xMarkers10[0];
           
        //    double rpmax;
        //    setRpmaxAndFmax(out rpmax);
        //    yMarkers10[0] = 0.020 * rpmax;
        //    mkrTriangleCurrentValues.AYValue = yMarkers10[0];
         



        //    CirclePointMarker mkr = new CirclePointMarker();
        //    mkr.Fill = new SolidColorBrush(Colors.Brown);
        //    mkr.Size = Constants.MARKERSIZE;
        //    mkr.Pen = new Pen(new SolidColorBrush(Colors.Brown), 1);
        //    _MarkerGraph10.Marker = mkr;

        //    //setTextMarkert10();
        //}


        public void SetAPoint_Manual(double A)
        {
            try
            {
                setAPoint_Manual(A);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetAPoint_Manual(double A)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje A (rucni mod) samo vrednost relativnog izduzenja
        /// </summary>
        private void setAPoint_Manual(double A)
        {

            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers8);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers8);
                _MarkerGraph8.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers8[0] = A;
                mkrTriangleCurrentValues.AXValue = xMarkers8[0];
                _a_X = xMarkers8[0];
                if (OptionsInPlottingMode.isAutoChecked)
                {
                    double rpmax;
                    setRpmaxAndFmax(out rpmax);
                    yMarkers8[0] = 0.020 * rpmax;
                    mkrTriangleCurrentValues.AYValue = yMarkers8[0];
                }
                else
                {
                    yMarkers8[0] = 0.020 * OptionsInPlottingMode.yRange;
                    mkrTriangleCurrentValues.AYValue = yMarkers8[0];
                }



                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Red), 1);
                _MarkerGraph8.Marker = mkr;

                printscreen.AX = xMarkers8[0];
                printscreen.AY = yMarkers8[0];
                isClickedByMouse_Plotting_A = true;
                //IsLuManualChanged = false;
                //setTextMarkert8();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setAPoint_Manual(double A)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje A (automatski i rucni mod) samo vrednost relativnog izduzenja
        /// </summary>
        private void setAPoint(double A)
        {

            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers8);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers8);
                _MarkerGraph8.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers8[0] = A;
                mkrTriangleCurrentValues.AXValue = xMarkers8[0];
                _a_X = xMarkers8[0];
                if (OptionsInPlottingMode.isAutoChecked)
                {
                    double rpmax;
                    setRpmaxAndFmax(out rpmax);
                    yMarkers8[0] = 0.020 * rpmax;
                    mkrTriangleCurrentValues.AYValue = yMarkers8[0];
                }
                else
                {
                    yMarkers8[0] = 0.020 * OptionsInPlottingMode.yRange;
                    mkrTriangleCurrentValues.AYValue = yMarkers8[0];
                }



                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Red), 1);
                _MarkerGraph8.Marker = mkr;

                //setTextMarkert8();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setAPoint(double A)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje Rp02 (rucni mod) van grafika plave boje
        /// </summary>
        private void setRp02PointOutsideGraphic(double x, double y)
        {

            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers7[0] = x;
                mkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                yMarkers7[0] = y;
                mkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Violet);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Violet), 1);
                _MarkerGraph7.Marker = mkr;

                //setTextMarkert7();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRp02PointOutsideGraphic(double x, double y)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje Rp02 (rucni mod)
        /// </summary>
        public void SetRp02Point(double x, double y)
        {
            try
            {
                setRp02Point(x, y);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetRp02Point(double x, double y)}", System.DateTime.Now);
            }
        }



        /// <summary>
        /// postavljanje Rp02 (rucni mod)
        /// </summary>
        private void setRp02Point(double x, double y)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers7[0] = x;
                mkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                yMarkers7[0] = y;
                mkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Violet);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Violet), 1);
                _MarkerGraph7.Marker = mkr;


                printscreen.Rp02X = x;
                printscreen.Rp02Y = y;
                isClickedByMouse_Plotting_Rp02 = true;
                //IsLuManualChanged = false;
                //setTextMarkert7();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRp02Point(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// na kraju drawFittingGraphic procedure proracunat Rp02 postavi na osnovu x i y
        /// (automatski mod)
        /// ovo treba samo jedanput da se poziva u programu kada se uradi Find References
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void setRp02PointCalculated(int indexOfRp02, double x, double y) 
        {
            try
            {
                if (indexOfRp02 == dataReader.FittingPreassureInMPa.Count)
                {
                    //upisi u log fajl [private void setRp02PointCalculated(int indexOfRp02, double x, double y) ]{if (indexOfRp02 == dataReader.FittingPreassureInMPa.Count)}
                    Logger.WriteNode("[private void setRp02PointCalculated(int indexOfRp02, double x, double y) ]{if (indexOfRp02 == dataReader.FittingPreassureInMPa.Count)}", System.DateTime.Now);
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                int i;
                for (i = 0; i < PointsOfFittingLine.Count; i++)
                {
                    if (indexOfRp02 <= dataReader.FittingPreassureInMPa.Count - 1 && PointsOfFittingLine.ElementAt(i).YAxisValue > dataReader.FittingPreassureInMPa[indexOfRp02])
                    {
                        break;
                    }
                }

                //if (i > 0)
                //{
                //    for (int iRp02 = 0; iRp02 < dataReader.FittingPreassureInMPa.Count; iRp02++)
                //    {
                //        if (dataReader.FittingPreassureInMPa[iRp02] > rp02 && dataReader.FittingRelativeElongation[iRp02] > Rp02RIXValue)
                //        {
                //            xMarkers7[0] = dataReader.FittingRelativeElongation[iRp02];
                //            MkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                //            yMarkers7[0] = dataReader.FittingPreassureInMPa[iRp02];
                //            MkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];

                //            break;
                //        }
                //    }
                //}

                xMarkers7[0] = x;
                mkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                yMarkers7[0] = y;
                mkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Violet);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Violet), 1);
                _MarkerGraph7.Marker = mkr;

                printscreen.Rp02X = x;
                printscreen.Rp02Y = y;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRp02PointCalculated(int indexOfRp02, double x, double y)}", System.DateTime.Now);
            }

        }

        //private void setTextMarkert7()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(xMarkersText7);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(yMarkersText7);
        //    _MarkerGraphText7.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    xMarkersText7[0] = xMarkers7[0] + OptionsInPlottingMode.xRange / 120;
        //    yMarkersText7[0] = yMarkers7[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "Rp02";
        //    _MarkerGraphText7.Marker = mkrText;
        //}

        /// <summary>
        /// postavljanje Rp02 (automatski mod) samo index Rp02
        /// </summary>
        private void setRp02Point(int indexOfRp02, DataReader dataReader, double rp02, double rp02XValue)
        {

            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                int i;
                for (i = 0; i < pointsOfFittingLine.Count; i++)
                {
                    if (pointsOfFittingLine.ElementAt(i).YAxisValue > dataReader.FittingPreassureInMPa[indexOfRp02])
                    {
                        break;
                    }
                }

                if (i > 0)
                {
                    for (int iRp02 = 0; iRp02 < dataReader.FittingPreassureInMPa.Count; iRp02++)
                    {
                        if (dataReader.FittingPreassureInMPa[iRp02] > rp02 && dataReader.FittingRelativeElongation[iRp02] > rp02XValue)
                        {
                            //xMarkers7[0] = dataReader.FittingRelativeElongation[iRp02];
                            //xMarkers7[0] = 0.2 + OptionsInPlottingMode.pointManualX3 - xTranslateAmountFittingMode;//ovde se koristi vrednost samo u zavisnoti od x za tacku T3
                            xMarkers7[0] = dataReader.FittingRelativeElongation[iRp02];
                            mkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                            yMarkers7[0] = dataReader.FittingPreassureInMPa[iRp02];
                            mkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];
                            //double v = 
                            //  xMarkers7[0] = OptionsInPlottingMode.pointManualX3 - xTranslateAmountFittingMode + 0.2;
                            //for (int ii = 0; ii < pointsOfFittingLine.Count; ii++)
                            //{
                            //    if (pointsOfFittingLine[ii].XAxisValue > rp02XValue)
                            //    {
                            //        yMarkers7[0] = pointsOfFittingLine[ii].YAxisValue;
                            //    }
                            //}
                            //    yMarkers7[0] = OptionsInPlottingMode.pointManualY3;
                            break;
                        }
                    }
                }


                //if (i > 0)
                //{
                //    xMarkers7[0] = pointsOfFittingLine.ElementAt(i - 1).XAxisValue;
                //    yMarkers7[0] = pointsOfFittingLine.ElementAt(i - 1).YAxisValue;
                //}
                //if (i == 1)
                //{
                //    xMarkers7[0] = pointsOfFittingLine.ElementAt(i).XAxisValue;
                //    yMarkers7[0] = pointsOfFittingLine.ElementAt(i).YAxisValue;
                //}

                //xMarkers7[0] = rp02XValue;
                //yMarkers7[0] = rp02;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Violet);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Violet), 1);
                _MarkerGraph7.Marker = mkr;

                //setTextMarkert7();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRp02Point(int indexOfRp02, DataReader dataReader, double rp02, double rp02XValue)}", System.DateTime.Now);
            }
        }



        /// <summary>
        /// postavljanje ReH (rucni mod)
        /// </summary>
        private void setReHPoint(double x, double y)
        {

            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers6);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers6);
                _MarkerGraph6.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers6[0] = x;
                mkrTriangleCurrentValues.ReHXValue = xMarkers6[0];
                yMarkers6[0] = y;
                mkrTriangleCurrentValues.ReHYValue = yMarkers6[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Gray);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Gray), 1);
                _MarkerGraph6.Marker = mkr;

                printscreen.ReHX = x;
                printscreen.ReHY = y;
                isClickedByMouse_Plotting_ReH = true;
                //IsLuManualChanged = false;
                //setTextMarkert6();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setReHPoint(double x, double y)}", System.DateTime.Now);
            }
        }


        //private void setTextMarkert6()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(xMarkersText6);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(yMarkersText6);
        //    _MarkerGraphText6.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    xMarkersText6[0] = xMarkers6[0] + OptionsInPlottingMode.xRange / 120;
        //    yMarkersText6[0] = yMarkers6[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "ReH";
        //    _MarkerGraphText6.Marker = mkrText;
        //}

        /// <summary>
        /// postavljanje ReH (automatski mod) samo index maksimuma tj ReH
        /// </summary>
        private void setReHPoint(int indexOfmax)
        {

            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers6);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers6);
                _MarkerGraph6.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers6[0] = dataReader.RelativeElongation[indexOfmax] - xTranslateAmountFittingMode;
                mkrTriangleCurrentValues.ReHXValue = xMarkers6[0];
                yMarkers6[0] = dataReader.PreassureInMPa[indexOfmax];
                mkrTriangleCurrentValues.ReHYValue = yMarkers6[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Gray);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Gray), 1);
                _MarkerGraph6.Marker = mkr;

                //setTextMarkert6();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setReHPoint(int indexOfmax))}", System.DateTime.Now);
            }
        }


       

      


        /// <summary>
        /// postavljanje ReL (rucni mod)
        /// </summary>
        private void setReLPoint(double x, double y)
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers5);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers5);
                _MarkerGraph5.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers5[0] = x;
                mkrTriangleCurrentValues.ReLXValue = xMarkers5[0];
                //u slucaju da se radi o postavljanju ReL kada je samo on utvrdjen (ReH ne postoji)
                //ne treba podizati gore marker
                //jer je on vec podignut u metodi printscreen-a private void setReLPointCalculated(double x, double y)
                if (isFoundOnlyReLCase == true)
                {
                    yMarkers5[0] = y;
                }
                else
                {
                    yMarkers5[0] = y;
                }
                mkrTriangleCurrentValues.ReLYValue = yMarkers5[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Orange);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Orange), 1);
                _MarkerGraph5.Marker = mkr;

                printscreen.ReLX = x;
                printscreen.ReLY = y;
                isClickedByMouse_Plotting_ReL = true;
                //IsLuManualChanged = false;

                //setTextMarkert5();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setReLPoint(double x, double y)}", System.DateTime.Now);
            }
        }

        //private void setTextMarkert5()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(xMarkersText5);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(yMarkersText5);
        //    _MarkerGraphText5.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    xMarkersText5[0] = xMarkers5[0] + OptionsInPlottingMode.xRange / 120;
        //    yMarkersText5[0] = yMarkers5[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "ReL";
        //    _MarkerGraphText5.Marker = mkrText;
        //}

        /// <summary>
        /// postavljanje ReL (automatski mod) samo index minimuma tj Rel
        /// </summary>
        private void setReLPoint(int indexOfmin)
        {
            try
            {
                if (isFoundOnlyReLCase == true)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers5);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers5);
                _MarkerGraph5.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers5[0] = dataReader.RelativeElongation[indexOfmin] - xTranslateAmountFittingMode;
                mkrTriangleCurrentValues.ReLXValue = xMarkers5[0];
                yMarkers5[0] = dataReader.PreassureInMPa[indexOfmin];
                mkrTriangleCurrentValues.ReLYValue = yMarkers5[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Orange);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Orange), 1);
                _MarkerGraph5.Marker = mkr;

                //setTextMarkert5();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setReLPoint(int indexOfmin)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje maksimuma grafika sa dva argumenta x i y (rucni mod)
        /// </summary>
        private void setRmPoint(double x, double y)
        {
            try
            {
                //OptionsInPlottingMode.pointCrossheadX = dataReader.RelativeElongation[getRpmaxIndex()];
                //OptionsInPlottingMode.pointCrossheadY = dataReader.PreassureInMPa[getRpmaxIndex()];
                x = Math.Round(x, 1);
                tffittingCrossheadPointX.Text = x.ToString();
                y = Math.Round(y, 0);
                tffittingCrossheadPointY.Text = y.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = x;
                mkrTriangleCurrentValues.RmXValue = xMarkers4[0];
                yMarkers4[0] = y;
                mkrTriangleCurrentValues.RmYValue = yMarkers4[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                printscreen.RmX = x;
                printscreen.RmY = y;
                isClickedByMouse_Plotting_Rm = true;
                //IsLuManualChanged = false;
                //setTextMarkert4();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRmPoint(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje maksimuma grafika (automatski mod)
        /// </summary>
        private void setRmPoint()
        {
            try
            {
                OptionsInPlottingMode.pointCrossheadX = dataReader.RelativeElongation[getRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadY = dataReader.PreassureInMPa[getRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX, 1);
                tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = OptionsInPlottingMode.pointCrossheadX;
                mkrTriangleCurrentValues.RmXValue = xMarkers4[0];
                yMarkers4[0] = OptionsInPlottingMode.pointCrossheadY;
                mkrTriangleCurrentValues.RmYValue = yMarkers4[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                //setTextMarkert4();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void setRmPoint()}", System.DateTime.Now);
            }
        }

        #endregion 


        #region checkBoxTriangleMarkersVisibility

        private void chbReLVisibility_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                setReLPoint(mkrTriangleCurrentValues.ReLXValue, mkrTriangleCurrentValues.ReLYValue);
                printscreen.setReLPoint_Manual(mkrTriangleCurrentValues.ReLXValue, mkrTriangleCurrentValues.ReLYValue);
                tfReL.Text = mkrTriangleCurrentValues.ReLYValue.ToString();


                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.chbReL.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbReLVisibility_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbReLVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _MarkerGraph5.DataSource = null;
                //_MarkerGraphText5.DataSource = null;
                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.chbReL.IsChecked = false;
                    tfReL.Text = Constants.NOTFOUNDMAXMIN;
                    //if (onMode != null && onMode.ResultsInterface != null)
                    //{
                    //    onMode.ResultsInterface.tfReL.Text = string.Empty;
                    //}
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbReLVisibility_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbReHVisibility_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                setReHPoint(mkrTriangleCurrentValues.ReHXValue, mkrTriangleCurrentValues.ReHYValue);
                tfReH.Text = mkrTriangleCurrentValues.ReHYValue.ToString();

                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.chbReH.IsChecked = true;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbReHVisibility_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbReHVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _MarkerGraph6.DataSource = null;
                //_MarkerGraphText6.DataSource = null;
                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.chbReH.IsChecked = false;
                    tfReH.Text = Constants.NOTFOUNDMAXMIN;
                    //onMode.ResultsInterface.tfReH.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbReHVisibility_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void chbRmVisibility_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                setRmPoint(mkrTriangleCurrentValues.RmXValue, mkrTriangleCurrentValues.RmYValue);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbRmVisibility_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRmVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _MarkerGraph4.DataSource = null;
                //_MarkerGraphText4.DataSource = null;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbRmVisibility_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void chbRp02Visibility_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pointsOfFittingLine != null)
                {
                    setRp02Point(mkrTriangleCurrentValues.Rp02XValue, mkrTriangleCurrentValues.Rp02YValue);
                }
                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.chbRp02.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbRp02Visibility_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbRp02Visibility_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _MarkerGraph7.DataSource = null;
                //_MarkerGraphText7.DataSource = null;
                if (onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.chbRp02.IsChecked = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbRp02Visibility_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void chbAVisibility_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (pointsOfFittingLine != null)
                {
                    setAPoint(mkrTriangleCurrentValues.AXValue);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbAVisibility_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbAVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                _MarkerGraph8.DataSource = null;
                //_MarkerGraphText8.DataSource = null;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbAVisibility_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }
        #endregion

        private void tfReL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tfReL.Text.Equals("-1") || tfReL.Text.Equals("0"))
                {
                    tfReL.Text = Constants.NOTFOUNDMAXMIN;
                    if (onMode != null && onMode.ResultsInterface != null)
                    {
                        onMode.ResultsInterface.tfReL.Text = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfReL_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfReH_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tfReH.Text.Equals("-1") || tfReH.Text.Equals("0"))
                {
                    tfReH.Text = Constants.NOTFOUNDMAXMIN;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfReH_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRp02_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tfRp02.Text.Equals("-1") || tfRp02.Text.Equals("0"))
                {
                    tfRp02.Text = Constants.NOTFOUNDMAXMIN;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfRp02_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRm_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (tfRm.Text.Equals("-1") || tfRm.Text.Equals("0"))
                {
                    tfRm.Text = Constants.NOTFOUNDMAXMIN;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void tfRm_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbShowChangeOfRAndEe_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                if (window.Animation.chbStartSampleShowChangedParAnimation.IsChecked == true)
                {
                    window.Animation.chbStartSampleShowChangedParAnimation.IsChecked = false;
                }
                if (window.PrintScreen.chbChangeOfRAndE.IsChecked == true)
                {
                    window.PrintScreen.chbChangeOfRAndE.IsChecked = false;
                }




                /************ Ponovno racunanje i iscrtavanje posle promene izduzenja A u offline modu **************/
                //NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru { new E2E4CalculationAfterManualFitting } !!!
                e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                e2e4CalculationAfterManualFitting.CalculateIndexFromChangedParametersFittingUntilA(A);
                e2e4CalculationAfterManualFitting.DivideE2AndE4Interval(e2e4CalculationAfterManualFitting.IndexFromChangedParametersFittingUntilA, OptionsInOnlineMode.E3E4Border, xTranslateAmountFittingMode, false);
                setE2E4MinMaxAvg(e2e4CalculationAfterManualFitting);
                //setResultsInterfaceForManualSetPoint();

                //e2e4CalculationAfterManualFitting = new E2E4CalculationAfterManualFitting(OptionsInPlottingMode.filePath, Rp02RI, onMode);
                RmaxwithPoint = e2e4CalculationAfterManualFitting.calculateRmaxWithPoint();
                e2e4CalculationAfterManualFitting.RecalculateChangeOfRFittingPoints(this.Rp02RI, A);

                /************ Ponovno racunanje i iscrtavanje posle promene izduzenja A u offline modu **************/
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void chbShowChangeOfRAndEe_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }

        private void chbShowChangeOfRAndEe_Unchecked(object sender, RoutedEventArgs e)
        {
           

        }

        private void menuIOnlineMain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode.OptionsOnline.chbIsCalibration.IsChecked == true || OptionsInOnlineMode.isCalibration == true)
                {
                }
                else
                {
                    MessageBox.Show("Morate cekirati etaloniranje da bi podesili opcije jungovog modula !");
                    return;
                }

                if (onMode.OptionsOnline.chbIsCalibration.IsChecked == false)
                {

                }
                if (isOptionsForYungModuoOpen == true)
                {
                    MessageBox.Show("Otvoren je prozor opcija podešavanja Jungovog modula!");
                    return;
                }

                optionsForYungsModuo.Show();
                isOptionsForYungModuoOpen = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void menuIOnlineMain_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void plotter_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (onMode != null && onMode.ResultsInterface != null)
                {
                    onMode.ResultsInterface.Activate();
                }
                //if (onMode.ResultsInterface != null)
                //{
                //    onMode.ResultsInterface.Close();
                //    onMode.ResultsInterface = new ResultsInterface(onMode,this,printscreen);
                //    onMode.ResultsInterface.Show();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void plotter_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }





        public void SetManualGraphicRatio() 
        {
            try
            {
                rbtnManual.IsChecked = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {public void SetManualGraphicRatio()}", System.DateTime.Now);
            }
        }
       

        


    }
}
