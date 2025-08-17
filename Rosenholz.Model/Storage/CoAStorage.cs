using Rosenholz.Model.CompletionOfAssignments;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model.Storage
{
    public sealed class CoAStorage
    {

        private static readonly CoAStorage instance = new CoAStorage();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static CoAStorage()
        {
        }

        private CoAStorage()
        {
            CreateTable();
        }


        public static CoAStorage Instance
        {
            get
            {
                return instance;
            }
        }

        public void CreateTable()
        {
            string dir = Settings.Settings.Instance.F16SubLocation;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.CompletionOfAssignmentsLocation, Settings.Settings.Instance.CompletionOfAssignmentsFileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS COA (" +
                    "TASKNAME TEXT NOT NULL," +
                    "ENDDATE TEXT NOT NULL," +
                    "TIMEESTIMATION TEXT NOT NULL," +
                    "DESCRIPTION TEXT NOT NULL," +
                    "LOCATION TEXT NOT NULL," +
                    "STATE TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }
        }

        public void InsertData(CoA Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.CompletionOfAssignmentsLocation, Settings.Settings.Instance.CompletionOfAssignmentsFileName)))
            {
                string command =
                    "INSERT INTO COA (TASKNAME, ENDDATE, TIMEESTIMATION, DESCRIPTION, LOCATION, STATE)" +
                    "VALUES ('" + Insertee.TaskName + "','" + Insertee.EndDate.ToString() + "','" + Insertee.TimeEstimation + "','" + Insertee.Description + "','" + Insertee.Location + "','" + Insertee.State + "');";

                con.InsertData(command);
            }
        }

        public IList<CoA> ReadData()
        {
            DataTable data = null;
            List<CoA> values = new List<CoA>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.CompletionOfAssignmentsLocation, Settings.Settings.Instance.CompletionOfAssignmentsFileName)))
            {
                data = con.ReadData("SELECT * FROM COA");
            }

            values = (from rw in data.AsEnumerable()
                      select new CoA()
                      {
                          TaskName = Convert.ToString(rw["TASKNAME"]),
#warning Methode ConvertToDateTime(rw verwenden!
                          EndDate = DateTime.Parse(Convert.ToString(rw["ENDDATE"])),
                          TimeEstimation = Convert.ToString(rw["TIMEESTIMATION"]),
                          Description = Convert.ToString(rw["DESCRIPTION"]),
                          Location = Convert.ToString(rw["LOCATION"]),
                          State = Boolean.Parse(Convert.ToString(rw["STATE"]))
                      }).ToList();

            return values;
        }
    }
}

