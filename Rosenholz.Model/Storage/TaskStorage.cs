using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rosenholz.Model
{
    public sealed class TaskStorage
    {
        private static readonly TaskStorage instance = new TaskStorage();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static TaskStorage()
        {
        }

        private TaskStorage()
        {
            CreateTable();
        }


        public static TaskStorage Instance
        {
            get
            {
                return instance;
            }
        }


        public void CreateTable()
        {

            string dir = Settings.Settings.Instance.TaskSubLocation;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);



            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS TASKS (" +
                    "ID TEXT PRIMARY KEY UNIQUE," +
                    "TASKSTATE TEXT NOT NULL," +
                    "CREATED TEXT NOT NULL," +
                    "TITLE TEXT NOT NULL," +
                    "DESCRIPTION TEXT NOT NULL," +
                    "TARGETDATE TEXT NOT NULL," +
                    "FOCUSDATE TEXT NOT NULL," +
                    "F16F22REFERENCE TEXT NOT NULL," +
                    "LEVEL TEXT NOT NULL," +
                    "AUREFERENCE TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TaskItemsFileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS TASKITEMS (" +
                    "CREATED TEXT NOT NULL," +
                    "STATUS TEXT NOT NULL," +
                    "RESPONSIBLE TEXT NOT NULL," +
                    "REFERENCEID TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TaskLinkFileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS LINKEDTASKITEMS (" +
                    "PARENT TEXT NOT NULL," +
                    "CHILD TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }
        }

        public void InsertTask(TaskModel Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                string command =
                    "INSERT INTO TASKS (ID, TASKSTATE, CREATED, TITLE, DESCRIPTION, TARGETDATE, FOCUSDATE, F16F22REFERENCE, LEVEL, AUREFERENCE)" +
                    "VALUES ('" + Insertee.Id.ToString() + "'," +
                            "'" + Insertee.TaskState.ToString() + "'," +
                            "'" + Insertee.Created.ToUniversalTime() + "'," +
                            "'" + Insertee.Title + "'," +
                            "'" + Insertee.Description + "'," +
                            "'" + Insertee.TargetDate + "'," +
                            "'" + Insertee.FocusDate + "'," +
                            "'" + Insertee.F16F22Reference + "'," +
                            "'" + Insertee.Level + "'," +
                            "'" + Insertee.AUReference + "');";

                con.InsertData(command);
            }
        }

        public void InsertTaskItem(TaskItemModel Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TaskItemsFileName)))
            {
                string command =
                    "INSERT INTO TASKITEMS (CREATED, STATUS, RESPONSIBLE, REFERENCEID)" +
                    "VALUES ('" + Insertee.Created.ToUniversalTime() + "'," +
                            "'" + Insertee.Status + "'," +
                            "'" + Insertee.Respobsible + "'," +
                            "'" + Insertee.ReferenceId.ToString() + "');";

                con.InsertData(command);
            }
        }

        public void InsertChild(TaskModel parent, TaskModel child)
        {
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TaskLinkFileName)))
            {

                {
                    string command =
                        "INSERT INTO LINKEDTASKITEMS (PARENT, CHILD)" +
                        "VALUES ('" + parent.Id + "'," +
                                "'" + child.Id + "');";

                    con.InsertData(command);
                }
            }
        }
        /// <summary>
        /// Gets the Children to a parent
        /// </summary>
        /// <param name="parent"></param>
        /// <returns></returns>
        public IList<TaskModel> GetChildren(TaskModel parent)
        {
            DataTable data = null;
            List<Guid> values = new List<Guid>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TaskLinkFileName)))
            {
                data = con.ReadData($"SELECT CHILD FROM LINKEDTASKITEMS WHERE PARENT='{parent.Id}'");
            }

            foreach (DataRow row in data.Rows)
            {
                values.Add(new Guid(row["CHILD"].ToString()));
            }

            DataTable data_childs = null;
            List<TaskModel> values_childs = new List<TaskModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                data_childs = con.ReadData($"SELECT * FROM TASKS WHERE LEVEL='{parent.Level + 1}'");
            }

            values_childs = (from rw in data_childs.AsEnumerable()
                             select new TaskModel()
                             {
                                 Id = Guid.Parse(Convert.ToString(rw["ID"])),
                                 TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                                 Created = ConvertToDateTime(rw["CREATED"]),
                                 Title = Convert.ToString(rw["TITLE"]),
                                 Description = Convert.ToString(rw["DESCRIPTION"]),
                                 TargetDate = ConvertToDateTime(rw["TARGETDATE"]),
                                 FocusDate = ConvertToDateTime(rw["FOCUSDATE"]),
                                 F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                                 Level = Convert.ToInt32(rw["LEVEL"]),
                                 AUReference = Convert.ToString(rw["AUREFERENCE"])
                             }).ToList();


            var retaval = values_childs.Where(t => values.Contains(t.Id));

            foreach (TaskModel task in retaval)
            {
                task.TaskItemItems = new ObservableCollection<TaskItemModel>(ReadTaskItems(task));
            }

            foreach (TaskModel task in retaval)
            {
                task.LinkedTaskItems = new ObservableCollection<TaskModel>(GetChildren(task));
            }

            //values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

            return retaval.ToList();
        }


        public IList<TaskModel> ReadTasks(Rosenholz.Model.TaskState state = TaskState.None)
        {
            SetStatus();
            DataTable data = null;
            List<TaskModel> values = new List<TaskModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                if (state == TaskState.None)
                    data = con.ReadData($"SELECT * FROM TASKS WHERE LEVEL='0'");
                else
                    data = con.ReadData($"SELECT * FROM TASKS WHERE TASKSTATE='{state.ToString()}' AND LEVEL='0'");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskModel()
                      {
                          Id = Guid.Parse(Convert.ToString(rw["ID"])),
                          TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                          Created = ConvertToDateTime(rw["CREATED"]),
                          Title = Convert.ToString(rw["TITLE"]),
                          Description = Convert.ToString(rw["DESCRIPTION"]),
                          TargetDate = ConvertToDateTime(rw["TARGETDATE"]),
                          FocusDate = ConvertToDateTime(rw["FOCUSDATE"]),
                          F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                          Level = Convert.ToInt32(rw["LEVEL"]),
                          AUReference = Convert.ToString(rw["AUREFERENCE"])
                      }).ToList();

            foreach (TaskModel task in values)
            {
                task.TaskItemItems = new ObservableCollection<TaskItemModel>(ReadTaskItems(task));
            }

            foreach (TaskModel task in values)
            {
                task.LinkedTaskItems = new ObservableCollection<TaskModel>(GetChildren(task));
            }

            return values;
        }

        public IList<TaskModel> ReadTask(string f22Reference)
        {
            SetStatus();
            DataTable data = null;
            List<TaskModel> values = new List<TaskModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                data = con.ReadData($"SELECT * FROM TASKS WHERE LEVEL='0' AND AUREFERENCE='{f22Reference}'");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskModel()
                      {
                          Id = Guid.Parse(Convert.ToString(rw["ID"])),
                          TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                          Created = ConvertToDateTime(rw["CREATED"]),
                          Title = Convert.ToString(rw["TITLE"]),
                          Description = Convert.ToString(rw["DESCRIPTION"]),
                          TargetDate = ConvertToDateTime(rw["TARGETDATE"]),
                          FocusDate = ConvertToDateTime(rw["FOCUSDATE"]),
                          F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                          Level = Convert.ToInt32(rw["LEVEL"]),
                          AUReference = Convert.ToString(rw["AUREFERENCE"])
                      }).ToList();

            foreach (TaskModel task in values)
            {
                task.TaskItemItems = new ObservableCollection<TaskItemModel>(ReadTaskItems(task));
            }

            foreach (TaskModel task in values)
            {
                task.LinkedTaskItems = new ObservableCollection<TaskModel>(GetChildren(task));
            }

            return values;
        }

        private void SetStatus()
        {
            DataTable data = null;
            List<TaskModel> values = new List<TaskModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                data = con.ReadData($"SELECT * FROM TASKS WHERE LEVEL='0'");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskModel()
                      {
                          Id = Guid.Parse(Convert.ToString(rw["ID"])),
                          TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                          Created = ConvertToDateTime(rw["CREATED"]),
                          Title = Convert.ToString(rw["TITLE"]),
                          Description = Convert.ToString(rw["DESCRIPTION"]),
                          TargetDate = ConvertToDateTime(rw["TARGETDATE"]),
                          FocusDate = ConvertToDateTime(rw["FOCUSDATE"]),
                          F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                          Level = Convert.ToInt32(rw["LEVEL"]),
                          AUReference = Convert.ToString(rw["AUREFERENCE"])
                      }).ToList();


            foreach (TaskModel task in values)
            {
                if (task.TaskState == TaskState.Closed || task.TaskState == TaskState.Archived)
                    continue;

                if (task.FocusDate > task.TargetDate)
                    UpdateFocusDate(task, task.TargetDate);

                if (task.Created.Year == DateTime.Now.Year && task.Created.Month == DateTime.Now.Month && task.Created.Day == DateTime.Now.Day)
                {
                    UpdateTaskState(task, TaskState.New);
                    continue;
                }
                else if (task.FocusDate > DateTime.Now && task.TargetDate > DateTime.Now)
                {
                    UpdateTaskState(task, TaskState.Terminated);
                    continue;
                }
                else if (task.FocusDate <= DateTime.Now && task.TargetDate > DateTime.Now)
                {
                    UpdateTaskState(task, TaskState.Focused);
                    continue;
                }
                else if (task.TargetDate <= DateTime.Now)
                {
                    UpdateTaskState(task, TaskState.Due);
                    continue;
                }
                else
                {
                    UpdateTaskState(task, TaskState.Due);
                    continue;
                }
            }
        }

        private static DateTime ConvertToDateTime(object rw)
        {
            var candidate = Convert.ToString(rw);
            DateTime convertionResult = DateTime.MinValue;

            string[] formats = { "dd/MM/yyyy HH:mm:ss",
                                 "dd/M/yyyy HH:mm:ss",
                                 "d/M/yyyy HH:mm:ss",
                                 "d/MM/yyyy HH:mm:ss",
                                 "dd/MM/yy HH:mm:ss",
                                 "dd/M/yy HH:mm:ss",
                                 "d/M/yy HH:mm:ss",
                                 "d/MM/yy HH:mm:ss",
                                 "dd/MM/yyyy HH:mm:ss tt",
                                 "dd/M/yyyy HH:mm:ss tt",
                                 "d/M/yyyy HH:mm:ss tt",
                                 "d/MM/yyyy HH:mm:ss tt",
                                 "dd/MM/yy HH:mm:ss tt",
                                 "dd/M/yy HH:mm:ss tt",
                                 "d/M/yy HH:mm:ss tt",
                                 "d/MM/yy HH:mm:ss tt",
                                 "dd.MM.yyyy HH:mm:ss"};

            bool conversionSuccess = false;

            try
            {
                conversionSuccess = DateTime.TryParseExact(candidate, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out convertionResult);

                if (convertionResult == DateTime.MinValue)
                {
                    var datestring = candidate;
                    DateTime time = DateTime.MinValue;
                    var matchingCulture = CultureInfo.GetCultures(CultureTypes.AllCultures).FirstOrDefault(ci => DateTime.TryParse(datestring, ci, DateTimeStyles.None, out time));

                    if (time == DateTime.MinValue)
                    {
                        conversionSuccess = false;
                        throw new CultureNotFoundException($"Datenformat von {candidate} konnte nicht in gültiges DateTime gewandelt werden.");
                    }
                    else
                        convertionResult = time;
                }

            }
            catch (Exception ex)
            {
                if (!conversionSuccess)
                {
                    MessageBox.Show(ex.Message);
                    throw ex;
                }
            }

            return convertionResult;
        }

        private IList<TaskItemModel> ReadTaskItems(TaskModel task)
        {
            DataTable data = null;
            List<TaskItemModel> values = new List<TaskItemModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TaskItemsFileName)))
            {
                data = con.ReadData($"SELECT * FROM TASKITEMS WHERE REFERENCEID = '{task.Id}'");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskItemModel()
                      {
                          Created = ConvertToDateTime(rw["CREATED"]),
                          Status = Convert.ToString(rw["STATUS"]),
                          Respobsible = Convert.ToString(rw["RESPONSIBLE"]),
                          ReferenceId = Guid.Parse((Convert.ToString(rw["REFERENCEID"])))
                      }).ToList();
            //values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

            return values;
        }

        public void UpdateTask(TaskModel toUpdate, TaskState state, string title, string description, DateTime targetDatre, DateTime focusdate)
        {
            List<TaskItemModel> values = new List<TaskItemModel>();
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                string command =
                    $"UPDATE TASKS " +
                    $"SET TASKSTATE = '{state.ToString()}', " +
                    $"TITLE = '{title}', " +
                    $"DESCRIPTION = '{description}', " +
                    $"TARGETDATE = '{targetDatre}', " +
                    $"FOCUSDATE = '{focusdate}' " +
                    $"WHERE " +
                    $"ID = '{toUpdate.Id}'"; ;

                con.UpdateData(command);
            }
        }

        public void UpdateTaskState(TaskModel toUpdate, TaskState state)
        {
            List<TaskItemModel> values = new List<TaskItemModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                string command =
                    $"UPDATE TASKS " +
                    $"SET TASKSTATE = '{state.ToString()}' " +
                    $"WHERE " +
                    $"ID = '{toUpdate.Id}'"; ;

                con.UpdateData(command);
            }
        }

        public void UpdateFocusDate(TaskModel toUpdate, DateTime focusdate)
        {
            List<TaskItemModel> values = new List<TaskItemModel>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.TaskSubLocation, Settings.Settings.Instance.TasksFileName)))
            {
                string command =
                    $"UPDATE TASKS " +
                    $"SET FOCUSDATE = '{focusdate}' " +
                    $"WHERE " +
                    $"ID = '{toUpdate.Id}'"; ;

                con.UpdateData(command);
            }
        }

    }
}
