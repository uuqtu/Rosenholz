using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rosenholz.Model
{
    public class TaskItemModel : INotifyPropertyChanged
    {
        private DateTime _created;
        private string _status;
        private string _responsible;
        private Guid _referenceId;

        public DateTime Created
        { get { return _created; } set { _created = value; } }

        public string Status { get { return _status; } set { _status = value; } }

        public string Respobsible { get { return _responsible; } set { _responsible = value; } }
        public Guid ReferenceId { get { return _referenceId; } set { _referenceId = value; } }

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
