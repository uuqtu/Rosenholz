using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaction logic for AUExplorer.xaml
    /// https://medium.com/@mikependon/designing-a-wpf-treeview-file-explorer-565a3f13f6f2
    /// https://github.com/mikependon/RepoDB.Tutorials/tree/master/WPF/TreeViewFileExplorer
    /// </summary>
    public partial class AUExplorer : UserControl
    {

        public AUExplorer()
        {
            InitializeComponent();
        }

        private void FolderView_OnFileOpen(object sender, FolderExplorer.FListView.FileOpenEventArgs e)
        {
            try
            {
                Process.Start(e.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Methode wird aufgerufen, wenn Sich der Pfad ändert.
        /// </summary>
        /// <param name="path"></param>
        public void OnCurrentFolderChanged(AUReference curentReference)
        {
            string path = FolderManager.Instance.GetAUFolder(curentReference?.AUReferenceString);
            this.FolderExplorerView.CurrentFolder = path;
            this.TextEditor.CurrentFolder = path;
            this.TextEditor.LoadFile(System.IO.Path.Combine(path, "_notes", "main.txt"));
            this.ButtonPanel.CurrentFolder = path;
            this.TaskViewer.AUReference = curentReference?.AUReferenceString;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
