using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using testTensileMachineGraphics.PointViewModel;
using testTensileMachineGraphics.Options;
//check if file in use
using System.Runtime.InteropServices;
using System.Xml;

namespace testTensileMachineGraphics
{
    public class DataReader
    {


        public bool IsEkstenziometerUsed = false;

        /// <summary>
        /// da li moze da se pristupi online fajlu, da bise procitao zadnje upisani podatak
        /// </summary>
        /// <param name="file">objekat tipa FileInfo, koji cuva informacije o online fajlu</param>
        /// <returns></returns>
        protected virtual bool IsFileinUse(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }


        #region members
        private double firstRoughForceData = 0;
        private double firstRoughForceDataOffline = 0;

        private string _filepath = String.Empty;
        private string _filepathOnline = String.Empty;
        private bool fileNotExist = false;


        //this part only for online
        private int staticPart = 14;
        //only testing this must be in options
        private double L0_OnLine = 50.0;
        private double S0_OnLine = 4.75760;


        private List<string> data;
        private List<List<string>> dataSplit;
        private List<string> data2;
        private List<List<string>> dataSplit2;
        private double l0 = 0.0;
        private double s0 = 0.0;
        private bool isNumbers = false;
        private bool isNumbersOnline = false;
        //private bool isFirstPositive = false;
        private List<double> roughForce = new List<double>();
        private List<double> roughDeltaL = new List<double>();
        private int firstPositive = -1;
        private int lastPositive = -1;
        private int numberofNegativeForceEnd = 0;
        private List<double> preassureInMPa = new List<double>();//[N/mm^2] roughForce * Constants.nutnMultiple / (Constants.nutnDivide * s0)
        private List<double> forceInKN = new List<double>();
        private List<double> forceInKN_Offline = new List<double>();

        private List<double> relativeElongation = new List<double>();//((absoluteElongation[i] * mmCoeff / mmDivide + l0) - l0)/(l0) * 100
        private List<double> substructionFromFirstLengthData = new List<double>();
        private List<double> absoluteElongation = new List<double>();//roughDeltaL * Constants.mmCoeff / Constants.mmDivide
        private double firstRoughDeltaL = 0.0;
        //private double firstRoughDeltaL_All = 0.0;


        private List<MyPoint> points = new List<MyPoint>();

        private bool isL0Offline = false;
        private bool isS0Offline = false;


        public int CountLine_ONLINE = 0;
        public int CountLine_ONLINE2 = 0;


        //this is for fitting graphic
        private List<double> fittingRelativeElongation = new List<double>();
        private List<double> fittingPreassureInMPa = new List<double>();

        #endregion


        #region properties

        public string Filepath
        {
            get { return _filepath; }
            set { _filepath = value; }
        }

        public bool FileNotExist
        {
            get { return fileNotExist; }
        }

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

        public List<double> ForceInKN
        {
            get { return forceInKN; }
        }

        public List<double> ForceInKN_Offline
        {
            get { return forceInKN_Offline; }
        }

        public List<double> FittingRelativeElongation
        {
            get { return fittingRelativeElongation; }
            set
            {
                if (value != null)
                {
                    fittingRelativeElongation = value;
                }
            }
        }

        public List<double> FittingPreassureInMPa
        {
            get { return fittingPreassureInMPa; }
            set
            {
                if (value != null)
                {
                    fittingPreassureInMPa = value;
                }
            }
        }

        public bool IsL0Offline
        {
            get { return isL0Offline; }
        }

        public bool IsS0Offline
        {
            get { return isS0Offline; }
        }

        private double l0Offline;
        public double L0Offline
        {
            get { return l0Offline; }
        }

        private double s0Offline;
        public double S0Offline
        {
            get { return s0Offline; }
        }


        public List<double> RoughForce
        {
            get { return roughForce; }
        }

        public List<double> RoughDeltaL
        {
            get { return roughDeltaL; }
        }

        #endregion


        #region constructors

