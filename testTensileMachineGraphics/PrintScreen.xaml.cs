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
using testTensileMachineGraphics.Reports.SumReportClasses;
using System.Xml.Linq;
using System.Diagnostics;

namespace testTensileMachineGraphics
{
    /// <summary>
    /// Interaction logic for PrintScreen.xaml
    /// </summary>
    public partial class PrintScreen : UserControl
    {


        private FooterOptions footerOptions;

        private bool isClickedNewMeasures = false;

        /// <summary>
        /// ime uzoraka koji se dobija sa zapamcenim nazivom uzorka
        /// </summary>
        public string DefaultPath = string.Empty;


        /// <summary>
        /// ne treba da postavlja markere posle online prekida kada se programskim putem postavljaju checkbox-ovi u izlaznom interfejsu
        /// </summary>
        public bool DontSetMarkers = false;

        public bool IsPrintScreenEmpty = false;

        #region ManualMarkersValue

        public double Rp02X { get; set; }
        public double Rp02Y { get; set; }

        public double ReLX { get; set; }
        public double ReLY { get; set; }

        public double ReHX { get; set; }
        public double ReHY { get; set; }

        public double RmX { get; set; }
        public double RmY { get; set; }

        public double AX { get; set; }
        public double AY { get; set; }

        #endregion

        #region IsVisibleChecked
        /// <summary>
        /// checkbox visibility on results interface
        /// </summary>
        public bool IsRp02 = true;
        public bool IsRt05 = true;
        public bool IsReL = true;
        public bool IsReH = true;

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



        #region LastFiveMarkers

        private List<double> xMarkers11 = new List<double>();
        public List<double> XMarkers11
        {
            get { return xMarkers11; }
            set
            {
                if (value != null)
                {
                    xMarkers11 = value;
                }
            }
        }
        private List<double> yMarkers11 = new List<double>();
        public List<double> YMarkers11
        {
            get { return yMarkers11; }
            set
            {
                if (value != null)
                {
                    yMarkers11 = value;
                }
            }
        }
        private List<double> xMarkersText11 = new List<double>();
        public List<double> XMarkersText11
        {
            get { return xMarkersText11; }
            set
            {
                if (value != null)
                {
                    xMarkersText11 = value;
                }
            }
        }
        private List<double> yMarkersText11 = new List<double>();
        public List<double> YMarkersText11
        {
            get { return yMarkersText11; }
            set
            {
                if (value != null)
                {
                    yMarkersText11 = value;
                }
            }
        }



        private List<double> xMarkers12 = new List<double>();
        public List<double> XMarkers12
        {
            get { return xMarkers12; }
            set
            {
                if (value != null)
                {
                    xMarkers12 = value;
                }
            }
        }
        private List<double> yMarkers12 = new List<double>();
        public List<double> YMarkers12
        {
            get { return yMarkers12; }
            set
            {
                if (value != null)
                {
                    yMarkers12 = value;
                }
            }
        }
        private List<double> xMarkersText12 = new List<double>();
        public List<double> XMarkersText12
        {
            get { return xMarkersText12; }
            set
            {
                if (value != null)
                {
                    xMarkersText12 = value;
                }
            }
        }
        private List<double> yMarkersText12 = new List<double>();
        public List<double> YMarkersText12
        {
            get { return yMarkersText12; }
            set
            {
                if (value != null)
                {
                    yMarkersText12 = value;
                }
            }
        }

        private List<double> xMarkers13 = new List<double>();
        public List<double> XMarkers13
        {
            get { return xMarkers13; }
            set
            {
                if (value != null)
                {
                    xMarkers13 = value;
                }
            }
        }
        private List<double> yMarkers13 = new List<double>();
        public List<double> YMarkers13
        {
            get { return yMarkers13; }
            set
            {
                if (value != null)
                {
                    yMarkers13 = value;
                }
            }
        }
        private List<double> xMarkersText13 = new List<double>();
        public List<double> XMarkersText13
        {
            get { return xMarkersText13; }
            set
            {
                if (value != null)
                {
                    xMarkersText13 = value;
                }
            }
        }
        private List<double> yMarkersText13 = new List<double>();
        public List<double> YMarkersText13
        {
            get { return yMarkersText13; }
            set
            {
                if (value != null)
                {
                    yMarkersText13 = value;
                }
            }
        }


        private List<double> xMarkers14 = new List<double>();
        public List<double> XMarkers14
        {
            get { return xMarkers14; }
            set
            {
                if (value != null)
                {
                    xMarkers14 = value;
                }
            }
        }
        private List<double> yMarkers14 = new List<double>();
        public List<double> YMarkers14
        {
            get { return yMarkers14; }
            set
            {
                if (value != null)
                {
                    yMarkers14 = value;
                }
            }
        }
        private List<double> xMarkersText14 = new List<double>();
        public List<double> XMarkersText14
        {
            get { return xMarkersText14; }
            set
            {
                if (value != null)
                {
                    xMarkersText14 = value;
                }
            }
        }
        private List<double> yMarkersText14 = new List<double>();
        public List<double> YMarkersText14
        {
            get { return yMarkersText14; }
            set
            {
                if (value != null)
                {
                    yMarkersText14 = value;
                }
            }
        }

        private List<double> xMarkers15 = new List<double>();
        public List<double> XMarkers15
        {
            get { return xMarkers15; }
            set
            {
                if (value != null)
                {
                    xMarkers15 = value;
                }
            }
        }
        private List<double> yMarkers15 = new List<double>();
        public List<double> YMarkers15
        {
            get { return yMarkers15; }
            set
            {
                if (value != null)
                {
                    yMarkers15 = value;
                }
            }
        }
        private List<double> xMarkersText15 = new List<double>();
        public List<double> XMarkersText15
        {
            get { return xMarkersText15; }
            set
            {
                if (value != null)
                {
                    xMarkersText15 = value;
                }
            }
        }
        private List<double> yMarkersText15 = new List<double>();
        public List<double> YMarkersText15
        {
            get { return yMarkersText15; }
            set
            {
                if (value != null)
                {
                    yMarkersText15 = value;
                }
            }
        }

        #endregion

        #endregion

        public bool IsNManualWindowCreated = false;

        private WindowForNManualCalculation nManualCalculation;
        public WindowForNManualCalculation NManualCalculation 
        {
            get { return nManualCalculation; }
            set 
            {
                if (value != null)
                {
                    nManualCalculation = value;
                }
            }
        }

        private GraphicPlotting plotting;
        private SumReportLastLoad sumReport;

        #region setMarkers_Automatic


        /// <summary>
        /// 
        /// </summary>
        public void SetT1Point()
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers);
                _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //double xt1 = 0;
                //bool isN = double.TryParse(plotting.tffittingManPoint1X.Text, out xt1);
                //xMarkers[0] = xt1 - plotting.XTranslateAmountFittingMode;

                double yt1 = 0;
                bool isN = double.TryParse(plotting.tffittingManPoint1Y.Text, out yt1);
                //yMarkers[0] = yt1;

                //postavi tacno na plavi grafik  marker za tacku T1
                double k = plotting.PointsOfFittingLine[1].YAxisValue / plotting.PointsOfFittingLine[1].XAxisValue;

