using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using testTensileMachineGraphics.Reports;
using System.Windows;


namespace testTensileMachineGraphics.MessageBoxes
{
    public partial class SaveDialogForm : System.Windows.Forms.Form
    {



        private PrintScreen printscreen;
        public PrintScreen Printscreen
        {
            get { return printscreen; }
            set 
            {
                if (value != null)
                {
                    printscreen = value;
                }
            }
        }


        private bool isSaveGraphicsChangeOfRAndE = false;
        public bool IsSaveGraphicsChangeOfRAndE
        {
            get { return isSaveGraphicsChangeOfRAndE; }
            set { isSaveGraphicsChangeOfRAndE = value; }
        }

        private bool isManualNCalculated = false;
        public bool IsManualNCalculated
        {
            get { return isManualNCalculated; }
            set { isManualNCalculated = value; }
        }

        private bool isClickedToSaveFile = false;
        public bool IsClickedToSaveFile
        {
            get { return isClickedToSaveFile; }
            set { isClickedToSaveFile = value; }
        }

        public SaveDialogForm(bool _isSaveGraphicsChangeOfRAndE, bool _isManualNCalculated, PrintScreen pScreen)
        {
            try
            {
                InitializeComponent();
                isSaveGraphicsChangeOfRAndE = _isSaveGraphicsChangeOfRAndE;
                isManualNCalculated = _isManualNCalculated;
                printscreen = pScreen;
                saveFileDialog1.Filter = "Text (*.txt)|*.txt";
                saveFileDialog1.FileName = getDefaultFileName();
                //saveFileDialog1.ShowDialog();
                saveFiles();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[SaveDialogForm.xaml.cs] {public SaveDialogForm(bool _isSaveGraphicsChangeOfRAndE, bool _isManualNCalculated, PrintScreen pScreen)}", System.DateTime.Now);
            }
        }

        private string getDefaultFileName() 
        {
            try
            {
                string defaultName = string.Empty;

                defaultName = Properties.Settings.Default.SaveDirectory + LastInputOutputSavedData.tfBrUzorka_GeneralData;

                //string myXMLString = String.Empty;
                ////List<string> myXMLStringList = File.ReadAllLines(Constants.sampleReportFilepath).ToList();
                //List<string> myXMLStringList = File.ReadAllLines(Properties.Settings.Default.sampleReportFilepath).ToList();
                //foreach (string s in myXMLStringList)
                //{
                //    myXMLString += s;
                //}

                //XmlDocument xml = new XmlDocument();
                //xml.LoadXml(myXMLString);
                //XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);

                //foreach (XmlNode xn in xnList)
                //{
                //    defaultName = xn[Constants.XML_GeneralData_BRUZORKA].InnerText;
                //}

                defaultName = defaultName.Replace('/', '_');

                return defaultName;
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[SaveDialogForm.xaml.cs] {private string getDefaultFileName()}", System.DateTime.Now);
                return string.Empty;
            }
        }



