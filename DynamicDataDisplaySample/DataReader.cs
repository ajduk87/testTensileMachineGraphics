using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using DynamicDataDisplaySample.VoltageViewModel;

namespace DynamicDataDisplaySample
{
    public class DataReader
    {
        
        #region members
        private string _filepath = String.Empty;
        private string _filepathOnline = String.Empty;


        //this part only for online
        private int staticPart = 14;
        private double L0_OnLine = 50.0;
        private double S0_OnLine = 4.75760;


        private List<string> data;
        private List<List<string>> dataSplit;
        private double l0 = 0.0;
        private double s0 = 0.0;
        private bool isNumbers = false;
        private bool isNumbersOnline = false;
        private bool isFirstPositive = false;
        private List<double> roughForce = new List<double>();
        private List<double> roughDeltaL = new List<double>();
        private int firstPositive = -1;
        private int lastPositive = -1;
        private int numberofNegativeForceEnd = 0;
        private  List<double> preassureInMPa = new List<double>();//[N/mm^2] roughForce * Constants.nutnMultiple / (Constants.nutnDivide * s0)

        private List<double> relativeElongation = new List<double>();//((absoluteElongation[i] * mmCoeff / mmDivide + l0) - l0)/(l0) * 100 
        private List<double> substructionFromFirstLengthData = new List<double>();
        private List<double> absoluteElongation = new List<double>();//roughDeltaL * Constants.mmCoeff / Constants.mmDivide
        private double firstRoughDeltaL = 0.0;


        private List<VoltagePoint> points = new List<VoltagePoint>();
        private string fp;

        #endregion

        #region properties

        /// <summary>
        /// readonly property
        /// </summary>
        public List<double> RelativeElongation 
        {
            get { return relativeElongation; }
        }


        public List<double> PreassureInMPa
        {
            get { return preassureInMPa; }
        }

        #endregion


        #region constructors

        public DataReader(string filepath) 
        {
            _filepath = filepath;
            _filepathOnline = filepath;
        }

        public DataReader(string filepath, string filepathOnline)
        {
            _filepath = filepath;
            _filepathOnline = filepathOnline;
        }

        #endregion

        #region methods

        private void ClearData()
        {
            isNumbers = false;
            data = new List<string>();
            dataSplit = new List<List<string>>();
            roughForce = new List<double>();
            roughDeltaL = new List<double>();
            preassureInMPa = new List<double>();
            relativeElongation = new List<double>();
            substructionFromFirstLengthData = new List<double>();
            absoluteElongation = new List<double>();
            numberofNegativeForceEnd = 0;
        }

