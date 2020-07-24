/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfEncryption
//	Support for AES-128 encryption
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
using System.IO;
using System.Security.Cryptography;

namespace PdfFileWriter
{
// PDF Reference Version 1.7 Table 3.20
public enum Permission
	{
	None = 0,
	LowQalityPrint = 4,		// bit 3
	ModifyContents = 8,		// bit 4
	ExtractContents = 0x10,	// bit 5
	Annotation = 0x20,		// bit 6
	Interactive = 0x100,	// bit 9
	Accessibility = 0x200,	// bit 10
	AssembleDoc = 0x400,	// bit 11
	Print = 0x804,			// bit 12 + bit 3
	All = 0xf3c,			// bits 3, 4, 5, 6, 9, 10, 11, 12
	}

////////////////////////////////////////////////////////////////////
// Encryption Class
////////////////////////////////////////////////////////////////////

public class PdfEncryption : PdfObject, IDisposable
	{
	private const Int32	PermissionBase = unchecked((Int32) 0xfffff0c0);

	internal Int32		Permissions;
	internal Byte[]		MasterKey;
	internal MD5		MD5 = MD5.Create();
	internal AesCryptoServiceProvider AES = new AesCryptoServiceProvider();

	private static readonly Byte[] PasswordPad =
		{
        (Byte) 0x28, (Byte) 0xBF, (Byte) 0x4E, (Byte) 0x5E, (Byte) 0x4E, (Byte) 0x75, (Byte) 0x8A, (Byte) 0x41,
		(Byte) 0x64, (Byte) 0x00, (Byte) 0x4E, (Byte) 0x56, (Byte) 0xFF, (Byte) 0xFA, (Byte) 0x01, (Byte) 0x08,
		(Byte) 0x2E, (Byte) 0x2E, (Byte) 0x00, (Byte) 0xB6, (Byte) 0xD0, (Byte) 0x68, (Byte) 0x3E, (Byte) 0x80,
        (Byte) 0x2F, (Byte) 0x0C, (Byte) 0xA9, (Byte) 0xFE, (Byte) 0x64, (Byte) 0x53, (Byte) 0x69, (Byte) 0x7A
		};

    private static readonly Byte[] Salt = {(Byte) 0x73, (Byte) 0x41, (Byte) 0x6c, (Byte) 0x54};

	////////////////////////////////////////////////////////////////////
	// Encryption Constructor
	////////////////////////////////////////////////////////////////////

