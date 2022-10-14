using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Rosenholz.Task.Windows
{
    /// <summary>
    /// Interaktionslogik für InputChildTaskWindow.xaml
    /// </summary>
    public partial class InputChildTaskWindow : Window, INotifyPropertyChanged
    {
        private TaskModel _parent;
        private Rosenholz.ViewModel.ReducedChildTaskEntryViewModel _vmo;
        public Rosenholz.ViewModel.ReducedChildTaskEntryViewModel Vmo { get { return _vmo; } set { _vmo = value; OnPropertyChanged(nameof(Vmo)); } }

        public InputChildTaskWindow(TaskModel parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void TaskEntryUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Vmo = new ViewModel.ReducedChildTaskEntryViewModel(_parent);
            this.DataContext = Vmo;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
