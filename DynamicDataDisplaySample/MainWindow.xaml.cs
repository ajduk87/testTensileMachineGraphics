using System.Windows;
using DynamicDataDisplaySample.VoltageViewModel;
using System.Windows.Threading;
using System;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Windows.Media;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using System.IO;

using System.Linq;
using System.Windows.Forms;
using DynamicDataDisplaySample.VoltageViewModel;


namespace DynamicDataDisplaySample
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        private bool isAnimateLine = false;
        private bool isAnimateMarkers = false;
        private bool isOnlineMarkers = false;
        private bool isOnlineLine = false;
        private int animateLineCnt = 0;
        private int animateMarkersCnt = 0;
        private DataReader dataReader;

        private int stopTimerCounter = 100;

        private List<double> xAxisMarkers = new List<double>();
        private List<double> yAxisMarkers = new List<double>();


        private List<double> xAxisMarkersSum = new List<double>();
        private List<double> yAxisMarkersSum = new List<double>();

        private List<double> xData;
        private List<double> yData;

        List<double> xm = new List<double>();
        List<double> ym = new List<double>();

        private int oldNumber = 0;
        

        private int _maxVoltage;
        public int MaxVoltage
        {
            get { return _maxVoltage; }
            set { _maxVoltage = value; this.OnPropertyChanged("MaxVoltage"); }
        }

        private int _minVoltage;
        public int MinVoltage
        {
            get { return _minVoltage; }
            set { _minVoltage = value; this.OnPropertyChanged("MinVoltage"); }
        }

        public VoltagePointCollection voltagePointCollection;
        public VoltagePointCollection points;
        DispatcherTimer updateCollectionTimer;
        private int counter = 0;

        public MainWindow()
        {
            InitializeComponent();
            //DataReader.ReadData(System.Environment.CurrentDirectory + "\\files\\___001.txt");
            //ReadData();
            string fp = System.Environment.CurrentDirectory + "\\files\\___001.txt";
            string fpOnline = "D:\\___temprorary\\online.txt";
            dataReader = new DataReader(fp,fpOnline);
            xData = new List<double>();
            yData = new List<double>();

            this.DataContext = this;

            voltagePointCollection = new VoltagePointCollection();
            points = new VoltagePointCollection();   

            updateCollectionTimer = new DispatcherTimer();
            updateCollectionTimer.Interval = TimeSpan.FromMilliseconds(10);
            updateCollectionTimer.Tick += new EventHandler(updateCollectionTimer_Tick);
            updateCollectionTimer.Start();
            //isOnlineLine = true;

            //var ds = new EnumerableDataSource<VoltagePoint>(voltagePointCollection);
            if (isOnlineLine)
            {
                var ds = new EnumerableDataSource<VoltagePoint>(points);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.Voltage);
                plotter.AddLineGraph(ds, Colors.Green, 4, "Volts"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"
            }


            //isOnlineMarkers = true;
            if (isOnlineMarkers)
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(dataReader.RelativeElongation);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(dataReader.PreassureInMPa);
                _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;
            }


            /*  EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xm);
             EnumerableDataSource<double> gY = new EnumerableDataSource<double>(ym);
             _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

             //no scaling - identity mapping
             gX.XMapping = xx => xx;
             gY.YMapping = yy => yy;

            CirclePointMarker mkr = new CirclePointMarker();
             mkr.Fill = new SolidColorBrush(Colors.Red);
             mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1.0);
             _MarkerGraph.Marker = mkr;*/

           // MaxVoltage = 1;
           // MinVoltage = -1;
            
        }

        void updateCollectionTimer_Tick(object sender, EventArgs e)
        {


            if (isOnlineLine)
            {
                int oldNumber = dataReader.RelativeElongation.Count;
                int previousCountSample = dataReader.RelativeElongation.Count;
                int newCounterValue = dataReader.ReadDataOnLine();


                if (dataReader.RelativeElongation.Count > 0)
                {


                    for (int i = dataReader.RelativeElongation.Count - 1; i < dataReader.RelativeElongation.Count; )
                    {
                        points.Add(new VoltagePoint(dataReader.PreassureInMPa[i], dataReader.RelativeElongation[i]));
                        i = i + 1;
                    }
                }

            }
           
            //points.Add(new VoltagePoint(ym[ym.Count - 1], xm[xm.Count - 1]));
            if (isOnlineMarkers)
            {
                int oldNumber = dataReader.RelativeElongation.Count;
                int previousCountSample = dataReader.RelativeElongation.Count;
                int newCounterValue = dataReader.ReadDataOnLine();




                //List<string> provera = new List<string>();
                //provera.Add("ulazak " + DateTime.Now + " oldNumber je " + oldNumber);
                //File.AppendAllLines(@"D:\provera.txt",provera);
                if (dataReader.RelativeElongation.Count > 0)
                {


                    for (int i = dataReader.RelativeElongation.Count - 1; i < dataReader.RelativeElongation.Count; )
                    {
                        points.Add(new VoltagePoint(dataReader.PreassureInMPa[i], dataReader.RelativeElongation[i]));
                        i = i + 1;
                    }

                    CirclePointMarker mkr = new CirclePointMarker();
                    mkr.Fill = new SolidColorBrush(Colors.Red);
                    mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                    _MarkerGraph.Marker = mkr;
                }
            }

            if (isAnimateLine) 
            {
                animateLineCnt++;
                points.Add(new VoltagePoint(dataReader.PreassureInMPa[animateLineCnt], dataReader.RelativeElongation[animateLineCnt]));
                if (animateLineCnt == dataReader.PreassureInMPa.Count - 1)
                {
                    updateCollectionTimer.Stop();
                    isAnimateLine = false;
                }
            }

            if (isAnimateMarkers)
            {

                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xAxisMarkers);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yAxisMarkers);
                _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _MarkerGraph.Marker = mkr;

                animateMarkersCnt++;
                xAxisMarkers.Add(xAxisMarkersSum[animateMarkersCnt]);
                yAxisMarkers.Add(yAxisMarkersSum[animateMarkersCnt]);
                if (animateMarkersCnt == xAxisMarkersSum.Count - 1)
                {
                    updateCollectionTimer.Stop();
                    isAnimateMarkers = false;
                }
            }

           // points.Add(new VoltagePoint(ym[ym.Count-1], xm[xm.Count - 1]));
           // i++;

        }

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void btnGetLine_Click(object sender, RoutedEventArgs e)
        {
         /*   for (int ii = 0; ii < xm.Count; ii++)
            {
                points.Add(new VoltagePoint(ym[ii],xm[ii]));
            }
            */
            dataReader.ReadData();
            //int progressBarValue = 0;
            List<double> xAxis = new List<double>();
            List<double> yAxis = new List<double>();
            //int step = DataReader.relativeElongation.Count / 100;
            //int step = dataReader.RelativeElongation.Count / (int)pbar.Maximum;
            //int makerStep = 50;

            //create points
            for (int i = 0; i < dataReader.RelativeElongation.Count; )
            {

                if (dataReader.RelativeElongation.Count > pbar.Maximum)
                {
                   /* if (i % step == 0)
                    {
                        updateProgressBar(progressBarValue++);
                    }*/
                }
              /*  if (i % (makerStep) == 0)
                {
                    xAxis.Add(DataReader.relativeElongation[i]);
                    yAxis.Add(DataReader.preassureInMPa[i]);
                }*/

                points.Add(new VoltagePoint(dataReader.PreassureInMPa[i], dataReader.RelativeElongation[i]));
                i = i + 1;
            }

            



          /*  EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xAxis);
            EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yAxis);
            _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

            //no scaling - identity mapping
            gX.XMapping = xx => xx;
            gY.YMapping = yy => yy;

            CirclePointMarker mkr = new CirclePointMarker();
            mkr.Fill = new SolidColorBrush(Colors.Red);
            mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 10);
            _MarkerGraph.Marker = mkr;*/



            updateProgressBar(100);
            var ds = new EnumerableDataSource<VoltagePoint>(points);
            ds.SetXMapping(x => x.XAxisValue);
            ds.SetYMapping(y => y.Voltage);
            plotter.AddLineGraph(ds, Colors.Green, 4, "Volts");
            plotter.FitToView();

        }

        private void btnGetMarkers_Click(object sender, RoutedEventArgs e)
        {
            dataReader.ReadData();
            int progressBarValue = 0;
            List<double> xAxis = new List<double>();
            List<double> yAxis = new List<double>();
            //int step = DataReader.relativeElongation.Count / 100;
            int step = dataReader.RelativeElongation.Count / (int)pbar.Maximum;
            int makerStep = 50;

            //create markers
            for (int i = 0; i < dataReader.RelativeElongation.Count; )
            {

                if (dataReader.RelativeElongation.Count > pbar.Maximum)
                {
                    if (i % step == 0)
                    {
                        updateProgressBar(progressBarValue++);
                    }
                }
                  if (i % (makerStep) == 0)
                  {
                      xAxis.Add(dataReader.RelativeElongation[i]);
                      yAxis.Add(dataReader.PreassureInMPa[i]);
                  }

                //points.Add(new VoltagePoint(DataReader.preassureInMPa[i], DataReader.relativeElongation[i]));
                i = i + 1;
            }





              EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xAxis);
              EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yAxis);
              _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

              //no scaling - identity mapping
              gX.XMapping = xx => xx;
              gY.YMapping = yy => yy;

              CirclePointMarker mkr = new CirclePointMarker();
              mkr.Fill = new SolidColorBrush(Colors.Red);
              mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
              _MarkerGraph.Marker = mkr;



             updateProgressBar(100);
             plotter.FitToView();
        }

        private void updateProgressBar(double value)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
            {
                this.pbar.Value = value; // Do all the ui thread updates here
            }));
        }

        private void btnStopTimer_Click(object sender, RoutedEventArgs e)
        {
            updateCollectionTimer.Stop();
            System.Windows.Forms.MessageBox.Show("Timer je zaustavljen!");
        }

        private void btnAnimateLine_Click(object sender, RoutedEventArgs e)
        {
            dataReader.ReadData();

            var ds = new EnumerableDataSource<VoltagePoint>(points);
            ds.SetXMapping(x => x.XAxisValue);
            ds.SetYMapping(y => y.Voltage);
            plotter.AddLineGraph(ds, Colors.Green, 4, "Volts"); 

            isAnimateLine = true;
            updateCollectionTimer.Start();
        }

        private void btnAnimateMarkers_Click(object sender, RoutedEventArgs e)
        {
            dataReader.ReadData();
       
            /*EnumerableDataSource<double> gX = new EnumerableDataSource<double>(xAxisMarkers);
            EnumerableDataSource<double> gY = new EnumerableDataSource<double>(yAxisMarkers);
            _MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

            //no scaling - identity mapping
            gX.XMapping = xx => xx;
            gY.YMapping = yy => yy;

            CirclePointMarker mkr = new CirclePointMarker();
            mkr.Fill = new SolidColorBrush(Colors.Red);
            mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
            _MarkerGraph.Marker = mkr;*/

            int makerStep = 50;
            //create markers
            for (int i = 0; i < dataReader.RelativeElongation.Count; )
            {


              
                if (i % (makerStep) == 0)
                {
                    xAxisMarkersSum.Add(dataReader.RelativeElongation[i]);
                    yAxisMarkersSum.Add(dataReader.PreassureInMPa[i]);
                }

                //points.Add(new VoltagePoint(DataReader.preassureInMPa[i], DataReader.relativeElongation[i]));
                i = i + 1;
            }

            isAnimateMarkers = true;
            updateCollectionTimer.Start();
        }

      
    }
}
