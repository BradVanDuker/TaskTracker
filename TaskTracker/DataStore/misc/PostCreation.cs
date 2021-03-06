﻿using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using CsvHelper;
using System.Globalization;
using System.Linq;
using DataStore.SQLiteDataManagers;
using Models;

namespace DataStore.misc
{

    // Create a simple Task csv file and use that to test csvhelper -- did nothing
    // Post your bug on csvhelper or stack overflow
    // Go back to simple store and work on something else
    // Do another project.
    abstract class PostCreation : DbUtilityHelper
    {
        static UserManager userManager;
        static TaskManager taskManager;
        public static void InitializeDb()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    userManager = new UserManager();
                    taskManager = new TaskManager(userManager);
                    connection.Open();


                    var tableNames = GetTableNames(connection);

                    if (!tableNames.Contains("Task"))
                    {
                        ExecuteNonQuery(GetTaskTableCreationString(), connection);
                        Console.WriteLine("Task table created");
                    }
                    if (GetRowCount(connection, "Task") == 0)
                    {
                        PopulateTaskTable(connection); // it crashes, but it saves the data.
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        #region Common Functions
        private static void RunCommand(SqliteConnection connection, SqliteCommand command)
        {
            RunCommands(connection, new List<SqliteCommand>() { command });
        }

        private static void RunCommands(SqliteConnection connection, IEnumerable<SqliteCommand> commands)
        {
            using (connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
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
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw e;
                    }
                    transaction.Commit();
                }
            }
        }


        private static IEnumerable<T> GetCSVData<T>(string fullFileName)
        {
            PrintMembers<T>();
            using (var reader = new StreamReader(fullFileName))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.IncludePrivateMembers = false;

                    csv.Parser.Configuration.IgnoreBlankLines = true;
                    csv.Parser.Configuration.Delimiter = "|";

                    var records = csv.GetRecords<T>().ToList();
                    return records;
                }
            }
        }

        private static void PrintMembers<T>()
        {
            Console.WriteLine();
            var t = typeof(T);

            Console.WriteLine("Properties...");
            var props = t.GetProperties().ToList();
            if (props.Count == 0)
            {
                Console.WriteLine("\t[None]");
            }
            else
            {
                foreach (var p in props)
                {
                    Console.WriteLine("\t" + p.Name);
                }
            }

            Console.WriteLine("Fields...");
            var fields = t.GetFields().ToList();
            if (fields.Count == 0)
            {
                Console.WriteLine("\t[None]");
            }
            else
            {
                foreach (var f in fields)
                {
                    Console.WriteLine($"\t{f.Name}");
                }
            }
            Console.WriteLine();
        }

        private static void PrintFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }


        private static string GetSqlFromFile(string fileName)
        {
            var fullName = Path.Join(cwdPath, sqlFolder, fileName);
            return File.ReadAllText(fullName);
        }
        #endregion



        #region User Stuff
        private static string GetUserTableCreationString()
        {
            return GetSqlFromFile("UserTableCreation.sql");
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
        }
        #endregion


        #region Task Stuff

        private static string GetTaskTableCreationString()
        {
            return GetSqlFromFile("TaskTableCreation.sql");
        }

        
        private static void PopulateTaskTable(SqliteConnection connection)
        {
            var fileName = Path.Join(cwdPath, "sqlStuff", "Task.csv");
            var intTask = GetCSVData<IntermediateTask>(fileName);
            foreach(var it in intTask)
            {
                var task = it.ToTask(userManager);
                taskManager.Insert(task);
            }
        }
        #endregion
    }
}
