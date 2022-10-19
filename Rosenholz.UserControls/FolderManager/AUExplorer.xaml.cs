using Rosenholz.Model;
using Rosenholz.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public delegate void TextEditorRequired(string reference);
    /// <summary>
    /// Interaction logic for AUExplorer.xaml
    /// https://medium.com/@mikependon/designing-a-wpf-treeview-file-explorer-565a3f13f6f2
    /// https://github.com/mikependon/RepoDB.Tutorials/tree/master/WPF/TreeViewFileExplorer
    /// </summary>
    public partial class AUExplorer : UserControl, INotifyPropertyChanged
    {
        private int _numberOfTasks;
        public int NumberOfTasks { get { return _numberOfTasks; } set { _numberOfTasks = value; OnPropertyChanged(nameof(NumberOfTasks)); } }


        public event TextEditorRequired TextEditorRequiredEvent;
        public event DisplayTaskViewModelRequired DisplayTaskViewModelRequiredEvent;
        /// <summary>
        /// Button Pannel braucht einen Task
        /// </summary>
        public event TaskCreationRequired TaskCreationRequiredEvent;
        public AUExplorer()
        {
            InitializeComponent();
            ButtonPanel.TaskCreationRequiredEvent += ButtonPanel_TaskCreationRequiredEvent;
            TaskViewer.DisplayTaskViewModelRequiredEvent += TaskViewer_DisplayTaskViewModelRequiredEvent;
            DataContext = this;
        }

        private void TaskViewer_DisplayTaskViewModelRequiredEvent(TaskModel task)
        {
            DisplayTaskViewModelRequiredEvent?.Invoke(task);
        }

        private void ButtonPanel_TaskCreationRequiredEvent(string currentFolder)
        {
            TaskCreationRequiredEvent?.Invoke(currentFolder);
        }

        private void FolderView_OnFileOpen(object sender, FolderExplorer.FListView.FileOpenEventArgs e)
        {
            try
            {
                if (e.FileName.EndsWith(".txt") && (Keyboard.Modifiers != ModifierKeys.Control))
                {
                    TextEditorRequiredEvent?.Invoke(e.FileName);
                }
                else
                {
                    Process.Start(new ProcessStartInfo(e.FileName) { UseShellExecute = true });

                }
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

            NumberOfTasks = Rosenholz.Model.TaskStorage.Instance.ReadTask(curentReference.AUReferenceString).Count;

        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
