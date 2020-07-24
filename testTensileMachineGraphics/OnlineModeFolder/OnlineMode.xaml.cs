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
using System.IO.Ports;
using LabJack.LabJackUD;
using testTensileMachineGraphics.Options;
using testTensileMachineGraphics.MessageBoxes;
using Microsoft.Research.DynamicDataDisplay.Charts;
using System.Xml;
using testTensileMachineGraphics.Reports;
using System.Threading;
using testTensileMachineGraphics.Windows;
using System.Diagnostics;
using System.Xml.Linq;
using testTensileMachineGraphics.Reports.SumReportClasses;
using System.Runtime.InteropServices;



namespace testTensileMachineGraphics.OnlineModeFolder
{
    /// <summary>
    /// Interaction logic for OnlineMode.xaml
    /// </summary>
    public partial class OnlineMode : UserControl
    {



        public bool IsEkstenziometerUsed = false;


        /// <summary>
        /// ako jos uvek nije bilo kliknuto na zeleno dugme 
        /// </summary>
        private bool firstImeClickedAtGreenButton = false;//samo jednom produciraj klik na dugme ucitaj tekuci grafik
        /// <summary>
        /// readonly property
        /// </summary>
        public bool FirstImeClickedAtGreenButton
        {
            get { return firstImeClickedAtGreenButton; }
        }

        /// <summary>
        /// ako je kliknuto na podateke u print modu znaci da zelimo da se podaci unose
        /// a ako je kliknuto u Online modu ne sme sada da se podaci menjaju samo mogu da se gledaju
        /// </summary>
        public bool IsClickedSampleDataAtPrintMode = false;
      
        public SaveDialogForm SaveDialogProperty;

        /// <summary>
        /// kaunter kojima se malo usporava osvezavanje brzine promene napona i izduzenja
        /// tj jedan korak brojaca counter predstavlja priblizno 20ms
        /// pa kada se Promena osvezava na 200ms vrednost brojaca na kome se okida dogadjaj je 10
        /// a za 400ms vrednost brojaca na kome se okida dogadjaj je 20
        /// a za 600ms vrednost brojaca na kome se okida dogadjaj je 30
        /// a za 800ms vrednost brojaca na kome se okida dogadjaj je 40
        /// i za 1000ms vrednost brojaca na kome se okida dogadjaj je 50
        /// mada brojac za jednu sekundu se ne koristi vece se samo apdejtuje u else grani od if(durationInmilisec < 1000)
        /// </summary>
        private int counter = 0;

        private SerialPort serialport = new SerialPort();
       // private SerialPort serialport2 = new SerialPort();
      
        /// <summary>
        /// posle SAMO JEDNOG kidanja funkcija   private void drawFittingGraphic(bool T3movingDirectionByYAxis = true, int numberOfCallForDrawFitting = 0) 
        /// klase GraphicPlotting
        /// poziva se tri puta
        /// kada se klikne na dugme za prestanak kidanja ili do kraja online upisa[nema nikakvog klika na dugme] 
        /// tada dolazi do tri puta zvanja gore pomenute metode
        /// i tada se tek posle treceg poziva treba ponistiti da je kliknuto na dugme za kidanje(crveno dugme sa natpisom //) 
        /// tj da se clan klase OnlineMode _isStoppedOnlineSample postavi na false
        /// 
        /// </summary>
        private int numberOfCallForDrawFitting = 0;

        /// <summary>
        /// da li je upravo zavrsen online mod
        /// bilo da se radi o kliku na dugme za kidanje(crveno dugme sa natpisom //) ili ili do kraja online upisa[nema nikakvog klika na dugme] 
        /// jel ako jeste preskoci postavljenje rucnim markera jel u tom trenutku jos uvek ne postoje
        /// </summary>
        private bool _isStoppedOnlineSample = false;
        /// <summary>
        /// da li je kliknuto na zavrsenje kidanja u online modu
        /// jel ako jeste preskoci postavljenje rucnim markera jel u tom trenutku jos uvek ne postoje
        /// </summary>
        public bool IsStoppedOnlineSample
        {
            get { return _isStoppedOnlineSample; }
            set { _isStoppedOnlineSample = value; }
        }

        private int indexFromChangedParametersFitting = 0;
        public int IndexFromChangedParametersFitting
        {
            get { return indexFromChangedParametersFitting; }
            set { indexFromChangedParametersFitting = value; }
        }


        private int indexOdsecanjaPromeneNaponaZaRsaTackom = 0;
        //e2
        private int indexOdsecanjaPromeneIzduzenjaZaEsaTackom_2 = 0;

        private List<double> maxChangesOfPreassureHistoryYs = new List<double>();
        private List<double> maxChangesOfElongationRange2HistoryYs = new List<double>();

        private bool isOptionsForManagingOfTTM = false;
        public bool IsOptionsForManagingOfTTM
        {
            get { return isOptionsForManagingOfTTM; }
            set { isOptionsForManagingOfTTM = value; }
        }

        private bool isOptionsForChangeGraphic = false;
        public bool IsOptionsForChangeGraphic
        {
            get { return isOptionsForChangeGraphic; }
            set { isOptionsForChangeGraphic = value; }
        }

        private bool isChangeParametersGraphicsOpen = false;
        public bool IsChangeParametersGraphicsOpen
        {
            get { return isChangeParametersGraphicsOpen; }
            set { isChangeParametersGraphicsOpen = value; }
        }


        private double minPossibleValueForMaxElongation = 0.0;
        private OptionsOnlineChangeOfRAndE opChangeOfRAndE;
        private OptionsOnlineManagingOfTTM opManagingOfTTM;

        private int cntTau = 0;
        private VelocityOfChangeParametersXY vXY;
        public VelocityOfChangeParametersXY VXY
        {
            get { return vXY; }
            set 
            {
                if (value != null)
                {
                    vXY = value;
                }
            }
        }
       
       

        //da li se vrsi upis u fajl tj da li masina jos radi bez obzira sto je prekinuto sa radom programa
        private bool isCurrentProgressWrittingInOnlineFile = false;
        /// <summary>
        /// da li se vrsi upis u fajl tj da li masina jos radi bez obzira sto je prekinuto sa radom programa
        /// </summary>
        public bool IsCurrentProgressWrittingInOnlineFile
        {
            get { return isCurrentProgressWrittingInOnlineFile; }
            set { isCurrentProgressWrittingInOnlineFile = value; }
        }
      

        private int headerSizeForCurrentSample = 0;
        private int linesNumberForCurrentOnlineFile = 0;
        private bool isClickedStopSample = false;


        public bool IsCurrentSampleSaved
        {
            get { return isCurrentSampleSaved; }
        }
        private bool isCurrentSampleSaved = false;
      
        //datas who need for calculation refresh time in animation mode
        private int numberOfSamplesInFirstSecond;
        private double firstSecondDurationInmilisec = 0;

        /// <summary>
        /// cuva informaciju o ulaznim podacima [misli se na prozore u koje se upisuju ulazni podaci]
        /// </summary>
        private OnlineFileHeader onHeader;
        /// <summary>
        /// cuva informaciju o ulaznim podacima [misli se na prozore u koje se upisuju ulazni podaci]
        /// </summary>
        public OnlineFileHeader OnHeader
        {
            get { return onHeader; }
        }

        /// <summary>
        /// cuva informaciju o izlaznim podacima [misli se na prozore u koje se upisuju izlazni podaci]
        /// </summary>
        private ResultsInterface resInterface;
        /// <summary>
        /// cuva informaciju o izlaznim podacima [misli se na prozore u koje se upisuju izlazni podaci]
        /// </summary>
        public ResultsInterface ResultsInterface
        {
            get { return resInterface; }
            set 
            {
                if (value != null)
                {
                    resInterface = value;
                }
            }
        }

        /// <summary>
        /// cuva informaciju o opcijama online moda [misli se na prozore u koje se upisuju zeljene opcije online moda]
        /// </summary>
        private OptionsOnline optionsOnline;
        /// <summary>
        /// cuva informaciju o opcijama online moda [misli se na prozore u koje se upisuju zeljene opcije online moda]
        /// </summary>
        public OptionsOnline OptionsOnline
        {
            get { return optionsOnline; }
        }

        double changeOfPreassure = 0;
        double changeOfPreassureForFirstDerivation = 0;
        double changeOfElongation = 0;
        double changeOfElongationForFirstDerivation = 0;

        //private int currSec = 0;
        private double durationInmilisec = 0;//vreme trajanja kidanja u milisekundama
        private int prevExecutionInMs = 0;//u kojoj milisekundi je bilo predhodno izvrsavanje
        private int currExecutionInMs = 0;//u kojoj milisekundi je bilo sadasnje izvrsavanje
        /// <summary>
        /// cuva informacije o brzinama Promena napona
        /// </summary>
        private List<string> changesOfPreassure = new List<string>();
        /// <summary>
        /// cuva informacije o brzinama izduzenja
        /// </summary>
        private List<string> changesOfElongation = new List<string>();
        /// <summary>
        /// cuva informacije o iscrtavanju tacaka [x - osa je izduzenje a y - osa je Promena napona]
        /// koje se posle primenjuju kada se radi animacija ucitanog fajla
        /// </summary>
        //private List<string> animationLines = new List<string>();
        /// <summary>
        /// cuva informacije o promeni brzine izduzenja
        /// [x - osa je vremenska] [y - osa je Promena brzina izduzenja]
        /// </summary>
        private List<string> e2e4Lines = new List<string>();
        public int milisecInSec = 1000;//u jednoj sekundi ima 1000 ms, ovo je moglo ici i kao konstanta

        public int counterWhoDetermitedOneSecond;
        /// <summary>
        /// brojac kojim se utvrdjuje da li je doslo isteka jedne sekunde
        /// </summary>
        private int currCounterWhoDetermitedOneSecond = 0;

       
        /// <summary>
        /// koliko dugo nije bilo upisa u online fajl
        /// kada vrednost ove promenljive postane veca od opcije OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters
        /// program zna da je zavrsen online upis u fajl , ako se pre toga ne klikne na crveno dugme za prekid
        /// </summary>
        private double howManyMilisecondsNoWriting = 0;
        private double howManyMilisecondsNoWriting2 = 0;

        private List<double> xMarkers = new List<double>();
        private List<double> yMarkers = new List<double>();

        private const string TURNON = "Uključi online mode";
        private const string TURNOFF = "Isključi online mode";
        private const string STATUS_TURNON = "Online mod je ukljucen";
        private const string STATUS_TURNOFF = "Online mod je iskljucen";

        private bool isOnlineMode = false;


       // private bool isOnlineMarkers = false;
       // private bool isOnlineLine = false;
        //only testing this must be in options
       // private int totalPoints = 20000;
        /// <summary>
        /// ovde se cuvaju informacije o trenutno upisu u online fajl
        /// </summary>
        public DataReader dataReader;
        //private DataReader dataReaderForElongationLessThanOne;
        /// <summary>
        /// ovde se cuva putanja online fajla
        /// </summary>
        private string fpOnlineGlobal = String.Empty;
        private string fpGlobal = String.Empty;
        /// <summary>
        /// ovde se cuva putanja online fajla
        /// </summary>
        public string FpOnlineGlobal
        {
            get { return fpOnlineGlobal; }
            set
            {
                if (fpOnlineGlobal != null)
                {
                    fpOnlineGlobal = value;
                }
            }
        }
        public string FpGlobal
        {
            get { return fpGlobal; }
            set 
            {
                if (fpGlobal != null)
                {
                    fpGlobal = value;
                }
            }
        }
       
        /// <summary>
        /// ovde se cuvaju informacije o tackama koje se iscrtavaju na grafiku u online modu
        /// </summary>
        public MyPointCollection points;
        private MyPointCollection pointsAll;
        /// <summary>
        /// ovde se cuva brojcana vrednost maksimalne promene napona
        /// </summary>
        private double maxChangeOfPreasure = 0.0;
        /// <summary>
        /// ovde se cuva brojcana vrednost maksimalne promene izduzenja
        /// </summary>
        private double maxChangeOfElongation = 0.0;
        /// <summary>
        /// ovde se cuva brojcana vrednost maksimalne sile
        /// </summary>
        private double maxForceInKN = 0.0;
        private double changeOfElongationForMaxPreassure = 0.0;


        public double MaxPreassure
        {
            get { return maxPreassure; }
            set { maxPreassure = value; }
        }

        public double MaxElongation
        {
            get { return maxElongation; }
            set { maxElongation = value; }
        }

        private double maxPreassure = 0.0;
        private double maxElongation = 0.0;

        private readonly MicroTimer _BlinkingButtonTimer;
        //private DispatcherTimer updateCollectionTimer;
        // Declare MicroTimer
        /// <summary>
        /// tajmer koji je zaduzen za iscrtavanje grafika u online modu
        /// kao i postavljanje tekstualnih polja
        /// i grafika promene napona i izduzenja
        /// prekida se kada se klikne na crveno dugme za prekid rada kidalice
        /// </summary>
        private readonly MicroTimer _microTimer;
        /// <summary>
        /// ovaj tajmer je zaduzen samo za utvrdjivanje da li u toku ili nije tekuci online upis
        /// jel online upis se nastavlja i po kliku korisnika na crveno dugme za prekid
        /// </summary>
        private readonly MicroTimer _microTimerMachineWorking;

        private readonly MicroTimer _LabJackWorking;
        private readonly MicroTimer _LabJackWorkingTest;
        private readonly MicroTimer _serialPortTestTimer;

        private List<string> updateTimerTimes = new List<string>();
        private DateTime beg;
        private DateTime end;
        private DateTime endWriting;

        private bool isOnlineFileHeaderWritten = false;

        public bool IsOnlineFileHeaderWritten 
        {
            get { return isOnlineFileHeaderWritten; }
            set { isOnlineFileHeaderWritten = value; }
        }

        public void removeRemarkForOnlineFileHeaderWritten() 
        {
            try
            {
                tfRemarkOnlineFileHeaderWritten.Text = String.Empty;
                tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Black;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void removeRemarkForOnlineFileHeaderWritten()}", System.DateTime.Now);
            }
        }

        private void deleteOnlineMode()
        {

            try
            {
                var numberOfOffline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Online Mode").Count();
                if (numberOfOffline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Online Mode").Single();
                    plotter.Children.Remove(lineToRemove);

                }
            }
            catch (Exception ex)
            {
                //Logger.WriteNode(ex.Message.ToString() + "[GraphicPlotting.xaml.cs] {private void deleteOnlyFittingLinePrintScreen()}", System.DateTime.Now);
            }
        }
       

