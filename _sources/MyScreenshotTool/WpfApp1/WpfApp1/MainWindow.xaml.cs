using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point startPos;      // mouse-down position
        Point currentPos;    // current mouse position
        bool drawing;

        public MainWindow()
        {
            //this.WindowState = WindowState.Maximized;
            this.WindowStyle = WindowStyle.None;
            this.ResizeMode = ResizeMode.NoResize;
            //this.BackColor = Color.White;
            this.Background = new SolidColorBrush(Colors.White);
            this.Opacity = 0.75;
            this.Cursor = Cursors.Cross;
            this.MouseDown += Canvas_MouseDown;
            this.MouseMove += Canvas_MouseMove;
            this.MouseUp += Canvas_MouseUp;
            //this.Paint += Canvas_Paint;
            this.KeyDown += Canvas_KeyDown;
            //this.DoubleBuffered = true;
            InitializeComponent();


            this.Width = SystemParameters.VirtualScreenWidth;
            this.Height = SystemParameters.VirtualScreenHeight;

            this.Left = SystemParameters.VirtualScreenLeft;
            this.Top = SystemParameters.VirtualScreenTop;

        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                this.Close();
            }
        }

        public Rectangle GetRectangle()
        {
            //return new Rectangle(
            //    Math.Min(startPos.X, currentPos.X),
            //    Math.Min(startPos.Y, currentPos.Y),
            //    Math.Abs(startPos.X - currentPos.X),
            //    Math.Abs(startPos.Y - currentPos.Y));
            return null;
        }

        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            //currentPos = startPos = e.Location;
            drawing = true;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            //currentPos = e.Location;
            //if (drawing) this.Invalidate();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        //private void Canvas_Paint(object sender, PaintEventArgs e)
        //{
        //    if (drawing) e.Graphics.DrawRectangle(Pens.Red, GetRectangle());
        //}

        //private void InitializeComponent()
        //{
        //    //this.SuspendLayout();
        //    // 
        //    // Canvas
        //    // 
        //    //this.ClientSize = new System.Drawing.Size(284, 261);
        //    this.Name = "Canvas";
        //    //this.ShowInTaskbar = false;
        //    //this.ResumeLayout(false);

        //}
    }
}
