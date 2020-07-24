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

namespace testTensileMachineGraphics.Options
{
    /// <summary>
    /// Interaction logic for OptionsAnimation.xaml
    /// </summary>
    public partial class OptionsAnimation : Window
    {

        public static bool isCreatedOptionsAnimation = false;

        private GraphicAnimation _animation;

        public GraphicAnimation Animation 
        {
            get { return _animation; }
            set 
            {
                if(value != null)
                {
                    _animation = value;
                }
            }
        }

        

        public OptionsAnimation()
        {
            InitializeComponent();
        }


        private void btnChooseDatabasePath_Click(object sender, RoutedEventArgs e)
        {
            string filePath = String.Empty;
            string extensionTxt = "txt";
            bool _okDatabasePath = false;
            OpenFileDialog openDlg = new OpenFileDialog();

            // Show open file dialog box 
            DialogResult result = openDlg.ShowDialog();

            // Process open file dialog box results 
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                filePath = openDlg.FileName;
                string ext = System.IO.Path.GetExtension(openDlg.FileName);
                string check = ext.Substring(1, ext.Length - 1);

                if (extensionTxt.Equals(check))
                {
                    _okDatabasePath = true;
                }
                else
                {
                    _okDatabasePath = false;
                }

                if (_okDatabasePath == false)
                {
                    filePath = String.Empty;
                    System.Windows.Forms.MessageBox.Show("Izabrani fajl " + filePath + " nije tekstualni fajl! Molimo vas učitajte fajl sa ispravnom ekstenzijom!", "POKUŠAJ UČITAVANJA NEISPRAVNOG FORMATA TEKSTUALNOG FAJLA");
                    return;
                }
            }

            tfFilepathPlotting.Text = filePath;
        }

