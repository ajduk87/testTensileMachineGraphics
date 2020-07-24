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

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for OptionsForYungsModuo.xaml
    /// </summary>
    public partial class OptionsForYungsModuo : Window
    {
        private GraphicPlotting _graphicPlotting;
        public GraphicPlotting Plotting 
        {
            get { return _graphicPlotting; }
            set 
            {
                if (value != null)
                {
                    _graphicPlotting = value;
                }
            }
        }

        public OptionsForYungsModuo()
        {
            try
            {
                InitializeComponent();

                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {public OptionsForYungsModuo()}", System.DateTime.Now);
            }
           
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                _graphicPlotting.IsOptionsForYungModuoOpen = false;
                this.Hide();
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)}", System.DateTime.Now);
            }
        }

        


        #region tfXelas

        public void SaveOptionstfXelas_Offline()
        {
            try
            {
                saveOptionstfXelas_Offline();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {public void SaveOptionstfXelas_Offline()}", System.DateTime.Now);
            }
        }

        private void saveOptionstfXelas_Offline()
        {
            try
            {
                double yungXelas;
                string stryungXelas = tfXelas.Text.Replace(',', '.');
                bool isN = Double.TryParse(stryungXelas, out yungXelas);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Xelas!");
                }
                else
                {
                    if (yungXelas < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Vrednost Xelas treba biti veća od nule!");
                        OptionsInPlottingMode.ReEqualsRp = 0.05;
                    }
                    OptionsInPlottingMode.ReEqualsRp = yungXelas;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {public void SaveOptionstfXelas_Offline()}", System.DateTime.Now);
            }
        }


        private void markSavedAsBlacktfXelas_Offline()
        {
            try
            {
                //tfXelas.Foreground = Brushes.Black;
                tfXelas.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void markSavedAsBlacktfXelas_Offline()}", System.DateTime.Now);
            }
        }


        private void tfXelas_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfXelas.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void tfXelas_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfXelas_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    _graphicPlotting.IsYungFirstTimeCalculate = false;

                    saveOptionstfXelas_Offline();
                    markSavedAsBlacktfXelas_Offline();

                    //ovo definisi u plotting delu
                    //_graphicPlotting.RecalculateYungsModuo(true);

                    if (_graphicPlotting.OnlineModeInstance.OnHeader != null)
                    {
                        if (_graphicPlotting.OnlineModeInstance.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                        {
                            _graphicPlotting.RecalculateYungsModuo(true, true);
                        }
                        else
                        {
                            _graphicPlotting.RecalculateYungsModuo(true);
                        }
                    }
                    else
                    {
                        _graphicPlotting.RecalculateYungsModuo(true);
                    }
                    _graphicPlotting.DrawFittingGraphic();
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void tfXelas_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion

      


        #region tfprocspustanja

        private void saveOptionstfprocspustanja_Offline()
        {
            try
            {
                double procspustanja;
                string strprocspustanja = tfprocspustanja.Text.Replace(',', '.');
                bool isN = Double.TryParse(strprocspustanja, out procspustanja);

                if (isN == false)
                {
                    System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje Pr. spustanja!");
                }
                else
                {
                    if (procspustanja < 0)
                    {
                        System.Windows.Forms.MessageBox.Show("Vrednost polja Pr. spustanja treba biti veća od nule!");
                        OptionsInPlottingMode.YungPrSpustanja = 0.05;
                    }
                    OptionsInPlottingMode.YungPrSpustanja = procspustanja;
                }


                _graphicPlotting.WriteXMLFileOffline();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void saveOptionstfprocspustanja_Offline()}", System.DateTime.Now);
            }
        }


        private void markSavedAsBlacktfprocspustanja_Offline()
        {
            try
            {
                //tfprocspustanja.Foreground = Brushes.Black;
                tfprocspustanja.Foreground = Brushes.White;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void markSavedAsBlacktfprocspustanja_Offline()}", System.DateTime.Now);
            }
        }

        private void tfprocspustanja_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tfprocspustanja.Foreground = Brushes.Red;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void tfprocspustanja_TextChanged(object sender, TextChangedEventArgs e)}", System.DateTime.Now);
            }
        }

        private void tfprocspustanja_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {

                    saveOptionstfprocspustanja_Offline();
                    markSavedAsBlacktfprocspustanja_Offline();

                    //ovo definisi u plotting delu
                    if (_graphicPlotting.OnlineModeInstance.OnHeader != null)
                    {
                        if (_graphicPlotting.OnlineModeInstance.OnHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
                        {
                            _graphicPlotting.RecalculateYungsModuo(false, true);
                        }
                        else
                        {
                            _graphicPlotting.RecalculateYungsModuo();
                        }
                    }
                    else
                    {
                        _graphicPlotting.RecalculateYungsModuo();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[OptionsForYungModuo.xaml.cs] {private void tfprocspustanja_KeyDown(object sender, KeyEventArgs e)}", System.DateTime.Now);
            }
        }

        #endregion
    }
}
