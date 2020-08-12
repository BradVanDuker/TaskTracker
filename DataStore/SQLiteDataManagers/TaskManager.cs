using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using TaskTracker;
using DataStore.misc;
using Dapper;
using DataStore.SQLiteDataManagers;
using System.Linq;
using Models;

namespace DataStore.SQLiteDataManagers
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
        

        override public IEnumerable<Task> GetAll()
        {
            var sql = "SELECT * FROM Task;";
            IEnumerable<IntermediateTask> intermediateTasks;
            using (var connection = GetConnection())
            {
                    connection.Open();
                    intermediateTasks = connection.Query<IntermediateTask>(sql);   
            }
            foreach (var it in intermediateTasks)
            {
                yield return it.ToTask(userManager);
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
            using (connection)
            {
                connection.Open();
                
                
            }
                
        }

        public override Task Get(int id)
        {
            var tasks = GetAll();
            var taskList = tasks.ToList();
            return taskList.First(t => t.Id == id);
        }

        override public void Delete(Task task)
        {
            var sql = "DELETE FROM Task WHERE Id == @id;";
            using (connection)
            {
                var command = GetCommand(sql, connection);
                var param = new SqliteParameter("id", task.Id);
                ExecuteNonQuery(sql, new SqliteParameter[] { param });
            }
        }
    }


    
}
