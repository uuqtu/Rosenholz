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
    /// Interaktionslogik für InputTask.xaml
    /// </summary>
    public partial class InputTask : Window, INotifyPropertyChanged
    {
        public Rosenholz.ViewModel.TaskEntryViewModel vmo { get; set; } = null;
        private string _aufRef;
        public InputTask(string auRef)
        {
            InitializeComponent();
            _aufRef = Rosenholz.Model.AUReference.GetAUStringFromPath(auRef).Value;
        }

        private void TaskEntryUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            vmo = new ViewModel.TaskEntryViewModel();
            vmo.Entry.AUReference = _aufRef;
            vmo.Entry.F16F22Reference = Rosenholz.Model.AUReference.GetMetadataF1622Reference(_aufRef);
            vmo.Entry.F22Reference = Rosenholz.Model.AUReference.GetMetadataF1622Reference(_aufRef);
            this.DataContext = vmo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Rosenholz.Model.TaskStorage.Instance.InsertTask(vmo.Entry);
            this.Close();
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