        public int ReadDataOnLine()
        {
            isNumbersOnline = false;
            string str = string.Empty;
            string line = String.Empty;
            data = new List<string>();
            dataSplit = new List<List<string>>();
            try
            {
               // for (int i = 0; i < 1; i++)
                //{
                //File.Open(@"D:\___temprorary\online.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                using (FileStream fs = File.Open(_filepathOnline, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        StreamReader sr = new StreamReader(fs);
                        //str = sr.ReadToEnd();
                        while(!sr.EndOfStream)
                        {
                            line = sr.ReadLine();
                            if (sr.Peek() == -1)
                            {
                                str = line;
                            }
                        }
                        sr.Close();
                        fs.Close();



                        //Console.Write("Read: " + str + Environment.NewLine);
                    }
                    System.Threading.Thread.Sleep(10);
                    if (str == String.Empty)
                    {
                        return 0;
                    }
                    data = str.Split('\n').ToList();


                    for (int i = 0; i < data.Count; )
                    {
                        List<string> curList = new List<string>();
                        curList = data[i].Split('\t').ToList();
                        for (int j = 0; j < curList.Count; j++)
                        {
                           /* if (curList[j].Contains("L0"))
                            {
                                for (int k = 0; k < curList.Count; k++)
                                {
                                    double templ0 = 0.0;
                                    bool isN = Double.TryParse(curList[k], out templ0);
                                    if (isN)
                                    {
                                        l0 = templ0;
                                    }
                                }
                            }

                            if (curList[j].Contains("S0"))
                            {
                                for (int k = 0; k < curList.Count; k++)
                                {
                                    double temps0 = 0.0;
                                    bool isN = Double.TryParse(curList[k], out temps0);
                                    if (isN)
                                    {
                                        s0 = temps0;
                                    }
                                }
                            }*/

                            double tempForce3 = 0.0;
                            bool isN3 = Double.TryParse(curList[0], out tempForce3);
                            if (isNumbersOnline || i > staticPart || isN3)
                            {
                                double tempForce = 0.0;
                                bool isN = Double.TryParse(curList[0], out tempForce);
                                if (isN == true && j == 0)
                                {
                                    double currPreassureInMPa = 0.0;
                                    roughForce.Add(tempForce);
                                    if (tempForce > 0)
                                    {
                                        if (preassureInMPa.Count == 0)
                                        {
                                            double tempDeltaL2 = 0.0;
                                            bool isNN2 = Double.TryParse(curList[1], out tempDeltaL2);
                                            firstRoughDeltaL = tempDeltaL2;
                                        }
                                        currPreassureInMPa = tempForce * Constants.nutnMultiple / (Constants.nutnDivide * S0_OnLine);
                                        currPreassureInMPa = Math.Round(currPreassureInMPa, 6);
                                        preassureInMPa.Add(currPreassureInMPa);
                                    }
                                }
                                else
                                {
                                    if (isN == false)
                                    {
                                        //MessageBox.Show("Prvi element mora biti double! Sila!");
                                    }
                                }

                                double tempDeltaL = 0.0;
                                bool isNN = Double.TryParse(curList[1], out tempDeltaL);
                                if (isN == true && j == 1)
                                {
                                    double currsubstructionFromFirstLengthData = 0.0;
                                    roughDeltaL.Add(tempDeltaL);
                                    if (tempForce > 0)
                                    {
                                        double currAbsoluteElongation = 0.0;
                                        currsubstructionFromFirstLengthData = firstRoughDeltaL - tempDeltaL;
                                        substructionFromFirstLengthData.Add(currsubstructionFromFirstLengthData);
                                        currAbsoluteElongation = currsubstructionFromFirstLengthData * Constants.mmCoeff / Constants.mmDivide;
                                        currAbsoluteElongation = Math.Round(currAbsoluteElongation, 6);
                                        absoluteElongation.Add(currAbsoluteElongation);

                                        // calculate relativeElongation
                                        double currRelativeElongation = (((currAbsoluteElongation + L0_OnLine) - L0_OnLine) / L0_OnLine) * 100;
                                        currRelativeElongation = Math.Round(currRelativeElongation, 6);
                                        relativeElongation.Add(currRelativeElongation);
                                    }
                                }
                                else
                                {
                                    if (isN == false)
                                    {
                                        //MessageBox.Show("Drugi element mora biti double! Izduzenje!");
                                    }
                                }
                            }
                        }// currList end for

                        if (curList[0].Contains("Sila"))
                        {
                            isNumbersOnline = true;
                        }
                        dataSplit.Add(curList);
                        i = i + 100;
                    }// outer for loop

                    for (int i = 0; i < roughForce.Count; i++)
                    {
                        if (roughForce[i] > 0)
                        {
                            firstPositive = i;
                            break;
                        }
                    }

                    for (int i = roughForce.Count - 1; i > 0; i--)
                    {
                        numberofNegativeForceEnd++;
                        if (roughForce[i] > 0)
                        {
                            numberofNegativeForceEnd--;
                            lastPositive = i;
                            break;
                        }
                    }

                    //remove negatives from begin
                    roughForce.RemoveRange(0, firstPositive);
                    roughDeltaL.RemoveRange(0, firstPositive);

                    //remove negatives from end
                    roughForce.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);
                    roughDeltaL.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);

