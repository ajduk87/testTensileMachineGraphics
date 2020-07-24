using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using testTensileMachineGraphics.Options;
using testTensileMachineGraphics.Reports;
using System.Xml;
using testTensileMachineGraphics.OnlineModeFolder.Input_Data;

namespace testTensileMachineGraphics.OnlineModeFolder
{
    /// <summary>
    /// Interaction logic for OnlineFileHeader.xaml
    /// </summary>
    public partial class OnlineFileHeader : Window
    {

        public ForceRanges ForceRanges;

        #region publicInformationAboutabdSoLoAndLc

        public string strtfAGlobal = string.Empty;
        public string strtfBGlobal = string.Empty;
        public string strtfDGlobal = string.Empty;
        public string strs0 = string.Empty;
        public string strl0 = string.Empty;
        public string strlc = string.Empty;


        #endregion

        private bool rbtnEpvVrstaPravougaonaChecked = false;

        public void DisableInputs()
        {

            try
            {
                #region GeneralData

                generalData.tfOperator.IsReadOnly = true;
                generalData.tfBrZbIzvestaja.IsReadOnly = true;
                generalData.tfBrUzorka.IsReadOnly = true;
                generalData.tfBrUzorkaNumberOfSample.IsReadOnly = true;
                generalData.tfSarza.IsReadOnly = true;
                generalData.tfRadniNalog.IsReadOnly = true;
                generalData.tfNaručilac.IsReadOnly = true;

                #endregion

                #region ConditionsOfTesting

                conditionsOfTesting.tfStandard.IsReadOnly = true;
                conditionsOfTesting.tfMetoda.IsReadOnly = true;
                conditionsOfTesting.tfStandardZaN.IsReadOnly = true;
                conditionsOfTesting.tfTemperatura.IsReadOnly = true;
                conditionsOfTesting.tfMasina.IsReadOnly = true;
                conditionsOfTesting.tfBegOpsegMasine.IsReadOnly = true;
                conditionsOfTesting.rbtnYes.IsEnabled = false;
                conditionsOfTesting.rbtnNo.IsEnabled = false;

                #endregion

                #region MaterialForTesting

                materialForTesting.tfProizvodjac.IsReadOnly = true;
                materialForTesting.tfDobavljac.IsReadOnly = true;
                materialForTesting.tfPolazniKvalitet.IsReadOnly = true;
                materialForTesting.tfNazivnaDebljina.IsReadOnly = true;
                materialForTesting.rbtnValjani.IsEnabled = false;
                materialForTesting.rbtnVučeni.IsEnabled = false;
                materialForTesting.rbtnKovani.IsEnabled = false;
                materialForTesting.rbtnLiveni.IsEnabled = false;

                #endregion

                #region PositionOfTube

                positionOfTube.tfCustomPravacValjanja.IsReadOnly = true;
                positionOfTube.tfCustomSirinaTrake.IsReadOnly = true;
                positionOfTube.tfCustomDuzinaTrake.IsReadOnly = true;

                #endregion

                #region RemarkOfTesting

                //napomena moze da se unosi i pre i posle ispitivanja tj nikad ga ne postavlja na readonly
                //remarkOfTesting.rtfNapomena.IsReadOnly = true;

                #endregion

                #region OnlineFileHeader

                rbtnEpvOblikObradjena.IsEnabled = false;
                rbtnEpvOblikNeobradjena.IsEnabled = false;
                rbtnEpvTipProporcionalna.IsEnabled = false;
                rbtnEpvTipNeproporcionalna.IsEnabled = false;
                rbtnEpvK1.IsEnabled = false;
                rbtnEpvK2.IsEnabled = false;
                rbtnEpvVrstaPravougaona.IsEnabled = false;
                rbtnEpvVrstaKruzni.IsEnabled = false;
                rbtnEpvVrstaCevasti.IsEnabled = false;
                rbtnEpvVrstaDeocev.IsEnabled = false;
                rbtnEpvVrstaSestaugaona.IsEnabled = false;

                if (tfAGlobal != null)
                {
                    tfAGlobal.IsReadOnly = true;
                }
                if (tfBGlobal != null)
                {
                    tfBGlobal.IsReadOnly = true;
                }
                if (tfDGlobal != null)
                {
                    tfDGlobal.IsReadOnly = true;
                }

                tfS0.IsReadOnly = true;
                tfL0.IsReadOnly = true;
                tfLc.IsReadOnly = true;


                #endregion
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {public void DisableInputs()}", System.DateTime.Now);
            }

        }

        public void EnableInputs()
        {
            try
            {
                #region GeneralData

                generalData.tfOperator.IsReadOnly = false;
                generalData.tfBrZbIzvestaja.IsReadOnly = false;
                generalData.tfBrUzorka.IsReadOnly = false;
                generalData.tfBrUzorkaNumberOfSample.IsReadOnly = false;
                generalData.tfSarza.IsReadOnly = false;
                generalData.tfRadniNalog.IsReadOnly = false;
                generalData.tfNaručilac.IsReadOnly = false;

                #endregion

                #region ConditionsOfTesting

                conditionsOfTesting.tfStandard.IsReadOnly = false;
                conditionsOfTesting.tfMetoda.IsReadOnly = false;
                conditionsOfTesting.tfStandardZaN.IsReadOnly = false;
                conditionsOfTesting.tfTemperatura.IsReadOnly = false;
                conditionsOfTesting.tfMasina.IsReadOnly = false;
                conditionsOfTesting.tfBegOpsegMasine.IsReadOnly = false;
                conditionsOfTesting.rbtnYes.IsEnabled = true;
                conditionsOfTesting.rbtnNo.IsEnabled = true;

                #endregion

                #region MaterialForTesting

                materialForTesting.tfProizvodjac.IsReadOnly = false;
                materialForTesting.tfDobavljac.IsReadOnly = false;
                materialForTesting.tfPolazniKvalitet.IsReadOnly = false;
                materialForTesting.tfNazivnaDebljina.IsReadOnly = false;
                materialForTesting.rbtnValjani.IsEnabled = true;
                materialForTesting.rbtnVučeni.IsEnabled = true;
                materialForTesting.rbtnKovani.IsEnabled = true;
                materialForTesting.rbtnLiveni.IsEnabled = true;

                #endregion

                #region PositionOfTube

                positionOfTube.tfCustomPravacValjanja.IsReadOnly = false;
                positionOfTube.tfCustomSirinaTrake.IsReadOnly = false;
                positionOfTube.tfCustomDuzinaTrake.IsReadOnly = false;

                #endregion

                #region RemarkOfTesting

                //napomena moze da se unosi i pre i posle ispitivanja tj nikad ga ne postavlja na readonly
                //remarkOfTesting.rtfNapomena.IsReadOnly = false;

                #endregion

                #region OnlineFileHeader

                rbtnEpvOblikObradjena.IsEnabled = true;
                rbtnEpvOblikNeobradjena.IsEnabled = true;
                rbtnEpvTipProporcionalna.IsEnabled = true;
                rbtnEpvTipNeproporcionalna.IsEnabled = true;
                rbtnEpvK1.IsEnabled = true;
                rbtnEpvK2.IsEnabled = true;
                rbtnEpvVrstaPravougaona.IsEnabled = true;
                rbtnEpvVrstaKruzni.IsEnabled = true;
                rbtnEpvVrstaCevasti.IsEnabled = true;
                rbtnEpvVrstaDeocev.IsEnabled = true;
                rbtnEpvVrstaSestaugaona.IsEnabled = true;

                if (tfAGlobal != null)
                {
                    tfAGlobal.IsReadOnly = false;
                }
                if (tfBGlobal != null)
                {
                    tfBGlobal.IsReadOnly = false;
                }
                if (tfDGlobal != null)
                {
                    tfDGlobal.IsReadOnly = false;
                }

                //tfS0.IsReadOnly = false;
                tfL0.IsReadOnly = false;
                tfLc.IsReadOnly = false;

                #endregion
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {public void EnableInputs()}", System.DateTime.Now);
            }
        }

        #region windows

        private GeneralData generalData;
        public GeneralData GeneralData
        {
            get { return generalData; }
            set 
            {
                if (value != null)
                {
                    generalData = value;
                }
            }
        }

        private ConditionsOfTesting conditionsOfTesting;
        public ConditionsOfTesting ConditionsOfTesting
        {
            get { return conditionsOfTesting; }
            set
            {
                if (value != null)
                {
                    conditionsOfTesting = value;
                }
            }
        }

        private MaterialForTesting materialForTesting;
        public MaterialForTesting MaterialForTesting
        {
            get { return materialForTesting; }
            set
            {
                if (value != null)
                {
                    materialForTesting = value;
                }
            }
        }

