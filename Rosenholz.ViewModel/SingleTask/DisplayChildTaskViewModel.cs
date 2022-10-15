using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.ViewModel
{

    public class DisplayChildTaskViewModel : DisplayTaskViewModelBase
    {

        public event ChildRequred ChildRequredEvent;

        public DisplayChildTaskViewModel(TaskModel entry)
        {
            base._entry = entry;
        }

        /// <summary>
        /// Schreibt das Update in die Datenbank
        /// </summary>
        /// <param name="window"></param>
        public override void UpdateTaskExecute(object window)
        {
            if (Entry != null)
            {
                Rosenholz.Model.TaskStorage.Instance.UpdateTask(Entry, Entry.TaskState, Entry.Title, Entry.Description, Entry.TargetDate, Entry.FocusDate);
                //TaskSourceChangedEvent?.Invoke();
            }
        }

        /// <summary>
        /// Setzt nur den Status Closed und schreibt das in die Datenbank
        /// </summary>
        /// <param name="window"></param>
        public override void CloseTaskExecute(object window)
        {
            if (Entry?.TaskState != TaskState.Closed)
            {
                Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.Closed);
                Entry.TaskState = TaskState.Closed;
                OnPropertyChanged(nameof(Entry.TaskState));
            }
            else
            {
                Rosenholz.Model.TaskStorage.Instance.UpdateTaskState(Entry, TaskState.New);
                Entry.TaskState = TaskState.New;
                OnPropertyChanged(nameof(Entry.TaskState));
            }
#warning anschauen
            Entry = Entry;
        }

        public override void CreateNewLinkedTaskExecute(object window)
        {
            //Der Parent wird dem Kind übergeben, damit die Verbindung angelegt werden kann.
            var child = ChildRequredEvent?.Invoke(Entry);
            if (child != null)
                Entry.LinkedTaskItems.Add(child);
            else
                Entry = null;
            //Entry = null;
#warning hier muss noch irgendwie einmal neu Laden getriggert werden
            //TaskSourceChangedEvent?.Invoke();
        }



    }
}
