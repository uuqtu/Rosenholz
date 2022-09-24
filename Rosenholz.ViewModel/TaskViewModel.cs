using Rosenholz.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.ViewModel
{
    public class TaskViewModel
    {
        private ObservableCollection<TaskItemModel> _taskItems;

        public ObservableCollection<TaskItemModel> TaskItems { get { return _taskItems; } }


        public void LoadItems()
        {

        }

    }
}
