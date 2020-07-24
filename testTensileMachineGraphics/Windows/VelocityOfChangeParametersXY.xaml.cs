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
using testTensileMachineGraphics.PointViewModel;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using testTensileMachineGraphics.OnlineModeFolder;
using testTensileMachineGraphics.Options;
using Microsoft.Research.DynamicDataDisplay.Charts;
using System.IO;
using System.Xml;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using testTensileMachineGraphics.Reports;

namespace testTensileMachineGraphics.Windows
{
    /// <summary>
    /// X is equal to e and Y is equal to R
    /// </summary>
    public partial class VelocityOfChangeParametersXY : Window
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        public bool AmIOpen = false;

        private HorizontalLine lineRmax;
        private HorizontalLine lineRmin;
        
        private HorizontalLine lineE2min;
        public HorizontalLine LineE2min 
        {
            get { return lineE2min; }
            set { lineE2min = value; }
        }
        private HorizontalLine lineE2max;
        public HorizontalLine LineE2max
        {
            get { return lineE2max; }
            set { lineE2max = value; }
        }
        private HorizontalLine lineE4min;
        public HorizontalLine LineE4min
        {
            get { return lineE4min; }
            set { lineE4min = value; }
        }
        private HorizontalLine lineE4max;
        public HorizontalLine LineE4max
        {
            get { return lineE4max; }
            set { lineE4max = value; }
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

        #region Lastdatas

        private double lastChangeValueOfR;
        /// <summary>
        /// poslednja zabelezena Promena napona
        /// </summary>
        public double LastChangeValueOfR
        {
            get { return lastChangeValueOfR; }
            set { lastChangeValueOfR = value; }
        }

        private double lastChangeValueOfE;
        /// <summary>
        /// poslednja zabelezena Promena izduzenja
        /// </summary>
        public double LastChangeValueOfE
        {
            get { return lastChangeValueOfE; }
            set { lastChangeValueOfE = value; }
        }

        private double lastROfEndOfInterval;
        /// <summary>
        /// krajnja vrednost napona na kraju intervala
        /// </summary>
        public double LastROfEndOfInterval
        {
            get { return lastROfEndOfInterval; }
            set { lastROfEndOfInterval = value; }
        }

        private double lastEOfEndOfInterval;
        /// <summary>
        /// krajnja vrednost izduzenja na kraju intervala
        /// </summary>
        public double LastEOfEndOfInterval
        {
            get { return lastEOfEndOfInterval; }
            set { lastEOfEndOfInterval = value; }
        }

        private double lastTAUOfEndOfInterval;
        /// <summary>
        /// koliko je do sada proteklo vremena
        /// </summary>
        public double LastTAUOfEndOfInterval
        {
            get { return lastTAUOfEndOfInterval; }
            set { lastTAUOfEndOfInterval = value; }
        }

        private MyPoint lastPointOfR;
        /// <summary>
        /// poslednja tacka koja treba da se doda pri iscrtavanju grafika na ploteru plotterChangeOfR
        /// </summary>
        public MyPoint LastPointOfR
        {
            get { return lastPointOfR; }
            set 
            {
                if (value != null)
                {
                    lastPointOfR = value;
                }
            }
        }

        private MyPoint lastPointOfE;
        /// <summary>
        /// poslednja tacka koja treba da se doda pri iscrtavanju grafika na ploteru plotterChangeOfE
        /// </summary>
        public MyPoint LastPointOfE
        {
            get { return lastPointOfE; }
            set
            {
                if (value != null)
                {
                    lastPointOfE = value;
                }
            }
        }

        #endregion

        #region ArrayDatas

        private List<double> arrayChangeValueOfR = new List<double>();
        /// <summary>
        /// sve zabelezene promene napona
        /// </summary>
        public List<double> ArrayChangeValueOfR
        {
            get { return arrayChangeValueOfR; }
            set 
            {
                if (value != null)
                {
                    arrayChangeValueOfR = value;
                }
            }
        }

        private List<double> arrayChangeValueOfE = new List<double>();
        /// <summary>
        /// sve zabelezene promene izduzenja
        /// </summary>
        public List<double> ArrayChangeValueOfE
        {
            get { return arrayChangeValueOfE; }
            set
            {
                if (value != null)
                {
                    arrayChangeValueOfE = value;
                }
            }
        }

        private List<double> arrayROfEndOfInterval = new List<double>();
        /// <summary>
        /// svi zabelezeni  naponi na krajevima intervala
        /// </summary>
        public List<double> ArrayROfEndOfInterval
        {
            get { return arrayROfEndOfInterval; }
            set
            {
                if (value != null)
                {
                    arrayROfEndOfInterval = value;
                }
            }
        }

        private List<double> arrayElongationOfEndOfInterval = new List<double>();
        /// <summary>
        /// sva zabelezena  izduzenja na krajevima intervala
        /// </summary>
        public List<double> ArrayElongationOfEndOfInterval
        {
            get { return arrayElongationOfEndOfInterval; }
            set
            {
                if (value != null)
                {
                    arrayElongationOfEndOfInterval = value;
                }
            }
        }


        private List<double> arrayElongationOfEndOfInterval_Fitting = new List<double>();
        /// <summary>
        /// sva zabelezena  izduzenja na krajevima intervala kod fitovanog grafika ya promenu napona i izduzenja
        /// </summary>
        public List<double> ArrayElongationOfEndOfInterval_Fitting
        {
            get { return arrayElongationOfEndOfInterval_Fitting; }
            set
            {
                if (value != null)
                {
                    arrayElongationOfEndOfInterval_Fitting = value;
                }
            }
        }


        private List<double> arrayTaus = new List<double>();
        public List<double> ArrayTaus
        {
            get { return arrayTaus; }
            set
            {
                if (value != null)
                {
                    arrayTaus = value;
                }
            }
        }


        #endregion


        #region E2E4Interval

        private List<double> arrayE2Interval = new List<double>();
        /// <summary>
        /// sve zabelezene promene napona
        /// </summary>
        public List<double> ArrayE2Interval
        {
            get { return arrayE2Interval; }
            set
            {
                if (value != null)
                {
                    arrayE2Interval = value;
                }
            }
        }

        private List<double> arrayE4Interval = new List<double>();
        /// <summary>
        /// sve zabelezene promene napona
        /// </summary>
        public List<double> ArrayE4Interval
        {
            get { return arrayE4Interval; }
            set
            {
                if (value != null)
                {
                    arrayE4Interval = value;
                }
            }
        }

        #endregion

        private MyPointCollection pointsChangeOfR = new MyPointCollection();
        public MyPointCollection PointsChangeOfR
        {
            get { return pointsChangeOfR; }
            set 
            {
                if (value != null)
                {
                    pointsChangeOfR = value;
                }
            }
        }
        private MyPointCollection pointsChangeOfE = new MyPointCollection();
        public MyPointCollection PointsChangeOfE
        {
            get { return pointsChangeOfE; }
            set
            {
                if (value != null)
                {
                    pointsChangeOfE = value;
                }
            }
        }


        private MyPointCollection pointsChangeOfRFitting = new MyPointCollection();
        public MyPointCollection PointsChangeOfRFitting
        {
            get { return pointsChangeOfRFitting; }
            set
            {
                if (value != null)
                {
                    pointsChangeOfRFitting = value;
                }
            }
        }

        private MyPointCollection pointsChangeOfEFitting = new MyPointCollection();
        public MyPointCollection PointsChangeOfEFitting
        {
            get { return pointsChangeOfEFitting; }
            set
            {
                if (value != null)
                {
                    pointsChangeOfEFitting = value;
                }
            }
        }
        //public void DodajChangeOfR(MyPoint p) 
        //{
        //    pointsChangeOfR.Add(new MyPoint(0,0));
        //    pointsChangeOfR.Add(new MyPoint(1, 1));
        //    pointsChangeOfR.Add(new MyPoint(3, 3));
        //    pointsChangeOfR.Add(p);

            
        //}

        public void CreateChangeOfRGraphic()
        {
            try
            {
                createChangeOfRGraphic();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void CreateChangeOfRGraphic()}", System.DateTime.Now);
            }
        }