	internal PdfEncryption
			(
			PdfDocument	Document,
			String		UserPassword,
			String		OwnerPassword,
			Permission	UserPermissions
			) : base(Document, false)
		{
		// Notes:
		// The PDF File Writer library supports AES 128 encryption only.
		// The library does not strip leading or trailing white space. They are part of the password.
		// EncriptMetadata is assumed to be true (this libraray does not use metadata).
		// Embeded Files Only is assumed to be false (this library does not have embeded files).

		AddToDictionary("/Filter", "/Standard");
		AddToDictionary("/R", "4");
		AddToDictionary("/V", "4");
		AddToDictionary("/Length", "128");

		// remove all unused bits and add all bits that must be one
		Permissions = ((Int32) UserPermissions & (Int32) Permission.All) | PermissionBase;
		AddToDictionary("/P", Permissions.ToString());

        // convert user string password to byte array
        Byte[] UserBinaryPassword = ProcessPassword(UserPassword);

        // convert owner string password to byte array
		if(String.IsNullOrEmpty(OwnerPassword)) OwnerPassword = BitConverter.ToUInt64(PdfDocument.RandomByteArray(8), 0).ToString(); 
		Byte[] OwnerBinaryPassword = ProcessPassword(OwnerPassword);

		// calculate owner key for crypto dictionary
		Byte[] OwnerKey = CreateOwnerKey(UserBinaryPassword, OwnerBinaryPassword);

		CreateMasterKey(UserBinaryPassword, OwnerKey);
		Byte[] UserKey = CreateUserKey();
 
		AddToDictionary("/O", Document.ByteArrayToPdfString(OwnerKey));
		AddToDictionary("/U", Document.ByteArrayToPdfString(UserKey));
		AddToDictionary("/StrF", "/StdCF");
		AddToDictionary("/StmF", "/StdCF");
		AddToDictionary("/CF", "<</StdCF<</Length 16/AuthEvent/DocOpen/CFM/AESV2>>>>");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Encrypt byte array
	////////////////////////////////////////////////////////////////////

    internal Byte[] EncryptByteArray
			(
			Int32	ObjectNumber,
			Byte[]	PlainText
			)
		{
		MemoryStream OutputStream = null;
		CryptoStream CryptoStream = null;

		// create encryption key
		Byte[] EncryptionKey = CreateEncryptionKey(ObjectNumber);

		// generate new initialization vector IV 
		AES.GenerateIV();

		// create cipher text buffer including initialization vector
		Int32 CipherTextLen = (PlainText.Length & 0x7ffffff0) + 16;
		Byte[] CipherText = new Byte[CipherTextLen + 16];
		Array.Copy(AES.IV, 0, CipherText, 0, 16);

		// set encryption key and key length
		AES.Key = EncryptionKey;

		// Create the streams used for encryption.
		OutputStream = new MemoryStream();
		CryptoStream = new CryptoStream(OutputStream, AES.CreateEncryptor(), CryptoStreamMode.Write);

		// write plain text byte array
		CryptoStream.Write(PlainText, 0, PlainText.Length);

		// encrypt plain text to cipher text
		CryptoStream.FlushFinalBlock();

		// get the result
		OutputStream.Seek(0, SeekOrigin.Begin);
		OutputStream.Read(CipherText, 16, CipherTextLen);

		// release resources
		CryptoStream.Clear();
		OutputStream.Close();

		// return result
		return(CipherText);
		}

	////////////////////////////////////////////////////////////////////
	// Process Password
	////////////////////////////////////////////////////////////////////

	private Byte[] ProcessPassword
			(
			String StringPassword
			)
		{
		// no user password
		if(String.IsNullOrEmpty(StringPassword)) return((Byte[]) PasswordPad.Clone());

		// convert password to byte array
        Byte[] BinaryPassword = new Byte[32];
		Int32 IndexEnd = Math.Min(StringPassword.Length, 32);
		for(Int32 Index = 0; Index < IndexEnd; Index++)
			{
			Char PWChar = StringPassword[Index];
			if(PWChar > 255)  throw new ApplicationException("Owner or user Password has invalid character (allowed 0-255)");
			BinaryPassword[Index] = (Byte) PWChar;
			}

		// if user password is shorter than 32 bytes, add padding			
		if(IndexEnd < 32) Array.Copy(PasswordPad, 0, BinaryPassword, IndexEnd, 32 - IndexEnd);

		// return password
		return(BinaryPassword);
	    }

	////////////////////////////////////////////////////////////////////
	// Create owner key
	////////////////////////////////////////////////////////////////////

	private Byte[] CreateOwnerKey
			(
			Byte[] UserBinaryPassword,
			Byte[] OwnerBinaryPassword
			)
		{
		// create hash array for owner password
		Byte[] OwnerHash = MD5.ComputeHash(OwnerBinaryPassword);

		// loop 50 times creating hash of a hash
		for(Int32 Index = 0; Index < 50; Index++) OwnerHash = MD5.ComputeHash(OwnerHash);

		Byte[] ownerKey = (Byte[]) UserBinaryPassword.Clone();
		Byte[] TempKey = new Byte[16];
		for(Int32 Index = 0; Index < 20; Index++)
			{
			for(Int32 Tindex = 0; Tindex < 16 ; Tindex++) TempKey[Tindex] = (Byte)(OwnerHash[Tindex] ^ Index);
			EncryptRC4(TempKey, ownerKey);
            }

		// return encryption key
		return(ownerKey);
		}

	////////////////////////////////////////////////////////////////////
	// Create master key
	////////////////////////////////////////////////////////////////////

    private void CreateMasterKey
			(
			Byte[] UserBinaryPassword,
			Byte[] OwnerKey
			)
		{
		// input byte array for MD5 hash function
		Byte[] HashInput = new Byte[UserBinaryPassword.Length + OwnerKey.Length + 4 + Document.DocumentID.Length];
		Int32 Ptr = 0;
		Array.Copy(UserBinaryPassword, 0, HashInput, Ptr, UserBinaryPassword.Length);
		Ptr += UserBinaryPassword.Length;
		Array.Copy(OwnerKey, 0, HashInput, Ptr, OwnerKey.Length);
		Ptr += OwnerKey.Length;
		HashInput[Ptr++] = (Byte) Permissions;
		HashInput[Ptr++] = (Byte) (Permissions >> 8);
		HashInput[Ptr++] = (Byte) (Permissions >> 16);
		HashInput[Ptr++] = (Byte) (Permissions >> 24);
		Array.Copy(Document.DocumentID, 0, HashInput, Ptr, Document.DocumentID.Length);
        MasterKey = MD5.ComputeHash(HashInput);

		// loop 50 times creating hash of a hash
		for(Int32 Index = 0; Index < 50; Index++) MasterKey = MD5.ComputeHash(MasterKey);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Create user key
	////////////////////////////////////////////////////////////////////

    private Byte[] CreateUserKey()
		{
		// input byte array for MD5 hash function
		Byte[] HashInput = new Byte[PasswordPad.Length + Document.DocumentID.Length];
		Array.Copy(PasswordPad, 0, HashInput, 0, PasswordPad.Length);
		Array.Copy(Document.DocumentID, 0, HashInput, PasswordPad.Length, Document.DocumentID.Length);
		Byte[] UserKey = MD5.ComputeHash(HashInput);
		Byte[] TempKey = new Byte[16];

		for(Int32 Index = 0; Index < 20; Index++)
			{
			for(Int32 Tindex = 0; Tindex < 16; Tindex++) TempKey[Tindex] = (Byte) (MasterKey[Tindex] ^ Index);
			EncryptRC4(TempKey, UserKey);
			}
		Array.Resize<Byte>(ref UserKey, 32);
		return(UserKey);
		}

	////////////////////////////////////////////////////////////////////
	// Create encryption key
	////////////////////////////////////////////////////////////////////

    private Byte[] CreateEncryptionKey
			(
			Int32	ObjectNumber
			)
		{
		Byte[] HashInput = new Byte[MasterKey.Length + 5 + Salt.Length];
		Int32 Ptr = 0;
		Array.Copy(MasterKey, 0, HashInput, Ptr, MasterKey.Length);
		Ptr += MasterKey.Length;
		HashInput[Ptr++] = (Byte) ObjectNumber;
		HashInput[Ptr++] = (Byte) (ObjectNumber >> 8);
		HashInput[Ptr++] = (Byte) (ObjectNumber >> 16);
		HashInput[Ptr++] = 0;	// Generation is always zero for this library
		HashInput[Ptr++] = 0;	// Generation is always zero for this library
		Array.Copy(Salt, 0, HashInput, Ptr, Salt.Length);
		Byte[] EncryptionKey = MD5.ComputeHash(HashInput);
		if(EncryptionKey.Length > 16) Array.Resize<Byte>(ref EncryptionKey, 16);
		return(EncryptionKey);
		}

	////////////////////////////////////////////////////////////////////
	// RC4 Encryption
	////////////////////////////////////////////////////////////////////

	private void EncryptRC4
			(
			Byte[]	Key,
			Byte[]	Data
			)
		{
		Byte[] State = new Byte[256];
		for(Int32 Index = 0; Index < 256; Index++) State[Index] = (Byte) Index;

		Int32 Index1 = 0;
		Int32 Index2 = 0;
		for(Int32 Index = 0; Index < 256; Index++)
			{
			Index2 = (Key[Index1] + State[Index] + Index2) & 255;
			Byte tmp = State[Index];
			State[Index] = State[Index2];
			State[Index2] = tmp;
			Index1 = (Index1 + 1) % Key.Length;
			}

		Int32 x = 0;
		Int32 y = 0;
		for(Int32 Index = 0; Index < Data.Length; Index++)
			{
			x = (x + 1) & 255;
			y = (State[x] + y) & 255;
			Byte tmp = State[x];
			State[x] = State[y];
			State[y] = tmp;
			Data[Index] = (Byte) (Data[Index] ^ State[(State[x] + State[y]) & 255]);
			}
		return;
		}

	////////////////////////////////////////////////////////////////////
	// dispose
	////////////////////////////////////////////////////////////////////

	public void Dispose()
		{
		AES.Clear();
		AES.Dispose();
		MD5.Clear();
		return;
		}
	}
}
