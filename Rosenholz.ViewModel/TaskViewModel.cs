using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Rosenholz.ViewModel
{
    public delegate void TaskContextChanged(TaskModel reference);
    public class TaskViewModel : INotifyPropertyChanged
    {
        private TaskModel _currentlySelectedTask;
        private ObservableCollection<TaskModel> _taskItems;
        private ListCollectionView _taskCollectionView;
        public event TaskContextChanged TaskContextChangedEvent;
        private string _textFilter;

        public ObservableCollection<TaskModel> TaskItems { get { return _taskItems; } set { _taskItems = value; OnPropertyChanged(nameof(TaskItems)); } }
        public ICollectionView TaskCollectionView
        {
            get { return this._taskCollectionView; }
        }

        public TaskModel CurrentlySelectedTask
        {
            get { return _currentlySelectedTask; }
            set
            {
                _currentlySelectedTask = value;
                OnPropertyChanged(nameof(CurrentlySelectedTask));
                if (CurrentlySelectedTask != null)
                    TaskContextChangedEvent?.Invoke(CurrentlySelectedTask);
            }
        }

        public string TextFilter
        {
            get { return this._textFilter; }
            set
            {
                _textFilter = value;
                OnPropertyChanged(nameof(TextFilter));

                //https://stackoverflow.com/questions/15473048/create-a-textboxsearch-to-filter-from-listview-wpf
                if (String.IsNullOrEmpty(value))
                    TaskCollectionView.Filter = null;
                else
                    TaskCollectionView.Filter = new Predicate<object>(o => ((TaskModel)o).Description?.ToLower()?.Contains(value.ToLower()) == true ||
                                                                       ((TaskModel)o).Title?.ToLower()?.Contains(value.ToLower()) == true);
            }
        }


        public void LoadItems(Rosenholz.Model.TaskState state)
        {
            var a = Rosenholz.Model.TaskStorage.Instance.ReadTasks(state);
            TaskItems = new ObservableCollection<TaskModel>(a);
            _taskCollectionView = new ListCollectionView(TaskItems);
            OnPropertyChanged(nameof(TaskCollectionView));
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