        private void createChangeOfRGraphic() 
        {
            try
            {
                deleteChangeOfRGraphic();

                int maxPoints = 300000;
                pointsChangeOfR = new MyPointCollection(maxPoints);
                pointsChangeOfR.Clear();


                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfR);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfR.AddLineGraph(ds, Colors.Green, 2, "Promena R-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfR.LegendVisible = false;

                if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR == true)
                {
                    plotterChangeOfR.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.yRangeChangeR);
                    restr.XRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.xRangeChangeR);

                    plotterChangeOfR.Viewport.Restrictions.Add(restr);
                }

                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR == true)
                {
                    plotterChangeOfR.FitToView();
                    plotterChangeOfR.Viewport.Restrictions.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void createChangeOfRGraphic()}", System.DateTime.Now);
            }

        }


        public void createChangeOfEGraphicFitting()
        {
            try
            {
                deleteChangeOfEGraphic();

                int maxPoints = 300000;
                pointsChangeOfE = new MyPointCollection(maxPoints);
                pointsChangeOfE.Clear();

                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfEFitting);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfE.AddLineGraph(ds, Colors.Blue, 2, "Promena e-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfE.LegendVisible = false;

                //if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation == true)
                //{
                plotterChangeOfE.Viewport.AutoFitToView = true;
                ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                restr.YRange = new DisplayRange(0.0, /*OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation*/pointsChangeOfEFitting.Last().YAxisValue + 0.1 * pointsChangeOfEFitting.Last().YAxisValue);
                restr.XRange = new DisplayRange(0.0, /*OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation*/pointsChangeOfEFitting.Last().XAxisValue + 0.1 * pointsChangeOfEFitting.Last().XAxisValue);

                plotterChangeOfE.Viewport.Restrictions.Add(restr);
                //}

                //if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation == true)
                //{
                //plotterChangeOfE.FitToView();
                //plotterChangeOfE.Viewport.Restrictions.Clear();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void createChangeOfEGraphicFitting()}", System.DateTime.Now);
            }
        }

        public void CreateChangeOfEGraphic()
        {
            try
            {
                createChangeOfEGraphic();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void CreateChangeOfEGraphic()}", System.DateTime.Now);
            }
        }

        private void createChangeOfEGraphic()
        {
            try
            {
                deleteChangeOfEGraphic();

                int maxPoints = 300000;
                pointsChangeOfE = new MyPointCollection(maxPoints);
                pointsChangeOfE.Clear();

                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfE);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfE.AddLineGraph(ds, Colors.Green, 2, "Promena e-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfE.LegendVisible = false;

                if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation == true)
                {
                    plotterChangeOfE.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation);
                    restr.XRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation);

                    plotterChangeOfE.Viewport.Restrictions.Add(restr);
                }

                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation == true)
                {
                    plotterChangeOfE.FitToView();
                    plotterChangeOfE.Viewport.Restrictions.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void createChangeOfEGraphic()}", System.DateTime.Now);
            }
        }

        public void createChangeOfRGraphicFitting() 
        {
            try
            {

                deleteChangeOfRGraphic_Fitting();

                int maxPoints = 300000;
                pointsChangeOfRFitting = new MyPointCollection(maxPoints);
                pointsChangeOfRFitting.Clear();


                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfRFitting);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfR.AddLineGraph(ds, Colors.Blue, 2, "Promena fitovanog R-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfR.LegendVisible = false;

                //if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR == true)
                //{
                plotterChangeOfR.Viewport.AutoFitToView = true;
                ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                restr.YRange = new DisplayRange(0.0, /*OptionsInOnlineModeChangeOfRAndE.yRangeChangeR*/pointsChangeOfRFitting.Last().YAxisValue + 0.1 * pointsChangeOfRFitting.Last().YAxisValue);
                restr.XRange = new DisplayRange(0.0, /*OptionsInOnlineModeChangeOfRAndE.xRangeChangeR*/pointsChangeOfRFitting.Last().XAxisValue + 0.1 * pointsChangeOfRFitting.Last().XAxisValue);

                plotterChangeOfR.Viewport.Restrictions.Add(restr);
                //}

                //if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR == true)
                //{
                //    plotterChangeOfR.FitToView();
                //    plotterChangeOfR.Viewport.Restrictions.Clear();
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void createChangeOfRGraphicFitting()}", System.DateTime.Now);
            }
        }

        public void CreateChangeOfRGraphic_Fitting()
        {
            try
            {
                createChangeOfRGraphic_Fitting();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void createChangeOfRGraphicFitting()}", System.DateTime.Now);
            }
        }


        private void createChangeOfRGraphic_Fitting()
        {
            try
            {
                deleteChangeOfRGraphic_Fitting();

                int maxPoints = 300000;
                pointsChangeOfRFitting = new MyPointCollection(maxPoints);
                pointsChangeOfRFitting.Clear();


                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfRFitting);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfR.AddLineGraph(ds, Colors.Blue, 2, "Promena fitovanog R-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfR.LegendVisible = false;

                if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR == true)
                {
                    plotterChangeOfR.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.yRangeChangeR);
                    restr.XRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.xRangeChangeR);

                    plotterChangeOfR.Viewport.Restrictions.Add(restr);
                }

                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR == true)
                {
                    plotterChangeOfR.FitToView();
                    plotterChangeOfR.Viewport.Restrictions.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void createChangeOfRGraphic_Fitting()}", System.DateTime.Now);
            }
        }

        public void CreateChangeOfRGraphic_Fitting(MyPointCollection fittingChangeOfR)
        {
            try
            {
                createChangeOfRGraphic_Fitting(fittingChangeOfR);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void CreateChangeOfRGraphic_Fitting(MyPointCollection fittingChangeOfR)}", System.DateTime.Now);
            }
        }


        private void createChangeOfRGraphic_Fitting(MyPointCollection fittingChangeOfR)
        {
            try
            {
                deleteChangeOfRGraphic_Fitting();

                int maxPoints = 300000;
                pointsChangeOfRFitting = new MyPointCollection(maxPoints);
                pointsChangeOfRFitting.Clear();
                pointsChangeOfRFitting = fittingChangeOfR;

                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfRFitting);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfR.AddLineGraph(ds, Colors.Blue, 2, "Promena fitovanog R-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfR.LegendVisible = false;


                if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeR == true)
                {
                    plotterChangeOfR.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.yRangeChangeR);
                    restr.XRange = new DisplayRange(-0.5, OptionsInOnlineModeChangeOfRAndE.xRangeChangeR);

                    plotterChangeOfR.Viewport.Restrictions.Add(restr);
                }

                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeR == true)
                {
                    plotterChangeOfR.FitToView();
                    plotterChangeOfR.Viewport.Restrictions.Clear();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void createChangeOfRGraphic_Fitting(MyPointCollection fittingChangeOfR)}", System.DateTime.Now);
            }
        }

        public void CreateChangeOfEGraphic_Fitting()
        {
            try
            {
                createChangeOfEGraphic_Fitting();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void CreateChangeOfEGraphic_Fitting()}", System.DateTime.Now);
            }
        }

        private void createChangeOfEGraphic_Fitting()
        {
            try
            {
                deleteChangeOfEEraphic_Fitting();

                int maxPoints = 300000;
                pointsChangeOfEFitting = new MyPointCollection(maxPoints);
                pointsChangeOfEFitting.Clear();


                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfEFitting);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfE.AddLineGraph(ds, Colors.Blue, 2, "Promena fitovanog e-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfE.LegendVisible = false;

                if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation == true)
                {
                    plotterChangeOfE.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(0.0, OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation);
                    restr.XRange = new DisplayRange(0.0, OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation);

                    plotterChangeOfE.Viewport.Restrictions.Add(restr);
                }

                if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation == true)
                {
                    plotterChangeOfE.FitToView();
                    plotterChangeOfE.Viewport.Restrictions.Clear();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void createChangeOfEGraphic_Fitting()}", System.DateTime.Now);
            }
        }

        public void CreateChangeOfEGraphic_Fitting(MyPointCollection fittingChangeOfE, bool setBorders = false)
        {
            try
            {
                createChangeOfEGraphic_Fitting(fittingChangeOfE, setBorders);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void CreateChangeOfEGraphic_Fitting(MyPointCollection fittingChangeOfE, bool setBorders = false)}", System.DateTime.Now);
            }
        }

        private void createChangeOfEGraphic_Fitting(MyPointCollection fittingChangeOfE, bool setBorders = false)
        {
            try
            {
                deleteChangeOfEEraphic_Fitting();

                int maxPoints = 300000;
                pointsChangeOfEFitting = new MyPointCollection(maxPoints);
                pointsChangeOfEFitting.Clear();
                pointsChangeOfEFitting = fittingChangeOfE;


                var ds = new EnumerableDataSource<MyPoint>(pointsChangeOfEFitting);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);


                plotterChangeOfE.AddLineGraph(ds, Colors.Blue, 2, "Promena fitovanog e-a"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
                plotterChangeOfE.LegendVisible = false;

                if (setBorders == false)
                {
                    if (OptionsInOnlineModeChangeOfRAndE.isManualCheckedChangeElongation == true)
                    {
                        plotterChangeOfE.Viewport.AutoFitToView = true;
                        ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                        restr.YRange = new DisplayRange(0.0, OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation);
                        restr.XRange = new DisplayRange(0.0, OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation);

                        plotterChangeOfE.Viewport.Restrictions.Add(restr);
                    }

                    if (OptionsInOnlineModeChangeOfRAndE.isAutoCheckedChangeElongation == true)
                    {
                        plotterChangeOfE.FitToView();
                        plotterChangeOfE.Viewport.Restrictions.Clear();
                    }
                }
                else
                {
                    plotterChangeOfE.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    if (pointsChangeOfEFitting.Count > 0)
                    {
                        restr.YRange = new DisplayRange(0.0, /*OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation*/ pointsChangeOfEFitting.Last().YAxisValue + 0.1 * pointsChangeOfEFitting.Last().YAxisValue);
                        restr.XRange = new DisplayRange(0.0, /*OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation*/ pointsChangeOfEFitting.Last().XAxisValue + 0.1 * pointsChangeOfEFitting.Last().XAxisValue);
                    }
                    else
                    {
                        restr.YRange = new DisplayRange(0.0, OptionsInOnlineModeChangeOfRAndE.yRangeChangeElongation);
                        restr.XRange = new DisplayRange(0.0, OptionsInOnlineModeChangeOfRAndE.xRangeChangeElongation);
                    }


                    plotterChangeOfE.Viewport.Restrictions.Add(restr);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void createChangeOfEGraphic_Fitting(MyPointCollection fittingChangeOfE, bool setBorders = false)}", System.DateTime.Now);
            }
        }


        private void loadOptionsOnlineManagingOfTTM()
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
                        }

                        if (textReader.Name.Equals("Rmin"))
                        {
                            OptionsInOnlineManagingOfTTM.Rmin = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }


                        if (textReader.Name.Equals("eR2"))
                        {
                            OptionsInOnlineManagingOfTTM.eR2 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("eR4"))
                        {
                            OptionsInOnlineManagingOfTTM.eR4 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }


                    } //end of  if (nType == XmlNodeType.Element)
                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void loadOptionsOnlineManagingOfTTM()}", System.DateTime.Now);
            }
        }

        //private double xconst = 1250 + 355;
        //private double yconst = 160;

        //private double xconst = 1250 + 245 - 130 + 280;
        //private double yconst = 120;

        private double xconst = 1250 + 245 - 130 + 280;
        private double yconst = 120;

        public VelocityOfChangeParametersXY(OnlineMode onlineMode, string title = "Brzina promene parametara")
        {
            try
            {
                InitializeComponent();
                this.onMode = onlineMode;
                createChangeOfRGraphic();
                createChangeOfEGraphic();
                createChangeOfRGraphic_Fitting();
                createChangeOfEGraphic_Fitting();

                this.onMode.IsChangeParametersGraphicsOpen = true;
                this.Title = title;

                loadOptionsOnlineManagingOfTTM();
                lineRmax = new HorizontalLine(OptionsInOnlineManagingOfTTM.Rmax);
                lineRmax.Stroke = Brushes.Red;
                lineRmax.StrokeThickness = 2;
                plotterChangeOfR.Children.Add(lineRmax);
                lineRmin = new HorizontalLine(OptionsInOnlineManagingOfTTM.Rmin);
                lineRmin.Stroke = Brushes.Red;
                lineRmin.StrokeThickness = 2;
                plotterChangeOfR.Children.Add(lineRmin);
                double Lc = 0;
                bool isN = double.TryParse(LastInputOutputSavedData.tfLc, out Lc);

                lineE2min = new HorizontalLine(OptionsInOnlineManagingOfTTM.eR2 * (Lc) * 0.8);
                lineE2min.Stroke = Brushes.Red;
                lineE2min.StrokeThickness = 2;
                plotterChangeOfE.Children.Add(lineE2min);
                lineE2max = new HorizontalLine(OptionsInOnlineManagingOfTTM.eR2 * (Lc) * 1.2);
                lineE2max.Stroke = Brushes.Red;
                lineE2max.StrokeThickness = 2;
                plotterChangeOfE.Children.Add(lineE2max);
                lineE4min = new HorizontalLine(OptionsInOnlineManagingOfTTM.eR4 * (Lc) * 0.8);
                lineE4min.Stroke = Brushes.Red;
                lineE4min.StrokeThickness = 2;
                plotterChangeOfE.Children.Add(lineE4min);
                lineE4max = new HorizontalLine(OptionsInOnlineManagingOfTTM.eR4 * (Lc) * 1.2);
                lineE4max.Stroke = Brushes.Red;
                lineE4max.StrokeThickness = 2;
                plotterChangeOfE.Children.Add(lineE4max);
                plotterChangeOfR.Legend.Visibility = Visibility.Hidden;
                plotterChangeOfE.Legend.Visibility = Visibility.Hidden;

                //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

                this.WindowStartupLocation = WindowStartupLocation.Manual;
                this.Left = xconst;
                this.Top = yconst;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public VelocityOfChangeParametersXY(OnlineMode onlineMode, string title = )}", System.DateTime.Now);
            }
        }

        public void FittingChangeOfRGraphic(int index) 
        {
            try
            {
                for (int i = 0; i < pointsChangeOfR.Count; i++)
                {
                    if (i > index)
                    {
                        pointsChangeOfRFitting.Add(pointsChangeOfR[i]);
                    }
                }
                deleteChangeOfRGraphic();
                plotterChangeOfR.LegendVisible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void FittingChangeOfRGraphic(int index)}", System.DateTime.Now);
            }
            
        }

        public void FittingChangeOfRGraphicFromBeetwen(int indexbeg, int indexend)
        {
            try
            {
                for (int i = 0; i < pointsChangeOfR.Count; i++)
                {
                    if (i > indexbeg && i < indexend - 1)
                    {
                        pointsChangeOfRFitting.Add(pointsChangeOfR[i]);
                    }
                }
                deleteChangeOfRGraphic();
                plotterChangeOfR.LegendVisible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void FittingChangeOfRGraphicFromBeetwen(int indexbeg, int indexend)}", System.DateTime.Now);
            }

        }

        public void FittingChangeOfEGraphicFromEnd(int index)
        {
            try
            {
                for (int i = 0; i < pointsChangeOfE.Count; i++)
                {
                    if (i < index - 1)
                    {
                        pointsChangeOfEFitting.Add(pointsChangeOfE[i]);
                    }
                }
                deleteChangeOfEGraphic();
                plotterChangeOfE.LegendVisible = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void FittingChangeOfEGraphicFromEnd(int index)}", System.DateTime.Now);
            }

        }


        public void DeleteChangeOfRGraphic()
        {
            try
            {
                deleteChangeOfRGraphic();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void DeleteChangeOfRGraphic()}", System.DateTime.Now);
            }
        }

        private void deleteChangeOfRGraphic()
        {
            try
            {
                var numberOfOnline = plotterChangeOfR.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena R-a").Count();
                if (numberOfOnline > 0)
                {
                    for (int i = 0; i < numberOfOnline; i++)
                    {
                        var lineToRemove = plotterChangeOfR.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena R-a").ElementAt(i);

                        plotterChangeOfR.Children.Remove(lineToRemove);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void deleteChangeOfRGraphic()}", System.DateTime.Now);
            }
        }

        public void FittingChangeOfEGraphic(int index)
        {
            try
            {
                for (int i = 0; i < pointsChangeOfE.Count; i++)
                {
                    if (i > index)
                    {
                        pointsChangeOfEFitting.Add(pointsChangeOfE[i]);
                    }
                }
                deleteChangeOfEGraphic();
                plotterChangeOfE.LegendVisible = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void FittingChangeOfEGraphic(int index)}", System.DateTime.Now);
            }

        }

        public void FittingArrayElongationOfEndOfInterval(int index, double E2E4Border, double XTranslateAmountFittingMode)
        {
            try
            {
                arrayElongationOfEndOfInterval_Fitting.Clear();
                for (int i = 0; i < arrayElongationOfEndOfInterval.Count; i++)
                {
                    if (i > index)
                    {
                        arrayElongationOfEndOfInterval_Fitting.Add(arrayElongationOfEndOfInterval[i]);
                    }
                }


                //razvrstaj tacke intervala E2 i E4
                arrayE2Interval.Clear();
                arrayE4Interval.Clear();
                List<double> arrayE4IntervalWithZeros = new List<double>();
                for (int i = 0; i < arrayElongationOfEndOfInterval_Fitting.Count; i++)
                {
                    //if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode < E2E4Border)
                    //{
                    //    arrayE2Interval.Add(pointsChangeOfEFitting[i].YAxisValue);
                    //}
                    //else
                    //{
                    //    arrayE4IntervalWithZeros.Add(pointsChangeOfEFitting[i].YAxisValue);
                    //}

                    if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode < 0.95 * E2E4Border)
                    {
                        arrayE2Interval.Add(pointsChangeOfEFitting[i].YAxisValue);
                    }
                    if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode > 1.05 * E2E4Border)
                    {
                        arrayE4IntervalWithZeros.Add(pointsChangeOfEFitting[i].YAxisValue);
                    }

                }


                //kad se zavrsi sa kidanje tzv Promena jednaka je nuli
                //te nule treba izbaciti iz E4Interval dela jel se one ne racunaju
                for (int i = 0; i < arrayE4IntervalWithZeros.Count; i++)
                {
                    if (arrayE4IntervalWithZeros[i] > 0)
                    {
                        arrayE4Interval.Add(arrayE4IntervalWithZeros[i]);
                    }
                }

                //kad zavrsis sa razvrstavanjem ocisti ovu kolekciju u kojoj se cuva kraj elongacije na intervalu gde se belezi Promena
                arrayElongationOfEndOfInterval.Clear();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void FittingArrayElongationOfEndOfInterval(int index, double E2E4Border, double XTranslateAmountFittingMode)}", System.DateTime.Now);
            }

        }

        public void DeleteChangeOfEGraphic()
        {
            try
            {
                deleteChangeOfEGraphic();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void DeleteChangeOfEGraphic()}", System.DateTime.Now);
            }
        }

        private void deleteChangeOfEGraphic() 
        {
            try
            {
                var numberOfOnline = plotterChangeOfE.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena e-a").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotterChangeOfE.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena e-a").Single();

                    plotterChangeOfE.Children.Remove(lineToRemove);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void deleteChangeOfEGraphic()}", System.DateTime.Now);
            }
        }





        public void DeleteChangeOfRGraphic_Fitting()
        {
            try
            {
                deleteChangeOfRGraphic_Fitting();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void DeleteChangeOfRGraphic_Fitting()}", System.DateTime.Now);
            }
        }

        private void deleteChangeOfRGraphic_Fitting()
        {
            try
            {
                var numberOfOnline = plotterChangeOfR.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena fitovanog R-a").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotterChangeOfR.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena fitovanog R-a").Single();

                    plotterChangeOfR.Children.Remove(lineToRemove);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void deleteChangeOfRGraphic_Fitting()}", System.DateTime.Now);
            }
        }

        public void DeleteChangeOfEGraphic_Fitting()
        {
            try
            {
                deleteChangeOfEEraphic_Fitting();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void DeleteChangeOfEGraphic_Fitting()}", System.DateTime.Now);
            }
        }

        private void deleteChangeOfEEraphic_Fitting()
        {
            try
            {
                var numberOfOnline = plotterChangeOfE.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena fitovanog e-a").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotterChangeOfE.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Promena fitovanog e-a").Single();

                    plotterChangeOfE.Children.Remove(lineToRemove);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void deleteChangeOfEEraphic_Fitting()}", System.DateTime.Now);
            }
        }


        #region setHorizontalLines

        //public void SetHorizontalLineE2(double valueHorizontalLine)
        //{
        //    setHorizontalLineE2(valueHorizontalLine);
        //}

        //private void setHorizontalLineE2(double valueHorizontalLine)
        //{
        //    lineE2.Value = valueHorizontalLine;
        //    plotterChangeOfE.Legend.Visibility = Visibility.Hidden;

        //}

        //public void SetHorizontalLineE4(double valueHorizontalLine)
        //{
        //    setHorizontalLineE4(valueHorizontalLine);
        //}

        //private void setHorizontalLineE4(double valueHorizontalLine)
        //{

        //    lineE4.Value = valueHorizontalLine;
        //    plotterChangeOfE.Legend.Visibility = Visibility.Hidden;

        //}

        public void SetHorizontalLineRmax(double valueHorizontalLine)
        {
            try
            {
                setHorizontalLineRmax(valueHorizontalLine);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {public void SetHorizontalLineRmax(double valueHorizontalLine)}", System.DateTime.Now);
            }
        }

        private void setHorizontalLineRmax(double valueHorizontalLine)
        {
            try
            {
                lineRmax.Value = valueHorizontalLine;
                plotterChangeOfR.Legend.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void setHorizontalLineRmax(double valueHorizontalLine)}", System.DateTime.Now);
            }

        }

        #endregion


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //this.onMode.IsChangeParametersGraphicsOpen = false;
                //AmIOpen = false;
                e.Cancel = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[VelocityOfChangeParametersXY.xaml.cs] {private void Window_Loaded(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }
    }
}
