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
        Rosenholz.ViewModel.TaskViewModel newTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskViewModel terminatedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskViewModel focusedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskViewModel dueTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskViewModel closedTaskViewModel { get; set; } = null;




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
            newTaskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            newTaskViewModel.LoadItems(TaskState.New);
            newTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            NewTaskViewUserControl.DataContext = newTaskViewModel;
        }
        private void TerminatedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            terminatedTaskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            terminatedTaskViewModel.LoadItems(TaskState.Terminated);
            terminatedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            TerminatedTaskViewUserControl.DataContext = terminatedTaskViewModel;
        }
        private void FocusedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            focusedTaskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            focusedTaskViewModel.LoadItems(TaskState.Focused);
            focusedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            FocusedTaskViewUserControl.DataContext = focusedTaskViewModel;
        }

        private void DueTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dueTaskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            dueTaskViewModel.LoadItems(TaskState.Due);
            dueTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            DueTaskViewUserControl.DataContext = dueTaskViewModel;
        }
        private void ClosedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            closedTaskViewModel = new Rosenholz.ViewModel.TaskViewModel();
            closedTaskViewModel.LoadItems(TaskState.Closed);
            closedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            ClosedTaskViewUserControl.DataContext = closedTaskViewModel;
        }




        private void TaskView_Loaded(object sender, RoutedEventArgs e)
        {
            Tevm = new Rosenholz.ViewModel.TaskEntryViewModel();
            TaskView.DataContext = Tevm;
            Tevm.TaskSourceChangedEvent += Tevm_TaskSourceChangedEvent;
        }

        private void Tevm_TaskSourceChangedEvent()
        {
            newTaskViewModel.LoadItems(TaskState.New);
            terminatedTaskViewModel.LoadItems(TaskState.Terminated);
            focusedTaskViewModel.LoadItems(TaskState.Focused);
            dueTaskViewModel.LoadItems(TaskState.Due);
            closedTaskViewModel.LoadItems(TaskState.Closed);
        }
    }
}
