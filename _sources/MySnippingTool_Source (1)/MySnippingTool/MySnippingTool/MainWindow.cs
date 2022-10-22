using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace MySnippingTool
{
    public partial class MainWindow : Form
    {
        // DLL libraries used to manage hotkeys
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        private const int TAKE_SNAP_HOTKEY_ID = 1;
      
        private ScreenCapture objScreenCapture;
        private int snapCount;
        private List<Bitmap> snaps;

        public MainWindow()
        {
            InitializeComponent();

            objScreenCapture = new ScreenCapture();
            snapCount = 0;
            snaps = new List<Bitmap>();

            // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            // Compute the addition of each combination of the keys you want to be pressed
            // ALT+CTRL = 1 + 2 = 3 , CTRL+SHIFT = 2 + 4 = 6...
            RegisterHotKey(this.Handle, TAKE_SNAP_HOTKEY_ID, 6, (int)Keys.Z);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312 && m.WParam.ToInt32() == TAKE_SNAP_HOTKEY_ID)
            {
                TakeSnap();
            }
            base.WndProc(ref m);
        }

        private void takeSnapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TakeSnap();            
        }

        private void setBoundsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            objScreenCapture.SetCanvas();
        }

        private void TakeSnap()
        {
            var snap = objScreenCapture.GetSnapShot();
            snaps.Add(snap);
            AddToPreview(snap);
        }

        private void AddToPreview(Bitmap snap)
        {
            imageList1.Images.Add(snap);
            listView1.Items.Add(new ListViewItem("Snap_" + (++snapCount), imageList1.Images.Count - 1)).EnsureVisible();            
        }

        private void saveAsImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utility.SaveAsImages(snaps);
        }

        private void saveAsWordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Utility.SaveAsWord(snaps);
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, TAKE_SNAP_HOTKEY_ID);
        }
    }
}