using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics.Options
{
    public static class OptionsInAnimation
    {
        public static bool isContinuousDisplay = false;
        public static bool isDiscreteDisplay = false;
        public static double refreshTimeInterval = 12;
        public static int Resolution = 11;
        public static int conResolution = 10;
        public static int disResolution = 10;


        public static double l0 = 10;
        public static double s0 = 10;


        public static double mmCoeff = 10;
        public static double mmDivide = 10;
        public static double nutnMultiple = 10;
        public static double nutnDivide = 10;

        public static bool isAutoChecked = false;
        public static double ratioForPreassure = 10;
        public static double ratioForElongation = 10;

        public static double maxChangeOfPreassure = Double.MinValue;
        public static double elongationForMaxChangeOfPreassure = Double.MinValue;

        public static string filePath = String.Empty;
    }
}
