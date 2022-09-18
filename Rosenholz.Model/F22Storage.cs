using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public sealed class F22Storage
    {

        private static readonly F22Storage instance = new F22Storage();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static F22Storage()
        {
        }

        private F22Storage()
        {
            CreateTable();
        }


        public static F22Storage Instance
        {
            get
            {
                return instance;
            }
        }


        public void CreateTable()
        {
            string dir = Path.GetDirectoryName(Settings.Settings.Instance.F22Location);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.F22Location))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS F22 (" +
                    "AUREFERENCE TEXT PRIMARY KEY UNIQUE," +
                    "F16F22REFERENCE TEXT NOT NULL," +
                    "PSEUDONYM TEXT NOT NULL," +
                    "CREATED TEXT NOT NULL," +
                    "LINK TEXT NOT NULL," +
                    "DOSSIER TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }
        }

        public void InsertData(F22 Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.F22Location))
            {
                string command =
                    "INSERT INTO F22 (AUREFERENCE, F16F22REFERENCE, PSEUDONYM, CREATED, LINK, DOSSIER)" +
                    "VALUES ('" + Insertee.AUReference.AUReferenceString + "','" + Insertee.F16F22Reference.F22String + "','" + Insertee.Pseudonym + "','" + Insertee.Created + "','" + Insertee.Link + "','" + Insertee.Dossier + "');";

                con.InsertData(command);
            }
        }

        public IList<F22> ReadData()
        {
            DataTable data = null;
            List<F22> values = new List<F22>();

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.F22Location))
            {
                data = con.ReadData("SELECT * FROM F22");
            }

            values = (from rw in data.AsEnumerable()
                      select new F22()
                      {
                          AUReference = new AUReference(Convert.ToString(rw["AUReference"])),
                          F16F22Reference = new F16F22Reference(Convert.ToString(rw["F16F22Reference"])),
                          Pseudonym = Convert.ToString(rw["Pseudonym"]),
                          Created = Convert.ToString(rw["Created"]),
                          Link = Convert.ToString(rw["Link"]),
                          Dossier = Convert.ToString(rw["Dossier"])
                      }).ToList();

            values.Sort((x, y) => y.AUReference.CompareTo(x.AUReference));

            return values;
        }

        public IList<F22> ReadAUElements(string f16F22Reference)
        {
            DataTable data = null;
            List<F22> values = new List<F22>();

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.F22Location))
            {
                data = con.ReadData($"SELECT * FROM F22 WHERE F16F22REFERENCE=\'{f16F22Reference}\'");
            }

            values = (from rw in data.AsEnumerable()
                      select new F22()
                      {
                          AUReference = new AUReference(Convert.ToString(rw["AUReference"])),
                          F16F22Reference = new F16F22Reference(Convert.ToString(rw["F16F22Reference"])),
                          Pseudonym = Convert.ToString(rw["Pseudonym"]),
                          Created = Convert.ToString(rw["Created"]),
                          Link = Convert.ToString(rw["Link"]),
                          Dossier = Convert.ToString(rw["Dossier"])
                      }).ToList();

            values.Sort((x, y) => y.AUReference.CompareTo(x.AUReference));

            return values;
        }

        public IList<F22> SelectData(F16F22Reference reference)
        {
            DataTable data = null;
            List<F22> values = new List<F22>();

            using (var con = new SQLiteConnectionHelper(Settings.Settings.Instance.F22Location))
            {
                data = con.ReadData($"SELECT * FROM F22 WHERE F16F22REFERENCE = '{reference.F22String}'");
            }

            values = (from rw in data.AsEnumerable()
                      select new F22()
                      {
                          AUReference = new AUReference(Convert.ToString(rw["AUReference"])),
                          F16F22Reference = new F16F22Reference(Convert.ToString(rw["F16F22Reference"])),
                          Pseudonym = Convert.ToString(rw["Pseudonym"]),
                          Created = Convert.ToString(rw["Created"]),
                          Link = Convert.ToString(rw["Link"]),
                          Dossier = Convert.ToString(rw["Dossier"])
                      }).ToList();


            return values;
        }
    }
}