                xMarkers[0] = yt1 / k;
                yMarkers[0] = yt1;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Blue);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                this._MarkerGraph.Marker = mkr;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void SetT1Point()}", System.DateTime.Now);
            }

        }


        public void SetT2Point()
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers2);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers2);
                _MarkerGraph2.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //double xt2 = 0;
                //bool isN = double.TryParse(plotting.tffittingManPoint2X.Text, out xt2);
                //xMarkers2[0] = xt2 - plotting.XTranslateAmountFittingMode;

                double yt2 = 0;
                bool isN = double.TryParse(plotting.tffittingManPoint2Y.Text, out yt2);
                //yMarkers2[0] = yt2;

                //postavi tacno na plavi grafik  marker za tacku T1
                double k = plotting.PointsOfFittingLine[1].YAxisValue / plotting.PointsOfFittingLine[1].XAxisValue;

                xMarkers2[0] = yt2 / k;
                yMarkers2[0] = yt2;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Yellow);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                this._MarkerGraph2.Marker = mkr;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void SetT2Point()}", System.DateTime.Now);
            }

        }


        public void SetT3Point()
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers3);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers3);
                _MarkerGraph3.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //double xt3 = 0;
                //bool isN = double.TryParse(plotting.tffittingManPoint3X.Text, out xt3);
                //xMarkers3[0] = xt3 - plotting.XTranslateAmountFittingMode;

                double yt3 = 0;
                bool isN = double.TryParse(plotting.tffittingManPoint3Y.Text, out yt3);
                //yMarkers3[0] = yt3;

                double k = plotting.PointsOfFittingLine[1].YAxisValue / plotting.PointsOfFittingLine[1].XAxisValue;

                xMarkers3[0] = yt3 / k;
                yMarkers3[0] = yt3;


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                this._MarkerGraph3.Marker = mkr;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void SetT3Point()}", System.DateTime.Now);
            }

        }

        /// <summary>
        /// postavljanje Rp02 (automatski mod) samo index Rp02
        /// </summary>
        private void setRp02Point(int indexOfRp02, DataReader dataReader, double rp02, double rp02XValue)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (rp02 == 0 || rp02 == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                int i;
                for (i = 0; i < plotting.PointsOfFittingLine.Count; i++)
                {
                    if (indexOfRp02 <= dataReader.FittingPreassureInMPa.Count - 1 && plotting.PointsOfFittingLine.ElementAt(i).YAxisValue > dataReader.FittingPreassureInMPa[indexOfRp02])
                    {
                        break;
                    }
                }

                if (i > 0)
                {
                    for (int iRp02 = 0; iRp02 < dataReader.FittingPreassureInMPa.Count; iRp02++)
                    {
                        if (dataReader.FittingPreassureInMPa[iRp02] > rp02 && dataReader.FittingRelativeElongation[iRp02] > plotting.ReH_X)
                        {
                            xMarkers7[0] = dataReader.FittingRelativeElongation[iRp02];
                            plotting.MkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                            yMarkers7[0] = dataReader.FittingPreassureInMPa[iRp02];
                            plotting.MkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];

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
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRp02Point(int indexOfRp02, DataReader dataReader, double rp02, double rp02XValue)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje Rt05 (automatski mod) samo vrednost relativnog izduzenja
        /// </summary>
        private void setRt05Point(double rt05)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (rt05 == 0 || rt05 == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers9);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers9);
                _MarkerGraph9.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers9[0] = 0.5;
                plotting.MkrTriangleCurrentValues.Rt05XValue = xMarkers9[0];
                //_a_X = xMarkers9[0];


                yMarkers9[0] = rt05;
                plotting.MkrTriangleCurrentValues.Rt05YValue = yMarkers9[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Purple);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Purple), 1);
                _MarkerGraph9.Marker = mkr;

                //setTextMarkert9();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRt05Point(double rt05)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje ReH (automatski mod) samo index maksimuma tj ReH
        /// </summary>
        private void setReHPoint(int indexOfmax)
        {
            try
            {
                if (plotting.chbReHVisibility.IsChecked == false)
                {
                    return;
                }

                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (plotting.ReH == 0 || plotting.ReH == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers6);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers6);
                _MarkerGraph6.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers6[0] = plotting.DataReader.RelativeElongation[indexOfmax] - plotting.XTranslateAmountFittingMode;
                plotting.MkrTriangleCurrentValues.ReHXValue = xMarkers6[0];
                yMarkers6[0] = plotting.DataReader.PreassureInMPa[indexOfmax];
                plotting.MkrTriangleCurrentValues.ReHYValue = yMarkers6[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Gray);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Gray), 1);
                _MarkerGraph6.Marker = mkr;

                //setTextMarkert6();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setReHPoint(int indexOfmax)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje ReL (automatski mod) samo index minimuma tj Rel
        /// </summary>
        private void setReLPoint(int indexOfmin)
        {
            try
            {
                if (plotting.chbReLVisibility.IsChecked == false)
                {
                    return;
                }

                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (plotting.ReL == 0 || plotting.ReL == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers5);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers5);
                _MarkerGraph5.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers5[0] = plotting.DataReader.RelativeElongation[indexOfmin] - plotting.XTranslateAmountFittingMode;
                plotting.MkrTriangleCurrentValues.ReLXValue = xMarkers5[0];
                yMarkers5[0] = plotting.DataReader.PreassureInMPa[indexOfmin];
                plotting.MkrTriangleCurrentValues.ReLYValue = yMarkers5[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Orange);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Orange), 1);
                _MarkerGraph5.Marker = mkr;

                //setTextMarkert5();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setReLPoint(int indexOfmin)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje maksimuma grafika (automatski mod)
        /// </summary>
        private void setRmPoint()
        {

            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (plotting.Rm == 0 || plotting.Rm == -1)
                {
                    return;
                }

                OptionsInPlottingMode.pointCrossheadX = plotting.DataReader.RelativeElongation[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadY = plotting.DataReader.PreassureInMPa[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX, 1);
                //tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                //tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = OptionsInPlottingMode.pointCrossheadX - plotting.XTranslateAmountFittingMode;
                plotting.MkrTriangleCurrentValues.RmXValue = xMarkers4[0];
                yMarkers4[0] = OptionsInPlottingMode.pointCrossheadY;
                plotting.MkrTriangleCurrentValues.RmYValue = yMarkers4[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                //setTextMarkert4();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRmPoint()}", System.DateTime.Now);
            }
        }

    


        /// <summary>
        /// postavljanje maksimuma grafika (automatski mod)
        /// </summary>
        public void setRmPoint2(double x, double y)
        {

            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (plotting.Rm == 0 || plotting.Rm == -1)
                {
                    return;
                }

                OptionsInPlottingMode.pointCrossheadX = plotting.DataReader.RelativeElongation[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadY = plotting.DataReader.PreassureInMPa[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX, 1);
                //tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                //tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = x;
                plotting.MkrTriangleCurrentValues.RmXValue = xMarkers4[0];
                yMarkers4[0] = y;
                plotting.MkrTriangleCurrentValues.RmYValue = yMarkers4[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                //setTextMarkert4();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRmPoint()}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje A (automatski) samo vrednost relativnog izduzenja
        /// </summary>
        public void setAPoint(double A)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (A == 0 || A == -1)
                {
                    return;
                }

                if (A == plotting.Ag)
                {
                    _MarkerGraph10.DataSource = null;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers8);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers8);
                _MarkerGraph8.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers8[0] = A;
                plotting.MkrTriangleCurrentValues.AXValue = xMarkers8[0];
                //_a_X = xMarkers8[0];


                if (OptionsInPlottingMode.isAutoChecked)
                {
                    yMarkers8[0] = 0.020 * plotting.Rm;
                    plotting.MkrTriangleCurrentValues.AYValue = yMarkers8[0];
                }
                else
                {
                    yMarkers8[0] = 0.020 * OptionsInPlottingMode.yRange;
                    plotting.MkrTriangleCurrentValues.AYValue = yMarkers8[0];
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
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setAPoint(double A)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje Ag (automatski) samo vrednost relativnog izduzenja
        /// </summary>
        private void setAgPoint(double Ag)
        {
            
            //if (plotting.PointsOfFittingLine == null)
            //{
            //    return;
            //}

            //if (Ag == 0 || Ag == -1)
            //{
            //    return;
            //}

            //if (Ag == plotting.A || Ag == AX)
            //{
            //    return;
            //}

            //EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers10);
            //EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers10);
            //_MarkerGraph10.DataSource = new CompositeDataSource(gX, gY);

            ////no scaling - identity mapping
            //gX.XMapping = xx => xx;
            //gY.YMapping = yy => yy;


            //xMarkers10[0] = Ag;
            //plotting.MkrTriangleCurrentValues.AgXValue = xMarkers10[0];
            ////_a_X = xMarkers8[0];

            
            //yMarkers10[0] = 0.020 * plotting.Rm;
            //plotting.MkrTriangleCurrentValues.AgYValue = yMarkers10[0];




            //CirclePointMarker mkr = new CirclePointMarker();
            //mkr.Fill = new SolidColorBrush(Colors.Brown);
            //mkr.Size = Constants.MARKERSIZE;
            //mkr.Pen = new Pen(new SolidColorBrush(Colors.Brown), 1);
            //_MarkerGraph10.Marker = mkr;

            ////setTextMarkert10();
        }

        #endregion


        #region setMarkers_AfterLuChanged

        /// <summary>
        /// postavljanje Rp02 (automatski mod) samo index Rp02
        /// </summary>
        private void setRp02Point_Lu(double ratioNewLuLastLu)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers7[0] = plotting.MkrTriangleCurrentValues.Rp02XValue * ratioNewLuLastLu;
                plotting.MkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                yMarkers7[0] = plotting.MkrTriangleCurrentValues.Rp02YValue;
                plotting.MkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];





                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Violet);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Violet), 1);
                _MarkerGraph7.Marker = mkr;

                //setTextMarkert7();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRp02Point_Lu(double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }


        public void SetRt05Point_Lu(double rt05) 
        {
            try
            {
                //postavi marker samo kada se  izduzenje merilo sa ekstenziometrom
                if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                {
                    setRt05Point_Lu(rt05);
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void SetRt05Point_Lu(double rt05)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje Rt05 (automatski mod) samo vrednost relativnog izduzenja
        /// </summary>
        private void setRt05Point_Lu(double rt05)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers9);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers9);
                _MarkerGraph9.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers9[0] = plotting.MkrTriangleCurrentValues.Rt05XValue/* * ratioNewLuLastLu*/;
                plotting.MkrTriangleCurrentValues.Rt05XValue = xMarkers9[0];


                yMarkers9[0] = rt05;
                plotting.MkrTriangleCurrentValues.Rt05YValue = yMarkers9[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Purple);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Purple), 1);
                _MarkerGraph9.Marker = mkr;

                //setTextMarkert9();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRt05Point_Lu(double rt05)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje ReH (automatski mod) samo index maksimuma tj ReH
        /// </summary>
        private void setReHPoint_Lu(double ratioNewLuLastLu)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers6);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers6);
                _MarkerGraph6.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers6[0] = plotting.MkrTriangleCurrentValues.ReHXValue * ratioNewLuLastLu;
                plotting.MkrTriangleCurrentValues.ReHXValue = xMarkers6[0];
                yMarkers6[0] = plotting.MkrTriangleCurrentValues.ReHYValue;
                plotting.MkrTriangleCurrentValues.ReHYValue = yMarkers6[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Gray);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Gray), 1);
                _MarkerGraph6.Marker = mkr;

                //setTextMarkert6();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setReHPoint_Lu(double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje ReL (automatski mod) samo index minimuma tj Rel
        /// </summary>
        private void setReLPoint_Lu(double ratioNewLuLastLu)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers5);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers5);
                _MarkerGraph5.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers5[0] = plotting.MkrTriangleCurrentValues.ReLXValue * ratioNewLuLastLu;
                plotting.MkrTriangleCurrentValues.ReLXValue = xMarkers5[0];
                yMarkers5[0] = plotting.MkrTriangleCurrentValues.ReLYValue;
                plotting.MkrTriangleCurrentValues.ReLYValue = yMarkers5[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Orange);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Orange), 1);
                _MarkerGraph5.Marker = mkr;

                //setTextMarkert5();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setReLPoint_Lu(double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje maksimuma grafika (automatski mod)
        /// </summary>
        private void setRmPoint_Lu(double ratioNewLuLastLu)
        {
            try
            {

                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                OptionsInPlottingMode.pointCrossheadX = plotting.DataReader.RelativeElongation[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadY = plotting.DataReader.PreassureInMPa[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX, 1);
                //tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                //tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = plotting.MkrTriangleCurrentValues.RmXValue * ratioNewLuLastLu;
                plotting.MkrTriangleCurrentValues.RmXValue = xMarkers4[0];
                yMarkers4[0] = plotting.MkrTriangleCurrentValues.RmYValue;
                plotting.MkrTriangleCurrentValues.RmYValue = yMarkers4[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                //setTextMarkert4();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRmPoint_Lu(double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje A (automatski) samo vrednost relativnog izduzenja
        /// </summary>
        private void setAPoint_Lu(double ratioNewLuLastLu)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers8);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers8);
                _MarkerGraph8.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers8[0] = plotting.MkrTriangleCurrentValues.AXValue * ratioNewLuLastLu;
                plotting.MkrTriangleCurrentValues.AXValue = xMarkers8[0];


                yMarkers8[0] = plotting.MkrTriangleCurrentValues.AYValue;
                plotting.MkrTriangleCurrentValues.AYValue = yMarkers8[0];




                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Red), 1);
                _MarkerGraph8.Marker = mkr;

                //setTextMarkert8();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setAPoint_Lu(double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje Ag (automatski) samo vrednost relativnog izduzenja
        /// </summary>
        private void setAgPoint_Lu(double ratioNewLuLastLu)
        {

            //if (plotting.PointsOfFittingLine == null)
            //{
            //    return;
            //}

            //EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers10);
            //EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers10);
            //_MarkerGraph10.DataSource = new CompositeDataSource(gX, gY);

            ////no scaling - identity mapping
            //gX.XMapping = xx => xx;
            //gY.YMapping = yy => yy;


            //xMarkers10[0] = plotting.MkrTriangleCurrentValues.AgXValue * ratioNewLuLastLu;
            //plotting.MkrTriangleCurrentValues.AgXValue = xMarkers10[0];
            ////_a_X = xMarkers8[0];


            //yMarkers10[0] = plotting.MkrTriangleCurrentValues.AgYValue;
            //plotting.MkrTriangleCurrentValues.AgYValue = yMarkers10[0];




            //CirclePointMarker mkr = new CirclePointMarker();
            //mkr.Fill = new SolidColorBrush(Colors.Brown);
            //mkr.Size = Constants.MARKERSIZE;
            //mkr.Pen = new Pen(new SolidColorBrush(Colors.Brown), 1);
            //_MarkerGraph10.Marker = mkr;

            //setTextMarkert10();
        }

        #endregion

        #region setMarkers_Manual

        /// <summary>
        /// postavljanje Rp02 (automatski mod) samo index Rp02
        /// </summary>
        public void setRp02Point_Manual(double x, double y)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (y == 0 || y == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers7);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers7);
                _MarkerGraph7.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers7[0] = x;
                plotting.MkrTriangleCurrentValues.Rp02XValue = xMarkers7[0];
                yMarkers7[0] = y;
                plotting.MkrTriangleCurrentValues.Rp02YValue = yMarkers7[0];





                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Violet);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Violet), 1);
                _MarkerGraph7.Marker = mkr;

                //setTextMarkert7();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setRp02Point_Manual(double x, double y)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje Rt05 (automatski mod) samo vrednost relativnog izduzenja
        /// </summary>
        public void setRt05Point_Manual(double x, double y)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers9);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers9);
                _MarkerGraph9.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers9[0] = x;
                plotting.MkrTriangleCurrentValues.Rt05XValue = xMarkers9[0];


                yMarkers9[0] = y;
                plotting.MkrTriangleCurrentValues.Rt05YValue = yMarkers9[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Purple);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Purple), 1);
                _MarkerGraph9.Marker = mkr;

                //setTextMarkert9();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setRt05Point_Manual(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje ReH (automatski mod) samo index maksimuma tj ReH
        /// </summary>
        public void setReHPoint_Manual(double x, double y)
        {
            try
            {
                if (plotting.chbReHVisibility.IsChecked == false)
                {
                    return;
                }

                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }


                if (y == 0 || y == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers6);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers6);
                _MarkerGraph6.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers6[0] = x;
                plotting.MkrTriangleCurrentValues.ReHXValue = xMarkers6[0];
                yMarkers6[0] = y;
                plotting.MkrTriangleCurrentValues.ReHYValue = yMarkers6[0];

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Gray);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Gray), 1);
                _MarkerGraph6.Marker = mkr;

                //setTextMarkert6();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setReHPoint_Manual(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// na kraju findonlyReL procedure GraphicPlotting fajla postavi na osnovu x i y ReL
        /// (automatski mod)
        /// ovo treba samo jedanput da se poziva u programu kada se uradi Find References
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetReLPointCalculated(double x, double y)
        {
            try
            {
                setReLPointCalculated(x, y);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void SetReLPointCalculated(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// na kraju findonlyReL procedure GraphicPlotting fajla postavi na osnovu x i y ReL
        /// (automatski mod)
        /// ovo treba samo jedanput da se poziva u programu kada se uradi Find References
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void setReLPointCalculated(double x, double y)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (y == 0 || y == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers5);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers5);
                _MarkerGraph5.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers5[0] = x;
                plotting.MkrTriangleCurrentValues.ReLXValue = xMarkers5[0];
                yMarkers5[0] = y;
                plotting.MkrTriangleCurrentValues.ReLYValue = yMarkers5[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Orange);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Orange), 1);
                _MarkerGraph5.Marker = mkr;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setReLPointCalculated(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje ReL (automatski mod) samo index minimuma tj Rel
        /// </summary>
        public void setReLPoint_Manual(double x, double y)
        {
            try
            {
                if (plotting.chbReLVisibility.IsChecked == false)
                {
                    return;
                }

                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (y == 0 || y == -1)
                {
                    return;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers5);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers5);
                _MarkerGraph5.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers5[0] = x;
                plotting.MkrTriangleCurrentValues.ReLXValue = xMarkers5[0];


                yMarkers5[0] = y;

                plotting.MkrTriangleCurrentValues.ReLYValue = yMarkers5[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Orange);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Orange), 1);
                _MarkerGraph5.Marker = mkr;

                //setTextMarkert5();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setReLPoint_Manual(double x, double y)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// postavljanje maksimuma grafika (automatski mod)
        /// </summary>
        public void setRmPoint_Manual(double x, double y)
        {
            try
            {

                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (y == 0 || y == -1)
                {
                    return;
                }

                if (plotting.GetRpmaxIndex() >= plotting.DataReader.RelativeElongation.Count - 1)
                {
                    return;
                }

                if (plotting.GetRpmaxIndex() >= plotting.DataReader.PreassureInMPa.Count - 1)
                {
                    return;
                }

                OptionsInPlottingMode.pointCrossheadX = plotting.DataReader.RelativeElongation[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadY = plotting.DataReader.PreassureInMPa[plotting.GetRpmaxIndex()];
                OptionsInPlottingMode.pointCrossheadX = Math.Round(OptionsInPlottingMode.pointCrossheadX, 1);
                //tffittingCrossheadPointX.Text = OptionsInPlottingMode.pointCrossheadX.ToString();
                OptionsInPlottingMode.pointCrossheadY = Math.Round(OptionsInPlottingMode.pointCrossheadY, 0);
                //tffittingCrossheadPointY.Text = OptionsInPlottingMode.pointCrossheadY.ToString();

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers4);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers4);
                _MarkerGraph4.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers4[0] = x;
                plotting.MkrTriangleCurrentValues.RmXValue = xMarkers4[0];
                yMarkers4[0] = y;
                plotting.MkrTriangleCurrentValues.RmYValue = yMarkers4[0];


                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Black);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph4.Marker = mkr;

                //setTextMarkert4();\
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setRmPoint_Manual(double x, double y)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje A (automatski) samo vrednost relativnog izduzenja
        /// </summary>
        public void setAPoint_Manual(double x)
        {
            try
            {
                if (plotting.PointsOfFittingLine == null)
                {
                    return;
                }

                if (x == 0 || x == -1)
                {
                    return;
                }

                if (x == plotting.Ag)
                {
                    _MarkerGraph10.DataSource = null;
                }

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers8);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers8);
                _MarkerGraph8.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;


                xMarkers8[0] = x;
                plotting.MkrTriangleCurrentValues.AXValue = xMarkers8[0];


                yMarkers8[0] = plotting.MkrTriangleCurrentValues.AYValue;
                plotting.MkrTriangleCurrentValues.AYValue = yMarkers8[0];




                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Red), 1);
                _MarkerGraph8.Marker = mkr;

                //setTextMarkert8();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setAPoint_Manual(double x)}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// postavljanje Ag (automatski) samo vrednost relativnog izduzenja
        /// </summary>
        public void setAgPoint_Manual(double x)
        {

            //if (plotting.PointsOfFittingLine == null)
            //{
            //    return;
            //}

            //EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xMarkers10);
            //EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yMarkers10);
            //_MarkerGraph10.DataSource = new CompositeDataSource(gX, gY);

            ////no scaling - identity mapping
            //gX.XMapping = xx => xx;
            //gY.YMapping = yy => yy;


            //xMarkers10[0] = x;
            //plotting.MkrTriangleCurrentValues.AgXValue = xMarkers10[0];
            ////_a_X = xMarkers8[0];


            //yMarkers10[0] = plotting.MkrTriangleCurrentValues.AgYValue;
            //plotting.MkrTriangleCurrentValues.AgYValue = yMarkers10[0];




            //CirclePointMarker mkr = new CirclePointMarker();
            //mkr.Fill = new SolidColorBrush(Colors.Brown);
            //mkr.Size = Constants.MARKERSIZE;
            //mkr.Pen = new Pen(new SolidColorBrush(Colors.Brown), 1);
            //_MarkerGraph10.Marker = mkr;

            ////setTextMarkert10();
        }

        #endregion


        public void setMarkers() 
        {
            try
            {
                if (DontSetMarkers == true)
                {
                    return;
                }

                //if (IsPrintScreenEmpty == true)
                //{
                //    return;
                //}
                //if (chbCalculateNManual.IsChecked == true)
                //{
                //    chbCalculateNManual.IsChecked = false;
                //}

                if (this.IsRp02 == false)
                {
                    _MarkerGraph7.DataSource = null;
                }
                else if (plotting.IsClickedByMouse_Plotting_Rp02 == false)
                {
                    setRp02Point(plotting.Rp02Index, plotting.DataReader, plotting.Rp02RI, plotting.Rp02RIXValue);
                }
                else
                {
                    setRp02Point_Manual(Rp02X, Rp02Y);
                }
                if (this.IsRt05 == false)
                {
                    _MarkerGraph9.DataSource = null;
                }
                else
                {
                    //samo postavljaj na grafiku Rt05 kada se za merenje izduzenja koristio ektenziometar
                    //if (plotting.OnlineModeInstance != null && plotting.OnlineModeInstance.OnHeader != null && plotting.OnlineModeInstance.OnHeader.ConditionsOfTesting != null && plotting.OnlineModeInstance.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                    if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                    {
                        setRt05Point(plotting.Rt05);
                    }
                }
                if (this.IsReH == false)
                {
                    _MarkerGraph6.DataSource = null;
                }
                else if (plotting.IsClickedByMouse_Plotting_ReH == false)
                {
                    setReHPoint(plotting.ReHIndex);
                }
                else
                {
                    setReHPoint_Manual(ReHX, ReHY);
                }
                if (this.IsReL == false)
                {
                    _MarkerGraph5.DataSource = null;
                }
                else if (plotting.IsClickedByMouse_Plotting_ReL == false)
                {
                    setReLPoint(plotting.ReLIndex);
                    //ukoliko je proracun izvrsen na konstantnom delu treba racku postaviti na osnovu ReLX i ReLY vrednosti
                    if (plotting.IsFoundOnlyReLCase == true)
                    {
                        setReLPoint_Manual(ReLX, ReLY);
                    }
                }
                else
                {
                    if (plotting.IsFoundOnlyReLCase == true)
                    {

                    }
                    else
                    {
                        setReLPoint_Manual(ReLX, ReLY);
                    }
                }
                if (plotting.IsClickedByMouse_Plotting_Rm == false)
                {
                    setRmPoint();
                }
                else
                {
                    setRmPoint_Manual(RmX, RmY);
                }
                if (plotting.IsClickedByMouse_Plotting_A == false)
                {
                    setAPoint(plotting.A);
                    if (plotting.A != plotting.Ag)
                    {
                        setAgPoint(plotting.Ag);
                    }
                }
                else
                {
                    setAPoint_Manual(AX);
                    if (AX != plotting.Ag)
                    {
                        setAgPoint(plotting.Ag);
                    }
                }

                //setAgPoint(plotting.Ag);

            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setMarkers()}", System.DateTime.Now);
            }
        }

        public void setMarkers_Lu(double ratioNewLuLastLu)
        {
            try
            {
                setRp02Point_Lu(ratioNewLuLastLu);
                setRt05Point_Lu(ratioNewLuLastLu);
                setReHPoint_Lu(ratioNewLuLastLu);
                setReLPoint_Lu(ratioNewLuLastLu);
                setRmPoint_Lu(ratioNewLuLastLu);
                setAPoint_Lu(ratioNewLuLastLu);
                setAgPoint_Lu(ratioNewLuLastLu);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void setMarkers_Lu(double ratioNewLuLastLu)}", System.DateTime.Now);
            }
        }

       
        public PrintScreen(GraphicPlotting pl)
        {
            try
            {
                InitializeComponent();
                plotting = pl;

                cmbInputWindow.SelectedIndex = 0;

                //ove markere prikazuju trouglice koji predstavljaju Rp02,Rmax,ReH,ReL,A,T1,T2 i T3  i za prikaz dijagrama Ag i Rt05 
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
                xMarkers11.Add(-1);
                yMarkers11.Add(-1);
                xMarkersText11.Add(-1);
                yMarkersText11.Add(-1);
                xMarkers12.Add(-1);
                yMarkers12.Add(-1);
                xMarkersText12.Add(-1);
                yMarkersText12.Add(-1);
                xMarkers13.Add(-1);
                yMarkers13.Add(-1);
                xMarkersText13.Add(-1);
                yMarkersText13.Add(-1);
                xMarkers14.Add(-1);
                yMarkers14.Add(-1);
                xMarkersText14.Add(-1);
                yMarkersText14.Add(-1);
                xMarkers15.Add(-1);
                yMarkers15.Add(-1);
                xMarkersText15.Add(-1);
                yMarkersText15.Add(-1);

                nManualCalculation = new WindowForNManualCalculation(plotting, this);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public PrintScreen(GraphicPlotting pl)}", System.DateTime.Now);
            }

            
        }

        public void UpdatePrintScreen(MyPointCollection fittingPoints) 
        {
            try
            {
                deleteOnlyPrintScreenLine();

                var ds = new EnumerableDataSource<MyPoint>(fittingPoints);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);

                plotterPrint.AddLineGraph(ds, Colors.Blue, 2, "grafik");
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void UpdatePrintScreen(MyPointCollection fittingPoints)}", System.DateTime.Now);
            }
        }



        private void btnPrintSampleOnlyMakeReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                //sakrij vidljivost legende grafika
                plotterPrint.LegendVisible = false;
                //sakrij markere sa printscreen-a
                hidePrintScreenMarkers();

                window.Plotting.Printscreen.plotterPrint.SaveScreenshot(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");
                if (window.OnlineMode.chbStartSampleShowChangedPar.IsChecked == true)
                {
                    if (window.OnlineMode.VXY != null)
                    {
                        if (chbChangeOfRAndE.IsChecked == true)
                        {
                            if (window.OnlineMode.VXY.plotterChangeOfR != null)
                            {
                                window.OnlineMode.VXY.plotterChangeOfR.SaveScreenshot(System.Environment.CurrentDirectory + "\\graphicChangeOfRTest1.png");
                            }
                            if (window.OnlineMode.VXY.plotterChangeOfE != null)
                            {
                                window.OnlineMode.VXY.plotterChangeOfE.SaveScreenshot(System.Environment.CurrentDirectory + "\\graphicChangeOfETest1.png");
                            }
                        }
                    }
                }
                DefaultPath = Properties.Settings.Default.SaveDirectory + LastInputOutputSavedData.tfBrUzorka_GeneralData;
                DefaultPath = DefaultPath.Replace('/', '_');

                //PDFSampleReport pdfSampleReport = new PDFSampleReport(11.5, 8.7, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                DefaultPath += ".pdf";
                PDFSampleReport pdfSampleReport = new PDFSampleReport(12, 9, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                pdfSampleReport.CreateReport();
                //File.Delete(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");

                //otkrij vidljivost legende grafika
                plotterPrint.LegendVisible = true;
                //otkrij vrati markere
                showPrintScreenMarkers();

                #region Remarks

                TextRange textRange = new TextRange(window.OnlineMode.OnHeader.RemarkOfTesting.rtfNapomena.Document.ContentStart, window.OnlineMode.OnHeader.RemarkOfTesting.rtfNapomena.Document.ContentEnd);

                string textRangeStr = textRange.Text;
                textRangeStr = textRangeStr.Replace("\r\n", String.Empty);
                if (textRangeStr.Length <= Constants.MAXREMARKSTESTINGLENGTH)
                {

                }
                else
                {
                    MessageBox.Show("Dužina napomene može biti najviše " + Constants.MAXREMARKSTESTINGLENGTH + " znakova.");
                }

                #endregion

                //pri pravljenu izvestaja zatvori sve prozore vezane za ulazne i izlazne podatke
                window.OnlineMode.ResultsInterface.Close();
                window.OnlineMode.OnHeader.Close();

                //zapamti tacke fitovanog grafika u tekstualni fajl, koje ces posle koristiti kod provere n-a
                //List<string> fittingsContext = new List<string>();
                //for (int i = 0; i < window.Plotting.DataReader.FittingPreassureInMPa.Count; i++ )
                //{
                //    fittingsContext.Add(window.Plotting.DataReader.FittingRelativeElongation[i].ToString() + "\t" +window.Plotting.DataReader.FittingPreassureInMPa[i].ToString());
                //}

                //string name = Constants.unsavedFilepath;
                //string namenFaktor = name.Split('.').ElementAt(0);
                //namenFaktor += ".nFaktor";
                //File.WriteAllLines(namenFaktor, fittingsContext.ToArray());
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnPrintSampleOnlyMakeReport_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        #region filePathForSampleReport


        private void btnChooseSampleReportPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = String.Empty;
                string extensionXml = "xml";
                bool _okDatabasePath = false;
                System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
                openDlg.Filter = "|*.xml";

                // Show open file dialog box 
                System.Windows.Forms.DialogResult result = openDlg.ShowDialog();

                // Process open file dialog box results 
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = openDlg.FileName;
                    string ext = System.IO.Path.GetExtension(openDlg.FileName);
                    string check = ext.Substring(1, ext.Length - 1);

                    if (extensionXml.Equals(check))
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
                        System.Windows.Forms.MessageBox.Show("Izabrani fajl " + filePath + " nije xml fajl! Molimo vas učitajte fajl sa ispravnom ekstenzijom!", "POKUŠAJ UČITAVANJA NEISPRAVNOG FORMATA XML FAJLA");
                        return;
                    }
                }

                tfFilepathSampleReport.Text = filePath;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnChooseSampleReportPath_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        private void btnLoadSampleReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tfFilepathSampleReport.Text == string.Empty)
                {
                    return;
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (window.OnlineMode.ResultsInterface == null)
                {
                    window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                }


                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(tfFilepathSampleReport.Text).ToList();
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
                    #region GeneralData

                    LastInputOutputSavedData.tfOperator_GeneralData = xn[Constants.XML_GeneralData_OPERATOR].InnerText;
                    LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData = xn[Constants.XML_GeneralData_BRZBIZVESTAJA].InnerText;
                    LastInputOutputSavedData.tfBrUzorka_GeneralData = xn[Constants.XML_GeneralData_BRUZORKA].InnerText;
                    LastInputOutputSavedData.tfSarza_GeneralData = xn[Constants.XML_GeneralData_SARZA].InnerText;
                    LastInputOutputSavedData.tfRadniNalog_GeneralData = xn[Constants.XML_GeneralData_RADNINALOG].InnerText;
                    LastInputOutputSavedData.tfNarucilac_GeneralData = xn[Constants.XML_GeneralData_NARUCILAC].InnerText;

                    #endregion

                    #region ConditionsOfTesting

                    LastInputOutputSavedData.tfStandard_ConditionsOfTesting = xn[Constants.XML_ConditionsOfTesting_STANDARD].InnerText;
                    LastInputOutputSavedData.tfMetoda_ConditionsOfTesting = xn[Constants.XML_ConditionsOfTesting_METODA].InnerText;
                    LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting = xn[Constants.XML_ConditionsOfTesting_STANDARDZAN].InnerText;
                    LastInputOutputSavedData.tfMasina_ConditionsOfTesting = xn[Constants.XML_ConditionsOfTesting_MASINA].InnerText;
                    LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting = xn[Constants.XML_ConditionsOfTesting_OPSEGMASINE].InnerText;
                    LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting = xn[Constants.XML_ConditionsOfTesting_TEMPERATURA].InnerText;
                    string Ekstenziometar = xn[Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR].InnerText;

                    if (Ekstenziometar.Equals(Constants.DA) == true)
                    {
                        LastInputOutputSavedData.rbtnYes_ConditionsOfTesting = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnYes_ConditionsOfTesting = "False";
                    }
                    if (Ekstenziometar.Equals(Constants.NE) == true)
                    {
                        LastInputOutputSavedData.rbtnNo_ConditionsOfTesting = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnNo_ConditionsOfTesting = "False";
                    }

                    #endregion

                    #region MaterialForTesting

                    LastInputOutputSavedData.tfProizvodjac_MaterialForTesting = xn[Constants.XML_MaterialForTesting_PROIZVODJAC].InnerText;
                    LastInputOutputSavedData.tfDobavljac_MaterialForTesting = xn[Constants.XML_MaterialForTesting_DOBAVLJAC].InnerText;
                    LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting = xn[Constants.XML_MaterialForTesting_POLAZNIKVALITET].InnerText;
                    LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting = xn[Constants.XML_MaterialForTesting_NAZIVNADEBLJINA].InnerText;
                    string NacinPrerade = xn[Constants.XML_MaterialForTesting_NACINPRERADE].InnerText;
                    if (NacinPrerade.Equals(Constants.VALJANI) == true)
                    {
                        LastInputOutputSavedData.rbtnValjani_MaterialForTesting = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnValjani_MaterialForTesting = "False";
                    }
                    if (NacinPrerade.Equals(Constants.VUČENI) == true)
                    {
                        LastInputOutputSavedData.rbtnVuceni_MaterialForTesting = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnVuceni_MaterialForTesting = "False";
                    }
                    if (NacinPrerade.Equals(Constants.LIVENI) == true)
                    {
                        LastInputOutputSavedData.rbtnLiveni_MaterialForTesting = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnLiveni_MaterialForTesting = "False";
                    }
                    if (NacinPrerade.Equals(Constants.KOVANI) == true)
                    {
                        LastInputOutputSavedData.rbtnKovani_MaterialForTesting = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnKovani_MaterialForTesting = "False";
                    }

                    #endregion

                    #region onlineHeader

                    string Oblik = xn[Constants.XML_Epruveta_EPRUVETAOBLIK].InnerText;
                    if (Oblik.Equals(Constants.OBRADJENA) == true)
                    {
                        LastInputOutputSavedData.rbtnEpvOblikObradjena = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvOblikObradjena = "False";
                    }
                    if (Oblik.Equals(Constants.NEOBRADJENA) == true)
                    {
                        LastInputOutputSavedData.rbtnEpvOblikNeobradjena = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvOblikNeobradjena = "False";
                    }

                    string Tip = xn[Constants.XML_Epruveta_TIP].InnerText;
                    if (Tip.Equals(Constants.PROPORCIONALNA) == true)
                    {
                        LastInputOutputSavedData.rbtnEpvTipProporcionalna = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvTipProporcionalna = "False";
                    }
                    if (Tip.Equals(Constants.NEPROPORCIONALNA) == true)
                    {
                        LastInputOutputSavedData.rbtnEpvTipNeproporcionalna = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvTipNeproporcionalna = "False";
                    }

                    string k = xn[Constants.XML_Epruveta_K].InnerText;
                    if (k.Equals(Constants.K1) == true)
                    {
                        LastInputOutputSavedData.rbtnEpvK1 = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvK1 = "False";
                    }
                    if (k.Equals(Constants.K2) == true)
                    {
                        LastInputOutputSavedData.rbtnEpvK2 = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvK2 = "False";
                    }


                    string VrstaEpruvete = xn[Constants.XML_Epruveta_VRSTAEPRUVETE].InnerText;
                    LastInputOutputSavedData.a0Pravougaona = String.Empty;
                    LastInputOutputSavedData.b0Pravougaona = String.Empty;
                    LastInputOutputSavedData.b0Pravougaona = String.Empty;
                    LastInputOutputSavedData.D0Kruzna = String.Empty;
                    LastInputOutputSavedData.D0Cevasta = String.Empty;
                    LastInputOutputSavedData.a0Cevasta = String.Empty;
                    LastInputOutputSavedData.D0Deocev = String.Empty;
                    LastInputOutputSavedData.a0Deocev = String.Empty;
                    LastInputOutputSavedData.b0Deocev = String.Empty;
                    LastInputOutputSavedData.d0Sestaugaona = String.Empty;
                    if (VrstaEpruvete.Equals(Constants.PRAVOUGAONA))
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "True";
                        LastInputOutputSavedData.a0Pravougaona = xn[Constants.a0].InnerText;
                        LastInputOutputSavedData.b0Pravougaona = xn[Constants.b0].InnerText;
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona = "False";
                    }
                    if (VrstaEpruvete.Equals(Constants.KRUZNA))
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaKruzna = "True";
                        LastInputOutputSavedData.D0Kruzna = xn[Constants.D0].InnerText;
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaKruzna = "False";
                    }
                    if (VrstaEpruvete.Equals(Constants.CEVASTA))
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaCevasta = "True";
                        LastInputOutputSavedData.D0Cevasta = xn[Constants.D0].InnerText;
                        LastInputOutputSavedData.a0Cevasta = xn[Constants.a0].InnerText;
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaCevasta = "False";
                    }
                    if (VrstaEpruvete.Equals(Constants.DEOCEVI))
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaDeocev = "True";
                        LastInputOutputSavedData.D0Deocev = xn[Constants.D0].InnerText;
                        LastInputOutputSavedData.a0Deocev = xn[Constants.a0].InnerText;
                        LastInputOutputSavedData.b0Deocev = xn[Constants.b0].InnerText;
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaDeocev = "False";
                    }
                    if (VrstaEpruvete.Equals(Constants.SESTAUGAONA))
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "True";
                        LastInputOutputSavedData.d0Sestaugaona = xn[Constants.d0].InnerText;
                    }
                    else
                    {
                        LastInputOutputSavedData.rbtnEpvVrstaSestaugaona = "False";
                    }

                    LastInputOutputSavedData.tfS0 = xn[Constants.XML_Epruveta_S0].InnerText;
                    LastInputOutputSavedData.tfL0 = xn[Constants.XML_Epruveta_L0].InnerText;
                    LastInputOutputSavedData.tfLc = xn[Constants.XML_Epruveta_LC].InnerText;

                    #endregion

                    #region PositionOfTube

                    LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube = xn[Constants.XML_PositionOfTube_PRAVACVALJANJA].InnerText;
                    LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube = xn[Constants.XML_PositionOfTube_SIRINATRAKE].InnerText;
                    LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube = xn[Constants.XML_PositionOfTube_DUZINATRAKE].InnerText;

                    #endregion

                    #region RemarkOfTesting

                    if (xn[Constants.XML_RemarkOfTesting_NAPOMENA] != null)
                    {
                        LastInputOutputSavedData.rtfNapomena_RemarkOfTesting = xn[Constants.XML_RemarkOfTesting_NAPOMENA].InnerText;
                    }
                    else
                    {
                        LastInputOutputSavedData.rtfNapomena_RemarkOfTesting = String.Empty;
                    }

                    #endregion

                    #region ResultsInterface

                    LastInputOutputSavedData.tfLu_ResultsInterface = xn[Constants.XML_ResultsInterface_Lu].InnerText;

                    LastInputOutputSavedData.tfRp02_ResultsInterface = xn[Constants.XML_ResultsInterface_Rp02].InnerText;
                    if (LastInputOutputSavedData.tfRp02_ResultsInterface.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbRp02_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbRp02_ResultsInterface = "True";
                    }

                    LastInputOutputSavedData.tfRt05_ResultsInterface = xn[Constants.XML_ResultsInterface_Rt05].InnerText;
                    if (LastInputOutputSavedData.tfRt05_ResultsInterface.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbRt05_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbRt05_ResultsInterface = "True";
                    }

                    LastInputOutputSavedData.tfReL_ResultsInterface = xn[Constants.XML_ResultsInterface_ReL].InnerText;
                    if (LastInputOutputSavedData.tfReL_ResultsInterface.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbReL_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbReL_ResultsInterface = "True";
                    }

                    LastInputOutputSavedData.tfReH_ResultsInterface = xn[Constants.XML_ResultsInterface_ReH].InnerText;
                    if (LastInputOutputSavedData.tfReH_ResultsInterface.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbReH_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbReH_ResultsInterface = "True";
                    }

                    LastInputOutputSavedData.tfRm_ResultsInterface = xn[Constants.XML_ResultsInterface_Rm].InnerText;


                    LastInputOutputSavedData.rbtnRp02_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnRt05_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReL_ResultsInterface = "False";
                    LastInputOutputSavedData.rbtnReH_ResultsInterface = "False";
                    foreach (string s in myXmlStrings)
                    {
                        if (s.Contains(Constants.XML_ResultsInterface_Rp02_Rm) == true)
                        {
                            LastInputOutputSavedData.rbtnRp02_ResultsInterface = "True";
                            LastInputOutputSavedData.tfRRm_ResultsInterface = xn[Constants.XML_ResultsInterface_Rp02_Rm].InnerText;
                        }
                        if (s.Contains(Constants.XML_ResultsInterface_Rt05_Rm) == true)
                        {
                            LastInputOutputSavedData.rbtnRt05_ResultsInterface = "True";
                            LastInputOutputSavedData.tfRRm_ResultsInterface = xn[Constants.XML_ResultsInterface_Rt05_Rm].InnerText;
                        }
                        if (s.Contains(Constants.XML_ResultsInterface_ReL_Rm) == true)
                        {
                            LastInputOutputSavedData.rbtnReL_ResultsInterface = "True";
                            LastInputOutputSavedData.tfRRm_ResultsInterface = xn[Constants.XML_ResultsInterface_ReL_Rm].InnerText;
                        }
                        if (s.Contains(Constants.XML_ResultsInterface_ReH_Rm) == true)
                        {
                            LastInputOutputSavedData.rbtnReH_ResultsInterface = "True";
                            LastInputOutputSavedData.tfRRm_ResultsInterface = xn[Constants.XML_ResultsInterface_ReH_Rm].InnerText;
                        }
                    }


                    LastInputOutputSavedData.tfF_ResultsInterface = xn[Constants.XML_ResultsInterface_F].InnerText;
                    LastInputOutputSavedData.tfFm_ResultsInterface = xn[Constants.XML_ResultsInterface_Fm].InnerText;
                    LastInputOutputSavedData.tfAg_ResultsInterface = xn[Constants.XML_ResultsInterface_Ag].InnerText;
                    LastInputOutputSavedData.tfAgt_ResultsInterface = xn[Constants.XML_ResultsInterface_Agt].InnerText;
                    LastInputOutputSavedData.tfA_ResultsInterface = xn[Constants.XML_ResultsInterface_A].InnerText;
                    LastInputOutputSavedData.tfAt_ResultsInterface = xn[Constants.XML_ResultsInterface_At].InnerText;
                    LastInputOutputSavedData.tfSu_ResultsInterface = xn[Constants.XML_ResultsInterface_Su].InnerText;
                    LastInputOutputSavedData.tfZ_ResultsInterface = xn[Constants.XML_ResultsInterface_Z].InnerText;

                    LastInputOutputSavedData.tfn_ResultsInterface = xn[Constants.XML_ResultsInterface_n].InnerText;
                    if (LastInputOutputSavedData.tfn_ResultsInterface.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbn_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbn_ResultsInterface = "True";
                    }

                    if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") == true)
                    {
                        LastInputOutputSavedData.au = xn[Constants.au].InnerText;
                        LastInputOutputSavedData.bu = xn[Constants.bu].InnerText;
                    }
                    if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") == true)
                    {
                        LastInputOutputSavedData.Du = xn[Constants.Du].InnerText;
                    }


                    LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = xn[Constants.XML_ResultsInterface_Rmax].InnerText;
                    LastInputOutputSavedData.isE2E4BorderSelected = xn[Constants.XML_isE2E4BorderSelected].InnerText;
                    if (LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbRmax_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbRmax_ResultsInterface = "True";
                    }
                    string e_R2 = xn[Constants.XML_ResultsInterface_eR2].InnerText;
                    if (e_R2.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbe2_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbe2_ResultsInterface = "True";
                        List<string> e2 = e_R2.Split('-').ToList();
                        LastInputOutputSavedData.e2Min_ResultsInterface = e2.ElementAt(0);
                        LastInputOutputSavedData.e2Max_ResultsInterface = e2.ElementAt(1).Substring(0, e2[1].Length - 1);
                    }
                    string e_R4 = xn[Constants.XML_ResultsInterface_eR4].InnerText;
                    if (e_R4.Equals(String.Empty) == true)
                    {
                        LastInputOutputSavedData.chbe4_ResultsInterface = "False";
                    }
                    else
                    {
                        LastInputOutputSavedData.chbe4_ResultsInterface = "True";
                        List<string> e4 = e_R4.Split('-').ToList();
                        LastInputOutputSavedData.e4Min_ResultsInterface = e4.ElementAt(0);
                        LastInputOutputSavedData.e4Max_ResultsInterface = e4.ElementAt(1).Substring(0, e4[1].Length - 1);
                    }

                    #endregion
                }


                //btnPrintSampleOnlyMakeReport.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                //MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                //sakrij vidljivost legende grafika
                plotterPrint.LegendVisible = false;

                //// Get file name.
                //string name = tfFilepathSampleReport.Text;
                ////GetAutomaticAnimation file name
                //string namePng = name.Split('.').ElementAt(0);
                //namePng += ".png";
                //window.Plotting.Printscreen.plotterPrint.SaveScreenshot(namePng);

                //PDFSampleReport pdfSampleReport = new PDFSampleReport(11.5, 8.7, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                PDFSampleReport pdfSampleReport = new PDFSampleReport(12, 9, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                pdfSampleReport.CreateReport(false, tfFilepathSampleReport.Text);
                //File.Delete(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");

                //otkrij vidljivost legende grafika
                plotterPrint.LegendVisible = true;

                #region Remarks

                TextRange textRange = new TextRange(window.OnlineMode.OnHeader.RemarkOfTesting.rtfNapomena.Document.ContentStart, window.OnlineMode.OnHeader.RemarkOfTesting.rtfNapomena.Document.ContentEnd);

                string textRangeStr = textRange.Text;
                textRangeStr = textRangeStr.Replace("\r\n", String.Empty);
                if (textRangeStr.Length <= Constants.MAXREMARKSTESTINGLENGTH)
                {

                }
                else
                {
                    MessageBox.Show("Dužina napomene može biti najviše " + Constants.MAXREMARKSTESTINGLENGTH + " znakova.");
                }

                #endregion

                //pri pravljenu izvestaja zatvori sve prozore vezane za ulazne i izlazne podatke
                window.OnlineMode.ResultsInterface.Close();
                window.OnlineMode.OnHeader.Close();

                //string namePdf = tfFilepathSampleReport.Text.Split('.').ElementAt(0);
                //namePdf += ".pdf";

                Process Proc = new Process();
                Proc.StartInfo = new ProcessStartInfo(pdfSampleReport.FileName);
                Proc.Start();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnLoadSampleReport_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void btnLoadSumReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tfFilepathSumReport.Text == string.Empty)
                {
                    return;
                }
                List<string> checkForPendulum = tfFilepathSumReport.Text.Split('.').ToList();

                if (checkForPendulum[0].EndsWith("klatno") == true)
                {
                    MessageBox.Show("Treba da učitate zbirni izveštaj za kidalicu a ne za klatno");
                    return;
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                sumReport = new SumReportLastLoad();
                XElement xmlLoad = XElement.Load(tfFilepathSumReport.Text);
                IEnumerable<XElement> allChildElements = xmlLoad.Elements();
                string strXmlContext = xmlLoad.ToString();
                if (strXmlContext.Contains(Constants.XML_root_SUMReport) == false)
                {
                    MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                    return;
                }

                SumReportRecord sumRecord;
                string brzbIzvestaja = String.Empty;
                string polazniKvalitet = String.Empty;
                string nazivnaDebljina = String.Empty;
                string ispitivac = String.Empty;
                string brUzorka = String.Empty;
                string sarza = String.Empty;
                string rm = String.Empty;
                string rp02 = String.Empty;
                string rt05 = String.Empty;
                string reL = String.Empty;
                string reH = String.Empty;
                string a = String.Empty;
                string at = String.Empty;
                string n = String.Empty;
                string z = String.Empty;
                string napomena = String.Empty;

                foreach (var child in allChildElements)
                {
                    IEnumerable<XElement> brzbIzvestajaNodes = child.Elements(Constants.XML_brzbIzvestaja_SUMReport);
                    brzbIzvestaja = brzbIzvestajaNodes.ElementAt(0).Value;
                    IEnumerable<XElement> polazniKvalitetNodes = child.Elements(Constants.XML_polazniKvalitet_SUMReport);
                    polazniKvalitet = polazniKvalitetNodes.ElementAt(0).Value;
                    IEnumerable<XElement> nazivnaDebljinaNodes = child.Elements(Constants.XML_nazivnaDebljina_SUMReport);
                    nazivnaDebljina = nazivnaDebljinaNodes.ElementAt(0).Value;
                    IEnumerable<XElement> ispitivacNodes = child.Elements(Constants.XML_ispitivac_SUMReport);
                    ispitivac = ispitivacNodes.ElementAt(0).Value;
                    IEnumerable<XElement> brUzorkaNodes = child.Elements(Constants.XML_brUzorka_SUMReport);
                    brUzorka = brUzorkaNodes.ElementAt(0).Value;
                    IEnumerable<XElement> sarzaNodes = child.Elements(Constants.XML_sarza_SUMReport);
                    sarza = sarzaNodes.ElementAt(0).Value;
                    IEnumerable<XElement> rmNodes = child.Elements(Constants.XML_Rm_SUMReport);
                    rm = rmNodes.ElementAt(0).Value;
                    IEnumerable<XElement> rp02Nodes = child.Elements(Constants.XML_Rp02_SUMReport);
                    rp02 = rp02Nodes.ElementAt(0).Value;
                    IEnumerable<XElement> rt05Nodes = child.Elements(Constants.XML_Rt05_SUMReport);
                    rt05 = rt05Nodes.ElementAt(0).Value;
                    IEnumerable<XElement> reLNodes = child.Elements(Constants.XML_ReL_SUMReport);
                    reL = reLNodes.ElementAt(0).Value;
                    IEnumerable<XElement> reHNodes = child.Elements(Constants.XML_ReH_SUMReport);
                    reH = reHNodes.ElementAt(0).Value;
                    IEnumerable<XElement> aNodes = child.Elements(Constants.XML_A_SUMReport);
                    a = aNodes.ElementAt(0).Value;
                    IEnumerable<XElement> atNodes = child.Elements(Constants.XML_At_SUMReport);
                    at = atNodes.ElementAt(0).Value;
                    IEnumerable<XElement> nNodes = child.Elements(Constants.XML_N_SUMReport);
                    n = nNodes.ElementAt(0).Value;
                    IEnumerable<XElement> zNodes = child.Elements(Constants.XML_Z_SUMReport);
                    z = zNodes.ElementAt(0).Value;
                    IEnumerable<XElement> napomenaNodes = child.Elements(Constants.XML_napomena_SUMReport);
                    napomena = napomenaNodes.ElementAt(0).Value;



                    sumRecord = new SumReportRecord(brzbIzvestaja, polazniKvalitet, nazivnaDebljina, ispitivac, brUzorka, sarza, rm, rp02, rt05, reL, reH, a, at, n, z, napomena);
                    sumReport.Records.Add(sumRecord);
                }

                PDFSumReport pdfSumReport = new PDFSumReport(8.5, 11, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface);
                pdfSumReport.CreateReport(sumReport);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnLoadSumReport_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #region filePathForSumReport

        private void btnChooseSumReportPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string filePath = String.Empty;
                string extensionXml = "xml";
                bool _okDatabasePath = false;
                System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
                openDlg.Filter = "|*.xml";

                // Show open file dialog box 
                System.Windows.Forms.DialogResult result = openDlg.ShowDialog();

                // Process open file dialog box results 
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    filePath = openDlg.FileName;
                    string ext = System.IO.Path.GetExtension(openDlg.FileName);
                    string check = ext.Substring(1, ext.Length - 1);

                    if (extensionXml.Equals(check))
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
                        System.Windows.Forms.MessageBox.Show("Izabrani fajl " + filePath + " nije xml fajl! Molimo vas učitajte fajl sa ispravnom ekstenzijom!", "POKUŠAJ UČITAVANJA NEISPRAVNOG FORMATA XML FAJLA");
                        return;
                    }
                }

                tfFilepathSumReport.Text = filePath;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnChooseSumReportPath_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        private void btnPrintSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //only for testing
                //testTensileMachineGraphics.OnlineModeFolder.Input_Data.GeneralData gd = new Input_Data.GeneralData(onHeader);
                //gd.Show();

                //testTensileMachineGraphics.OnlineModeFolder.Input_Data.ConditionsOfTesting ct = new Input_Data.ConditionsOfTesting(onHeader);
                //ct.Show();

                //testTensileMachineGraphics.OnlineModeFolder.Input_Data.MaterialForTesting mTesting = new Input_Data.MaterialForTesting(onHeader);
                //mTesting.Show();


                //testTensileMachineGraphics.OnlineModeFolder.Input_Data.PositionOfTube pOfTube = new Input_Data.PositionOfTube(onHeader);
                //pOfTube.Show();

                //testTensileMachineGraphics.OnlineModeFolder.Input_Data.RemarkOfTesting remark = new Input_Data.RemarkOfTesting(onHeader);
                //remark.Show();

                //only for testing



                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                window.Plotting.Printscreen.plotterPrint.SaveScreenshot(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");

                //PDFSampleReport pdfSampleReport = new PDFSampleReport(11, 8.5, onHeader,window.Plotting,resInterface);
                PDFSampleReport pdfSampleReport = new PDFSampleReport(12, 8.5, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                //pdfSampleReport.CreateReport();
                //File.Delete(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");
                string DefaultPath2 = Properties.Settings.Default.SaveDirectory + LastInputOutputSavedData.tfBrUzorka_GeneralData;
                DefaultPath2 = DefaultPath2.Replace('/', '_');

                //PDFSampleReport pdfSampleReport = new PDFSampleReport(11.5, 8.7, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                DefaultPath2 += ".pdf";
                plotting.OnlineModeInstance.SendToPrinter(DefaultPath2/*Constants.PATHOFSAMPLEREPORT*/);

                //string DefaultPath3 = Properties.Settings.Default.SaveDirectoryForReports + LastInputOutputSavedData.tfBrUzorka_GeneralData;
                //DefaultPath3 = DefaultPath3.Replace('/', '_');

                //PDFSampleReport pdfSampleReport = new PDFSampleReport(11.5, 8.7, window.OnlineMode.OnHeader, window.Plotting, window.OnlineMode.ResultsInterface, DefaultPath);
                //DefaultPath3 += ".pdf";
                //plotting.OnlineModeInstance.SendToPrinter(DefaultPath3); 


                if (window.OnlineMode.IsCurrentProgressWrittingInOnlineFile == true)
                {
                    MessageBox.Show("Online upis je još uvek u toku!");
                    return;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnPrintSample_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private bool isSavingInDuration = false;

        private void btnSaveSamplePrintMode_Click(object sender, RoutedEventArgs e)
        {

            try
            {


                isSavingInDuration = true;
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);


                if (chbCalculateNManual.IsChecked == false)
                {

                    //uzmi novi screenshot za grafik koji ce da sluzi pri pamcenju a kasnije pri kliku na dume Ucitaj Izvestaj
                    /*************************************************************/
                    //save the graphic

                    //sakrij vidljivost legende grafika
                    this.plotterPrint.LegendVisible = false;
                    //sakrij markere
                    this.HidePrintScreenMarkers();

                    //this.plotterPrint.SaveScreenshot(Constants.sampleReportGraphicFilepath);
                    this.plotterPrint.SaveScreenshot(Properties.Settings.Default.sampleReportGraphicFilepath);

                    //ako postoje grafici za promenu napona i izduzenja i njih zapamti
                    //OVO OVDE SE NE IZVRSAVA
                    //JEL SE GRAFICI PROMENE NAPONA I IZDUZENJA NE MENJAJU KADA SE CEKIRA RUCNO RACUNANJE n-a
                    //MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                    //if (window.OnlineMode.chbStartSampleShowChangedPar.IsChecked == true)
                    //{
                    //if (window.OnlineMode.VXY != null)
                    //{
                    //    if (window.OnlineMode.VXY.plotterChangeOfR != null)
                    //    {
                    //        window.OnlineMode.VXY.plotterChangeOfR.SaveScreenshot(Constants.sampleReportGraphicFilepathChangeOfR);
                    //    }
                    //    if (window.OnlineMode.VXY.plotterChangeOfE != null)
                    //    {
                    //        window.OnlineMode.VXY.plotterChangeOfE.SaveScreenshot(Constants.sampleReportGraphicFilepathChangeOfE);
                    //    }
                    //}
                    //}

                    //otkrij markere
                    this.ShowPrintScreenMarkers();
                    //otkrij vidljivost legende grafika
                    this.plotterPrint.LegendVisible = true;
                    /*************************************************************/
                }


                window.OnlineMode.btnSaveSample.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                isSavingInDuration = false;

                List<string> inputOutputLines = new List<string>();
                if (plotting.OnlineModeInstance.IsLessThanOne == false)
                {
                    plotting.OnlineModeInstance.setResultsInterface(string.Empty, string.Empty);
                }


                //zabelezi zadnje upisan online header
                plotting.OnlineModeInstance.WriteXMLLastOnlineHeader();
                //zabelezi izlazne podatke zadnje pokidanog uzorka
                plotting.OnlineModeInstance.WriteXMLLastResultsInterface();
                //zabelezi u ovu strukturu ulazne i izlazne podatke zadnje pokidanog uzorka
                LastInputOutputSavedData.GetData();
                //ovde cuvaj informacije input-output fajla
                plotting.OnlineModeInstance.getInputOutputLines(ref inputOutputLines);

                plotting.OnlineModeInstance.ResultsInterface.tfRmax.Text = LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface;
                plotting.OnlineModeInstance.ResultsInterface.tfE2.Text = LastInputOutputSavedData.e2Max_ResultsInterface;
                plotting.OnlineModeInstance.ResultsInterface.tfE4.Text = LastInputOutputSavedData.e4Max_ResultsInterface;

            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnSaveSamplePrintMode_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void deleteOnlyFittingLinePrintScreen()
        {
            try
            {
                var numberOfOffline = this.plotterPrint.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "grafik").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = this.plotterPrint.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "grafik").Single();
                    this.plotterPrint.Children.Remove(lineToRemove);
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void deleteOnlyFittingLinePrintScreen()}", System.DateTime.Now);
            }
        }

        public void HideT1T2T3() 
        {
            try
            {
                _MarkerGraph.Visibility = Visibility.Hidden;
                _MarkerGraph2.Visibility = Visibility.Hidden;
                _MarkerGraph3.Visibility = Visibility.Hidden;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void HideT1T2T3()}", System.DateTime.Now);
            }
        }

        private void deletePrintScreenMarkers() 
        {
            try
            {
                _MarkerGraph.DataSource = null;
                _MarkerGraph2.DataSource = null;
                _MarkerGraph3.DataSource = null;
                _MarkerGraph4.DataSource = null;
                _MarkerGraph5.DataSource = null;
                _MarkerGraph6.DataSource = null;
                _MarkerGraph7.DataSource = null;
                _MarkerGraph8.DataSource = null;
                _MarkerGraph9.DataSource = null;
                _MarkerGraph10.DataSource = null;

                _MarkerGraph11.DataSource = null;
                _MarkerGraph12.DataSource = null;
                _MarkerGraph13.DataSource = null;
                _MarkerGraph14.DataSource = null;
                _MarkerGraph15.DataSource = null;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void deletePrintScreenMarkers()}", System.DateTime.Now);
            }

        }

        public void DeleteOnlyNManualMarkers() 
        {
            try
            {
                _MarkerGraph11.DataSource = null;
                _MarkerGraph12.DataSource = null;
                _MarkerGraph13.DataSource = null;
                _MarkerGraph14.DataSource = null;
                _MarkerGraph15.DataSource = null;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void DeleteOnlyNManualMarkers()}", System.DateTime.Now);
            }
        }

        public void HidePrintScreenMarkers()
        {
            try
            {
                hidePrintScreenMarkers();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void HidePrintScreenMarkers()}", System.DateTime.Now);
            }
        }

        private void hidePrintScreenMarkers()
        {
            try
            {
                _MarkerGraph.Visibility = Visibility.Hidden;
                _MarkerGraph2.Visibility = Visibility.Hidden;
                _MarkerGraph3.Visibility = Visibility.Hidden;
                _MarkerGraph4.Visibility = Visibility.Hidden;
                _MarkerGraph5.Visibility = Visibility.Hidden;
                _MarkerGraph6.Visibility = Visibility.Hidden;
                _MarkerGraph7.Visibility = Visibility.Hidden;
                _MarkerGraph8.Visibility = Visibility.Hidden;
                _MarkerGraph9.Visibility = Visibility.Hidden;
                _MarkerGraph10.Visibility = Visibility.Hidden;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void hidePrintScreenMarkers()}", System.DateTime.Now);
            }
        }

        public void ShowPrintScreenMarkers()
        {
            try
            {
                showPrintScreenMarkers();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void ShowPrintScreenMarkers()}", System.DateTime.Now);
            }
        }

        private void showPrintScreenMarkers()
        {
            try
            {
                _MarkerGraph.Visibility = Visibility.Visible;
                _MarkerGraph2.Visibility = Visibility.Visible;
                _MarkerGraph3.Visibility = Visibility.Visible;
                _MarkerGraph4.Visibility = Visibility.Visible;
                _MarkerGraph5.Visibility = Visibility.Visible;
                _MarkerGraph6.Visibility = Visibility.Visible;
                _MarkerGraph7.Visibility = Visibility.Visible;
                _MarkerGraph8.Visibility = Visibility.Visible;
                _MarkerGraph9.Visibility = Visibility.Visible;
                _MarkerGraph10.Visibility = Visibility.Visible;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void showPrintScreenMarkers()}", System.DateTime.Now);
            }
        }

        public void SetRedColorForExistingSample()
        {
            try
            {
                setRedColorForExistingSample();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {public void SetRedColorForExistingSample()}", System.DateTime.Now);
            }
        }


        private void setRedColorForExistingSample() 
        {
            try
            {
                if (plotting != null && plotting.OnlineModeInstance != null && plotting.OnlineModeInstance.OnHeader != null && plotting.OnlineModeInstance.OnHeader.GeneralData != null)
                {
                    string pathOfFile = Properties.Settings.Default.SaveDirectory + plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorka.Text + "_" + plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorkaNumberOfSample.Text + ".txt";
                    if (File.Exists(pathOfFile) == true)
                    {
                        plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorka.Foreground = Brushes.Red;
                        plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorkaNumberOfSample.Foreground = Brushes.Red;
                    }
                    else
                    {
                        plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorka.Foreground = Brushes.Black;
                        plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorkaNumberOfSample.Foreground = Brushes.Black;
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setRedColorForExistingSample()}", System.DateTime.Now);
            }
        }

        private void btnEnterNewSamplePrintMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isSavingInDuration == true)
                {
                    MessageBox.Show("Pamcenje je u toku!");
                    return;
                }

               
                plotting.IsMessageShownForRangeReHX = false;
                plotting.BadCalculationHappened = false;

                chbChangeOfRAndE.IsChecked = false;
                plotting.EnableGraphicPlotting();

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                if (window.OnlineMode.OnHeader != null)
                {
                    window.OnlineMode.OnHeader.EnableInputs();
                }

                btnSampleDataPrintMode.IsEnabled = false;
                IsPrintScreenEmpty = true;
                isClickedNewMeasures = true;
                

                //window.Plotting.IsLuManualChanged = false;
                window.Plotting.IsClickedByMouse_Plotting_Rp02 = false;
                window.Plotting.IsClickedByMouse_Plotting_ReL = false;
                window.Plotting.IsClickedByMouse_Plotting_ReH = false;
                window.Plotting.IsClickedByMouse_Plotting_Rm = false;
                window.Plotting.IsClickedByMouse_Plotting_A = false;
                Rp02X = -1;
                Rp02Y = -1;
                ReLX = -1;
                ReLY = -1;
                ReHX = -1;
                ReHY = -1;
                RmX = -1;
                RmY = -1;
                AX = -1;
                AY = -1;
                deletePrintScreenMarkers();



                window.OnlineMode.IsClickedSampleDataAtPrintMode = true;
                window.OnlineMode.btnSampleData.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));



                //ocisti prethodni prikaz grafika
                deleteOnlyFittingLinePrintScreen();
                //ocisti markere
                deletePrintScreenMarkers();

                window.OnlineMode.OnHeader.GeneralData.tfBrUzorka.SelectAll();
                window.OnlineMode.OnHeader.GeneralData.tfBrUzorka.Focus();

                //ocisti podatke vezane za rucno racunanje N-a
                if (window.Plotting.Fs_FittingForManualNProperty != null)
                {
                    window.Plotting.Fs_FittingForManualNProperty.Clear();
                }
                if (window.Plotting.DeltaLsInProcForManualNProperty != null)
                {
                    window.Plotting.DeltaLsInProcForManualNProperty.Clear();
                    window.Plotting.NManual = 0;
                }
                if (nManualCalculation != null && IsNManualWindowCreated == true)
                {
                    nManualCalculation.Close();
                }
                if (window.Plotting.OnlineModeInstance.ResultsInterface != null)
                {
                    window.Plotting.OnlineModeInstance.ResultsInterface.Close();
                }

                window.OnlineMode.MaxPreassure = 0.0;
                window.OnlineMode.MaxElongation = 0.0;

                setRedColorForExistingSample();

                window.OnlineMode.createOnlineGraphics();

                if (chbPrikaziOpcije.IsChecked == true)
                {
                    plotting.btnShowOfflineOptions.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                }


               
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString(), System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnEnterNewSamplePrintMode_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
            
        }

        private void btnSampleDataPrintMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                window.OnlineMode.IsClickedSampleDataAtPrintMode = true;
                window.OnlineMode.btnSampleData.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));



                if (window.OnlineMode.ResultsInterface != null)
                {
                    window.OnlineMode.ResultsInterface.SetAuBuOrDu();
                    window.OnlineMode.ResultsInterface.Show();
                }
                if (chbCalculateNManual.IsChecked == true)
                {
                    if (nManualCalculation != null && IsNManualWindowCreated == false)
                    {
                        nManualCalculation = null;
                        nManualCalculation = new WindowForNManualCalculation(plotting, this);
                        nManualCalculation.Hide();
                        nManualCalculation.Show();
                    }
                    else
                    {
                        nManualCalculation.Hide();
                        nManualCalculation.Show();
                    }
                }

                if (plotting.IDontWantShowT1T2T3AtPrintScreen == true)
                {
                    plotting.Printscreen.HideT1T2T3();
                }

                if (chbChangeOfRAndE.IsChecked == true)
                {
                    if (window.OnlineMode.VXY.Height > 0)
                    {
                        window.OnlineMode.VXY.Hide();
                        window.OnlineMode.VXY.Show();
                    }
                }

                window.OnlineMode.ResultsInterface.SetLastCheckedRadioButton();
             
                LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = plotting.OnlineModeInstance.RmaxGlobal.ToString();

                List<string> rmaxwithpointList = File.ReadAllLines(Constants.RmaxWithPoint).ToList();
                LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = rmaxwithpointList[0];
                plotting.OnlineModeInstance.ResultsInterface.tfRmax.Text = LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface;
                LastInputOutputSavedData.e2Max_ResultsInterface = plotting.E2e4CalculationAfterManualFitting.ArrayE2Interval.Max().ToString();
                plotting.OnlineModeInstance.ResultsInterface.tfE2.Text = LastInputOutputSavedData.e2Max_ResultsInterface;
                LastInputOutputSavedData.e4Max_ResultsInterface = plotting.E2e4CalculationAfterManualFitting.ArrayE4Interval.Max().ToString();
                plotting.OnlineModeInstance.ResultsInterface.tfE4.Text = LastInputOutputSavedData.e4Max_ResultsInterface;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnSampleDataPrintMode_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private bool checkCorrectionOfOnlineFileHeader() 
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (window.OnlineMode.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    double aglobal;

                    bool isN = Double.TryParse(window.OnlineMode.OnHeader.strtfAGlobal, out aglobal);
                    if (isN == false)
                    {
                        MessageBox.Show("Parametar a mora biti unet kao broj !");
                        return false;
                    }

                    double bglobal;

                    bool isNN = Double.TryParse(window.OnlineMode.OnHeader.strtfBGlobal, out bglobal);
                    if (isNN == false)
                    {
                        MessageBox.Show("Parametar b mora biti unet kao broj !");
                        return false;
                    }
                }

                if (window.OnlineMode.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                {
                    double dglobal;

                    bool isN = Double.TryParse(window.OnlineMode.OnHeader.strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        MessageBox.Show("Parametar D mora biti unet kao broj !");
                        return false;
                    }
                }

                if (window.OnlineMode.OnHeader.rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    double dglobal;

                    bool isN = Double.TryParse(window.OnlineMode.OnHeader.strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        MessageBox.Show("Parametar D mora biti unet kao broj !");
                        return false;
                    }

                    double aglobal;

                    bool isNN = Double.TryParse(window.OnlineMode.OnHeader.strtfAGlobal, out aglobal);
                    if (isNN == false)
                    {
                        MessageBox.Show("Parametar a mora biti unet kao broj !");
                        return false;
                    }
                }


                if (window.OnlineMode.OnHeader.rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    double dglobal;

                    bool isN = Double.TryParse(window.OnlineMode.OnHeader.strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        MessageBox.Show("Parametar D mora biti unet kao broj !");
                        return false;
                    }

                    double aglobal;

                    bool isNN = Double.TryParse(window.OnlineMode.OnHeader.strtfAGlobal, out aglobal);
                    if (isNN == false)
                    {
                        MessageBox.Show("Parametar a mora biti unet kao broj !");
                        return false;
                    }

                    double bglobal;

                    bool isNNN = Double.TryParse(window.OnlineMode.OnHeader.strtfBGlobal, out bglobal);
                    if (isNNN == false)
                    {
                        MessageBox.Show("Parametar b mora biti unet kao broj !");
                        return false;
                    }
                }


                if (window.OnlineMode.OnHeader.rbtnEpvVrstaSestaugaona.IsChecked == true)
                {
                    double dglobal;

                    bool isN = Double.TryParse(window.OnlineMode.OnHeader.strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        MessageBox.Show("Parametar d mora biti unet kao broj !");
                        return false;
                    }
                }

                //set and save s0 and l0
                double s0 = Double.MinValue;

                bool isNNNN = Double.TryParse(window.OnlineMode.OnHeader.strs0, out s0);
                if (isNNNN == false)
                {
                    MessageBox.Show("Parametar So nije zabeležen u obliku broja!");
                    return false;
                }

                double l0 = Double.MinValue;

                isNNNN = Double.TryParse(window.OnlineMode.OnHeader.strl0, out l0);
                if (isNNNN == false)
                {
                    MessageBox.Show("Parametar Lo nije zabeležen u obliku broja!");
                    return false;
                }

                double lc = Double.MinValue;

                isNNNN = Double.TryParse(window.OnlineMode.OnHeader.strlc, out lc);
                if (isNNNN == false)
                {
                    MessageBox.Show("Parametar Lc nije zabeležen u obliku broja!");
                    return false;
                }

                OptionsInOnlineMode.S0 = s0;
                OptionsInOnlineMode.L0 = l0;

                window.OnlineMode.OptionsOnline.WriteXMLOnlineFile();

                return true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private bool checkCorrectionOfOnlineFileHeader()}", System.DateTime.Now);
                return false;
            }
        }

        /// <summary>
        /// sa promenom Lc-a menjaju se 4 crvene granice za promenu elongacije 
        /// </summary>
        private void setVelocityOfChangeParametersXYE2E4MinMax() 
        {

            try
            {

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                double LcLocal = 0;
                bool isN = double.TryParse(plotting.OnlineModeInstance.OnHeader.tfLc.Text, out LcLocal);

                if (isN == false)
                {
                    return;
                }



                if (plotting != null && plotting.OnlineModeInstance != null && plotting.OnlineModeInstance.VXY != null)
                {
                    plotting.OnlineModeInstance.VXY.LineE2min.Value = OptionsInOnlineManagingOfTTM.eR2 * (LcLocal) * 0.8;
                    plotting.OnlineModeInstance.VXY.LineE4min.Value = OptionsInOnlineManagingOfTTM.eR4 * (LcLocal) * 0.8;
                    plotting.OnlineModeInstance.VXY.LineE2max.Value = OptionsInOnlineManagingOfTTM.eR2 * (LcLocal) * 1.2;
                    plotting.OnlineModeInstance.VXY.LineE4max.Value = OptionsInOnlineManagingOfTTM.eR4 * (LcLocal) * 1.2;
                }
                if (window != null && window.Animation != null && window.Animation.VXYAnimation != null)
                {
                    window.Animation.VXYAnimation.LineE2min.Value = OptionsInOnlineManagingOfTTM.eR2 * (LcLocal) * 0.8;
                    window.Animation.VXYAnimation.LineE4min.Value = OptionsInOnlineManagingOfTTM.eR4 * (LcLocal) * 0.8;
                    window.Animation.VXYAnimation.LineE2max.Value = OptionsInOnlineManagingOfTTM.eR2 * (LcLocal) * 1.2;
                    window.Animation.VXYAnimation.LineE4max.Value = OptionsInOnlineManagingOfTTM.eR4 * (LcLocal) * 1.2;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void setVelocityOfChangeParametersXYE2E4MinMax()}", System.DateTime.Now);
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

                        }
                        if (textReader.Name.Equals("forceUpper1"))
                        {
                            ForceRangesOptions.forceUpper1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnMultiple1"))
                        {
                            ForceRangesOptions.nutnMultiple1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnDivide1"))
                        {
                            ForceRangesOptions.nutnDivide1 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }



                        if (textReader.Name.Equals("forceLower2"))
                        {
                            ForceRangesOptions.forceLower2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("forceUpper2"))
                        {
                            ForceRangesOptions.forceUpper2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnMultiple2"))
                        {
                            ForceRangesOptions.nutnMultiple2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnDivide2"))
                        {
                            ForceRangesOptions.nutnDivide2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }



                        if (textReader.Name.Equals("forceLower3"))
                        {
                            ForceRangesOptions.forceLower3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("forceUpper3"))
                        {
                            ForceRangesOptions.forceUpper3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnMultiple3"))
                        {
                            ForceRangesOptions.nutnMultiple3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnDivide3"))
                        {
                            ForceRangesOptions.nutnDivide3 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }




                        if (textReader.Name.Equals("forceLower4"))
                        {
                            ForceRangesOptions.forceLower4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("forceUpper4"))
                        {
                            ForceRangesOptions.forceUpper4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnMultiple4"))
                        {
                            ForceRangesOptions.nutnMultiple4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnDivide4"))
                        {
                            ForceRangesOptions.nutnDivide4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }




                        if (textReader.Name.Equals("forceLower5"))
                        {
                            ForceRangesOptions.forceLower5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("forceUpper5"))
                        {
                            ForceRangesOptions.forceUpper5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnMultiple5"))
                        {
                            ForceRangesOptions.nutnMultiple5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnDivide5"))
                        {
                            ForceRangesOptions.nutnDivide5 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }




                        if (textReader.Name.Equals("forceLower6"))
                        {
                            ForceRangesOptions.forceLower6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("forceUpper6"))
                        {
                            ForceRangesOptions.forceUpper6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnMultiple6"))
                        {
                            ForceRangesOptions.nutnMultiple6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

                        }
                        if (textReader.Name.Equals("nutnDivide6"))
                        {
                            ForceRangesOptions.nutnDivide6 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());

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

        private void btnGoToOnlineMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                loadXMLFileForceRanges();
                double opseg = 0;

                bool isN = double.TryParse(plotting.OnlineModeInstance.OnHeader.ConditionsOfTesting.tfBegOpsegMasine.Text, out opseg);

                if (ForceRangesOptions.forceUpper1 == opseg || ForceRangesOptions.forceUpper2 == opseg || ForceRangesOptions.forceUpper3 == opseg || ForceRangesOptions.forceUpper4 == opseg || ForceRangesOptions.forceUpper5 == opseg || ForceRangesOptions.forceUpper6 == opseg)
                {
                }
                else
                {
                    MessageBox.Show("Izabrali ste nepostojeci opseg sile!");
                    return;
                }


                if (plotting.OnlineModeInstance.OnHeader.GeneralData != null)
                {
                    if (plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrZbIzvestaja.Text.Equals(string.Empty) == true)
                    {
                        MessageBox.Show("Morate uneti informaciju o zbirnom izveštaju !");
                        return;
                    }

                    if (plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorka.Text.Equals(string.Empty) == true || plotting.OnlineModeInstance.OnHeader.GeneralData.tfBrUzorkaNumberOfSample.Text.Equals(string.Empty) == true)
                    {
                        MessageBox.Show("Morate uneti informaciju o pojedinačnom izveštaju !");
                        return;
                    }
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                setVelocityOfChangeParametersXYE2E4MinMax();

                if (window.OnlineMode.OnHeader != null)
                {
                    window.OnlineMode.OnHeader.Close();
                }

                if (isClickedNewMeasures == false)
                {
                    MessageBox.Show("Morate kliknuti na dugme NOVO MERENJE i uneti podatke novog merenja !");
                    return;
                }
                else
                {
                    isClickedNewMeasures = false;
                }

                //checkCorrectionOfOnlineFileHeader
                bool isOnlineFileHeaderCorrect = true;
                isOnlineFileHeaderCorrect = checkCorrectionOfOnlineFileHeader();
                if (isOnlineFileHeaderCorrect == false)
                {
                    return;
                }

                window.OnlineMode.WriteXMLLastOnlineHeader();

                window.OnlineMode.btnEnterNewSample.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                //sada selektujemo tab1 da bi se vratio u online mode
                window.tab_first.IsSelected = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void btnGoToOnlineMode_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbCalculateNManual_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                window.OnlineMode.MaxPreassure = 0.0;
                window.OnlineMode.MaxElongation = 0.0;

                if (IsPrintScreenEmpty == true)
                {
                    return;
                }

                if (plotting.Ag < OptionsInPlottingMode.EndIntervalForN)
                {
                    MessageBox.Show("Vrednost Ag je manja od postavljenje krajnje granice za racunanje n-a.\n Ag = " + plotting.Ag + " %    " + "A2 = " + OptionsInPlottingMode.EndIntervalForN + "% .");
                    chbCalculateNManual.IsChecked = false;
                    return;
                }

                if (plotting.NHardeningExponentManual.NumberOfSamples == 5)
                {

                    EnumerableDataSource<double> gX11 = new EnumerableDataSource<double>(xMarkers11);
                    EnumerableDataSource<double> gY11 = new EnumerableDataSource<double>(yMarkers11);
                    _MarkerGraph11.DataSource = new CompositeDataSource(gX11, gY11);

                    //no scaling - identity mapping
                    gX11.XMapping = xx => xx;
                    gY11.YMapping = yy => yy;

                    EnumerableDataSource<double> gX12 = new EnumerableDataSource<double>(xMarkers12);
                    EnumerableDataSource<double> gY12 = new EnumerableDataSource<double>(yMarkers12);
                    _MarkerGraph12.DataSource = new CompositeDataSource(gX12, gY12);

                    //no scaling - identity mapping
                    gX12.XMapping = xx => xx;
                    gY12.YMapping = yy => yy;

                    EnumerableDataSource<double> gX13 = new EnumerableDataSource<double>(xMarkers13);
                    EnumerableDataSource<double> gY13 = new EnumerableDataSource<double>(yMarkers13);
                    _MarkerGraph13.DataSource = new CompositeDataSource(gX13, gY13);

                    //no scaling - identity mapping
                    gX13.XMapping = xx => xx;
                    gY13.YMapping = yy => yy;

                    EnumerableDataSource<double> gX14 = new EnumerableDataSource<double>(xMarkers14);
                    EnumerableDataSource<double> gY14 = new EnumerableDataSource<double>(yMarkers14);
                    _MarkerGraph14.DataSource = new CompositeDataSource(gX14, gY14);

                    //no scaling - identity mapping
                    gX14.XMapping = xx => xx;
                    gY14.YMapping = yy => yy;

                    EnumerableDataSource<double> gX15 = new EnumerableDataSource<double>(xMarkers15);
                    EnumerableDataSource<double> gY15 = new EnumerableDataSource<double>(yMarkers15);
                    _MarkerGraph15.DataSource = new CompositeDataSource(gX15, gY15);

                    //no scaling - identity mapping
                    gX15.XMapping = xx => xx;
                    gY15.YMapping = yy => yy;



                    xMarkers11[0] = plotting.NHardeningExponentManual.DeltaLsInProcent[0];
                    xMarkers12[0] = plotting.NHardeningExponentManual.DeltaLsInProcent[1];
                    xMarkers13[0] = plotting.NHardeningExponentManual.DeltaLsInProcent[2];
                    xMarkers14[0] = plotting.NHardeningExponentManual.DeltaLsInProcent[3];
                    xMarkers15[0] = plotting.NHardeningExponentManual.DeltaLsInProcent[4];
                    yMarkers11[0] = plotting.PreassureForNManualProperty[0];
                    yMarkers12[0] = plotting.PreassureForNManualProperty[1];
                    yMarkers13[0] = plotting.PreassureForNManualProperty[2];
                    yMarkers14[0] = plotting.PreassureForNManualProperty[3];
                    yMarkers15[0] = plotting.PreassureForNManualProperty[4];



                    CirclePointMarker mkr11 = new CirclePointMarker();
                    mkr11.Fill = new SolidColorBrush(Colors.Black);
                    mkr11.Size = Constants.MARKERSIZE;
                    mkr11.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                    _MarkerGraph11.Marker = mkr11;

                    CirclePointMarker mkr12 = new CirclePointMarker();
                    mkr12.Fill = new SolidColorBrush(Colors.Black);
                    mkr12.Size = Constants.MARKERSIZE;
                    mkr12.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                    _MarkerGraph12.Marker = mkr12;

                    CirclePointMarker mkr13 = new CirclePointMarker();
                    mkr13.Fill = new SolidColorBrush(Colors.Black);
                    mkr13.Size = Constants.MARKERSIZE;
                    mkr13.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                    _MarkerGraph13.Marker = mkr13;

                    CirclePointMarker mkr14 = new CirclePointMarker();
                    mkr14.Fill = new SolidColorBrush(Colors.Black);
                    mkr14.Size = Constants.MARKERSIZE;
                    mkr14.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                    _MarkerGraph14.Marker = mkr14;

                    CirclePointMarker mkr15 = new CirclePointMarker();
                    mkr15.Fill = new SolidColorBrush(Colors.Black);
                    mkr15.Size = Constants.MARKERSIZE;
                    mkr15.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                    _MarkerGraph15.Marker = mkr15;



                    nManualCalculation = null;
                    nManualCalculation = new WindowForNManualCalculation(plotting, this);
                    //nManualCalculation.Show();
                    this.btnSampleDataPrintMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                    //uzmi novi screenshot za grafik koji ce da sluzi pri pamcenju a kasnije pri kliku na dume Ucitaj Izvestaj
                    /*************************************************************/
                    //save the graphic

                    //sakrij vidljivost legende grafika
                    plotting.Printscreen.plotterPrint.LegendVisible = false;
                    //sakrij markere
                    plotting.Printscreen.HidePrintScreenMarkers();

                    //plotting.Printscreen.plotterPrint.SaveScreenshot(Constants.sampleReportGraphicFilepath);
                    plotting.Printscreen.plotterPrint.SaveScreenshot(Properties.Settings.Default.sampleReportGraphicFilepath);

                    //ako postoje grafici za promenu napona i izduzenja i njih zapamti
                    //OVO OVDE SE NE IZVRSAVA
                    //JEL SE GRAFICI PROMENE NAPONA I IZDUZENJA NE MENJAJU KADA SE CEKIRA RUCNO RACUNANJE n-a
                    //MainWindow window = (MainWindow)MainWindow.GetWindow(plotting.Printscreen);
                    //if (window.OnlineMode.chbStartSampleShowChangedPar.IsChecked == true)
                    //{
                    //    if (window.OnlineMode.VXY != null)
                    //    {
                    //        if (window.OnlineMode.VXY.plotterChangeOfR != null)
                    //        {
                    //            window.OnlineMode.VXY.plotterChangeOfR.SaveScreenshot(Constants.sampleReportGraphicFilepathChangeOfR);
                    //        }
                    //        if (window.OnlineMode.VXY.plotterChangeOfE != null)
                    //        {
                    //            window.OnlineMode.VXY.plotterChangeOfE.SaveScreenshot(Constants.sampleReportGraphicFilepathChangeOfE);
                    //        }
                    //    }
                    //}

                    //otkrij markere
                    plotting.Printscreen.ShowPrintScreenMarkers();
                    //otkrij vidljivost legende grafika
                    plotting.Printscreen.plotterPrint.LegendVisible = true;
                    /*************************************************************/
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void chbCalculateNManual_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbCalculateNManual_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPrintScreenEmpty == true)
                {
                    return;
                }

                if (plotting.NHardeningExponentManual.NumberOfSamples == 5)
                {
                    _MarkerGraph11.DataSource = null;
                    _MarkerGraph12.DataSource = null;
                    _MarkerGraph13.DataSource = null;
                    _MarkerGraph14.DataSource = null;
                    _MarkerGraph15.DataSource = null;

                    nManualCalculation.Hide();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void chbCalculateNManual_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        public void Delete()
        {
            deleteOnlyPrintScreenLine();
            DeleteOnlyNManualMarkers();
            deletePrintScreenMarkers();
        }


        private void deleteOnlyPrintScreenLine()
        {
            try
            {
                var numberOfOffline = plotterPrint.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "grafik").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotterPrint.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "grafik").Single();
                    plotterPrint.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void deleteOnlyPrintScreenLine()}", System.DateTime.Now);
            }
        }

        private void chbChangeOfRAndE_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                window.OnlineMode.VXY.Visibility = Visibility.Visible;

                if (window.Animation.chbStartSampleShowChangedParAnimation.IsChecked == true)
                {
                    window.Animation.chbStartSampleShowChangedParAnimation.IsChecked = false;
                }
                if (window.Plotting.chbShowChangeOfRAndEe.IsChecked == true)
                {
                    window.Plotting.chbShowChangeOfRAndEe.IsChecked = false;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void chbChangeOfRAndE_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
          
        }

        private void chbChangeOfRAndE_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                window.OnlineMode.VXY.Visibility = Visibility.Hidden;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[PrintScreen.xaml.cs] {private void chbChangeOfRAndE_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
            
        }

        private void btnFooterOptions_Click(object sender, RoutedEventArgs e)
        {
            footerOptions = new FooterOptions();
            footerOptions.ShowDialog();
        }


  

    }
}