using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

#if DEBUG
            string dir = @"C:\Users\z0035hes\Desktop\MFS\ZTV";
#else
            string dir = Path.GetDirectoryName(Settings.Settings.Instance.TaskLocation);
#endif


            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\tasks.db"))
#else
                        using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
#endif
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
                    "ISCHILD TEXT NOT NULL," +
                    "AUREFERENCE TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\taskitems.db"))
#else
                   using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
#endif

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

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\linkedtaskitems.db"))
#else
                   using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
#endif

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
#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\tasks.db"))
#else
                        using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
#endif
            {
                string command =
                    "INSERT INTO TASKS (ID, TASKSTATE, CREATED, TITLE, DESCRIPTION, TARGETDATE, FOCUSDATE, F16F22REFERENCE, ISCHILD, AUREFERENCE)" +
                    "VALUES ('" + Insertee.Id.ToString() + "'," +
                            "'" + Insertee.TaskState.ToString() + "'," +
                            "'" + Insertee.Created.ToUniversalTime() + "'," +
                            "'" + Insertee.Title + "'," +
                            "'" + Insertee.Description + "'," +
                            "'" + Insertee.TargetDate + "'," +
                            "'" + Insertee.FocusDate + "'," +
                            "'" + Insertee.F16F22Reference + "'," +
                            "'" + Insertee.IsChild + "'," +
                            "'" + Insertee.AUReference + "');";

                con.InsertData(command);
            }
        }

        public void InsertTaskItem(TaskItemModel Insertee)
        {
#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\taskitems.db"))
#else
                   using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
#endif

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

        public void InsertChild(TaskItemModel parent, TaskItemModel child)
        {
#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\linkedtaskitems.db"))
#else
                   using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
#endif

            {
                string command =
                    "INSERT INTO LINKEDTASKITEMS (PARENT, CHILD)" +
                    "VALUES ('" + parent.ReferenceId + "'," +
                            "'" + child.ReferenceId + "');";

                con.InsertData(command);
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

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\linkedtaskitems.db"))
#else
                        using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
#endif
            {
                data = con.ReadData($"SELECT CHILD FROM LINKEDTASKITEMS WHERE PARENT='{parent.Id}'");
            }

            foreach (DataRow row in data.Rows)
            {
                values.Add(new Guid(row["CHILD"].ToString()));
            }

            DataTable data_childs = null;
            List<TaskModel> values_childs = new List<TaskModel>();

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\tasks.db"))
#else
                        using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
#endif
            {
                data_childs = con.ReadData($"SELECT * FROM TASKS WHERE ISCHILD='{true.ToString()}'");
            }

            values_childs = (from rw in data.AsEnumerable()
                             select new TaskModel()
                             {
                                 Id = Guid.Parse(Convert.ToString(rw["ID"])),
                                 TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                                 Created = DateTime.Parse(Convert.ToString(rw["CREATED"])),
                                 Title = Convert.ToString(rw["TITLE"]),
                                 Description = Convert.ToString(rw["DESCRIPTION"]),
                                 TargetDate = DateTime.Parse(Convert.ToString(rw["TARGETDATE"])),
                                 FocusDate = DateTime.Parse(Convert.ToString(rw["FOCUSDATE"])),
                                 F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                                 IsChild = Convert.ToBoolean(rw["ISCHILD"]),
                                 AUReference = Convert.ToString(rw["AUREFERENCE"])
                             }).ToList();


            var retaval = values_childs.Where(t => values.Contains(t.Id));

            foreach (TaskModel task in retaval)
            {
                task.TaskItemItems = new ObservableCollection<TaskItemModel>(ReadTaskItems(task));
            }


            //values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

            return retaval.ToList();
        }


        public IList<TaskModel> ReadTasks(Rosenholz.Model.TaskState state = TaskState.None)
        {
            DataTable data = null;
            List<TaskModel> values = new List<TaskModel>();

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\tasks.db"))
#else
                        using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
#endif
            {
                if (state == TaskState.None)
                    data = con.ReadData($"SELECT * FROM TASKS WHERE ISCHILD='{false.ToString()}'");
                else
                    data = con.ReadData($"SELECT * FROM TASKS WHERE TASKSTATE='{state.ToString()}' AND ISCHILD='{false.ToString()}'");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskModel()
                      {
                          Id = Guid.Parse(Convert.ToString(rw["ID"])),
                          TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                          Created = DateTime.Parse(Convert.ToString(rw["CREATED"])),
                          Title = Convert.ToString(rw["TITLE"]),
                          Description = Convert.ToString(rw["DESCRIPTION"]),
                          TargetDate = DateTime.Parse(Convert.ToString(rw["TARGETDATE"])),
                          FocusDate = DateTime.Parse(Convert.ToString(rw["FOCUSDATE"])),
                          F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                          IsChild = Convert.ToBoolean(rw["ISCHILD"]),
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

            //values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

            return values;
        }

        private IList<TaskItemModel> ReadTaskItems(TaskModel task)
        {
            DataTable data = null;
            List<TaskItemModel> values = new List<TaskItemModel>();

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\taskitems.db"))
#else
                   using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
#endif
            {
                data = con.ReadData($"SELECT * FROM TASKITEMS WHERE REFERENCEID = '{task.Id}'");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskItemModel()
                      {
                          Created = DateTime.Parse(Convert.ToString(rw["CREATED"])),
                          Status = Convert.ToString(rw["STATUS"]),
                          Respobsible = Convert.ToString(rw["RESPONSIBLE"]),
                          ReferenceId = Guid.Parse((Convert.ToString(rw["REFERENCEID"])))
                      }).ToList();
            //values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

            return values;
        }

        public void UpdateTask(TaskModel toUpdate, TaskState state, string title, string description, DateTime targetDatre, DateTime focusdate)
        {
            DataTable data = null;
            List<TaskItemModel> values = new List<TaskItemModel>();

#if DEBUG
            using (var con = new SQLiteConnectionHelper(@"C:\Users\z0035hes\Desktop\MFS\ZTV\tasks.db"))
#else
                   using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
#endif
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

    }
}
