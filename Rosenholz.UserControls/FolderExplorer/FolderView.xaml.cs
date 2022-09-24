using Rosenholz.UserControls.FolderExplorer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Path = System.IO.Path;

namespace Rosenholz.UserControls.FolderExplorer
{
    /// <summary>
    /// Interaktionslogik für FolderViewHost.xaml
    /// </summary>
    public partial class FolderView : UserControl
    {
        #region Properties
        public static readonly DependencyProperty CurrentFolderProperty = DependencyProperty.Register("CurrentFolder", typeof(string), typeof(FolderView),
              new PropertyMetadata(""));
        public string CurrentFolder
        {
            get { return (string)GetValue(CurrentFolderProperty); }
            set { SetValue(CurrentFolderProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(FolderView),
            new PropertyMetadata("*.*"));

        public static readonly DependencyProperty FilterIndexProperty = DependencyProperty.Register("FilterIndex", typeof(int), typeof(FolderView),
            new PropertyMetadata(0));

        public int FilterIndex
        {
            get { return (int)GetValue(FilterIndexProperty); }
            set { SetValue(FilterIndexProperty, value); }
        }

        public string Filter
        {
            get { return (string)GetValue(FilterProperty); }
            set { SetValue(FilterProperty, value); }
        }

        public static readonly DependencyProperty ShowIconsProperty = DependencyProperty.Register("ShowIcons", typeof(bool), typeof(FolderView),
                new PropertyMetadata(true));

        public bool ShowIcons
        {
            get { return (bool)GetValue(ShowIconsProperty); }
            set { SetValue(ShowIconsProperty, value); }
        }

        public static readonly DependencyProperty ShowFoldersProperty = DependencyProperty.Register("ShowFolders", typeof(bool), typeof(FolderView),
        new PropertyMetadata(true));

        public bool ShowFolders
        {
            get { return (bool)GetValue(ShowFoldersProperty); }
            set { SetValue(ShowFoldersProperty, value); }
        }

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(bool), typeof(FolderView),
                new PropertyMetadata(false));

        public bool ShowHidden
        {
            get { return (bool)GetValue(ShowHiddenProperty); }
            set { SetValue(ShowHiddenProperty, value); }
        }


        public static readonly DependencyProperty RecentLocationsProperty = DependencyProperty.Register("RecentLocations",
            typeof(IEnumerable), typeof(FolderView),
                new PropertyMetadata(null, RecentLocationsChanged));
        static void RecentLocationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //(d as FolderView).PopulateView();
        }
        public IEnumerable RecentLocations
        {
            get { return (IEnumerable)GetValue(RecentLocationsProperty); }
            set { SetValue(RecentLocationsProperty, value); }
        }

        #endregion

        #region Events

        public event EventHandler<FListView.FileOpenEventArgs> OnFileOpen;

        #endregion

        public FolderView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentFolder = Settings.Settings.Instance.BasePath;
            //Directory.GetCurrentDirectory();
        }

        private void lstFiles_OnFileOpen(object sender, FListView.FileOpenEventArgs e)
        {
            if (OnFileOpen != null)
                OnFileOpen(this, e);
        }

        private void lstFiles_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                MessageBox.Show("Dieses File kann hier nicht abgelegt werden.");
            }
        }

        private void lstFiles_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                var importList = new List<string>();
                foreach (string s in files)
                {
                    if (Directory.Exists(s))
                        foreach (var f in Directory.GetFiles(s, "*.*", SearchOption.AllDirectories))
                            importList.Add(f);
                    else if (File.Exists(s))
                        importList.Add(s);
                }

                foreach (var item in importList)
                {
                    string folderName = "";
                    try
                    {
                        string pattern = @"[A][U]_[0-9]{3,4}_\d\d";
                        Match match = Regex.Match(CurrentFolder, pattern);
                        folderName = match.Value;
                        if (!match.Success)
                        {
                            return;
                        }
                    }
                    catch (RegexMatchTimeoutException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    //Funktioniert nicht in Unteroderndn
                    //string folderName = new DirectoryInfo(CurrentFolder).Name;
                    try
                    {
                        File.Copy(item, Path.Combine(CurrentFolder, $"{folderName}_{Path.GetFileName(item)}"));
                    }
                    catch (Exception ex)
                    {
                        var rslt = MessageBox.Show(ex.Message, "Error while File.Copy - Override?", MessageBoxButton.YesNo);
                        if (rslt == MessageBoxResult.Yes)
                            File.Copy(item, Path.Combine(CurrentFolder, $"{folderName}_{Path.GetFileName(item)}"), true);
                        else
                            MessageBox.Show("Did not copy.");
                    }
                }
            }
        }
    }

    [ValueConversion(typeof(int), typeof(bool))]
    public class IntToBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return ((int)value > Int32.Parse(parameter.ToString()));
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class FilterStringToListConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            string s = value.ToString();
            return s.Split('|');
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class AutoGrayImage : System.Windows.Controls.Image
    {
        public AutoGrayImage()
        {
            IsEnabledChanged += new
          DependencyPropertyChangedEventHandler(AutoGrayImage_IsEnabledChanged);
        }

        void AutoGrayImage_IsEnabledChanged(object sender,
                                DependencyPropertyChangedEventArgs e)
        {
            Source = IsEnabled ? Source2 : GrayedImage;
        }
        FormatConvertedBitmap GrayedImage = null;

        public static readonly DependencyProperty Source2Property =
                     DependencyProperty.Register("Source2", typeof(BitmapSource),
                            typeof(AutoGrayImage), new PropertyMetadata(null,
                            OnSource2Changed));
        /// <summary>
        /// Sets the image to be grayed, or not.
        /// </summary>
        public BitmapSource Source2
        {
            get { return (BitmapSource)GetValue(Source2Property); }
            set { SetValue(Source2Property, value); }
        }
        static void OnSource2Changed(DependencyObject sender,
                                      DependencyPropertyChangedEventArgs e)
        {
            AutoGrayImage s = sender as AutoGrayImage;
            if (s.Source2 == null)
            {
                s.GrayedImage = null;
            }
            else
            {
                s.GrayedImage = new FormatConvertedBitmap(s.Source2,
                                    PixelFormats.Gray8, null, 0);
                s.OpacityMask = new ImageBrush(s.Source2);
            }
            s.AutoGrayImage_IsEnabledChanged(s, new
                                 DependencyPropertyChangedEventArgs());
        }
    }
}
