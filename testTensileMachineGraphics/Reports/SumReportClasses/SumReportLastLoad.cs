using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testTensileMachineGraphics.Reports.SumReportClasses
{
    /// <summary>
    /// u ovoj strukturi se cuva informacija o zadnjem ucitanom zbirnom izvestaju
    /// </summary>
    public class SumReportLastLoad
    {
        #region members

        private List<SumReportRecord> _records;

        #endregion

        #region constructors

        public SumReportLastLoad() 
        {
            try
            {
                _records = new List<SumReportRecord>();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[SumReportLastLoad.cs] {public SumReportLastLoad()}", System.DateTime.Now);
            }
        }

        #endregion

        #region properties

        public List<SumReportRecord> Records 
        {
            get { return _records; }
            set 
            {
                if (value != null)
                {
                    _records = value;
                }
            }
        }

        #endregion
    }
}
