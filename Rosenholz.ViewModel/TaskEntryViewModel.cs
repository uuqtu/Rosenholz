using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.ViewModel
{
    public delegate void TaskSourceChanged(TaskModel reference);
    public class TaskEntryViewModel : INotifyPropertyChanged
    {
        public event TaskSourceChanged TaskSourceChangedEvent;
        private TaskModel _entry = null;
        private string _status;
        private string _responsible;

        public string Status { get { return _status; } set { _status = value; OnPropertyChanged(nameof(Status)); } }
        public string Responsible { get { return _responsible; } set { _responsible = value; OnPropertyChanged(nameof(Responsible)); } }

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
            }
        }

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
            return !string.IsNullOrWhiteSpace(Status) &&
                   !string.IsNullOrWhiteSpace(Responsible);
        }

        public void AddTaskEntryExecute(object window)
        {
            var tim = new TaskItemModel(Entry.Id);
            tim.Respobsible = Responsible;
            tim.Status = Status;
            Entry.TaskItemItems.Add(tim);
            Rosenholz.Model.TaskStorage.Instance.InsertTaskItem(tim);
        }

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
