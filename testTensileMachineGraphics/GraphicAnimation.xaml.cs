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
using testTensileMachineGraphics.Options;
using testTensileMachineGraphics.PointViewModel;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System.Windows.Threading;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System.IO;
using Microsoft.Research.DynamicDataDisplay.Charts;
using testTensileMachineGraphics.Windows;

namespace testTensileMachineGraphics
{
    /// <summary>
    /// Interaction logic for GraphicAnimation.xaml
    /// </summary>
    public partial class GraphicAnimation : UserControl
    {


        private VelocityOfChangeParametersXY vXYAnimation;
        public VelocityOfChangeParametersXY VXYAnimation
        {
            get { return vXYAnimation; }
            set
            {
                if (value != null)
                {
                    vXYAnimation = value;
                }
            }
        }

        private int counter = 0;
        private int counter2 = 0;
        private int cntChangeOfPreassure = 0;
        private int cntChangeOfElongation = 0;
        
        private List<string> updateTimerTimesAnim = new List<string>();
        private DateTime begAnim;
        private DateTime endAnim;



        private double changeOfPreassure = 0;
        //private double changeOfPreassureForFirstDerivation = 0;
        private double changeOfElongation = 0;
        //private double changeOfElongationForFirstDerivation = 0;
        

        double changeOfElongationForFirstDerivation = 0;

        private DateTime beg;
        private DateTime end;
        private DateTime now;

        private double durationInmilisec = 0;//vreme trajanja kidanja u milisekundama
        private int prevExecutionInMs = 0;//u kojoj milisekundi je bilo predhodno izvrsavanje
        private int currExecutionInMs = 0;//u kojoj milisekundi je bilo sadasnje izvrsavanje
        private List<string> changesOfPreassure = new List<string>();
        private List<string> changesOfElongation = new List<string>();
        private List<string> animationLines = new List<string>();
        public int milisecInSec = 1000;

        private double maxChangeOfPreasure = 0.0;
        private double maxChangeOfElongation = 0.0;
        private double maxForceInKN = 0.0;

        public int counterWhoDetermitedOneSecond;
        private int currCounterWhoDetermitedOneSecondAnim = 0;
        
        //public int counterWhoDetermitedOneSecond;
        private int currCounterWhoDetermitedOneSecond = 0;

        private const string TURNON = "Pokreni animaciju";
        private const string TURNOFF = "Zaustavi animaciju";
        private const string STATUS_TURNON = "Animacija je pokrenuta";
        private const string STATUS_TURNOFF = "Animacija je zaustavljena";

        private double maxChangeOfPreasureAnim = 0.0;
        private double maxChangeOfElongationAnim = 0.0;

        private bool isAnimateMode = false; 

        //private DispatcherTimer updateCollectionTimer;

        //public DispatcherTimer UpdateCollectionTimer
        //{
        //    get { return updateCollectionTimer; }
        //}


        private DataReader dataReader;
        public MyPointCollection points;
        //private List<double> preassures = new List<double>();
        //private List<double> elongations = new List<double>();
        private List<double> changeOfPreassures = new List<double>();
        private List<double> changeOFElongations = new List<double>();
        private List<double> preassuresEndInterval = new List<double>();
        private List<double> elongationsEndInterval = new List<double>();
        private double maxPreassure = 0;
        private double maxForceInkN = 0;
        private double maxElongation = 0;
        private double currentPreassure = 0;
        private double currentElongation = 0;
        private double currentElongationInMM = 0;
        private double currentDurationInms = 0;
        private double currentForceInKN = 0;
        private double currentMaximumForceInkN = 0;
        private double currentChangeOfPreassure = 0;
        private double currentMAXChangeOfPreassure = 0;
        private double currentMAXChangeOfElongation = 0;
        private double currentChangeOfElongation = 0;



        private int animateLineCnt = 0;
        

