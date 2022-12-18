using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model.CompletionOfAssignments
{
    public class CoA
    {
        private string _taskName;
        private DateTime _endDate;
        private string _timeEstimation;
        private string _description;
        private string _location;
        private bool _state;


        public CoA()
        {

        }

        public CoA(string taskName, DateTime _endDate, string timeEstimation, string description, string location, bool state)
        {
            TaskName = taskName;
            EndDate = _endDate;
            TimeEstimation = timeEstimation;
            Description = description;
            Location = location;
            State = state;
        }

        public string TaskName
        {
            get
            {
                return _taskName;
            }
            set
            {
                _taskName = value;
                OnPropertyChanged(nameof(TaskName));
            }
        }
        public DateTime EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
                OnPropertyChanged(nameof(EndDate));
            }
        }
        public string TimeEstimation
        {
            get
            {
                return _timeEstimation;
            }
            set
            {
                _timeEstimation = value;
                OnPropertyChanged(nameof(TimeEstimation));
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public string Location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        public bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged(nameof(State));
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

