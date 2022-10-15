using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.ViewModel
{
    public abstract class CaptureTaskViewModelBase : ViewModelBase
    {
        internal TaskModel _entry;
        private RelayCommand _addTaskEntryCommand;

        public TaskModel Entry
        {
            get { return _entry; }
            set
            {
                _entry = value;
                OnPropertyChanged(nameof(Entry));
            }
        }

        #region AddTaskToDatabase
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

        public abstract void AddTaskEntryExecute(object window);
        #endregion
    }
}
