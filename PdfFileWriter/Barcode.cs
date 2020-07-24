/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	Barcode
//	Single diminsion barcode class.
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
using System.Text;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
// Base class for one dimension barcode
/////////////////////////////////////////////////////////////////////

public class Barcode
	{
	public		Int32[]		CodeArray;
	public		String		Text;
	protected	Int32		_BarCount = 0;
	protected	Int32		_TotalWidth = 0;

	/////////////////////////////////////////////////////////////////////
	// Constructor
	// This class cannot be instantiated by itself
	/////////////////////////////////////////////////////////////////////

	protected Barcode() {}

	/////////////////////////////////////////////////////////////////////
	// Total number of black and white bars
	/////////////////////////////////////////////////////////////////////

	public Int32 BarCount
		{
		get
			{
			return(_BarCount);
			}
		}

	/////////////////////////////////////////////////////////////////////
	// Total barcode width in narrow bar units
	/////////////////////////////////////////////////////////////////////

	public Int32 TotalWidth
		{
		get
			{
			return(_TotalWidth);
			}
		}

	/////////////////////////////////////////////////////////////////////
	// Width of one bar at position index in narrow bar units
	// This virtual function must be implemented by derived class
	// Index range is 0 to BarCount - 1
	/////////////////////////////////////////////////////////////////////

	public virtual Int32 BarWidth
			(
			Int32 Index
			)
		{
		throw new ApplicationException("Barcode.BarWidth: Not defined in derived class");
		}
 	}

/////////////////////////////////////////////////////////////////////
// Barcode 128
// This program supports ascii range of 0 to 127
// Range 128 to 255 is not supported.
/////////////////////////////////////////////////////////////////////

