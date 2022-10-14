using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.ViewModel
{

    public delegate void TaskSourceChanged();
    public class TaskEntryViewModel : INotifyPropertyChanged
    {
        private TaskModel _entry = null;
        private string _status;
        public event TaskSourceChanged TaskSourceChangedEvent;

        public string Status { get { return _status; } set { _status = value; OnPropertyChanged(nameof(Status)); } }
        private string _openCloseButtonText => (Entry?.TaskState != TaskState.Closed) ? "Aufgabe schließen" : "Aufgabe wiedereröffnen";
        public string OpenCloseButtonText
        {
            get { return _openCloseButtonText; }
        }


        public TaskEntryViewModel()
        {
        }

        public TaskModel Entry
        {
            get { return _entry; }
            set
            {
                _entry = value;
                OnPropertyChanged(nameof(Entry));
                OnPropertyChanged(nameof(OpenCloseButtonText));
            }
        }

        private RelayCommand _updateTaskCommand;
        public RelayCommand UpdateTaskCommand
        {
            get
            {
                if (_updateTaskCommand == null)
                {
                    _updateTaskCommand = new RelayCommand(
                        (parameter) => UpdateTaskExecute(parameter),
                        (parameter) => CanEcexuteTaskupdate(parameter)
                    );
                }
                return _updateTaskCommand;
            }
        }

        private bool CanEcexuteTaskupdate(object parameter)
        {
            return true;
        }

        public void UpdateTaskExecute(object window)
        {
            if (Entry != null)
            {
                Rosenholz.Model.TaskStorage.Instance.UpdateTask(Entry, Entry.TaskState, Entry.Title, Entry.Description, Entry.TargetDate, Entry.FocusDate);
                TaskSourceChangedEvent?.Invoke();
            }
        }

        #region Task Item Entry 
        private RelayCommand _addTaskItemEntryCommand;
        public RelayCommand AddTaskItemEntryCommand
        {
            get
            {
                if (_addTaskItemEntryCommand == null)
                {
                    _addTaskItemEntryCommand = new RelayCommand(
                        (parameter) => AddTaskEntryExecute(parameter),
                        (parameter) => CanEcexuteTaskEntryAdd(parameter)
                    );
                }
                return _addTaskItemEntryCommand;
            }
        }

        private bool CanEcexuteTaskEntryAdd(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Status);
        }

        public void AddTaskEntryExecute(object window)
        {
            if (Entry != null)
            {
                var status = Status.Split('@');

                var tim = new TaskItemModel(Entry.Id);
                tim.Status = status[0];
                if (status.Length > 1)
                    tim.Respobsible = status[1];
                else
                    tim.Respobsible = "-";
                Entry.TaskItemItems.Add(tim);
                Rosenholz.Model.TaskStorage.Instance.InsertTaskItem(tim);
            }
            Status = "";
        }
        #endregion

        #region Open AU Folder 
        private RelayCommand _openAUFolderCommand;
        public RelayCommand OpenAUFolderCommand
        {
            get
            {
                if (_openAUFolderCommand == null)
                {
                    _openAUFolderCommand = new RelayCommand(
                        (parameter) => OpenAUFolderExecute(parameter),
                        (parameter) => CanEcexuteOpenAUFolder(parameter)
                    );
                }
                return _openAUFolderCommand;
            }
        }

        private bool CanEcexuteOpenAUFolder(object parameter)
        {
            return !(Entry?.AUReference == null);
        }

        public void OpenAUFolderExecute(object window)
        {
            Process.Start(Path.Combine(Rosenholz.Settings.Settings.Instance.BasePath, "ZAV", Entry.AUReference.Substring(Entry.AUReference.Length - 2, 2), Entry.AUReference));
        }
        #endregion

        #region Aufgabe schließen
        private RelayCommand _closeTaskCommand;
        public RelayCommand CloseTaskCommand
        {
            get
            {
                if (_closeTaskCommand == null)
                {
                    _closeTaskCommand = new RelayCommand(
                        (parameter) => CloseTaskExecute(parameter),
                        (parameter) => CanEcexuteCloseTask(parameter)
                    );
                }
                return _closeTaskCommand;
            }
        }

        private bool CanEcexuteCloseTask(object parameter)
        {
            return !(Entry == null);
        }

        public void CloseTaskExecute(object window)
        {
            if (Entry?.TaskState != TaskState.Closed)
                Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.Closed);
            else
                Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.New);
            Entry = null;
            TaskSourceChangedEvent?.Invoke();

        }
        #endregion

        #region Aufgabe archivieren
        private RelayCommand _archiveTaskCommand;
        public RelayCommand ArchiveTaskCommand
        {
            get
            {
                if (_archiveTaskCommand == null)
                {
                    _archiveTaskCommand = new RelayCommand(
                        (parameter) => archiveTaskExecute(parameter),
                        (parameter) => CanEcexutearchiveTask(parameter)
                    );
                }
                return _archiveTaskCommand;
            }
        }

        private bool CanEcexutearchiveTask(object parameter)
        {
            return !(Entry == null);
        }

        public void archiveTaskExecute(object window)
        {
            Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.Archived);
            Entry = null;
            TaskSourceChangedEvent?.Invoke();

        }
        #endregion

        #region View leeren 
        private RelayCommand _clearTaskViewCommand;
        public RelayCommand ClearTaskViewCommand
        {
            get
            {
                if (_clearTaskViewCommand == null)
                {
                    _clearTaskViewCommand = new RelayCommand(
                        (parameter) => ClearTaskViewExecute(parameter),
                        (parameter) => CanEcexuteclearTaskView(parameter)
                    );
                }
                return _clearTaskViewCommand;
            }
        }

        private bool CanEcexuteclearTaskView(object parameter)
        {
            return !(Entry == null);
        }

        public void ClearTaskViewExecute(object window)
        {
            Entry = null;
            TaskSourceChangedEvent?.Invoke();

        }
        #endregion

        #region Neue verlinkte Aufgabe
        private RelayCommand _createNewLinkedTaskCommand;
        public RelayCommand CreateNewLinkedTaskCommand
        {
            get
            {
                if (_createNewLinkedTaskCommand == null)
                {
                    _createNewLinkedTaskCommand = new RelayCommand(
                        (parameter) => CreateNewLinkedTaskExecute(parameter),
                        (parameter) => CanEcexutecreateNewLinkedTask(parameter)
                    );
                }
                return _createNewLinkedTaskCommand;
            }
        }

        private bool CanEcexutecreateNewLinkedTask(object parameter)
        {
            return !(Entry == null);
        }

        public void CreateNewLinkedTaskExecute(object window)
        {
            Entry = null;
            TaskSourceChangedEvent?.Invoke();
        }
        #endregion

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
