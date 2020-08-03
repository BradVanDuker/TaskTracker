using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data;
using TaskTracker;


namespace DataStore.DataManagers
{
    class TaskManager : DataManager<Task>
    {
        private readonly SqliteConnection connection;
        private readonly UserManager userManager;
        private readonly List<Converter> DbToInstanceConverters;

        public TaskManager(SqliteConnection connection, UserManager userManager)
        {
            this.connection = connection;
            this.userManager = userManager;
            DbToInstanceConverters = new List<Converter>();

            #region DbToInstanceConverters

#nullable enable
            static Nullable<DateTime> _StringToDateTime(string? s)
            {
                if (s != null)
                {
                    return DateTime.Parse(s);
                }
                else
                {
                    return null;
                }

            }
#pragma warning disable CS8603 // Possible null reference return.
            var StringToDateTime = new Func<object, object>(s => _StringToDateTime((string?)s));
#pragma warning restore CS8603 // Possible null reference return.
#nullable restore

            //var PassThrough = new Func<object, object>(x => x);
            object PassThrough(object x)
            {
                return x;
            }

            object Int64ToInt32(object i)
            {
                Console.WriteLine(i.GetType());
                Console.WriteLine(i.ToString());
                int newValue = Convert.ToInt32(i);
                Console.WriteLine(newValue.GetType());
                Console.WriteLine(newValue);

                return Convert.ToInt32(newValue);
                //return Convert.ToInt32((Int64)i);
            }

            //var IdToUser = new Func<object, object>(id => userManager.Get((int)id));
            User IdToUser(object id)
            {
                return userManager.Get((int)id);
            }
                        
            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "Id",
                TargetAttrName = "Id",
                InputProcessor = Int64ToInt32
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "Title",
                TargetAttrName = "Title",
                InputProcessor = PassThrough
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "Description",
                TargetAttrName = "Description",
                InputProcessor = PassThrough
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "AssignedToUserId",
                TargetAttrName = "AssignedTo",
                InputProcessor = IdToUser
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "SourceUserId",
                TargetAttrName = "Source",
                InputProcessor = IdToUser
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "DateCreated",
                TargetAttrName = "DateCreated",
                InputProcessor = StringToDateTime
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "DateAssigned",
                TargetAttrName = "Source",
                InputProcessor = IdToUser
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "DateCompleted",
                TargetAttrName = "DateCompleted",
                InputProcessor = IdToUser
            });

            DbToInstanceConverters.Add(new Converter()
            {
                OriginAttrName = "Notes",
                TargetAttrName = "Notes",
                InputProcessor = PassThrough
            });
            #endregion
        }


        class Converter
        {

            public string OriginAttrName { get; set; }

            public string TargetAttrName { get; set; }

            public Func<object, object> InputProcessor { get; set; }

            private Action<object, object> GetPropertySetter()
            {
                return typeof(Task).GetProperty(TargetAttrName).SetValue;
            }

            public void Do(SqliteDataReader reader, Task taskInstance)
            {
                Console.WriteLine($"origin: {OriginAttrName}, target: {TargetAttrName}");
                var propSetter = GetPropertySetter();
                //var inputValue = GetDbColValue(reader);
                var inputValue = reader.GetValue(OriginAttrName);
                var value = InputProcessor(inputValue);
                propSetter(taskInstance, value);
            }

            private object GetDbColValue(SqliteDataReader reader)
            {
                return reader.GetFieldValue<object>(OriginAttrName);
            }
        }

        private Task CreatTaskFromReader(SqliteDataReader reader)
        {
            throw new NotImplementedException();

            //var task = new Task();
            //foreach(var converter in this.DbToInstanceConverters)
            //{
            //    converter.Do(reader, task);
            //}
            //return task;
        }

        override public IEnumerable<Task> GetAll()
        {
            var sql = "SELECT * FROM Task;";
            using (var connection = GetConnection())
            {
                connection.Open();
                var tasks = new List<Task>();
                using (var command = GetCommand(sql, connection))
                {
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {  
                        var t = CreatTaskFromReader(reader);
                        tasks.Add(t);
                    }
                }
                return tasks;
            }
        }

        override public int Insert(Task task)
        {
            var sql = "INSERT INTO Task (Title, Description, AssignedToUserId, " +
                            "SourceUserId, DateCreated, DateAssigned, DateCompleted, Notes)" +
                        "VALUES(@title, @description, @assignedToUserId, @sourceUserId, " +
                            "@dateCreated, @dateAssigned, @dateCompleted, @notes);";
            using (this.connection)
            {

                connection.Open();

                var args = new SqliteParameter[]
                {
                        new SqliteParameter("@title", task.Title),
                        new SqliteParameter("@description", task.Description),
                        new SqliteParameter("@dateCreated", task.DateCreated),
                        new SqliteParameter("@dateAssigned", task.DateAssigned?.ToString() ?? ""),
                        new SqliteParameter("@dateCompleted", task.DateCompleted?.ToString() ?? ""),
                        new SqliteParameter("@notes", task.Notes ?? ""),
                        new SqliteParameter("@assignedToUserId", task.AssignedTo.Id),
                        new SqliteParameter("@sourceUserId", task.Source.Id)
            };
                return InsertAndGetId(sql, args);
            }
        }

        public override void Update(Task thing)
        {
            throw new NotImplementedException();
        }

        public override Task Get(int id)
        {
            throw new NotImplementedException();
        }

        override public void Delete(Task task)
        {
            throw new NotImplementedException();
        }


    }

    
}
