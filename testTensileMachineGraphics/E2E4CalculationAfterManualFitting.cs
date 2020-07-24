using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using testTensileMachineGraphics.PointViewModel;
using testTensileMachineGraphics.OnlineModeFolder;
using System.IO;
using System.Windows.Forms;
using testTensileMachineGraphics.Options;

namespace testTensileMachineGraphics
{
    /// <summary>
    /// klasa koja sluzi za ucitavanje Promena napona i izduzenja i racunanja e2min,e2max,e4min i e4max posle rucnog namestanja fitovanog grafika
    /// </summary>
    public class E2E4CalculationAfterManualFitting
    {


        private OnlineMode onlineMode; 


        /// <summary>
        /// od ovog indeksa vrednosti su vece od rp02/2
        /// </summary>
        private int indexFromChangedParametersFittingRp02 = 0;
        /// <summary>
        /// od ovog indeksa vrednosti su vece od rp02/2
        /// </summary>
        public int IndexFromChangedParametersFittingRp02
        {
            get { return indexFromChangedParametersFittingRp02; }
            set { indexFromChangedParametersFittingRp02 = value; }
        }

        /// <summary>
        /// do ovog indeksa ide kidanje tj tacka A ,tacka kidanja (crveni trouglic)
        /// </summary>
        private int indexFromChangedParametersFittingUntilA = 0;

        /// <summary>
        /// do ovog indeksa ide kidanje tj tacka A ,tacka kidanja (crveni trouglic)
        /// </summary>
        public int IndexFromChangedParametersFittingUntilA
        {
            get { return indexFromChangedParametersFittingUntilA; }
            set { indexFromChangedParametersFittingUntilA = value; }
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

        #region constructors

        /// <summary>
        /// NAPOMENA: Fitovana promena elongacije se izracunava u konstruktoru !!!
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="rp02"></param>
        /// <param name="onlineMode"></param>
        public E2E4CalculationAfterManualFitting(string filepath, double rp02, OnlineMode onlineMode)
        {
            try
            {

                this.onlineMode = onlineMode;

                string nameChangedParameters = filepath.Split('.').ElementAt(0);
                nameChangedParameters += ".e2e4";
                List<string> dataListChangedParameters = new List<string>();
                if (File.Exists(nameChangedParameters) == true)
                {
                    dataListChangedParameters = File.ReadAllLines(nameChangedParameters).ToList();
                }
                else
                {
                    MessageBox.Show("Ne postoji e2e4 fajl za učitani fajl !");
                }

                for (int i = 0; i < dataListChangedParameters.Count; i++)
                {
                    List<string> currentLine = dataListChangedParameters[i].Split('*').ToList();
                    double lastChangeOfR = 0;
                    bool isN = Double.TryParse(currentLine[0], out lastChangeOfR);
                    if (isN)
                    {
                        arrayChangeValueOfR.Add(lastChangeOfR);
                    }
                    double lastChangeOfRTau = 0;
                    isN = Double.TryParse(currentLine[1], out lastChangeOfRTau);




                    double lastChangeOfE = 0;
                    isN = Double.TryParse(currentLine[2], out lastChangeOfE);
                    if (isN)
                    {
                        arrayChangeValueOfE.Add(lastChangeOfE);
                    }
                    double lastChangeOfETau = 0;
                    isN = Double.TryParse(currentLine[3], out lastChangeOfETau);


                    double lastElongationOfEndOfInterval = 0;
                    isN = Double.TryParse(currentLine[4], out lastElongationOfEndOfInterval);
                    if (isN)
                    {
                        arrayElongationOfEndOfInterval.Add(lastElongationOfEndOfInterval);
                    }

                    double lastROfEndOfInterval = 0;
                    isN = Double.TryParse(currentLine[5], out lastROfEndOfInterval);
                    if (isN)
                    {
                        arrayROfEndOfInterval.Add(lastROfEndOfInterval);
                    }

                    MyPoint lastpointOfChangeR = new MyPoint(lastChangeOfR, lastChangeOfRTau);
                    pointsChangeOfR.Add(lastpointOfChangeR);
                    MyPoint lastpointOfChangeE = new MyPoint(lastChangeOfE, lastChangeOfETau);
                    pointsChangeOfE.Add(lastpointOfChangeE);

                }//end for loop


                //get fitting points for change of R greater than rp02/2
                int ii;
                for (ii = 0; ii < pointsChangeOfR.Count; ii++)
                {
                    if (arrayROfEndOfInterval[ii] < rp02 / 2)
                    {

                    }
                    else
                    {
                        ii--;
                        break;
                    }

                }


                int iiUntilA;
                for (iiUntilA = 0; iiUntilA < pointsChangeOfR.Count; iiUntilA++)
                {
                    if (arrayElongationOfEndOfInterval[iiUntilA] <= onlineMode.Plotting.A)
                    {

                    }
                    else
                    {
                        iiUntilA--;
                        break;
                    }

                }


                for (int j = 0; j < pointsChangeOfR.Count; j++)
                {
                    if (j > ii && j < iiUntilA)
                    {
                        pointsChangeOfRFitting.Add(pointsChangeOfR[j]);
                    }
                }
                for (int j = 0; j < pointsChangeOfE.Count; j++)
                {
                    if (/*j > ii &&*/ j < iiUntilA)
                    {
                        pointsChangeOfEFitting.Add(pointsChangeOfE[j]);
                    }
                }

                indexFromChangedParametersFittingRp02 = ii;
                indexFromChangedParametersFittingUntilA = iiUntilA;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {public E2E4CalculationAfterManualFitting(string filepath, double rp02, OnlineMode onlineMode)}", System.DateTime.Now);
            }

         }
       

        #endregion


      
        #region methods

        public void DivideE2AndE4Interval(int index, double E2E4Border, double XTranslateAmountFittingMode, bool e2e4selected = true) 
        {
            try
            {
                XTranslateAmountFittingMode = 0;//nema fitovanja vec se radi na osnovu originalnih podataka (zeleni grafik)
                arrayElongationOfEndOfInterval_Fitting.Clear();
                for (int i = 0; i < arrayElongationOfEndOfInterval.Count; i++)
                {
                    //fituje se sve do tacke kidanja
                    if (i <= index - 1)
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
                    if (e2e4selected == true)
                    {
                        if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode < 0.95 * E2E4Border)
                        {
                            if (pointsChangeOfEFitting[i] != null)
                            {
                                arrayE2Interval.Add(pointsChangeOfEFitting[i].YAxisValue);
                            }
                        }
                        if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode > 1.05 * E2E4Border)
                        {
                            if (pointsChangeOfEFitting[i] != null)
                            {
                                arrayE4IntervalWithZeros.Add(pointsChangeOfEFitting[i].YAxisValue);
                            }
                        }
                    }
                    else
                    {
                        if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode < 0.95 * E2E4Border)//this is actually E3E4Border
                        {
                            if (pointsChangeOfEFitting[i] != null)
                            {
                                arrayE2Interval.Add(pointsChangeOfEFitting[i].YAxisValue);
                            }
                        }
                        if (arrayElongationOfEndOfInterval_Fitting[i] - XTranslateAmountFittingMode > 1.05 * E2E4Border)
                        {
                            if (pointsChangeOfEFitting[i] != null)
                            {
                                arrayE4IntervalWithZeros.Add(pointsChangeOfEFitting[i].YAxisValue);
                            }
                        }
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
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {public void DivideE2AndE4Interval(int index, double E2E4Border, double XTranslateAmountFittingMode, bool e2e4selected = true)}", System.DateTime.Now);
            }

        }

