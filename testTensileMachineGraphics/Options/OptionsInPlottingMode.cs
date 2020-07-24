using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics.Options
{
    public class OptionsInPlottingMode
    {




        public static bool isContinuousDisplay = false;
        public static bool isDiscreteDisplay = false;
        public static int Resolution = 10;
        public static int DerivationResolution = 10;



        public static double mmCoeff = 10;
        public static double mmDivide = 10;
        public static double mmCoeffWithEkstenziometer = 10;
        public static double mmDivideWithEkstenziometer = 10;
        public static double nutnMultiple = 10;
        public static double nutnDivide = 10;

        public static bool isAutoChecked = false;
        public static bool isManualChecked = false;

        public static bool PrikaziOriginalAfterRatioChanging = false;
        public static bool PrikaziFitovaniAfterRatioChanging = false;

        public static double xRange = 0.95;
        public static double yRange = 0.95;

        public static string filePath = String.Empty;

        public static bool isFittingChecked = false;

        public static bool isAutoFittingChecked = false;
        public static double pointCrossheadX = 0.0;
        public static double pointCrossheadY = 0.0;
        public static double pointAutoX1 = 0.0;
        public static double pointAutoX2 = 0.0;
        public static double pointAutoX3 = 0.0;
        public static double pointAutoY1 = 0.0;
        public static double pointAutoY2 = 0.0;
        public static double pointAutoY3 = 0.0;
        public static double procentAuto1 = 0.0;
        public static double procentAuto2 = 0.0;
        public static double procentAuto3 = 0.0;

        public static bool isManualFittingChecked = false;
        public static double pointManualX1 = 0.0;
        public static double pointManualY1 = 0.0;
        public static double pointManualX2 = 0.0;
        public static double pointManualY2 = 0.0;
        public static double pointManualX3 = 0.0;
        public static double pointManualY3 = 0.0;

        /// <summary>
        /// maksimalna dozvoljena vrednost postavljanja tacaka T1,T2 i T3 po x-osi
        /// </summary>
        public static double fittingAutoMaxXValue = 0.0;

        public static bool isShowOriginalDataGraphic = false;

        public static double ReHXRange = 3;
        public static double YieldInterval = 10; //koliko mora najmanje biti pad napona(pritiska) u MPa da bi se ustanovilo da je doslo do pojave Hukovog efekta 
        public static double MaxSubBetweenReLAndReLF = 10;  // ako je razlika veca od 10 MPa znaci da je lazni minimum i pravi
        public static double RationReLRpmaxForOnlyReL = 0.95;
        public static double RationXReLXmaxForOnlyReL = 0.05; // X is relative elongation
        public static double OnlyReLPreassureUnit = 9.99;// on MPa <!--na koliki skok napona se odredjuje izduzenje na osnovu kog ustanovljava se da li je doslo do slucaja kada postoji samo ReL-->
        public static double MinPossibleValueForOnlyReLElongationInProcent = 0;
        public static double MaxPossibleValueForOnlyReLPreassureInMPa = 0;

        public static bool UkljuciNepodrazumevaniModKidanja = false;
        public static double TearingPointCoeff = 0;
        public static double TearingMinFallPreassure = 0;
        public static int ResolutionForTearing = 10;

        public static double DefaultPreassureOfTearingInProcent = 80;

        public static bool isOriginalCheckBoxChecked = false;
        public static bool isChangeRatioCheckBoxChecked = false;

        public static double BeginIntervalForN = 3;
        public static double EndIntervalForN = 19;


        public static double ReEqualsRp = 0.05;
        public static double YungPrSpustanja = 0;
    

    }
}