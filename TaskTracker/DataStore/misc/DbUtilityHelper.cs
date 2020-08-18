
using System;
using System.Text;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Reflection;



namespace DataStore.misc
{
    public abstract class DbUtilityHelper
    {
        protected static readonly string cwdPath = GetDirectory();
        protected static readonly string dbName = "TaskTracker.db";
        protected static readonly string sqlFolder = "sqlStuff";


        public static string GetConnectionInfo(SqliteConnection cnn)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.AppendLine($"Connection String: {cnn.ConnectionString}").
                AppendLine($"State: {cnn.State}")
                .AppendLine($"Connection Timeout: {cnn.ConnectionTimeout}")
                .AppendLine($"Database: {cnn.Database}")
                .AppendLine($"Data Source: {cnn.DataSource}")
                .AppendLine($"Server Version: {cnn.ServerVersion}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return sb.ToString();
        }

        protected static string GetDirectory()
        {

            var currentLocation = Assembly.GetExecutingAssembly().Location;
            var currentDirectory = new FileInfo(currentLocation).Directory;
            var almostThere = currentDirectory.Parent.Parent.Parent.Parent.FullName;
            var directory = Path.Combine(almostThere, "DataStore");

            //return projectDirectory;
            //return System.AppDomain.CurrentDomain.BaseDirectory;
            //var path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            
            //var path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..\\..\\..\\"));

            
            return directory;
        }

        

        public static void ExecuteNonQuery(string sql, SqliteConnection openConnection)
        {
            using (SqliteCommand cmd = new SqliteCommand(sql, openConnection))
            {
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
            }
        }

 
        public static List<string> GetTableNames(SqliteConnection openConnection)
        {
            using (SqliteCommand cmd = openConnection.CreateCommand())
            {
                var sql = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%'";
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                SqliteDataReader reader = cmd.ExecuteReader();
                List<string> tableNames = new List<string>();
                while (reader.Read())
                {
                    tableNames.Add(reader.GetString(0));
                }
                return tableNames;
            }
        }


        public static int GetRowCount(SqliteConnection openConnection, string tableName, string colName = "Id")
        {
            try
            {
                using (var cmd = openConnection.CreateCommand())
                {
                    cmd.CommandText = $"SELECT COUNT({colName}) FROM {tableName};";
                    cmd.CommandType = CommandType.Text;
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    return reader.GetInt32(0);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(GetConnectionInfo(openConnection));
                Console.WriteLine(e);
                throw e;
            }
        }



        protected static string GetConnectionString(string id = "Default")
        {
            return Path.Combine(cwdPath, dbName); 
        }

        public static SqliteConnection GetConnection()
        {
            SqliteConnection conn;
            string connectionString = $"Data Source={GetConnectionString()};";
            try
            {
                conn = new SqliteConnection(connectionString);
                return conn;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