        /// <summary>
        /// kreira prazan grafik koji se se popunjavati odgovarajucim vrednostima kada pocne online upis
        /// </summary>
        public void createOnlineGraphics()
        {
            try
            {
                //ukoliko postoji jedan obrisi ga, jel se uskoro da se napravi novi prazan
                //u koji se moci da se cuvaju informacije o sledecem online upisu
                var numberOfOnline = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Online Mode").Count();
                if (numberOfOnline > 0)
                {
                    var lineToRemove = plotter.Children.OfType<LineGraph>().Where(x => x.Description.ToString() == "Online Mode").Single();


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
                    pointsAll = new MyPointCollection(maxPoints);
                    points.Clear();
                    pointsAll.Clear();

                    var ds = new EnumerableDataSource<MyPoint>(points);
                    ds.SetXMapping(x => x.XAxisValue);
                    ds.SetYMapping(y => y.YAxisValue);

                    //dodavanje praznog online grafika
                    plotter.AddLineGraph(ds, Colors.Green, 2, "Online Mode"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"

                    //ukoliko je cekirano rucno postavljanje razmere 
                    //ovde se postavljaju granice grafika koje se kasnije ne mogu vise menjati
                    if (OptionsInOnlineMode.isManualChecked)
                    {

                        plotter.Viewport.AutoFitToView = true;
                        ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                        restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInOnlineMode.yRange);
                        restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInOnlineMode.xRange);

                        plotter.Viewport.Restrictions.Add(restr);

                    }

                    //ukoliko je cekirano automatsko postavljanje razmere ulazi ovde
                    if (OptionsInOnlineMode.isAutoChecked)
                    {
                        //razmera se manja u odnosu na zadnju isctanu tacku online grafika
                        plotter.FitToView();
                        plotter.Viewport.Restrictions.Clear();
                    }

                }));

            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void createOnlineGraphics()}", System.DateTime.Now);
            }

        }

        public void CreateOnlineGraphicsInitial()
        {
            try
            {
                createOnlineGraphicsInitial();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void CreateOnlineGraphicsInitial()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// kreira prazan grafik koji se se popunjavati odgovarajucim vrednostima kada pocne online upis
        /// ali samo pri pokretanju programa se ovaj metod poziva
        /// </summary>
        private void createOnlineGraphicsInitial()
        {
            try
            {
                //isOnlineLine = true;
                int maxPoints = 300000;
                points = new MyPointCollection(maxPoints);
                pointsAll = new MyPointCollection(maxPoints);

                var ds = new EnumerableDataSource<MyPoint>(points);
                ds.SetXMapping(x => x.XAxisValue);
                ds.SetYMapping(y => y.YAxisValue);




                plotter.AddLineGraph(ds, Colors.Green, 2, "Online Mode"); // to use this method you need "using Microsoft.Research.DynamicDataDisplay;"

                if (OptionsInOnlineMode.isManualChecked)
                {

                    plotter.Viewport.AutoFitToView = true;
                    ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
                    restr.YRange = new DisplayRange(-0.01 * OptionsInPlottingMode.yRange/*-0.5*/, OptionsInOnlineMode.yRange);
                    restr.XRange = new DisplayRange(-0.01 * OptionsInPlottingMode.xRange/*-0.5*/, OptionsInOnlineMode.xRange);
                    plotter.Viewport.Restrictions.Add(restr);

                }

                if (OptionsInOnlineMode.isAutoChecked)
                {
                    plotter.FitToView();
                    plotter.Viewport.Restrictions.Clear();
                }

            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void createOnlineGraphicsInitial()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// ucitava opcije koje su zapamcene zadnje u online modu
        /// svodi se na citanje xml fajla na putanji System.Environment.CurrentDirectory + "\\configuration\\onlineModeOptions.xml", koja je definisana u promenljivoj Constants.onlineModeOptionsXml
        /// i popunjavanje strukture OptionsInOnlineMode, 
        /// pa se na osnovu sadrzaja strukture OptionsInOnlineMode popunjava prozor [Options]OptionsOnline.xaml
        /// </summary>
        private void LoadOnlineoptions()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.onlineModeOptionsXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("refreshTimeInterval"))
                        {
                            OptionsInOnlineMode.refreshTimeInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("Resolution"))
                        {
                            OptionsInOnlineMode.Resolution = textReader.ReadElementContentAsInt();
                        }
                        if (textReader.Name.Equals("L0"))
                        {
                            OptionsInOnlineMode.L0 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("S0"))
                        {
                            OptionsInOnlineMode.S0 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }
                        if (textReader.Name.Equals("nutnDivide"))
                        {
                            OptionsInOnlineMode.nutnDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCalForceDivide.Text = OptionsInOnlineMode.nutnDivide.ToString();
                            //onlineMode.OptionsOnline.tfCalForceDivide.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCalForceDivide.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("nutnMultiple"))
                        {
                            OptionsInOnlineMode.nutnMultiple = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCalForceMultiple.Text = OptionsInOnlineMode.nutnMultiple.ToString();
                            //onlineMode.OptionsOnline.tfCalForceMultiple.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCalForceMultiple.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmDivide"))
                        {
                            OptionsInOnlineMode.mmDivide = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCalElonDivide.Text = OptionsInOnlineMode.mmDivide.ToString();
                            //onlineMode.OptionsOnline.tfCalElonDivide.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCalElonDivide.Foreground = Brushes.White;

                        }
                        if (textReader.Name.Equals("mmCoeff"))
                        {
                            OptionsInOnlineMode.mmCoeff = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCalElonMultiple.Text = OptionsInOnlineMode.mmCoeff.ToString();
                            //onlineMode.OptionsOnline.tfCalElonMultiple.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCalElonMultiple.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmDivideWithEkstenziometer"))
                        {
                            OptionsInOnlineMode.mmDivideWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCalElonDivide2.Text = OptionsInOnlineMode.mmDivideWithEkstenziometer.ToString();
                            //this.OptionsOnline.tfCalElonDivide2.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCalElonDivide2.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("mmCoeffWithEkstenziometer"))
                        {
                            OptionsInOnlineMode.mmCoeffWithEkstenziometer = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCalElonMultiple2.Text = OptionsInOnlineMode.mmCoeffWithEkstenziometer.ToString();
                            //this.OptionsOnline.tfCalElonMultiple2.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCalElonMultiple2.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("isAutoChecked"))
                        {
                            string isAuto = textReader.ReadElementContentAsString();
                            if (isAuto.Equals("True"))
                            {
                                OptionsInOnlineMode.isAutoChecked = true;
                                //onlineMode.rbtnAuto.IsChecked = true;

                                OptionsInOnlineMode.isManualChecked = false;
                                //onlineMode.rbtnManual.IsChecked = false;
                                //onlineMode.tfRatioForce.IsReadOnly = false;
                                //onlineMode.tfRatioElongation.IsReadOnly = false;

                                OptionsInOnlineMode.xRange = 0.95;
                                OptionsInOnlineMode.yRange = 0.95;
                            }
                            if (isAuto.Equals("False"))
                            {
                                OptionsInOnlineMode.isAutoChecked = false;
                                //onlineMode.rbtnAuto.IsChecked = false;

                                OptionsInOnlineMode.isManualChecked = true;
                                //onlineMode.rbtnManual.IsChecked = true;
                                //onlineMode.tfRatioForce.IsReadOnly = true;
                                //onlineMode.tfRatioElongation.IsReadOnly = true;
                            }
                        }
                        if (textReader.Name.Equals("isManualChecked"))
                        {
                            string isManual = textReader.ReadElementContentAsString();
                            if (isManual.Equals("True"))
                            {
                                OptionsInOnlineMode.isManualChecked = true;
                                //onlineMode.rbtnManual.IsChecked = true;
                                //onlineMode.tfRatioForce.IsReadOnly = true;
                                //onlineMode.tfRatioElongation.IsReadOnly = true;
                            }
                            if (isManual.Equals("False"))
                            {
                                OptionsInOnlineMode.isManualChecked = false;
                                //onlineMode.rbtnManual.IsChecked = false;
                                //onlineMode.tfRatioForce.IsReadOnly = false;
                                //onlineMode.tfRatioElongation.IsReadOnly = false;

                                OptionsInOnlineMode.xRange = 0.95;
                                OptionsInOnlineMode.yRange = 0.95;
                            }
                        }

                        if (textReader.Name.Equals("ratioElongation"))
                        {
                            OptionsInOnlineMode.xRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfRatioElongation.Text = OptionsInOnlineMode.xRange.ToString();
                            //onlineMode.OptionsOnline.tfRatioElongation.Foreground = Brushes.Black;
                            this.OptionsOnline.tfRatioElongation.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("ratioForce"))
                        {
                            OptionsInOnlineMode.yRange = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfRatioForce.Text = OptionsInOnlineMode.yRange.ToString();
                            //onlineMode.OptionsOnline.tfRatioForce.Foreground = Brushes.Black;
                            this.OptionsOnline.tfRatioForce.Foreground = Brushes.White;
                        }
                        if (textReader.Name.Equals("onlineWriteEndTimeInterval"))
                        {
                            OptionsInOnlineMode.onlineWriteEndTimeInterval = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfEndOnlineWrite.Text = OptionsInOnlineMode.onlineWriteEndTimeInterval.ToString();
                            //onlineMode.OptionsOnline.tfEndOnlineWrite.Foreground = Brushes.Black;
                            this.OptionsOnline.tfEndOnlineWrite.Foreground = Brushes.White;
                        }


                        if (textReader.Name.Equals("calculateMaxForceForTf"))
                        {
                            string calculateMaxForceForTfStr = textReader.ReadElementContentAsString();
                            if (calculateMaxForceForTfStr.Equals("True"))
                            {
                                OptionsInOnlineMode.calculateMaxForceForTf = true;
                                this.tfMaxForceInKN.Text = Constants.ZERO;
                            }
                            if (calculateMaxForceForTfStr.Equals("False"))
                            {
                                OptionsInOnlineMode.calculateMaxForceForTf = false;
                                this.tfMaxForceInKN.Text = String.Empty;
                            }
                        }

                        if (textReader.Name.Equals("isCalibration"))
                        {
                            string isCalibrationStr = textReader.ReadElementContentAsString();
                            if (isCalibrationStr.Equals("True"))
                            {
                                OptionsInOnlineMode.isCalibration = true;
                            }
                            if (isCalibrationStr.Equals("False"))
                            {
                                OptionsInOnlineMode.isCalibration = false;
                            }
                        }


                        if (textReader.Name.Equals("timeIntervalForCalculationOfChangedParameters"))
                        {
                            OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("isE2E4BorderSelected"))
                        {
                            string isE2E4BorderSelectedstr = textReader.ReadElementContentAsString();
                            if (isE2E4BorderSelectedstr.Equals("True"))
                            {
                                OptionsInOnlineMode.isE2E4BorderSelected = true;
                            }
                            if (isE2E4BorderSelectedstr.Equals("False"))
                            {
                                OptionsInOnlineMode.isE2E4BorderSelected = false;

                            }
                        }


                        if (textReader.Name.Equals("E2E4Border"))
                        {
                            OptionsInOnlineMode.E2E4Border = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfE2E4Border.Text = OptionsInOnlineMode.E2E4Border.ToString();
                            //onlineMode.OptionsOnline.tfE2E4Border.Foreground = Brushes.Black;
                            this.OptionsOnline.tfE2E4Border.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("E3E4Border"))
                        {
                            OptionsInOnlineMode.E3E4Border = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfE3E4Border.Text = OptionsInOnlineMode.E3E4Border.ToString();
                            //this.OptionsOnline.tfE3E4Border.Foreground = Brushes.Black;
                            this.OptionsOnline.tfE3E4Border.Foreground = Brushes.White;
                        }

                        if (textReader.Name.Equals("COM"))
                        {
                            OptionsInOnlineMode.COM = Convert.ToInt32(textReader.ReadElementContentAsString());
                            this.OptionsOnline.tfCOM.Text = OptionsInOnlineMode.COM.ToString();
                            //this.OptionsOnline.tfCOM.Foreground = Brushes.Black;
                            this.OptionsOnline.tfCOM.Foreground = Brushes.White;
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

        /// <summary>
        /// ucitava S0 i L0 da bi se popunili zadnji podaci, ulaznih prozora koji iskacu, kada se u online modu klikne na dugme podaci o uzorku
        /// </summary>
        private void initializeLastS0AndL0Online()
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.lastOnlineHeaderXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("tfS0"))
                        {
                            OptionsInOnlineMode.S0 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }

                        if (textReader.Name.Equals("tfL0"))
                        {
                            OptionsInOnlineMode.L0 = MainWindow.SetPointAtDoubleInstedComma(textReader.ReadElementContentAsString());
                        }


                    }//if (nType == XmlNodeType.Element)

                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString() + " {initializeLastS0AndL0Online}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void initializeLastS0AndL0Online()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// cuva inforacije o prozoru koji se prikazuje u offline modu
        /// </summary>
        private GraphicPlotting plotting;
        /// <summary>
        /// cuva inforacije o prozoru koji se prikazuje u offline modu
        /// </summary>
        public GraphicPlotting Plotting
        {
            get { return plotting; }
            set 
            {
                if (value != null)
                {
                    plotting = value;
                }
            }
        }

        private int lngHandle;

        private bool simulationMode = false;

        public OnlineMode(bool simulationM = false)
        {
            try
            {
                InitializeComponent();
                simulationMode = simulationM;
                //this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight * 0.80);
                //this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth * 0.80);

                //only testing this must be in options
                string fp = System.Environment.CurrentDirectory + "\\files\\___001.txt";
                //string fpOnline = Constants.onlineFilepath;
                string fpOnline = Properties.Settings.Default.onlineFilepath;
                dataReader = new DataReader(fpOnline, OptionsInOnlineMode.L0, OptionsInOnlineMode.S0);
                fpOnlineGlobal = fpOnline;
                fpGlobal = fp;

                this.DataContext = this;

                //points = new MyPointCollection(totalPoints);


                maxChangeOfPreasure = 0.0;
                maxChangeOfElongation = 0.0;
                maxForceInKN = 0.0;

                counterWhoDetermitedOneSecond = Convert.ToInt32(milisecInSec / OptionsInOnlineMode.refreshTimeInterval);
                currCounterWhoDetermitedOneSecond = 0;


                // Instantiate new _BlinkingButtonTimer and add event handler
                _BlinkingButtonTimer = new MicroTimer();
                _BlinkingButtonTimer.MicroTimerElapsed +=
                    new MicroTimer.MicroTimerElapsedEventHandler(BlinkingTimer);
                long intervalBlinkingButtonTimer = 500000;//500 ms
                _BlinkingButtonTimer.Interval = intervalBlinkingButtonTimer;
                // Ignore event if late by half the interval
                _BlinkingButtonTimer.IgnoreEventIfLateBy = intervalBlinkingButtonTimer / 2;

                // Instantiate new MicroTimer and add event handler
                _microTimer = new MicroTimer();
                _microTimer.MicroTimerElapsed +=
                    new MicroTimer.MicroTimerElapsedEventHandler(OnTimedEvent);

                _microTimerMachineWorking = new MicroTimer();
                _microTimerMachineWorking.MicroTimerElapsed +=
                    new MicroTimer.MicroTimerElapsedEventHandler(OnTimedEventMachineWorking);

                _LabJackWorking = new MicroTimer();
                _LabJackWorking.MicroTimerElapsed +=
                    new MicroTimer.MicroTimerElapsedEventHandler(Timer2_Tick);
                _LabJackWorking.Interval = 16660;

                changeOfPreassureForFirstDerivation = 0;
                //animationLines.Add("Refresh time in [ms]: " + OptionsInOnlineMode.refreshTimeInterval);
                //File.WriteAllLines(Constants.animationFilepath, animationLines);

                optionsOnline = new OptionsOnline(this);
                LoadOnlineoptions();
                optionsOnline.Close();

                //createOnlineGraphicsInitial();

                initializeLastS0AndL0Online();

                //set serial port
                serialport.BaudRate = 38400;
                serialport.PortName = "COM" + OptionsInOnlineMode.COM.ToString();
                serialport.DataReceived += new SerialDataReceivedEventHandler(dataReceived);
                if (simulationMode == false)
                {
                    serialport.Open();//otvori ga samo jednom i to u konstruktoru
                }


                if (simulationMode == false)
                {
                    LJUD.OpenLabJack(LJUD.DEVICE.U6, LJUD.CONNECTION.USB, "0", true, ref lngHandle);
                }


                ////only for testing purpose
                if (simulationMode == true)
                {
                    sensorForPreassure = new SensorSimulator(System.Environment.CurrentDirectory + "\\SensorsData.txt");
                    sensorForElongation = new SensorSimulator(System.Environment.CurrentDirectory + "\\SensorsData.txt");
                    sensorForPreassure.LoadData(1);
                    sensorForElongation.LoadData(2);

                    //napravi i pokreni testne tajmere
                    long intervalTestLabJack = 16660;//16660 microseconds
                    _LabJackWorkingTest = new MicroTimer();
                    _LabJackWorkingTest.MicroTimerElapsed +=
                        new MicroTimer.MicroTimerElapsedEventHandler(Timer2_TickTest);
                    _LabJackWorkingTest.Interval = intervalTestLabJack;


                    // Ignore event if late by half the interval
                    _LabJackWorkingTest.IgnoreEventIfLateBy = intervalTestLabJack / 2;



                    long intervalTestserialPort = 16660;//16660 microseconds
                    _serialPortTestTimer = new MicroTimer();
                    _serialPortTestTimer.MicroTimerElapsed +=
                        new MicroTimer.MicroTimerElapsedEventHandler(dataReceivedTest);
                    _serialPortTestTimer.Interval = intervalTestserialPort;


                    // Ignore event if late by half the interval
                    _serialPortTestTimer.IgnoreEventIfLateBy = intervalTestserialPort / 2;

                }




            }
            catch (IOException ex)
            {
                if (ex.Message.ToString().StartsWith("The port") && ex.Message.ToString().Contains("does not exist"))
                {
                    MessageBox.Show(ex.Message.ToString());
                    Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public OnlineMode(bool simulationM = false)}", System.DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString() + " {Online Mode}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public OnlineMode(bool simulationM = false)}", System.DateTime.Now);
            }

        }

        double numScansRequested = 0;
        double NUM_SCANS = 10;

        private void StartStreamWithEkstenziometar()
        {
            try
            {
                //Configure the stream:
                //Configure all analog inputs for 16-bit resolution
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.AIN_RESOLUTION, 9, 0, 0);

                //Configure the analog input range on channel 2 for bipolar 10 volts (extensometer)
                //LJUD.AddRequest(lngHandle, LJUD.IO.PUT_AIN_RANGE, 2, LJUD.RANGES.BIP10V, 0, 0);
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_AIN_RANGE, 1, 2, 0, 0);


                //LJUD.AddRequest(lngHandle, LJUD.IO.GET_AIN, (LJUD.CHANNEL)1, 0, 0, 0);




               


                numScansRequested = NUM_SCANS;
                if (onHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                {
                    //Execute the list of requests.
                    LJUD.GoOne(lngHandle);

                    //LJUD.ePut(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TDAC_SCL_PIN_NUM, 2, 0);

                    //LJUD.ePut(lngHandle, LJUD.IO.TDAC_COMMUNICATION, LJUD.CHANNEL.TDAC_UPDATE_DACA, 10.0, 0);

                    numScansRequested = NUM_SCANS;

                    _LabJackWorking.Start();
                }



                //'Nulovanje sile  
                //double highLevel = 1;
                //double lowLevel = 0;

                //LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, LJUD.CHANNEL.IP_ADDRESS, ref highLevel, 0);
                //System.Threading.Thread.Sleep(900);
                //LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, LJUD.CHANNEL.IP_ADDRESS, ref lowLevel, 0);

                //'Execute the requests.
                //LJUD.GoOne(lngHandle);

                //LJUD.ePut(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TDAC_SCL_PIN_NUM, 2, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Da biste nastavili da radite morate proveriti na kom COM-u vam se nalazi uredjaj[LabJack] i restartovati racunar!");
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void StartStreamWithEkstenziometar()}", System.DateTime.Now);
            }
        }


        private void StartStream() 
        {


            try
            {
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_COUNTER_PIN_OFFSET, 0, 0, 0);

                //'Use the 48 MHz timer clock base with divider.  Since we are using clock with divisor
                //'support, Counter0 is not available.
                //byte[] ar = new byte { 0 };
                //LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_CLOCK_BASE, LJUD.TIMERCLOCKS.MHZ48_DIV, 0, 0);
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_CLOCK_BASE, 26, 0, 0);

                //'Set the divisor to 48 so the actual timer clock is 1 MHz.
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_CLOCK_DIVISOR, 48, 0, 0);

                //'Enable 2 timers.  They will use FIO0 and FIO1.
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.NUMBER_TIMERS_ENABLED, 2, 0, 0);

                //'Configure Timer0 and Timer1 as Quadrature inputs.
                //LJUD.AddRequest(lngHandle, LJUD.IO.PUT_TIMER_MODE, 0, LJUD.TIMERMODE.QUAD, 0, 0);
                //LJUD.AddRequest(lngHandle, LJUD.IO.PUT_TIMER_MODE, 1, LJUD.TIMERMODE.QUAD, 0, 0);
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_TIMER_MODE, 0, 8, 0, 0);
                LJUD.AddRequest(lngHandle, LJUD.IO.PUT_TIMER_MODE, 1, 8, 0, 0);

                //'Nulovanje sile  
                //double highLevel = 1;
                //double lowLevel = 0;
                //LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, 2, ref highLevel, 0);
                //System.Threading.Thread.Sleep(900); 
                //LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, 2, ref lowLevel, 0);

                

                LJUD.ePut(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TDAC_SCL_PIN_NUM, 2, 0);

                //numScansRequested = NUM_SCANS;
                if (onHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    //'Execute the requests.
                    LJUD.GoOne(lngHandle);
                    _LabJackWorking.Start();
                }


                //'Nulovanje sile  
                //double highLevel = 1;
                //double lowLevel = 0;

                //LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, LJUD.CHANNEL.IP_ADDRESS, ref highLevel, 0);
                //System.Threading.Thread.Sleep(900);
                //LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, LJUD.CHANNEL.IP_ADDRESS, ref lowLevel, 0);

                //'Execute the requests.
                //LJUD.GoOne(lngHandle);

                //LJUD.ePut(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TDAC_SCL_PIN_NUM, 2, 0);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void StartStream()}", System.DateTime.Now);
            }
        }

        private void btnSetTlb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //'Nulovanje sile  
                double highLevel = 1;
                double lowLevel = 0;
                LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, LJUD.CHANNEL.IP_ADDRESS, ref highLevel, 0);
                System.Threading.Thread.Sleep(900);
                LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, LJUD.CHANNEL.IP_ADDRESS, ref lowLevel, 0);

                //'Execute the requests.
                LJUD.GoOne(lngHandle);

                LJUD.ePut(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TDAC_SCL_PIN_NUM, 2, 0);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnSetTlb_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }


    //     Private Sub StartStream()
    //    If Not running Then
    //        Try
    //            LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_COUNTER_PIN_OFFSET, 0, 0, 0)

    //            'Use the 48 MHz timer clock base with divider.  Since we are using clock with divisor
    //            'support, Counter0 is not available.
    //            LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_CLOCK_BASE, LJUD.TIMERCLOCKS.MHZ48_DIV, 0, 0)

    //            'Set the divisor to 48 so the actual timer clock is 1 MHz.
    //            LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TIMER_CLOCK_DIVISOR, 48, 0, 0)

    //            'Enable 2 timers.  They will use FIO0 and FIO1.
    //            LJUD.AddRequest(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.NUMBER_TIMERS_ENABLED, 2, 0, 0)

    //            'Configure Timer0 and Timer1 as Quadrature inputs.
    //            LJUD.AddRequest(lngHandle, LJUD.IO.PUT_TIMER_MODE, 0, LJUD.TIMERMODE.QUAD, 0, 0)
    //            LJUD.AddRequest(lngHandle, LJUD.IO.PUT_TIMER_MODE, 1, LJUD.TIMERMODE.QUAD, 0, 0)

    //            'Execute the requests.
    //            LJUD.GoOne(lngHandle)

    //            'LJUD.ePut(lngHandle, LJUD.IO.PUT_CONFIG, LJUD.CHANNEL.TDAC_SCL_PIN_NUM, 2, 0)

    //            numScansRequested = NUM_SCANS

    //        Catch ex As LabJackUDException
    //            ErrorMessage(ex)
    //        End Try

    //        ' Get results until there is no more data available for error checking
    //        Dim isFinished As Boolean
    //        isFinished = False
    //        While Not isFinished
    //            Try
    //                LJUD.GetNextResult(lngHandle, ioType, channel, dummyDouble, dummyInt, dummyDouble)
    //            Catch ex As LabJackUDException
    //                ' If we get an error, report it.  If the error is NO_MORE_DATA_AVAILABLE we are done
    //                If (ex.LJUDError = U6.LJUDERROR.NO_MORE_DATA_AVAILABLE) Then
    //                    isFinished = True
    //                Else
    //                    ErrorMessage(ex)
    //                End If
    //            End Try
    //        End While

    //        Try
    //            'Start the measurement.
    //            cutOff = Servisne_opcije.CO
    //            rezolucija = Servisne_opcije.Rezol

    //            'Ocitavanje nule za silu
    //            'LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, 5, 1, 0)
    //            'Threading.Thread.Sleep(700)
    //            'LJUD.eGet(lngHandle, LJUD.IO.PUT_DIGITAL_BIT, 5, 0, 0)

    //            If Not (comPort.IsOpen = True) Then
    //                comPort.Open()
    //            End If

    //            Dim stemp As Double
    //            x0 = 0
    //            For i As Integer = 1 To 10
    //                Double.TryParse(msg, stemp)
    //                x0 += stemp
    //            Next
    //            x0 /= 10

    //            'If cbUprav.Checked Then

    //            '    brzina1 = CDbl(txtBrzina1.Text)
    //            '    referenca = brzina1

    //            '    Kprop = 0.01
    //            '    Kint = 0

    //            '    Timer2A.Interval = READ_INTERVAL
    //            '    Timer2A.Enabled = True
    //            '    stp = New Stopwatch
    //            '    stp.Start()

    //            'Else

    //            If flagETAL Then
    //                TimerE.Interval = READ_INTERVAL
    //                TimerE.Enabled = True
    //            Else
    //                Timer2.Interval = READ_INTERVAL
    //                Timer2.Enabled = True
    //            End If

    //            'End If

    //            'Toggle the mode
    //            running = True
    //        Catch ex As LabJackUDException
    //            ErrorMessage(ex)
    //        Catch ex As Exception
    //            MessageBox.Show("Neuspelo pokretanje: " & ex.Message)
    //        End Try
    //    Else
    //        StopStream()
    //    End If
    //End Sub


    //    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
     

    //        'Ocitavanje kanala
    //        LJUD.eGet(lngHandle, LJUD.IO.GET_TIMER, 0, adblDataIL, 0)
           
    //End Sub

        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;

        private bool IsFileLocked(Exception exception)
        {
            try
            {
                int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
                return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        internal bool CanReadFile(string filePath)
        {
            //Try-Catch so we dont crash the program and can check the exception
            try
            {
                //The "using" is important because FileStream implements IDisposable and
                //"using" will avoid a heap exhaustion situation when too many handles  
                //are left undisposed.
                using (FileStream fileStream = File.Open(Constants.onlineFilepath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    if (fileStream != null) fileStream.Close();  //This line is me being overly cautious, fileStream will never be null unless an exception occurs... and I know the "using" does it but its helpful to be explicit - especially when we encounter errors - at least for me anyway!
                }
            }
            catch (IOException ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (IsFileLocked(ex))
                {
                    // do something, eg File.Copy or present the user with a MsgBox - I do not recommend Killing the process that is locking the file
                    return false;
                }
            }
            finally
            { }
            return true;
        }

        private double adblDataIL = 0;
        private double adblDataILDummy = 0;
     

        private void Timer2_TickTest(object sender, MicroTimerEventArgs timerEventArgs)
        {
            try
            {

                if (counterForElongation >= sensorForElongation.DataOfSensor.Count)
                {
                    //zaustavi tajmer
                    _LabJackWorkingTest.Stop();
                   return;
                }

                if (CanReadFile(Constants.onlineFilepath) == false)
                {
                    return;
                }

                //'Ocitavanje kanala
                //LJUD.eGet(lngHandle, LJUD.IO.GET_TIMER, 0, ref adblDataIL, 0);
                if (File.Exists(Constants.onlineFilepath))
                {
                    //string currData = historyOfMsgPreassure.Last() + '\t' + adblDataIL.ToString();
                    string currData = string.Empty;
                    currData = historyOfMsgPreassure.Last() + '\t' + sensorForElongation.DataOfSensor[counterForElongation].ToString();
                    if (currData.Contains("SET") == true)
                    {
                        return;
                    }
                    counterForElongation++;
                    List<string> list = new List<string>();
                    list.Add(currData);
                    FileInfo file = new FileInfo(Constants.onlineFilepath);
                    if (IsFileinUse(file) == false)
                    {
                        if (CanReadFile(Constants.onlineFilepath) == false)
                        {
                            return;
                        }
                        //using (FileStream fs = File.Open(Constants.onlineFilepath, FileMode.Append, FileAccess.Write, FileShare.Write))
                        //{
                        //    //byte[] bytes = Encoding.ASCII.GetBytes(status);
                        //    //fs.Write(bytes, 0, bytes.Length);
                        //    byte[] bytes = Encoding.ASCII.GetBytes(currData);
                        //    fs.Write(bytes, 0, bytes.Length);
                        //    fs.Close();
                        //    //Console.Write("Write:" + currData + Environment.NewLine);
                        //}
                        File.AppendAllLines(Constants.onlineFilepath, list);
                    }

                }


                //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                //{
                //    tfMaxForceInKN.Text = adblDataIL.ToString();
                //}));
            }
            catch (Exception ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (IsFileLocked(ex))
                {
                    // do something, eg File.Copy or present the user with a MsgBox - I do not recommend Killing the process that is locking the file
                    return;
                }
            }
            finally { }
        }



        private void Timer2_Tick(object sender, MicroTimerEventArgs timerEventArgs) 
        {
            try
            {
              
                //'Ocitavanje kanala
                if (this.IsEkstenziometerUsed == false)
                {
                     LJUD.eGet(lngHandle, LJUD.IO.GET_TIMER, 0, ref adblDataIL, 0);
                     adblDataIL = Math.Round(adblDataIL, 3);

                     LJUD.eGet(lngHandle, LJUD.IO.GET_AIN, 1, ref adblDataILDummy, 0);
                }
                else
                {
                     LJUD.eGet(lngHandle, LJUD.IO.GET_AIN, 1, ref adblDataIL, 0);
                     adblDataIL = Math.Round(adblDataIL,3);
                     //LJUD.eGet(lngHandle, LJUD.IO.GET_TIMER, 0, ref adblDataIL, 0);
                     LJUD.eGet(lngHandle, LJUD.IO.GET_TIMER, 0, ref adblDataILDummy, 0);
                }
                if (File.Exists(Constants.onlineFilepath))
                {
                    string currData = string.Empty;
                    currData = historyOfMsgPreassure.Last() + '\t' + adblDataIL.ToString();
                    List<string> list = new List<string>();
                    list.Add(currData);
                    FileInfo file = new FileInfo(Constants.onlineFilepath);
                    if (IsFileinUse(file) == false)
                    {
                        if (CanReadFile(Constants.onlineFilepath) == false)
                        {
                            return;
                        }
                        //using (FileStream fs = File.Open(Constants.onlineFilepath, FileMode.Append, FileAccess.Write, FileShare.Write))
                        //{
                            //byte[] bytes = Encoding.ASCII.GetBytes(status);
                            //fs.Write(bytes, 0, bytes.Length);
                            //byte[] bytes = Encoding.ASCII.GetBytes(currData);
                            //fs.Write(bytes, 0, bytes.Length);
                            //fs.Close();

                            //Console.Write("Write:" + currData + Environment.NewLine);
                        //}
                        File.AppendAllLines(Constants.onlineFilepath, list);
                    }

                }


                //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                //{
                //    tfMaxForceInKN.Text = adblDataIL.ToString();
                //}));

            }
            catch (LabJackUDException ljex)
            {
                //MessageBox.Show("Exception message: " + ljex.ToString() + System.Environment.NewLine + ljex.StackTrace);
            }
            catch (Exception ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (IsFileLocked(ex))
                {
                    // do something, eg File.Copy or present the user with a MsgBox - I do not recommend Killing the process that is locking the file
                    return;
                }
            }
            finally { }
        }

        private bool green = true;
        private void BlinkingTimer(object sender, MicroTimerEventArgs timerEventArgs)
        {
            try
            {
                if (green == true)
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        btnStartSample.Background = Brushes.White;
                        btnStartSample.Foreground = Brushes.Black;
                    }));
                   
                    green = false;
                }
                else
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        btnStartSample.Background = Brushes.LawnGreen;
                        btnStartSample.Foreground = Brushes.White;
                    }));
                  
                    green = true;
                }
            }
            catch (Exception ex) 
            {
                
            }
        }


        private string msg = string.Empty;
        private List<string> historyOfMsgPreassure = new List<string>();

        //only for testing purpose
        private int counterForPreassure = 0;
        private int counterForElongation = 0;
        private SensorSimulator sensorForPreassure;
        private SensorSimulator sensorForElongation;

        private void dataReceivedTest(object sender, MicroTimerEventArgs timerEventArgs)
        {
            try
            {
                //msg = String.Empty;
                //msg = serialport.ReadExisting();
                if (counterForPreassure >= sensorForPreassure.DataOfSensor.Count)
                {
                    //zaustavi tajmer
                    _serialPortTestTimer.Stop();
                    return;
                }

                msg = sensorForPreassure.DataOfSensor[counterForPreassure].ToString();
                counterForPreassure++;
                historyOfMsgPreassure.Add(msg);
                //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                //{
                //    tfMaxForceInKN.Text = msg;
                //}));

            }
            catch (Exception ex)
            {
               
            }
        }

        private void dataReceived(System.Object sender,System.IO.Ports.SerialDataReceivedEventArgs e) 
        {
            try
            {
                msg = String.Empty;
                msg = serialport.ReadExisting();
                
                List<string> lstmessages = msg.Split('\n').ToList();
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    if (lstmessages.First().Length > 0)
                    {
                        //tfMaxForceInKN.Text = lstmessages.First().Substring(0, lstmessages.First().Length - 1);
                        //double number;
                        //bool isN = double.TryParse(tfMaxForceInKN.Text, out number);
                        //number = number / 100;
                        //historyOfMsgPreassure.Add(number.ToString());
                        historyOfMsgPreassure.Add(lstmessages.First().Substring(0, lstmessages.First().Length - 1));
                    }

                }));
   
            }
            catch (Exception ex)
            {
                
            }
        }

        /// <summary>
        /// postavlja radiodugmad prozora za online opcije
        /// </summary>
        public void setRadioButtons() 
        {
            try
            {
       
                if (OptionsInOnlineMode.isAutoChecked)
                {
                    optionsOnline.rbtnAuto.IsChecked = true;
                    optionsOnline.rbtnAuto.IsChecked = true;

                    optionsOnline.rbtnManual.IsChecked = false;
                    optionsOnline.tfRatioForce.IsReadOnly = true;
                    optionsOnline.tfRatioElongation.IsReadOnly = true;
                }
                else
                {
                    optionsOnline.rbtnAuto.IsChecked = false;

                    optionsOnline.rbtnManual.IsChecked = true;
                    optionsOnline.tfRatioForce.IsReadOnly = false;
                    optionsOnline.tfRatioElongation.IsReadOnly = false;
                }

                if (OptionsInOnlineMode.isE2E4BorderSelected)
                {
                    optionsOnline.rbtnE2E4.IsChecked = true;
                    optionsOnline.rbtnE3E4.IsChecked = false;
                }
                else 
                {
                    optionsOnline.rbtnE2E4.IsChecked = false;
                    optionsOnline.rbtnE3E4.IsChecked = true;
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                //Logger.WriteNode(ex.Message.ToString() + " {setRadioButtons}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void setRadioButtons()}", System.DateTime.Now);
            }
        }




     


        /// <summary>
        /// sluzi za upisivanje zadnjeg unetog zaglavlja tekstualnog fajla u kome se cuvaju informacije o online modu zadnje pokidanog uzorka 
        /// </summary>
        public void WriteXMLLastOnlineHeader()
        {
            try
            {
                writeXMLLastOnlineHeader();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void WriteXMLLastOnlineHeader()}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// sluzi za upisivanje zadnjeg unetog zaglavlja tekstualnog fajla u kome se cuvaju informacije o online modu zadnje pokidanog uzorka 
        /// </summary>
        private void writeXMLLastOnlineHeader() 
        {
            try
            {
                if (onHeader == null)
                {
                    return;
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                if (plotting.Printscreen.IsPrintScreenEmpty == true && window.tab_second.IsSelected == true)
                {
                    return;
                }


                //write in xml file
                string textRangeStr = String.Empty;
                if (onHeader.RemarkOfTesting != null)
                {
                    TextRange textRange = new TextRange(onHeader.RemarkOfTesting.rtfNapomena.Document.ContentStart, onHeader.RemarkOfTesting.rtfNapomena.Document.ContentEnd);
                    textRangeStr = textRange.Text;
                    textRangeStr = textRangeStr.Replace("\r\n", String.Empty);
                }

                if (onHeader != null && onHeader.GeneralData != null && onHeader.ConditionsOfTesting != null && onHeader.MaterialForTesting != null && onHeader.PositionOfTube != null && onHeader.RemarkOfTesting != null)
                {
                    XElement xmlRoot = new XElement("OnlineHeader",
                                            new XElement("OnlineHeaderLastWritten",
                        // GeneralData
                                                        new XElement("tfOperator_GeneralData", onHeader.GeneralData.tfOperator.Text),
                                                        new XElement("tfBrZbIzvestaja_GeneralData", onHeader.GeneralData.tfBrZbIzvestaja.Text),
                                                        new XElement("tfBrUzorka_GeneralData", onHeader.GeneralData.tfBrUzorka.Text + "/" + onHeader.GeneralData.tfBrUzorkaNumberOfSample.Text),
                                                        new XElement("tfSarza_GeneralData", onHeader.GeneralData.tfSarza.Text),
                                                        new XElement("tfRadniNalog_GeneralData", onHeader.GeneralData.tfRadniNalog.Text),
                                                        new XElement("tfNaručilac_GeneralData", onHeader.GeneralData.tfNaručilac.Text),

                                                        //ConditionsOfTesting
                                                        new XElement("tfStandard_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfStandard.Text),
                                                        new XElement("tfMetoda_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfMetoda.Text),
                                                        new XElement("tfStandardZaN_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfStandardZaN.Text),
                                                        new XElement("tfMasina_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfMasina.Text),
                                                        new XElement("tfBegOpsegMasine_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfBegOpsegMasine.Text),
                                                        new XElement("tfTemperatura_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfTemperatura.Text),
                                                        (onHeader.ConditionsOfTesting.rbtnYes.IsChecked == true) ? new XElement("rbtnYes_ConditionsOfTesting", "True") : new XElement("rbtnYes_ConditionsOfTesting", "False"),
                                                        (onHeader.ConditionsOfTesting.rbtnNo.IsChecked == true) ? new XElement("rbtnNo_ConditionsOfTesting", "True") : new XElement("rbtnNo_ConditionsOfTesting", "False"),

                                                        //MaterialForTesting
                                                        new XElement("tfProizvodjac_MaterialForTesting", onHeader.MaterialForTesting.tfProizvodjac.Text),
                                                        new XElement("tfDobavljac_MaterialForTesting", onHeader.MaterialForTesting.tfDobavljac.Text),
                                                        new XElement("tfPolazniKvalitet_MaterialForTesting", onHeader.MaterialForTesting.tfPolazniKvalitet.Text),
                                                        new XElement("tfNazivnaDebljina_MaterialForTesting", onHeader.MaterialForTesting.tfNazivnaDebljina.Text),
                                                        (onHeader.MaterialForTesting.rbtnValjani.IsChecked == true) ? new XElement("rbtnValjani_MaterialForTesting", "True") : new XElement("rbtnValjani_MaterialForTesting", "False"),
                                                        (onHeader.MaterialForTesting.rbtnVučeni.IsChecked == true) ? new XElement("rbtnVučeni_MaterialForTesting", "True") : new XElement("rbtnVučeni_MaterialForTesting", "False"),
                                                        (onHeader.MaterialForTesting.rbtnKovani.IsChecked == true) ? new XElement("rbtnKovani_MaterialForTesting", "True") : new XElement("rbtnKovani_MaterialForTesting", "False"),
                                                        (onHeader.MaterialForTesting.rbtnLiveni.IsChecked == true) ? new XElement("rbtnLiveni_MaterialForTesting", "True") : new XElement("rbtnLiveni_MaterialForTesting", "False"),

                                                        //Epruveta_OnlineHeader
                                                        (onHeader.rbtnEpvOblikObradjena.IsChecked == true) ? new XElement("rbtnEpvOblikObradjena", "True") : new XElement("rbtnEpvOblikObradjena", "False"),
                                                        (onHeader.rbtnEpvOblikNeobradjena.IsChecked == true) ? new XElement("rbtnEpvOblikNeobradjena", "True") : new XElement("rbtnEpvOblikNeobradjena", "False"),
                                                        (onHeader.rbtnEpvTipProporcionalna.IsChecked == true) ? new XElement("rbtnEpvTipProporcionalna", "True") : new XElement("rbtnEpvTipProporcionalna", "False"),
                                                        (onHeader.rbtnEpvTipNeproporcionalna.IsChecked == true) ? new XElement("rbtnEpvTipNeproporcionalna", "True") : new XElement("rbtnEpvTipNeproporcionalna", "False"),
                                                        (onHeader.rbtnEpvK1.IsChecked == true) ? new XElement("rbtnEpvK1", "True") : new XElement("rbtnEpvK1", "False"),
                                                        (onHeader.rbtnEpvK2.IsChecked == true) ? new XElement("rbtnEpvK2", "True") : new XElement("rbtnEpvK2", "False"),
                                                        (onHeader.rbtnEpvVrstaPravougaona.IsChecked == true) ? new XElement("rbtnEpvVrstaPravougaona", "True") : new XElement("rbtnEpvVrstaPravougaona", "False"),
                                                        (onHeader.rbtnEpvVrstaPravougaona.IsChecked == true) ? new XElement("a", onHeader.tfAGlobal.Text == String.Empty ? LastInputOutputSavedData.a0Pravougaona : onHeader.tfAGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaPravougaona.IsChecked == true) ? new XElement("b", onHeader.tfBGlobal.Text == String.Empty ? LastInputOutputSavedData.b0Pravougaona : onHeader.tfBGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaKruzni.IsChecked == true) ? new XElement("rbtnEpvVrstaKruzni", "True") : new XElement("rbtnEpvVrstaKruzni", "False"),
                                                        (onHeader.rbtnEpvVrstaKruzni.IsChecked == true) ? new XElement("D", onHeader.tfDGlobal.Text == String.Empty ? LastInputOutputSavedData.D0Kruzna : onHeader.tfDGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaCevasti.IsChecked == true) ? new XElement("rbtnEpvVrstaCevasti", "True") : new XElement("rbtnEpvVrstaCevasti", "False"),
                                                        (onHeader.rbtnEpvVrstaCevasti.IsChecked == true) ? new XElement("D", onHeader.tfDGlobal.Text == String.Empty ? LastInputOutputSavedData.D0Cevasta : onHeader.tfDGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaCevasti.IsChecked == true) ? new XElement("a", onHeader.tfAGlobal.Text == String.Empty ? LastInputOutputSavedData.a0Cevasta : onHeader.tfAGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaDeocev.IsChecked == true) ? new XElement("rbtnEpvVrstaDeocev", "True") : new XElement("rbtnEpvVrstaDeocev", "False"),
                                                        (onHeader.rbtnEpvVrstaDeocev.IsChecked == true) ? new XElement("D", onHeader.tfDGlobal.Text == String.Empty ? LastInputOutputSavedData.D0Deocev : onHeader.tfDGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaDeocev.IsChecked == true) ? new XElement("a", onHeader.tfAGlobal.Text == String.Empty ? LastInputOutputSavedData.a0Deocev : onHeader.tfAGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaDeocev.IsChecked == true) ? new XElement("b", onHeader.tfBGlobal.Text == String.Empty ? LastInputOutputSavedData.b0Deocev : onHeader.tfBGlobal.Text) : null,
                                                        (onHeader.rbtnEpvVrstaSestaugaona.IsChecked == true) ? new XElement("rbtnEpvVrstaSestaugaona", "True") : new XElement("rbtnEpvVrstaSestaugaona", "False"),
                                                        (onHeader.rbtnEpvVrstaSestaugaona.IsChecked == true) ? new XElement("d", onHeader.tfDGlobal.Text == String.Empty ? LastInputOutputSavedData.d0Sestaugaona : onHeader.tfDGlobal.Text) : null,
                                                        new XElement("tfS0", onHeader.tfS0.Text),
                                                        new XElement("tfL0", onHeader.tfL0.Text),
                                                        new XElement("tfLc", onHeader.tfLc.Text),

                                                        // PositionOfTube
                                                        new XElement("tfCustomPravacValjanja_PositionOfTube", onHeader.PositionOfTube.tfCustomPravacValjanja.Text),
                                                        new XElement("tfCustomSirinaTrake_PositionOfTube", onHeader.PositionOfTube.tfCustomSirinaTrake.Text),
                                                        new XElement("tfCustomDuzinaTrake_PositionOfTube", onHeader.PositionOfTube.tfCustomDuzinaTrake.Text),

                                                        // Remarks
                                                        (LastInputOutputSavedData.rtfNapomena_RemarkOfTesting.Length <= Constants.MAXREMARKSTESTINGLENGTH) ? new XElement("rtfNapomena_RemarkOfTesting", textRangeStr) : new XElement("rtfNapomena_RemarkOfTesting", String.Empty)

                                                        )
                                            );

                    xmlRoot.Save(Constants.lastOnlineHeaderXml);
                }


                //using (XmlWriter writer = XmlWriter.Create(Constants.lastOnlineHeaderXml))
                //{
                //    writer.WriteStartDocument();
                //    writer.WriteStartElement("OnlineHeader");


                //    writer.WriteStartElement("OnlineHeaderLastWritten");

                //    if (onHeader != null)
                //    {

                //        #region GeneralData

                //        if (onHeader.GeneralData != null)
                //        {

                //            //writer.WriteElementString("tfbrIzvestaja_GeneralData", onHeader.GeneralData.tfbrIzvestaja.Text);
                //            writer.WriteElementString("tfOperator_GeneralData", onHeader.GeneralData.tfOperator.Text);
                //            writer.WriteElementString("tfBrZbIzvestaja_GeneralData", onHeader.GeneralData.tfBrZbIzvestaja.Text);
                //            writer.WriteElementString("tfBrUzorka_GeneralData", onHeader.GeneralData.tfBrUzorka.Text + Constants.KOSA_CRTA + onHeader.GeneralData.tfBrUzorkaNumberOfSample.Text);
                //            writer.WriteElementString("tfSarza_GeneralData", onHeader.GeneralData.tfSarza.Text);
                //            writer.WriteElementString("tfRadniNalog_GeneralData", onHeader.GeneralData.tfRadniNalog.Text);
                //            writer.WriteElementString("tfNaručilac_GeneralData", onHeader.GeneralData.tfNaručilac.Text);
                //        }


                //        #endregion

                //        #region ConditionsOfTesting

                //        if (onHeader.ConditionsOfTesting != null)
                //        {

                //            writer.WriteElementString("tfStandard_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfStandard.Text);
                //            writer.WriteElementString("tfMetoda_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfMetoda.Text);
                //            writer.WriteElementString("tfStandardZaN_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfStandardZaN.Text);
                //            writer.WriteElementString("tfMasina_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfMasina.Text);
                //            writer.WriteElementString("tfBegOpsegMasine_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfBegOpsegMasine.Text);
                //            //writer.WriteElementString("tfEndOpsegMasine_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfEndOpsegMasine.Text);
                //            writer.WriteElementString("tfTemperatura_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfTemperatura.Text);

                //            if (onHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                //            {
                //                writer.WriteElementString("rbtnYes_ConditionsOfTesting", "True");
                //            }
                //            else
                //            {
                //                writer.WriteElementString("rbtnYes_ConditionsOfTesting", "False");
                //            }

                //            if (onHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                //            {
                //                writer.WriteElementString("rbtnNo_ConditionsOfTesting", "True");
                //            }
                //            else
                //            {
                //                writer.WriteElementString("rbtnNo_ConditionsOfTesting", "False");
                //            }
                //        }

                //        #endregion

                //        #region MaterialForTesting

                //        if (onHeader.MaterialForTesting != null)
                //        {

                //            writer.WriteElementString("tfProizvodjac_MaterialForTesting", onHeader.MaterialForTesting.tfProizvodjac.Text);
                //            writer.WriteElementString("tfDobavljac_MaterialForTesting", onHeader.MaterialForTesting.tfDobavljac.Text);
                //            writer.WriteElementString("tfPolazniKvalitet_MaterialForTesting", onHeader.MaterialForTesting.tfPolazniKvalitet.Text);
                //            writer.WriteElementString("tfNazivnaDebljina_MaterialForTesting", onHeader.MaterialForTesting.tfNazivnaDebljina.Text);

                //            if (onHeader.MaterialForTesting.rbtnValjani.IsChecked == true)
                //            {
                //                writer.WriteElementString("rbtnValjani_MaterialForTesting", "True");
                //            }
                //            else
                //            {
                //                writer.WriteElementString("rbtnValjani_MaterialForTesting", "False");
                //            }


                //            if (onHeader.MaterialForTesting.rbtnVučeni.IsChecked == true)
                //            {
                //                writer.WriteElementString("rbtnVučeni_MaterialForTesting", "True");
                //            }
                //            else
                //            {
                //                writer.WriteElementString("rbtnVučeni_MaterialForTesting", "False");
                //            }

                //            if (onHeader.MaterialForTesting.rbtnKovani.IsChecked == true)
                //            {
                //                writer.WriteElementString("rbtnKovani_MaterialForTesting", "True");
                //            }
                //            else
                //            {
                //                writer.WriteElementString("rbtnKovani_MaterialForTesting", "False");
                //            }


                //            if (onHeader.MaterialForTesting.rbtnLiveni.IsChecked == true)
                //            {
                //                writer.WriteElementString("rbtnLiveni_MaterialForTesting", "True");
                //            }
                //            else
                //            {
                //                writer.WriteElementString("rbtnLiveni_MaterialForTesting", "False");
                //            }
                //        }

                //        #endregion


                //        #region Epruveta_OnlineHeader



                //        if (onHeader.rbtnEpvOblikObradjena.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvOblikObradjena", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvOblikObradjena", "False");
                //        }

                //        if (onHeader.rbtnEpvOblikNeobradjena.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvOblikNeobradjena", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvOblikNeobradjena", "False");
                //        }



                //        if (onHeader.rbtnEpvTipProporcionalna.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvTipProporcionalna", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvTipProporcionalna", "False");
                //        }

                //        if (onHeader.rbtnEpvTipNeproporcionalna.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvTipNeproporcionalna", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvTipNeproporcionalna", "False");
                //        }



                //        if (onHeader.rbtnEpvTipProporcionalna.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvTipProporcionalna", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvTipProporcionalna", "False");
                //        }

                //        if (onHeader.rbtnEpvTipNeproporcionalna.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvTipNeproporcionalna", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvTipNeproporcionalna", "False");
                //        }



                //        if (onHeader.rbtnEpvK1.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvK1", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvK1", "False");
                //        }

                //        if (onHeader.rbtnEpvK2.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvK2", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvK2", "False");
                //        }



                //        //if (onHeader.rbtnEpvUzduzni.IsChecked == true)
                //        //{
                //        //    writer.WriteElementString("rbtnEpvUzduzni", "True");
                //        //}
                //        //else
                //        //{
                //        //    writer.WriteElementString("rbtnEpvUzduzni", "False");
                //        //}

                //        //if (onHeader.rbtnEpvPoprecni.IsChecked == true)
                //        //{
                //        //    writer.WriteElementString("rbtnEpvPoprecni", "True");
                //        //}
                //        //else
                //        //{
                //        //    writer.WriteElementString("rbtnEpvPoprecni", "False");
                //        //}



                //        if (onHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaPravougaona", "True");
                //            writer.WriteElementString("a", onHeader.tfAGlobal.Text);
                //            writer.WriteElementString("b", onHeader.tfBGlobal.Text);
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaPravougaona", "False");
                //        }

                //        if (onHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaKruzni", "True");
                //            writer.WriteElementString("D", onHeader.tfDGlobal.Text);
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaKruzni", "False");
                //        }

                //        if (onHeader.rbtnEpvVrstaCevasti.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaCevasti", "True");
                //            writer.WriteElementString("D", onHeader.tfDGlobal.Text);
                //            writer.WriteElementString("a", onHeader.tfAGlobal.Text);
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaCevasti", "False");
                //        }


                //        if (onHeader.rbtnEpvVrstaDeocev.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaDeocev", "True");
                //            writer.WriteElementString("D", onHeader.tfDGlobal.Text);
                //            writer.WriteElementString("a", onHeader.tfAGlobal.Text);
                //            writer.WriteElementString("b", onHeader.tfBGlobal.Text);
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaDeocev", "False");
                //        }


                //        if (onHeader.rbtnEpvVrstaSestaugaona.IsChecked == true)
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaSestaugaona", "True");
                //            writer.WriteElementString("d", onHeader.tfDGlobal.Text);
                //        }
                //        else
                //        {
                //            writer.WriteElementString("rbtnEpvVrstaSestaugaona", "False");
                //        }

                //        writer.WriteElementString("tfS0", onHeader.tfS0.Text);
                //        writer.WriteElementString("tfL0", onHeader.tfL0.Text);
                //        writer.WriteElementString("tfLc", onHeader.tfLc.Text);
                //        //TextRange textRange = new TextRange(onHeader.rtfNapomena.Document.ContentStart, onHeader.rtfNapomena.Document.ContentEnd);

                //        //string textRangeStr = textRange.Text;
                //        //textRangeStr = textRangeStr.Replace("\r\n",String.Empty);

                //        //writer.WriteElementString("rtfNapomena", textRangeStr.Text);

                //        #endregion


                //        #region PositionOfTube

                //        if (onHeader.PositionOfTube != null)
                //        {

                //            writer.WriteElementString("tfCustomPravacValjanja_PosiotionOfTube", onHeader.PositionOfTube.tfCustomPravacValjanja.Text);

                //            writer.WriteElementString("tfCustomSirinaTrake_PositionOfTube", onHeader.PositionOfTube.tfCustomSirinaTrake.Text);

                //            writer.WriteElementString("tfCustomDuzinaTrake_PositionOfTube", onHeader.PositionOfTube.tfCustomDuzinaTrake.Text);
                //        }

                //        #endregion

                //        #region Remarks

                if (onHeader.RemarkOfTesting != null)
                {

                    if (textRangeStr.Length <= Constants.MAXREMARKSTESTINGLENGTH)
                    {
                        //writer.WriteElementString("rtfNapomena_RemarkOfTesting", textRangeStr);
                    }
                    else
                    {
                        MessageBox.Show("Dužina napomene može biti najviše " + Constants.MAXREMARKSTESTINGLENGTH + " znakova.");
                    }
                }

                //        #endregion
                //    }

                //    writer.WriteEndElement();


                //    writer.WriteEndElement();
                //    writer.WriteEndDocument();
                //    writer.Close();
                //}
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void writeXMLLastOnlineHeader()}", System.DateTime.Now);
            }
         }

        /// <summary>
        /// sluzi za upisivanje izlaznih podataka zadnje pokidanog uzorka
        /// </summary>
        public void WriteXMLLastResultsInterface()
        {
            try
            {
                writeXMLLastResultsInterface();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void WriteXMLLastResultsInterface()}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// sluzi za upisivanje izlaznih podataka zadnje pokidanog uzorka
        /// </summary>
        private void writeXMLLastResultsInterface() 
        {
            try
            {
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
                if (resInterface == null)
                {
                    return;
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (window == null)
                {
                    return;
                }

                //ovo ne valja
                //ne upisuj u fajl kada ucitavas zadnje pokidani uzorak pri inicijalnom pokretanju aplikacije
                //if (window.FirstImeClicked == false)
                //{
                //    window.FirstImeClicked = true;  
                //    return;
                //}

                //write in xml file


                XElement xmlRoot = new XElement("ResultsInterface",
                                                new XElement("ResultsInterfaceLastWritten",
                                                            new XElement("tfLu", resInterface.tfLu.Text),
                                                            (resInterface.chbRp02.IsChecked == true) ? new XElement("chbRp02", "True") : new XElement("chbRp02", "False"),
                                                            (resInterface.rbtnRp02.IsChecked == true) ? new XElement("rbtnRp02", "True") : new XElement("rbtnRp02", "False"),
                                                            new XElement("tfRp02", resInterface.tfRp02.Text),
                                                            (resInterface.chbRt05.IsChecked == true) ? new XElement("chbRt05", "True") : new XElement("chbRt05", "False"),
                                                            (resInterface.rbtnRt05.IsChecked == true) ? new XElement("rbtnRt05", "True") : new XElement("rbtnRt05", "False"),
                                                            new XElement("tfRt05", resInterface.tfRt05.Text),
                                                            (resInterface.chbReL.IsChecked == true) ? new XElement("chbReL", "True") : new XElement("chbReL", "False"),
                                                            (resInterface.rbtnReL.IsChecked == true) ? new XElement("rbtnReL", "True") : new XElement("rbtnReL", "False"),
                                                            new XElement("tfReL", resInterface.tfReL.Text),
                                                            (resInterface.chbReH.IsChecked == true) ? new XElement("chbReH", "True") : new XElement("chbReH", "False"),
                                                            (resInterface.rbtnReH.IsChecked == true) ? new XElement("rbtnReH", "True") : new XElement("rbtnReH", "False"),
                                                            new XElement("tfReH", resInterface.tfReH.Text),
                                                            new XElement("tfRm", resInterface.tfRm.Text),
                                                            new XElement("tfF", resInterface.tfF.Text),
                                                            new XElement("tfFm", resInterface.tfFm.Text),
                                                            new XElement("tfAg", resInterface.tfAg.Text),
                                                            new XElement("tfAgt", resInterface.tfAgt.Text),
                                                            new XElement("tfRRm", resInterface.tfRRm.Text),
                                                            new XElement("tfA", resInterface.tfA.Text),
                                                            new XElement("tfAt", resInterface.tfAt.Text),
                                                            (window.Plotting.IsRectangle == true) ? new XElement("IsRectangle", "True") : new XElement("IsRectangle", "False"),
                                                            (window.Plotting.IsRectangle == true) ? new XElement("tfau", resInterface.au) : null,
                                                            (window.Plotting.IsRectangle == true) ? new XElement("tfbu", resInterface.bu) : null,
                                                            (window.Plotting.IsCircle == true) ? new XElement("IsCircle", "True") : new XElement("IsCircle", "False"),
                                                            (window.Plotting.IsCircle == true) ? new XElement("tfDu", resInterface.Du) : null,
                                                            new XElement("tfSu", resInterface.tfSu.Text),
                                                            new XElement("tfZ", resInterface.tfZ.Text),
                                                            (resInterface.chbn.IsChecked == true) ? new XElement("chbn", "True") : new XElement("chbn", "False"),
                                                            new XElement("tfn", resInterface.tfn.Text),
                                                            (resInterface.chbRmax.IsChecked == true) ? new XElement("chbRmax", "True") : new XElement("chbRmax", "False"),
                                                            new XElement("tfRmaxWithPoint", resInterface.tfRmax.Text),
                                                            (resInterface.chbe2.IsChecked == true) ? new XElement("chbe2", "True") : new XElement("chbe2", "False"),
                                                            (window.Plotting.E2MinValue != -1) ? new XElement("e2Min", window.Plotting.E2MinValue.ToString()) : new XElement("e2Min", String.Empty),
                                                            (window.Plotting.E2MaxValue != -1) ? new XElement("e2Max", window.Plotting.E2MaxValue.ToString()) : new XElement("e2Max", String.Empty),
                                                            (window.Plotting.E2AvgValue != -1) ? new XElement("e2Avg", window.Plotting.E2AvgValue.ToString()) : new XElement("e2Avg", String.Empty),
                                                            (resInterface.chbe4.IsChecked == true) ? new XElement("chbe4", "True") : new XElement("chbe4", "False"),
                                                            (window.Plotting.E4MinValue != -1) ? new XElement("e4Min", window.Plotting.E4MinValue.ToString()) : new XElement("e4Min", String.Empty),
                                                            (window.Plotting.E4MaxValue != -1) ? new XElement("e4Max", window.Plotting.E4MaxValue.ToString()) : new XElement("e4Max", String.Empty),
                                                            (window.Plotting.E4AvgValue != -1) ? new XElement("e4Avg", window.Plotting.E4AvgValue.ToString()) : new XElement("e4Avg", String.Empty),
                                                            (resInterface.chbRe.IsChecked == true) ? new XElement("chbRe", "True") : new XElement("chbRe", "False"),
                                                            (window.Plotting.Re != -1) ? new XElement("tfRe", window.Plotting.Re.ToString()) : new XElement("tfRe", String.Empty),
                                                            (resInterface.chbE.IsChecked == true) ? new XElement("chbE", "True") : new XElement("chbE", "False"),
                                                            (window.Plotting.YungsModuo != -1) ? new XElement("tfE", window.Plotting.YungsModuo.ToString()) : new XElement("tfE", String.Empty)

                                                            )
                                                );

                FileInfo file = new FileInfo(Constants.lastResultsInterfaceXml);
                if (IsFileinUse(file) == false)
                {

                    xmlRoot.Save(Constants.lastResultsInterfaceXml);

                }

                //using (XmlWriter writer = XmlWriter.Create(Constants.lastResultsInterfaceXml))
                //{




                //    writer.WriteStartDocument();
                //    writer.WriteStartElement("ResultsInterface");


                //    writer.WriteStartElement("ResultsInterfaceLastWritten");

                //    writer.WriteElementString("tfLu", resInterface.tfLu.Text);


                //if (resInterface.chbRp02.IsChecked == true)
                //{
                //    //writer.WriteElementString("chbRp02", "True");
                //    resInterface.rbtnRp02.IsEnabled = true;
                //}
                //else
                //{
                //    //writer.WriteElementString("chbRp02", "False");
                //    resInterface.rbtnRp02.IsEnabled = false;
                //}

                //    if (resInterface.rbtnRp02.IsChecked == true)
                //    {
                //        writer.WriteElementString("rbtnRp02", "True"); 
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnRp02", "False");  
                //    }



                //    writer.WriteElementString("tfRp02", resInterface.tfRp02.Text);

                //if (resInterface.chbRt05.IsChecked == true)
                //{
                //    //writer.WriteElementString("chbRt05", "True");
                //    resInterface.rbtnRt05.IsEnabled = true;
                //}
                //else
                //{
                //    //writer.WriteElementString("chbRt05", "False");
                //    resInterface.rbtnRt05.IsEnabled = false;
                //}

                //    if (resInterface.rbtnRt05.IsChecked == true)
                //    {
                //        writer.WriteElementString("rbtnRt05", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnRt05", "False");
                //    }


                //    writer.WriteElementString("tfRt05", resInterface.tfRt05.Text);

                //if (resInterface.chbReL.IsChecked == true)
                //{
                //    //writer.WriteElementString("chbReL", "True");
                //    resInterface.rbtnReL.IsEnabled = true;
                //}
                //else
                //{
                //    //writer.WriteElementString("chbReL", "False");
                //    resInterface.rbtnReL.IsEnabled = false;
                //}

                //    if (resInterface.rbtnReL.IsChecked == true)
                //    {
                //        writer.WriteElementString("rbtnReL", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnReL", "False");
                //    }


                //    writer.WriteElementString("tfReL", resInterface.tfReL.Text);


                //if (resInterface.chbReH.IsChecked == true)
                //{
                //    //writer.WriteElementString("chbReH", "True");
                //    resInterface.rbtnReH.IsEnabled = true;
                //}
                //else
                //{
                //    //writer.WriteElementString("chbReH", "False");
                //    resInterface.rbtnReH.IsEnabled = false;
                //}

                //    if (resInterface.rbtnReH.IsChecked == true)
                //    {
                //        writer.WriteElementString("rbtnReH", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("rbtnReH", "False");
                //    }


                //    writer.WriteElementString("tfReH", resInterface.tfReH.Text);

                //    writer.WriteElementString("tfRm", resInterface.tfRm.Text);
                //    writer.WriteElementString("tfF", resInterface.tfF.Text);
                //    writer.WriteElementString("tfFm", resInterface.tfFm.Text);
                //    writer.WriteElementString("tfAg", resInterface.tfAg.Text);
                //    writer.WriteElementString("tfAgt", resInterface.tfAgt.Text);
                //    writer.WriteElementString("tfRRm", resInterface.tfRRm.Text);
                //    writer.WriteElementString("tfA", resInterface.tfA.Text);
                //    writer.WriteElementString("tfAt", resInterface.tfAt.Text);
                //    if (window.Plotting.IsRectangle == true)
                //    {
                //        writer.WriteElementString("IsRectangle", "True");
                //        writer.WriteElementString("tfau", resInterface.au);
                //        writer.WriteElementString("tfbu", resInterface.bu);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("IsRectangle", "False");
                //    }
                //    if (window.Plotting.IsCircle == true)
                //    {
                //        writer.WriteElementString("IsCircle", "True");
                //        writer.WriteElementString("tfDu", resInterface.Du);
                //    }
                //    else
                //    {
                //        writer.WriteElementString("IsCircle", "False");
                //    }
                //    writer.WriteElementString("tfSu", resInterface.tfSu.Text);
                //    writer.WriteElementString("tfZ", resInterface.tfZ.Text);


                //    if (resInterface.chbn.IsChecked == true)
                //    {
                //        writer.WriteElementString("chbn", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("chbn", "False");
                //    }

                //    writer.WriteElementString("tfn", resInterface.tfn.Text);

                //    //write RmaxWithPoint
                //    if (resInterface.chbRmax.IsChecked == true)
                //    {
                //        writer.WriteElementString("chbRmax", "True");
                //    }
                //    else
                //    {
                //        writer.WriteElementString("chbRmax", "False");
                //    }
                //    writer.WriteElementString("tfRmaxWithPoint", resInterface.tfRmax.Text);


                //    //write last e2min, e2max, e4min and e4max
                //    //MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                //    if (window.Plotting != null)
                //    {
                //        if (resInterface.chbe2.IsChecked == true)
                //        {
                //            writer.WriteElementString("chbe2", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("chbe2", "False");
                //        }
                //        if (window.Plotting.E2MinValue != -1)
                //        {
                //            writer.WriteElementString("e2Min", window.Plotting.E2MinValue.ToString());
                //            writer.WriteElementString("e2Max", window.Plotting.E2MaxValue.ToString());
                //        }
                //        else
                //        {
                //            writer.WriteElementString("e2Min", String.Empty);
                //            writer.WriteElementString("e2Max", String.Empty);
                //        }

                //        if (resInterface.chbe4.IsChecked == true)
                //        {
                //            writer.WriteElementString("chbe4", "True");
                //        }
                //        else
                //        {
                //            writer.WriteElementString("chbe4", "False");
                //        }
                //        if (window.Plotting.E4MinValue != -1)
                //        {
                //            writer.WriteElementString("e4Min", window.Plotting.E4MinValue.ToString());
                //            writer.WriteElementString("e4Max", window.Plotting.E4MaxValue.ToString());
                //        }
                //        else
                //        {
                //            writer.WriteElementString("e4Min", String.Empty);
                //            writer.WriteElementString("e4Max", String.Empty);
                //        }
                //    }



                //    writer.WriteEndElement();


                //    writer.WriteEndElement();
                //    writer.WriteEndDocument();
                //    writer.Close();
                //}
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void writeXMLLastResultsInterface()}", System.DateTime.Now);
            }
        }

      
        /// <summary>
        /// cuvanje informacije o tome koje su se opcije koristile u online modu
        /// pa se pri pokretanju animacije tog uzorka ucitavaju u readonly polja opcija animacije
        /// </summary>
        //private void writeSettingsInAnimationFile() 
        //{
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_L0 + onHeader.tfL0.Text);
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_S0 + onHeader.tfS0.Text);

        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_nutnDivide + optionsOnline.tfCalForceDivide.Text);
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_nutnMultiple + optionsOnline.tfCalForceMultiple.Text);
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_mmDivide + optionsOnline.tfCalElonDivide.Text);
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_mmCoeff + optionsOnline.tfCalElonMultiple.Text);

        //    if (OptionsInOnlineMode.isManualChecked)
        //    {
        //        animationLines.Add(Constants.ANIMATIONFILEHEADER_razmeraPreassure + optionsOnline.tfRatioForce.Text);
        //        animationLines.Add(Constants.ANIMATIONFILEHEADER_razmeraElongation + optionsOnline.tfRatioElongation.Text);
        //    }
        //    if (OptionsInOnlineMode.isAutoChecked)
        //    {
        //        animationLines.Add("isAutoChecked : True");
        //        animationLines.Add(Constants.ANIMATIONFILEHEADER_razmeraPreassure + "0.95");
        //        animationLines.Add(Constants.ANIMATIONFILEHEADER_razmeraElongation + "0.95");
        //    }
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_refreshAnimationTime);
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure);
        //    animationLines.Add(Constants.ANIMATIONFILEHEADER_resolution);
        //}

        /// <summary>
        /// da li moze da se pristupi online fajlu, da bise procitao zadnje upisani podatak
        /// </summary>
        /// <param name="file">objekat tipa FileInfo, koji cuva informacije o online fajlu</param>
        /// <returns></returns>
        protected virtual bool IsFileinUse(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }


        /// <summary>
        /// cuvaju se u posebnim fajlovima informacije o zadnje pokidanom uzorku
        /// tj online.txt,  animation.anim, e2e4Online.e2e4 pamti u istom direktorijumu kao nezapamceno.txt, nezapamceno.anim, nezapamceno.e2e4
        /// </summary>
        private void saveUnsavedFile() 
        {
            try
            {
                // Get file name.
                //string name = Constants.unsavedFilepath;
                string name = Properties.Settings.Default.unsavedFilepath;
                List<string> dataList = new List<string>();

                //FileInfo fileinfo = new FileInfo(Constants.onlineFilepath);
                FileInfo fileinfo = new FileInfo(Properties.Settings.Default.onlineFilepath);
                bool isInUse = true;
                while (isInUse == true)
                {
                    isInUse = IsFileinUse(fileinfo);
                    if (isInUse == false)
                    {
                        isInUse = IsFileinUse(fileinfo);
                        if (isInUse == false)
                        {
                            //dataList = File.ReadAllLines(Constants.onlineFilepath).ToList();
                            dataList = File.ReadAllLines(Properties.Settings.Default.onlineFilepath).ToList();
                        }
                        else
                        {
                            return;
                        }
                        // Write to the file name selected.
                        // ... You can write the text from a TextBox instead of a string literal.
                        File.WriteAllLines(name, dataList);
                        break;
                    }
                }


                //GetAutomaticAnimation file name
                //string nameAnimation = name.Split('.').ElementAt(0);
                //nameAnimation += ".anim";
                //List<string> dataListAnimation = new List<string>();
                //dataListAnimation = File.ReadAllLines(Constants.animationFilepath).ToList();
                //// Write to the file name selected.
                //// ... You can write the text from a TextBox instead of a string literal.
                //if (File.Exists(nameAnimation) == true)
                //{
                //    File.Delete(nameAnimation);
                //}
                //File.WriteAllLines(nameAnimation, dataListAnimation);


                //GetAutomaticChangedParameters file name
                string nameChangedParameters = name.Split('.').ElementAt(0);
                nameChangedParameters += ".e2e4";
                List<string> dataListChangedParameters = new List<string>();
                //dataListChangedParameters = File.ReadAllLines(Constants.e2e4Filepath).ToList();
                dataListChangedParameters = File.ReadAllLines(Properties.Settings.Default.e2e4Filepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                if (File.Exists(nameChangedParameters) == true)
                {
                    File.Delete(nameChangedParameters);
                }
                File.WriteAllLines(nameChangedParameters, dataListChangedParameters);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void saveUnsavedFile()}", System.DateTime.Now);
            }

        }


        private void saveUnsavedPreassureElongation(ref List<double> preassure, ref List<double> elongation) 
        {
            try
            {
                if (preassure == null || elongation == null)
                {
                    return;
                }
                string currData = string.Empty;
                List<string> datas = new List<string>();
                for (int i = 0; i < preassure.Count; i++)
                {
                    if (i >= preassure.Count || i >= elongation.Count)
                    {
                        break;
                    }
                    currData = preassure[i] + "\t" + elongation[i];
                    datas.Add(currData);
                }
                string nameInputOutputPE = Properties.Settings.Default.unsavedFilepathPreassureElongation;

                if (File.Exists(nameInputOutputPE) == true)
                {
                    File.Delete(nameInputOutputPE);
                }
                File.WriteAllLines(nameInputOutputPE, datas);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void saveUnsavedPreassureElongation(ref List<double> preassure, ref List<double> elongation)}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// cuvaju se u posebnim fajlovima informacije o zadnje pokidanom uzorku
        /// tj ulazIzlazOnline.inputoutput pamti u istom direktorijumu kao nezapamceno.inputoutput
        /// </summary>
        private void saveUnsavedInputOutput() 
        {
            try
            {
                // Get file name.
                //string name = Constants.unsavedFilepath;
                string name = Properties.Settings.Default.unsavedFilepath;


                //GetAutomaticAnimation file name
                string nameInputOutput = name.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";
                List<string> dataListInputOutput = new List<string>();
                //dataListInputOutput = File.ReadAllLines(Constants.inputOutputFilepath).ToList();
                dataListInputOutput = File.ReadAllLines(Properties.Settings.Default.inputOutputFilepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                if (File.Exists(nameInputOutput) == true)
                {
                    File.Delete(nameInputOutput);
                }
                File.WriteAllLines(nameInputOutput, dataListInputOutput);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void saveUnsavedInputOutput()}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// upisuje informacije na osnovu kojih ce da se napravi pojedinacan izvestaj zadnje pokidanog uzorka
        /// </summary>
        public void WriteSampleReportOnlineXml()
        {
            try
            {
                writeSampleReportOnlineXml();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void WriteSampleReportOnlineXml()}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// upisuje informacije na osnovu kojih ce da se napravi pojedinacan izvestaj zadnje pokidanog uzorka
        /// </summary>
        private void writeSampleReportOnlineXml() 
        {
            try
            {
                if (this.Plotting.Printscreen.plotterPrint == null)
                {
                    return;
                }


                //save the graphic

                //sakrij vidljivost legende grafika
                this.Plotting.Printscreen.plotterPrint.LegendVisible = false;
                //sakrij markere
                this.Plotting.Printscreen.HidePrintScreenMarkers();
                if (double.IsNaN(this.Plotting.Printscreen.plotterPrint.Width) == false && double.IsNaN(this.Plotting.Printscreen.plotterPrint.Height) == false)
                {
                    //this.Plotting.Printscreen.plotterPrint.SaveScreenshot(Constants.sampleReportGraphicFilepath);
                    this.Plotting.Printscreen.plotterPrint.SaveScreenshot(Properties.Settings.Default.sampleReportGraphicFilepath);
                }

                //ako postoje grafici za promenu napona i izduzenja i njih zapamti
                if (chbStartSampleShowChangedPar.IsChecked == true)
                {
                    if (this.VXY != null)
                    {
                        if (this.VXY.plotterChangeOfR != null)
                        {
                            //this.VXY.plotterChangeOfR.SaveScreenshot(Constants.sampleReportGraphicFilepathChangeOfR);
                            this.VXY.plotterChangeOfR.SaveScreenshot(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR);
                        }
                        if (this.VXY.plotterChangeOfE != null)
                        {
                            //this.VXY.plotterChangeOfE.SaveScreenshot(Constants.sampleReportGraphicFilepathChangeOfE);
                            this.VXY.plotterChangeOfE.SaveScreenshot(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE);
                        }
                    }
                }

                //otkrij markere
                this.Plotting.Printscreen.ShowPrintScreenMarkers();
                //otkrij vidljivost legende grafika
                this.Plotting.Printscreen.plotterPrint.LegendVisible = true;




                //write xml file for sample report
                //if (File.Exists(Constants.sampleReportFilepath) == false)
                if (File.Exists(Properties.Settings.Default.sampleReportFilepath) == false)
                {
                    //File.Delete(Constants.sampleReportFilepath);
                    File.Delete(Properties.Settings.Default.sampleReportFilepath);
                }
                if (plotting.PreassureForNManualProperty != null && plotting.PreassureForNManualProperty.Count == 5)
                {
                    //write in xml file
                    XElement xmlRoot = new XElement(Constants.XML_roots_ROOT,
                                                new XElement(Constants.XML_roots_Sadrzaj,
                        //GeneralData
                                                                        new XElement(Constants.XML_GeneralData_OPERATOR, LastInputOutputSavedData.tfOperator_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_BRZBIZVESTAJA, LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_BRUZORKA, LastInputOutputSavedData.tfBrUzorka_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_SARZA, LastInputOutputSavedData.tfSarza_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_RADNINALOG, LastInputOutputSavedData.tfRadniNalog_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_NARUCILAC, LastInputOutputSavedData.tfNarucilac_GeneralData),
                        //ConditionsOfTesting
                                                                        new XElement(Constants.XML_ConditionsOfTesting_STANDARD, LastInputOutputSavedData.tfStandard_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_METODA, LastInputOutputSavedData.tfMetoda_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_STANDARDZAN, LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_MASINA, LastInputOutputSavedData.tfMasina_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_OPSEGMASINE, LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_TEMPERATURA, LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting),
                                                                        LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True") ? new XElement(Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR, Constants.DA) : null,
                                                                        LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True") ? new XElement(Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR, Constants.NE) : null,
                        //MaterialForTesting
                                                                        new XElement(Constants.XML_MaterialForTesting_PROIZVODJAC, LastInputOutputSavedData.tfProizvodjac_MaterialForTesting),
                                                                        new XElement(Constants.XML_MaterialForTesting_DOBAVLJAC, LastInputOutputSavedData.tfDobavljac_MaterialForTesting),
                                                                        new XElement(Constants.XML_MaterialForTesting_POLAZNIKVALITET, LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting),
                                                                        new XElement(Constants.XML_MaterialForTesting_NAZIVNADEBLJINA, LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting),
                                                                        LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.VALJANI) : null,
                                                                        LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.VUČENI) : null,
                                                                        LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.KOVANI) : null,
                                                                        LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.LIVENI) : null,
                        //Epruveta_OnlineHeader
                                                                        LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True") ? new XElement(Constants.XML_Epruveta_EPRUVETAOBLIK, Constants.OBRADJENA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True") ? new XElement(Constants.XML_Epruveta_EPRUVETAOBLIK, Constants.NEOBRADJENA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True") ? new XElement(Constants.XML_Epruveta_TIP, Constants.PROPORCIONALNA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True") ? new XElement(Constants.XML_Epruveta_TIP, Constants.NEPROPORCIONALNA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvK1.Equals("True") ? new XElement(Constants.XML_Epruveta_K, Constants.K1) : null,
                                                                        LastInputOutputSavedData.rbtnEpvK2.Equals("True") ? new XElement(Constants.XML_Epruveta_K, Constants.K2) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.PRAVOUGAONA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.a0, LastInputOutputSavedData.a0Pravougaona) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.b0, LastInputOutputSavedData.b0Pravougaona) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.KRUZNA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement(Constants.D0, LastInputOutputSavedData.D0Kruzna) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.CEVASTA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement(Constants.D0, LastInputOutputSavedData.D0Cevasta) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement(Constants.a0, LastInputOutputSavedData.a0Cevasta) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.DEOCEVI) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.D0, LastInputOutputSavedData.D0Deocev) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.a0, LastInputOutputSavedData.a0Deocev) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.b0, LastInputOutputSavedData.b0Deocev) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.SESTAUGAONA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True") ? new XElement(Constants.d0, LastInputOutputSavedData.d0Sestaugaona) : null,
                                                                        new XElement(Constants.XML_Epruveta_S0, LastInputOutputSavedData.tfS0),
                                                                        new XElement(Constants.XML_Epruveta_L0, LastInputOutputSavedData.tfL0),
                                                                        new XElement(Constants.XML_Epruveta_LC, LastInputOutputSavedData.tfLc),
                        //PositionOfTube
                                                                        new XElement(Constants.XML_PositionOfTube_PRAVACVALJANJA, LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube),
                                                                        new XElement(Constants.XML_PositionOfTube_SIRINATRAKE, LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube),
                                                                        new XElement(Constants.XML_PositionOfTube_DUZINATRAKE, LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube),
                        //Remarks
                                                                        LastInputOutputSavedData.rtfNapomena_RemarkOfTesting.Length <= Constants.MAXREMARKSTESTINGLENGTH ? new XElement(Constants.XML_RemarkOfTesting_NAPOMENA, LastInputOutputSavedData.rtfNapomena_RemarkOfTesting) : new XElement(Constants.XML_RemarkOfTesting_NAPOMENA, String.Empty),
                        //ResultInterface
                                                                        new XElement(Constants.XML_ResultsInterface_Lu, LastInputOutputSavedData.tfLu_ResultsInterface),
                                                                        LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Rp02, LastInputOutputSavedData.tfRp02_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_Rp02, String.Empty),
                                                                        LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Rt05, LastInputOutputSavedData.tfRt05_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_Rt05, String.Empty),
                                                                        LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_ReL, LastInputOutputSavedData.tfReL_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_ReL, String.Empty),
                                                                        LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_ReH, LastInputOutputSavedData.tfReH_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_ReH, String.Empty),
                                                                        new XElement(Constants.XML_ResultsInterface_Rm, LastInputOutputSavedData.tfRm_ResultsInterface),
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rp02 ? new XElement(Constants.XML_ResultsInterface_Rp02_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rt05 ? new XElement(Constants.XML_ResultsInterface_Rt05_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReL ? new XElement(Constants.XML_ResultsInterface_ReL_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReH ? new XElement(Constants.XML_ResultsInterface_ReH_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        new XElement(Constants.XML_ResultsInterface_F, LastInputOutputSavedData.tfF_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Fm, LastInputOutputSavedData.tfFm_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Ag, LastInputOutputSavedData.tfAg_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Agt, LastInputOutputSavedData.tfAgt_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_A, LastInputOutputSavedData.tfA_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_At, LastInputOutputSavedData.tfAt_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Su, LastInputOutputSavedData.tfSu_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Z, LastInputOutputSavedData.tfZ_ResultsInterface),
                                                                        LastInputOutputSavedData.chbn_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_n, LastInputOutputSavedData.tfn_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_n, String.Empty),
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.au, LastInputOutputSavedData.au) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.bu, LastInputOutputSavedData.bu) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement(Constants.Du, LastInputOutputSavedData.Du) : null,
                                                                        LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Rmax, LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_Rmax, String.Empty),
                                                                        LastInputOutputSavedData.isE2E4BorderSelected.Equals("True") ? new XElement(Constants.XML_isE2E4BorderSelected, "True") : new XElement(Constants.XML_isE2E4BorderSelected, "False"),
                                                                        LastInputOutputSavedData.chbe2_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_eR2, LastInputOutputSavedData.e2Min_ResultsInterface + "-" + LastInputOutputSavedData.e2Max_ResultsInterface + "%") : new XElement(Constants.XML_ResultsInterface_eR2, String.Empty),
                                                                        LastInputOutputSavedData.chbe4_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_eR4, LastInputOutputSavedData.e4Min_ResultsInterface + "-" + LastInputOutputSavedData.e4Max_ResultsInterface + "%") : new XElement(Constants.XML_ResultsInterface_eR4, String.Empty),
                                                                        LastInputOutputSavedData.chbRe_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Re, LastInputOutputSavedData.Re_ResultsInterface + "MPa") : new XElement(Constants.XML_ResultsInterface_Re, String.Empty),
                                                                        LastInputOutputSavedData.chbE_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_E, LastInputOutputSavedData.E_ResultsInterface + "GPa") : new XElement(Constants.XML_ResultsInterface_E, String.Empty),
                        //other stuff
                                                                        new XElement(Constants.XML_ShowGraphicChangeOfRAndE, plotting.Printscreen.chbChangeOfRAndE.IsChecked == true ? "True" : "False"),
                                                                        new XElement(Constants.XML_ManualnIsCalculated, plotting.Printscreen.chbCalculateNManual.IsChecked == true ? "True" : "False"),
                                                                        new XElement(Constants.XML_R1, plotting.PreassureForNManualProperty[0].ToString()),
                                                                        new XElement(Constants.XML_R2, plotting.PreassureForNManualProperty[1].ToString()),
                                                                        new XElement(Constants.XML_R3, plotting.PreassureForNManualProperty[2].ToString()),
                                                                        new XElement(Constants.XML_R4, plotting.PreassureForNManualProperty[3].ToString()),
                                                                        new XElement(Constants.XML_R5, plotting.PreassureForNManualProperty[4].ToString()),
                                                                        new XElement(Constants.XML_manualN, plotting.NManual.ToString()),
                                                                        new XElement(Constants.XML_manualN_BeginInterval, OptionsInPlottingMode.BeginIntervalForN),
                                                                        new XElement(Constants.XML_manualN_EndInterval, OptionsInPlottingMode.EndIntervalForN)
                                                            )
                                                    );

                    //xmlRoot.Save(Constants.sampleReportFilepath);
                    xmlRoot.Save(Properties.Settings.Default.sampleReportFilepath);
                }
                else
                {
                    //write in xml file
                    XElement xmlRoot = new XElement(Constants.XML_roots_ROOT,
                                                new XElement(Constants.XML_roots_Sadrzaj,
                        //GeneralData
                                                                        new XElement(Constants.XML_GeneralData_OPERATOR, LastInputOutputSavedData.tfOperator_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_BRZBIZVESTAJA, LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_BRUZORKA, LastInputOutputSavedData.tfBrUzorka_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_SARZA, LastInputOutputSavedData.tfSarza_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_RADNINALOG, LastInputOutputSavedData.tfRadniNalog_GeneralData),
                                                                        new XElement(Constants.XML_GeneralData_NARUCILAC, LastInputOutputSavedData.tfNarucilac_GeneralData),
                        //ConditionsOfTesting
                                                                        new XElement(Constants.XML_ConditionsOfTesting_STANDARD, LastInputOutputSavedData.tfStandard_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_METODA, LastInputOutputSavedData.tfMetoda_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_STANDARDZAN, LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_MASINA, LastInputOutputSavedData.tfMasina_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_OPSEGMASINE, LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting),
                                                                        new XElement(Constants.XML_ConditionsOfTesting_TEMPERATURA, LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting),
                                                                        LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True") ? new XElement(Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR, Constants.DA) : null,
                                                                        LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True") ? new XElement(Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR, Constants.NE) : null,
                        //MaterialForTesting
                                                                        new XElement(Constants.XML_MaterialForTesting_PROIZVODJAC, LastInputOutputSavedData.tfProizvodjac_MaterialForTesting),
                                                                        new XElement(Constants.XML_MaterialForTesting_DOBAVLJAC, LastInputOutputSavedData.tfDobavljac_MaterialForTesting),
                                                                        new XElement(Constants.XML_MaterialForTesting_POLAZNIKVALITET, LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting),
                                                                        new XElement(Constants.XML_MaterialForTesting_NAZIVNADEBLJINA, LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting),
                                                                        LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.VALJANI) : null,
                                                                        LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.VUČENI) : null,
                                                                        LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.KOVANI) : null,
                                                                        LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True") ? new XElement(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.LIVENI) : null,
                        //Epruveta_OnlineHeader
                                                                        LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True") ? new XElement(Constants.XML_Epruveta_EPRUVETAOBLIK, Constants.OBRADJENA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True") ? new XElement(Constants.XML_Epruveta_EPRUVETAOBLIK, Constants.NEOBRADJENA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True") ? new XElement(Constants.XML_Epruveta_TIP, Constants.PROPORCIONALNA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True") ? new XElement(Constants.XML_Epruveta_TIP, Constants.NEPROPORCIONALNA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvK1.Equals("True") ? new XElement(Constants.XML_Epruveta_K, Constants.K1) : null,
                                                                        LastInputOutputSavedData.rbtnEpvK2.Equals("True") ? new XElement(Constants.XML_Epruveta_K, Constants.K2) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.PRAVOUGAONA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.a0, LastInputOutputSavedData.a0Pravougaona) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.b0, LastInputOutputSavedData.b0Pravougaona) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.KRUZNA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement(Constants.D0, LastInputOutputSavedData.D0Kruzna) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.CEVASTA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement(Constants.D0, LastInputOutputSavedData.D0Cevasta) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") ? new XElement(Constants.a0, LastInputOutputSavedData.a0Cevasta) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.DEOCEVI) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.D0, LastInputOutputSavedData.D0Deocev) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.a0, LastInputOutputSavedData.a0Deocev) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") ? new XElement(Constants.b0, LastInputOutputSavedData.b0Deocev) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True") ? new XElement(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.SESTAUGAONA) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True") ? new XElement(Constants.d0, LastInputOutputSavedData.d0Sestaugaona) : null,
                                                                        new XElement(Constants.XML_Epruveta_S0, LastInputOutputSavedData.tfS0),
                                                                        new XElement(Constants.XML_Epruveta_L0, LastInputOutputSavedData.tfL0),
                                                                        new XElement(Constants.XML_Epruveta_LC, LastInputOutputSavedData.tfLc),
                        //PositionOfTube
                                                                        new XElement(Constants.XML_PositionOfTube_PRAVACVALJANJA, LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube),
                                                                        new XElement(Constants.XML_PositionOfTube_SIRINATRAKE, LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube),
                                                                        new XElement(Constants.XML_PositionOfTube_DUZINATRAKE, LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube),
                        //Remarks
                                                                        LastInputOutputSavedData.rtfNapomena_RemarkOfTesting.Length <= Constants.MAXREMARKSTESTINGLENGTH ? new XElement(Constants.XML_RemarkOfTesting_NAPOMENA, LastInputOutputSavedData.rtfNapomena_RemarkOfTesting) : new XElement(Constants.XML_RemarkOfTesting_NAPOMENA, String.Empty),
                        //ResultInterface
                                                                        new XElement(Constants.XML_ResultsInterface_Lu, LastInputOutputSavedData.tfLu_ResultsInterface),
                                                                        LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Rp02, LastInputOutputSavedData.tfRp02_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_Rp02, String.Empty),
                                                                        LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Rt05, LastInputOutputSavedData.tfRt05_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_Rt05, String.Empty),
                                                                        LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_ReL, LastInputOutputSavedData.tfReL_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_ReL, String.Empty),
                                                                        LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_ReH, LastInputOutputSavedData.tfReH_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_ReH, String.Empty),
                                                                        new XElement(Constants.XML_ResultsInterface_Rm, LastInputOutputSavedData.tfRm_ResultsInterface),
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rp02 ? new XElement(Constants.XML_ResultsInterface_Rp02_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rt05 ? new XElement(Constants.XML_ResultsInterface_Rt05_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReL ? new XElement(Constants.XML_ResultsInterface_ReL_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        ResultsInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReH ? new XElement(Constants.XML_ResultsInterface_ReH_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface) : null,
                                                                        new XElement(Constants.XML_ResultsInterface_F, LastInputOutputSavedData.tfF_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Fm, LastInputOutputSavedData.tfFm_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Ag, LastInputOutputSavedData.tfAg_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Agt, LastInputOutputSavedData.tfAgt_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_A, LastInputOutputSavedData.tfA_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_At, LastInputOutputSavedData.tfAt_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Su, LastInputOutputSavedData.tfSu_ResultsInterface),
                                                                        new XElement(Constants.XML_ResultsInterface_Z, LastInputOutputSavedData.tfZ_ResultsInterface),
                                                                        LastInputOutputSavedData.chbn_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_n, LastInputOutputSavedData.tfn_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_n, String.Empty),
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.au, LastInputOutputSavedData.au) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") ? new XElement(Constants.bu, LastInputOutputSavedData.bu) : null,
                                                                        LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") ? new XElement(Constants.Du, LastInputOutputSavedData.Du) : null,
                                                                        LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Rmax, LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface) : new XElement(Constants.XML_ResultsInterface_Rmax, String.Empty),
                                                                        LastInputOutputSavedData.isE2E4BorderSelected.Equals("True") ? new XElement(Constants.XML_isE2E4BorderSelected, "True") : new XElement(Constants.XML_isE2E4BorderSelected, "False"),
                                                                        LastInputOutputSavedData.chbe2_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_eR2, LastInputOutputSavedData.e2Min_ResultsInterface + "-" + LastInputOutputSavedData.e2Max_ResultsInterface + "%") : new XElement(Constants.XML_ResultsInterface_eR2, String.Empty),
                                                                        LastInputOutputSavedData.chbe4_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_eR4, LastInputOutputSavedData.e4Min_ResultsInterface + "-" + LastInputOutputSavedData.e4Max_ResultsInterface + "%") : new XElement(Constants.XML_ResultsInterface_eR4, String.Empty),
                                                                        LastInputOutputSavedData.chbRe_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_Re, LastInputOutputSavedData.Re_ResultsInterface + "MPa") : new XElement(Constants.XML_ResultsInterface_Re, String.Empty),
                                                                        LastInputOutputSavedData.chbE_ResultsInterface.Equals("True") ? new XElement(Constants.XML_ResultsInterface_E, LastInputOutputSavedData.E_ResultsInterface + "GPa") : new XElement(Constants.XML_ResultsInterface_E, String.Empty),
                        //other stuff
                                                                        new XElement(Constants.XML_ShowGraphicChangeOfRAndE, plotting.Printscreen.chbChangeOfRAndE.IsChecked == true ? "True" : "False"),
                                                                        new XElement(Constants.XML_ManualnIsCalculated, plotting.Printscreen.chbCalculateNManual.IsChecked == true ? "True" : "False"),
                                                                        new XElement(Constants.XML_R1, string.Empty),
                                                                        new XElement(Constants.XML_R2, string.Empty),
                                                                        new XElement(Constants.XML_R3, string.Empty),
                                                                        new XElement(Constants.XML_R4, string.Empty),
                                                                        new XElement(Constants.XML_R5, string.Empty),
                                                                        new XElement(Constants.XML_manualN, plotting.NManual.ToString()),
                                                                        new XElement(Constants.XML_manualN_BeginInterval, OptionsInPlottingMode.BeginIntervalForN),
                                                                        new XElement(Constants.XML_manualN_EndInterval, OptionsInPlottingMode.EndIntervalForN)
                                                            )
                                                    );

                    //xmlRoot.Save(Constants.sampleReportFilepath);
                    xmlRoot.Save(Properties.Settings.Default.sampleReportFilepath);
                }
                //    using (XmlWriter writer = XmlWriter.Create(Constants.sampleReportFilepath))
                //    {
                //        writer.WriteStartDocument();
                //        writer.WriteStartElement(Constants.XML_roots_ROOT);


                //        writer.WriteStartElement(Constants.XML_roots_Sadrzaj);



                //#region GeneralData

                //writer.WriteElementString(Constants.XML_GeneralData_OPERATOR, LastInputOutputSavedData.tfOperator_GeneralData);
                //writer.WriteElementString(Constants.XML_GeneralData_BRZBIZVESTAJA, LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData);
                //writer.WriteElementString(Constants.XML_GeneralData_BRUZORKA, LastInputOutputSavedData.tfBrUzorka_GeneralData);
                //writer.WriteElementString(Constants.XML_GeneralData_SARZA, LastInputOutputSavedData.tfSarza_GeneralData);
                //writer.WriteElementString(Constants.XML_GeneralData_RADNINALOG, LastInputOutputSavedData.tfRadniNalog_GeneralData);
                //writer.WriteElementString(Constants.XML_GeneralData_NARUCILAC, LastInputOutputSavedData.tfNarucilac_GeneralData);

                //#endregion

                //#region ConditionsOfTesting

                //writer.WriteElementString(Constants.XML_ConditionsOfTesting_STANDARD, LastInputOutputSavedData.tfStandard_ConditionsOfTesting);
                //writer.WriteElementString(Constants.XML_ConditionsOfTesting_METODA, LastInputOutputSavedData.tfMetoda_ConditionsOfTesting);
                //writer.WriteElementString(Constants.XML_ConditionsOfTesting_STANDARDZAN, LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting);
                //writer.WriteElementString(Constants.XML_ConditionsOfTesting_MASINA, LastInputOutputSavedData.tfMasina_ConditionsOfTesting);
                //writer.WriteElementString(Constants.XML_ConditionsOfTesting_OPSEGMASINE, LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting);
                ////writer.WriteElementString("tfEndOpsegMasine_ConditionsOfTesting", onHeader.ConditionsOfTesting.tfEndOpsegMasine.Text);
                //writer.WriteElementString(Constants.XML_ConditionsOfTesting_TEMPERATURA, LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting);

                //if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR, Constants.DA);
                //}


                //if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ConditionsOfTesting_EKSTENZIOMETAR, Constants.NE);
                //}


                //#endregion

                //#region MaterialForTesting

                //writer.WriteElementString(Constants.XML_MaterialForTesting_PROIZVODJAC, LastInputOutputSavedData.tfProizvodjac_MaterialForTesting);
                //writer.WriteElementString(Constants.XML_MaterialForTesting_DOBAVLJAC, LastInputOutputSavedData.tfDobavljac_MaterialForTesting);
                //writer.WriteElementString(Constants.XML_MaterialForTesting_POLAZNIKVALITET, LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting);
                //writer.WriteElementString(Constants.XML_MaterialForTesting_NAZIVNADEBLJINA, LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting);

                //if (LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.VALJANI);
                //}

                //if (LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.VUČENI);
                //}

                //if (LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.KOVANI);
                //}

                //if (LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_MaterialForTesting_NACINPRERADE, Constants.LIVENI);
                //}


                //#endregion


                //#region Epruveta_OnlineHeader

                //if (LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_Epruveta_EPRUVETAOBLIK, Constants.OBRADJENA);
                //}
                //if (LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_Epruveta_EPRUVETAOBLIK, Constants.NEOBRADJENA);
                //}    



                //if (LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_Epruveta_TIP, Constants.PROPORCIONALNA);
                //}
                //if (LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_Epruveta_TIP, Constants.NEPROPORCIONALNA);
                //}



                //if (LastInputOutputSavedData.rbtnEpvK1.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_Epruveta_K, Constants.K1);
                //}
                //if (LastInputOutputSavedData.rbtnEpvK2.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_Epruveta_K, Constants.K2);
                //}



                //if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                //{
                //writer.WriteElementString(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.PRAVOUGAONA);
                //writer.WriteElementString(Constants.a0, LastInputOutputSavedData.a0Pravougaona);
                //writer.WriteElementString(Constants.b0, LastInputOutputSavedData.b0Pravougaona);
                //}
                //if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                //{
                //writer.WriteElementString(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.KRUZNA);
                //writer.WriteElementString(Constants.D0, LastInputOutputSavedData.D0Kruzna);
                //}
                //if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                //{
                //writer.WriteElementString(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.CEVASTA);
                //writer.WriteElementString(Constants.D0, LastInputOutputSavedData.D0Cevasta);
                //writer.WriteElementString(Constants.a0, LastInputOutputSavedData.a0Cevasta);
                //}
                //if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                //{
                //writer.WriteElementString(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.DEOCEVI);
                //writer.WriteElementString(Constants.D0, LastInputOutputSavedData.D0Deocev);
                //writer.WriteElementString(Constants.a0, LastInputOutputSavedData.a0Deocev);
                //writer.WriteElementString(Constants.b0, LastInputOutputSavedData.b0Deocev);
                //}
                //if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
                //{
                //writer.WriteElementString(Constants.XML_Epruveta_VRSTAEPRUVETE, Constants.SESTAUGAONA);
                //writer.WriteElementString(Constants.d0, LastInputOutputSavedData.d0Sestaugaona);
                //}

                //writer.WriteElementString(Constants.XML_Epruveta_S0, LastInputOutputSavedData.tfS0);
                //writer.WriteElementString(Constants.XML_Epruveta_L0, LastInputOutputSavedData.tfL0);
                //writer.WriteElementString(Constants.XML_Epruveta_LC, LastInputOutputSavedData.tfLc);

                //#endregion


                //#region PositionOfTube

                //writer.WriteElementString(Constants.XML_PositionOfTube_PRAVACVALJANJA, LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube);
                //writer.WriteElementString(Constants.XML_PositionOfTube_SIRINATRAKE, LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube);
                //writer.WriteElementString(Constants.XML_PositionOfTube_DUZINATRAKE, LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube);

                //#endregion

                //#region Remarks

                //if (LastInputOutputSavedData.rtfNapomena_RemarkOfTesting.Length <= Constants.MAXREMARKSTESTINGLENGTH)
                //{
                //    writer.WriteElementString(Constants.XML_RemarkOfTesting_NAPOMENA, LastInputOutputSavedData.rtfNapomena_RemarkOfTesting);
                //}
                //else
                //{
                //    MessageBox.Show("Dužina napomene može biti najviše " + Constants.MAXREMARKSTESTINGLENGTH + " znakova.");
                //}

                //#endregion


                //#region ResultInterface

                //writer.WriteElementString(Constants.XML_ResultsInterface_Lu, LastInputOutputSavedData.tfLu_ResultsInterface);
                //if (LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rp02, LastInputOutputSavedData.tfRp02_ResultsInterface);
                //}
                //else 
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rp02, String.Empty);
                //}
                //if (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rt05, LastInputOutputSavedData.tfRt05_ResultsInterface);
                //}
                //else 
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rt05, String.Empty);
                //}
                //if (LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_ReL, LastInputOutputSavedData.tfReL_ResultsInterface);
                //}
                //else
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_ReL, String.Empty);
                //}
                //if (LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_ReH, LastInputOutputSavedData.tfReH_ResultsInterface);
                //}
                //else
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_ReH, String.Empty);
                //}

                //writer.WriteElementString(Constants.XML_ResultsInterface_Rm, LastInputOutputSavedData.tfRm_ResultsInterface);
                ////if (LastInputOutputSavedData.rbtnRp02_ResultsInterface.Equals("True"))
                ////{
                ////    writer.WriteElementString(Constants.XML_ResultsInterface_Rp02_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                ////}
                ////if (LastInputOutputSavedData.rbtnRt05_ResultsInterface.Equals("True"))
                ////{
                ////    writer.WriteElementString(Constants.XML_ResultsInterface_Rt05_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                ////}
                ////if (LastInputOutputSavedData.rbtnReL_ResultsInterface.Equals("True"))
                ////{
                ////    writer.WriteElementString(Constants.XML_ResultsInterface_ReL_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                ////}
                ////if (LastInputOutputSavedData.rbtnReH_ResultsInterface.Equals("True"))
                ////{
                ////    writer.WriteElementString(Constants.XML_ResultsInterface_ReH_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                ////}

                //if (resInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rp02)
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rp02_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                //}
                //if (resInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.Rt05)
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rt05_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                //}
                //if (resInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReL)
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_ReL_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                //}
                //if (resInterface.LastCheckedRbtn == LastCheckedRadioButtonInResultsInterface.ReH)
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_ReH_Rm, LastInputOutputSavedData.tfRRm_ResultsInterface);
                //}

                //writer.WriteElementString(Constants.XML_ResultsInterface_F, LastInputOutputSavedData.tfF_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_Fm, LastInputOutputSavedData.tfFm_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_Ag, LastInputOutputSavedData.tfAg_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_Agt, LastInputOutputSavedData.tfAgt_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_A, LastInputOutputSavedData.tfA_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_At, LastInputOutputSavedData.tfAt_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_Su, LastInputOutputSavedData.tfSu_ResultsInterface);
                //writer.WriteElementString(Constants.XML_ResultsInterface_Z, LastInputOutputSavedData.tfZ_ResultsInterface);
                //if (LastInputOutputSavedData.chbn_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_n, LastInputOutputSavedData.tfn_ResultsInterface);
                //}
                //else
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_n, String.Empty);
                //}
                //if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.au, LastInputOutputSavedData.au);
                //    writer.WriteElementString(Constants.bu, LastInputOutputSavedData.bu);
                //}
                //if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.Du, LastInputOutputSavedData.Du);
                //}

                //if (LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rmax, LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface);
                //}
                //else 
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_Rmax, String.Empty);
                //}
                //if (LastInputOutputSavedData.chbe2_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_eR2, LastInputOutputSavedData.e2Min_ResultsInterface + "-" + LastInputOutputSavedData.e2Max_ResultsInterface + "%");
                //}
                //else 
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_eR2, String.Empty);
                //}
                //if (LastInputOutputSavedData.chbe4_ResultsInterface.Equals("True"))
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_eR4, LastInputOutputSavedData.e4Min_ResultsInterface + "-" + LastInputOutputSavedData.e4Max_ResultsInterface + "%");
                //}
                //else 
                //{
                //    writer.WriteElementString(Constants.XML_ResultsInterface_eR4, String.Empty);
                //}

                //#endregion


                //writer.WriteEndElement();


                //writer.WriteEndElement();
                //writer.WriteEndDocument();
                //writer.Close();



                //  }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void writeSampleReportOnlineXml()}", System.DateTime.Now);
            }
        }

       

        /// <summary>
        /// ovo se radi prilikom pocetnog ucitavanja aplikacije da bi se dobio rezultat zadnje pokidanog uzorka pre gasenja aplikacije
        /// </summary>
        //public void showEmptyResultsInterface() 
        public void loadFirstAfterRunResultsInterface(bool loadRp02 = true, bool loadRm = true, bool loadReL = true, bool loadReH = true, bool loadA = true) 
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    MainWindow window = (MainWindow)MainWindow.GetWindow(this);

                    if (ResultsInterface.isCreatedResultsInterface != null)
                    {
                        if (ResultsInterface.isCreatedResultsInterface == true)
                        {
                            //MessageBox.Show("Prozor sa rezultatima je vec otvoren!");
                            return;
                        }
                    }

                    ResultsInterface resultsInterface = new ResultsInterface(this, window.Plotting, window.PrintScreen);
                    resInterface = resultsInterface;
                    ResultsInterface.isCreatedResultsInterface = true;
                    //resInterface.Show();

                    //i postavi na koju se putanju fajla trenutno radi

                    resInterface.FilePath = window.Plotting.tfFilepathPlotting.Text;

                    resInterface.SetTextBoxesForResultsInterface(loadRp02, loadRm, loadReL, loadReH, loadA);
                    resInterface.SetRadioButtons();
                    resInterface.SetTextBoxes();
                    resInterface.SetTextBoxesForResultsInterface();

                }));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void loadFirstAfterRunResultsInterface(bool loadRp02 = true, bool loadRm = true, bool loadReL = true, bool loadReH = true, bool loadA = true)}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// ovo se koristi samo ako se u online modu zeli prikazati i prozor sa izlaznim podacima, kada se klikne na dugme podaci o uzorku
        /// u ovoj metodi se postavljaju samo vrednosti odredjene cekiranim radiodugmetom
        /// </summary>
        public void setResultsInterfaceFandRRm() 
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    MainWindow window = (MainWindow)MainWindow.GetWindow(this);


                    double F;
                    dataReader.Filepath = window.Plotting.tfFilepathPlotting.Text;
                    double s0 = dataReader.getS0Offline();
                    if (this.ResultsInterface.rbtnRp02.IsChecked == true)
                    {
                        F = window.Plotting.Rp02RI * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                    }
                    if (this.ResultsInterface.rbtnRt05.IsChecked == true)
                    {
                        F = window.Plotting.Rt05 * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                    }
                    if (this.ResultsInterface.rbtnReL.IsChecked == true)
                    {
                        F = window.Plotting.ReL * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                    }
                    if (this.ResultsInterface.rbtnReH.IsChecked == true)
                    {
                        F = window.Plotting.ReH * s0 /* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                    }



                    double RatioRRm;
                    if (this.ResultsInterface.rbtnRp02.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.Rp02RI / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }
                    if (this.ResultsInterface.rbtnRt05.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.Rt05 / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }
                    if (this.ResultsInterface.rbtnReL.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.ReL / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }
                    if (this.ResultsInterface.rbtnReH.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.ReH / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }

                    writeXMLLastResultsInterface();
                }));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void setResultsInterfaceFandRRm()}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// ovo se koristi samo ako se u online modu zeli prikazati i prozor sa izlaznim podacima, kada se klikne na dugme podaci o uzorku
        /// u ovoj metodi se postavljaju samo vrednosti izlaznih podataka
        /// </summary>
        public void  setResultsInterface(string su, string z, int numberOfCallForDrawFitting = 0)
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                    if (window.Plotting.Rp02RI != -1 || Double.IsNegativeInfinity(window.Plotting.Rp02RI) == true || Double.IsPositiveInfinity(window.Plotting.Rp02RI) == true)
                    {
                        if (window.Plotting.Rp02RI != 0)
                        {
                            window.Plotting.Rp02RI = Math.Round(window.Plotting.Rp02RI, 0);
                            resInterface.tfRp02.Text = window.Plotting.Rp02RI.ToString();
                            window.Plotting.tfRp02.Text = window.Plotting.Rp02RI.ToString();
                            if (this.ResultsInterface.chbRp02.IsChecked == true)
                            {
                                resInterface.tfRp02.Foreground = Brushes.Black;
                            }
                            else
                            {
                                resInterface.tfRp02.Foreground = Brushes.AliceBlue;
                            }
                        }
                        else
                        {
                            resInterface.tfRp02.Text = String.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfRp02.Text = String.Empty;
                    }
                    if (window.Plotting.Rt05 != -1)
                    {
                        if (window.Plotting.Rt05 != 0)
                        {
                            window.Plotting.Rt05 = Math.Round(window.Plotting.Rt05, 0);
                            resInterface.tfRt05.Text = window.Plotting.Rt05.ToString();
                            if (this.ResultsInterface.chbRt05.IsChecked == true)
                            {
                                resInterface.tfRt05.Foreground = Brushes.Black;
                            }
                            else
                            {
                                resInterface.tfRt05.Foreground = Brushes.AliceBlue;
                            }
                        }
                        else
                        {
                            resInterface.tfRt05.Text = String.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfRt05.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.Rt05) == true || Double.IsPositiveInfinity(window.Plotting.Rt05) == true || window.Plotting.Rt05 == Double.MinValue || window.Plotting.Rt05 == Double.MaxValue))
                    {
                        resInterface.tfRt05.Text = String.Empty;
                    }



                    if (window.Plotting.ReL != -1)
                    {
                        if (window.Plotting.ReL != 0)
                        {
                            window.Plotting.ReL = Math.Round(window.Plotting.ReL, 0);
                            resInterface.tfReL.Text = window.Plotting.ReL.ToString();
                            if (this.ResultsInterface.chbReL.IsChecked == true)
                            {
                                resInterface.tfReL.Foreground = Brushes.Black;
                            }
                            else
                            {
                                resInterface.tfReL.Foreground = Brushes.AliceBlue;
                            }
                        }
                        else
                        {
                            resInterface.tfReL.Text = String.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfReL.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.ReL) == true || Double.IsPositiveInfinity(window.Plotting.ReL) == true || window.Plotting.ReL == Double.MinValue || window.Plotting.ReL == Double.MaxValue))
                    {
                        resInterface.tfReL.Text = String.Empty;
                    }



                    if (window.Plotting.ReH != -1)
                    {
                        if (window.Plotting.ReH != 0)
                        {
                            resInterface.tfReH.Text = window.Plotting.ReH.ToString();
                            if (this.ResultsInterface.chbReH.IsChecked == true)
                            {
                                resInterface.tfReH.Foreground = Brushes.Black;
                            }
                            else
                            {
                                resInterface.tfReH.Foreground = Brushes.AliceBlue;
                            }
                        }
                        else
                        {
                            resInterface.tfReH.Text = String.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfReH.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.ReH) == true || Double.IsPositiveInfinity(window.Plotting.ReH) == true || window.Plotting.ReH == Double.MinValue || window.Plotting.ReH == Double.MaxValue))
                    {
                        resInterface.tfReH.Text = String.Empty;
                    }



                    if (window.Plotting.Rm != -1)
                    {
                        resInterface.tfRm.Text = window.Plotting.Rm.ToString();
                        if (resInterface.tfRm.Text.Equals("0"))
                        {
                            resInterface.tfRm.Text = String.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfRm.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.Rm) == true || Double.IsPositiveInfinity(window.Plotting.Rm) == true || window.Plotting.Rm == Double.MinValue || window.Plotting.Rm == Double.MaxValue))
                    {
                        resInterface.tfRm.Text = String.Empty;
                    }


                    if (window.Plotting.Fm != -1)
                    {
                        resInterface.tfFm.Text = window.Plotting.Fm.ToString();
                    }
                    else
                    {
                        resInterface.tfFm.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.Fm) == true || Double.IsPositiveInfinity(window.Plotting.Fm) == true || window.Plotting.Fm == Double.MinValue || window.Plotting.Fm == Double.MaxValue))
                    {
                        resInterface.tfFm.Text = String.Empty;
                    }


                    if (window.Plotting.A != -1)
                    {
                        if (plotting.IsAActive == true)
                        {
                            plotting.AManualClickedValue = Math.Round(plotting.AManualClickedValue, 2);
                            resInterface.tfA.Text = plotting.tfA.Text;
                            writeXMLLastResultsInterface();
                        }
                        else
                        {
                            resInterface.tfA.Text = window.Plotting.A.ToString();
                        }
                    }
                    else
                    {
                        resInterface.tfA.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.A) == true || Double.IsPositiveInfinity(window.Plotting.A) == true || window.Plotting.A == Double.MinValue || window.Plotting.A == Double.MaxValue))
                    {
                        resInterface.tfA.Text = String.Empty;
                    }


                    if (window.Plotting.At != -1)
                    {
                        resInterface.tfAt.Text = window.Plotting.At.ToString();
                    }
                    else
                    {
                        resInterface.tfAt.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.At) == true || Double.IsPositiveInfinity(window.Plotting.At) == true || window.Plotting.At == Double.MinValue || window.Plotting.At == Double.MaxValue))
                    {
                        resInterface.tfAt.Text = String.Empty;
                    }


                    if (window.Plotting.Ag != -1 && window.Plotting.Ag <= window.Plotting.A)
                    {
                        window.Plotting.Ag = Math.Round(window.Plotting.Ag, 3);
                        if (window.Plotting.Ag >= 0)
                        {
                            resInterface.tfAg.Text = window.Plotting.Ag.ToString();
                        }
                        else
                        {
                            resInterface.tfAg.Text = string.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfAg.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.Ag) == true || Double.IsPositiveInfinity(window.Plotting.Ag) == true || window.Plotting.Ag == Double.MinValue || window.Plotting.Ag == Double.MaxValue))
                    {
                        resInterface.tfAg.Text = String.Empty;
                    }


                    if (window.Plotting.Agt != -1)
                    {
                        resInterface.tfAgt.Text = window.Plotting.Agt.ToString();
                    }
                    else
                    {
                        resInterface.tfAgt.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.Agt) == true || Double.IsPositiveInfinity(window.Plotting.Agt) == true || window.Plotting.Agt == Double.MinValue || window.Plotting.Agt == Double.MaxValue))
                    {
                        resInterface.tfAgt.Text = String.Empty;
                    }

                    if (window.Plotting.Lu != -1)
                    {
                        resInterface.tfLu.Text = window.Plotting.Lu.ToString();
                    }
                    else
                    {
                        resInterface.tfLu.Text = String.Empty;
                    }
                    if ((Double.IsNegativeInfinity(window.Plotting.Lu) == true || Double.IsPositiveInfinity(window.Plotting.Lu) == true || window.Plotting.Lu == Double.MinValue || window.Plotting.Lu == Double.MaxValue))
                    {
                        resInterface.tfLu.Text = String.Empty;
                    }


                    double F;
                    dataReader.Filepath = window.Plotting.tfFilepathPlotting.Text;
                    double s0 = dataReader.getS0Offline();
                    if (this.ResultsInterface.rbtnRp02.IsChecked == true)
                    {
                        F = window.Plotting.Rp02RI * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                        //this.ResultsInterface.tfF.Text = string.Empty;
                    }
                    if (this.ResultsInterface.rbtnRt05.IsChecked == true)
                    {
                        F = window.Plotting.Rt05 * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                        //this.ResultsInterface.tfF.Text = string.Empty;
                    }
                    if (this.ResultsInterface.rbtnReL.IsChecked == true)
                    {
                        F = window.Plotting.ReL * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                        //this.ResultsInterface.tfF.Text = string.Empty;
                    }
                    if (this.ResultsInterface.rbtnReH.IsChecked == true)
                    {
                        F = window.Plotting.ReH * s0/* / 1000*/;//in N not in kN
                        F = Math.Round(F, 2);
                        this.ResultsInterface.tfF.Text = F.ToString();
                        //this.ResultsInterface.tfF.Text = string.Empty;
                    }



                    double RatioRRm;
                    if (this.ResultsInterface.rbtnRp02.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.Rp02RI / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }
                    if (this.ResultsInterface.rbtnRt05.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.Rt05 / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }
                    if (this.ResultsInterface.rbtnReL.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.ReL / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }
                    if (this.ResultsInterface.rbtnReH.IsChecked == true)
                    {
                        RatioRRm = window.Plotting.ReH / window.Plotting.Rm;
                        RatioRRm = Math.Round(RatioRRm, 3);
                        this.ResultsInterface.tfRRm.Text = RatioRRm.ToString();
                    }


                    if (window.Plotting.RmaxwithPoint != -1)
                    {
                        resInterface.tfRmax.Text = window.Plotting.RmaxwithPoint.ToString();

                        if (this.ResultsInterface.chbRmax.IsChecked == true)
                        {
                            resInterface.tfRmax.Foreground = Brushes.Black;
                        }
                        else
                        {
                            //resInterface.tfRmax.Text = String.Empty;
                            resInterface.tfRmax.Foreground = Brushes.AliceBlue;
                            LastInputOutputSavedData.chbRmax_ResultsInterface = "False";
                            //LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = String.Empty;
                        }
                    }
                    else
                    {
                        resInterface.tfRmax.Text = String.Empty;
                        //LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = String.Empty;
                    }

                    if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                    {
                        LastInputOutputSavedData.isE2E4BorderSelected = "True";
                    }
                    else
                    {
                        LastInputOutputSavedData.isE2E4BorderSelected = "False";
                    }

                    if (window.Plotting.E2MinValue != -1 && window.Plotting.E2MaxValue != -1)
                    {
                        //resInterface.tfE2.Text = /*window.Plotting.E2MinValue.ToString() + " - " + window.Plotting.E2MaxValue.ToString()*/ window.Plotting.E2AvgValue.ToString();
                        resInterface.tfE2.Text = /*window.Plotting.E2MinValue.ToString() + " - " + window.Plotting.E2MaxValue.ToString()*/ window.Plotting.E2e4CalculationAfterManualFitting.ArrayE2Interval.Max().ToString();

                        if (this.ResultsInterface.chbe2.IsChecked == true)
                        {
                            resInterface.tfE2.Foreground = Brushes.Black;
                        }
                        else
                        {
                            resInterface.tfE2.Foreground = Brushes.AliceBlue;
                            //resInterface.tfE2.Text = String.Empty;
                            //LastInputOutputSavedData.e2Min_ResultsInterface = String.Empty;
                            //LastInputOutputSavedData.e2Max_ResultsInterface = String.Empty;
                            LastInputOutputSavedData.chbe2_ResultsInterface = "False";
                        }
                    }
                    else
                    {
                        resInterface.tfE2.Text = String.Empty;
                        //LastInputOutputSavedData.e2Min_ResultsInterface = String.Empty;
                        //LastInputOutputSavedData.e2Max_ResultsInterface = String.Empty;
                        LastInputOutputSavedData.chbe2_ResultsInterface = "False";
                    }

                    if (window.Plotting.E4MinValue != -1 && window.Plotting.E4MaxValue != -1)
                    {
                        //resInterface.tfE4.Text = /*window.Plotting.E4MinValue.ToString() + " - " + window.Plotting.E4MaxValue.ToString()*/window.Plotting.E4AvgValue.ToString();
                        resInterface.tfE4.Text = /*window.Plotting.E4MinValue.ToString() + " - " + window.Plotting.E4MaxValue.ToString()*/window.Plotting.E2e4CalculationAfterManualFitting.ArrayE4Interval.Max().ToString();
                        if (this.ResultsInterface.chbe4.IsChecked == true)
                        {
                            resInterface.tfE4.Foreground = Brushes.Black;
                        }
                        else
                        {
                            resInterface.tfE4.Foreground = Brushes.AliceBlue;
                            //resInterface.tfE4.Text = String.Empty;
                            //LastInputOutputSavedData.e4Min_ResultsInterface = String.Empty;
                            //LastInputOutputSavedData.e4Max_ResultsInterface = String.Empty;
                            LastInputOutputSavedData.chbe4_ResultsInterface = "False";
                        }
                    }
                    else
                    {
                        resInterface.tfE4.Text = String.Empty;
                        //LastInputOutputSavedData.e4Min_ResultsInterface = String.Empty;
                        //LastInputOutputSavedData.e4Max_ResultsInterface = String.Empty;
                        LastInputOutputSavedData.chbe4_ResultsInterface = "False";
                    }

                    if (window.Plotting.Re != -1)
                    {
                        //window.Plotting.Re = window.Plotting.Border005Global;
                        //window.Plotting.Re = Math.Round(window.Plotting.Re,0);
                        //if (this.OnHeader != null && this.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                        //{
                        resInterface.tfRe.Text = window.Plotting.Re.ToString();
                        //}
                        if (this.ResultsInterface.chbRe.IsChecked == true)
                        {
                            resInterface.tfRe.Foreground = Brushes.Black;
                        }
                        else
                        {
                            resInterface.tfRe.Foreground = Brushes.AliceBlue;
                            LastInputOutputSavedData.chbRe_ResultsInterface = "False";
                        }
                    }
                    else
                    {
                        if (this.OnHeader != null && this.OnHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                        {
                            resInterface.tfRe.Text = String.Empty;
                        }
                        LastInputOutputSavedData.chbRe_ResultsInterface = "False";
                    }

                    if (window.Plotting.YungsModuo != -1)
                    {
                        resInterface.tfE.Text = window.Plotting.YungsModuo.ToString();
                        if (this.resInterface.chbE.IsChecked == true)
                        {
                            resInterface.tfE.Foreground = Brushes.Black;
                        }
                        else
                        {
                            resInterface.tfE.Foreground = Brushes.AliceBlue;
                            LastInputOutputSavedData.chbE_ResultsInterface = "False";
                        }
                    }
                    else
                    {
                        resInterface.tfE.Text = String.Empty;
                        LastInputOutputSavedData.chbE_ResultsInterface = "False";
                    }

                    if (numberOfCallForDrawFitting % 3 == 0 && window.Plotting.NHardeningExponent != null)
                    {
                        if (window.Plotting.Ag >= OptionsInPlottingMode.EndIntervalForN)
                        {
                            window.Plotting.NHardeningExponent.N = Math.Round(window.Plotting.NHardeningExponent.N, 4);
                            resInterface.tfn.Text = window.Plotting.NHardeningExponent.N.ToString();

                            if (this.ResultsInterface.chbn.IsChecked == true)
                            {
                                resInterface.tfn.Foreground = Brushes.Black;
                            }
                            else
                            {
                                resInterface.tfn.Foreground = Brushes.AliceBlue;
                                //resInterface.tfn.Text = String.Empty;
                                LastInputOutputSavedData.tfn_ResultsInterface = String.Empty;
                            }
                        }
                        else
                        {
                            resInterface.tfn.Foreground = Brushes.AliceBlue;
                            //resInterface.tfn.Text = String.Empty;
                            LastInputOutputSavedData.tfn_ResultsInterface = String.Empty;
                        }
                    }


                    if (window.Plotting.IsRectangle == true)
                    {
                        //string currInOutFileName = resInterface.GetCurrentInputOutputFile();
                        //List<string> dataListInputOutput = new List<string>();
                        //dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                        if (resInterface.tfAGlobal != null)
                        {
                            if (_isStoppedOnlineSample == true)
                            {
                                resInterface.tfAGlobal.Text = String.Empty;
                            }
                            else
                            {
                                //ne zelimo da se pamti nego uvek da se cisti
                                //resInterface.tfAGlobal.Text = window.Plotting.au;
                                resInterface.tfAGlobal.Text = string.Empty;
                            }
                        }
                        if (resInterface.tfBGlobal != null)
                        {
                            if (_isStoppedOnlineSample == true)
                            {
                                resInterface.tfBGlobal.Text = String.Empty;
                            }
                            else
                            {
                                //ne zelimo da se pamti nego uvek da se cisti
                                //resInterface.tfBGlobal.Text = window.Plotting.bu;
                                resInterface.tfBGlobal.Text = string.Empty;
                            }
                        }
                    }

                    if (window.Plotting.IsCircle == true)
                    {
                        //string currInOutFileName = resInterface.GetCurrentInputOutputFile();
                        //List<string> dataListInputOutput = new List<string>();
                        //dataListInputOutput = File.ReadAllLines(currInOutFileName).ToList();
                        if (resInterface.tfDGlobal != null)
                        {
                            if (_isStoppedOnlineSample == true)
                            {
                                resInterface.tfDGlobal.Text = String.Empty;
                                resInterface.tfDGlobal.IsReadOnly = false;
                            }
                            else
                            {
                                //resInterface.tfDGlobal.Text = window.Plotting.Du;
                                //necemo da se pamti zadnja uneta vrednost nego da se cisti uvek
                                resInterface.tfDGlobal.Text = string.Empty;
                                resInterface.tfDGlobal.IsReadOnly = false;
                            }
                        }
                    }

                    if (su != null && su.Equals("0"))
                    {
                        resInterface.tfSu.Text = string.Empty;
                    }
                    else
                    {
                        resInterface.tfSu.Text = su;
                    }
                    if (z != null && z.Equals("100"))
                    {
                        resInterface.tfZ.Text = string.Empty;
                    }
                    else
                    {
                        resInterface.tfZ.Text = z;
                    }


                    writeXMLLastResultsInterface();


                    if (_isStoppedOnlineSample == true && numberOfCallForDrawFitting % 3 == 0)
                    {
                        _isStoppedOnlineSample = false;
                        numberOfCallForDrawFitting = 0;
                        resInterface.tfLu.SelectAll();
                        resInterface.tfLu.Focus();
                    }

                }));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void  setResultsInterface(string su, string z, int numberOfCallForDrawFitting = 0)}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// ovo se zove kada se u online modu klikne na dugme podaci o uzorku
        /// ovde se prikazuju prozori sa izlaznim podacima
        /// </summary>
        private void showInputOutputDataInFocus() 
        {
            try
            {
                onHeader.GeneralData.Show();
                onHeader.ConditionsOfTesting.Show();
                onHeader.MaterialForTesting.Show();
                onHeader.PositionOfTube.Show();
                onHeader.RemarkOfTesting.Show();
                onHeader.Show();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void showInputOutputDataInFocus()}", System.DateTime.Now);
            }
        }

        private bool stopSampling = false;

        /// <summary>
        /// u ovoj metodi se vrsi provera da li postoji ili ne postoji upis u online fajl
        /// jel upis se nastavlja i posle klika na dugme za prekid kidanja
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="timerEventArgs"></param>
        private void OnTimedEventMachineWorking(object sender,MicroTimerEventArgs timerEventArgs)
        {
            try
            {
                // Do something small that takes significantly less time than Interval.
                // BeginInvoke executes on the UI thread but this calling thread does not
                //  wait for completion before continuing (i.e. it executes asynchronously)
                FileInfo finfo = new FileInfo(Constants.onlineFilepath);
                //beg = DateTime.Now;
                isCurrentProgressWrittingInOnlineFile = true;
                int oldNumber = dataReader.CountLine_ONLINE2;
                long oldSize = finfo.Length;
                System.Threading.Thread.Sleep(500);
                int newCounterValue = dataReader.ReadDataOnLine2();
                int newNumber = dataReader.CountLine_ONLINE2;
                FileInfo finfoNew = new FileInfo(Constants.onlineFilepath);
                long newSize = finfoNew.Length;

                //checking for online writing ending
                //ukoliko nema izmedju dva poziva tajmera upisa u online file zabelezi u promenljivoj howManyMilisecondsNoWriting
                //koliko dugo nije bilo upisa u online fajl
                if (oldNumber == newNumber || oldSize == newSize)
                {
                    howManyMilisecondsNoWriting2 = howManyMilisecondsNoWriting2 + /*OptionsInOnlineMode.refreshTimeInterval*/500;
                    //ako je vreme u kome nije zabelezen nijedan upis vece od vremena cekanja postavljenog u online opcijama
                    //tada prekini izvrsavanje tajmera jer je tekuci online upis zavrsen
                    if (howManyMilisecondsNoWriting2 >= OptionsInOnlineMode.onlineWriteEndTimeInterval)
                    {
                        //obavesti program da online upis vise nije u toku
                        isCurrentProgressWrittingInOnlineFile = false;
                        howManyMilisecondsNoWriting2 = 0;

                        _microTimerMachineWorking.Stop();
                        _microTimerMachineWorking.Abort();
                        stopSampling = true;

                        return;
                    }
                }
                else
                {
                    howManyMilisecondsNoWriting2 = 0;
                    isCurrentProgressWrittingInOnlineFile = true;
                }

                //end = DateTime.Now;
            }
            catch (Exception ex)
            {
 
            }
        }

        /// <summary>
        /// kada uzimas novo merenje ocisti grafike koji beleze promenu napona i izduzenja od prethodnog merenja  
        /// </summary>
        private void clearGraphicsOfChangeRandE() 
        {
            try
            {
                if (vXY != null)
                {
                    vXY.DeleteChangeOfRGraphic();
                    vXY.DeleteChangeOfRGraphic_Fitting();
                    vXY.DeleteChangeOfEGraphic();
                    vXY.DeleteChangeOfEGraphic_Fitting();
                    vXY.CreateChangeOfRGraphic();
                    vXY.CreateChangeOfRGraphic_Fitting();
                    vXY.CreateChangeOfEGraphic();
                    vXY.CreateChangeOfEGraphic_Fitting();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void clearGraphicsOfChangeRandE()}", System.DateTime.Now);
            }
        }


        /// <summary>
        /// fitovanje do Rp02/2 grafika na kojima se prati Promena napona i izduzenja 
        /// kao i racunanje min i max intervala E(R2) i E(R4)
        /// </summary>
        private void doFittingChangeOfRandEGraphics(/*double E2E4Border, double XTranslateAmountFittingMode*/) 
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);


                if (window.Plotting.DataReader == null || vXY == null)
                {
                    return;
                }

                //sredi grafike za promenu napona i izduzenja tj u pocetku gde god je vrednost napona manja od Rm/2
                //int rmIndex = window.Plotting.GetRpmaxIndex();
                //int rp02Index = window.Plotting.Rp02Index;
                //double rm = dataReader.PreassureInMPa[rmIndex];
                //double rm = window.Plotting.DataReader.PreassureInMPa[rmIndex];
                double rp02 = window.Plotting.Rp02RI;
                int ii;
                //for (ii = 0; ii < vXY.PointsChangeOfR.Count; ii++)
                for (ii = 0; ii < vXY.ArrayElongationOfEndOfInterval.Count; ii++)
                {
                    //if (vXY.ArrayROfEndOfInterval[ii] < rp02 / 2)
                    //{

                    //}
                    //else
                    //{
                    //    break;
                    //}

                    if (vXY.ArrayElongationOfEndOfInterval[ii] < plotting.A)
                    {

                    }
                    else
                    {
                        break;
                    }

                }

                int iiRp02;
                for (iiRp02 = 0; iiRp02 < vXY.PointsChangeOfR.Count; iiRp02++)
                {
                    if (vXY.ArrayROfEndOfInterval[iiRp02] < rp02 / 2)
                    {

                    }
                    else
                    {
                        break;
                    }

                }

                //indexOdsecanjaPromeneNaponaZaRsaTackom = ii + 1;
                //vXY.FittingChangeOfRGraphic(iiRp02 + 1);
                //ne fituje se Promena izduzenja
                //indexOdsecanjaPromeneIzduzenjaZaEsaTackom_2 = ii + 1;
                vXY.FittingChangeOfEGraphicFromEnd(ii + 1);
                vXY.FittingChangeOfRGraphicFromBeetwen(iiRp02, ii + 1);
                //vXY.FittingChangeOfEGraphic(ii + 1);
                //vXY.FittingArrayElongationOfEndOfInterval(ii + 1, E2E4Border, XTranslateAmountFittingMode);
                //vXY.FittingChangeOfEGraphic(0);
                indexFromChangedParametersFitting = ii + 1;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void doFittingChangeOfRandEGraphics(/*double E2E4Border, double XTranslateAmountFittingMode*/)}", System.DateTime.Now);
            }
        }

        public double RmaxGlobal;

        /// <summary>
        /// racunanje maksimalne promene napona u toku kidanja
        /// </summary>
        private void calculateRmaxWithPoint()
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (vXY == null)
                {
                    window.Plotting.RmaxwithPoint = -1;
                    return;
                }
                List<double> candidatesForMaximumChangeOfPreassure = new List<double>();
                //skupljanje vrednosti Y - ose, koja odredjue promene napona u toku kidanja 
                for (int i = 0; i < vXY.PointsChangeOfRFitting.Count; i++)
                {
                    candidatesForMaximumChangeOfPreassure.Add(vXY.PointsChangeOfRFitting[i].YAxisValue);
                }


                //nalazenje maksimalne vrednosti promene napona kidanja
                if (candidatesForMaximumChangeOfPreassure.Count > 0)
                {
                    window.Plotting.RmaxwithPoint = candidatesForMaximumChangeOfPreassure.Max();
                    LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = window.Plotting.RmaxwithPoint.ToString();
                    RmaxGlobal = window.Plotting.RmaxwithPoint;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void calculateRmaxWithPoint()}", System.DateTime.Now);
            }

        }

        private void calculateE2WithPoint()
        {
            //List<double> candidatesForMaximumChangeOfE2WithPoint = new List<double>();
            //for (int i = 0; i < vXY.PointsChangeOfEFitting.Count; i++)
            //{
            //    candidatesForMaximumChangeOfE2WithPoint.Add(vXY.PointsChangeOfEFitting[i].YAxisValue);
            //}

            //MainWindow window = (MainWindow)MainWindow.GetWindow(this);
            //window.Plotting.EmaxwithPoint = candidatesForMaximumChangeOfE2WithPoint.Max();
        }

        private double calculateEstimatedVelocityOfTraverse(double elongationEnd, double elongationBegin) 
        {
            try
            {
                if (cntTau <= 0)
                {
                    return 0;
                }
                double result = 0;
                double Lo2 = 0;
                bool isNN2 = double.TryParse(LastInputOutputSavedData.tfL0, out Lo2);
                result = (elongationEnd - elongationBegin) / 100 * Lo2;
                return result;

                if (OptionsInOnlineMode.isE2E4BorderSelected == true)
                {
                    if (elongationEnd <= OptionsInOnlineMode.E2E4Border)
                    {
                        double Lc = 0;
                        double Lo = 0;
                        bool isN = double.TryParse(LastInputOutputSavedData.tfLc, out Lc);
                        bool isNN = double.TryParse(LastInputOutputSavedData.tfL0, out Lo);
                        //result = OptionsInOnlineManagingOfTTM.eR2 * (Lc + (elongationEnd - elongationBegin) / 100 * Lo);

                        return result;
                    }
                    else
                    {
                        double Lc = 0;
                        double Lo = 0;
                        bool isN = double.TryParse(LastInputOutputSavedData.tfLc, out Lc);
                        bool isNN = double.TryParse(LastInputOutputSavedData.tfL0, out Lo);
                        //result = OptionsInOnlineManagingOfTTM.eR4 * (Lc + (elongationEnd - elongationBegin) / 100 * Lo);

                        return result;
                    }
                }
                else
                {
                    if (elongationEnd <= OptionsInOnlineMode.E3E4Border)
                    {
                        double Lc = 0;
                        double Lo = 0;
                        bool isN = double.TryParse(LastInputOutputSavedData.tfLc, out Lc);
                        bool isNN = double.TryParse(LastInputOutputSavedData.tfL0, out Lo);
                        //result = OptionsInOnlineManagingOfTTM.eR3 * (Lc + elongation / 100 * Lo)/cntTau;
                        //result = OptionsInOnlineManagingOfTTM.eR2 * (Lc + (elongationEnd - elongationBegin) / 100 * Lo);

                        return result;
                    }
                    else
                    {
                        double Lc = 0;
                        double Lo = 0;
                        bool isN = double.TryParse(LastInputOutputSavedData.tfLc, out Lc);
                        bool isNN = double.TryParse(LastInputOutputSavedData.tfL0, out Lo);
                        //result = OptionsInOnlineManagingOfTTM.eR4 * (Lc + (elongationEnd - elongationBegin) / 100 * Lo);

                        return result;
                    }
                }
            }
            catch (Exception ex) 
            {
                return -1;
            }
        }

        private void OnTimedEvent(object sender,
                                MicroTimerEventArgs timerEventArgs)
        {
            try
            {
                // Do something small that takes significantly less time than Interval.
                // BeginInvoke executes on the UI thread but this calling thread does not
                //  wait for completion before continuing (i.e. it executes asynchronously)

                beg = DateTime.Now;

                int oldNumber = dataReader.CountLine_ONLINE;
                int previousCountSample = dataReader.RelativeElongation.Count;
                int newCounterValue = dataReader.ReadDataOnLine();
                int newNumber = dataReader.CountLine_ONLINE;

                //checking for online writing ending
                if (oldNumber == newNumber || stopSampling == true)
                {
                    //howManyMilisecondsNoWriting = howManyMilisecondsNoWriting + OptionsInOnlineMode.refreshTimeInterval;
                    howManyMilisecondsNoWriting = (beg.Hour * 60 * 60 * 1000 + beg.Minute * 60 * 1000 + beg.Second * 1000 + beg.Millisecond) - (endWriting.Hour * 60 * 60 * 1000 + endWriting.Minute * 60 * 1000 + endWriting.Second * 1000 + endWriting.Millisecond);


                    if (howManyMilisecondsNoWriting >= OptionsInOnlineMode.onlineWriteEndTimeInterval)
                    {

                        //MessageBox.Show("Online upis je završen !");
                        //resetuj brojac vremenske ose [x - osa grafika za promenu napona i izduzenja]
                        cntTau = 0;
                        counter = 0;
                        //obavesti program da online upis vise nije u toku
                        _isStoppedOnlineSample = true;


                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            //dugme kada online upis nije u toku treba da postane enable-van
                            //u toku online upisa ne sme se kliknuti na dugme podaci o uzorku jel puca program
                            //pa stoga je onemogucen klik na to dugme u toku online upisa
                            btnSampleData.IsEnabled = true;
                        }));
                        //MessageBox.Show(dataReader.CountLine_ONLINE.ToString());
                        //find and replace some text in online txt file using c#


                        //double numberOfSamples;
                        ////ovo vise ne pije vodu jel upola kidanja moze da se prekine kidanje
                        //List<string> lines = File.ReadAllLines(fpOnlineGlobal).ToList();
                        ////double numberOfPreassuresInMpa = dataReader.PreassureInMPa.Count;
                        //numberOfSamples = lines.Count - onHeader.HeaderSize;
                        ////numberOfPreassuresInMpa = numberOfPreassuresInMpa - onHeader.HeaderSize;
                        //string text = File.ReadAllText(fpOnlineGlobal);
                        //text = text.Replace(Constants.ONLINEFILEHEADER_BrojZapisa, Constants.ONLINEFILEHEADER_BrojZapisa + numberOfSamples.ToString());
                        //File.WriteAllText(fpOnlineGlobal, text);




                        durationInmilisec = 0;
                        changeOfPreassure = 0;
                        changeOfElongation = 0;
                        isOnlineFileHeaderWritten = false;
                        //currSec = 0;
                        //firstEntermilisec = 0;

                        //write first derivation file
                        //List<string> lines = new List<string>();
                        //string tempLine = string.Empty;
                        //for (int i = 0; i < fD.Count; i++)
                        //{
                        //    tempLine = fD.getXAtEelement(i) + "     " + fD.getYAtEelement(i) + "        " + fD.DeltaX.ElementAt(i) + "      " + fD.DeltaY.ElementAt(i) + "      " + fD.FirstDerivationList.ElementAt(i);
                        //    lines.Add(tempLine);
                        //}
                        //File.WriteAllLines(System.Environment.CurrentDirectory + "\\ChangesOfPreassure.txt", changesOfPreassure);
                        //File.WriteAllLines(System.Environment.CurrentDirectory + "\\ChangesOfElongation.txt", changesOfElongation);
                        //File.WriteAllLines(System.Environment.CurrentDirectory + "\\timerWriting.txt", updateTimerTimes);
                        //if (File.Exists(Constants.animationFilepath) == true)
                        //{
                        //    //File.AppendAllLines(Constants.animationFilepath, animationLines);
                        //    //find and replace some text in animation anim extension file using c#
                        //    double refreshAnimTime = 20;
                        //    if (numberOfSamplesInFirstSecond > 0)
                        //    {
                        //        refreshAnimTime = 1000 / numberOfSamplesInFirstSecond;
                        //    }
                        //    firstSecondDurationInmilisec = 0;
                        //    numberOfSamplesInFirstSecond = 0;
                        //    string textAnim = File.ReadAllText(Constants.animationFilepath);
                        //    textAnim = textAnim.Replace(Constants.ANIMATIONFILEHEADER_refreshAnimationTime, Constants.ANIMATIONFILEHEADER_refreshAnimationTime + refreshAnimTime);
                        //    textAnim = textAnim.Replace(Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure, Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure + maxChangeOfPreasure + "~" + changeOfElongationForMaxPreassure);
                        //    textAnim = textAnim.Replace(Constants.ANIMATIONFILEHEADER_resolution, Constants.ANIMATIONFILEHEADER_resolution + OptionsInOnlineMode.Resolution);

                        //    File.WriteAllText(Constants.animationFilepath, textAnim);
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Ne postoji animacijski fajl!");
                        //}
                        //if (File.Exists(Constants.e2e4Filepath) == true)
                        if (File.Exists(Properties.Settings.Default.e2e4Filepath) == true)
                        {
                            //File.WriteAllLines(Constants.e2e4Filepath, e2e4Lines);Properties.Settings.Default.e2e4Filepath
                            File.WriteAllLines(Properties.Settings.Default.e2e4Filepath, e2e4Lines);
                        }
                        else
                        {
                            // File.AppendAllLines(Constants.e2e4Filepath, e2e4Lines);
                            File.AppendAllLines(Properties.Settings.Default.e2e4Filepath, e2e4Lines);
                            // MessageBox.Show("Ne postoji e2e4 fajl za online upis!");
                        }
                        //File.WriteAllLines(System.Environment.CurrentDirectory + "\\firstDerivation.txt", lines);
                        //fD = new FirstDerivation();

                        currCounterWhoDetermitedOneSecond = 0;
                        maxChangeOfPreasure = 0.0;
                        changeOfElongationForMaxPreassure = 0.0;
                        maxChangeOfElongation = 0.0;
                        maxForceInKN = 0.0;

                        //updateCollectionTimer.Stop();
                        //zaustavi tajmer jel nema  vise online upisa
                        _microTimer.Stop();
                        //ako je tajmer zavrsio ovde znaci da pre toga nije kliknuto na crveno dugme za zavrsetak kidanja 
                        isClickedStopSample = false;

                        //kreiraj nov, prazan grafik spreman za prihvatanje novog online upisa
                        createOnlineGraphics();

                        howManyMilisecondsNoWriting = 0;



                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                            window.IsOnlineModeFinished = true;

                            //predji na tab tri
                            if (OptionsInOnlineMode.isManualChecked)
                            {
                                window.PrintScreen.HidePrintScreenMarkers();
                                window.tab_third.IsSelected = true;
                            }

                            //u then granu ovog uspola ulazi ako je uposte bilo nekog upisa
                            //jel zeleno dugme moze da pokrene pracenje kidanja i kada nema online upisa 
                            //pa je u tom slucaju newNumber jednak 0
                            if (newNumber > 0)
                            {
                                //ovde se ulazi ako je bilo upisa tj newNumber > 0
                                //ali postojao je samo upis koji je imao negativnu silu
                                //tada je vrednost dataReader.PreassureInMPa.Count jednaka 0
                                if (dataReader.PreassureInMPa.Count == 0)
                                {
                                    //MessageBox.Show("Online fajl je prazan!");
                                    tfRemarkOnlineFileHeaderWritten.Text = Constants.ONLINEFILE_NOTENTERSAMPLEDATA;
                                    tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Red;
                                    //btnStopSample.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                                    StopSample_EndOfOnlineWritting();
                                    return;
                                }
                                //ako je bilo kakvog validnog upisa (sila je bila zabelezena da je veca od nule)
                                //ulazi se ovde jel treba da se zapamte  validni podaci koji su upisivani u toku online upisa
                                //if (dataReader.PreassureInMPa.Count > 0)
                                //{
                                //    //MessageOnlineModeYesNo mYesNo = new MessageOnlineModeYesNo();
                                //    //mYesNo.ShowDialog();
                                //    //if (mYesNo.IsYesChosen)
                                //    //{
                                //    //    //SaveDialogForm saveDialog = new SaveDialogForm();
                                //    //    //SaveDialogProperty = saveDialog;
                                //    //    //string currFileNamePath = saveDialog.saveFileDialog1.FileName;


                                //    //    //window.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                                //    //    //window.Plotting.tfFilepathPlottingKeyDown();

                                //    //    window.Plotting.drawFittingGraphic(currFileNamePath);//sluzi samo za racunanje datareadera

                                //    //    window.Plotting.SetRpmaxAndFmaxReHANDReL();

                                //    //    window.Plotting.SetRatioAndCalibrationAfterOnlineWriting();

                                //    //    //window.Plotting.drawFittingGraphic(currFileNamePath);//iscrtavanje grafika
                                //    //    window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                                //    //    if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                                //    //    {
                                //    //        //drawFittingGraphic();
                                //    //        window.Plotting.DeleteOfflineModeOnly();
                                //    //        window.Plotting.drawFittingGraphic(currFileNamePath);

                                //    //    }
                                //    //    else
                                //    //    {
                                //    //        window.Plotting.drawFittingGraphic(currFileNamePath);
                                //    //    }
                                //    //}
                                //    //else
                                //    //{

                                //        // cuvaju se u posebnim fajlovima informacije o zadnje pokidanom uzorku
                                //        // tj online.txt,  animation.anim, e2e4Online.e2e4 pamti u istom direktorijumu kao nezapamceno.txt, nezapamceno.anim, nezapamceno.e2e4
                                //        saveUnsavedFile();
                                //        //u tekst polju offline upisa gde se govori koji fajl je trenutno ucitan u offline modu
                                //        //upisuje se fajl nezapamceno.txt dok se ne klikne na dugme zapamti pa se da ime onda ce se nezapamceno.txt promeniti shodno datom imenu fajla
                                //        string currFileNamePath = Constants.unsavedFilepath;

                                //        window.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                                //        window.Plotting.tfFilepathPlottingKeyDown();

                                //        numberOfCallForDrawFitting++;
                                //        window.Plotting.drawFittingGraphic(currFileNamePath);//sluzi samo za racunanje datareadera
                                //        window.Plotting.SetRpmaxAndFmaxReHANDReL();
                                //        window.Plotting.SetRatioAndCalibrationAfterOnlineWriting();


                                //        //window.Plotting.drawFittingGraphic(currFileNamePath);//iscrtavanje grafika 
                                //        numberOfCallForDrawFitting++;
                                //        window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                                //        if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                                //        {
                                //            //drawFittingGraphic();
                                //            window.Plotting.DeleteOfflineModeOnly();
                                //            numberOfCallForDrawFitting++;
                                //            window.Plotting.drawFittingGraphic(Constants.unsavedFilepath, numberOfCallForDrawFitting);

                                //        }
                                //        else
                                //        {
                                //            numberOfCallForDrawFitting++;
                                //            window.Plotting.drawFittingGraphic(Constants.unsavedFilepath, numberOfCallForDrawFitting);
                                //        }
                                //    //}


                                if (dataReader.PreassureInMPa.Count > 0)
                                {
                                    //ova metoda se poziva i kada se klikne na crveno dugme za prekid kidanja uzorka 
                                    StopSample_EndOfOnlineWritting();
                                }




                                //}// end of then branch if (dataReader.PreassureInMPa.Count > 0)

                            }//end of then branch  if (newNumber > 0)


                            //ovo treba da usledi kada se bude dolazilo do klika na dugme Novo merenje
                            //File.Delete(Constants.onlineFilepath);
                            //var onlineFile = File.Create(Constants.onlineFilepath);
                            //onlineFile.Close();

                            //dataReader.ClearData();

                            //File.Delete(Constants.animationFilepath);
                            //var animationFile = File.Create(Constants.animationFilepath);
                            //animationFile.Close();



                            //clear process parameters
                            //tj postavljanje tekstualnih polja na 0.0
                            tfForceInN.Text = Constants.ZERO;
                            if (OptionsInOnlineMode.calculateMaxForceForTf == true)
                            {
                                tfMaxForceInKN.Text = Constants.ZERO;
                            }
                            //tfElongationForMaxForce.Text = Constants.ZERO;
                            tfPreassureInMPa.Text = Constants.ZERO;
                            tfElongationInMM.Text = Constants.ZERO;
                            tfElongationInProcent.Text = Constants.ZERO;
                            tfDeltaPreassure.Text = Constants.ZERO;
                            tfMaxDeltaPreassure.Text = Constants.ZERO;
                            //tfElongationMaxPreassure.Text = Constants.ZERO;
                            tfDeltaElongation.Text = Constants.ZERO;
                            tfMaxDeltaElongation.Text = Constants.ZERO;
                            //tfPreassureMaxElongation.Text = Constants.ZERO;

                            //ispod dugmeta podaci o uzorku ispisi korisniku poruku da trenutni podaci nisu uneti
                            tfRemarkOnlineFileHeaderWritten.Text = Constants.ONLINEFILE_NOTENTERSAMPLEDATA;
                            tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Red;




                            //doFittingChangeOfRandEGraphics(OptionsInOnlineMode.E2E4Border, window.Plotting.XTranslateAmountFittingMode);
                            //izracunaj maksimalnu promenu napona u toku tek zavrsenog kidanja
                            calculateRmaxWithPoint();
                            //calculateE2WithPoint();
                            //window.Plotting.setE2E4MinMax();

                            //postavi i prikazi prozor sa izlaznim podacima
                            loadFirstAfterRunResultsInterface();
                            resInterface.Show();
                            List<string> inputOutputLines = new List<string>();
                            setResultsInterface(string.Empty, string.Empty);


                            //zabelezi zadnje upisan online header
                            this.WriteXMLLastOnlineHeader();
                            //zabelezi izlazne podatke zadnje pokidanog uzorka
                            this.WriteXMLLastResultsInterface();
                            //zabelezi u ovu strukturu ulazne i izlazne podatke zadnje pokidanog uzorka
                            LastInputOutputSavedData.GetData();
                            //ovde cuvaj informacije input-output fajla
                            getInputOutputLines(ref inputOutputLines);
                            //upis u fajl ulazIzlazOnline.inputoutput
                            //if (File.Exists(Constants.inputOutputFilepath) == true)
                            if (File.Exists(Properties.Settings.Default.inputOutputFilepath) == true)
                            {
                                //File.WriteAllLines(Constants.inputOutputFilepath, inputOutputLines);
                                File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, inputOutputLines);
                            }
                            else
                            {
                                //File.AppendAllLines(Constants.inputOutputFilepath, inputOutputLines);
                                File.AppendAllLines(Properties.Settings.Default.inputOutputFilepath, inputOutputLines);
                                // MessageBox.Show("Ne postoji ulazno - izlazni fajl za online upis!");
                            }
                            // cuvaju se u posebnim fajlovima informacije o zadnje pokidanom uzorku
                            // tj ulazIzlazOnline.inputoutput pamti u istom direktorijumu kao nezapamceno.inputoutput
                            saveUnsavedInputOutput();
                            saveUnsavedPreassureElongation(ref preassure, ref elongation);
                            writeSampleReportOnlineXml();

                            //ocisti sve podatke za sledeci upis online moda 
                            dataReader.ClearData();


                            //sada selektujemo tab3 da bi se odmah prikazao print screen izgleda fitovanog grafika
                            //window.tab_third.IsSelected = true;
                            //ova metoda se vise ne koristi
                            //showInputOutputDataInFocus();
                            resInterface.tfLu.SelectAll();
                            resInterface.tfLu.Focus();

                            window.PrintScreen.IsPrintScreenEmpty = false;
                            window.PrintScreen.btnSampleDataPrintMode.IsEnabled = true;
                            window.PrintScreen.ShowPrintScreenMarkers();

                            if (chbStartSampleShowChangedPar.IsChecked == true)
                            {
                                window.PrintScreen.chbChangeOfRAndE.IsEnabled = true;
                            }
                            else
                            {
                                window.PrintScreen.chbChangeOfRAndE.IsEnabled = false;
                            }

                        }));

                        howManyMilisecondsNoWriting = 0;
                        return;
                    }
                }
                else
                {
                    howManyMilisecondsNoWriting = 0;
                    endWriting = DateTime.Now;
                    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                    {
                        //dugme kada online upis nije u toku treba da postane enable-van
                        //u toku online upisa ne sme se kliknuti na dugme podaci o uzorku jel puca program
                        //pa stoga je onemogucen klik na to dugme u toku online upisa
                        btnSampleData.IsEnabled = false;
                    }));
                }

                DateTime now;
                long writeAnimData = -1;

                //doslo je do upisa u online fajl
                if (dataReader.RelativeElongation.Count > 0)
                {

                    //ovde samo zabelezi poslednje upisan podatak u online fajl
                    for (int i = dataReader.RelativeElongation.Count - 1; i < dataReader.RelativeElongation.Count; )
                    {
                        //if (dataReader.RelativeElongation[i] < 0)
                        //{
                        //    dataReader.RelativeElongation[i] = 0;
                        //}
                        if (i >= dataReader.RelativeElongation.Count)
                        {
                            break;
                        }
                        //pointsAll.Add(new MyPoint(dataReader.PreassureInMPa[i], dataReader.RelativeElongation[i]));

                        //if (pointsAll.Count % OptionsInOnlineMode.Resolution == 0)
                        //{
                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            //dodaj u kolekciju , na osnovu koje se iscrtava online grafik, poslednje upisan podatak u online fajl 
                            points.Add(new MyPoint(dataReader.PreassureInMPa[i], dataReader.RelativeElongation[i]));
                            //zabelezi ovo da se dogodilo u animacioni fajl
                            //da bi se posle animiralo
                            //animationLines.Add(dataReader.PreassureInMPa[i] + "*" + dataReader.RelativeElongation[i]);
                            //i kada se desilo
                            writeAnimData = DateTime.Now.Millisecond;
                        }));

                        //}//end if of Resolution

                        //changeOfPreassure = dataReader.PreassureInMPa[dataReader.PreassureInMPa.Count - 1] - dataReader.PreassureInMPa[dataReader.PreassureInMPa.Count - 100];
                        currCounterWhoDetermitedOneSecond++;

                        //double forceInN = dataReader.PreassureInMPa[i] * OptionsInOnlineMode.S0;
                        double forceInN = dataReader.ForceInKN.Last();
                        double potentialElonfationForMaxForce = dataReader.RelativeElongation.Last();//ako je forceInN najveca tada ova promenljiva postaje vrednost zduzenja prilikom delovanja najvece sile
                        if (forceInN < 10)
                        {
                            forceInN = Math.Round(forceInN, 3);
                        }
                        else if (forceInN < 100)
                        {
                            forceInN = Math.Round(forceInN, 2);
                        }
                        else
                        {
                            forceInN = Math.Round(forceInN, 1);
                        }

                        if (potentialElonfationForMaxForce < 10)
                        {
                            potentialElonfationForMaxForce = Math.Round(potentialElonfationForMaxForce, 3);
                        }
                        else if (potentialElonfationForMaxForce < 100)
                        {
                            potentialElonfationForMaxForce = Math.Round(potentialElonfationForMaxForce, 2);
                        }
                        else
                        {
                            potentialElonfationForMaxForce = Math.Round(potentialElonfationForMaxForce, 1);
                        }

                        if (forceInN > maxForceInKN)
                        {
                            maxForceInKN = forceInN;
                            if (OptionsInOnlineMode.calculateMaxForceForTf == true)
                            {
                                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                {
                                    tfMaxForceInKN.Text = maxForceInKN.ToString();
                                    //tfElongationForMaxForce.Text = potentialElonfationForMaxForce.ToString();
                                }));
                            }
                        }


                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            tfForceInN.Text = forceInN.ToString();
                        }));


                        if (firstSecondDurationInmilisec < 1000)
                        {
                            numberOfSamplesInFirstSecond = points.Count;
                        }

                        if (dataReader.PreassureInMPa[i] < 10)
                        {
                            dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 3);
                        }
                        else if ((dataReader.PreassureInMPa[i] < 100))
                        {
                            dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 2);
                        }
                        else
                        {
                            dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 1);
                        }

                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            tfPreassureInMPa.Text = dataReader.PreassureInMPa[i].ToString();
                            //ovde racunaj maksimalni napon u MPa
                            if (dataReader.PreassureInMPa[i] > maxPreassure)
                            {
                                maxPreassure = dataReader.PreassureInMPa[i];
                            }
                        }));
                        double elongationInMM = dataReader.RelativeElongation[i] * OptionsInOnlineMode.L0 / 100.0;
                        if (elongationInMM < 10)
                        {
                            elongationInMM = Math.Round(elongationInMM, 3);
                        }
                        else if (elongationInMM < 100)
                        {
                            elongationInMM = Math.Round(elongationInMM, 2);
                        }
                        else
                        {
                            elongationInMM = Math.Round(elongationInMM, 1);
                        }

                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            tfElongationInMM.Text = elongationInMM.ToString();
                        }));
                        if (dataReader.RelativeElongation[i] < 10)
                        {
                            dataReader.RelativeElongation[i] = Math.Round(dataReader.RelativeElongation[i], 3);
                        }
                        else if (dataReader.RelativeElongation[i] < 100)
                        {
                            dataReader.RelativeElongation[i] = Math.Round(dataReader.RelativeElongation[i], 2);
                        }
                        else
                        {
                            dataReader.RelativeElongation[i] = Math.Round(dataReader.RelativeElongation[i], 1);
                        }

                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                        {
                            tfElongationInProcent.Text = dataReader.RelativeElongation[i].ToString();
                            if (dataReader.RelativeElongation[i] > maxElongation)
                            {
                                maxElongation = dataReader.RelativeElongation[i];
                            }
                        }));

                        //ako hocemo brzo osvezavanje ide van ovog if-a
                        if (durationInmilisec < 1000/*200*/ /*OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters*/)
                        {
                            if (points.Count == 1)
                            {
                                prevExecutionInMs = beg.Millisecond;
                                currExecutionInMs = beg.Millisecond;
                                changeOfPreassure += points[points.Count - 1].YAxisValue;
                                //ne racunamo vise stvarnu nego predvidjenu brzinu tj brzina koja bi trebala da bude a ne ostvarena
                                //changeOfElongation += points[points.Count - 1].XAxisValue * 0.01;
                                changeOfElongation += calculateEstimatedVelocityOfTraverse(points[points.Count - 1].XAxisValue, 0);
                            }
                            if (points.Count > 1)
                            {
                                currExecutionInMs = beg.Millisecond;
                                if (currExecutionInMs >= prevExecutionInMs)
                                {
                                    durationInmilisec = durationInmilisec + currExecutionInMs - prevExecutionInMs;
                                    firstSecondDurationInmilisec = firstSecondDurationInmilisec + currExecutionInMs - prevExecutionInMs;
                                }
                                if (currExecutionInMs < prevExecutionInMs)
                                {
                                    durationInmilisec = durationInmilisec + 1000/*200*/ /*OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters*/ + currExecutionInMs - prevExecutionInMs;
                                    firstSecondDurationInmilisec = firstSecondDurationInmilisec + 1000/*200*/ /*OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters*/ + currExecutionInMs - prevExecutionInMs;
                                }
                                prevExecutionInMs = currExecutionInMs;
                                changeOfPreassure += points[points.Count - 1].YAxisValue - points[points.Count - 2].YAxisValue;
                                //ne racunamo vise stvarnu nego predvidjenu brzinu tj brzina koja bi trebala da bude a ne ostvarena
                                //changeOfElongation += 0.01 * (points[points.Count - 1].XAxisValue - points[points.Count - 2].XAxisValue);
                                changeOfElongation += calculateEstimatedVelocityOfTraverse(points[points.Count - 1].XAxisValue, points[points.Count - 2].XAxisValue);


                                /*******        OSVEZAVANJE SAMIH Promena 200ms,400ms,600ms,800ms i 1s        ******/
                                counter++;

                                if (changeOfPreassure < 10)
                                {
                                    changeOfPreassure = Math.Round(changeOfPreassure, 3);
                                }
                                else if (changeOfPreassure < 100)
                                {
                                    changeOfPreassure = Math.Round(changeOfPreassure, 2);
                                }
                                else
                                {
                                    changeOfPreassure = Math.Round(changeOfPreassure, 1);
                                }

                                changeOfElongation = Math.Round(changeOfElongation, 5);


                                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 200)
                                {
                                    //200ms
                                    if (counter == 10)
                                    {
                                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                        {
                                            tfDeltaPreassure.Text = changeOfPreassure.ToString();
                                            tfDeltaElongation.Text = changeOfElongation.ToString();
                                        }));
                                        counter = 0;
                                    }
                                }
                                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 400)
                                {
                                    //400ms
                                    if (counter == 20)
                                    {
                                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                        {
                                            tfDeltaPreassure.Text = changeOfPreassure.ToString();
                                            tfDeltaElongation.Text = changeOfElongation.ToString();
                                        }));
                                        counter = 0;
                                    }
                                }
                                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 600)
                                {
                                    //600ms
                                    if (counter == 30)
                                    {
                                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                        {
                                            tfDeltaPreassure.Text = changeOfPreassure.ToString();
                                            tfDeltaElongation.Text = changeOfElongation.ToString();
                                        }));
                                        counter = 0;
                                    }
                                }
                                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 800)
                                {
                                    //800ms
                                    if (counter == 40)
                                    {
                                        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                        {
                                            tfDeltaPreassure.Text = changeOfPreassure.ToString();
                                            tfDeltaElongation.Text = changeOfElongation.ToString();
                                        }));
                                        counter = 0;
                                    }
                                }
                                //if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters == 1000)
                                //{
                                //    //1000ms
                                //    if (counter == 50)
                                //    {
                                //        this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                //        {
                                //            tfDeltaPreassure.Text = changeOfPreassure.ToString();
                                //            tfDeltaElongation.Text = changeOfElongation.ToString();
                                //        }));
                                //        counter = 0;
                                //    }
                                //}
                                /*******        OSVEZAVANJE SAMIH Promena 200ms,400ms,600ms,800ms i 1s        ******/


                                changeOfElongationForFirstDerivation = 0.01 * (points[points.Count - 1].XAxisValue - points[points.Count - 2].XAxisValue);
                                changeOfPreassureForFirstDerivation = changeOfPreassureForFirstDerivation + changeOfPreassure;
                            }
                            now = DateTime.Now;
                            string temp;
                            temp = changeOfPreassure + "            " + points[points.Count - 1].XAxisValue + now + "[" + now.Millisecond + "]";
                            changesOfPreassure.Add(temp);
                            string tempElongation;
                            tempElongation = changeOfElongation + "            " + points[points.Count - 1].YAxisValue + now + "[" + now.Millisecond + "]";
                            changesOfElongation.Add(tempElongation);


                            //if (changeOfElongationForFirstDerivation > 0)
                            //{
                            //   fD.addX(dataReader.RelativeElongation[i]);
                            //   fD.addY(dataReader.PreassureInMPa[i]);

                            //   fD.DeltaX.Add(changeOfElongationForFirstDerivation.ToString());
                            //   fD.DeltaY.Add(changeOfPreassureForFirstDerivation.ToString());

                            //   fD.addFirstDerivationPoint(changeOfElongation, changeOfPreassureForFirstDerivation);
                            //   changeOfPreassureForFirstDerivation = 0;
                            //   changeOfElongationForFirstDerivation = 0;
                            //}

                        }
                        else
                        {

                            if (changeOfPreassure < 10)
                            {
                                changeOfPreassure = /*(1000 / changeOfParametersXYTauInterval) **/ Math.Round(changeOfPreassure, 3);
                            }
                            else if (changeOfPreassure < 100)
                            {
                                changeOfPreassure = /*(1000 / changeOfParametersXYTauInterval) **/ Math.Round(changeOfPreassure, 2);
                            }
                            else
                            {
                                changeOfPreassure = /*(1000 / changeOfParametersXYTauInterval) **/ Math.Round(changeOfPreassure, 1);
                            }

                            this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            {
                                tfDeltaPreassure.Text = changeOfPreassure.ToString();

                                //zabelezi promenu napona na grafiku koja belezi promenu napona kroz vreme
                                cntTau++;
                                if (vXY != null)
                                {
                                    vXY.LastChangeValueOfR = changeOfPreassure;
                                    vXY.ArrayChangeValueOfR.Add(vXY.LastChangeValueOfR);
                                    vXY.LastROfEndOfInterval = dataReader.PreassureInMPa[i];
                                    vXY.ArrayROfEndOfInterval.Add(vXY.LastROfEndOfInterval);
                                    //vXY.LastEOfEndOfInterval = dataReader.RelativeElongation[i];
                                    //vXY.ArrayElongationOfEndOfInterval.Add(vXY.LastEOfEndOfInterval);
                                    vXY.LastPointOfR = new MyPoint(vXY.LastChangeValueOfR, 0.001 * cntTau * 1000 /*OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters*/);//ide puta 0.001 jel se vremenska osa izrazava u sekundama a ne u milisekundama
                                    vXY.PointsChangeOfR.Add(vXY.LastPointOfR);
                                }

                            }));
                            if (changeOfPreassure > maxChangeOfPreasure)
                            {


                                maxChangeOfPreasure = changeOfPreassure;
                                maxChangesOfPreassureHistoryYs.Add(maxChangeOfPreasure);
                                //processParameters.tfMaxDeltaPreassure.Text = maxChangeOfPreasure.ToString();
                                //changeOfElongationForMaxPreassure = dataReader.RelativeElongation[i];
                                changeOfElongationForMaxPreassure = points[points.Count - 1].XAxisValue;
                                //processParameters.tfElongationMaxPreassure.Text = changeOfElongationForMaxPreassure.ToString();
                                if (maxChangeOfPreasure < 10)
                                {
                                    maxChangeOfPreasure = Math.Round(maxChangeOfPreasure, 3);
                                }
                                else if (maxChangeOfPreasure < 100)
                                {
                                    maxChangeOfPreasure = Math.Round(maxChangeOfPreasure, 2);
                                }
                                else
                                {
                                    maxChangeOfPreasure = Math.Round(maxChangeOfPreasure, 1);
                                }
                                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                {
                                    tfMaxDeltaPreassure.Text = maxChangeOfPreasure.ToString();
                                    MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                                    window.Plotting.RmaxwithPoint = maxChangeOfPreasure;

                                }));

                                if (changeOfElongationForMaxPreassure < 10)
                                {
                                    changeOfElongationForMaxPreassure = Math.Round(changeOfElongationForMaxPreassure, 3);
                                }
                                else if (changeOfElongationForMaxPreassure < 100)
                                {
                                    changeOfElongationForMaxPreassure = Math.Round(changeOfElongationForMaxPreassure, 2);
                                }
                                else
                                {
                                    changeOfElongationForMaxPreassure = Math.Round(changeOfElongationForMaxPreassure, 1);
                                }

                                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                {
                                    //tfElongationMaxPreassure.Text = changeOfElongationForMaxPreassure.ToString();

                                }));
                            }//only when reached maximum change of preassure





                            //double changeOfElongation;
                            //changeOfElongation = points[points.Count - 1].XAxisValue - points[points.Count - 2/*BECAUSE REFRESH TIME IS 100ms*/].XAxisValue;

                            //if (changeOfElongation < 10)
                            //{
                            //    changeOfElongation = (1000 / changeOfParametersXYTauInterval) * Math.Round(changeOfElongation, 3);
                            //}
                            //else if (changeOfElongation < 100)
                            //{
                            //    changeOfElongation = (1000 / changeOfParametersXYTauInterval) * Math.Round(changeOfElongation, 2);
                            //}
                            //else
                            //{
                            //    changeOfElongation = (1000 / changeOfParametersXYTauInterval) * Math.Round(changeOfElongation, 1);
                            //}

                            changeOfElongation = /*(1000 / changeOfParametersXYTauInterval) **/ Math.Round(changeOfElongation, 5);

                            this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            {
                                tfDeltaElongation.Text = changeOfElongation.ToString();


                                if (vXY != null)
                                {
                                    //zabelezi promenu izduzenja na grafiku koja belezi promenu izduzenja kroz vreme
                                    //ovde se tau brojac (cntTau) ne uvecava jer se uvecava kod promene napona
                                    vXY.LastChangeValueOfE = changeOfElongation;
                                    vXY.ArrayChangeValueOfE.Add(vXY.LastChangeValueOfE);
                                    //vXY.LastROfEndOfInterval = dataReader.PreassureInMPa[i];
                                    //vXY.ArrayROfEndOfInterval.Add(vXY.LastROfEndOfInterval);
                                    vXY.LastEOfEndOfInterval = dataReader.RelativeElongation[i];
                                    vXY.ArrayElongationOfEndOfInterval.Add(vXY.LastEOfEndOfInterval);
                                    vXY.LastPointOfE = new MyPoint(vXY.LastChangeValueOfE, 0.001 * cntTau * 1000 /*OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters*/);//ide puta 0.001 jel se vremenska osa izrazava u sekundama a ne u milisekundama
                                    vXY.PointsChangeOfE.Add(vXY.LastPointOfE);
                                }
                            }));
                            if (changeOfElongation > maxChangeOfElongation)
                            {
                                double changeOfPreassureForMaxElongation;
                                maxChangeOfElongation = changeOfElongation;
                                maxChangesOfElongationRange2HistoryYs.Add(maxChangeOfElongation);

                                changeOfPreassureForMaxElongation = points[points.Count - 1].YAxisValue;
                                //if (maxChangeOfElongation < 10)
                                //{
                                //    maxChangeOfElongation = Math.Round(maxChangeOfElongation, 3);
                                //}
                                //else if (maxChangeOfElongation < 100)
                                //{
                                //    maxChangeOfElongation = Math.Round(maxChangeOfElongation, 2);
                                //}
                                //else
                                //{
                                //    maxChangeOfElongation = Math.Round(maxChangeOfElongation, 1);
                                //}

                                maxChangeOfElongation = Math.Round(maxChangeOfElongation, 5);

                                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                {
                                    tfMaxDeltaElongation.Text = maxChangeOfElongation.ToString();
                                    //MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                                    //window.Plotting.EmaxwithPoint = maxChangeOfElongation;
                                }));
                                if (changeOfPreassureForMaxElongation < 10)
                                {
                                    changeOfPreassureForMaxElongation = Math.Round(changeOfPreassureForMaxElongation, 3);
                                }
                                else if (changeOfPreassureForMaxElongation < 100)
                                {
                                    changeOfPreassureForMaxElongation = Math.Round(changeOfPreassureForMaxElongation, 2);
                                }
                                else
                                {
                                    changeOfPreassureForMaxElongation = Math.Round(changeOfPreassureForMaxElongation, 1);
                                }

                                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                                {
                                    //tfPreassureMaxElongation.Text = changeOfPreassureForMaxElongation.ToString();
                                }));

                            }//only when reached maximum change of elongation


                            ////double forceInN = dataReader.PreassureInMPa[i] * OptionsInOnlineMode.S0;
                            //double forceInN = dataReader.ForceInKN.Last();
                            //double potentialElonfationForMaxForce = dataReader.RelativeElongation.Last();//ako je forceInN najveca tada ova promenljiva postaje vrednost zduzenja prilikom delovanja najvece sile
                            //if (forceInN < 10)
                            //{
                            //    forceInN = Math.Round(forceInN, 3);
                            //}
                            //else if (forceInN < 100)
                            //{
                            //    forceInN = Math.Round(forceInN, 2);
                            //}
                            //else
                            //{
                            //    forceInN = Math.Round(forceInN, 1);
                            //}

                            //if (potentialElonfationForMaxForce < 10)
                            //{
                            //    potentialElonfationForMaxForce = Math.Round(potentialElonfationForMaxForce, 3);
                            //}
                            //else if (potentialElonfationForMaxForce < 100)
                            //{
                            //    potentialElonfationForMaxForce = Math.Round(potentialElonfationForMaxForce, 2);
                            //}
                            //else
                            //{
                            //    potentialElonfationForMaxForce = Math.Round(potentialElonfationForMaxForce, 1);
                            //}

                            //if (forceInN > maxForceInKN)
                            //{
                            //    maxForceInKN = forceInN;
                            //    this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            //    {
                            //        tfMaxForceInKN.Text = maxForceInKN.ToString();
                            //        tfElongationForMaxForce.Text = potentialElonfationForMaxForce.ToString();
                            //    }));
                            //}


                            //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            //{
                            //    tfForceInN.Text = forceInN.ToString();
                            //}));
                            //if (dataReader.PreassureInMPa[i] < 10)
                            //{
                            //    dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 3);
                            //}
                            //else if ((dataReader.PreassureInMPa[i] < 100))
                            //{
                            //    dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 2);
                            //}
                            //else
                            //{
                            //    dataReader.PreassureInMPa[i] = Math.Round(dataReader.PreassureInMPa[i], 1);
                            //}

                            //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            //{
                            //    tfPreassureInMPa.Text = dataReader.PreassureInMPa[i].ToString();
                            //}));
                            //double elongationInMM = dataReader.RelativeElongation[i] * OptionsInOnlineMode.L0 / 100.0;
                            //if (elongationInMM < 10)
                            //{
                            //    elongationInMM = Math.Round(elongationInMM, 3);
                            //}
                            //else if (elongationInMM < 100)
                            //{
                            //    elongationInMM = Math.Round(elongationInMM, 2);
                            //}
                            //else
                            //{
                            //    elongationInMM = Math.Round(elongationInMM, 1);
                            //}

                            //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            //{
                            //    tfElongationInMM.Text = elongationInMM.ToString();
                            //}));
                            //if (dataReader.RelativeElongation[i] < 10)
                            //{
                            //    dataReader.RelativeElongation[i] = Math.Round(dataReader.RelativeElongation[i], 3);
                            //}
                            //else if (dataReader.RelativeElongation[i] < 100)
                            //{
                            //    dataReader.RelativeElongation[i] = Math.Round(dataReader.RelativeElongation[i], 2);
                            //}
                            //else
                            //{
                            //    dataReader.RelativeElongation[i] = Math.Round(dataReader.RelativeElongation[i], 1);
                            //}

                            //this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                            //{
                            //    tfElongationInProcent.Text = dataReader.RelativeElongation[i].ToString();
                            //}));
                            currCounterWhoDetermitedOneSecond = 0;

                            //if (changeOfElongation > 0)
                            //{
                            //    fD.addX(dataReader.RelativeElongation[i]);
                            //    fD.addY(dataReader.PreassureInMPa[i]);

                            //    fD.DeltaX.Add(changeOfElongation.ToString());
                            //    fD.DeltaY.Add(changeOfPreassure.ToString());

                            //    fD.addFirstDerivationPoint(changeOfElongation, changeOfPreassure);
                            //}
                            //}//end of  if (points.Count > 10 && currCounterWhoDetermitedOneSecond % 10 == 0)


                            durationInmilisec = durationInmilisec - 1000/*200*/ /*OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters*/;
                            changeOfPreassure = 0;
                            changeOfElongation = 0;

                            if (vXY != null)
                            {
                                //save last e2e4line
                                string laste2e4line = vXY.LastPointOfR.YAxisValue + "*" + vXY.LastPointOfR.XAxisValue + "*" + vXY.LastPointOfE.YAxisValue + "*" + vXY.LastPointOfE.XAxisValue + "*" + vXY.LastEOfEndOfInterval + "*" + vXY.LastROfEndOfInterval;
                                e2e4Lines.Add(laste2e4line);
                            }

                        }//end of else for one second duration

                        i = i + 1;

                    }//end for loop
                }// end of if (dataReader.RelativeElongation.Count > 0)

                end = DateTime.Now;
                string line = beg + "           " + end + "[" + beg.Millisecond + "---------" + end.Millisecond + "]" + "{ " + writeAnimData + " }";
                updateTimerTimes.Add(line);
            }
            catch (Exception ex)
            {
 
            }
        }


        /// <summary>
        /// ako je showNowInputWindows jednako true, odmah namesti da iskacu ulazni(plavi) prozori
        /// ako je showNowInputWindows jednako false, ulazni(plavi) prozori ne treba da iskacu
        /// </summary>
        /// <param name="showNowInputWindows"></param>
        public void showInputData(bool showNowInputWindows = true) 
        {
            try
            {
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    if (OnlineFileHeader.isCreatedOnlineHeader == true)
                    {
                        //MessageBox.Show("Prozor sa ulaznim podacima je vec otvoren!");
                        return;
                    }

                    if (File.Exists(fpOnlineGlobal) == false)
                    {
                        MessageBox.Show("Neispravna putanja do online fajla!");
                        return;
                    }
                    OnlineFileHeader onlineHeader = new OnlineFileHeader(this);
                    OnlineFileHeader.isCreatedOnlineHeader = true;
                    onHeader = onlineHeader;
                    if (showNowInputWindows == true)
                    {
                        onlineHeader.Show();
                        onlineHeader.showInputDataWindows();
                    }


                }));
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void showInputData(bool showNowInputWindows = true)}", System.DateTime.Now);
            }
        }
        /// <summary>
        /// dogadja kada se klikne na opciju menija podesavanje online opcija
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowOnlineOptions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isOnlineMode == true)
                {
                    MessageBox.Show("Ne možete podešavati opcije online moda u toku online moda!");
                    return;
                }


                if (OptionsOnline.isCreatedOptionsOnline)
                {
                    MessageBox.Show("Otvoren je prozor opcija u online modu!");
                    return;
                }
                optionsOnline = new OptionsOnline(this);
                OptionsOnline.isCreatedOptionsOnline = true;
                optionsOnline.OnlineModeInstance = this;
                //ucitaj poslednje podesavanje online opcija
                LoadOnlineoptions();
                //postavio radio dugmad na prozoru online opcija
                setRadioButtons();

                //postavio dugme za potvrdu na prozoru online opcija
                optionsOnline.setCheckboxes();
                //prikazi prozora opcija online moda
                optionsOnline.Show();

                //ovo je samo dok se vrsi razvoj results interfejsa

                //vXY = new VelocityOfChangeParametersXY(this);
                //vXY.Show();

                //ResultsInterface resultsInterface = new ResultsInterface(this,window.Plotting);
                //resInterface = resultsInterface;
                //resInterface.Show();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnShowOnlineOptions_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
            
        }

      
        /// <summary>
        /// obrada dogadjaja na klik dugmeta podaci o uzorku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSampleData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onHeader != null)
                {
                    onHeader.GeneralData.Close();
                }
                if (resInterface != null)
                {
                    resInterface.Close();
                }

                if (OnlineFileHeader.isCreatedOnlineHeader == true)
                {
                    //MessageBox.Show("Prozor sa ulaznim podacima je vec otvoren!");
                    return;
                }

                if (File.Exists(fpOnlineGlobal) == false)
                {
                    MessageBox.Show("Neispravna putanja do online fajla!");
                    return;
                }

                OnlineFileHeader onlineHeader = new OnlineFileHeader(this);
                if (onlineHeader != null)
                {
                    onlineHeader.showInputDataWindows();
                    OnlineFileHeader.isCreatedOnlineHeader = true;
                    onHeader = onlineHeader;
                    if (IsClickedSampleDataAtPrintMode == false)
                    {
                        onHeader.DisableInputs();
                    }
                    else
                    {
                        onHeader.EnableInputs();
                        IsClickedSampleDataAtPrintMode = false;
                    }
                    if (this.Plotting.Printscreen.IsPrintScreenEmpty == false && this.Plotting.Printscreen.cmbInputWindow.SelectedIndex == 4)
                    {
                        onlineHeader.Show();
                    }
                    if (this.Plotting.Printscreen.IsPrintScreenEmpty == true)
                    {
                        onlineHeader.Show();
                    }
                }
                //ovde se samo postavljaju izlazni podaci zadnje pokidanog uzorka ali se ne prikazuje prozor jel smo u online modu
                //taj prozor se prikazuje kada se klikne na dugme podaci o uzorku u stampajucem modu
                loadFirstAfterRunResultsInterface();
                //if (resInterface != null)
                //{

                //   resInterface.Show();

                //}
                onHeader.GeneralData.tfBrUzorka.SelectAll();
                onHeader.GeneralData.tfBrUzorka.Focus();

                if (resInterface.rbtnReHStr.Equals("False") && resInterface.rbtnReLStr.Equals("False") && resInterface.rbtnRt05Str.Equals("False") && resInterface.rbtnRp02Str.Equals("False"))
                {
                    resInterface.tfF.Text = string.Empty;
                    resInterface.tfRRm.Text = string.Empty;
                }
                else
                {
                    resInterface.SetRadioButtons();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnSampleData_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
            
        }

        #region onlineButtons

        /// <summary>
        /// obrada dogadjaja na klik dugmeta Novo merenje
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnterNewSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isCurrentProgressWrittingInOnlineFile == true)
                {
                    MessageBox.Show("Online upis je još uvek u toku!");
                    return;
                }

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                //kada se zatvori aplikacija zadnji uzorak se pamti
                //kada se ponovo pokrene aplikacija prvi uzorak je zapamcen pa se promenljiva isCurrentSampleSaved postavlja na true
                if (window.IsThisFirstSample == true)
                {
                    isCurrentSampleSaved = true;
                }

                //ako nije zapamcen poslednje pokidan uzorak pitaj korisnika da li zeli da ga zapamti
                if (isCurrentSampleSaved == false)
                {

                    MessageOnlineModeYesNo mYesNo = new MessageOnlineModeYesNo();
                    mYesNo.ShowDialog();
                    if (mYesNo.IsYesChosen)
                    {
                        bool isSavedChangeRandE = false;
                        if (plotting.Printscreen.chbChangeOfRAndE.IsChecked == true)
                        {
                            isSavedChangeRandE = true;
                        }
                        else
                        {
                            isSavedChangeRandE = false;
                        }
                        bool isManualNCalculated = false;
                        if (plotting.Printscreen.chbCalculateNManual.IsChecked == true)
                        {
                            isManualNCalculated = true;
                        }
                        else
                        {
                            isManualNCalculated = false;
                        }
                        SaveDialogForm saveDialog = new SaveDialogForm(isSavedChangeRandE, isManualNCalculated, this.Plotting.Printscreen);
                        SaveDialogProperty = saveDialog;
                        string currFileNamePath = saveDialog.saveFileDialog1.FileName;


                        if (SaveDialogProperty.IsClickedToSaveFile == true)
                        {
                            window.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                            window.Plotting.tfFilepathPlottingKeyDown();
                            isCurrentSampleSaved = true;
                            //da bi se proracunalo sve
                            window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                            window.tab_second.IsSelected = true;
                        }
                    }
                }



                //File.Delete(Constants.onlineFilepath);
                File.Delete(Properties.Settings.Default.onlineFilepath);
                //var onlineFile = File.Create(Constants.onlineFilepath);
                var onlineFile = File.Create(Properties.Settings.Default.onlineFilepath);
                onlineFile.Close();

                //File.Delete(Constants.animationFilepath);
                //var animationFile = File.Create(Constants.animationFilepath);
                //animationFile.Close();

                if (onHeader == null)
                {
                    onHeader = new OnlineFileHeader(this);
                }
                headerSizeForCurrentSample = onHeader.MakeOnlineFileHeader();
                this.IsOnlineFileHeaderWritten = true;
                this.removeRemarkForOnlineFileHeaderWritten();

                this.WriteXMLLastOnlineHeader();
                clearGraphicsOfChangeRandE();

                //ukloni tacku R2/R4 kod svakog novog merenja jel se uvek
                //prvo korisit vrednost postavljena u opcijama
                window.Plotting.XMarkers9[0] = 0;
                window.Plotting.YMarkers9[0] = 0;
                window.Plotting.setPointAtGraphicR2R4(window.Plotting.XMarkers9[0], window.Plotting.YMarkers9[0]);

                //zatvori prozore sa ulaznim i prozor sa izlaznim podacima jel pocinjes da radis novo merenje
                if (onHeader.GeneralData != null)
                {
                    onHeader.GeneralData.Close();
                    //kad zatvoris jedan ulazni prozor zatvorio si i sve ostale prozore
                }
                if (resInterface != null)
                {
                    resInterface.Close();
                }
                if (plotting.Printscreen.NManualCalculation != null)
                {
                    if (plotting.Printscreen.NManualCalculation.IsVisible == true)
                    {
                        plotting.Printscreen.NManualCalculation.Close();
                    }
                }


                //zatvaranje ovog prozora predstavlja pocetak novog ciklusa kidanja
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnEnterNewSample_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
            
        }


        private void clearPlottingSixResults() 
        {
            try
            {
                plotting.ReH = 0;
                plotting.ReL = 0;
                plotting.Rm = 0;
                plotting.Fm = 0;
                plotting.Rp02RI = 0;
                plotting.A = 0;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void clearPlottingSixResults()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// obrada dogadjaja na klik zelenog dugmeta za pocetak pracenja online upisa 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStartSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (dataReader != null)
                {
                    dataReader.ClearData();
                }

                plotting.Printscreen.chbPrikaziOpcije.IsChecked = false;
                plotting.IsLuManualChanged = false;

                plotting.IsYungFirstTimeCalculate = true;
                plotting.IWantToBackFirstCalculateYung = false;
                plotting.IsEverT1T2orT3ManualSetted = false;
                clearPlottingSixResults();

                this.Plotting.IsSecondAndMoreLuManual = false;
                this.Plotting.NumberOfLuManual = 0;
                stopSampling = false;
                firstImeClickedAtGreenButton = true;
                //serialport.Open();//otvori ga samo jednom i to u konstruktoru
                //if (onHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                //{
                //    this.IsEkstenziometerUsed = false;
                //    dataReader.IsEkstenziometerUsed = false;
                //    if (simulationMode == false)
                //    {
                //        StartStream();
                //    }
                //}

                //if (onHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                //{
                //    this.IsEkstenziometerUsed = true;
                //    dataReader.IsEkstenziometerUsed = true;
                //    if (simulationMode == false)
                //    {
                //        StartStreamWithEkstenziometar();
                //    }
                //}

                if (onHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    this.IsEkstenziometerUsed = false;
                    dataReader.IsEkstenziometerUsed = false;
                    if (simulationMode == false)
                    {
                        StartStream();
                        StartStreamWithEkstenziometar();
                    }
                }

                if (onHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
                {
                    this.IsEkstenziometerUsed = true;
                    dataReader.IsEkstenziometerUsed = true;
                    if (simulationMode == false)
                    {
                        StartStreamWithEkstenziometar();
                        StartStream();
                    }
                }

                if (simulationMode == true)
                {
                    _LabJackWorkingTest.Start();
                    _serialPortTestTimer.Start();
                }



                if (isCurrentProgressWrittingInOnlineFile == true)
                {
                    MessageBox.Show("Online upis je još uvek u toku!");
                    btnStopSample.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    _microTimerMachineWorking.Stop();
                    return;
                }


                isCurrentSampleSaved = false;
                if (File.Exists(fpOnlineGlobal) == false)
                {
                    MessageBox.Show("Neispravna putanja do online fajla!");
                    return;
                }
                if (isOnlineMode == false)
                {
                    if (isOnlineFileHeaderWritten == false)
                    {
                        MessageBox.Show("Zaglavlje online fajla nije uneto!");
                        return;
                    }
                    isOnlineMode = true;
                    _BlinkingButtonTimer.Start();
                    //animationLines = new List<string>();
                    e2e4Lines = new List<string>();
                    //writeSettingsInAnimationFile();


                    //long interval = 1000;
                    long interval = 1;
                    // Set timer interval
                    _microTimer.Interval = interval;
                    _microTimerMachineWorking.Interval = interval;

                    // Ignore event if late by half the interval
                    _microTimer.IgnoreEventIfLateBy = interval / 2;
                    _microTimerMachineWorking.IgnoreEventIfLateBy = interval / 2;

                    // Start timer
                    _microTimer.Start();
                    _microTimerMachineWorking.Start();


                    menuIshowGraphicChangeOfParameters.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.MenuItem.ClickEvent));

                    lblOnlineStatus.Content = STATUS_TURNON;
                    lblOnlineStatus.Foreground = Brushes.Red;
                    lblOnlineStatus.FontWeight = FontWeights.ExtraBlack;

                    //ProcessStartInfo startInfo = new ProcessStartInfo(@"D:\___temporary\OnLineWriter\OnLineWriter\bin\Debug\OnLineWriter.exe");
                    //Process.Start(startInfo);


                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnStartSample_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        //upisi zabelezene pritiske uparene sa izduzenjem
        private List<double> preassure;
        private List<double> elongation;
        private MyPointCollection forLessThanOnePoints;
        public bool IsLessThanOne = false;
        /// <summary>
        /// ovo se poziva kada se klikne na crveno dugme za prekid kidanja uzorka
        /// korsnik ga je prekinuo a nije prekinuto jel vise nema upisa u online fajls
        /// </summary>
        private void callAfterManualStopingTearing() 
        {
            try
            {

                IsLessThanOne = false;
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (OptionsInOnlineMode.isAutoChecked)
                {
                    window.PrintScreen.HidePrintScreenMarkers();
                    window.tab_second.IsSelected = true;
                }


                //ako je prekinut rad sa praznim grafikom vrednost isAllValuesIsZero je true
                //grafik je prazan ako je 99 posto vrednosti za relativno izduzenje jednako 0
                bool isAllValuesIsZero = false;
                double numberOfZeroes = 0;
                double numberOfNonZeroes = 0;



                preassure = dataReader.PreassureInMPa;
                elongation = dataReader.RelativeElongation;

                foreach (var elon in elongation)
                {
                    if (elon > 0)
                    {
                        numberOfNonZeroes++;
                    }
                    else
                    {
                        numberOfZeroes++;
                    }
                }

                if (99 * numberOfNonZeroes <= numberOfZeroes && OptionsInOnlineMode.isCalibration == false)
                {
                    isAllValuesIsZero = true;
                }
              



                for (int i = 0; i < preassure.Count; i++)
                { 

                }

                //serialport.Close();//ovo nemoj nikad da otkomentarises
                if (simulationMode == false)
                {
                    _LabJackWorking.Stop();
                }
                if (simulationMode == true)
                {
                    _LabJackWorkingTest.Stop();
                    _serialPortTestTimer.Stop();
                    counterForPreassure = 0;
                    counterForElongation = 0;
                }
                _BlinkingButtonTimer.Stop();
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    btnStartSample.Background = Brushes.LawnGreen;
                    btnStartSample.Foreground = Brushes.White;
                }));
              
                historyOfMsgPreassure = new List<string>();

                plotting.Printscreen.DontSetMarkers = true;
                //double numberOfSamples;
                ////ovo vise ne pije vodu jel upola kidanja moze da se prekine kidanje
                //List<string> lines = File.ReadAllLines(fpOnlineGlobal).ToList();
                ////double numberOfPreassuresInMpa = dataReader.PreassureInMPa.Count;
                //numberOfSamples = lines.Count - onHeader.HeaderSize;
                ////numberOfPreassuresInMpa = numberOfPreassuresInMpa - onHeader.HeaderSize;
                //string text = File.ReadAllText(fpOnlineGlobal);
                //text = text.Replace(Constants.ONLINEFILEHEADER_BrojZapisa, Constants.ONLINEFILEHEADER_BrojZapisa + numberOfSamples.ToString());
                //File.WriteAllText(fpOnlineGlobal, text);
                cntTau = 0;
                counter = 0;
                _isStoppedOnlineSample = true;
                this.Dispatcher.Invoke(DispatcherPriority.Background, new System.Action(delegate()
                {
                    btnSampleData.IsEnabled = true;
                }));

                durationInmilisec = 0;
                changeOfPreassure = 0;
                changeOfElongation = 0;
                isOnlineFileHeaderWritten = false;

                
                if (isAllValuesIsZero == true)
                {
                    window.tab_third.IsSelected = true;
                    MessageBox.Show("Nema podataka za analizu !", "NEMA PODATAKA");
                    return;
                }

                if (window.PrintScreen != null)
                {
                    window.PrintScreen.btnSampleDataPrintMode.IsEnabled = true;
                    window.PrintScreen.IsPrintScreenEmpty = false;
                }

                if (ResultsInterface != null)
                {
                    ResultsInterface.tfRRm.Foreground = Brushes.Black;
                    ResultsInterface.tfF.Foreground = Brushes.Black;
                }

                if (ResultsInterface != null) 
                {
                    ResultsInterface.tfLu.IsReadOnly = false;
                }

                if (dataReader.RelativeElongation.Max() < 1.1)
                {
                    IsLessThanOne = true;

                   


                    forLessThanOnePoints = new MyPointCollection();
                    foreach (var point in points)
                    {
                        forLessThanOnePoints.Add(point);
                    }
                    window.PrintScreen.UpdatePrintScreen(forLessThanOnePoints);
                    window.Plotting.DeleteFittingPath();
                    window.Plotting.DeleteNonFittingPath();
                    window.Plotting.PointsOfFittingLine.Clear();
                    window.Plotting.points.Clear();
                    window.Plotting.PointsOfFittingLine = forLessThanOnePoints;
                    
                    double rmLoc = Double.MinValue;
                   
                    ResultsInterface = new ResultsInterface(this, window.Plotting, window.PrintScreen);
                    //window.Plotting.setRpmaxAndFmax(out rmLoc);
                    window.Plotting.Rm = dataReader.PreassureInMPa.Max();
                    window.Plotting.Rm = Math.Round(window.Plotting.Rm, 0);
                    window.Plotting.Lu = window.Plotting.L0 * (100 + dataReader.RelativeElongation.Max()) / 100;
                    window.Plotting.Lu = Math.Round(window.Plotting.Lu, 2);
                    window.Plotting.Fm = dataReader.ForceInKN.Max() * 1000;
                    window.Plotting.Fm = Math.Round(window.Plotting.Fm, 0);
                    window.Plotting.A = dataReader.RelativeElongation.Max();
                    window.Plotting.A = Math.Round(window.Plotting.A, 2);
                    if (ResultsInterface != null)
                    {
                        if (ResultsInterface != null)
                        {
                            ResultsInterface.tfLu.IsReadOnly = true;
                        }
                        //ResultsInterface.EmptyResultsInterface();
                        window.Plotting.tfRm.Text = window.Plotting.Rm.ToString();
                        window.Plotting.tfFm.Text = window.Plotting.Fm.ToString();
                        window.Plotting.tfReL.Text = string.Empty;
                        window.Plotting.tfReH.Text = string.Empty;
                        window.Plotting.tfA.Text = window.Plotting.A.ToString();
                        window.Plotting.tfRp02.Text = string.Empty;
                        ResultsInterface.tfLu.Text = window.Plotting.Lu.ToString();
                        ResultsInterface.tfRm.Text = window.Plotting.Rm.ToString();
                        ResultsInterface.tfFm.Text = window.Plotting.Fm.ToString();
                        ResultsInterface.tfA.Text = window.Plotting.A.ToString();
                        ResultsInterface.tfAt.Text = string.Empty;
                        ResultsInterface.tfAgt.Text = string.Empty;

                        
                        if (ResultsInterface.chbRt05.IsChecked == false)
                        {
                            ResultsInterface.tfRt05.Text = string.Empty;
                        }
                        //window.Plotting.btnPlottingModeClick();
                        window.tab_third.IsSelected = true;

                        
                        window.PrintScreen.btnSampleDataPrintMode.IsEnabled = true;

                        ResultsInterface.chbRp02.IsChecked = false;
                        ResultsInterface.chbRt05.IsChecked = false;
                        ResultsInterface.chbReL.IsChecked = false;
                        ResultsInterface.chbReH.IsChecked = false;
                        ResultsInterface.chbn.IsChecked = false;
                        ResultsInterface.chbE.IsChecked = false;
                        ResultsInterface.chbe2.IsChecked = false;
                        ResultsInterface.chbe4.IsChecked = false;
                        ResultsInterface.chbRe.IsChecked = false;
                        ResultsInterface.tfRp02.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfRt05.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfReL.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfReH.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfn.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfE.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfE2.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfE4.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfRe.Foreground = Brushes.AliceBlue;

                        ResultsInterface.tfRRm.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfF.Foreground = Brushes.AliceBlue;
                        ResultsInterface.tfRRm.Text = string.Empty;
                        ResultsInterface.tfF.Text = string.Empty;
                        LastInputOutputSavedData.tfRRm_ResultsInterface = string.Empty;
                        LastInputOutputSavedData.tfF_ResultsInterface = string.Empty;

                        ResultsInterface.Show();
                    }



                    //clear process parameters
                    tfForceInN.Text = Constants.ZERO;
                    if (OptionsInOnlineMode.calculateMaxForceForTf == true)
                    {
                        tfMaxForceInKN.Text = Constants.ZERO;
                    }
                    //tfElongationForMaxForce.Text = Constants.ZERO;
                    tfPreassureInMPa.Text = Constants.ZERO;
                    tfElongationInMM.Text = Constants.ZERO;
                    tfElongationInProcent.Text = Constants.ZERO;
                    tfDeltaPreassure.Text = Constants.ZERO;
                    tfMaxDeltaPreassure.Text = Constants.ZERO;
                    //tfElongationMaxPreassure.Text = Constants.ZERO;
                    tfDeltaElongation.Text = Constants.ZERO;
                    tfMaxDeltaElongation.Text = Constants.ZERO;
                    //tfPreassureMaxElongation.Text = Constants.ZERO;

                    tfRemarkOnlineFileHeaderWritten.Text = Constants.ONLINEFILE_NOTENTERSAMPLEDATA;
                    tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Red;

                    //dataReaderForElongationLessThanOne = new DataReader(dataReader.Filepath);
                    //dataReaderForElongationLessThanOne = dataReader;
                    createOnlineGraphics();
                    //dataReader.ClearData();

                    LastInputOutputSavedData.GetData();
                    List<string> inputOutputLinesLessThanOne = new List<string>();
                    //setResultsInterface(string.Empty, string.Empty);

                    this.WriteXMLLastOnlineHeader();
                    this.WriteXMLLastResultsInterface();
                    LastInputOutputSavedData.GetData();
                    getInputOutputLines(ref inputOutputLinesLessThanOne);

                    //if (File.Exists(Constants.inputOutputFilepath) == true)
                    if (File.Exists(Properties.Settings.Default.inputOutputFilepath) == true)
                    {
                        //File.WriteAllLines(Constants.inputOutputFilepath, inputOutputLines);
                        File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, inputOutputLinesLessThanOne);
                    }
                    else
                    {
                        //File.AppendAllLines(Constants.inputOutputFilepath, inputOutputLines);
                        File.AppendAllLines(Properties.Settings.Default.inputOutputFilepath, inputOutputLinesLessThanOne);
                        // MessageBox.Show("Ne postoji ulazno - izlazni fajl za online upis!");
                    }


                    return;
                }



                //if (File.Exists(Constants.animationFilepath) == true)
                //{
                //    //File.AppendAllLines(Constants.animationFilepath, animationLines);
                //    //find and replace some text in animation anim extension file using c#
                //    double refreshAnimTime = 20;
                //    if (numberOfSamplesInFirstSecond > 0)
                //    {
                //        refreshAnimTime = 1000 / numberOfSamplesInFirstSecond;
                //    }
                //    firstSecondDurationInmilisec = 0;
                //    numberOfSamplesInFirstSecond = 0;
                //    string textAnim = File.ReadAllText(Constants.animationFilepath);
                //    textAnim = textAnim.Replace(Constants.ANIMATIONFILEHEADER_refreshAnimationTime, Constants.ANIMATIONFILEHEADER_refreshAnimationTime + refreshAnimTime);
                //    textAnim = textAnim.Replace(Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure, Constants.ANIMATIONFILEHEADER_maxChangeOfPreassure + maxChangeOfPreasure + "~" + changeOfElongationForMaxPreassure);
                //    textAnim = textAnim.Replace(Constants.ANIMATIONFILEHEADER_resolution, Constants.ANIMATIONFILEHEADER_resolution + OptionsInOnlineMode.Resolution);

                //    File.WriteAllText(Constants.animationFilepath, textAnim);
                //}
                //else
                //{
                //    MessageBox.Show("Ne postoji animacijski fajl!");
                //}
                //if (File.Exists(Constants.e2e4Filepath) == true)
                if (File.Exists(Properties.Settings.Default.e2e4Filepath) == true)
                {
                    //File.WriteAllLines(Constants.e2e4Filepath, e2e4Lines);
                    File.WriteAllLines(Properties.Settings.Default.e2e4Filepath, e2e4Lines);
                }
                else
                {
                    //File.AppendAllLines(Constants.e2e4Filepath, e2e4Lines);
                    File.AppendAllLines(Properties.Settings.Default.e2e4Filepath, e2e4Lines);
                    // MessageBox.Show("Ne postoji e2e4 fajl za online upis!");
                }

                currCounterWhoDetermitedOneSecond = 0;
                maxChangeOfPreasure = 0.0;
                changeOfElongationForMaxPreassure = 0.0;
                maxChangeOfElongation = 0.0;
                maxForceInKN = 0.0;
                _microTimer.Stop();
                isClickedStopSample = false;

                createOnlineGraphics();

                howManyMilisecondsNoWriting = 0;




                
                if (window != null)
                {
                    window.IsOnlineModeFinished = true;
                }
                ////predji na tab tri
                ////ali prvo postavi razmeru print screen grafika
                //plotting.Printscreen.plotterPrint.Viewport.AutoFitToView = true;
                //ViewportAxesRangeRestriction restrPrint = new ViewportAxesRangeRestriction();
                //restrPrint.YRange = new DisplayRange(-0.5, /*OptionsInPlottingMode.yRange*/ 1.1 * maxPreassure);
                //restrPrint.XRange = new DisplayRange(-0.5, /*OptionsInPlottingMode.xRange*/ 1.1 * maxElongation);

                //plotting.Printscreen.plotterPrint.Viewport.Restrictions.Add(restrPrint);


                //plotting.rbtnManual.IsChecked = true;
                //double y = 1.1 * maxPreassure;
                //double x = 1.1 * maxElongation;
                //plotting.tfRatioForce.Text = y.ToString();
                //plotting.tfRatioElongation.Text = x.ToString();
                //plotting.ChangePlottingRatioForce();
                //plotting.ChangePlottingRatioElongation();

                //maxPreassure = 0.0;
                //maxElongation = 0.0;

                if (OptionsInOnlineMode.isManualChecked)
                {
                    window.PrintScreen.HidePrintScreenMarkers();
                    window.tab_third.IsSelected = true;
                }
               

                if (dataReader.PreassureInMPa.Count == 0)
                {
                    //MessageBox.Show("Online fajl je prazan!");
                    tfRemarkOnlineFileHeaderWritten.Text = Constants.ONLINEFILE_NOTENTERSAMPLEDATA;
                    tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Red;
                    btnStopSample.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    return;
                }

                if (dataReader.PreassureInMPa.Count > 0)
                {
                    //MessageOnlineModeYesNo mYesNo = new MessageOnlineModeYesNo();
                    //mYesNo.ShowDialog();
                    //if (mYesNo.IsYesChosen)
                    //{
                    //    SaveDialogForm saveDialog = new SaveDialogForm();
                    //    SaveDialogProperty = saveDialog;
                    //    string currFileNamePath = saveDialog.saveFileDialog1.FileName;

                  
                    //    window.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                    //    window.Plotting.tfFilepathPlottingKeyDown();

                    //    window.Plotting.drawFittingGraphic(currFileNamePath);//sluzi samo za racunanje datareadera

                    //    window.Plotting.SetRpmaxAndFmaxReHANDReL();

                    //    window.Plotting.SetRatioAndCalibrationAfterOnlineWriting();

                    //    //window.Plotting.drawFittingGraphic(currFileNamePath);//iscrtavanje grafika
                    //    window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    //    if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                    //    {
                    //        //drawFittingGraphic();
                    //        window.Plotting.DeleteOfflineModeOnly();
                    //        window.Plotting.drawFittingGraphic(currFileNamePath);

                    //    }
                    //    else
                    //    {
                    //        window.Plotting.drawFittingGraphic(currFileNamePath);
                    //    }
                    //}
                    //else
                    //{
                    saveUnsavedFile();
                    //string currFileNamePath = Constants.unsavedFilepath;
                    string currFileNamePath = Properties.Settings.Default.unsavedFilepath;


                    window.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                    window.Plotting.tfFilepathPlottingKeyDown();

                    double lastRecordedRelativeElongation = 0;
                    int numberofRelativeElongationData = dataReader.RelativeElongation.Count - 1;
                    for (int i = numberofRelativeElongationData; i >= 0; i--)
                    {
                        if (dataReader.RelativeElongation[i] < 0)
                        {
                            dataReader.RelativeElongation.RemoveAt(i);
                        } 
                    }
                    lastRecordedRelativeElongation = dataReader.RelativeElongation.Last(); 


                    if (lastRecordedRelativeElongation > minPossibleValueForMaxElongation)
                    {
                        numberOfCallForDrawFitting++;
                        
                        window.Plotting.drawFittingGraphic(currFileNamePath);//sluzi samo za racunanje datareadera
                       
                        
                        window.Plotting.SetRpmaxAndFmaxReHANDReL();

                        window.Plotting.SetRatioAndCalibrationAfterOnlineWriting();

                        //window.Plotting.drawFittingGraphic(currFileNamePath);//iscrtavanje grafika 
                        numberOfCallForDrawFitting++;
                        
                        window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                        plotting.Printscreen.DontSetMarkers = false;
                        if (OptionsInPlottingMode.isShowOriginalDataGraphic == false)
                        {
                            //drawFittingGraphic();
                            window.Plotting.DeleteOfflineModeOnly();
                            numberOfCallForDrawFitting++;
                            window.Plotting.drawFittingGraphic(currFileNamePath, numberOfCallForDrawFitting);
                        }
                        else
                        {
                            numberOfCallForDrawFitting++;
                            window.Plotting.drawFittingGraphic(currFileNamePath, numberOfCallForDrawFitting);
                        }
                     } // if (dataReader.RelativeElongation.Last() > minPossibleValueForMaxElongation)




                    //}

                    
                   
                }

              

                //btnOnlineMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));


                //ovo treba da usledi kada se bude dolazilo do klika na dugme Novo merenje
                //File.Delete(Constants.onlineFilepath);

                //var onlineFile = File.Create(Constants.onlineFilepath);
                //onlineFile.Close();

                //dataReader.ClearData();

                //ovo treba da usledi kada se bude dolazilo do klika na dugme Novo merenje
                //File.Delete(Constants.animationFilepath);
                //var animationFile = File.Create(Constants.animationFilepath);
                //animationFile.Close();

                //clear process parameters
                tfForceInN.Text = Constants.ZERO;
                if (OptionsInOnlineMode.calculateMaxForceForTf == true)
                {
                    tfMaxForceInKN.Text = Constants.ZERO;
                }
                //tfElongationForMaxForce.Text = Constants.ZERO;
                tfPreassureInMPa.Text = Constants.ZERO;
                tfElongationInMM.Text = Constants.ZERO;
                tfElongationInProcent.Text = Constants.ZERO;
                tfDeltaPreassure.Text = Constants.ZERO;
                tfMaxDeltaPreassure.Text = Constants.ZERO;
                //tfElongationMaxPreassure.Text = Constants.ZERO;
                tfDeltaElongation.Text = Constants.ZERO;
                tfMaxDeltaElongation.Text = Constants.ZERO;
                //tfPreassureMaxElongation.Text = Constants.ZERO;

                tfRemarkOnlineFileHeaderWritten.Text = Constants.ONLINEFILE_NOTENTERSAMPLEDATA;
                tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Red;




               
                

                //doFittingChangeOfRandEGraphics(OptionsInOnlineMode.E2E4Border, window.Plotting.XTranslateAmountFittingMode);


                calculateRmaxWithPoint();
                //calculateE2WithPoint();
                //window.Plotting.setE2E4MinMax();

                loadFirstAfterRunResultsInterface();
                List<string> inputOutputLines = new List<string>();
                setResultsInterface(string.Empty, string.Empty);
               
                this.WriteXMLLastOnlineHeader();
                this.WriteXMLLastResultsInterface();
                LastInputOutputSavedData.GetData();
                getInputOutputLines(ref inputOutputLines);
               

                //if (File.Exists(Constants.inputOutputFilepath) == true)
                if (File.Exists(Properties.Settings.Default.inputOutputFilepath) == true)
                {
                    //File.WriteAllLines(Constants.inputOutputFilepath, inputOutputLines);
                    File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, inputOutputLines);
                }
                else
                {
                    //File.AppendAllLines(Constants.inputOutputFilepath, inputOutputLines);
                    File.AppendAllLines(Properties.Settings.Default.inputOutputFilepath, inputOutputLines);
                    // MessageBox.Show("Ne postoji ulazno - izlazni fajl za online upis!");
                }
                saveUnsavedInputOutput();
                saveUnsavedPreassureElongation(ref preassure, ref elongation);
                writeSampleReportOnlineXml();

                ////sredi grafike za promenu napona i izduzenja tj u pocetku gde god je vrednost napona manja od Rm/2
                //int rmIndex = window.Plotting.GetRpmaxIndex();
                ////double rm = dataReader.PreassureInMPa[rmIndex];
                //double rm = window.Plotting.DataReader.PreassureInMPa[rmIndex];
                //int ii;
                //for (ii = 0; ii < vXY.PointsChangeOfR.Count; ii++)
                //{
                //    if (vXY.ArrayROfEndOfInterval[ii] < rm / 2)
                //    {

                //    }
                //    else
                //    {
                //        break;
                //    }

                //}
                //vXY.FittingChangeOfRGraphic(ii + 1);
                //vXY.FittingChangeOfEGraphic(ii + 1);


                dataReader.ClearData();

                

                howManyMilisecondsNoWriting = 0;

                if (OptionsInOnlineMode.isManualChecked)
                {
                    //i na kraju
                    //sada selektujemo tab3 da bi se odmah prikazao print screen izgleda fitovanog grafika
                    window.tab_third.IsSelected = true;
                }
                else
                {
                    window.tab_second.IsSelected = true;
                }
                //this.btnSampleData.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                IsClickedSampleDataAtPrintMode = true;
                
                //this.ResultsInterface.Show();

                //vise se ova metoda ne koristi
                //showInputOutputDataInFocus();
                this.ResultsInterface.tfLu.SelectAll();
                this.ResultsInterface.tfLu.Focus();
                window.PrintScreen.IsPrintScreenEmpty = false;
                window.PrintScreen.btnSampleDataPrintMode.IsEnabled = true;
                window.PrintScreen.ShowPrintScreenMarkers();

                if (chbStartSampleShowChangedPar.IsChecked == true)
                {
                    window.PrintScreen.chbChangeOfRAndE.IsEnabled = true;
                }
                else
                {
                    window.PrintScreen.chbChangeOfRAndE.IsEnabled = false;
                }

                /*********** Ovo je prebaceno u drawFittingGraphic metodu **********/
                //doFittingChangeOfRandEGraphics();

                //if (this.VXY != null)
                //{
                //    //this.VXY.PointsChangeOfRFitting = e2e4CalculationAfterManualFitting.PointsChangeOfRFitting;
                //    this.VXY.CreateChangeOfRGraphic_Fitting(VXY.PointsChangeOfRFitting);
                //    this.VXY.DeleteChangeOfRGraphic();
                //    //this.VXY.PointsChangeOfEFitting = VXY.PointsChangeOfEFitting;
                //    this.VXY.CreateChangeOfEGraphic_Fitting(VXY.PointsChangeOfEFitting);
                //    this.VXY.DeleteChangeOfEGraphic();
                //}
                /*********** Ovo je prebaceno u drawFittingGraphic metodu **********/

                if (OptionsInOnlineMode.isAutoChecked)
                {
                    window.tab_second.IsSelected = true;
                }

                if (chbStartSampleShowChangedPar.IsChecked == true)
                {
                    window.PrintScreen.chbChangeOfRAndE.IsEnabled = true;
                    //window.PrintScreen.chbChangeOfRAndE.IsChecked = true;
                }
                else
                {
                    window.PrintScreen.chbChangeOfRAndE.IsChecked = false;
                    window.PrintScreen.chbChangeOfRAndE.IsEnabled = false;
                }
                //ovo mora da bi bili sigurni da posle zavrsetka online moda dodje do dobrog postavljanja automatskog tacki fitovanja
                plotting.OptionsPlotting.tabforT3();
                plotting.SetManualPointsToAutoPointsValue();
                plotting.SetPointAtGraphicX1_withXY(OptionsInPlottingMode.pointManualX1, OptionsInPlottingMode.pointManualY1);
                plotting.SetPointAtGraphicX2_withXY(OptionsInPlottingMode.pointManualX2, OptionsInPlottingMode.pointManualY2);
                plotting.SetPointAtGraphicX3_withXY(OptionsInPlottingMode.pointManualX3, OptionsInPlottingMode.pointManualY3);

                plotting.btnPlottingModeClick();

                plotting.chbShowChangeOfRAndEe.IsChecked = true;
                resInterface.chbRt05.IsChecked = false;
                resInterface.tfRt05.Foreground = Brushes.AliceBlue;
                resInterface.chbE.IsChecked = false;
                resInterface.tfE.Foreground = Brushes.AliceBlue;

                showInputDataWindowsAfterTearing();
                resInterface.SetLastCheckedRadioButton();





                LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface = plotting.OnlineModeInstance.RmaxGlobal.ToString();
                plotting.OnlineModeInstance.ResultsInterface.tfRmax.Text = LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface;
                LastInputOutputSavedData.e2Max_ResultsInterface = plotting.E2e4CalculationAfterManualFitting.ArrayE2Interval.Max().ToString();
                plotting.OnlineModeInstance.ResultsInterface.tfE2.Text = LastInputOutputSavedData.e2Max_ResultsInterface;
                LastInputOutputSavedData.e4Max_ResultsInterface = plotting.E2e4CalculationAfterManualFitting.ArrayE4Interval.Max().ToString();
                plotting.OnlineModeInstance.ResultsInterface.tfE4.Text = LastInputOutputSavedData.e4Max_ResultsInterface;


                return;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "callAfterManualStopingTearing()");
                //Logger.WriteNode(ex.Message.ToString() + " {callAfterManualStopingTearing}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void callAfterManualStopingTearing()}", System.DateTime.Now);
                //MessageBox.Show("Kidanje je prekinuto na samom početku!", "Nema kidanja");
                saveUnsavedFile();

                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                window.IsOnlineModeFinished = true;

                //clear process parameters
                tfForceInN.Text = Constants.ZERO;
                if (OptionsInOnlineMode.calculateMaxForceForTf == true)
                {
                    tfMaxForceInKN.Text = Constants.ZERO;
                }
                //tfElongationForMaxForce.Text = Constants.ZERO;
                tfPreassureInMPa.Text = Constants.ZERO;
                tfElongationInMM.Text = Constants.ZERO;
                tfElongationInProcent.Text = Constants.ZERO;
                tfDeltaPreassure.Text = Constants.ZERO;
                tfMaxDeltaPreassure.Text = Constants.ZERO;
                //tfElongationMaxPreassure.Text = Constants.ZERO;
                tfDeltaElongation.Text = Constants.ZERO;
                tfMaxDeltaElongation.Text = Constants.ZERO;
                //tfPreassureMaxElongation.Text = Constants.ZERO;

                tfRemarkOnlineFileHeaderWritten.Text = Constants.ONLINEFILE_NOTENTERSAMPLEDATA;
                tfRemarkOnlineFileHeaderWritten.Foreground = Brushes.Red;



                //doFittingChangeOfRandEGraphics(OptionsInOnlineMode.E2E4Border, window.Plotting.XTranslateAmountFittingMode);

                calculateRmaxWithPoint();
                //calculateE2WithPoint();
                //window.Plotting.setE2E4MinMax();

                loadFirstAfterRunResultsInterface();
                setResultsInterface(string.Empty, string.Empty);
                showInputData();

           

                dataReader.ClearData();

              

                howManyMilisecondsNoWriting = 0;

                resInterface.chbRt05.IsChecked = false;
                resInterface.tfRt05.Foreground = Brushes.AliceBlue;

                return;
            }
        }

        private void showInputDataWindowsAfterTearing()
        {
            try
            {
                plotting.Printscreen.IsPrintScreenEmpty = false;

                this.ResultsInterface.Show();

                //ako se radi etaloniranje tada ne iskacu ulazni podaci jel nisu validni
                if (OptionsInOnlineMode.isCalibration == true)
                {
                    return;
                }
                if (plotting.A == 0)
                {
                    return;
                }

                if (Plotting.Printscreen.cmbInputWindow.SelectedIndex == 0)
                {
                    onHeader.GeneralData.Show();
                    onHeader.GeneralData.WindowStartupLocation = WindowStartupLocation.Manual;
                    onHeader.GeneralData.Left = onHeader.GeneralData.xconst_InPrintScreenMode;
                    onHeader.GeneralData.Top = onHeader.GeneralData.yconst_InPrintScreenMode;
                }
                else if (Plotting.Printscreen.cmbInputWindow.SelectedIndex == 1)
                {
                    onHeader.ConditionsOfTesting.Show();
                }
                else if (Plotting.Printscreen.cmbInputWindow.SelectedIndex == 2)
                {
                    onHeader.MaterialForTesting.Show();
                }
                else if (Plotting.Printscreen.cmbInputWindow.SelectedIndex == 3)
                {
                    onHeader.PositionOfTube.Show();
                }
                else if (Plotting.Printscreen.cmbInputWindow.SelectedIndex == 5)
                {
                    onHeader.RemarkOfTesting.Show();
                }

                if (chbStartSampleShowChangedPar.IsChecked == true)
                {
                    if (vXY.Height > 0)
                    {
                        vXY.Hide();
                        vXY.Show();
                    }
                }

                plotting.Printscreen.IsPrintScreenEmpty = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void showInputDataWindowsAfterTearing()}", System.DateTime.Now);
            }

        }

        /// <summary>
        /// pamte se za zadnji pokidani uzorak ulazni i izlazni podaci zadnje pokidanog uzorka
        /// </summary>
        /// <param name="refInputOutputLines"></param>
        public void getInputOutputLines(ref List<string> refInputOutputLines) 
        {
            try
            {
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                if (window == null)
                {
                    return;
                }

                //InputLines

                #region inputLines

                refInputOutputLines.Add("ULAZNI PODACI");
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_OPSTIPODACI);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_OPERATOR + "\t" + LastInputOutputSavedData.tfOperator_GeneralData);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_BRZBIZVESTAJA + "\t" + LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_BRUZORKA + "\t" + LastInputOutputSavedData.tfBrUzorka_GeneralData);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_SARZA + "\t" + LastInputOutputSavedData.tfSarza_GeneralData);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_RADNINALOG + "\t" + LastInputOutputSavedData.tfRadniNalog_GeneralData);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NARUCILAC + "\t" + LastInputOutputSavedData.tfNarucilac_GeneralData);

                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_MATERIJALISPITIVANJA);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_PROIZVODJAC + "\t" + LastInputOutputSavedData.tfProizvodjac_MaterialForTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_DOBAVLJAC + "\t" + LastInputOutputSavedData.tfDobavljac_MaterialForTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_POLAZNIKVALITET + "\t" + LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NAZIVNADEBLJINA + "\t" + LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting);
                if (LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "\t" + Constants.VALJANI);
                }
                if (LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "\t" + Constants.VUČENI);
                }
                if (LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "\t" + Constants.KOVANI);
                }
                if (LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "\t" + Constants.LIVENI);
                }

                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_USLOVIISPITIVANJA);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_STANDARD + "\t" + LastInputOutputSavedData.tfStandard_ConditionsOfTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_METODA + "\t" + LastInputOutputSavedData.tfMetoda_ConditionsOfTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_STANDARDZAN + "\t" + LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_TEMPERATURA + "\t" + LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_MASINA + "\t" + LastInputOutputSavedData.tfMasina_ConditionsOfTesting);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_OPSEGMASINE + "\t" + LastInputOutputSavedData.tfBegOpsegMasine_ConditionsOfTesting);
                if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_EKSTENZIOMETAR + "\t" + Constants.DA);
                }
                if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_EKSTENZIOMETAR + "\t" + Constants.NE);
                }

                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_EPRUVETA);
                if (LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_EPRUVETAOBLIK + "\t" + Constants.OBRADJENA);
                }
                if (LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_EPRUVETAOBLIK + "\t" + Constants.NEOBRADJENA);
                }
                if (LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_TIP + "\t" + Constants.NEPROPORCIONALNA);
                }
                if (LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_TIP + "\t" + Constants.PROPORCIONALNA);
                }
                if (LastInputOutputSavedData.rbtnEpvK1.Equals("True") && LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_K + "\t" + Constants.K1);
                }
                if (LastInputOutputSavedData.rbtnEpvK2.Equals("True") && LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_K + "\t" + Constants.K2);
                }

                //LastInputOutputSavedData.Find_au();
                if (resInterface.tfAGlobal != null)
                {
                    LastInputOutputSavedData.au = resInterface.tfAGlobal.Text;
                }
                //LastInputOutputSavedData.Find_bu();
                if (resInterface.tfBGlobal != null)
                {
                    LastInputOutputSavedData.bu = resInterface.tfBGlobal.Text;
                }
                //LastInputOutputSavedData.Find_Du();
                if (resInterface.tfDGlobal != null)
                {
                    LastInputOutputSavedData.Du = resInterface.tfDGlobal.Text;
                    resInterface.tfDGlobal.IsReadOnly = false;
                }
                //LastInputOutputSavedData.Find_du();
                LastInputOutputSavedData.du = string.Empty;
                if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "\t" + Constants.PRAVOUGAONA);
                    refInputOutputLines.Add(Constants.a0 + "\t" + LastInputOutputSavedData.a0Pravougaona);
                    refInputOutputLines.Add(Constants.au + "\t" + LastInputOutputSavedData.au);
                    refInputOutputLines.Add(Constants.b0 + "\t" + LastInputOutputSavedData.b0Pravougaona);
                    refInputOutputLines.Add(Constants.bu + "\t" + LastInputOutputSavedData.bu);
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "\t" + Constants.KRUZNA);
                    refInputOutputLines.Add(Constants.D0 + "\t" + LastInputOutputSavedData.D0Kruzna);
                    refInputOutputLines.Add(Constants.Du + "\t" + LastInputOutputSavedData.Du);
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "\t" + Constants.CEVASTA);
                    refInputOutputLines.Add(Constants.D0 + "\t" + LastInputOutputSavedData.D0Cevasta);
                    refInputOutputLines.Add(Constants.Du + "\t" + LastInputOutputSavedData.Du);
                    refInputOutputLines.Add(Constants.a0 + "\t" + LastInputOutputSavedData.a0Cevasta);
                    refInputOutputLines.Add(Constants.au + "\t" + LastInputOutputSavedData.au);
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "\t" + Constants.DEOCEVI);
                    refInputOutputLines.Add(Constants.D0 + "\t" + LastInputOutputSavedData.D0Deocev);
                    refInputOutputLines.Add(Constants.Du + "\t" + LastInputOutputSavedData.Du);
                    refInputOutputLines.Add(Constants.a0 + "\t" + LastInputOutputSavedData.a0Deocev);
                    refInputOutputLines.Add(Constants.au + "\t" + LastInputOutputSavedData.au);
                    refInputOutputLines.Add(Constants.b0 + "\t" + LastInputOutputSavedData.b0Deocev);
                    refInputOutputLines.Add(Constants.bu + "\t" + LastInputOutputSavedData.bu);
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "\t" + Constants.SESTAUGAONA);
                    refInputOutputLines.Add(Constants.d0 + "\t" + LastInputOutputSavedData.d0Sestaugaona);
                    refInputOutputLines.Add(Constants.du + "\t" + LastInputOutputSavedData.du);
                }

                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_S0 + "\t" + LastInputOutputSavedData.tfS0);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_L0 + "\t" + LastInputOutputSavedData.tfL0);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_LC + "\t" + LastInputOutputSavedData.tfLc);



                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_POLOZAJEPRUVETE);

                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_PRAVACVALJANJA + "\t" + LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_SIRINATRAKE + "\t" + LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube);
                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_DUZINATRAKE + "\t" + LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube);



                refInputOutputLines.Add(Constants.ONLINEFILEHEADER_NAPOMENA + "\t" + LastInputOutputSavedData.rtfNapomena_RemarkOfTesting);

                #endregion

                //OutputLines

                #region outputLines

                refInputOutputLines.Add("IZLAZNI PODACI");
                refInputOutputLines.Add(Constants.Lu + "\t" + LastInputOutputSavedData.tfLu_ResultsInterface + "\t" + Constants.mm);
                if (LastInputOutputSavedData.rbtnRp02_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.rbtnRp02 + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.rbtnRp02 + "\t" + "False");
                }
                if (LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.chbRp02 + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.chbRp02 + "\t" + "False");
                }
                refInputOutputLines.Add(Constants.Rp02 + "\t" + LastInputOutputSavedData.tfRp02_ResultsInterface + "\t" + Constants.MPa);

                if (LastInputOutputSavedData.rbtnRt05_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.rbtnRt05 + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.rbtnRt05 + "\t" + "False");
                }
                if (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.chbRt05 + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.chbRt05 + "\t" + "False");
                }
                refInputOutputLines.Add(Constants.Rt05 + "\t" + LastInputOutputSavedData.tfRt05_ResultsInterface + "\t" + Constants.MPa);

                if (LastInputOutputSavedData.rbtnReL_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.rbtnReL + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.rbtnReL + "\t" + "False");
                }
                if (LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.chbReL + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.chbReL + "\t" + "False");
                }
                refInputOutputLines.Add(Constants.ReL + "\t" + LastInputOutputSavedData.tfReL_ResultsInterface + "\t" + Constants.MPa);

                if (LastInputOutputSavedData.rbtnReH_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.rbtnReH + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.rbtnReH + "\t" + "False");
                }
                if (LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.chbReH + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.chbReH + "\t" + "False");
                }
                refInputOutputLines.Add(Constants.ReH + "\t" + LastInputOutputSavedData.tfReH_ResultsInterface + "\t" + Constants.MPa);

                refInputOutputLines.Add(Constants.Rm + "\t" + LastInputOutputSavedData.tfRm_ResultsInterface + "\t" + Constants.MPa);
                refInputOutputLines.Add(Constants.R_Rm + "\t" + LastInputOutputSavedData.tfRRm_ResultsInterface);
                refInputOutputLines.Add(Constants.F + "\t" + LastInputOutputSavedData.tfF_ResultsInterface);
                refInputOutputLines.Add(Constants.Fm + "\t" + LastInputOutputSavedData.tfFm_ResultsInterface);
                refInputOutputLines.Add(Constants.Ag + "\t" + LastInputOutputSavedData.tfAg_ResultsInterface);
                refInputOutputLines.Add(Constants.Agt + "\t" + LastInputOutputSavedData.tfAgt_ResultsInterface);
                refInputOutputLines.Add(Constants.A + "\t" + LastInputOutputSavedData.tfA_ResultsInterface);
                refInputOutputLines.Add(Constants.At + "\t" + LastInputOutputSavedData.tfAt_ResultsInterface);
                refInputOutputLines.Add(Constants.Su + "\t" + LastInputOutputSavedData.tfSu_ResultsInterface + "\t" + Constants.mm2);
                refInputOutputLines.Add(Constants.Z + "\t" + LastInputOutputSavedData.tfZ_ResultsInterface);
                if (LastInputOutputSavedData.chbn_ResultsInterface.Equals("True"))
                {
                    refInputOutputLines.Add(Constants.chbn + "\t" + "True");
                }
                else
                {
                    refInputOutputLines.Add(Constants.chbn + "\t" + "False");
                }
                refInputOutputLines.Add(Constants.n + "\t" + LastInputOutputSavedData.tfn_ResultsInterface);

                if (OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters < 1000)
                {
                    if (LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbRmax + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbRmax + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.Rmax + "\t" + LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface + "\t" + Constants.MPa2 + OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters + Constants.ms);

                    refInputOutputLines.Add(Constants.XML_isE2E4BorderSelected + "\t" + LastInputOutputSavedData.isE2E4BorderSelected);

                    if (LastInputOutputSavedData.chbe2_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbe2 + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbe2 + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.eR2 + "\t" + LastInputOutputSavedData.e2Min_ResultsInterface + " - " + LastInputOutputSavedData.e2Max_ResultsInterface + "\t" + Constants.jedan + OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters + Constants.ms);
                    if (LastInputOutputSavedData.chbe4_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbe4 + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbe4 + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.eR4 + "\t" + LastInputOutputSavedData.e4Min_ResultsInterface + " - " + LastInputOutputSavedData.e4Max_ResultsInterface + "\t" + Constants.jedan + OptionsInOnlineMode.timeIntervalForCalculationOfChangedParameters + Constants.ms);
                }
                else
                {
                    if (LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbRmax + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbRmax + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.Rmax + "\t" + LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface + "\t" + Constants.MPa_1000ms);
                    if (LastInputOutputSavedData.chbe2_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbe2 + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbe2 + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.eR2 + "\t" + LastInputOutputSavedData.e2Min_ResultsInterface + " - " + LastInputOutputSavedData.e2Max_ResultsInterface + "\t" + Constants.secNaMinusJedan);
                    if (LastInputOutputSavedData.chbe4_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbe4 + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbe4 + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.eR4 + "\t" + LastInputOutputSavedData.e4Min_ResultsInterface + " - " + LastInputOutputSavedData.e4Max_ResultsInterface + "\t" + Constants.secNaMinusJedan);

                    if (LastInputOutputSavedData.chbRe_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbRe + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbRe + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.Re + "\t" + LastInputOutputSavedData.Re_ResultsInterface + Constants.MPa);

                    if (LastInputOutputSavedData.chbE_ResultsInterface.Equals("True"))
                    {
                        refInputOutputLines.Add(Constants.chbE + "\t" + "True");
                    }
                    else
                    {
                        refInputOutputLines.Add(Constants.chbE + "\t" + "False");
                    }
                    refInputOutputLines.Add(Constants.E + "\t" + LastInputOutputSavedData.E_ResultsInterface + Constants.GPa);

                }


                //pre nego sto upises sadzaj na osnovu kog ce se postaviti dinamicki deo izlaznog interfejsa proveri da li je bila cekirana pravougaona, kruzna ili neka treca vrsta epruvete
                if (this != null)
                {
                    //MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                    if (this.OnHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
                    {
                        if ((this.ResultsInterface != null))
                        {
                            //this.ResultsInterface.IsRectangle = true;
                            window.Plotting.IsRectangle = true;
                            //this.ResultsInterface.IsCircle = false;
                            window.Plotting.IsCircle = false;
                        }
                    }
                    else if (this.OnHeader.rbtnEpvVrstaKruzni.IsChecked == true)
                    {
                        if (this.ResultsInterface != null)
                        {
                            //this.ResultsInterface.IsCircle = true;
                            window.Plotting.IsCircle = true;
                            //this.ResultsInterface.IsRectangle = false;
                            window.Plotting.IsRectangle = false;
                        }
                    }
                    else
                    {
                        if (this.ResultsInterface != null)
                        {
                            //this.ResultsInterface.IsRectangle = false;
                            //isRectangle = false;
                            //this.ResultsInterface.IsCircle = false;
                            //isCircle = false;
                        }
                    }
                }

                if (window.Plotting.IsRectangle == true)
                {
                    refInputOutputLines.Add(Constants.au + "\t" + LastInputOutputSavedData.au + "\t" + Constants.mm);
                    refInputOutputLines.Add(Constants.bu + "\t" + LastInputOutputSavedData.bu + "\t" + Constants.mm);
                }


                if (window.Plotting.IsCircle == true)
                {
                    refInputOutputLines.Add(Constants.Du2 + "\t" + LastInputOutputSavedData.Du + "\t" + Constants.mm);
                }

                #endregion
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void getInputOutputLines(ref List<string> refInputOutputLines)}", System.DateTime.Now);
            }

        }
        /// <summary>
        /// operacije koje se moraju odraditi po zavrsenom online upisu
        /// </summary>
        private void StopSample_EndOfOnlineWritting() 
        {
            try
            {
                if (isOnlineMode == true)
                {
                    isOnlineMode = false;

                    isClickedStopSample = true;

                    // Stop timer
                    _microTimer.Stop();
                    _microTimer.Abort();
                    callAfterManualStopingTearing();


                    lblOnlineStatus.Content = STATUS_TURNOFF;
                    lblOnlineStatus.Foreground = Brushes.Black;
                    lblOnlineStatus.FontWeight = FontWeights.Normal;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void StopSample_EndOfOnlineWritting()}", System.DateTime.Now);
            }
        }

        /// <summary>
        /// obrada klika na crveno dugme kada korisnik zeli da prekine kidanje  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStopSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (isCurrentProgressWrittingInOnlineFile == false)
                //{
                //    MessageBox.Show("Online upis nije u toku!");
                //    return;
                //}
                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                window.IsThisFirstSample = false;

                if (isOnlineMode == true)
                {
                    isOnlineMode = false;

                    isClickedStopSample = true;


                    // Stop timer
                    _microTimer.Stop();
                    _microTimer.Abort();


                    _isStoppedOnlineSample = true;

                    callAfterManualStopingTearing();


                    lblOnlineStatus.Content = STATUS_TURNOFF;
                    lblOnlineStatus.Foreground = Brushes.Black;
                    lblOnlineStatus.FontWeight = FontWeights.Normal;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnStopSample_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        /// u<summary>
        /// ukoliko u zbirnom izvestaju zapamcen bio prethodno uzorak koji se pamti samo ga pronadji i promeni u xml fajlu
        /// ako si ga pronasao i promenio vrednost promenljive isSampleAlreadyExist postavi na true, u suprotnom na false
        /// </summary>
        /// <param name="sumRecord"></param>
        /// <param name="isSampleAlreadyExist"></param>
        private void modifyExistingSampleInSumReport(SumReportRecord sumRecord, out bool isSampleAlreadyExist) 
        {
            try
            {
                isSampleAlreadyExist = false;

                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(Properties.Settings.Default.SumReportDir + sumRecord.BrzbIzvestaja + ".xml").ToList();
                if (myXmlStrings.Count == 0)
                {
                    return;
                }
                foreach (string s in myXmlStrings)
                {
                    myXmlString += s;
                }

                if (myXmlString.Contains(Constants.XML_roots_ROOT2) == false)
                {
                    MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                    return;
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(myXmlString);
                XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT2 + "/" + Constants.XML_roots_Uzorak);

                foreach (XmlNode xn in xnList)
                {
                    string sampleNumForComparation = xn[Constants.XML_GeneralData_BRUZORKA_SAMALIMBSLOVOM].InnerText;
                    if (sampleNumForComparation.Equals(sumRecord.BrUzorka) == true)
                    {
                        //pokupi zadnje vrednosti sa result interface-a u sumRecord i tek onda postavljaj u xml fajl na osnovu koga se pravi zbirni izvestaj
                        if (resInterface != null)
                        {
                            sumRecord.Rm = resInterface.tfRm.Text;
                            if (resInterface.chbRp02.IsChecked == true)
                            {
                                sumRecord.Rp02 = resInterface.tfRp02.Text;
                            }
                            if (resInterface.chbRt05.IsChecked == true)
                            {
                                sumRecord.Rt05 = resInterface.tfRt05.Text;
                            }
                            if (resInterface.chbReL.IsChecked == true)
                            {
                                sumRecord.ReL = resInterface.tfReL.Text;
                            }
                            if (resInterface.chbReH.IsChecked == true)
                            {
                                sumRecord.ReH = resInterface.tfReH.Text;
                            }
                            sumRecord.A = resInterface.tfA.Text;
                            sumRecord.At = resInterface.tfAt.Text;
                            if (resInterface.chbn.IsChecked == true)
                            {
                                sumRecord.N = resInterface.tfn.Text;
                            }
                            sumRecord.Z = resInterface.tfZ.Text;
                        }



                        xn[Constants.XML_brzbIzvestaja_SUMReport].InnerText = sumRecord.BrzbIzvestaja;
                        xn[Constants.XML_polazniKvalitet_SUMReport].InnerText = sumRecord.PolazniKvalitet;
                        xn[Constants.XML_nazivnaDebljina_SUMReport].InnerText = sumRecord.NazivnaDebljina;
                        xn[Constants.XML_ispitivac_SUMReport].InnerText = sumRecord.Ispitivac;
                        xn[Constants.XML_brUzorka_SUMReport].InnerText = sumRecord.BrUzorka;
                        xn[Constants.XML_sarza_SUMReport].InnerText = sumRecord.Sarza;
                        xn[Constants.XML_Rm_SUMReport].InnerText = sumRecord.Rm;
                        xn[Constants.XML_Rp02_SUMReport].InnerText = sumRecord.Rp02;
                        xn[Constants.XML_Rt05_SUMReport].InnerText = sumRecord.Rt05;
                        xn[Constants.XML_ReL_SUMReport].InnerText = sumRecord.ReL;
                        xn[Constants.XML_ReH_SUMReport].InnerText = sumRecord.ReH;
                        xn[Constants.XML_A_SUMReport].InnerText = sumRecord.A;
                        xn[Constants.XML_At_SUMReport].InnerText = sumRecord.At;
                        xn[Constants.XML_N_SUMReport].InnerText = sumRecord.N;
                        xn[Constants.XML_Z_SUMReport].InnerText = sumRecord.Z;
                        xn[Constants.XML_napomena_SUMReport].InnerText = sumRecord.Napomena;
                        isSampleAlreadyExist = true;
                    }

                }

                if (xml != null && isSampleAlreadyExist == true)
                {
                    xml.Save(Properties.Settings.Default.SumReportDir + sumRecord.BrzbIzvestaja + ".xml");
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void modifyExistingSampleInSumReport(SumReportRecord sumRecord, out bool isSampleAlreadyExist)}", System.DateTime.Now);
                isSampleAlreadyExist = false;
            }
        }

        /// <summary>
        /// obrada dogadjaja kada se klikne na dugme Zapamti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSample_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ovde se prelazi u mod vec ranije zapamcenog fajla
                //bitno zbog tacnog odredjivanja dinamickog dela ResultsInterface-a au i bu ili Du ili nista
                IsStoppedOnlineSample = false;
                if (isCurrentProgressWrittingInOnlineFile == true)
                {
                    MessageBox.Show("Online upis je još uvek u toku!");
                    return;
                }


                //first save and write in txt file number of samples in current tearing
                double numberOfSamples;
                //ovo vise ne pije vodu jel upola kidanja moze da se prekine kidanje
                //List<string> lines = File.ReadAllLines(fpOnlineGlobal).ToList();
                //List<string> lines = File.ReadAllLines(Constants.unsavedFilepath).ToList();
                List<string> lines = File.ReadAllLines(Properties.Settings.Default.unsavedFilepath).ToList();
                //double numberOfPreassuresInMpa = dataReader.PreassureInMPa.Count;
                numberOfSamples = lines.Count - headerSizeForCurrentSample/*onHeader.HeaderSize*/;
                //numberOfSamples = linesNumberForCurrentOnlineFile - headerSizeForCurrentSample;
                //numberOfPreassuresInMpa = numberOfPreassuresInMpa - onHeader.HeaderSize;
                //string text = File.ReadAllText(Constants.unsavedFilepath);
                string text = File.ReadAllText(Properties.Settings.Default.unsavedFilepath);
                text = text.Replace(Constants.ONLINEFILEHEADER_BrojZapisa, Constants.ONLINEFILEHEADER_BrojZapisa + numberOfSamples.ToString());
                //File.WriteAllText(fpOnlineGlobal, text);
                //File.WriteAllText(Constants.unsavedFilepath, text);
                File.WriteAllText(Properties.Settings.Default.unsavedFilepath, text);


                MainWindow window = (MainWindow)MainWindow.GetWindow(this);
                SaveDialogForm saveDialog;
                bool isShowChangeGraphicRAndE = false;
                if (plotting.Printscreen.chbChangeOfRAndE.IsChecked == true)
                {
                    isShowChangeGraphicRAndE = true;
                }
                else
                {
                    isShowChangeGraphicRAndE = false;
                }
                bool ismanualNCalculated = false;
                if (plotting.Printscreen.chbCalculateNManual.IsChecked == true)
                {
                    ismanualNCalculated = true;
                }
                else
                {
                    ismanualNCalculated = false;
                }
                saveDialog = new SaveDialogForm(isShowChangeGraphicRAndE, ismanualNCalculated, this.Plotting.Printscreen);

                string currFileNamePath = String.Empty;
                if (saveDialog != null)
                {
                    SaveDialogProperty = saveDialog;
                    currFileNamePath = saveDialog.saveFileDialog1.FileName;
                }




                if (SaveDialogProperty.IsClickedToSaveFile == true)
                {
                    //write sample in sum report
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
                    //string z = String.Empty;
                    string z = resInterface.tfZ.Text;
                    string napomena = String.Empty;
                    //List<string> linesIO = File.ReadAllLines(Constants.inputOutputFilepath).ToList();
                    List<string> linesIO = File.ReadAllLines(Properties.Settings.Default.inputOutputFilepath).ToList();

                    for (int index = 0; index < linesIO.Count; index++)
                    {
                        string line = linesIO[index];
                        List<string> lineParts = line.Split('\t').ToList();
                        if (lineParts.Count > 1)
                        {
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_BRZBIZVESTAJA) == true)
                            {
                                brzbIzvestaja = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_POLAZNIKVALITET) == true)
                            {
                                polazniKvalitet = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_NAZIVNADEBLJINA) == true)
                            {
                                nazivnaDebljina = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_OPERATOR) == true)
                            {
                                ispitivac = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_BRUZORKA) == true)
                            {
                                brUzorka = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_SARZA) == true)
                            {
                                sarza = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.Rm) == true)
                            {
                                rm = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.Rp02) == true)
                            {
                                if (LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True"))
                                {
                                    //ukoliko se ne poklapaju rezultati sa onim sto je u fajlu zapamceno stavi sta je u rezultatu i zapamti u inputoutput fajlu
                                    if (lineParts.ElementAt(1).Equals(resInterface.tfRp02.Text) == false)
                                    {
                                        rp02 = resInterface.tfRp02.Text;
                                        //zapamti u inputoutput fajl promene koje su nastale u rucnom rezimu rada u Analizi Dijagrama
                                        linesIO[index] = lineParts.ElementAt(0) + "\t" + resInterface.tfRp02.Text + "\t" + lineParts.ElementAt(2);
                                        File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, linesIO.ToArray());
                                    }
                                    else
                                    {
                                        rp02 = lineParts.ElementAt(1);
                                    }
                                }
                                else
                                {
                                    rp02 = string.Empty;
                                }
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.Rt05) == true)
                            {
                                if (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True"))
                                {
                                    //ukoliko se ne poklapaju rezultati sa onim sto je u fajlu zapamceno stavi sta je u rezultatu i zapamti u inputoutput fajlu
                                    if (lineParts.ElementAt(1).Equals(resInterface.tfRt05.Text) == false)
                                    {
                                        rt05 = resInterface.tfRt05.Text;
                                        //zapamti u inputoutput fajl promene koje su nastale u rucnom rezimu rada u Analizi Dijagrama
                                        linesIO[index] = lineParts.ElementAt(0) + "\t" + resInterface.tfRt05.Text + "\t" + lineParts.ElementAt(2);
                                        File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, linesIO.ToArray());
                                    }
                                    else
                                    {
                                        rt05 = lineParts.ElementAt(1);
                                    }
                                }
                                else
                                {
                                    rt05 = string.Empty;
                                }
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ReL) == true)
                            {
                                if (LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True"))
                                {
                                    //ukoliko se ne poklapaju rezultati sa onim sto je u fajlu zapamceno stavi sta je u rezultatu i zapamti u inputoutput fajlu
                                    if (lineParts.ElementAt(1).Equals(resInterface.tfReL.Text) == false)
                                    {
                                        reL = resInterface.tfReL.Text;
                                        //zapamti u inputoutput fajl promene koje su nastale u rucnom rezimu rada u Analizi Dijagrama
                                        linesIO[index] = lineParts.ElementAt(0) + "\t" + resInterface.tfReL.Text + "\t" + lineParts.ElementAt(2);
                                        File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, linesIO.ToArray());
                                    }
                                    else
                                    {
                                        reL = lineParts.ElementAt(1);
                                    }
                                }
                                else
                                {
                                    reL = string.Empty;
                                }
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ReH) == true)
                            {
                                if (LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True"))
                                {
                                    //ukoliko se ne poklapaju rezultati sa onim sto je u fajlu zapamceno stavi sta je u rezultatu i zapamti u inputoutput fajlu
                                    if (lineParts.ElementAt(1).Equals(resInterface.tfReL.Text) == false)
                                    {
                                        reH = resInterface.tfReH.Text;
                                        //zapamti u inputoutput fajl promene koje su nastale u rucnom rezimu rada u Analizi Dijagrama
                                        linesIO[index] = lineParts.ElementAt(0) + "\t" + resInterface.tfReH.Text + "\t" + lineParts.ElementAt(2);
                                        File.WriteAllLines(Properties.Settings.Default.inputOutputFilepath, linesIO.ToArray());
                                    }
                                    else
                                    {
                                        reH = lineParts.ElementAt(1);
                                    }
                                }
                                else
                                {
                                    reH = string.Empty;
                                }
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.A) == true)
                            {
                                a = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.At) == true)
                            {
                                at = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.n) == true)
                            {
                                if (LastInputOutputSavedData.chbn_ResultsInterface.Equals("True"))
                                {
                                    n = lineParts.ElementAt(1);
                                }
                                else
                                {
                                    n = string.Empty;
                                }
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.Z) == true)
                            {
                                z = lineParts.ElementAt(1);
                            }
                            if (lineParts.ElementAt(0).Equals(Constants.ONLINEFILEHEADER_NAPOMENA) == true)
                            {
                                napomena = lineParts.ElementAt(1);
                            }
                        }

                    }

                    if (z.Equals(string.Empty))
                    {
                        z = resInterface.tfZ.Text;
                    }


                    sumRecord = new SumReportRecord(brzbIzvestaja, polazniKvalitet, nazivnaDebljina, ispitivac, brUzorka, sarza, rm, rp02, rt05, reL, reH, a, at, n, z, napomena);

                    XElement xmlRoot = null;
                    XElement xmlNew = null;

                    if (File.Exists(Properties.Settings.Default.SumReportDir + brzbIzvestaja + ".xml") == false)
                    {
                        xmlRoot = new XElement(Constants.XML_root_SUMReport,
                                        new XElement(Constants.XML_Uzorak_SUMReport,
                                            new XElement(Constants.XML_brzbIzvestaja_SUMReport, sumRecord.BrzbIzvestaja),
                                            new XElement(Constants.XML_polazniKvalitet_SUMReport, sumRecord.PolazniKvalitet),
                                            new XElement(Constants.XML_nazivnaDebljina_SUMReport, sumRecord.NazivnaDebljina),
                                            new XElement(Constants.XML_ispitivac_SUMReport, sumRecord.Ispitivac),
                                            new XElement(Constants.XML_brUzorka_SUMReport, sumRecord.BrUzorka),
                                            new XElement(Constants.XML_sarza_SUMReport, sumRecord.Sarza),
                                            new XElement(Constants.XML_Rm_SUMReport, sumRecord.Rm),
                                            new XElement(Constants.XML_Rp02_SUMReport, sumRecord.Rp02),
                                            new XElement(Constants.XML_Rt05_SUMReport, sumRecord.Rt05),
                                            new XElement(Constants.XML_ReL_SUMReport, sumRecord.ReL),
                                            new XElement(Constants.XML_ReH_SUMReport, sumRecord.ReH),
                                            new XElement(Constants.XML_A_SUMReport, sumRecord.A),
                                            new XElement(Constants.XML_At_SUMReport, sumRecord.At),
                                            new XElement(Constants.XML_N_SUMReport, sumRecord.N),
                                            new XElement(Constants.XML_Z_SUMReport, sumRecord.Z),
                                            new XElement(Constants.XML_napomena_SUMReport, sumRecord.Napomena),
                                            new XElement(Constants.XML_KV_SUMReport, string.Empty),
                                            new XElement(Constants.XML_KU_SUMReport, string.Empty)
                                        ));

                        if (xmlRoot != null)
                        {
                            xmlRoot.Save(Properties.Settings.Default.SumReportDir + brzbIzvestaja + ".xml");
                        }
                    }
                    if (File.Exists(Properties.Settings.Default.SumReportDir + brzbIzvestaja + ".xml") == true)
                    {
                        xmlRoot = XElement.Load(Properties.Settings.Default.SumReportDir + brzbIzvestaja + ".xml");
                        //kada se dodaje nov uzorak proveri da li se on vec pojavio u zbirnom izvestaju
                        bool isSampleAlreadyExist = false;

                        modifyExistingSampleInSumReport(sumRecord, out isSampleAlreadyExist);

                        //ako se do sada broj uzorka koji se dodaje nije pojavio u zbirnom izvestaju dodaje se kao nov clan (red u tabeli zbirnog izvestaja) 
                        if (isSampleAlreadyExist == false)
                        {
                            xmlNew = new XElement(Constants.XML_Uzorak_SUMReport,
                                                new XElement(Constants.XML_brzbIzvestaja_SUMReport, sumRecord.BrzbIzvestaja),
                                                new XElement(Constants.XML_polazniKvalitet_SUMReport, sumRecord.PolazniKvalitet),
                                                new XElement(Constants.XML_nazivnaDebljina_SUMReport, sumRecord.NazivnaDebljina),
                                                new XElement(Constants.XML_ispitivac_SUMReport, sumRecord.Ispitivac),
                                                new XElement(Constants.XML_brUzorka_SUMReport, sumRecord.BrUzorka),
                                                new XElement(Constants.XML_sarza_SUMReport, sumRecord.Sarza),
                                                new XElement(Constants.XML_Rm_SUMReport, sumRecord.Rm),
                                                new XElement(Constants.XML_Rp02_SUMReport, sumRecord.Rp02),
                                                new XElement(Constants.XML_Rt05_SUMReport, sumRecord.Rt05),
                                                new XElement(Constants.XML_ReL_SUMReport, sumRecord.ReL),
                                                new XElement(Constants.XML_ReH_SUMReport, sumRecord.ReH),
                                                new XElement(Constants.XML_A_SUMReport, sumRecord.A),
                                                new XElement(Constants.XML_At_SUMReport, sumRecord.At),
                                                new XElement(Constants.XML_N_SUMReport, sumRecord.N),
                                                new XElement(Constants.XML_Z_SUMReport, sumRecord.Z),
                                                new XElement(Constants.XML_napomena_SUMReport, sumRecord.Napomena),
                                                new XElement(Constants.XML_KV_SUMReport, string.Empty),
                                                new XElement(Constants.XML_KU_SUMReport, string.Empty)
                                            );
                            if (xmlNew != null)
                            {
                                xmlRoot.Add(xmlNew);
                            }

                            if (xmlRoot != null)
                            {
                                xmlRoot.Save(Properties.Settings.Default.SumReportDir + brzbIzvestaja + ".xml");
                            }
                        }
                    }
                    //if (xmlRoot != null)
                    //{
                    //    xmlRoot.Save(Properties.Settings.Default.SumReportDir + brzbIzvestaja + ".xml");
                    //}
                }

                if (SaveDialogProperty.IsClickedToSaveFile == true)
                {
                    window.Plotting.tfFilepathPlotting.Text = currFileNamePath;
                    window.Plotting.tfFilepathPlottingKeyDown();
                    isCurrentSampleSaved = true;
                    if (IsLessThanOne == false)
                    {
                        window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    }
                    else
                    {
                        forLessThanOnePoints = new MyPointCollection();
                        foreach (var point in points)
                        {
                            forLessThanOnePoints.Add(point);
                        }
                        //window.PrintScreen.UpdatePrintScreen(forLessThanOnePoints);
                        //window.Plotting.DeleteFittingPath();
                        //window.Plotting.DeleteNonFittingPath();
                        //window.Plotting.PointsOfFittingLine.Clear();
                        //window.Plotting.points.Clear();
                        //window.Plotting.PointsOfFittingLine = forLessThanOnePoints;

                        double rmLoc = Double.MinValue;



                        ResultsInterface = new ResultsInterface(this, window.Plotting, window.PrintScreen);
                        //window.Plotting.setRpmaxAndFmax(out rmLoc);
                        window.Plotting.Rm = dataReader.PreassureInMPa.Max();
                        window.Plotting.Rm = Math.Round(window.Plotting.Rm, 0);
                        window.Plotting.Lu = window.Plotting.L0 * (100 + dataReader.RelativeElongation.Max()) / 100;
                        window.Plotting.Lu = Math.Round(window.Plotting.Lu, 2);
                        window.Plotting.Fm = dataReader.ForceInKN.Max() * 1000;
                        window.Plotting.Fm = Math.Round(window.Plotting.Fm, 0);
                        window.Plotting.A = dataReader.RelativeElongation.Max();
                        window.Plotting.A = Math.Round(window.Plotting.A, 2);
                        if (ResultsInterface != null)
                        {
                            //ResultsInterface.EmptyResultsInterface();
                            window.Plotting.tfRm.Text = window.Plotting.Rm.ToString();
                            window.Plotting.tfFm.Text = window.Plotting.Fm.ToString();
                            window.Plotting.tfReL.Text = string.Empty;
                            window.Plotting.tfReH.Text = string.Empty;
                            window.Plotting.tfA.Text = window.Plotting.A.ToString();
                            window.Plotting.tfRp02.Text = string.Empty;
                            ResultsInterface.tfLu.Text = window.Plotting.Lu.ToString();
                            ResultsInterface.tfRm.Text = window.Plotting.Rm.ToString();
                            ResultsInterface.tfFm.Text = window.Plotting.Fm.ToString();
                            ResultsInterface.tfA.Text = window.Plotting.A.ToString();
                            ResultsInterface.tfAt.Text = string.Empty;
                            ResultsInterface.tfAgt.Text = string.Empty;
                            if (ResultsInterface.chbRt05.IsChecked == false)
                            {
                                ResultsInterface.tfRt05.Text = string.Empty;
                            }
                            //window.Plotting.btnPlottingModeClick();
                            window.tab_third.IsSelected = true;


                            window.PrintScreen.btnSampleDataPrintMode.IsEnabled = true;
                            plotting.OnlineModeInstance.ResultsInterface.tfRmax.Text = LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface;
                            plotting.OnlineModeInstance.ResultsInterface.tfE2.Text = LastInputOutputSavedData.e2Max_ResultsInterface;
                            plotting.OnlineModeInstance.ResultsInterface.tfE4.Text = LastInputOutputSavedData.e4Max_ResultsInterface;
                            ResultsInterface.Show();
                        }


                    }
                    //if (OptionsInPlottingMode.isOriginalCheckBoxChecked == true)
                    //{
                    //    window.Plotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                    //}

                    //window.tab_second.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void btnSaveSample_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }



        /// <summary>
        /// This opens Acrobat Reader and tells it to send the PDF to the default printer, and then shuts down Acrobat after three seconds.
        /// </summary>
        //public void SendToPrinter(string pdfFileName)
        //{
        //    try
        //    {
        //        string printerName = "HP LaserJet Professional P1102w";
        //        Process proc = new Process();
        //        proc.StartInfo.FileName = @"D:\Program Files\Adobe\Reader 11.0\Reader\AcroRd32.exe";
        //        proc.StartInfo.Arguments = @" /t /h " + "\"" + pdfFileName + "\"" + " " + "\"" + printerName + "\"";
        //        proc.StartInfo.UseShellExecute = true;
        //        proc.StartInfo.CreateNoWindow = true;
        //        //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        //        proc.Start();

        //        Thread.Sleep(5000);

        //        if (proc.HasExited == false)
        //        {
        //            proc.WaitForInputIdle();
        //            proc.Kill();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }


        //}

        public void SendToPrinter(string pdfFileName)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo();
                info.Verb = "print";
                //info.FileName = @"D:\output.pdf";
                info.FileName = pdfFileName;
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;

                Process p = new Process();
                p.StartInfo = info;
                p.Start();

                p.WaitForInputIdle();
                System.Threading.Thread.Sleep(3000);
                if (false == p.CloseMainWindow())
                    p.Kill();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {public void SendToPrinter(string pdfFileName)}", System.DateTime.Now);
            }
        }

   


        //private void btnPrintSample_Click(object sender, RoutedEventArgs e)
        //{
        //    //only for testing
        //    //testTensileMachineGraphics.OnlineModeFolder.Input_Data.GeneralData gd = new Input_Data.GeneralData(onHeader);
        //    //gd.Show();

        //    //testTensileMachineGraphics.OnlineModeFolder.Input_Data.ConditionsOfTesting ct = new Input_Data.ConditionsOfTesting(onHeader);
        //    //ct.Show();

        //    //testTensileMachineGraphics.OnlineModeFolder.Input_Data.MaterialForTesting mTesting = new Input_Data.MaterialForTesting(onHeader);
        //    //mTesting.Show();


        //    //testTensileMachineGraphics.OnlineModeFolder.Input_Data.PositionOfTube pOfTube = new Input_Data.PositionOfTube(onHeader);
        //    //pOfTube.Show();

        //    //testTensileMachineGraphics.OnlineModeFolder.Input_Data.RemarkOfTesting remark = new Input_Data.RemarkOfTesting(onHeader);
        //    //remark.Show();

        //    //only for testing



        //    MainWindow window = (MainWindow)MainWindow.GetWindow(this);
           
        //    window.Plotting.Printscreen.plotterPrint.SaveScreenshot(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");

        //    //PDFSampleReport pdfSampleReport = new PDFSampleReport(11, 8.5, onHeader,window.Plotting,resInterface);
        //    PDFSampleReport pdfSampleReport = new PDFSampleReport(11, 9, onHeader, window.Plotting, resInterface);
        //    //pdfSampleReport.CreateReport();
        //    //File.Delete(System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");
        //    //sendToPrinter(Constants.PATHOFSAMPLEREPORT);  
              

        //    if (isCurrentProgressWrittingInOnlineFile == true)
        //    {
        //        MessageBox.Show("Online upis je još uvek u toku!");
        //        return;
        //    }
        //}

       

        #endregion

      



        #region menuEvents


        private void showOnlineOptions_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnShowOnlineOptions.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void showOnlineOptions_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void menuIshowGraphicChangeOfParameters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isChangeParametersGraphicsOpen == false)
                {
                    if (isOptionsForChangeGraphic == false)
                    {
                        //ove dve linije koda idu sa ucitavanjem opcija kada zelimo da pozovemo prozor sa graficima a da prethodno nismo nijedanput kliknuli na otvaranje prozora sa opcijama grafika za promenu napona i ixduzenja
                        opChangeOfRAndE = new OptionsOnlineChangeOfRAndE(this);
                        opChangeOfRAndE.LoadOptionsOnlineChangeOfRAndE();
                        //uradi ono sto se radi kada se zatvori prozor , ali ovde nema prozora pa nema ni zatvaranja pa mora u kodu da se promenljiva isOptionsForChangeGraphic postavi na false
                        isOptionsForChangeGraphic = false;
                    }

                    vXY = new VelocityOfChangeParametersXY(this);

                    //ukoiko je polje za potvrdu kod zelenog dugmeta cekirano
                    //prikazi grafike koje prate promenu brzine napona i izduzenja
                    if (chbStartSampleShowChangedPar.IsChecked == false)
                    {
                        if (vXY != null)
                        {
                            vXY.Visibility = Visibility.Hidden;
                        }
                    }
                    else
                    {
                        if (vXY != null)
                        {
                            if (vXY.Height > 0)
                            {
                                vXY.Visibility = Visibility.Visible;
                                vXY.Show();
                            }
                        }
                    }


                }
                else
                {
                    if (vXY != null)
                    {
                        if (chbStartSampleShowChangedPar.IsChecked == false)
                        {
                            vXY.Visibility = Visibility.Hidden;

                        }
                        else
                        {
                            vXY.Visibility = Visibility.Visible;
                            if (vXY.Height > 0)
                            {
                                vXY.Show();
                                vXY.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void menuIshowGraphicChangeOfParameters_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void menuIshowOptionsChangeOfParameters_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isOptionsForChangeGraphic == false)
                {
                    opChangeOfRAndE = new OptionsOnlineChangeOfRAndE(this);
                    //online mode instanca se nalazi u okviru objekta vXY kod polja tj properti-ja OnlineModeInstance
                    //opChangeOfRAndE.OnlineModeInstance = this;
                    opChangeOfRAndE.setRadioButtons2();

                    opChangeOfRAndE.Show();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void menuIshowOptionsChangeOfParameters_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void menuIshowOptionsManagingOfTTM_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (isOptionsForManagingOfTTM == false)
                {
                    opManagingOfTTM = new OptionsOnlineManagingOfTTM(this);
                    //online mode instanca se nalazi u okviru objekta vXY kod polja tj properti-ja OnlineModeInstance
                    //opChangeOfRAndE.OnlineModeInstance = this;
                    //opChangeOfRAndE.setRadioButtons2();
                    opManagingOfTTM.Show();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineMode.xaml.cs] {private void menuIshowOptionsManagingOfTTM_Click(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

       

      

      

      

        

       

       








    }
}