        private PositionOfTube positionOfTube;
        public PositionOfTube PositionOfTube
        {
            get { return positionOfTube; }
            set
            {
                if (value != null)
                {
                    positionOfTube = value;
                }
            }
        }

        private RemarkOfTesting remarkOfTesting;
        public RemarkOfTesting RemarkOfTesting
        {
            get { return remarkOfTesting; }
            set
            {
                if (value != null)
                {
                    remarkOfTesting = value;
                }
            }
        }


        #endregion


        public static bool isCreatedOnlineHeader = false;

        private OnlineMode onMode = null;

        public OnlineMode OnlineModeInstance
        {
            get { return onMode; }

            set
            {
                if (value != null)
                {
                    onMode = value;
                }
            }
        }

        public TextBox tfAGlobal = null;
        public TextBox tfBGlobal = null;
        public TextBox tfDGlobal = null;
        public TextBox tfMGlobal = null;
        public TextBox tfPGlobal = null;
        public TextBox tfLTGlobal = null;


        public int HeaderSize = 0;

       

        private void setToNullGlobalTextboxes() 
        {
            try
            {
                tfAGlobal = null;
                tfBGlobal = null;
                tfDGlobal = null;
                tfMGlobal = null;
                tfPGlobal = null;
                tfLTGlobal = null;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void setToNullGlobalTextboxes()}", System.DateTime.Now);
            }
        }

