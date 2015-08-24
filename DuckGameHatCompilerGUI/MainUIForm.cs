using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuckGameHatCompiler
{
	public partial class DGHC_MainForm : Form
	{
		private ProgramCore core;
		private string basetitle = "Duck Game Hat Tool";
		private static Color[] duckColors = new Color[]
        {
            Color.FromArgb( 255 , 255 , 255 , 255 ),
            Color.FromArgb( 255 , 125, 125, 125 ),
            Color.FromArgb( 255 , 247, 224, 90),
            Color.FromArgb( 205, 107, 29)
        };
		private static Color transparencyColor = Color.FromArgb( 255 , 255 , 0 , 255 );
		private Color currentDuckColor;
		public bool isQuacking;
		private System.IO.FileSystemWatcher watcher;	//but who watches the watchmen?
		private System.Media.SoundPlayer quackPlayer;
		private int imageSizeMultiplier;
		private bool acceptCurrentDrop;
		private bool enableWatcher = true; //TODO: config
		private List<string> processFiles = new List<String>();
		private DuckStateManager duckStateManager = new DuckStateManager();	
		
		public DGHC_MainForm( ProgramCore mycore , string[] programargs )
		{
			InitializeComponent();

			this.core = mycore;

			foreach( string arg in programargs.ToList<string>() )
			{
				if( !arg.StartsWith( "-" ) && System.IO.File.Exists( arg ) && core.CanReadFile( arg ) )
				{
					this.processFiles.Add( arg );
				}
			}

			imageSizeMultiplier = 4;

			//drag and drop support
			this.DragEnter += new DragEventHandler( FileDragEnter );
			this.DragDrop += new DragEventHandler( FileDragDrop );
			this.FormClosing += new FormClosingEventHandler( AboutToCloseMe );
			//quack box
			this.hatsSmallPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler( this.hatsPictureBox_Click );
			this.hatsSmallPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler( this.hatsPictureBox_ReleaseClick );

			//doesn't work for now, gotta figure out why
			if( enableWatcher )
			{
				watcher = new System.IO.FileSystemWatcher();
				watcher.Changed += new FileSystemEventHandler( OnPNGFileChanged );
			}
		}

		//we're about to be closed, ask the user if he wants to save us
		//actually, for the scope of this program, does it really matter? he can drag an image on top of the form again in like half a second
		//it's not really that big of a deal
		private void AboutToCloseMe( object sender , FormClosingEventArgs e )
		{
			/*
			if( core.FileInfo != null && e.CloseReason == CloseReason.UserClosing )
			{
				closeToolStripMenuItem_Click( sender , e );
			}
			*/
		}

		private void Form1_Load( object sender , EventArgs e )
		{
			ChangeDuckColor( duckColors[0] );
			whiteDuckButton.Checked = true;
			this.quackMode.Checked = false;
			UpdateForm( false );

			System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.IO.Stream quackSoundStream = myAssembly.GetManifestResourceStream( "DuckGameHatCompiler.EmbeddedResources.quack.wav" );

			quackPlayer = new System.Media.SoundPlayer( quackSoundStream );

			foreach( string filename in processFiles )
			{
				//open only the first valid one
				if( core.OpenFile( filename ) )
				{
					UpdateForm();
					break;
				}
			}
		}

		void StartWatchingFile( string abspath )
		{
			if( watcher != null )
			{
				watcher.Path = System.IO.Path.GetDirectoryName( abspath );
				watcher.Filter = System.IO.Path.GetFileName( abspath );
				watcher.EnableRaisingEvents = true;
				watcher.NotifyFilter = NotifyFilters.LastWrite;
			}
		}

		void StopWatchingFile( )
		{
			if( watcher != null )
			{
				watcher.Path = null;
				watcher.Filter = null;
				watcher.EnableRaisingEvents = false;
			}
		}

		void OnPNGFileChanged( object source , FileSystemEventArgs e )
		{
			//file was written to but doesn't exist in the folder we were tracking anymore, don't bother
			if( !System.IO.File.Exists( e.FullPath ) )
				return;

			core.OpenFile( e.FullPath , true );
			UpdateForm();
		}

		void FileDragEnter( object sender , DragEventArgs e )
		{
			if( e.Data.GetDataPresent( DataFormats.FileDrop ) )
			{
				string[] files = ( string[] )e.Data.GetData( DataFormats.FileDrop );

				//we don't handle multiple files, or files we can't even read
				if( files.Length > 1 || !core.CanReadFile( files.First() ) )
				{
					e.Effect = DragDropEffects.None;
					acceptCurrentDrop = false;
				}
				else
				{
					e.Effect = DragDropEffects.Copy;
					acceptCurrentDrop = true;
				}
			}
		}

		void FileDragDrop( object sender , DragEventArgs e )
		{
			if( !acceptCurrentDrop )
				return;

			string[] files = ( string[] )e.Data.GetData( DataFormats.FileDrop );
			foreach( string file in files )
			{
				if( core.OpenFile( file ) )
				{
					UpdateForm();
					break;
				}
			}

			acceptCurrentDrop = false;
		}

		Image GetImage( bool withhat = true , bool separate = false , bool quack = false )
		{
			
			//check if this image was already cached with this color and hat, otherwise dispose of them
			
			Image imageresult = duckStateManager.GetImage( withhat , currentDuckColor , separate , quack );

			//skip all the work, we've already got this
			if( imageresult != null )
			{
				return imageresult;
			}

			using( Bitmap workingimage = new Bitmap( 64 , 32 ) )
			using( Bitmap halfimage = new Bitmap( 32 , 32 ) )
			using( Image duck = GetDuck() )
			{
				using( Graphics g = Graphics.FromImage( workingimage ) )
				{
					g.DrawImage( duck , new Rectangle( Point.Empty , workingimage.Size ) );

					//load the hat and then draw it too
					if( withhat )
					{

						using( System.IO.MemoryStream ms = new System.IO.MemoryStream( core.FileInfo.TextureData ) )
						using( Image hattemp = Image.FromStream( ms ) )
						using( Bitmap hat = new Bitmap( hattemp ) )
						{
							hat.MakeTransparent( transparencyColor );
							Rectangle rect = new Rectangle( 0 , 0 , hat.Width , hat.Height );
							
							g.DrawImage( hat , rect );
							
							//this hat is small, draw it again on the second panel this time

							if( hat.Width <= 32 )
							{
								rect.X = 32;
								g.DrawImage( hat , rect );
							}

						}

					}

				}

				//we need to separate this image into two, so we're going to create another image and use that for the result instead
				
				if( separate )
				{
					//halve the width of the total image, then add an offset depending on the quack
					int xoffset = 0;
					if( quack )
					{
						xoffset = 32;
					}

					using( Graphics g = Graphics.FromImage( halfimage ) )
					{
						Rectangle rect = new Rectangle( -xoffset , 0 , workingimage.Width , workingimage.Height );
						g.DrawImage( workingimage , rect );
					}

					
					imageresult = ResizeImage( halfimage , imageSizeMultiplier );
				}
				else
				{
					imageresult = ResizeImage( workingimage , imageSizeMultiplier );
				}

				
			}
			
			//add this result to our duckstatemanager so we can retrieve it later on
			duckStateManager.AddImage( imageresult , currentDuckColor , withhat , DuckStateManager.GetEnumByBools( separate , quack ) );
			return imageresult;
		}

		public void UpdateImage( )
		{
			this.hatImageBox.Image = GetImage( core.FileInfo != null );
			this.hatsSmallPictureBox.Image = GetImage( core.FileInfo != null , true , isQuacking );
		}

		public void UpdateForm( bool updateImage = true )
		{
			if( core.FileInfo != null )
			{
				this.hatNameBox.Text = core.FileInfo.HatName;

				string loaded = "quack.quack";

				if( core.FileInfo.TexturePath != null )
				{
					loaded = core.FileInfo.TexturePath;
					StartWatchingFile( core.FileInfo.TexturePath );
				}

				if( core.FileInfo.SavePath != null )
				{
					loaded = core.FileInfo.SavePath;
				}

				this.Text = basetitle + ": " + System.IO.Path.GetFileName( loaded );
			}
			else
			{
				this.Text = basetitle + ": No hat";
				this.hatNameBox.Text = "No hat";

				StopWatchingFile();
			}

			if( updateImage )
			{
				//when we call to this, we always have new data to load, so it's sensible to cleanup the duck state cache
				//since when we want to simply update the image displayed, we'll call UpdateImage() directly
				//but this is called when all data is changed
				duckStateManager.Cleanup();
				UpdateImage();
			}
			UpdateMenu();
		}

		private void UpdateMenu( )
		{
			this.closeToolStripMenuItem.Enabled = core.FileInfo != null;
			this.savepngAsToolStripMenuItem.Enabled = core.FileInfo != null;
			this.savehatAsToolStripMenuItem.Enabled = core.FileInfo != null;
			this.saveToolStripMenuItem.Enabled = core.FileInfo != null;
		}

		private Image ResizeImage( Image img , int mult )
		{
			if( img == null )
				return null;

			Bitmap bigger = new Bitmap( img.Width * mult , img.Height * mult , img.PixelFormat );
			using( Graphics g = Graphics.FromImage( bigger ) )
			{
				
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage( img , new Rectangle( Point.Empty , bigger.Size ) );
				bigger.MakeTransparent( transparencyColor );
			}

			return bigger;
		}

		private Image OverlapImages( Image img1 , Image img2 , int w , int h )
		{
			Image ret;

			using( Bitmap overlapped = new Bitmap( w , h , img1.PixelFormat ) )
			using( Graphics g = Graphics.FromImage( overlapped ) )
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
				g.DrawImage( img1 , new Rectangle( Point.Empty , img1.Size ) );

				g.DrawImage( img2 , new Rectangle( Point.Empty , img2.Size ) );
				ret = ( Image )overlapped.Clone();
			}

			return ret;
		}

		Image GetDuck( )
		{
			Bitmap result = new Bitmap( 64 , 32 );

			System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();

			float[][] colorMatrixElements = { 
                new float[] { currentDuckColor.R / 255f, 0 , 0,  0, 0},        // red scaling factor of 2 
                new float[] { 0,  currentDuckColor.G / 255f,  0,  0, 0},        // green scaling factor of 1 
                new float[] { 0,  0,  currentDuckColor.B / 255f,  0, 0},        // blue scaling factor of 1 
                new float[] { 0,  0,  0,  1, 1},        // alpha scaling factor of 1 
                new float[] { 0, 0, 0, 0, 0}
            };

			System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix( colorMatrixElements );

			System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();

			attr.SetColorMatrix(
			   colorMatrix ,
			   System.Drawing.Imaging.ColorMatrixFlag.Default ,
			   System.Drawing.Imaging.ColorAdjustType.Bitmap );

			using( System.IO.Stream noColorStream = myAssembly.GetManifestResourceStream( "DuckGameHatCompiler.EmbeddedResources.baseduck_nocolor.png" ) )
			using( System.IO.Stream colorStream = myAssembly.GetManifestResourceStream( "DuckGameHatCompiler.EmbeddedResources.baseduck_color.png" ) )
			using( Image baseDuck = Image.FromStream( noColorStream ) )
			using( Image colorDuck = Image.FromStream( colorStream ) )
			{
				//separate the two, as unfortunately the same draw call will also be affected by the attribute
				using( Graphics g = Graphics.FromImage( result ) )
				{
					g.DrawImage( baseDuck , new Rectangle( Point.Empty , result.Size ) );
				}

				using( Graphics g = Graphics.FromImage( result ) )
				{
					g.DrawImage( colorDuck , new Rectangle( Point.Empty , colorDuck.Size ) , 0 , 0 , result.Width , result.Height , GraphicsUnit.Pixel , attr );
				}
			}

			return result;
		}

		private void ChangeDuckColor( Color newColor )
		{
			currentDuckColor = newColor;
			UpdateImage();
		}



		private void openpngToolStripMenuItem_Click( object sender , EventArgs e )
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Hat files or PNG (.hat)|*.hat;*.png";
			openFileDialog.FilterIndex = 1;
			openFileDialog.CheckFileExists = true;
			openFileDialog.Multiselect = false;

			DialogResult res = openFileDialog.ShowDialog();

			if( res == DialogResult.OK )
			{
				core.OpenFile( openFileDialog.FileName );
			}

			UpdateForm();
		}

		private void closeToolStripMenuItem_Click( object sender , EventArgs e )
		{
			if( core.FileInfo == null )
				return;

			if( !core.CloseCurrent() )
			{
				DialogResult res = MessageBox.Show( "Do you want to save changes to " + core.FileInfo.HatName + "?" , "Save alert" , MessageBoxButtons.YesNo );

				//we can try saving automatically the changes without showing another save dialog
				if( res == System.Windows.Forms.DialogResult.Yes )
				{
					saveToolStripMenuItem_Click( sender , e );
				}

				core.CloseCurrent( true );
			}

			UpdateForm();
		}

		private void savehatAsToolStripMenuItem_Click( object sender , EventArgs e )
		{

			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.AddExtension = true;
			saveFileDialog.OverwritePrompt = true;
			saveFileDialog.DefaultExt = ".hat";
			saveFileDialog.Filter = "Hat files (.hat)|*.hat";
			saveFileDialog.FilterIndex = 1;

			DialogResult res = saveFileDialog.ShowDialog();
			if( res == DialogResult.OK )
			{
				core.SaveHat( saveFileDialog.FileName );
			}
			UpdateForm( false );
		}

		private void savepngAsToolStripMenuItem_Click( object sender , EventArgs e )
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.AddExtension = true;
			saveFileDialog.DefaultExt = ".png";
			saveFileDialog.OverwritePrompt = true;
			saveFileDialog.Filter = "PNG file (.png)|*.png";
			saveFileDialog.FilterIndex = 1;

			DialogResult res = saveFileDialog.ShowDialog();
			if( res == DialogResult.OK )
			{
				core.SavePNG( saveFileDialog.FileName );
			}
			UpdateForm( false );
		}

		private void menuStrip1_ItemClicked( object sender , ToolStripItemClickedEventArgs e )
		{

		}

		private void pictureBox1_Click_1( object sender , EventArgs e )
		{

		}


		private void aboutToolStripMenuItem_Click( object sender , EventArgs e )
		{
			using( AboutThisToolBox aboutbox = new AboutThisToolBox() )
			{
				aboutbox.ShowDialog( this );
			}
		}

		private void textBox1_TextChanged( object sender , EventArgs e )
		{
			if( core.FileInfo != null )
			{
				core.FileInfo.HatName = hatNameBox.Text;
				core.FileInfo.Saved = false;
				UpdateForm( false );
			}
		}

		private void whiteDuckButton_CheckedChanged( object sender , EventArgs e )
		{
			ChangeDuckColor( duckColors[0] );
		}
		private void grayDuckButton_CheckedChanged( object sender , EventArgs e )
		{
			ChangeDuckColor( duckColors[1] );
		}

		private void yellowDuckButton_CheckedChanged( object sender , EventArgs e )
		{
			ChangeDuckColor( duckColors[2] );
		}
		private void orangeDuckButton_CheckedChanged( object sender , EventArgs e )
		{
			ChangeDuckColor( duckColors[3] );
		}

		private void saveToolStripMenuItem_Click( object sender , EventArgs e )
		{
			if( core.FileInfo == null )
				return;

			//this file has been saved before, save right away
			if( core.FileInfo.SavePath != null )
			{
				core.SaveHat( core.FileInfo.SavePath );
			}
			else
			{
				savehatAsToolStripMenuItem_Click( sender , e );
			}

			UpdateForm( false );
		}

		private void hatsPictureBox_Click( object sender , EventArgs e )
		{
			if( quackPlayer != null )
			{
				quackPlayer.Play();
			}
			isQuacking = true;
			UpdateImage();
		}

		private void hatsPictureBox_ReleaseClick( object sender , EventArgs e )
		{
			isQuacking = false;
			UpdateImage();
		}


		private void quackMode_CheckedChanged( object sender , EventArgs e )
		{
			this.hatsSmallPictureBox.Visible = this.quackMode.Checked;
			this.hatImageBox.Visible = !this.quackMode.Checked;
		}


	}
}
