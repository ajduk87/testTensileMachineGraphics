using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Research.DynamicDataDisplay.Common;

namespace DynamicDataDisplaySample.VoltageViewModel
{
    public class VoltagePointCollection : RingArray <VoltagePoint>
    {
        private const int TOTAL_POINTS = 20000;

        public VoltagePointCollection()
            : base(TOTAL_POINTS) // here i set how much values to show 
        {    
        }
    }

    public class VoltagePoint
    {        
        //public DateTime Date { get; set; }

        public double XAxisValue { get; set; }
        
        public double Voltage { get; set; }

        public VoltagePoint(double voltage, double xAxisValue)
        {
            this.XAxisValue = xAxisValue;
            this.Voltage = voltage;
        }
    }
}
