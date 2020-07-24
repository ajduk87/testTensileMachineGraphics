using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using testTensileMachineGraphics.Options;
using System.IO;

namespace testTensileMachineGraphics
{
    public class SensorSimulator
    {

        #region members


        private List<double> dataOfSensorRough = new List<double>();
        private List<double> dataOfSensor = new List<double>();
        private string filepath = string.Empty;

        #endregion


        #region constructors

        public SensorSimulator(string fp)
        {
            filepath = fp;
        }

        #endregion

        #region properties

        public List<double> DataOfSensor
        {
            get { return dataOfSensor; }
            set
            {
                if (value != null)
                {
                    dataOfSensor = value;
                }
            }
        }

        public string Filepath
        {
            get { return filepath; }
            set { filepath = value; }
        }

        #endregion

        #region methods

        private List<string> context = new List<string>();



        public void LoadData(int columnNumber)
        {
            dataOfSensor.Clear();
            context.Clear();
            context = File.ReadAllLines(filepath).ToList();

            List<string> currDatas = new List<string>();
            double firstAbsoluteElongation = 0, tempAbsoluteElongation = 0, currRelativeElongation = 0;


            foreach (var currData in context)
            {
                currDatas = currData.Split('\t').ToList();
                double number = 0;
                bool isN = false;
                
                isN = double.TryParse(currDatas[columnNumber - 1], out number);
                dataOfSensor.Add(number);
                
            }
        }

        #endregion
    }
}
