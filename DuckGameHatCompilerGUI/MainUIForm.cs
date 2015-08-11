using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DuckGameHatCompilerGUI
{
    public partial class DGHC_MainForm : Form
    {
        private ProgramCore core;

        private Image noColorDuck;
        private Image colorDuck;
        private Image finalDuck;

        private string basetitle = "Duck Game Hat Tool";
        
        Color[] duckColors = new Color[]
        {
            Color.FromArgb( 255 , 255 , 255 , 255 ),
            Color.FromArgb( 255 , 125, 125, 125 ),
            Color.FromArgb( 255 , 247, 224, 90),
            Color.FromArgb( 205, 107, 29)
        };

        Color lastColor;

		System.IO.FileSystemWatcher watcher;	//but who watches the watchmen?
        System.Media.SoundPlayer quackPlayer;

        public DGHC_MainForm( ProgramCore core )
        {
            InitializeComponent();
            this.core = core;

            //drag and drop support
			this.DragEnter += new DragEventHandler( FileDragEnter );
			this.DragDrop += new DragEventHandler( FileDragDrop );
            
            //quack box
            this.hatsSmallPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hatsPictureBox_Click);
            this.hatsSmallPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.hatsPictureBox_ReleaseClick);

            //doesn't work for now, gotta figure out why
            //watcher = new System.IO.FileSystemWatcher();
			//watcher.Changed += new FileSystemEventHandler( OnPNGFileChanged );
            
            noColorDuck = null;
            colorDuck = null;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ChangeDuckColor(duckColors[0]);
            whiteDuckButton.Checked = true;
            this.quackMode.Checked = false;
            UpdateForm(false);


            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream quackSoundStream = myAssembly.GetManifestResourceStream("DuckGameHatCompilerGUI.EmbeddedResources.quack.wav");

            quackPlayer = new System.Media.SoundPlayer(quackSoundStream);
        }

		void StartWatchingFile( string abspath )
		{
            /*
			watcher.Path = System.IO.Path.GetDirectoryName( abspath );
			watcher.Filter = System.IO.Path.GetFileName( abspath );
            watcher.EnableRaisingEvents = true;
			*/
		}

		void StopWatchingFile()
		{
            /*
			watcher.Path = null;
			watcher.Filter = null;
            watcher.EnableRaisingEvents = false;
			*/
		}

		void OnPNGFileChanged(object source, FileSystemEventArgs e)
		{
			if ( core == null || e.ChangeType != WatcherChangeTypes.Changed )
				return;

			core.OpenFile( e.FullPath, true );

		}

		void FileDragEnter( object sender, DragEventArgs e )
		{
			if ( e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
		}

		void FileDragDrop( object sender, DragEventArgs e )
		{
			string[] files = (string[])e.Data.GetData( DataFormats.FileDrop );
			foreach( string file in files )
			{
				if ( core.OpenFile(file) )
				{
					UpdateForm();
					break;
				}
			}
		}
		
        public void UpdateImage()
        {
            //goddamn memory leaks
            if (this.hatImageBox.Image != null && this.hatImageBox.Image != finalDuck )
            {
                this.hatImageBox.Image.Dispose();
            }

            if (this.hatsSmallPictureBox.Image != null && this.hatsSmallPictureBox.Image != finalDuck)
            {
                this.hatsSmallPictureBox.Image.Dispose();
            }

            if (finalDuck == null)
            {
                ChangeDuckColor(lastColor);
            }

            if ( core != null && core.FileInfo != null && core.FileInfo.TextureData != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(core.FileInfo.TextureData))
                {
                    Image oldimg = Image.FromStream( ms );

                    Image img = ResizeImage(oldimg, 4);

                    oldimg.Dispose();

                    Image overlapped = OverlapImages(finalDuck, img, finalDuck.Width, finalDuck.Height);

                    img.Dispose();

                    this.hatImageBox.Image = overlapped;
                    this.hatsSmallPictureBox.Image = overlapped;
                }
            }
            else
            {
                
                this.hatImageBox.Image = finalDuck;
                this.hatsSmallPictureBox.Image = finalDuck;
            }
        }

        public void UpdateForm( bool updateImage = true )
        {
            if ( core != null && core.FileInfo != null )
            {
                this.hatNameBox.Text = core.FileInfo.HatName;
                
                string loaded = "quack.quack";

                if ( core.FileInfo.TexturePath != null )
                {
                    loaded = core.FileInfo.TexturePath;
					StartWatchingFile( core.FileInfo.TexturePath );
                }

                if (core.FileInfo.SavePath != null)
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

            if (updateImage)
                UpdateImage();

            UpdateMenu();
        }

        private void UpdateMenu()
        {
            if (core == null)
                return;

            if ( core.FileInfo != null )
            {
                this.closeToolStripMenuItem.Enabled = true;
                this.savepngAsToolStripMenuItem.Enabled = true;
                this.savehatAsToolStripMenuItem.Enabled = true;
                this.saveToolStripMenuItem.Enabled = true;
            }
            else
            {
                this.closeToolStripMenuItem.Enabled = false;
                this.savepngAsToolStripMenuItem.Enabled = false;
                this.savehatAsToolStripMenuItem.Enabled = false;
                this.saveToolStripMenuItem.Enabled = false;
            }
        }

        private Image ResizeImage( Image img, int mult )
        {
            if (img == null)
                return null;

            Image ret;
            using (Bitmap bigger = new Bitmap(img.Width * mult, img.Height * mult, img.PixelFormat))
            using (Graphics g = Graphics.FromImage(bigger))
            {
                Color tc = Color.FromArgb( 255 , 255, 0, 255);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(img, new Rectangle(Point.Empty, bigger.Size));
                bigger.MakeTransparent(tc);
                ret = (Image)bigger.Clone();
            }
            return ret;
        }

        private Image OverlapImages( Image img1 , Image img2 , int w, int h )
        {
            Image ret;

            using ( Bitmap overlapped = new Bitmap( w , h , img1.PixelFormat ))
            using ( Graphics g = Graphics.FromImage(overlapped) )
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(img1, new Rectangle(Point.Empty, img1.Size));

                g.DrawImage(img2, new Rectangle(Point.Empty, img2.Size));
                ret = (Image)overlapped.Clone();
            }

            return ret;
        }

        void LoadDuckParts()
        {
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            
            //only need to load this once
            if( noColorDuck == null )
            {
                System.IO.Stream noColorStream = myAssembly.GetManifestResourceStream("DuckGameHatCompilerGUI.EmbeddedResources.baseduck_nocolor.png");
                Image img = Image.FromStream(noColorStream);
                noColorDuck = ResizeImage(img, 4);
                img.Dispose();
            }

            if ( colorDuck != null )
            {
                colorDuck.Dispose();
                colorDuck = null;
            }

            System.IO.Stream ColorStream = myAssembly.GetManifestResourceStream("DuckGameHatCompilerGUI.EmbeddedResources.baseduck_color.png");
            Image cimg = Image.FromStream(ColorStream);
            colorDuck = ResizeImage(cimg, 4);
            cimg.Dispose();
        }

        private void ChangeDuckColor( Color newColor )
        {
            lastColor = newColor;

            LoadDuckParts();

            if (noColorDuck == null || colorDuck == null)
                return;

            if ( finalDuck != null )
            {
                finalDuck.Dispose();
                finalDuck = null;
            }

            finalDuck = new Bitmap(64 * 4, 32 * 4);

            float[][] colorMatrixElements = { 
               new float[] { newColor.R / 255f, 0 , 0,  0, 0},        // red scaling factor of 2 
               new float[] { 0,  newColor.G / 255f,  0,  0, 0},        // green scaling factor of 1 
               new float[] { 0,  0,  newColor.B / 255f,  0, 0},        // blue scaling factor of 1 
               new float[] { 0,  0,  0,  1, 1},        // alpha scaling factor of 1 
               new float[] { 0, 0, 0, 0, 0}
            };

            System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(colorMatrixElements);

            System.Drawing.Imaging.ImageAttributes attr = new System.Drawing.Imaging.ImageAttributes();

            attr.SetColorMatrix(
               colorMatrix,
               System.Drawing.Imaging.ColorMatrixFlag.Default,
               System.Drawing.Imaging.ColorAdjustType.Bitmap);

            //draw in two separate passes, otherwise the attributes will affect the original image too
            using (Graphics g = Graphics.FromImage(finalDuck))
            {
                g.DrawImage(noColorDuck, new Rectangle(Point.Empty, noColorDuck.Size));
            }

            using (Graphics g = Graphics.FromImage(finalDuck))
            {
                g.DrawImage(colorDuck, new Rectangle(Point.Empty, colorDuck.Size), 0, 0, colorDuck.Width, colorDuck.Height, GraphicsUnit.Pixel, attr);
            }

            UpdateImage();
        }

        
        
        private void openpngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (core == null)
                return;
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Hat files or PNG (.hat)|*.hat;*.png";
            openFileDialog.FilterIndex = 1;
            openFileDialog.CheckFileExists = true;
            openFileDialog.Multiselect = false;

            DialogResult res = openFileDialog.ShowDialog();

            if (res == DialogResult.OK)
            {
                core.OpenFile(openFileDialog.FileName);
            }

            UpdateForm();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (core == null || core.FileInfo == null )
                return;

            if ( !core.CloseCurrent() )
            {
                DialogResult res = MessageBox.Show("Do you want to save changes to " + core.FileInfo.HatName + "?", "Save alert", MessageBoxButtons.YesNo);
                
                //we can try saving automatically the changes without showing another save dialog
                if ( res == System.Windows.Forms.DialogResult.Yes )
                {
                    saveToolStripMenuItem_Click(sender, e);
                }

                core.CloseCurrent( true );
            }

            UpdateForm();
        }

        private void savehatAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (core == null)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.DefaultExt = ".hat";
            saveFileDialog.Filter = "Hat files (.hat)|*.hat";
            saveFileDialog.FilterIndex = 1;

            DialogResult res = saveFileDialog.ShowDialog();
            if ( res == DialogResult.OK )
            {
                core.SaveHat( saveFileDialog.FileName);
            }
            UpdateForm(false);
        }

        private void savepngAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (core == null)
                return;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "PNG file (.png)|*.png";
            saveFileDialog.FilterIndex = 1;

            DialogResult res = saveFileDialog.ShowDialog();
            if (res == DialogResult.OK)
            {
                core.SavePNG(saveFileDialog.FileName);
            }
            UpdateForm(false);
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if ( core != null && core.FileInfo != null )
            {
                core.FileInfo.HatName = hatNameBox.Text;
                core.FileInfo.Saved = false;
                UpdateForm( false );
            }
        }

        private void yellowDuckButton_CheckedChanged(object sender, EventArgs e)
        {
            ChangeDuckColor(duckColors[2]);
        }


        private void whiteDuckButton_CheckedChanged(object sender, EventArgs e)
        {
            ChangeDuckColor(duckColors[0]);
        }

        private void orangeDuckButton_CheckedChanged(object sender, EventArgs e)
        {
            ChangeDuckColor(duckColors[3]);
        }

        private void grayDuckButton_CheckedChanged(object sender, EventArgs e)
        {
            ChangeDuckColor(duckColors[1]);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (core == null || core.FileInfo == null)
                return;

            //this file has been saved before, save right away
            if ( core.FileInfo.SavePath != null )
            {
                core.SaveHat(core.FileInfo.SavePath);
            }
            else
            {
                savehatAsToolStripMenuItem_Click(sender, e);
            }

            UpdateForm(false);
        }

        private void hatsPictureBox_Click(object sender, EventArgs e)
        {
            if ( quackPlayer != null )
            {
                quackPlayer.Play();
            }

            if (this.hatsSmallPictureBox.Image != null )
            {
                Image old = this.hatsSmallPictureBox.Image;
                this.hatsSmallPictureBox.Image = ((Bitmap)old).Clone(new Rectangle(128, 0, 128, 128), old.PixelFormat);
                old.Dispose();
            }
        }

        private void hatsPictureBox_ReleaseClick( object sender, EventArgs e)
        {
            if (finalDuck != null)
            {
                finalDuck.Dispose();
                finalDuck = null;
            }
            UpdateImage();
        }


        private void quackMode_CheckedChanged(object sender, EventArgs e)
        {
            this.hatsSmallPictureBox.Visible = this.quackMode.Checked;
            this.hatImageBox.Visible = !this.quackMode.Checked;
        }

        
    }
}
