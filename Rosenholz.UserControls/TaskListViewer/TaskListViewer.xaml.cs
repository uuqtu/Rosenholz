using Rosenholz.UserControls.FolderExplorer;
using Rosenholz.ViewModel.TaskListViewer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Rosenholz.UserControls
{
    /// <summary>
    /// Interaktionslogik für TaskListViewer.xaml
    /// </summary>
    public partial class TaskListViewer : UserControl, INotifyPropertyChanged
    {
        private string _aUReference;

        public string AUReference
        {
            get { return _aUReference; }
            set { _aUReference = value; OnPropertyChanged(nameof(AUReference)); OnAUContextChanged(_aUReference); }
        }

        private void OnAUContextChanged(string aUReference)
        {
            vmo = new TaskListViewerViewModel();
            this.DataContext = vmo;
            vmo.LoadAUReferencedTask(AUReference ?? "");
        }

        TaskListViewerViewModel vmo = null;

        public TaskListViewer()
        {
            InitializeComponent();
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            vmo?.LoadAUReferencedTask(AUReference ?? "");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
