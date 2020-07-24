/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	TextBox
//  Support class for PdfContents class. Format text to fit column.
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
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace PdfFileWriter
{
public class TextLine
	{
	public Double		Ascent;
	public Double		Descent;
	public Boolean		EndOfParagraph;
	public TextSeg[]	SegArray;

	public Double LineHeight
		{
		get
			{
			return(Ascent + Descent);
			}
		}

	public TextLine
			(
			Double			Ascent,
			Double			Descent,
			Boolean			EndOfParagraph,
			TextSeg[]		SegArray
			)
		{
		this.Ascent = Ascent;
		this.Descent = Descent;
		this.EndOfParagraph = EndOfParagraph;
		this.SegArray = SegArray;
		return;
		}
	}

public class TextSeg
	{
	public PdfFont		Font;
	public Double		FontSize;
	public DrawStyle	DrawStyle;
	public Color		FontColor;
	public Double		SegWidth;
	public Int32		SpaceCount;
	public String		Text;
	public String		WebLink;

	public TextSeg() {}

	public TextSeg
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor,
			String		WebLink
			)
		{
		this.Font = Font;
		this.FontSize = FontSize;
		this.DrawStyle = DrawStyle;
		this.FontColor = FontColor;
		Text = String.Empty;
		this.WebLink = String.IsNullOrWhiteSpace(WebLink) ? null : WebLink;
		return;
		}

	public TextSeg
			(
			TextSeg		CopySeg
			)
		{
		this.Font = CopySeg.Font;
		this.FontSize = CopySeg.FontSize;
		this.DrawStyle = CopySeg.DrawStyle;
		this.FontColor = CopySeg.FontColor;
		Text = String.Empty;
		this.WebLink = CopySeg.WebLink;
		return;
		}

	public Boolean IsEqual
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor,
			String		WebLink
			)
		{
		if(this.Font != Font || this.FontSize != FontSize || this.DrawStyle != DrawStyle || this.FontColor != FontColor) return(false);
		if(String.IsNullOrWhiteSpace(WebLink)) return(this.WebLink == null);
		return(this.WebLink != null && this.WebLink.Equals(WebLink));
		}
	}

