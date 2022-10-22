namespace MySnippingTool
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setBoundsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.takeSnapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsWordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsImagesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(457, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setBoundsToolStripMenuItem,
            this.takeSnapToolStripMenuItem,
            this.saveAsWordToolStripMenuItem,
            this.saveAsImagesToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // setBoundsToolStripMenuItem
            // 
            this.setBoundsToolStripMenuItem.Name = "setBoundsToolStripMenuItem";
            this.setBoundsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.setBoundsToolStripMenuItem.Text = "Set Bounds";
            this.setBoundsToolStripMenuItem.Click += new System.EventHandler(this.setBoundsToolStripMenuItem_Click);
            // 
            // takeSnapToolStripMenuItem
            // 
            this.takeSnapToolStripMenuItem.Name = "takeSnapToolStripMenuItem";
            this.takeSnapToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.takeSnapToolStripMenuItem.Text = "Take Snap";
            this.takeSnapToolStripMenuItem.Click += new System.EventHandler(this.takeSnapToolStripMenuItem_Click);
            // 
            // saveAsWordToolStripMenuItem
            // 
            this.saveAsWordToolStripMenuItem.Name = "saveAsWordToolStripMenuItem";
            this.saveAsWordToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveAsWordToolStripMenuItem.Text = "Save as Word";
            this.saveAsWordToolStripMenuItem.Click += new System.EventHandler(this.saveAsWordToolStripMenuItem_Click);
            // 
            // saveAsImagesToolStripMenuItem
            // 
            this.saveAsImagesToolStripMenuItem.Name = "saveAsImagesToolStripMenuItem";
            this.saveAsImagesToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.saveAsImagesToolStripMenuItem.Text = "Save as Images";
            this.saveAsImagesToolStripMenuItem.Click += new System.EventHandler(this.saveAsImagesToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(457, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(251, 22);
            this.toolStripLabel1.Text = "Global Shortcut Key to Capture: Ctrl + Shft + Z";
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(0, 49);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(457, 247);
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 296);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "My Snipping Tool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setBoundsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsWordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsImagesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem takeSnapToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ListView listView1;


    }
}

