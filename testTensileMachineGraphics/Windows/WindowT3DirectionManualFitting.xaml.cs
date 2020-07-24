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

namespace testTensileMachineGraphics.Windows
{
    /// <summary>
    /// Interaction logic for WindowT3DirectionManualFitting.xaml
    /// </summary>
    public partial class WindowT3DirectionManualFitting : Window
    {

        private GraphicPlotting _graphicPlotting;

        public GraphicPlotting GraphicPlotting
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

        private const double xconst = 100;
        private const double yconst = 0;

        private double x;
        public double X
        {
            get { return x; }
            set { x = value; }
        }

        private double y;
        public double Y
        {
            get { return y; }
            set { y = value; }
        }


        public WindowT3DirectionManualFitting()
        {
            InitializeComponent();
        }

        public void setWindowForChosingPoints()
        {
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = x + xconst;
            this.Top = y + yconst;
        }

        private void rbtnT3MovingByX_Checked(object sender, RoutedEventArgs e)
        {
            _graphicPlotting.T3movingDirectionByYAxis = false;
           
            this.Close();
        }

        private void rbtnT3MovingByY_Checked(object sender, RoutedEventArgs e)
        {
            _graphicPlotting.T3movingDirectionByYAxis = true;
           
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //ukidamo postojanje ovog prozora u aplikaciji
            //_graphicPlotting.IsWindowForMovingT3DirectionShown = false;
        }
    }
}