        public void MultipleArrayOfEAndRChange(double ratio) 
        {
            try
            {
                for (int i = 0; i < pointsChangeOfEFitting.Count; i++)
                {
                    pointsChangeOfEFitting[i].YAxisValue = ratio * pointsChangeOfEFitting[i].YAxisValue;
                }

                for (int i = 0; i < arrayE2Interval.Count; i++)
                {
                    arrayE2Interval[i] = ratio * arrayE2Interval[i];
                    arrayE2Interval[i] = Math.Round(arrayE2Interval[i], 5);
                }
                for (int i = 0; i < arrayE4Interval.Count; i++)
                {
                    arrayE4Interval[i] = ratio * arrayE4Interval[i];
                    arrayE4Interval[i] = Math.Round(arrayE4Interval[i], 5);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {public void MultipleArrayOfEAndRChange(double ratio)}", System.DateTime.Now);
            }
        }


        public double calculateRmaxWithPoint()
        {
            try
            {
                if (pointsChangeOfRFitting == null)
                {
                    return -1;
                }
                List<double> candidatesForMaxRWitPoint = new List<double>();

                bool firstElement = true;

                foreach (var point in pointsChangeOfRFitting)
                {
                    if (firstElement == false)
                    {
                        candidatesForMaxRWitPoint.Add(point.YAxisValue);
                    }
                    else
                    {
                        firstElement = false;
                    }
                }
                if (candidatesForMaxRWitPoint.Count == 0)
                {
                    return -1;
                }
                else
                {
                    return candidatesForMaxRWitPoint.Max();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {public double calculateRmaxWithPoint()}", System.DateTime.Now);
                return 0;
            }
        }




        public void CalculateIndexFromChangedParametersFittingUntilA(double A) 
        {
            try
            {
                int iiUntilA;
                for (iiUntilA = 0; iiUntilA < pointsChangeOfR.Count; iiUntilA++)
                {
                    if (iiUntilA >= arrayElongationOfEndOfInterval.Count)
                    {
                        //upisi u log fajl [public void CalculateIndexFromChangedParametersFittingUntilA(double A) ]{if (iiUntilA >= arrayElongationOfEndOfInterval.Count)}
                        Logger.WriteNode("[public void CalculateIndexFromChangedParametersFittingUntilA(double A) ]{if (iiUntilA >= arrayElongationOfEndOfInterval.Count)}", System.DateTime.Now);
                        break;
                    }
                    if (arrayElongationOfEndOfInterval[iiUntilA] <= A)
                    {

                    }
                    else
                    {
                        iiUntilA--;
                        break;
                    }

                }


                indexFromChangedParametersFittingUntilA = iiUntilA;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {public void CalculateIndexFromChangedParametersFittingUntilA(double A)}", System.DateTime.Now);
            }
        }

        private void clearE2E4Data() 
        {
            try
            {
                arrayChangeValueOfR.Clear();
                arrayChangeValueOfE.Clear();
                arrayElongationOfEndOfInterval.Clear();
                arrayROfEndOfInterval.Clear();
                pointsChangeOfR.Clear();
                pointsChangeOfE.Clear();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {private void clearE2E4Data()}", System.DateTime.Now);
            }
        }


        public void RecalculateChangeOfRFittingPoints(double rp02, double A) 
        {
            try
            {
                pointsChangeOfRFitting.Clear();
                clearE2E4Data();


                string nameChangedParameters = OptionsInPlottingMode.filePath.Split('.').ElementAt(0);
                nameChangedParameters += ".e2e4";
                List<string> dataListChangedParameters = new List<string>();
                if (File.Exists(nameChangedParameters) == true)
                {
                    dataListChangedParameters = File.ReadAllLines(nameChangedParameters).ToList();
                }
                else
                {
                    MessageBox.Show("Ne postoji e2e4 fajl za učitani fajl !");
                }

                for (int i = 0; i < dataListChangedParameters.Count; i++)
                {
                    List<string> currentLine = dataListChangedParameters[i].Split('*').ToList();
                    double lastChangeOfR = 0;
                    bool isN = Double.TryParse(currentLine[0], out lastChangeOfR);
                    if (isN)
                    {
                        arrayChangeValueOfR.Add(lastChangeOfR);
                    }
                    double lastChangeOfRTau = 0;
                    isN = Double.TryParse(currentLine[1], out lastChangeOfRTau);




                    double lastChangeOfE = 0;
                    isN = Double.TryParse(currentLine[2], out lastChangeOfE);
                    if (isN)
                    {
                        arrayChangeValueOfE.Add(lastChangeOfE);
                    }
                    double lastChangeOfETau = 0;
                    isN = Double.TryParse(currentLine[3], out lastChangeOfETau);


                    double lastElongationOfEndOfInterval = 0;
                    isN = Double.TryParse(currentLine[4], out lastElongationOfEndOfInterval);
                    if (isN)
                    {
                        arrayElongationOfEndOfInterval.Add(lastElongationOfEndOfInterval);
                    }

                    double lastROfEndOfInterval = 0;
                    isN = Double.TryParse(currentLine[5], out lastROfEndOfInterval);
                    if (isN)
                    {
                        arrayROfEndOfInterval.Add(lastROfEndOfInterval);
                    }

                    MyPoint lastpointOfChangeR = new MyPoint(lastChangeOfR, lastChangeOfRTau);
                    pointsChangeOfR.Add(lastpointOfChangeR);
                    MyPoint lastpointOfChangeE = new MyPoint(lastChangeOfE, lastChangeOfETau);
                    pointsChangeOfE.Add(lastpointOfChangeE);

                }//end for loop





                int ii;
                //pointsChangeOfR.Count mora biti jednako arrayROfEndOfInterval.Count
                for (ii = 0; ii < pointsChangeOfR.Count; ii++)
                {
                    if (arrayROfEndOfInterval[ii] < rp02 / 2)
                    {

                    }
                    else
                    {
                        ii--;
                        break;
                    }

                }
                indexFromChangedParametersFittingRp02 = ii;


                int iiUntilA;
                //pointsChangeOfR.Count mora biti jednako arrayElongationOfEndOfInterval.Count
                for (iiUntilA = 0; iiUntilA < pointsChangeOfR.Count; iiUntilA++)
                {
                    if (arrayElongationOfEndOfInterval[iiUntilA] <= A)
                    {

                    }
                    else
                    {
                        iiUntilA--;
                        break;
                    }

                }
                indexFromChangedParametersFittingUntilA = iiUntilA;

                bool firstelement = true;

                for (int j = 0; j < pointsChangeOfR.Count; j++)
                {
                    if (j > indexFromChangedParametersFittingRp02 && j < indexFromChangedParametersFittingUntilA)
                    {
                        if (firstelement == false)
                        {
                            pointsChangeOfRFitting.Add(pointsChangeOfR[j]);
                        }
                        else
                        {
                            firstelement = false;
                        }
                    }
                }


                //for (int j = 0; j < pointsChangeOfE.Count; j++)
                //{
                //    if (j < indexFromChangedParametersFittingUntilA)
                //    {
                //        pointsChangeOfEFitting.Add(pointsChangeOfE[j]);
                //    }
                //}
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "RecalculateChangeOfRFittingPoints");
                //Logger.WriteNode(ex.Message.ToString() + " {RecalculateChangeOfRFittingPoints}", System.DateTime.Now);
                Logger.WriteNode(ex.Message.ToString() + "[E2E4CalculationAfterManualFitting.cs] {public void RecalculateChangeOfRFittingPoints(double rp02, double A)}", System.DateTime.Now);
            }

        }

        #endregion
    }
}
