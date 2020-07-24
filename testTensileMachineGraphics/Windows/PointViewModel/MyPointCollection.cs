using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.DynamicDataDisplay.Common;

namespace testTensileMachineGraphics.PointViewModel
{
    public class MyPointCollection : RingArray<MyPoint>
    {
         private const int TOTAL_POINTSDef = 20000;

        public MyPointCollection(int TOTAL_POINTS)
            : base(TOTAL_POINTS) // here i set how much values to show 
        {
        }

        public MyPointCollection()
            : base(TOTAL_POINTSDef) // here i set how much values to show 
        {
        }
    }


    public class MyPoint
    {
        //public DateTime Date { get; set; }

        public double XAxisValue { get; set; }

        public double YAxisValue { get; set; }

        public MyPoint(double yAxisValue, double xAxisValue)
        {
            this.XAxisValue = xAxisValue;
            this.YAxisValue = yAxisValue;
        }
    }


}