        private void saveFiles() 
        {
            try
            {
                saveFileDialog1.FileName += ".txt";
                if (File.Exists(saveFileDialog1.FileName) == true)
                {
                    System.Windows.MessageBoxResult result = MessageBox.Show("Fajl " + saveFileDialog1.FileName + " postoji! Želite li da obrišete postojeći fajl?", "Fajl sa istim nazivom", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            {
                                break;
                            }
                        case MessageBoxResult.No:
                            {
                                return;
                            }
                    }

                }


                // Get file name.
                string name = saveFileDialog1.FileName;
                List<string> dataList = new List<string>();
                //ne pamti se vise iz online-a vec iz onog dela koji sluzi za pamcenje zadnjeg uzorka (nezapamcen.txt) jer se sada moze prekinuti pre zavrsetka online fajla
                //dataList = File.ReadAllLines(Constants.onlineFilepath).ToList();
                //dataList = File.ReadAllLines(Constants.unsavedFilepath).ToList();
                dataList = File.ReadAllLines(Properties.Settings.Default.unsavedFilepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                File.WriteAllLines(name, dataList);

                //GetAutomaticAnimation file name
                //string nameAnimation = saveFileDialog1.FileName.Split('.').ElementAt(0);
                //nameAnimation += ".anim";
                //List<string> dataListAnimation = new List<string>();
                //dataListAnimation = File.ReadAllLines(Constants.animationFilepath).ToList();
                //// Write to the file name selected.
                //// ... You can write the text from a TextBox instead of a string literal.
                //if (File.Exists(nameAnimation) == true)
                //{
                //    File.Delete(nameAnimation);
                //}
                //File.WriteAllLines(nameAnimation, dataListAnimation);


                //GetAutomaticChangedParameters file name
                string nameChangedParameters = saveFileDialog1.FileName.Split('.').ElementAt(0);
                nameChangedParameters += ".e2e4";
                List<string> dataListChangedParameters = new List<string>();
                //dataListChangedParameters = File.ReadAllLines(Constants.e2e4Filepath).ToList();
                dataListChangedParameters = File.ReadAllLines(Properties.Settings.Default.e2e4Filepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                if (File.Exists(nameChangedParameters) == true)
                {
                    File.Delete(nameChangedParameters);
                }
                File.WriteAllLines(nameChangedParameters, dataListChangedParameters);


                //GetAutomaticInputOutput file name
                string nameInputOutput = saveFileDialog1.FileName.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";
                List<string> dataListInputOutput = new List<string>();
                //dataListInputOutput = File.ReadAllLines(Constants.inputOutputFilepath).ToList();
                dataListInputOutput = File.ReadAllLines(Properties.Settings.Default.inputOutputFilepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                if (File.Exists(nameInputOutput) == true)
                {
                    File.Delete(nameInputOutput);
                }
                File.WriteAllLines(nameInputOutput, dataListInputOutput);

                //Copy png image
                string namePng = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePng += ".png";
                if (File.Exists(namePng) == true)
                {
                    File.Delete(namePng);
                }
                //File.Copy(Constants.sampleReportGraphicFilepath,namePng);
                File.Copy(Properties.Settings.Default.sampleReportGraphicFilepath, namePng);
                //Kopiraj i png image Promena Napona i Izduzenja ako je korisnik cekirao da zeli da ih zapamti
                string namePngPromenaNapona = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePngPromenaNapona += "PromenaNapona.png";
                if (File.Exists(namePngPromenaNapona) == true)
                {
                    File.Delete(namePngPromenaNapona);
                }
                //File.Copy(Constants.sampleReportGraphicFilepathChangeOfR, namePngPromenaNapona);
                File.Copy(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR, namePngPromenaNapona);

                string namePngPromenaIzduzenja = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePngPromenaIzduzenja += "PromenaIzduzenja.png";
                if (File.Exists(namePngPromenaIzduzenja) == true)
                {
                    File.Delete(namePngPromenaIzduzenja);
                }
                //File.Copy(Constants.sampleReportGraphicFilepathChangeOfE, namePngPromenaIzduzenja);
                File.Copy(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE, namePngPromenaIzduzenja);


                //Copy xml file for sample report
                string nameSampleReportXml = saveFileDialog1.FileName.Split('.').ElementAt(0);
                nameSampleReportXml += ".xml";
                if (File.Exists(nameSampleReportXml) == true)
                {
                    File.Delete(nameSampleReportXml);
                }
                //File.Copy(Constants.sampleReportFilepath, nameSampleReportXml);
                File.Copy(Properties.Settings.Default.sampleReportFilepath, nameSampleReportXml);
                //ovde postavi korisnikovu odluku da li zeli da ucita u izvestaj i grafike promene napona i izduzenja
                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(nameSampleReportXml).ToList();
                if (myXmlStrings.Count == 0)
                {
                    return;
                }
                foreach (string s in myXmlStrings)
                {
                    myXmlString += s;
                }

                if (myXmlString.Contains(Constants.XML_roots_ROOT) == false)
                {
                    MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                    return;
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(myXmlString);
                XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);
                if (isSaveGraphicsChangeOfRAndE == true)
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ShowGraphicChangeOfRAndE].InnerText = "True";
                    }
                }
                else
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ShowGraphicChangeOfRAndE].InnerText = "False";
                    }
                }
                if (isManualNCalculated == true)
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ManualnIsCalculated].InnerText = "True";
                    }
                }
                else
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ManualnIsCalculated].InnerText = "False";
                    }
                }

                xml.Save(nameSampleReportXml);



                //Copy nFaktor file for checking n in application NHardeningFactorChecking
                //string nametNFaktorChecking = saveFileDialog1.FileName.Split('.').ElementAt(0);
                //nametNFaktorChecking += ".nFaktor";
                //if (File.Exists(nametNFaktorChecking) == true)
                //{
                //    File.Delete(nametNFaktorChecking);
                //}
                //string sourceName = Constants.unsavedFilepath;
                //string sourceNamenFaktor = sourceName.Split('.').ElementAt(0);
                //sourceNamenFaktor += ".nFaktor";
                //File.Copy(sourceNamenFaktor, nametNFaktorChecking);

                //save sample report in default directory
                string namePDFSampleReport = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePDFSampleReport += ".pdf";
                List<string> temp = namePDFSampleReport.Split('\\').ToList();
                if (File.Exists(namePDFSampleReport) == true)
                {
                    File.Delete(namePDFSampleReport);
                }
                printscreen.DefaultPath = namePDFSampleReport;
                printscreen.btnPrintSampleOnlyMakeReport.RaiseEvent(new System.Windows.RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                if (File.Exists(Properties.Settings.Default.SaveDirectoryForReports + temp.Last()) == true)
                {
                    File.Delete(Properties.Settings.Default.SaveDirectoryForReports + temp.Last());
                }
                File.Copy(printscreen.DefaultPath, Properties.Settings.Default.SaveDirectoryForReports + temp.Last());

                isClickedToSaveFile = true;

                string namePE = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePE += ".pe";
                if (File.Exists(namePE) == true)
                {
                    File.Delete(namePE);
                }
                File.Copy(Properties.Settings.Default.unsavedFilepathPreassureElongation, namePE);

                this.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[SaveDialogForm.xaml.cs] {private void saveFiles()}", System.DateTime.Now);
            }
        }



        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try { 
                // Get file name.
                string name = saveFileDialog1.FileName;
                List<string> dataList = new List<string>();
                //ne pamti se vise iz online-a vec iz onog dela koji sluzi za pamcenje zadnjeg uzorka (nezapamcen.txt) jer se sada moze prekinuti pre zavrsetka online fajla
                //dataList = File.ReadAllLines(Constants.onlineFilepath).ToList();
                //dataList = File.ReadAllLines(Constants.unsavedFilepath).ToList();
                dataList = File.ReadAllLines(Properties.Settings.Default.unsavedFilepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                File.WriteAllLines(name, dataList);

                //GetAutomaticAnimation file name
                //string nameAnimation = saveFileDialog1.FileName.Split('.').ElementAt(0);
                //nameAnimation += ".anim";
                //List<string> dataListAnimation = new List<string>();
                //dataListAnimation = File.ReadAllLines(Constants.animationFilepath).ToList();
                //// Write to the file name selected.
                //// ... You can write the text from a TextBox instead of a string literal.
                //if (File.Exists(nameAnimation) == true)
                //{
                //    File.Delete(nameAnimation);
                //}
                //File.WriteAllLines(nameAnimation, dataListAnimation);


                //GetAutomaticChangedParameters file name
                string nameChangedParameters = saveFileDialog1.FileName.Split('.').ElementAt(0);
                nameChangedParameters += ".e2e4";
                List<string> dataListChangedParameters = new List<string>();
                //dataListChangedParameters = File.ReadAllLines(Constants.e2e4Filepath).ToList();
                dataListChangedParameters = File.ReadAllLines(Properties.Settings.Default.e2e4Filepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                if (File.Exists(nameChangedParameters) == true)
                {
                    File.Delete(nameChangedParameters);
                }
                File.WriteAllLines(nameChangedParameters, dataListChangedParameters);


                //GetAutomaticInputOutput file name
                string nameInputOutput = saveFileDialog1.FileName.Split('.').ElementAt(0);
                nameInputOutput += ".inputoutput";
                List<string> dataListInputOutput = new List<string>();
                //dataListInputOutput = File.ReadAllLines(Constants.inputOutputFilepath).ToList();
                dataListInputOutput = File.ReadAllLines(Properties.Settings.Default.inputOutputFilepath).ToList();
                // Write to the file name selected.
                // ... You can write the text from a TextBox instead of a string literal.
                if (File.Exists(nameInputOutput) == true)
                {
                    File.Delete(nameInputOutput);
                }
                File.WriteAllLines(nameInputOutput, dataListInputOutput);

                //Copy png image
                string namePng = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePng += ".png";
                if (File.Exists(namePng) == true)
                {
                    File.Delete(namePng);
                }
                //File.Copy(Constants.sampleReportGraphicFilepath,namePng);
                File.Copy(Properties.Settings.Default.sampleReportGraphicFilepath, namePng);
                //Kopiraj i png image Promena Napona i Izduzenja ako je korisnik cekirao da zeli da ih zapamti
                string namePngPromenaNapona = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePngPromenaNapona += "PromenaNapona.png";
                if (File.Exists(namePngPromenaNapona) == true)
                {
                    File.Delete(namePngPromenaNapona);
                }
                //File.Copy(Constants.sampleReportGraphicFilepathChangeOfR, namePngPromenaNapona);
                File.Copy(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfR, namePngPromenaNapona);

                string namePngPromenaIzduzenja = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePngPromenaIzduzenja += "PromenaIzduzenja.png";
                if (File.Exists(namePngPromenaIzduzenja) == true)
                {
                    File.Delete(namePngPromenaIzduzenja);
                }
                //File.Copy(Constants.sampleReportGraphicFilepathChangeOfE, namePngPromenaIzduzenja);
                File.Copy(Properties.Settings.Default.sampleReportGraphicFilepathChangeOfE, namePngPromenaIzduzenja);


                //Copy xml file for sample report
                string nameSampleReportXml = saveFileDialog1.FileName.Split('.').ElementAt(0);
                nameSampleReportXml += ".xml";
                if (File.Exists(nameSampleReportXml) == true)
                {
                    File.Delete(nameSampleReportXml);
                }
                //File.Copy(Constants.sampleReportFilepath, nameSampleReportXml);
                File.Copy(Properties.Settings.Default.sampleReportFilepath, nameSampleReportXml);
                //ovde postavi korisnikovu odluku da li zeli da ucita u izvestaj i grafike promene napona i izduzenja
                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(nameSampleReportXml).ToList();
                if (myXmlStrings.Count == 0)
                {
                    return;
                }
                foreach (string s in myXmlStrings)
                {
                    myXmlString += s;
                }

                if (myXmlString.Contains(Constants.XML_roots_ROOT) == false)
                {
                    MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                    return;
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(myXmlString);
                XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);
                if (isSaveGraphicsChangeOfRAndE == true)
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ShowGraphicChangeOfRAndE].InnerText = "True";
                    }
                }
                else
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ShowGraphicChangeOfRAndE].InnerText = "False";
                    }
                }
                if (isManualNCalculated == true)
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ManualnIsCalculated].InnerText = "True";
                    }
                }
                else
                {
                    foreach (XmlNode xn in xnList)
                    {
                        xn[Constants.XML_ManualnIsCalculated].InnerText = "False";
                    }
                }

                xml.Save(nameSampleReportXml);



                //Copy nFaktor file for checking n in application NHardeningFactorChecking
                //string nametNFaktorChecking = saveFileDialog1.FileName.Split('.').ElementAt(0);
                //nametNFaktorChecking += ".nFaktor";
                //if (File.Exists(nametNFaktorChecking) == true)
                //{
                //    File.Delete(nametNFaktorChecking);
                //}
                //string sourceName = Constants.unsavedFilepath;
                //string sourceNamenFaktor = sourceName.Split('.').ElementAt(0);
                //sourceNamenFaktor += ".nFaktor";
                //File.Copy(sourceNamenFaktor, nametNFaktorChecking);

                //save sample report in default directory
                string namePDFSampleReport = saveFileDialog1.FileName.Split('.').ElementAt(0);
                namePDFSampleReport += ".pdf";
                List<string> temp = namePDFSampleReport.Split('\\').ToList();
                if (File.Exists(namePDFSampleReport) == true)
                {
                    File.Delete(namePDFSampleReport);
                }
                printscreen.DefaultPath = namePDFSampleReport;
                printscreen.btnPrintSampleOnlyMakeReport.RaiseEvent(new System.Windows.RoutedEventArgs(System.Windows.Controls.Button.ClickEvent));

                File.Copy(printscreen.DefaultPath, Properties.Settings.Default.SaveDirectoryForReports + temp.Last());

                isClickedToSaveFile = true;

                this.Close();
            }
            catch (Exception ex) 
            {
                Logger.WriteNode(ex.Message.ToString() + "[SaveDialogForm.xaml.cs] {private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)}", System.DateTime.Now);
            }
       }
        
    }
}
