using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics.Options
{
    public static class OptionsInOnlineMode
    {
        
        public static double refreshTimeInterval = 12;
        public static int Resolution = 10;
        public static double L0 = 10;
        public static double S0 = 10;


        public static double mmCoeff = 10;
        public static double mmDivide = 10;
        public static double mmCoeffWithEkstenziometer = 10;
        public static double mmDivideWithEkstenziometer = 10;
        public static double nutnMultiple = 10;
        public static double nutnDivide = 10;

        public static bool isAutoChecked = false;
        
        public static bool isManualChecked = false;
        public static double xRange = 0.95;
        public static double yRange = 0.95;


        public static double onlineWriteEndTimeInterval = 2000;

        public static bool calculateMaxForceForTf = false;
        public static bool isCalibration = false;

        /// <summary>
        /// ovde se cuva vremenski interval [u ms] na kome se racuna Promena parametara (brzina promene napona i izduzenja)
        /// </summary>
        public static double timeIntervalForCalculationOfChangedParameters = 100;



        public static bool isE2E4BorderSelected = true;
        public static double E2E4Border = 0.95;
        public static double E3E4Border = 0.95;

        public static int COM = 1;

    }
}
