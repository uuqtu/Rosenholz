﻿using System;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double initialCanvasX;
        private double initialCanvasY;

        private double HISTORY_X = -1;
        private double HISTORY_Y = -1;
        private double HISTORY_WIDTH = -1;
        private double HISTORY_HEIGHT = -1;

        private bool mouseDownState = false;
        private bool isDraw = false;

        private int screenWidth;
        private int screenHeight;
        Bitmap image;

        private string screenshotPath;
        private string screenshotName;

        // Ink Canvas
        System.Windows.Ink.StrokeCollection HISTORY_INK_REMOVED = new System.Windows.Ink.StrokeCollection();
        private bool HISTORY_INK_HANDLE = false;

        public MainWindow()
        {
            InitializeComponent();
            screenWidth = Screen.PrimaryScreen.Bounds.Width;
            screenHeight = Screen.PrimaryScreen.Bounds.Height;

            //Set MacOS Mojave Crosshair Cursor
            //mainCanvas.Cursor = new System.Windows.Input.Cursor(new MemoryStream(Properties.Resources.Crosshair));

            mainCanvas.Cursor = System.Windows.Input.Cursors.Cross;
            InitScreenshot();

            drawCanvas.Strokes.StrokesChanged += Strokes_StrokesChanged;
        }

        private void Strokes_StrokesChanged(object sender, System.Windows.Ink.StrokeCollectionChangedEventArgs e)
        {

            if (e.Removed.Count > 0 && !HISTORY_INK_HANDLE)
            {
                HISTORY_INK_REMOVED.Add(e.Removed.First().Clone());
            }

        }

        private void MainWindow_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Exit application
            if (e.Key == Key.Escape)
            {
                if (isDraw)
                {
                    isDraw = false;
                    System.Windows.Controls.Panel.SetZIndex(drawCanvas, 1);
                    System.Windows.Controls.Panel.SetZIndex(mainRectangle, 3);
                    drawCanvas.IsEnabled = false;
                } else
                {
                    StopApplication();
                }
            }

            // Undo
            if (e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl) && !Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (drawCanvas.Strokes.Count > 0)
                {
                    HISTORY_INK_HANDLE = true;
                    HISTORY_INK_REMOVED.Add(drawCanvas.Strokes.Last());
                    drawCanvas.Strokes.Remove(drawCanvas.Strokes.Last());
                    HISTORY_INK_HANDLE = false;
                }
            }

            // Redo
            if (e.Key == Key.Y && Keyboard.IsKeyDown(Key.LeftCtrl) || e.Key == Key.Z && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.LeftShift))
            {
                if (HISTORY_INK_REMOVED.Count > 0)
                {
                    HISTORY_INK_HANDLE = true;
                    drawCanvas.Strokes.Add(HISTORY_INK_REMOVED.Last());
                    HISTORY_INK_REMOVED.Remove(HISTORY_INK_REMOVED.Last());
                    HISTORY_INK_HANDLE = false;
                }
            }
        }

        private void InitScreenshot()
        {
            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

                BitmapSource bitmapSource = BitmapConversion.ToWpfBitmap(bitmap);
                this.image = bitmap;
                mainImage.Source = bitmapSource;
            }
        }

        private Bitmap TakeCroppedScreenshot()
        {
            System.Windows.Size sourcePoints = CalculateDPI((int)Canvas.GetLeft(mainRectangle) + 1, (int)Canvas.GetTop(mainRectangle) + 1);
            System.Windows.Size destinationPoints = CalculateDPI((int)mainRectangle.ActualWidth, (int)mainRectangle.ActualHeight);

            using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);

                Bitmap bitmap2 = CropBitmap(bitmap, new Rectangle((int)sourcePoints.Width, (int)sourcePoints.Height, (int)destinationPoints.Width, (int)destinationPoints.Height));
                return bitmap2;
            }
        }

        private Bitmap CropBitmap(Bitmap img, Rectangle cropArea)
        {
            Bitmap bmp = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics gph = Graphics.FromImage(bmp))
            {
                gph.DrawImage(img, new Rectangle(2, 2, bmp.Width, bmp.Height), cropArea, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        private bool SaveScreenshot(Bitmap bitmap)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "(*.png)|*.png",
                FileName = "Screenshot_1"
            };

            DialogResult dialogResult = saveFileDialog.ShowDialog();

            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                screenshotPath = saveFileDialog.FileName;
                screenshotName = System.IO.Path.GetFileName(screenshotPath);
                bitmap.Save(saveFileDialog.FileName);
                saveFileDialog.Dispose();
                return true;
            }
            else if (dialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                saveFileDialog.Dispose();
                return false;
            }
            return false;
        }

        public System.Windows.Size CalculateDPI(int x, int y)
        {
            PresentationSource mainWindowPresentationSource = PresentationSource.FromVisual(this);
            Matrix m = mainWindowPresentationSource.CompositionTarget.TransformToDevice;
            var dpiWidthFactor = m.M11;
            var dpiHeightFactor = m.M22;
            double finalX = x * dpiWidthFactor;
            double finalY = y * dpiHeightFactor;

            return new System.Windows.Size(finalX, finalY);
        }

        private void ShowNotification(string title, string fileName)
        {
            NotifyIcon notifyIcon = new NotifyIcon
            {
                Visible = true,
                Icon = SystemIcons.Information
            };

            notifyIcon.BalloonTipClicked += NotifyIcon_BalloonTipClicked;
            notifyIcon.BalloonTipClosed += (sender, e) => { var thisIcon = (NotifyIcon)sender; thisIcon.Visible = false; thisIcon.Dispose(); };
            notifyIcon.ShowBalloonTip(5000, title, $"Screenshot is saved to {fileName}", ToolTipIcon.None);
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            var thisIcon = (NotifyIcon)sender; thisIcon.Visible = false; thisIcon.Dispose();
            Process.Start(screenshotPath);
        }

        private void MainCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mainMenu.Visibility = Visibility.Hidden;
            mainResolution.Visibility = Visibility.Visible;

            initialCanvasX = e.GetPosition(mainCanvas).X;
            initialCanvasY = e.GetPosition(mainCanvas).Y;
            mouseDownState = true;

            Canvas.SetLeft(mainRectangle, initialCanvasX);
            Canvas.SetTop(mainRectangle, initialCanvasY);

            Canvas.SetLeft(mainRectangleBorder, initialCanvasX);
            Canvas.SetLeft(mainRectangleBorder, initialCanvasY);

            UpdateScreen(e.GetPosition(mainCanvas).X, e.GetPosition(mainCanvas).Y);





        }

        private void MainCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (mouseDownState)
            {
                UpdateScreen(e.GetPosition(mainCanvas).X, e.GetPosition(mainCanvas).Y);
            }
        }

        private void MainCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (mouseDownState)
            {
                // Prevent missclick
                System.Windows.Size actualCoordinates = CalculateDPI((int)mainRectangle.Width, (int)mainRectangle.Height);

                if (actualCoordinates.Width == 0 || actualCoordinates.Height == 0)
                {
                    initialCanvasX = HISTORY_X;
                    initialCanvasY = HISTORY_Y;
                    UpdateScreen(HISTORY_X + HISTORY_WIDTH, HISTORY_Y + HISTORY_HEIGHT);
                }
                else
                {
                    HISTORY_X = Canvas.GetLeft(mainRectangle);
                    HISTORY_Y = Canvas.GetTop(mainRectangle);

                    HISTORY_WIDTH = mainRectangle.Width;
                    HISTORY_HEIGHT = mainRectangle.Height;
                }

                // Init Menu
                mainMenu.Visibility = Visibility.Visible;
                mainMenu.Width = (screenshotButton.Width * mainMenu.Children.Count-1) - 30;
                mainMenu.Height = mainMenu.Height + screenshotButton.Height / 2;

                // Middle
                //Canvas.SetLeft(mainMenuBorder, mainWindow.Width / 2 - mainMenuBorder.Width / 2);
                //Canvas.SetTop(mainMenuBorder, mainWindow.Height * 0.85);

                // Corner
                Canvas.SetLeft(mainMenu, (initialCanvasX + mainRectangle.Width) - mainMenu.Width - 1);
                Canvas.SetTop(mainMenu, (initialCanvasY + mainRectangle.Height) + 10);

                // Ink Canvas
                drawCanvas.Width = screenWidth;
                drawCanvas.Height = screenHeight;

                mouseDownState = false;
            }
        }

        private void UpdateScreen(double x, double y)
        {
            // Screen Handling
            double eventX = x;
            double eventY = y;

            if (eventX < initialCanvasX)
            {
                Canvas.SetLeft(mainRectangle, eventX);
                Canvas.SetLeft(mainRectangleBorder, eventX);
            }
            else
            {
                Canvas.SetLeft(mainRectangle, initialCanvasX);
                Canvas.SetLeft(mainRectangleBorder, initialCanvasX);

            }

            if (eventY < initialCanvasY)
            {
                Canvas.SetTop(mainRectangle, eventY);
                Canvas.SetTop(mainRectangleBorder, eventY);
            }
            else
            {
                Canvas.SetTop(mainRectangle, initialCanvasY);
                Canvas.SetTop(mainRectangleBorder, initialCanvasY);
            }


            mainRectangle.Width = Math.Abs(initialCanvasX - eventX);
            mainRectangle.Height = Math.Abs(initialCanvasY - eventY);

            mainRectangleBorder.Width = Math.Abs(initialCanvasX - eventX);
            mainRectangleBorder.Height = Math.Abs(initialCanvasY - eventY);


            // Dark Area

            // Top
            topRectangle.Width = screenWidth;
            if (eventY < initialCanvasY)
            {
                topRectangle.Height = Math.Abs(mainRectangle.Height - initialCanvasY);
            }
            else
            {
                topRectangle.Height = initialCanvasY;
            }

            // Left
            Canvas.SetTop(leftRectangle, topRectangle.Height);
            leftRectangle.Height = screenHeight - topRectangle.Height;
            if (eventX < initialCanvasX)
            {
                leftRectangle.Width = Math.Abs(mainRectangle.Width - initialCanvasX);
            }
            else
            {
                leftRectangle.Width = initialCanvasX;
            }

            // Bottom
            Canvas.SetLeft(bottomRectangle, leftRectangle.Width);
            bottomRectangle.Width = screenWidth - leftRectangle.Width;
            bottomRectangle.Height = Math.Abs(screenHeight - (initialCanvasY + mainRectangle.Height));
            if (eventY < initialCanvasY)
            {
                Canvas.SetTop(bottomRectangle, initialCanvasY);
                bottomRectangle.Height = screenHeight - initialCanvasY;
            }
            else
            {
                Canvas.SetTop(bottomRectangle, initialCanvasY + mainRectangle.Height);
            }

            // Right
            Canvas.SetTop(rightRectangle, 0);
            Canvas.SetLeft(rightRectangle, (initialCanvasX + mainRectangle.Width));
            rightRectangle.Width = Math.Abs(screenWidth - (initialCanvasX + mainRectangle.Width));
            rightRectangle.Height = Math.Abs(screenHeight - bottomRectangle.Height);

            if (eventX < initialCanvasX)
            {
                Canvas.SetLeft(rightRectangle, initialCanvasX);
                rightRectangle.Width = screenWidth - initialCanvasX;
            }

            if (!(Canvas.GetLeft(rightRectangle) < 0))
            {
                topRectangle.Width = Canvas.GetLeft(rightRectangle);
            }
            


            // Resolution Label Handling
            System.Windows.Size actualCoordinates = CalculateDPI((int)mainRectangle.Width, (int)mainRectangle.Height);
            mainResolution.Content = (int)actualCoordinates.Width + " x " + (int)actualCoordinates.Height;
            Canvas.SetLeft(mainResolution, (initialCanvasX) + 1);
            Canvas.SetTop(mainResolution, (initialCanvasY - mainResolution.ActualHeight - mainResolution.Padding.Top) - 1);
        }

        private void ScreenshotButton_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bitmap = TakeCroppedScreenshot();
            if (SaveScreenshot(bitmap))
            {
                mainCanvas.Visibility = Visibility.Hidden;
                mainWindow.Visibility = Visibility.Hidden;
                ShowNotification("ScreenCapture", screenshotName);
                StopApplication();
            }
        }

        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            if (isDraw)
            {
                isDraw = false;
                System.Windows.Controls.Panel.SetZIndex(drawCanvas, 1);
                System.Windows.Controls.Panel.SetZIndex(mainRectangle, 3);
                drawCanvas.IsEnabled = false;
            }
            else
            {
                isDraw = true;
                System.Windows.Controls.Panel.SetZIndex(drawCanvas, 3);
                System.Windows.Controls.Panel.SetZIndex(mainRectangle, 1);
                drawCanvas.IsEnabled = true;
            }
        }

        private void ScreenshotButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            SetElementsVisibilityOnScreenshot(Visibility.Hidden);
        }

        private void ScreenshotButton_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SetElementsVisibilityOnScreenshot(Visibility.Visible);
        }

        private void SetElementsVisibilityOnScreenshot(Visibility visibility)
        {
            mainResolution.Visibility = visibility;
            mainRectangleBorder.Visibility = visibility;
            mainRectangle.Visibility = visibility;
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            StopApplication();
        }

        private void StopApplication()
        {
            image.Dispose();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            mainWindow.Close();
        }
    }
}
