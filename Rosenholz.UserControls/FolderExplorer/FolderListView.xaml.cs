using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace Rosenholz.UserControls.FolderExplorer
{
    /// <summary>
    /// Interaktionslogik für FolderListView.xaml
    /// </summary>
    public partial class FListView : ListView
    {
        #region Properties
        public static readonly DependencyProperty CurrentFolderProperty = DependencyProperty.Register("CurrentFolder", typeof(string), typeof(FListView),
              new PropertyMetadata("", UpdateViewS));
        static void UpdateViewS(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d as FListView).TheVM != null)
                (d as FListView).TheVM.PopulateView();
        }

        public string CurrentFolder
        {
            get { return (string)GetValue(CurrentFolderProperty); }
            set { SetValue(CurrentFolderProperty, value); }
        }

        public static readonly DependencyProperty FilterProperty = DependencyProperty.Register("Filter", typeof(string), typeof(FListView),
            new PropertyMetadata("*.*"));

        public static readonly DependencyProperty FilterIndexProperty = DependencyProperty.Register("FilterIndex", typeof(int), typeof(FListView),
            new PropertyMetadata(0, UpdateViewS));

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

        public static readonly DependencyProperty ShowIconsProperty = DependencyProperty.Register("ShowIcons", typeof(bool), typeof(FListView),
                new PropertyMetadata(true));
        public bool ShowIcons
        {
            get { return (bool)GetValue(ShowIconsProperty); }
            set { SetValue(ShowIconsProperty, value); }
        }

        public static readonly DependencyProperty ShowFoldersProperty = DependencyProperty.Register("ShowFolders", typeof(bool), typeof(FListView),
        new PropertyMetadata(true, UpdateViewS));
        public bool ShowFolders
        {
            get { return (bool)GetValue(ShowFoldersProperty); }
            set { SetValue(ShowFoldersProperty, value); }
        }

        public static readonly DependencyProperty ShowHiddenProperty = DependencyProperty.Register("ShowHidden", typeof(bool), typeof(FListView),
                new PropertyMetadata(false, UpdateViewS));
        public bool ShowHidden
        {
            get { return (bool)GetValue(ShowHiddenProperty); }
            set { SetValue(ShowHiddenProperty, value); }
        }

        #endregion

        #region Events

        public class FileOpenEventArgs : EventArgs
        {
            public string FileName;
        }
        public event EventHandler<FileOpenEventArgs> OnFileOpen;

        #endregion

        #region Commands
        public static RoutedCommand BackCommand = new RoutedCommand();
        public static RoutedCommand ForwardCommand = new RoutedCommand();
        public static RoutedCommand UpCommand = new RoutedCommand();
        public static RoutedCommand RefreshCommand = new RoutedCommand();
        public static RoutedCommand NewFolderCommand = new RoutedCommand();
        #endregion

        FileListViewVM TheVM = null;

        public FListView()
        {
            InitializeComponent();
            DataContext = TheVM = new FileListViewVM(this);

            CommandBindings.Add(new CommandBinding(BackCommand, (s, e) => TheVM.Back(), (s, e) => e.CanExecute = (TheVM.RecentFolders.Count > 1)));
            CommandBindings.Add(new CommandBinding(ForwardCommand, (s, e) => TheVM.Forward(), (s, e) => e.CanExecute = (TheVM.FutureFolders.Count > 0)));
            CommandBindings.Add(new CommandBinding(UpCommand, (s, e) => TheVM.Up(), (s, e) => e.CanExecute = (CurrentFolder != null ? CurrentFolder.Split(new char[] { System.IO.Path.DirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries).Length > 1 : false)));
            CommandBindings.Add(new CommandBinding(RefreshCommand, (s, e) => TheVM.PopulateView(), (s, e) => e.CanExecute = true)); ;
            CommandBindings.Add(new CommandBinding(NewFolderCommand, (s, e) => CreateNewFolder(), (s, e) => e.CanExecute = true)); ;
        }

        public void CreateNewFolder()
        {
            Rosenholz.Windows.InputBox box = new Windows.InputBox("Wie soll der neue Ordner heißen?");
            box.ShowDialog();
            var newFolder = box.InputString;

            if (!string.IsNullOrWhiteSpace(newFolder))
            {
                if (!Directory.Exists(Path.Combine(CurrentFolder, newFolder)))
                    Directory.CreateDirectory(Path.Combine(CurrentFolder, newFolder));
            }
        }

        protected void HandleDoubleClickOrReturn(object sender, MouseButtonEventArgs e)
        {
            FSItemVM info;
            if (sender is FSItemVM)
                info = sender as FSItemVM;
            else
                info = ((ListViewItem)sender).Content as FSItemVM;

            if (info != null)
            {
                if (info.type == FSItemType.Folder)
                    CurrentFolder = info.FullPath;
                else if (info.type == FSItemType.File && OnFileOpen != null)
                    OnFileOpen(this, new FileOpenEventArgs() { FileName = info.FullPath });
            }
        }

        private void lstFiles_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Return) && SelectedItem != null)
                HandleDoubleClickOrReturn(SelectedItem, null);
        }

        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

        }
    }
}
