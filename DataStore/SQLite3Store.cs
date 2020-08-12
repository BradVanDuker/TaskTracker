using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using DataStore.misc;
using DataStore.SQLiteDataManagers;
using Models;

namespace TaskTracker.DataStore
{
    public class SQLite3Store 
    {
        /* It appears Sqlite3 library doesn't integrate fully into ADO.NET */
        //private static readonly string dbName = "TaskTracker.db";
        //private static readonly string cwdPath = Directory.GetCurrentDirectory();
        private readonly SqliteConnection connection;

        private readonly UserManager userManager;
        private readonly TaskManager taskManager;

        public SQLite3Store()
        {
            this.connection = DbUtilityHelper.GetConnection();
            this.userManager = new UserManager();
            this.taskManager = new TaskManager(userManager);
        }

        public SQLite3Store(SqliteConnection connection)
        {
            this.connection = connection;
            this.userManager = new UserManager();
            this.taskManager = new TaskManager(userManager);
        }

        

        
        #region User
        public IEnumerable<User> GetAllUsers()
        {
            return userManager.GetAll();
        }

        public User GetUser(int id)
        {
            //return this.GetAllUsers().First(x => x.Id == id);
            return userManager.Get(id);
        }

        public User GetUser(string name)
        {
            //return this.GetAllUsers().First(x => x.Name == name);
            return userManager.Get(name);
        }

        public User AddUser(User user)
        {
            int newId = userManager.Insert(user);
            return new User(user.Name, newId);
        }

        public void DeleteUser(User user)
        {
            userManager.Delete(user);

            //throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            userManager.Update(user);
        }

        #endregion



        #region Tasks

        public Task AddTask(Task task)
        {
            var newId = taskManager.Insert(task);
            var newTask = new Task(
                title: task.Title,
                description: task.Description,
                assignedTo: task.AssignedTo,
                source: task.Source,
                dateCreated: task.DateCreated,
                id: newId);
            newTask.Notes = task.Notes;
            newTask.DateCompleted = task.DateCompleted;
            newTask.RelatedTasks = task.RelatedTasks;
            newTask.DateAssigned = task.DateAssigned;

            return newTask;
        }

        public IEnumerable<Task> GetAllTasks()
        {
            return taskManager.GetAll();
        }
        public Task GetTask(int id)
        {
            throw new NotImplementedException();
        }

        public void SaveTask(Task task)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(Task task)
        {
            throw new NotImplementedException();
        }

        public void DeleteTask(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateTask(Task task)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}