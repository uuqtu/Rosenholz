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

#warning Modelle vereinheitlichen Children auch erlauben Children zu haben.

    }
}
