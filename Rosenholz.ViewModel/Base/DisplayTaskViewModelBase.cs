using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.ViewModel
{
    public delegate void TaskSourceChanged();
    public delegate TaskModel ChildRequred(TaskModel parent);
    public delegate void TaskModelViewRequired(TaskModel parent);
    public abstract class DisplayTaskViewModelBase : ViewModelBase
    {
        #region private
        internal TaskModel _entry;
        internal string _status;
        internal string _openCloseButtonText => (Entry?.TaskState != TaskState.Closed) ? "Aufgabe schließen" : "Aufgabe wiedereröffnen";
        internal RelayCommand _addTaskItemEntryCommand;
        internal RelayCommand _updateTaskCommand;
        internal RelayCommand _openAUFolderCommand;
        internal RelayCommand _closeTaskCommand;
        internal RelayCommand _openLinkedTaskCommand;
        internal RelayCommand _createNewLinkedTaskCommand;
        internal RelayCommand _archiveTaskCommand;
        internal RelayCommand _clearTaskViewCommand;
        #endregion

        public TaskModel CurrentChildSelected { get; set; }
        public string Status { get { return _status; } set { _status = value; OnPropertyChanged(nameof(Status)); } }

        public string OpenCloseButtonText
        {
            get { return _openCloseButtonText; }
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

        #region AddTaskItemEntryCommand : Ermöglicht das Schreiben von Status-Einträgen 

        public RelayCommand AddTaskItemEntryCommand
        {
            get
            {
                if (_addTaskItemEntryCommand == null)
                {
                    _addTaskItemEntryCommand = new RelayCommand(
                        (parameter) => AddTaskEntryExecute(parameter),
                        (parameter) => !string.IsNullOrWhiteSpace(Status)
                    );
                }
                return _addTaskItemEntryCommand;
            }
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

        #region Open AU Folder: Öffnet den Ordner des archivierten Untersuchungsvorgangs
        public RelayCommand OpenAUFolderCommand
        {
            get
            {
                if (_openAUFolderCommand == null)
                {
                    _openAUFolderCommand = new RelayCommand(
                        (parameter) => Process.Start(Path.Combine(Rosenholz.Settings.Settings.Instance.BasePath, "ZAV", Entry.AUReference.Substring(Entry.AUReference.Length - 2, 2), Entry.AUReference)),
                    (parameter) => !(Entry?.AUReference == null)
                    );
                }
                return _openAUFolderCommand;
            }
        }

        #endregion

        #region Aufgabe schließen: Setzen des Closed Task-State
        public RelayCommand CloseTaskCommand
        {
            get
            {
                if (_closeTaskCommand == null)
                {
                    _closeTaskCommand = new RelayCommand(
                        (parameter) => CloseTaskExecute(parameter),
                        (parameter) => !(Entry == null)
                    );
                }
                return _closeTaskCommand;
            }
        }

        public abstract void CloseTaskExecute(object window);
        #endregion

        #region Update To Database: Writes Changes. 
        public RelayCommand UpdateTaskCommand
        {
            get
            {
                if (_updateTaskCommand == null)
                {
                    _updateTaskCommand = new RelayCommand(
                        (parameter) => UpdateTaskExecute(parameter),
                        (parameter) => true
                    );
                }
                return _updateTaskCommand;
            }
        }

        public abstract void UpdateTaskExecute(object parameter);
        #endregion

        #region Neue verlinkte Aufgabe
        public RelayCommand CreateNewLinkedTaskCommand
        {
            get
            {
                if (_createNewLinkedTaskCommand == null)
                {
                    _createNewLinkedTaskCommand = new RelayCommand(
                        (parameter) => CreateNewLinkedTaskExecute(parameter),
                        (parameter) => !(Entry == null)
                    );
                }
                return _createNewLinkedTaskCommand;
            }
        }


        public abstract void CreateNewLinkedTaskExecute(object window);
        #endregion


        #region Dummy, damit Felder grau.

        private bool Dummy(object parameter)
        {
            return !(Entry == null);
        }


        public virtual RelayCommand ArchiveTaskCommand
        {
            get
            {
                if (_archiveTaskCommand == null)
                {
                    _archiveTaskCommand = new RelayCommand(
                        (parameter) => Dummy(parameter),
                        (parameter) => false
                    );
                }
                return _archiveTaskCommand;
            }
        }


        public virtual RelayCommand ClearTaskViewCommand
        {
            get
            {
                if (_clearTaskViewCommand == null)
                {
                    _clearTaskViewCommand = new RelayCommand(
                        (parameter) => Dummy(parameter),
                        (parameter) => false
                    );
                }
                return _clearTaskViewCommand;
            }
        }




        public virtual RelayCommand OpenLinkedTaskCommand
        {
            get
            {
                if (_openLinkedTaskCommand == null)
                {
                    _openLinkedTaskCommand = new RelayCommand(
                        (parameter) => Dummy(parameter),
                        (parameter) => false
                    );
                }
                return _openLinkedTaskCommand;
            }
        }
        #endregion

    }
}
