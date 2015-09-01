using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


//this is the class that handles all the logic from the UI, this way if the user doesn't want to use the GUI, we can still work in console mode

namespace DuckGameHatCompiler
{
	public enum ImageError
	{
		None = 0 ,
		TooBig ,		//exceeds ProgramCore.max h/w
		TooSmall ,			//other way around
		NotLoaded,			//it isn't loaded yo
		NotPng ,			//not a png?
		Corrupted ,			//garbled f%£$)($£
		FileSizeTooBig		//exceeds ProgramCore.maxImageSize
	}

	public class CurrentFileInfo
	{
		//TODO: is the encryption initialization vector really that important to store? we can always use a static one in case

		private byte[] textureData; //still arguing about this one, do we NEED to turn this into a texture2d? as that'd require XNA bindings
		public byte[] TextureData
		{
			get
			{
				return textureData;
			}
			set
			{
				textureData = value;
			}
		}

		private string texturePath; //used for autorefresh, this will be null if the png came from a .hat

		public string TexturePath
		{
			get
			{
				return texturePath;
			}
			set
			{
				texturePath = value;
			}
		}

		private string hatName;
		public string HatName
		{
			get
			{
				return hatName;
			}
			set
			{
				hatName = value;
			}
		}

		private string savePath; //when opening a png, this is always null, as there's no existing .hat file yet

		public string SavePath
		{
			get
			{
				return savePath;
			}
			set
			{
				savePath = value;
			}
		}


		private bool saved; //by default, nothing is saved
		public bool Saved
		{
			get
			{
				return saved;
			}
			set
			{
				saved = value;
			}
		}

		private string iv = "duckgamehatbyjvs"; //kinda like a signature in this case, but really it's just the initialization vector for the encryption
		public string IV
		{
			get
			{
				return iv;
			}
			set
			{
				iv = value;
			}
		}

		public CurrentFileInfo( )
		{
			this.TextureData = null;
			this.TexturePath = null;
			this.HatName = "Default";
			this.SavePath = null;
			this.Saved = false;
		}
	}

	public class ProgramCore
	{
		public static readonly int maxHeight = 32;
		public static readonly int minHeight = 32;

		public static readonly int maxWidth = 64;
		public static readonly int minWidth = 32;

		protected CurrentFileInfo fileInfo;
		private static readonly int maxImageSize = 8 * 1024;	//not gonna enforce it yet
		//straight from duck game's source code

		//TODO: read straight from duckgame's exe with mono.cecil?
		private static readonly byte[] securityKey = new byte[]
		{
			243,
			22,
			152,
			32,
			1,
			244,
			122,
			111,
			97,
			42,
			13,
			2,
			19,
			15,
			45,
			230
		};

		//TODO: read straight from duckgame's exe with mono.cecil?
		private static readonly long duckgameversioncheck = 402965919293045L;

		public CurrentFileInfo FileInfo
		{
			get
			{
				return fileInfo;
			}
			set
			{
				fileInfo = value;
			}
		}

		public ProgramCore( )
		{
			this.FileInfo = null;
		}

		public bool CloseCurrent( bool forced = false )
		{
			if( this.FileInfo != null && !forced )
			{
				if( !this.FileInfo.Saved )
				{
					return false;
				}
			}

			this.FileInfo = null;
			return true;
		}

		public bool OpenFile( string abspath , bool modifycurrent = false )
		{
			if( !System.IO.File.Exists( abspath ) )
				return false;

			if( !CanReadFile( abspath ) )
				return false;

			CloseCurrent( true );

			return ParseFile( abspath );
		}

		public bool SaveHat( string abspath )
		{
			if( FileInfo.TextureData == null )
				return false;

			byte[] array = EncryptHat( FileInfo );

			if( array == null )
				return false;

			FileInfo.SavePath = abspath;
			FileInfo.Saved = true;

			using( FileStream file = new FileStream( abspath , FileMode.Create , FileAccess.Write ) )
			{
				file.Write( array , 0 , array.Length );
			}

			return true;
		}

		public bool SavePNG( string abspath )
		{
			if( FileInfo.TextureData == null )
				return false;

			FileInfo.TexturePath = abspath;
			FileInfo.Saved = true;

			using( FileStream file = new FileStream( abspath , FileMode.Create , FileAccess.Write ) )
			{
				file.Write( FileInfo.TextureData , 0 , FileInfo.TextureData.Length );
			}

			return true;
		}

		//I need to make this less lame in some way, I don't like repeating code and this seems prone to do that
		public bool CanReadFile( string abspath )
		{
			string ext = System.IO.Path.GetExtension( abspath ).ToLower();
			return ext.Contains( ".png" ) || ext.Contains( ".hat" );
		}

		bool ParseFile( string abspath , bool modifycurrent = false )
		{
			byte[] data = null;

			using( FileStream file = new FileStream( abspath , FileMode.Open , FileAccess.Read ) )
			{
				data = new byte[file.Length];
				file.Read( data , 0 , data.Length );
			}

			//couldn't read shit m8

			if( data == null )
				return false;

			using( MemoryStream stream = new MemoryStream( data ) )
			{
				string ext = System.IO.Path.GetExtension( abspath ).ToLower();

				//does the XNA duck game uses even support jpegs? I'm probably gonna have to test this out but I'm gonna go with no for now
				if( ext.Contains( ".png" ) )
				{
					CurrentFileInfo fi = LoadPNG( stream , modifycurrent );
					if( fi != null )
					{
						if( !modifycurrent )
						{
							fi.TexturePath = abspath;
							fi.Saved = true;
							fi.HatName = System.IO.Path.GetFileNameWithoutExtension( abspath ).ToUpper(); //default the hat name to the filename without the extension
						}
						this.FileInfo = fi;
						return true;
					}
					return false;
				}
				else if( ext.Contains( ".hat" ) )
				{
					CurrentFileInfo fi = DecryptHat( stream , modifycurrent );

					if( fi != null )
					{
						if( !modifycurrent )
						{
							fi.SavePath = abspath;
							fi.Saved = true;
						}
						this.FileInfo = fi;
						return true;
					}
					return false;
				}
			}
			return false;
		}

