using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Rosenholz.Model
{
    public class SQLiteConnectionHelper : IDisposable
    {

        private SQLiteConnection _sqlite_conn;

        public SQLiteConnectionHelper(string dbName)
        {
            CreateConnection(dbName);
        }

        public void Dispose()
        {
            _sqlite_conn.Close();
            _sqlite_conn.Dispose();
        }

        private SQLiteConnection CreateConnection(string dbName)
        {
            // Create a new database connection:
            _sqlite_conn = new SQLiteConnection("Data Source=" + dbName + ";Version=3;New=True;Compress=True;");
            // Open the connection:
            try
            {
                _sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return _sqlite_conn;
        }

        public void CreateTable(string command)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = _sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = command;
            sqlite_cmd.ExecuteNonQuery();
        }

        public void InsertData(string command)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = _sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = command;
            sqlite_cmd.ExecuteNonQuery();
        }

        public DataTable ReadData(string command)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = _sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = command;

            SQLiteDataAdapter da = new SQLiteDataAdapter(sqlite_cmd);

            DataTable data = new DataTable();

            da.Fill(data);

            return data;
        }
    }
}
