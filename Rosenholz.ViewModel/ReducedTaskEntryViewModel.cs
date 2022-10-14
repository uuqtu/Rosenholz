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
    public class ReducedTaskEntryViewModel : INotifyPropertyChanged
    {
        private TaskModel _entry;

        public ReducedTaskEntryViewModel()
        {
            _entry = new TaskModel();
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

        private RelayCommand _addTaskEntryCommand;
        public RelayCommand AddTaskEntryCommand
        {
            get
            {
                if (_addTaskEntryCommand == null)
                {
                    _addTaskEntryCommand = new RelayCommand(
                        (parameter) => AddTaskEntryExecute(parameter),
                        (parameter) => CanEcexuteTaskEntryAdd(parameter)
                    );
                }
                return _addTaskEntryCommand;
            }
        }

        private bool CanEcexuteTaskEntryAdd(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Entry.Title) &&
                   !string.IsNullOrWhiteSpace(Entry.Description) &&
                   Entry.FocusDate <= Entry.TargetDate;
        }

        public void AddTaskEntryExecute(object window)
        {
            Rosenholz.Model.TaskStorage.Instance.InsertTask(Entry);
            if (window is Window)
            {
                (window as Window).Close();
            }

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