                //}
                    return data.Count;
            }
            catch (Exception ex)
            {
                Console.Write("read" + ex.ToString()); Console.Read();
                return - 1;
            }
        }

        public void ReadData()
        {
            ClearData();
            //data = File.ReadAllLines(System.Environment.CurrentDirectory + "\\files\\___002.txt").ToList();
            data = File.ReadAllLines(_filepath).ToList();


            for (int i = 0; i < data.Count; i++)
            {
                List<string> curList = new List<string>();
                curList = data[i].Split('\t').ToList();
                for (int j = 0; j < curList.Count; j++)
                {
                    if (curList[j].Contains("L0"))
                    {
                        for (int k = 0; k < curList.Count; k++)
                        {
                            double templ0 = 0.0;
                            bool isN = Double.TryParse(curList[k], out templ0);
                            if (isN)
                            {
                                l0 = templ0;
                            }
                        }
                    }

                    if (curList[j].Contains("S0"))
                    {
                        for (int k = 0; k < curList.Count; k++)
                        {
                            double temps0 = 0.0;
                            bool isN = Double.TryParse(curList[k], out temps0);
                            if (isN)
                            {
                                s0 = temps0;
                            }
                        }
                    }

                    double tempForceTest = 0.0;
                    bool isNTest = Double.TryParse(curList[0], out tempForceTest);
                    if (isNumbers || isNTest)
                    {
                        double tempForce = 0.0;
                        bool isN = Double.TryParse(curList[0], out tempForce);
                        if (isN == true && j == 0)
                        {
                            double currPreassureInMPa = 0.0;
                            roughForce.Add(tempForce);
                            if (tempForce > 0)
                            {
                                if (preassureInMPa.Count == 0)
                                {
                                    double tempDeltaL2 = 0.0;
                                    bool isNN2 = Double.TryParse(curList[1], out tempDeltaL2);
                                    firstRoughDeltaL = tempDeltaL2;
                                }
                                s0 = 4.75;//only for test purpose
                                currPreassureInMPa = tempForce * Constants.nutnMultiple / (Constants.nutnDivide * s0);
                                currPreassureInMPa = Math.Round(currPreassureInMPa, 6);
                                preassureInMPa.Add(currPreassureInMPa);
                            }
                        }
                        else
                        {
                            if (isN == false)
                            {
                                MessageBox.Show("Prvi element mora biti double! Sila!");
                            }
                        }

                        double tempDeltaL = 0.0;
                        bool isNN = Double.TryParse(curList[1], out tempDeltaL);
                        if (isN == true && j == 1)
                        {
                            double currsubstructionFromFirstLengthData = 0.0;
                            roughDeltaL.Add(tempDeltaL);
                            if (tempForce > 0)
                            {
                                double currAbsoluteElongation = 0.0;
                                currsubstructionFromFirstLengthData = firstRoughDeltaL - tempDeltaL;
                                substructionFromFirstLengthData.Add(currsubstructionFromFirstLengthData);
                                currAbsoluteElongation = currsubstructionFromFirstLengthData * Constants.mmCoeff / Constants.mmDivide;
                                currAbsoluteElongation = Math.Round(currAbsoluteElongation, 6);
                                absoluteElongation.Add(currAbsoluteElongation);

                                // calculate relativeElongation
                                l0 = 50;//only for test purpose
                                double currRelativeElongation = (((currAbsoluteElongation + l0) - l0) / l0) * 100;
                                currRelativeElongation = Math.Round(currRelativeElongation, 6);
                                relativeElongation.Add(currRelativeElongation);
                            }
                        }
                        else
                        {
                            if (isN == false)
                            {
                                MessageBox.Show("Drugi element mora biti double! Izduzenje!");
                            }
                        }
                    }
                }// currList end for

                if (curList[0].Contains("Sila"))
                {
                    isNumbers = true;
                }
                dataSplit.Add(curList);

            }// outer for loop

            for (int i = 0; i < roughForce.Count; i++)
            {
                if (roughForce[i] > 0)
                {
                    firstPositive = i;
                    break;
                }
            }

            for (int i = roughForce.Count - 1; i > 0; i--)
            {
                numberofNegativeForceEnd++;
                if (roughForce[i] > 0)
                {
                    numberofNegativeForceEnd--;
                    lastPositive = i;
                    break;
                }
            }

            //remove negatives from begin
            roughForce.RemoveRange(0, firstPositive);
            roughDeltaL.RemoveRange(0, firstPositive);

            //remove negatives from end
            roughForce.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);
            roughDeltaL.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);




        }

        #endregion
    }
}