        private void btnSaveAnimationOptions_Click(object sender, RoutedEventArgs e)
        {
                
            if (rbtnContinuous.IsChecked == true)
            {
                OptionsInAnimation.isContinuousDisplay = true;
            }
            else
            {
                OptionsInAnimation.isContinuousDisplay = false;
            }


            if (rbtnDiscrete.IsChecked == true)
            {
                OptionsInAnimation.isDiscreteDisplay = true;
            }
            else
            {
                OptionsInAnimation.isDiscreteDisplay = false;
            }



            //if (cmbRefresh.SelectedIndex == 0)
            //{
            //    OptionsInAnimation.refreshTimeInterval = 10;
            //    _animation.UpdateCollectionTimer.Interval = TimeSpan.FromMilliseconds(OptionsInAnimation.refreshTimeInterval);
            //    _animation.counterWhoDetermitedOneSecond = 100;
            //}
            //if (cmbRefresh.SelectedIndex == 1)
            //{
            //    OptionsInAnimation.refreshTimeInterval = 16.6;
            //    _animation.UpdateCollectionTimer.Interval = TimeSpan.FromMilliseconds(OptionsInAnimation.refreshTimeInterval);
            //    _animation.counterWhoDetermitedOneSecond = 60;
            //}
            //if (cmbRefresh.SelectedIndex == 2)
            //{
            //    OptionsInAnimation.refreshTimeInterval = 40;
            //    _animation.UpdateCollectionTimer.Interval = TimeSpan.FromMilliseconds(OptionsInAnimation.refreshTimeInterval);
            //    _animation.counterWhoDetermitedOneSecond = 25;
            //}
          
            int resolution;
            bool isN = Int32.TryParse(tfResolutionCon.Text, out resolution);

            if (isN == false)
            {
                System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje odredjeno za rezoluciju kontinualnog prikaza !");
            }
            else
            {
                OptionsInAnimation.conResolution = resolution;

            }

            int resolutiondis;
            isN = Int32.TryParse(tfResolutionDis.Text, out resolutiondis);

            if (isN == false)
            {
                System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje odredjeno za rezoluciju diskretnog prikaza !");
            }
            else
            {
                OptionsInAnimation.disResolution = resolutiondis;

            }

           

            double forceDiv;
            string strforceDiv = tfCalForceDivide.Text.Replace(',', '.');
            isN = Double.TryParse(strforceDiv, out forceDiv);

            if (isN == false)
            {
                System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje sile!");
            }
            else
            {
                OptionsInAnimation.nutnDivide = forceDiv;
            }


            double forceMul;
            string strforceMul = tfCalForceMultiple.Text.Replace(',', '.');
            isN = Double.TryParse(strforceMul, out forceMul);

            if (isN == false)
            {
                System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje sile!");
            }
            else
            {
                OptionsInAnimation.nutnMultiple = forceMul;
            }



            double elonDiv;
            string strelonDiv = tfCalElonDivide.Text.Replace(',', '.');
            isN = Double.TryParse(strelonDiv, out elonDiv);

            if (isN == false)
            {
                System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za deljenje izduženja!");
            }
            else
            {
                OptionsInAnimation.mmDivide = elonDiv;
            }


            double elonMul;
            string strelonMul = tfCalElonMultiple.Text.Replace(',', '.');
            isN = Double.TryParse(strelonMul, out elonMul);

            if (isN == false)
            {
                System.Windows.Forms.MessageBox.Show("Trebate uneti broj u polje koeficijenta za množenje sile!");
            }
            else
            {
                OptionsInAnimation.mmCoeff = elonMul;
            }


            OptionsInAnimation.filePath = tfFilepathPlotting.Text;


            //write in xml file
            using (XmlWriter writer = XmlWriter.Create(System.Environment.CurrentDirectory + "\\configuration\\animationModeOptions.xml"))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("OnlineMode");


                writer.WriteStartElement("OnlineModeCurrentOptions");

                writer.WriteElementString("isContinuousDisplay", OptionsInAnimation.isContinuousDisplay.ToString());
                writer.WriteElementString("isDiscreteDisplay", OptionsInAnimation.isDiscreteDisplay.ToString());
                writer.WriteElementString("refreshTimeInterval", OptionsInAnimation.refreshTimeInterval.ToString());
                writer.WriteElementString("conResolution", OptionsInAnimation.conResolution.ToString());
                writer.WriteElementString("disResolution", OptionsInAnimation.disResolution.ToString());
                writer.WriteElementString("nutnDivide", OptionsInAnimation.nutnDivide.ToString());
                writer.WriteElementString("nutnMultiple", OptionsInAnimation.nutnMultiple.ToString());
                writer.WriteElementString("mmDivide", OptionsInAnimation.mmDivide.ToString());
                writer.WriteElementString("mmCoeff", OptionsInAnimation.mmCoeff.ToString());
                writer.WriteElementString("filePath", OptionsInAnimation.filePath);


                writer.WriteEndElement();


                writer.WriteEndElement();
                writer.WriteEndDocument();
            }


