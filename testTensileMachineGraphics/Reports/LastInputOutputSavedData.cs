using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Xml;

namespace testTensileMachineGraphics.Reports
{
    public class LastInputOutputSavedData
    {
        #region members

        //input data

        private static string _tfOperator_GeneralData = String.Empty;
        private static string _tfBrZbIzvestaja_GeneralData = String.Empty;
        private static string _tfBrUzorka_GeneralData = String.Empty;
        private static string _tfSarza_GeneralData = String.Empty;
        private static string _tfRadniNalog_GeneralData = String.Empty;
        private static string _tfNarucilac_GeneralData = String.Empty;
        private static string _tfStandard_ConditionsOfTesting = String.Empty;
        private static string _tfMetoda_ConditionsOfTesting = String.Empty;
        private static string _tfStandardZaN_ConditionsOfTesting = String.Empty;
        private static string _tfMasina_ConditionsOfTesting = String.Empty;
        private static string _tfBegOpsegMasine_ConditionsOfTesting = String.Empty;
        private static string _tfTemperatura_ConditionsOfTesting = String.Empty;
        private static string _rbtnYes_ConditionsOfTesting = String.Empty;
        private static string _rbtnNo_ConditionsOfTesting = String.Empty;
        private static string _tfProizvodjac_MaterialForTesting = String.Empty;
        private static string _tfDobavljac_MaterialForTesting = String.Empty;
        private static string _tfPolazniKvalitet_MaterialForTesting = String.Empty;
        private static string _tfNazivnaDebljina_MaterialForTesting = String.Empty;
        private static string _rbtnValjani_MaterialForTesting = String.Empty;
        private static string _rbtnVuceni_MaterialForTesting = String.Empty;
        private static string _rbtnKovani_MaterialForTesting = String.Empty;
        private static string _rbtnLiveni_MaterialForTesting = String.Empty;
        private static string _rbtnEpvOblikObradjena = String.Empty;
        private static string _rbtnEpvOblikNeobradjena = String.Empty;
        private static string _rbtnEpvTipProporcionalna = String.Empty;
        private static string _rbtnEpvTipNeproporcionalna = String.Empty;
        private static string _rbtnEpvK1 = String.Empty;
        private static string _rbtnEpvK2 = String.Empty;
        private static string _rbtnEpvVrstaPravougaona = String.Empty;
        private static string _a0Pravougaona = String.Empty;
        private static string _b0Pravougaona = String.Empty;
        private static string _rbtnEpvVrstaKruzna = String.Empty;
        private static string _D0Kruzna = String.Empty;
        private static string _rbtnEpvVrstaCevasta = String.Empty;
        private static string _D0Cevasta = String.Empty;
        private static string _a0Cevasta = String.Empty;
        private static string _rbtnEpvVrstaDeocev = String.Empty;
        private static string _D0Deocev = String.Empty;
        private static string _a0Deocev = String.Empty;
        private static string _b0Deocev = String.Empty;
        private static string _rbtnEpvVrstaSestaugaona = String.Empty;
        private static string _d0Sestaugaona = String.Empty;
        private static string _au = String.Empty;
        private static string _bu = String.Empty;
        private static string _du = String.Empty;
        private static string _Du = String.Empty;
       
        
        
       
        private static string _tfS0 = String.Empty;
        private static string _tfL0 = String.Empty;
        private static string _tfLc = String.Empty;
        private static string _tfCustomPravacValjanja_PositionOfTube = String.Empty;
        private static string _tfCustomSirinaTrake_PositionOfTube = String.Empty;
        private static string _tfCustomDuzinaTrake_PositionOfTube = String.Empty;
        private static string _rtfNapomena_RemarkOfTesting = String.Empty;

        //output data

        private static string _tfLu_ResultsInterface = String.Empty;
        private static string _chbRp02_ResultsInterface = String.Empty;
        private static string _rbtnRp02_ResultsInterface = String.Empty;
        private static string _tfRp02_ResultsInterface = String.Empty;
        private static string _chbRt05_ResultsInterface = String.Empty;
        private static string _rbtnRt05_ResultsInterface = String.Empty;
        private static string _tfRt05_ResultsInterface = String.Empty;
        private static string _chbReL_ResultsInterface = String.Empty;
        private static string _rbtnReL_ResultsInterface = String.Empty;
        private static string _tfReL_ResultsInterface = String.Empty;
        private static string _chbReH_ResultsInterface = String.Empty;
        private static string _rbtnReH_ResultsInterface = String.Empty;
        private static string _tfReH_ResultsInterface = String.Empty;
        private static string _tfRm_ResultsInterface = String.Empty;
        private static string _tfF_ResultsInterface = String.Empty;
        private static string _tfFm_ResultsInterface = String.Empty;
        private static string _tfAg_ResultsInterface = String.Empty;
        private static string _tfAgt_ResultsInterface = String.Empty;
        private static string _tfRRm_ResultsInterface = String.Empty;
        private static string _tfA_ResultsInterface = String.Empty;
        private static string _tfAt_ResultsInterface = String.Empty;
        private static string _tfSu_ResultsInterface = String.Empty;
        private static string _tfZ_ResultsInterface = String.Empty;
        private static string _chbn_ResultsInterface = String.Empty;
        private static string _chbRmax_ResultsInterface = String.Empty;
        private static string _tfn_ResultsInterface = String.Empty;
        private static string _tfRmaxWithPoint_ResultsInterface = String.Empty;
        private static string _isE2E4BorderSelected = String.Empty;
        private static string _chbe2_ResultsInterface = String.Empty;
        private static string _e2Min_ResultsInterface = String.Empty;
        private static string _e2Max_ResultsInterface = String.Empty;
        private static string _chbe4_ResultsInterface = String.Empty;
        private static string _e4Min_ResultsInterface = String.Empty;
        private static string _e4Max_ResultsInterface = String.Empty;
        private static string _Re_ResultsInterface = String.Empty;
        private static string _chbRe_ResultsInterface = String.Empty;
        private static string _E_ResultsInterface = String.Empty;
        private static string _chbE_ResultsInterface = String.Empty;

