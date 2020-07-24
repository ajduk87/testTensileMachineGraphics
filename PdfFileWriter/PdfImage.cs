/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfImage
//	PDF Image resource.
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
public class PdfImage : PdfObject, IDisposable
	{
	public  Double			WidthPt;
	public  Double			HeightPt;
	public	Int32			WidthPix;
	public	Int32			HeightPix;
	private	Int32			NewWidthPix;
	private	Double			NewDensity;
	private	Int32			NewHeightPix;
	private Int32			ImageQuality;
	private String			ImageFileName;
	private PdfObject		ImageLengthObject;
	private Bitmap			Picture;

	////////////////////////////////////////////////////////////////////
	// Constructor
	////////////////////////////////////////////////////////////////////

	public PdfImage
			(
			PdfDocument		Document,
			String			ImageFileName
			) : base(Document, true, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// save image file name
		this.ImageFileName = ImageFileName;

		// test exitance
		if(!File.Exists(ImageFileName)) throw new ApplicationException("Image file " + ImageFileName + " does not exist");

		// get file length
		FileInfo FI = new FileInfo(ImageFileName);
		Int64 ImageFileLength = FI.Length;
		if(ImageFileLength >= 0x40000000) throw new ApplicationException("Image file " + ImageFileName + " too long");

		// image width and height in pixels
		WidthPix = 0;
		HeightPix = 0;
		ImageQuality = 100;

		try
			{
			// load the image to get image info
			Picture = new Bitmap(ImageFileName);

			// get image width and height in pixels
			WidthPix = Picture.Width;
			HeightPix = Picture.Height;

			// image width and height in points
			WidthPt = (Double) WidthPix * 72.0 / Picture.HorizontalResolution;
			HeightPt = (Double) HeightPix * 72.0 / Picture.VerticalResolution;
			}

		catch(ArgumentException)
			{
			throw new ApplicationException("Invalid image file: " + ImageFileName);
			}

		// create stream length object
		ImageLengthObject = new PdfObject(Document, false);
		AddToDictionary("/Length", ImageLengthObject.ObjectNumber.ToString() + " 0 R");

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate height from width to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	private Double HeightFromWidth
			(
			Double Width
			)
		{
		return(Width * HeightPix / WidthPix);
		}

	////////////////////////////////////////////////////////////////////
	// Calculate width from height to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	private Double WidthFromHeight
			(
			Double Height
			)
		{
		return(Height * WidthPix / HeightPix);
		}

	////////////////////////////////////////////////////////////////////
	// Calculate best fit to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	public SizeD ImageSize
			(
			SizeD InputSize
			)
		{
		SizeD OutputSize = new SizeD();
		OutputSize.Height = HeightFromWidth(InputSize.Width);
		if(OutputSize.Height <= InputSize.Height)
			{
			OutputSize.Width = InputSize.Width;
			}
		else
			{
			OutputSize.Width = WidthFromHeight(InputSize.Height);
			OutputSize.Height = InputSize.Height;
			}
		return(OutputSize);
		}

	////////////////////////////////////////////////////////////////////
	// Calculate best fit to preserve aspect ratio
	////////////////////////////////////////////////////////////////////

	public SizeD ImageSize
			(
			Double	Width,
			Double	Height
			)
		{
		SizeD OutputSize = new SizeD();
		OutputSize.Height = HeightFromWidth(Width);
		if(OutputSize.Height <= Height)
			{
			OutputSize.Width = Width;
			}
		else
			{
			OutputSize.Width = WidthFromHeight(Height);
			OutputSize.Height = Height;
			}
		return(OutputSize);
		}

////////////////////////////////////////////////////////////////////
// set image density
////////////////////////////////////////////////////////////////////

	public SizeD ImageSizeAndDensity
			(
			double	Width,
			double	Height,
			double	Density
			)
		{
		// make sure width and height have the same ratio as original image
		SizeD Size = ImageSize(Width, Height);
	
		// convert to pixels
		Int32 NewWidth = (Int32) (Density * Size.Width + 0.5);
		Int32 NewHeight = (Int32) (Density * Size.Height + 0.5);
	
	    //if new size is greater than original do nothing
        if(NewWidth >= WidthPix || NewHeight >= HeightPix)
            {
            this.NewWidthPix = 0;
            this.NewHeightPix = 0;
            this.NewDensity = 0;
            }
        else
            {
			this.NewWidthPix = NewWidth;
			this.NewHeightPix = NewHeight;
			this.NewDensity = Density;
			}
		return(Size);
		}

	////////////////////////////////////////////////////////////////////
	// set image quality
	////////////////////////////////////////////////////////////////////

	public void SetImageQuality(int ImageQuality)
		{
		if(ImageQuality < 0 || ImageQuality > 100) throw new ApplicationException("Image file " + ImageFileName + " quality must be 0 to 100");
		this.ImageQuality = ImageQuality;
		return;
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

		// change image density
		if(NewDensity > 0)
			{
			WidthPix = NewWidthPix;
			HeightPix = NewHeightPix;
			}
	
		// add items to dictionary
		AddToDictionary("/Subtype", "/Image");
		AddToDictionary("/Width", WidthPix.ToString());
		AddToDictionary("/Height", HeightPix.ToString());
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
			// create memory stream
			MemoryStream MS = new MemoryStream();

			// change density
			if(NewDensity > 0) Picture = new Bitmap(Picture, WidthPix, HeightPix);

			// image quality is not 100
			if(ImageQuality != 100)
				{
				// build EncoderParameter object for image quality
				EncoderParameters EncoderParameters = new EncoderParameters(1);
				EncoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, ImageQuality);

				// save in jpeg format with specified quality
				Picture.Save(MS, GetEncoderInfo("image/jpeg"), EncoderParameters);
				}

			else
				{
				// save in jpeg format with 100% quality
				Picture.Save(MS, ImageFormat.Jpeg);
				}

			// image byte array
			Byte[] ByteContents = MS.GetBuffer();

			// encryption
			if(Document.Encryption != null) ByteContents = Document.Encryption.EncryptByteArray(ObjectNumber, ByteContents);

			// write memory stream internal buffer to PDF file
			PdfFile.Write(ByteContents);

			// close and dispose memory stream
			MS.Close();
			MS = null;

			// dispose of the image
			Picture.Dispose();
			Picture = null;
			}

		// save stream length
		ImageLengthObject.ContentsString = new StringBuilder(((Int32) (PdfFile.BaseStream.Position - streamStart)).ToString());

		// output stream
		PdfFile.Write(Encoding.ASCII.GetBytes("\nendstream\nendobj\n"));

		// clear memory
		GC.Collect();
		return;
		}

 	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

   private ImageCodecInfo GetEncoderInfo(String mimeType)
	    {
        int Index;
        ImageCodecInfo[] Encoders = ImageCodecInfo.GetImageEncoders();
        for(Index = 0; Index < Encoders.Length; ++Index)
	        {
            if(Encoders[Index].MimeType == mimeType) return(Encoders[Index]);
	        }
        throw new ApplicationException("Image file " + ImageFileName + " image/jpeg encoder does not exist");;
		}

	////////////////////////////////////////////////////////////////////
	// Dispose
	////////////////////////////////////////////////////////////////////

	public void Dispose()
		{
		// release bitmap
		Picture.Dispose();

		// exit
		return;		
		}
	}
}
