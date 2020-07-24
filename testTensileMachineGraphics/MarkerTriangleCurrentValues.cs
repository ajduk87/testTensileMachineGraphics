using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics
{
    public class MarkerTriangleCurrentValues
    {

        #region constructors

        public MarkerTriangleCurrentValues() 
        {
            rmXValue = 0;
            rmYValue = 0;

            reHXValue = 0;
            reHYValue = 0;

            reLXValue = 0;
            reLYValue = 0;

            rp02XValue = 0;
            rp02YValue = 0;

            aXValue = 0;
            aYValue = 0;

            rt05XValue = 0;
            rt05YValue = 0;
            agXValue = 0;
            agYValue = 0;


        }

        #endregion

        #region properties

        public double RmXValue
        {
            get { return rmXValue; }
            set
            {
                if (value != null)
                {
                    rmXValue = value;
                }
            }
        }
        public double RmYValue
        {
            get { return rmYValue; }
            set
            {
                if (value != null)
                {
                    rmYValue = value;
                }
            }
        }



        public double ReHXValue
        {
            get { return reHXValue; }
            set
            {
                if (value != null)
                {
                    reHXValue = value;
                }
            }
        }
        public double ReHYValue
        {
            get { return reHYValue; }
            set
            {
                if (value != null)
                {
                    reHYValue = value;
                }
            }
        }



        public double ReLXValue
        {
            get { return reLXValue; }
            set
            {
                if (value != null)
                {
                    reLXValue = value;
                }
            }
        }
        public double ReLYValue
        {
            get { return reLYValue; }
            set
            {
                if (value != null)
                {
                    reLYValue = value;
                }
            }
        }




        public double Rp02XValue
        {
            get { return rp02XValue; }
            set
            {
                if (value != null)
                {
                    rp02XValue = value;
                }
            }
        }
        public double Rp02YValue
        {
            get { return rp02YValue; }
            set
            {
                if (value != null)
                {
                    rp02YValue = value;
                }
            }
        }



        public double AXValue
        {
            get { return aXValue; }
            set
            {
                if (value != null)
                {
                    aXValue = value;
                }
            }
        }
        public double AYValue
        {
            get { return aYValue; }
            set
            {
                if (value != null)
                {
                    aYValue = value;
                }
            }
        }

        public double AtXValue
        {
            get { return atXValue; }
            set
            {
                if (value != null)
                {
                    atXValue = value;
                }
            }
        }
        public double AtYValue
        {
            get { return atYValue; }
            set
            {
                if (value != null)
                {
                    atYValue = value;
                }
            }
        }


        public double Rt05XValue
        {
            get { return rt05XValue; }
            set
            {
                if (value != null)
                {
                    rt05XValue = value;
                }
            }
        }
        public double Rt05YValue
        {
            get { return rt05YValue; }
            set
            {
                if (value != null)
                {
                    rt05YValue = value;
                }
            }
        }


        public double AgXValue
        {
            get { return agXValue; }
            set
            {
                if (value != null)
                {
                    agXValue = value;
                }
            }
        }
        public double AgYValue
        {
            get { return agYValue; }
            set
            {
                if (value != null)
                {
                    agYValue = value;
                }
            }
        }

        #endregion

        #region members

        private double rmXValue;//marker 4
        private double rmYValue;//marker 4

        private double reHXValue;//marker 6
        private double reHYValue;//marker 6

        private double reLXValue;//marker 5
        private double reLYValue;//marker 5

        private double rp02XValue;//marker 7
        private double rp02YValue;//marker 7

        private double aXValue;//marker 8
        private double aYValue;//marker 8
        private double atXValue;
        private double atYValue;

        private double rt05XValue;//marker 9
        private double rt05YValue;//marker 9
        private double agXValue;//marker 10
        private double agYValue;//marker 10

        #endregion
        
        
    }
}
