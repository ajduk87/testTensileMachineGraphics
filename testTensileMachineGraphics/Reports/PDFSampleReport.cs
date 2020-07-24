using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PdfFileWriter;
using System.Diagnostics;
using testTensileMachineGraphics.OnlineModeFolder;
using System.Windows.Documents;
using System.IO;
using System.Drawing;
using System.Windows.Controls;
using testTensileMachineGraphics.Options;
using System.Xml;
using System.Windows.Forms;
namespace testTensileMachineGraphics.Reports
{
    public class PDFSampleReport
    {

        public string FileName = string.Empty;

        public string DefaultName = string.Empty;

        private bool _makeFromLastSample = false;

        private Double widthbrojVerzije = 0;
        private Double widthR1 = 0;
        private Double widthR2 = 0;
        private Double widthR3 = 0;
        private Double widthR4 = 0;
        private Double widthR5 = 0;
        private Double widthRucnoN = 0;
        private Double widthRucnoNInterval = 0;

        private Double widtha0 = 0;
        private Double widthau = 0;
        private Double widthb0 = 0;
        private Double widthbu = 0;
        private Double widthd0 = 0;
        private Double widthdu = 0;
        private Double widthD0 = 0;
        private Double widthDu = 0;
        private Double widthtfL0 = 0;
        private Double widthtfLu = 0;
        private Double widthtfLc = 0;
        private Double widthtfS0 = 0;
        private Double widthtfSu = 0;

        private Double widthtfF = 0;
        private Double widthtfFm = 0;
        private Double widthtfRmax = 0;
        private Double widthtfE2 = 0;
        private Double widthtfE4 = 0;

        private Double widthtfRp02 = 0;
        private Double widthtfRt05 = 0;
        private Double widthtfReL = 0;
        private Double widthtfReH = 0;
        private Double widthtfRm = 0;
        private Double widthtfRRm = 0;
        private Double widthRe = 0;
        private Double widthE = 0;

        private Double widthtfAg = 0;
        private Double widthtfAgt = 0;
        private Double widthtfA = 0;
        private Double widthtfAt = 0;
        private Double widthtfZ = 0;
        private Double widthtfn = 0;
        private Double widthtfn2 = 0;
       

        private OnlineFileHeader onlineHeader;
        private GraphicPlotting plotting;
        private ResultsInterface resultsInterface;

        private  Double DegToRad = Math.PI / 180.0;
        private Double  Margin = 0.25;
        private  Double PageWidth = 8.5;
        private  Double PageHeight = 11.0;
        private  Double HeadingHeight = 0.25;
        private  Double NoteX = 0.1;
        private  Double NoteY0 = 0.1;
        private Double NoteY1;
        private Double NoteY2;
        private  Double NoteSize = 10;


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


        public PDFSampleReport(double h, double w, OnlineFileHeader onHeader, GraphicPlotting plott, ResultsInterface resInterface, string path) 
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

            DefaultName = path;
        
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

        /// <summary>
        /// vrednost promenljive makeFromLastSample je true kada se klikne na dugme napravi izvestaj a false kada se klikne na ucitaj izvestaj
        /// </summary>
        /// <param name="makeFromLastSample"></param>
        public void CreateReport(bool makeFromLastSample = true, string xmlName = "")
        {


            //load last saved data
            if (makeFromLastSample == true)
            {
                LastInputOutputSavedData.GetData();
            }


            // create document (letter size, portrait, inches)
            //string FileName = System.Environment.CurrentDirectory + "\\pojedinacanIzvestajTest.pdf";
            //string FileName = Constants.PATHOFSAMPLEREPORT;
            string FileName = DefaultName;

            if (DefaultName.Equals(string.Empty) == false)
            {

                FileInfo finfo = new FileInfo(FileName);
                bool isInUse = false;

                if (finfo.Exists == true)
                {
                    isInUse = IsFileinUse(finfo);
                    if (isInUse == true)
                    {
                        System.Windows.Forms.MessageBox.Show(" FAJL SA PUTANJOM" + System.Environment.NewLine + FileName + System.Environment.NewLine + "JE OTVOREN!" + System.Environment.NewLine + "Zatvorite fajl pa probajte ponovo kreirati izveštaj!");
                        return;
                    }
                    finfo.Delete();
                }
            }

            if (resultsInterface == null)
            {
                System.Windows.Forms.MessageBox.Show(" NE POSTOJE IZLAZNI PODACI !");
                return;
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

            // create page base contents
            CreateBaseContents();

            // pages
            if (makeFromLastSample == true)
            {
                CreatePage1Contents();
            }
            else 
            {
                CreatePage1Contents(makeFromLastSample, xmlName);
            }
            //CreatePage2Contents();

            bool alreadyExist = false;
            // create pdf file
            if (makeFromLastSample == false)
            {
                string namePDFSampleReport = xmlName.Split('.').ElementAt(0);
                namePDFSampleReport += ".pdf";
                if (File.Exists(namePDFSampleReport) == true)
                {
                    //File.Delete(namePDFSampleReport);
                    alreadyExist = true;
                    //ako postoji fajl sa tim imenom obrisi taj fajl da bi kasnije kreirao novi fajl
                    File.Delete(namePDFSampleReport);
                    alreadyExist = false;
                }
                FileName = namePDFSampleReport;
                this.FileName = namePDFSampleReport;
            }

            if (alreadyExist == false)
            {
                Document.CreateFile(FileName);
                this.FileName = FileName;
            }

            // start default PDF reader and display the file

            if (plotting.Printscreen.chbShowReports.IsChecked == true)
            {
                Process Proc = new Process();
                Proc.StartInfo = new ProcessStartInfo(FileName);
                Proc.Start();
            }

          

            // exit
            return;
        }


        ////////////////////////////////////////////////////////////////////
        // create base contents for all pages
        ////////////////////////////////////////////////////////////////////

        public void CreateBaseContents()
        {

            // create unattached contents
            BaseContents = new PdfContents(Document);

            // save graphics state
            BaseContents.SaveGraphicsState();


            // restore graphics state
            BaseContents.RestoreGraphicsState();

            // exit
            return;
        }

        /// <summary>
        /// OVA METODA SE KORISIT SAMO KOD DUGMETA [UCITAJ IZVESTAJ]
        /// ovako se postavlja slika glavnog grafika u slucaju da je nesto cekirano.
        /// A nesto je cekirano ako je
        /// 1. samo rucno n cekirano 2. samo prikaz Promena cekiran 3. ili i rucno n i prikaz Promena grafika cekiran
        /// </summary>
        private void setMainImageIfSomethingChecked(string pathOfgraphicPlotting)
        {
            //odavde se crtaju slike
            PointD p1 = new PointD(0.2, 8);
            PointD p2 = new PointD(8, 8);


            PdfImage Image1 = new PdfImage(Document, pathOfgraphicPlotting);
            SizeD ImageSize = Image1.ImageSizeAndDensity(PageWidth, 4.7, 500);
            //Contents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
            Contents.DrawImage(Image1, 0.55, p1.Y - 3.8 + 0.3, ImageSize.Width - 2.8, ImageSize.Height);

            //draw logo
            PdfImage Image2 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\logoLjig.png");
            SizeD ImageSize2 = Image2.ImageSizeAndDensity(PageWidth, 4.7, 500);
            //Contents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
            Contents.DrawImage(Image2, 5.5, 10.0 + 0.3, 2, 1);
        }
        /// <summary>
        /// OVA METODA SE KORISIT SAMO KOD DUGMETA [UCITAJ IZVESTAJ]
        /// </summary>
        private void setChangeOfRAndEData(string pathOfgraphicChangeOfR, string pathOfgraphicChangeOfE)
        {
            //odavde se crtaju slike
            PointD p1 = new PointD(0.2, 8);
            PointD p2 = new PointD(8, 8);

            PdfImage Image2 = new PdfImage(Document, pathOfgraphicChangeOfR);
            SizeD ImageSize2 = Image2.ImageSizeAndDensity(5.5, 1.2, 500);
            Contents.DrawImage(Image2, 7, p1.Y - 0.45 + 0.3, ImageSize2.Width, ImageSize2.Height);

            PdfImage Image3 = new PdfImage(Document, pathOfgraphicChangeOfE);
            SizeD ImageSize3 = Image3.ImageSizeAndDensity(5.5, 1.2, 500);
            Contents.DrawImage(Image3, 7, p1.Y - 1.65 + 0.3, ImageSize3.Width, ImageSize3.Height);
        }

        /// <summary>
        /// OVA METODA SE KORISIT SAMO KOD DUGMETA [UCITAJ IZVESTAJ]
        /// </summary>
        private void setManualNData(string r1, string r2, string r3, string r4, string r5, string manualn, string begInterval, string endInterval) 
        {
            //odavde podaci za rucno n
            Contents.DrawText(ArialNormal, 8, 6.8, 6.05, TextJustify.Left, " R1");
            Contents.DrawLine(7.1, 6.2 + 0.3, 7.9, 6.2 + 0.3);
            Contents.DrawLine(7.1, 6.0 + 0.3, 7.9, 6.0 + 0.3);
            Contents.DrawLine(7.1, 6.2 + 0.3, 7.1, 6.0 + 0.3);
            Contents.DrawLine(7.9, 6.2 + 0.3, 7.9, 6.0 + 0.3);

            widthR1 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, r1, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 6.05 + 0.3, TextJustify.Left, r1);
            Contents.DrawText(ArialNormal, 8, 7.9, 6.05 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.80 + 0.3, TextJustify.Left, " R2");
            Contents.DrawLine(7.1, 5.95 + 0.3, 7.9, 5.95 + 0.3);
            Contents.DrawLine(7.1, 5.75 + 0.3, 7.9, 5.75 + 0.3);
            Contents.DrawLine(7.1, 5.95 + 0.3, 7.1, 5.75 + 0.3);
            Contents.DrawLine(7.9, 5.95 + 0.3, 7.9, 5.75 + 0.3);

            widthR2 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, r2, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.80 + 0.3, TextJustify.Left, r2);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.80 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.55 + 0.3, TextJustify.Left, " R3");
            Contents.DrawLine(7.1, 5.7 + 0.3, 7.9, 5.7 + 0.3);
            Contents.DrawLine(7.1, 5.5 + 0.3, 7.9, 5.5 + 0.3);
            Contents.DrawLine(7.1, 5.7 + 0.3, 7.1, 5.5 + 0.3);
            Contents.DrawLine(7.9, 5.7 + 0.3, 7.9, 5.5 + 0.3);

