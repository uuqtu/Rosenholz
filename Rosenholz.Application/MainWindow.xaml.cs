using Microsoft.VisualBasic;
using Microsoft.Win32;
using Rosenholz.Model;
using Rosenholz.UserControls;
using Rosenholz.ViewModel;
using Rosenholz.ViewModel.DrawIo;
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
using MessageBox = System.Windows.MessageBox;
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
        Rosenholz.ViewModel.DisplayParentTaskViewModel ArchivedTevm { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel newTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel terminatedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel archivedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel focusedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel dueTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.TaskCollectionDisplayViewModel closedTaskViewModel { get; set; } = null;
        Rosenholz.ViewModel.DrawIo.DrawIoViewModel drawIoViewModel { get; set; } = null;

        #region Memorex
        Rosenholz.ViewModel.Memorex.CategoryViewModel categoryViewModelObject { get; set; } = null;
        Rosenholz.ViewModel.Memorex.InputViewModel inputViewModelObject { get; set; } = null;
        Rosenholz.ViewModel.Memorex.SearchViewModel searchViewModelObject { get; set; } = null;
        #endregion

        public bool IsDesiredCloseButtonClicked = false;
        public bool AskForValidation = true;


        public Rosenholz.ViewModel.F22ViewModel f22ViewModelObject { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();

            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            AUExplorer.TextEditorRequiredEvent += AUExplorer_TextEditorRequiredEvent;
            AUExplorer.TaskCreationRequiredEvent += AUExplorer_TaskCreationRequiredEvent;
            AUExplorer.DisplayTaskViewModelRequiredEvent += AUExplorer_DisplayTaskViewModelRequiredEvent;
            AUExplorer.WindowStateChangeRequiredEvent += AUExplorer_WindowStateChangeRequiredEvent;

            drawIoViewModel = new Rosenholz.ViewModel.DrawIo.DrawIoViewModel();
            drawIoViewModel.StartWebServer();

            DataContext = this;
        }

        /// <summary>
        /// Es ist immer am besten, wenn die App geschlossen wird, wenn das System seinen zustand ändert.
        /// Einfach aus sicherheit für die f16 und f22 Dateien.
        /// Daher wir die App einfach hart Beendet. ;-)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            Console.WriteLine(e.Mode.ToString());
            if (e.Mode == PowerModes.Suspend)
            {
                Environment.Exit(0);
            }

            if (e.Mode == PowerModes.Resume)
            {
                Environment.Exit(0);
            }
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
            {
                AUExplorer.OnCurrentFolderChanged(reference);
                //Get Folder in DrawIO Model view
                drawIoViewModel.FolderPath = FolderManager.Instance.GetAUFolder(reference?.AUReferenceString);
            }
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

        private void ArchivedTaskViewUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            archivedTaskViewModel = new Rosenholz.ViewModel.TaskCollectionDisplayViewModel();
            archivedTaskViewModel.LoadItems(TaskState.Archived);
            archivedTaskViewModel.TaskContextChangedEvent += delegate (TaskModel m) { CurrentlySelectedModel = ArchivedTevm.Entry = m; };
            ArchivedTaskViewUserControl.DataContext = archivedTaskViewModel;
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
        /// <summary>
        /// For the draw.io view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawIoView_Loaded(object sender, RoutedEventArgs e)
        {            
            drawIoViewModel.SetSource();
            DrawIoUserControl.DataContext = drawIoViewModel;
        }

        /// <summary>
        /// For the Task manager view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskView_Loaded(object sender, RoutedEventArgs e)
        {
            Tevm = new Rosenholz.ViewModel.DisplayParentTaskViewModel();
            TaskView.DataContext = Tevm;
            Tevm.TaskSourceChangedEvent += Tevm_TaskSourceChangedEvent;
            Tevm.ChildRequredEvent += Tevm_ChildRequredEvent;
            Tevm.DisplayTaskViewModelRequiredEvent += Tevm_TaskModelViewRequiredEvent;
        }
        /// <summary>
        /// For the Archived Tasks View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TaskViewArchived_Loaded(object sender, RoutedEventArgs e)
        {
            ArchivedTevm = new Rosenholz.ViewModel.DisplayParentTaskViewModel();
            TaskViewArchived.DataContext = ArchivedTevm;
            ArchivedTevm.TaskSourceChangedEvent += Tevm_TaskSourceChangedEvent;
            ArchivedTevm.ChildRequredEvent += Tevm_ChildRequredEvent;
            ArchivedTevm.DisplayTaskViewModelRequiredEvent += Tevm_TaskModelViewRequiredEvent;
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

        private void MemorexCategoryView_Loaded(object sender, RoutedEventArgs e)
        {
            categoryViewModelObject = new Rosenholz.ViewModel.Memorex.CategoryViewModel();
            categoryViewModelObject.CategoryViewModelChanged += CategoryViewModelObject_CategoryViewModelChanged;
            MemorexCategoryView.DataContext = categoryViewModelObject;

        }

        private void CategoryViewModelObject_CategoryViewModelChanged(object sender, EventArgs e)
        {
            inputViewModelObject.LoadCategories();
        }

        private void MemorexInputView_Loaded(object sender, RoutedEventArgs e)
        {
            inputViewModelObject = new Rosenholz.ViewModel.Memorex.InputViewModel();
            inputViewModelObject.LoadCategories();
            inputViewModelObject.InputViewModelChanged += InputViewModelObject_InputViewModelChanged;
            MemorexInputView.DataContext = inputViewModelObject;
        }

        private void InputViewModelObject_InputViewModelChanged(object sender, EventArgs e)
        {
            searchViewModelObject.LoadItems();
        }

        private void MemorexSearchView_Loaded(object sender, RoutedEventArgs e)
        {
            searchViewModelObject = new Rosenholz.ViewModel.Memorex.SearchViewModel();
            searchViewModelObject.LoadItems();
            MemorexSearchView.DataContext = searchViewModelObject;
        }

        private void Tevm_TaskSourceChangedEvent()
        {
            newTaskViewModel.LoadItems(TaskState.New);
            terminatedTaskViewModel.LoadItems(TaskState.Terminated);
            focusedTaskViewModel.LoadItems(TaskState.Focused);
            dueTaskViewModel.LoadItems(TaskState.Due);
            closedTaskViewModel.LoadItems(TaskState.Closed);
            //Kann sein dass das hier nicht nötig ist, aber ich wollte kein extra event handler
            archivedTaskViewModel.LoadItems(TaskState.Archived);
        }

        private void TaskbarIcon_TrayMouseDoubleClick(System.Object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
            Thread.Sleep(500);
            this.WindowState = WindowState.Maximized;
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            switch (this.WindowState)
            {
                case WindowState.Maximized:
                    ShowInTaskbar = true;
                    break;
                case WindowState.Minimized:
                    ShowInTaskbar = false;
                    break;
                case WindowState.Normal:

                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                F22ViewModel.ArchiveF22Execute();
                F16ViewModel.ArchiveF16Execute();

                F22ViewModel.WriteF22ItemsCommandExecute();
                F16ViewModel.WriteF16ItemsCommandExecute();
            }
            catch (Exception ex)
            {
                Rosenholz.Extensions.MessageBox box = new Extensions.MessageBox("Fehler beim Erzeugen der Archive!", ex.Message);
                box.ShowDialog();
            }


            if (IsDesiredCloseButtonClicked)
            {
                if (AskForValidation)
                    e.Cancel = !IsValidated();
                else
                    e.Cancel = false;

                // Desired X button clicked - statements
                if (e.Cancel)
                {
                    IsDesiredCloseButtonClicked = false; // reset the flag
                    return;
                }
            }
            else
            {
                e.Cancel = true;
                this.WindowState = WindowState.Minimized;
            }
        }

        private bool IsValidated()
        {
            //this.Visibility = Visibility.Hidden;

            Rosenholz.Extensions.MessageBox box = new Extensions.MessageBox("Warning", "Is it okay to exit?");
            box.ShowDialog();

            return box.DialogResult == true;
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
                            this.WindowState = WindowState.Minimized;
                            Thread.Sleep(500);
                            this.WindowState = WindowState.Maximized;
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
                        (parameter) => { this.WindowState = WindowState.Minimized; },
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
                strings.Add(String.Concat(item.TargetDate.ToShortDateString(), ":\r\n[", item.F16F22Reference, "]-[", item.AUReference, "]\r\n", item.Title, "\r\n"));
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

            if (strings.Count == 0)
                return;

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
                        (parameter) => { IsDesiredCloseButtonClicked = true; Close(); },
                        (parameter) => true
                    );
                }
                return _closeCommand;
            }
        }

        /// <summary>Brings main window to foreground.</summary>
        public void BringToForeground()
        {
            if (this.WindowState == WindowState.Minimized || this.Visibility == Visibility.Hidden)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }

            // According to some sources these steps gurantee that an app will be brought to foreground.
            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }


    }
}
