/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfIndirectString
//	Indirect string object. Used for web link.
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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
public class PdfIndirectString : PdfObject
	{
	public PdfIndirectString
			(
			PdfDocument		Document,
			StringBuilder	PdfText
			) : base(Document, false)
		{
		ContentsString = PdfText;
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
		// no encryption
		if(Document.Encryption == null)
			{
			// call PdfObject routine
			base.WriteObjectToPdfFile(PdfFile);
			return;
			}

		// write object header
		PdfFile.Write(Encoding.ASCII.GetBytes(String.Format("{0} 0 obj\n(", ObjectNumber)));

		// contents in bytes
		Byte[] ByteContents = new Byte[ContentsString.Length - 2];

		// convert content from string to byte array removing the enclosing parentesis
		// do not use Encoding.ASCII.GetBytes(...)
		for(Int32 Index = 0; Index < ByteContents.Length; Index++) ByteContents[Index] = (Byte) ContentsString[Index + 1];

		// encrypt the string
		ByteContents = Document.Encryption.EncryptByteArray(ObjectNumber, ByteContents);

		// search for \ ( and )
		List<Byte> BC = new List<Byte>(ByteContents);
		for(Int32 Index = 0; Index < BC.Count; Index++)
			{
			if(BC[Index] == (Byte) '\\' || BC[Index] == (Byte) '(' || BC[Index] == (Byte) ')') BC.Insert(Index++, (Byte) '\\');
			}

		// write memory stream internal buffer to PDF file
		PdfFile.Write(BC.ToArray());

		// output object trailer
		PdfFile.Write(Encoding.ASCII.GetBytes(")\nendobj\n"));

		// exit
		return;
		}
	}

////////////////////////////////////////////////////////////////////
// String indirect object
////////////////////////////////////////////////////////////////////

public class StrObject : IComparable<StrObject>
	{
	public String	Str;
	public Int32	ObjNo;

	public StrObject
			(
			String	Str
			)
		{
		this.Str = Str;
		return;
		}

	public Int32 CompareTo
			(
			StrObject	Other
			)
		{
		return(String.Compare(this.Str, Other.Str));
		}
	}
}