            widthR3 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, r3, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.55 + 0.3, TextJustify.Left, r3);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.55 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.30 + 0.3, TextJustify.Left, " R4");
            Contents.DrawLine(7.1, 5.45 + 0.3, 7.9, 5.45 + 0.3);
            Contents.DrawLine(7.1, 5.25 + 0.3, 7.9, 5.25 + 0.3);
            Contents.DrawLine(7.1, 5.45 + 0.3, 7.1, 5.25 + 0.3);
            Contents.DrawLine(7.9, 5.45 + 0.3, 7.9, 5.25 + 0.3);

            widthR4 = Contents.DrawText(ArialNormal, 8, 0, 0, TextJustify.Left, r4, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.30, TextJustify.Left, r4);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.30 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.05 + 0.3, TextJustify.Left, " R5");
            Contents.DrawLine(7.1, 5.2 + 0.3, 7.9, 5.2 + 0.3);
            Contents.DrawLine(7.1, 5.0 + 0.3, 7.9, 5.0 + 0.3);
            Contents.DrawLine(7.1, 5.2 + 0.3, 7.1, 5.0 + 0.3);
            Contents.DrawLine(7.9, 5.2 + 0.3, 7.9, 5.0 + 0.3);

            widthR5 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, r5, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.05 + 0.3, TextJustify.Left, r5);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.05 + 0.3, TextJustify.Left, " MPa");
            //ovde ide ispod a ne u produzetku
            Contents.DrawText(ArialNormal, 8, 6.95, 4.80 + 0.3, TextJustify.Left, " rucno n");
            Contents.DrawLine(7, 4.75 + 0.3, 7.8, 4.75 + 0.3);
            Contents.DrawLine(7, 4.55 + 0.3, 7.8, 4.55 + 0.3);
            Contents.DrawLine(7, 4.75 + 0.3, 7, 4.55 + 0.3);
            Contents.DrawLine(7.8, 4.75 + 0.3, 7.8, 4.55 + 0.3);
            widthRucnoN = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, manualn, false);
            Contents.DrawText(ArialNormal, 8, 7.8 - widthRucnoN - 0.05, 4.60 + 0.3, TextJustify.Left, manualn);

            Contents.DrawText(ArialNormal, 15, 6.9, 4.45 + 0.3, TextJustify.Left, "{");
            Contents.DrawLine(7, 4.50 + 0.3, 7.8, 4.50 + 0.3);
            Contents.DrawLine(7, 4.30 + 0.3, 7.8, 4.30 + 0.3);
            Contents.DrawLine(7, 4.50 + 0.3, 7, 4.30 + 0.3);
            Contents.DrawLine(7.8, 4.50 + 0.3, 7.8, 4.30 + 0.3);
            widthRucnoNInterval = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, begInterval + "-" + endInterval + "%", false);
            Contents.DrawText(ArialNormal, 8, 7.8 - widthRucnoNInterval - 0.05, 4.35 + 0.3, TextJustify.Left, begInterval + "-" + endInterval + "%");
        }

        /// <summary>
        /// ovako se postavlja slika glavnog grafika u slucaju da je nesto cekirano.
        /// A nesto je cekirano ako je
        /// 1. samo rucno n cekirano 2. samo prikaz Promena cekiran 3. ili i rucno n i prikaz Promena grafika cekiran
        /// </summary>
        private void setMainImageIfSomethingChecked() 
        {
            //odavde se crtaju slike
            PointD p1 = new PointD(0.2, 8);
            PointD p2 = new PointD(8, 8);


            PdfImage Image1 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");
            SizeD ImageSize = Image1.ImageSizeAndDensity(PageWidth, 4.7, 500);
            //Contents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
            Contents.DrawImage(Image1, 0.55, p1.Y - 3.8 + 0.3, ImageSize.Width - 2.8, ImageSize.Height);


            //draw logo
            PdfImage Image2 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\logoLjig.png");
            SizeD ImageSize2 = Image2.ImageSizeAndDensity(PageWidth, 4.7, 500);
            //Contents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
            Contents.DrawImage(Image2, 5.5, 10.0 + 0.3, 2, 1);
        }

        private void setChangeOfRAndEData() 
        {
            //odavde se crtaju slike
            PointD p1 = new PointD(0.2, 8);
            PointD p2 = new PointD(8, 8);

            PdfImage Image2 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\graphicChangeOfRTest1.png");
            SizeD ImageSize2 = Image2.ImageSizeAndDensity(1.5, 1.2, 500);
            Contents.DrawImage(Image2, 7, p1.Y - 0.45 + 0.3, ImageSize2.Width, ImageSize2.Height);

            PdfImage Image3 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\graphicChangeOfETest1.png");
            SizeD ImageSize3 = Image3.ImageSizeAndDensity(1.5, 1.2, 500);
            Contents.DrawImage(Image3, 7, p1.Y - 1.65 + 0.3, ImageSize3.Width, ImageSize3.Height);
        }

        private void setManualNData() 
        {
            //odavde podaci za rucno n
            Contents.DrawText(ArialNormal, 8, 6.8, 6.05 + 0.3, TextJustify.Left, " R1");
            Contents.DrawLine(7.1, 6.2 + 0.3, 7.9, 6.2 + 0.3);
            Contents.DrawLine(7.1, 6.0 + 0.3, 7.9, 6.0 + 0.3);
            Contents.DrawLine(7.1, 6.2 + 0.3, 7.1, 6.0 + 0.3);
            Contents.DrawLine(7.9, 6.2 + 0.3, 7.9, 6.0 + 0.3);

            widthR1 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.R1, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 6.05 + 0.3, TextJustify.Left, LastInputOutputSavedData.R1);
            Contents.DrawText(ArialNormal, 8, 7.9, 6.05 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.80 + 0.3, TextJustify.Left, " R2");
            Contents.DrawLine(7.1, 5.95 + 0.3, 7.9, 5.95 + 0.3);
            Contents.DrawLine(7.1, 5.75 + 0.3, 7.9, 5.75 + 0.3);
            Contents.DrawLine(7.1, 5.95 + 0.3, 7.1, 5.75 + 0.3);
            Contents.DrawLine(7.9, 5.95 + 0.3, 7.9, 5.75 + 0.3);

            widthR2 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.R2, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.80 + 0.3, TextJustify.Left, LastInputOutputSavedData.R2);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.80 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.55 + 0.3, TextJustify.Left, " R3");
            Contents.DrawLine(7.1, 5.7 + 0.3, 7.9, 5.7 + 0.3);
            Contents.DrawLine(7.1, 5.5 + 0.3, 7.9, 5.5 + 0.3);
            Contents.DrawLine(7.1, 5.7 + 0.3, 7.1, 5.5 + 0.3);
            Contents.DrawLine(7.9, 5.7 + 0.3, 7.9, 5.5 + 0.3);

            widthR3 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.R3, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.55 + 0.3, TextJustify.Left, LastInputOutputSavedData.R3);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.55 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.30 + 0.3, TextJustify.Left, " R4");
            Contents.DrawLine(7.1, 5.45 + 0.3, 7.9, 5.45 + 0.3);
            Contents.DrawLine(7.1, 5.25 + 0.3, 7.9, 5.25 + 0.3);
            Contents.DrawLine(7.1, 5.45 + 0.3, 7.1, 5.25 + 0.3);
            Contents.DrawLine(7.9, 5.45 + 0.3, 7.9, 5.25 + 0.3);

            widthR4 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.R4, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.30 + 0.3, TextJustify.Left, LastInputOutputSavedData.R4);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.30 + 0.3, TextJustify.Left, " MPa");

            Contents.DrawText(ArialNormal, 8, 6.8, 5.05 + 0.3, TextJustify.Left, " R5");
            Contents.DrawLine(7.1, 5.2 + 0.3, 7.9, 5.2 + 0.3);
            Contents.DrawLine(7.1, 5.0 + 0.3, 7.9, 5.0 + 0.3);
            Contents.DrawLine(7.1, 5.2 + 0.3, 7.1, 5.0 + 0.3);
            Contents.DrawLine(7.9, 5.2 + 0.3, 7.9, 5.0 + 0.3);

            widthR5 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.R5, false);
            Contents.DrawText(ArialNormal, 8, 7.9 - widthR1 - 0.05, 5.05 + 0.3, TextJustify.Left, LastInputOutputSavedData.R5);
            Contents.DrawText(ArialNormal, 8, 7.9, 5.05 + 0.3, TextJustify.Left, " MPa");
            //ovde ide ispod a ne u produzetku
            Contents.DrawText(ArialNormal, 8, 6.95, 4.80 + 0.3, TextJustify.Left, " ručno n");
            Contents.DrawLine(7, 4.75 + 0.3, 7.8, 4.75 + 0.3);
            Contents.DrawLine(7, 4.55 + 0.3, 7.8, 4.55 + 0.3);
            Contents.DrawLine(7, 4.75 + 0.3, 7, 4.55 + 0.3);
            Contents.DrawLine(7.8, 4.75 + 0.3, 7.8, 4.55 + 0.3);
            widthRucnoN = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.Manualn, false);
            Contents.DrawText(ArialNormal, 8, 7.8 - widthRucnoN - 0.05, 4.60 + 0.3, TextJustify.Left, LastInputOutputSavedData.Manualn);

            Contents.DrawText(ArialNormal, 15, 6.9, 4.45 + 0.3, TextJustify.Left, "{");
            Contents.DrawLine(7, 4.50 + 0.3, 7.8, 4.50 + 0.3);
            Contents.DrawLine(7, 4.30 + 0.3, 7.8, 4.30 + 0.3);
            Contents.DrawLine(7, 4.50 + 0.3, 7, 4.30 + 0.3);
            Contents.DrawLine(7.8, 4.50 + 0.3, 7.8, 4.30 + 0.3);
            widthRucnoNInterval = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, OptionsInPlottingMode.BeginIntervalForN + "-" + OptionsInPlottingMode.EndIntervalForN + "%", false);
            Contents.DrawText(ArialNormal, 8, 7.8 - widthRucnoNInterval - 0.05, 4.35 + 0.3, TextJustify.Left, OptionsInPlottingMode.BeginIntervalForN + "-" + OptionsInPlottingMode.EndIntervalForN + "%");

        }

       

        public void CreatePage1Contents(bool makeFromLastSample = true, string xmlName = "")
        {
            _makeFromLastSample = makeFromLastSample;

            
           

            if (makeFromLastSample == true)
            {

                //xmlName = Constants.sampleReportFilepath;
               
                //obavezno obrisati

        

                if (plotting.Printscreen.chbCalculateNManual.IsChecked == false && plotting.Printscreen.chbChangeOfRAndE.IsChecked == false)
                {
                    PdfImage Image1 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\graphicPlottingTest1.png");
                    SizeD ImageSize = Image1.ImageSizeAndDensity(PageWidth, 4.7, 500);
                    //SizeD ImageSize = Image1.ImageSizeAndDensity(DispWidth, 2 * AreaHeight, 150.0);

                    PointD p1 = new PointD(0.2, 8);
                    PointD p2 = new PointD(8, 8);
                    // add contents to page
                    Contents = AddPageToDocument(1);
                    //Contents.DrawText(ArialBold, 14, 0.5 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2,TextJustify.Center, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");
                    //Contents.DrawText(ArialBold, 10, 0.05 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2, TextJustify.Left, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");
                    xmlName = Properties.Settings.Default.sampleReportFilepath;
                    Contents.DrawText(ArialBold, 8, 0.5, 0.2, TextJustify.Left, "LJ. OB.__________________");
                    Contents.DrawText(ArialBold, 8, 0.95, 0.21, TextJustify.Left, OptionsFooter.brojObrasca);
                    Contents.DrawText(ArialBold, 8, 2.05, 0.21, TextJustify.Left, "Verzija " + OptionsFooter.brojVerzije);
                    widthbrojVerzije = Contents.DrawText(ArialNormal, 8, 0, 0, TextJustify.Left, "Verzija " + OptionsFooter.brojVerzije, false);
                    Contents.DrawText(ArialBold, 8, 2.05 + widthbrojVerzije, 0.21, TextJustify.Left, " /" + OptionsFooter.Godina);
                    Contents.DrawText(ArialBold, 8, 6, 0.2, TextJustify.Left, "Fajl : ");
                    if (xmlName.Equals("") || xmlName.Contains("izvestajOnline"))
                    {
                        Contents.DrawText(ArialBold, 8, 6.5, 0.2, TextJustify.Left, "LJOB" + OptionsFooter.brojObrasca + "." + onlineHeader.GeneralData.tfBrUzorka.Text + "_" + onlineHeader.GeneralData.tfBrUzorkaNumberOfSample.Text + ".pdf");
                    }
                    else
                    {
                        List<string> xmlListNames = xmlName.Split('\\').ToList();
                        List<string> names = xmlListNames[3].Split('.').ToList();
                        Contents.DrawText(ArialBold, 8, 6.5, 0.2, TextJustify.Left, "LJOB" + OptionsFooter.brojObrasca + "." + names[0] + ".pdf");
                    }
                    Contents.DrawText(ArialBold, 14, 0.05 * PageWidth, 11.5, TextJustify.Left, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");
                    //Contents.DrawImage(Image1, 2,4, ImageSize.Width, ImageSize.Height);
                    Contents.DrawLine(0.5, 8.95 + 0.3, 0.5, 4.10 + 0.3);
                    Contents.DrawLine(8.25, 8.95 + 0.3, 8.25, 4.10 + 0.3);
                    Contents.DrawLine(0.5, 4.10 + 0.3, 8.25, 4.10 + 0.3);
                    Contents.DrawLine(0.5, 8.95 + 0.3, 8.25, 8.95 + 0.3);
                    //Contents.DrawImage(Image1, 1.15, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
                    Contents.DrawImage(Image1, 0.55, p1.Y - 3.8 + 0.3, ImageSize.Width - 2.1, ImageSize.Height);


                    //draw logo
                    PdfImage Image2 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\logoLjig.png");
                    SizeD ImageSize2 = Image2.ImageSizeAndDensity(PageWidth, 4.7, 500);
                    //Contents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
                    Contents.DrawImage(Image2, 5.5, 10.0 + 0.3, 2, 1);
                   
                }
                else 
                {
                   
                    //ovo se izvrsava [ crta se slika grafika ] kada se klikne da dugme Napravi Izvestaj
                    //add contents to page
                    Contents = AddPageToDocument(1);
                    Contents.DrawText(ArialBold, 8, 0.5, 0.2, TextJustify.Left, "LJ. OB.__________________");
                    Contents.DrawText(ArialBold, 8, 0.95, 0.21, TextJustify.Left, OptionsFooter.brojObrasca);
                    Contents.DrawText(ArialBold, 8, 2.05, 0.21, TextJustify.Left, "Verzija " + OptionsFooter.brojVerzije);
                    widthbrojVerzije = Contents.DrawText(ArialNormal, 8, 0, 0, TextJustify.Left, "Verzija " + OptionsFooter.brojVerzije, false);
                    Contents.DrawText(ArialBold, 8, 2.05 + widthbrojVerzije, 0.21, TextJustify.Left, " /" + OptionsFooter.Godina);
                    Contents.DrawText(ArialBold, 8, 6, 0.2, TextJustify.Left, "Fajl : ");

                    if (xmlName.Equals("") || xmlName.Contains("izvestajOnline"))
                    {
                        Contents.DrawText(ArialBold, 8, 6.5, 0.2, TextJustify.Left, "LJOB" + OptionsFooter.brojObrasca + "." + onlineHeader.GeneralData.tfBrUzorka.Text + "_" + onlineHeader.GeneralData.tfBrUzorkaNumberOfSample.Text + ".pdf");
                    }
                    else
                    {
                        List<string> xmlListNames = xmlName.Split('\\').ToList();
                        List<string> names = xmlListNames[3].Split('.').ToList();
                        Contents.DrawText(ArialBold, 8, 6.5, 0.2, TextJustify.Left, "LJOB" + OptionsFooter.brojObrasca + "." + names[0] + ".pdf");
                    }
                    Contents.DrawText(ArialBold, 14, 0.05 * PageWidth, 11.5, TextJustify.Left, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");
                    Contents.DrawLine(0.5, 8.95 + 0.3, 0.5, 4.10 + 0.3);
                    Contents.DrawLine(8.25, 8.95 + 0.3, 8.25, 4.10 + 0.3);
                    Contents.DrawLine(0.5, 4.10 + 0.3, 8.25, 4.10 + 0.3);
                    Contents.DrawLine(0.5, 8.95 + 0.3, 8.25, 8.95 + 0.3);

                    if (plotting.Printscreen.chbChangeOfRAndE.IsChecked == true && plotting.Printscreen.chbCalculateNManual.IsChecked == false)
                    {
                        setMainImageIfSomethingChecked();
                        setChangeOfRAndEData();
                    }

                    if (plotting.Printscreen.chbCalculateNManual.IsChecked == true && plotting.Printscreen.chbChangeOfRAndE.IsChecked == false)
                    {
                        setMainImageIfSomethingChecked();
                        setManualNData();
                    }

                    if (plotting.Printscreen.chbChangeOfRAndE.IsChecked == true && plotting.Printscreen.chbCalculateNManual.IsChecked == true)
                    {
                        setMainImageIfSomethingChecked();
                        setChangeOfRAndEData();
                        setManualNData();
                    }
                }
            }
            else
            {
                //ovo se izvrsava [ crta se slika grafika ] kada se klikne da dugme Ucitaj Izvestaj

                // Get file name.
                string name = xmlName;
                //GetAutomaticAnimation file name
                string namePng = name.Split('.').ElementAt(0);
                namePng += ".png";
                string namePromenaNaponaPng = name.Split('.').ElementAt(0);
                namePromenaNaponaPng += "PromenaNapona.png";
                string namePromenaIzduzenjaPng = name.Split('.').ElementAt(0);
                namePromenaIzduzenjaPng += "PromenaIzduzenja.png";

                //odredi dali treba da se prikazuju grafici promene iz ucitanog fajla ili da li treba da se prikazu podaci za rucno racunano n
                string showChangeOfRAndE = string.Empty;
                string isManualNCalculated = string.Empty;
                
                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(name).ToList();
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
                    MessageBox.Show(" Ucitali ste fajl sa pogrešnim formatom !! ");
                    return;
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(myXmlString);
                XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);

                foreach (XmlNode xn in xnList)
                {
                    showChangeOfRAndE = xn[Constants.XML_ShowGraphicChangeOfRAndE].InnerText;
                    isManualNCalculated = xn[Constants.XML_ManualnIsCalculated].InnerText;
                }
                
                
                // add contents to page
                Contents = AddPageToDocument(1);
                //Contents.DrawText(ArialBold, 14, 0.5 * PageWidth, PageHeight - Margin - (HeadingHeight - ArialBold.CapHeight(24)) / 2,TextJustify.Center, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");
                Contents.DrawText(ArialBold, 8, 0.5, 0.21, TextJustify.Left, "LJ. OB.__________________");
                Contents.DrawText(ArialBold, 8, 0.95, 0.21, TextJustify.Left, OptionsFooter.brojObrasca);
                Contents.DrawText(ArialBold, 8, 2.05, 0.21, TextJustify.Left, "Verzija " + OptionsFooter.brojVerzije);
                widthbrojVerzije = Contents.DrawText(ArialNormal, 8, 0, 0, TextJustify.Left, "Verzija " + OptionsFooter.brojVerzije, false);
                Contents.DrawText(ArialBold, 8, 2.05 + widthbrojVerzije, 0.21, TextJustify.Left, " /" + OptionsFooter.Godina);
                Contents.DrawText(ArialBold, 8, 6, 0.2, TextJustify.Left, "Fajl : ");

                if (xmlName.Equals("") || xmlName.Contains("izvestajOnline"))
                {
                    Contents.DrawText(ArialBold, 8, 6.5, 0.2, TextJustify.Left, "LJOB" + OptionsFooter.brojObrasca + "." + onlineHeader.GeneralData.tfBrUzorka.Text + "_" + onlineHeader.GeneralData.tfBrUzorkaNumberOfSample.Text + ".pdf");
                }
                else
                {
                    List<string> xmlListNames = xmlName.Split('\\').ToList();
                    List<string> names = xmlListNames[3].Split('.').ToList();
                    Contents.DrawText(ArialBold, 8, 6.5, 0.2, TextJustify.Left, "LJOB" + OptionsFooter.brojObrasca + "." + names[0] + ".pdf");
                }
                Contents.DrawText(ArialBold, 14, 0.05 * PageWidth, 11.5, TextJustify.Left, "IZVEŠTAJ O ISPITIVANJU MEHANIČKIH OSOBINA");
                //Contents.DrawImage(Image1, 2,4, ImageSize.Width, ImageSize.Height);
                Contents.DrawLine(0.5, 8.95 + 0.3, 0.5, 4.10 + 0.3);
                Contents.DrawLine(8.25, 8.95 + 0.3, 8.25, 4.10 + 0.3);
                Contents.DrawLine(0.5, 4.10 + 0.3, 8.25, 4.10 + 0.3);
                Contents.DrawLine(0.5, 8.95 + 0.3, 8.25, 8.95 + 0.3);

                //if (plotting.Printscreen.chbCalculateNManual.IsChecked == false && plotting.Printscreen.chbChangeOfRAndE.IsChecked == false)
                if (isManualNCalculated.Equals("False") && showChangeOfRAndE.Equals("False"))
                {
                    PdfImage Image1 = new PdfImage(Document, namePng);
                    SizeD ImageSize = Image1.ImageSizeAndDensity(PageWidth, 4.7, 500);
                    //SizeD ImageSize = Image1.ImageSizeAndDensity(DispWidth, 2 * AreaHeight, 150.0);

                    PointD p1 = new PointD(0.2, 8);
                    PointD p2 = new PointD(8, 8);
                   
                    //Contents.DrawImage(Image1, 1.15, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
                    Contents.DrawImage(Image1, 0.55, p1.Y - 3.8 + 0.3, ImageSize.Width - 2.1, ImageSize.Height);


                    //draw logo
                    PdfImage Image2 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\logoLjig.png");
                    SizeD ImageSize2 = Image2.ImageSizeAndDensity(PageWidth, 4.7, 500);
                    //Contents.DrawImage(Image1, 0.95, p1.Y - 3.8, ImageSize.Width - 1, ImageSize.Height);
                    Contents.DrawImage(Image2, 5.5, 10.0 + 0.3, 2, 1);
                }
                //else if (plotting.Printscreen.chbCalculateNManual.IsChecked == false && plotting.Printscreen.chbChangeOfRAndE.IsChecked == true)
                else if (isManualNCalculated.Equals("False") && showChangeOfRAndE.Equals("True"))
                {
                    //odavde se crtaju slike
                    setMainImageIfSomethingChecked(namePng);
                    setChangeOfRAndEData(namePromenaNaponaPng, namePromenaIzduzenjaPng);
                }
                else if (isManualNCalculated.Equals("True") && showChangeOfRAndE.Equals("False"))
                {
                    setMainImageIfSomethingChecked(namePng);
                    string r1 = String.Empty;
                    string r2 = String.Empty;
                    string r3 = String.Empty;
                    string r4 = String.Empty;
                    string r5 = String.Empty;
                    string manualn = String.Empty;
                    string begInterval = String.Empty;
                    string endInterval = String.Empty;

                    foreach (XmlNode xn in xnList)
                    {
                        r1 = xn[Constants.XML_R1].InnerText;
                        r2 = xn[Constants.XML_R2].InnerText;
                        r3 = xn[Constants.XML_R3].InnerText;
                        r4 = xn[Constants.XML_R4].InnerText;
                        r5 = xn[Constants.XML_R5].InnerText;
                        manualn = xn[Constants.XML_manualN].InnerText;
                        begInterval = xn[Constants.XML_manualN_BeginInterval].InnerText;
                        endInterval = xn[Constants.XML_manualN_EndInterval].InnerText;
                    }

                    setManualNData(r1, r2, r3, r4, r5, manualn, begInterval, endInterval);
                }
                else if (isManualNCalculated.Equals("True") && showChangeOfRAndE.Equals("True"))
                {
                    setMainImageIfSomethingChecked(namePng);
                    setChangeOfRAndEData(namePromenaNaponaPng, namePromenaIzduzenjaPng);
                    string r1 = String.Empty;
                    string r2 = String.Empty;
                    string r3 = String.Empty;
                    string r4 = String.Empty;
                    string r5 = String.Empty;
                    string manualn = String.Empty;
                    string begInterval = String.Empty;
                    string endInterval = String.Empty;

                    foreach (XmlNode xn in xnList)
                    {
                        r1 = xn[Constants.XML_R1].InnerText;
                        r2 = xn[Constants.XML_R2].InnerText;
                        r3 = xn[Constants.XML_R3].InnerText;
                        r4 = xn[Constants.XML_R4].InnerText;
                        r5 = xn[Constants.XML_R5].InnerText;
                        manualn = xn[Constants.XML_manualN].InnerText;
                        begInterval = xn[Constants.XML_manualN_BeginInterval].InnerText;
                        endInterval = xn[Constants.XML_manualN_EndInterval].InnerText;
                    }

                    setManualNData(r1, r2, r3, r4, r5, manualn, begInterval, endInterval);
                }
            }

            //upper part of report sample

            setLongestFourParts();

            //if (onlineHeader.GeneralData != null && onlineHeader.MaterialForTesting != null)
            //{
                setFirstColumn_UpperPart();
            //}
            //if (onlineHeader.MaterialForTesting != null)
            //{
                setSecondColumn_UpperPart();
            //}
            //if (onlineHeader.GeneralData != null && onlineHeader.ConditionsOfTesting != null)
            //{
                setThirdColumn_UpperPart();
            //}
            //if (onlineHeader.ConditionsOfTesting != null)
            //{
                //setFourthColumn_UpperPart();
            //}
            //if (onlineHeader.ConditionsOfTesting != null)
            //{
            //    setConditionsOfTesting();
            //}
            //if (onlineHeader.MaterialForTesting != null)
            //{
            //    setMaterialForTesting();
            //}
            //upper part of report sample

            //graphic footnote

            setFootnote();

            //graphic footnote

            //lower part of report sample
            //if(onlineHeader != null)
            //{
                setKindOfTube();
            //}
            //if (onlineHeader.PositionOfTube != null)
            //{
                setPositionOfTube();
            //}
            //if (onlineHeader != null && plotting != null && resultsInterface != null)
            //{
                setTubeDimension(makeFromLastSample, xmlName);
            //}
            //if (resultsInterface != null)
            //{
                setOutputProperties(makeFromLastSample, xmlName);//postavljanje podataka iz izlaznog interfejsa [ResultsInterface]
            //}

            //if (onlineHeader.RemarkOfTesting != null)
            //{
                setRemarksOfTesting();
            //}


            //if (resultsInterface != null)
            //{
                setFFmAndChangedParameters();
            //}

            //if (onlineHeader.GeneralData != null)
            //{
                setPrintingTimeAndOperator();
            //}

            setPrintingTimeAndOperator();

            //lower part of report sample

            // exit
            return;
        }


        #region upperPart

        /// <summary>
        /// standard za ispitivanje zatezanja
        /// metoda
        /// standard za n
        /// datum ispitivanja
        /// </summary>
        private void setLongestFourParts() 
        {
            Contents.DrawText(ArialNormal, 8, 0.5, 10.45 + 0.3, TextJustify.Left, "ST. ZA ISP. ZAT.");
            Contents.DrawLine(1.5, 10.6 + 0.3, 5, 10.6 + 0.3);
            Contents.DrawLine(1.5, 10.43 + 0.3, 5, 10.43 + 0.3);
            Contents.DrawLine(1.5, 10.6 + 0.3, 1.5, 10.43 + 0.3);
            Contents.DrawLine(5, 10.6 + 0.3, 5, 10.43 + 0.3);
            Contents.DrawText(ArialNormal, 8, 1.5, 10.45 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfStandard_ConditionsOfTesting);


            Contents.DrawText(ArialNormal, 8, 0.5, 10.25 + 0.3, TextJustify.Left, "METODA");
            Contents.DrawLine(1.5, 10.4 + 0.3, 2.0, 10.4 + 0.3);
            Contents.DrawLine(1.5, 10.23 + 0.3, 2.0, 10.23 + 0.3);
            Contents.DrawLine(1.5, 10.4 + 0.3, 1.5, 10.23 + 0.3);
            Contents.DrawLine(2.0, 10.4 + 0.3, 2.0, 10.23 + 0.3);
            Contents.DrawText(ArialNormal, 8, 1.5, 10.25 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfMetoda_ConditionsOfTesting);

            var celzijusStepeni = " \xB0" + String.Empty + "C";
            Contents.DrawText(ArialNormal, 8, 2.1, 10.25 + 0.3, TextJustify.Left, "TEMPERATURA");
            Contents.DrawLine(3.0, 10.4 + 0.3, 3.5, 10.4 + 0.3);
            Contents.DrawLine(3.0, 10.23 + 0.3, 3.5, 10.23 + 0.3);
            Contents.DrawLine(3.0, 10.4 + 0.3, 3.0, 10.23 + 0.3);
            Contents.DrawLine(3.5, 10.4 + 0.3, 3.5, 10.23 + 0.3);

            Contents.DrawText(ArialNormal, 8, 3.0, 10.25 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting + celzijusStepeni);

            Contents.DrawText(ArialNormal, 8, 3.6, 10.25 + 0.3, TextJustify.Left, "EKSTENZ.");
            Contents.DrawLine(4.3, 10.4 + 0.3, 5, 10.4 + 0.3);
            Contents.DrawLine(4.3, 10.23 + 0.3, 5, 10.23 + 0.3);
            Contents.DrawLine(4.3, 10.4 + 0.3, 4.3, 10.23 + 0.3);
            Contents.DrawLine(5, 10.4 + 0.3, 5, 10.23 + 0.3);

            //if (onlineHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 4.3, 10.25 + 0.3, TextJustify.Left, " DA");
            }
            //}
            //if (onlineHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 4.3, 10.25 + 0.3, TextJustify.Left, " NE");
            }
            //}



            Contents.DrawText(ArialNormal, 8, 0.5, 10.05 + 0.3, TextJustify.Left, "STAND. ZA N");
            Contents.DrawLine(1.5, 10.2 + 0.3, 5, 10.2 + 0.3);
            Contents.DrawLine(1.5, 10.03 + 0.3, 5, 10.03 + 0.3);
            Contents.DrawLine(1.5, 10.2 + 0.3, 1.5, 10.03 + 0.3);
            Contents.DrawLine(5, 10.2 + 0.3, 5, 10.03 + 0.3);
            Contents.DrawText(ArialNormal, 8, 1.5, 10.05 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting);//6.125 + 1 = 7.125


            Contents.DrawText(ArialNormal, 8, 0.5, 9.85 + 0.3, TextJustify.Left, "DATUM ISPITIV.");
            Contents.DrawLine(1.5, 10 + 0.3, 5, 10 + 0.3);
            Contents.DrawLine(1.5, 9.83 + 0.3, 5, 9.83 + 0.3);
            Contents.DrawLine(1.5, 10 + 0.3, 1.5, 9.83 + 0.3);
            Contents.DrawLine(5, 10 + 0.3, 5, 9.83 + 0.3);
            Contents.DrawText(ArialNormal, 8, 1.5, 9.85 + 0.3, TextJustify.Left, " " + DateTime.Now.ToShortDateString());

        }

        /// <summary>
        /// sirina ide od 0.5 do 2.375 (razmak je sirine 0.175) [ukupno 1.875]
        /// fikisni deo sirina 1, nefiksni 0.7 
        /// </summary>
        private void setFirstColumn_UpperPart() 
        {

            Contents.DrawText(ArialNormal, 8, 0.5, 9.65 + 0.3, TextJustify.Left, "BR ZBIR. IZVEŠ.");
            Contents.DrawLine(1.5, 9.8 + 0.3, 2.2 + 0.6, 9.8 + 0.3);
            Contents.DrawLine(1.5, 9.63 + 0.3, 2.2 + 0.6, 9.63 + 0.3);
            Contents.DrawLine(1.5, 9.8 + 0.3, 1.5, 9.63 + 0.3);
            Contents.DrawLine(2.2 + 0.6, 9.8 + 0.3, 2.2 + 0.6, 9.63 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 1.5, 9.65, TextJustify.Left, " " + onlineHeader.GeneralData.tfBrZbIzvestaja.Text);//0.5 + 1 = 1.5 
            Contents.DrawText(ArialNormal, 8, 1.5, 9.65 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfBrZbIzvestaja_GeneralData);//0.5 + 1 = 1.5 

            Contents.DrawText(ArialNormal, 8, 0.5, 9.45 + 0.3, TextJustify.Left, "BR. UZORKA");
            Contents.DrawLine(1.5, 9.6 + 0.3, 2.2 + 0.6, 9.6 + 0.3);
            Contents.DrawLine(1.5, 9.43 + 0.3, 2.2 + 0.6, 9.43 + 0.3);
            Contents.DrawLine(1.5, 9.6 + 0.3, 1.5, 9.43 + 0.3);
            Contents.DrawLine(2.2 + 0.6, 9.6 + 0.3, 2.2 + 0.6, 9.43 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 1.5, 9.45, TextJustify.Left, " " + onlineHeader.GeneralData.tfBrUzorka.Text);
            Contents.DrawText(ArialNormal, 8, 1.5, 9.45 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfBrUzorka_GeneralData);


            Contents.DrawText(ArialNormal, 8, 0.5, 9.25 + 0.3, TextJustify.Left, "ŠARŽA");
            Contents.DrawLine(1.5, 9.4 + 0.3, 2.2 + 0.6, 9.4 + 0.3);
            Contents.DrawLine(1.5, 9.23 + 0.3, 2.2 + 0.6, 9.23 + 0.3);
            Contents.DrawLine(1.5, 9.4 + 0.3, 1.5, 9.23 + 0.3);
            Contents.DrawLine(2.2 + 0.6, 9.4 + 0.3, 2.2 + 0.6, 9.23 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 1.5, 9.25, TextJustify.Left, " " + onlineHeader.GeneralData.tfSarza.Text);
            Contents.DrawText(ArialNormal, 8, 1.5, 9.25 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfSarza_GeneralData);


            Contents.DrawText(ArialNormal, 8, 0.5, 9.05 + 0.3, TextJustify.Left, "NAČIN PRERADE");
            Contents.DrawLine(1.5, 9.2 + 0.3, 2.2 + 0.6, 9.2 + 0.3);
            Contents.DrawLine(1.5, 9.03 + 0.3, 2.2 + 0.6, 9.03 + 0.3);
            Contents.DrawLine(1.5, 9.2 + 0.3, 1.5, 9.03 + 0.3);
            Contents.DrawLine(2.2 + 0.6, 9.2 + 0.3, 2.2 + 0.6, 9.03 + 0.3);

            //if(onlineHeader.MaterialForTesting.rbtnValjani.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnValjani_MaterialForTesting.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 1.5, 9.05 + 0.3, TextJustify.Left, " VALJANI");
            }
            //}
            //if (onlineHeader.MaterialForTesting.rbtnVučeni.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnVuceni_MaterialForTesting.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 1.5, 9.05 + 0.3, TextJustify.Left, " VUČENI");
            }
            //}
            //if (onlineHeader.MaterialForTesting.rbtnKovani.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnKovani_MaterialForTesting.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 1.5, 9.05 + 0.3, TextJustify.Left, " KOVANI");
            }
            //}
            //if (onlineHeader.MaterialForTesting.rbtnLiveni.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnLiveni_MaterialForTesting.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 1.5, 9.05 + 0.3, TextJustify.Left, " LIVENI");
            }
            //}
            
        }

        /// <summary>
        /// sirina ide od 2.375 do 4.25 (razmak je sirine 0.175) [ukupno 1.875]
        /// fikisni deo sirina 1, nefiksni 0.7 
        /// </summary>
        private void setSecondColumn_UpperPart()
        {
            Contents.DrawText(ArialNormal, 8, 2.375 + 0.6, 9.65 + 0.3, TextJustify.Left, "PROIZVODJAČ");
            Contents.DrawLine(3.375 + 0.6, 9.8 + 0.3, 4.075 + 0.6 + 0.6, 9.8 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.63 + 0.3, 4.075 + 0.6 + 0.6, 9.63 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.8 + 0.3, 3.375 + 0.6, 9.63 + 0.3);
            Contents.DrawLine(4.075 + 0.6 + 0.6, 9.8 + 0.3, 4.075 + 0.6 + 0.6, 9.63 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.65, TextJustify.Left, " " + onlineHeader.MaterialForTesting.tfProizvodjac.Text);//2.375 + 1 = 3.375 
            Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.65 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfProizvodjac_MaterialForTesting);//2.375 + 1 = 3.375 

            Contents.DrawText(ArialNormal, 8, 2.375 + 0.6, 9.45 + 0.3, TextJustify.Left, "DOBAVLJAČ");
            Contents.DrawLine(3.375 + 0.6, 9.6 + 0.3, 4.075 + 0.6 + 0.6, 9.6 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.43 + 0.3, 4.075 + 0.6 + 0.6, 9.43 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.6 + 0.3, 3.375 + 0.6, 9.43 + 0.3);
            Contents.DrawLine(4.075 + 0.6 + 0.6, 9.6 + 0.3, 4.075 + 0.6 + 0.6, 9.43 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.45, TextJustify.Left, " " + onlineHeader.MaterialForTesting.tfDobavljac.Text);
            Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.45 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfDobavljac_MaterialForTesting);

            Contents.DrawText(ArialNormal, 8, 2.375 + 0.6, 9.25 + 0.3, TextJustify.Left, "POLAZNI KVALIT.");
            Contents.DrawLine(3.375 + 0.6, 9.4 + 0.3, 4.075 + 0.6 + 0.6, 9.4 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.23 + 0.3, 4.075 + 0.6 + 0.6, 9.23 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.4 + 0.3, 3.375 + 0.6, 9.23 + 0.3);
            Contents.DrawLine(4.075 + 0.6 + 0.6, 9.4 + 0.3, 4.075 + 0.6 + 0.6, 9.23 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.25, TextJustify.Left, " " + onlineHeader.MaterialForTesting.tfPolazniKvalitet.Text);
            Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.25 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfPolazniKvalitet_MaterialForTesting);

            Contents.DrawText(ArialNormal, 8, 2.375 + 0.6, 9.05 + 0.3, TextJustify.Left, "NAZ. DEBLJINA");
            Contents.DrawLine(3.375 + 0.6, 9.2 + 0.3, 4.075 + 0.6 + 0.6, 9.2 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.03 + 0.3, 4.075 + 0.6 + 0.6, 9.03 + 0.3);
            Contents.DrawLine(3.375 + 0.6, 9.2 + 0.3, 3.375 + 0.6, 9.03 + 0.3);
            Contents.DrawLine(4.075 + 0.6 + 0.6, 9.2 + 0.3, 4.075 + 0.6 + 0.6, 9.03 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.05, TextJustify.Left, " " + onlineHeader.MaterialForTesting.tfPolazniKvalitet.Text); 
            Contents.DrawText(ArialNormal, 8, 3.375 + 0.6, 9.05 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfNazivnaDebljina_MaterialForTesting);
        }

        /// <summary>
        /// sirina ide od 4.25 do 6.125 (razmak je sirine 0.175) [ukupno 1.875]
        /// fikisni deo sirina 1, nefiksni 0.7 
        /// </summary>
        private void setThirdColumn_UpperPart()
        {
           
            //Contents.DrawText(ArialNormal, 8, 4.25, 9.65, TextJustify.Left, "ST. ZA ISP. ZAT.");
            //Contents.DrawLine(5.25, 9.8, 5.95, 9.8);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95
            //Contents.DrawLine(5.25, 9.63, 5.95, 9.63);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95
            //Contents.DrawLine(5.25, 9.8, 5.25, 9.63);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95
            //Contents.DrawLine(5.95, 9.8, 5.95, 9.63);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95

            ////Contents.DrawText(ArialNormal, 8, 5.25, 9.65, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfStandard.Text);//4.25 + 1 = 5.25
            //Contents.DrawText(ArialNormal, 8, 5.25, 9.65, TextJustify.Left, " " + LastInputOutputSavedData.tfStandard_ConditionsOfTesting);//4.25 + 1 = 5.25

            //Contents.DrawText(ArialNormal, 8, 4.25, 9.45, TextJustify.Left, "METODA");
            //Contents.DrawLine(5.25, 9.6, 5.95, 9.6);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95
            //Contents.DrawLine(5.25, 9.43, 5.95, 9.43);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95
            //Contents.DrawLine(5.25, 9.6, 5.25, 9.43);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95
            //Contents.DrawLine(5.95, 9.6, 5.95, 9.43);//4.25 + 1 = 5.25 AND 5.25 + 0.7 = 5.95

            ////Contents.DrawText(ArialNormal, 8, 5.25, 9.45, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfMetoda.Text);//4.25 + 1 = 5.25
            //Contents.DrawText(ArialNormal, 8, 5.25, 9.45, TextJustify.Left, " " + LastInputOutputSavedData.tfMetoda_ConditionsOfTesting);//4.25 + 1 = 5.25

            Contents.DrawText(ArialNormal, 8, 4.25 + 1.2, 9.65 + 0.3, TextJustify.Left, "RADNI NALOG");
            Contents.DrawLine(5.25 + 1.2, 9.8 + 0.3, 5.95 + 1.2 + 0.6, 9.8 + 0.3);
            Contents.DrawLine(5.25 + 1.2, 9.63 + 0.3, 5.95 + 1.2 + 0.6, 9.63 + 0.3);
            Contents.DrawLine(5.25 + 1.2, 9.8 + 0.3, 5.25 + 1.2, 9.63 + 0.3);
            Contents.DrawLine(5.95 + 1.2 + 0.6, 9.8 + 0.3, 5.95 + 1.2 + 0.6, 9.63 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.65, TextJustify.Left, " " + onlineHeader.GeneralData.tfRadniNalog.Text);//4.25 + 1 = 5.25
            Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.65 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfRadniNalog_GeneralData);//4.25 + 1 = 5.25


            Contents.DrawText(ArialNormal, 8, 4.25 + 1.2, 9.45 + 0.3, TextJustify.Left, "NARUČILAC");
            Contents.DrawLine(5.25 + 1.2, 9.6 + 0.3, 5.95 + 1.2 + 0.6, 9.6 + 0.3);
            Contents.DrawLine(5.25 + 1.2, 9.43 + 0.3, 5.95 + 1.2 + 0.6, 9.43 + 0.3);
            Contents.DrawLine(5.25 + 1.2, 9.6 + 0.3, 5.25 + 1.2, 9.43 + 0.3);
            Contents.DrawLine(5.95 + 1.2 + 0.6, 9.6 + 0.3, 5.95 + 1.2 + 0.6, 9.43 + 0.3);

            //Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.45, TextJustify.Left, " " + onlineHeader.GeneralData.tfRadniNalog.Text);//4.25 + 1 = 5.25
            Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.45 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfRadniNalog_GeneralData);//4.25 + 1 = 5.25


            //var celzijusStepeni = " \xB0" + String.Empty + "C";
            //Contents.DrawText(ArialNormal, 8, 4.25 + 1.2, 9.25, TextJustify.Left, "TEMPERATURA");
            //Contents.DrawLine(5.25 + 1.2, 9.4, 5.95 + 1.2 + 0.6, 9.4);
            //Contents.DrawLine(5.25 + 1.2, 9.23, 5.95 + 1.2 + 0.6, 9.23);
            //Contents.DrawLine(5.25 + 1.2, 9.4, 5.25 + 1.2, 9.23);
            //Contents.DrawLine(5.95 + 1.2 + 0.6, 9.4, 5.95 + 1.2 + 0.6, 9.23);

            //Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.25, TextJustify.Left, " " + LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting + celzijusStepeni);


            //Contents.DrawText(ArialNormal, 8, 4.25 + 1.2, 9.05, TextJustify.Left, "EKSTENZ.");
            //Contents.DrawLine(5.25 + 1.2, 9.2, 5.95 + 1.2 + 0.6, 9.2);
            //Contents.DrawLine(5.25 + 1.2, 9.03, 5.95 + 1.2 + 0.6, 9.03);
            //Contents.DrawLine(5.25 + 1.2, 9.2, 5.25 + 1.2, 9.03);
            //Contents.DrawLine(5.95 + 1.2 + 0.6, 9.2, 5.95 + 1.2 + 0.6, 9.03);

            ////if (onlineHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
            //{
            //    Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.05, TextJustify.Left, " DA");
            //}
            ////}
            ////if (onlineHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
            //{
            //    Contents.DrawText(ArialNormal, 8, 5.25 + 1.2, 9.05, TextJustify.Left, " NE");
            //}
            ////}
           
        }


         /// <summary>
        /// sirina ide od 6.125 do 8 (razmak je sirine 0.175) [ukupno 1.875]
        /// fikisni deo sirina 1, nefiksni 0.7 
        /// </summary>
        private void setFourthColumn_UpperPart()
        {

            //var celzijusStepeni = " \xB0" + String.Empty + "C";

            //Contents.DrawText(ArialNormal, 8, 6.125, 9.65, TextJustify.Left, "TEMPERATURA");
            //Contents.DrawLine(7.125, 9.8, 7.825, 9.8);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.63, 7.825, 9.63);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.8, 7.125, 9.63);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.825, 9.8, 7.825, 9.63);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825

            ////Contents.DrawText(ArialNormal, 8, 7.125, 9.65, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfTemperatura.Text + celzijusStepeni);//6.125 + 1 = 7.125
            //Contents.DrawText(ArialNormal, 8, 7.125, 9.65, TextJustify.Left, " " + LastInputOutputSavedData.tfTemperatura_ConditionsOfTesting + celzijusStepeni);//6.125 + 1 = 7.125


            //Contents.DrawText(ArialNormal, 8, 6.125, 9.45, TextJustify.Left, "EKSTENZ.");
            //Contents.DrawLine(7.125, 9.6, 7.825, 9.6);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.43, 7.825, 9.43);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.6, 7.125, 9.43);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.825, 9.6, 7.825, 9.43);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825

            ////if (onlineHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnYes_ConditionsOfTesting.Equals("True"))
            //{
            //    Contents.DrawText(ArialNormal, 8, 7.125, 9.45, TextJustify.Left, " DA");//6.125 + 1 = 7.125
            //}
            ////}
            ////if (onlineHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnNo_ConditionsOfTesting.Equals("True"))
            //{
            //    Contents.DrawText(ArialNormal, 8, 7.125, 9.45, TextJustify.Left, " NE");//6.125 + 1 = 7.125
            //}
            ////}


            //Contents.DrawText(ArialNormal, 8, 6.125, 9.25, TextJustify.Left, "STAND. ZA N");
            //Contents.DrawLine(7.125, 9.4, 7.825, 9.4);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.23, 7.825, 9.23);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.4, 7.125, 9.23);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.825, 9.4, 7.825, 9.23);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825

            ////Contents.DrawText(ArialNormal, 8, 7.125, 9.25, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfStandardZaN.Text);//6.125 + 1 = 7.125
            //Contents.DrawText(ArialNormal, 8, 7.125, 9.25, TextJustify.Left, " " + LastInputOutputSavedData.tfStandardZaN_ConditionsOfTesting);//6.125 + 1 = 7.125


            //Contents.DrawText(ArialNormal, 8, 6.125, 9.05, TextJustify.Left, "DATUM ISPITIV.");
            //Contents.DrawLine(7.125, 9.2, 7.825, 9.2);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.03, 7.825, 9.03);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.125, 9.2, 7.125, 9.03);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825
            //Contents.DrawLine(7.825, 9.2, 7.825, 9.03);//6.125 + 1 = 7.125 AND 7.125 + 0.7 = 7.825

            //Contents.DrawText(ArialNormal, 8, 7.125, 9.05, TextJustify.Left, " " + DateTime.Now.ToShortDateString());//6.125 + 1 = 7.125
        }


        /// <summary>
        ///  sirina ide od 3 do 5.5 (razmak je sirine 0.2) [ukupno 2.5]
        ///  fikisni deo sitina 1, nefiksni 1.3 
        /// </summary>
        //private void setConditionsOfTesting() 
        //{
        //    Contents.DrawLine(3, 10.0, 5.3, 10.0);
        //    Contents.DrawLine(3, 9.8, 5.3, 9.8);
        //    Contents.DrawText(ArialNormal, 10, 3, 9.85, TextJustify.Left, " USLOVI ISPITIVANJA");
        //    Contents.DrawLine(3, 9.8, 3, 10.0);
        //    Contents.DrawLine(5.3, 9.8, 5.3, 10.0);


        //    Contents.DrawText(ArialNormal, 10, 3, 9.65, TextJustify.Left, " Standard : ");
        //    Contents.DrawLine(3, 9.6, 4, 9.6);
        //    Contents.DrawLine(3, 9.6, 3, 9.8);
        //    Contents.DrawLine(4, 9.6, 4, 9.8);

        //    Contents.DrawText(ArialNormal, 10, 4, 9.65, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfStandard.Text);
        //    Contents.DrawLine(4, 9.6, 5.3, 9.6);
        //    Contents.DrawLine(4, 9.6, 4, 9.8);
        //    Contents.DrawLine(5.3, 9.6, 5.3, 9.8);

        //    Contents.DrawText(ArialNormal, 10, 3, 9.45, TextJustify.Left, " Metoda : ");
        //    Contents.DrawLine(3, 9.4, 4, 9.4);
        //    Contents.DrawLine(3, 9.4, 3, 9.6);
        //    Contents.DrawLine(4, 9.4, 4, 9.6);

        //    Contents.DrawText(ArialNormal, 10, 4, 9.45, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfMetoda.Text);
        //    Contents.DrawLine(4, 9.4, 5.3, 9.4);
        //    Contents.DrawLine(4, 9.4, 4, 9.6);
        //    Contents.DrawLine(5.3, 9.4, 5.3, 9.6);


        //    Contents.DrawText(ArialNormal, 10, 3, 9.25, TextJustify.Left, " Opseg ");
        //    Contents.DrawText(ArialNormal, 10, 3, 9.05, TextJustify.Left, " temperatura : ");
        //    Contents.DrawLine(3, 9.0, 4, 9.0);
        //    Contents.DrawLine(3, 9.0, 3, 9.4);
        //    Contents.DrawLine(4, 9.0, 4, 9.4);

        //    var celzijusStepeni = " \xB0" + String.Empty + "C - ";//oznaka za celzijusov stepen // or "unit\xB2"
        //    var celzijusStepeni2 = " \xB0" + String.Empty + "C";

        //    //Contents.DrawText(ArialNormal, 10, 4, 9.15, TextJustify.Left, " " + onlineHeader.ConditionsOfTesting.tfBegOpsegTemperatura.Text + celzijusStepeni + onlineHeader.ConditionsOfTesting.tfEndOpsegTemperatura.Text + celzijusStepeni2);
        //    //Contents.DrawLine(4, 9.0, 5.3, 9.0);
        //    //Contents.DrawLine(4, 9.0, 4, 9.4);
        //    //Contents.DrawLine(5.3, 9.0, 5.3, 9.4);


        //    Contents.DrawText(ArialNormal, 9, 3, 8.85, TextJustify.Left, " Ekstenziometar : ");
        //    Contents.DrawLine(3, 8.8, 4, 8.8);
        //    Contents.DrawLine(3, 8.8, 3, 9.0);
        //    Contents.DrawLine(4, 8.8, 4, 9.0);

        //    if (onlineHeader.ConditionsOfTesting.rbtnYes.IsChecked == true)
        //    {
        //        Contents.DrawText(ArialNormal, 10, 4, 8.85, TextJustify.Left, " " + "DA");
        //        Contents.DrawLine(4, 8.8, 5.3, 8.8);
        //        Contents.DrawLine(4, 8.8, 4, 9.0);
        //        Contents.DrawLine(5.3, 8.8, 5.3, 9.0);
        //    }
        //    if (onlineHeader.ConditionsOfTesting.rbtnNo.IsChecked == true)
        //    {
        //        Contents.DrawText(ArialNormal, 10, 4, 8.85, TextJustify.Left, " " + "NE");
        //        Contents.DrawLine(4, 8.8, 5.3, 8.8);
        //        Contents.DrawLine(4, 8.8, 4, 9.0);
        //        Contents.DrawLine(5.3, 8.8, 5.3, 9.0);
        //    }


        //}

        /// <summary>
        /// sirina ide od 5.5 do 8 (razmak je sirine 0.2) [ukupno 2.5]
        /// fikisni deo sitina 1, nefiksni 1.3 
        /// </summary>
        //private void setMaterialForTesting() 
        //{
        //    Contents.DrawLine(5.5, 10.0, 7.8, 10.0);
        //    Contents.DrawLine(5.5, 9.8, 7.8, 9.8);
        //    Contents.DrawText(ArialNormal, 10, 5.5, 9.85, TextJustify.Left, " MATERIJAL ISPITIVANJA");
        //    Contents.DrawLine(5.5, 9.8, 5.5, 10.0);
        //    Contents.DrawLine(7.8, 9.8, 7.8, 10.0);


        //    Contents.DrawText(ArialNormal, 9.5, 5.5, 9.65, TextJustify.Left, " Polazni kvalitet : ");
        //    Contents.DrawLine(5.5, 9.6, 6.5, 9.6);
        //    Contents.DrawLine(5.5, 9.6, 5.5, 9.8);
        //    Contents.DrawLine(6.5, 9.6, 6.5, 9.8);

        //    Contents.DrawText(ArialNormal, 10, 6.5, 9.65, TextJustify.Left, " " + onlineHeader.MaterialForTesting.tfPolazniKvalitet.Text);
        //    Contents.DrawLine(6.5, 9.6, 7.8, 9.6);
        //    Contents.DrawLine(6.5, 9.6, 6.5, 9.8);
        //    Contents.DrawLine(7.8, 9.6, 7.8, 9.8);


        //    Contents.DrawText(ArialNormal, 10, 5.5, 9.45, TextJustify.Left, " Nazivna ");
        //    Contents.DrawText(ArialNormal, 10, 5.5, 9.25, TextJustify.Left, " debljina : ");
        //    Contents.DrawLine(5.5, 9.2, 6.5, 9.2);
        //    Contents.DrawLine(5.5, 9.2, 5.5, 9.6);
        //    Contents.DrawLine(6.5, 9.2, 6.5, 9.6);

        //    Contents.DrawText(ArialNormal, 10, 6.5, 9.35, TextJustify.Left, " " + onlineHeader.MaterialForTesting.tfNazivnaDebljina.Text);
        //    Contents.DrawLine(6.5, 9.2, 7.8, 9.2);
        //    Contents.DrawLine(6.5, 9.2, 6.5, 9.6);
        //    Contents.DrawLine(7.8, 9.2, 7.8, 9.6);

        //    Contents.DrawText(ArialNormal, 10, 5.5, 9.05, TextJustify.Left, " Način ");
        //    Contents.DrawText(ArialNormal, 10, 5.5, 8.85, TextJustify.Left, " prerade : ");
        //    Contents.DrawLine(5.5, 8.8, 6.5, 8.8);
        //    Contents.DrawLine(5.5, 8.8, 5.5, 9.2);
        //    Contents.DrawLine(6.5, 8.8, 6.5, 9.2);

        //    if (onlineHeader.MaterialForTesting.rbtnValjani.IsChecked == true)
        //    {
        //        Contents.DrawText(ArialNormal, 10, 6.5, 8.95, TextJustify.Left, " " + "VALJANI");
        //        Contents.DrawLine(6.5, 8.8, 7.8, 8.8);
        //        Contents.DrawLine(6.5, 8.8, 6.5, 9.2);
        //        Contents.DrawLine(7.8, 8.8, 7.8, 9.2);
        //    }
        //    if (onlineHeader.MaterialForTesting.rbtnVučeni.IsChecked == true)
        //    {
        //        Contents.DrawText(ArialNormal, 10, 6.5, 8.95, TextJustify.Left, " " + "VUČENI");
        //        Contents.DrawLine(6.5, 8.8, 7.8, 8.8);
        //        Contents.DrawLine(6.5, 8.8, 6.5, 9.2);
        //        Contents.DrawLine(7.8, 8.8, 7.8, 9.2);
        //    }
        //    if (onlineHeader.MaterialForTesting.rbtnKovani.IsChecked == true)
        //    {
        //        Contents.DrawText(ArialNormal, 10, 6.5, 8.95, TextJustify.Left, " " + "KOVANI");
        //        Contents.DrawLine(6.5, 8.8, 7.8, 8.8);
        //        Contents.DrawLine(6.5, 8.8, 6.5, 9.2);
        //        Contents.DrawLine(7.8, 8.8, 7.8, 9.2);
        //    }
        //    if (onlineHeader.MaterialForTesting.rbtnLiveni.IsChecked == true)
        //    {
        //        Contents.DrawText(ArialNormal, 10, 6.5, 8.95, TextJustify.Left, " " + "LIVENI");
        //        Contents.DrawLine(6.5, 8.8, 7.8, 8.8);
        //        Contents.DrawLine(6.5, 8.8, 6.5, 9.2);
        //        Contents.DrawLine(7.8, 8.8, 7.8, 9.2);
        //    }
        //}


        #endregion



        private void setFootnote() 
        {
            Contents.DrawText(ArialNormal, 5.5, 1.4, 4.15 + 0.3, TextJustify.Left, "*Rezultati se odnose na ispitani uzorak");
        }


        /// <summary>
        /// sirina ide od 0.5 do 2.375 (razmak je sirine 0.075) [ukupno 1.875]
        /// fikisni deo sitina 0.5, nefiksni 1.3 
        /// </summary>
        private void setKindOfTube() 
        {
            //Contents.DrawLine(0.5, 3.8, 1, 3.8);
            //Contents.DrawLine(0.5, 3.4, 1, 3.4);
            Contents.DrawText(ArialNormal, 8, 0.5, 3.75 + 0.3, TextJustify.Left, "TIP EPR.");
            //Contents.DrawText(ArialNormal, 8, 0.5, 3.65, TextJustify.Left, "EPRUVETE");
            //Contents.DrawLine(0.5, 3.4, 0.5, 3.8);
            //Contents.DrawLine(1, 3.4, 1, 3.8);

            //if (onlineHeader.rbtnEpvTipProporcionalna.IsChecked == true && onlineHeader.rbtnEpvK1.IsChecked == true)
            //{
            Contents.DrawLine(1.3, 3.9 + 0.3, 2.8, 3.9 + 0.3);
            Contents.DrawLine(1.3, 3.7 + 0.3, 2.8, 3.7 + 0.3);
            Contents.DrawLine(1.3, 3.7 + 0.3, 1.3, 3.9 + 0.3);
            Contents.DrawLine(2.8, 3.7 + 0.3, 2.8, 3.9 + 0.3);
            if (LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True") && LastInputOutputSavedData.rbtnEpvK1.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 1.3, 3.75 + 0.3, TextJustify.Left, " PROPORCIONALNA (5.65)"); 
            }
            //}
            //if (onlineHeader.rbtnEpvTipProporcionalna.IsChecked == true && onlineHeader.rbtnEpvK2.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvTipProporcionalna.Equals("True") && LastInputOutputSavedData.rbtnEpvK2.Equals("True"))
            {
                //Contents.DrawLine(1, 3.7, 2.3, 3.7);
                //Contents.DrawLine(1, 3.5, 2.3, 3.5);
                Contents.DrawText(ArialNormal, 8, 1.3, 3.75 + 0.3, TextJustify.Left, " PROPORCIONALNA (11.3)");
                //Contents.DrawLine(1, 3.5, 1, 3.7);
                //Contents.DrawLine(2.3, 3.5, 2.3, 3.7);
            }
            //}
            //if (onlineHeader.rbtnEpvTipNeproporcionalna.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvTipNeproporcionalna.Equals("True"))
            {
                //Contents.DrawLine(1, 3.7, 2.3, 3.7);
                //Contents.DrawLine(1, 3.5, 2.3, 3.5);
                Contents.DrawText(ArialNormal, 8, 1.3, 3.75 + 0.3, TextJustify.Left, " NEPROPORCIONALNA");
                //Contents.DrawLine(1, 3.5, 1, 3.7);
                //Contents.DrawLine(2.3, 3.5, 2.3, 3.7);
            }
            //}



            //Contents.DrawLine(0.5, 3.4, 1, 3.4);
            //Contents.DrawLine(0.5, 3.2, 1, 3.2);
            Contents.DrawText(ArialNormal, 8, 0.5, 3.5 + 0.3, TextJustify.Left, "OBLIK EPR.");
            //Contents.DrawLine(0.5, 3.2, 0.5, 3.4);
            //Contents.DrawLine(1, 3.2, 1, 3.4);

            //if (onlineHeader.rbtnEpvOblikObradjena.IsChecked == true)
            //{
            Contents.DrawLine(1.3, 3.65 + 0.3, 2.8, 3.65 + 0.3);
            Contents.DrawLine(1.3, 3.45 + 0.3, 2.8, 3.45 + 0.3);
            Contents.DrawLine(1.3, 3.45 + 0.3, 1.3, 3.65 + 0.3);
            Contents.DrawLine(2.8, 3.45 + 0.3, 2.8, 3.65 + 0.3);
            if (LastInputOutputSavedData.rbtnEpvOblikObradjena.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 8, 1.3, 3.5 + 0.3, TextJustify.Left, " OBRADJEN");
            }
            //}
            //if (onlineHeader.rbtnEpvOblikNeobradjena.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvOblikNeobradjena.Equals("True"))
            {
                //Contents.DrawLine(1, 3.4, 2.3, 3.4);
                //Contents.DrawLine(1, 3.2, 2.3, 3.2);
                Contents.DrawText(ArialNormal, 8, 1.3, 3.5 + 0.3, TextJustify.Left, " NEOBRADJEN");
                //Contents.DrawLine(1, 3.2, 1, 3.4);
                //Contents.DrawLine(2.3, 3.2, 2.3, 3.4);
            }
            //}



            Contents.DrawText(ArialNormal, 8, 0.5, 3.25 + 0.3, TextJustify.Left, "VRSTA EPR.");


            Contents.DrawLine(1.3, 3.4 + 0.3, 2.8, 3.4 + 0.3); //1,3
            Contents.DrawLine(1.3, 3.2 + 0.3, 2.8, 3.2 + 0.3); //1,3
            //Contents.DrawText(ArialNormal, 5.5, 4.75, 3.35, TextJustify.Left, " PRAVOUGAONA");
            Contents.DrawLine(1.3, 3.2 + 0.3, 1.3, 3.4 + 0.3);
            Contents.DrawLine(2.8, 3.2 + 0.3, 2.8, 3.4 + 0.3);
            //if (onlineHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
            {
                //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
                //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
                Contents.DrawText(ArialNormal, 8, 1.3, 3.25 + 0.3, TextJustify.Left, " PRAVOUGAONA");
                //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
                //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            }
            //}
            //if (onlineHeader.rbtnEpvVrstaKruzni.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
            {
                //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
                //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
                Contents.DrawText(ArialNormal, 8, 1.3, 3.25 + 0.3, TextJustify.Left, " KRUŽNA");
                //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
                //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            }
            //}
            //if (onlineHeader.rbtnEpvVrstaCevasti.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
            {
                //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
                //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
                Contents.DrawText(ArialNormal, 8, 1.3, 3.25 + 0.3, TextJustify.Left, " CEVASTA");
                //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
                //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            }
            //}
            //if (onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
            {
                //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
                //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
                Contents.DrawText(ArialNormal, 8, 1.3, 3.25 + 0.3, TextJustify.Left, " DELOM CEVASTA");
                //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
                //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            }
            //}
            //if (onlineHeader.rbtnEpvVrstaSestaugaona.IsChecked == true)
            //{
            if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
            {
                //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
                //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
                Contents.DrawText(ArialNormal, 8, 1.3, 3.25 + 0.3, TextJustify.Left, " ŠESTAUGAONA");
                //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
                //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            }
            //}
              
         
            
        }


        /// <summary>
        /// sirina ide od 2.375 do 4.250 (razmak je sirine 0.075) [ukupno 1.875]
        /// fikisni deo sitina 0.5, nefiksni 1.3 
        /// </summary>
        private void setPositionOfTube() 
        {
            //Contents.DrawLine(2.375, 3.8, 4.175, 3.8); //0,5 + 1,3 = 1.8
            //Contents.DrawLine(2.375, 3.6, 4.175, 3.6); //0,5 + 1,3 = 1.8
            Contents.DrawText(ArialBold, 8, 0.5, 2.65 + 0.3, TextJustify.Left, "POLOŽAJ EPRUVETE U ODNOSU NA");
            //Contents.DrawLine(2.375, 3.6, 2.375, 3.8);
            //Contents.DrawLine(4.175, 3.6, 4.175, 3.8);


            //Contents.DrawLine(0.5, 3.2, 1, 3.2);
            //Contents.DrawLine(0.5, 2.8, 1, 2.8);
            //Contents.DrawText(ArialNormal, 8, 0.5, 3.05, TextJustify.Left, "PRAVAC");
            //Contents.DrawText(ArialNormal, 8, 0.5, 2.85, TextJustify.Left, "VALJANJA");
            //Contents.DrawText(ArialNormal, 8, 0.5, 2.65, TextJustify.Left, "VUČENJA");
            Contents.DrawText(ArialNormal, 8, 0.5, 2.40 + 0.3, TextJustify.Left, "PR. VALJ/VUČ.");
            //Contents.DrawLine(0.5, 2.8, 0.5, 3.2);
            //Contents.DrawLine(1, 2.8, 1, 3.2);

            //if (onlineHeader.rbtnEpvUzduzni.IsChecked == true)
            //{
            //    Contents.DrawLine(1, 3.2, 2.3, 3.2);
            //    Contents.DrawLine(1, 2.8, 2.3, 2.8);
            //    Contents.DrawText(ArialNormal, 8, 1, 2.95, TextJustify.Left, " UZDUŽNI");
            //    Contents.DrawLine(1, 2.8, 1, 3.2);
            //    Contents.DrawLine(2.3, 2.8, 2.3, 3.2);
            //}
            //if (onlineHeader.rbtnEpvPoprecni.IsChecked == true)
            //{
            //    Contents.DrawLine(1, 3.2, 2.3, 3.2);
            //    Contents.DrawLine(1, 2.8, 2.3, 2.8);
            //    Contents.DrawText(ArialNormal, 8, 1, 2.95, TextJustify.Left, " POPREČNI");
            //    Contents.DrawLine(1, 2.8, 1, 3.2);
            //    Contents.DrawLine(2.3, 2.8, 2.3, 3.2);
            //}


            Contents.DrawLine(1.3, 2.55 + 0.3, 2.8, 2.55 + 0.3);
            Contents.DrawLine(1.3, 2.35 + 0.3, 2.8, 2.35 + 0.3);
            //    Contents.DrawText(ArialNormal, 8, 1, 2.95, TextJustify.Left, " UZDUŽNI");
            Contents.DrawLine(1.3, 2.35 + 0.3, 1.3, 2.55 + 0.3);
            Contents.DrawLine(2.8, 2.35 + 0.3, 2.8, 2.55 + 0.3);

            string customDegree = "\xB0";

            //Contents.DrawText(ArialNormal, 8, 1, 2.75, TextJustify.Left, " " + onlineHeader.PositionOfTube.tfCustomPravacValjanja.Text + customDegree);
            Contents.DrawText(ArialNormal, 8, 1.3, 2.40 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfCustomPravacValjanja_PositionOfTube + customDegree);


            //Contents.DrawLine(2.375, 3.2, 2.875, 3.2); //0,5
            //Contents.DrawLine(2.375, 2.8, 2.875, 2.8); //0,5
            //Contents.DrawText(ArialNormal, 8, 0.5, 2.45, TextJustify.Left, "ŠIRINU");
            //Contents.DrawText(ArialNormal, 8, 0.5, 2.25, TextJustify.Left, "PROIZV.");
            Contents.DrawText(ArialNormal, 8, 0.5, 2.15 + 0.3, TextJustify.Left, "ŠIRINA PRO.");
            //Contents.DrawLine(2.375, 2.8, 2.375, 3.2);
            //Contents.DrawLine(2.875, 2.8, 2.875, 3.2);


            Contents.DrawLine(1.3, 2.3 + 0.3, 2.8, 2.3 + 0.3); //1,3
            Contents.DrawLine(1.3, 2.1 + 0.3, 2.8, 2.1 + 0.3); //1,3
            //Contents.DrawText(ArialNormal, 5.5, 3.275, 2.95, TextJustify.Left, " IVICE");//2.95 + 0.4 = 3.35
            Contents.DrawLine(1.3, 2.1 + 0.3, 1.3, 2.3 + 0.3);
            Contents.DrawLine(2.8, 2.1 + 0.3, 2.8, 2.3 + 0.3);
           
           
            //Contents.DrawText(ArialNormal, 6, 2.875, 3.35, TextJustify.Left, " " + onlineHeader.PositionOfTube.tfCustomSirinaTrake.Text.ToUpper());
            Contents.DrawText(ArialNormal, 8, 1.3, 2.15 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfCustomSirinaTrake_PositionOfTube.ToUpper());
            
        


            //Contents.DrawLine(2.375, 2.8, 2.875, 2.8); //0,5
            //Contents.DrawLine(2.375, 2.4, 2.875, 2.4); //0,5
            //Contents.DrawText(ArialNormal, 8, 0.5, 2.05, TextJustify.Left, "DUŽINU");
            //Contents.DrawText(ArialNormal, 8, 0.5, 1.85, TextJustify.Left, "PROIZV.");
            Contents.DrawText(ArialNormal, 8, 0.5, 1.90 + 0.3, TextJustify.Left, "DUŽINA PRO.");
            //Contents.DrawLine(2.375, 2.4, 2.375, 2.8);
            //Contents.DrawLine(2.875, 2.4, 2.875, 2.8);


            Contents.DrawLine(1.3, 2.05 + 0.3, 2.8, 2.05 + 0.3); //1,3
            Contents.DrawLine(1.3, 1.85 + 0.3, 2.8, 1.85 + 0.3); //1,3
            //Contents.DrawText(ArialNormal, 5.5, 2.875, 2.55, TextJustify.Left, " POČETAK");
            Contents.DrawLine(1.3, 1.85 + 0.3, 1.3, 2.05 + 0.3);
            Contents.DrawLine(2.8, 1.85 + 0.3, 2.8, 2.05 + 0.3);
            
            //Contents.DrawText(ArialNormal, 5.5, 2.875, 2.95, TextJustify.Left, " " + onlineHeader.PositionOfTube.tfCustomDuzinaTrake.Text.ToUpper());
            Contents.DrawText(ArialNormal, 8, 1.3, 1.90 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfCustomDuzinaTrake_PositionOfTube.ToUpper());
      
         

        }

        /// <summary>
        /// sirina ide od 4.250 do 6.425 (razmak je sirine 0.075) [ukupno 2.175] {6.425 - 0.075 = 6.35}
        /// fikisni deo sitina 0.5, nefiksni 1.3 
        /// </summary>
        private void setTubeDimension(bool makeFromLastSample = true, string xmlName = "") 
        {
            //Contents.DrawLine(4.25, 3.8, 6.35, 3.8); //0,5 + 1,3 = 1.8
            //Contents.DrawLine(4.25, 3.6, 6.35, 3.6); //0,5 + 1,3 = 1.8
            Contents.DrawText(ArialBold, 8, 3.1, 3.85 + 0.3, TextJustify.Left, "DIMENZIJE EPRUVETE");
            //Contents.DrawLine(4.25, 3.6, 4.25, 3.8);
            //Contents.DrawLine(6.35, 3.6, 6.35, 3.8);

            ////Contents.DrawLine(4.25, 3.6, 4.75, 3.6); //0,5
            ////Contents.DrawLine(4.25, 3.2, 4.75, 3.2); //0,5
            //Contents.DrawText(ArialNormal, 8, 2.5, 3.65, TextJustify.Left, "VRSTA");
            //Contents.DrawText(ArialNormal, 8, 2.5, 3.45, TextJustify.Left, "EPRUVETE");
            ////Contents.DrawLine(4.25, 3.2, 4.25, 3.6);
            ////Contents.DrawLine(4.75, 3.2, 4.75, 3.6);


            //Contents.DrawLine(3.35, 3.7, 4.95, 3.7); //1,3
            //Contents.DrawLine(3.35, 3.5, 4.95, 3.5); //1,3
            ////Contents.DrawText(ArialNormal, 5.5, 4.75, 3.35, TextJustify.Left, " PRAVOUGAONA");
            //Contents.DrawLine(3.35, 3.5, 3.35, 3.7);
            //Contents.DrawLine(4.95, 3.5, 4.95, 3.7);
            ////if (onlineHeader.rbtnEpvVrstaPravougaona.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
            //{
            //    //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
            //    //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
            //    Contents.DrawText(ArialNormal, 8, 3.35, 3.55, TextJustify.Left, " PRAVOUGAONA");
            //    //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
            //    //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            //}
            ////}
            ////if (onlineHeader.rbtnEpvVrstaKruzni.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
            //{
            //    //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
            //    //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
            //    Contents.DrawText(ArialNormal, 8, 3.35, 3.55, TextJustify.Left, " KRUŽNA");
            //    //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
            //    //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            //}
            ////}
            ////if (onlineHeader.rbtnEpvVrstaCevasti.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
            //{
            //    //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
            //    //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
            //    Contents.DrawText(ArialNormal, 8, 3.35, 3.55, TextJustify.Left, " CEVASTA");
            //    //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
            //    //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            //}
            ////}
            ////if (onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
            //{
            //    //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
            //    //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
            //    Contents.DrawText(ArialNormal, 8, 3.35, 3.55, TextJustify.Left, " DELOM CEVASTA");
            //    //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
            //    //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            //}
            ////}
            ////if (onlineHeader.rbtnEpvVrstaSestaugaona.IsChecked == true)
            ////{
            //if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
            //{
            //    //Contents.DrawLine(4.75, 3.6, 6.35, 3.6); //1,3
            //    //Contents.DrawLine(4.75, 3.2, 6.35, 3.2); //1,3
            //    Contents.DrawText(ArialNormal, 8, 3.35, 3.55, TextJustify.Left, " ŠESTAUGAONA");
            //    //Contents.DrawLine(4.75, 3.2, 4.75, 3.6);
            //    //Contents.DrawLine(6.35, 3.2, 6.35, 3.6);
            //}
            ////}


            //a0
            //Contents.DrawLine(4.25, 3.2, 4.65, 3.2); //0,4
            //Contents.DrawLine(4.25, 3.0, 4.65, 3.0); //0,4
            Contents.DrawText(ArialNormal, 8, 3.1, 3.65 + 0.3, TextJustify.Left, " " + "a");
            Contents.DrawText(ArialNormal, 5.5, 3.2, 3.65 + 0.3, TextJustify.Left, " " + "0");//2.6 - 2.5 = 0.1
            //Contents.DrawLine(4.25, 3.0, 4.25, 3.2);
            //Contents.DrawLine(4.65, 3.0, 4.65, 3.2);


            Contents.DrawLine(3.3, 3.8 + 0.3, 4.0, 3.8 + 0.3); //1
            Contents.DrawLine(3.3, 3.6 + 0.3, 4.0, 3.6 + 0.3); //1
            //if ((onlineHeader.rbtnEpvVrstaPravougaona.IsChecked == true || onlineHeader.rbtnEpvVrstaCevasti.IsChecked == true || onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true) && onlineHeader.tfAGlobal.Text.Equals(String.Empty) == false)
            // {
            if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True") && (LastInputOutputSavedData.a0Pravougaona.Equals(String.Empty) == false || LastInputOutputSavedData.a0Cevasta.Equals(String.Empty) == false || LastInputOutputSavedData.a0Deocev.Equals(String.Empty) == false))
            {
                 //Contents.DrawText(ArialNormal, 6, 4.60, 3.1, TextJustify.Left, " " + onlineHeader.tfAGlobal.Text);//Contents.DrawLine(4.65, 3.0, 5.3, 3.0); 

                if (onlineHeader.tfAGlobal != null)
                {
                    if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                    {
                        LastInputOutputSavedData.a0Pravougaona = onlineHeader.tfAGlobal.Text;
                    }
                    if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                    {
                        LastInputOutputSavedData.a0Cevasta = onlineHeader.tfAGlobal.Text;
                    }
                    if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                    {
                        LastInputOutputSavedData.a0Deocev = onlineHeader.tfAGlobal.Text;
                    }
                }
                if (LastInputOutputSavedData.a0Pravougaona.Equals(String.Empty) == false)
                {
                    widtha0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.a0Pravougaona, false);
                    Contents.DrawText(ArialNormal, 8, 4.0 - widtha0 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.a0Pravougaona);//Contents.DrawLine(4.65, 3.0, 5.3, 3.0); 
                }
                if (LastInputOutputSavedData.a0Cevasta.Equals(String.Empty) == false)
                {
                    widtha0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.a0Cevasta, false);
                    Contents.DrawText(ArialNormal, 8, 4.0 - widtha0 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.a0Cevasta);//Contents.DrawLine(4.65, 3.0, 5.3, 3.0); 
                }
                if (LastInputOutputSavedData.a0Deocev.Equals(String.Empty) == false)
                {
                    widtha0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.a0Deocev, false);
                    Contents.DrawText(ArialNormal, 8, 4.0 - widtha0 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.a0Deocev);//Contents.DrawLine(4.65, 3.0, 5.3, 3.0); 
                }
            }
            // }
            Contents.DrawText(ArialNormal, 8, 4.0, 3.65 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(3.3, 3.6 + 0.3, 3.3, 3.8 + 0.3);
            Contents.DrawLine(4.0, 3.6 + 0.3, 4.0, 3.8 + 0.3);
    

            //au
            //Contents.DrawLine(4.25, 3.0, 4.65, 3.0); //0,4
            //Contents.DrawLine(4.25, 2.8, 4.65, 2.8); //0,4
            Contents.DrawText(ArialNormal, 8, 3.1, 3.40 + 0.3, TextJustify.Left, " " + "a");//4.25 + 0.165 = 4.415 - 0.05
            Contents.DrawText(ArialNormal, 5.5, 3.2, 3.40 + 0.3, TextJustify.Left, " " + "u");//4.415 + 0.115 = 4.53 - 0.05
            //Contents.DrawLine(4.25, 2.8, 4.25, 3.0);
            //Contents.DrawLine(4.65, 2.8, 4.65, 3.0);

            //resultsInterface.Au nije jos implementiran
            Contents.DrawLine(3.3, 3.55 + 0.3, 4.0, 3.55 + 0.3); //1
            Contents.DrawLine(3.3, 3.35 + 0.3, 4.0, 3.35 + 0.3); //1
            //if ((onlineHeader.rbtnEpvVrstaPravougaona.IsChecked == true || onlineHeader.rbtnEpvVrstaCevasti.IsChecked == true || onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true) && resultsInterface.Au.Text.Equals(String.Empty) == false)
            //{
            //LastInputOutputSavedData.Find_au();
            if (resultsInterface != null)
            {
                if (resultsInterface.tfAGlobal != null)
                {
                    LastInputOutputSavedData.au = resultsInterface.tfAGlobal.Text;
                }
            }
            else
            {
                LastInputOutputSavedData.au = string.Empty;
            }
            if ((LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True")) && LastInputOutputSavedData.au.Equals(String.Empty) == false)
            {
                //Contents.DrawText(ArialNormal, 8, 4.60, 2.90, TextJustify.Left, " " + resultsInterface.Au.Text);//Contents.DrawLine(4.65, 2.8, 5.3, 2.8);
                widthau = Contents.DrawText(ArialNormal, 8, 4.0 - LastInputOutputSavedData.au.Count() * 0.1 - 0.05, 3.00 + 0.3, TextJustify.Left, LastInputOutputSavedData.au, false);
                Contents.DrawText(ArialNormal, 8, 4.0 - widthau - 0.05, 3.40 + 0.3, TextJustify.Left, LastInputOutputSavedData.au);//Contents.DrawLine(4.65, 2.8, 5.3, 2.8);
            }
            //}
            Contents.DrawText(ArialNormal, 8, 4.0, 3.40 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(3.3, 3.35 + 0.3, 3.3, 3.55 + 0.3);
            Contents.DrawLine(4.0, 3.35 + 0.3, 4.0, 3.55 + 0.3);
      

            //b0
            //Contents.DrawLine(4.25, 2.8, 4.65, 2.8); //0,4
            //Contents.DrawLine(4.25, 2.6, 4.65, 2.6); //0,4
            Contents.DrawText(ArialNormal, 8, 3.1, 3.15 + 0.3, TextJustify.Left, " " + "b");
            Contents.DrawText(ArialNormal, 5.5, 3.2, 3.15 + 0.3, TextJustify.Left, " " + "0");
            //Contents.DrawLine(4.25, 2.6, 4.25, 2.8);
            //Contents.DrawLine(4.65, 2.6, 4.65, 2.8);


            Contents.DrawLine(3.3, 3.3 + 0.3, 4.0, 3.3 + 0.3); //1
            Contents.DrawLine(3.3, 3.1 + 0.3, 4.0, 3.1 + 0.3); //1
            //if ((onlineHeader.rbtnEpvVrstaPravougaona.IsChecked == true || onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true) && onlineHeader.tfBGlobal.Text.Equals(String.Empty) == false)
            //{


            if (onlineHeader.tfBGlobal != null)
            {
                if (LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True"))
                {
                    LastInputOutputSavedData.b0Pravougaona = onlineHeader.tfBGlobal.Text;
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                {
                    LastInputOutputSavedData.b0Deocev = onlineHeader.tfBGlobal.Text;
                }
            }


            if ((LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True")) && (LastInputOutputSavedData.b0Pravougaona.Equals(String.Empty) == false || LastInputOutputSavedData.b0Deocev.Equals(String.Empty) == false))
            {
                //Contents.DrawText(ArialNormal, 6, 4.60, 2.70, TextJustify.Left, " " + onlineHeader.tfBGlobal.Text);//Contents.DrawLine(4.65, 2.6, 5.3, 2.6);
                if (LastInputOutputSavedData.b0Pravougaona.Equals(String.Empty) == false)
                {
                    widthb0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.b0Pravougaona, false);
                    Contents.DrawText(ArialNormal, 8, 4.0 - widthb0 - 0.05, 3.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.b0Pravougaona);//Contents.DrawLine(4.65, 2.6, 5.3, 2.6);
                }
                if (LastInputOutputSavedData.b0Deocev.Equals(String.Empty) == false)
                {
                    widthb0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.b0Deocev, false);
                    Contents.DrawText(ArialNormal, 8, 4.0 - widthb0 - 0.05, 3.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.b0Deocev);//Contents.DrawLine(4.65, 2.6, 5.3, 2.6);
                }
            }
            //}
            Contents.DrawText(ArialNormal, 8, 4.0, 3.15 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(3.3, 3.1 + 0.3, 3.3, 3.3 + 0.3);
            Contents.DrawLine(4.0, 3.1 + 0.3, 4.0, 3.3 + 0.3);
            


            //bu
            //Contents.DrawLine(4.25, 2.6, 4.65, 2.6); //0,4
            //Contents.DrawLine(4.25, 2.4, 4.65, 2.4); //0,4
            Contents.DrawText(ArialNormal, 8, 3.1, 2.90 + 0.3, TextJustify.Left, " " + "b");
            Contents.DrawText(ArialNormal, 5.5, 3.2, 2.90 + 0.3, TextJustify.Left, " " + "u");
            //Contents.DrawLine(4.25, 2.4, 4.25, 2.6);
            //Contents.DrawLine(4.65, 2.4, 4.65, 2.6);

            //resultsInterface.Bu nije jos implementiran
            Contents.DrawLine(3.3, 3.05 + 0.3, 4.0, 3.05 + 0.3); //1
            Contents.DrawLine(3.3, 2.85 + 0.3, 4.0, 2.85 + 0.3); //1
            //if((onlineHeader.rbtnEpvVrstaPravougaona.IsChecked == true || onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true) && resultsInterface.Bu.Text.Equals(String.Empty) == false)
            //{
            //LastInputOutputSavedData.Find_bu();
            if (resultsInterface != null)
            {
                if (resultsInterface.tfBGlobal != null)
                {
                    LastInputOutputSavedData.bu = resultsInterface.tfBGlobal.Text;
                }
            }
            else
            {
                LastInputOutputSavedData.bu = String.Empty;
            }
            if ((LastInputOutputSavedData.rbtnEpvVrstaPravougaona.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True")) && LastInputOutputSavedData.bu.Equals(String.Empty) == false)
            {
                //Contents.DrawText(ArialNormal, 6, 4.60, 2.50, TextJustify.Left, " " + resultsInterface.Bu.Text + " mm");//Contents.DrawLine(4.65, 2.4, 5.3, 2.4);
                widthbu = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.bu, false);
                Contents.DrawText(ArialNormal, 8, 4.0 - widthbu - 0.05, 2.90 + 0.3, TextJustify.Left, LastInputOutputSavedData.bu);//Contents.DrawLine(4.65, 2.4, 5.3, 2.4);
            }
            //}
            Contents.DrawText(ArialNormal, 8, 4.0, 2.90 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(3.3, 2.85 + 0.3, 3.3, 3.05 + 0.3);
            Contents.DrawLine(4.0, 2.85 + 0.3, 4.0, 3.05 + 0.3);


            //d0
            //Contents.DrawLine(4.25, 2.4, 4.65, 2.4); //0,4
            //Contents.DrawLine(4.25, 2.2, 4.65, 2.2); //0,4
            Contents.DrawText(ArialNormal, 8, 3.1, 2.65 + 0.3, TextJustify.Left, " " + "d");
            Contents.DrawText(ArialNormal, 5.5, 3.2, 2.65 + 0.3, TextJustify.Left, " " + "0");
            //Contents.DrawLine(4.25, 2.2, 4.25, 2.4);
            //Contents.DrawLine(4.65, 2.2, 4.65, 2.4);

            Contents.DrawLine(3.3, 2.8 + 0.3, 4.0, 2.8 + 0.3); //1
            Contents.DrawLine(3.3, 2.6 + 0.3, 4.0, 2.6 + 0.3); //1
            //if ((onlineHeader.rbtnEpvVrstaSestaugaona.IsChecked == true) && onlineHeader.tfDGlobal.Text.Equals(String.Empty) == false)
            // {
            if (onlineHeader.tfDGlobal != null)
            {
                if (LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True"))
                {
                    LastInputOutputSavedData.d0Sestaugaona = onlineHeader.tfDGlobal.Text;
                }
            }
            if ((LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True")) && LastInputOutputSavedData.d0Sestaugaona.Equals(String.Empty) == false)
            {
                //Contents.DrawText(ArialNormal, 6, 4.60, 2.30, TextJustify.Left, " " + onlineHeader.tfDGlobal.Text);//Contents.DrawLine(4.65, 2.2, 5.3, 2.2);
                widthd0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.d0Sestaugaona, false);
                Contents.DrawText(ArialNormal, 8, 4.0 - widthd0 - 0.05, 2.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.d0Sestaugaona);//Contents.DrawLine(4.65, 2.2, 5.3, 2.2);
            }
            // }
            Contents.DrawText(ArialNormal, 8, 4.0, 2.65 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(3.3, 2.6 + 0.3, 3.3, 2.8 + 0.3);
            Contents.DrawLine(4.0, 2.6 + 0.3, 4.0, 2.8 + 0.3);


            //du
            //Contents.DrawLine(4.25, 2.2, 4.65, 2.2); //0,4
            //Contents.DrawLine(4.25, 2.0, 4.65, 2.0); //0,4
            Contents.DrawText(ArialNormal, 8, 3.1, 2.4 + 0.3, TextJustify.Left, " " + "d");
            Contents.DrawText(ArialNormal, 5.5, 3.2, 2.4 + 0.3, TextJustify.Left, " " + "u");
            //Contents.DrawLine(4.25, 2.0, 4.25, 2.2);
            //Contents.DrawLine(4.65, 2.0, 4.65, 2.2);

            //resultsInterface.du nije jos implementiran
            Contents.DrawLine(3.3, 2.55 + 0.3, 4.0, 2.55 + 0.3); //1
            Contents.DrawLine(3.3, 2.35 + 0.3, 4.0, 2.35 + 0.3); //1
            //if ((onlineHeader.rbtnEpvVrstaSestaugaona.IsChecked == true) && resultsInterface.du.Text.Equals(String.Empty) == false)
            //{
            //LastInputOutputSavedData.Find_du();
            LastInputOutputSavedData.du = string.Empty;
            if ((LastInputOutputSavedData.rbtnEpvVrstaSestaugaona.Equals("True")) && LastInputOutputSavedData.du.Equals(String.Empty) == false)
            {
                //Contents.DrawText(ArialNormal, 6, 4.60, 2.10, TextJustify.Left, " " + resultsInterface.Bu.Text + " mm");//Contents.DrawLine(4.65, 2.0, 5.3, 2.0);
                widthdu = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.du, false);
                Contents.DrawText(ArialNormal, 8, 4.0 - widthdu - 0.05, 2.4 + 0.3, TextJustify.Left, LastInputOutputSavedData.du);//Contents.DrawLine(4.65, 2.0, 5.3, 2.0);
            }
            //}
            Contents.DrawText(ArialNormal, 8, 4.0, 2.4 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(3.3, 2.35 + 0.3, 3.3, 2.55 + 0.3);
            Contents.DrawLine(4.0, 2.35 + 0.3, 4.0, 2.55 + 0.3);


            //D0
            //Contents.DrawLine(5.3, 3.2, 5.7, 3.2); //0,4
            //Contents.DrawLine(5.3, 3.0, 5.7, 3.0); //0,4
            Contents.DrawText(ArialNormal, 8, 4.3, 3.65 + 0.3, TextJustify.Left, " " + "D");
            Contents.DrawText(ArialNormal, 5.5, 4.42, 3.65 + 0.3, TextJustify.Left, " " + "0");//4.12 - 4 = 0.12
            //Contents.DrawLine(5.3, 3.0, 5.3, 3.2);
            //Contents.DrawLine(5.7, 3.0, 5.7, 3.2);


            Contents.DrawLine(4.52, 3.8 + 0.3, 5.22, 3.8 + 0.3); //1
            Contents.DrawLine(4.52, 3.6 + 0.3, 5.22, 3.6 + 0.3); //1
            //if ((onlineHeader.rbtnEpvVrstaKruzni.IsChecked == true || onlineHeader.rbtnEpvVrstaCevasti.IsChecked == true || onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true) && onlineHeader.tfDGlobal.Text.Equals(String.Empty) == false)
            //{
            if (onlineHeader.tfDGlobal != null)
            {
                if (LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True"))
                {
                    LastInputOutputSavedData.D0Kruzna = onlineHeader.tfDGlobal.Text;
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True"))
                {
                    LastInputOutputSavedData.D0Cevasta = onlineHeader.tfDGlobal.Text;
                }
                if (LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True"))
                {
                    LastInputOutputSavedData.D0Deocev = onlineHeader.tfDGlobal.Text;
                }
            }
            if ((LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True")) && (LastInputOutputSavedData.D0Kruzna.Equals(String.Empty) == false || LastInputOutputSavedData.D0Cevasta.Equals(String.Empty) == false || LastInputOutputSavedData.D0Deocev.Equals(String.Empty) == false))
            {
                //Contents.DrawText(ArialNormal, 6, 5.65, 3.1, TextJustify.Left, " " + onlineHeader.tfDGlobal.Text);//Contents.DrawLine(5.7, 3.0, 6.35, 3.0);
                if (LastInputOutputSavedData.D0Kruzna.Equals(String.Empty) == false)
                {
                    widthD0 = Contents.DrawText(ArialNormal, 8, 5.22 - LastInputOutputSavedData.D0Kruzna.Count() * 0.1 - 0.05, 3.25 + 0.3, TextJustify.Left, LastInputOutputSavedData.D0Kruzna, false);
                    Contents.DrawText(ArialNormal, 8, 5.22 - widthD0 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.D0Kruzna);//Contents.DrawLine(5.7, 3.0, 6.35, 3.0);
                }
                if (LastInputOutputSavedData.D0Cevasta.Equals(String.Empty) == false)
                {
                    widthD0 = Contents.DrawText(ArialNormal, 8, 5.22 - LastInputOutputSavedData.D0Cevasta.Count() * 0.1 - 0.05, 3.25 + 0.3, TextJustify.Left, LastInputOutputSavedData.D0Cevasta, false);
                    Contents.DrawText(ArialNormal, 8, 5.22 - widthD0 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.D0Cevasta);//Contents.DrawLine(5.7, 3.0, 6.35, 3.0);
                }
                if (LastInputOutputSavedData.D0Deocev.Equals(String.Empty) == false)
                {
                    widthD0 = Contents.DrawText(ArialNormal, 8, 5.22 - LastInputOutputSavedData.D0Deocev.Count() * 0.1 - 0.05, 3.25 + 0.3, TextJustify.Left, LastInputOutputSavedData.D0Deocev, false);
                    Contents.DrawText(ArialNormal, 8, 5.22 - widthD0 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.D0Deocev);//Contents.DrawLine(5.7, 3.0, 6.35, 3.0);
                }
            }
            //}
            Contents.DrawText(ArialNormal, 8, 5.22, 3.65 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(4.52, 3.6 + 0.3, 4.52, 3.8 + 0.3);
            Contents.DrawLine(5.22, 3.6 + 0.3, 5.22, 3.8 + 0.3);

            //Du
            //Contents.DrawLine(5.3, 3.0, 5.7, 3.0); //0,4
            //Contents.DrawLine(5.3, 2.8, 5.7, 2.8); //0,4
            Contents.DrawText(ArialNormal, 8, 4.3, 3.40 + 0.3, TextJustify.Left, " " + "D");//5.3 + 0.165 = 5.465
            Contents.DrawText(ArialNormal, 5.5, 4.42, 3.40 + 0.3, TextJustify.Left, " " + "u");//5.465 + 0.115 = 5.58
            //Contents.DrawLine(5.3, 2.8, 5.3, 3.0);
            //Contents.DrawLine(5.7, 2.8, 5.7, 3.0);

            //resultsInterface.Du nije jos implementiran
            Contents.DrawLine(4.52, 3.55 + 0.3, 5.22, 3.55 + 0.3); //1
            Contents.DrawLine(4.52, 3.35 + 0.3, 5.22, 3.35 + 0.3); //1
            //if((onlineHeader.rbtnEpvVrstaKruzni.IsChecked == true || onlineHeader.rbtnEpvVrstaCevasti.IsChecked == true || onlineHeader.rbtnEpvVrstaDeocev.IsChecked == true) && resultsInterface.Du.Text.Equals(String.Empty) == false)
            //{
            //LastInputOutputSavedData.Find_Du();
            if (resultsInterface != null)
            {
                if (resultsInterface.tfDGlobal != null)
                {
                    LastInputOutputSavedData.Du = resultsInterface.tfDGlobal.Text;
                    resultsInterface.tfDGlobal.IsReadOnly = false;
                }
            }
            else
            {
                LastInputOutputSavedData.Du = String.Empty;
            }
            if ((LastInputOutputSavedData.rbtnEpvVrstaKruzna.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaCevasta.Equals("True") || LastInputOutputSavedData.rbtnEpvVrstaDeocev.Equals("True")) && LastInputOutputSavedData.Du.Equals(String.Empty) == false)
            {
                //Contents.DrawText(ArialNormal, 6, 5.65, 2.90, TextJustify.Left, " " + resultsInterface.Du.Text + " mm");//Contents.DrawLine(4.65, 2.8, 5.3, 2.8);
                widthDu = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.Du, false);
                Contents.DrawText(ArialNormal, 8, 5.22 - widthDu - 0.05, 3.40 + 0.3, TextJustify.Left, LastInputOutputSavedData.Du);//Contents.DrawLine(4.65, 2.8, 5.3, 2.8);
            }
            //}
            Contents.DrawText(ArialNormal, 8, 5.22, 3.40 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(4.52, 3.35 + 0.3, 4.52, 3.55 + 0.3);
            Contents.DrawLine(5.22, 3.35 + 0.3, 5.22, 3.55 + 0.3);


            //L0
            //Contents.DrawLine(5.3, 2.8, 5.7, 2.8); //0,4
            //Contents.DrawLine(5.3, 2.6, 5.7, 2.6); //0,4
            Contents.DrawText(ArialNormal, 8, 4.31, 3.15 + 0.3, TextJustify.Left, " " + "L");
            Contents.DrawText(ArialNormal, 5.5, 4.42, 3.15 + 0.3, TextJustify.Left, " " + "0");
            //Contents.DrawLine(5.3, 2.6, 5.3, 2.8);
            //Contents.DrawLine(5.7, 2.6, 5.7, 2.8);


            Contents.DrawLine(4.52, 3.30 + 0.3, 5.22, 3.30 + 0.3); //1
            Contents.DrawLine(4.52, 3.10 + 0.3, 5.22, 3.10 + 0.3); //1
            //if (plotting.L0 > 0)
            //{

            //    Contents.DrawText(ArialNormal, 6, 5.65, 2.7, TextJustify.Left, " " + plotting.L0);//Contents.DrawLine(5.7, 2.6, 6.35, 2.6);
            //}
            
            if (LastInputOutputSavedData.tfL0.Equals(string.Empty) == false)
            {
                widthtfL0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfL0, false);
                Contents.DrawText(ArialNormal, 8, 5.22 - widthtfL0 - 0.05, 3.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfL0);//Contents.DrawLine(5.7, 2.6, 6.35, 2.6);
            }
            Contents.DrawText(ArialNormal, 8, 5.22, 3.15 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(4.52, 3.10 + 0.3, 4.52, 3.30 + 0.3);
            Contents.DrawLine(5.22, 3.10 + 0.3, 5.22, 3.30 + 0.3);

            //Lu
            //Contents.DrawLine(5.3, 2.6, 5.7, 2.6); //0,4
            //Contents.DrawLine(5.3, 2.4, 5.7, 2.4); //0,4
            Contents.DrawText(ArialNormal, 8, 4.31, 2.90 + 0.3, TextJustify.Left, " " + "L");
            Contents.DrawText(ArialNormal, 5.5, 4.42, 2.90 + 0.3, TextJustify.Left, " " + "u");
            //Contents.DrawLine(5.3, 2.4, 5.3, 2.6);
            //Contents.DrawLine(5.7, 2.4, 5.7, 2.6);


            Contents.DrawLine(4.52, 3.05 + 0.3, 5.22, 3.05 + 0.3); //1
            Contents.DrawLine(4.52, 2.85 + 0.3, 5.22, 2.85 + 0.3); //1
            //if (resultsInterface.tfLu.Text.Equals(String.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 6, 5.65, 2.50, TextJustify.Left, " " + resultsInterface.tfLu.Text);//Contents.DrawLine(5.7, 2.4, 6.35, 2.4);
            //}
            if (LastInputOutputSavedData.tfLu_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfLu = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfLu_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 5.22 - widthtfLu - 0.05, 2.90 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfLu_ResultsInterface);//Contents.DrawLine(5.7, 2.6, 6.35, 2.6);
            }
            Contents.DrawText(ArialNormal, 8, 5.22, 2.90 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(4.52, 2.85 + 0.3, 4.52, 3.05 + 0.3);
            Contents.DrawLine(5.22, 2.85 + 0.3, 5.22, 3.05 + 0.3);


            //Lc
            //Contents.DrawLine(5.3, 2.6, 5.7, 2.6); //0,4
            //Contents.DrawLine(5.3, 2.4, 5.7, 2.4); //0,4
            Contents.DrawText(ArialNormal, 8, 4.31, 2.65 + 0.3, TextJustify.Left, " " + "L");
            Contents.DrawText(ArialNormal, 5.5, 4.42, 2.65 + 0.3, TextJustify.Left, " " + "c");
            //Contents.DrawLine(5.3, 2.4, 5.3, 2.6);
            //Contents.DrawLine(5.7, 2.4, 5.7, 2.6);


            Contents.DrawLine(4.52, 2.80 + 0.3, 5.22, 2.80 + 0.3); //1
            Contents.DrawLine(4.52, 2.60 + 0.3, 5.22, 2.60 + 0.3); //1
            //if (resultsInterface.tfLu.Text.Equals(String.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 6, 5.65, 2.50, TextJustify.Left, " " + resultsInterface.tfLu.Text);//Contents.DrawLine(5.7, 2.4, 6.35, 2.4);
            //}
            if (LastInputOutputSavedData.tfLc.Equals(string.Empty) == false)
            {
                widthtfLc = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfLc, false);
                Contents.DrawText(ArialNormal, 8, 5.22 - widthtfLc - 0.05, 2.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfLc);//Contents.DrawLine(5.7, 2.6, 6.35, 2.6);
            }
            Contents.DrawText(ArialNormal, 8, 5.22, 2.65 + 0.3, TextJustify.Left, " mm");
            Contents.DrawLine(4.52, 2.60 + 0.3, 4.52, 2.80 + 0.3);
            Contents.DrawLine(5.22, 2.60 + 0.3, 5.22, 2.80 + 0.3);


            //S0
            //Contents.DrawLine(5.3, 2.4, 5.7, 2.4); //0,4
            //Contents.DrawLine(5.3, 2.2, 5.7, 2.2); //0,4
            Contents.DrawText(ArialNormal, 8, 4.31, 2.40 + 0.3, TextJustify.Left, " " + "S");
            Contents.DrawText(ArialNormal, 5.5, 4.42, 2.40 + 0.3, TextJustify.Left, " " + "0");
            //Contents.DrawLine(5.3, 2.2, 5.3, 2.4);
            //Contents.DrawLine(5.7, 2.2, 5.7, 2.4);

            Contents.DrawLine(4.52, 2.55 + 0.3, 5.22, 2.55 + 0.3); //1
            Contents.DrawLine(4.52, 2.35 + 0.3, 5.22, 2.35 + 0.3); //1
            string mm2 = "mm\xB2";
            //if (plotting.S0 > 0)
            //{
            //    Contents.DrawText(ArialNormal, 6, 5.65, 2.30, TextJustify.Left, " " + onlineHeader.tfS0.Text);//Contents.DrawLine(5.7, 2.2, 6.35, 2.2);
            //}
            if (LastInputOutputSavedData.tfS0.Equals(string.Empty) == false)
            {
                widthtfS0 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfS0, false);
                Contents.DrawText(ArialNormal, 8, 5.22 - widthtfS0 - 0.05, 2.40 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfS0);//Contents.DrawLine(5.7, 2.2, 6.35, 2.2);
            }
            Contents.DrawText(ArialNormal, 8, 5.26, 2.40 + 0.3, TextJustify.Left, mm2);
            Contents.DrawLine(4.52, 2.35 + 0.3, 4.52, 2.55 + 0.3);
            Contents.DrawLine(5.22, 2.35 + 0.3, 5.22, 2.55 + 0.3);

            //Su
            //Contents.DrawLine(5.3, 2.2, 5.7, 2.2); //0,4
            //Contents.DrawLine(5.3, 2.0, 5.7, 2.0); //0,4
            Contents.DrawText(ArialNormal, 8, 4.31, 2.15 + 0.3, TextJustify.Left, " " + "S");
            Contents.DrawText(ArialNormal, 5.5, 4.42, 2.15 + 0.3, TextJustify.Left, " " + "u");
            //Contents.DrawLine(5.3, 2.0, 5.3, 2.2);
            //Contents.DrawLine(5.7, 2.0, 5.7, 2.2);


            Contents.DrawLine(4.52, 2.30 + 0.3, 5.22, 2.30 + 0.3); //1
            Contents.DrawLine(4.52, 2.10 + 0.3, 5.22, 2.10 + 0.3); //1
            //if (resultsInterface.tfSu.Text.Equals(String.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 6, 5.65, 2.10, TextJustify.Left, " " + resultsInterface.tfSu.Text);//Contents.DrawLine(5.7, 2.0, 6.35, 2.0);
            //}
           
            if (File.Exists(xmlName) == true)
            {
                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(xmlName).ToList();
                if (myXmlStrings.Count == 0)
                {
                    return;
                }
                foreach (string s in myXmlStrings)
                {
                    myXmlString += s;
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(myXmlString);
                XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);

                foreach (XmlNode xn in xnList)
                {
                    LastInputOutputSavedData.tfSu_ResultsInterface = xn[Constants.XML_ResultsInterface_Su].InnerText;
                }
            }
            if (LastInputOutputSavedData.tfSu_ResultsInterface.Equals("0"))
            {
                LastInputOutputSavedData.tfSu_ResultsInterface = string.Empty;
            }
            if (LastInputOutputSavedData.tfSu_ResultsInterface.Equals(String.Empty) == false)
            {
                LastInputOutputSavedData.tfSu_ResultsInterface = LastInputOutputSavedData.tfSu_ResultsInterface;
                widthtfSu = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfSu_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 5.22 - widthtfSu - 0.05, 2.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfSu_ResultsInterface);//Contents.DrawLine(5.7, 2.0, 6.35, 2.0);
            }
            Contents.DrawText(ArialNormal, 8, 5.26, 2.15 + 0.3, TextJustify.Left, mm2);
            Contents.DrawLine(4.52, 2.10 + 0.3, 4.52, 2.30 + 0.3);
            Contents.DrawLine(5.22, 2.10 + 0.3, 5.22, 2.30 + 0.3);


        }

        /// <summary>
        /// sirina ide od 6.425 do 8.1 [ukupno 1.675]
        /// prvi fiksni deo 0.3375 i drugi fiksni deo 0.2375 i dva nefiksna sa 0.55
        /// </summary>
        private void setOutputProperties(bool makeFromLastSample = true, string xmlName = "")
        {
            //Contents.DrawLine(6.425, 3.8, 8.1, 3.8); // 0.3375 + 0.2375 + 2 * 0.55
            //Contents.DrawLine(6.425, 3.6, 8.1, 3.6); // 0.3375 + 0.2375 + 2 * 0.55
            Contents.DrawLine(7.65, 3.6 + 0.3, 8.0875, 3.6 + 0.3); // ovde se iscrtava gornja linija za Ag
            Contents.DrawText(ArialBold, 8, 5.5, 3.85 + 0.3, TextJustify.Left, " MEHANIČKO-TEHNIČKE OSOBINE");
            //Contents.DrawLine(6.425, 3.6, 6.425, 3.8);
            //Contents.DrawLine(8.1, 3.6, 8.1, 3.8);

            //Rp02
            //Contents.DrawLine(6.425, 3.6, 6.7625, 3.6); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 3.4, 6.7625, 3.4); // 0.3375 + 6.425 = 6.7625
            //Contents.DrawText(ArialNormal, 11, 6.445, 3.52, TextJustify.Left, ".");//po x osi 6.445 - 6.425 = 0.02 ; po y osi 3.52 - 3.45 = 0.07
            //Contents.DrawText(ArialNormal, 5.5, 6.425, 3.45, TextJustify.Left, " Rmax");
            Contents.DrawText(ArialNormal, 8, 5.5, 3.65 + 0.3, TextJustify.Left, " R");
            Contents.DrawText(ArialNormal, 6.5, 5.6, 3.65 + 0.3, TextJustify.Left, " p0.2");
            //Contents.DrawLine(6.425, 3.4, 6.425, 3.6);
            //Contents.DrawLine(6.7625, 3.4, 6.7625, 3.6);

            Contents.DrawLine(5.95, 3.8 + 0.3, 6.65, 3.8 + 0.3);
            Contents.DrawLine(5.95, 3.6 + 0.3, 6.65, 3.6 + 0.3); 
            //if (resultsInterface.tfRp02.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 3.50, TextJustify.Left, " " + resultsInterface.tfRp02.Text);
            //}
            //if (LastInputOutputSavedData.tfRp02_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True") ||*/ resultsInterface.chbRp02.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbRp02_ResultsInterface.Equals("True")))
            {
                widthtfRp02 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRp02_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthtfRp02 - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRp02_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 3.65 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 3.6 + 0.3, 5.95, 3.8 + 0.3);
            Contents.DrawLine(6.65, 3.6 + 0.3, 6.65, 3.8 + 0.3);

            //Rt05
            //Contents.DrawLine(6.425, 3.4, 6.7625, 3.4); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 3.2, 6.7625, 3.2); // 0.3375 + 6.425 = 6.7625
            Contents.DrawText(ArialNormal, 8, 5.5, 3.40 + 0.3, TextJustify.Left, " R");
            Contents.DrawText(ArialNormal, 6.5, 5.6, 3.40 + 0.3, TextJustify.Left, " t0.5");
            //Contents.DrawLine(6.425, 3.2, 6.425, 3.4);
            //Contents.DrawLine(6.7625, 3.2, 6.7625, 3.4);

            Contents.DrawLine(5.95, 3.55 + 0.3, 6.65, 3.55 + 0.3);
            Contents.DrawLine(5.95, 3.35 + 0.3, 6.65, 3.35 + 0.3); 
            //if (resultsInterface.tfRt05.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 3.30, TextJustify.Left, " " + resultsInterface.tfRt05.Text);
            //}
            //if (LastInputOutputSavedData.tfRt05_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True") ||*/ resultsInterface.chbRt05.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True")))
            {
                widthtfRt05 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRt05_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthtfRt05 - 0.05, 3.40 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRt05_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 3.40 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 3.35 + 0.3, 5.95, 3.55 + 0.3);
            Contents.DrawLine(6.65, 3.35 + 0.3, 6.65, 3.55 + 0.3);


            //ReH
            //Contents.DrawLine(6.425, 3.2, 6.7625, 3.2); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 3.0, 6.7625, 3.0); // 0.3375 + 6.425 = 6.7625
            Contents.DrawText(ArialNormal, 8, 5.5, 3.15 + 0.3, TextJustify.Left, " ReH");
            //Contents.DrawLine(6.425, 3.0, 6.425, 3.2);
            //Contents.DrawLine(6.7625, 3.0, 6.7625, 3.2);

            Contents.DrawLine(5.95, 3.3 + 0.3, 6.65, 3.3 + 0.3);
            Contents.DrawLine(5.95, 3.1 + 0.3, 6.65, 3.1 + 0.3); 
            //if (resultsInterface.tfReH.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 3.1, TextJustify.Left, " " + resultsInterface.tfReH.Text);
            //}
            //if (LastInputOutputSavedData.tfReH_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True") ||*/ resultsInterface.chbReH.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbReH_ResultsInterface.Equals("True")))
            {
                widthtfReH = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfReH_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthtfReH - 0.05, 3.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfReH_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 3.15 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 3.1 + 0.3, 5.95, 3.3 + 0.3);
            Contents.DrawLine(6.65, 3.1 + 0.3, 6.65, 3.3 + 0.3);


            //ReL
            //Contents.DrawLine(6.425, 3.0, 6.7625, 3.0); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 2.8, 6.7625, 2.8); // 0.3375 + 6.425 = 6.7625
            Contents.DrawText(ArialNormal, 8, 5.5, 2.90 + 0.3, TextJustify.Left, " ReL");
            //Contents.DrawLine(6.425, 2.8, 6.425, 3.0);
            //Contents.DrawLine(6.7625, 2.8, 6.7625, 3.0);

            Contents.DrawLine(5.95, 3.05 + 0.3, 6.65, 3.05 + 0.3);
            Contents.DrawLine(5.95, 2.85 + 0.3, 6.65, 2.85 + 0.3); 
            //if (resultsInterface.tfReL.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 2.90, TextJustify.Left, " " + resultsInterface.tfReL.Text);
            //}
            //if (LastInputOutputSavedData.tfReL_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True") ||*/ resultsInterface.chbReL.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbReL_ResultsInterface.Equals("True")))
            {
                widthtfReL = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfReL_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthtfReL - 0.05, 2.9 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfReL_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 2.9 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 2.85 + 0.3, 5.95, 3.05 + 0.3);
            Contents.DrawLine(6.65, 2.85 + 0.3, 6.65, 3.05 + 0.3);


            //Rm
            //Contents.DrawLine(6.425, 2.8, 6.7625, 2.8); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 2.6, 6.7625, 2.6); // 0.3375 + 6.425 = 6.7625
            Contents.DrawText(ArialNormal, 8, 5.5, 2.65 + 0.3, TextJustify.Left, " Rm");
            //Contents.DrawLine(6.425, 2.6, 6.425, 2.8);

            Contents.DrawLine(5.95, 2.8 + 0.3, 6.65, 2.8 + 0.3);
            Contents.DrawLine(5.95, 2.6 + 0.3, 6.65, 2.6 + 0.3); 
            //if (resultsInterface.tfRm.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 2.70, TextJustify.Left, " " + resultsInterface.tfRm.Text);
            //}
            if (LastInputOutputSavedData.tfRm_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfRm = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRm_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthtfRm - 0.05, 2.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRm_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 2.65 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 2.6 + 0.3, 5.95, 2.8 + 0.3);
            Contents.DrawLine(6.65, 2.6 + 0.3, 6.65, 2.8 + 0.3);

            //R/Rm
            //Contents.DrawLine(6.425, 2.6, 6.7625, 2.6); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 2.4, 6.7625, 2.4); // 0.3375 + 6.425 = 6.7625
            //if (resultsInterface.rbtnRp02.IsChecked == true)
            if (LastInputOutputSavedData.rbtnRp02_ResultsInterface.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 7, 5.54, 2.4 + 0.3, TextJustify.Left, "R");
                Contents.DrawText(ArialNormal, 4, 5.61, 2.4 + 0.3, TextJustify.Left, "p0.2");
                Contents.DrawText(ArialNormal, 7, 5.72, 2.4 + 0.3, TextJustify.Left, "/Rm");
            }
            //if (resultsInterface.rbtnRt05.IsChecked == true)
            if (LastInputOutputSavedData.rbtnRt05_ResultsInterface.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 7, 5.54, 2.4 + 0.3, TextJustify.Left, "R");
                Contents.DrawText(ArialNormal, 4, 5.61, 2.4 + 0.3, TextJustify.Left, "t0.5");
                Contents.DrawText(ArialNormal, 7, 5.72, 2.4 + 0.3, TextJustify.Left, "/Rm");
            }
            //if (resultsInterface.rbtnReH.IsChecked == true)
            if (LastInputOutputSavedData.rbtnReH_ResultsInterface.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 7, 5.54, 2.4 + 0.3, TextJustify.Left, "ReH/Rm");
            }
            //if (resultsInterface.rbtnReL.IsChecked == true)
            if (LastInputOutputSavedData.rbtnReL_ResultsInterface.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 7, 5.54, 2.4 + 0.3, TextJustify.Left, "ReL/Rm");
            }
            //Contents.DrawLine(6.425, 2.4, 6.425, 2.6);
            //Contents.DrawLine(6.7625, 2.4, 6.7625, 2.6);

            Contents.DrawLine(5.95, 2.55 + 0.3, 6.65, 2.55 + 0.3);
            Contents.DrawLine(5.95, 2.35 + 0.3, 6.65, 2.35 + 0.3);
            //if (resultsInterface.tfRRm.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 2.50, TextJustify.Left, " " + resultsInterface.tfRRm.Text);
            //}
            if (LastInputOutputSavedData.tfRRm_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfRRm = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRRm_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthtfRRm - 0.05, 2.4 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRRm_ResultsInterface);
            }
            Contents.DrawLine(5.95, 2.35 + 0.3, 5.95, 2.55 + 0.3);
            Contents.DrawLine(6.65, 2.35 + 0.3, 6.65, 2.55 + 0.3);
           
            //Re
            Contents.DrawText(ArialNormal, 8, 5.5, 2.15 + 0.3, TextJustify.Left, " Re");

            Contents.DrawLine(5.95, 2.3 + 0.3, 6.65, 2.3 + 0.3);
            Contents.DrawLine(5.95, 2.1 + 0.3, 6.65, 2.1 + 0.3);
            if (LastInputOutputSavedData.Re_ResultsInterface.Equals(string.Empty) == false && LastInputOutputSavedData.chbRe_ResultsInterface.Equals("True"))
            {
                widthRe = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.Re_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthRe - 0.05, 2.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.Re_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 2.15 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 2.1 + 0.3, 5.95, 2.3 + 0.3);
            Contents.DrawLine(6.65, 2.1 + 0.3, 6.65, 2.3 + 0.3);

            //E
            Contents.DrawText(ArialNormal, 8, 5.5, 1.9 + 0.3, TextJustify.Left, " E");

            Contents.DrawLine(5.95, 2.05 + 0.3, 6.65, 2.05 + 0.3);
            Contents.DrawLine(5.95, 1.85 + 0.3, 6.65, 1.85 + 0.3);
            if (LastInputOutputSavedData.E_ResultsInterface.Equals(string.Empty) == false && LastInputOutputSavedData.chbE_ResultsInterface.Equals("True"))
            {
                widthE = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.E_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 6.65 - widthE - 0.05, 1.9 + 0.3, TextJustify.Left, LastInputOutputSavedData.E_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 6.65, 1.9 + 0.3, TextJustify.Left, " MPa");
            Contents.DrawLine(5.95, 1.85 + 0.3, 5.95, 2.05 + 0.3);
            Contents.DrawLine(6.65, 1.85 + 0.3, 6.65, 2.05 + 0.3);

            //F
            //Contents.DrawLine(6.425, 2.4, 6.7625, 2.4); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 2.2, 6.7625, 2.2); // 0.3375 + 6.425 = 6.7625
            //Contents.DrawText(ArialNormal, 5.5, 6.425, 2.25, TextJustify.Left, " F");
            //Contents.DrawLine(6.425, 2.2, 6.425, 2.4);
            //Contents.DrawLine(6.7625, 2.2, 6.7625, 2.4);

            //Contents.DrawLine(6.7625, 2.4, 7.3125, 2.4); // 0.55 + 6.7625 = 7.3125 
            //Contents.DrawLine(6.7625, 2.2, 7.3125, 2.2); // 0.55 + 6.7625 = 7.3125 
            //if (resultsInterface.tfF.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 2.25, TextJustify.Left, " " + resultsInterface.tfF.Text + " kN");
            //}
            //Contents.DrawLine(6.7625, 2.2, 6.7625, 2.4);
            //Contents.DrawLine(7.3125, 2.2, 7.3125, 2.4);

            //Fm
            //Contents.DrawLine(6.425, 2.2, 6.7625, 2.2); // 0.3375 + 6.425 = 6.7625 
            //Contents.DrawLine(6.425, 2.0, 6.7625, 2.0); // 0.3375 + 6.425 = 6.7625
            //Contents.DrawText(ArialNormal, 5.5, 6.425, 2.05, TextJustify.Left, " Fm");
            //Contents.DrawLine(6.425, 2.0, 6.425, 2.2);
            //Contents.DrawLine(6.7625, 2.0, 6.7625, 2.2);

            //Contents.DrawLine(6.7625, 2.2, 7.3125, 2.2); // 0.55 + 6.7625 = 7.3125 
            //Contents.DrawLine(6.7625, 2.0, 7.3125, 2.0); // 0.55 + 6.7625 = 7.3125 
            //if (resultsInterface.tfFm.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.7625, 2.05, TextJustify.Left, " " + resultsInterface.tfFm.Text + " kN");
            //}
            //Contents.DrawLine(6.7625, 2.0, 6.7625, 2.2);
            //Contents.DrawLine(7.3125, 2.0, 7.3125, 2.2);

            //second column

            //Ag
            //Contents.DrawLine(7.3125, 3.6, 7.55, 3.6); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 3.4, 7.55, 3.4); // 0.2375 + 7.3125 = 7.55
            Contents.DrawText(ArialNormal, 8, 7, 3.65 + 0.3, TextJustify.Left, " Ag");
            //Contents.DrawLine(7.3125, 3.4, 7.3125, 3.6);
            //Contents.DrawLine(7.55, 3.4, 7.55, 3.6);

            //Contents.DrawLine(7.55, 3.6, 8,0, 3.6); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            Contents.DrawLine(7.3, 3.8 + 0.3, 8.1, 3.8 + 0.3); // 0.55 + 7.55 = 8.1 - 0.1125
            //gornja linija iscrtavanja za Agt
            Contents.DrawLine(7.3, 3.6 + 0.3, 8.1, 3.6 + 0.3); // 0.55 + 7.55 = 8.1 - 0.1125
            //if (resultsInterface.tfAg.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 7.65, 3.50, TextJustify.Left, " " + resultsInterface.tfAg.Text);
            //}
            if (LastInputOutputSavedData.tfAg_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfAg = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfAg_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfAg - 0.05, 3.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfAg_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 8.1, 3.65 + 0.3, TextJustify.Left, " %");
            Contents.DrawLine(7.3, 3.8 + 0.3, 7.3, 3.6 + 0.3);
            Contents.DrawLine(8.1, 3.8 + 0.3, 8.1, 3.6 + 0.3);

            //Agt
            //Contents.DrawLine(7.3125, 3.4, 7.55, 3.4); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 3.2, 7.55, 3.2); // 0.2375 + 7.3125 = 7.55
            Contents.DrawText(ArialNormal, 8, 7, 3.4 + 0.3, TextJustify.Left, " Agt");
            //Contents.DrawLine(7.3125, 3.2, 7.3125, 3.4);
            //Contents.DrawLine(7.55, 3.2, 7.55, 3.4);

            //Contents.DrawLine(7.65, 3.4, 8,0875, 3.4); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            Contents.DrawLine(7.3, 3.55 + 0.3, 8.1, 3.55 + 0.3); 
            //gornja linija iscrtavanja za A
            Contents.DrawLine(7.3, 3.35 + 0.3, 8.1, 3.35 + 0.3); // 0.55 + 7.55 = 8.1 - 0.1125
            //if (LastInputOutputSavedData.tfAgt_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True") ||*/ resultsInterface.chbRt05.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True")))
            {
                widthtfAgt = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfAgt_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfAgt - 0.05, 3.4 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfAgt_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 8.1, 3.4 + 0.3, TextJustify.Left, " %");
            Contents.DrawLine(7.3, 3.55 + 0.3, 7.3, 3.35 + 0.3);
            Contents.DrawLine(8.1, 3.55 + 0.3, 8.1, 3.35 + 0.3);



            //A
            //Contents.DrawLine(7.3125, 3.2, 7.55, 3.2); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 3.0, 7.55, 3.0); // 0.2375 + 7.3125 = 7.55
            Contents.DrawText(ArialNormal, 8, 7, 3.15 + 0.3, TextJustify.Left, " A"); 
            //Contents.DrawLine(7.3125, 3.0, 7.3125, 3.2);
            //Contents.DrawLine(7.55, 3.0, 7.55, 3.2);

            //Contents.DrawLine(7.65, 3.2, 8.0875, 3.2); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            Contents.DrawLine(7.3, 3.3 + 0.3, 8.1, 3.3 + 0.3); 
            //gornja linija iscrtavanja za At
            Contents.DrawLine(7.3, 3.1 + 0.3, 8.1, 3.1 + 0.3); 
            //if (resultsInterface.tfA.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 7.65, 3.10, TextJustify.Left, " " + resultsInterface.tfA.Text);
            //}
            if (LastInputOutputSavedData.tfA_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfA = Contents.DrawText(ArialNormal, 8, 8.1 - widthtfA - 0.05, 3.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfA_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfA - 0.05, 3.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfA_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 8.1, 3.15 + 0.3, TextJustify.Left, " %");
            Contents.DrawLine(7.3, 3.3 + 0.3, 7.3, 3.1 + 0.3);
            Contents.DrawLine(8.1, 3.3 + 0.3, 8.1, 3.1 + 0.3);

            //At
            //Contents.DrawLine(7.3125, 3.0, 7.55, 3.0); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 2.8, 7.55, 2.8); // 0.2375 + 7.3125 = 7.55
            Contents.DrawText(ArialNormal, 8, 7, 2.9 + 0.3, TextJustify.Left, " At");
            //Contents.DrawLine(7.3125, 2.8, 7.3125, 3.0);
            //Contents.DrawLine(7.55, 2.8, 7.55, 3.0);

            //Contents.DrawLine(7.65, 3.0, 8.0875, 3.0); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            Contents.DrawLine(7.3, 3.05 + 0.3, 8.1, 3.05 + 0.3); 
            //gornja linija iscrtavanja za Z
            Contents.DrawLine(7.3, 2.85 + 0.3, 8.1, 2.85 + 0.3); 
            //if (resultsInterface.tfAt.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 7.65, 2.90, TextJustify.Left, " " + resultsInterface.tfAt.Text);
            //}
            //if (LastInputOutputSavedData.tfAt_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True") ||*/ resultsInterface.chbRt05.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbRt05_ResultsInterface.Equals("True")))
            {
                widthtfAt = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfAt_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfAt - 0.05, 2.9 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfAt_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 8.1, 2.9 + 0.3, TextJustify.Left, " %");
            Contents.DrawLine(7.3, 3.05 + 0.3, 7.3, 2.85 + 0.3);
            Contents.DrawLine(8.1, 3.05 + 0.3, 8.1, 2.85 + 0.3);

            //Z
            //Contents.DrawLine(7.3125, 2.8, 7.55, 2.8); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 2.6, 7.55, 2.6); // 0.2375 + 7.3125 = 7.55
            Contents.DrawText(ArialNormal, 8, 7, 2.65 + 0.3, TextJustify.Left, " Z");
            //gornja linija iscrtavanja za n prva linija
            Contents.DrawLine(7.3, 2.8 + 0.3, 8.1, 2.8 + 0.3); 
            //gornja linija iscrtavanja za n druga linija
            Contents.DrawLine(7.3, 2.6 + 0.3, 8.1, 2.6 + 0.3); 
            //Contents.DrawLine(7.3125, 2.6, 7.3125, 2.8);
            //Contents.DrawLine(7.55, 2.6, 7.55, 2.8);

            //Contents.DrawLine(7.65, 2.8, 8.0875, 2.8); // 0.55 + 7.55 = 8.1 - 0.1125 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            //Contents.DrawLine(7.65, 2.65, 8.0875, 2.65); // 0.55 + 7.55 = 8.1 - 0.1125
            //if (resultsInterface.tfZ.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 7.65, 2.70, TextJustify.Left, " " + resultsInterface.tfZ.Text);
            //}
            if (File.Exists(xmlName) == true)
            {
                string myXmlString = String.Empty;
                List<string> myXmlStrings = File.ReadAllLines(xmlName).ToList();
                if (myXmlStrings.Count == 0)
                {
                    return;
                }
                foreach (string s in myXmlStrings)
                {
                    myXmlString += s;
                }

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(myXmlString);
                XmlNodeList xnList = xml.SelectNodes("/" + Constants.XML_roots_ROOT + "/" + Constants.XML_roots_Sadrzaj);

                foreach (XmlNode xn in xnList)
                {
                    LastInputOutputSavedData.tfZ_ResultsInterface = xn[Constants.XML_ResultsInterface_Z].InnerText;
                }
            }
            if (LastInputOutputSavedData.tfZ_ResultsInterface.Equals("100"))
            {
                LastInputOutputSavedData.tfZ_ResultsInterface = string.Empty;
            }
            if (LastInputOutputSavedData.tfZ_ResultsInterface.Equals(string.Empty) == false)
            {

                LastInputOutputSavedData.tfZ_ResultsInterface = LastInputOutputSavedData.tfZ_ResultsInterface;
                widthtfZ = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfZ_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfZ - 0.05, 2.65 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfZ_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 8.1, 2.65 + 0.3, TextJustify.Left, " %");
            Contents.DrawLine(7.3, 2.8 + 0.3, 7.3, 2.6 + 0.3);
            Contents.DrawLine(8.1, 2.8 + 0.3, 8.1, 2.6 + 0.3);


            //n
            //Contents.DrawLine(7.3125, 2.6, 7.55, 2.6); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 2.4, 7.55, 2.4); // 0.2375 + 7.3125 = 7.55
            Contents.DrawText(ArialNormal, 8, 7, 2.45 + 0.3, TextJustify.Left, " n");
            //Contents.DrawLine(7.3125, 2.4, 7.3125, 2.6);
            //Contents.DrawLine(7.55, 2.4, 7.55, 2.6);

            //Contents.DrawLine(7.65, 2.6, 8.0875, 2.6); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            Contents.DrawLine(7.3, 2.55 + 0.3, 8.1, 2.55 + 0.3);
            Contents.DrawLine(7.3, 2.35 + 0.3, 8.1, 2.35 + 0.3); 
            //if (LastInputOutputSavedData.tfn_ResultsInterface.Equals(string.Empty) == false)
            if (/*LastInputOutputSavedData.chbn_ResultsInterface.Equals("True") ||*/ resultsInterface.chbn.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbn_ResultsInterface.Equals("True")))
            {
                widthtfn = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfn_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfn - 0.05, 2.4 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfn_ResultsInterface);
            }
            Contents.DrawLine(7.3, 2.55 + 0.3, 7.3, 2.35 + 0.3);
            Contents.DrawLine(8.1, 2.55 + 0.3, 8.1, 2.35 + 0.3);

            //druga linija za polje n-a
            Contents.DrawText(ArialNormal, 15, 7.2, 2.25 + 0.3, TextJustify.Left, "{");
            Contents.DrawLine(7.3, 2.3 + 0.3, 8.1, 2.3 + 0.3);
            Contents.DrawLine(7.3, 2.1 + 0.3, 8.1, 2.1 + 0.3);
            Contents.DrawLine(7.3, 2.3 + 0.3, 7.3, 2.1 + 0.3);
            Contents.DrawLine(8.1, 2.3 + 0.3, 8.1, 2.1 + 0.3);
            //widthtfn2 = Contents.DrawText(ArialNormal, 10, 0, 0, TextJustify.Left, "10-20%", false);
            if (/*LastInputOutputSavedData.chbn_ResultsInterface.Equals("True") ||*/ resultsInterface.chbn.IsChecked == true && _makeFromLastSample == true || (LastInputOutputSavedData.chbn_ResultsInterface.Equals("True")))
            {
                widthtfn2 = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, OptionsInPlottingMode.BeginIntervalForN + "-" + OptionsInPlottingMode.EndIntervalForN + "%", false);
                Contents.DrawText(ArialNormal, 8, 8.1 - widthtfn2 - 0.05, 2.15 + 0.3, TextJustify.Left, OptionsInPlottingMode.BeginIntervalForN + "-" + OptionsInPlottingMode.EndIntervalForN + "%");
            }

            //RmaxWithPoint
            //Contents.DrawLine(7.3125, 2.4, 7.55, 2.4); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 2.2, 7.55, 2.2); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawText(ArialNormal, 15, 7.3240, 2.32, TextJustify.Left, ".");//po x osi 7.3240 - 7.3125 = 0.0115 ; po y osi 2.32 - 2.25 = 0.07
            //Contents.DrawText(ArialNormal, 5.5, 7.3125, 2.25, TextJustify.Left, " Rmax");
            //Contents.DrawLine(7.3125, 2.2, 7.3125, 2.4);
            //Contents.DrawLine(7.55, 2.2, 7.55, 2.4);

            ////Contents.DrawLine(7.55, 2.4, 8,1, 2.4); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            //Contents.DrawLine(7.55, 2.2, 8.1, 2.2); // 0.55 + 7.55 = 8.1
            //if (resultsInterface.tfRmax.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 7.55, 2.25, TextJustify.Left, " " + resultsInterface.tfRmax.Text + " MPa/s");
            //}
            //Contents.DrawLine(7.55, 2.4, 7.55, 2.2);
            //Contents.DrawLine(8.1, 2.4, 8.1, 2.2);

            //EmaxWithPoint
            //Contents.DrawLine(7.3125, 2.2, 7.55, 2.2); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawLine(7.3125, 2.0, 7.55, 2.0); // 0.2375 + 7.3125 = 7.55
            //Contents.DrawText(ArialNormal, 15, 7.3280, 2.12, TextJustify.Left, ".");//po x osi 7.3240 - 7.3125 = 0.0115 ; po y osi 2.32 - 2.25 = 0.07
            //Contents.DrawText(ArialNormal, 8.5, 7.3280, 2.05, TextJustify.Left, "e");
            //Contents.DrawText(ArialNormal, 5, 7.3825, 2.05, TextJustify.Left, "max");
            //Contents.DrawLine(7.3125, 2.0, 7.3125, 2.2);
            //Contents.DrawLine(7.55, 2.0, 7.55, 2.2);

            ////Contents.DrawLine(7.55, 2.2, 8,1, 2.2); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            //Contents.DrawLine(7.55, 2.0, 8.1, 2.0); // 0.55 + 7.55 = 8.1
            //if (resultsInterface.tfE2.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 7.55, 2.05, TextJustify.Left, " " + resultsInterface.tfE2.Text + " %/s");
            //}
            //Contents.DrawLine(7.55, 2.2, 7.55, 2.0);
            //Contents.DrawLine(8.1, 2.2, 8.1, 2.0);
        }

        private void setRemarksOfTesting() 
        {
            Contents.DrawLine(0.5, 1.7 + 0.3, 4.6, 1.7 + 0.3);
            Contents.DrawLine(0.5, 1.5 + 0.3, 4.6, 1.5 + 0.3);
            Contents.DrawText(ArialNormal, 8, 0.5, 1.55 + 0.3, TextJustify.Left, " NAPOMENA");
            Contents.DrawLine(0.5, 1.5 + 0.3, 0.5, 1.7 + 0.3);
            Contents.DrawLine(4.6, 1.5 + 0.3, 4.6, 1.7 + 0.3);


            Contents.DrawLine(0.5, 0.6 + 0.3, 4.6, 0.6 + 0.3);
            Contents.DrawLine(0.5, 0.6 + 0.3, 0.5, 1.5 + 0.3);
            Contents.DrawLine(4.6, 0.6 + 0.3, 4.6, 1.5 + 0.3);

            
            //var textRangeNapomena = new TextRange(onlineHeader.RemarkOfTesting.rtfNapomena.Document.ContentStart, onlineHeader.RemarkOfTesting.rtfNapomena.Document.ContentEnd);
            string textRangeNapomena = LastInputOutputSavedData.rtfNapomena_RemarkOfTesting;
            //string textRangeNapomenaString = textRangeNapomena.Text;
            string textRangeNapomenaString = textRangeNapomena;
            textRangeNapomenaString = textRangeNapomenaString.Replace("\r\n", String.Empty);

            List<string> remarksList = textRangeNapomenaString.Split(' ').ToList();
            double beginHeight = 1.30;
            double stepForHeight = 0.2;
            int counter = 0;
            double currentHeight;
            string currStringForWritting = string.Empty;
            string stringForWritting = string.Empty;
            int indexRowNumber = 0;

            for (int i = 0; i < remarksList.Count; i++)
            {
                while (counter < 60)
                {
                    counter = counter + remarksList.ElementAt(i).Length;

                    if (counter < 60)
                    {
                        currStringForWritting = currStringForWritting + remarksList.ElementAt(i) + " ";
                        stringForWritting = currStringForWritting;
                    }

                    i++;
                    //ako smo dosli do poslednje reci izadji iz while petlje i upisi poslednji string koji si zabelezio
                    if (i >= remarksList.Count)
                    {
                        break;
                    }
                    
                }
                //vrati se za jednu rec unazad da ne bi izasao iz okvira polja predvidjenog za pisanje napomene
                if (counter > 60)
                {
                    i--;
                    counter = 0;
                    stringForWritting = currStringForWritting;
                    currStringForWritting = string.Empty;
                }
                Contents.DrawText(ArialNormal, 8, 0.55, beginHeight - indexRowNumber * stepForHeight + 0.3, TextJustify.Left, stringForWritting);
                stringForWritting = string.Empty;
                indexRowNumber++;
            }

            
        }

        private void setFFmAndChangedParameters() 
        {
            Contents.DrawText(ArialBold, 8, 5.95, 1.45 + 0.3, TextJustify.Left, "OPTEREĆENJE");

            //F

            Contents.DrawText(ArialNormal, 8, 4.65, 1.15 + 0.3, TextJustify.Left, "F");


            Contents.DrawLine(4.9, 1.3 + 0.3, 5.9, 1.3 + 0.3);
            Contents.DrawLine(4.9, 1.1 + 0.3, 5.9, 1.1 + 0.3); 
            //if (resultsInterface.tfF.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 5.1, 1.70, TextJustify.Left, " " + resultsInterface.tfF.Text);
            //}
            if (LastInputOutputSavedData.tfF_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfF = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfF_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 5.9 - widthtfF - 0.05, 1.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfF_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 5.9, 1.15 + 0.3, TextJustify.Left, " N");
            Contents.DrawLine(4.9, 1.1 + 0.3, 4.9, 1.3 + 0.3);
            Contents.DrawLine(5.9, 1.1 + 0.3, 5.9, 1.3 + 0.3);

            //Fm

            Contents.DrawText(ArialNormal, 8, 4.65, 0.90 + 0.3, TextJustify.Left, "Fm");


            Contents.DrawLine(4.9, 1.05 + 0.3, 5.9, 1.05 + 0.3);
            Contents.DrawLine(4.9, 0.85 + 0.3, 5.9, 0.85 + 0.3); 
            //if (resultsInterface.tfFm.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 5.1, 1.50, TextJustify.Left, " " + resultsInterface.tfFm.Text);
            //}
            if (LastInputOutputSavedData.tfFm_ResultsInterface.Equals(string.Empty) == false)
            {
                widthtfFm = Contents.DrawText(ArialNormal, 8, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfFm_ResultsInterface, false);
                Contents.DrawText(ArialNormal, 8, 5.9 - widthtfFm - 0.05, 0.90 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfFm_ResultsInterface);
            }
            Contents.DrawText(ArialNormal, 8, 5.9, 0.90 + 0.3, TextJustify.Left, " N");
            Contents.DrawLine(4.9, 0.85 + 0.3, 4.9, 1.0 + 0.35);
            Contents.DrawLine(5.9, 0.85 + 0.3, 5.9, 1.05 + 0.3);

            //RmaxWithPoint
            Contents.DrawText(ArialNormal, 25, 6.185, 1.27 + 0.3, TextJustify.Left, ".");//po x osi 6.6840 - 6.6725 = 0.0115 ; po y osi 1.77 - 1.70 = 0.07
            Contents.DrawText(ArialNormal, 8, 6.2, 1.15 + 0.3, TextJustify.Left, "Rmax");

            Contents.DrawLine(6.56, 1.3 + 0.3, 7.66, 1.3 + 0.3); // 0.55 + 7.55 = 8.1 isrtavanje ove linije nesto ne valja pa je zato zakomentarisan
            Contents.DrawLine(6.56, 1.1 + 0.3, 7.66, 1.1 + 0.3); // 0.55 + 7.55 = 8.1
            //if (resultsInterface.tfRmax.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.96, 1.70, TextJustify.Left, " " + resultsInterface.tfRmax.Text);
            //}
            //if (LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface.Equals(string.Empty) == false)
            //{
            if (/*LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("False") ||*/ (resultsInterface.chbRmax.IsChecked == false && _makeFromLastSample == true) || (LastInputOutputSavedData.chbRmax_ResultsInterface.Equals("False")))
                {
                }
                else
                {
                    widthtfRmax = Contents.DrawText(ArialNormal, 10, 0, 0 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface, false);
                    Contents.DrawText(ArialNormal, 10, 7.66 - widthtfRmax - 0.05, 1.15 + 0.3, TextJustify.Left, LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface);
                    resultsInterface.tfRmax.Text = LastInputOutputSavedData.tfRmaxWithPoint_ResultsInterface.ToString();
                }
            //}
            Contents.DrawText(ArialNormal, 10, 7.66, 1.15 + 0.3, TextJustify.Left, " " + resultsInterface.tblRmaxMeasure.Text);
            Contents.DrawLine(6.56, 1.3 + 0.3, 6.56, 1.1 + 0.3);
            Contents.DrawLine(7.66, 1.3 + 0.3, 7.66, 1.1 + 0.3);


            //E2WithPoint
            //Contents.DrawText(ArialNormal, 25, 6.183, 0.98, TextJustify.Left, ".");
            Contents.DrawText(ArialNormal, 8, 6.2, 0.90 + 0.3, TextJustify.Left, "Vc");
            //if (OptionsInOnlineMode.isE2E4BorderSelected == true)
            if (LastInputOutputSavedData.isE2E4BorderSelected.Equals("True"))
            {
                Contents.DrawText(ArialNormal, 6, 6.33, 0.88 + 0.3, TextJustify.Left, "(R2)");
            }
         
            else
            {
                Contents.DrawText(ArialNormal, 6, 6.33, 0.88 + 0.3, TextJustify.Left, "(R3)");
            }


            Contents.DrawLine(/*6.4*/6.56, 1.05 + 0.3, 7.66, 1.05 + 0.3); // 0.55 + 7.55 = 8.1 
            Contents.DrawLine(/*6.4*/6.56, 0.85 + 0.3, 7.66, 0.85 + 0.3); // 0.55 + 7.55 = 8.1
            //if (resultsInterface.tfE2.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.96, 1.50, TextJustify.Left, " " + resultsInterface.tfE2.Text);
            //}
            //if (LastInputOutputSavedData.e2Min_ResultsInterface.Equals(string.Empty) == false && LastInputOutputSavedData.e2Max_ResultsInterface.Equals(string.Empty) == false)
            //{
            if (/*LastInputOutputSavedData.chbe2_ResultsInterface.Equals("False") ||*/ (resultsInterface.chbe2.IsChecked == false && _makeFromLastSample == true) || (LastInputOutputSavedData.chbe2_ResultsInterface.Equals("False")))
                {
                }
                else
                {
                    LastInputOutputSavedData.e2Max_ResultsInterface = plotting.E2e4CalculationAfterManualFitting.ArrayE2Interval.Max().ToString();
                    plotting.OnlineModeInstance.ResultsInterface.tfE2.Text = LastInputOutputSavedData.e2Max_ResultsInterface;
                    widthtfE2 = Contents.DrawText(ArialNormal, 10, 0, 0 + 0.3, TextJustify.Left, /*LastInputOutputSavedData.e2Min_ResultsInterface + " - " +*/ LastInputOutputSavedData.e2Max_ResultsInterface, false);
                    Contents.DrawText(ArialNormal, 10, 7.66 - widthtfE2 - 0.05, 0.90 + 0.3, TextJustify.Left, /*LastInputOutputSavedData.e2Min_ResultsInterface + " - " +*/ LastInputOutputSavedData.e2Max_ResultsInterface);                  
                }
            //}
            Contents.DrawText(ArialNormal, 10, 7.66, 0.90 + 0.3, TextJustify.Left, " " + resultsInterface.tblE2Measure.Text);
            Contents.DrawLine(/*6.4*/6.56, 1.05 + 0.3,/*6.4*/6.56, 0.85 + 0.3);
            Contents.DrawLine(7.66, 1.05 + 0.3, 7.66, 0.85 + 0.3);

            //E4WithPoint
            //Contents.DrawText(ArialNormal, 25, 6.183, 0.73, TextJustify.Left, ".");
            Contents.DrawText(ArialNormal, 8, 6.2, 0.65 + 0.3, TextJustify.Left, "Vc");
            Contents.DrawText(ArialNormal, 6, 6.33, 0.63 + 0.3, TextJustify.Left, "(R4)");


            Contents.DrawLine(/*6.4*/6.56, 0.8 + 0.3, 7.66, 0.8 + 0.3);
            Contents.DrawLine(/*6.4*/6.56, 0.6 + 0.3, 7.66, 0.6 + 0.3); 
            //if (resultsInterface.tfE2.Text.Equals(string.Empty) == false)
            //{
            //    Contents.DrawText(ArialNormal, 5.5, 6.96, 1.30, TextJustify.Left, " " + resultsInterface.tfE4.Text);
            //}
            //if (LastInputOutputSavedData.e4Min_ResultsInterface.Equals(string.Empty) == false && LastInputOutputSavedData.e4Max_ResultsInterface.Equals(string.Empty) == false)
            //{
            if (/*LastInputOutputSavedData.chbe4_ResultsInterface.Equals("False") ||*/ (resultsInterface.chbe4.IsChecked == false && _makeFromLastSample == true) || (LastInputOutputSavedData.chbe4_ResultsInterface.Equals("False")))
                {
                }
                else
                {
                    LastInputOutputSavedData.e4Max_ResultsInterface = plotting.E2e4CalculationAfterManualFitting.ArrayE4Interval.Max().ToString();
                    plotting.OnlineModeInstance.ResultsInterface.tfE4.Text = LastInputOutputSavedData.e4Max_ResultsInterface;
                    widthtfE4 = Contents.DrawText(ArialNormal, 10, 0, 0 + 0.3, TextJustify.Left, /*LastInputOutputSavedData.e4Min_ResultsInterface + " - " +*/ LastInputOutputSavedData.e4Max_ResultsInterface, false);
                    Contents.DrawText(ArialNormal, 10, 7.66 - widthtfE4 - 0.05, 0.65 + 0.3, TextJustify.Left, /*LastInputOutputSavedData.e4Min_ResultsInterface + " - " +*/ LastInputOutputSavedData.e4Max_ResultsInterface);
                    resultsInterface.tfE4.Text = LastInputOutputSavedData.e4Max_ResultsInterface.ToString();
                }
            //}
            Contents.DrawText(ArialNormal, 10, 7.66, 0.65 + 0.3, TextJustify.Left, " " + resultsInterface.tblE4Measure.Text);
            Contents.DrawLine(/*6.4*/6.56, 0.6 + 0.3, /*6.4*/6.56, 0.8 + 0.3);
            Contents.DrawLine(7.66, 0.6 + 0.3, 7.66, 0.8 + 0.3);

        }


        private void setPrintingTimeAndOperator() 
        {
            //Contents.DrawLine(4, 0.8, 4, 0.8);
            //Contents.DrawLine(0.5, 0.6, 4, 0.6);
            //Contents.DrawLine(0.5, 0.6, 0.5, 0.8);
            //Contents.DrawLine(4, 0.6, 4, 0.8);

            Contents.DrawText(ArialNormal, 8, 0.5, 0.25 + 0.3, TextJustify.Left, " VREME ŠTAMPANJA");

            Contents.DrawLine(1.7, 0.4 + 0.3, 2.5, 0.4 + 0.3);
            Contents.DrawLine(1.7, 0.2 + 0.3, 2.5, 0.2 + 0.3);
            Contents.DrawLine(1.7, 0.2 + 0.3, 1.7, 0.4 + 0.3);
            Contents.DrawLine(2.5, 0.2 + 0.3, 2.5, 0.4 + 0.3);

            int hours = DateTime.Now.Hour;
            int minutes = DateTime.Now.Minute;
            int seconds = DateTime.Now.Second;
            string hoursStr = String.Empty;
            string minutesStr = String.Empty;
            string secondsStr = String.Empty;
            if (hours < 10)
            {
                hoursStr = "0" + hours;
            }
            else
            {
                hoursStr = hours.ToString();
            }
            if (minutes < 10)
            {
                minutesStr = "0" + minutes;
            }
            else
            {
                minutesStr = minutes.ToString();
            }
            if (seconds < 10)
            {
                secondsStr = "0" + seconds;
            }
            else
            {
                secondsStr = seconds.ToString();
            }
            string timeNow = hoursStr + ':' + minutesStr + ':' + secondsStr;
            Contents.DrawText(ArialNormal, 8, 1.7, 0.25 + 0.3, TextJustify.Left, " " + timeNow);


            Contents.DrawText(ArialNormal, 8, 2.8, 0.25 + 0.3, TextJustify.Left, " DATUM ŠTAMPANJA");
            Contents.DrawLine(4, 0.4 + 0.3, 4.75, 0.4 + 0.3);
            Contents.DrawLine(4, 0.2 + 0.3, 4.75, 0.2 + 0.3);
            Contents.DrawLine(4, 0.2 + 0.3, 4, 0.4 + 0.3);
            Contents.DrawLine(4.75, 0.2 + 0.3, 4.75, 0.4 + 0.3);
            Contents.DrawText(ArialNormal, 8, 4, 0.25 + 0.3, TextJustify.Left, " " + DateTime.Now.ToShortDateString());

            Contents.DrawText(ArialNormal, 8, 4.95, 0.25 + 0.3, TextJustify.Left, " ISPITIVAČ");
            Contents.DrawLine(5.6, 0.4 + 0.3, 7.25, 0.4 + 0.3);
            Contents.DrawLine(5.6, 0.2 + 0.3, 7.25, 0.2 + 0.3);
            Contents.DrawLine(5.6, 0.2 + 0.3, 5.6, 0.4 + 0.3);
            Contents.DrawLine(7.25, 0.2 + 0.3, 7.25, 0.4 + 0.3);
            //Contents.DrawText(ArialNormal, 5.5, 4.7, 0.30, TextJustify.Left, " " + onlineHeader.GeneralData.tfOperator.Text);
            Contents.DrawText(ArialNormal, 8, 5.6, 0.25 + 0.3, TextJustify.Left, " " + LastInputOutputSavedData.tfOperator_GeneralData);
        }

        //public void CreatePage2Contents()
        //{
        //    PdfImage Image1 = new PdfImage(Document, System.Environment.CurrentDirectory + "\\slika2.png");
        //    //SizeD ImageSize = Image1.ImageSizeAndDensity(0.8 * AreaWidth, 0.8 * AreaHeight, 150.0);
        //    //SizeD ImageSize = Image1.ImageSizeAndDensity(DispWidth, 2 * AreaHeight, 150.0);

        //    PointD p1 = new PointD(0.2, 8);
        //    PointD p2 = new PointD(8, 8);

        //    // add contents to page
        //    Contents = AddPageToDocument(2);
        //    //Contents.DrawImage(Image1, 0.2, p1.Y - 4.25, ImageSize.Width, ImageSize.Height);
        //    // draw examples
        //    //Example1b(AreaX2, AreaY3);
        //    //Example1c(AreaX1, AreaY2);
        //    //Example1d(AreaX2, AreaY2);
        //    //Example1e(AreaX1, AreaY1);
        //    //Example1f(AreaX2, AreaY1);

        //    // exit
        //    return;
        //}

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



    }// end of class
}
