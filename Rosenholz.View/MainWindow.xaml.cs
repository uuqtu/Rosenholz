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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rosenholz.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Rosenholz.Model.TaskModel CurrentlySelectedModel;
        Rosenholz.ViewModel.TaskEntryViewModel Tevm { get; set; } = null;
        public Rosenholz.ViewModel.F22ViewModel f22ViewModelObject { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void F16UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.F16ViewModel f16ViewModelObject = new ViewModel.F16ViewModel();
            f16ViewModelObject.LoadF16Items();
            f16ViewModelObject.F22ContextChangeEvent += F16ViewModelObject_F22ContextChangeEvent; ;
            F16ViewControl.DataContext = f16ViewModelObject;
        }

        private void F16ViewModelObject_F22ContextChangeEvent(F16F22Reference reference)
        {
            f22ViewModelObject = new ViewModel.F22ViewModel();
            f22ViewModelObject.AUContextChangeEvent += F22ViewModelObject_AUContextChangeEvent;
            f22ViewModelObject.SelectItems(reference);
            f22ViewModelObject.CurrentF16Reference = reference;
            F22ViewControl.DataContext = f22ViewModelObject;
        }


        private void F22ViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            f22ViewModelObject = new ViewModel.F22ViewModel();

        }

        private void F22ViewModelObject_AUContextChangeEvent(AUReference reference)
        {
            if (reference != null)
                AUExplorer.OnCurrentFolderChanged(FolderManager.Instance.GetAUFolder(reference.AUReferenceString));
            else
                AUExplorer.OnCurrentFolderChanged(null);
        }

        private void AUExplorer_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void NewTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.TaskViewModel taskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            taskViewModel.LoadItems(TaskState.New);
            taskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            NewTaskViewUserControl.DataContext = taskViewModel;
        }
        private void TerminatedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.TaskViewModel taskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            taskViewModel.LoadItems(TaskState.Terminated);
            taskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            TerminatedTaskViewUserControl.DataContext = taskViewModel;
        }
        private void FocusedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.TaskViewModel taskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            taskViewModel.LoadItems(TaskState.Focused);
            taskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            FocusedTaskViewUserControl.DataContext = taskViewModel;
        }

        private void DueTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.TaskViewModel taskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            taskViewModel.LoadItems(TaskState.Due);
            taskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            DueTaskViewUserControl.DataContext = taskViewModel;
        }
        private void ClosedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.TaskViewModel taskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            taskViewModel.LoadItems(TaskState.Closed);
            taskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            ClosedTaskViewUserControl.DataContext = taskViewModel;
        }




        private void TaskView_Loaded(object sender, RoutedEventArgs e)
        {
            Tevm = new Rosenholz.ViewModel.TaskEntryViewModel();
            TaskView.DataContext = Tevm;            
        }


    }
}