        private void initialCheckRadiobuttons()
        {
            try
            {
                //rbtnVuceni.IsChecked = true;
                rbtnEpvOblikObradjena.IsChecked = true;
                rbtnEpvTipProporcionalna.IsChecked = true;
                rbtnEpvK1.IsChecked = true;
                //rbtnEpvUzduzni.IsChecked = true;
                rbtnEpvVrstaPravougaona.IsChecked = true;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void initialCheckRadiobuttons()}", System.DateTime.Now);
            }
        }

        private double xconst = 1250 + 300 + 85;
        private double yconst = 60;

        private double xconst_InPrintScreenMode = 1068;
        private double yconst_InPrintScreenMode = 388;

        private void setTextBoxesForOnlineHeader() 
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
                            GeneralData.tfOperator.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfBrZbIzvestaja_GeneralData"))
                        {
                            GeneralData.tfBrZbIzvestaja.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfBrUzorka_GeneralData"))
                        {
                            //GeneralData.tfBrUzorka.Text = textReader.ReadElementContentAsString();
                            string sumtfBrUzorka = textReader.ReadElementContentAsString();
                            List<string> brUzorka = sumtfBrUzorka.Split('/').ToList();
                            if (brUzorka.Count == 2)
                            {
                                GeneralData.tfBrUzorka.Text = brUzorka.ElementAt(0);
                                GeneralData.tfBrUzorkaNumberOfSample.Text = brUzorka.ElementAt(1);
                            }
                        }

                        if (textReader.Name.Equals("tfSarza_GeneralData"))
                        {
                            GeneralData.tfSarza.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfRadniNalog_GeneralData"))
                        {
                            GeneralData.tfRadniNalog.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfNaručilac_GeneralData"))
                        {
                            GeneralData.tfNaručilac.Text = textReader.ReadElementContentAsString();
                        }

                        #endregion


                        #region ConditionsOfTesting

                        if (textReader.Name.Equals("tfStandard_ConditionsOfTesting"))
                        {
                            ConditionsOfTesting.tfStandard.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfMetoda_ConditionsOfTesting"))
                        {
                            ConditionsOfTesting.tfMetoda.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfStandardZaN_ConditionsOfTesting"))
                        {
                            ConditionsOfTesting.tfStandardZaN.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfMasina_ConditionsOfTesting"))
                        {
                            ConditionsOfTesting.tfMasina.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfBegOpsegMasine_ConditionsOfTesting"))
                        {
                            ConditionsOfTesting.tfBegOpsegMasine.Text = textReader.ReadElementContentAsString();
                        }

                        //if (textReader.Name.Equals("tfEndOpsegMasine_ConditionsOfTesting"))
                        //{
                        //    ConditionsOfTesting.tfEndOpsegMasine.Text = textReader.ReadElementContentAsString();
                        //}

                        if (textReader.Name.Equals("tfTemperatura_ConditionsOfTesting"))
                        {
                            ConditionsOfTesting.tfTemperatura.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("rbtnYes_ConditionsOfTesting"))
                        {
                            string rbtnYesStr = textReader.ReadElementContentAsString();
                            if (rbtnYesStr.Equals("True"))
                            {
                                ConditionsOfTesting.rbtnYes.IsChecked = true;
                            }
                            else
                            {
                                ConditionsOfTesting.rbtnYes.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnNo_ConditionsOfTesting"))
                        {
                            string rbtnNoStr = textReader.ReadElementContentAsString();
                            if (rbtnNoStr.Equals("True"))
                            {
                                ConditionsOfTesting.rbtnNo.IsChecked = true;
                            }
                            else
                            {
                                ConditionsOfTesting.rbtnNo.IsChecked = false;
                            }
                        }


                        #endregion

                        #region MaterialForTesting

                        if (textReader.Name.Equals("tfProizvodjac_MaterialForTesting"))
                        {
                            MaterialForTesting.tfProizvodjac.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfDobavljac_MaterialForTesting"))
                        {
                            MaterialForTesting.tfDobavljac.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfPolazniKvalitet_MaterialForTesting"))
                        {
                            MaterialForTesting.tfPolazniKvalitet.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfNazivnaDebljina_MaterialForTesting"))
                        {
                            MaterialForTesting.tfNazivnaDebljina.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("rbtnValjani_MaterialForTesting"))
                        {
                            string rbtnValjaniStr = textReader.ReadElementContentAsString();
                            if (rbtnValjaniStr.Equals("True"))
                            {
                                MaterialForTesting.rbtnValjani.IsChecked = true;
                            }
                            else
                            {
                                MaterialForTesting.rbtnValjani.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnVučeni_MaterialForTesting"))
                        {
                            string rbtnVučeniStr = textReader.ReadElementContentAsString();
                            if (rbtnVučeniStr.Equals("True"))
                            {
                                MaterialForTesting.rbtnVučeni.IsChecked = true;
                            }
                            else
                            {
                                MaterialForTesting.rbtnVučeni.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnKovani_MaterialForTesting"))
                        {
                            string rbtnKovaniStr = textReader.ReadElementContentAsString();
                            if (rbtnKovaniStr.Equals("True"))
                            {
                                MaterialForTesting.rbtnKovani.IsChecked = true;
                            }
                            else
                            {
                                MaterialForTesting.rbtnKovani.IsChecked = false;
                            }
                        }


                        if (textReader.Name.Equals("rbtnLiveni_MaterialForTesting"))
                        {
                            string rbtnLiveniStr = textReader.ReadElementContentAsString();
                            if (rbtnLiveniStr.Equals("True"))
                            {
                                MaterialForTesting.rbtnLiveni.IsChecked = true;
                            }
                            else
                            {
                                MaterialForTesting.rbtnLiveni.IsChecked = false;
                            }
                        }

                        #endregion

                        #region Epruveta

                        if (textReader.Name.Equals("rbtnEpvOblikObradjena"))
                        {
                            string rbtnEpvOblikObradjenaStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvOblikObradjenaStr.Equals("True"))
                            {
                                rbtnEpvOblikObradjena.IsChecked = true;
                            }
                            else
                            {
                                rbtnEpvOblikObradjena.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnEpvOblikNeobradjena"))
                        {
                            string rbtnEpvOblikNeobradjenaStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvOblikNeobradjenaStr.Equals("True"))
                            {
                                rbtnEpvOblikNeobradjena.IsChecked = true;
                            }
                            else
                            {
                                rbtnEpvOblikNeobradjena.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnEpvTipProporcionalna"))
                        {
                            string rbtnEpvTipProporcionalnaStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvTipProporcionalnaStr.Equals("True"))
                            {
                                rbtnEpvTipProporcionalna.IsChecked = true;
                            }
                            else
                            {
                                rbtnEpvTipProporcionalna.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnEpvTipNeproporcionalna"))
                        {
                            string rbtnEpvTipNeproporcionalnaStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvTipNeproporcionalnaStr.Equals("True"))
                            {
                                rbtnEpvTipNeproporcionalna.IsChecked = true;
                            }
                            else
                            {
                                rbtnEpvTipNeproporcionalna.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnEpvK1"))
                        {
                            string rbtnEpvK1Str = textReader.ReadElementContentAsString();
                            if (rbtnEpvK1Str.Equals("True"))
                            {
                                rbtnEpvK1.IsChecked = true;
                            }
                            else
                            {
                                rbtnEpvK1.IsChecked = false;
                            }
                        }

                        if (textReader.Name.Equals("rbtnEpvK2"))
                        {
                            string rbtnEpvK2Str = textReader.ReadElementContentAsString();
                            if (rbtnEpvK2Str.Equals("True"))
                            {
                                rbtnEpvK2.IsChecked = true;
                            }
                            else
                            {
                                rbtnEpvK2.IsChecked = false;
                            }
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
                            if (rbtnEpvVrstaPravougaonaStr.Equals("True"))
                            {
                                rbtnEpvVrstaPravougaona.IsChecked = true;
                                //if(textReader.Name.Equals("a"))
                                //{
                                //    tfAGlobal.Text = textReader.ReadElementContentAsString(); 
                                //}
                                //if (textReader.Name.Equals("b"))
                                //{
                                //    tfBGlobal.Text = textReader.ReadElementContentAsString();
                                //}

                            }
                            else
                            {
                                rbtnEpvVrstaPravougaona.IsChecked = false;
                            }
                        }
                        if (textReader.Name.Equals("a"))
                        {
                            if (tfAGlobal != null)
                            {
                                tfAGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }
                        if (textReader.Name.Equals("b"))
                        {
                            if (tfBGlobal != null)
                            {
                                tfBGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }


                        if (textReader.Name.Equals("rbtnEpvVrstaKruzni"))
                        {
                            string rbtnEpvVrstaKruzniStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvVrstaKruzniStr.Equals("True"))
                            {
                                rbtnEpvVrstaKruzni.IsChecked = true;
                                //if (textReader.Name.Equals("D"))
                                //{
                                //    tfDGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                            }
                            else
                            {
                                rbtnEpvVrstaKruzni.IsChecked = false;
                            }
                        }
                        if (textReader.Name.Equals("D"))
                        {
                            if (tfDGlobal != null)
                            {
                                tfDGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }

                        if (textReader.Name.Equals("rbtnEpvVrstaCevasti"))
                        {
                            string rbtnEpvVrstaCevastiStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvVrstaCevastiStr.Equals("True"))
                            {
                                rbtnEpvVrstaCevasti.IsChecked = true;
                                //if (textReader.Name.Equals("D"))
                                //{
                                //    tfDGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                                //if (textReader.Name.Equals("a"))
                                //{
                                //    tfAGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                            }
                            else
                            {
                                rbtnEpvVrstaCevasti.IsChecked = false;
                            }
                        }
                        if (textReader.Name.Equals("D"))
                        {
                            if (tfDGlobal != null)
                            {
                                tfDGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }
                        if (textReader.Name.Equals("a"))
                        {
                            if (tfAGlobal != null)
                            {
                                tfAGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }


                        if (textReader.Name.Equals("rbtnEpvVrstaDeocev"))
                        {
                            string rbtnEpvVrstaDeocevStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvVrstaDeocevStr.Equals("True"))
                            {
                                rbtnEpvVrstaDeocev.IsChecked = true;
                                //if (textReader.Name.Equals("D"))
                                //{
                                //    tfDGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                                //if (textReader.Name.Equals("a"))
                                //{
                                //    tfAGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                                //if (textReader.Name.Equals("b"))
                                //{
                                //    tfBGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                            }
                            else
                            {
                                rbtnEpvVrstaDeocev.IsChecked = false;
                            }
                        }
                        if (textReader.Name.Equals("D"))
                        {
                            if (tfDGlobal != null)
                            {
                                tfDGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }
                        if (textReader.Name.Equals("a"))
                        {
                            if (tfAGlobal != null)
                            {
                                tfAGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }
                        if (textReader.Name.Equals("b"))
                        {
                            if (tfBGlobal != null)
                            {
                                tfBGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }


                        if (textReader.Name.Equals("rbtnEpvVrstaSestaugaona"))
                        {
                            string rbtnEpvVrstaSestaugaonaStr = textReader.ReadElementContentAsString();
                            if (rbtnEpvVrstaSestaugaonaStr.Equals("True"))
                            {
                                rbtnEpvVrstaSestaugaona.IsChecked = true;
                                //if (textReader.Name.Equals("d"))
                                //{
                                //    tfDGlobal.Text = textReader.ReadElementContentAsString();
                                //}
                            }
                            else
                            {
                                rbtnEpvVrstaSestaugaona.IsChecked = false;
                            }
                        }
                        if (textReader.Name.Equals("d"))
                        {
                            if (tfDGlobal != null)
                            {
                                tfDGlobal.Text = textReader.ReadElementContentAsString();
                            }
                        }

                        if (textReader.Name.Equals("tfL0"))
                        {
                            tfL0.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfLc"))
                        {
                            tfLc.Text = textReader.ReadElementContentAsString();
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
                            PositionOfTube.tfCustomPravacValjanja.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfCustomSirinaTrake_PositionOfTube"))
                        {
                            PositionOfTube.tfCustomSirinaTrake.Text = textReader.ReadElementContentAsString();
                        }

                        if (textReader.Name.Equals("tfCustomDuzinaTrake_PositionOfTube"))
                        {
                            PositionOfTube.tfCustomDuzinaTrake.Text = textReader.ReadElementContentAsString();
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
                            RemarkOfTesting.rtfNapomena.Document = mcflowdoc;


                        }

                        #endregion


                    }//if (nType == XmlNodeType.Element)

                }//end of while loop


                textReader.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void setTextBoxesForOnlineHeader()}", System.DateTime.Now);
            }
        }


        public void makeInputDataWindows() 
        {
            try
            {
                generalData = new GeneralData(this);
                ForceRanges fr = new ForceRanges(onMode);
                this.ForceRanges = fr;
                conditionsOfTesting = new ConditionsOfTesting(this, ForceRanges);
                materialForTesting = new MaterialForTesting(this);
                positionOfTube = new PositionOfTube(this);
                remarkOfTesting = new RemarkOfTesting(this);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {public void makeInputDataWindows()}", System.DateTime.Now);
            }
        }

        public void showInputDataWindows()
        {
            try
            {
                if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == false && onMode.Plotting.Printscreen.cmbInputWindow.SelectedIndex == 0)
                {
                    generalData.Show();
                }
                else if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == false && onMode.Plotting.Printscreen.cmbInputWindow.SelectedIndex == 1)
                {
                    conditionsOfTesting.Show();
                }
                else if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == false && onMode.Plotting.Printscreen.cmbInputWindow.SelectedIndex == 2)
                {
                    materialForTesting.Show();
                }
                else if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == false && onMode.Plotting.Printscreen.cmbInputWindow.SelectedIndex == 3)
                {
                    positionOfTube.Show();
                }
                else if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == false && onMode.Plotting.Printscreen.cmbInputWindow.SelectedIndex == 5)
                {
                    remarkOfTesting.Show();
                }

                if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == true)
                {
                    generalData.Show();
                    conditionsOfTesting.Show();
                    materialForTesting.Show();
                    positionOfTube.Show();
                    remarkOfTesting.Show();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {public void showInputDataWindows()}", System.DateTime.Now);
            }
        }

        public OnlineFileHeader(OnlineMode onlineMode)
        {
            try
            {
                InitializeComponent();
                //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                initialCheckRadiobuttons();
                onMode = onlineMode;

                if (onMode.Plotting.Printscreen.IsPrintScreenEmpty == true)
                {
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                    this.Left = xconst;
                    this.Top = yconst;
                }
                else
                {
                    //WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                    this.WindowStartupLocation = WindowStartupLocation.Manual;
                    this.Left = xconst_InPrintScreenMode;
                    this.Top = yconst_InPrintScreenMode;
                }

                makeInputDataWindows();
                setTextBoxesForOnlineHeader();
                //showInputDataWindows();
                var mm2 = "mm\xB2";
                lblS0mm2.Text = mm2;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {public OnlineFileHeader(OnlineMode onlineMode)}", System.DateTime.Now);
            }
        }

        #region textchangedEvents

        private void tfAGlobal_TextChanged(object sender, TextChangedEventArgs e) 
        {
            try
            {
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    //LastInputOutputSavedData.a0Pravougaona = tfAGlobal.Text;
                    double s0Result = -1;
                    double aglobal = -1;
                    double bglobal = -1;

                    string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfAGlobal, out aglobal);
                    if (isN == false)
                    {
                        if (tfAGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar a mora biti unet kao broj !");
                        }
                    }

                    if (tfBGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                        bool isNN = Double.TryParse(strtfBGlobal, out bglobal);
                        if (isNN == false)
                        {
                            if (tfBGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar b mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = aglobal * bglobal;
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }




                if (rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    //LastInputOutputSavedData.a0Cevasta = tfAGlobal.Text;
                    double s0Result = -1;
                    double aglobal = -1;
                    double dglobal = -1;


                    string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfAGlobal, out aglobal);
                    if (isN == false)
                    {
                        if (tfAGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar a mora biti unet kao broj !");
                        }
                    }

                    if (tfDGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                        bool isNN = Double.TryParse(strtfDGlobal, out dglobal);
                        if (isNN == false)
                        {
                            if (tfDGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar D mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = Math.PI / 4 * (Math.Pow(dglobal, 2) - Math.Pow((dglobal - 2 * aglobal), 2));
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }



                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    //LastInputOutputSavedData.a0Deocev = tfAGlobal.Text;
                    double s0Result = -1;
                    double aglobal = -1;
                    double bglobal = -1;
                    double dglobal = -1;


                    string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfAGlobal, out aglobal);
                    if (isN == false)
                    {
                        if (tfAGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar a mora biti unet kao broj !");
                        }
                    }

                    string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                    bool isNN = Double.TryParse(strtfBGlobal, out bglobal);
                    if (isNN == false)
                    {
                        if (tfBGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar b mora biti unet kao broj !");
                        }
                    }

                    if (tfDGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                        bool isNNN = Double.TryParse(strtfDGlobal, out dglobal);
                        if (isNNN == false)
                        {
                            if (tfDGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar D mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = aglobal * bglobal * (1 + (Math.Pow(bglobal, 2) / 6 * dglobal * (dglobal - 2 * aglobal)));
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }

            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfAGlobal_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfAGlobal_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {

                    tfAGlobal.SelectAll();
                    tfAGlobal.Focus();

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfAGlobal_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfAGlobal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    if (e.Key == Key.Tab)
                    {
                        if (tfBGlobal != null)
                        {
                            tfBGlobal.SelectAll();
                            tfBGlobal.Focus();
                        }
                    }
                }

                if (rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    if (e.Key == Key.Tab)
                    {
                        tfL0.SelectAll();
                        tfL0.Focus();
                    }
                }

                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    if (e.Key == Key.Tab)
                    {
                        if (tfBGlobal != null)
                        {
                            tfBGlobal.SelectAll();
                            tfBGlobal.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfAGlobal_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {

            try
            {
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    double s0Result = -1;
                    double bglobal = -1;
                    double aglobal = -1;

                    string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfBGlobal, out bglobal);
                    if (isN == false)
                    {
                        if (tfBGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar b mora biti unet kao broj !");
                        }
                    }

                    if (tfAGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                        bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                        if (isNN == false)
                        {
                            if (tfAGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar a mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = bglobal * aglobal;
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }


                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    double s0Result = -1;
                    double bglobal = -1;
                    double aglobal = -1;
                    double dglobal = -1;


                    string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfBGlobal, out bglobal);
                    if (isN == false)
                    {
                        if (tfBGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar b mora biti unet kao broj !");
                        }
                    }

                    string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                    if (isNN == false)
                    {
                        if (tfAGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar a mora biti unet kao broj !");
                        }
                    }

                    if (tfDGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                        bool isNNN = Double.TryParse(strtfDGlobal, out dglobal);
                        if (isNNN == false)
                        {
                            if (tfDGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar D mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = aglobal * bglobal * (1 + (Math.Pow(bglobal, 2) / 6 * dglobal * (dglobal - 2 * aglobal)));
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfBGlobal_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfBGlobal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    if (e.Key == Key.Tab)
                    {

                        tfL0.SelectAll();
                        tfL0.Focus();

                    }
                }


                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    if (e.Key == Key.Tab)
                    {
                        tfL0.SelectAll();
                        tfL0.Focus();

                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfBGlobal_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDGlobal_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (rbtnEpvVrstaKruzni.IsChecked == true)
                {
                    if (tfDGlobal != null)
                    {
                        tfDGlobal.SelectAll();
                        tfDGlobal.Focus();
                    }
                }

                if (rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    if (tfDGlobal != null)
                    {
                        tfDGlobal.SelectAll();
                        tfDGlobal.Focus();
                    }
                }

                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    if (tfDGlobal != null)
                    {
                        tfDGlobal.SelectAll();
                        tfDGlobal.Focus();
                    }
                }

                if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                {
                    if (tfDGlobal != null)
                    {
                        tfDGlobal.SelectAll();
                        tfDGlobal.Focus();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfDGlobal_MouseEnter(object sender, MouseEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDGlobal_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    if (rbtnEpvVrstaKruzni.IsChecked == true)
                    {
                        tfL0.SelectAll();
                        tfL0.Focus();
                    }

                    if (rbtnEpvVrstaCevasti.IsChecked == true)
                    {
                        if (tfAGlobal != null)
                        {
                            tfAGlobal.SelectAll();
                            tfAGlobal.Focus();
                        }
                    }

                    if (rbtnEpvVrstaDeocev.IsChecked == true)
                    {
                        if (tfAGlobal != null)
                        {
                            tfAGlobal.SelectAll();
                            tfAGlobal.Focus();
                        }
                    }

                    if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                    {
                        tfL0.SelectAll();
                        tfL0.Focus();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfDGlobal_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        private void tfDGlobal_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {

                if (rbtnEpvVrstaKruzni.IsChecked == true)
                {
                    double s0Result = -1;
                    double dglobal = -1;

                    string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        if (tfDGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar D mora biti unet kao broj !");
                            tfS0.Text = String.Empty;
                        }
                        else
                        {
                            tfS0.Text = String.Empty;
                        }
                    }
                    else
                    {
                        s0Result = dglobal * dglobal * Math.PI / 4;
                        s0Result = Math.Round(s0Result, 3);
                        tfS0.Text = s0Result.ToString();
                    }
                }


                if (rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    double s0Result = -1;
                    double dglobal = -1;
                    double aglobal = -1;


                    string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        if (tfDGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar D mora biti unet kao broj !");
                        }
                    }

                    if (tfAGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                        bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                        if (isNN == false)
                        {
                            if (tfAGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar a mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = Math.PI / 4 * (Math.Pow(dglobal, 2) - Math.Pow((dglobal - 2 * aglobal), 2));
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }


                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    double s0Result = -1;
                    double dglobal = -1;
                    double aglobal = -1;
                    double bglobal = -1;


                    string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        if (tfDGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar D mora biti unet kao broj !");
                        }
                    }

                    string strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                    if (isNN == false)
                    {
                        if (tfAGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar a mora biti unet kao broj !");
                        }
                    }

                    if (tfBGlobal.Text.Equals(String.Empty) == true)
                    {
                        tfS0.Text = String.Empty;
                    }
                    else
                    {
                        string strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                        bool isNNN = Double.TryParse(strtfBGlobal, out bglobal);
                        if (isNNN == false)
                        {
                            if (tfBGlobal.Text.Equals(String.Empty) == false)
                            {
                                MessageBox.Show("Parametar b mora biti unet kao broj !");
                                tfS0.Text = String.Empty;
                            }
                        }
                        else
                        {
                            s0Result = aglobal * bglobal * (1 + (Math.Pow(bglobal, 2) / 6 * dglobal * (dglobal - 2 * aglobal)));
                            s0Result = Math.Round(s0Result, 3);
                            tfS0.Text = s0Result.ToString();
                        }
                    }
                }

                if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                {
                    double s0Result = -1;
                    double dglobal = -1;

                    string strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    if (isN == false)
                    {
                        if (tfDGlobal.Text.Equals(String.Empty) == false)
                        {
                            MessageBox.Show("Parametar d mora biti unet kao broj !");
                            tfS0.Text = String.Empty;
                        }
                        else
                        {
                            tfS0.Text = String.Empty;
                        }
                    }
                    else
                    {
                        s0Result = (3 * Math.Pow(dglobal, 2) * Math.Sqrt(3)) / 8;
                        s0Result = Math.Round(s0Result, 3);
                        tfS0.Text = s0Result.ToString();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfDGlobal_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }

        }

        #endregion


        private void rbtnEpvVrstaPravougaona_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = true;
                    onMode.Plotting.IsCircle = false;
                }

                TextBlock lblA = new TextBlock();
                lblA.Height = 25;
                lblA.Width = 30;
                lblA.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblA.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblA.Text = "    a ";

                Grid.SetRow(lblA, 3);
                Grid.SetColumn(lblA, 0);


                TextBox tfA = new TextBox();

                tfA.Height = 25;

                tfA.Width = 100;

                tfA.Text = String.Empty;


                Grid.SetRow(tfA, 3);
                Grid.SetColumn(tfA, 1);

                TextBlock lblAmm = new TextBlock();
                lblAmm.Height = 25;
                lblAmm.Width = 30;
                lblAmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblAmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblAmm.Text = " mm";
                Grid.SetRow(lblAmm, 3);
                Grid.SetColumn(lblAmm, 2);


                TextBlock lblB = new TextBlock();
                lblB.Height = 25;
                lblB.Width = 30;
                lblB.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblB.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblB.Text = "    b ";

                Grid.SetRow(lblB, 5);
                Grid.SetColumn(lblB, 0);


                TextBox tfB = new TextBox();

                tfB.Height = 25;

                tfB.Width = 100;

                tfB.Text = String.Empty;


                Grid.SetRow(tfB, 5);
                Grid.SetColumn(tfB, 1);

                TextBlock lblBmm = new TextBlock();
                lblBmm.Height = 25;
                lblBmm.Width = 30;
                lblBmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblBmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblBmm.Text = " mm";
                Grid.SetRow(lblBmm, 5);
                Grid.SetColumn(lblBmm, 2);

                gridTextboxs.Children.Add(lblA);
                gridTextboxs.Children.Add(tfA);
                gridTextboxs.Children.Add(lblAmm);
                gridTextboxs.Children.Add(lblB);
                gridTextboxs.Children.Add(tfB);
                gridTextboxs.Children.Add(lblBmm);

                tfAGlobal = tfA;
                tfBGlobal = tfB;

                tfAGlobal.TextChanged += new TextChangedEventHandler(tfAGlobal_TextChanged);
                tfBGlobal.TextChanged += new TextChangedEventHandler(tfBGlobal_TextChanged);
                tfAGlobal.KeyDown += new KeyEventHandler(tfAGlobal_KeyDown);
                tfBGlobal.KeyDown += new KeyEventHandler(tfBGlobal_KeyDown);
                tfAGlobal.MouseEnter += new MouseEventHandler(tfAGlobal_MouseEnter);

                tfAGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfBGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfAGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
                tfBGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaPravougaona_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

       

        private void rbtnEpvVrstaPravougaona_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                }

                gridTextboxs.Children.Clear();
                setToNullGlobalTextboxes();
                tfS0.Text = String.Empty;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaPravougaona_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }


        private void rbtnEpvVrstaKruzni_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = true;
                }

                TextBlock lblD = new TextBlock();
                lblD.Height = 25;
                lblD.Width = 30;
                lblD.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblD.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblD.Text = "    D ";

                Grid.SetRow(lblD, 4);
                Grid.SetColumn(lblD, 0);


                TextBox tfD = new TextBox();

                tfD.Height = 25;

                tfD.Width = 100;

                tfD.Text = String.Empty;


                Grid.SetRow(tfD, 4);
                Grid.SetColumn(tfD, 1);

                TextBlock lblDmm = new TextBlock();
                lblDmm.Height = 25;
                lblDmm.Width = 30;
                lblDmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblDmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblDmm.Text = " mm";
                Grid.SetRow(lblDmm, 4);
                Grid.SetColumn(lblDmm, 2);

                gridTextboxs.Children.Add(lblD);
                gridTextboxs.Children.Add(tfD);
                gridTextboxs.Children.Add(lblDmm);

                tfDGlobal = tfD;

                tfDGlobal.TextChanged += new TextChangedEventHandler(tfDGlobal_TextChanged);
                tfDGlobal.KeyDown += new KeyEventHandler(tfDGlobal_KeyDown);
                tfDGlobal.MouseEnter += new MouseEventHandler(tfDGlobal_MouseEnter);

                tfDGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfDGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaKruzni_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

     

      

        private void rbtnEpvVrstaKruzni_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsCircle = false;
                }

                gridTextboxs.Children.Clear();
                setToNullGlobalTextboxes();
                tfS0.Text = String.Empty;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaKruzni_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaCevasti_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                TextBlock lblD = new TextBlock();
                lblD.Height = 25;
                lblD.Width = 30;
                lblD.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblD.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblD.Text = "    D ";

                Grid.SetRow(lblD, 3);
                Grid.SetColumn(lblD, 0);


                TextBox tfD = new TextBox();

                tfD.Height = 25;

                tfD.Width = 100;

                tfD.Text = String.Empty;


                Grid.SetRow(tfD, 3);
                Grid.SetColumn(tfD, 1);

                TextBlock lblDmm = new TextBlock();
                lblDmm.Height = 25;
                lblDmm.Width = 30;
                lblDmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblDmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblDmm.Text = " mm";
                Grid.SetRow(lblDmm, 3);
                Grid.SetColumn(lblDmm, 2);


                TextBlock lblA = new TextBlock();
                lblA.Height = 25;
                lblA.Width = 30;
                lblA.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblA.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblA.Text = "    a ";

                Grid.SetRow(lblA, 5);
                Grid.SetColumn(lblA, 0);


                TextBox tfA = new TextBox();

                tfA.Height = 25;

                tfA.Width = 100;

                tfA.Text = String.Empty;


                Grid.SetRow(tfA, 5);
                Grid.SetColumn(tfA, 1);

                TextBlock lblAmm = new TextBlock();
                lblAmm.Height = 25;
                lblAmm.Width = 30;
                lblAmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblAmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblAmm.Text = " mm";
                Grid.SetRow(lblAmm, 5);
                Grid.SetColumn(lblAmm, 2);


                gridTextboxs.Children.Add(lblD);
                gridTextboxs.Children.Add(tfD);
                gridTextboxs.Children.Add(lblDmm);
                gridTextboxs.Children.Add(lblA);
                gridTextboxs.Children.Add(tfA);
                gridTextboxs.Children.Add(lblAmm);

                tfDGlobal = tfD;
                tfAGlobal = tfA;

                tfDGlobal.TextChanged += new TextChangedEventHandler(tfDGlobal_TextChanged);
                tfAGlobal.TextChanged += new TextChangedEventHandler(tfAGlobal_TextChanged);
                tfDGlobal.MouseEnter += new MouseEventHandler(tfDGlobal_MouseEnter);
                tfDGlobal.KeyDown += new KeyEventHandler(tfDGlobal_KeyDown);
                tfAGlobal.KeyDown += new KeyEventHandler(tfAGlobal_KeyDown);

                tfAGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfDGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfAGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
                tfDGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaCevasti_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaCevasti_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                gridTextboxs.Children.Clear();
                setToNullGlobalTextboxes();
                tfS0.Text = String.Empty;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaCevasti_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaDeocev_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                TextBlock lblD = new TextBlock();
                lblD.Height = 25;
                lblD.Width = 30;
                lblD.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblD.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblD.Text = "    D ";

                Grid.SetRow(lblD, 2);
                Grid.SetColumn(lblD, 0);


                TextBox tfD = new TextBox();

                tfD.Height = 25;

                tfD.Width = 100;

                tfD.Text = String.Empty;


                Grid.SetRow(tfD, 2);
                Grid.SetColumn(tfD, 1);

                TextBlock lblDmm = new TextBlock();
                lblDmm.Height = 25;
                lblDmm.Width = 30;
                lblDmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblDmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblDmm.Text = " mm";
                Grid.SetRow(lblDmm, 2);
                Grid.SetColumn(lblDmm, 2);

                TextBlock lblA = new TextBlock();
                lblA.Height = 25;
                lblA.Width = 30;
                lblA.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblA.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblA.Text = "    a ";

                Grid.SetRow(lblA, 4);
                Grid.SetColumn(lblA, 0);


                TextBox tfA = new TextBox();

                tfA.Height = 25;

                tfA.Width = 100;

                tfA.Text = String.Empty;


                Grid.SetRow(tfA, 4);
                Grid.SetColumn(tfA, 1);

                TextBlock lblAmm = new TextBlock();
                lblAmm.Height = 25;
                lblAmm.Width = 30;
                lblAmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblAmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblAmm.Text = " mm";
                Grid.SetRow(lblAmm, 4);
                Grid.SetColumn(lblAmm, 2);


                TextBlock lblB = new TextBlock();
                lblB.Height = 25;
                lblB.Width = 30;
                lblB.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblB.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblB.Text = "    b ";

                Grid.SetRow(lblB, 6);
                Grid.SetColumn(lblB, 0);


                TextBox tfB = new TextBox();

                tfB.Height = 25;

                tfB.Width = 100;

                tfB.Text = String.Empty;


                Grid.SetRow(tfB, 6);
                Grid.SetColumn(tfB, 1);

                TextBlock lblBmm = new TextBlock();
                lblBmm.Height = 25;
                lblBmm.Width = 30;
                lblBmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblBmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblBmm.Text = " mm";
                Grid.SetRow(lblBmm, 6);
                Grid.SetColumn(lblBmm, 2);

                gridTextboxs.Children.Add(lblD);
                gridTextboxs.Children.Add(tfD);
                gridTextboxs.Children.Add(lblDmm);
                gridTextboxs.Children.Add(lblA);
                gridTextboxs.Children.Add(tfA);
                gridTextboxs.Children.Add(lblAmm);
                gridTextboxs.Children.Add(lblB);
                gridTextboxs.Children.Add(tfB);
                gridTextboxs.Children.Add(lblBmm);

                tfDGlobal = tfD;
                tfAGlobal = tfA;
                tfBGlobal = tfB;

                tfDGlobal.TextChanged += new TextChangedEventHandler(tfDGlobal_TextChanged);
                tfAGlobal.TextChanged += new TextChangedEventHandler(tfAGlobal_TextChanged);
                tfBGlobal.TextChanged += new TextChangedEventHandler(tfBGlobal_TextChanged);
                tfDGlobal.MouseEnter += new MouseEventHandler(tfDGlobal_MouseEnter);
                tfDGlobal.KeyDown += new KeyEventHandler(tfDGlobal_KeyDown);
                tfAGlobal.KeyDown += new KeyEventHandler(tfAGlobal_KeyDown);
                tfBGlobal.KeyDown += new KeyEventHandler(tfBGlobal_KeyDown);

                tfAGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfBGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfDGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfAGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
                tfBGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
                tfDGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaDeocev_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaDeocev_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                gridTextboxs.Children.Clear();
                setToNullGlobalTextboxes();
                tfS0.Text = String.Empty;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaDeocev_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaMaseni_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                TextBlock lblM = new TextBlock();
                lblM.Height = 25;
                lblM.Width = 30;
                lblM.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblM.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblM.Text = "    M ";

                Grid.SetRow(lblM, 2);
                Grid.SetColumn(lblM, 0);


                TextBox tfM = new TextBox();

                tfM.Height = 25;

                tfM.Width = 100;

                tfM.Text = String.Empty;


                Grid.SetRow(tfM, 2);
                Grid.SetColumn(tfM, 1);


                TextBlock lblP = new TextBlock();
                lblP.Height = 25;
                lblP.Width = 30;
                lblP.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblP.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblP.Text = "    p ";

                Grid.SetRow(lblP, 4);
                Grid.SetColumn(lblP, 0);


                TextBox tfP = new TextBox();

                tfP.Height = 25;

                tfP.Width = 100;

                tfP.Text = String.Empty;


                Grid.SetRow(tfP, 4);
                Grid.SetColumn(tfP, 1);


                TextBlock lblLT = new TextBlock();
                lblLT.Height = 25;
                lblLT.Width = 30;
                lblLT.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblLT.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblLT.Text = "    Lt ";

                Grid.SetRow(lblLT, 6);
                Grid.SetColumn(lblLT, 0);


                TextBox tfLT = new TextBox();

                tfLT.Height = 25;

                tfLT.Width = 100;

                tfLT.Text = String.Empty;


                Grid.SetRow(tfLT, 6);
                Grid.SetColumn(tfLT, 1);

                gridTextboxs.Children.Add(lblM);
                gridTextboxs.Children.Add(tfM);
                gridTextboxs.Children.Add(lblP);
                gridTextboxs.Children.Add(tfP);
                gridTextboxs.Children.Add(lblLT);
                gridTextboxs.Children.Add(tfLT);

                tfMGlobal = tfM;
                tfPGlobal = tfP;
                tfLTGlobal = tfLT;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaMaseni_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaMaseni_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                gridTextboxs.Children.Clear();
                setToNullGlobalTextboxes();
                tfS0.Text = String.Empty;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaMaseni_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvVrstaSestaugaona_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                TextBlock lblD = new TextBlock();
                lblD.Height = 25;
                lblD.Width = 30;
                lblD.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblD.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                lblD.Text = "    d ";

                Grid.SetRow(lblD, 4);
                Grid.SetColumn(lblD, 0);


                TextBox tfD = new TextBox();

                tfD.Height = 25;

                tfD.Width = 100;

                tfD.Text = String.Empty;


                Grid.SetRow(tfD, 4);
                Grid.SetColumn(tfD, 1);

                TextBlock lblDmm = new TextBlock();
                lblDmm.Height = 25;
                lblDmm.Width = 30;
                lblDmm.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                lblDmm.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                lblDmm.Text = " mm";
                Grid.SetRow(lblDmm, 4);
                Grid.SetColumn(lblDmm, 2);

                gridTextboxs.Children.Add(lblD);
                gridTextboxs.Children.Add(tfD);
                gridTextboxs.Children.Add(lblDmm);

                tfDGlobal = tfD;

                tfDGlobal.TextChanged += new TextChangedEventHandler(tfDGlobal_TextChanged);
                tfDGlobal.MouseEnter += new MouseEventHandler(tfDGlobal_MouseEnter);
                tfDGlobal.KeyDown += new KeyEventHandler(tfDGlobal_KeyDown);

                tfDGlobal.Foreground = System.Windows.Media.Brushes.Black;
                tfDGlobal.Background = System.Windows.Media.Brushes.AliceBlue;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaSestaugaona_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }

        private void rbtnEpvVrstaSestaugaona_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (onMode != null)
                {
                    onMode.Plotting.IsRectangle = false;
                    onMode.Plotting.IsCircle = false;
                }

                gridTextboxs.Children.Clear();
                setToNullGlobalTextboxes();
                tfS0.Text = String.Empty;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvVrstaSestaugaona_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        #region closingEvent

        public int MakeOnlineFileHeader()
        {
            try
            {
                return makeOnlineFileHeader();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {public int MakeOnlineFileHeader()}", System.DateTime.Now);
                return 0;
            }
        }

        private int makeOnlineFileHeader()
        {
            try
            {
                List<string> lines = new List<string>();
                lines.Add(Constants.ONLINEFILEHEADER_FIRSTLINE);

                lines.Add(string.Empty);
                #region GeneraData

                lines.Add(Constants.ONLINEFILEHEADER_OPSTIPODACI);
                //lines.Add(Constants.ONLINEFILEHEADER_BROJIZVESTAJA + generalData.tfbrIzvestaja.Text);
                lines.Add(Constants.ONLINEFILEHEADER_OPERATOR + generalData.tfOperator.Text);
                lines.Add(Constants.ONLINEFILEHEADER_BRZBIZVESTAJA + generalData.tfBrZbIzvestaja.Text);
                lines.Add(Constants.ONLINEFILEHEADER_BRUZORKA + generalData.tfBrUzorka.Text + Constants.KOSA_CRTA + generalData.tfBrUzorkaNumberOfSample.Text);
                lines.Add(Constants.ONLINEFILEHEADER_SARZA + generalData.tfSarza.Text);
                lines.Add(Constants.ONLINEFILEHEADER_RADNINALOG + generalData.tfRadniNalog.Text);
                lines.Add(Constants.ONLINEFILEHEADER_NARUCILAC + generalData.tfNaručilac.Text);


                #endregion

                lines.Add(string.Empty);
                #region ConditionsOfTesting

                lines.Add(Constants.ONLINEFILEHEADER_USLOVIISPITIVANJA);
                lines.Add(Constants.ONLINEFILEHEADER_STANDARD + conditionsOfTesting.tfStandard.Text);
                lines.Add(Constants.ONLINEFILEHEADER_METODA + conditionsOfTesting.tfMetoda.Text);
                lines.Add(Constants.ONLINEFILEHEADER_STANDARDZAN + conditionsOfTesting.tfStandardZaN.Text);
                lines.Add(Constants.ONLINEFILEHEADER_MASINA + conditionsOfTesting.tfMasina.Text);
                var celzijusStepeni = "\xB0 C - ";//oznaka za celzijusov stepen // or "unit\xB2"
                var celzijusStepeni2 = "\xB0 C";
                //lines.Add(Constants.ONLINEFILEHEADER_OPSEGMASINE + conditionsOfTesting.tfBegOpsegMasine.Text + celzijusStepeni + conditionsOfTesting.tfEndOpsegMasine.Text + celzijusStepeni2);
                lines.Add(Constants.ONLINEFILEHEADER_TEMPERATURA + conditionsOfTesting.tfTemperatura.Text);
                if (conditionsOfTesting.rbtnYes.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_EKSTENZIOMETAR + "DA");
                }
                if (conditionsOfTesting.rbtnNo.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_EKSTENZIOMETAR + "NE");
                }

                #endregion


                lines.Add(string.Empty);
                #region MaterialForTesting

                lines.Add(Constants.ONLINEFILEHEADER_MATERIJALISPITIVANJA);
                lines.Add(Constants.ONLINEFILEHEADER_PROIZVODJAC + materialForTesting.tfProizvodjac.Text);
                lines.Add(Constants.ONLINEFILEHEADER_DOBAVLJAC + materialForTesting.tfDobavljac.Text);
                lines.Add(Constants.ONLINEFILEHEADER_POLAZNIKVALITET + materialForTesting.tfPolazniKvalitet.Text);
                lines.Add(Constants.ONLINEFILEHEADER_NAZIVNADEBLJINA + materialForTesting.tfNazivnaDebljina.Text);
                if (materialForTesting.rbtnValjani.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "Valjani");
                }
                if (materialForTesting.rbtnVučeni.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "Vučeni");
                }
                if (materialForTesting.rbtnKovani.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "Kovani");
                }
                if (materialForTesting.rbtnLiveni.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_NACINPRERADE + "Liveni");
                }

                #endregion

                lines.Add(string.Empty);
                #region Epruveta

                lines.Add(Constants.ONLINEFILEHEADER_EPRUVETA);
                if (rbtnEpvOblikObradjena.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_EPRUVETAOBLIK + "Obradjena");
                }
                if (rbtnEpvOblikNeobradjena.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_EPRUVETAOBLIK + "Neobradjena");
                }
                if (rbtnEpvTipProporcionalna.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_TIP + "Proporcionalna");
                    if (rbtnEpvK1.IsChecked == true)
                    {
                        lines.Add(Constants.ONLINEFILEHEADER_K + "5.65");
                    }
                    if (rbtnEpvK2.IsChecked == true)
                    {
                        lines.Add(Constants.ONLINEFILEHEADER_K + "11.3");
                    }
                }
                if (rbtnEpvTipNeproporcionalna.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_TIP + "Neproporcionalna");
                }
                //if (rbtnEpvUzduzni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_PRAVACISPITIV + "Uzdužni");
                //}
                //if (rbtnEpvPoprecni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_PRAVACISPITIV + "Poprečni");
                //}
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Pravougaona");
                }
                if (rbtnEpvVrstaKruzni.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Kružni");
                }
                if (rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Cevasti");
                }
                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Deo cev.");
                }
                if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                {
                    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Šestaugaona");
                }
                lines.Add(Constants.ONLINEFILEHEADER_S0 + tfS0.Text);
                lines.Add(Constants.ONLINEFILEHEADER_L0 + tfL0.Text);
                lines.Add(Constants.ONLINEFILEHEADER_LC + tfLc.Text);


                #endregion


                lines.Add(string.Empty);

                #region PositionOfTube

                lines.Add(Constants.ONLINEFILEHEADER_POLOZAJEPRUVETE);

                string zeroStr = "0\xB0";
                string fortyfiveStr = "45\xB0";
                string ninetyStr = "90\xB0";
                string customDegree = "\xB0";

                lines.Add(Constants.ONLINEFILEHEADER_PRAVACVALJANJA + positionOfTube.tfCustomPravacValjanja.Text);

                lines.Add(Constants.ONLINEFILEHEADER_SIRINATRAKE + positionOfTube.tfCustomSirinaTrake.Text);

                lines.Add(Constants.ONLINEFILEHEADER_DUZINATRAKE + positionOfTube.tfCustomDuzinaTrake.Text);


                #endregion

                lines.Add(System.Environment.NewLine);
                #region RemarkOfTesting

                var textRangeNapomena = new TextRange(remarkOfTesting.rtfNapomena.Document.ContentStart, remarkOfTesting.rtfNapomena.Document.ContentEnd);
                string textRangeNapomenaString = textRangeNapomena.Text;
                textRangeNapomenaString = textRangeNapomenaString.Replace("\r\n", String.Empty);
                lines.Add(Constants.ONLINEFILEHEADER_NAPOMENA + textRangeNapomenaString);

                #endregion





                //var textRangeNapomena = new TextRange(rtfNapomena.Document.ContentStart, rtfNapomena.Document.ContentEnd);
                //string textRangeNapomenaString = textRangeNapomena.Text;
                //textRangeNapomenaString = textRangeNapomenaString.Replace("\r\n", String.Empty);
                //lines.Add(Constants.ONLINEFILEHEADER_NAPOMENA + textRangeNapomenaString);
                lines.Add(System.Environment.NewLine);
                lines.Add(Constants.ONLINEFILEHEADER_BrojZapisa);
                lines.Add(Constants.ONLINEFILEHEADER_SilaA);
                HeaderSize = lines.Count;

                File.WriteAllLines(onMode.FpOnlineGlobal, lines);

                return HeaderSize;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private int makeOnlineFileHeader()}", System.DateTime.Now);
                return 0;
            }
        }

        private void checkCorrectionOfOnlineFileHeader(System.ComponentModel.CancelEventArgs e) 
        {
            try
            {
                //List<string> lines = new List<string>();
                //lines.Add(Constants.ONLINEFILEHEADER_FIRSTLINE);

                //if(rbtnVuceni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_PROGRAM + "Vučeni");
                //}
                //if (rbtnValjani.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_PROGRAM + "Valjani");
                //}
                //lines.Add(Constants.ONLINEFILEHEADER_ISPITIVANJE);
                //lines.Add(Constants.ONLINEFILEHEADER_STANDARD + tfStandard.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_MASINA + tfMasina.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_NARUCILAC);
                //lines.Add(Constants.ONLINEFILEHEADER_NARUCILAC2 + tfNarucilac.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_RADNINALOG + tfRadniNalog.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_ZAHZAISP + tfZahZaIsp.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_NALZAISP + tfNalZaIsp.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_MATERIJAL);
                //lines.Add(Constants.ONLINEFILEHEADER_OBLIK + tfOblik.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_SASTAV + tfSastav.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_STDPROIZV + tfStdProizv.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_UZORAK + tfUzorak.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_BROJMERENJA + tfBrojMerenja.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_BRTRAKROND + tfBrTrakRond.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_EPRUVETA);
                //if (rbtnEpvOblikObradjena.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_EPRUVETAOBLIK + "Obradjena");
                //}
                //if (rbtnEpvOblikNeobradjena.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_EPRUVETAOBLIK + "Neobradjena");
                //}
                //if (rbtnEpvTipProporcionalna.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_TIP + "Proporcionalna");
                //    if (rbtnEpvK1.IsChecked == true)
                //    {
                //        lines.Add(Constants.ONLINEFILEHEADER_K + "5.65");
                //    }
                //    if (rbtnEpvK2.IsChecked == true)
                //    {
                //        lines.Add(Constants.ONLINEFILEHEADER_K + "11.3");
                //    }
                //}
                //if (rbtnEpvTipNeproporcionalna.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_TIP + "Neproporcionalna");
                //}
                //if (rbtnEpvUzduzni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_PRAVACISPITIV + "Uzdužni");
                //}
                //if (rbtnEpvPoprecni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_PRAVACISPITIV + "Poprečni");
                //}
                //if (rbtnEpvVrstaPravougaona.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Pravougaona");
                //}
                //if (rbtnEpvVrstaKruzni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Kružni");
                //}
                //if (rbtnEpvVrstaCevasti.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Cevasti");
                //}
                //if (rbtnEpvVrstaDeocev.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Deo cev.");
                //}
                //if (rbtnEpvVrstaMaseni.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Maseni");
                //}
                //if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                //{
                //    lines.Add(Constants.ONLINEFILEHEADER_VRSTAEPRUVETE + "Šestaugaona");
                //}
                //lines.Add(Constants.ONLINEFILEHEADER_S0 + tfS0.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_L0 + tfL0.Text);
                //var textRangeNapomena = new TextRange(rtfNapomena.Document.ContentStart, rtfNapomena.Document.ContentEnd);
                //lines.Add(Constants.ONLINEFILEHEADER_NAPOMENA + textRangeNapomena.Text);
                //lines.Add(Constants.ONLINEFILEHEADER_BrojZapisa);
                //lines.Add(Constants.ONLINEFILEHEADER_SilaA);
                //HeaderSize = lines.Count;

                //File.WriteAllLines(onMode.FpOnlineGlobal, lines);


                //set and save s0 and l0
                double s0 = Double.MinValue;
                strs0 = tfS0.Text.Replace(',', '.');
                //bool isN = Double.TryParse(strs0,out s0);
                //if (isN == false)
                //{
                //    MessageBox.Show("Parametar So nije zabeležen u obliku broja!");
                //    e.Cancel = true;
                //    return;
                //}
                double l0 = Double.MinValue;
                strl0 = tfL0.Text.Replace(',', '.');
                //isN = Double.TryParse(strl0, out l0);
                //if (isN == false)
                //{
                //    MessageBox.Show("Parametar Lo nije zabeležen u obliku broja!");
                //    e.Cancel = true;
                //    return;
                //}

                //OptionsInOnlineMode.S0 = s0;
                //OptionsInOnlineMode.L0 = l0;

                //onMode.OptionsOnline.WriteXMLOnlineFile();

                double lc = Double.MinValue;
                strlc = tfLc.Text.Replace(',', '.');
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private int makeOnlineFileHeader()}", System.DateTime.Now);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //if (tfBGlobal != null)
                //{
                //    MessageBox.Show(tfBGlobal.Text);
                //}
                //List<string> lines = new List<string>();

                //lines.Add("Zaglavlje online fajla");

                //File.WriteAllLines(onMode.FpOnlineGlobal,lines);
                OnlineFileHeader.isCreatedOnlineHeader = false;
                if (rbtnEpvVrstaPravougaona.IsChecked == true)
                {
                    double aglobal;
                    strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    //bool isN = Double.TryParse(strtfAGlobal, out aglobal);
                    //if (isN == false)
                    //{
                    //    MessageBox.Show("Parametar a mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}

                    double bglobal;
                    strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                    //bool isNN = Double.TryParse(strtfBGlobal, out bglobal);
                    //if (isNN == false)
                    //{
                    //    MessageBox.Show("Parametar b mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}
                }

                if (rbtnEpvVrstaKruzni.IsChecked == true)
                {
                    double dglobal;
                    strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    //bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    //if (isN == false)
                    //{
                    //    MessageBox.Show("Parametar d mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}
                }

                if (rbtnEpvVrstaCevasti.IsChecked == true)
                {
                    double dglobal;
                    strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    //bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    //if (isN == false)
                    //{
                    //    MessageBox.Show("Parametar d mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}

                    double aglobal;
                    strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    //bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                    //if (isNN == false)
                    //{
                    //    MessageBox.Show("Parametar a mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}
                }


                if (rbtnEpvVrstaDeocev.IsChecked == true)
                {
                    double dglobal;
                    strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    //bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    //if (isN == false)
                    //{
                    //    MessageBox.Show("Parametar d mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}

                    double aglobal;
                    strtfAGlobal = tfAGlobal.Text.Replace(',', '.');
                    //bool isNN = Double.TryParse(strtfAGlobal, out aglobal);
                    //if (isNN == false)
                    //{
                    //    MessageBox.Show("Parametar a mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}

                    double bglobal;
                    strtfBGlobal = tfBGlobal.Text.Replace(',', '.');
                    //bool isNNN = Double.TryParse(strtfBGlobal, out bglobal);
                    //if (isNNN == false)
                    //{
                    //    MessageBox.Show("Parametar b mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}
                }

                //if (rbtnEpvVrstaMaseni.IsChecked == true)
                //{
                //    double mglobal;
                //    string strtfMGlobal = tfMGlobal.Text.Replace(',', '.');
                //    bool isN = Double.TryParse(strtfMGlobal, out mglobal);
                //    if (isN == false)
                //    {
                //        MessageBox.Show("Parametar M mora biti unet kao broj !");
                //        e.Cancel = true;
                //    }

                //    double pglobal;
                //    string strtfPGlobal = tfPGlobal.Text.Replace(',', '.');
                //    bool isNN = Double.TryParse(strtfPGlobal, out pglobal);
                //    if (isNN == false)
                //    {
                //        MessageBox.Show("Parametar P mora biti unet kao broj !");
                //        e.Cancel = true;
                //    }

                //    double ltglobal;
                //    string strtfLtGlobal = tfLTGlobal.Text.Replace(',', '.');
                //    bool isNNN = Double.TryParse(strtfLtGlobal, out ltglobal);
                //    if (isNNN == false)
                //    {
                //        MessageBox.Show("Parametar Lt mora biti unet kao broj !");
                //        e.Cancel = true;
                //    }
                //}

                if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                {
                    double dglobal;
                    strtfDGlobal = tfDGlobal.Text.Replace(',', '.');
                    //bool isN = Double.TryParse(strtfDGlobal, out dglobal);
                    //if (isN == false)
                    //{
                    //    MessageBox.Show("Parametar d mora biti unet kao broj !");
                    //    e.Cancel = true;
                    //}
                }

                checkCorrectionOfOnlineFileHeader(e);
                //onMode.IsOnlineFileHeaderWritten = true;
                //onMode.removeRemarkForOnlineFileHeaderWritten();

                //onMode.WriteXMLLastOnlineHeader();

                //if (tfAGlobal != null)
                //{
                //    if(rbtnEpvVrstaPravougaona.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.a0Pravougaona = tfAGlobal.Text;
                //    }
                //    if (rbtnEpvVrstaCevasti.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.a0Cevasta = tfAGlobal.Text;
                //    }
                //    if (rbtnEpvVrstaDeocev.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.a0Deocev = tfAGlobal.Text;
                //    }
                //}

                //if (tfBGlobal != null)
                //{
                //    if (rbtnEpvVrstaPravougaona.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.b0Pravougaona = tfBGlobal.Text;
                //    }
                //    if (rbtnEpvVrstaDeocev.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.b0Deocev = tfBGlobal.Text;
                //    }
                //}

                //if (tfDGlobal != null)
                //{
                //    if (rbtnEpvVrstaKruzni.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.D0Kruzna = tfDGlobal.Text;
                //    }
                //    if (rbtnEpvVrstaCevasti.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.D0Cevasta = tfDGlobal.Text;
                //    }
                //    if (rbtnEpvVrstaDeocev.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.D0Deocev = tfDGlobal.Text;
                //    }
                //    if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                //    {
                //        LastInputOutputSavedData.d0Sestaugaona = tfDGlobal.Text;
                //    }
                //}

                onMode.WriteXMLLastOnlineHeader();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
            
        }

        #endregion

        private void rbtnEpvTipProporcionalna_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rbtnEpvK1.IsEnabled = true;
                rbtnEpvK2.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvTipProporcionalna_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void rbtnEpvTipNeproporcionalna_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                rbtnEpvK1.IsEnabled = false;
                rbtnEpvK2.IsEnabled = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void rbtnEpvTipNeproporcionalna_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

      
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (this.Width == this.MaxWidth && this.Height == this.MaxHeight)
                {
                    this.Left = xconst;
                    this.Top = yconst;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void Window_SizeChanged(object sender, SizeChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                this.GeneralData.Close();
                this.ConditionsOfTesting.Close();
                this.MaterialForTesting.Close();
                this.PositionOfTube.Close();
                this.RemarkOfTesting.Close();
                this.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void Window_Closed(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfS0_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void tfL0_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    tfLc.SelectAll();
                    tfLc.Focus();
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfL0_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfLc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    if (rbtnEpvVrstaPravougaona.IsChecked == true)
                    {
                        if (tfAGlobal != null)
                        {
                            tfAGlobal.SelectAll();
                            tfAGlobal.Focus();
                        }
                    }

                    if (rbtnEpvVrstaKruzni.IsChecked == true)
                    {
                        if (tfDGlobal != null)
                        {
                            tfDGlobal.SelectAll();
                            tfDGlobal.Focus();
                            tfDGlobal.IsReadOnly = false;
                        }
                    }

                    if (rbtnEpvVrstaCevasti.IsChecked == true)
                    {
                        if (tfDGlobal != null)
                        {
                            tfDGlobal.SelectAll();
                            tfDGlobal.Focus();
                            tfDGlobal.IsReadOnly = false;
                        }
                    }

                    if (rbtnEpvVrstaDeocev.IsChecked == true)
                    {
                        if (tfDGlobal != null)
                        {
                            tfDGlobal.SelectAll();
                            tfDGlobal.Focus();
                            tfDGlobal.IsReadOnly = false;
                        }
                    }

                    if (rbtnEpvVrstaSestaugaona.IsChecked == true)
                    {
                        if (tfDGlobal != null)
                        {
                            tfDGlobal.SelectAll();
                            tfDGlobal.Focus();
                            tfDGlobal.IsReadOnly = false;
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OnlineFileHeader.xaml.cs] {private void tfLc_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

    


        #region MoveFocusFromRadioButtons

        private void rbtnEpvOblikObradjena_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvOblikNeobradjena_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvTipProporcionalna_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvTipNeproporcionalna_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }



        private void rbtnEpvK1_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvK2_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvVrstaPravougaona_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvVrstaKruzni_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }


        private void rbtnEpvVrstaCevasti_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvVrstaDeocev_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        private void rbtnEpvVrstaSestaugaona_GotFocus(object sender, RoutedEventArgs e)
        {
            // MoveFocus takes a TraversalRequest as its argument.
            TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);

            // Gets the element with keyboard focus.
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                elementWithFocus.MoveFocus(request);
            }
        }

        #endregion

     

       


       

     

      


       

        







    }
}
