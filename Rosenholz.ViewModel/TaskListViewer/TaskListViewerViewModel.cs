using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.ViewModel.TaskListViewer
{
    public class TaskListViewerViewModel : ViewModelBase
    {
        ObservableCollection<TaskModel> _taskModels = null;

        public ObservableCollection<TaskModel> TaskModels { get { return _taskModels; } set { _taskModels = value; OnPropertyChanged(nameof(TaskModels)); } }
        public TaskListViewerViewModel()
        {
        }

        public void LoadAllTasks()
        {
            TaskModels = new ObservableCollection<TaskModel>(Rosenholz.Model.TaskStorage.Instance.ReadTasks());
        }

        public void LoadAUReferencedTask(string task)
        {
            TaskModels = new ObservableCollection<TaskModel>(Rosenholz.Model.TaskStorage.Instance.ReadTask(task));
        }


    }
}
