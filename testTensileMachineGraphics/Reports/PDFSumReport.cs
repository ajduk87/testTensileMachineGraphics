using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfFileWriter;
using testTensileMachineGraphics.OnlineModeFolder;
using System.IO;
using System.Diagnostics;
using testTensileMachineGraphics.Reports.SumReportClasses;
using System.Xml;
using System.Xml.Linq;

namespace testTensileMachineGraphics.Reports
{
    public class PDFSumReport
    {
        private const int numOfRowPerPage = 30;
        private int numOfRemarks = 0;
        private SumReportLastLoad _sumReport;
        private List<bool> rows = new List<bool>();

        private OnlineFileHeader onlineHeader;
        private GraphicPlotting plotting;
        private ResultsInterface resultsInterface;

        private Double DegToRad = Math.PI / 180.0;
        private Double Margin = 0.25;
        private Double PageWidth = 11.0;
        private Double PageHeight = 8.5;
        private Double HeadingHeight = 0.25;
        private Double NoteX = 0.1;
        private Double NoteY0 = 0.1;
        private Double NoteY1;
        private Double NoteY2;
        private Double NoteSize = 10;


        private Double AreaWidth;
        private Double AreaHeight;
        private Double AreaX1;
        //private Double AreaX2 = Margin + AreaWidth;
        private Double AreaX2;
        //private const Double AreaX3 = Margin + 2 * AreaWidth;
        private Double AreaX3;
        private Double AreaY1;
        private Double AreaY2;
        private Double AreaY3;
        private Double AreaY4;
        private Double AreaY5;


        private PdfDocument Document;
        private PdfPage Page;
        private PdfContents BaseContents;
        private PdfContents Contents;
        private PdfFont ArialNormal;
        private PdfFont ArialBold;
        private PdfFont ArialItalic;
        private PdfFont ArialBoldItalic;
        private PdfFont TimesNormal;
        private PdfFont TimesBold;
        private PdfFont TimesItalic;
        private PdfFont TimesBoldItalic;
        private PdfFont LucidaNormal;
        private PdfFont Comic;
        private PdfFont Symbol;


        private Double DispWidth;
        private Double DispHeight;


