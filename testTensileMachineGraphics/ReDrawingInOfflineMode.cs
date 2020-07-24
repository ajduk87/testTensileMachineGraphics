using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics
{
    /// <summary>
    /// sluzi za ponovno iscrtavanje grafika kada se promeni neka opcija u offline modu
    /// </summary>
    public class ReDrawingInOfflineMode
    {
        private DataReader dataReader;

        public DataReader DataReader 
        {
            get { return dataReader; }
            set 
            {
                if (value != null)
                {
                    dataReader = value;
                }
            }
        }


        public ReDrawingInOfflineMode(DataReader d) 
        {
            dataReader = d;
        }

    }
}
