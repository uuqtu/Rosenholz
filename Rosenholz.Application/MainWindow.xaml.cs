﻿using Microsoft.VisualBasic;
using Rosenholz.Model;
using Rosenholz.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

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
            AUExplorer.TextEditorRequiredEvent += AUExplorer_TextEditorRequiredEvent;
            AUExplorer.TaskCreationRequiredEvent += AUExplorer_TaskCreationRequiredEvent;
            AUExplorer.DisplayTaskViewModelRequiredEvent += AUExplorer_DisplayTaskViewModelRequiredEvent;
            AUExplorer.WindowStateChangeRequiredEvent += AUExplorer_WindowStateChangeRequiredEvent;

            DataContext = this;
        }

        private void AUExplorer_WindowStateChangeRequiredEvent(WindowState state)
        {
            WindowState = state;
        }

        private void AUExplorer_DisplayTaskViewModelRequiredEvent(TaskModel parent)
        {
            Rosenholz.Windows.ShowChildTaskWindow childview = new Rosenholz.Windows.ShowChildTaskWindow(parent);
            childview.ShowDialog();
        }

        //Button Pannel braucht einen Task
        private void AUExplorer_TaskCreationRequiredEvent(string currentFolder)
        {
            InputTask t = new Rosenholz.Windows.InputTask(currentFolder);
            t.ShowDialog();
        }

        private void AUExplorer_TextEditorRequiredEvent(string reference)
        {
            Rosenholz.Windows.TextEditor.TextEditorStandAlone tsa = new Windows.TextEditor.TextEditorStandAlone(reference);
            tsa.ShowDialog();
        }

        private void F16UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Rosenholz.ViewModel.F16ViewModel f16ViewModelObject = new ViewModel.F16ViewModel();
            f16ViewModelObject.LoadF16Items();
            f16ViewModelObject.F22ContextChangeEvent += F16ViewModelObject_F22ContextChangeEvent;
            f16ViewModelObject.F16EntryRequiredEvent += F16ViewModelObject_F16EntryRequiredEvent;
            F16ViewControl.DataContext = f16ViewModelObject;
        }

        private void F16ViewModelObject_F16EntryRequiredEvent(string currentF22, List<F16> list)
        {
            CreateF16 model = new CreateF16(currentF22, list);
            model.ShowDialog();
        }

        private void F16ViewModelObject_F22ContextChangeEvent(F16F22Reference reference)
        {
            f22ViewModelObject = new ViewModel.F22ViewModel();
            f22ViewModelObject.AUContextChangeEvent += F22ViewModelObject_AUContextChangeEvent;
            f22ViewModelObject.F22EntryRequiredEvent += F22ViewModelObject_F22EntryRequiredEvent;
            f22ViewModelObject.SelectItems(reference);
            f22ViewModelObject.CurrentF16Reference = reference;
            F22ViewControl.DataContext = f22ViewModelObject;
        }

        private Tuple<bool, AUReference> F22ViewModelObject_F22EntryRequiredEvent(F16F22Reference reference)
        {
            CreateF22Entry model = new CreateF22Entry(reference);
            model.ShowDialog();

            return new Tuple<bool, AUReference>(model.Aborted, model.NewAUReference);
        }

        private void F22ViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            f22ViewModelObject = new ViewModel.F22ViewModel();

        }

        private void F22ViewModelObject_AUContextChangeEvent(AUReference reference)
        {
            if (reference != null)
                AUExplorer.OnCurrentFolderChanged(reference);
            else
                AUExplorer.OnCurrentFolderChanged(null);
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
            ShowDueTasksBalloon();
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
            Tevm.DisplayTaskViewModelRequiredEvent += Tevm_TaskModelViewRequiredEvent;
        }

        private void Tevm_TaskModelViewRequiredEvent(TaskModel child)
        {
            Rosenholz.Windows.ShowChildTaskWindow childview = new Rosenholz.Windows.ShowChildTaskWindow(child);
            childview.ShowDialog();
        }

        private TaskModel Tevm_ChildRequredEvent(TaskModel parent)
        {
            Rosenholz.Windows.InputChildTaskWindow child = new Rosenholz.Windows.InputChildTaskWindow(parent);
            child.ShowDialog();
            return child.ReturnValue;
        }





        private void Tevm_TaskSourceChangedEvent()
        {
            newTaskViewModel.LoadItems(TaskState.New);
            terminatedTaskViewModel.LoadItems(TaskState.Terminated);
            focusedTaskViewModel.LoadItems(TaskState.Focused);
            dueTaskViewModel.LoadItems(TaskState.Due);
            closedTaskViewModel.LoadItems(TaskState.Closed);
        }

        private void TaskbarIcon_TrayMouseDoubleClick(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
            Thread.Sleep(500);
            WindowState = WindowState.Maximized;
        }

        private RelayCommand _maximizeCommand;
        public RelayCommand MaximizeCommand
        {
            get
            {
                if (_maximizeCommand == null)
                {
                    _maximizeCommand = new RelayCommand(
                        (parameter) =>
                        {
                            WindowState = WindowState.Minimized;
                            Thread.Sleep(500);
                            WindowState = WindowState.Maximized;
                        },
                        (parameter) => true
                    );
                }
                return _maximizeCommand;
            }
        }

        private RelayCommand _minimizeCommand;
        public RelayCommand MinimizeCommand
        {
            get
            {
                if (_minimizeCommand == null)
                {
                    _minimizeCommand = new RelayCommand(
                        (parameter) => { WindowState = WindowState.Minimized; },
                        (parameter) => true
                    );
                }
                return _minimizeCommand;
            }
        }


        private RelayCommand _getDueCommand;
        public RelayCommand GetDueCommand
        {
            get
            {
                if (_getDueCommand == null)
                {
                    _getDueCommand = new RelayCommand(
                        (parameter) => DisplayDueTasks(),
                        (parameter) => true
                    );
                }
                return _getDueCommand;
            }
        }

        private List<string> GetDueTasks()
        {
            var a = dueTaskViewModel.TaskItems;

            List<string> strings = new List<string>();

            foreach (var item in a)
            {
                strings.Add(String.Concat(item.TargetDate.ToShortDateString(), ":\r\n", item.Title, "\r\n"));
            }

            return strings;
        }

        private void DisplayDueTasks()
        {
            string fileName = Path.GetRandomFileName();
            fileName = Path.ChangeExtension(fileName, ".txt");
            fileName = Path.Combine(Path.GetTempPath(), fileName);

            File.WriteAllLines(fileName, GetDueTasks());

            Rosenholz.Windows.TextEditor.TextEditorStandAlone tsa = new Windows.TextEditor.TextEditorStandAlone(fileName);
            tsa.Show();
        }

        private void ShowDueTasksBalloon()
        {
            var strings = GetDueTasks();

            FancyBalloon balloon = new FancyBalloon();
            var str = string.Join("\r\n", strings);
            balloon.BalloonText = str;

            //show balloon and close it after 4 seconds
            MyNotifyIcon.ShowCustomBalloon(balloon, PopupAnimation.Slide, 5000);
        }

        private RelayCommand _closeCommand;
        public RelayCommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(
                        (parameter) => { Close(); },
                        (parameter) => true
                    );
                }
                return _closeCommand;
            }
        }
    }
}