        public PDFSumReport(double h, double w, OnlineFileHeader onHeader, GraphicPlotting plott, ResultsInterface resInterface) 
        {
            PageHeight = h;
            PageWidth = w;

            onlineHeader = onHeader;
            plotting = plott;
            resultsInterface = resInterface;



            DispWidth = PageWidth - 2 * Margin;
            DispHeight = PageHeight - 2 * Margin - HeadingHeight;

            AreaWidth = DispWidth / 4;
            AreaHeight = DispHeight / 3;
            AreaX1 = Margin;
            AreaX2 = Margin + 3 * AreaWidth;
            AreaX3 = Margin + AreaWidth;
            AreaY1 = Margin;
            AreaY2 = Margin + AreaHeight;
            AreaY3 = Margin + 2 * AreaHeight;
            AreaY4 = Margin + 3 * AreaHeight;
            AreaY5 = Margin + 4 * AreaHeight;
        
        }


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


        
        public void CreateReport(SumReportLastLoad sumReport)
        {
            _sumReport = sumReport;
            foreach(SumReportRecord record in _sumReport.Records)
            {
                rows.Add(true);
                if (record.Napomena.Equals(String.Empty) == false)
                {
                    numOfRemarks++;
                    rows.Add(false);
                }
            }
            //LastInputOutputSavedData.GetData();
            

            // create document (letter size, portrait, inches)
          
            //string FileName = Constants.PATHOFSUMREPORT;
            string FileName =  Properties.Settings.Default.PATHOFSUMREPORT + _sumReport.Records[0].BrzbIzvestaja + ".pdf";
            FileInfo finfo = new FileInfo(FileName);


            bool isInUse = false;

            if (finfo.Exists == true)
            {
                isInUse = IsFileinUse(finfo);
                if (isInUse == true)
                {
                    System.Windows.Forms.MessageBox.Show(" FAJL SA PUTANJOM" + System.Environment.NewLine + FileName + System.Environment.NewLine + "JE OTVOREN!" + System.Environment.NewLine + "Zatvorite fajl pa probajte ponovo prikazati zbirni izveštaj!");
                    return;
                }
                finfo.Delete();
            }


            Document = new PdfDocument(PageWidth, PageHeight, UnitOfMeasure.Inch);
            


            // set encryption
            //Document.SetEncryption("password", Permission.All & ~Permission.Print);

            // define font resource
            ArialNormal = new PdfFont(Document, "Arial", System.Drawing.FontStyle.Regular, true);
            ArialBold = new PdfFont(Document, "Arial", System.Drawing.FontStyle.Bold, true);
            ArialItalic = new PdfFont(Document, "Arial", System.Drawing.FontStyle.Italic, true);
            ArialBoldItalic = new PdfFont(Document, "Arial", System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, true);
            TimesNormal = new PdfFont(Document, "Times New Roman", System.Drawing.FontStyle.Regular, true);
            TimesBold = new PdfFont(Document, "Times New Roman", System.Drawing.FontStyle.Bold, true);
            TimesItalic = new PdfFont(Document, "Times New Roman", System.Drawing.FontStyle.Italic, true);
            TimesBoldItalic = new PdfFont(Document, "Times New Roman", System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, true);
            LucidaNormal = new PdfFont(Document, "Lucida Console", System.Drawing.FontStyle.Regular, true);
            Comic = new PdfFont(Document, "Comic Sans MS", System.Drawing.FontStyle.Regular, true);
            //Symbol = new PdfFont(Document, "Wingdings", System.Drawing.FontStyle.Regular, true);


            NoteY1 = NoteY0 + ArialNormal.LineSpacing(NoteSize);
            NoteY2 = NoteY1 + ArialNormal.LineSpacing(NoteSize);

            // print euro sign instead of cent sign
            //ArialNormal.CharSubstitution(0x20ac, 0x20ac, 161);
            //ArialNormal.CharSubstitution(9679, 9679, 162);
            //ArialNormal.CharSubstitution(1488, 1514, 177);		// hebrew
            //ArialNormal.CharSubstitution(1040, 1045, 204);		// russian
            //ArialNormal.CharSubstitution(945, 950, 210);		// greek
            ArialNormal.CharSubstitution(353, 353, 210); //š
            ArialNormal.CharSubstitution(273, 273, 211);//đ
            ArialNormal.CharSubstitution(269, 269, 212);//č
            ArialNormal.CharSubstitution(263, 263, 213);//ć
            ArialNormal.CharSubstitution(382, 382, 214);//ž
            ArialNormal.CharSubstitution(352, 352, 215);//Š
            ArialNormal.CharSubstitution(272, 272, 216);//Đ
            ArialNormal.CharSubstitution(268, 268, 217);//Č
            ArialNormal.CharSubstitution(262, 262, 218);//Ć
            ArialNormal.CharSubstitution(381, 381, 219);//ž
            ArialNormal.CharSubstitution(8320, 8320, 220);//nula u indeksu

            ArialBold.CharSubstitution(353, 353, 210);//š
            ArialBold.CharSubstitution(273, 273, 211);//đ
            ArialBold.CharSubstitution(269, 269, 212);//č
            ArialBold.CharSubstitution(263, 263, 213);//ć
            ArialBold.CharSubstitution(382, 382, 214);//ž
            ArialBold.CharSubstitution(352, 352, 215);//Š
            ArialBold.CharSubstitution(272, 272, 216);//Đ
            ArialBold.CharSubstitution(268, 268, 217);//Č
            ArialBold.CharSubstitution(262, 262, 218);//Ć
            ArialBold.CharSubstitution(381, 381, 219);//ž

            //// create page base contents
            CreateBaseContents();

            CreatePagesContents();

            //// pages
            //if (makeFromLastSample == true)
            //{
            //    CreatePage1Contents();
            //}
            //else
            //{
            //    CreatePage1Contents(makeFromLastSample, xmlName);
            //}
            ////CreatePage2Contents();


            // create pdf file
            Document.CreateFile(FileName);

            // start default PDF reader and display the file
            Process Proc = new Process();
            Proc.StartInfo = new ProcessStartInfo(FileName);
            Proc.Start();

            // exit
            return;
        }


