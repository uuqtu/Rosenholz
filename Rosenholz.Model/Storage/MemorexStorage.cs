using Rosenholz.Model.Memorex;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rosenholz.Model.Storage
{
    public sealed class MemorexStorage
    {

        private static readonly MemorexStorage instance = new MemorexStorage();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static MemorexStorage()
        {
        }

        private MemorexStorage()
        {
            CreateTable();
        }


        public static MemorexStorage Instance
        {
            get
            {
                return instance;
            }
        }


        public void CreateTable()
        {
            string dir = Settings.Settings.Instance.MemorexSubLocation;

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);


            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS ENTRIES (" +
                                                         "ID TEXT PRIMARY KEY, " +
                                                         "LINK TEXT, " +
                                                         "SEARCHWORDS TEXT, " +
                                                         "CATEGORY TEXT)";

                con.CreateTable(command);
            }

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                string command =
                    "CREATE TABLE IF NOT EXISTS CATEGORIES (" +
                                                "ID TEXT PRIMARY KEY, " +
                                                "CATEGORY TEXT)";

                con.CreateTable(command);
            }
        }

        public void InsertData(KnowledgeElement Insertee)
        {
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                string command =
                    "INSERT INTO ENTRIES (ID, LINK, SEARCHWORDS, CATEGORY)" +
                    "VALUES ('" + Insertee.Guid + "','" + Insertee.Link + "','" + string.Join(" ", Insertee.Searchwords) + "','" + Insertee.Category + "');";

                con.InsertData(command);
            }
        }

        public void InserCategoryIfNotExist(string Insertee)
        {
            var cats = ReadData().Select(s => s.Category.ToLowerInvariant()).ToArray();
            if (cats.Contains(Insertee.ToLowerInvariant()))
                return;

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                string guid = Guid.NewGuid().ToString();

                string command =
                    "INSERT INTO CATEGORIES (ID, CATEGORY)" +
                    "VALUES ('" + guid + "','" + Insertee + "');";

                con.InsertData(command);
            }
        }

        public void DeleteEntry(string guid)
        {
            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                string command = $"DELETE FROM ENTRIES WHERE ID LIKE '{guid}' ";

                con.InsertData(command);
            }
        }


        public IList<KnowledgeElement> ReadData()
        {
            DataTable data = null;
            List<KnowledgeElement> values = new List<KnowledgeElement>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                data = con.ReadData("SELECT * FROM ENTRIES");
            }

            values = (from rw in data.AsEnumerable()
                      select new KnowledgeElement()
                      {
                          Category = Convert.ToString(rw["CATEGORY"]),
                          Guid = Convert.ToString(rw["ID"]),
                          Searchwords = Convert.ToString(rw["SEARCHWORDS"]),
                          Link = Convert.ToString(rw["LINK"])
                      }).ToList();

            return values;
        }

        public IList<string> ReadCategoryData()
        {
            DataTable data = null;
            List<string> values = new List<string>();

            using (var con = new SQLiteConnectionHelper(Path.Combine(Settings.Settings.Instance.MemorexSubLocation, Settings.Settings.Instance.MemorexFileName)))
            {
                data = con.ReadData("SELECT DISTINCT CATEGORY FROM CATEGORIES");
            }

            //values = data.Rows.Cast<DataRow>().Select(r => Convert.ToString(r["CATEGORY"])).ToList();

            values = (from rw in data.AsEnumerable()
                      select rw["CATEGORY"]).OfType<string>().ToList();

            return values;
        }




    }
}