        public void createAnimationGraphics()
        {
            try
            {
                var numberOfOnline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Animation Mode").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Animation Mode").Single();


                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        plotter.Children.Remove(lineToRemove);
                    }));
                }


                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    _MarkerGraph.DataSource = null;
                }));

                numberOfOnline = plotter.Children.OfType<MarkerPointsGraph>().Count();
                if (numberOfOnline > 0)
                {
                    var markersToRemove = plotter.Children.OfType<MarkerPointsGraph>().Single();


                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        plotter.Children.Remove(markersToRemove);
                    }));
                }

                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    int maxPoints = 300000;
                    points = new MyPointCollection(maxPoints);
                    points.Clear();


                    var ds = new EnumerableDataSource<MyPoint>(points);
                    ds.SetXMapping(x => x.XAxisValue);
                    ds.SetYMapping(y => y.YAxisValue);


                    plotter.AddLineGraph(ds, Colors.Green, 2, "Animation Mode"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"



                    if (OptionsInAnimation.isAutoChecked)
                    {
                        plotter.FitToView();
                        plotter.Viewport.Restrictions.Clear();
                    }
                    else
                    {
                        plotter.Viewport.AutoFitToView = true;
                        ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                        restr.YRange = new DisplayRange(-0.5, OptionsInAnimation.ratioForPreassure);
                        restr.XRange = new DisplayRange(-0.5, OptionsInAnimation.ratioForElongation);

                        plotter.Viewport.Restrictions.Add(restr);
                    }

                }));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {public void createAnimationGraphics()}", System.DateTime.Now);
            }

        }

        // Declare MicroTimer
        private readonly MicroTimer _microTimer;


        public GraphicAnimation()
        {
            try
            {
                InitializeComponent();
                //this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.80);
                //this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.80);
                //setStatusLabels();

                // Instantiate new MicroTimer and add event handler
                _microTimer = new MicroTimer();
                _microTimer.MicroTimerElapsed +=
                    new MicroTimer.MicroTimerElapsedEventHandler(updateCollectionTimer_Tick);
                //updateCollectionTimer = new DispatcherTimer();
                //updateCollectionTimer.Interval = TimeSpan.FromMilliseconds(OptionsInAnimation.refreshTimeInterval);
                //updateCollectionTimer.Tick += new EventHandler(updateCollectionTimer_Tick);

                //btnShowAnimationParameters.Visibility = Visibility.Hidden;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {public GraphicAnimation()}", System.DateTime.Now);
            }
            
        }

      

        private void updateCollectionTimer_Tick(object sender,  MicroTimerEventArgs timerEventArgs)
        {
            try
            {
                currentDurationInms = currentDurationInms + 20;

                if ((int)currentDurationInms / 1000 >= changeOfPreassures.Count)
                {
                    currentDurationInms = 0;
                    currentPreassure = 0;
                    currentElongation = 0;
                    currentElongationInMM = 0;
                    currentForceInKN = 0;
                    currentMaximumForceInkN = 0;
                    cntChangeOfPreassure = 0;
                    cntChangeOfElongation = 0;
                    currentMAXChangeOfPreassure = 0;
                    currentMAXChangeOfElongation = 0;


                    counter = 0;
                    counter2 = 0;




                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {

                        vXYAnimation.PointsChangeOfE.Clear();
                        vXYAnimation.PointsChangeOfR.Clear();
                        vXYAnimation.DeleteChangeOfEGraphic();
                        vXYAnimation.DeleteChangeOfRGraphic();
                        vXYAnimation.CreateChangeOfEGraphic();
                        vXYAnimation.CreateChangeOfRGraphic();
                        MessageBox.Show("Animiran je ceo uzorak.");
                        SetRatio();

                        btnAnimationMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                        //clear process parameters
                        tfForceInkN.Text = Constants.ZERO;
                        tfMaxForceInKN.Text = Constants.ZERO;
                        tfPreassureInMPa.Text = Constants.ZERO;
                        tfElongationInMM.Text = Constants.ZERO;
                        tfElongationInProcent.Text = Constants.ZERO;
                        tfDeltaPreassure.Text = Constants.ZERO;
                        tfMaxDeltaPreassure.Text = Constants.ZERO;
                        tfDeltaElongation.Text = Constants.ZERO;
                        tfMaxDeltaElongation.Text = Constants.ZERO;

                    }));

                    return;
                }

                counter++;
                counter2++;

                double currentPreassureRound = 0;
                double currentElongationRound = 0;
                double currentElongationInMMRound = 0;
                double currentForceInKNMRound = 0;

                currentPreassure = currentPreassure + (changeOfPreassures[(int)currentDurationInms / 1000] / 49);
                currentElongation = currentElongation + 100 * (changeOFElongations[(int)currentDurationInms / 1000] / 49);
                currentElongationInMM = currentElongation * OptionsInAnimation.l0 / 100.0;
                currentForceInKN = currentPreassure * OptionsInAnimation.s0 / 1000;
                if (currentForceInKN > currentMaximumForceInkN)
                {
                    currentMaximumForceInkN = currentForceInKN;
                    if (currentMaximumForceInkN < 10)
                    {
                        currentMaximumForceInkN = Math.Round(currentMaximumForceInkN, 3);
                    }
                    else if (currentMaximumForceInkN < 100)
                    {
                        currentMaximumForceInkN = Math.Round(currentMaximumForceInkN, 2);
                    }
                    else
                    {
                        currentMaximumForceInkN = Math.Round(currentMaximumForceInkN, 1);
                    }
                }

                if (currentPreassure < 10)
                {
                    currentPreassureRound = Math.Round(currentPreassure, 3);
                }
                else if (currentPreassure < 100)
                {
                    currentPreassureRound = Math.Round(currentPreassure, 2);
                }
                else
                {
                    currentPreassureRound = Math.Round(currentPreassure, 1);
                }


                if (currentElongation < 10)
                {
                    currentElongationRound = Math.Round(currentElongation, 3);
                }
                else if (currentElongation < 100)
                {
                    currentElongationRound = Math.Round(currentElongation, 2);
                }
                else
                {
                    currentElongationRound = Math.Round(currentElongation, 1);
                }

                if (currentElongationRound > maxElongation)
                {
                    currentElongationRound = maxElongation;
                }


                if (currentElongationInMM < 10)
                {
                    currentElongationInMMRound = Math.Round(currentElongationInMM, 3);
                }
                else if (currentElongationInMM < 100)
                {
                    currentElongationInMMRound = Math.Round(currentElongationInMM, 2);
                }
                else
                {
                    currentElongationInMMRound = Math.Round(currentElongationInMM, 1);
                }

                if (currentForceInKN < 10)
                {
                    currentForceInKNMRound = Math.Round(currentForceInKN, 3);
                }
                else if (currentForceInKN < 100)
                {
                    currentForceInKNMRound = Math.Round(currentForceInKN, 2);
                }
                else
                {
                    currentForceInKNMRound = Math.Round(currentForceInKN, 1);
                }

                if (currentPreassureRound > maxPreassure)
                {
                    currentPreassureRound = maxPreassure;
                }
                if (currentForceInKNMRound > maxForceInkN)
                {
                    currentForceInKNMRound = maxForceInkN;
                }


                //BEGIN change of preassure
                if (counter % 5 == 0)
                {
                    currentChangeOfPreassure = currentChangeOfPreassure + changeOfPreassures[cntChangeOfPreassure] / 10;

                    if (currentChangeOfPreassure < 10)
                    {
                        currentChangeOfPreassure = Math.Round(currentChangeOfPreassure, 3);
                    }
                    else if (currentChangeOfPreassure < 100)
                    {
                        currentChangeOfPreassure = Math.Round(currentChangeOfPreassure, 2);
                    }
                    else
                    {
                        currentChangeOfPreassure = Math.Round(currentChangeOfPreassure, 1);
                    }


                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        tfDeltaPreassure.Text = currentChangeOfPreassure.ToString();
                    }));
                }
                if (counter == 50)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        if (currentChangeOfPreassure > currentMAXChangeOfPreassure)
                        {
                            currentMAXChangeOfPreassure = currentChangeOfPreassure;
                            tfMaxDeltaPreassure.Text = currentMAXChangeOfPreassure.ToString();
                        }
                    }));




                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        if (chbStartSampleShowChangedParAnimation.IsChecked == true)
                        {
                            MyPoint pointChangeOfR = new MyPoint(changeOfPreassures[cntChangeOfPreassure], cntChangeOfPreassure + 1);
                            vXYAnimation.PointsChangeOfR.Add(pointChangeOfR);
                        }
                    }));



                    //ovo se resetuje tek to celom animmiranom uzorku
                    cntChangeOfPreassure++;



                    currentChangeOfPreassure = 0;
                    counter = 0;
                }
                //END change of preassure




                //BEGIN change of elongation
                if (counter2 % 5 == 0)
                {
                    currentChangeOfElongation = currentChangeOfElongation + changeOFElongations[cntChangeOfElongation] / 10;


                    currentChangeOfElongation = Math.Round(currentChangeOfElongation, 5);
                    if (currentChangeOfElongation < 0.00001)
                    {
                        currentChangeOfElongation = 0;
                    }

                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        tfDeltaElongation.Text = currentChangeOfElongation.ToString();
                    }));
                }
                if (counter2 == 50)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        if (currentChangeOfElongation > currentMAXChangeOfElongation)
                        {
                            currentMAXChangeOfElongation = currentChangeOfElongation;
                            tfMaxDeltaElongation.Text = currentMAXChangeOfElongation.ToString();
                        }
                    }));



                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        if (chbStartSampleShowChangedParAnimation.IsChecked == true)
                        {
                            MyPoint pointChangeOfE = new MyPoint(changeOFElongations[cntChangeOfPreassure], cntChangeOfPreassure + 1);
                            vXYAnimation.PointsChangeOfE.Add(pointChangeOfE);
                        }
                    }));



                    //ovo se resetuje tek to celom animmiranom uzorku
                    cntChangeOfElongation++;



                    currentChangeOfElongation = 0;
                    counter2 = 0;
                }
                //END change of elongation


                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    points.Add(new MyPoint(currentPreassureRound, currentElongationRound));
                    tfPreassureInMPa.Text = currentPreassureRound.ToString();
                    tfElongationInProcent.Text = currentElongationRound.ToString();
                    tfElongationInMM.Text = currentElongationInMMRound.ToString();
                    tfForceInkN.Text = currentForceInKNMRound.ToString();
                    if (currentMaximumForceInkN > maxForceInkN)
                    {
                        currentMaximumForceInkN = maxForceInkN;
                    }
                    tfMaxForceInKN.Text = currentMaximumForceInkN.ToString();
                }));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void updateCollectionTimer_Tick(object sender,  MicroTimerEventArgs timerEventArgs)}", System.DateTime.Now);
            }
        }

        //private void setSavedAnimationOptions(OptionsAnimation opAnimation) 
        //{
        //    if (OptionsInAnimation.isContinuousDisplay)
        //    {
        //        opAnimation.rbtnContinuous.IsChecked = true;
        //        opAnimation.rbtnContinuous.Foreground = Brushes.Black;
        //    }
        //    else
        //    {
        //        opAnimation.rbtnContinuous.IsChecked = false;
        //        opAnimation.rbtnContinuous.Foreground = Brushes.Black;
        //    }

        //    if (OptionsInAnimation.isDiscreteDisplay)
        //    {
        //        opAnimation.rbtnDiscrete.IsChecked = true;
        //        opAnimation.rbtnDiscrete.Foreground = Brushes.Black;
        //    }
        //    else
        //    {
        //        opAnimation.rbtnDiscrete.IsChecked = false;
        //        opAnimation.rbtnDiscrete.Foreground = Brushes.Black;
        //    }

        //    //opAnimation.tfRefresh.Text = OptionsInAnimation.refreshTimeInterval.ToString();
        //    if (OptionsInAnimation.refreshTimeInterval == 10)
        //    {
        //        opAnimation.cmbRefresh.SelectedIndex = 0;
        //        counterWhoDetermitedOneSecond = 100;
        //    }
        //    if (OptionsInAnimation.refreshTimeInterval == 16.6)
        //    {
        //        opAnimation.cmbRefresh.SelectedIndex = 1;
        //        counterWhoDetermitedOneSecond = 60;

        //    }
        //    if (OptionsInAnimation.refreshTimeInterval == 40)
        //    {
        //        opAnimation.cmbRefresh.SelectedIndex = 2;
        //        counterWhoDetermitedOneSecond = 25;
        //    }

        //    opAnimation.cmbRefresh.Foreground = Brushes.Black;
        //    opAnimation.tfResolutionCon.Text = OptionsInAnimation.conResolution.ToString();
        //    opAnimation.tfResolutionCon.Foreground = Brushes.Black;
        //    opAnimation.tfResolutionDis.Text = OptionsInAnimation.disResolution.ToString();
        //    opAnimation.tfResolutionDis.Foreground = Brushes.Black;


        //    opAnimation.tfCalForceDivide.Text = OptionsInAnimation.nutnDivide.ToString();
        //    opAnimation.tfCalForceDivide.Foreground = Brushes.Black;
        //    opAnimation.tfCalForceMultiple.Text = OptionsInAnimation.nutnMultiple.ToString();
        //    opAnimation.tfCalForceMultiple.Foreground = Brushes.Black;
        //    opAnimation.tfCalElonDivide.Text = OptionsInAnimation.mmDivide.ToString();
        //    opAnimation.tfCalElonDivide.Foreground = Brushes.Black;
        //    opAnimation.tfCalElonMultiple.Text = OptionsInAnimation.mmCoeff.ToString();
        //    opAnimation.tfCalElonMultiple.Foreground = Brushes.Black;

        //    opAnimation.tfFilepathPlotting.Text = OptionsInAnimation.filePath;
        //    opAnimation.tfFilepathPlotting.Foreground = Brushes.Black;
        //}

        //private void btnShowAnimationOptions_Click(object sender, RoutedEventArgs e)
        //{
        //    if (OptionsAnimation.isCreatedOptionsAnimation)
        //    {
        //        MessageBox.Show("Otvoren je prozor opcija animacije grafika!");
        //        return;
        //    }

        //    OptionsAnimation opAnimation = new OptionsAnimation();
        //    OptionsAnimation.isCreatedOptionsAnimation = true;
        //    opAnimation.Animation = this;


        //    //set saved value for particular plotting mode
        //    setSavedAnimationOptions(opAnimation);


        //    opAnimation.Show();


        //    OptionsAnimation.isCreatedOptionsAnimation = true;
        //}

        private void btnAnimationMode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tfFilepathAnimation.Text.Equals(String.Empty) == true)
                {
                    MessageBox.Show("Niste uneli animacioni fajl!");
                    return;
                }

                if (tfFilepathAnimation.Text.Contains("e2e4") == false)
                {
                    MessageBox.Show("Ekstenzija animacionog fajla mora biti e2e4!");
                    return;
                }

                if (chbStartSampleShowChangedParAnimation.IsChecked == true)
                {
                    if (vXYAnimation.Height > 0)
                    {
                        vXYAnimation.Show();
                    }
                }

                if (isAnimateMode == false)
                {
                    isAnimateMode = true;
                    //btnShowAnimationOptions.IsReadOnly = false;
                    btnAnimationMode.Content = TURNOFF;
                    lblAnimationStatus.Content = STATUS_TURNON;
                    lblAnimationStatus.Foreground = Brushes.Red;
                    lblAnimationStatus.FontWeight = FontWeights.ExtraBlack;

                    long interval = 20000;//20000 [microsec] = 20 [ms]
                    //long interval = (long)OptionsInAnimation.refreshTimeInterval * 1000 * OptionsInAnimation.Resolution;//this change this in microsec
                    //// Set timer interval
                    _microTimer.Interval = interval;

                    //// Ignore event if late by half the interval
                    _microTimer.IgnoreEventIfLateBy = interval / 2;

                    //_microTimer.Interval = (long)OptionsInAnimation.refreshTimeInterval;
                    _microTimer.Start();

                }
                else
                {
                    isAnimateMode = false;
                    //btnShowAnimationOptions.IsReadOnly = true;
                    _microTimer.Stop();
                    _microTimer.Abort();

                    btnAnimationMode.Content = TURNON;
                    lblAnimationStatus.Content = STATUS_TURNOFF;
                    lblAnimationStatus.Foreground = Brushes.Black;
                    lblAnimationStatus.FontWeight = FontWeights.Normal;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void btnAnimationMode_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void calculateApproximateMaxPreassure()
        {

        }

        /// <summary>
        /// ucitavanje promena brzina napona i elongacije na osnovu kojih ce se posle animirati fajl
        /// </summary>
        /// <param name="filePath"></param>
        private void loadAnimationDatas(string filePath) 
        {
            try
            {
                List<string> datas = File.ReadAllLines(filePath).ToList();



                foreach (var data in datas)
                {
                    List<string> dataList = new List<string>();
                    double changeR = 0;
                    double changeE = 0;
                    double R = 0;
                    double e = 0;
                    dataList = data.Split('*').ToList();
                    bool isN = double.TryParse(dataList[0], out changeR);
                    changeOfPreassures.Add(changeR);
                    isN = double.TryParse(dataList[2], out changeE);
                    //moramo da podelimo sa Lo da bismo dobili promenu relativnog izduzenja[koje je u procentima] u 1 sekundi
                    changeE = changeE / OptionsInAnimation.l0;
                    changeOFElongations.Add(changeE);
                    isN = double.TryParse(dataList[4], out e);
                    elongationsEndInterval.Add(e);
                    isN = double.TryParse(dataList[5], out R);
                    preassuresEndInterval.Add(R);
                    //maxPreassure = preassuresEndInterval.Max();
                    //maxForceInkN = maxPreassure * OptionsInAnimation.s0 / 1000;
                    //if (maxForceInkN < 10)
                    //{
                    //    maxForceInkN = Math.Round(maxForceInkN, 3);
                    //}
                    //else if (maxForceInkN < 100)
                    //{
                    //    maxForceInkN = Math.Round(maxForceInkN, 2);
                    //}
                    //else
                    //{
                    //    maxForceInkN = Math.Round(maxForceInkN, 1);
                    //}
                    maxElongation = elongationsEndInterval.Max();
                }

                //preassures = new List<double>();
                //elongations = new List<double>();

                //bool isStarFound = false;

                //while (isStarFound == false)
                //{
                //    if (datas.ElementAt(0).Contains('*') == false)
                //    {
                //        datas.RemoveAt(0);
                //    }
                //    else
                //    {
                //        isStarFound = true;
                //    }
                //}

                //for (int i = 0; i < datas.Count; i++)
                //{
                //    List<string> currData = datas[i].Split('*').ToList();

                //    double currPreassure;
                //    string strPreassure = currData[0].Replace(',', '.');
                //    bool isN = Double.TryParse(strPreassure, out currPreassure);
                //    if (isN == true)
                //    {
                //        preassures.Add(currPreassure);
                //    }

                //    double currElongation;
                //    string strElongation = currData[1].Replace(',', '.');
                //    bool isNN = Double.TryParse(strElongation, out currElongation);
                //    if (isNN == true)
                //    {
                //        elongations.Add(currElongation);
                //    }
                //}
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void loadAnimationDatas(string filePath)}", System.DateTime.Now);
            }
        }

  

        private void loadAnimationSettings(string filePath) 
        {
            try
            {
                List<string> lines = File.ReadAllLines(filePath).ToList();
                int cnt = 0;
                bool isFound = false;
                while (isFound == false)
                {
                    string stringForCheck = lines[cnt];
                    //if (Constants.L0.Contains(stringForCheck) == true)
                    if (stringForCheck.Contains(Constants.L0) == true)
                    {
                        isFound = true;
                        double l0;
                        string strL0 = lines[cnt].Split(':')[1];
                        strL0 = strL0.Replace(',', '.');
                        bool isN = Double.TryParse(strL0, out l0);
                        if (isN == true)
                        {
                            OptionsInAnimation.l0 = l0;
                        }
                    }
                    cnt++;
                }

                if (isFound == false)
                {
                    MessageBox.Show("Animacija. Nemogućnost čitanja početne dužine L0.");
                }


                cnt = 0;
                isFound = false;
                while (isFound == false)
                {
                    string stringForCheck = lines[cnt];
                    //if (Constants.L0.Contains(stringForCheck) == true)
                    if (stringForCheck.Contains(Constants.S0) == true)
                    {
                        isFound = true;
                        double s0;
                        string strS0 = lines[cnt].Split(':')[1];
                        strS0 = strS0.Replace(',', '.');
                        bool isN = Double.TryParse(strS0, out s0);
                        if (isN == true)
                        {
                            OptionsInAnimation.s0 = s0;
                        }
                    }
                    cnt++;
                }

                if (isFound == false)
                {
                    MessageBox.Show("Animacija. Nemogućnost čitanja početne dužine S0.");
                }

                cnt = 0;
                isFound = false;
                while (isFound == false)
                {
                    string stringForCheck = lines[cnt];
                    //if (Constants.L0.Contains(stringForCheck) == true)
                    if (stringForCheck.Contains(Constants.Rm) == true)
                    {
                        isFound = true;
                        double rm;
                        string strRm = lines[cnt].Split('\t')[1];
                        strRm = strRm.Replace(',', '.');
                        bool isN = Double.TryParse(strRm, out rm);
                        if (isN == true)
                        {
                            maxPreassure = rm;
                            if (maxPreassure < 10)
                            {
                                maxPreassure = Math.Round(maxPreassure, 3);
                            }
                            else if (maxPreassure < 100)
                            {
                                maxPreassure = Math.Round(maxPreassure, 2);
                            }
                            else
                            {
                                maxPreassure = Math.Round(maxPreassure, 1);
                            }
                        }
                    }
                    cnt++;
                }

                if (isFound == false)
                {
                    MessageBox.Show("Animacija. Nemogućnost čitanja Rm-a.");
                }


                cnt = 0;
                isFound = false;
                while (isFound == false)
                {
                    string stringForCheck = lines[cnt];
                    //if (Constants.L0.Contains(stringForCheck) == true)
                    if (stringForCheck.Contains(Constants.Fm) == true)
                    {
                        isFound = true;
                        double fm;
                        string strFm = lines[cnt].Split('\t')[1];
                        strFm = strFm.Replace(',', '.');
                        bool isN = Double.TryParse(strFm, out fm);
                        if (isN == true)
                        {
                            maxForceInkN = fm/1000;
                            if (maxForceInkN < 10)
                            {
                                maxForceInkN = Math.Round(maxForceInkN, 3);
                            }
                            else if (maxForceInkN < 100)
                            {
                                maxForceInkN = Math.Round(maxForceInkN, 2);
                            }
                            else
                            {
                                maxForceInkN = Math.Round(maxForceInkN, 1);
                            }
                        }
                    }
                    cnt++;
                }

                if (isFound == false)
                {
                    MessageBox.Show("Animacija. Nemogućnost čitanja Fm-a.");
                }

                    //tfAnimationRatioRemark.Text = String.Empty;
                    //tfAnimationRatioRemark.Foreground = Brushes.Black;
                    //bool isAutoCheckedRatios = false;
                    //OptionsInAnimation.isAutoChecked = false;

                    //List<string> lines = File.ReadAllLines(filePath).ToList();
                    //int cnt = 0;
                    //bool isFound = false;
                    //while (isFound == false && cnt < 50)
                    //{
                    //    string stringForCheck = lines[cnt].Split(':')[0];
                    //    if (Constants.ANIMATIONFILEHEADER_L0.Contains(stringForCheck) == true)
                    //    {
                    //        isFound = true;
                    //        double l0;
                    //        string strL0 = lines[cnt].Split(':')[1];
                    //        strL0 = strL0.Replace(',', '.');
                    //        bool isN = Double.TryParse(strL0, out l0);
                    //        if (isN == true)
                    //        {
                    //            OptionsInAnimation.l0 = l0;
                    //            tfL0.Text = l0.ToString();
                    //        }
                    //    }
                    //    cnt++;
                    //}
                    //if (cnt > 50)
                    //{
                    //    MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //}



                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_S0.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double s0;
                    //                string strS0 = lines[cnt].Split(':')[1];
                    //                strS0 = strS0.Replace(',', '.');
                    //                bool isN = Double.TryParse(strS0, out s0);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.s0 = s0;
                    //                    tfS0.Text = s0.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }



                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_nutnDivide.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double nutnDivide;
                    //                string strnutnDivide = lines[cnt].Split(':')[1];
                    //                strnutnDivide = strnutnDivide.Replace(',', '.');
                    //                bool isN = Double.TryParse(strnutnDivide, out nutnDivide);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.nutnDivide = nutnDivide;
                    //                    tfCalForceDivide.Text = nutnDivide.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }



                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_nutnMultiple.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double nutnMultiple;
                    //                string strnutnMultiple = lines[cnt].Split(':')[1];
                    //                strnutnMultiple = strnutnMultiple.Replace(',', '.');
                    //                bool isN = Double.TryParse(strnutnMultiple, out nutnMultiple);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.nutnMultiple = nutnMultiple;
                    //                    tfCalForceMultiple.Text = nutnMultiple.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }




                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_mmDivide.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double mmDivide;
                    //                string strmmDivide = lines[cnt].Split(':')[1];
                    //                strmmDivide = strmmDivide.Replace(',', '.');
                    //                bool isN = Double.TryParse(strmmDivide, out mmDivide);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.mmDivide = mmDivide;
                    //                    tfCalElonDivide.Text = mmDivide.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }


                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_mmCoeff.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double mmCoeff;
                    //                string strmmCoeff = lines[cnt].Split(':')[1];
                    //                strmmCoeff = strmmCoeff.Replace(',', '.');
                    //                bool isN = Double.TryParse(strmmCoeff, out mmCoeff);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.mmCoeff = mmCoeff;
                    //                    tfCalElonMultiple.Text = mmCoeff.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }

                    //        /*  refresh time */

                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            //string refreshTime = "Refresh time in [ms]   ";
                    //            if (Constants.ANIMATIONFILEHEADER_refreshAnimationTime.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double refreshTime2;
                    //                string strrefreshTime = lines[cnt].Split(':')[1];
                    //                strrefreshTime = strrefreshTime.Replace(',', '.');
                    //                bool isN = Double.TryParse(strrefreshTime, out refreshTime2);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.refreshTimeInterval = refreshTime2;
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }

                    //        /*  refresh time */

                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            string isAutoCheckedStr = "isAutoChecked ";
                    //            if (isAutoCheckedStr.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;

                    //                string isAutoCheckedResult = lines[cnt].Split(':')[1];
                    //                if (isAutoCheckedResult.Contains("True"))
                    //                {
                    //                    tfAnimationRatioRemark.Text = Constants.ANIMATIONRATIOREMARKS;
                    //                    tfAnimationRatioRemark.Foreground = Brushes.Red;
                    //                    OptionsInAnimation.isAutoChecked = true;
                    //                    isAutoCheckedRatios = true;
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }


                    //        /* get max change of preassure */

                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double maxChangeOfPreassure = Double.MinValue;
                    //                double elongationForMaxChangeOfPreassure = Double.MinValue;
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

                    //        /* get max change of preassure */

                    //        /* get resolution of plotting */

                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_resolution.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                int resolution = Int32.MinValue;
                    //                string strResolution = lines[cnt].Split(':')[1];

                    //                strResolution = strResolution.Replace(',', '.');
                    //                bool isN = Int32.TryParse(strResolution, out resolution);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.Resolution = resolution;
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Ucitan animacijski fajl nije ispravnog formata!");
                    //        }

                    //        /* get resolution of plotting */


                    //        if (isAutoCheckedRatios == true)
                    //        {
                    //            tfRatioForce.Text = "0.95";
                    //            tfRatioElongation.Text = "0.95";
                    //            createAnimationGraphics();
                    //            return;
                    //        }


                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_razmeraPreassure.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double ratioForPreassure;
                    //                string strratioForPreassure = lines[cnt].Split(':')[1];
                    //                strratioForPreassure = strratioForPreassure.Replace(',', '.');
                    //                bool isN = Double.TryParse(strratioForPreassure, out ratioForPreassure);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.ratioForPreassure = ratioForPreassure;
                    //                    tfRatioForce.Text = ratioForPreassure.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }


                    //        cnt = 0;
                    //        isFound = false;
                    //        while (isFound == false && cnt < 50)
                    //        {
                    //            string stringForCheck = lines[cnt].Split(':')[0];
                    //            if (Constants.ANIMATIONFILEHEADER_razmeraElongation.Contains(stringForCheck) == true)
                    //            {
                    //                isFound = true;
                    //                double ratioForElongation;
                    //                string strratioForElongation = lines[cnt].Split(':')[1];
                    //                strratioForElongation = strratioForElongation.Replace(',', '.');
                    //                bool isN = Double.TryParse(strratioForElongation, out ratioForElongation);
                    //                if (isN == true)
                    //                {
                    //                    OptionsInAnimation.ratioForElongation = ratioForElongation;
                    //                    tfRatioElongation.Text = ratioForElongation.ToString();
                    //                }
                    //            }
                    //            cnt++;
                    //        }
                    //        if (cnt > 50)
                    //        {
                    //            MessageBox.Show("Učitan animacijski fajl nije ispravnog formata!");
                    //        }



                    //        createAnimationGraphics();
                    //        return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Neispravan format animacijskog fajla[citanje s0,l0,Rm ili Fm-a] !!!", "ANIMACIJA NEISPRAVAN FORMAT");
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void loadAnimationSettings(string filePath)}", System.DateTime.Now);
            }

        }

        private void btnChooseDatabasePath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                vXYAnimation = new VelocityOfChangeParametersXY(window.OnlineMode, "Brzina promene parametara - Animacija");
                changeOfPreassures = new List<double>();
                changeOFElongations = new List<double>();
                preassuresEndInterval = new List<double>();
                elongationsEndInterval = new List<double>();
                maxPreassure = 0;
                maxForceInkN = 0;
                maxElongation = 0;

                //if (animateLineCnt == preassures.Count - 1)
                //{
                if (animateLineCnt > 0)
                {
                    _microTimer.Stop();
                    animateLineCnt = 0;
                    MessageBox.Show("      Prekinuto je animiranje prethodnog uzorka !         ", "ANIMACIJA UZORKA ZAVRŠENA");
                    createAnimationGraphics();
                    //dataReader.ClearData();
                    //currCounterWhoDetermitedOneSecondAnim = 0;
                    //File.WriteAllLines(System.Environment.CurrentDirectory + "\\timerWritingAnim.txt", updateTimerTimesAnim);
                    durationInmilisec = 0;
                    changeOfPreassure = 0;
                    changeOfElongation = 0;
                    currCounterWhoDetermitedOneSecond = 0;
                    maxChangeOfPreasure = 0.0;
                    maxChangeOfElongation = 0.0;
                    maxForceInKN = 0.0;

                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        btnAnimationMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                        //clear process parameters
                        tfForceInkN.Text = Constants.ZERO;
                        tfMaxForceInKN.Text = Constants.ZERO;
                        //tfPreassureInMPa.Text = Constants.ZERO;
                        tfElongationInMM.Text = Constants.ZERO;
                        tfElongationInProcent.Text = Constants.ZERO;
                        tfDeltaPreassure.Text = Constants.ZERO;
                        tfMaxDeltaPreassure.Text = Constants.ZERO;
                        //tfElongationMaxPreassure.Text = Constants.ZERO;
                        tfDeltaElongation.Text = Constants.ZERO;
                        tfMaxDeltaElongation.Text = Constants.ZERO;
                        //tfPreassureMaxElongation.Text = Constants.ZERO;

                    }));

                }

                _microTimer.Abort();
                //    return;

                //}




                string filePath = String.Empty;
                string extensionTxt = "e2e4";
                bool _okDatabasePath = false;
                System.Windows.Forms.OpenFileDialog openDlg = new System.Windows.Forms.OpenFileDialog();
                openDlg.Filter = "| *.e2e4";

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
                        System.Windows.Forms.MessageBox.Show("Izabrani fajl " + filePath + " nije fajl sa anim ekstenzijom! Molimo vas učitajte fajl sa ispravnom ekstenzijom!", "POKUŠAJ UČITAVANJA NEISPRAVNOG FORMATA ANIMACIONOG FAJLA");
                        return;
                    }
                }

                tfFilepathAnimation.Text = filePath;
                string nameInputOutput = filePath.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";
                if (File.Exists(nameInputOutput) == false)
                {
                    MessageBox.Show("Ne postoji fajl sa putanjom : " + nameInputOutput);
                    return;
                }
                loadAnimationSettings(nameInputOutput);
                loadAnimationDatas(filePath);
                SetRatio();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void btnChooseDatabasePath_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

      


        private void SetRatio() 
        {
            try
            {
                OptionsInAnimation.isAutoChecked = false;
                OptionsInAnimation.ratioForPreassure = 1.1 * maxPreassure;
                OptionsInAnimation.ratioForElongation = 1.1 * maxElongation;
                createAnimationGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void SetRatio()}", System.DateTime.Now);
            }
        }

        private void chbStartSampleShowChangedParAnimation_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                if (window.Plotting.chbShowChangeOfRAndEe.IsChecked == true)
                {
                    window.Plotting.chbShowChangeOfRAndEe.IsChecked = false;
                }
                if (window.PrintScreen.chbChangeOfRAndE.IsChecked == true)
                {
                    window.PrintScreen.chbChangeOfRAndE.IsChecked = false;
                }

                if (vXYAnimation != null/* && vXYAnimation.AmIOpen == true*/)
                {
                    vXYAnimation.Visibility = Visibility.Visible;
                    return;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void chbStartSampleShowChangedParAnimation_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbStartSampleShowChangedParAnimation_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (vXYAnimation != null /*&& vXYAnimation.AmIOpen == true*/)
                {
                    vXYAnimation.Visibility = Visibility.Hidden;
                    return;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[GraphicAnimation.xaml.cs] {private void chbStartSampleShowChangedParAnimation_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

    }
}
