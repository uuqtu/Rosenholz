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
    /// Interaktionslogik für ShowChildTaskWindow.xaml
    /// </summary>
    public partial class ShowChildTaskWindow : Window, INotifyPropertyChanged
    {
        private TaskModel _toShow;
        private Rosenholz.ViewModel.DisplayChildTaskViewModel _vmo;
        public Rosenholz.ViewModel.DisplayChildTaskViewModel Vmo { get { return _vmo; } set { _vmo = value; OnPropertyChanged(nameof(Vmo)); } }

        public ShowChildTaskWindow(TaskModel toShow)
        {
            _toShow = toShow;
            InitializeComponent();
        }

        private void TaskEntryUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Vmo = new ViewModel.DisplayChildTaskViewModel(_toShow);
            Vmo.ChildRequredEvent += Vmo_ChildRequredEvent;
            Vmo.DisplayTaskViewModelRequiredEvent += Vmo_DisplayTaskViewModelRequiredEvent;
            this.DataContext = Vmo;
        }

        private void Vmo_DisplayTaskViewModelRequiredEvent(TaskModel parent)
        {
            Rosenholz.Windows.ShowChildTaskWindow childview = new Rosenholz.Windows.ShowChildTaskWindow(parent);
            childview.ShowDialog();
        }

        private TaskModel Vmo_ChildRequredEvent(TaskModel parent)
        {
            Rosenholz.Windows.InputChildTaskWindow child = new Rosenholz.Windows.InputChildTaskWindow(parent);
            child.ShowDialog();
            return child.ReturnValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
