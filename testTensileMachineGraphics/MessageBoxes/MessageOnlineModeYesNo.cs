using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace testTensileMachineGraphics.MessageBoxes
{
    public partial class MessageOnlineModeYesNo : Form
    {

        #region members

        private string _messageText = "Želite li da sačuvate zadnje merenje ?";
        private bool _isYesChosen = false;

        #endregion

        #region properties

        public string MessageText
        {
            get { return _messageText; }
            set 
            {
                if (value != null)
                {
                    _messageText = value;
                }
            }
        }

        public bool IsYesChosen 
        {
            get { return _isYesChosen; }
            set { _isYesChosen = value; }
        }



        #endregion

        public MessageOnlineModeYesNo()
        {
            try
            {
                InitializeComponent();
                lblMessageText.Text = _messageText;
                this.Location = new Point(0, 0);
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MessageOnlineModeYesNo.xaml.cs] {public MessageOnlineModeYesNo()}", System.DateTime.Now);
            }
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            try
            {
                _isYesChosen = true;
                this.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[MessageOnlineModeYesNo.xaml.cs] {private void btnYes_Click(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            try
            {
                _isYesChosen = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[MessageOnlineModeYesNo.xaml.cs] {private void btnNo_Click(object sender, EventArgs e)}", System.DateTime.Now);
            }
        }
    }
}