        //manual calculation of n
        private static string _R1 = String.Empty;
        private static string _R2 = String.Empty;
        private static string _R3 = String.Empty;
        private static string _R4 = String.Empty;
        private static string _R5 = String.Empty;
        private static string _Manualn = String.Empty;

        #endregion

        #region constructors

        #endregion

        #region properties


        //input data

        public static string tfOperator_GeneralData 
        {
            get { return _tfOperator_GeneralData; }
            set { _tfOperator_GeneralData = value; }
        }
        public static string tfBrZbIzvestaja_GeneralData
        {
            get { return _tfBrZbIzvestaja_GeneralData; }
            set { _tfBrZbIzvestaja_GeneralData = value; }
        }
        public static string tfBrUzorka_GeneralData
        {
            get { return _tfBrUzorka_GeneralData; }
            set { _tfBrUzorka_GeneralData = value; }
        }
        public static string tfSarza_GeneralData
        {
            get { return _tfSarza_GeneralData; }
            set { _tfSarza_GeneralData = value; }
        }
        public static string tfRadniNalog_GeneralData
        {
            get { return _tfRadniNalog_GeneralData; }
            set { _tfRadniNalog_GeneralData = value; }
        }
        public static string tfNarucilac_GeneralData
        {
            get { return _tfNarucilac_GeneralData; }
            set { _tfNarucilac_GeneralData = value; }
        }
        public static string tfStandard_ConditionsOfTesting
        {
            get { return _tfStandard_ConditionsOfTesting; }
            set { _tfStandard_ConditionsOfTesting = value; }
        }
        public static string tfMetoda_ConditionsOfTesting
        {
            get { return _tfMetoda_ConditionsOfTesting; }
            set { _tfMetoda_ConditionsOfTesting = value; }
        }
        public static string tfStandardZaN_ConditionsOfTesting
        {
            get { return _tfStandardZaN_ConditionsOfTesting; }
            set { _tfStandardZaN_ConditionsOfTesting = value; }
        }
        public static string tfMasina_ConditionsOfTesting
        {
            get { return _tfMasina_ConditionsOfTesting; }
            set { _tfMasina_ConditionsOfTesting = value; }
        }
        public static string tfBegOpsegMasine_ConditionsOfTesting
        {
            get { return _tfBegOpsegMasine_ConditionsOfTesting; }
            set { _tfBegOpsegMasine_ConditionsOfTesting = value; }
        }
        public static string tfTemperatura_ConditionsOfTesting
        {
            get { return _tfTemperatura_ConditionsOfTesting; }
            set { _tfTemperatura_ConditionsOfTesting = value; }
        }
        public static string rbtnYes_ConditionsOfTesting
        {
            get { return _rbtnYes_ConditionsOfTesting; }
            set { _rbtnYes_ConditionsOfTesting = value; }
        }
        public static string rbtnNo_ConditionsOfTesting
        {
            get { return _rbtnNo_ConditionsOfTesting; }
            set { _rbtnNo_ConditionsOfTesting = value; }
        }
        public static string tfProizvodjac_MaterialForTesting
        {
            get { return _tfProizvodjac_MaterialForTesting; }
            set { _tfProizvodjac_MaterialForTesting = value; }
        }
        public static string tfDobavljac_MaterialForTesting
        {
            get { return _tfDobavljac_MaterialForTesting; }
            set { _tfDobavljac_MaterialForTesting = value; }
        }
        public static string tfPolazniKvalitet_MaterialForTesting
        {
            get { return _tfPolazniKvalitet_MaterialForTesting; }
            set { _tfPolazniKvalitet_MaterialForTesting = value; }
        }
        public static string tfNazivnaDebljina_MaterialForTesting
        {
            get { return _tfNazivnaDebljina_MaterialForTesting; }
            set { _tfNazivnaDebljina_MaterialForTesting = value; }
        }
        public static string rbtnValjani_MaterialForTesting
        {
            get { return _rbtnValjani_MaterialForTesting; }
            set { _rbtnValjani_MaterialForTesting = value; }
        }
        public static string rbtnVuceni_MaterialForTesting
        {
            get { return _rbtnVuceni_MaterialForTesting; }
            set { _rbtnVuceni_MaterialForTesting = value; }
        }
        public static string rbtnKovani_MaterialForTesting
        {
            get { return _rbtnKovani_MaterialForTesting; }
            set { _rbtnKovani_MaterialForTesting = value; }
        }
        public static string rbtnLiveni_MaterialForTesting
        {
            get { return _rbtnLiveni_MaterialForTesting; }
            set { _rbtnLiveni_MaterialForTesting = value; }
        }
        public static string rbtnEpvOblikObradjena
        {
            get { return _rbtnEpvOblikObradjena; }
            set { _rbtnEpvOblikObradjena = value; }
        }
        public static string rbtnEpvOblikNeobradjena
        {
            get { return _rbtnEpvOblikNeobradjena; }
            set { _rbtnEpvOblikNeobradjena = value; }
        }
        public static string rbtnEpvTipProporcionalna
        {
            get { return _rbtnEpvTipProporcionalna; }
            set { _rbtnEpvTipProporcionalna = value; }
        }
        public static string rbtnEpvTipNeproporcionalna
        {
            get { return _rbtnEpvTipNeproporcionalna; }
            set { _rbtnEpvTipNeproporcionalna = value; }
        }
        public static string rbtnEpvK1
        {
            get { return _rbtnEpvK1; }
            set { _rbtnEpvK1 = value; }
        }
        public static string rbtnEpvK2
        {
            get { return _rbtnEpvK2; }
            set { _rbtnEpvK2 = value; }
        }
        public static string rbtnEpvVrstaPravougaona
        {
            get { return _rbtnEpvVrstaPravougaona; }
            set { _rbtnEpvVrstaPravougaona = value; }
        }
        public static string a0Pravougaona
        {
            get { return _a0Pravougaona; }
            set { _a0Pravougaona = value; }
        }
        public static string b0Pravougaona
        {
            get { return _b0Pravougaona; }
            set { _b0Pravougaona = value; }
        }
        public static string rbtnEpvVrstaKruzna
        {
            get { return _rbtnEpvVrstaKruzna; }
            set { _rbtnEpvVrstaKruzna = value; }
        }
        public static string D0Kruzna
        {
            get { return _D0Kruzna; }
            set { _D0Kruzna = value; }
        }
        public static string rbtnEpvVrstaCevasta
        {
            get { return _rbtnEpvVrstaCevasta; }
            set { _rbtnEpvVrstaCevasta = value; }
        }
        public static string D0Cevasta
        {
            get { return _D0Cevasta; }
            set { _D0Cevasta = value; }
        }
        public static string a0Cevasta
        {
            get { return _a0Cevasta; }
            set { _a0Cevasta = value; }
        }
        public static string rbtnEpvVrstaDeocev
        {
            get { return _rbtnEpvVrstaDeocev; }
            set { _rbtnEpvVrstaDeocev = value; }
        }
        public static string D0Deocev
        {
            get { return _D0Deocev; }
            set { _D0Deocev = value; }
        }
        public static string a0Deocev
        {
            get { return _a0Deocev; }
            set { _a0Deocev = value; }
        }
        public static string b0Deocev
        {
            get { return _b0Deocev; }
            set { _b0Deocev = value; }
        }
        public static string rbtnEpvVrstaSestaugaona
        {
            get { return _rbtnEpvVrstaSestaugaona; }
            set { _rbtnEpvVrstaSestaugaona = value; }
        }
        public static string d0Sestaugaona
        {
            get { return _d0Sestaugaona; }
            set { _d0Sestaugaona = value; }
        }
        public static string au
        {
            get { return _au; }
            set { _au = value; }
        }
        public static string bu
        {
            get { return _bu; }
            set { _bu = value; }
        }
        public static string du
        {
            get { return _du; }
            set { _du = value; }
        }
        public static string Du
        {
            get { return _Du; }
            set { _Du = value; }
        }
        public static string tfS0
        {
            get { return _tfS0; }
            set { _tfS0 = value; }
        }
        public static string tfL0
        {
            get { return _tfL0; }
            set { _tfL0 = value; }
        }
        public static string tfLc
        {
            get { return _tfLc; }
            set { _tfLc = value; }
        }
        public static string tfCustomPravacValjanja_PositionOfTube
        {
            get { return _tfCustomPravacValjanja_PositionOfTube; }
            set { _tfCustomPravacValjanja_PositionOfTube = value; }
        }
        public static string tfCustomSirinaTrake_PositionOfTube
        {
            get { return _tfCustomSirinaTrake_PositionOfTube; }
            set { _tfCustomSirinaTrake_PositionOfTube = value; }
        }
        public static string tfCustomDuzinaTrake_PositionOfTube
        {
            get { return _tfCustomDuzinaTrake_PositionOfTube; }
            set { _tfCustomDuzinaTrake_PositionOfTube = value; }
        }
        public static string rtfNapomena_RemarkOfTesting
        {
            get { return _rtfNapomena_RemarkOfTesting; }
            set { _rtfNapomena_RemarkOfTesting = value; }
        }



