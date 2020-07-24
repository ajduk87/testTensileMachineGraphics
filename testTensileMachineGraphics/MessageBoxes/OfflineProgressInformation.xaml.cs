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

namespace testTensileMachineGraphics.MessageBoxes
{
    /// <summary>
    /// Interaction logic for OfflineProgressInformation.xaml
    /// </summary>
    public partial class OfflineProgressInformation : Window
    {
        public OfflineProgressInformation()
        {
            try
            {
                InitializeComponent();
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[OfflineProgressInformation.xaml.cs] {public OfflineProgressInformation()}", System.DateTime.Now);
            }
        }
    }
}
