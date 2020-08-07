
using System;
using System.Text;
using Microsoft.Data.Sqlite;
using System.IO;
using System.Data;
using DatabaseUtils;
using System.Collections.Generic;
using TaskTracker.DataStore;
using CsvHelper;
using System.Globalization;
using TaskTracker;
using System.Linq;

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
            // . netcoreapp3.1
            // .. debug
            // ... bin
            // .... project directory
            var currentDirectory = Environment.CurrentDirectory;
            var projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName;
            return projectDirectory;
        }

        

        public static void ExecuteNonQuery(string sql, SqliteConnection openConnection)
        {
            DatabaseUtils.SqliteUtilities.ExecuteNonQuery(sql, openConnection);
        }

 
        public static List<string> GetTableNames(SqliteConnection openConnection)
        {
            return DatabaseUtils.SqliteUtilities.GetTableNames(openConnection);
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


        #region bootstrap
        


        //private static string GetTaskTableCreationString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("CREATE TABLE \"Task\" (")
        //        .Append("\t\"Id\"    INTEGER,")
        //        .Append("\t\"Title\" TEXT NOT NULL,")
        //        .Append("\t\"Description\"   TEXT NOT NULL,")
        //        .Append("\t\"AssignedToUserId\"  INTEGER NOT NULL,")
        //        .Append("\t\"SourceUserId\"  INTEGER NOT NULL,")
        //        .Append("\t\"DateCreated\"   TEXT NOT NULL,")
        //        .Append("\t\"DateAssigned\"  TEXT, ")
        //        .Append("\t\"DateCompleted\" TEXT, ")
        //        .Append("\t\"Notes\" TEXT,")
        //        .Append("\tPRIMARY KEY(\"Id\" AUTOINCREMENT));");
        //    return sb.ToString();
        //}

        //private static void PopulateTaskTable(SqliteConnection openConnection)
        //{
        //    var sql = "INSERT INTO Task (Title, Description, AssignedToUserId, " +
        //                    "SourceUserId, DateCreated, DateAssigned, DateCompleted, Notes)" +
        //                "VALUES(@title, @description, @assignedToUserId, @sourceUserId, " +
        //                    "@dateCreated, @dateAssigned, @dateCompleted, @notes);";

        //    var simpleStore = new SimpleStore();
        //    using (var transaction = openConnection.BeginTransaction())
        //    {
        //        foreach (var task in simpleStore.GetAllTasks())
        //        {
        //            using (var command = openConnection.CreateCommand())
        //            {
        //                command.Transaction = transaction;
        //                command.CommandText = sql;
        //                command.CommandType = CommandType.Text;

        //                command.Parameters.Add(new SqliteParameter("@title", task.Title));
        //                command.Parameters.Add(new SqliteParameter("@description", task.Description));
        //                command.Parameters.Add(new SqliteParameter("@assignedToUserId", 1));
        //                command.Parameters.Add(new SqliteParameter("@sourceUserId", 1));
        //                command.Parameters.Add(new SqliteParameter("@dateCreated", task.DateCreated));
        //                command.Parameters.Add(new SqliteParameter("@dateAssigned", task.DateAssigned?.ToString() ?? ""));
        //                command.Parameters.Add(new SqliteParameter("@dateCompleted", task.DateCompleted?.ToString() ?? ""));
        //                command.Parameters.Add(new SqliteParameter("@notes", task.Notes ?? ""));

        //                command.ExecuteNonQuery();
        //            }
        //        }
        //        transaction.Commit();
        //    }
        //}
        #endregion


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
