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
    public class CaptureTaskViewModel : CaptureTaskViewModelBase
    {
        public CaptureTaskViewModel()
        {
            base._entry = new TaskModel();
        }

        public override void AddTaskEntryExecute(object window)
        {
            Rosenholz.Model.TaskStorage.Instance.InsertTask(Entry);
            if (window is Window)
            {
                (window as Window).Close();
            }
        }
    }
}