            //setStatusLabels();
            markSavedAnimationOptionsAsBlack();
            _animation.createAnimationGraphics();
        
        }

        private void markSavedAnimationOptionsAsBlack()
        {
            rbtnContinuous.Foreground = Brushes.Black;
            rbtnDiscrete.Foreground = Brushes.Black;
            tfResolutionCon.Foreground = Brushes.Black;
            tfResolutionDis.Foreground = Brushes.Black;
            //tfRefresh.Foreground = Brushes.Black;
            cmbRefresh.Foreground = Brushes.Black;


            tfCalForceDivide.Foreground = Brushes.Black;
            tfCalForceMultiple.Foreground = Brushes.Black;
            tfCalElonDivide.Foreground = Brushes.Black;
            tfCalElonMultiple.Foreground = Brushes.Black;

            tfFilepathPlotting.Foreground = Brushes.Black;
        }

        //private void setStatusLabels()
        //{
        //    if (OptionsInAnimation.isContinuousDisplay == true)
        //    {
        //        _animation.lblContinuousDisplay.Content = Constants.CONTINUOUSDISPLAY_ONLINE + "čekiran.";
        //    }
        //    if (OptionsInAnimation.isContinuousDisplay == false)
        //    {
        //        _animation.lblContinuousDisplay.Content = Constants.CONTINUOUSDISPLAY_ONLINE + "odčekiran.";
        //    }

        //    if (OptionsInAnimation.isDiscreteDisplay == true)
        //    {
        //        _animation.lblDiscreteDisplay.Content = Constants.DISCRETEDISPLAY_ONLINE + "čekiran.";
        //    }
        //    if (OptionsInAnimation.isDiscreteDisplay == false)
        //    {
        //        _animation.lblDiscreteDisplay.Content = Constants.DISCRETEDISPLAY_ONLINE + "odčekiran.";
        //    }

        //    _animation.lblResolutionCon.Content = Constants.RESOLUTIONCONTINUOUS_ONLINE + OptionsInAnimation.conResolution.ToString();
        //    _animation.lblResolutionDis.Content = Constants.RESOLUTIONDISCRETE_ONLINE + OptionsInAnimation.disResolution.ToString();
        //    _animation.lblRefreshTimeInterval.Content = Constants.REFRESHINTERVALTIME_ONLINE + OptionsInAnimation.refreshTimeInterval.ToString() + " [ms]";

        //    _animation.lblCalibrationForceDiv.Content = Constants.FORCEDIV_ONLINE + OptionsInAnimation.nutnDivide.ToString();
        //    _animation.lblCalibrationForceMul.Content = Constants.FORCEMUL_ONLINE + OptionsInAnimation.nutnMultiple.ToString();
        //    _animation.lblCalibrationElonDiv.Content = Constants.ELONDIV_ONLINE + OptionsInAnimation.mmDivide.ToString();
        //    _animation.lblCalibrationElonMul.Content = Constants.ELONMUL_ONLINE + OptionsInAnimation.mmCoeff.ToString();
        //    _animation.lblFilepathPart1.Content = Constants.FILEPATHPLOTTING;
        //    _animation.lblFilepathPart2.Content = OptionsInAnimation.filePath;

        //}


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            OptionsAnimation.isCreatedOptionsAnimation = false;
        }


        #region markUnsavedWithRedColor

        private void rbtnContinuous_Checked(object sender, RoutedEventArgs e)
        {
            rbtnContinuous.Foreground = Brushes.Red;
        }

        private void rbtnContinuous_Unchecked(object sender, RoutedEventArgs e)
        {
            rbtnContinuous.Foreground = Brushes.Red;
        }

        private void rbtnDiscrete_Checked(object sender, RoutedEventArgs e)
        {
            rbtnDiscrete.Foreground = Brushes.Red;
        }

        private void rbtnDiscrete_Unchecked(object sender, RoutedEventArgs e)
        {
            rbtnDiscrete.Foreground = Brushes.Red;
        }


        private void cmbRefresh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbRefresh.Foreground = Brushes.Red;
        }
        /*private void tfRefresh_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfRefresh.Foreground = Brushes.Red;
        }*/

        private void tfResolutionCon_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfResolutionCon.Foreground = Brushes.Red;
        }

        private void tfResolutionDis_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfResolutionDis.Foreground = Brushes.Red;
        }

        private void tfFilepathPlotting_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfFilepathPlotting.Foreground = Brushes.Red;
        }

        private void tfCalForceDivide_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfCalForceDivide.Foreground = Brushes.Red;
        }

        private void tfCalForceMultiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfCalForceMultiple.Foreground = Brushes.Red;
        }

        private void tfCalElonDivide_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfCalElonDivide.Foreground = Brushes.Red;
        }

        private void tfCalElonMultiple_TextChanged(object sender, TextChangedEventArgs e)
        {
            tfCalElonMultiple.Foreground = Brushes.Red;
        }

        #endregion

       


    }
}
