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
    public class CaptureChildTaskViewModel : CaptureTaskViewModelBase
    {
        private TaskModel _parent;

        public CaptureChildTaskViewModel(TaskModel parent)
        {
            _parent = parent;
            _entry = new TaskModel();
            _entry.AUReference = parent.AUReference;
            _entry.F16F22Reference = parent.F16F22Reference;
            _entry.IsChild = true;
        }

        public override void AddTaskEntryExecute(object window)
        {
            Rosenholz.Model.TaskStorage.Instance.InsertChild(_parent, Entry);
            Rosenholz.Model.TaskStorage.Instance.InsertTask(Entry);
            if (window is Window)
            {
                (window as Window).Close();
            }
        }
    }
}
