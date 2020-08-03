using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using TaskTracker;
using CsvHelper;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace DataStore.misc
{
    abstract class PostCreation : DbUtilityHelper
    {
        
        public static void InitializeDb()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();

                    var tableNames = GetTableNames(connection);


                    if (!tableNames.Contains("User"))
                    {
                        ExecuteNonQuery(GetUserTableCreationString(), connection);
                        Console.WriteLine("User table created");
                    }
                    if (GetRowCount(connection, "User") == 0)
                    {
                        PopulateUserTable(connection);
                    }

                    //if (!tableNames.Contains("Task"))
                    //{
                    //    ExecuteNonQuery(GetTaskTableCreationString(), connection);
                    //    Console.WriteLine("Task table created");
                    //}
                    //if (GetRowCount(connection, "Task") == 0)
                    //{
                    //    PopulateTaskTable(connection);
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        

        private static string GetSqlFromFile(string fileName)
        {
            var fullName = Path.Join(cwdPath, sqlFolder, fileName);
            return File.ReadAllText(fullName);
        }

        private static string GetUserTableCreationString()
        {
            return GetSqlFromFile("UserTableCreation.sql");
        }

        private static string GetTaskTableCreationString()
        {
            return GetSqlFromFile("TaskTableCreation.sql");
        }

        internal class IntermediateUser : User
        {

            new public int Id { get; set; }
            new public string Name { get; set; }

            public IntermediateUser() : base("")
            {
            }

        }

        private static void PopulateUserTable(SqliteConnection connection)
        {
            var fileName = Path.Join(cwdPath, "sqlStuff", "User.csv");

            var users = GetCSVData<IntermediateUser>(fileName);
            var sql =
                "INSERT INTO User ('Id', Name) "
                + "Values(@Id, @Name);";
            var commands = new List<SqliteCommand>();
            foreach (var user in users)
            {
                var cmd = new SqliteCommand(sql);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqliteParameter("Id", user.Id));
                cmd.Parameters.Add(new SqliteParameter("Name", user.Name));
                commands.Add(cmd);
            }

            RunCommands(connection, commands);

            //using (connection)
            //{
            //    connection.Open();
            //    using (var transaction = connection.BeginTransaction())
            //    {
            //        try
            //        {
            //            foreach (var user in users)
            //            {
            //                SqliteParameter id = new SqliteParameter("Id", user.Id);
            //                SqliteParameter name = new SqliteParameter("Name", user.Name);
            //                using (var command = connection.CreateCommand())
            //                {

            //                    command.Parameters.Add(id);
            //                    command.Parameters.Add(name);
            //                    command.Transaction = transaction;
            //                    command.CommandText = sql;
            //                    command.CommandType = CommandType.Text;
            //                    command.ExecuteNonQuery();
            //                }
            //            }
            //            transaction.Commit();
            //        }
            //        catch (Exception e)
            //        {
            //            transaction.Rollback();
            //            throw e;
            //        }
            //    }
            //}
        }

        internal class IntermediateTask : Task
        {
             new public int Id { get; set; }

            IntermediateTask() : base("", "", new IntermediateUser(), new IntermediateUser())
            {

            }
        }
        private static void PopulateTaskTable(SqliteConnection connection)
        {
            var fileName = Path.Join(cwdPath, "Task.csv");
            var tasks = GetCSVData<IntermediateTask>(fileName);
            var sql = "INSERT INTO Task (Id, Title, Description, AssignedToUserId, " +
                            "SourceUserId, DateCreated, DateAssigned, DateCompleted, Notes) " +
                        "VALUES(@Id, @Title, @Description, @AssignedToUserId, " +
                            "@SourceUserId, @DateCreated, @DateAssigned, @DateCompleted, @Notes);";
            using(connection)
            {
                using (var transaction = connection.BeginTransaction())
                {
                    foreach(var task in tasks)
                    {
                        using (var command = connection.CreateCommand())
                        {
                            command.Parameters.Add(new SqliteParameter("Id", task.Id));
                            command.Parameters.Add(new SqliteParameter("Title", task.Title));
                            command.Parameters.Add(new SqliteParameter("Description", task.Description));
                            command.Parameters.Add(new SqliteParameter("AssignedToUserId", task.AssignedTo.Id));
                            command.Parameters.Add(new SqliteParameter("SourceUserId", task.Source.Id));
                            command.Parameters.Add(new SqliteParameter("DateCreated", task.DateCreated));
                            command.Parameters.Add(new SqliteParameter("DateAssigned", task.DateAssigned));
                            command.Parameters.Add(new SqliteParameter("DateCompleted", task.DateCompleted));
                            command.Parameters.Add(new SqliteParameter("Notes", task.Notes));
                            command.Transaction = transaction;
                            command.CommandText = sql;
                            command.CommandType = CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private static void RunCommand(SqliteConnection connection, SqliteCommand command)
        {
            RunCommands(connection, new List<SqliteCommand>() { command });
        }

        private static void RunCommands(SqliteConnection connection, IEnumerable<SqliteCommand> commands)
        {
            using(connection)
            {
                connection.Open();
                using(var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var command in commands)
                        {
                            using (command)
                            {
                                command.Connection = connection;
                                command.Transaction = transaction;
                                command.CommandType = CommandType.Text;
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                }
            }
        }


        private static IEnumerable<T> GetCSVData<T>(string fullFileName)
        {
            using (var reader = new StreamReader(fullFileName))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Parser.Configuration.Delimiter = "|";
                    var records = csv.GetRecords<T>().ToList();
                    return records;
                }
            }
        }
    }
}
