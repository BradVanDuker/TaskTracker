using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data;
using TaskTracker;
using DataStore.misc;
using Dapper;

namespace DataStore.DataManagers
{
    public class TaskManager : DataManager<Task>
    {
        private readonly SqliteConnection connection;
        private readonly UserManager userManager;


        public TaskManager(UserManager userManager)
        {
            this.connection = DbUtilityHelper.GetConnection();
            this.userManager = userManager;
        }
        

        private IEnumerable<Task> CreatTaskFromReader(SqliteDataReader reader)
        {
            var sql = "SELECT * FROM Task;";
            using (connection)
            {
                var tasks = connection.Query<Task>(sql);
                return tasks;
            }
            
        }

        override public IEnumerable<Task> GetAll()
        {
            var sql = "SELECT * FROM Task;";
            
            using (var connection = GetConnection())
            {
                try
                {
                    connection.Open();
                    var tasks = connection.Query<Task>(sql);
                    return tasks;
                    
                }
                catch (Exception e)
                {
                    Console.WriteLine("!!!" + DbUtilityHelper.GetConnectionInfo(connection));
                    
                    throw e;
                }
            }
            
        }

        override public int Insert(Task task)
        {
            var sql = "INSERT INTO Task (Title, Description, AssignedToUserId, " +
                            "SourceUserId, DateCreated, DateAssigned, DateCompleted, Notes)" +
                        "VALUES(@title, @description, @assignedToUserId, @sourceUserId, " +
                            "@dateCreated, @dateAssigned, @dateCompleted, @notes);";
            int rowId = -1;
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
                rowId = InsertAndGetId(sql, args);
            }
            return rowId;
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