        ////////////////////////////////////////////////////////////////////
        // create base contents for all pages
        ////////////////////////////////////////////////////////////////////
        /// <summary>
        /// ovde se kreira naslov svake strane, zaglavlje tabele kao i same ivice tabela
        /// </summary>
        public void CreateBaseContents()
        {

            // create unattached contents
            BaseContents = new PdfContents(Document);

            // save graphics state
            BaseContents.SaveGraphicsState();


            // restore graphics state
            BaseContents.RestoreGraphicsState();

            BaseContents.DrawText(ArialBold, 14, 0.5 * PageWidth, 11.5, TextJustify.Center, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");

            //draw logo
            PdfImage Image2 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\logoLjig.png");
            //BaseContents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
            BaseContents.DrawImage(Image2, 8.5, 7.7, 1.4, 0.7);


            BaseContents.DrawText(ArialNormal, 8, 0.48, 7.85, TextJustify.Left, "BR. ZB. IZVEŠTAJA ");
            BaseContents.DrawLine(1.6, 8.0, 2.3, 8.0);
            BaseContents.DrawLine(1.6, 7.8, 2.3, 7.8);
            BaseContents.DrawLine(1.6, 8.0, 1.6, 7.8);
            BaseContents.DrawLine(2.3, 8.0, 2.3, 7.8);
            BaseContents.DrawText(ArialNormal, 8, 1.6, 7.85, TextJustify.Left, _sumReport.Records[0].BrzbIzvestaja);

            //horizontal lines
            BaseContents.DrawLine(0.45, 7.6, 10.8, 7.6);
            //BaseContents.DrawLine(0.45, 7.4, 10.8, 7.4);
            BaseContents.DrawLine(0.45, 7.2, 10.8, 7.2);
            BaseContents.DrawLine(0.45, 7.0, 10.8, 7.0);
            BaseContents.DrawLine(0.45, 6.8, 10.8, 6.8);
            BaseContents.DrawLine(0.45, 6.6, 10.8, 6.6);
            BaseContents.DrawLine(0.45, 6.4, 10.8, 6.4);
            BaseContents.DrawLine(0.45, 6.2, 10.8, 6.2);
            BaseContents.DrawLine(0.45, 6.0, 10.8, 6.0);
            BaseContents.DrawLine(0.45, 5.8, 10.8, 5.8);
            BaseContents.DrawLine(0.45, 5.6, 10.8, 5.6);
            BaseContents.DrawLine(0.45, 5.4, 10.8, 5.4);
            BaseContents.DrawLine(0.45, 5.2, 10.8, 5.2);
            BaseContents.DrawLine(0.45, 5.0, 10.8, 5.0);
            BaseContents.DrawLine(0.45, 4.8, 10.8, 4.8);
            BaseContents.DrawLine(0.45, 4.6, 10.8, 4.6);
            BaseContents.DrawLine(0.45, 4.4, 10.8, 4.4);
            BaseContents.DrawLine(0.45, 4.2, 10.8, 4.2);
            BaseContents.DrawLine(0.45, 4.0, 10.8, 4.0);
            BaseContents.DrawLine(0.45, 3.8, 10.8, 3.8);
            BaseContents.DrawLine(0.45, 3.6, 10.8, 3.6);
            BaseContents.DrawLine(0.45, 3.4, 10.8, 3.4);
            BaseContents.DrawLine(0.45, 3.2, 10.8, 3.2);
            BaseContents.DrawLine(0.45, 3.0, 10.8, 3.0);
            BaseContents.DrawLine(0.45, 2.8, 10.8, 2.8);
            BaseContents.DrawLine(0.45, 2.6, 10.8, 2.6);
            BaseContents.DrawLine(0.45, 2.4, 10.8, 2.4);
            BaseContents.DrawLine(0.45, 2.2, 10.8, 2.2);
            BaseContents.DrawLine(0.45, 2.0, 10.8, 2.0);
            BaseContents.DrawLine(0.45, 1.8, 10.8, 1.8);
            BaseContents.DrawLine(0.45, 1.6, 10.8, 1.6);
            BaseContents.DrawLine(0.45, 1.4, 10.8, 1.4);
            BaseContents.DrawLine(0.45, 1.2, 10.8, 1.2);
            //BaseContents.DrawLine(0.45, 1.0, 10.8, 1.0);//31st row
            //BaseContents.DrawLine(0.45, 0.8, 10.8, 0.8);//32nd row
            //BaseContents.DrawLine(0.45, 0.6, 10.8, 0.6);//33th row
            //BaseContents.DrawLine(0.45, 0.4, 10.8, 0.4);//34th row

            //vertical two lines
            //BaseContents.DrawLine(0.45, 7.6, 0.45, 0.4);//this is for 34 rows per page
            //BaseContents.DrawLine(10.8, 7.6, 10.8, 0.4);

            BaseContents.DrawLine(0.45, 7.6, 0.45, 1.2);
            BaseContents.DrawLine(10.8, 7.6, 10.8, 1.2);

            //header
            BaseContents.DrawLine(0.75, 7.6, 0.75, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 0.48, 7.42, TextJustify.Left, "RED.");
            BaseContents.DrawText(ArialNormal, 8, 0.48, 7.24, TextJustify.Left, "BR.");
            BaseContents.DrawLine(1.45 + 0.2125, 7.6, 1.45 + 0.2125, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 1.00, 7.42, TextJustify.Left, "BROJ"/*"POLAZNI"*/);
            BaseContents.DrawText(ArialNormal, 8, 1.00, 7.24, TextJustify.Left, "UZORKA"/*"KVALITET"*/);
            BaseContents.DrawLine(2.15 + 2 * 0.2125, 7.6, 2.15 + 2 * 0.2125, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 1.95, 7.32, TextJustify.Left, "ŠARŽA" /*"NAZIVNA"*/);
            //BaseContents.DrawText(ArialNormal, 8, 1.75, 7.24, TextJustify.Left, "DEBLJINA");
            //BaseContents.DrawLine(3.8, 7.6, 3.8, 7.2);
            //BaseContents.DrawText(ArialNormal, 8, 2.67, 7.42, TextJustify.Left, "ISPITIVAČ");
            BaseContents.DrawLine(4.5 - 0.7125, 7.6, 4.5 - 0.7125, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 2.95, 7.42, TextJustify.Left, "POLAZNI"/*"BROJ"*/);
            BaseContents.DrawText(ArialNormal, 8, 2.95, 7.24, TextJustify.Left, "KVALITET"/*"UZORKA"*/);
            BaseContents.DrawLine(4.6, 7.6, 4.6, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 3.95, 7.42, TextJustify.Left, "NAZIVNA"/*"ŠARŽA"*/);
            BaseContents.DrawText(ArialNormal, 8, 3.95, 7.24, TextJustify.Left, "DEBLJINA"/*"ŠARŽA"*/);
            //BaseContents.DrawLine(8.5, 7.6, 8.5, 7.2);//vertical line na kraju mehanicko tehnickih osobina
            BaseContents.DrawLine(8.5, 7.4, 8.5, 7.2); // samo za A ne za sve jel smo dodali Z
            BaseContents.DrawLine(4.6, 7.4, 9.4, 7.4);//horizontal line
            BaseContents.DrawText(ArialNormal, 8.5, 6.0, 7.45, TextJustify.Left, "MEHANIČKO-TEHNIČKE OSOBINE");


            BaseContents.DrawText(ArialNormal, 8,7.45 /*4.65*/, 7.24, TextJustify.Left, "Rm[MPa]");
            BaseContents.DrawLine(5.2 + 0.00, 7.4, 5.2 + 0.00, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 4.65/*5.28*/, 7.24, TextJustify.Left, "R");
            BaseContents.DrawText(ArialNormal, 6.5, 4.73 /*5.36*/, 7.24, TextJustify.Left, "p0.2");
            BaseContents.DrawText(ArialNormal, 8, 4.92 /*5.55*/, 7.24, TextJustify.Left, "[MPa]");
            BaseContents.DrawLine(5.9 + 0.00, 7.4, 5.9 + 0.00, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 5.28/*5.98*/, 7.24, TextJustify.Left, "R");
            BaseContents.DrawText(ArialNormal, 6.5,5.36 /*6.05*/, 7.24, TextJustify.Left, "t0.5");
            BaseContents.DrawText(ArialNormal, 8, 5.55 /*6.24*/, 7.24, TextJustify.Left, "[MPa]");
            BaseContents.DrawLine(6.6 + 2 * 0.00, 7.4, 6.6 + 2 * 0.00, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 6.0/*6.7*/, 7.24, TextJustify.Left, "ReL[MPa]");
            BaseContents.DrawLine(7.3 + 3 * 0.00, 7.4, 7.3 + 3 * 0.00, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 6.7 /*7.41*/, 7.24, TextJustify.Left, "ReH[MPa]");
            BaseContents.DrawLine(8.0 + 4 * 0.00, 7.4, 8.0 + 4 * 0.00, 7.2);
            //BaseContents.DrawLine(8.7, 7.4, 8.7, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 8.05, 7.24, TextJustify.Left, "A[%]");
            BaseContents.DrawLine(9.0 + 4 * 0.00, 7.6, 9.0 + 4 * 0.00, 7.2);//za kraj mehanicko tehnickih osobina vertikalna linija
            BaseContents.DrawText(ArialNormal, 8, 9.15, 7.24, TextJustify.Left, "KV[J]");
            BaseContents.DrawLine(9.6 + 4 * 0.00, 7.4, 9.6 + 4 * 0.00, 7.2);
            BaseContents.DrawText(ArialNormal, 8, 9.75, 7.24, TextJustify.Left, "KU[J]");
            BaseContents.DrawLine(10.2 + 4 * 0.00, 7.4, 10.2 + 4 * 0.00, 7.2);
            //BaseContents.DrawLine(9.4, 7.4, 9.4, 7.2);
            //BaseContents.DrawText(ArialNormal, 8, 9.0, 7.24, TextJustify.Left, "At");
            //BaseContents.DrawLine(10.8, 7.6, 10.8, 7.4);//vertical line
            BaseContents.DrawLine(9.4, 7.4, 10.8, 7.4);//horizontal line
            //BaseContents.DrawText(ArialNormal, 8, 9.45, 7.42, TextJustify.Left, "FAKTOR");
            //BaseContents.DrawLine(10.2, 7.6, 10.2, 7.2);//faktor
            BaseContents.DrawText(ArialNormal, 8, 10.5, 7.24, TextJustify.Left, "n");
            BaseContents.DrawText(ArialNormal, 8, 8.6, 7.24, TextJustify.Left, "Z[%]");
            //BaseContents.DrawText(ArialNormal, 8, 0.5, 7.05, TextJustify.Left, "NAPOMENATEST");

            
            //BaseContents.DrawLine(0.75, 7.4, 0.75, 7.2);
            //BaseContents.DrawLine(1.45, 7.4, 1.45, 7.2);
            //BaseContents.DrawLine(2.15, 7.4, 2.15, 7.2);
            //BaseContents.DrawLine(3.8, 7.4, 3.8, 7.2);

            BaseContents.DrawText(ArialNormal, 8, 0.45, 0.25, TextJustify.Left, "VERIFIKOVAO");
            BaseContents.DrawLine(1.3, 0.2, 5, 0.2);//horizontal line

            BaseContents.DrawText(ArialNormal, 8, 7, 0.25, TextJustify.Left, "ISPITIVAČ");
            BaseContents.DrawLine(7.8, 0.4, 9.8, 0.4);
            BaseContents.DrawLine(7.8, 0.2, 7.8, 0.4);
            BaseContents.DrawLine(9.8, 0.2, 9.8, 0.4);
            BaseContents.DrawLine(7.8, 0.2, 9.8, 0.2);
            BaseContents.DrawText(ArialNormal, 8, 7.8 + 0.01, 0.25, TextJustify.Left, _sumReport.Records[0].Ispitivac);//u jednom zbirnom izvestaju treba da ima samo jedan ispitivac

            // exit
            return;
        }



        private string KV(string zbIzv, string brIzvestaja)
        {
            string res = string.Empty;


            string pathXml = string.Empty;


            pathXml = Properties.Settings.Default.SumReportDir + zbIzv + "klatno" + ".xml";
            
            if (File.Exists(pathXml) == false)
            {
                return res;
            }


            string myXmlString = String.Empty;
            List<string> myXmlStrings = File.ReadAllLines(pathXml).ToList();
            if (myXmlStrings.Count == 0)
            {
                return res;
            }
            foreach (string s in myXmlStrings)
            {
                myXmlString += s;
            }

            if (myXmlString.Contains(Constants.XML_roots_ROOT2) == false)
            {
                System.Windows.Forms.MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                return res;
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(myXmlString);
            XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT2 + "/" + Constants.XML_roots_Uzorak);

            string temp = string.Empty;


            XElement xmlRoot = null;
            XElement xmlNew = null;
            int index = 0;
            List<string> brIzvestajaList = brIzvestaja.Split('/').ToList();
            List<string> tempList;

            foreach (XmlNode xn in xnList)
            {
                if (xn[Constants.XML_GeneralData_BRUZORKAKLATNO] != null)
                {
                    tempList = xn[Constants.XML_GeneralData_BRUZORKAKLATNO].InnerText.Split('/').ToList();
                    temp = tempList[0] + "/1";
                }
                //if (temp.Equals(brIzvestaja))
                if (temp.Equals(brIzvestajaList[0] + "/1"))
                {
                    return xn[Constants.XML_KV_SUMReport].InnerText;
                }
            }


            return res;
        }

        private string KU(string zbIzv, string brIzvestaja)
        {
            string res = string.Empty;


            string pathXml = string.Empty;


            pathXml = Properties.Settings.Default.SumReportDir + zbIzv + "klatno" + ".xml";

            if (File.Exists(pathXml) == false)
            {
                return res;
            }


            string myXmlString = String.Empty;
            List<string> myXmlStrings = File.ReadAllLines(pathXml).ToList();
            if (myXmlStrings.Count == 0)
            {
                return res;
            }
            foreach (string s in myXmlStrings)
            {
                myXmlString += s;
            }

            if (myXmlString.Contains(Constants.XML_roots_ROOT2) == false)
            {
                System.Windows.Forms.MessageBox.Show(" Učitali ste fajl sa pogrešnim formatom !! ");
                return res;
            }

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(myXmlString);
            XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT2 + "/" + Constants.XML_roots_Uzorak);

            string temp = string.Empty;


            XElement xmlRoot = null;
            XElement xmlNew = null;
            int index = 0;
            List<string> brIzvestajaList = brIzvestaja.Split('/').ToList();
            List<string> tempList;

            foreach (XmlNode xn in xnList)
            {
                if (xn[Constants.XML_GeneralData_BRUZORKAKLATNO] != null)
                {
                    tempList = xn[Constants.XML_GeneralData_BRUZORKAKLATNO].InnerText.Split('/').ToList();
                    temp = tempList[0] + "/1";
                }
                //if (temp.Equals(brIzvestaja))
                if (temp.Equals(brIzvestajaList[0] + "/1"))
                {
                    return xn[Constants.XML_KU_SUMReport].InnerText;
                }
            }


            return res;
        }