        //output data
        public static string tfLu_ResultsInterface
        {
            get { return _tfLu_ResultsInterface; }
            set { _tfLu_ResultsInterface = value; }
        }
        public static string chbRp02_ResultsInterface
        {
            get { return _chbRp02_ResultsInterface; }
            set { _chbRp02_ResultsInterface = value; }
        }
        public static string rbtnRp02_ResultsInterface
        {
            get { return _rbtnRp02_ResultsInterface; }
            set { _rbtnRp02_ResultsInterface = value; }
        }
        public static string tfRp02_ResultsInterface
        {
            get { return _tfRp02_ResultsInterface; }
            set { _tfRp02_ResultsInterface = value; }
        }
        public static string chbRt05_ResultsInterface
        {
            get { return _chbRt05_ResultsInterface; }
            set { _chbRt05_ResultsInterface = value; }
        }
        public static string rbtnRt05_ResultsInterface
        {
            get { return _rbtnRt05_ResultsInterface; }
            set { _rbtnRt05_ResultsInterface = value; }
        }
        public static string tfRt05_ResultsInterface
        {
            get { return _tfRt05_ResultsInterface; }
            set { _tfRt05_ResultsInterface = value; }
        }
        public static string chbReL_ResultsInterface
        {
            get { return _chbReL_ResultsInterface; }
            set { _chbReL_ResultsInterface = value; }
        }
        public static string rbtnReL_ResultsInterface
        {
            get { return _rbtnReL_ResultsInterface; }
            set { _rbtnReL_ResultsInterface = value; }
        }
        public static string tfReL_ResultsInterface
        {
            get { return _tfReL_ResultsInterface; }
            set { _tfReL_ResultsInterface = value; }
        }
        public static string chbReH_ResultsInterface
        {
            get { return _chbReH_ResultsInterface; }
            set { _chbReH_ResultsInterface = value; }
        }
        public static string rbtnReH_ResultsInterface
        {
            get { return _rbtnReH_ResultsInterface; }
            set { _rbtnReH_ResultsInterface = value; }
        }
        public static string tfReH_ResultsInterface
        {
            get { return _tfReH_ResultsInterface; }
            set { _tfReH_ResultsInterface = value; }
        }
        public static string tfRm_ResultsInterface
        {
            get { return _tfRm_ResultsInterface; }
            set { _tfRm_ResultsInterface = value; }
        }
        public static string tfF_ResultsInterface
        {
            get { return _tfF_ResultsInterface; }
            set { _tfF_ResultsInterface = value; }
        }
        public static string tfFm_ResultsInterface
        {
            get { return _tfFm_ResultsInterface; }
            set { _tfFm_ResultsInterface = value; }
        }
        public static string tfAg_ResultsInterface
        {
            get { return _tfAg_ResultsInterface; }
            set { _tfAg_ResultsInterface = value; }
        }
        public static string tfAgt_ResultsInterface
        {
            get { return _tfAgt_ResultsInterface; }
            set { _tfAgt_ResultsInterface = value; }
        }
        public static string tfRRm_ResultsInterface
        {
            get { return _tfRRm_ResultsInterface; }
            set { _tfRRm_ResultsInterface = value; }
        }
        public static string tfA_ResultsInterface
        {
            get { return _tfA_ResultsInterface; }
            set { _tfA_ResultsInterface = value; }
        }
        public static string tfAt_ResultsInterface
        {
            get { return _tfAt_ResultsInterface; }
            set { _tfAt_ResultsInterface = value; }
        }
        public static string tfSu_ResultsInterface
        {
            get { return _tfSu_ResultsInterface; }
            set { _tfSu_ResultsInterface = value; }
        }
        public static string tfZ_ResultsInterface
        {
            get { return _tfZ_ResultsInterface; }
            set { _tfZ_ResultsInterface = value; }
        }
        public static string chbn_ResultsInterface
        {
            get { return _chbn_ResultsInterface; }
            set { _chbn_ResultsInterface = value; }
        }
        public static string tfn_ResultsInterface
        {
            get { return _tfn_ResultsInterface; }
            set { _tfn_ResultsInterface = value; }
        }
        public static string chbRmax_ResultsInterface
        {
            get { return _chbRmax_ResultsInterface; }
            set { _chbRmax_ResultsInterface = value; }
        }
        public static string tfRmaxWithPoint_ResultsInterface
        {
            get { return _tfRmaxWithPoint_ResultsInterface; }
            set { _tfRmaxWithPoint_ResultsInterface = value; }
        }
        public static string isE2E4BorderSelected
        {
            get { return _isE2E4BorderSelected; }
            set { _isE2E4BorderSelected = value; }
        }
        public static string chbe2_ResultsInterface
        {
            get { return _chbe2_ResultsInterface; }
            set { _chbe2_ResultsInterface = value; }
        }
        public static string e2Min_ResultsInterface
        {
            get { return _e2Min_ResultsInterface; }
            set { _e2Min_ResultsInterface = value; }
        }
        public static string e2Max_ResultsInterface
        {
            get { return _e2Max_ResultsInterface; }
            set { _e2Max_ResultsInterface = value; }
        }
        public static string chbe4_ResultsInterface
        {
            get { return _chbe4_ResultsInterface; }
            set { _chbe4_ResultsInterface = value; }
        }
        public static string e4Min_ResultsInterface
        {
            get { return _e4Min_ResultsInterface; }
            set { _e4Min_ResultsInterface = value; }
        }
        public static string e4Max_ResultsInterface
        {
            get { return _e4Max_ResultsInterface; }
            set { _e4Max_ResultsInterface = value; }
        }
        public static string Re_ResultsInterface
        {
            get { return _Re_ResultsInterface; }
            set { _Re_ResultsInterface = value; }
        }
        public static string chbRe_ResultsInterface
        {
            get { return _chbRe_ResultsInterface; }
            set { _chbRe_ResultsInterface = value; }
        }
        public static string E_ResultsInterface
        {
            get { return _E_ResultsInterface; }
            set { _E_ResultsInterface = value; }
        }
        public static string chbE_ResultsInterface
        {
            get { return _chbE_ResultsInterface; }
            set { _chbE_ResultsInterface = value; }
        }