        private void isEkstenziometerUsedInitial()
        {
            //get saved options in online mode
            // Create an XML reader for this file.

            XmlTextReader textReader = new XmlTextReader(Constants.lastOnlineHeaderXml);

            // Read until end of file
            while (textReader.Read())
            {
                XmlNodeType nType = textReader.NodeType;

                // if node type is an element
                if (nType == XmlNodeType.Element)
                {


                    if (textReader.Name.Equals("rbtnYes_ConditionsOfTesting"))
                    {
                        string isYes = textReader.ReadElementContentAsString();
                        if (isYes.Equals("True"))
                        {
                            this.IsEkstenziometerUsed = true;
                        }
                        if (isYes.Equals("False"))
                        {
                            this.IsEkstenziometerUsed = false;
                        }
                    }
                    if (textReader.Name.Equals("rbtnNo_ConditionsOfTesting"))
                    {
                        string isNo = textReader.ReadElementContentAsString();
                        if (isNo.Equals("True"))
                        {
                            this.IsEkstenziometerUsed = false;
                        }
                        if (isNo.Equals("False"))
                        {
                            this.IsEkstenziometerUsed = true;
                        }
                    }



                }
            }//end of while loop


            textReader.Close();
        }



        public DataReader(string filepath)
        {
            _filepath = filepath;
            //_filepathOnline = filepath;
            isEkstenziometerUsedInitial();
        }

        public DataReader(string filepath, string filepathOnline)
        {
            _filepath = filepath;
            _filepathOnline = filepathOnline;
            isEkstenziometerUsedInitial();
        }

        public DataReader(string filepathOnline, double l0Online, double s0Online)
        {
            _filepathOnline = filepathOnline;
            L0_OnLine = l0Online;
            S0_OnLine = s0Online;
            isEkstenziometerUsedInitial();
        }

        #endregion

        #region methods

        public double getS0Offline()
        {
            try
            {
                ClearData();
                //data = File.ReadAllLines(System.Environment.CurrentDirectory + "\\files\\___002.txt").ToList();
                if (File.Exists(_filepath) == false)
                {
                    MessageBox.Show("Fajl sa izabranom putanjom " + _filepath + " ne postoji!" + System.Environment.NewLine + " Proverite izabranu putanju fajla.");
                    fileNotExist = true;
                    return 0;
                }
                data = File.ReadAllLines(_filepath).ToList();


                for (int i = 0; i < data.Count; i++)
                {
                    List<string> curList = new List<string>();
                    curList = data[i].Split('\t').ToList();
                    for (int j = 0; j < curList.Count; j++)
                    {

                        if (curList[j].Contains("S0"))
                        {
                            for (int k = 0; k < curList.Count; k++)
                            {
                                string strS0 = curList[0].Split(':')[1].Trim();
                                double temps0 = 0.0;
                                //bool isN = Double.TryParse(curList[k], out temps0);
                                bool isN = Double.TryParse(strS0, out temps0);
                                if (isN)
                                {
                                    s0 = temps0;
                                    s0Offline = temps0;
                                    isS0Offline = true;
                                    return s0Offline;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }// end of main for loop
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public double getS0Offline()}", System.DateTime.Now);
                return 0;
            }
        }

        public double getL0Offline()
        {
            try
            {
                ClearData();
                //data = File.ReadAllLines(System.Environment.CurrentDirectory + "\\files\\___002.txt").ToList();
                if (File.Exists(_filepath) == false)
                {
                    MessageBox.Show("Fajl sa izabranom putanjom " + _filepath + " ne postoji!" + System.Environment.NewLine + " Proverite izabranu putanju fajla.");
                    fileNotExist = true;
                    return 0;
                }
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
                                string strL0 = curList[0].Split(':')[1].Trim();
                                double templ0 = 0.0;
                                //bool isN = Double.TryParse(curList[k], out temps0);
                                bool isN = Double.TryParse(strL0, out templ0);
                                if (isN)
                                {
                                    l0 = templ0;
                                    l0Offline = templ0;
                                    isL0Offline = true;
                                    return l0Offline;
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                }// end of main for loop
                return 0;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public double getL0Offline()}", System.DateTime.Now);
                return 0;
            }
        }

        public void ClearFittingData()
        {
            try
            {
                fittingRelativeElongation = new List<double>();
                fittingPreassureInMPa = new List<double>();
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public void ClearFittingData()}", System.DateTime.Now);
            }
        }

        public void ClearData()
        {
            try
            {
                isNumbers = false;
                data = new List<string>();
                dataSplit = new List<List<string>>();
                data2 = new List<string>();
                dataSplit2 = new List<List<string>>();
                roughForce = new List<double>();
                roughDeltaL = new List<double>();
                preassureInMPa = new List<double>();
                relativeElongation = new List<double>();
                substructionFromFirstLengthData = new List<double>();
                absoluteElongation = new List<double>();
                forceInKN = new List<double>();
                forceInKN_Offline = new List<double>();
                numberofNegativeForceEnd = 0;
                isL0Offline = false;
                isS0Offline = false;
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public void ClearData()}", System.DateTime.Now);
            }
        }


