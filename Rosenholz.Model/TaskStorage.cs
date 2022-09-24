﻿using System;
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
            string dir = Path.GetDirectoryName(Settings.Settings.Instance.TaskLocation);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            dir = Path.GetDirectoryName(Settings.Settings.Instance.TaskItemLocation);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
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
                    "F22REFERENCE TEXT NOT NULL," +
                    "AUREFERENCE TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
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
        }

        public void InsertTask(TaskModel Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
            {
                string command =
                    "INSERT INTO TASKS (ID, TASKSTATE, CREATED, TITLE, DESCRIPTION, TARGETDATE, FOCUSDATE, F16F22REFERENCE, F22REFERENCE, AUREFERENCE)" +
                    "VALUES ('" + Insertee.Id.ToString() + "'," +
                            "'" + Insertee.TaskState.ToString() + "'," +
                            "'" + Insertee.Created.ToUniversalTime() + "'," +
                            "'" + Insertee.Title + "'," +
                            "'" + Insertee.Description + "'," +
                            "'" + Insertee.TargetDate + "'," +
                            "'" + Insertee.FocusDate + "'," +
                            "'" + Insertee.F16F22Reference + "'," +
                            "'" + Insertee.F22Reference + "'," +
                            "'" + Insertee.AUReference + "');";

                con.InsertData(command);
            }
        }

        public void InsertTaskItem(TaskItemModel Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
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

        public IList<TaskModel> ReadTasks()
        {
            DataTable data = null;
            List<TaskModel> values = new List<TaskModel>();

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskLocation))
            {
                data = con.ReadData("SELECT * FROM TASKS");
            }

            values = (from rw in data.AsEnumerable()
                      select new TaskModel()
                      {
                          Id = Guid.Parse(Convert.ToString(rw["ID"])),
                          TaskState = Model.TaskModel.ParseTaskState(Convert.ToString(rw["TASKSTATE"])),
                          Created = DateTime.Parse(Convert.ToString(rw["CREATED"])),
                          Title = Convert.ToString(rw["TITLE"]),
                          Description = Convert.ToString(rw["DESCRIPTION"]),
                          TargetDate = Convert.ToString(rw["TARGETDATE"]),
                          FocusDate = DateTime.Parse(Convert.ToString(rw["FOCUSDATE"])),
                          F16F22Reference = Convert.ToString(rw["F16F22REFERENCE"]),
                          F22Reference = Convert.ToString(rw["F22REFERENCE"]),
                          AUReference = Convert.ToString(rw["AUREFERENCE"])
                      }).ToList();

            foreach (TaskModel task in values)
            {
                task.TaskItemItems = new ObservableCollection<TaskItemModel>(ReadTaskItems(task));
            }

            //values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

            return values;
        }

        private IList<TaskItemModel> ReadTaskItems(TaskModel task)
        {
            DataTable data = null;
            List<TaskItemModel> values = new List<TaskItemModel>();

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.TaskItemLocation))
            {
                data = con.ReadData("SELECT * FROM TASKITEMS");
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

    }
}
