namespace DuckGameHatCompilerGUI
{
    partial class DGHC_MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savehatAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.savepngAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hatImageBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hatNameBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.whiteDuckButton = new System.Windows.Forms.RadioButton();
            this.grayDuckButton = new System.Windows.Forms.RadioButton();
            this.yellowDuckButton = new System.Windows.Forms.RadioButton();
            this.orangeDuckButton = new System.Windows.Forms.RadioButton();
            this.hatsSmallPictureBox = new System.Windows.Forms.PictureBox();
            this.quackMode = new System.Windows.Forms.CheckBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hatImageBox)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hatsSmallPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(416, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.savehatAsToolStripMenuItem,
            this.savepngAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openpngToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // savehatAsToolStripMenuItem
            // 
            this.savehatAsToolStripMenuItem.Name = "savehatAsToolStripMenuItem";
            this.savehatAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.savehatAsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.savehatAsToolStripMenuItem.Text = "Save .hat as...";
            this.savehatAsToolStripMenuItem.Click += new System.EventHandler(this.savehatAsToolStripMenuItem_Click);
            // 
            // savepngAsToolStripMenuItem
            // 
            this.savepngAsToolStripMenuItem.Name = "savepngAsToolStripMenuItem";
            this.savepngAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.savepngAsToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.savepngAsToolStripMenuItem.Text = "Save .png as...";
            this.savepngAsToolStripMenuItem.Click += new System.EventHandler(this.savepngAsToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Visible = false;
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // hatImageBox
            // 
            this.hatImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hatImageBox.Location = new System.Drawing.Point(15, 83);
            this.hatImageBox.Name = "hatImageBox";
            this.hatImageBox.Size = new System.Drawing.Size(256, 128);
            this.hatImageBox.TabIndex = 1;
            this.hatImageBox.TabStop = false;
            this.hatImageBox.Click += new System.EventHandler(this.pictureBox1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "HatName";
            // 
            // hatNameBox
            // 
            this.hatNameBox.Location = new System.Drawing.Point(70, 48);
            this.hatNameBox.Name = "hatNameBox";
            this.hatNameBox.Size = new System.Drawing.Size(147, 20);
            this.hatNameBox.TabIndex = 4;
            this.hatNameBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(294, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Change duck color";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.whiteDuckButton);
            this.flowLayoutPanel1.Controls.Add(this.grayDuckButton);
            this.flowLayoutPanel1.Controls.Add(this.yellowDuckButton);
            this.flowLayoutPanel1.Controls.Add(this.orangeDuckButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(297, 100);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(94, 111);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // whiteDuckButton
            // 
            this.whiteDuckButton.AutoSize = true;
            this.whiteDuckButton.Location = new System.Drawing.Point(3, 3);
            this.whiteDuckButton.Name = "whiteDuckButton";
            this.whiteDuckButton.Size = new System.Drawing.Size(53, 17);
            this.whiteDuckButton.TabIndex = 0;
            this.whiteDuckButton.TabStop = true;
            this.whiteDuckButton.Text = "White";
            this.whiteDuckButton.UseVisualStyleBackColor = true;
            this.whiteDuckButton.CheckedChanged += new System.EventHandler(this.whiteDuckButton_CheckedChanged);
            // 
            // grayDuckButton
            // 
            this.grayDuckButton.AutoSize = true;
            this.grayDuckButton.Location = new System.Drawing.Point(3, 26);
            this.grayDuckButton.Name = "grayDuckButton";
            this.grayDuckButton.Size = new System.Drawing.Size(47, 17);
            this.grayDuckButton.TabIndex = 1;
            this.grayDuckButton.TabStop = true;
            this.grayDuckButton.Text = "Gray";
            this.grayDuckButton.UseVisualStyleBackColor = true;
            this.grayDuckButton.CheckedChanged += new System.EventHandler(this.grayDuckButton_CheckedChanged);
            // 
            // yellowDuckButton
            // 
            this.yellowDuckButton.AutoSize = true;
            this.yellowDuckButton.Location = new System.Drawing.Point(3, 49);
            this.yellowDuckButton.Name = "yellowDuckButton";
            this.yellowDuckButton.Size = new System.Drawing.Size(56, 17);
            this.yellowDuckButton.TabIndex = 2;
            this.yellowDuckButton.TabStop = true;
            this.yellowDuckButton.Text = "Yellow";
            this.yellowDuckButton.UseVisualStyleBackColor = true;
            this.yellowDuckButton.CheckedChanged += new System.EventHandler(this.yellowDuckButton_CheckedChanged);
            // 
            // orangeDuckButton
            // 
            this.orangeDuckButton.AutoSize = true;
            this.orangeDuckButton.Location = new System.Drawing.Point(3, 72);
            this.orangeDuckButton.Name = "orangeDuckButton";
            this.orangeDuckButton.Size = new System.Drawing.Size(60, 17);
            this.orangeDuckButton.TabIndex = 3;
            this.orangeDuckButton.TabStop = true;
            this.orangeDuckButton.Text = "Orange";
            this.orangeDuckButton.UseVisualStyleBackColor = true;
            this.orangeDuckButton.CheckedChanged += new System.EventHandler(this.orangeDuckButton_CheckedChanged);
            // 
            // hatsSmallPictureBox
            // 
            this.hatsSmallPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hatsSmallPictureBox.Location = new System.Drawing.Point(89, 83);
            this.hatsSmallPictureBox.Name = "hatsSmallPictureBox";
            this.hatsSmallPictureBox.Size = new System.Drawing.Size(128, 128);
            this.hatsSmallPictureBox.TabIndex = 7;
            this.hatsSmallPictureBox.TabStop = false;
            this.hatsSmallPictureBox.Visible = false;
            // 
            // quackMode
            // 
            this.quackMode.AutoSize = true;
            this.quackMode.Location = new System.Drawing.Point(300, 51);
            this.quackMode.Name = "quackMode";
            this.quackMode.Size = new System.Drawing.Size(78, 17);
            this.quackMode.TabIndex = 8;
            this.quackMode.Text = "Quack test";
            this.quackMode.UseVisualStyleBackColor = true;
            this.quackMode.CheckedChanged += new System.EventHandler(this.quackMode_CheckedChanged);
            // 
            // DGHC_MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 219);
            this.Controls.Add(this.quackMode);
            this.Controls.Add(this.hatsSmallPictureBox);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hatNameBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hatImageBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DGHC_MainForm";
            this.Text = "Duck Game Hat Tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hatImageBox)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.hatsSmallPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem savehatAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.PictureBox hatImageBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hatNameBox;
        private System.Windows.Forms.ToolStripMenuItem savepngAsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton whiteDuckButton;
        private System.Windows.Forms.RadioButton grayDuckButton;
        private System.Windows.Forms.RadioButton orangeDuckButton;
        private System.Windows.Forms.RadioButton yellowDuckButton;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.PictureBox hatsSmallPictureBox;
        private System.Windows.Forms.CheckBox quackMode;

    }
}