public class TextBox
	{
	private Double			_BoxWidth;
	private Double			_BoxHeight;
	private Int32			_ParagraphCount;
	private Double			_FirstLineIndent;	
	private Double			LineBreakFactor;	 // should be >= 0.1 and <= 0.9
	private Char			PrevChar;
	private Double			LineWidth;
	private Double			LineBreakWidth;
	private Int32			BreakSegIndex;
	private Int32			BreakPtr;
	private Double			BreakWidth;
	private List<TextSeg>	SegArray;
	private List<TextLine>	LineArray;

	public Double BoxWidth {get{return(_BoxWidth);}}
	public Double BoxHeight {get{return(_BoxHeight);}}
	public Int32  ParagraphCount {get{return(_ParagraphCount);}}
	public Double FirstLineIndent {get{return(_FirstLineIndent);}}

	public TextBox
			(
			Double		TextWidth
			)
		{
		this._BoxWidth = TextWidth;
		_FirstLineIndent = 0.0;
		LineBreakFactor = 0.5;
		SegArray = new List<TextSeg>();
		LineArray = new List<TextLine>();
		Clear();
		return;
		}

	public TextBox
			(
			Double		TextWidth,
			Double		FirstLineIndent
			)
		{
		this._BoxWidth = TextWidth;
		this._FirstLineIndent = FirstLineIndent;
		LineBreakFactor = 0.5;
		SegArray = new List<TextSeg>();
		LineArray = new List<TextLine>();
		Clear();
		return;
		}

	public TextBox
			(
			Double		TextWidth,
			Double		FirstLineIndent,
			Double		LineBreakFactor
			)
		{
		this._BoxWidth = TextWidth;
		this._FirstLineIndent = FirstLineIndent;
		if(LineBreakFactor < 0.1 || LineBreakFactor > 0.9) throw new ApplicationException("LineBreakFactor must be between 0.1 and 0.9");
		this.LineBreakFactor = LineBreakFactor;
		SegArray = new List<TextSeg>();
		LineArray = new List<TextLine>();
		Clear();
		return;
		}

	public void Clear()
		{
		_BoxHeight = 0.0;
		_ParagraphCount = 0;
		PrevChar = ' ';
		LineWidth = 0.0;
		LineBreakWidth = 0.0;
		BreakSegIndex = 0;
		BreakPtr = 0;
		BreakWidth = 0;
		SegArray.Clear();
		LineArray.Clear();
		return;
		}

	public Int32 LineCount
		{
		get
			{
			return(LineArray.Count);
			}
		}

	public TextLine this[Int32 Index]
		{
		get
			{
			return(LineArray[Index]);
			}
		}

	public void Terminate()
		{
		// terminate last line
		if(SegArray.Count != 0) AddLine(true);

		// remove trailing empty paragraphs
		for(Int32 Index = LineArray.Count - 1; Index >= 0; Index--)
			{
			TextLine Line = LineArray[Index];
			if(!Line.EndOfParagraph || Line.SegArray.Length > 1 || Line.SegArray[0].SegWidth != 0) break;
			_BoxHeight -= Line.Ascent + Line.Descent;
			_ParagraphCount--;
			LineArray.RemoveAt(Index);
			}

		// exit
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle.Normal, Color.Empty, Text, null);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			String		Text,
			String		WebLink
			)
		{
		AddText(Font, FontSize, DrawStyle.Underline, Color.Blue, Text, WebLink);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle, Color.Empty, Text, null);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			Color		FontColor,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle.Normal, FontColor, Text, null);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor,
			String		Text
			)
		{
		AddText(Font, FontSize, DrawStyle, FontColor, Text, null);
		return;
		}

	public void AddText
			(
			PdfFont		Font,
			Double		FontSize,
			DrawStyle	DrawStyle,
			Color		FontColor,
			String		Text,
			String		WebLink
			)
		{
		// text is null or empty
		if(String.IsNullOrEmpty(Text)) return;

		// create new text segment
		TextSeg Seg;

		// segment array is empty or new segment is different than last one
		if(SegArray.Count == 0 || !SegArray[SegArray.Count - 1].IsEqual(Font, FontSize, DrawStyle, FontColor, WebLink))
			{
			Seg = new TextSeg(Font, FontSize, DrawStyle, FontColor, WebLink);
			SegArray.Add(Seg);
			}

		// add new text to most recent text segment
		else
			{
			Seg = SegArray[SegArray.Count - 1];
			}

		// save text start pointer
		Int32 TextStart = 0;

		// loop for characters
		for(Int32 TextPtr = 0; TextPtr < Text.Length; TextPtr++)
			{
			// shortcut to current character
			Char CurChar = Text[TextPtr];

			// end of paragraph
			if(CurChar == '\n' || CurChar == '\r')
				{
				// append text to current segemnt
				Seg.Text += Text.Substring(TextStart, TextPtr - TextStart);

				// test for new line after carriage return
				if(CurChar == '\r' && TextPtr + 1 < Text.Length && Text[TextPtr + 1] == '\n') TextPtr++;

				// move pointer to one after the eol
				TextStart = TextPtr + 1;

				// add line
				AddLine(true);

				// update last character
				PrevChar = ' ';

				// end of text
				if(TextPtr + 1 == Text.Length) return;

				// add new empty segment
				Seg = new TextSeg(Font, FontSize, DrawStyle, FontColor, WebLink);
				SegArray.Add(Seg);
				continue;
				}

			// validate character
			Font.ValidateChar(CurChar);

			// character width
			Double CharWidth = Font.CharWidth(FontSize, Seg.DrawStyle, CurChar);

			// space
			if(CurChar == ' ')
				{
				// test for transition from non space to space
				// this is a potential line break point
				if(PrevChar != ' ')
					{
					// save potential line break information
					LineBreakWidth = LineWidth;
					BreakSegIndex = SegArray.Count - 1;
					BreakPtr = Seg.Text.Length + TextPtr - TextStart;
					BreakWidth = Seg.SegWidth;
					}

				// add to line width
				LineWidth += CharWidth;
				Seg.SegWidth += CharWidth;

				// update last character
				PrevChar = CurChar;
				continue;
				}

			// add current segment width and to overall line width
			Seg.SegWidth += CharWidth;
			LineWidth += CharWidth;

			// for next loop set last character
			PrevChar = CurChar;

			Double Width = _BoxWidth;
			if(_FirstLineIndent != 0 && (LineArray.Count == 0 || LineArray[LineArray.Count - 1].EndOfParagraph)) Width -= _FirstLineIndent;

			// current line width is less than or equal box width
			if(LineWidth <= Width) continue;

			// append text to current segemnt
			Seg.Text += Text.Substring(TextStart, TextPtr - TextStart + 1);
			TextStart = TextPtr + 1;

			// there are no breaks in this line or last segment is too long
			if(LineBreakWidth < LineBreakFactor * Width)
				{
				BreakSegIndex = SegArray.Count - 1;
				BreakPtr = Seg.Text.Length - 1;
				BreakWidth = Seg.SegWidth - CharWidth;
				}

			// break line
			BreakLine();

			// add line up to break point
			AddLine(false);
			}

		// save text
		Seg.Text += Text.Substring(TextStart);

		// exit
		return;
		}

	private void BreakLine()
		{
		// break segment at line break seg index into two segments
		TextSeg BreakSeg = SegArray[BreakSegIndex];

		// add extra segment to segment array
		if(BreakPtr != 0)
			{
			TextSeg ExtraSeg = new TextSeg(BreakSeg);
			ExtraSeg.SegWidth = BreakWidth;
			ExtraSeg.Text = BreakSeg.Text.Substring(0, BreakPtr);
			SegArray.Insert(BreakSegIndex, ExtraSeg);
			BreakSegIndex++;
			}

		// remove blanks from the area between the two sides of the segment
		for(; BreakPtr < BreakSeg.Text.Length && BreakSeg.Text[BreakPtr] == ' '; BreakPtr++);

		// save the area after the first line
		if(BreakPtr < BreakSeg.Text.Length)
			{
			BreakSeg.Text = BreakSeg.Text.Substring(BreakPtr);
			BreakSeg.SegWidth = BreakSeg.Font.TextWidth(BreakSeg.FontSize, BreakSeg.Text);
			}
		else
			{
			BreakSeg.Text = String.Empty;
			BreakSeg.SegWidth = 0.0;
			}
		BreakPtr = 0;
		BreakWidth = 0.0;
		return;
		}

	private void AddLine
			(
			Boolean		EndOfParagraph
			)
		{
		// end of paragraph
		if(EndOfParagraph) BreakSegIndex = SegArray.Count;

		// test for possible trailing blanks
		if(SegArray[BreakSegIndex - 1].Text.EndsWith(" "))
			{
			// remove trailing blanks
			while(BreakSegIndex > 0)
				{
				TextSeg TempSeg = SegArray[BreakSegIndex - 1];
				TempSeg.Text = TempSeg.Text.TrimEnd(new Char[] {' '});
				TempSeg.SegWidth = TempSeg.Font.TextWidth(TempSeg.FontSize, TempSeg.Text);
				if(TempSeg.Text.Length != 0 || BreakSegIndex == 1 && EndOfParagraph) break;
				BreakSegIndex--;
				SegArray.RemoveAt(BreakSegIndex);
				}
			}

		// test for abnormal case of a blank line and not end of paragraph
		if(BreakSegIndex > 0)
			{
			// allocate segment array
			TextSeg[] LineSegArray = new TextSeg[BreakSegIndex];

			// copy segments
			SegArray.CopyTo(0, LineSegArray, 0, BreakSegIndex);

			// line ascent and descent
			Double LineAscent = 0;
			Double LineDescent = 0;

			// loop for segments until line break segment index
			foreach(TextSeg Seg in LineSegArray)
				{
				Double Ascent = Seg.Font.AscentPlusLeading(Seg.FontSize);
				if(Ascent > LineAscent) LineAscent = Ascent;
				Double Descent = Seg.Font.DescentPlusLeading(Seg.FontSize);
				if(Descent > LineDescent) LineDescent = Descent;

				Int32 SpaceCount = 0;
				foreach(Char Chr in Seg.Text) if(Chr == ' ') SpaceCount++;
				Seg.SpaceCount = SpaceCount;
				}

			// add line
			LineArray.Add(new TextLine(LineAscent, LineDescent, EndOfParagraph, LineSegArray));

			// update column height
			_BoxHeight += LineAscent + LineDescent;

			// update paragraph count
			if(EndOfParagraph) _ParagraphCount++;

			// remove segments
			SegArray.RemoveRange(0, BreakSegIndex);
			}

		// switch to next line
		LineBreakWidth = 0.0;
		BreakSegIndex = 0;

		// new line width
		LineWidth = 0.0;
		foreach(TextSeg Seg in SegArray) LineWidth += Seg.SegWidth;
		return;
		}
	}
}