public class Barcode128 : Barcode
	{
	// each code128 character is encoded as 3 black bars and 3 white bars
	public const Int32 CODE_CHAR_BARS = 6;

	// each code128 character width is 11 narrow bars
	public const Int32 CODE_CHAR_WIDTH = 11;

	// function characters
	public const Char	FNC1_CHAR = (Char) 256;
	public const Char	FNC2_CHAR = (Char) 257;
	public const Char	FNC3_CHAR = (Char) 258;

	// special codes
	public const Int32	FNC1 = 102;
	public const Int32	FNC2 = 97;
	public const Int32	FNC3 = 96;
	public const Int32	SHIFT = 98;
	public const Int32	CODEA = 101;	// or FN4 for code set A
	public const Int32	CODEB = 100;	// or FN4 for code set B
	public const Int32	CODEC = 99;
	public const Int32	STARTA = 103;
	public const Int32	STARTB = 104;
	public const Int32	STARTC = 105;
	public const Int32	STOP = 106;

	// Barcode 128 consists of 107 codes.
	// Each code is made of 6 bars. Three black and three white.
	// Each bar is expressed as multiple of the narrow bar.
	// total width is always 11 narrow bar units.
	// after the stop code there is always one more black bar
	// with two units width.
	// Each code can have one of three possible meanings
	// depending on mode.
	// array [107, 6]
	public static readonly Byte[,] CodeTable = 
		{
							//        CODEA   CODEB   CODEC 
		{2, 1, 2, 2, 2, 2},	// 0		SP		SP		0
		{2, 2, 2, 1, 2, 2},	// 1		!		!		1
		{2, 2, 2, 2, 2, 1},	// 2		"		"		2
		{1, 2, 1, 2, 2, 3},	// 3		#		#		3
		{1, 2, 1, 3, 2, 2},	// 4		$		$		4
		{1, 3, 1, 2, 2, 2},	// 5		%		%		5
		{1, 2, 2, 2, 1, 3},	// 6		&		&		6
		{1, 2, 2, 3, 1, 2},	// 7		'		'		7
		{1, 3, 2, 2, 1, 2},	// 8		(		(		8
		{2, 2, 1, 2, 1, 3},	// 9		)		)		9
		{2, 2, 1, 3, 1, 2},	// 10		*		*		10
		{2, 3, 1, 2, 1, 2},	// 11		+		+		11
		{1, 1, 2, 2, 3, 2},	// 12		,		,		12
		{1, 2, 2, 1, 3, 2},	// 13		-		-		13
		{1, 2, 2, 2, 3, 1},	// 14		.		.		14
		{1, 1, 3, 2, 2, 2},	// 15		/		/		15
		{1, 2, 3, 1, 2, 2},	// 16		0		0		16
		{1, 2, 3, 2, 2, 1},	// 17		1		1		17
		{2, 2, 3, 2, 1, 1},	// 18		2		2		18
		{2, 2, 1, 1, 3, 2},	// 19		3		3		19
		{2, 2, 1, 2, 3, 1},	// 20		4		4		20
		{2, 1, 3, 2, 1, 2},	// 21		5		5		21
		{2, 2, 3, 1, 1, 2},	// 22		6		6		22
		{3, 1, 2, 1, 3, 1},	// 23		7		7		23
		{3, 1, 1, 2, 2, 2},	// 24		8		8		24
		{3, 2, 1, 1, 2, 2},	// 25		9		9		25
		{3, 2, 1, 2, 2, 1},	// 26		:		:		26
		{3, 1, 2, 2, 1, 2},	// 27		;		;		27
		{3, 2, 2, 1, 1, 2},	// 28		<		<		28
		{3, 2, 2, 2, 1, 1},	// 29		=		=		29
		{2, 1, 2, 1, 2, 3},	// 30		>		>		30
		{2, 1, 2, 3, 2, 1},	// 31		?		?		31
		{2, 3, 2, 1, 2, 1},	// 32		@		@		32
		{1, 1, 1, 3, 2, 3},	// 33		A		A		33
		{1, 3, 1, 1, 2, 3},	// 34		B		B		34
		{1, 3, 1, 3, 2, 1},	// 35		C		C		35
		{1, 1, 2, 3, 1, 3},	// 36		D		D		36
		{1, 3, 2, 1, 1, 3},	// 37		E		E		37
		{1, 3, 2, 3, 1, 1},	// 38		F		F		38
		{2, 1, 1, 3, 1, 3},	// 39		G		G		39
		{2, 3, 1, 1, 1, 3},	// 40		H		H		40
		{2, 3, 1, 3, 1, 1},	// 41		I		I		41
		{1, 1, 2, 1, 3, 3},	// 42		J		J		42
		{1, 1, 2, 3, 3, 1},	// 43		K		K		43
		{1, 3, 2, 1, 3, 1},	// 44		L		L		44
		{1, 1, 3, 1, 2, 3},	// 45		M		M		45
		{1, 1, 3, 3, 2, 1},	// 46		N		N		46
		{1, 3, 3, 1, 2, 1},	// 47		O		O		47
		{3, 1, 3, 1, 2, 1},	// 48		P		P		48
		{2, 1, 1, 3, 3, 1},	// 49		Q		Q		49
		{2, 3, 1, 1, 3, 1},	// 50		R		R		50
		{2, 1, 3, 1, 1, 3},	// 51		S		S		51
		{2, 1, 3, 3, 1, 1},	// 52		T		T		52
		{2, 1, 3, 1, 3, 1},	// 53		U		U		53
		{3, 1, 1, 1, 2, 3},	// 54		V		V		54
		{3, 1, 1, 3, 2, 1},	// 55		W		W		55
		{3, 3, 1, 1, 2, 1},	// 56		X		X		56
		{3, 1, 2, 1, 1, 3},	// 57		Y		Y		57
		{3, 1, 2, 3, 1, 1},	// 58		Z		Z		58
		{3, 3, 2, 1, 1, 1},	// 59		[		[		59
		{3, 1, 4, 1, 1, 1},	// 60		\		\		60
		{2, 2, 1, 4, 1, 1},	// 61		]		]		61
		{4, 3, 1, 1, 1, 1},	// 62		^		^		62
		{1, 1, 1, 2, 2, 4},	// 63		_		_		63
		{1, 1, 1, 4, 2, 2},	// 64		NUL		`		64
		{1, 2, 1, 1, 2, 4},	// 65		SOH		a		65
		{1, 2, 1, 4, 2, 1},	// 66		STX		b		66
		{1, 4, 1, 1, 2, 2},	// 67		ETX		c		67
		{1, 4, 1, 2, 2, 1},	// 68		EOT		d		68
		{1, 1, 2, 2, 1, 4},	// 69		ENQ		e		69
		{1, 1, 2, 4, 1, 2},	// 70		ACK		f		70
		{1, 2, 2, 1, 1, 4},	// 71		BEL		g		71
		{1, 2, 2, 4, 1, 1},	// 72		BS		h		72
		{1, 4, 2, 1, 1, 2},	// 73		HT		i		73
		{1, 4, 2, 2, 1, 1},	// 74		LF		j		74
		{2, 4, 1, 2, 1, 1},	// 75		VT		k		75
		{2, 2, 1, 1, 1, 4},	// 76		FF		I		76
		{4, 1, 3, 1, 1, 1},	// 77		CR		m		77
		{2, 4, 1, 1, 1, 2},	// 78		SO		n		78
		{1, 3, 4, 1, 1, 1},	// 79		SI		o		79
		{1, 1, 1, 2, 4, 2},	// 80		DLE		p		80
		{1, 2, 1, 1, 4, 2},	// 81		DC1		q		81
		{1, 2, 1, 2, 4, 1},	// 82		DC2		r		82
		{1, 1, 4, 2, 1, 2},	// 83		DC3		s		83
		{1, 2, 4, 1, 1, 2},	// 84		DC4		t		84
		{1, 2, 4, 2, 1, 1},	// 85		NAK		u		85
		{4, 1, 1, 2, 1, 2},	// 86		SYN		v		86
		{4, 2, 1, 1, 1, 2},	// 87		ETB		w		87
		{4, 2, 1, 2, 1, 1},	// 88		CAN		x		88
		{2, 1, 2, 1, 4, 1},	// 89		EM		y		89
		{2, 1, 4, 1, 2, 1},	// 90		SUB		z		90
		{4, 1, 2, 1, 2, 1},	// 91		ESC		{		91
		{1, 1, 1, 1, 4, 3},	// 92		FS		|		92
		{1, 1, 1, 3, 4, 1},	// 93		GS		}		93
		{1, 3, 1, 1, 4, 1},	// 94		RS		~		94
		{1, 1, 4, 1, 1, 3},	// 95		US		DEL		95
		{1, 1, 4, 3, 1, 1},	// 96		FNC 3	FNC 3	96
		{4, 1, 1, 1, 1, 3},	// 97		FNC 2	FNC 2	97
		{4, 1, 1, 3, 1, 1},	// 98		SHIFT	SHIFT	98
		{1, 1, 3, 1, 4, 1},	// 99		CODE C	CODE C	99
		{1, 1, 4, 1, 3, 1},	// 100		CODE B	FNC 4	CODE B
		{3, 1, 1, 1, 4, 1},	// 101		FNC 4	CODE A	CODE A
		{4, 1, 1, 1, 3, 1},	// 102		FNC 1	FNC 1	FNC 1
		{2, 1, 1, 4, 1, 2},	// 103		Start A	Start A	Start A
		{2, 1, 1, 2, 1, 4},	// 104		Start B	Start B	Start B
		{2, 1, 1, 2, 3, 2},	// 105		Start C	Start C	Start C
 		{2, 3, 3, 1, 1, 1},	// 106		Stop	Stop	Stop
		};

	// code set
	private enum CodeSet
		{
		Undefined,
		CodeA,
		CodeB,
		CodeC,
		ShiftA,
		ShiftB,
		};

	////////////////////////////////////////////////////////////////////
	// Bar width as function of position in the barcode
	////////////////////////////////////////////////////////////////////

	public override Int32 BarWidth
			(
			Int32	Index
			)
		{
		return(Index + 1 < _BarCount ? CodeTable[CodeArray[Index / CODE_CHAR_BARS], Index % CODE_CHAR_BARS]: 2);
		}

	////////////////////////////////////////////////////////////////////
	// Convert text to code 128
	// Valid input characters are ASCII 0 to 127.
	// In addition three control function codes are available
	//		FNC1_CHAR = (Char) 256;
	//		FNC2_CHAR = (Char) 257;
	//		FNC3_CHAR = (Char) 258;
	// The constructor will optimize the translation of text to code.
	// The code array will be divided into segments of
	// CodeA, CODEB and CODEC
	////////////////////////////////////////////////////////////////////

	public Barcode128
			(
			String			Text
			)
		{
		// test argument
		if(String.IsNullOrEmpty(Text)) throw new ApplicationException("Barcode128: Text is null or empty");

		// save text
		this.Text = Text;

		// text length
		Int32 TextLen = Text.Length;

		// leading FNC1
		Int32 LeadFnc1End;
		for(LeadFnc1End = 0; LeadFnc1End < TextLen && Text[LeadFnc1End] == FNC1_CHAR; LeadFnc1End++);

		// leading digits
		Int32 LeadDigitsEnd;
		for(LeadDigitsEnd = LeadFnc1End; LeadDigitsEnd < TextLen && Text[LeadDigitsEnd] >= '0' && Text[LeadDigitsEnd] <= '9'; LeadDigitsEnd++);

		// lead digits count
		Int32 LeadDigitsCount = LeadDigitsEnd - LeadFnc1End;

		// if leading digits is odd remove the last one
		if((LeadDigitsCount & 1) != 0)
			{
			LeadDigitsEnd--;
			LeadDigitsCount--;
			}

		// trailing FNC1
		Int32 TrailFnc1Start;
		for(TrailFnc1Start = TextLen - 1; TrailFnc1Start >= LeadDigitsEnd && Text[TrailFnc1Start] == FNC1_CHAR; TrailFnc1Start--);
		TrailFnc1Start++;

		// trailing digits
		Int32 TrailDigitsStart;
		for(TrailDigitsStart = TrailFnc1Start - 1; TrailDigitsStart >= LeadDigitsEnd && Text[TrailDigitsStart] >= '0' && Text[TrailDigitsStart] <= '9'; TrailDigitsStart--);
		TrailDigitsStart++;

		// trailing digits count
		Int32 TrailDigitsCount = TrailFnc1Start - TrailDigitsStart;

		// if trailing digits is odd remove the first one
		if((TrailDigitsCount & 1) != 0)
			{
			TrailDigitsStart++;
			TrailDigitsCount--;
			}

		// initialize code array end pointer
		Int32 CodeEnd = 0;

		// test for all digits with or without leading and or trailing FNC1
		if(LeadDigitsEnd == TrailDigitsStart && LeadDigitsCount != 0)
			{
			// create code array
			CodeArray = new Int32[1 + LeadFnc1End + (LeadDigitsEnd - LeadFnc1End) / 2 + (TextLen - TrailFnc1Start) + 2];

			// start with code set C
			CodeArray[CodeEnd] = CodeEnd == 0 ? STARTC : CODEC;
			CodeEnd++;

			// add FNC1 if required
			for(Int32 Index = 0; Index < LeadFnc1End; Index++) CodeArray[CodeEnd++] = FNC1;

			// convert to pairs of digits
			EncodeDigits(LeadFnc1End, LeadDigitsEnd, ref CodeEnd);

			// add FNC1 if required
			for(Int32 Index = TrailFnc1Start; Index < TextLen; Index++) CodeArray[CodeEnd++] = FNC1;
			}

		// text has digits and non digits
		else
			{
			// remove leading digits if less than 4
			if(LeadDigitsCount < 4)
				{
				LeadDigitsEnd = 0;
				LeadFnc1End = 0;
				LeadDigitsCount = 0;
				}

			// remove traling digits if less than 4
			if(TrailDigitsCount < 4)
				{
				TrailDigitsStart = TextLen;
				TrailFnc1Start = TextLen;
				TrailDigitsCount = 0;
				}

			// create code array (worst case length)
			CodeArray = new Int32[2 * TextLen + 2];

			// lead digits
			if(LeadDigitsCount != 0)
				{
				// start with code set C
				CodeArray[CodeEnd] = CodeEnd == 0 ? STARTC : CODEC;
				CodeEnd++;

				// add FNC1 if required
				for(Int32 Index = 0; Index < LeadFnc1End; Index++) CodeArray[CodeEnd++] = FNC1;

				// convert to pairs of digits
				EncodeDigits(LeadFnc1End, LeadDigitsEnd, ref CodeEnd);
				}

			Int32 StartOfNonDigits = LeadDigitsEnd;
			Int32 StartOfDigits = LeadDigitsEnd;
			Int32 EndOfDigits;

			// scan text between end of leading digits to start of trailing digits
			for(;;)
				{
				// look for a digit
				for(; StartOfDigits < TrailDigitsStart && (Text[StartOfDigits] < '0' || Text[StartOfDigits] > '9'); StartOfDigits++);
				EndOfDigits = StartOfDigits;

				// we have at least one
				if(StartOfDigits < TrailDigitsStart)
					{
					// count how many digits we have
					for(EndOfDigits++; EndOfDigits < TrailDigitsStart && Text[EndOfDigits] >= '0' && Text[EndOfDigits] <= '9'; EndOfDigits++);

					// test for odd number of digits
					if(((EndOfDigits - StartOfDigits) & 1) != 0) StartOfDigits++;

					// if we have less than 6 process digits as non digits
					if(EndOfDigits - StartOfDigits < 6)
						{
						StartOfDigits = EndOfDigits;
						continue;
						}
					}

				// process non digits up to StartOfDigits
				EncodeNonDigits(StartOfNonDigits, StartOfDigits, ref CodeEnd);

				// if there are no digits at the end, get out of the loop
				if(StartOfDigits == TrailDigitsStart) break;

				// add code set C
				CodeArray[CodeEnd] = CodeEnd == 0 ? STARTC : CODEC;
				CodeEnd++;

				// convert to pairs of digits
				EncodeDigits(StartOfDigits, EndOfDigits, ref CodeEnd);

				// adjust start of digits and non digits
				StartOfDigits = EndOfDigits;
				StartOfNonDigits = EndOfDigits;
				}

			// trailing digits
			if(TrailDigitsCount != 0)
				{
				// add code set C
				CodeArray[CodeEnd++] = CODEC;

				// convert to pairs of digits
				EncodeDigits(TrailDigitsStart, TrailFnc1Start, ref CodeEnd);

				// add trailing FNC1 if required
				for(Int32 Index = TrailFnc1Start; Index < TextLen; Index++) CodeArray[CodeEnd++] = FNC1;
				}

			// adjust code array to right length
			Array.Resize<Int32>(ref CodeArray, CodeEnd + 2);
			}

		// checksum and STOP
		Checksum();

		// set number of bars for enumeration
		_BarCount = CODE_CHAR_BARS * CodeArray.Length + 1;

		// save total width
		_TotalWidth = CODE_CHAR_WIDTH * CodeArray.Length + 2;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Code Array and convert to text
	// Each code must be 0 to 106.
	// The first code must be 103, 104 or 105.
	// The stop code 106 if present must be the last code.
	// If the last code is not 106, the method calculates the checksum
	// and append the checksum and stop character at the end of the array.
	// Note: if the stop code is missing you must not have a checksum.
	// If the last code is 106, the method recalculates the checksum
	// and replaces the existing checksum.
	// The text output is made of ASCII characters 0 to 127 and
	// three function characters 256, 257 and 258
	////////////////////////////////////////////////////////////////////

	public Barcode128
			(
			Int32[]		_CodeArray
			)
		{
		// save code array
		CodeArray = _CodeArray;

		// test argument
		if(CodeArray == null || CodeArray.Length < 2) throw new ApplicationException("Barcode128: Code array is null or empty");

		// code array length
		Int32 Length = CodeArray.Length;

		// if last element is not stop, add two more codes
		if(CodeArray[Length - 1] != STOP)
			{
			// add two elements to the array
			Length += 2;
			Array.Resize<Int32>(ref CodeArray, Length);
			}

		// checksum (we ignore user supplied checksum and override it with our own)
		// and add STOP at the end
		Checksum();

		// set number of bars
		_BarCount = CODE_CHAR_BARS * Length + 1;

		// save total width
		_TotalWidth = CODE_CHAR_WIDTH * Length + 2;

		// convert code array to text
		StringBuilder Str = new StringBuilder();

		// conversion state
		CodeSet CodeSet;

		// start code
		switch(CodeArray[0])
			{
			case STARTA:
				CodeSet = CodeSet.CodeA;
				break;

			case STARTB:
				CodeSet = CodeSet.CodeB;
				break;

			case STARTC:
				CodeSet = CodeSet.CodeC;
				break;

			default:
				// first code must be FNC1, FNC2 or FNC3
				throw new ApplicationException("Barcode128: Code array first element must be start code (103, 104, 105)");
			}

		// loop for all characters except for start, checksum and stop
		Int32 End = Length - 2;
		for(Int32 Index = 1; Index < End; Index++)
			{
			Int32 Code = CodeArray[Index];
			if(Code < 0 || Code > FNC1) throw new ApplicationException("Barcode128: Code array has invalid codes (not 0 to 106)");
			switch(CodeSet)
				{
				case CodeSet.CodeA:
					if(Code == CODEA) throw new ApplicationException("Barcode128: No support for FNC4");
					else if(Code == CODEB) CodeSet = CodeSet.CodeB;
					else if(Code == CODEC) CodeSet = CodeSet.CodeC;
					else if(Code == SHIFT) CodeSet = CodeSet.ShiftB;
					else if(Code == FNC1) Str.Append(FNC1_CHAR);
					else if(Code == FNC2) Str.Append(FNC2_CHAR);
					else if(Code == FNC3) Str.Append(FNC3_CHAR);
					else if(Code < 64) Str.Append((Char) (' ' + Code));									
					else Str.Append((Char) (Code - 64));
					break;

				case CodeSet.CodeB:
					if(Code == CODEA) CodeSet = CodeSet.CodeA;
					else if(Code == CODEB) throw new ApplicationException("Barcode128: No support for FNC4");
					else if(Code == CODEC) CodeSet = CodeSet.CodeC;
					else if(Code == SHIFT) CodeSet = CodeSet.ShiftB;
					else if(Code == FNC1) Str.Append(FNC1_CHAR);
					else if(Code == FNC2) Str.Append(FNC2_CHAR);
					else if(Code == FNC3) Str.Append(FNC3_CHAR);
					else Str.Append((Char) (' ' + Code));									
					break;

				case CodeSet.ShiftA:
					if(Code < 64) Str.Append((Char) (' ' + Code));									
					else if(Code < 96) Str.Append((Char) (Code - 64));
					else throw new ApplicationException("Barcode128: SHIFT error");
					CodeSet = CodeSet.CodeB;
					break;

				case CodeSet.ShiftB:
					if(Code < 96) Str.Append((Char) (' ' + Code));									
					else throw new ApplicationException("Barcode128: SHIFT error");
					CodeSet = CodeSet.CodeA;
					break;

				case CodeSet.CodeC:
					if(Code == CODEA) CodeSet = CodeSet.CodeA;
					else if(Code == CODEB) CodeSet = CodeSet.CodeB;
					else if(Code == FNC1) Str.Append(FNC1_CHAR);
					else
						{
						Str.Append((Char) ('0' + Code / 10));												
						Str.Append((Char) ('0' + Code % 10));												
						}
					break;
				}

			}

		// save text
		Text = Str.ToString();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Process block of digits
	////////////////////////////////////////////////////////////////////

	private void EncodeDigits
			(
			Int32		TextStart,
			Int32		TextEnd,
			ref Int32	CodeEnd
			)
		{
		// convert to pairs of digits
		for(Int32 Index = TextStart; Index < TextEnd; Index += 2) CodeArray[CodeEnd++] = 10 * (Text[Index] - '0') + (Text[Index + 1] - '0');
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Process block of non-digits
	////////////////////////////////////////////////////////////////////

	private void EncodeNonDigits
			(
			Int32		TextStart,
			Int32		TextEnd,
			ref Int32	CodeEnd
			)
		{
		// assume code set B
		Int32 CodeSeg = CodeEnd;
		CodeArray[CodeEnd++] = CodeSeg == 0 ? STARTB : CODEB;
		CodeSet CurCodeSet = CodeSet.Undefined;

		for(Int32 Index = TextStart; Index < TextEnd; Index++)
			{
			// get char
			Int32 CurChar = Text[Index];

			// currect character is part of code set A
			if(CurChar < 32)
				{
				switch(CurCodeSet)
					{
					// current segment is undefined
					// all characters up to this point are 32 to 95 eigther A or B
					case CodeSet.Undefined:
						// change first segemnt to be code set A
						CodeArray[CodeSeg] = CodeSeg == 0 ? STARTA : CODEA;
						CurCodeSet = CodeSet.CodeA;
						break;

					// currect segment is code B
					case CodeSet.CodeB:
						// save current location as start of new segment
						CodeSeg = CodeEnd;

						// one time shift to A
						CodeArray[CodeEnd++] = SHIFT;
						CurCodeSet = CodeSet.ShiftA;
						break;

					// currect segment is Code B with one time shift to A
					case CodeSet.ShiftA:
						// convert the last shift A to code A
						CodeArray[CodeSeg] = CODEA;
						CurCodeSet = CodeSet.CodeA;
						break;

					// currect segment is Code A with one time shift to B
					case CodeSet.ShiftB:
						// disable the Shift B. this is a code A segment with one shift B
						CurCodeSet = CodeSet.CodeA;
						break;
					}

				// save character
				CodeArray[CodeEnd++] = CurChar + 64;
				continue;
				}

			// current character is part of either code set A or code set B
			if(CurChar < 96)
				{
				CodeArray[CodeEnd++] = CurChar - ' ';
				continue;
				}

			// currect character is part of code set B
			if(CurChar < 128)
				{
				switch(CurCodeSet)
					{
					// current segment is undefined
					// all characters up to this point are 32 to 95 eigther A or B
					case CodeSet.Undefined:
						// make first segemnt to be code set B
						CurCodeSet = CodeSet.CodeB;
						break;

					// currect segment is code A
					case CodeSet.CodeA:
						// save current location as start of new segment
						CodeSeg = CodeEnd;

						// one time shift to B
						CodeArray[CodeEnd++] = SHIFT;
						CurCodeSet = CodeSet.ShiftB;
						break;

					// currect segment is Code B with one time shift to A
					case CodeSet.ShiftA:
						// disable the ShiftA. this is a code B segment with one shift A
						CurCodeSet = CodeSet.CodeB;
						break;

					// currect segment is Code A with one time shift to B
					case CodeSet.ShiftB:
						// convert the last shift B to code B
						CodeArray[CodeSeg] = CODEB;
						CurCodeSet = CodeSet.CodeB;
						break;
					}

				// save character
				CodeArray[CodeEnd++] = CurChar - ' ';
				continue;
				}

			// function code
			if(CurChar >= FNC1_CHAR && CurChar <= FNC3_CHAR)
				{
				CodeArray[CodeEnd++] = CurChar == FNC1_CHAR ? FNC1 : (CurChar == FNC2_CHAR ? FNC2 : FNC3);
				continue;
				}

			// invalid character
			throw new ApplicationException("FormaCode128 input characters must be 0 to 127 or function code (256, 257, 258)");
			}
 
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Code 128 checksum calculations
	// The method stores the checksum and STOP character
	////////////////////////////////////////////////////////////////////

	private void Checksum()
		{
		// calculate checksum
		Int32 Length = CodeArray.Length - 2;
		Int32 ChkSum = CodeArray[0];
		for(Int32 Index = 1; Index < Length; Index++) ChkSum += Index * CodeArray[Index];

		// final checksum
		CodeArray[Length] = ChkSum % 103;

		// stop code
		CodeArray[Length + 1] = STOP;
		return;
		}
	}

/////////////////////////////////////////////////////////////////////
// Barcode 39
/////////////////////////////////////////////////////////////////////

public class Barcode39 : Barcode
	{
	// each code39 code is encoded as 5 black bars and 5 white bars
	public const Int32 CODE_CHAR_BARS = 10;

	// total length expressed in narrow bar units
	public const Int32 CODE_CHAR_WIDTH = 16;

	// Barcode39 start and stop character (normally displayed as *)
	public const Int32 START_STOP_CODE = 43;

	// Barcode39 supported characters (excluding the *)
	public const String CharSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

	// array [44, 10]
	public static readonly Byte[,] CodeTable = 
		{
		{1, 1, 1, 3, 3, 1, 3, 1, 1, 1},		// 0  0
		{3, 1, 1, 3, 1, 1, 1, 1, 3, 1},		// 1  1
		{1, 1, 3, 3, 1, 1, 1, 1, 3, 1},		// 2  2
		{3, 1, 3, 3, 1, 1, 1, 1, 1, 1},		// 3  3
		{1, 1, 1, 3, 3, 1, 1, 1, 3, 1},		// 4  4
		{3, 1, 1, 3, 3, 1, 1, 1, 1, 1},		// 5  5
		{1, 1, 3, 3, 3, 1, 1, 1, 1, 1},		// 6  6
		{1, 1, 1, 3, 1, 1, 3, 1, 3, 1},		// 7  7
		{3, 1, 1, 3, 1, 1, 3, 1, 1, 1},		// 8  8
		{1, 1, 3, 3, 1, 1, 3, 1, 1, 1},		// 9  9
		{3, 1, 1, 1, 1, 3, 1, 1, 3, 1},		// 10 A
		{1, 1, 3, 1, 1, 3, 1, 1, 3, 1},		// 11 B
		{3, 1, 3, 1, 1, 3, 1, 1, 1, 1},		// 12 C
		{1, 1, 1, 1, 3, 3, 1, 1, 3, 1},		// 13 D
		{3, 1, 1, 1, 3, 3, 1, 1, 1, 1},		// 14 E
		{1, 1, 3, 1, 3, 3, 1, 1, 1, 1},		// 15 F
		{1, 1, 1, 1, 1, 3, 3, 1, 3, 1},		// 16 G
		{3, 1, 1, 1, 1, 3, 3, 1, 1, 1},		// 17 H
		{1, 1, 3, 1, 1, 3, 3, 1, 1, 1},		// 18 I
		{1, 1, 1, 1, 3, 3, 3, 1, 1, 1},		// 19 J
		{3, 1, 1, 1, 1, 1, 1, 3, 3, 1},		// 20 K
		{1, 1, 3, 1, 1, 1, 1, 3, 3, 1},		// 21 L
		{3, 1, 3, 1, 1, 1, 1, 3, 1, 1},		// 22 M
		{1, 1, 1, 1, 3, 1, 1, 3, 3, 1},		// 23 N
		{3, 1, 1, 1, 3, 1, 1, 3, 1, 1},		// 24 O
		{1, 1, 3, 1, 3, 1, 1, 3, 1, 1},		// 25 P
		{1, 1, 1, 1, 1, 1, 3, 3, 3, 1},		// 26 Q
		{3, 1, 1, 1, 1, 1, 3, 3, 1, 1},		// 27 R
		{1, 1, 3, 1, 1, 1, 3, 3, 1, 1},		// 28 S
		{1, 1, 1, 1, 3, 1, 3, 3, 1, 1},		// 29 T
		{3, 3, 1, 1, 1, 1, 1, 1, 3, 1},		// 30 U
		{1, 3, 3, 1, 1, 1, 1, 1, 3, 1},		// 31 V
		{3, 3, 3, 1, 1, 1, 1, 1, 1, 1},		// 32 W
		{1, 3, 1, 1, 3, 1, 1, 1, 3, 1},		// 33 X
		{3, 3, 1, 1, 3, 1, 1, 1, 1, 1},		// 34 Y
		{1, 3, 3, 1, 3, 1, 1, 1, 1, 1},		// 35 Z
		{1, 3, 1, 1, 1, 1, 3, 1, 3, 1},		// 36 -
		{3, 3, 1, 1, 1, 1, 3, 1, 1, 1},		// 37 .
		{1, 3, 3, 1, 1, 1, 3, 1, 1, 1},		// 38 (space)
		{1, 3, 1, 3, 1, 3, 1, 1, 1, 1},		// 39 $
		{1, 3, 1, 3, 1, 1, 1, 3, 1, 1},		// 40 /
		{1, 3, 1, 1, 1, 3, 1, 3, 1, 1},		// 41 +
		{1, 1, 1, 3, 1, 3, 1, 3, 1, 1},		// 42 %
		{1, 3, 1, 1, 3, 1, 3, 1, 1, 1},		// 43 *
		};

	////////////////////////////////////////////////////////////////////
	// Bar width as function of position in the barcode
	////////////////////////////////////////////////////////////////////

	public override Int32 BarWidth
			(
			Int32	Index
			)
		{
		return(CodeTable[CodeArray[Index / CODE_CHAR_BARS], Index % CODE_CHAR_BARS]);
		}

	////////////////////////////////////////////////////////////////////
	// Convert text to code
	// Valid characters are:
	//		Digits: 0 to 9
	//		Capital Letters: A to Z
	//		Dash: -
	//		Period: .
	//		Space
	//		Dollar: $
	//		Slash: /
	//		Plus: +
	//		Percent: %
	//		Asteririsk: * (This is the start stop character it
	//					   cannot be in the middle of the text)
	////////////////////////////////////////////////////////////////////

	public Barcode39
			(
			String			Text
			)
		{
		// test argument
		if(String.IsNullOrEmpty(Text)) throw new ApplicationException("Barcode39: Text cannot be null or empty");

		// save text
		this.Text = Text;

		// barcode array
		CodeArray = new Int32[Text.Length + 2];

		// put * at the begining
		Int32 CodePtr = 0;
		CodeArray[CodePtr++] = START_STOP_CODE;

		// encode the text
		for(Int32 Index = 0; Index < Text.Length; Index++)
			{
			Int32 Code = CharSet.IndexOf(Text[Index]);
			if(Code == START_STOP_CODE)
				{
				if(Index == 0 || Index == Text.Length - 1) continue;
				throw new ApplicationException("Barcode39: Start/Stop character (asterisk *) is not allowed in the middle of the text");
				}
			if(Code < 0) throw new ApplicationException("Barcode39: Invalid character");
			CodeArray[CodePtr++] = Code;
			}

		// put * at the end
		CodeArray[CodePtr] = START_STOP_CODE;

		// set number of bars for enumeration
		_BarCount = CODE_CHAR_BARS * CodeArray.Length - 1;

		// set total width
		_TotalWidth = CODE_CHAR_WIDTH * CodeArray.Length - 1;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Code Array and convert to text
	// If the code array is missing start and/or stop characters
	// the constructor will add it.
	// Valid codes are:
	//		0 to 9		for digits 0 to 9
	//		10 to 35	for capital Letters A to Z
	//		36			for dash -
	//		37			for period .
	//		38			for space
	//		39			for dollar $
	//		40			for slash /
	//		41			for plus +
	//		42			for percent %
	//		43			for asteririsk: * (This is the start stop
	//					character it cannot be in the middle of the text)
	////////////////////////////////////////////////////////////////////

	public Barcode39
			(
			Int32[]		_CodeArray
			)
		{
		// save code array
		CodeArray = _CodeArray;

		// test argument
		if(CodeArray == null || CodeArray.Length == 0) throw new ApplicationException("Barcode39: Code array is null or empty");

		// test for start code
		if(CodeArray[0] != START_STOP_CODE)
			{
			Int32[] TempArray = new Int32[CodeArray.Length + 1];
			TempArray[0] = START_STOP_CODE;
			Array.Copy(CodeArray, 0, TempArray, 1, CodeArray.Length);
			CodeArray = TempArray;
			}

		// test for stop code
		if(CodeArray[CodeArray.Length - 1] != START_STOP_CODE)
			{
			Array.Resize<Int32>(ref CodeArray, CodeArray.Length + 1);
			CodeArray[CodeArray.Length - 1] = START_STOP_CODE;
			}

		// set number of bars
		_BarCount = CODE_CHAR_BARS * CodeArray.Length - 1;

		// set total width
		_TotalWidth = CODE_CHAR_WIDTH * CodeArray.Length - 1;

		// convert code array to text without start or stop characters
		StringBuilder Str = new StringBuilder();
		for(Int32 Index = 1; Index < CodeArray.Length - 2; Index++)
			{
			Int32 Code = CodeArray[Index];
			if(Code < 0 || Code >= START_STOP_CODE) throw new ApplicationException("Barcode39: Code array contains invalid code (0 to 42)");
			Str.Append(CharSet[Code]);
			}

		// convert str to text
		Text = Str.ToString();

		// exit
		return;
		}
	}

/////////////////////////////////////////////////////////////////////
// Barcode EAN-13 and UPC-A
// Note UPC-A is a subset of EAN-13
// UPC-A is made of 12 digits
// EAN-13 is made of 13 digits
// If the first digit of EAN-13 is zero it is considered to be
// UPC-A. The zero will be eliminated.
// The barcode in both cases is made out of 12 sybols.
/////////////////////////////////////////////////////////////////////

public class BarcodeEAN13 : Barcode
	{
	// each code EAN-13 or UPC-A code is encoded as 2 black bars and 2 white bars
	// there are exactly 12 characters in a barcode
	public const Int32 BARCODE_LEN = 12;
	public const Int32 BARCODE_HALF_LEN = 6;
	public const Int32 LEAD_BARS = 3;
	public const Int32 SEPARATOR_BARS = 5;
	public const Int32 CODE_CHAR_BARS = 4;
	public const Int32 CODE_CHAR_WIDTH = 7;

	// array [20, 4]
	public static readonly Byte[,] CodeTable = 
		{
		{3, 2, 1, 1},		// A-0 Odd parity
		{2, 2, 2, 1},		// A-1
		{2, 1, 2, 2},		// A-2
		{1, 4, 1, 1},		// A-3
		{1, 1, 3, 2},		// A-4
		{1, 2, 3, 1},		// A-5
		{1, 1, 1, 4},		// A-6
		{1, 3, 1, 2},		// A-7
		{1, 2, 1, 3},		// A-8
		{3, 1, 1, 2},		// A-9
		{1, 1, 2, 3},		// B-0 Even Parity
		{1, 2, 2, 2},		// B-1
		{2, 2, 1, 2},		// B-2
		{1, 1, 4, 1},		// B-3
		{2, 3, 1, 1},		// B-4
		{1, 3, 2, 1},		// B-5
		{4, 1, 1, 1},		// B-6
		{2, 1, 3, 1},		// B-7
		{3, 1, 2, 1},		// B-8
		{2, 1, 1, 3},		// B-9
		};

	// first digit of EAN-13 odd/even translation table
	public static readonly Byte[,] ParityTable =
		{
		{ 0,  0,  0,  0,  0},	// 0
		{ 0, 10,  0, 10, 10},	// 1
		{ 0, 10, 10,  0, 10},	// 2
		{ 0, 10, 10, 10,  0},	// 3
		{10,  0,  0, 10, 10},	// 4
		{10, 10,  0,  0, 10},	// 5
		{10, 10, 10,  0,  0},	// 6
		{10,  0, 10,  0, 10},	// 7
		{10,  0, 10, 10,  0},	// 8
		{10, 10,  0, 10,  0},	// 9
		};

	private Int32 FirstDigit;

	////////////////////////////////////////////////////////////////////
	// Barcode EAN-13 single bar width
	////////////////////////////////////////////////////////////////////

	public override Int32 BarWidth
			(
			Int32 BarIndex
			)
		{
		// leading bars
		if(BarIndex < LEAD_BARS) return(1);

		// left side 6 digits
		if(BarIndex < LEAD_BARS + BARCODE_HALF_LEN * CODE_CHAR_BARS)
			{
			Int32 Index = BarIndex - LEAD_BARS;
			return(CodeTable[CodeArray[Index  / CODE_CHAR_BARS], Index % CODE_CHAR_BARS]);
			}

		// separator bars
		if(BarIndex < LEAD_BARS + BARCODE_HALF_LEN * CODE_CHAR_BARS + SEPARATOR_BARS) return(1);

		// right side 6 digits
		if(BarIndex < LEAD_BARS + BARCODE_LEN * CODE_CHAR_BARS + SEPARATOR_BARS)
			{
			Int32 Index = BarIndex - (LEAD_BARS + BARCODE_HALF_LEN * CODE_CHAR_BARS + SEPARATOR_BARS);
			return(CodeTable[CodeArray[BARCODE_HALF_LEN + Index  / CODE_CHAR_BARS], Index % CODE_CHAR_BARS]);
			}

		// trailing bars
		return(1);
		}

	////////////////////////////////////////////////////////////////////
	// Convert text to code EAN-13 or UPC-A
	// All characters must be digits.
	// The code is EAN-13 if string length is 13 characters
	// and first digit is not zero.
	// The code is UPC-A if string length is 12 characters
	// or string length is 13 and first character is zero.
	// The last character is a checksum. The checksum must be
	// given however the costructor calculates the checksum and
	// override the one given. In other words, if you do not
	// know the checksum just set the last digit to 0.
	////////////////////////////////////////////////////////////////////

	public BarcodeEAN13
			(
			String			_Text
			)
		{
		// save text
		Text = _Text;

		// test argument
		if(String.IsNullOrEmpty(Text)) throw new ApplicationException("Barcode EAN-13/UPC-A: Text must not be null");

		// text length
		Int32 Length = Text.Length;
		if(Length != 12 && Length != 13) throw new ApplicationException("Barcode EAN-13/UPC-A: Text must be 12 for UPC-A or 13 for EAN-13");

		// first digit
		FirstDigit = Length == 12 ? 0 : (Int32) Text[0] - '0';
		if(FirstDigit < 0 || FirstDigit > 9) throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid character (must be 0 to 9)");
		
		// barcode array
		CodeArray = new Int32[BARCODE_LEN];

		// encode the text
		Int32 CodePtr = 0;
		for(Int32 Index = Length == 12 ? 0 : 1; Index < Length; Index++)
			{
			Int32 CodeValue = (Int32) Text[Index] - '0';
			if(CodeValue < 0 || CodeValue > 9) throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid character (must be 0 to 9)");
			if(FirstDigit != 0 && Index >= 2 && Index <= 6) CodeValue += ParityTable[FirstDigit, Index - 2];
			CodeArray[CodePtr++] = CodeValue;
			}

		// calculate checksum
		Checksum();

		// add it to text
		Text = Text.Substring(0, Text.Length - 1) + ((Char) ('0' + CodeArray[BARCODE_LEN - 1])).ToString();

		// set number of bars
		_BarCount = BARCODE_LEN * CODE_CHAR_BARS + 2 * LEAD_BARS + SEPARATOR_BARS;

		// set total width
		_TotalWidth = BARCODE_LEN * CODE_CHAR_WIDTH + 2 * LEAD_BARS + SEPARATOR_BARS;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set Code Array and convert to text
	// Code array must be 12 elements long for both EAN-13 or UPC-A.
	// In the case of UPC-A the 12 elements of code array correspond
	// one to one with the 12 digits of the encoded value.
	// In the case of EAN-13 the 12 code elements corresponds to
	// element 2 to 13 of the code text character. The first text
	// character controls how elements 2 to 5 of the code array are
	// encoded. Please read the following article for full description.
	// http://www.barcodeisland.com/ean13.phtml
	// In this class, odd parity encoding is code equals digit.
	// Even parity is code equals digit plus 10.
	// The last code element is a checksum. The checksum must be
	// given however the costructor calculates the checksum and
	// override the one given. In other words, if you do not
	// know the checksum just set the last element to 0.
	////////////////////////////////////////////////////////////////////

	public BarcodeEAN13
			(
			Int32[]		_CodeArray
			)
		{
		// save code array
		CodeArray = _CodeArray;

		// test argument
		if(CodeArray == null || CodeArray.Length != BARCODE_LEN) throw new ApplicationException("Barcode EAN-13/UPC-A: Code array must be exactly 12 characters");

		StringBuilder Str = new StringBuilder();
		Int32[] ParityTest = new Int32[5];

		// convert code array to text
		for(Int32 Index = 0; Index < BARCODE_LEN - 1; Index++)
			{
			Int32 Code = CodeArray[Index];
			if(Code < 0 || Code >= 20 || Code >= 10 && (Index == 0 || Index >= 6)) throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid code");

			if(Index >= 1 && Index < 6 && Code >= 10) ParityTest[Index - 1] = 10;

			if(Index == 5)
				{
				for(FirstDigit = 0; FirstDigit < 10; FirstDigit++)
					{
					Int32 Scan;
					for(Scan = 0; Scan < 5 && ParityTable[FirstDigit, Scan] == ParityTest[Scan]; Scan++);
					if(Scan == 5) break;
					}
				if(FirstDigit == 10) throw new ApplicationException("Barcode EAN-13/UPC-A: Invalid code");
				if(FirstDigit != 0) Str.Insert(0, (Char) ('0' + FirstDigit));
				}

			Str.Append((Char) ('0' + (Code % 10)));
			}

		// calculate checksum
		Checksum();

		// add it to text
		Str.Append((Char) ('0' + CodeArray[BARCODE_LEN - 1]));

		// save text
		Text = Str.ToString();

		// set number of bars
		_BarCount = BARCODE_LEN * CODE_CHAR_BARS + 2 * LEAD_BARS + SEPARATOR_BARS;

		// set total width
		_TotalWidth = BARCODE_LEN * CODE_CHAR_WIDTH + 2 * LEAD_BARS + SEPARATOR_BARS;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Code EAN-13 checksum calculations
	////////////////////////////////////////////////////////////////////

	private void Checksum()
		{
		// calculate checksum
		Int32 ChkSum = FirstDigit;
		Boolean Odd = true;
		for(Int32 Index = 0; Index < BARCODE_LEN - 1; Index++)
			{
			ChkSum += (Odd ? 3 : 1) * CodeArray[Index];
			Odd = !Odd;
			}

		// final checksum
		ChkSum = ChkSum % 10;
		CodeArray[BARCODE_LEN - 1] = ChkSum == 0 ? 0 : 10 - ChkSum;
		return;
		}
	}
}
