/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfQRCode
//	Display QR Code as image resource.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2014 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	Version History:
//
//	Version 1.0 2013/04/01
//		Original revision
//	Version 1.1 2013/04/09
//		Allow program to be compiled in regions that define
//		decimal separator to be non period (comma)
//	Version 1.2 2013/07/21
//		The original revision supported image resources with
//		jpeg file format only.
//		Version 1.2 support all image files acceptable to Bitmap class.
//		See ImageFormat class. The program was tested with:
//		Bmp, Gif, Icon, Jpeg, Png and Tiff.
//	Version 1.3 2014/02/07
//		Fix bug in PdfContents.DrawBezierNoP2(PointD P1, PointD P3)
//	Version 1.4 2014/03/01
//		PdfContents
//		Add method: public void TranslateScaleRotate(Double OrigX,
//			Double OrigY, Double ScaleX, Double ScaleY, Double Rotate);
//		Add method: public String ReverseString(Strint Text);
//		Fix some problems with DrawXObject(...); methods
//		PdfFont
//		Extensive changes to font substitution (see article)
//		PdfImage
//		Add method: public SizeD ImageSizeAndDensity(Double Width,
//			Double Height, Double Density);
//		This method controls the size of the bitmap (see article)
//		Add method: public void SetImageQuality(Int32 ImageQuality);
//		This method controls the image quality (see article)
//		PdfTilingPattern
//		Fix bug in public static PdfTilingPattern SetWeavePattern(...);
//	Version 1.5 2014/05/05
//		Add barcode feature. Supported barcodes are:
//		Code-128, Code39, UPC-A, EAN-13
//	Version 1.6 2014/07/09
//		Fix FontApi unanaged code resource disposition.
//		Clear PdfDocument object after CreateFile.
//	Version 1.7 2014/08/25
//		Add encryption support
//		Add Web link support
//		Add QRCode support
//		Change compression to .net System.io.compression
//
/////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
public class PdfQRCode : PdfObject
	{
	private Byte[,]		QRCodeMatrix;
	public  Int32		MatrixDimension;
	private Int32		ModuleSize = 4;
	private Int32		QuietZone = 16;
	private PdfObject	ImageLengthObject;

	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfQRCode
			(
			PdfDocument		Document,
			String			DataString,
			ErrorCorrection	ErrorCorrection
			) : base(Document, true, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// create QR Code object
		QREncoder Encoder = new QREncoder();
		QRCodeMatrix = Encoder.EncodeQRCode(DataString, ErrorCorrection);
		MatrixDimension = Encoder.MatrixDimension;

		// create stream length object
		ImageLengthObject = new PdfObject(Document, false);
		AddToDictionary("/Length", ImageLengthObject.ObjectNumber.ToString() + " 0 R");

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfQRCode
			(
			PdfDocument		Document,
			String[]		SegDataString,
			ErrorCorrection	ErrorCorrection
			) : base(Document, true, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// create QR Code object
		QREncoder Encoder = new QREncoder();
		QRCodeMatrix = Encoder.EncodeQRCode(SegDataString, ErrorCorrection);
		MatrixDimension = Encoder.MatrixDimension;

		// create stream length object
		ImageLengthObject = new PdfObject(Document, false);
		AddToDictionary("/Length", ImageLengthObject.ObjectNumber.ToString() + " 0 R");

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set module size and quiet zone size
	////////////////////////////////////////////////////////////////////

	public void SetModuleSize
			(
			Int32	ModuleSize,
			Int32	QuietZone
			)
		{
		this.ModuleSize = ModuleSize;
		this.QuietZone = QuietZone;
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Image width
	////////////////////////////////////////////////////////////////////

	public Int32 ImageWidth
		{
		get
			{
			return(MatrixDimension * ModuleSize + 2 * QuietZone);
			}
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile
			(
			BinaryWriter PdfFile
			)
		{
		// write object header
		PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("{0} 0 obj\n", ObjectNumber)));

		// image width
		String ImageWidthStr = ImageWidth.ToString();

		// add items to dictionary
		AddToDictionary("/Subtype", "/Image");
		AddToDictionary("/Width", ImageWidthStr);
		AddToDictionary("/Height", ImageWidthStr);
		AddToDictionary("/Filter", "/DCTDecode");
		AddToDictionary("/ColorSpace", "/DeviceRGB");
		AddToDictionary("/BitsPerComponent", "8");

		// write dictionary
		DictionaryToPdfFile(PdfFile);

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("stream\n"));

		// save pdf file position
		Int64 streamStart = PdfFile.BaseStream.Position;

		// debug
		if(Document.Debug)
			{
			PdfFile.Write(Encoding.ASCII.GetBytes("*** IMAGE PLACE HOLDER ***"));
			}

		// copy image file to output file
		else
			{
			// create bitmap image
			Bitmap Image = QRCodeImage();

			// create memory stream
			MemoryStream MS = new MemoryStream();

			// save image to memory stream
			Image.Save(MS, ImageFormat.Jpeg);

			// image byte array
			Byte[] ByteContents = MS.GetBuffer();

			// encryption
			if(Document.Encryption != null) ByteContents = Document.Encryption.EncryptByteArray(ObjectNumber, ByteContents);

			// write memory stream internal buffer to PDF file
			PdfFile.Write(ByteContents);

			// close and dispose memory stream
			MS.Close();

			// dispose bitmap resources
			Image.Dispose();
			}

		// save stream length
		ImageLengthObject.ContentsString = new StringBuilder(((Int32) (PdfFile.BaseStream.Position - streamStart)).ToString());

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("\nendstream\nendobj\n"));

		// invoke garbage collector
		GC.Collect();
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Convert QRCode boolean matrix into QRCode image
	////////////////////////////////////////////////////////////////////

	private Bitmap QRCodeImage()
		{
		// create white and black brushes
		SolidBrush BrushWhite = new SolidBrush(Color.White);
		SolidBrush BrushBlack = new SolidBrush(Color.Black);

		// create picture object and make it white
		Int32 PictureSide = ImageWidth;
		Bitmap Picture = new Bitmap(PictureSide, PictureSide);
		Graphics Graphics = Graphics.FromImage(Picture);
		Graphics.FillRectangle(BrushWhite, 0, 0, PictureSide, PictureSide);

		// paint QR Code image
		for(Int32 Row = 0; Row < MatrixDimension; Row++) for(Int32 Col = 0; Col < MatrixDimension; Col++)
			{
			if((QRCodeMatrix[Row, Col] & 1) != 0) Graphics.FillRectangle(BrushBlack, QuietZone + Col * ModuleSize, QuietZone + Row * ModuleSize, ModuleSize, ModuleSize);
			}
		Graphics.Dispose();
		return(Picture);
		}
	}
}
