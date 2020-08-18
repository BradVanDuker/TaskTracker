using Models;
using System;
using System.Collections.Generic;
using System.Text;
using TaskTracker;

namespace TaskTracker
{
    public delegate void QuitHandler();
    public interface IController
    {
        IEnumerable<Task> GetTasks();

        IEnumerable<User> GetUsers();

        public void AddTask(Task task);

        public void DeleteTask(int id);

        public void UpdateTask(int id, string propertyName, object newValue);
    }
}
