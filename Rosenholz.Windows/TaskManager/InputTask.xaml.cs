using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
    /// Interaktionslogik für InputTask.xaml
    /// </summary>
    public partial class InputTask : Window, INotifyPropertyChanged
    {
        private Rosenholz.ViewModel.CaptureTaskViewModel _vmo;
        public Rosenholz.ViewModel.CaptureTaskViewModel Vmo { get { return _vmo; } set { _vmo = value; OnPropertyChanged(nameof(Vmo)); } }
        private string _aufRef;
        public InputTask(string auRef)
        {
            InitializeComponent();
            _aufRef = Rosenholz.Model.AUReference.GetAUStringFromPath(auRef).Value;
        }

        private void TaskEntryUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Vmo = new ViewModel.CaptureTaskViewModel();
            Vmo.Entry.AUReference = _aufRef;
            Vmo.Entry.F16F22Reference = Rosenholz.Model.AUReference.GetMetadataF1622Reference(_aufRef);
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
