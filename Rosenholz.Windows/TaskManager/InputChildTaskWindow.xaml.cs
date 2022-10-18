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

namespace Rosenholz.Windows
{
    /// <summary>
    /// Interaktionslogik für InputChildTaskWindow.xaml
    /// </summary>
    public partial class InputChildTaskWindow : Window, INotifyPropertyChanged
    {
        public TaskModel ReturnValue;
        private TaskModel _parent;
        private Rosenholz.ViewModel.CaptureChildTaskViewModel _vmo;
        public Rosenholz.ViewModel.CaptureChildTaskViewModel Vmo { get { return _vmo; } set { _vmo = value; OnPropertyChanged(nameof(Vmo)); } }

        public InputChildTaskWindow(TaskModel parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private void TaskEntryUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Vmo = new ViewModel.CaptureChildTaskViewModel(_parent);
            Vmo.OnChildIsSet += Vmo_OnChildIsSet;
            this.DataContext = Vmo;
        }

        private void Vmo_OnChildIsSet(TaskModel child)
        {
            ReturnValue = child;
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
