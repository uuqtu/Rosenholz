using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public class TaskModel
    {
        private Guid _id;
        private DateTime _created;
        private string _title;
        private string _description;
        private DateTime _targetDate;
        private TaskState _taskState;
        private DateTime _focusDate;
        private ObservableCollection<TaskItemModel> _taskItemItems;
        private ObservableCollection<TaskModel> _linkedtaskItems;

        private string _f16f22Reference;
        private string _f16f22ReferenceName;
        private int _level;
        private string _auReference; 
        private string _auReferenceName;

        public TaskModel(Guid id, DateTime created, string title, string description, DateTime targetDate, TaskState taskState, DateTime focusDate, ObservableCollection<TaskItemModel> taskItemItems, string f16f22Reference, int level, string auReference, ObservableCollection<TaskModel> linkedTaskItems)
        {
            Id = id;
            Created = created;
            Title = title;
            Description = description;
            TargetDate = targetDate;
            TaskState = taskState;
            FocusDate = focusDate;
            TaskItemItems = taskItemItems;
            F16F22Reference = f16f22Reference;
            Level = level;
            AUReference = auReference;
            LinkedTaskItems = linkedTaskItems;
        }

        public TaskModel()
        {
            _id = Guid.NewGuid();
            _created = DateTime.Now;
            _targetDate = DateTime.Now;
            _focusDate = DateTime.Now;
            _taskState = TaskState.New;
            _level = 0;
        }

        public TaskModel(string titel)
        {
            _title = titel;
            _id = Guid.NewGuid();
            _created = DateTime.Now;
            _taskState = TaskState.New;
        }

        public Guid Id { get { return _id; } set { _id = value; OnPropertyChanged(nameof(Id)); } }
        public DateTime Created { get { return _created; } set { _created = value; OnPropertyChanged(nameof(Created)); } }
        public string Title { get { return _title; } set { _title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get { return _description; } set { _description = value; OnPropertyChanged(nameof(Description)); } }
        public DateTime TargetDate { get { return _targetDate; } set { _targetDate = value; OnPropertyChanged(nameof(TargetDate)); } }
        public TaskState TaskState { get { return _taskState; } set { _taskState = value; OnPropertyChanged(nameof(TaskState)); OnPropertyChanged(nameof(TaskStateString)); } }
        public DateTime FocusDate { get { return _focusDate; } set { _focusDate = value; OnPropertyChanged(nameof(FocusDate)); } }
        public string F16F22Reference { get { return _f16f22Reference; } set { _f16f22Reference = value; OnPropertyChanged(nameof(F16F22Reference)); } }
        public string F16F22ReferenceName { get { return Model.F16Storage.Instance.GetF16(F16F22Reference).Keyword; } set { _f16f22ReferenceName = value; OnPropertyChanged(nameof(F16F22ReferenceName)); } }
        public int Level { get { return _level; } set { _level = value; OnPropertyChanged(nameof(Level)); } }
        public string AUReference { get { return _auReference; } set { _auReference = value; OnPropertyChanged(nameof(AUReference)); } }
        public string AUReferenceName { get { return Model.F22Storage.Instance.GetF22(AUReference).Pseudonym; } set { _auReferenceName = value; OnPropertyChanged(nameof(AUReferenceName)); } }
        public ObservableCollection<TaskItemModel> TaskItemItems { get { return _taskItemItems; } set { _taskItemItems = value; OnPropertyChanged(nameof(TaskItemItems)); } }
        public ObservableCollection<TaskModel> LinkedTaskItems { get { return _linkedtaskItems; } set { _linkedtaskItems = value; OnPropertyChanged(nameof(LinkedTaskItems)); } }
        public string TaskStateString => TaskState.ToString();

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public static TaskState ParseTaskState(string value)
        {

            Enum.TryParse(Convert.ToString(value), out TaskState myStatus);

            return myStatus;
        }

    }

    public enum TaskState
    {
        New = 0, // Aufgabe, die als Created Datum "heute" hat
        Terminated = 1, // Aufgabe, die Ziel und Fokustermin in der Zukunft hat
        Focused = 2, // --> Aufgabe, die den Fokustermin erreicht hat
        Due = 4, //--> Aufgabe die das Zieldatum überschritten hat
        Closed = 5,
        Archived = 6,
        None = 99
    }
}