        /// <summary>
        /// ovde se samo popunjavaju odgovarajuce vrednosti na osnovu sadrzaja odgorvarajuceg XML fajla
        /// samo ide DrawText u ovoj metodi
        /// </summary>
        public void CreatePagesContents()
        {



            int pagenumber = (_sumReport.Records.Count + numOfRemarks)/numOfRowPerPage  +  1;
            int ordNumber = 1;
            int currNumberOfRemark = 0;
            int ii = 72;
            for (int i = 0; i < pagenumber; i++)
            {
                Contents = AddPageToDocument(1);
                for (int j = 0; j < numOfRowPerPage; j++)
                {
                    if (rows.Count == i * numOfRowPerPage + j)
                    {
                        break;
                    }

                    if (rows[i * numOfRowPerPage + j] == true)
                    {
                        Contents.DrawLine(0.75, 0.1 * (ii - 2 * j), 0.75, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(1.45 + 0.2125, 0.1 * (ii - 2 * j), 1.45 + 0.2125, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(2.15 + 2 * 0.2125, 0.1 * (ii - 2 * j), 2.15 + 2 * 0.2125, 0.1 * (ii - 2 * j - 2));
                        //Contents.DrawLine(3.8, 0.1 * (ii - 2 * j), 3.8, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(4.5 - 0.7125, 0.1 * (ii - 2 * j), 4.5 - 0.7125, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(4.6, 0.1 * (ii - 2 * j), 4.6, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(5.2, 0.1 * (ii - 2 * j), 5.2, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(5.9 + 0.00, 0.1 * (ii - 2 * j), 5.9 + 0.00, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(6.6 + 2 * 0.00, 0.1 * (ii - 2 * j), 6.6 + 2 * 0.00, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(7.3 + 3 * 0.00, 0.1 * (ii - 2 * j), 7.3 + 3 * 0.00, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(8.0 + 4 * 0.00, 0.1 * (ii - 2 * j), 8.0 + 4 * 0.00, 0.1 * (ii - 2 * j - 2));
                        //Contents.DrawLine(8.7, 0.1 * (ii - 2 * j), 8.7, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(8.5, 0.1 * (ii - 2 * j), 8.5, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(10.2, 0.1 * (ii - 2 * j), 10.2, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(9.60, 0.1 * (ii - 2 * j), 9.60, 0.1 * (ii - 2 * j - 2));
                        Contents.DrawLine(9.0, 0.1 * (ii - 2 * j), 9.0, 0.1 * (ii - 2 * j - 2));

                        Contents.DrawText(ArialNormal, 8, 0.50 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, ordNumber.ToString());
                        //Contents.DrawText(ArialNormal, 8, 0.80 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].PolazniKvalitet);
                        Contents.DrawText(ArialNormal, 8, 0.80 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrUzorka);
                        //Contents.DrawText(ArialNormal, 8, 1.50 + 0.2125 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].NazivnaDebljina);
                        Contents.DrawText(ArialNormal, 8, 1.50 + 0.2125 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Sarza);
                        //Contents.DrawText(ArialNormal, 8, 2.15 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Ispitivac);
                        //Contents.DrawText(ArialNormal, 8, 2.20 + 2 * 0.2125 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrUzorka);
                        Contents.DrawText(ArialNormal, 8, 2.20 + 2 * 0.2125 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].PolazniKvalitet);
                        //Contents.DrawText(ArialNormal, 8, 4.55 - 0.7425 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Sarza);
                        Contents.DrawText(ArialNormal, 8, 4.55 - 0.7425 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].NazivnaDebljina);
                        Contents.DrawText(ArialNormal, 8, 7.35/*4.65*/ + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Rm);
                        Contents.DrawText(ArialNormal, 8, 4.65/*5.25*/ + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Rp02);
                        Contents.DrawText(ArialNormal, 8, 5.25/*5.95*/ + 0.00 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Rt05);
                        Contents.DrawText(ArialNormal, 8, 5.95/*6.65*/ + 2 * 0.00 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].ReL);
                        Contents.DrawText(ArialNormal, 8, 6.65/*7.35*/ + 3 * 0.00 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].ReH);
                        Contents.DrawText(ArialNormal, 8, 8.05 + 4 * 0.00 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].A);
                        //Contents.DrawText(ArialNormal, 8, 8.7 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].At);
                        Contents.DrawText(ArialNormal, 8, 10.25, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].N);
                        Contents.DrawText(ArialNormal, 8, 8.55 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].Z);
                        if (_sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrUzorka.EndsWith("/1") == true)
                        {
                            Contents.DrawText(ArialNormal, 8, 9.05, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, KV(_sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrzbIzvestaja, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrUzorka));
                            Contents.DrawText(ArialNormal, 8, 9.65, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, KU(_sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrzbIzvestaja, _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark].BrUzorka));
                        }
                        ordNumber++;
                    }
                    else
                    {
                        Contents.DrawText(ArialBold, 8, 0.45 + 0.01, 0.1 * (ii - 2 * j - 2) + 0.05, TextJustify.Left, "Napomena za br. uzorka " + _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark - 1].BrUzorka + ":" + _sumReport.Records[i * numOfRowPerPage + j - currNumberOfRemark - 1].Napomena);
                        currNumberOfRemark++;
                    }
                    ////rest of vertical lines
                    //for (int ii = 72; ii >= 6; )
                    //{
                    //    BaseContents.DrawLine(0.75, 0.1 * ii, 0.75, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(1.45, 0.1 * ii, 1.45, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(2.15, 0.1 * ii, 2.15, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(3.8, 0.1 * ii, 3.8, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(4.5, 0.1 * ii, 4.5, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(5.2, 0.1 * ii, 5.2, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(5.9, 0.1 * ii, 5.9, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(6.6, 0.1 * ii, 6.6, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(7.3, 0.1 * ii, 7.3, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(8.0, 0.1 * ii, 8.0, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(8.7, 0.1 * ii, 8.7, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(9.4, 0.1 * ii, 9.4, 0.1 * (ii - 2));
                    //    BaseContents.DrawLine(10.1, 0.1 * ii, 10.1, 0.1 * (ii - 2));
                    //    i = i - 2;
                    //}
                }

               
            }
            //Contents.DrawText(ArialBold, 14, 0.05 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2, TextJustify.Left, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");

        }




       ////////////////////////////////////////////////////////////////////
        // Add page to document and draw page number
        ////////////////////////////////////////////////////////////////////

        public PdfContents AddPageToDocument
                (
                Int32 PageNo
                )
        {
            // add new page with two contents objects
            Page = new PdfPage(Document);
            Page.AddContents(BaseContents);
            PdfContents Contents = new PdfContents(Page);

            // draw page number right justified
            Contents.SaveGraphicsState();
            //Contents.DrawText(ArialNormal, 10, PageWidth - Margin, PageHeight - 0.75 - ArialNormal.CapHeight(10), TextJustify.Right, String.Format("Page {0}", PageNo));
            Contents.RestoreGraphicsState();
            return (Contents);
        }

    }
}