        //manual calculation of n
        public static string R1
        {
            get { return _R1; }
            set { _R1 = value; }
        }
        public static string R2
        {
            get { return _R2; }
            set { _R2 = value; }
        }
        public static string R3
        {
            get { return _R3; }
            set { _R3 = value; }
        }
        public static string R4
        {
            get { return _R4; }
            set { _R4 = value; }
        }
        public static string R5
        {
            get { return _R5; }
            set { _R5 = value; }
        }

        public static string Manualn
        {
            get { return _Manualn; }
            set { _Manualn = value; }
        }

        #endregion


        #region methods

        private static void clearData() 
        {
            try
            {

                //input data

                _tfOperator_GeneralData = String.Empty;
                _tfBrZbIzvestaja_GeneralData = String.Empty;
                _tfBrUzorka_GeneralData = String.Empty;
                _tfSarza_GeneralData = String.Empty;
                _tfRadniNalog_GeneralData = String.Empty;
                _tfNarucilac_GeneralData = String.Empty;
                _tfStandard_ConditionsOfTesting = String.Empty;
                _tfMetoda_ConditionsOfTesting = String.Empty;
                _tfStandardZaN_ConditionsOfTesting = String.Empty;
                _tfMasina_ConditionsOfTesting = String.Empty;
                _tfBegOpsegMasine_ConditionsOfTesting = String.Empty;
                _tfTemperatura_ConditionsOfTesting = String.Empty;
                _rbtnYes_ConditionsOfTesting = String.Empty;
                _rbtnNo_ConditionsOfTesting = String.Empty;
                _tfProizvodjac_MaterialForTesting = String.Empty;
                _tfDobavljac_MaterialForTesting = String.Empty;
                _tfPolazniKvalitet_MaterialForTesting = String.Empty;
                _tfNazivnaDebljina_MaterialForTesting = String.Empty;
                _rbtnValjani_MaterialForTesting = String.Empty;
                _rbtnVuceni_MaterialForTesting = String.Empty;
                _rbtnKovani_MaterialForTesting = String.Empty;
                _rbtnLiveni_MaterialForTesting = String.Empty;
                _rbtnEpvOblikObradjena = String.Empty;
                _rbtnEpvOblikNeobradjena = String.Empty;
                _rbtnEpvTipProporcionalna = String.Empty;
                _rbtnEpvTipNeproporcionalna = String.Empty;
                _rbtnEpvK1 = String.Empty;
                _rbtnEpvK2 = String.Empty;
                _rbtnEpvVrstaPravougaona = String.Empty;
                _a0Pravougaona = String.Empty;
                _b0Pravougaona = String.Empty;
                _rbtnEpvVrstaKruzna = String.Empty;
                _D0Kruzna = String.Empty;
                _rbtnEpvVrstaCevasta = String.Empty;
                _D0Cevasta = String.Empty;
                _a0Cevasta = String.Empty;
                _rbtnEpvVrstaDeocev = String.Empty;
                _D0Deocev = String.Empty;
                _a0Deocev = String.Empty;
                _b0Deocev = String.Empty;
                _rbtnEpvVrstaSestaugaona = String.Empty;
                _d0Sestaugaona = String.Empty;
                _au = String.Empty;
                _bu = String.Empty;
                _du = String.Empty;
                _Du = String.Empty;




                _tfS0 = String.Empty;
                _tfL0 = String.Empty;
                _tfLc = String.Empty;

                _tfCustomPravacValjanja_PositionOfTube = String.Empty;
                _tfCustomSirinaTrake_PositionOfTube = String.Empty;
                _tfCustomDuzinaTrake_PositionOfTube = String.Empty;

                _rtfNapomena_RemarkOfTesting = String.Empty;

                //output data

                _tfLu_ResultsInterface = String.Empty;
                _chbRp02_ResultsInterface = String.Empty;
                _rbtnRp02_ResultsInterface = String.Empty;
                _tfRp02_ResultsInterface = String.Empty;
                _chbRt05_ResultsInterface = String.Empty;
                _rbtnRt05_ResultsInterface = String.Empty;
                _tfRt05_ResultsInterface = String.Empty;
                _chbReL_ResultsInterface = String.Empty;
                _rbtnReL_ResultsInterface = String.Empty;
                _tfReL_ResultsInterface = String.Empty;
                _chbReH_ResultsInterface = String.Empty;
                _rbtnReH_ResultsInterface = String.Empty;
                _tfReH_ResultsInterface = String.Empty;
                _tfRm_ResultsInterface = String.Empty;
                _tfF_ResultsInterface = String.Empty;
                _tfFm_ResultsInterface = String.Empty;
                _tfAg_ResultsInterface = String.Empty;
                _tfAgt_ResultsInterface = String.Empty;
                _tfRRm_ResultsInterface = String.Empty;
                _tfA_ResultsInterface = String.Empty;
                _tfAt_ResultsInterface = String.Empty;
                _tfSu_ResultsInterface = String.Empty;
                _tfZ_ResultsInterface = String.Empty;
                _chbn_ResultsInterface = String.Empty;
                _tfn_ResultsInterface = String.Empty;
                _tfRmaxWithPoint_ResultsInterface = String.Empty;
                _e2Min_ResultsInterface = String.Empty;
                _e2Max_ResultsInterface = String.Empty;
                _e4Min_ResultsInterface = String.Empty;
                _e4Max_ResultsInterface = String.Empty;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[LastInputOutputSavedData.cs] {private static void clearData()}", System.DateTime.Now);
            }

        }

