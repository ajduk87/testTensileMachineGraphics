/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfPage
//	PDF page class. An indirect PDF object.
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
public class PdfPage : PdfObject
	{
	internal List<PdfContents> ContentsArray;

	////////////////////////////////////////////////////////////////////
	// Default constructor
	// Page size is taken from PdfDocument
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument Document
			) : base(Document, false, "/Page")
		{
		PdfPageConstructor(Document.PageSize.Width, Document.PageSize.Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// PageSize override the default page size
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			SizeD			PageSize
			) : base(Document, false, "/Page")
		{
		PdfPageConstructor(ToPt(PageSize.Width), ToPt(PageSize.Height));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// PaperType and orientation override the default page size
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			PaperType		PaperType,
			Boolean			Landscape
			) : base(Document, false, "/Page")
		{
		// get standard paper size
		Double Width = PdfDocument.PaperTypeSize[(Int32) PaperType].Width;
		Double Height = PdfDocument.PaperTypeSize[(Int32) PaperType].Height;

		// for landscape swap width and height
		if(Landscape)
			{
			Double Temp = Width;
			Width = Height;
			Height = Temp;
			}

		// exit
		PdfPageConstructor(Width, Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor
	// Width and Height override the default page size
	////////////////////////////////////////////////////////////////////

	public PdfPage
			(
			PdfDocument		Document,
			Double			Width,
			Double			Height
			) : base(Document, false, "/Page")
		{
		PdfPageConstructor(ToPt(Width), ToPt(Height));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor common method
	////////////////////////////////////////////////////////////////////

	private void PdfPageConstructor
			(
			Double	Width,
			Double	Height
			)
		{
		// add page to parent array of pages
		if(Document.PageCount == 0)
			{
			Document.PagesObject.AddToDictionary("/Kids", String.Format("[{0} 0 R]", ObjectNumber));
			}
		else
			{
			String Kids = Document.PagesObject.GetDictionaryValue("/Kids");
			Document.PagesObject.AddToDictionary("/Kids", Kids.Replace("]", String.Format(" {0} 0 R]", ObjectNumber)));			
			}

		// update page count
		Document.PagesObject.AddToDictionary("/Count", (++Document.PageCount).ToString());

		// link page to parent
		AddToDictionary("/Parent", Document.PagesObject);

		// add page size in points
		AddToDictionary("/MediaBox", String.Format(NFI.DecSep, "[0 0 {0} {1}]", Round(Width), Round(Height)));

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Add existing contents to page
	////////////////////////////////////////////////////////////////////

	public void AddContents
			(
			PdfContents	Contents
			)
		{
		// set page contents flag
		Contents.PageContents = true;
 
		// add content to content array
		if(ContentsArray == null) ContentsArray = new List<PdfContents>();
		ContentsArray.Add(Contents);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Constructor for web link
	// NOTE:
	//	The four position arguments are in relation to the
	//	bottom left corner of the paper
	////////////////////////////////////////////////////////////////////

	public void AddWebLink
			(
			Double		LeftPos,
			Double		BottomPos,
			Double		RightPos,
			Double		TopPos,
			String		WebLink
			)
		{
		// if web link is empty, ignore this call
		if(String.IsNullOrWhiteSpace(WebLink)) return;

		// search list
		Int32 ObjNo;
		StrObject StrObj = new StrObject(WebLink);
		Int32 Index = Document.StrObjects.BinarySearch(StrObj);

		// this string is a duplicate
		if(Index >= 0)
			{
			// get object number
			ObjNo = Document.StrObjects[Index].ObjNo;
			}

		// new string
		else
			{
			// copy text to temp string builder
			StringBuilder PdfText = new StringBuilder("(");

			// scan string
			foreach(Char TestChar in WebLink)
				{
				// validate character
				if(TestChar < ' ' || TestChar > '~' && TestChar < 161 || TestChar > 255) throw new ApplicationException("Web link invalid character");
				if(TestChar == '\\' || TestChar == '(' || TestChar == ')') PdfText.Append('\\');
				PdfText.Append(TestChar);
				}
			PdfText.Append(')');
		
			// create indirect string object for web link
			PdfObject WebLinkObject = new PdfIndirectString(Document, PdfText);

			// get object number
			ObjNo = WebLinkObject.ObjectNumber;

			// save new string
			StrObj.ObjNo = ObjNo;
			Document.StrObjects.Insert(~Index, StrObj);
			}

		// create PdfObject for annotation
		PdfObject Annotation = new PdfObject(Document, false, "/Annot");
		Annotation.AddToDictionary("/A", String.Format("<</S/URI/Type/Action/URI {0} 0 R>>", ObjNo));
		Annotation.AddToDictionary("/BS", "<</W 0>>");
		Annotation.AddToDictionary("/F", "4");
		Annotation.AddToDictionary("/Rect", String.Format(NFI.DecSep, "[{0} {1} {2} {3}]", ToPt(LeftPos), ToPt(BottomPos), ToPt(RightPos), ToPt(TopPos)));
		Annotation.AddToDictionary("/Subtype", "/Link");

		// add annotation object to page dictionary
		String Value = GetDictionaryValue("/Annots");
		if(Value == null) AddToDictionary("/Annots", String.Format("[{0} 0 R]", Annotation.ObjectNumber));
		else AddToDictionary("/Annots", Value.Replace("]", String.Format(" {0} 0 R]", Annotation.ObjectNumber)));

		// exit
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
		// we have at least one contents object
		if(ContentsArray != null)
			{
			// page has one contents object
			if(ContentsArray.Count == 1)
				{
				AddToDictionary("/Contents", String.Format("[{0} 0 R]", ContentsArray[0].ObjectNumber));
				AddToDictionary("/Resources", BuildResourcesDictionary(ContentsArray[0].ResObjects, true));
				}

			// page is made of multiple contents
			else
				{
				// contents dictionary entry
				StringBuilder ContentsStr = new StringBuilder("[");

				// build contents dictionary entry
				foreach(PdfContents Contents in ContentsArray) ContentsStr.AppendFormat("{0} 0 R ", Contents.ObjectNumber);

				// add terminating bracket
				ContentsStr.Length--;
				ContentsStr.Append(']');
				AddToDictionary("/Contents", ContentsStr.ToString());

				// resources array of all contents objects
				List<PdfObject> ResObjects = new List<PdfObject>();

				// loop for all contents objects
				foreach(PdfContents Contents in ContentsArray)
					{
					// make sure we have resources
					if(Contents.ResObjects != null)
						{
						// loop for resources within this contents object
						foreach(PdfObject ResObject in Contents.ResObjects)
							{
							// check if we have it already
							Int32 Ptr = ResObjects.BinarySearch(ResObject);
							if(Ptr < 0) ResObjects.Insert(~Ptr, ResObject);
							}
						}
					}

				// save to dictionary
				AddToDictionary("/Resources", BuildResourcesDictionary(ResObjects, true));
				}
			}

		// call PdfObject routine
		base.WriteObjectToPdfFile(PdfFile);

		// exit
		return;
		}
	}
}
