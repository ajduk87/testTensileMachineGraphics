using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using testTensileMachineGraphics.PointViewModel;

namespace testTensileMachineGraphics
{
    public class Derivation
    {
        #region members

        private List<double> coeffs = new List<double>();
        private List<double> xAxisValues = new List<double>();
        private List<double> yAxisValues = new List<double>();

        #endregion


        #region constructors

        public Derivation() 
        {
            List<double> coeffs = new List<double>();
            List<double> xAxisValues = new List<double>();
            List<double> yAxisValues = new List<double>();
        }

        #endregion


        #region properties

        /// <summary>
        /// readonly
        /// </summary>
        public List<double> Coeffs 
        {
            get { return coeffs; }
        }

        public List<double> XAxisValues
        {
            get { return xAxisValues; }
        }

        public List<double> YAxisValues
        {
            get { return yAxisValues; }
        }

        #endregion


        #region methods

        public void findCoeffs(List<double> ys, List<double> xs,int derivationResolution) 
        {
            if (ys.Count != xs.Count)
            {
                System.Windows.Forms.MessageBox.Show("X i Y moraju imati isti broj elemenata!" + System.Environment.NewLine + "[Derivaion class]");
                return;
            }

           

            double xd;
            double yd;
            double k = 0;

            for (int i = 0; i < xs.Count; i++)
            {
                if (i % derivationResolution == 0)
                {
                    xAxisValues.Add(xs[i]);
                    yAxisValues.Add(ys[i]);

                    if (i == 0)
                    {
                        xd = xs[i] - 0;
                        yd = ys[i] - 0;
                    }
                    else
                    {
                        xd = xs[i] - xs[i - derivationResolution];
                        yd = ys[i] - ys[i - derivationResolution];

                        if (xd != 0)
                        {
                            k = yd / xd;
                        }
                        else
                        {
                            k = Double.MaxValue;
                        }

                        coeffs.Add(k);
                    }
                }
            }
        }

      
        public void Clear() 
        {
            coeffs.Clear();
            xAxisValues.Clear();
            yAxisValues.Clear();
        }

        #endregion
    }
}
