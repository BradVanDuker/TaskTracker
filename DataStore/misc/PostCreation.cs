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
using DataStore.DataManagers;

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
                var task = it.toTask();
                taskManager.Insert(task);
            }
        }
        
        

        public class IntermediateTask
        {
            private int _Id;
            public int Id
            {
                get { return _Id; }
                set { this._Id = value; }
            }
            public string Title { get; set; }
            public string Description { get; set; }
            public int AssignedToUserId{ get; set; }
            public int SourceUserId { get; set; }
            public string DateCreated { get; set; }
#nullable enable

            private string? _DateAssigned;
            public string? DateAssigned
            { 
                get { return _DateAssigned; }
                set { _DateAssigned = ConvertBlanksToNull(value); }
            }

            private string? _DateCompleted;
            public string? DateCompleted
            {
                get { return _DateCompleted; }
                set { _DateCompleted = ConvertBlanksToNull(value); }
            }

            private string? _Notes;
            public string? Notes
            {
                get { return _Notes; }
                set { _Notes = ConvertBlanksToNull(value); }
            }

            private string? ConvertBlanksToNull(string? value)
            {
                return value != "" ? value : null;
            }
#nullable disable

            public List<object> ToList()
            {
                var myProps = this.GetType().GetProperties();
                var values = new List<object>();
                foreach(var prop in myProps)
                {
                    values.Add(prop.GetValue(this));
                }
                return values;
            }

            public Task toTask()
            {

                DateTime? processNullableDate(string? stringRep)
                {
                    if(stringRep == null || stringRep == "")
                    {
                        return null;
                    }
                    else
                    {
                        return DateTime.Parse(stringRep);
                    }
                }
                var dateCreated =  processNullableDate(this.DateCreated);
                var dateAssigned = processNullableDate(this.DateAssigned);
                var dateCompleted = processNullableDate(this.DateCompleted);

                var task = new Task(
                    title: this.Title,
                    description: this.Description,
                    assignedTo: userManager.Get(this.AssignedToUserId),
                    source: userManager.Get(this.SourceUserId),
                    id: this.Id,
                    dateCreated: dateCreated,
                    dateAssigned: dateAssigned,
                    dateCompleted: dateCompleted
                    );
                return task;
            }
        }

        #endregion
        
    }
}