		CurrentFileInfo LoadPNG( MemoryStream stream , bool modifycurrentinfo = false )
		{
			CurrentFileInfo fi = FileInfo;

			if( !modifycurrentinfo || fi == null )
			{
				fi = new CurrentFileInfo();
			}

			fi.TextureData = stream.ToArray();
			return fi;
		}

		//should we embed this into OpenFile? we would need exceptions to communicate with whoever calls that function
		public ImageError IsImageValid( CurrentFileInfo fi = null )
		{
			//no arguments, read current FileInfo

			if( fi == null )
			{
				fi = FileInfo;
			}

			if( FileInfo == null || FileInfo.TextureData == null )
			{
				return ImageError.NotLoaded;
			}

			if( FileInfo.TextureData.Length > ProgramCore.maxImageSize )
			{
				return ImageError.FileSizeTooBig;
			}
			
			using( MemoryStream stream = new MemoryStream( FileInfo.TextureData ) )
			using( System.Drawing.Image img = System.Drawing.Image.FromStream( stream ) )
			{
				if( !System.Drawing.Imaging.ImageFormat.Png.Equals( img.RawFormat ) )
				{
					return ImageError.NotPng;
				}
				
				if( img.Height > ProgramCore.maxHeight || img.Width > ProgramCore.maxWidth )
				{
					return ImageError.TooBig;
				}

				if( img.Height < ProgramCore.minHeight || img.Width < ProgramCore.minWidth )
				{
					return ImageError.TooSmall;
				}
			}

			return ImageError.None;
		}

		//used by the saving system, this will still need to be saved manually though
		byte[] EncryptHat( CurrentFileInfo fi )
		{
			using( MemoryStream ms = new MemoryStream() )
			using( MemoryStream unencryptedStream = new MemoryStream() )
			using( BinaryWriter writer = new BinaryWriter( unencryptedStream ) )
			{
				writer.Write( duckgameversioncheck ); //supposedly a version or something
				writer.Write( fi.HatName );
				writer.Write( ( Int32 )fi.TextureData.Length );
				writer.Write( fi.TextureData , 0 , fi.TextureData.Length );

				//get the security key in bytes
				byte[] ivbytes = Encoding.ASCII.GetBytes( fi.IV );

				System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
				rijndaelManaged.Key = ProgramCore.securityKey;
				rijndaelManaged.IV = ivbytes;

				//DON'T BE CONFUSED, we're reusing this variable before we encrypt what's in it
				byte[] process = unencryptedStream.ToArray();
				byte[] result = null;

				using( MemoryStream tempms = new MemoryStream() )
				using( CryptoStream cs = new CryptoStream( tempms , rijndaelManaged.CreateEncryptor( rijndaelManaged.Key , rijndaelManaged.IV ) , CryptoStreamMode.Write ) )
				{
					cs.Write( process , 0 , process.Length );
					cs.FlushFinalBlock();
					result = tempms.ToArray();
				}

				//couldn't encrypt for some reason??? in that case the cryptostream might have thrown an exception
				if( result != null && result.Length > 1 )
				{
					byte[] IVLENGTH = BitConverter.GetBytes( ivbytes.Length );

					ms.Write( IVLENGTH , 0 , IVLENGTH.Length );
					ms.Write( ivbytes , 0 , ivbytes.Length );
					ms.Write( result , 0 , result.Length );

					return ms.ToArray();
				}
			}

			return null;
		}

		//used by the loading system, ideally it'd return a CurrentFileInfo, null if it fails
		CurrentFileInfo DecryptHat( MemoryStream ms , bool modifycurrent = false )
		{
			//ms contains the whole file in memory

			CurrentFileInfo fi = FileInfo;
			if( !modifycurrent || fi == null )
			{
				fi = new CurrentFileInfo();
			}

			//copied straight from the code, landon might not like this
			byte[] ivlen = new byte[4];
			if( ms.Read( ivlen , 0 , ivlen.Length ) != ivlen.Length )
			{
				return null;
			}

			byte[] iv = new byte[System.BitConverter.ToInt32( ivlen , 0 )];
			if( ms.Read( iv , 0 , iv.Length ) != iv.Length )
			{
				return null;
			}

			System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
			rijndaelManaged.Key = ProgramCore.securityKey;
			rijndaelManaged.IV = iv;

			using( ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor( rijndaelManaged.Key , rijndaelManaged.IV ) )
			using( System.Security.Cryptography.CryptoStream input = new System.Security.Cryptography.CryptoStream( ms , decryptor , System.Security.Cryptography.CryptoStreamMode.Read ) )
			using( System.IO.BinaryReader binaryReader = new System.IO.BinaryReader( input ) )
			{
				long versionnum = binaryReader.ReadInt64();
				if( versionnum == ProgramCore.duckgameversioncheck )
				{
					fi.HatName = binaryReader.ReadString();
					int texLen = binaryReader.ReadInt32();
					fi.TextureData = binaryReader.ReadBytes( texLen );
					fi.Saved = true;
				}
				else
				{
					return null;
				}
			}

			return fi;
		}
	}
}
