using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics
{
    public class Constants
    {
        public const int TOTAL_POINTSDef = 30000;

        //this must be in xml settings last data
        public static string onlineModeOptionsXml = System.Environment.CurrentDirectory + "\\configuration\\onlineModeOptions.xml";
        public static string ForceRangesonlineOptions = System.Environment.CurrentDirectory + "\\configuration\\ForceRangesonlineOptions.xml";
        public static string FooterOptions = System.Environment.CurrentDirectory + "\\configuration\\FotterOptions.xml";
        public static string plottingModeOptionsXml = System.Environment.CurrentDirectory + "\\configuration\\plottingModeOptions.xml";
        public static string onlineModeChangeOfRAndEOptionsXml = System.Environment.CurrentDirectory + "\\configuration\\onlineModeChangeOfRAndEOptions.xml";
        public static string onlineModeManagingOfTTMXml = System.Environment.CurrentDirectory + "\\configuration\\onlineModeManagingOfTTM.xml";
        public static string lastOnlineHeaderXml = System.Environment.CurrentDirectory + "\\configuration\\lastOnlineHeader.xml";
        public static string lastResultsInterfaceXml = System.Environment.CurrentDirectory + "\\configuration\\lastResultsInterface.xml";
        public static string logFilePath = System.Environment.CurrentDirectory + "\\configuration\\Log\\";
        public static string RmaxWithPoint = System.Environment.CurrentDirectory + "\\configuration\\RmaxWithPoint.txt";

        public static string TRENUTNIOPSEGSILE_ONLINE = "";


        //only testing this must be in options
        public static string onlineFilepath = /*"D:\\___temporary\\online.txt"*/ string.Empty;
        //public const string animationFilepath = "D:\\___temporary\\animation.anim";
        public static string e2e4Filepath = /*"D:\\___temporary\\e2e4Online.e2e4"*/ string.Empty;
        public static string inputOutputFilepath = /*"D:\\___temporary\\ulazIzlazOnline.inputoutput"*/ string.Empty;
        public static string sampleReportFilepath = /*"D:\\___temporary\\izvestajOnline.xml"*/ string.Empty;
        public static string sampleReportGraphicFilepath = /*"D:\\___temporary\\izvestajGrafikOnline.png"*/ string.Empty;
        public static string sampleReportGraphicFilepathChangeOfR = /*"D:\\___temporary\\izvestajGrafikOnlinePromenaR.png"*/ string.Empty;
        public static string sampleReportGraphicFilepathChangeOfE = /*"D:\\___temporary\\izvestajGrafikOnlinePromenaE.png"*/ string.Empty;
        public static string unsavedFilepath = /*"D:\\___temporary\\nezapamceno.txt"*/ string.Empty;
        public const double mmCoeff = 1.219;
        public const double mmDivide = 10000;
        public const double nutnMultiple = 24789.4;
        public const double nutnDivide = 1000000;


        // status labels in online mode
        public const string CONTINUOUSDISPLAY_ONLINE = "Kontinualan prikaz grafika je ";
        public const string DISCRETEDISPLAY_ONLINE = "Diskretan prikaz grafika je ";
        public const string REFRESHINTERVALTIME_ONLINE = "Vremenski interval osvežavanja grafika je ";
        public const string RESOLUTIONCONTINUOUS_ONLINE = "Rezolucija kontinualnog prikaza grafika je ";
        public const string RESOLUTIONDISCRETE_ONLINE = "Rezolucija diskretnog prikaza grafika je ";
        public const string L0_ONLINE = "Početna dužina u mm je ";
        public const string S0_ONLINE = "Početni poprečni presek u mm^2 je ";
        public const string FORCEDIV_ONLINE = "Koeficijent sa kojim se deli sila je ";
        public const string FORCEMUL_ONLINE = "Koeficijent sa kojim se množi sila je ";
        public const string ELONDIV_ONLINE = "Koeficijent sa kojim se deli izduženje je ";
        public const string ELONMUL_ONLINE = "Koeficijent sa kojim se množi izduženje je ";

        public const string ENDWRITINGTIMEINTERVAL_ONLINE = "Vremenski interval odredjivanja kraja online upisa je ";

        public const string FILEPATHPLOTTING = "Putanja fajla koji se prikazuje je : ";

        public const string ZERO = "0.0";

        public const int MARKERSIZE = 6;

        public const string NOTFOUNDMAXMIN = "Nije pronadjen";

        public const int CROSSHEADPOINT1 = 3;
        public const int CROSSHEADPOINT2 = 10;


        public const string NOTCALCULATETOTALRELATIVEELONGATION = "Nije izračunato relativno izduženje.";
        public const string CALCULATETOTALRELATIVEELONGATION = "Izračunato relativno izduženje  u [%] je ";

        public const string TOTALELONGATIONCAPTION = "Ukupno izduženje";


        //animation header file
        public const string L0 = "L0";
        public const string S0 = "S0";

        public const string ANIMATIONFILEHEADER_L0 = "Početna dužina [L0] : ";
        public const string ANIMATIONFILEHEADER_S0 = "Početna površina [S0] :";
        public const string ANIMATIONFILEHEADER_nutnDivide = "Delilac sile :  ";
        public const string ANIMATIONFILEHEADER_nutnMultiple = "Množitelj sile :  ";
        public const string ANIMATIONFILEHEADER_mmDivide = "Delilac izduženja :  ";
        public const string ANIMATIONFILEHEADER_mmCoeff = "Množitelj izduženja :  ";
        public const string ANIMATIONFILEHEADER_razmeraPreassure = "Napon[MPa]  :  ";
        public const string ANIMATIONFILEHEADER_razmeraElongation = "Izduženje[%]  :  ";
        public const string ANIMATIONFILEHEADER_refreshAnimationTime = "Refresh Animation Time :  ";
        public const string ANIMATIONFILEHEADER_maxChangeOfPreassure = "Max Promena napona :  ";
        public const string ANIMATIONFILEHEADER_resolution = "Resolution :  ";

        public const string ANIMATIONRATIOREMARKS = "Napomena : U online modu bila je čekirana opcija automatskog podešavanja";

        //online file
        public const string ONLINEFILE_NOTENTERSAMPLEDATA = "Nisu uneti podaci o uzorku!!!";

        //online header file 

        public const string ONLINEFILEHEADER_FIRSTLINE = "Izvestaj o ispitivanju materijala u mehanickoj laboratoriji ";
        

        #region GeneralData

        public const string ONLINEFILEHEADER_OPSTIPODACI = "OPŠTI PODACI";
        //public const string ONLINEFILEHEADER_BROJIZVESTAJA = "Broj izveštaja : ";
        public const string ONLINEFILEHEADER_OPERATOR = "Operator : ";
        public const string ONLINEFILEHEADER_BRZBIZVESTAJA = "Br. zb. izveštaja : ";
        public const string ONLINEFILEHEADER_BRUZORKA = "Br. uzorka : ";
        public const string ONLINEFILEHEADER_SARZA = "Šarža : ";
        public const string ONLINEFILEHEADER_RADNINALOG = "Radni nalog : ";
        public const string ONLINEFILEHEADER_NARUCILAC = "Naručilac : ";
       
        #endregion


        #region ConditionsOfTesting

        public const string ONLINEFILEHEADER_USLOVIISPITIVANJA = "USLOVI ISPITIVANJA";
        public const string ONLINEFILEHEADER_STANDARD = "Standard : ";
        public const string ONLINEFILEHEADER_METODA = "Metoda : ";
        public const string ONLINEFILEHEADER_STANDARDZAN = "Standard za n : ";
        public const string ONLINEFILEHEADER_MASINA = "Mašina : ";
        public const string ONLINEFILEHEADER_OPSEGMASINE = "Opseg  mašine : ";
        public const string ONLINEFILEHEADER_TEMPERATURA = "Temperatura : ";
        public const string ONLINEFILEHEADER_EKSTENZIOMETAR = "Ekstenziometar : ";

        #endregion

        #region MaterialForTesting

        public const string ONLINEFILEHEADER_MATERIJALISPITIVANJA = "MATERIJAL ISPITIVANJA";
        public const string ONLINEFILEHEADER_PROIZVODJAC = "Proizvodjač : ";
        public const string ONLINEFILEHEADER_DOBAVLJAC = "Dobavljač : ";
        public const string ONLINEFILEHEADER_POLAZNIKVALITET = "Polazni kvalitet : ";
        public const string ONLINEFILEHEADER_NAZIVNADEBLJINA = "Nazivna debljina : ";
        public const string ONLINEFILEHEADER_NACINPRERADE = "Način prerade : ";

        #endregion

        #region Epruveta

        public const string ONLINEFILEHEADER_EPRUVETA = "EPRUVETA";
        public const string ONLINEFILEHEADER_EPRUVETAOBLIK = "Oblik : ";
        public const string ONLINEFILEHEADER_TIP = "Tip : ";
        public const string ONLINEFILEHEADER_K = "k : ";
        public const string ONLINEFILEHEADER_PRAVACISPITIV = "Pravac ispitiv. : ";
        public const string ONLINEFILEHEADER_VRSTAEPRUVETE = "Vrsta epruvete : ";
        public const string ONLINEFILEHEADER_S0 = "S0 : ";
        public const string ONLINEFILEHEADER_L0 = "L0 : ";
        public const string ONLINEFILEHEADER_LC = "Lc : ";

        #endregion


        #region PositionOfTube

        public const string ONLINEFILEHEADER_POLOZAJEPRUVETE = "POLOŽAJ EPRUVETE U ODNOSU NA";
        public const string ONLINEFILEHEADER_PRAVACVALJANJA = "Pravac valjanja : ";
        public const string ONLINEFILEHEADER_SIRINATRAKE = "Širina trake : ";
        public const string ONLINEFILEHEADER_DUZINATRAKE = "Dužina trake : ";

        #endregion

        #region RemarkOfTesting

        public const string ONLINEFILEHEADER_NAPOMENA = "Napomena : ";

        #endregion

        public const string ONLINEFILEHEADER_BrojZapisa = "Broj zapisa :  ";
        public const string ONLINEFILEHEADER_SilaA = "Sila	a   ";

        //max size of remarksOfTesting
        public const double MAXREMARKSTESTINGLENGTH = 140;

        //path of creating sample reports
        public static string PATHOFSAMPLEREPORT = System.Environment.CurrentDirectory + "\\pojedinacanIzvestajTest.pdf";
        public static string PATHOFSUMREPORT = System.Environment.CurrentDirectory + "\\zbirniIzvestajTest.pdf";



        //labels for change of preassure and elongation
        public const string CHANGEOFPREASSURE_200ms = "Promena napona [MPa/200ms]:";
        public const string MAXCHANGEOFPREASSURE_200ms = "Max Promena napona[MPa/200ms]:";
        public const string CHANGEOFELONGATION_200ms = "Brzina promene deformacije [1/200ms]:";
        public const string MAXCHANGEOFELONGATION_200ms = "Max Promena izduženja   [1/200ms]:";

        public const string CHANGEOFPREASSURE_400ms = "Promena napona [MPa/400ms]:";
        public const string MAXCHANGEOFPREASSURE_400ms = "Max Promena napona[MPa/400ms]:";
        public const string CHANGEOFELONGATION_400ms = "Brzina promene deformacije [1/400ms]:";
        public const string MAXCHANGEOFELONGATION_400ms = "Max Promena izduženja   [1/400ms]:";

        public const string CHANGEOFPREASSURE_600ms = "Promena napona [MPa/600ms]:";
        public const string MAXCHANGEOFPREASSURE_600ms = "Max Promena napona[MPa/600ms]:";
        public const string CHANGEOFELONGATION_600ms = "Brzina promene deformacije [1/600ms]:";
        public const string MAXCHANGEOFELONGATION_600ms = "Max Promena izduženja   [1/600ms]:";

        public const string CHANGEOFPREASSURE_800ms = "Promena napona [MPa/800ms]:";
        public const string MAXCHANGEOFPREASSURE_800ms = "Max Promena napona[MPa/800ms]:";
        public const string CHANGEOFELONGATION_800ms = "Brzina promene deformacije [1/800ms]:";
        public const string MAXCHANGEOFELONGATION_800ms = "Max Promena izduženja   [1/800ms]:";

        public const string CHANGEOFPREASSURE_1000ms = "Promena napona [MPa/s]:";
        public const string MAXCHANGEOFPREASSURE_1000ms = "Max Promena napona[MPa/s]:";
        public const string CHANGEOFELONGATION_1000ms = "Brzina promene deformacije [1/s]:";
        public const string MAXCHANGEOFELONGATION_1000ms = "Max Promena izduženja   [1/s]:";

        public const string RESINTERFACE_MAXCHANGEOFPREASSURE_200ms = "MPa/200ms";
        public const string RESINTERFACE_ELONGATIONRange2_200ms = "1/200ms";

        public const string RESINTERFACE_MAXCHANGEOFPREASSURE_400ms = "MPa/400ms";
        public const string RESINTERFACE_ELONGATIONRange2_400ms = "1/400ms";

        public const string RESINTERFACE_MAXCHANGEOFPREASSURE_600ms = "MPa/600ms";
        public const string RESINTERFACE_ELONGATIONRange2_600ms = "1/600ms";

        public const string RESINTERFACE_MAXCHANGEOFPREASSURE_800ms = "MPa/800ms";
        public const string RESINTERFACE_ELONGATIONRange2_800ms = "1/800ms";

        public const string RESINTERFACE_MAXCHANGEOFPREASSURE_1000ms = "MPa/s";
        public const string RESINTERFACE_ELONGATIONRange2_1000ms = "1/s";

        #region radioButtonsValues

        public const string VALJANI = "VALJANI";
        public const string VUČENI = "VUČENI";
        public const string KOVANI = "KOVANI";
        public const string LIVENI = "LIVENI";

        public const string DA = "DA";
        public const string NE = "NE";

        public const string OBRADJENA = "OBRADJENA";
        public const string NEOBRADJENA = "NEOBRADJENA";
        public const string NEPROPORCIONALNA = "NEPROPORCIONALNA";
        public const string PROPORCIONALNA = "PROPORCIONALNA";

        public const string K1 = "5.65";
        public const string K2 = "11.3";

        public const string PRAVOUGAONA = "PRAVOUGAONA";
        public const string KRUZNA = "KRUŽNA";
        public const string CEVASTA = "CEVASTA";
        public const string DEOCEVI = "DEO CEVI";
        public const string SESTAUGAONA = "ŠESTAUGAONA";
        public const string a0 = "a0";
        public const string b0 = "b0";
        public const string d0 = "d0";
        public const string D0 = "D0";
        public const string au = "au";
        public const string bu = "bu";
        public const string du = "du";
        public const string Du = "Du";

       
        public const string zeroStr = "0 \xB0";
        public const string fortyfiveStr = "45 \xB0";
        public const string ninetyStr = "90 \xB0";
        public const string customDegree = "\xB0";

        public const string IVICE = "IVICE";
        public const string SREDINU = "SREDINU";

        public const string POCETAK = "POČETAK";
        public const string SREDINU2 = "SREDINU";
        public const string KRAJ = "KRAJ";


        #endregion

        #region resultsInterfaceLabels

        public const string Lu = "Lu";
        public const string rbtnRp02 = "rbtnRp02";
        public const string Rp02 = "Rp02";
        public const string Rp02Manual = "Rp02Manual";
        public const string Rp02X = "Rp02X";
        public const string chbRp02 = "chbRp02";
        public const string rbtnRt05 = "rbtnRt05";
        public const string Rt05 = "Rt05";
        public const string chbRt05 = "chbRt05";
        public const string rbtnReL = "rbtnReL";
        public const string ReL = "ReL";
        public const string ReLManual = "ReLManual";
        public const string ReLX = "ReLX";
        public const string chbReL = "chbReL";
        public const string rbtnReH = "rbtnReH";
        public const string ReH = "ReH";
        public const string ReHManual = "ReHManual";
        public const string ReHX = "ReHX";
        public const string chbReH = "chbReH";
        public const string Rm = "Rm";
        public const string RmManual = "RmManual";
        public const string RmX = "RmX";
        public const string R_Rm = "R/Rm";
        public const string F = "F";
        public const string Fm = "Fm";
        public const string FmManual = "FmManual";
        public const string Ag = "Ag";
        public const string Agt = "Agt";
        public const string A = "A";
        public const string AManual = "AManual";
        public const string At = "At";
        public const string Su = "Su";
        public const string Z = "Z";
        public const string n = "n";
        public const string chbn = "chbn";
        public const string chbRmax = "chbRmax";
        public const string chbe2 = "chbe2";
        public const string chbe4 = "chbe4";
        public const string chbRe = "chbRe";
        public const string chbE = "chbE";
        public const string Rmax = "Rmax";
        public const string eR2 = "e(R2)";
        public const string eR4 = "e(R4)";
        public const string Re = "Re";
        public const string E = "E";
        public const string Du2 = "Du";

        #endregion

        #region measures

        public const string mm = "mm";
        public const string mm2 = "mm\xB2";
        public const string kN = "kN";
        public const string N = "N";
        public const string MPa = "MPa";
        public const string GPa = "GPa";
        public const string MPa2 = "MPa/";
        public const string jedan = "1/";
        public const string secNaMinusJedan = "1/s";
        public const string ms = "ms";
        public const string procent = "%";
        public const string MPa_200ms = "MPa/200ms";
        public const string MPa_400ms = "MPa/400ms";
        public const string MPa_600ms = "MPa/600ms";
        public const string MPa_800ms = "MPa/800ms";
        public const string MPa_1000ms = "MPa/s";

        #endregion

        public const string KOSA_CRTA = "/";
        public const string VRSTAEPRUVETE = "Vrsta epruvete :";


        #region XMLOfSampleReports

        #region XML_roots

        public const string XML_roots_ROOT2 = "ZbirniIzvestaj";
        public const string XML_roots_ROOT = "PojedinacniIzvestaj";
        public const string XML_roots_Sadrzaj = "Sadrzaj";
        public const string XML_roots_Uzorak = "Uzorak";

        #endregion

        #region XML_GeneralData

        public const string XML_GeneralData_OPERATOR = "Operator";
        public const string XML_GeneralData_BRZBIZVESTAJA = "BrZbIzvestaja";
        public const string XML_GeneralData_BRUZORKA = "BrUzorka";
        public const string XML_GeneralData_BRUZORKA_SAMALIMBSLOVOM = "brUzorka";
        public const string XML_GeneralData_BRUZORKAKLATNO = "brUzorka";
        public const string XML_GeneralData_SARZA = "Sarza";
        public const string XML_GeneralData_RADNINALOG = "RadniNalog";
        public const string XML_GeneralData_NARUCILAC = "Narucilac";

        #endregion

        #region XML_ConditionsOfTesting

        public const string XML_ConditionsOfTesting_STANDARD = "Standard";
        public const string XML_ConditionsOfTesting_METODA = "Metoda";
        public const string XML_ConditionsOfTesting_STANDARDZAN = "StandardZaN";
        public const string XML_ConditionsOfTesting_MASINA = "Masina";
        public const string XML_ConditionsOfTesting_OPSEGMASINE = "OpsegMasine";
        public const string XML_ConditionsOfTesting_TEMPERATURA = "Temperatura";
        public const string XML_ConditionsOfTesting_EKSTENZIOMETAR = "Ekstenziometar";

        #endregion

        #region XML_MaterialForTesting

        public const string XML_MaterialForTesting_PROIZVODJAC = "Proizvodjac";
        public const string XML_MaterialForTesting_DOBAVLJAC = "Dobavljac";
        public const string XML_MaterialForTesting_POLAZNIKVALITET = "PolazniKvalitet";
        public const string XML_MaterialForTesting_NAZIVNADEBLJINA = "NazivnaDebljina";
        public const string XML_MaterialForTesting_NACINPRERADE = "NacinPrerade";

        #endregion


        #region XML_Epruveta

        public const string XML_Epruveta_EPRUVETAOBLIK = "Oblik";
        public const string XML_Epruveta_TIP = "Tip";
        public const string XML_Epruveta_K = "k";
        public const string XML_Epruveta_VRSTAEPRUVETE = "VrstaEpruvete";
        public const string XML_Epruveta_S0 = "S0";
        public const string XML_Epruveta_L0 = "L0";
        public const string XML_Epruveta_LC = "Lc";

        #endregion

        #region XML_PositionOfTube

        public const string XML_PositionOfTube_PRAVACVALJANJA = "PravacValjanja";
        public const string XML_PositionOfTube_SIRINATRAKE = "SirinaTrake";
        public const string XML_PositionOfTube_DUZINATRAKE = "DuzinaTrake";

        #endregion

        #region XML_RemarkOfTesting

        public const string XML_RemarkOfTesting_NAPOMENA = "Napomena";

        #endregion

        #region XML_ResultsInterface

        public const string XML_ResultsInterface_Lu = "Lu";
        public const string XML_ResultsInterface_Rp02 = "Rp02";
        public const string XML_ResultsInterface_Rt05 = "Rt05";
        public const string XML_ResultsInterface_ReL = "ReL";
        public const string XML_ResultsInterface_ReH = "ReH";
        public const string XML_ResultsInterface_Rm = "Rm";
        public const string XML_ResultsInterface_Rp02_Rm = "Rp02_Rm";
        public const string XML_ResultsInterface_Rt05_Rm = "Rt05_Rm";
        public const string XML_ResultsInterface_ReL_Rm = "ReL_Rm";
        public const string XML_ResultsInterface_ReH_Rm = "ReH_Rm";
        public const string XML_ResultsInterface_F = "F";
        public const string XML_ResultsInterface_Fm = "Fm";
        public const string XML_ResultsInterface_Ag = "Ag";
        public const string XML_ResultsInterface_Agt = "Agt";
        public const string XML_ResultsInterface_A = "A";
        public const string XML_ResultsInterface_At = "At";
        public const string XML_ResultsInterface_Su = "Su";
        public const string XML_ResultsInterface_Z = "Z";
        public const string XML_ResultsInterface_n = "n";
        public const string XML_ResultsInterface_Rmax = "Rmax";
        public const string XML_isE2E4BorderSelected = "isE2E4BorderSelected";
        public const string XML_ResultsInterface_eR2 = "e_R2";
        public const string XML_ResultsInterface_eR4 = "e_R4";
        public const string XML_ResultsInterface_Re = "Re";
        public const string XML_ResultsInterface_E = "E";
        public const string XML_ResultsInterface_Du = "Du";
        public const string XML_ResultsInterface_au = "au";
        public const string XML_ResultsInterface_bu = "bu";

        #endregion

        public const string XML_ShowGraphicChangeOfRAndE = "PrikaziGrafikPromenaRiE";
        public const string XML_ManualnIsCalculated = "RucnonJeIzracunato";
        public const string XML_R1 = "R1";
        public const string XML_R2 = "R2";
        public const string XML_R3 = "R3";
        public const string XML_R4 = "R4";
        public const string XML_R5 = "R5";
        public const string XML_manualN = "Rucnon";
        public const string XML_manualN_BeginInterval = "Rucnon_PocetakIntervala";
        public const string XML_manualN_EndInterval = "Rucnon_KrajIntervala";

        #endregion

        #region XMLOfSumReports

        #region XML_roots_SUMReport

        public const string XML_root_SUMReport = "ZbirniIzvestaj";
        public const string XML_Uzorak_SUMReport = "Uzorak";

        #endregion

        #region XML_input_SUMReport

        public const string XML_brzbIzvestaja_SUMReport = "brzbIzvestaja";
        public const string XML_polazniKvalitet_SUMReport = "polazniKvalitet";
        public const string XML_nazivnaDebljina_SUMReport = "nazivnaDebljina";
        public const string XML_ispitivac_SUMReport = "ispitivac";
        public const string XML_brUzorka_SUMReport = "brUzorka";
        public const string XML_sarza_SUMReport = "sarza";
        public const string XML_napomena_SUMReport = "napomena";
        public const string XML_KV_SUMReport = "KV";
        public const string XML_KU_SUMReport = "KU";

        #endregion

        #region XML_output_SUMReport

        public const string XML_Rm_SUMReport = "rm";
        public const string XML_Rp02_SUMReport = "rp02";
        public const string XML_Rt05_SUMReport = "rt05";
        public const string XML_ReL_SUMReport = "reL";
        public const string XML_ReH_SUMReport = "reH";
        public const string XML_A_SUMReport = "a";
        public const string XML_At_SUMReport = "at";
        public const string XML_N_SUMReport = "n";
        public const string XML_Z_SUMReport = "z";

        #endregion

        #endregion

    }
}
