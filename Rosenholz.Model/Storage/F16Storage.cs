using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model
{
    public sealed class F16Storage
    {

        private static readonly F16Storage instance = new F16Storage();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static F16Storage()
        {
        }

        private F16Storage()
        {
            CreateTable();
        }


        public static F16Storage Instance
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

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.F16SubLocation, Settings.Settings.Instance.F16FileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS F16 (" +
                    "F16F22REFERENCE TEXT PRIMARY KEY UNIQUE," +
                    "KEYWORD TEXT NOT NULL," +
                    "LABEL TEXT NOT NULL," +
                    "PURPOSE TEXT NOT NULL" +
                    ");";

                con.CreateTable(command);
            }
    }

    public void InsertData(F16 Insertee)
    {
        using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.F16SubLocation, Settings.Settings.Instance.F16FileName)))
        {
            string command =
                "INSERT INTO F16 (F16F22REFERENCE, KEYWORD, LABEL, PURPOSE)" +
                "VALUES ('" + Insertee.F16F22Reference.F22String + "','" + Insertee.Keyword + "','" + Insertee.Label + "','" + Insertee.Purpose + "');";

            con.InsertData(command);
        }
    }

    public IList<F16> ReadData()
    {
        DataTable data = null;
        List<F16> values = new List<F16>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.F16SubLocation, Settings.Settings.Instance.F16FileName)))
            {
                data = con.ReadData("SELECT * FROM F16");
            }

        values = (from rw in data.AsEnumerable()
                  select new F16()
                  {
                      Keyword = Convert.ToString(rw["Keyword"]),
                      Label = Convert.ToString(rw["Label"]),
                      Purpose = Convert.ToString(rw["Purpose"]),
                      F16F22Reference = new F16F22Reference(Convert.ToString(rw["F16F22Reference"]))
                  }).ToList();



        values.Sort((x, y) => y.F16F22Reference.CompareTo(x.F16F22Reference));

        return values;
    }

        public F16 GetF16(string f16F22Reference)
        {
            DataTable data = null;
            List<F16> values = new List<F16>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.F16SubLocation, Settings.Settings.Instance.F16FileName)))
            {
                data = con.ReadData($"SELECT * FROM F16 WHERE F16F22REFERENCE=\'{f16F22Reference}\'");
            }

            values = (from rw in data.AsEnumerable()
                      select new F16()
                      {
                          Keyword = Convert.ToString(rw["Keyword"]),
                          Label = Convert.ToString(rw["Label"]),
                          Purpose = Convert.ToString(rw["Purpose"]),
                          F16F22Reference = new F16F22Reference(Convert.ToString(rw["F16F22Reference"]))
                      }).ToList();

            return values.First();
        }

        public void WriteFiles()
    {
        var items = ReadData();




    }


}
}
