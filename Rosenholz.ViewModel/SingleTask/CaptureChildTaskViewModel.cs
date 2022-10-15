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
    //Make Child accessable via event
    public delegate void ChildIsSet(TaskModel child);
    public class CaptureChildTaskViewModel : CaptureTaskViewModelBase
    {
        private TaskModel _parent;
        public event ChildIsSet OnChildIsSet;

        public CaptureChildTaskViewModel(TaskModel parent)
        {
            _parent = parent;
            _entry = new TaskModel();
            _entry.AUReference = parent.AUReference;
            _entry.F16F22Reference = parent.F16F22Reference;
            _entry.Level = parent.Level + 1;
        }

        public override void AddTaskEntryExecute(object window)
        {
            Rosenholz.Model.TaskStorage.Instance.InsertChild(_parent, Entry);
            Rosenholz.Model.TaskStorage.Instance.InsertTask(Entry);
            if (window is Window)
            {
                OnChildIsSet?.Invoke(Entry);
                (window as Window).Close();
            }
        }
    }
}