        const int ERROR_SHARING_VIOLATION = 32;
        const int ERROR_LOCK_VIOLATION = 33;

        private bool IsFileLocked(Exception exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == ERROR_SHARING_VIOLATION || errorCode == ERROR_LOCK_VIOLATION;
        }

        internal bool CanReadFile(string filePath)
        {
            //Try-Catch so we dont crash the program and can check the exception
            try
            {
                //The "using" is important because FileStream implements IDisposable and
                //"using" will avoid a heap exhaustion situation when too many handles
                //are left undisposed.
                using (FileStream fileStream = File.Open(_filepathOnline, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    if (fileStream != null) fileStream.Close();  //This line is me being overly cautious, fileStream will never be null unless an exception occurs... and I know the "using" does it but its helpful to be explicit - especially when we encounter errors - at least for me anyway!

                }
            }
            catch (IOException ex)
            {
                //THE FUNKY MAGIC - TO SEE IF THIS FILE REALLY IS LOCKED!!!
                if (IsFileLocked(ex))
                {
                    // do something, eg File.Copy or present the user with a MsgBox - I do not recommend Killing the process that is locking the file
                    return false;
                }
            }
            finally
            { }
            return true;
        }


        public int ReadDataOnLine2()
        {
            try
            {
                if (CanReadFile(_filepathOnline) == false)
                {
                    return -1;
                }

                isNumbersOnline = false;
                string str = string.Empty;
                string line = String.Empty;
                data2 = new List<string>();
                dataSplit2 = new List<List<string>>();

                    FileInfo file = new FileInfo(_filepathOnline);
                    if (IsFileinUse(file) == true)
                    {
                        return -1;
                    }
                    // for (int i = 0; i < 1; i++)
                    //{
                    //File.Open(@"D:\___temporary\online.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    if (CanReadFile(_filepathOnline) == false)
                    {
                        return -1;
                    }
                    using (FileStream fs = File.Open(_filepathOnline, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        StreamReader sr = new StreamReader(fs);
                        CountLine_ONLINE2 = 0;
                        //str = sr.ReadToEnd();
                        while (!sr.EndOfStream)
                        {
                            CountLine_ONLINE2++;
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

                    data2 = str.Split('\n').ToList();

                    return data2.Count;
            }
            catch (Exception ex)
            {
                Console.Write("read" + ex.ToString()); Console.Read();
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public int ReadDataOnLine2()}", System.DateTime.Now);
                return -1;
            }
        }



        public int ReadDataOnLine()
        {

            try
            {
                isNumbersOnline = false;
                string str = string.Empty;
                string line = String.Empty;
                data = new List<string>();
                dataSplit = new List<List<string>>();

                    FileInfo file = new FileInfo(_filepathOnline);
                    if (IsFileinUse(file) == true)
                    {
                        return -1;//vrati da je fajl online bio zauzet od strane drugog procesa
                    }
                    // for (int i = 0; i < 1; i++)
                    //{
                    //File.Open(@"D:\___temporary\online.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                    if (CanReadFile(_filepathOnline) == false)
                    {
                        return -1;
                    }
                    using (FileStream fs = File.Open(_filepathOnline, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        StreamReader sr = new StreamReader(fs);
                        CountLine_ONLINE = 0;
                        //str = sr.ReadToEnd();
                        while (!sr.EndOfStream)
                        {
                            CountLine_ONLINE++;
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
                    //System.Threading.Thread.Sleep(10);
                    if (str == String.Empty)
                    {
                        return 0;
                    }
                    List<string> potentialData = new List<string>();

                    potentialData = str.Split('\n').ToList();
                    data = potentialData.Where(row => row.Split('\t').First().Length == 6).ToList();


                for (int i = 0; i < data.Count; )
                    {
                        List<string> curList = new List<string>();
                        curList = data[i].Split('\t').ToList();
                        //curList = data[i].Split('\b').ToList();
                        if (curList.Count == 1)
                        {
                            curList = data[i].Split('\b').ToList();
                        }
                        for (int j = 0; j < curList.Count; j++)
                        {
                            double tempForce3 = 0.0;
                            bool isN3 = Double.TryParse(curList[0], out tempForce3);
                            if (isNumbersOnline || i > staticPart || isN3)
                            {
                                double tempForce = 0.0;
                                bool isN = Double.TryParse(curList[0], out tempForce);
                                //uzimaj i pozitivne i negativne vrednosti za sirove vrednosti sile
                                //if (tempForce < 0) { tempForce = (-1) * tempForce; }
                                if (isN == true && j == 0)
                                {
                                    double currPreassureInMPa = 0.0;
                                    double currForceInKN = 0.0;

                                    if (roughForce.Count == 0 && tempForce >= 0)
                                    {
                                        firstRoughForceData = tempForce;
                                        roughForce.Add(tempForce);
                                    }



                                    if (tempForce >= 0)
                                    {
                                        tempForce = tempForce - firstRoughForceData;
                                        if (tempForce < 0)
                                        {
                                            tempForce = 0;
                                        }
                                        roughForce.Add(tempForce);

                                        if (preassureInMPa.Count == 0)
                                        {
                                            double tempDeltaL2 = 0.0;
                                            bool isNN2 = Double.TryParse(curList[1], out tempDeltaL2);
                                            firstRoughDeltaL = tempDeltaL2;
                                        }
                                        currPreassureInMPa = tempForce * OptionsInOnlineMode.nutnMultiple / (OptionsInOnlineMode.nutnDivide * OptionsInOnlineMode.S0);
                                        currPreassureInMPa = Math.Round(currPreassureInMPa, 6);
                                        //currForceInKN = tempForce * OptionsInOnlineMode.nutnMultiple / (OptionsInOnlineMode.nutnDivide);
                                        currForceInKN = tempForce * OptionsInOnlineMode.nutnMultiple / (OptionsInOnlineMode.nutnDivide * 1000);
                                        currForceInKN = Math.Round(currForceInKN, 6);
                                        preassureInMPa.Add(currPreassureInMPa);
                                        forceInKN.Add(currForceInKN);
                                    }

                                }
                                else
                                {

                                }

                                double tempDeltaL = 0.0;
                                bool isNN = Double.TryParse(curList[1], out tempDeltaL);
                                if (isN == true && j == 1)
                                {
                                    double currsubstructionFromFirstLengthData = 0.0;
                                    roughDeltaL.Add(tempDeltaL);
                                    if (tempForce >= 0)
                                    {
                                        double currAbsoluteElongation = 0.0;
                                        currsubstructionFromFirstLengthData = firstRoughDeltaL - tempDeltaL;
                                        substructionFromFirstLengthData.Add(currsubstructionFromFirstLengthData);

                                        if (this.IsEkstenziometerUsed == false)
                                        {
                                            currAbsoluteElongation = currsubstructionFromFirstLengthData * OptionsInOnlineMode.mmCoeff / OptionsInOnlineMode.mmDivide;
                                        }
                                        if (this.IsEkstenziometerUsed == true)
                                        {
                                            currAbsoluteElongation = currsubstructionFromFirstLengthData * OptionsInOnlineMode.mmCoeffWithEkstenziometer / OptionsInOnlineMode.mmDivideWithEkstenziometer;
                                        }
                                        currAbsoluteElongation = Math.Round(currAbsoluteElongation, 6);
                                        absoluteElongation.Add(currAbsoluteElongation);

                                        // calculate relativeElongation
                                        double currRelativeElongation = (((currAbsoluteElongation + OptionsInOnlineMode.L0) - OptionsInOnlineMode.L0) / OptionsInOnlineMode.L0) * 100;
                                        //if (currRelativeElongation < 0)
                                        //{
                                        //    currRelativeElongation = 0;
                                        //}
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
                    if (data.Count > 1)
                    {
                        roughForce.RemoveRange(0, firstPositive);
                        roughDeltaL.RemoveRange(0, firstPositive);
                    }

                    //remove negatives from end
                    if (data.Count > 1)
                    {
                        roughForce.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);
                        roughDeltaL.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);
                    }

                    //}



                        return data.Count;
            }
            catch (Exception ex)
            {
                Console.Write("read" + ex.ToString()); Console.Read();
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public int ReadDataOnLine()}", System.DateTime.Now);
                return -1;
            }
        }

        public void ReadData()
        {
            try
            {
                ClearData();
                //data = File.ReadAllLines(System.Environment.CurrentDirectory + "\\files\\___002.txt").ToList();
                if (File.Exists(_filepath) == false)
                {
                    MessageBox.Show("Fajl sa izabranom putanjom " + _filepath + " ne postoji!" + System.Environment.NewLine + " Proverite izabranu putanju fajla.");
                    fileNotExist = true;
                    return;
                }
                List<string> potentialData = new List<string>();

                potentialData = File.ReadAllLines(_filepath).ToList();
                data = potentialData.Where(row => row.Split('\t').First().Length == 6).ToList();


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
                                string strL0 = curList[0].Split(':')[1].Trim();
                                double templ0 = 0.0;
                                //bool isN = Double.TryParse(curList[k], out templ0);
                                bool isN = Double.TryParse(strL0, out templ0);
                                if (isN)
                                {
                                    l0 = templ0;
                                    l0Offline = templ0;
                                    isL0Offline = true;
                                }
                            }
                        }

                        if (curList[j].Contains("S0"))
                        {
                            for (int k = 0; k < curList.Count; k++)
                            {
                                string strS0 = curList[0].Split(':')[1].Trim();
                                double temps0 = 0.0;
                                //bool isN = Double.TryParse(curList[k], out temps0);
                                bool isN = Double.TryParse(strS0, out temps0);
                                if (isN)
                                {
                                    s0 = temps0;
                                    s0Offline = temps0;
                                    isS0Offline = true;
                                }
                            }
                        }

                        double tempForceTest = 0.0;
                        bool isNTest = Double.TryParse(curList[0], out tempForceTest);
                        if (isNumbers || isNTest)
                        {
                            double tempForce = 0.0;
                            double currForceInKN = 0.0;
                            if (curList[0].Equals(string.Empty) == true)
                            {
                                curList[0] = "0";
                            }

                            bool isN = Double.TryParse(curList[0], out tempForce);

                            if (isN == true && j == 0)
                            {
                                double currPreassureInMPa = 0.0;

                                if (roughForce.Count == 0 && tempForce >= 0)
                                {
                                    firstRoughForceDataOffline = tempForce;
                                    roughForce.Add(tempForce);
                                }


                                if (tempForce >= 0)
                                {

                                    tempForce = tempForce - firstRoughForceDataOffline;
                                    if (tempForce < 0)
                                    {
                                        tempForce = 0;
                                    }
                                    roughForce.Add(tempForce);

                                    if (preassureInMPa.Count == 0)
                                    {
                                        double tempDeltaL2 = 0.0;
                                        bool isNN2 = Double.TryParse(curList[1], out tempDeltaL2);
                                        firstRoughDeltaL = tempDeltaL2;
                                    }
                                    //s0 = 4.75;//only for test purpose
                                    //currPreassureInMPa = tempForce * Constants.nutnMultiple / (Constants.nutnDivide * s0);
                                    currPreassureInMPa = tempForce * OptionsInPlottingMode.nutnMultiple / (OptionsInPlottingMode.nutnDivide * s0);
                                    currPreassureInMPa = Math.Round(currPreassureInMPa, 6);
                                    //currForceInKN = tempForce * OptionsInPlottingMode.nutnMultiple / (OptionsInPlottingMode.nutnDivide);
                                    currForceInKN = tempForce * OptionsInPlottingMode.nutnMultiple / (OptionsInPlottingMode.nutnDivide * 1000);
                                    currForceInKN = Math.Round(currForceInKN, 6);
                                    preassureInMPa.Add(currPreassureInMPa);
                                    forceInKN_Offline.Add(currForceInKN);
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
                            if (isNN == true && j == 1)
                            {
                                double currsubstructionFromFirstLengthData = 0.0;
                                roughDeltaL.Add(tempDeltaL);
                                if (tempForce >= 0)
                                {
                                    double currAbsoluteElongation = 0.0;
                                    currsubstructionFromFirstLengthData = firstRoughDeltaL - tempDeltaL;
                                    substructionFromFirstLengthData.Add(currsubstructionFromFirstLengthData);
                                    //currAbsoluteElongation = currsubstructionFromFirstLengthData * Constants.mmCoeff / Constants.mmDivide;
                                    //currAbsoluteElongation = currsubstructionFromFirstLengthData * OptionsInPlottingMode.mmCoeff / OptionsInPlottingMode.mmDivide;

                                    if (this.IsEkstenziometerUsed == false)
                                    {
                                        currAbsoluteElongation = currsubstructionFromFirstLengthData * OptionsInOnlineMode.mmCoeff / OptionsInOnlineMode.mmDivide;
                                    }
                                    if (this.IsEkstenziometerUsed == true)
                                    {
                                        currAbsoluteElongation = currsubstructionFromFirstLengthData * OptionsInOnlineMode.mmCoeffWithEkstenziometer / OptionsInOnlineMode.mmDivideWithEkstenziometer;
                                    }
                                    currAbsoluteElongation = Math.Round(currAbsoluteElongation, 6);
                                    absoluteElongation.Add(currAbsoluteElongation);

                                    // calculate relativeElongation
                                    //l0 = 50;//only for test purpose
                                    double currRelativeElongation = (((currAbsoluteElongation + l0) - l0) / l0) * 100;
                                    //if (currRelativeElongation < 0)
                                    //{
                                    //    currRelativeElongation = 0;
                                    //}
                                    currRelativeElongation = Math.Round(currRelativeElongation, 6);
                                    relativeElongation.Add(currRelativeElongation);
                                }
                            }
                            else
                            {
                                if (isNN == false)
                                {
                                    // MessageBox.Show("Drugi element mora biti double! Izduzenje!");
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
                if (firstPositive != -1)
                {
                    roughForce.RemoveRange(0, firstPositive);
                    roughDeltaL.RemoveRange(0, firstPositive);


                    //remove negatives from end
                    if (lastPositive != -1 && lastPositive - firstPositive + numberofNegativeForceEnd + 1 <= roughDeltaL.Count - 1 && lastPositive - firstPositive + numberofNegativeForceEnd + 1 <= roughForce.Count - 1)
                    {
                        roughForce.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);
                        roughDeltaL.RemoveRange(lastPositive - firstPositive + 1, numberofNegativeForceEnd);
                    }

                }
            }
            catch (Exception ex)
            {
                Logger.WriteNode(ex.Message.ToString() + "[DataReader.cs] {public void ReadData()}", System.DateTime.Now);
            }


        }

        #endregion

    }
}
