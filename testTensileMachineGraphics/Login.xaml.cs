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

namespace testTensileMachineGraphics
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private MainWindow window;
        private bool ISOK = false;

        public Login()
        {
            InitializeComponent();

            window = new MainWindow();
            ISOK = false;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (tfUser.Text.Equals("franjaPPU") && tfPassword.Password.Equals("franjaPPU"))
            {
                ISOK = true;
                this.Hide();
            }

            if (ISOK)
            {
                window.Show();
            }
            else if (ISOK == false)
            {
                MessageBox.Show("Niste uneli ispravnu šifru ili korisničko ime!", "DALJE NEĆEŠ MOĆI");
                System.Environment.Exit(0);
            }
        }
    }
}
