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
using System.Xml;
using System.Windows.Forms;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.PointMarkers;
using Microsoft.Research.DynamicDataDisplay;
using System.IO;

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for OptionsPlotting.xaml
    /// </summary>
    public partial class OptionsPlotting : Window
    {

        public static bool isCreatedOptionsPlotting = false;

        private GraphicPlotting _graphicPlotting;

        public GraphicPlotting GraphicPlotting
        {
            set
            {
                if (value != null)
                {
                    _graphicPlotting = value;
                }
            }
        }

        private void redrawAfterChangingResolutionOfDisplay()
        {
            try
            {
                _graphicPlotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                _graphicPlotting.DrawFittingGraphic();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void redrawAfterChangingResolutionOfDisplay()}", System.DateTime.Now);
            }
        }

        private void redrawAfterChangingCalibrationparameter() 
        {
            try
            {
                _graphicPlotting.Redrawing.DataReader.ReadData();
                _graphicPlotting.btnPlottingMode.RaiseEvent(new RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));
                _graphicPlotting.DrawFittingGraphic();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void redrawAfterChangingCalibrationparameter()}", System.DateTime.Now);
            }
        }

        private void emptyRelReHRpmaxFmax()
        {
            try
            {
                _graphicPlotting.tfReL.Text = String.Empty;
                if (_graphicPlotting != null && _graphicPlotting.OnlineModeInstance != null && _graphicPlotting.OnlineModeInstance.ResultsInterface != null)
                {
                    _graphicPlotting.OnlineModeInstance.ResultsInterface.tfReL.Text = String.Empty;
                }
                _graphicPlotting.ReL = -1;
                _graphicPlotting.MarkerGraph5.DataSource = null;
                //_graphicPlotting.MarkerGraphText5.DataSource = null;
                _graphicPlotting.tfReH.Text = String.Empty;
                _graphicPlotting.ReH = -1;
                _graphicPlotting.MarkerGraph6.DataSource = null;
                // _graphicPlotting.MarkerGraphText6.DataSource = null;
                _graphicPlotting.tfRm.Text = String.Empty;
                _graphicPlotting.Rm = -1;
                _graphicPlotting.MarkerGraph4.DataSource = null;
                // _graphicPlotting.MarkerGraphText4.DataSource = null;
                _graphicPlotting.tfFm.Text = String.Empty;
                _graphicPlotting.Fm = -1;
                //_graphicPlotting.tfRp02.Text = String.Empty;
                //_graphicPlotting.Rp02RI = -1;
                //_graphicPlotting.chbRp02Visibility.IsEnabled = false;
                //_graphicPlotting.chbRp02Visibility.IsChecked = true;
                //_graphicPlotting.MarkerGraph7.DataSource = null;
                // _graphicPlotting.MarkerGraphText7.DataSource = null;
                _graphicPlotting.tfA.Text = String.Empty;
                _graphicPlotting.A = -1;
                _graphicPlotting.chbAVisibility.IsChecked = true;
                //_graphicPlotting.chbAVisibility.IsEnabled = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void emptyRelReHRpmaxFmax()}", System.DateTime.Now);
            }

        }

        private void emptyRelReHRpmaxFmaxRp02() 
        {
            try
            {
                _graphicPlotting.tfReL.Text = String.Empty;
                if (_graphicPlotting != null && _graphicPlotting.OnlineModeInstance != null && _graphicPlotting.OnlineModeInstance.ResultsInterface != null)
                {
                    _graphicPlotting.OnlineModeInstance.ResultsInterface.tfReL.Text = String.Empty;
                }
                _graphicPlotting.ReL = -1;
                _graphicPlotting.MarkerGraph5.DataSource = null;
                //_graphicPlotting.MarkerGraphText5.DataSource = null;
                _graphicPlotting.tfReH.Text = String.Empty;
                _graphicPlotting.ReH = -1;
                _graphicPlotting.MarkerGraph6.DataSource = null;
                // _graphicPlotting.MarkerGraphText6.DataSource = null;
                _graphicPlotting.tfRm.Text = String.Empty;
                _graphicPlotting.Rm = -1;
                _graphicPlotting.MarkerGraph4.DataSource = null;
                // _graphicPlotting.MarkerGraphText4.DataSource = null;
                _graphicPlotting.tfFm.Text = String.Empty;
                _graphicPlotting.Fm = -1;
                _graphicPlotting.tfRp02.Text = String.Empty;
                _graphicPlotting.Rp02RI = -1;
                //_graphicPlotting.chbRp02Visibility.IsEnabled = false;
                //_graphicPlotting.chbRp02Visibility.IsChecked = true;
                _graphicPlotting.MarkerGraph7.DataSource = null;
                // _graphicPlotting.MarkerGraphText7.DataSource = null;
                _graphicPlotting.tfA.Text = String.Empty;
                _graphicPlotting.A = -1;
                _graphicPlotting.chbAVisibility.IsChecked = true;
                //_graphicPlotting.chbAVisibility.IsEnabled = false;

            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void emptyRelReHRpmaxFmaxRp02()}", System.DateTime.Now);
            }
            
        }

        public OptionsPlotting() 
        {
            try
            {
                InitializeComponent();
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                this.Title = "Podešavanja opcija iscrtavanja grafika ";
                //if (OptionsInPlottingMode.CalculateFromYungModuo == true)
                //{
                //    chbCalculateFromYung.IsChecked = true;
                //}
                //else
                //{
                //    chbCalculateFromYung.IsChecked = false;
                //}
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public OptionsPlotting()}", System.DateTime.Now);
            }
        }

       
      


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                OptionsPlotting.isCreatedOptionsPlotting = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

      
        
        #region Resolution

        private void saveOptionstfResolution_Offline()
        {
            try
            {
                int resolution;
                string strResolution = tfResolution.Text.Replace(',', '.');
                bool isN = Int32.TryParse(strResolution, out resolution);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje rezolucije prikaza grafika!");
                }
                else
                {
                    OptionsInPlottingMode.Resolution = resolution;
                }


                emptyRelReHRpmaxFmaxRp02();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfResolution_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfResolution_Offline()
        {
            try
            {
                //tfResolution.Foreground = Brushes.Black;
                tfResolution.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfResolution_Offline()}", System.DateTime.Now);
            }
        }

        private void tfResolution_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfResolution.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfResolution_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfResolution_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfResolution_Offline();
                    markSavedOnlineOptionsAsBlacktfResolution_Offline();
                    _graphicPlotting.DeleteFittingPath();
                    _graphicPlotting.DeleteAFittingAndOfflineLines();

                    emptyRelReHRpmaxFmaxRp02();

                    redrawAfterChangingResolutionOfDisplay();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfResolution_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region DerivationResolution

        private void saveOptionstfDerivationResolution_Offline()
        {
            try
            {
                int derivationResolution;
                string strderivationResolution = tfDerivationResolution.Text.Replace(',', '.');
                bool isN = Int32.TryParse(strderivationResolution, out derivationResolution);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje rezolucija računanja izvoda!");
                }
                else
                {
                    OptionsInPlottingMode.DerivationResolution = derivationResolution;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfDerivationResolution_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfDerivationResolution_Offline()
        {
            try
            {
                //tfDerivationResolution.Foreground = Brushes.Black;
                tfDerivationResolution.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfDerivationResolution_Offline()}", System.DateTime.Now);
            }
        }

        private void tfDerivationResolution_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfDerivationResolution.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfDerivationResolution_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDerivationResolution_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfDerivationResolution_Offline();
                    markSavedOnlineOptionsAsBlacktfDerivationResolution_Offline();

                    emptyRelReHRpmaxFmax();

                    _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                    _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfDerivationResolution_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        #region ForceDivide


        private void saveOptionstfCalForceDivide_Offline()
        {
            try
            {
                double forceDiv;
                string strforceDiv = tfCalForceDivide.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceDiv, out forceDiv);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje sile!");
                }
                else
                {
                    OptionsInPlottingMode.nutnDivide = forceDiv;
                    OptionsInOnlineMode.nutnDivide = forceDiv;
                    _graphicPlotting.OnlineModeInstance.OptionsOnline.tfCalForceDivide.Text = forceDiv.ToString();
                }


                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.OnlineModeInstance.OptionsOnline.WriteXMLOnlineFile();
                _graphicPlotting.createOfflineGraphics();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfCalForceDivide_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceDivide_Offline()
        {
            try
            {
                //tfCalForceDivide.Foreground = Brushes.Black;
                tfCalForceDivide.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalForceDivide_Offline()}", System.DateTime.Now);
            }
        }

        private void tfCalForceDivide_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceDivide.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalForceDivide_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalForceDivide_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfCalForceDivide_Offline();
                    markSavedOnlineOptionsAsBlacktfCalForceDivide_Offline();
                    _graphicPlotting.DeleteFittingPath();

                    emptyRelReHRpmaxFmaxRp02();
                    //_graphicPlotting.Redrawing.DataReader.ReadData();
                    //_graphicPlotting.DrawFittingGraphic();

                    redrawAfterChangingCalibrationparameter();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalForceDivide_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ForceMultiple

        private void saveOptionstfCalForceMultiple_Offline()
        {
            try
            {
                double forceMul;
                string strforceMul = tfCalForceMultiple.Text.Replace(',', '.');
                bool isN = Double.TryParse(strforceMul, out forceMul);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje sile!");
                }
                else
                {
                    OptionsInPlottingMode.nutnMultiple = forceMul;
                    OptionsInOnlineMode.nutnMultiple = forceMul;
                    _graphicPlotting.OnlineModeInstance.OptionsOnline.tfCalForceMultiple.Text = forceMul.ToString();
                }


                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.OnlineModeInstance.OptionsOnline.WriteXMLOnlineFile();
                _graphicPlotting.createOfflineGraphics();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfCalForceMultiple_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalForceMultiple_Offline()
        {
            try
            {
                //tfCalForceMultiple.Foreground = Brushes.Black;
                tfCalForceMultiple.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalForceMultiple_Offline()}", System.DateTime.Now);
            }
        }

        private void tfCalForceMultiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalForceMultiple.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalForceMultiple_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalForceMultiple_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfCalForceMultiple_Offline();
                    markSavedOnlineOptionsAsBlacktfCalForceMultiple_Offline();
                    _graphicPlotting.DeleteFittingPath();

                    emptyRelReHRpmaxFmaxRp02();

                    redrawAfterChangingCalibrationparameter();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalForceMultiple_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion


        #region ElongationDivide

        private void saveOptionstfCalElonDivide_Offline()
        {
            try
            {
                double elonDiv;
                string strelonDiv = tfCalElonDivide.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonDiv, out elonDiv);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje izduženja!");
                }
                else
                {
                    OptionsInPlottingMode.mmDivide = elonDiv;
                    OptionsInOnlineMode.mmDivide = elonDiv;
                    _graphicPlotting.OnlineModeInstance.OptionsOnline.tfCalElonDivide.Text = elonDiv.ToString();
                }


                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.OnlineModeInstance.OptionsOnline.WriteXMLOnlineFile();
                _graphicPlotting.createOfflineGraphics();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfCalElonDivide_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonDivide_Offline()
        {
            try
            {
                //tfCalElonDivide.Foreground = Brushes.Black;
                tfCalElonDivide.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonDivide_Offline()}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonDivide.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonDivide_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfCalElonDivide_Offline();
                    markSavedOnlineOptionsAsBlacktfCalElonDivide_Offline();
                    _graphicPlotting.DeleteFittingPath();

                    emptyRelReHRpmaxFmaxRp02();

                    redrawAfterChangingCalibrationparameter();

                    _graphicPlotting.IsChangedElongationCalibrationparameter = true;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonDivide_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }



        #endregion

        #region ElongationMultiple

        private void saveOptionstfCalElonMultiple_Offline()
        {
            try
            {
                double elonMul;
                string strelonMul = tfCalElonMultiple.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonMul, out elonMul);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje sile!");
                }
                else
                {
                    OptionsInPlottingMode.mmCoeff = elonMul;
                    OptionsInOnlineMode.mmCoeff = elonMul;
                    _graphicPlotting.OnlineModeInstance.OptionsOnline.tfCalElonMultiple.Text = elonMul.ToString();
                }

                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.OnlineModeInstance.OptionsOnline.WriteXMLOnlineFile();
                _graphicPlotting.createOfflineGraphics();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfCalElonMultiple_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonMultiple_Offline()
        {
            try
            {
                //tfCalElonMultiple.Foreground = Brushes.Black;
                tfCalElonMultiple.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonMultiple_Offline()}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonMultiple.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonMultiple_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfCalElonMultiple_Offline();
                    markSavedOnlineOptionsAsBlacktfCalElonMultiple_Offline();
                    _graphicPlotting.DeleteFittingPath();

                    emptyRelReHRpmaxFmaxRp02();

                    redrawAfterChangingCalibrationparameter();

                    _graphicPlotting.IsChangedElongationCalibrationparameter = true;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonMultiple_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion

        #region ElongationDivideWithEkstenziometer

        private void saveOptionstfCalElonDivide2_Offline()
        {
            try
            {
                double elonDiv;
                string strelonDiv = tfCalElonDivide2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonDiv, out elonDiv);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje izduženja[ekstenziometar]!");
                }
                else
                {
                    OptionsInPlottingMode.mmDivideWithEkstenziometer = elonDiv;
                    OptionsInOnlineMode.mmDivideWithEkstenziometer = elonDiv;
                    _graphicPlotting.OnlineModeInstance.OptionsOnline.tfCalElonDivide2.Text = elonDiv.ToString();
                }


                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.OnlineModeInstance.OptionsOnline.WriteXMLOnlineFile();
                _graphicPlotting.createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfCalElonDivide2_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonDivide2_Offline()
        {
            try
            {
                //tfCalElonDivide2.Foreground = Brushes.Black;
                tfCalElonDivide2.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonDivide2_Offline()}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonDivide2.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonDivide2_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonDivide2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfCalElonDivide2_Offline();
                    markSavedOnlineOptionsAsBlacktfCalElonDivide2_Offline();
                    _graphicPlotting.DeleteFittingPath();

                    emptyRelReHRpmaxFmaxRp02();

                    redrawAfterChangingCalibrationparameter();

                    _graphicPlotting.IsChangedElongationCalibrationparameter = true;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonDivide2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region ElongationMultipleWithEkstenziometer

        private void saveOptionstfCalElonMultiple2_Offline()
        {
            try
            {
                double elonMul;
                string strelonMul = tfCalElonMultiple2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strelonMul, out elonMul);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje sile[ekstenziometar]!");
                }
                else
                {
                    OptionsInPlottingMode.mmCoeffWithEkstenziometer = elonMul;
                    OptionsInOnlineMode.mmCoeffWithEkstenziometer = elonMul;
                    _graphicPlotting.OnlineModeInstance.OptionsOnline.tfCalElonMultiple2.Text = elonMul.ToString();
                }

                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.OnlineModeInstance.OptionsOnline.WriteXMLOnlineFile();
                _graphicPlotting.createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfCalElonMultiple2_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfCalElonMultiple2_Offline()
        {
            try
            {
                //tfCalElonMultiple2.Foreground = Brushes.Black;
                tfCalElonMultiple2.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfCalElonMultiple2_Offline()}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfCalElonMultiple2.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonMultiple2_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfCalElonMultiple2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsGraphicPlottingLoaded = false;
                    _graphicPlotting.disableFittingPart();
                    saveOptionstfCalElonMultiple2_Offline();
                    markSavedOnlineOptionsAsBlacktfCalElonMultiple2_Offline();
                    _graphicPlotting.DeleteFittingPath();

                    emptyRelReHRpmaxFmaxRp02();

                    redrawAfterChangingCalibrationparameter();

                    _graphicPlotting.IsChangedElongationCalibrationparameter = true;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfCalElonMultiple2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region fittingAutoPoint1

        //public void setTextMarkert1()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(_graphicPlotting.XMarkersText);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(_graphicPlotting.YMarkersText);
        //    //_graphicPlotting.MarkerGraphText.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    _graphicPlotting.XMarkersText[0] = _graphicPlotting.XMarkers[0] + OptionsInPlottingMode.xRange / 120;
        //    _graphicPlotting.YMarkersText[0] = _graphicPlotting.YMarkers[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "T1";
        //    //_graphicPlotting.MarkerGraphText.Marker = mkrText;
        //}

        public void saveOptionstffittingAutoProcent1_Offline()
        {
            try
            {
                double autoProcent1;
                string strautoProcent1 = tffittingAutoProcent1.Text.Replace(',', '.');
                bool isN = Double.TryParse(strautoProcent1, out autoProcent1);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje procenta koordinate prve tačke automatskog fitovanja!");
                }
                else if (autoProcent1 < 0)
                {
                    System.Windows.Forms.MessageBox.Show("Broj unet u polje procenta koordinate prve tačke automatskog fitovanja ne sme biti negativan!");
                    OptionsInPlottingMode.procentAuto1 = 0;
                }
                else
                {
                    OptionsInPlottingMode.procentAuto1 = autoProcent1;
                }

                double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto1 / 100;

                int index = _graphicPlotting.GetClosestPointIndex(criteriaPreassure);
                OptionsInPlottingMode.pointAutoX1 = _graphicPlotting.DataReader.RelativeElongation[index];
                OptionsInPlottingMode.pointAutoY1 = _graphicPlotting.DataReader.PreassureInMPa[index];

                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void saveOptionstffittingAutoProcent1_Offline()}", System.DateTime.Now);
            }

        }



        public void setPointAtGraphictffittingAutoProcent1()
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(_graphicPlotting.XMarkers);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(_graphicPlotting.YMarkers);
                _graphicPlotting.MarkerGraph.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;



                _graphicPlotting.XMarkers[0] = OptionsInPlottingMode.pointAutoX1;
                _graphicPlotting.YMarkers[0] = OptionsInPlottingMode.pointAutoY1;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Blue);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);


                _graphicPlotting.MarkerGraph.Marker = mkr;

                //setTextMarkert1();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void setPointAtGraphictffittingAutoProcent1()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingAutoProcent1_Offline()
        {
            try
            {
                //tffittingAutoProcent1.Foreground = Brushes.Black;
                tffittingAutoProcent1.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingAutoProcent1_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingAutoProcent1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingAutoProcent1.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tffittingAutoProcent1_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingAutoProcent1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.SetManualPointsToAutoPointsValue();
                    saveOptionstffittingAutoProcent1_Offline();
                    markSavedOnlineOptionsAsBlacktffittingAutoProcent1_Offline();
                    setPointAtGraphictffittingAutoProcent1();
                    _graphicPlotting.CalculateCurrKandN();
                    if (chbShowOriginalData.IsChecked == true)
                    {
                        OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                    }
                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.DrawFittingGraphic();
                    }
                    _graphicPlotting.chbFititngManualMode.IsChecked = false;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        //ovo treba da sluzi samo kao patch
                        _graphicPlotting.DrawFittingGraphic();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tffittingAutoProcent1_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region fittingAutoPoint2

        //public void setTextMarkert2()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(_graphicPlotting.XMarkersText2);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(_graphicPlotting.YMarkersText2);
        //    //_graphicPlotting.MarkerGraphText2.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    _graphicPlotting.XMarkersText2[0] = _graphicPlotting.XMarkers2[0] + OptionsInPlottingMode.xRange / 120;
        //    _graphicPlotting.YMarkersText2[0] = _graphicPlotting.YMarkers2[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "T2";
        //    //_graphicPlotting.MarkerGraphText2.Marker = mkrText;
        //}

        public void saveOptionstffittingAutoProcent2_Offline()
        {
            try
            {
                double autoProcent2;
                string strautoProcent2 = tffittingAutoProcent2.Text.Replace(',', '.');
                bool isN = Double.TryParse(strautoProcent2, out autoProcent2);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje procenta koordinate druge tačke automatskog fitovanja!");
                }
                else if (autoProcent2 < 0)
                {
                    System.Windows.Forms.MessageBox.Show("Broj unet u polje procenta koordinate druge tačke automatskog fitovanja ne sme biti negativan!");
                    OptionsInPlottingMode.procentAuto2 = 0;
                }
                else
                {
                    OptionsInPlottingMode.procentAuto2 = autoProcent2;
                }

                double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto2 / 100;

                int index = _graphicPlotting.GetClosestPointIndex(criteriaPreassure);
                OptionsInPlottingMode.pointAutoX2 = _graphicPlotting.DataReader.RelativeElongation[index];
                OptionsInPlottingMode.pointAutoY2 = _graphicPlotting.DataReader.PreassureInMPa[index];

                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void saveOptionstffittingAutoProcent2_Offline()}", System.DateTime.Now);
            }

        }

        public void setPointAtGraphictffittingAutoProcent2()
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(_graphicPlotting.XMarkers2);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(_graphicPlotting.YMarkers2);
                _graphicPlotting.MarkerGraph2.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX1);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY1);

                _graphicPlotting.XMarkers2[0] = OptionsInPlottingMode.pointAutoX2;
                _graphicPlotting.YMarkers2[0] = OptionsInPlottingMode.pointAutoY2;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Yellow);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _graphicPlotting.MarkerGraph2.Marker = mkr;

                //setTextMarkert2();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void setPointAtGraphictffittingAutoProcent2()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingAutoProcent2_Offline()
        {
            try
            {
                //tffittingAutoProcent2.Foreground = Brushes.Black;
                tffittingAutoProcent2.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingAutoProcent2_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingAutoProcent2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingAutoProcent2.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tffittingAutoProcent2_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tffittingAutoProcent2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.SetManualPointsToAutoPointsValue();
                    saveOptionstffittingAutoProcent2_Offline();
                    markSavedOnlineOptionsAsBlacktffittingAutoProcent2_Offline();
                    setPointAtGraphictffittingAutoProcent2();

                    _graphicPlotting.CalculateCurrKandN();
                    if (chbShowOriginalData.IsChecked == true)
                    {
                        OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                    }
                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.DrawFittingGraphic();
                    }
                    _graphicPlotting.chbFititngManualMode.IsChecked = false;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        //ovo treba da sluzi samo kao patch
                        _graphicPlotting.DrawFittingGraphic();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tffittingAutoProcent2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion

        #region fittingAutoPoint3

        //public void setTextMarkert3()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(_graphicPlotting.XMarkersText3);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(_graphicPlotting.YMarkersText3);
        //    //_graphicPlotting.MarkerGraphText3.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    _graphicPlotting.XMarkersText3[0] = _graphicPlotting.XMarkers3[0] + OptionsInPlottingMode.xRange/120;
        //    _graphicPlotting.YMarkersText3[0] = _graphicPlotting.YMarkers3[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "T3";
        //    //_graphicPlotting.MarkerGraphText3.Marker = mkrText;
        //}


        public void saveOptionstffittingAutoProcent3_Offline()
        {
            try
            {
                double autoProcent3;
                string strautoProcent3 = tffittingAutoProcent3.Text.Replace(',', '.');
                bool isN = Double.TryParse(strautoProcent3, out autoProcent3);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje procenta koordinate treće tačke automatskog fitovanja!");
                }
                else if (autoProcent3 < 0)
                {
                    System.Windows.Forms.MessageBox.Show("Broj unet u polje procenta koordinate treće tačke automatskog fitovanja ne sme biti negativan!");
                    OptionsInPlottingMode.procentAuto3 = 0;
                }
                else
                {
                    OptionsInPlottingMode.procentAuto3 = autoProcent3;
                }

                double criteriaPreassure = OptionsInPlottingMode.pointCrossheadY * OptionsInPlottingMode.procentAuto3 / 100;

                int index = _graphicPlotting.GetClosestPointIndex(criteriaPreassure);
                OptionsInPlottingMode.pointAutoX3 = _graphicPlotting.DataReader.RelativeElongation[index];
                OptionsInPlottingMode.pointAutoY3 = _graphicPlotting.DataReader.PreassureInMPa[index];

                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void saveOptionstffittingAutoProcent3_Offline()}", System.DateTime.Now);
            }
        }

        public void setPointAtGraphictffittingAutoProcent3()
        {
            try
            {
                EnumerableDataSource<double> gX = new EnumerableDataSource<double>(_graphicPlotting.XMarkers3);
                EnumerableDataSource<double> gY = new EnumerableDataSource<double>(_graphicPlotting.YMarkers3);
                _graphicPlotting.MarkerGraph3.DataSource = new CompositeDataSource(gX, gY);

                //no scaling - identity mapping
                gX.XMapping = xx => xx;
                gY.YMapping = yy => yy;

                //xMarkers.Clear();
                //yMarkers.Clear();

                //xMarkers.Add(pfX1);
                //yMarkers.Add(OptionsInPlottingMode.pointManualY1);

                _graphicPlotting.XMarkers3[0] = OptionsInPlottingMode.pointAutoX3;
                _graphicPlotting.YMarkers3[0] = OptionsInPlottingMode.pointAutoY3;

                CirclePointMarker mkr = new CirclePointMarker();
                mkr.Fill = new SolidColorBrush(Colors.Red);
                mkr.Size = Constants.MARKERSIZE;
                mkr.Pen = new Pen(new SolidColorBrush(Colors.Black), 1);
                _graphicPlotting.MarkerGraph3.Marker = mkr;

                //setTextMarkert3();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void setPointAtGraphictffittingAutoProcent3()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktffittingAutoProcent3_Offline()
        {
            try
            {
                //tffittingAutoProcent3.Foreground = Brushes.Black;
                tffittingAutoProcent3.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktffittingAutoProcent3_Offline()}", System.DateTime.Now);
            }
        }

        private void tffittingAutoProcent3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tffittingAutoProcent3.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tffittingAutoProcent3_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        public void tabforT3() 
        {
            try
            {
                _graphicPlotting.SetManualPointsToAutoPointsValue();
                saveOptionstffittingAutoProcent3_Offline();
                markSavedOnlineOptionsAsBlacktffittingAutoProcent3_Offline();
                setPointAtGraphictffittingAutoProcent3();

                _graphicPlotting.CalculateCurrKandN();
                if (chbShowOriginalData.IsChecked == true)
                {
                    OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                }
                _graphicPlotting.DrawFittingGraphic();
                _graphicPlotting.chbFititngManualMode.IsChecked = false;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void tabforT3()}", System.DateTime.Now);
            }
        }

        private void tffittingAutoProcent3_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.SetManualPointsToAutoPointsValue();
                    saveOptionstffittingAutoProcent3_Offline();
                    markSavedOnlineOptionsAsBlacktffittingAutoProcent3_Offline();
                    setPointAtGraphictffittingAutoProcent3();

                    _graphicPlotting.CalculateCurrKandN();
                    if (chbShowOriginalData.IsChecked == true)
                    {
                        OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                    }
                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.DrawFittingGraphic();
                    }
                    _graphicPlotting.chbFititngManualMode.IsChecked = false;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        //ovo treba da sluzi samo kao patch
                        _graphicPlotting.DrawFittingGraphic();
                    }

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tffittingAutoProcent3_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }



        #endregion

        #region fittingAutoMaxXValue

        public void saveOptionstfFittingAutoMaxXValue_Offline()
        {
            try
            {
                double maxXValue;
                string strmaxXValue = tfFittingAutoMaxXValue.Text.Replace(',', '.');
                bool isN = Double.TryParse(strmaxXValue, out maxXValue);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Max dozvoljena vr. po x-u!");
                }
                else if (maxXValue < 0)
                {
                    System.Windows.Forms.MessageBox.Show("Broj unet u polje Max dozvoljena vr. po x-u ne sme biti negativan!");
                    OptionsInPlottingMode.fittingAutoMaxXValue = 0;
                }
                else
                {
                    OptionsInPlottingMode.fittingAutoMaxXValue = maxXValue;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void saveOptionstfFittingAutoMaxXValue_Offline()}", System.DateTime.Now);
            }

        }

        private void markSavedOnlineOptionsAsBlacktfFittingAutoMaxXValue_Offline()
        {
            try
            {
                //tfFittingAutoMaxXValue.Foreground = Brushes.Black;
                tfFittingAutoMaxXValue.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfFittingAutoMaxXValue_Offline()}", System.DateTime.Now);
            }
        }

        private void tfFittingAutoMaxXValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfFittingAutoMaxXValue.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfFittingAutoMaxXValue_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfFittingAutoMaxXValue_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfFittingAutoMaxXValue_Offline();
                    markSavedOnlineOptionsAsBlacktfFittingAutoMaxXValue_Offline();

                    //_graphicPlotting.CalculateCurrKandN();
                    //if (chbShowOriginalData.IsChecked == true)
                    //{
                    //    OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                    //}
                    //_graphicPlotting.DrawFittingGraphic();
                    //_graphicPlotting.chbFititngManualMode.IsChecked = false;
                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfFittingAutoMaxXValue_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region R2R4Caption

        //public void setTextMarkert9()
        //{
        //    EnumerableDataSource<double> gXT = new EnumerableDataSource<double>(_graphicPlotting.XMarkersText9);
        //    EnumerableDataSource<double> gYT = new EnumerableDataSource<double>(_graphicPlotting.YMarkersText9);
        //    _graphicPlotting.MarkerGraphText9.DataSource = new CompositeDataSource(gXT, gYT);

        //    //no scaling - identity mapping
        //    gXT.XMapping = xx => xx;
        //    gYT.YMapping = yy => yy;

        //    _graphicPlotting.XMarkersText9[0] = _graphicPlotting.XMarkers9[0] + _graphicPlotting.XMarkers9[0] / 120;
        //    _graphicPlotting.YMarkersText9[0] = _graphicPlotting.YMarkers9[0];

        //    CenteredTextMarker mkrText = new CenteredTextMarker();
        //    mkrText.Text = "R2/R4";
        //    _graphicPlotting.MarkerGraphText9.Marker = mkrText;
        //}


        #endregion

        private void chbShowOriginalData_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void chbShowOriginalData_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void chbShowOriginalData_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void chbShowOriginalData_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

        public void OddOriginalClick() 
        {
            try
            {
                OptionsInPlottingMode.isShowOriginalDataGraphic = true;
                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void OddOriginalClick()}", System.DateTime.Now);
            }
        }

        public void EvenOriginalClick()
        {
            try
            {
                OptionsInPlottingMode.isShowOriginalDataGraphic = false;
                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {public void EvenOriginalClick()}", System.DateTime.Now);
            }
        }



        /// <summary>
        /// interval za odredjivanje ReH
        /// </summary>
        #region yield



        private void saveOptionstfYield_Offline()
        {
            try
            {
                double yield;
                string stryield = tfYield.Text.Replace(',', '.');
                bool isN = Double.TryParse(stryield, out yield);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Min pad u [MPa] za [ReH]!");
                }
                else
                {
                    OptionsInPlottingMode.YieldInterval = yield;
                }


                _graphicPlotting.WriteXMLFileOffline();
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfYield_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOptionsAsBlacktfYield_Offline()
        {
            try
            {
                //tfYield.Foreground = Brushes.Black;
                tfYield.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfYield_Offline()}", System.DateTime.Now);
            }
        }


        private void tfYield_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfYield.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfYield_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfYield_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.MarkerGraph5.DataSource = null;
                    //_graphicPlotting.MarkerGraphText5.DataSource = null;
                    _graphicPlotting.MarkerGraph6.DataSource = null;
                    //_graphicPlotting.MarkerGraphText6.DataSource = null;
                    saveOptionstfYield_Offline();
                    markSavedOptionsAsBlacktfYield_Offline();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                        _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);

                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfYield_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion


        #region MaxSubBetweenReLFAndReL

        private void saveOptionstfMaxSubsReLFReL_Offline()
        {
            try
            {
                double maxSubsReLFReL;
                string strmaxSubsReLFReL = tfMaxSubsReLFReL.Text.Replace(',', '.');
                bool isN = Double.TryParse(strmaxSubsReLFReL, out maxSubsReLFReL);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Max razlika u [MPa] [ReLF] i [ReL]!");
                }
                else
                {
                    OptionsInPlottingMode.MaxSubBetweenReLAndReLF = maxSubsReLFReL;
                }


                _graphicPlotting.WriteXMLFileOffline();
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfMaxSubsReLFReL_Offline()}", System.DateTime.Now);
            }
        }


        private void markSavedOptionsAsBlacktfMaxSubsReLFReL_Offline()
        {
            try
            {
                //tfMaxSubsReLFReL.Foreground = Brushes.Black;
                tfMaxSubsReLFReL.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfMaxSubsReLFReL_Offline()}", System.DateTime.Now);
            }
        }


        private void tfMaxSubsReLFReL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfMaxSubsReLFReL.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfMaxSubsReLFReL_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfMaxSubsReLFReL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.MarkerGraph5.DataSource = null;
                    //_graphicPlotting.MarkerGraphText5.DataSource = null;
                    _graphicPlotting.MarkerGraph6.DataSource = null;
                    //_graphicPlotting.MarkerGraphText6.DataSource = null;
                    saveOptionstfMaxSubsReLFReL_Offline();
                    markSavedOptionsAsBlacktfMaxSubsReLFReL_Offline();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                        _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);

                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfMaxSubsReLFReL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region tfReHXRange

        private void saveOptionstfReHXRange_Offline()
        {
            try
            {
                double rationtfReHXRange;
                string strrationtfReHXRange = tfReHXRange.Text.Replace(',', '.');
                bool isN = Double.TryParse(strrationtfReHXRange, out rationtfReHXRange);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Oblast trazenja ReH-a!");
                }
                else
                {
                    OptionsInPlottingMode.ReHXRange = rationtfReHXRange;
                }


                _graphicPlotting.WriteXMLFileOffline();
                if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                {
                    _graphicPlotting.DrawFittingGraphic();
                }
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfReHXRange_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOptionsAsBlacktfReHXRange_Offline()
        {
            try
            {
                //tfReHXRange.Foreground = Brushes.Black;
                tfReHXRange.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfReHXRange_Offline()}", System.DateTime.Now);
            }
        }

        private void tfReHXRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfReHXRange.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfReHXRange_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfReHXRange_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfReHXRange_Offline();
                    markSavedOptionsAsBlacktfReHXRange_Offline();


                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                        _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);

                        _graphicPlotting.btnPlottingModeClick();
                    }

                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfReHXRange_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

        #region RationReLRpmaxForOnlyReL

        private void saveOptionstfRationReLRpmaxForOnlyReL_Offline()
        {
            try
            {
                double rationReLRpmaxForOnlyReL;
                string strrationReLRpmaxForOnlyReL = tfRationReLRpmaxForOnlyReL.Text.Replace(',', '.');
                bool isN = Double.TryParse(strrationReLRpmaxForOnlyReL, out rationReLRpmaxForOnlyReL);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje (Max napon za ReL) / (Rpmax) [%]!");
                }
                else
                {
                    OptionsInPlottingMode.RationReLRpmaxForOnlyReL = rationReLRpmaxForOnlyReL;
                }


                _graphicPlotting.WriteXMLFileOffline();
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfRationReLRpmaxForOnlyReL_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOptionsAsBlacktfRationReLRpmaxForOnlyReL_Offline()
        {
            try
            {
                //tfRationReLRpmaxForOnlyReL.Foreground = Brushes.Black;
                tfRationReLRpmaxForOnlyReL.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfRationReLRpmaxForOnlyReL_Offline()}", System.DateTime.Now);
            }
        }


        private void tfRationReLRpmaxForOnlyReL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfRationReLRpmaxForOnlyReL.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfRationReLRpmaxForOnlyReL_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfRationReLRpmaxForOnlyReL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfRationReLRpmaxForOnlyReL_Offline();
                    markSavedOptionsAsBlacktfRationReLRpmaxForOnlyReL_Offline();



                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                        _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfRationReLRpmaxForOnlyReL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion





        #region tfXReLForOnlyReL

        private void saveOptionstfXReLForOnlyReL_Offline()
        {
            try
            {
                double XReLForOnlyReL;
                string stXReLForOnlyReL = tfXReLForOnlyReL.Text.Replace(',', '.');
                bool isN = Double.TryParse(stXReLForOnlyReL, out XReLForOnlyReL);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("(Min AReL) [%]!");
                }
                else
                {
                    OptionsInPlottingMode.MinPossibleValueForOnlyReLElongationInProcent = XReLForOnlyReL;
                }


                _graphicPlotting.WriteXMLFileOffline();
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfXReLForOnlyReL_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOptionsAsBlacktfXReLForOnlyReL_Offline()
        {
            try
            {
                //tfRationXReLXmaxForOnlyReL.Foreground = Brushes.Black;
                tfXReLForOnlyReL.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfXReLForOnlyReL_Offline()}", System.DateTime.Now);
            }
        }

        private void tfXReLForOnlyReL_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfXReLForOnlyReL.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfXReLForOnlyReL_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfXReLForOnlyReL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfXReLForOnlyReL_Offline();
                    markSavedOptionsAsBlacktfXReLForOnlyReL_Offline();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                        _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);

                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfXReLForOnlyReL_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion


        #region tfOnlyReLReassureUnit

        private void saveOptionstfOnlyReLPreassureUnit_Offline()
        {
            try
            {
                double onlyReLPreassureUnit;
                string stronlyReLPreassureUnit = tfOnlyReLPreassureUnit.Text.Replace(',', '.');
                bool isN = Double.TryParse(stronlyReLPreassureUnit, out onlyReLPreassureUnit);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Jediničan napon za odredjivanje samo ReL-a nije unet u obliku broja!");
                }
                else
                {
                    OptionsInPlottingMode.OnlyReLPreassureUnit = onlyReLPreassureUnit;
                }


                _graphicPlotting.WriteXMLFileOffline();
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfOnlyReLPreassureUnit_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOptionsAsBlacktfOnlyReLPreassureUnit_Offline()
        {
            try
            {
                //tfOnlyReLPreassureUnit.Foreground = Brushes.Black;
                tfOnlyReLPreassureUnit.Foreground = Brushes.White;
            }catch(Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfOnlyReLPreassureUnit_Offline()}", System.DateTime.Now);
            }
        }

        private void tfOnlyReLPreassureUnit_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfOnlyReLPreassureUnit.Foreground = Brushes.Red;
            }catch(Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfOnlyReLPreassureUnit_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfOnlyReLPreassureUnit_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting._MarkerGraph5.DataSource = null;
                    //_graphicPlotting._MarkerGraphText5.DataSource = null;
                    saveOptionstfOnlyReLPreassureUnit_Offline();
                    markSavedOptionsAsBlacktfOnlyReLPreassureUnit_Offline();

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.SetRpmaxAndFmaxReHANDReL();
                        _graphicPlotting.TranslateReHRelAndRmtoFittingGraphic(_graphicPlotting.IndexOfPointClosestToRedProperty, _graphicPlotting.XTranslateAmountFittingMode);

                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }catch(Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfOnlyReLPreassureUnit_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion




        #region ResolutionForTearing

        private void saveOptionstfResolutionForTearing_Offline()
        {
            try
            {
                int resolutionForTearing;
                string strresolutionForTearing = tfResolutionForTearing.Text.Replace(',', '.');
                bool isN = Int32.TryParse(strresolutionForTearing, out resolutionForTearing);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje rezolucija pri kidanju!");
                }
                else
                {
                    OptionsInPlottingMode.ResolutionForTearing = resolutionForTearing;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfResolutionForTearing_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOptionsAsBlacktfResolutionForTearing_Offline()
        {
            try
            {
                //tfResolutionForTearing.Foreground = Brushes.Black;
                tfResolutionForTearing.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOptionsAsBlacktfResolutionForTearing_Offline()}", System.DateTime.Now);
            }
        }

        private void tfResolutionForTearing_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfResolutionForTearing.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfResolutionForTearing_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfResolutionForTearing_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {

                    saveOptionstfResolutionForTearing_Offline();
                    markSavedOptionsAsBlacktfResolutionForTearing_Offline();

                    _graphicPlotting.deleteTotalRelativeElongation_RedLine();
                    _graphicPlotting.lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.DrawFittingGraphic();
                    }
                    _graphicPlotting.IsClickedByMouse_Plotting_A = false;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfResolutionForTearing_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }


        #endregion


        #region TearingPointCoeff

        private void saveOptionstfTearingPointCoeff_Offline()
        {
            try
            {
                double tearingPointCoeff;
                string strtearingPointCoeff = tfTearingPointCoeff.Text.Replace(',', '.');
                bool isN = Double.TryParse(strtearingPointCoeff, out tearingPointCoeff);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Koeficijent kidanja!");
                }
                else
                {
                    if (tearingPointCoeff < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Koeficijent mora biti broj veći od nule!");
                        OptionsInPlottingMode.TearingPointCoeff = Double.MaxValue;
                    }
                    OptionsInPlottingMode.TearingPointCoeff = tearingPointCoeff;
                }


                _graphicPlotting.WriteXMLFileOffline();
                //createOfflineGraphics();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfTearingPointCoeff_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfTearingPointCoeff_Offline()
        {
            try
            {
                //tfTearingPointCoeff.Foreground = Brushes.Black;
                tfTearingPointCoeff.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfTearingPointCoeff_Offline()}", System.DateTime.Now);
            }
        }

        private void tfTearingPointCoeff_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfTearingPointCoeff.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfTearingPointCoeff_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfTearingPointCoeff_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTearingPointCoeff_Offline();
                    markSavedOnlineOptionsAsBlacktfTearingPointCoeff_Offline();

                    _graphicPlotting.deleteTotalRelativeElongation_RedLine();
                    _graphicPlotting.lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.DrawFittingGraphic();
                    }
                    _graphicPlotting.IsClickedByMouse_Plotting_A = false;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfTearingPointCoeff_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion



        #region TearingMinFallPreassure

        private void saveOptionstfTearingMinFallPreassure_Offline()
        {
            try
            {
                double tearingMinFallPreassure;
                string strtearingMinFallPreassure = tfTearingMinFallPreassure.Text.Replace(',', '.');
                bool isN = Double.TryParse(strtearingMinFallPreassure, out tearingMinFallPreassure);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Min pad napona kod kidanja!");
                }
                else
                {
                    if (tearingMinFallPreassure < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Min pad napona kod kidanja mora biti broj veći od nule!");
                        OptionsInPlottingMode.TearingMinFallPreassure = Double.MaxValue;
                    }
                    OptionsInPlottingMode.TearingMinFallPreassure = tearingMinFallPreassure;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfTearingMinFallPreassure_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfTearingMinFallPreassure_Offline()
        {
            try
            {
                //tfTearingMinFallPreassure.Foreground = Brushes.Black;
                tfTearingMinFallPreassure.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfTearingMinFallPreassure_Offline()}", System.DateTime.Now);
            }
        }


        private void tfTearingMinFallPreassure_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfTearingMinFallPreassure.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfTearingMinFallPreassure_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfTearingMinFallPreassure_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfTearingMinFallPreassure_Offline();
                    markSavedOnlineOptionsAsBlacktfTearingMinFallPreassure_Offline();

                    _graphicPlotting.deleteTotalRelativeElongation_RedLine();
                    _graphicPlotting.lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.DrawFittingGraphic();


                        _graphicPlotting.btnPlottingModeClick();
                    }

                    _graphicPlotting.IsClickedByMouse_Plotting_A = false;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfTearingMinFallPreassure_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


        private void chbNepodrazumevaniModkidanja_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja == true)
                {
                    return;
                }

                OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja = true;
                //tfResolutionForTearing.IsReadOnly = true;
                //tfTearingPointCoeff.IsReadOnly = true;
                ////tfTearingMinFallPreassure.IsReadOnly = true;
                //tfDefaultPreassureOfTearing.IsReadOnly = false;
                tfResolutionForTearing.IsReadOnly = false;
                tfTearingPointCoeff.IsReadOnly = false;
                //tfTearingMinFallPreassure.IsReadOnly = true;
                tfDefaultPreassureOfTearing.IsReadOnly = true;

                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.deleteTotalRelativeElongation_RedLine();
                _graphicPlotting.lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void chbNepodrazumevaniModkidanja_Checked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }

        }

        private void chbNepodrazumevaniModkidanja_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja == false)
                {
                    return;
                }

                OptionsInPlottingMode.UkljuciNepodrazumevaniModKidanja = false;
                //tfResolutionForTearing.IsReadOnly = false;
                //tfTearingPointCoeff.IsReadOnly = false;
                ////tfTearingMinFallPreassure.IsReadOnly = false;
                //tfDefaultPreassureOfTearing.IsReadOnly = true;
                tfResolutionForTearing.IsReadOnly = true;
                tfTearingPointCoeff.IsReadOnly = true;
                //tfTearingMinFallPreassure.IsReadOnly = false;
                tfDefaultPreassureOfTearing.IsReadOnly = false;

                _graphicPlotting.WriteXMLFileOffline();
                _graphicPlotting.deleteTotalRelativeElongation_RedLine();
                _graphicPlotting.lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void chbNepodrazumevaniModkidanja_Unchecked(object sender, RoutedEventArgs e)}", System.DateTime.Now);
            }
        }

       


        #region DefaultPreassureOfTearing

        private void saveOptionstfDefaultPreassureOfTearing_Offline()
        {
            try
            {
                double defaultPreassureOfTearing;
                string strdefaultPreassureOfTearing = tfDefaultPreassureOfTearing.Text.Replace(',', '.');
                bool isN = Double.TryParse(strdefaultPreassureOfTearing, out defaultPreassureOfTearing);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Podrazumevani napon kidanja u odnosu na Rpmax!");
                }
                else
                {
                    if (defaultPreassureOfTearing < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Podrazumevani napon kidanja u odnosu na Rpmax biti broj veći od nule!");
                        OptionsInPlottingMode.DefaultPreassureOfTearingInProcent = Double.MaxValue;
                    }
                    OptionsInPlottingMode.DefaultPreassureOfTearingInProcent = defaultPreassureOfTearing;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfDefaultPreassureOfTearing_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedOnlineOptionsAsBlacktfDefaultPreassureOfTearing_Offline()
        {
            try
            {
                //tfDefaultPreassureOfTearing.Foreground = Brushes.Black;
                tfDefaultPreassureOfTearing.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedOnlineOptionsAsBlacktfDefaultPreassureOfTearing_Offline()}", System.DateTime.Now);
            }
        }

        private void tfDefaultPreassureOfTearing_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfDefaultPreassureOfTearing.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfDefaultPreassureOfTearing_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfDefaultPreassureOfTearing_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    saveOptionstfDefaultPreassureOfTearing_Offline();
                    markSavedOnlineOptionsAsBlacktfDefaultPreassureOfTearing_Offline();

                    _graphicPlotting.deleteTotalRelativeElongation_RedLine();
                    _graphicPlotting.lblCalculateTotalRelElong.Text = Constants.NOTCALCULATETOTALRELATIVEELONGATION;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        //double k = (_graphicPlotting.PointsOfFittingLine[_graphicPlotting.PointsOfFittingLine.Count - 2].YAxisValue - _graphicPlotting.PointsOfFittingLine[_graphicPlotting.PointsOfFittingLine.Count - 1].YAxisValue) / (_graphicPlotting.PointsOfFittingLine[_graphicPlotting.PointsOfFittingLine.Count - 2].XAxisValue - _graphicPlotting.PointsOfFittingLine[_graphicPlotting.PointsOfFittingLine.Count - 1].XAxisValue);
                        double procent = 0;
                        bool isN = double.TryParse(tfDefaultPreassureOfTearing.Text, out procent);
                        if (isN == true)
                        {


                            double xx = 0;
                            double yy = 0;
                            List<string> datas;
                            string path = _graphicPlotting.tfFilepathPlotting.Text.Split('.').ElementAt(0);
                            path += ".pe";
                            List<double> preassure = new List<double>();
                            List<double> elongation = new List<double>();
                            if (_graphicPlotting.OnlineModeInstance.dataReader.PreassureInMPa.Count == 0)
                            {
                                datas = File.ReadAllLines(path).ToList();
                                foreach (var data in datas)
                                {
                                    List<string> pe = data.Split('\t').ToList();
                                    double pr = 0;
                                    double el = 0;
                                    bool isNN = double.TryParse(pe[0], out pr);
                                    if (isNN)
                                    {
                                        preassure.Add(pr);
                                    }
                                    isNN = double.TryParse(pe[1], out el);
                                    if (isNN)
                                    {
                                        elongation.Add(el);
                                    }
                                }
                            }

                            //nadji najblizu tacku na zelenom grafiku 
                            for (int i = preassure.Count - 1; i >= 0; i--)
                            {
                                if (preassure[i] >= 0.01 * procent * _graphicPlotting.Rm)
                                {
                                    yy = preassure[i];
                                    xx = elongation[i];
                                    //xx = xx - _graphicPlotting.XTranslateAmountFittingMode;
                                    break;
                                }
                            }

                            double t1x = 0;
                            bool isNNN = double.TryParse(_graphicPlotting.tffittingManPoint1X.Text, out t1x);
                            double t1y = 0;
                            isNNN = double.TryParse(_graphicPlotting.tffittingManPoint1Y.Text, out t1y);
                            double t2x = 0;
                            isNNN = double.TryParse(_graphicPlotting.tffittingManPoint2X.Text, out t2x);
                            double t2y = 0;
                            isNNN = double.TryParse(_graphicPlotting.tffittingManPoint2Y.Text, out t2y);
                            double currKGlobal = (t2y - t1y) / (t2x - t1x);

                            double AX = (-1) * (yy - currKGlobal * xx) / currKGlobal;
                            AX = Math.Round(AX, 2);
                            _graphicPlotting.AManualClickedValue = AX;
                            _graphicPlotting.tfA.Text = _graphicPlotting.AManualClickedValue.ToString();
                            _graphicPlotting.A = _graphicPlotting.AManualClickedValue;

                            //why this don't work without arguments i don't know
                            //but this works fine with arguments
                            _graphicPlotting.DrawFittingGraphic(/*true, 0, true, xx, yy*/);
                        }
                        else
                        {
                            _graphicPlotting.DrawFittingGraphic();
                        }
                    }
                    _graphicPlotting.IsClickedByMouse_Plotting_A = false;
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfDefaultPreassureOfTearing_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

     



        #region tfBeginIntervalForN

        private void saveOptionstfBeginIntervalForN_Offline()
        {
            try
            {
                double beginIntervalForN;
                string strbeginIntervalForN = tfBeginIntervalForN.Text.Replace(',', '.');
                bool isN = Double.TryParse(strbeginIntervalForN, out beginIntervalForN);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Početak opsega [za računanje opsega n-a]!");
                }
                else
                {
                    if (beginIntervalForN < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Podrazumevana početna granica za račuanje n-a treba biti veća od nule!");
                        OptionsInPlottingMode.BeginIntervalForN = 3;
                    }
                    OptionsInPlottingMode.BeginIntervalForN = beginIntervalForN;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfBeginIntervalForN_Offline()}", System.DateTime.Now);
            }
        }


        private void markSavedAsBlacktfBeginIntervalForN_Offline()
        {
            try
            {
                //tfBeginIntervalForN.Foreground = Brushes.Black;
                tfBeginIntervalForN.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedAsBlacktfBeginIntervalForN_Offline()}", System.DateTime.Now);
            }
        }


        private void tfBeginIntervalForN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfBeginIntervalForN.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfBeginIntervalForN_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfBeginIntervalForN_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    if (_graphicPlotting.PreassureForNManualProperty == null)
                    {
                        return;
                    }

                    saveOptionstfBeginIntervalForN_Offline();
                    markSavedAsBlacktfBeginIntervalForN_Offline();
                    _graphicPlotting.PreassureForNManualProperty.Clear();

                    _graphicPlotting.ReCalculateAutoAndManualN();

                    //posle promene granica odcekiraj rucno n
                    _graphicPlotting.Printscreen.chbCalculateNManual.IsChecked = false;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfBeginIntervalForN_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

      


        #endregion

       

        #region tfEndIntervalForN

        private void saveOptionstfEndIntervalForN_Offline()
        {
            try
            {
                double endIntervalForN;
                string strendIntervalForN = tfEndIntervalForN.Text.Replace(',', '.');
                bool isN = Double.TryParse(strendIntervalForN, out endIntervalForN);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Kraj opsega [za računanje opsega n-a]!");
                }
                else
                {
                    if (endIntervalForN < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Podrazumevana krajna granica za račuanje n-a treba biti veća od nule!");
                        OptionsInPlottingMode.EndIntervalForN = 19;
                    }
                    OptionsInPlottingMode.EndIntervalForN = endIntervalForN;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void saveOptionstfEndIntervalForN_Offline()}", System.DateTime.Now);
            }
        }

        private void markSavedAsBlacktfEndIntervalForN_Offline()
        {
            try
            {
                //tfEndIntervalForN.Foreground = Brushes.Black;
                tfEndIntervalForN.Foreground = Brushes.White;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void markSavedAsBlacktfEndIntervalForN_Offline()}", System.DateTime.Now);
            }
        }


        private void tfEndIntervalForN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfEndIntervalForN.Foreground = Brushes.Red;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfEndIntervalForN_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfEndIntervalForN_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    if (_graphicPlotting.PreassureForNManualProperty == null)
                    {
                        return;
                    }

                    saveOptionstfEndIntervalForN_Offline();
                    markSavedAsBlacktfEndIntervalForN_Offline();
                    _graphicPlotting.PreassureForNManualProperty.Clear();

                    _graphicPlotting.ReCalculateAutoAndManualN();

                    //posle promene granica odcekiraj rucno n
                    _graphicPlotting.Printscreen.chbCalculateNManual.IsChecked = false;

                    if (_graphicPlotting.Printscreen.chbPrikaziOpcije.IsChecked == false)
                    {
                        _graphicPlotting.btnPlottingModeClick();
                    }
                }
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsPlotting.xaml.cs] {private void tfEndIntervalForN_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion


       

    }
}
