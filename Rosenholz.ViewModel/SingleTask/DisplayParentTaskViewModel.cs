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
    public delegate void ChildRequred(TaskModel parent);
    public delegate void TaskModelViewRequired(TaskModel parent);
    public class DisplayParentTaskViewModel : DisplayTaskViewModelBase
    {
        public event TaskSourceChanged TaskSourceChangedEvent;
        public event ChildRequred ChildRequredEvent;
        public event TaskModelViewRequired TaskModelViewRequiredEvent;

        public DisplayParentTaskViewModel()
        {
        }

        #region Update Task 

        /// <summary>
        /// Schreibt die update Werte in die Datenbnak und informiert, dass sich etwas geändert hat.
        /// </summary>
        /// <param name="window"></param>
        public override void UpdateTaskExecute(object window)
        {
            if (Entry != null)
            {
                Rosenholz.Model.TaskStorage.Instance.UpdateTask(Entry, Entry.TaskState, Entry.Title, Entry.Description, Entry.TargetDate, Entry.FocusDate);
                TaskSourceChangedEvent?.Invoke();
            }
        }
        #endregion


        /// <summary>
        /// Passiert wenn der Status Closed gesetzt wird.
        /// </summary>
        /// <param name="window"></param>
        public override void CloseTaskExecute(object window)
        {
            if (Entry?.TaskState != TaskState.Closed)
                Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.Closed);
            else
                Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.New);
            Entry = null;
            TaskSourceChangedEvent?.Invoke();
        }



        #region Aufgabe archivieren
        public new RelayCommand ArchiveTaskCommand
        {
            get
            {
                if (_archiveTaskCommand == null)
                {
                    _archiveTaskCommand = new RelayCommand(
                        (parameter) => archiveTaskExecute(parameter),
                        (parameter) => !(Entry == null)
                    );
                }
                return _archiveTaskCommand;
            }
        }

        public void archiveTaskExecute(object window)
        {
            Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.Archived);
            Entry = null;
            TaskSourceChangedEvent?.Invoke();
        }
        #endregion

        #region View leeren 
        public new RelayCommand ClearTaskViewCommand
        {
            get
            {
                if (_clearTaskViewCommand == null)
                {
                    _clearTaskViewCommand = new RelayCommand(
                        (parameter) => ClearTaskViewExecute(parameter),
                        (parameter) => !(Entry == null)
                    );
                }
                return _clearTaskViewCommand;
            }
        }

        public void ClearTaskViewExecute(object window)
        {
            Entry = null;
            TaskSourceChangedEvent?.Invoke();

        }
        #endregion

        #region Neue verlinkte Aufgabe
        public new RelayCommand CreateNewLinkedTaskCommand
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


        public void CreateNewLinkedTaskExecute(object window)
        {
            //Der Parent wird dem Kind übergeben, damit die Verbindung angelegt werden kann.
            ChildRequredEvent?.Invoke(Entry);
            Entry = null;
            TaskSourceChangedEvent?.Invoke();
        }
        #endregion

        #region Öffne verlinkte Aufgabe
        public new RelayCommand OpenLinkedTaskCommand
        {
            get
            {
                if (_openLinkedTaskCommand == null)
                {
                    _openLinkedTaskCommand = new RelayCommand(
                        (parameter) => TaskModelViewRequiredEvent?.Invoke(CurrentChildSelected),
                        (parameter) => !(CurrentChildSelected == null)
                    );
                }
                return _openLinkedTaskCommand;
            }
        }

        #endregion

    }
}
