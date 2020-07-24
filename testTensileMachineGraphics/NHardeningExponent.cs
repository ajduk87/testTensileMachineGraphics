using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics
{
    public class NHardeningExponent
    {
        #region members

        private List<double> sigma_Array;
        private List<double> epsilon_Array;
        private List<double> x_Array;
        private List<double> y_Array;
        private int numberOfSamples;

        private List<double> _fs;
        private List<double> _deltaLsInProcent;
        private double _s0;
        private double _l0;
        private double _A;
        private double _mE;


        public double N = 0;

        #endregion


        #region constructors

        public NHardeningExponent() 
        {
            try
            {
                sigma_Array = new List<double>();
                epsilon_Array = new List<double>();
                x_Array = new List<double>();
                y_Array = new List<double>();
                numberOfSamples = 0;

                _fs = new List<double>();
                _s0 = 0;
                _l0 = 0;
                _A = 0;
                _mE = 0;

                N = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {public NHardeningExponent()}", System.DateTime.Now);
            }
        }

        public NHardeningExponent(List<double> Fs, List<double> deltaLsInProcent, double s0, double l0, double A, double fittingFirstTwoPointCoeff)
        {
            try
            {
                sigma_Array = new List<double>();
                epsilon_Array = new List<double>();
                x_Array = new List<double>();
                y_Array = new List<double>();
                numberOfSamples = Fs.Count;

                _fs = Fs;
                _deltaLsInProcent = deltaLsInProcent;
                _s0 = s0;
                _l0 = l0;
                _A = A;
                _mE = fittingFirstTwoPointCoeff;
                //_mE = 200;

                N = 0;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {public NHardeningExponent(List<double> Fs, List<double> deltaLsInProcent, double s0, double l0, double A, double fittingFirstTwoPointCoeff)}", System.DateTime.Now);
            }
        }


        #endregion


        #region properties

        public List<double> Sigma_Array
        {
            get { return sigma_Array; }
            set 
            {
                if (value != null)
                {
                    sigma_Array = value;
                }
            }
        }
        public List<double> Epsilon_Array
        {
            get { return epsilon_Array; }
            set
            {
                if (value != null)
                {
                    epsilon_Array = value;
                }
            }
        }
        public List<double> X_Array 
        {
            get { return x_Array; }
            set
            {
                if (value != null)
                {
                    x_Array = value;
                }
            }
        }
        public List<double> Y_Array 
        {
            get { return y_Array; }
            set
            {
                if (value != null)
                {
                    y_Array = value;
                }
            }
        }
        public int NumberOfSamples 
        {
            get { return numberOfSamples; }
            set { numberOfSamples = value; }
        }


        public List<double> Fs
        {
            get { return _fs; }
            set
            {
                if (value != null)
                {
                    _fs = value;
                }
            }
        }

        public List<double> DeltaLsInProcent
        {
            get { return _deltaLsInProcent; }
            set
            {
                if (value != null)
                {
                    _deltaLsInProcent = value;
                }
            }
        }


        #endregion

        #region methods

        private void calculate_Sigma() 
        {
            try
            {
                //double deltaL = 0.01 * _A * _l0;
                double currentSigma = 0;
                for (int i = 0; i < _fs.Count; i++)
                {
                    double deltaL = 0.01 * _deltaLsInProcent[i] * _l0;
                    currentSigma = ((_fs[i] * 1000) / _s0) * ((_l0 + deltaL) / _l0);
                    //currentSigma = _fs[i] * ((_l0 + deltaL) / (_l0 * _s0))/1000;
                    sigma_Array.Add(currentSigma);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {private void calculate_Sigma()}", System.DateTime.Now);
            }
        }

        private void calculate_Epsilon()
        {
            try
            {
                //double deltaL = 0.01 * _A * _l0;
                double currentEpsilon = 0;
                double logArgument;
                for (int i = 0; i < _fs.Count; i++)
                {
                    double deltaL = 0.01 * _deltaLsInProcent[i] * _l0;
                    //logArgument = (_l0 + deltaL) / _l0 - (_fs[i]*1000) / (_s0 * 200000/* _mE */ );
                    logArgument = (_l0 + deltaL) / _l0;
                    if (logArgument != 0)
                    {
                        currentEpsilon = Math.Log(logArgument, Math.E);
                    }
                    else
                    {
                        currentEpsilon = 0;
                    }
                    epsilon_Array.Add(currentEpsilon);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {private void calculate_Epsilon()}", System.DateTime.Now);
            }
        }


        private void calculate_X() 
        {
            try
            {
                double currentX = 0;
                for (int i = 0; i < epsilon_Array.Count; i++)
                {
                    if (epsilon_Array[i] != 0)
                    {
                        currentX = Math.Log(epsilon_Array[i], Math.E);
                    }
                    else
                    {
                        currentX = 0;
                    }

                    x_Array.Add(currentX);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {private void calculate_X()}", System.DateTime.Now);
            }
        }

        private void calculate_Y() 
        {
            try
            {
                double currentY = 0;
                for (int i = 0; i < sigma_Array.Count; i++)
                {
                    if (sigma_Array[i] != 0)
                    {
                        currentY = Math.Log(sigma_Array[i], Math.E);
                    }
                    else
                    {
                        currentY = 0;
                    }


                    y_Array.Add(currentY);

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {private void calculate_Y()}", System.DateTime.Now);
            }
        }

        private double calculate_N() 
        {
            try
            {
                double firstElementUpper = 0;

                for (int i = 0; i < numberOfSamples; i++)
                {
                    firstElementUpper = firstElementUpper + (x_Array[i] * y_Array[i]);
                }
                firstElementUpper = numberOfSamples * firstElementUpper;



                double secondElementUpper = 0;
                double sumOfXs = 0;
                for (int i = 0; i < numberOfSamples; i++)
                {
                    sumOfXs = sumOfXs + x_Array[i];
                }

                double sumOfYs = 0;
                for (int i = 0; i < numberOfSamples; i++)
                {
                    sumOfYs = sumOfYs + y_Array[i];
                }
                secondElementUpper = sumOfXs * sumOfYs;



                double firstElementLower = 0;
                double sumOfXSquares = 0;
                for (int i = 0; i < numberOfSamples; i++)
                {
                    sumOfXSquares = sumOfXSquares + x_Array[i] * x_Array[i];
                }
                firstElementLower = numberOfSamples * sumOfXSquares;


                double secondElemenLower = sumOfXs * sumOfXs;

                double n = (firstElementUpper - secondElementUpper) / (firstElementLower - secondElemenLower);

                N = n;

                return n;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {private double calculate_N()}", System.DateTime.Now);
                return -1;
            }

        }


        public double Get_N()
        {
            try
            {
                calculate_Sigma();
                calculate_Epsilon();
                calculate_X();
                calculate_Y();
                return calculate_N();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[NHardeningExponent.cs] {public double Get_N()}", System.DateTime.Now);
                return -1;
            }
        }

        #endregion
    }
}