        private static void getInputData() 
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.lastOnlineHeaderXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        #region GeneralData

                        //if (textReader.Name.Equals("tfbrIzvestaja_GeneralData"))
                        //{
                        //    GeneralData.tfbrIzvestaja.Text = textReader.ReadElementContentAsString();
                        //}

                        if (textReader.Name.Equals("tfOperator_GeneralData"))
                        {
                            _tfOperator_GeneralData = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfBrZbIzvestaja_GeneralData"))
                        {
                            _tfBrZbIzvestaja_GeneralData = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfBrUzorka_GeneralData"))
                        {
                            _tfBrUzorka_GeneralData = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfSarza_GeneralData"))
                        {
                            _tfSarza_GeneralData = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfRadniNalog_GeneralData"))
                        {
                            _tfRadniNalog_GeneralData = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfNaručilac_GeneralData"))
                        {
                            _tfNarucilac_GeneralData = textReader.ReadElementContentAsString();
                        }

                        #endregion


                        #region ConditionsOfTesting

                        if (textReader.Name.Equals("tfStandard_ConditionsOfTesting"))
                        {
                            _tfStandard_ConditionsOfTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfMetoda_ConditionsOfTesting"))
                        {
                            _tfMetoda_ConditionsOfTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfStandardZaN_ConditionsOfTesting"))
                        {
                            _tfStandardZaN_ConditionsOfTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfMasina_ConditionsOfTesting"))
                        {
                            _tfMasina_ConditionsOfTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfBegOpsegMasine_ConditionsOfTesting"))
                        {
                            _tfBegOpsegMasine_ConditionsOfTesting = textReader.ReadElementContentAsString();
                        }

                        //if (textReader.Name.Equals("tfEndOpsegMasine_ConditionsOfTesting"))
                        //{
                        //    ConditionsOfTesting.tfEndOpsegMasine.Text = textReader.ReadElementContentAsString();
                        //}

                        if (textReader.Name.Equals("tfTemperatura_ConditionsOfTesting"))
                        {
                            _tfTemperatura_ConditionsOfTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("rbtnYes_ConditionsOfTesting"))
                        {
                            string rbtnYesStr = textReader.ReadElementContentAsString();
                            _rbtnYes_ConditionsOfTesting = rbtnYesStr;
                            //if (rbtnYesStr.Equals("True"))
                            //{
                            //    ConditionsOfTesting.rbtnYes.IsChecked = true;
                            //}
                            //else
                            //{
                            //    ConditionsOfTesting.rbtnYes.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnNo_ConditionsOfTesting"))
                        {
                            string rbtnNoStr = textReader.ReadElementContentAsString();
                            _rbtnNo_ConditionsOfTesting = rbtnNoStr;
                            //if (rbtnNoStr.Equals("True"))
                            //{
                            //    ConditionsOfTesting.rbtnNo.IsChecked = true;
                            //}
                            //else
                            //{
                            //    ConditionsOfTesting.rbtnNo.IsChecked = false;
                            //}
                        }


                        #endregion

                        #region MaterialForTesting

                        if (textReader.Name.Equals("tfProizvodjac_MaterialForTesting"))
                        {
                            _tfProizvodjac_MaterialForTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfDobavljac_MaterialForTesting"))
                        {
                            _tfDobavljac_MaterialForTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfPolazniKvalitet_MaterialForTesting"))
                        {
                            _tfPolazniKvalitet_MaterialForTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfNazivnaDebljina_MaterialForTesting"))
                        {
                            _tfNazivnaDebljina_MaterialForTesting = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("rbtnValjani_MaterialForTesting"))
                        {
                            string rbtnValjaniStr = textReader.ReadElementContentAsString();
                            _rbtnValjani_MaterialForTesting = rbtnValjaniStr;
                            //if (rbtnValjaniStr.Equals("True"))
                            //{
                            //    MaterialForTesting.rbtnValjani.IsChecked = true;
                            //}
                            //else
                            //{
                            //    MaterialForTesting.rbtnValjani.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnVučeni_MaterialForTesting"))
                        {
                            string rbtnVučeniStr = textReader.ReadElementContentAsString();
                            _rbtnVuceni_MaterialForTesting = rbtnVučeniStr;
                            //if (rbtnVučeniStr.Equals("True"))
                            //{
                            //    MaterialForTesting.rbtnVučeni.IsChecked = true;
                            //}
                            //else
                            //{
                            //    MaterialForTesting.rbtnVučeni.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnKovani_MaterialForTesting"))
                        {
                            string rbtnKovaniStr = textReader.ReadElementContentAsString();
                            _rbtnKovani_MaterialForTesting = rbtnKovaniStr;
                            //if (rbtnKovaniStr.Equals("True"))
                            //{
                            //    MaterialForTesting.rbtnKovani.IsChecked = true;
                            //}
                            //else
                            //{
                            //    MaterialForTesting.rbtnKovani.IsChecked = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnLiveni_MaterialForTesting"))
                        {
                            string rbtnLiveniStr = textReader.ReadElementContentAsString();
                            _rbtnLiveni_MaterialForTesting = rbtnLiveniStr;
                            //if (rbtnLiveniStr.Equals("True"))
                            //{
                            //    MaterialForTesting.rbtnLiveni.IsChecked = true;
                            //}
                            //else
                            //{
                            //    MaterialForTesting.rbtnLiveni.IsChecked = false;
                            //}
                        }

                        #endregion

                        #region Epruveta

                        if (textReader.Name.Equals("rbtnEpvOblikObradjena"))
                        {
                            string rbtnEpvOblikObradjenaStr = textReader.ReadElementContentAsString();
                            _rbtnEpvOblikObradjena = rbtnEpvOblikObradjenaStr;
                            //if (rbtnEpvOblikObradjenaStr.Equals("True"))
                            //{
                            //    rbtnEpvOblikObradjena.IsChecked = true;
                            //}
                            //else
                            //{
                            //    rbtnEpvOblikObradjena.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvOblikNeobradjena"))
                        {
                            string rbtnEpvOblikNeobradjenaStr = textReader.ReadElementContentAsString();
                            _rbtnEpvOblikNeobradjena = rbtnEpvOblikNeobradjenaStr;
                            //if (rbtnEpvOblikNeobradjenaStr.Equals("True"))
                            //{
                            //    rbtnEpvOblikNeobradjena.IsChecked = true;
                            //}
                            //else
                            //{
                            //    rbtnEpvOblikNeobradjena.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvTipProporcionalna"))
                        {
                            string rbtnEpvTipProporcionalnaStr = textReader.ReadElementContentAsString();
                            _rbtnEpvTipProporcionalna = rbtnEpvTipProporcionalnaStr;
                            //if (rbtnEpvTipProporcionalnaStr.Equals("True"))
                            //{
                            //    rbtnEpvTipProporcionalna.IsChecked = true;
                            //}
                            //else
                            //{
                            //    rbtnEpvTipProporcionalna.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvTipNeproporcionalna"))
                        {
                            string rbtnEpvTipNeproporcionalnaStr = textReader.ReadElementContentAsString();
                            _rbtnEpvTipNeproporcionalna = rbtnEpvTipNeproporcionalnaStr;
                            //if (rbtnEpvTipNeproporcionalnaStr.Equals("True"))
                            //{
                            //    rbtnEpvTipNeproporcionalna.IsChecked = true;
                            //}
                            //else
                            //{
                            //    rbtnEpvTipNeproporcionalna.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvK1"))
                        {
                            string rbtnEpvK1Str = textReader.ReadElementContentAsString();
                            _rbtnEpvK1 = rbtnEpvK1Str;
                            //if (rbtnEpvK1Str.Equals("True"))
                            //{
                            //    rbtnEpvK1.IsChecked = true;
                            //}
                            //else
                            //{
                            //    rbtnEpvK1.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvK2"))
                        {
                            string rbtnEpvK2Str = textReader.ReadElementContentAsString();
                            _rbtnEpvK2 = rbtnEpvK2Str;
                            //if (rbtnEpvK2Str.Equals("True"))
                            //{
                            //    rbtnEpvK2.IsChecked = true;
                            //}
                            //else
                            //{
                            //    rbtnEpvK2.IsChecked = false;
                            //}
                        }

                        //if (textReader.Name.Equals("rbtnEpvUzduzni"))
                        //{
                        //    string rbtnEpvUzduzniStr = textReader.ReadElementContentAsString();
                        //    if (rbtnEpvUzduzniStr.Equals("True"))
                        //    {
                        //        rbtnEpvUzduzni.IsChecked = true;
                        //    }
                        //    else
                        //    {
                        //        rbtnEpvUzduzni.IsChecked = false;
                        //    }
                        //}

                        //if (textReader.Name.Equals("rbtnEpvPoprecni"))
                        //{
                        //    string rbtnEpvPoprecniStr = textReader.ReadElementContentAsString();
                        //    if (rbtnEpvPoprecniStr.Equals("True"))
                        //    {
                        //        rbtnEpvPoprecni.IsChecked = true;
                        //    }
                        //    else
                        //    {
                        //        rbtnEpvPoprecni.IsChecked = false;
                        //    }
                        //}


                        if (textReader.Name.Equals("rbtnEpvVrstaPravougaona"))
                        {
                            string rbtnEpvVrstaPravougaonaStr = textReader.ReadElementContentAsString();
                            _rbtnEpvVrstaPravougaona = rbtnEpvVrstaPravougaonaStr;
                            //if (rbtnEpvVrstaPravougaonaStr.Equals("True"))
                            //{
                            //    rbtnEpvVrstaPravougaona.IsChecked = true;
                            if (textReader.Name.Equals("a"))
                            {
                                _a0Pravougaona = textReader.ReadElementContentAsString();
                            }
                            if (textReader.Name.Equals("b"))
                            {
                                _b0Pravougaona = textReader.ReadElementContentAsString();
                            }

                            //}
                            //else
                            //{
                            //    rbtnEpvVrstaPravougaona.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvVrstaKruzni"))
                        {
                            string rbtnEpvVrstaKruzniStr = textReader.ReadElementContentAsString();
                            _rbtnEpvVrstaKruzna = rbtnEpvVrstaKruzniStr;
                            //if (rbtnEpvVrstaKruzniStr.Equals("True"))
                            //{
                            //    rbtnEpvVrstaKruzni.IsChecked = true;
                            if (textReader.Name.Equals("D"))
                            {
                                _D0Kruzna = textReader.ReadElementContentAsString();
                            }
                            //}
                            //else
                            //{
                            //    rbtnEpvVrstaKruzni.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("rbtnEpvVrstaCevasti"))
                        {
                            string rbtnEpvVrstaCevastiStr = textReader.ReadElementContentAsString();
                            _rbtnEpvVrstaCevasta = rbtnEpvVrstaCevastiStr;
                            //if (rbtnEpvVrstaCevastiStr.Equals("True"))
                            //{
                            //    rbtnEpvVrstaCevasti.IsChecked = true;
                            if (textReader.Name.Equals("D"))
                            {
                                _D0Cevasta = textReader.ReadElementContentAsString();
                            }
                            if (textReader.Name.Equals("a"))
                            {
                                _a0Cevasta = textReader.ReadElementContentAsString();
                            }
                            //}
                            //else
                            //{
                            //    rbtnEpvVrstaCevasti.IsChecked = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnEpvVrstaDeocev"))
                        {
                            string rbtnEpvVrstaDeocevStr = textReader.ReadElementContentAsString();
                            _rbtnEpvVrstaDeocev = rbtnEpvVrstaDeocevStr;
                            //if (rbtnEpvVrstaDeocevStr.Equals("True"))
                            //{
                            //    rbtnEpvVrstaDeocev.IsChecked = true;
                            if (textReader.Name.Equals("D"))
                            {
                                _D0Deocev = textReader.ReadElementContentAsString();
                            }
                            if (textReader.Name.Equals("a"))
                            {
                                _a0Deocev = textReader.ReadElementContentAsString();
                            }
                            if (textReader.Name.Equals("b"))
                            {
                                _b0Deocev = textReader.ReadElementContentAsString();
                            }
                            //}
                            //else
                            //{
                            //    rbtnEpvVrstaDeocev.IsChecked = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnEpvVrstaSestaugaona"))
                        {
                            string rbtnEpvVrstaSestaugaonaStr = textReader.ReadElementContentAsString();
                            _rbtnEpvVrstaSestaugaona = rbtnEpvVrstaSestaugaonaStr;
                            //if (rbtnEpvVrstaSestaugaonaStr.Equals("True"))
                            //{
                            //    rbtnEpvVrstaSestaugaona.IsChecked = true;
                            if (textReader.Name.Equals("d"))
                            {
                                _d0Sestaugaona = textReader.ReadElementContentAsString();
                            }
                            //}
                            //else
                            //{
                            //    rbtnEpvVrstaSestaugaona.IsChecked = false;
                            //}
                        }

                        if (textReader.Name.Equals("tfS0"))
                        {
                            _tfS0 = textReader.ReadElementContentAsString();
                        }


                        if (textReader.Name.Equals("tfL0"))
                        {
                            _tfL0 = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfLc"))
                        {
                            _tfLc = textReader.ReadElementContentAsString();
                        }
                        //if (textReader.Name.Equals("rtfNapomena"))
                        //{
                        //    string content = textReader.ReadElementContentAsString();

                        //    // Create a FlowDocument
                        //    FlowDocument mcFlowDoc = new FlowDocument();



                        //    // Create a paragraph with text
                        //    Paragraph para = new Paragraph();
                        //    para.Inlines.Add(new Run(content));

                        //    // Add the paragraph to blocks of paragraph
                        //    mcFlowDoc.Blocks.Add(para);

                        //    // Set contents
                        //    rtfNapomena.Document = mcFlowDoc;


                        //}

                        #endregion


                        #region PositionOfTube


                        if (textReader.Name.Equals("tfCustomPravacValjanja_PositionOfTube"))
                        {
                            _tfCustomPravacValjanja_PositionOfTube = textReader.ReadElementContentAsString();
                        }


                        if (textReader.Name.Equals("tfCustomSirinaTrake_PositionOfTube"))
                        {
                            _tfCustomSirinaTrake_PositionOfTube = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfCustomDuzinaTrake_PositionOfTube"))
                        {
                            _tfCustomDuzinaTrake_PositionOfTube = textReader.ReadElementContentAsString();
                        }

                        #endregion


                        #region RemarkOfTesting

                        if (textReader.Name.Equals("rtfNapomena_RemarkOfTesting"))
                        {
                            string content = textReader.ReadElementContentAsString();

                            // create a flowdocument
                            FlowDocument mcflowdoc = new FlowDocument();



                            // create a paragraph with text
                            Paragraph para = new Paragraph();
                            para.Inlines.Add(new Run(content));

                            // add the paragraph to blocks of paragraph
                            mcflowdoc.Blocks.Add(para);

                            // set contents
                            _rtfNapomena_RemarkOfTesting = new TextRange(mcflowdoc.ContentStart, mcflowdoc.ContentEnd).Text;
                        }

                        #endregion


                    }//if (nType == XmlNodeType.Element)

                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[LastInputOutputSavedData.cs] {private static void getInputData()}", System.DateTime.Now);
            }
        }

        private static void getOutputData() 
        {
            try
            {
                //get saved options in online mode
                // Create an XML reader for this file.

                XmlTextReader textReader = new XmlTextReader(Constants.lastResultsInterfaceXml);

                // Read until end of file
                while (textReader.Read())
                {
                    XmlNodeType nType = textReader.NodeType;

                    // if node type is an element
                    if (nType == XmlNodeType.Element)
                    {

                        if (textReader.Name.Equals("tfLu"))
                        {
                            _tfLu_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        #region Rp02

                        if (textReader.Name.Equals("chbRp02"))
                        {
                            string chbRp02Str = textReader.ReadElementContentAsString();
                            _chbRp02_ResultsInterface = chbRp02Str;
                            //if (chbRp02Str.Equals("True"))
                            //{
                            //    chbRp02.IsChecked = true;
                            //    rbtnRp02.IsEnabled = true;

                            //}
                            //else
                            //{
                            //    chbRp02.IsChecked = false;
                            //    rbtnRp02.IsChecked = false;
                            //    rbtnRp02.IsEnabled = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnRp02"))
                        {
                            string rbtnRp02Str = textReader.ReadElementContentAsString();
                            _rbtnRp02_ResultsInterface = rbtnRp02Str;
                            //if (chbRp02.IsChecked == true)
                            //{
                            //    if (rbtnRp02Str.Equals("True"))
                            //    {
                            //        rbtnRp02.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnRp02.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfRp02"))
                        {
                            _tfRp02_ResultsInterface = textReader.ReadElementContentAsString();

                        }

                        #endregion


                        #region Rt05

                        if (textReader.Name.Equals("chbRt05"))
                        {
                            string chbRt05Str = textReader.ReadElementContentAsString();
                            _chbRt05_ResultsInterface = chbRt05Str;
                            //if (chbRt05Str.Equals("True"))
                            //{
                            //    chbRt05.IsChecked = true;
                            //    rbtnRt05.IsEnabled = true;

                            //}
                            //else
                            //{
                            //    chbRt05.IsChecked = false;
                            //    rbtnRt05.IsChecked = false;
                            //    rbtnRt05.IsEnabled = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnRt05"))
                        {
                            string rbtnRt05Str = textReader.ReadElementContentAsString();
                            _rbtnRt05_ResultsInterface = rbtnRt05Str;
                            //if (chbRt05.IsChecked == true)
                            //{
                            //    if (rbtnRt05Str.Equals("True"))
                            //    {
                            //        rbtnRt05.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnRt05.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfRt05"))
                        {
                            _tfRt05_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        #endregion

                        #region ReL

                        if (textReader.Name.Equals("chbReL"))
                        {
                            string chbReLStr = textReader.ReadElementContentAsString();
                            _chbReL_ResultsInterface = chbReLStr;
                            //if (chbReLStr.Equals("True"))
                            //{
                            //    chbReL.IsChecked = true;
                            //    rbtnReL.IsEnabled = true;

                            //}
                            //else
                            //{
                            //    chbReL.IsChecked = false;
                            //    rbtnReL.IsChecked = false;
                            //    rbtnReL.IsEnabled = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnReL"))
                        {
                            string rbtnReLStr = textReader.ReadElementContentAsString();
                            _rbtnReL_ResultsInterface = rbtnReLStr;
                            //if (chbReL.IsChecked == true)
                            //{
                            //    if (rbtnReLStr.Equals("True"))
                            //    {
                            //        rbtnReL.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnReL.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfReL"))
                        {
                            _tfReL_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        #endregion


                        #region ReH

                        if (textReader.Name.Equals("chbReH"))
                        {
                            string chbReHStr = textReader.ReadElementContentAsString();
                            _chbReH_ResultsInterface = chbReHStr;
                            //if (chbReHStr.Equals("True"))
                            //{
                            //    chbReH.IsChecked = true;
                            //    rbtnReH.IsEnabled = true;

                            //}
                            //else
                            //{
                            //    chbReH.IsChecked = false;
                            //    rbtnReH.IsChecked = false;
                            //    rbtnReH.IsEnabled = false;
                            //}
                        }


                        if (textReader.Name.Equals("rbtnReH"))
                        {
                            string rbtnReHStr = textReader.ReadElementContentAsString();
                            _rbtnReH_ResultsInterface = rbtnReHStr;
                            //if (chbReH.IsChecked == true)
                            //{
                            //    if (rbtnReHStr.Equals("True"))
                            //    {
                            //        rbtnReH.IsChecked = true;
                            //    }
                            //    else
                            //    {
                            //        rbtnReH.IsChecked = false;
                            //    }
                            //}
                        }

                        if (textReader.Name.Equals("tfReH"))
                        {
                            _tfReH_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        #endregion


                        if (textReader.Name.Equals("tfRm"))
                        {
                            _tfRm_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfF"))
                        {
                            _tfF_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfFm"))
                        {
                            _tfFm_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfAg"))
                        {
                            _tfAg_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfAgt"))
                        {
                            _tfAgt_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfRRm"))
                        {
                            _tfRRm_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfA"))
                        {
                            _tfA_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfAt"))
                        {
                            _tfAt_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfSu"))
                        {
                            _tfSu_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        //ovo se automatski postavlja menjanjem tekstualnog polja tfSu
                        if (textReader.Name.Equals("tfZ"))
                        {
                            _tfZ_ResultsInterface = textReader.ReadElementContentAsString();
                        }


                        if (textReader.Name.Equals("chbn"))
                        {
                            string chbnStr = textReader.ReadElementContentAsString();
                            _chbn_ResultsInterface = chbnStr;
                        }

                        if (textReader.Name.Equals("tfn"))
                        {
                            _tfn_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("chbRmax"))
                        {
                            string chbRmaxStr = textReader.ReadElementContentAsString();
                            _chbRmax_ResultsInterface = chbRmaxStr;
                        }

                        if (textReader.Name.Equals("tfRmaxWithPoint"))
                        {
                            _tfRmaxWithPoint_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("chbe2"))
                        {
                            string chbe2Str = textReader.ReadElementContentAsString();
                            _chbe2_ResultsInterface = chbe2Str;
                        }


                        if (textReader.Name.Equals("e2Min"))
                        {
                            _e2Min_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("e2Max"))
                        {
                            _e2Max_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("chbe4"))
                        {
                            string chbe4Str = textReader.ReadElementContentAsString();
                            _chbe4_ResultsInterface = chbe4Str;
                        }

                        if (textReader.Name.Equals("e4Min"))
                        {
                            _e4Min_ResultsInterface = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("e4Max"))
                        {
                            _e4Max_ResultsInterface = textReader.ReadElementContentAsString();
                        }


                    }//if (nType == XmlNodeType.Element)

                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[LastInputOutputSavedData.cs] {private static void getOutputData()}", System.DateTime.Now);
            }
        }

        public static void GetData() 
        {
            try
            {
                clearData();
                getInputData();
                getOutputData();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[LastInputOutputSavedData.cs] {public static void GetData()}", System.DateTime.Now);
            }
        }

        //public static void Find_au() 
        //{
        //    _au = "0";
        //}

        //public static void Find_bu()
        //{
        //    _bu = "0";
        //}

        //public static void Find_du()
        //{
        //    _du = "0";
        //}

        //public static void Find_Du()
        //{
        //    _Du = "0";
        //}

        #endregion
    }
}
