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

namespace Rosenholz.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Rosenholz.Model.TaskModel CurrentlySelectedModel;
        Rosenholz.ViewModel.DisplayParentTaskViewModel Tevm { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel newTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel terminatedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel focusedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel dueTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel closedTaskViewModel { get; set; } = null;




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
            newTaskViewModel = new Rosenholz.ViewModel.TaskCollectionDisplayViewModel();
            newTaskViewModel.LoadItems(TaskState.New);
            newTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            NewTaskViewUserControl.DataContext = newTaskViewModel;
        }
        private void TerminatedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            terminatedTaskViewModel = new Rosenholz.ViewModel.TaskCollectionDisplayViewModel();
            terminatedTaskViewModel.LoadItems(TaskState.Terminated);
            terminatedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            TerminatedTaskViewUserControl.DataContext = terminatedTaskViewModel;
        }
        private void FocusedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            focusedTaskViewModel = new Rosenholz.ViewModel.TaskCollectionDisplayViewModel();
            focusedTaskViewModel.LoadItems(TaskState.Focused);
            focusedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            FocusedTaskViewUserControl.DataContext = focusedTaskViewModel;
        }

        private void DueTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dueTaskViewModel = new Rosenholz.ViewModel.TaskCollectionDisplayViewModel();
            dueTaskViewModel.LoadItems(TaskState.Due);
            dueTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            DueTaskViewUserControl.DataContext = dueTaskViewModel;
        }
        private void ClosedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            closedTaskViewModel = new Rosenholz.ViewModel.TaskCollectionDisplayViewModel();
            closedTaskViewModel.LoadItems(TaskState.Closed);
            closedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = Tevm.Entry = m; };
            ClosedTaskViewUserControl.DataContext = closedTaskViewModel;
        }




        private void TaskView_Loaded(object sender, RoutedEventArgs e)
        {
            Tevm = new Rosenholz.ViewModel.DisplayParentTaskViewModel();
            TaskView.DataContext = Tevm;
            Tevm.TaskSourceChangedEvent += Tevm_TaskSourceChangedEvent;
            Tevm.ChildRequredEvent += Tevm_ChildRequredEvent;
            Tevm.TaskModelViewRequiredEvent += Tevm_TaskModelViewRequiredEvent;
        }

        private void Tevm_TaskModelViewRequiredEvent(TaskModel child)
        {
            Rosenholz.Task.Windows.ShowChildTaskWindow childview = new Rosenholz.Task.Windows.ShowChildTaskWindow(child);
            childview.ShowDialog();
        }

        private void Tevm_ChildRequredEvent(TaskModel parent)
        {
            Rosenholz.Task.Windows.InputChildTaskWindow child = new Rosenholz.Task.Windows.InputChildTaskWindow(parent);
            child.ShowDialog();
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
